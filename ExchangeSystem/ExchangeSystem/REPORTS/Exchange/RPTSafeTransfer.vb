Imports System.ComponentModel
Imports System.IO

Public Class RPTSafeTransfer
    Private Sub RPTSafeTransfer_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel1.Text = My.Settings.Website
        XrLabel13.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel27.Text = GetUserName
        XrLabel18.Text = Cur_Code(FrmSafeTransfer.CurrencyID.Text, FrmSafeTransfer.WithdrawalValue.EditValue, False, False)
        XrLabel19.Text = Cur_Code(FrmSafeTransfer.CurrencyID.Text, FrmSafeTransfer.WithdrawalValue.EditValue, True, "n2")
    End Sub
End Class