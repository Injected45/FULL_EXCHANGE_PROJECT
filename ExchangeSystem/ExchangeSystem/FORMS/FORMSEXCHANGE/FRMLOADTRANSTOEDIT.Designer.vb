<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMLOADTRANSTOEDIT
    Inherits TemplateForm

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
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMLOADTRANSTOEDIT))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.TransType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.EditType = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SenderName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SPhone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SPhone2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RecievedName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RPhone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RPhone2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnSelect = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepSelectBtn = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.SearchTxT = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.GLKVIEW = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.TransType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EditType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepSelectBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SearchTxT.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GLKVIEW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.TransType)
        Me.LayoutControl1.Controls.Add(Me.EditType)
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.SearchTxT)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1569, 380)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'TransType
        '
        Me.TransType.Location = New System.Drawing.Point(657, 16)
        Me.TransType.Name = "TransType"
        Me.TransType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TransType.Properties.Items.AddRange(New Object() {"حوالة داخلية", "حوالة خارجية"})
        Me.TransType.Size = New System.Drawing.Size(821, 36)
        Me.TransType.StyleController = Me.LayoutControl1
        Me.TransType.TabIndex = 13
        '
        'EditType
        '
        Me.EditType.Location = New System.Drawing.Point(16, 16)
        Me.EditType.Name = "EditType"
        Me.EditType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.EditType.Properties.Items.AddRange(New Object() {"بيانات الراسل", "بيانات المستلم", "كلاهما"})
        Me.EditType.Size = New System.Drawing.Size(560, 36)
        Me.EditType.StyleController = Me.LayoutControl1
        Me.EditType.TabIndex = 14
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GCRole.Location = New System.Drawing.Point(16, 101)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepSelectBtn})
        Me.GCRole.Size = New System.Drawing.Size(1537, 262)
        Me.GCRole.TabIndex = 4
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.Code, Me.SenderName, Me.SPhone1, Me.SPhone2, Me.RecievedName, Me.RPhone1, Me.RPhone2, Me.BtnSelect})
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "#"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        '
        'Code
        '
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "الرمز"
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        '
        'SenderName
        '
        Me.SenderName.Caption = "اسم الراسل"
        Me.SenderName.FieldName = "اسم الراسل"
        Me.SenderName.Name = "SenderName"
        Me.SenderName.Visible = True
        Me.SenderName.VisibleIndex = 2
        '
        'SPhone1
        '
        Me.SPhone1.Caption = "هاتف الراسل"
        Me.SPhone1.FieldName = "هاتف الراسل"
        Me.SPhone1.Name = "SPhone1"
        Me.SPhone1.Visible = True
        Me.SPhone1.VisibleIndex = 3
        '
        'SPhone2
        '
        Me.SPhone2.Caption = "جوال الراسل"
        Me.SPhone2.FieldName = "جوال الراسل"
        Me.SPhone2.Name = "SPhone2"
        Me.SPhone2.Visible = True
        Me.SPhone2.VisibleIndex = 4
        '
        'RecievedName
        '
        Me.RecievedName.Caption = "اسم المستلم"
        Me.RecievedName.FieldName = "اسم المستلم"
        Me.RecievedName.Name = "RecievedName"
        Me.RecievedName.Visible = True
        Me.RecievedName.VisibleIndex = 5
        '
        'RPhone1
        '
        Me.RPhone1.Caption = "هاتف المستلم"
        Me.RPhone1.FieldName = "هاتف المستلم"
        Me.RPhone1.Name = "RPhone1"
        Me.RPhone1.Visible = True
        Me.RPhone1.VisibleIndex = 6
        '
        'RPhone2
        '
        Me.RPhone2.Caption = "جوال المستلم"
        Me.RPhone2.FieldName = "جوال المستلم"
        Me.RPhone2.Name = "RPhone2"
        Me.RPhone2.Visible = True
        Me.RPhone2.VisibleIndex = 7
        '
        'BtnSelect
        '
        Me.BtnSelect.Caption = "اختيار"
        Me.BtnSelect.ColumnEdit = Me.RepSelectBtn
        Me.BtnSelect.FieldName = "اختيار"
        Me.BtnSelect.Name = "BtnSelect"
        Me.BtnSelect.Visible = True
        Me.BtnSelect.VisibleIndex = 8
        '
        'RepSelectBtn
        '
        Me.RepSelectBtn.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources.plus
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.RepSelectBtn.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.RepSelectBtn.Name = "RepSelectBtn"
        Me.RepSelectBtn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'SearchTxT
        '
        Me.SearchTxT.Location = New System.Drawing.Point(16, 58)
        Me.SearchTxT.Name = "SearchTxT"
        Me.SearchTxT.Properties.AdvancedModeOptions.AutoCompleteMode = DevExpress.XtraEditors.TextEditAutoCompleteMode.SuggestAppend
        Me.SearchTxT.Properties.Appearance.Options.UseTextOptions = True
        Me.SearchTxT.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SearchTxT.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SearchTxT.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SearchTxT.Properties.NullText = ""
        Me.SearchTxT.Properties.PopupView = Me.GLKVIEW
        Me.SearchTxT.Size = New System.Drawing.Size(1462, 36)
        Me.SearchTxT.StyleController = Me.LayoutControl1
        Me.SearchTxT.TabIndex = 4
        '
        'GLKVIEW
        '
        Me.GLKVIEW.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GLKVIEW.Name = "GLKVIEW"
        Me.GLKVIEW.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GLKVIEW.OptionsView.ShowGroupPanel = False
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem1, Me.LayoutControlItem8})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1569, 380)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.TransType
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "نوع الحوالة"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(641, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(902, 42)
        Me.LayoutControlItem2.Text = "نوع الحوالة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(59, 22)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.EditType
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "نوع التعديل"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(641, 42)
        Me.LayoutControlItem4.Text = "نوع التعديل"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(59, 22)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCRole
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 4, 4)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1543, 270)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.SearchTxT
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "ملاحظات"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(1543, 42)
        Me.LayoutControlItem8.Text = "البحث"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(59, 22)
        '
        'FRMLOADTRANSTOEDIT
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1569, 380)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMLOADTRANSTOEDIT.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMLOADTRANSTOEDIT"
        Me.Text = "البحث"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.TransType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EditType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepSelectBtn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SearchTxT.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GLKVIEW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents TransType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents EditType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SenderName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SPhone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SPhone2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RecievedName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RPhone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RPhone2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnSelect As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepSelectBtn As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SearchTxT As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents GLKVIEW As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
End Class
