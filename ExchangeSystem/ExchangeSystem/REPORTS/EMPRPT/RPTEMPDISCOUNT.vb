Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTEMPDISCOUNT
    Private Sub RPTEMPDISCOUNT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel6.Text = My.Settings.Website
        XrLabel7.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox1.Image = Image.FromStream(ms)
        End Using
        XrLabel72.Text = Cur_Code(FRMEMPDISCOUNT.CURRENCYID.Text, FRMEMPDISCOUNT.DISVAL.EditValue, False, False)
        XrLabel15.Text = GetUserName
    End Sub
End Class