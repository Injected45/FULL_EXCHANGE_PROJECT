#!/usr/bin/env bash
# =====================================================================================
# apply_handports.sh — re-apply every hand-ported routine in migration/proof/
#
# WHY THIS EXISTS (read before you skip it):
# The migrator's `functions` / `procs` / `hardprocs` commands begin each object with
#     DROP FUNCTION|PROCEDURE IF EXISTS <name>;
# and then try to CREATE the auto-converted version. For an object we hand-ported *because* the
# converter cannot do it, that CREATE fails — so the DROP has silently DELETED the working hand-port
# and left nothing behind. The routine simply vanishes from MySQL and the screen that calls it breaks.
#
# RULE: after ANY `migrator functions|procs|hardprocs|views|tvpprocs` run, re-run THIS script.
#
# Order matters: cross-DB objects and the synonym shims first (other routines resolve against them),
# then functions (procs call them), then procs.
# =====================================================================================
set -u
MYSQL="/c/xampp/mysql/bin/mysql.exe -u root --default-character-set=utf8mb4"
PROOF="$(cd "$(dirname "$0")" && pwd)/proof"

# explicit order; anything else in proof/ is applied afterwards in name order
ORDERED=(
  handport_crossdb_EXCHANGESYS_compat.sql
  handport_crossdb_ExSyAccounts2026_AccSafeActivityTb_Insert.sql
  handport_crossdb_ExSyAccountsCurrency2026_AccSafeActivityTb_Insert.sql
  handport_synonyms.sql
  handport_Extract_text_functions.sql
  handport_NormalizePhone.sql
  handport_CurrencyPriceShow.sql
)

applied=0; failed=0
apply() {
  local f="$1"
  [ -f "$PROOF/$f" ] || return 0
  local err
  err=$($MYSQL < "$PROOF/$f" 2>&1 | grep -viE "insecure|password on the command line" | grep -i "ERROR" | head -1)
  if [ -n "$err" ]; then
    echo "  FAIL  $f"
    echo "        $err"
    failed=$((failed+1))
  else
    echo "  ok    $f"
    applied=$((applied+1))
  fi
}

echo "Applying hand-ports from $PROOF"
for f in "${ORDERED[@]}"; do apply "$f"; done
for p in "$PROOF"/*.sql; do
  f=$(basename "$p")
  case " ${ORDERED[*]} " in *" $f "*) continue ;; esac
  apply "$f"
done

echo
echo "hand-ports applied: $applied, failed: $failed"
[ "$failed" -eq 0 ] || exit 1
