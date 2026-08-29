<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLoadCanceledMainBranch
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim ColumnDefinition1 As DevExpress.XtraLayout.ColumnDefinition = New DevExpress.XtraLayout.ColumnDefinition()
        Dim RowDefinition1 As DevExpress.XtraLayout.RowDefinition = New DevExpress.XtraLayout.RowDefinition()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLoadCanceledMainBranch))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.RowHandle = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SenderName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SPhone = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RecievedName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RPhone = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RBName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ExVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ConfirmCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnConfirm = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.DeliveredCurrencyID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchDeliveredID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchRecievedID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnInsertDate = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.BtnRecievedCurrency = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnDeliveredCurrency = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.btnOverallVal = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.BtnExVal = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.BtnBranchRecieved = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnBranchDeliveredID = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
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
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1822, 254)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCROLE
        '
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Location = New System.Drawing.Point(16, 16)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnInsertDate, Me.BtnRecievedCurrency, Me.BtnDeliveredCurrency, Me.btnOverallVal, Me.BtnExVal, Me.BtnBranchRecieved, Me.BtnBranchDeliveredID, Me.BtnConfirm})
        Me.GCROLE.Size = New System.Drawing.Size(1790, 222)
        Me.GCROLE.TabIndex = 5
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.RowHandle, Me.Code, Me.InsertDate, Me.SenderName, Me.SPhone, Me.RecievedName, Me.RPhone, Me.RBName, Me.OverallVal, Me.ExVal, Me.ConfirmCol, Me.DeliveredCurrencyID, Me.BranchDeliveredID, Me.BranchRecievedID})
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
        Me.RowHandle.Width = 33
        '
        'Code
        '
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "Code"
        Me.Code.MinWidth = 16
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 152
        '
        'InsertDate
        '
        Me.InsertDate.Caption = "التاريخ"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.MinWidth = 16
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 2
        Me.InsertDate.Width = 152
        '
        'SenderName
        '
        Me.SenderName.Caption = "اسم الراسل"
        Me.SenderName.FieldName = "SenderName"
        Me.SenderName.MinWidth = 16
        Me.SenderName.Name = "SenderName"
        Me.SenderName.Visible = True
        Me.SenderName.VisibleIndex = 3
        Me.SenderName.Width = 152
        '
        'SPhone
        '
        Me.SPhone.Caption = "هاتف الراسل"
        Me.SPhone.FieldName = "SPhone"
        Me.SPhone.MinWidth = 16
        Me.SPhone.Name = "SPhone"
        Me.SPhone.Visible = True
        Me.SPhone.VisibleIndex = 4
        Me.SPhone.Width = 152
        '
        'RecievedName
        '
        Me.RecievedName.Caption = "اسم المستلم"
        Me.RecievedName.FieldName = "RecievedName"
        Me.RecievedName.Name = "RecievedName"
        Me.RecievedName.Visible = True
        Me.RecievedName.VisibleIndex = 5
        '
        'RPhone
        '
        Me.RPhone.Caption = "هاتف"
        Me.RPhone.FieldName = "RPhone"
        Me.RPhone.Name = "RPhone"
        Me.RPhone.Visible = True
        Me.RPhone.VisibleIndex = 6
        '
        'RBName
        '
        Me.RBName.Caption = "الفرع المرسل"
        Me.RBName.FieldName = "RBName"
        Me.RBName.Name = "RBName"
        Me.RBName.Visible = True
        Me.RBName.VisibleIndex = 7
        '
        'OverallVal
        '
        Me.OverallVal.Caption = "قيمة الحوالة"
        Me.OverallVal.FieldName = "OverallVal"
        Me.OverallVal.MinWidth = 16
        Me.OverallVal.Name = "OverallVal"
        Me.OverallVal.Visible = True
        Me.OverallVal.VisibleIndex = 8
        Me.OverallVal.Width = 152
        '
        'ExVal
        '
        Me.ExVal.Caption = "العمولة"
        Me.ExVal.FieldName = "ExVal"
        Me.ExVal.MinWidth = 16
        Me.ExVal.Name = "ExVal"
        Me.ExVal.Visible = True
        Me.ExVal.VisibleIndex = 9
        Me.ExVal.Width = 152
        '
        'ConfirmCol
        '
        Me.ConfirmCol.Caption = "تسليم"
        Me.ConfirmCol.ColumnEdit = Me.BtnConfirm
        Me.ConfirmCol.FieldName = "ConfirmCol"
        Me.ConfirmCol.Name = "ConfirmCol"
        Me.ConfirmCol.Visible = True
        Me.ConfirmCol.VisibleIndex = 10
        Me.ConfirmCol.Width = 40
        '
        'BtnConfirm
        '
        Me.BtnConfirm.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources._return
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnConfirm.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnConfirm.ContextImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.BtnConfirm.Name = "BtnConfirm"
        Me.BtnConfirm.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'DeliveredCurrencyID
        '
        Me.DeliveredCurrencyID.Caption = "العملة المستلمة"
        Me.DeliveredCurrencyID.FieldName = "DeliveredCurrencyID"
        Me.DeliveredCurrencyID.Name = "DeliveredCurrencyID"
        Me.DeliveredCurrencyID.Visible = True
        Me.DeliveredCurrencyID.VisibleIndex = 11
        '
        'BranchDeliveredID
        '
        Me.BranchDeliveredID.Caption = "إلى"
        Me.BranchDeliveredID.FieldName = "BranchDeliveredID"
        Me.BranchDeliveredID.Name = "BranchDeliveredID"
        Me.BranchDeliveredID.Visible = True
        Me.BranchDeliveredID.VisibleIndex = 12
        '
        'BranchRecievedID
        '
        Me.BranchRecievedID.Caption = "من"
        Me.BranchRecievedID.FieldName = "BranchRecievedID"
        Me.BranchRecievedID.Name = "BranchRecievedID"
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
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.Root.LayoutMode = DevExpress.XtraLayout.Utils.LayoutMode.Table
        Me.Root.Name = "Root"
        ColumnDefinition1.SizeType = System.Windows.Forms.SizeType.Percent
        ColumnDefinition1.Width = 100.0R
        Me.Root.OptionsTableLayoutGroup.ColumnDefinitions.AddRange(New DevExpress.XtraLayout.ColumnDefinition() {ColumnDefinition1})
        RowDefinition1.Height = 100.0R
        RowDefinition1.SizeType = System.Windows.Forms.SizeType.Percent
        Me.Root.OptionsTableLayoutGroup.RowDefinitions.AddRange(New DevExpress.XtraLayout.RowDefinition() {RowDefinition1})
        Me.Root.Size = New System.Drawing.Size(1822, 254)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1796, 228)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'FrmLoadCanceledMainBranch
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1822, 254)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmLoadCanceledMainBranch.IconOptions.Image"), System.Drawing.Image)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.deliveryhand
        Me.Name = "FrmLoadCanceledMainBranch"
        Me.Text = "تسليم حوالات تم رفض إلغاؤها"
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
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents RowHandle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SenderName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SPhone As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RecievedName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RPhone As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RBName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ExVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ConfirmCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnConfirm As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents DeliveredCurrencyID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BranchDeliveredID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnInsertDate As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents BtnRecievedCurrency As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnDeliveredCurrency As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents btnOverallVal As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents BtnExVal As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents BtnBranchRecieved As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnBranchDeliveredID As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchRecievedID As DevExpress.XtraGrid.Columns.GridColumn
End Class
