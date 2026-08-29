Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Imports System.ComponentModel

Public Class FRMCURRENCYPRICE
    Dim clscp As New CLSCURRENCYPRICE
    Public IsUpdate As Boolean

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(26, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NEWRECORD()
        LOADCIDFROM()
        CodeID.Text = GETMAXID("CurrencyPricesTb", "ID") + 1
        InsertDate.EditValue = Date.Now
        CurrencyTo.EditValue = -1
        SPrice.EditValue = 0.000
        BPrice.EditValue = 0.000
        CurrencyPower.SelectedIndex = -1
        Dim bindlis As List(Of EnterCurrency) = New List(Of EnterCurrency)
        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Caption = "تاكيد سعر النشرة"
        DVGFormat()
        FormLocation(Me)
    End Sub
    'Sub LOADCIDFROM()
    '    Dim DT As New DataTable
    '    DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP")
    '    If DT.Rows.Count > 0 Then
    '        CurrencyFrom.Properties.DataSource = DT
    '        CurrencyFrom.Properties.ValueMember = "ID"
    '        CurrencyFrom.Properties.DisplayMember = "CuName"
    '        CurrencyFrom.Properties.ShowHeader = False
    '    Else
    '        CurrencyFrom.Properties.DataSource = Nothing
    '    End If
    'End Sub
    Sub LOADCIDFROM()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP_Dl")
        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "ID"
            CurrencyFrom.Properties.DisplayMember = "CuName"
            CurrencyFrom.Properties.ShowHeader = False
            CurrencyFrom.EditValue = DT.Rows(0)("ID")
        Else
            CurrencyFrom.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub CurrencyFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyFrom.QueryPopUp
        CurrencyFrom.Properties.PopulateColumns()
        CurrencyFrom.Properties.Columns("ID").Visible = False
    End Sub
    Sub LOADCIDTO()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_CurADDPrice")
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
            CurrencyTo.Properties.ShowHeader = False
            'CurrencyTo.Properties.Columns("ID").Visible = False
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub CurrencyTo_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyTo.QueryPopUp
        'If CurrencyTo.Properties.DataSource <> Nothing Then
        '    CurrencyTo.Properties.PopulateColumns()
        '    CurrencyTo.Properties.Columns("ID").Visible = False
        'End If

    End Sub
    Public Function CPDInsert() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("CPID")
        dt.Columns.Add("CIDFROM")
        dt.Columns.Add("CIDTO")
        dt.Columns.Add("SalePrice")
        dt.Columns.Add("BuyPrice")
        dt.Columns.Add("CurrencyPower")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "CurrencyIDFrom")
            If CellValue <> String.Empty Then
                dt.Rows.Add(CodeID.Text, GVRole.GetRowCellValue(i, "CIDFROM"), GVRole.GetRowCellValue(i, "CIDTO"), GVRole.GetRowCellValue(i, "SalePrice"),
                            GVRole.GetRowCellValue(i, "BuyPrice"), GVRole.GetRowCellValue(i, "CurrencyPowers"))
            End If
        Next
        Return dt
    End Function
    Public Function GCRole_seteing() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("CurrencyIDFrom")
        dt.Columns.Add("CurrencyIDTo")
        dt.Columns.Add("SalePrice")
        dt.Columns.Add("BuyPrice")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "CurrencyIDFrom")
            If CellValue <> String.Empty Then
                dt.Rows.Add(GVRole.GetRowCellValue(i, "CurrencyIDFrom"), GVRole.GetRowCellValue(i, "CurrencyIDTo"), GVRole.GetRowCellValue(i, "SalePrice"),
                             GVRole.GetRowCellValue(i, "BuyPrice").ToString)
            End If
        Next
        Return dt
    End Function
    Public Overrides Sub SetData()
        Dim dt As New DataTable
        dt.Clear()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True

        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookAndFeelError, "يجب اختيار عملة واحدة على الأقل", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        clscp.CURRENCYPRICE_Insert(CodeID.Text, CPDInsert, CurrencyPower.SelectedIndex, IsUpdate)
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
#Region "GVROLE"
    Sub DVGFormat()
        GVRole.AddNewRow()
        GVRole.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.True
        GVRole.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub addCatToDVG()
        If CurrencyFrom.Text = "" Then
            CurrencyFrom.ErrorText = "يرجى اختيار الطرف الأول من العملة"
            Exit Sub
        End If
        If CurrencyTo.Text = "" Then
            CurrencyTo.ErrorText = "يجب اختيار الطرف الثاني من العملة"
            Exit Sub
        End If
        If SPrice.EditValue <= 0.000 Then
            SPrice.ErrorText = "القيمة لا يجب أن تكون صرف أو أقل"
            Exit Sub
        End If
        If BPrice.EditValue <= 0.000 Then
            BPrice.ErrorText = "القيمة لا يجب أن تكون صرف أو أقل"
            Exit Sub
        End If
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyIDFrom", CurrencyFrom.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyIDTo", CurrencyTo.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BuyPrice", Convert.ToDecimal(BPrice.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "SalePrice", Convert.ToDecimal(SPrice.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CIDFROM", CurrencyFrom.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CIDTO", CurrencyTo.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyPowers", CurrencyPower.SelectedIndex)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyPower", CurrencyPower.Text)
        DVGFormat()
        CurrencyFrom.EditValue = -1
        CurrencyTo.EditValue = -1
        CurrencyPower.SelectedIndex = -1
        SPrice.EditValue = 0.000
        BPrice.EditValue = 0.000
        CurrencyFrom.Focus()
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

        If e.Column.FieldName Is "CurrencyIDFrom" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("CurrencyIDFrom"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(52, 69, 82)
                e.Appearance.BackColor2 = Color.FromArgb(52, 69, 82)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "Dele" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("Dele"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(232, Color.Aqua)
                e.Appearance.BackColor2 = Color.FromArgb(232, Color.Aqua)
            End If
        End If
    End Sub
    Private Sub GCRole_DoubleClick(sender As Object, e As EventArgs) Handles GCRole.DoubleClick
        If IsUpdate = False Then
            If GVRole.RowCount = 2 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "CurrencyIDFrom")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                CurrencyFrom.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub
    Private Sub GVRole_RowCountChanged(sender As Object, e As EventArgs) Handles GVRole.RowCountChanged
        For i As Integer = 0 To GVRole.RowCount - 1
            GVRole.SetRowCellValue(i, "SN", i + 1)
        Next
    End Sub
    Private Sub GCRole_Click(sender As Object, e As EventArgs) Handles GCRole.Click
        If IsUpdate = False Then
            If GVRole.RowCount > 0 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "CurrencyIDFrom")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                CurrencyFrom.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub
#End Region
    Private Sub SPrice_Leave(sender As Object, e As EventArgs) Handles SPrice.Leave
        addCatToDVG()
        CurrencyFrom.Focus()
    End Sub
    Private Sub CurrencyFrom_TextChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.TextChanged
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة الأولى"
            Exit Sub
        End If
        LOADCIDTO()
    End Sub
    Private Sub FRMCURRENCYPRICE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
End Class
Public Class EnterCurrency
    Public Property SN() As Integer
    Public Property CurrencyIDFrom() As String
    Public Property CurrencyIDTo() As String
    Public Property SalePrice() As Decimal
    Public Property BuyPrice() As Decimal
    Public Property CIDFROM() As Integer
    Public Property CIDTO() As Integer
    Public Property CurrencyPowers() As Boolean
    Public Property CurrencyPower() As String
End Class