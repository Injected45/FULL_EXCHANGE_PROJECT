<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMCurrencySpeculation
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
        Me.components = New System.ComponentModel.Container()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CurrencyFrom = New DevExpress.XtraEditors.LookUpEdit()
        Me.CurrencyTo = New DevExpress.XtraEditors.LookUpEdit()
        Me.BPrice1 = New DevExpress.XtraEditors.SpinEdit()
        Me.BPrice2 = New DevExpress.XtraEditors.TextEdit()
        Me.Purchaseprice = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.BehaviorManager1 = New DevExpress.Utils.Behaviors.BehaviorManager(Me.components)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CurrencyFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BPrice1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BPrice2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Purchaseprice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CurrencyFrom)
        Me.LayoutControl1.Controls.Add(Me.CurrencyTo)
        Me.LayoutControl1.Controls.Add(Me.BPrice1)
        Me.LayoutControl1.Controls.Add(Me.BPrice2)
        Me.LayoutControl1.Controls.Add(Me.Purchaseprice)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1235, 215)
        Me.LayoutControl1.TabIndex = 5
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CurrencyFrom
        '
        Me.CurrencyFrom.Location = New System.Drawing.Point(646, 17)
        Me.CurrencyFrom.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyFrom.Name = "CurrencyFrom"
        Me.CurrencyFrom.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyFrom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyFrom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyFrom.Properties.NullText = ""
        Me.CurrencyFrom.Size = New System.Drawing.Size(408, 46)
        Me.CurrencyFrom.StyleController = Me.LayoutControl1
        Me.CurrencyFrom.TabIndex = 16
        '
        'CurrencyTo
        '
        Me.CurrencyTo.Location = New System.Drawing.Point(646, 77)
        Me.CurrencyTo.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyTo.Name = "CurrencyTo"
        Me.CurrencyTo.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyTo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyTo.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyTo.Properties.NullText = ""
        Me.CurrencyTo.Size = New System.Drawing.Size(408, 46)
        Me.CurrencyTo.StyleController = Me.LayoutControl1
        Me.CurrencyTo.TabIndex = 22
        '
        'BPrice1
        '
        Me.BPrice1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.BPrice1.Location = New System.Drawing.Point(22, 17)
        Me.BPrice1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BPrice1.Name = "BPrice1"
        Me.BPrice1.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.BPrice1.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BPrice1.Properties.Appearance.ForeColor = System.Drawing.Color.Black
        Me.BPrice1.Properties.Appearance.Options.UseBackColor = True
        Me.BPrice1.Properties.Appearance.Options.UseFont = True
        Me.BPrice1.Properties.Appearance.Options.UseForeColor = True
        Me.BPrice1.Properties.Appearance.Options.UseTextOptions = True
        Me.BPrice1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BPrice1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BPrice1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BPrice1.Properties.DisplayFormat.FormatString = "{0:N2}"
        Me.BPrice1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.BPrice1.Properties.EditFormat.FormatString = "{0:N2}"
        Me.BPrice1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.BPrice1.Properties.MaskSettings.Set("mask", "N2")
        Me.BPrice1.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.BPrice1.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.BPrice1.Properties.UseMaskAsDisplayFormat = True
        Me.BPrice1.Size = New System.Drawing.Size(457, 54)
        Me.BPrice1.StyleController = Me.LayoutControl1
        Me.BPrice1.TabIndex = 18
        '
        'BPrice2
        '
        Me.BPrice2.EditValue = "0.00"
        Me.BPrice2.Location = New System.Drawing.Point(22, 137)
        Me.BPrice2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BPrice2.Name = "BPrice2"
        Me.BPrice2.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.BPrice2.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BPrice2.Properties.Appearance.ForeColor = System.Drawing.Color.Black
        Me.BPrice2.Properties.Appearance.Options.UseBackColor = True
        Me.BPrice2.Properties.Appearance.Options.UseFont = True
        Me.BPrice2.Properties.Appearance.Options.UseForeColor = True
        Me.BPrice2.Properties.Appearance.Options.UseTextOptions = True
        Me.BPrice2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BPrice2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BPrice2.Properties.DisplayFormat.FormatString = "{0:N2}"
        Me.BPrice2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.BPrice2.Properties.EditFormat.FormatString = "{0:N2}"
        Me.BPrice2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.BPrice2.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        Me.BPrice2.Properties.MaskSettings.Set("mask", "n3")
        Me.BPrice2.Properties.ReadOnly = True
        Me.BPrice2.Size = New System.Drawing.Size(1032, 54)
        Me.BPrice2.StyleController = Me.LayoutControl1
        Me.BPrice2.TabIndex = 25
        '
        'Purchaseprice
        '
        Me.Purchaseprice.Location = New System.Drawing.Point(22, 77)
        Me.Purchaseprice.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Purchaseprice.Name = "Purchaseprice"
        Me.Purchaseprice.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Purchaseprice.Properties.Appearance.ForeColor = System.Drawing.Color.Black
        Me.Purchaseprice.Properties.Appearance.Options.UseFont = True
        Me.Purchaseprice.Properties.Appearance.Options.UseForeColor = True
        Me.Purchaseprice.Properties.Appearance.Options.UseTextOptions = True
        Me.Purchaseprice.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Purchaseprice.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Purchaseprice.Properties.DisplayFormat.FormatString = "{0:N5}"
        Me.Purchaseprice.Properties.EditFormat.FormatString = "{0:N5}"
        Me.Purchaseprice.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.Purchaseprice.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        Me.Purchaseprice.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False")
        Me.Purchaseprice.Properties.MaskSettings.Set("mask", "f")
        Me.Purchaseprice.Properties.ReadOnly = True
        Me.Purchaseprice.Size = New System.Drawing.Size(457, 54)
        Me.Purchaseprice.StyleController = Me.LayoutControl1
        Me.Purchaseprice.TabIndex = 24
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem14, Me.LayoutControlItem7, Me.LayoutControlItem11, Me.LayoutControlItem6, Me.LayoutControlItem9})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1235, 215)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem14.Control = Me.CurrencyTo
        Me.LayoutControlItem14.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem14.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem14.Location = New System.Drawing.Point(624, 60)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.Size = New System.Drawing.Size(575, 60)
        Me.LayoutControlItem14.Text = "العملة المصروفة"
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(137, 36)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem7.Control = Me.BPrice2
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 120)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(1199, 67)
        Me.LayoutControlItem7.Text = "القيمة المصروفة"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(137, 36)
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LayoutControlItem11.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem11.Control = Me.CurrencyFrom
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(624, 0)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(575, 60)
        Me.LayoutControlItem11.Text = "العملة المستلمة"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(137, 36)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem6.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem6.Control = Me.BPrice1
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "الراتب"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(624, 60)
        Me.LayoutControlItem6.Text = "القيمة المستلمة"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(137, 36)
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem9.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem9.Control = Me.Purchaseprice
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 60)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(624, 60)
        Me.LayoutControlItem9.Text = "السعر"
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(137, 36)
        '
        'FRMCurrencySpeculation
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1235, 268)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FRMCurrencySpeculation"
        Me.Text = "مضاربة عملة"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CurrencyFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BPrice1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BPrice2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Purchaseprice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents CurrencyFrom As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CurrencyTo As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BPrice1 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents BPrice2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Purchaseprice As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BehaviorManager1 As DevExpress.Utils.Behaviors.BehaviorManager
End Class
