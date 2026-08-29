Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraReports.UI

Public Class LeaveManagerOpinion
    Public BossName As String
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If CompanyName.SelectedIndex = -1 Then
            CompanyName.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If CompanyName.SelectedIndex = 1 Then
            BossName = "ادريس رافع الشوبكي"
        End If
        If CompanyName.SelectedIndex = 0 Then
            BossName = "فهد الشوبكي"
        End If
        If CompanyName.SelectedIndex = 2 Then
            BossName = "عبدالحميد العلواني"
        End If
        If CompanyName.SelectedIndex = 3 Then
            BossName = "المهدي الشوبكي"
        End If
        If CodeID.Text = String.Empty Then
            CodeID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If Notes.Text = String.Empty Then
            Notes.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        RUN_EXUTE_TXT("UPDATE LeaveTB SET IsAccepted=1,ManagerOpinion='" & Notes.Text.Trim.Replace("'", "''") & "' WHERE Code='" & CodeID.Text & "'")
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = CodeID.Text}
        Dim results As DataTable = RUN_QUARY_PRO("LeaveTB_Select", PR)
        If results Is Nothing Or results.Rows.Count = 0 Then
            ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
            Exit Sub
        End If
        Dim row As DataRow = results.Rows(0)
        If results IsNot Nothing OrElse results.Rows.Count > 0 Then
            Dim report As New RPTLEAVE
            report.DataSource = results
            report.DataMember = "LeaveTB"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.XrLabel12.Text = BossName
            report.CreateDocument()
            report.ShowPreview()
        Else
            ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
        End If
        CodeID.Text = ""
        Notes.Text = ""
        BossName = ""
        FRMLeaveConfirm.LOADDATA()
        Me.Close()
    End Sub
End Class