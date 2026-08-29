Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Tile

Public Class FrmCurrnceyStatment2026

    Sub Newrecord()
        New_Controlrs(Me)
        LoadToControlar(BranchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        LoadToControlar(CountryID, "CountriesTb_LoadToLKP", "CName", "ID", Nothing)
        CountryID.EditValue = COUNTRYNID
        BranchID.EditValue = BID
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        PriceType.SelectedIndex = 0
    End Sub

    Private Sub PriceType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PriceType.SelectedIndexChanged
        If PriceType.SelectedIndex <> 1 Then
            CountryID.EditValue = COUNTRYNID
            CountryID.Enabled = False
        Else
            CountryID.Enabled = True
            CountryID.EditValue = Nothing
        End If
        LoadData()
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        If CountryID.EditValue = COUNTRYNID Then
            BranchID.EditValue = BID
        Else
            BranchID.EditValue = MAINBID
        End If
        LoadData()
    End Sub

    Sub LoadData()
        GridControl11.DataSource = Nothing
        GridControl1.DataSource = Nothing
        If IsEmpty(PriceType) Then Exit Sub
        If IsEmpty(CountryID) Then Exit Sub
        If IsEmpty(BranchID) Then Exit Sub
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = If(PriceType.SelectedIndex = 2, 3, SafeToInt(PriceType.SelectedIndex))}
        PR(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PR(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        LoadToControlar(GridControl11, "GET_TABLE_FOR_All", "", "", PR)
        TileView11.Appearance.GroupText.Font = New Font("Droid Arabic Kufi", 13, FontStyle.Bold)
        TileView11.Appearance.GroupText.ForeColor = Color.DarkOrange
        TileView11.Appearance.GroupText.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        'TileView11.OptionsTiles.GroupTextPadding = New Padding(5, 5, 5, 5)
    End Sub

    Private Sub FrmCurrnceyStatment2026_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Newrecord()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        LoadData()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        GridControl1.DataSource = Nothing
        If Not ValidateControl(PriceType, "نوع التسعير") Then Exit Sub
        If Not ValidateControl(CountryID, "الدولة") Then Exit Sub
        If Not ValidateControl(BranchID, "الفرع") Then Exit Sub
        If Not ValidateControl(D1, "من تاريخ") Then Exit Sub
        If Not ValidateControl(D2, "إلى تاريخ") Then Exit Sub
        Dim PR(5) As SqlParameter
        PR(0) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = If(PriceType.SelectedIndex = 2, 3, SafeToInt(PriceType.SelectedIndex))}
        PR(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PR(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PR(5) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 5}
        LoadToControlar(GridControl1, "NewCurrencyBuyAndSale_CRUD", "", "", PR)
        DVGFormat(GVRole)
        Summary()
    End Sub

    Sub Summary()
        QBuyTotal.Text = 0
        QSaleTotal.Text = 0
        DL_Buy.Text = 0
        DL_Sale.Text = 0
        GridColumnSummaryItem_grivview(GVRole, "Credit", QBuyTotal)
        GridColumnSummaryItem_grivview(GVRole, "Debit", QSaleTotal)
        GridColumnSummaryItem_grivview(GVRole, "DL_Credit", DL_Buy)
        GridColumnSummaryItem_grivview(GVRole, "DL_Debit", DL_Sale)
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        Summary()
    End Sub
End Class