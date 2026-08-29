Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.DataAccess.Sql
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class View_RecieveInternalEx



    Dim DT As New DataTable

    Sub LoadData()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@RecievedBranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(1) = New SqlParameter("@UNAME", SqlDbType.Int) With {.Value = CUST1.EditValue}
        PRM(2) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(3) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(4) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = TransType.SelectedIndex}
        DT.Clear()
        DT = RUN_QUARY_PRO("ItarnalAndExtarnalEX_ShowALLRecords", PRM)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("القيمة").AppearanceCell.BackColor = Color.FromArgb(0, 128, 43)
            GVRole.Columns("العمولة").AppearanceCell.BackColor = Color.FromArgb(0, 153, 204)
        End If
    End Sub

    Sub DVGFROMAT()

        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AllowFindPanel = True
        GVRole.GroupPanelText = ""
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        GVRole.ShowFindPanel()

        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10.5, FontStyle.Regular)
        Next

        GVRole.Appearance.EvenRow.BackColor = Color.White
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
        GVRole.Columns("المرسل").Width = 220
        GVRole.Columns("المستلم").Width = 220
        'GVRole.Columns("#").Width = 30
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 153, 153), e.Bounds)
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
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        'Dim View As GridView = TryCast(sender, GridView)
        Dim view As GridView = TryCast(sender, GridView)
        If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
            If e.Column.FieldName = "القيمة" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.FromArgb(0, 128, 43)
            End If
            If e.Column.FieldName = "العمولة" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.FromArgb(0, 153, 204)
            End If
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        GVRole.ActiveFilter.Clear()
        XtraMessageBox.AllowCustomLookAndFeel = True
        GCRole.DataSource = Nothing
        OverallVal1.EditValue = 0.000
        ExValtotal.EditValue = 0.000
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب إختيار الفرع"
            Return
        End If
        If CUST1.EditValue = -1 Then
            CUST1.ErrorText = "يجب إختيار الموظف"
            Return
        End If
        If D1.EditValue Is Nothing Then
            D1.ErrorText = "يجب إختيار التاريخ"
            Return
        End If
        If D2.EditValue Is Nothing Then
            D2.ErrorText = "يجب إختيار التاريخ"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية"
            Return
        End If
        If TransType.SelectedIndex = -1 Then
            TransType.ErrorText = "يجب إختيار نوع التحويل"
            Return
        End If
        LoadData()
        If GVRole.RowCount > 0 Then
            DVGFROMAT()
        Else
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub


    Sub LOADBRNACH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit1")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
        DT.Rows.Add(0, "كل الفروع")
    End Sub
    Sub Loadusers()


        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        Dim DT1 As New DataTable
        DT1.Clear()
        DT1 = RUN_QUARY_PRO("TB_Users_LoadUSERBYBRANCHANDALLBRANCH", PRM)
        CUST1.Properties.DataSource = DT1
        CUST1.Properties.ValueMember = "USID"
        CUST1.Properties.DisplayMember = "UName"
        CUST1.Properties.ShowHeader = False
        DT1.Rows.Add("كل الموظفين", 0)
    End Sub


    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False

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

            If dt.Rows(0)("Can_branch") = 0 Then
                CUST1.Enabled = dt.Rows(0)("Can_safID")
                CUST1.EditValue = userID
            Else
                CUST1.Enabled = dt.Rows(0)("Can_branch")
            End If

            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            CUST1.Enabled = False
            CUST1.EditValue = userID
            BranchID.EditValue = BID
        End If
    End Sub

    Private Sub View_RecieveInternalEx_Load(sender As Object, e As EventArgs) Handles Me.Load
        'GVRole.OptionsBehavior.Editable = False
        LOADBRNACH()
        BranchID.EditValue = BID
        Loadusers()
        CUST1.EditValue = UserID
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        GCRole.DataSource = Nothing
        OverallVal1.EditValue = 0.000
        ExValtotal.EditValue = 0.000

        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 62)


    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        Loadusers()

    End Sub

    Private Sub CUST1_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CUST1.QueryPopUp
        CUST1.Properties.PopulateColumns()
        CUST1.Properties.Columns("USID").Visible = False
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

            Dim Quary As String

            If TransType.SelectedIndex = 2 Then
                Quary = "ItarnalAndExtarnalEX_ShowALLRecords"
            End If
            If TransType.SelectedIndex = 1 Then
                Quary = "ExtarnalEX_ShowALLRecords"
            End If
            If TransType.SelectedIndex = 0 Then
                Quary = "InternalExValues_ShowALLRecords"
            End If
            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@RecievedBranchID", BranchID.EditValue)
            PRM(1) = New SqlParameter("@UNAME", CUST1.EditValue)
            PRM(2) = New SqlParameter("@D1", D1.EditValue)
            PRM(3) = New SqlParameter("@D2", D2.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO(Quary, PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "InternalEx"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTView_RecieveInternalEx

                report.DataSource = ds
                report.DataMember = "InternalEx"
                report.FilterString = GVRole.ActiveFilterString
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        If GVRole.RowCount > 0 Then
            Dim OverallVal As New GridColumnSummaryItem()
            OverallVal.SummaryType = SummaryItemType.Sum
            OverallVal.FieldName = "القيمة"
            GVRole.Columns("القيمة").Summary.Add(OverallVal)
            Dim ExVal As New GridColumnSummaryItem()
            ExVal.SummaryType = SummaryItemType.Sum
            ExVal.FieldName = "العمولة"
            GVRole.Columns("العمولة").Summary.Add(ExVal)
            GVRole.Columns("العمولة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            ExValtotal.EditValue = Convert.ToDouble(GVRole.Columns("العمولة").SummaryItem.SummaryValue)
            GVRole.Columns("القيمة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallVal1.EditValue = Convert.ToDouble(GVRole.Columns("القيمة").SummaryItem.SummaryValue)
        End If
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        Dim OverallVal As New GridColumnSummaryItem()
        OverallVal.SummaryType = SummaryItemType.Sum
        OverallVal.FieldName = "القيمة"
        GVRole.Columns("القيمة").Summary.Add(OverallVal)
        Dim ExVal As New GridColumnSummaryItem()
        ExVal.SummaryType = SummaryItemType.Sum
        ExVal.FieldName = "العمولة"
        GVRole.Columns("العمولة").Summary.Add(ExVal)
        ExValtotal.EditValue = Convert.ToDouble(GVRole.Columns("العمولة").SummaryItem.SummaryValue)
        OverallVal1.EditValue = Convert.ToDouble(GVRole.Columns("القيمة").SummaryItem.SummaryValue)
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub TransType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TransType.SelectedIndexChanged
        GCRole.DataSource = Nothing
    End Sub
End Class
