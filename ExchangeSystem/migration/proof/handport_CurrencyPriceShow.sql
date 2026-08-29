-- =====================================================================================
-- Hand-port: CurrencyPriceShow
--
-- Converter output was correct EXCEPT the ORDER BY. Both branches are a UNION whose first
-- SELECT aliases  b.CuName AS NAMECURNSE , and then order by the ORIGINAL column name:
--     ORDER BY CuName
-- SQL Server tolerates this. MySQL does not: after a UNION the ORDER BY may only reference the
-- result-set column names (which come from the FIRST select), so it fails with
--     Unknown column 'CuName' in 'order clause'
-- Faithful fix: order by the result column that CuName was aliased to -> NAMECURNSE.
-- Same rows, same order, same column names/positions. Nothing else changed.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS CurrencyPriceShow;
DELIMITER $$
CREATE PROCEDURE `CurrencyPriceShow`(IN `p_TypeID` TINYINT UNSIGNED)
BEGIN
IF p_TypeID = 1
	 THEN
		SELECT
			b.CuName AS NAMECURNSE
		   ,b.CurCode AS CODE
		   ,a.BuyPrice AS BUprese
		   ,a.SalePrice AS selecsPress
		   ,'' AS insertDAte
		   ,'' AS UESerINSER
		   ,'نقدي' AS `Type`
		   ,'البيع نقدي' AS BAnckNAME
		FROM `CurrencyPriceDetailsTb` AS a
		INNER JOIN CurrencyMainTb AS b
			ON a.CurrencyIDTo = b.ID
		UNION
		SELECT
			b.CuName
		   ,b.CurCode
		   ,a.BuyPrice
		   ,a.SalePrice
		   ,''
		   ,''
		   ,'البيع عالمصرف'
		   ,C.BankName
		FROM `CurrencyPriceDetailsBancksTb` AS a
		INNER JOIN CurrencyMainTb AS b
			ON a.CurrencyIDTo = b.ID
		INNER JOIN BanksTb AS C
			ON a.Banck_ID = C.ID

		ORDER BY NAMECURNSE;
	END IF;
	IF p_TypeID = 2
	 THEN
		SELECT
			b.CuName AS NAMECURNSE
		   ,b.CurCode AS CODE
		   ,a.BuyPrice AS BUprese
		   ,a.SalePrice AS selecsPress
		   ,'' AS insertDAte
		   ,'' AS UESerINSER
		   ,'نقدي' AS `Type`
		   ,'البيع نقدي' AS BAnckNAME
		FROM `CurrencyPriceOwenDetailsTb` AS a
		INNER JOIN CurrencyMainTb AS b
			ON a.CurrencyIDTo = b.ID
		UNION
		SELECT
			b.CuName
		   ,b.CurCode
		   ,a.BuyPrice
		   ,a.SalePrice
		   ,''
		   ,''
		   ,'البيع عالمصرف'
		   ,C.BankName
		FROM `CurrencyPriceDetailsBancksTb` AS a
		INNER JOIN CurrencyMainTb AS b
			ON a.CurrencyIDTo = b.ID
		INNER JOIN BanksTb AS C
			ON a.Banck_ID = C.ID

		ORDER BY NAMECURNSE;
	END IF;
END
$$
DELIMITER ;
