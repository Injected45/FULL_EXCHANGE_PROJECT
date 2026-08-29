Imports System.Data.SqlClient

Public Class CLSAGENTACTIVITY
    Public Sub INSERTTB_AGENTACTIVITY(InsertDate As Date, ISID As String, OvearAll As Double, ExVall As Double, BranchID As Integer, ISHandallEX As Boolean, HandallExVal As Decimal, SafeID As Integer, CurrencyID As Integer, Notes As String, BranchIDTo As Integer)
        Dim PRM(10) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(2) = New SqlParameter("@OvearAll", SqlDbType.Decimal) With {.Value = OvearAll}
        PRM(3) = New SqlParameter("@ExVall", SqlDbType.Decimal) With {.Value = ExVall}
        PRM(4) = New SqlParameter("@ISHandallEX", SqlDbType.Bit) With {.Value = ISHandallEX}
        PRM(5) = New SqlParameter("@HandallExVal", SqlDbType.Decimal) With {.Value = HandallExVal}
        PRM(6) = New SqlParameter("@BranchIDFrom", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(8) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(10) = New SqlParameter("@BranchIDTo", SqlDbType.Int) With {.Value = BranchIDTo}
        RUN_EXUTE_PRO("AgentActivityTb_Insert", PRM)
    End Sub
    Public Sub Update_AGENTACTIVITY(InsertDate As Date, ISID As String, Debit As Double, Credit As Double, BranchID As Integer, OperationTypeID As Integer, TypeID As Integer, SafeID As Integer, CurrencyID As Integer, Notes As String, BranchIDTo As Integer)
        Dim PRM(10) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(2) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
        PRM(3) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
        PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(5) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        PRM(6) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        PRM(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(8) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(10) = New SqlParameter("@BranchIDTo", SqlDbType.Int) With {.Value = BranchIDTo}
        RUN_EXUTE_PRO("AgentActivityTb_Update", PRM)
    End Sub
    Public Sub DELETE_AGENTACTIVITY(ISID As String, TypeID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        RUN_EXUTE_PRO("AgentActivityTb_Delete", PRM)
    End Sub
End Class
