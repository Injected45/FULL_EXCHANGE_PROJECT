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
Public Class FRMNewPROFITS
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
    Sub NewRecord()
        BranchID.EditValue = -1
        LOADBRANCH()
        DVGFROMAT()
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverAllBouns.EditValue = 0.000
        OverallLosing.EditValue = 0.000
        OverallLosing.BackColor = BackColor.Red
    End Sub
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        BranchID.Properties.DataSource = Nothing
        DT.Clear()
        DT = RUN_QUARY_TXT("CONDB_ActivityType_LoadDataIntoLookUpEdit")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "AccCode"
            BranchID.Properties.DisplayMember = "AccName"
            BranchID.Properties.ShowHeader = False
        End If
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
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_GetActivityBenefits", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFROMAT()
            GVRole.Columns("الإيراد").AppearanceCell.BackColor = Color.Green
            GVRole.Columns("الخسارة").AppearanceCell.BackColor = Color.Red
            ADDCOLUMN()
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
        BranchID.Properties.Columns("AccCode").Visible = False
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



    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", BranchID.EditValue)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("CONDB_GetActivityBenefits", PRM)
            Dim ds As New DataSet
            dt.TableName = "AccSafeActivityTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTNewPROFITS
                report.DataSource = ds
                report.DataMember = "AccSafeActivityTb"
                report.FilterString = GVRole.ActiveFilterString
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class