Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTPettyCash
    Private Sub RPTPettyCash_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel4.Text = My.Settings.Website
        XrLabel3.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel39.Text = GetUserName
        'XrLabel25.Text = Cur_Code(FRMPettyCash.CurrencyID.Text, FRMPettyCash.PettyCashVal.EditValue, False, False)
    End Sub


End Class