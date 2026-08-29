Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid

Public Class NewBasicCurrencyPrcie
    Dim clscp As New CLSNEWOWNCURRENCYPRICE
    Public IsUpdate, IsAgent As Boolean
    Public VAgentID, LKBANKID, LKACCTYPE, SerOrAgentID As Integer
    Public PRCTYPE, ACCTYPE As Byte


    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(30, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NEWRECORD()
        LoadFirstCurrency()
        LoadCountry()
        CodeID.Text = GETMAXID("NewCurrencyPricesOwnTb", "ID") + 1
        InsertDate.EditValue = Date.Now
        SPrice.EditValue = 0.000
        BPrice.EditValue = 0.000
        If PriceType.SelectedIndex = 0 Or PriceType.SelectedIndex = 1 Then
            BranchID.Enabled = False
            'BankID.Enabled = False
            AccountType.Enabled = False
        ElseIf PriceType.SelectedIndex = 2 Then
            AccountType.Enabled = True
            If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Then
                BranchID.Enabled = True
            End If
            'BankID.Enabled = False
        ElseIf PriceType.SelectedIndex = 3 Then
            BranchID.Enabled = False
            AccountType.Enabled = False
            'BankID.Enabled = True
        End If
        CountryID.EditValue = -1
        PriceType.SelectedIndex = -1
        BankID.EditValue = -1
        AccountType.SelectedIndex = -1
        BranchID.EditValue = -1
        CurrencyFrom.EditValue = -1
        CurrencyTo.EditValue = -1
        CurrencyPower.SelectedIndex = -1
        Dim bindlis As List(Of EnterCurrency) = New List(Of EnterCurrency)
        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
        FormLocation(Me)
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
        If CurrencyPower.SelectedIndex < 0 Then
            ErrorMessage(Me, "رسالة خطأ", "يجب اختيار قوة العملة")
            Exit Sub
        End If
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyIDFrom", CurrencyFrom.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyIDTo", CurrencyTo.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BuyPrice", Convert.ToDecimal(BPrice.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "SalePrice", Convert.ToDecimal(SPrice.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CurrencyPowers", CurrencyPower.SelectedIndex)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CIDFROM", CurrencyFrom.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "CIDTO", CurrencyTo.EditValue)
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
#Region "LOADCONTROLS"
    Public Sub LoadCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            NEWDVGFROMAT(CountryGV)
        End If
    End Sub
    Public Sub LoadAgent()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        If AccountType.SelectedIndex = 1 Then
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LOADOutSideAgentTOGLKPWITHCOUNTRYOrInsideAgent", PR)
            If DT.Rows.Count > 0 Then
                BranchID.Properties.DataSource = DT
                BranchID.Properties.ValueMember = "DBRID"
                BranchID.Properties.DisplayMember = "BName"
                NEWDVGFROMAT(AgentGV)
            Else
                BranchID.Properties.DataSource = Nothing
            End If
        ElseIf AccountType.SelectedIndex = 2 Then
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPNotSelectCOUNTRY", PR)
            If DTT.Rows.Count > 0 Then
                BranchID.Properties.DataSource = DTT
                BranchID.Properties.ValueMember = "DBRID"
                BranchID.Properties.DisplayMember = "BName"
                NEWDVGFROMAT(AgentGV)
            Else
                BranchID.Properties.DataSource = Nothing
            End If
        ElseIf AccountType.SelectedIndex = 0 Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int)
            PRM(0).Value = CountryID.EditValue
            PRM(1) = New SqlParameter("@TransType", SqlDbType.Int)
            PRM(1).Value = 0
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPWITHTransType", PRM)
            If DT.Rows.Count > 0 Then
                BranchID.Properties.DataSource = DT
                BranchID.Properties.ValueMember = "DBRID"
                BranchID.Properties.DisplayMember = "BName"
                NEWDVGFROMAT(AgentGV)
            Else
                BranchID.Properties.DataSource = Nothing
            End If
        End If
    End Sub
    Public Sub LoadFirstCurrency()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_FCurrGV")
        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "CurrID"
            CurrencyFrom.Properties.DisplayMember = "CurrencyName"
            NEWDVGFROMAT(FCurrGV)
        Else
            CurrencyFrom.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LoadSecondCurrency()
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة الأولى"
            Exit Sub
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("CurrencyMainTb_SCurrGVNOTEXIST", PR)
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "CurrID2"
            CurrencyTo.Properties.DisplayMember = "CurrencyName2"
            NEWDVGFROMAT(SCurrGV)
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LoadBank()
        BankID.Properties.DataSource = Nothing
        BankID.EditValue = -1
        If IsEmpty(CountryID) Or IsEmpty(PriceType) Then Exit Sub
        If PriceType.SelectedIndex = 2 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
            LoadToControlar(BankID, "TransTypeTb_SelectByCountry", "SRNAME", "SRID", PR)
        Else
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@CountryId", SqlDbType.Int)
            PRM(0).Value = SafeToInt(CountryID.EditValue)
            LoadToControlar(BankID, "BanksTb_LOADTOLKP_2026", "BankName", "BNKID", PRM, True, "نقدا")
        End If
    End Sub
