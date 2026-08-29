Imports System.ComponentModel
Imports System.IO

Public Class RPTANOTHEREXPENS
    Private Sub RPTANOTHEREXPENS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel9.Text = GetUserName
        XrLabel21.Text = My.Settings.ARName
        XrLabel24.Text = My.Settings.Website
        XrLabel27.Text = My.Settings.FaceBook
        XrLabel13.Text = My.Settings.Mobile1
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        If FRMANOTHEREXPENS.IsAseet = True Then
            XrLabel7.Text = "شراء أصل"
            XrLabel51.Text = ":الأصـل"
            XrLabel5.Text = "Buy Aseet"
        End If
    End Sub
End Class