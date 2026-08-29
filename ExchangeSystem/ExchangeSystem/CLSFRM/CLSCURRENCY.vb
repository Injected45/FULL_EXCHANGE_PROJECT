Imports System.Data.SqlClient

Public Class CLSCURRENCY
    Public Function SERACH_CURRENCY(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CURRENCY_SEARCHBYCODE", PRM)
        Return DT
    End Function
    Public Function CHECK_BRANCH_NAME(ByVal CurrencyName As String, BranchID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@CurrencyName", SqlDbType.NVarChar, -1) With {.Value = CurrencyName}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("CURRENCYTB_SEARCH_NAME", PRM)
        End If
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB__CURRENCY(Code As String, CurrencyName As String, PartName As String, PartValue As Integer, ExchangeRate As Decimal, EqualValue As Decimal, IsLocal As Boolean, IsActive As Boolean,
                                  IsDefault As Boolean, BranchID As Integer)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@CurrencyName", SqlDbType.NVarChar, -1) With {.Value = CurrencyName}
        PRM(2) = New SqlParameter("@PartName", SqlDbType.NVarChar, -1) With {.Value = PartName}
        PRM(3) = New SqlParameter("@PartValue", SqlDbType.Int) With {.Value = PartValue}
        PRM(4) = New SqlParameter("@ExchangeRate", SqlDbType.Decimal) With {.Precision = 12, .Scale = 3, .Value = ExchangeRate}
        PRM(5) = New SqlParameter("@EqualValue", SqlDbType.Decimal) With {.Precision = 12, .Scale = 3, .Value = EqualValue}
        PRM(6) = New SqlParameter("@IsLocal", SqlDbType.Bit) With {.Value = IsLocal}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@IsDefault", SqlDbType.Bit) With {.Value = IsDefault}
        PRM(9) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        RUN_EXUTE_PRO("CurrencyTb_Insert", PRM)

    End Sub

    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_CURRENCY(Code As String, CurrencyName As String, PartName As String, PartValue As Integer, ExchangeRate As Double, EqualValue As Double, IsLocal As Boolean, IsActive As Boolean,
                                 IsDefault As Boolean, BranchID As Integer)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@CurrencyName", SqlDbType.NVarChar, -1) With {.Value = CurrencyName}
        PRM(2) = New SqlParameter("@PartName", SqlDbType.NVarChar, -1) With {.Value = PartName}
        PRM(3) = New SqlParameter("@PartValue", SqlDbType.Int) With {.Value = PartValue}
        PRM(4) = New SqlParameter("@ExchangeRate", SqlDbType.Decimal) With {.Precision = 12, .Scale = 3, .Value = ExchangeRate}
        PRM(5) = New SqlParameter("@EqualValue", SqlDbType.Decimal) With {.Precision = 12, .Scale = 3, .Value = EqualValue}
        PRM(6) = New SqlParameter("@IsLocal", SqlDbType.Bit) With {.Value = IsLocal}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@IsDefault", SqlDbType.Bit) With {.Value = IsDefault}
        PRM(9) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        RUN_EXUTE_PRO("CURRENCYTB_UPDATE", PRM)
    End Sub
End Class
