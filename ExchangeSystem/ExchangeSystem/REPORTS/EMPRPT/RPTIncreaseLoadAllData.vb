Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTIncreaseLoadAllData
    Private Sub RPTIncreaseLoadAllData_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        ApplyLocalization("en-US")
        D1.Text = FrmIncreaseLoadAllData.D1.EditValue
        D2.Text = FrmIncreaseLoadAllData.D2.EditValue
        XrLabel1.Text = FrmIncreaseLoadAllData.BranchID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class