Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.PivotGrid.Internal
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMBranches_Budget
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        branchID.Properties.DataSource = Nothing
        DT.Clear()
        DT.NewRow()
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("BName", GetType(String))
        DT = RUN_QUARY_TXT("COBRANCHTB_LoadDataIntoLookUpEdit_FILL_pro")
        DT.Rows.Add(0, "كل الفروع")
        If DT.Rows.Count > 0 Then
            branchID.Properties.DataSource = DT
            branchID.Properties.ValueMember = "DBRID"
            branchID.Properties.DisplayMember = "BName"

        End If
    End Sub
    Sub DVGFROMAT1()
        GridLocalizer.Active = New MyGridLocalizer()
        GridView1.OptionsBehavior.Editable = False
        GridView1.OptionsBehavior.EditingMode = False
        GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView1.OptionsView.ShowGroupPanel = False
        GridView1.GroupPanelText = ""
        GridView1.OptionsView.ShowFooter = False
        GridView1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GridView1.Columns.Count - 1
            GridView1.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GridView1.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView1.OptionsView.EnableAppearanceEvenRow = True
        GridView1.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView1.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Sub DVGFROMAT()
        GridLocalizer.Active = New MyGridLocalizer()
        GridView2.OptionsBehavior.Editable = False
        GridView2.OptionsBehavior.EditingMode = False
        GridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView2.OptionsView.ShowGroupPanel = False
        GridView2.GroupPanelText = ""
        GridView2.OptionsView.ShowFooter = False
        GridView2.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GridView2.Columns.Count - 1
            GridView2.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView2.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView2.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView2.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GridView2.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView2.OptionsView.EnableAppearanceEvenRow = True
        GridView2.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView2.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView2.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView2.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            branchID.Enabled = dt.Rows(0)("Can_branch")
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            branchID.EditValue = BID
        Else
            branchID.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            branchID.EditValue = BID
        End If
    End Sub
    Public Sub new_recordes()
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
        LOADBRANCH()
        DVGFROMAT()
        DVGFROMAT1()
        ' branchID.EditValue = BID
        DT1.EditValue = Date.Now
        DT2.EditValue = Date.Now
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub

    Public Sub Budget_per_mini_iliali(branchI As Integer)
        Try
            new_recordes()
            GridControl1.DataSource = Nothing
            GridControl2.DataSource = Nothing
            branchID.EditValue = branchI
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchI}
            prm(1) = New SqlParameter("@sumorDebit", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            prm(2) = New SqlParameter("@sumorCredit", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("Budget_per_mini_iliali", prm)
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
                GridControl2.DataSource = dt
            End If
            SumTotal()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub SumTotal()
        SUMdibet.EditValue= 0
        Sumcredit.EditValue = 0
        OverAllTotal.EditValue = 0
        If GridView1.RowCount > 0 Then
            Dim ExVal As New GridColumnSummaryItem()
            ExVal.SummaryType = SummaryItemType.Sum
            ExVal.FieldName = "TenTotel"
            GridView1.Columns("TenTotel").Summary.Add(ExVal)
            SUMdibet.EditValue = Convert.ToDouble(GridView1.Columns("TenTotel").SummaryItem.SummaryValue)
        End If
        If GridView2.RowCount > 0 Then
            Dim ExVal As New GridColumnSummaryItem()
            ExVal.SummaryType = SummaryItemType.Sum
            ExVal.FieldName = "TenTotel"
            GridView2.Columns("TenTotel").Summary.Add(ExVal)
            Sumcredit.EditValue = Convert.ToDouble(GridView2.Columns("TenTotel").SummaryItem.SummaryValue)
        End If
        OverAllTotal.EditValue = SUMdibet.EditValue - Sumcredit.EditValue
        If OverAllTotal.EditValue > 0 Then
            OverAllTotal.BackColor = Color.Green
        Else
            OverAllTotal.BackColor = Color.Red
        End If
    End Sub

    ''كود عدم غرض الامانات  التي عليها توصيل داخلي وتصفيتها من خلال قريد فيو ابروحها 
    Private Sub GridView1_CustomRowFilter(sender As Object, e As RowFilterEventArgs) Handles GridView1.CustomRowFilter
        ' Cast the sender to a GridView object
        Dim view As GridView = TryCast(sender, GridView)

        ' If the cast fails, exit the subroutine
        If view Is Nothing Then Return

        ' Attempt to retrieve the value of the "TipeId" column for the current row
        Dim tipeId As Integer
        Try
            tipeId = Convert.ToInt32(view.GetListSourceRowCellValue(e.ListSourceRow, "AccDmType"))
        Catch ex As Exception
            ' Handle any conversion errors, such as DBNull values
            e.Visible = False
            e.Handled = True
            Return
        End Try

        ' Hide the row if the "TipeId" value is 12
        If tipeId = 1 Then
            e.Visible = False ' Set the row to be invisible
            e.Handled = True  ' Indicate that the visibility has been handled
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Budget_per_mini_iliali(branchID.EditValue)
    End Sub

    Private Sub GridView2_CustomRowFilter(sender As Object, e As RowFilterEventArgs) Handles GridView2.CustomRowFilter
        ' Cast the sender to a GridView object
        Dim view As GridView = TryCast(sender, GridView)

        ' If the cast fails, exit the subroutine
        If view Is Nothing Then Return

        ' Attempt to retrieve the value of the "TipeId" column for the current row
        Dim tipeId As Integer
        Try
            tipeId = Convert.ToInt32(view.GetListSourceRowCellValue(e.ListSourceRow, "AccDmType"))
        Catch ex As Exception
            ' Handle any conversion errors, such as DBNull values
            e.Visible = False
            e.Handled = True
            Return
        End Try

        ' Hide the row if the "TipeId" value is 12
        If tipeId = 0 Then
            e.Visible = False ' Set the row to be invisible
            e.Handled = True  ' Indicate that the visibility has been handled
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click

    End Sub
End Class