Imports System.ComponentModel
Imports System.IO

Public Class RPTSALARYCALCULATION
    Private Sub RPTSALARYCALCULATION_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        If FRMSALARYCALCULATION.GVRole.ActiveFilterString = Nothing Then
            XrLabel3.Text = "كشف احتساب مرتبات كل الموظفين"
        Else
            XrLabel3.Text = "كشف احتساب مرتبات الموظفين"
        End If
        MDATE.Text = Format(FRMSALARYCALCULATION.SALARYMONTH.EditValue, "MM").ToString
        YDATE.Text = Format(FRMSALARYCALCULATION.YDATE.EditValue, "yyyy").ToString
        XrLabel4.Text = GetUserName
        'OverallBounusTotal.Text = Format(FRMSALARYCALCULATION.OverallBounusTotal.EditValue, "N3")
        'OverallSalaryTotal.Text = Format(FRMSALARYCALCULATION.OverallSalaryTotal.EditValue, "N3")
        'OverallConstanceTotal.Text = Format(FRMSALARYCALCULATION.OverallConstanceTotal.EditValue, "N3")
        'OverallDiscount.Text = Format(FRMSALARYCALCULATION.OverallDiscount.EditValue, "N3")
        'OverallAdvancePaymentTotal.Text = Format(FRMSALARYCALCULATION.OverallAdvancePaymentTotal.EditValue, "N3")
        'OverallNetTotal.Text = Format(FRMSALARYCALCULATION.OverallNetTotal.EditValue, "N3")
    End Sub
End Class