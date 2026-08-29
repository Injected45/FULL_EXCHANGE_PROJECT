Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraSpellChecker

Public Class RPTEMPWITHDRAWAL2

    Private Sub RPTEMPWITHDRAWAL_BeforePrint(sender As Object, e As CancelEventArgs) Handles MyBase.BeforePrint
        'WithdrawalDate.Text = Format(FRMEMPWITHDRAWAL.WithdrawalDate.EditValue, "yyyy/MM/dd").ToString
        'WithdrawalValue.Text = FRMEMPWITHDRAWAL.WithdrawalValue.Text
        'SafeID.Text = FRMEMPWITHDRAWAL.SafeID.Text
        XrLabel21.Text = My.Settings.ARName
        XrLabel9.Text = My.Settings.Website
        XrLabel8.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        If FRMEMPWITHDRAWAL.LOADTYPE = 5 Then
            XrLabel2.Text = FRMEMPWITHDRAWAL.CurrencyFrom.Text
            XrLabel93.Text = Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.Text, True, "")
            XrLabel25.Text = Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.Text, False, False)
            If FRMEMPWITHDRAWAL.TYPEs = 2 Then
                XrLabel7.Text = "سند صرف نقد أجنبي"
            Else
                XrLabel7.Text = "سند صرف من حساب"
            End If
            XrLabel58.Text = "صرفت لصالح"
            XrLabel5.Text = "WITHDRAWAL BILL"
            XrLabel87.Text = ":تم الصرف نقدا من"
            XrLabel81.Text = ":من حساب"
            XrPictureBox35.Image = My.Resources.R_dollar
            XrPictureBox36.Image = My.Resources.R_dollar
            XrPictureBox33.Image = My.Resources.R_dollar
            XrPictureBox5.Visible = True
            XrPictureBox6.Visible = False
            XrLabel55.Visible = False
            PaidFor.Visible = True

            IDNo.Visible = False
            XrLabel58.Visible = True
            XrPictureBox4.Visible = False
            XrLabel4.Visible = False
            Phone.Visible = False
            XrLabel3.Visible = False
            XrLabel13.Visible = False
        ElseIf FRMEMPWITHDRAWAL.LOADTYPE = 7 Then
            XrLabel2.Text = FRMEMPWITHDRAWAL.CurrencyFrom.Text
            XrLabel93.Text = Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.Text, True, "")
            XrLabel25.Text = Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.Text, False, False)
            If FRMEMPWITHDRAWAL.TYPEs = 2 Then
                XrLabel7.Text = "سند إيداع نقد أجنبي"

            Else
                XrLabel7.Text = "سند إيداع في حساب"

            End If
            XrLabel58.Text = "قبض من"
            XrLabel5.Text = "DEPOSIT BILL"
            XrLabel87.Text = ":تم الإيداع في"
            XrLabel81.Text = ":لحساب"
            XrPictureBox14.Visible = True
            XrPictureBox5.Visible = True
            XrLabel60.Visible = False
            XrLabel52.Visible = False
            XrPictureBox6.Visible = True
            XrLabel55.Visible = True
            PaidFor.Visible = True
            IDNo.Visible = True
            XrLabel58.Visible = True
            XrPictureBox4.Visible = False
            XrLabel4.Visible = True
            Phone.Visible = True
            XrLabel3.Visible = False
            XrLabel13.Visible = False
            XrPictureBox14.Visible = False
            'XrPictureBox36.Location = New Point(1923.77, 844.35)
            'XrLabel94.Location = New Point(1672.84, 831.12)
            'XrLabel93.Location = New Point(1158.07, 831.12)
            'XrPictureBox35.Location = New Point(1098.07, 844.35)
            'XrLabel91.Location = New Point(952.71, 831.12)
            'XrLabel25.Location = New Point(45.25, 831.12)
            'XrLabel95.Location = New Point(46.33, 921.29)
            'XrShape3.HeightF = 274.46
            'XrShape3.Location = New Point(18, 772.26)
        End If

        XrLabel24.Text = GetUserName



    End Sub
End Class