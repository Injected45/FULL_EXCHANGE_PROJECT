<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMADDFORIGNACCOUNT
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
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.IsActiveTG = New DevExpress.XtraEditors.ToggleSwitch()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.CountryID = New DevExpress.XtraEditors.LookUpEdit()
        Me.InsertDate = New DevExpress.XtraEditors.DateEdit()
        Me.CItyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CItyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Controls.Add(Me.IsActiveTG)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.CountryID)
        Me.LayoutControl1.Controls.Add(Me.InsertDate)
        Me.LayoutControl1.Controls.Add(Me.CItyID)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(957, 198)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(507, 17)
        Me.Code.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Code.Name = "Code"
        Me.Code.Properties.Appearance.Options.UseTextOptions = True
        Me.Code.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Code.Size = New System.Drawing.Size(364, 46)
        Me.Code.StyleController = Me.LayoutControl1
        Me.Code.TabIndex = 0
        '
        'IsActiveTG
        '
        Me.IsActiveTG.EditValue = True
        Me.IsActiveTG.Location = New System.Drawing.Point(287, 17)
        Me.IsActiveTG.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.IsActiveTG.Name = "IsActiveTG"
        Me.IsActiveTG.Properties.AutoHeight = False
        Me.IsActiveTG.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.IsActiveTG.Properties.OffText = "غير نشط"
        Me.IsActiveTG.Properties.OnText = "نشط"
        Me.IsActiveTG.Size = New System.Drawing.Size(212, 46)
        Me.IsActiveTG.StyleController = Me.LayoutControl1
        Me.IsActiveTG.TabIndex = 3
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.SimpleButton11.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.searchn
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(28, 28)
        Me.SimpleButton11.Location = New System.Drawing.Point(22, 17)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(257, 41)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 4
        '
        'CountryID
        '
        Me.CountryID.Location = New System.Drawing.Point(507, 69)
        Me.CountryID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CountryID.Name = "CountryID"
        Me.CountryID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CountryID.Properties.NullText = ""
        Me.CountryID.Properties.PopupSizeable = False
        Me.CountryID.Size = New System.Drawing.Size(364, 46)
        Me.CountryID.StyleController = Me.LayoutControl1
        Me.CountryID.TabIndex = 12
        '
        'InsertDate
        '
        Me.InsertDate.EditValue = Nothing
        Me.InsertDate.Location = New System.Drawing.Point(22, 69)
        Me.InsertDate.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Properties.Appearance.Options.UseTextOptions = True
        Me.InsertDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertDate.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.InsertDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.InsertDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.InsertDate.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.InsertDate.Properties.ReadOnly = True
        Me.InsertDate.Properties.UseMaskAsDisplayFormat = True
        Me.InsertDate.Size = New System.Drawing.Size(413, 46)
        Me.InsertDate.StyleController = Me.LayoutControl1
        Me.InsertDate.TabIndex = 2
        '
        'CItyID
        '
        Me.CItyID.Location = New System.Drawing.Point(507, 121)
        Me.CItyID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CItyID.Name = "CItyID"
        Me.CItyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CItyID.Properties.NullText = ""
        Me.CItyID.Properties.PopupSizeable = False
        Me.CItyID.Size = New System.Drawing.Size(364, 46)
        Me.CItyID.StyleController = Me.LayoutControl1
        Me.CItyID.TabIndex = 12
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(22, 121)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Properties.PopupSizeable = False
        Me.CurrencyID.Size = New System.Drawing.Size(413, 46)
        Me.CurrencyID.StyleController = Me.LayoutControl1
        Me.CurrencyID.TabIndex = 12
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem4, Me.LayoutControlItem8, Me.LayoutControlItem6, Me.LayoutControlItem21, Me.LayoutControlItem3, Me.LayoutControlItem1, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(957, 198)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.Code
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "اسم المخزن"
        Me.LayoutControlItem4.Location = New System.Drawing.Point(485, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(436, 52)
        Me.LayoutControlItem4.Text = "الرمز"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(42, 27)
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
        Me.LayoutControlItem8.Location = New System.Drawing.Point(265, 0)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(220, 52)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SimpleButton11
        Me.LayoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(265, 52)
        Me.LayoutControlItem6.Text = "LayoutControlItem4"
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.CountryID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(485, 52)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(436, 52)
        Me.LayoutControlItem21.Text = "الدولة"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(42, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.InsertDate
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(485, 52)
        Me.LayoutControlItem3.Text = "التاريخ"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(42, 27)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CItyID
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "الفرع"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(485, 104)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(436, 66)
        Me.LayoutControlItem1.Text = "المدينة"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(42, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.CurrencyID
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "الفرع"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(485, 66)
        Me.LayoutControlItem2.Text = "العملة"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(42, 27)
        '
        'FRMADDFORIGNACCOUNT
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(957, 251)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.Name = "FRMADDFORIGNACCOUNT"
        Me.Text = "حساب عملات أجنبية"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IsActiveTG.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CountryID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsertDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CItyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents IsActiveTG As DevExpress.XtraEditors.ToggleSwitch
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CountryID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents InsertDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CItyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
End Class
