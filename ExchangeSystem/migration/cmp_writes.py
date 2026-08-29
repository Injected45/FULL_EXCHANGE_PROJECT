"""
Static write-path check:  does each MySQL proc write the SAME columns, to the SAME table, in the SAME order
as its T-SQL original?

Why static: the migrator's `hardverify` mode EXECUTES each write proc against SQL Server to compare results.
That is not safe here — the T-SQL procs reference `ExSyAccounts2026` by absolute name (and via SYNONYMs), so
even running them against a snapshot copy would still mutate the REAL 506k-row ledger. So instead of executing
anything, we compare the INSERT/UPDATE targets textually.

What it catches: a column dropped, added, or REORDERED by the converter. The VALUES list is positional — one
shifted column silently writes the debit into the credit field, and nothing errors.

Inputs (produced by dump_bodies.sh):
    ss_bodies.txt   ###ProcName###<T-SQL body>      (one per line)
    my_bodies.txt   ###ProcName###<MySQL body>      (mysql -N escapes newlines/tabs as \\n \\t)
"""
import io
import re
import sys

# The procs are full of ARABIC column aliases (AS 'الرمز'). Printing them through the default Windows console
# code page (cp1252) raises UnicodeEncodeError, so force UTF-8 on stdout.
try:
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
except Exception:
    pass

BS = chr(92)  # backslash — kept out of literals to avoid escaping confusion


def strip_comments(body, escaped_newlines):
    """Remove -- line comments and /* */ block comments.

    MUST run before newlines are flattened to spaces. The T-SQL bodies contain
        INSERT INTO X
            (
                -- ID - column value is auto-generated
                SafeID,
    and if the newline is flattened first, the comment fuses onto the next column name and every one of these
    procs reports a bogus mismatch. (That produced 13 false positives on the first run.)
    """
    if escaped_newlines:
        # mysql -N emits the two-character escapes; turn them into real newlines first
        body = body.replace(BS + "n", "\n").replace(BS + "t", " ").replace(BS + "r", " ")
    body = re.sub(r"/\*.*?\*/", " ", body, flags=re.S)
    body = re.sub(r"--[^\n]*", " ", body)
    return body.replace("\n", " ").replace("\t", " ")


def load(path, escaped_newlines=False):
    txt = io.open(path, encoding="utf-8", errors="replace").read()
    parts = re.split(r"###([A-Za-z_][\w]*)###", txt)
    return {
        parts[i]: strip_comments(parts[i + 1], escaped_newlines)
        for i in range(1, len(parts) - 1, 2)
    }


# '@' is in the table-name class on purpose: T-SQL writes into TABLE VARIABLES ("INSERT INTO @SID (id) ..").
# Without it those INSERTs matched nothing, so the whole statement was INVISIBLE to this check — a blind spot
# (a port that filled the WRONG staging table would not have been noticed).
# '#' is in the class for the same reason '@' is: T-SQL also stages into LOCAL TEMP TABLES
# ("INSERT INTO #SendTypeTB_TMB (SID, SName) .."). Without '#' the regex stopped at the '#' and matched
# nothing, so every such INSERT was INVISIBLE on the T-SQL side — the check then saw "T-SQL writes nothing,
# MySQL writes something" and, worse, could not have noticed a port that filled the WRONG staging table.
INS = re.compile(
    r"INSERT\s+(?:INTO\s+)?([@#\[\]\w.`]+)\s*\(([^;()]*?)\)\s*(?:VALUES|SELECT|\()",
    re.I | re.S,
)


def norm_tbl(t):
    t = t.replace("[", "").replace("]", "").replace("`", "").split(".")[-1].lower()
    # A T-SQL table VARIABLE "@SID" becomes a MySQL session temp table "tmp_SID" (and a TVP parameter becomes
    # "tvp_<name>") — the same staging object under a required new name. Strip the marker so the two spellings
    # compare equal. This does NOT blunt the canary: the CATEGORYTYPESTB_Insert bug wrote to tvp_Type where the
    # T-SQL wrote to the REAL table CATEGORYTYPESDETAILSTB, and "type" != "categorytypesdetailstb" still differs.
    # '#' marks a T-SQL LOCAL TEMP TABLE ("#SendTypeTB_TMB"), which ports to a MySQL TEMPORARY
    # TABLE ("tmp_SendTypeTB_TMB") — same staging object, required new spelling. Strip both markers.
    t = t.lstrip("@").lstrip("#")
    t = re.sub(r"^(?:tmp|tvp)_", "", t)
    return t


