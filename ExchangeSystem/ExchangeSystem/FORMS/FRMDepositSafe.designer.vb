<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMDepositSafe
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMDepositSafe))
        Me.panelLeft = New System.Windows.Forms.Panel()
        Me.LayoutControl2 = New DevExpress.XtraLayout.LayoutControl()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.GridControl11 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn11 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACCicount = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACCNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.INSERCREDET = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.INSERTDEBIT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SUMcredat = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SUMdebit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.total = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Noetes = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACCID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DT1 = New DevExpress.XtraEditors.DateEdit()
        Me.DT2 = New DevExpress.XtraEditors.DateEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton21 = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CustName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Debit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Credit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACCNETtotel = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID_Cu = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TypeMov_cus = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.panelLeft.SuspendLayout()
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl2.SuspendLayout()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panelLeft
        '
        Me.panelLeft.Controls.Add(Me.LayoutControl2)
        Me.panelLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelLeft.Location = New System.Drawing.Point(0, 0)
        Me.panelLeft.Name = "panelLeft"
        Me.panelLeft.Size = New System.Drawing.Size(1513, 701)
        Me.panelLeft.TabIndex = 2
        '
        'LayoutControl2
        '
        Me.LayoutControl2.Controls.Add(Me.BranchID)
        Me.LayoutControl2.Controls.Add(Me.GridControl11)
        Me.LayoutControl2.Controls.Add(Me.DT1)
        Me.LayoutControl2.Controls.Add(Me.DT2)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton21)
        Me.LayoutControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl2.Name = "LayoutControl2"
        Me.LayoutControl2.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl2.Root = Me.Root
        Me.LayoutControl2.Size = New System.Drawing.Size(1513, 701)
        Me.LayoutControl2.TabIndex = 0
        Me.LayoutControl2.Text = "LayoutControl2"
        '
        'BranchID
        '
        Me.BranchID.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BranchID.Location = New System.Drawing.Point(1111, 51)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(317, 36)
        Me.BranchID.StyleController = Me.LayoutControl2
        Me.BranchID.TabIndex = 0
        '
        'GridControl11
        '
        Me.GridControl11.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GridControl11.Location = New System.Drawing.Point(32, 95)
        Me.GridControl11.MainView = Me.GVRole
        Me.GridControl11.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GridControl11.Name = "GridControl11"
        Me.GridControl11.Size = New System.Drawing.Size(1449, 576)
        Me.GridControl11.TabIndex = 6
        Me.GridControl11.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn11, Me.ACCicount, Me.ACCNAME, Me.INSERCREDET, Me.INSERTDEBIT, Me.SUMcredat, Me.SUMdebit, Me.total, Me.Noetes, Me.ACCID})
        Me.GVRole.DetailHeight = 294
        Me.GVRole.GridControl = Me.GridControl11
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsBehavior.ReadOnly = True
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 640
        Me.GVRole.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GVRole.OptionsSelection.EnableAppearanceHideSelection = False
        '
        'GridColumn11
        '
        Me.GridColumn11.Caption = "#"
        Me.GridColumn11.FieldName = "SN"
        Me.GridColumn11.MinWidth = 16
        Me.GridColumn11.Name = "GridColumn11"
        Me.GridColumn11.Width = 60
        '
        'ACCicount
        '
        Me.ACCicount.Caption = "رقم الحساب"
        Me.ACCicount.FieldName = "acccode"
        Me.ACCicount.MinWidth = 16
        Me.ACCicount.Name = "ACCicount"
        Me.ACCicount.Visible = True
        Me.ACCicount.VisibleIndex = 0
        Me.ACCicount.Width = 107
        '
        'ACCNAME
        '
        Me.ACCNAME.Caption = "اسم الحساب"
        Me.ACCNAME.FieldName = "ACCNAME"
        Me.ACCNAME.MinWidth = 16
        Me.ACCNAME.Name = "ACCNAME"
        Me.ACCNAME.Visible = True
        Me.ACCNAME.VisibleIndex = 1
        Me.ACCNAME.Width = 136
        '
        'INSERCREDET
        '
        Me.INSERCREDET.Caption = "الرصيد الافتتاحي الدائن "
        Me.INSERCREDET.FieldName = "INSERCREDET"
        Me.INSERCREDET.MinWidth = 16
        Me.INSERCREDET.Name = "INSERCREDET"
        Me.INSERCREDET.Width = 147
        '
        'INSERTDEBIT
        '
        Me.INSERTDEBIT.Caption = "رصيد الافتتاحي  المدين"
        Me.INSERTDEBIT.FieldName = "INSERTDEBIT"
        Me.INSERTDEBIT.MinWidth = 16
        Me.INSERTDEBIT.Name = "INSERTDEBIT"
        Me.INSERTDEBIT.Width = 147
        '
        'SUMcredat
        '
        Me.SUMcredat.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.SUMcredat.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SUMcredat.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.SUMcredat.AppearanceCell.Options.UseBackColor = True
        Me.SUMcredat.AppearanceCell.Options.UseFont = True
        Me.SUMcredat.AppearanceCell.Options.UseForeColor = True
        Me.SUMcredat.Caption = " حركة  الدائن"
        Me.SUMcredat.FieldName = "credet"
        Me.SUMcredat.MinWidth = 16
        Me.SUMcredat.Name = "SUMcredat"
        Me.SUMcredat.Visible = True
        Me.SUMcredat.VisibleIndex = 3
        Me.SUMcredat.Width = 130
        '
        'SUMdebit
        '
        Me.SUMdebit.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.SUMdebit.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SUMdebit.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.SUMdebit.AppearanceCell.Options.UseBackColor = True
        Me.SUMdebit.AppearanceCell.Options.UseFont = True
        Me.SUMdebit.AppearanceCell.Options.UseForeColor = True
        Me.SUMdebit.Caption = " حركة المدين"
        Me.SUMdebit.FieldName = "debet"
        Me.SUMdebit.MinWidth = 16
        Me.SUMdebit.Name = "SUMdebit"
        Me.SUMdebit.Visible = True
        Me.SUMdebit.VisibleIndex = 2
        Me.SUMdebit.Width = 130
        '
        'total
        '
        Me.total.AppearanceCell.BackColor = System.Drawing.Color.White
        Me.total.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.total.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText
        Me.total.AppearanceCell.Options.UseBackColor = True
        Me.total.AppearanceCell.Options.UseFont = True
        Me.total.AppearanceCell.Options.UseForeColor = True
        Me.total.Caption = "اجمالي الدائن"
        Me.total.FieldName = "total"
        Me.total.MinWidth = 16
        Me.total.Name = "total"
        Me.total.Width = 196
        '
        'Noetes
        '
        Me.Noetes.AppearanceCell.BackColor = System.Drawing.Color.White
        Me.Noetes.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Noetes.AppearanceCell.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText
        Me.Noetes.AppearanceCell.Options.UseBackColor = True
        Me.Noetes.AppearanceCell.Options.UseFont = True
        Me.Noetes.AppearanceCell.Options.UseForeColor = True
        Me.Noetes.Caption = "اجمالي المدين"
        Me.Noetes.FieldName = "Noetes"
        Me.Noetes.MinWidth = 16
        Me.Noetes.Name = "Noetes"
        Me.Noetes.Width = 185
        '
        'ACCID
        '
        Me.ACCID.Caption = "رقم"
        Me.ACCID.FieldName = "ACCID"
        Me.ACCID.MinWidth = 16
        Me.ACCID.Name = "ACCID"
        Me.ACCID.Width = 60
        '
        'DT1
        '
        Me.DT1.EditValue = Nothing
        Me.DT1.Location = New System.Drawing.Point(780, 51)
        Me.DT1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.DT1.Name = "DT1"
        Me.DT1.Properties.Appearance.Options.UseTextOptions = True
        Me.DT1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DT1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DT1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DT1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DT1.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.DT1.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.DT1.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.DT1.Properties.Name = "DT1"
        Me.DT1.Properties.UseMaskAsDisplayFormat = True
        Me.DT1.Size = New System.Drawing.Size(272, 36)
        Me.DT1.StyleController = Me.LayoutControl2
        Me.DT1.TabIndex = 6
        '
        'DT2
        '
        Me.DT2.EditValue = Nothing
        Me.DT2.Location = New System.Drawing.Point(444, 51)
        Me.DT2.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.DT2.Name = "DT2"
        Me.DT2.Properties.Appearance.Options.UseTextOptions = True
        Me.DT2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DT2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DT2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DT2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DT2.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.DT2.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.DT2.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.DT2.Properties.Name = "DT1"
        Me.DT2.Properties.UseMaskAsDisplayFormat = True
        Me.DT2.Size = New System.Drawing.Size(277, 36)
        Me.DT2.StyleController = Me.LayoutControl2
        Me.DT2.TabIndex = 6
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton11.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton11.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton11.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton11.Location = New System.Drawing.Point(232, 51)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(206, 38)
        Me.SimpleButton11.StyleController = Me.LayoutControl2
        Me.SimpleButton11.TabIndex = 4
        Me.SimpleButton11.Text = "عرض "
        '
        'SimpleButton21
        '
        Me.SimpleButton21.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton21.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton21.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton21.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton21.Location = New System.Drawing.Point(32, 51)
        Me.SimpleButton21.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton21.Name = "SimpleButton21"
        Me.SimpleButton21.Size = New System.Drawing.Size(194, 38)
        Me.SimpleButton21.StyleController = Me.LayoutControl2
        Me.SimpleButton21.TabIndex = 5
        Me.SimpleButton21.Text = "طباعه"
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1513, 701)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.CustomizationFormText = "البيانات الاساسية"
        Me.LayoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem5, Me.LayoutControlItem1, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem9, Me.LayoutControlItem10})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(1487, 677)
        Me.LayoutControlGroup2.Text = "البيانات الاساسية"
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem5.Control = Me.BranchID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "الفــرع"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(1079, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(376, 44)
        Me.LayoutControlItem5.Text = "الفــرع"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(37, 28)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl11
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1455, 582)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem4.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem4.Control = Me.DT1
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "من"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(748, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(331, 44)
        Me.LayoutControlItem4.Text = "من"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(37, 28)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem3.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem3.Control = Me.DT2
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "إلى"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(412, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(336, 44)
        Me.LayoutControlItem3.Text = "إلى"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(37, 28)
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.SimpleButton11
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "LayoutControlItem9"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(200, 0)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(212, 44)
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.SimpleButton21
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "LayoutControlItem10"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(200, 44)
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem10.TextVisible = False
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
        'CustName
        '
        Me.CustName.Caption = "اسم الحساب "
        Me.CustName.FieldName = "CustName"
        Me.CustName.Name = "CustName"
        Me.CustName.Visible = True
        Me.CustName.VisibleIndex = 1
        Me.CustName.Width = 250
        '
        'Debit
        '
        Me.Debit.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.Debit.AppearanceCell.BorderColor = System.Drawing.Color.Transparent
        Me.Debit.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.Debit.AppearanceCell.Options.UseBackColor = True
        Me.Debit.AppearanceCell.Options.UseBorderColor = True
        Me.Debit.AppearanceCell.Options.UseForeColor = True
        Me.Debit.Caption = "أجمالي المدين "
        Me.Debit.FieldName = "Debit"
        Me.Debit.Name = "Debit"
        Me.Debit.Visible = True
        Me.Debit.VisibleIndex = 2
        Me.Debit.Width = 125
        '
        'Credit
        '
        Me.Credit.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.Credit.AppearanceCell.BorderColor = System.Drawing.Color.Transparent
        Me.Credit.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.Credit.AppearanceCell.Options.UseBackColor = True
        Me.Credit.AppearanceCell.Options.UseBorderColor = True
        Me.Credit.AppearanceCell.Options.UseForeColor = True
        Me.Credit.Caption = "إجمالي الدائن "
        Me.Credit.FieldName = "Credit"
        Me.Credit.Name = "Credit"
        Me.Credit.Visible = True
        Me.Credit.VisibleIndex = 3
        Me.Credit.Width = 125
        '
        'ACCNETtotel
        '
        Me.ACCNETtotel.AppearanceCell.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.ACCNETtotel.AppearanceCell.BorderColor = System.Drawing.Color.Yellow
        Me.ACCNETtotel.AppearanceCell.Options.UseBackColor = True
        Me.ACCNETtotel.AppearanceCell.Options.UseBorderColor = True
        Me.ACCNETtotel.Caption = "الرصيد "
        Me.ACCNETtotel.FieldName = "ACCNETtotel"
        Me.ACCNETtotel.Name = "ACCNETtotel"
        Me.ACCNETtotel.Visible = True
        Me.ACCNETtotel.VisibleIndex = 4
        Me.ACCNETtotel.Width = 127
        '
        'ID_Cu
        '
        Me.ID_Cu.Caption = "رقم الحساب "
        Me.ID_Cu.FieldName = "AccID"
        Me.ID_Cu.Name = "ID_Cu"
        '
        'TypeMov_cus
        '
        Me.TypeMov_cus.Caption = "نوع الحساب"
        Me.TypeMov_cus.FieldName = "TypeMov"
        Me.TypeMov_cus.Name = "TypeMov_cus"
        '
        'FRMDepositSafe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1518, 701)
        Me.Controls.Add(Me.panelLeft)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.BENEFIT
        Me.Name = "FRMDepositSafe"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "خزينة الودائع"
        Me.panelLeft.ResumeLayout(False)
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl2.ResumeLayout(False)
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panelLeft As Panel
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CustName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Debit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Credit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACCNETtotel As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ID_Cu As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TypeMov_cus As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControl2 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents GridControl11 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridColumn11 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACCicount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACCNAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents INSERCREDET As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents INSERTDEBIT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SUMcredat As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SUMdebit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents total As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Noetes As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACCID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DT1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DT2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton21 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
End Class
