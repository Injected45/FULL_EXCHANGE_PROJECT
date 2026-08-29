Imports System.ComponentModel

Public Class RPTSaleCurrencyForCUST
    Private Sub RPTSaleCurrencyForCUST_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel16.Text = FRMSaleCurrencyForCUST.CurrencyID.Text
    End Sub
End Class