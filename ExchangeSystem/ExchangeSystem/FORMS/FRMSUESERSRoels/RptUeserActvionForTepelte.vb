Imports System.ComponentModel

Public Class RptUeserActvionForTepelte
    Private Sub RptUeserActvionForTepelte_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel24.Text = GetUserName
    End Sub
End Class