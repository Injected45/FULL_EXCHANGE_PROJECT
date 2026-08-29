Public Class EXternal_ExValSahreByHand
    Private Sub EXternal_ExValSahreByHand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExValShare.EditValue = 0.000
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        FRMEXTERNALTRANS.HandelExAVal = ExValShare.EditValue
        Me.Close()
    End Sub
End Class