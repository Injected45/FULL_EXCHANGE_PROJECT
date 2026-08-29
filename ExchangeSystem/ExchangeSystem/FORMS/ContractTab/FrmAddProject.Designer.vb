<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddProject
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAddProject))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.IsActive = New DevExpress.XtraEditors.ToggleSwitch()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.PictureEdit11 = New DevExpress.XtraEditors.PictureEdit()
        Me.ProName = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.Projectvalue = New DevExpress.XtraEditors.SpinEdit()
        Me.LCI2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Projectvalue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCI2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.IsActive)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.PictureEdit11)
        Me.LayoutControl1.Controls.Add(Me.ProName)
        Me.LayoutControl1.Controls.Add(Me.Projectvalue)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsFocus.EnableAutoTabOrder = False
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(564, 202)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(272, 17)
        Me.Code.Name = "Code"
        Me.Code.Size = New System.Drawing.Size(197, 36)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 4
        '
        'IsActive
        '
        Me.IsActive.EditValue = True
        Me.IsActive.Location = New System.Drawing.Point(16, 22)
        Me.IsActive.Name = "IsActive"
        Me.IsActive.Properties.OffText = "غير نشط"
        Me.IsActive.Properties.OnText = "نشط"
        Me.IsActive.Size = New System.Drawing.Size(200, 25)
        Me.IsActive.StyleController = Me.LayoutControl1
        Me.IsActive.TabIndex = 3
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(16, 101)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(453, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 1
        '
        'PictureEdit11
        '
        Me.PictureEdit11.EditValue = Global.ExchangeSystem.My.Resources.Resources.searchn
        Me.PictureEdit11.Location = New System.Drawing.Point(222, 17)
        Me.PictureEdit11.Name = "PictureEdit11"
        Me.PictureEdit11.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit11.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit11.Properties.InitialImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.searchn
        Me.PictureEdit11.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit11.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.PictureEdit11.Properties.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.PictureEdit11.Size = New System.Drawing.Size(44, 36)
        Me.PictureEdit11.StyleController = Me.LayoutControl1
        Me.PictureEdit11.TabIndex = 2
        '
        'ProName
        '
        Me.ProName.Location = New System.Drawing.Point(16, 59)
        Me.ProName.Name = "ProName"
        Me.ProName.Size = New System.Drawing.Size(453, 36)
        Me.ProName.StyleController = Me.LayoutControl1
        Me.ProName.TabIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem21, Me.LayoutControlItem16, Me.LCI2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(564, 202)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(256, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(282, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(63, 21)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.Control = Me.IsActive
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(206, 42)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.ProName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(538, 42)
        Me.LayoutControlItem2.Text = "اسم المشروع"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(63, 21)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(538, 42)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(63, 21)
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.PictureEdit11
        Me.LayoutControlItem16.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem16.CustomizationFormText = "LayoutControlItem16"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(206, 0)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(50, 42)
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem16.TextVisible = False
        '
        'Projectvalue
        '
        Me.Projectvalue.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Projectvalue.Location = New System.Drawing.Point(16, 144)
        Me.Projectvalue.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Projectvalue.Name = "Projectvalue"
        Me.Projectvalue.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.Projectvalue.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.Projectvalue.Properties.Appearance.ForeColor = System.Drawing.Color.White
        Me.Projectvalue.Properties.Appearance.Options.UseBackColor = True
        Me.Projectvalue.Properties.Appearance.Options.UseFont = True
        Me.Projectvalue.Properties.Appearance.Options.UseForeColor = True
        Me.Projectvalue.Properties.Appearance.Options.UseTextOptions = True
        Me.Projectvalue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Projectvalue.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Projectvalue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinRight, "", -1, False, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.Projectvalue.Properties.MaskSettings.Set("mask", "n3")
        Me.Projectvalue.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.Projectvalue.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.Projectvalue.Properties.UseMaskAsDisplayFormat = True
        Me.Projectvalue.Size = New System.Drawing.Size(453, 40)
        Me.Projectvalue.StyleController = Me.LayoutControl1
        Me.Projectvalue.TabIndex = 10
        '
        'LCI2
        '
        Me.LCI2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LCI2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LCI2.Control = Me.Projectvalue
        Me.LCI2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCI2.CustomizationFormText = "الرصيد الكلي"
        Me.LCI2.Location = New System.Drawing.Point(0, 126)
        Me.LCI2.Name = "LCI2"
        Me.LCI2.Size = New System.Drawing.Size(538, 48)
        Me.LCI2.Text = "الصافي"
        Me.LCI2.TextSize = New System.Drawing.Size(63, 21)
        '
        'FrmAddProject
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(564, 245)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FrmAddProject.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4, 7, 4, 7)
        Me.Name = "FrmAddProject"
        Me.Text = "إضافة مشروع"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Projectvalue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCI2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActive As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents PictureEdit11 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents ProName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Projectvalue As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LCI2 As DevExpress.XtraLayout.LayoutControlItem
End Class
