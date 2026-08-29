<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMultiAcountEdit
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMultiAcountEdit))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.DateEdit11 = New DevExpress.XtraEditors.DateEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.FirstAccMain = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.SecondAccMain = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.OverAllTotal = New DevExpress.XtraEditors.SpinEdit()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Debit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Credit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NotesDe = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Delete = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnDele = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccIDTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Branch = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Notes2 = New DevExpress.XtraEditors.TextEdit()
        Me.ValueType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.FirstAccParent = New DevExpress.XtraEditors.LookUpEdit()
        Me.FirstAccID = New DevExpress.XtraEditors.LookUpEdit()
        Me.SecondAccParent = New DevExpress.XtraEditors.LookUpEdit()
        Me.SecondAccID = New DevExpress.XtraEditors.LookUpEdit()
        Me.SimpleButton111 = New DevExpress.XtraEditors.SimpleButton()
        Me.MovmentType = New DevExpress.XtraEditors.TextEdit()
        Me.AccVal = New DevExpress.XtraEditors.SpinEdit()
        Me.BranchIDTo = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlGroup4 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit11.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit11.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FirstAccMain.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SecondAccMain.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDele, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Notes2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ValueType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FirstAccParent.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FirstAccID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SecondAccParent.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SecondAccID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MovmentType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AccVal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchIDTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.DateEdit11)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.FirstAccMain)
        Me.LayoutControl1.Controls.Add(Me.SecondAccMain)
        Me.LayoutControl1.Controls.Add(Me.OverAllTotal)
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.Notes2)
        Me.LayoutControl1.Controls.Add(Me.ValueType)
        Me.LayoutControl1.Controls.Add(Me.FirstAccParent)
        Me.LayoutControl1.Controls.Add(Me.FirstAccID)
        Me.LayoutControl1.Controls.Add(Me.SecondAccParent)
        Me.LayoutControl1.Controls.Add(Me.SecondAccID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton111)
        Me.LayoutControl1.Controls.Add(Me.MovmentType)
        Me.LayoutControl1.Controls.Add(Me.AccVal)
        Me.LayoutControl1.Controls.Add(Me.BranchIDTo)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1940, 821)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(1412, -92)
        Me.Code.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.ReadOnly = True
        Me.Code.Size = New System.Drawing.Size(400, 46)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 0
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(1351, 64)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Properties.PopupSizeable = False
        Me.CurrencyID.Size = New System.Drawing.Size(461, 46)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 7
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(1351, 12)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(461, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 3
        '
        'DateEdit11
        '
        Me.DateEdit11.EditValue = Nothing
        Me.DateEdit11.Enabled = False
        Me.DateEdit11.Location = New System.Drawing.Point(1351, -40)
        Me.DateEdit11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.DateEdit11.Name = "DateEdit11"
        Me.DateEdit11.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit11.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit11.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.DateEdit11.Properties.ReadOnly = True
        Me.DateEdit11.Properties.UseMaskAsDisplayFormat = True
        Me.DateEdit11.Size = New System.Drawing.Size(461, 46)
        Me.DateEdit11.StyleController = Me.LayoutControl1
        Me.DateEdit11.TabIndex = 12
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(1351, -92)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(53, 41)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'FirstAccMain
        '
        Me.FirstAccMain.Location = New System.Drawing.Point(1372, 211)
        Me.FirstAccMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.FirstAccMain.Name = "FirstAccMain"
        Me.FirstAccMain.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.FirstAccMain.Properties.Items.AddRange(New Object() {"الأصول", "الخصوم", "المصروفات", "الإيرادات"})
        Me.FirstAccMain.Size = New System.Drawing.Size(419, 46)
        Me.FirstAccMain.StyleController = Me.LayoutControl1
        Me.FirstAccMain.TabIndex = 4
        '
        'SecondAccMain
        '
        Me.SecondAccMain.Location = New System.Drawing.Point(1372, 533)
        Me.SecondAccMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SecondAccMain.Name = "SecondAccMain"
        Me.SecondAccMain.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SecondAccMain.Properties.Items.AddRange(New Object() {"الأصول", "الخصوم", "المصروفات", "الإيرادات"})
        Me.SecondAccMain.Size = New System.Drawing.Size(419, 46)
        Me.SecondAccMain.StyleController = Me.LayoutControl1
        Me.SecondAccMain.TabIndex = 4
        '
        'OverAllTotal
        '
        Me.OverAllTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllTotal.Location = New System.Drawing.Point(1351, 706)
        Me.OverAllTotal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OverAllTotal.Name = "OverAllTotal"
        Me.OverAllTotal.Properties.AllowMouseWheel = False
        Me.OverAllTotal.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllTotal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllTotal.Properties.MaskSettings.Set("mask", "n")
        Me.OverAllTotal.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.OverAllTotal.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.OverAllTotal.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllTotal.Size = New System.Drawing.Size(461, 46)
        Me.OverAllTotal.StyleController = Me.LayoutControl1
        Me.OverAllTotal.TabIndex = 9
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(6, 3, 6, 3)
        Me.GCRole.Location = New System.Drawing.Point(64, -49)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GCRole.Name = "GCRole"
        Me.GCRole.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnDele})
        Me.GCRole.Size = New System.Drawing.Size(1258, 836)
        Me.GCRole.TabIndex = 21
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.AccName, Me.AccID, Me.Debit, Me.Credit, Me.NotesDe, Me.Delete, Me.ID, Me.AccIDTo, Me.Branch, Me.BranchTo})
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 1100
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.MinWidth = 27
        Me.SN.Name = "SN"
        Me.SN.Width = 103
        '
        'AccName
        '
        Me.AccName.Caption = "اسم الحساب"
        Me.AccName.FieldName = "AccName"
        Me.AccName.MinWidth = 27
        Me.AccName.Name = "AccName"
        Me.AccName.Visible = True
        Me.AccName.VisibleIndex = 0
        Me.AccName.Width = 103
        '
        'AccID
        '
        Me.AccID.Caption = "رقم الحساب"
        Me.AccID.FieldName = "AccID"
        Me.AccID.MinWidth = 27
        Me.AccID.Name = "AccID"
        Me.AccID.Width = 103
        '
        'Debit
        '
        Me.Debit.Caption = "مدين"
        Me.Debit.FieldName = "Debit"
        Me.Debit.MinWidth = 27
        Me.Debit.Name = "Debit"
        Me.Debit.UnboundDataType = GetType(Decimal)
        Me.Debit.Visible = True
        Me.Debit.VisibleIndex = 1
        Me.Debit.Width = 103
        '
        'Credit
        '
        Me.Credit.Caption = "دائن"
        Me.Credit.FieldName = "Credit"
        Me.Credit.MinWidth = 27
        Me.Credit.Name = "Credit"
        Me.Credit.UnboundDataType = GetType(Decimal)
        Me.Credit.Visible = True
        Me.Credit.VisibleIndex = 2
        Me.Credit.Width = 103
        '
        'NotesDe
        '
        Me.NotesDe.Caption = "ملاحظات"
        Me.NotesDe.FieldName = "NotesDe"
        Me.NotesDe.MinWidth = 27
        Me.NotesDe.Name = "NotesDe"
        Me.NotesDe.Visible = True
        Me.NotesDe.VisibleIndex = 3
        Me.NotesDe.Width = 103
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
        Me.Delete.Width = 103
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
        'ID
        '
        Me.ID.Caption = "ID"
        Me.ID.FieldName = "ID"
        Me.ID.MinWidth = 27
        Me.ID.Name = "ID"
        Me.ID.Width = 103
        '
        'AccIDTo
        '
        Me.AccIDTo.Caption = "AccID"
        Me.AccIDTo.FieldName = "AccIDTo"
        Me.AccIDTo.MinWidth = 27
        Me.AccIDTo.Name = "AccIDTo"
        Me.AccIDTo.Width = 103
        '
        'Branch
        '
        Me.Branch.Caption = "الفرع"
        Me.Branch.FieldName = "Branch"
        Me.Branch.MinWidth = 27
        Me.Branch.Name = "Branch"
        Me.Branch.Width = 103
        '
        'BranchTo
        '
        Me.BranchTo.Caption = "إلى فرع"
        Me.BranchTo.FieldName = "BranchIDTo"
        Me.BranchTo.MinWidth = 27
        Me.BranchTo.Name = "BranchTo"
        Me.BranchTo.Width = 103
        '
        'Notes2
        '
        Me.Notes2.Location = New System.Drawing.Point(1412, 758)
        Me.Notes2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Notes2.Name = "Notes2"
        Me.Notes2.Properties.Appearance.Options.UseTextOptions = True
        Me.Notes2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Notes2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Notes2.Size = New System.Drawing.Size(440, 46)
        Me.Notes2.StyleController = Me.LayoutControl1
        Me.Notes2.TabIndex = 8
        '
        'ValueType
        '
        Me.ValueType.Location = New System.Drawing.Point(1638, 367)
        Me.ValueType.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ValueType.Name = "ValueType"
        Me.ValueType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ValueType.Properties.Items.AddRange(New Object() {"مدين", "دائن"})
        Me.ValueType.Size = New System.Drawing.Size(153, 46)
        Me.ValueType.StyleController = Me.LayoutControl1
        Me.ValueType.TabIndex = 22
        '
        'FirstAccParent
        '
        Me.FirstAccParent.Location = New System.Drawing.Point(1372, 263)
        Me.FirstAccParent.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.FirstAccParent.Name = "FirstAccParent"
        Me.FirstAccParent.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.FirstAccParent.Properties.NullText = ""
        Me.FirstAccParent.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.FirstAccParent.Properties.PopupSizeable = False
        Me.FirstAccParent.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.FirstAccParent.Size = New System.Drawing.Size(419, 46)
        Me.FirstAccParent.StyleController = Me.LayoutControl1
        Me.FirstAccParent.TabIndex = 5
        '
        'FirstAccID
        '
        Me.FirstAccID.Location = New System.Drawing.Point(1372, 315)
        Me.FirstAccID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.FirstAccID.Name = "FirstAccID"
        Me.FirstAccID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.FirstAccID.Properties.NullText = ""
        Me.FirstAccID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.FirstAccID.Properties.PopupSizeable = False
        Me.FirstAccID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.FirstAccID.Size = New System.Drawing.Size(419, 46)
        Me.FirstAccID.StyleController = Me.LayoutControl1
        Me.FirstAccID.TabIndex = 6
        '
        'SecondAccParent
        '
        Me.SecondAccParent.Location = New System.Drawing.Point(1372, 585)
        Me.SecondAccParent.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SecondAccParent.Name = "SecondAccParent"
        Me.SecondAccParent.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SecondAccParent.Properties.NullText = ""
        Me.SecondAccParent.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.SecondAccParent.Properties.PopupSizeable = False
        Me.SecondAccParent.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.SecondAccParent.Size = New System.Drawing.Size(419, 46)
        Me.SecondAccParent.StyleController = Me.LayoutControl1
        Me.SecondAccParent.TabIndex = 5
        '
        'SecondAccID
        '
        Me.SecondAccID.Location = New System.Drawing.Point(1372, 637)
        Me.SecondAccID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SecondAccID.Name = "SecondAccID"
        Me.SecondAccID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SecondAccID.Properties.NullText = ""
        Me.SecondAccID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.SecondAccID.Properties.PopupSizeable = False
        Me.SecondAccID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.SecondAccID.Size = New System.Drawing.Size(419, 46)
        Me.SecondAccID.StyleController = Me.LayoutControl1
        Me.SecondAccID.TabIndex = 6
        '
        'SimpleButton111
        '
        Me.SimpleButton111.Appearance.BackColor = System.Drawing.Color.WhiteSmoke
        Me.SimpleButton111.Appearance.Options.UseBackColor = True
        Me.SimpleButton111.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton111.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.plus
        Me.SimpleButton111.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton111.Location = New System.Drawing.Point(1351, 758)
        Me.SimpleButton111.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton111.Name = "SimpleButton111"
        Me.SimpleButton111.Size = New System.Drawing.Size(53, 41)
        Me.SimpleButton111.StyleController = Me.LayoutControl1
        Me.SimpleButton111.TabIndex = 2
        '
        'MovmentType
        '
        Me.MovmentType.Location = New System.Drawing.Point(1351, 116)
        Me.MovmentType.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MovmentType.Name = "MovmentType"
        Me.MovmentType.Properties.Appearance.Options.UseTextOptions = True
        Me.MovmentType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.MovmentType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.MovmentType.Size = New System.Drawing.Size(461, 46)
        Me.MovmentType.StyleController = Me.LayoutControl1
        Me.MovmentType.TabIndex = 8
        '
        'AccVal
        '
        Me.AccVal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.AccVal.Location = New System.Drawing.Point(1371, 368)
        Me.AccVal.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.AccVal.Name = "AccVal"
        Me.AccVal.Properties.AllowMouseWheel = False
        Me.AccVal.Properties.Appearance.Options.UseTextOptions = True
        Me.AccVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccVal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AccVal.Properties.MaskSettings.Set("mask", "n")
        Me.AccVal.Properties.MaskSettings.Set("autoHideDecimalSeparator", True)
        Me.AccVal.Properties.MaskSettings.Set("hideInsignificantZeros", True)
        Me.AccVal.Properties.UseMaskAsDisplayFormat = True
        Me.AccVal.Size = New System.Drawing.Size(154, 46)
        Me.AccVal.StyleController = Me.LayoutControl1
        Me.AccVal.TabIndex = 9
        '
        'BranchIDTo
        '
        Me.BranchIDTo.Location = New System.Drawing.Point(1372, 481)
        Me.BranchIDTo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BranchIDTo.Name = "BranchIDTo"
        Me.BranchIDTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchIDTo.Properties.NullText = ""
        Me.BranchIDTo.Properties.PopupSizeable = False
        Me.BranchIDTo.Size = New System.Drawing.Size(419, 46)
        Me.BranchIDTo.StyleController = Me.LayoutControl1
        Me.BranchIDTo.TabIndex = 3
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem21, Me.LayoutControlItem18, Me.LayoutControlGroup2, Me.LayoutControlGroup3, Me.LayoutControlGroup4, Me.LayoutControlItem3, Me.LayoutControlItem6, Me.LayoutControlItem12, Me.LayoutControlItem20, Me.LayoutControlItem9, Me.LayoutControlItem14})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1919, 930)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.Code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(1369, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(514, 52)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(1308, 104)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(575, 52)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem18
        '
        Me.LayoutControlItem18.Control = Me.SimpleButton11
        Me.LayoutControlItem18.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem18.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem18.Location = New System.Drawing.Point(1308, 0)
        Me.LayoutControlItem18.Name = "LayoutControlItem18"
        Me.LayoutControlItem18.Size = New System.Drawing.Size(61, 52)
        Me.LayoutControlItem18.Text = "LayoutControlItem4"
        Me.LayoutControlItem18.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.BorderColor = System.Drawing.Color.Red
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.CustomizationFormText = "الجانب المدين"
        Me.LayoutControlGroup2.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem5, Me.LayoutControlItem8, Me.LayoutControlItem15})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(1308, 260)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(575, 270)
        Me.LayoutControlGroup2.Text = "الطرف الأول"
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.FirstAccMain
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الفرع"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem2.Text = "الحساب الأب"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.FirstAccID
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "الفرع"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem4.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem4.Text = "الطرف الأول"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.FirstAccParent
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "الفرع"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem5.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem5.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem5.Text = "الحساب"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.ValueType
        Me.LayoutControlItem8.Location = New System.Drawing.Point(266, 156)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(267, 54)
        Me.LayoutControlItem8.Text = "طبيعة  القيمة"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem15
        '
        Me.LayoutControlItem15.Control = Me.AccVal
        Me.LayoutControlItem15.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem15.CustomizationFormText = "الراتب"
        Me.LayoutControlItem15.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem15.Name = "LayoutControlItem15"
        Me.LayoutControlItem15.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 4, 4)
        Me.LayoutControlItem15.Size = New System.Drawing.Size(266, 54)
        Me.LayoutControlItem15.Text = "إجمالي القيمة"
        Me.LayoutControlItem15.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.AppearanceGroup.BorderColor = System.Drawing.Color.Lime
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup3.CustomizationFormText = "الجانب الدائن"
        Me.LayoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem10, Me.LayoutControlItem13, Me.LayoutControlItem11, Me.LayoutControlItem16})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(1308, 530)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(575, 268)
        Me.LayoutControlGroup3.Text = "الطرف الثاني"
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.SecondAccMain
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "الفرع"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem10.Text = "الحساب الأب"
        Me.LayoutControlItem10.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.SecondAccID
        Me.LayoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem13.CustomizationFormText = "الفرع"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem13.Text = "الطرف الثاني"
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.SecondAccParent
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "الفرع"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem11.Text = "الحساب"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.BranchIDTo
        Me.LayoutControlItem16.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem16.CustomizationFormText = "الفرع"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(533, 52)
        Me.LayoutControlItem16.Text = "الفرع"
        Me.LayoutControlItem16.TextSize = New System.Drawing.Size(84, 27)
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
        Me.LayoutControlGroup4.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem7})
        Me.LayoutControlGroup4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup4.Name = "LayoutControlGroup4"
        Me.LayoutControlGroup4.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup4.Size = New System.Drawing.Size(1308, 902)
        Me.LayoutControlGroup4.Text = " "
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.GCRole
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(1266, 842)
        Me.LayoutControlItem7.Text = "LayoutControlItem1"
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.DateEdit11
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(1308, 52)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(575, 52)
        Me.LayoutControlItem3.Text = "التاريخ"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.CurrencyID
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "الفرع"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(1308, 156)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(575, 52)
        Me.LayoutControlItem6.Text = "العملة"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.OverAllTotal
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(1308, 798)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(575, 52)
        Me.LayoutControlItem12.Text = "القيمة"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(84, 27)
        '
        'LayoutControlItem20
        '
        Me.LayoutControlItem20.Control = Me.Notes2
        Me.LayoutControlItem20.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem20.CustomizationFormText = "الرمز"
        Me.LayoutControlItem20.Location = New System.Drawing.Point(1369, 850)
        Me.LayoutControlItem20.Name = "LayoutControlItem20"
        Me.LayoutControlItem20.Size = New System.Drawing.Size(514, 52)
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
        Me.LayoutControlItem9.Location = New System.Drawing.Point(1308, 850)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(61, 52)
        Me.LayoutControlItem9.Text = "LayoutControlItem4"
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.Control = Me.MovmentType
        Me.LayoutControlItem14.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem14.CustomizationFormText = "الرمز"
        Me.LayoutControlItem14.Location = New System.Drawing.Point(1308, 208)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.Size = New System.Drawing.Size(575, 52)
        Me.LayoutControlItem14.Text = "وصف العملية"
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(84, 27)
        '
        'FrmMultiAcountEdit
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1940, 874)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FrmMultiAcountEdit"
        Me.Text = "تسجيل عمليات مالية"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit11.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit11.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FirstAccMain.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SecondAccMain.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDele, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Notes2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ValueType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FirstAccParent.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FirstAccID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SecondAccParent.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SecondAccID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MovmentType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AccVal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchIDTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents DateEdit11 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents FirstAccMain As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SecondAccMain As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents OverAllTotal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Debit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Credit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NotesDe As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Delete As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnDele As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccIDTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Notes2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlGroup4 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents ValueType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents FirstAccParent As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents FirstAccID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SecondAccParent As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SecondAccID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SimpleButton111 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AccID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Branch As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents MovmentType As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AccVal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents BranchIDTo As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchTo As DevExpress.XtraGrid.Columns.GridColumn
End Class
