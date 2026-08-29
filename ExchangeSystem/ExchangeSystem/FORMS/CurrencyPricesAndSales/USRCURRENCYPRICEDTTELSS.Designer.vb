<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class USRCURRENCYPRICEDTTELSS
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim GridFormatRule1 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim TableColumnDefinition1 As DevExpress.XtraEditors.TableLayout.TableColumnDefinition = New DevExpress.XtraEditors.TableLayout.TableColumnDefinition()
        Dim TableColumnDefinition2 As DevExpress.XtraEditors.TableLayout.TableColumnDefinition = New DevExpress.XtraEditors.TableLayout.TableColumnDefinition()
        Dim TableRowDefinition1 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TableRowDefinition2 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TableRowDefinition3 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TileViewItemElement1 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement2 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement3 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement4 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement5 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement6 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(USRCURRENCYPRICEDTTELSS))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.CuName = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.ID = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.typesf = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GridControl2 = New DevExpress.XtraGrid.GridControl()
        Me.TileView1 = New DevExpress.XtraGrid.Views.Tile.TileView()
        Me.ITYPE = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.IDCruns = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyIDTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SalePrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BuyPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyPower = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BankSalePrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BankBuyPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyBuyPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencySalePrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.cluemnsEdit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.CurrencyBankBuyPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyBankSalePrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Typesd = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TileView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CuName
        '
        Me.CuName.Caption = "أسم العملة "
        Me.CuName.FieldName = "CuName"
        Me.CuName.Name = "CuName"
        Me.CuName.Visible = True
        Me.CuName.VisibleIndex = 1
        '
        'ID
        '
        Me.ID.Caption = "رقم العملة "
        Me.ID.FieldName = "ID"
        Me.ID.Name = "ID"
        Me.ID.Visible = True
        Me.ID.VisibleIndex = 0
        '
        'typesf
        '
        Me.typesf.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.typesf.AppearanceCell.Options.UseBackColor = True
        Me.typesf.Caption = "نوع البيع"
        Me.typesf.FieldName = "typesf"
        Me.typesf.Name = "typesf"
        Me.typesf.Visible = True
        Me.typesf.VisibleIndex = 2
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GridControl2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1080, 165)
        Me.Panel1.TabIndex = 2
        '
        'GridControl2
        '
        Me.GridControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl2.Location = New System.Drawing.Point(0, 0)
        Me.GridControl2.MainView = Me.TileView1
        Me.GridControl2.Name = "GridControl2"
        Me.GridControl2.Size = New System.Drawing.Size(1080, 165)
        Me.GridControl2.TabIndex = 0
        Me.GridControl2.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.TileView1})
        '
        'TileView1
        '
        Me.TileView1.Appearance.Group.BackColor = System.Drawing.Color.Transparent
        Me.TileView1.Appearance.Group.Options.UseBackColor = True
        Me.TileView1.Appearance.ItemFocused.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TileView1.Appearance.ItemFocused.BorderColor = System.Drawing.Color.Red
        Me.TileView1.Appearance.ItemFocused.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.TileView1.Appearance.ItemFocused.Options.UseBackColor = True
        Me.TileView1.Appearance.ItemFocused.Options.UseBorderColor = True
        Me.TileView1.Appearance.ItemFocused.Options.UseForeColor = True
        Me.TileView1.Appearance.ItemNormal.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.TileView1.Appearance.ItemNormal.BorderColor = System.Drawing.Color.Black
        Me.TileView1.Appearance.ItemNormal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TileView1.Appearance.ItemNormal.Options.UseBackColor = True
        Me.TileView1.Appearance.ItemNormal.Options.UseBorderColor = True
        Me.TileView1.Appearance.ItemNormal.Options.UseForeColor = True
        Me.TileView1.Appearance.ItemNormal.Options.UseTextOptions = True
        Me.TileView1.Appearance.ItemNormal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TileView1.Appearance.ItemNormal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TileView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
        Me.TileView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ID, Me.CuName, Me.typesf, Me.ITYPE})
        Me.TileView1.FocusBorderColor = System.Drawing.Color.DimGray
        GridFormatRule1.Column = Me.CuName
        GridFormatRule1.Description = Nothing
        GridFormatRule1.Name = "Format0"
        GridFormatRule1.Rule = Nothing
        Me.TileView1.FormatRules.Add(GridFormatRule1)
        Me.TileView1.GridControl = Me.GridControl2
        Me.TileView1.Name = "TileView1"
        Me.TileView1.OptionsTiles.ItemSize = New System.Drawing.Size(372, 106)
        TableColumnDefinition1.Length.Value = 253.0R
        TableColumnDefinition2.Length.Value = 95.0R
        Me.TileView1.TileColumns.Add(TableColumnDefinition1)
        Me.TileView1.TileColumns.Add(TableColumnDefinition2)
        TableRowDefinition1.Length.Value = 23.0R
        TableRowDefinition2.Length.Value = 40.0R
        TableRowDefinition3.Length.Value = 27.0R
        Me.TileView1.TileRows.Add(TableRowDefinition1)
        Me.TileView1.TileRows.Add(TableRowDefinition2)
        Me.TileView1.TileRows.Add(TableRowDefinition3)
        TileViewItemElement1.Appearance.Normal.Options.UseTextOptions = True
        TileViewItemElement1.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TileViewItemElement1.Appearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        TileViewItemElement1.Column = Me.CuName
        TileViewItemElement1.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement1.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement1.RowIndex = 1
        TileViewItemElement1.Text = "CuName"
        TileViewItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement2.Appearance.Normal.Options.UseTextOptions = True
        TileViewItemElement2.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TileViewItemElement2.Appearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        TileViewItemElement2.Column = Me.ID
        TileViewItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement2.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement2.Text = "ID"
        TileViewItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement3.Appearance.Normal.Options.UseTextOptions = True
        TileViewItemElement3.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TileViewItemElement3.Appearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        TileViewItemElement3.ColumnIndex = 1
        TileViewItemElement3.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement3.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement3.RowIndex = 1
        TileViewItemElement3.Text = "اسم العلملة"
        TileViewItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement4.Appearance.Disabled.BackColor = System.Drawing.Color.Transparent
        TileViewItemElement4.Appearance.Disabled.Options.UseBackColor = True
        TileViewItemElement4.Appearance.Normal.BackColor = System.Drawing.Color.Transparent
        TileViewItemElement4.Appearance.Normal.Options.UseBackColor = True
        TileViewItemElement4.Appearance.Normal.Options.UseTextOptions = True
        TileViewItemElement4.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TileViewItemElement4.Appearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        TileViewItemElement4.Appearance.Selected.BackColor = System.Drawing.Color.Transparent
        TileViewItemElement4.Appearance.Selected.Options.UseBackColor = True
        TileViewItemElement4.ColumnIndex = 1
        TileViewItemElement4.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement4.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement4.Text = "رقم العملة"
        TileViewItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement5.Appearance.Normal.Options.UseTextOptions = True
        TileViewItemElement5.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TileViewItemElement5.Appearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        TileViewItemElement5.ColumnIndex = 1
        TileViewItemElement5.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement5.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement5.RowIndex = 2
        TileViewItemElement5.Text = "نوع البيع"
        TileViewItemElement5.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement6.Appearance.Normal.Options.UseTextOptions = True
        TileViewItemElement6.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TileViewItemElement6.Appearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        TileViewItemElement6.Column = Me.typesf
        TileViewItemElement6.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement6.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement6.RowIndex = 2
        TileViewItemElement6.Text = "TileViewColumn2"
        TileViewItemElement6.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        Me.TileView1.TileTemplate.Add(TileViewItemElement1)
        Me.TileView1.TileTemplate.Add(TileViewItemElement2)
        Me.TileView1.TileTemplate.Add(TileViewItemElement3)
        Me.TileView1.TileTemplate.Add(TileViewItemElement4)
        Me.TileView1.TileTemplate.Add(TileViewItemElement5)
        Me.TileView1.TileTemplate.Add(TileViewItemElement6)
        '
        'ITYPE
        '
        Me.ITYPE.Caption = "رمز المعاملة"
        Me.ITYPE.FieldName = "ITYPE"
        Me.ITYPE.Name = "ITYPE"
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 165)
        Me.GridControl1.MainView = Me.GVRole
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(1080, 487)
        Me.GridControl1.TabIndex = 3
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.IDCruns, Me.CurrencyIDTo, Me.SalePrice, Me.BuyPrice, Me.CurrencyPower, Me.BankSalePrice, Me.BankBuyPrice, Me.CurrencyBuyPrice, Me.CurrencySalePrice, Me.cluemnsEdit, Me.CurrencyBankBuyPrice, Me.CurrencyBankSalePrice, Me.Typesd})
        Me.GVRole.GridControl = Me.GridControl1
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
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 53
        '
        'IDCruns
        '
        Me.IDCruns.Caption = "رقم العملة"
        Me.IDCruns.FieldName = "IDCruns"
        Me.IDCruns.Name = "IDCruns"
        '
        'CurrencyIDTo
        '
        Me.CurrencyIDTo.Caption = "أسم العملة "
        Me.CurrencyIDTo.FieldName = "CurrencyIDTo"
        Me.CurrencyIDTo.Name = "CurrencyIDTo"
        Me.CurrencyIDTo.Visible = True
        Me.CurrencyIDTo.VisibleIndex = 1
        Me.CurrencyIDTo.Width = 190
        '
        'SalePrice
        '
        Me.SalePrice.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.SalePrice.AppearanceCell.Options.UseBackColor = True
        Me.SalePrice.Caption = "بيع"
        Me.SalePrice.FieldName = "SalePrice"
        Me.SalePrice.Name = "SalePrice"
        Me.SalePrice.UnboundDataType = GetType(Decimal)
        Me.SalePrice.Visible = True
        Me.SalePrice.VisibleIndex = 3
        Me.SalePrice.Width = 80
        '
        'BuyPrice
        '
        Me.BuyPrice.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.BuyPrice.AppearanceCell.Options.UseBackColor = True
        Me.BuyPrice.Caption = "الشراء"
        Me.BuyPrice.FieldName = "BuyPrice"
        Me.BuyPrice.Name = "BuyPrice"
        Me.BuyPrice.UnboundDataType = GetType(Decimal)
        Me.BuyPrice.Visible = True
        Me.BuyPrice.VisibleIndex = 2
        Me.BuyPrice.Width = 82
        '
        'CurrencyPower
        '
        Me.CurrencyPower.Caption = "قوة العملة"
        Me.CurrencyPower.FieldName = "CurrencyPower"
        Me.CurrencyPower.Name = "CurrencyPower"
        Me.CurrencyPower.Visible = True
        Me.CurrencyPower.VisibleIndex = 4
        Me.CurrencyPower.Width = 114
        '
        'BankSalePrice
        '
        Me.BankSalePrice.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.BankSalePrice.AppearanceCell.Options.UseBackColor = True
        Me.BankSalePrice.Caption = "بيع عالمصرف"
        Me.BankSalePrice.FieldName = "BankSalePrice"
        Me.BankSalePrice.Name = "BankSalePrice"
        Me.BankSalePrice.Width = 86
        '
        'BankBuyPrice
        '
        Me.BankBuyPrice.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.BankBuyPrice.AppearanceCell.Options.UseBackColor = True
        Me.BankBuyPrice.Caption = "شراء عالمصرف"
        Me.BankBuyPrice.FieldName = "BankBuyPrice"
        Me.BankBuyPrice.Name = "BankBuyPrice"
        Me.BankBuyPrice.Width = 81
        '
        'CurrencyBuyPrice
        '
        Me.CurrencyBuyPrice.Caption = "بيع العملة"
        Me.CurrencyBuyPrice.FieldName = "CurrencyBuyPrice"
        Me.CurrencyBuyPrice.Name = "CurrencyBuyPrice"
        Me.CurrencyBuyPrice.Visible = True
        Me.CurrencyBuyPrice.VisibleIndex = 5
        Me.CurrencyBuyPrice.Width = 114
        '
        'CurrencySalePrice
        '
        Me.CurrencySalePrice.Caption = "سعر شراء العملة "
        Me.CurrencySalePrice.FieldName = "CurrencySalePrice"
        Me.CurrencySalePrice.Name = "CurrencySalePrice"
        Me.CurrencySalePrice.Visible = True
        Me.CurrencySalePrice.VisibleIndex = 6
        Me.CurrencySalePrice.Width = 168
        '
        'cluemnsEdit
        '
        Me.cluemnsEdit.Caption = "تعديل"
        Me.cluemnsEdit.ColumnEdit = Me.RepositoryItemButtonEdit1
        Me.cluemnsEdit.FieldName = "cluemnsEdit"
        Me.cluemnsEdit.Name = "cluemnsEdit"
        Me.cluemnsEdit.Visible = True
        Me.cluemnsEdit.VisibleIndex = 7
        Me.cluemnsEdit.Width = 80
        '
        'RepositoryItemButtonEdit1
        '
        Me.RepositoryItemButtonEdit1.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = CType(resources.GetObject("EditorButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
        Me.RepositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'CurrencyBankBuyPrice
        '
        Me.CurrencyBankBuyPrice.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.CurrencyBankBuyPrice.AppearanceCell.Options.UseBackColor = True
        Me.CurrencyBankBuyPrice.Caption = "سعر الشراء عالمصرف"
        Me.CurrencyBankBuyPrice.FieldName = "CurrencyBankBuyPrice"
        Me.CurrencyBankBuyPrice.Name = "CurrencyBankBuyPrice"
        '
        'CurrencyBankSalePrice
        '
        Me.CurrencyBankSalePrice.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.CurrencyBankSalePrice.AppearanceCell.Options.UseBackColor = True
        Me.CurrencyBankSalePrice.Caption = "سعر البيع عالمصرف"
        Me.CurrencyBankSalePrice.FieldName = "CurrencyBankSalePrice"
        Me.CurrencyBankSalePrice.Name = "CurrencyBankSalePrice"
        '
        'Typesd
        '
        Me.Typesd.Caption = "نوع البيع "
        Me.Typesd.FieldName = "Typesd"
        Me.Typesd.Name = "Typesd"
        '
        'USRCURRENCYPRICEDTTELSS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GridControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "USRCURRENCYPRICEDTTELSS"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Size = New System.Drawing.Size(1080, 652)
        Me.Panel1.ResumeLayout(False)
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TileView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents GridControl2 As DevExpress.XtraGrid.GridControl
    Friend WithEvents TileView1 As DevExpress.XtraGrid.Views.Tile.TileView
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents CuName As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyIDTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SalePrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BuyPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyPower As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cluemnsEdit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents BankSalePrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BankBuyPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents IDCruns As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyBuyPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencySalePrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyBankBuyPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyBankSalePrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents typesf As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents ITYPE As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents Typesd As DevExpress.XtraGrid.Columns.GridColumn
    Public WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
End Class
