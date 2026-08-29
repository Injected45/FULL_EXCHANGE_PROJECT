Imports DevExpress
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Tile
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMSaleCurrencyForCUST
    Sub NEWRECORD()
        'LOADBRANCH()
        'BranchID.EditValue = BID
        NatNumber.EditValue = -1
        CurrencyID.EditValue = -1
        CustName.Text = ""
        'D1.EditValue = Date.Now
        'D2.EditValue = Date.Now
        NEWDVGFROMAT(GVRole)

        LOADNATNumber()
    End Sub
    'Sub LOADBRANCH()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_TXT("CoBranches_LoadconnectedBranch")
    '    If dt.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = dt
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.ShowHeader = False
    '    End If
    'End Sub
    Sub LOADCIDFROMT()
        If NatNumber.Text <> String.Empty Then
            Dim DT As New DataTable
        Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@CustIDNo", SqlDbType.NVarChar, 50) With {.Value = NatNumber.Text}
            DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOLKP_BasedonNatNumber", prm)
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CuName"
            CurrencyID.Properties.ShowHeader = False
            CurrencyID.Properties.PopulateColumns()
            CurrencyID.Properties.Columns("ID").Visible = False
        Else
            CurrencyID.Properties.DataSource = Nothing
        End If
        End If
    End Sub
    Public Sub LOADNATNumber()


        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CurrenciesBuyandsellTB_LoadNatNumber")
        If dt.Rows.Count > 0 Then
            NatNumber.Properties.DataSource = dt
            NatNumber.Properties.ValueMember = "CustIDNo"
            NatNumber.Properties.DisplayMember = "CustIDNo"
            NatNumber.Properties.ShowHeader = False
        End If

    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False


        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        'GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        'GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(2, 84, 100), e.Bounds)
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


    Public Sub CurrencymovementPruse_FILLGRid()
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "الرجاء اختيار العملة "
            Return
        End If
        'If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
        '    BranchID.ErrorText = " هذا الحقل مطلوب"
        '    Return
        'End If
        If NatNumber.Text = String.Empty Then
            NatNumber.ErrorText = " هذا الحقل مطلوب"
            Return
        End If

        'If D1.Text = String.Empty Then
        '    D1.ErrorText = " هذا الحقل مطلوب"
        '    Return
        'End If


        'If D2.Text = String.Empty Then
        '    D2.ErrorText = " هذا الحقل مطلوب"
        '    Return
        'End If


        'If D1.EditValue > D2.EditValue Then
        '    D1.ErrorText = "عذرا يجب ان يكون التاريخ الاول اصغر او يساوي التاريخ الثاني"
        '    Return
        'End If

        GridControl2.DataSource = Nothing

        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@AccID", SqlDbType.NVarChar, 50) With {.Value = NatNumber.EditValue}
        'prm(1) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = D1.EditValue}
        'prm(2) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = D2.EditValue}
        prm(1) = New SqlParameter("@CurrID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CurrencymovementCust_SalelimtedStatment", prm)

        If dt.Rows.Count > 0 Then
            GridControl2.DataSource = dt
            CustName.Text = dt.Rows(0)("CusName")
            DVGFormat()
        Else
            MessageBox.Show("عذرا لايوجد بيانات في الوقت الحالي ", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    'Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs)
    '    BranchID.Properties.PopulateColumns()
    '    BranchID.Properties.Columns("DBRID").Visible = False
    'End Sub

    Private Sub FRMSaleCurrencyForCUST_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    'Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs)
    '    If BranchID.Text <> String.Empty And BranchID.EditValue <> -1 Then
    '        LOADCUST_WITHBRANCH2(BranchID.EditValue, NatNumber)
    '    End If
    'End Sub

    'Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles NatNumber.QueryPopUp
    '    NatNumber.Properties.PopulateColumns()
    '    NatNumber.Properties.Columns("CusName").Visible = False
    'End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        CurrencymovementPruse_FILLGRid()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            If GVRole.RowCount = 0 Then
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@AccID", SqlDbType.NVarChar, 50) With {.Value = NatNumber.EditValue}
            'prm(1) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = D1.EditValue}
            'prm(2) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(1) = New SqlParameter("@CurrID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CurrencymovementCust_SalelimtedStatment", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTSaleCurrencyForCUST
                report.DataSource = dt
                'report.DataAdapter = DA
                report.DataMember = "AccSafeActivityTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub NatNumber_EditValueChanged(sender As Object, e As EventArgs) Handles NatNumber.EditValueChanged
        GridControl2.DataSource = Nothing
        CurrencyID.Properties.DataSource = Nothing
        CurrencyID.EditValue = -1
        CustName.Text = ""
        LOADCIDFROMT()
    End Sub

    Private Sub CurrencyID_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyID.EditValueChanged
        GridControl2.DataSource = Nothing
    End Sub
End Class