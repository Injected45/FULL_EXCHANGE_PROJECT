#!/usr/bin/env bash
# =====================================================================================
# replay_external_transfer.sh <HistoricalCode>
#
# The ExternalEx_Insert counterpart of replay_internal_transfer.sh. Same idea: SQL Server's output for
# every historical transfer is already in the migrated data, so a faithful port must reproduce it. This
# replays one historical transfer through the MySQL proc under a TESTX- code, using that transfer's own
# 62 parameters, and diffs the ledger rows against the historical ones.
#
# Six of the 62 parameters are NOT recoverable from the row (they come from form state):
#   p_AgentCheck        -> TransType, which the form binds from the same combo
#   p_EMPCUSTSELECT / p_EMPTOSELECT -> lookup selections; only consulted when IsAccFrom/IsAccTo say the
#                          counterparty is an ACCOUNT, so they are passed 0 and only cash/branch transfers
#                          (IsAccFrom = IsAccTo = 0) are trustworthy targets for this harness
#   p_IsHandelExAVal / p_HandelExAVal -> hand-carried-fee flags, passed 0
#   p_NewPrice_SenDForWhatsapp -> an OUT-only value the proc writes
# A DIFF on a transfer that uses accounts should therefore be investigated as a possible harness gap
# BEFORE it is called a port defect.
#
# Everything it writes is removed again at the end. LOCAL ONLY - never point this at production.
# =====================================================================================
set -u
MY="/c/xampp/mysql/bin/mysql.exe -u root -N --default-character-set=utf8mb4 -D EXCHANGESYS2026"
SRC="${1:?usage: replay_external_transfer.sh <HistoricalCode> [AgentCheck]}"
# p_AgentCheck comes from a combo box (TransType.SelectedIndex), NOT directly from the TransType column,
# so it can be overridden to find the value the historical run actually used.
AGENT="${2:-}"
TEST="TESTX-$$"
q_early() { $MY -e "$1" 2>&1 | tr -d "
"; }
q() { $MY -e "$1" 2>&1 | tr -d '\r'; }

FINAL=$(q "SELECT IFNULL(ConfirmedType,0) FROM ExternalEx WHERE Code='$SRC';")
[ -z "$FINAL" ] && { echo "no such transfer: $SRC"; exit 1; }
[ -z "$AGENT" ] && AGENT=$(q "SELECT IFNULL(TransType,0) FROM ExternalEx WHERE Code='$SRC';")

# build the CALL from the row itself so no parameter is transcribed by hand
mkcall() {   # $1 = SafeRecievedID to use, $2 = ConfirmedType step
  q "SELECT CONCAT(
      'CALL ExternalEx_Insert(999999,''$TEST'',''T SENDER'',''0910000000'','''','''',',
      RecievedCurrencyID,',',CountryIDFrom,',',RecievedBranchID,',''T RECEIVER'',''0920000000'','''',',
      IFNULL(CityIDTo,0),',',DeliveredCurrencyID,',',CountryIDTo,',$AGENT,',
      IFNULL(IsPrivateAccount,0),','''','''',',IFNULL(ServiceType,0),',',IFNULL(IsServiceVal,0),',',
      IFNULL(ServiceExVal,0),',',IFNULL(BranchDeleviredID,0),',',IFNULL(CurrRecievedVal,0),',',
      IFNULL(ExVal,0),',',IFNULL(ExtraVal,0),',',IFNULL(NetTotal,0),',''',InsertDate,''',',
      $1,',',IFNULL(SafeDeliveredID,0),',NULL,',IFNULL(IsDelivered,0),',''replay'',',
      IFNULL(IsAccFrom,0),',',IFNULL(AccFrom,0),',',IFNULL(IsAccTo,0),',',IFNULL(AccTo,0),',',
      IFNULL(IsCash,0),',',IFNULL(TransPrice,0),',',IFNULL(BBranchAccID,0),',',IFNULL(BBRANCHID,0),
      ',0,',IFNULL(BankServiceType,0),',\@m,''replay'','''',\@s,0,0,',IFNULL(IsInOrOut,0),',$2,',
      IFNULL(NewTrancPrice,0),',',IFNULL(NewFinalTotal,0),',',IFNULL(OurAccID,0),',',
      IFNULL(ConfirmedSafeID,0),',NULL,',IFNULL(CurrDeliveredVal,0),',0,0,',IFNULL(BankIDTo,0),',',
      IFNULL(TransPrice1,0),',\@w);')
    FROM ExternalEx WHERE Code='$SRC';"
}

step() {   # $1 = SafeID, $2 = ConfirmedType
  local c; c=$(mkcall "$1" "$2")
  [ -z "$c" ] && return 0
  printf 'SET @m=0; SET @s=%s; SET @w=0;\n%s\nSELECT CONCAT("   step ConfirmedType=%s safe=%s -> ",@m," ",@s);\n' "''" "$c" "$2" "$1" > /tmp/xc.$$
  $MY < /tmp/xc.$$ 2>&1 | tr -d '\r' | tail -1
  rm -f /tmp/xc.$$
}

S_SAVE=$(q "SELECT SafeID FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC' ORDER BY ID LIMIT 1;")
S_LAST=$(q "SELECT SafeID FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC' ORDER BY ID DESC LIMIT 1;")
[ -z "$S_SAVE" ] && S_SAVE=$(q "SELECT IFNULL(SafeRecievedID,0) FROM ExternalEx WHERE Code='$SRC';")
[ -z "$S_LAST" ] && S_LAST="$S_SAVE"

step "$S_SAVE" 0
[ "${FINAL:-0}" -ge 1 ] && step "$S_LAST" 1
[ "${FINAL:-0}" -ge 2 ] && step "$S_LAST" 2

COLS="CONCAT_WS('|',AccIDFrom,AccIDTo,Debit,Credit,CurrencyID,AccBranchID,TypeID,OperationTypeID,SafeID)"
q "SELECT $COLS FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$SRC'  ORDER BY ID;" > /tmp/xp_ss.$$
q "SELECT $COLS FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST' ORDER BY ID;" > /tmp/xp_my.$$
n_ss=$(wc -l < /tmp/xp_ss.$$); n_my=$(wc -l < /tmp/xp_my.$$)
if diff -q /tmp/xp_ss.$$ /tmp/xp_my.$$ >/dev/null; then
  echo "   RESULT: MATCH  ($n_ss ledger rows identical to SQL Server)"
else
  echo "   RESULT: DIFF   (SQL Server $n_ss rows, MySQL $n_my rows)"
  diff /tmp/xp_ss.$$ /tmp/xp_my.$$ | head -12 | sed 's/^/      /'
fi
q "SELECT CONCAT('   balance: debit=',IFNULL(SUM(Debit),0),' credit=',IFNULL(SUM(Credit),0)) FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST';"

q "DELETE FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST'; DELETE FROM ExternalEx WHERE Code='$TEST';" >/dev/null
left=$(q "SELECT (SELECT COUNT(*) FROM ExSyAccounts_AccSafeActivityTb WHERE ISID='$TEST')+(SELECT COUNT(*) FROM ExternalEx WHERE Code='$TEST');")
[ "$left" = "0" ] || echo "   !! cleanup left $left rows for $TEST"
rm -f /tmp/xp_ss.$$ /tmp/xp_my.$$
