<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmBankPortfolio
    Inherits TemplateForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmBankPortfolio))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.D1 = New DevExpress.XtraEditors.DateEdit()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.D2 = New DevExpress.XtraEditors.DateEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.OverAllTotal = New DevExpress.XtraEditors.SpinEdit()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.ContextMenuStrip11 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.BankID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem26 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem25 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TabbedControlGroup1 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TabbedControlGroup2 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip11.SuspendLayout()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BankID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.D1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.D2)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.OverAllTotal)
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Controls.Add(Me.BankID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1812, 743)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'D1
        '
        Me.D1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.D1.EditValue = New Date(2024, 8, 6, 0, 0, 0, 0)
        Me.D1.Location = New System.Drawing.Point(1331, 174)
        Me.D1.Name = "D1"
        Me.D1.Properties.Appearance.Options.UseTextOptions = True
        Me.D1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.D1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.D1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.MaskSettings.Set("mask", "M ")
        Me.D1.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.D1.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D1.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.DateOnlyMaskManager))
        Me.D1.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False")
        Me.D1.Properties.Name = "DT1"
        Me.D1.Properties.UseMaskAsDisplayFormat = True
        Me.D1.Size = New System.Drawing.Size(327, 46)
        Me.D1.StyleController = Me.LayoutControl1
        Me.D1.TabIndex = 3
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton2.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton2.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton2.Location = New System.Drawing.Point(1331, 336)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(441, 46)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 6
        Me.SimpleButton2.Text = "طباعه"
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton11.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton11.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton11.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton11.Location = New System.Drawing.Point(1331, 282)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(441, 46)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 5
        Me.SimpleButton11.Text = "عرض "
        '
        'D2
        '
        Me.D2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.D2.EditValue = New Date(2024, 8, 6, 0, 0, 0, 0)
        Me.D2.Location = New System.Drawing.Point(1331, 228)
        Me.D2.Name = "D2"
        Me.D2.Properties.Appearance.Options.UseTextOptions = True
        Me.D2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.D2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.D2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D2.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.D2.Properties.MaskSettings.Set("mask", "yyyy")
        Me.D2.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.DateOnlyMaskManager))
        Me.D2.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False")
        Me.D2.Properties.Name = "DT1"
        Me.D2.Properties.UseMaskAsDisplayFormat = True
        Me.D2.Size = New System.Drawing.Size(327, 46)
        Me.D2.StyleController = Me.LayoutControl1
        Me.D2.TabIndex = 4
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(1331, 120)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.[False]
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(327, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'OverAllTotal
        '
        Me.OverAllTotal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OverAllTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllTotal.Location = New System.Drawing.Point(40, 656)
        Me.OverAllTotal.MaximumSize = New System.Drawing.Size(0, 35)
        Me.OverAllTotal.Name = "OverAllTotal"
        Me.OverAllTotal.Properties.Appearance.BackColor = System.Drawing.Color.DodgerBlue
        Me.OverAllTotal.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OverAllTotal.Properties.Appearance.ForeColor = System.Drawing.Color.White
        Me.OverAllTotal.Properties.Appearance.Options.UseBackColor = True
        Me.OverAllTotal.Properties.Appearance.Options.UseFont = True
        Me.OverAllTotal.Properties.Appearance.Options.UseForeColor = True
        Me.OverAllTotal.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllTotal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllTotal.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.DodgerBlue
        Me.OverAllTotal.Properties.AppearanceDisabled.Options.UseBackColor = True
        Me.OverAllTotal.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.OverAllTotal.Properties.MaskSettings.Set("mask", "n3")
        Me.OverAllTotal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverAllTotal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverAllTotal.Properties.ReadOnly = True
        Me.OverAllTotal.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllTotal.Size = New System.Drawing.Size(307, 35)
        Me.OverAllTotal.StyleController = Me.LayoutControl1
        Me.OverAllTotal.TabIndex = 12
        '
        'GCROLE
        '
        Me.GCROLE.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GCROLE.ContextMenuStrip = Me.ContextMenuStrip11
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GCROLE.Location = New System.Drawing.Point(60, 138)
        Me.GCROLE.LookAndFeel.UseDefaultLookAndFeel = False
        Me.GCROLE.MainView = Me.GVRole
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.Size = New System.Drawing.Size(1203, 486)
        Me.GCROLE.TabIndex = 9
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'ContextMenuStrip11
        '
        Me.ContextMenuStrip11.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip11.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem11, Me.ToolStripMenuItem2, Me.ToolStripMenuItem3})
        Me.ContextMenuStrip11.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip11.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ContextMenuStrip11.Size = New System.Drawing.Size(352, 112)
        '
        'ToolStripMenuItem11
        '
        Me.ToolStripMenuItem11.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem11.Name = "ToolStripMenuItem11"
        Me.ToolStripMenuItem11.Size = New System.Drawing.Size(351, 36)
        Me.ToolStripMenuItem11.Text = "عرض حساب نشاط التجاري"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(351, 36)
        Me.ToolStripMenuItem2.Text = "طباعه كشف لحركة نشاط "
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem3.Image = CType(resources.GetObject("ToolStripMenuItem3.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(351, 36)
        Me.ToolStripMenuItem3.Text = "ارسال كشف حساب النشاط التجاري "
        '
        'GVRole
        '
        Me.GVRole.DetailHeight = 294
        Me.GVRole.GridControl = Me.GCROLE
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsBehavior.Editable = False
        Me.GVRole.OptionsBehavior.ReadOnly = True
        Me.GVRole.OptionsCustomization.AllowColumnMoving = False
        Me.GVRole.OptionsCustomization.AllowColumnResizing = False
        Me.GVRole.OptionsCustomization.AllowFilter = False
        Me.GVRole.OptionsCustomization.AllowGroup = False
        Me.GVRole.OptionsCustomization.AllowSort = False
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 1029
        Me.GVRole.OptionsFilter.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsFilter.AllowFilterEditor = False
        Me.GVRole.OptionsFilter.AllowMRUFilterList = False
        Me.GVRole.OptionsFilter.FilterEditorAllowCustomExpressions = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsFilter.InHeaderSearchMode = DevExpress.XtraGrid.Views.Grid.GridInHeaderSearchMode.Disabled
        Me.GVRole.OptionsFilter.ShowCustomFunctions = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsFilter.ShowInHeaderSearchResults = DevExpress.XtraGrid.Views.Grid.ShowInHeaderSearchResultsMode.None
        Me.GVRole.OptionsFind.AllowFindPanel = False
        Me.GVRole.OptionsFind.AllowMruItems = False
        Me.GVRole.OptionsFind.Behavior = DevExpress.XtraEditors.FindPanelBehavior.Filter
        Me.GVRole.OptionsFind.FindNullPrompt = "ابحث هنا ..."
        Me.GVRole.OptionsMenu.EnableColumnMenu = False
        Me.GVRole.OptionsMenu.EnableGroupPanelMenu = False
        Me.GVRole.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GVRole.OptionsSelection.EnableAppearanceHotTrackedRow = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'BankID
        '
        Me.BankID.Location = New System.Drawing.Point(1331, 66)
        Me.BankID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BankID.Name = "BankID"
        Me.BankID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BankID.Properties.NullText = ""
        Me.BankID.Properties.PopupSizeable = False
        Me.BankID.Size = New System.Drawing.Size(327, 46)
        Me.BankID.StyleController = Me.LayoutControl1
        Me.BankID.TabIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2, Me.TabbedControlGroup1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1812, 743)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup2.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup2.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlGroup2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup2.CustomizationFormText = "البيانات الرئيسية"
        Me.LayoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem11, Me.LayoutControlItem10, Me.LayoutControlItem9, Me.LayoutControlItem6, Me.LayoutControlItem26, Me.LayoutControlItem25})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(1291, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(489, 711)
        Me.LayoutControlGroup2.Text = "البيانات الرئيسية"
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.D1
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem11.CustomizationFormText = "من"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 108)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(449, 54)
        Me.LayoutControlItem11.Text = "الشهر"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(94, 27)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.SimpleButton2
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "LayoutControlItem10"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 270)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(449, 375)
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.SimpleButton11
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "LayoutControlItem9"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 216)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(449, 54)
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.D2
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem6.CustomizationFormText = "من"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 162)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(449, 54)
        Me.LayoutControlItem6.Text = "إلى تاريخ"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(94, 27)
        '
        'LayoutControlItem26
        '
        Me.LayoutControlItem26.Control = Me.BankID
        Me.LayoutControlItem26.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem26.CustomizationFormText = "المصرف"
        Me.LayoutControlItem26.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem26.Name = "LayoutControlItem26"
        Me.LayoutControlItem26.Size = New System.Drawing.Size(449, 54)
        Me.LayoutControlItem26.Text = "المصرف"
        Me.LayoutControlItem26.TextSize = New System.Drawing.Size(94, 27)
        '
        'LayoutControlItem25
        '
        Me.LayoutControlItem25.Control = Me.BranchID
        Me.LayoutControlItem25.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem25.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem25.Location = New System.Drawing.Point(0, 54)
        Me.LayoutControlItem25.Name = "LayoutControlItem25"
        Me.LayoutControlItem25.Size = New System.Drawing.Size(449, 54)
        Me.LayoutControlItem25.Text = "الفرع"
        Me.LayoutControlItem25.TextSize = New System.Drawing.Size(94, 27)
        '
        'TabbedControlGroup1
        '
        Me.TabbedControlGroup1.CustomizationFormText = "TabbedControlGroup1"
        Me.TabbedControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.TabbedControlGroup1.Name = "TabbedControlGroup1"
        Me.TabbedControlGroup1.SelectedTabPage = Me.LayoutControlGroup3
        Me.TabbedControlGroup1.Size = New System.Drawing.Size(1291, 711)
        Me.TabbedControlGroup1.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup3})
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup3.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup3.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup3.CustomizationFormText = "التفاصيل"
        Me.LayoutControlGroup3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.LayoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem13, Me.TabbedControlGroup2, Me.EmptySpaceItem1})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(1251, 632)
        Me.LayoutControlGroup3.Text = "التفاصيل"
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem13.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem13.Control = Me.OverAllTotal
        Me.LayoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem13.CustomizationFormText = "الصافي"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 573)
        Me.LayoutControlItem13.MaxSize = New System.Drawing.Size(0, 59)
        Me.LayoutControlItem13.MinSize = New System.Drawing.Size(185, 59)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(429, 59)
        Me.LayoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem13.Spacing = New DevExpress.XtraLayout.Utils.Padding(0, 0, 4, 0)
        Me.LayoutControlItem13.Text = "إجمالي الحافظة"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(94, 27)
        '
        'TabbedControlGroup2
        '
        Me.TabbedControlGroup2.CustomizationFormText = "حركة الحساب"
        Me.TabbedControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.TabbedControlGroup2.Name = "TabbedControlGroup2"
        Me.TabbedControlGroup2.SelectedTabPage = Me.LayoutControlGroup4
        Me.TabbedControlGroup2.Size = New System.Drawing.Size(1251, 573)
        Me.TabbedControlGroup2.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup4})
        Me.TabbedControlGroup2.Text = "حركة الحساب"
        '
        'LayoutControlGroup4
        '
        Me.LayoutControlGroup4.CustomizationFormText = "حركة العملة"
        Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2})
        Me.LayoutControlGroup4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
        Me.LayoutControlGroup4.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup4.Size = New System.Drawing.Size(1211, 494)
        Me.LayoutControlGroup4.Text = "الحافظة"
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GCROLE
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(1211, 494)
        Me.LayoutControlItem2.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(429, 573)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(822, 59)
        '
        'FrmBankPortfolio
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1812, 743)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmBankPortfolio.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "FrmBankPortfolio"
        Me.Text = "طباعة حافظة مصرفية"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip11.ResumeLayout(False)
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BankID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents D1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents D2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem26 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem25 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents OverAllTotal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents ContextMenuStrip11 As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem11 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents TabbedControlGroup1 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TabbedControlGroup2 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BankID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
End Class
