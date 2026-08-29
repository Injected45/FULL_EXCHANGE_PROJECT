Imports System.ComponentModel
Imports System.IO

Public Class RPT_Revenuesfor_associations
    Private Sub RPT_Revenuesfor_associations_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel2.Text = My.Settings.ARName
        Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
        Using ms As New MemoryStream(imageBytes)
            XrPictureBox1.Image = Image.FromStream(ms)
        End Using
        XrLabel4.Text = Association_revenues.AssID.Text
        D1.Text = Association_revenues.D1.Text
        D2.Text = Association_revenues.D2.Text
        XrLabel25.Text = Cur_Code("ليبي", Association_revenues.OverAllTotal1.Text, True, "n2")
        XrLabel7.Text = Cur_Code("ليبي", Association_revenues.OverAllCredit.Text, True, "n2")
        XrLabel1.Text = Cur_Code("ليبي", Association_revenues.OverAllDebit.Text, True, "n2")
        If Association_revenues.OverAllTotal1.EditValue < 0 Then
            XrPictureBox4.Image = My.Resources.R_dollar
        End If
        XrLabel11.Text = GetUserName
    End Sub

End Class