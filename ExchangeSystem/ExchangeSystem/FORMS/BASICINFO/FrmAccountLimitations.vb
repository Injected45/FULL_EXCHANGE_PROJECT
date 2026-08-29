Imports System.Data.SqlClient

Public Class FrmAccountLimitations
    Public IsUpdate As Integer
    Sub NewRecord()
        AccountType.EditValue = -1
        AccID.EditValue = -1
        LimitationVal.EditValue = 0.000
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Caption = "إلغاء السقف"
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnSave.Enabled = True
        IsUpdate = 0
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit2", "BName", "DBRID", Nothing)
        BranchID.EditValue = BID
        PreviewVal.EditValue = 0.000
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        AccountType.EditValue = -1
        AccountType.Properties.DataSource = Nothing
        If BranchID.EditValue > -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            LoadToControlar(AccountType, "Accounts_ForLimitation", "AccName", "AccCode", PR)
        End If
    End Sub
    Private Sub AccountType_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType.EditValueChanged
        AccID.EditValue = -1
        AccID.Properties.DataSource = Nothing
        If BranchID.EditValue > -1 And AccountType.EditValue > -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            LoadToControlar(AccID, "Accounts_ForLimitationBASEDONAccParent", "AccName", "AccID", PR)
        End If
    End Sub
    Private Sub AccID_EditValueChanged(sender As Object, e As EventArgs) Handles AccID.EditValueChanged
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        If AccID.EditValue = -1 Then
            Exit Sub
        End If
        PreviewVal.EditValue = GetLKPColumnVal(AccID, "LimitedVal")
        Dim value As Object = GetLKPColumnVal(AccID, "IsLimited")
        If value = 1 Then
            BtnEdit.Enabled = True
            BtnDelete.Enabled = True
            BtnSave.Enabled = False
        Else
            BtnEdit.Enabled = False
            BtnDelete.Enabled = False
            BtnSave.Enabled = True
        End If
    End Sub
    Public Overrides Sub SetData()
        If AccountType.EditValue = -1 Then
            AccountType.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If AccID.EditValue = -1 Then
            AccID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If LimitationVal.EditValue < 0.000 Then
            LimitationVal.ErrorText = "القيمة لا يجب أن تكون أصغر من صفر"
            Exit Sub
        End If
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID.EditValue}
        PRM(1) = New SqlParameter("@LimitationVal", SqlDbType.Decimal) With {.Value = LimitationVal.EditValue}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = 0}
        RUN_EXUTE_PRO("Accounts_LimitedUpdate", PRM)
        FrmSavedSuccessfully.ShowDialog()
        NewRecord()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Private Sub FrmAccountLimitations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If AccountType.EditValue = -1 Then
            AccountType.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If AccID.EditValue = -1 Then
            AccID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If LimitationVal.EditValue < 0.000 Then
            LimitationVal.ErrorText = "القيمة لا يجب أن تكون أصغر من صفر"
            Exit Sub
        End If
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID.EditValue}
        PRM(1) = New SqlParameter("@LimitationVal", SqlDbType.Decimal) With {.Value = LimitationVal.EditValue}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = 2}
        RUN_EXUTE_PRO("Accounts_LimitedUpdate", PRM)
        MyBase.UPDATERECORD()
        NewRecord()
    End Sub
    Public Overrides Sub Remove()
        If AccountType.EditValue = -1 Then
            AccountType.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If AccID.EditValue = -1 Then
            AccID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID.EditValue}
        PRM(1) = New SqlParameter("@LimitationVal", SqlDbType.Decimal) With {.Value = LimitationVal.EditValue}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = 1}
        RUN_EXUTE_PRO("Accounts_LimitedUpdate", PRM)
        MyBase.UPDATERECORD()
        NewRecord()
        MyBase.Remove()
    End Sub
    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
End Class