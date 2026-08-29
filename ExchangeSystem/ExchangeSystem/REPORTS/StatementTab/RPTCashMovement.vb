Imports System.ComponentModel

Public Class RPTCashMovement
    Private Sub RPTCashMovement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        If FRMCashMovement.MovmentType.SelectedIndex = 0 Then
            XrLabel5.Text = "كشف حركة سندات الصرف"
        End If
        If FRMCashMovement.MovmentType.SelectedIndex = 1 Then
            XrLabel5.Text = "كشف حركة سندات القبض"
            XrPictureBox4.Image = My.Resources.G_dollar
        End If
        If FRMCashMovement.MovmentType.SelectedIndex = 2 Then
            XrLabel5.Text = "كشف حركة المصروفات العمومية"
            'tableCell3.Text = "المستخدم"
            tableCell3.Text = "طبيعة الحركة"
        End If

        D1.Text = Format(FRMCashMovement.D1.EditValue, "yyy-MM-dd")
        D2.Text = Format(FRMCashMovement.D2.EditValue, "yyy-MM-dd")
        XrLabel6.Text = FRMCashMovement.BranchID.Text
        XrLabel13.Text = FRMCashMovement.CurrencyID.Text
        XrLabel25.Text = Cur_Code(FRMCashMovement.CurrencyID.Text, FRMCashMovement.OverallVal.Text, True, "n2")

        XrLabel11.Text = GetUserName
    End Sub

End Class