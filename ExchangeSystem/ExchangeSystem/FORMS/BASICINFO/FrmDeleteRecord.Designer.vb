<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDeleteRecord
    Inherits DevExpress.XtraEditors.XtraForm

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
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem23 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.BtnEdit = New DevExpress.XtraEditors.SimpleButton()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.TypeID = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TypeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.BtnEdit)
        Me.LayoutControl1.Controls.Add(Me.TypeID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(688, 161)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Location = New System.Drawing.Point(16, 57)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Size = New System.Drawing.Size(618, 36)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem23, Me.LayoutControlItem5, Me.LayoutControlItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(688, 161)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem23
        '
        Me.LayoutControlItem23.Control = Me.Code
        Me.LayoutControlItem23.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem23.CustomizationFormText = "ملاحظات"
        Me.LayoutControlItem23.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem23.Name = "LayoutControlItem23"
        Me.LayoutControlItem23.Size = New System.Drawing.Size(662, 42)
        Me.LayoutControlItem23.Text = "الرمز"
        Me.LayoutControlItem23.TextSize = New System.Drawing.Size(22, 21)
        '
        'BtnEdit
        '
        Me.BtnEdit.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.BtnEdit.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.Appearance.Options.UseBackColor = True
        Me.BtnEdit.Appearance.Options.UseForeColor = True
        Me.BtnEdit.Appearance.Options.UseTextOptions = True
        Me.BtnEdit.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnEdit.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnEdit.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnEdit.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.AppearanceHovered.Options.UseForeColor = True
        Me.BtnEdit.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.AppearancePressed.Options.UseForeColor = True
        Me.BtnEdit.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
        Me.BtnEdit.ImageOptions.SvgImage = My.Resources.Resources.deletebtn
        Me.BtnEdit.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnEdit.Location = New System.Drawing.Point(16, 99)
        Me.BtnEdit.MaximumSize = New System.Drawing.Size(0, 48)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(656, 47)
        Me.BtnEdit.StyleController = Me.LayoutControl1
        Me.BtnEdit.TabIndex = 0
        Me.BtnEdit.Text = "حذف"
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem5.Control = Me.BtnEdit
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(662, 53)
        Me.LayoutControlItem5.Text = "LayoutControlItem4"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'TypeID
        '
        Me.TypeID.Location = New System.Drawing.Point(16, 15)
        Me.TypeID.Name = "TypeID"
        Me.TypeID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TypeID.Properties.Items.AddRange(New Object() {"حوالة داخلية", "حوالة خارجية"})
        Me.TypeID.Size = New System.Drawing.Size(618, 36)
        Me.TypeID.StyleController = Me.LayoutControl1
        Me.TypeID.TabIndex = 5
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.TypeID
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(662, 42)
        Me.LayoutControlItem1.Text = "النوع"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(22, 21)
        '
        'FrmDeleteRecord
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(688, 161)
        Me.Controls.Add(Me.LayoutControl1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmDeleteRecord"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "حذف سجل"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TypeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem23 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents TypeID As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
End Class
