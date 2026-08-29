Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTProPettyCashSettlement
    Private Sub RPTProPettyCashSettlement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel2.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel24.Text = GetUserName
    End Sub
End Class