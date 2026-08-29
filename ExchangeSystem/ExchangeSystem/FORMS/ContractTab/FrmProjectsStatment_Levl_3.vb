Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Base.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting.Shape.Native
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSpellChecker
Public Class FrmProjectsStatment_Levl_3
    Sub LOADDATA()
        GcRole.DataSource = Nothing

        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار المشروع"
            Return
        End If
        If ExpID.EditValue = -1 Or ExpID.Text = "" Then
            ExpID.ErrorText = "يجب اختيار المصروف"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            MsgBox("بداية الفترة لا يجب أن تكون أكبر من نهاية الفترة")
            Return
        End If
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@ProjectAccID", SqlDbType.BigInt) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PR(3) = New SqlParameter("@ExID", SqlDbType.Int) With {.Value = ExpID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_ProjectAccountStatement_Level_3", PR)
        If DT.Rows.Count > 0 Then
            GcRole.DataSource = DT
            DVGFormat(GVRole)
            If BranchID.EditValue = 0 Then
                GVRole.Columns("ProName").Visible = True
            Else
                GVRole.Columns("ProName").Visible = False
            End If
            If ExpID.EditValue = 0 Then
                GVRole.Columns("exn").Visible = True
            Else
                GVRole.Columns("exn").Visible = False
            End If
        End If
    End Sub

    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CONDB_ProjectTb_LoadDataIntoLKP")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "StockAccID"
            BranchID.Properties.DisplayMember = "ProName"
            BranchID.Properties.ShowHeader = False
            'dt.Rows(0).Delete()
            dt.Rows.Add("كل المشروعات", 0)
        End If
    End Sub
    Sub LOADEXID(ProjId As ULong)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ProjectAccID", SqlDbType.Int) With {.Value = ProjId}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_ExpensesTb_LoadToLKP_BasedOnProject", PR)
        If dt.Rows.Count > 0 Then
            ExpID.Properties.DataSource = dt
            ExpID.Properties.ValueMember = "AccID"
            ExpID.Properties.DisplayMember = "ExName"
            ExpID.Properties.ShowHeader = False
            dt.Rows.Add(0, "كل المصروفات")
        End If
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

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs)
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


    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("StockAccID").Visible = False
    End Sub
    Sub SumTotal()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If GVRole.RowCount > 0 Then
            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "SumDebit"
            GVRole.Columns("SumDebit").Summary.Add(OverallSalary)

            OverAllDebit.EditValue = 0.000
            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("SumDebit").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "SumCredit"
            GVRole.Columns("SumCredit").Summary.Add(OverallConstance)

            OverAllCredit.EditValue = 0.000
            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("SumCredit").SummaryItem.SummaryValue)
            '---------------------------------------------------
            OverAllTotal.EditValue = OverAllDebit.EditValue - OverAllCredit.EditValue
        End If
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        GcRole.DataSource = Nothing
        LOADDATA()
        SumTotal()
    End Sub

    Sub NewRecord()
        DVGFormat(GVRole)
        LOADBRANCH()
        GcRole.DataSource = Nothing
        D1.DateTime = New DateTime(DateTime.Now.Year, 1, 1)
        D2.DateTime = Date.Now
        GVRole.OptionsBehavior.Editable = False
    End Sub

    'Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

    '    Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
    '    XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
    '    Dim lookFeelError As New UserLookAndFeel(Me)
    '    lookFeelError.Style = LookAndFeelStyle.Skin
    '    lookFeelError.UseDefaultLookAndFeel = False
    '    lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
    '    XtraMessageBox.AllowCustomLookAndFeel = True
    '    If GVRole.RowCount = 0 Then
    '        XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Exit Sub
    '    End If
    '    Try
    '        If SQLCON.State = ConnectionState.Closed Then
    '            SQLCON.Open()
    '        End If
    '        Using cmd1 As SqlCommand = New SqlCommand("AccSafeActivityTb_GETEXPENSE")
    '            cmd1.CommandType = CommandType.StoredProcedure
    '            cmd1.Parameters.AddWithValue("@BranchID", BranchID.EditValue)
    '            cmd1.Parameters.AddWithValue("@D1", D1.EditValue)
    '            cmd1.Parameters.AddWithValue("@D2", D2.EditValue)
    '            cmd1.Connection = SQLCON
    '            Dim DA As New SqlDataAdapter(cmd1)
    '            Dim ds As New DataSet
    '            DA.Fill(ds)
    '            Using dr1 As SqlDataReader = cmd1.ExecuteReader()
    '                dr1.Read()
    '                If dr1.HasRows Then
    '                    Dim report As New RPTEXPESESMOVEMENTTATEMENTS
    '                    report.DataSource = ds
    '                    report.DataAdapter = DA
    '                    report.DataMember = "AccountsTb"
    '                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
    '                    report.CreateDocument()
    '                    report.ShowPreview()
    '                Else
    '                    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                End If
    '            End Using
    '        End Using
    '        If SQLCON.State = ConnectionState.Open Then
    '            SQLCON.Close()
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '    End Try
    'End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub

    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
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
    Private Sub GVRole_CustomUnboundColumnData_1(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GcRole.DataSource = Nothing
        ExpID.Properties.DataSource = Nothing
        ExpID.EditValue = -1
        OverAllCredit.EditValue = 0
        OverAllDebit.EditValue = 0
        OverAllTotal.EditValue = 0
        If BranchID.EditValue <> -1 And BranchID.Text <> String.Empty Then
            LOADEXID(BranchID.EditValue)
        End If
    End Sub

    Private Sub ExpID_EditValueChanged(sender As Object, e As EventArgs) Handles ExpID.EditValueChanged
        GcRole.DataSource = Nothing
        OverAllCredit.EditValue = 0
        OverAllDebit.EditValue = 0
        OverAllTotal.EditValue = 0
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار المشروع"
            Return
        End If
        If ExpID.EditValue = -1 Or ExpID.Text = "" Then
            ExpID.ErrorText = "يجب اختيار المصروف"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            MsgBox("بداية الفترة لا يجب أن تكون أكبر من نهاية الفترة")
            Return
        End If
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@ProjectAccID", SqlDbType.BigInt) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PR(3) = New SqlParameter("@ExID", SqlDbType.Int) With {.Value = ExpID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_ZPRT_ProjectAccountStatement_Level_3", PR)
        If DT.Rows.Count > 0 Then
            Dim report As New RPTProjectsStatment_Levl_3
            report.DataSource = DT
            report.DataMember = "AccountsTb"
            report.FilterString = GVRole.ActiveFilterString
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        End If
    End Sub
End Class