Imports System.ComponentModel
Imports System.IO

Public Class RPTViewCurrencyPurchaseTransactions
    Private Sub RPTViewCurrencyPurchaseTransactions_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        XrLabel17.Text = FRMViewCurrencyPurchaseTransactions2.Branchid.Text
        If FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text = "الكل" Then
            XrLabel20.Text = "كشف حركة بيع وشراء العملات"
            XrLabel13.Text = "كل العملات"

        Else
            XrLabel20.Text = "كشف حركة بيع وشراء عملة"
            XrLabel13.Text = FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text
        End If

        D1.Text = FRMViewCurrencyPurchaseTransactions2.DateEdit11.EditValue
        D2.Text = FRMViewCurrencyPurchaseTransactions2.DateEdit2.EditValue
        XrLabel23.Text = Format(FRMViewCurrencyPurchaseTransactions2.Losses, "N3") + " د.ل "
        XrLabel26.Text = Format(FRMViewCurrencyPurchaseTransactions2.Profit, "N3") + " د.ل "

        XrLabel8.Text = GetUserName
    End Sub
End Class