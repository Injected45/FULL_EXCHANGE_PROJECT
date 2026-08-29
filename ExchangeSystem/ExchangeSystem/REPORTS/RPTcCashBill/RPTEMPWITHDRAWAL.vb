Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTEMPWITHDRAWAL

    Private Sub RPTEMPWITHDRAWAL_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        'WithdrawalDate.Text = Format(FRMEMPWITHDRAWAL.WithdrawalDate.EditValue, "yyyy/MM/dd").ToString
        'WithdrawalValue.Text = FRMEMPWITHDRAWAL.WithdrawalValue.Text
        'SafeID.Text = FRMEMPWITHDRAWAL.SafeID.Text
        XrLabel21.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        If FRMEMPWITHDRAWALNEW.LOADTYPE = 5 Then
            XrLabel7.Text = "سند صرف من حساب موظف"
            XrLabel5.Text = "WITHDRAWAL BILL"
            XrLabel87.Text = ":تم الصرف نقدا من"
            XrLabel81.Text = ":من حساب الموظف"
            XrPictureBox35.Image = My.Resources.R_dollar
            XrPictureBox36.Image = My.Resources.R_dollar
            XrPictureBox33.Image = My.Resources.R_dollar
        End If

        'If FRMEMPWITHDRAWAL.LOADTYPE = 5 Then
        '    XrLabel7.Text = "سند صرف من حساب موظف"
        '    XrLabel5.Text = "WITHDRAWAL BILL"
        '    XrLabel87.Text = ":تم الصرف نقدا من"
        '    XrLabel81.Text = ":من حساب الموظف"
        '    XrPictureBox35.Image = My.Resources.R_dollar
        '    XrPictureBox36.Image = My.Resources.R_dollar
        '    XrPictureBox33.Image = My.Resources.R_dollar
        'ElseIf FRMEMPWITHDRAWAL.LOADTYPE = 6 Then
        '    XrLabel7.Text = "سند صرف من حساب عميل"
        '    XrLabel5.Text = "WITHDRAWAL BILL"
        '    XrLabel87.Text = ":تم الصرف نقدا من"
        '    XrLabel81.Text = ":من حساب العميل"
        '    XrPictureBox35.Image = My.Resources.R_dollar
        '    XrPictureBox36.Image = My.Resources.R_dollar
        '    XrPictureBox33.Image = My.Resources.R_dollar
        'ElseIf FRMEMPWITHDRAWAL.LOADTYPE = 7 Then
        '    XrLabel7.Text = "سند إيداع في حساب موظف"
        '    XrLabel5.Text = "DEPOSIT BILL"
        '    XrLabel87.Text = ":تم الإيداع في"
        '    XrLabel81.Text = ":لحساب الموظف"
        '    XrPictureBox5.Visible = False
        '    XrPictureBox14.Visible = False
        '    XrPictureBox6.Visible = False
        '    XrLabel55.Visible = False
        '    XrLabel56.Visible = False
        '    XrLabel57.Visible = False
        '    XrLabel58.Visible = False
        '    XrLabel60.Visible = False
        '    XrLabel52.Visible = False
        '    XrShape4.Visible = False
        '    XrShape3.HeightF = 260
        'ElseIf FRMEMPWITHDRAWAL.LOADTYPE = 8 Then
        '    XrLabel7.Text = "سند إيداع في حساب عميل"
        '    XrLabel5.Text = "DEPOSIT BILL"
        '    XrLabel87.Text = ":تم الإيداع في"
        '    XrLabel81.Text = ":لحساب العميل"
        '    XrPictureBox5.Visible = False
        '    XrPictureBox14.Visible = False
        '    XrPictureBox6.Visible = False
        '    XrLabel55.Visible = False
        '    XrLabel56.Visible = False
        '    XrLabel57.Visible = False
        '    XrLabel58.Visible = False
        '    XrLabel60.Visible = False
        '    XrLabel52.Visible = False
        '    XrShape4.Visible = False
        '    XrShape3.HeightF = 260
        'End If

        XrLabel39.Text = GetUserName

    End Sub
End Class