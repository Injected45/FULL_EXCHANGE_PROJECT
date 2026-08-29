Imports System.ComponentModel

Public Class RPTASSMember_Statment
    Private Sub RPTASSMember_Statment_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel8.Text = GetUserName
    End Sub
End Class