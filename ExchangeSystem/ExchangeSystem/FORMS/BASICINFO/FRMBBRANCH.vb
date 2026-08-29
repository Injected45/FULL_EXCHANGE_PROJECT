Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Controls

Public Class FRMBBRANCH
    Public IsUpdate, msgST As Boolean
    Dim bbcls As New BRANCHCLSS
    Public AccID As ULong
    'Public Sub CHECKBUTTONS()
    '    CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
    '    If DT.Rows.Count > 0 Then
    '        If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
    '        If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True
    '    End If
    'End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(79, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always



        End If


    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        BranchName.Text = ""
        AccountNo.Text = ""
        IBAN.Text = ""
        MobileNo.Text = ""
        AuthorizedToSign.Text = ""
        LOADDelegate()
        LOADBRNCHDIERCT(BranchID)
        LOADBANK()
        BankID.EditValue = -1
        BranchID.EditValue = -1
        DelegateID.EditValue = -1
        CurrencyID.EditValue = -1
        CurrencyID.Enabled = False
        CodeID.Enabled = False
        BankID.Select()
        IsActiveTG.IsOn = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        AccountType.Enabled = True
        CodeID.Text = GETMAXID("BBranchTb", "ID") + 1
        BankID.Enabled = True
        LOADDATA()
        LSBOX.SelectedIndex = -1
        If UserType = 1 Then
            BranchID.Enabled = True
        Else
            BranchID.Enabled = False
        End If
        LoadToControlar(CurrencyID, "CurrencyMainTb_LOADTOLKP", "CuName", "ID", Nothing)
    End Sub
    Public Sub LOADBANK()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("BanksTb_SelectAll")
        BankID.Properties.DataSource = DT
        BankID.Properties.ValueMember = "ID"
        BankID.Properties.DisplayMember = "BankName"
        BankID.Properties.ShowHeader = False
    End Sub
    Public Sub LOADDelegate()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("DelegateTb_LOADTOLBX")
        If DT.Rows.Count > 0 Then
            DelegateID.Properties.DataSource = DT
            DelegateID.Properties.DisplayMember = "DNAME"
            DelegateID.Properties.ValueMember = "ID"
            DelegateID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub DelegateID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles DelegateID.QueryPopUp
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("DelegateTb_LOADTOLBX")
        If DT.Rows.Count > 0 Then
            DelegateID.Properties.PopulateColumns()
            DelegateID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Private Sub BankID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BankID.QueryPopUp
        BankID.Properties.PopulateColumns()
        BankID.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub BankID_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BankID.ButtonClick
        If e.Button.Index = 1 Then
            FRMBANK.ShowDialog()
        End If
    End Sub
    Private Sub DelegateID_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles DelegateID.ButtonClick
        If e.Button.Index = 1 Then
            FRMDELEGATE.ShowDialog()
        End If
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("BBranchTb_LOADTOLBX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "BranchName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            Dim dt As New DataTable
            dt.Clear()
            dt = bbcls.EMPCSFT_CHECKPOHNE(BranchName.Text.Trim)
            If dt.Rows.Count > 0 Then
                ErrorMessage(Me, "رسالة خطأ", "اسم الفرع موجود مسبقاً")
                Exit Sub
            End If
            If BranchName.Text = String.Empty Then
                BranchName.ErrorText = "هذا الحقل مطلوب"
                Return
                If AccountNo.Text = String.Empty Then
                    AccountNo.ErrorText = "هذا الحقل مطلوب"
                    Return
                End If
            End If
            If BankID.EditValue = -1 Or BankID.Text = String.Empty Then
                BankID.ErrorText = "يجب اختيار المصرف"
                Return
            End If
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If DelegateID.EditValue = -1 Or DelegateID.Text = String.Empty Then
                DelegateID.ErrorText = "يرجى اختيار المندوب"
                Return
            End If
            If AccountType.SelectedIndex = -1 Or AccountType.Text = String.Empty Then
                AccountType.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            IsActiveTG.EditValue = True
            Try
                bbcls.EMPCSFT_INSERT(CodeID.Text, BankID.EditValue, BranchName.Text.Trim, AccountNo.Text.Trim, IBAN.Text.Trim, BranchID.EditValue, MobileNo.Text.Trim, AuthorizedToSign.Text.Trim, DelegateID.EditValue,
                                 AccID, IsActiveTG.EditValue, IsUpdate, AccountType.SelectedIndex, CurrencyID.EditValue)
            Catch ex As Exception
                ErrorMessage(Me, "رسالة خطأ", ex.Message)
            End Try
        End If
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If BranchName.Text = String.Empty Then
                BranchName.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If AccountNo.Text = String.Empty Then
                AccountNo.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If BankID.EditValue = -1 Or BankID.Text = String.Empty Then
                BankID.ErrorText = "يجب اختيار المصرف"
                Return
            End If
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If DelegateID.EditValue = -1 Or DelegateID.Text = String.Empty Then
                DelegateID.ErrorText = "يرجى اختيار المندوب"
                Return
            End If
            If AccountType.SelectedIndex = -1 Or AccountType.Text = String.Empty Then
                AccountType.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            Try
                bbcls.EMPCSFT_INSERT(CodeID.Text, BankID.EditValue, BranchName.Text.Trim, AccountNo.Text.Trim, IBAN.Text.Trim, BranchID.EditValue, MobileNo.Text.Trim, AuthorizedToSign.Text.Trim, DelegateID.EditValue,
                                 AccID, IsActiveTG.EditValue, IsUpdate, AccountType.SelectedIndex, CurrencyID.EditValue)
            Catch ex As Exception
                ErrorMessage(Me, "رسالة خطأ", ex.Message)
            End Try
        End If
    End Sub

    Private Sub FRMDELEGATE_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True 'this will prevent ding sound 
    End Sub
    Private Sub FRMBBRANCH_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        'CHECKBUTTONS()
        NEWRECORD()
    End Sub

    Private Sub AccountType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AccountType.SelectedIndexChanged
        If IsEmpty(AccountType) Then Exit Sub
        If AccountType.SelectedIndex = 2 And IsUpdate = False Then
            CurrencyID.EditValue = -1
            CurrencyID.Enabled = True
        Else
            CurrencyID.EditValue = DefaultCurrency
            CurrencyID.Enabled = False
        End If
    End Sub

    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        For I As Integer = 0 To LSBOX.Items.Count - 1
            Dim DT As New DataTable
            DT.Clear()
            DT = bbcls.EMPCSFT_Select(LSBOX.SelectedValue)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                BranchName.Text = DT.Rows(0)("BranchName").ToString
                MobileNo.Text = DT.Rows(0)("MobileNo").ToString
                AccountNo.Text = DT.Rows(0)("AccountNo").ToString
                AuthorizedToSign.Text = DT.Rows(0)("AuthorizedToSign").ToString
                BankID.EditValue = DT.Rows(0)("BankID")
                BranchID.EditValue = DT.Rows(0)("BranchID")
                CodeID.Text = DT.Rows(0)("ID")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
                DelegateID.EditValue = DT.Rows(0)("DelegateID")
                AccountType.SelectedIndex = DT.Rows(0)("AccountType")
                CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
                IBAN.Text = DT.Rows(0)("IBAN").ToString
                BankID.Enabled = False
                AccountType.Enabled = False
            End If
        Next
    End Sub
End Class
Public Class BRANCHCLSS
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BBranchTb_SelectByID", PRM)
        Return DT
    End Function
    Public Function EMPCSFT_SelectAll() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("BBranchTb_SelectAll")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(ID As Integer, BankID As Integer, BranchName As String, AccountNo As String, IBAN As String, BranchID As Integer, MobileNo As String, AuthorizedToSign As String, DelegateID As Integer,
                              AccID As ULong, IsActive As Boolean, IsUpdate As Boolean, AccountType As Int32, CurrID As Integer)
        Dim PRM(15) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID}
        PRM(2) = New SqlParameter("@BranchName", SqlDbType.NVarChar, -1) With {.Value = BranchName}
        PRM(3) = New SqlParameter("@AccountNo", SqlDbType.NVarChar, -1) With {.Value = AccountNo}
        PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(5) = New SqlParameter("@MobileNo", SqlDbType.NVarChar, -1) With {.Value = MobileNo}
        PRM(6) = New SqlParameter("@AuthorizedToSign", SqlDbType.NVarChar, -1) With {.Value = AuthorizedToSign}
        PRM(7) = New SqlParameter("@DelegateID", SqlDbType.Int) With {.Value = DelegateID}
        PRM(8) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID}
        PRM(9) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(10) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(11) = New SqlParameter("@AccountType", SqlDbType.TinyInt) With {.Value = AccountType}
        PRM(12) = New SqlParameter("@msgIN", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(13) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(14) = New SqlParameter("@IBAN", SqlDbType.NVarChar, -1) With {.Value = IBAN}
        PRM(15) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrID}
        RUN_EXUTE_PRO("BBranchTb_Insert", PRM)
        If PRM(12).Value = 0 Then
            ErrorMessage(FRMBBRANCH, "رسالة خطأ", PRM(13).Value.ToString)
            Exit Sub
        Else
            FRMBBRANCH.msgST = 1
            If IsUpdate = 0 Then
                FrmSavedSuccessfully.Show()
                FRMBBRANCH.NEWRECORD()
            ElseIf IsUpdate = 1 Then
                FRMBBRANCH.msgST = 1
                FrmEditMessage.Show()
                FRMBBRANCH.NEWRECORD()
            End If
        End If
    End Sub
    Public Function EMPCSFT_CHECKPOHNE(BranchName As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchName", SqlDbType.NVarChar, -1) With {.Value = BranchName}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BBranchTb_CHECKNAME", PRM)
        Return DT
    End Function
End Class