def norm_cols(s):
    out = []
    for x in s.split(","):
        x = x.strip().replace("[", "").replace("]", "").replace("`", "")
        x = x.split(".")[-1].strip()
        if x:
            out.append(x.lower())
    return out


# ---- column-ownership map (for multi-table UPDATE target resolution) ---------------------------------
# A T-SQL "UPDATE RealTbl SET .. FROM SomeView AS A JOIN RealTbl AS b" has ONE explicit target (RealTbl).
# MySQL has no single target in a multi-table UPDATE: "UPDATE SomeView A JOIN RealTbl b SET col=.." updates
# whichever joined table OWNS `col`. Taking the first table as the target (the old heuristic) then wrongly
# reports the view as the target and false-positives against the T-SQL. So for a MULTI-table UPDATE we resolve
# the target the way MySQL actually does: the listed table that owns the SET columns. col_owner.tsv is dumped
# from information_schema ("table<TAB>column", lowercased) next to this script; absent -> we fall back cleanly.
# Temp TVP tables (tvp_*) are NOT in the map, so the CATEGORYTYPESTB_Insert-style "wrote to tvp_ temp table"
# bug is still caught (its single-table UPDATE never enters this path, and tvp_ owns nothing here anyway).
COL_OWNER = {}   # colname_lower -> set(table_lower)
try:
    for _ln in io.open("col_owner.tsv", encoding="utf-8", errors="replace"):
        # mysql -N (batch mode) escapes a real TAB in the data as the two characters "\t"; normalise both forms.
        _ln = _ln.replace(BS + "t", "\t")
        if "\t" in _ln:
            _t, _c = _ln.rstrip("\n").split("\t", 1)
            COL_OWNER.setdefault(_c.strip().lower(), set()).add(_t.strip().lower())
except (OSError, IOError):
    pass


def candidate_tables(tpart):
    """Table names referenced in a multi-table UPDATE clause (before SET), in order.

    Splits on JOIN keywords and top-level commas and takes the first identifier of each segment (the table,
    not its alias). Names are normalised (brackets/backticks/schema-qualifier stripped, lowercased).
    """
    segs = re.split(r"\b(?:INNER|LEFT|RIGHT|FULL|CROSS|OUTER|JOIN)\b", tpart, flags=re.I)
    tables = []
    for seg in segs:
        for piece in seg.split(","):
            piece = piece.strip()
            if not piece:
                continue
            first = re.split(r"\s+", piece)[0]
            tbl = norm_tbl(first)
            if re.fullmatch(r"\w+", tbl or ""):
                tables.append(tbl)
    return tables


def inserts(body):
    res = []
    for m in INS.finditer(body):
        cols = norm_cols(m.group(2))
        if cols:
            res.append((norm_tbl(m.group(1)), tuple(cols)))
    return res


# UPDATE <table-part> SET <sets> [FROM <src>] [WHERE ..]
# <table-part> is deliberately loose: it must swallow the T-SQL alias form ("UPDATE a"), the MySQL JOIN form
# ("UPDATE Invoices a INNER JOIN Pay b ON ..") and the MySQL multi-table form ("UPDATE t1, tvp_x AS a").
# The table-part must stay INSIDE the UPDATE statement: it may not cross ';' nor any other statement keyword.
# Without that guard the lazy .*? happily runs from an UPDATE to a much later "SET @var = .." variable
# assignment and reports a target of '='.
# \bUPDATE\b — the word boundary is ESSENTIAL: this codebase has parameters called @IsUpdate / p_IsUpdate,
# and without it the regex matches the "Update" inside the PARAMETER NAME and then captures "= 1 THEN ..." as
# the target table.
# The SET body must ALSO stop at the next UPDATE/INSERT/DELETE. An "UPDATE t SET .. FROM a JOIN b ON .." with
# NO WHERE (its filter is the JOIN) is legal and common here; with only WHERE/;/$ as terminators the lazy (.*?)
# ran straight through the FOLLOWING statement to a later WHERE, so that next UPDATE was never matched at all —
# the write-path check silently compared fewer statements than the proc actually has. UPDATE/INSERT/DELETE
# cannot appear inside a SET expression (a subquery may only SELECT), so they are safe terminators.
UPD = re.compile(
    r"\bUPDATE\b\s+((?:(?!\b(?:SET|SELECT|INSERT|DELETE|VALUES|BEGIN|END|IF|WHILE|CALL|EXEC)\b)[^;])+?)"
    r"\s+SET\s+(.*?)(?=\bWHERE\b|\bUPDATE\b|\bINSERT\b|\bDELETE\b|;|$)",
    re.I | re.S,
)

