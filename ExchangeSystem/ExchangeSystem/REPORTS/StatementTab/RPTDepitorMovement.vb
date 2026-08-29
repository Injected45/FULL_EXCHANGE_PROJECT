Imports System.ComponentModel

Public Class RPTDepitorMovement
    Private Sub RPTDepitorMovement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        'D1.Text = Format(FRMDebtorsMovment.D1.EditValue, "yyyy/MM/dd")
        'D2.Text = Format(FRMDebtorsMovment.D2.EditValue, "yyyy/MM/dd")
        OverAllTotal1.Text = Format(FRMDebtorsMovment.OverAllTotal1.EditValue, "N3")
        XrLabel4.Text = FRMDebtorsMovment.CUST.Text
        XrLabel6.Text = FRMDebtorsMovment.BranchID.Text
        XrLabel8.Text = GetUserName
        XrLabel21.Text = "كشف حساب – كود " + FRMDebtorsMovment.CustCode.Text
        If FRMDebtorsMovment.OverAllTotal1.BackColor = Color.Green Then
            XrPictureBox5.Image = My.Resources.G_dollar
        End If
        If FRMDebtorsMovment.PrefiweBalance < 0 Then
            XrPictureBox1.Image = My.Resources.R_dollar
        End If
    End Sub
End Class