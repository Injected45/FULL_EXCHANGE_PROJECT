using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using MySqlConnector;

// =====================================================================================
// Phase-1 Migrator — SQL Server -> MariaDB/MySQL schema + data for the 134 business tables.
//   migrator schema   generate + apply MariaDB DDL (tables, PKs, identity, indexes, 2 FKs)
//   migrator data     copy all rows (FK checks off, identity values preserved), reseed AUTO_INCREMENT
//   migrator verify   diff row counts per table (source vs target)
//   migrator all      schema then data then verify
// Connection strings from appsettings.json (gitignored). Source = the read-only SNAPSHOT.
// =====================================================================================

var cmd = args.Length > 0 ? args[0].ToLowerInvariant() : "all";
var cfg = JsonSerializer.Deserialize<Cfg>(File.ReadAllText("appsettings.json"),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
Console.OutputEncoding = Encoding.UTF8;

// Derive the emitted "USE <db>;" target from the MySql connection string (keeps the tool database-agnostic).
Target.Db = new MySqlConnectionStringBuilder(cfg.MySql).Database;
Console.WriteLine($"Target MySQL schema: {Target.Db}");

var tables = LoadTables(cfg.SqlServer);
Console.WriteLine($"Business tables: {tables.Count}");

try
{
    switch (cmd)
    {
        case "schema": DoSchema(cfg, tables); break;
        case "data": DoData(cfg, tables); break;
        case "verify": DoVerify(cfg, tables); break;
        case "views": DoViews(cfg); break;
        case "functions": DoFunctions(cfg); break;
        case "procs": DoProcs(cfg); break;
        case "hardprocs": DoHardProcs(cfg); break;
        case "hardverify": DoHardVerify(cfg, tables); break;
        case "tvpprocs": DoTvpProcs(cfg); break;   // table-valued-parameter procs -> tvp_<name> temp-table staging
        case "harvest": DoHarvest(cfg); break;   // harvest <targetsFile> : convert+create each named proc on the (scratch) MySql target, write successes
        case "crossdb": DoCrossDb(cfg); break;   // crossdb <sourceDB> <targetSchema> <table1,table2,...>
        case "debugfn": DoDebugFn(cfg); break;   // debugfn <name> : print the T-SQL and the converted MySQL body
        case "dumpbodies": DoDumpBodies(cfg); break;  // dumpbodies <outDir> : proc bodies from BOTH engines, UTF-8
        case "all": DoSchema(cfg, tables); DoData(cfg, tables); DoVerify(cfg, tables); break;
        default: Console.WriteLine("usage: migrator [schema|data|verify|views|functions|procs|hardprocs|hardverify|crossdb|all]"); return 2;
    }
}
catch (Exception ex) { Console.Error.WriteLine($"ERROR: {ex.Message}"); return 1; }
return 0;

// ---------------- schema ----------------
void DoSchema(Cfg c, List<Table> tbls)
{
    var ddl = new StringBuilder();
    ddl.AppendLine("SET FOREIGN_KEY_CHECKS=0;");
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables

    var unmappedDefaults = new List<string>();
    foreach (var t in tbls)
    {
        var cols = LoadColumns(src, t.ObjectId);
        var pk = LoadPk(src, t.ObjectId);
        // DEFAULT constraints must travel WITH the table. Dropping them is the silent-data-loss bug
        // documented in README_MYSQL §8.2 (Store_Image: rows inserted, IsActive left NULL, grid filtered
        // them out with WHERE IsActive=1 -> user sees no error and no data).
        var defs = LoadDefaults(src, t.ObjectId);
        ddl.AppendLine($"DROP TABLE IF EXISTS `{t.Name}`;");
        ddl.AppendLine($"CREATE TABLE `{t.Name}` (");
        var lines = new List<string>();
        foreach (var col in cols)
        {
            var def = $"  `{col.Name}` {MapType(col)} {(col.IsNullable ? "NULL" : "NOT NULL")}";
            if (col.IsIdentity) def += " AUTO_INCREMENT";
            // AUTO_INCREMENT and DEFAULT are mutually exclusive in MySQL.
            else if (defs.TryGetValue(col.Name, out var rawDefault))
            {
                var my = MapDefault(rawDefault, col.Type);
                if (my != null) def += $" DEFAULT {my}";
                else unmappedDefaults.Add($"{t.Name}.{col.Name}  {col.Type}  {rawDefault}");
            }
            lines.Add(def);
        }
        if (pk.Count > 0)
            lines.Add($"  PRIMARY KEY ({string.Join(",", pk.Select(p => $"`{p}`"))})");
        // AUTO_INCREMENT column must be a key; if identity not in PK, add a key for it.
        foreach (var col in cols.Where(c2 => c2.IsIdentity && !pk.Contains(c2.Name)))
            lines.Add($"  KEY `ak_{col.Name}` (`{col.Name}`)");
        ddl.AppendLine(string.Join(",\n", lines));
        ddl.AppendLine(") ENGINE=InnoDB ROW_FORMAT=DYNAMIC DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
        ddl.AppendLine();
    }

    // secondary indexes (non-PK). Skip indexes whose key would exceed MariaDB's 3072-byte limit
    // (wide SQL Server "covering" indexes) or that index a TEXT/BLOB column — log them for review.
    int idxCount = 0; var skipped = new List<string>();
    foreach (var t in tbls)
    {
        var colMap = LoadColumns(src, t.ObjectId).ToDictionary(x => x.Name, x => x);
        foreach (var ix in LoadIndexes(src, t.ObjectId))
        {
            int bytes = ix.Columns.Sum(cn => colMap.TryGetValue(cn, out var col) ? EstKeyBytes(col) : 9999);
            if (bytes > 3000)
            {
                skipped.Add($"{t.Name}.{ix.Name} (~{bytes} key bytes, cols: {string.Join(",", ix.Columns)})");
                continue;
            }
            var cols = string.Join(",", ix.Columns.Select(x => $"`{x}`"));
            ddl.AppendLine($"CREATE {(ix.IsUnique ? "UNIQUE " : "")}INDEX `{Trunc(ix.Name, 60)}_{t.Name.GetHashCode() & 0xffff}` ON `{t.Name}` ({cols});");
            idxCount++;
        }
    }
    if (unmappedDefaults.Count > 0)
        File.WriteAllText("schema_manual_defaults.txt",
            "DEFAULT constraints that could NOT be translated faithfully and were therefore OMITTED.\n" +
            "Review each one — an omitted DEFAULT is a silent-data-loss risk (README_MYSQL §8.2).\n\n" +
            string.Join("\n", unmappedDefaults), new UTF8Encoding(false));
    Console.WriteLine($"Column DEFAULTs: carried across; {unmappedDefaults.Count} needed manual review.");

    if (skipped.Count > 0)
        File.WriteAllText("schema_skipped_indexes.txt",
            "Indexes skipped (exceed MariaDB 3072-byte key limit or index a TEXT/BLOB column). These are\n" +
            "SQL Server wide 'covering' indexes — performance hints, not correctness. Review if a query needs them\n" +
            "(MySQL alternative: a shorter index or a prefixed index like col(255)).\n\n" + string.Join("\n", skipped),
            new UTF8Encoding(false));

    // the 2 FKs
    foreach (var fk in LoadForeignKeys(src))
        ddl.AppendLine($"ALTER TABLE `{fk.ParentTable}` ADD CONSTRAINT `{fk.Name}` FOREIGN KEY (`{fk.ParentCol}`) REFERENCES `{fk.RefTable}` (`{fk.RefCol}`);");

    ddl.AppendLine("SET FOREIGN_KEY_CHECKS=1;");

    File.WriteAllText("schema_mariadb.sql", ddl.ToString(), new UTF8Encoding(false));
    Console.WriteLine($"Generated schema_mariadb.sql ({tbls.Count} tables, {idxCount} indexes). Applying...");

    using var dst = OpenMy(c.MySql);
    // ensure database charset
    Exec(dst, "ALTER DATABASE `" + dst.Database + "` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;");
    Exec(dst, "SET FOREIGN_KEY_CHECKS=0;");   // session-wide: lets DROP/CREATE ignore FK order
    int applied = 0, failed = 0;
    foreach (var stmt in SplitStatements(ddl.ToString()))
    {
        try { Exec(dst, stmt); applied++; }
        catch (Exception ex) { failed++; Console.WriteLine($"  DDL FAIL: {ex.Message}\n    {Trunc(stmt, 160)}"); }
    }
    Exec(dst, "SET FOREIGN_KEY_CHECKS=1;");
    Console.WriteLine($"Schema applied: {applied} statements ok, {failed} failed.");
}

// ---------------- data ----------------
void DoData(Cfg c, List<Table> tbls)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    long grand = CopyAll(src, dst, tbls, truncateFirst: false, log: true);
    Console.WriteLine($"Data copied: {grand} rows total across {tbls.Count} tables.");
}

// Copy all rows src->dst (optionally TRUNCATE first), with FK/unique checks off and AUTO_INCREMENT reseed.
long CopyAll(SqlConnection src, MySqlConnection dst, List<Table> tbls, bool truncateFirst, bool log)
{
    Exec(dst, "SET FOREIGN_KEY_CHECKS=0;");
    Exec(dst, "SET UNIQUE_CHECKS=0;");
    Exec(dst, "SET NAMES utf8mb4;");
    // README_MYSQL §7 (error 1267): SET NAMES alone leaves the CONNECTION collation at utf8mb4_general_ci
    // while every column is utf8mb4_unicode_ci. MySQL BAKES the session collation into a routine at CREATE
    // time, so without this the routine hits "Illegal mix of collations" at runtime on any UNION or string
    // comparison. Must be set on the CREATING connection, not just in the emitted .sql header.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");
    if (truncateFirst) foreach (var t in tbls) Exec(dst, $"TRUNCATE `{t.Name}`");

    long grand = 0;
    foreach (var t in tbls)
    {
        var cols = LoadColumns(src, t.ObjectId);
        var colList = string.Join(",", cols.Select(x => $"`{x.Name}`"));
        var paramList = string.Join(",", cols.Select((_, i) => $"@p{i}"));
        var insertSql = $"INSERT INTO `{t.Name}` ({colList}) VALUES ({paramList})";
        var selectSql = $"SELECT {string.Join(",", cols.Select(x => $"[{x.Name}]"))} FROM [{t.Schema}].[{t.Name}]";
        using var read = new SqlCommand(selectSql, src);
        using var tx = dst.BeginTransaction();
        using var ins = new MySqlCommand(insertSql, dst, tx);
        for (int i = 0; i < cols.Count; i++) ins.Parameters.Add(new MySqlParameter($"@p{i}", null));
        long n = 0;
        using (var rdr = read.ExecuteReader())
            while (rdr.Read())
            {
                for (int i = 0; i < cols.Count; i++) { var v = rdr.GetValue(i); ins.Parameters[i].Value = v is DBNull ? null : v; }
                ins.ExecuteNonQuery(); n++;
            }
        tx.Commit();
        grand += n;
        if (log && n > 0) Console.WriteLine($"  {t.Name}: {n}");
    }
    foreach (var t in tbls)
    {
        var idCol = LoadColumns(src, t.ObjectId).FirstOrDefault(x => x.IsIdentity);
        if (idCol is null) continue;
        var max = Scalar(dst, $"SELECT IFNULL(MAX(`{idCol.Name}`),0)+1 FROM `{t.Name}`");
        Exec(dst, $"ALTER TABLE `{t.Name}` AUTO_INCREMENT={max}");
    }
    Exec(dst, "SET FOREIGN_KEY_CHECKS=1;");
    Exec(dst, "SET UNIQUE_CHECKS=1;");
    return grand;
}

// ---------------- cross-DB co-location: migrate selected tables of an external DB into a MySQL schema ----------------
void DoCrossDb(Cfg c)
{
    var srcDb = args.Length > 1 ? args[1] : throw new Exception("usage: crossdb <sourceDB> <targetSchema> [t1,t2,...]");
    var schema = args.Length > 2 ? args[2] : srcDb;
    var only = args.Length > 3 ? args[3].Split(',').Select(s => s.Trim()).ToHashSet(StringComparer.OrdinalIgnoreCase) : null;

    var srcCs = System.Text.RegularExpressions.Regex.Replace(c.SqlServer, @"(?i)Database=[^;]*", "Database=" + srcDb);
    using var src = Open(srcCs);
    using var dst0 = OpenMy(c.MySql);
    Exec(dst0, $"CREATE DATABASE IF NOT EXISTS `{schema}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;");
    var dstCs = System.Text.RegularExpressions.Regex.Replace(c.MySql, @"(?i)Database=[^;]*", "Database=" + schema);
    using var dst = OpenMy(dstCs);
    Exec(dst, "SET FOREIGN_KEY_CHECKS=0;");

    // load requested tables from the source DB (any schema)
    var tbls = new List<Table>();
    using (var cmd2 = new SqlCommand(@"SELECT s.name, t.name, t.object_id FROM sys.tables t
        JOIN sys.schemas s ON t.schema_id=s.schema_id ORDER BY t.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) { var nm = r.GetString(1); if (only == null || only.Contains(nm)) tbls.Add(new Table { Schema = r.GetString(0), Name = nm, ObjectId = r.GetInt32(2) }); }

    foreach (var t in tbls)
    {
        var cols = LoadColumns(src, t.ObjectId); var pk = LoadPk(src, t.ObjectId);
        var xdefs = LoadDefaults(src, t.ObjectId);   // carry DEFAULTs here too (README §8.2)
        var lines = cols.Select(col =>
        {
            var d = $"  `{col.Name}` {MapType(col)} {(col.IsNullable ? "NULL" : "NOT NULL")}";
            if (col.IsIdentity) return d + " AUTO_INCREMENT";
            if (xdefs.TryGetValue(col.Name, out var raw) && MapDefault(raw, col.Type) is string my) d += $" DEFAULT {my}";
            return d;
        }).ToList();
        if (pk.Count > 0) lines.Add($"  PRIMARY KEY ({string.Join(",", pk.Select(p => $"`{p}`"))})");
        else foreach (var col in cols.Where(x => x.IsIdentity)) lines.Add($"  KEY `ak_{col.Name}` (`{col.Name}`)");
        Exec(dst, $"DROP TABLE IF EXISTS `{t.Name}`");
        Exec(dst, $"CREATE TABLE `{t.Name}` (\n{string.Join(",\n", lines)}\n) ENGINE=InnoDB ROW_FORMAT=DYNAMIC DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }
    long n = CopyAll(src, dst, tbls, truncateFirst: true, log: true);

    // Secondary indexes — the main `schema` command copies these, but crossdb originally did not, so the
    // co-located ledger (ExSyAccounts2026.AccSafeActivityTb, 506k rows) arrived with ONLY its primary key.
    // Every proc that scans it then does a full table scan: AgentAccountStatement went from working to
    // "Command Timeout expired". Correctness was fine; the migration just became unusably slow.
    int idx = 0;
    foreach (var t in tbls)
    {
        var colMap = LoadColumns(src, t.ObjectId).ToDictionary(x => x.Name, x => x);
        foreach (var ix in LoadIndexes(src, t.ObjectId))
        {
            int bytes = ix.Columns.Sum(cn => colMap.TryGetValue(cn, out var col) ? EstKeyBytes(col) : 9999);
            if (bytes > 3000) { Console.WriteLine($"  skip wide index {t.Name}.{ix.Name} (~{bytes} key bytes)"); continue; }
            var icols = string.Join(",", ix.Columns.Select(x => $"`{x}`"));
            try
            {
                Exec(dst, $"CREATE {(ix.IsUnique ? "UNIQUE " : "")}INDEX `{Trunc(ix.Name, 60)}_{t.Name.GetHashCode() & 0xffff}` ON `{t.Name}` ({icols});");
                idx++;
            }
            catch (Exception ex) { Console.WriteLine($"  index FAIL {t.Name}.{ix.Name}: {ex.Message}"); }
        }
    }
    Console.WriteLine($"crossdb {srcDb} -> schema `{schema}`: {tbls.Count} tables, {n} rows, {idx} indexes.");
}

// ---------------- Phase-3 behavioral verification (safe: resets the disposable MariaDB between procs) ----------------
void DoHardVerify(Cfg c, List<Table> tbls)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");

    // the transactional procs that were converted+created
    var names = new List<string>();
    using (var cmd2 = new SqlCommand(@"SELECT o.name FROM sys.objects o JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.type='P' AND o.name NOT LIKE 'Private_%' AND m.definition NOT LIKE '%tsqlt%'
          AND (LEN(m.definition)>5000 OR m.definition LIKE '%TRAN%' OR m.definition LIKE '%TRY%')
          AND m.definition NOT LIKE '%CURSOR%' AND m.definition NOT LIKE '%sp_executesql%'
          AND m.definition NOT LIKE '%EXEC(%' AND m.definition NOT LIKE '%READONLY%' ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader()) while (r.Read()) names.Add(r.GetString(0));

    int pass = 0, tested = 0; var fail = new List<string>(); var missing = 0;
    foreach (var name in names)
    {
        // only verify procs that actually got created on MariaDB
        if (Convert.ToInt64(Scalar(dst, $"SELECT COUNT(*) FROM information_schema.routines WHERE routine_schema=DATABASE() AND routine_name='{name}'")) == 0) { missing++; continue; }
        var ps = LoadProcParams(src, name);
        if (ps.Any(p => MapType(p.Col).Contains("BLOB"))) continue;
        CopyAll(src, dst, tbls, truncateFirst: true, log: false);   // reset MariaDB to clean, comparable state
        tested++;
        try
        {
            var (ra, ra_out) = CallProcSql(src, name, ps);
            var (rb, rb_out) = CallProcMyNoWrap(dst, name, ps);     // proc self-commits; MariaDB reset next round
            if (RowsEqual(ra, rb) && OutEqual(ra_out, rb_out)) pass++;
            else fail.Add($"{name}: rows({ra.Count}/{rb.Count}) or OUT differ");
        }
        catch (Exception ex) { fail.Add($"{name}: {OneLine(ex.Message)}"); }
    }
    CopyAll(src, dst, tbls, truncateFirst: true, log: false);       // leave MariaDB clean
    File.WriteAllText("hardprocs_verify.txt", $"PHASE 3 behavioral verification\ntested {tested}, PASS {pass}, FAIL {fail.Count}, not-created {missing}\n\n" + string.Join("\n", fail), new UTF8Encoding(false));
    Console.WriteLine($"Phase-3 verify: tested {tested} | PASS {pass} | FAIL {fail.Count} | not-created {missing} -> hardprocs_verify.txt");
}

(List<string>, Dictionary<string, string>) CallProcMyNoWrap(MySqlConnection c, string name, List<Prm> ps)
{
    using var cmd2 = new MySqlCommand(name, c) { CommandType = CommandType.StoredProcedure };
    foreach (var p in ps)
        cmd2.Parameters.Add(new MySqlParameter("@p_" + p.Col.Name, SampleValue(p.Col)) { Direction = p.IsOut ? ParameterDirection.InputOutput : ParameterDirection.Input });
    var rows = ReadReaderRows(cmd2);
    var outv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    foreach (MySqlParameter p in cmd2.Parameters) if (p.Direction != ParameterDirection.Input) { var k = p.ParameterName.TrimStart('@'); if (k.StartsWith("p_")) k = k.Substring(2); outv[k] = NormScalar(p.Value); }
    return (rows, outv);
}

// ---------------- verify ----------------
void DoVerify(Cfg c, List<Table> tbls)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    int ok = 0, bad = 0;
    foreach (var t in tbls)
    {
        long s = Convert.ToInt64(Scalar(src, $"SELECT COUNT(*) FROM [{t.Schema}].[{t.Name}]"));
        long d;
        try { d = Convert.ToInt64(Scalar(dst, $"SELECT COUNT(*) FROM `{t.Name}`")); }
        catch { d = -1; }
        if (s == d) ok++;
        else { bad++; Console.WriteLine($"  MISMATCH {t.Name}: source={s} target={d}"); }
    }
    Console.WriteLine($"\nVerify: {ok}/{tbls.Count} tables match row counts ({bad} mismatched).");
}

// ---------------- views (Phase 2, mechanical) ----------------
void DoViews(Cfg c)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");   // allow "sum (x)" spacing
    Exec(dst, "SET NAMES utf8mb4;");
    // Views are stored WITH the creating connection's collation. Without this a view with a UNION or string
    // comparison hits "Illegal mix of collations (general_ci vs unicode_ci)" — the same 1267 that bit the
    // routines. README §7.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");

    // load view defs (exclude Private_ system views and tSQLt unit-test framework objects)
    var views = new List<(string Name, string Def)>();
    using (var cmd2 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.views o
        JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.name NOT LIKE 'Private_%'
          AND m.definition NOT LIKE '%tsqlt%' AND m.definition NOT LIKE '%extended_properties%'
        ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) views.Add((r.GetString(0), r.GetString(1)));

    // CREATE pass with retries so view-on-view dependencies resolve (converge topologically).
    var pending = views.ToList(); var createOk = new List<string>(); var createErr = new Dictionary<string, string>();
    for (int passNo = 0; passNo < 4 && pending.Count > 0; passNo++)
    {
        var still = new List<(string, string)>();
        foreach (var (name, def) in pending)
        {
            try { Exec(dst, $"DROP VIEW IF EXISTS `{name.Trim()}`"); Exec(dst, ConvertViewDdl(name, def)); createOk.Add(name); }
            catch (Exception ex) { createErr[name] = OneLine(ex.Message); still.Add((name, def)); }
        }
        if (still.Count == pending.Count) break;   // no progress -> stop
        pending = still;
    }
    int created = createOk.Count, pass = 0;
    var createFail = pending.Select(p => $"{p.Item1}: {createErr.GetValueOrDefault(p.Item1, "?")}").ToList();
    var diffFail = new List<string>();

    var defByName = views.ToDictionary(v => v.Name, v => v.Def); var passed = new List<string>();
    foreach (var name in createOk)
    {
        try
        {
            var a = ReadAllRows(src, $"SELECT * FROM [dbo].[{name}]");
            var b = ReadAllRows(dst, $"SELECT * FROM `{name}`");
            if (RowsEqual(a, b)) { pass++; passed.Add(name); }
            else diffFail.Add($"{name}: rows/values differ (src={a.Count}, dst={b.Count})");
        }
        catch (Exception ex) { diffFail.Add($"{name}: query error {OneLine(ex.Message)}"); }
    }
    WriteVerified("converted_verified_views.sql", "VIEW", passed.Select(n => ConvertViewDdl(n, defByName[n])));

    File.WriteAllText("views_needs_manual.txt",
        "VIEWS THAT NEED MANUAL REVIEW (typically T-SQL '+' string concat -> MySQL CONCAT, or date-fn signatures)\n\n" +
        "== failed to CREATE ==\n" + string.Join("\n", createFail) +
        "\n\n== created but diff FAILED ==\n" + string.Join("\n", diffFail), new UTF8Encoding(false));

    Console.WriteLine($"Views: {views.Count} total | created {created} | diff PASS {pass} | needs manual {createFail.Count + diffFail.Count}");
    Console.WriteLine($"  (create-fail {createFail.Count}, diff-fail {diffFail.Count}) -> views_needs_manual.txt");
}

static string ConvertViewDdl(string name, string def)
{
    // Two of these T-SQL view names carry a TRAILING SPACE ("InternalEx_SelectType_View_not_BRanchId ").
    // SQL Server tolerates it; MySQL rejects a trailing-space identifier (error 1103 Incorrect table name).
    // Trim it. (Same class as the shipping migration's trailing-space routine name.)
    name = name.Trim();
    var m = System.Text.RegularExpressions.Regex.Match(def, @"CREATE\s+VIEW\s+.+?\s+AS\s+(.*)",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
    string body = m.Success ? m.Groups[1].Value : def;
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\[?dbo\]?\.", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);  // dbo./DBO./[dbo].
    body = StripTypeBrackets(body);   // [VARCHAR](200) -> VARCHAR(200)  BEFORE [x] -> `x`
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\[([^\]]+)\]", "`$1`");
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    // Strip comments. ConvertViewDdl did not, and MySQL's line comment REQUIRES a space after "--" — the
    // legacy "--,c.id" is NOT a comment to MySQL, it parses as "minus minus , c.id" and breaks the SELECT list.
    body = StripSqlComments(body);   // string-aware: never eats a literal containing '--'
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bISNULL\s*\(", "IFNULL(", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bGETDATE\s*\(\s*\)", "NOW()", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bLEN\s*\(", "CHAR_LENGTH(", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bCHARINDEX\s*\(", "LOCATE(", IC);
    // drop explicit SQL Server collations (e.g. COLLATE Arabic_CI_AS) — DB default utf8mb4_unicode_ci applies
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bCOLLATE\s+\w+", "", IC);
    // A view body is a SELECT EXPRESSION, so the EXPRESSION-level converters apply just like in a proc body.
    // ConvertViewDdl previously did none of these, so views with CONVERT / DATEADD / FORMAT / a '+' feeding a
    // LIKE all failed to create even though the identical expression in a proc converted fine.
    // T-SQL "SELECT <alias> = <expr>" puts the output-column ALIAS on the LEFT. MySQL has no such form — it
    // reads "alias = expr" as a boolean comparison, yields an unnamed column, and any outer reference to that
    // alias fails ("Unknown column 'CaseStauts'"). Rewrite the CASE-valued form to "expr AS alias". Scoped to
    // "<ident> = CASE .. END" (masked first so the CASE's own commas/parens don't confuse the match), which is
    // the shape these views use; a plain "col = val" in a WHERE is never a bare "ident = CASE" at list level.
    {
        var vcases = new List<string>();
        var masked = MaskCaseExpressions(body, vcases);
        masked = System.Text.RegularExpressions.Regex.Replace(masked,
            @"(?<=[,\n]\s*)([A-Za-z_]\w*)\s*=\s*(__CASE\d+__)", "$2 AS $1", IC);
        // also the first item right after SELECT
        masked = System.Text.RegularExpressions.Regex.Replace(masked,
            @"(?<=\bSELECT\s)(\s*)([A-Za-z_]\w*)\s*=\s*(__CASE\d+__)", "$1$3 AS $2", IC);
        body = UnmaskCaseExpressions(masked, vcases);
    }
    body = ConvertSelectTop(body);      // SELECT TOP n -> LIMIT n ; "TOP 100 PERCENT" dropped
    body = ConvertDateFuncs(body);      // DATEADD/DATEDIFF/DATEPART
    body = ConvertTsqlConvert(body);    // CONVERT(t,x) -> CAST(x AS t)   (also the unary "+ convert(..)")
    body = FixTsqlFormat(body);         // FORMAT(x,'N3') -> FORMAT(x,3) / DATE_FORMAT
    body = FixStringConcat(body, CollectStringVars(body));  // '+' concat -> CONCAT, incl. the LIKE case
    return $"CREATE OR REPLACE VIEW `{name}` AS {body}";
}

List<string> ReadAllRows(IDbConnection c, string sql)
{
    using var cmd2 = c.CreateCommand(); cmd2.CommandText = sql;
    using var r = cmd2.ExecuteReader();
    var rows = new List<string>();
    while (r.Read())
    {
        var sb = new StringBuilder();
        for (int i = 0; i < r.FieldCount; i++)
        {
            var v = r.GetValue(i);
            sb.Append(v is DBNull or null ? "NULL" : NormVal(v)).Append('|');
        }
        rows.Add(sb.ToString());
    }
    rows.Sort(StringComparer.Ordinal);
    return rows;
}

static string NormVal(object v) => v switch
{
    bool b => b ? "1" : "0",
    byte tb => tb.ToString(),
    DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
    decimal d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
    double db => db.ToString("R", System.Globalization.CultureInfo.InvariantCulture),
    byte[] by => Convert.ToHexString(by),
    _ => v.ToString()?.TrimEnd() ?? ""
};

static bool RowsEqual(List<string> a, List<string> b)
{
    if (a.Count != b.Count) return false;
    for (int i = 0; i < a.Count; i++) if (a[i] != b[i]) return false;
    return true;
}

static string OneLine(string s) => s.Replace("\r", " ").Replace("\n", " ");

// Write the diff-VERIFIED converted routines to a runnable .sql artifact (DELIMITER-wrapped for routines).
static void WriteVerified(string file, string kind, IEnumerable<string> ddls)
{
    var list = ddls.ToList();
    bool pending = file.Contains("PENDING");
    var sb = new StringBuilder();
    if (pending)
    {
        sb.AppendLine($"-- {list.Count} {kind}(s) — SQL Server -> MariaDB/MySQL. SYNTACTICALLY CONVERTED + CREATED, but");
        sb.AppendLine("-- NOT behaviorally diff-verified (these write data; verify with curated inputs before trusting).");
    }
    else
    {
        sb.AppendLine($"-- {list.Count} diff-VERIFIED {kind}(s) — SQL Server -> MariaDB/MySQL. Generated by `migrator`.");
        sb.AppendLine("-- Only objects whose result matched the SQL Server original exactly are included.");
    }
    sb.AppendLine($"USE {Target.Db};");
    sb.AppendLine("SET NAMES utf8mb4;");
    // Without this the connection collates as utf8mb4_general_ci while the columns are utf8mb4_unicode_ci,
    // producing "Illegal mix of collations" (1267) on UNION-heavy / string-comparing routines. README §7.
    sb.AppendLine("SET collation_connection='utf8mb4_unicode_ci';");
    sb.AppendLine("SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';   -- allows 'func (x)' spacing in legacy T-SQL");
    sb.AppendLine();
    bool routine = kind is "PROCEDURE" or "FUNCTION";
    foreach (var d in list)
    {
        if (routine)
        {
            var nm = System.Text.RegularExpressions.Regex.Match(d, @"CREATE\s+(?:PROCEDURE|FUNCTION)\s+`(\w+)`", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups[1].Value;
            sb.AppendLine($"DROP {kind} IF EXISTS `{nm}`;");
            sb.AppendLine("DELIMITER //"); sb.AppendLine(d.TrimEnd().TrimEnd(';') + " //"); sb.AppendLine("DELIMITER ;");
        }
        else sb.AppendLine(d.TrimEnd().TrimEnd(';') + ";");   // views use CREATE OR REPLACE (already idempotent)
        sb.AppendLine();
    }
    File.WriteAllText(file, sb.ToString(), new UTF8Encoding(false));
    Console.WriteLine($"  wrote {list.Count} {(pending ? "converted (pending verify)" : "diff-verified")} {kind}(s) -> {file}");
}

// ---------------- scalar functions (Phase 2, mechanical) ----------------
// Print the converted MySQL body of one routine (function or proc) so a conversion bug can be seen directly
// instead of inferred from a truncated MySQL syntax error. usage: migrator debugfn <name>
void DoDebugFn(Cfg c)
{
    var name = args.Length > 1 ? args[1] : throw new Exception("usage: debugfn <routineName>");
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    string? def = null; string type = "";
    using (var cmd2 = new SqlCommand(@"SELECT m.definition, o.type FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id WHERE o.name=@n", src))
    {
        cmd2.Parameters.AddWithValue("@n", name);
        using var r = cmd2.ExecuteReader();
        if (r.Read()) { def = r.GetString(0); type = r.GetString(1).Trim(); }
    }
    if (def == null) { Console.WriteLine($"routine '{name}' not found"); return; }

    Console.WriteLine("======== T-SQL SOURCE ========");
    Console.WriteLine(def);
    Console.WriteLine("======== CONVERTED MySQL ========");
    if (type == "FN")
    {
        var ps = LoadFnParams(src, name, out var retType);
        Console.WriteLine(ConvertFunctionDdl(name, def, ps.Where(p => p.Name.Length > 0).ToList(), retType));
    }
    else
    {
        var ps = LoadProcParams(src, name);
        Console.WriteLine(ConvertProcDdl(name, def, ps));
    }
}

// Dump every stored-procedure body from BOTH engines to UTF-8 files, for the static write-path / alias
// comparison (cmp_writes.py).
//
// This exists because `sqlcmd` CANNOT be used for it: it renders the bodies through the console code page and
// turns every Arabic identifier into '?????'. These procs are full of Arabic column aliases
// (AS 'الرمز', AS 'اسم الراسل'), so a sqlcmd dump makes every alias compare unequal and the check is useless.
// SqlClient reads NVARCHAR properly, so the dump is done here instead.
void DoDumpBodies(Cfg c)
{
    var outDir = args.Length > 1 ? args[1] : ".";
    Directory.CreateDirectory(outDir);

    // every schema the app writes through — the ledger procs live in the satellite DBs
    var dbs = new[] { "EXCHANGESYS2026", "ExSyAccounts2026", "ExSyAccountsCurrency2026" };
    var ss = new StringBuilder();
    var my = new StringBuilder();

    foreach (var db in dbs)
    {
        var srcCs = System.Text.RegularExpressions.Regex.Replace(c.SqlServer, @"(?i)Database=[^;]*", "Database=" + db);
        try
        {
            using var s = Open(srcCs);
            using var cmd1 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
                JOIN sys.sql_modules m ON o.object_id=m.object_id
                WHERE o.type='P' AND o.is_ms_shipped=0", s);
            using var r1 = cmd1.ExecuteReader();
            while (r1.Read()) ss.Append("###").Append(db).Append("__").Append(r1.GetString(0)).Append("###").AppendLine(r1.GetString(1));
        }
        catch (Exception ex) { Console.WriteLine($"  skip source {db}: {ex.Message}"); }

        var myCs = System.Text.RegularExpressions.Regex.Replace(c.MySql, @"(?i)Database=[^;]*", "Database=" + db);
        try
        {
            using var m = OpenMy(myCs);
            using var cmd2 = new MySqlCommand(
                "SELECT ROUTINE_NAME, ROUTINE_DEFINITION FROM information_schema.ROUTINES " +
                "WHERE ROUTINE_SCHEMA=@d AND ROUTINE_TYPE='PROCEDURE'", m);
            cmd2.Parameters.AddWithValue("@d", db);
            using var r2 = cmd2.ExecuteReader();
            while (r2.Read()) my.Append("###").Append(db).Append("__").Append(r2.GetString(0)).Append("###").AppendLine(r2.GetString(1));
        }
        catch (Exception ex) { Console.WriteLine($"  skip target {db}: {ex.Message}"); }
    }

    var enc = new UTF8Encoding(false);
    File.WriteAllText(Path.Combine(outDir, "ss_bodies.txt"), ss.ToString(), enc);
    File.WriteAllText(Path.Combine(outDir, "my_bodies.txt"), my.ToString(), enc);
    Console.WriteLine($"dumped -> {outDir}: ss={CountMarkers(ss)} procs, my={CountMarkers(my)} procs (UTF-8)");
}

static int CountMarkers(StringBuilder sb) =>
    System.Text.RegularExpressions.Regex.Matches(sb.ToString(), @"^###", System.Text.RegularExpressions.RegexOptions.Multiline).Count;

void DoFunctions(Cfg c)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET NAMES utf8mb4;");
    // README_MYSQL §7 (error 1267): SET NAMES alone leaves the CONNECTION collation at utf8mb4_general_ci
    // while every column is utf8mb4_unicode_ci. MySQL BAKES the session collation into a routine at CREATE
    // time, so without this the routine hits "Illegal mix of collations" at runtime on any UNION or string
    // comparison. Must be set on the CREATING connection, not just in the emitted .sql header.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");
    // IGNORE_SPACE: allow "sum (x)" (space before paren) common in this T-SQL; drop pedantic modes for legacy queries
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");

    var fns = new List<(string Name, string Def)>();
    using (var cmd2 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.type='FN' AND o.name NOT LIKE 'Private_%'
          AND m.definition NOT LIKE '%tsqlt%' ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) fns.Add((r.GetString(0), r.GetString(1)));

    // build header metadata + converted DDL up front
    var ddlMap = new Dictionary<string, string>();
    var paramsMap = new Dictionary<string, List<Col>>();
    var retMap = new Dictionary<string, string>();
    var fnDef = fns.ToDictionary(f => f.Name, f => f.Def);
    foreach (var (name, def) in fns)
    {
        var ps = LoadFnParams(src, name, out var retType);
        paramsMap[name] = ps.Where(p => p.Name.Length > 0).ToList();   // inputs only (param_id 0 = return)
        retMap[name] = retType;
        ddlMap[name] = ConvertFunctionDdl(name, def, paramsMap[name], retType);
    }

    // CREATE pass with retries (function-calls-function dependencies)
    var pending = fns.Select(f => f.Name).ToList(); var ok = new List<string>(); var err = new Dictionary<string, string>();
    for (int passNo = 0; passNo < 4 && pending.Count > 0; passNo++)
    {
        var still = new List<string>();
        foreach (var name in pending)
        {
            try { Exec(dst, $"DROP FUNCTION IF EXISTS `{name}`"); Exec(dst, ddlMap[name]); ok.Add(name); }
            catch (Exception ex) { err[name] = OneLine(ex.Message); still.Add(name); }
        }
        if (still.Count == pending.Count) break;
        pending = still;
    }

    // diff-test created functions with a type-based sample input set
    int pass = 0; var diffFail = new List<string>(); var untested = new List<string>(); var passed = new List<string>(); var srcBroken = new List<string>();
    foreach (var name in ok)
    {
        var ps = paramsMap[name];
        if (ps.Any(p => MapType(p).Contains("BLOB")))
        { untested.Add($"{name}: has binary param"); continue; }
        var args = string.Join(",", ps.Select(SampleArg));
        string a;
        try { a = NormScalar(Scalar(src, $"SELECT dbo.[{name}]({args})")); }
        catch (Exception sx)   // SQL Server ORIGINAL errors -> broken source (e.g. dropped table)
        {
            bool myOk = true; try { Scalar(dst, $"SELECT `{name}`({args})"); } catch { myOk = false; }
            if (!myOk) srcBroken.Add($"{name}: source error ({OneLine(sx.Message)})");
            else diffFail.Add($"{name}: SQL-source errors but MySQL ran");
            continue;
        }
        try
        {
            var b = NormScalar(Scalar(dst, $"SELECT `{name}`({args})"));
            if (a == b) { pass++; passed.Add(name); continue; }
            // RETRY with '+'->CONCAT (only this failing fn; can't regress others). Recreate + re-test.
            try
            {
                var ddl2 = ConvertFunctionDdl(name, fnDef[name], ps, retMap[name], applyConcat: true);
                Exec(dst, $"DROP FUNCTION IF EXISTS `{name}`"); Exec(dst, ddl2);
                var b2 = NormScalar(Scalar(dst, $"SELECT `{name}`({args})"));
                if (a == b2) { pass++; passed.Add(name); ddlMap[name] = ddl2; continue; }
            }
            catch { }
            diffFail.Add($"{name}({args}): SQL=[{a}] MySQL=[{b}]");
        }
        catch (Exception ex) { diffFail.Add($"{name}: MySQL call error {OneLine(ex.Message)}"); }
    }
    if (srcBroken.Count > 0) File.WriteAllText("functions_source_broken.txt",
        "Functions whose SQL Server ORIGINAL also errors (broken source). Conversion faithful; not bugs.\n\n" + string.Join("\n", srcBroken), new UTF8Encoding(false));
    WriteVerified("converted_verified_functions.sql", "FUNCTION", passed.Select(n => ddlMap[n]));

    var createFail = pending.Select(n => $"{n}: {err.GetValueOrDefault(n, "?")}").ToList();
    File.WriteAllText("functions_needs_manual.txt",
        "SCALAR FUNCTIONS NEEDING MANUAL WORK\n\n== failed to CREATE (T-SQL body needs hand-translation: multi-line statements, SELECT @x= patterns, etc.) ==\n"
        + string.Join("\n", createFail) + "\n\n== created but diff FAILED ==\n" + string.Join("\n", diffFail)
        + "\n\n== created, not auto-tested ==\n" + string.Join("\n", untested), new UTF8Encoding(false));

    Console.WriteLine($"Functions: {fns.Count} total | created {ok.Count} | diff PASS {pass} | not-tested {untested.Count} | source-broken {srcBroken.Count} | real diff-fail {diffFail.Count}");
    Console.WriteLine($"  (create-fail {createFail.Count}, real diff-fail {diffFail.Count}, source-broken {srcBroken.Count})");
}

List<Col> LoadFnParams(SqlConnection c, string fnName, out string retType)
{
    using var cmd2 = new SqlCommand(@"SELECT p.name, ty.name, p.max_length, p.precision, p.scale, p.parameter_id
        FROM sys.parameters p JOIN sys.types ty ON p.user_type_id=ty.user_type_id
        WHERE p.object_id=OBJECT_ID(@n) ORDER BY p.parameter_id", c);
    cmd2.Parameters.AddWithValue("@n", "dbo." + fnName);
    using var r = cmd2.ExecuteReader();
    var list = new List<Col>(); retType = "INT";
    while (r.Read())
    {
        var col = new Col { Name = r.GetString(0).TrimStart('@'), Type = r.GetString(1), MaxLength = r.GetInt16(2), Precision = r.GetByte(3), Scale = r.GetByte(4), IsNullable = true };
        if (r.GetInt32(5) == 0) retType = MapType(col);   // parameter_id 0 = return value
        else list.Add(col);
    }
    return list;
}

static string ConvertFunctionDdl(string name, string def, List<Col> inputs, string retType, bool applyConcat = false)
{
    // body = everything after "RETURNS <type>" and an OPTIONAL "AS" (T-SQL functions often omit AS:
    // "returns bigint begin ... end"). The return type may be "type" or "type(n,m)".
    var m = System.Text.RegularExpressions.Regex.Match(def, @"\bRETURNS\s+\w+(?:\s*\([^)]*\))?\s*(?:\bAS\b)?\s*([\s\S]*)$",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    string body = TransformBody(m.Success ? m.Groups[1].Value : def, inputs.Select(p => p.Name), applyConcat);
    var ph = string.Join(", ", inputs.Select(p => $"`p_{p.Name}` {MapType(p)}"));
    return $"CREATE FUNCTION `{name}`({ph}) RETURNS {retType} DETERMINISTIC\n{body}";
}

// T-SQL TRY/CATCH + transaction -> MySQL HANDLER + START TRANSACTION. No-op when there's no CATCH.
static string ConvertTryCatch(string body)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    var S = System.Text.RegularExpressions.RegexOptions.Singleline;
    var catchM = System.Text.RegularExpressions.Regex.Match(body, @"BEGIN\s+CATCH\b(.*?)\bEND\s+CATCH\b", IC | S);
    string catchBody = catchM.Success ? catchM.Groups[1].Value : "";
    if (catchM.Success) body = body.Remove(catchM.Index, catchM.Length);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bEND\s+TRY\b", "", IC);
    // transaction keywords (whole body)
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bBEGIN\s+TRAN(SACTION)?\b", "START TRANSACTION", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\b(COMMIT|ROLLBACK)\s+TRAN(SACTION)?\b", "$1", IC);
    // RAISERROR/THROW -> SIGNAL ; EXEC proc -> CALL proc()
    // RAISERROR ( N'message', 16, 1 );  ->  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'message';
    // The call is frequently written across SEVERAL LINES ("RAISERROR (" alone, then the args). A
    // "[^\n]*" rule only consumed the first line and left "N'..', 16, 1 );" behind as orphaned garbage, which
    // is a hard syntax error. Match the whole balanced argument list instead, across newlines, and KEEP the
    // author's message (the first argument) rather than replacing every error with a generic 'error' —
    // the app surfaces these strings to the user, and they are in Arabic.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?is)\bRAISERROR\b\s*(?:\(\s*((?:[^()]|\([^()]*\))*?)\s*\))?\s*;?", m =>
    {
        var args = m.Groups[1].Success ? SplitTopLevelStr(m.Groups[1].Value) : new List<string>();
        string msg = args.Count > 0 ? args[0].Trim() : "";
        // keep only a literal message; a variable/expression is not safe to inline into SET MESSAGE_TEXT
        var lit = System.Text.RegularExpressions.Regex.Match(msg, @"^[Nn]?('(?:[^']|'')*')$");
        var text = lit.Success ? lit.Groups[1].Value : "'error'";
        return "SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT=" + text + ";";
    }, IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bTHROW\b[^\n]*", "SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='error'", IC);
    if (catchM.Success)
    {
        string handler;
        // The overwhelmingly common CATCH here is "capture the error and RE-RAISE it":
        //     ROLLBACK
        //     DECLARE @ErrMsg ... ; SELECT @ErrMsg = ERROR_MESSAGE(), ... ; RAISERROR(@ErrMsg, ...)
        // MySQL's RESIGNAL re-raises the CURRENT handled exception WITH ITS ORIGINAL MESSAGE — exactly the
        // intent, in one keyword. It also beats the generic SIGNAL '...error' because the app sees the real
        // message. The error-variable DECLAREs and the SELECT @x=ERROR_MESSAGE() existed only to feed
        // RAISERROR, so they are subsumed. Detected on the RAW catch body (RAISERROR/THROW present).
        if (System.Text.RegularExpressions.Regex.IsMatch(catchBody, @"\b(RAISERROR|THROW)\b", IC))
        {
            handler = "DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; RESIGNAL; END;";
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(catchBody, @"\bERROR_(NUMBER|MESSAGE|LINE|STATE|SEVERITY|PROCEDURE)\s*\(\s*\)", IC))
        {
            // CATCH that returns the error to the caller as a RESULT SET: "SELECT ERROR_NUMBER() AS ErrorNumber,
            // ERROR_MESSAGE() AS ErrorMessage". T-SQL's ERROR_* functions have no MySQL equivalent; the handled
            // condition's number/message come from GET DIAGNOSTICS instead. Collapse the (multi-line) catch body
            // to one line, swap the ERROR_* calls for the captured session vars, and prepend the GET DIAGNOSTICS.
            // __err_no/__err_msg are LOCALs declared inside the handler block (undecorated names — a later pass
            // strips the '@' off session vars, which would break GET DIAGNOSTICS targets). DECLAREs come first,
            // as MySQL requires, then ROLLBACK, then the capture.
            var cb = System.Text.RegularExpressions.Regex.Replace(catchBody, @"\s*\r?\n\s*", " ");
            cb = System.Text.RegularExpressions.Regex.Replace(cb, @"\bROLLBACK\b[^;]*;?", "", IC).Trim();  // handler rolls back itself
            cb = System.Text.RegularExpressions.Regex.Replace(cb, @"\bERROR_NUMBER\s*\(\s*\)", "__err_no", IC);
            cb = System.Text.RegularExpressions.Regex.Replace(cb, @"\bERROR_MESSAGE\s*\(\s*\)", "__err_msg", IC);
            cb = System.Text.RegularExpressions.Regex.Replace(cb, @"\bERROR_(LINE|STATE|SEVERITY|PROCEDURE)\s*\(\s*\)", "NULL", IC);
            cb = cb.TrimEnd();
            if (cb.Length > 0 && !cb.EndsWith(";")) cb += ";";
            handler = "DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN " +
                      "DECLARE __err_no BIGINT DEFAULT 0; DECLARE __err_msg VARCHAR(512) DEFAULT ''; ROLLBACK; " +
                      "GET DIAGNOSTICS CONDITION 1 __err_no = MYSQL_ERRNO, __err_msg = MESSAGE_TEXT; " + cb + " END;";
        }
        else
        {
            // No re-raise (e.g. swallow-and-log, or set an OUT param): keep the catch statements. Build the
            // handler as a SINGLE LINE with each statement ';'-terminated, so HoistDeclares moves it to the
            // top of the block (MySQL requires DECLARE ... HANDLER before any executable statement).
            var cbLines = catchBody.Split('\n')
                .Select(l => System.Text.RegularExpressions.Regex.Replace(l, @"\bROLLBACK\b[^\n]*", "", IC).Trim())  // handler does its own rollback
                .Where(l => l.Length > 0)
                .Select(l => l.TrimEnd(';') + ";");
            var cb = string.Join(" ", cbLines);
            handler = "DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN ROLLBACK; " + cb + " END;";
        }
        body = System.Text.RegularExpressions.Regex.Replace(body, @"\bBEGIN\s+TRY\b", handler, IC);
    }
    // EXEC [db].[dbo].[proc] [args]  ->  CALL proc([args]).  Handles bracketed/cross-DB names (runs before
    // bracket/dbo stripping) and the OUTPUT/OUT keyword (dropped — OUT-ness lives in the MySQL signature).
    // The argument list spans MULTIPLE LINES here: the legacy EXECs pass ~18 positional args one per line.
    // The old "[^;\r\n]*?" arg capture stopped at the first newline, dropping every arg -> "CALL proc()"
    // (this silently un-armed ExSyAccounts_AccSafeActivityTb_Insert in many write procs). Now args are
    // captured across newlines with Singleline, stopping at the next ';' or the next top-level statement.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"\bEXEC(?:UTE)?\s+(?:\[?\w+\]?\s*\.\s*)?(?:\[?dbo\]?\s*\.\s*)?\[?([A-Za-z0-9_]+)\]?[ \t]*" +
        @"((?:(?!\b(?:SELECT|INSERT|UPDATE|DELETE|EXEC|EXECUTE|IF|ELSEIF|ELSE|WHILE|BEGIN|END|SET|DECLARE|RETURN|COMMIT|ROLLBACK|CALL|WITH)\b)[^;])*?)" +
        @"\s*(?=;|\r?\n\s*(?:SELECT|INSERT|UPDATE|DELETE|EXEC|EXECUTE|IF|ELSEIF|ELSE|WHILE|BEGIN|END|SET|DECLARE|RETURN|COMMIT|ROLLBACK|CALL|WITH)\b|\z)",
        m =>
        {
            var args = System.Text.RegularExpressions.Regex.Replace(m.Groups[2].Value, @"\b(OUTPUT|OUT)\b", "", IC).Trim();
            args = System.Text.RegularExpressions.Regex.Replace(args, @"\s*\r?\n\s*", " ").Trim();   // collapse to one line
            return args.Length == 0 ? $"CALL {m.Groups[1].Value}()" : $"CALL {m.Groups[1].Value}({args})";
        }, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
    // AccBranchcticityTb_insert has a trailing T-SQL default param (@PaymentMethodsID = 1). Legacy EXECs pass
    // 11 args and rely on the default; MySQL CALL needs all 12 -> pad an 11-arg call with the literal 1.
    // (only matches calls whose args contain no nested parentheses — those are simple var/literal lists.)
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bCALL\s+AccBranchcticityTb_insert\s*\(([^()]*)\)", mm =>
    {
        var a = SplitTopLevel(mm.Groups[1].Value, ',');
        return a.Count == 11 ? "CALL AccBranchcticityTb_insert(" + mm.Groups[1].Value.TrimEnd() + ", 1)" : mm.Value;
    }, IC);
    return body;
}

// Shared T-SQL body -> MySQL/MariaDB body transform (used by functions and procs).
// applyConcat: opt-in '+'->CONCAT (only used as a RETRY on already-failing objects, so it cannot regress a pass).
static string TransformBody(string body, IEnumerable<string> paramNames, Boolean applyConcat, IEnumerable<string> tvpNames = null)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    // Inline parameterized TVF calls FIRST, on the raw T-SQL, so the injected SELECT is then converted by
    // every rule below exactly like hand-written body text (see LoadTvfs / InlineTvfCalls).
    //
    // RETRY-ONLY (same convention as applyConcat): a TVF inlined as a derived table can be far SLOWER than
    // the TVF was on SQL Server, and applying it to procs that already convert cleanly made the diff-test
    // phase hang on a heavy query. Restricting it to objects that FAILED without it means it can never
    // regress — or slow down — a proc that already works.
    if (applyConcat) body = InlineTvfCalls(body);
    body = ConvertTryCatch(body);                                                          // TRY/CATCH -> HANDLER (no-op if none)
    body = StripSqlComments(body);   // string-aware: never eats a literal containing '--'
    // A block-opening "BEGIN;" is legal in T-SQL (BEGIN + an empty statement) — several procs write it right
    // after AS ("AS\n BEGIN;"). In MySQL a block BEGIN never takes a ';' (a DECLARE cannot follow "BEGIN;"),
    // so strip the trailing ';' from a bare block-opening BEGIN. "BEGIN TRANSACTION"/"BEGIN TRY" don't match
    // (a keyword sits between BEGIN and the ';'), so only the block opener is touched.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bBEGIN\s*;", "BEGIN", IC);
    // T-SQL tolerates a number GLUED to a following clause keyword ("max(id)+1from [t]", "@x=1where .."). MySQL
    // needs the space, and — worse — the missing word boundary makes "\bFROM\b" miss it, so the SELECT-assignment
    // rewrite loses the FROM clause and emits an invalid "SET x = ..+1from t". An identifier cannot start with a
    // digit, so a digit immediately followed by one of these keywords is always a token boundary — insert a space.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)(\d)(FROM|WHERE|GROUP|ORDER|HAVING|UNION|AND|OR)\b", "$1 $2");
    // strip dbo. — but when glued to a preceding word with brackets (e.g. "from[dbo].X") leave a SPACE so the
    // tokens don't merge into "fromX"; the schema-qualified "EmployeeSalary.dbo.X" (dot-preceded) collapses to empty.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?<=\w)\[?dbo\]?\s*\.", " ", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\[?dbo\]?\s*\.", "", IC);  // remaining dbo. / [dbo]. (case-insensitive)
    // The Exchange DB also reaches rhalla2026Teset through its NON-dbo owner schema:
    // "rhalla2026Teset.[db_owner].[whatsapp_contacts]". Left alone this yields an invalid 3-part MySQL name.
    // Strip db_owner exactly like dbo so it collapses to the valid 2-part `rhalla2026Teset`.`whatsapp_contacts`.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?<=\w)\[?db_owner\]?\s*\.", " ", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\[?db_owner\]?\s*\.", "", IC);

    // T-SQL TABLE-VALUED FUNCTIONS that were re-created as MySQL VIEWS (MySQL has no TVFs — see
    // handport_tvf_*.sql). A caller says  FROM dbo.GET_TABLE_FOR_Costof() AS a , and after the dbo. strip that
    // is  FROM GET_TABLE_FOR_Costof() AS a  — invalid, because you cannot invoke a view like a function.
    // Strip the empty "()" so it reads as a plain view reference. Only the PARAMETERLESS TVFs are views; the
    // parameterized ones are handled separately, so only their exact names are listed here.
    foreach (var tvfView in new[] { "GET_TABLE_FOR_Costof", "NEW_GET_TABLE_FOR_Costof" })
        body = System.Text.RegularExpressions.Regex.Replace(
            body, @"\b" + tvfView + @"\s*\(\s*\)", tvfView, IC);

    body = StripTypeBrackets(body);   // [VARCHAR](200) -> VARCHAR(200)  BEFORE [x] -> `x`
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\[([^\]]+)\]", "`$1`");
    // T-SQL session SET options have no MySQL equivalent -> strip them. NOCOUNT was already handled, but
    // XACT_ABORT is used by ~half the procs in this database ("SET XACT_ABORT ON;" right after AS) and was
    // left in place, so MySQL choked on the bare "ON;" and the proc failed to create.
    // XACT_ABORT ON means "abort+rollback the whole transaction on any error" -- that behavior is already
    // provided by the EXIT HANDLER that ConvertTryCatch/the proc wrapper installs, so dropping the statement
    // does not change semantics.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bSET\s+(?:NOCOUNT|XACT_ABORT|ANSI_NULLS|QUOTED_IDENTIFIER|ANSI_WARNINGS|ANSI_PADDING|ARITHABORT|CONCAT_NULL_YIELDS_NULL|NUMERIC_ROUNDABORT|IMPLICIT_TRANSACTIONS|CURSOR_CLOSE_ON_COMMIT|DEADLOCK_PRIORITY)\s+(?:ON|OFF|LOW|HIGH|NORMAL)\s*;?", "");
    // join multi-line DECLARE lists ("DECLARE @a int,\n @b nvarchar(max),\n @c date") onto one line so the
    // multi-declare splitter below sees all items (repeat until stable for 3+ line lists).
    // ...but first pull up a bare "DECLARE" that sits ALONE on its line (the list starts on the NEXT line):
    //     DECLARE
    //       @AccCreditVal AS DECIMAL(15,3),
    //       @AccdebitVal  AS DECIMAL(15,3);
    // The joiner below only continues a line that already ends in a comma, so without this the list is
    // orphaned and every item after the first is emitted as a bare statement -> syntax error.
    // (?:\r?\n[ \t]*)+ — one or MORE newline runs, so a BLANK line between the bare DECLARE and the first item
    //   DECLARE
    //                     <- blank line
    //     @MaxCodeID INT,
    // is still collapsed. With a single "\r?\n" the blank line left DECLARE orphaned and the last list item was
    // emitted as a stray empty "DECLARE ;" (MySQL syntax error).
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)^([ \t]*)DECLARE[ \t]*(?:\r?\n[ \t]*)+", "$1DECLARE ");
    // Join a multi-line DECLARE list onto one line. Two continuation styles occur, and they INTERLEAVE in the
    // messiest procs (comma on its OWN line between items, e.g. after a stripped trailing comment):
    //     DECLARE @A AS BIGINT
    //             ,
    //             @B AS BIGINT ,
    //             @C AS BIGINT
    //             ,
    //   (a) TRAILING comma: "DECLARE ..,\n <next>"  -> pull the next line up.
    //   (b) LEADING  comma: "DECLARE ..\n ,"        -> pull the comma up (so (a) then fires on the next pass).
    // Running each once, in a fixed order, leaves the tail of an interleaved list orphaned as a stray
    // "DECLARE ;" (every item after the break vanishes). Running BOTH to a shared fixpoint collapses the
    // whole list regardless of how the commas are laid out.
    { string prev; do {
        prev = body;
        body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)(\bDECLARE\b[^\n;]*,)\s*\r?\n\s*", "$1 ");
        body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)^([ \t]*DECLARE\b[^\n;]*?)\s*\r?\n[ \t]*,", "$1,");
    } while (body != prev); }
    // T-SQL "DELETE <table> [WHERE ..]" (no FROM) -> MySQL "DELETE FROM <table>"; the WHERE/end lookahead keeps
    // the alias form "DELETE a FROM t a JOIN.." untouched (already MySQL-valid).
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)\bDELETE\s+(?!FROM\b)([\w.`]+)\s*(?=\bWHERE\b|;|$)", "DELETE FROM $1 ");
    // T-SQL "UPDATE <alias> SET <sets> FROM <src> [WHERE <cond>]" -> MySQL "UPDATE <src> SET <sets> [WHERE ..]".
    // MySQL has no UPDATE..FROM; the FROM table source (incl. JOINs) moves to right after UPDATE and the
    // redundant leading alias is dropped. The statement END is ';' OR the next top-level statement keyword —
    // NOT just ';': this converter runs before the boundary pass inserts semicolons, and the legacy T-SQL
    // omits them, so a "[^;]+;" terminator silently skipped every multi-line UPDATE..FROM (they then failed
    // with "near 'FROM ..'"). A plain UPDATE..WHERE (no FROM) is left unchanged by the FROM check inside.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bUPDATE\s+([\w.`]+)\s+SET\s+((?:(?!\b(?:UPDATE|INSERT|DELETE|SELECT|IF|ELSEIF|ELSE|WHILE|END|BEGIN|DECLARE|RETURN|COMMIT|ROLLBACK|CALL)\b)[^;])+?)\s*(?=;|\r?\n\s*(?:UPDATE|INSERT|DELETE|SELECT|IF|ELSEIF|ELSE|WHILE|END|BEGIN|DECLARE|RETURN|COMMIT|ROLLBACK|CALL)\b|\z);?",
        mm =>
    {
        var target = mm.Groups[1].Value;
        var stmt = mm.Groups[2].Value;
        var fromM = System.Text.RegularExpressions.Regex.Match(stmt, @"\bFROM\b", IC);
        if (!fromM.Success) return mm.Value;   // plain UPDATE..SET..WHERE -> leave as-is
        var sets = stmt.Substring(0, fromM.Index).Trim();
        var rest = stmt.Substring(fromM.Index + fromM.Length);
        var whM = System.Text.RegularExpressions.Regex.Match(rest, @"\bWHERE\b", IC);
        var src = (whM.Success ? rest.Substring(0, whM.Index) : rest).Trim();
        var where = whM.Success ? " " + rest.Substring(whM.Index).Trim() : "";

        // The UPDATE TARGET must survive. In the usual T-SQL shape the target is an ALIAS that is also
        // defined in the FROM clause ("UPDATE a SET .. FROM Invoices a JOIN .."), so emitting
        // "UPDATE <from-source> SET .." is right. But when the target is a TABLE that does NOT appear in
        // the FROM ("UPDATE CATEGORYTYPESDETAILSTB SET .. FROM @Type AS a"), dropping it made the FROM
        // source the update target — MySQL then happily UPDATEd the tvp_Type TEMP TABLE and the real table
        // was never written. The screen "saves" and nothing changes, with no error anywhere.
        // So: if the target is not present in the FROM source, keep it in the (multi-table) UPDATE list.
        var bare = target.Trim('`');
        bool targetInSrc = System.Text.RegularExpressions.Regex.IsMatch(
            src, @"(?i)(^|[^\w.])" + System.Text.RegularExpressions.Regex.Escape(bare) + @"($|[^\w])");
        var tables = targetInSrc ? src : target + ", " + src;
        return "UPDATE " + tables + " SET " + sets + where + ";";
    });
    // T-SQL temp tables (#X): MySQL has no '#' (it also starts a comment, which would silently eat the table name).
    // Rename to tmp_X; CREATE TABLE -> CREATE TEMPORARY TABLE (drop-if-exists so re-calling the proc in a session is safe);
    // "INSERT #X" (T-SQL allows no INTO) -> "INSERT INTO tmp_X".
    if (body.Contains('#'))
    {
        body = System.Text.RegularExpressions.Regex.Replace(body, @"#(\w+)", "tmp_$1");
        body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bCREATE\s+TABLE\s+(tmp_\w+)", "DROP TEMPORARY TABLE IF EXISTS $1; CREATE TEMPORARY TABLE $1");
        body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bINSERT\s+(tmp_\w+)", "INSERT INTO $1");
    }
    // T-SQL "IF OBJECT_ID('tempdb..X') IS NOT NULL DROP TABLE X;" temp-table guards have no MySQL equivalent.
    // The CREATE TABLE -> CREATE TEMPORARY rewrite above already prepends a DROP TEMPORARY TABLE IF EXISTS, so
    // these guards are redundant — strip them (the lone single-line IF..DROP is also invalid MySQL syntax).
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bIF\s+OBJECT_ID\s*\([^)]*\)\s+IS\s+NOT\s+NULL\s+DROP\s+TABLE\s+[\w`]+\s*;?", "");
    // any remaining bare "DROP TABLE tmp_x" -> idempotent temporary drop
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bDROP\s+TABLE\s+(tmp_\w+)", "DROP TEMPORARY TABLE IF EXISTS $1");
    // "RowID INT IDENTITY(1,1)" in a #temp / table-variable DDL -> AUTO_INCREMENT.
    // MySQL additionally requires an AUTO_INCREMENT column to be a key, and in these staging tables the
    // identity column IS the key (it exists to number the rows for the ROW_NUMBER-style loops below), so
    // PRIMARY KEY is added. Only seeds of the form (1,1) are accepted — any other seed/increment would
    // change the numbering, and is left alone so the proc fails loudly instead of renumbering silently.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bIDENTITY\s*\(\s*1\s*,\s*1\s*\)", "AUTO_INCREMENT PRIMARY KEY");
    // T-SQL TABLE VARIABLES: "DECLARE @TIMPP AS TABLE (BName NVARCHAR(MAX), Debit DECIMAL(18,3), ..)".
    // MySQL has no table variable, but it is exactly what a #temp table is here — a session-scoped staging
    // table — so it becomes  DROP TEMPORARY TABLE IF EXISTS tmp_X; CREATE TEMPORARY TABLE tmp_X (..);
    // and every later "@X" reference is rewritten to tmp_X.
    //
    // ORDER MATTERS: this must run BEFORE the @-parameter rewrites below, which would otherwise turn
    // "@TIMPP" into a scalar "p_TIMPP" and lose the table entirely; and before the multi-DECLARE splitter,
    // which would try to split the column list. The column list is left verbatim so the normal type mapping
    // (NVARCHAR(MAX) -> LONGTEXT, etc.) still applies to it further down the pipeline.
    {
        var tblVars = new List<string>();
        var tvRe = new System.Text.RegularExpressions.Regex(@"(?i)\bDECLARE\s+@(\w+)\s+(?:AS\s+)?TABLE\s*\(");
        for (int guard = 0; guard < 100; guard++)
        {
            var mtv = tvRe.Match(body);
            if (!mtv.Success) break;
            var nm = mtv.Groups[1].Value;
            int open = body.IndexOf('(', mtv.Index);
            var cols = ReadBalanced(body, open, out int closeAfter);
            if (cols == null) break;                       // unbalanced — leave it for the manual bucket
            body = body.Substring(0, mtv.Index)
                 + "DROP TEMPORARY TABLE IF EXISTS tmp_" + nm + ";\nCREATE TEMPORARY TABLE tmp_" + nm + " (" + cols + ");"
                 + body.Substring(closeAfter);
            tblVars.Add(nm);
        }
        foreach (var nm in tblVars.Distinct())
            body = System.Text.RegularExpressions.Regex.Replace(
                body, @"@" + System.Text.RegularExpressions.Regex.Escape(nm) + @"\b", "tmp_" + nm, IC);
    }
    // A multi-declare written across SEVERAL lines --
    //     DECLARE @IDCode      INT,
    //             @SumVal      DECIMAL(18, 3),
    //             @TotalDebit  DECIMAL(18, 3) = 0;
    // -- is invisible to the line-anchored splitter below, which sees only "DECLARE @IDCode INT," and leaves
    // the continuation lines stranded as bare expressions. Join them first.
    // The continuation test is exact rather than heuristic: a T-SQL declaration list continues if and only if
    // the line ends with a comma, so nothing else can be swallowed.
    // A DECLARE statement continues onto the next line in exactly two cases, and both are structural rather
    // than heuristic, so nothing else can be swallowed:
    //   * the line ends with a comma            -> another declarator follows
    //   * its parentheses are still unbalanced  -> we are inside a default value, e.g.
    //         DECLARE @b      INT = 1,
    //                 @bCount INT = (SELECT
    //                             COUNT(*)
    //                     FROM
    //                             @BranchBalances);
    {
        var dlines = body.Split('\n').ToList();
        for (int i = 0; i < dlines.Count; i++)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(dlines[i], @"^[ \t]*DECLARE\b", IC)) continue;
            // a handler's body is a BEGIN..END block, not a declarator list — never join it
            if (System.Text.RegularExpressions.Regex.IsMatch(dlines[i].Trim(),
                    @"^DECLARE\s+(EXIT|CONTINUE|UNDO)\s+HANDLER\b", IC)) continue;
            while (i + 1 < dlines.Count)
            {
                var cur = dlines[i];
                int bal = cur.Count(c => c == '(') - cur.Count(c => c == ')');
                if (bal <= 0 && !cur.TrimEnd().EndsWith(",")) break;
                dlines[i] = cur.TrimEnd() + " " + dlines[i + 1].Trim();
                dlines.RemoveAt(i + 1);
            }
        }
        body = string.Join("\n", dlines);
    }
    // T-SQL "DECLARE @a AS t, @b AS t2" (multi-declare) -> separate "DECLARE @a AS t;" lines (MySQL requirement).
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)^[ \t]*DECLARE\s+(.+)$", mm =>
    {
        if (mm.Value.Contains("HANDLER", StringComparison.OrdinalIgnoreCase)) return mm.Value;
        // .Trim() first: a trailing "; " (semicolon THEN whitespace) survives a bare TrimEnd(';') and the
        // per-item ";" we append then yields "...DECIMAL(15,3);;" -- an empty statement MySQL rejects.
        var items = SplitTopLevel(mm.Groups[1].Value.Trim().TrimEnd(';'), ',');
        return items.Count < 2 ? mm.Value : string.Join("\n", items.Select(it => "DECLARE " + it.Trim().TrimEnd(';').Trim() + ";"));
    });
    // TVP (table-valued) params have no MySQL equivalent: the data layer stages the passed rows into a session
    // TEMPORARY TABLE named tvp_<name>, so rewrite the @<tvp> table references to that table (NOT a p_ scalar param).
    if (tvpNames != null)
        foreach (var tn in tvpNames.Distinct())
            body = System.Text.RegularExpressions.Regex.Replace(body, @"@" + System.Text.RegularExpressions.Regex.Escape(tn) + @"\b", "tvp_" + tn, IC);
    // Prefix PARAMETERS with p_ so a param named like a column (e.g. @ID vs column ID) can't collide —
    // MariaDB would otherwise resolve the bare name to the column, making "col = param" an always-true "col = col".
    foreach (var pn in paramNames.Distinct())
        body = System.Text.RegularExpressions.Regex.Replace(body, @"@" + System.Text.RegularExpressions.Regex.Escape(pn) + @"\b", "p_" + pn, IC);
    // Prefix DECLAREd local variables with v_ (while @ still marks them) so they can't collide with a
    // same-named column after @-stripping — e.g. T-SQL "@UPass = UPass" -> "v_UPass = UPass" (var vs column).
    foreach (var v in System.Text.RegularExpressions.Regex.Matches(body, @"DECLARE\s+@(\w+)", IC).Select(mm => mm.Groups[1].Value).Distinct())
        body = System.Text.RegularExpressions.Regex.Replace(body, @"@" + System.Text.RegularExpressions.Regex.Escape(v) + @"\b", "v_" + v, IC);
    // T-SQL @@SYSTEM variables MUST be translated BEFORE the generic "@x -> x" strip below: that strip
    // matches the SECOND @ of "@@ROWCOUNT" (the first is followed by @, not a word char), leaving
    // "@ROWCOUNT" - after which the @@ROWCOUNT rule can never match and MySqlConnector reports
    // "Parameter '@ROWCOUNT' must be defined". Order matters here.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bSCOPE_IDENTITY\s*\(\s*\)", "LAST_INSERT_ID()", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"@@IDENTITY\b", "LAST_INSERT_ID()", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"@@ROWCOUNT\b", "ROW_COUNT()", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"@(\w+)", "$1");           // strip @ from remaining (parameters)
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bDECLARE\s+(\w+)\s+AS\s+", "DECLARE $1 ", IC);
    // drop the optional T-SQL CONVERT style argument: CONVERT(type, expr, 101) -> CONVERT(type, expr)
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bsmalldatetime\b", "DATETIME", IC);   // T-SQL smalldatetime -> DATETIME (in DECLAREs)
    // SQL Server TIME/DATETIME2/DATETIMEOFFSET allow fractional-seconds precision up to 7; MySQL's max is 6.
    // A column declared TIME(7) (common in these temp tables) is a hard syntax error in MySQL. Cap any
    // fractional precision >6 at 6. datetime2 also needs its name normalised to datetime.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bdatetime2\s*\(\s*7\s*\)", "DATETIME(6)");
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bdatetime2\b", "DATETIME");
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\b(time|datetime|timestamp)\s*\(\s*7\s*\)", "$1(6)");
    // CONVERT(type[(len)], expr [, style]) -> CAST(expr AS mysqltype).
    // Done with a BALANCED-PAREN parser, not a regex: the expression argument is frequently a call that
    // itself contains parens and commas -- e.g. CONVERT(VARCHAR, ROW_NUMBER() OVER (ORDER BY CuName)) --
    // which the old character-class regexes could not span, leaving a raw CONVERT( behind (34 procs failed
    // on exactly this). MariaDB 10.2+ supports window functions, so once CONVERT is translated they work.
    body = ConvertDateFuncs(body);      // DATEADD/DATEDIFF/DATEPART -> DATE_ADD/TIMESTAMPDIFF/EXTRACT
    body = ConvertTsqlConvert(body);
    // Native T-SQL CAST(x AS <type>) also needs its TARGET TYPE remapped: MySQL's CAST accepts only a small
    // set of types. "CAST(x AS VARCHAR)" / "AS INT" are hard syntax errors -> CHAR / SIGNED.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bAS\s+(?:n?varchar|n?char)\s*(?:\(\s*(?:\d+|max)\s*\))?\s*\)", "AS CHAR)", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bAS\s+(?:int|integer|bigint|smallint|tinyint)\s*\)", "AS SIGNED)", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bISNULL\s*\(", "IFNULL(", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bGETDATE\s*\(\s*\)", "NOW()", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bLEN\s*\(", "CHAR_LENGTH(", IC);
    // CHARINDEX(needle, haystack [, start]) -> LOCATE(needle, haystack [, start]) — same argument order.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bCHARINDEX\s*\(", "LOCATE(", IC);
    // Identity / row-count functions (32 procs use these; there was no handling, so every one failed):
    //   SCOPE_IDENTITY() / @@IDENTITY  -> LAST_INSERT_ID()   (last auto-increment value on this connection)
    //   @@ROWCOUNT                     -> ROW_COUNT()        (rows affected by the previous statement)
    // T-SQL query hint "OPTION (RECOMPILE)" etc. — no MySQL equivalent, drop it (it sits at statement end).
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?i)\bOPTION\s*\([^)]*\)", "");
    // T-SQL TABLE LOCK HINTS "<table> WITH (UPDLOCK, HOLDLOCK)" / "WITH (NOLOCK)" — no MySQL equivalent, drop.
    // Anchored on a hint keyword right after "WITH (" so a CTE ("WITH cte AS (..)") is never touched (a CTE has
    // an identifier + AS before its '(', not a lock keyword).
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bWITH\s*\(\s*(?:NOLOCK|UPDLOCK|HOLDLOCK|ROWLOCK|READPAST|TABLOCKX|TABLOCK|XLOCK|PAGLOCK|NOWAIT|READCOMMITTED|READUNCOMMITTED|REPEATABLEREAD|SERIALIZABLE|SNAPSHOT|FORCESEEK|FORCESCAN|INDEX)[^)]*\)", "");
    // ORDER BY after a top-level UNION may not use table qualifiers in MySQL (error 1250).
    body = FixUnionOrderBy(body);
    // MySQL RESERVED WORDS used as bare column names. T-SQL does not reserve these, so the bodies leave them
    // unquoted ("WHERE keys = @keys"), which is a syntax error in MySQL. Backtick them in identifier position
    // (preceded by . / space / ( / , and followed by a comparator / comma / paren / space) — never inside a
    // string literal.
    //
    // The full set of reserved words actually used as column names in this schema was found EMPIRICALLY, by
    // asking the server "SELECT 1 AS <name>" for all 982 distinct column names rather than trusting a list:
    //     Order, key, keys, long, MaxValue          (plus Arabic/space names, which views already backtick)
    // Only `keys`, `first_value` and `MaxValue` are safe to rewrite unconditionally. `Order`, `key` and `long`
    // are NOT included on purpose: they collide with ORDER BY / PRIMARY KEY / LONG, and a blanket rewrite would
    // corrupt those clauses. If a proc ever needs them, quote at the call site instead.
    // MAXVALUE is reserved only for partition DDL, which this application never emits, so it is collision-free.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?<=[\s.(,])(keys|first_value|maxvalue)(?=\s*[-=<>,)\s])", "`$1`", IC);
    // NOTE: do NOT blindly rewrite "ELSE IF" -> "ELSEIF" here. It looks right (README §3.4) but it fights the
    // braceless-IF handling below: that emits "... END IF;" for the preceding IF, so a following ELSEIF is
    // then orphaned after a closed IF and the routine fails to compile (it broke NormalizePhone /
    // Sum_MainSAfes, which had been converting cleanly). Chained ELSE IF belongs in the hand-port bucket
    // until ConvertControlFlow can scope the whole IF/ELSE-IF chain as one unit.
    // T-SQL FORMAT(x,'N3','en-us') -> MySQL FORMAT(x,3). Silent-wrongness class (money loses its decimals).
    body = FixTsqlFormat(body);
    // '+' string concat -> CONCAT. Runs AFTER CONVERT->CAST so a CAST(.. AS CHAR) operand is recognisable,
    // and after ISNULL->IFNULL. See FixStringConcat: silent-wrongness class, README §3.2.
    body = FixStringConcat(body, CollectStringVars(body));
    // bit comparison: SQL Server accepts ='true'/'false' (->1/0); MySQL converts 'true' to 0 -> wrong rows.
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(=|<>|!=)\s*'true'", "$1 1", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(=|<>|!=)\s*'false'", "$1 0", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bCOLLATE\s+\w+", "", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\b(?:n?varchar|n?char)\s*\(\s*max\s*\)", "LONGTEXT", IC);  // (n)varchar/(n)char(max) -> LONGTEXT
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\b(?:varbinary|binary)\s*\(\s*max\s*\)", "LONGBLOB", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"\bnvarchar\b", "VARCHAR", IC);
    // T-SQL compound assignment: "SET @x += 1" / "-=" / "*=" / "/=". MySQL has no compound assignment
    // operator, so this is a hard syntax error. Expand to "SET x = x + 1". Run BEFORE the IF rewrites so
    // an expanded SET is still recognized as a single-statement IF body.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bSET\s+([vp]_\w+)\s*([+\-*/])=\s*", m => $"SET {m.Groups[1].Value} = {m.Groups[1].Value} {m.Groups[2].Value} ");
    // The SAME compound assignment also appears via SELECT (no FROM): "SELECT @x += 1". If left as a bare
    // SELECT it returns a RESULT SET, which is illegal inside a function ("Not allowed to return a result set
    // from a function" — this is what broke GetAccline). Rewrite to a SET so the no-FROM SELECT-assignment
    // path never sees the '+=' it cannot parse. Var already prefixed v_/p_ at this point.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bSELECT\s+([vp]_\w+)\s*([+\-*/])=\s*([^;\r\n]+)", m =>
            $"SET {m.Groups[1].Value} = {m.Groups[1].Value} {m.Groups[2].Value} ({m.Groups[3].Value.Trim()})");

    // MULTI-LINE braceless IF -- very common in this codebase, e.g.
    //     IF @IDCode IS NULL
    //         SET @IDCode = 1
    //     ELSE
    //         SET @IDCode += 1
    // This MUST run BEFORE ConvertControlFlow: that routine scopes an IF body by its BEGIN..END, and on a
    // braceless IF it mis-scopes, swallowing the following statements and emitting a stray second "END IF".
    // Running here is safe because T-SQL has no THEN in an IF statement, and the condition pattern refuses to
    // cross a THEN (so an already-converted IF can never be re-processed).
    // The condition must not cross THEN (would re-process an already-converted IF) NOR BEGIN. Without the
    // BEGIN guard, a T-SQL one-liner such as
    //     if @AccVal = 0.000 BEGIN select @AccVal = 0.000 END
    //     RETURN isnull(@AccVal, 0.000)
    // has its whole "BEGIN … END" block absorbed INTO the condition, and the RETURN on the next line is
    // treated as the IF body -> "IF … END IF; THEN RETURN …; END IF;". Refusing to cross BEGIN leaves the
    // block for ConvertControlFlow, which scopes BEGIN…END properly.
    const string COND = @"((?:(?!\bTHEN\b)(?!\bBEGIN\b)[^\r\n])+?)";
    const string STMT = @"((?:SET|RETURN|SELECT)\b[^\r\n;]+)";
    // CRITICAL GUARD: the body statement must be COMPLETE on its single line. A T-SQL braceless IF whose body
    // is a multi-line SELECT --
    //     IF @x = 1
    //         SELECT a.Code,
    //                a.Name
    //         FROM ...
    // -- would otherwise be captured as just "SELECT a.Code," and we would staple "; END IF;" onto it,
    // TRUNCATING the statement and orphaning the rest. Requiring the NEXT line to begin a new statement (or
    // end the block) means a continuation line like FROM/WHERE/"a.Name," makes the rule decline to fire, and
    // the proc lands in the manual bucket instead of being silently mangled.
    const string ENDS = @"(?=\s*\r?\n\s*(?:SET|SELECT|IF|ELSE|ELSEIF|RETURN|INSERT|UPDATE|DELETE|END|BEGIN|COMMIT|ROLLBACK|DECLARE|WHILE|CALL|EXEC|SIGNAL|LEAVE|DROP|TRUNCATE)\b|\s*\r?\n\s*$|\s*$)";
    // with ELSE
    body = System.Text.RegularExpressions.Regex.Replace(body,
        $@"(?i)\bIF\s+{COND}\s*\r?\n\s*{STMT};?\s*\r?\n\s*ELSE\s*\r?\n\s*{STMT};?{ENDS}",
        "IF $1 THEN $2; ELSE $3; END IF;");
    // without ELSE (and not followed by one)
    body = System.Text.RegularExpressions.Regex.Replace(body,
        $@"(?i)\bIF\s+{COND}\s*\r?\n\s*{STMT};?(?!\s*\r?\n\s*ELSE\b){ENDS}",
        "IF $1 THEN $2; END IF;");

    // NOTE — a generalised "braceless IF with a MULTI-LINE body" rule was tried here and REVERTED. Finding the
    // body's extent by consuming lines until one starts with a statement keyword looks safe but is not: an
    //     IF @gnm = 1
    //         UPDATE BenefitDistribution
    //         SET ISID = @ISID, ..
    // truncates at the UPDATE's own SET clause, because "SET" also starts a statement. Distinguishing the two
    // needs the curKw/clauseCont state machine that the ';'-insertion pass below already carries, not a regex.
    // These procs are hand-ported in migration/proof/ instead; see handport_BenefitDistribution_UPDATE.sql.

    // control flow: IF cond BEGIN..END [ELSE BEGIN..END] -> IF cond THEN.. [ELSE..] END IF;  (CASE..END preserved)
    body = ConvertControlFlow(body);
    // single-line IF..ELSE without BEGIN: "IF cond SET.. ELSE SET.." -> "IF cond THEN ..; ELSE ..; END IF;"
    // The condition uses the THEN-free pattern: ConvertControlFlow can leave an ALREADY-converted IF and a
    // following statement on one line ("IF c THEN SET x; ELSE SET y; END IF; RETURN z"). With a plain (.+?)
    // the condition happily swallows "...END IF;" and re-wraps the trailing RETURN, emitting a stray
    // "THEN RETURN z; END IF;" -- which is a syntax error. Refusing to cross THEN makes these rules idempotent.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        $@"(?im)^(\s*)IF\s+{COND}\s+(SET\b[^\n;]+?|RETURN\b[^\n;]+?)\s+ELSE\s+(SET\b[^\n;]+|RETURN\b[^\n;]+?)\s*;?\s*$", "$1IF $2 THEN $3; ELSE $4; END IF;");
    // single-line IF without BEGIN: "IF cond SET..|RETURN.." -> "IF cond THEN ..; END IF;"
    body = System.Text.RegularExpressions.Regex.Replace(body,
        $@"(?im)^(\s*)IF\s+{COND}\s+(SET\b[^\n;]+|RETURN\b[^\n;]+?)\s*;?\s*$", "$1IF $2 THEN $3; END IF;");
    // SELECT @x = expr FROM ...  ->  SELECT expr INTO x FROM ...  (rewrite only the prefix up to FROM,
    // so it works for MULTI-LINE statements too; the FROM/JOIN/WHERE clause is left untouched).
    // (the assignment expr must not span into a following statement: stop before any intervening SELECT)
    // Collapse multi-line CASE..END *expressions* onto one line. This MUST happen BEFORE the assignment
    // rewrites below. A T-SQL assignment whose value is a multi-line CASE --
    //     SELECT @AccVal = @BranchInComeVal + CASE
    //                                            WHEN @BranchVal > 0 THEN 0
    //                                            ELSE (@BranchVal * -1)
    //                                         END
    // -- is seen by the line-anchored "assignment with no FROM -> SET" rule as ending at "CASE", so it emitted
    // "SET v_AccVal = v_BranchInComeVal + CASE;" and orphaned the WHEN/ELSE/END. Collapsing first means those
    // rules see one complete statement. (A CASE *expression* contains no BEGIN/IF, so END unambiguously
    // closes the CASE here.)
    body = CollapseCaseExpressions(body);

    // SELECT TOP n .. -> SELECT .. LIMIT n. There was NO handling of this at all (a systematic gap). Done here,
    // before the assignment rewrites, so "SELECT TOP 1 @x = e FROM t" first loses its TOP and gains a trailing
    // LIMIT 1, then the assignment rule turns it into "SELECT e INTO x FROM t LIMIT 1".
    body = ConvertSelectTop(body);

    // Pull up a bare "SELECT" that sits ALONE on its line with the assignment on the NEXT line:
    //     SELECT
    //         @fatherparent = at.AccCode
    //     FROM AccountsTb at WHERE ...
    // The SELECT-assignment rules need "SELECT @x =" contiguous; with SELECT orphaned on its own line the
    // MID-LINE "->SET" rule (which stops at a newline) fires first and drops the FROM/WHERE, leaving the
    // stray "SET v_x = at.AccCode WHERE;". Joining SELECT to its assignment lets the SELECT..INTO..FROM rule win.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?im)^([ \t]*)SELECT[ \t]*\r?\n[ \t]*(?=[vp]_\w+\s*=[^=]|@\w+\s*=[^=])", "$1SELECT ");

    // A "SELECT @x = ISNULL((SELECT MAX(..) FROM t WHERE ..), 0)" assignment whose RHS holds a MULTI-LINE
    // parenthesised SUBQUERY: the line-anchored "no FROM -> SET" rule below matches only the FIRST physical
    // line ("SELECT @x = ISNULL((SELECT") and emits a truncated "SET v_x = IFNULL((SELECT;". Join such an
    // assignment onto ONE line first — while its parens are unbalanced (open subquery), pull up the next line —
    // so the assignment rules see the whole balanced expression. Scoped to assignment starts, so ordinary
    // multi-line SELECT lists are untouched.
    body = CollapseUnbalancedAssignment(body);

    // T-SQL MULTI-TARGET assignment written in this codebase's LEADING-COMMA style:
    //     SELECT @MovementType   = '...'
    //            ,@SafeIDMovement = dbo.GET_ReturnsSafeIDMovement(..)
    // Only the FIRST target reaches the "SELECT @x = .. (no FROM) -> SET" rule; the continuation line keeps its
    // leading comma and lands in the output as a dangling ",v_SafeIDMovement = .." — a syntax error.
    // Pull such continuation lines up onto the assignment line so the existing multi-target splitter (which
    // turns "SET a = x, b = y" into separate SETs) can see the whole list.
    //
    // The pattern is deliberately narrow — a continuation line must start with ", <variable> =" — so an ordinary
    // leading-comma SELECT COLUMN list (",a.Col" / ",dbo.fn(x) AS y", pervasive in these procs) never matches:
    // only a VARIABLE assignment can appear in that position.
    // Both spellings are accepted because the @-to-v_/p_ rename has ALREADY run by this point (so the text
    // normally reads ",v_SafeIDMovement ="), exactly as the SELECT-join rule above does.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?im)\r?\n[ \t]*(?=,[ \t]*(?:@\w+|[vp]_\w+)[ \t]*=[^=])", " ");

    // MASK CASE expressions for the duration of the assignment rewrites. A "SELECT @x = CASE .. END FROM t"
    // whose CASE contains a subquery — "WHEN EXISTS(SELECT 1 FROM CustomersTb ..)" — otherwise trips the
    // SELECT..INTO rule, whose "(?!FROM)" guard stops at the FIRST FROM (the one inside the CASE subquery),
    // never reaching the real FROM. The statement then falls through to the no-FROM "->SET" rule, which emits
    // "SET v_x = CASE..END;" and ORPHANS the real "FROM t WHERE .." (this broke Account_GetParentName and
    // every function that assigns a CASE-with-subquery). With the CASE replaced by an opaque __CASE0__ token,
    // the only FROM the rules see is the real one. Restored right after.
    var asgCases = new List<string>();
    body = MaskCaseExpressions(body, asgCases);

    // multi-target "SELECT @a=e1, @b=e2 FROM .."  ->  "SELECT e1, e2 INTO a, b FROM .."
    // MUST handle the MULTI-LINE form, which is the common one here:
    //     SELECT @AccDmType     = a.AccDmType,
    //            @CanUseBankVal = a.CanUseBankVal
    //     FROM AccountsTb a
    // Left line-anchored (the old ^..$ rule), the first line has no FROM, so it fell through to the
    // "assignment with no FROM -> SET" rule below and became "SET v_AccDmType = a.AccDmType,;" — a syntax
    // error that killed Account_GetAccVal and, through it, every proc that calls it.
    // The match deliberately ENDS at FROM and does not capture the FROM clause. Capturing it (e.g. "([^;]*?)"
    // up to the next ';') over-consumes: at this stage the body has almost no semicolons, so the first SELECT's
    // match would run to the end of the routine and swallow every later SELECT, which then never gets matched.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?is)\bSELECT\s+((?:(?!\bSELECT\b)(?!\bFROM\b)[^;])+?)\s*\bFROM\b", m =>
    {
        var parts = SplitTopLevel(m.Groups[1].Value, ',');
        // every element must be a plain "@var = expr" assignment, else this is an ordinary SELECT list
        if (parts.Count < 2 || !parts.All(p => System.Text.RegularExpressions.Regex.IsMatch(p.Trim(), @"^\w+\s*=[^=]"))) return m.Value;
        var vars = new List<string>(); var exprs = new List<string>();
        foreach (var p in parts) { int eq = p.IndexOf('='); vars.Add(p.Substring(0, eq).Trim()); exprs.Add(p.Substring(eq + 1).Trim()); }
        return "SELECT " + string.Join(", ", exprs) + " INTO " + string.Join(", ", vars) + " FROM";
    }, IC);
    // single-target "SELECT @a = <expr> FROM .." -> "SELECT <expr> INTO a FROM ..".
    // The expression may itself contain '=' — the common case is a CASE expression:
    //     SELECT @AccCreditVal = CASE WHEN @AccDmType = 1 THEN ... ELSE ... END
    //     FROM ...
    // The old pattern excluded '=' from the expression ([^;=]), so it declined to match, and the statement
    // then fell through to the "no FROM -> SET" rule and was mangled into "SET v_x = CASE;". Allow '=' inside
    // the expression; the multi-target rule above has already handled genuine comma-separated assignments.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?is)\bSELECT\s+(\w+)\s*=\s*((?:(?!\bSELECT\b)(?!\bFROM\b)[^;])+?)\s*\bFROM\b", "SELECT $2 INTO $1 FROM");
    // A multi-target assignment with NO FROM can also span lines:
    //     SELECT @MovementType   = 'بيع عملة',
    //            @SafeIDMovement = GETSafeIDMovement(...)
    // The single-line splitter below is ^..$ anchored, so it would convert only the first line and leave the
    // second orphaned ("SET v_MovementType='بيع عملة',;" then a dangling "v_SafeIDMovement = ..."). Join the
    // continuation lines first — but ONLY when the next line is itself an assignment ("<name> = <not '='>"),
    // so a genuine multi-column SELECT list is never collapsed. Fixpoint for 3+ targets.
    { string prev; do {
        prev = body;
        body = System.Text.RegularExpressions.Regex.Replace(
            body,
            @"(?im)^([ \t]*(?:SELECT|SET)\s+[\w]+\s*=[^;\r\n]*,)\s*\r?\n[ \t]*(?=[\w]+\s*=[^=])",
            "$1 ");
    } while (body != prev); }
    // assignment(s) with no FROM -> SET; split top-level comma-separated targets into separate SETs
    // (so "SELECT @a=x, @b=y" becomes "SET a=x; SET b=y;" — keeps per-expr concat fixing correct).
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)^\s*SELECT\s+(\w+\s*=\s*[^;]+?)\s*;?\s*$", m =>
    {
        var parts = SplitTopLevel(m.Groups[1].Value, ',');
        if (parts.Count > 0 && parts.All(p => System.Text.RegularExpressions.Regex.IsMatch(p.Trim(), @"^\w+\s*=")))
            return string.Join("\n", parts.Select(p => "SET " + p.Trim() + ";"));
        return "SET " + m.Groups[1].Value.Trim() + ";";
    });
    // Same assignment, but MID-LINE — the rule above is ^..$ anchored and so misses the body of a converted
    // one-liner IF, e.g.  "IF v_x = 0 THEN  select v_AccVal = 0.000  END IF;"  (T-SQL wrote the whole
    // IF/BEGIN/END on one line). Left as SELECT it is a syntax error inside a function.
    // Restricted to v_/p_ targets (real assignment) and refuses to cross FROM, so a genuine query is untouched.
    // GUARD (?!...FROM): do NOT fire when a FROM clause is on the NEXT line — that is a real
    // "SELECT @x = col FROM t WHERE .." query whose SELECT..INTO..FROM rewrite above should have won; firing
    // here would drop its FROM/WHERE and leave the stray "SET v_x = col WHERE;" (broke ExpensesTb_Insert).
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\bSELECT\s+([vp]_\w+)\s*=\s*((?:(?!\bFROM\b)[^;\r\n])+?)\s*(?=;|\bEND\b|\r|\n|$)(?!\s*\r?\n\s*FROM\b)",
        "SET $1 = $2;", IC);
    // With that guard, a "SELECT @x = col \n FROM t WHERE .." that the INTO rule missed is left as a plain
    // SELECT..FROM — so convert the assignment form to INTO here too (multi-line, FROM on a later line).
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?is)\bSELECT\s+([vp]_\w+)\s*=\s*((?:(?!\bSELECT\b)(?!\bFROM\b)[^;])+?)\s*\bFROM\b",
        "SELECT $2 INTO $1 FROM", IC);
    body = UnmaskCaseExpressions(body, asgCases);   // restore the CASE expressions masked above
    // T-SQL compound assignment "x += v" / "x -= v" -> "x = x + v" — general (any column/var, so it also
    // fixes the 2nd+ targets in a multi-column "UPDATE t SET a -= 1, b -= 2").
    body = System.Text.RegularExpressions.Regex.Replace(body, @"([\w`.]+)\s*\+=\s*", "$1 = $1 + ", IC);
    body = System.Text.RegularExpressions.Regex.Replace(body, @"([\w`.]+)\s*-=\s*", "$1 = $1 - ", IC);
    // T-SQL '+' string concat -> CONCAT (only when an operand is a string literal or CAST(.. AS CHAR);
    // numeric '+' is left untouched). Applied to SET right-hand sides.
    // (matches SET assignments anywhere, incl. inside "IF .. THEN SET x=..;" — RHS ends at the first ';')
    // The RHS must stop at the statement's own end. This pass runs BEFORE the boundary pass inserts ';', and
    // the legacy code omits ';' between statements, so a plain "[^;]+?;" terminator ran PAST the intended
    // statement to the next ';' far away. A numeric "SET @currentRow = @currentRow + 1" then swallowed the
    // following "SET @CodeID = .. + ' - ' + .." — whose string literals made the merged text look "stringy",
    // and FixConcat wrapped BOTH statements into one broken CONCAT (unbalanced parens). Stop the RHS at ';' OR
    // the next top-level statement keyword on a new line (terminator is a LOOKAHEAD, so ';' stays in place).
    // ^[ \t]* line-anchor: match ONLY a standalone "SET x = .." statement, never an UPDATE's SET clause
    // ("UPDATE users SET Reg='NO', Count_ACtivties += 1 WHERE id=@ID" — that line starts with UPDATE, so it is
    // skipped). Without the anchor, "SET Reg =" inside the UPDATE matched and FixConcat swallowed the following
    // ", Count_ACtivties = .. WHERE id=.." into one broken CONCAT.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?im)^([ \t]*SET\s+\w+\s*=\s*)((?:(?!\r?\n\s*(?:SET|SELECT|INSERT|UPDATE|DELETE|IF|ELSEIF|ELSE|WHILE|END|BEGIN|DECLARE|RETURN|COMMIT|ROLLBACK|CALL|EXEC)\b)[^;])+?)\s*(?=;|\r?\n\s*(?:SET|SELECT|INSERT|UPDATE|DELETE|IF|ELSEIF|ELSE|WHILE|END|BEGIN|DECLARE|RETURN|COMMIT|ROLLBACK|CALL|EXEC)\b|$)",
        m => m.Groups[1].Value + FixConcat(m.Groups[2].Value));
    // T-SQL select-list column alias "name = expr" -> "expr AS name". This is applied by
    // FixSelectListAliasAssign, which walks the body statement-by-statement and only rewrites the region
    // BETWEEN a top-level SELECT and its FROM — never inside an UPDATE..SET, where "col = expr, col = expr"
    // has the identical shape but the opposite meaning. (A naive whole-body regex flipped UPDATE SET
    // assignments into "expr AS col" and broke 60+ procs — hence the scoped walker.)
    //
    // It runs UNCONDITIONALLY, not only on the applyConcat retry, because "SELECT x = 'y'" is VALID MySQL
    // syntax (a boolean comparison): the proc CREATEs fine and the retry never fires — it fails only at RUN
    // TIME with "Unknown column 'x' in 'field list'" (InternalEx_LOADTOCONFIRM hit exactly this).
    body = FixSelectListAliasAssign(body, IC);

    // Retry-only fixes (applyConcat=true): applied ONLY when re-converting an already-failing object,
    // so they cannot regress a passing one. Whole-body '+'->CONCAT regressed passes 3× when applied broadly.
    if (applyConcat)
    {
        // common search pattern: LIKE '%' + @x + '%'  (T-SQL concat) -> LIKE CONCAT('%', x, '%')  [before FixConcatInText]
        body = System.Text.RegularExpressions.Regex.Replace(body,
            @"(?i)\bLIKE\s*\+?\s*'%'\s*\+\s*([\w.`]+)\s*\+\s*'%'", "LIKE CONCAT('%', $1, '%')");
        body = FixConcatInText(body);
        // multi-target assignment collapsed by the main path into "SET a = x, b = y" -> split into separate SETs
        body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)^[ \t]*SET\s+(\w+\s*=\s*.+)$", m =>
        {
            var parts = SplitTopLevel(m.Groups[1].Value.TrimEnd(';'), ',');
            if (parts.Count < 2 || !parts.All(p => System.Text.RegularExpressions.Regex.IsMatch(p.Trim(), @"^\w+\s*="))) return m.Value;
            return string.Join(" ", parts.Select(p => "SET " + p.Trim() + ";"));
        });
    }
    // DECLARE x t = v  ->  DECLARE x t DEFAULT v. The type may carry a parenthesised precision WITH SPACES,
    // e.g. "DECLARE v_AccVal DECIMAL(18, 3) = 0" — so the type group must allow "( .. )" including the inner
    // space/comma, otherwise the "= 0" survives as an illegal "DECIMAL(18, 3) = 0".
    body = System.Text.RegularExpressions.Regex.Replace(body, @"(?im)\bDECLARE\s+(\w+)\s+(\w+(?:\s*\([^)]*\))?)\s*=\s*", "DECLARE $1 $2 DEFAULT ");
    // ';' at statement boundaries (next statement keyword / END) — ONLY at paren-depth 0, so a line-leading
    // SELECT inside a subquery (update set x=( select.. )) is NOT mistaken for a new statement.
    // ELSEIF must be listed BEFORE ELSE: "^ELSE\b" does not match "ELSEIF" (no word boundary after ELSE), so
    // without an explicit ELSEIF alternative the statement inside the preceding THEN branch never gets its ';'
    // terminator and MySQL fails with "near 'ELSEIF'". END IF / END WHILE likewise close a block.
    var boundary = new System.Text.RegularExpressions.Regex(@"^(WITH|DECLARE|SET|SELECT|INSERT|UPDATE|DELETE|RETURN|WHILE|IF|ELSEIF|ELSE|END\s+IF|END\s+WHILE|COMMIT|ROLLBACK|START\s+TRANSACTION|CALL|SIGNAL|LEAVE|TRUNCATE|DROP|OPEN|CLOSE|FETCH)\b", IC);
    var outl = new List<string>(); int depth = 0; string curKw = ""; bool sawSet = false; bool sawValues = false; bool sawInsertSelect = false;
    foreach (var raw in body.Split('\n'))
    {
        var line = raw.TrimEnd();
        var t = line.Trim();
        bool atTop = depth == 0 && t.Length > 0;
        bool startsSet = System.Text.RegularExpressions.Regex.IsMatch(t, @"^SET\b", IC);
        bool startsSelect = System.Text.RegularExpressions.Regex.IsMatch(t, @"^SELECT\b", IC);
        // A SET line that belongs to an open UPDATE (its SET clause), or a SELECT that is an INSERT's source,
        // is a clause continuation — NOT a new statement. curKw tracks the current statement's lead keyword
        // even when UPDATE/INSERT and their clause sit on separate lines.
        // A SELECT that is the main query of a CTE ("WITH cte AS (..) SELECT ..") is a CONTINUATION of the
        // WITH statement, not a new statement — MySQL parses "WITH (..) SELECT" as one unit. Inserting a ';'
        // after the CTE's closing ')' breaks it. curKw stays "WITH" until that SELECT is consumed.
        // A SELECT after an INSERT is a continuation ONLY for "INSERT .. SELECT ..". If the INSERT already had
        // a VALUES clause ("INSERT .. (cols) VALUES (..)"), the following SELECT is a SEPARATE statement (the
        // common "return the inserted row" pattern) and MUST get its ';' — otherwise MySQL sees
        // "VALUES (..) SELECT .." and fails. sawValues distinguishes the two.
        // An "INSERT .. (cols) SELECT <values>" whose source SELECT has NO FROM can be followed by ANOTHER
        // top-level SELECT (e.g. "SELECT COUNT(..) INTO v"). Only the FIRST SELECT is the INSERT's source; the
        // second is a new statement and MUST get its ';'. sawInsertSelect flips true once the source SELECT is
        // consumed so the next SELECT is treated as a boundary rather than a second continuation.
        bool clauseCont = atTop && ((startsSet && curKw == "UPDATE" && !sawSet)
                                    || (startsSelect && curKw == "INSERT" && !sawValues && !sawInsertSelect)
                                    || (startsSelect && curKw == "WITH"));
        // "^END\b" catches bare END, "END IF", "END WHILE" AND "END;" — the legacy code writes the outer block
        // close as "END;" (semicolon already attached). The old t.Equals("END")/StartsWith("END ") checks missed
        // "END;", so the last statement before it never got its ';' terminator ("near 'END'").
        // A block-opening "BEGIN" on its own line also ends the statement before it. The legacy code writes
        //     DECLARE @FATHERPERINT decimal(18,0)
        //     BEGIN  ... END
        // and without this the DECLARE never gets its ';' ("near 'BEGIN'").
        bool isBoundary = atTop && !clauseCont && (boundary.IsMatch(t) ||
            System.Text.RegularExpressions.Regex.IsMatch(t, @"^(END|BEGIN)\b", IC));
        if (isBoundary)
            for (int i = outl.Count - 1; i >= 0; i--)
            {
                var pt = outl[i].Trim();
                if (pt.Length == 0) continue;
                // Lines that OPEN a block must never be terminated: "DECLARE .. HANDLER FOR SQLEXCEPTION" and
                // "WHILE .. DO" are both legally followed by a bare BEGIN, and a ';' there is a syntax error.
                bool opensBlock = EndsKw(pt, "BEGIN") || EndsKw(pt, "THEN") || EndsKw(pt, "ELSE") || EndsKw(pt, "DO")
                    || System.Text.RegularExpressions.Regex.IsMatch(pt, @"\bHANDLER\s+FOR\b", IC);
                if (!pt.EndsWith(";") && !opensBlock
                    && !EndsKw(pt, "UNION") && !EndsKw(pt, "ALL") && !EndsKw(pt, "EXCEPT") && !EndsKw(pt, "INTERSECT"))
                    outl[i] += ";";
                break;
            }
        if (isBoundary)
        {
            var mkw = System.Text.RegularExpressions.Regex.Match(t, @"^(WITH|DECLARE|SET|SELECT|INSERT|UPDATE|DELETE|RETURN|WHILE|IF|ELSE)\b", IC);
            curKw = mkw.Success ? mkw.Groups[1].Value.ToUpperInvariant() : "END";
            sawSet = false;
            sawValues = false;
            sawInsertSelect = false;
        }
        if (atTop && startsSet && curKw == "UPDATE") sawSet = true;
        // the INSERT's source SELECT has now been seen — any further top-level SELECT starts a new statement
        if (clauseCont && startsSelect && curKw == "INSERT") sawInsertSelect = true;
        // a VALUES clause anywhere in the current INSERT marks it as a values-insert (not INSERT..SELECT)
        if (curKw == "INSERT" && System.Text.RegularExpressions.Regex.IsMatch(t, @"\bVALUES\b", IC)) sawValues = true;
        outl.Add(line);
        depth += line.Count(ch => ch == '(') - line.Count(ch => ch == ')');
    }
    var joined = string.Join("\n", outl);
    // Safety net: several rewrites above can leave an empty statement (";;" or ";\n;"). MySQL treats an empty
    // statement as a syntax error, so collapse runs of semicolons into one.
    joined = System.Text.RegularExpressions.Regex.Replace(joined, @";(\s*;)+", ";");
    // Must run AFTER the ';'-insertion pass above: it splits the body on top-level ';' to find statements.
    joined = FixMultiRowSelectInto(joined);
    return HoistDeclares(joined);
}