# "<table> [AS] <alias>" inside a FROM clause, so a T-SQL alias target can be resolved back to its real table
ALIAS = re.compile(r"([\[\]\w.`@]+)\s+(?:AS\s+)?([A-Za-z_]\w*)", re.I)


def set_cols(s):
    """Column names on the LEFT of each top-level '=' in a SET list."""
    out = []
    depth = 0
    cur = ""
    for ch in s:
        if ch == "(":
            depth += 1
        elif ch == ")":
            depth -= 1
        if ch == "," and depth == 0:
            out.append(cur)
            cur = ""
        else:
            cur += ch
    out.append(cur)
    names = []
    for part in out:
        if "=" not in part:
            continue
        lhs = part.split("=", 1)[0].strip()
        # T-SQL compound assignment: "Count_ACtivties += 1" -> the LHS is still just the column name.
        # (The converter correctly expands this to "Count_ACtivties = Count_ACtivties + 1".)
        lhs = lhs.rstrip("+-*/").strip()
        lhs = lhs.replace("[", "").replace("]", "").replace("`", "").split(".")[-1].strip()
        if re.fullmatch(r"\w+", lhs or ""):
            names.append(lhs.lower())
    return names


# Statement keywords that, at paren-depth 0, begin a NEW statement after an UPDATE .. SET. The legacy T-SQL
# omits ';' between statements, so "UPDATE t SET c=0 SELECT @x=.." has the SET body run straight into the
# next SELECT. The converter inserts the ';' (so the MySQL side is clean) — cut the T-SQL SET body at the same
# place so the two compare like with like. Subquery SELECTs sit inside parens (depth>0) and are NOT cut.
# Keywords that begin a NEW statement after an UPDATE .. SET. NOTE: BEGIN/END are deliberately NOT here — a
# SET value very often contains a CASE .. END expression, and cutting at that END would drop every assignment
# after it (e.g. "SET AccName = '..'+CASE..END+@x, AccPhone=@p" would lose AccPhone). The keywords below never
# appear inside a SET expression at statement level.
_STMT_KW = re.compile(r"\b(SELECT|INSERT|UPDATE|DELETE|WHILE|CALL|EXEC|RETURN|DECLARE|FROM)\b", re.I)
_TOK = re.compile(r"'|\"|\(|\)|\bCASE\b|\bEND\b|\b\w+\b", re.I)


def cut_set_body(sets):
    """The SET assignments only — truncated before the FROM source or the next top-level statement keyword.

    Tracks paren depth, string quotes AND CASE..END nesting so a CASE inside a value is never mistaken for a
    statement boundary and the value's trailing assignments survive.
    """
    depth, case_depth, q = 0, 0, None
    i = 0
    for m in _TOK.finditer(sets):
        tok = m.group(0)
        if q:
            if tok == q:
                q = None
            continue
        if tok in ("'", '"'):
            q = tok
        elif tok == "(":
            depth += 1
        elif tok == ")":
            depth -= 1
        elif depth == 0:
            up = tok.upper()
            if up == "CASE":
                case_depth += 1
            elif up == "END":
                if case_depth > 0:
                    case_depth -= 1
            elif case_depth == 0 and _STMT_KW.fullmatch(tok):
                return sets[: m.start()]
    return sets


