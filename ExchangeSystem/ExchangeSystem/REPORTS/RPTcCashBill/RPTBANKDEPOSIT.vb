Imports System.ComponentModel
Imports System.IO

Public Class RPTBANKDEPOSIT

    Private Sub RPTBANKDEPOSIT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel11.Text = My.Settings.Website
        XrLabel24.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        If FRMBANKDEPOSIT.DiscountFrom.SelectedIndex = 2 Then
            XrPictureBox18.Image = My.Resources.G_dollar
            XrPictureBox16.Image = My.Resources.G_dollar
            XrPictureBox14.Image = My.Resources.G_dollar
            XrPictureBox12.Image = My.Resources.G_dollar
        End If
        If FRMBANKDEPOSIT.LOADTYPE = 16 Then
            XrLabel7.Text = "إيداع بصك"
            XrLabel18.Text = ":صافي الإيداع"
            XrLabel76.Text = ":إيداع لحساب"
            XrLabel67.Text = ":لـــحـــســـاب فـــرع"
            XrPictureBox7.Image = My.Resources.EMPICON
            XrPictureBox3.Image = My.Resources.BANKICON
            XrLabel5.Text = FRMBANKDEPOSIT.GetBANKNAME
            XrLabel13.Text = ":من حساب"
        ElseIf FRMBANKDEPOSIT.LOADTYPE = 17 Then
            XrLabel7.Text = "سحب بصك"
            XrLabel18.Text = ":صافي السحب"
            XrLabel67.Text = ":من حسـاب"
            XrLabel76.Text = ":سحب لحسـاب فـرع"
            XrLabel13.Text = ":الـمستفيد"
            XrPictureBox3.Image = My.Resources.EMPICON
            XrPictureBox7.Image = My.Resources.BANKICON
            XrPictureBox1.Image = My.Resources.R_dollar
            XrPictureBox35.Image = My.Resources.R_dollar
            XrPictureBox13.Image = My.Resources.R_dollar
            XrLabel5.Text = FRMBANKDEPOSIT.GetBANKNAME
        ElseIf FRMBANKDEPOSIT.LOADTYPE = 18 Then
            XrLabel7.Text = "إيداع بصك في حساب عميل"
            XrLabel18.Text = ":صافي الإيداع"
            XrLabel76.Text = ":إيداع لحساب العميل"
            XrLabel67.Text = ":لـــحـــســـاب فـــرع"
            XrPictureBox7.Image = My.Resources.EMPICON
            XrPictureBox3.Image = My.Resources.BANKICON
            XrLabel5.Text = FRMBANKDEPOSIT.GetBANKNAME
        ElseIf FRMBANKDEPOSIT.LOADTYPE = 19 Then
            XrLabel7.Text = "سحب بصك من حساب عميل"
            XrLabel18.Text = ":صافي السحب"
            XrLabel67.Text = ":من حـسـاب الـعـميل"
            XrLabel76.Text = ":سحب لحسـاب فـرع"
            XrPictureBox3.Image = My.Resources.EMPICON
            XrPictureBox7.Image = My.Resources.BANKICON
            XrPictureBox1.Image = My.Resources.R_dollar
            XrPictureBox35.Image = My.Resources.R_dollar
            XrPictureBox13.Image = My.Resources.R_dollar
            XrLabel5.Text = FRMBANKDEPOSIT.GetBANKNAME
        End If
        If FRMBANKDEPOSIT.DiscountFrom.SelectedIndex = 1 Then
            XrPictureBox12.Visible = False
            XrLabel20.Visible = False
            XrLabel22.Visible = False
        End If
        XrLabel39.Text = GetUserName
        XrLabel36.Text = Cur_Code(FRMBANKDEPOSIT.CURRENCYID.Text, FRMBANKDEPOSIT.BillVal.EditValue, False, False)
    End Sub
End Class