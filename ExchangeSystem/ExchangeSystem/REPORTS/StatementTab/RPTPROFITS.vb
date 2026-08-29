Imports System.ComponentModel

Public Class RPTPROFITS
    Private Sub RPTPROFITS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FRMPROFITS.D1.EditValue
        D2.Text = FRMPROFITS.D2.EditValue
        XrLabel41.Text = FRMPROFITS.BranchID.Text
        XrLabel2.Text = GetUserName
    End Sub
End Class