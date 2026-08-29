Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CurrencySafeTransfer
    Dim clsctm As New CLSCURRENCYTRANSFERMAINSAFES
    Dim clsaem As New CLSACCEMPACTIVITY
    Dim clsdc As New CLSDAILYCLOSE
    Dim accsafe As New CLSAccSafeActivity
    Dim curID As Integer
    Public IsUpdate As Boolean
    Public Overrides Sub SetData()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الخزنة المنقول منها"
            Exit Sub
        End If
        If WithdrawalTo.EditValue = -1 Then
            WithdrawalTo.ErrorText = "يرجى اختيار الخزنة المنقول إليها"
            Exit Sub
        End If
        If Val(Convert.ToDecimal(WithdrawalValue.Text)) > Val(Convert.ToDecimal(SafeBalance.Text)) Then
            WithdrawalValue.ErrorText = "القيمة المنقولة لا يجب أن تكون أكبر من القيمة الموجودة"
            Exit Sub
        End If
        If WithdrawalValue.Text = "0.000" Or WithdrawalValue.Text = String.Empty Then
            WithdrawalValue.ErrorText = "القيمة المنقولة لا يجب أن تكون صفر أو فارغة"
            Exit Sub
        End If
        If SafeBalance.Text = "0.000" Or SafeBalance.Text = String.Empty Or Val(Convert.ToDouble(SafeBalance.Text < 0.000)) Then
            WithdrawalValue.ErrorText = "قيمة رصيد الخزنة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If
        Dim DCode As String
        Dim DID As Integer
        DID = GETMAXID("DailyCloseTb", "ID") + 1
        DCode = "1" & "0" & "17" & "-" & DID
        clsdc.INSERTTB_DailyCloseTb(WithdrawalDate.EditValue, DCode, curID, WithdrawalFrom.EditValue, WithdrawalTo.EditValue, 0.000, Convert.ToDecimal(WithdrawalValue.Text), 0.000, 0.000, 0.000,
                                       0.000, 0.000, 0.000, 0.000, UserID, BranchID.EditValue, 2)

        clsctm.INSERTTB_CURRENCYTB(WithdrawalDate.EditValue, WithdrawalFrom.EditValue, Convert.ToDecimal(WithdrawalValue.Text), 0.000, curID, BranchID.EditValue, UserID, Notes.Text, WDCode.Text, WithdrawalTo.EditValue)
        clsctm.INSERTTB_CURRENCYTB(WithdrawalDate.EditValue, WithdrawalTo.EditValue, 0.000, Convert.ToDecimal(WithdrawalValue.Text), curID, BranchID.EditValue, UserID, Notes.Text, WDCode.Text, WithdrawalFrom.EditValue)
        clsaem.INSERTTB_ACCEMPACTIVITY(WDCode.Text.Trim, WithdrawalFrom.EditValue, 0.000, Convert.ToDecimal(WithdrawalValue.Text), WithdrawalDate.EditValue, Notes.Text.Trim, WDCode.Text.Trim, True, 1, 16, True, BranchID.EditValue, "نقل من خزنة" & Space(1) & WithdrawalFrom.Text & Space(1) & "إلى خزنة" & Space(1) & WithdrawalTo.Text, WithdrawalFrom.EditValue)
        clsaem.INSERTTB_ACCEMPACTIVITY(WDCode.Text.Trim, WithdrawalTo.EditValue, Convert.ToDecimal(WithdrawalValue.Text), 0.000, WithdrawalDate.EditValue, Notes.Text.Trim, WDCode.Text.Trim, True, 1, 16, True, BranchID.EditValue, "نقل من خزنة" & Space(1) & WithdrawalTo.Text & Space(1) & "إلى خزنة" & Space(1) & WithdrawalFrom.Text, WithdrawalFrom.EditValue)


        accsafe.AccSafeActivityTb_InsertSafeTrance(UserID, 0.000, WithdrawalValue.EditValue, WithdrawalDate.EditValue, WDCode.Text.Trim, BranchID.EditValue,
                                                       WithdrawalFrom.EditValue, WithdrawalTo.EditValue, "نقل إلى" & Space(1) & WithdrawalTo.Text, curID, Notes.Text.Trim)
        accsafe.AccSafeActivityTb_InsertSafeTrance(UserID, WithdrawalValue.EditValue, 0.000, WithdrawalDate.EditValue, WDCode.Text.Trim, BranchID.EditValue,
                                                       WithdrawalTo.EditValue, WithdrawalFrom.EditValue, "نقل من" & Space(1) & WithdrawalFrom.Text, curID, Notes.Text.Trim)
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        NEWRECORD()
        MyBase.Save()
    End Sub
    Sub LOADBRANCHES()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CoBranches_LoadDataIntoLookUpEdit")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADWithdrawalFrom()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("TB_Users_SelectCurrencySafe")
        If DT.Rows.Count > 0 Then
            WithdrawalFrom.Properties.DataSource = DT
            WithdrawalFrom.Properties.ValueMember = "USID"
            WithdrawalFrom.Properties.DisplayMember = "UName"
            WithdrawalFrom.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADWITHDRAWALTO()
        If BranchID.EditValue <> -1 Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PRM(0).Value = BranchID.EditValue
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("TB_Users_LoadUSERBYBRANCH", PRM)
            If DT.Rows.Count > 0 Then
                WithdrawalTo.Properties.DataSource = DT
                WithdrawalTo.Properties.ValueMember = "USID"
                WithdrawalTo.Properties.DisplayMember = "UName"
                WithdrawalTo.Properties.ShowHeader = False
            End If
        End If
    End Sub


    Sub NEWRECORD()
        IsUpdate = False
        LCI.Text = "رصيد الخزنة"
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        WDCode.Text = BID & "0" & 16 & "-" & GETMAXID("CurrencyActivityTb", "ID") + 1
        'If GProfIDLog > 1 Then
        '    LOADBRANCHES()
        '    BranchID.EditValue = BID
        '    BranchID.Enabled = False
        '    'BranchID.ReadOnly = False
        '    WithdrawalFrom.Properties.Columns("ID").Visible = False
        '    WithdrawalTo.Properties.Columns("ID").Visible = False

        'ElseIf GProfIDLog = 1 Then

        '    BranchID.Enabled = True
        '    'BranchID.ReadOnly = True
        '    LOADBRANCHES()
        'End If
        LOADBRANCHES()


        WDCode.Enabled = False
        WDCode.ReadOnly = True
        WithdrawalDate.EditValue = Date.Now
        WithdrawalDate.Enabled = False
        WithdrawalDate.ReadOnly = True
        'BranchID.EditValue = -1
        WithdrawalFrom.EditValue = -1
        SafeBalance.Text = "0.000"
        WithdrawalTo.EditValue = -1
        WithdrawalValue.Text = "0.000"
        Notes.Text = ""
    End Sub
    Private Sub WithdrawalFrom_TextChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.TextChanged
        If BranchID.EditValue <> -1 Then
            If WithdrawalFrom.EditValue <> -1 Then

                Dim PRM(1) As SqlParameter
                PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                PRM(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = curID}

                Dim DT As New DataTable
                DT.Clear()
                DT = RUN_QUARY_PRO("AccCurAct_GetWhoHasValue", PRM)
                If DT.Rows.Count = 0 Then
                    MsgBox("لا يوجد رصيد في الخزنة من العملة المختارة")
                    Exit Sub
                Else
                    SafeBalance.EditValue = DT.Rows(0)("aa")
                End If
            End If
        End If

    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub WithdrawalFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles WithdrawalFrom.QueryPopUp
        If BranchID.EditValue <= -1 Then
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookAndFeelError, "يرجى اختيار الخزنة أولاً", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        WithdrawalFrom.Properties.PopulateColumns()
        WithdrawalFrom.Properties.Columns("USID").Visible = False
        WithdrawalFrom.Properties.Columns("ID").Visible = False
    End Sub

    Private Sub WithdrawalTo_QueryPopUp(sender As Object, e As CancelEventArgs) Handles WithdrawalTo.QueryPopUp
        If BranchID.EditValue <= -1 Then
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookAndFeelError, "يرجى اختيار الخزنة أولاً", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

    End Sub

    Private Sub CurrencySafeTransfer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub WithdrawalFrom_EditValueChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.EditValueChanged
        If WithdrawalFrom.EditValue <> -1 Then
            Dim editor As LookUpEdit = CType(sender, LookUpEdit)
            Dim row As DataRowView = CType(editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue), DataRowView)
            Dim value As Object = row("ID")
            curID = value
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged

        If BranchID.EditValue <> -1 Then
            LOADWithdrawalFrom()
            LOADWITHDRAWALTO()
        End If
    End Sub
End Class