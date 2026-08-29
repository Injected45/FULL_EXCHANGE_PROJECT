<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmDiscountsAndBonuses
    Inherits FrmMaster

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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDiscountsAndBonuses))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.AccName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.EmplloyID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DisVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BounsVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NotesDe = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Delete = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnDele = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.Type = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BonesOrDis = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Branch = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.BounsOrDis = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.MVal = New DevExpress.XtraEditors.SpinEdit()
        Me.Notes2 = New DevExpress.XtraEditors.TextEdit()
        Me.TypeID = New DevExpress.XtraEditors.LookUpEdit()
        Me.EmpID = New DevExpress.XtraEditors.LookUpEdit()
        Me.SimpleButton111 = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDele, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BounsOrDis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TypeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmpID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.BounsOrDis)
        Me.LayoutControl1.Controls.Add(Me.MVal)
        Me.LayoutControl1.Controls.Add(Me.Notes2)
        Me.LayoutControl1.Controls.Add(Me.TypeID)
        Me.LayoutControl1.Controls.Add(Me.EmpID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton111)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1940, 588)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(6, 3, 6, 3)
        Me.GCRole.Location = New System.Drawing.Point(43, 60)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GCRole.Name = "GCRole"
        Me.GCRole.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnDele})
        Me.GCRole.Size = New System.Drawing.Size(1320, 494)
        Me.GCRole.TabIndex = 22
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.AccName, Me.EmplloyID, Me.DisVal, Me.BounsVal, Me.NotesDe, Me.Delete, Me.Type, Me.BonesOrDis, Me.Branch, Me.SN})
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 1100
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'AccName
        '
        Me.AccName.Caption = "الموظف"
        Me.AccName.FieldName = "AccName"
        Me.AccName.MinWidth = 27
        Me.AccName.Name = "AccName"
        Me.AccName.Visible = True
        Me.AccName.VisibleIndex = 0
        Me.AccName.Width = 393
        '
        'EmplloyID
        '
        Me.EmplloyID.Caption = "رقم الموظف"
        Me.EmplloyID.FieldName = "EmplloyID"
        Me.EmplloyID.MinWidth = 27
        Me.EmplloyID.Name = "EmplloyID"
        Me.EmplloyID.Width = 103
        '
        'DisVal
        '
        Me.DisVal.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.DisVal.AppearanceCell.Options.UseBackColor = True
        Me.DisVal.Caption = "الخصم"
        Me.DisVal.FieldName = "DisVal"
        Me.DisVal.MinWidth = 27
        Me.DisVal.Name = "DisVal"
        Me.DisVal.UnboundDataType = GetType(Decimal)
        Me.DisVal.Visible = True
        Me.DisVal.VisibleIndex = 1
        Me.DisVal.Width = 212
        '
        'BounsVal
        '
        Me.BounsVal.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BounsVal.AppearanceCell.Options.UseBackColor = True
        Me.BounsVal.Caption = "العلاوة"
        Me.BounsVal.FieldName = "BounsVal"
        Me.BounsVal.MinWidth = 27
        Me.BounsVal.Name = "BounsVal"
        Me.BounsVal.UnboundDataType = GetType(Decimal)
        Me.BounsVal.Visible = True
        Me.BounsVal.VisibleIndex = 2
        Me.BounsVal.Width = 212
        '
        'NotesDe
        '
        Me.NotesDe.Caption = "ملاحظات"
        Me.NotesDe.FieldName = "NotesDe"
        Me.NotesDe.MinWidth = 27
        Me.NotesDe.Name = "NotesDe"
        Me.NotesDe.Visible = True
        Me.NotesDe.VisibleIndex = 3
        Me.NotesDe.Width = 315
        '
        'Delete
        '
        Me.Delete.Caption = "حذف"
        Me.Delete.ColumnEdit = Me.BtnDele
        Me.Delete.FieldName = "Delete"
        Me.Delete.MinWidth = 27
        Me.Delete.Name = "Delete"
        Me.Delete.Visible = True
        Me.Delete.VisibleIndex = 4
        Me.Delete.Width = 122
        '
        'BtnDele
        '
        Me.BtnDele.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = CType(resources.GetObject("EditorButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnDele.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnDele.Name = "BtnDele"
        Me.BtnDele.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'Type
        '
        Me.Type.Caption = "نوع العملية"
        Me.Type.FieldName = "Type"
        Me.Type.MinWidth = 27
        Me.Type.Name = "Type"
        Me.Type.Width = 103
        '
        'BonesOrDis
        '
        Me.BonesOrDis.Caption = "علاوة أو خصم"
        Me.BonesOrDis.FieldName = "BonesOrDis"
        Me.BonesOrDis.MinWidth = 27
        Me.BonesOrDis.Name = "BonesOrDis"
        Me.BonesOrDis.Width = 103
        '
        'Branch
        '
        Me.Branch.Caption = "الفرع"
        Me.Branch.FieldName = "Branch"
        Me.Branch.MinWidth = 27
        Me.Branch.Name = "Branch"
        Me.Branch.Width = 103
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.MinWidth = 27
        Me.SN.Name = "SN"
        Me.SN.Width = 103
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(1392, 69)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Properties.PopupSizeable = False
        Me.CurrencyID.Size = New System.Drawing.Size(455, 46)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 7
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(1392, 17)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(455, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 3
        '
        'BounsOrDis
        '
        Me.BounsOrDis.Location = New System.Drawing.Point(1392, 173)
        Me.BounsOrDis.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BounsOrDis.Name = "BounsOrDis"
        Me.BounsOrDis.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BounsOrDis.Properties.Items.AddRange(New Object() {"خصم", "علاوة"})
        Me.BounsOrDis.Size = New System.Drawing.Size(455, 46)
        Me.BounsOrDis.StyleController = Me.LayoutControl1
        Me.BounsOrDis.TabIndex = 4
        '
        'MVal
        '
        Me.MVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.MVal.Location = New System.Drawing.Point(1392, 277)
        Me.MVal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MVal.Name = "MVal"
        Me.MVal.Properties.AllowMouseWheel = False
        Me.MVal.Properties.Appearance.Options.UseTextOptions = True
        Me.MVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.MVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.MVal.Properties.MaskSettings.Set("mask", "n")
        Me.MVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.MVal.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.MVal.Properties.UseMaskAsDisplayFormat = True
        Me.MVal.Size = New System.Drawing.Size(455, 46)
        Me.MVal.StyleController = Me.LayoutControl1
        Me.MVal.TabIndex = 9
        '
        'Notes2
        '
        Me.Notes2.Location = New System.Drawing.Point(1452, 329)
        Me.Notes2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Notes2.Name = "Notes2"
        Me.Notes2.Properties.Appearance.Options.UseTextOptions = True
        Me.Notes2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Notes2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Notes2.Size = New System.Drawing.Size(400, 46)
        Me.Notes2.StyleController = Me.LayoutControl1
        Me.Notes2.TabIndex = 8
        '
        'TypeID
        '
        Me.TypeID.Location = New System.Drawing.Point(1392, 225)
        Me.TypeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TypeID.Name = "TypeID"
        Me.TypeID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TypeID.Properties.NullText = ""
        Me.TypeID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.TypeID.Properties.PopupSizeable = False
        Me.TypeID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.TypeID.Size = New System.Drawing.Size(455, 46)
        Me.TypeID.StyleController = Me.LayoutControl1
        Me.TypeID.TabIndex = 5
        '
        'EmpID
        '
        Me.EmpID.Location = New System.Drawing.Point(1392, 121)
        Me.EmpID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.EmpID.Name = "EmpID"
        Me.EmpID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.EmpID.Properties.NullText = ""
        Me.EmpID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.EmpID.Properties.PopupSizeable = False
        Me.EmpID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.EmpID.Size = New System.Drawing.Size(455, 46)
        Me.EmpID.StyleController = Me.LayoutControl1
        Me.EmpID.TabIndex = 6
        '
        'SimpleButton111
        '
        Me.SimpleButton111.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.SimpleButton111.Appearance.Options.UseBackColor = True
        Me.SimpleButton111.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton111.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.plus
        Me.SimpleButton111.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton111.Location = New System.Drawing.Point(1392, 329)
        Me.SimpleButton111.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton111.Name = "SimpleButton111"
        Me.SimpleButton111.Size = New System.Drawing.Size(52, 41)
        Me.SimpleButton111.StyleController = Me.LayoutControl1
        Me.SimpleButton111.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem21, Me.LayoutControlGroup4, Me.LayoutControlItem6, Me.LayoutControlItem12, Me.LayoutControlItem20, Me.LayoutControlItem9, Me.LayoutControlItem2, Me.LayoutControlItem5, Me.EmptySpaceItem1, Me.LayoutControlItem4})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1940, 588)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(1370, 0)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(534, 52)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(49, 27)
        '
        'LayoutControlGroup4
        '
        Me.LayoutControlGroup4.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup4.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup4.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup4.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup4.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup4.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlGroup4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup4.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup4.CustomizationFormText = " "
        Me.LayoutControlGroup4.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
        Me.LayoutControlGroup4.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup4.Size = New System.Drawing.Size(1370, 560)
        Me.LayoutControlGroup4.Text = " "
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCRole
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1328, 500)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.CurrencyID
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "الفرع"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(1370, 52)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(534, 52)
        Me.LayoutControlItem6.Text = "العملة"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(49, 27)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.MVal
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(1370, 260)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(534, 52)
        Me.LayoutControlItem12.Text = "القيمة"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(49, 27)
        '
        'LayoutControlItem20
        '
        Me.LayoutControlItem20.Control = Me.Notes2
        Me.LayoutControlItem20.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem20.CustomizationFormText = "الرمز"
        Me.LayoutControlItem20.Location = New System.Drawing.Point(1430, 312)
        Me.LayoutControlItem20.Name = "LayoutControlItem20"
        Me.LayoutControlItem20.Size = New System.Drawing.Size(474, 52)
        Me.LayoutControlItem20.Text = "الملاحظات"
        Me.LayoutControlItem20.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem20.TextSize = New System.Drawing.Size(61, 27)
        Me.LayoutControlItem20.TextToControlDistance = 5
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.SimpleButton111
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(1370, 312)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(60, 52)
        Me.LayoutControlItem9.Text = "LayoutControlItem4"
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BounsOrDis
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الفرع"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(1370, 156)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(534, 52)
        Me.LayoutControlItem2.Text = "العملية"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(49, 27)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.TypeID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "الفرع"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(1370, 208)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem5.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem5.Size = New System.Drawing.Size(534, 52)
        Me.LayoutControlItem5.Text = "النوع"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(49, 27)
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(1370, 364)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(534, 196)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.EmpID
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "الفرع"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(1370, 104)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem4.Size = New System.Drawing.Size(534, 52)
        Me.LayoutControlItem4.Text = "الموظف"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(49, 27)
        '
        'FrmDiscountsAndBonuses
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1940, 641)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = Global.ExchangeSystem.My.Resources.Resources.newtablestyle_32x32
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FrmDiscountsAndBonuses"
        Me.Text = "علاوات وخصومات جماعية"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDele, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BounsOrDis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TypeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmpID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BounsOrDis As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents MVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Notes2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TypeID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents EmpID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SimpleButton111 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents AccName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EmplloyID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DisVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BounsVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NotesDe As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Delete As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnDele As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents Type As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BonesOrDis As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Branch As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
End Class