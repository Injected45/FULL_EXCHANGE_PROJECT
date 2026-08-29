-- =====================================================================================
-- Hand-port: ZRPT_CurrencyPriceDetailsTb_Grid
--
-- Body is the CONVERTER'S OWN OUTPUT; only two expressions are rewritten by hand, plus one srcpatch
-- (migration/srcpatch/ZRPT_CurrencyPriceDetailsTb_Grid.sql) that adds the BEGIN/END the author omitted
-- around a braceless "IF @typeMSGN = 1 SELECT @typeMSGNNAME = 'نقدي'".
--
-- The two rewritten expressions are T-SQL's string-aggregation idiom, which has no MySQL counterpart:
--     STUFF((SELECT ', ', <row text> FROM .. FOR XML PATH('')), 1, 2, '')
-- becomes
--     (SELECT GROUP_CONCAT(<row text> ORDER BY CuName SEPARATOR ', ') FROM (<same query>) AS gc)
-- exactly as in handport_pricelist_grids.sql, which does this for the four sibling price-list procs.
--
-- Two details that are easy to get wrong:
--   * ROW_NUMBER() must move INTO a derived table. MariaDB 10.4 rejects a window function inside
--     GROUP_CONCAT (error 4074), so `rn` is computed in the subquery and only referenced in the CONCAT.
--   * group_concat_max_len is raised to 1,000,000. Its default is 1024 bytes, and GROUP_CONCAT
--     TRUNCATES SILENTLY at that limit — a long Arabic price list would simply have been cut off,
--     whereas FOR XML PATH had no such limit. This is the one line here that prevents silent data loss.
--
-- The SEPARATOR ', ' reproduces what STUFF(..,1,2,'') achieved: the T-SQL query emits a leading ', '
-- before every row and then strips the first two characters. Same text, same order (by CuName, matching
-- the rn numbering), same NULL-when-no-rows behaviour.
--
-- Everything else — both currency branches, all Arabic labels and column aliases, the result-set shape —
-- is the converter's output unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ZRPT_CurrencyPriceDetailsTb_Grid`;
DELIMITER $$
CREATE PROCEDURE `ZRPT_CurrencyPriceDetailsTb_Grid`(IN `p_CurrencyIDFrom` INT, INOUT `p_MSG` LONGTEXT, IN `p_typeMSGN` INT, IN `p_TypeID` INT)
BEGIN
DECLARE v_typeMSGNNAME LONGTEXT;
DECLARE v_CurrencyIDFromNme LONGTEXT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_CurrencyPower BIT;

SET SESSION group_concat_max_len = 1000000;
	IF p_TypeID = 1
	 THEN
	IF p_typeMSGN = 1
		 THEN
