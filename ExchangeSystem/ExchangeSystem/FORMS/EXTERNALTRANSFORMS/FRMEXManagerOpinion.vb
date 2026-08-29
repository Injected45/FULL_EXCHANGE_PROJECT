Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI
Public Class FRMEXManagerOpinion
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If CodeID.Text = String.Empty Then
            CodeID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If Notes.Text = String.Empty Then
            Notes.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        Dim Txt As String
        Txt = CodeID.Text & vbNewLine & Notes.Text
        WATSAPPMsAG("120363373217260385@g.us", Txt, True)
        CodeID.Text = ""
        Notes.Text = ""
        Me.Close()
    End Sub
End Class