Imports System.ComponentModel

Public Class RPTView_RecieveInternalEx
    Private Sub RPTView_RecieveInternalEx_BeforePrint(sender As Object, e As CancelEventArgs) Handles Me.BeforePrint
        XrLabel6.Text = View_RecieveInternalEx.BranchID.Text
        XrLabel7.Text = View_RecieveInternalEx.CUST1.Text
        D1.Text = View_RecieveInternalEx.D1.EditValue
        D2.Text = View_RecieveInternalEx.D2.EditValue
        OverAllNet.Text = Cur_Code("دينار ليبي", View_RecieveInternalEx.ExValtotal.Text, True, False)
        OverAllTotal.Text = Cur_Code("دينار ليبي", View_RecieveInternalEx.OverallVal1.Text, True, False)
        If View_RecieveInternalEx.GVRole.ActiveFilterString.Contains("صادرة") Then
                XrLabel5.Text = "كشف حركة التحويلات الصادرة"
            End If
        If View_RecieveInternalEx.GVRole.ActiveFilterString.Contains("واردة") Then
            XrLabel5.Text = "كشف حركة التحويلات الواردة"
        End If
        XrLabel4.Text = GetUserName
    End Sub
End Class