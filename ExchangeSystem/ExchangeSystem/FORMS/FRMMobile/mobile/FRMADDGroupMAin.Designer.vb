<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMADDGroupMAin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMADDGroupMAin))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.ScreenID = New DevExpress.XtraEditors.TextEdit()
        Me.GroupNAme = New DevExpress.XtraEditors.TextEdit()
        Me.ShortName = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.group_ena = New DevExpress.XtraEditors.TextEdit()
        Me.Main_ID = New DevExpress.XtraEditors.LookUpEdit()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.ScreenID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupNAme.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShortName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.group_ena.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Main_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.ScreenID)
        Me.LayoutControl1.Controls.Add(Me.GroupNAme)
        Me.LayoutControl1.Controls.Add(Me.ShortName)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.group_ena)
        Me.LayoutControl1.Controls.Add(Me.Main_ID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(792, 309)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'ScreenID
        '
        Me.ScreenID.Location = New System.Drawing.Point(99, 59)
        Me.ScreenID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ScreenID.Name = "ScreenID"
        Me.ScreenID.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.ScreenID.Properties.Appearance.Options.UseBackColor = True
        Me.ScreenID.Properties.Appearance.Options.UseTextOptions = True
        Me.ScreenID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ScreenID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ScreenID.Properties.ReadOnly = True
        Me.ScreenID.Size = New System.Drawing.Size(574, 36)
        Me.ScreenID.StyleController = Me.LayoutControl1
        Me.ScreenID.TabIndex = 4
        '
        'GroupNAme
        '
        Me.GroupNAme.Location = New System.Drawing.Point(32, 103)
        Me.GroupNAme.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupNAme.Name = "GroupNAme"
        Me.GroupNAme.Properties.Appearance.Options.UseTextOptions = True
        Me.GroupNAme.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GroupNAme.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GroupNAme.Size = New System.Drawing.Size(641, 36)
        Me.GroupNAme.StyleController = Me.LayoutControl1
        Me.GroupNAme.TabIndex = 5
        '
        'ShortName
        '
        Me.ShortName.Location = New System.Drawing.Point(32, 229)
        Me.ShortName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ShortName.Name = "ShortName"
        Me.ShortName.Properties.Appearance.Options.UseTextOptions = True
        Me.ShortName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ShortName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ShortName.Size = New System.Drawing.Size(641, 36)
        Me.ShortName.StyleController = Me.LayoutControl1
        Me.ShortName.TabIndex = 6
        '
        'SimpleButton1
        '
        Me.SimpleButton1.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1.Location = New System.Drawing.Point(32, 59)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(61, 38)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 10
        Me.SimpleButton1.Text = " "
        '
        'group_ena
        '
        Me.group_ena.Location = New System.Drawing.Point(32, 145)
        Me.group_ena.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.group_ena.Name = "group_ena"
        Me.group_ena.Properties.Appearance.Options.UseTextOptions = True
        Me.group_ena.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.group_ena.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.group_ena.Size = New System.Drawing.Size(641, 36)
        Me.group_ena.StyleController = Me.LayoutControl1
        Me.group_ena.TabIndex = 5
        '
        'Main_ID
        '
        Me.Main_ID.Location = New System.Drawing.Point(32, 187)
        Me.Main_ID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Main_ID.Name = "Main_ID"
        Me.Main_ID.Properties.Appearance.Options.UseTextOptions = True
        Me.Main_ID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Main_ID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Main_ID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Main_ID.Properties.NullText = ""
        Me.Main_ID.Size = New System.Drawing.Size(641, 36)
        Me.Main_ID.StyleController = Me.LayoutControl1
        Me.Main_ID.TabIndex = 5
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        Me.LayoutControlGroup1.Name = "Root"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(792, 309)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem7, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(766, 283)
        Me.LayoutControlGroup2.Text = "اضافة تــبــويــب"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.Control = Me.ScreenID
        Me.LayoutControlItem1.CustomizationFormText = "رقم التبــويــب"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(67, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(667, 44)
        Me.LayoutControlItem1.Text = "رقم التبويب"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem2.Control = Me.GroupNAme
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(734, 42)
        Me.LayoutControlItem2.Text = "الاسم "
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem3.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem3.Control = Me.ShortName
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 170)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(734, 54)
        Me.LayoutControlItem3.Text = "اختصار الاســم"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.SimpleButton1
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(67, 44)
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem4.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem4.Control = Me.group_ena
        Me.LayoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem4.CustomizationFormText = "الاسم "
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 86)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(734, 42)
        Me.LayoutControlItem4.Text = "الاسم الانجليزي"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem5.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem5.Control = Me.Main_ID
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "الاسم "
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 128)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(734, 42)
        Me.LayoutControlItem5.Text = "المجموعة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(71, 21)
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRMADDGroupMAin
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 352)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = Global.ExchangeSystem.My.Resources.Resources.othercharts_32x321
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "FRMADDGroupMAin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "أضــافــة تبويب فرعي"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.ScreenID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupNAme.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShortName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.group_ena.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Main_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents ScreenID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GroupNAme As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ShortName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
    Friend WithEvents group_ena As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Main_ID As DevExpress.XtraEditors.LookUpEdit
End Class
