<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMADDMEMBER
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMADDMEMBER))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.EMPNAME = New DevExpress.XtraEditors.TextEdit()
        Me.PassportNo = New DevExpress.XtraEditors.TextEdit()
        Me.PictureEdit11 = New DevExpress.XtraEditors.PictureEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.ASSOCIATION = New DevExpress.XtraEditors.LookUpEdit()
        Me.PHONE1 = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.lblcurrencyname = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EMPNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PassportNo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ASSOCIATION.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PHONE1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblcurrencyname, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.EMPNAME)
        Me.LayoutControl1.Controls.Add(Me.PassportNo)
        Me.LayoutControl1.Controls.Add(Me.PictureEdit11)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.ASSOCIATION)
        Me.LayoutControl1.Controls.Add(Me.PHONE1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsFocus.EnableAutoTabOrder = False
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(877, 276)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(166, 17)
        Me.CodeID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(599, 46)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'EMPNAME
        '
        Me.EMPNAME.Location = New System.Drawing.Point(166, 69)
        Me.EMPNAME.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.EMPNAME.Name = "EMPNAME"
        Me.EMPNAME.Properties.Appearance.Options.UseTextOptions = True
        Me.EMPNAME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPNAME.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.EMPNAME.Size = New System.Drawing.Size(599, 46)
        Me.EMPNAME.StyleController = Me.LayoutControl1
        Me.EMPNAME.TabIndex = 0
        '
        'PassportNo
        '
        Me.PassportNo.Location = New System.Drawing.Point(441, 161)
        Me.PassportNo.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.PassportNo.Name = "PassportNo"
        Me.PassportNo.Properties.Appearance.Options.UseTextOptions = True
        Me.PassportNo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PassportNo.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PassportNo.Size = New System.Drawing.Size(324, 46)
        Me.PassportNo.StyleController = Me.LayoutControl1
        Me.PassportNo.TabIndex = 1
        '
        'PictureEdit11
        '
        Me.PictureEdit11.EditValue = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.PictureEdit11.Location = New System.Drawing.Point(22, 17)
        Me.PictureEdit11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.PictureEdit11.Name = "PictureEdit11"
        Me.PictureEdit11.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit11.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit11.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit11.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.PictureEdit11.Properties.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.PictureEdit11.Size = New System.Drawing.Size(136, 46)
        Me.PictureEdit11.StyleController = Me.LayoutControl1
        Me.PictureEdit11.TabIndex = 6
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(22, 121)
        Me.IsActiveTG.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(734, 34)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 4
        '
        'ASSOCIATION
        '
        Me.ASSOCIATION.Location = New System.Drawing.Point(22, 213)
        Me.ASSOCIATION.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ASSOCIATION.Name = "ASSOCIATION"
        EditorButtonImageOptions1.SvgImage = CType(resources.GetObject("EditorButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.ASSOCIATION.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.ASSOCIATION.Properties.NullText = ""
        Me.ASSOCIATION.Properties.PopupSizeable = False
        Me.ASSOCIATION.Size = New System.Drawing.Size(743, 46)
        Me.ASSOCIATION.StyleController = Me.LayoutControl1
        Me.ASSOCIATION.TabIndex = 3
        '
        'PHONE1
        '
        Me.PHONE1.Location = New System.Drawing.Point(22, 161)
        Me.PHONE1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.PHONE1.Name = "PHONE1"
        Me.PHONE1.Properties.Appearance.Options.UseTextOptions = True
        Me.PHONE1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PHONE1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PHONE1.Size = New System.Drawing.Size(321, 46)
        Me.PHONE1.StyleController = Me.LayoutControl1
        Me.PHONE1.TabIndex = 2
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.Options.UseTextOptions = True
        Me.SimpleButton1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton1.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.plus
        Me.SimpleButton1.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton1.Location = New System.Drawing.Point(22, 69)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(136, 41)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 7
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem7, Me.LayoutControlItem16, Me.lblcurrencyname, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem8})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(877, 276)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(144, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(697, 52)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(68, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.EMPNAME
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(144, 52)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(697, 52)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(68, 27)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.PassportNo
        Me.LayoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem7.CustomizationFormText = "جواز سفر"
        Me.LayoutControlItem7.Location = New System.Drawing.Point(419, 144)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(422, 52)
        Me.LayoutControlItem7.Text = "رقم الهوية"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(68, 27)
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.PictureEdit11
        Me.LayoutControlItem16.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem16.CustomizationFormText = "LayoutControlItem16"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(144, 52)
        Me.LayoutControlItem16.TextVisible = False
        '
        'lblcurrencyname
        '
        Me.lblcurrencyname.Control = Me.ASSOCIATION
        Me.lblcurrencyname.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.lblcurrencyname.CustomizationFormText = "اسم المخزن"
        Me.lblcurrencyname.Location = New System.Drawing.Point(0, 196)
        Me.lblcurrencyname.Name = "lblcurrencyname"
        Me.lblcurrencyname.Size = New System.Drawing.Size(841, 52)
        Me.lblcurrencyname.Text = "الجمعية"
        Me.lblcurrencyname.TextSize = New System.Drawing.Size(68, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.PHONE1
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 144)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(419, 52)
        Me.LayoutControlItem4.Text = "هاتف"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(68, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.SimpleButton1
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(144, 52)
        Me.LayoutControlItem3.TextVisible = False
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 103, 3, 3)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(841, 40)
        Me.LayoutControlItem8.TextVisible = False
        '
        'FRMADDMEMBER
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(877, 329)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = CType(resources.GetObject("FRMADDMEMBER.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FRMADDMEMBER"
        Me.Text = "إضافة عضو بجمعية"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EMPNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PassportNo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit11.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ASSOCIATION.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PHONE1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblcurrencyname, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents EMPNAME As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PassportNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PictureEdit11 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents ASSOCIATION As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lblcurrencyname As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PHONE1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