#End Region
    Private Sub PriceType_TextChanged(sender As Object, e As EventArgs) Handles PriceType.TextChanged
        BankID.EditValue = -1
        LoadBank()
        If PriceType.SelectedIndex = 0 Then
            BranchID.Enabled = False
            'BankID.Enabled = True
            AccountType.Enabled = False
        ElseIf PriceType.SelectedIndex = 1 Then
            BranchID.Enabled = True
            'BankID.Enabled = True
            AccountType.SelectedIndex = 0
            AccountType.Enabled = False
            LoadAgent()
        ElseIf PriceType.SelectedIndex = 2 Then
            AccountType.Enabled = True
            If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Then
                LoadAgent()
                BranchID.Enabled = True
                VAgentID = BranchID.EditValue
            Else
                BranchID.Enabled = False
                VAgentID = 0
            End If
            'BankID.Enabled = False
        ElseIf PriceType.SelectedIndex = 3 Then
            BranchID.Enabled = False
            AccountType.Enabled = False
            'BankID.Enabled = True

        End If
        If PriceType.SelectedIndex = 0 Then
            IsAgent = False
            VAgentID = 0
        Else
            IsAgent = True
            VAgentID = BranchID.EditValue
        End If
        If PriceType.SelectedIndex = 3 Then
            LKBANKID = 0
        Else
            LKBANKID = BankID.EditValue
        End If
        If PriceType.SelectedIndex = 2 Then
            LKACCTYPE = 0
        Else
            LKACCTYPE = AccountType.SelectedIndex
        End If
    End Sub
    Private Sub AccountType_TextChanged(sender As Object, e As EventArgs) Handles AccountType.TextChanged
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Or AccountType.SelectedIndex = 0 Then
            LoadAgent()
            BranchID.Enabled = True
            VAgentID = BranchID.EditValue
        Else
            BranchID.Enabled = False
            VAgentID = 0
        End If
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
                             GVRole.GetRowCellValue(i, "BuyPrice"))
            End If
        Next
        Return dt
    End Function
    Public Overrides Sub SetData()
        If PriceType.SelectedIndex = 2 Or PriceType.SelectedIndex = 1 Then
            If AccountType.SelectedIndex = -1 Then
                AccountType.ErrorText = "يجب اختيار نوع الحساب"
                Exit Sub
            End If
            If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 0 Then
                If BranchID.EditValue = -1 Then
                    BranchID.ErrorText = "يجب اختيار الفرع"
                    Exit Sub
                End If
            End If
            'ElseIf PriceType.SelectedIndex <> 2 Then
            If BankID.EditValue = -1 Then
                BankID.ErrorText = "يجب اختيار التسعير"
                Exit Sub
            End If
        End If
        If GVRole.RowCount <> 1 Then
            ErrorMessage(Me, "يجب اختيار سعر واحد", "رسالة خطأ")
            Exit Sub
        End If
        'If CurrencyPower.SelectedIndex < 0 Then
        '    ErrorMessage(Me, "يجب اختيار قوة العملة", "رسالة خطأ")
        '    Exit Sub
        'End If
        If PriceType.SelectedIndex = 2 Or PriceType.SelectedIndex = 1 Then
            If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Or AccountType.SelectedIndex = 0 Then
                IsAgent = True
                VAgentID = BranchID.EditValue
            Else
                IsAgent = False
                VAgentID = 0
            End If
        End If
        'If PriceType.SelectedIndex <> 2 Then
        '    LKBANKID = SafeToInt(BankID.EditValue)
        'Else
        '    LKBANKID = 0
        'End If

        If PriceType.SelectedIndex = 2 Then
            LKACCTYPE = AccountType.SelectedIndex
        Else
            LKACCTYPE = 0
        End If


        clscp.CURRENCYPRICE_Insert(CodeID.Text, CountryID.EditValue, PriceType.SelectedIndex, UserID, VAgentID,
                                   LKACCTYPE, SafeToInt(BankID.EditValue), CPDInsert, CurrencyPower.SelectedIndex, IsUpdate, VAgentID)

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
    Private Sub NewBasicCurrencyPrcie_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        LoadBank()
    End Sub

    Private Sub CurrencyFrom_TextChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.TextChanged
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة الأولى"
            Exit Sub
        End If
        LoadSecondCurrency()
    End Sub

    Private Sub CountryID_TextChanged(sender As Object, e As EventArgs) Handles CountryID.TextChanged
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Or AccountType.SelectedIndex = 0 Then
            LoadAgent()
            BranchID.Enabled = True
            VAgentID = BranchID.EditValue
        Else
            BranchID.Enabled = False
            VAgentID = 0
        End If
        If AccountType.SelectedIndex = 1 Then
            SORAG.Text = "الوكيل"
        ElseIf AccountType.SelectedIndex = 2 Then
            SORAG.Text = "الخدمة"
        ElseIf AccountType.SelectedIndex = 3 Then
            SORAG.Text = "وكيل عام"
        ElseIf AccountType.SelectedIndex = 0 Then
            SORAG.Text = "الحساب"
        End If
    End Sub
    Private Sub SalePrice_Leave(sender As Object, e As EventArgs) Handles SPrice.Leave
        addCatToDVG()
        CurrencyFrom.Focus()
    End Sub

    Private Sub AccountType_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType.EditValueChanged
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Or AccountType.SelectedIndex = 0 Then
            LoadAgent()
            BranchID.Enabled = True
            VAgentID = BranchID.EditValue
        Else
            BranchID.Enabled = False
            VAgentID = 0
        End If
        If AccountType.SelectedIndex = 1 Then
            SORAG.Text = "الوكيل"
        ElseIf AccountType.SelectedIndex = 2 Then
            SORAG.Text = "الفرع"
        ElseIf AccountType.SelectedIndex = 3 Then
            SORAG.Text = "وكيل عام"
        ElseIf AccountType.SelectedIndex = 0 Then
            SORAG.Text = "الحساب"
        End If
    End Sub

    Private Sub BankID_EditValueChanged(sender As Object, e As EventArgs) Handles BankID.EditValueChanged
        If PriceType.SelectedIndex = 3 Then
            LKBANKID = BankID.EditValue
        Else
            LKBANKID = 0
        End If
    End Sub
