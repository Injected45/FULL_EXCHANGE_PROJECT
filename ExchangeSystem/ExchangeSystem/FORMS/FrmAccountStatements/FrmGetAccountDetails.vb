Imports DevExpress.Data
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FrmGetAccountDetails
    Sub LoadData()
        GCROLE.DataSource = Nothing
        GVROLE.Columns.Clear()
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmSelectAccountsBetweenBranches.BranchID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = FrmSelectAccountsBetweenBranches.D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = FrmSelectAccountsBetweenBranches.D2.EditValue}
        PRM(3) = New SqlParameter("@SumDebit", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        PRM(4) = New SqlParameter("@SumCredit", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        PRM(5) = New SqlParameter("@OverallTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccSafeActivityTb_GetAccountsDetails", PRM)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            'SumDebit.EditValue = PRM(3).Value
            'SumCredit.EditValue = PRM(4).Value
            'OverAllTotal.EditValue = PRM(5).Value
            DVGFROMAT()
            SumTotal()
        Else
            GCROLE.DataSource = Nothing
            GVROLE.Columns.Clear()
        End If
        SumTotal()
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
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
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
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVROLE.RowCellStyle
        'Dim View As GridView = TryCast(sender, GridView)
        Dim view As GridView = TryCast(sender, GridView)
        If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
            If e.Column.FieldName = "مدين" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Red
            End If
            If e.Column.FieldName = "دائن" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Green
            End If
        End If
    End Sub
    Private Sub FrmGetAccountDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo

        Me.Text = "تفاصيل الحركات لـ" & FrmSelectAccountsBetweenBranches.BranchID.Text
        LoadData()
    End Sub

    Sub SumTotal()
        SumDebit.EditValue = 0.000
        SumCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
        CreditSum.FieldName = "دائن"
        GVROLE.Columns("دائن").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "مدين"
            GVROLE.Columns("مدين").Summary.Add(DebitSum)
            SumDebit.EditValue = Convert.ToDouble(GVROLE.Columns("مدين").SummaryItem.SummaryValue)
            SumCredit.EditValue = Convert.ToDouble(GVROLE.Columns("دائن").SummaryItem.SummaryValue)
        OverAllTotal.EditValue = SumCredit.EditValue - SumDebit.EditValue
    End Sub

    Private Sub GVROLE_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVROLE.FocusedRowChanged
        SumTotal()
    End Sub

    Private Sub GVROLE_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVROLE.ColumnFilterChanged
        SumTotal()
    End Sub
End Class