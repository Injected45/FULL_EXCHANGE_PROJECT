Imports System.ComponentModel

Public Class RPTSelectAccountsBetweenBranches
    Private Sub RPTSelectAccountsBetweenBranches_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        D1.Text = FrmSelectAccountsBetweenBranches.D1.EditValue
        D2.Text = FrmSelectAccountsBetweenBranches.D2.EditValue
        XrLabel4.Text = FrmSelectAccountsBetweenBranches.BranchID.Text
        OverAllTotal.Text = Format(FrmSelectAccountsBetweenBranches.OverAllBenefits.EditValue, "N3")
        OverAllTotal1.Text = Format(FrmSelectAccountsBetweenBranches.OverAllNetTotal.EditValue, "N3")
        XrLabel5.Text = GetUserName
    End Sub
End Class