// MySQL requires all DECLAREs at the top of the block. Move single-line DECLARE statements to just after
// the leading BEGIN (or to the very top if the body has no leading BEGIN — the proc wrapper adds one).
static string HoistDeclares(string body)
{
    var lines = body.Split('\n').ToList();
    var declares = new List<string>(); var handlers = new List<string>(); var rest = new List<string>();
    foreach (var l in lines)
    {
        var lt = l.Trim();
        if (System.Text.RegularExpressions.Regex.IsMatch(lt, @"^DECLARE\b.*;\s*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            // MySQL requires the strict order: variable/condition DECLAREs, then cursors, then HANDLERs.
            if (System.Text.RegularExpressions.Regex.IsMatch(lt, @"^DECLARE\s+(EXIT|CONTINUE|UNDO)\s+HANDLER\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                handlers.Add(lt);
            else declares.Add(lt);
        }
        else rest.Add(l);
    }
    if (declares.Count == 0 && handlers.Count == 0) return body;
    declares.AddRange(handlers);   // variables first, handlers last
    int insertAt = 0;
    for (int i = 0; i < rest.Count; i++)
    {
        if (rest[i].Trim().Length == 0) continue;
        if (System.Text.RegularExpressions.Regex.IsMatch(rest[i].Trim(), @"^BEGIN\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) insertAt = i + 1;
        break;
    }
    rest.InsertRange(insertAt, declares);
    return string.Join("\n", rest);
}

// Parse "@name TYPE[(...)] = <default>" pairs from a proc PARAMETER HEADER (the text before the body "AS"),
// returning only the params whose default is NON-NULL (those are the ones lost in the MySQL port — see the
// call site in ConvertProcDdl). The type-length "(...)" is optional and consumed so "@x DECIMAL(18,2)=0" works;
// the default is captured up to the next comma/newline and its trailing ')' (the param-list close paren) trimmed.
static IEnumerable<(string Name, string Default)> ParseNonNullParamDefaults(string header)
{
    foreach (System.Text.RegularExpressions.Match pm in System.Text.RegularExpressions.Regex.Matches(
        header, @"@(\w+)\s+[A-Za-z]\w*(?:\s*\([^)]*\))?\s*=\s*([^,\r\n]+)",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase))
    {
        var name = pm.Groups[1].Value;
        var dflt = pm.Groups[2].Value.Trim().TrimEnd(')').Trim();
        if (dflt.Length == 0 || dflt.Equals("NULL", StringComparison.OrdinalIgnoreCase)) continue;
        yield return (name, dflt);
    }
}

// T-SQL bare "BEGIN ... END" is pure grouping — T-SQL blocks are NOT a variable scope. When such a block
// LEADS the body and is followed by MORE statements (e.g. ACCOUNTSTB_selectmax: a BEGIN...END holding the
// DECLAREs + first assignment, then IF branches that USE those vars, then the final SELECT), the wrapper
// below mistook that inner block for the whole proc body: it left the trailing statements OUTSIDE the
// compound (MySQL dropped them at CREATE, so the proc returned nothing) and would have block-scoped the
// DECLAREs away from their later use. Flatten it — remove the leading BEGIN and its matching END — so the
// DECLAREs hoist to proc scope and the trailing statements stay in the body. Fires ONLY when real content
// follows the matching END, so the common "AS BEGIN <all> END" procs are left byte-for-byte untouched.
static string FlattenLeadingGroupingBegin(string body)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    int i = 0;
    while (i < body.Length)
    {
        while (i < body.Length && char.IsWhiteSpace(body[i])) i++;
        var mset = System.Text.RegularExpressions.Regex.Match(body.Substring(i), @"^SET\s+(NOCOUNT|XACT_ABORT)\b[^\r\n]*", IC);
        if (mset.Success) { i += mset.Length; continue; }
        break;
    }
    var mb = System.Text.RegularExpressions.Regex.Match(body.Substring(i), @"^BEGIN\b(?!\s+(TRAN|TRANSACTION|TRY|CATCH))", IC);
    if (!mb.Success) return body;
    int beginKwStart = i, scan = i + mb.Length, depth = 1, matchEndStart = -1, matchEndEnd = -1;
    foreach (System.Text.RegularExpressions.Match tk in System.Text.RegularExpressions.Regex.Matches(
        body.Substring(scan), @"\bBEGIN\b|\bCASE\b|\bEND\b", IC))
    {
        var w = tk.Value.ToUpperInvariant();
        if (w == "BEGIN" || w == "CASE") depth++;
        else { depth--; if (depth == 0) { matchEndStart = scan + tk.Index; matchEndEnd = matchEndStart + tk.Length; break; } }
    }
    if (matchEndStart < 0) return body;                                   // unbalanced — leave it to the normal path
    if (!System.Text.RegularExpressions.Regex.IsMatch(body.Substring(matchEndEnd),
            @"\b(IF|SELECT|UPDATE|INSERT|DELETE|SET|WHILE|EXEC|EXECUTE|DECLARE|CALL|BEGIN)\b", IC))
        return body;                                                      // block IS the whole body — common case, untouched
    body = body.Remove(matchEndStart, matchEndEnd - matchEndStart);       // drop matching END first (keeps earlier indices valid)
    body = body.Remove(beginKwStart, mb.Length);                          // then drop the leading BEGIN
    return body;
}

static bool EndsKw(string s, string kw) => s.EndsWith(kw, StringComparison.OrdinalIgnoreCase);

// General '+'-concat fixer for whole text (views, SELECT lists): finds chains of simple operands
// (identifiers / `a`.`b` / 'literals') joined by '+', and if a string literal is present, wraps in CONCAT.
// Numeric '+' (no string literal in the chain) is left untouched. Operands with parens (CAST/func) are
// handled by FixConcat on SET right-hand sides instead.
static string FixConcatInText(string s) =>
    System.Text.RegularExpressions.Regex.Replace(s,
        @"(?:'[^']*'|[\w`.]+)(?:\s*\+\s*(?:'[^']*'|[\w`.]+))+",
        m =>
        {
            var parts = SplitTopLevel(m.Value, '+');
            if (parts.Count < 2 || !parts.Any(p => p.Contains('\''))) return m.Value;
            return "CONCAT(" + string.Join(", ", parts.Select(p => p.Trim())) + ")";
        });

// Convert a T-SQL '+' string-concat expression to MySQL CONCAT(...). Only fires when an operand is a
// string literal or CAST(.. AS CHAR) — numeric '+' is preserved. Paren/quote-aware top-level split.
static string FixConcat(string expr)
{
    var parts = SplitTopLevel(expr, '+');
    if (parts.Count < 2) return expr;
    bool stringy = parts.Any(p => p.Contains('\'') ||
        System.Text.RegularExpressions.Regex.IsMatch(p, @"CAST\s*\(.*\bAS\s+CHAR", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
    if (!stringy) return expr;                                  // numeric addition -> leave alone
    return "CONCAT(" + string.Join(", ", parts.Select(p => p.Trim())) + ")";
}

// Split an expression on a top-level operator char, ignoring occurrences inside parens or '...' strings.
static List<string> SplitTopLevel(string expr, char op)
{
    var parts = new List<string>(); int depth = 0; bool inStr = false; int start = 0;
    for (int i = 0; i < expr.Length; i++)
    {
        char ch = expr[i];
        if (ch == '\'') inStr = !inStr;
        else if (!inStr && ch == '(') depth++;
        else if (!inStr && ch == ')') depth--;
        else if (!inStr && depth == 0 && ch == op) { parts.Add(expr[start..i]); start = i + 1; }
    }
    parts.Add(expr[start..]);
    return parts;
}

// Join a "SELECT/SET <var> = <expr>" assignment that opens a parenthesised SUBQUERY spanning multiple lines
// (e.g. "= ISNULL((SELECT MAX(x)\n FROM t\n WHERE ..), 0)") onto a single physical line, so the line-anchored
// assignment rewrites see the whole balanced RHS instead of truncating at the first newline. Only assignment
// lines with a NET-OPEN paren count pull up following lines, and only until the parens balance — an ordinary
// multi-line SELECT column list (no leading "<var> =") is never touched.
static string CollapseUnbalancedAssignment(string body)
{
    var lines = body.Split('\n');
    var outp = new List<string>();
    int NetParens(string s)
    {
        int d = 0; bool q = false;
        foreach (var ch in s) { if (ch == '\'') q = !q; else if (!q && ch == '(') d++; else if (!q && ch == ')') d--; }
        return d;
    }
    for (int i = 0; i < lines.Length; i++)
    {
        var cur = lines[i];
        bool isAssign = System.Text.RegularExpressions.Regex.IsMatch(
            cur, @"^\s*(?:SELECT|SET)\s+@?\w+\s*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (isAssign)
        {
            // pull up following lines while the accumulated expression has more '(' than ')'
            while (NetParens(cur) > 0 && i + 1 < lines.Length)
                cur = cur.TrimEnd() + " " + lines[++i].Trim();
        }
        outp.Add(cur);
    }
    return string.Join("\n", outp);
}

// Join multi-line CASE..END expressions into a single line. Inc on CASE, dec on END (a CASE expression
// contains no BEGIN/IF, so the first END at depth>0 closes the innermost CASE). Newlines inside become spaces.
static string CollapseCaseExpressions(string body)
{
    var toks = System.Text.RegularExpressions.Regex.Matches(body, @"\w+|\s+|[^\w\s]").Select(m => m.Value).ToList();
    var sb = new System.Text.StringBuilder(); int caseDepth = 0;
    bool W(string t, string k) => t.Equals(k, StringComparison.OrdinalIgnoreCase);
    foreach (var t in toks)
    {
        if (W(t, "CASE")) caseDepth++;
        else if (W(t, "END") && caseDepth > 0) caseDepth--;
        if (caseDepth > 0 && (t.Contains('\n') || t.Contains('\r'))) sb.Append(t.Replace('\r', ' ').Replace('\n', ' '));
        else sb.Append(t);
    }
    return sb.ToString();
}

// Block-structure-aware T-SQL IF/BEGIN/END -> MySQL IF/THEN/END IF (handles ELSE; leaves CASE..END and
// plain BEGIN..END / WHILE..BEGIN intact). Stack scan, not regex — so nesting is balanced.
static string ConvertControlFlow(string body)
{
    var tokens = System.Text.RegularExpressions.Regex.Matches(body, @"\w+|\s+|[^\w\s]").Select(m => m.Value).ToList();
    var outp = new List<string>();
    var stack = new Stack<char>();   // 'I'=IF block, 'P'=plain BEGIN, 'C'=CASE
    bool W(string tk, string kw) => tk.Equals(kw, StringComparison.OrdinalIgnoreCase);
    int NextSig(int from) { int k = from; while (k < tokens.Count && string.IsNullOrWhiteSpace(tokens[k])) k++; return k; }

    int i = 0;
    while (i < tokens.Count)
    {
        var tk = tokens[i];
        if (W(tk, "IF"))
        {
            var cond = new List<string> { tk }; int j = i + 1; bool found = false;
            while (j < tokens.Count)
            {
                if (W(tokens[j], "BEGIN")) { found = true; break; }
                if (W(tokens[j], "END") || tokens[j] == ";") break;   // no-BEGIN IF -> leave for manual
                cond.Add(tokens[j]); j++;
            }
            if (found) { outp.AddRange(cond); outp.Add(" THEN "); stack.Push('I'); i = j + 1; continue; }
            outp.Add(tk); i++; continue;
        }
        // WHILE cond BEGIN .. END  ->  WHILE cond DO .. END WHILE;   (same shape as the IF handling above:
        // scan forward to the block-opening BEGIN, emit the MySQL keyword, and remember the block kind so the
        // matching END closes it correctly.) Without this the WHILE kept its T-SQL shape and MySQL rejected it.
        if (W(tk, "WHILE"))
        {
            var wcond = new List<string> { tk }; int wj = i + 1; bool wfound = false;
            while (wj < tokens.Count)
            {
                if (W(tokens[wj], "BEGIN")) { wfound = true; break; }
                if (W(tokens[wj], "END") || tokens[wj] == ";") break;   // no-BEGIN WHILE -> leave for manual
                wcond.Add(tokens[wj]); wj++;
            }
            if (wfound) { outp.AddRange(wcond); outp.Add(" DO "); stack.Push('W'); i = wj + 1; continue; }
            outp.Add(tk); i++; continue;
        }
        if (W(tk, "CASE")) { outp.Add(tk); stack.Push('C'); i++; continue; }
        if (W(tk, "BEGIN")) { outp.Add("BEGIN"); stack.Push('P'); i++; continue; }
        if (W(tk, "END"))
        {
            int k = NextSig(i + 1);
            if (stack.Count > 0 && stack.Peek() == 'I' && k < tokens.Count && W(tokens[k], "ELSE"))
            {
                int b = NextSig(k + 1);
                if (b < tokens.Count && W(tokens[b], "BEGIN")) { outp.Add(" ELSE "); i = b + 1; continue; }
                if (b < tokens.Count && W(tokens[b], "IF"))
                {
                    // ELSE IF cond BEGIN.. -> ELSEIF cond THEN.. (reuse current 'I' block; END chains naturally)
                    var cond2 = new List<string>(); int j2 = b + 1; bool f2 = false;
                    while (j2 < tokens.Count) { if (W(tokens[j2], "BEGIN")) { f2 = true; break; } if (W(tokens[j2], "END") || tokens[j2] == ";") break; cond2.Add(tokens[j2]); j2++; }
                    if (f2) { outp.Add(" ELSEIF "); outp.AddRange(cond2); outp.Add(" THEN "); i = j2 + 1; continue; }
                    outp.Add(" ELSE "); i = k + 1; continue;   // else-if w/o BEGIN: leave (was failing anyway)
                }
                // ELSE <single statement> (T-SQL ELSE binds exactly one statement) -> ELSE stmt; END IF;
                outp.Add(" ELSE ");
                int p = k + 1;
                while (p < tokens.Count && tokens[p] != ";" && !W(tokens[p], "END") && !W(tokens[p], "BEGIN")) { outp.Add(tokens[p]); p++; }
                if (p < tokens.Count && tokens[p] == ";") { outp.Add(";"); p++; }
                outp.Add(" END IF;");
                stack.Pop();
                i = p; continue;
            }
            char m = stack.Count > 0 ? stack.Pop() : 'P';
            outp.Add(m == 'I' ? "END IF;" : m == 'W' ? "END WHILE;" : "END");
            i++; continue;
        }
        outp.Add(tk); i++;
    }
    return string.Join("", outp);
}

// ---------------- simple procedures (Phase 2, mechanical) ----------------
void DoProcs(Cfg c)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET NAMES utf8mb4;");
    // README_MYSQL §7 (error 1267): SET NAMES alone leaves the CONNECTION collation at utf8mb4_general_ci
    // while every column is utf8mb4_unicode_ci. MySQL BAKES the session collation into a routine at CREATE
    // time, so without this the routine hits "Illegal mix of collations" at runtime on any UNION or string
    // comparison. Must be set on the CREATING connection, not just in the emitted .sql header.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");

    // mechanical procs only: <=5KB, no transaction/try/cursor/dynamic-SQL/TVP
    var procs = new List<(string Name, string Def)>();
    using (var cmd2 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.type='P' AND o.name NOT LIKE 'Private_%' AND m.definition NOT LIKE '%tsqlt%'
          AND o.name NOT LIKE 'sp_%diagram%' AND o.name NOT LIKE 'sp_upgraddiagrams' AND o.name NOT LIKE 'fn_diagramobjects'
          AND LEN(m.definition)<=5000
          AND m.definition NOT LIKE '%TRAN%' AND m.definition NOT LIKE '%TRY%' AND m.definition NOT LIKE '%CURSOR%'
          AND m.definition NOT LIKE '%sp_executesql%' AND m.definition NOT LIKE '%EXEC(%' AND m.definition NOT LIKE '%READONLY%'
        ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) procs.Add((r.GetString(0), r.GetString(1)));

    var ddlMap = new Dictionary<string, string>(); var paramsMap = new Dictionary<string, List<Prm>>();
    foreach (var (name, def) in procs)
    {
        var ps = LoadProcParams(src, name);
        paramsMap[name] = ps;
        ddlMap[name] = ConvertProcDdl(name, def, ps);
    }

    var procDef = procs.ToDictionary(p => p.Name, p => p.Def);
    var pending = procs.Select(p => p.Name).ToList(); var ok = new List<string>(); var err = new Dictionary<string, string>();
    for (int passNo = 0; passNo < 3 && pending.Count > 0; passNo++)
    {
        var still = new List<string>();
        foreach (var name in pending)
        {
            try { Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddlMap[name]); ok.Add(name); }
            catch
            {
                // retry CREATE with the safe extra fixes (concat + select-list "name = CASE" alias) — only on an
                // already-failing proc, so it cannot regress one that created cleanly.
                try
                {
                    var ddl2 = ConvertProcDdl(name, procDef[name], paramsMap[name], applyConcat: true);
                    Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddl2);
                    ddlMap[name] = ddl2; ok.Add(name);
                }
                catch (Exception ex2) { err[name] = OneLine(ex2.Message); still.Add(name); }
            }
        }
        if (still.Count == pending.Count) break;
        pending = still;
    }

    // diff-test: call with sample args, compare result sets (multiset) + OUT params
    int pass = 0; var diffFail = new List<string>(); var untested = new List<string>(); var passed = new List<string>(); var srcBroken = new List<string>();
    foreach (var name in ok)
    {
        var ps = paramsMap[name];
        if (ps.Any(p => MapType(p.Col).Contains("BLOB"))) { untested.Add($"{name}: binary param"); continue; }
        List<string> ra; Dictionary<string, string> ra_out;
        try { (ra, ra_out) = CallProcSql(src, name, ps); }
        catch (Exception sx)
        {
            // SQL Server ORIGINAL errored -> proc is broken in the source (e.g. references a dropped table).
            // If MySQL errors too, the conversion is faithful (both fail identically) — not a conversion bug.
            bool myOk = true; try { CallProcMy(dst, name, ps); } catch { myOk = false; }
            if (!myOk) srcBroken.Add($"{name}: source error ({OneLine(sx.Message)})");
            else diffFail.Add($"{name}: SQL-source errors but MySQL ran");
            continue;
        }
        // first attempt
        bool matched = false; string detail = "";
        try { var (rb, rb_out) = CallProcMy(dst, name, ps); matched = RowsEqual(ra, rb) && OutEqual(ra_out, rb_out); detail = $"rows({ra.Count}/{rb.Count}) or OUT"; }
        catch (Exception ex) { detail = "MySQL " + OneLine(ex.Message); }
        if (matched) { pass++; passed.Add(name); continue; }
        // RETRY with extra fixes (concat + select-list alias) — fires on mismatch OR MySQL error. Safe: only failing procs.
        try
        {
            var ddl2 = ConvertProcDdl(name, procDef[name], ps, applyConcat: true);
            Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddl2);
            var (rb2, rb2_out) = CallProcMy(dst, name, ps);
            if (RowsEqual(ra, rb2) && OutEqual(ra_out, rb2_out)) { pass++; passed.Add(name); ddlMap[name] = ddl2; continue; }
        }
        catch { }
        diffFail.Add($"{name}: {detail} differ");
    }
    if (srcBroken.Count > 0) File.WriteAllText("procs_source_broken.txt",
        "Procs whose SQL Server ORIGINAL also errors (broken source — reference dropped tables/cols). Conversion is\n" +
        "faithful (both engines fail identically); NOT conversion bugs and NOT counted as failures.\n\n" + string.Join("\n", srcBroken), new UTF8Encoding(false));
    WriteVerified("converted_verified_procs.sql", "PROCEDURE", passed.Select(n => ddlMap[n]));

    var createFail = pending.Select(n => $"{n}: {err.GetValueOrDefault(n, "?")}").ToList();
    File.WriteAllText("procs_needs_manual.txt",
        "SIMPLE PROCS NEEDING MANUAL WORK\n\n== failed to CREATE ==\n" + string.Join("\n", createFail) +
        "\n\n== created but diff FAILED ==\n" + string.Join("\n", diffFail) +
        "\n\n== created, not auto-tested ==\n" + string.Join("\n", untested), new UTF8Encoding(false));

    Console.WriteLine($"Simple procs: {procs.Count} total | created {ok.Count} | diff PASS {pass} | not-tested {untested.Count} | source-broken {srcBroken.Count} | real diff-fail {diffFail.Count}");
    Console.WriteLine($"  (create-fail {createFail.Count}, real diff-fail {diffFail.Count}, source-broken {srcBroken.Count}) -> procs_needs_manual.txt / procs_source_broken.txt");
}

// PHASE 3 — transactional (TRY/CATCH + TRAN) procs. CONVERT + CREATE ONLY (no execution -> no data
// mutation). These write data, so behavioral diff-testing needs curated inputs and is done separately.
// Output is kept apart from the diff-VERIFIED artifacts and clearly labelled as pending verification.
void DoHardProcs(Cfg c)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET NAMES utf8mb4;");
    // README_MYSQL §7 (error 1267): SET NAMES alone leaves the CONNECTION collation at utf8mb4_general_ci
    // while every column is utf8mb4_unicode_ci. MySQL BAKES the session collation into a routine at CREATE
    // time, so without this the routine hits "Illegal mix of collations" at runtime on any UNION or string
    // comparison. Must be set on the CREATING connection, not just in the emitted .sql header.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");

    var procs = new List<(string Name, string Def)>();
    // transactional OR large procs (the HARD + large-MANUAL buckets), excluding TVP/cursor/dynamic-SQL
    using (var cmd2 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.type='P' AND o.name NOT LIKE 'Private_%' AND m.definition NOT LIKE '%tsqlt%'
          AND (LEN(m.definition)>5000 OR m.definition LIKE '%TRAN%' OR m.definition LIKE '%TRY%')
          AND m.definition NOT LIKE '%CURSOR%' AND m.definition NOT LIKE '%sp_executesql%'
          AND m.definition NOT LIKE '%EXEC(%' AND m.definition NOT LIKE '%READONLY%'
        ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) procs.Add((r.GetString(0), r.GetString(1)));

    var ddlMap = new Dictionary<string, string>(); var paramsMap = new Dictionary<string, List<Prm>>();
    var procDef = procs.ToDictionary(p => p.Name, p => p.Def);
    foreach (var (name, def) in procs) { var ps = LoadProcParams(src, name); paramsMap[name] = ps; ddlMap[name] = ConvertProcDdl(name, def, ps); }

    var pending = procs.Select(p => p.Name).ToList(); var ok = new List<string>(); var err = new Dictionary<string, string>();
    for (int passNo = 0; passNo < 3 && pending.Count > 0; passNo++)
    {
        var still = new List<string>();
        foreach (var name in pending)
        {
            try { Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddlMap[name]); ok.Add(name); }
            catch
            {
                // retry CREATE with the safe extra fixes (concat + select-list "name = CASE" alias) — failing procs only
                try
                {
                    var ddl2 = ConvertProcDdl(name, procDef[name], paramsMap[name], applyConcat: true);
                    Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddl2);
                    ddlMap[name] = ddl2; ok.Add(name);
                }
                catch (Exception ex2) { err[name] = OneLine(ex2.Message); still.Add(name); }
            }
        }
        if (still.Count == pending.Count) break;
        pending = still;
    }

    var createFail = pending.Select(n => $"{n}: {err.GetValueOrDefault(n, "?")}").ToList();
    File.WriteAllText("hardprocs_needs_manual.txt", "PHASE 3 transactional procs that FAILED to convert+create:\n\n" + string.Join("\n", createFail), new UTF8Encoding(false));
    WriteVerified("converted_PENDING_hardprocs.sql", "PROCEDURE", ok.Select(n => ddlMap[n]));
    Console.WriteLine($"Phase-3 transactional procs: {procs.Count} total | converted+created {ok.Count} | create-fail {createFail.Count}");
    Console.WriteLine($"  NOTE: created only (syntactic). NOT behaviorally diff-verified — they write data; needs curated inputs.");
}

