Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTNewPROFITS
    Private Sub RPTNewPROFITS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel6.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        D1.Text = Format(FRMNewPROFITS.D1.EditValue, "yyyy/MM/dd").ToString
        D2.Text = Format(FRMNewPROFITS.D2.EditValue, "yyyy/MM/dd").ToString
        XrLabel41.Text = FRMNewPROFITS.BranchID.Text
        XrLabel2.Text = GetUserName
    End Sub
End Class