Imports System.ComponentModel

Public Class RPTShowSafeMovement
    Private Sub RPTSelectByEmSafe_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FrmShowSafeMovement.D1.Text
        D2.Text = FrmShowSafeMovement.D2.Text
        OverAllTotal.Text = Cur_Code(FrmShowSafeMovement.CurrencyID.Text, FrmShowSafeMovement.OverAllTotal.EditValue, True, "n2")
        XrLabel25.Text = Cur_Code(FrmShowSafeMovement.CurrencyID.Text, FrmShowSafeMovement.OverAllDebit.EditValue, True, "n2")
        XrLabel4.Text = Cur_Code(FrmShowSafeMovement.CurrencyID.Text, FrmShowSafeMovement.OverAllCredit.EditValue, True, "n2")
        XrLabel16.Text = FrmShowSafeMovement.CurrencyID.Text
        XrLabel2.Text = FrmShowSafeMovement.BranchID.Text
        XrLabel6.Text = FrmShowSafeMovement.SafeID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class