Imports System.Data.SqlClient

Public Class FrmAddLeaveType
    Public IsUpdate As Boolean
    Dim cu As New BANKCLSS
    Public AccID As ULong

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(79, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Public Sub CHECKBUTTONS()
        CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        Dim DT As New DataTable
        DT.Clear()
        DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        If DT.Rows.Count > 0 Then
            If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
            If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True
        End If
    End Sub
    Sub NEWRECORD()
        CHECKBUTTONS()
        IsUpdate = False
        BANKNAME.Text = ""
        CodeID.Enabled = False
        BANKNAME.Select()
        IsActiveTG.IsOn = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("dbo.AddLeaveTypeTb", "ID") + 1
        LOADDATA()
        LSBOX.SelectedIndex = -1
        'FrmLeave.InitializeLeaveTypes()
        LoadToControlar(FrmLeave.LeaveTypeID, "AddLeaveTypeTb_LOADTOLSBOX", "LeaveName", "ID", Nothing)
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("AddLeaveTypeTb_LOADTOLSBOX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "LeaveName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
        'CHECKBUTTONS()
    End Sub
    Sub InsertOrUpdate()
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text.Trim}
        PRM(1) = New SqlParameter("@LeaveName", SqlDbType.NVarChar, -1) With {.Value = BANKNAME.Text.Trim}
        PRM(2) = New SqlParameter("@IsActive ", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
        PRM(3) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(4) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(5) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("AddLeaveTypeTb_Insert", PRM)
        If PRM(4).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(5).Value.ToString)
            Exit Sub
        End If
        If IsUpdate = False Then
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        ElseIf IsUpdate = True Then
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            InsertOrUpdate()
            'FrmLeave.InitializeLeaveTypes()
            LoadToControlar(FrmLeave.LeaveTypeID, "AddLeaveTypeTb_LOADTOLSBOX", "LeaveName", "ID", Nothing)
        End If
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            InsertOrUpdate()
            'FrmLeave.InitializeLeaveTypes()
            LoadToControlar(FrmLeave.LeaveTypeID, "AddLeaveTypeTb_LOADTOLSBOX", "LeaveName", "ID", Nothing)
        End If
    End Sub
    Private Sub FRMBANK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
        lodePreportes()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        For I As Integer = 0 To LSBOX.Items.Count - 1
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
            PRM(0).Value = Convert.ToInt32(CodeID.Text)
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AddLeaveTypeTb_LOADTOUPDATE", PRM)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                BANKNAME.Text = DT.Rows(0)("BankName").ToString
                CodeID.Text = DT.Rows(0)("ID")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            End If
        Next
    End Sub
End Class
