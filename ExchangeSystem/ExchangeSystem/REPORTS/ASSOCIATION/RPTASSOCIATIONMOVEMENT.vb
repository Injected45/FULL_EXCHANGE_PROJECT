Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTASSOCIATIONMOVEMENT
    Private Sub RPTASSOCIATIONMOVEMENT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel2.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox1.Image = Image.FromStream(ms)
        End Using
        XrLabel4.Text = FRMASSOCIATIONMOVEMENT.AssID.Text
        D1.Text = FRMASSOCIATIONMOVEMENT.D1.Text
        D2.Text = FRMASSOCIATIONMOVEMENT.D2.Text
        XrLabel25.Text = Cur_Code("ليبي", FRMASSOCIATIONMOVEMENT.OverAllTotal1.Text, True, "n2")
        XrLabel7.Text = Cur_Code("ليبي", FRMASSOCIATIONMOVEMENT.OverAllCredit.Text, True, "n2")
        XrLabel1.Text = Cur_Code("ليبي", FRMASSOCIATIONMOVEMENT.OverAllDebit.Text, True, "n2")
        If FRMASSOCIATIONMOVEMENT.OverAllTotal1.EditValue < 0 Then
            XrPictureBox4.Image = My.Resources.R_dollar
        End If
        XrLabel11.Text = GetUserName
    End Sub
End Class