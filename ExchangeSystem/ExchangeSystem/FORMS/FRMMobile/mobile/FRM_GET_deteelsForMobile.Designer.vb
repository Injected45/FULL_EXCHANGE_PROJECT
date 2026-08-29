<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_GET_deteelsForMobile
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM_GET_deteelsForMobile))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.brcnch_name_Txt = New DevExpress.XtraEditors.TextEdit()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SenderName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SPhone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RecievedName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RPhone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertTime = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemTimeEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit()
        Me.CityName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ExVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.cety_name = New DevExpress.XtraEditors.TextEdit()
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.TabbedControlGroup1 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TabbedControlGroup2 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TabbedControlGroup3 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.SimpleLabelItem1 = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.SimpleLabelItem2 = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.SimpleLabelItem3 = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.commint_count = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.totla_over = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.SimpleLabelItem6 = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.VirtualServerModeSource1 = New DevExpress.Data.VirtualServerModeSource(Me.components)
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.brcnch_name_Txt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTimeEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cety_name.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SimpleLabelItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SimpleLabelItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SimpleLabelItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.commint_count, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.totla_over, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SimpleLabelItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VirtualServerModeSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.brcnch_name_Txt)
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.cety_name)
        Me.LayoutControl1.Controls.Add(Me.PictureEdit1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(1270, 215, 650, 400)
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1165, 621)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'brcnch_name_Txt
        '
        Me.brcnch_name_Txt.Location = New System.Drawing.Point(656, 63)
        Me.brcnch_name_Txt.Name = "brcnch_name_Txt"
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.Label = "الفرع"
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.brcnch_name_Txt.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.brcnch_name_Txt.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.brcnch_name_Txt.Properties.Appearance.Options.UseBackColor = True
        Me.brcnch_name_Txt.Properties.Appearance.Options.UseTextOptions = True
        Me.brcnch_name_Txt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.brcnch_name_Txt.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.brcnch_name_Txt.Properties.ReadOnly = True
        Me.brcnch_name_Txt.Size = New System.Drawing.Size(477, 58)
        Me.brcnch_name_Txt.StyleController = Me.LayoutControl1
        Me.brcnch_name_Txt.TabIndex = 0
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(32, 190)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTimeEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(1101, 282)
        Me.GridControl1.TabIndex = 3
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.Code, Me.SenderName, Me.SPhone1, Me.RecievedName, Me.RPhone1, Me.InsertDate, Me.InsertTime, Me.CityName, Me.ExVal, Me.OverallVal})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 51
        '
        'Code
        '
        Me.Code.Caption = "رمز "
        Me.Code.FieldName = "Code"
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 116
        '
        'SenderName
        '
        Me.SenderName.Caption = "الراسل"
        Me.SenderName.FieldName = "SenderName"
        Me.SenderName.Name = "SenderName"
        Me.SenderName.Visible = True
        Me.SenderName.VisibleIndex = 2
        Me.SenderName.Width = 237
        '
        'SPhone1
        '
        Me.SPhone1.Caption = "هاتف"
        Me.SPhone1.FieldName = "SPhone1"
        Me.SPhone1.Name = "SPhone1"
        Me.SPhone1.Visible = True
        Me.SPhone1.VisibleIndex = 3
        Me.SPhone1.Width = 97
        '
        'RecievedName
        '
        Me.RecievedName.Caption = "المستلم"
        Me.RecievedName.FieldName = "RecievedName"
        Me.RecievedName.Name = "RecievedName"
        Me.RecievedName.Visible = True
        Me.RecievedName.VisibleIndex = 4
        Me.RecievedName.Width = 196
        '
        'RPhone1
        '
        Me.RPhone1.Caption = "هاتف"
        Me.RPhone1.FieldName = "RPhone1"
        Me.RPhone1.Name = "RPhone1"
        Me.RPhone1.Visible = True
        Me.RPhone1.VisibleIndex = 5
        Me.RPhone1.Width = 103
        '
        'InsertDate
        '
        Me.InsertDate.Caption = "تاريخ"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 6
        Me.InsertDate.Width = 71
        '
        'InsertTime
        '
        Me.InsertTime.Caption = "التوقيت"
        Me.InsertTime.ColumnEdit = Me.RepositoryItemTimeEdit1
        Me.InsertTime.FieldName = "InsertTime"
        Me.InsertTime.Name = "InsertTime"
        Me.InsertTime.Visible = True
        Me.InsertTime.VisibleIndex = 7
        Me.InsertTime.Width = 71
        '
        'RepositoryItemTimeEdit1
        '
        Me.RepositoryItemTimeEdit1.AutoHeight = False
        Me.RepositoryItemTimeEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemTimeEdit1.DisplayFormat.FormatString = "hh:mm:g"
        Me.RepositoryItemTimeEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemTimeEdit1.EditFormat.FormatString = "hh:mm:g"
        Me.RepositoryItemTimeEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.RepositoryItemTimeEdit1.Name = "RepositoryItemTimeEdit1"
        Me.RepositoryItemTimeEdit1.TimeEditStyle = DevExpress.XtraEditors.Repository.TimeEditStyle.TouchUI
        Me.RepositoryItemTimeEdit1.UseMaskAsDisplayFormat = True
        Me.RepositoryItemTimeEdit1.XlsxFormatString = "hh:mm:st"
        '
        'CityName
        '
        Me.CityName.Caption = "المدينة"
        Me.CityName.FieldName = "CityName"
        Me.CityName.Name = "CityName"
        '
        'ExVal
        '
        Me.ExVal.Caption = "العمولة"
        Me.ExVal.FieldName = "ExVal"
        Me.ExVal.Name = "ExVal"
        Me.ExVal.Visible = True
        Me.ExVal.VisibleIndex = 8
        Me.ExVal.Width = 71
        '
        'OverallVal
        '
        Me.OverallVal.Caption = "القيمة"
        Me.OverallVal.FieldName = "OverallVal"
        Me.OverallVal.Name = "OverallVal"
        Me.OverallVal.Visible = True
        Me.OverallVal.VisibleIndex = 9
        Me.OverallVal.Width = 88
        '
        'cety_name
        '
        Me.cety_name.Location = New System.Drawing.Point(191, 63)
        Me.cety_name.Name = "cety_name"
        Me.cety_name.Properties.AdvancedModeOptions.Label = "اسم المدينة"
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.Options.UseFont = True
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.Options.UseForeColor = True
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.Options.UseTextOptions = True
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.cety_name.Properties.AdvancedModeOptions.LabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.ForeColor = System.Drawing.Color.Black
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseFont = True
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseForeColor = True
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.Options.UseTextOptions = True
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        Me.cety_name.Properties.AdvancedModeOptions.ShiftedLabelAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.cety_name.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.cety_name.Properties.Appearance.Options.UseBackColor = True
        Me.cety_name.Properties.Appearance.Options.UseTextOptions = True
        Me.cety_name.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.cety_name.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.cety_name.Properties.ReadOnly = True
        Me.cety_name.Size = New System.Drawing.Size(459, 58)
        Me.cety_name.StyleController = Me.LayoutControl1
        Me.cety_name.TabIndex = 2
        '
        'PictureEdit1
        '
        Me.PictureEdit1.EditValue = CType(resources.GetObject("PictureEdit1.EditValue"), Object)
        Me.PictureEdit1.Location = New System.Drawing.Point(32, 63)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.ReadOnly = True
        Me.PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        Me.PictureEdit1.Size = New System.Drawing.Size(153, 58)
        Me.PictureEdit1.StyleController = Me.LayoutControl1
        Me.PictureEdit1.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.TabbedControlGroup1, Me.TabbedControlGroup2, Me.TabbedControlGroup3})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1165, 621)
        Me.Root.TextVisible = False
        '
        'TabbedControlGroup1
        '
        Me.TabbedControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.TabbedControlGroup1.Name = "TabbedControlGroup1"
        Me.TabbedControlGroup1.SelectedTabPage = Me.LayoutControlGroup1
        Me.TabbedControlGroup1.Size = New System.Drawing.Size(1139, 127)
        Me.TabbedControlGroup1.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1})
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1107, 64)
        Me.LayoutControlGroup1.Text = "البيانات الاساسية"
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.PictureEdit1
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(159, 64)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.cety_name
        Me.LayoutControlItem3.Location = New System.Drawing.Point(159, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(465, 64)
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.brcnch_name_Txt
        Me.LayoutControlItem4.Location = New System.Drawing.Point(624, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(483, 64)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'TabbedControlGroup2
        '
        Me.TabbedControlGroup2.Location = New System.Drawing.Point(0, 127)
        Me.TabbedControlGroup2.Name = "TabbedControlGroup2"
        Me.TabbedControlGroup2.SelectedTabPage = Me.LayoutControlGroup2
        Me.TabbedControlGroup2.Size = New System.Drawing.Size(1139, 351)
        Me.TabbedControlGroup2.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(1107, 288)
        Me.LayoutControlGroup2.Text = "التفاصيل"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1107, 288)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'TabbedControlGroup3
        '
        Me.TabbedControlGroup3.Location = New System.Drawing.Point(0, 478)
        Me.TabbedControlGroup3.Name = "TabbedControlGroup3"
        Me.TabbedControlGroup3.SelectedTabPage = Me.LayoutControlGroup3
        Me.TabbedControlGroup3.Size = New System.Drawing.Size(1139, 117)
        Me.TabbedControlGroup3.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup3})
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.AppearanceGroup.BorderColor = System.Drawing.Color.White
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup3.AppearanceItemCaption.BackColor = System.Drawing.Color.White
        Me.LayoutControlGroup3.AppearanceItemCaption.Options.UseBackColor = True
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.SimpleLabelItem1, Me.SimpleLabelItem2, Me.SimpleLabelItem3, Me.commint_count, Me.totla_over, Me.SimpleLabelItem6})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(1107, 54)
        Me.LayoutControlGroup3.Text = "الاجمالايات"
        '
        'SimpleLabelItem1
        '
        Me.SimpleLabelItem1.AllowHotTrack = False
        Me.SimpleLabelItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.SimpleLabelItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleLabelItem1.Location = New System.Drawing.Point(420, 0)
        Me.SimpleLabelItem1.Name = "SimpleLabelItem1"
        Me.SimpleLabelItem1.Size = New System.Drawing.Size(421, 27)
        Me.SimpleLabelItem1.Text = "مجموع العمولة"
        Me.SimpleLabelItem1.TextSize = New System.Drawing.Size(71, 21)
        '
        'SimpleLabelItem2
        '
        Me.SimpleLabelItem2.AllowHotTrack = False
        Me.SimpleLabelItem2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.SimpleLabelItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleLabelItem2.Location = New System.Drawing.Point(841, 0)
        Me.SimpleLabelItem2.Name = "SimpleLabelItem2"
        Me.SimpleLabelItem2.Size = New System.Drawing.Size(266, 27)
        Me.SimpleLabelItem2.Text = "عدد الحولات"
        Me.SimpleLabelItem2.TextSize = New System.Drawing.Size(71, 21)
        '
        'SimpleLabelItem3
        '
        Me.SimpleLabelItem3.AllowHotTrack = False
        Me.SimpleLabelItem3.AppearanceItemCaption.Options.UseTextOptions = True
        Me.SimpleLabelItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleLabelItem3.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleLabelItem3.Location = New System.Drawing.Point(841, 27)
        Me.SimpleLabelItem3.Name = "SimpleLabelItem3"
        Me.SimpleLabelItem3.Size = New System.Drawing.Size(266, 27)
        Me.SimpleLabelItem3.Text = "0"
        Me.SimpleLabelItem3.TextSize = New System.Drawing.Size(71, 21)
        '
        'commint_count
        '
        Me.commint_count.AllowHotTrack = False
        Me.commint_count.AppearanceItemCaption.Options.UseTextOptions = True
        Me.commint_count.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.commint_count.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.commint_count.Location = New System.Drawing.Point(420, 27)
        Me.commint_count.Name = "commint_count"
        Me.commint_count.OptionsPrint.AppearanceItem.BackColor = System.Drawing.Color.Red
        Me.commint_count.OptionsPrint.AppearanceItem.Options.UseBackColor = True
        Me.commint_count.OptionsPrint.AppearanceItemControl.BackColor = System.Drawing.Color.Red
        Me.commint_count.OptionsPrint.AppearanceItemControl.Options.UseBackColor = True
        Me.commint_count.Size = New System.Drawing.Size(421, 27)
        Me.commint_count.Text = "0"
        Me.commint_count.TextSize = New System.Drawing.Size(71, 21)
        '
        'totla_over
        '
        Me.totla_over.AllowHotTrack = False
        Me.totla_over.AppearanceItemCaption.Options.UseTextOptions = True
        Me.totla_over.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.totla_over.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.totla_over.Location = New System.Drawing.Point(0, 27)
        Me.totla_over.Name = "totla_over"
        Me.totla_over.Size = New System.Drawing.Size(420, 27)
        Me.totla_over.Text = "0"
        Me.totla_over.TextSize = New System.Drawing.Size(71, 21)
        '
        'SimpleLabelItem6
        '
        Me.SimpleLabelItem6.AllowHotTrack = False
        Me.SimpleLabelItem6.AppearanceItemCaption.Options.UseTextOptions = True
        Me.SimpleLabelItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleLabelItem6.Location = New System.Drawing.Point(0, 0)
        Me.SimpleLabelItem6.Name = "SimpleLabelItem6"
        Me.SimpleLabelItem6.Size = New System.Drawing.Size(420, 27)
        Me.SimpleLabelItem6.Text = "الاجمالي"
        Me.SimpleLabelItem6.TextSize = New System.Drawing.Size(71, 21)
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRM_GET_deteelsForMobile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1165, 621)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FRM_GET_deteelsForMobile.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Name = "FRM_GET_deteelsForMobile"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "شاشة عرض حوالات المدينة"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.brcnch_name_Txt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTimeEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cety_name.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SimpleLabelItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SimpleLabelItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SimpleLabelItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.commint_count, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.totla_over, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SimpleLabelItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VirtualServerModeSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents VirtualServerModeSource1 As DevExpress.Data.VirtualServerModeSource
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SenderName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SPhone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RecievedName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RPhone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertTime As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CityName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ExVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
    Friend WithEvents RepositoryItemTimeEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit
    Friend WithEvents cety_name As DevExpress.XtraEditors.TextEdit
    Friend WithEvents brcnch_name_Txt As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents TabbedControlGroup1 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TabbedControlGroup2 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TabbedControlGroup3 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SimpleLabelItem1 As DevExpress.XtraLayout.SimpleLabelItem
    Friend WithEvents SimpleLabelItem2 As DevExpress.XtraLayout.SimpleLabelItem
    Friend WithEvents SimpleLabelItem3 As DevExpress.XtraLayout.SimpleLabelItem
    Friend WithEvents commint_count As DevExpress.XtraLayout.SimpleLabelItem
    Friend WithEvents totla_over As DevExpress.XtraLayout.SimpleLabelItem
    Friend WithEvents SimpleLabelItem6 As DevExpress.XtraLayout.SimpleLabelItem
End Class
