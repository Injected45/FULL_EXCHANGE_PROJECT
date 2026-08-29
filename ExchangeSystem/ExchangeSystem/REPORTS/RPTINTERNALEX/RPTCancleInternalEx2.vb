Imports System.ComponentModel
Imports System.IO

Public Class RPTCancleInternalEx2
    Private Sub RPTCancleInternalEx2_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel5.Text = My.Settings.ARName
        XrLabel32.Text = My.Settings.Website
        XrLabel48.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel25.Text = GetUserName
        XrLabel29.Text = Cur_Code("دينار ليبي", FRMRETRUNINTERNALEX.OvarAllVall1, False, False)
    End Sub
End Class