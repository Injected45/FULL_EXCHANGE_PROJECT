<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CurrencyMain
    Inherits DevExpress.XtraEditors.XtraForm

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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.LSBOX = New DevExpress.XtraEditors.ListBoxControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.CuName = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.BtnNew = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnSave = New DevExpress.XtraEditors.SimpleButton()
        Me.IBAN = New DevExpress.XtraEditors.TextEdit()
        Me.CurCode = New DevExpress.XtraEditors.TextEdit()
        Me.IsDefault = New DevExpress.XtraEditors.CheckEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LSBOX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CuName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IBAN.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsDefault.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.LSBOX)
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.CuName)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.BtnNew)
        Me.LayoutControl1.Controls.Add(Me.BtnSave)
        Me.LayoutControl1.Controls.Add(Me.IBAN)
        Me.LayoutControl1.Controls.Add(Me.CurCode)
        Me.LayoutControl1.Controls.Add(Me.IsDefault)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(402, 542)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'LSBOX
        '
        Me.LSBOX.Location = New System.Drawing.Point(16, 215)
        Me.LSBOX.Name = "LSBOX"
        Me.LSBOX.Size = New System.Drawing.Size(370, 269)
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
        Me.CodeID.Size = New System.Drawing.Size(168, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'CuName
        '
        Me.CuName.Location = New System.Drawing.Point(16, 58)
        Me.CuName.Name = "CuName"
        Me.CuName.Properties.Appearance.Options.UseTextOptions = True
        Me.CuName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CuName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CuName.Size = New System.Drawing.Size(301, 36)
        Me.CuName.StyleController = Me.LayoutControl1
        Me.CuName.TabIndex = 5
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
        Me.BtnNew.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.refresh
        Me.BtnNew.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnNew.Location = New System.Drawing.Point(207, 490)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(179, 36)
        Me.BtnNew.StyleController = Me.LayoutControl1
        Me.BtnNew.TabIndex = 2
        Me.BtnNew.Text = "جديد"
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
        Me.BtnSave.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.save
        Me.BtnSave.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.BtnSave.Location = New System.Drawing.Point(16, 490)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(185, 36)
        Me.BtnSave.StyleController = Me.LayoutControl1
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "حفظ"
        '
        'IBAN
        '
        Me.IBAN.Location = New System.Drawing.Point(16, 100)
        Me.IBAN.Name = "IBAN"
        Me.IBAN.Properties.Appearance.Options.UseTextOptions = True
        Me.IBAN.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.IBAN.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.IBAN.Size = New System.Drawing.Size(301, 36)
        Me.IBAN.StyleController = Me.LayoutControl1
        Me.IBAN.TabIndex = 5
        '
        'CurCode
        '
        Me.CurCode.Location = New System.Drawing.Point(16, 142)
        Me.CurCode.Name = "CurCode"
        Me.CurCode.Properties.Appearance.Options.UseTextOptions = True
        Me.CurCode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CurCode.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CurCode.Size = New System.Drawing.Size(301, 36)
        Me.CurCode.StyleController = Me.LayoutControl1
        Me.CurCode.TabIndex = 5
        '
        'IsDefault
        '
        Me.IsDefault.Location = New System.Drawing.Point(16, 184)
        Me.IsDefault.Name = "IsDefault"
        Me.IsDefault.Properties.Caption = "عملة افتراضية"
        Me.IsDefault.Size = New System.Drawing.Size(298, 25)
        Me.IsDefault.StyleController = Me.LayoutControl1
        Me.IsDefault.TabIndex = 7
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem8, Me.LayoutControlItem2, Me.LayoutControlItem4, Me.LayoutControlItem11, Me.LayoutControlItem7, Me.LayoutControlItem6, Me.LayoutControlItem3, Me.LayoutControlItem9})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(402, 542)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(133, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(243, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(53, 21)
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
        Me.LayoutControlItem2.Control = Me.CuName
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(376, 42)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(53, 21)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.BtnNew
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(191, 474)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(185, 42)
        Me.LayoutControlItem4.Text = "LayoutControlItem2"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.BtnSave
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(0, 474)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(191, 42)
        Me.LayoutControlItem11.Text = "LayoutControlItem1"
        Me.LayoutControlItem11.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem11.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.CurCode
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(376, 42)
        Me.LayoutControlItem7.Text = "رمز العملة"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(53, 21)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.IBAN
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(376, 42)
        Me.LayoutControlItem6.Text = "كود العملة"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(53, 21)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.LSBOX
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 199)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(376, 275)
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.IsDefault
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 168)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 75, 3, 3)
        Me.LayoutControlItem9.Size = New System.Drawing.Size(376, 31)
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextVisible = False
        '
        'CurrencyMain
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(402, 542)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Font = New System.Drawing.Font("Hacen Tunisia", 11.25!)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.information_point_svgrepo_com
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CurrencyMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إضافة اسم عملة"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LSBOX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CuName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IBAN.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsDefault.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CuName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnNew As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BtnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents IBAN As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CurCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LSBOX As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents IsDefault As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
End Class