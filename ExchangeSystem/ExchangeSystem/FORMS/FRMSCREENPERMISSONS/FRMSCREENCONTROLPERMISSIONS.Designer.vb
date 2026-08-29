<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMSCREENCONTROLPERMISSIONS
    Inherits DevExpress.XtraEditors.XtraForm

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
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.BtnInsertDate = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.btnOverallVal = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.BtnExVal = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.CityID = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnBranchRecieved = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnBranchDeliveredID = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnConfirm = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.BtnRecievedCurrency = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.BtnDeliveredCurrency = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        Me.UserID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.UserName = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnInsertDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnInsertDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnOverallVal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnExVal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CityID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnBranchRecieved, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnBranchDeliveredID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnRecievedCurrency, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnDeliveredCurrency, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.GCROLE)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(330, 761)
        Me.PanelControl1.TabIndex = 0
        '
        'GCROLE
        '
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Location = New System.Drawing.Point(23, 18)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnInsertDate, Me.BtnRecievedCurrency, Me.BtnDeliveredCurrency, Me.btnOverallVal, Me.BtnExVal, Me.BtnBranchRecieved, Me.BtnBranchDeliveredID, Me.BtnConfirm, Me.CityID})
        Me.GCROLE.Size = New System.Drawing.Size(296, 724)
        Me.GCROLE.TabIndex = 5
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.UserID, Me.UserName})
        Me.GVROLE.DetailHeight = 308
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'BtnInsertDate
        '
        Me.BtnInsertDate.AutoHeight = False
        Me.BtnInsertDate.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnInsertDate.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BtnInsertDate.Name = "BtnInsertDate"
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
        'CityID
        '
        Me.CityID.AutoHeight = False
        Me.CityID.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CityID.Name = "CityID"
        Me.CityID.NullText = ""
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
        'BtnConfirm
        '
        Me.BtnConfirm.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources.handconfirm
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnConfirm.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnConfirm.Name = "BtnConfirm"
        Me.BtnConfirm.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
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
        'UserID
        '
        Me.UserID.Caption = "UserID"
        Me.UserID.Name = "UserID"
        Me.UserID.Visible = True
        Me.UserID.VisibleIndex = 0
        '
        'UserName
        '
        Me.UserName.Caption = "UserName"
        Me.UserName.Name = "UserName"
        Me.UserName.Visible = True
        Me.UserName.VisibleIndex = 1
        '
        'FRMSCREENCONTROLPERMISSIONS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1148, 761)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "FRMSCREENCONTROLPERMISSIONS"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "صلاحيات الشاشات"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnInsertDate.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnInsertDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnOverallVal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnExVal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CityID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnBranchRecieved, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnBranchDeliveredID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnRecievedCurrency, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnDeliveredCurrency, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents UserID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents UserName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnInsertDate As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents BtnRecievedCurrency As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnDeliveredCurrency As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents btnOverallVal As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents BtnExVal As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents BtnBranchRecieved As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnBranchDeliveredID As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents BtnConfirm As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents CityID As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
End Class
