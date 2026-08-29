<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmViewBranchSafeDetails
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
        Dim SuperToolTip1 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
        Dim ToolTipItem1 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmViewBranchSafeDetails))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.BtnPrint = New DevExpress.XtraEditors.SimpleButton()
        Me.D1 = New DevExpress.XtraEditors.DateEdit()
        Me.D2 = New DevExpress.XtraEditors.DateEdit()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.count = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.BtnPrint)
        Me.LayoutControl1.Controls.Add(Me.D1)
        Me.LayoutControl1.Controls.Add(Me.D2)
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1111, 768)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'BtnPrint
        '
        Me.BtnPrint.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.BtnPrint.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.quiries
        Me.BtnPrint.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.BtnPrint.Location = New System.Drawing.Point(16, 17)
        Me.BtnPrint.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(131, 34)
        Me.BtnPrint.StyleController = Me.LayoutControl1
        ToolTipItem1.Text = "طباعة"
        SuperToolTip1.Items.Add(ToolTipItem1)
        Me.BtnPrint.SuperTip = SuperToolTip1
        Me.BtnPrint.TabIndex = 4
        '
        'D1
        '
        Me.D1.EditValue = Nothing
        Me.D1.Location = New System.Drawing.Point(450, 16)
        Me.D1.Name = "D1"
        Me.D1.Properties.Appearance.Options.UseTextOptions = True
        Me.D1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.D1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.D1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D1.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.D1.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.D1.Properties.UseMaskAsDisplayFormat = True
        Me.D1.Size = New System.Drawing.Size(267, 36)
        Me.D1.StyleController = Me.LayoutControl1
        Me.D1.TabIndex = 2
        '
        'D2
        '
        Me.D2.EditValue = Nothing
        Me.D2.Location = New System.Drawing.Point(153, 16)
        Me.D2.Name = "D2"
        Me.D2.Properties.Appearance.Options.UseTextOptions = True
        Me.D2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.D2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.D2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.D2.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D2.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.D2.Properties.UseMaskAsDisplayFormat = True
        Me.D2.Size = New System.Drawing.Size(252, 36)
        Me.D2.StyleController = Me.LayoutControl1
        Me.D2.TabIndex = 3
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCRole.Location = New System.Drawing.Point(32, 101)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(1047, 635)
        Me.GCRole.TabIndex = 5
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.AccName, Me.count})
        Me.GVRole.DetailHeight = 308
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.AppearanceCell.Options.UseTextOptions = True
        Me.SN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.AppearanceHeader.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.0!)
        Me.SN.AppearanceHeader.Options.UseFont = True
        Me.SN.AppearanceHeader.Options.UseTextOptions = True
        Me.SN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 161
        '
        'AccName
        '
        Me.AccName.AppearanceCell.Options.UseTextOptions = True
        Me.AccName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccName.AppearanceHeader.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.0!)
        Me.AccName.AppearanceHeader.Options.UseFont = True
        Me.AccName.AppearanceHeader.Options.UseTextOptions = True
        Me.AccName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccName.Caption = "اسم الموظف"
        Me.AccName.FieldName = "AccName"
        Me.AccName.Name = "AccName"
        Me.AccName.Visible = True
        Me.AccName.VisibleIndex = 1
        Me.AccName.Width = 520
        '
        'count
        '
        Me.count.AppearanceCell.Options.UseTextOptions = True
        Me.count.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.count.AppearanceHeader.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.0!)
        Me.count.AppearanceHeader.Options.UseFont = True
        Me.count.AppearanceHeader.Options.UseTextOptions = True
        Me.count.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.count.Caption = "عدد الحركات"
        Me.count.FieldName = "count"
        Me.count.Name = "count"
        Me.count.Visible = True
        Me.count.VisibleIndex = 2
        Me.count.Width = 334
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(762, 16)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.ImmediatePopup = True
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(294, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 7
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup3, Me.LayoutControlItem4, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1111, 768)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.AppearanceGroup.BorderColor = System.Drawing.Color.FromArgb(CType(CType(79, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup3.CustomizationFormText = " "
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem5})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(1085, 700)
        Me.LayoutControlGroup3.Text = " "
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.GCRole
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(1053, 641)
        Me.LayoutControlItem5.Text = "LayoutControlItem1"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem4.Control = Me.BtnPrint
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(137, 42)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.D1
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(434, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(312, 42)
        Me.LayoutControlItem1.Text = "من"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(23, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.D2
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(137, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(297, 42)
        Me.LayoutControlItem2.Text = "إلى"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(23, 21)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.BranchID
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "المصرف"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(746, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(339, 42)
        Me.LayoutControlItem3.Text = "الفرع"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(23, 21)
        '
        'FrmViewBranchSafeDetails
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1111, 768)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmViewBranchSafeDetails.IconOptions.Image"), System.Drawing.Image)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.details
        Me.Name = "FrmViewBranchSafeDetails"
        Me.Text = "حركة خزائن الموظفين"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents BtnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents D1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents D2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents count As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
