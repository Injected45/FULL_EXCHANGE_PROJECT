Public Class FRMCURRENCYPRICEDTTELSS
    Public frm = New USRCURRENCYPRICEDTTELSS
    Private Sub FRMCURRENCYPRICEDTTELSS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PanelControl1.Height = 55
        SimpleButton2.PerformClick()
    End Sub

    Private Sub SimpleButton7_Click(sender As Object, e As EventArgs) Handles SimpleButton7.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If LabelControl2.Left < Me.Width Then
            LabelControl2.Left += 7
        Else
            LabelControl2.Left = -LabelControl2.Width
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        frm = New USRCURRENCYPRICEDTTELSS
        frm.Dock = DockStyle.Fill
        PanelControl3.Controls.Clear()
        PanelControl3.Controls.Add(frm)
        frm.LOADCIDFROM(1)
        Timer1.Start()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        frm = New USRCURRENCYPRICEDTTELSS
        frm.Dock = DockStyle.Fill
        PanelControl3.Controls.Clear()
        PanelControl3.Controls.Add(frm)
        frm.LOADCIDFROM(2)
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        FRMCurrencyMovements.TypeID = 1
        FRMCurrencyMovements.ShowDialog()
    End Sub

    Private Sub PanelControl3_Paint(sender As Object, e As PaintEventArgs) Handles PanelControl3.Paint

    End Sub

    Private Sub PrintBTN_Click(sender As Object, e As EventArgs) Handles PrintBTN.Click
        frm.printRPT()
    End Sub
End Class