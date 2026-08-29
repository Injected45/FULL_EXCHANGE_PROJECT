Imports System.ComponentModel
Imports System.IO

Public Class RPTPROEXPORTITEM
    Private Sub RPTPROEXPORTITEM_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel24.Text = My.Settings.Website
        XrLabel27.Text = My.Settings.FaceBook
        XrLabel13.Text = My.Settings.Mobile1
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel9.Text = GetUserName
    End Sub
End Class