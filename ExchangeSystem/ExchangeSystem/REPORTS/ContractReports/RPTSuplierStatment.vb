Imports System.ComponentModel
Imports DevExpress.XtraReports.UI
Imports System.IO

Public Class RPTSuplierStatment

    Private Sub RPTSuplierStatment_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using
        D1.Text = FRMSuplierStatment.D1.EditValue
        D2.Text = FRMSuplierStatment.D2.EditValue
        OverAllTotal.Text = Cur_Code("دينار ليبي", FRMSuplierStatment.OverAllTotal.EditValue, True, False)
        OverAllTotal1.Text = Cur_Code("دينار ليبي", FRMSuplierStatment.OvarAllPrint, True, False)
        XrLabel25.Text = Cur_Code("دينار ليبي", FRMSuplierStatment.OverAllDebit.EditValue, True, False)
        XrLabel20.Text = Cur_Code("دينار ليبي", FRMSuplierStatment.OverAllCredit.EditValue, True, False)
        If FRMSuplierStatment.OverAllTotal.EditValue < 0 Then
            XrPictureBox13.Image = My.Resources.R_dollar
        End If
        If FRMSuplierStatment.OvarAllPrint < 0 Then
            XrPictureBox12.Image = My.Resources.R_dollar
        End If

        XrLabel7.Text = FRMSuplierStatment.EMPID.Text
        XrLabel6.Text = FRMSuplierStatment.BranchID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class