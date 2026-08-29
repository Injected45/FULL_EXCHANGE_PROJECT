Imports System.ComponentModel
Imports System.IO

Public Class RPTIndividualSalaryEMP2



    Private Sub RPTIndividualSalaryEMP_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel2.Text = My.Settings.Website
        XrLabel5.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel16.Text = FRMSALARYCALCULATION.SALARYMONTH.Text
        YDATE.Text = FRMSALARYCALCULATION.YDATE.Text
        'XrLabel9.Text = FrmIndividualSalaryEMP.BranchID.Text
        'EMPID.Text = FrmIndividualSalaryEMP.EMPID.Text
        'XrLabel1.Text = FrmIndividualSalaryEMP.EMPID.Text
        'OverAllTotal.Text = Format(FrmIndividualSalaryEMP.OverAllTotal.EditValue, "N3")
        XrLabel36.Text = Cur_Code("دينار ليبي", FRMSALARYCALCULATION.EMPSalaary, False, False)
        XrLabel15.Text = GetUserName
    End Sub
End Class