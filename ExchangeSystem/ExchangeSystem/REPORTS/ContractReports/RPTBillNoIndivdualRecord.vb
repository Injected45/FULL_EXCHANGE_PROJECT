Imports System.ComponentModel

Public Class RPTBillNoIndividualRecord
    Private Sub RPTBillNoIndividualRecord_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel9.Text = GetUserName
    End Sub
End Class