// TVP procs: table-valued-parameter procedures. MySQL has no TVP, so the converter strips the TVP param from
// the signature and rewrites "@tvp" body references to a session TEMPORARY TABLE "tvp_<name>" that the data
// layer (MD_CONNECTION_MYSQL) populates from the passed DataTable before CALLing. This step converts + creates
// them (create only — runtime needs the staged temp table; the data layer / a verify harness provides it).
void DoTvpProcs(Cfg c)
{
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET NAMES utf8mb4;");
    // README_MYSQL §7 (error 1267): SET NAMES alone leaves the CONNECTION collation at utf8mb4_general_ci
    // while every column is utf8mb4_unicode_ci. MySQL BAKES the session collation into a routine at CREATE
    // time, so without this the routine hits "Illegal mix of collations" at runtime on any UNION or string
    // comparison. Must be set on the CREATING connection, not just in the emitted .sql header.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");

    var procs = new List<(string Name, string Def)>();
    using (var cmd2 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.type='P' AND o.name NOT LIKE 'Private_%' AND m.definition NOT LIKE '%tsqlt%'
          AND m.definition LIKE '%READONLY%' AND m.definition NOT LIKE '%CURSOR%'
        ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) procs.Add((r.GetString(0), r.GetString(1)));

    // emit the staging-table DDL (CREATE TEMPORARY TABLE tvp_<typecol-shape>) for reference / the verify harness
    var typeCols = new Dictionary<string, List<(string Col, string Type)>>();

    var ddlMap = new Dictionary<string, string>(); var paramsMap = new Dictionary<string, List<Prm>>();
    var procDef = procs.ToDictionary(p => p.Name, p => p.Def);
    foreach (var (name, def) in procs) { var ps = LoadProcParams(src, name); paramsMap[name] = ps; ddlMap[name] = ConvertProcDdl(name, def, ps); }

    var pending = procs.Select(p => p.Name).ToList(); var ok = new List<string>(); var err = new Dictionary<string, string>();
    for (int passNo = 0; passNo < 3 && pending.Count > 0; passNo++)
    {
        var still = new List<string>();
        foreach (var name in pending)
        {
            try { Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddlMap[name]); ok.Add(name); }
            catch
            {
                try { var ddl2 = ConvertProcDdl(name, procDef[name], paramsMap[name], applyConcat: true); Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddl2); ddlMap[name] = ddl2; ok.Add(name); }
                catch (Exception ex2) { err[name] = OneLine(ex2.Message); still.Add(name); }
            }
        }
        if (still.Count == pending.Count) break;
        pending = still;
    }

    var createFail = pending.Select(n => $"{n}: {err.GetValueOrDefault(n, "?")}").ToList();
    File.WriteAllText("tvpprocs_needs_manual.txt", "TVP procs that FAILED to convert+create:\n\n" + string.Join("\n", createFail), new UTF8Encoding(false));
    WriteVerified("converted_PENDING_tvpprocs.sql", "PROCEDURE", ok.Select(n => ddlMap[n]));
    Console.WriteLine($"TVP procs: {procs.Count} total | converted+created {ok.Count} | create-fail {createFail.Count} -> tvpprocs_needs_manual.txt");
    Console.WriteLine($"  NOTE: created with tvp_<name> temp-table refs; runtime requires the data layer to stage the DataTable into tvp_<name>.");
}

