Imports System.ComponentModel
Imports System.IO

Public Class RPTViewuCurrencyPurchaseTransactions0
    Private Sub RPTViewCurrencyPurchaseTransactions2_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        XrLabel26.Text = FRMViewCurrencyPurchaseTransactions2.Branchid.Text
        If FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text = "الكل" Then
            XrLabel20.Text = "كشف حركة بيع وشراء عملات"
        Else
            XrLabel20.Text = "كشف حركة بيع وشراء عملة"
        End If

        If FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text = "الكل" Then
            XrLabel13.Text = "كل العملات"
            XrPictureBox2.Visible = False
            XrLabel4.Visible = False
            XrLabel2.Visible = False
            XrPictureBox3.Visible = False
            XrLabel6.Visible = False
            XrLabel11.Visible = False
            'XrLabel17.Visible = False
        Else
            XrLabel13.Text = FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text
        End If


        D1.Text = FRMViewCurrencyPurchaseTransactions2.DateEdit11.EditValue
        D2.Text = FRMViewCurrencyPurchaseTransactions2.DateEdit2.EditValue

        'XrLabel7.Text = Cur_Code1(FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text)
        XrLabel18.Text = Cur_Code1(FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text)
        'XrLabel27.Text = Cur_Code1(FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text)
        XrLabel30.Text = Cur_Code1(FRMViewCurrencyPurchaseTransactions2.CurrencyTo.Text)
        XrLabel29.Text = Format(FRMViewCurrencyPurchaseTransactions2.TextEdit22.EditValue, "N0")
        XrLabel25.Text = Format(FRMViewCurrencyPurchaseTransactions2.TextEdit2.EditValue, "N0")
        XrLabel2.Text = Format(FRMViewCurrencyPurchaseTransactions2.TextEdit21.EditValue, "N0")










        XrLabel8.Text = GetUserName

    End Sub
End Class