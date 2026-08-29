Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewBalanceTransport
    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Me.Close()
    End Sub
    Public Sub LoadData()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("WithdrawalTb_SELECTALL", PRM)
        If DT.Rows.Count > 0 Then
            GVRole.Columns.Clear()
            GCRole.DataSource = DT
            ADDCOLUMN()
            DVGFROMAT()
            GVRole.Columns("WDCode").Caption = "الرمز"
            GVRole.Columns("WithdrawalDate").Caption = "التاريخ"
            GVRole.Columns("firstuname").Caption = "الخزنة المنقول منها"
            GVRole.Columns("Seconduname").Caption = "الخزنة المنقول إليها"
            GVRole.Columns("WithdrawalValue").Caption = "القيمة المنقولة"
            GVRole.Columns("BName").Caption = "الفرع"
        End If
    End Sub
    Sub ADDCOLUMN()
        Dim colCounter As GridColumn
        colCounter = GVRole.Columns.AddVisible("RowHandle")
        colCounter.Caption = "#"
        colCounter.VisibleIndex = 0
        colCounter.Width = 50
        colCounter.UnboundType = DevExpress.Data.UnboundColumnType.Integer
        colCounter.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
    End Sub
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "RowHandle" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
        Dim view As GridView = TryCast(sender, GridView)
    End Sub

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim roleId As String = view.GetFocusedRowCellValue("WDCode")
            FrmSafeTransfer.IsUpdate = True
            FrmSafeTransfer.DISAPLEDTOOLS()
            FrmSafeTransfer.SHOW_WD_DATA(roleId)
            Me.Close()
        End If
    End Sub
    Private Sub FrmViewBalanceTransport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GridLocalizer.Active = New MyGridLocalizer()
        LoadData()
    End Sub

End Class