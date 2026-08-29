Imports System.ComponentModel
Imports System.IO

Public Class RPTselectACountes
    Private Sub RPTselectACountes_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox10.Image = Image.FromStream(ms)
        End Using

        BranchIDd.Text = FRMselectACountes.BranchIDd.Text
        D1.Text = FRMselectACountes.DT1.EditValue
        D2.Text = FRMselectACountes.DT2.EditValue
        XrLabel6.Text = FRMselectACountes.ACCTYP.Text
        XrLabel13.Text = FRMselectACountes.ACCCMD1.Text
        XrLabel14.Text = FRMselectACountes.ACCline.Text
        If FRMselectACountes.ACCTYP.SelectedIndex = 1 Then
            XrPictureBox2.Visible = False
            XrLabel19.Visible = False
            XrLabel14.Visible = False
        End If
        XrLabel8.Text = GetUserName
    End Sub
End Class