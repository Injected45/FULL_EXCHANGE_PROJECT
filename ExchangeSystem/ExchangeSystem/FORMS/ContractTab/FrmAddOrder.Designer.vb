<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddOrder
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
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.OrderName = New DevExpress.XtraEditors.TextEdit()
        Me.OrderType = New DevExpress.XtraEditors.LookUpEdit()
        Me.PictureEdit11 = New DevExpress.XtraEditors.PictureEdit()
        Me.Phone = New DevExpress.XtraEditors.SpinEdit()
        Me.Notes = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OrderName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OrderType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Phone.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.OrderName)
        Me.LayoutControl1.Controls.Add(Me.OrderType)
        Me.LayoutControl1.Controls.Add(Me.PictureEdit11)
        Me.LayoutControl1.Controls.Add(Me.Phone)
        Me.LayoutControl1.Controls.Add(Me.Notes)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsFocus.EnableAutoTabOrder = False
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(659, 211)
        Me.LayoutControl1.TabIndex = 5
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(121, 16)
        Me.Code.Name = "Code"
        Me.Code.Properties.AllowFocused = False
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Size = New System.Drawing.Size(448, 36)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 0
        '
        'OrderName
        '
        Me.OrderName.Location = New System.Drawing.Point(314, 100)
        Me.OrderName.Name = "OrderName"
        Me.OrderName.Properties.Appearance.Options.UseTextOptions = True
        Me.OrderName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OrderName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OrderName.Size = New System.Drawing.Size(255, 36)
        Me.OrderName.StyleController = Me.LayoutControl1
        Me.OrderName.TabIndex = 2
        '
        'OrderType
        '
        Me.OrderType.Location = New System.Drawing.Point(16, 58)
        Me.OrderType.Name = "OrderType"
        Me.OrderType.Properties.Appearance.Options.UseTextOptions = True
        Me.OrderType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OrderType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OrderType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.OrderType.Properties.NullText = ""
        Me.OrderType.Properties.PopupSizeable = False
        Me.OrderType.Size = New System.Drawing.Size(553, 36)
        Me.OrderType.StyleController = Me.LayoutControl1
        Me.OrderType.TabIndex = 1
        '
        'PictureEdit11
        '
        Me.PictureEdit11.EditValue = Global.ExchangeSystem.My.Resources.Resources.searchn
        Me.PictureEdit11.Location = New System.Drawing.Point(16, 16)
        Me.PictureEdit11.Name = "PictureEdit11"
        Me.PictureEdit11.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit11.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit11.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit11.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.PictureEdit11.Properties.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.PictureEdit11.Size = New System.Drawing.Size(99, 36)
        Me.PictureEdit11.StyleController = Me.LayoutControl1
        Me.PictureEdit11.TabIndex = 2
        '
        'Phone
        '
        Me.Phone.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Phone.Location = New System.Drawing.Point(16, 100)
        Me.Phone.Name = "Phone"
        Me.Phone.Properties.Appearance.Options.UseTextOptions = True
        Me.Phone.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Phone.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Phone.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.Phone.Properties.MaskSettings.Set("mask", "d")
        Me.Phone.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.Phone.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.Phone.Properties.UseMaskAsDisplayFormat = True
        Me.Phone.Size = New System.Drawing.Size(218, 36)
        Me.Phone.StyleController = Me.LayoutControl1
        Me.Phone.TabIndex = 3
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(16, 142)
        Me.Notes.Name = "Notes"
        Me.Notes.Properties.Appearance.Options.UseTextOptions = True
        Me.Notes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Notes.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Notes.Size = New System.Drawing.Size(553, 36)
        Me.Notes.StyleController = Me.LayoutControl1
        Me.Notes.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem16, Me.LayoutControlItem3, Me.LayoutControlItem21, Me.LayoutControlItem4})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(659, 211)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(105, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(528, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.OrderName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(298, 84)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(335, 42)
        Me.LayoutControlItem2.Text = "اسم الطلبية"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.PictureEdit11
        Me.LayoutControlItem16.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem16.CustomizationFormText = "LayoutControlItem16"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(105, 42)
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem16.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.Phone
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(298, 42)
        Me.LayoutControlItem3.Text = "رقم الهاتف"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.OrderType
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(633, 42)
        Me.LayoutControlItem21.Text = "نوع الطلبية"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.Notes
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(633, 59)
        Me.LayoutControlItem4.Text = "الملاحظاات"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(58, 21)
        '
        'FrmAddOrder
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 254)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "FrmAddOrder"
        Me.Text = "إضافة طلبية"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OrderName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OrderType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Phone.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents OrderName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents OrderType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents PictureEdit11 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents Phone As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Notes As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
End Class
