<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddSecrein
    'Inherits DevExpress.XtraEditors.XtraForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmAddSecrein))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.ScreenID = New DevExpress.XtraEditors.TextEdit()
        Me.ScreenName = New DevExpress.XtraEditors.TextEdit()
        Me.EnglishName = New DevExpress.XtraEditors.TextEdit()
        Me.ShortName = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.GroupNAme_ID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.GridLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.id = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GroupNAme = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Canshowe = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.MainScreenID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.MainID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.MainName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CanShow = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.ScreenID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ScreenName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EnglishName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShortName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupNAme_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MainScreenID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.ScreenID)
        Me.LayoutControl1.Controls.Add(Me.ScreenName)
        Me.LayoutControl1.Controls.Add(Me.EnglishName)
        Me.LayoutControl1.Controls.Add(Me.ShortName)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.GroupNAme_ID)
        Me.LayoutControl1.Controls.Add(Me.MainScreenID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(964, 376)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'ScreenID
        '
        Me.ScreenID.Location = New System.Drawing.Point(93, 59)
        Me.ScreenID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ScreenID.Name = "ScreenID"
        Me.ScreenID.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.ScreenID.Properties.Appearance.Options.UseBackColor = True
        Me.ScreenID.Properties.Appearance.Options.UseTextOptions = True
        Me.ScreenID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ScreenID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ScreenID.Properties.ReadOnly = True
        Me.ScreenID.Size = New System.Drawing.Size(752, 36)
        Me.ScreenID.StyleController = Me.LayoutControl1
        Me.ScreenID.TabIndex = 4
        '
        'ScreenName
        '
        Me.ScreenName.Location = New System.Drawing.Point(32, 103)
        Me.ScreenName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ScreenName.Name = "ScreenName"
        Me.ScreenName.Properties.Appearance.Options.UseTextOptions = True
        Me.ScreenName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ScreenName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ScreenName.Size = New System.Drawing.Size(813, 36)
        Me.ScreenName.StyleController = Me.LayoutControl1
        Me.ScreenName.TabIndex = 5
        '
        'EnglishName
        '
        Me.EnglishName.Location = New System.Drawing.Point(32, 145)
        Me.EnglishName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.EnglishName.Name = "EnglishName"
        Me.EnglishName.Properties.Appearance.Options.UseTextOptions = True
        Me.EnglishName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EnglishName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.EnglishName.Size = New System.Drawing.Size(813, 36)
        Me.EnglishName.StyleController = Me.LayoutControl1
        Me.EnglishName.TabIndex = 6
        '
        'ShortName
        '
        Me.ShortName.Location = New System.Drawing.Point(32, 271)
        Me.ShortName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ShortName.Name = "ShortName"
        Me.ShortName.Properties.Appearance.Options.UseTextOptions = True
        Me.ShortName.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ShortName.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ShortName.Size = New System.Drawing.Size(813, 36)
        Me.ShortName.StyleController = Me.LayoutControl1
        Me.ShortName.TabIndex = 9
        '
        'SimpleButton1
        '
        Me.SimpleButton1.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1.Location = New System.Drawing.Point(32, 59)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(55, 38)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 10
        Me.SimpleButton1.Text = " "
        '
        'GroupNAme_ID
        '
        Me.GroupNAme_ID.Location = New System.Drawing.Point(32, 229)
        Me.GroupNAme_ID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupNAme_ID.Name = "GroupNAme_ID"
        Me.GroupNAme_ID.Properties.Appearance.Options.UseTextOptions = True
        Me.GroupNAme_ID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GroupNAme_ID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.GroupNAme_ID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.GroupNAme_ID.Properties.NullText = ""
        Me.GroupNAme_ID.Properties.PopupView = Me.GridLookUpEdit1View
        Me.GroupNAme_ID.Size = New System.Drawing.Size(813, 36)
        Me.GroupNAme_ID.StyleController = Me.LayoutControl1
        Me.GroupNAme_ID.TabIndex = 7
        '
        'GridLookUpEdit1View
        '
        Me.GridLookUpEdit1View.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.id, Me.GroupNAme, Me.Canshowe})
        Me.GridLookUpEdit1View.DetailHeight = 286
        Me.GridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridLookUpEdit1View.Name = "GridLookUpEdit1View"
        Me.GridLookUpEdit1View.OptionsEditForm.PopupEditFormWidth = 900
        Me.GridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridLookUpEdit1View.OptionsView.ShowGroupPanel = False
        '
        'id
        '
        Me.id.Caption = "id"
        Me.id.FieldName = "id"
        Me.id.MinWidth = 22
        Me.id.Name = "id"
        Me.id.Width = 84
        '
        'GroupNAme
        '
        Me.GroupNAme.Caption = "GroupNAme"
        Me.GroupNAme.FieldName = "GroupNAme"
        Me.GroupNAme.MinWidth = 22
        Me.GroupNAme.Name = "GroupNAme"
        Me.GroupNAme.Visible = True
        Me.GroupNAme.VisibleIndex = 0
        Me.GroupNAme.Width = 84
        '
        'Canshowe
        '
        Me.Canshowe.Caption = "Canshowe"
        Me.Canshowe.FieldName = "Canshowe"
        Me.Canshowe.MinWidth = 22
        Me.Canshowe.Name = "Canshowe"
        Me.Canshowe.Width = 84
        '
        'MainScreenID
        '
        Me.MainScreenID.Location = New System.Drawing.Point(32, 187)
        Me.MainScreenID.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MainScreenID.Name = "MainScreenID"
        Me.MainScreenID.Properties.Appearance.Options.UseTextOptions = True
        Me.MainScreenID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.MainScreenID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.MainScreenID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.MainScreenID.Properties.NullText = ""
        Me.MainScreenID.Properties.PopupView = Me.GridView1
        Me.MainScreenID.Size = New System.Drawing.Size(813, 36)
        Me.MainScreenID.StyleController = Me.LayoutControl1
        Me.MainScreenID.TabIndex = 8
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.MainID, Me.MainName, Me.CanShow})
        Me.GridView1.DetailHeight = 286
        Me.GridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsEditForm.PopupEditFormWidth = 900
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'MainID
        '
        Me.MainID.Caption = "MainID"
        Me.MainID.FieldName = "MainID"
        Me.MainID.MinWidth = 22
        Me.MainID.Name = "MainID"
        Me.MainID.Width = 84
        '
        'MainName
        '
        Me.MainName.Caption = "MainName"
        Me.MainName.FieldName = "MainName"
        Me.MainName.MinWidth = 22
        Me.MainName.Name = "MainName"
        Me.MainName.Visible = True
        Me.MainName.VisibleIndex = 0
        Me.MainName.Width = 84
        '
        'CanShow
        '
        Me.CanShow.Caption = "CanShow"
        Me.CanShow.FieldName = "CanShow"
        Me.CanShow.MinWidth = 22
        Me.CanShow.Name = "CanShow"
        Me.CanShow.Width = 84
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(964, 376)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem5, Me.LayoutControlItem6, Me.LayoutControlItem7, Me.LayoutControlItem4})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(938, 350)
        Me.LayoutControlGroup2.Text = "اضافة شاشة"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.ScreenID
        Me.LayoutControlItem1.Location = New System.Drawing.Point(61, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(845, 44)
        Me.LayoutControlItem1.Text = "رقم الشاشة"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.ScreenName
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(906, 42)
        Me.LayoutControlItem2.Text = "الاسم العربي"
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.EnglishName
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 86)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(906, 42)
        Me.LayoutControlItem3.Text = "الاسم الانجليزي"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.MainScreenID
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 128)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(906, 42)
        Me.LayoutControlItem5.Text = "المجموعة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.ShortName
        Me.LayoutControlItem6.Location = New System.Drawing.Point(0, 212)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(906, 79)
        Me.LayoutControlItem6.Text = "اختصار الاسم"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(71, 21)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.SimpleButton1
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(61, 44)
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.GroupNAme_ID
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 170)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(906, 42)
        Me.LayoutControlItem4.Text = "نوع الشاشة "
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(71, 21)
        '
        'FrmAddSecrein
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(964, 419)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = Global.ExchangeSystem.My.Resources.Resources.othercharts_32x321
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "FrmAddSecrein"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "أضافة شاشة جديدة"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.ScreenID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ScreenName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EnglishName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShortName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupNAme_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MainScreenID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents ScreenID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ScreenName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents EnglishName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ShortName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GroupNAme_ID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents GridLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents MainScreenID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents id As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GroupNAme As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Canshowe As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents MainID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents MainName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CanShow As DevExpress.XtraGrid.Columns.GridColumn
End Class
