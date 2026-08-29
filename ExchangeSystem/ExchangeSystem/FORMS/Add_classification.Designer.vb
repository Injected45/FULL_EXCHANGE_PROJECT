<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Add_classification
    Inherits FrmMaster

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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.code = New DevExpress.XtraEditors.TextEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CNAME = New DevExpress.XtraEditors.TextEdit()
        Me.AseetType = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.AseetTypedf = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AseetType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AseetTypedf, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.code)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.CNAME)
        Me.LayoutControl1.Controls.Add(Me.AseetType)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(762, 131)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'code
        '
        Me.code.Location = New System.Drawing.Point(497, 16)
        Me.code.Name = "code"
        Me.code.Size = New System.Drawing.Size(171, 36)
        Me.code.StyleController = Me.LayoutControl1
        Me.code.TabIndex = 4
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(257, 15)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.0!)
        Me.BranchID.Properties.Appearance.Options.UseFont = True
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(156, 42)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'CNAME
        '
        Me.CNAME.Location = New System.Drawing.Point(16, 62)
        Me.CNAME.Name = "CNAME"
        Me.CNAME.Size = New System.Drawing.Size(652, 36)
        Me.CNAME.StyleController = Me.LayoutControl1
        Me.CNAME.TabIndex = 5
        '
        'AseetType
        '
        Me.AseetType.Location = New System.Drawing.Point(16, 15)
        Me.AseetType.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.AseetType.Name = "AseetType"
        Me.AseetType.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.0!)
        Me.AseetType.Properties.Appearance.Options.UseFont = True
        Me.AseetType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.AseetType.Properties.NullText = ""
        Me.AseetType.Properties.PopupSizeable = False
        Me.AseetType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.AseetType.Size = New System.Drawing.Size(157, 42)
        Me.AseetType.StyleController = Me.LayoutControl1
        Me.AseetType.TabIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem21, Me.LayoutControlItem2, Me.AseetTypedf})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(762, 131)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.Control = Me.code
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الكود"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(481, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(255, 46)
        Me.LayoutControlItem1.Text = "الكود"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(62, 21)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem21.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem21.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(241, 0)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2)
        Me.LayoutControlItem21.Size = New System.Drawing.Size(240, 46)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(62, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem2.Control = Me.CNAME
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم الاصل"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 46)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(736, 59)
        Me.LayoutControlItem2.Text = "اسم التصنيف"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(62, 21)
        '
        'AseetTypedf
        '
        Me.AseetTypedf.AppearanceItemCaption.Options.UseTextOptions = True
        Me.AseetTypedf.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AseetTypedf.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.AseetTypedf.Control = Me.AseetType
        Me.AseetTypedf.ControlAlignment = System.Drawing.ContentAlignment.TopLeft
        Me.AseetTypedf.CustomizationFormText = "الفرع"
        Me.AseetTypedf.Location = New System.Drawing.Point(0, 0)
        Me.AseetTypedf.Name = "AseetTypedf"
        Me.AseetTypedf.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2)
        Me.AseetTypedf.Size = New System.Drawing.Size(241, 46)
        Me.AseetTypedf.Text = "نوع التصنيف"
        Me.AseetTypedf.TextSize = New System.Drawing.Size(62, 21)
        '
        'Add_classification
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(762, 174)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "Add_classification"
        Me.Text = "أضافة تصنيف"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AseetType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AseetTypedf, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CNAME As DevExpress.XtraEditors.TextEdit
    Friend WithEvents AseetType As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AseetTypedf As DevExpress.XtraLayout.LayoutControlItem
End Class
