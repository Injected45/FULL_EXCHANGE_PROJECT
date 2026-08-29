Public Class FRMBayAndSaleCurr2026
    Sub Newrecord()
        New_Controlrs(Me)
        Enable_Controls(Me, True)
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnPrint.Enabled = False
        BankID.Enabled = False
        LoadToControlar(CountryIDFrom, "CountriesTb_LoadToLKP", "CName", "ID", Nothing)
        LoadToControlar(BBranchID, "BBranchTb_SelectAll", "BranchName", "ID", Nothing)
    End Sub
    Private Sub FRMBayAndSaleCurr2026_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Newrecord()
    End Sub
End Class