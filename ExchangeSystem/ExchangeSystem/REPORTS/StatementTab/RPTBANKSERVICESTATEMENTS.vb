Imports System.ComponentModel

Public Class RPTBANKSERVICESTATEMENTS
    Private Sub RPTBANKSERVICESTATEMENTS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FRMBANKSERVICESTATEMENTS.D1.EditValue
        D2.Text = FRMBANKSERVICESTATEMENTS.D2.EditValue
        XrLabel9.Text = FRMBANKSERVICESTATEMENTS.ServiceID.Text
        XrLabel18.Text = FRMBANKSERVICESTATEMENTS.BranchID.Text
        XrLabel7.Text = FRMBANKSERVICESTATEMENTS.BBranchID.Text
        XrLabel8.Text = GetUserName
    End Sub
End Class