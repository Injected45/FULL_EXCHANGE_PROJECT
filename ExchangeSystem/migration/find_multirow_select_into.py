"""
Find every  SELECT .. INTO v_x FROM <table> WHERE <cols>  whose predicate is NOT unique IN THE DATA.

WHY THIS EXISTS
T-SQL tolerates a multi-row  SELECT @v = col FROM ..  : it assigns each row in turn and the LAST one wins.
MySQL raises  ERROR 1172 "Result consisted of more than one row"  and ABORTS the routine. A faithful
statement-for-statement port therefore inherits a landmine that only detonates on the data that happens to
have duplicates.

This has already caused two live failures:
  * EMP_GetADVPMNTTtoalsIndivdual  -> broke VW_SALARYCALCLAST and the whole salary screen
  * the BranchRatesTb rate lookups -> aborted EVERY agent transfer (46 duplicate branch pairs)

The converter cannot catch these: the code looks fine, and whether it breaks depends on the CONTENTS of the
table. So this checks the data instead - for each single-table lookup it asks the server whether the WHERE
columns are actually unique:

    SELECT 1 FROM <table> GROUP BY <where cols> HAVING COUNT(*) > 1 LIMIT 1

A hit means the statement CAN return several rows and will abort at runtime for those values.

Deliberately conservative: only single-table lookups with plain "col = value" predicates are judged, so a
report is a real finding rather than something to explain away. Everything else is COUNTED and can be listed
with --show-skipped, so the blind spot stays visible instead of looking like a clean bill of health.

All probes for one schema are sent as a single batched query - one mysql process per schema, not per lookup.
"""
import re
import subprocess
import sys

try:                       # the statements carry Arabic literals
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
except Exception:
    pass

MYSQL = [r"C:\xampp\mysql\bin\mysql.exe", "-u", "root", "-N", "--default-character-set=utf8mb4"]
SCHEMAS = ("EXCHANGESYS2026", "ExSyAccounts2026", "ExSyAccountsCurrency2026")
SEP = "|~|"   # mysql -N escapes a real TAB as the two characters \t, so a tab separator never survives


def q(sql, db=None):
    cmd = MYSQL + (["-D", db] if db else []) + ["-e", sql]
    r = subprocess.run(cmd, capture_output=True, text=True, encoding="utf-8", errors="replace")
    return [l for l in (r.stdout or "").replace("\r", "").split("\n") if l.strip()]


STMT = re.compile(
    r"\bSELECT\b(?P<sel>(?:(?!\bFROM\b).)*?)\bINTO\b(?:(?!\bFROM\b).)*?"
    r"\bFROM\b\s*`?(?P<tbl>\w+)`?(?:\s+(?:AS\s+)?`?\w+`?)?\s*\bWHERE\b(?P<where>.*)$",
    re.I | re.S,
)
PRED = re.compile(r"(?:^|\bAND\b)\s*(?:`?\w+`?\s*\.\s*)?`?(\w+)`?\s*=\s*(\S+)", re.I)
BAD_PRED = re.compile(r"\b(LIKE|IN|BETWEEN|OR)\b|<>|!=|<|>", re.I)


def collect():
    """-> (lookups, skipped)   lookups: list of (db, routine, table, cols, text)"""
    lookups, skipped = [], []
    for db in SCHEMAS:
        rows = q("SELECT CONCAT(ROUTINE_SCHEMA,'%s',ROUTINE_NAME,'%s',"
                 "REPLACE(REPLACE(IFNULL(ROUTINE_DEFINITION,''),CHAR(10),' '),CHAR(9),' ')) "
                 "FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='%s'" % (SEP, SEP, db))
        for line in rows:
            p = line.split(SEP, 2)
            if len(p) != 3:
                continue
            schema, name, body = p
            body = body.replace("\\n", " ").replace("\\t", " ")
            for stmt in body.split(";"):
                if not re.search(r"\bINTO\s+v_\w+", stmt, re.I) or not re.search(r"\bFROM\b", stmt, re.I):
                    continue
                if re.search(r"\bLIMIT\b", stmt, re.I):
                    continue                       # already bounded to one row
                flat = " ".join(stmt.split())
                m = STMT.search(flat)
                if not m or re.search(r"\bJOIN\b|\(\s*SELECT\b", flat, re.I):
                    skipped.append((schema, name, flat[:100])); continue
                # An AGGREGATE with no GROUP BY collapses any number of matching rows to exactly ONE, so it
                # can never raise 1172 no matter how duplicated the key is. Excluding these is what takes
                # the report from "29 findings, mostly noise" down to the handful that are real.
                if re.search(r"\b(SUM|COUNT|MAX|MIN|AVG|GROUP_CONCAT)\s*\(", m.group("sel") or "", re.I) \
                        and not re.search(r"\bGROUP\s+BY\b", flat, re.I):
                    continue
                where = m.group("where")
                preds = PRED.findall(where)
                if not preds or BAD_PRED.search(where):
                    skipped.append((schema, name, flat[:100])); continue
                # A predicate pinned to a LITERAL narrows the rows; one driven by a PARAMETER is the thing
                # that has to be unique. Grouping over the whole table ignores that and reports nonsense:
                # "WHERE IsMain = 1" is unique, yet GROUP BY IsMain "finds duplicates" among the IsMain = 0
                # rows. So literals become a WHERE and only parameter columns are grouped on.
                lit, par = [], []
                for col, rhs in preds:
                    (lit if re.match(r"^-?[0-9.]+$|^'", rhs) else par).append((col.lower(), rhs))
                lookups.append((schema, name, m.group("tbl"), tuple(lit), tuple(p[0] for p in par), flat[:150]))
    return lookups, skipped


