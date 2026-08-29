Imports System.ComponentModel

Public Class RPTBalnceSheet_Detials
    Private Sub RPTBalnceSheet_Detials_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel8.Text = GetUserName
    End Sub
End Class