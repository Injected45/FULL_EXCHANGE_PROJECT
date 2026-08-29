Imports System.ComponentModel

Public Class RPTCustomerAccountStatement
    Private Sub RPTCustomerAccountStatement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint

        If FRMCustomerAccountStatement.TypeMov.SelectedIndex = 0 Then
                XrLabel1.Text = "كشف حركة حسابات العملاء"
            End If
            If FRMCustomerAccountStatement.TypeMov.SelectedIndex = 1 Then
                XrLabel1.Text = "كشف حركة حسابات الموظفين"
            End If
            If FRMCustomerAccountStatement.TypeMov.SelectedIndex = 2 Then
                XrLabel1.Text = "كشف حركة حسابات العملاء والموظفين"
            End If
            XrLabel4.Text = FRMCustomerAccountStatement.BranchID.Text
            XrLabel13.Text = FRMCustomerAccountStatement.CurrencyTo.Text
        OverAllTotal1.Text = Cur_Code(FRMCustomerAccountStatement.CurrencyTo.Text, FRMCustomerAccountStatement.OverAllTotal1.Text, True, "n2")
        XrLabel5.Text = Cur_Code1(FRMCustomerAccountStatement.CurrencyTo.Text)
        XrLabel6.Text = Cur_Code1(FRMCustomerAccountStatement.CurrencyTo.Text)
        If FRMCustomerAccountStatement.TypeMov.SelectedIndex = -1 Then
            XrLabel1.Text = "كشف حركة حسابات المستثمرين"
            XrLabel4.Text = FRMALLDebtorsMovment.BranchID.Text
            XrLabel13.Text = FRMALLDebtorsMovment.CurrencyTo.Text
            OverAllTotal1.Text = Cur_Code(FRMALLDebtorsMovment.CurrencyTo.Text, FRMALLDebtorsMovment.OverAllTotal1.Text, True, "n2")
            XrLabel5.Text = Cur_Code1(FRMALLDebtorsMovment.CurrencyTo.Text)
            XrLabel6.Text = Cur_Code1(FRMALLDebtorsMovment.CurrencyTo.Text)
            If FRMALLDebtorsMovment.OverAllTotal1.EditValue < 0 Then
                XrPictureBox12.Image = My.Resources.R_dollar
            End If
        End If

        XrLabel2.Text = GetUserName
    End Sub
End Class