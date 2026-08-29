<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmViewBalanceTransport
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
        Me.components = New System.ComponentModel.Container()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.BarManager11 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar11 = New DevExpress.XtraBars.Bar()
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop1 = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom1 = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft1 = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight1 = New DevExpress.XtraBars.BarDockControl()
        Me.BarSubItem11 = New DevExpress.XtraBars.BarSubItem()
        Me.BarSubItem2 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem11 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 36)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(734, 339)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.Location = New System.Drawing.Point(12, 12)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(710, 315)
        Me.GCRole.TabIndex = 5
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.DetailHeight = 317
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(734, 339)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCRole
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(714, 319)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'BarManager11
        '
        Me.BarManager11.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar11})
        Me.BarManager11.DockControls.Add(Me.barDockControlTop1)
        Me.BarManager11.DockControls.Add(Me.barDockControlBottom1)
        Me.BarManager11.DockControls.Add(Me.barDockControlLeft1)
        Me.BarManager11.DockControls.Add(Me.barDockControlRight1)
        Me.BarManager11.Form = Me
        Me.BarManager11.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem11, Me.BarSubItem2, Me.BarButtonItem11, Me.BarButtonItem2, Me.BarButtonItem3})
        Me.BarManager11.MainMenu = Me.Bar11
        Me.BarManager11.MaxItemId = 5
        '
        'Bar11
        '
        Me.Bar11.BarName = "Custom 2"
        Me.Bar11.DockCol = 0
        Me.Bar11.DockRow = 0
        Me.Bar11.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar11.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem2)})
        Me.Bar11.OptionsBar.MultiLine = True
        Me.Bar11.OptionsBar.UseWholeRow = True
        Me.Bar11.Text = "Custom 2"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "إغلاق"
        Me.BarButtonItem2.Id = 3
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'barDockControlTop1
        '
        Me.barDockControlTop1.CausesValidation = False
        Me.barDockControlTop1.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop1.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop1.Manager = Me.BarManager11
        Me.barDockControlTop1.Size = New System.Drawing.Size(734, 36)
        '
        'barDockControlBottom1
        '
        Me.barDockControlBottom1.CausesValidation = False
        Me.barDockControlBottom1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom1.Location = New System.Drawing.Point(0, 375)
        Me.barDockControlBottom1.Manager = Me.BarManager11
        Me.barDockControlBottom1.Size = New System.Drawing.Size(734, 0)
        '
        'barDockControlLeft1
        '
        Me.barDockControlLeft1.CausesValidation = False
        Me.barDockControlLeft1.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft1.Location = New System.Drawing.Point(0, 36)
        Me.barDockControlLeft1.Manager = Me.BarManager11
        Me.barDockControlLeft1.Size = New System.Drawing.Size(0, 339)
        '
        'barDockControlRight1
        '
        Me.barDockControlRight1.CausesValidation = False
        Me.barDockControlRight1.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight1.Location = New System.Drawing.Point(734, 36)
        Me.barDockControlRight1.Manager = Me.BarManager11
        Me.barDockControlRight1.Size = New System.Drawing.Size(0, 339)
        '
        'BarSubItem11
        '
        Me.BarSubItem11.Caption = "Save Information"
        Me.BarSubItem11.Id = 0
        Me.BarSubItem11.Name = "BarSubItem11"
        '
        'BarSubItem2
        '
        Me.BarSubItem2.Caption = "Save Information"
        Me.BarSubItem2.Id = 1
        Me.BarSubItem2.Name = "BarSubItem2"
        '
        'BarButtonItem11
        '
        Me.BarButtonItem11.Caption = "إضافة سائق"
        Me.BarButtonItem11.Id = 2
        Me.BarButtonItem11.Name = "BarButtonItem11"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "حذف"
        Me.BarButtonItem3.Id = 4
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'FrmViewBalanceTransport
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(734, 375)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft1)
        Me.Controls.Add(Me.barDockControlRight1)
        Me.Controls.Add(Me.barDockControlBottom1)
        Me.Controls.Add(Me.barDockControlTop1)
        'Me.IconOptions.Image = Global.ShippingSystem.My.Resources.Resources.search_16px1
        Me.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.Name = "FrmViewBalanceTransport"
        Me.Text = "شاشة البحث في نقل الخزائن"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents BarManager11 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar11 As DevExpress.XtraBars.Bar
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarSubItem11 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem2 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem11 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
End Class
