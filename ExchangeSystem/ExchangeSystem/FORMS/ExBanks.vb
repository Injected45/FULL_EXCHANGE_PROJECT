Imports System.Data.SqlClient

Public Class ExBanks
    Public IsUpdate As Boolean
    Dim cu As New BANKCLSS
    Public AccID As ULong
    Public Sub LoadCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            NEWDVGFROMAT(CountryGV)
        End If
    End Sub
    Sub NEWRECORD()
        LoadCountry()
        CountryID.EditValue = -1
        IsUpdate = False
        BANKNAME.Text = ""
        CodeID.Enabled = False
        BANKNAME.Select()
        IsActiveTG.IsOn = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("ExBanksTb", "ID") + 1

        LSBOX.SelectedIndex = -1
        FRMEXBBRANCH.LOADBANK()
        CountryID.Enabled = True
    End Sub
    Public Sub LOADDATA(CountryIDto As Integer)
        LSBOX.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 5}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExBanksTb_CRUD", PR)
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "BankName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text}
        PRM(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PRM(2) = New SqlParameter("@BankName", SqlDbType.NVarChar, -1) With {.Value = BANKNAME.Text.Trim}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}
        PRM(4) = New SqlParameter("@Action", SqlDbType.Bit) With {.Value = 0}
        RUN_EXUTE_PRO("ExBanksTb_CRUD", PRM)
        If IsUpdate = 0 Then
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        ElseIf IsUpdate = 1 Then
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text}
        PRM(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PRM(2) = New SqlParameter("@BankName", SqlDbType.NVarChar, -1) With {.Value = BANKNAME.Text.Trim}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}
        PRM(4) = New SqlParameter("@Action", SqlDbType.Bit) With {.Value = 1}
        RUN_EXUTE_PRO("ExBanksTb_CRUD", PRM)
        If IsUpdate = 0 Then
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        ElseIf IsUpdate = 1 Then
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub

    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 3}
            prm(1) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = SafeToInt(LSBOX.SelectedValue)}

            Dim dt As DataTable = RUN_QUARY_PRO_alter("ExBanksTb_CRUD", prm)
            If dt.Rows.Count <= 0 Then Exit Sub
            BtnSave.Enabled = False
            BtnEdit.Enabled = True
            BANKNAME.Text = dt.Rows(0)("BankName").ToString
            CodeID.Text = dt.Rows(0)("ID")
            IsActiveTG.EditValue = dt.Rows(0)("IsActive")
            CountryID.EditValue = dt.Rows(0)("CountryID")
        Catch ex As Exception
            ErrorMessage(Me, "خطأ غير متوقع: " & ex.Message, "خطأ")
        End Try
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        If CountryID.EditValue > -1 Then

            LOADDATA(CountryID.EditValue)
        End If
    End Sub
End Class
