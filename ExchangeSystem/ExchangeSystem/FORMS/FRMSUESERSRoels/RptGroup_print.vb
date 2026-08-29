Imports System.ComponentModel

Public Class RptGroup_print
    Private Sub RptGroup_print_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel24.Text = GetUserName
    End Sub
End Class