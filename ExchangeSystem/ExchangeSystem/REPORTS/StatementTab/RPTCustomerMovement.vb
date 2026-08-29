Imports System.ComponentModel

Public Class RPTCustomerMovement
    Private Sub RPTCustomerMovement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FrmCustomerMovement.D1.Text
        D2.Text = FrmCustomerMovement.D2.Text
        XrLabel4.Text = FrmCustomerMovement.CUST.Text
        XrLabel6.Text = FrmCustomerMovement.BranchID.Text
        'XrLabel8.Text = FrmCustomerMovement.CustCode.Text
        'XrLabel13.Text = FrmCustomerMovement.PreBalance
        XrLabel8.Text = GetUserName
        If FrmCustomerMovement.TabbedControlGroup1.SelectedTabPageIndex = 0 Then
            XrLabel21.Text = "كشف حساب – كود " + FrmCustomerMovement.CustCode.Text
        End If
        If FrmCustomerMovement.TabbedControlGroup1.SelectedTabPageIndex = 1 Then
            XrLabel21.Text = "كشف حركة المعاملات النقدية للعميل"
        End If
        If FrmCustomerMovement.TabbedControlGroup1.SelectedTabPageIndex = 2 Then
            XrLabel21.Text = "كشف حركة المعاملات المصرفية للعميل"
        End If
        If FrmCustomerMovement.OverAllTotal.BackColor = Color.Red Then
            XrPictureBox12.Image = My.Resources.R_dollar
        End If
    End Sub
End Class