Imports System.ComponentModel
Imports System.IO

Public Class RPTEXTERNALFRM
    Private Sub RPTEXTERNALFRM_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel37.Text = GetUserName
        XrLabel2.Text = GetBranchName
        If FRMEXTERNALTRANS.ConfirmType = 3 Then
            XrLabel7.Text = "حوالة خارجية ملغاة"
            XrLabel8.Text = "CANCELED EXTERNAL TRANSFERS"
        End If
    End Sub
End Class