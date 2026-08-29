-- =====================================================================================
-- Hand-port: the PARAMETERIZED table-valued function GET_TABLE_FOR_CostofByBranch(@BranchId)
-- and its only caller GET_TABLE_FOR_Costof_Proc.
--
-- MySQL/MariaDB has NO table-valued functions, and a VIEW cannot take a parameter. But this TVF's ONLY use of
-- its parameter is the predicate  "AND a.BID = @BranchId"  — and a.BID is a GROUP BY KEY that the function also
-- RETURNS as a column. When the filter column is a grouping key, filtering BEFORE the GROUP BY and filtering
-- AFTER it select exactly the same groups (each group lies entirely within one BID). So:
--
--     dbo.GET_TABLE_FOR_CostofByBranch(@x)  ==  SELECT * FROM <view without that predicate> WHERE BID = @x
--
-- The view therefore drops ONLY "AND a.BID = @BranchId"; every other predicate, join, aggregate, expression and
-- GROUP BY key is byte-for-byte the original. The caller re-applies the filter in a derived table, so its own
-- shape (alias `a`, column list) is untouched.
--
-- Mechanical translations only: dbo. dropped; ISNULL -> IFNULL; the two unqualified columns in the
-- costmerForPreseEn subquery are qualified to their (unambiguous) owners — TB_Users has AccID and
-- ExSyAccountsCurrency_AccSafeActivityTb does not, so `accid` is y.AccID and `AccIDTo` is x.AccIDTo.
-- That subquery is UNCORRELATED (it references neither @BranchId nor the outer query), so hoisting the BID
-- filter out of the view cannot change its value.
--
-- CRITICAL: a T-SQL "RETURNS @TMB TABLE (Costof DEC(13,3), SalePrice DECIMAL(12,3), ...)" declaration is an
-- implicit CAST on EVERY returned column — the INSERT INTO @TMB coerces each value to the declared type. A
-- plain view returns the raw expression precision instead, which is a SILENT difference: SQL Server hands the
-- app Costof=6.976 / SalePrice=5.800, an uncast view hands it 6.9762250 / 5.800000. So every column below is
-- CAST to the exact type from the RETURNS TABLE declaration (verified equal to SQL Server after this change).
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

CREATE OR REPLACE VIEW `GET_TABLE_FOR_CostofByBranch_v` AS
SELECT
        b.CuName,
        CAST(
        CASE
                WHEN c.CurrencyPower = 0
                    THEN
                    CASE
                            WHEN (IFNULL(SUM(CRedetDL),0.000) - IFNULL(SUM(ACC_DEPET_DL),0.000)) = 0
                                THEN 0.00
                            ELSE
                            (IFNULL(SUM(a.CRedetTO),0.000) - IFNULL(SUM(a.ACC_DEPET_TO),0.000)) / (IFNULL(SUM(CRedetDL),0.000) - IFNULL(SUM(ACC_DEPET_DL),0.000))
                    END
                ELSE
                CASE
                        WHEN (IFNULL(SUM(a.CRedetTO),0.000) - IFNULL(SUM(a.ACC_DEPET_TO),0.000)) = 0
                            THEN 0.00
                        ELSE
                        (IFNULL(SUM(CRedetDL),0.000) - IFNULL(SUM(ACC_DEPET_DL),0.000)) / (IFNULL(SUM(a.CRedetTO),0.000) - IFNULL(SUM(a.ACC_DEPET_TO),0.000))
                END
        END AS DECIMAL(13,3))                                               AS Costof,
        CAST((IFNULL(SUM(a.CRedetTO),0.000) - IFNULL(SUM(a.ACC_DEPET_TO),0.000)) AS DECIMAL(13,3)) AS Foreignbalance,
        CAST((IFNULL(SUM(CRedetDL),0.000) - IFNULL(SUM(ACC_DEPET_DL),0.000)) AS DECIMAL(13,3))     AS LocalBalance,
        CAST(IFNULL((SELECT
                        SUM(x.Credit) - SUM(x.Debit)
                FROM
                        ExSyAccountsCurrency_AccSafeActivityTb AS x
                    LEFT JOIN TB_Users AS y ON x.AccIDFrom = y.AccID
                WHERE
                        x.OperationTypeID <> 34
                        AND (x.TypeID = 22 OR x.TypeID = 21)
                        AND x.AccIDTo <> y.AccID), 0.000) AS DECIMAL(13,3))  AS costmerForPreseEn,
        CAST(a.BID AS SIGNED)                                               AS BID,
        CAST(a.ACCFRom AS SIGNED)                                           AS ACCFRom,
        CAST(c.CurrencyPower AS SIGNED)                                     AS CurrencyPower,
        CAST(c.SalePrice AS DECIMAL(12,3))                                  AS SalePrice,
        CAST(c.BuyPrice AS DECIMAL(12,3))                                   AS BuyPrice
FROM
        ExSyAccountsCurrency_CurrencymovementPruse AS a
    INNER JOIN
        CurrencyMainTb AS b
            ON a.ACCFRom = b.ID
    INNER JOIN
        NewCurrencyPriceOwnDetailsTb AS c
            ON b.ID = c.CurrencyIDTo
    INNER JOIN
        NewCurrencyPricesOwnTb AS D
            ON c.CPID = d.ID
WHERE
        a.ISactive = 1
        AND d.PriceType = 0
GROUP BY
        b.CuName,
        c.CurrencyPower,
        a.BID,
        a.ACCFRom,
        c.CurrencyPower,
        c.SalePrice,
        c.BuyPrice;

