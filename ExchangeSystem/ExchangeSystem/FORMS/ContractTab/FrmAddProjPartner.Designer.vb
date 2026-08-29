<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddProPartner
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAddProPartner))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.IsActive = New DevExpress.XtraEditors.ToggleSwitch()
        Me.ProName = New DevExpress.XtraEditors.TextEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.PictureEdit11 = New DevExpress.XtraEditors.PictureEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.IsPartner = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsPartner.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.IsActive)
        Me.LayoutControl1.Controls.Add(Me.ProName)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.PictureEdit11)
        Me.LayoutControl1.Controls.Add(Me.IsPartner)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(623, 153)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(316, 16)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Size = New System.Drawing.Size(224, 36)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 4
        '
        'IsActive
        '
        Me.IsActive.EditValue = True
        Me.IsActive.Location = New System.Drawing.Point(16, 21)
        Me.IsActive.Name = "IsActive"
        Me.IsActive.Properties.OffText = "غير نشط"
        Me.IsActive.Properties.OnText = "نشط"
        Me.IsActive.Size = New System.Drawing.Size(236, 25)
        Me.IsActive.StyleController = Me.LayoutControl1
        Me.IsActive.TabIndex = 3
        '
        'ProName
        '
        Me.ProName.Location = New System.Drawing.Point(16, 58)
        Me.ProName.Name = "ProName"
        Me.ProName.Properties.Appearance.Options.UseTextOptions = True
        Me.ProName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ProName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ProName.Size = New System.Drawing.Size(524, 36)
        Me.ProName.StyleController = Me.LayoutControl1
        Me.ProName.TabIndex = 0
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(314, 100)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(226, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 1
        '
        'PictureEdit11
        '
        Me.PictureEdit11.EditValue = Global.ExchangeSystem.My.Resources.Resources.searchn
        Me.PictureEdit11.Location = New System.Drawing.Point(258, 16)
        Me.PictureEdit11.Name = "PictureEdit11"
        Me.PictureEdit11.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit11.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit11.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit11.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.PictureEdit11.Properties.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.PictureEdit11.Size = New System.Drawing.Size(52, 36)
        Me.PictureEdit11.StyleController = Me.LayoutControl1
        Me.PictureEdit11.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem21, Me.LayoutControlItem16, Me.LayoutControlItem3})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(623, 153)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(300, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(297, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(51, 21)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.Control = Me.IsActive
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(242, 42)
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
        Me.LayoutControlItem2.Size = New System.Drawing.Size(597, 42)
        Me.LayoutControlItem2.Text = "اسم الأصل"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(51, 21)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(298, 84)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(299, 43)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(51, 21)
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.PictureEdit11
        Me.LayoutControlItem16.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem16.CustomizationFormText = "LayoutControlItem16"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(242, 0)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(58, 42)
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem16.TextVisible = False
        '
        'IsPartner
        '
        Me.IsPartner.Location = New System.Drawing.Point(16, 100)
        Me.IsPartner.Name = "IsPartner"
        Me.IsPartner.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.IsPartner.Properties.Items.AddRange(New Object() {"شريك", "عادي"})
        Me.IsPartner.Size = New System.Drawing.Size(225, 36)
        Me.IsPartner.StyleController = Me.LayoutControl1
        Me.IsPartner.TabIndex = 5
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.IsPartner
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(298, 43)
        Me.LayoutControlItem3.Text = "النوع"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(51, 21)
        '
        'FrmAddProjPartner
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 196)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FrmAddProjPartner.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Name = "FrmAddProjPartner"
        Me.Text = "إضافة شريك مشروع"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsPartner.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActive As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents ProName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents PictureEdit11 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents IsPartner As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
