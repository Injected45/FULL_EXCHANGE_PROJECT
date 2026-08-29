#!/usr/bin/env bash
# =====================================================================================
# replay_internal_transfer.sh <HistoricalCode>
#
# Cross-engine money test for InternalEx_Insert1 WITHOUT writing to SQL Server.
#
# SQL Server's output for every historical transfer is already sitting in the migrated data, so a faithful
# port must reproduce it. This replays one historical transfer through the MySQL proc under a TEST- code,
# using that transfer's own parameters, and diffs the ledger rows it writes against the historical ones.
#
# The step SafeIDs are recovered from the historical rows themselves (a transfer is saved by one user,
# confirmed by another, delivered by a third), because passing the wrong one would produce a spurious diff.
#
# Everything it writes is removed again at the end. LOCAL ONLY - never point this at production.
# =====================================================================================
set -u
MY="/c/xampp/mysql/bin/mysql.exe -u root -N --default-character-set=utf8mb4 -D EXCHANGESYS2026"
SRC="${1:?usage: replay_internal_transfer.sh <HistoricalCode>}"
TEST="TESTR-$$"
q() { $MY -e "$1" 2>&1 | tr -d '\r'; }

read -r OV EX RC DC DP BR BD IAF AF IAT AT BB EXV ST <<<"$(q "
  SELECT CONCAT_WS(' ',OverallVal,ExVal,RecievedCurrencyID,DeliveredCurrencyID,DeliveryPlace,
         BranchRecievedID,BranchDeliveredID,IsAccFrom,AccFrom,IsAccTo,AccTo,IFNULL(BBRANCHID,0),
         IFNULL(EXTRVAL,0),IFNULL(ServiceType,0))
  FROM InternalEx WHERE Code='$SRC';")"
[ -z "${OV:-}" ] && { echo "no such transfer: $SRC"; exit 1; }

# Which steps actually happened, and who performed them, taken from the transfer itself.
# Driving this from the historical rows alone is wrong in two ways that both produce false diffs:
#   * a transfer left at ConfirmType = 1 was never delivered - replaying a deliver invents rows;
#   * some agent transfers post no OperationTypeID = 3 row at all, so "no op3" does NOT mean "never
#     confirmed" - the confirm still has to run or the deliver is rejected as out-of-sequence.
FINAL=$(q "SELECT ConfirmType FROM InternalEx WHERE Code='$SRC';")
S_SAVE=$(q "SELECT SafeID FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC' ORDER BY ID LIMIT 1;")
S_DELV=$(q "SELECT SafeID FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC' ORDER BY ID DESC LIMIT 1;")
S_CONF=$(q "SELECT SafeID FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC' AND OperationTypeID=3 ORDER BY ID LIMIT 1;")
[ -z "$S_CONF" ] && S_CONF="$S_DELV"

step() {  # $1 = SafeID, $2 = ConfirmType
  [ -z "$1" ] && return 0
  q "SET @m=0; SET @s='';
     CALL InternalEx_Insert1('$TEST','T SENDER','0910000000','','','T RECEIVER','0920000000','','',
       $RC,$DC,$OV,$EX, $1,$DP,$BR,$BD, $2, 'replay', @m,@s, 999999, $IAF,$AF,$IAT,$AT, $BB,$EXV,$ST, 0,0.000,0.000);
     SELECT CONCAT('   step ConfirmType=$2 safe=$1 -> ',@m,' ',@s);"
}
step "$S_SAVE" 0
[ "${FINAL:-0}" -ge 1 ] && step "$S_CONF" 1
[ "${FINAL:-0}" -ge 2 ] && step "$S_DELV" 2

COLS="CONCAT_WS('|',AccIDFrom,AccIDTo,Debit,Credit,CurrencyID,AccBranchID,TypeID,OperationTypeID,SafeID)"
q "SELECT $COLS FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC'   ORDER BY ID;" > /tmp/rp_ss.$$
q "SELECT $COLS FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST' ORDER BY ID;" > /tmp/rp_my.$$

n_ss=$(wc -l < /tmp/rp_ss.$$); n_my=$(wc -l < /tmp/rp_my.$$)
if diff -q /tmp/rp_ss.$$ /tmp/rp_my.$$ >/dev/null; then
  echo "   RESULT: MATCH  ($n_ss ledger rows identical to SQL Server)"
else
  echo "   RESULT: DIFF   (SQL Server $n_ss rows, MySQL $n_my rows)"
  diff /tmp/rp_ss.$$ /tmp/rp_my.$$ | head -12 | sed 's/^/      /'
fi
q "SELECT CONCAT('   balance: debit=',IFNULL(SUM(Debit),0),' credit=',IFNULL(SUM(Credit),0)) FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST';"

q "DELETE FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST'; DELETE FROM InternalEx WHERE Code='$TEST';" >/dev/null
left=$(q "SELECT (SELECT COUNT(*) FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST')+(SELECT COUNT(*) FROM InternalEx WHERE Code='$TEST');")
[ "$left" = "0" ] || echo "   !! cleanup left $left rows for $TEST"
rm -f /tmp/rp_ss.$$ /tmp/rp_my.$$
