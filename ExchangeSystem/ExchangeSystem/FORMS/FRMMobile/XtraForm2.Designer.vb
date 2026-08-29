<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class XtraForm2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XtraForm2))
        Me.dataLayoutControl1 = New DevExpress.XtraDataLayout.DataLayoutControl()
        Me.Notes = New DevExpress.XtraEditors.MemoEdit()
        Me.Daily = New DevExpress.XtraEditors.SpinEdit()
        Me.Weekly = New DevExpress.XtraEditors.SpinEdit()
        Me.monthly = New DevExpress.XtraEditors.SpinEdit()
        Me.Annual = New DevExpress.XtraEditors.SpinEdit()
        Me.ueserTyp = New DevExpress.XtraEditors.LookUpEdit()
        Me.layoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.windowsUIButtonPanelCloseButton = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        Me.labelControl = New DevExpress.XtraEditors.LabelControl()
        CType(Me.dataLayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.dataLayoutControl1.SuspendLayout()
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Daily.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Weekly.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.monthly.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Annual.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ueserTyp.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dataLayoutControl1
        '
        Me.dataLayoutControl1.AllowCustomization = False
        Me.dataLayoutControl1.Controls.Add(Me.Notes)
        Me.dataLayoutControl1.Controls.Add(Me.Daily)
        Me.dataLayoutControl1.Controls.Add(Me.Weekly)
        Me.dataLayoutControl1.Controls.Add(Me.monthly)
        Me.dataLayoutControl1.Controls.Add(Me.Annual)
        Me.dataLayoutControl1.Controls.Add(Me.ueserTyp)
        Me.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dataLayoutControl1.Location = New System.Drawing.Point(68, 90)
        Me.dataLayoutControl1.Margin = New System.Windows.Forms.Padding(4)
        Me.dataLayoutControl1.Name = "dataLayoutControl1"
        Me.dataLayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.dataLayoutControl1.Root = Me.layoutControlGroup1
        Me.dataLayoutControl1.Size = New System.Drawing.Size(849, 414)
        Me.dataLayoutControl1.TabIndex = 0
        '
        'Notes
        '
        Me.Notes.Location = New System.Drawing.Point(16, 342)
        Me.Notes.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Notes.Name = "Notes"
        Me.Notes.Size = New System.Drawing.Size(817, 56)
        Me.Notes.StyleController = Me.dataLayoutControl1
        Me.Notes.TabIndex = 8
        '
        'Daily
        '
        Me.Daily.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Daily.Location = New System.Drawing.Point(16, 80)
        Me.Daily.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Daily.Name = "Daily"
        Me.Daily.Properties.AdvancedModeOptions.Label = "تحويل اليومي"
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
        Me.Daily.Properties.MaskSettings.Set("mask", "n")
        Me.Daily.Properties.MaxValue = New Decimal(New Integer() {-1981284352, -1966660860, 0, 0})
        Me.Daily.Properties.UseMaskAsDisplayFormat = True
        Me.Daily.Size = New System.Drawing.Size(817, 64)
        Me.Daily.StyleController = Me.dataLayoutControl1
        Me.Daily.TabIndex = 4
        '
        'Weekly
        '
        Me.Weekly.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Weekly.Location = New System.Drawing.Point(16, 150)
        Me.Weekly.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Weekly.Name = "Weekly"
        Me.Weekly.Properties.AdvancedModeOptions.Label = "معدل التحويل الاسبوعي"
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
        Me.Weekly.Properties.MaskSettings.Set("mask", "n")
        Me.Weekly.Properties.MaxValue = New Decimal(New Integer() {1874919424, 2328306, 0, 0})
        Me.Weekly.Properties.UseMaskAsDisplayFormat = True
        Me.Weekly.Properties.XlsxFormatString = "0.00"
        Me.Weekly.Size = New System.Drawing.Size(817, 58)
        Me.Weekly.StyleController = Me.dataLayoutControl1
        Me.Weekly.TabIndex = 5
        '
        'monthly
        '
        Me.monthly.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.monthly.Location = New System.Drawing.Point(16, 214)
        Me.monthly.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.monthly.Name = "monthly"
        Me.monthly.Properties.AdvancedModeOptions.Label = "معدل التحويل الشهري"
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.monthly.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.monthly.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.monthly.Properties.Appearance.Options.UseTextOptions = True
        Me.monthly.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.monthly.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.monthly.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.monthly.Properties.EditFormat.FormatString = "0.00"
        Me.monthly.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.monthly.Properties.MaskSettings.Set("mask", "n")
        Me.monthly.Properties.MaxValue = New Decimal(New Integer() {1874919424, 2328306, 0, 0})
        Me.monthly.Properties.UseMaskAsDisplayFormat = True
        Me.monthly.Size = New System.Drawing.Size(817, 58)
        Me.monthly.StyleController = Me.dataLayoutControl1
        Me.monthly.TabIndex = 6
        '
        'Annual
        '
        Me.Annual.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Annual.Location = New System.Drawing.Point(16, 278)
        Me.Annual.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Annual.Name = "Annual"
        Me.Annual.Properties.AdvancedModeOptions.Label = "معدل التحويل السنوي "
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
        Me.Annual.Size = New System.Drawing.Size(817, 58)
        Me.Annual.StyleController = Me.dataLayoutControl1
        Me.Annual.TabIndex = 7
        '
        'ueserTyp
        '
        Me.ueserTyp.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ueserTyp.Location = New System.Drawing.Point(16, 16)
        Me.ueserTyp.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ueserTyp.Name = "ueserTyp"
        Me.ueserTyp.Properties.AdvancedModeOptions.Label = "نوع الحساب"
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.ueserTyp.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.ueserTyp.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ueserTyp.Properties.Appearance.Options.UseTextOptions = True
        Me.ueserTyp.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ueserTyp.Properties.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Show
        Me.ueserTyp.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ueserTyp.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ueserTyp.Properties.EditFormat.FormatString = "0.00"
        Me.ueserTyp.Properties.NullText = ""
        Me.ueserTyp.Size = New System.Drawing.Size(817, 58)
        Me.ueserTyp.StyleController = Me.dataLayoutControl1
        Me.ueserTyp.TabIndex = 9
        '
        'layoutControlGroup1
        '
        Me.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.layoutControlGroup1.GroupBordersVisible = False
        Me.layoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem6})
        Me.layoutControlGroup1.Name = "Root"
        Me.layoutControlGroup1.Size = New System.Drawing.Size(849, 414)
        Me.layoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Daily
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 64)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(823, 70)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.Weekly
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 134)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(823, 64)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.monthly
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 198)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(823, 64)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.Annual
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 262)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(823, 64)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.Notes
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 326)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(823, 62)
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.ueserTyp
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(823, 64)
        Me.LayoutControlItem6.TextVisible = False
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
        Me.windowsUIButtonPanelCloseButton.Margin = New System.Windows.Forms.Padding(4)
        Me.windowsUIButtonPanelCloseButton.MaximumSize = New System.Drawing.Size(68, 0)
        Me.windowsUIButtonPanelCloseButton.MinimumSize = New System.Drawing.Size(68, 0)
        Me.windowsUIButtonPanelCloseButton.Name = "windowsUIButtonPanelCloseButton"
        Me.windowsUIButtonPanelCloseButton.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.windowsUIButtonPanelCloseButton.Padding = New System.Windows.Forms.Padding(8, 7, 0, 0)
        Me.windowsUIButtonPanelCloseButton.Size = New System.Drawing.Size(68, 461)
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
        Me.labelControl.Location = New System.Drawing.Point(68, 43)
        Me.labelControl.Margin = New System.Windows.Forms.Padding(4)
        Me.labelControl.Name = "labelControl"
        Me.labelControl.Padding = New System.Windows.Forms.Padding(15, 7, 0, 0)
        Me.labelControl.Size = New System.Drawing.Size(849, 47)
        Me.labelControl.TabIndex = 1
        Me.labelControl.Text = "معدل تحويلات التطبيق"
        '
        'XtraForm2
        '
        Me.Appearance.BackColor = System.Drawing.Color.White
        Me.Appearance.Options.UseBackColor = True
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.ClientSize = New System.Drawing.Size(917, 504)
        Me.Controls.Add(Me.dataLayoutControl1)
        Me.Controls.Add(Me.labelControl)
        Me.Controls.Add(Me.windowsUIButtonPanelCloseButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.IconOptions.LargeImage = CType(resources.GetObject("XtraForm2.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "XtraForm2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "عرض معدلات التحويل من التطبيق"
        Me.Controls.SetChildIndex(Me.windowsUIButtonPanelCloseButton, 0)
        Me.Controls.SetChildIndex(Me.labelControl, 0)
        Me.Controls.SetChildIndex(Me.dataLayoutControl1, 0)
        CType(Me.dataLayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.dataLayoutControl1.ResumeLayout(False)
        CType(Me.Notes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Daily.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Weekly.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.monthly.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Annual.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ueserTyp.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private WithEvents dataLayoutControl1 As DevExpress.XtraDataLayout.DataLayoutControl
    Private WithEvents layoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Private WithEvents windowsUIButtonPanelCloseButton As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Private WithEvents labelControl As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Notes As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Daily As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents Weekly As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents monthly As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents Annual As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents ueserTyp As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class
