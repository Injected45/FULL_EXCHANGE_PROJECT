<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ImportItemsStatment
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImportItemsStatment))
        Me.panelFILl = New DevExpress.XtraEditors.PanelControl()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CustName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CateNName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OUTQT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ITMQUT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.UnitPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.NetQut = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.panelFILl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelFILl.SuspendLayout()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panelFILl
        '
        Me.panelFILl.Controls.Add(Me.LayoutControl1)
        Me.panelFILl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelFILl.Location = New System.Drawing.Point(0, 0)
        Me.panelFILl.Name = "panelFILl"
        Me.panelFILl.Size = New System.Drawing.Size(1538, 678)
        Me.panelFILl.TabIndex = 9
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(2, 2)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.LayoutControlGroup2
        Me.LayoutControl1.Size = New System.Drawing.Size(1534, 674)
        Me.LayoutControl1.TabIndex = 2
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(32, 117)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1470, 523)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.CustName, Me.CateNName, Me.OUTQT, Me.ITMQUT, Me.NetQut, Me.UnitPrice})
        Me.GridView1.DetailHeight = 334
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.AppearanceCell.Options.UseTextOptions = True
        Me.SN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.AppearanceHeader.Options.UseTextOptions = True
        Me.SN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 99
        '
        'CustName
        '
        Me.CustName.AppearanceCell.Options.UseTextOptions = True
        Me.CustName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CustName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CustName.AppearanceHeader.Options.UseTextOptions = True
        Me.CustName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CustName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CustName.Caption = "اسم المورد"
        Me.CustName.FieldName = "CustName"
        Me.CustName.Name = "CustName"
        Me.CustName.Visible = True
        Me.CustName.VisibleIndex = 1
        Me.CustName.Width = 386
        '
        'CateNName
        '
        Me.CateNName.AppearanceCell.BackColor = System.Drawing.Color.Transparent
        Me.CateNName.AppearanceCell.BorderColor = System.Drawing.Color.Transparent
        Me.CateNName.AppearanceCell.ForeColor = System.Drawing.Color.Black
        Me.CateNName.AppearanceCell.Options.UseBackColor = True
        Me.CateNName.AppearanceCell.Options.UseBorderColor = True
        Me.CateNName.AppearanceCell.Options.UseForeColor = True
        Me.CateNName.AppearanceCell.Options.UseTextOptions = True
        Me.CateNName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CateNName.AppearanceHeader.Options.UseTextOptions = True
        Me.CateNName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CateNName.Caption = "المادة"
        Me.CateNName.FieldName = "CateNName"
        Me.CateNName.Name = "CateNName"
        Me.CateNName.Visible = True
        Me.CateNName.VisibleIndex = 2
        Me.CateNName.Width = 268
        '
        'OUTQT
        '
        Me.OUTQT.AppearanceCell.BackColor = System.Drawing.Color.Transparent
        Me.OUTQT.AppearanceCell.BorderColor = System.Drawing.Color.Black
        Me.OUTQT.AppearanceCell.Options.UseBackColor = True
        Me.OUTQT.AppearanceCell.Options.UseBorderColor = True
        Me.OUTQT.AppearanceCell.Options.UseTextOptions = True
        Me.OUTQT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OUTQT.AppearanceHeader.Options.UseTextOptions = True
        Me.OUTQT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OUTQT.Caption = "الكمية الإجمالية"
        Me.OUTQT.FieldName = "OUTQT"
        Me.OUTQT.Name = "OUTQT"
        Me.OUTQT.Visible = True
        Me.OUTQT.VisibleIndex = 3
        Me.OUTQT.Width = 207
        '
        'ITMQUT
        '
        Me.ITMQUT.AppearanceCell.Options.UseTextOptions = True
        Me.ITMQUT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ITMQUT.AppearanceHeader.Options.UseTextOptions = True
        Me.ITMQUT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ITMQUT.Caption = "الكمية المستهلكة"
        Me.ITMQUT.FieldName = "ITMQUT"
        Me.ITMQUT.Name = "ITMQUT"
        Me.ITMQUT.Visible = True
        Me.ITMQUT.VisibleIndex = 4
        Me.ITMQUT.Width = 172
        '
        'UnitPrice
        '
        Me.UnitPrice.AppearanceCell.Options.UseTextOptions = True
        Me.UnitPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UnitPrice.AppearanceHeader.Options.UseTextOptions = True
        Me.UnitPrice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UnitPrice.Caption = "متوسط سعر الوحدة"
        Me.UnitPrice.FieldName = "UnitPrice"
        Me.UnitPrice.Name = "UnitPrice"
        Me.UnitPrice.Visible = True
        Me.UnitPrice.VisibleIndex = 6
        Me.UnitPrice.Width = 172
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup2.GroupBordersVisible = False
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup3, Me.EmptySpaceItem1})
        Me.LayoutControlGroup2.Name = "Root"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(1534, 674)
        Me.LayoutControlGroup2.TextVisible = False
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 62)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(1508, 584)
        Me.LayoutControlGroup3.Text = "التفاصيل "
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1476, 529)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.AllowHotTrack = False
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(1508, 62)
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'NetQut
        '
        Me.NetQut.AppearanceCell.Options.UseTextOptions = True
        Me.NetQut.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NetQut.AppearanceHeader.Options.UseTextOptions = True
        Me.NetQut.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NetQut.Caption = "الكمية الموجودة"
        Me.NetQut.FieldName = "NetQut"
        Me.NetQut.Name = "NetQut"
        Me.NetQut.Visible = True
        Me.NetQut.VisibleIndex = 5
        Me.NetQut.Width = 134
        '
        'ImportItemsStatment
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1538, 678)
        Me.Controls.Add(Me.panelFILl)
        Me.IconOptions.Image = CType(resources.GetObject("ImportItemsStatment.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.Name = "ImportItemsStatment"
        Me.Text = "عرض"
        CType(Me.panelFILl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelFILl.ResumeLayout(False)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelFILl As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CustName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CateNName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OUTQT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ITMQUT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents UnitPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents NetQut As DevExpress.XtraGrid.Columns.GridColumn
End Class
