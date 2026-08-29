<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMEMPDISCOUNT
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
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.DISVAL = New DevExpress.XtraEditors.SpinEdit()
        Me.DISTYPEID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CURRENCYID = New DevExpress.XtraEditors.LookUpEdit()
        Me.BRANCHID = New DevExpress.XtraEditors.LookUpEdit()
        Me.EMPID = New DevExpress.XtraEditors.LookUpEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.Notes = New DevExpress.XtraEditors.MemoEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DISVAL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DISTYPEID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CURRENCYID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BRANCHID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.DISVAL)
        Me.LayoutControl1.Controls.Add(Me.DISTYPEID)
        Me.LayoutControl1.Controls.Add(Me.CURRENCYID)
        Me.LayoutControl1.Controls.Add(Me.BRANCHID)
        Me.LayoutControl1.Controls.Add(Me.EMPID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsFocus.EnableAutoTabOrder = False
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(694, 321)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(107, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(493, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 5
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(16, 100)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(254, 36)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 4
        '
        'DISVAL
        '
        Me.DISVAL.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.DISVAL.Location = New System.Drawing.Point(16, 142)
        Me.DISVAL.Name = "DISVAL"
        Me.DISVAL.Properties.AllowMouseWheel = False
        Me.DISVAL.Properties.Appearance.Options.UseTextOptions = True
        Me.DISVAL.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DISVAL.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DISVAL.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.DISVAL.Properties.MaskSettings.Set("mask", "n0")
        Me.DISVAL.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.DISVAL.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.DISVAL.Properties.UseMaskAsDisplayFormat = True
        Me.DISVAL.Size = New System.Drawing.Size(251, 36)
        Me.DISVAL.StyleController = Me.LayoutControl1
        Me.DISVAL.TabIndex = 2
        '
        'DISTYPEID
        '
        Me.DISTYPEID.Location = New System.Drawing.Point(351, 142)
        Me.DISTYPEID.Name = "DISTYPEID"
        Me.DISTYPEID.Properties.Appearance.Options.UseTextOptions = True
        Me.DISTYPEID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DISTYPEID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DISTYPEID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DISTYPEID.Properties.NullText = ""
        Me.DISTYPEID.Size = New System.Drawing.Size(249, 36)
        Me.DISTYPEID.StyleController = Me.LayoutControl1
        Me.DISTYPEID.TabIndex = 1
        Me.DISTYPEID.Visible = False
        '
        'CURRENCYID
        '
        Me.CURRENCYID.Location = New System.Drawing.Point(16, 58)
        Me.CURRENCYID.Name = "CURRENCYID"
        Me.CURRENCYID.Properties.Appearance.Options.UseTextOptions = True
        Me.CURRENCYID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CURRENCYID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CURRENCYID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CURRENCYID.Properties.NullText = ""
        Me.CURRENCYID.Size = New System.Drawing.Size(251, 36)
        Me.CURRENCYID.StyleController = Me.LayoutControl1
        Me.CURRENCYID.TabIndex = 0
        '
        'BRANCHID
        '
        Me.BRANCHID.Location = New System.Drawing.Point(351, 58)
        Me.BRANCHID.Name = "BRANCHID"
        Me.BRANCHID.Properties.Appearance.Options.UseTextOptions = True
        Me.BRANCHID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BRANCHID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BRANCHID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BRANCHID.Properties.NullText = ""
        Me.BRANCHID.Size = New System.Drawing.Size(249, 36)
        Me.BRANCHID.StyleController = Me.LayoutControl1
        Me.BRANCHID.TabIndex = 0
        '
        'EMPID
        '
        Me.EMPID.Location = New System.Drawing.Point(351, 100)
        Me.EMPID.Name = "EMPID"
        Me.EMPID.Properties.Appearance.Options.UseTextOptions = True
        Me.EMPID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.EMPID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.EMPID.Properties.NullText = ""
        Me.EMPID.Size = New System.Drawing.Size(249, 36)
        Me.EMPID.StyleController = Me.LayoutControl1
        Me.EMPID.TabIndex = 0
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(16, 16)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(85, 34)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(16, 184)
        Me.Notes.Name = "Notes"
        Me.Notes.Size = New System.Drawing.Size(584, 121)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 6
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem12, Me.LayoutControlItem3, Me.LayoutControlItem8, Me.LayoutControlItem5, Me.LayoutControlItem4, Me.LayoutControlItem6, Me.LayoutControlItem1, Me.LayoutControlItem7})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(694, 321)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.DISTYPEID
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(335, 126)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(333, 42)
        Me.LayoutControlItem2.Text = "نوع الخصم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(62, 22)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.DISVAL
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(335, 42)
        Me.LayoutControlItem12.Text = "قيمة الخصم"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(62, 22)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.EMPID
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem3.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(335, 84)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(333, 42)
        Me.LayoutControlItem3.Text = "الموظف"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(62, 22)
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 78, 3, 3)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(335, 42)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.BRANCHID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem5.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(335, 42)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(333, 42)
        Me.LayoutControlItem5.Text = "الفرع"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(62, 22)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.CURRENCYID
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem4.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(335, 42)
        Me.LayoutControlItem4.Text = "العملة"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(62, 22)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SimpleButton11
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(91, 42)
        Me.LayoutControlItem6.Text = "LayoutControlItem4"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(91, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(577, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(62, 22)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.Notes
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 168)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(668, 127)
        Me.LayoutControlItem7.Text = "ملاحظات"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(62, 22)
        '
        'FRMEMPDISCOUNT
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(694, 365)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.discount
        Me.Margin = New System.Windows.Forms.Padding(4, 7, 4, 7)
        Me.MaximizeBox = False
        Me.Name = "FRMEMPDISCOUNT"
        Me.Tag = "45"
        Me.Text = "خصم على موظف"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DISVAL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DISTYPEID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CURRENCYID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BRANCHID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents DISVAL As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents DISTYPEID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CURRENCYID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BRANCHID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EMPID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Notes As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
End Class
