<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMViewADDPARTNER
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMViewADDPARTNER))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrentAcc = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Phone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.PTRATE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.IsActive = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemToggleSwitch11 = New DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TypeID = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemToggleSwitch11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1751, 545)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.Location = New System.Drawing.Point(18, 14)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemToggleSwitch11})
        Me.GCRole.Size = New System.Drawing.Size(1715, 517)
        Me.GCRole.TabIndex = 0
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.CurrentAcc, Me.TypeID, Me.AccName, Me.BName, Me.Phone1, Me.PTRATE, Me.IsActive})
        Me.GVRole.DetailHeight = 267
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.PopupEditFormWidth = 914
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.MinWidth = 23
        Me.SN.Name = "SN"
        Me.SN.Summary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SN", "{0}")})
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 124
        '
        'CurrentAcc
        '
        Me.CurrentAcc.Caption = "رقم الحساب"
        Me.CurrentAcc.FieldName = "CurrentAcc"
        Me.CurrentAcc.MinWidth = 23
        Me.CurrentAcc.Name = "CurrentAcc"
        Me.CurrentAcc.Width = 86
        '
        'AccName
        '
        Me.AccName.Caption = "الحساب"
        Me.AccName.FieldName = "AccName"
        Me.AccName.MinWidth = 23
        Me.AccName.Name = "AccName"
        Me.AccName.Visible = True
        Me.AccName.VisibleIndex = 3
        Me.AccName.Width = 415
        '
        'BName
        '
        Me.BName.Caption = "الفرع"
        Me.BName.FieldName = "BName"
        Me.BName.MinWidth = 23
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 1
        Me.BName.Width = 306
        '
        'Phone1
        '
        Me.Phone1.Caption = "الهاتف"
        Me.Phone1.FieldName = "Phone1"
        Me.Phone1.MinWidth = 23
        Me.Phone1.Name = "Phone1"
        Me.Phone1.Visible = True
        Me.Phone1.VisibleIndex = 4
        Me.Phone1.Width = 310
        '
        'PTRATE
        '
        Me.PTRATE.Caption = "النسبة"
        Me.PTRATE.FieldName = "PTRATE"
        Me.PTRATE.MinWidth = 23
        Me.PTRATE.Name = "PTRATE"
        Me.PTRATE.Visible = True
        Me.PTRATE.VisibleIndex = 5
        Me.PTRATE.Width = 149
        '
        'IsActive
        '
        Me.IsActive.Caption = "الحالة"
        Me.IsActive.ColumnEdit = Me.RepositoryItemToggleSwitch11
        Me.IsActive.FieldName = "IsActive"
        Me.IsActive.MinWidth = 23
        Me.IsActive.Name = "IsActive"
        Me.IsActive.Visible = True
        Me.IsActive.VisibleIndex = 6
        Me.IsActive.Width = 180
        '
        'RepositoryItemToggleSwitch11
        '
        Me.RepositoryItemToggleSwitch11.AutoHeight = False
        Me.RepositoryItemToggleSwitch11.Name = "RepositoryItemToggleSwitch11"
        Me.RepositoryItemToggleSwitch11.OffText = "Off"
        Me.RepositoryItemToggleSwitch11.OnText = "On"
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1751, 545)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GCRole
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(1721, 523)
        Me.LayoutControlItem2.Text = "LayoutControlItem1"
        Me.LayoutControlItem2.TextVisible = False
        '
        'TypeID
        '
        Me.TypeID.Caption = "نوع الحساب"
        Me.TypeID.FieldName = "TypeID"
        Me.TypeID.Name = "TypeID"
        Me.TypeID.Visible = True
        Me.TypeID.VisibleIndex = 2
        Me.TypeID.Width = 199
        '
        'FRMViewADDPARTNER
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1751, 545)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMViewADDPARTNER.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FRMViewADDPARTNER"
        Me.Text = "عرض"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemToggleSwitch11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrentAcc As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Phone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents IsActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemToggleSwitch11 As DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PTRATE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TypeID As DevExpress.XtraGrid.Columns.GridColumn
End Class
