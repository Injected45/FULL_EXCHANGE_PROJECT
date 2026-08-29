
CREATE PROCEDURE dbo.MultiAcountEditTB_Insert
  (
    @Code        NVARCHAR(MAX),
    @BranchID    INT,
    @BranchIDTo  INT,
    @SafeID      BIGINT,
    @CurrencyID  INT,
    @IsUpdate    BIT,
    @Type        dbo.MultiAcountEditTB_GCRole_seteing1 READONLY,
    @MSGSTatues  INT                                   OUTPUT,
    @MsgBox      NVARCHAR(MAX)                         OUTPUT,
    @MovmentType NVARCHAR(MAX),
    @OverAllVal  DECIMAL(18, 3),
    @ValType     INT,
    @FirstAccID  BIGINT
  )
AS
  BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
      BEGIN TRANSACTION;

      DECLARE @IDCode         INT,
              @SumVal         DECIMAL(18, 3),
              @MainBranchID   INT,
              @MainCurrentAcc BIGINT,
              @TotalDebit     DECIMAL(18, 3) = 0,
              @TotalCredit    DECIMAL(18, 3) = 0,
              @MultiAccAccID  BIGINT;

      -- الحصول على معلومات الفرع الرئيسي
      SELECT
              @MainBranchID   = ID,
              @MainCurrentAcc = ISNULL(CurrentAccID, 0)
      FROM
              CoBranch
      WHERE
              IsMain = 1;
      SELECT
              @MultiAccAccID = AccID
      FROM
              AccountsTb a
      WHERE
              a.AccParent = 1030401
              AND a.BranchID = @BranchIDTo

      IF @MainBranchID IS NULL
        BEGIN
          SET @MSGSTatues = 0;
          SET @MsgBox = N'لم يتم تحديد فرع رئيسي في النظام.';
          ROLLBACK;
          RETURN;
        END

      -- توليد كود فريد
      SELECT
              @IDCode = ISNULL(MAX(IDCode), 0) + 1
      FROM
              MultiAcountEditTB;

      -- التحقق من صحة مجموع الطرف الثاني
      SELECT
              @SumVal = CASE
                               WHEN @ValType = 0
                                 THEN
                                 ISNULL(SUM(Credit), 0)
                               ELSE
                               ISNULL(SUM(Debit), 0)
                       END
      FROM
              @Type;

      IF @SumVal <> @OverAllVal
        BEGIN
          SET @MSGSTatues = 0;
          SET @MsgBox = N'مجموع الطرف الثاني لا يساوي القيمة الإجمالية';
          ROLLBACK;
          RETURN;
        END

      -- تسجيل الحركة الرئيسية
      INSERT INTO MultiAcountEditTB
            (
              Code,
              InsertDate,
              BranchID,
              SafeID,
              CurrencyID,
              IsActive,
              IDCode,
              MovmentType
            )
      VALUES
              (
                @Code, GETDATE(), @BranchID, @SafeID, @CurrencyID, 1, @IDCode, @MovmentType
              );

      -- ============================================
      -- جدول يحتوي على جميع الحركات (الطرف الأول + الطرف الثاني)
      -- ============================================
      DECLARE @AllEntries TABLE
        (
          RowID      INT IDENTITY (1, 1),
          BranchID   INT,
          AccID      BIGINT,
          Debit      DECIMAL(18, 3),
          Credit     DECIMAL(18, 3),
          IsFirst    BIT,
          AccIDToRef BIGINT  -- الحساب المقابل للعرض
        );

      -- إضافة الطرف الأول
      DECLARE @FirstDebit  DECIMAL(18, 3) = 0,
              @FirstCredit DECIMAL(18, 3) = 0;
      IF @ValType = 1
        BEGIN
          SET @FirstDebit = 0;
          SET @FirstCredit = @OverAllVal;
        END
      ELSE
        BEGIN
          SET @FirstDebit = @OverAllVal;
          SET @FirstCredit = 0;
        END

      INSERT INTO @AllEntries
            (
              BranchID,
              AccID,
              Debit,
              Credit,
              IsFirst,
              AccIDToRef
            )
      VALUES
              (
                @BranchID, @FirstAccID, @FirstDebit, @FirstCredit, 1, @MultiAccAccID
              );

      -- إضافة الطرف الثاني
      INSERT INTO @AllEntries
            (
              BranchID,
              AccID,
              Debit,
              Credit,
              IsFirst,
              AccIDToRef
            )
          SELECT
                  a.BranchIDTo,
                  a.AccIDTo,
                  CASE
                          WHEN @ValType = 1
                            THEN
                            a.Debit
                          ELSE
                          0
                  END,
                  CASE
                          WHEN @ValType = 0
                            THEN
                            a.Credit
                          ELSE
                          0
                  END,
                  0,
                  CASE
                          WHEN a.BranchIDTo = @BranchID
                            THEN
                            @FirstAccID
                          ELSE
                          @MainCurrentAcc
                  END
          FROM
                  @Type a;

      -- ============================================
      -- تسجيل جميع الحركات الأساسية (بدون تسوية جارية)
      -- ============================================
      INSERT INTO ExSyAccounts2026.dbo.AccSafeActivityTb
            (
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
              AccIDTo
            )
          SELECT
                  @SafeID,
                  Debit,
                  Credit,
                  GETDATE(),
                  @Code,
                  1,
                  55,
                  91,
                  BranchID,
                  AccID,
                  AccIDToRef
          FROM
                  @AllEntries;

      -- ============================================
      -- حساب صافي كل فرع غير رئيسي
      -- ============================================
      DECLARE @BranchBalances TABLE
        (
          BranchID  INT,
          NetAmount DECIMAL(18, 3)  -- >0: مدين للرئيسي، <0: دائن للرئيسي
        );

      INSERT INTO @BranchBalances
            (
              BranchID,
              NetAmount
            )
          SELECT
                  BranchID,
                  SUM(Debit - Credit) AS NetAmount
          FROM
                  @AllEntries
          WHERE
                  BranchID <> @MainBranchID
          GROUP BY
                  BranchID;

      -- ============================================
      -- إنشاء حركات التسوية بين كل فرع غير رئيسي والرئيسي
      -- ============================================
      DECLARE @b      INT;
      DECLARE @bCount INT;
      SET @b = 1;
      SET @bCount = (SELECT COUNT(*) FROM @BranchBalances);
      DECLARE @BalBranchID INT,
              @Net         DECIMAL(18, 3);

      WHILE @b <= @bCount
        BEGIN
          SELECT
                  @BalBranchID = BranchID,
                  @Net         = NetAmount
          FROM
                  (   SELECT
                              ROW_NUMBER() OVER (ORDER BY BranchID) AS rn,
                              BranchID,
                              NetAmount
                      FROM
                              @BranchBalances) t
          WHERE
                  rn = @b;

          DECLARE @CurrentAcc BIGINT;
          SET @CurrentAcc = ISNULL((SELECT CurrentAccID FROM CoBranch WHERE ID = @BalBranchID) , 0);

          IF @Net > 0
            BEGIN
              -- الفرع مدين للرئيسي
              -- 1. في فرعه: مدين على جاري الرئيسي
              INSERT INTO ExSyAccounts2026.dbo.AccSafeActivityTb
                    (
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
                      AccIDTo
                    )
              VALUES
                      (
                        @SafeID, 0, @Net, GETDATE(), @Code, 1, 55, 91, @BalBranchID, @MainCurrentAcc, @CurrentAcc
                      );

              -- 2. في الرئيسي: دائن على جاري الفرع
              INSERT INTO ExSyAccounts2026.dbo.AccSafeActivityTb
                    (
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
                      AccIDTo
                    )
              VALUES
                      (
                        @SafeID, @Net, 0, GETDATE(), @Code, 1, 55, 91, @MainBranchID, @CurrentAcc, @MainCurrentAcc
                      );
            END
          ELSE
          IF @Net < 0
            BEGIN
              -- الفرع دائن للرئيسي
              SET @Net = -@Net;
              -- 1. في فرعه: دائن على جاري الرئيسي
              INSERT INTO ExSyAccounts2026.dbo.AccSafeActivityTb
                    (
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
                      AccIDTo
                    )
              VALUES
                      (
                        @SafeID, @Net, 0, GETDATE(), @Code, 1, 55, 91, @BalBranchID, @MainCurrentAcc, @CurrentAcc
                      );

              -- 2. في الرئيسي: مدين على جاري الفرع
              INSERT INTO ExSyAccounts2026.dbo.AccSafeActivityTb
                    (
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
                      AccIDTo
                    )
              VALUES
                      (
                        @SafeID, 0, @Net, GETDATE(), @Code, 1, 55, 91, @MainBranchID, @CurrentAcc, @MainCurrentAcc
                      );
            END

          SET @b = @b + 1;
        END

      -- ============================================
      -- تسجيل تفاصيل القيد
      -- ============================================
      INSERT INTO MultiAcountEditDetailsTB
            (
              MISID,
              AccID,
              Debit,
              Credit,
              Notes,
              IsActive,
              InsertDate,
              Branch,
              AccIDTo,
              BranchIDTo
            )
          SELECT
                  @Code,
                  @FirstAccID,
                  CASE
                          WHEN @ValType = 1
                            THEN
                            a.Debit
                          ELSE
                          0
                  END,
                  CASE
                          WHEN @ValType = 0
                            THEN
                            a.Credit
                          ELSE
                          0
                  END,
                  ISNULL(a.NotesDe, N'تفاصيل القيد'),
                  1,
                  GETDATE(),
                  a.Branch,
                  a.AccIDTo,
                  a.BranchIDTo
          FROM
                  @Type a;

      -- ============================================
      -- التحقق من التوازن المحاسبي
      -- ============================================
      SELECT
              @TotalDebit  = SUM(ISNULL(Debit, 0)),
              @TotalCredit = SUM(ISNULL(Credit, 0))
      FROM
              ExSyAccounts2026.dbo.AccSafeActivityTb
      WHERE
              ISID = @Code;

      IF ABS(@TotalDebit - @TotalCredit) > 0.01
        BEGIN
          SET @MSGSTatues = 0;
          SET @MsgBox = N'خطأ في التوازن المحاسبي. المدين: '
          + CAST(@TotalDebit AS NVARCHAR(20))
          + N'، الدائن: '
          + CAST(@TotalCredit AS NVARCHAR(20));
          ROLLBACK;
          RETURN;
        END

      COMMIT TRANSACTION;
      SET @MSGSTatues = 1;
      SET @MsgBox = N'تم حفظ القيد متعدد الحسابات بنجاح.';

    END TRY
    BEGIN CATCH
      IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

      DECLARE @ErrorMessage NVARCHAR(4000);
      DECLARE @ErrorLine INT;
      SET @ErrorMessage = ERROR_MESSAGE();
      SET @ErrorLine = ERROR_LINE();

      SET @MSGSTatues = 0;
      SET @MsgBox = N'خطأ في السطر ' + CAST(@ErrorLine AS NVARCHAR(10))
      + N': ' + @ErrorMessage;

      INSERT INTO ErrorLog
            (
              ErrorMessage,
              ErrorDate,
              ProcedureName
            )
      VALUES
              (
                @ErrorMessage, GETDATE(), 'MultiAcountEditTB_Insert'
              );

      RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
  END

