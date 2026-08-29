<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInternalFastCall
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmInternalFastCall))
        Me.OverAllEx = New DevExpress.XtraEditors.SpinEdit()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.OverAllTotal = New DevExpress.XtraEditors.SpinEdit()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(Me.OverAllEx.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OverAllEx
        '
        Me.OverAllEx.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllEx.Enabled = False
        Me.OverAllEx.Location = New System.Drawing.Point(290, 350)
        Me.OverAllEx.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OverAllEx.Name = "OverAllEx"
        Me.OverAllEx.Properties.AllowMouseWheel = False
        Me.OverAllEx.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllEx.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllEx.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllEx.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.OverAllEx.Properties.MaskSettings.Set("mask", "n")
        Me.OverAllEx.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverAllEx.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverAllEx.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllEx.Size = New System.Drawing.Size(234, 46)
        Me.OverAllEx.StyleController = Me.LayoutControl1
        Me.OverAllEx.TabIndex = 2
        Me.OverAllEx.Visible = False
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.OverAllEx)
        Me.LayoutControl1.Controls.Add(Me.OverAllTotal)
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1589, 413)
        Me.LayoutControl1.TabIndex = 3
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'OverAllTotal
        '
        Me.OverAllTotal.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.OverAllTotal.Enabled = False
        Me.OverAllTotal.Location = New System.Drawing.Point(532, 350)
        Me.OverAllTotal.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OverAllTotal.Name = "OverAllTotal"
        Me.OverAllTotal.Properties.AllowMouseWheel = False
        Me.OverAllTotal.Properties.Appearance.Options.UseTextOptions = True
        Me.OverAllTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverAllTotal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.OverAllTotal.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.OverAllTotal.Properties.MaskSettings.Set("mask", "n")
        Me.OverAllTotal.Properties.MaskSettings.Set("hideInsignificantZeros", False)
        Me.OverAllTotal.Properties.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.OverAllTotal.Properties.UseMaskAsDisplayFormat = True
        Me.OverAllTotal.Size = New System.Drawing.Size(210, 46)
        Me.OverAllTotal.StyleController = Me.LayoutControl1
        Me.OverAllTotal.TabIndex = 1
        Me.OverAllTotal.Visible = False
        '
        'GCROLE
        '
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(5, 3, 5, 3)
        Me.GCROLE.Location = New System.Drawing.Point(21, 17)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.Size = New System.Drawing.Size(1547, 327)
        Me.GCROLE.TabIndex = 0
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.DetailHeight = 334
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsEditForm.PopupEditFormWidth = 1029
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.EmptySpaceItem1, Me.EmptySpaceItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1589, 413)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1555, 333)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.OverAllTotal
        Me.LayoutControlItem2.Location = New System.Drawing.Point(511, 333)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(218, 52)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.OverAllEx
        Me.LayoutControlItem3.Location = New System.Drawing.Point(269, 333)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(242, 52)
        Me.LayoutControlItem3.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 333)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(269, 52)
        '
        'EmptySpaceItem2
        '
        Me.EmptySpaceItem2.Location = New System.Drawing.Point(729, 333)
        Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Size = New System.Drawing.Size(826, 52)
        '
        'FrmInternalFastCall
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1589, 413)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmInternalFastCall.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "FrmInternalFastCall"
        Me.Text = "استعلام"
        CType(Me.OverAllEx.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.OverAllTotal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OverAllEx As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents OverAllTotal As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
End Class
