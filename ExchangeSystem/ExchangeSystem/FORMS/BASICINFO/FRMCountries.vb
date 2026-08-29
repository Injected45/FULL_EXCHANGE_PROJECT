Imports System.Data.SqlClient

Public Class FRMCountries
    Public IsUpdate, IsMain As Boolean
    Public CCode As String
    Sub NEWRECORD()
        CheckMainCountries()
        IsUpdate = False
        CName.Text = ""
        Code.Text = GET_LAST_RECORD("CountiresTb", "ID") + 1
        LB.SelectedIndex = -1
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        LOADCOUNTRIES()
        LOADCurrencies()
        If IsMain = True Then
            DefaultCountry.Enabled = False
        Else
            DefaultCountry.Enabled = True
        End If
    End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(FRmIDsql, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If


    End Sub
    Sub CheckMainCountries()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_QUERY_ONLY("CountriesTb_CheckMainExist")
        If dt.Rows.Count > 0 Then
            IsMain = dt.Rows(0)("IsMain")

        End If
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If CName.Text.Trim = "" Then
                CName.ErrorText = "هذا الحقل مطلوب"
                CName.Select()
                Exit Sub
            End If
            If IsMain = True Then
                If DefaultCountry.Checked = True Then
                    DefaultCountry.ErrorText = "الدولة الافتراضية موجودة مسبقاً"
                End If
            End If
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@CName", SqlDbType.NVarChar, (150))
            PR(0).Value = CName.Text
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CountriesTb_SearchByName", PR)
            If DT.Rows.Count > 0 Then
                CName.ErrorText = "هذا الاسم موجود مسبقاً"
                CName.Select()
                Exit Sub
            End If
                ' Route through RUN_EXUTE_PRO instead of opening SQLCON directly. Under MySQL mode SQLCON
            ' (a System.Data.SqlClient.SqlConnection) is never given a connection string, so SQLCON.Open()
            ' threw "The ConnectionString property has not been initialized." — and a raw SqlCommand cannot
            ' talk to MySQL anyway. RUN_EXUTE_PRO dispatches to the MySQL routing layer (or SQL Server when
            ' USE_MYSQL is False), exactly like the CountriesTb_SearchByName call above.
            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@CCode", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
            PRM(1) = New SqlParameter("@CName", SqlDbType.NVarChar, -1) With {.Value = CName.Text.Trim}
            PRM(2) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code.Text}
            PRM(3) = New SqlParameter("@IsMain", SqlDbType.Bit) With {.Value = DefaultCountry.Checked}
            PRM(4) = New SqlParameter("@DefualtCurrency", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
            RUN_EXUTE_PRO("CountriesTb_Insert", PRM)
        End If
        FrmCoBranch.LOADCOUNTRIES()
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If CName.Text.Trim = "" Then
                CName.ErrorText = "هذا الحقل مطلوب"
                CName.Select()
                Exit Sub
            End If
            ' Same MySQL-routing fix as SetData above (was: raw SqlCommand on SQLCON).
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@CCode", SqlDbType.NVarChar, -1) With {.Value = CCode}
            PRM(1) = New SqlParameter("@CName", SqlDbType.NVarChar, -1) With {.Value = CName.Text.Trim}
            RUN_EXUTE_PRO("CountriesTb_Insert", PRM)
        End If
        FrmCoBranch.LOADCOUNTRIES()
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub

    Sub LOADCOUNTRIES()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToListBox")
        LB.DataSource = DT
        LB.ValueMember = "ID"
        LB.DisplayMember = "CName"
    End Sub
    Sub LOADCurrencies()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CurrencyMainTb_LOADTOLKP")
        CurrencyID.Properties.DataSource = DT
        CurrencyID.Properties.ValueMember = "ID"
        CurrencyID.Properties.DisplayMember = "CuName"
    End Sub
    Private Sub FRMCountries_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub LB_Click(sender As Object, e As EventArgs) Handles LB.Click

        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CName", SqlDbType.NVarChar, (150))
        PR(0).Value = CName.Text
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CountriesTb_SearchByName", PR)
        If DT.Rows.Count > 0 Then
            IsUpdate = True
            BtnSave.Enabled = False
            BtnEdit.Enabled = True
            CName.Text = DT.Rows(0)("CName").ToString
            CCode = DT.Rows(0)("CCode").ToString
            CName.Select()
        End If
    End Sub
End Class