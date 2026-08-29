<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMSELECTACCOUNT
    Inherits DevExpress.XtraEditors.XtraForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMSELECTACCOUNT))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.AccountType = New DevExpress.XtraEditors.LookUpEdit()
        Me.AccID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem17 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.coltreeListColumn0 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.AccountType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AccID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.AccountType)
        Me.LayoutControl1.Controls.Add(Me.AccID)
        resources.ApplyResources(Me.LayoutControl1, "LayoutControl1")
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        '
        'AccountType
        '
        resources.ApplyResources(Me.AccountType, "AccountType")
        Me.AccountType.Name = "AccountType"
        Me.AccountType.Properties.Appearance.Options.UseTextOptions = True
        Me.AccountType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccountType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AccountType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("AccountType.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.AccountType.Properties.NullText = resources.GetString("AccountType.Properties.NullText")
        Me.AccountType.StyleController = Me.LayoutControl1
        '
        'AccID
        '
        resources.ApplyResources(Me.AccID, "AccID")
        Me.AccID.Name = "AccID"
        Me.AccID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("AccID.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
        Me.AccID.Properties.NullText = resources.GetString("AccID.Properties.NullText")
        Me.AccID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains
        Me.AccID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch
        Me.AccID.StyleController = Me.LayoutControl1
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem17, Me.LayoutControlItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(520, 120)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem17
        '
        Me.LayoutControlItem17.Control = Me.AccountType
        resources.ApplyResources(Me.LayoutControlItem17, "LayoutControlItem17")
        Me.LayoutControlItem17.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem17.Name = "LayoutControlItem17"
        Me.LayoutControlItem17.Size = New System.Drawing.Size(494, 42)
        Me.LayoutControlItem17.TextSize = New System.Drawing.Size(53, 21)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.AccID
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(494, 52)
        resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(53, 21)
        '
        'coltreeListColumn0
        '
        resources.ApplyResources(Me.coltreeListColumn0, "coltreeListColumn0")
        Me.coltreeListColumn0.FieldName = "treeListColumn0"
        Me.coltreeListColumn0.Name = "coltreeListColumn0"
        '
        'FRMSELECTACCOUNT
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.IconOptions.SvgImage = CType(resources.GetObject("FRMSELECTACCOUNT.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMSELECTACCOUNT"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.AccountType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AccID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem17 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AccountType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents AccID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents coltreeListColumn0 As DevExpress.XtraTreeList.Columns.TreeListColumn
End Class
