Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FRMProBANKDEPOSIT
    Public IDCode, DiscountAccID As ULong
    Public LOADTYPE, EMPID, OpType As Integer
    Dim UserMovement, GetAccountName As String
    Public GetBANKNAME As String
    Public IsUpdate, CanChangeSafe As Boolean
    Sub DISAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        BranchID.Enabled = False
        AccountType.Enabled = False
        BankName.Enabled = False
        AccFrom.Enabled = False
        AccFromValue.Enabled = False
        AccToValue.Enabled = False
        AccTo.Enabled = False
        BillVal.Enabled = False
        CURRENCYID.Enabled = False
        BillNo.Enabled = False
        Notes.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = False
        BtnPrint.Enabled = True
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(158, UserID)


        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub ENAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        BranchID.Enabled = False
        AccountType.Enabled = True
        AccFrom.Enabled = True
        AccFromValue.Enabled = False
        AccTo.Enabled = True
        BankName.Enabled = True
        AccToValue.Enabled = False
        BillVal.Enabled = True
        CURRENCYID.Enabled = True
        BillNo.Enabled = True
        Notes.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
    End Sub
    Sub NEWRECORD()
        LoadBANK()
        AccFrom.Properties.DataSource = Nothing
        AccTo.Properties.DataSource = Nothing
        AccountType.Properties.DataSource = Nothing
        AccountType.EditValue = -1
        IsUpdate = False
        ENAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        InsertDate.EditValue = Date.Now
        BranchID.EditValue = -1
        LOADBRANCH()
        AccFrom.EditValue = -1
        AccTo.EditValue = -1
        BankName.EditValue = -1
        BranchID.EditValue = BID
        BranchID.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        AccFromValue.EditValue = 0.000
        AccToValue.EditValue = 0.000
        BillVal.EditValue = 0.000
        BillNo.Text = ""
        Notes.Text = ""
        lodePreportes()
    End Sub
    Sub LOADRECURRENCY()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CURRENCYTB_LoadWithBranch", PRM)
        If DT.Rows.Count > 0 Then
            CURRENCYID.Properties.DataSource = DT
            CURRENCYID.Properties.ValueMember = "ID"
            CURRENCYID.Properties.DisplayMember = "CurrencyName"
            CURRENCYID.Properties.PopulateColumns()
            CURRENCYID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Sub LoadBANKNAME(AccBBranchID As Integer)
        If BranchID.Text <> String.Empty Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = AccBBranchID}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_GetBankName", PR)
            If dt.Rows.Count > 0 Then
                GetBANKNAME = dt.Rows(0)("BankName")
            Else
                GetBANKNAME = ""
            End If
        End If
    End Sub
    Sub LoadBANK()
        If BranchID.Text <> String.Empty Then
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO_ONLY("BanksTb_LODE")
            If dt.Rows.Count > 0 Then
                BankName.Properties.DataSource = dt
                BankName.Properties.ValueMember = "ID"
                BankName.Properties.DisplayMember = "BankName"
                BankName.Properties.ShowHeader = False
            End If
        End If
    End Sub
    Sub LoadAccountType(Branch As Integer)
        AccountType.Properties.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = Branch}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_AccountTB_LoadLine3ToLKP", PR)
        If dt.Rows.Count > 0 Then
            AccountType.Properties.DataSource = dt
            AccountType.Properties.ValueMember = "AccCode"
            AccountType.Properties.DisplayMember = "AccName"
            AccountType.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADBBRNACH(AccBankID As Integer)
        If BankName.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = AccBankID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_LOADTOLKPBasedOnBankID", PR)
            If dt.Rows.Count > 0 Then
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFrom.Properties.DataSource = dt
                    AccFrom.Properties.ValueMember = "AccID"
                    AccFrom.Properties.DisplayMember = "BranchName"
                    AccFrom.Properties.ShowHeader = False
                ElseIf LOADTYPE = 48 Then
                    AccTo.Properties.DataSource = dt
                    AccTo.Properties.ValueMember = "AccID"
                    AccTo.Properties.DisplayMember = "BranchName"
                    AccTo.Properties.ShowHeader = False
                End If
            Else
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFrom.Properties.DataSource = Nothing
                ElseIf LOADTYPE = 48 Then
                    AccTo.Properties.DataSource = Nothing
                End If
            End If
        End If
    End Sub
    Sub LOADBBRNACH2()
        If BranchID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_LOADTOAccIDTOLKP", PR)
            If dt.Rows.Count > 0 Then
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFrom.Properties.DataSource = dt
                    AccFrom.Properties.ValueMember = "AccID"
                    AccFrom.Properties.DisplayMember = "BranchName"
                    AccFrom.Properties.ShowHeader = False
                ElseIf LOADTYPE = 48 Then
                    AccTo.Properties.DataSource = dt
                    AccTo.Properties.ValueMember = "AccID"
                    AccTo.Properties.DisplayMember = "BranchName"
                    AccTo.Properties.ShowHeader = False
                End If
            Else
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFrom.Properties.DataSource = Nothing
                ElseIf LOADTYPE = 48 Then
                    AccTo.Properties.DataSource = Nothing
                End If
            End If
        End If
    End Sub
    Sub LOADAccFromOrAccTo()
        Try
            If BranchID.Text <> String.Empty And AccountType.EditValue <> -1 Then
                Dim PR(3) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
                PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
                PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 1}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("AccountsTb_LOADINTOLKPBASEDONAccParent", PR)
                If dt.Rows.Count > 0 Then
                    If LOADTYPE = 48 Then
                        AccFrom.Properties.DataSource = dt
                        AccFrom.Properties.ValueMember = "AccID"
                        AccFrom.Properties.DisplayMember = "AccName"
                        AccFrom.Properties.ShowHeader = False
                    ElseIf LOADTYPE = 17 Or LOADTYPE = 19 Then
                        AccTo.Properties.DataSource = dt
                        AccTo.Properties.ValueMember = "AccID"
                        AccTo.Properties.DisplayMember = "AccName"
                        AccTo.Properties.ShowHeader = False
                    Else
                        AccFrom.EditValue = -1
                        AccFrom.Properties.DataSource = Nothing
                        AccTo.EditValue = -1
                        AccTo.Properties.DataSource = Nothing
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AccFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccFrom.QueryPopUp
        If LOADTYPE = 48 Then
            If AccountType.EditValue <> -1 Or AccountType.Text <> String.Empty Then
                Dim PR(3) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
                PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
                PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 1}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("AccountsTb_LOADINTOLKPBASEDONAccParent", PR)
                If dt.Rows.Count > 0 Then
                    AccFrom.Properties.PopulateColumns()
                    AccFrom.Properties.Columns("AccID").Visible = False
                End If
            End If
        ElseIf LOADTYPE = 17 Or LOADTYPE = 19 Then
            If BankName.EditValue <> -1 And BankName.Text <> String.Empty Then
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BankName.EditValue}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("BBranchTb_LOADTOAccIDTOLKP", PR)
                If dt.Rows.Count > 0 Then
                    AccFrom.Properties.PopulateColumns()
                    AccFrom.Properties.Columns("AccID").Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub AccTo_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccTo.QueryPopUp
        If LOADTYPE = 48 Then
            If BankName.EditValue <> -1 And BankName.Text <> String.Empty Then
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BankName.EditValue}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("BBranchTb_LOADTOAccIDTOLKP", PR)
                If dt.Rows.Count > 0 Then
                    AccTo.Properties.PopulateColumns()
                    AccTo.Properties.Columns("AccID").Visible = False
                End If
            End If
        ElseIf LOADTYPE = 17 Or LOADTYPE = 19 Then
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 1}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LOADINTOLKPBASEDONAccParent", PR)
            If dt.Rows.Count > 0 Then
                AccTo.Properties.PopulateColumns()
                AccTo.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub

    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public AccSafeID As ULong
    Public Overrides Sub SetData()
        IsDataValidLKP(BranchID)
        If AccFrom.EditValue = -1 Or AccFrom.Text = String.Empty Then
            AccFrom.ErrorText = "يجب اختيار الحساب"
            Exit Sub
        End If
        If AccTo.EditValue = -1 Or AccTo.Text = String.Empty Then
            AccTo.ErrorText = "يجب اختيار الحساب"
            Exit Sub
        End If
        If BillVal.EditValue <= 0.000 Then
            BillVal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If
        If CURRENCYID.EditValue = -1 Then

            CURRENCYID.ErrorText = "يجب اختيار العملة"
            Exit Sub
        End If
        If BillNo.Text = String.Empty Then

            BillNo.ErrorText = "هذا الحقل لايجب أن يكون فارغ"
            Exit Sub
        End If
        GetAccountName = AccFrom.Text
        If LOADTYPE = 48 Then
            OpType = 78
            UserMovement = "إيداع بصك في حساب مشروع" & AccFrom.Text
        End If
        If IsUpdate = 0 Then
            BANKDEPOSIT_Insert(CodeID.Text.Trim, InsertDate.EditValue, BranchID.EditValue, UserID, AccFrom.EditValue, AccTo.EditValue, IDCode, BillVal.EditValue, 1,
                                      1, 0, BillVal.EditValue, 0, BillNo.Text.Trim, Notes.Text.Trim, IsActive, IsUpdate, LOADTYPE,
                                      OpType, CURRENCYID.EditValue, UserMovement, UserID, GetAccountName, DiscountAccID)
        End If
        MyBase.SetData()
    End Sub
    Public Sub BANKDEPOSIT_Insert(ByVal Code As String, ByVal InsertDate As Date, ByVal BranchID As Integer, ByVal SafeAccID As ULong, ByVal AccFrom As ULong,
                                            AccTo As ULong, CODEID As ULong, BillVal As Decimal, IsDiscount As Boolean, DiscountFrom As Int32, DiscountType As Int32, OverAllTotal As Decimal,
                                            DiscountVal As Decimal, BillNo As String, Notes As String, IsActive As Boolean, IsUpdate As Boolean, TypeID As Integer, OperationTypeID As Integer,
                                            CurrencyID As Integer, MovementType2 As String, UserID As Integer, GetAccountName As String, DiscountAccID As ULong)
        Try
            Dim PRM(25) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(3) = New SqlParameter("@SafeAccID", SqlDbType.BigInt) With {.Value = SafeAccID}
            PRM(4) = New SqlParameter("@AccFrom", SqlDbType.BigInt) With {.Value = AccFrom}
            PRM(5) = New SqlParameter("@AccTo", SqlDbType.BigInt) With {.Value = AccTo}
            PRM(6) = New SqlParameter("@CODEID", SqlDbType.BigInt) With {.Value = CODEID}
            PRM(7) = New SqlParameter("@BillVal", SqlDbType.Decimal) With {.Value = BillVal}
            PRM(8) = New SqlParameter("@IsDiscount", SqlDbType.Bit) With {.Value = IsDiscount}
            PRM(9) = New SqlParameter("@DiscountFrom", SqlDbType.TinyInt) With {.Value = DiscountFrom}
            PRM(10) = New SqlParameter("@DiscountType", SqlDbType.TinyInt) With {.Value = DiscountType}
            PRM(11) = New SqlParameter("@OverAllTotal", SqlDbType.Decimal) With {.Value = OverAllTotal}
            PRM(12) = New SqlParameter("@DiscountVal", SqlDbType.Decimal) With {.Value = DiscountVal}
            PRM(13) = New SqlParameter("@BillNo", SqlDbType.NVarChar, 50) With {.Value = BillNo}
            PRM(14) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(15) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
            PRM(16) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(17) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
            PRM(18) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
            PRM(19) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            PRM(20) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = MovementType2}
            PRM(21) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PRM(22) = New SqlParameter("@GetAccountName", SqlDbType.NVarChar, -1) With {.Value = GetAccountName}
            PRM(23) = New SqlParameter("@DiscountAccID", SqlDbType.BigInt) With {.Value = DiscountAccID}
            PRM(24) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(25) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("CONDB_BankDipWdTb_Insert", PRM)
            If PRM(24).Value = 0 Then
                ErrorMessage(Me, "رسالة تنبيه", PRM(25).Value)
                If Me.IsUpdate = False Then
                    BankDipWdTb_MaxID(Me.LOADTYPE, Me.BranchID.EditValue)
                    Exit Sub
                End If

            End If
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                Me.Print()
            End If
            FrmSavedSuccessfully.Show()
            Me.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try


    End Sub
    Public Overrides Sub UPDATERECORD()
        IsDataValidLKP(BranchID)
        IsDataValidLKP(AccFrom)
        IsDataValidLKP(AccTo)
        IsDataValidTextEdit(CodeID)
        IsDataValidSpinEdit(BillVal)
        If LOADTYPE = 17 Or LOADTYPE = 19 Then
            IsDataValidSpinEdit(AccFromValue)
            IsDataValidSpinEdit(AccToValue)
        End If
        GetAccountName = AccFrom.Text
        If LOADTYPE = 48 Then
            OpType = 78
            UserMovement = "معالجة خطأ لإيداع بصك في حساب مشروع" & AccFrom.Text
        End If
        If IsUpdate = True Then
            BANKDEPOSIT_Insert(CodeID.Text.Trim, InsertDate.EditValue, BranchID.EditValue, UserID, AccFrom.EditValue, AccTo.EditValue, IDCode, BillVal.EditValue, 1,
                                     1, 0, BillVal.EditValue, 0, BillNo.Text.Trim, Notes.Text.Trim, 1, IsUpdate, LOADTYPE,
                                      OpType, CURRENCYID.EditValue, UserMovement, UserID, GetAccountName, DiscountAccID)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Function BankDipWdTb_SelectByCode(Code As String, TypeID As Int32) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_BankDipWdTb_SelectByCode", PRM)
        Return DT
    End Function
    Public Sub BANKDEPOSIT_GetRecord(X, T)
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = X}
        PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = T}
        Dim dt As New DataTable
        dt.Clear()
        dt = BankDipWdTb_SelectByCode(X, T)
        If dt.Rows.Count > 0 Then
            LoadBANK()
            CodeID.Text = dt.Rows(0)("Code").ToString
            InsertDate.EditValue = dt.Rows(0)("InsertDate")
            BranchID.EditValue = dt.Rows(0)("BranchID")
            LoadAccountType(dt.Rows(0)("BranchID"))
            AccountType.EditValue = dt.Rows(0)("AccParent")
            LOADBBRNACH2()
            LOADAccFromOrAccTo()
            AccFrom.EditValue = dt.Rows(0)("AccFrom")
            AccTo.EditValue = dt.Rows(0)("AccTo")
            CURRENCYID.EditValue = dt.Rows(0)("CurrencyID")
            BillVal.EditValue = dt.Rows(0)("BillVal")
            BillNo.Text = dt.Rows(0)("BillNo").ToString
            Notes.Text = dt.Rows(0)("Notes").ToString
            BankName.Text = GetBANKNAME
        End If
    End Sub

    Public Sub GETVALTOLTEl()
        Try
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = AccFrom.EditValue}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = 1}
            PR(2) = New SqlParameter("@LadType", SqlDbType.Int) With {.Value = LOADTYPE}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("[EMPORCUST_GetAccValCashOnly](@AccName,@crunseType,@LadType) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFromValue.EditValue = dt.Rows(0)("GetAccVal") * -1
                Else
                    AccFromValue.EditValue = dt.Rows(0)("GetAccVal")
                End If
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub

    Public Sub GETVALTOLTEl1()
        Try

            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = AccTo.EditValue}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = 1}
            PR(2) = New SqlParameter("@LoadType", SqlDbType.Int) With {.Value = LOADTYPE}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("[EMPORCUST_GetAccValCashOnly](@AccName,@crunseType,@LoadType) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                If LOADTYPE = 48 Then
                    AccToValue.EditValue = dt.Rows(0)("GetAccVal") * -1
                Else
                    AccToValue.EditValue = dt.Rows(0)("GetAccVal")
                End If
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Private Sub AccFrom_TextChanged(sender As Object, e As EventArgs) Handles AccFrom.TextChanged
        AccFromValue.EditValue = 0.000
        If AccFrom.Text <> String.Empty Or AccFrom.EditValue <> -1 Then
            GETVALTOLTEl()
            If LOADTYPE = 17 Or LOADTYPE = 19 Then
                LoadBANKNAME(AccFrom.EditValue)
            End If
        Else
            AccFromValue.EditValue = 0.000
        End If

    End Sub

    Private Sub AccTo_TextChanged(sender As Object, e As EventArgs) Handles AccTo.TextChanged
        AccToValue.EditValue = 0.000
        If AccTo.Text <> String.Empty Or AccTo.EditValue <> -1 Then
            GETVALTOLTEl1()
            If LOADTYPE = 48 Then
                LoadBANKNAME(AccTo.EditValue)
            End If
        Else
            AccToValue.EditValue = 0.000
        End If

    End Sub


    Private Sub BankName_TextChanged(sender As Object, e As EventArgs) Handles BankName.TextChanged
        If IsUpdate = False Then
            AccFrom.EditValue = -1
            AccTo.EditValue = -1
            AccFromValue.EditValue = 0.000
            AccToValue.EditValue = 0.000
            If BankName.EditValue <> -1 And BankName.Text <> String.Empty Then
                LOADBBRNACH(BankName.EditValue)
            End If
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWBANKDEPOSIT.ShowDialog()
    End Sub

    Private Sub AccountType_TextChanged(sender As Object, e As EventArgs) Handles AccountType.TextChanged
        LOADAccFromOrAccTo()
    End Sub

    Private Sub AccountType_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccountType.QueryPopUp
        If BranchID.EditValue <> -1 And BranchID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CONDB_AccountTB_LoadLine3ToLKP", PR)
            If dt.Rows.Count > 0 Then
                AccountType.Properties.PopulateColumns()
                AccountType.Properties.Columns("AccCode").Visible = False
            End If
        End If
    End Sub

    Private Sub me_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub me_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        BtnNew.PerformClick()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        Try
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@Code", CodeID.Text)
            PRM(1) = New SqlParameter("@TypeID", LOADTYPE)
            Dim dt As DataTable = RUN_QUARY_PRO("CONDB_ZRPT_BankDipWdTb_SelectByCode", PRM)
            Dim ds As New DataSet
            dt.TableName = "BankDipWdTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTProBANFDEPOSIT
                report.DataSource = ds
                report.DataMember = "BankDipWdTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.XrLabel36.Text = Cur_Code(CURRENCYID.Text, BillVal.EditValue, False, "n2")
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Public Sub BankDipWdTb_MaxID(TypeID As Integer, BranchID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.TinyInt) With {.Value = BranchID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_BankDipWdTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            CodeID.Text = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            BankDipWdTb_MaxID(LOADTYPE, BranchID.EditValue)
            LOADRECURRENCY()
        End If
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            BankDipWdTb_MaxID(LOADTYPE, BranchID.EditValue)
            LOADRECURRENCY()
            LoadAccountType(BranchID.EditValue)
            CURRENCYID.EditValue = 1
        End If
    End Sub

    Private Sub BankName_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BankName.QueryPopUp
        BankName.Properties.PopulateColumns()
        BankName.Properties.Columns("ID").Visible = False
    End Sub

End Class