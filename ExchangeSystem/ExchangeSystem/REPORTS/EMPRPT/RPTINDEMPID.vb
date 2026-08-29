Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTINDEMPID
    Dim arl As New arabicconverter
    Private Sub RPTINDEMPID_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel10.Text = My.Settings.Website
        XrLabel8.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel36.Text = arl.numtolit(FrmIndividualSalaryEMP.OverAllTotal.EditValue, 3, "دينار ليبي", "درهم", True, True)
        XrLabel15.Text = GetUserName
    End Sub
End Class