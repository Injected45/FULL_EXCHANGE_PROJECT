<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmResignation
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
        Me.Code = New DevExpress.XtraEditors.ButtonEdit()
        Me.Notes = New DevExpress.XtraEditors.TextEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.EMPID = New DevExpress.XtraEditors.LookUpEdit()
        Me.InsertDate = New DevExpress.XtraEditors.DateEdit()
        Me.ResignatoinDate = New DevExpress.XtraEditors.DateEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.lay = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ResignatoinDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ResignatoinDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.EMPID)
        Me.LayoutControl1.Controls.Add(Me.InsertDate)
        Me.LayoutControl1.Controls.Add(Me.ResignatoinDate)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(987, 249)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Location = New System.Drawing.Point(563, 17)
        Me.Code.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.Code.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.Code.Properties.ReadOnly = True
        Me.Code.Size = New System.Drawing.Size(293, 46)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 11
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(22, 169)
        Me.Notes.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Notes.Name = "Notes"
        Me.Notes.Size = New System.Drawing.Size(834, 46)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 6
        '
        'BranchID
        '
        Me.BranchID.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BranchID.Location = New System.Drawing.Point(563, 117)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(293, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(22, 17)
        Me.IsActiveTG.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.ContentAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(212, 46)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 13
        '
        'EMPID
        '
        Me.EMPID.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EMPID.Location = New System.Drawing.Point(22, 117)
        Me.EMPID.Name = "EMPID"
        Me.EMPID.Properties.Appearance.Options.UseTextOptions = True
        Me.EMPID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.EMPID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.EMPID.Properties.NullText = ""
        Me.EMPID.Size = New System.Drawing.Size(424, 46)
        Me.EMPID.StyleController = Me.LayoutControl1
        Me.EMPID.TabIndex = 1
        '
        'InsertDate
        '
        Me.InsertDate.EditValue = Nothing
        Me.InsertDate.Location = New System.Drawing.Point(241, 15)
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
        Me.InsertDate.Properties.UseMaskAsDisplayFormat = True
        Me.InsertDate.Size = New System.Drawing.Size(206, 46)
        Me.InsertDate.StyleController = Me.LayoutControl1
        Me.InsertDate.TabIndex = 3
        '
        'ResignatoinDate
        '
        Me.ResignatoinDate.EditValue = Nothing
        Me.ResignatoinDate.Location = New System.Drawing.Point(21, 67)
        Me.ResignatoinDate.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ResignatoinDate.Name = "ResignatoinDate"
        Me.ResignatoinDate.Properties.Appearance.Options.UseTextOptions = True
        Me.ResignatoinDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ResignatoinDate.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ResignatoinDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ResignatoinDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ResignatoinDate.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.ResignatoinDate.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.ResignatoinDate.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.ResignatoinDate.Properties.UseMaskAsDisplayFormat = True
        Me.ResignatoinDate.Size = New System.Drawing.Size(426, 46)
        Me.ResignatoinDate.StyleController = Me.LayoutControl1
        Me.ResignatoinDate.TabIndex = 3
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem7, Me.LayoutControlItem5, Me.LayoutControlItem1, Me.LayoutControlItem10, Me.LayoutControlItem8, Me.LayoutControlItem14, Me.lay, Me.EmptySpaceItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(987, 249)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.Code
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "الرمز"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(541, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(410, 52)
        Me.LayoutControlItem7.Text = "الرمز"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(87, 27)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.Notes
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "ملاحظات"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 152)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(951, 69)
        Me.LayoutControlItem5.Text = "ملاحظات"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(87, 27)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem1.Control = Me.BranchID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الفــرع"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(541, 100)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(410, 52)
        Me.LayoutControlItem1.Text = "الفــرع"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(87, 27)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem10.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem10.AppearanceItemCaptionDisabled.Options.UseTextOptions = True
        Me.LayoutControlItem10.AppearanceItemCaptionDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem10.AppearanceItemCaptionDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem10.Control = Me.IsActiveTG
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(220, 52)
        Me.LayoutControlItem10.Text = "LayoutControlItem8"
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem8.Control = Me.EMPID
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem8.CustomizationFormText = "الفــرع"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 100)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(541, 52)
        Me.LayoutControlItem8.Text = "الموظف"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(87, 27)
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem14.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem14.Control = Me.InsertDate
        Me.LayoutControlItem14.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem14.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem14.Location = New System.Drawing.Point(220, 0)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem14.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 1, 1)
        Me.LayoutControlItem14.Size = New System.Drawing.Size(321, 52)
        Me.LayoutControlItem14.Text = "التاريخ"
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(87, 32)
        '
        'lay
        '
        Me.lay.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.lay.AppearanceItemCaption.Options.UseFont = True
        Me.lay.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.lay.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.lay.Control = Me.ResignatoinDate
        Me.lay.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.lay.CustomizationFormText = "التاريخ"
        Me.lay.Location = New System.Drawing.Point(0, 52)
        Me.lay.Name = "lay"
        Me.lay.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.lay.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.lay.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.lay.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.lay.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.lay.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.lay.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 1, 1)
        Me.lay.Size = New System.Drawing.Size(541, 48)
        Me.lay.Text = "تاريخ الإنهاء"
        Me.lay.TextSize = New System.Drawing.Size(87, 32)
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(541, 52)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(410, 48)
        '
        'FrmResignation
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(987, 302)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FrmResignation"
        Me.Text = "طلب استقالة موظف"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ResignatoinDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ResignatoinDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents Code As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents Notes As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents EMPID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents InsertDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ResignatoinDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lay As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
End Class
