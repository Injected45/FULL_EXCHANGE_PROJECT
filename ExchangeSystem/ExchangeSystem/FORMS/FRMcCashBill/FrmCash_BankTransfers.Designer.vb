<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCash_BankTransfers
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCash_BankTransfers))
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.WithdrawalDate = New DevExpress.XtraEditors.DateEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.AccID = New DevExpress.XtraEditors.LookUpEdit()
        Me.AccNetVal = New DevExpress.XtraEditors.TextEdit()
        Me.WDValue = New DevExpress.XtraEditors.SpinEdit()
        Me.AccountType = New DevExpress.XtraEditors.LookUpEdit()
        Me.TransType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.Notes = New DevExpress.XtraEditors.MemoEdit()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WithdrawalDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AccID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AccNetVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WDValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AccountType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.WithdrawalDate)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.AccID)
        Me.LayoutControl1.Controls.Add(Me.AccNetVal)
        Me.LayoutControl1.Controls.Add(Me.WDValue)
        Me.LayoutControl1.Controls.Add(Me.AccountType)
        Me.LayoutControl1.Controls.Add(Me.TransType)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(848, 384)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Location = New System.Drawing.Point(511, 27)
        Me.Code.Margin = New System.Windows.Forms.Padding(2)
        Me.Code.Name = "Code"
        Me.Code.Properties.AllowFocused = False
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Properties.ReadOnly = True
        Me.Code.Size = New System.Drawing.Size(223, 36)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 0
        '
        'SimpleButton11
        '
        Me.SimpleButton11.AllowFocus = False
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton11.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(422, 27)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(2)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(85, 34)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        Me.SimpleButton11.Text = "fo"
        '
        'WithdrawalDate
        '
        Me.WithdrawalDate.EditValue = Nothing
        Me.WithdrawalDate.Location = New System.Drawing.Point(27, 27)
        Me.WithdrawalDate.Margin = New System.Windows.Forms.Padding(2)
        Me.WithdrawalDate.Name = "WithdrawalDate"
        Me.WithdrawalDate.Properties.Appearance.Options.UseTextOptions = True
        Me.WithdrawalDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WithdrawalDate.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WithdrawalDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.WithdrawalDate.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.WithdrawalDate.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.WithdrawalDate.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.WithdrawalDate.Properties.ReadOnly = True
        Me.WithdrawalDate.Properties.UseMaskAsDisplayFormat = True
        Me.WithdrawalDate.Size = New System.Drawing.Size(304, 36)
        Me.WithdrawalDate.StyleController = Me.LayoutControl1
        Me.WithdrawalDate.TabIndex = 3
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(422, 67)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(2)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.BranchID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.BranchID.Size = New System.Drawing.Size(312, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 4
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(27, 67)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(2)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Size = New System.Drawing.Size(304, 36)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 8
        '
        'AccID
        '
        Me.AccID.Location = New System.Drawing.Point(426, 181)
        Me.AccID.Margin = New System.Windows.Forms.Padding(2)
        Me.AccID.Name = "AccID"
        Me.AccID.Properties.Appearance.Options.UseTextOptions = True
        Me.AccID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AccID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.AccID.Properties.NullText = ""
        Me.AccID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.AccID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.AccID.Size = New System.Drawing.Size(291, 36)
        Me.AccID.StyleController = Me.LayoutControl1
        Me.AccID.TabIndex = 5
        '
        'AccNetVal
        '
        Me.AccNetVal.EditValue = ""
        Me.AccNetVal.Location = New System.Drawing.Point(44, 181)
        Me.AccNetVal.Margin = New System.Windows.Forms.Padding(2)
        Me.AccNetVal.Name = "AccNetVal"
        Me.AccNetVal.Properties.Appearance.Options.UseTextOptions = True
        Me.AccNetVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccNetVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AccNetVal.Properties.ReadOnly = True
        Me.AccNetVal.Properties.UseMaskAsDisplayFormat = True
        Me.AccNetVal.Size = New System.Drawing.Size(291, 36)
        Me.AccNetVal.StyleController = Me.LayoutControl1
        Me.AccNetVal.TabIndex = 6
        '
        'WDValue
        '
        Me.WDValue.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.WDValue.Location = New System.Drawing.Point(44, 221)
        Me.WDValue.Margin = New System.Windows.Forms.Padding(2)
        Me.WDValue.Name = "WDValue"
        Me.WDValue.Properties.Appearance.Options.UseTextOptions = True
        Me.WDValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WDValue.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.WDValue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.WDValue.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.WDValue.Properties.MaskSettings.Set("mask", "n")
        Me.WDValue.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.WDValue.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.WDValue.Properties.UseMaskAsDisplayFormat = True
        Me.WDValue.Size = New System.Drawing.Size(673, 36)
        Me.WDValue.StyleController = Me.LayoutControl1
        Me.WDValue.TabIndex = 6
        '
        'AccountType
        '
        Me.AccountType.Location = New System.Drawing.Point(44, 141)
        Me.AccountType.Margin = New System.Windows.Forms.Padding(2)
        Me.AccountType.Name = "AccountType"
        Me.AccountType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.AccountType.Properties.NullText = ""
        Me.AccountType.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.AccountType.Properties.PopupSizeable = False
        Me.AccountType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.AccountType.Size = New System.Drawing.Size(291, 36)
        Me.AccountType.StyleController = Me.LayoutControl1
        Me.AccountType.TabIndex = 18
        '
        'TransType
        '
        Me.TransType.Location = New System.Drawing.Point(426, 141)
        Me.TransType.Margin = New System.Windows.Forms.Padding(2)
        Me.TransType.Name = "TransType"
        Me.TransType.Properties.Appearance.Options.UseTextOptions = True
        Me.TransType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TransType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TransType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TransType.Properties.Items.AddRange(New Object() {"من نقدي إلى مصرف", "من مصرف إلى نقدي"})
        Me.TransType.Properties.PopupSizeable = True
        Me.TransType.Size = New System.Drawing.Size(291, 36)
        Me.TransType.StyleController = Me.LayoutControl1
        Me.TransType.TabIndex = 5
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2, Me.LayoutControlItem8})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(848, 384)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseFont = True
        Me.LayoutControlGroup2.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlGroup2.AppearanceTabPage.Header.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceTabPage.Header.Options.UseFont = True
        Me.LayoutControlGroup2.AppearanceTabPage.HeaderActive.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceTabPage.HeaderActive.Options.UseFont = True
        Me.LayoutControlGroup2.AppearanceTabPage.HeaderDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceTabPage.HeaderDisabled.Options.UseFont = True
        Me.LayoutControlGroup2.AppearanceTabPage.HeaderHotTracked.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceTabPage.HeaderHotTracked.Options.UseFont = True
        Me.LayoutControlGroup2.AppearanceTabPage.PageClient.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.AppearanceTabPage.PageClient.Options.UseFont = True
        Me.LayoutControlGroup2.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem4, Me.LayoutControlItem2, Me.LayoutControlItem7, Me.LayoutControlItem10, Me.LayoutControlGroup4})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.OptionsItemText.TextToControlDistance = 3
        Me.LayoutControlGroup2.OptionsPrint.AppearanceGroupCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.OptionsPrint.AppearanceGroupCaption.Options.UseFont = True
        Me.LayoutControlGroup2.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlGroup2.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlGroup2.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlGroup2.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlGroup2.Padding = New DevExpress.XtraLayout.Utils.Padding(9, 9, 9, 9)
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(822, 271)
        Me.LayoutControlGroup2.Spacing = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlGroup2.Text = "LayoutControlGroup1"
        Me.LayoutControlGroup2.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem1.Control = Me.Code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرقم"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(484, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(314, 40)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem4.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem4.Control = Me.SimpleButton11
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(395, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem4.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem4.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem4.Size = New System.Drawing.Size(89, 40)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.Trimming = DevExpress.Utils.Trimming.Character
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
        Me.LayoutControlItem2.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem2.Size = New System.Drawing.Size(395, 40)
        Me.LayoutControlItem2.Text = "التاريخ"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem7.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem7.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem7.Control = Me.BranchID
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(395, 40)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem7.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem7.Size = New System.Drawing.Size(403, 40)
        Me.LayoutControlItem7.Text = "الفرع"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem10.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem10.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem10.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem10.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem10.Control = Me.CurrencyID
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "العملة"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 40)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem10.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem10.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem10.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem10.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem10.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem10.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem10.Size = New System.Drawing.Size(395, 40)
        Me.LayoutControlItem10.Text = "العملة"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlGroup4
        '
        Me.LayoutControlGroup4.AppearanceGroup.BorderColor = System.Drawing.Color.Red
        Me.LayoutControlGroup4.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup4.AppearanceGroup.Options.UseFont = True
        Me.LayoutControlGroup4.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlGroup4.AppearanceTabPage.Header.Options.UseFont = True
        Me.LayoutControlGroup4.AppearanceTabPage.HeaderActive.Options.UseFont = True
        Me.LayoutControlGroup4.AppearanceTabPage.HeaderDisabled.Options.UseFont = True
        Me.LayoutControlGroup4.AppearanceTabPage.HeaderHotTracked.Options.UseFont = True
        Me.LayoutControlGroup4.AppearanceTabPage.PageClient.Options.UseFont = True
        Me.LayoutControlGroup4.CustomizationFormText = "الجانب المدين"
        Me.LayoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.LayoutControlItem6, Me.LayoutControlItem5, Me.LayoutControlItem20, Me.LayoutControlItem11})
        Me.LayoutControlGroup4.Location = New System.Drawing.Point(0, 80)
        Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
        Me.LayoutControlGroup4.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup4.OptionsPrint.AppearanceGroupCaption.Options.UseFont = True
        Me.LayoutControlGroup4.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlGroup4.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlGroup4.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlGroup4.Padding = New DevExpress.XtraLayout.Utils.Padding(13, 13, 10, 10)
        Me.LayoutControlGroup4.Size = New System.Drawing.Size(798, 167)
        Me.LayoutControlGroup4.Spacing = New DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2)
        Me.LayoutControlGroup4.Text = "تفاصيل التحويل"
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem3.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem3.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem3.Control = Me.AccID
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(382, 40)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem3.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem3.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem3.Size = New System.Drawing.Size(382, 40)
        Me.LayoutControlItem3.Text = "الحساب"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem6.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem6.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem6.Control = Me.AccNetVal
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 40)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem6.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem6.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem6.Size = New System.Drawing.Size(382, 40)
        Me.LayoutControlItem6.Text = "رصيد الحساب"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem5.AppearanceItemCaption.TextOptions.Trimming = DevExpress.Utils.Trimming.Character
        Me.LayoutControlItem5.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem5.Control = Me.WDValue
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 80)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem5.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem5.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem5.Size = New System.Drawing.Size(764, 40)
        Me.LayoutControlItem5.Text = "القيمة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem20
        '
        Me.LayoutControlItem20.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem20.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem20.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem20.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem20.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem20.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem20.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem20.Control = Me.AccountType
        Me.LayoutControlItem20.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem20.CustomizationFormText = "المصرف"
        Me.LayoutControlItem20.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem20.Name = "LayoutControlItem20"
        Me.LayoutControlItem20.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem20.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem20.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem20.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem20.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem20.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem20.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem20.Size = New System.Drawing.Size(382, 40)
        Me.LayoutControlItem20.Text = "نوع الحساب"
        Me.LayoutControlItem20.TextSize = New System.Drawing.Size(71, 25)
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem11.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem11.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem11.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem11.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem11.Control = Me.TransType
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(382, 0)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem11.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem11.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem11.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem11.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem11.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem11.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem11.Size = New System.Drawing.Size(382, 40)
        Me.LayoutControlItem11.Text = "نوع التحويل"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(71, 25)
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(15, 286)
        Me.Notes.Margin = New System.Windows.Forms.Padding(2)
        Me.Notes.Name = "Notes"
        Me.Notes.Properties.Appearance.Options.UseTextOptions = True
        Me.Notes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Notes.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Notes.Size = New System.Drawing.Size(731, 83)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 7
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 271)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(822, 87)
        Me.LayoutControlItem8.Text = "ملاحظات"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(71, 25)
        '
        'FrmCash_BankTransfers
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(848, 427)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "FrmCash_BankTransfers"
        Me.Text = "تحويل بين النقدي والمصرف"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WithdrawalDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AccID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AccNetVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WDValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AccountType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents WithdrawalDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents AccID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents AccNetVal As DevExpress.XtraEditors.TextEdit
    Friend WithEvents WDValue As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents AccountType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents TransType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Notes As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
End Class
