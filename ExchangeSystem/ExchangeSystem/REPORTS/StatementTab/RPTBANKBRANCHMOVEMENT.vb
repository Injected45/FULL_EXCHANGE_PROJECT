Imports System.ComponentModel

Public Class RPTBANKBRANCHMOVEMENT
    Private Sub RPTBANKBRANCHMOVEMENT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FRMBANKBRANCHMOVEMENT.D1.EditValue
        D2.Text = FRMBANKBRANCHMOVEMENT.D2.EditValue
        OverAllTotal.Text = Format(FRMBANKBRANCHMOVEMENT.OverAllTotal.EditValue, "N3")
        OverAllTotal1.Text = Format(FRMBANKBRANCHMOVEMENT.OverAllTotal1.EditValue, "N3")
        XrLabel7.Text = FRMBANKBRANCHMOVEMENT.BBANKID.Text
        XrLabel18.Text = FRMBANKBRANCHMOVEMENT.BranchID.Text
        XrLabel8.Text = GetUserName
        If FRMBANKBRANCHMOVEMENT.NET = -1 Then
            XrPictureBox5.Image = My.Resources.G_dollar
        End If

        If FRMBANKBRANCHMOVEMENT.Period = -1 Then
            XrPictureBox4.Image = My.Resources.G_dollar
        End If
        If FRMBANKBRANCHMOVEMENT.Prievew = -1 Then
            XrPictureBox9.Image = My.Resources.G_dollar
        End If
    End Sub
End Class