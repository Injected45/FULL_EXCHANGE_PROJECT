Imports System.ComponentModel

Public Class RPTCURRENCYPRICEDTTELSS
    Private Sub RPTCURRENCYPRICEDTTELSS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel8.Text = GetUserName
    End Sub
End Class