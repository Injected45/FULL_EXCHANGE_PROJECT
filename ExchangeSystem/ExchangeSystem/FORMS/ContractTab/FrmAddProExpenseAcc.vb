Imports System.ComponentModel
Imports System.Data.SqlClient
Public Class FrmAddProExpenseAcc
    Public CLSA As New CLSAccount
    Dim CLSEX As New CLSEXPENSES
    Public AcID, IDCode, AccCode, AccEm As ULong
    Public Property EXID As Integer
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
        LOADEXPENSE()
        TypeEx.SelectedIndex = 0
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
    Sub LOADEXPENSE()
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO_ONLY("CONDB_ExpressExpense_Select")
        If DTT.Rows.Count > 0 Then
            ExName.Properties.DataSource = DTT
            ExName.Properties.ValueMember = "ID"
            ExName.Properties.DisplayMember = "ExName"
            ExName.Properties.ShowHeader = False
        End If
    End Sub

    Private Sub ExName_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ExName.ButtonClick
        If e.Button.Index = 1 Then
            FrmAddProExpense.ShowDialog()
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
        Dim prm(6) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = 0}
        prm(1) = New SqlParameter("@ExName", SqlDbType.NVarChar, -1) With {.Value = ExName.Text.Trim}
        prm(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        prm(3) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(4) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(5) = New SqlParameter("@BranchiD", SqlDbType.Int) With {.Value = BranchID.EditValue}
        prm(6) = New SqlParameter("@ExpenseType", SqlDbType.Int) With {.Value = TypeEx.SelectedIndex}

        RUN_EXUTE_PRO("CONDB_ExpensesTb_Insert", prm)
        NEWRECORD()
        FrmSavedSuccessfully.Show()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub ExName_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ExName.QueryPopUp
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO_ONLY("CONDB_ExpressExpense_Select")
        If DTT.Rows.Count > 0 Then
            ExName.Properties.PopulateColumns()
            ExName.Properties.Columns("ID").Visible = False
        End If
    End Sub
End Class