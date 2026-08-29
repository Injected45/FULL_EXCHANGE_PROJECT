<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ExValSahreByHand
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.ExVal = New DevExpress.XtraEditors.SpinEdit()
        Me.ExValShare = New DevExpress.XtraEditors.SpinEdit()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton21 = New DevExpress.XtraEditors.SimpleButton()
        Me.InsertDate = New DevExpress.XtraEditors.DateEdit()
        Me.ExValShare1 = New DevExpress.XtraEditors.SpinEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExValShare.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExValShare1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.ExVal)
        Me.LayoutControl1.Controls.Add(Me.ExValShare)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton21)
        Me.LayoutControl1.Controls.Add(Me.InsertDate)
        Me.LayoutControl1.Controls.Add(Me.ExValShare1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(835, 206)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(407, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Properties.ReadOnly = True
        Me.CodeID.Size = New System.Drawing.Size(332, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'ExVal
        '
        Me.ExVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ExVal.Enabled = False
        Me.ExVal.Location = New System.Drawing.Point(16, 58)
        Me.ExVal.Name = "ExVal"
        Me.ExVal.Properties.Appearance.Options.UseTextOptions = True
        Me.ExVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ExVal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExVal.Properties.MaskSettings.Set("mask", "n3")
        Me.ExVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.ExVal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.ExVal.Properties.UseMaskAsDisplayFormat = True
        Me.ExVal.Size = New System.Drawing.Size(723, 36)
        Me.ExVal.StyleController = Me.LayoutControl1
        Me.ExVal.TabIndex = 2
        '
        'ExValShare
        '
        Me.ExValShare.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ExValShare.Location = New System.Drawing.Point(407, 100)
        Me.ExValShare.Name = "ExValShare"
        Me.ExValShare.Properties.Appearance.Options.UseTextOptions = True
        Me.ExValShare.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExValShare.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ExValShare.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExValShare.Properties.MaskSettings.Set("mask", "N1")
        Me.ExValShare.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.ExValShare.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.ExValShare.Properties.UseMaskAsDisplayFormat = True
        Me.ExValShare.Size = New System.Drawing.Size(332, 36)
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
        Me.SimpleButton2.Location = New System.Drawing.Point(407, 142)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(412, 28)
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
        Me.SimpleButton21.Location = New System.Drawing.Point(16, 142)
        Me.SimpleButton21.Name = "SimpleButton21"
        Me.SimpleButton21.Size = New System.Drawing.Size(385, 28)
        Me.SimpleButton21.StyleController = Me.LayoutControl1
        Me.SimpleButton21.TabIndex = 2
        Me.SimpleButton21.Text = "إلغاء"
        '
        'InsertDate
        '
        Me.InsertDate.EditValue = Nothing
        Me.InsertDate.Location = New System.Drawing.Point(16, 16)
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
        Me.InsertDate.Size = New System.Drawing.Size(305, 36)
        Me.InsertDate.StyleController = Me.LayoutControl1
        Me.InsertDate.TabIndex = 2
        '
        'ExValShare1
        '
        Me.ExValShare1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ExValShare1.Location = New System.Drawing.Point(16, 100)
        Me.ExValShare1.Name = "ExValShare1"
        Me.ExValShare1.Properties.Appearance.Options.UseTextOptions = True
        Me.ExValShare1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExValShare1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ExValShare1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExValShare1.Properties.MaskSettings.Set("mask", "N1")
        Me.ExValShare1.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.ExValShare1.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.ExValShare1.Properties.UseMaskAsDisplayFormat = True
        Me.ExValShare1.Size = New System.Drawing.Size(305, 36)
        Me.ExValShare1.StyleController = Me.LayoutControl1
        Me.ExValShare1.TabIndex = 3
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem13, Me.LayoutControlItem2, Me.LayoutControlItem6})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(835, 206)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(391, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(418, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(64, 21)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton2
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(391, 126)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(418, 54)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.SimpleButton21
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(391, 54)
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
        Me.LayoutControlItem5.Size = New System.Drawing.Size(391, 42)
        Me.LayoutControlItem5.Text = "التاريخ"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(64, 21)
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.ExVal
        Me.LayoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem13.CustomizationFormText = "الراتب"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(809, 42)
        Me.LayoutControlItem13.Text = "قيمة العمولة"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(64, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.ExValShare
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الراتب"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(391, 84)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(418, 42)
        Me.LayoutControlItem2.Text = "عمولة الوكيل"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(64, 21)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.ExValShare1
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "الراتب"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(391, 42)
        Me.LayoutControlItem6.Text = "عمولة الوكيل"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(64, 21)
        '
        'ExValSahreByHand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(835, 206)
        Me.ControlBox = False
        Me.Controls.Add(Me.LayoutControl1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExValSahreByHand"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "خصم يدوي"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExValShare.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExValShare1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ExVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents ExValShare As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton21 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents InsertDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ExValShare1 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class
