<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMBTNEOWNCURRENCYEDIT
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMBTNEOWNCURRENCYEDIT))
        Me.ID_CODE = New DevExpress.XtraEditors.TextEdit()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CurrencyIDFrom = New DevExpress.XtraEditors.TextEdit()
        Me.CurrencyIDTo = New DevExpress.XtraEditors.TextEdit()
        Me.BuyPrice = New DevExpress.XtraEditors.TextEdit()
        Me.SalePrice = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.PriceType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.ID_CODE.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CurrencyIDFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyIDTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BuyPrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SalePrice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PriceType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ID_CODE
        '
        Me.ID_CODE.Location = New System.Drawing.Point(49, 57)
        Me.ID_CODE.Name = "ID_CODE"
        Me.ID_CODE.Properties.AdvancedModeOptions.Label = "رمز العملة"
        Me.ID_CODE.Properties.AdvancedModeOptions.LabelAppearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ID_CODE.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.ID_CODE.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.ID_CODE.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ID_CODE.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ID_CODE.Properties.Appearance.Options.UseTextOptions = True
        Me.ID_CODE.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ID_CODE.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ID_CODE.Properties.AutoHeight = False
        Me.ID_CODE.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.ID_CODE.Size = New System.Drawing.Size(552, 57)
        Me.ID_CODE.StyleController = Me.LayoutControl1
        Me.ID_CODE.TabIndex = 0
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.ID_CODE)
        Me.LayoutControl1.Controls.Add(Me.CurrencyIDFrom)
        Me.LayoutControl1.Controls.Add(Me.CurrencyIDTo)
        Me.LayoutControl1.Controls.Add(Me.BuyPrice)
        Me.LayoutControl1.Controls.Add(Me.SalePrice)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.PriceType)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(633, 379)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CurrencyIDFrom
        '
        Me.CurrencyIDFrom.Location = New System.Drawing.Point(328, 185)
        Me.CurrencyIDFrom.Name = "CurrencyIDFrom"
        Me.CurrencyIDFrom.Properties.AdvancedModeOptions.Label = "العملة الأولى"
        Me.CurrencyIDFrom.Properties.AdvancedModeOptions.LabelAppearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrencyIDFrom.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.CurrencyIDFrom.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyIDFrom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyIDFrom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyIDFrom.Size = New System.Drawing.Size(273, 59)
        Me.CurrencyIDFrom.StyleController = Me.LayoutControl1
        Me.CurrencyIDFrom.TabIndex = 3
        '
        'CurrencyIDTo
        '
        Me.CurrencyIDTo.Location = New System.Drawing.Point(49, 185)
        Me.CurrencyIDTo.Name = "CurrencyIDTo"
        Me.CurrencyIDTo.Properties.AdvancedModeOptions.Label = "العملة الثانية"
        Me.CurrencyIDTo.Properties.AdvancedModeOptions.LabelAppearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrencyIDTo.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.CurrencyIDTo.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyIDTo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyIDTo.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyIDTo.Size = New System.Drawing.Size(273, 59)
        Me.CurrencyIDTo.StyleController = Me.LayoutControl1
        Me.CurrencyIDTo.TabIndex = 4
        '
        'BuyPrice
        '
        Me.BuyPrice.EditValue = "0.00"
        Me.BuyPrice.Location = New System.Drawing.Point(328, 250)
        Me.BuyPrice.Name = "BuyPrice"
        Me.BuyPrice.Properties.AdvancedModeOptions.Label = "سعر الشراء"
        Me.BuyPrice.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.White
        Me.BuyPrice.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.BuyPrice.Properties.Appearance.BackColor = System.Drawing.Color.Red
        Me.BuyPrice.Properties.Appearance.Options.UseBackColor = True
        Me.BuyPrice.Properties.Appearance.Options.UseTextOptions = True
        Me.BuyPrice.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BuyPrice.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BuyPrice.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        Me.BuyPrice.Properties.MaskSettings.Set("mask", "0.000000")
        Me.BuyPrice.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.BuyPrice.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.BuyPrice.Properties.UseMaskAsDisplayFormat = True
        Me.BuyPrice.Size = New System.Drawing.Size(273, 58)
        Me.BuyPrice.StyleController = Me.LayoutControl1
        Me.BuyPrice.TabIndex = 5
        '
        'SalePrice
        '
        Me.SalePrice.EditValue = "0.00"
        Me.SalePrice.Location = New System.Drawing.Point(49, 250)
        Me.SalePrice.Name = "SalePrice"
        Me.SalePrice.Properties.AdvancedModeOptions.Label = "سعر البيع"
        Me.SalePrice.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.White
        Me.SalePrice.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.SalePrice.Properties.Appearance.BackColor = System.Drawing.Color.Green
        Me.SalePrice.Properties.Appearance.Options.UseBackColor = True
        Me.SalePrice.Properties.Appearance.Options.UseTextOptions = True
        Me.SalePrice.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SalePrice.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SalePrice.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        Me.SalePrice.Properties.MaskSettings.Set("mask", "0.000000")
        Me.SalePrice.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.SalePrice.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.SalePrice.Properties.UseMaskAsDisplayFormat = True
        Me.SalePrice.Size = New System.Drawing.Size(273, 58)
        Me.SalePrice.StyleController = Me.LayoutControl1
        Me.SalePrice.TabIndex = 6
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.BackColor = System.Drawing.Color.Black
        Me.SimpleButton1.Appearance.Options.UseBackColor = True
        Me.SimpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton1.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1.Location = New System.Drawing.Point(328, 314)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Padding = New System.Windows.Forms.Padding(0, 0, 40, 0)
        Me.SimpleButton1.Size = New System.Drawing.Size(273, 38)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 7
        Me.SimpleButton1.Text = "حفظ الاعدادات"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.SimpleButton2.Appearance.Options.UseBackColor = True
        Me.SimpleButton2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton2.Location = New System.Drawing.Point(49, 314)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(273, 28)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 8
        Me.SimpleButton2.Text = "الغاء"
        '
        'PriceType
        '
        Me.PriceType.Location = New System.Drawing.Point(49, 120)
        Me.PriceType.Name = "PriceType"
        Me.PriceType.Properties.AdvancedModeOptions.Label = "نوع البيع"
        Me.PriceType.Properties.AdvancedModeOptions.LabelAppearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PriceType.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.PriceType.Properties.AdvancedModeOptions.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.[True]
        Me.PriceType.Properties.Appearance.Options.UseTextOptions = True
        Me.PriceType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PriceType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PriceType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.PriceType.Properties.Items.AddRange(New Object() {"بيع وشراء داخلي", "بيع وشراء خارجي", "تحويلات خارجية", "مصرف"})
        Me.PriceType.Size = New System.Drawing.Size(552, 59)
        Me.PriceType.StyleController = Me.LayoutControl1
        Me.PriceType.TabIndex = 8
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(616, 382)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.BackColor = System.Drawing.Color.LightGray
        Me.LayoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBackColor = True
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem6, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.LayoutControlItem5, Me.LayoutControlItem3})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(590, 358)
        Me.LayoutControlGroup1.Text = "البيانات الاساسية "
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.ID_CODE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(56, 63)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(558, 63)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.Text = "رمز العملة"
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.CurrencyIDFrom
        Me.LayoutControlItem2.Location = New System.Drawing.Point(279, 128)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(279, 65)
        Me.LayoutControlItem2.Text = "العملة الاولى"
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.BuyPrice
        Me.LayoutControlItem4.Location = New System.Drawing.Point(279, 193)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(279, 64)
        Me.LayoutControlItem4.Text = "سعر الشراء"
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SimpleButton1
        Me.LayoutControlItem6.Location = New System.Drawing.Point(279, 257)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(279, 44)
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.SimpleButton2
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 257)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(279, 44)
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.PriceType
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 63)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(558, 65)
        Me.LayoutControlItem8.Text = "نوع البيع"
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.SalePrice
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 193)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(279, 64)
        Me.LayoutControlItem5.Text = "سعر البيع"
        Me.LayoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem5.TextToControlDistance = 0
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.CurrencyIDTo
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 128)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(279, 65)
        Me.LayoutControlItem3.Text = "العملة الثانية"
        Me.LayoutControlItem3.TextVisible = False
        '
        'FRMBTNEOWNCURRENCYEDIT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(633, 379)
        Me.Controls.Add(Me.LayoutControl1)
        Me.MaximizeBox = False
        Me.Name = "FRMBTNEOWNCURRENCYEDIT"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "تعديل في أسعار العملة "
        CType(Me.ID_CODE.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CurrencyIDFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyIDTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BuyPrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SalePrice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PriceType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CurrencyIDFrom As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CurrencyIDTo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BuyPrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents SalePrice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PriceType As DevExpress.XtraEditors.ComboBoxEdit
    Public WithEvents ID_CODE As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Public WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
