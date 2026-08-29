Public Class FrmShowCancelReason

    Sub LOADREASONS()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("TransCancelRequestTb_GetReasonID")
        If DT.Rows.Count > 0 Then
            ReasonID.Properties.DataSource = DT
            ReasonID.Properties.ValueMember = "ID"
            ReasonID.Properties.DisplayMember = "NewCause"
            ReasonID.Properties.ShowHeader = False
            ReasonID.Properties.PopulateColumns()
            ReasonID.Properties.Columns("ID").Visible = False
        End If

    End Sub
    Private Sub FrmShowCancelReason_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADREASONS()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FrmConfirmAgentCanceled.ReasonID = ReasonID.EditValue
        Me.Close()
    End Sub
End Class