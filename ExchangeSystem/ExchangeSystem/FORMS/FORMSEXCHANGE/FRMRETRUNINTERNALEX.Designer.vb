<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMRETRUNINTERNALEX
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.RowHandle = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SenderName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SPhone = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ExVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ExtraComission = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ConfirmCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnConfirm = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.RBName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DBName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RecievedCurrencyID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BRRID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BRDID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NetTotal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnInsertDate = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.BtnRecievedCurrency = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnDeliveredCurrency = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.btnOverallVal = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.BtnExVal = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.BtnBranchRecieved = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnBranchDeliveredID = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.InternalExCH = New DevExpress.XtraEditors.CheckEdit()
        Me.ExternalExCH = New DevExpress.XtraEditors.CheckEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnInsertDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnInsertDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnRecievedCurrency, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDeliveredCurrency, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnOverallVal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnExVal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnBranchRecieved, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnBranchDeliveredID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InternalExCH.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExternalExCH.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Controls.Add(Me.InternalExCH)
        Me.LayoutControl1.Controls.Add(Me.ExternalExCH)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1669, 442)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCROLE
        '
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Location = New System.Drawing.Point(16, 48)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnInsertDate, Me.BtnRecievedCurrency, Me.BtnDeliveredCurrency, Me.btnOverallVal, Me.BtnExVal, Me.BtnBranchRecieved, Me.BtnBranchDeliveredID, Me.BtnConfirm})
        Me.GCROLE.Size = New System.Drawing.Size(1637, 378)
        Me.GCROLE.TabIndex = 4
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.RowHandle, Me.Code, Me.InsertDate, Me.SenderName, Me.SPhone, Me.OverallVal, Me.ExVal, Me.ExtraComission, Me.ConfirmCol, Me.RBName, Me.DBName, Me.RecievedCurrencyID, Me.BRRID, Me.BRDID, Me.NetTotal})
        Me.GVROLE.DetailHeight = 308
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'RowHandle
        '
        Me.RowHandle.Caption = "#"
        Me.RowHandle.FieldName = "RowHandle"
        Me.RowHandle.Name = "RowHandle"
        Me.RowHandle.Visible = True
        Me.RowHandle.VisibleIndex = 0
        Me.RowHandle.Width = 32
        '
        'Code
        '
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "Code"
        Me.Code.MinWidth = 16
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 151
        '
        'InsertDate
        '
        Me.InsertDate.Caption = "التاريخ"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.MinWidth = 16
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 2
        Me.InsertDate.Width = 151
        '
        'SenderName
        '
        Me.SenderName.Caption = "اسم الراسل"
        Me.SenderName.FieldName = "SenderName"
        Me.SenderName.MinWidth = 16
        Me.SenderName.Name = "SenderName"
        Me.SenderName.Visible = True
        Me.SenderName.VisibleIndex = 3
        Me.SenderName.Width = 151
        '
        'SPhone
        '
        Me.SPhone.Caption = "هاتف الراسل"
        Me.SPhone.FieldName = "SPhone"
        Me.SPhone.MinWidth = 16
        Me.SPhone.Name = "SPhone"
        Me.SPhone.Visible = True
        Me.SPhone.VisibleIndex = 4
        Me.SPhone.Width = 151
        '
        'OverallVal
        '
        Me.OverallVal.Caption = "قيمة الحوالة"
        Me.OverallVal.FieldName = "OverallVal"
        Me.OverallVal.MinWidth = 16
        Me.OverallVal.Name = "OverallVal"
        Me.OverallVal.Visible = True
        Me.OverallVal.VisibleIndex = 5
        Me.OverallVal.Width = 151
        '
        'ExVal
        '
        Me.ExVal.Caption = "العمولة"
        Me.ExVal.FieldName = "ExVal"
        Me.ExVal.MinWidth = 16
        Me.ExVal.Name = "ExVal"
        Me.ExVal.Visible = True
        Me.ExVal.VisibleIndex = 6
        Me.ExVal.Width = 151
        '
        'ExtraComission
        '
        Me.ExtraComission.Caption = "خصم من العمولة"
        Me.ExtraComission.FieldName = "ExtraComission"
        Me.ExtraComission.MinWidth = 16
        Me.ExtraComission.Name = "ExtraComission"
        Me.ExtraComission.Visible = True
        Me.ExtraComission.VisibleIndex = 7
        Me.ExtraComission.Width = 171
        '
        'ConfirmCol
        '
        Me.ConfirmCol.Caption = "إرجاع"
        Me.ConfirmCol.ColumnEdit = Me.BtnConfirm
        Me.ConfirmCol.FieldName = "ConfirmCol"
        Me.ConfirmCol.Name = "ConfirmCol"
        Me.ConfirmCol.Visible = True
        Me.ConfirmCol.VisibleIndex = 9
        Me.ConfirmCol.Width = 41
        '
        'BtnConfirm
        '
        Me.BtnConfirm.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources._return
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnConfirm.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnConfirm.ContextImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources._return
        Me.BtnConfirm.ContextImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.BtnConfirm.Name = "BtnConfirm"
        Me.BtnConfirm.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'RBName
        '
        Me.RBName.Caption = "الفرع المرسل"
        Me.RBName.FieldName = "RBName"
        Me.RBName.Name = "RBName"
        Me.RBName.Visible = True
        Me.RBName.VisibleIndex = 10
        Me.RBName.Width = 68
        '
        'DBName
        '
        Me.DBName.Caption = "الفرع المسلم"
        Me.DBName.FieldName = "DBName"
        Me.DBName.Name = "DBName"
        Me.DBName.Visible = True
        Me.DBName.VisibleIndex = 11
        Me.DBName.Width = 68
        '
        'RecievedCurrencyID
        '
        Me.RecievedCurrencyID.Caption = "العملة المستلمة"
        Me.RecievedCurrencyID.FieldName = "RecievedCurrencyID"
        Me.RecievedCurrencyID.Name = "RecievedCurrencyID"
        Me.RecievedCurrencyID.Visible = True
        Me.RecievedCurrencyID.VisibleIndex = 12
        Me.RecievedCurrencyID.Width = 68
        '
        'BRRID
        '
        Me.BRRID.Caption = "GridColumn1"
        Me.BRRID.FieldName = "BRRID"
        Me.BRRID.Name = "BRRID"
        Me.BRRID.Visible = True
        Me.BRRID.VisibleIndex = 13
        Me.BRRID.Width = 68
        '
        'BRDID
        '
        Me.BRDID.Caption = "GridColumn2"
        Me.BRDID.FieldName = "BRDID"
        Me.BRDID.Name = "BRDID"
        Me.BRDID.Visible = True
        Me.BRDID.VisibleIndex = 14
        Me.BRDID.Width = 89
        '
        'NetTotal
        '
        Me.NetTotal.Caption = "الصافي"
        Me.NetTotal.FieldName = "NetTotal"
        Me.NetTotal.Name = "NetTotal"
        Me.NetTotal.Visible = True
        Me.NetTotal.VisibleIndex = 8
        Me.NetTotal.Width = 169
        '
        'BtnInsertDate
        '
        Me.BtnInsertDate.AutoHeight = False
        Me.BtnInsertDate.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnInsertDate.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnInsertDate.Name = "BtnInsertDate"
        '
        'BtnRecievedCurrency
        '
        Me.BtnRecievedCurrency.AutoHeight = False
        Me.BtnRecievedCurrency.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnRecievedCurrency.Name = "BtnRecievedCurrency"
        Me.BtnRecievedCurrency.NullText = ""
        Me.BtnRecievedCurrency.ShowFooter = False
        Me.BtnRecievedCurrency.ShowHeader = False
        '
        'BtnDeliveredCurrency
        '
        Me.BtnDeliveredCurrency.AutoHeight = False
        Me.BtnDeliveredCurrency.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnDeliveredCurrency.Name = "BtnDeliveredCurrency"
        Me.BtnDeliveredCurrency.NullText = ""
        Me.BtnDeliveredCurrency.ShowFooter = False
        Me.BtnDeliveredCurrency.ShowHeader = False
        '
        'btnOverallVal
        '
        Me.btnOverallVal.AutoHeight = False
        Me.btnOverallVal.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.btnOverallVal.MaskSettings.Set("mask", "n3")
        Me.btnOverallVal.MaskSettings.Set("hideInsignificantZeros", False)
        Me.btnOverallVal.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.btnOverallVal.Name = "btnOverallVal"
        Me.btnOverallVal.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.btnOverallVal.UseMaskAsDisplayFormat = True
        '
        'BtnExVal
        '
        Me.BtnExVal.AutoHeight = False
        Me.BtnExVal.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnExVal.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.BtnExVal.MaskSettings.Set("hideInsignificantZeros", False)
        Me.BtnExVal.MaskSettings.Set("mask", "n3")
        Me.BtnExVal.Name = "BtnExVal"
        Me.BtnExVal.UseMaskAsDisplayFormat = True
        '
        'BtnBranchRecieved
        '
        Me.BtnBranchRecieved.AutoHeight = False
        Me.BtnBranchRecieved.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnBranchRecieved.Name = "BtnBranchRecieved"
        Me.BtnBranchRecieved.NullText = ""
        Me.BtnBranchRecieved.ShowFooter = False
        Me.BtnBranchRecieved.ShowHeader = False
        '
        'BtnBranchDeliveredID
        '
        Me.BtnBranchDeliveredID.AutoHeight = False
        Me.BtnBranchDeliveredID.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnBranchDeliveredID.Name = "BtnBranchDeliveredID"
        Me.BtnBranchDeliveredID.NullText = ""
        Me.BtnBranchDeliveredID.ShowFooter = False
        Me.BtnBranchDeliveredID.ShowHeader = False
        '
        'InternalExCH
        '
        Me.InternalExCH.Location = New System.Drawing.Point(837, 16)
        Me.InternalExCH.Margin = New System.Windows.Forms.Padding(3, 3, 10, 3)
        Me.InternalExCH.Name = "InternalExCH"
        Me.InternalExCH.Properties.Caption = "حوالة داخلية"
        Me.InternalExCH.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio1
        Me.InternalExCH.Properties.CheckBoxOptions.SvgColorChecked = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information
        Me.InternalExCH.Size = New System.Drawing.Size(744, 26)
        Me.InternalExCH.StyleController = Me.LayoutControl1
        Me.InternalExCH.TabIndex = 8
        '
        'ExternalExCH
        '
        Me.ExternalExCH.Location = New System.Drawing.Point(16, 16)
        Me.ExternalExCH.Name = "ExternalExCH"
        Me.ExternalExCH.Properties.Caption = "حوالة خارجية"
        Me.ExternalExCH.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio1
        Me.ExternalExCH.Properties.CheckBoxOptions.SvgColorChecked = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information
        Me.ExternalExCH.Size = New System.Drawing.Size(815, 26)
        Me.ExternalExCH.StyleController = Me.LayoutControl1
        Me.ExternalExCH.TabIndex = 9
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem7, Me.LayoutControlItem8})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1669, 442)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 32)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1643, 384)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.InternalExCH
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "LayoutControlItem7"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(821, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 75, 3, 3)
        Me.LayoutControlItem7.Size = New System.Drawing.Size(822, 32)
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.ExternalExCH
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(821, 32)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'FRMRETRUNINTERNALEX
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1669, 442)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = Global.ExchangeSystem.My.Resources.Resources.icons8_return_100
        Me.Name = "FRMRETRUNINTERNALEX"
        Me.Text = "ترجيع حوالة"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnInsertDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnInsertDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnRecievedCurrency, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDeliveredCurrency, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnOverallVal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnExVal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnBranchRecieved, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnBranchDeliveredID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InternalExCH.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExternalExCH.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents RowHandle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnInsertDate As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents SenderName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SPhone As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnOverallVal As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents ExVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnExVal As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents ExtraComission As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ConfirmCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnConfirm As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents BtnRecievedCurrency As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnDeliveredCurrency As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnBranchRecieved As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnBranchDeliveredID As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents InternalExCH As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents ExternalExCH As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RBName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DBName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RecievedCurrencyID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BRRID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BRDID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NetTotal As DevExpress.XtraGrid.Columns.GridColumn
End Class
