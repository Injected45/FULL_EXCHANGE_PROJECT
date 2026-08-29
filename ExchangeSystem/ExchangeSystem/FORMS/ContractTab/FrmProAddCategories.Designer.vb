<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProAddCategories
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmProAddCategories))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.ItemID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CountryID = New DevExpress.XtraEditors.LookUpEdit()
        Me.ITMDescription = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ItemID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ITMDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.ItemID)
        Me.LayoutControl1.Controls.Add(Me.CountryID)
        Me.LayoutControl1.Controls.Add(Me.ITMDescription)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(514, 209)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(214, 15)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(234, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.searchdata
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(139, 17)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(70, 34)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'ItemID
        '
        Me.ItemID.Location = New System.Drawing.Point(259, 55)
        Me.ItemID.Name = "ItemID"
        Me.ItemID.Properties.Appearance.Options.UseTextOptions = True
        Me.ItemID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ItemID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ItemID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ItemID.Properties.NullText = ""
        Me.ItemID.Size = New System.Drawing.Size(189, 36)
        Me.ItemID.StyleController = Me.LayoutControl1
        Me.ItemID.TabIndex = 4
        '
        'CountryID
        '
        Me.CountryID.Location = New System.Drawing.Point(15, 55)
        Me.CountryID.Name = "CountryID"
        Me.CountryID.Properties.Appearance.Options.UseTextOptions = True
        Me.CountryID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CountryID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CountryID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CountryID.Properties.NullText = ""
        Me.CountryID.Size = New System.Drawing.Size(189, 36)
        Me.CountryID.StyleController = Me.LayoutControl1
        Me.CountryID.TabIndex = 4
        '
        'ITMDescription
        '
        Me.ITMDescription.Location = New System.Drawing.Point(16, 137)
        Me.ITMDescription.Name = "ITMDescription"
        Me.ITMDescription.Size = New System.Drawing.Size(431, 36)
        Me.ITMDescription.StyleController = Me.LayoutControl1
        Me.ITMDescription.TabIndex = 19
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(16, 17)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.ContentAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(117, 34)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 2
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(16, 95)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(431, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 1
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem4, Me.LayoutControlItem7, Me.LayoutControlItem21, Me.LayoutControlItem8, Me.LayoutControlItem3, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(514, 209)
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
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرقم"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(199, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem1.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(289, 40)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(35, 25)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton11
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(123, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(76, 40)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem7.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem7.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem7.Control = Me.ItemID
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(244, 40)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem7.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem7.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem7.Size = New System.Drawing.Size(244, 38)
        Me.LayoutControlItem7.Text = "المادة"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(35, 25)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.ITMDescription
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "اسم صاحب الحساب"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(0, 120)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(488, 61)
        Me.LayoutControlItem21.Text = "الوصف"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(35, 21)
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(123, 40)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.BranchID
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "الفرع"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(488, 42)
        Me.LayoutControlItem3.Text = "الفرع"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(35, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem2.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem2.Control = Me.CountryID
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "نقل من خزنة"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 40)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem2.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem2.Padding = New DevExpress.XtraLayout.Utils.Padding(2, 2, 1, 1)
        Me.LayoutControlItem2.Size = New System.Drawing.Size(244, 38)
        Me.LayoutControlItem2.Text = "الدولة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(35, 25)
        '
        'FrmProAddCategories
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(514, 252)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FrmProAddCategories.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4, 7, 4, 7)
        Me.Name = "FrmProAddCategories"
        Me.Text = "إضافة تفاصيل مادة"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ItemID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ITMDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ItemID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CountryID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ITMDescription As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
