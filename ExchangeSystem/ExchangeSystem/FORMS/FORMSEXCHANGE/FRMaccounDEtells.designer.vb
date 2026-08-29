Imports DevExpress

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMaccounDEtells
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMaccounDEtells))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl2 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.cerrns = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.total = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID_CRUNSE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TYpeCrence = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.IDB = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.typID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.insertdate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.safID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.debit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CREDET = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.totel = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.noets = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.dt2 = New DevExpress.XtraEditors.DateEdit()
        Me.DT1 = New DevExpress.XtraEditors.DateEdit()
        Me.ACCCODE = New DevExpress.XtraEditors.TextEdit()
        Me.ACCNAME = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.sumleabel = New DevExpress.XtraLayout.SimpleLabelItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.smblabecredetl = New DevExpress.XtraEditors.LabelControl()
        Me.sumlabeldebit = New DevExpress.XtraEditors.LabelControl()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dt2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dt2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ACCCODE.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ACCNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sumleabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl2)
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.dt2)
        Me.LayoutControl1.Controls.Add(Me.DT1)
        Me.LayoutControl1.Controls.Add(Me.ACCCODE)
        Me.LayoutControl1.Controls.Add(Me.ACCNAME)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1342, 714)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl2
        '
        Me.GridControl2.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GridControl2.Location = New System.Drawing.Point(16, 16)
        Me.GridControl2.MainView = Me.GridView1
        Me.GridControl2.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GridControl2.Name = "GridControl2"
        Me.GridControl2.ShowOnlyPredefinedDetails = True
        Me.GridControl2.Size = New System.Drawing.Size(589, 223)
        Me.GridControl2.TabIndex = 14
        Me.GridControl2.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.cerrns, Me.total, Me.ID_CRUNSE, Me.TYpeCrence})
        Me.GridView1.DetailHeight = 294
        Me.GridView1.GridControl = Me.GridControl2
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditForm
        Me.GridView1.OptionsBehavior.ReadOnly = True
        Me.GridView1.OptionsEditForm.PopupEditFormWidth = 640
        Me.GridView1.OptionsFilter.AllowFilterEditor = False
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GridView1.OptionsSelection.EnableAppearanceHideSelection = False
        Me.GridView1.OptionsSelection.EnableAppearanceHotTrackedRow = DevExpress.Utils.DefaultBoolean.[False]
        '
        'cerrns
        '
        Me.cerrns.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cerrns.AppearanceCell.Options.UseFont = True
        Me.cerrns.AppearanceHeader.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cerrns.AppearanceHeader.Options.UseFont = True
        Me.cerrns.Caption = "العلمة"
        Me.cerrns.FieldName = "cerrns"
        Me.cerrns.MinWidth = 16
        Me.cerrns.Name = "cerrns"
        Me.cerrns.Visible = True
        Me.cerrns.VisibleIndex = 0
        Me.cerrns.Width = 60
        '
        'total
        '
        Me.total.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.total.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.total.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.total.AppearanceCell.Options.UseBackColor = True
        Me.total.AppearanceCell.Options.UseFont = True
        Me.total.AppearanceCell.Options.UseForeColor = True
        Me.total.AppearanceHeader.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.total.AppearanceHeader.Options.UseFont = True
        Me.total.Caption = "الرصيد"
        Me.total.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.total.FieldName = "total"
        Me.total.MinWidth = 16
        Me.total.Name = "total"
        Me.total.Visible = True
        Me.total.VisibleIndex = 1
        Me.total.Width = 60
        '
        'ID_CRUNSE
        '
        Me.ID_CRUNSE.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ID_CRUNSE.AppearanceCell.Options.UseFont = True
        Me.ID_CRUNSE.AppearanceHeader.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ID_CRUNSE.AppearanceHeader.Options.UseFont = True
        Me.ID_CRUNSE.Caption = "رقم العملة"
        Me.ID_CRUNSE.FieldName = "ID_CRUNSE"
        Me.ID_CRUNSE.Name = "ID_CRUNSE"
        '
        'TYpeCrence
        '
        Me.TYpeCrence.Caption = "نوع العملة"
        Me.TYpeCrence.FieldName = "TYpeCrence"
        Me.TYpeCrence.Name = "TYpeCrence"
        '
        'GridControl1
        '
        Me.GridControl1.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GridControl1.Location = New System.Drawing.Point(16, 245)
        Me.GridControl1.MainView = Me.GVRole
        Me.GridControl1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1310, 453)
        Me.GridControl1.TabIndex = 9
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.IDB, Me.SN, Me.Code, Me.BName, Me.typID, Me.insertdate, Me.safID, Me.debit, Me.CREDET, Me.totel, Me.noets})
        Me.GVRole.DetailHeight = 294
        Me.GVRole.GridControl = Me.GridControl1
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 640
        Me.GVRole.OptionsFind.AlwaysVisible = True
        Me.GVRole.OptionsFind.Behavior = DevExpress.XtraEditors.FindPanelBehavior.Filter
        Me.GVRole.OptionsFind.FindFilterColumns = ""
        Me.GVRole.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always
        Me.GVRole.OptionsFind.FindNullPrompt = "ابحث هنا ..."
        Me.GVRole.OptionsFind.FindPanelLocation = DevExpress.XtraGrid.Views.Grid.GridFindPanelLocation.Panel
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'IDB
        '
        Me.IDB.Caption = "GridColumn1"
        Me.IDB.FieldName = "IDB"
        Me.IDB.MinWidth = 16
        Me.IDB.Name = "IDB"
        Me.IDB.Width = 60
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.MinWidth = 16
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 82
        '
        'Code
        '
        Me.Code.Caption = "كود المعاملة"
        Me.Code.FieldName = "Code"
        Me.Code.MinWidth = 16
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 156
        '
        'BName
        '
        Me.BName.Caption = "اسم الحساب"
        Me.BName.FieldName = "BName"
        Me.BName.MinWidth = 16
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 2
        Me.BName.Width = 412
        '
        'typID
        '
        Me.typID.Caption = "نوع العملية"
        Me.typID.FieldName = "typID"
        Me.typID.MinWidth = 16
        Me.typID.Name = "typID"
        Me.typID.Width = 118
        '
        'insertdate
        '
        Me.insertdate.Caption = "تاريخ "
        Me.insertdate.FieldName = "insertdate"
        Me.insertdate.MinWidth = 16
        Me.insertdate.Name = "insertdate"
        Me.insertdate.Visible = True
        Me.insertdate.VisibleIndex = 7
        Me.insertdate.Width = 213
        '
        'safID
        '
        Me.safID.Caption = "اسم المستخدم"
        Me.safID.FieldName = "safID"
        Me.safID.MinWidth = 16
        Me.safID.Name = "safID"
        Me.safID.Visible = True
        Me.safID.VisibleIndex = 6
        Me.safID.Width = 121
        '
        'debit
        '
        Me.debit.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.debit.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.debit.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.debit.AppearanceCell.Options.UseBackColor = True
        Me.debit.AppearanceCell.Options.UseFont = True
        Me.debit.AppearanceCell.Options.UseForeColor = True
        Me.debit.Caption = "المدين"
        Me.debit.FieldName = "debit"
        Me.debit.MinWidth = 16
        Me.debit.Name = "debit"
        Me.debit.Visible = True
        Me.debit.VisibleIndex = 3
        Me.debit.Width = 154
        '
        'CREDET
        '
        Me.CREDET.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.CREDET.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CREDET.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.CREDET.AppearanceCell.Options.UseBackColor = True
        Me.CREDET.AppearanceCell.Options.UseFont = True
        Me.CREDET.AppearanceCell.Options.UseForeColor = True
        Me.CREDET.Caption = "الدائن"
        Me.CREDET.FieldName = "CREDET"
        Me.CREDET.MinWidth = 16
        Me.CREDET.Name = "CREDET"
        Me.CREDET.Visible = True
        Me.CREDET.VisibleIndex = 4
        Me.CREDET.Width = 154
        '
        'totel
        '
        Me.totel.AppearanceCell.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.totel.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.totel.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.totel.AppearanceCell.Options.UseBackColor = True
        Me.totel.AppearanceCell.Options.UseFont = True
        Me.totel.AppearanceCell.Options.UseForeColor = True
        Me.totel.Caption = "الرصيد"
        Me.totel.FieldName = "totel"
        Me.totel.MinWidth = 16
        Me.totel.Name = "totel"
        Me.totel.Visible = True
        Me.totel.VisibleIndex = 5
        Me.totel.Width = 120
        '
        'noets
        '
        Me.noets.Caption = "ملاحضات"
        Me.noets.FieldName = "noets"
        Me.noets.MinWidth = 16
        Me.noets.Name = "noets"
        Me.noets.Width = 158
        '
        'dt2
        '
        Me.dt2.EditValue = Nothing
        Me.dt2.Location = New System.Drawing.Point(648, 143)
        Me.dt2.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.dt2.Name = "dt2"
        Me.dt2.Properties.Appearance.Options.UseTextOptions = True
        Me.dt2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.dt2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.dt2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dt2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dt2.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.dt2.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.dt2.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.dt2.Properties.UseMaskAsDisplayFormat = True
        Me.dt2.Size = New System.Drawing.Size(301, 36)
        Me.dt2.TabIndex = 4
        '
        'DT1
        '
        Me.DT1.EditValue = Nothing
        Me.DT1.Location = New System.Drawing.Point(977, 143)
        Me.DT1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.DT1.Name = "DT1"
        Me.DT1.Properties.Appearance.Options.UseTextOptions = True
        Me.DT1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DT1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DT1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DT1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DT1.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.DT1.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.DT1.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.DT1.Properties.UseMaskAsDisplayFormat = True
        Me.DT1.Size = New System.Drawing.Size(259, 36)
        Me.DT1.TabIndex = 5
        '
        'ACCCODE
        '
        Me.ACCCODE.Location = New System.Drawing.Point(648, 59)
        Me.ACCCODE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ACCCODE.Name = "ACCCODE"
        Me.ACCCODE.Properties.Appearance.Options.UseTextOptions = True
        Me.ACCCODE.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ACCCODE.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ACCCODE.Properties.ReadOnly = True
        Me.ACCCODE.Size = New System.Drawing.Size(588, 36)
        Me.ACCCODE.TabIndex = 12
        '
        'ACCNAME
        '
        Me.ACCNAME.Location = New System.Drawing.Point(648, 101)
        Me.ACCNAME.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ACCNAME.Name = "ACCNAME"
        Me.ACCNAME.Properties.Appearance.Options.UseTextOptions = True
        Me.ACCNAME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ACCNAME.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ACCNAME.Properties.ReadOnly = True
        Me.ACCNAME.Size = New System.Drawing.Size(588, 36)
        Me.ACCNAME.TabIndex = 13
        '
        'SimpleButton1
        '
        Me.SimpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton1.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1.Location = New System.Drawing.Point(868, 185)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(180, 38)
        Me.SimpleButton1.TabIndex = 15
        Me.SimpleButton1.Text = "طباعة"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton2.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton2.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton2.Location = New System.Drawing.Point(1054, 185)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(179, 38)
        Me.SimpleButton2.TabIndex = 16
        Me.SimpleButton2.Text = "عرض "
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem6, Me.LayoutControlGroup1, Me.LayoutControlItem7, Me.EmptySpaceItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1342, 714)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.GridControl1
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 229)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(1316, 459)
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.BorderColor = System.Drawing.Color.White
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup1.AppearanceItemCaption.BorderColor = System.Drawing.Color.White
        Me.LayoutControlGroup1.AppearanceItemCaption.Options.UseBorderColor = True
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem5, Me.LayoutControlItem2, Me.LayoutControlItem8, Me.sumleabel, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(616, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(700, 229)
        Me.LayoutControlGroup1.Text = " "
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.Control = Me.dt2
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(329, 42)
        Me.LayoutControlItem1.Text = "الي"
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(17, 21)
        Me.LayoutControlItem1.TextToControlDistance = 5
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.ACCNAME
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(668, 42)
        Me.LayoutControlItem5.Text = "اسم الحساب"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem2.Control = Me.DT1
        Me.LayoutControlItem2.Location = New System.Drawing.Point(329, 84)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(339, 42)
        Me.LayoutControlItem2.Text = "من "
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.SimpleButton2
        Me.LayoutControlItem8.Location = New System.Drawing.Point(406, 126)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 80, 3, 3)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(262, 44)
        Me.LayoutControlItem8.TextVisible = False
        '
        'sumleabel
        '
        Me.sumleabel.AppearanceItemCaption.BackColor = System.Drawing.Color.Red
        Me.sumleabel.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sumleabel.AppearanceItemCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        Me.sumleabel.AppearanceItemCaption.ForeColor = System.Drawing.Color.Yellow
        Me.sumleabel.AppearanceItemCaption.Options.UseBackColor = True
        Me.sumleabel.AppearanceItemCaption.Options.UseFont = True
        Me.sumleabel.AppearanceItemCaption.Options.UseForeColor = True
        Me.sumleabel.AppearanceItemCaption.Options.UseTextOptions = True
        Me.sumleabel.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.sumleabel.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.sumleabel.AutoSizeMode = DevExpress.XtraLayout.SimpleLabelAutoSizeMode.None
        Me.sumleabel.Location = New System.Drawing.Point(0, 126)
        Me.sumleabel.Name = "sumleabel"
        Me.sumleabel.Size = New System.Drawing.Size(220, 44)
        Me.sumleabel.Text = "0.00"
        Me.sumleabel.TextSize = New System.Drawing.Size(58, 36)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.ACCCODE
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(668, 42)
        Me.LayoutControlItem3.Text = "كشف حركة "
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(58, 21)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton1
        Me.LayoutControlItem4.Location = New System.Drawing.Point(220, 126)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(186, 44)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.GridControl2
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(595, 229)
        Me.LayoutControlItem7.TextVisible = False
        '
        'EmptySpaceItem2
        '
        Me.EmptySpaceItem2.Location = New System.Drawing.Point(595, 0)
        Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Size = New System.Drawing.Size(21, 229)
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Panel1.Controls.Add(Me.smblabecredetl)
        Me.Panel1.Controls.Add(Me.sumlabeldebit)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 714)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1342, 93)
        Me.Panel1.TabIndex = 17
        '
        'smblabecredetl
        '
        Me.smblabecredetl.Appearance.BackColor = System.Drawing.Color.Green
        Me.smblabecredetl.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 14.0!, System.Drawing.FontStyle.Bold)
        Me.smblabecredetl.Appearance.ForeColor = System.Drawing.Color.Yellow
        Me.smblabecredetl.Appearance.Options.UseBackColor = True
        Me.smblabecredetl.Appearance.Options.UseFont = True
        Me.smblabecredetl.Appearance.Options.UseForeColor = True
        Me.smblabecredetl.Appearance.Options.UseTextOptions = True
        Me.smblabecredetl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.smblabecredetl.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.smblabecredetl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.smblabecredetl.Location = New System.Drawing.Point(795, 0)
        Me.smblabecredetl.Name = "smblabecredetl"
        Me.smblabecredetl.Size = New System.Drawing.Size(235, 46)
        Me.smblabecredetl.TabIndex = 1
        Me.smblabecredetl.Text = "0.00"
        '
        'sumlabeldebit
        '
        Me.sumlabeldebit.Appearance.BackColor = System.Drawing.Color.Red
        Me.sumlabeldebit.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 14.0!, System.Drawing.FontStyle.Bold)
        Me.sumlabeldebit.Appearance.ForeColor = System.Drawing.Color.Yellow
        Me.sumlabeldebit.Appearance.Options.UseBackColor = True
        Me.sumlabeldebit.Appearance.Options.UseFont = True
        Me.sumlabeldebit.Appearance.Options.UseForeColor = True
        Me.sumlabeldebit.Appearance.Options.UseTextOptions = True
        Me.sumlabeldebit.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.sumlabeldebit.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.sumlabeldebit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.sumlabeldebit.Location = New System.Drawing.Point(1035, 0)
        Me.sumlabeldebit.Name = "sumlabeldebit"
        Me.sumlabeldebit.Size = New System.Drawing.Size(235, 46)
        Me.sumlabeldebit.TabIndex = 0
        Me.sumlabeldebit.Text = "0.00"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.LayoutControl1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1342, 714)
        Me.Panel2.TabIndex = 18
        '
        'FRMaccounDEtells
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1342, 807)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.IconOptions.LargeImage = CType(resources.GetObject("FRMaccounDEtells.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.MaximizeBox = False
        Me.Name = "FRMaccounDEtells"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "عرض حساب"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dt2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dt2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ACCCODE.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ACCNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sumleabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As XtraLayout.LayoutControl
    Friend WithEvents dt2 As XtraEditors.DateEdit
    Friend WithEvents DT1 As XtraEditors.DateEdit
    Friend WithEvents Root As XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As XtraLayout.LayoutControlItem
    Friend WithEvents GridControl1 As XtraGrid.GridControl
    Friend WithEvents GVRole As XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem6 As XtraLayout.LayoutControlItem
    Friend WithEvents SN As XtraGrid.Columns.GridColumn
    Friend WithEvents Code As XtraGrid.Columns.GridColumn
    Friend WithEvents BName As XtraGrid.Columns.GridColumn
    Friend WithEvents debit As XtraGrid.Columns.GridColumn
    Friend WithEvents CREDET As XtraGrid.Columns.GridColumn
    Friend WithEvents IDB As XtraGrid.Columns.GridColumn
    Friend WithEvents typID As XtraGrid.Columns.GridColumn
    Friend WithEvents insertdate As XtraGrid.Columns.GridColumn
    Friend WithEvents safID As XtraGrid.Columns.GridColumn
    Friend WithEvents totel As XtraGrid.Columns.GridColumn
    Friend WithEvents noets As XtraGrid.Columns.GridColumn
    Friend WithEvents ACCCODE As XtraEditors.TextEdit
    Friend WithEvents LayoutControlGroup1 As XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As XtraLayout.LayoutControlItem
    Friend WithEvents GridControl2 As XtraGrid.GridControl
    Friend WithEvents GridView1 As XtraGrid.Views.Grid.GridView
    Friend WithEvents ACCNAME As XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As XtraLayout.LayoutControlItem
    Friend WithEvents cerrns As XtraGrid.Columns.GridColumn
    Friend WithEvents total As XtraGrid.Columns.GridColumn
    Friend WithEvents EmptySpaceItem2 As XtraLayout.EmptySpaceItem
    Friend WithEvents sumleabel As XtraLayout.SimpleLabelItem
    Friend WithEvents SimpleButton1 As XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As XtraLayout.LayoutControlItem
    Friend WithEvents ID_CRUNSE As XtraGrid.Columns.GridColumn
    Friend WithEvents Panel1 As Panel
    Friend WithEvents smblabecredetl As XtraEditors.LabelControl
    Friend WithEvents sumlabeldebit As XtraEditors.LabelControl
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TYpeCrence As XtraGrid.Columns.GridColumn
End Class
