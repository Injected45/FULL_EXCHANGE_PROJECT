Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewExpenses
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("ExpensesTb_LOADTODVGBYBRANCH", PR)
        If DTT.Rows.Count > 0 Then
            GCRole.DataSource = DTT
            DVGFROMAT()
        End If
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("رمز المصروف").ToString
            FrmExpenses.IsUpdate = True
            FrmExpenses.BtnSave.Enabled = False
            FrmExpenses.BtnEdit.Enabled = True
            FrmExpenses.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            FrmExpenses.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            'FrmExpenses.SHOW_EXP(CO)
        End If
        Me.Close()
    End Sub

    Private Sub FrmViewExpenses_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFROMAT()
        LOADBRANCH()
        BranchID.EditValue = -1
    End Sub
End Class