def resolve_target(tpart, tail):
    """The real table an UPDATE writes to.

    T-SQL writes  `UPDATE a SET .. FROM Invoices AS a JOIN ..`  — the target is an ALIAS.
    MySQL writes  `UPDATE Invoices a JOIN .. SET ..`            — the target is the real table.
    To compare like with like, a T-SQL alias target is resolved back to its table via the FROM clause.
    """
    # Multi-table UPDATE ("t1 JOIN t2 SET .." / "t1, t2 SET .."): MySQL updates the listed table that owns the
    # SET columns, not the first one. Resolve by column ownership so it lines up with the T-SQL explicit target.
    is_multi = bool(re.search(r"\bJOIN\b", tpart, re.I)) or ("," in tpart)
    if is_multi and COL_OWNER:
        cands = candidate_tables(tpart)
        setcols = [c for c in set_cols(cut_set_body(tail)) if c in COL_OWNER]  # cols we know the owner of
        if cands and setcols:
            owners = [t for t in cands if all(t in COL_OWNER[c] for c in setcols)]
            if len(set(owners)) == 1:
                return owners[0]

    first = re.split(r"[,\s]+", tpart.strip())[0]
    tgt = norm_tbl(first)
    fm = re.search(r"\bFROM\b(.*)$", tail, re.I | re.S)
    if fm and re.fullmatch(r"\w+", first.strip()):
        for tbl_name, alias in ALIAS.findall(fm.group(1)):
            if alias.lower() == first.strip().lower() and alias.lower() != "as":
                return norm_tbl(tbl_name)
    return tgt


def updates(body):
    """(target_table, set_columns) for every UPDATE."""
    res = []
    for m in UPD.finditer(body):
        cols = set_cols(cut_set_body(m.group(2)))
        if not cols:
            continue
        res.append((resolve_target(m.group(1), m.group(2)), tuple(cols)))
    return res


# DELETE targets. T-SQL allows "DELETE a FROM t AS a WHERE ..", "DELETE FROM t WHERE ..", and even
# "DELETE t WHERE ..". MySQL needs "DELETE FROM t WHERE ..". A target rewritten to the wrong table, or an
# alias left dangling, deletes the wrong rows.
# The tail must stop at the next STATEMENT, not just at ';'. T-SQL frequently omits semicolons, so a
# ".*?(?=;|$)" tail swallows every following statement and only the FIRST delete in a proc is ever seen
# (ProfileName_delete has 4; the naive regex found 1).
DEL = re.compile(
    r"\bDELETE\b\s+(?:FROM\s+)?([@#\[\]\w.`]+)"
    r"((?:(?!\b(?:DELETE|INSERT|UPDATE|SELECT|BEGIN|END|IF|WHILE|COMMIT|ROLLBACK|SET|CALL|EXEC)\b)[^;])*)",
    re.I | re.S,
)


def deletes(body):
    res = []
    for m in DEL.finditer(body):
        tgt = norm_tbl(m.group(1))
        tail = m.group(2)
        # T-SQL "DELETE a FROM Table AS a" -> resolve the alias to the real table
        fm = re.search(r"\bFROM\b(.*)$", tail, re.I | re.S)
        if fm and re.fullmatch(r"\w+", m.group(1).strip()):
            for tbl_name, alias in ALIAS.findall(fm.group(1)):
                if alias.lower() == m.group(1).strip().lower():
                    tgt = norm_tbl(tbl_name)
                    break
        res.append(tgt)
    return res


# ---- unguarded writes -------------------------------------------------------------------------------
# An UPDATE or DELETE that LOST its WHERE clause hits EVERY ROW in the table. This is the most destructive
# thing a bad translation can do, and it is completely silent — the proc succeeds.
# Reported as: statements that HAVE a WHERE in the T-SQL but NOT in the MySQL.
DML = re.compile(r"\b(UPDATE|DELETE|INSERT)\b", re.I)


def guarded(body):
    """For each UPDATE / DELETE, does it carry a WHERE? Returns a list of booleans, in source order.

    The statement's window runs from the keyword to the NEXT ';' or the next top-level DML keyword,
    whichever comes first. Deliberately NOT regex-terminated on SET/END: an UPDATE's WHERE comes *after* its
    SET, and `SET x = CASE .. END` would otherwise cut the window short and report a false "unguarded".
    SELECT is not a terminator either, because it legitimately appears as a subquery inside the WHERE.
    """
    starts = [(m.start(), m.group(1).upper()) for m in DML.finditer(body)]
    out = []
    for i, (pos, kw) in enumerate(starts):
        if kw == "INSERT":
            continue
        end = len(body)
        semi = body.find(";", pos)
        if semi != -1:
            end = semi
        if i + 1 < len(starts):
            end = min(end, starts[i + 1][0])
        # ...and stop at the next statement that STARTS at paren-depth 0. The legacy T-SQL omits ';', so an
        # UPDATE whose filter is its JOIN (no WHERE at all — legal, and faithful in MySQL) otherwise ran on
        # into a LATER statement's WHERE and was reported "guarded" on the T-SQL side but not on the MySQL
        # side — a phantom "lost a WHERE". Depth-0 only: a SELECT inside the WHERE (subquery) is NOT a new
        # statement. SET is excluded — it is part of the UPDATE itself.
        end = min(end, _stmt_window_end(body, pos, end))
        out.append(bool(re.search(r"\bWHERE\b", body[pos:end], re.I)))
    return out


