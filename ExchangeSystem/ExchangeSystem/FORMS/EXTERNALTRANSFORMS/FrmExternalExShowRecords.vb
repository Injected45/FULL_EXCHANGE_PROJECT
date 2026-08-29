Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmExternalExShowRecords
    Dim DT As New DataTable
    Sub LoadData()
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ExternalEx_ShowRecords")
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            NEWDVGFROMAT(GVROLE)
            GVROLE.Columns("القيمة").AppearanceCell.BackColor = Color.FromArgb(0, 128, 43)
            GVROLE.Columns("العمولة").AppearanceCell.BackColor = Color.FromArgb(0, 153, 204)
        End If
    End Sub
    Sub DVGFROMAT()
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsFind.AllowFindPanel = True
        GVROLE.GroupPanelText = ""
        GVROLE.OptionsView.ShowFooter = False
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10.5, FontStyle.Regular)
        Next
        GVROLE.Appearance.EvenRow.BackColor = Color.White
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVROLE.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 153, 153), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVROLE.RowCellStyle
        'Dim View As GridView = TryCast(sender, GridView)
        Dim view As GridView = TryCast(sender, GridView)
        If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
            If e.Column.FieldName = "القيمة" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.FromArgb(0, 128, 43)
            End If
            If e.Column.FieldName = "العمولة" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.FromArgb(0, 153, 204)
            End If
        End If
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVROLE.DoubleClick
        If GVROLE.RowCount > 0 Then
            Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
            Dim view As GridView = TryCast(sender, GridView)
            Dim info = view.CalcHitInfo(ea.Location)
            If info.InRow OrElse info.InRowCell Then
                Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
                FRMEXTERNALTRANS.IsUpdate = 1
                FRMEXTERNALTRANS.SHOW_RECORD(CO)
                FRMEXTERNALTRANS.ShowDialog()
                Me.Close()
            End If
        End If

    End Sub

    Private Sub FrmInternalExShowRecords_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub


    Private Sub GCROLE_KeyDown(sender As Object, e As KeyEventArgs) Handles GCROLE.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class