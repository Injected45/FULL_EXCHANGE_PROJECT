Imports System.ComponentModel

Public Class XtraReport3
    Private Sub XtraReport3_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FrmShowSafeMovement.D1.EditValue
        D2.Text = FrmShowSafeMovement.D2.EditValue
        OverAllTotal.Text = Cur_Code(FrmShowSafeMovement.CurrencyID.Text, FrmShowSafeMovement.OverAllTotal.EditValue, True, "n2")
        XrLabel25.Text = Cur_Code(FrmShowSafeMovement.CurrencyID.Text, FrmShowSafeMovement.OverAllDebit.EditValue, True, "n2")
        XrLabel4.Text = Cur_Code(FrmShowSafeMovement.CurrencyID.Text, FrmShowSafeMovement.OverAllCredit.EditValue, True, "n2")
        XrLabel16.Text = FrmShowSafeMovement.BankServicesID.Text
        XrLabel6.Text = FrmShowSafeMovement.SafeID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class