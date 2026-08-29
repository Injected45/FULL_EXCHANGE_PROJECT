Imports System.ComponentModel
Imports System.IO

Public Class RPTRecieveInternalEx2

    Private Sub RPTRecieveInternalEx2_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel5.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel25.Text = GetUserName
        'XrLabel29.Text.
        XrLabel29.Text = Cur_Code("دينار ليبي", FRMINTERNALTRANSFER.OverallVal.EditValue, False, False)
    End Sub
End Class