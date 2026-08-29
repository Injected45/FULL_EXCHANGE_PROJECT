Imports System.ComponentModel

Public Class XtraReport_sell
    Private Sub XtraReport_sell_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel46.Text = GetUserName
    End Sub
End Class