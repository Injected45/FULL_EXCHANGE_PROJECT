#!/usr/bin/env bash
# =====================================================================================
# audit.sh — the SILENT-BUG checks. Every one must print 0.
#
# Run after ANY migrator run (and after apply_handports.sh).
#
# Why this file exists: the bugs that actually hurt in this migration are the ones where BOTH engines
# accept the syntax and only the MEANING differs. They never raise an error — they just hand you wrong
# numbers or an empty screen. A create-failure is loud and lands in a worklist; these are invisible.
# Every check below was added *after* finding a live instance that the previous checks had passed clean.
# =====================================================================================
set -u
MY="/c/xampp/mysql/bin/mysql.exe -u root"
DB=EXCHANGESYS2026
q() { $MY -N --default-character-set=utf8mb4 -e "$1" 2>/dev/null | grep -v "prefix" ; }

fail=0
chk() { # name, count
  printf "  %-56s %s\n" "$1" "$2"
  [ "$2" = "0" ] || fail=1
}

echo "SILENT-BUG AUDIT ($DB)"

chk "1  '+' next to a string literal" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_DEFINITION REGEXP \"[+][[:space:]]*'|'[[:space:]]*[+]\";")"

# The first branch's [^)]* cannot cross an inner '(' correctly, so for a nested call like
# CONCAT(CAST(TIMESTAMPDIFF(..) + 1 AS CHAR)) it matches CONCAT('s open against TIMESTAMPDIFF's close and
# flags the numeric "+ 1". A REAL string concat after a func ')' is never followed by a bare number, so
# require the char after '+' to be a non-digit ([^0-9[:space:]]) — this drops the arithmetic false positive
# while still catching CONCAT(x) + col / SPACE(1) + name etc.
chk "2  '+' next to a string-returning function" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND (ROUTINE_DEFINITION REGEXP '(SPACE|CONCAT|SUBSTRING|LEFT|RIGHT|TRIM|REPLACE|UPPER|LOWER)[(][^)]*[)][[:space:]]*[+][[:space:]]*[^0-9[:space:]]' OR ROUTINE_DEFINITION REGEXP '[+][[:space:]]*(SPACE|CONCAT|SUBSTRING|LEFT|RIGHT|TRIM|REPLACE|UPPER|LOWER)[(]');")"

chk "3  '+' chain feeding a LIKE" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_DEFINITION REGEXP '[+][^,;)]{0,60}LIKE';")"

chk "4  CONCAT( with a bare keyword / spliced mid-word" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND (ROUTINE_DEFINITION REGEXP 'CONCAT[[:space:]]*[(][[:space:]]*(END|THEN|ELSE|WHEN)[[:space:]]*[,)]' OR ROUTINE_DEFINITION REGEXP '[[:alpha:]]CONCAT[(]');")"

# (^|[^A-Za-z_]) is the word-boundary guard: DATE_FORMAT( legitimately takes a quoted pattern and must NOT be
# mistaken for an un-converted T-SQL FORMAT(x,'N3'). Do NOT use \b here — it silently matches NOTHING in this
# MariaDB build, which turns the check into a false PASS (verified).
# Any raw T-SQL FORMAT() with a QUOTED format arg — money ('N3'/'f3') OR date ('yyyy-MM-dd'). Legit MySQL
# FORMAT() never takes a quoted 2nd arg, so any occurrence is an un-converted T-SQL FORMAT (a silent bug: on
# a date it yields a giant number, on money it drops/relocates decimals). `(?:[^()]|[(][^()]*[)])*` lets the
# FIRST argument hold a ONE-LEVEL nested call — e.g. FORMAT(Account_GetAccVal(a.AccID,1,0),'f3') — which the
# old `[^)]*` could not see past. DATE_FORMAT is excluded: its FORMAT is preceded by '_', barred by [^A-Za-z_].
chk "5  raw T-SQL FORMAT(x,'..') left intact (money/date silent-wrong)" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_DEFINITION REGEXP \"(^|[^A-Za-z_])FORMAT[[:space:]]*[(](?:[^()]|[(][^()]*[)])*,[[:space:]]*'\";")"

