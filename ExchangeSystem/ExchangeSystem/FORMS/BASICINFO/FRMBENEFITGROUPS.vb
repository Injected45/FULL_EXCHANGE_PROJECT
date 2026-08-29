Imports System.Data.SqlClient
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports MetroFramework

Public Class FRMBENEFITGROUPS
    Dim IsUpdate As Boolean
    Sub DVGFormat()
        'GCROLE.LookAndFeel.UseDefaultLookAndFeel = False
        'GVROLE.BorderStyle = BorderStyles.NoBorder
        GVROLE.AddNewRow()
        GVROLE.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        GVROLE.OptionsBehavior.ReadOnly = True
        GVROLE.OptionsSelection.EnableAppearanceFocusedRow = False
        GVROLE.OptionsView.ShowIndicator = False
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsBehavior.Editable = False
        'GVROLE.NewItemRowText = ""
        'GVROLE.NewItemRowText = String.Empty
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.DarkGray
        GVROLE.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub addCatToDVG()
        Dim rowIdx As Integer = GVROLE.DataRowCount - 1
        For i As Integer = rowIdx To 0 Step -1
            Dim CellValue As Object = GVROLE.GetRowCellValue(i, "ISID")
            If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                GVROLE.DeleteRow(i)
            End If
        Next
        If NuID.EditValue = 0 Then
            NuID.ErrorText = "هذا الحقل المطلوب"
            Exit Sub
        End If
        If NuID.EditValue > GNum.EditValue Then
            NuID.ErrorText = "العدد لا يجب أن يكون أكبر من عدد الفروع"
            Exit Sub
        End If
        If NuRatio.EditValue = 0.000 Then
            NuRatio.ErrorText = "النسبة لا يجب أن تكون صفر"
            Exit Sub
        End If
        'If GVROLE.RowCount > GNum.EditValue Then
        '    MetroMessageBox.Show(Me, "عدد الصفوف لا يجب أن يكون أكبر من عدد الفروع", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.SetRowCellValue(GridControl.NewItemRowHandle, "ISID", CodeID.Text.Trim)
        GVROLE.SetRowCellValue(GridControl.NewItemRowHandle, "NumID", NuID.EditValue)
        GVROLE.SetRowCellValue(GridControl.NewItemRowHandle, "NumRatio", NuRatio.EditValue)

        DVGFormat()

        NuID.EditValue = 0
        NuRatio.EditValue = 0.00
        NuID.Select()

    End Sub
    Sub DeleteRow()
        Dim rowIdx As Integer = GVROLE.DataRowCount - 1
        For i As Integer = rowIdx To 0 Step -1
            Dim CellValue As Object = GVROLE.GetRowCellValue(i, "ISID")
            If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                GVROLE.DeleteRow(i)
            End If
        Next
        DVGFormat()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub SetData()
        If GName.Text = "" Then
            GName.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If GNum.EditValue = 0 Then
            GName.ErrorText = "هذا الحقل لا يجب أن تكون قيمته صفر"
            Exit Sub
        End If
        If GVROLE.RowCount <> GNum.EditValue Then
            MessageBox.Show(Me, "عدد الصفوف لا يجب أن يكون بنفس عدد الفروع", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
        PR(1) = New SqlParameter("@GName", SqlDbType.NVarChar, -1) With {.Value = GName.Text.Trim}
        PR(2) = New SqlParameter("@GNum", SqlDbType.TinyInt) With {.Value = GNum.EditValue}
        RUN_QUARY_PRO("BenefitGroupsTb_INSERT", PR)
        '=============================================
        For i As Integer = 0 To GVROLE.RowCount - 1
            Dim iscode As String = GVROLE.GetFocusedRowCellValue("ISID")
            Dim nuMID As Integer = Convert.ToInt32(GVROLE.GetRowCellValue(i, "NumID"))
            Dim nuMRID As Decimal = GVROLE.GetRowCellValue(i, "NumRatio")
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = CodeID.Text}
            PRM(1) = New SqlParameter("@NumID", SqlDbType.Int) With {.Value = nuMID}
            PRM(2) = New SqlParameter("@NumRatio", SqlDbType.Decimal) With {.Value = Convert.ToDecimal(GVROLE.GetRowCellValue(i, "NumRatio")), .Precision = 12, .Scale = 3}
            RUN_QUARY_PRO("BenefitGroupsDetailsTb_INSERT", PRM)
        Next
        '===============================================
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If GName.Text = "" Then
                GName.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            If GNum.EditValue = 0 Then
                GName.ErrorText = "هذا الحقل لا يجب أن تكون قيمته صفر"
                Exit Sub
            End If
            If GVROLE.RowCount <> GNum.EditValue Then
                MessageBox.Show(Me, "عدد الصفوف لا يجب أن يكون بنفس عدد الفروع", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
            PR(1) = New SqlParameter("@GName", SqlDbType.NVarChar, -1) With {.Value = GName.Text.Trim}
            PR(2) = New SqlParameter("@GNum", SqlDbType.TinyInt) With {.Value = GNum.EditValue}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("BenefitGroupsTb_UPDATE", PR)
            '=============================================
            Dim PRMM(0) As SqlParameter
            PRMM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
            Dim DTTT As New DataTable
            DTTT.Clear()
            DTTT = RUN_QUARY_PRO("BenefitGroupsDetailsTb_DELETEBEFOREUPDATE", PRMM)
            '===========================================

            For i As Integer = 0 To GVROLE.RowCount - 1
                Dim iscode As String = GVROLE.GetFocusedRowCellValue("ISID")
                Dim nuMID As Integer = Convert.ToInt32(GVROLE.GetRowCellValue(i, "NumID"))
                Dim nuMRID As Decimal = GVROLE.GetRowCellValue(i, "NumRatio")
                Dim PRM(2) As SqlParameter
                PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = CodeID.Text}
                PRM(1) = New SqlParameter("@NumID", SqlDbType.Int) With {.Value = nuMID}
                PRM(2) = New SqlParameter("@NumRatio", SqlDbType.Decimal) With {.Value = Convert.ToDecimal(GVROLE.GetRowCellValue(i, "NumRatio")), .Precision = 12, .Scale = 3}
                RUN_QUARY_PRO("BenefitGroupsDetailsTb_INSERT", PRM)
            Next
        End If
        '===============================================
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_RECROD_TO_UPDATE()
        If SEARCHTXT.Text <> String.Empty Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = SEARCHTXT.Text}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("BenefitGroupsTb_SEARCH", PRM)
            If DT.Rows.Count > 0 Then
                CodeID.Text = DT.Rows(0)("Code").ToString
                GName.Text = DT.Rows(0)("GName").ToString
                GNum.EditValue = DT.Rows(0)("GNum")
                IsUpdate = True
                BtnSave.Enabled = False
                BtnEdit.Enabled = True
            End If
        End If

    End Sub
    Sub NEWRECORD()
        CodeID.Text = GETMAXID("BenefitGroupsDetailsTb", "ID") + 1
        GName.Text = ""
        SEARCHTXT.Text = ""
        GNum.EditValue = 0
        NuID.EditValue = 0
        NuRatio.EditValue = 0.000
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        BtnDelete.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Dim bindlis As List(Of Entry) = New List(Of Entry)

        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCROLE.DataSource = binddata
        '===========================
        GCROLE.LookAndFeel.UseDefaultLookAndFeel = False
        GVROLE.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
    End Sub

    Private Sub NuRatio_Leave(sender As Object, e As EventArgs) Handles NuRatio.Leave
        'GVROLE_InitNewRow(Nothing, Nothing)
        addCatToDVG()
        NuID.Select()
    End Sub

    Private Sub FRMBENEFITGROUPS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub GVROLE_Click(sender As Object, e As EventArgs) Handles GVROLE.Click
        DeleteRow()

    End Sub

    Private Sub GVROLE_RowClick(sender As Object, e As RowClickEventArgs) Handles GVROLE.RowClick
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
    End Sub
    Private Sub GVROLE_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 151, 167), e.Bounds)
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
    Private Sub GVROLE_InitNewRow(sender As Object, e As InitNewRowEventArgs) Handles GVROLE.InitNewRow
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
        'GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        'GVROLE.OptionsView.ShowGroupPanel = False
        'GVROLE.AddNewRow()
        'GVROLE.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom
        'GVROLE.SetRowCellValue(GridControl.NewItemRowHandle, "ISID", CodeID.Text.Trim)
        'GVROLE.SetRowCellValue(GridControl.NewItemRowHandle, "NumID", NuID.EditValue)
        'GVROLE.SetRowCellValue(GridControl.NewItemRowHandle, "NumRatio", NuRatio.EditValue)

        'DVGFormat()

        'NuID.EditValue = 0
        'NuRatio.EditValue = 0.00
        'NuID.Select()
    End Sub

    Private Sub GName1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SEARCHTXT.KeyPress

    End Sub

    Private Sub GName1_KeyDown(sender As Object, e As KeyEventArgs) Handles SEARCHTXT.KeyDown
        If e.KeyCode = Keys.Enter Then
            SHOW_RECROD_TO_UPDATE()
            e.Handled = True
        End If
    End Sub

    Private Sub GVROLE_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GVROLE.RowCellClick
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
    End Sub

    Private Sub GVROLE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles GVROLE.KeyPress
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
    End Sub

    Private Sub GVROLE_KeyUp(sender As Object, e As KeyEventArgs) Handles GVROLE.KeyUp

    End Sub

    Private Sub GVROLE_GotFocus(sender As Object, e As EventArgs) Handles GVROLE.GotFocus
        Dim rowIdx As Integer = GVROLE.DataRowCount - 1
        For i As Integer = rowIdx To 0 Step -1
            Dim CellValue As Object = GVROLE.GetRowCellValue(i, "ISID")
            If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                GVROLE.DeleteRow(i)
            End If
        Next
    End Sub

    Private Sub GVROLE_DoubleClick(sender As Object, e As EventArgs) Handles GVROLE.DoubleClick
        'Dim rowIdx As Integer = GVROLE.DataRowCount - 1
        'For i As Integer = rowIdx To 0 Step -1
        '    Dim CellValue As Object = GVROLE.GetRowCellValue(i, "ISID")
        '    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
        '        GVROLE.DeleteRow(i)
        '    End If
        'Next
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVROLE.OptionsView.ShowGroupPanel = False
        DVGFormat()
    End Sub

    Private Sub SEARCHTXT_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles SEARCHTXT.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            SHOW_RECROD_TO_UPDATE()

        End If
    End Sub
End Class
Public Class Entry
    Public Property ISID() As String
    Public Property NumID() As Integer
    Public Property NumRatio() As Double

End Class
Public Class BENEFITGROUPSCLS

End Class