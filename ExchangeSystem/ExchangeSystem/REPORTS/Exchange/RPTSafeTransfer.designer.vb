<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class RPTSafeTransfer
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RPTSafeTransfer))
        Dim ShapeRectangle1 As DevExpress.XtraPrinting.Shape.ShapeRectangle = New DevExpress.XtraPrinting.Shape.ShapeRectangle()
        Dim ShapeRectangle2 As DevExpress.XtraPrinting.Shape.ShapeRectangle = New DevExpress.XtraPrinting.Shape.ShapeRectangle()
        Dim ShapeRectangle3 As DevExpress.XtraPrinting.Shape.ShapeRectangle = New DevExpress.XtraPrinting.Shape.ShapeRectangle()
        Dim SelectQuery1 As DevExpress.DataAccess.Sql.SelectQuery = New DevExpress.DataAccess.Sql.SelectQuery()
        Dim Column1 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression1 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Table1 As DevExpress.DataAccess.Sql.Table = New DevExpress.DataAccess.Sql.Table()
        Dim Column2 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression2 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column3 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression3 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column4 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression4 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column5 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression5 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column6 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression6 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column7 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression7 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column8 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression8 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column9 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression9 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column10 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression10 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column11 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression11 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column12 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression12 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim XrWatermark1 As DevExpress.XtraReports.UI.XRWatermark = New DevExpress.XtraReports.UI.XRWatermark()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.ReportHeader = New DevExpress.XtraReports.UI.ReportHeaderBand()
        Me.XrPictureBox7 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel24 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel28 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo8 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrLabel44 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo7 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrLabel43 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox24 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox23 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox25 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox21 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel13 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel9 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox20 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel61 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox26 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel27 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox43 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel35 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox11 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox12 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel11 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox5 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel8 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox4 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel16 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox3 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox1 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel17 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel18 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel19 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel20 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel22 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox6 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox8 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel51 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel50 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel23 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel25 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel26 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.CURRENCYID = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel14 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel12 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox2 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel21 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel5 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrShape2 = New DevExpress.XtraReports.UI.XRShape()
        Me.XrShape3 = New DevExpress.XtraReports.UI.XRShape()
        Me.XrShape1 = New DevExpress.XtraReports.UI.XRShape()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.SqlDataSource1 = New DevExpress.DataAccess.Sql.SqlDataSource(Me.components)
        Me.Title = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailCaption1 = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailData1 = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailData3_Odd = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.PageInfo = New DevExpress.XtraReports.UI.XRControlStyle()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'TopMargin
        '
        Me.TopMargin.Dpi = 254.0!
        Me.TopMargin.HeightF = 29.10833!
        Me.TopMargin.Name = "TopMargin"
        '
        'BottomMargin
        '
        Me.BottomMargin.Dpi = 254.0!
        Me.BottomMargin.HeightF = 23.84254!
        Me.BottomMargin.Name = "BottomMargin"
        '
        'ReportHeader
        '
        Me.ReportHeader.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrPictureBox7, Me.XrLabel24, Me.XrLabel28, Me.XrPageInfo8, Me.XrLabel44, Me.XrPageInfo7, Me.XrLabel43, Me.XrPictureBox24, Me.XrPictureBox23, Me.XrLabel1, Me.XrPictureBox25, Me.XrPictureBox21, Me.XrLabel13, Me.XrLabel9, Me.XrPictureBox20, Me.XrLabel61, Me.XrPictureBox26, Me.XrLabel27, Me.XrPictureBox43, Me.XrLabel35, Me.XrPictureBox11, Me.XrPictureBox12, Me.XrLabel2, Me.XrLabel3, Me.XrLabel4, Me.XrLabel11, Me.XrPictureBox5, Me.XrLabel8, Me.XrLabel10, Me.XrPictureBox4, Me.XrLabel16, Me.XrPictureBox3, Me.XrPictureBox1, Me.XrLabel17, Me.XrLabel18, Me.XrLabel20, Me.XrLabel22, Me.XrPictureBox6, Me.XrPictureBox8, Me.XrLabel51, Me.XrLabel50, Me.XrLabel23, Me.XrLabel25, Me.XrLabel26, Me.XrLabel15, Me.CURRENCYID, Me.XrLabel14, Me.XrLabel12, Me.XrPictureBox2, Me.XrLabel21, Me.XrLabel5, Me.XrLabel7, Me.XrShape2, Me.XrShape3, Me.XrLabel19, Me.XrShape1})
        Me.ReportHeader.Dpi = 254.0!
        Me.ReportHeader.HeightF = 1396.704!
        Me.ReportHeader.Name = "ReportHeader"
        '
        'XrPictureBox7
        '
        Me.XrPictureBox7.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox7.Dpi = 254.0!
        Me.XrPictureBox7.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox7.ImageSource"))
        Me.XrPictureBox7.LocationFloat = New DevExpress.Utils.PointFloat(1169.406!, 329.8446!)
        Me.XrPictureBox7.Name = "XrPictureBox7"
        Me.XrPictureBox7.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox7.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox7.StylePriority.UseBorderColor = False
        '
        'XrLabel24
        '
        Me.XrLabel24.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel24.Dpi = 254.0!
        Me.XrLabel24.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel24.LocationFloat = New DevExpress.Utils.PointFloat(1039.673!, 312.5528!)
        Me.XrLabel24.Multiline = True
        Me.XrLabel24.Name = "XrLabel24"
        Me.XrLabel24.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel24.SizeF = New System.Drawing.SizeF(129.7336!, 90.16849!)
        Me.XrLabel24.StylePriority.UseBorderColor = False
        Me.XrLabel24.StylePriority.UseBorders = False
        Me.XrLabel24.StylePriority.UseFont = False
        Me.XrLabel24.StylePriority.UseTextAlignment = False
        Me.XrLabel24.Text = ":العملة"
        Me.XrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel28
        '
        Me.XrLabel28.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel28.Dpi = 254.0!
        Me.XrLabel28.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CurrencyName]")})
        Me.XrLabel28.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel28.LocationFloat = New DevExpress.Utils.PointFloat(578.0544!, 312.5528!)
        Me.XrLabel28.Multiline = True
        Me.XrLabel28.Name = "XrLabel28"
        Me.XrLabel28.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel28.SizeF = New System.Drawing.SizeF(461.6186!, 90.1684!)
        Me.XrLabel28.StylePriority.UseBorderColor = False
        Me.XrLabel28.StylePriority.UseBorders = False
        Me.XrLabel28.StylePriority.UseFont = False
        Me.XrLabel28.StylePriority.UseTextAlignment = False
        Me.XrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPageInfo8
        '
        Me.XrPageInfo8.Dpi = 254.0!
        Me.XrPageInfo8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo8.LocationFloat = New DevExpress.Utils.PointFloat(14.22961!, 1295.17!)
        Me.XrPageInfo8.Name = "XrPageInfo8"
        Me.XrPageInfo8.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo8.SizeF = New System.Drawing.SizeF(267.0162!, 66.99609!)
        Me.XrPageInfo8.StylePriority.UseFont = False
        Me.XrPageInfo8.StylePriority.UseTextAlignment = False
        Me.XrPageInfo8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrPageInfo8.TextFormatString = "{0:hh:mm:ss}"
        '
        'XrLabel44
        '
        Me.XrLabel44.Dpi = 254.0!
        Me.XrLabel44.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel44.LocationFloat = New DevExpress.Utils.PointFloat(281.2457!, 1295.17!)
        Me.XrLabel44.Name = "XrLabel44"
        Me.XrLabel44.SizeF = New System.Drawing.SizeF(187.4837!, 66.99609!)
        Me.XrLabel44.StylePriority.UseFont = False
        Me.XrLabel44.StylePriority.UseTextAlignment = False
        Me.XrLabel44.Text = " وقت الطباعة"
        Me.XrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel44.TextFormatString = "{0:hh:mm tt}"
        '
        'XrPageInfo7
        '
        Me.XrPageInfo7.Dpi = 254.0!
        Me.XrPageInfo7.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo7.LocationFloat = New DevExpress.Utils.PointFloat(591.3372!, 1295.17!)
        Me.XrPageInfo7.Name = "XrPageInfo7"
        Me.XrPageInfo7.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo7.SizeF = New System.Drawing.SizeF(262.0302!, 66.99609!)
        Me.XrPageInfo7.StylePriority.UseFont = False
        Me.XrPageInfo7.StylePriority.UseTextAlignment = False
        Me.XrPageInfo7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrPageInfo7.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel43
        '
        Me.XrLabel43.Dpi = 254.0!
        Me.XrLabel43.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel43.LocationFloat = New DevExpress.Utils.PointFloat(853.3673!, 1295.17!)
        Me.XrLabel43.Name = "XrLabel43"
        Me.XrLabel43.SizeF = New System.Drawing.SizeF(189.352!, 66.99609!)
        Me.XrLabel43.StylePriority.UseFont = False
        Me.XrLabel43.StylePriority.UseTextAlignment = False
        Me.XrLabel43.Text = "تاريخ الطباعة"
        Me.XrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel43.TextFormatString = "{0:hh:mm tt}"
        '
        'XrPictureBox24
        '
        Me.XrPictureBox24.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox24.Dpi = 254.0!
        Me.XrPictureBox24.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox24.ImageSource"))
        Me.XrPictureBox24.LocationFloat = New DevExpress.Utils.PointFloat(1042.719!, 1300.25!)
        Me.XrPictureBox24.Name = "XrPictureBox24"
        Me.XrPictureBox24.SizeF = New System.Drawing.SizeF(54.71436!, 56.73682!)
        Me.XrPictureBox24.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox24.StylePriority.UseBorderColor = False
        '
        'XrPictureBox23
        '
        Me.XrPictureBox23.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox23.Dpi = 254.0!
        Me.XrPictureBox23.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox23.ImageSource"))
        Me.XrPictureBox23.LocationFloat = New DevExpress.Utils.PointFloat(468.7296!, 1300.25!)
        Me.XrPictureBox23.Name = "XrPictureBox23"
        Me.XrPictureBox23.SizeF = New System.Drawing.SizeF(54.71436!, 56.73669!)
        Me.XrPictureBox23.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox23.StylePriority.UseBorderColor = False
        '
        'XrLabel1
        '
        Me.XrLabel1.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel1.Dpi = 254.0!
        Me.XrLabel1.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(93.88316!, 1163.701!)
        Me.XrLabel1.Multiline = True
        Me.XrLabel1.Name = "XrLabel1"
        Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel1.SizeF = New System.Drawing.SizeF(374.829!, 61.97656!)
        Me.XrLabel1.StylePriority.UseBorderColor = False
        Me.XrLabel1.StylePriority.UseBorders = False
        Me.XrLabel1.StylePriority.UseFont = False
        Me.XrLabel1.StylePriority.UseTextAlignment = False
        Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox25
        '
        Me.XrPictureBox25.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox25.Dpi = 254.0!
        Me.XrPictureBox25.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox25.ImageSource"))
        Me.XrPictureBox25.LocationFloat = New DevExpress.Utils.PointFloat(468.7122!, 1169.204!)
        Me.XrPictureBox25.Name = "XrPictureBox25"
        Me.XrPictureBox25.SizeF = New System.Drawing.SizeF(52.06854!, 50.48108!)
        Me.XrPictureBox25.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox25.StylePriority.UseBorderColor = False
        '
        'XrPictureBox21
        '
        Me.XrPictureBox21.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox21.Dpi = 254.0!
        Me.XrPictureBox21.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox21.ImageSource"))
        Me.XrPictureBox21.LocationFloat = New DevExpress.Utils.PointFloat(1039.673!, 1169.204!)
        Me.XrPictureBox21.Name = "XrPictureBox21"
        Me.XrPictureBox21.SizeF = New System.Drawing.SizeF(54.71436!, 50.48108!)
        Me.XrPictureBox21.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox21.StylePriority.UseBorderColor = False
        '
        'XrLabel13
        '
        Me.XrLabel13.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel13.Dpi = 254.0!
        Me.XrLabel13.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel13.LocationFloat = New DevExpress.Utils.PointFloat(617.1661!, 1163.701!)
        Me.XrLabel13.Multiline = True
        Me.XrLabel13.Name = "XrLabel13"
        Me.XrLabel13.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel13.SizeF = New System.Drawing.SizeF(422.5064!, 61.97656!)
        Me.XrLabel13.StylePriority.UseBorderColor = False
        Me.XrLabel13.StylePriority.UseBorders = False
        Me.XrLabel13.StylePriority.UseFont = False
        Me.XrLabel13.StylePriority.UseTextAlignment = False
        Me.XrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrLabel9
        '
        Me.XrLabel9.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel9.Dpi = 254.0!
        Me.XrLabel9.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel9.LocationFloat = New DevExpress.Utils.PointFloat(1705.239!, 1163.701!)
        Me.XrLabel9.Multiline = True
        Me.XrLabel9.Name = "XrLabel9"
        Me.XrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel9.SizeF = New System.Drawing.SizeF(215.9495!, 61.97656!)
        Me.XrLabel9.StylePriority.UseBorderColor = False
        Me.XrLabel9.StylePriority.UseBorders = False
        Me.XrLabel9.StylePriority.UseFont = False
        Me.XrLabel9.StylePriority.UseTextAlignment = False
        Me.XrLabel9.Text = "للإستفســـار"
        Me.XrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox20
        '
        Me.XrPictureBox20.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox20.Dpi = 254.0!
        Me.XrPictureBox20.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox20.ImageSource"))
        Me.XrPictureBox20.LocationFloat = New DevExpress.Utils.PointFloat(1921.189!, 1169.204!)
        Me.XrPictureBox20.Name = "XrPictureBox20"
        Me.XrPictureBox20.SizeF = New System.Drawing.SizeF(52.0686!, 50.48108!)
        Me.XrPictureBox20.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox20.StylePriority.UseBorderColor = False
        '
        'XrLabel61
        '
        Me.XrLabel61.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel61.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel61.Dpi = 254.0!
        Me.XrLabel61.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Phone]")})
        Me.XrLabel61.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel61.LocationFloat = New DevExpress.Utils.PointFloat(1169.406!, 1163.701!)
        Me.XrLabel61.Multiline = True
        Me.XrLabel61.Name = "XrLabel61"
        Me.XrLabel61.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel61.SizeF = New System.Drawing.SizeF(393.6929!, 61.97656!)
        Me.XrLabel61.StylePriority.UseBorderColor = False
        Me.XrLabel61.StylePriority.UseBorders = False
        Me.XrLabel61.StylePriority.UseFont = False
        Me.XrLabel61.StylePriority.UseTextAlignment = False
        Me.XrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox26
        '
        Me.XrPictureBox26.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox26.Dpi = 254.0!
        Me.XrPictureBox26.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox26.ImageSource"))
        Me.XrPictureBox26.LocationFloat = New DevExpress.Utils.PointFloat(1563.099!, 1169.204!)
        Me.XrPictureBox26.Name = "XrPictureBox26"
        Me.XrPictureBox26.SizeF = New System.Drawing.SizeF(53.34949!, 50.48108!)
        Me.XrPictureBox26.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox26.StylePriority.UseBorderColor = False
        '
        'XrLabel27
        '
        Me.XrLabel27.CanGrow = False
        Me.XrLabel27.Dpi = 254.0!
        Me.XrLabel27.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel27.LocationFloat = New DevExpress.Utils.PointFloat(1097.433!, 1295.17!)
        Me.XrLabel27.Multiline = True
        Me.XrLabel27.Name = "XrLabel27"
        Me.XrLabel27.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel27.SizeF = New System.Drawing.SizeF(605.1608!, 66.99634!)
        Me.XrLabel27.StylePriority.UseFont = False
        Me.XrLabel27.StylePriority.UseTextAlignment = False
        Me.XrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        '
        'XrPictureBox43
        '
        Me.XrPictureBox43.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox43.Dpi = 254.0!
        Me.XrPictureBox43.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox43.ImageSource"))
        Me.XrPictureBox43.LocationFloat = New DevExpress.Utils.PointFloat(1916.361!, 1295.17!)
        Me.XrPictureBox43.Name = "XrPictureBox43"
        Me.XrPictureBox43.SizeF = New System.Drawing.SizeF(56.89636!, 62.02869!)
        Me.XrPictureBox43.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox43.StylePriority.UseBorderColor = False
        '
        'XrLabel35
        '
        Me.XrLabel35.Dpi = 254.0!
        Me.XrLabel35.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel35.LocationFloat = New DevExpress.Utils.PointFloat(1705.239!, 1295.17!)
        Me.XrLabel35.Name = "XrLabel35"
        Me.XrLabel35.SizeF = New System.Drawing.SizeF(211.1215!, 66.99609!)
        Me.XrLabel35.StylePriority.UseFont = False
        Me.XrLabel35.StylePriority.UseTextAlignment = False
        Me.XrLabel35.Text = ":اسم المستخدم"
        Me.XrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox11
        '
        Me.XrPictureBox11.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox11.Dpi = 254.0!
        Me.XrPictureBox11.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox11.ImageSource"))
        Me.XrPictureBox11.LocationFloat = New DevExpress.Utils.PointFloat(1926.813!, 329.8446!)
        Me.XrPictureBox11.Name = "XrPictureBox11"
        Me.XrPictureBox11.SizeF = New System.Drawing.SizeF(60.00684!, 61.06427!)
        Me.XrPictureBox11.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox11.StylePriority.UseBorderColor = False
        '
        'XrPictureBox12
        '
        Me.XrPictureBox12.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox12.Dpi = 254.0!
        Me.XrPictureBox12.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox12.ImageSource"))
        Me.XrPictureBox12.LocationFloat = New DevExpress.Utils.PointFloat(516.123!, 329.8446!)
        Me.XrPictureBox12.Name = "XrPictureBox12"
        Me.XrPictureBox12.SizeF = New System.Drawing.SizeF(60.00604!, 61.06427!)
        Me.XrPictureBox12.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox12.StylePriority.UseBorderColor = False
        '
        'XrLabel2
        '
        Me.XrLabel2.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel2.Dpi = 254.0!
        Me.XrLabel2.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(1656.939!, 939.996!)
        Me.XrLabel2.Multiline = True
        Me.XrLabel2.Name = "XrLabel2"
        Me.XrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel2.SizeF = New System.Drawing.SizeF(269.8751!, 90.16843!)
        Me.XrLabel2.StylePriority.UseBorderColor = False
        Me.XrLabel2.StylePriority.UseBorders = False
        Me.XrLabel2.StylePriority.UseFont = False
        Me.XrLabel2.StylePriority.UseTextAlignment = False
        Me.XrLabel2.Text = ":بيان الملاحظات"
        Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel3
        '
        Me.XrLabel3.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel3.Dpi = 254.0!
        Me.XrLabel3.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(1656.938!, 844.7477!)
        Me.XrLabel3.Multiline = True
        Me.XrLabel3.Name = "XrLabel3"
        Me.XrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel3.SizeF = New System.Drawing.SizeF(269.8752!, 90.16843!)
        Me.XrLabel3.StylePriority.UseBorderColor = False
        Me.XrLabel3.StylePriority.UseBorders = False
        Me.XrLabel3.StylePriority.UseFont = False
        Me.XrLabel3.StylePriority.UseTextAlignment = False
        Me.XrLabel3.Text = ":تنفيذ الموظف"
        Me.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel4
        '
        Me.XrLabel4.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel4.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel4.BorderWidth = 1.5!
        Me.XrLabel4.Dpi = 254.0!
        Me.XrLabel4.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(48.29114!, 934.9159!)
        Me.XrLabel4.Multiline = True
        Me.XrLabel4.Name = "XrLabel4"
        Me.XrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel4.SizeF = New System.Drawing.SizeF(1946.523!, 5.080048!)
        Me.XrLabel4.StylePriority.UseBorderColor = False
        Me.XrLabel4.StylePriority.UseBorders = False
        Me.XrLabel4.StylePriority.UseBorderWidth = False
        Me.XrLabel4.StylePriority.UseFont = False
        Me.XrLabel4.StylePriority.UseTextAlignment = False
        Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel11
        '
        Me.XrLabel11.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel11.Dpi = 254.0!
        Me.XrLabel11.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Uname]")})
        Me.XrLabel11.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel11.LocationFloat = New DevExpress.Utils.PointFloat(48.29114!, 844.7477!)
        Me.XrLabel11.Multiline = True
        Me.XrLabel11.Name = "XrLabel11"
        Me.XrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel11.SizeF = New System.Drawing.SizeF(1606.002!, 90.16852!)
        Me.XrLabel11.StylePriority.UseBorderColor = False
        Me.XrLabel11.StylePriority.UseBorders = False
        Me.XrLabel11.StylePriority.UseFont = False
        Me.XrLabel11.StylePriority.UseTextAlignment = False
        Me.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox5
        '
        Me.XrPictureBox5.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox5.Dpi = 254.0!
        Me.XrPictureBox5.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox5.ImageSource"))
        Me.XrPictureBox5.LocationFloat = New DevExpress.Utils.PointFloat(1926.814!, 857.7675!)
        Me.XrPictureBox5.Name = "XrPictureBox5"
        Me.XrPictureBox5.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox5.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox5.StylePriority.UseBorderColor = False
        '
        'XrLabel8
        '
        Me.XrLabel8.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel8.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel8.BorderWidth = 1.5!
        Me.XrLabel8.Dpi = 254.0!
        Me.XrLabel8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel8.LocationFloat = New DevExpress.Utils.PointFloat(48.29114!, 1030.164!)
        Me.XrLabel8.Multiline = True
        Me.XrLabel8.Name = "XrLabel8"
        Me.XrLabel8.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel8.SizeF = New System.Drawing.SizeF(1946.523!, 5.080017!)
        Me.XrLabel8.StylePriority.UseBorderColor = False
        Me.XrLabel8.StylePriority.UseBorders = False
        Me.XrLabel8.StylePriority.UseBorderWidth = False
        Me.XrLabel8.StylePriority.UseFont = False
        Me.XrLabel8.StylePriority.UseTextAlignment = False
        Me.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel10
        '
        Me.XrLabel10.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel10.Dpi = 254.0!
        Me.XrLabel10.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Notes]")})
        Me.XrLabel10.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(48.29114!, 939.996!)
        Me.XrLabel10.Multiline = True
        Me.XrLabel10.Name = "XrLabel10"
        Me.XrLabel10.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel10.SizeF = New System.Drawing.SizeF(1606.002!, 90.16843!)
        Me.XrLabel10.StylePriority.UseBorderColor = False
        Me.XrLabel10.StylePriority.UseBorders = False
        Me.XrLabel10.StylePriority.UseFont = False
        Me.XrLabel10.StylePriority.UseTextAlignment = False
        Me.XrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox4
        '
        Me.XrPictureBox4.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox4.Dpi = 254.0!
        Me.XrPictureBox4.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox4.ImageSource"))
        Me.XrPictureBox4.LocationFloat = New DevExpress.Utils.PointFloat(1926.814!, 953.0159!)
        Me.XrPictureBox4.Name = "XrPictureBox4"
        Me.XrPictureBox4.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox4.StylePriority.UseBorderColor = False
        '
        'XrLabel16
        '
        Me.XrLabel16.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel16.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel16.BorderWidth = 1.5!
        Me.XrLabel16.Dpi = 254.0!
        Me.XrLabel16.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel16.LocationFloat = New DevExpress.Utils.PointFloat(49.33073!, 613.8901!)
        Me.XrLabel16.Multiline = True
        Me.XrLabel16.Name = "XrLabel16"
        Me.XrLabel16.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel16.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel16.StylePriority.UseBorderColor = False
        Me.XrLabel16.StylePriority.UseBorders = False
        Me.XrLabel16.StylePriority.UseBorderWidth = False
        Me.XrLabel16.StylePriority.UseFont = False
        Me.XrLabel16.StylePriority.UseTextAlignment = False
        Me.XrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrPictureBox3
        '
        Me.XrPictureBox3.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox3.Dpi = 254.0!
        Me.XrPictureBox3.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox3.ImageSource"))
        Me.XrPictureBox3.LocationFloat = New DevExpress.Utils.PointFloat(1000.812!, 631.3051!)
        Me.XrPictureBox3.Name = "XrPictureBox3"
        Me.XrPictureBox3.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox3.StylePriority.UseBorderColor = False
        '
        'XrPictureBox1
        '
        Me.XrPictureBox1.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox1.Dpi = 254.0!
        Me.XrPictureBox1.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox1.ImageSource"))
        Me.XrPictureBox1.LocationFloat = New DevExpress.Utils.PointFloat(1926.814!, 631.3051!)
        Me.XrPictureBox1.Name = "XrPictureBox1"
        Me.XrPictureBox1.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox1.StylePriority.UseBorderColor = False
        '
        'XrLabel17
        '
        Me.XrLabel17.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel17.Dpi = 254.0!
        Me.XrLabel17.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel17.LocationFloat = New DevExpress.Utils.PointFloat(855.4492!, 618.9701!)
        Me.XrLabel17.Multiline = True
        Me.XrLabel17.Name = "XrLabel17"
        Me.XrLabel17.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel17.SizeF = New System.Drawing.SizeF(145.3628!, 90.16803!)
        Me.XrLabel17.StylePriority.UseBorderColor = False
        Me.XrLabel17.StylePriority.UseBorders = False
        Me.XrLabel17.StylePriority.UseFont = False
        Me.XrLabel17.StylePriority.UseTextAlignment = False
        Me.XrLabel17.Text = ":بالحروف"
        Me.XrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel18
        '
        Me.XrLabel18.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel18.CanGrow = False
        Me.XrLabel18.Dpi = 254.0!
        Me.XrLabel18.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel18.LocationFloat = New DevExpress.Utils.PointFloat(25.00001!, 618.9703!)
        Me.XrLabel18.Multiline = True
        Me.XrLabel18.Name = "XrLabel18"
        Me.XrLabel18.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel18.SizeF = New System.Drawing.SizeF(827.8033!, 90.1684!)
        Me.XrLabel18.StylePriority.UseBorderColor = False
        Me.XrLabel18.StylePriority.UseBorders = False
        Me.XrLabel18.StylePriority.UseFont = False
        Me.XrLabel18.StylePriority.UseTextAlignment = False
        Me.XrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrLabel19
        '
        Me.XrLabel19.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel19.Dpi = 254.0!
        Me.XrLabel19.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel19.LocationFloat = New DevExpress.Utils.PointFloat(1060.818!, 618.9703!)
        Me.XrLabel19.Multiline = True
        Me.XrLabel19.Name = "XrLabel19"
        Me.XrLabel19.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel19.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel19.SizeF = New System.Drawing.SizeF(741.7997!, 90.1684!)
        Me.XrLabel19.StylePriority.UseBorderColor = False
        Me.XrLabel19.StylePriority.UseBorders = False
        Me.XrLabel19.StylePriority.UseFont = False
        Me.XrLabel19.StylePriority.UseTextAlignment = False
        Me.XrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        Me.XrLabel19.TextFormatString = "{0:N3}"
        '
        'XrLabel20
        '
        Me.XrLabel20.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel20.Dpi = 254.0!
        Me.XrLabel20.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel20.LocationFloat = New DevExpress.Utils.PointFloat(1802.618!, 618.9701!)
        Me.XrLabel20.Multiline = True
        Me.XrLabel20.Name = "XrLabel20"
        Me.XrLabel20.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel20.SizeF = New System.Drawing.SizeF(124.1959!, 90.16846!)
        Me.XrLabel20.StylePriority.UseBorderColor = False
        Me.XrLabel20.StylePriority.UseBorders = False
        Me.XrLabel20.StylePriority.UseFont = False
        Me.XrLabel20.StylePriority.UseTextAlignment = False
        Me.XrLabel20.Text = ":القيمة"
        Me.XrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel22
        '
        Me.XrLabel22.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel22.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel22.BorderWidth = 1.5!
        Me.XrLabel22.Dpi = 254.0!
        Me.XrLabel22.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel22.LocationFloat = New DevExpress.Utils.PointFloat(49.33057!, 709.1389!)
        Me.XrLabel22.Multiline = True
        Me.XrLabel22.Name = "XrLabel22"
        Me.XrLabel22.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel22.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel22.StylePriority.UseBorderColor = False
        Me.XrLabel22.StylePriority.UseBorders = False
        Me.XrLabel22.StylePriority.UseBorderWidth = False
        Me.XrLabel22.StylePriority.UseFont = False
        Me.XrLabel22.StylePriority.UseTextAlignment = False
        Me.XrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrPictureBox6
        '
        Me.XrPictureBox6.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox6.Dpi = 254.0!
        Me.XrPictureBox6.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox6.ImageSource"))
        Me.XrPictureBox6.LocationFloat = New DevExpress.Utils.PointFloat(1000.812!, 505.7217!)
        Me.XrPictureBox6.Name = "XrPictureBox6"
        Me.XrPictureBox6.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox6.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox6.StylePriority.UseBorderColor = False
        '
        'XrPictureBox8
        '
        Me.XrPictureBox8.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox8.Dpi = 254.0!
        Me.XrPictureBox8.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox8.ImageSource"))
        Me.XrPictureBox8.LocationFloat = New DevExpress.Utils.PointFloat(1926.814!, 505.7217!)
        Me.XrPictureBox8.Name = "XrPictureBox8"
        Me.XrPictureBox8.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox8.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox8.StylePriority.UseBorderColor = False
        '
        'XrLabel51
        '
        Me.XrLabel51.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel51.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel51.Dpi = 254.0!
        Me.XrLabel51.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel51.LocationFloat = New DevExpress.Utils.PointFloat(855.4492!, 492.7213!)
        Me.XrLabel51.Multiline = True
        Me.XrLabel51.Name = "XrLabel51"
        Me.XrLabel51.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel51.SizeF = New System.Drawing.SizeF(145.3629!, 90.16846!)
        Me.XrLabel51.StylePriority.UseBorderColor = False
        Me.XrLabel51.StylePriority.UseBorders = False
        Me.XrLabel51.StylePriority.UseFont = False
        Me.XrLabel51.StylePriority.UseTextAlignment = False
        Me.XrLabel51.Text = ":إلــــــــــــى"
        Me.XrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel50
        '
        Me.XrLabel50.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel50.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel50.CanGrow = False
        Me.XrLabel50.Dpi = 254.0!
        Me.XrLabel50.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Seconduname]")})
        Me.XrLabel50.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel50.LocationFloat = New DevExpress.Utils.PointFloat(49.33073!, 492.7217!)
        Me.XrLabel50.Multiline = True
        Me.XrLabel50.Name = "XrLabel50"
        Me.XrLabel50.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel50.SizeF = New System.Drawing.SizeF(803.4727!, 90.16846!)
        Me.XrLabel50.StylePriority.UseBorderColor = False
        Me.XrLabel50.StylePriority.UseBorders = False
        Me.XrLabel50.StylePriority.UseFont = False
        Me.XrLabel50.StylePriority.UseTextAlignment = False
        Me.XrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrLabel23
        '
        Me.XrLabel23.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel23.CanGrow = False
        Me.XrLabel23.Dpi = 254.0!
        Me.XrLabel23.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[firstuname]")})
        Me.XrLabel23.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel23.LocationFloat = New DevExpress.Utils.PointFloat(1060.818!, 492.7215!)
        Me.XrLabel23.Multiline = True
        Me.XrLabel23.Name = "XrLabel23"
        Me.XrLabel23.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel23.SizeF = New System.Drawing.SizeF(741.7993!, 90.1684!)
        Me.XrLabel23.StylePriority.UseBorderColor = False
        Me.XrLabel23.StylePriority.UseBorders = False
        Me.XrLabel23.StylePriority.UseFont = False
        Me.XrLabel23.StylePriority.UseTextAlignment = False
        Me.XrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrLabel25
        '
        Me.XrLabel25.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel25.Dpi = 254.0!
        Me.XrLabel25.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel25.LocationFloat = New DevExpress.Utils.PointFloat(1802.617!, 492.7213!)
        Me.XrLabel25.Multiline = True
        Me.XrLabel25.Name = "XrLabel25"
        Me.XrLabel25.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel25.SizeF = New System.Drawing.SizeF(124.1962!, 90.16843!)
        Me.XrLabel25.StylePriority.UseBorderColor = False
        Me.XrLabel25.StylePriority.UseBorders = False
        Me.XrLabel25.StylePriority.UseFont = False
        Me.XrLabel25.StylePriority.UseTextAlignment = False
        Me.XrLabel25.Text = ":مـــــــن"
        Me.XrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel26
        '
        Me.XrLabel26.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel26.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel26.BorderWidth = 1.5!
        Me.XrLabel26.Dpi = 254.0!
        Me.XrLabel26.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel26.LocationFloat = New DevExpress.Utils.PointFloat(49.33073!, 582.89!)
        Me.XrLabel26.Multiline = True
        Me.XrLabel26.Name = "XrLabel26"
        Me.XrLabel26.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel26.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel26.StylePriority.UseBorderColor = False
        Me.XrLabel26.StylePriority.UseBorders = False
        Me.XrLabel26.StylePriority.UseBorderWidth = False
        Me.XrLabel26.StylePriority.UseFont = False
        Me.XrLabel26.StylePriority.UseTextAlignment = False
        Me.XrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel15
        '
        Me.XrLabel15.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel15.Dpi = 254.0!
        Me.XrLabel15.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(399.4048!, 312.5529!)
        Me.XrLabel15.Multiline = True
        Me.XrLabel15.Name = "XrLabel15"
        Me.XrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel15.SizeF = New System.Drawing.SizeF(116.7183!, 90.16843!)
        Me.XrLabel15.StylePriority.UseBorderColor = False
        Me.XrLabel15.StylePriority.UseBorders = False
        Me.XrLabel15.StylePriority.UseFont = False
        Me.XrLabel15.StylePriority.UseTextAlignment = False
        Me.XrLabel15.Text = ":التاريخ"
        Me.XrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'CURRENCYID
        '
        Me.CURRENCYID.BorderColor = System.Drawing.Color.LightGray
        Me.CURRENCYID.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.CURRENCYID.Dpi = 254.0!
        Me.CURRENCYID.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WithdrawalDate]")})
        Me.CURRENCYID.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.CURRENCYID.LocationFloat = New DevExpress.Utils.PointFloat(25.00001!, 312.5533!)
        Me.CURRENCYID.Multiline = True
        Me.CURRENCYID.Name = "CURRENCYID"
        Me.CURRENCYID.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.CURRENCYID.SizeF = New System.Drawing.SizeF(370.4008!, 90.16843!)
        Me.CURRENCYID.StylePriority.UseBorderColor = False
        Me.CURRENCYID.StylePriority.UseBorders = False
        Me.CURRENCYID.StylePriority.UseFont = False
        Me.CURRENCYID.StylePriority.UseTextAlignment = False
        Me.CURRENCYID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        Me.CURRENCYID.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel14
        '
        Me.XrLabel14.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel14.Dpi = 254.0!
        Me.XrLabel14.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WDCode]")})
        Me.XrLabel14.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel14.LocationFloat = New DevExpress.Utils.PointFloat(1230.151!, 312.5529!)
        Me.XrLabel14.Multiline = True
        Me.XrLabel14.Name = "XrLabel14"
        Me.XrLabel14.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel14.SizeF = New System.Drawing.SizeF(458.5718!, 90.1684!)
        Me.XrLabel14.StylePriority.UseBorderColor = False
        Me.XrLabel14.StylePriority.UseBorders = False
        Me.XrLabel14.StylePriority.UseFont = False
        Me.XrLabel14.StylePriority.UseTextAlignment = False
        Me.XrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrLabel12
        '
        Me.XrLabel12.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel12.Dpi = 254.0!
        Me.XrLabel12.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel12.LocationFloat = New DevExpress.Utils.PointFloat(1688.724!, 312.5529!)
        Me.XrLabel12.Multiline = True
        Me.XrLabel12.Name = "XrLabel12"
        Me.XrLabel12.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel12.SizeF = New System.Drawing.SizeF(238.0896!, 90.16843!)
        Me.XrLabel12.StylePriority.UseBorderColor = False
        Me.XrLabel12.StylePriority.UseBorders = False
        Me.XrLabel12.StylePriority.UseFont = False
        Me.XrLabel12.StylePriority.UseTextAlignment = False
        Me.XrLabel12.Text = ":رقم المعاملة"
        Me.XrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrPictureBox2
        '
        Me.XrPictureBox2.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox2.Dpi = 254.0!
        Me.XrPictureBox2.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource(Global.ExchangeSystem.My.Resources.Resources.cancel, True)
        Me.XrPictureBox2.LocationFloat = New DevExpress.Utils.PointFloat(893.5372!, 3.0!)
        Me.XrPictureBox2.Name = "XrPictureBox2"
        Me.XrPictureBox2.SizeF = New System.Drawing.SizeF(247.4659!, 224.8959!)
        Me.XrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox2.StylePriority.UseBorderColor = False
        '
        'XrLabel21
        '
        Me.XrLabel21.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel21.Dpi = 254.0!
        Me.XrLabel21.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel21.LocationFloat = New DevExpress.Utils.PointFloat(1350.512!, 0!)
        Me.XrLabel21.Multiline = True
        Me.XrLabel21.Name = "XrLabel21"
        Me.XrLabel21.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel21.SizeF = New System.Drawing.SizeF(683.488!, 227.8959!)
        Me.XrLabel21.StylePriority.UseBorderColor = False
        Me.XrLabel21.StylePriority.UseBorders = False
        Me.XrLabel21.StylePriority.UseFont = False
        Me.XrLabel21.StylePriority.UseTextAlignment = False
        Me.XrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel5
        '
        Me.XrLabel5.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel5.Dpi = 254.0!
        Me.XrLabel5.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel5.LocationFloat = New DevExpress.Utils.PointFloat(0!, 100.7517!)
        Me.XrLabel5.Multiline = True
        Me.XrLabel5.Name = "XrLabel5"
        Me.XrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel5.SizeF = New System.Drawing.SizeF(766.0493!, 90.16837!)
        Me.XrLabel5.StylePriority.UseBorderColor = False
        Me.XrLabel5.StylePriority.UseBorders = False
        Me.XrLabel5.StylePriority.UseFont = False
        Me.XrLabel5.StylePriority.UseTextAlignment = False
        Me.XrLabel5.Text = "SAFE TRANSFER"
        Me.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel7
        '
        Me.XrLabel7.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel7.Dpi = 254.0!
        Me.XrLabel7.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(0!, 0!)
        Me.XrLabel7.Multiline = True
        Me.XrLabel7.Name = "XrLabel7"
        Me.XrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel7.SizeF = New System.Drawing.SizeF(766.0493!, 100.7517!)
        Me.XrLabel7.StylePriority.UseBorderColor = False
        Me.XrLabel7.StylePriority.UseBorders = False
        Me.XrLabel7.StylePriority.UseFont = False
        Me.XrLabel7.StylePriority.UseTextAlignment = False
        Me.XrLabel7.Text = "نقل ما بين الخزائن"
        Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrShape2
        '
        Me.XrShape2.Dpi = 254.0!
        Me.XrShape2.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.XrShape2.LineWidth = 3.0!
        Me.XrShape2.LocationFloat = New DevExpress.Utils.PointFloat(18.00005!, 297.8293!)
        Me.XrShape2.Name = "XrShape2"
        ShapeRectangle1.Fillet = 20
        Me.XrShape2.Shape = ShapeRectangle1
        Me.XrShape2.SizeF = New System.Drawing.SizeF(2001.208!, 119.6416!)
        Me.XrShape2.StylePriority.UseForeColor = False
        '
        'XrShape3
        '
        Me.XrShape3.Dpi = 254.0!
        Me.XrShape3.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.XrShape3.LineWidth = 3.0!
        Me.XrShape3.LocationFloat = New DevExpress.Utils.PointFloat(18.00005!, 815.2743!)
        Me.XrShape3.Name = "XrShape3"
        ShapeRectangle2.Fillet = 20
        Me.XrShape3.Shape = ShapeRectangle2
        Me.XrShape3.SizeF = New System.Drawing.SizeF(2001.208!, 262.5166!)
        Me.XrShape3.StylePriority.UseForeColor = False
        '
        'XrShape1
        '
        Me.XrShape1.Dpi = 254.0!
        Me.XrShape1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.XrShape1.LineWidth = 3.0!
        Me.XrShape1.LocationFloat = New DevExpress.Utils.PointFloat(18.00005!, 468.3284!)
        Me.XrShape1.Name = "XrShape1"
        ShapeRectangle3.Fillet = 20
        Me.XrShape1.Shape = ShapeRectangle3
        Me.XrShape1.SizeF = New System.Drawing.SizeF(2001.208!, 288.9749!)
        Me.XrShape1.StylePriority.UseForeColor = False
        '
        'Detail
        '
        Me.Detail.Dpi = 254.0!
        Me.Detail.Expanded = False
        Me.Detail.HeightF = 63.42!
        Me.Detail.HierarchyPrintOptions.Indent = 50.8!
        Me.Detail.Name = "Detail"
        '
        'SqlDataSource1
        '
        Me.SqlDataSource1.ConnectionName = "localhost_EXCHANGESYS_Connection 17"
        Me.SqlDataSource1.Name = "SqlDataSource1"
        ColumnExpression1.ColumnName = "WithdrawalID"
        Table1.Name = "WithdrawalTb"
        ColumnExpression1.Table = Table1
        Column1.Expression = ColumnExpression1
        ColumnExpression2.ColumnName = "WIDCode"
        ColumnExpression2.Table = Table1
        Column2.Expression = ColumnExpression2
        ColumnExpression3.ColumnName = "WDCode"
        ColumnExpression3.Table = Table1
        Column3.Expression = ColumnExpression3
        ColumnExpression4.ColumnName = "WithdrawalDate"
        ColumnExpression4.Table = Table1
        Column4.Expression = ColumnExpression4
        ColumnExpression5.ColumnName = "WithdrawalFrom"
        ColumnExpression5.Table = Table1
        Column5.Expression = ColumnExpression5
        ColumnExpression6.ColumnName = "WithdrawalTo"
        ColumnExpression6.Table = Table1
        Column6.Expression = ColumnExpression6
        ColumnExpression7.ColumnName = "CurrencyID"
        ColumnExpression7.Table = Table1
        Column7.Expression = ColumnExpression7
        ColumnExpression8.ColumnName = "WithdrawalValue"
        ColumnExpression8.Table = Table1
        Column8.Expression = ColumnExpression8
        ColumnExpression9.ColumnName = "Notes"
        ColumnExpression9.Table = Table1
        Column9.Expression = ColumnExpression9
        ColumnExpression10.ColumnName = "IsActive"
        ColumnExpression10.Table = Table1
        Column10.Expression = ColumnExpression10
        ColumnExpression11.ColumnName = "BranchID"
        ColumnExpression11.Table = Table1
        Column11.Expression = ColumnExpression11
        ColumnExpression12.ColumnName = "SAFEID"
        ColumnExpression12.Table = Table1
        Column12.Expression = ColumnExpression12
        SelectQuery1.Columns.Add(Column1)
        SelectQuery1.Columns.Add(Column2)
        SelectQuery1.Columns.Add(Column3)
        SelectQuery1.Columns.Add(Column4)
        SelectQuery1.Columns.Add(Column5)
        SelectQuery1.Columns.Add(Column6)
        SelectQuery1.Columns.Add(Column7)
        SelectQuery1.Columns.Add(Column8)
        SelectQuery1.Columns.Add(Column9)
        SelectQuery1.Columns.Add(Column10)
        SelectQuery1.Columns.Add(Column11)
        SelectQuery1.Columns.Add(Column12)
        SelectQuery1.Name = "WithdrawalTb"
        SelectQuery1.Tables.Add(Table1)
        Me.SqlDataSource1.Queries.AddRange(New DevExpress.DataAccess.Sql.SqlQuery() {SelectQuery1})
        Me.SqlDataSource1.ResultSchemaSerializable = resources.GetString("SqlDataSource1.ResultSchemaSerializable")
        '
        'Title
        '
        Me.Title.BackColor = System.Drawing.Color.Transparent
        Me.Title.BorderColor = System.Drawing.Color.Black
        Me.Title.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.Title.BorderWidth = 1.0!
        Me.Title.Font = New DevExpress.Drawing.DXFont("Arial", 14.25!)
        Me.Title.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Title.Name = "Title"
        Me.Title.Padding = New DevExpress.XtraPrinting.PaddingInfo(15.0!, 15.0!, 0!, 0!, 254.0!)
        '
        'DetailCaption1
        '
        Me.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(CType(CType(93, Byte), Integer), CType(CType(98, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.DetailCaption1.BorderColor = System.Drawing.Color.White
        Me.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left
        Me.DetailCaption1.BorderWidth = 2.0!
        Me.DetailCaption1.Font = New DevExpress.Drawing.DXFont("Arial", 8.25!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.DetailCaption1.ForeColor = System.Drawing.Color.White
        Me.DetailCaption1.Name = "DetailCaption1"
        Me.DetailCaption1.Padding = New DevExpress.XtraPrinting.PaddingInfo(15.0!, 15.0!, 0!, 0!, 254.0!)
        Me.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        '
        'DetailData1
        '
        Me.DetailData1.BorderColor = System.Drawing.Color.Transparent
        Me.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left
        Me.DetailData1.BorderWidth = 2.0!
        Me.DetailData1.Font = New DevExpress.Drawing.DXFont("Arial", 8.25!)
        Me.DetailData1.ForeColor = System.Drawing.Color.Black
        Me.DetailData1.Name = "DetailData1"
        Me.DetailData1.Padding = New DevExpress.XtraPrinting.PaddingInfo(15.0!, 15.0!, 0!, 0!, 254.0!)
        Me.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        '
        'DetailData3_Odd
        '
        Me.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent
        Me.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.DetailData3_Odd.BorderWidth = 1.0!
        Me.DetailData3_Odd.Font = New DevExpress.Drawing.DXFont("Arial", 8.25!)
        Me.DetailData3_Odd.ForeColor = System.Drawing.Color.Black
        Me.DetailData3_Odd.Name = "DetailData3_Odd"
        Me.DetailData3_Odd.Padding = New DevExpress.XtraPrinting.PaddingInfo(15.0!, 15.0!, 0!, 0!, 254.0!)
        Me.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        '
        'PageInfo
        '
        Me.PageInfo.Font = New DevExpress.Drawing.DXFont("Arial", 8.25!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.PageInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.PageInfo.Name = "PageInfo"
        Me.PageInfo.Padding = New DevExpress.XtraPrinting.PaddingInfo(15.0!, 15.0!, 0!, 0!, 254.0!)
        '
        'RPTSafeTransfer
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.TopMargin, Me.BottomMargin, Me.ReportHeader, Me.Detail})
        Me.ComponentStorage.AddRange(New System.ComponentModel.IComponent() {Me.SqlDataSource1})
        Me.DataMember = "WithdrawalTb"
        Me.DataSource = Me.SqlDataSource1
        Me.Dpi = 254.0!
        Me.Font = New DevExpress.Drawing.DXFont("Arial", 9.75!)
        Me.Margins = New DevExpress.Drawing.DXMargins(37.0!, 29.0!, 29.10833!, 23.84254!)
        Me.PageHeightF = 2970.0!
        Me.PageWidthF = 2100.0!
        Me.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4
        Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
        Me.SnapGridSize = 25.0!
        Me.StyleSheet.AddRange(New DevExpress.XtraReports.UI.XRControlStyle() {Me.Title, Me.DetailCaption1, Me.DetailData1, Me.DetailData3_Odd, Me.PageInfo})
        Me.Version = "25.1"
        XrWatermark1.Id = "Watermark1"
        Me.Watermarks.AddRange(New DevExpress.XtraPrinting.Drawing.Watermark() {XrWatermark1})
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand
    Friend WithEvents ReportHeader As DevExpress.XtraReports.UI.ReportHeaderBand
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents SqlDataSource1 As DevExpress.DataAccess.Sql.SqlDataSource
    Friend WithEvents Title As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailCaption1 As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailData1 As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailData3_Odd As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents PageInfo As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents CURRENCYID As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel14 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel12 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox2 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel21 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel5 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel11 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox5 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox4 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel16 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox3 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox1 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel17 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel18 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel19 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel20 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel22 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox6 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox8 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel51 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel50 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel23 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel25 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel26 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrShape3 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrShape2 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrShape1 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrPictureBox11 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox12 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel9 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox20 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel61 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox26 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel27 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox43 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel35 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo8 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrLabel44 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo7 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrLabel43 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox24 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox23 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox25 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox21 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel13 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox7 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel24 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel28 As DevExpress.XtraReports.UI.XRLabel
End Class
