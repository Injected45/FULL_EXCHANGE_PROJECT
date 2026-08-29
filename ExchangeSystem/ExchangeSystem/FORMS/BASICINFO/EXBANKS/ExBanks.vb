Imports System.Data.SqlClient

Public Class ExBanks
    Public IsUpdate As Integer
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
    Public Sub lodePreportes()
        'Dim dt As New DataTable
        'dt.Clear()
        'dt = SElectUEserFormButtn(79, UserID)

        'If dt.Rows.Count > 0 Then
        '    If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        '    If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always



        'End If


    End Sub
    Public Sub CHECKBUTTONS()
        'CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        'If DT.Rows.Count > 0 Then
        '    If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
        '    If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True
        'End If
    End Sub
    Sub NEWRECORD()
        CHECKBUTTONS()
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
        Dim DT As New DataTable
        DT.Clear()
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryIDto}
        prm(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 5}

        DT = RUN_QUARY_PRO("ExBanksTb_CRUD", prm)
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "BankName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
        CHECKBUTTONS()
    End Sub
    Public Sub EMPCSFT_INSERT()
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text}
        PRM(1) = New SqlParameter("@BankName", SqlDbType.NVarChar, -1) With {.Value = BANKNAME.Text.Trim}
        PRM(2) = New SqlParameter("@Action", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        RUN_EXUTE_PRO("ExBanksTb_CRUD", PRM)
    End Sub
    Public Sub EMPCSFT_DELETE(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("EmployeeClassificationTb_Delete", PRM)
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        IsUpdate = 0
        If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            EMPCSFT_INSERT()
            FrmSavedSuccessfully.Show()
            NEWRECORD()

    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        IsUpdate = 1
        If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            EMPCSFT_INSERT()
            FrmEditMessage.Show()
        NEWRECORD()
    End Sub
    Private Sub FRMBANK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
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

            CodeID.Text = SafeToInt(dt.Rows(0).Item("ID"))
            BANKNAME.Text = SafeToString(dt.Rows(0).Item("BankName"))
            CountryID.EditValue = SafeToInt(dt.Rows(0).Item("CountryID"))
            IsActiveTG.EditValue = dt.Rows(0).Item("IsActive")
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
