Public Class FRMCANCELTOAGENT
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMCONFIRMISSUED.ConfirmCancelRequest = True
        Me.Close()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        FRMCONFIRMISSUED.ConfirmCancelRequest = False
        FRMCONFIRMISSUED.GVROLE.Columns("BranchDeliveredID").OptionsColumn.AllowEdit = True
        FRMCONFIRMISSUED.GVROLE.Columns("BranchDeliveredID").OptionsColumn.ReadOnly = False
        'FRMCONFIRMISSUED.DVGFormat()
        Me.Close()
    End Sub
End Class