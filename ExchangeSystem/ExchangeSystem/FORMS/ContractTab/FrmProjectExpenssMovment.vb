Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Public Class FrmProjectExpenssMovment
    Sub LOADDATA()
        GcRole.DataSource = Nothing

        If ProjectID.EditValue = -1 Then
            ProjectID.ErrorText = "يجب اختيار المشروع"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            MsgBox("بداية الفترة لا يجب أن تكون أكبر من نهاية الفترة")
            Return
        End If
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@ProjectAccID", SqlDbType.BigInt) With {.Value = ProjectID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_ProjectAccountStatement_Level_2", PR)
        If DT.Rows.Count > 0 Then
            GcRole.DataSource = DT
            DVGFormat(GVRole)
        End If
    End Sub

    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CONDB_ProjectTb_LoadDataIntoLKP")
        If dt.Rows.Count > 0 Then
            ProjectID.Properties.DataSource = dt
            ProjectID.Properties.ValueMember = "StockAccID"
            ProjectID.Properties.DisplayMember = "ProName"
            ProjectID.Properties.ShowHeader = False
            dt.Rows.Add("كل المشروعات", 0)
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

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
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


    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ProjectID.QueryPopUp
        ProjectID.Properties.PopulateColumns()
        ProjectID.Properties.Columns("StockAccID").Visible = False
    End Sub
    Sub SumTotal()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If GVRole.RowCount > 0 Then
            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "Debit"
            GVRole.Columns("Debit").Summary.Add(OverallSalary)

            OverAllDebit.EditValue = 0.000
            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("Debit").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "Credit"
            GVRole.Columns("Credit").Summary.Add(OverallConstance)
            OverAllCredit.EditValue = 0.000
            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("Credit").SummaryItem.SummaryValue)
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
        ProjectCheckClosed()
    End Sub

    Sub NewRecord()
        DVGFormat(GVRole)
        LOADBRANCH()
        GcRole.DataSource = Nothing
        D1.DateTime = New DateTime(DateTime.Now.Year, 1, 1)
        D2.DateTime = Date.Now
        GVRole.OptionsBehavior.Editable = False
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If ProjectID.EditValue = -1 Then
            ProjectID.ErrorText = "يجب اختيار المشروع"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            MsgBox("بداية الفترة لا يجب أن تكون أكبر من نهاية الفترة")
            Return
        End If
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@ProjectAccID", SqlDbType.BigInt) With {.Value = ProjectID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_ZRPT_ProjectAccountStatement_Level_2", PR)
        If DT.Rows.Count > 0 Then
            Dim report As New RPTProjectExpenssMovment
            report.DataSource = DT
            report.DataMember = "AccountsTb"
            report.FilterString = GVRole.ActiveFilterString
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        End If
    End Sub

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

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        If GVRole.RowCount > 0 Then
            If GVRole.GetFocusedRowCellValue("Type") = 0 Then
                FrmProjectsStatment_Levl_3.NewRecord()
                FrmProjectsStatment_Levl_3.BranchID.EditValue = ProjectID.EditValue
                FrmProjectsStatment_Levl_3.LOADEXID(ProjectID.EditValue)
                FrmProjectsStatment_Levl_3.ExpID.EditValue = GVRole.GetFocusedRowCellValue("AccID")
                FrmProjectsStatment_Levl_3.LOADDATA()
                FrmProjectsStatment_Levl_3.SumTotal()
                FrmProjectsStatment_Levl_3.ShowDialog()
            Else
                ErrorMessage(Me, "رسالة خطأ", "عذرا يمكن عرض تفاصيل المصروفات فقط")
            End If
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles ProjectID.EditValueChanged
        GcRole.DataSource = Nothing
        OverAllCredit.EditValue = 0
        OverAllDebit.EditValue = 0
        OverAllTotal.EditValue = 0
    End Sub

    Private Sub SimpleButton12_Click(sender As Object, e As EventArgs) Handles BtnClosing.Click
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(1) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = ProjectID.EditValue}
        PRM(2) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
        PRM(3) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes.Text.Trim}
        PRM(4) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(5) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("CONDB_ProjectClosingTb_Insert", PRM)
        If PRM(4).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(5).Value.ToString)
            Exit Sub
        Else
            InfoMessage(Me, "رسالة معلومات", "تمت عملية الإقفال بنجاح")
        End If
    End Sub
    Sub ProjectCheckClosed()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = ProjectID.EditValue}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("CONDB_ProjectTb_CheckClosed", PR)
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("IsClosed") = True Then
                BtnClosing.Enabled = False
                Notes.Enabled = False
            Else
                BtnClosing.Enabled = True
                Notes.Enabled = True
            End If
        End If
    End Sub
End Class