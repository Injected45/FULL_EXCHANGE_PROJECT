<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAccountLimitations
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
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.AccID = New DevExpress.XtraEditors.LookUpEdit()
        Me.LimitationVal = New DevExpress.XtraEditors.SpinEdit()
        Me.PreviewVal = New DevExpress.XtraEditors.SpinEdit()
        Me.AccountType = New DevExpress.XtraEditors.LookUpEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.AccID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LimitationVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PreviewVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AccountType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.AccID)
        Me.LayoutControl1.Controls.Add(Me.LimitationVal)
        Me.LayoutControl1.Controls.Add(Me.PreviewVal)
        Me.LayoutControl1.Controls.Add(Me.AccountType)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(706, 330)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'AccID
        '
        Me.AccID.Location = New System.Drawing.Point(20, 129)
        Me.AccID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.AccID.Name = "AccID"
        Me.AccID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.AccID.Properties.NullText = ""
        Me.AccID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.AccID.Properties.PopupSizeable = False
        Me.AccID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.AccID.Properties.SelectFirstRowOnEnterKey = DevExpress.Utils.DefaultBoolean.[True]
        Me.AccID.Size = New System.Drawing.Size(542, 46)
        Me.AccID.StyleController = Me.LayoutControl1
        Me.AccID.TabIndex = 2
        '
        'LimitationVal
        '
        Me.LimitationVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.LimitationVal.Location = New System.Drawing.Point(20, 183)
        Me.LimitationVal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LimitationVal.Name = "LimitationVal"
        Me.LimitationVal.Properties.AllowMouseWheel = False
        Me.LimitationVal.Properties.Appearance.BackColor = System.Drawing.Color.Green
        Me.LimitationVal.Properties.Appearance.ForeColor = System.Drawing.Color.Yellow
        Me.LimitationVal.Properties.Appearance.Options.UseBackColor = True
        Me.LimitationVal.Properties.Appearance.Options.UseForeColor = True
        Me.LimitationVal.Properties.Appearance.Options.UseTextOptions = True
        Me.LimitationVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LimitationVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LimitationVal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.LimitationVal.Properties.DisplayFormat.FormatString = "{0:N2}"
        Me.LimitationVal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.LimitationVal.Properties.EditFormat.FormatString = "{0:N2}"
        Me.LimitationVal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.LimitationVal.Properties.MaskSettings.Set("mask", "n")
        Me.LimitationVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.LimitationVal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.LimitationVal.Properties.UseMaskAsDisplayFormat = True
        Me.LimitationVal.Size = New System.Drawing.Size(542, 46)
        Me.LimitationVal.StyleController = Me.LayoutControl1
        Me.LimitationVal.TabIndex = 3
        '
        'PreviewVal
        '
        Me.PreviewVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.PreviewVal.Location = New System.Drawing.Point(20, 237)
        Me.PreviewVal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.PreviewVal.Name = "PreviewVal"
        Me.PreviewVal.Properties.AllowMouseWheel = False
        Me.PreviewVal.Properties.Appearance.BackColor = System.Drawing.Color.Red
        Me.PreviewVal.Properties.Appearance.ForeColor = System.Drawing.Color.Yellow
        Me.PreviewVal.Properties.Appearance.Options.UseBackColor = True
        Me.PreviewVal.Properties.Appearance.Options.UseForeColor = True
        Me.PreviewVal.Properties.Appearance.Options.UseTextOptions = True
        Me.PreviewVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PreviewVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PreviewVal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.PreviewVal.Properties.DisplayFormat.FormatString = "{0:N2}"
        Me.PreviewVal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.PreviewVal.Properties.EditFormat.FormatString = "{0:N2}"
        Me.PreviewVal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.PreviewVal.Properties.MaskSettings.Set("mask", "n")
        Me.PreviewVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.PreviewVal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.PreviewVal.Properties.ReadOnly = True
        Me.PreviewVal.Properties.UseMaskAsDisplayFormat = True
        Me.PreviewVal.Size = New System.Drawing.Size(542, 46)
        Me.PreviewVal.StyleController = Me.LayoutControl1
        Me.PreviewVal.TabIndex = 4
        '
        'AccountType
        '
        Me.AccountType.Location = New System.Drawing.Point(20, 75)
        Me.AccountType.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.AccountType.Name = "AccountType"
        Me.AccountType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.AccountType.Properties.NullText = ""
        Me.AccountType.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.AccountType.Properties.PopupSizeable = False
        Me.AccountType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.AccountType.Properties.SelectFirstRowOnEnterKey = DevExpress.Utils.DefaultBoolean.[True]
        Me.AccountType.Size = New System.Drawing.Size(542, 46)
        Me.AccountType.StyleController = Me.LayoutControl1
        Me.AccountType.TabIndex = 0
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(20, 21)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(542, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem21, Me.LayoutControlItem10, Me.LayoutControlItem6, Me.LayoutControlItem1, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(706, 330)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.AccID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(0, 108)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(674, 54)
        Me.LayoutControlItem21.Text = "الحساب"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(104, 27)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.AccountType
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "النوع"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 54)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(674, 54)
        Me.LayoutControlItem10.Text = "نوع الحساب"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(104, 27)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.LimitationVal
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "الحد الأعلى للدين"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 162)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(674, 54)
        Me.LayoutControlItem6.Text = "الحد الأعلى للدين"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(104, 27)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.PreviewVal
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الحد الأعلى للدين"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 216)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(674, 80)
        Me.LayoutControlItem1.Text = "القيمة السابقة"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(104, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BranchID
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الفرع"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(674, 54)
        Me.LayoutControlItem2.Text = "الفرع"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(104, 27)
        '
        'FrmAccountLimitations
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(706, 383)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.Name = "FrmAccountLimitations"
        Me.Text = "الحد الأعلى للدين"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.AccID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LimitationVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PreviewVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AccountType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents AccID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LimitationVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PreviewVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AccountType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
End Class
