<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAdvancePaymentLoadAllData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAdvancePaymentLoadAllData))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.D1 = New DevExpress.XtraEditors.DateEdit()
        Me.D2 = New DevExpress.XtraEditors.DateEdit()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton12 = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Controls.Add(Me.D1)
        Me.LayoutControl1.Controls.Add(Me.D2)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton12)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(886, 732)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GCRole.Location = New System.Drawing.Point(16, 58)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(854, 658)
        Me.GCRole.TabIndex = 3
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'D1
        '
        Me.D1.EditValue = Nothing
        Me.D1.Location = New System.Drawing.Point(384, 16)
        Me.D1.Name = "D1"
        Me.D1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D1.Properties.DisplayFormat.FormatString = ""
        Me.D1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.D1.Properties.EditFormat.FormatString = ""
        Me.D1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.D1.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.D1.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D1.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.D1.Properties.UseMaskAsDisplayFormat = True
        Me.D1.Size = New System.Drawing.Size(453, 36)
        Me.D1.StyleController = Me.LayoutControl1
        Me.D1.TabIndex = 0
        '
        'D2
        '
        Me.D2.EditValue = Nothing
        Me.D2.Location = New System.Drawing.Point(217, 16)
        Me.D2.Name = "D2"
        Me.D2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.D2.Properties.CalendarTimeProperties.MaskSettings.Set("mask", "yyyy")
        Me.D2.Properties.CalendarTimeProperties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D2.Properties.CalendarTimeProperties.MaskSettings.Set("spinWithCarry", False)
        Me.D2.Properties.CalendarTimeProperties.UseMaskAsDisplayFormat = True
        Me.D2.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.D2.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.D2.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.D2.Properties.UseMaskAsDisplayFormat = True
        Me.D2.Size = New System.Drawing.Size(128, 36)
        Me.D2.StyleController = Me.LayoutControl1
        Me.D2.TabIndex = 2
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(116, 16)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(95, 34)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'SimpleButton12
        '
        Me.SimpleButton12.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton12.Appearance.Options.UseBackColor = True
        Me.SimpleButton12.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton12.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.printer
        Me.SimpleButton12.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton12.Location = New System.Drawing.Point(16, 16)
        Me.SimpleButton12.Name = "SimpleButton12"
        Me.SimpleButton12.Size = New System.Drawing.Size(94, 34)
        Me.SimpleButton12.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(886, 732)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GCRole
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(860, 664)
        Me.LayoutControlItem3.Text = "LayoutControlItem1"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.D1
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الشهر"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(368, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(492, 42)
        Me.LayoutControlItem1.Text = "من"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(17, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.D2
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "السنة"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(201, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(167, 42)
        Me.LayoutControlItem2.Text = "إلى"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(17, 21)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton11
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(100, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(101, 42)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.SimpleButton12
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem5"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(100, 42)
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'FrmAdvancePaymentLoadAllData
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(886, 732)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmAdvancePaymentLoadAllData.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FrmAdvancePaymentLoadAllData"
        Me.Text = "استعلام عن السلف"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.D2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents D1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents D2 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton12 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
End Class