SET v_typeMSGNNAME = 'نقدي';
		END IF;
	BEGIN
		IF p_CurrencyIDFrom = 1
			 THEN
				SELECT
						e.ID			AS CID,
						e.CuName		AS CNAME,
						v_typeMSGNNAME   AS typeMSGNNAME,
						b.ID			AS IDCruns,
						c.CuName		AS CurrencyIDTo,
						b.SalePrice,
						b.BuyPrice,
						CASE 								WHEN b.CurrencyPower = 0 									THEN 									'اقوى' 								ELSE 								'أقل' 						END				AS CurrencyPower,
						b.BankSalePrice AS BankSalePrice,
						b.BankBuyPrice  AS BankBuyPrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.SalePrice 								ELSE 								SalePrice 						END				AS CurrencySalePrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.BuyPrice 								ELSE 								BuyPrice 						END				AS CurrencyBuyPrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.BankSalePrice 								ELSE 								BankSalePrice 						END				AS CurrencyBankSalePrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.BankBuyPrice 								ELSE 								BankBuyPrice 						END				AS CurrencyBankBuyPrice,
						1				AS Typesd
				FROM
						`CurrencyPricesTb` AS a
					INNER JOIN
						CurrencyPriceDetailsTb AS b
							ON a.CurrencyIDFrom = b.CurrencyIDFrom
					INNER JOIN
						`CurrencyMainTb` AS c
							ON b.CurrencyIDTo = c.ID
					INNER JOIN
						`CurrencyMainTb` AS e
							ON p_CurrencyIDFrom = e.ID
				WHERE
						a.Isactive = 1
						AND b.Isactive = 1
						AND c.Isactive = 1
						AND a.CurrencyIDFrom = p_CurrencyIDFrom;
				SELECT a.CuName INTO v_CurrencyIDFromNme FROM
						CurrencyMainTb AS a
				WHERE
						a.ID = p_CurrencyIDFrom;
				SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `CurrencyPricesTb` AS a INNER JOIN CurrencyPriceDetailsTb AS b ON a.CurrencyIDFrom = b.CurrencyIDFrom INNER JOIN `CurrencyMainTb` AS c ON p_CurrencyIDFrom = c.ID WHERE a.Isactive = 1 AND b.Isactive = 1 AND c.Isactive = 1 AND a.CurrencyIDFrom = 1) AS gc));
				SELECT
						p_MSG;
			END IF;
		IF p_CurrencyIDFrom <> 1
			 THEN

				SELECT a.CurrencyPower INTO v_CurrencyPower FROM
						CurrencyPriceDetailsTb AS a
				WHERE
						a.CurrencyIDFrom = 1
						AND a.CurrencyIDTo = p_CurrencyIDFrom;
				IF v_CurrencyPower = 1
					 THEN
						SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyIDFrom;

						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								v_typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								b.BuyPrice	  AS BuyPrice,
								b.SalePrice	  AS SalePrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقل' 										ELSE 										'اقوى' 								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											b.SalePrice 										ELSE 										1 / SalePrice 								END			  AS CurrencySalePrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											b.BuyPrice 										ELSE 										1 / BuyPrice 								END			  AS CurrencyBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								`CurrencyPricesTb` AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = p_CurrencyIDFrom

						UNION
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								v_typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE 										WHEN CurrencyPower = 0 											THEN 											b.SalePrice * v_CurrencyBuyPrice 										ELSE 										v_CurrencyBuyPrice / SalePrice 								END			  AS SalePrice,
								CASE 										WHEN CurrencyPower = 0 											THEN 											b.BuyPrice * v_CurrencySalePrice 										ELSE 										v_CurrencySalePrice / BuyPrice 								END			  AS BuyPrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقوى' 										ELSE 										'أقل' 								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE 										WHEN CurrencyPower = 0 											THEN 											v_CurrencyBuyPrice * b.SalePrice 										ELSE 										v_CurrencyBuyPrice / SalePrice 								END			  AS CurrencyBuyPrice,

								CASE 										WHEN CurrencyPower = 0 											THEN 											v_CurrencySalePrice * b.BuyPrice 										ELSE 										v_CurrencySalePrice / BuyPrice 								END			  AS CurrencySalePrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								`CurrencyPricesTb` AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> p_CurrencyIDFrom;
						SET p_MSG = '';
					END IF;
				IF v_CurrencyPower = 0
					 THEN
						SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyIDFrom;

						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								v_typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE 										WHEN CurrencyPower = 1 											THEN 											b.SalePrice 										ELSE 										1 / SalePrice 								END			  AS SalePrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											b.BuyPrice 										ELSE 										1 / BuyPrice 								END			  AS BuyPrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقل' 										ELSE 										'اقوى' 								END			  AS CurrencyPower,
								b.BuyPrice	  AS CurrencyBuyPrice,
								b.SalePrice	  AS CurrencySalePrice,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								`CurrencyPricesTb` AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = p_CurrencyIDFrom

						UNION
						SELECT
								e.ID								AS CID,
								e.CuName							AS CNAME,
								v_typeMSGNNAME						AS typeMSGNNAME,
								b.ID								AS IDCruns,
								c.CuName							AS CurrencyIDTo,
								1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
								1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقوى' 										ELSE 										'أقل' 								END									AS CurrencyPower,

								CASE 										WHEN CurrencyPower = 1 											THEN 											v_CurrencyBuyPrice * b.SalePrice 										ELSE 										v_CurrencyBuyPrice / SalePrice 								END									AS CurrencyBuyPrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											v_CurrencySalePrice * b.BuyPrice 										ELSE 										v_CurrencySalePrice / BuyPrice 								END									AS CurrencySalePrice,
								0.00								AS BankSalePrice,
								0.00								AS BankBuyPrice,
								0,
								00									AS CurrencyBankBuyPrice,
								0.00								AS CurrencyBankSalePrice,
								1									AS Typesd
						FROM
								`CurrencyPricesTb` AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> p_CurrencyIDFrom;
						SET p_MSG = '';
					END IF;
			END IF;
	END;
	IF p_typeMSGN = 2
		 THEN
