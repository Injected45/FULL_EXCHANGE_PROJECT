Imports System.Data.SqlClient

Public Class FRMEXBBRANCH
    Public IsUpdate As Integer, msgST As Boolean
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
        'Dim dt As New DataTable
        'dt.Clear()
        'dt = SElectUEserFormButtn(79, UserID)

        'If dt.Rows.Count > 0 Then
        '    If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        '    If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always



        'End If


    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        BranchName.Text = ""
        LOADBANK()
        BankID.EditValue = -1
        CodeID.Enabled = False
        BankID.Select()
        IsActiveTG.IsOn = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("ExBBranchTb", "ID") + 1
        BankID.Enabled = True
        LOADDATA()
        LSBOX.SelectedIndex = -1
    End Sub
    Public Sub LOADBANK()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 6}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExBanksTb_CRUD", PR)
        BankID.Properties.DataSource = DT
        BankID.Properties.ValueMember = "ID"
        BankID.Properties.DisplayMember = "BankName"
        BankID.Properties.ShowHeader = False
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 5}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExBBranchTb_CRUD", PR)
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "BranchName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
    End Sub
    Public Sub EMPCSFT_INSERT()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text}
        PRM(1) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID.EditValue}
        PRM(2) = New SqlParameter("@BranchName", SqlDbType.NVarChar, -1) With {.Value = BranchName.Text.Trim}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}
        PRM(4) = New SqlParameter("@Action", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("ExBBranchTb_CRUD", PRM)
        If IsUpdate = 0 Then
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        ElseIf IsUpdate = 1 Then
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        IsUpdate = 0

        If BranchName.Text = String.Empty Then
                BranchName.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If BankID.EditValue = -1 Or BankID.Text = String.Empty Then
                BankID.ErrorText = "يجب اختيار المصرف"
                Return
            End If
            IsActiveTG.EditValue = True
        Try
            EMPCSFT_INSERT()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        IsUpdate = 1
        If BranchName.Text = String.Empty Then
            BranchName.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If BankID.EditValue = -1 Or BankID.Text = String.Empty Then
                BankID.ErrorText = "يجب اختيار المصرف"
            Return
        End If
        Try
            EMPCSFT_INSERT()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub


    Private Sub FRMBBRANCH_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        'CHECKBUTTONS()
        NEWRECORD()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 3}
            prm(1) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = SafeToInt(LSBOX.SelectedValue)}

            Dim dt As DataTable = RUN_QUARY_PRO_alter("ExBBranchTb_CRUD", prm)
            If dt.Rows.Count <= 0 Then Exit Sub
            BtnSave.Enabled = False
            BtnEdit.Enabled = True
            BranchName.Text = dt.Rows(0)("BranchName").ToString
            BankID.EditValue = dt.Rows(0)("BankID")
            CodeID.Text = dt.Rows(0)("ID")
            IsActiveTG.EditValue = dt.Rows(0)("IsActive")
            BankID.Enabled = False
        Catch ex As Exception
            ErrorMessage(Me, "خطأ غير متوقع: " & ex.Message, "خطأ")
        End Try
    End Sub

    Private Sub BankID_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles BankID.ButtonClick
        If e.Button.Index = 1 Then
            ExBanks.ShowDialog()
        End If
    End Sub
End Class
