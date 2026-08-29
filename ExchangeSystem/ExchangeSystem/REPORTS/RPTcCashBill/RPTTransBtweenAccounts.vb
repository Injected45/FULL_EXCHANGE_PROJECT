Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraReports.UI

Public Class RPTTransBtweenAccounts
    Private Sub RPTTransBtweenAccounts_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel9.Text = My.Settings.Website
        XrLabel8.Text = My.Settings.FaceBook
        XrLabel10.Text = My.Settings.Mobile1
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        XrLabel24.Text = GetUserName
        XrLabel93.Text = Cur_Code(FRMTransBtweenAccounts.CURRENCYID.Text, FRMTransBtweenAccounts.BillVal.Text, True, "n2")
        XrLabel25.Text = Cur_Code(FRMTransBtweenAccounts.CURRENCYID.Text, FRMTransBtweenAccounts.BillVal.Text, False, "n2")
    End Sub
End Class