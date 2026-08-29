CREATE PROCEDURE [dbo].[PCSettlementTB_InsertAccDetails]
	(
		@Code		   [NVARCHAR](MAX),
		@InsertDate	   [DATE],
		@BranchID	   [INT],
		@SafeID		   [BIGINT],
		@CurrencyID	   [INT],
		@SettlementVal [DECIMAL](12, 3),
		@ExpensVal	   [DECIMAL](12, 3),
		@AccIDEX	   [BIGINT],
		@NotesDe	   NVARCHAR(MAX),
		@AccIDSafeID   [BIGINT], --حساب خزنة الموظف
		@IsUpdate	   [BIT],
		@ISID		   NVARCHAR(MAX),
		@PCVal		   [DECIMAL](12, 3),
		@EXID		   [BIGINT]
	)
AS
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRANSACTION;
	DECLARE @NetVal		  AS DECIMAL(15, 3),
			@MovementType AS NVARCHAR(100);
	SELECT
			@NetVal = @PCVal - @SettlementVal;
	IF @IsUpdate = 0
		BEGIN
		INSERT INTO dbo.PCSettlementDetailsTB
				(
					PCISID,
					SPCISID,
					ExpensVal,
					AccIDEX,
					EXID,
					Notes,
					IsActive
				)
		VALUES
				(
					@ISID, @Code, @ExpensVal, @AccIDEX, @EXID, @NotesDe, 1
				);
		END
	BEGIN
		--الحسابات
		SELECT
				@MovementType = ' تسوية عهدة موظف رقم ' + CHAR(13) + @ISID;
		IF @SettlementVal < @PCVal
			BEGIN
				--حساب المصروفات كمدين
				INSERT INTO dbo.ExSyAccounts_AccSafeActivityTb
						(
							--ID - column value is auto-generated
							SafeID,
							Debit,
							Credit,
							InsertDate,
							ISID,
							IsActive,
							TypeID,
							OperationTypeID,
							AccBranchID,
							AccIDFrom,
							AccIDTo,
							IsConfirmed,
							IsCanceled,
							MovementType,
							CurrencyID,
							DailyClosed,
							SafeIDDailyClose,
							Note
						)
				VALUES
						(
							-- ID - bigint
							@SafeID, -- SafeID - int
							@ExpensVal, -- Debit - decimal
							0.000, -- Credit - decimal
							@InsertDate, -- InsertDate - date
							@Code, -- ISID - nvarchar
							1, -- IsActive - bit
							14, -- TypeID - int
							44, -- OperationTypeID - int
							@BranchID, -- AccBranchID - int
							@AccIDEX, -- AccIDFrom - int
							@AccIDSafeID, -- AccIDTo - int
							1, -- IsConfirmed - bit
							0, -- IsCanceled - int
							@MovementType, -- MovementType - nvarchar
							@CurrencyID, -- CurrencyID - int
							0, -- DailyClosed - bit
							0, -- SafeIDDailyClose - int,
							@NotesDe
						);
			END;
		IF @SettlementVal = @PCVal
			BEGIN
				--حساب المصروفات كمدين
				INSERT INTO dbo.ExSyAccounts_AccSafeActivityTb
						(
							--ID - column value is auto-generated
							SafeID,
							Debit,
							Credit,
							InsertDate,
							ISID,
							IsActive,
							TypeID,
							OperationTypeID,
							AccBranchID,
							AccIDFrom,
							AccIDTo,
							IsConfirmed,
							IsCanceled,
							MovementType,
							CurrencyID,
							DailyClosed,
							SafeIDDailyClose,
							Note
						)
				VALUES
						(
							-- ID - bigint
							@SafeID, -- SafeID - int
							@ExpensVal, -- Debit - decimal
							0.000, -- Credit - decimal
							@InsertDate, -- InsertDate - date
							@Code, -- ISID - nvarchar
							1, -- IsActive - bit
							14, -- TypeID - int
							44, -- OperationTypeID - int
							@BranchID, -- AccBranchID - int
							@AccIDEX, -- AccIDFrom - int
							@AccIDSafeID, -- AccIDTo - int
							1, -- IsConfirmed - bit
							0, -- IsCanceled - int
							@MovementType, -- MovementType - nvarchar
							@CurrencyID, -- CurrencyID - int
							0, -- DailyClosed - bit
							0, -- SafeIDDailyClose - int,
							@NotesDe
						);
			END;
		IF @SettlementVal > @PCVal
			BEGIN
				--حساب المصروفات كمدين
				INSERT INTO dbo.ExSyAccounts_AccSafeActivityTb
						(
							--ID - column value is auto-generated
							SafeID,
							Debit,
							Credit,
							InsertDate,
							ISID,
							IsActive,
							TypeID,
							OperationTypeID,
							AccBranchID,
							AccIDFrom,
							AccIDTo,
							IsConfirmed,
							IsCanceled,
							MovementType,
							CurrencyID,
							DailyClosed,
							SafeIDDailyClose,
							Note
						)
				VALUES
						(
							-- ID - bigint
							@SafeID, -- SafeID - int
							@ExpensVal, -- Debit - decimal
							0.000, -- Credit - decimal
							@InsertDate, -- InsertDate - date
							@Code, -- ISID - nvarchar
							1, -- IsActive - bit
							14, -- TypeID - int
							44, -- OperationTypeID - int
							@BranchID, -- AccBranchID - int
							@AccIDEX, -- AccIDFrom - int
							@AccIDSafeID, -- AccIDTo - int
							1, -- IsConfirmed - bit
							0, -- IsCanceled - int
							@MovementType, -- MovementType - nvarchar
							@CurrencyID, -- CurrencyID - int
							0, -- DailyClosed - bit
							0, -- SafeIDDailyClose - int,
							@NotesDe
						);
			END;
	END;
	IF @IsUpdate = 1
		BEGIN
			UPDATE
					dbo.PCSettlementDetailsTB
			SET
					dbo.PCSettlementDetailsTB.IsActive = 0
			WHERE
				dbo.PCSettlementDetailsTB.SPCISID = @Code;
			IF @SettlementVal < @PCVal
				BEGIN
					--حساب المصروفات كدائن
					INSERT INTO dbo.ExSyAccounts_AccSafeActivityTb
							(
								--ID - column value is auto-generated
								SafeID,
								Debit,
								Credit,
								InsertDate,
								ISID,
								IsActive,
								TypeID,
								OperationTypeID,
								AccBranchID,
								AccIDFrom,
								AccIDTo,
								IsConfirmed,
								IsCanceled,
								MovementType,
								CurrencyID,
								DailyClosed,
								SafeIDDailyClose,
								Note
							)
					VALUES
							(
								-- ID - bigint
								@SafeID, -- SafeID - int
								0.000, -- Debit - decimal
								@ExpensVal, -- Credit - decimal
								@InsertDate, -- InsertDate - date
								@Code, -- ISID - nvarchar
								1, -- IsActive - bit
								14, -- TypeID - int
								44, -- OperationTypeID - int
								@BranchID, -- AccBranchID - int
								@AccIDEX, -- AccIDFrom - int
								@AccIDSafeID, -- AccIDTo - int
								1, -- IsConfirmed - bit
								0, -- IsCanceled - int
								@MovementType, -- MovementType - nvarchar
								@CurrencyID, -- CurrencyID - int
								0, -- DailyClosed - bit
								0, -- SafeIDDailyClose - int,
								@NotesDe
							);
				END;
			IF @SettlementVal = @PCVal
				BEGIN
					--حساب المصروفات كدائن
					INSERT INTO dbo.ExSyAccounts_AccSafeActivityTb
							(
								--ID - column value is auto-generated
								SafeID,
								Debit,
								Credit,
								InsertDate,
								ISID,
								IsActive,
								TypeID,
								OperationTypeID,
								AccBranchID,
								AccIDFrom,
								AccIDTo,
								IsConfirmed,
								IsCanceled,
								MovementType,
								CurrencyID,
								DailyClosed,
								SafeIDDailyClose,
								Note
							)
					VALUES
							(
								-- ID - bigint
								@SafeID, -- SafeID - int
								0.000, -- Debit - decimal
								@ExpensVal, -- Credit - decimal
								@InsertDate, -- InsertDate - date
								@Code, -- ISID - nvarchar
								1, -- IsActive - bit
								14, -- TypeID - int
								44, -- OperationTypeID - int
								@BranchID, -- AccBranchID - int
								@AccIDEX, -- AccIDFrom - int
								@AccIDSafeID, -- AccIDTo - int
								1, -- IsConfirmed - bit
								0, -- IsCanceled - int
								@MovementType, -- MovementType - nvarchar
								@CurrencyID, -- CurrencyID - int
								0, -- DailyClosed - bit
								0, -- SafeIDDailyClose - int,
								@NotesDe
							);
				END;
			IF @SettlementVal > @PCVal
				BEGIN
					--حساب المصروفات كدائن
					INSERT INTO dbo.ExSyAccounts_AccSafeActivityTb
							(
								--ID - column value is auto-generated
								SafeID,
								Debit,
								Credit,
								InsertDate,
								ISID,
								IsActive,
								TypeID,
								OperationTypeID,
								AccBranchID,
								AccIDFrom,
								AccIDTo,
								IsConfirmed,
								IsCanceled,
								MovementType,
								CurrencyID,
								DailyClosed,
								SafeIDDailyClose,
								Note
							)
					VALUES
							(
								-- ID - bigint
								@SafeID, -- SafeID - int
								0.000, -- Debit - decimal
								@ExpensVal, -- Credit - decimal
								@InsertDate, -- InsertDate - date
								@Code, -- ISID - nvarchar
								1, -- IsActive - bit
								14, -- TypeID - int
								44, -- OperationTypeID - int
								@BranchID, -- AccBranchID - int
								@AccIDEX, -- AccIDFrom - int
								@AccIDSafeID, -- AccIDTo - int
								1, -- IsConfirmed - bit
								0, -- IsCanceled - int
								@MovementType, -- MovementType - nvarchar
								@CurrencyID, -- CurrencyID - int
								0, -- DailyClosed - bit
								0, -- SafeIDDailyClose - int,
								@NotesDe
							);
				END;
		END;

	COMMIT;