-- ---------------------------------------------------------------------------
-- Caller. "FROM dbo.GET_TABLE_FOR_CostofByBranch(@BranchId) AS a" -> the view filtered by BID in a derived
-- table, so the alias `a` and the SELECT list stay exactly as the converter emitted them.
-- FORMAT(x,'N3','en-us') -> FORMAT(x,3): .NET "N3" is 3 decimals WITH the thousands separator, which is what
-- MySQL FORMAT(x,3) produces in the default en_US locale.
DROP PROCEDURE IF EXISTS `GET_TABLE_FOR_Costof_Proc`;
DELIMITER $$
CREATE PROCEDURE `GET_TABLE_FOR_Costof_Proc`(IN `p_BranchId` INT)
BEGIN
    SELECT
            a.CuName,
            a.Costof,
            FORMAT(a.Foreignbalance, 3)    AS Foreignbalance,
            FORMAT(a.LocalBalance, 3)      AS LocalBalance,
            FORMAT(a.costmerForPreseEn, 3) AS costmerForPreseEn,
            a.SalePrice,
            a.BuyPrice
    FROM
            (SELECT * FROM `GET_TABLE_FOR_CostofByBranch_v` WHERE BID = p_BranchId) AS a;
END$$
DELIMITER ;

-- =====================================================================================
-- Same treatment for NEW_GET_TABLE_FOR_CostofByBranch(@BranchId) + its caller New_GET_TABLE_FOR_Costof_PROC.
-- Identical shape and identical RETURNS TABLE types; differs only in the FROM (no NewCurrencyPricesOwnTb join)
-- and the WHERE (a.IsActive = 1 AND a.TYPEFROM = 0 instead of a.ISactive = 1 AND d.PriceType = 0). BID is again
-- a GROUP BY key that is returned, so hoisting "AND a.BID = @BranchId" into the caller is exact.
-- =====================================================================================
CREATE OR REPLACE VIEW `NEW_GET_TABLE_FOR_CostofByBranch_v` AS
SELECT
        b.CuName,
        CAST(
        CASE
                WHEN c.CurrencyPower = 0
                    THEN
                    CASE
                            WHEN (IFNULL(SUM(CRedetDL), 0.000) - IFNULL(SUM(ACC_DEPET_DL), 0.000)) = 0
                                THEN 0.00
                            ELSE
                            (IFNULL(SUM(a.CRedetTO), 0.000) - IFNULL(SUM(a.ACC_DEPET_TO), 0.000)) / (IFNULL(SUM(CRedetDL), 0.000) - IFNULL(SUM(ACC_DEPET_DL), 0.000))
                    END
                ELSE
                CASE
                        WHEN (IFNULL(SUM(a.CRedetTO), 0.000) - IFNULL(SUM(a.ACC_DEPET_TO), 0.000)) = 0
                            THEN 0.00
                        ELSE
                        (IFNULL(SUM(CRedetDL), 0.000) - IFNULL(SUM(ACC_DEPET_DL), 0.000)) / (IFNULL(SUM(a.CRedetTO), 0.000) - IFNULL(SUM(a.ACC_DEPET_TO), 0.000))
                END
        END AS DECIMAL(13,3))                                                     AS Costof,
        CAST((IFNULL(SUM(a.CRedetTO), 0.000) - IFNULL(SUM(a.ACC_DEPET_TO), 0.000)) AS DECIMAL(13,3)) AS Foreignbalance,
        CAST((IFNULL(SUM(CRedetDL), 0.000) - IFNULL(SUM(ACC_DEPET_DL), 0.000)) AS DECIMAL(13,3))     AS LocalBalance,
        CAST(IFNULL((SELECT
                        SUM(x.Credit) - SUM(x.Debit)
                FROM
                        ExSyAccountsCurrency_AccSafeActivityTb AS x
                    LEFT JOIN
                      TB_Users AS y
                          ON x.AccIDFrom = y.AccID
                WHERE
                        x.OperationTypeID <> 34
                        AND (   x.TypeID = 22
                                OR x.TypeID = 21)
                        AND x.AccIDTo <> y.AccID), 0.000) AS DECIMAL(13,3))       AS costmerForPreseEn,
        CAST(a.BID AS SIGNED)                                                     AS BID,
        CAST(a.ACCFRom AS SIGNED)                                                 AS ACCFRom,
        CAST(c.CurrencyPower AS SIGNED)                                           AS CurrencyPower,
        CAST(c.SalePrice AS DECIMAL(12,3))                                        AS SalePrice,
        CAST(c.BuyPrice AS DECIMAL(12,3))                                         AS BuyPrice
FROM
        ExSyAccountsCurrency_CurrencymovementPruse AS a
    INNER JOIN
      CurrencyMainTb AS b
        ON a.ACCFRom = b.ID
    INNER JOIN
      NewCurrencyPriceOwnDetailsTb AS c
        ON b.ID = c.CurrencyIDTo
WHERE
        a.IsActive = 1
        AND a.TYPEFROM = 0
GROUP BY
        b.CuName,
        c.CurrencyPower,
        a.BID,
        a.ACCFRom,
        c.CurrencyPower,
        c.SalePrice,
        c.BuyPrice;

DROP PROCEDURE IF EXISTS `New_GET_TABLE_FOR_Costof_Proc`;
DELIMITER $$
CREATE PROCEDURE `New_GET_TABLE_FOR_Costof_Proc`(IN `p_BranchId` INT)
BEGIN
    SELECT
            a.CuName,
            a.Costof,
            FORMAT(a.Foreignbalance, 3)    AS Foreignbalance,
            FORMAT(a.LocalBalance, 3)      AS LocalBalance,
            FORMAT(a.costmerForPreseEn, 3) AS costmerForPreseEn,
            a.SalePrice,
            a.BuyPrice
    FROM
            (SELECT * FROM `NEW_GET_TABLE_FOR_CostofByBranch_v` WHERE BID = p_BranchId) AS a;
END$$
DELIMITER ;
