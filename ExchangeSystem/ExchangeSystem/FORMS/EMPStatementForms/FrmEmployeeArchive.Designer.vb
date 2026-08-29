<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmEmployeeArchive
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
        Dim TableColumnDefinition1 As DevExpress.XtraEditors.TableLayout.TableColumnDefinition = New DevExpress.XtraEditors.TableLayout.TableColumnDefinition()
        Dim TableColumnDefinition2 As DevExpress.XtraEditors.TableLayout.TableColumnDefinition = New DevExpress.XtraEditors.TableLayout.TableColumnDefinition()
        Dim TableColumnDefinition3 As DevExpress.XtraEditors.TableLayout.TableColumnDefinition = New DevExpress.XtraEditors.TableLayout.TableColumnDefinition()
        Dim TableRowDefinition1 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TableRowDefinition2 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TableRowDefinition3 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TableRowDefinition4 As DevExpress.XtraEditors.TableLayout.TableRowDefinition = New DevExpress.XtraEditors.TableLayout.TableRowDefinition()
        Dim TableSpan1 As DevExpress.XtraEditors.TableLayout.TableSpan = New DevExpress.XtraEditors.TableLayout.TableSpan()
        Dim TileViewItemElement1 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement2 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim TileViewItemElement3 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement = New DevExpress.XtraGrid.Views.Tile.TileViewItemElement()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmEmployeeArchive))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TileView1 = New DevExpress.XtraGrid.Views.Tile.TileView()
        Me.EMPNAME = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.ECNAME = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.EMPDATE = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.img = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.IsActive = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.TileViewColumn7 = New DevExpress.XtraGrid.Columns.TileViewColumn()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.AgentGV = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.DBRID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TileView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AgentGV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1459, 573)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem8, Me.LayoutControlItem3, Me.LayoutControlItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1459, 573)
        Me.Root.TextVisible = False
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(16, 58)
        Me.GridControl1.MainView = Me.TileView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1427, 499)
        Me.GridControl1.TabIndex = 2
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.TileView1})
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1433, 505)
        Me.LayoutControlItem1.TextVisible = False
        '
        'TileView1
        '
        Me.TileView1.Appearance.EmptySpace.BackColor = System.Drawing.Color.DarkSlateGray
        Me.TileView1.Appearance.EmptySpace.Options.UseBackColor = True
        Me.TileView1.Appearance.Group.BackColor = System.Drawing.Color.Transparent
        Me.TileView1.Appearance.Group.Options.UseBackColor = True
        Me.TileView1.Appearance.ItemFocused.BackColor = System.Drawing.Color.Transparent
        Me.TileView1.Appearance.ItemFocused.Options.UseBackColor = True
        Me.TileView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.EMPNAME, Me.Code, Me.ECNAME, Me.EMPDATE, Me.img, Me.IsActive, Me.TileViewColumn7})
        Me.TileView1.FocusBorderColor = System.Drawing.Color.Teal
        Me.TileView1.GridControl = Me.GridControl1
        Me.TileView1.Name = "TileView1"
        Me.TileView1.OptionsTiles.IndentBetweenGroups = 54
        Me.TileView1.OptionsTiles.IndentBetweenItems = 12
        Me.TileView1.OptionsTiles.ItemPadding = New System.Windows.Forms.Padding(9)
        Me.TileView1.OptionsTiles.ItemSize = New System.Drawing.Size(420, 212)
        Me.TileView1.OptionsTiles.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.TileView1.OptionsTiles.RowCount = 0
        Me.TileView1.TileColumns.Add(TableColumnDefinition1)
        Me.TileView1.TileColumns.Add(TableColumnDefinition2)
        Me.TileView1.TileColumns.Add(TableColumnDefinition3)
        Me.TileView1.TileRows.Add(TableRowDefinition1)
        Me.TileView1.TileRows.Add(TableRowDefinition2)
        Me.TileView1.TileRows.Add(TableRowDefinition3)
        Me.TileView1.TileRows.Add(TableRowDefinition4)
        TableSpan1.RowSpan = 4
        Me.TileView1.TileSpans.Add(TableSpan1)
        TileViewItemElement1.Column = Me.img
        TileViewItemElement1.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement1.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement1.Text = "img"
        TileViewItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement2.Column = Me.EMPNAME
        TileViewItemElement2.ColumnIndex = 1
        TileViewItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement2.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement2.Text = "EMPNAME"
        TileViewItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement3.ColumnIndex = 2
        TileViewItemElement3.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        TileViewItemElement3.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze
        TileViewItemElement3.Text = ":اسم الموظف"
        TileViewItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
        Me.TileView1.TileTemplate.Add(TileViewItemElement1)
        Me.TileView1.TileTemplate.Add(TileViewItemElement2)
        Me.TileView1.TileTemplate.Add(TileViewItemElement3)
        '
        'EMPNAME
        '
        Me.EMPNAME.Caption = "اسم الموظف"
        Me.EMPNAME.FieldName = "EMPNAME"
        Me.EMPNAME.Name = "EMPNAME"
        Me.EMPNAME.Visible = True
        Me.EMPNAME.VisibleIndex = 0
        '
        'Code
        '
        Me.Code.Caption = "الرقم الوظيفي"
        Me.Code.FieldName = "Code"
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        '
        'ECNAME
        '
        Me.ECNAME.Caption = "التصنيف"
        Me.ECNAME.FieldName = "ECNAME"
        Me.ECNAME.Name = "ECNAME"
        Me.ECNAME.Visible = True
        Me.ECNAME.VisibleIndex = 2
        '
        'EMPDATE
        '
        Me.EMPDATE.Caption = "تاريخ الالتحاق بالعمل"
        Me.EMPDATE.FieldName = "EMPDATE"
        Me.EMPDATE.Name = "EMPDATE"
        Me.EMPDATE.Visible = True
        Me.EMPDATE.VisibleIndex = 3
        '
        'img
        '
        Me.img.Caption = "الصورة"
        Me.img.FieldName = "img"
        Me.img.Name = "img"
        Me.img.Visible = True
        Me.img.VisibleIndex = 4
        '
        'IsActive
        '
        Me.IsActive.Caption = "الحالة"
        Me.IsActive.FieldName = "IsActive"
        Me.IsActive.Name = "IsActive"
        Me.IsActive.Visible = True
        Me.IsActive.VisibleIndex = 5
        '
        'TileViewColumn7
        '
        Me.TileViewColumn7.Caption = "TileViewColumn7"
        Me.TileViewColumn7.Name = "TileViewColumn7"
        Me.TileViewColumn7.Visible = True
        Me.TileViewColumn7.VisibleIndex = 6
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.BranchID
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(643, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(790, 42)
        Me.LayoutControlItem8.Text = "الفرع"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(23, 21)
        '
        'AgentGV
        '
        Me.AgentGV.DetailHeight = 334
        Me.AgentGV.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.AgentGV.Name = "AgentGV"
        Me.AgentGV.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.AgentGV.OptionsView.ShowGroupPanel = False
        '
        'DBRID
        '
        Me.DBRID.Caption = "الرمز"
        Me.DBRID.FieldName = "DBRID"
        Me.DBRID.Name = "DBRID"
        '
        'BName
        '
        Me.BName.Caption = "الاسم"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 0
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(659, 16)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupView = Me.AgentGV
        Me.BranchID.Size = New System.Drawing.Size(745, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.SimpleButton11
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(643, 42)
        Me.LayoutControlItem3.TextVisible = False
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(29, 29)
        Me.SimpleButton11.Location = New System.Drawing.Point(16, 16)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(637, 28)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 7
        Me.SimpleButton11.Text = "عرض"
        '
        'FrmEmployeeArchive
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1459, 573)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmEmployeeArchive.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FrmEmployeeArchive"
        Me.Text = "أرشيف الموظفين"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TileView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AgentGV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents TileView1 As DevExpress.XtraGrid.Views.Tile.TileView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EMPNAME As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents ECNAME As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents EMPDATE As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents img As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents IsActive As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents TileViewColumn7 As DevExpress.XtraGrid.Columns.TileViewColumn
    Friend WithEvents BranchID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents AgentGV As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents DBRID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
End Class
