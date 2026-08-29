-- =====================================================================================
-- Hand-port: AccSafeActivityTb_SelectByEmSafe — read the base table, not the view
--
-- SYMPTOM (reported from UI testing, 2026-07-23):
--   "The Command Timeout expired before the operation completed."
-- It hangs when run directly too, so it was never a network or connector problem. A client timeout does
-- NOT cancel the server-side query: the abandoned call was still running 19 MINUTES later, holding a
-- metadata lock that blocked DDL on the view.
--
-- CAUSE: the proc reads View_AccSafeActiv_SafeLoad, whose select list contains
--     ROW_NUMBER() OVER (ORDER BY a.InsertDate)                          -- a window function
--     FORMAT(SafeIDEMP_GetNetTotal(a.AccIDFrom, a.AccBranchID, ...), 3)  -- a per-row scalar function
-- A window function (like a GROUP BY) BLOCKS derived-table merging in MariaDB, so the view is
-- MATERIALISED IN FULL before the caller's WHERE is applied — 488,749 rows, with the scalar function
-- evaluated once per row — to return a handful. SQL Server merges the view and never pays that cost.
-- (MariaDB 10.6+ could push the condition down; 10.4 cannot.)
--
-- FIX: this proc uses NEITHER of those two columns — it computes its own ROW_NUMBER() OVER (ORDER BY
-- a.ID) and its own running balance from SUM(..) OVER (..). Three of its four references to the view
-- only aggregate a.Debit / a.Credit. So the view reference is replaced with a derived table over the
-- base table exposing exactly the columns this proc uses, under the same names:
--     ISID -> `الكود`   InsertDate -> `التاريخ`   Debit -> `مدين`   Credit -> `دائن`
--     SafeIDMovement -> `طبيعة الحركة`   (plus Debit, Credit, AccIDFrom, CurrencyID, IsActive,
--                                         AccBranchID, InsertDate, ID unchanged)
-- With no window function in it, that derived table MERGES, the WHERE reaches the ledger's indexes, and
-- the result set is unchanged — same columns, same order, same values.
--
-- View_AccSafeActiv_SafeLoad is deliberately left in place (it is part of the migrated schema) and is
-- now cheaper anyway: handport_view_AccSafeActiv_SafeLoad_perf.sql removed a GROUP BY from it that was a
-- proven no-op. This proc was its only consumer.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `AccSafeActivityTb_SelectByEmSafe`;
DELIMITER $$
CREATE PROCEDURE `AccSafeActivityTb_SelectByEmSafe`(IN `p_BranchID` INT, IN `p_CurrencyID` INT, IN `p_SafeID` BIGINT, IN `p_D1` DATE, IN `p_D2` DATE, INOUT `p_SumDebitFinal` DECIMAL(15,3), INOUT `p_SumCreditFinal` DECIMAL(15,3), INOUT `p_OverAllNetTotalFinal` DECIMAL(15,3), INOUT `p_OverAllPeroidTotal` DECIMAL(15,3), INOUT `p_PreviewBalanceTotal` DECIMAL(15,3))
BEGIN
DECLARE v_ABranchID INT;
DECLARE v_ACurrencyID INT;
DECLARE v_ASafeID BIGINT;
DECLARE v_AD1 DATE;
DECLARE v_AD2 DATE;
DECLARE v_OldPreviwovs DECIMAL(18, 3);
DECLARE v_OverAllNetTotal DECIMAL(12, 3);
DECLARE v_SumDebit  DECIMAL(12, 3);
DECLARE v_SumCredit DECIMAL(12, 3);
DECLARE v_IsDefault BIT;


    START TRANSACTION;
    SET v_ABranchID = p_BranchID;
    SET v_ACurrencyID = p_CurrencyID;
    SET v_ASafeID = p_SafeID;
    SET v_AD1 = p_D1;
    SET v_AD2 = p_D2;
    SELECT CMT.IsDefault INTO v_IsDefault FROM
            CurrencyMainTb AS CMT
    WHERE
            CMT.ID = v_ACurrencyID
 ORDER BY CMT.ID DESC LIMIT 1;


    IF v_IsDefault = 1
       THEN
        SELECT SUM(a.Debit) - SUM(a.Credit) INTO v_OverAllNetTotal FROM
                ExSyAccounts_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.AccBranchID = v_ABranchID;

        SELECT IFNULL(SUM(a.Debit) - SUM(a.Credit), 0) INTO v_OldPreviwovs FROM
                ExSyAccounts_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = p_CurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = p_SafeID
                AND a.InsertDate < p_D1
                AND a.AccBranchID = p_BranchID;

        SELECT
                ''                                         AS 'SN',
                'الــــرصــــيـــــد الـــــســـــابــــق' AS 'الكود',
                'الــــرصــــيـــــد' AS 'RPTCode',
               'الـــــســـــابــــق' AS 'RPTDate',
                ''                                         AS 'التاريخ',
                ''                                         AS 'مدين',
                ''                                         AS 'دائن',
                v_OldPreviwovs                              AS 'الصافي',
                ''                                         AS 'طبيعة الحركة',
                v_OldPreviwovs                              AS OldPreviwovs,
                v_OverAllNetTotal                           AS OverAllNetTotal
        UNION ALL
        SELECT
                ROW_NUMBER() OVER (
                ORDER BY a.ID ASC) AS 'SN',

                `الكود`,
                a.الكود AS 'RPTCode',
