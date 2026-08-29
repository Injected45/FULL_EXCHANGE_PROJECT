<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EXternal_ExValSahreByHand
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.ExValShare = New DevExpress.XtraEditors.SpinEdit()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.ExValShare.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.ExValShare)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(604, 141)
        Me.LayoutControl1.TabIndex = 2
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'ExValShare
        '
        Me.ExValShare.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ExValShare.Location = New System.Drawing.Point(20, 21)
        Me.ExValShare.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ExValShare.Name = "ExValShare"
        Me.ExValShare.Properties.Appearance.Options.UseTextOptions = True
        Me.ExValShare.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExValShare.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ExValShare.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExValShare.Properties.MaskSettings.Set("mask", "n")
        Me.ExValShare.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.ExValShare.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.ExValShare.Properties.UseMaskAsDisplayFormat = True
        Me.ExValShare.Size = New System.Drawing.Size(465, 46)
        Me.ExValShare.StyleController = Me.LayoutControl1
        Me.ExValShare.TabIndex = 3
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.SimpleButton2.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton2.Appearance.Options.UseBackColor = True
        Me.SimpleButton2.Appearance.Options.UseForeColor = True
        Me.SimpleButton2.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton2.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton2.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton2.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton2.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton2.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton2.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton2.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.checked_s
        Me.SimpleButton2.ImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.SimpleButton2.Location = New System.Drawing.Point(20, 75)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(564, 34)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "موافق"
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(604, 141)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton2
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 54)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(572, 53)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.ExValShare
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الراتب"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(572, 54)
        Me.LayoutControlItem2.Text = "عمولة الوكيل"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(79, 27)
        '
        'EXternal_ExValSahreByHand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 27.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 141)
        Me.ControlBox = False
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "EXternal_ExValSahreByHand"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "خصم يدوي"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.ExValShare.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents ExValShare As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
End Class
