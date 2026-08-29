<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewViewCurrencyBuy
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMIViewNTCURSALES))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.adate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.aISbaunk = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.fromasf = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.acode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.buycur = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BPrice1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.salecur = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Purchasprice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BPrice2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.tosaf = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.LayoutControl1.Size = New System.Drawing.Size(1542, 661)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GCRole.Location = New System.Drawing.Point(16, 16)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(1510, 629)
        Me.GCRole.TabIndex = 5
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.adate, Me.aISbaunk, Me.fromasf, Me.acode, Me.buycur, Me.BPrice1, Me.salecur, Me.Purchasprice, Me.BPrice2, Me.tosaf})
        Me.GVRole.DetailHeight = 317
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'adate
        '
        Me.adate.Caption = "التاريخ"
        Me.adate.FieldName = "adate"
        Me.adate.Name = "adate"
        Me.adate.Visible = True
        Me.adate.VisibleIndex = 1
        '
        'aISbaunk
        '
        Me.aISbaunk.Caption = "نوع السداد"
        Me.aISbaunk.FieldName = "aISbaunk"
        Me.aISbaunk.Name = "aISbaunk"
        Me.aISbaunk.Visible = True
        Me.aISbaunk.VisibleIndex = 2
        '
        'fromasf
        '
        Me.fromasf.Caption = "من حساب"
        Me.fromasf.FieldName = "fromasf"
        Me.fromasf.Name = "fromasf"
        Me.fromasf.Visible = True
        Me.fromasf.VisibleIndex = 3
        '
        'acode
        '
        Me.acode.Caption = "الرمز"
        Me.acode.FieldName = "acode"
        Me.acode.Name = "acode"
        Me.acode.Visible = True
        Me.acode.VisibleIndex = 0
        '
        'buycur
        '
        Me.buycur.Caption = "ع/المشتراه"
        Me.buycur.FieldName = "buycur"
        Me.buycur.Name = "buycur"
        Me.buycur.Visible = True
        Me.buycur.VisibleIndex = 4
        '
        'BPrice1
        '
        Me.BPrice1.Caption = "ق/المشتراه"
        Me.BPrice1.FieldName = "BPrice1"
        Me.BPrice1.Name = "BPrice1"
        Me.BPrice1.Visible = True
        Me.BPrice1.VisibleIndex = 5
        '
        'salecur
        '
        Me.salecur.Caption = "ع/المصروفة"
        Me.salecur.FieldName = "salecur"
        Me.salecur.Name = "salecur"
        Me.salecur.Visible = True
        Me.salecur.VisibleIndex = 6
        '
        'Purchasprice
        '
        Me.Purchasprice.Caption = "سعر الشراء"
        Me.Purchasprice.FieldName = "Purchasprice"
        Me.Purchasprice.Name = "Purchasprice"
        Me.Purchasprice.Visible = True
        Me.Purchasprice.VisibleIndex = 7
        '
        'BPrice2
        '
        Me.BPrice2.Caption = "ق/المصروفة"
        Me.BPrice2.FieldName = "BPrice2"
        Me.BPrice2.Name = "BPrice2"
        Me.BPrice2.Visible = True
        Me.BPrice2.VisibleIndex = 8
        '
        'tosaf
        '
        Me.tosaf.Caption = "لحساب"
        Me.tosaf.FieldName = "tosaf"
        Me.tosaf.Name = "tosaf"
        Me.tosaf.Visible = True
        Me.tosaf.VisibleIndex = 9
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1542, 661)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GCRole
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(1516, 635)
        Me.LayoutControlItem3.Text = "LayoutControlItem1"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'FRMIViewNTCURSALES
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1542, 661)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMIViewNTCURSALES.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMIViewNTCURSALES"
        Me.Text = "FRMIViewNTCURSALES"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents acode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents adate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents aISbaunk As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents fromasf As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents buycur As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BPrice1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents salecur As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Purchasprice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BPrice2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents tosaf As DevExpress.XtraGrid.Columns.GridColumn
End Class
