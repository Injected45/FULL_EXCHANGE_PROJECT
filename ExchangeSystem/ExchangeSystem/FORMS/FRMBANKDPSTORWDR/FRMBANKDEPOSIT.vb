Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient
Public Class FRMBANKDEPOSIT
    Dim cslsbd As New CLSBANKDEPOSIT
    Public IDCode, DiscountAccID As ULong
    Public LOADTYPE, EMPID, OpType As Integer
    Dim UserMovement, GetAccountName As String
    Public GetBANKNAME As String
    Public IsUpdate, CanChangeSafe As Boolean
    Public isbanck As Int16
    Sub DISAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        BranchID.Enabled = False
        AccountType.Enabled = False
        BankName.Enabled = False
        SafeID.Enabled = False
        AccFrom.Enabled = False
        AccFromValue.Enabled = False
        AccToValue.Enabled = False
        AccTo.Enabled = False
        BillVal.Enabled = False
        IsDiscount.Enabled = False
        DiscountFrom.Enabled = False
        DiscountType.Enabled = False
        OverAllTotal.Enabled = False
        DiscountVal.Enabled = False
        CURRENCYID.Enabled = False
        BillNo.Enabled = False
        Notes.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        'BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = False
        BtnPrint.Enabled = True
        TxtName.Enabled = False
        TxtPhone.Enabled = False
        EXID.Enabled = False
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(FRmIDsql, UserID)


        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then
                SafeID.Enabled = False
                CanChangeSafe = False
            Else
                SafeID.Enabled = True
                CanChangeSafe = True
            End If
        End If


    End Sub
    Sub ENAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        BranchID.Enabled = False
        If CanChangeSafe = True Then
            SafeID.Enabled = True
        Else
            SafeID.Enabled = False
        End If
        AccountType.Enabled = True
        AccFrom.Enabled = True
        AccFromValue.Enabled = False
        AccTo.Enabled = True
        BankName.Enabled = True
        AccToValue.Enabled = False
        BillVal.Enabled = True
        IsDiscount.Enabled = True
        DiscountFrom.Enabled = True
        DiscountType.Enabled = True
        OverAllTotal.Enabled = False
        DiscountVal.Enabled = True
        CURRENCYID.Enabled = True
        BillNo.Enabled = True
        Notes.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        TxtName.Enabled = True
        TxtPhone.Enabled = True
        EXID.Enabled = False
    End Sub
    Sub NEWRECORD()
        LoadBANK()
        'LoadOwnAccountName()
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
        'LOADBBRNACH(BankName.EditValue)
        AccFrom.EditValue = -1
        AccTo.EditValue = -1
        BankName.EditValue = -1
        BranchID.EditValue = BID
        BranchID.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        AccFromValue.EditValue = 0.000
        AccToValue.EditValue = 0.000
        BillVal.EditValue = 0.000
        IsDiscount.SelectedIndex = -1
        DiscountFrom.SelectedIndex = -1
        DiscountType.SelectedIndex = -1
        OverAllTotal.EditValue = 0.000
        DiscountVal.EditValue = 0.000
        BillNo.Text = ""
        Notes.Text = ""
        SafeID.EditValue = UserAccID
        TxtName.Text = ""
        TxtPhone.Text = ""
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
        dt = RUN_QUARY_TXT("CoBranches_LoadconnectedBranch")
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
    'Sub LoadOwnAccountName()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_PRO_ONLY("BankDipWdTb_LoadOwnAccountName")
    '    If dt.Rows.Count > 0 Then
    '        TxtName.Properties.DataSource = dt
    '        TxtName.Properties.ValueMember = "OwnAccountPhone"
    '        TxtName.Properties.DisplayMember = "OwnAccountName"
    '        'TxtName.Properties.ShowHeader = False
    '    End If

    'End Sub
    Sub LoadBANK()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("BanksTb_LODE")
        If dt.Rows.Count > 0 Then
            BankName.Properties.DataSource = dt
            BankName.Properties.ValueMember = "ID"
            BankName.Properties.DisplayMember = "BankName"
            BankName.Properties.ShowHeader = False
        End If
    End Sub
    Sub LoadAccountType()
        AccountType.Properties.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AccountTB_LoadLine3ToLKP", PR)
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
                ElseIf LOADTYPE = 16 Or LOADTYPE = 18 Or LOADTYPE = 48 Then
                    AccTo.Properties.DataSource = dt
                    AccTo.Properties.ValueMember = "AccID"
                    AccTo.Properties.DisplayMember = "BranchName"
                    AccTo.Properties.ShowHeader = False
                End If
            Else
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFrom.Properties.DataSource = Nothing
                ElseIf LOADTYPE = 16 Or LOADTYPE = 18 Then
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
                ElseIf LOADTYPE = 16 Or LOADTYPE = 18 Or LOADTYPE = 48 Then
                    AccTo.Properties.DataSource = dt
                    AccTo.Properties.ValueMember = "AccID"
                    AccTo.Properties.DisplayMember = "BranchName"
                    AccTo.Properties.ShowHeader = False
                End If
            Else
                If LOADTYPE = 17 Or LOADTYPE = 19 Then
                    AccFrom.Properties.DataSource = Nothing
                ElseIf LOADTYPE = 16 Or LOADTYPE = 18 Then
                    AccTo.Properties.DataSource = Nothing
                End If
            End If
        End If
    End Sub
    Sub LOADAccFromOrAccTo()
        Try
            EXID.EditValue = -1
            If BranchID.Text <> String.Empty And AccountType.EditValue <> -1 Then
                Dim PR(3) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                PR(1) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = If(LOADTYPE = 17, 0, 1)}
                PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
                PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 1}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("BankWithdrawal_LOADINTOLKPBASEDONAccParent", PR)
                If dt.Rows.Count > 0 Then
                    If LOADTYPE = 16 Or LOADTYPE = 18 Or LOADTYPE = 48 Then
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

                If LOADTYPE = 17 And AccountType.Text.Contains("مشروعات") Then
                    EXID.Enabled = True
                Else
                    EXID.Enabled = False
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Sub LOADACCTYPE()

        Try
            EXID.EditValue = -1
            If BranchID.Text <> String.Empty And AccountType.EditValue <> -1 Then
                Dim PR(0) As SqlParameter

                PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccFrom.EditValue}

                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("AccountTB_SELECTLoadLine3ToLKP", PR)
                If dt.Rows.Count > 0 Then
                    If LOADTYPE = 16 Or LOADTYPE = 18 Or LOADTYPE = 48 Then
                        AccountType.Properties.DataSource = dt
                        AccountType.Properties.ValueMember = "AccCode"
                        AccountType.Properties.DisplayMember = "AccName"
                        AccountType.Properties.ShowHeader = False
                    End If
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Sub LoadSafeID()
        SafeID.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKP", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.DataSource = dt
                SafeID.Properties.ValueMember = "AccID"
                SafeID.Properties.DisplayMember = "UNAME"
                SafeID.Properties.ShowHeader = False
            ElseIf dt.Rows.Count = 0 Then
                SafeID.Properties.DataSource = Nothing
            End If
        End If
    End Sub
    Private Sub AccFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccFrom.QueryPopUp
        If LOADTYPE = 16 Or LOADTYPE = 18 Or LOADTYPE = 48 Then
            'If AccountType.EditValue <> -1 Or AccountType.Text <> String.Empty Then
            '    Dim PR(3) As SqlParameter
            '    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            '    PR(1) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = 0}
            '    PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            '    PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 1}
            '    Dim dt As New DataTable
            '    dt.Clear()
            '    dt = RUN_QUARY_PRO("Withdrawal_LOADINTOLKPBASEDONAccParent", PR)
            '    If dt.Rows.Count > 0 Then
            '        AccFrom.Properties.PopulateColumns()
            '        AccFrom.Properties.Columns("AccID").Visible = False
            '    End If
            'End If
            HideAllColumnsExceptDisplay(AccFrom)
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
        If LOADTYPE = 16 Or LOADTYPE = 18 Or LOADTYPE = 48 Then
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
            'Dim PR(3) As SqlParameter
            'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            'PR(1) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = 0}
            'PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            'PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 1}
            'Dim dt As New DataTable
            'dt.Clear()
            'dt = RUN_QUARY_PRO("Withdrawal_LOADINTOLKPBASEDONAccParent", PR)
            'If dt.Rows.Count > 0 Then
            '    AccTo.Properties.PopulateColumns()
            '    AccTo.Properties.Columns("AccID").Visible = False
            'End If
            HideAllColumnsExceptDisplay(AccTo)
        End If
    End Sub
    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        SafeID.Properties.PopulateColumns()
        SafeID.Properties.Columns("AccID").Visible = False
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        If isbanck = 6 Then

            BankName.EditValue = -1
            BankName.Enabled = False
            AccountType.Enabled = False
            AccFrom.Enabled = False
            AccTo.Enabled = False
            AccFromValue.EditValue = 0.000
            IsDiscount.Enabled = False
            DiscountFrom.SelectedIndex = 1
            BankName.EditValue = 1
            AccountType.EditValue = 20101401
            AccFrom.EditValue = 2262
            AccTo.EditValue = 2899
            DiscountFrom.Enabled = False
        Else
            BankName.EditValue = -1
            BankName.Enabled = True
            AccountType.Enabled = True
            AccFrom.Enabled = True
            AccTo.Enabled = True
            AccFromValue.EditValue = 0.000
            IsDiscount.Enabled = True
            DiscountFrom.Enabled = True
            NEWRECORD()
        End If
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public AccSafeID As ULong
    Public DiscountTp, DiscountF, IsDis As Integer
    Public Overrides Sub SetData()
        IsDataValidLKP(BranchID)
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يجب اختيار الخزنة"
            Exit Sub
        End If
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
        If IsDiscount.SelectedIndex = 0 Then
            If DiscountFrom.SelectedIndex = -1 Then
                DiscountFrom.ErrorText = "هذا الحقل لايجب أن يكون فارغ"
                Exit Sub
            End If
            If DiscountType.SelectedIndex = -1 Then
                DiscountType.ErrorText = "هذا الحقل لايجب أن يكون فارغ"
                Exit Sub
            End If
        End If
        If CURRENCYID.EditValue = -1 Then

            CURRENCYID.ErrorText = "يجب اختيار العملة"
            Exit Sub
        End If
        If BillNo.Text = String.Empty Then

            BillNo.ErrorText = "هذا الحقل لايجب أن يكون فارغ"
            Exit Sub
        End If
        If DiscountFrom.SelectedIndex = 0 Then
            If DiscountVal.EditValue <= 0.000 Then
                DiscountVal.ErrorText = "يجب اختيار قيمة العمولة"
                Exit Sub
            End If
        End If
        If IsDiscount.SelectedIndex = -1 Then
            IsDiscount.ErrorText = "يجب اختيار نوع الخصم"
            Exit Sub
        End If
        If LOADTYPE = 17 Or LOADTYPE = 19 Then
            If AccFromValue.EditValue <= 0 Then
                AccFromValue.ErrorText = "لا يوجد رصيد في الحساب"
                Exit Sub
            ElseIf OverAllTotal.EditValue > AccFromValue.EditValue Then
                AccFromValue.ErrorText = "رصيد الحساب غير كاف"
                Exit Sub
            End If
            If AccountType.Text.Contains("عمل") = False And AccountType.Text.Contains("مدينون") = False And AccountType.Text.Contains("شركاء") = False And AccountType.Text.Contains("مصروفات") = False And AccountType.Text.Contains("مشروعات") = False And AccountType.Text.Contains("مقاولون") = False And AccountType.Text.Contains("موردو") = False And AccountType.Text.Contains("ورشة") = False And AccountType.Text.Contains("رخام") = False And AccountType.Text.Contains("أجهزة") = False And AccountType.Text.Contains("أثاث") = False And AccountType.Text.Contains("برامج وبرمجيات") = False And AccountType.Text.Contains("مركبات") = False And AccountType.Text.Contains("مباني") = False And AccountType.Text.Contains("آلات ومعدات") = False Then
                If AccToValue.EditValue <= 0 Then
                    AccToValue.ErrorText = "لا يوجد رصيد في الحساب"
                    Exit Sub
                ElseIf OverAllTotal.EditValue > AccToValue.EditValue Then
                    AccToValue.ErrorText = "رصيد الحساب غير كاف"
                    Exit Sub
                End If
            End If

        End If
        IsDataValidComboBoxEdit(IsDiscount)
        If IsDiscount.SelectedIndex = 0 Then
            IsDataValidSpinEdit(DiscountVal)
            IsDataValidComboBoxEdit(DiscountFrom)
            IsDataValidComboBoxEdit(DiscountType)
        End If

        GetAccountName = AccFrom.Text
        If LOADTYPE = 16 Then
            OpType = 46
            UserMovement = "إيداع بصك في حساب الموظف" & AccFrom.Text
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccFrom.EditValue
            Else
                DiscountAccID = 0
            End If
        ElseIf LOADTYPE = 17 Then
            OpType = 47
            UserMovement = "سحب بصك من حساب الموظف" & AccTo.Text
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccTo.EditValue
            Else
                DiscountAccID = 0
            End If
        ElseIf LOADTYPE = 18 Then
            UserMovement = "إيداع بصك في حساب العميل" & AccFrom.Text
            OpType = 48
            If DiscountFrom.SelectedIndex = 0 Then

                DiscountAccID = AccFrom.EditValue
            Else
                DiscountAccID = 0
            End If
        ElseIf LOADTYPE = 19 Then
            OpType = 49
            UserMovement = "سحب بصك من حساب العميل" & AccTo.Text
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccTo.EditValue
            Else
                DiscountAccID = 0
            End If
        End If
        If DiscountType.SelectedIndex = 0 Then
            DiscountTp = 0
        ElseIf DiscountType.SelectedIndex = 1 Then
            DiscountTp = 1
        End If
        If DiscountFrom.SelectedIndex = 0 Then
            DiscountF = 0
        ElseIf DiscountFrom.SelectedIndex = 1 Then
            DiscountF = 1
        End If
        If IsDiscount.SelectedIndex = 0 Then
            IsDis = 0
        ElseIf IsDiscount.SelectedIndex = 1 Then
            IsDis = 1
        End If
        Dim ExpenssID As Integer
        If LOADTYPE = 17 And AccountType.Text.Contains("مشروعات") Then
            ExpenssID = EXID.EditValue
        Else
            ExpenssID = 0
        End If
        If IsUpdate = 0 Then
            cslsbd.BANKDEPOSIT_Insert(CodeID.Text.Trim, InsertDate.EditValue, BranchID.EditValue, SafeID.EditValue, AccFrom.EditValue, AccTo.EditValue, IDCode, BillVal.EditValue, IsDiscount.SelectedIndex,
                                      DiscountF, DiscountTp, OverAllTotal.EditValue, DiscountVal.EditValue, BillNo.Text.Trim, Notes.Text.Trim, IsActive, IsUpdate, LOADTYPE,
                                      OpType, CURRENCYID.EditValue, UserMovement, UserID, GetAccountName, DiscountAccID, TxtName.Text, TxtPhone.Text, ExpenssID, AccountType.EditValue)
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub UPDATERECORD()
        IsDataValidLKP(BranchID)
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يجب اختيار الخزنة"
            Exit Sub
        End If
        IsDataValidLKP(AccFrom)
        IsDataValidLKP(AccTo)
        IsDataValidTextEdit(CodeID)
        IsDataValidSpinEdit(BillVal)
        If LOADTYPE = 17 Or LOADTYPE = 19 Then
            IsDataValidSpinEdit(AccFromValue)
            IsDataValidSpinEdit(AccToValue)
        End If
        IsDataValidComboBoxEdit(IsDiscount)
        If IsDiscount.SelectedIndex = 0 Then
            IsDataValidSpinEdit(DiscountVal)
            IsDataValidComboBoxEdit(DiscountFrom)
            IsDataValidComboBoxEdit(DiscountType)
        End If
        GetAccountName = AccFrom.Text
        Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
        Dim lookAndFeelError2 As New UserLookAndFeel(Me)
        lookAndFeelError2.Style = LookAndFeelStyle.Skin
        lookAndFeelError2.UseDefaultLookAndFeel = False
        lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim result = XtraMessageBox.Show(lookAndFeelError2, "في حال الموافقة سيتم استرجاع القيمة هل تريد المتابعة؟", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.No Then
            Exit Sub
        End If
        If LOADTYPE = 16 Then
            OpType = 46
            UserMovement = "معالجة بصك في حساب الموظف" & AccFrom.Text
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccFrom.EditValue
            Else
                DiscountAccID = 0
            End If
        ElseIf LOADTYPE = 17 Then
            OpType = 47
            UserMovement = "معالجة سحب بصك من حساب الموظف" & AccTo.Text
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccTo.EditValue
            Else
                DiscountAccID = 0
            End If
        ElseIf LOADTYPE = 18 Then
            UserMovement = "معالجة إيداع بصك في حساب العميل" & AccFrom.Text
            OpType = 48
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccFrom.EditValue
            Else
                DiscountAccID = 0
            End If
        ElseIf LOADTYPE = 19 Then
            OpType = 49
            UserMovement = "معالجة سحب بصك من حساب العميل" & AccTo.Text
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountAccID = AccTo.EditValue
            Else
                DiscountAccID = 0
            End If
        End If
        Dim ExpenssID As Integer
        If LOADTYPE = 17 And AccountType.Text.Contains("مشروعات") Then
            ExpenssID = EXID.EditValue
        Else
            ExpenssID = 0
        End If
        If IsUpdate = True Then
            cslsbd.BANKDEPOSIT_Insert(CodeID.Text.Trim, InsertDate.EditValue, BranchID.EditValue, SafeID.EditValue, AccFrom.EditValue, AccTo.EditValue, IDCode, BillVal.EditValue, IsDiscount.SelectedIndex,
                                      DiscountF, DiscountTp, OverAllTotal.EditValue, DiscountVal.EditValue, BillNo.Text.Trim, Notes.Text.Trim, IsActive, IsUpdate, LOADTYPE,
                                      OpType, CURRENCYID.EditValue, UserMovement, UserID, GetAccountName, DiscountAccID, TxtName.Text, TxtPhone.Text, ExpenssID, AccountType.EditValue)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Sub BANKDEPOSIT_GetRecord(X, T)
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = X}
        PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = T}
        Dim dt As New DataTable
        dt.Clear()
        dt = cslsbd.BankDipWdTb_SelectByCode(X, T)
        If dt.Rows.Count > 0 Then
            LoadBANK()

            CodeID.Text = dt.Rows(0)("Code").ToString
            InsertDate.EditValue = dt.Rows(0)("InsertDate")
            BranchID.EditValue = dt.Rows(0)("BranchID")
            If LOADTYPE = 17 Then
                AccountType.EditValue = dt.Rows(0)("AccParent")
            End If
            If LOADTYPE = 16 Then
                AccountType.EditValue = dt.Rows(0)("AccParCode")
            End If
            SafeID.EditValue = dt.Rows(0)("SafeAccID")
            LOADBBRNACH2()
            AccFrom.EditValue = dt.Rows(0)("AccFrom")
            AccTo.EditValue = dt.Rows(0)("AccTo")
            CURRENCYID.EditValue = dt.Rows(0)("CurrencyID")
            BillVal.EditValue = dt.Rows(0)("BillVal")
            DiscountType.SelectedIndex = dt.Rows(0)("DiscountType")
            OverAllTotal.EditValue = dt.Rows(0)("OverAllTotal")
            DiscountVal.EditValue = dt.Rows(0)("DiscountVal")
            BillNo.Text = dt.Rows(0)("BillNo").ToString
            Notes.Text = dt.Rows(0)("Notes").ToString
            DiscountFrom.SelectedIndex = dt.Rows(0)("DiscountFrom")
            If dt.Rows(0)("DiscountFrom") = 1 Then
                DiscountType.SelectedIndex = -1
            End If
            TxtName.Text = dt.Rows(0)("OwnAccountName").ToString
            TxtPhone.Text = dt.Rows(0)("OwnAccountPhone").ToString
            BankName.Text = GetBANKNAME
        End If
    End Sub
    Private Sub DiscountType_TextChanged(sender As Object, e As EventArgs) Handles DiscountType.TextChanged
        If IsUpdate = 0 Then
            DiscountVal.EditValue = 0.000
        End If
    End Sub
    Private Sub DiscountVal_TextChanged(sender As Object, e As EventArgs) Handles DiscountVal.TextChanged
        If IsUpdate = 0 Then
            If DiscountType.SelectedIndex = 1 Then
                If DiscountFrom.SelectedIndex = 0 Then
                    If LOADTYPE = 16 Or LOADTYPE = 18 Then
                        OverAllTotal.EditValue = BillVal.EditValue - DiscountVal.EditValue
                    Else
                        OverAllTotal.EditValue = BillVal.EditValue + DiscountVal.EditValue
                    End If
                ElseIf DiscountFrom.SelectedIndex = 1 Then
                    OverAllTotal.EditValue = BillVal.EditValue + DiscountVal.EditValue
                End If
            Else
                OverAllTotal.EditValue = BillVal.EditValue
            End If
        End If
    End Sub

    Private Sub BillVal_TextChanged(sender As Object, e As EventArgs) Handles BillVal.TextChanged
        If IsUpdate = 0 Then
            If DiscountVal.EditValue = 0 Then
                DiscountVal.EditValue = BillVal.EditValue
            Else
                If DiscountType.SelectedIndex = 1 Then
                    If DiscountFrom.SelectedIndex = 0 Then
                        OverAllTotal.EditValue = BillVal.EditValue - DiscountVal.EditValue

                    ElseIf DiscountFrom.SelectedIndex = 1 Then
                        OverAllTotal.EditValue = BillVal.EditValue + DiscountVal.EditValue
                    End If
                Else
                    OverAllTotal.EditValue = BillVal.EditValue
                End If
            End If
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


    Private Sub AccFrom_TextChanged(sender As Object, e As EventArgs) Handles AccFrom.TextChanged
        AccFromValue.EditValue = 0.000
        If AccFrom.Text <> String.Empty Or AccFrom.EditValue <> -1 Then
            AccFromValue.EditValue = GetLKPColumnVal(AccFrom, "AccVal")
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
            AccToValue.EditValue = GetLKPColumnVal(AccTo, "AccVal")
            If LOADTYPE = 16 Or LOADTYPE = 18 Then
                LoadBANKNAME(AccTo.EditValue)
            End If
        Else
            AccToValue.EditValue = 0.000
        End If
        If AccountType.Text.Contains("مشروعات") = False Then
            If LOADTYPE = 17 And AccTo.Text.Contains("مصروفات") Then
                EXID.Enabled = True
            Else
                EXID.Enabled = False
            End If
        End If
    End Sub

    Private Sub DiscountFrom_TextChanged(sender As Object, e As EventArgs) Handles DiscountFrom.TextChanged
        If IsUpdate = 0 Then
            DiscountVal.EditValue = 0.000
            If DiscountFrom.SelectedIndex = 0 Then
                DiscountVal.EditValue = 0.000
                IsDiscount.SelectedIndex = 0
                DiscountType.SelectedIndex = -1
                DiscountType.Enabled = True
                DiscountVal.Enabled = True
            Else
                DiscountVal.EditValue = 0.000
                IsDiscount.SelectedIndex = 1
                DiscountType.Enabled = False
                DiscountVal.Enabled = False
            End If
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
            dt = RUN_QUARY_PRO("AccountTB_LoadLine3ToLKP", PR)
            If dt.Rows.Count > 0 Then
                AccountType.Properties.PopulateColumns()
                AccountType.Properties.Columns("AccCode").Visible = False
            End If

        End If

    End Sub

    Private Sub FRMBANKDEPOSIT_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub EXID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EXID.QueryPopUp
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[CONDB_ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
            If dt.Rows.Count > 0 Then
                EXID.Properties.PopulateColumns()
                EXID.Properties.Columns("ID").Visible = False
            End If
        End If
    End Sub

    Private Sub TxtName_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles TxtName.ButtonClick
        FRMLoadOwnName.ShowDialog()
    End Sub







    Private Sub FRMBANKDEPOSIT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        BtnNew.PerformClick()
        If UserType = 1 Then
            SafeID.Enabled = True
        Else
            SafeID.Enabled = False
        End If
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
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_BankDipWdTb_SelectByCode", PRM)
            dt.TableName = "BankDipWdTb"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTBANKDEPOSIT
                report.DataSource = ds
                report.DataMember = "BankDipWdTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                If LOADTYPE = 17 And AccountType.Text.Contains("مشروعات") Then
                    report.XrPictureBox8.Visible = True
                    report.XrLabel27.Visible = True
                    report.XrLabel26.Visible = True
                Else
                    report.XrPictureBox8.Visible = False
                    report.XrLabel27.Visible = False
                    report.XrLabel26.Visible = False
                End If
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            cslsbd.BankDipWdTb_MaxID(LOADTYPE, BranchID.EditValue)
            LoadSafeID()
            SafeID.EditValue = UserAccID
            'LOADBBRNACH(BankName.EditValue)

            LOADRECURRENCY()
        End If
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            cslsbd.BankDipWdTb_MaxID(LOADTYPE, BranchID.EditValue)
            LoadSafeID()
            'LOADBBRNACH(BankName.EditValue)
            'LOADEXPANSTYPE()
            LOADRECURRENCY()
            LoadAccountType()
            CURRENCYID.EditValue = 1
            SafeID.EditValue = UserAccID
        End If
    End Sub

    Private Sub BankName_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BankName.QueryPopUp
        BankName.Properties.PopulateColumns()
        BankName.Properties.Columns("ID").Visible = False
    End Sub

    Sub LOADEXPANSTYPE()

        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[CONDB_ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
            If dt.Rows.Count > 0 Then
                EXID.Properties.DataSource = dt
                EXID.Properties.ValueMember = "ID"
                EXID.Properties.DisplayMember = "ExName"
                EXID.Properties.ShowHeader = False
                'EXID.Properties.PopulateColumns()
                'EXID.Properties.Columns(0).Visible = False
            End If
        Else
            EXID.Enabled = False
            EXID.Properties.DataSource = Nothing
        End If
    End Sub

    'Private Sub TxtName_QueryPopUp(sender As Object, e As CancelEventArgs)
    '    TxtName.Properties.PopulateColumns()
    '    TxtName.Properties.Columns("OwnAccountPhone").Visible = False
    'End Sub
End Class