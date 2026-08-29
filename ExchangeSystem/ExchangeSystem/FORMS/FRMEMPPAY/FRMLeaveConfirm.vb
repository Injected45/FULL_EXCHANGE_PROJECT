Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraReports.UI

Public Class FRMLeaveConfirm
    Public LeaveConType As Integer = 0
    Sub LOADDATA()
        Try
            If LeaveConType = 1 Then
                GVROLE.Columns("EndCol").Visible = False
                GVROLE.Columns("RejectCol").Visible = True
                GVROLE.Columns("ConfirmCol").Visible = True
                GVROLE.Columns("BossName").Visible = True
            Else
                GVROLE.Columns("EndCol").Visible = True
                GVROLE.Columns("RejectCol").Visible = False
                GVROLE.Columns("ConfirmCol").Visible = False
                GVROLE.Columns("BossName").Visible = False
            End If
            GCROLE.DataSource = Nothing
            DVGFormat()
            Dim Prm(0) As SqlParameter
            Prm(0) = New SqlParameter("@LoadType", SqlDbType.Int) With {.Value = LeaveConType}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("LeaveTB_LOADTODVGToConfirm", Prm)
            If DT.Rows.Count > 0 Then
                GCROLE.DataSource = DT
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub DVGFormat()
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsFind.AlwaysVisible = True
        GVROLE.OptionsView.ShowFooter = False
        GVROLE.OptionsBehavior.Editable = True
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVROLE.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub FRMLeaveConfirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADDATA()
    End Sub

    Private Sub BtnConfirm_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles BtnConfirm.ButtonClick
        Dim info As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim BossName As String = GVROLE.GetRowCellDisplayText(info.RowHandle, GVROLE.Columns("BossName"))

        'If String.IsNullOrWhiteSpace(BossName) Then
        '    ErrorMessage(Me, "معلومة", "الرجاء كتابة اسم المدير أولاً.")
        '    Exit Sub
        'End If
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
        LeaveManagerOpinion.Notes.Select()
        LeaveManagerOpinion.CodeID.Text = iscode
        LeaveManagerOpinion.ShowDialog()
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_QUERY_ONLY("UPDATE LeaveTB SET IsAccepted=1 WHERE Code='" & iscode & "'")
        'Dim PR(0) As SqlParameter
        'PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = iscode}
        'Dim results As DataTable = RUN_QUARY_PRO("LeaveTB_Select", PR)
        'If results Is Nothing Or results.Rows.Count = 0 Then
        '    ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
        '    Exit Sub
        'End If
        'Dim row As DataRow = results.Rows(0)
        'If results IsNot Nothing OrElse results.Rows.Count > 0 Then
        '    Dim report As New RPTLEAVE
        '    report.DataSource = results
        '    report.DataMember = "LeaveTB"
        '    Dim tool As ReportPrintTool = New ReportPrintTool(report)
        '    report.XrLabel12.Text = BossName
        '    report.CreateDocument()
        '    report.ShowPreview()
        'Else
        '    ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
        'End If
        'LOADDATA()
    End Sub

    Private Sub BtnReject_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles BtnReject.ButtonClick
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_QUERY_ONLY("UPDATE LeaveTB SET IsAccepted=2 WHERE Code='" & iscode & "'")
        LOADDATA()
    End Sub

    Private Sub BtnEnd_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles BtnEnd.ButtonClick
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code").ToString
        FrmLeave.NewRecord()
        'FrmLeave.IsUpdate = 1
        'FrmLeave.BranchID.EditValue = -1
        FrmLeave.SHOW_EMCUSCODE(iscode)
        'FrmLeave.BtnSave.Enabled = False
        'FrmLeave.BtnEdit.Enabled = True
        'FrmLeave.BtnPrint.Enabled = True
        'FrmLeave.BtnDelete.Enabled = True
        FrmLeave.ShowDialog()
        LOADDATA()
    End Sub

    Private Sub GCROLE_Click(sender As Object, e As EventArgs) Handles GCROLE.Click

    End Sub
End Class