Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmExpenses
    Public CLSA As New CLSAccount
    Dim CLSEX As New CLSEXPENSES
    Public AcID, IDCode, AccCode, AccEm As ULong
    Public Property EMBID As Integer
    Public StID, AccLine, AccCat As Integer
    Public Property X As String
    Public IsUpdate, UpdateBySalary As Boolean


    Public Overrides Sub CHECKBUTTONS()
        lodePreportes()
        MyBase.CHECKBUTTONS()
    End Sub



    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(3, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            'If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            'If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NEWRECORD()
        ExName.Text = ""
        LOADBRANCH()
        BranchID.EditValue = -1
        ExName.EditValue = -1
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        IsUpdate = False
        ExName.Properties.DataSource = Nothing
    End Sub

    Private Sub FrmExpenses_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResizeControls.SubResize(Me, 30, 25)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.CenterToScreen()
        Me.WindowState = FormWindowState.Normal
        CHECKBUTTONS()
        NEWRECORD()
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

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.Columns("DBRID").Visible = False
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        ExName.Properties.DataSource = Nothing
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 0}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("FatherExpensesTb_LOADTOLKPBasedOnExType", PR)
        If DTT.Rows.Count > 0 Then
            ExName.Properties.DataSource = DTT
            ExName.Properties.ValueMember = "AccCode"
            ExName.Properties.DisplayMember = "AccName"
            ExName.Properties.ShowHeader = False
        End If
    End Sub

    Private Sub ExName_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ExName.ButtonClick
        If e.Button.Index = 1 Then
            ADDFatherExpeens.ShowDialog()
        End If
    End Sub

    Public Overrides Sub SetData()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع أولا"
            Return
        End If
        If ExName.EditValue = -1 Then
            ExName.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        Dim dt As New DataTable
        dt.Clear()

        Dim prm(3) As SqlParameter
        prm(0) = New SqlParameter("@ExName", SqlDbType.NVarChar, -1) With {.Value = ExName.Text.Trim}
        prm(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(2) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = ExName.EditValue}
            prm(3) = New SqlParameter("@TypeEx", SqlDbType.TinyInt) With {.Value = 0}
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
        ExName.Properties.DataSource = Nothing
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 0}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("FatherExpensesTb_LOADTOLKPBasedOnExType", PR)
        If DTT.Rows.Count > 0 Then
            ExName.Properties.DataSource = DTT
            ExName.Properties.ValueMember = "AccCode"
            ExName.Properties.DisplayMember = "AccName"
            ExName.Properties.ShowHeader = False
        End If
    End Sub
End Class