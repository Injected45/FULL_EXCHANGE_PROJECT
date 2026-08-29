Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraReports.UI

Public Class RPTINDIVDUALSALARYCALC

    Private Sub RPTINDIVDUALSALARYCALC_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel40.Text = My.Settings.ARName
        XrLabel5.Text = My.Settings.Website
        XrLabel6.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox17.Image = Image.FromStream(ms)
        End Using
        XrLabel15.Text = GetUserName

        XrLabel17.Text = Cur_Code("دينار ليبي", FRMINDIVDUALSALARYCALC.NetEMPVal, True, False)
        If FRMINDIVDUALSALARYCALC.GCRole.DataSource.Rows(0)("الصافي") < 0 Then
            XrPictureBox3.Image = My.Resources.R_dollar
        End If
        GETCASHEMPCUST(FRMINDIVDUALSALARYCALC.EMPID.EditValue)
        XrLabel19.Text = Cur_Code("دينار ليبي", EMPCUSTCASHVAL, True, False)
        If FRMINDIVDUALSALARYCALC.IsTotal.SelectedIndex = 0 Then
            XrPictureBox6.Visible = False
            XrLabel14.Visible = False
            XrLabel2.Visible = False
        End If
    End Sub


End Class