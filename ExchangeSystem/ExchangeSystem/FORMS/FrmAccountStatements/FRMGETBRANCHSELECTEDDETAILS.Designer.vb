<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMGETBRANCHSELECTEDDETAILS
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions3 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject9 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject10 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject11 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject12 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMGETBRANCHSELECTEDDETAILS))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SumDebit = New DevExpress.XtraEditors.SpinEdit()
        Me.SumCredit = New DevExpress.XtraEditors.SpinEdit()
        Me.OverAllTotal = New DevExpress.XtraEditors.SpinEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SumDebit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SumCredit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Controls.Add(Me.SumDebit)
        Me.LayoutControl1.Controls.Add(Me.SumCredit)
        Me.LayoutControl1.Controls.Add(Me.OverAllTotal)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1267, 753)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCROLE
        '
        Me.GCROLE.Location = New System.Drawing.Point(16, 16)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.Size = New System.Drawing.Size(1235, 679)
        Me.GCROLE.TabIndex = 0
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'SumDebit
        '
        Me.SumDebit.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.SumDebit.Location = New System.Drawing.Point(857, 701)
        Me.SumDebit.Name = "SumDebit"
        Me.SumDebit.Properties.AllowMouseWheel = False
        Me.SumDebit.Properties.Appearance.BackColor = System.Drawing.Color.Red
        Me.SumDebit.Properties.Appearance.Options.UseBackColor = True
        Me.SumDebit.Properties.Appearance.Options.UseTextOptions = True
        Me.SumDebit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SumDebit.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SumDebit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.SumDebit.Properties.MaskSettings.Set("mask", "n3")
        Me.SumDebit.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.SumDebit.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.SumDebit.Properties.ReadOnly = True
        Me.SumDebit.Properties.UseMaskAsDisplayFormat = True
        Me.SumDebit.Size = New System.Drawing.Size(304, 36)
        Me.SumDebit.StyleController = Me.LayoutControl1
        Me.SumDebit.TabIndex = 2
        '
        'SumCredit
        '
        Me.SumCredit.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.SumCredit.Location = New System.Drawing.Point(445, 701)
        Me.SumCredit.Name = "SumCredit"
        Me.SumCredit.Properties.AllowMouseWheel = False
        Me.SumCredit.Properties.Appearance.BackColor = System.Drawing.Color.Green
        Me.SumCredit.Properties.Appearance.Options.UseBackColor = True
        Me.SumCredit.Properties.Appearance.Options.UseTextOptions = True
        Me.SumCredit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SumCredit.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SumCredit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, False, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.SumCredit.Properties.MaskSettings.Set("mask", "n3")
        Me.SumCredit.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.SumCredit.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.SumCredit.Properties.ReadOnly = True
        Me.SumCredit.Properties.UseMaskAsDisplayFormat = True
        Me.SumCredit.Size = New System.Drawing.Size(316, 36)
        Me.SumCredit.StyleController = Me.LayoutControl1
        Me.SumCredit.TabIndex = 3
        '
        'OverAllTotal
        '
        Me.OverAllTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllTotal.Location = New System.Drawing.Point(16, 701)
        Me.OverAllTotal.Name = "OverAllTotal"
        Me.OverAllTotal.Properties.AllowMouseWheel = False
        Me.OverAllTotal.Properties.Appearance.BackColor = System.Drawing.Color.DodgerBlue
        Me.OverAllTotal.Properties.Appearance.Options.UseBackColor = True
        Me.OverAllTotal.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllTotal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllTotal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, False, False, False, EditorButtonImageOptions3, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject9, SerializableAppearanceObject10, SerializableAppearanceObject11, SerializableAppearanceObject12, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.OverAllTotal.Properties.MaskSettings.Set("mask", "n3")
        Me.OverAllTotal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverAllTotal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverAllTotal.Properties.ReadOnly = True
        Me.OverAllTotal.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllTotal.Size = New System.Drawing.Size(333, 36)
        Me.OverAllTotal.StyleController = Me.LayoutControl1
        Me.OverAllTotal.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1267, 753)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.OptionsTableLayoutItem.ColumnSpan = 3
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1241, 685)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.SumDebit
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "إجمالي المدين"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(841, 685)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.OptionsTableLayoutItem.ColumnIndex = 2
        Me.LayoutControlItem2.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem2.Size = New System.Drawing.Size(400, 42)
        Me.LayoutControlItem2.Text = "إجمالي المدين"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.SumCredit
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "إجمالي الدائن"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(429, 685)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.OptionsTableLayoutItem.ColumnIndex = 1
        Me.LayoutControlItem3.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem3.Size = New System.Drawing.Size(412, 42)
        Me.LayoutControlItem3.Text = "إجمالي الدائن"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(74, 22)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.OverAllTotal
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "الصافي"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 685)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.OptionsTableLayoutItem.RowIndex = 1
        Me.LayoutControlItem4.Size = New System.Drawing.Size(429, 42)
        Me.LayoutControlItem4.Text = "الصافي"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(74, 22)
        '
        'FRMGETBRANCHSELECTEDDETAILS
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1267, 753)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMGETBRANCHSELECTEDDETAILS.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMGETBRANCHSELECTEDDETAILS"
        Me.Text = "تفاصيل الفرع"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SumDebit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SumCredit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SumDebit As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents SumCredit As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents OverAllTotal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
End Class