// HARVEST — convert+create a SPECIFIC list of procs (the live-missing set) against the configured MySql
// target (point it at a throwaway SCRATCH db). CREATE PROCEDURE validates only SYNTAX (not table/column refs),
// so an empty scratch db is enough to prove a conversion is syntactically valid; harvested DDL is written to
// a runnable file and then applied to the live db. No execution -> no data mutation.
void DoHarvest(Cfg c)
{
    var targetsFile = args.Length > 1 ? args[1] : "harvest_targets.txt";
    var targets = File.ReadAllLines(targetsFile).Select(l => l.Trim()).Where(l => l.Length > 0)
        .Select(l => l.ToLowerInvariant()).ToHashSet();
    using var src = Open(c.SqlServer);
    LoadTvfs(src);   // parameterized TVFs -> inlined derived tables
    using var dst = OpenMy(c.MySql);
    Exec(dst, "SET NAMES utf8mb4;");
    // README_MYSQL §7 (error 1267): SET NAMES alone leaves the CONNECTION collation at utf8mb4_general_ci
    // while every column is utf8mb4_unicode_ci. MySQL BAKES the session collation into a routine at CREATE
    // time, so without this the routine hits "Illegal mix of collations" at runtime on any UNION or string
    // comparison. Must be set on the CREATING connection, not just in the emitted .sql header.
    Exec(dst, "SET collation_connection='utf8mb4_unicode_ci';");
    Exec(dst, "SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';");
    var procs = new List<(string Name, string Def)>();
    using (var cmd2 = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id WHERE o.type='P' ORDER BY o.name", src))
    using (var r = cmd2.ExecuteReader())
        while (r.Read()) { var nm = r.GetString(0); if (targets.Contains(nm.ToLowerInvariant())) procs.Add((nm, r.GetString(1))); }

    var ok = new List<string>(); var fail = new Dictionary<string, string>(); var ddlOut = new List<string>();
    Directory.CreateDirectory("harvest_debug");
    foreach (var (name, def) in procs)
    {
        var ps = LoadProcParams(src, name);
        var ddl = ConvertProcDdl(name, def, ps);
        File.WriteAllText($"harvest_debug/{name}.sql", ddl, new UTF8Encoding(false));   // ground-truth converted DDL (pass or fail)
        try { Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddl); ok.Add(name); ddlOut.Add(ddl); }
        catch
        {
            try { var ddl2 = ConvertProcDdl(name, def, ps, applyConcat: true); Exec(dst, $"DROP PROCEDURE IF EXISTS `{name}`"); Exec(dst, ddl2); ok.Add(name); ddlOut.Add(ddl2); }
            catch (Exception ex2) { fail[name] = OneLine(ex2.Message); }
        }
    }
    WriteVerified("harvested.sql", "PROCEDURE", ddlOut);
    File.WriteAllText("harvest_fail.txt", string.Join("\n", fail.OrderBy(k => k.Key).Select(kv => kv.Key + ": " + kv.Value)), new UTF8Encoding(false));
    Console.WriteLine($"Harvest: targets {targets.Count} | matched {procs.Count} | created {ok.Count} | failed {fail.Count} -> harvested.sql / harvest_fail.txt");
}

