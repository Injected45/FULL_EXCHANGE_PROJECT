<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMCALCULATEALLMEMBERS
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMCALCULATEALLMEMBERS))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.YDATE = New DevExpress.XtraEditors.DateEdit()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.MDATE = New DevExpress.XtraEditors.DateEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.OverallNetTotal = New DevExpress.XtraEditors.SpinEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.ASSOCIATION = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.lblcurrencyname = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.YDATE.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.YDATE.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MDATE.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MDATE.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OverallNetTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ASSOCIATION.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblcurrencyname, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.YDATE)
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.MDATE)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.OverallNetTotal)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.ASSOCIATION)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1485, 700)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.SimpleButton2.Appearance.Options.UseBackColor = True
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton2.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.printer
        Me.SimpleButton2.ImageOptions.SvgImageSize = New System.Drawing.Size(34, 34)
        Me.SimpleButton2.Location = New System.Drawing.Point(258, 69)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(249, 48)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 4
        '
        'YDATE
        '
        Me.YDATE.EditValue = Nothing
        Me.YDATE.Location = New System.Drawing.Point(763, 69)
        Me.YDATE.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.YDATE.Name = "YDATE"
        Me.YDATE.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.YDATE.Properties.Appearance.Options.UseFont = True
        Me.YDATE.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.YDATE.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.YDATE.Properties.CalendarTimeProperties.MaskSettings.Set("mask", "yyyy")
        Me.YDATE.Properties.CalendarTimeProperties.MaskSettings.Set("useAdvancingCaret", False)
        Me.YDATE.Properties.CalendarTimeProperties.MaskSettings.Set("spinWithCarry", False)
        Me.YDATE.Properties.CalendarTimeProperties.UseMaskAsDisplayFormat = True
        Me.YDATE.Properties.MaskSettings.Set("mask", "yyyy")
        Me.YDATE.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.YDATE.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.YDATE.Properties.UseMaskAsDisplayFormat = True
        Me.YDATE.Size = New System.Drawing.Size(273, 50)
        Me.YDATE.StyleController = Me.LayoutControl1
        Me.YDATE.TabIndex = 2
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.GCRole.Location = New System.Drawing.Point(41, 176)
        Me.GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(1403, 334)
        Me.GCRole.TabIndex = 6
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.DetailHeight = 334
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 1029
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'MDATE
        '
        Me.MDATE.EditValue = Nothing
        Me.MDATE.Location = New System.Drawing.Point(1110, 69)
        Me.MDATE.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MDATE.Name = "MDATE"
        Me.MDATE.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.MDATE.Properties.Appearance.Options.UseFont = True
        Me.MDATE.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.MDATE.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.MDATE.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "d"
        Me.MDATE.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.MDATE.Properties.CalendarTimeProperties.EditFormat.FormatString = "d"
        Me.MDATE.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.MDATE.Properties.CalendarTimeProperties.MaskSettings.Set("spinWithCarry", False)
        Me.MDATE.Properties.CalendarTimeProperties.MaskSettings.Set("useAdvancingCaret", False)
        Me.MDATE.Properties.CalendarTimeProperties.MaskSettings.Set("mask", "M ")
        Me.MDATE.Properties.CalendarTimeProperties.UseMaskAsDisplayFormat = True
        Me.MDATE.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista
        Me.MDATE.Properties.MaskSettings.Set("mask", "MM")
        Me.MDATE.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.MDATE.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.MDATE.Properties.MaxDate = New Date(9999, 12, 31, 23, 59, 0, 0)
        Me.MDATE.Properties.UseMaskAsDisplayFormat = True
        Me.MDATE.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView
        Me.MDATE.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.[True]
        Me.MDATE.Size = New System.Drawing.Size(288, 50)
        Me.MDATE.StyleController = Me.LayoutControl1
        Me.MDATE.TabIndex = 0
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.SimpleButton1.Appearance.Options.UseBackColor = True
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.pie_chart
        Me.SimpleButton1.ImageOptions.SvgImageSize = New System.Drawing.Size(34, 34)
        Me.SimpleButton1.Location = New System.Drawing.Point(21, 69)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(229, 48)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 5
        Me.SimpleButton1.Text = "احتساب الاشتراك"
        '
        'OverallNetTotal
        '
        Me.OverallNetTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverallNetTotal.Location = New System.Drawing.Point(41, 616)
        Me.OverallNetTotal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OverallNetTotal.Name = "OverallNetTotal"
        Me.OverallNetTotal.Properties.Appearance.BackColor = System.Drawing.Color.DodgerBlue
        Me.OverallNetTotal.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.OverallNetTotal.Properties.Appearance.Options.UseBackColor = True
        Me.OverallNetTotal.Properties.Appearance.Options.UseFont = True
        Me.OverallNetTotal.Properties.Appearance.Options.UseTextOptions = True
        Me.OverallNetTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallNetTotal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverallNetTotal.Properties.AppearanceDisabled.Options.UseTextOptions = True
        Me.OverallNetTotal.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallNetTotal.Properties.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverallNetTotal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.OverallNetTotal.Properties.MaskSettings.Set("mask", "n")
        Me.OverallNetTotal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverallNetTotal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverallNetTotal.Properties.UseMaskAsDisplayFormat = True
        Me.OverallNetTotal.Size = New System.Drawing.Size(410, 50)
        Me.OverallNetTotal.StyleController = Me.LayoutControl1
        Me.OverallNetTotal.TabIndex = 7
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.Appearance.Options.UseFont = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(34, 34)
        Me.SimpleButton11.Location = New System.Drawing.Point(515, 69)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(240, 48)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 3
        '
        'ASSOCIATION
        '
        Me.ASSOCIATION.Location = New System.Drawing.Point(763, 17)
        Me.ASSOCIATION.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ASSOCIATION.Name = "ASSOCIATION"
        Me.ASSOCIATION.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ASSOCIATION.Properties.NullText = ""
        Me.ASSOCIATION.Properties.PopupSizeable = False
        Me.ASSOCIATION.Size = New System.Drawing.Size(635, 46)
        Me.ASSOCIATION.StyleController = Me.LayoutControl1
        Me.ASSOCIATION.TabIndex = 6
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem10, Me.LayoutControlGroup1, Me.LayoutControlGroup2, Me.LayoutControlItem11, Me.lblcurrencyname, Me.EmptySpaceItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1485, 700)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.MDATE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(1089, 52)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(362, 56)
        Me.LayoutControlItem1.Text = "الشهر"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(45, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.YDATE
        Me.LayoutControlItem2.Location = New System.Drawing.Point(742, 52)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(347, 56)
        Me.LayoutControlItem2.Text = "السنة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(45, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton1
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(237, 56)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.SimpleButton11
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(494, 52)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(248, 56)
        Me.LayoutControlItem10.Text = "LayoutControlItem4"
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem8, Me.EmptySpaceItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 516)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1451, 156)
        Me.LayoutControlGroup1.Text = " "
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.LayoutControlItem8.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseBackColor = True
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.Options.UseTextOptions = True
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.Control = Me.OverallNetTotal
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "ج. الصافي"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.Options.UseTextOptions = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.OptionsPrint.AppearanceItem.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.Options.UseTextOptions = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemControl.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.Options.UseTextOptions = True
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.OptionsPrint.AppearanceItemText.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.Size = New System.Drawing.Size(418, 96)
        Me.LayoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment
        Me.LayoutControlItem8.Text = "ج. الكلي"
        Me.LayoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.LayoutControlItem8.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(125, 38)
        Me.LayoutControlItem8.TextToControlDistance = 2
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(418, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(993, 96)
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 108)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(1451, 408)
        Me.LayoutControlGroup2.Text = " "
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GCRole
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(1411, 340)
        Me.LayoutControlItem3.Text = "LayoutControlItem1"
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.SimpleButton2
        Me.LayoutControlItem11.Location = New System.Drawing.Point(237, 52)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(257, 56)
        Me.LayoutControlItem11.TextVisible = False
        '
        'lblcurrencyname
        '
        Me.lblcurrencyname.Control = Me.ASSOCIATION
        Me.lblcurrencyname.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.lblcurrencyname.CustomizationFormText = "اسم المخزن"
        Me.lblcurrencyname.Location = New System.Drawing.Point(742, 0)
        Me.lblcurrencyname.Name = "lblcurrencyname"
        Me.lblcurrencyname.Size = New System.Drawing.Size(709, 52)
        Me.lblcurrencyname.Text = "الجمعية"
        Me.lblcurrencyname.TextSize = New System.Drawing.Size(45, 27)
        '
        'EmptySpaceItem2
        '
        Me.EmptySpaceItem2.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Size = New System.Drawing.Size(742, 52)
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRMCALCULATEALLMEMBERS
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1485, 700)
        Me.Controls.Add(Me.LayoutControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.IconOptions.Image = CType(resources.GetObject("FRMCALCULATEALLMEMBERS.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.Name = "FRMCALCULATEALLMEMBERS"
        Me.Text = "احتساب اشتراكات أعضاء جمعية"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.YDATE.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.YDATE.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MDATE.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MDATE.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OverallNetTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ASSOCIATION.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblcurrencyname, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents TextEdit1 As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents YDATE As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents MDATE As DevExpress.XtraEditors.DateEdit
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents OverallNetTotal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents ASSOCIATION As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblcurrencyname As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
End Class
