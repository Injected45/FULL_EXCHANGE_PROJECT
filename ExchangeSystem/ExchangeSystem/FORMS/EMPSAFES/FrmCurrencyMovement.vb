Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmCurrencyMovement
    Sub LOADDATA()
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon



        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True

        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        PRM(1) = New SqlParameter("@CurrencyID", SqlDbType.Int)
        PRM(1).Value = CurrencyID.EditValue
        PRM(2) = New SqlParameter("@InsertDateFrom", SqlDbType.Date)
        PRM(2).Value = D1.EditValue
        PRM(3) = New SqlParameter("@InsertDateTo", SqlDbType.Date)
        PRM(3).Value = D2.EditValue
        Dim SumDebit As SqlParameter = New SqlParameter
        PRM(4) = New SqlParameter("@SumDebit", SqlDbType.Decimal)
        PRM(4).Direction = ParameterDirection.Output

        'PRM(5).Direction = ParameterDirection.Output



        Dim SumCredit As SqlParameter = New SqlParameter
        PRM(5) = New SqlParameter("@SumCredit", SqlDbType.Decimal)
        PRM(5).Direction = ParameterDirection.Output

        'PRM(5).Value = New SqlParameter("@SumCredit", SqlDbType.Decimal)
        'OverAllDebit.EditValue = 'PRM(5).Value
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("DailyCloseTb_LOADCURRENCYMOVEMENT", PRM)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            OverAllDebit.EditValue = PRM(4).Value
            OverAllCredit.EditValue = PRM(5).Value
            DVGFROMAT()
        Else
            XtraMessageBox.Show(lookAndFeelError, "لا يوجد بيانات لعرضها خلال الفترة المختارة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
    Sub BranchToLKP()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADCURRENCY()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CURRENCYTB_LoadToLKPSafeID")
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CurrencyName"
            CurrencyID.Properties.PopulateColumns()
            CurrencyID.Properties.ShowHeader = False
        End If
    End Sub
    Sub NEWRECORD()
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        BranchToLKP()
        LOADCURRENCY()
        OverAllCredit.Properties.Buttons(0).Visible = False
        OverAllDebit.Properties.Buttons(0).Visible = False
        BranchID.EditValue = -1
        CurrencyID.EditValue = -1
        DVGFROMAT()
        If UserType = 1 Then
            BranchID.Enabled = True
        Else
            BranchID.Enabled = False


        End If
    End Sub

    Private Sub FrmCurrencyMovement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True


        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى اختيار الفرع أولاً"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Then
            CurrencyID.ErrorText = "يرجى اختيار العملة أولاً"
            Exit Sub
        End If
        If D1.EditValue > D2.EditValue Then
            XtraMessageBox.Show(lookAndFeelError, "تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        LOADDATA()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
        CurrencyID.Properties.Columns("SafeCurID").Visible = False
    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
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
        Dim View As GridView = TryCast(sender, GridView)

        If e.Column.FieldName Is "#" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("#"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(231, 72, 86)
                e.Appearance.BackColor2 = Color.FromArgb(231, 72, 86)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If

    End Sub

    Private Sub GVRole_RowStyle(sender As Object, e As RowStyleEventArgs) Handles GVRole.RowStyle

    End Sub
End Class