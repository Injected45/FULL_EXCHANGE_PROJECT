-- =====================================================================================
-- Hand-port: BalanceSheet_Statment_Detials
--
-- Why the converter could not do it: the proc uses the T-SQL ";WITH cte AS (..) SELECT .." idiom. The
-- leading semicolon (a T-SQL habit to terminate whatever came before) makes the ';'-insertion pass treat
-- the CTE and its main SELECT as two statements, so MySQL sees a bare "SELECT .. FROM Movements" with no
-- CTE in scope and fails at the SELECT.
--
-- Mechanical changes only:
--   ;WITH Movements AS (..)      -> WITH Movements AS (..)   (one statement, as MySQL requires)
--   N'رصيد سابق'                  -> 'رصيد سابق'   (the N prefix is a T-SQL Unicode marker; the column is
--                                   already utf8mb4, and leaving N'..' would concatenate a literal N)
--   ISNULL(x, y)                 -> IFNULL(x, y)
--   DATEADD(DAY, 1, @D2)         -> DATE_ADD(p_D2, INTERVAL 1 DAY)
--   FORMAT(x, 'N2')              -> FORMAT(x, 2)
--        T-SQL 'N2' = 2 decimals WITH thousands separators, which is exactly MySQL FORMAT(x,2) in the
--        default en-US locale. NOT the REPLACE(FORMAT(..),',','') form (that is for 'F2', no separators).
--   SELECT @v = expr FROM ..     -> SELECT expr INTO v_v FROM ..
--   DECLARE @x DECIMAL(18,3) = 0 -> DECLARE v_x DECIMAL(18,3) DEFAULT 0
--   dbo.X                        -> X
--   SET NOCOUNT / XACT_ABORT     -> dropped (no MySQL equivalent, no result-set effect)
--
-- The running-total window function
--     SUM(MovementValue) OVER (ORDER BY SortOrder, InsertDate, ID ROWS UNBOUNDED PRECEDING)
-- is kept verbatim — MariaDB 10.2+ supports window frames, and this one drives the Balance column.
--
-- Both UNION ALL branches, the account-nature CASE ladders (@AccType = 1 -> credit-natural, else
-- debit-natural), the opening-balance sign split into Debit/Credit, and the final ORDER BY are unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `BalanceSheet_Statment_Detials`;
DELIMITER $$
CREATE PROCEDURE `BalanceSheet_Statment_Detials`(IN `p_AccID` INT, IN `p_D1` DATE, IN `p_D2` DATE)
BEGIN
    -- --------------------------------------------------
    -- 1 جلب طبيعة الحساب
    -- 1 = دائن طبيعي
    -- غير ذلك = مدين طبيعي
    -- --------------------------------------------------
    DECLARE v_AccType TINYINT UNSIGNED;
    DECLARE v_OpeningBalance DECIMAL(18,3) DEFAULT 0;

    SELECT at.AccDmType
      INTO v_AccType
      FROM AccountsTb at
     WHERE at.AccID = p_AccID;

    -- --------------------------------------------------
    -- 2 حساب الرصيد السابق
    -- --------------------------------------------------
    SELECT IFNULL(SUM(a.Credit - a.Debit), 0)
      INTO v_OpeningBalance
      FROM ExSyAccounts_AccSafeActivityTb a
     WHERE a.AccIDFrom = p_AccID
       AND a.IsActive = 1
       AND a.InsertDate < p_D1;

    -- --------------------------------------------------
    -- 3 تجهيز الحركات + الرصيد السابق
    -- --------------------------------------------------
    WITH Movements AS
    (
        -- ----------------------------------------------
        -- صف الرصيد السابق
        -- ----------------------------------------------
        SELECT
            0 AS SortOrder,
            0 AS ID,
            'رصيد سابق' AS ISID,
            NULL AS InsertDate,
            at.AccParent,
            at.AccID,
            'رصيد سابق' AS AccName,
            c.AccName AS ParentName1,
            CASE WHEN v_OpeningBalance < 0
                 THEN ABS(v_OpeningBalance)
                 ELSE 0 END AS Debit,
            CASE WHEN v_OpeningBalance > 0
                 THEN v_OpeningBalance
                 ELSE 0 END AS Credit,
            '' AS MovementType,
            -- قيمة الحركة حسب طبيعة الحساب
            CASE
                WHEN v_AccType = 1
                     THEN v_OpeningBalance
                ELSE -v_OpeningBalance
            END AS MovementValue
        FROM AccountsTb at
        INNER JOIN AccountsTb c
            ON at.AccParent = c.AccCode
        WHERE at.AccID = p_AccID
        UNION ALL
        -- ----------------------------------------------
        -- العمليات الفعلية
        -- ----------------------------------------------
        SELECT
            1 AS SortOrder,
            b.ID,
            b.ISID,
            b.InsertDate,
            at.AccParent,
            at.AccID,
            at.AccName,
            c.AccName,
            b.Debit,
            b.Credit,
            b.MovementType,
            CASE
                WHEN v_AccType = 1
                    THEN (b.Credit - b.Debit)
                ELSE (b.Debit - b.Credit)
            END AS MovementValue
        FROM AccountsTb at
        INNER JOIN ExSyAccounts_AccSafeActivityTb b
            ON at.AccID = b.AccIDFrom
        INNER JOIN AccountsTb c
            ON at.AccParent = c.AccCode
        WHERE
            b.AccIDFrom = p_AccID
            AND b.IsActive = 1
            AND b.InsertDate >= p_D1
            AND b.InsertDate < DATE_ADD(p_D2, INTERVAL 1 DAY)
    )
    -- --------------------------------------------------
    -- 4 عرض النتيجة مع الرصيد التراكمي
    -- --------------------------------------------------
    SELECT
        InsertDate,
        ID,
        ISID,
        AccParent,
        AccID,
        AccName,
        ParentName1,
        FORMAT(Debit, 2)  AS Debit,
        FORMAT(Credit, 2) AS Credit,
        MovementType,
        FORMAT(
            SUM(MovementValue) OVER (
                ORDER BY SortOrder, InsertDate, ID
                ROWS UNBOUNDED PRECEDING
            ),
        2) AS Balance
    FROM Movements
    ORDER BY SortOrder, InsertDate, ID;
END$$
DELIMITER ;
