Imports System.ComponentModel
Imports System.IO

Public Class RPTLOADSALARIES
    Private Sub RPTLOADSALARIES_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        ApplyLocalization("en-US")
        D1.Text = FRMLOADSALARIES.D1.EditValue
        D2.Text = FRMLOADSALARIES.D2.EditValue
        OverAllTotal.Text = Cur_Code(FRMLOADSALARIES.CurrencyTo.Text, FRMLOADSALARIES.OverAllTotal.EditValue, True, "n2")
        OverAllTotal1.Text = Cur_Code(FRMLOADSALARIES.CurrencyTo.Text, FRMLOADSALARIES.OvarAllPrint, True, "n2")
        XrLabel25.Text = Cur_Code(FRMLOADSALARIES.CurrencyTo.Text, FRMLOADSALARIES.OverAllDebit.EditValue, True, "n2")
        XrLabel20.Text = Cur_Code(FRMLOADSALARIES.CurrencyTo.Text, FRMLOADSALARIES.OverAllCredit.EditValue, True, "n2")
        If FRMLOADSALARIES.OverAllTotal.EditValue < 0 Then
            XrPictureBox13.Image = My.Resources.R_dollar
        End If
        If FRMLOADSALARIES.OvarAllPrint < 0 Then
            XrPictureBox12.Image = My.Resources.R_dollar
        End If
        If FRMLOADSALARIES.TabbedControlGroup1.SelectedTabPageIndex = 0 Then
            XrLabel21.Text = "كشف حركة حساب الموظف"
        End If
        If FRMLOADSALARIES.TabbedControlGroup1.SelectedTabPageIndex = 1 Then
            XrLabel21.Text = "كشف حركة المعاملات النقدية للموظف"
        End If
        If FRMLOADSALARIES.TabbedControlGroup1.SelectedTabPageIndex = 2 Then
            XrLabel21.Text = "كشف حركة المعاملات المصرفية للموظف"
        End If
        XrLabel7.Text = FRMLOADSALARIES.EMPID.Text
        XrLabel6.Text = FRMLOADSALARIES.BranchID.Text
        If Application.OpenForms().OfType(Of FRMEmpStetment).Any Then
            D1.Text = FRMEmpStetment.D1.EditValue
            D2.Text = FRMEmpStetment.D2.EditValue
            OverAllTotal.Text = Cur_Code("دينار ليبي", FRMEmpStetment.OverAllTotal.EditValue, True, "n2")
            OverAllTotal1.Text = Cur_Code("دينار ليبي", FRMEmpStetment.OvarAllPrint, True, "n2")
            XrLabel25.Text = Cur_Code("دينار ليبي", FRMEmpStetment.OverAllDebit.EditValue, True, "n2")
            XrLabel20.Text = Cur_Code("دينار ليبي", FRMEmpStetment.OverAllCredit.EditValue, True, "n2")
            XrLabel21.Text = "كشف حركة حساب الموظف"
            XrLabel7.Text = FRMEmpStetment.EMPID.Text
            XrLabel6.Text = FRMEmpStetment.BranchID.Text
        End If

        XrLabel8.Text = GetUserName

    End Sub
End Class