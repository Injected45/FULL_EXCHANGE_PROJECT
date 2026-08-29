Imports System.ComponentModel
Imports System.IO

Public Class RPTEMPADVANCEPAYMENT
    Private Sub RPTEMPADVANCEPAYMENT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel21.Text = My.Settings.ARName
        XrLabel4.Text = My.Settings.Website
        XrLabel3.Text = My.Settings.FaceBook
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox2.Image = Image.FromStream(ms)
        End Using
        ApplyLocalization("en-US")
        XrLabel15.Text = GetUserName
        If FRMEMPADVANCEPAYMENT.RepaymentPeroid.EditValue < 3 Or FRMEMPADVANCEPAYMENT.RepaymentPeroid.EditValue > 10 Then
            XrLabel11.Text = "شهر"
        ElseIf FRMEMPADVANCEPAYMENT.RepaymentPeroid.EditValue > 2 And FRMEMPADVANCEPAYMENT.RepaymentPeroid.EditValue < 11 Then
            XrLabel11.Text = "أشهر"
        End If
        XrLabel72.Text = Cur_Code(FRMEMPADVANCEPAYMENT.CURRENCYID.Text, FRMEMPADVANCEPAYMENT.OverAllVal.EditValue, False, False)
    End Sub
End Class