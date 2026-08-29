-- =====================================================================================
-- Hand-port: the 4 price-list Grid procs (only ones using STUFF/FOR XML string-aggregation).
-- Body is the converter's own output; the SINGLE STUFF((SELECT ', ', <row> .. FOR XML PATH('')),1,2,'')
-- expression is replaced with GROUP_CONCAT(<row> ORDER BY CuName SEPARATOR ', ') over a DERIVED TABLE
-- that computes ROW_NUMBER() (MariaDB 10.4 rejects a window function inside GROUP_CONCAT, err 4074).
-- group_concat_max_len is raised so a long price list is not silently truncated (FOR XML had no limit).
-- Faithful: same rows, same order (by CuName), same separator, same NULL-when-empty behaviour as T-SQL '+'.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';


DROP PROCEDURE IF EXISTS `CurrencyPriceDetailsTb_Grid`;
DELIMITER $$
CREATE PROCEDURE `CurrencyPriceDetailsTb_Grid`(IN `p_CurrencyIDFrom` INT, INOUT `p_MSG` LONGTEXT, IN `p_typeMSGN` INT)
BEGIN
DECLARE v_IsDefault int;
DECLARE v_CurrencyIDFromNme LONGTEXT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_CurrencyPower BIT;

SET SESSION group_concat_max_len = 1000000;
	SELECT a.IsDefault INTO v_IsDefault FROM CurrencyMainTb as a where a.id=p_CurrencyIDFrom;


	START TRANSACTION;




	IF p_typeMSGN = 1
		 THEN

			IF v_IsDefault = 1
				 THEN
					SELECT
							b.ID			AS IDCruns,
							c.CuName		AS CurrencyIDTo,
							b.SalePrice,
							b.BuyPrice,

							CASE  									WHEN b.CurrencyPower = 0  										THEN  										'اقوى'  									ELSE  									'أقل'  							END				AS CurrencyPower,
							b.BankSalePrice AS BankSalePrice,
							b.BankBuyPrice  AS BankBuyPrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.SalePrice  									ELSE  									SalePrice  							END				AS CurrencySalePrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.BuyPrice  									ELSE  									BuyPrice  							END				AS CurrencyBuyPrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.BankSalePrice  									ELSE  									BankSalePrice  							END				AS CurrencyBankSalePrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.BankBuyPrice  									ELSE  									BankBuyPrice  							END				AS CurrencyBankBuyPrice,
							1				AS Typesd
					FROM
							`CurrencyPricesTb` AS a
						INNER JOIN
							CurrencyPriceDetailsTb AS b
								ON a.CurrencyIDFrom = b.CurrencyIDFrom
						INNER JOIN
							`CurrencyMainTb` AS c
								ON b.CurrencyIDTo = c.ID
					WHERE
							a.IsActive = 1
							AND b.IsActive = 1
							AND c.IsActive = 1
							AND a.CurrencyIDFrom = p_CurrencyIDFrom;

					SELECT a.CuName INTO v_CurrencyIDFromNme FROM
							CurrencyMainTb AS a
					WHERE
							a.ID = p_CurrencyIDFrom;

					SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `CurrencyPricesTb` AS a INNER JOIN CurrencyPriceDetailsTb AS b ON a.CurrencyIDFrom = b.CurrencyIDFrom INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1) AS gc));
					SELECT
							p_MSG;
				END IF;



			IF v_IsDefault <> 1
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
									b.ID		AS IDCruns,
									c.CuName	AS CurrencyIDTo,
									b.BuyPrice  AS BuyPrice,
									b.SalePrice AS SalePrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقل'  											ELSE  											'اقوى'  									END			AS CurrencyPower,
									0.00		AS BankSalePrice,
									0.00		AS BankBuyPrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.SalePrice  											ELSE  											1 / SalePrice  									END			AS CurrencySalePrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.BuyPrice  											ELSE  											1 / BuyPrice  									END			AS CurrencyBuyPrice,
									0,
									00			AS CurrencyBankBuyPrice,
									0.00		AS CurrencyBankSalePrice,
									1			AS Typesd
							FROM
									`CurrencyPricesTb` AS a
								INNER JOIN
									CurrencyPriceDetailsTb AS b
										ON a.CurrencyIDFrom = b.CurrencyIDFrom
								INNER JOIN
									`CurrencyMainTb` AS c
										ON b.CurrencyIDFrom = c.ID
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo = p_CurrencyIDFrom

							UNION

							SELECT
									b.ID	 AS IDCruns,
									c.CuName AS CurrencyIDTo,
									CASE  											WHEN CurrencyPower = 0  												THEN  												b.SalePrice * v_CurrencyBuyPrice  											ELSE  											v_CurrencyBuyPrice / SalePrice  									END		 AS SalePrice,
									CASE  											WHEN CurrencyPower = 0  												THEN  												b.BuyPrice * v_CurrencySalePrice  											ELSE  											v_CurrencySalePrice / BuyPrice  									END		 AS BuyPrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقوى'  											ELSE  											'أقل'  									END		 AS CurrencyPower,
									0.00	 AS BankSalePrice,
									0.00	 AS BankBuyPrice,
									CASE  											WHEN CurrencyPower = 0  												THEN  												v_CurrencyBuyPrice * b.SalePrice  											ELSE  											v_CurrencyBuyPrice / SalePrice  									END		 AS CurrencyBuyPrice,
									CASE  											WHEN CurrencyPower = 0  												THEN  												v_CurrencySalePrice * b.BuyPrice  											ELSE  											v_CurrencySalePrice / BuyPrice  									END		 AS CurrencySalePrice,
									0,
									00		 AS CurrencyBankBuyPrice,
									0.00	 AS CurrencyBankSalePrice,
									1		 AS Typesd
							FROM
									`CurrencyPricesTb` AS a
								INNER JOIN
									CurrencyPriceDetailsTb AS b
										ON a.CurrencyIDFrom = b.CurrencyIDFrom
								INNER JOIN
									`CurrencyMainTb` AS c
										ON b.CurrencyIDTo = c.ID
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
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
									b.ID		AS IDCruns,
									c.CuName	AS CurrencyIDTo,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.SalePrice  											ELSE  											1 / SalePrice  									END			AS SalePrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.BuyPrice  											ELSE  											1 / BuyPrice  									END			AS BuyPrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقل'  											ELSE  											'اقوى'  									END			AS CurrencyPower,
									b.BuyPrice  AS CurrencyBuyPrice,
									b.SalePrice AS CurrencySalePrice,
									0.00		AS BankSalePrice,
									0.00		AS BankBuyPrice,
									0,
									00			AS CurrencyBankBuyPrice,
									0.00		AS CurrencyBankSalePrice,
									1			AS Typesd
							FROM
									`CurrencyPricesTb` AS a


								INNER JOIN
									CurrencyPriceDetailsTb AS b
										ON a.CurrencyIDFrom = b.CurrencyIDFrom
								INNER JOIN
									`CurrencyMainTb` AS c
										ON b.CurrencyIDFrom = c.ID
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo = p_CurrencyIDFrom

							UNION

							SELECT
									b.ID								AS IDCruns,
									c.CuName							AS CurrencyIDTo,
									1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
									1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقوى'  											ELSE  											'أقل'  									END									AS CurrencyPower,
									CASE  											WHEN CurrencyPower = 1  												THEN  												v_CurrencyBuyPrice * b.SalePrice  											ELSE  											v_CurrencyBuyPrice / SalePrice  									END									AS CurrencyBuyPrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												v_CurrencySalePrice * b.BuyPrice  											ELSE  											v_CurrencySalePrice / BuyPrice  									END									AS CurrencySalePrice,
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
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo <> p_CurrencyIDFrom;

							SET p_MSG = '';
						END IF;
				END IF;
		END IF;
	IF p_typeMSGN = 2
		 THEN
			SELECT
					b.ID	 AS IDCruns,
					c.CuName AS CurrencyIDTo,
					b.SalePrice,
					b.BuyPrice,
					CASE  							WHEN b.CurrencyPower = 0  								THEN  								'اقوى'  							ELSE  							'أقل'  					END		 AS CurrencyPower,
					0.00	 AS BankSalePrice,
					0.00	 AS BankBuyPrice,
					CASE  							WHEN CurrencyPower = 1  								THEN  								1 / b.SalePrice  							ELSE  							b.SalePrice  					END		 AS CurrencySalePrice,
					CASE  							WHEN CurrencyPower = 1  								THEN  								1 / b.BuyPrice  							ELSE  							b.BuyPrice  					END		 AS CurrencyBuyPrice,
					0.00	 AS CurrencyBankSalePrice,
					0.00	 AS CurrencyBankBuyPrice,
					2		 AS Typesd
			FROM
					`Currency_settingForBancksRet` AS a
				INNER JOIN
					CurrencyPriceDetailsBancksTb AS b
						ON a.CurrencyFrom = b.CurrencyIDFrom
				INNER JOIN
					`CurrencyMainTb` AS c
						ON b.CurrencyIDTo = c.ID
			WHERE
					a.IsActive = 1
					AND b.IsActive = 1
					AND c.IsActive = 1
					AND b.Banck_ID = p_CurrencyIDFrom;

			SET p_MSG = 'لايوجد اسعار متوفرة في الوقت الحالي ';
		END IF;
	COMMIT;
