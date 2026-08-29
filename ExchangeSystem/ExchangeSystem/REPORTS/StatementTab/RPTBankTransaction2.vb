Imports System.ComponentModel

Public Class RPTBankTransaction2
    Private Sub RPTBankTransaction2_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel11.Text = GetUserName
        D1.Text = FRMBankTransaction.D1.Text
        D2.Text = FRMBankTransaction.D2.Text
    End Sub
End Class