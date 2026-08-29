Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTExpenseInquiry
    Private Sub RPTExpenseInquiry_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel4.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        D1.Text = Format(FRMExpenseInquiry.D1.EditValue, "yyy-MM-dd")
        D2.Text = Format(FRMExpenseInquiry.D1.EditValue, "yyy-MM-dd")
        XrLabel8.Text = GetUserName
        XrLabel16.Text = FRMExpenseInquiry.BranchID.Text
        XrLabel1.Text = FRMExpenseInquiry.AccIDEX.Text
    End Sub

End Class