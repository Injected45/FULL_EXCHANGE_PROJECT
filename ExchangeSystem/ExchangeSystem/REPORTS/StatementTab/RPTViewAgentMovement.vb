Imports System.ComponentModel

Public Class RPTViewAgentMovement
    Private Sub RPTViewAgentMovement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        ApplyLocalization("en-US")
        XrLabel8.Text = GetUserName
        If FrmViewAgentMovement.NETtotal = -1 Then
            XrPictureBox12.Image = My.Resources.R_dollar
        End If
        If FrmViewAgentMovement.Peroid = -1 Then
            XrPictureBox13.Image = My.Resources.R_dollar
        End If
        If FrmViewAgentMovement.PBalance = -1 Then
            XrPictureBox9.Image = My.Resources.R_dollar
        End If

    End Sub
End Class