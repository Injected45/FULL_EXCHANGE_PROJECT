Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmConfirmCanceledInternalEx
    Public Sub LOADDATA()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
        'PRM(1) = New SqlParameter("@CancelStatus", SqlDbType.Int) With {.Value = CancelStatus}

        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("InternalEx_FollowingConfirmCanceledInternalEx", PRM)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = Nothing
            GCROLE.DataSource = DT
            DVGFROMAT()
        Else
            GCROLE.DataSource = Nothing
            FRMMAIN.SelectType = 0
        End If

        If GVROLE.RowCount > 0 Then
            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "القيمة"
            GVROLE.Columns("القيمة").Summary.Add(OverallSalary)

            'OverAllTotal.EditValue = 0.000
            OverAllTotal.EditValue = Convert.ToDouble(GVROLE.Columns("القيمة").SummaryItem.SummaryValue)
            ''-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "العمولة"
            GVROLE.Columns("العمولة").Summary.Add(OverallConstance)

            'OverAllEx.EditValue = 0.000
            OverAllEx.EditValue = Convert.ToDouble(GVROLE.Columns("العمولة").SummaryItem.SummaryValue)
            ''---------------------------------------------------



        End If

    End Sub

    Sub DVGFROMAT()
        AddSerialColumn(GVROLE)
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

    Private Sub FrmConfirmCanceledInternalEx_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFROMAT()
        LOADDATA()
    End Sub

    Private Sub GVROLE_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs)
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

    Private Sub GVROLE_Click(sender As Object, e As EventArgs) Handles GVROLE.Click
        If GVROLE.RowCount > 0 Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}

            Dim DT As New DataTable
            DT = RUN_QUARY_PRO("InternalEx_FollowingConfirmCanceledInternalEx", PRM)

            If GVROLE.GetFocusedRowCellValue("حالة الطلب") = "تمت الموافقة لإعادتها للراسل" Then
                FRMRETRUNINTERNALEX.InternalExCH.Checked = True
                FRMRETRUNINTERNALEX.LOADDATA()
                FRMRETRUNINTERNALEX.ShowDialog()
            End If
        End If
    End Sub
End Class