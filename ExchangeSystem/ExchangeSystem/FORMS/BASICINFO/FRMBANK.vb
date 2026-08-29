Imports System.Data.SqlClient

Public Class FRMBANK
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
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(79, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            


        End If


    End Sub
    Public Sub CHECKBUTTONS()
        CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        Dim DT As New DataTable
        DT.Clear()
        DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        If DT.Rows.Count > 0 Then
            If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
            If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True
        End If
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
        CodeID.Text = GETMAXID("BanksTb", "ID") + 1

        LSBOX.SelectedIndex = -1
        FRMBBRANCH.LOADBANK()
        CountryID.Enabled = True
    End Sub
    Public Sub LOADDATA(CountryIDto As Integer)
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryIDto}

        DT = RUN_QUARY_PRO("BanksTb_SelectAll_CountryID", prm)
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
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            cu.EMPCSFT_INSERT(CodeID.Text, BANKNAME.Text.Trim, IsUpdate, CountryID.EditValue)
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        End If
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            cu.EMPCSFT_INSERT(CodeID.Text, BANKNAME.Text.Trim, IsUpdate, CountryID.EditValue)
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub
    Private Sub FRMBANK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        For I As Integer = 0 To LSBOX.Items.Count - 1
            Dim DT As New DataTable
            DT.Clear()
            DT = cu.EMPCSFT_Select(LSBOX.SelectedValue)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                BANKNAME.Text = DT.Rows(0)("BankName").ToString
                CodeID.Text = DT.Rows(0)("ID")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
                CountryID.EditValue = DT.Rows(0)("CountryID")
                AccID = DT.Rows(0)("AccID")
            End If
        Next
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        If CountryID.EditValue > -1 Then

            LOADDATA(CountryID.EditValue)
        End If
    End Sub
End Class
Public Class BANKCLSS
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BanksTb_SelectByID", PRM)
        Return DT
    End Function
    Public Function EMPCSFT_SelectAll() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("EmployeeClassificationTb_SelectAll")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(ID As Integer, BankName As String, IsUpdate As Boolean, CountryID As Integer)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@BankName", SqlDbType.NVarChar, -1) With {.Value = BankName}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
        RUN_EXUTE_PRO("BanksTb_Insert", PRM)
    End Sub
    Public Sub EMPCSFT_DELETE(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("EmployeeClassificationTb_Delete", PRM)
    End Sub
End Class