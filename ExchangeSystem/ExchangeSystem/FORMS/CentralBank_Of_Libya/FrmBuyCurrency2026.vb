Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.About
Imports DevExpress.XtraEditors
Imports DevExpress.XtraPrinting.Shape.Native
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSpellChecker
Imports Microsoft

Public Class FrmBuyCurrency2026
    Public Type As Integer
    Dim IsUpdate As Int16
    Public Frmid As Integer
    Sub Newrecord()
        New_Controlrs(Me)
        Enable_Controls(Me, True)
        IsUpdate = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Enabled = False
        BranchID.Enabled = False
        BuyPrice.Enabled = False
        AccValFrom.Enabled = False
        AccValTo.Enabled = False
        PaymentVal.Enabled = False
        TypeID.SelectedIndex = 0
        If UserType = 1 Then
            TypeID.Enabled = True
        Else
            TypeID.Enabled = False
        End If
        InsertDate.EditValue = Date.Now
        LoadToControlar(BranchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        LoadToControlar(CountryID, "CountriesTb_LoadToLKP", "CName", "ID", Nothing)
        OwnBamkLoad()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
        LoadToControlar(CurrIDFrom, "CurrencyMainTb_LOADTOLKP_2026", "CuName", "ID", PR)
        CountryID.EditValue = COUNTRYNID
        Code.Text = Type.ToString + "-" + (GETIDMAX_Pro("NewCurrencyBuyAndSale", "IDCode", 1, "BuyOrSale") + 1).ToString()
        BranchID.EditValue = BID
        BuyPrice.EditValue = 0
        BuyVal.EditValue = 0
        PaymentVal.EditValue = 0
        BuyPrice.Select
        BuyVal.Select()
        PaymentVal.Select()
        CurrIDFrom.Select()
        'LoadToControlar(BBranchID, "BBranchTb_SelectAll", "BranchName", "ID", Nothing)
    End Sub
    Private Sub FRMBayAndSaleCurr2026_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Newrecord()
        lodePreportes()
    End Sub
    Public Overrides Sub BNew()
        Newrecord()
        MyBase.BNew()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(Frmid, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Private Sub TypeID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeID.SelectedIndexChanged
        If IsUpdate = False Then
            If TypeID.SelectedIndex <> 1 Then
                CountryID.EditValue = COUNTRYNID
                CountryID.Enabled = False
            Else
                CountryID.Enabled = True
                CountryID.EditValue = Nothing
            End If
        End If
        BranchID_EditValueChanged(Nothing, Nothing)
        LoadBuyType()
        LoadCurrncyTo()
        LoadAccIDFrom()
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        If CountryID.EditValue = COUNTRYNID Then
            BranchID.EditValue = BID
        Else
            BranchID.EditValue = MAINBID
        End If
        LoadCurrncyTo()
        LoadAccIDFrom()
        LoadBuyType()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        BuyType.Properties.DataSource = Nothing
        PaymentType.Properties.DataSource = Nothing
        'If Not IsEmpty(BranchID) Then
        LoadBuyType()
        Dim PR1(2) As SqlParameter
        PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
        PR1(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1}
        PR1(2) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = 0}
        LoadToControlar(PaymentType, "BuyCurrency2026_LOADINTOLKPBASEDONAccParent", "AccName", "AccCode", PR1, True, "نقدا")
        'End If
    End Sub

    Sub LoadBuyType()
        BuyType.Properties.DataSource = Nothing
        BuyType.EditValue = Nothing
        'If IsEmpty(BranchID) Or IsEmpty(TypeID) Or IsEmpty(CountryID) Then Exit Sub
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
        PR(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1}
        PR(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
        PR(3) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = If(TypeID.SelectedIndex = 2, 3, SafeToInt(TypeID.SelectedIndex))}
        LoadToControlar(BuyType, "BuyCurrency2026_LOADINTOLKPBASEDONAccParent", "AccName", "AccCode", PR, True, "نقدا")
    End Sub

    Private Sub BuyType_EditValueChanged(sender As Object, e As EventArgs) Handles BuyType.EditValueChanged
        LoadAccIDFrom()
    End Sub
    Sub LoadAccIDFrom()
        AccIDFrom.Properties.DataSource = Nothing
        AccIDFrom.EditValue = Nothing
        If Not IsEmpty(BuyType) Then
            Dim PR(6) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = SafeToInt(BuyType.EditValue)}
            PR(2) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = If(IsUpdate = False, 0, 1)}
            PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = SafeToInt(CurrIDFrom.EditValue)}
            PR(4) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
            PR(5) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
            PR(6) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = If(TypeID.SelectedIndex = 2, 3, SafeToInt(TypeID.SelectedIndex))}
            LoadToControlar(AccIDFrom, "BuyCurrency2026_LOADINTOLKPBASEDONAccParent", "AccName", "AccID", PR)
            HideAllColumnsExceptDisplayAndVAl(AccIDFrom)
            If IsUpdate = False Then
                If BuyType.EditValue = 0 And TypeID.SelectedIndex <> 1 Then
                    AccIDFrom.EditValue = UserAccID
                    AccIDFrom.Enabled = False
                Else
                    AccIDFrom.EditValue = Nothing
                    AccIDFrom.Enabled = True
                End If
                If BuyType.EditValue = 0 Or SafeToInt(GetLKPColumnVal(BuyType, "IsBank")) = 1 Then
                    CustName.Enabled = True
                    CustPhone.Enabled = True
                    NationalNumper.Enabled = True
                    PassNumber.Enabled = True
                Else
                    CustName.Enabled = False
                    CustPhone.Enabled = False
                    NationalNumper.Enabled = False
                    PassNumber.Enabled = False
                End If
            End If
        End If

    End Sub

    Private Sub PaymentType_EditValueChanged(sender As Object, e As EventArgs) Handles PaymentType.EditValueChanged
        If IsUpdate = False Then
            AccIDTo.Properties.DataSource = Nothing
            AccIDTo.EditValue = Nothing
            OwnBankName.EditValue = Nothing
            OwnBankNumber.Text = String.Empty
        End If
        If Not IsEmpty(PaymentType) Then
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = SafeToInt(PaymentType.EditValue)}
            PR(2) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = 1}
            PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = DefaultCurrency}
            PR(4) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
            LoadToControlar(AccIDTo, "BuyCurrency2026_LOADINTOLKPBASEDONAccParent", "AccName", "AccID", PR)
            HideAllColumnsExceptDisplayAndVAl(AccIDTo)
            If IsUpdate = False Then
                If PaymentType.EditValue = 0 Then
                    AccIDTo.EditValue = UserAccID
                    AccIDTo.Enabled = False
                Else
                    AccIDTo.EditValue = Nothing
                    AccIDTo.Enabled = True
                End If
            End If
        End If
        If IsUpdate = False Then
            If SafeToInt(GetLKPColumnVal(PaymentType, "IsBank")) = 1 Then
            OwnBankName.Enabled = True
            OwnBankNumber.Enabled = True
        Else
            OwnBankName.Enabled = False
            OwnBankNumber.Enabled = False
        End If
        End If
        LoadCurrncyTo()
    End Sub

    Private Sub CurrIDFrom_EditValueChanged(sender As Object, e As EventArgs) Handles CurrIDFrom.EditValueChanged
        LoadCurrncyTo()
        LoadAccIDFrom()
        'BuyPrice.EditValue = GetLKPColumnVal(CurrIDTo, "BuyPrice")
    End Sub

    Sub LoadCurrncyTo()
        CurrIDTo.Properties.DataSource = Nothing
        CurrIDTo.EditValue = Nothing
        If IsEmpty(CurrIDFrom) Or IsEmpty(TypeID) Or IsEmpty(PaymentType) Then Exit Sub
        Dim PR(4) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1}
        PR(1) = New SqlParameter("@CurrID", SqlDbType.Int) With {.Value = SafeToInt(CurrIDFrom.EditValue)}
        PR(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
        PR(3) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = If(TypeID.SelectedIndex = 2, 3, SafeToInt(TypeID.SelectedIndex))}
        PR(4) = New SqlParameter("@BuyType", SqlDbType.BigInt) With {.Value = SafeToInt(PaymentType.EditValue)}
        LoadToControlar(CurrIDTo, "CurrencyMainTb_LOADTOLKP_2026", "CuName", "ID", PR)
        CurrIDTo.EditValue = DefaultCurrency
        BuyPrice.EditValue = GetLKPColumnVal(CurrIDTo, "BuyPrice")
    End Sub

    Sub GetNetTot()
        PaymentVal.EditValue = 0
        If BuyPrice.EditValue = 0 Or IsEmpty(BuyPrice) Then Exit Sub
        If GetLKPColumnVal(CurrIDTo, "CurrencyPower") = 0 Then
            PaymentVal.EditValue = Math.Floor((SafeToDecimal(BuyVal.EditValue) / SafeToDecimal(BuyPrice.EditValue)) / 5) * 5
        Else
            PaymentVal.EditValue = Math.Floor((SafeToDecimal(BuyVal.EditValue) * SafeToDecimal(BuyPrice.EditValue)) / 5) * 5
        End If

    End Sub

    Private Sub BuyVal_EditValueChanged(sender As Object, e As EventArgs) Handles BuyVal.EditValueChanged
        GetNetTot()
    End Sub

    Private Sub BuyPrice_EditValueChanged(sender As Object, e As EventArgs) Handles BuyPrice.EditValueChanged
        GetNetTot()
    End Sub

    Sub Insert_CurrencyBuySale()
        Try
            '================= Validation =================
            If Not ValidateControl(Code, "الرمز") Then Exit Sub
            If Not ValidateControl(BranchID, "الفرع") Then Exit Sub
            If Not ValidateControl(CurrIDFrom, "العملة من") Then Exit Sub
            If Not ValidateControl(CurrIDTo, "العملة إلى") Then Exit Sub
            If Not ValidateControl(AccIDFrom, "حساب من") Then Exit Sub
            If Not ValidateControl(AccIDTo, "حساب إلى") Then Exit Sub
            If Not ValidateControl(BuyPrice, "السعر") Then Exit Sub
            If Not ValidateControl(BuyVal, "القيمة المشتراه") Then Exit Sub
            If Not ValidateControl(PaymentVal, "القيمة المصروفة") Then Exit Sub
            'If Not ValidateControl(ThePurpose, "الغرض") Then Exit Sub
            If SafeToInt(GetLKPColumnVal(BuyType, "IsBank")) <> 1 Then
                If Not ValidateControl(CustName, "اسم العميل") Then Exit Sub
                If Not ValidateControl(CustPhone, "هاتف العميل") Then Exit Sub
                If Not ValidateControl(NationalNumper, "الرقم الوطني") Then Exit Sub
                If Not ValidateControl(PassNumber, "رقم الجواز") Then Exit Sub
            End If
            If SafeToInt(GetLKPColumnVal(PaymentType, "IsBank")) = 1 Then
                If Not ValidateControl(OwnBankName, "اسم صاحب الحساب") Then Exit Sub
                If Not ValidateControl(OwnBankNumber, "رقم الحساب") Then Exit Sub
            End If
            If BuyVal.EditValue <= 0 Then
                ErrorMessage(Me, "تنبية", "القيمة المشتراه يجب أن تكون أكبر من صفر")
                Exit Sub
            End If
            '================= Parameters =================
            Dim prm(24) As SqlParameter

            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1}

            prm(1) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = 0}

            prm(2) = New SqlParameter("@BuyOrSale", SqlDbType.Int) With {.Value = 1} ' 2 شراء / 1 بيع

            prm(3) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = If(TypeID.SelectedIndex = 2, 3, SafeToInt(TypeID.SelectedIndex))}

            prm(4) = New SqlParameter("@Code", SqlDbType.NVarChar) With {.Value = SafeToString(Code.Text.Trim)}

            prm(5) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}

            prm(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}

            prm(7) = New SqlParameter("@ParentFromID", SqlDbType.BigInt) With {.Value = SafeToInt(BuyType.EditValue)}

            prm(8) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = SafeToInt(AccIDFrom.EditValue)}

            prm(9) = New SqlParameter("@CurrencyIDFrom", SqlDbType.Int) With {.Value = SafeToInt(CurrIDFrom.EditValue)}

            prm(10) = New SqlParameter("@Price", SqlDbType.Decimal) With {.Value = SafeToDecimal(BuyPrice.EditValue)}

            prm(11) = New SqlParameter("@ValFrom", SqlDbType.Decimal) With {.Value = SafeToDecimal(BuyVal.EditValue)}

            prm(12) = New SqlParameter("@ParentToID", SqlDbType.BigInt) With {.Value = SafeToInt(PaymentType.EditValue)}

            prm(13) = New SqlParameter("@AccIDTo", SqlDbType.BigInt) With {.Value = SafeToInt(AccIDTo.EditValue)}

            prm(14) = New SqlParameter("@CurrencyIDTo", SqlDbType.Int) With {.Value = SafeToInt(CurrIDTo.EditValue)}

            prm(15) = New SqlParameter("@ValTo", SqlDbType.Decimal) With {.Value = SafeToDecimal(PaymentVal.EditValue)}

            prm(16) = New SqlParameter("@Notes", SqlDbType.NVarChar) With {.Value = SafeToString(Notes.Text.Trim)}

            prm(17) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}

            prm(18) = New SqlParameter("@CustName", SqlDbType.NVarChar) With {.Value = SafeToString(CustName.Text.Trim)}

            prm(19) = New SqlParameter("@CustPhone", SqlDbType.NVarChar) With {.Value = SafeToString(CustPhone.Text.Trim)}

            prm(20) = New SqlParameter("@NationalNumper", SqlDbType.NVarChar) With {.Value = SafeToString(NationalNumper.Text.Trim)}

            prm(21) = New SqlParameter("@PassNumber", SqlDbType.NVarChar) With {.Value = SafeToString(PassNumber.Text.Trim)}

            prm(22) = New SqlParameter("@ThePurpose", SqlDbType.Int) With {.Value = 0}

            prm(23) = New SqlParameter("@OwnBankName", SqlDbType.NVarChar) With {.Value = SafeToString(OwnBankName.Text.Trim)}

            prm(24) = New SqlParameter("@OwnBankNumber", SqlDbType.NVarChar) With {.Value = SafeToString(OwnBankNumber.Text.Trim)}
            '================= Execute =================
            Dim dt As DataTable = RUN_QUARY_PRO_alter("NewCurrencyBuyAndSale_CRUD", prm)

            XtraMessageBox.Show("تم الحفظ بنجاح", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Print()

            Newrecord()

        Catch ex As SqlClient.SqlException

            Select Case ex.State
                Case 101
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case 100
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ' إعادة توليد الكود
                    Code.Text = Type.ToString + "-" + (GETIDMAX_Pro("NewCurrencyBuyAndSale", "IDCode", 1, "BuyOrSale") + 1).ToString()

                Case Else
                    XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

        End Try
    End Sub

    Public Overrides Sub Save()
        Insert_CurrencyBuySale()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        Try
            '================= Validation =================
            If Not ValidateControl(Code, "الرمز") Then Exit Sub
            '================= Parameters =================
            Dim prm(3) As SqlParameter

            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 2}

            prm(1) = New SqlParameter("@BuyOrSale", SqlDbType.Int) With {.Value = 3} ' 3 استرجاع شراء

            prm(2) = New SqlParameter("@Code", SqlDbType.NVarChar) With {.Value = SafeToString(Code.Text.Trim)}

            prm(3) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}


            '================= Execute =================
            Dim dt As DataTable = RUN_QUARY_PRO_alter("NewCurrencyBuyAndSale_CRUD", prm)

            XtraMessageBox.Show("تم الاسترجاع بنجاح", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information)


            Newrecord()

            MyBase.UPDATERECORD()
        Catch ex As SqlClient.SqlException

            Select Case ex.State
                Case 101
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case Else
                    XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

        End Try
    End Sub

    Sub LoadToView()
        ViweBuyCurrency2026.GCRole.DataSource = Nothing
        ViweBuyCurrency2026.GVRole.Columns.Clear()
        Dim prm(2) As SqlParameter
        prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 3}
        prm(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
        'prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BID)}
        prm(2) = New SqlParameter("@BuyOrSale", SqlDbType.Int) With {.Value = 1}
        LoadToControlar(ViweBuyCurrency2026.GCRole, "NewCurrencyBuyAndSale_CRUD", "", "", prm)
        ViweBuyCurrency2026.ParentForm = Me
        NEWDVGFROMAT(ViweBuyCurrency2026.GVRole)
        ViweBuyCurrency2026.ShowDialog()
    End Sub

    Public Sub GetRecord(CO As String)
        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = CO}

            Dim dt As DataTable = RUN_QUARY_PRO_alter("NewCurrencyBuyAndSale_CRUD", prm)

            If dt.Rows.Count <= 0 Then Exit Sub

            Dim r As DataRow = dt.Rows(0)

            '================= UI حالة =================
            Enable_Controls(Me, False)
            IsUpdate = True
            BtnSave.Enabled = False
            BtnEdit.Enabled = True
            BtnEdit.Caption = "استرجاع"
            BtnPrint.Enabled = True

            '================= البيانات =================
            Code.Text = SafeToString(r("Code"))

            BranchID.EditValue = SafeToInt(r("BranchID"))
            CountryID.EditValue = SafeToInt(r("CountryID"))

            TypeID.SelectedIndex = SafeToInt(r("PriceType"))

            CurrIDFrom.EditValue = SafeToInt(r("CurrencyIDFrom"))
            BuyType.EditValue = SafeToInt(r("ParentFromID"))
            AccIDFrom.EditValue = SafeToInt(r("AccIDFrom"))

            BuyPrice.EditValue = SafeToDecimal(r("Price"))
            BuyVal.EditValue = SafeToDecimal(r("ValFrom"))

            PaymentType.EditValue = SafeToInt(r("ParentToID"))
            AccIDTo.EditValue = SafeToInt(r("AccIDTo"))
            CurrIDTo.EditValue = SafeToInt(r("CurrencyIDTo"))

            PaymentVal.EditValue = SafeToDecimal(r("ValTo"))
            CustName.Text = SafeToString(r("CustName"))
            CustPhone.Text = SafeToString(r("CustPhone"))
            NationalNumper.Text = SafeToString(r("NationalNumper"))
            PassNumber.Text = SafeToString(r("PassNumber"))
            'ThePurpose.EditValue = SafeToInt(r("ThePurpose"))
            OwnBankName.Text = SafeToString(r("OwnBankName"))
            OwnBankNumber.Text = SafeToString(r("OwnBankNumber"))
            Notes.Text = SafeToString(r("Notes"))

        Catch ex As Exception
            XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        LoadToView()
    End Sub

    Private Sub AccIDFrom_EditValueChanged(sender As Object, e As EventArgs) Handles AccIDFrom.EditValueChanged
        AccValFrom.EditValue = 0
        CustName.Text = String.Empty
        CustPhone.Text = String.Empty
        NationalNumper.Text = String.Empty
        PassNumber.Text = String.Empty
        If IsEmpty(AccIDFrom) Then Exit Sub
        AccValFrom.EditValue = GetLKPColumnVal(AccIDFrom, "CurrAccVal")
        If BuyType.EditValue <> 0 AndAlso SafeToInt(GetLKPColumnVal(BuyType, "IsBank")) <> 1 Then
            CustName.Text = SafeToString(GetLKPColumnVal(AccIDFrom, "AccName"))
            CustPhone.Text = SafeToString(GetLKPColumnVal(AccIDFrom, "AccPhone"))
            NationalNumper.Text = SafeToString(GetLKPColumnVal(AccIDFrom, "AccNatNumber"))
            PassNumber.Text = SafeToString(GetLKPColumnVal(AccIDFrom, "AccIDNo"))
        End If
    End Sub
    Public Overrides Sub Print()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@Action", 4)
            PRM(1) = New SqlParameter("@Code", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("NewCurrencyBuyAndSale_CRUD", PRM)
            Dim ds As New DataSet
            dt.TableName = "CurrenciesBuyandsellTB"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTINTCURSALES1
                report.DataSource = ds
                report.DataMember = "CurrenciesBuyandsellTB"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.XrLabel26.Text = Cur_Code(CurrIDTo.Text, PaymentVal.Text, False, "n2")
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        MyBase.Print()
    End Sub

    Private Sub AccIDTo_EditValueChanged(sender As Object, e As EventArgs) Handles AccIDTo.EditValueChanged
        AccValTo.EditValue = 0
        If IsEmpty(AccIDTo) Then Exit Sub
        AccValTo.EditValue = GetLKPColumnVal(AccIDTo, "AccVal")
    End Sub

#Region "تعبئة صاحب الحساب"
    Sub OwnBamkLoad()
        OwnBankName.Properties.DataSource = Nothing
        ' البحث عن الكود الحالي لديك:
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 4}
        LoadToControlar(OwnBankName, "NewCurrencyBuyAndSale_CRUD", "OwnBankName", "OwnBankNumber", prm)

        ' --- الإضافات المطلوبة هنا ---
        ' 1. السماح بالكتابة اليدوية داخل اللوك اب
        OwnBankName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard

        ' 2. السماح بقبول القيمة المكتوبة حتى لو لم تكن في القائمة
        OwnBankName.Properties.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.True

        ' 3. تفعيل البحث والفلترة التلقائية أثناء الكتابة
        OwnBankName.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter
        OwnBankName.Properties.ImmediatePopup = True ' فتح القائمة تلقائياً عند الكتابة
    End Sub

    Private Sub OwnBankName_ProcessNewValue(sender As Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles OwnBankName.ProcessNewValue
        ' نتحقق أن القيمة المكتوبة ليست فارغة
        Dim newValue As String = e.DisplayValue.ToString().Trim()
        If String.IsNullOrEmpty(newValue) Then Return

        ' جلب المصدر الحالي للبيانات (DataTable)
        Dim dt As DataTable = TryCast(OwnBankName.Properties.DataSource, DataTable)

        If dt IsNot Nothing Then
            ' إضافة القيمة الجديدة لجدول الذاكرة حتى يتعرف عليها النظام كخيار صالح
            ' نضع القيمة في حقل الاسم وحقل الرقم (أو نتركه فارغاً)
            Dim newRow As DataRow = dt.NewRow()
            newRow("OwnBankName") = newValue
            newRow("OwnBankNumber") = "" ' اترك الرقم فارغاً ليقوم المستخدم بكتابته يدوياً في التكست الآخر
            dt.Rows.Add(newRow)

            ' إبلاغ الأداة أننا قبلنا القيمة بنجاح
            e.Handled = True
        End If
    End Sub

    Private Sub OwnBankName_EditValueChanged(sender As Object, e As EventArgs) Handles OwnBankName.EditValueChanged, OwnBankName.TabIndexChanged
        OwnBankNumber.Text = String.Empty
        If IsEmpty(OwnBankName) Then Exit Sub
        OwnBankNumber.Text = SafeToString(GetLKPColumnVal(OwnBankName, "OwnBankNumber"))
    End Sub
#End Region
End Class