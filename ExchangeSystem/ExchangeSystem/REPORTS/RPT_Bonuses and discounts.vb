Imports System.ComponentModel

Public Class RPT_Bonuses_and_discounts
    Private Sub RPT_Bonuses_and_discounts_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel5.Text = GetUserName
    End Sub
End Class