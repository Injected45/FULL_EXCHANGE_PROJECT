<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMEMPWDOVERBALANCE
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.WDCode = New DevExpress.XtraEditors.TextEdit()
        Me.WithdrawalFrom = New DevExpress.XtraEditors.LookUpEdit()
        Me.Notes = New DevExpress.XtraEditors.MemoEdit()
        Me.WithdrawalDate = New DevExpress.XtraEditors.DateEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.SafeID = New DevExpress.XtraEditors.LookUpEdit()
        Me.WDValue = New DevExpress.XtraEditors.SpinEdit()
        Me.WithdrawalValue = New DevExpress.XtraEditors.SpinEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.WDCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SafeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WDValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.WDCode)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalFrom)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalDate)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.SafeID)
        Me.LayoutControl1.Controls.Add(Me.WDValue)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalValue)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(677, 334)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'WDCode
        '
        Me.WDCode.Location = New System.Drawing.Point(383, 14)
        Me.WDCode.Name = "WDCode"
        Me.WDCode.Properties.Appearance.Options.UseTextOptions = True
        Me.WDCode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WDCode.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WDCode.Size = New System.Drawing.Size(192, 36)
        Me.WDCode.StyleController = Me.LayoutControl1
        Me.WDCode.TabIndex = 0
        '
        'WithdrawalFrom
        '
        Me.WithdrawalFrom.Location = New System.Drawing.Point(311, 92)
        Me.WithdrawalFrom.Name = "WithdrawalFrom"
        Me.WithdrawalFrom.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalFrom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalFrom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalFrom.Properties.NullText = ""
        Me.WithdrawalFrom.Size = New System.Drawing.Size(264, 36)
        Me.WithdrawalFrom.StyleController = Me.LayoutControl1
        Me.WithdrawalFrom.TabIndex = 5
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(15, 168)
        Me.Notes.Name = "Notes"
        Me.Notes.Properties.Appearance.Options.UseTextOptions = True
        Me.Notes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Notes.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Notes.Size = New System.Drawing.Size(560, 152)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 7
        '
        'WithdrawalDate
        '
        Me.WithdrawalDate.EditValue = Nothing
        Me.WithdrawalDate.Location = New System.Drawing.Point(15, 14)
        Me.WithdrawalDate.Name = "WithdrawalDate"
        Me.WithdrawalDate.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalDate.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalDate.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.WithdrawalDate.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.WithdrawalDate.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.WithdrawalDate.Properties.UseMaskAsDisplayFormat = True
        Me.WithdrawalDate.Size = New System.Drawing.Size(205, 36)
        Me.WithdrawalDate.StyleController = Me.LayoutControl1
        Me.WithdrawalDate.TabIndex = 3
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(311, 54)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(264, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 4
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(312, 16)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(66, 34)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'SafeID
        '
        Me.SafeID.Location = New System.Drawing.Point(15, 54)
        Me.SafeID.Name = "SafeID"
        Me.SafeID.Properties.Appearance.Options.UseTextOptions = True
        Me.SafeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SafeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SafeID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SafeID.Properties.NullText = ""
        Me.SafeID.Size = New System.Drawing.Size(205, 36)
        Me.SafeID.StyleController = Me.LayoutControl1
        Me.SafeID.TabIndex = 5
        '
        'WDValue
        '
        Me.WDValue.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.WDValue.Location = New System.Drawing.Point(15, 130)
        Me.WDValue.Name = "WDValue"
        Me.WDValue.Properties.Appearance.Options.UseTextOptions = True
        Me.WDValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WDValue.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WDValue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WDValue.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.WDValue.Properties.MaskSettings.Set("mask", "n3")
        Me.WDValue.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.WDValue.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.WDValue.Properties.UseMaskAsDisplayFormat = True
        Me.WDValue.Size = New System.Drawing.Size(560, 36)
        Me.WDValue.StyleController = Me.LayoutControl1
        Me.WDValue.TabIndex = 6
        '
        'WithdrawalValue
        '
        Me.WithdrawalValue.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.WithdrawalValue.Location = New System.Drawing.Point(15, 92)
        Me.WithdrawalValue.Name = "WithdrawalValue"
        Me.WithdrawalValue.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalValue.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalValue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.WithdrawalValue.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.WithdrawalValue.Properties.MaskSettings.Set("mask", "n3")
        Me.WithdrawalValue.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.WithdrawalValue.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.WithdrawalValue.Properties.UseMaskAsDisplayFormat = True
        Me.WithdrawalValue.Size = New System.Drawing.Size(205, 36)
        Me.WithdrawalValue.StyleController = Me.LayoutControl1
        Me.WithdrawalValue.TabIndex = 6
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem7, Me.LayoutControlItem6, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem9})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(677, 334)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem1.Control = Me.WDCode
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرقم"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(368, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(283, 40)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem3.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem3.Control = Me.WithdrawalFrom
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(296, 78)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem3.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem3.Size = New System.Drawing.Size(355, 38)
        Me.LayoutControlItem3.Text = "الحساب"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem8.Control = Me.Notes
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "ملاحظات"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 154)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(651, 154)
        Me.LayoutControlItem8.Text = "ملاحظات"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem2.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem2.Control = Me.WithdrawalDate
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem2.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem2.Size = New System.Drawing.Size(296, 40)
        Me.LayoutControlItem2.Text = "التاريخ"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem7.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem7.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem7.Control = Me.BranchID
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(296, 40)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem7.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem7.Size = New System.Drawing.Size(355, 38)
        Me.LayoutControlItem7.Text = "الفرع"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem6.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem6.Control = Me.WithdrawalValue
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem6.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem6.Size = New System.Drawing.Size(296, 38)
        Me.LayoutControlItem6.Text = "رصيد الحساب"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton11
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(296, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(72, 40)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem5.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem5.Control = Me.WDValue
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 116)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem5.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem5.Size = New System.Drawing.Size(651, 38)
        Me.LayoutControlItem5.Text = "القيمة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem9.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem9.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem9.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem9.Control = Me.SafeID
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 40)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem9.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem9.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem9.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem9.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem9.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem9.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem9.Size = New System.Drawing.Size(296, 38)
        Me.LayoutControlItem9.Text = "الخزنة"
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(71, 25)
        '
        'FRMEMPWDOVERBALANCE
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(677, 377)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.confirm
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "FRMEMPWDOVERBALANCE"
        Me.Tag = "57"
        Me.Text = "سحب من حساب"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.WDCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SafeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WDValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents WDCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents WithdrawalFrom As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents Notes As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents WithdrawalDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SafeID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents WDValue As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents WithdrawalValue As DevExpress.XtraEditors.SpinEdit
End Class
