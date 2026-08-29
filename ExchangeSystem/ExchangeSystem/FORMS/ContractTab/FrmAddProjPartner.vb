Imports System.Data.SqlClient
Imports System.ComponentModel

Public Class FrmAddProPartner
    Public IsUpdate As Boolean, msgST As Integer
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub

    Sub newRecord()
        Code.Text = GETMAXID("ContractDB.dbo.AddPartnerTb", "ID") + 1
        ProName.Text = ""
        ProName.Focus()
        LOADBRANCH()
        IsPartner.SelectedIndex = 0
        BranchID.EditValue = -1
        IsActive.IsOn = True
        IsActive.IsOn = True
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub

    Private Sub FrmAddProject_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        newRecord()
    End Sub
    Public Sub CUSTOMER_INSERT()
        Dim PRM(8) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code.Text}
        PRM(1) = New SqlParameter("@PartnerName", SqlDbType.NVarChar, -1) With {.Value = ProName.Text.Trim}
        PRM(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(3) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
        PRM(4) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(5) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(6) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive.EditValue}
        PRM(8) = New SqlParameter("@IsPartner", SqlDbType.Bit) With {.Value = IsPartner.SelectedIndex}
        RUN_EXUTE_PRO("CONDB_AddPartner_Insert", PRM)
        Me.msgST = PRM(5).Value
        If PRM(5).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(6).Value.ToString)
            Exit Sub
        Else
            Me.BtnNew.PerformClick()
        End If
    End Sub
    Public Sub Pro_SelectByID(x)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = x}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_AddPartnerTb_Select", PRM)
        If dt.Rows.Count > 0 Then
            Code.Text = dt.Rows(0).Item("ID")
            ProName.Text = dt.Rows(0).Item("AssestName")
            BranchID.EditValue = dt.Rows(0).Item("BranchID")
            IsActive.IsOn = dt.Rows(0).Item("IsActive")
            IsPartner.SelectedIndex = dt.Rows(0).Item("IsPartner")
        End If
    End Sub
    Public Sub DISAPLEDCONTROLS(IsEnabled As Boolean)
        ProName.Enabled = IsEnabled
        BranchID.Enabled = IsEnabled
        IsActive.Enabled = IsEnabled
    End Sub
    Public Overrides Sub SetData()
        If ProName.Text = String.Empty Then
            ProName.ErrorText = "يرجى إخال اسم المشروع"
            Exit Sub
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى إختيار الفرع"
            Exit Sub
        End If
        CUSTOMER_INSERT()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub BNew()
        newRecord()
        MyBase.BNew()
    End Sub

    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        FrmViewPartner.ShowDialog()
    End Sub

    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.Columns("DBRID").Visible = False
        End If
    End Sub
End Class