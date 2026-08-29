<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMTRANSTYPE
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
        Dim WindowsUIButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMTRANSTYPE))
        Dim WindowsUIButtonImageOptions2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsUIButtonImageOptions3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.WBP = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        Me.LSBOX = New DevExpress.XtraEditors.ListBoxControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.TransName = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.CountryID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.GLGV1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LSBOX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GLGV1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.WBP)
        Me.LayoutControl1.Controls.Add(Me.LSBOX)
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.TransName)
        Me.LayoutControl1.Controls.Add(Me.CountryID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(352, 438)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'WBP
        '
        Me.WBP.AppearanceButton.Hovered.BackColor = System.Drawing.Color.White
        Me.WBP.AppearanceButton.Hovered.Options.UseBackColor = True
        Me.WBP.AppearanceButton.Normal.BackColor = System.Drawing.Color.White
        Me.WBP.AppearanceButton.Normal.Options.UseBackColor = True
        Me.WBP.AppearanceButton.Pressed.BackColor = System.Drawing.Color.White
        Me.WBP.AppearanceButton.Pressed.Options.UseBackColor = True
        Me.WBP.BackColor = System.Drawing.Color.Gray
        WindowsUIButtonImageOptions1.SvgImage = CType(resources.GetObject("WindowsUIButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        WindowsUIButtonImageOptions2.SvgImage = CType(resources.GetObject("WindowsUIButtonImageOptions2.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        WindowsUIButtonImageOptions3.SvgImage = CType(resources.GetObject("WindowsUIButtonImageOptions3.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        WindowsUIButtonImageOptions3.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.WBP.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUIButton("جديد", False, WindowsUIButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, CType(1, Short), -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton("حفظ", False, WindowsUIButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, CType(2, Short), -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton("تعديل", False, WindowsUIButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, True, Nothing, True, False, True, CType(3, Short), -1, False)})
        Me.WBP.ForeColor = System.Drawing.Color.White
        Me.WBP.Location = New System.Drawing.Point(16, 382)
        Me.WBP.Name = "WBP"
        Me.WBP.Size = New System.Drawing.Size(320, 40)
        Me.WBP.TabIndex = 7
        Me.WBP.Text = "WindowsUIButtonPanel1"
        '
        'LSBOX
        '
        Me.LSBOX.Location = New System.Drawing.Point(16, 142)
        Me.LSBOX.Name = "LSBOX"
        Me.LSBOX.Size = New System.Drawing.Size(320, 234)
        Me.LSBOX.StyleController = Me.LayoutControl1
        Me.LSBOX.TabIndex = 6
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(149, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(139, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(16, 16)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.ContentAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(127, 36)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 2
        '
        'TransName
        '
        Me.TransName.Location = New System.Drawing.Point(16, 58)
        Me.TransName.Name = "TransName"
        Me.TransName.Properties.Appearance.Options.UseTextOptions = True
        Me.TransName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TransName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TransName.Size = New System.Drawing.Size(272, 36)
        Me.TransName.StyleController = Me.LayoutControl1
        Me.TransName.TabIndex = 5
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(352, 438)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(133, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(193, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(32, 22)
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.Options.UseTextOptions = True
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem8.AppearanceItemCaptionDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem8.Control = Me.IsActiveTG
        Me.LayoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem8.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(133, 42)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.TransName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(326, 42)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(32, 22)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.LSBOX
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(326, 240)
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.WBP
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 366)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(326, 46)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'CountryID
        '
        Me.CountryID.Location = New System.Drawing.Point(16, 100)
        Me.CountryID.Name = "CountryID"
        Me.CountryID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CountryID.Properties.NullText = ""
        Me.CountryID.Properties.PopupView = Me.GLGV1
        Me.CountryID.Size = New System.Drawing.Size(272, 36)
        Me.CountryID.StyleController = Me.LayoutControl1
        Me.CountryID.TabIndex = 0
        '
        'GLGV1
        '
        Me.GLGV1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ID, Me.CCode, Me.CName})
        Me.GLGV1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GLGV1.Name = "GLGV1"
        Me.GLGV1.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GLGV1.OptionsView.ShowGroupPanel = False
        '
        'ID
        '
        Me.ID.Caption = "رقم"
        Me.ID.FieldName = "ID"
        Me.ID.Name = "ID"
        '
        'CCode
        '
        Me.CCode.Caption = "الكود"
        Me.CCode.FieldName = "CCode"
        Me.CCode.Name = "CCode"
        '
        'CName
        '
        Me.CName.Caption = "الدولة"
        Me.CName.FieldName = "CName"
        Me.CName.Name = "CName"
        Me.CName.Visible = True
        Me.CName.VisibleIndex = 0
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.CountryID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "الدولة"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(326, 42)
        Me.LayoutControlItem5.Text = "الدولة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(32, 22)
        '
        'FRMTRANSTYPE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(352, 438)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = CType(resources.GetObject("FRMTRANSTYPE.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMTRANSTYPE"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إضافة نوع تحويل"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LSBOX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GLGV1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LSBOX As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents TransName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents WBP As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CountryID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents GLGV1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
End Class
