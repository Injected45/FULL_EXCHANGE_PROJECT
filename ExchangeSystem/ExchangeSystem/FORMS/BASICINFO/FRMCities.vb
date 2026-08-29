Imports System.Data.SqlClient
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities.IO


Public Class FRMCities
    Public IsUpdate As Boolean
    Public Code As String
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(FRmIDsql, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

        End If


    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        CityName.Text = ""
        Code = GET_LAST_RECORD("CitiesTb", "ID") + 1
        LB.DataSource = Nothing
        CountriesID.EditValue = -1
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        LOADCOUNTRIES()
        BtnNew.Enabled = True
        Me.CountriesID.Enabled = True
    End Sub


    Sub LOADCOUNTRIES()
        Try


            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_TXT("CountriesTb_LoadToLKP")
            If DT.Rows.Count > 0 Then
                CountriesID.Properties.DataSource = DT
                CountriesID.Properties.ValueMember = "ID"
                CountriesID.Properties.DisplayMember = "CName"
                CountriesID.Properties.PopulateColumns()
                'CountriesID.Properties.Columns("ID").Visible = False
                'CountriesID.Properties.Columns("CCode").Visible = False
            End If


        Catch ex As Exception

        End Try
    End Sub
    Sub LOADCITIES()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        PR(0).Value = CountriesID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CitiesTb_LoadToLKPBasedonCountry", PR)
        If DT.Rows.Count > 0 Then
            LB.DataSource = DT
            LB.ValueMember = "CTID"
            LB.DisplayMember = "CityName"
        End If
    End Sub

    Private Sub LB_Click(sender As Object, e As EventArgs) Handles LB.Click
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        PRM(0).Value = LB.SelectedValue
        Dim DT1 As New DataTable
        DT1.Clear()
        DT1 = RUN_QUARY_PRO("CountriesTb_BindLKPForCities", PRM)
        If DT1.Rows.Count > 0 Then
            CountriesID.EditValue = DT1.Rows(0)("CountryID")
        End If
        '==============================================
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        PR(0).Value = CountriesID.EditValue
        PR(1) = New SqlParameter("@CityName", SqlDbType.NVarChar, (150))
        PR(1).Value = LB.Text
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CitiesTb_SearchByName", PR)
        If DT.Rows.Count > 0 Then
            CityName.Text = DT.Rows(0)("CityName").ToString
            Code = DT.Rows(0)("Code").ToString
            CityName.Select()
        End If
        IsUpdate = True
        BtnSave.Enabled = False
        BtnEdit.Enabled = True
    End Sub
    Private Sub CountriesID_TextChanged(sender As Object, e As EventArgs) Handles CountriesID.TextChanged
        If CountriesID.EditValue <> -1 Or CountriesID.Text <> String.Empty Then
            LOADCITIES()
        End If
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If CountriesID.EditValue = -1 Then
                CountriesID.ErrorText = "يجب اختيار الدولة أولاً"
                CountriesID.Select()
                Exit Sub
            End If
            If CityName.Text = String.Empty Then
                CityName.ErrorText = "هذا الحقل مطلوب"
                CityName.Select()
                Exit Sub
            End If
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@CountryID", SqlDbType.Int)
            PR(0).Value = CountriesID.EditValue
            PR(1) = New SqlParameter("@CityName", SqlDbType.NVarChar, (150))
            PR(1).Value = CityName.Text
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CitiesTb_SearchByName", PR)
            If DT.Rows.Count > 0 Then
                CityName.ErrorText = "هذا الاسم موجود مسبقاً"
                CityName.Select()
                Exit Sub
            End If
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@CCode", SqlDbType.NVarChar, 50) With {.Value = Code}
            PRM(1) = New SqlParameter("@CName", SqlDbType.NVarChar, -1) With {.Value = CityName.Text.Trim}
            PRM(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountriesID.EditValue}
            RUN_EXUTE_PRO("CitiesTb_Insert", PRM)
        End If
        If Application.OpenForms().OfType(Of FrmCoBranch).Any Then
            FrmCoBranch.LOADCITIES()
        End If
        If Application.OpenForms().OfType(Of FRMEXTERNALTRANS).Any Then
            FRMEXTERNALTRANS.LoadCities()
        End If
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If CountriesID.EditValue = -1 Then
                CountriesID.ErrorText = "يجب اختيار الدولة أولاً"
                CountriesID.Select()
                Exit Sub
            End If
            If CityName.Text = String.Empty Then
                CityName.ErrorText = "هذا الحقل مطلوب"
                CityName.Select()
                Exit Sub
            End If
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@CCode", SqlDbType.NVarChar, 50) With {.Value = Code}
            PRM(1) = New SqlParameter("@CName", SqlDbType.NVarChar, -1) With {.Value = CityName.Text.Trim}
            PRM(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountriesID.EditValue}
            RUN_EXUTE_PRO("CitiesTb_Update", PRM)
        End If
        If Application.OpenForms().OfType(Of FrmCoBranch).Any Then
            FrmCoBranch.LOADCITIES()
        End If
        If Application.OpenForms().OfType(Of FRMEXTERNALTRANS).Any Then
            FRMEXTERNALTRANS.LoadCities()
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub

    Private Sub FRMCities_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub

End Class