CAST(`التاريخ` AS CHAR) AS 'RPTDate',
                CAST(`التاريخ` AS CHAR),
                CAST(`مدين` AS CHAR),
                CAST(`دائن` AS CHAR),
                CAST((SUM(`مدين`) OVER (PARTITION BY a.AccIDFrom ORDER BY a.id)
                - SUM(`دائن`) OVER (PARTITION BY a.AccIDFrom ORDER BY a.id)) + v_OldPreviwovs AS CHAR)
                AS 'الصافي',

                a.`طبيعة الحركة`,

                v_OldPreviwovs              AS OldPreviwovs,
                v_OverAllNetTotal           AS OverAllNetTotal
        FROM
                (SELECT b.ISID AS `الكود`, b.InsertDate AS `التاريخ`, b.Debit AS `مدين`, b.Credit AS `دائن`, b.SafeIDMovement AS `طبيعة الحركة`, b.Debit, b.Credit, b.AccIDFrom, b.CurrencyID, b.IsActive, b.AccBranchID, b.InsertDate, b.ID FROM ExSyAccounts_AccSafeActivityTb b) AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;




        SELECT SUM(a.Debit) INTO v_SumDebit FROM
                (SELECT b.ISID AS `الكود`, b.InsertDate AS `التاريخ`, b.Debit AS `مدين`, b.Credit AS `دائن`, b.SafeIDMovement AS `طبيعة الحركة`, b.Debit, b.Credit, b.AccIDFrom, b.CurrencyID, b.IsActive, b.AccBranchID, b.InsertDate, b.ID FROM ExSyAccounts_AccSafeActivityTb b) AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SELECT SUM(a.Credit) INTO v_SumCredit FROM
                (SELECT b.ISID AS `الكود`, b.InsertDate AS `التاريخ`, b.Debit AS `مدين`, b.Credit AS `دائن`, b.SafeIDMovement AS `طبيعة الحركة`, b.Debit, b.Credit, b.AccIDFrom, b.CurrencyID, b.IsActive, b.AccBranchID, b.InsertDate, b.ID FROM ExSyAccounts_AccSafeActivityTb b) AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SET p_SumDebitFinal = v_SumDebit;
        SET p_SumCreditFinal = v_SumCredit;
        SELECT SUM(a.Debit) - SUM(a.Credit) INTO p_OverAllNetTotalFinal FROM
                ExSyAccounts_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.AccBranchID = v_ABranchID;
        SELECT SUM(a.Debit) - SUM(a.Credit) INTO p_OverAllPeroidTotal FROM
                ExSyAccounts_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SELECT IFNULL(SUM(a.Debit) - SUM(a.Credit), 0.000) INTO p_PreviewBalanceTotal FROM
                (SELECT b.ISID AS `الكود`, b.InsertDate AS `التاريخ`, b.Debit AS `مدين`, b.Credit AS `دائن`, b.SafeIDMovement AS `طبيعة الحركة`, b.Debit, b.Credit, b.AccIDFrom, b.CurrencyID, b.IsActive, b.AccBranchID, b.InsertDate, b.ID FROM ExSyAccounts_AccSafeActivityTb b) AS a
        WHERE
                a.IsActive = 1
                AND (   a.CurrencyID = v_ACurrencyID
                        OR a.CurrencyID = 13)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
      END IF;
    IF v_IsDefault = 0
       THEN
        SELECT SUM(a.Credit) - SUM(a.Debit) INTO v_OverAllNetTotal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.AccBranchID = v_ABranchID;
        SELECT IFNULL(SUM(a.Credit) - SUM(a.Debit), 0) INTO v_OldPreviwovs FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = p_CurrencyID)
                AND a.AccIDFrom = p_SafeID
                AND a.InsertDate < p_D1
                AND a.AccBranchID = p_BranchID;

        SELECT
                ''                                         AS 'SN',
                'الــــرصــــيـــــد الـــــســـــابــــق' AS 'الكود',
                ''                                         AS 'التاريخ',
                ''                                         AS 'مدين',
                ''                                         AS 'دائن',
                v_OldPreviwovs                              AS 'الصافي',
                ''                                         AS 'طبيعة الحركة',
                v_OldPreviwovs                              AS OldPreviwovs,
                v_OverAllNetTotal                           AS OverAllNetTotal
        UNION ALL

        SELECT
                CAST(ROW_NUMBER() OVER (ORDER BY a.InsertDate ASC) AS CHAR) AS `SN`,
                a.ISID                                                           AS `الكود`,
                CAST(a.InsertDate AS CHAR)                                  AS `التاريخ`,
                CAST(a.Credit AS CHAR)                                      AS `مدين`,
                CAST(a.Debit AS CHAR)                                       AS `دائن`,
                CAST((SUM(a.Credit) OVER (PARTITION BY a.AccIDFrom ORDER BY a.id)
                - SUM(a.Debit) OVER (PARTITION BY a.AccIDFrom ORDER BY a.id)) + v_OldPreviwovs AS CHAR)
                AS 'الصافي',
                a.SafeIDMovement                                                 AS `طبيعة الحركة`,
                v_OldPreviwovs                                                    AS OldPreviwovs,
                v_OverAllNetTotal                                                 AS OverAllNetTotal
        FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SELECT SUM(a.Credit) INTO v_SumDebit FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SELECT SUM(a.Debit) INTO v_SumCredit FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SET p_SumDebitFinal = v_SumDebit;
        SET p_SumCreditFinal = v_SumCredit;
        SELECT SUM(a.Credit) - SUM(a.Debit) INTO p_OverAllNetTotalFinal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.AccBranchID = v_ABranchID;
        SELECT SUM(a.Credit) - SUM(a.Debit) INTO p_OverAllPeroidTotal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate >= v_AD1
                AND a.InsertDate <= v_AD2
                AND a.AccBranchID = v_ABranchID;
        SELECT IFNULL(SUM(a.Credit) - SUM(a.Debit), 0.000) INTO p_PreviewBalanceTotal FROM
                ExSyAccountsCurrency_AccSafeActivityTb AS a
        WHERE
                a.IsActive = 1
                AND (a.CurrencyID = v_ACurrencyID)
                AND a.AccIDFrom = v_ASafeID
                AND a.InsertDate < v_AD1
                AND a.AccBranchID = v_ABranchID;
      END IF;
    SELECT
            p_SumDebitFinal,
            p_SumCreditFinal,
            p_OverAllPeroidTotal,
            p_OverAllNetTotalFinal AS OverAllNetTotalFinal,
            p_PreviewBalanceTotal,
            v_OldPreviwovs         AS OldPreviwovs;
    COMMIT;
  END$$
DELIMITER ;
