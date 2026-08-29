Imports System.ComponentModel

Public Class RPTMoneyCard
    Public Property IsActiveValue As Boolean

    Private Sub RPTMoneyCard_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint

        XrLabel27.Text = GetUserName
    End Sub

    'Private Sub RPTMoneyCard_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
    '    XrLabel21.Visible = Not IsActiveValue
    '    XrLabel22.Visible = Not IsActiveValue
    'End Sub
End Class