Imports System.ComponentModel

Public Class RPTMainSafeBalanc2
    Private Sub RPTMainSafeBalanc2_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        BranchID.Text = FrmMainSafeBalance.BranchID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class