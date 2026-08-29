Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Public Class CurrBenfitsStatment
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True



    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            BranchID.EditValue = BID
        End If
    End Sub
    Sub NewRecord()
        BranchID.EditValue = -1
        LOADBRANCH()
        LOADCIDFROMT()
        DVGFROMAT()
        CurrencyID.EditValue = 0
        BranchID.EditValue = BID
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverAllBouns.EditValue = 0.000
        OverallLosing.EditValue = 0.000
        OverallLosing.BackColor = BackColor.Red
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 169)
    End Sub
    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}

        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DT.Rows.Add(0, "الكل")
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CuName"

        Else
            CurrencyID.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
    End Sub

    Sub LoadData()
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        OverAllBouns.EditValue = 0.000
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If D1.Text = String.Empty Then
            D1.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If D2.Text = String.Empty Then
            D2.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If D1.EditValue > D1.EditValue Then
            D1.ErrorText = "عذراً لايمكن ان يكون التاريخ الاول اكبر من تاريخ الثاني"
            Exit Sub
        End If
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("Branch_GetCurrBenefits", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFROMAT()
            GVRole.Columns("الإيراد").AppearanceCell.BackColor = Color.Green
            GVRole.Columns("الخسارة").AppearanceCell.BackColor = Color.Red
            ADDCOLUMN()
            'OverAllNetTotalFinal.EditValue = PR(4).Value
        End If
    End Sub

    Private Sub FRMPROFITS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)
        NewRecord()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        GVRole.Columns.Clear()
        LoadData()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        'BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "RowHandle" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
        Dim view As GridView = TryCast(sender, GridView)
    End Sub
    Sub ADDCOLUMN()
        Dim colCounter As GridColumn
        colCounter = GVRole.Columns.AddVisible("RowHandle")
        colCounter.Caption = "#"
        colCounter.VisibleIndex = 0
        colCounter.Width = 50
        colCounter.UnboundType = UnboundColumnType.Integer
        colCounter.OptionsColumn.AllowSort = DefaultBoolean.False
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        If GVRole.RowCount > 0 Then
            'Dim OverallSalary As New GridColumnSummaryItem()
            'OverallSalary.SummaryType = SummaryItemType.Sum
            'OverallSalary.FieldName = "العمولة"
            'GVRole.Columns("العمولة").Summary.Add(OverallSalary)
            'GVRole.Columns("العمولة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            'OverAllEX.EditValue = 0.000
            'OverAllEX.EditValue = Convert.ToDouble(GVRole.Columns("العمولة").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "الإيراد"
            GVRole.Columns("الإيراد").Summary.Add(OverallConstance)
            GVRole.Columns("الإيراد").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverAllBouns.EditValue = 0.000
            OverAllBouns.EditValue = Convert.ToDouble(GVRole.Columns("الإيراد").SummaryItem.SummaryValue)
            '---------------------------------------------------
            Dim OverallLosingTotal As New GridColumnSummaryItem()
            OverallLosingTotal.SummaryType = SummaryItemType.Sum
            OverallLosingTotal.FieldName = "الخسارة"
            GVRole.Columns("الخسارة").Summary.Add(OverallLosingTotal)
            GVRole.Columns("الخسارة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallLosing.EditValue = 0.000
            OverallLosing.EditValue = Convert.ToDouble(GVRole.Columns("الخسارة").SummaryItem.SummaryValue)
            '---------------------------------------------
            If Convert.ToDouble(GVRole.Columns("الإيراد").SummaryItem.SummaryValue) >= Convert.ToDouble(GVRole.Columns("الخسارة").SummaryItem.SummaryValue) Then
                OverAllNet.BackColor = Color.Green
            Else
                OverAllNet.BackColor = Color.Red
            End If
            OverAllNet.EditValue = Convert.ToDouble(GVRole.Columns("الإيراد").SummaryItem.SummaryValue) - Convert.ToDouble(GVRole.Columns("الخسارة").SummaryItem.SummaryValue)
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
    End Sub

    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub
End Class