Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRM_Total_Branches_Cash
    Public Sub lode_date()
        GridControl1.Width = Me.Width - 40

        GridControl1.Padding = New Padding(20, 0, 20, 0) ' إضافة مسافة فارغة من الجانب الأيمن

        Dim DT As New DataTable
        DVGFROMAT()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = 0}
        DT.Clear()
        DT = RUN_QUARY_PRO("Budget_per_mini_iliali_FILLALL", prm)
        GridControl1.DataSource = Nothing
        If DT.Rows.Count > 0 Then
            GridControl1.DataSource = DT


        End If

    End Sub

    Sub DVGFROMAT()
        GridLocalizer.Active = New MyGridLocalizer()
        GridView1.ShowFindPanel()
        GridView1.OptionsBehavior.Editable = False
        GridView1.OptionsBehavior.EditingMode = False
        GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView1.OptionsView.ShowGroupPanel = False
        GridView1.GroupPanelText = ""
        GridView1.OptionsView.ShowFooter = False
        GridView1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        For i As Integer = 0 To GridView1.Columns.Count - 1
            GridView1.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next

        GridView1.OptionsView.EnableAppearanceEvenRow = True
        GridView1.Appearance.EvenRow.BackColor = Color.White
        GridView1.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub FRM_Total_Branches_Cash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lode_date()
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        If GridView1.RowCount > 0 Then
            FRMBranches_Budget.Budget_per_mini_iliali(GridView1.GetFocusedRowCellValue("BranchID"))
            FRMBranches_Budget.ShowDialog()
        End If
    End Sub

    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName Is "netfortotAL" Then
            Dim status As String = View.GetRowCellDisplayText(e.RowHandle, View.Columns("netfortotAL"))

            If View.RowCount > 0 Then
                If status > 0 Then
                    e.Appearance.BackColor = Color.Green
                    e.Appearance.ForeColor = Color.Yellow
                Else
                    e.Appearance.BackColor = Color.Red
                    e.Appearance.ForeColor = Color.Yellow
                End If
            End If
        End If




    End Sub
End Class