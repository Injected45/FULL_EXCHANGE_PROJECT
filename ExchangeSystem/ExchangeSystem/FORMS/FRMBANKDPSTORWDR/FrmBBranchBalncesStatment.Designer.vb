<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmBBranchBalncesStatment
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmBBranchBalncesStatment))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl2 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BankName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NetTotal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TextEdit22 = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit22.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl2)
        Me.LayoutControl1.Controls.Add(Me.TextEdit22)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1040, 602)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl2
        '
        Me.GridControl2.Location = New System.Drawing.Point(16, 16)
        Me.GridControl2.MainView = Me.GVRole
        Me.GridControl2.Name = "GridControl2"
        Me.GridControl2.Size = New System.Drawing.Size(1008, 528)
        Me.GridControl2.TabIndex = 4
        Me.GridControl2.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.GVRole.Appearance.FooterPanel.Options.UseBackColor = True
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.BankName, Me.BranchName, Me.NetTotal})
        Me.GVRole.DetailHeight = 334
        Me.GVRole.GridControl = Me.GridControl2
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[True]
        Me.GVRole.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[True]
        Me.GVRole.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[True]
        Me.GVRole.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[True]
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 83
        '
        'BankName
        '
        Me.BankName.Caption = "اسم المصرف"
        Me.BankName.FieldName = "BankName"
        Me.BankName.Name = "BankName"
        Me.BankName.Visible = True
        Me.BankName.VisibleIndex = 1
        Me.BankName.Width = 70
        '
        'BranchName
        '
        Me.BranchName.Caption = "الحساب"
        Me.BranchName.FieldName = "BranchName"
        Me.BranchName.Name = "BranchName"
        Me.BranchName.Visible = True
        Me.BranchName.VisibleIndex = 2
        '
        'NetTotal
        '
        Me.NetTotal.Caption = "الرصيد"
        Me.NetTotal.FieldName = "NetTotal"
        Me.NetTotal.Name = "NetTotal"
        Me.NetTotal.Visible = True
        Me.NetTotal.VisibleIndex = 3
        Me.NetTotal.Width = 95
        '
        'TextEdit22
        '
        Me.TextEdit22.EditValue = "0"
        Me.TextEdit22.Location = New System.Drawing.Point(16, 550)
        Me.TextEdit22.Name = "TextEdit22"
        Me.TextEdit22.Properties.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question
        Me.TextEdit22.Properties.Appearance.ForeColor = System.Drawing.Color.White
        Me.TextEdit22.Properties.Appearance.Options.UseBackColor = True
        Me.TextEdit22.Properties.Appearance.Options.UseForeColor = True
        Me.TextEdit22.Properties.Appearance.Options.UseTextOptions = True
        Me.TextEdit22.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TextEdit22.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TextEdit22.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        Me.TextEdit22.Properties.MaskSettings.Set("mask", "n3")
        Me.TextEdit22.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.TextEdit22.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.TextEdit22.Properties.ReadOnly = True
        Me.TextEdit22.Properties.UseMaskAsDisplayFormat = True
        Me.TextEdit22.Size = New System.Drawing.Size(952, 36)
        Me.TextEdit22.StyleController = Me.LayoutControl1
        Me.TextEdit22.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem7})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1040, 602)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl2
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1014, 534)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.TextEdit22
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "اجمالي المصروف بالعملة المحلية"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 534)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(1014, 42)
        Me.LayoutControlItem7.Text = "الإجمالي"
        Me.LayoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(40, 21)
        Me.LayoutControlItem7.TextToControlDistance = 16
        '
        'FrmBBranchBalncesStatment
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1040, 602)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmBBranchBalncesStatment.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FrmBBranchBalncesStatment"
        Me.Text = "أرصدة المصارف"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit22.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControl2 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents NetTotal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BankName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TextEdit22 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
End Class
