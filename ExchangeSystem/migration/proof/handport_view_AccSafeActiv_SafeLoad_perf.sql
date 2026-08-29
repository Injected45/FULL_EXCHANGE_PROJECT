-- =====================================================================================
-- View_AccSafeActiv_SafeLoad — drop a GROUP BY that is provably a NO-OP
--
-- SYMPTOM (found by the team during UI testing, 2026-07-23):
--   AccSafeActivityTb_SelectByEmSafe -> "The Command Timeout expired before the operation completed."
--   It also hangs when run directly, so it is not a network or connector problem.
--
-- CAUSE:
-- The T-SQL view carries
--     GROUP BY a.InsertDate, a.ISID, a.SafeIDMovement, a.AccIDFrom, a.CurrencyID, a.IsActive,
--              a.Debit, a.Credit, a.AccBranchID, a.InsertDate, a.ID
-- and the port reproduced it faithfully. But the grouping key CONTAINS a.ID, the PRIMARY KEY of the
-- underlying table, so every group is exactly one row and the clause changes nothing. Measured:
--     base rows = 506,189      distinct grouping keys = 506,189
-- There is also no aggregate anywhere in the select list, so nothing depends on the grouping.
--
-- SQL Server's optimizer spots that functional dependency, eliminates the group-by, and pushes the
-- caller's WHERE into the view. MariaDB 10.4 does not: a GROUP BY blocks derived-table merging, so the
-- view is MATERIALISED IN FULL before any filter is applied. EXPLAIN showed
--     <derived2>  type=ALL  key=NULL  rows=488749  Using temporary; Using filesort
-- for a query that should touch a handful of rows.
--
-- That is bad twice over, because the select list calls the scalar function
--     SafeIDEMP_GetNetTotal(a.AccIDFrom, a.AccBranchID, a.CurrencyID, a.ID)
-- once PER ROW — so materialising the whole view runs it half a million times.
--
-- FIX: recreate the view WITHOUT the GROUP BY. Same rows, same columns, same order; the view can now be
-- merged, the WHERE reaches the base table's indexes, and the scalar function runs only for the rows the
-- caller actually asked for.
--
-- (MariaDB 10.6+ has derived_condition_pushdown, which would also solve this. 10.4 does not, so the
--  redundant clause has to go rather than being worked around with an optimizer switch.)
--
-- Nothing else changes: ROW_NUMBER() OVER (ORDER BY a.InsertDate) is evaluated over the identical row
-- set, and the collate on the formatted column is kept so the UNION-ing views still match collations.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

CREATE OR REPLACE VIEW `View_AccSafeActiv_SafeLoad` AS
SELECT
    ROW_NUMBER() OVER (ORDER BY `a`.`InsertDate`)                    AS `#`,
    `a`.`ISID`                                                       AS `الكود`,
    `a`.`InsertDate`                                                 AS `التاريخ`,
    `a`.`Debit`                                                      AS `مدين`,
    `a`.`Credit`                                                     AS `دائن`,
    FORMAT(`SafeIDEMP_GetNetTotal`(`a`.`AccIDFrom`, `a`.`AccBranchID`, `a`.`CurrencyID`, `a`.`ID`), 3)
        COLLATE utf8mb4_unicode_ci                                   AS `الصافي`,
    `a`.`SafeIDMovement`                                             AS `طبيعة الحركة`,
    `a`.`AccIDFrom`                                                  AS `AccIDFrom`,
    `a`.`CurrencyID`                                                 AS `CurrencyID`,
    `a`.`IsActive`                                                   AS `IsActive`,
    `a`.`Debit`                                                      AS `Debit`,
    `a`.`Credit`                                                     AS `Credit`,
    `a`.`AccBranchID`                                                AS `AccBranchID`,
    `a`.`InsertDate`                                                 AS `InsertDate`,
    `a`.`ID`                                                         AS `ID`
FROM `ExSyAccounts_AccSafeActivityTb` `a`;
