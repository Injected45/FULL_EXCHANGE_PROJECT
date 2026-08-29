Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class FRMADDPARTENR
    Public IsUpdate As Boolean
    Dim acct As New ACCCREDIT
    Public CustID, msgST As Integer
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(FRmIDsql, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        IsActiveTG.EditValue = True
        InsertDate.EditValue = Date.Now
        AccName.Text = String.Empty
        Phone1.Text = String.Empty
        Phone2.Text = String.Empty
        Phone3.Text = String.Empty
        Notes.Text = String.Empty
        IDNo.Text = String.Empty
        PTRATE.EditValue = 0
        CustID = 0
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LOADBRNCHDIERCT(BranchID)
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        TypeID.SelectedIndex = 0
        If UserType = 1 Then
            BranchID.Enabled = True
        Else
            BranchID.Enabled = False
        End If
    End Sub

    Sub Show_CreditAccount(x)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.BigInt) With {.Value = x}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AddPartnerTb_SelectALL", PR)
        If DT.Rows.Count > 0 Then
            CustID = DT.Rows(0)("ID")
            BranchID.EditValue = DT.Rows(0)("BranchID")
            Code.Text = DT.Rows(0)("Code").ToString
            IsActiveTG.IsOn = DT.Rows(0)("isactive")
            AccName.EditValue = DT.Rows(0)("AccName")
            Phone1.EditValue = DT.Rows(0)("Phone1")
            Phone2.EditValue = DT.Rows(0)("Phone2")
            Phone3.EditValue = DT.Rows(0)("Phone3")
            PTRATE.EditValue = DT.Rows(0)("PTRATE")
            TypeID.SelectedIndex = DT.Rows(0)("TypeID")
            BranchID.Enabled = False
        End If
        'End If
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub SetData()
        If AccName.Text = String.Empty Then
            AccName.ErrorText = "الاسم لا يجب أن يكون فارغا"
            Exit Sub
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If
        If TypeID.SelectedIndex < 0 Then
            TypeID.ErrorText = "يجب اختيار نوع الحساب"
            Exit Sub
        End If
        'If PTRATE.EditValue = 0 Or PTRATE.EditValue < 0 Then
        '    PTRATE.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
        '    Exit Sub
        'End If
        CUSTOMER_INSERT()
        'NEWRECORD()
        MyBase.SetData()
    End Sub

    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then


            If AccName.Text = String.Empty Then
                AccName.ErrorText = "الاسم لا يجب أن يكون فارغا"
                Exit Sub
            End If
            Dim DT As New DataTable
            DT.Clear()
            DT = acct.CHECK_ACCCREDIT_NAME(AccName.Text.Trim)
            If DT.Rows.Count > 0 Then
                AccName.ErrorText = "اسم الحساب موجود مسبقا"
            End If
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يجب اختيار الفرع"
                Exit Sub
            End If
            CUSTOMER_INSERT()
            NEWRECORD()
        End If
        MyBase.UPDATERECORD()
    End Sub

    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub FRMADDPARTENR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            If IsUpdate = False Then
                Code.Text = BranchID.EditValue & "0" & "34" & "0" & GETMAXID("AddPartnerTb", "ID") + 1
            ElseIf IsUpdate = True Then
                Code.Text = ""
                Code.Text = BranchID.EditValue & "0" & "34" & "0" & CustID
            End If
        End If
    End Sub


    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            If IsUpdate = False Then
                Code.Text = BranchID.EditValue & "0" & "34" & "0" & GETMAXID("AddPartnerTb", "ID") + 1
            ElseIf IsUpdate = True Then
                Code.Text = ""
                Code.Text = BranchID.EditValue & "0" & "34" & "0" & CustID
            End If
        End If
    End Sub

    Private Sub BranchID_ListChanged(sender As Object, e As ListChangedEventArgs) Handles BranchID.ListChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            If IsUpdate = False Then
                Code.Text = BranchID.EditValue & "0" & "34" & "0" & GETMAXID("AddPartnerTb", "ID") + 1
            ElseIf IsUpdate = True Then
                Code.Text = ""
                Code.Text = BranchID.EditValue & "0" & "34" & "0" & CustID
            End If
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        NEWRECORD()
        FRMViewADDPARTNER.ShowDialog()
    End Sub


    Public Sub CUSTOMER_INSERT()
        Dim PRM(15) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CustID}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code.Text.Trim}
        PRM(2) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate.EditValue}
        PRM(3) = New SqlParameter("@AccName", SqlDbType.NVarChar, 50) With {.Value = AccName.Text.Trim}
        PRM(4) = New SqlParameter("@Phone1", SqlDbType.NVarChar, 50) With {.Value = Phone1.Text.Trim}
        PRM(5) = New SqlParameter("@Phone2", SqlDbType.NVarChar, 50) With {.Value = Phone2.Text.Trim}
        PRM(6) = New SqlParameter("@IDNoe ", SqlDbType.NVarChar, 50) With {.Value = IDNo.Text.Trim}
        PRM(7) = New SqlParameter("@PTRATE ", SqlDbType.Decimal) With {.Value = PTRATE.EditValue}
        PRM(8) = New SqlParameter("@IsActive ", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
        PRM(9) = New SqlParameter("@UserID ", SqlDbType.Int) With {.Value = UserID}
        PRM(10) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(11) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(12) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(13) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(14) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}
        PRM(15) = New SqlParameter("@Phone3", SqlDbType.NVarChar, 250) With {.Value = Phone3.Text.Trim}
        RUN_EXUTE_PRO("AddPartnerTb_Insert", PRM)
        Me.msgST = PRM(12).Value
        If PRM(12).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(13).Value.ToString)
            Exit Sub
        Else
            Me.BtnNew.PerformClick()
            FrmSavedSuccessfully.Show()
        End If
    End Sub
End Class