<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCurrencyMovement
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
        Dim SuperToolTip1 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
        Dim ToolTipItem1 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCurrencyMovement))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.OverAllDebit = New DevExpress.XtraEditors.SpinEdit()
        Me.OverAllCredit = New DevExpress.XtraEditors.SpinEdit()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.D1 = New DevExpress.XtraEditors.DateEdit()
        Me.D2 = New DevExpress.XtraEditors.DateEdit()
        Me.BtnPrint = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCI1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCI2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OverAllDebit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OverAllCredit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCI1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCI2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.OverAllDebit)
        Me.LayoutControl1.Controls.Add(Me.OverAllCredit)
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.D1)
        Me.LayoutControl1.Controls.Add(Me.D2)
        Me.LayoutControl1.Controls.Add(Me.BtnPrint)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1372, 583)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(1047, 51)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.ImmediatePopup = True
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Size = New System.Drawing.Size(264, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(732, 51)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.ImmediatePopup = True
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Size = New System.Drawing.Size(272, 36)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 2
        '
        'OverAllDebit
        '
        Me.OverAllDebit.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllDebit.Location = New System.Drawing.Point(376, 51)
        Me.OverAllDebit.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.OverAllDebit.Name = "OverAllDebit"
        Me.OverAllDebit.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.OverAllDebit.Properties.Appearance.ForeColor = System.Drawing.Color.White
        Me.OverAllDebit.Properties.Appearance.Options.UseBackColor = True
        Me.OverAllDebit.Properties.Appearance.Options.UseForeColor = True
        Me.OverAllDebit.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllDebit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllDebit.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllDebit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.OverAllDebit.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.OverAllDebit.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverAllDebit.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverAllDebit.Properties.MaskSettings.Set("valueAfterDelete", Nothing)
        Me.OverAllDebit.Properties.MaskSettings.Set("mask", "n")
        Me.OverAllDebit.Properties.ReadOnly = True
        Me.OverAllDebit.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllDebit.Size = New System.Drawing.Size(246, 36)
        Me.OverAllDebit.StyleController = Me.LayoutControl1
        Me.OverAllDebit.TabIndex = 3
        '
        'OverAllCredit
        '
        Me.OverAllCredit.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllCredit.Location = New System.Drawing.Point(32, 51)
        Me.OverAllCredit.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.OverAllCredit.Name = "OverAllCredit"
        Me.OverAllCredit.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(211, Byte), Integer), CType(CType(34, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.OverAllCredit.Properties.Appearance.ForeColor = System.Drawing.Color.White
        Me.OverAllCredit.Properties.Appearance.Options.UseBackColor = True
        Me.OverAllCredit.Properties.Appearance.Options.UseForeColor = True
        Me.OverAllCredit.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllCredit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllCredit.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllCredit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.OverAllCredit.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.OverAllCredit.Properties.MaskSettings.Set("mask", "n")
        Me.OverAllCredit.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverAllCredit.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverAllCredit.Properties.ReadOnly = True
        Me.OverAllCredit.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllCredit.Size = New System.Drawing.Size(234, 36)
        Me.OverAllCredit.StyleController = Me.LayoutControl1
        Me.OverAllCredit.TabIndex = 4
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCRole.Location = New System.Drawing.Point(16, 150)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(1340, 418)
        Me.GCRole.TabIndex = 8
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.DetailHeight = 279
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'D1
        '
        Me.D1.EditValue = Nothing
        Me.D1.Location = New System.Drawing.Point(732, 93)
        Me.D1.Name = "D1"
        Me.D1.Properties.Appearance.Options.UseTextOptions = True
        Me.D1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.D1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.D1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Size = New System.Drawing.Size(274, 36)
        Me.D1.StyleController = Me.LayoutControl1
        Me.D1.TabIndex = 5
        '
        'D2
        '
        Me.D2.EditValue = Nothing
        Me.D2.Location = New System.Drawing.Point(376, 93)
        Me.D2.Name = "D2"
        Me.D2.Properties.Appearance.Options.UseTextOptions = True
        Me.D2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.D2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.D2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Size = New System.Drawing.Size(247, 36)
        Me.D2.StyleController = Me.LayoutControl1
        Me.D2.TabIndex = 6
        '
        'BtnPrint
        '
        Me.BtnPrint.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.BtnPrint.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.quiries
        Me.BtnPrint.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.BtnPrint.Location = New System.Drawing.Point(32, 94)
        Me.BtnPrint.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(236, 34)
        Me.BtnPrint.StyleController = Me.LayoutControl1
        ToolTipItem1.Text = "طباعة"
        SuperToolTip1.Items.Add(ToolTipItem1)
        Me.BtnPrint.SuperTip = SuperToolTip1
        Me.BtnPrint.TabIndex = 7
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1, Me.LayoutControlItem5})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1372, 583)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.LayoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.LayoutControlItem6, Me.LCI1, Me.LCI2, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.EmptySpaceItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1346, 135)
        Me.LayoutControlGroup1.Text = "بيانات أساسية"
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.BranchID
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "المصرف"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(1015, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(299, 42)
        Me.LayoutControlItem3.Text = "الفرع"
        Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(24, 22)
        Me.LayoutControlItem3.TextToControlDistance = 5
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.CurrencyID
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "المصرف"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(700, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem6.Text = "العملة"
        Me.LayoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(32, 22)
        Me.LayoutControlItem6.TextToControlDistance = 5
        '
        'LCI1
        '
        Me.LCI1.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LCI1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LCI1.Control = Me.OverAllDebit
        Me.LCI1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCI1.CustomizationFormText = "مـــــــــــــــــــــــــــــــدين"
        Me.LCI1.Location = New System.Drawing.Point(344, 0)
        Me.LCI1.Name = "LCI1"
        Me.LCI1.Size = New System.Drawing.Size(356, 42)
        Me.LCI1.Text = "مـــــــــــــــــــــــــــــــدين"
        Me.LCI1.TextSize = New System.Drawing.Size(88, 22)
        '
        'LCI2
        '
        Me.LCI2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LCI2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LCI2.Control = Me.OverAllCredit
        Me.LCI2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCI2.CustomizationFormText = "دائن"
        Me.LCI2.Location = New System.Drawing.Point(0, 0)
        Me.LCI2.Name = "LCI2"
        Me.LCI2.Size = New System.Drawing.Size(344, 42)
        Me.LCI2.Text = "دائن"
        Me.LCI2.TextSize = New System.Drawing.Size(88, 22)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.D1
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(700, 42)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(315, 42)
        Me.LayoutControlItem1.Text = "مــن"
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(19, 22)
        Me.LayoutControlItem1.TextToControlDistance = 16
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.D2
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(344, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(356, 42)
        Me.LayoutControlItem2.Text = "إلــــــــــــــــــــــــــــــــــى"
        Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(87, 22)
        Me.LayoutControlItem2.TextToControlDistance = 16
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem4.Control = Me.BtnPrint
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 105, 3, 3)
        Me.LayoutControlItem4.Size = New System.Drawing.Size(344, 42)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.AllowHotTrack = False
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(1015, 42)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(299, 42)
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.GCRole
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 135)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(1346, 424)
        Me.LayoutControlItem5.Text = "LayoutControlItem1"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'FrmCurrencyMovement
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1372, 583)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmCurrencyMovement.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FrmCurrencyMovement"
        Me.Text = "عرض حركات العملة"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OverAllDebit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OverAllCredit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCI1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCI2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents OverAllDebit As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents OverAllCredit As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LCI1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LCI2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents D1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents D2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
End Class
