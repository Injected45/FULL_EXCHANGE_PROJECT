Public Class FRMADDEXISITMEMBER
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ASSOCIATIONTB_LOADMEMBERS")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "MEMBERNAME"
            LSBOX.ValueMember = "ID"
        End If
    End Sub

    Private Sub FRMADDEXISITMEMBER_Load(sender As Object, e As EventArgs) Handles Me.Load
        LOADDATA()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        FRMADDMEMBER.EMPNAME.Text = LSBOX.Text
        FRMADDMEMBER.EMPNAME.Enabled = False
        Me.Close()
    End Sub
End Class