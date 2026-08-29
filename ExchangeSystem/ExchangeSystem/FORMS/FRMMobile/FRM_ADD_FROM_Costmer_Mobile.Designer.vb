<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_ADD_FROM_Costmer_Mobile
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM_ADD_FROM_Costmer_Mobile))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.NAME_for_Cousntas = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.National_Number = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.phone = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CityName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Type_Account = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.inserDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.FormattedTime = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OK = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.GridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Cansel = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.TabbedControlGroup1 = New DevExpress.XtraLayout.TabbedControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Cansel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1363, 536)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(32, 63)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.Cansel, Me.OK})
        Me.GridControl1.Size = New System.Drawing.Size(1299, 441)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.ID, Me.NAME_for_Cousntas, Me.CName, Me.National_Number, Me.phone, Me.CityName, Me.Type_Account, Me.inserDate, Me.FormattedTime, Me.GridColumn1, Me.GridColumn2})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 51
        '
        'ID
        '
        Me.ID.Caption = "ID"
        Me.ID.FieldName = "ID"
        Me.ID.Name = "ID"
        '
        'NAME_for_Cousntas
        '
        Me.NAME_for_Cousntas.Caption = "اسم العميل"
        Me.NAME_for_Cousntas.FieldName = "NAME_for_Cousntas"
        Me.NAME_for_Cousntas.Name = "NAME_for_Cousntas"
        Me.NAME_for_Cousntas.Visible = True
        Me.NAME_for_Cousntas.VisibleIndex = 1
        Me.NAME_for_Cousntas.Width = 306
        '
        'CName
        '
        Me.CName.Caption = "الجنسية"
        Me.CName.FieldName = "CName"
        Me.CName.Name = "CName"
        Me.CName.Visible = True
        Me.CName.VisibleIndex = 2
        Me.CName.Width = 99
        '
        'National_Number
        '
        Me.National_Number.Caption = "رقم الوطني/اواثبات الشخصي"
        Me.National_Number.FieldName = "National_Number"
        Me.National_Number.Name = "National_Number"
        Me.National_Number.Visible = True
        Me.National_Number.VisibleIndex = 3
        Me.National_Number.Width = 145
        '
        'phone
        '
        Me.phone.Caption = "الهاتف"
        Me.phone.FieldName = "phone"
        Me.phone.Name = "phone"
        Me.phone.Visible = True
        Me.phone.VisibleIndex = 4
        Me.phone.Width = 197
        '
        'CityName
        '
        Me.CityName.Caption = "المدينة"
        Me.CityName.FieldName = "CityName"
        Me.CityName.Name = "CityName"
        Me.CityName.Visible = True
        Me.CityName.VisibleIndex = 5
        Me.CityName.Width = 73
        '
        'Type_Account
        '
        Me.Type_Account.Caption = "نوع الحساب"
        Me.Type_Account.FieldName = "Type_Account"
        Me.Type_Account.Name = "Type_Account"
        Me.Type_Account.Visible = True
        Me.Type_Account.VisibleIndex = 6
        Me.Type_Account.Width = 107
        '
        'inserDate
        '
        Me.inserDate.Caption = "تاريخ الانشاء"
        Me.inserDate.FieldName = "inserDate"
        Me.inserDate.Name = "inserDate"
        Me.inserDate.Visible = True
        Me.inserDate.VisibleIndex = 7
        Me.inserDate.Width = 97
        '
        'FormattedTime
        '
        Me.FormattedTime.Caption = "التوقيت"
        Me.FormattedTime.FieldName = "FormattedTime"
        Me.FormattedTime.Name = "FormattedTime"
        Me.FormattedTime.Visible = True
        Me.FormattedTime.VisibleIndex = 8
        Me.FormattedTime.Width = 73
        '
        'GridColumn1
        '
        Me.GridColumn1.Caption = "تاكيد"
        Me.GridColumn1.ColumnEdit = Me.OK
        Me.GridColumn1.Name = "GridColumn1"
        Me.GridColumn1.Visible = True
        Me.GridColumn1.VisibleIndex = 9
        Me.GridColumn1.Width = 53
        '
        'OK
        '
        Me.OK.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = CType(resources.GetObject("EditorButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.OK.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.OK.Name = "OK"
        Me.OK.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'GridColumn2
        '
        Me.GridColumn2.Caption = "الغاء"
        Me.GridColumn2.ColumnEdit = Me.Cansel
        Me.GridColumn2.Name = "GridColumn2"
        Me.GridColumn2.Visible = True
        Me.GridColumn2.VisibleIndex = 10
        Me.GridColumn2.Width = 66
        '
        'Cansel
        '
        Me.Cansel.AutoHeight = False
        EditorButtonImageOptions2.SvgImage = CType(resources.GetObject("EditorButtonImageOptions2.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions2.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.Cansel.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.Cansel.Name = "Cansel"
        Me.Cansel.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.TabbedControlGroup1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1363, 536)
        Me.Root.TextVisible = False
        '
        'TabbedControlGroup1
        '
        Me.TabbedControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.TabbedControlGroup1.Name = "TabbedControlGroup1"
        Me.TabbedControlGroup1.SelectedTabPage = Me.LayoutControlGroup1
        Me.TabbedControlGroup1.Size = New System.Drawing.Size(1337, 510)
        Me.TabbedControlGroup1.TabPages.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1})
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(1305, 447)
        Me.LayoutControlGroup1.Text = "العملاء"
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1305, 447)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRM_ADD_FROM_Costmer_Mobile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1363, 536)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FRM_ADD_FROM_Costmer_Mobile.IconOptions.LargeImage"), System.Drawing.Image)
        Me.Name = "FRM_ADD_FROM_Costmer_Mobile"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "اعتماد عميل من التطبيق"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Cansel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents TabbedControlGroup1 As DevExpress.XtraLayout.TabbedControlGroup
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents NAME_for_Cousntas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents National_Number As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents phone As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CityName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Type_Account As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents inserDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents FormattedTime As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OK As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents GridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Cansel As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
End Class
