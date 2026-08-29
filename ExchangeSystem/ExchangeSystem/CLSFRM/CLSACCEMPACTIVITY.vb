Imports System.Data.SqlClient

Public Class CLSACCEMPACTIVITY
    Public Function SEARCH_ACCEMPAC(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccEmpActivityTb_SearchByCode", PRM)
        Return DT
    End Function
    Public Function DELETEALL_ACCEMPAC(ISID As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1)
        PRM(0).Value = ISID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccEmpActivityTb_DeleteAll", PRM)
        Return DT
    End Function
    Public Function LOAD_CUREENCY() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CURRENCYTB_LoadDataIntoLookUpEdit")
        Return DT
    End Function
    Public Function LOAD_COBRANCH() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("COBRANCHTB_LoadDataIntoLookUpEdit")
        Return DT
    End Function
    Public Function LOAD_SAFE() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SAFETB_LoadDataIntoLookUpEdit")
        Return DT
    End Function
    Public Function LOAD_DRIVER() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("DRIVERTB_LoadDataIntoLookUpEdit")
        Return DT
    End Function
    '111
    Public Sub INSERTTB_ACCEMPACTIVITY(Code As String, SafeID As Integer, Debit As Double, Credit As Double, InsertDate As Date, Description As String, ISID As String, IsActive As Boolean, TypeID As Integer,
                                       OperationTypeID As Integer, IsConfirmed As Boolean, AccBranchID As Integer, MovementType As String, CurrencyID As Integer)
        Dim PRM(13) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(2) = New SqlParameter("@Debit", SqlDbType.Float) With {.Value = Debit}
        PRM(3) = New SqlParameter("@Credit", SqlDbType.Float) With {.Value = Credit}
        PRM(4) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(5) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = ISID}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        PRM(9) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        PRM(10) = New SqlParameter("@IsConfirmed", SqlDbType.Bit) With {.Value = IsConfirmed}
        PRM(11) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
        PRM(12) = New SqlParameter("@MovementType", SqlDbType.NVarChar, 50) With {.Value = MovementType}
        PRM(13) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        RUN_EXUTE_PRO("AccEmpActivityTb_Insert", PRM)
    End Sub
    Public Sub UPDATE_ACCEMPACTIVITY(Code As String, SafeID As Integer, Debit As Double, Credit As Double, InsertDate As Date, Description As String, ISID As String, IsActive As Boolean, TypeID As Integer, OperationTypeID As Integer,
                                     IsConfirmed As Boolean, AccBranchID As Integer, MovementType As String, CurrencyID As Integer)
        Dim PRM(13) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(2) = New SqlParameter("@Debit", SqlDbType.Float) With {.Value = Debit}
        PRM(3) = New SqlParameter("@Credit", SqlDbType.Float) With {.Value = Credit}
        PRM(4) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(5) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = ISID}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        PRM(9) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        PRM(10) = New SqlParameter("@IsConfirmed", SqlDbType.Bit) With {.Value = IsConfirmed}
        PRM(11) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
        PRM(12) = New SqlParameter("@MovementType", SqlDbType.NVarChar,50) With {.Value = MovementType}
        PRM(13) = New SqlParameter("@CurrencyID", SqlDbType.int) With {.Value = CurrencyID}
        RUN_EXUTE_PRO("AccEmpActivityTb_Update", PRM)
    End Sub
    Public Sub DELETE_ACCEMPACTIVITY(ISID As String, IsActive As Boolean, TypeID As Integer)
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(1) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(2) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}

        RUN_EXUTE_PRO("AccEmpActivityTb_Delete", PRM)
    End Sub

    Public Sub UPDATECONFIROM_ACCEMPACTIVITY(ISID As String, IsActive As Boolean, IsConfirmed As Boolean)
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(1) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(2) = New SqlParameter("@IsConfirmed", SqlDbType.Bit) With {.Value = IsConfirmed}

        RUN_EXUTE_PRO("AccEmpActivityTb_UpdateConfirm", PRM)
    End Sub
End Class
