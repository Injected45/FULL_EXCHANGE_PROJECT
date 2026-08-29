Imports System.ComponentModel

Public Class RPTCashMovement2
    Private Sub RPTCashMovement2_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = Format(FRMCashMovement.D1.EditValue, "yyy-MM-dd")
        D2.Text = Format(FRMCashMovement.D2.EditValue, "yyy-MM-dd")
        XrLabel4.Text = FRMCashMovement.BranchID.Text
        XrLabel13.Text = FRMCashMovement.CurrencyID.Text
        XrLabel2.Text = GetUserName
    End Sub
End Class