Imports System.ComponentModel

Public Class RPTEXPESESMOVEMENTTATEMENTS
    Private Sub RPTEXPESESMOVEMENTTATEMENTS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = Format(FRMEXPESESMOVEMENTTATEMENTS.D1.EditValue, "yyy-MM-dd")
        D2.Text = Format(FRMEXPESESMOVEMENTTATEMENTS.D1.EditValue, "yyy-MM-dd")
        XrLabel6.Text = GetUserName
        XrLabel1.Text = FRMEXPESESMOVEMENTTATEMENTS.BranchID.Text

    End Sub
End Class