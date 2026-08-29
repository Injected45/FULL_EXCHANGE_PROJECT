
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading

Public Class FRMselectACountes
    Public stodbemnt As Boolean
    Public IsUpdate As Boolean
    Public acclineselect As Integer
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        BranchIDd.Properties.DataSource = Nothing
        DT.Clear()
        DT.NewRow()
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("BName", GetType(String))


        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        DT.Rows.Add(0, "كل الفروع")
        If DT.Rows.Count > 0 Then
            BranchIDd.Properties.DataSource = DT
            BranchIDd.Properties.ValueMember = "DBRID"
            BranchIDd.Properties.DisplayMember = "BName"
            BranchIDd.Properties.PopulateColumns()
            BranchIDd.Properties.ShowHeader = False
        End If
        DT.Dispose()
    End Sub
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.GroupPanelText = ""
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            'Thread.CurrentThread.CurrentUICulture = CultureInfo
        Next
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchIDd.Enabled = dt.Rows(0)("Can_branch")
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            BranchIDd.EditValue = BID
        Else
            BranchIDd.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            BranchIDd.EditValue = BID
        End If
    End Sub
    Sub DVGFROMAT2()
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
    End Sub
    Public Sub loda()
        Try
            GridControl1.DataSource = Nothing
            If DT1.EditValue > DT2.EditValue Then
                DT1.ErrorText = "لا يمكن ان يكون التاريخ الاول اكبر من التاريخ الثاني"
                DT1.Focus()
                Exit Sub
            End If
            If DT1.Text = String.Empty Then
                DT1.ErrorText = "يرجا اختيار التاريخ"
                DT1.Focus()
                Exit Sub
            End If
            If DT2.Text = String.Empty Then
                DT2.ErrorText = "يرجا اختيار التاريخ"
                DT2.Focus()
                Exit Sub
            End If
            Dim dt As New DataTable
            dt.Clear()
            dt = ACCOUNTSTB_select_for(BranchIDd.EditValue)
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
                ACCOUNTSTB_select_Tota()
                DVGFROMAT()
            Else
                MessageBox.Show("لا يوجد بيانات خلال هذه الفترة", "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            'Thread.CurrentThread.CurrentUICulture = CultureInfo
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تبيه", ex.Message)
        End Try
        'DT1.Dispose()
    End Sub
    Public Function ACCOUNTSTB_select_for(BranchID As Integer) As DataTable
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@tybid", SqlDbType.Int) With {.Value = ACCTYP.SelectedIndex}
        PRM(1) = New SqlParameter("@dat1", SqlDbType.Date) With {.Value = DT1.EditValue}
        PRM(2) = New SqlParameter("@date2", SqlDbType.Date) With {.Value = DT2.EditValue}
        If ACCline.SelectedIndex = -1 Then
            acclineselect = 0
        Else
            acclineselect = ACCline.SelectedIndex
        End If
        PRM(3) = New SqlParameter("@accline", SqlDbType.TinyInt) With {.Value = acclineselect}
        PRM(4) = New SqlParameter("@BrnchID", SqlDbType.TinyInt) With {.Value = BranchID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ACCOUNTSTB_select_for", PRM)
        Return DT
    End Function
    Private Sub BranchIDd_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchIDd.QueryPopUp
        BranchIDd.Properties.PopulateColumns()
        BranchIDd.Properties.Columns("DBRID").Visible = False
    End Sub
    Private Sub FRMselectACountes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRANCH()
        DT1.EditValue = Date.Now
        DT2.EditValue = Date.Now
        GridControl1.DataSource = Nothing
        ACCTYP.SelectedIndex = 1
        ACCCMD1.SelectedIndex = 1
        BranchIDd.EditValue = BID
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub
    Private Sub DT2_Leave(sender As Object, e As EventArgs) Handles DT2.Leave
        If DT1.EditValue > DT2.EditValue Then
            DT1.ErrorText = "لا يمكن ان يكون التاريخ الاول اكبر من التاريخ الثاني"
            DT1.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub SimpleButton1_Click_1(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        loda()
        SumTotsl()
    End Sub

    Private Sub ACCTYP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ACCTYP.SelectedIndexChanged
        If ACCTYP.SelectedIndex = 0 Then
            ACCline.Enabled = True
            ACCline.SelectedIndex = 0
        ElseIf ACCTYP.SelectedIndex = 1 Then
            ACCline.Enabled = False
            ACCline.SelectedIndex = -1
        End If
    End Sub

    Public Sub ACCOUNTSTB_select_Tota()
        GridControl2.DataSource = Nothing
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@dat1", SqlDbType.Date) With {.Value = DT1.EditValue}
        PRM(1) = New SqlParameter("@date2", SqlDbType.Date) With {.Value = DT2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[ACCOUNTSTB_select_Total]", PRM)
        If DT.Rows.Count > 0 Then
            GridControl2.DataSource = DT
            DVGFROMAT2()
        Else
            ErrorMessage(Me, "رسالة تبيه", "لايوجد بيانات خلال هذه الفتره ")
        End If
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Try
            If GVRole.RowCount > 0 Then
                Dim PRM(8) As SqlParameter
                PRM(0) = New SqlParameter("@ACCCODE", SqlDbType.BigInt) With {.Value = GVRole.GetFocusedRowCellValue("AccID")}
                PRM(1) = New SqlParameter("@date1", SqlDbType.Date) With {.Value = DT1.EditValue}
                PRM(2) = New SqlParameter("@date2", SqlDbType.Date) With {.Value = DT2.EditValue}
                PRM(3) = New SqlParameter("@smblabecredetl", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(4) = New SqlParameter("@sumlabeldebit", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(5) = New SqlParameter("@lbealtotal", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(6) = New SqlParameter("@totalacount", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(7) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = 1}
                PRM(8) = New SqlParameter("@BranchID", SqlDbType.NChar) With {.Value = BranchIDd.EditValue}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("[dbo].[ACCOUNTSTB_SelectICOUNTDetelles2]", PRM)
                If dt.Rows.Count > 0 Then
                    FRMaccounDEtells.ACCCODE.Tag = GVRole.GetFocusedRowCellValue("AccID")
                    FRMaccounDEtells.DT1.EditValue = DT1.EditValue
                    FRMaccounDEtells.dt2.EditValue = DT2.EditValue
                    FRMaccounDEtells.loaddate(GVRole.GetFocusedRowCellValue("AccID"), 1)
                    If FRMaccounDEtells.Visible = True Then FRMaccounDEtells.Visible = False
                    FRMaccounDEtells.Show()
                Else
                    FRMaccounDEtells.DT1.EditValue = DT1.EditValue
                    FRMaccounDEtells.dt2.EditValue = DT2.EditValue
                    FRMaccounDEtells.ACCCODE.Tag = GVRole.GetFocusedRowCellValue("AccID")
                    FRMaccounDEtells.ACCCODE.EditValue = GVRole.GetFocusedRowCellValue("AccCode")
                    FRMaccounDEtells.ACCNAME.Text = GVRole.GetFocusedRowCellValue("AccName")
                    FRMaccounDEtells.DT1.Select()
                    If FRMaccounDEtells.Visible = True Then FRMaccounDEtells.Visible = False
                    FRMaccounDEtells.Show()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ في برنامج", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub PRINTRPT(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GVRole.RowCount = 0 Then
            InfoMessage(Me, "رسالة تبيه", "لا يوجد بيانات لطباعتها")
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@tybid", ACCTYP.SelectedIndex)
            PRM(1) = New SqlParameter("@dat1", DT1.EditValue)
            PRM(2) = New SqlParameter("@date2", DT2.EditValue)
            PRM(3) = New SqlParameter("@accline", acclineselect)
            PRM(4) = New SqlParameter("@BrnchID", BranchIDd.EditValue)
            Dim ds As New DataSet
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_ACCOUNTSTB_select_for", PRM)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                dt.TableName = "ACCOUNTSTB"
                ds.Tables.Add(dt)
                Dim report As New RPTselectACountes
                report.DataSource = ds
                report.DataMember = "ACCOUNTSTB"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تبيه", ex.Message)
        End Try
    End Sub
    Sub SumTotsl()
        OverallCredit.EditValue = 0.000
        OverallDepit.EditValue = 0.000
        If GVRole.RowCount > 0 Then

            Dim OverallVal As New GridColumnSummaryItem()
            OverallVal.SummaryType = SummaryItemType.Sum
            OverallVal.FieldName = "Noetes"
            GVRole.Columns("Noetes").Summary.Add(OverallVal)
            Dim ExVal As New GridColumnSummaryItem()
            ExVal.SummaryType = SummaryItemType.Sum
            ExVal.FieldName = "total"
            GVRole.Columns("total").Summary.Add(ExVal)
            OverallCredit.EditValue = Convert.ToDouble(GVRole.Columns("total").SummaryItem.SummaryValue)
            OverallDepit.EditValue = Convert.ToDouble(GVRole.Columns("Noetes").SummaryItem.SummaryValue)
        End If
    End Sub

    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotsl()
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotsl()
    End Sub
End Class