End Class
Public Class CLSNEWOWNCURRENCYPRICE
    Public Sub CURRENCYPRICE_Insert(ID As ULong, CountryID As Integer, PriceType As Integer, ADDueser As Integer, BranchID As Integer,
                                    AccountType As Integer, BankID As Integer, dt As DataTable, CurrencyPower As Boolean, IsUpdate As Boolean, ServiceTyID As Integer)
        Try
            Dim PRM(10) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = ID}
            PRM(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
            PRM(2) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = PriceType}
            PRM(3) = New SqlParameter("@ADDueser", SqlDbType.Int) With {.Value = ADDueser}
            PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(5) = New SqlParameter("@AccountType", SqlDbType.Int) With {.Value = AccountType}
            PRM(6) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID}
            PRM(7) = New SqlParameter("@TypeTb", SqlDbType.Structured) With {.Value = dt}
            PRM(8) = New SqlParameter("@CurrencyPower", SqlDbType.Bit) With {.Value = CurrencyPower}
            PRM(9) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = 0}
            PRM(10) = New SqlParameter("@ServiceTypeID", SqlDbType.Int) With {.Value = ServiceTyID}
            RUN_EXUTE_PRO("NewCurrencyPricesOwnTb_Insert", PRM)
            FrmSavedSuccessfully.ShowDialog()
            NewBasicCurrencyPrcie.NEWRECORD()
        Catch ex As SqlClient.SqlException

            XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)


        End Try
    End Sub
End Class