END
$$
DELIMITER ;


DROP PROCEDURE IF EXISTS `NEWCurrencyPriceDetailsTb_Grid`;
DELIMITER $$
CREATE PROCEDURE `NEWCurrencyPriceDetailsTb_Grid`(IN `p_CurrencyIDFrom` INT, INOUT `p_MSG` LONGTEXT, IN `p_AccounType` INT, IN `p_BranchID` INT, IN `p_CountryID` INT, IN `p_BankID` INT, IN `p_PriceType` INT)
BEGIN
DECLARE v_IsDefault INT;
DECLARE v_AccParent BIGINT;
DECLARE v_CurrencyIDFromNme LONGTEXT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_CurrencyPower BIT;

SET SESSION group_concat_max_len = 1000000;
  SELECT a.IsDefault INTO v_IsDefault FROM
          CurrencyMainTb AS a
  WHERE
          a.ID = p_CurrencyIDFrom;


SELECT IFNULL(at.AccCode,0) INTO v_AccParent FROM AccountsTb at WHERE at.AccCode=p_BankID AND at.AccParent=10105;


  START TRANSACTION;




  IF p_PriceType = 0
     THEN

      IF v_IsDefault = 1
         THEN
          SELECT
                  b.ID        AS IDCruns,
                  c.CuName    AS CurrencyIDTo,
                  b.SalePrice,
                  b.BuyPrice,

                  CASE                            WHEN b.CurrencyPower = 0                              THEN                              'اقوى'                            ELSE                            'أقل'                    END         AS CurrencyPower,
                  b.SalePrice AS BankSalePrice,
                  b.BuyPrice  AS BankBuyPrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.SalePrice                            ELSE                            SalePrice                    END         AS CurrencySalePrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.BuyPrice                            ELSE                            BuyPrice                    END         AS CurrencyBuyPrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.SalePrice                            ELSE                            b.SalePrice                    END         AS CurrencyBankSalePrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.BuyPrice                            ELSE                            b.BuyPrice                    END         AS CurrencyBankBuyPrice,
                  1           AS Typesd
          FROM
                  `NewCurrencyPricesOwnTb` AS a
              INNER JOIN
                NewCurrencyPriceOwnDetailsTb AS b
                  ON a.ID = b.CPID
              INNER JOIN
                `CurrencyMainTb` AS c
                  ON b.CurrencyIDTo = c.ID
          WHERE
                  a.IsActive = 1
                  AND b.IsActive = 1
                  AND c.IsActive = 1
                  AND a.CurrencyIDFrom = p_CurrencyIDFrom
                  AND a.CountryID = p_CountryID
                  AND a.PriceType = p_PriceType
                  AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

          SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                  CurrencyMainTb AS a
          WHERE
                  a.ID = p_CurrencyIDFrom;

          SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.ID = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.PriceType = p_PriceType AND a.CountryID = p_CountryID AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)) AS gc));
          SELECT
                  p_MSG;
        END IF;



      IF v_IsDefault <> 1
         THEN

          SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                  NewCurrencyPriceOwnDetailsTb AS a
              INNER JOIN
                NewCurrencyPricesOwnTb AS b
                  ON a.CPID = b.ID
          WHERE
                  a.CurrencyIDFrom = 1
                  AND a.CurrencyIDTo = p_CurrencyIDFrom
                  AND b.PriceType = p_PriceType
                  AND b.CountryID = p_CountryID
                  AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);
          IF v_CurrencyPower = 1
             THEN
              SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.BuyPrice  AS BuyPrice,
                      b.SalePrice AS SalePrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقل'                                ELSE                                'اقوى'                        END         AS CurrencyPower,
                      0.00        AS BankSalePrice,
                      0.00        AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.SalePrice                                ELSE                                1 / SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.BuyPrice                                ELSE                                1 / BuyPrice                        END         AS CurrencyBuyPrice,
                      0,
                      00          AS CurrencyBankBuyPrice,
                      0.00        AS CurrencyBankSalePrice,
                      1           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDFrom = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo = p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)

              UNION

              SELECT
                      b.ID     AS IDCruns,
                      c.CuName AS CurrencyIDTo,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  b.SalePrice * v_CurrencyBuyPrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END      AS SalePrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  b.BuyPrice * v_CurrencySalePrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END      AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END      AS CurrencyPower,
                      0.00     AS BankSalePrice,
                      0.00     AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  v_CurrencyBuyPrice * b.SalePrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END      AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  v_CurrencySalePrice * b.BuyPrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END      AS CurrencySalePrice,
                      0,
                      00       AS CurrencyBankBuyPrice,
                      0.00     AS CurrencyBankSalePrice,
                      1        AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo <> p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SET p_MSG = '';
            END IF;

          IF v_CurrencyPower = 0
             THEN
              SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.SalePrice                                ELSE                                1 / SalePrice                        END         AS SalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.BuyPrice                                ELSE                                1 / BuyPrice                        END         AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقل'                                ELSE                                'اقوى'                        END         AS CurrencyPower,
                      b.BuyPrice  AS CurrencyBuyPrice,
                      b.SalePrice AS CurrencySalePrice,
                      0.00        AS BankSalePrice,
                      0.00        AS BankBuyPrice,
                      0,
                      00          AS CurrencyBankBuyPrice,
                      0.00        AS CurrencyBankSalePrice,
                      1           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDFrom = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo = p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)

              UNION

              SELECT
                      b.ID                                AS IDCruns,
                      c.CuName                            AS CurrencyIDTo,
                      1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                      1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END                                 AS CurrencyPower,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  v_CurrencyBuyPrice * b.SalePrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END                                 AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  v_CurrencySalePrice * b.BuyPrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END                                 AS CurrencySalePrice,
                      0.00                                AS BankSalePrice,
                      0.00                                AS BankBuyPrice,
                      0,
                      00                                  AS CurrencyBankBuyPrice,
                      0.00                                AS CurrencyBankSalePrice,
                      1                                   AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo <> p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);
              SET p_MSG = '';
            END IF;
        END IF;
    END IF;

  IF p_PriceType = 1
     THEN

      IF v_IsDefault = 1
         THEN
          SELECT
                  b.ID        AS IDCruns,
                  c.CuName    AS CurrencyIDTo,
                  b.SalePrice,
                  b.BuyPrice,

                  CASE                            WHEN b.CurrencyPower = 0                              THEN                              'اقوى'                            ELSE                            'أقل'                    END         AS CurrencyPower,
                  b.SalePrice AS BankSalePrice,
                  b.BuyPrice  AS BankBuyPrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.SalePrice                            ELSE                            SalePrice                    END         AS CurrencySalePrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.BuyPrice                            ELSE                            BuyPrice                    END         AS CurrencyBuyPrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.SalePrice                            ELSE                            b.SalePrice                    END         AS CurrencyBankSalePrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.BuyPrice                            ELSE                            b.BuyPrice                    END         AS CurrencyBankBuyPrice,
                  2           AS Typesd
          FROM
                  `NewCurrencyPricesOwnTb` AS a
              INNER JOIN
                NewCurrencyPriceOwnDetailsTb AS b
                  ON a.ID = b.CPID
              INNER JOIN
                `CurrencyMainTb` AS c
                  ON b.CurrencyIDTo = c.ID
          WHERE
                  a.IsActive = 1
                  AND b.IsActive = 1
                  AND c.IsActive = 1
                  AND a.CurrencyIDFrom = p_CurrencyIDFrom
                  AND a.CountryID = p_CountryID
                  AND a.PriceType = p_PriceType
                  AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

          SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                  CurrencyMainTb AS a
          WHERE
                  a.ID = p_CurrencyIDFrom;

          SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.ID = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.PriceType = p_PriceType AND a.CountryID = p_CountryID AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)) AS gc));
          SELECT
                  p_MSG;
        END IF;



      IF v_IsDefault <> 1
         THEN


          SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                  NewCurrencyPriceOwnDetailsTb AS a
              INNER JOIN
                NewCurrencyPricesOwnTb AS b
                  ON a.CPID = b.ID
          WHERE
                  a.CurrencyIDFrom = 1
                  AND a.CurrencyIDTo = p_CurrencyIDFrom
                  AND b.AccountType = p_AccounType
                  AND b.CountryID = p_CountryID;
          IF v_CurrencyPower = 1
             THEN
              SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.BuyPrice  AS BuyPrice,
                      b.SalePrice AS SalePrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقل'                                ELSE                                'اقوى'                        END         AS CurrencyPower,
                      0.00        AS BankSalePrice,
                      0.00        AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.SalePrice                                ELSE                                1 / SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.BuyPrice                                ELSE                                1 / BuyPrice                        END         AS CurrencyBuyPrice,
                      0,
                      00          AS CurrencyBankBuyPrice,
                      0.00        AS CurrencyBankSalePrice,
                      2           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDFrom = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo = p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)

              UNION

              SELECT
                      b.ID     AS IDCruns,
                      c.CuName AS CurrencyIDTo,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  b.SalePrice * v_CurrencyBuyPrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END      AS SalePrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  b.BuyPrice * v_CurrencySalePrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END      AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END      AS CurrencyPower,
                      0.00     AS BankSalePrice,
                      0.00     AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  v_CurrencyBuyPrice * b.SalePrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END      AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  v_CurrencySalePrice * b.BuyPrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END      AS CurrencySalePrice,
                      0,
                      00       AS CurrencyBankBuyPrice,
                      0.00     AS CurrencyBankSalePrice,
                      2        AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo <> p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);
              SET p_MSG = '';
            END IF;

          IF v_CurrencyPower = 0
             THEN
              SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.SalePrice                                ELSE                                1 / SalePrice                        END         AS SalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.BuyPrice                                ELSE                                1 / BuyPrice                        END         AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقل'                                ELSE                                'اقوى'                        END         AS CurrencyPower,
                      b.BuyPrice  AS CurrencyBuyPrice,
                      b.SalePrice AS CurrencySalePrice,
                      0.00        AS BankSalePrice,
                      0.00        AS BankBuyPrice,
                      0,
                      00          AS CurrencyBankBuyPrice,
                      0.00        AS CurrencyBankSalePrice,
                      2           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDFrom = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo = p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)

              UNION

              SELECT
                      b.ID                                AS IDCruns,
                      c.CuName                            AS CurrencyIDTo,
                      1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                      1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END                                 AS CurrencyPower,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  v_CurrencyBuyPrice * b.SalePrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END                                 AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  v_CurrencySalePrice * b.BuyPrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END                                 AS CurrencySalePrice,
                      0.00                                AS BankSalePrice,
                      0.00                                AS BankBuyPrice,
                      0,
                      00                                  AS CurrencyBankBuyPrice,
                      0.00                                AS CurrencyBankSalePrice,
                      2                                   AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo <> p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SET p_MSG = '';
            END IF;
        END IF;
    END IF;

  IF p_PriceType = 2
     THEN
      IF p_AccounType = 0
         THEN

          IF v_IsDefault = 1
             THEN
              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.SalePrice,
                      b.BuyPrice,

                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END         AS CurrencyPower,
                      b.SalePrice AS BankSalePrice,
                      b.BuyPrice  AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                BuyPrice                        END         AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                b.SalePrice                        END         AS CurrencyBankSalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                b.BuyPrice                        END         AS CurrencyBankBuyPrice,
                      3           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = p_CurrencyIDFrom
                      AND a.CountryID = p_CountryID
                      AND a.PriceType = p_PriceType
                      AND a.AccountType = p_AccounType
                       AND BankID=p_BankID;

              SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                      CurrencyMainTb AS a
              WHERE
                      a.ID = p_CurrencyIDFrom;

              SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.ID = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.PriceType = p_PriceType AND a.CountryID = p_CountryID AND a.AccountType = p_AccounType AND BankID=p_BankID) AS gc));
              SELECT
                      p_MSG;
            END IF;



          IF v_IsDefault <> 1
             THEN


              SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.AccountType = p_AccounType
                      AND b.CountryID = p_CountryID
                      AND b.PriceType = p_PriceType;
              IF v_CurrencyPower = 1
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.AccountType = p_AccounType
                          AND b.CountryID = p_CountryID
                          AND b.PriceType = p_PriceType
                          AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          b.BuyPrice  AS BuyPrice,
                          b.SalePrice AS SalePrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS CurrencySalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS CurrencyBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          3           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID     AS IDCruns,
                          c.CuName AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.SalePrice * v_CurrencyBuyPrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.BuyPrice * v_CurrencySalePrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END      AS CurrencyPower,
                          0.00     AS BankSalePrice,
                          0.00     AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS CurrencySalePrice,
                          0,
                          00       AS CurrencyBankBuyPrice,
                          0.00     AS CurrencyBankSalePrice,
                          3        AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID;

                  SET p_MSG = '';
                END IF;

              IF v_CurrencyPower = 0
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.AccountType = p_AccounType
                          AND b.CountryID = p_CountryID
                          AND b.PriceType = p_PriceType
                           AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          b.BuyPrice  AS CurrencyBuyPrice,
                          b.SalePrice AS CurrencySalePrice,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          3           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID                                AS IDCruns,
                          c.CuName                            AS CurrencyIDTo,
                          1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                          1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END                                 AS CurrencyPower,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END                                 AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END                                 AS CurrencySalePrice,
                          0.00                                AS BankSalePrice,
                          0.00                                AS BankBuyPrice,
                          0,
                          00                                  AS CurrencyBankBuyPrice,
                          0.00                                AS CurrencyBankSalePrice,
                          3                                   AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.PriceType = p_PriceType
                          AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;
            END IF;
        END IF;
      IF p_AccounType = 1
         THEN

          IF v_IsDefault = 1
             THEN
              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.SalePrice,
                      b.BuyPrice,

                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END         AS CurrencyPower,
                      b.SalePrice AS BankSalePrice,
                      b.BuyPrice  AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                BuyPrice                        END         AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                b.SalePrice                        END         AS CurrencyBankSalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                BuyPrice                        END         AS CurrencyBankBuyPrice,
                      3           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = p_CurrencyIDFrom
                      AND a.CountryID = p_CountryID
                      AND a.AccountType = p_AccounType
                      AND a.BranchID = p_BranchID
                      AND a.PriceType = p_PriceType
 AND BankID=p_BankID;

              SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                      CurrencyMainTb AS a
              WHERE
                      a.ID = p_CurrencyIDFrom;

              SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.ID = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.AccountType = p_AccounType AND a.CountryID = p_CountryID AND a.BranchID = p_BranchID AND a.PriceType = p_PriceType AND BankID=p_BankID) AS gc));
              SELECT
                      p_MSG;
            END IF;



          IF v_IsDefault <> 1
             THEN


              SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.AccountType = p_AccounType
                      AND b.CountryID = p_CountryID
                      AND b.BranchID = p_BranchID
                      AND b.PriceType = p_PriceType
                       AND BankID=p_BankID;
              IF v_CurrencyPower = 1
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.AccountType = p_AccounType
                          AND b.CountryID = p_CountryID
                          AND b.BranchID = p_BranchID
                          AND b.PriceType = p_PriceType
                           AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          b.BuyPrice  AS BuyPrice,
                          b.SalePrice AS SalePrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS CurrencySalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS CurrencyBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          3           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.BranchID = p_BranchID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID     AS IDCruns,
                          c.CuName AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.SalePrice * v_CurrencyBuyPrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.BuyPrice * v_CurrencySalePrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END      AS CurrencyPower,
                          0.00     AS BankSalePrice,
                          0.00     AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS CurrencySalePrice,
                          0,
                          00       AS CurrencyBankBuyPrice,
                          0.00     AS CurrencyBankSalePrice,
                          3        AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.id = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.BranchID = p_BranchID
                          AND a.PriceType = p_PriceType
 AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;

              IF v_CurrencyPower = 0
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.AccountType = p_AccounType
                          AND b.CountryID = p_CountryID
                          AND b.BranchID = p_BranchID
                          AND b.PriceType = p_PriceType
                           AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          b.BuyPrice  AS CurrencyBuyPrice,
                          b.SalePrice AS CurrencySalePrice,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          3           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.BranchID = p_BranchID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID                                AS IDCruns,
                          c.CuName                            AS CurrencyIDTo,
                          1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                          1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END                                 AS CurrencyPower,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END                                 AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END                                 AS CurrencySalePrice,
                          0.00                                AS BankSalePrice,
                          0.00                                AS BankBuyPrice,
                          0,
                          00                                  AS CurrencyBankBuyPrice,
                          0.00                                AS CurrencyBankSalePrice,
                          3                                   AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.BranchID = p_BranchID
                          AND a.PriceType = p_PriceType
 AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;
            END IF;
        END IF;

      IF p_AccounType = 2
         THEN

          IF v_IsDefault = 1
             THEN
              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.SalePrice,
                      b.BuyPrice,

                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END         AS CurrencyPower,
                      b.SalePrice AS BankSalePrice,
                      b.BuyPrice  AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                BuyPrice                        END         AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                b.SalePrice                        END         AS CurrencyBankSalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                b.BuyPrice                        END         AS CurrencyBankBuyPrice,
                      4           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.id = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = p_CurrencyIDFrom
                      AND a.CountryID = p_CountryID
                      AND a.AccountType = p_AccounType
                      AND a.ServiceTypeID = p_BranchID
                      AND a.PriceType = p_PriceType
 AND BankID=p_BankID;

              SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                      CurrencyMainTb AS a
              WHERE
                      a.ID = p_CurrencyIDFrom;

              SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.id = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.AccountType = p_AccounType AND a.CountryID = p_CountryID AND a.ServiceTypeID = p_BranchID AND a.PriceType = p_PriceType AND BankID=p_BankID) AS gc));
              SELECT
                      p_MSG;
            END IF;



          IF v_IsDefault <> 1
             THEN


              SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.AccountType = p_AccounType
                      AND b.CountryID = p_CountryID
                      AND b.ServiceTypeID = p_BranchID
                      AND b.PriceType = p_PriceType
                       AND BankID=p_BankID;
              IF v_CurrencyPower = 1
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.AccountType = p_AccounType
                          AND b.CountryID = p_CountryID
                          AND b.ServiceTypeID = p_BranchID
                          AND b.PriceType = p_PriceType
                           AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          b.BuyPrice  AS BuyPrice,
                          b.SalePrice AS SalePrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS CurrencySalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS CurrencyBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          4           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.id = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.ServiceTypeID = p_BranchID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID     AS IDCruns,
                          c.CuName AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.SalePrice * v_CurrencyBuyPrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.BuyPrice * v_CurrencySalePrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END      AS CurrencyPower,
                          0.00     AS BankSalePrice,
                          0.00     AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS CurrencySalePrice,
                          0,
                          00       AS CurrencyBankBuyPrice,
                          0.00     AS CurrencyBankSalePrice,
                          4        AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.id = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.ServiceTypeID = p_BranchID
                          AND a.PriceType = p_PriceType
 AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;

              IF v_CurrencyPower = 0
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.AccountType = p_AccounType
                          AND b.CountryID = p_CountryID
                          AND b.ServiceTypeID = p_BranchID
                          AND b.PriceType = p_PriceType
                           AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          b.BuyPrice  AS CurrencyBuyPrice,
                          b.SalePrice AS CurrencySalePrice,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          4           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a


                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.id = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.ServiceTypeID = p_BranchID
                          AND a.PriceType = p_PriceType
                           AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID                                AS IDCruns,
                          c.CuName                            AS CurrencyIDTo,
                          1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                          1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END                                 AS CurrencyPower,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END                                 AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END                                 AS CurrencySalePrice,
                          0.00                                AS BankSalePrice,
                          0.00                                AS BankBuyPrice,
                          0,
                          00                                  AS CurrencyBankBuyPrice,
                          0.00                                AS CurrencyBankSalePrice,
                          4                                   AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.id = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.AccountType = p_AccounType
                          AND a.CountryID = p_CountryID
                          AND a.ServiceTypeID = p_BranchID
                          AND a.PriceType = p_PriceType
 AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;
            END IF;
        END IF;
    END IF;

  IF p_PriceType = 3
     THEN

      IF v_IsDefault = 1
         THEN
          SELECT
                  b.ID        AS IDCruns,
                  c.CuName    AS CurrencyIDTo,
                  b.SalePrice,
                  b.BuyPrice,

                  CASE                            WHEN b.CurrencyPower = 0                              THEN                              'اقوى'                            ELSE                            'أقل'                    END         AS CurrencyPower,
                  b.SalePrice AS BankSalePrice,
                  b.BuyPrice  AS BankBuyPrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.SalePrice                            ELSE                            SalePrice                    END         AS CurrencySalePrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.BuyPrice                            ELSE                            BuyPrice                    END         AS CurrencyBuyPrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.SalePrice                            ELSE                            b.SalePrice                    END         AS CurrencyBankSalePrice,
                  CASE                            WHEN CurrencyPower = 1                              THEN                              1 / b.BuyPrice                            ELSE                            b.BuyPrice                    END         AS CurrencyBankBuyPrice,
                  5           AS Typesd
          FROM
                  `NewCurrencyPricesOwnTb` AS a
              INNER JOIN
                NewCurrencyPriceOwnDetailsTb AS b
                  ON a.ID = b.CPID
              INNER JOIN
                `CurrencyMainTb` AS c
                  ON b.CurrencyIDTo = c.ID
          WHERE
                  a.IsActive = 1
                  AND b.IsActive = 1
                  AND c.IsActive = 1
                  AND a.CurrencyIDFrom = p_CurrencyIDFrom
                  AND a.CountryID = p_CountryID
                  AND a.PriceType = p_PriceType
                 AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

          SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                  CurrencyMainTb AS a
          WHERE
                  a.ID = p_CurrencyIDFrom;

          SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.ID = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.PriceType = p_PriceType AND a.CountryID = p_CountryID AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)) AS gc));
          SELECT
                  p_MSG;
        END IF;



      IF v_IsDefault <> 1
         THEN


          SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                  NewCurrencyPriceOwnDetailsTb AS a
              INNER JOIN
                NewCurrencyPricesOwnTb AS b
                  ON a.CPID = b.ID
          WHERE
                  a.CurrencyIDFrom = 1
                  AND a.CurrencyIDTo = p_CurrencyIDFrom
                  AND b.PriceType = p_PriceType
                  AND b.CountryID = p_CountryID
                  AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);
          IF v_CurrencyPower = 1
             THEN
              SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.BuyPrice  AS BuyPrice,
                      b.SalePrice AS SalePrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقل'                                ELSE                                'اقوى'                        END         AS CurrencyPower,
                      0.00        AS BankSalePrice,
                      0.00        AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.SalePrice                                ELSE                                1 / SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.BuyPrice                                ELSE                                1 / BuyPrice                        END         AS CurrencyBuyPrice,
                      0,
                      00          AS CurrencyBankBuyPrice,
                      0.00        AS CurrencyBankSalePrice,
                      5           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDFrom = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo = p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)

              UNION

              SELECT
                      b.ID     AS IDCruns,
                      c.CuName AS CurrencyIDTo,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  b.SalePrice * v_CurrencyBuyPrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END      AS SalePrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  b.BuyPrice * v_CurrencySalePrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END      AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END      AS CurrencyPower,
                      0.00     AS BankSalePrice,
                      0.00     AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  v_CurrencyBuyPrice * b.SalePrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END      AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 0                                  THEN                                  v_CurrencySalePrice * b.BuyPrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END      AS CurrencySalePrice,
                      0,
                      00       AS CurrencyBankBuyPrice,
                      0.00     AS CurrencyBankSalePrice,
                      5        AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo <> p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SET p_MSG = '';
            END IF;

          IF v_CurrencyPower = 0
             THEN
              SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.SalePrice                                ELSE                                1 / SalePrice                        END         AS SalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  b.BuyPrice                                ELSE                                1 / BuyPrice                        END         AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقل'                                ELSE                                'اقوى'                        END         AS CurrencyPower,
                      b.BuyPrice  AS CurrencyBuyPrice,
                      b.SalePrice AS CurrencySalePrice,
                      0.00        AS BankSalePrice,
                      0.00        AS BankBuyPrice,
                      0,
                      00          AS CurrencyBankBuyPrice,
                      0.00        AS CurrencyBankSalePrice,
                      5           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a


                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDFrom = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo = p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID)

              UNION

              SELECT
                      b.ID                                AS IDCruns,
                      c.CuName                            AS CurrencyIDTo,
                      1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                      1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END                                 AS CurrencyPower,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  v_CurrencyBuyPrice * b.SalePrice                                ELSE                                v_CurrencyBuyPrice / SalePrice                        END                                 AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  v_CurrencySalePrice * b.BuyPrice                                ELSE                                v_CurrencySalePrice / BuyPrice                        END                                 AS CurrencySalePrice,
                      0.00                                AS BankSalePrice,
                      0.00                                AS BankBuyPrice,
                      0,
                      00                                  AS CurrencyBankBuyPrice,
                      0.00                                AS CurrencyBankSalePrice,
                      5                                   AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = 1
                      AND b.CurrencyIDTo <> p_CurrencyIDFrom
                      AND a.PriceType = p_PriceType
                      AND a.CountryID = p_CountryID
                      AND ((v_AccParent IS NULL AND BankID=0) OR BankID = p_BankID);

              SET p_MSG = '';
            END IF;
        END IF;
    END IF;


  IF p_PriceType = 2
     THEN
      IF p_AccounType = 3
         THEN



          IF v_IsDefault = 1
             THEN
              SELECT
                      b.ID        AS IDCruns,
                      c.CuName    AS CurrencyIDTo,
                      b.SalePrice,
                      b.BuyPrice,

                      CASE                                WHEN b.CurrencyPower = 0                                  THEN                                  'اقوى'                                ELSE                                'أقل'                        END         AS CurrencyPower,
                      b.SalePrice AS BankSalePrice,
                      b.BuyPrice  AS BankBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                SalePrice                        END         AS CurrencySalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                BuyPrice                        END         AS CurrencyBuyPrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.SalePrice                                ELSE                                SalePrice                        END         AS CurrencyBankSalePrice,
                      CASE                                WHEN CurrencyPower = 1                                  THEN                                  1 / b.BuyPrice                                ELSE                                BuyPrice                        END         AS CurrencyBankBuyPrice,
                      5           AS Typesd
              FROM
                      `NewCurrencyPricesOwnTb` AS a
                  INNER JOIN
                    NewCurrencyPriceOwnDetailsTb AS b
                      ON a.ID = b.CPID
                  INNER JOIN
                    `CurrencyMainTb` AS c
                      ON b.CurrencyIDTo = c.ID
              WHERE
                      a.IsActive = 1
                      AND b.IsActive = 1
                      AND c.IsActive = 1
                      AND a.CurrencyIDFrom = p_CurrencyIDFrom
                      AND a.CountryID = p_CountryID
                      AND a.PriceType = p_PriceType
                      AND a.AccountType = p_AccounType
