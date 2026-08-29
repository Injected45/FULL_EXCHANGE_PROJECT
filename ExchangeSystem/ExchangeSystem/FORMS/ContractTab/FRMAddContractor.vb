Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient

Public Class FRMAddContractor
    Dim clscust As New CLSCUSTOMER
    Public Property InsDate As Date
    Public CustID As Integer
    Public Property IsUpdate As Boolean
    Public msgST As Int16
    Public Overrides Sub CHECKBUTTONS()
        MyBase.CHECKBUTTONS()
    End Sub


    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(13, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub FRMCUSTOMER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'lodePreportes()
        NEWRECORD()
    End Sub

#Region "Save, Update"
    Public Overrides Sub SetData()
        If CUSTNAME.Text = String.Empty Then
            CUSTNAME.ErrorText = "هذ الحقل مطلوب"
            CUSTNAME.Select()
            Return
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "هذ الحقل مطلوب"
            BranchID.Select()
            Return
        End If
        Dim DT As New DataTable
        DT.Clear()
        DT = CustomersTb_CheckPHoneExist(PHONE1.Text.Trim, PHONE2.Text.Trim)
        If DT.Rows.Count > 0 Then
            ErrorMessage(Me, "رسالة خطأ", "رقم الهاتف موجود مسبقاً")
            Exit Sub
        End If
        CUSTOMER_INSERT(Date.Now, CodeID.Text, CUSTNAME.Text.Trim, PHONE1.Text.Trim, PHONE2.Text.Trim, CUSTADDRESS.Text.Trim, BranchID.EditValue, IsUpdate, 0, CanDebit.SelectedIndex)
        If msgST = 1 Then
            MyBase.SetData()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Public Function CustomersTb_CheckPHoneExist(Phone1 As String, Phone2 As String) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
        PRM(1) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_CustomersTb_CheckPHoneExist", PRM)
        Return DT
    End Function
    Public Sub CUSTOMER_INSERT(InsertDate As Date, Code As String, CustName As String, Phone1 As String, Phone2 As String, CustmAddress As String, BranchID As Integer, IsUpdate As Boolean, ID As Integer, CanDebit As Boolean)
        Dim PRM(11) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(2) = New SqlParameter("@CustName", SqlDbType.NVarChar, 100) With {.Value = CustName}
        PRM(3) = New SqlParameter("@Phone1", SqlDbType.NVarChar, 12) With {.Value = Phone1}
        PRM(4) = New SqlParameter("@Phone2", SqlDbType.NVarChar, 12) With {.Value = Phone2}
        PRM(5) = New SqlParameter("@CustmAddress", SqlDbType.NVarChar, 150) With {.Value = CustmAddress}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(8) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(9) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(10) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(11) = New SqlParameter("@CanDebit", SqlDbType.Bit) With {.Value = CanDebit}
        RUN_EXUTE_PRO("CONDB_ContractorTb_Insert", PRM)

        Me.msgST = PRM(8).Value
        If PRM(8).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(9).Value)
            Exit Sub
        Else
            Me.BtnNew.PerformClick()
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWCUSTOMERS.ShowDialog()
    End Sub

    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If CUSTNAME.Text = String.Empty Then
                CUSTNAME.ErrorText = "هذ الحقل مطلوب"
                CUSTNAME.Select()
                Return
            End If
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "هذ الحقل مطلوب"
                BranchID.Select()
                Return
            End If
            Dim DT As New DataTable
            DT.Clear()
            DT = CustomersTb_CheckPHoneExistUpdate(CustID)
            If DT.Rows.Count > 0 Then
                ErrorMessage(Me, "رسالة خطأ", "رقم الهاتف موجود مسبقاً")
                Exit Sub
            End If
            clscust.CUSTOMER_INSERT(Date.Now, CodeID.Text, CUSTNAME.Text.Trim, PHONE1.Text.Trim, PHONE2.Text.Trim, CUSTADDRESS.Text.Trim, BranchID.EditValue, IsUpdate, CustID, CanDebit.SelectedIndex,
                                    0, "", "", 0, "", "", "", 0, Date.Now, 0, 0, 0, "", 0, 0, "", "", "")
        End If
        If msgST = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub
    Public Function CustomersTb_CheckPHoneExistUpdate(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_CustomersTb_CheckPHoneExistUpdate", PRM)
        Return DT
    End Function
#End Region
    Sub NEWRECORD()
        IsUpdate = False
        LOADBRANCH()
        BranchID.EditValue = BID
        CUSTNAME.Text = String.Empty
        PHONE1.Text = String.Empty
        PHONE2.Text = String.Empty
        CUSTADDRESS.Text = String.Empty
        IsActiveTG.IsOn = True
        CodeID.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        CanDebit.SelectedIndex = 1
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Sub SHOW_CUST(x)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clscust.CustomersTb_Select(x)
            If DT.Rows.Count > 0 Then
                CodeID.Text = DT.Rows(0)("Code").ToString
                CUSTNAME.Text = DT.Rows(0)("CustName").ToString
                PHONE1.Text = DT.Rows(0)("PHONE1").ToString
                PHONE2.Text = DT.Rows(0)("PHONE2").ToString
                CUSTADDRESS.Text = DT.Rows(0)("CustmAddress").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                InsDate = DT.Rows(0)("InsertDate")
                'IsActiveTG.IsOn = DT.Rows(0)("IsActive")
                CustID = DT.Rows(0)("ID")
                CanDebit.SelectedIndex = DT.Rows(0)("CanDebit")
            End If
        End If
    End Sub

    Private Sub FRMCUSTOMER_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IsUpdate = False
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            If IsUpdate = False Then
                CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & GETMAXID("ContractDB.dbo.ContractorTb", "ID") + 1
            ElseIf IsUpdate = True Then
                CodeID.Text = ""
                CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
            End If
        End If
    End Sub


    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            If IsUpdate = False Then
                CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & GETMAXID("ContractDB.dbo.ContractorTb", "ID") + 1
            ElseIf IsUpdate = True Then
                CodeID.Text = ""
                CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
            End If
        End If
    End Sub

    Private Sub BranchID_ListChanged(sender As Object, e As ListChangedEventArgs) Handles BranchID.ListChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            If IsUpdate = False Then
                CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & GETMAXID("ContractDB.dbo.ContractorTb", "ID") + 1
            ElseIf IsUpdate = True Then
                CodeID.Text = ""
                CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
            End If
        End If
    End Sub
End Class