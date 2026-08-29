Imports System.Data.SqlClient

Public Class CLSCURRENCYTRANSFERMAINSAFES
    Public Function LOAD_WITHDRAWALTODVG() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CurrencyActivityTb_SEARCH")
        Return DT
    End Function
    Public Function SHOW_CURRENCYACTTB_DATA(CCode As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CCode", SqlDbType.NVarChar, -1) With {.Value = CCode.ToString}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CurrencyActivityTb_SelectALL", PRM)
        Return DT
    End Function
    Public Sub INSERTTB_CURRENCYTB(InserDate As Date, SafeCurID As Integer, SDebit As Double, SCredit As Double, CurrencyID As Integer, BranchID As Integer, SafeID As Integer, Notes As String, CCode As String, SafeCurTo As Integer)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@InserDate", SqlDbType.Date) With {.Value = InserDate}
        PRM(1) = New SqlParameter("@SafeCurID", SqlDbType.Int) With {.Value = SafeCurID}
        PRM(2) = New SqlParameter("@SDebit", SqlDbType.Decimal) With {.Value = SDebit}
        PRM(3) = New SqlParameter("@SCredit", SqlDbType.Decimal) With {.Value = SCredit}
        PRM(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(5) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(6) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(7) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(8) = New SqlParameter("@CCode", SqlDbType.NVarChar, -1) With {.Value = CCode}
        PRM(9) = New SqlParameter("@SafeCurTo", SqlDbType.Int) With {.Value = SafeCurTo}
        RUN_EXUTE_PRO("CurrencyActivityTb_Insert", PRM)
    End Sub
    Public Sub update_CURRENCYTB(InserDate As Date, SafeCurID As Integer, SDebit As Double, SCredit As Double, CurrencyID As Integer, BranchID As Integer, SafeID As Integer, Notes As String, CCode As String, SafeCurTo As Integer)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@InserDate", SqlDbType.Date) With {.Value = InserDate}
        PRM(1) = New SqlParameter("@SafeCurID", SqlDbType.Int) With {.Value = SafeCurID}
        PRM(2) = New SqlParameter("@SDebit", SqlDbType.Decimal) With {.Value = SDebit}
        PRM(3) = New SqlParameter("@SCredit", SqlDbType.Decimal) With {.Value = SCredit}
        PRM(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(5) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(6) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(7) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(8) = New SqlParameter("@CCode", SqlDbType.NVarChar, -1) With {.Value = CCode}
        PRM(9) = New SqlParameter("@SafeCurTo", SqlDbType.Int) With {.Value = SafeCurTo}
        RUN_EXUTE_PRO("CurrencyActivityTb_Update", PRM)
    End Sub
    Public Sub UPDATE_CURRENCYTB_ACCEMPACTIVITY(Code As String, SafeID As Integer, Debit As Double, Credit As Double, InsertDate As Date, Description As String, ISID As String, IsActive As Boolean, TypeID As Integer, OperationTypeID As Integer)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(2) = New SqlParameter("@Debit", SqlDbType.Float) With {.Value = Debit}
        PRM(3) = New SqlParameter("@Credit", SqlDbType.Float) With {.Value = Credit}
        PRM(4) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(5) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = ISID}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        PRM(9) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        RUN_EXUTE_PRO("AccEmpActivityTb_Update", PRM)
    End Sub
    Public Sub DELETE_WITHDRAWAL(WDCode As String)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@WDCode", SqlDbType.NVarChar, -1) With {.Value = WDCode.ToString}
        RUN_EXUTE_PRO("Withdrawal_Delete", PRM)
    End Sub
End Class
