CREATE PROC [dbo].[ZRPT_CurrencyPriceDetailsTb_Grid]
	@CurrencyIDFrom AS INT,
	@MSG			AS NVARCHAR(MAX) OUTPUT,
	@typeMSGN		AS INT,
	@TypeID AS INT
AS
	SET NOCOUNT ON
	SET XACT_ABORT ON
	DECLARE @typeMSGNNAME AS NVARCHAR(MAX)
	IF @TypeID = 1
	BEGIN
	IF @typeMSGN = 1
		BEGIN
		SELECT
				@typeMSGNNAME = 'نقدي'
		END
	BEGIN
		IF @CurrencyIDFrom = 1
			BEGIN
				SELECT
						e.ID			AS CID,
						e.CuName		AS CNAME,
						@typeMSGNNAME   AS typeMSGNNAME,
						b.ID			AS IDCruns,
						c.CuName		AS CurrencyIDTo,
						b.SalePrice,
						b.BuyPrice,
						CASE
								WHEN b.CurrencyPower = 0
									THEN
									'اقوى'
								ELSE
								'أقل'
						END				AS CurrencyPower,
						b.BankSalePrice AS BankSalePrice,
						b.BankBuyPrice  AS BankBuyPrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.SalePrice
								ELSE
								SalePrice
						END				AS CurrencySalePrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.BuyPrice
								ELSE
								BuyPrice
						END				AS CurrencyBuyPrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.BankSalePrice
								ELSE
								BankSalePrice
						END				AS CurrencyBankSalePrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.BankBuyPrice
								ELSE
								BankBuyPrice
						END				AS CurrencyBankBuyPrice,
						1				AS Typesd
				FROM
						[dbo].[CurrencyPricesTb] AS a
					INNER JOIN
						CurrencyPriceDetailsTb AS b
							ON a.CurrencyIDFrom = b.CurrencyIDFrom
					INNER JOIN
						[dbo].[CurrencyMainTb] AS c
							ON b.CurrencyIDTo = c.ID
					INNER JOIN
						[dbo].[CurrencyMainTb] AS e
							ON @CurrencyIDFrom = e.ID
				WHERE
						a.Isactive = 1
						AND b.Isactive = 1
						AND c.Isactive = 1
						AND a.CurrencyIDFrom = @CurrencyIDFrom
				DECLARE @CurrencyIDFromNme AS NVARCHAR(MAX)
				SELECT
						@CurrencyIDFromNme = a.CuName
				FROM
						CurrencyMainTb AS a
				WHERE
						a.ID = @CurrencyIDFrom
				SET @MSG = 'اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى' + ' ' + ' مقابل' + ' ' + @CurrencyIDFromNme + ' '
				+ STUFF((SELECT
							', ',
							CONVERT(NVARCHAR, ROW_NUMBER() OVER (ORDER BY CuName)) + c.CuName + ' '
							+ 'سعر الشراء : ' + CONVERT(NVARCHAR, BuyPrice) + ' ' + 'سعر البيع  : ' + CONVERT(NVARCHAR, SalePrice)
							+ ' ' + 'سعر التحويل ' + CONVERT(NVARCHAR, 0.00) + ' ' + 'سعر الشراء ' + CONVERT(NVARCHAR, 0.00)
					FROM
							[dbo].[CurrencyPricesTb] AS a
						INNER JOIN
							CurrencyPriceDetailsTb AS b
								ON a.CurrencyIDFrom = b.CurrencyIDFrom
						INNER JOIN
							[dbo].[CurrencyMainTb] AS c
								ON @CurrencyIDFrom = c.ID
					WHERE
							a.Isactive = 1
							AND b.Isactive = 1
							AND c.Isactive = 1
							AND a.CurrencyIDFrom = 1
					FOR XML PATH (''))
				, 1, 2, '')
				SELECT
						@MSG
			END
		IF @CurrencyIDFrom <> 1
			BEGIN

				DECLARE @CurrencySalePrice AS FLOAT,
						@CurrencyBuyPrice  AS FLOAT,
						@CurrencyPower	   AS BIT
				SELECT
						@CurrencyPower = a.CurrencyPower
				FROM
						CurrencyPriceDetailsTb AS a
				WHERE
						a.CurrencyIDFrom = 1
						AND a.CurrencyIDTo = @CurrencyIDFrom
				IF @CurrencyPower = 1
					BEGIN
						SELECT
								@CurrencySalePrice = a.SalePrice,
								@CurrencyBuyPrice  = a.BuyPrice
						FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = @CurrencyIDFrom
						---//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								@typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								b.BuyPrice	  AS BuyPrice,
								b.SalePrice	  AS SalePrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقل'
										ELSE
										'اقوى'
								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											b.SalePrice
										ELSE
										1 / SalePrice
								END			  AS CurrencySalePrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											b.BuyPrice
										ELSE
										1 / BuyPrice
								END			  AS CurrencyBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								[dbo].[CurrencyPricesTb] AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = @CurrencyIDFrom
						---////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						UNION
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								@typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE
										WHEN CurrencyPower = 0
											THEN
											b.SalePrice * @CurrencyBuyPrice
										ELSE
										@CurrencyBuyPrice / SalePrice
								END			  AS SalePrice,
								CASE
										WHEN CurrencyPower = 0
											THEN
											b.BuyPrice * @CurrencySalePrice
										ELSE
										@CurrencySalePrice / BuyPrice
								END			  AS BuyPrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقوى'
										ELSE
										'أقل'
								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE
										WHEN CurrencyPower = 0
											THEN
											@CurrencyBuyPrice * b.SalePrice
										ELSE
										@CurrencyBuyPrice / SalePrice
								END			  AS CurrencyBuyPrice,

								CASE
										WHEN CurrencyPower = 0
											THEN
											@CurrencySalePrice * b.BuyPrice
										ELSE
										@CurrencySalePrice / BuyPrice
								END			  AS CurrencySalePrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								[dbo].[CurrencyPricesTb] AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> @CurrencyIDFrom
						SET @MSG = ''
					END
				IF @CurrencyPower = 0
					BEGIN
						SELECT
								@CurrencySalePrice = a.SalePrice,
								@CurrencyBuyPrice  = a.BuyPrice
						FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = @CurrencyIDFrom
						---//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								@typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE
										WHEN CurrencyPower = 1
											THEN
											b.SalePrice
										ELSE
										1 / SalePrice
								END			  AS SalePrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											b.BuyPrice
										ELSE
										1 / BuyPrice
								END			  AS BuyPrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقل'
										ELSE
										'اقوى'
								END			  AS CurrencyPower,
								b.BuyPrice	  AS CurrencyBuyPrice,
								b.SalePrice	  AS CurrencySalePrice,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								[dbo].[CurrencyPricesTb] AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = @CurrencyIDFrom
						-- ---////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						UNION
						SELECT
								e.ID								AS CID,
								e.CuName							AS CNAME,
								@typeMSGNNAME						AS typeMSGNNAME,
								b.ID								AS IDCruns,
								c.CuName							AS CurrencyIDTo,
								1 / (@CurrencySalePrice * BuyPrice) AS SalePrice,
								1 / (@CurrencyBuyPrice * SalePrice) AS BuyPrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقوى'
										ELSE
										'أقل'
								END									AS CurrencyPower,

								CASE
										WHEN CurrencyPower = 1
											THEN
											@CurrencyBuyPrice * b.SalePrice
										ELSE
										@CurrencyBuyPrice / SalePrice
								END									AS CurrencyBuyPrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											@CurrencySalePrice * b.BuyPrice
										ELSE
										@CurrencySalePrice / BuyPrice
								END									AS CurrencySalePrice,
								0.00								AS BankSalePrice,
								0.00								AS BankBuyPrice,
								0,
								00									AS CurrencyBankBuyPrice,
								0.00								AS CurrencyBankSalePrice,
								1									AS Typesd
						FROM
								[dbo].[CurrencyPricesTb] AS a
							INNER JOIN
								CurrencyPriceDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> @CurrencyIDFrom
						SET @MSG = ''
					END
			END
	END
	IF @typeMSGN = 2
		BEGIN
		SELECT
				@typeMSGNNAME = 'عالمصرف'
		END
	BEGIN
		SELECT
				e.ID		  AS CID,
				e.CuName	  AS CNAME,
				@typeMSGNNAME AS typeMSGNNAME,
				b.ID		  AS IDCruns,
				c.CuName	  AS CurrencyIDTo,
				b.SalePrice,
				b.BuyPrice,
				CASE
						WHEN b.CurrencyPower = 0
							THEN
							'اقوى'
						ELSE
						'أقل'
				END			  AS CurrencyPower,
				0.00		  AS BankSalePrice,
				0.00		  AS BankBuyPrice,

				CASE
						WHEN CurrencyPower = 1
							THEN
							1 / b.SalePrice
						ELSE
						b.SalePrice
				END			  AS CurrencySalePrice,

				CASE
						WHEN CurrencyPower = 1
							THEN
							1 / b.BuyPrice
						ELSE
						b.BuyPrice
				END			  AS CurrencyBuyPrice,
				0.00		  AS CurrencyBankSalePrice,

				0.00		  AS CurrencyBankBuyPrice,
				2			  AS Typesd
		FROM
				[dbo].[Currency_settingForBancksRet] AS a
			INNER JOIN
				CurrencyPriceDetailsBancksTb AS b
					ON a.CurrencyFrom = b.CurrencyIDFrom
			INNER JOIN
				[dbo].[CurrencyMainTb] AS c
					ON b.CurrencyIDTo = c.ID
			INNER JOIN
				[dbo].[CurrencyMainTb] AS e
					ON @CurrencyIDFrom = e.ID
		WHERE
				a.Isactive = 1
				AND b.Isactive = 1
				AND c.Isactive = 1
				AND b.Banck_ID = @CurrencyIDFrom
		SET @MSG = 'لايوجد اسعار متوفرة في الوقت الحالي '
	END
	END
	IF @TypeID = 2
	BEGIN
	IF @typeMSGN = 1
		BEGIN
		SELECT
				@typeMSGNNAME = 'نقدي'
		END
	BEGIN
		IF @CurrencyIDFrom = 1
			BEGIN
				SELECT
						e.ID			AS CID,
						e.CuName		AS CNAME,
						@typeMSGNNAME   AS typeMSGNNAME,
						b.ID			AS IDCruns,
						c.CuName		AS CurrencyIDTo,
						b.SalePrice,
						b.BuyPrice,
						CASE
								WHEN b.CurrencyPower = 0
									THEN
									'اقوى'
								ELSE
								'أقل'
						END				AS CurrencyPower,
						b.BankSalePrice AS BankSalePrice,
						b.BankBuyPrice  AS BankBuyPrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.SalePrice
								ELSE
								SalePrice
						END				AS CurrencySalePrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.BuyPrice
								ELSE
								BuyPrice
						END				AS CurrencyBuyPrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.BankSalePrice
								ELSE
								BankSalePrice
						END				AS CurrencyBankSalePrice,

						CASE
								WHEN CurrencyPower = 1
									THEN
									1 / b.BankBuyPrice
								ELSE
								BankBuyPrice
						END				AS CurrencyBankBuyPrice,
						1				AS Typesd
				FROM
						[dbo].[CurrencyPricesOwnTb] AS a
					INNER JOIN
						CurrencyPriceOwnDetailsTb AS b
							ON a.CurrencyIDFrom = b.CurrencyIDFrom
					INNER JOIN
						[dbo].[CurrencyMainTb] AS c
							ON b.CurrencyIDTo = c.ID
					INNER JOIN
						[dbo].[CurrencyMainTb] AS e
							ON @CurrencyIDFrom = e.ID
				WHERE
						a.Isactive = 1
						AND b.Isactive = 1
						AND c.Isactive = 1
						AND a.CurrencyIDFrom = @CurrencyIDFrom				
				SELECT
						@CurrencyIDFromNme = a.CuName
				FROM
						CurrencyMainTb AS a
				WHERE
						a.ID = @CurrencyIDFrom
				SET @MSG = 'اسعار العملات النقد الاجنبي لدي شركة الرحالة الاولى' + ' ' + ' مقابل' + ' ' + @CurrencyIDFromNme + ' '
				+ STUFF((SELECT
							', ',
							CONVERT(NVARCHAR, ROW_NUMBER() OVER (ORDER BY CuName)) + c.CuName + ' '
							+ 'سعر الشراء : ' + CONVERT(NVARCHAR, BuyPrice) + ' ' + 'سعر البيع  : ' + CONVERT(NVARCHAR, SalePrice)
							+ ' ' + 'سعر التحويل ' + CONVERT(NVARCHAR, 0.00) + ' ' + 'سعر الشراء ' + CONVERT(NVARCHAR, 0.00)
					FROM
							[dbo].[CurrencyPricesOwnTb] AS a
						INNER JOIN
							CurrencyPriceOwnDetailsTb AS b
								ON a.CurrencyIDFrom = b.CurrencyIDFrom
						INNER JOIN
							[dbo].[CurrencyMainTb] AS c
								ON @CurrencyIDFrom = c.ID
					WHERE
							a.Isactive = 1
							AND b.Isactive = 1
							AND c.Isactive = 1
							AND a.CurrencyIDFrom = 1
					FOR XML PATH (''))
				, 1, 2, '')
				SELECT
						@MSG
			END
		IF @CurrencyIDFrom <> 1
			BEGIN			
				SELECT
						@CurrencyPower = a.CurrencyPower
				FROM
						CurrencyPriceOwnDetailsTb AS a
				WHERE
						a.CurrencyIDFrom = 1
						AND a.CurrencyIDTo = @CurrencyIDFrom
				IF @CurrencyPower = 1
					BEGIN
						SELECT
								@CurrencySalePrice = a.SalePrice,
								@CurrencyBuyPrice  = a.BuyPrice
						FROM
								CurrencyPriceOwnDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = @CurrencyIDFrom
						---//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								@typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								b.BuyPrice	  AS BuyPrice,
								b.SalePrice	  AS SalePrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقل'
										ELSE
										'اقوى'
								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											b.SalePrice
										ELSE
										1 / SalePrice
								END			  AS CurrencySalePrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											b.BuyPrice
										ELSE
										1 / BuyPrice
								END			  AS CurrencyBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								[dbo].[CurrencyPricesOwnTb] AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = @CurrencyIDFrom
						---////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						UNION
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								@typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE
										WHEN CurrencyPower = 0
											THEN
											b.SalePrice * @CurrencyBuyPrice
										ELSE
										@CurrencyBuyPrice / SalePrice
								END			  AS SalePrice,
								CASE
										WHEN CurrencyPower = 0
											THEN
											b.BuyPrice * @CurrencySalePrice
										ELSE
										@CurrencySalePrice / BuyPrice
								END			  AS BuyPrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقوى'
										ELSE
										'أقل'
								END			  AS CurrencyPower,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,

								CASE
										WHEN CurrencyPower = 0
											THEN
											@CurrencyBuyPrice * b.SalePrice
										ELSE
										@CurrencyBuyPrice / SalePrice
								END			  AS CurrencyBuyPrice,

								CASE
										WHEN CurrencyPower = 0
											THEN
											@CurrencySalePrice * b.BuyPrice
										ELSE
										@CurrencySalePrice / BuyPrice
								END			  AS CurrencySalePrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								[dbo].[CurrencyPricesOwnTb] AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> @CurrencyIDFrom
						SET @MSG = ''
					END
				IF @CurrencyPower = 0
					BEGIN
						SELECT
								@CurrencySalePrice = a.SalePrice,
								@CurrencyBuyPrice  = a.BuyPrice
						FROM
								CurrencyPriceDetailsTb AS a
						WHERE
								a.CurrencyIDFrom = 1
								AND a.CurrencyIDTo = @CurrencyIDFrom
						---//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						SELECT
								e.ID		  AS CID,
								e.CuName	  AS CNAME,
								@typeMSGNNAME AS typeMSGNNAME,
								b.ID		  AS IDCruns,
								c.CuName	  AS CurrencyIDTo,
								CASE
										WHEN CurrencyPower = 1
											THEN
											b.SalePrice
										ELSE
										1 / SalePrice
								END			  AS SalePrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											b.BuyPrice
										ELSE
										1 / BuyPrice
								END			  AS BuyPrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقل'
										ELSE
										'اقوى'
								END			  AS CurrencyPower,
								b.BuyPrice	  AS CurrencyBuyPrice,
								b.SalePrice	  AS CurrencySalePrice,
								0.00		  AS BankSalePrice,
								0.00		  AS BankBuyPrice,
								0,
								00			  AS CurrencyBankBuyPrice,
								0.00		  AS CurrencyBankSalePrice,
								1			  AS Typesd
						FROM
								[dbo].[CurrencyPricesOwnTb] AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDFrom = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo = @CurrencyIDFrom
						-- ---////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						UNION
						SELECT
								e.ID								AS CID,
								e.CuName							AS CNAME,
								@typeMSGNNAME						AS typeMSGNNAME,
								b.ID								AS IDCruns,
								c.CuName							AS CurrencyIDTo,
								1 / (@CurrencySalePrice * BuyPrice) AS SalePrice,
								1 / (@CurrencyBuyPrice * SalePrice) AS BuyPrice,
								CASE
										WHEN b.CurrencyPower = 0
											THEN
											'اقوى'
										ELSE
										'أقل'
								END									AS CurrencyPower,

								CASE
										WHEN CurrencyPower = 1
											THEN
											@CurrencyBuyPrice * b.SalePrice
										ELSE
										@CurrencyBuyPrice / SalePrice
								END									AS CurrencyBuyPrice,

								CASE
										WHEN CurrencyPower = 1
											THEN
											@CurrencySalePrice * b.BuyPrice
										ELSE
										@CurrencySalePrice / BuyPrice
								END									AS CurrencySalePrice,
								0.00								AS BankSalePrice,
								0.00								AS BankBuyPrice,
								0,
								00									AS CurrencyBankBuyPrice,
								0.00								AS CurrencyBankSalePrice,
								1									AS Typesd
						FROM
								[dbo].[CurrencyPricesOwnTb] AS a
							INNER JOIN
								CurrencyPriceOwnDetailsTb AS b
									ON a.CurrencyIDFrom = b.CurrencyIDFrom
							INNER JOIN
								[dbo].[CurrencyMainTb] AS c
									ON b.CurrencyIDTo = c.ID
							INNER JOIN
								[dbo].[CurrencyMainTb] AS e
									ON @CurrencyIDFrom = e.ID
						WHERE
								a.Isactive = 1
								AND b.Isactive = 1
								AND c.Isactive = 1
								AND a.CurrencyIDFrom = 1
								AND b.CurrencyIDTo <> @CurrencyIDFrom
						SET @MSG = ''
					END
			END
	END
	IF @typeMSGN = 2
		BEGIN
		SELECT
				@typeMSGNNAME = 'عالمصرف'
		END
	BEGIN
		SELECT
				e.ID		  AS CID,
				e.CuName	  AS CNAME,
				@typeMSGNNAME AS typeMSGNNAME,
				b.ID		  AS IDCruns,
				c.CuName	  AS CurrencyIDTo,
				b.SalePrice,
				b.BuyPrice,
				CASE
						WHEN b.CurrencyPower = 0
							THEN
							'اقوى'
						ELSE
						'أقل'
				END			  AS CurrencyPower,
				0.00		  AS BankSalePrice,
				0.00		  AS BankBuyPrice,

				CASE
						WHEN CurrencyPower = 1
							THEN
							1 / b.SalePrice
						ELSE
						b.SalePrice
				END			  AS CurrencySalePrice,

				CASE
						WHEN CurrencyPower = 1
							THEN
							1 / b.BuyPrice
						ELSE
						b.BuyPrice
				END			  AS CurrencyBuyPrice,
				0.00		  AS CurrencyBankSalePrice,

				0.00		  AS CurrencyBankBuyPrice,
				2			  AS Typesd
		FROM
				[dbo].[Currency_settingForBancksRet] AS a
			INNER JOIN
				CurrencyPriceDetailsBancksTb AS b
					ON a.CurrencyFrom = b.CurrencyIDFrom
			INNER JOIN
				[dbo].[CurrencyMainTb] AS c
					ON b.CurrencyIDTo = c.ID
			INNER JOIN
				[dbo].[CurrencyMainTb] AS e
					ON @CurrencyIDFrom = e.ID
		WHERE
				a.Isactive = 1
				AND b.Isactive = 1
				AND c.Isactive = 1
				AND b.Banck_ID = @CurrencyIDFrom
		SET @MSG = 'لايوجد اسعار متوفرة في الوقت الحالي '
	END
	END

