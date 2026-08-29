Imports System.ComponentModel
Imports DevExpress.XtraReports.UI

Public Class RPTSETTLEMENTSTATEMENT

    Private Sub RPTSETTLEMENTSTATEMENT_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FRMSETTLEMENTSTATEMENT.D1.EditValue
        D2.Text = FRMSETTLEMENTSTATEMENT.D2.EditValue
        XrLabel4.Text = GetUserName
        XrLabel7.Text = FRMSETTLEMENTSTATEMENT.IsSettlement.Text
        XrLabel6.Text = FRMSETTLEMENTSTATEMENT.BranchID.Text
        If FRMSETTLEMENTSTATEMENT.IsSettlement.SelectedIndex = 1 Then
            tableCell5.Text = "العهدة"
        Else
            tableCell5.Text = "الموظف"
        End If

    End Sub
End Class