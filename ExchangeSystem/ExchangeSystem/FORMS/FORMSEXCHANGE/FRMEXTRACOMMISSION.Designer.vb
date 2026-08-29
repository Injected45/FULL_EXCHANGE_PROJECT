<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMEXTRACOMMISSION
    Inherits TemplateForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMEXTRACOMMISSION))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.ExVal = New DevExpress.XtraEditors.SpinEdit()
        Me.ExtraCommission = New DevExpress.XtraEditors.SpinEdit()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton21 = New DevExpress.XtraEditors.SimpleButton()
        Me.InsertDate = New DevExpress.XtraEditors.DateEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtraCommission.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.ExVal)
        Me.LayoutControl1.Controls.Add(Me.ExtraCommission)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton21)
        Me.LayoutControl1.Controls.Add(Me.InsertDate)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(795, 188)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(383, 15)
        Me.CodeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Properties.ReadOnly = True
        Me.CodeID.Size = New System.Drawing.Size(292, 46)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'ExVal
        '
        Me.ExVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ExVal.Enabled = False
        Me.ExVal.Location = New System.Drawing.Point(383, 67)
        Me.ExVal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ExVal.Name = "ExVal"
        Me.ExVal.Properties.Appearance.Options.UseTextOptions = True
        Me.ExVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ExVal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExVal.Properties.MaskSettings.Set("mask", "n")
        Me.ExVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.ExVal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.ExVal.Properties.UseMaskAsDisplayFormat = True
        Me.ExVal.Size = New System.Drawing.Size(292, 46)
        Me.ExVal.StyleController = Me.LayoutControl1
        Me.ExVal.TabIndex = 2
        '
        'ExtraCommission
        '
        Me.ExtraCommission.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ExtraCommission.Location = New System.Drawing.Point(21, 67)
        Me.ExtraCommission.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ExtraCommission.Name = "ExtraCommission"
        Me.ExtraCommission.Properties.Appearance.Options.UseTextOptions = True
        Me.ExtraCommission.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExtraCommission.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ExtraCommission.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtraCommission.Properties.MaskSettings.Set("mask", "n")
        Me.ExtraCommission.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.ExtraCommission.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.ExtraCommission.Properties.UseMaskAsDisplayFormat = True
        Me.ExtraCommission.Size = New System.Drawing.Size(255, 46)
        Me.ExtraCommission.StyleController = Me.LayoutControl1
        Me.ExtraCommission.TabIndex = 3
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
        Me.SimpleButton2.Location = New System.Drawing.Point(383, 119)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(391, 34)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "موافق"
        '
        'SimpleButton21
        '
        Me.SimpleButton21.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.SimpleButton21.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton21.Appearance.Options.UseBackColor = True
        Me.SimpleButton21.Appearance.Options.UseForeColor = True
        Me.SimpleButton21.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton21.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton21.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton21.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton21.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton21.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton21.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton21.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.exit_logout
        Me.SimpleButton21.ImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.SimpleButton21.Location = New System.Drawing.Point(21, 119)
        Me.SimpleButton21.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton21.Name = "SimpleButton21"
        Me.SimpleButton21.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton21.StyleController = Me.LayoutControl1
        Me.SimpleButton21.TabIndex = 2
        Me.SimpleButton21.Text = "إلغاء"
        '
        'InsertDate
        '
        Me.InsertDate.EditValue = Nothing
        Me.InsertDate.Location = New System.Drawing.Point(21, 15)
        Me.InsertDate.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Properties.Appearance.Options.UseTextOptions = True
        Me.InsertDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertDate.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.InsertDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.InsertDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.InsertDate.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.InsertDate.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.InsertDate.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.InsertDate.Properties.ReadOnly = True
        Me.InsertDate.Properties.UseMaskAsDisplayFormat = True
        Me.InsertDate.Size = New System.Drawing.Size(255, 46)
        Me.InsertDate.StyleController = Me.LayoutControl1
        Me.InsertDate.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem13, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem5})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(795, 188)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(362, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(399, 52)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(78, 27)
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.ExVal
        Me.LayoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem13.CustomizationFormText = "الراتب"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(362, 52)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(399, 52)
        Me.LayoutControlItem13.Text = "قيمة العمولة"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(78, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.ExtraCommission
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الراتب"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(362, 52)
        Me.LayoutControlItem2.Text = "الخصم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(78, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton2
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(362, 104)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(399, 60)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.SimpleButton21
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(362, 60)
        Me.LayoutControlItem3.Text = "LayoutControlItem4"
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.InsertDate
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(362, 52)
        Me.LayoutControlItem5.Text = "التاريخ"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(78, 27)
        '
        'FRMEXTRACOMMISSION
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(795, 188)
        Me.ControlBox = False
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMEXTRACOMMISSION.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMEXTRACOMMISSION"
        Me.Text = "خصم من العمولة"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtraCommission.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ExVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents ExtraCommission As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton21 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents InsertDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
End Class
