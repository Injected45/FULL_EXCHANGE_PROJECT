<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProSupplier
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CodeID = New DevExpress.XtraEditors.TextEdit()
        Me.CUSTNAME = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.PHONE1 = New DevExpress.XtraEditors.TextEdit()
        Me.PHONE2 = New DevExpress.XtraEditors.TextEdit()
        Me.CUSTADDRESS = New DevExpress.XtraEditors.TextEdit()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.CanDebit = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CUSTNAME.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PHONE1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PHONE2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CUSTADDRESS.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CanDebit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CodeID)
        Me.LayoutControl1.Controls.Add(Me.CUSTNAME)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.PHONE1)
        Me.LayoutControl1.Controls.Add(Me.PHONE2)
        Me.LayoutControl1.Controls.Add(Me.CUSTADDRESS)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.CanDebit)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(611, 239)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CodeID
        '
        Me.CodeID.Location = New System.Drawing.Point(139, 16)
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Properties.Appearance.Options.UseTextOptions = True
        Me.CodeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CodeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CodeID.Size = New System.Drawing.Size(381, 36)
        Me.CodeID.StyleController = Me.LayoutControl1
        Me.CodeID.TabIndex = 0
        '
        'CUSTNAME
        '
        Me.CUSTNAME.Location = New System.Drawing.Point(139, 58)
        Me.CUSTNAME.Name = "CUSTNAME"
        Me.CUSTNAME.Properties.Appearance.Options.UseTextOptions = True
        Me.CUSTNAME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTNAME.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTNAME.Size = New System.Drawing.Size(381, 36)
        Me.CUSTNAME.StyleController = Me.LayoutControl1
        Me.CUSTNAME.TabIndex = 3
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(16, 58)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.ContentAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(117, 36)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 4
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.VIEW
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.SimpleButton11.Location = New System.Drawing.Point(16, 16)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(117, 36)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 2
        '
        'PHONE1
        '
        Me.PHONE1.Location = New System.Drawing.Point(296, 100)
        Me.PHONE1.Name = "PHONE1"
        Me.PHONE1.Properties.Appearance.Options.UseTextOptions = True
        Me.PHONE1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PHONE1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PHONE1.Size = New System.Drawing.Size(224, 36)
        Me.PHONE1.StyleController = Me.LayoutControl1
        Me.PHONE1.TabIndex = 5
        '
        'PHONE2
        '
        Me.PHONE2.Location = New System.Drawing.Point(16, 100)
        Me.PHONE2.Name = "PHONE2"
        Me.PHONE2.Properties.Appearance.Options.UseTextOptions = True
        Me.PHONE2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.PHONE2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.PHONE2.Size = New System.Drawing.Size(199, 36)
        Me.PHONE2.StyleController = Me.LayoutControl1
        Me.PHONE2.TabIndex = 6
        '
        'CUSTADDRESS
        '
        Me.CUSTADDRESS.Location = New System.Drawing.Point(16, 184)
        Me.CUSTADDRESS.Name = "CUSTADDRESS"
        Me.CUSTADDRESS.Properties.Appearance.Options.UseTextOptions = True
        Me.CUSTADDRESS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTADDRESS.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTADDRESS.Size = New System.Drawing.Size(504, 36)
        Me.CUSTADDRESS.StyleController = Me.LayoutControl1
        Me.CUSTADDRESS.TabIndex = 9
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(291, 142)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(229, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 7
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem8, Me.LayoutControlItem4, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem21, Me.LayoutControlItem7})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(611, 239)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CodeID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الرمز"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(123, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(462, 42)
        Me.LayoutControlItem1.Text = "الرمز"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(59, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.CUSTNAME
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(123, 42)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(462, 42)
        Me.LayoutControlItem2.Text = "الاسم"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(59, 21)
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(123, 42)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.SimpleButton11
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(123, 42)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.PHONE1
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(280, 84)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(305, 42)
        Me.LayoutControlItem3.Text = "هاتف"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(59, 21)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.PHONE2
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(280, 42)
        Me.LayoutControlItem5.Text = "جوال"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(59, 21)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.CUSTADDRESS
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 168)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(585, 45)
        Me.LayoutControlItem6.Text = "العنوان"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(59, 21)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(275, 126)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(310, 42)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(59, 21)
        '
        'CanDebit
        '
        Me.CanDebit.Location = New System.Drawing.Point(16, 142)
        Me.CanDebit.Name = "CanDebit"
        Me.CanDebit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CanDebit.Properties.Items.AddRange(New Object() {"نعم", "لا"})
        Me.CanDebit.Size = New System.Drawing.Size(194, 36)
        Me.CanDebit.StyleController = Me.LayoutControl1
        Me.CanDebit.TabIndex = 8
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.CanDebit
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 126)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(275, 42)
        Me.LayoutControlItem7.Text = "تصريح بالآجل"
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(59, 21)
        '
        'FRMCUSTOMER
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(611, 282)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.customer__1_
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.Name = "FrmProSupplier"
        'Me.Tag = "13"
        Me.Text = "شاشة إضافة مورد"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CodeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CUSTNAME.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PHONE1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PHONE2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CUSTADDRESS.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CanDebit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents CodeID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CUSTNAME As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PHONE1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents PHONE2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CUSTADDRESS As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CanDebit As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
End Class
