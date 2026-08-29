<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmSafeTransfer
    Inherits FrmMaster

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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.WDCode = New DevExpress.XtraEditors.TextEdit()
        Me.WithdrawalFrom = New DevExpress.XtraEditors.LookUpEdit()
        Me.WithdrawalTo = New DevExpress.XtraEditors.LookUpEdit()
        Me.Notes = New DevExpress.XtraEditors.MemoEdit()
        Me.SafeBalance = New DevExpress.XtraEditors.TextEdit()
        Me.WithdrawalDate = New DevExpress.XtraEditors.DateEdit()
        Me.WithdrawalValue = New DevExpress.XtraEditors.SpinEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.BtnView = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCI = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.WDCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SafeBalance.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCI, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.WDCode)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalFrom)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalTo)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Controls.Add(Me.SafeBalance)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalDate)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalValue)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.BtnView)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(909, 254)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'WDCode
        '
        Me.WDCode.Location = New System.Drawing.Point(504, 11)
        Me.WDCode.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.WDCode.Name = "WDCode"
        Me.WDCode.Properties.Appearance.Options.UseTextOptions = True
        Me.WDCode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WDCode.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WDCode.Size = New System.Drawing.Size(248, 36)
        Me.WDCode.StyleController = Me.LayoutControl1
        Me.WDCode.TabIndex = 0
        '
        'WithdrawalFrom
        '
        Me.WithdrawalFrom.Location = New System.Drawing.Point(438, 87)
        Me.WithdrawalFrom.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.WithdrawalFrom.Name = "WithdrawalFrom"
        Me.WithdrawalFrom.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalFrom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalFrom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalFrom.Properties.NullText = ""
        Me.WithdrawalFrom.Size = New System.Drawing.Size(314, 36)
        Me.WithdrawalFrom.StyleController = Me.LayoutControl1
        Me.WithdrawalFrom.TabIndex = 5
        '
        'WithdrawalTo
        '
        Me.WithdrawalTo.Location = New System.Drawing.Point(438, 125)
        Me.WithdrawalTo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.WithdrawalTo.Name = "WithdrawalTo"
        Me.WithdrawalTo.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalTo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalTo.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalTo.Properties.NullText = ""
        Me.WithdrawalTo.Size = New System.Drawing.Size(314, 36)
        Me.WithdrawalTo.StyleController = Me.LayoutControl1
        Me.WithdrawalTo.TabIndex = 8
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(17, 163)
        Me.Notes.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Notes.Name = "Notes"
        Me.Notes.Properties.Appearance.Options.UseTextOptions = True
        Me.Notes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Notes.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Notes.Size = New System.Drawing.Size(735, 80)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 11
        '
        'SafeBalance
        '
        Me.SafeBalance.Location = New System.Drawing.Point(17, 87)
        Me.SafeBalance.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SafeBalance.Name = "SafeBalance"
        Me.SafeBalance.Properties.Appearance.Options.UseTextOptions = True
        Me.SafeBalance.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SafeBalance.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SafeBalance.Size = New System.Drawing.Size(277, 36)
        Me.SafeBalance.StyleController = Me.LayoutControl1
        Me.SafeBalance.TabIndex = 7
        '
        'WithdrawalDate
        '
        Me.WithdrawalDate.EditValue = Nothing
        Me.WithdrawalDate.Location = New System.Drawing.Point(17, 11)
        Me.WithdrawalDate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.WithdrawalDate.Name = "WithdrawalDate"
        Me.WithdrawalDate.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalDate.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalDate.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.WithdrawalDate.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.WithdrawalDate.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.WithdrawalDate.Properties.UseMaskAsDisplayFormat = True
        Me.WithdrawalDate.Size = New System.Drawing.Size(277, 36)
        Me.WithdrawalDate.StyleController = Me.LayoutControl1
        Me.WithdrawalDate.TabIndex = 3
        '
        'WithdrawalValue
        '
        Me.WithdrawalValue.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.WithdrawalValue.Location = New System.Drawing.Point(17, 125)
        Me.WithdrawalValue.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.WithdrawalValue.Name = "WithdrawalValue"
        Me.WithdrawalValue.Properties.AllowMouseWheel = False
        Me.WithdrawalValue.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalValue.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalValue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.WithdrawalValue.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.WithdrawalValue.Properties.MaskSettings.Set("mask", "n0")
        Me.WithdrawalValue.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.WithdrawalValue.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.WithdrawalValue.Properties.UseMaskAsDisplayFormat = True
        Me.WithdrawalValue.Size = New System.Drawing.Size(277, 36)
        Me.WithdrawalValue.StyleController = Me.LayoutControl1
        Me.WithdrawalValue.TabIndex = 9
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(438, 49)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(314, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 4
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(17, 49)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Size = New System.Drawing.Size(277, 36)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 6
        '
        'BtnView
        '
        Me.BtnView.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.BtnView.Appearance.Options.UseBackColor = True
        Me.BtnView.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.BtnView.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.quiries
        Me.BtnView.ImageOptions.SvgImageSize = New System.Drawing.Size(27, 27)
        Me.BtnView.Location = New System.Drawing.Point(439, 12)
        Me.BtnView.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnView.Name = "BtnView"
        Me.BtnView.Size = New System.Drawing.Size(60, 33)
        Me.BtnView.StyleController = Me.LayoutControl1
        Me.BtnView.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem7, Me.LayoutControlItem9, Me.LayoutControlItem6, Me.LayoutControlItem4, Me.LCI})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(909, 254)
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
        Me.LayoutControlItem1.Location = New System.Drawing.Point(487, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(392, 38)
        Me.LayoutControlItem1.Text = "الرقم"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(122, 25)
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
        Me.LayoutControlItem3.Location = New System.Drawing.Point(421, 76)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem3.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem3.Size = New System.Drawing.Size(458, 38)
        Me.LayoutControlItem3.Text = "نقل من خزنة"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(122, 25)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem5.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem5.Control = Me.WithdrawalTo
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(421, 114)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem5.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem5.Size = New System.Drawing.Size(458, 38)
        Me.LayoutControlItem5.Text = "نقل إلى خزنة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(122, 25)
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 152)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(879, 82)
        Me.LayoutControlItem8.Text = "ملاحظات"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(122, 25)
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
        Me.LayoutControlItem2.Size = New System.Drawing.Size(421, 38)
        Me.LayoutControlItem2.Text = "التاريخ"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(122, 25)
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
        Me.LayoutControlItem7.Location = New System.Drawing.Point(421, 38)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem7.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem7.Size = New System.Drawing.Size(458, 38)
        Me.LayoutControlItem7.Text = "الفرع"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(122, 25)
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.BtnView
        Me.LayoutControlItem9.Location = New System.Drawing.Point(421, 0)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(66, 38)
        Me.LayoutControlItem9.TextVisible = False
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
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 114)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem6.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem6.Size = New System.Drawing.Size(421, 38)
        Me.LayoutControlItem6.Text = "القيمة المنقولة"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(122, 25)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem4.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem4.Control = Me.CurrencyID
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 38)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem4.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem4.Size = New System.Drawing.Size(421, 38)
        Me.LayoutControlItem4.Text = "العملة"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(122, 25)
        '
        'LCI
        '
        Me.LCI.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LCI.AppearanceItemCaption.Options.UseFont = True
        Me.LCI.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LCI.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LCI.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LCI.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LCI.Control = Me.SafeBalance
        Me.LCI.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCI.CustomizationFormText = "رصيد الخزنة"
        Me.LCI.Location = New System.Drawing.Point(0, 76)
        Me.LCI.Name = "LCI"
        Me.LCI.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LCI.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LCI.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LCI.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LCI.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LCI.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LCI.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LCI.Size = New System.Drawing.Size(421, 38)
        Me.LCI.Text = "رصيد الخزنة من العملة"
        Me.LCI.TextSize = New System.Drawing.Size(122, 25)
        '
        'FrmSafeTransfer
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(909, 297)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.money_cash_svgrepo_com
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "FrmSafeTransfer"
        Me.Tag = "22"
        Me.Text = "التحويل بين الخزنات"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.WDCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SafeBalance.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents WDCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents WithdrawalFrom As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents WithdrawalTo As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents Notes As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents SafeBalance As DevExpress.XtraEditors.TextEdit
    Friend WithEvents WithdrawalDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents WithdrawalValue As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LCI As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnView As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
End Class
