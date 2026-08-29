<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMBENEFITBRANCHTYPE
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMBENEFITBRANCHTYPE))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.GName = New DevExpress.XtraEditors.TextEdit()
        Me.NuRatio = New DevExpress.XtraEditors.SpinEdit()
        Me.GNum = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.SEARCHTXT = New DevExpress.XtraEditors.TextEdit()
        Me.NuRatio1 = New DevExpress.XtraEditors.SpinEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuRatio.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GNum.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEARCHTXT.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NuRatio1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.GName)
        Me.LayoutControl1.Controls.Add(Me.NuRatio)
        Me.LayoutControl1.Controls.Add(Me.GNum)
        Me.LayoutControl1.Controls.Add(Me.SEARCHTXT)
        Me.LayoutControl1.Controls.Add(Me.NuRatio1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(829, 246)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(407, 16)
        Me.CodeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Properties.ReadOnly = True
        Me.CodeID.Size = New System.Drawing.Size(292, 46)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'GName
        '
        Me.GName.Location = New System.Drawing.Point(407, 68)
        Me.GName.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GName.Name = "GName"
        Me.GName.Properties.Appearance.Options.UseTextOptions = True
        Me.GName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GName.Size = New System.Drawing.Size(292, 46)
        Me.GName.StyleController = Me.LayoutControl1
        Me.GName.TabIndex = 3
        '
        'NuRatio
        '
        Me.NuRatio.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuRatio.Location = New System.Drawing.Point(22, 120)
        Me.NuRatio.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.NuRatio.Name = "NuRatio"
        Me.NuRatio.Properties.Appearance.Options.UseTextOptions = True
        Me.NuRatio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuRatio.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuRatio.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuRatio.Properties.MaskSettings.Set("mask", "f")
        Me.NuRatio.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuRatio.Properties.MaskSettings.Set("hideInsignificantZeros", Nothing)
        Me.NuRatio.Properties.UseMaskAsDisplayFormat = True
        Me.NuRatio.Size = New System.Drawing.Size(677, 46)
        Me.NuRatio.StyleController = Me.LayoutControl1
        Me.NuRatio.TabIndex = 5
        '
        'GNum
        '
        Me.GNum.EditValue = ""
        Me.GNum.Location = New System.Drawing.Point(22, 68)
        Me.GNum.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GNum.Name = "GNum"
        Me.GNum.Properties.Appearance.Options.UseTextOptions = True
        Me.GNum.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GNum.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GNum.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.GNum.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered
        Me.GNum.Properties.Items.AddRange(New Object() {"فرع", "وسيط"})
        Me.GNum.Size = New System.Drawing.Size(269, 46)
        Me.GNum.StyleController = Me.LayoutControl1
        Me.GNum.TabIndex = 4
        '
        'SEARCHTXT
        '
        Me.SEARCHTXT.Location = New System.Drawing.Point(22, 16)
        Me.SEARCHTXT.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SEARCHTXT.Name = "SEARCHTXT"
        Me.SEARCHTXT.Properties.Appearance.Options.UseTextOptions = True
        Me.SEARCHTXT.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SEARCHTXT.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SEARCHTXT.Size = New System.Drawing.Size(269, 46)
        Me.SEARCHTXT.StyleController = Me.LayoutControl1
        Me.SEARCHTXT.TabIndex = 2
        '
        'NuRatio1
        '
        Me.NuRatio1.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.NuRatio1.Location = New System.Drawing.Point(22, 172)
        Me.NuRatio1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.NuRatio1.Name = "NuRatio1"
        Me.NuRatio1.Properties.Appearance.Options.UseTextOptions = True
        Me.NuRatio1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NuRatio1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NuRatio1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.NuRatio1.Properties.MaskSettings.Set("mask", "f")
        Me.NuRatio1.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.NuRatio1.Properties.MaskSettings.Set("hideInsignificantZeros", Nothing)
        Me.NuRatio1.Properties.UseMaskAsDisplayFormat = True
        Me.NuRatio1.Size = New System.Drawing.Size(677, 46)
        Me.NuRatio1.StyleController = Me.LayoutControl1
        Me.NuRatio1.TabIndex = 6
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem12, Me.LayoutControlItem6, Me.LayoutControlItem8})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(829, 246)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(385, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(408, 52)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(86, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.GName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(385, 52)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(408, 52)
        Me.LayoutControlItem2.Text = "اسم المجموعة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(86, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.NuRatio
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "الراتب"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(793, 52)
        Me.LayoutControlItem3.Text = "نسبة الوسيط"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(86, 27)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.GNum
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "الراتب"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(385, 52)
        Me.LayoutControlItem12.Text = "النوع"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(86, 27)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SEARCHTXT
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem6.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleCenter
        Me.LayoutControlItem6.ImageOptions.SvgImage = CType(resources.GetObject("LayoutControlItem6.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.LayoutControlItem6.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(385, 52)
        Me.LayoutControlItem6.Text = " "
        Me.LayoutControlItem6.TextLocation = DevExpress.Utils.Locations.Right
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(86, 27)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.NuRatio1
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "الراتب"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(793, 64)
        Me.LayoutControlItem8.Text = "نسبة الفرع"
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(86, 27)
        '
        'FRMBENEFITBRANCHTYPE
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(829, 299)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FRMBENEFITBRANCHTYPE"
        Me.Text = "تقسيم العمولة حسب نوع الفرع"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuRatio.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GNum.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEARCHTXT.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NuRatio1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents NuRatio As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents GNum As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents SEARCHTXT As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents NuRatio1 As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
End Class