SET v_typeMSGNNAME = 'عالمصرف';
		END IF;
	BEGIN
		SELECT
				e.ID		  AS CID,
				e.CuName	  AS CNAME,
				v_typeMSGNNAME AS typeMSGNNAME,
				b.ID		  AS IDCruns,
				c.CuName	  AS CurrencyIDTo,
				b.SalePrice,
				b.BuyPrice,
				CASE 						WHEN b.CurrencyPower = 0 							THEN 							'اقوى' 						ELSE 						'أقل' 				END			  AS CurrencyPower,
				0.00		  AS BankSalePrice,
				0.00		  AS BankBuyPrice,

				CASE 						WHEN CurrencyPower = 1 							THEN 							1 / b.SalePrice 						ELSE 						b.SalePrice 				END			  AS CurrencySalePrice,

				CASE 						WHEN CurrencyPower = 1 							THEN 							1 / b.BuyPrice 						ELSE 						b.BuyPrice 				END			  AS CurrencyBuyPrice,
				0.00		  AS CurrencyBankSalePrice,

				0.00		  AS CurrencyBankBuyPrice,
				2			  AS Typesd
		FROM
				`Currency_settingForBancksRet` AS a
			INNER JOIN
				CurrencyPriceDetailsBancksTb AS b
					ON a.CurrencyFrom = b.CurrencyIDFrom
			INNER JOIN
				`CurrencyMainTb` AS c
					ON b.CurrencyIDTo = c.ID
			INNER JOIN
				`CurrencyMainTb` AS e
					ON p_CurrencyIDFrom = e.ID
		WHERE
				a.Isactive = 1
				AND b.Isactive = 1
				AND c.Isactive = 1
				AND b.Banck_ID = p_CurrencyIDFrom;
		SET p_MSG = 'لايوجد اسعار متوفرة في الوقت الحالي ';
	END;
	END IF;
	IF p_TypeID = 2
	 THEN
	IF p_typeMSGN = 1
		 THEN
SET v_typeMSGNNAME = 'نقدي';
		END IF;
	BEGIN
		IF p_CurrencyIDFrom = 1
			 THEN
				SELECT
						e.ID			AS CID,
						e.CuName		AS CNAME,
						v_typeMSGNNAME   AS typeMSGNNAME,
						b.ID			AS IDCruns,
						c.CuName		AS CurrencyIDTo,
						b.SalePrice,
						b.BuyPrice,
						CASE 								WHEN b.CurrencyPower = 0 									THEN 									'اقوى' 								ELSE 								'أقل' 						END				AS CurrencyPower,
						b.BankSalePrice AS BankSalePrice,
						b.BankBuyPrice  AS BankBuyPrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.SalePrice 								ELSE 								SalePrice 						END				AS CurrencySalePrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.BuyPrice 								ELSE 								BuyPrice 						END				AS CurrencyBuyPrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.BankSalePrice 								ELSE 								BankSalePrice 						END				AS CurrencyBankSalePrice,

						CASE 								WHEN CurrencyPower = 1 									THEN 									1 / b.BankBuyPrice 								ELSE 								BankBuyPrice 						END				AS CurrencyBankBuyPrice,
						1				AS Typesd
				FROM
						`CurrencyPricesOwnTb` AS a
					INNER JOIN
						CurrencyPriceOwnDetailsTb AS b
							ON a.CurrencyIDFrom = b.CurrencyIDFrom
					INNER JOIN
						`CurrencyMainTb` AS c
							ON b.CurrencyIDTo = c.ID
					INNER JOIN
						`CurrencyMainTb` AS e
							ON p_CurrencyIDFrom = e.ID
				WHERE
						a.Isactive = 1
						AND b.Isactive = 1
						AND c.Isactive = 1
						AND a.CurrencyIDFrom = p_CurrencyIDFrom;
				SELECT a.CuName INTO v_CurrencyIDFromNme FROM
						CurrencyMainTb AS a
				WHERE
						a.ID = p_CurrencyIDFrom;
				SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `CurrencyPricesOwnTb` AS a INNER JOIN CurrencyPriceOwnDetailsTb AS b ON a.CurrencyIDFrom = b.CurrencyIDFrom INNER JOIN `CurrencyMainTb` AS c ON p_CurrencyIDFrom = c.ID WHERE a.Isactive = 1 AND b.Isactive = 1 AND c.Isactive = 1 AND a.CurrencyIDFrom = 1) AS gc));
				SELECT
						p_MSG;
			END IF;
		IF p_CurrencyIDFrom <> 1
			 THEN
				SELECT a.CurrencyPower INTO v_CurrencyPower FROM
						CurrencyPriceOwnDetailsTb AS a
				WHERE
						a.CurrencyIDFrom = 1
						AND a.CurrencyIDTo = p_CurrencyIDFrom;
				IF v_CurrencyPower = 1
					 THEN
						SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
								CurrencyPriceOwnDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyIDFrom;

						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								v_typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								b.BuyPrice	  AS BuyPrice,
								b.SalePrice	  AS SalePrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقل' 										ELSE 										'اقوى' 								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											b.SalePrice 										ELSE 										1 / SalePrice 								END			  AS CurrencySalePrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											b.BuyPrice 										ELSE 										1 / BuyPrice 								END			  AS CurrencyBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								`CurrencyPricesOwnTb` AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = p_CurrencyIDFrom

						UNION
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								v_typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE 										WHEN CurrencyPower = 0 											THEN 											b.SalePrice * v_CurrencyBuyPrice 										ELSE 										v_CurrencyBuyPrice / SalePrice 								END			  AS SalePrice,
								CASE 										WHEN CurrencyPower = 0 											THEN 											b.BuyPrice * v_CurrencySalePrice 										ELSE 										v_CurrencySalePrice / BuyPrice 								END			  AS BuyPrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقوى' 										ELSE 										'أقل' 								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE 										WHEN CurrencyPower = 0 											THEN 											v_CurrencyBuyPrice * b.SalePrice 										ELSE 										v_CurrencyBuyPrice / SalePrice 								END			  AS CurrencyBuyPrice,

								CASE 										WHEN CurrencyPower = 0 											THEN 											v_CurrencySalePrice * b.BuyPrice 										ELSE 										v_CurrencySalePrice / BuyPrice 								END			  AS CurrencySalePrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								`CurrencyPricesOwnTb` AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> p_CurrencyIDFrom;
						SET p_MSG = '';
					END IF;
				IF v_CurrencyPower = 0
					 THEN
						SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = p_CurrencyIDFrom;

						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								v_typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE 										WHEN CurrencyPower = 1 											THEN 											b.SalePrice 										ELSE 										1 / SalePrice 								END			  AS SalePrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											b.BuyPrice 										ELSE 										1 / BuyPrice 								END			  AS BuyPrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقل' 										ELSE 										'اقوى' 								END			  AS CurrencyPower,
								b.BuyPrice	  AS CurrencyBuyPrice,
								b.SalePrice	  AS CurrencySalePrice,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								`CurrencyPricesOwnTb` AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = p_CurrencyIDFrom

						UNION
						SELECT
								e.ID								AS CID,
								e.CuName							AS CNAME,
								v_typeMSGNNAME						AS typeMSGNNAME,
								b.ID								AS IDCruns,
								c.CuName							AS CurrencyIDTo,
								1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
								1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
								CASE 										WHEN b.CurrencyPower = 0 											THEN 											'اقوى' 										ELSE 										'أقل' 								END									AS CurrencyPower,

								CASE 										WHEN CurrencyPower = 1 											THEN 											v_CurrencyBuyPrice * b.SalePrice 										ELSE 										v_CurrencyBuyPrice / SalePrice 								END									AS CurrencyBuyPrice,

								CASE 										WHEN CurrencyPower = 1 											THEN 											v_CurrencySalePrice * b.BuyPrice 										ELSE 										v_CurrencySalePrice / BuyPrice 								END									AS CurrencySalePrice,
								0.00								AS BankSalePrice,
								0.00								AS BankBuyPrice,
								0,
								00									AS CurrencyBankBuyPrice,
								0.00								AS CurrencyBankSalePrice,
								1									AS Typesd
						FROM
								`CurrencyPricesOwnTb` AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								`CurrencyMainTb` AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								`CurrencyMainTb` AS e
									ON p_CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> p_CurrencyIDFrom;
						SET p_MSG = '';
					END IF;
			END IF;
	END;
	IF p_typeMSGN = 2
		 THEN
