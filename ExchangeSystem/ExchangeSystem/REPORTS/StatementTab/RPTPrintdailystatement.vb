Imports System.ComponentModel

Public Class RPTPrintdailystatement
    Private Sub RPTPrintdailystatement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel3.Text = GetUserName
        XrLabel1.Text = FrmShowSafeMovement.SafeID.Text
    End Sub
End Class