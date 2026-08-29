Imports System.ComponentModel
Imports System.IO

Public Class RPTPETTYCASHTBLOADTOREPORT
    Private Sub RPTPETTYCASHTBLOADTOREPORT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel4.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
    End Sub
End Class