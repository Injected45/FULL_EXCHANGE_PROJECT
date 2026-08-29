Imports System.Data.SqlClient

Public Class CLSCURRENCYPRICE
    Public Sub CURRENCYPRICE_Insert(ID As ULong, dt As DataTable, CurrencyPower As Boolean, IsUpdate As Boolean)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = ID}
        PRM(1) = New SqlParameter("@TypeTb", SqlDbType.Structured) With {.Value = dt}
        PRM(2) = New SqlParameter("@CurrencyPower", SqlDbType.Bit) With {.Value = CurrencyPower}
        PRM(3) = New SqlParameter("@IsUpdate", SqlDbType.Money) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("CurrencyPricesTb_Insert", PRM)

    End Sub
End Class
