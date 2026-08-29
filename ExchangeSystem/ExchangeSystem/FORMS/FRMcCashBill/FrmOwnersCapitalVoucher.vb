Public Class FrmOwnersCapitalVoucher
    Sub NewRecord()
        New_Controlrs(Me)
        LoadToControlar(BranchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        LoadToControlar(CurrencyID, "CURRENCYTB_LoadToLKP", "CurrencyName", "ID", Nothing)
        InsertDate.EditValue = Date.Now
        Code.Text = GETMAXID("OwnersCapitalVouchersTB", "ID")

    End Sub
End Class