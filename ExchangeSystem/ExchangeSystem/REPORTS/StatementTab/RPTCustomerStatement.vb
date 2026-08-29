Imports System.ComponentModel
Imports DevExpress.XtraReports.UI

Public Class RPTCustomerStatement
    Private Sub RPTCustomerStatement_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel2.Text = GetUserName
        XrLabel4.Text = FRMCustomerAccountStatement.BranchID.Text
        XrLabel13.Text = FRMCustomerAccountStatement.CurrencyTo.Text
    End Sub
End Class