SET v_typeMSGNNAME = 'عالمصرف';
		END IF;
	BEGIN
		SELECT
				e.ID		  AS CID,
				e.CuName	  AS CNAME,
				v_typeMSGNNAME AS typeMSGNNAME,
				b.ID		  AS IDCruns,
				c.CuName	  AS CurrencyIDTo,
				b.SalePrice,
				b.BuyPrice,
				CASE 						WHEN b.CurrencyPower = 0 							THEN 							'اقوى' 						ELSE 						'أقل' 				END			  AS CurrencyPower,
				0.00		  AS BankSalePrice,
				0.00		  AS BankBuyPrice,

				CASE 						WHEN CurrencyPower = 1 							THEN 							1 / b.SalePrice 						ELSE 						b.SalePrice 				END			  AS CurrencySalePrice,

				CASE 						WHEN CurrencyPower = 1 							THEN 							1 / b.BuyPrice 						ELSE 						b.BuyPrice 				END			  AS CurrencyBuyPrice,
				0.00		  AS CurrencyBankSalePrice,

				0.00		  AS CurrencyBankBuyPrice,
				2			  AS Typesd
		FROM
				`Currency_settingForBancksRet` AS a
			INNER JOIN
				CurrencyPriceDetailsBancksTb AS b
					ON a.CurrencyFrom = b.CurrencyIDFrom
			INNER JOIN
				`CurrencyMainTb` AS c
					ON b.CurrencyIDTo = c.ID
			INNER JOIN
				`CurrencyMainTb` AS e
					ON p_CurrencyIDFrom = e.ID
		WHERE
				a.Isactive = 1
				AND b.Isactive = 1
				AND c.Isactive = 1
				AND b.Banck_ID = p_CurrencyIDFrom;
		SET p_MSG = 'لايوجد اسعار متوفرة في الوقت الحالي ';
	END;
	END IF;
END

$$
DELIMITER ;
