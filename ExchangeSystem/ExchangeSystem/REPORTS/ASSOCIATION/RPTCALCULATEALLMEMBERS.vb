Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTCALCULATEALLMEMBERS
    Private Sub RPTCALCULATEALLMEMBERS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        MDATE.Text = FRMCALCULATEALLMEMBERS.MDATE.Text
        YDATE.Text = FRMCALCULATEALLMEMBERS.YDATE.Text
        XrLabel3.Text = FRMCALCULATEALLMEMBERS.ASSOCIATION.Text
        XrLabel7.Text = Cur_Code("ليبي", FRMCALCULATEALLMEMBERS.OverallNetTotal.EditValue, True, "n2")
        XrLabel4.Text = GetUserName
    End Sub
End Class