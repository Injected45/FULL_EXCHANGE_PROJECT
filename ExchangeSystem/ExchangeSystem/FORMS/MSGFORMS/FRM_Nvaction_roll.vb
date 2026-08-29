

Public Class FRM_Nvaction_roll
    Inherits DevExpress.XtraEditors.XtraForm

    Public Property NotificationText As String = "رسالة افتراضية"
    Private Const PaddingX As Integer = 20
    Private Const PaddingY As Integer = 20


    Public Sub New(notificationText As String)
        InitializeComponent()
        lblMessageControl.Text = notificationText
        Using g As Graphics = Me.CreateGraphics()
            Dim size As SizeF = g.MeasureString(notificationText, lblMessageControl.Font)
            Me.Width = Math.Max(400, CInt(size.Width) + 80)
            Me.Height = Math.Max(80, CInt(size.Height) + 40)
        End Using
    End Sub

    Private Sub FRM_Nvaction_roll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Dim screenArea As Rectangle = Screen.PrimaryScreen.WorkingArea
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(screenArea.Right - Me.Width - 10, screenArea.Bottom - Me.Height - 10)

        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Me.Close()
    End Sub

End Class