_NEWSTMT = re.compile(r"\b(SELECT|INSERT|UPDATE|DELETE|DECLARE|WHILE|EXEC|EXECUTE|COMMIT|ROLLBACK|RETURN)\b", re.I)


def _stmt_window_end(body, pos, hard_end):
    """Index of the next depth-0 statement keyword after `pos` (exclusive of the one AT pos), else hard_end."""
    depth, q, i = 0, None, pos
    # skip the leading keyword itself
    first = _NEWSTMT.match(body, pos)
    i = first.end() if first else pos
    while i < hard_end:
        ch = body[i]
        if q:
            if ch == q:
                q = None
            i += 1
            continue
        if ch in "'\"":
            q = ch
        elif ch == "(":
            depth += 1
        elif ch == ")":
            depth -= 1
        elif depth == 0:
            m = _NEWSTMT.match(body, i)
            if m:
                return i
        i += 1
    return hard_end


# ---- result-set column aliases ----------------------------------------------------------------------
# The DevExpress forms bind grid columns BY NAME (and sometimes by index). If a translation renamed, dropped
# or re-ordered an output column, the grid silently shows nothing in that column — no error. The converter
# has no business touching an alias, so ANY difference is a bug.
#
# Types must be excluded: "CAST(x AS varchar)" -> "CAST(x AS CHAR)" is a legitimate change and the token after
# AS is a TYPE, not an alias.
TYPE_WORDS = {
    # data types — "CAST(x AS varchar)" -> "CAST(x AS CHAR)" is a legitimate change, not an alias rename
    "char", "varchar", "nvarchar", "nchar", "text", "ntext", "longtext", "int", "integer", "signed",
    "unsigned", "bigint", "smallint", "tinyint", "bit", "decimal", "numeric", "float", "real", "money",
    "date", "datetime", "datetime2", "smalldatetime", "time", "binary", "varbinary", "image", "blob",
    "longblob", "uniqueidentifier", "xml",
    # keywords that follow an AS but are not aliases. SQL Server's ROUTINE_DEFINITION includes the whole
    # "CREATE PROC .. @p AS INT AS <body>" header, while MySQL's is the BEGIN..END body only — so the T-SQL
    # side alone sees the body-introducing "AS SET NOCOUNT ON" / "AS BEGIN" and would report a phantom alias.
    "set", "begin", "select", "declare", "with", "on", "off", "table", "update", "insert", "delete",
    "if", "while", "return", "exec", "execute", "dec",
}
ALIAS_AS = re.compile(r"\bAS\s+((?:\[[^\]]+\])|(?:`[^`]+`)|(?:'[^']+')|(?:\"[^\"]+\")|\w+)", re.I)

# Aliases are extracted ONLY from a SELECT's output list (between SELECT and its FROM). Scanning the whole
# body instead produces pure noise, because the T-SQL side has AS-tokens that MySQL structurally cannot have:
#   * the proc header            CREATE PROC x @p AS INT AS <body>
#   * every local declaration    DECLARE @code AS DECIMAL(18,0)     (MySQL's DECLARE has no AS)
# Both vanish once we only look inside SELECT lists.
SELECT_LIST = re.compile(r"\bSELECT\b(?:\s+DISTINCT\b|\s+TOP\s+\d+\b)?(.*?)\bFROM\b", re.I | re.S)


def aliases(body):
    out = []
    for sm in SELECT_LIST.finditer(body):
        sel = sm.group(1)
        # a "SELECT @x = expr" is an assignment, not a result set — it becomes SET in MySQL
        if re.match(r"\s*[@\w]+\s*=[^=]", sel):
            continue
        for m in ALIAS_AS.finditer(sel):
            a = m.group(1).strip().strip("[]`'\"")
            low = a.lower()
            if low in TYPE_WORDS:
                continue
            out.append(low)
    return out