string ConvertProcDdl(string name, string def, List<Prm> ps, bool applyConcat = false)
{
    def = ApplySrcPatch(name, def);
    // Strip comments up front so a comment right after the body "AS" (e.g. "AS\n--declare..\nSELECT")
    // doesn't defeat the AS-before-statement-keyword detection below.
    def = StripSqlComments(def);   // string-aware: never eats a literal containing '--'
    // The body starts at the "AS" that precedes the first statement keyword — NOT the "as" inside a
    // param decl like "@x as int". Match AS followed by a statement starter.
    var m = System.Text.RegularExpressions.Regex.Match(def,
        @"\bAS\b\s+(?=BEGIN\b|SELECT\b|UPDATE\b|INSERT\b|DELETE\b|DECLARE\b|SET\b|IF\b|WHILE\b|WITH\b|RETURN\b|EXEC\b|TRUNCATE\b|DROP\b|CREATE\b|CALL\b)",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
    string rawBody = m.Success ? def.Substring(m.Index + m.Length) : def;
    rawBody = FlattenLeadingGroupingBegin(rawBody);
    // T-SQL parameter defaults ("@x TYPE = <non-null>") are silently LOST in MySQL, which has no
    // proc-parameter defaults: an omitted arg arrives as NULL (EnsureAllProcParams fills it), which
    // flips behavior. Concretely, Companies_Crud has "@IsActive BIT = 1"; the "add company" form omits
    // @IsActive, so on SQL Server the row inserts IsActive=1 (visible), but on MySQL it inserted NULL and
    // then vanished from every "WHERE IsActive = 1" read. Re-apply each NON-NULL default as a leading
    // normalizer on the RAW T-SQL body: "SET @x = ISNULL(@x, <default>)". Riding the normal pipeline gets
    // ISNULL->IFNULL, @x->p_x, and DECLARE-hoisting for free, so the emitted order stays legal (DECLAREs,
    // then these SETs, then the body). NULL defaults need nothing — the omitted-param path already yields NULL.
    var hdr = m.Success ? def.Substring(0, m.Index) : def;
    foreach (var (pn, dv) in ParseNonNullParamDefaults(hdr))
        rawBody = $"SET @{pn} = ISNULL(@{pn}, {dv});\n" + rawBody;
    string body = TransformBody(rawBody,
        ps.Where(p => !p.IsTvp).Select(p => p.Col.Name), applyConcat,
        ps.Where(p => p.IsTvp).Select(p => p.Col.Name)).Trim();
    // T-SQL bare RETURN (proc early-exit / status return) has no MySQL equivalent inside a procedure
    // ("RETURN is only allowed in a FUNCTION"). Map it to LEAVE of a labeled outer block. Only the
    // value-less form (RETURN; / RETURN <eol>); a "RETURN <expr>" status code is left for hand-review.
    var ICp = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    bool hasReturn = System.Text.RegularExpressions.Regex.IsMatch(body, @"\bRETURN\b\s*(?=;|$|\r|\n|END\b)", ICp);
    if (hasReturn)
        body = System.Text.RegularExpressions.Regex.Replace(body, @"\bRETURN\b\s*(;?)", "LEAVE proc;", ICp);
    if (!System.Text.RegularExpressions.Regex.IsMatch(body, @"^BEGIN\b", ICp))
    {
        if (!body.EndsWith(";")) body += ";";   // terminate the (last) statement before wrapping
        body = (hasReturn ? "proc: BEGIN\n" : "BEGIN\n") + body + "\nEND";
    }
    else if (hasReturn)   // label the proc's own outermost BEGIN so LEAVE proc resolves
        body = System.Text.RegularExpressions.Regex.Replace(body, @"^BEGIN\b", "proc: BEGIN", ICp);
    // TVP params are NOT MySQL proc params (staged into tvp_<name> temp tables instead) — exclude from signature.
    var ph = string.Join(", ", ps.Where(p => !p.IsTvp).Select(p => $"{(p.IsOut ? "INOUT" : "IN")} `p_{p.Col.Name}` {MapType(p.Col)}"));
    return $"CREATE PROCEDURE `{name}`({ph})\n{body}";
}

List<Prm> LoadProcParams(SqlConnection c, string name)
{
    using var cmd2 = new SqlCommand(@"SELECT p.name, ty.name, p.max_length, p.precision, p.scale, p.is_output, ty.is_table_type
        FROM sys.parameters p JOIN sys.types ty ON p.user_type_id=ty.user_type_id
        WHERE p.object_id=OBJECT_ID(@n) ORDER BY p.parameter_id", c);
    cmd2.Parameters.AddWithValue("@n", "dbo." + name);
    using var r = cmd2.ExecuteReader();
    var list = new List<Prm>();
    while (r.Read())
    {
        bool tvp = r.GetBoolean(6);
        list.Add(new Prm { Col = new Col { Name = r.GetString(0).TrimStart('@'), Type = r.GetString(1), MaxLength = r.GetInt16(2), Precision = r.GetByte(3), Scale = r.GetByte(4), IsNullable = true }, IsOut = r.GetBoolean(5), IsTvp = tvp, TvpType = tvp ? r.GetString(1) : "" });
    }
    return list;
}

(List<string>, Dictionary<string, string>) CallProcSql(SqlConnection c, string name, List<Prm> ps)
{
    // wrap + rollback so write procs (non-transactional inserts/updates) don't mutate the snapshot
    using var tx = c.BeginTransaction();
    using var cmd2 = new SqlCommand("dbo." + name, c, tx) { CommandType = CommandType.StoredProcedure };
    foreach (var p in ps)
        cmd2.Parameters.Add(new SqlParameter("@" + p.Col.Name, SampleValue(p.Col)) { Direction = p.IsOut ? ParameterDirection.InputOutput : ParameterDirection.Input });
    var rows = ReadReaderRows(cmd2);
    var outv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    foreach (SqlParameter p in cmd2.Parameters) if (p.Direction != ParameterDirection.Input) outv[p.ParameterName.TrimStart('@')] = NormScalar(p.Value);
    try { tx.Rollback(); } catch { /* a transactional proc may have already closed the txn (its own COMMIT/ROLLBACK) */ }
    return (rows, outv);
}

(List<string>, Dictionary<string, string>) CallProcMy(MySqlConnection c, string name, List<Prm> ps)
{
    using var tx = c.BeginTransaction();
    using var cmd2 = new MySqlCommand(name, c, tx) { CommandType = CommandType.StoredProcedure };
    foreach (var p in ps)
        cmd2.Parameters.Add(new MySqlParameter("@p_" + p.Col.Name, SampleValue(p.Col)) { Direction = p.IsOut ? ParameterDirection.InputOutput : ParameterDirection.Input });
    var rows = ReadReaderRows(cmd2);
    var outv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    foreach (MySqlParameter p in cmd2.Parameters) if (p.Direction != ParameterDirection.Input) { var k = p.ParameterName.TrimStart('@'); if (k.StartsWith("p_")) k = k.Substring(2); outv[k] = NormScalar(p.Value); }
    tx.Rollback();
    return (rows, outv);
}

static List<string> ReadReaderRows(IDbCommand cmd)
{
    var rows = new List<string>(); int set = 0;
    using var r = cmd.ExecuteReader();
    do
    {
        while (r.Read())
        {
            var sb = new StringBuilder().Append(set).Append(':');
            for (int i = 0; i < r.FieldCount; i++) { var v = r.GetValue(i); sb.Append(v is DBNull or null ? "<null>" : NormScalar(v)).Append('|'); }
            rows.Add(sb.ToString());
        }
        set++;
    } while (r.NextResult());
    rows.Sort(StringComparer.Ordinal);
    return rows;
}

static bool OutEqual(Dictionary<string, string> a, Dictionary<string, string> b)
{
    foreach (var k in a.Keys.Union(b.Keys))
        if (a.GetValueOrDefault(k, "∅") != b.GetValueOrDefault(k, "∅")) return false;
    return true;
}

static object SampleValue(Col p)
{
    var m = MapType(p);
    if (m.StartsWith("VARCHAR") || m.StartsWith("CHAR") || m.Contains("TEXT")) return "1";
    if (m == "DATE" || m == "DATETIME") return new DateTime(2024, 1, 1);
    if (m == "TIME") return new TimeSpan(0);
    return 1;
}

static string SampleArg(Col p)
{
    var m = MapType(p);
    if (m.StartsWith("VARCHAR") || m.StartsWith("CHAR") || m.Contains("TEXT")) return "'1'";
    if (m == "DATE") return "'2024-01-01'";
    if (m == "DATETIME") return "'2024-01-01 00:00:00'";
    if (m == "TIME") return "'00:00:00'";
    if (m == "CHAR(36)") return "'00000000-0000-0000-0000-000000000000'";
    return "1";   // numeric / bit
}

static string NormScalar(object v) => v switch
{
    null or DBNull => "<null>",
    bool b => b ? "1" : "0",
    DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
    decimal d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
    double db => db.ToString("R", System.Globalization.CultureInfo.InvariantCulture),
    byte[] by => Convert.ToHexString(by),
    _ => v.ToString()?.TrimEnd() ?? ""
};

// ---------------- type map ----------------
static string MapType(Col c)
{
    switch (c.Type)
    {
        case "int": return "INT";
        case "bigint": return "BIGINT";
        case "smallint": return "SMALLINT";
        case "tinyint": return "TINYINT UNSIGNED";      // SQL Server tinyint is 0..255
        case "bit": return "TINYINT(1)";
        case "decimal":
        case "numeric": return $"DECIMAL({c.Precision},{c.Scale})";
        case "money": return "DECIMAL(19,4)";
        case "float": return "DOUBLE";
        case "real": return "FLOAT";
        case "date": return "DATE";
        case "datetime":
        case "datetime2":
        case "smalldatetime": return "DATETIME";
        case "time": return "TIME";
        case "image": return "LONGBLOB";
        case "uniqueidentifier": return "CHAR(36)";
        case "text":
        case "ntext": return "LONGTEXT";
        case "varbinary":
        case "binary": return c.MaxLength == -1 ? "LONGBLOB" : (c.MaxLength > 255 ? "BLOB" : $"VARBINARY({c.MaxLength})");
        case "char": return c.MaxLength > 255 ? "TEXT" : $"CHAR({c.MaxLength})";
        case "nchar": { int n = c.MaxLength / 2; return n > 255 ? "TEXT" : $"CHAR({n})"; }
        case "varchar": return c.MaxLength == -1 ? "LONGTEXT" : (c.MaxLength > 255 ? "TEXT" : $"VARCHAR({c.MaxLength})");
        case "nvarchar": { if (c.MaxLength == -1) return "LONGTEXT"; int n = c.MaxLength / 2; return n > 255 ? "TEXT" : $"VARCHAR({n})"; }
        default: return "LONGTEXT";
    }
}

// Estimated bytes a column contributes to a btree key (utf8mb4 = 4 bytes/char). TEXT/BLOB -> huge -> skip.
static int EstKeyBytes(Col c)
{
    var m = MapType(c);
    if (m.Contains("TEXT") || m.Contains("BLOB")) return 9999;
    if (m.StartsWith("VARCHAR") || m.StartsWith("CHAR")) { int n = int.Parse(m[(m.IndexOf('(') + 1)..m.IndexOf(')')]); return n * 4 + 2; }
    if (m.StartsWith("VARBINARY")) { int n = int.Parse(m[(m.IndexOf('(') + 1)..m.IndexOf(')')]); return n + 2; }
    if (m.StartsWith("DECIMAL")) return c.Precision / 2 + 1;
    return m switch { "BIGINT" or "DOUBLE" or "DATETIME" => 8, "INT" or "FLOAT" => 4, "SMALLINT" => 2, "TINYINT UNSIGNED" or "TINYINT(1)" => 1, "DATE" or "TIME" => 3, "CHAR(36)" => 144, _ => 8 };
}

// ---------------- metadata loaders ----------------
List<Table> LoadTables(string conn)
{
    using var c = Open(conn);
    using var cmd2 = new SqlCommand(@"SELECT s.name, t.name, t.object_id FROM sys.tables t
        JOIN sys.schemas s ON t.schema_id=s.schema_id
        WHERE t.name NOT LIKE 'plan_persist%' AND t.name NOT LIKE 'sys%' AND t.name NOT LIKE 'Private_%'
        ORDER BY t.name", c);
    using var r = cmd2.ExecuteReader();
    var list = new List<Table>();
    while (r.Read()) list.Add(new Table { Schema = r.GetString(0), Name = r.GetString(1), ObjectId = r.GetInt32(2) });
    return list;
}

// Column DEFAULT constraints for one table: column name -> raw T-SQL definition, e.g. "((0))", "(getdate())".
Dictionary<string, string> LoadDefaults(SqlConnection c, int oid)
{
    using var cmd2 = new SqlCommand(@"SELECT COL_NAME(dc.parent_object_id, dc.parent_column_id), dc.definition
        FROM sys.default_constraints dc WHERE dc.parent_object_id=@o", c);
    cmd2.Parameters.AddWithValue("@o", oid);
    using var r = cmd2.ExecuteReader();
    var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    while (r.Read()) if (!r.IsDBNull(0)) d[r.GetString(0)] = r.GetString(1);
    return d;
}

// Translate a T-SQL default expression to its MySQL equivalent. Returns null when it cannot be
// translated faithfully -- the caller logs those instead of guessing (README: never invent behavior).
//   ((0)) -> 0        ((0.000)) -> 0.000      ('Main') -> 'Main'      (N'نص') -> 'نص'
//   (getdate()) -> CURRENT_TIMESTAMP | (CURDATE()) | (CURTIME())  depending on the column type.
// NOTE the parentheses around CURDATE()/CURTIME(): MariaDB requires an expression default to be
// parenthesized for date/time columns (README §8.2). Only CURRENT_TIMESTAMP is allowed bare.
static string? MapDefault(string raw, string colType)
{
    var s = raw.Trim();
    // strip the redundant wrapping parens SQL Server stores: "((0))" -> "0"
    while (s.StartsWith("(") && s.EndsWith(")") && Balanced(s[1..^1])) s = s[1..^1].Trim();

    var t = colType.ToLowerInvariant();
    if (s.Equals("getdate()", StringComparison.OrdinalIgnoreCase) ||
        s.Equals("sysdatetime()", StringComparison.OrdinalIgnoreCase))
    {
        if (t == "date") return "(CURDATE())";
        if (t == "time") return "(CURTIME())";
        return "CURRENT_TIMESTAMP";           // datetime / datetime2 / smalldatetime
    }
    if (s.Equals("newid()", StringComparison.OrdinalIgnoreCase)) return "(UUID())";
    if (s.Equals("null", StringComparison.OrdinalIgnoreCase)) return "NULL";

    // numeric literal (covers ((0)), ((1)), ((0.000)), ((4370)), and negatives)
    if (decimal.TryParse(s, System.Globalization.NumberStyles.Any,
                         System.Globalization.CultureInfo.InvariantCulture, out _)) return s;

    // string literal, optionally N-prefixed. Preserve the value EXACTLY (Arabic text lives here).
    if (s.StartsWith("N'") || s.StartsWith("n'")) s = s[1..];
    if (s.Length >= 2 && s[0] == '\'' && s[^1] == '\'')
    {
        var inner = s[1..^1].Replace("''", "'");          // un-double T-SQL escaping
        return "'" + inner.Replace("\\", "\\\\").Replace("'", "''") + "'";
    }

    return null;   // e.g. "NEXT VALUE FOR [Seq_...]" -- reported, never guessed
}

// T-SQL frequently BRACKETS the data type in a DECLARE: "DECLARE @ErrMsg [VARCHAR](200), @Sev [INT]".
// The generic [x] -> `x` rule turns that into `VARCHAR`(200) / `INT`, i.e. a BACKTICKED IDENTIFIER where a
// TYPE is expected -> "create failed" for every proc that declares its locals this way.
// Strip the brackets around type names only, and only in unambiguous positions, so a COLUMN legitimately
// named [Date] / [Text] / [Money] is left alone:
//   (a) a bracketed type immediately followed by '(' -- e.g. [VARCHAR](MAX), [DECIMAL](15,3)
//   (b) a bracketed type immediately preceded by an @variable (optionally "AS") -- e.g. @Sev [INT]
static string StripTypeBrackets(string s)
{
    const string SIZED = "VARCHAR|NVARCHAR|CHAR|NCHAR|DECIMAL|NUMERIC|BINARY|VARBINARY|FLOAT|DATETIME2|DATETIMEOFFSET";
    const string BARE = "INT|INTEGER|BIGINT|SMALLINT|TINYINT|BIT|DATE|DATETIME|SMALLDATETIME|TIME|MONEY|SMALLMONEY|REAL|TEXT|NTEXT|IMAGE|UNIQUEIDENTIFIER|XML|SQL_VARIANT";
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    s = System.Text.RegularExpressions.Regex.Replace(s, $@"\[({SIZED})\]\s*(?=\()", "$1", IC);
    s = System.Text.RegularExpressions.Regex.Replace(s, $@"(@\w+\s+(?:AS\s+)?)\[({SIZED}|{BARE})\]", "$1$2", IC);
    return s;
}

// ---------------------------------------------------------------------------------------------------
// T-SQL '+' string concatenation -> MySQL CONCAT(...).   README_MYSQL §3.2 — THE #1 SILENT KILLER.
// MySQL's '+' is ARITHMETIC ONLY. "'رقم: ' + @code" does not error — it coerces the text to a number and
// yields 0. A proc created this way looks healthy and returns garbage. Real example found in this database:
//     ACCCODE like + convert(varchar, @accacount) + '%'      (T-SQL)
//   -> ACCCODE like + CAST(p_accacount AS CHAR) + '%'        (MySQL: LIKE 0 -> matches nothing, no error)
//
// A blanket '+'->CONCAT is NOT safe (it would wreck numeric addition, and the original author recorded that
// it regressed passing objects). So a chain of '+'-separated operands is rewritten ONLY when at least one
// operand is provably a string:
//     * a string literal            'x'
//     * a CAST(... AS CHAR)
//     * a variable/param DECLAREd as VARCHAR/CHAR/TEXT/LONGTEXT  (names passed in via strVars)
// A purely numeric chain (a.Debit + a.Credit) has no string operand and is left untouched.
static string FixStringConcat(string body, HashSet<string> strVars)
{
    // 1) Unary '+' that T-SQL tolerates in front of a concat chain: "LIKE + convert(..)" / "= + 'x'".
    //    MySQL reads it as a numeric sign. Drop it.
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)(\bLIKE\s*|[=,(]\s*)\+\s*(?=CAST\s*\(|CONCAT\s*\(|')", "$1");
    // ...and the same unary '+' sitting inside a CASE branch: "THEN +SPACE(1) + ' x'".
    // \s* not \s+ — the T-SQL is sometimes glued ("THEN+SPACE(1)"). The replacement re-inserts a space so the
    // keyword cannot fuse with the operand ("THENCONCAT").
    body = System.Text.RegularExpressions.Regex.Replace(body,
        @"(?i)\b(THEN|ELSE)\s*\+\s*(?=[A-Za-z_'(])", "$1 ");

    // 2) A CASE ... END is a perfectly legal operand of a concat:
    //        CASE WHEN @t=0 THEN 'شراء' ELSE 'بيع' END + SPACE(1) + @price
    //    but it cannot be expressed as an "atom" in the chain regex (CASE/END are keywords, and it nests).
    //    So swap each BALANCED CASE..END for an opaque placeholder token first, run the chain rewrite (the
    //    placeholder behaves like an ordinary identifier), then restore. Nesting and string literals are
    //    tracked while scanning, so an inner CASE inside an ELSE branch is captured with its parent.
    var cases = new List<string>();
    body = MaskCaseExpressions(body, cases);

    // CRITICAL: masking hides the CASE *branches* from the chain rewrite below, and a branch very often
    // contains its own concat ("THEN 'من حساب ' + @name"). Left masked, those '+' survive into MySQL as
    // arithmetic — the exact silent bug we are trying to kill. So fix each masked CASE's INTERIOR first
    // (recursively: an inner CASE gets masked and fixed by the nested call, and the text strictly shrinks,
    // so this terminates).
    for (int k = 0; k < cases.Count; k++)
    {
        var c = cases[k];                                   // "CASE … END"
        var inner = c.Substring(4, c.Length - 4 - 3);       // strip the leading CASE and trailing END
        cases[k] = c.Substring(0, 4) + FixStringConcat(inner, strVars) + c.Substring(c.Length - 3);
    }

    // An operand must never be a bare SQL keyword. Without this guard the END of a CASE expression is read as
    // an identifier operand, so  "CASE ... END + ' ' + @x"  becomes  "CONCAT(END, ' ', x)"  — nonsense that
    // fails to create (loudly, thankfully). Keywords that can legally sit just before a '+'.
    const string KW = @"(?!(?:END|THEN|ELSE|WHEN|CASE|AND|OR|NOT|IS|NULL|LIKE|IN|BETWEEN|SELECT|FROM|WHERE|SET|RETURN|AS|ON|BY)\b)";
    // \b is ESSENTIAL on every identifier alternative. Without it the engine may start an atom in the MIDDLE
    // of a word: inside "END" it skips the E (not a keyword match at that offset), matches "ND" as an
    // identifier, and emits "E" + "CONCAT(ND, ...)" — i.e. "ECONCAT(ND,". The \b forces an atom to begin at a
    // word boundary, so the keyword guard above is actually evaluated against the WHOLE word.
    // The function-call alternative uses a .NET BALANCING-GROUP paren matcher so it matches parens to ANY
    // depth. The previous fixed 3-level pattern could not span "CAST(IFNULL((SELECT MAX(x) FROM ..), 0) + 1
    // AS CHAR)" (4 levels), so that operand of a '+' concat was mis-split, emitting the garbage "CAST)(...".
    const string BAL = @"\((?:[^()]|(?<d>\()|(?<-d>\)))*(?(d)(?!))\)";
    const string ATOM =
        @"(?:[Nn]?'(?:[^']|'')*'" +                        // string literal, optionally N-prefixed (national)
        @"|\b" + KW + @"[A-Za-z_]\w*\s*" + BAL +           // function call (balanced parens, any depth)
        @"|\b" + KW + @"[A-Za-z_][\w.]*" +                 // identifier / column / var
        @"|\b\d+(?:\.\d+)?)";                              // number

    var chain = new System.Text.RegularExpressions.Regex(
        $@"{ATOM}(?:\s*\+\s*{ATOM})+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

    var result = chain.Replace(body, m =>
    {
        var parts = SplitTopLevel(m.Value, '+');
        if (parts.Count < 2) return m.Value;

        bool stringy = parts.Any(p =>
        {
            var t = p.Trim();
            if (t.StartsWith("'") || t.StartsWith("N'") || t.StartsWith("n'")) return true;  // literal (opt. N-prefix)
            if (System.Text.RegularExpressions.Regex.IsMatch(t, @"(?i)\bAS\s+CHAR\s*\)")) return true;  // CAST(.. AS CHAR)
            // A call to a function that RETURNS A STRING proves the whole chain is a concat, even when no
            // operand is a literal and none is a declared variable. This is how
            //     B.BankName + SPACE(1) + A.BranchName
            // (two plain COLUMNS around a SPACE) was being missed — MySQL evaluated it as arithmetic and the
            // branch-name column silently rendered as 0.
            if (System.Text.RegularExpressions.Regex.IsMatch(t,
                    @"(?i)^(CONCAT|SPACE|SUBSTRING|SUBSTR|LEFT|RIGHT|TRIM|LTRIM|RTRIM|REPLACE|UPPER|LOWER|LPAD|RPAD|REVERSE|CHAR|DATE_FORMAT|FORMAT)\s*\(")) return true;
            // a masked CASE..END whose branches yield TEXT proves the chain is a concat
            var cm = System.Text.RegularExpressions.Regex.Match(t, @"^__CASE(\d+)__$");
            if (cm.Success) return cases[int.Parse(cm.Groups[1].Value)].Contains('\'');
            return strVars.Contains(t);                                                // declared string var / param
        });

        // CONTEXT rule: a '+' chain feeding a LIKE is a string concat BY DEFINITION, even when every operand
        // is a plain column and nothing else gives it away. Real case found here:
        //     WHERE GCODE + GRNAME LIKE '%' + @ISID + '%'
        // Left as '+', MySQL computes GCODE+GRNAME arithmetically -> 0, so the predicate becomes
        // "0 LIKE '%x%'" and the search screen silently returns NO ROWS.
        if (!stringy)
        {
            var after = body.Substring(m.Index + m.Length);
            if (System.Text.RegularExpressions.Regex.IsMatch(after, @"^\s*(?:NOT\s+)?LIKE\b",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase)) stringy = true;
        }

        if (!stringy) return m.Value;                                                  // numeric addition: leave alone

        return "CONCAT(" + string.Join(", ", parts.Select(p => p.Trim())) + ")";
    });

    return UnmaskCaseExpressions(result, cases);
}

// Replace every balanced CASE..END with __CASE<n>__ , stashing the original text in `store`.
// Scans character by character so nested CASEs and string literals (which may themselves contain the words
// CASE/END) are handled correctly.
static string MaskCaseExpressions(string s, List<string> store)
{
    var sb = new StringBuilder();
    int i = 0;
    while (i < s.Length)
    {
        // copy string literals verbatim — 'END' inside a literal must not be seen as a keyword
        if (s[i] == '\'')
        {
            int st = i++;
            while (i < s.Length) { if (s[i] == '\'') { if (i + 1 < s.Length && s[i + 1] == '\'') i += 2; else { i++; break; } } else i++; }
            sb.Append(s, st, i - st);
            continue;
        }
        if (IsKeywordAt(s, i, "CASE"))
        {
            int start = i, depth = 0, j = i;
            while (j < s.Length)
            {
                if (s[j] == '\'')
                {
                    j++;
                    while (j < s.Length) { if (s[j] == '\'') { if (j + 1 < s.Length && s[j + 1] == '\'') j += 2; else { j++; break; } } else j++; }
                    continue;
                }
                if (IsKeywordAt(s, j, "CASE")) { depth++; j += 4; continue; }
                if (IsKeywordAt(s, j, "END"))
                {
                    depth--; j += 3;
                    if (depth == 0) break;
                    continue;
                }
                j++;
            }
            if (depth != 0) { sb.Append(s[i]); i++; continue; }   // unbalanced: leave alone
            store.Add(s.Substring(start, j - start));
            sb.Append("__CASE").Append(store.Count - 1).Append("__");
            i = j;
            continue;
        }
        sb.Append(s[i]);
        i++;
    }
    return sb.ToString();
}

static string UnmaskCaseExpressions(string s, List<string> store)
{
    for (int k = store.Count - 1; k >= 0; k--) s = s.Replace("__CASE" + k + "__", store[k]);
    return s;
}

// keyword match at position i, honouring word boundaries on both sides
static bool IsKeywordAt(string s, int i, string kw)
{
    if (i + kw.Length > s.Length) return false;
    if (string.Compare(s, i, kw, 0, kw.Length, StringComparison.OrdinalIgnoreCase) != 0) return false;
    if (i > 0 && (char.IsLetterOrDigit(s[i - 1]) || s[i - 1] == '_')) return false;
    int e = i + kw.Length;
    if (e < s.Length && (char.IsLetterOrDigit(s[e]) || s[e] == '_')) return false;
    return true;
}

// Names of v_/p_ identifiers declared with a character type, so FixStringConcat can recognise "v_a + v_b"
// as a STRING concat (neither operand is a literal, but both are text) rather than silent arithmetic.
static HashSet<string> CollectStringVars(string body)
{
    var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(
                 body, @"(?im)\b(?:DECLARE|IN|OUT|INOUT)?\s*`?([vp]_\w+)`?\s+(?:VARCHAR|CHAR|TEXT|LONGTEXT|MEDIUMTEXT|TINYTEXT)\b"))
        set.Add(m.Groups[1].Value);
    return set;
}

// ---------------------------------------------------------------------------------------------------
// T-SQL  FORMAT(expr, 'N3', 'en-us')  ->  MySQL  FORMAT(expr, 3)      — ANOTHER SILENT-WRONGNESS CLASS.
// Both engines HAVE a FORMAT() function, so nothing errors — but they mean different things:
//     T-SQL : FORMAT(x, '<.NET format string>' [, culture])   'N3' = thousands separators + 3 decimals
//     MySQL : FORMAT(x, D [, locale])                         D = NUMBER of decimal places
// MySQL coerces the string 'N3' to the number 0, so every formatted money value comes back with ZERO
// decimals: 1,022,611  instead of  1,022,611.330. Found in 61 routines / 331 call sites here.
// Parsed with balanced parens because the expression is normally nested, e.g.
//     FORMAT(IFNULL(SUM(b.Debit), 0.000), 'N3', 'en-us')
static string FixTsqlFormat(string body)
{
    int from = 0;
    while (true)
    {
        var m = System.Text.RegularExpressions.Regex.Match(body.Substring(from), @"\bFORMAT\s*\(",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (!m.Success) break;
        int start = from + m.Index;
        int open = start + m.Length - 1;

        int i = open + 1, depth = 1; char quote = '\0';
        var args = new List<string>(); var cur = new StringBuilder();
        for (; i < body.Length && depth > 0; i++)
        {
            char ch = body[i];
            if (quote != '\0') { cur.Append(ch); if (ch == quote) quote = '\0'; continue; }
            if (ch == '\'' || ch == '"') { quote = ch; cur.Append(ch); continue; }
            if (ch == '(') depth++;
            else if (ch == ')') { depth--; if (depth == 0) break; }
            if (depth == 1 && ch == ',') { args.Add(cur.ToString()); cur.Clear(); continue; }
            cur.Append(ch);
        }
        if (depth != 0) break;
        args.Add(cur.ToString());
        int close = i;

        // only rewrite when arg[1] is a QUOTED .NET format string; FORMAT(x, 3) is already MySQL-correct
        if (args.Count >= 2 && args[1].Trim().StartsWith("'"))
        {
            var fmt = args[1].Trim().Trim('\'');
            string? repl = null;

            var nm = System.Text.RegularExpressions.Regex.Match(fmt, @"^[Nn](\d+)$");
            var fm = System.Text.RegularExpressions.Regex.Match(fmt, @"^[Ff](\d+)$");
            if (nm.Success)
                repl = $"FORMAT({args[0].Trim()}, {int.Parse(nm.Groups[1].Value)})";
            else if (fm.Success)
                // .NET "F<n>" = fixed-point, n decimals, NO thousands separator. MySQL FORMAT() always inserts
                // the separator ("12,345.678"), so strip it to match T-SQL exactly ("12345.678").
                repl = $"REPLACE(FORMAT({args[0].Trim()}, {int.Parse(fm.Groups[1].Value)}), ',', '')";
            else if (fmt.Equals("N", StringComparison.OrdinalIgnoreCase))
                repl = $"FORMAT({args[0].Trim()}, 2)";                       // .NET "N" defaults to 2 decimals
            else if (fmt.Equals("F", StringComparison.OrdinalIgnoreCase))
                repl = $"REPLACE(FORMAT({args[0].Trim()}, 2), ',', '')";     // .NET "F" defaults to 2 decimals, no sep
            else if (System.Text.RegularExpressions.Regex.IsMatch(fmt, @"^[#0,.\s]+$"))
            {
                int dot = fmt.IndexOf('.');                                  // custom numeric, e.g. '#,##0.000'
                repl = $"FORMAT({args[0].Trim()}, {(dot < 0 ? 0 : fmt.Length - dot - 1)})";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(fmt, @"[yMdHhmst]"))
            {
                // DATE/TIME pattern. T-SQL FORMAT doubles as a date formatter — 'hh:mm:ss tt'.
                // Treating it as numeric (the bug this branch fixes) turned 07:13:53 PM into 20,260,712,191,353.
                repl = $"DATE_FORMAT({args[0].Trim()}, '{MapDateFormat(fmt)}')";
            }
            // anything else (currency 'C', percent 'P', …) is left alone -> MySQL errors LOUDLY, which is what
            // we want: it lands in the manual bucket instead of silently producing a wrong value.

            if (repl != null)
            {
                body = body.Substring(0, start) + repl + body.Substring(close + 1);
                from = start + repl.Length;
                continue;
            }
        }
        from = close + 1;
    }
    return body;
}

// .NET / T-SQL date-format pattern  ->  MySQL DATE_FORMAT specifier string.
// CASE MATTERS in .NET: 'M' is month, 'm' is minute; 'H' is 24-hour, 'h' is 12-hour. Longest tokens first,
// and literal text (':', '/', spaces) passes through untouched.
static string MapDateFormat(string fmt)
{
    var map = new (string net, string my)[]
    {
        ("yyyy","%Y"), ("yy","%y"),
        ("MMMM","%M"), ("MMM","%b"), ("MM","%m"), ("M","%c"),
        ("dddd","%W"), ("ddd","%a"), ("dd","%d"), ("d","%e"),
        ("HH","%H"), ("H","%k"),
        ("hh","%h"), ("h","%l"),
        ("mm","%i"), ("m","%i"),          // MINUTES (lower-case m) -> %i
        ("ss","%s"), ("s","%s"),
        ("tt","%p"), ("t","%p"),
        ("fff","%f"),
    };
    var sb = new StringBuilder();
    int i = 0;
    while (i < fmt.Length)
    {
        bool hit = false;
        foreach (var (net, my) in map)
        {
            if (i + net.Length <= fmt.Length && string.CompareOrdinal(fmt, i, net, 0, net.Length) == 0)
            {
                sb.Append(my); i += net.Length; hit = true; break;
            }
        }
        if (!hit)
        {
            if (fmt[i] == '\'') { i++; continue; }        // .NET literal quoting
            if (fmt[i] == '%') sb.Append("%%"); else sb.Append(fmt[i]);
            i++;
        }
    }
    return sb.ToString();
}

// T-SQL date functions -> MySQL. All parsed with balanced parens because the date argument is usually a
// nested call, e.g. DATEADD(DAY, 1, CAST(@D2 AS date)).
//   DATEADD(part, n, d)  -> DATE_ADD(d, INTERVAL n <part>)
//   DATEDIFF(part, a, b) -> TIMESTAMPDIFF(<part>, a, b)   (arg order matches)
//   DATEPART(part, d)    -> EXTRACT(<part> FROM d)
static string ConvertDateFuncs(string body)
{
    string MapPart(string p)
    {
        p = p.Trim().ToLowerInvariant();
        switch (p)
        {
            case "dd": case "d": case "day": case "dayofyear": case "dy": return "DAY";
            case "mm": case "m": case "month": return "MONTH";
            case "yy": case "yyyy": case "year": return "YEAR";
            case "hh": case "hour": return "HOUR";
            case "mi": case "n": case "minute": return "MINUTE";
            case "ss": case "s": case "second": return "SECOND";
            case "wk": case "ww": case "week": return "WEEK";
            case "qq": case "q": case "quarter": return "QUARTER";
            default: return p.ToUpperInvariant();
        }
    }
    foreach (var fn in new[] { "DATEADD", "DATEDIFF", "DATEPART" })
    {
        int from = 0;
        while (true)
        {
            var m = System.Text.RegularExpressions.Regex.Match(body.Substring(from),
                @"\b" + fn + @"\s*\(", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!m.Success) break;
            int start = from + m.Index, open = start + m.Length - 1;
            var inner = ReadBalanced(body, open, out int close);
            if (inner == null) { from = open + 1; continue; }
            var args = SplitTopLevelStr(inner);
            string repl;
            if (fn == "DATEADD" && args.Count == 3)
                repl = $"DATE_ADD({args[2].Trim()}, INTERVAL {args[1].Trim()} {MapPart(args[0])})";
            else if (fn == "DATEDIFF" && args.Count == 3)
                repl = $"TIMESTAMPDIFF({MapPart(args[0])}, {args[1].Trim()}, {args[2].Trim()})";
            else if (fn == "DATEPART" && args.Count == 2)
            {
                // Not every T-SQL date part is a valid MySQL EXTRACT unit. WEEKDAY and DAYOFYEAR must map to
                // the dedicated MySQL functions instead:
                //   DATEPART(WEEKDAY,x)   -> DAYOFWEEK(x)  — both return 1=Sunday..7=Saturday (T-SQL default
                //                            @@DATEFIRST=7), so the WHEN 1..7 day-name CASE stays correct.
                //   DATEPART(DAYOFYEAR,x) -> DAYOFYEAR(x)  — 1..366 (EXTRACT(DAY..) would wrongly give day-of-month).
                // Emit MySQL's dedicated scalar date functions (DAY(x)/MONTH(x)/YEAR(x)/...) rather than
                // EXTRACT(unit FROM x). They are equivalent, but crucially they contain NO "FROM" keyword — the
                // EXTRACT form's inner FROM was being mistaken for a query's FROM clause by the SELECT-assignment
                // rewrite ("SELECT @d = DATEPART(DAY,GETDATE())" -> broken "SELECT EXTRACT(DAY INTO v_d FROM ..").
                var part = args[0].Trim().ToLowerInvariant();
                var d = args[1].Trim();
                string dfn =
                    (part == "weekday" || part == "dw" || part == "w") ? "DAYOFWEEK" :
                    (part == "dayofyear" || part == "dy" || part == "y") ? "DAYOFYEAR" :
                    (part == "dd" || part == "d" || part == "day") ? "DAY" :
                    (part == "mm" || part == "m" || part == "month") ? "MONTH" :
                    (part == "yy" || part == "yyyy" || part == "year") ? "YEAR" :
                    (part == "hh" || part == "hour") ? "HOUR" :
                    (part == "mi" || part == "n" || part == "minute") ? "MINUTE" :
                    (part == "ss" || part == "s" || part == "second") ? "SECOND" :
                    (part == "wk" || part == "ww" || part == "week") ? "WEEK" :
                    (part == "qq" || part == "q" || part == "quarter") ? "QUARTER" : null;
                repl = dfn != null ? $"{dfn}({d})" : $"EXTRACT({MapPart(args[0])} FROM {d})";
            }
            else { from = close; continue; }
            body = body.Substring(0, start) + repl + body.Substring(close);
            from = start + repl.Length;
        }
    }
    return body;
}

// SELECT TOP n  ->  SELECT .. LIMIT n. Removes the "TOP n" (also "TOP (n)", "TOP @v", and drops a trailing
// "PERCENT"/"WITH TIES" which have no MySQL equivalent) and appends "LIMIT n" at the END of that SELECT
// statement — i.e. just before the statement-terminating ';' at paren depth 0, so LIMIT lands after any
// ORDER BY, as MySQL requires. A subquery "(SELECT TOP n ..)" ends at its own ')' rather than a ';'.
static string ConvertSelectTop(string body)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    while (true)
    {
        var m = System.Text.RegularExpressions.Regex.Match(body,
            @"\bSELECT\s+TOP\s*(?:\(\s*)?(\d+|@\w+|\w+)\s*(?:\))?\s*(PERCENT)?\s*(WITH\s+TIES)?\s+", IC);
        if (!m.Success) break;
        string n = m.Groups[1].Value;
        bool isPercent = m.Groups[2].Success;   // "TOP n PERCENT"
        // strip "TOP n .." back to just "SELECT "
        int afterSelect = m.Index + "SELECT".Length;
        body = body.Substring(0, afterSelect) + " " + body.Substring(m.Index + m.Length);

        // "TOP (100) PERCENT" is the T-SQL idiom for "all rows" (it only exists so a view may carry ORDER BY).
        // PERCENT has no MySQL LIMIT equivalent, so DROP the TOP entirely rather than emit a wrong row count.
        if (isPercent) continue;

        // Find the end of THIS select statement. It ends at the first ';' at depth 0, OR the ')' that closes
        // an enclosing subquery, OR — crucially — the next TOP-LEVEL STATEMENT KEYWORD. The keyword check is
        // essential because ConvertSelectTop runs BEFORE the statement-boundary pass inserts ';', and the
        // legacy T-SQL omits semicolons between statements; without it the scan runs into the following
        // statement and LIMIT lands in the wrong place (this dropped LIMIT off the first SELECT of
        // GetCostOf_Currency). The keyword must be at the START of a line (after a newline) at depth 0.
        var nextStmt = new System.Text.RegularExpressions.Regex(
            @"\r?\n\s*(SELECT|INSERT|UPDATE|DELETE|SET|IF|ELSEIF|ELSE|WHILE|RETURN|DECLARE|BEGIN|END|COMMIT|ROLLBACK|CALL|SIGNAL|LEAVE)\b", IC);
        int i = afterSelect, depth = 0; char q = '\0'; int insertAt = body.Length;
        for (; i < body.Length; i++)
        {
            char ch = body[i];
            if (q != '\0') { if (ch == q) q = '\0'; continue; }
            if (ch == '\'' || ch == '"') { q = ch; continue; }
            else if (ch == '(') depth++;
            else if (ch == ')') { if (depth == 0) { insertAt = i; break; } depth--; }
            else if (ch == ';' && depth == 0) { insertAt = i; break; }
            else if (depth == 0 && ch == '\n')
            {
                var mm = nextStmt.Match(body, i);
                if (mm.Success && mm.Index == i) { insertAt = i; break; }   // next statement starts here
            }
        }
        // don't double-append if a LIMIT is already right there
        var tail = body.Substring(afterSelect, insertAt - afterSelect);
        if (!System.Text.RegularExpressions.Regex.IsMatch(tail, @"\bLIMIT\b", IC))
            body = body.Substring(0, insertAt) + " LIMIT " + n + " " + body.Substring(insertAt);
    }
    return body;
}

static string ReadBalanced(string s, int open, out int closeAfter)
{
    closeAfter = open + 1;
    if (open >= s.Length || s[open] != '(') return null;
    int depth = 0; char q = '\0';
    for (int i = open; i < s.Length; i++)
    {
        char ch = s[i];
        if (q != '\0') { if (ch == q) q = '\0'; continue; }
        if (ch == '\'' || ch == '"') q = ch;
        else if (ch == '(') depth++;
        else if (ch == ')') { depth--; if (depth == 0) { closeAfter = i + 1; return s.Substring(open + 1, i - open - 1); } }
    }
    return null;
}

static List<string> SplitTopLevelStr(string s)
{
    var outp = new List<string>(); int depth = 0; char q = '\0'; var cur = new StringBuilder();
    foreach (var ch in s)
    {
        if (q != '\0') { cur.Append(ch); if (ch == q) q = '\0'; continue; }
        if (ch == '\'' || ch == '"') { q = ch; cur.Append(ch); continue; }
        if (ch == '(') depth++;
        else if (ch == ')') depth--;
        if (ch == ',' && depth == 0) { outp.Add(cur.ToString()); cur.Clear(); }
        else cur.Append(ch);
    }
    if (cur.Length > 0) outp.Add(cur.ToString());
    return outp;
}

// T-SQL  CONVERT(type[(len)], expr [, style])  ->  MySQL  CAST(expr AS mysqltype)
// Scans for CONVERT( and walks the argument list tracking paren depth and string literals, so an expression
// argument containing its own parens/commas (function calls, window functions, nested CONVERTs) is captured
// whole. The optional 3rd arg (the T-SQL style code, e.g. 103) has no MySQL equivalent and is DROPPED —
// which is what the old rules did too. Recurses so nested CONVERTs are handled inside-out.
static string ConvertTsqlConvert(string body)
{
    // Each iteration replaces the first CONVERT( with CAST(, so the count strictly decreases — the cap is only
    // an infinite-loop backstop. Some procs (e.g. SalaryCalculationTb_Insert) contain well over 100 CONVERTs,
    // so the cap must be far above any real proc's CONVERT count or the tail survives unconverted.
    for (int guard = 0; guard < 100000; guard++)
    {
        var m = System.Text.RegularExpressions.Regex.Match(body, @"\bCONVERT\s*\(",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (!m.Success) break;

        int open = m.Index + m.Length - 1;          // index of '('
        int i = open + 1, depth = 1;
        char quote = '\0';
        var args = new List<string>();
        var cur = new StringBuilder();
        for (; i < body.Length && depth > 0; i++)
        {
            char ch = body[i];
            if (quote != '\0') { cur.Append(ch); if (ch == quote) quote = '\0'; continue; }
            if (ch == '\'' || ch == '"') { quote = ch; cur.Append(ch); continue; }
            if (ch == '(') depth++;
            else if (ch == ')') { depth--; if (depth == 0) break; }
            if (depth == 1 && ch == ',') { args.Add(cur.ToString()); cur.Clear(); continue; }
            cur.Append(ch);
        }
        if (depth != 0) break;                       // unbalanced -> leave it for manual review
        args.Add(cur.ToString());
        int closeIdx = i;

        string replacement;
        if (args.Count < 2)
        {
            replacement = "CONVERT_UNPARSED(" + string.Join(",", args) + ")";   // shouldn't happen; keep visible
        }
        else
        {
            var type = args[0].Trim();
            var expr = args[1].Trim();
            // A 3rd argument is a T-SQL STYLE CODE. On a CHAR target it selects a DATE/TIME rendering, and
            // DROPPING it is a SILENT bug: CONVERT(VARCHAR(8), GETDATE(), 108) means "19:06:01", but a plain
            // CAST(NOW() AS CHAR) yields the whole "2026-07-16 19:06:01". Map the styles this database uses
            // (verified against SQL Server output). An unmapped style falls through to the old CAST.
            string styleFmt = null;
            if (args.Count >= 3 && System.Text.RegularExpressions.Regex.IsMatch(MapCastType(type), "^CHAR$"))
            {
                var style = args[2].Trim();
                // 108 -> hh:mi:ss (24h)            e.g. "19:06:01"
                // 0   -> h:miAM on a TIME source   e.g. "7:06PM"  (every call site here converts a TIME column;
                //        a DATETIME source would instead be "mon dd yyyy hh:miAM" — not used in this database)
                if (style == "108") styleFmt = "'%H:%i:%s'";
                else if (style == "0") styleFmt = "'%l:%i%p'";
            }
            if (styleFmt != null)
                replacement = $"DATE_FORMAT({expr}, {styleFmt})";
            else if (args.Count >= 3 && args[2].Trim() == "22"
                     && System.Text.RegularExpressions.Regex.IsMatch(MapCastType(type), "^CHAR$"))
                // 22 -> "mm/dd/yy  h:mi:ss AM": T-SQL RIGHT-ALIGNS the 12-hour hour in 2 chars (note the double
                // space for a single-digit hour), which no single DATE_FORMAT pattern reproduces.
                replacement = $"CONCAT(DATE_FORMAT({expr}, '%m/%d/%y '), LPAD(DATE_FORMAT({expr}, '%l'), 2, ' '), DATE_FORMAT({expr}, ':%i:%s %p'))";
            else
                replacement = $"CAST({expr} AS {MapCastType(type)})";
        }
        body = body.Substring(0, m.Index) + replacement + body.Substring(closeIdx + 1);
    }
    return body;
}

// T-SQL type name -> a type MySQL's CAST actually accepts.
static string MapCastType(string t)
{
    var bare = System.Text.RegularExpressions.Regex.Replace(t, @"\(.*\)", "").Trim().ToLowerInvariant();
    var size = System.Text.RegularExpressions.Regex.Match(t, @"\((.*)\)").Groups[1].Value.Trim();
    switch (bare)
    {
        case "varchar": case "nvarchar": case "char": case "nchar": case "text": case "ntext":
            return "CHAR";                                        // MySQL CAST has no VARCHAR
        case "int": case "integer": case "bigint": case "smallint": case "tinyint":
            return "SIGNED";
        case "bit": return "SIGNED";
        case "date": return "DATE";
        case "datetime": case "datetime2": case "smalldatetime": return "DATETIME";
        case "time": return "TIME";
        case "float": case "real": return "DECIMAL(18,6)";        // MySQL CAST has no FLOAT/DOUBLE target
        case "decimal": case "numeric": case "money": case "smallmoney":
            return size.Length > 0 && size.Contains(",") ? $"DECIMAL({size})" : "DECIMAL(18,2)";
        default: return "CHAR";
    }
}

static bool Balanced(string s)
{
    int d = 0;
    foreach (var ch in s) { if (ch == '(') d++; else if (ch == ')') { d--; if (d < 0) return false; } }
    return d == 0;
}

List<Col> LoadColumns(SqlConnection c, int oid)
{
    using var cmd2 = new SqlCommand(@"SELECT c.name, ty.name, c.max_length, c.precision, c.scale, c.is_nullable, c.is_identity
        FROM sys.columns c JOIN sys.types ty ON c.user_type_id=ty.user_type_id
        WHERE c.object_id=@o ORDER BY c.column_id", c);
    cmd2.Parameters.AddWithValue("@o", oid);
    using var r = cmd2.ExecuteReader();
    var list = new List<Col>();
    while (r.Read())
        list.Add(new Col { Name = r.GetString(0), Type = r.GetString(1), MaxLength = r.GetInt16(2), Precision = r.GetByte(3), Scale = r.GetByte(4), IsNullable = r.GetBoolean(5), IsIdentity = r.GetBoolean(6) });
    return list;
}

List<string> LoadPk(SqlConnection c, int oid)
{
    using var cmd2 = new SqlCommand(@"SELECT col.name FROM sys.indexes i
        JOIN sys.index_columns ic ON i.object_id=ic.object_id AND i.index_id=ic.index_id
        JOIN sys.columns col ON ic.object_id=col.object_id AND ic.column_id=col.column_id
        WHERE i.is_primary_key=1 AND i.object_id=@o ORDER BY ic.key_ordinal", c);
    cmd2.Parameters.AddWithValue("@o", oid);
    using var r = cmd2.ExecuteReader();
    var list = new List<string>();
    while (r.Read()) list.Add(r.GetString(0));
    return list;
}

List<Idx> LoadIndexes(SqlConnection c, int oid)
{
    using var cmd2 = new SqlCommand(@"SELECT i.name, i.is_unique, col.name, ic.key_ordinal FROM sys.indexes i
        JOIN sys.index_columns ic ON i.object_id=ic.object_id AND i.index_id=ic.index_id
        JOIN sys.columns col ON ic.object_id=col.object_id AND ic.column_id=col.column_id
        WHERE i.is_primary_key=0 AND i.type IN (1,2) AND i.name IS NOT NULL AND ic.is_included_column=0 AND i.object_id=@o
        ORDER BY i.name, ic.key_ordinal", c);
    cmd2.Parameters.AddWithValue("@o", oid);
    using var r = cmd2.ExecuteReader();
    var map = new Dictionary<string, Idx>();
    while (r.Read())
    {
        var name = r.GetString(0);
        if (!map.TryGetValue(name, out var ix)) { ix = new Idx { Name = name, IsUnique = r.GetBoolean(1) }; map[name] = ix; }
        ix.Columns.Add(r.GetString(2));
    }
    return map.Values.ToList();
}

List<Fk> LoadForeignKeys(SqlConnection c)
{
    using var cmd2 = new SqlCommand(@"SELECT fk.name, OBJECT_NAME(fk.parent_object_id), pc.name,
        OBJECT_NAME(fk.referenced_object_id), rc.name
        FROM sys.foreign_keys fk
        JOIN sys.foreign_key_columns fkc ON fk.object_id=fkc.constraint_object_id
        JOIN sys.columns pc ON fkc.parent_object_id=pc.object_id AND fkc.parent_column_id=pc.column_id
        JOIN sys.columns rc ON fkc.referenced_object_id=rc.object_id AND fkc.referenced_column_id=rc.column_id", c);
    using var r = cmd2.ExecuteReader();
    var list = new List<Fk>();
    while (r.Read()) list.Add(new Fk { Name = r.GetString(0), ParentTable = r.GetString(1), ParentCol = r.GetString(2), RefTable = r.GetString(3), RefCol = r.GetString(4) });
    return list;
}

// ---------------- helpers ----------------
// ---------------------------------------------------------------------------------------------------
//  PARAMETERIZED TABLE-VALUED FUNCTIONS  ->  inlined derived tables
//
//  MySQL has NO table-valued functions and a VIEW cannot take a parameter, so a caller's
//      FROM dbo.GET_TABLE_FOR_Costof_Main_saf(@Bid) AS a
//  has no direct translation. But a TVF is just a parameterised SELECT, so it can be INLINED as a
//  derived table with the caller's arguments substituted for the function's parameters:
//      FROM ( SELECT .. WHERE x = @Bid ) AS a
//  That is exact for ANY parameter (unlike the view+caller-filter trick, which is only sound when the
//  parameter filters on a GROUP BY key).
//
//  Inlining happens on the RAW T-SQL, before every other rewrite, so the injected SELECT then flows
//  through the SAME pipeline as the rest of the body (dbo. strip, ISNULL->IFNULL, CONVERT, concat, ...).
//
//  Only single-SELECT TVFs are inlined. A multi-statement TVF with IF branches / several INSERTs into the
//  return table (AccSafeActivityTb_GETBLANSE, GET_Total_Currency_CustomersTb, ...) is NOT a single query and
//  is deliberately left alone so it fails loudly into the manual bucket instead of being silently mistranslated.
// ---------------------------------------------------------------------------------------------------

static void LoadTvfs(SqlConnection src)
{
    if (TvfStore.Map.Count > 0) return;
    var defs = new List<(string Name, string Def)>();
    using (var cmd = new SqlCommand(@"SELECT o.name, m.definition FROM sys.objects o
        JOIN sys.sql_modules m ON o.object_id=m.object_id
        WHERE o.type IN ('TF','IF') AND o.is_ms_shipped=0", src))
    using (var r = cmd.ExecuteReader())
        while (r.Read()) defs.Add((r.GetString(0), r.GetString(1)));

    foreach (var (name, def) in defs)
    {
        var sel = ExtractTvfSelect(def);
        if (sel == null) continue;                       // multi-statement / conditional -> leave for manual
        var prms = LoadTvfParams(src, name);
        TvfStore.Map[name] = new TvfDef(name, prms, sel);
    }
}

static List<string> LoadTvfParams(SqlConnection src, string fn)
{
    var list = new List<string>();
    using var cmd = new SqlCommand(
        "SELECT p.name FROM sys.parameters p WHERE p.object_id=OBJECT_ID(@n) AND p.parameter_id>0 ORDER BY p.parameter_id", src);
    cmd.Parameters.AddWithValue("@n", "dbo." + fn);
    using var r = cmd.ExecuteReader();
    while (r.Read()) list.Add(r.GetString(0));           // keeps the leading '@'
    return list;
}

// The single SELECT a TVF returns, or null when the body is not a single query.
static string? ExtractTvfSelect(string def)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline;
    var body = StripSqlComments(def);   // string-aware: never eats a literal containing '--'

    // multi-statement: RETURNS @t TABLE(..) ... INSERT INTO @t <SELECT> ... RETURN
    var ins = System.Text.RegularExpressions.Regex.Matches(body, @"\bINSERT\s+INTO\s+@\w+", IC);
    if (ins.Count > 1) return null;                       // several inserts -> not a single query
    if (ins.Count == 1)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(body, @"\bIF\s+@", IC)) return null;  // conditional
        var after = body.Substring(ins[0].Index + ins[0].Length);
        // skip an optional explicit column list "(a,b,c)" right after the target table
        after = System.Text.RegularExpressions.Regex.Replace(after, @"^\s*\([^()]*(?:\([^()]*\)[^()]*)*\)", "", IC);
        var m = System.Text.RegularExpressions.Regex.Match(after, @"\bSELECT\b", IC);
        if (!m.Success) return null;
        var sel = after.Substring(m.Index);
        var endRet = System.Text.RegularExpressions.Regex.Match(sel, @"\bRETURN\b", IC);
        if (endRet.Success) sel = sel.Substring(0, endRet.Index);
        return StripTrailingEnd(sel);
    }

    // inline TVF: RETURNS TABLE AS RETURN ( SELECT .. )
    var mr = System.Text.RegularExpressions.Regex.Match(body, @"\bRETURNS\s+TABLE\b.*?\bRETURN\b", IC);
    if (!mr.Success) return null;
    var tail = body.Substring(mr.Index + mr.Length).Trim();
    if (tail.StartsWith("("))
    {
        var inner = ReadBalanced(tail, 0, out _);
        if (inner != null) return inner;
    }
    var ms = System.Text.RegularExpressions.Regex.Match(tail, @"\bSELECT\b", IC);
    return ms.Success ? StripTrailingEnd(tail.Substring(ms.Index)) : null;
}

// After a UNION, MySQL allows ORDER BY to reference only the OUTPUT column names of the union — never a
// table-qualified column. T-SQL is happy with "ORDER BY Typeid, x.AccBranchID" and MySQL answers
// "ERROR 1250: Table 'x' from one of the SELECTs cannot be used in ORDER clause".
// The qualified name is always also an output alias (the branches say "a.ID AS AccBranchID"), so dropping
// just the "alias." prefix is exact. Only ORDER BY clauses that sit at paren-depth 0 AFTER a depth-0 UNION
// are touched — an ORDER BY inside a subquery/derived table is left alone.
static string FixUnionOrderBy(string body)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    int depth = 0; char q = '\0'; bool sawUnion = false;
    for (int i = 0; i < body.Length; i++)
    {
        char ch = body[i];
        if (q != '\0') { if (ch == q) q = '\0'; continue; }
        if (ch == '\'' || ch == '"') { q = ch; continue; }
        if (ch == '(') { depth++; continue; }
        if (ch == ')') { depth--; continue; }
        if (ch == ';') { sawUnion = false; continue; }          // new statement
        if (depth != 0) continue;
        if (System.Text.RegularExpressions.Regex.IsMatch(body.Substring(i, Math.Min(6, body.Length - i)), @"^UNION\b", IC))
        { sawUnion = true; i += 4; continue; }
        if (!sawUnion) continue;
        var m = System.Text.RegularExpressions.Regex.Match(body.Substring(i), @"^ORDER\s+BY\b", IC);
        if (!m.Success) continue;
        // clause runs to the statement terminator
        int start = i + m.Length, end = start;
        int d2 = 0;
        while (end < body.Length && !(d2 == 0 && body[end] == ';'))
        {
            if (body[end] == '(') d2++; else if (body[end] == ')') d2--;
            end++;
        }
        var clause = body.Substring(start, end - start);
        var fixedClause = System.Text.RegularExpressions.Regex.Replace(clause, @"\b[A-Za-z_]\w*\s*\.\s*(\w+)", "$1");
        body = body.Substring(0, start) + fixedClause + body.Substring(end);
        i = start + fixedClause.Length;
        sawUnion = false;
    }
    return body;
}

// T-SQL tolerates  SELECT @v = expr FROM .. GROUP BY k  returning MANY rows: it assigns each row in
// turn and the LAST one silently wins. MySQL's equivalent  SELECT expr INTO v .. GROUP BY k  raises
//     ERROR 1172 (42000): Result consisted of more than one row
// and ABORTS the routine — so a faithful statement-for-statement translation inherits a landmine that
// only detonates on the data that actually has >1 group.
//
// WHICH row does SQL Server leave in the variable? "Last row wins" is undefined in the standard, so it
// was MEASURED against the live SQL Server database. Two independent cases, both landing on the LAST
// group in ASCENDING group-key order:
//   EMP_GetADVPMNTTtoalsIndivdual(38): groups OverAllVal 1000 -> 750.000, 2100 -> 600.000; SQL Server
//                                      returns 600.000  => the higher (last) key
//   Get_AllBranchSafesVal(3):          EmpSafes groups AccParent 1010401 -> 0.000, 1010403 -> 33943.990;
//                                      SQL Server returns 33944.414 with MainSafes 0.424
//                                      => EmpSafes 33943.990, the higher (last) key
// So "ORDER BY <keys> DESC LIMIT 1" reproduces the measured value exactly AND is deterministic (better
// than the source, which is order-dependent).
//
// Fires ONLY when the GROUP BY is at the statement's own top level. A GROUP BY inside a derived table
// belongs to that subquery — appending an outer ORDER BY on its key would be wrong and usually invalid.
// Statements that already carry their own ORDER BY or LIMIT are left alone.
static string FixMultiRowSelectInto(string body)
{
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    var outp = new System.Text.StringBuilder();
    foreach (var (stmt, term) in SplitStatementsTopLevel(body))
    {
        outp.Append(FixOneSelectInto(stmt, IC)).Append(term);
    }
    return outp.ToString();

    static string FixOneSelectInto(string stmt, System.Text.RegularExpressions.RegexOptions IC)
    {
        // BEFORE the "^SELECT" guard below: a lookup often sits inside an IF, so the statement text starts
        // with IF rather than SELECT and would otherwise never be examined.
        stmt = FixKnownNonUniqueLookup(stmt, IC);
        if (!System.Text.RegularExpressions.Regex.IsMatch(stmt.TrimStart(), @"^SELECT\b", IC)) return stmt;
        // locate the top-level GROUP BY / ORDER BY / LIMIT / HAVING / INTO / FROM / UNION positions
        int depth = 0; char q = '\0';
        int gb = -1, gbEnd = -1, ob = -1, lim = -1, having = -1, into = -1, from = -1, union = -1;
        for (int i = 0; i < stmt.Length; i++)
        {
            char ch = stmt[i];
            if (q != '\0') { if (ch == q) q = '\0'; continue; }
            if (ch == '\'' || ch == '"' || ch == '`') { q = ch; continue; }
            if (ch == '(') { depth++; continue; }
            if (ch == ')') { depth--; continue; }
            if (depth != 0) continue;
            var rest = stmt.Substring(i);
            if (gb < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^GROUP\s+BY\b", IC))
            { var m = System.Text.RegularExpressions.Regex.Match(rest, @"^GROUP\s+BY\b", IC); gb = i; gbEnd = i + m.Length; continue; }
            if (ob < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^ORDER\s+BY\b", IC)) { ob = i; continue; }
            if (lim < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^LIMIT\b", IC)) { lim = i; continue; }
            if (having < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^HAVING\b", IC)) { having = i; continue; }
            if (into < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^INTO\s+[A-Za-z_@]", IC)) { into = i; continue; }
            if (from < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^FROM\b", IC)) { from = i; continue; }
            if (union < 0 && System.Text.RegularExpressions.Regex.IsMatch(rest, @"^UNION\b", IC)) { union = i; continue; }
        }
        // not an INTO-lookup, or the row is already pinned (LIMIT / ORDER BY present) -> leave it exactly as is
        if (into < 0 || ob >= 0 || lim >= 0) return stmt;
        if (gb >= 0)
        {
            // GROUP BY with no ORDER BY: T-SQL's "SELECT @x = .. GROUP BY" keeps the HIGHEST group key row.
            // Reproduce it deterministically with ORDER BY <keys> DESC LIMIT 1.
            int keysEnd = (having > gbEnd) ? having : stmt.Length;
            var keys = stmt.Substring(gbEnd, keysEnd - gbEnd).Trim();
            if (keys.Length == 0) return stmt;
            var parts = SplitTopLevel(keys, ',');
            if (parts.Count == 0) return stmt;
            var order = string.Join(", ", parts.Select(p => p.Trim() + " DESC"));
            return stmt.TrimEnd() + "\n ORDER BY " + order + " LIMIT 1";
        }
        // No GROUP BY / ORDER BY / LIMIT: a plain "SELECT <expr(s)> INTO <var(s)> FROM <table(s)> WHERE .."
        // (a JOIN is common — FixKnownNonUniqueLookup deliberately skips those). T-SQL tolerates >1 matching
        // row and keeps one; MySQL raises ERROR 1172 and ABORTS the routine (this crashed Get_CurrencyPower).
        // A bare LIMIT 1 makes it non-fatal and is EXACTLY faithful wherever the selected value(s) are
        // invariant across the matching rows — the common case, and verified so for Get_CurrencyPower (0 of 57
        // param-sets have a conflicting CurrencyPower). Guard out the forms where it is wrong or pointless:
        //   * no FROM         -> a "SELECT <expr> INTO v" already yields exactly one row
        //   * a top-level UNION-> "INTO" placement/semantics differ; leave for hand-review
        //   * a pure aggregate (SUM/COUNT/MAX/MIN/AVG/GROUP_CONCAT in the select list, no GROUP BY) -> one row already
        if (from < 0 || union >= 0) return stmt;
        var selList = stmt.Substring(0, into);
        if (System.Text.RegularExpressions.Regex.IsMatch(selList, @"\b(SUM|COUNT|MAX|MIN|AVG|GROUP_CONCAT)\s*\(", IC)) return stmt;
        return stmt.TrimEnd() + " LIMIT 1";
    }
}

// Lookups whose WHERE is NOT unique IN THE DATA, so a single-row "SELECT .. INTO" over them returns
// several rows: T-SQL keeps the last one, MySQL raises ERROR 1172 and ABORTS the routine.
//
// These cannot be found by reading the code — the statement looks perfectly ordinary and whether it breaks
// depends on the table's CONTENTS. They come from migration/find_multirow_select_into.py, which probes the
// live data. Re-run it after any data reload; if it reports a new table, add it here.
//
//   CurrencyMainTb    WHERE IsDefault = 0        -> 8 rows. CoBranch_Insert and CountriesTb_Insert
//                                                   therefore failed on EVERY call, not just sometimes.
//   AdvancePaymentTb  WHERE EMPID = ?            -> employee 38 has two advances.
//   BranchRatesTb     WHERE FBranchID/SBranchID  -> 51 branches appear more than once.
//   TB_Users          WHERE EMPID = ?            -> 35 users share EMPID = 0, the "no employee" sentinel.
//
// Which row does SQL Server keep? Measured five times (EMP_GetADVPMNTTtoalsIndivdual, Get_AllBranchSafesVal,
// the BranchRatesTb rate pairs, AdvancePaymentTb EMPID=38, CurrencyMainTb IsDefault=0) — always the
// HIGHEST-ID row. So "ORDER BY <pk> DESC LIMIT 1" reproduces it exactly and is deterministic.
//
// Appending it is a NO-OP wherever the predicate happens to be unique (one row either way), so it can only
// fix, never change, a working lookup.
static string FixKnownNonUniqueLookup(string stmt, System.Text.RegularExpressions.RegexOptions IC)
{
    var known = new (string Table, string Pk)[]
    {
        ("CurrencyMainTb", "ID"), ("AdvancePaymentTb", "ID"), ("BranchRatesTb", "ID"),
        ("TB_Users", "USID"),     // note: its key is USID, not ID
    };
    if (!System.Text.RegularExpressions.Regex.IsMatch(stmt, @"\bINTO\s+v_\w+", IC)) return stmt;
    if (System.Text.RegularExpressions.Regex.IsMatch(stmt, @"\b(LIMIT|GROUP\s+BY|ORDER\s+BY)\b", IC)) return stmt;
    // an aggregate with no GROUP BY already collapses to exactly one row
    var head = stmt.Substring(0, Math.Max(0, stmt.IndexOf("INTO", StringComparison.OrdinalIgnoreCase) is int k && k > 0 ? k : 0));
    if (System.Text.RegularExpressions.Regex.IsMatch(head, @"\b(SUM|COUNT|MAX|MIN|AVG|GROUP_CONCAT)\s*\(", IC)) return stmt;

    foreach (var (tbl, pk) in known)
    {
        // single table only: a JOIN changes which rows come back and is out of scope here
        var m = System.Text.RegularExpressions.Regex.Match(stmt,
            @"\bFROM\s+`?" + System.Text.RegularExpressions.Regex.Escape(tbl) + @"`?(?:\s+(?:AS\s+)?`?(\w+)`?)?\s*\bWHERE\b", IC);
        if (!m.Success) continue;
        if (System.Text.RegularExpressions.Regex.IsMatch(stmt, @"\bJOIN\b", IC)) continue;
        var alias = m.Groups[1].Success && !m.Groups[1].Value.Equals("WHERE", StringComparison.OrdinalIgnoreCase)
            ? m.Groups[1].Value : tbl;
        return stmt.TrimEnd() + "\n ORDER BY " + alias + "." + pk + " DESC LIMIT 1";
    }
    return stmt;
}

// Split a routine body into statements at top-level ';' (quote- and paren-aware), returning each
// statement together with the terminator that followed it so the body can be rebuilt byte-for-byte.
static IEnumerable<(string stmt, string term)> SplitStatementsTopLevel(string body)
{
    int depth = 0; char q = '\0'; int start = 0;
    for (int i = 0; i < body.Length; i++)
    {
        char ch = body[i];
        if (q != '\0') { if (ch == q) q = '\0'; continue; }
        if (ch == '\'' || ch == '"' || ch == '`') { q = ch; continue; }
        if (ch == '(') { depth++; continue; }
        if (ch == ')') { depth--; continue; }
        if (ch == ';' && depth == 0)
        {
            yield return (body.Substring(start, i - start), ";");
            start = i + 1;
        }
    }
    if (start < body.Length) yield return (body.Substring(start), "");
}

// ---- T-SQL SOURCE PATCHES ---------------------------------------------------------------------------
// A handful of procs use constructs the converter deliberately refuses to guess at — chiefly the BRACELESS
// IF whose body is a multi-line statement. (A regex cannot tell an UPDATE's own SET clause from the start
// of the next statement; that needs the curKw/clauseCont state machine in the ';'-insertion pass. The
// attempt to do it with a regex truncated "UPDATE t SET .." to "UPDATE t" and was reverted.)
//
// The alternative to a converter hack is hand-transcribing 300-1200 line money procs, where every retyped
// line is a chance to mistype a column in a positional INSERT. So instead we patch the T-SQL SOURCE
// MINIMALLY — normally just adding the BEGIN/END the author omitted — and let the SAME proven converter,
// and the same diff/audit harness, do the actual translation.
//
// RULES for a file in migration/srcpatch/<ProcName>.sql:
//   * it is still T-SQL, and must be SEMANTICALLY IDENTICAL to what SQL Server executes today;
//   * keep the diff against the original as small as possible, so it stays reviewable;
//   * it is NOT a place to change behaviour — that belongs in a hand-port under migration/proof/.
static string ApplySrcPatch(string name, string def)
{
    // (a local variable, not a static field: this file uses top-level statements, so these helpers are
    //  local functions and cannot carry static state.)
    var SrcPatchDir = System.IO.Path.GetFullPath(
        System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "srcpatch"));
    try
    {
        if (!System.IO.Directory.Exists(SrcPatchDir)) return def;
        // match case-insensitively: sys.objects casing and the file name need not agree
        foreach (var f in System.IO.Directory.GetFiles(SrcPatchDir, "*.sql"))
            if (string.Equals(System.IO.Path.GetFileNameWithoutExtension(f), name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"  [srcpatch] {name}");
                return System.IO.File.ReadAllText(f);
            }
    }
    catch { /* a missing/unreadable patch dir must never break the run */ }
    return def;
}

// Strip T-SQL comments WITHOUT touching string literals or bracketed identifiers.
//
// The naive Regex.Replace(@"--[^\n]*", "") this replaces also ate the INSIDE of a literal. These procs use
// '---' as a placeholder value:
//     ELSE '---'  END   SalePriceType
// and the regex turned that into  ELSE '  — truncating the literal and swallowing the rest of the CASE, which
// is how NewCurrencyBuyandSale_CRUD failed to create. Any literal containing '--' was at risk, and where the
// literal merely came out SHORTER rather than unbalanced the damage would have been silent.
static string StripSqlComments(string s)
{
    // NORMALISE LINE ENDINGS FIRST. sys.sql_modules stores whatever the author saved — usually CRLF, but at
    // least one proc (SendTypeTB_Roll_luckbedit) is stored with CR-ONLY terminators, which made every
    // line-anchored rule below see the whole body as a single line. Others leave a stray CR behind when a
    // rule joins two lines on '\n', which then shows up as a phantom byte difference between two servers
    // (GetCostOf_Currency differed from its production copy by exactly one 0x0D). T-SQL treats CR, LF and
    // CRLF all as whitespace, so collapsing them to '\n' changes nothing and makes every later rule reliable.
    s = s.Replace("\r\n", "\n").Replace('\r', '\n');
    var sb = new System.Text.StringBuilder(s.Length);
    for (int i = 0; i < s.Length; i++)
    {
        char ch = s[i];
        if (ch == '\'')                       // string literal; '' is an escaped quote
        {
            sb.Append(ch);
            for (i++; i < s.Length; i++)
            {
                sb.Append(s[i]);
                if (s[i] != '\'') continue;
                if (i + 1 < s.Length && s[i + 1] == '\'') { sb.Append(s[++i]); continue; }
                break;
            }
            continue;
        }
        if (ch == '[')                        // bracketed identifier, e.g. [Order--Details]
        {
            sb.Append(ch);
            for (i++; i < s.Length; i++) { sb.Append(s[i]); if (s[i] == ']') break; }
            continue;
        }
        if (ch == '-' && i + 1 < s.Length && s[i + 1] == '-')
        {
            while (i < s.Length && s[i] != '\n') i++;   // drop to end of line
            if (i < s.Length) sb.Append('\n');          // keep the newline: many later rules are line-anchored
            continue;
        }
        if (ch == '/' && i + 1 < s.Length && s[i + 1] == '*')
        {
            int depth = 1; i += 2;                      // T-SQL block comments NEST
            while (i < s.Length && depth > 0)
            {
                if (s[i] == '/' && i + 1 < s.Length && s[i + 1] == '*') { depth++; i += 2; continue; }
                if (s[i] == '*' && i + 1 < s.Length && s[i + 1] == '/') { depth--; i += 2; continue; }
                i++;
            }
            i--;                                        // the for-loop's i++ lands just past the close
            sb.Append(' ');
            continue;
        }
        sb.Append(ch);
    }
    return sb.ToString();
}

// T-SQL "SELECT alias = expr" -> "SELECT expr AS alias", applied ONLY inside a real select list.
//
// Why not one whole-body regex: an UPDATE..SET clause is "col = expr, col = expr" — identical shape,
// opposite meaning. A broad regex flipped those into "expr AS col" and broke 60+ procs. So this walks the
// body, finds each top-level SELECT, works only in the span from that SELECT to its matching FROM (the
// select list), and rewrites "name = <simple expr>" items there. Anything without a FROM (an UPDATE, a
// SET, a SELECT..INTO with no table) is never touched.
//
// "SELECT x = 'y'" is valid MySQL *syntax* (a comparison), so the proc CREATEs and only fails at runtime
// with "Unknown column 'x'"; this is the only place that can catch it.
static string FixSelectListAliasAssign(string body, System.Text.RegularExpressions.RegexOptions IC)
{
    // one select-list item: "name = <string|number|dotted-ident|CASE..END|NULL>" that ENDS the item
    // (next token is a comma or the list's FROM). @vars are excluded (\w+ won't match the '@'), so
    // "SELECT @a = 'x'" variable assignments are left alone.
    var item = new System.Text.RegularExpressions.Regex(
        @"(^|,)(\s*)([A-Za-z_]\w*)\s*=\s*(CASE\b[\s\S]+?\bEND|[Nn]?'(?:[^']|'')*'|-?\d+(?:\.\d+)?|[A-Za-z_]\w*(?:\.\w+)?|NULL)(?=\s*(?:,|$))",
        IC);

    var sb = new System.Text.StringBuilder(body.Length);
    int pos = 0;
    var selRe = new System.Text.RegularExpressions.Regex(@"\bSELECT\b", IC);
    while (true)
    {
        var mSel = selRe.Match(body, pos);
        if (!mSel.Success) { sb.Append(body, pos, body.Length - pos); break; }
        // copy up to and including SELECT
        sb.Append(body, pos, mSel.Index + mSel.Length - pos);
        int listStart = mSel.Index + mSel.Length;
        // find this SELECT's own FROM at paren-depth 0 (so a subquery's FROM doesn't end the outer list);
        // a SELECT with no depth-0 FROM (SELECT .. INTO var, SELECT scalar) has no list to rewrite.
        int depth = 0; char q = '\0'; int caseDepth = 0; int fromAt = -1;
        for (int i = listStart; i < body.Length; i++)
        {
            char c = body[i];
            if (q != '\0') { if (c == q) q = '\0'; continue; }
            if (c == '\'' || c == '"' || c == '`') { q = c; continue; }
            if (c == '(') { depth++; continue; }
            if (c == ')') { depth--; continue; }
            if (depth != 0) continue;
            // Skip CASE..END expressions in the select list: their WHEN/THEN/ELSE/END would otherwise be
            // mistaken for statement boundaries. Track nesting so a nested CASE is handled too.
            if (System.Text.RegularExpressions.Regex.IsMatch(body.Substring(i, Math.Min(5, body.Length - i)), @"^CASE\b", IC)) { caseDepth++; i += 3; continue; }
            if (caseDepth > 0)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(body.Substring(i, Math.Min(4, body.Length - i)), @"^END\b", IC)) { caseDepth--; i += 2; }
                continue;
            }
            if (c == ';') break;   // statement ends before any FROM -> this SELECT has no list to rewrite
            if (System.Text.RegularExpressions.Regex.IsMatch(body.Substring(i, Math.Min(6, body.Length - i)), @"^FROM\b", IC)) { fromAt = i; break; }
            // Any statement keyword before FROM means this SELECT has no "SELECT..FROM" list here (e.g.
            // "SELECT LAST_INSERT_ID() AS x;" followed by an UPDATE..SET). Stopping here is what prevents the
            // walker from running past the query and flipping the next statement's UPDATE SET assignments.
            if (System.Text.RegularExpressions.Regex.IsMatch(
                    body.Substring(i, Math.Min(10, body.Length - i)),
                    @"^(SELECT|UPDATE|INSERT|DELETE|SET|IF|ELSEIF|ELSE|DECLARE|WHILE|CALL|BEGIN|END|SIGNAL|LEAVE|RETURN|COMMIT|ROLLBACK|INTO)\b", IC))
                break;
        }
        if (fromAt < 0) { pos = listStart; continue; }   // no list here; resume scanning after SELECT
        var list = body.Substring(listStart, fromAt - listStart);
        // within the list, flip only depth-0 items (a comma inside a function call is not an item break)
        list = RewriteAliasItems(list, item);
        sb.Append(list);
        pos = fromAt;                                    // continue from FROM (its own SELECTs handled later)
    }
    return sb.ToString();

    static string RewriteAliasItems(string list, System.Text.RegularExpressions.Regex item)
    {
        // split the select list on TOP-LEVEL commas, flip each piece, rejoin — so a comma inside
        // CASE/func args never counts as an item boundary.
        var pieces = SplitTopLevel(list, ',');
        for (int i = 0; i < pieces.Count; i++)
        {
            var m = item.Match("," + pieces[i]);          // prime with a leading comma so ^| matches
            if (m.Success && m.Index == 0)
                pieces[i] = m.Groups[2].Value + m.Groups[4].Value + " AS " + m.Groups[3].Value
                          + pieces[i].Substring(m.Length - 1);
        }
        return string.Join(",", pieces);
    }
}

static string StripTrailingEnd(string s)
{
    s = s.Trim();
    // drop a trailing block END (and any stray ';') left over from the function body
    s = System.Text.RegularExpressions.Regex.Replace(s, @"(?is)\s*;?\s*\bEND\b\s*;?\s*$", "");
    return s.Trim();
}

// Replace every  <fn>(arg, ...) [AS] alias  with the function's SELECT, parameters substituted.
static string InlineTvfCalls(string body)
{
    if (TvfStore.Map.Count == 0) return body;
    var IC = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
    foreach (var tvf in TvfStore.Map.Values)
    {
        if (tvf.Prms.Count == 0) continue;                // parameterless ones are handled as VIEWs
        var rx = new System.Text.RegularExpressions.Regex(
            @"(?:\[?dbo\]?\s*\.\s*)?\[?" + System.Text.RegularExpressions.Regex.Escape(tvf.Name) + @"\]?\s*\(", IC);
        // `from` advances PAST each insertion. Restarting at 0 would re-match the function name if it also
        // occurs inside the text we just injected, re-inlining it forever (body grows every pass -> hang).
        int from = 0;
        for (int guard = 0; guard < 200 && from < body.Length; guard++)
        {
            var m = rx.Match(body, from);
            if (!m.Success) break;
            int open = m.Index + m.Length - 1;
            var argsRaw = ReadBalanced(body, open, out int close);
            if (argsRaw == null) { from = open + 1; continue; }
            var args = SplitTopLevelStr(argsRaw);
            if (args.Count != tvf.Prms.Count) { from = close; continue; }  // arity mismatch -> leave alone

            var sel = tvf.Select;
            // longest parameter name first, so @Bid does not clobber @BidX
            foreach (var (p, a) in tvf.Prms.Select((p, i) => (p, args[i]))
                                           .OrderByDescending(x => x.p.Length))
                sel = System.Text.RegularExpressions.Regex.Replace(
                    sel, System.Text.RegularExpressions.Regex.Escape(p) + @"(?![A-Za-z0-9_])", "(" + a.Trim() + ")", IC);

            var repl = "(" + sel + ")";
            // EVERY derived table needs an alias in MySQL. T-SQL is happy with a bare
            // "FROM dbo.fn(@a,@b)" (no AS), which inlines to "FROM (SELECT ..)" and then fails with
            // "Every derived table must have its own alias". Append one only when the call site has none.
            var after = body.Substring(close);
            var mAlias = System.Text.RegularExpressions.Regex.Match(after,
                @"^\s*(?:AS\s+)?([A-Za-z_]\w*)", IC);
            bool hasAlias = mAlias.Success &&
                !System.Text.RegularExpressions.Regex.IsMatch(mAlias.Groups[1].Value,
                    @"^(WHERE|GROUP|ORDER|HAVING|INNER|LEFT|RIGHT|FULL|CROSS|JOIN|ON|UNION|LIMIT|SELECT|INSERT|UPDATE|DELETE|SET|END|AND|OR)$", IC);
            if (!hasAlias) repl += " AS tvf_" + tvf.Name;
            body = body.Substring(0, m.Index) + repl + body.Substring(close);
            from = m.Index + repl.Length;                 // resume AFTER the inlined block
        }
    }
    return body;
}

static SqlConnection Open(string cs) { var c = new SqlConnection(cs); c.Open(); return c; }
static MySqlConnection OpenMy(string cs) { var c = new MySqlConnection(cs); c.Open(); return c; }
static void Exec(MySqlConnection c, string sql) { using var cmd = new MySqlCommand(sql, c); cmd.ExecuteNonQuery(); }
static object Scalar(IDbConnection c, string sql) { using var cmd = c.CreateCommand(); cmd.CommandText = sql; return cmd.ExecuteScalar() ?? 0; }
static string Trunc(string s, int n) => s.Length <= n ? s : s.Substring(0, n);
static IEnumerable<string> SplitStatements(string sql) =>
    sql.Split(';').Select(s => s.Trim()).Where(s => s.Length > 0 && !s.StartsWith("SET FOREIGN_KEY_CHECKS"));

record Cfg { public string SqlServer { get; init; } = ""; public string MySql { get; init; } = ""; }

// Target MySQL schema name for the emitted "USE <db>;" header. Set once at startup from the MySql
// connection string, so the same tool works for any database (was hardcoded to the shipping DB).
static class Target { public static string Db = ""; }
class Table { public string Schema = ""; public string Name = ""; public int ObjectId; }
class Col { public string Name = ""; public string Type = ""; public int MaxLength; public int Precision; public int Scale; public bool IsNullable; public bool IsIdentity; }
class Idx { public string Name = ""; public bool IsUnique; public List<string> Columns = new(); }
class Prm { public Col Col = new(); public bool IsOut; public bool IsTvp; public string TvpType = ""; }
class Fk { public string Name = ""; public string ParentTable = ""; public string ParentCol = ""; public string RefTable = ""; public string RefCol = ""; }

// Parameterized table-valued function: its ordered parameter names and the single SELECT it returns.
record TvfDef(string Name, List<string> Prms, string Select);
static class TvfStore { public static readonly Dictionary<string, TvfDef> Map = new(StringComparer.OrdinalIgnoreCase); }
