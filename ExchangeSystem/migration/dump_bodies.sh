#!/usr/bin/env bash
# Dump every stored-procedure body from BOTH engines into /tmp for cmp_writes.py.
#
# IMPORTANT: do NOT flatten newlines in the SQL Server dump. The bodies contain `-- line comments`, and a
# comment can only be stripped if its terminating newline is still there. Flattening first makes `--[^\n]*`
# eat the entire rest of the procedure.
set -u
OUT=${1:-/tmp}
SRC='DESKTOP-M233HRE\SQLEXPRESS'

# ALL schemas the app writes through — not just the main one. ExSyAccounts2026 holds the 506k-row money
# ledger and its insert proc is reached from EXCHANGESYS2026 via a SYNONYM, so it is on the hottest write
# path in the system. Leaving it out of the comparison would leave the ledger writes unverified.
DBS="EXCHANGESYS2026 ExSyAccounts2026 ExSyAccountsCurrency2026"

: > "$OUT/ss_bodies.txt"
: > "$OUT/my_bodies.txt"

for DB in $DBS; do
  # proc names are prefixed with the schema so identically-named procs in different schemas
  # (e.g. AccSafeActivityTb_Insert exists in BOTH ExSyAccounts2026 and ExSyAccountsCurrency2026)
  # are never conflated.
  sqlcmd -S "$SRC" -E -d "$DB" -y 0 -Q "SET NOCOUNT ON;
  SELECT '###${DB}__' + o.name + '###' + m.definition
  FROM sys.objects o JOIN sys.sql_modules m ON o.object_id=m.object_id
  WHERE o.type='P' AND o.is_ms_shipped=0;" -l 60 2>/dev/null >> "$OUT/ss_bodies.txt"

  /c/xampp/mysql/bin/mysql.exe -u root -N --default-character-set=utf8mb4 -e "
  SELECT CONCAT('###${DB}__', ROUTINE_NAME, '###', ROUTINE_DEFINITION) FROM information_schema.ROUTINES
  WHERE ROUTINE_SCHEMA='$DB' AND ROUTINE_TYPE='PROCEDURE';" 2>/dev/null | grep -v "prefix" >> "$OUT/my_bodies.txt"
done

echo "ss_bodies.txt: $(grep -c '###' "$OUT/ss_bodies.txt") procs"
echo "my_bodies.txt: $(grep -c '###' "$OUT/my_bodies.txt") procs"
