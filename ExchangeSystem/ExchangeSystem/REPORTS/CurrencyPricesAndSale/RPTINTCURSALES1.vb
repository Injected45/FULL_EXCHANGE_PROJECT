Imports System.ComponentModel

Public Class RPTINTCURSALES1
    Private Sub RPTINTCURSALES1_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint

        XrLabel15.Text = GetUserName
        'If FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 1 Then
        '    XrLabel34.Text = FRMNEWCURRENCYBUY.SafeTypeFrom.Text
        '    XrLabel62.Text = FRMNEWCURRENCYBUY.SafeTypeTo.Text
        '    XrLabel26.Text = Cur_Code(FRMNEWCURRENCYBUY.CurrencyTo.Text, FRMNEWCURRENCYBUY.BPrice2.Text, False, "n2")
        '    XrLabel41.Text = Cur_Code(FRMNEWCURRENCYBUY.CurrencyFrom.Text, FRMNEWCURRENCYBUY.BPrice1.Text, True, "n2")
        '    XrLabel19.Text = Cur_Code(FRMNEWCURRENCYBUY.CurrencyTo.Text, FRMNEWCURRENCYBUY.BPrice2.Text, True, "n2")
        'End If
        'If FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 2 Then
        '    XrLabel34.Text = FRMNEWCURRENCYSALE.SafeTypeFrom.Text
        '    XrLabel62.Text = FRMNEWCURRENCYSALE.SafeTypeTo.Text
        '    XrLabel26.Text = Cur_Code(FRMNEWCURRENCYSALE.CurrencyTo.Text, FRMNEWCURRENCYSALE.BPrice2.Text, False, "n2")
        '    XrLabel41.Text = Cur_Code(FRMNEWCURRENCYSALE.CurrencyFrom.Text, FRMNEWCURRENCYSALE.BPrice1.Text, True, "n2")
        '    XrLabel19.Text = Cur_Code(FRMNEWCURRENCYSALE.CurrencyTo.Text, FRMNEWCURRENCYSALE.BPrice2.Text, True, "n2")

        '    XrLabel47.Text = ":نوع الصرف"
        '    XrLabel16.Text = ":العملة المباعة"
        '    XrLabel39.Text = ":سعر البيع"
        '    XrLabel60.Text = ":نوع السداد"
        '    XrLabel38.Text = ":العملة المستلمة"
        '    XrLabel7.Text = "سند بيع عملة نقدا"
        '    XrPictureBox7.Image = My.Resources.R_dollar
        '    XrPictureBox16.Image = My.Resources.R_dollar
        '    XrPictureBox17.Image = My.Resources.R_dollar
        '    XrPictureBox15.Image = My.Resources.G_dollar

        '    XrPictureBox5.Image = My.Resources.G_dollar
        '    XrPictureBox8.Image = My.Resources.G_dollar

        'End If

        'If FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 3 Then
        '    XrLabel34.Text = FRMNEWCURRENCYBUY.SafeTypeFrom.Text
        '    XrLabel62.Text = FRMNEWCURRENCYBUY.SafeTypeTo.Text
        '    XrLabel26.Text = Cur_Code(FRMNEWCURRENCYBUY.CurrencyTo.Text, FRMNEWCURRENCYBUY.BPrice2.Text, False, "n2")
        '    XrLabel41.Text = Cur_Code(FRMNEWCURRENCYBUY.CurrencyFrom.Text, FRMNEWCURRENCYBUY.BPrice1.Text, True, "n2")
        '    XrLabel19.Text = Cur_Code(FRMNEWCURRENCYBUY.CurrencyTo.Text, FRMNEWCURRENCYBUY.BPrice2.Text, True, "n2")

        '    XrLabel47.Text = ":نوع الاسترجاع"
        '    XrLabel16.Text = ":العملة المسترجعه"
        '    XrLabel39.Text = ":سعر الشراء"
        '    XrLabel60.Text = ":نوع الاستلام"
        '    XrLabel38.Text = ":العملة المستلمة"
        '    XrLabel7.Text = "سند استرجاع عملة مشتراه نقدا"
        '    XrPictureBox7.Image = My.Resources.R_dollar
        '    XrPictureBox16.Image = My.Resources.R_dollar
        '    XrPictureBox17.Image = My.Resources.R_dollar
        '    XrPictureBox15.Image = My.Resources.G_dollar

        '    XrPictureBox5.Image = My.Resources.G_dollar
        '    XrPictureBox8.Image = My.Resources.G_dollar

        'End If
        'If FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 4 Then
        '    XrLabel34.Text = FRMNEWCURRENCYSALE.SafeTypeFrom.Text
        '    XrLabel62.Text = FRMNEWCURRENCYSALE.SafeTypeTo.Text
        '    XrLabel26.Text = Cur_Code(FRMNEWCURRENCYSALE.CurrencyTo.Text, FRMNEWCURRENCYSALE.BPrice2.Text, False, "n2")
        '    XrLabel41.Text = Cur_Code(FRMNEWCURRENCYSALE.CurrencyFrom.Text, FRMNEWCURRENCYSALE.BPrice1.Text, True, "n2")
        '    XrLabel19.Text = Cur_Code(FRMNEWCURRENCYSALE.CurrencyTo.Text, FRMNEWCURRENCYSALE.BPrice2.Text, True, "n2")

        '    XrLabel47.Text = ":نوع الاسترجاع"
        '    XrLabel16.Text = ":العملة المسترجعه"
        '    XrLabel39.Text = ":سعر البيع"
        '    XrLabel60.Text = ":نوع الصرف"
        '    XrLabel38.Text = ":العملة المصروفة"
        '    XrLabel7.Text = "سند استرجاع عملة مباعه نقدا"
        '    'XrPictureBox7.Image = My.Resources.R_dollar
        '    'XrPictureBox16.Image = My.Resources.R_dollar
        '    'XrPictureBox17.Image = My.Resources.R_dollar
        '    'XrPictureBox15.Image = My.Resources.G_dollar

        '    'XrPictureBox5.Image = My.Resources.G_dollar
        '    'XrPictureBox8.Image = My.Resources.G_dollar

        'End If
    End Sub
End Class