Imports System.ComponentModel

Public Class RPTMAINSAFEDETAILS
    Private Sub RPTMAINSAFEDETAILS_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        BranchID.Text = FRMBRANCHSAFEDETALIS.SafeName
        XrLabel8.Text = GetUserName
        If FRMBRANCHSAFEDETALIS.OverAllCredit.EditValue > 0.00 Then
            OverAllCredit.Text = Format(FRMBRANCHSAFEDETALIS.OverAllCredit.EditValue, "N3")
        Else
            OverAllCredit.Text = Format(0.000, "N3")
        End If
        If FRMBRANCHSAFEDETALIS.OverAllDebit.EditValue > 0.00 Then
            OverAllDebit.Text = Format(FRMBRANCHSAFEDETALIS.OverAllDebit.EditValue, "N3")
        Else
            OverAllDebit.Text = Format(0.000, "N3")
        End If
        If FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.EditValue > 0.00 Then

            OverAllPeroidTotal.Text = Format(FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.EditValue, "N3")
        Else
            OverAllPeroidTotal.Text = Format(0.000, "N3")
        End If
        If FRMBRANCHSAFEDETALIS.OverallPrint > 0.000 Then
            OverAllTotal.Text = Format(FRMBRANCHSAFEDETALIS.OverallPrint, "N3")
        Else
            OverAllTotal.Text = Format(0.000, "N3")
        End If
    End Sub
End Class