Public Class FRMLoadOwnName
    Sub LoadOwnAccountName()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("BankDipWdTb_LoadOwnAccountName")
        If dt.Rows.Count > 0 Then
            LSBOX.DataSource = dt
            LSBOX.ValueMember = "OwnAccountPhone"
            LSBOX.DisplayMember = "OwnAccountName"
            'TxtName.Properties.ShowHeader = False
        End If

    End Sub


    Public Sub FRMSELECTACCOUNT_Load(sender As Object, e As EventArgs) Handles Me.Load
        LSBOX.DataSource = Nothing
        LoadOwnAccountName()
    End Sub


    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        FRMBANKDEPOSIT.TxtName.Text =LSBOX.Text
        FRMBANKDEPOSIT.TxtPhone.Text = LSBOX.SelectedValue
    End Sub
End Class