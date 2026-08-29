Imports System.ComponentModel

Public Class RPTCurrencyMovements
    Private Sub RPTCurrencyMovements_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel3.Text = FRMCurrencyMovements.frm.TYPElock.Text
        XrLabel13.Text = FRMCurrencyMovements.frm.SafeID.Text
        XrLabel14.Text = FRMCurrencyMovements.frm.TextEdit5.Text
        XrLabel1.Text = FRMCurrencyMovements.frm.BanckID.Text
        D1.Text = FRMCurrencyMovements.frm.DateEdit1.Text
        D2.Text = FRMCurrencyMovements.frm.DateEdit2.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class