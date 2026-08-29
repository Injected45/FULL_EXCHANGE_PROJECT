Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraReports.UI

Public Class RPTINCOME
    Private Sub RPTINCOME_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        D1.Text = FRMINCOMESTATMENT.DT1.EditValue
        D2.Text = FRMINCOMESTATMENT.DT2.EditValue
        XrLabel6.Text = FRMINCOMESTATMENT.branchID.Text
        XrLabel3.Text = GetUserName
        Dim OT As Double = FRMINCOMESTATMENT.OverAllTotal.EditValue
        OverallTotal.Text = OT
    End Sub
End Class