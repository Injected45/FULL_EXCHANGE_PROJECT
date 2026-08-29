<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMBENEFITGROUPS
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
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.ISID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NumID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnNuID = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.NumRatio = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnNumRatio = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.GName = New DevExpress.XtraEditors.TextEdit()
        Me.NuRatio = New DevExpress.XtraEditors.SpinEdit()
        Me.NuID = New DevExpress.XtraEditors.SpinEdit()
        Me.SEARCHTXT = New DevExpress.XtraEditors.TextEdit()
        Me.NuID1 = New DevExpress.XtraEditors.SpinEdit()
        Me.NuRatio1 = New DevExpress.XtraEditors.SpinEdit()
        Me.NuID2 = New DevExpress.XtraEditors.SpinEdit()
        Me.NuRatio2 = New DevExpress.XtraEditors.SpinEdit()
        Me.GNum = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnNuID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnNumRatio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuRatio.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEARCHTXT.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuID1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuRatio1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuID2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuRatio2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GNum.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.GName)
        Me.LayoutControl1.Controls.Add(Me.NuRatio)
        Me.LayoutControl1.Controls.Add(Me.NuID)
        Me.LayoutControl1.Controls.Add(Me.SEARCHTXT)
        Me.LayoutControl1.Controls.Add(Me.NuID1)
        Me.LayoutControl1.Controls.Add(Me.NuRatio1)
        Me.LayoutControl1.Controls.Add(Me.NuID2)
        Me.LayoutControl1.Controls.Add(Me.NuRatio2)
        Me.LayoutControl1.Controls.Add(Me.GNum)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsFocus.EnableAutoTabOrder = False
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(637, 403)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCROLE
        '
        Me.GCROLE.Location = New System.Drawing.Point(16, 226)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnNuID, Me.BtnNumRatio})
        Me.GCROLE.Size = New System.Drawing.Size(605, 161)
        Me.GCROLE.TabIndex = 6
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(167, Byte), Integer))
        Me.GVROLE.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(167, Byte), Integer))
        Me.GVROLE.Appearance.HeaderPanel.Options.UseBackColor = True
        Me.GVROLE.Appearance.HeaderPanel.Options.UseBorderColor = True
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ISID, Me.NumID, Me.NumRatio})
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsFind.AllowFindPanel = False
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'ISID
        '
        Me.ISID.Caption = "الرمز"
        Me.ISID.FieldName = "ISID"
        Me.ISID.Name = "ISID"
        Me.ISID.Visible = True
        Me.ISID.VisibleIndex = 0
        '
        'NumID
        '
        Me.NumID.Caption = "الرقم"
        Me.NumID.ColumnEdit = Me.BtnNuID
        Me.NumID.FieldName = "NumID"
        Me.NumID.Name = "NumID"
        Me.NumID.Visible = True
        Me.NumID.VisibleIndex = 1
        '
        'BtnNuID
        '
        Me.BtnNuID.AutoHeight = False
        Me.BtnNuID.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnNuID.MaskSettings.Set("mask", "d")
        Me.BtnNuID.MaskSettings.Set("hideInsignificantZeros", False)
        Me.BtnNuID.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.BtnNuID.Name = "BtnNuID"
        Me.BtnNuID.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.BtnNuID.UseMaskAsDisplayFormat = True
        '
        'NumRatio
        '
        Me.NumRatio.Caption = "النسبة"
        Me.NumRatio.ColumnEdit = Me.BtnNumRatio
        Me.NumRatio.FieldName = "NumRatio"
        Me.NumRatio.Name = "NumRatio"
        Me.NumRatio.Visible = True
        Me.NumRatio.VisibleIndex = 2
        '
        'BtnNumRatio
        '
        Me.BtnNumRatio.AutoHeight = False
        Me.BtnNumRatio.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnNumRatio.MaskSettings.Set("mask", "n")
        Me.BtnNumRatio.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.BtnNumRatio.MaskSettings.Set("hideInsignificantZeros", False)
        Me.BtnNumRatio.Name = "BtnNumRatio"
        Me.BtnNumRatio.UseMaskAsDisplayFormat = True
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(312, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Properties.ReadOnly = True
        Me.CodeID.Size = New System.Drawing.Size(219, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 5
        '
        'GName
        '
        Me.GName.Location = New System.Drawing.Point(312, 58)
        Me.GName.Name = "GName"
        Me.GName.Properties.Appearance.Options.UseTextOptions = True
        Me.GName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GName.Size = New System.Drawing.Size(219, 36)
        Me.GName.StyleController = Me.LayoutControl1
        Me.GName.TabIndex = 0
        '
        'NuRatio
        '
        Me.NuRatio.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuRatio.Location = New System.Drawing.Point(16, 100)
        Me.NuRatio.Name = "NuRatio"
        Me.NuRatio.Properties.Appearance.Options.UseTextOptions = True
        Me.NuRatio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuRatio.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuRatio.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuRatio.Properties.MaskSettings.Set("mask", "f")
        Me.NuRatio.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuRatio.Properties.MaskSettings.Set("hideInsignificantZeros", Nothing)
        Me.NuRatio.Properties.UseMaskAsDisplayFormat = True
        Me.NuRatio.Size = New System.Drawing.Size(200, 36)
        Me.NuRatio.StyleController = Me.LayoutControl1
        Me.NuRatio.TabIndex = 3
        '
        'NuID
        '
        Me.NuID.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuID.Location = New System.Drawing.Point(312, 100)
        Me.NuID.Name = "NuID"
        Me.NuID.Properties.Appearance.Options.UseTextOptions = True
        Me.NuID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuID.Properties.MaskSettings.Set("mask", "d")
        Me.NuID.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuID.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.NuID.Properties.UseMaskAsDisplayFormat = True
        Me.NuID.Size = New System.Drawing.Size(219, 36)
        Me.NuID.StyleController = Me.LayoutControl1
        Me.NuID.TabIndex = 2
        '
        'SEARCHTXT
        '
        Me.SEARCHTXT.Location = New System.Drawing.Point(16, 16)
        Me.SEARCHTXT.Name = "SEARCHTXT"
        Me.SEARCHTXT.Properties.Appearance.Options.UseTextOptions = True
        Me.SEARCHTXT.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SEARCHTXT.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SEARCHTXT.Size = New System.Drawing.Size(200, 36)
        Me.SEARCHTXT.StyleController = Me.LayoutControl1
        Me.SEARCHTXT.TabIndex = 0
        '
        'NuID1
        '
        Me.NuID1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuID1.Location = New System.Drawing.Point(312, 142)
        Me.NuID1.Name = "NuID1"
        Me.NuID1.Properties.Appearance.Options.UseTextOptions = True
        Me.NuID1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuID1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuID1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuID1.Properties.MaskSettings.Set("mask", "d")
        Me.NuID1.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuID1.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.NuID1.Properties.UseMaskAsDisplayFormat = True
        Me.NuID1.Size = New System.Drawing.Size(219, 36)
        Me.NuID1.StyleController = Me.LayoutControl1
        Me.NuID1.TabIndex = 2
        '
        'NuRatio1
        '
        Me.NuRatio1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuRatio1.Location = New System.Drawing.Point(16, 142)
        Me.NuRatio1.Name = "NuRatio1"
        Me.NuRatio1.Properties.Appearance.Options.UseTextOptions = True
        Me.NuRatio1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuRatio1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuRatio1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuRatio1.Properties.MaskSettings.Set("mask", "f")
        Me.NuRatio1.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuRatio1.Properties.MaskSettings.Set("hideInsignificantZeros", Nothing)
        Me.NuRatio1.Properties.UseMaskAsDisplayFormat = True
        Me.NuRatio1.Size = New System.Drawing.Size(200, 36)
        Me.NuRatio1.StyleController = Me.LayoutControl1
        Me.NuRatio1.TabIndex = 3
        '
        'NuID2
        '
        Me.NuID2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuID2.Location = New System.Drawing.Point(312, 184)
        Me.NuID2.Name = "NuID2"
        Me.NuID2.Properties.Appearance.Options.UseTextOptions = True
        Me.NuID2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuID2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuID2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuID2.Properties.MaskSettings.Set("mask", "d")
        Me.NuID2.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuID2.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.NuID2.Properties.UseMaskAsDisplayFormat = True
        Me.NuID2.Size = New System.Drawing.Size(219, 36)
        Me.NuID2.StyleController = Me.LayoutControl1
        Me.NuID2.TabIndex = 2
        '
        'NuRatio2
        '
        Me.NuRatio2.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuRatio2.Location = New System.Drawing.Point(16, 184)
        Me.NuRatio2.Name = "NuRatio2"
        Me.NuRatio2.Properties.Appearance.Options.UseTextOptions = True
        Me.NuRatio2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuRatio2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuRatio2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuRatio2.Properties.MaskSettings.Set("mask", "f")
        Me.NuRatio2.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuRatio2.Properties.MaskSettings.Set("hideInsignificantZeros", Nothing)
        Me.NuRatio2.Properties.UseMaskAsDisplayFormat = True
        Me.NuRatio2.Size = New System.Drawing.Size(200, 36)
        Me.NuRatio2.StyleController = Me.LayoutControl1
        Me.NuRatio2.TabIndex = 3
        '
        'GNum
        '
        Me.GNum.EditValue = ""
        Me.GNum.Location = New System.Drawing.Point(16, 58)
        Me.GNum.Name = "GNum"
        Me.GNum.Properties.Appearance.Options.UseTextOptions = True
        Me.GNum.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GNum.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GNum.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.GNum.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered
        Me.GNum.Properties.Items.AddRange(New Object() {"فرع", "وسيط"})
        Me.GNum.Size = New System.Drawing.Size(200, 36)
        Me.GNum.StyleController = Me.LayoutControl1
        Me.GNum.TabIndex = 1
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem12, Me.LayoutControlItem6, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.LayoutControlItem9, Me.LayoutControlItem10})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(637, 403)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(296, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(296, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem2.Text = "اسم المجموعة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.NuID
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "الراتب"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(296, 84)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem4.Text = "الفرع الوسيط"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.NuRatio
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "الراتب"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(296, 42)
        Me.LayoutControlItem3.Text = "النسبة"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.GCROLE
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 210)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(611, 167)
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.GNum
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(296, 42)
        Me.LayoutControlItem12.Text = "النوع"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SEARCHTXT
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem6.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.LayoutControlItem6.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.searchdata
        Me.LayoutControlItem6.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(296, 42)
        Me.LayoutControlItem6.Text = " "
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Right
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.NuID1
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "الراتب"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(296, 126)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem7.Text = "الفرع الثاني"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.NuRatio1
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "الراتب"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(296, 42)
        Me.LayoutControlItem8.Text = "النسبة"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.NuID2
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "الراتب"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(296, 168)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem9.Text = "الفرع"
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.NuRatio2
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "الراتب"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 168)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(296, 42)
        Me.LayoutControlItem10.Text = "النسبة"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(74, 22)
        '
        'FRMBENEFITGROUPS
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 447)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.percentage
        Me.Name = "FRMBENEFITGROUPS"
        Me.Text = "نموذج تقسيم العمولة للفروع"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnNuID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnNumRatio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuRatio.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEARCHTXT.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuID1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuRatio1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuID2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuRatio2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GNum.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents NuRatio As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents NuID As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ISID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NumID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NumRatio As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SEARCHTXT As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnNuID As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents BtnNumRatio As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents NuID1 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents NuRatio1 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents NuID2 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents NuRatio2 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents GNum As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
End Class
