<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMVIEWSAFE
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMVIEWSAFE))
        Me.BarManager11 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar11 = New DevExpress.XtraBars.Bar()
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnView = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop1 = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom1 = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft1 = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight1 = New DevExpress.XtraBars.BarDockControl()
        Me.BarSubItem11 = New DevExpress.XtraBars.BarSubItem()
        Me.BarSubItem2 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.SearchTxt = New DevExpress.XtraEditors.TextEdit()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.BarManager11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.SearchTxt.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BarManager11
        '
        Me.BarManager11.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar11})
        Me.BarManager11.DockControls.Add(Me.barDockControlTop1)
        Me.BarManager11.DockControls.Add(Me.barDockControlBottom1)
        Me.BarManager11.DockControls.Add(Me.barDockControlLeft1)
        Me.BarManager11.DockControls.Add(Me.barDockControlRight1)
        Me.BarManager11.Form = Me
        Me.BarManager11.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BarSubItem11, Me.BarSubItem2, Me.BtnView, Me.BarButtonItem2, Me.BarButtonItem3})
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
        'BtnView
        '
        Me.BtnView.Caption = "إضافة خزنة"
        Me.BtnView.Id = 2
        Me.BtnView.Name = "BtnView"
        '
        'barDockControlTop1
        '
        Me.barDockControlTop1.CausesValidation = False
        Me.barDockControlTop1.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop1.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop1.Manager = Me.BarManager11
        Me.barDockControlTop1.Size = New System.Drawing.Size(623, 47)
        '
        'barDockControlBottom1
        '
        Me.barDockControlBottom1.CausesValidation = False
        Me.barDockControlBottom1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom1.Location = New System.Drawing.Point(0, 347)
        Me.barDockControlBottom1.Manager = Me.BarManager11
        Me.barDockControlBottom1.Size = New System.Drawing.Size(623, 0)
        '
        'barDockControlLeft1
        '
        Me.barDockControlLeft1.CausesValidation = False
        Me.barDockControlLeft1.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft1.Location = New System.Drawing.Point(0, 47)
        Me.barDockControlLeft1.Manager = Me.BarManager11
        Me.barDockControlLeft1.Size = New System.Drawing.Size(0, 300)
        '
        'barDockControlRight1
        '
        Me.barDockControlRight1.CausesValidation = False
        Me.barDockControlRight1.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight1.Location = New System.Drawing.Point(623, 47)
        Me.barDockControlRight1.Manager = Me.BarManager11
        Me.barDockControlRight1.Size = New System.Drawing.Size(0, 300)
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
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "حذف"
        Me.BarButtonItem3.Id = 4
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.SearchTxt)
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 47)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(623, 300)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'SearchTxt
        '
        Me.SearchTxt.Location = New System.Drawing.Point(16, 16)
        Me.SearchTxt.Name = "SearchTxt"
        Me.SearchTxt.Size = New System.Drawing.Size(454, 40)
        Me.SearchTxt.StyleController = Me.LayoutControl1
        Me.SearchTxt.TabIndex = 5
        '
        'GCRole
        '
        Me.GCRole.Location = New System.Drawing.Point(16, 62)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(591, 222)
        Me.GCRole.TabIndex = 4
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(623, 300)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.SearchTxt
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "بحث حسب اسم التحكم"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(597, 46)
        Me.LayoutControlItem1.Text = "بحث حسب اسم الخزنة"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(121, 25)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GCRole
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 46)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(597, 228)
        Me.LayoutControlItem2.Text = "LayoutControlItem1"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'FRMVIEWSAFE
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 347)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.barDockControlLeft1)
        Me.Controls.Add(Me.barDockControlRight1)
        Me.Controls.Add(Me.barDockControlBottom1)
        Me.Controls.Add(Me.barDockControlTop1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMVIEWSAFE.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMVIEWSAFE"
        Me.Text = "شاشة عرض الخزنات"
        CType(Me.BarManager11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.SearchTxt.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BarManager11 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar11 As DevExpress.XtraBars.Bar
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnView As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents barDockControlTop1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight1 As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BarSubItem11 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem2 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SearchTxt As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
End Class
