
Module MD_MESSAGE
    Public NUMBER_MASSAG As Integer = 0
    Public IF_YES As Boolean = 0
    Public Sub MASAGE_BEGEN(ByVal TIMER As Timer, ByVal PIC As PictureBox, ByVal TXT_MASAG As Label, ByVal TXT_ As String)
        TIMER.Start()
        TXT_MASAG.Visible = True
        PIC.Visible = True
        TXT_MASAG.ForeColor = Color.Black
        PIC.Image = My.Resources.loading
        TXT_MASAG.Text = TXT_
        TXT_MASAG.BackColor = Color.White
        PIC.BackColor = Color.White
    End Sub
    Public Sub MASAGE_END(ByVal TIMER As Timer, ByVal PIC As PictureBox, ByVal TXT_MASAG As Label, ByVal TXT_ As String, ByVal TIMER_CLOS As Timer)
        TXT_MASAG.Visible = True
        PIC.Visible = True
        TXT_MASAG.ForeColor = Color.Black
        PIC.Image = My.Resources.TRUE2
        TXT_MASAG.Text = TXT_
        TXT_MASAG.BackColor = Color.Wheat
        PIC.BackColor = Color.Wheat
        TIMER.Stop()
        TIMER_CLOS.Start()
    End Sub
    Public Sub MASAGE_ERERE(ByVal TIMER As Timer, ByVal PIC As PictureBox, ByVal TXT_MASAG As Label, ByVal TXT_ As String)
        TXT_MASAG.Visible = True
        PIC.Visible = True
        TXT_MASAG.ForeColor = Color.Black
        PIC.Image = My.Resources.FALSE2
        TXT_MASAG.Text = TXT_
        TXT_MASAG.BackColor = Color.Red
        PIC.BackColor = Color.Red
        TIMER.Start()
    End Sub
    Public Sub MASAGE_ERERE_FOCUS(ByVal TIMER As Timer, ByVal PIC As PictureBox, ByVal TXT_MASAG As Label, ByVal TXT_ As String, ByVal TBOX As TextBox)
        TXT_MASAG.Visible = True
        PIC.Visible = True
        TXT_MASAG.ForeColor = Color.Black
        PIC.Image = My.Resources.FALSE2
        TXT_MASAG.Text = TXT_
        TXT_MASAG.BackColor = Color.Red
        PIC.BackColor = Color.Red
        TIMER.Start()
        TBOX.Focus()
    End Sub
    'Public Sub MASAGE_FRM_CHOSE(ByVal MSG_ As String, ByVal TITLE_ As String)
    '    Dim F As New FRM_CHOSE
    '    F.TXT_MASSAGE.Text = MSG_
    '    F.LBL_TITLE.Text = TITLE_
    '    F.ShowDialog()
    'End Sub
End Module
