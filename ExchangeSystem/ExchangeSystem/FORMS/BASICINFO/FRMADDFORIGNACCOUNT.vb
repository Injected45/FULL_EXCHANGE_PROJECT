Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMADDFORIGNACCOUNT
    Public FACCID, msgST As Integer
    Public IsUpdate As Boolean
    Sub LoadCurrency()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CurrencyMainTb_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CuName"
            CurrencyID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LoadCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            CountryID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LoadCityID()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CitiesTb_LoadToLKPBasedonCountry", PR)
        If DT.Rows.Count > 0 Then
            CItyID.Properties.DataSource = DT
            CItyID.Properties.ValueMember = "CTID"
            CItyID.Properties.DisplayMember = "CityName"
            CItyID.Properties.ShowHeader = False
        End If
    End Sub
    Sub NEWRECORD()
        LoadCountry()
        LoadCurrency()
        'CountryID.EditValue = COUNTRYNID
        CurrencyID.EditValue = -1
        CItyID.EditValue = -1
        InsertDate.EditValue = Date.Now
        Code.Text = GETMAXID("ADDFORIGNACCOUNTTB", "ID") + 1
    End Sub

    Private Sub FRMADDFORIGNACCOUNT_Load(sender As Object, e As EventArgs) Handles Me.Load
        NEWRECORD()
    End Sub
    Public Sub CUSTOMER_INSERT()
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code.Text.Trim}
        PRM(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PRM(2) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = CItyID.EditValue}
        PRM(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        PRM(4) = New SqlParameter("@IsActive ", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
        PRM(5) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate.EditValue}
        PRM(6) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(7) = New SqlParameter("@UserID ", SqlDbType.Int) With {.Value = UserID}
        PRM(8) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(9) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("ADDFORIGNACCOUNTTB_Insert", PRM)
        Me.msgST = PRM(8).Value
        If PRM(8).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(9).Value.ToString)
            Exit Sub
        Else
            Me.BtnNew.PerformClick()
        End If
    End Sub
    Public Overrides Sub SetData()
        If CountryID.EditValue = -1 Then
            CountryID.ErrorText = "يجب اختيار الدولة"
            Exit Sub
        End If
        If CItyID.EditValue = -1 Then
            CItyID.ErrorText = "يجب اختيار المدينة"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Then
            CurrencyID.ErrorText = "يجب اختيار العملة"
            Exit Sub
        End If
        CUSTOMER_INSERT()
        If msgST = 1 Then
            MyBase.SetData()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        LoadCityID()
    End Sub

    Private Sub CountryID_TextChanged(sender As Object, e As EventArgs) Handles CountryID.TextChanged
        LoadCityID()
    End Sub

    Private Sub CountryID_Popup(sender As Object, e As EventArgs) Handles CountryID.Popup

    End Sub

    Private Sub CountryID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CountryID.QueryPopUp
        CountryID.Properties.ForceInitialize()
        CountryID.Properties.PopulateColumns()
        CountryID.Properties.Columns("CouID").Visible = False
    End Sub

    Private Sub CItyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CItyID.QueryPopUp
        If CountryID.EditValue <> -1 Or CountryID.Text <> String.Empty Then
            CItyID.Properties.ForceInitialize()
            CItyID.Properties.PopulateColumns()
            CItyID.Properties.Columns("CTID").Visible = False
        Else
            CountryID.ErrorText = "يجب اختيار الدولة أولاً"
            Exit Sub
        End If
    End Sub
End Class