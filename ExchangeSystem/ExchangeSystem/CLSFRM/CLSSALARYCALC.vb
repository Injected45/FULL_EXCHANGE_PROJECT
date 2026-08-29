Imports System.Data.SqlClient

Public Class CLSSALARYCALC
    Public Sub AccSafeActivity_insert(SafeID As ULong, Debit As Double, InsertDate As Date, BranchID As Integer, CurrencyID As Integer, EmpID As Integer, MonthVal As String, YearVal As String, CodeID As String)
        Dim prm(8) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
        prm(2) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        prm(5) = New SqlParameter("@EmpID", SqlDbType.Int) With {.Value = EmpID}
        prm(6) = New SqlParameter("@MonthVal", SqlDbType.NVarChar, 10) With {.Value = MonthVal}
        prm(7) = New SqlParameter("@YearVal", SqlDbType.NVarChar, 50) With {.Value = YearVal}
        prm(8) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = CodeID}
        RUN_EXUTE_PRO("AccSafeActivityTb_InsertSalaryCalc", prm)
    End Sub
    Public Sub SalaryCalculationTb_insert1(INSERTDATE As Date, EMPID As Integer, BranchID As Integer, SalaryVal As Decimal, ConstanceVal As Decimal, BONUSVAL As Decimal,
                                          DiscountsVal As Decimal, AdvancePaymentDisc As Decimal, SALARYTOTAL As Decimal, SALARYMONTH As Int32, SALARYEAR As Integer, CodeID As String,
                                          SafeID As Integer, IsIndivdual As Boolean, SecondSalaryVal As Decimal, SecondConstanceVal As Decimal, SecondBONUSVAL As Decimal, SecondAdvancePaymentDisc As Decimal,
                                                           SecondSALARYTOTAL As Decimal, SecondDiscountsVal As Decimal, SalaryCalc As Integer, Notes As String)
        Dim prm(21) As SqlParameter
        prm(0) = New SqlParameter("@INSERTDATE", SqlDbType.Date) With {.Value = INSERTDATE}
        prm(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(3) = New SqlParameter("@SalaryVal", SqlDbType.Decimal) With {.Value = SalaryVal}
        prm(4) = New SqlParameter("@ConstanceVal", SqlDbType.Decimal) With {.Value = ConstanceVal}
        prm(5) = New SqlParameter("@BONUSVAL", SqlDbType.Decimal) With {.Value = BONUSVAL}
        prm(6) = New SqlParameter("@DiscountsVal", SqlDbType.Decimal) With {.Value = DiscountsVal}
        prm(7) = New SqlParameter("@AdvancePaymentDisc", SqlDbType.Decimal) With {.Value = AdvancePaymentDisc}
        prm(8) = New SqlParameter("@SALARYTOTAL", SqlDbType.Decimal) With {.Value = SALARYTOTAL}
        prm(9) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = SALARYMONTH}
        prm(10) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = SALARYEAR}
        prm(11) = New SqlParameter("@CodeID", SqlDbType.NVarChar, -1) With {.Value = CodeID}
        prm(12) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(13) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = IsIndivdual}
        prm(14) = New SqlParameter("@SecondSalaryVal", SqlDbType.Decimal) With {.Value = SecondSalaryVal}
        prm(15) = New SqlParameter("@SecondConstanceVal", SqlDbType.Decimal) With {.Value = SecondConstanceVal}
        prm(16) = New SqlParameter("@SecondBONUSVAL", SqlDbType.Decimal) With {.Value = SecondBONUSVAL}
        prm(17) = New SqlParameter("@SecondAdvancePaymentDisc", SqlDbType.Decimal) With {.Value = SecondAdvancePaymentDisc}
        prm(18) = New SqlParameter("@SecondSALARYTOTAL", SqlDbType.Decimal) With {.Value = SecondSALARYTOTAL}
        prm(19) = New SqlParameter("@SecondDiscountsVal", SqlDbType.Decimal) With {.Value = SecondDiscountsVal}
        prm(20) = New SqlParameter("@SalaryCalc", SqlDbType.Int) With {.Value = SalaryCalc}
        prm(21) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        RUN_EXUTE_PRO("SalaryCalculationTb_Insert", prm)
    End Sub
    Public Sub SalaryCalculationTb_insert2(SafeID As Integer, SalaryCalc As Integer, SALARYMONTH As Int32, SALARYEAR As Integer, SalaryCalcType As Integer)
        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@SalaryCalc", SqlDbType.Int) With {.Value = SalaryCalc}
        prm(2) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = SALARYMONTH}
        prm(3) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = SALARYEAR}
        prm(4) = New SqlParameter("@SalaryCalcType", SqlDbType.Int) With {.Value = SalaryCalcType}
        RUN_EXUTE_PRO("SalaryCalculationTb_Insert2", prm)
    End Sub
    Public Sub SalaryCalculationTb_IndivdualInsert(INSERTDATE As Date, EMPID As Integer, BranchID As Integer, SalaryVal As Decimal, ConstanceVal As Decimal,
                                                   BONUSVAL As Decimal, DiscountsVal As Decimal, AdvancePaymentDisc As Decimal, SALARYTOTAL As Decimal,
                                                   SALARYMONTH As Int32, SALARYEAR As Integer, CodeID As String, SafeID As Integer, IsIndivdual As Boolean,
                                                   SecondSalaryVal As Decimal, SecondConstanceVal As Decimal, SecondBONUSVAL As Decimal,
                                                   SecondAdvancePaymentDisc As Decimal, SecondSALARYTOTAL As Decimal, SecondDiscountsVal As Decimal,
                                                   SalaryCalc As Integer, IsTotal As Boolean, DDAte As Integer, RestValue As Decimal)
        Dim prm(23) As SqlParameter
        prm(0) = New SqlParameter("@INSERTDATE", SqlDbType.Date) With {.Value = INSERTDATE}
        prm(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(3) = New SqlParameter("@SalaryVal", SqlDbType.Decimal) With {.Value = SalaryVal}
        prm(4) = New SqlParameter("@ConstanceVal", SqlDbType.Decimal) With {.Value = ConstanceVal}
        prm(5) = New SqlParameter("@BONUSVAL", SqlDbType.Decimal) With {.Value = BONUSVAL}
        prm(6) = New SqlParameter("@DiscountsVal", SqlDbType.Decimal) With {.Value = DiscountsVal}
        prm(7) = New SqlParameter("@AdvancePaymentDisc", SqlDbType.Decimal) With {.Value = AdvancePaymentDisc}
        prm(8) = New SqlParameter("@SALARYTOTAL", SqlDbType.Decimal) With {.Value = SALARYTOTAL}
        prm(9) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = SALARYMONTH}
        prm(10) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = SALARYEAR}
        prm(11) = New SqlParameter("@CodeID", SqlDbType.NVarChar, -1) With {.Value = CodeID}
        prm(12) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(13) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = IsIndivdual}
        prm(14) = New SqlParameter("@SecondSalaryVal", SqlDbType.Decimal) With {.Value = SecondSalaryVal}
        prm(15) = New SqlParameter("@SecondConstanceVal", SqlDbType.Decimal) With {.Value = SecondConstanceVal}
        prm(16) = New SqlParameter("@SecondBONUSVAL", SqlDbType.Decimal) With {.Value = SecondBONUSVAL}
        prm(17) = New SqlParameter("@SecondAdvancePaymentDisc", SqlDbType.Decimal) With {.Value = SecondAdvancePaymentDisc}
        prm(18) = New SqlParameter("@SecondSALARYTOTAL", SqlDbType.Decimal) With {.Value = SecondSALARYTOTAL}
        prm(19) = New SqlParameter("@SecondDiscountsVal", SqlDbType.Decimal) With {.Value = SecondDiscountsVal}
        prm(20) = New SqlParameter("@SalaryCalc", SqlDbType.Int) With {.Value = SalaryCalc}
        prm(21) = New SqlParameter("@IsTotal", SqlDbType.Bit) With {.Value = IsTotal}
        prm(22) = New SqlParameter("@DDATE", SqlDbType.Int) With {.Value = DDAte}
        prm(23) = New SqlParameter("@RestValue", SqlDbType.Decimal) With {.Value = RestValue}
        RUN_EXUTE_PRO("SalaryCalculationTb_IndivdualInsert", prm)
    End Sub

    '' عملية الححفظ في قاعدة بيانات المقاولات
    Public Sub SalaryCalculationTb_insert_CONDB(INSERTDATE As Date, EMPID As Integer, BranchID As Integer, SalaryVal As Decimal, ConstanceVal As Decimal, BONUSVAL As Decimal,
                                          DiscountsVal As Decimal, AdvancePaymentDisc As Decimal, SALARYTOTAL As Decimal, SALARYMONTH As Int32, SALARYEAR As Integer, CodeID As String,
                                          SafeID As Integer, IsIndivdual As Boolean, SecondSalaryVal As Decimal, SecondConstanceVal As Decimal, SecondBONUSVAL As Decimal, SecondAdvancePaymentDisc As Decimal,
                                                           SecondSALARYTOTAL As Decimal, SecondDiscountsVal As Decimal, SalaryCalc As Integer, Notes As String)
        Dim prm(21) As SqlParameter
        prm(0) = New SqlParameter("@INSERTDATE", SqlDbType.Date) With {.Value = INSERTDATE}
        prm(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(3) = New SqlParameter("@SalaryVal", SqlDbType.Decimal) With {.Value = SalaryVal}
        prm(4) = New SqlParameter("@ConstanceVal", SqlDbType.Decimal) With {.Value = ConstanceVal}
        prm(5) = New SqlParameter("@BONUSVAL", SqlDbType.Decimal) With {.Value = BONUSVAL}
        prm(6) = New SqlParameter("@DiscountsVal", SqlDbType.Decimal) With {.Value = DiscountsVal}
        prm(7) = New SqlParameter("@AdvancePaymentDisc", SqlDbType.Decimal) With {.Value = AdvancePaymentDisc}
        prm(8) = New SqlParameter("@SALARYTOTAL", SqlDbType.Decimal) With {.Value = SALARYTOTAL}
        prm(9) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = SALARYMONTH}
        prm(10) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = SALARYEAR}
        prm(11) = New SqlParameter("@CodeID", SqlDbType.NVarChar, -1) With {.Value = CodeID}
        prm(12) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(13) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = IsIndivdual}
        prm(14) = New SqlParameter("@SecondSalaryVal", SqlDbType.Decimal) With {.Value = SecondSalaryVal}
        prm(15) = New SqlParameter("@SecondConstanceVal", SqlDbType.Decimal) With {.Value = SecondConstanceVal}
        prm(16) = New SqlParameter("@SecondBONUSVAL", SqlDbType.Decimal) With {.Value = SecondBONUSVAL}
        prm(17) = New SqlParameter("@SecondAdvancePaymentDisc", SqlDbType.Decimal) With {.Value = SecondAdvancePaymentDisc}
        prm(18) = New SqlParameter("@SecondSALARYTOTAL", SqlDbType.Decimal) With {.Value = SecondSALARYTOTAL}
        prm(19) = New SqlParameter("@SecondDiscountsVal", SqlDbType.Decimal) With {.Value = SecondDiscountsVal}
        prm(20) = New SqlParameter("@SalaryCalc", SqlDbType.Int) With {.Value = SalaryCalc}
        prm(21) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        RUN_EXUTE_PRO("CONDB_SalaryCalculationTb_Insert", prm)
    End Sub
End Class
