<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMSaleCurrencyForCUST
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMSaleCurrencyForCUST))
        Me.TYPEFROM = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CRedetDL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.buyprice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CRedetTO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CustIDNo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl2 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ISID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.MaxVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACC_DEPET_TO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACC_DEPET_DL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.salesPurchaseprice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Inseart_Date = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACCCRint0 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Revenue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LeftVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.thepurpose = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.NatNumber = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TabbedControlGroup1 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.CustName = New DevExpress.XtraEditors.TextEdit()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NatNumber.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CustName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TYPEFROM
        '
        Me.TYPEFROM.Caption = "النوع"
        Me.TYPEFROM.FieldName = "TYPEFROM"
        Me.TYPEFROM.Name = "TYPEFROM"
        Me.TYPEFROM.Visible = True
        Me.TYPEFROM.VisibleIndex = 7
        Me.TYPEFROM.Width = 101
        '
        'CRedetDL
        '
        Me.CRedetDL.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.CRedetDL.AppearanceCell.Options.UseBackColor = True
        Me.CRedetDL.Caption = "قيمة د.ل"
        Me.CRedetDL.FieldName = "CRedetDL"
        Me.CRedetDL.Name = "CRedetDL"
        Me.CRedetDL.Summary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CRedetDL", "sum={0:0.##}")})
        Me.CRedetDL.Visible = True
        Me.CRedetDL.VisibleIndex = 4
        Me.CRedetDL.Width = 149
        '
        'buyprice
        '
        Me.buyprice.Caption = "سعر الشراء"
        Me.buyprice.FieldName = "buyprice"
        Me.buyprice.Name = "buyprice"
        Me.buyprice.Visible = True
        Me.buyprice.VisibleIndex = 4
        Me.buyprice.Width = 80
        '
        'CRedetTO
        '
        Me.CRedetTO.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.CRedetTO.AppearanceCell.Options.UseBackColor = True
        Me.CRedetTO.Caption = "ك/مشتراة"
        Me.CRedetTO.FieldName = "CRedetTO"
        Me.CRedetTO.Name = "CRedetTO"
        Me.CRedetTO.Visible = True
        Me.CRedetTO.VisibleIndex = 4
        Me.CRedetTO.Width = 98
        '
        'CustIDNo
        '
        Me.CustIDNo.Caption = "الرقم الوطني"
        Me.CustIDNo.FieldName = "CustIDNo"
        Me.CustIDNo.Name = "CustIDNo"
        Me.CustIDNo.Visible = True
        Me.CustIDNo.VisibleIndex = 3
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl2)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.NatNumber)
        Me.LayoutControl1.Controls.Add(Me.CustName)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1458, 671)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl2
        '
        Me.GridControl2.Location = New System.Drawing.Point(32, 63)
        Me.GridControl2.MainView = Me.GVRole
        Me.GridControl2.Name = "GridControl2"
        Me.GridControl2.Size = New System.Drawing.Size(1046, 576)
        Me.GridControl2.TabIndex = 8
        Me.GridControl2.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.GVRole.Appearance.FooterPanel.Options.UseBackColor = True
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.ISID, Me.MaxVal, Me.ACC_DEPET_TO, Me.ACC_DEPET_DL, Me.salesPurchaseprice, Me.Inseart_Date, Me.ACCCRint0, Me.Revenue, Me.LeftVal, Me.thepurpose})
        Me.GVRole.DetailHeight = 334
        Me.GVRole.GridControl = Me.GridControl2
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[True]
        Me.GVRole.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[True]
        Me.GVRole.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[True]
        Me.GVRole.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[True]
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 44
        '
        'ISID
        '
        Me.ISID.Caption = "الكود"
        Me.ISID.FieldName = "ISID"
        Me.ISID.Name = "ISID"
        Me.ISID.Visible = True
        Me.ISID.VisibleIndex = 1
        Me.ISID.Width = 66
        '
        'MaxVal
        '
        Me.MaxVal.Caption = "سقف الكمية"
        Me.MaxVal.FieldName = "MaxVal"
        Me.MaxVal.Name = "MaxVal"
        Me.MaxVal.Visible = True
        Me.MaxVal.VisibleIndex = 3
        '
        'ACC_DEPET_TO
        '
        Me.ACC_DEPET_TO.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.ACC_DEPET_TO.AppearanceCell.Options.UseBackColor = True
        Me.ACC_DEPET_TO.Caption = "ك/ مباعة"
        Me.ACC_DEPET_TO.FieldName = "ACC_DEPET_TO"
        Me.ACC_DEPET_TO.Name = "ACC_DEPET_TO"
        Me.ACC_DEPET_TO.Visible = True
        Me.ACC_DEPET_TO.VisibleIndex = 4
        Me.ACC_DEPET_TO.Width = 100
        '
        'ACC_DEPET_DL
        '
        Me.ACC_DEPET_DL.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ACC_DEPET_DL.AppearanceCell.Options.UseBackColor = True
        Me.ACC_DEPET_DL.Caption = "قيمة البيع"
        Me.ACC_DEPET_DL.FieldName = "ACC_DEPET_DL"
        Me.ACC_DEPET_DL.Name = "ACC_DEPET_DL"
        Me.ACC_DEPET_DL.Summary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ACC_DEPET_DL", "SUM ={0:0.##}")})
        Me.ACC_DEPET_DL.Width = 139
        '
        'salesPurchaseprice
        '
        Me.salesPurchaseprice.Caption = "سعر البيع"
        Me.salesPurchaseprice.FieldName = "salesPurchaseprice"
        Me.salesPurchaseprice.Name = "salesPurchaseprice"
        Me.salesPurchaseprice.Visible = True
        Me.salesPurchaseprice.VisibleIndex = 5
        Me.salesPurchaseprice.Width = 66
        '
        'Inseart_Date
        '
        Me.Inseart_Date.Caption = "ت/الاضافة"
        Me.Inseart_Date.FieldName = "Inseart_Date"
        Me.Inseart_Date.Name = "Inseart_Date"
        Me.Inseart_Date.Visible = True
        Me.Inseart_Date.VisibleIndex = 2
        Me.Inseart_Date.Width = 80
        '
        'ACCCRint0
        '
        Me.ACCCRint0.Caption = "ع ش المحلية"
        Me.ACCCRint0.FieldName = "ACCCRint0"
        Me.ACCCRint0.Name = "ACCCRint0"
        '
        'Revenue
        '
        Me.Revenue.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Revenue.AppearanceCell.Options.UseBackColor = True
        Me.Revenue.Caption = "قيمة د.ل"
        Me.Revenue.FieldName = "Revenue"
        Me.Revenue.Name = "Revenue"
        Me.Revenue.Visible = True
        Me.Revenue.VisibleIndex = 6
        Me.Revenue.Width = 122
        '
        'LeftVal
        '
        Me.LeftVal.Caption = "المتبقي"
        Me.LeftVal.FieldName = "LeftVal"
        Me.LeftVal.Name = "LeftVal"
        Me.LeftVal.Visible = True
        Me.LeftVal.VisibleIndex = 8
        '
        'thepurpose
        '
        Me.thepurpose.Caption = "الغرض"
        Me.thepurpose.FieldName = "thepurpose"
        Me.thepurpose.Name = "thepurpose"
        Me.thepurpose.Visible = True
        Me.thepurpose.VisibleIndex = 7
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton1.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1.Location = New System.Drawing.Point(1116, 183)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(310, 38)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 4
        Me.SimpleButton1.Text = "عرض "
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton2.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton2.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton2.Location = New System.Drawing.Point(1116, 227)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(310, 38)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 5
        Me.SimpleButton2.Text = "طباعه"
        '
        'CurrencyID
        '
        Me.CurrencyID.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CurrencyID.Location = New System.Drawing.Point(1116, 141)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Appearance.Options.UseTextOptions = True
        Me.CurrencyID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurrencyID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Size = New System.Drawing.Size(208, 36)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 2
        '
        'NatNumber
        '
        Me.NatNumber.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NatNumber.Location = New System.Drawing.Point(1116, 53)
        Me.NatNumber.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.NatNumber.Name = "NatNumber"
        Me.NatNumber.Properties.Appearance.Options.UseTextOptions = True
        Me.NatNumber.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NatNumber.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NatNumber.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NatNumber.Properties.NullText = ""
        Me.NatNumber.Size = New System.Drawing.Size(208, 36)
        Me.NatNumber.StyleController = Me.LayoutControl1
        Me.NatNumber.TabIndex = 3
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1, Me.TabbedControlGroup1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1458, 671)
        Me.Root.TextLocation = DevExpress.Utils.Locations.Right
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlGroup1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem10, Me.LayoutControlItem9, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.LayoutControlItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(1084, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(348, 645)
        Me.LayoutControlGroup1.Text = "البيانات الرئيسية"
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.SimpleButton2
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 174)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(316, 418)
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.SimpleButton1
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 130)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(316, 44)
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem7.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem7.Control = Me.CurrencyID
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem7.CustomizationFormText = "الفــرع"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 88)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(316, 42)
        Me.LayoutControlItem7.Text = "العملة"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(86, 28)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem8.Control = Me.NatNumber
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem8.CustomizationFormText = "الفــرع"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(316, 42)
        Me.LayoutControlItem8.Text = "الرقم الوطني"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(86, 28)
        '
        'TabbedControlGroup1
        '
        Me.TabbedControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.TabbedControlGroup1.Name = "TabbedControlGroup1"
        Me.TabbedControlGroup1.SelectedTabPage = Me.LayoutControlGroup2
        Me.TabbedControlGroup1.Size = New System.Drawing.Size(1084, 645)
        Me.TabbedControlGroup1.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup2.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup2.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.LayoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(1052, 582)
        Me.LayoutControlGroup2.Text = "التفاصيل"
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GridControl2
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(1052, 582)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'CustName
        '
        Me.CustName.Location = New System.Drawing.Point(1116, 95)
        Me.CustName.Name = "CustName"
        Me.CustName.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.CustName.Properties.Appearance.Options.UseFont = True
        Me.CustName.Properties.Appearance.Options.UseTextOptions = True
        Me.CustName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CustName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CustName.Properties.AppearanceReadOnly.Options.UseTextOptions = True
        Me.CustName.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CustName.Properties.AppearanceReadOnly.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CustName.Properties.ReadOnly = True
        Me.CustName.Size = New System.Drawing.Size(208, 40)
        Me.CustName.StyleController = Me.LayoutControl1
        Me.CustName.TabIndex = 9
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 10.0!)
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem1.Control = Me.CustName
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(316, 46)
        Me.LayoutControlItem1.Text = "العميل"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(86, 27)
        '
        'FRMSaleCurrencyForCUST
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1458, 671)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMSaleCurrencyForCUST.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.Name = "FRMSaleCurrencyForCUST"
        Me.Text = "حركة بيع عملة لعميل"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NatNumber.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CustName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TYPEFROM As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CRedetDL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents buyprice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CRedetTO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CustIDNo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents NatNumber As DevExpress.XtraEditors.LookUpEdit
    Public WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TabbedControlGroup1 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControl2 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ISID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACC_DEPET_TO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACC_DEPET_DL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents salesPurchaseprice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Inseart_Date As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACCCRint0 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Revenue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents thepurpose As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents MaxVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LeftVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CustName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
End Class
