<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMAGNETBENEFITGROUPS
    Inherits FrmMaster

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
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.GRNAME = New DevExpress.XtraEditors.TextEdit()
        Me.AGNETRATE = New DevExpress.XtraEditors.SpinEdit()
        Me.SECONDPARTYRATE = New DevExpress.XtraEditors.SpinEdit()
        Me.SEARCHTXT = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GRNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AGNETRATE.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SECONDPARTYRATE.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEARCHTXT.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.GRNAME)
        Me.LayoutControl1.Controls.Add(Me.AGNETRATE)
        Me.LayoutControl1.Controls.Add(Me.SECONDPARTYRATE)
        Me.LayoutControl1.Controls.Add(Me.SEARCHTXT)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsFocus.EnableAutoTabOrder = False
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(876, 192)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(428, 16)
        Me.CodeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Properties.ReadOnly = True
        Me.CodeID.Size = New System.Drawing.Size(291, 46)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 5
        '
        'GRNAME
        '
        Me.GRNAME.Location = New System.Drawing.Point(22, 68)
        Me.GRNAME.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GRNAME.Name = "GRNAME"
        Me.GRNAME.Properties.Appearance.Options.UseTextOptions = True
        Me.GRNAME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GRNAME.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GRNAME.Size = New System.Drawing.Size(697, 46)
        Me.GRNAME.StyleController = Me.LayoutControl1
        Me.GRNAME.TabIndex = 0
        '
        'AGNETRATE
        '
        Me.AGNETRATE.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.AGNETRATE.Location = New System.Drawing.Point(428, 120)
        Me.AGNETRATE.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.AGNETRATE.Name = "AGNETRATE"
        Me.AGNETRATE.Properties.Appearance.Options.UseTextOptions = True
        Me.AGNETRATE.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AGNETRATE.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AGNETRATE.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.AGNETRATE.Properties.MaskSettings.Set("mask", "f")
        Me.AGNETRATE.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.AGNETRATE.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.AGNETRATE.Properties.UseMaskAsDisplayFormat = True
        Me.AGNETRATE.Size = New System.Drawing.Size(291, 46)
        Me.AGNETRATE.StyleController = Me.LayoutControl1
        Me.AGNETRATE.TabIndex = 1
        '
        'SECONDPARTYRATE
        '
        Me.SECONDPARTYRATE.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.SECONDPARTYRATE.Location = New System.Drawing.Point(22, 120)
        Me.SECONDPARTYRATE.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SECONDPARTYRATE.Name = "SECONDPARTYRATE"
        Me.SECONDPARTYRATE.Properties.Appearance.Options.UseTextOptions = True
        Me.SECONDPARTYRATE.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SECONDPARTYRATE.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SECONDPARTYRATE.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SECONDPARTYRATE.Properties.MaskSettings.Set("mask", "f")
        Me.SECONDPARTYRATE.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.SECONDPARTYRATE.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.SECONDPARTYRATE.Properties.UseMaskAsDisplayFormat = True
        Me.SECONDPARTYRATE.Size = New System.Drawing.Size(263, 46)
        Me.SECONDPARTYRATE.StyleController = Me.LayoutControl1
        Me.SECONDPARTYRATE.TabIndex = 3
        '
        'SEARCHTXT
        '
        Me.SEARCHTXT.Location = New System.Drawing.Point(22, 16)
        Me.SEARCHTXT.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SEARCHTXT.Name = "SEARCHTXT"
        Me.SEARCHTXT.Properties.Appearance.Options.UseTextOptions = True
        Me.SEARCHTXT.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SEARCHTXT.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SEARCHTXT.Size = New System.Drawing.Size(263, 46)
        Me.SEARCHTXT.StyleController = Me.LayoutControl1
        Me.SEARCHTXT.TabIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem6, Me.LayoutControlItem12})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(876, 192)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(406, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(434, 52)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(113, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GRNAME
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(840, 52)
        Me.LayoutControlItem2.Text = "اسم المجموعة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(113, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.SECONDPARTYRATE
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "الراتب"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(406, 62)
        Me.LayoutControlItem3.Text = "نسبة الطرف الثاني"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(113, 27)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SEARCHTXT
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem6.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.LayoutControlItem6.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.searchdata
        Me.LayoutControlItem6.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(406, 52)
        Me.LayoutControlItem6.Text = " "
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Right
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(113, 27)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.AGNETRATE
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(406, 104)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(434, 62)
        Me.LayoutControlItem12.Text = "نسبة الوكيل"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(113, 27)
        '
        'FRMAGNETBENEFITGROUPS
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(876, 245)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.percentage
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "FRMAGNETBENEFITGROUPS"
        Me.Text = "نموذج تقسيم العمولة للوكلاء"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GRNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AGNETRATE.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SECONDPARTYRATE.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEARCHTXT.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GRNAME As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AGNETRATE As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SECONDPARTYRATE As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SEARCHTXT As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class
