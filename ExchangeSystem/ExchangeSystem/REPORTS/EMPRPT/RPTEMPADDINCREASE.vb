Imports System.ComponentModel
Imports System.IO

Public Class RPTEMPADDINCREASE
    Private Sub RPTEMPADDINCREASE_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel4.Text = My.Settings.Website
        XrLabel3.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel92.Text = Cur_Code(FRMEMPADDINCREASE.CURRENCYID.Text, FRMEMPADDINCREASE.DISVAL.EditValue, False, False)
        XrLabel15.Text = GetUserName
    End Sub
End Class