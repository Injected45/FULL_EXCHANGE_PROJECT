<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMPettyCash
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.PettyCashVal = New DevExpress.XtraEditors.SpinEdit()
        Me.EMPID = New DevExpress.XtraEditors.LookUpEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.Notes = New DevExpress.XtraEditors.MemoEdit()
        Me.SafeID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.PettyCashVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SafeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.PettyCashVal)
        Me.LayoutControl1.Controls.Add(Me.EMPID)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Controls.Add(Me.SafeID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(726, 107, 650, 400)
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(974, 292)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'PettyCashVal
        '
        Me.PettyCashVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.PettyCashVal.Location = New System.Drawing.Point(490, 172)
        Me.PettyCashVal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.PettyCashVal.Name = "PettyCashVal"
        Me.PettyCashVal.Properties.Appearance.Options.UseTextOptions = True
        Me.PettyCashVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PettyCashVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PettyCashVal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.PettyCashVal.Properties.MaskSettings.Set("mask", "n0")
        Me.PettyCashVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.PettyCashVal.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.PettyCashVal.Properties.UseMaskAsDisplayFormat = True
        Me.PettyCashVal.Size = New System.Drawing.Size(363, 46)
        Me.PettyCashVal.StyleController = Me.LayoutControl1
        Me.PettyCashVal.TabIndex = 2
        '
        'EMPID
        '
        Me.EMPID.Location = New System.Drawing.Point(22, 120)
        Me.EMPID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.EMPID.Name = "EMPID"
        Me.EMPID.Properties.Appearance.Options.UseTextOptions = True
        Me.EMPID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.EMPID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.EMPID.Properties.NullText = ""
        Me.EMPID.Size = New System.Drawing.Size(361, 46)
        Me.EMPID.StyleController = Me.LayoutControl1
        Me.EMPID.TabIndex = 0
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(22, 172)
        Me.IsActiveTG.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(347, 46)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 4
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(490, 68)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(363, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(22, 68)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Size = New System.Drawing.Size(361, 46)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 0
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(22, 16)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(119, 41)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'Code
        '
        Me.Code.Location = New System.Drawing.Point(149, 16)
        Me.Code.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Size = New System.Drawing.Size(704, 46)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 5
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(22, 224)
        Me.Notes.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Notes.Name = "Notes"
        Me.Notes.Size = New System.Drawing.Size(831, 52)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 6
        '
        'SafeID
        '
        Me.SafeID.Location = New System.Drawing.Point(490, 120)
        Me.SafeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SafeID.Name = "SafeID"
        Me.SafeID.Properties.Appearance.Options.UseTextOptions = True
        Me.SafeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SafeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SafeID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SafeID.Properties.NullText = ""
        Me.SafeID.Size = New System.Drawing.Size(363, 46)
        Me.SafeID.StyleController = Me.LayoutControl1
        Me.SafeID.TabIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem12, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem4, Me.LayoutControlItem6, Me.LayoutControlItem1, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(974, 292)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.PettyCashVal
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(468, 156)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(470, 52)
        Me.LayoutControlItem12.Text = "قيمة العهدة"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(77, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.EMPID
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem3.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(468, 52)
        Me.LayoutControlItem3.Text = "الموظف"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(77, 27)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.BranchID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem5.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(468, 52)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(470, 52)
        Me.LayoutControlItem5.Text = "الفرع"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(77, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.CurrencyID
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem4.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(468, 52)
        Me.LayoutControlItem4.Text = "العملة"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(77, 27)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SimpleButton11
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(127, 52)
        Me.LayoutControlItem6.Text = "LayoutControlItem4"
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(127, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(811, 52)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(77, 27)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.Notes
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "ملاحظات"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 208)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(938, 58)
        Me.LayoutControlItem7.Text = "ملاحظات"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(77, 27)
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
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 117, 3, 3)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(468, 52)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.SafeID
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(468, 104)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(470, 52)
        Me.LayoutControlItem2.Text = "الخزنة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(77, 27)
        '
        'FRMPettyCash
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(974, 345)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.PettyCash
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FRMPettyCash"
        Me.Tag = "53"
        Me.Text = "صرف عهدة"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.PettyCashVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SafeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents PettyCashVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents EMPID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Notes As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SafeID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
End Class
