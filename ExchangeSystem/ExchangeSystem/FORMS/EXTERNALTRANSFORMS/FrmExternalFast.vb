Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Public Class FrmExternalFast
    Public SelectBtn As Boolean, SelectType As Integer
    Public Sub LOADDATA()
        GCROLE.DataSource = Nothing
        GVROLE.Columns.Clear()
        OverAllEx.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
        PRM(1) = New SqlParameter("@SelectType", SqlDbType.Int) With {.Value = SelectType}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("ExternalEx_SelectType", PRM)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            GVROLE.Columns("RecievedBranchID").Visible = False
            DVGFROMAT()

            'If FRMMAIN.SelectType = 4 Or FRMMAIN.SelectType = 2 Then
            If GVROLE.RowCount > 0 Then
                Dim OverallSalary As New GridColumnSummaryItem()
                OverallSalary.SummaryType = SummaryItemType.Sum
                OverallSalary.FieldName = "قيمة الحوالة"
                GVROLE.Columns("قيمة الحوالة").Summary.Add(OverallSalary)

                'OverAllTotal.EditValue = 0.000
                OverAllTotal.EditValue = Convert.ToDouble(GVROLE.Columns("قيمة الحوالة").SummaryItem.SummaryValue)
                ''-----------------------------------------------------------------------------
                Dim OverallConstance As New GridColumnSummaryItem()
                OverallConstance.SummaryType = SummaryItemType.Sum
                OverallConstance.FieldName = "العمولة"
                GVROLE.Columns("العمولة").Summary.Add(OverallConstance)

                'OverAllEx.EditValue = 0.000
                OverAllEx.EditValue = Convert.ToDouble(GVROLE.Columns("العمولة").SummaryItem.SummaryValue)
                ''---------------------------------------------------



            End If
        Else
            GCROLE.DataSource = Nothing
            'FRMMAIN.SelectType = 0
        End If
    End Sub
    Sub DVGFROMAT()
        GVROLE.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVROLE.OptionsBehavior.Editable = False
        GVROLE.OptionsBehavior.EditingMode = False
        GVROLE.OptionsBehavior.ReadOnly = True
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsView.ShowFooter = False
        GVROLE.OptionsSelection.EnableAppearanceFocusedRow = False
        GVROLE.OptionsSelection.MultiSelectMode = False
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVROLE.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Private Sub FrmInternalFastCall_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFROMAT()
    End Sub

    Private Sub FrmInternalFastCall_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        FRMMAIN.SelectType = 0
    End Sub

    Private Sub GVROLE_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
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



    Private Sub GVROLE_DoubleClick(sender As Object, e As EventArgs) Handles GVROLE.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        Dim co As String = view.GetFocusedRowCellValue("الرمز")
        If SelectType = 5 Then
            FRMEXTERNALTRANS.ConfirmType = 3
            FRMEXTERNALTRANS.ISUpdate = 1
            FRMEXTERNALTRANS.SHOW_RECORD(co)
            FRMEXTERNALTRANS.ShowDialog()
            GCROLE.DataSource = Nothing
            GVROLE.Columns.Clear()
            LOADDATA()
        End If
        If SelectType = 4 Then
            FRMEXTERNALTRANS.ConfirmType = 2
            FRMEXTERNALTRANS.ISUpdate = 1
            FRMEXTERNALTRANS.SHOW_RECORD(co)
            FRMEXTERNALTRANS.ShowDialog()
            GCROLE.DataSource = Nothing
            GVROLE.Columns.Clear()
            LOADDATA()
        End If
    End Sub

    Private Sub GVROLE_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVROLE.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)

        If e.Column.FieldName Is "الحالة" Then
            Dim sendstatus As String = View.GetRowCellDisplayText(e.RowHandle, View.Columns("الحالة"))
            'View.GetRowCellDisplayText(e.RowHandle, View.Columns("الحالة"))
            If View.RowCount > 0 Then
                If sendstatus = "جاهزة للتسليم" Then
                    e.Appearance.BackColor = Color.FromArgb(19, 106, 97)
                    e.Appearance.BackColor2 = Color.FromArgb(19, 106, 97)
                    e.Appearance.ForeColor = Color.White
                ElseIf sendstatus = "تحت الإجراء" Then
                    e.Appearance.BackColor = Color.FromArgb(254, 74, 73)
                    e.Appearance.BackColor2 = Color.FromArgb(254, 74, 73)
                    e.Appearance.ForeColor = Color.White

                End If
            End If
        End If
    End Sub
End Class