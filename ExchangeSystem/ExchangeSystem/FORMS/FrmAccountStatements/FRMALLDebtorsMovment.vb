Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient

Public Class FRMALLDebtorsMovment
    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}

        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DT.Rows.Add(1, "دينار ليبي")
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
            DVGFormat(GridLookUpEdit1View)
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub

    Private Sub GridView1_CustomUnboundColumnData_1(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        DVGFormat(GridView2)

    End Sub

    Sub DVGFormat(GridView11 As GridView)
        Dim gvrolls As New GridView
        gvrolls = GridView11
        gvrolls.OptionsBehavior.EditingMode = True
        gvrolls.OptionsBehavior.ReadOnly = True
        gvrolls.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        gvrolls.OptionsView.ShowGroupPanel = False
        gvrolls.OptionsFind.AlwaysVisible = True
        gvrolls.ShowFindPanel()
        gvrolls.OptionsView.ShowFooter = False
        For i As Integer = 0 To GridView11.Columns.Count - 1
            gvrolls.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        gvrolls.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        gvrolls.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        gvrolls.OptionsView.EnableAppearanceEvenRow = True
        gvrolls.Appearance.OddRow.BackColor = Color.WhiteSmoke
        gvrolls.OptionsView.EnableAppearanceOddRow = True

        ' GridLocalizer.Active = New MyGridLocalizer()


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
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub
    Public Sub NOWRecored()
        LOADCIDFROMT()
        CurrencyTo.EditValue = 1
        LOADBRANCH()
        BranchID.EditValue = BID
        GridControl1.DataSource = Nothing
        GridView1.OptionsView.ShowGroupPanel = False
        'FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub

    Private Sub FRMALLDebtorsMovment_Load(sender As Object, e As EventArgs) Handles Me.Load
        NOWRecored()
    End Sub

    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
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


    Public Sub CustomerAccountStatement()
        Try
            GridControl1.DataSource = Nothing
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            OverAllTotal1.EditValue = 0.000
            Dim dt As New DataTable
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}

            dt.Clear()
            dt = RUN_QUARY_PRO("CreditAccountStatement", prm)


            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
                dt.Dispose()


            Else
                MessageBox.Show("عذرا لايوجد بيانات في الوقت الحالي", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        CustomerAccountStatement()
    End Sub

    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName = "ACCNETtotel" Then
            Dim _length As String = CStr(e.CellValue)
            If _length <= 0 Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Red
            Else
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Green
            End If

        End If
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        If GridView1.RowCount > 0 Then
            LOADBRNCHDIERCT(FRMDebtorsMovment.BranchID)
            FRMDebtorsMovment.NEWRECORD()
            FRMDebtorsMovment.BranchID.EditValue = BranchID.EditValue
            FRMDebtorsMovment.LoadDepitorAccounts()
            FRMDebtorsMovment.CUST.EditValue = GridView1.GetFocusedRowCellValue("AccID")
            FRMDebtorsMovment.CustCode.Text = GridView1.GetFocusedRowCellValue("AccID").ToString
            FRMDebtorsMovment.CurrencyTo.EditValue = CurrencyTo.EditValue
            FRMDebtorsMovment.ShowDialog()


        End If
    End Sub

    Private Sub GridView1_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        Dim CreditSum As New GridColumnSummaryItem()
        CreditSum.SummaryType = SummaryItemType.Sum
        CreditSum.FieldName = "Credit"
        GridView1.Columns("Credit").Summary.Add(CreditSum)
        Dim DebitSum As New GridColumnSummaryItem()
        DebitSum.SummaryType = SummaryItemType.Sum
        DebitSum.FieldName = "Debit"
        GridView1.Columns("Debit").Summary.Add(DebitSum)
        GridView1.Columns("Debit").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
        OverAllDebit.EditValue = GridView1.Columns("Debit").SummaryItem.SummaryValue
        OverAllDebit.Properties.Appearance.BackColor = Color.Red
        GridView1.Columns("Credit").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
        OverAllCredit.EditValue = GridView1.Columns("Credit").SummaryItem.SummaryValue

        OverAllTotal1.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
    End Sub

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GridView1.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", BranchID.EditValue)
            prm(1) = New SqlParameter("@CurrencyTo", CurrencyTo.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CreditAccountStatement", prm)
            If dt.Rows.Count > 0 Then

                Dim report As New RPTCustomerAccountStatement
                report.DataSource = dt
                report.DataAdapter = ""
                report.DataMember = "CreditAccountsTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'SQLCON.Close()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


End Class