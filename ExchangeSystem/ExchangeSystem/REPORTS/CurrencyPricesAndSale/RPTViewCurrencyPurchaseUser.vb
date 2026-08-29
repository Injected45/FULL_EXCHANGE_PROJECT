Imports System.ComponentModel
Imports System.IO

Public Class RPTViewCurrencyPurchaseUser
    Private Sub RPTViewCurrencyPurchaseUser_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        If FrmCustomerCurrBayAndSale.CurrencyTo.Text = "الكل" Then
            XrLabel20.Text = "كشف حركة بيع وشراء العملات"
            XrLabel13.Text = "كل العملات"
            XrPictureBox9.Visible = False
            XrPictureBox11.Visible = False
            XrLabel19.Visible = False
            XrLabel23.Visible = False
            XrLabel26.Visible = False
            XrLabel21.Visible = False
        Else
            XrLabel20.Text = "كشف حركة بيع وشراء عملة"
            XrLabel13.Text = FrmCustomerCurrBayAndSale.CurrencyTo.Text
        End If

        XrLabel8.Text = GetUserName
    End Sub
End Class