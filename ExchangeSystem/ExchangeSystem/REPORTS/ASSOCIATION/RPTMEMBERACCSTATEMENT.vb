Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTMEMBERACCSTATEMENT
    Private Sub RPTMEMBERACCSTATEMENT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel2.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox1.Image = Image.FromStream(ms)
        End Using
        D1.Text = Format(FRMMEMBERSLOADALL.D1.EditValue, "yyyy/MM/dd").ToString
        D2.Text = Format(FRMMEMBERSLOADALL.D2.EditValue, "yyyy/MM/dd").ToString
        XrLabel25.Text = Cur_Code("ليبي", FRMMEMBERACCSTATEMENT.OverAllTotal1.EditValue, True, "n2")
        XrLabel7.Text = Cur_Code("ليبي", FRMMEMBERACCSTATEMENT.OverAllCredit.EditValue, True, "n2")
        XrLabel4.Text = Cur_Code("ليبي", FRMMEMBERACCSTATEMENT.OverAllDebit.EditValue, True, "n2")
        If FRMMEMBERACCSTATEMENT.OverAllTotal1.EditValue > 0 Then
            XrPictureBox4.Image = My.Resources.G_dollar
        End If
        XrLabel12.Text = GetUserName
        XrLabel1.Text = FRMMEMBERSLOADALL.membenam
    End Sub
End Class