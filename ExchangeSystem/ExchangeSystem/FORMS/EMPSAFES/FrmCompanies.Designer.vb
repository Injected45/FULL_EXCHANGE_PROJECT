<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCompanies
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CompanyName = New DevExpress.XtraEditors.TextEdit()
        Me.TypeID = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.EMPID = New DevExpress.XtraEditors.LookUpEdit()
        Me.ParentID = New DevExpress.XtraEditors.LookUpEdit()
        Me.MangerName = New DevExpress.XtraEditors.TextEdit()
        Me.LicenseNumber = New DevExpress.XtraEditors.TextEdit()
        Me.TaxID = New DevExpress.XtraEditors.TextEdit()
        Me.TreeList1 = New DevExpress.XtraTreeList.TreeList()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CompanyName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TypeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ParentID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MangerName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LicenseNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TaxID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TreeList1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CompanyName)
        Me.LayoutControl1.Controls.Add(Me.TypeID)
        Me.LayoutControl1.Controls.Add(Me.EMPID)
        Me.LayoutControl1.Controls.Add(Me.ParentID)
        Me.LayoutControl1.Controls.Add(Me.MangerName)
        Me.LayoutControl1.Controls.Add(Me.LicenseNumber)
        Me.LayoutControl1.Controls.Add(Me.TaxID)
        Me.LayoutControl1.Controls.Add(Me.TreeList1)
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1126, 484)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CompanyName
        '
        Me.CompanyName.Location = New System.Drawing.Point(436, 72)
        Me.CompanyName.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CompanyName.Name = "CompanyName"
        Me.CompanyName.Properties.Appearance.Options.UseTextOptions = True
        Me.CompanyName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CompanyName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CompanyName.Size = New System.Drawing.Size(568, 46)
        Me.CompanyName.StyleController = Me.LayoutControl1
        Me.CompanyName.TabIndex = 2
        '
        'TypeID
        '
        Me.TypeID.Location = New System.Drawing.Point(436, 126)
        Me.TypeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TypeID.Name = "TypeID"
        Me.TypeID.Properties.Appearance.Options.UseTextOptions = True
        Me.TypeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TypeID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TypeID.Properties.Items.AddRange(New Object() {"شركة", "قسم"})
        Me.TypeID.Size = New System.Drawing.Size(568, 46)
        Me.TypeID.StyleController = Me.LayoutControl1
        Me.TypeID.TabIndex = 6
        '
        'EMPID
        '
        Me.EMPID.Location = New System.Drawing.Point(436, 234)
        Me.EMPID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.EMPID.Name = "EMPID"
        Me.EMPID.Properties.Appearance.Options.UseTextOptions = True
        Me.EMPID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.EMPID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.EMPID.Properties.NullText = ""
        Me.EMPID.Size = New System.Drawing.Size(568, 46)
        Me.EMPID.StyleController = Me.LayoutControl1
        Me.EMPID.TabIndex = 2
        '
        'ParentID
        '
        Me.ParentID.Location = New System.Drawing.Point(436, 180)
        Me.ParentID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ParentID.Name = "ParentID"
        Me.ParentID.Properties.Appearance.Options.UseTextOptions = True
        Me.ParentID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ParentID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ParentID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ParentID.Properties.NullText = ""
        Me.ParentID.Size = New System.Drawing.Size(568, 46)
        Me.ParentID.StyleController = Me.LayoutControl1
        Me.ParentID.TabIndex = 2
        '
        'MangerName
        '
        Me.MangerName.Location = New System.Drawing.Point(436, 288)
        Me.MangerName.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MangerName.Name = "MangerName"
        Me.MangerName.Properties.Appearance.Options.UseTextOptions = True
        Me.MangerName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.MangerName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.MangerName.Size = New System.Drawing.Size(568, 46)
        Me.MangerName.StyleController = Me.LayoutControl1
        Me.MangerName.TabIndex = 2
        '
        'LicenseNumber
        '
        Me.LicenseNumber.Location = New System.Drawing.Point(436, 342)
        Me.LicenseNumber.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LicenseNumber.Name = "LicenseNumber"
        Me.LicenseNumber.Properties.Appearance.Options.UseTextOptions = True
        Me.LicenseNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LicenseNumber.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LicenseNumber.Size = New System.Drawing.Size(568, 46)
        Me.LicenseNumber.StyleController = Me.LayoutControl1
        Me.LicenseNumber.TabIndex = 2
        '
        'TaxID
        '
        Me.TaxID.Location = New System.Drawing.Point(436, 396)
        Me.TaxID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TaxID.Name = "TaxID"
        Me.TaxID.Properties.Appearance.Options.UseTextOptions = True
        Me.TaxID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TaxID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TaxID.Size = New System.Drawing.Size(568, 46)
        Me.TaxID.StyleController = Me.LayoutControl1
        Me.TaxID.TabIndex = 2
        '
        'TreeList1
        '
        Me.TreeList1.FixedLineWidth = 4
        Me.TreeList1.HorzScrollStep = 6
        Me.TreeList1.Location = New System.Drawing.Point(23, 23)
        Me.TreeList1.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.TreeList1.MinWidth = 43
        Me.TreeList1.Name = "TreeList1"
        Me.TreeList1.OptionsBehavior.Editable = False
        Me.TreeList1.OptionsView.ShowColumns = False
        Me.TreeList1.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.Never
        Me.TreeList1.Size = New System.Drawing.Size(400, 438)
        Me.TreeList1.TabIndex = 17
        Me.TreeList1.TreeLevelWidth = 39
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(436, 18)
        Me.Code.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Size = New System.Drawing.Size(568, 46)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem10, Me.LayoutControlItem14, Me.LayoutControlItem1, Me.LayoutControlItem18, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem4})
        Me.Root.Name = "Root"
        Me.Root.Padding = New DevExpress.XtraLayout.Utils.Padding(14, 14, 14, 14)
        Me.Root.Size = New System.Drawing.Size(1126, 484)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.CompanyName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(418, 54)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.TypeID
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "النوع"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(418, 108)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem10.Text = "النوع"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.LicenseNumber
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(418, 324)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem4.Text = "رقم الترخيص"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem14.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem14.Control = Me.TreeList1
        Me.LayoutControlItem14.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem14.CustomizationFormText = "LayoutControlItem14"
        Me.LayoutControlItem14.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!)
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem14.Padding = New DevExpress.XtraLayout.Utils.Padding(9, 9, 9, 9)
        Me.LayoutControlItem14.Size = New System.Drawing.Size(418, 456)
        Me.LayoutControlItem14.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.ParentID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(418, 162)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem1.Text = "تابع لــ"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem18
        '
        Me.LayoutControlItem18.Control = Me.EMPID
        Me.LayoutControlItem18.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem18.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem18.Location = New System.Drawing.Point(418, 216)
        Me.LayoutControlItem18.Name = "LayoutControlItem18"
        Me.LayoutControlItem18.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem18.Text = "حساب المدير"
        Me.LayoutControlItem18.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.MangerName
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(418, 270)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem3.Text = "اسم المدير"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.TaxID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(418, 378)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(680, 78)
        Me.LayoutControlItem5.Text = "الرقم الضريبي"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.Code
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم الحساب"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(418, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(680, 54)
        Me.LayoutControlItem6.Text = "الكود"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(84, 27)
        '
        'FrmCompanies
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1126, 537)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "FrmCompanies"
        Me.Text = "إضافة شركة أو قسم"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CompanyName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TypeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EMPID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ParentID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MangerName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LicenseNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TaxID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TreeList1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CompanyName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TypeID As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EMPID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents ParentID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents MangerName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LicenseNumber As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TaxID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TreeList1 As DevExpress.XtraTreeList.TreeList
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class
