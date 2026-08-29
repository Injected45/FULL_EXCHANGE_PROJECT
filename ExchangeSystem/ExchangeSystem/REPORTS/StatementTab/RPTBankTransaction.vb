Imports System.ComponentModel

Public Class RPTBankTransaction
    Private Sub RPTBankTransaction_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel11.Text = GetUserName
        'XrLabel5.Text = FRMBankTransaction.TransType.Text
        D1.Text = FRMBankTransaction.D1.Text
        D2.Text = FRMBankTransaction.D2.Text
    End Sub
End Class