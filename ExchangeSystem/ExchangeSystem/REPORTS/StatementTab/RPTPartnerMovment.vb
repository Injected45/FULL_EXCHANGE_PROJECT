Imports System.ComponentModel

Public Class RPTPartnerMovment
    Private Sub RPTPartnerMovment_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel8.Text = GetUserName
    End Sub
End Class