Imports System.ComponentModel

Public Class RptBankPortfolio
    Private Sub RptBankPortfolio_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        TxtDate.Text = Date.Now.ToString("dd/MM/yyyy")
        XrLabel24.Text = GetUserName
    End Sub
End Class