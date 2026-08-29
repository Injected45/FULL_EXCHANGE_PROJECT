

Imports System.ComponentModel

Public Class RPTMainSafeBalance
    Private Sub RPTMainSafeBalance_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint

        If FrmMainSafeBalance.SType.SelectedIndex = 0 Then
            XrLabel21.Text = "كشف أرصدة الخزائن الرئيسية"
        Else
            XrLabel21.Text = "كشف أرصدة الخزائن"
        End If
        BranchID.Text = FrmMainSafeBalance.BranchID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class