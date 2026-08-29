-- =====================================================================================
-- Table-valued functions -> MySQL VIEWS
--
-- MySQL/MariaDB has NO table-valued functions: a function may only return a SCALAR. So a T-SQL TVF cannot
-- be a function here. The mapping depends on whether it takes parameters and how it is called:
--
--   * PARAMETERLESS TVF, only ever used as  FROM dbo.fn() AS a WHERE ...   ->   a VIEW.
--     The caller `FROM dbo.fn() AS a` becomes `FROM fn AS a` (the converter strips dbo. and the () — see the
--     TvfViews handling in Program.cs). Selecting-from a view with an outer WHERE is identical semantics.
--
-- This file covers the two PARAMETERLESS ones, which together unblock 20 referencing routines:
--   GET_TABLE_FOR_Costof       (17 refs)
--   NEW_GET_TABLE_FOR_Costof   (3 refs)
--
-- Both are multi-statement TVFs whose whole body is a single "INSERT INTO @t <SELECT> ; RETURN", so the view
-- is exactly that SELECT. The 9 columns of the SELECT line up 1:1, in order, with the RETURNS @t TABLE(...)
-- declaration, so the result-set shape is preserved exactly.
--
-- Mechanical translations applied: dbo. dropped; ISNULL -> IFNULL; the synonym
-- ExSyAccountsCurrency_CurrencymovementPruse is already a MySQL view (handport_synonyms.sql). Nothing else
-- changed — same joins, same WHERE, same GROUP BY, same column order.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';

CREATE OR REPLACE VIEW `GET_TABLE_FOR_Costof` AS
SELECT
        b.CuName,
        CASE
                WHEN c.CurrencyPower = 0
                    THEN
                    CASE
                            WHEN IFNULL(SUM(CRedetDL) - SUM(ACC_DEPET_DL),0.000) = 0
                                THEN 0.00
                            ELSE
                            IFNULL(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO),0.000) / IFNULL(SUM(CRedetDL) - SUM(ACC_DEPET_DL),0.000)
                    END
                ELSE
                CASE
                        WHEN IFNULL(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO),0.000) = 0
                            THEN 0.00
                        ELSE
                        IFNULL(SUM(CRedetDL) - SUM(ACC_DEPET_DL),0.000) / IFNULL(SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO),0.000)
                END
        END                                     AS Costof,
        (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) AS Foreignbalance,
        (SUM(CRedetDL) - SUM(ACC_DEPET_DL))     AS LocalBalance,
        a.BID,
        a.ACCFRom,
        c.CurrencyPower,
        c.SalePrice,
        c.BuyPrice
FROM
        ExSyAccountsCurrency_CurrencymovementPruse AS a
    LEFT JOIN
        CurrencyMainTb AS b
            ON a.ACCFRom = b.ID
    LEFT JOIN
        CurrencyPriceDetailsTb AS c
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

CREATE OR REPLACE VIEW `NEW_GET_TABLE_FOR_Costof` AS
SELECT
        b.CuName,
        CASE
                WHEN c.CurrencyPower = 0
                  THEN
                  CASE
                          WHEN (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) = 0
                            THEN 0.00
                          ELSE
                          (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) / (SUM(CRedetDL) - SUM(ACC_DEPET_DL))
                  END
                ELSE
                CASE
                        WHEN SUM(CRedetDL) - SUM(ACC_DEPET_DL) = 0
                          THEN 0.00
                        ELSE
                        (SUM(CRedetDL) - SUM(ACC_DEPET_DL)) / (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO))
                END
        END                                     AS Costof,
        (SUM(a.CRedetTO) - SUM(a.ACC_DEPET_TO)) AS Foreignbalance,
        (SUM(CRedetDL) - SUM(ACC_DEPET_DL))     AS LocalBalance,
        a.BID,
        a.ACCFRom,
        c.CurrencyPower,
        c.SalePrice,
        c.BuyPrice,
        A.TYPEFROM,
        a.CountryID,
        c.CurrencyIDTo,
        d.AccountType
FROM
        ExSyAccountsCurrency_CurrencymovementPruse AS a
    INNER JOIN NewCurrencyPricesOwnTb AS d ON a.TYPEFROM = d.PriceType
    INNER JOIN CurrencyMainTb AS b ON a.ACCFRom = b.ID
    INNER JOIN NewCurrencyPriceOwnDetailsTb AS c ON d.ID = c.CPID
WHERE
        a.IsActive = 1
GROUP BY
        b.CuName, c.CurrencyPower, a.BID, a.ACCFRom, c.CurrencyPower,
        c.SalePrice, c.BuyPrice, A.TYPEFROM, a.CountryID, c.CurrencyIDTo, d.AccountType;
