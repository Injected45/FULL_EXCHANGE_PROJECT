Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmADDAsset
    Sub NEWRECORD()
        AseetType.Text = ""
        LOADBRANCH()
        BranchID.EditValue = -1
        AseetType.EditValue = -1
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        AseetName.Text = ""
        AseetType.Properties.DataSource = Nothing
    End Sub
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

    Private Sub FrmADDAsset_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
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
    Public Overrides Sub SetData()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع أولا"
            Return
        End If
        If AseetType.EditValue = -1 Then
            AseetType.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If AseetName.Text = "" Then
            AseetName.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        Dim dt As New DataTable
        dt.Clear()

        Dim prm(3) As SqlParameter
        prm(0) = New SqlParameter("@ExName", SqlDbType.NVarChar, -1) With {.Value = AseetName.Text.Trim}
        prm(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        prm(2) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = AseetType.EditValue}
        prm(3) = New SqlParameter("@TypeEx", SqlDbType.TinyInt) With {.Value = 1}
        RUN_EXUTE_PRO("ExpensesTb_Insert1", prm)
        NEWRECORD()
        FrmSavedSuccessfully.Show()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        AseetType.Properties.DataSource = Nothing
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 1}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("FatherExpensesTb_LOADTOLKPBasedOnExType", PR)
        If DTT.Rows.Count > 0 Then
            AseetType.Properties.DataSource = DTT
            AseetType.Properties.ValueMember = "AccCode"
            AseetType.Properties.DisplayMember = "AccName"
            AseetType.Properties.ShowHeader = False
        End If
    End Sub
End Class