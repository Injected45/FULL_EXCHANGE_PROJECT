Imports System.ComponentModel
Imports System.IO

Public Class RPTCORRECTINDIVDUALSALARYCALC
    Private Sub RPTCORRECTINDIVDUALSALARYCALC_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel40.Text = My.Settings.ARName
        XrLabel5.Text = My.Settings.Website
        XrLabel6.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox17.Image = Image.FromStream(ms)
        End Using
        XrLabel15.Text = GetUserName
    End Sub
End Class