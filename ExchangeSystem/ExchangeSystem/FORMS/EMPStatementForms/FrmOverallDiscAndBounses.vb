Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FrmOverallDiscAndBounses
    Sub LOADDATA()
        GCRole.DataSource = Nothing
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("DiscountsAndBounses_Statement", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFROMAT()
        End If
    End Sub
    'Sub LOADBRANCH()
    '    Dim dt As New DataTable
    '    dt.Clear()
    'dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit2")
    'If dt.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = dt
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.ShowHeader = False
    '    End If
    'End Sub

    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
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
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        'Dim gv As GridView = TryCast(sender, GridView)
        'If gv.GetRowCellValue(e.RowHandle, "النوع") = "خصم" Then
        '    If e.Column.FieldName = "خصم" Then
        '        e.Appearance.BackColor = Color.FromArgb(24, 37, 37, 1)
        '    Else
        '        e.Appearance.BackColor = Color.FromArgb(37, 224, 102, 1)
        '    End If
        'End If
        Dim View As GridView = TryCast(sender, GridView)
        If View.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
            If e.Column.FieldName = "النوع" Then
                If View.GetRowCellDisplayText(e.RowHandle, View.Columns("النوع")) = "علاوة" Then
                    e.Appearance.ForeColor = Color.White
                    e.Appearance.BackColor = Color.FromArgb(97, 166, 100)
                Else
                    e.Appearance.ForeColor = Color.White
                    e.Appearance.BackColor = Color.FromArgb(219, 71, 67)
                End If
            End If
        End If


        'Dim View As GridView = TryCast(sender, GridView)

        'If e.Column.FieldName Is "النوع" Then
        '    If View.GetRowCellDisplayText(e.RowHandle, View.Columns("علاوة")) Then
        '        e.Appearance.BackColor = Color.FromArgb(37, 224, 102, 1)
        '        e.Appearance.BackColor2 = Color.FromArgb(37, 224, 102, 1)
        '        If View.RowCount > 0 Then
        '            e.Appearance.BackColor = Color.FromArgb(37, 224, 102, 1)
        '            e.Appearance.BackColor2 = Color.FromArgb(37, 224, 102, 1)
        '            e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
        '        End If
        '    ElseIf View.GetRowCellDisplayText(e.RowHandle, View.Columns("خصم")) Then
        '        e.Appearance.BackColor = Color.FromArgb(24, 37, 37, 1)
        '        e.Appearance.BackColor2 = Color.FromArgb(24, 37, 37, 1)
        '        If View.RowCount > 0 Then
        '            e.Appearance.BackColor = Color.FromArgb(24, 37, 37, 1)
        '            e.Appearance.BackColor2 = Color.FromArgb(24, 37, 37, 1)
        '            e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
        '        End If

        '    End If
        'End If

    End Sub

    Private Sub FrmDiscountsLoadAllData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFROMAT()
        LOADBRNCHHasEmp(BranchID)
        D1.EditValue=Date.Now
        D2.EditValue = Date.Now
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        GCRole.DataSource = Nothing
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        GVRole.OptionsBehavior.Editable = False
        'FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
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
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        'BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LOADDATA()
    End Sub


    Sub Print()

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
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يجب اختيار الفرع"
                Return
            End If
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("DiscountsAndBounses_Statement", PR)
            If DT.Rows.Count > 0 Then
                Dim report As New XtraReport4
                report.DataSource = DT
                report.DataMember = "SalaryCalculationTb"
                report.FilterString = GVRole.ActiveFilterString
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'Else
                '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GCRole.DataSource = Nothing
    End Sub
End Class