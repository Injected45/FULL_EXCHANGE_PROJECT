<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUpdateUEsers2
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.ID_ueser = New DevExpress.XtraEditors.TextEdit()
        Me.UPass = New DevExpress.XtraEditors.LookUpEdit()
        Me.BranchID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Name_forUeser = New DevExpress.XtraEditors.TextEdit()
        Me.ACCID = New DevExpress.XtraEditors.GridLookUpEdit()
        Me.GridLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.ACCIDACCID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ACCNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.ID_ueser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UPass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Name_forUeser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ACCID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.ID_ueser)
        Me.LayoutControl1.Controls.Add(Me.UPass)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Controls.Add(Me.Name_forUeser)
        Me.LayoutControl1.Controls.Add(Me.ACCID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(1034, 364)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'ID_ueser
        '
        Me.ID_ueser.Location = New System.Drawing.Point(43, 68)
        Me.ID_ueser.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ID_ueser.Name = "ID_ueser"
        Me.ID_ueser.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.ID_ueser.Properties.Appearance.Options.UseBackColor = True
        Me.ID_ueser.Properties.Appearance.Options.UseTextOptions = True
        Me.ID_ueser.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ID_ueser.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ID_ueser.Properties.ReadOnly = True
        Me.ID_ueser.Size = New System.Drawing.Size(827, 46)
        Me.ID_ueser.StyleController = Me.LayoutControl1
        Me.ID_ueser.TabIndex = 0
        '
        'UPass
        '
        Me.UPass.Location = New System.Drawing.Point(43, 276)
        Me.UPass.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.UPass.Name = "UPass"
        Me.UPass.Properties.Appearance.Options.UseTextOptions = True
        Me.UPass.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UPass.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.UPass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)})
        Me.UPass.Properties.NullText = ""
        Me.UPass.Properties.UseSystemPasswordChar = True
        Me.UPass.Size = New System.Drawing.Size(827, 46)
        Me.UPass.StyleController = Me.LayoutControl1
        Me.UPass.TabIndex = 5
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(43, 172)
        Me.BranchID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupView = Me.GridView1
        Me.BranchID.Size = New System.Drawing.Size(827, 46)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 3
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.BName, Me.ID})
        Me.GridView1.DetailHeight = 382
        Me.GridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsEditForm.PopupEditFormWidth = 1100
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'BName
        '
        Me.BName.Caption = "الفرع "
        Me.BName.FieldName = "BName"
        Me.BName.MinWidth = 27
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 0
        Me.BName.Width = 103
        '
        'ID
        '
        Me.ID.Caption = "رقم الفرع"
        Me.ID.FieldName = "ID"
        Me.ID.MinWidth = 27
        Me.ID.Name = "ID"
        Me.ID.Width = 103
        '
        'Name_forUeser
        '
        Me.Name_forUeser.Location = New System.Drawing.Point(43, 120)
        Me.Name_forUeser.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name_forUeser.Name = "Name_forUeser"
        Me.Name_forUeser.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.Name_forUeser.Properties.Appearance.Options.UseBackColor = True
        Me.Name_forUeser.Properties.Appearance.Options.UseTextOptions = True
        Me.Name_forUeser.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Name_forUeser.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Name_forUeser.Properties.ReadOnly = True
        Me.Name_forUeser.Size = New System.Drawing.Size(827, 46)
        Me.Name_forUeser.StyleController = Me.LayoutControl1
        Me.Name_forUeser.TabIndex = 2
        '
        'ACCID
        '
        Me.ACCID.Location = New System.Drawing.Point(43, 224)
        Me.ACCID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.ACCID.Name = "ACCID"
        Me.ACCID.Properties.Appearance.Options.UseTextOptions = True
        Me.ACCID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ACCID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ACCID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ACCID.Properties.NullText = ""
        Me.ACCID.Properties.PopupView = Me.GridLookUpEdit1View
        Me.ACCID.Size = New System.Drawing.Size(827, 46)
        Me.ACCID.StyleController = Me.LayoutControl1
        Me.ACCID.TabIndex = 4
        '
        'GridLookUpEdit1View
        '
        Me.GridLookUpEdit1View.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ACCIDACCID, Me.ACCNAME})
        Me.GridLookUpEdit1View.DetailHeight = 382
        Me.GridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridLookUpEdit1View.Name = "GridLookUpEdit1View"
        Me.GridLookUpEdit1View.OptionsEditForm.PopupEditFormWidth = 1100
        Me.GridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridLookUpEdit1View.OptionsView.ShowGroupPanel = False
        '
        'ACCIDACCID
        '
        Me.ACCIDACCID.Caption = "رقم الفرع"
        Me.ACCIDACCID.FieldName = "ACCID"
        Me.ACCIDACCID.MinWidth = 27
        Me.ACCIDACCID.Name = "ACCIDACCID"
        Me.ACCIDACCID.Width = 103
        '
        'ACCNAME
        '
        Me.ACCNAME.Caption = "أسم الخزينة "
        Me.ACCNAME.FieldName = "ACCNAME"
        Me.ACCNAME.MinWidth = 27
        Me.ACCNAME.Name = "ACCNAME"
        Me.ACCNAME.Visible = True
        Me.ACCNAME.VisibleIndex = 0
        Me.ACCNAME.Width = 103
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1034, 364)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(998, 336)
        Me.LayoutControlGroup2.Text = "بيانات المستخدم"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.ID_ueser
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(956, 52)
        Me.LayoutControlItem1.Text = "رقم المستخدم "
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(99, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.Name_forUeser
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(956, 52)
        Me.LayoutControlItem2.Text = "أسم االمستخدم "
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(99, 27)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.BranchID
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(956, 52)
        Me.LayoutControlItem3.Text = "الفرع "
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(99, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.UPass
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 208)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(956, 60)
        Me.LayoutControlItem4.Text = "كلمة المرور "
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(99, 27)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.ACCID
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(956, 52)
        Me.LayoutControlItem5.Text = "الخزينة "
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(99, 27)
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FrmUpdateUEsers2
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1034, 417)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.MaximizeBox = False
        Me.Name = "FrmUpdateUEsers2"
        Me.Text = "شاشة تغير الدخول الخاصة بالمستخدمين"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.ID_ueser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UPass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Name_forUeser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ACCID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents ID_ueser As DevExpress.XtraEditors.TextEdit
    Friend WithEvents UPass As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BranchID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Name_forUeser As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ACCID As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents GridLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ACCIDACCID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ACCNAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
End Class