# ---- the VALUES list ---------------------------------------------------------------------------------
# The write-column checks prove the right COLUMNS are targeted. They cannot see a wrong VALUE: if the
# converter shifted or swapped an expression, the INSERT still names the right columns and still succeeds —
# it just files the Debit under Credit. That is exactly the kind of bug nobody notices until the books are
# wrong. So compare the VALUES list positionally, after normalising the translations we KNOW are legitimate.
FUNC_MAP = {
    "isnull": "ifnull",
    "getdate": "now",
    "len": "char_length",
    "charindex": "locate",
    "convert": "cast",       # CONVERT(t, x) and CAST(x AS t) both collapse to "cast"
    "space": "space",
}


def canon(expr):
    """Canonical form of a VALUES / SET expression, so T-SQL and MySQL spellings of the SAME thing match."""
    e = expr.strip().lower()
    e = re.sub(r"\s+", " ", e)
    e = e.replace("[", "").replace("]", "").replace("`", "")
    e = re.sub(r"\bdbo\.", "", e)
    e = e.replace("@", "")                       # @SafeID -> safeid
    e = re.sub(r"\bp_", "", e)                   # p_SafeID -> safeid
    e = re.sub(r"\bv_", "", e)
    # DATE RENDERING: T-SQL spells it CONVERT(<char type>, x, <style>) and the port spells it
    # DATE_FORMAT(x, '<pattern>') — the SAME operation, so reduce BOTH to "~datefmt x". Without this a correct
    # translation (CONVERT(VARCHAR(8),GETDATE(),108) -> DATE_FORMAT(NOW(),'%H:%i:%s')) reads as a changed value.
    # Only the 3-arg CONVERT (a style code) is a date rendering; the 2-arg form stays a plain CAST.
    # MUST run BEFORE the FUNC_MAP loop below, which rewrites `convert` -> `cast` and would hide the 3-arg form.
    # The argument patterns are PAREN-AWARE on purpose. A lazy `(.+?)` looks equivalent but happily runs past
    # the CONVERT's own closing paren and grabs a comma from an ENCLOSING call: in
    #     ROUND((CONVERT(DECIMAL(12,0), @SalaryVal / 30) * @DaysNumber) / 5.0), 0)
    # it matched through to ROUND's ", 0)" and mis-read a 2-arg CONVERT as a 3-arg (styled) one — reporting a
    # phantom write-path difference against the correct CAST(.. AS DECIMAL(12,0)) on the MySQL side.
    _ARG = r"(?:[^(),]|\([^()]*\))*"
    e = re.sub(r"\bconvert\s*\(\s*[^,()]*(?:\([^()]*\))?\s*,\s*(" + _ARG + r")\s*,\s*\d+\s*\)", r"~datefmt \1 ", e)
    e = re.sub(r"\bdate_format\s*\(\s*(" + _ARG + r")\s*,\s*'[^']*'\s*\)", r"~datefmt \1 ", e)
    for a, b in FUNC_MAP.items():                # isnull(..) -> ifnull(..)
        e = re.sub(r"\b" + a + r"\b", b, e)
    # String concat: T-SQL "a + b + 'x'" and MySQL "CONCAT(a, b, 'x')" are the SAME expression. Drop both the
    # '+' operator and the CONCAT name so the two spellings canonicalise identically.
    e = re.sub(r"\bconcat\b", " ", e)
    e = e.replace("+", " ")
    # CONVERT(date, x) vs CAST(x AS date): reduce to a sorted token bag so the two spellings agree.
    e = re.sub(r"\bas\b", " ", e)
    e = re.sub(r"[(),]", " ", e)
    # The CAST TARGET TYPE legitimately changes in translation: CAST(x AS nvarchar) -> CAST(x AS CHAR),
    # CAST(x AS int) -> CAST(x AS SIGNED). Collapse each family to one token so the type rename is not
    # mistaken for a changed value.
    e = re.sub(r"\b(nvarchar|varchar|nchar|char|text|ntext|longtext)\b", "~str", e)
    e = re.sub(r"\b(int|integer|bigint|smallint|tinyint|signed|unsigned)\b", "~int", e)
    e = re.sub(r"\b(decimal|numeric|money|float|real)\b", "~num", e)
    e = re.sub(r"\b(datetime2?|smalldatetime)\b", "~dt", e)
    toks = sorted(t for t in e.split() if t)
    return " ".join(toks)


