Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class FrmCash_BankTransfers
    Dim IsUpdate As Int16
    Sub NewRecord()
        New_Controlrs(Me)
        Enable_Controls(Me, True)
        IsUpdate = 0
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        WithdrawalDate.EditValue = Date.Now
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LoadToControlar(BranchID, "COBRANCHTB_LoadDataIntoLookUpEdit_FILL_pro", "BName", "DBRID", Nothing)
        LoadToControlar(CurrencyID, "CurrencyMainTb_LOAD_Defult_TOLKP", "CuName", "ID", Nothing)
        Code.Text = "59-" & (GETIDMAX("Cash_BankTransfersTB", "ID") + 1).ToString()
        CurrencyID.EditValue = DefaultCurrency
        BranchID.EditValue = BID
        TransType.SelectedIndex = 0
        TransType.Select()
    End Sub
    Private Sub GeneralVoucher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
        BranchID_EditValueChanged(Nothing, Nothing)
    End Sub
    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
    Public Overrides Sub Save()
        Insert()
        MyBase.Save()
    End Sub

    Sub Insert()
        Try
            If Not ValidateControl(Code, "الرمز") Then Exit Sub
            If Not ValidateControl(BranchID, "الفرع") Then Exit Sub
            If Not ValidateControl(CurrencyID, "العملة") Then Exit Sub
            If Not ValidateControl(TransType, "نوع التحويل") Then Exit Sub
            If Not ValidateControl(AccountType, "نوع الحساب") Then Exit Sub
            If Not ValidateControl(AccID, "الحساب") Then Exit Sub
            If Not ValidateControl(WDValue, "القيمة") Then Exit Sub

            ' تجهيز الباراميترات — حسب الستورد Cash_BankTransfersTB_CRUD
            Dim prm(11) As SqlParameter
            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1}  ' 1 = إضافة
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, 250) With {.Value = SafeToString(Code.Text.Trim)}
            prm(2) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = DBNull.Value}
            prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
            prm(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = SafeToInt(CurrencyID.EditValue)}
            prm(5) = New SqlParameter("@TransType", SqlDbType.Int) With {.Value = SafeToInt(TransType.SelectedIndex)}
            prm(6) = New SqlParameter("@ParentID", SqlDbType.BigInt) With {.Value = SafeToInt(AccountType.EditValue)}
            prm(7) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = SafeToInt(AccID.EditValue)}
            prm(8) = New SqlParameter("@TransVal", SqlDbType.Decimal) With {.Value = SafeToDecimal(WDValue.EditValue)}
            prm(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = SafeToString(Notes.Text.Trim)}
            prm(10) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            prm(11) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}

            ' تنفيذ الإجراء
            Dim dt As DataTable = RUN_QUARY_PRO_alter("Cash_BankTransfersTB_CRUD", prm)

            XtraMessageBox.Show("تم الحفظ بنجاح.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Print()
            NewRecord()

        Catch ex As SqlClient.SqlException
            Select Case ex.State
                Case 101
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case 100
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Code.Text = "59" + "-" + Convert.ToString(GETIDMAX("Cash_BankTransfersTB", "ID") + 1)
                Case Else
                    XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
        End Try
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        AccountType.Properties.DataSource = Nothing
        AccountType.EditValue = Nothing
        If IsEmpty(BranchID) Or BranchID.Text = String.Empty Then Exit Sub
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
        LoadToControlar(AccountType, "InternalEX_AccountTBLoadLine3ToLKP", "AccName", "AccCode", PR)
    End Sub

    Private Sub AccountType_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType.EditValueChanged
        AccID.Properties.DataSource = Nothing
        If AccountType.Text <> String.Empty Then
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            PR(2) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = 1}
            PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = DefaultCurrency}
            LoadToControlar(AccID, "InternalEX_LOADINTOLKPBASEDONAccParent", "AccName", "AccID", PR)
            HideAllColumnsExceptDisplayAndVAl(AccID)
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadToView()
    End Sub

    Sub LoadToView()
        'FrmGenralView.GCRole.DataSource = Nothing
        'FrmGenralView.GVRole.Columns.Clear()
        'Dim prm(2) As SqlParameter
        'prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 3}
        'prm(1) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = True}
        'prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BID)}
        'LoadToControlar(FrmGenralView.GCRole, "Cash_BankTransfersTB_CRUD", "", "", prm)
        'FrmGenralView.ParentForm = Me
        'DVGFROMAT(FrmGenralView.GVRole)
        'FrmGenralView.ShowDialog()
    End Sub

    Public Sub GetRecord(codeValue As String)
        Try
            ' @Action = 3 (عرض في العدسة حسب الستورد)
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, 250) With {.Value = codeValue}

            Dim dt As DataTable = RUN_QUARY_PRO_alter("Cash_BankTransfersTB_CRUD", prm)
            If dt.Rows.Count <= 0 Then Exit Sub

            Enable_Controls(Me, False)
            IsUpdate = True

            BtnSave.Enabled = False
            BtnEdit.Enabled = True
            BtnPrint.Enabled = True

            ' تعبئة الحقول
            Code.Text = SafeToString(dt.Rows(0).Item("Code"))
            BranchID.EditValue = SafeToInt(dt.Rows(0).Item("BranchID"))
            WithdrawalDate.EditValue = dt.Rows(0).Item("InseretDate")
            CurrencyID.EditValue = SafeToInt(dt.Rows(0).Item("CurrencyID"))
            TransType.SelectedIndex = SafeToInt(dt.Rows(0).Item("TransType"))
            AccountType.EditValue = SafeToInt(dt.Rows(0).Item("ParentID"))
            AccID.EditValue = SafeToInt(dt.Rows(0).Item("AccID"))
            WDValue.EditValue = SafeToDecimal(dt.Rows(0).Item("TransVal"))
            Notes.Text = SafeToString(dt.Rows(0).Item("Notes"))

        Catch ex As Exception
            XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Overrides Sub UPDATERECORD()
        Try
            Dim prm(3) As SqlParameter

            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 2} ' تعديل / إلغاء
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, 250) With {.Value = SafeToString(Code.Text.Trim)}
            prm(2) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            prm(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}

            Dim dt As DataTable = RUN_QUARY_PRO_alter("Cash_BankTransfersTB_CRUD", prm)

            XtraMessageBox.Show("تم تحديث السجل بنجاح.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            NewRecord()

        Catch ex As SqlClient.SqlException
            Select Case ex.State
                Case 101
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case Else
                    XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
        End Try

        MyBase.UPDATERECORD()
    End Sub


    Private Sub AccID_EditValueChanged(sender As Object, e As EventArgs) Handles AccID.EditValueChanged
        GetAccVal()
    End Sub

    Public Overrides Sub Print()
        'If IsEmpty(Code) Then Exit Sub
        'Dim prm(1) As SqlParameter
        'prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 3}
        'prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = SafeToString(Code.Text.Trim)}
        'Dim dt As New DataTable
        'dt.Clear()
        'dt = RUN_QUARY_PRO_alter("GeneralVoucherTB_CRUD", prm)
        'If dt.Rows.Count > 0 Then
        '    Dim report As New XtraReport1
        '    report.DataSource = dt
        '    report.DataMember = "ProjectTb"
        '    Dim tool As ReportPrintTool = New ReportPrintTool(report)
        '    report.XrLabel19.Text = Cur_Code(CurrencyID.Text, WDValue.Text, False, "n2")
        '    report.XrLabel25.Text = Cur_Code(CurrencyID.Text, WDValue.Text, True, "n2")
        '    report.CreateDocument()
        '    report.ShowPreview()
        'Else
        '    MessageBox.Show("عذرا لايوجد بيانات لطباعتها", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End If
    End Sub

    Sub GetAccVal()
        AccNetVal.EditValue = 0
        If IsEmpty(AccID) Or IsEmpty(TransType) Then Exit Sub
        If TransType.SelectedIndex = 1 Then
            AccNetVal.EditValue = GetLKPColumnVal(AccID, "GetBankAccVal")
        Else
            AccNetVal.EditValue = GetLKPColumnVal(AccID, "GetAccVal")
        End If

    End Sub

    Private Sub TransType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TransType.SelectedIndexChanged
        GetAccVal()
    End Sub


End Class