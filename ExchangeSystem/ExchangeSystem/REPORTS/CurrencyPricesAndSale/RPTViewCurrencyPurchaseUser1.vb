Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTViewCurrencyPurchaseUser1
    Private Sub RPTViewCurrencyPurchaseUser1_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        If FrmCustomerCurrBayAndSale.CurrencyTo.Text = "الكل" Then
            XrLabel20.Text = "كشف حركة بيع العملات"
            XrLabel13.Text = "كل العملات"
            XrPictureBox4.Visible = False
            XrLabel6.Visible = False
            XrLabel7.Visible = False
        Else
            XrLabel20.Text = "كشف حركة بيع عملة"
            XrLabel13.Text = FrmCustomerCurrBayAndSale.CurrencyTo.Text
        End If

        XrLabel8.Text = GetUserName
    End Sub

End Class