def insert_values(body):
    """(table, [canonical value expressions]) for each INSERT .. VALUES (..).

    Parsed with BALANCED PARENS, not a regex: the VALUES list is full of nested calls
    (CONVERT(DATE, GETDATE()), CONCAT(..)), and T-SQL frequently omits the trailing semicolon — so a
    ".*?\\)(?=;|$)" tail matched nothing at all on the T-SQL side and every proc silently compared as "[]".
    """
    res = []
    for m in re.finditer(r"\bINSERT\b\s+(?:INTO\s+)?([@#\[\]\w.`]+)\s*\(", body, re.I):
        # column list
        cols_txt, i = balanced(body, m.end() - 1)
        if cols_txt is None:
            continue
        vm = re.match(r"\s*VALUES\s*\(", body[i:], re.I)
        if not vm:
            continue
        vals_txt, _ = balanced(body, i + vm.end() - 1)
        if vals_txt is None:
            continue
        vals = split_top(vals_txt)
        if vals:
            res.append((norm_tbl(m.group(1)), tuple(canon(v) for v in vals)))
    return res


def update_values(body):
    """(target, [canonical RHS expressions]) for each UPDATE .. SET.

    Same bug class as insert_values: the SET column list can be perfectly right while the VALUE assigned to a
    column is wrong (swapped with its neighbour, or a literal changed). The column check cannot see that.
    """
    res = []
    for m in UPD.finditer(body):
        tpart, sets = m.group(1), m.group(2)
        # T-SQL's "UPDATE a SET .. FROM t AS a JOIN .." keeps the table source AFTER the SET list, and the
        # legacy code often omits the ';' before the NEXT statement — cut both off so the LAST assignment does
        # not swallow the FROM clause or bleed into a following SELECT.
        sets = cut_set_body(sets)
        rhs = []
        for part in split_top(sets):
            if "=" not in part:
                continue
            lhs, val = part.split("=", 1)
            raw_lhs = lhs.strip()
            compound = raw_lhs.endswith(("+", "-", "*", "/"))
            lhs = raw_lhs.rstrip("+-*/").strip()
            if not re.fullmatch(r"[\[\]`\w.]+", lhs or ""):
                continue
            # T-SQL "Cnt += 1" MEANS "Cnt = Cnt + 1" — which is exactly what the converter emits. Expand it
            # so the two spellings canonicalise to the same thing.
            if compound:
                val = lhs + " + " + val
            rhs.append(canon(val))
        if rhs:
            res.append((resolve_target(tpart, m.group(2)), tuple(rhs)))
    return res


def balanced(s, open_idx):
    """Text inside the parens starting at s[open_idx] == '(', and the index just past the closer."""
    if open_idx >= len(s) or s[open_idx] != "(":
        return None, open_idx
    depth, i, q = 0, open_idx, None
    while i < len(s):
        ch = s[i]
        if q:
            if ch == q:
                q = None
        elif ch in "'\"":
            q = ch
        elif ch == "(":
            depth += 1
        elif ch == ")":
            depth -= 1
            if depth == 0:
                return s[open_idx + 1:i], i + 1
        i += 1
    return None, open_idx


def split_top(s):
    """Split on top-level commas (respecting parens and string literals)."""
    out, depth, cur, q = [], 0, "", None
    for ch in s:
        if q:
            cur += ch
            if ch == q:
                q = None
            continue
        if ch in "'\"":
            q = ch
            cur += ch
            continue
        if ch == "(":
            depth += 1
        elif ch == ")":
            depth -= 1
        if ch == "," and depth == 0:
            out.append(cur)
            cur = ""
        else:
            cur += ch
    if cur.strip():
        out.append(cur)
    return [x for x in out if x.strip()]


