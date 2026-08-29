<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExBanks
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.BANKNAME = New DevExpress.XtraEditors.TextEdit()
        Me.LSBOX = New System.Windows.Forms.ListBox()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.BtnNew = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnEdit = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnSave = New DevExpress.XtraEditors.SimpleButton()
        Me.CountryID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.CountryGV = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.CouID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CountryName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BANKNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CountryGV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.BANKNAME)
        Me.LayoutControl1.Controls.Add(Me.LSBOX)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.BtnNew)
        Me.LayoutControl1.Controls.Add(Me.BtnEdit)
        Me.LayoutControl1.Controls.Add(Me.BtnSave)
        Me.LayoutControl1.Controls.Add(Me.CountryID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(333, 416)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(141, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(132, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'BANKNAME
        '
        Me.BANKNAME.Location = New System.Drawing.Point(16, 58)
        Me.BANKNAME.Name = "BANKNAME"
        Me.BANKNAME.Properties.Appearance.Options.UseTextOptions = True
        Me.BANKNAME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BANKNAME.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BANKNAME.Size = New System.Drawing.Size(257, 36)
        Me.BANKNAME.StyleController = Me.LayoutControl1
        Me.BANKNAME.TabIndex = 5
        '
        'LSBOX
        '
        Me.LSBOX.FormattingEnabled = True
        Me.LSBOX.ItemHeight = 22
        Me.LSBOX.Location = New System.Drawing.Point(17, 142)
        Me.LSBOX.MaximumSize = New System.Drawing.Size(300, 300)
        Me.LSBOX.Name = "LSBOX"
        Me.LSBOX.Size = New System.Drawing.Size(300, 202)
        Me.LSBOX.TabIndex = 6
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
        Me.IsActiveTG.Size = New System.Drawing.Size(119, 36)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 2
        '
        'BtnNew
        '
        Me.BtnNew.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(183, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.BtnNew.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnNew.Appearance.Options.UseBackColor = True
        Me.BtnNew.Appearance.Options.UseForeColor = True
        Me.BtnNew.Appearance.Options.UseTextOptions = True
        Me.BtnNew.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnNew.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnNew.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnNew.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnNew.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnNew.AppearanceHovered.Options.UseForeColor = True
        Me.BtnNew.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnNew.AppearancePressed.Options.UseForeColor = True
        Me.BtnNew.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        'Me.BtnNew.ImageOptions.SvgImage = My.Resources.refresh
        Me.BtnNew.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnNew.Location = New System.Drawing.Point(235, 364)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(82, 36)
        Me.BtnNew.StyleController = Me.LayoutControl1
        Me.BtnNew.TabIndex = 2
        Me.BtnNew.Text = "جديد"
        '
        'BtnEdit
        '
        Me.BtnEdit.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(77, Byte), Integer))
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
        Me.BtnEdit.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        'Me.BtnEdit.ImageOptions.SvgImage = My.Resources.editbtn
        Me.BtnEdit.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnEdit.Location = New System.Drawing.Point(16, 364)
        Me.BtnEdit.MaximumSize = New System.Drawing.Size(0, 48)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(104, 36)
        Me.BtnEdit.StyleController = Me.LayoutControl1
        Me.BtnEdit.TabIndex = 0
        Me.BtnEdit.Text = "تعديل"
        '
        'BtnSave
        '
        Me.BtnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(72, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(98, Byte), Integer))
        Me.BtnSave.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnSave.Appearance.Options.UseBackColor = True
        Me.BtnSave.Appearance.Options.UseForeColor = True
        Me.BtnSave.Appearance.Options.UseTextOptions = True
        Me.BtnSave.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnSave.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnSave.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnSave.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnSave.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnSave.AppearanceHovered.Options.UseForeColor = True
        Me.BtnSave.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnSave.AppearancePressed.Options.UseForeColor = True
        Me.BtnSave.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        'Me.BtnSave.ImageOptions.SvgImage = My.Resources.save
        Me.BtnSave.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnSave.Location = New System.Drawing.Point(126, 364)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(103, 36)
        Me.BtnSave.StyleController = Me.LayoutControl1
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "حفظ"
        '
        'CountryID
        '
        Me.CountryID.Location = New System.Drawing.Point(16, 100)
        Me.CountryID.Name = "CountryID"
        Me.CountryID.Properties.Appearance.Options.UseTextOptions = True
        Me.CountryID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CountryID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CountryID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CountryID.Properties.NullText = ""
        Me.CountryID.Properties.PopupView = Me.CountryGV
        Me.CountryID.Size = New System.Drawing.Size(257, 36)
        Me.CountryID.StyleController = Me.LayoutControl1
        Me.CountryID.TabIndex = 14
        '
        'CountryGV
        '
        Me.CountryGV.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.CouID, Me.CountryName})
        Me.CountryGV.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.CountryGV.Name = "CountryGV"
        Me.CountryGV.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.CountryGV.OptionsView.ShowGroupPanel = False
        '
        'CouID
        '
        Me.CouID.Caption = "الرمز"
        Me.CouID.FieldName = "CouID"
        Me.CouID.Name = "CouID"
        '
        'CountryName
        '
        Me.CountryName.Caption = "اسم الدولة"
        Me.CountryName.FieldName = "CountryName"
        Me.CountryName.Name = "CountryName"
        Me.CountryName.Visible = True
        Me.CountryName.VisibleIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem11, Me.LayoutControlItem5, Me.LayoutControlItem6})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(333, 416)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(125, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(182, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(28, 21)
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
        Me.LayoutControlItem8.Size = New System.Drawing.Size(125, 42)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BANKNAME
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(307, 42)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(28, 21)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.LSBOX
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(307, 222)
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.BtnNew
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(219, 348)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(88, 42)
        Me.LayoutControlItem4.Text = "LayoutControlItem2"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.BtnSave
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(110, 348)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(109, 42)
        Me.LayoutControlItem11.Text = "LayoutControlItem1"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem11.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem5.Control = Me.BtnEdit
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 348)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(110, 42)
        Me.LayoutControlItem5.Text = "LayoutControlItem4"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.CountryID
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(307, 42)
        Me.LayoutControlItem6.Text = "الدولة"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(28, 21)
        '
        'FRMBANK
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(333, 416)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Font = New System.Drawing.Font("Hacen Tunisia", 11.25!)
        'Me.IconOptions.SvgImage = My.Resources.information_point_svgrepo_com
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExBanks"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إضافة مصرف"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BANKNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CountryGV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents BANKNAME As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LSBOX As ListBox
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CountryID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents CountryGV As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents CouID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CountryName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
End Class

