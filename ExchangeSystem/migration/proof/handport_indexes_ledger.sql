-- =====================================================================================
-- Indexes on the money ledger ExSyAccounts2026.AccSafeActivityTb (506,189 rows).
--
-- WHY THIS EXISTS: indexes never change a result, but here their absence made a screen unusable.
-- AccSafeActivityTb_TOTETDATE — called ONCE PER ROW by the account-statement procs — runs
--     SELECT SUM(Debit)-SUM(Credit) FROM ExSyAccounts_AccSafeActivityTb
--      WHERE AccIDFrom = ? AND IsActive = 1 AND InsertDate < ?
-- For a busy account (AccID 517) MySQL could only use AccIDFronidx (AccIDFrom alone), read ~108,000
-- rows and then filter — about 1 second PER CALL. Over the 1,327 statement rows that is ~20 minutes,
-- and ACCOUNTSTB_SelectICOUNTDetelles2 simply hung. SQL Server does the same work quickly because
-- AccIDFrom is its CLUSTERED key (rows physically contiguous) and because of the covering indexes below.
--
-- 1-2: restore the two indexes SQL Server has that the schema migration did not carry over.
--      SQL Server's INCLUDE(col) has no MySQL equivalent — a trailing key column is the standard
--      substitute, giving the same index-only coverage (it only costs a little extra key width).
-- 3:   MySQL-specific. It replaces SQL Server's clustered-on-AccIDFrom physical ordering for exactly
--      the TOTETDATE predicate, so the lookup becomes index-only instead of 108k row reads.
--
-- IF NOT EXISTS keeps this idempotent, so apply_handports.sh can re-run it safely.
-- =====================================================================================
USE ExSyAccounts2026;

-- 1) = SQL Server IDX_AccSafeActivityTb2 KEY(IsActive,AccBranchID,AccIDFrom,ID,CurrencyID) INCLUDE(Debit)
CREATE INDEX IF NOT EXISTS `IDX_AccSafeActivityTb2`
    ON `AccSafeActivityTb` (`IsActive`, `AccBranchID`, `AccIDFrom`, `ID`, `CurrencyID`, `Debit`);

-- 2) = SQL Server IDX_AccSafeActivityTb4 KEY(IsActive,AccBranchID,InsertDate,CurrencyID) INCLUDE(Credit)
CREATE INDEX IF NOT EXISTS `IDX_AccSafeActivityTb4`
    ON `AccSafeActivityTb` (`IsActive`, `AccBranchID`, `InsertDate`, `CurrencyID`, `Credit`);

-- 3) covering index for AccSafeActivityTb_TOTETDATE / the running-balance functions
CREATE INDEX IF NOT EXISTS `IX_my_AccIDFrom_IsActive_InsertDate`
    ON `AccSafeActivityTb` (`AccIDFrom`, `IsActive`, `InsertDate`, `Debit`, `Credit`);