chk "6  routine created with the wrong collation (err 1267)" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND COLLATION_CONNECTION<>'utf8mb4_unicode_ci';")"

# A multi-line T-SQL "EXEC proc @a,@b,.." whose args were dropped becomes "CALL proc()" — the call succeeds
# but writes NOTHING. Most damaging on the ledger insert. Flag any empty-arg CALL to a proc that takes params.
chk "6b empty-arg CALL to the ledger insert (silent skip)" \
 "$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_DEFINITION REGEXP 'CALL [A-Za-z_]*AccSafeActivityTb_Insert[a-z]*[(][)]';")"

# README §8.3: a row that migrated in with ID = 0 makes a recursive tree screen loop forever and the app
# closes with no error. Checked across EVERY AUTO_INCREMENT `ID` column, not just one table.
z=$(q "SELECT GROUP_CONCAT(CONCAT('SELECT ''',TABLE_NAME,''' t FROM \`$DB\`.\`',TABLE_NAME,'\` WHERE \`ID\`=0') SEPARATOR ' UNION ALL ')
       FROM information_schema.COLUMNS WHERE TABLE_SCHEMA='$DB' AND COLUMN_NAME='ID' AND EXTRA LIKE '%auto_increment%';")
n7=$( [ -n "$z" ] && q "SELECT COUNT(*) FROM ($z) x;" || echo 0 )
chk "7  a row with ID = 0 in an AUTO_INCREMENT table (tree recursion)" "${n7:-0}"

# ---- 8: SCHEMA-AWARE. Catches a '+' between two plain TEXT COLUMNS, where nothing else in the
#         expression (no literal, no string function, no LIKE) reveals that it is a concat.
#         This is the class that all of checks 1-3 pass clean.
TMP=$(mktemp -d)
q "SELECT DISTINCT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA IN ('$DB','ExSyAccounts2026','ExSyAccountsCurrency2026') AND DATA_TYPE IN ('varchar','char','text','longtext','mediumtext','tinytext');" | sort -u > "$TMP/t.txt"
q "SELECT DISTINCT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA IN ('$DB','ExSyAccounts2026','ExSyAccountsCurrency2026') AND DATA_TYPE NOT IN ('varchar','char','text','longtext','mediumtext','tinytext');" | sort -u > "$TMP/n.txt"
comm -23 "$TMP/t.txt" "$TMP/n.txt" > "$TMP/textonly.txt"
q "SELECT CONCAT(ROUTINE_NAME,'\t',REPLACE(REPLACE(ROUTINE_DEFINITION,'\n',' '),'\r',' ')) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_DEFINITION LIKE '%+%';" > "$TMP/plus.tsv"

n8=$(python - "$TMP" <<'PY'
import io, re, sys
d = sys.argv[1]
cols = {l.strip().lower() for l in io.open(d+'/textonly.txt', encoding='utf-8', errors='replace')
        if re.fullmatch(r'[A-Za-z_]\w*', l.strip() or '_')}
OPD = r'([A-Za-z_]\w*(?:\.[A-Za-z_]\w*)?)'
L, R = re.compile(OPD+r'\s*\+', re.I), re.compile(r'\+\s*'+OPD, re.I)
hits = set()
for line in io.open(d+'/plus.tsv', encoding='utf-8', errors='replace'):
    if '\t' not in line: continue
    name, body = line.split('\t', 1)
    for m in list(L.finditer(body)) + list(R.finditer(body)):
        if m.group(1).split('.')[-1].lower() in cols:
            hits.add(name.strip())
for h in sorted(hits): print("      !!", h, file=sys.stderr)
print(len(hits))
PY
)
rm -rf "$TMP"
chk "8  '+' adjacent to a known TEXT COLUMN (schema-aware)" "$n8"

# ---- 9: T-SQL FORMAT() is BOTH a number formatter and a DATE formatter:
#            FORMAT(x,'N3')          -> money
#            FORMAT(x,'hh:mm:ss tt') -> time
#         Mapping a DATE pattern onto MySQL's numeric FORMAT(x,D) is silent: "07:13:53 PM" came back as
#         "20,260,712,191,353". A date pattern MUST become DATE_FORMAT(x,'%h:%i:%s %p').
#         So: for every SOURCE routine that formats a DATE, assert the MySQL side uses DATE_FORMAT.
SRC='DESKTOP-M233HRE\SQLEXPRESS'
datefmt=$(sqlcmd -S "$SRC" -E -d "$DB" -h -1 -W -Q "SET NOCOUNT ON;
SELECT o.name FROM sys.sql_modules m JOIN sys.objects o ON o.object_id=m.object_id
WHERE o.is_ms_shipped=0 AND m.definition LIKE '%FORMAT(%'
  AND (m.definition LIKE '%hh:%' OR m.definition LIKE '%HH:%' OR m.definition LIKE '%yyyy%'
    OR m.definition LIKE '%tt''%' OR m.definition LIKE '%dd/%' OR m.definition LIKE '%MM/%');" -l 20 2>/dev/null \
  | tr -d '\r' | grep -E '^[A-Za-z_][A-Za-z0-9_]*$')

n9=0
for r in $datefmt; do
  has=$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_NAME='$r' AND ROUTINE_DEFINITION LIKE '%DATE_FORMAT%';")
  exists=$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_NAME='$r';")
  # only flag routines that actually made it into MySQL but lost their DATE_FORMAT
  if [ "${exists:-0}" != "0" ] && [ "${has:-0}" = "0" ]; then
    echo "      !! $r  (source formats a DATE; MySQL side has no DATE_FORMAT)"
    n9=$((n9+1))
  fi
done
chk "9  date formatted with numeric FORMAT() instead of DATE_FORMAT" "$n9"

# ---- 9b: the SAME silent class via the OTHER T-SQL spelling — CONVERT(<char type>, <date>, <style>). The 3rd
#          argument is a STYLE CODE that picks the rendering: CONVERT(VARCHAR(8), GETDATE(), 108) is "19:06:01",
#          CONVERT(NVARCHAR, DateForTime, 22) is "07/16/26  7:06:01 PM". Dropping the style and emitting a bare
#          CAST(x AS CHAR) yields the WHOLE datetime instead — no error, just a wrong string on the screen.
#          (Found live in CurrencyMovements_fillCrid / ZRPT_CurrencyMovements_fillCrid / ExternalEx_LoadToConfirm.)
#          So: for every SOURCE routine with a styled char CONVERT, assert the MySQL side uses DATE_FORMAT.
convstyle=$(sqlcmd -S "$SRC" -E -d "$DB" -h -1 -W -Q "SET NOCOUNT ON;
SELECT o.name FROM sys.sql_modules m JOIN sys.objects o ON o.object_id=m.object_id
WHERE o.is_ms_shipped=0 AND m.definition LIKE '%CONVERT(%CHAR%,%,%)%'
  AND (m.definition LIKE '%, 108)%' OR m.definition LIKE '%,108)%'
    OR m.definition LIKE '%, 22)%'  OR m.definition LIKE '%,22)%'
    OR m.definition LIKE '%Time,0)%' OR m.definition LIKE '%Time, 0)%');" -l 20 2>/dev/null \
  | tr -d '\r' | grep -E '^[A-Za-z_][A-Za-z0-9_]*$')

n9b=0
for r in $convstyle; do
  has=$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_NAME='$r' AND ROUTINE_DEFINITION LIKE '%DATE_FORMAT%';")
  exists=$(q "SELECT COUNT(*) FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_NAME='$r';")
  if [ "${exists:-0}" != "0" ] && [ "${has:-0}" = "0" ]; then
    echo "      !! $r  (source CONVERTs a date WITH a style code; MySQL side has no DATE_FORMAT)"
    n9b=$((n9b+1))
  fi
done
chk "9b CONVERT(char,date,style) style code dropped -> bare CAST" "$n9b"

# ---- 10: WRITE-PATH check. The 565 transactional procs are NEVER behaviourally diff-tested (the migrator's
#          `hardverify` EXECUTES them against SQL Server, which is unsafe here: the T-SQL references
#          ExSyAccounts2026 by absolute name and via SYNONYMs, so even a snapshot copy would still mutate the
#          real 506k-row ledger). So compare them STATICALLY instead: same target table, same columns, same
#          ORDER, for every INSERT and every UPDATE ... SET.
#          This is what caught CATEGORYTYPESTB_Insert writing to the tvp_ TEMP TABLE instead of the real one.
HERE="$(cd "$(dirname "$0")" && pwd)"

# First prove the detectors still WORK. A parser that silently matches nothing reports a perfect 0 — which is
# indistinguishable from "clean". test_cmp_writes.py fires each detector at a known-bad and a known-good input.
if ! python "$HERE/test_cmp_writes.py" >/dev/null 2>&1; then
  echo "      !! test_cmp_writes.py FAILED — the write-path detectors are broken; checks 10-11 mean nothing"
  fail=1
fi

TMPW=$(mktemp -d)
# Dump via the MIGRATOR, not sqlcmd: sqlcmd renders the bodies through the console code page and turns every
# Arabic identifier into '?????'. These procs are full of Arabic column aliases, so a sqlcmd dump would make
# the comparison meaningless. Covers all three schemas (the money ledger lives in ExSyAccounts2026).
( cd "$HERE/migrator" && dotnet run --no-build -- dumpbodies "$TMPW" ) >/dev/null 2>&1
cp "$HERE/cmp_writes.py" "$TMPW/" 2>/dev/null
# column-ownership map so cmp_writes can resolve a multi-table UPDATE's target (the joined table that OWNS the
# SET columns) instead of blindly taking the first table — see cmp_writes.py COL_OWNER. All three schemas.
$MY -N --default-character-set=utf8mb4 -e "SELECT CONCAT(TABLE_NAME,CHAR(9),COLUMN_NAME) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA IN ('$DB','ExSyAccounts2026','ExSyAccountsCurrency2026');" 2>/dev/null > "$TMPW/col_owner.tsv"
OUTW=$( cd "$TMPW" && python cmp_writes.py 2>/dev/null )

# 10: a write statement that lost its WHERE — updates/deletes EVERY row in the table. The worst outcome here.
n10=$(printf '%s\n' "$OUTW" | grep -oE 'LOST A WHERE: [0-9]+' | awk '{print $4+0}')
# 11: INSERT / UPDATE-SET / DELETE targets+columns that differ from the T-SQL
n11=$(printf '%s\n' "$OUTW" | grep -oE 'MISMATCHED: [0-9]+' | awk '{s+=$2} END {print s+0}')
if [ "${n10:-0}" != "0" ] || [ "${n11:-0}" != "0" ]; then
  printf '%s\n' "$OUTW" | grep -E '^\s+(!!|--)' | head -20 | sed 's/^/      /'
fi
rm -rf "$TMPW"
chk "10 UPDATE/DELETE that LOST its WHERE (hits every row)" "${n10:-0}"
chk "11 write path: INSERT/UPDATE/DELETE targets+columns differ" "${n11:-0}"

# 12: a single-row "SELECT .. INTO" whose WHERE is NOT unique in the DATA. T-SQL keeps the last row; MySQL
# raises ERROR 1172 and ABORTS the routine. Invisible to every other check here because the code looks fine
# — whether it fires depends on the table's contents — and it has already caused two live failures (the
# salary view, and every agent transfer). Re-runs the probe, so a DATA RELOAD that introduces new duplicates
# is caught here rather than by a user hitting a dead screen.
OUT12=$(python "$HERE/find_multirow_select_into.py" 2>/dev/null)
n12=$(printf '%s\n' "$OUT12" | grep -oE 'abort its routine: [0-9]+' | awk '{print $4+0}')
if [ "${n12:-0}" != "0" ]; then printf '%s\n' "$OUT12" | grep -E '^\s+(!!|  )' | head -12 | sed 's/^/      /'; fi
chk "12 SELECT..INTO over a NON-UNIQUE key (aborts with 1172)" "${n12:-0}"

echo
if [ "$fail" -eq 0 ]; then
  echo "ALL CLEAN."
else
  echo "FAILURES ABOVE — a silent-wrongness class is live. Do not ship."
fi
exit $fail