AND BankID=p_BankID;

              SELECT a.CuName INTO v_CurrencyIDFromNme FROM
                      CurrencyMainTb AS a
              WHERE
                      a.ID = p_CurrencyIDFrom;

              SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `NewCurrencyPricesOwnTb` AS a INNER JOIN NewCurrencyPriceOwnDetailsTb AS b ON a.ID = b.CPID INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1 AND a.PriceType = p_PriceType AND a.CountryID = p_CountryID AND a.AccountType = p_AccounType AND BankID=p_BankID) AS gc));
              SELECT
                      p_MSG;
            END IF;



          IF v_IsDefault <> 1
             THEN

              SELECT a.CurrencyPower INTO v_CurrencyPower FROM
                      NewCurrencyPriceOwnDetailsTb AS a
                  INNER JOIN
                    NewCurrencyPricesOwnTb AS b
                      ON a.CPID = b.ID
              WHERE
                      a.CurrencyIDFrom = 1
                      AND a.CurrencyIDTo = p_CurrencyIDFrom
                      AND b.PriceType = p_PriceType
                      AND b.CountryID = p_CountryID
                      AND b.AccountType = p_AccounType
                      AND BankID=p_BankID;
              IF v_CurrencyPower = 1
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.PriceType = p_PriceType
                          AND b.CountryID = p_CountryID
                          AND b.AccountType = p_AccounType
                          AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          b.BuyPrice  AS BuyPrice,
                          b.SalePrice AS SalePrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS CurrencySalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS CurrencyBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          5           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.PriceType = p_PriceType
                          AND a.CountryID = p_CountryID
                          AND a.AccountType = p_AccounType
                          AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID     AS IDCruns,
                          c.CuName AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.SalePrice * v_CurrencyBuyPrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      b.BuyPrice * v_CurrencySalePrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END      AS CurrencyPower,
                          0.00     AS BankSalePrice,
                          0.00     AS BankBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END      AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 0                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END      AS CurrencySalePrice,
                          0,
                          00       AS CurrencyBankBuyPrice,
                          0.00     AS CurrencyBankSalePrice,
                          5        AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.PriceType = p_PriceType
                          AND a.CountryID = p_CountryID
                          AND a.AccountType = p_AccounType
AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;

              IF v_CurrencyPower = 0
                 THEN
                  SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
                          NewCurrencyPriceOwnDetailsTb AS a
                      INNER JOIN
                        NewCurrencyPricesOwnTb AS b
                          ON a.CPID = b.ID
                  WHERE
                          a.CurrencyIDFrom = 1
                          AND a.CurrencyIDTo = p_CurrencyIDFrom
                          AND b.PriceType = p_PriceType
                          AND b.CountryID = p_CountryID
                          AND b.AccountType = p_AccounType
                         AND BankID=p_BankID;

                  SELECT
                          b.ID        AS IDCruns,
                          c.CuName    AS CurrencyIDTo,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.SalePrice                                    ELSE                                    1 / SalePrice                            END         AS SalePrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      b.BuyPrice                                    ELSE                                    1 / BuyPrice                            END         AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقل'                                    ELSE                                    'اقوى'                            END         AS CurrencyPower,
                          b.BuyPrice  AS CurrencyBuyPrice,
                          b.SalePrice AS CurrencySalePrice,
                          0.00        AS BankSalePrice,
                          0.00        AS BankBuyPrice,
                          0,
                          00          AS CurrencyBankBuyPrice,
                          0.00        AS CurrencyBankSalePrice,
                          5           AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDFrom = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo = p_CurrencyIDFrom
                          AND a.PriceType = p_PriceType
                          AND a.CountryID = p_CountryID
                          AND a.AccountType = p_AccounType
                          AND BankID=p_BankID

                  UNION

                  SELECT
                          b.ID                                AS IDCruns,
                          c.CuName                            AS CurrencyIDTo,
                          1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
                          1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
                          CASE                                    WHEN b.CurrencyPower = 0                                      THEN                                      'اقوى'                                    ELSE                                    'أقل'                            END                                 AS CurrencyPower,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencyBuyPrice * b.SalePrice                                    ELSE                                    v_CurrencyBuyPrice / SalePrice                            END                                 AS CurrencyBuyPrice,
                          CASE                                    WHEN CurrencyPower = 1                                      THEN                                      v_CurrencySalePrice * b.BuyPrice                                    ELSE                                    v_CurrencySalePrice / BuyPrice                            END                                 AS CurrencySalePrice,
                          0.00                                AS BankSalePrice,
                          0.00                                AS BankBuyPrice,
                          0,
                          00                                  AS CurrencyBankBuyPrice,
                          0.00                                AS CurrencyBankSalePrice,
                          5                                   AS Typesd
                  FROM
                          `NewCurrencyPricesOwnTb` AS a
                      INNER JOIN
                        NewCurrencyPriceOwnDetailsTb AS b
                          ON a.ID = b.CPID
                      INNER JOIN
                        `CurrencyMainTb` AS c
                          ON b.CurrencyIDTo = c.ID
                  WHERE
                          a.IsActive = 1
                          AND b.IsActive = 1
                          AND c.IsActive = 1
                          AND a.CurrencyIDFrom = 1
                          AND b.CurrencyIDTo <> p_CurrencyIDFrom
                          AND a.PriceType = p_PriceType
                          AND a.CountryID = p_CountryID
                          AND a.AccountType = p_AccounType
