Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRMExpenseInquiry
    Public IsUpdate, UpdateBySalary As Boolean
    Public AcID, IDCode, AccCode, AccEm, CodeID, ExpenseID As ULong
    Sub LOADDATA()


        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If AccIDEX.EditValue = -1 Or AccIDEX.Text = String.Empty Then
            AccIDEX.ErrorText = "يجب اختيار المصروف"
            Exit Sub
        End If
        If D1.EditValue > D2.EditValue Then
            MsgBox("بداية الفترة لا يجب أن تكون أكبر من نهاية الفترة")
            Return
        End If
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PR(3) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccIDEX.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccSafeActivityTb_GETByEXPENSE", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("طبيعة الحركة").Width = 400
            DVGFROMAT()
            'Else
            '    GCRole.DataSource = Nothing
            '    GVRole.Columns.Clear()
        End If
    End Sub
    'Sub LOADBRANCH()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
    '    If dt.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = dt
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.ShowHeader = False
    '    End If
    'End Sub

    Sub LOADEXPANSTYPE()
        'AccIDEX.Properties.DataSource = Nothing
        AccIDEX.EditValue = -1
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.DataSource = dt
                AccIDEX.Properties.ValueMember = "AccID"
                AccIDEX.Properties.DisplayMember = "ExName"
                AccIDEX.Properties.ShowHeader = False
            Else
                AccIDEX.Properties.DataSource = Nothing
            End If
        Else
            AccIDEX.Enabled = False
            AccIDEX.Properties.DataSource = Nothing
        End If
    End Sub
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
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
        BranchID.Properties.Columns("DBRID").Visible = False

    End Sub
    Sub SumTotal()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If GVRole.RowCount > 0 Then
            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "مدين"
            GVRole.Columns("مدين").Summary.Add(OverallSalary)

            OverAllDebit.EditValue = 0.000
            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("مدين").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "دائن"
            GVRole.Columns("دائن").Summary.Add(OverallConstance)

            OverAllCredit.EditValue = 0.000
            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن").SummaryItem.SummaryValue)
            '---------------------------------------------------
            OverAllTotal.EditValue = OverAllDebit.EditValue - OverAllTotal.EditValue
        End If
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click

        LOADDATA()
        SumTotal()

    End Sub

    Private Sub FRMExpenseInquiry_Load(sender As Object, e As EventArgs) Handles Me.Load
        DVGFROMAT()
        LOADBRNCHDIERCT(BranchID)
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        GCRole.DataSource = Nothing
        BranchID.EditValue = -1
        D1.DateTime = Date.Now
        D2.DateTime = Date.Now
        GVRole.OptionsBehavior.Editable = False
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
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


    Private Sub AccID_TextChanged(sender As Object, e As EventArgs) Handles AccIDEX.TextChanged
        OverAllCredit.EditValue= 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ExName", SqlDbType.NVarChar, -1) With {.Value = AccIDEX.Text}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ExpensesTb_SelectID", PR)
        If dt.Rows.Count > 0 Then
            ExpenseID = dt.Rows(0)("ID")
        End If
        If IsUpdate = False Then

            Dim rowIdx As Integer = GVRole.DataRowCount - 1
            For i As Integer = rowIdx To 0 Step -1
                Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
                If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                    GVRole.DeleteRow(i)
                End If
            Next
        End If
    End Sub

    Private Sub AccID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccIDEX.QueryPopUp
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ExpensesTb_LOADTOLKPBasedOnBranchID", PR)
        If dt.Rows.Count > 0 Then
            AccIDEX.Properties.PopulateColumns()
            AccIDEX.Properties.Columns("AccID").Visible = False
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            LOADEXPANSTYPE()
        End If
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
            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", BranchID.EditValue)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            PRM(3) = New SqlParameter("@AccID", AccIDEX.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_GETByEXPENSE", PRM)
            Dim ds As New DataSet
            dt.TableName = "AccountsTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTExpenseInquiry
                report.DataSource = ds
                report.DataMember = "AccountsTb"
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

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
End Class