Imports System.Data.SqlClient

Public Class FRMCurrencySpeculation


    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(37, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NewRecord()
        CurrencyFrom.EditValue = -1
        CurrencyTo.EditValue = -1
        BPrice1.EditValue = 0.000

    End Sub

    Sub LOADCIDFROM()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "ID"
            CurrencyFrom.Properties.DisplayMember = "CuName"
            CurrencyFrom.Properties.ShowHeader = False
        Else
            CurrencyFrom.Properties.DataSource = Nothing
        End If
    End Sub

    Sub LOADCIDTO()
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة الأولى"
            Exit Sub
        End If
        CurrencyTo.Properties.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOLKPNOTEXIST", PR)
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
            CurrencyTo.Properties.ShowHeader = False
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub



    Private Sub FRMCurrencySpeculation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        LOADCIDFROM()
        NewRecord()
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Caption = "احتساب"

    End Sub

    Private Sub CurrencyFrom_TextChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.TextChanged
        LOADCIDTO()
        Purchaseprice.Text = 0.000
        BPrice2.Text = 0.000
    End Sub

    Public Sub get_BracuNetBurnnc()
        Try
            If CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "الرجاء اختيار العملة الاولى "
                Return
            End If
            If CurrencyTo.Text = String.Empty Then
                CurrencyTo.ErrorText = "الرجاء اختيار العملة الثانية "
                Exit Sub
            End If
            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            prm(1) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
            prm(2) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(3) = New SqlParameter("@BPrice11", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@Purchaseprice", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@ISbaunk", SqlDbType.Int) With {.Value = 0}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("get_CurrencySpeculation", prm)
            If BPrice1.EditValue > 0 Then
                BPrice2.Text = modFORnamber(prm(3).Value)
            Else
                BPrice2.Text = 0.000
                BPrice1.ErrorText = "الرجاء ادخال القيمة الاولى "
            End If
            Purchaseprice.Text = prm(4).Value
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ في نظـــــــــام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Overrides Sub Save()
        get_BracuNetBurnnc()
        MyBase.Save()
    End Sub

    Private Sub CurrencyTo_TextChanged(sender As Object, e As EventArgs) Handles CurrencyTo.TextChanged
        Purchaseprice.Text = 0.000
        BPrice2.Text = 0.000
    End Sub

    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub

    Private Sub FRMCurrencySpeculation_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class