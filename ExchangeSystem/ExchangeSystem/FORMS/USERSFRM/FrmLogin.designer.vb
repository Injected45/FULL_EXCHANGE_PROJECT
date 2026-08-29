<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLogin
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Login = New DevExpress.XtraEditors.SimpleButton()
        Me.CancelLog = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        Me.UserPassword = New DevExpress.XtraEditors.TextEdit()
        Me.UserName = New DevExpress.XtraEditors.TextEdit()
        Me.BaWo = New System.ComponentModel.BackgroundWorker()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UserPassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UserName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Login
        '
        Me.Login.ImageOptions.Image = Global.ExchangeSystem.My.Resources.Resources.messaging_32
        Me.Login.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.Login.Location = New System.Drawing.Point(10, 283)
        Me.Login.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Login.Name = "Login"
        Me.Login.Size = New System.Drawing.Size(231, 33)
        Me.Login.TabIndex = 5
        '
        'CancelLog
        '
        Me.CancelLog.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelLog.ImageOptions.Image = Global.ExchangeSystem.My.Resources.Resources.cancel_production_order_16px
        Me.CancelLog.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.CancelLog.Location = New System.Drawing.Point(10, 321)
        Me.CancelLog.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.CancelLog.Name = "CancelLog"
        Me.CancelLog.Size = New System.Drawing.Size(231, 33)
        Me.CancelLog.TabIndex = 6
        '
        'PictureEdit1
        '
        Me.PictureEdit1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureEdit1.EditValue = Global.ExchangeSystem.My.Resources.Resources._2
        Me.PictureEdit1.Location = New System.Drawing.Point(43, 17)
        Me.PictureEdit1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.PictureEdit1.Size = New System.Drawing.Size(157, 155)
        Me.PictureEdit1.TabIndex = 4
        '
        'UserPassword
        '
        Me.UserPassword.Location = New System.Drawing.Point(10, 244)
        Me.UserPassword.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.UserPassword.Name = "UserPassword"
        Me.UserPassword.Properties.Appearance.Options.UseTextOptions = True
        Me.UserPassword.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UserPassword.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.UserPassword.Properties.NullText = "كلمة المرور"
        Me.UserPassword.Properties.NullValuePrompt = "كلمة المرور"
        Me.UserPassword.Properties.UseSystemPasswordChar = True
        Me.UserPassword.Size = New System.Drawing.Size(231, 36)
        Me.UserPassword.TabIndex = 1
        '
        'UserName
        '
        Me.UserName.Location = New System.Drawing.Point(10, 202)
        Me.UserName.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.UserName.Name = "UserName"
        Me.UserName.Properties.Appearance.Options.UseImage = True
        Me.UserName.Properties.Appearance.Options.UseTextOptions = True
        Me.UserName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UserName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.UserName.Properties.NullText = "اسم المستخدم"
        Me.UserName.Properties.NullValuePrompt = "اسم المستخدم"
        Me.UserName.Size = New System.Drawing.Size(230, 36)
        Me.UserName.TabIndex = 0
        '
        'BaWo
        '
        '
        'FrmLogin
        '
        Me.AcceptButton = Me.Login
        Me.Appearance.BackColor = System.Drawing.Color.White
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelLog
        Me.ClientSize = New System.Drawing.Size(250, 374)
        Me.Controls.Add(Me.CancelLog)
        Me.Controls.Add(Me.Login)
        Me.Controls.Add(Me.PictureEdit1)
        Me.Controls.Add(Me.UserPassword)
        Me.Controls.Add(Me.UserName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.IconOptions.Image = Global.ExchangeSystem.My.Resources.Resources.icons8_lock_32
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmLogin"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "شاشة الدخول"
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UserPassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UserName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents UserName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents UserPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents Login As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CancelLog As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BaWo As System.ComponentModel.BackgroundWorker
End Class
