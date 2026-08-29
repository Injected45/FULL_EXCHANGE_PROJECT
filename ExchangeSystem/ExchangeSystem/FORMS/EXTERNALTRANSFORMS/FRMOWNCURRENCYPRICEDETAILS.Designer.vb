<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMOWNCURRENCYPRICEDETAILS
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.PrintBTN = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton7 = New DevExpress.XtraEditors.SimpleButton()
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.LBLCOUNTFORMSG = New DevExpress.XtraEditors.LabelControl()
        Me.SimpleButton6 = New DevExpress.XtraEditors.SimpleButton()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Black
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.Controls.Add(Me.PrintBTN)
        Me.PanelControl1.Controls.Add(Me.SimpleButton2)
        Me.PanelControl1.Controls.Add(Me.SimpleButton1)
        Me.PanelControl1.Controls.Add(Me.SimpleButton7)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(1080, 55)
        Me.PanelControl1.TabIndex = 0
        '
        'PrintBTN
        '
        Me.PrintBTN.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrintBTN.Appearance.BackColor = System.Drawing.Color.Black
        Me.PrintBTN.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PrintBTN.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(229, Byte), Integer), CType(CType(124, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.PrintBTN.Appearance.Options.UseBackColor = True
        Me.PrintBTN.Appearance.Options.UseFont = True
        Me.PrintBTN.Appearance.Options.UseForeColor = True
        Me.PrintBTN.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.PrintBTN.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.printer
        Me.PrintBTN.ImageOptions.SvgImageSize = New System.Drawing.Size(40, 40)
        Me.PrintBTN.Location = New System.Drawing.Point(445, 2)
        Me.PrintBTN.Name = "PrintBTN"
        Me.PrintBTN.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        Me.PrintBTN.Size = New System.Drawing.Size(166, 44)
        Me.PrintBTN.TabIndex = 16
        Me.PrintBTN.Text = "     طباعة "
        Me.PrintBTN.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.Cursor
        Me.PrintBTN.ToolTipIconType = DevExpress.Utils.ToolTipIconType.WindLogo
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Appearance.BackColor = System.Drawing.Color.Black
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(229, Byte), Integer), CType(CType(124, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.SimpleButton2.Appearance.Options.UseBackColor = True
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton2.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.pie_chart
        Me.SimpleButton2.ImageOptions.SvgImageSize = New System.Drawing.Size(40, 40)
        Me.SimpleButton2.Location = New System.Drawing.Point(869, 0)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        Me.SimpleButton2.Size = New System.Drawing.Size(211, 46)
        Me.SimpleButton2.TabIndex = 14
        Me.SimpleButton2.Text = "نشرة النقدي"
        Me.SimpleButton2.ToolTip = "نشرة النقدي"
        Me.SimpleButton2.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.Cursor
        Me.SimpleButton2.ToolTipIconType = DevExpress.Utils.ToolTipIconType.WindLogo
        Me.SimpleButton2.ToolTipTitle = "حركات النشرة"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Appearance.BackColor = System.Drawing.Color.Black
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(229, Byte), Integer), CType(CType(124, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.SimpleButton1.Appearance.Options.UseBackColor = True
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton1.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.request
        Me.SimpleButton1.ImageOptions.SvgImageSize = New System.Drawing.Size(40, 40)
        Me.SimpleButton1.Location = New System.Drawing.Point(618, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        Me.SimpleButton1.Size = New System.Drawing.Size(230, 44)
        Me.SimpleButton1.TabIndex = 13
        Me.SimpleButton1.Text = "نشـــــــــــــــــرة المصرف"
        Me.SimpleButton1.ToolTip = "أسعار  عملة علي مصارف"
        Me.SimpleButton1.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.Cursor
        Me.SimpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.WindLogo
        Me.SimpleButton1.ToolTipTitle = "عرض اشرة العملة علي مصرف "
        '
        'SimpleButton7
        '
        Me.SimpleButton7.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton7.Appearance.Options.UseFont = True
        Me.SimpleButton7.Dock = System.Windows.Forms.DockStyle.Left
        Me.SimpleButton7.Location = New System.Drawing.Point(2, 2)
        Me.SimpleButton7.Name = "SimpleButton7"
        Me.SimpleButton7.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        Me.SimpleButton7.Size = New System.Drawing.Size(39, 51)
        Me.SimpleButton7.TabIndex = 12
        Me.SimpleButton7.ToolTip = "إغلاق النظام"
        Me.SimpleButton7.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.Cursor
        Me.SimpleButton7.ToolTipIconType = DevExpress.Utils.ToolTipIconType.WindLogo
        Me.SimpleButton7.ToolTipTitle = "إغلاق النظام"
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.Color.Black
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl3.Location = New System.Drawing.Point(0, 55)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(1080, 665)
        Me.PanelControl3.TabIndex = 2
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.LBLCOUNTFORMSG)
        Me.Panel2.Controls.Add(Me.SimpleButton6)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 665)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1080, 55)
        Me.Panel2.TabIndex = 3
        '
        'LBLCOUNTFORMSG
        '
        Me.LBLCOUNTFORMSG.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LBLCOUNTFORMSG.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LBLCOUNTFORMSG.Appearance.ForeColor = System.Drawing.Color.Red
        Me.LBLCOUNTFORMSG.Appearance.Options.UseFont = True
        Me.LBLCOUNTFORMSG.Appearance.Options.UseForeColor = True
        Me.LBLCOUNTFORMSG.Appearance.Options.UseTextOptions = True
        Me.LBLCOUNTFORMSG.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.LBLCOUNTFORMSG.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom
        Me.LBLCOUNTFORMSG.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LBLCOUNTFORMSG.Location = New System.Drawing.Point(31, 3)
        Me.LBLCOUNTFORMSG.Name = "LBLCOUNTFORMSG"
        Me.LBLCOUNTFORMSG.Size = New System.Drawing.Size(10, 25)
        Me.LBLCOUNTFORMSG.TabIndex = 20
        Me.LBLCOUNTFORMSG.Text = "0"
        '
        'SimpleButton6
        '
        Me.SimpleButton6.Appearance.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton6.Appearance.Options.UseFont = True
        Me.SimpleButton6.Dock = System.Windows.Forms.DockStyle.Left
        Me.SimpleButton6.Location = New System.Drawing.Point(0, 0)
        Me.SimpleButton6.Name = "SimpleButton6"
        Me.SimpleButton6.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        Me.SimpleButton6.Size = New System.Drawing.Size(41, 55)
        Me.SimpleButton6.TabIndex = 19
        Me.SimpleButton6.ToolTip = "اشعارات النظام"
        Me.SimpleButton6.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.[Object]
        Me.SimpleButton6.ToolTipIconType = DevExpress.Utils.ToolTipIconType.WindLogo
        Me.SimpleButton6.ToolTipTitle = "استعلامات النظام"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.LabelControl2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1080, 55)
        Me.Panel3.TabIndex = 18
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(229, Byte), Integer), CType(CType(124, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.LabelControl2.Appearance.Options.UseFont = True
        Me.LabelControl2.Appearance.Options.UseForeColor = True
        Me.LabelControl2.Appearance.Options.UseTextOptions = True
        Me.LabelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl2.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LabelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl2.AppearanceDisabled.Options.UseTextOptions = True
        Me.LabelControl2.AppearanceDisabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl2.AppearanceHovered.Options.UseTextOptions = True
        Me.LabelControl2.AppearanceHovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl2.AppearancePressed.Options.UseTextOptions = True
        Me.LabelControl2.AppearancePressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.LabelControl2.AutoEllipsis = True
        Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl2.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft
        Me.LabelControl2.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.LabelControl2.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.LabelControl2.Location = New System.Drawing.Point(11, -1)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(1069, 52)
        Me.LabelControl2.TabIndex = 19
        Me.LabelControl2.Text = "اسعار العملات النقدي الاجنبي لدي شركة الرحالة الاولى"
        '
        'FRMOWNCURRENCYPRICEDETAILS
        '
        Me.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1080, 720)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.PanelControl3)
        Me.Controls.Add(Me.PanelControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FRMOWNCURRENCYPRICEDETAILS"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "FRMOWNCURRENCYPRICEDETAILS"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub


    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Panel2 As Panel
    Friend WithEvents LBLCOUNTFORMSG As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton6 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Panel3 As Panel
    Friend WithEvents SimpleButton7 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PrintBTN As DevExpress.XtraEditors.SimpleButton
    Public WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
End Class
