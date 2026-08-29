Imports System.Data.SqlClient

Public Class CLSDAILYCLOSE
    Public Sub INSERTTB_DailyCloseTb(InsertDate As Date, Code As String, CurrnecyID As Integer, SafeIDFrom As Integer, SafeIDTo As Integer, OverallDebit As Double, OverallCredit As Double, OverallRBVAL As Double,
                                     OverallDBVAL As Double, RBVALCanceled As Double, RBVALFromBranches As Double, OverallMainVal As Double, RBVALFromCanceledBranches As Double,
                                     BenefitNetTotal As Double, SafeCID As Integer, BranchID As Integer, TransType As Integer)
        Dim PRM(16) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(2) = New SqlParameter("@CurrnecyID", SqlDbType.Int) With {.Value = CurrnecyID}
        PRM(3) = New SqlParameter("@SafeIDFrom", SqlDbType.Int) With {.Value = SafeIDFrom}
        PRM(4) = New SqlParameter("@SafeIDTo", SqlDbType.Int) With {.Value = SafeIDTo}
        PRM(5) = New SqlParameter("@OverallDebit", SqlDbType.Decimal) With {.Value = OverallDebit}
        PRM(6) = New SqlParameter("@OverallCredit", SqlDbType.Decimal) With {.Value = OverallCredit}
        PRM(7) = New SqlParameter("@OverallRBVAL", SqlDbType.Decimal) With {.Value = OverallRBVAL}
        PRM(8) = New SqlParameter("@OverallDBVAL", SqlDbType.Decimal) With {.Value = OverallDBVAL}
        PRM(9) = New SqlParameter("@RBVALCanceled", SqlDbType.Decimal) With {.Value = RBVALCanceled}
        PRM(10) = New SqlParameter("@RBVALFromBranches", SqlDbType.Decimal) With {.Value = RBVALFromBranches}
        PRM(11) = New SqlParameter("@OverallMainVal", SqlDbType.Decimal) With {.Value = OverallMainVal}
        PRM(12) = New SqlParameter("@RBVALFromCanceledBranches", SqlDbType.Decimal) With {.Value = RBVALFromCanceledBranches}
        PRM(13) = New SqlParameter("@BenefitNetTotal", SqlDbType.Decimal) With {.Value = BenefitNetTotal}
        PRM(14) = New SqlParameter("@SafeCID", SqlDbType.Int) With {.Value = SafeCID}
        PRM(15) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(16) = New SqlParameter("@TransType", SqlDbType.TinyInt) With {.Value = TransType}
        RUN_EXUTE_PRO("DailyCloseTb_Insert", PRM)
    End Sub
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
End Class
