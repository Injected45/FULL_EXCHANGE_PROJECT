Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTMEMBERSLOADALL
    Private Sub RPTMEMBERSLOADALL_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel12.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox5.Image = Image.FromStream(ms)
        End Using
        XrLabel1.Text = FRMMEMBERSLOADALL.AssID.Text
        D1.Text = Format(FRMMEMBERSLOADALL.D1.EditValue, "dd/MM/yyyy")
        D2.Text = Format(FRMMEMBERSLOADALL.D2.EditValue, "dd/MM/yyyy")
        XrLabel7.Text = Cur_Code("ليبي", FRMMEMBERSLOADALL.OverAllDebit.EditValue, True, "n2")
        XrLabel6.Text = Cur_Code("ليبي", FRMMEMBERSLOADALL.OverAllCredit.EditValue, True, "n2")
        XrLabel9.Text = Cur_Code("ليبي", FRMMEMBERSLOADALL.OverAllTotal1.EditValue, True, "n2")
        If FRMMEMBERSLOADALL.OverAllTotal1.EditValue > 0 Then
            XrPictureBox4.Image = My.Resources.G_dollar
        End If
        XrLabel4.Text = GetUserName
    End Sub
End Class