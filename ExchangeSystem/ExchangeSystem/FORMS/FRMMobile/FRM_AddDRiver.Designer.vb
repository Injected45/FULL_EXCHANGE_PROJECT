<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_AddDRiver
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM_AddDRiver))
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.LayoutControl2 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.DriverName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.TabbedControlGroup1 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.branchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.GridControl2 = New DevExpress.XtraGrid.GridControl()
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.IDID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CodeCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.accontID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DriverNameDriverName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Phone = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CarModel = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.UName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Reg = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.created_at = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl2.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.branchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.LayoutControl2)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(596, 467)
        Me.PanelControl1.TabIndex = 0
        '
        'LayoutControl2
        '
        Me.LayoutControl2.Controls.Add(Me.GridControl1)
        Me.LayoutControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl2.Location = New System.Drawing.Point(2, 2)
        Me.LayoutControl2.Name = "LayoutControl2"
        Me.LayoutControl2.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl2.Root = Me.LayoutControlGroup1
        Me.LayoutControl2.Size = New System.Drawing.Size(592, 463)
        Me.LayoutControl2.TabIndex = 0
        Me.LayoutControl2.Text = "LayoutControl2"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(32, 63)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(528, 368)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.DriverName, Me.ID, Me.Code})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        '
        'DriverName
        '
        Me.DriverName.Caption = "اسم السائق"
        Me.DriverName.FieldName = "DriverName"
        Me.DriverName.Name = "DriverName"
        Me.DriverName.Visible = True
        Me.DriverName.VisibleIndex = 0
        '
        'ID
        '
        Me.ID.Caption = "رقم السائق"
        Me.ID.FieldName = "ID"
        Me.ID.Name = "ID"
        '
        'Code
        '
        Me.Code.Caption = "كود السائق"
        Me.Code.FieldName = "Code"
        Me.Code.Name = "Code"
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.TabbedControlGroup1})
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(592, 463)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'TabbedControlGroup1
        '
        Me.TabbedControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.TabbedControlGroup1.Name = "TabbedControlGroup1"
        Me.TabbedControlGroup1.SelectedTabPage = Me.LayoutControlGroup2
        Me.TabbedControlGroup1.Size = New System.Drawing.Size(566, 437)
        Me.TabbedControlGroup1.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(534, 374)
        Me.LayoutControlGroup2.Text = "السائقين"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(534, 374)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.LayoutControl1)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(596, 0)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(728, 467)
        Me.PanelControl2.TabIndex = 1
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.branchID)
        Me.LayoutControl1.Controls.Add(Me.GridControl2)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(2, 2)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(724, 463)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'branchID
        '
        Me.branchID.Location = New System.Drawing.Point(16, 16)
        Me.branchID.Name = "branchID"
        Me.branchID.Properties.AdvancedModeOptions.Label = "الفرع"
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.branchID.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.branchID.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.branchID.Properties.Appearance.Options.UseTextOptions = True
        Me.branchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.branchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.branchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.branchID.Properties.NullText = ""
        Me.branchID.Properties.PopupSizeable = False
        Me.branchID.Size = New System.Drawing.Size(692, 58)
        Me.branchID.StyleController = Me.LayoutControl1
        Me.branchID.TabIndex = 7
        '
        'GridControl2
        '
        Me.GridControl2.Location = New System.Drawing.Point(16, 80)
        Me.GridControl2.MainView = Me.GridView2
        Me.GridControl2.Name = "GridControl2"
        Me.GridControl2.Size = New System.Drawing.Size(692, 367)
        Me.GridControl2.TabIndex = 6
        Me.GridControl2.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView2})
        '
        'GridView2
        '
        Me.GridView2.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.IDID, Me.CodeCode, Me.accontID, Me.DriverNameDriverName, Me.Phone, Me.CarModel, Me.UName, Me.Reg, Me.created_at, Me.BName, Me.SN})
        Me.GridView2.GridControl = Me.GridControl2
        Me.GridView2.Name = "GridView2"
        Me.GridView2.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView2.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView2.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView2.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        '
        'IDID
        '
        Me.IDID.Caption = "ID"
        Me.IDID.FieldName = "ID"
        Me.IDID.Name = "IDID"
        '
        'CodeCode
        '
        Me.CodeCode.Caption = "كود السائق"
        Me.CodeCode.FieldName = "Code"
        Me.CodeCode.Name = "CodeCode"
        Me.CodeCode.Visible = True
        Me.CodeCode.VisibleIndex = 1
        Me.CodeCode.Width = 76
        '
        'accontID
        '
        Me.accontID.Caption = "رقم حساب السائق"
        Me.accontID.FieldName = "accontID"
        Me.accontID.Name = "accontID"
        Me.accontID.Width = 53
        '
        'DriverNameDriverName
        '
        Me.DriverNameDriverName.Caption = "اسم السائق"
        Me.DriverNameDriverName.FieldName = "DriverName"
        Me.DriverNameDriverName.Name = "DriverNameDriverName"
        Me.DriverNameDriverName.Visible = True
        Me.DriverNameDriverName.VisibleIndex = 2
        Me.DriverNameDriverName.Width = 155
        '
        'Phone
        '
        Me.Phone.Caption = "رقم الهاتف"
        Me.Phone.FieldName = "Phone"
        Me.Phone.Name = "Phone"
        Me.Phone.Visible = True
        Me.Phone.VisibleIndex = 3
        Me.Phone.Width = 135
        '
        'CarModel
        '
        Me.CarModel.Caption = "نوع السيارة"
        Me.CarModel.FieldName = "CarModel"
        Me.CarModel.Name = "CarModel"
        Me.CarModel.Width = 38
        '
        'UName
        '
        Me.UName.Caption = "اسم المستخدم"
        Me.UName.FieldName = "UName"
        Me.UName.Name = "UName"
        Me.UName.Visible = True
        Me.UName.VisibleIndex = 5
        Me.UName.Width = 54
        '
        'Reg
        '
        Me.Reg.Caption = "حالة الحساب في التطبيق"
        Me.Reg.FieldName = "Reg"
        Me.Reg.Name = "Reg"
        Me.Reg.Visible = True
        Me.Reg.VisibleIndex = 6
        Me.Reg.Width = 57
        '
        'created_at
        '
        Me.created_at.Caption = "تاريخ انشاء تطبيق"
        Me.created_at.FieldName = "created_at"
        Me.created_at.Name = "created_at"
        Me.created_at.Visible = True
        Me.created_at.VisibleIndex = 7
        Me.created_at.Width = 41
        '
        'BName
        '
        Me.BName.Caption = "الفرع"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 4
        Me.BName.Width = 66
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 23
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(724, 463)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.GridControl2
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 64)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(698, 373)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.branchID
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(698, 64)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRM_AddDRiver
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1324, 467)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FRM_AddDRiver.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Name = "FRM_AddDRiver"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "FRM_AddDRiver"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl2.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.branchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControl2 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents TabbedControlGroup1 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl2 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
    Friend WithEvents DriverName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents branchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents IDID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CodeCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents accontID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DriverNameDriverName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Phone As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CarModel As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents UName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Reg As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents created_at As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
End Class
