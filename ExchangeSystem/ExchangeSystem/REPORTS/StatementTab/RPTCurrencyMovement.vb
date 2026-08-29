Imports System.ComponentModel

Public Class RPTCurrencyMovement
    Private Sub RPTCurrencyMovement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FrmCurrencyMovement.D1.EditValue
        D2.Text = FrmCurrencyMovement.D2.EditValue
        XrLabel4.Text = FrmCurrencyMovement.CurrencyID.Text
        XrLabel6.Text = FrmCurrencyMovement.BranchID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class