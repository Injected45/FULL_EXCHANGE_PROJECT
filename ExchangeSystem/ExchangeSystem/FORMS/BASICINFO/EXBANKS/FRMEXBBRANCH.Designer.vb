<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMEXBBRANCH
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.LSBOX = New DevExpress.XtraEditors.ListBoxControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.BranchName = New DevExpress.XtraEditors.TextEdit()
        Me.BtnNew = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnEdit = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnSave = New DevExpress.XtraEditors.SimpleButton()
        Me.BankID = New DevExpress.XtraEditors.LookUpEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.BranchN = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LSBOX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BankID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.LSBOX)
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.BranchName)
        Me.LayoutControl1.Controls.Add(Me.BtnNew)
        Me.LayoutControl1.Controls.Add(Me.BtnEdit)
        Me.LayoutControl1.Controls.Add(Me.BtnSave)
        Me.LayoutControl1.Controls.Add(Me.BankID)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(716, 486)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'LSBOX
        '
        Me.LSBOX.Location = New System.Drawing.Point(16, 16)
        Me.LSBOX.Name = "LSBOX"
        Me.LSBOX.Size = New System.Drawing.Size(297, 420)
        Me.LSBOX.StyleController = Me.LayoutControl1
        Me.LSBOX.TabIndex = 9
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(442, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(205, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'BranchName
        '
        Me.BranchName.Location = New System.Drawing.Point(319, 100)
        Me.BranchName.Name = "BranchName"
        Me.BranchName.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchName.Size = New System.Drawing.Size(328, 36)
        Me.BranchName.StyleController = Me.LayoutControl1
        Me.BranchName.TabIndex = 3
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
        Me.BtnNew.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnNew.Location = New System.Drawing.Point(185, 442)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(128, 28)
        Me.BtnNew.StyleController = Me.LayoutControl1
        Me.BtnNew.TabIndex = 6
        Me.BtnNew.Text = "جديد"
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
        Me.BtnEdit.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnEdit.Location = New System.Drawing.Point(16, 442)
        Me.BtnEdit.MaximumSize = New System.Drawing.Size(0, 48)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(69, 28)
        Me.BtnEdit.StyleController = Me.LayoutControl1
        Me.BtnEdit.TabIndex = 8
        Me.BtnEdit.Text = "تعديل"
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
        Me.BtnSave.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnSave.Location = New System.Drawing.Point(91, 442)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(88, 28)
        Me.BtnSave.StyleController = Me.LayoutControl1
        Me.BtnSave.TabIndex = 7
        Me.BtnSave.Text = "حفظ"
        '
        'BankID
        '
        Me.BankID.Location = New System.Drawing.Point(319, 58)
        Me.BankID.Name = "BankID"
        Me.BankID.Properties.Appearance.Options.UseTextOptions = True
        Me.BankID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BankID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources.plus
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BankID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BankID.Properties.NullText = ""
        Me.BankID.Size = New System.Drawing.Size(328, 36)
        Me.BankID.StyleController = Me.LayoutControl1
        Me.BankID.TabIndex = 2
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(319, 16)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.ContentAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(117, 36)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.BranchN, Me.LayoutControlItem10, Me.LayoutControlItem8, Me.LayoutControlItem4, Me.LayoutControlItem11, Me.LayoutControlItem5, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(716, 486)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(426, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(264, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(37, 21)
        '
        'BranchN
        '
        Me.BranchN.Control = Me.BranchName
        Me.BranchN.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.BranchN.CustomizationFormText = "اسم المخزن"
        Me.BranchN.Location = New System.Drawing.Point(303, 84)
        Me.BranchN.Name = "BranchN"
        Me.BranchN.Size = New System.Drawing.Size(387, 376)
        Me.BranchN.Text = "الاسم"
        Me.BranchN.TextSize = New System.Drawing.Size(37, 21)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.BankID
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "المصرف"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(303, 42)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(387, 42)
        Me.LayoutControlItem10.Text = "المصرف"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(37, 21)
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(303, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(123, 42)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.BtnNew
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(169, 426)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(134, 34)
        Me.LayoutControlItem4.Text = "LayoutControlItem2"
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.BtnSave
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(75, 426)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(94, 34)
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
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 426)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(75, 34)
        Me.LayoutControlItem5.Text = "LayoutControlItem4"
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.LSBOX
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(303, 426)
        Me.LayoutControlItem2.TextVisible = False
        '
        'FRMEXBBRANCH
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(716, 486)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Font = New System.Drawing.Font("Hacen Tunisia", 11.25!)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMEXBBRANCH"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "16"
        Me.Text = "إضافة فرع مصرف"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LSBOX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BankID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BranchName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchN As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BankID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LSBOX As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
End Class

