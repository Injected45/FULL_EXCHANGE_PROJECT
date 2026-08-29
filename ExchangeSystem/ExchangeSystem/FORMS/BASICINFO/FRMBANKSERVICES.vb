Imports System.Data.SqlClient
Imports DevExpress.Utils.Drawing.Helpers.NativeMethods
Imports DevExpress.XtraEditors
Imports TableDependency.SqlClient.Base

Public Class FRMBANKSERVICES
    Public IsUpdate As Boolean
    Sub NEWRECORD()
        IsUpdate = False
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("BankServicesTb", "ID") + 1
        ServiceName.Text = String.Empty
        LoadBBranch()
        BBranchID.EditValue = -1
        IsActiveTG.IsOn = True
        LOADBSERVICE()
        ValRate.EditValue = 0.000
        DifferentialVal.EditValue = 0.000
        TransVal.EditValue = 0.000
    End Sub
    Sub LoadBBranch()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("BBranchTb_SelectByAccountType")
        If DT.Rows.Count > 0 Then
            BBranchID.Properties.DataSource = DT
            BBranchID.Properties.ValueMember = "ID"
            BBranchID.Properties.DisplayMember = "BranchName"
            GVLKP.Columns("AccountNo").Visible = False
            NEWDVGFROMAT(GVLKP)
        End If
        DT.Dispose()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(79, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always



        End If


    End Sub
    Sub LOADBSERVICE()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("BankServicesTb_LOADTOLISTBOX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.ValueMember = "ID"
            LSBOX.DisplayMember = "ServiceName"
        End If
        DT.Dispose()
    End Sub
    Sub BSERVICE_IN_UP()
        'Try
        Dim PRM(10) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text.Trim}
            PRM(1) = New SqlParameter("@ServiceName", SqlDbType.NVarChar, -1) With {.Value = ServiceName.Text.Trim}
            PRM(2) = New SqlParameter("@BBranchID", SqlDbType.Int) With {.Value = BBranchID.EditValue}
            PRM(3) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(4) = New SqlParameter("@InsertUser", SqlDbType.Int) With {.Value = UserID}
            PRM(5) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(6) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
            PRM(8) = New SqlParameter("@ValRate", SqlDbType.Decimal) With {.Value = ValRate.EditValue}
            PRM(9) = New SqlParameter("@DifferentialVal", SqlDbType.Decimal) With {.Value = DifferentialVal.EditValue}
            PRM(10) = New SqlParameter("@TransVal", SqlDbType.Decimal) With {.Value = TransVal.EditValue}
            RUN_EXUTE_PRO("BankServicesTb_Insert", PRM)
            If PRM(5).Value = 0 Then
                ErrorMessage(Me, "رسالة خطأ", PRM(6).Value.ToString)
                Exit Sub
            Else
                If IsUpdate = False Then
                    FrmSavedSuccessfully.Show()
                    NEWRECORD()
                ElseIf IsUpdate = True Then
                    FrmEditMessage.Show()
                    NEWRECORD()
                End If
            End If
            LOADBSERVICE()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As System.EventArgs) Handles BtnSave.Click
        If ServiceName.Text = String.Empty Then
            ServiceName.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If BBranchID.EditValue = -1 Or BBranchID.Text = String.Empty Then
            BBranchID.ErrorText = "هذا الحقل مطلوب"
        End If
        BSERVICE_IN_UP()
    End Sub

    Private Sub BtnNew_Click(sender As Object, e As System.EventArgs) Handles BtnNew.Click
        NEWRECORD()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As System.EventArgs) Handles BtnEdit.Click
        If ServiceName.Text = String.Empty Then
            ServiceName.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If BBranchID.EditValue = -1 Or BBranchID.Text = String.Empty Then
            BBranchID.ErrorText = "هذا الحقل مطلوب"
        End If
        BSERVICE_IN_UP()
    End Sub

    Private Sub LSBOX_Click(sender As Object, e As System.EventArgs) Handles LSBOX.Click
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = LSBOX.SelectedValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BankServicesTb_SelectByID", PR)
        If DT.Rows.Count > 0 Then
            IsUpdate = True
            BtnEdit.Enabled = True
            BtnSave.Enabled = False
            ServiceName.Text = DT.Rows(0)("ServiceName").ToString
            BBranchID.EditValue = DT.Rows(0)("BBranchID")
            IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            CodeID.Text = DT.Rows(0)("ID")
            ValRate.EditValue = DT.Rows(0)("ValRate")
            DifferentialVal.EditValue = DT.Rows(0)("DifferentialVal")
            TransVal.EditValue = DT.Rows(0)("TransVal")
        End If
    End Sub

    Private Sub FRMBANKSERVICES_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
End Class