<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMADDASSOCIATION
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMADDASSOCIATION))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.BANKNAME = New DevExpress.XtraEditors.TextEdit()
        Me.LSBOX = New System.Windows.Forms.ListBox()
        Me.BtnNew = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnSave = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnEdit = New DevExpress.XtraEditors.SimpleButton()
        Me.AssValue = New DevExpress.XtraEditors.SpinEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BANKNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AssValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.BANKNAME)
        Me.LayoutControl1.Controls.Add(Me.LSBOX)
        Me.LayoutControl1.Controls.Add(Me.BtnNew)
        Me.LayoutControl1.Controls.Add(Me.BtnSave)
        Me.LayoutControl1.Controls.Add(Me.BtnEdit)
        Me.LayoutControl1.Controls.Add(Me.AssValue)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(402, 589)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(20, 20)
        Me.CodeID.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(258, 46)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(20, 74)
        Me.IsActiveTG.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(251, 38)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 2
        '
        'BANKNAME
        '
        Me.BANKNAME.Location = New System.Drawing.Point(20, 120)
        Me.BANKNAME.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BANKNAME.Name = "BANKNAME"
        Me.BANKNAME.Properties.Appearance.Options.UseTextOptions = True
        Me.BANKNAME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BANKNAME.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BANKNAME.Size = New System.Drawing.Size(258, 46)
        Me.BANKNAME.StyleController = Me.LayoutControl1
        Me.BANKNAME.TabIndex = 5
        '
        'LSBOX
        '
        Me.LSBOX.FormattingEnabled = True
        Me.LSBOX.ItemHeight = 27
        Me.LSBOX.Location = New System.Drawing.Point(20, 228)
        Me.LSBOX.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.LSBOX.MaximumSize = New System.Drawing.Size(374, 367)
        Me.LSBOX.Name = "LSBOX"
        Me.LSBOX.Size = New System.Drawing.Size(362, 274)
        Me.LSBOX.TabIndex = 6
        '
        'BtnNew
        '
        Me.BtnNew.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(183, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.BtnNew.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnNew.Appearance.Options.UseBackColor = True
        Me.BtnNew.Appearance.Options.UseForeColor = True
        Me.BtnNew.Appearance.Options.UseTextOptions = True
        Me.BtnNew.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnNew.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnNew.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnNew.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnNew.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnNew.AppearanceHovered.Options.UseForeColor = True
        Me.BtnNew.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnNew.AppearancePressed.Options.UseForeColor = True
        Me.BtnNew.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnNew.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.refresh
        Me.BtnNew.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnNew.Location = New System.Drawing.Point(284, 525)
        Me.BtnNew.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(98, 44)
        Me.BtnNew.StyleController = Me.LayoutControl1
        Me.BtnNew.TabIndex = 2
        Me.BtnNew.Text = "جديد"
        '
        'BtnSave
        '
        Me.BtnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(72, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(98, Byte), Integer))
        Me.BtnSave.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnSave.Appearance.Options.UseBackColor = True
        Me.BtnSave.Appearance.Options.UseForeColor = True
        Me.BtnSave.Appearance.Options.UseTextOptions = True
        Me.BtnSave.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnSave.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnSave.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnSave.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnSave.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnSave.AppearanceHovered.Options.UseForeColor = True
        Me.BtnSave.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnSave.AppearancePressed.Options.UseForeColor = True
        Me.BtnSave.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnSave.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.save
        Me.BtnSave.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnSave.Location = New System.Drawing.Point(152, 525)
        Me.BtnSave.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(124, 44)
        Me.BtnSave.StyleController = Me.LayoutControl1
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "حفظ"
        '
        'BtnEdit
        '
        Me.BtnEdit.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.BtnEdit.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.Appearance.Options.UseBackColor = True
        Me.BtnEdit.Appearance.Options.UseForeColor = True
        Me.BtnEdit.Appearance.Options.UseTextOptions = True
        Me.BtnEdit.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnEdit.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnEdit.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnEdit.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.AppearanceHovered.Options.UseForeColor = True
        Me.BtnEdit.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.AppearancePressed.Options.UseForeColor = True
        Me.BtnEdit.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnEdit.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.editbtn
        Me.BtnEdit.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnEdit.Location = New System.Drawing.Point(20, 525)
        Me.BtnEdit.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnEdit.MaximumSize = New System.Drawing.Size(0, 59)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(124, 44)
        Me.BtnEdit.StyleController = Me.LayoutControl1
        Me.BtnEdit.TabIndex = 0
        Me.BtnEdit.Text = "تعديل"
        '
        'AssValue
        '
        Me.AssValue.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.AssValue.Location = New System.Drawing.Point(20, 174)
        Me.AssValue.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.AssValue.Name = "AssValue"
        Me.AssValue.Properties.Appearance.Options.UseTextOptions = True
        Me.AssValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AssValue.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AssValue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.AssValue.Properties.MaskSettings.Set("mask", "n")
        Me.AssValue.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.AssValue.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.AssValue.Properties.UseMaskAsDisplayFormat = True
        Me.AssValue.Size = New System.Drawing.Size(258, 46)
        Me.AssValue.StyleController = Me.LayoutControl1
        Me.AssValue.TabIndex = 7
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem11, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem8})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(402, 589)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(370, 54)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BANKNAME
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 100)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(370, 54)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.LSBOX
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 208)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(370, 297)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.BtnNew
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(264, 505)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(106, 52)
        Me.LayoutControlItem4.Text = "LayoutControlItem2"
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.BtnSave
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(132, 505)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(132, 52)
        Me.LayoutControlItem11.Text = "LayoutControlItem1"
        Me.LayoutControlItem11.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem5.Control = Me.BtnEdit
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 505)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(132, 52)
        Me.LayoutControlItem5.Text = "LayoutControlItem4"
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.AssValue
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 154)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(370, 54)
        Me.LayoutControlItem6.Text = "قيمة الاشتراك"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.Options.UseTextOptions = True
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.Control = Me.IsActiveTG
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 54)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(370, 46)
        Me.LayoutControlItem8.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 111, 0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'FRMADDASSOCIATION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 27.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(402, 589)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = CType(resources.GetObject("FRMADDASSOCIATION.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "FRMADDASSOCIATION"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إضافة جمعية"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BANKNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AssValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents BANKNAME As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LSBOX As ListBox
    Friend WithEvents BtnNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AssValue As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class