def main():
    # Both dumps come from `migrator dumpbodies`, which reads through SqlClient / MySqlConnector and writes
    # UTF-8 with REAL newlines. (sqlcmd cannot be used: it renders Arabic identifiers as '?????', which would
    # make every alias compare unequal.) So neither side needs un-escaping.
    ss = load("ss_bodies.txt")
    my = load("my_bodies.txt")
    print("procs: SQL Server %d, MySQL %d" % (len(ss), len(my)))

    rc = 0

    # --- the destructive one: an UPDATE/DELETE that lost its WHERE hits every row, silently ---
    unguarded = []
    for name, sbody in ss.items():
        if name not in my:
            continue
        a = guarded(sbody)
        b = guarded(my[name])
        # same number of write statements, but MySQL has fewer guarded ones -> a WHERE was lost
        if len(a) == len(b) and a.count(True) > b.count(True):
            unguarded.append((name, a.count(True), b.count(True)))
        elif a.count(True) > 0 and b.count(True) == 0 and len(b) > 0:
            unguarded.append((name, a.count(True), b.count(True)))
    print("")
    print("%-24s procs checked: %-4d LOST A WHERE: %d" % ("UPDATE/DELETE guards", len(ss), len(unguarded)))
    for name, na, nb in unguarded:
        print("  !! %s  T-SQL guarded=%d  MySQL guarded=%d" % (name, na, nb))
    if unguarded:
        rc = 1

    # T-SQL writes that are DEAD CODE in the SOURCE database and are therefore deliberately not reproduced.
    # Every entry must record WHY it is dead, verified against SQL Server — the default for a missing write
    # is that it is a BUG, so keep this list tiny and never add to it to silence an unexplained diff.
    #
    #   MultiAcountEditTB_Insert: its CATCH ends with "INSERT INTO ErrorLog (ErrorMessage, ErrorDate,
    #   ProcedureName)", but OBJECT_ID('ErrorLog') is NULL in EXCHANGESYS2026 — no such table, view or
    #   synonym exists. That INSERT could therefore never have succeeded: any error inside the proc made the
    #   CATCH itself fail with "Invalid object name 'ErrorLog'". Reproducing it in MySQL would replace the
    #   real error text with "Table doesn't exist" and make the handler strictly worse, so the port keeps
    #   ROLLBACK + RESIGNAL, which hands the ORIGINAL message to the app exactly as SQL Server does.
    DEAD_SOURCE_WRITES = {
        ("EXCHANGESYS2026__MultiAcountEditTB_Insert", "errorlog"),
    }

    # GATES — a mismatch here is a real bug and fails the audit.
    for label, extract in (
        ("INSERT column lists", inserts),
        ("UPDATE SET column lists", updates),
        ("DELETE targets", deletes),
        ("INSERT VALUES (positional)", insert_values),
        ("UPDATE SET values", update_values),
    ):
        checked = 0
        mismatched = []
        for name, sbody in ss.items():
            if name not in my:
                continue
            a = extract(sbody)
            b = extract(my[name])
            a = [t for t in a if (name, t[0]) not in DEAD_SOURCE_WRITES]
            if not a and not b:
                continue
            checked += 1
            if a != b:
                mismatched.append((name, a, b))

        print("")
        print("%-24s procs compared: %-4d MISMATCHED: %d" % (label, checked, len(mismatched)))
        for name, a, b in mismatched:
            print("  -- " + name)
            print("     T-SQL: " + repr(a)[:260])
            print("     MySQL: " + repr(b)[:260])
        if mismatched:
            rc = 1

    # INFORMATIONAL ONLY — deliberately NOT a gate.
    #
    # Comparing result-column aliases textually is a heuristic, and a noisy gate is worse than no gate: it
    # trains people to ignore audit failures. Every proc it flagged so far turned out to be an extractor
    # artifact, not a bug — e.g. CoBranch_LoadDataIntoDataGridview was reported as differing, but EXECUTING it
    # on both engines returns byte-identical columns (الرقم, Code, BName, BType, Mobile1, Mobile2, IsActive).
    # Treat a hit here as "go look", never as "it is broken": confirm by calling the proc on both engines.
    #
    # Result-set columns ARE properly verified for read procs by the migrator's diff test (it compares the
    # actual rows), which covers 203 of them.
    alias_mism = []
    for name, sbody in ss.items():
        if name not in my:
            continue
        a, b = aliases(sbody), aliases(my[name])
        if (a or b) and a != b:
            alias_mism.append(name)
    print("")
    print("%-24s %d proc(s) to eyeball (HEURISTIC — not a gate; confirm by executing)"
          % ("result column aliases", len(alias_mism)))
    for n in alias_mism:
        print("   ? " + n)

    return rc


if __name__ == "__main__":
    sys.exit(main())
