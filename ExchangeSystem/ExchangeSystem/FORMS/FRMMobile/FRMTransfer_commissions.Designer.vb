<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMTransfer_commissions
    'Inherits DevExpress.XtraEditors.XtraForm
    Inherits FrmMaster
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    '''
    Private Sub InitializeComponent()
        Dim WindowsUIButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMTransfer_commissions))
        Me.dataLayoutControl1 = New DevExpress.XtraDataLayout.DataLayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.First_Value = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemSpinEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.Second_value = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Commission_value = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Daily = New DevExpress.XtraEditors.SpinEdit()
        Me.Weekly = New DevExpress.XtraEditors.SpinEdit()
        Me.Annual = New DevExpress.XtraEditors.SpinEdit()
        Me.ID_TEXT = New DevExpress.XtraEditors.TextEdit()
        Me.layoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TabbedControlGroup1 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.windowsUIButtonPanelCloseButton = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        Me.labelControl = New DevExpress.XtraEditors.LabelControl()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.dataLayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.dataLayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemSpinEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Daily.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Weekly.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Annual.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ID_TEXT.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dataLayoutControl1
        '
        Me.dataLayoutControl1.AllowCustomization = False
        Me.dataLayoutControl1.Controls.Add(Me.GridControl1)
        Me.dataLayoutControl1.Controls.Add(Me.Daily)
        Me.dataLayoutControl1.Controls.Add(Me.Weekly)
        Me.dataLayoutControl1.Controls.Add(Me.Annual)
        Me.dataLayoutControl1.Controls.Add(Me.ID_TEXT)
        Me.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dataLayoutControl1.Location = New System.Drawing.Point(52, 90)
        Me.dataLayoutControl1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dataLayoutControl1.Name = "dataLayoutControl1"
        Me.dataLayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.dataLayoutControl1.Root = Me.layoutControlGroup1
        Me.dataLayoutControl1.Size = New System.Drawing.Size(931, 540)
        Me.dataLayoutControl1.TabIndex = 0
        '
        'GridControl1
        '
        Me.GridControl1.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(1, 1, 1, 1)
        Me.GridControl1.Location = New System.Drawing.Point(28, 167)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemSpinEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(875, 343)
        Me.GridControl1.TabIndex = 5
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ID, Me.First_Value, Me.Second_value, Me.Commission_value, Me.SN})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsEditForm.PopupEditFormWidth = 700
        Me.GridView1.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        '
        'ID
        '
        Me.ID.Caption = " رقم "
        Me.ID.FieldName = "ID"
        Me.ID.MinWidth = 17
        Me.ID.Name = "ID"
        Me.ID.Width = 66
        '
        'First_Value
        '
        Me.First_Value.Caption = "من"
        Me.First_Value.ColumnEdit = Me.RepositoryItemSpinEdit1
        Me.First_Value.FieldName = "First_Value"
        Me.First_Value.MinWidth = 17
        Me.First_Value.Name = "First_Value"
        Me.First_Value.Visible = True
        Me.First_Value.VisibleIndex = 1
        Me.First_Value.Width = 261
        '
        'RepositoryItemSpinEdit1
        '
        Me.RepositoryItemSpinEdit1.AutoHeight = False
        Me.RepositoryItemSpinEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemSpinEdit1.Name = "RepositoryItemSpinEdit1"
        '
        'Second_value
        '
        Me.Second_value.Caption = "الي"
        Me.Second_value.ColumnEdit = Me.RepositoryItemSpinEdit1
        Me.Second_value.FieldName = "Second_value"
        Me.Second_value.MinWidth = 17
        Me.Second_value.Name = "Second_value"
        Me.Second_value.Visible = True
        Me.Second_value.VisibleIndex = 2
        Me.Second_value.Width = 261
        '
        'Commission_value
        '
        Me.Commission_value.Caption = "العمولة"
        Me.Commission_value.FieldName = "Commission_value"
        Me.Commission_value.MinWidth = 17
        Me.Commission_value.Name = "Commission_value"
        Me.Commission_value.Visible = True
        Me.Commission_value.VisibleIndex = 3
        Me.Commission_value.Width = 262
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.MinWidth = 17
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 63
        '
        'Daily
        '
        Me.Daily.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Daily.Location = New System.Drawing.Point(638, 57)
        Me.Daily.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Daily.Name = "Daily"
        Me.Daily.Properties.AdvancedModeOptions.Label = "من "
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.Daily.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Daily.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.Daily.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.Daily.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.Daily.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.Daily.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Daily.Properties.Appearance.Options.UseTextOptions = True
        Me.Daily.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Daily.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Daily.Properties.AutoHeight = False
        Me.Daily.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Daily.Properties.DisplayFormat.FormatString = "0.00"
        Me.Daily.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.Daily.Properties.MaskSettings.Set("mask", "d")
        Me.Daily.Properties.MaxValue = New Decimal(New Integer() {-1981284352, -1966660860, 0, 0})
        Me.Daily.Properties.UseMaskAsDisplayFormat = True
        Me.Daily.Size = New System.Drawing.Size(279, 58)
        Me.Daily.StyleController = Me.dataLayoutControl1
        Me.Daily.TabIndex = 0
        '
        'Weekly
        '
        Me.Weekly.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Weekly.Location = New System.Drawing.Point(355, 57)
        Me.Weekly.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Weekly.Name = "Weekly"
        Me.Weekly.Properties.AdvancedModeOptions.Label = "الي"
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.Weekly.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.Weekly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Weekly.Properties.Appearance.Options.UseTextOptions = True
        Me.Weekly.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Weekly.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Weekly.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Weekly.Properties.EditFormat.FormatString = "0.00"
        Me.Weekly.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.Weekly.Properties.MaskSettings.Set("mask", "d")
        Me.Weekly.Properties.MaxValue = New Decimal(New Integer() {1874919424, 2328306, 0, 0})
        Me.Weekly.Properties.UseMaskAsDisplayFormat = True
        Me.Weekly.Properties.XlsxFormatString = "0.00"
        Me.Weekly.Size = New System.Drawing.Size(277, 58)
        Me.Weekly.StyleController = Me.dataLayoutControl1
        Me.Weekly.TabIndex = 2
        '
        'Annual
        '
        Me.Annual.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Annual.Location = New System.Drawing.Point(14, 57)
        Me.Annual.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Annual.Name = "Annual"
        Me.Annual.Properties.AdvancedModeOptions.Label = "عمولة تحويل"
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.Annual.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.Annual.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Annual.Properties.Appearance.Options.UseTextOptions = True
        Me.Annual.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Annual.Properties.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Show
        Me.Annual.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Annual.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Annual.Properties.EditFormat.FormatString = "0.00"
        Me.Annual.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.Annual.Properties.MaskSettings.Set("mask", "n")
        Me.Annual.Properties.MaxValue = New Decimal(New Integer() {1874919424, 2328306, 0, 0})
        Me.Annual.Properties.UseMaskAsDisplayFormat = True
        Me.Annual.Size = New System.Drawing.Size(335, 58)
        Me.Annual.StyleController = Me.dataLayoutControl1
        Me.Annual.TabIndex = 3
        '
        'ID_TEXT
        '
        Me.ID_TEXT.Location = New System.Drawing.Point(14, 15)
        Me.ID_TEXT.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ID_TEXT.Name = "ID_TEXT"
        Me.ID_TEXT.Size = New System.Drawing.Size(903, 36)
        Me.ID_TEXT.StyleController = Me.dataLayoutControl1
        Me.ID_TEXT.TabIndex = 6
        '
        'layoutControlGroup1
        '
        Me.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.layoutControlGroup1.GroupBordersVisible = False
        Me.layoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.TabbedControlGroup1, Me.LayoutControlItem5})
        Me.layoutControlGroup1.Name = "Root"
        Me.layoutControlGroup1.Size = New System.Drawing.Size(931, 540)
        Me.layoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Daily
        Me.LayoutControlItem1.Location = New System.Drawing.Point(624, 42)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(285, 64)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.Weekly
        Me.LayoutControlItem2.Location = New System.Drawing.Point(341, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(283, 64)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.Annual
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(341, 64)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'TabbedControlGroup1
        '
        Me.TabbedControlGroup1.Location = New System.Drawing.Point(0, 106)
        Me.TabbedControlGroup1.Name = "TabbedControlGroup1"
        Me.TabbedControlGroup1.SelectedTabPage = Me.LayoutControlGroup2
        Me.TabbedControlGroup1.Size = New System.Drawing.Size(909, 410)
        Me.TabbedControlGroup1.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(881, 349)
        Me.LayoutControlGroup2.Text = "مجموعات"
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GridControl1
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(881, 349)
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.ID_TEXT
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(909, 42)
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        Me.LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        '
        'windowsUIButtonPanelCloseButton
        '
        Me.windowsUIButtonPanelCloseButton.ButtonInterval = 0
        WindowsUIButtonImageOptions1.ImageUri.Uri = "Backward;Size32x32;GrayScaled"
        Me.windowsUIButtonPanelCloseButton.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUIButton("", True, WindowsUIButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, Nothing, -1, False)})
        Me.windowsUIButtonPanelCloseButton.ContentAlignment = System.Drawing.ContentAlignment.TopCenter
        Me.windowsUIButtonPanelCloseButton.Dock = System.Windows.Forms.DockStyle.Left
        Me.windowsUIButtonPanelCloseButton.ForeColor = System.Drawing.Color.Gray
        Me.windowsUIButtonPanelCloseButton.Location = New System.Drawing.Point(0, 43)
        Me.windowsUIButtonPanelCloseButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.windowsUIButtonPanelCloseButton.MaximumSize = New System.Drawing.Size(52, 0)
        Me.windowsUIButtonPanelCloseButton.MinimumSize = New System.Drawing.Size(52, 0)
        Me.windowsUIButtonPanelCloseButton.Name = "windowsUIButtonPanelCloseButton"
        Me.windowsUIButtonPanelCloseButton.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.windowsUIButtonPanelCloseButton.Padding = New System.Windows.Forms.Padding(6, 8, 0, 0)
        Me.windowsUIButtonPanelCloseButton.Size = New System.Drawing.Size(52, 587)
        Me.windowsUIButtonPanelCloseButton.TabIndex = 2
        Me.windowsUIButtonPanelCloseButton.Text = "windowsUIButtonPanel1"
        Me.windowsUIButtonPanelCloseButton.UseButtonBackgroundImages = False
        '
        'labelControl
        '
        Me.labelControl.AllowHtmlString = True
        Me.labelControl.Appearance.Font = New System.Drawing.Font("Cairo", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelControl.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.labelControl.Appearance.Options.UseFont = True
        Me.labelControl.Appearance.Options.UseForeColor = True
        Me.labelControl.Appearance.Options.UseTextOptions = True
        Me.labelControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.labelControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.labelControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.labelControl.Location = New System.Drawing.Point(52, 43)
        Me.labelControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.labelControl.Name = "labelControl"
        Me.labelControl.Padding = New System.Windows.Forms.Padding(11, 8, 0, 0)
        Me.labelControl.Size = New System.Drawing.Size(931, 47)
        Me.labelControl.TabIndex = 1
        Me.labelControl.Text = "عمولات التحويل"
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRMTransfer_commissions
        '
        Me.Appearance.BackColor = System.Drawing.Color.White
        Me.Appearance.Options.UseBackColor = True
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.ClientSize = New System.Drawing.Size(983, 630)
        Me.Controls.Add(Me.dataLayoutControl1)
        Me.Controls.Add(Me.labelControl)
        Me.Controls.Add(Me.windowsUIButtonPanelCloseButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.IconOptions.LargeImage = CType(resources.GetObject("FRMTransfer_commissions.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMTransfer_commissions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "عرض معدلات التحويل من التطبيق"
        Me.Controls.SetChildIndex(Me.windowsUIButtonPanelCloseButton, 0)
        Me.Controls.SetChildIndex(Me.labelControl, 0)
        Me.Controls.SetChildIndex(Me.dataLayoutControl1, 0)
        CType(Me.dataLayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.dataLayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemSpinEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Daily.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Weekly.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Annual.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ID_TEXT.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private WithEvents dataLayoutControl1 As DevExpress.XtraDataLayout.DataLayoutControl
    Private WithEvents layoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Private WithEvents windowsUIButtonPanelCloseButton As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Private WithEvents labelControl As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Daily As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents Weekly As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents Annual As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents First_Value As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Second_value As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Commission_value As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TabbedControlGroup1 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RepositoryItemSpinEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents ID_TEXT As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
End Class