AND BankID=p_BankID;
                  SET p_MSG = '';
                END IF;
            END IF;
        END IF;
    END IF;
  COMMIT;
END
$$
DELIMITER ;


DROP PROCEDURE IF EXISTS `OwnCurrencyPriceDetailsTb_Grid`;
DELIMITER $$
CREATE PROCEDURE `OwnCurrencyPriceDetailsTb_Grid`(IN `p_CurrencyIDFrom` INT, INOUT `p_MSG` LONGTEXT, IN `p_typeMSGN` INT, IN `p_TypeID` INT)
BEGIN
DECLARE v_CurrencyIDFromNme LONGTEXT;
DECLARE v_CurrencySalePrice FLOAT;
DECLARE v_CurrencyBuyPrice FLOAT;
DECLARE v_CurrencyPower BIT;


SET SESSION group_concat_max_len = 1000000;
	START TRANSACTION;


	IF p_TypeID = 2
	 THEN
	IF p_typeMSGN = 1
		 THEN

			IF p_CurrencyIDFrom = 1
				 THEN
					SELECT
							b.ID			AS IDCruns,
							c.CuName		AS CurrencyIDTo,
							b.SalePrice,
							b.BuyPrice,
							CASE  									WHEN b.CurrencyPower = 0  										THEN  										'اقوى'  									ELSE  									'أقل'  							END				AS CurrencyPower,
							b.BankSalePrice AS BankSalePrice,
							b.BankBuyPrice  AS BankBuyPrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.SalePrice  									ELSE  									SalePrice  							END				AS CurrencySalePrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.BuyPrice  									ELSE  									BuyPrice  							END				AS CurrencyBuyPrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.BankSalePrice  									ELSE  									BankSalePrice  							END				AS CurrencyBankSalePrice,
							CASE  									WHEN CurrencyPower = 1  										THEN  										1 / b.BankBuyPrice  									ELSE  									BankBuyPrice  							END				AS CurrencyBankBuyPrice,
							1				AS Typesd
					FROM
							`CurrencyPricesOwnTb` AS a
						INNER JOIN
							CurrencyPriceOwnDetailsTb AS b
								ON a.CurrencyIDFrom = b.CurrencyIDFrom
						INNER JOIN
							`CurrencyMainTb` AS c
								ON b.CurrencyIDTo = c.ID
					WHERE
							a.IsActive = 1
							AND b.IsActive = 1
							AND c.IsActive = 1
							AND a.CurrencyIDFrom = p_CurrencyIDFrom;

					SELECT a.CuName INTO v_CurrencyIDFromNme FROM
							CurrencyMainTb AS a
					WHERE
							a.ID = p_CurrencyIDFrom;

					SET p_MSG = CONCAT('اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى', ' ', ' مقابل', ' ', v_CurrencyIDFromNme, ' ', (SELECT GROUP_CONCAT(CONCAT(gc.rn, gc.CuName, ' ', 'سعر الشراء : ', gc.BuyPrice, ' ', 'سعر البيع  : ', gc.SalePrice, ' ', 'سعر التحويل ', 0.00, ' ', 'سعر الشراء ', 0.00) ORDER BY gc.CuName SEPARATOR ', ') FROM (SELECT ROW_NUMBER() OVER (ORDER BY c.CuName) AS rn, c.CuName AS CuName, BuyPrice AS BuyPrice, SalePrice AS SalePrice FROM `CurrencyPricesOwnTb` AS a INNER JOIN CurrencyPriceOwnDetailsTb AS b ON a.CurrencyIDFrom = b.CurrencyIDFrom INNER JOIN `CurrencyMainTb` AS c ON b.CurrencyIDTo = c.ID WHERE a.IsActive = 1 AND b.IsActive = 1 AND c.IsActive = 1 AND a.CurrencyIDFrom = 1) AS gc));
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
									b.ID		AS IDCruns,
									c.CuName	AS CurrencyIDTo,
									b.BuyPrice  AS BuyPrice,
									b.SalePrice AS SalePrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقل'  											ELSE  											'اقوى'  									END			AS CurrencyPower,
									0.00		AS BankSalePrice,
									0.00		AS BankBuyPrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.SalePrice  											ELSE  											1 / SalePrice  									END			AS CurrencySalePrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.BuyPrice  											ELSE  											1 / BuyPrice  									END			AS CurrencyBuyPrice,
									0,
									00			AS CurrencyBankBuyPrice,
									0.00		AS CurrencyBankSalePrice,
									1			AS Typesd
							FROM
									`CurrencyPricesOwnTb` AS a
								INNER JOIN
									CurrencyPriceOwnDetailsTb AS b
										ON a.CurrencyIDFrom = b.CurrencyIDFrom
								INNER JOIN
									`CurrencyMainTb` AS c
										ON b.CurrencyIDFrom = c.ID
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo = p_CurrencyIDFrom

							UNION

							SELECT
									b.ID	 AS IDCruns,
									c.CuName AS CurrencyIDTo,
									CASE  											WHEN CurrencyPower = 0  												THEN  												b.SalePrice * v_CurrencyBuyPrice  											ELSE  											v_CurrencyBuyPrice / SalePrice  									END		 AS SalePrice,
									CASE  											WHEN CurrencyPower = 0  												THEN  												b.BuyPrice * v_CurrencySalePrice  											ELSE  											v_CurrencySalePrice / BuyPrice  									END		 AS BuyPrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقوى'  											ELSE  											'أقل'  									END		 AS CurrencyPower,
									0.00	 AS BankSalePrice,
									0.00	 AS BankBuyPrice,
									CASE  											WHEN CurrencyPower = 0  												THEN  												v_CurrencyBuyPrice * b.SalePrice  											ELSE  											v_CurrencyBuyPrice / SalePrice  									END		 AS CurrencyBuyPrice,
									CASE  											WHEN CurrencyPower = 0  												THEN  												v_CurrencySalePrice * b.BuyPrice  											ELSE  											v_CurrencySalePrice / BuyPrice  									END		 AS CurrencySalePrice,
									0,
									00		 AS CurrencyBankBuyPrice,
									0.00	 AS CurrencyBankSalePrice,
									1		 AS Typesd
							FROM
									`CurrencyPricesOwnTb` AS a
								INNER JOIN
									CurrencyPriceOwnDetailsTb AS b
										ON a.CurrencyIDFrom = b.CurrencyIDFrom
								INNER JOIN
									`CurrencyMainTb` AS c
										ON b.CurrencyIDTo = c.ID
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo <> p_CurrencyIDFrom;

							SET p_MSG = '';
						END IF;

					IF v_CurrencyPower = 0
						 THEN
							SELECT a.SalePrice, a.BuyPrice INTO v_CurrencySalePrice, v_CurrencyBuyPrice FROM
									CurrencyPriceOwnDetailsTb AS a
							WHERE
									a.CurrencyIDFrom = 1
									AND a.CurrencyIDTo = p_CurrencyIDFrom;

							SELECT
									b.ID		AS IDCruns,
									c.CuName	AS CurrencyIDTo,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.SalePrice  											ELSE  											1 / SalePrice  									END			AS SalePrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												b.BuyPrice  											ELSE  											1 / BuyPrice  									END			AS BuyPrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقل'  											ELSE  											'اقوى'  									END			AS CurrencyPower,
									b.BuyPrice  AS CurrencyBuyPrice,
									b.SalePrice AS CurrencySalePrice,
									0.00		AS BankSalePrice,
									0.00		AS BankBuyPrice,
									0,
									00			AS CurrencyBankBuyPrice,
									0.00		AS CurrencyBankSalePrice,
									1			AS Typesd
							FROM
									`CurrencyPricesOwnTb` AS a


								INNER JOIN
									CurrencyPriceOwnDetailsTb AS b
										ON a.CurrencyIDFrom = b.CurrencyIDFrom
								INNER JOIN
									`CurrencyMainTb` AS c
										ON b.CurrencyIDFrom = c.ID
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo = p_CurrencyIDFrom

							UNION

							SELECT
									b.ID								AS IDCruns,
									c.CuName							AS CurrencyIDTo,
									1 / (v_CurrencySalePrice * BuyPrice) AS SalePrice,
									1 / (v_CurrencyBuyPrice * SalePrice) AS BuyPrice,
									CASE  											WHEN b.CurrencyPower = 0  												THEN  												'اقوى'  											ELSE  											'أقل'  									END									AS CurrencyPower,
									CASE  											WHEN CurrencyPower = 1  												THEN  												v_CurrencyBuyPrice * b.SalePrice  											ELSE  											v_CurrencyBuyPrice / SalePrice  									END									AS CurrencyBuyPrice,
									CASE  											WHEN CurrencyPower = 1  												THEN  												v_CurrencySalePrice * b.BuyPrice  											ELSE  											v_CurrencySalePrice / BuyPrice  									END									AS CurrencySalePrice,
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
							WHERE
									a.IsActive = 1
									AND b.IsActive = 1
									AND c.IsActive = 1
									AND a.CurrencyIDFrom = 1
									AND b.CurrencyIDTo <> p_CurrencyIDFrom;

							SET p_MSG = '';
						END IF;
				END IF;
		END IF;
	IF p_typeMSGN = 2
		 THEN
			SELECT
					b.ID	 AS IDCruns,
					c.CuName AS CurrencyIDTo,
					b.SalePrice,
					b.BuyPrice,
					CASE  							WHEN b.CurrencyPower = 0  								THEN  								'اقوى'  							ELSE  							'أقل'  					END		 AS CurrencyPower,
					0.00	 AS BankSalePrice,
					0.00	 AS BankBuyPrice,
					CASE  							WHEN CurrencyPower = 1  								THEN  								1 / b.SalePrice  							ELSE  							b.SalePrice  					END		 AS CurrencySalePrice,
					CASE  							WHEN CurrencyPower = 1  								THEN  								1 / b.BuyPrice  							ELSE  							b.BuyPrice  					END		 AS CurrencyBuyPrice,
					0.00	 AS CurrencyBankSalePrice,
					0.00	 AS CurrencyBankBuyPrice,
					2		 AS Typesd
			FROM
					`Currency_settingForBancksRet` AS a
				INNER JOIN
					CurrencyPriceDetailsBancksTb AS b
						ON a.CurrencyFrom = b.CurrencyIDFrom
				INNER JOIN
					`CurrencyMainTb` AS c
						ON b.CurrencyIDTo = c.ID
			WHERE
					a.IsActive = 1
					AND b.IsActive = 1
					AND c.IsActive = 1
					AND b.Banck_ID = p_CurrencyIDFrom;

			SET p_MSG = 'لايوجد اسعار متوفرة في الوقت الحالي ';
		END IF;
		END IF;
	COMMIT;
END
$$
DELIMITER ;