def probe(db, keys):
    """keys: set of (table, literal-preds, param-cols) -> duplicate count (None if unjudgeable)"""
    if not keys:
        return {}
    parts, order = [], []
    for tbl, lit, par in sorted(keys):
        wh = (" WHERE " + " AND ".join("`%s` = %s" % (c, v) for c, v in lit)) if lit else ""
        if par:
            grp = ",".join("`%s`" % c for c in par)
            inner = "SELECT 1 FROM `%s`%s GROUP BY %s HAVING COUNT(*)>1" % (tbl, wh, grp)
        else:
            # every predicate is a literal: the question is simply whether it matches more than one row
            inner = "SELECT 1 FROM `%s`%s HAVING COUNT(*)>1" % (tbl, wh)
        parts.append("SELECT (SELECT COUNT(*) FROM (%s) x) AS n" % inner)
        order.append((tbl, lit, par))
    out, res = [], {}
    # one process per chunk keeps the command line sane while staying far off per-lookup spawning
    CH = 40
    for i in range(0, len(parts), CH):
        chunk = parts[i:i + CH]
        rows = q(" UNION ALL ".join(chunk) + ";", db)
        if any("ERROR" in r for r in rows):        # a bad column/table in the chunk: fall back per item
            for ppart, key in zip(chunk, order[i:i + CH]):
                r2 = q(ppart + ";", db)
                res[key] = None if (not r2 or any("ERROR" in x for x in r2)) else int(r2[0].split("\t")[-1])
            continue
        out.extend(rows)
        for r, key in zip(rows, order[i:i + CH]):
            try:
                res[key] = int(r.split("\t")[-1])
            except ValueError:
                res[key] = None
    return res


def main():
    show_skipped = "--show-skipped" in sys.argv
    lookups, skipped = collect()

    findings, unjudged = [], 0
    for db in SCHEMAS:
        keys = {(t, lit, par) for (s, n, t, lit, par, x) in lookups if s.lower() == db.lower()}
        res = probe(db, keys)
        for (s, n, t, lit, par, txt) in lookups:
            if s.lower() != db.lower():
                continue
            c = [x[0] for x in lit] + list(par)
            v = res.get((t, lit, par))
            if v is None:
                unjudged += 1
            elif v > 0:
                findings.append((s, n, t, c, v, txt))

    print("single-table SELECT..INTO lookups judged   : %d" % (len(lookups) - unjudged))
    print("could not be judged (view / bad column)    : %d" % unjudged)
    print("not analysed (join / subquery / non-equality): %d%s"
          % (len(skipped), "" if show_skipped else "   (--show-skipped to list)"))
    print("")
    if not findings:
        print("NON-UNIQUE LOOKUPS: none - every judged predicate is unique in the current data.")
    else:
        print("NON-UNIQUE LOOKUPS - each CAN raise ERROR 1172 and abort its routine: %d" % len(findings))
        for s, n, t, c, v, txt in sorted(findings):
            print("  !! %s.%s" % (s, n))
            print("     %s(%s): %d duplicated key value(s) in the data" % (t, ",".join(c), v))
            print("     %s" % txt)
    if show_skipped:
        print("\n-- not analysed --")
        for s, n, txt in skipped:
            print("   %s.%s  %s" % (s, n, txt))
    return 1 if findings else 0


if __name__ == "__main__":
    sys.exit(main())
