Imports System.Data.SqlClient

Public Class CLSEXPENSES
    Public Sub ExpensesTb_Insert(InsertDate As Date, Code As String, ExName As String, BranchID As Integer, AccID As ULong, IsActive As Boolean, IsUpdate As Boolean, TypeEx As Int32)
        Dim prm(7) As SqlParameter
        prm(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        prm(2) = New SqlParameter("@ExName", SqlDbType.NVarChar, (150)) With {.Value = ExName}
        prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(4) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID}
        prm(5) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        prm(6) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        prm(7) = New SqlParameter("@TypeEx", SqlDbType.TinyInt) With {.Value = TypeEx}
        RUN_EXUTE_PRO("ExpensesTb_Insert", prm)
    End Sub
    Public Sub ExpensesTb_Update(Code As String, ExName As String, BranchID As Integer, IsActive As Boolean)
        Dim prm(3) As SqlParameter
        prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        prm(1) = New SqlParameter("@ExName", SqlDbType.NVarChar, (150)) With {.Value = ExName}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(3) = New SqlParameter("@IsActive", SqlDbType.BigInt) With {.Value = IsActive}
        RUN_EXUTE_PRO("ExpensesTb_Update", prm)
    End Sub
    Public Function CHECK_Expenses(ByVal ExName As String, BranchID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@ExName", SqlDbType.NVarChar, 250)
        PRM(0).Value = ExName.Trim
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(1).Value = BranchID
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("ExpensesTb_CHECKEXNAME", PRM)
        End If
        Return DT
    End Function
    Public Function ExpensesTb_SelectAll(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExpensesTb_SelectAll", PRM)
        Return DT
    End Function
    Public Sub UPDATETB_ExpensesAccountData(AccCode As ULong, AccName As String, AccParent As Decimal, BranchID As Integer, IDcode As ULong, AccID As ULong)
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = AccCode}
        PRM(1) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1) With {.Value = AccName}
        PRM(2) = New SqlParameter("@AccParent", SqlDbType.Decimal) With {.Value = AccParent}
        PRM(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(4) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDcode}
        PRM(5) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = AccID}
        RUN_EXUTE_PRO("ACCOUNTSTB_UPDATECHANGEDBRANCH", PRM)
    End Sub
End Class
