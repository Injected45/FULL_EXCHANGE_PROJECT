Imports System.ComponentModel

Public Class RPT_serrlemnt_with_agent
    Private Sub RPT_serrlemnt_with_agent_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel8.Text = GetUserName
    End Sub
End Class