<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMCurrencyMovements
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMCurrencyMovements))
        Me.ApplicationMenu1 = New DevExpress.XtraBars.Ribbon.ApplicationMenu(Me.components)
        Me.GalleryDropDown1 = New DevExpress.XtraBars.Ribbon.GalleryDropDown(Me.components)
        Me.RibbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem()
        Me.SkinDropDownButtonItem1 = New DevExpress.XtraBars.SkinDropDownButtonItem()
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem4 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem5 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem7 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem8 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem9 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnRefreish = New DevExpress.XtraBars.BarButtonItem()
        Me.RibbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup3 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup4 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.GalleryDropDown2 = New DevExpress.XtraBars.Ribbon.GalleryDropDown(Me.components)
        Me.RibbonPage3 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PanelDOck = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.CardView1 = New DevExpress.XtraGrid.Views.Card.CardView()
        Me.NAMECURNSE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CODE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BUprese = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.selecsPress = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BAnckNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Panel3 = New System.Windows.Forms.Panel()
        CType(Me.ApplicationMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GalleryDropDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GalleryDropDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CardView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ApplicationMenu1
        '
        Me.ApplicationMenu1.Name = "ApplicationMenu1"
        '
        'GalleryDropDown1
        '
        Me.GalleryDropDown1.Name = "GalleryDropDown1"
        '
        'RibbonControl1
        '
        Me.RibbonControl1.EmptyAreaImageOptions.ImagePadding = New System.Windows.Forms.Padding(30, 29, 30, 29)
        Me.RibbonControl1.ExpandCollapseItem.Id = 0
        Me.RibbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.RibbonControl1.ExpandCollapseItem, Me.BarButtonItem1, Me.BarButtonItem2, Me.SkinDropDownButtonItem1, Me.BarButtonItem3, Me.BarButtonItem4, Me.BarButtonItem5, Me.BarButtonItem6, Me.BarButtonItem7, Me.BarButtonItem8, Me.BarButtonItem9, Me.BtnRefreish})
        Me.RibbonControl1.Location = New System.Drawing.Point(0, 0)
        Me.RibbonControl1.MaxItemId = 12
        Me.RibbonControl1.Name = "RibbonControl1"
        Me.RibbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.RibbonPage1})
        Me.RibbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2007
        Me.RibbonControl1.Size = New System.Drawing.Size(1078, 249)
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.ActAsDropDown = True
        Me.BarButtonItem1.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check
        Me.BarButtonItem1.Caption = "اسعار الشراء علي المصرف"
        Me.BarButtonItem1.DropDownControl = Me.GalleryDropDown1
        Me.BarButtonItem1.Id = 1
        Me.BarButtonItem1.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.request
        Me.BarButtonItem1.ItemAppearance.Disabled.BackColor = System.Drawing.Color.Transparent
        Me.BarButtonItem1.ItemAppearance.Disabled.Options.UseBackColor = True
        Me.BarButtonItem1.ItemClickFireMode = DevExpress.XtraBars.BarItemEventFireMode.Immediate
        Me.BarButtonItem1.Name = "BarButtonItem1"
        Me.BarButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "BarButtonItem2"
        Me.BarButtonItem2.Id = 2
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'SkinDropDownButtonItem1
        '
        Me.SkinDropDownButtonItem1.Id = 3
        Me.SkinDropDownButtonItem1.Name = "SkinDropDownButtonItem1"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "BarButtonItem3"
        Me.BarButtonItem3.Id = 4
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'BarButtonItem4
        '
        Me.BarButtonItem4.Caption = "BarButtonItem4"
        Me.BarButtonItem4.Id = 5
        Me.BarButtonItem4.Name = "BarButtonItem4"
        '
        'BarButtonItem5
        '
        Me.BarButtonItem5.Caption = "أسعار حركات بيع العملة بالنقدي"
        Me.BarButtonItem5.Id = 6
        Me.BarButtonItem5.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.currency
        Me.BarButtonItem5.Name = "BarButtonItem5"
        '
        'BarButtonItem6
        '
        Me.BarButtonItem6.Caption = "BarButtonItem6"
        Me.BarButtonItem6.Id = 7
        Me.BarButtonItem6.Name = "BarButtonItem6"
        '
        'BarButtonItem7
        '
        Me.BarButtonItem7.Caption = "عرض جميع حركات اسعار العملة "
        Me.BarButtonItem7.Id = 8
        Me.BarButtonItem7.ImageOptions.Image = Global.ExchangeSystem.My.Resources.Resources.othercharts_32x32
        Me.BarButtonItem7.Name = "BarButtonItem7"
        Me.BarButtonItem7.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large
        '
        'BarButtonItem8
        '
        Me.BarButtonItem8.Caption = "طباعة"
        Me.BarButtonItem8.Id = 9
        Me.BarButtonItem8.ImageOptions.Image = Global.ExchangeSystem.My.Resources.Resources.print_32x32
        Me.BarButtonItem8.Name = "BarButtonItem8"
        Me.BarButtonItem8.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText
        '
        'BarButtonItem9
        '
        Me.BarButtonItem9.Caption = "ارسال عبر وتساب"
        Me.BarButtonItem9.Id = 10
        Me.BarButtonItem9.ImageOptions.Image = CType(resources.GetObject("BarButtonItem9.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem9.Name = "BarButtonItem9"
        '
        'BtnRefreish
        '
        Me.BtnRefreish.Caption = "تحديث"
        Me.BtnRefreish.Id = 11
        Me.BtnRefreish.ImageOptions.Image = Global.ExchangeSystem.My.Resources.Resources.icons8_refresh_100
        Me.BtnRefreish.Name = "BtnRefreish"
        Me.BtnRefreish.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText
        '
        'RibbonPage1
        '
        Me.RibbonPage1.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RibbonPage1.Appearance.Options.UseFont = True
        Me.RibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup1, Me.RibbonPageGroup2, Me.RibbonPageGroup3, Me.RibbonPageGroup4})
        Me.RibbonPage1.ImageOptions.SvgImage = CType(resources.GetObject("RibbonPage1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RibbonPage1.Name = "RibbonPage1"
        Me.RibbonPage1.Text = "عرض حركات اسعار العملات"
        '
        'RibbonPageGroup1
        '
        Me.RibbonPageGroup1.AllowTextClipping = False
        Me.RibbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.[False]
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarButtonItem1)
        Me.RibbonPageGroup1.Name = "RibbonPageGroup1"
        Me.RibbonPageGroup1.Text = "حركة الشراء علي المصرف"
        '
        'RibbonPageGroup2
        '
        Me.RibbonPageGroup2.AllowTextClipping = False
        Me.RibbonPageGroup2.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.[True]
        Me.RibbonPageGroup2.ItemLinks.Add(Me.BarButtonItem5)
        Me.RibbonPageGroup2.Name = "RibbonPageGroup2"
        Me.RibbonPageGroup2.Text = "حركة بيع العملةعلي المصرف"
        '
        'RibbonPageGroup3
        '
        Me.RibbonPageGroup3.AllowTextClipping = False
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarButtonItem7)
        Me.RibbonPageGroup3.Name = "RibbonPageGroup3"
        Me.RibbonPageGroup3.Text = "عرض حركات اسعاربيع العملة"
        '
        'RibbonPageGroup4
        '
        Me.RibbonPageGroup4.Alignment = DevExpress.XtraBars.Ribbon.RibbonPageGroupAlignment.Far
        Me.RibbonPageGroup4.ItemLinks.Add(Me.BarButtonItem8)
        Me.RibbonPageGroup4.ItemLinks.Add(Me.BarButtonItem9)
        Me.RibbonPageGroup4.ItemLinks.Add(Me.BtnRefreish)
        Me.RibbonPageGroup4.Name = "RibbonPageGroup4"
        '
        'GalleryDropDown2
        '
        Me.GalleryDropDown2.Name = "GalleryDropDown2"
        Me.GalleryDropDown2.Ribbon = Me.RibbonControl1
        '
        'RibbonPage3
        '
        Me.RibbonPage3.Name = "RibbonPage3"
        Me.RibbonPage3.Text = "RibbonPage3"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.PanelDOck)
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 249)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1078, 538)
        Me.Panel2.TabIndex = 2
        '
        'PanelDOck
        '
        Me.PanelDOck.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelDOck.Location = New System.Drawing.Point(0, 0)
        Me.PanelDOck.Name = "PanelDOck"
        Me.PanelDOck.Size = New System.Drawing.Size(801, 538)
        Me.PanelDOck.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GridControl1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(801, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(277, 538)
        Me.Panel1.TabIndex = 0
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 0)
        Me.GridControl1.MainView = Me.CardView1
        Me.GridControl1.MenuManager = Me.RibbonControl1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(277, 538)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.CardView1})
        '
        'CardView1
        '
        Me.CardView1.Appearance.FieldCaption.Options.UseTextOptions = True
        Me.CardView1.Appearance.FieldCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CardView1.Appearance.FieldCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CardView1.Appearance.FieldValue.Options.UseTextOptions = True
        Me.CardView1.Appearance.FieldValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CardView1.Appearance.FieldValue.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CardView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.CardView1.CardCaptionFormat = "اسعار العملات النقدي الاجنبي"
        Me.CardView1.CardWidth = 250
        Me.CardView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.NAMECURNSE, Me.CODE, Me.BUprese, Me.selecsPress, Me.BAnckNAME})
        Me.CardView1.DetailHeight = 334
        Me.CardView1.GridControl = Me.GridControl1
        Me.CardView1.Name = "CardView1"
        Me.CardView1.OptionsView.ShowCardExpandButton = False
        Me.CardView1.OptionsView.ShowEmptyFields = False
        Me.CardView1.OptionsView.ShowLines = False
        Me.CardView1.OptionsView.ShowQuickCustomizeButton = False
        Me.CardView1.OptionsView.ShowViewCaption = True
        Me.CardView1.PaintStyleName = "Office2003"
        Me.CardView1.SynchronizeClones = False
        Me.CardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.[Auto]
        Me.CardView1.ViewCaption = "اسعار بيع وشراء العملات النقدي الاجنبي"
        '
        'NAMECURNSE
        '
        Me.NAMECURNSE.AppearanceCell.Options.UseTextOptions = True
        Me.NAMECURNSE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.NAMECURNSE.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.NAMECURNSE.Caption = "أسم العملة"
        Me.NAMECURNSE.FieldName = "NAMECURNSE"
        Me.NAMECURNSE.Name = "NAMECURNSE"
        Me.NAMECURNSE.Visible = True
        Me.NAMECURNSE.VisibleIndex = 0
        '
        'CODE
        '
        Me.CODE.AppearanceCell.Options.UseTextOptions = True
        Me.CODE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CODE.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CODE.Caption = "رمز العملة"
        Me.CODE.FieldName = "CODE"
        Me.CODE.Name = "CODE"
        '
        'BUprese
        '
        Me.BUprese.AppearanceCell.Options.UseTextOptions = True
        Me.BUprese.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BUprese.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BUprese.Caption = "سعر الشراء"
        Me.BUprese.FieldName = "BUprese"
        Me.BUprese.ImageOptions.Alignment = System.Drawing.StringAlignment.Center
        Me.BUprese.ImageOptions.SvgImage = CType(resources.GetObject("BUprese.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BUprese.MaxWidth = 20
        Me.BUprese.Name = "BUprese"
        Me.BUprese.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.BUprese.OptionsColumn.ReadOnly = True
        Me.BUprese.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        Me.BUprese.Visible = True
        Me.BUprese.VisibleIndex = 1
        Me.BUprese.Width = 20
        '
        'selecsPress
        '
        Me.selecsPress.AppearanceCell.Options.UseTextOptions = True
        Me.selecsPress.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.selecsPress.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.selecsPress.Caption = "سعر البيع"
        Me.selecsPress.FieldName = "selecsPress"
        Me.selecsPress.Name = "selecsPress"
        Me.selecsPress.Visible = True
        Me.selecsPress.VisibleIndex = 2
        '
        'BAnckNAME
        '
        Me.BAnckNAME.AppearanceCell.Options.UseTextOptions = True
        Me.BAnckNAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BAnckNAME.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BAnckNAME.AppearanceHeader.Options.UseTextOptions = True
        Me.BAnckNAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BAnckNAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BAnckNAME.Caption = "أسم المصرف"
        Me.BAnckNAME.FieldName = "BAnckNAME"
        Me.BAnckNAME.Name = "BAnckNAME"
        Me.BAnckNAME.OptionsColumn.ReadOnly = True
        Me.BAnckNAME.Visible = True
        Me.BAnckNAME.VisibleIndex = 3
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 748)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1078, 39)
        Me.Panel3.TabIndex = 4
        '
        'FRMCurrencyMovements
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1078, 787)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.RibbonControl1)
        Me.Name = "FRMCurrencyMovements"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "تقرير عرض حركة أسعار العملات "
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.ApplicationMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GalleryDropDown1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GalleryDropDown2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CardView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ApplicationMenu1 As DevExpress.XtraBars.Ribbon.ApplicationMenu
    Friend WithEvents GalleryDropDown1 As DevExpress.XtraBars.Ribbon.GalleryDropDown
    Friend WithEvents RibbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents SkinDropDownButtonItem1 As DevExpress.XtraBars.SkinDropDownButtonItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem4 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem5 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem7 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonPageGroup3 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonPageGroup4 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents GalleryDropDown2 As DevExpress.XtraBars.Ribbon.GalleryDropDown
    Friend WithEvents RibbonPage3 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents Panel2 As Panel
    Friend WithEvents BarButtonItem8 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem9 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnRefreish As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents PanelDOck As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents CardView1 As DevExpress.XtraGrid.Views.Card.CardView
    Friend WithEvents NAMECURNSE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CODE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BUprese As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents selecsPress As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BAnckNAME As DevExpress.XtraGrid.Columns.GridColumn
End Class
