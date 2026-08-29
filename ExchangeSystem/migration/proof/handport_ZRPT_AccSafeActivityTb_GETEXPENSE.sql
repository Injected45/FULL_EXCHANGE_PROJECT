-- =====================================================================================
-- Hand-port: ZRPT_AccSafeActivityTb_GETEXPENSE
--
-- Why the converter could not do it: the proc's FROM is a MULTI-STATEMENT table-valued function,
--     FROM dbo.AccSafeActivityTb_GETEXPENSEBALANCE(@BranchID, @D1, @D2, 1) AS x
-- (RETURNS @TMT TABLE .. INSERT INTO @TMT .. RETURN). MySQL has no table-valued functions, and the
-- converter only inlines INLINE TVFs (single RETURN(SELECT ..)).
--
-- The TVF is flattened into the proc as a TEMPORARY TABLE with the SAME declared column types —
-- that matters, because the declared types are an implicit CAST: ACCCODE is NVARCHAR(MAX) while
-- the value inserted is the numeric A.ISID, so the report receives ISID as TEXT, and Balance is
-- DECIMAL(13,3) which ROUNDS the scalar function's result. Dropping either cast would silently
-- change what the report prints.
--
-- Both @CurrencyFrom branches are reproduced even though this proc always passes 1: keeping the
-- branch makes the port a faithful copy of the function rather than a specialisation of it.
--
-- Mechanical changes only:
--   @TMT TABLE(..)             -> TEMPORARY TABLE tmp_AccSafeActivityTb_GETEXPENSEBALANCE(..)
--   NVARCHAR(MAX)              -> LONGTEXT
--   [dbo].X / dbo.X            -> X
--   FORMAT(x.InsertDate, 'yyyy/MM/dd') -> DATE_FORMAT(x.InsertDate, '%Y/%m/%d')
--   BEGIN TRANSACTION          -> START TRANSACTION (+ a COMMIT the source omits; the proc is
--                                 read-only, so this changes nothing except not leaving an open
--                                 transaction behind on a pooled MySQL connection)
--   SET NOCOUNT / XACT_ABORT   -> dropped (no MySQL equivalent, no result-set effect)
--
-- The commented-out JOINs in the source are kept commented. Column order, the Arabic aliases, and
-- the (deliberate) source pairing of x.Credit->'دائن' / x.Debit->'مدين' are unchanged.
-- SafeIDEMP_GetNetTotal1 already exists in MySQL as a FUNCTION (verified).
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ZRPT_AccSafeActivityTb_GETEXPENSE`;
DELIMITER $$
CREATE PROCEDURE `ZRPT_AccSafeActivityTb_GETEXPENSE`(IN `p_BranchID` INT, IN `p_D1` DATE, IN `p_D2` DATE)
BEGIN
    DECLARE v_CurrencyFrom INT DEFAULT 1;   -- the constant the source passes as the 4th TVF arg

    START TRANSACTION;

    DROP TEMPORARY TABLE IF EXISTS tmp_AccSafeActivityTb_GETEXPENSEBALANCE;
    CREATE TEMPORARY TABLE tmp_AccSafeActivityTb_GETEXPENSEBALANCE
    (
        AccName      LONGTEXT,
        ACCCODE      LONGTEXT,
        Credit       DECIMAL(13, 3),
        Debit        DECIMAL(13, 3),
        InsertDate   DATE,
        Balance      DECIMAL(13, 3),
        Note         LONGTEXT,
        MovementType LONGTEXT
    );

    IF v_CurrencyFrom = 1 THEN
        INSERT INTO tmp_AccSafeActivityTb_GETEXPENSEBALANCE
            SELECT
                    B.AccName,
                    A.ISID                                                              AS ACCCODE,
                    A.Credit,
                    A.Debit,
                    A.InsertDate,
                    SafeIDEMP_GetNetTotal1(A.AccBranchID, 1, A.ID, p_D1, p_D2)          AS Balance,
                    A.Note,
                    A.MovementType
            FROM
                    ExSyAccounts_AccSafeActivityTb AS A
                INNER JOIN
                    PCSettlementDetailsTB AS e
                        ON A.AccIDFrom = e.AccIDEX
                INNER JOIN
                    AccountsTb AS B
                        ON A.AccIDFrom = B.AccID
--              INNER JOIN
--                  ANOTHEREXPENSTB AS F
--                      ON A.AccIDFrom = F.AccIDPC
            WHERE
                    A.AccBranchID = p_BranchID
                    AND A.InsertDate BETWEEN p_D1 AND p_D2
                    AND A.IsActive = 1
                    AND e.IsActive = 1
                    AND A.CurrencyID = 1
            UNION
            SELECT
                    B.AccName,
                    A.ISID                                                              AS ACCCODE,
                    A.Credit,
                    A.Debit,
                    A.InsertDate,
                    SafeIDEMP_GetNetTotal1(A.AccBranchID, 1, A.ID, p_D1, p_D2)          AS Balance,
                    A.Note,
                    A.MovementType
            FROM
                    ExSyAccounts_AccSafeActivityTb AS A
--              INNER JOIN
--                  PCSettlementDetailsTB AS e
--                      ON A.AccIDFrom = e.AccIDEX
                INNER JOIN
                    AccountsTb AS B
                        ON A.AccIDFrom = B.AccID
                INNER JOIN
                    ANOTHEREXPENSTB AS F
                        ON A.AccIDFrom = F.AccIDPC
            WHERE
                    A.AccBranchID = p_BranchID
                    AND A.InsertDate BETWEEN p_D1 AND p_D2
                    AND A.IsActive = 1
                    AND F.IsActive = 1
                    AND A.CurrencyID = 1;
    END IF;

    IF v_CurrencyFrom <> 1 THEN
        INSERT INTO tmp_AccSafeActivityTb_GETEXPENSEBALANCE
            SELECT
                    B.AccName,
                    A.ISID                                                              AS ACCCODE,
                    A.Credit,
                    A.Debit,
                    A.InsertDate,
                    SafeIDEMP_GetNetTotal1(A.AccBranchID, v_CurrencyFrom, A.ID, p_D1, p_D2) AS Balance,
                    A.Note,
                    A.MovementType
            FROM
                    ExSyAccountsCurrency_AccSafeActivityTb AS A
                INNER JOIN
                    PCSettlementDetailsTB AS e
                        ON A.AccIDFrom = e.AccIDEX
                INNER JOIN
                    AccountsTb AS B
                        ON e.AccIDEX = B.AccID
--              INNER JOIN
--                  ANOTHEREXPENSTB AS F
--                      ON A.AccIDFrom = F.AccIDPC
            WHERE
                    A.AccBranchID = p_BranchID
                    AND A.InsertDate BETWEEN p_D1 AND p_D2
                    AND A.CurrencyID = v_CurrencyFrom
                    AND A.IsActive = 1
            UNION
            SELECT
                    B.AccName,
                    A.ISID                                                              AS ACCCODE,
                    A.Credit,
                    A.Debit,
                    A.InsertDate,
                    SafeIDEMP_GetNetTotal1(A.AccBranchID, v_CurrencyFrom, A.ID, p_D1, p_D2) AS Balance,
                    A.Note,
                    A.MovementType
            FROM
                    ExSyAccountsCurrency_AccSafeActivityTb AS A
--              INNER JOIN
--                  PCSettlementDetailsTB AS e
--                      ON A.AccIDFrom = e.AccIDEX
                INNER JOIN
                    AccountsTb AS B
                        ON A.AccIDFrom = B.AccID
                INNER JOIN
                    ANOTHEREXPENSTB AS F
                        ON A.AccIDFrom = F.AccIDPC
            WHERE
                    A.AccBranchID = p_BranchID
                    AND A.InsertDate BETWEEN p_D1 AND p_D2
                    AND A.IsActive = 1
                    AND F.IsActive = 1
                    AND A.CurrencyID = v_CurrencyFrom;
    END IF;

    SELECT
            x.ACCCODE                                  AS 'الرمز',
            DATE_FORMAT(x.InsertDate, '%Y/%m/%d')      AS 'التاريخ',
            x.AccName                                  AS 'اسم الحساب',
            x.Credit                                   AS 'دائن',
            x.Debit                                    AS 'مدين',
            x.MovementType                             AS 'طبيعة الحركة',
            x.Note                                     AS 'ملاحظات'
    FROM
            tmp_AccSafeActivityTb_GETEXPENSEBALANCE AS x;

    DROP TEMPORARY TABLE IF EXISTS tmp_AccSafeActivityTb_GETEXPENSEBALANCE;

    COMMIT;
END$$
DELIMITER ;
