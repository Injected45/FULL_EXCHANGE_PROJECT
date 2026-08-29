<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RPTEMPWITHDRAWAL2
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RPTEMPWITHDRAWAL2))
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
        Me.XrLabel13 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.IDNo = New DevExpress.XtraReports.UI.XRLabel()
        Me.PaidFor = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox14 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox6 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox5 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel58 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel55 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel60 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel52 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.Phone = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox4 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel24 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox23 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox24 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox43 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel35 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel43 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo7 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrLabel44 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo8 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrLabel11 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox20 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel8 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox21 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox25 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel9 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox26 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox1 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox7 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel12 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel14 = New DevExpress.XtraReports.UI.XRLabel()
        Me.CURRENCYID = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox9 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel17 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox10 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel25 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox32 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox33 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel81 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel82 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel83 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel84 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel85 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel86 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox34 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel87 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel88 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox35 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox36 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel91 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel93 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel94 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel95 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel96 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox2 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel21 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel5 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrShape2 = New DevExpress.XtraReports.UI.XRShape()
        Me.XrShape1 = New DevExpress.XtraReports.UI.XRShape()
        Me.XrShape3 = New DevExpress.XtraReports.UI.XRShape()
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
        Me.TopMargin.HeightF = 21.17083!
        Me.TopMargin.Name = "TopMargin"
        '
        'BottomMargin
        '
        Me.BottomMargin.Dpi = 254.0!
        Me.BottomMargin.HeightF = 21.88174!
        Me.BottomMargin.Name = "BottomMargin"
        '
        'ReportHeader
        '
        Me.ReportHeader.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel13, Me.XrLabel3, Me.IDNo, Me.PaidFor, Me.XrPictureBox14, Me.XrPictureBox6, Me.XrPictureBox5, Me.XrLabel58, Me.XrLabel55, Me.XrLabel60, Me.XrLabel52, Me.XrLabel4, Me.Phone, Me.XrPictureBox4, Me.XrLabel24, Me.XrPictureBox23, Me.XrPictureBox24, Me.XrPictureBox43, Me.XrLabel35, Me.XrLabel43, Me.XrPageInfo7, Me.XrLabel44, Me.XrPageInfo8, Me.XrLabel11, Me.XrPictureBox20, Me.XrLabel8, Me.XrPictureBox21, Me.XrPictureBox25, Me.XrLabel9, Me.XrLabel10, Me.XrPictureBox26, Me.XrPictureBox1, Me.XrLabel1, Me.XrLabel2, Me.XrPictureBox7, Me.XrLabel12, Me.XrLabel14, Me.CURRENCYID, Me.XrLabel15, Me.XrPictureBox9, Me.XrLabel17, Me.XrPictureBox10, Me.XrLabel25, Me.XrPictureBox32, Me.XrPictureBox33, Me.XrLabel81, Me.XrLabel82, Me.XrLabel83, Me.XrLabel84, Me.XrLabel85, Me.XrLabel86, Me.XrPictureBox34, Me.XrLabel87, Me.XrLabel88, Me.XrPictureBox35, Me.XrPictureBox36, Me.XrLabel91, Me.XrLabel93, Me.XrLabel94, Me.XrLabel95, Me.XrLabel96, Me.XrPictureBox2, Me.XrLabel21, Me.XrLabel5, Me.XrLabel7, Me.XrShape2, Me.XrShape1, Me.XrShape3})
        Me.ReportHeader.Dpi = 254.0!
        Me.ReportHeader.HeightF = 1444.204!
        Me.ReportHeader.Name = "ReportHeader"
        '
        'XrLabel13
        '
        Me.XrLabel13.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel13.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel13.BorderWidth = 1.5!
        Me.XrLabel13.Dpi = 254.0!
        Me.XrLabel13.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel13.LocationFloat = New DevExpress.Utils.PointFloat(46.29102!, 1028.539!)
        Me.XrLabel13.Multiline = True
        Me.XrLabel13.Name = "XrLabel13"
        Me.XrLabel13.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel13.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel13.StylePriority.UseBorderColor = False
        Me.XrLabel13.StylePriority.UseBorders = False
        Me.XrLabel13.StylePriority.UseBorderWidth = False
        Me.XrLabel13.StylePriority.UseFont = False
        Me.XrLabel13.StylePriority.UseTextAlignment = False
        Me.XrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel3
        '
        Me.XrLabel3.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel3.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel3.BorderWidth = 1.5!
        Me.XrLabel3.Dpi = 254.0!
        Me.XrLabel3.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(46.33057!, 921.2856!)
        Me.XrLabel3.Multiline = True
        Me.XrLabel3.Name = "XrLabel3"
        Me.XrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel3.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel3.StylePriority.UseBorderColor = False
        Me.XrLabel3.StylePriority.UseBorders = False
        Me.XrLabel3.StylePriority.UseBorderWidth = False
        Me.XrLabel3.StylePriority.UseFont = False
        Me.XrLabel3.StylePriority.UseTextAlignment = False
        Me.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'IDNo
        '
        Me.IDNo.BorderColor = System.Drawing.Color.LightGray
        Me.IDNo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid
        Me.IDNo.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.IDNo.CanGrow = False
        Me.IDNo.Dpi = 254.0!
        Me.IDNo.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[IDNo]")})
        Me.IDNo.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.IDNo.LocationFloat = New DevExpress.Utils.PointFloat(1158.074!, 928.1403!)
        Me.IDNo.Multiline = True
        Me.IDNo.Name = "IDNo"
        Me.IDNo.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.IDNo.SizeF = New System.Drawing.SizeF(532.8884!, 90.16852!)
        Me.IDNo.StylePriority.UseBorderColor = False
        Me.IDNo.StylePriority.UseBorderDashStyle = False
        Me.IDNo.StylePriority.UseBorders = False
        Me.IDNo.StylePriority.UseFont = False
        Me.IDNo.StylePriority.UseTextAlignment = False
        Me.IDNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'PaidFor
        '
        Me.PaidFor.BorderColor = System.Drawing.Color.LightGray
        Me.PaidFor.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid
        Me.PaidFor.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.PaidFor.CanGrow = False
        Me.PaidFor.Dpi = 254.0!
        Me.PaidFor.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PaidFor]")})
        Me.PaidFor.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.PaidFor.LocationFloat = New DevExpress.Utils.PointFloat(1158.074!, 831.1171!)
        Me.PaidFor.Multiline = True
        Me.PaidFor.Name = "PaidFor"
        Me.PaidFor.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.PaidFor.SizeF = New System.Drawing.SizeF(510.7412!, 90.16846!)
        Me.PaidFor.StylePriority.UseBorderColor = False
        Me.PaidFor.StylePriority.UseBorderDashStyle = False
        Me.PaidFor.StylePriority.UseBorders = False
        Me.PaidFor.StylePriority.UseFont = False
        Me.PaidFor.StylePriority.UseTextAlignment = False
        Me.PaidFor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox14
        '
        Me.XrPictureBox14.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox14.Dpi = 254.0!
        Me.XrPictureBox14.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox14.ImageSource"))
        Me.XrPictureBox14.LocationFloat = New DevExpress.Utils.PointFloat(1045.11!, 951.6003!)
        Me.XrPictureBox14.Name = "XrPictureBox14"
        Me.XrPictureBox14.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox14.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox14.StylePriority.UseBorderColor = False
        '
        'XrPictureBox6
        '
        Me.XrPictureBox6.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox6.Dpi = 254.0!
        Me.XrPictureBox6.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox6.ImageSource"))
        Me.XrPictureBox6.LocationFloat = New DevExpress.Utils.PointFloat(1923.774!, 941.3702!)
        Me.XrPictureBox6.Name = "XrPictureBox6"
        Me.XrPictureBox6.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox6.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox6.StylePriority.UseBorderColor = False
        '
        'XrPictureBox5
        '
        Me.XrPictureBox5.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox5.Dpi = 254.0!
        Me.XrPictureBox5.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox5.ImageSource"))
        Me.XrPictureBox5.LocationFloat = New DevExpress.Utils.PointFloat(1923.774!, 844.3471!)
        Me.XrPictureBox5.Name = "XrPictureBox5"
        Me.XrPictureBox5.SizeF = New System.Drawing.SizeF(60.00598!, 61.0643!)
        Me.XrPictureBox5.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox5.StylePriority.UseBorderColor = False
        '
        'XrLabel58
        '
        Me.XrLabel58.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel58.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel58.Dpi = 254.0!
        Me.XrLabel58.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel58.LocationFloat = New DevExpress.Utils.PointFloat(1672.844!, 831.1171!)
        Me.XrLabel58.Multiline = True
        Me.XrLabel58.Name = "XrLabel58"
        Me.XrLabel58.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel58.SizeF = New System.Drawing.SizeF(250.9304!, 90.16846!)
        Me.XrLabel58.StylePriority.UseBorderColor = False
        Me.XrLabel58.StylePriority.UseBorders = False
        Me.XrLabel58.StylePriority.UseFont = False
        Me.XrLabel58.StylePriority.UseTextAlignment = False
        Me.XrLabel58.Text = ":قـــبــــض مـــن"
        Me.XrLabel58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel55
        '
        Me.XrLabel55.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel55.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel55.Dpi = 254.0!
        Me.XrLabel55.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel55.LocationFloat = New DevExpress.Utils.PointFloat(1691.365!, 928.1403!)
        Me.XrLabel55.Multiline = True
        Me.XrLabel55.Name = "XrLabel55"
        Me.XrLabel55.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel55.SizeF = New System.Drawing.SizeF(232.4094!, 90.16846!)
        Me.XrLabel55.StylePriority.UseBorderColor = False
        Me.XrLabel55.StylePriority.UseBorders = False
        Me.XrLabel55.StylePriority.UseFont = False
        Me.XrLabel55.StylePriority.UseTextAlignment = False
        Me.XrLabel55.Text = ":إثبات شخصي"
        Me.XrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel60
        '
        Me.XrLabel60.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel60.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel60.Dpi = 254.0!
        Me.XrLabel60.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel60.LocationFloat = New DevExpress.Utils.PointFloat(812.7012!, 938.3703!)
        Me.XrLabel60.Multiline = True
        Me.XrLabel60.Name = "XrLabel60"
        Me.XrLabel60.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel60.SizeF = New System.Drawing.SizeF(232.4091!, 90.16846!)
        Me.XrLabel60.StylePriority.UseBorderColor = False
        Me.XrLabel60.StylePriority.UseBorders = False
        Me.XrLabel60.StylePriority.UseFont = False
        Me.XrLabel60.StylePriority.UseTextAlignment = False
        Me.XrLabel60.Text = ":الــتـــوقــيــــــــع"
        Me.XrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel52
        '
        Me.XrLabel52.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel52.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash
        Me.XrLabel52.Borders = DevExpress.XtraPrinting.BorderSide.Bottom
        Me.XrLabel52.Dpi = 254.0!
        Me.XrLabel52.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel52.LocationFloat = New DevExpress.Utils.PointFloat(236.8306!, 991.2872!)
        Me.XrLabel52.Multiline = True
        Me.XrLabel52.Name = "XrLabel52"
        Me.XrLabel52.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel52.SizeF = New System.Drawing.SizeF(575.4678!, 5.501831!)
        Me.XrLabel52.StylePriority.UseBorderColor = False
        Me.XrLabel52.StylePriority.UseBorderDashStyle = False
        Me.XrLabel52.StylePriority.UseBorders = False
        Me.XrLabel52.StylePriority.UseFont = False
        Me.XrLabel52.StylePriority.UseTextAlignment = False
        Me.XrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel4
        '
        Me.XrLabel4.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel4.Dpi = 254.0!
        Me.XrLabel4.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(847.1378!, 831.1172!)
        Me.XrLabel4.Multiline = True
        Me.XrLabel4.Name = "XrLabel4"
        Me.XrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel4.SizeF = New System.Drawing.SizeF(250.9304!, 90.16846!)
        Me.XrLabel4.StylePriority.UseBorderColor = False
        Me.XrLabel4.StylePriority.UseBorders = False
        Me.XrLabel4.StylePriority.UseFont = False
        Me.XrLabel4.StylePriority.UseTextAlignment = False
        Me.XrLabel4.Text = ":الهـــــــاتــــــــف"
        Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'Phone
        '
        Me.Phone.BorderColor = System.Drawing.Color.LightGray
        Me.Phone.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid
        Me.Phone.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.Phone.CanGrow = False
        Me.Phone.Dpi = 254.0!
        Me.Phone.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Phone2]")})
        Me.Phone.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.Phone.LocationFloat = New DevExpress.Utils.PointFloat(45.25152!, 831.1172!)
        Me.Phone.Multiline = True
        Me.Phone.Name = "Phone"
        Me.Phone.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.Phone.SizeF = New System.Drawing.SizeF(797.8574!, 90.16833!)
        Me.Phone.StylePriority.UseBorderColor = False
        Me.Phone.StylePriority.UseBorderDashStyle = False
        Me.Phone.StylePriority.UseBorders = False
        Me.Phone.StylePriority.UseFont = False
        Me.Phone.StylePriority.UseTextAlignment = False
        Me.Phone.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox4
        '
        Me.XrPictureBox4.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox4.Dpi = 254.0!
        Me.XrPictureBox4.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox4.ImageSource"))
        Me.XrPictureBox4.LocationFloat = New DevExpress.Utils.PointFloat(1098.068!, 844.3472!)
        Me.XrPictureBox4.Name = "XrPictureBox4"
        Me.XrPictureBox4.SizeF = New System.Drawing.SizeF(60.0061!, 61.06409!)
        Me.XrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox4.StylePriority.UseBorderColor = False
        '
        'XrLabel24
        '
        Me.XrLabel24.Dpi = 254.0!
        Me.XrLabel24.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel24.LocationFloat = New DevExpress.Utils.PointFloat(1163.091!, 1307.613!)
        Me.XrLabel24.Multiline = True
        Me.XrLabel24.Name = "XrLabel24"
        Me.XrLabel24.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel24.SizeF = New System.Drawing.SizeF(553.4926!, 66.99609!)
        Me.XrLabel24.StylePriority.UseFont = False
        Me.XrLabel24.StylePriority.UseTextAlignment = False
        Me.XrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        '
        'XrPictureBox23
        '
        Me.XrPictureBox23.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox23.Dpi = 254.0!
        Me.XrPictureBox23.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox23.ImageSource"))
        Me.XrPictureBox23.LocationFloat = New DevExpress.Utils.PointFloat(534.3876!, 1312.693!)
        Me.XrPictureBox23.Name = "XrPictureBox23"
        Me.XrPictureBox23.SizeF = New System.Drawing.SizeF(54.71436!, 56.73669!)
        Me.XrPictureBox23.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox23.StylePriority.UseBorderColor = False
        '
        'XrPictureBox24
        '
        Me.XrPictureBox24.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox24.Dpi = 254.0!
        Me.XrPictureBox24.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox24.ImageSource"))
        Me.XrPictureBox24.LocationFloat = New DevExpress.Utils.PointFloat(1108.377!, 1312.693!)
        Me.XrPictureBox24.Name = "XrPictureBox24"
        Me.XrPictureBox24.SizeF = New System.Drawing.SizeF(54.71436!, 56.73682!)
        Me.XrPictureBox24.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox24.StylePriority.UseBorderColor = False
        '
        'XrPictureBox43
        '
        Me.XrPictureBox43.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox43.Dpi = 254.0!
        Me.XrPictureBox43.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox43.ImageSource"))
        Me.XrPictureBox43.LocationFloat = New DevExpress.Utils.PointFloat(1930.351!, 1307.613!)
        Me.XrPictureBox43.Name = "XrPictureBox43"
        Me.XrPictureBox43.SizeF = New System.Drawing.SizeF(56.89636!, 62.02869!)
        Me.XrPictureBox43.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox43.StylePriority.UseBorderColor = False
        '
        'XrLabel35
        '
        Me.XrLabel35.Dpi = 254.0!
        Me.XrLabel35.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel35.LocationFloat = New DevExpress.Utils.PointFloat(1719.23!, 1307.613!)
        Me.XrLabel35.Name = "XrLabel35"
        Me.XrLabel35.SizeF = New System.Drawing.SizeF(211.1215!, 66.99609!)
        Me.XrLabel35.StylePriority.UseFont = False
        Me.XrLabel35.StylePriority.UseTextAlignment = False
        Me.XrLabel35.Text = ":اسم المستخدم"
        Me.XrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrLabel43
        '
        Me.XrLabel43.Dpi = 254.0!
        Me.XrLabel43.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel43.LocationFloat = New DevExpress.Utils.PointFloat(919.0254!, 1307.613!)
        Me.XrLabel43.Name = "XrLabel43"
        Me.XrLabel43.SizeF = New System.Drawing.SizeF(189.352!, 66.99609!)
        Me.XrLabel43.StylePriority.UseFont = False
        Me.XrLabel43.StylePriority.UseTextAlignment = False
        Me.XrLabel43.Text = "تاريخ الطباعة"
        Me.XrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel43.TextFormatString = "{0:hh:mm tt}"
        '
        'XrPageInfo7
        '
        Me.XrPageInfo7.Dpi = 254.0!
        Me.XrPageInfo7.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo7.LocationFloat = New DevExpress.Utils.PointFloat(656.9952!, 1307.613!)
        Me.XrPageInfo7.Name = "XrPageInfo7"
        Me.XrPageInfo7.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo7.SizeF = New System.Drawing.SizeF(262.0302!, 66.99609!)
        Me.XrPageInfo7.StylePriority.UseFont = False
        Me.XrPageInfo7.StylePriority.UseTextAlignment = False
        Me.XrPageInfo7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrPageInfo7.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel44
        '
        Me.XrLabel44.Dpi = 254.0!
        Me.XrLabel44.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel44.LocationFloat = New DevExpress.Utils.PointFloat(346.9039!, 1307.613!)
        Me.XrLabel44.Name = "XrLabel44"
        Me.XrLabel44.SizeF = New System.Drawing.SizeF(187.4837!, 66.99609!)
        Me.XrLabel44.StylePriority.UseFont = False
        Me.XrLabel44.StylePriority.UseTextAlignment = False
        Me.XrLabel44.Text = " وقت الطباعة"
        Me.XrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel44.TextFormatString = "{0:hh:mm tt}"
        '
        'XrPageInfo8
        '
        Me.XrPageInfo8.Dpi = 254.0!
        Me.XrPageInfo8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo8.LocationFloat = New DevExpress.Utils.PointFloat(79.88776!, 1307.613!)
        Me.XrPageInfo8.Name = "XrPageInfo8"
        Me.XrPageInfo8.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo8.SizeF = New System.Drawing.SizeF(267.0162!, 66.99609!)
        Me.XrPageInfo8.StylePriority.UseFont = False
        Me.XrPageInfo8.StylePriority.UseTextAlignment = False
        Me.XrPageInfo8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrPageInfo8.TextFormatString = "{0:hh:mm:ss}"
        '
        'XrLabel11
        '
        Me.XrLabel11.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel11.Dpi = 254.0!
        Me.XrLabel11.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel11.LocationFloat = New DevExpress.Utils.PointFloat(1718.747!, 1198.236!)
        Me.XrLabel11.Multiline = True
        Me.XrLabel11.Name = "XrLabel11"
        Me.XrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel11.SizeF = New System.Drawing.SizeF(215.9495!, 61.97656!)
        Me.XrLabel11.StylePriority.UseBorderColor = False
        Me.XrLabel11.StylePriority.UseBorders = False
        Me.XrLabel11.StylePriority.UseFont = False
        Me.XrLabel11.StylePriority.UseTextAlignment = False
        Me.XrLabel11.Text = "للإستفســـار"
        Me.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox20
        '
        Me.XrPictureBox20.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox20.Dpi = 254.0!
        Me.XrPictureBox20.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox20.ImageSource"))
        Me.XrPictureBox20.LocationFloat = New DevExpress.Utils.PointFloat(1934.696!, 1203.74!)
        Me.XrPictureBox20.Name = "XrPictureBox20"
        Me.XrPictureBox20.SizeF = New System.Drawing.SizeF(52.0686!, 50.48108!)
        Me.XrPictureBox20.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox20.StylePriority.UseBorderColor = False
        '
        'XrLabel8
        '
        Me.XrLabel8.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel8.CanGrow = False
        Me.XrLabel8.Dpi = 254.0!
        Me.XrLabel8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel8.LocationFloat = New DevExpress.Utils.PointFloat(600.2823!, 1198.236!)
        Me.XrLabel8.Multiline = True
        Me.XrLabel8.Name = "XrLabel8"
        Me.XrLabel8.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel8.SizeF = New System.Drawing.SizeF(486.0063!, 61.97656!)
        Me.XrLabel8.StylePriority.UseBorderColor = False
        Me.XrLabel8.StylePriority.UseBorders = False
        Me.XrLabel8.StylePriority.UseFont = False
        Me.XrLabel8.StylePriority.UseTextAlignment = False
        Me.XrLabel8.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox21
        '
        Me.XrPictureBox21.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox21.Dpi = 254.0!
        Me.XrPictureBox21.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox21.ImageSource"))
        Me.XrPictureBox21.LocationFloat = New DevExpress.Utils.PointFloat(1086.289!, 1203.74!)
        Me.XrPictureBox21.Name = "XrPictureBox21"
        Me.XrPictureBox21.SizeF = New System.Drawing.SizeF(54.71436!, 50.48108!)
        Me.XrPictureBox21.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox21.StylePriority.UseBorderColor = False
        '
        'XrPictureBox25
        '
        Me.XrPictureBox25.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox25.Dpi = 254.0!
        Me.XrPictureBox25.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox25.ImageSource"))
        Me.XrPictureBox25.LocationFloat = New DevExpress.Utils.PointFloat(514.723!, 1203.74!)
        Me.XrPictureBox25.Name = "XrPictureBox25"
        Me.XrPictureBox25.SizeF = New System.Drawing.SizeF(52.06854!, 50.48108!)
        Me.XrPictureBox25.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox25.StylePriority.UseBorderColor = False
        '
        'XrLabel9
        '
        Me.XrLabel9.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel9.CanGrow = False
        Me.XrLabel9.Dpi = 254.0!
        Me.XrLabel9.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel9.LocationFloat = New DevExpress.Utils.PointFloat(17.99997!, 1198.236!)
        Me.XrLabel9.Multiline = True
        Me.XrLabel9.Name = "XrLabel9"
        Me.XrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel9.SizeF = New System.Drawing.SizeF(496.7231!, 61.97656!)
        Me.XrLabel9.StylePriority.UseBorderColor = False
        Me.XrLabel9.StylePriority.UseBorders = False
        Me.XrLabel9.StylePriority.UseFont = False
        Me.XrLabel9.StylePriority.UseTextAlignment = False
        Me.XrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrLabel10
        '
        Me.XrLabel10.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel10.Dpi = 254.0!
        Me.XrLabel10.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Phone]")})
        Me.XrLabel10.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(1174.465!, 1198.236!)
        Me.XrLabel10.Multiline = True
        Me.XrLabel10.Name = "XrLabel10"
        Me.XrLabel10.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel10.SizeF = New System.Drawing.SizeF(458.7068!, 61.97656!)
        Me.XrLabel10.StylePriority.UseBorderColor = False
        Me.XrLabel10.StylePriority.UseBorders = False
        Me.XrLabel10.StylePriority.UseFont = False
        Me.XrLabel10.StylePriority.UseTextAlignment = False
        Me.XrLabel10.Text = "0924565555-0915658978"
        Me.XrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox26
        '
        Me.XrPictureBox26.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox26.Dpi = 254.0!
        Me.XrPictureBox26.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox26.ImageSource"))
        Me.XrPictureBox26.LocationFloat = New DevExpress.Utils.PointFloat(1633.172!, 1203.74!)
        Me.XrPictureBox26.Name = "XrPictureBox26"
        Me.XrPictureBox26.SizeF = New System.Drawing.SizeF(53.34949!, 50.48108!)
        Me.XrPictureBox26.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox26.StylePriority.UseBorderColor = False
        '
        'XrPictureBox1
        '
        Me.XrPictureBox1.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox1.Dpi = 254.0!
        Me.XrPictureBox1.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource(Global.ExchangeSystem.My.Resources.Resources.G_dollar, True)
        Me.XrPictureBox1.LocationFloat = New DevExpress.Utils.PointFloat(1919.745!, 496.1353!)
        Me.XrPictureBox1.Name = "XrPictureBox1"
        Me.XrPictureBox1.SizeF = New System.Drawing.SizeF(60.0061!, 61.06424!)
        Me.XrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox1.StylePriority.UseBorderColor = False
        '
        'XrLabel1
        '
        Me.XrLabel1.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel1.Dpi = 254.0!
        Me.XrLabel1.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(1686.522!, 482.9061!)
        Me.XrLabel1.Multiline = True
        Me.XrLabel1.Name = "XrLabel1"
        Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel1.SizeF = New System.Drawing.SizeF(233.2229!, 90.16843!)
        Me.XrLabel1.StylePriority.UseBorderColor = False
        Me.XrLabel1.StylePriority.UseBorders = False
        Me.XrLabel1.StylePriority.UseFont = False
        Me.XrLabel1.StylePriority.UseTextAlignment = False
        Me.XrLabel1.Text = ":اسم العملة"
        Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel2
        '
        Me.XrLabel2.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel2.Dpi = 254.0!
        Me.XrLabel2.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(1158.113!, 482.9065!)
        Me.XrLabel2.Multiline = True
        Me.XrLabel2.Name = "XrLabel2"
        Me.XrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel2.SizeF = New System.Drawing.SizeF(528.4091!, 90.16846!)
        Me.XrLabel2.StylePriority.UseBorderColor = False
        Me.XrLabel2.StylePriority.UseBorders = False
        Me.XrLabel2.StylePriority.UseFont = False
        Me.XrLabel2.StylePriority.UseTextAlignment = False
        Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox7
        '
        Me.XrPictureBox7.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox7.Dpi = 254.0!
        Me.XrPictureBox7.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox7.ImageSource"))
        Me.XrPictureBox7.LocationFloat = New DevExpress.Utils.PointFloat(1919.745!, 324.8591!)
        Me.XrPictureBox7.Name = "XrPictureBox7"
        Me.XrPictureBox7.SizeF = New System.Drawing.SizeF(60.00598!, 61.06424!)
        Me.XrPictureBox7.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox7.StylePriority.UseBorderColor = False
        '
        'XrLabel12
        '
        Me.XrLabel12.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel12.Dpi = 254.0!
        Me.XrLabel12.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel12.LocationFloat = New DevExpress.Utils.PointFloat(1686.522!, 311.63!)
        Me.XrLabel12.Multiline = True
        Me.XrLabel12.Name = "XrLabel12"
        Me.XrLabel12.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel12.SizeF = New System.Drawing.SizeF(233.2229!, 90.16849!)
        Me.XrLabel12.StylePriority.UseBorderColor = False
        Me.XrLabel12.StylePriority.UseBorders = False
        Me.XrLabel12.StylePriority.UseFont = False
        Me.XrLabel12.StylePriority.UseTextAlignment = False
        Me.XrLabel12.Text = ":رقم المعاملة"
        Me.XrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel14
        '
        Me.XrLabel14.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel14.Dpi = 254.0!
        Me.XrLabel14.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Code]")})
        Me.XrLabel14.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel14.LocationFloat = New DevExpress.Utils.PointFloat(1158.113!, 311.63!)
        Me.XrLabel14.Multiline = True
        Me.XrLabel14.Name = "XrLabel14"
        Me.XrLabel14.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel14.SizeF = New System.Drawing.SizeF(528.4087!, 90.16846!)
        Me.XrLabel14.StylePriority.UseBorderColor = False
        Me.XrLabel14.StylePriority.UseBorders = False
        Me.XrLabel14.StylePriority.UseFont = False
        Me.XrLabel14.StylePriority.UseTextAlignment = False
        Me.XrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'CURRENCYID
        '
        Me.CURRENCYID.BorderColor = System.Drawing.Color.LightGray
        Me.CURRENCYID.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.CURRENCYID.Dpi = 254.0!
        Me.CURRENCYID.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InsertDate]")})
        Me.CURRENCYID.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.CURRENCYID.LocationFloat = New DevExpress.Utils.PointFloat(68.1394!, 311.6303!)
        Me.CURRENCYID.Multiline = True
        Me.CURRENCYID.Name = "CURRENCYID"
        Me.CURRENCYID.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.CURRENCYID.SizeF = New System.Drawing.SizeF(269.8591!, 90.16843!)
        Me.CURRENCYID.StylePriority.UseBorderColor = False
        Me.CURRENCYID.StylePriority.UseBorders = False
        Me.CURRENCYID.StylePriority.UseFont = False
        Me.CURRENCYID.StylePriority.UseTextAlignment = False
        Me.CURRENCYID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        Me.CURRENCYID.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel15
        '
        Me.XrLabel15.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel15.Dpi = 254.0!
        Me.XrLabel15.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(337.9985!, 311.6299!)
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
        'XrPictureBox9
        '
        Me.XrPictureBox9.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox9.Dpi = 254.0!
        Me.XrPictureBox9.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox9.ImageSource"))
        Me.XrPictureBox9.LocationFloat = New DevExpress.Utils.PointFloat(454.7168!, 324.8591!)
        Me.XrPictureBox9.Name = "XrPictureBox9"
        Me.XrPictureBox9.SizeF = New System.Drawing.SizeF(60.0061!, 61.06424!)
        Me.XrPictureBox9.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox9.StylePriority.UseBorderColor = False
        '
        'XrLabel17
        '
        Me.XrLabel17.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel17.CanGrow = False
        Me.XrLabel17.Dpi = 254.0!
        Me.XrLabel17.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BName]")})
        Me.XrLabel17.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel17.LocationFloat = New DevExpress.Utils.PointFloat(514.723!, 311.63!)
        Me.XrLabel17.Multiline = True
        Me.XrLabel17.Name = "XrLabel17"
        Me.XrLabel17.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel17.SizeF = New System.Drawing.SizeF(578.0927!, 90.16852!)
        Me.XrLabel17.StylePriority.UseBorderColor = False
        Me.XrLabel17.StylePriority.UseBorders = False
        Me.XrLabel17.StylePriority.UseFont = False
        Me.XrLabel17.StylePriority.UseTextAlignment = False
        Me.XrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox10
        '
        Me.XrPictureBox10.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox10.Dpi = 254.0!
        Me.XrPictureBox10.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox10.ImageSource"))
        Me.XrPictureBox10.LocationFloat = New DevExpress.Utils.PointFloat(1098.107!, 324.8591!)
        Me.XrPictureBox10.Name = "XrPictureBox10"
        Me.XrPictureBox10.SizeF = New System.Drawing.SizeF(60.0061!, 61.06421!)
        Me.XrPictureBox10.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox10.StylePriority.UseBorderColor = False
        '
        'XrLabel25
        '
        Me.XrLabel25.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel25.CanGrow = False
        Me.XrLabel25.Dpi = 254.0!
        Me.XrLabel25.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel25.LocationFloat = New DevExpress.Utils.PointFloat(45.25154!, 725.8685!)
        Me.XrLabel25.Multiline = True
        Me.XrLabel25.Name = "XrLabel25"
        Me.XrLabel25.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel25.SizeF = New System.Drawing.SizeF(907.4535!, 90.16858!)
        Me.XrLabel25.StylePriority.UseBorderColor = False
        Me.XrLabel25.StylePriority.UseBorders = False
        Me.XrLabel25.StylePriority.UseFont = False
        Me.XrLabel25.StylePriority.UseTextAlignment = False
        Me.XrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox32
        '
        Me.XrPictureBox32.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox32.Dpi = 254.0!
        Me.XrPictureBox32.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox32.ImageSource"))
        Me.XrPictureBox32.LocationFloat = New DevExpress.Utils.PointFloat(1923.774!, 589.1553!)
        Me.XrPictureBox32.Name = "XrPictureBox32"
        Me.XrPictureBox32.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox32.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox32.StylePriority.UseBorderColor = False
        '
        'XrPictureBox33
        '
        Me.XrPictureBox33.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox33.Dpi = 254.0!
        Me.XrPictureBox33.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox33.ImageSource"))
        Me.XrPictureBox33.LocationFloat = New DevExpress.Utils.PointFloat(1098.108!, 493.9065!)
        Me.XrPictureBox33.Name = "XrPictureBox33"
        Me.XrPictureBox33.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox33.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox33.StylePriority.UseBorderColor = False
        '
        'XrLabel81
        '
        Me.XrLabel81.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel81.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel81.Dpi = 254.0!
        Me.XrLabel81.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel81.LocationFloat = New DevExpress.Utils.PointFloat(1589.392!, 578.1553!)
        Me.XrLabel81.Multiline = True
        Me.XrLabel81.Name = "XrLabel81"
        Me.XrLabel81.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel81.SizeF = New System.Drawing.SizeF(334.3821!, 90.1684!)
        Me.XrLabel81.StylePriority.UseBorderColor = False
        Me.XrLabel81.StylePriority.UseBorders = False
        Me.XrLabel81.StylePriority.UseFont = False
        Me.XrLabel81.StylePriority.UseTextAlignment = False
        Me.XrLabel81.Text = ":من حساب الموظف"
        Me.XrLabel81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel82
        '
        Me.XrLabel82.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel82.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel82.BorderWidth = 1.5!
        Me.XrLabel82.Dpi = 254.0!
        Me.XrLabel82.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel82.LocationFloat = New DevExpress.Utils.PointFloat(45.29102!, 668.3238!)
        Me.XrLabel82.Multiline = True
        Me.XrLabel82.Name = "XrLabel82"
        Me.XrLabel82.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel82.SizeF = New System.Drawing.SizeF(1946.523!, 5.080017!)
        Me.XrLabel82.StylePriority.UseBorderColor = False
        Me.XrLabel82.StylePriority.UseBorders = False
        Me.XrLabel82.StylePriority.UseBorderWidth = False
        Me.XrLabel82.StylePriority.UseFont = False
        Me.XrLabel82.StylePriority.UseTextAlignment = False
        Me.XrLabel82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel83
        '
        Me.XrLabel83.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel83.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel83.CanGrow = False
        Me.XrLabel83.Dpi = 254.0!
        Me.XrLabel83.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EMPID]")})
        Me.XrLabel83.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel83.LocationFloat = New DevExpress.Utils.PointFloat(45.25152!, 578.1553!)
        Me.XrLabel83.Multiline = True
        Me.XrLabel83.Name = "XrLabel83"
        Me.XrLabel83.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel83.SizeF = New System.Drawing.SizeF(1541.494!, 90.16846!)
        Me.XrLabel83.StylePriority.UseBorderColor = False
        Me.XrLabel83.StylePriority.UseBorders = False
        Me.XrLabel83.StylePriority.UseFont = False
        Me.XrLabel83.StylePriority.UseTextAlignment = False
        Me.XrLabel83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrLabel84
        '
        Me.XrLabel84.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel84.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel84.Dpi = 254.0!
        Me.XrLabel84.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel84.LocationFloat = New DevExpress.Utils.PointFloat(1716.8!, 1071.93!)
        Me.XrLabel84.Multiline = True
        Me.XrLabel84.Name = "XrLabel84"
        Me.XrLabel84.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel84.SizeF = New System.Drawing.SizeF(202.9449!, 90.16846!)
        Me.XrLabel84.StylePriority.UseBorderColor = False
        Me.XrLabel84.StylePriority.UseBorders = False
        Me.XrLabel84.StylePriority.UseFont = False
        Me.XrLabel84.StylePriority.UseTextAlignment = False
        Me.XrLabel84.Text = ":الملاحظات"
        Me.XrLabel84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel85
        '
        Me.XrLabel85.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel85.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel85.BorderWidth = 1.5!
        Me.XrLabel85.Dpi = 254.0!
        Me.XrLabel85.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel85.LocationFloat = New DevExpress.Utils.PointFloat(45.29102!, 1169.156!)
        Me.XrLabel85.Multiline = True
        Me.XrLabel85.Name = "XrLabel85"
        Me.XrLabel85.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel85.SizeF = New System.Drawing.SizeF(1946.523!, 5.080017!)
        Me.XrLabel85.StylePriority.UseBorderColor = False
        Me.XrLabel85.StylePriority.UseBorders = False
        Me.XrLabel85.StylePriority.UseBorderWidth = False
        Me.XrLabel85.StylePriority.UseFont = False
        Me.XrLabel85.StylePriority.UseTextAlignment = False
        Me.XrLabel85.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel86
        '
        Me.XrLabel86.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel86.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel86.Dpi = 254.0!
        Me.XrLabel86.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Notes]")})
        Me.XrLabel86.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel86.LocationFloat = New DevExpress.Utils.PointFloat(45.25152!, 1071.93!)
        Me.XrLabel86.Multiline = True
        Me.XrLabel86.Name = "XrLabel86"
        Me.XrLabel86.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel86.SizeF = New System.Drawing.SizeF(1669.736!, 90.16833!)
        Me.XrLabel86.StylePriority.UseBorderColor = False
        Me.XrLabel86.StylePriority.UseBorders = False
        Me.XrLabel86.StylePriority.UseFont = False
        Me.XrLabel86.StylePriority.UseTextAlignment = False
        Me.XrLabel86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox34
        '
        Me.XrPictureBox34.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox34.Dpi = 254.0!
        Me.XrPictureBox34.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox34.ImageSource"))
        Me.XrPictureBox34.LocationFloat = New DevExpress.Utils.PointFloat(1919.745!, 1084.95!)
        Me.XrPictureBox34.Name = "XrPictureBox34"
        Me.XrPictureBox34.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox34.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox34.StylePriority.UseBorderColor = False
        '
        'XrLabel87
        '
        Me.XrLabel87.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel87.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel87.Dpi = 254.0!
        Me.XrLabel87.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel87.LocationFloat = New DevExpress.Utils.PointFloat(781.8215!, 482.9065!)
        Me.XrLabel87.Multiline = True
        Me.XrLabel87.Name = "XrLabel87"
        Me.XrLabel87.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel87.SizeF = New System.Drawing.SizeF(316.2858!, 90.16849!)
        Me.XrLabel87.StylePriority.UseBorderColor = False
        Me.XrLabel87.StylePriority.UseBorders = False
        Me.XrLabel87.StylePriority.UseFont = False
        Me.XrLabel87.StylePriority.UseTextAlignment = False
        Me.XrLabel87.Text = ":تم الصرف نقدا من "
        Me.XrLabel87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel88
        '
        Me.XrLabel88.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel88.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel88.Dpi = 254.0!
        Me.XrLabel88.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AccName]")})
        Me.XrLabel88.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel88.LocationFloat = New DevExpress.Utils.PointFloat(45.291!, 482.9066!)
        Me.XrLabel88.Multiline = True
        Me.XrLabel88.Name = "XrLabel88"
        Me.XrLabel88.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel88.SizeF = New System.Drawing.SizeF(731.445!, 90.16852!)
        Me.XrLabel88.StylePriority.UseBorderColor = False
        Me.XrLabel88.StylePriority.UseBorders = False
        Me.XrLabel88.StylePriority.UseFont = False
        Me.XrLabel88.StylePriority.UseTextAlignment = False
        Me.XrLabel88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight
        '
        'XrPictureBox35
        '
        Me.XrPictureBox35.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox35.Dpi = 254.0!
        Me.XrPictureBox35.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox35.ImageSource"))
        Me.XrPictureBox35.LocationFloat = New DevExpress.Utils.PointFloat(1098.068!, 738.2032!)
        Me.XrPictureBox35.Name = "XrPictureBox35"
        Me.XrPictureBox35.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox35.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox35.StylePriority.UseBorderColor = False
        '
        'XrPictureBox36
        '
        Me.XrPictureBox36.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox36.Dpi = 254.0!
        Me.XrPictureBox36.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox36.ImageSource"))
        Me.XrPictureBox36.LocationFloat = New DevExpress.Utils.PointFloat(1923.775!, 738.2032!)
        Me.XrPictureBox36.Name = "XrPictureBox36"
        Me.XrPictureBox36.SizeF = New System.Drawing.SizeF(60.0061!, 61.06427!)
        Me.XrPictureBox36.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox36.StylePriority.UseBorderColor = False
        '
        'XrLabel91
        '
        Me.XrLabel91.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel91.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel91.Dpi = 254.0!
        Me.XrLabel91.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel91.LocationFloat = New DevExpress.Utils.PointFloat(952.705!, 725.8682!)
        Me.XrLabel91.Multiline = True
        Me.XrLabel91.Name = "XrLabel91"
        Me.XrLabel91.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel91.SizeF = New System.Drawing.SizeF(145.3628!, 90.16803!)
        Me.XrLabel91.StylePriority.UseBorderColor = False
        Me.XrLabel91.StylePriority.UseBorders = False
        Me.XrLabel91.StylePriority.UseFont = False
        Me.XrLabel91.StylePriority.UseTextAlignment = False
        Me.XrLabel91.Text = ":بالحروف"
        Me.XrLabel91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel93
        '
        Me.XrLabel93.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel93.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel93.Dpi = 254.0!
        Me.XrLabel93.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel93.LocationFloat = New DevExpress.Utils.PointFloat(1158.074!, 725.8682!)
        Me.XrLabel93.Multiline = True
        Me.XrLabel93.Name = "XrLabel93"
        Me.XrLabel93.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel93.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel93.SizeF = New System.Drawing.SizeF(510.7416!, 90.16833!)
        Me.XrLabel93.StylePriority.UseBorderColor = False
        Me.XrLabel93.StylePriority.UseBorders = False
        Me.XrLabel93.StylePriority.UseFont = False
        Me.XrLabel93.StylePriority.UseTextAlignment = False
        Me.XrLabel93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        Me.XrLabel93.TextFormatString = "{0:N0}"
        '
        'XrLabel94
        '
        Me.XrLabel94.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel94.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel94.Dpi = 254.0!
        Me.XrLabel94.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel94.LocationFloat = New DevExpress.Utils.PointFloat(1672.844!, 725.8682!)
        Me.XrLabel94.Multiline = True
        Me.XrLabel94.Name = "XrLabel94"
        Me.XrLabel94.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel94.SizeF = New System.Drawing.SizeF(250.9304!, 90.16858!)
        Me.XrLabel94.StylePriority.UseBorderColor = False
        Me.XrLabel94.StylePriority.UseBorders = False
        Me.XrLabel94.StylePriority.UseFont = False
        Me.XrLabel94.StylePriority.UseTextAlignment = False
        Me.XrLabel94.Text = ":مـبـلــغ وقــدره"
        Me.XrLabel94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel95
        '
        Me.XrLabel95.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel95.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel95.BorderWidth = 1.5!
        Me.XrLabel95.Dpi = 254.0!
        Me.XrLabel95.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel95.LocationFloat = New DevExpress.Utils.PointFloat(46.291!, 816.0371!)
        Me.XrLabel95.Multiline = True
        Me.XrLabel95.Name = "XrLabel95"
        Me.XrLabel95.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel95.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel95.StylePriority.UseBorderColor = False
        Me.XrLabel95.StylePriority.UseBorders = False
        Me.XrLabel95.StylePriority.UseBorderWidth = False
        Me.XrLabel95.StylePriority.UseFont = False
        Me.XrLabel95.StylePriority.UseTextAlignment = False
        Me.XrLabel95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel96
        '
        Me.XrLabel96.BorderColor = System.Drawing.Color.WhiteSmoke
        Me.XrLabel96.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel96.BorderWidth = 1.5!
        Me.XrLabel96.Dpi = 254.0!
        Me.XrLabel96.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel96.LocationFloat = New DevExpress.Utils.PointFloat(46.33057!, 573.0753!)
        Me.XrLabel96.Multiline = True
        Me.XrLabel96.Name = "XrLabel96"
        Me.XrLabel96.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel96.SizeF = New System.Drawing.SizeF(1945.483!, 5.080017!)
        Me.XrLabel96.StylePriority.UseBorderColor = False
        Me.XrLabel96.StylePriority.UseBorders = False
        Me.XrLabel96.StylePriority.UseBorderWidth = False
        Me.XrLabel96.StylePriority.UseFont = False
        Me.XrLabel96.StylePriority.UseTextAlignment = False
        Me.XrLabel96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrPictureBox2
        '
        Me.XrPictureBox2.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox2.Dpi = 254.0!
        Me.XrPictureBox2.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource(Global.ExchangeSystem.My.Resources.Resources.cancel, True)
        Me.XrPictureBox2.LocationFloat = New DevExpress.Utils.PointFloat(893.5372!, 7.0!)
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
        Me.XrLabel21.LocationFloat = New DevExpress.Utils.PointFloat(1281.72!, 3.999998!)
        Me.XrLabel21.Multiline = True
        Me.XrLabel21.Name = "XrLabel21"
        Me.XrLabel21.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel21.SizeF = New System.Drawing.SizeF(752.2797!, 190.92!)
        Me.XrLabel21.StylePriority.UseBorderColor = False
        Me.XrLabel21.StylePriority.UseBorders = False
        Me.XrLabel21.StylePriority.UseFont = False
        Me.XrLabel21.StylePriority.UseTextAlignment = False
        Me.XrLabel21.Text = "شركة الرحالة "
        Me.XrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel5
        '
        Me.XrLabel5.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel5.Dpi = 254.0!
        Me.XrLabel5.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel5.LocationFloat = New DevExpress.Utils.PointFloat(0!, 104.7517!)
        Me.XrLabel5.Multiline = True
        Me.XrLabel5.Name = "XrLabel5"
        Me.XrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel5.SizeF = New System.Drawing.SizeF(837.4868!, 90.16837!)
        Me.XrLabel5.StylePriority.UseBorderColor = False
        Me.XrLabel5.StylePriority.UseBorders = False
        Me.XrLabel5.StylePriority.UseFont = False
        Me.XrLabel5.StylePriority.UseTextAlignment = False
        Me.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel7
        '
        Me.XrLabel7.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel7.Dpi = 254.0!
        Me.XrLabel7.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(0!, 3.999998!)
        Me.XrLabel7.Multiline = True
        Me.XrLabel7.Name = "XrLabel7"
        Me.XrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel7.SizeF = New System.Drawing.SizeF(837.4868!, 100.7517!)
        Me.XrLabel7.StylePriority.UseBorderColor = False
        Me.XrLabel7.StylePriority.UseBorders = False
        Me.XrLabel7.StylePriority.UseFont = False
        Me.XrLabel7.StylePriority.UseTextAlignment = False
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
        Me.XrShape2.SizeF = New System.Drawing.SizeF(2001.208!, 132.8708!)
        Me.XrShape2.StylePriority.UseForeColor = False
        '
        'XrShape1
        '
        Me.XrShape1.Dpi = 254.0!
        Me.XrShape1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.XrShape1.LineWidth = 3.0!
        Me.XrShape1.LocationFloat = New DevExpress.Utils.PointFloat(17.99997!, 458.7!)
        Me.XrShape1.Name = "XrShape1"
        ShapeRectangle2.Fillet = 20
        Me.XrShape1.Shape = ShapeRectangle2
        Me.XrShape1.SizeF = New System.Drawing.SizeF(2001.208!, 238.7041!)
        Me.XrShape1.StylePriority.UseForeColor = False
        '
        'XrShape3
        '
        Me.XrShape3.Dpi = 254.0!
        Me.XrShape3.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.XrShape3.LineWidth = 3.0!
        Me.XrShape3.LocationFloat = New DevExpress.Utils.PointFloat(13.79204!, 712.384!)
        Me.XrShape3.Name = "XrShape3"
        ShapeRectangle3.Fillet = 20
        Me.XrShape3.Shape = ShapeRectangle3
        Me.XrShape3.SizeF = New System.Drawing.SizeF(2001.208!, 352.5463!)
        Me.XrShape3.StylePriority.UseForeColor = False
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
        Me.SqlDataSource1.ConnectionName = "localhost_EXCHANGESYS_Connection 13"
        Me.SqlDataSource1.Name = "SqlDataSource1"
        ColumnExpression1.ColumnName = "ID"
        Table1.Name = "EMPORCUSTWITHDRAWALTB"
        ColumnExpression1.Table = Table1
        Column1.Expression = ColumnExpression1
        ColumnExpression2.ColumnName = "Code"
        ColumnExpression2.Table = Table1
        Column2.Expression = ColumnExpression2
        ColumnExpression3.ColumnName = "InsertDate"
        ColumnExpression3.Table = Table1
        Column3.Expression = ColumnExpression3
        ColumnExpression4.ColumnName = "EMPID"
        ColumnExpression4.Table = Table1
        Column4.Expression = ColumnExpression4
        ColumnExpression5.ColumnName = "WDVAL"
        ColumnExpression5.Table = Table1
        Column5.Expression = ColumnExpression5
        ColumnExpression6.ColumnName = "DPSVAL"
        ColumnExpression6.Table = Table1
        Column6.Expression = ColumnExpression6
        ColumnExpression7.ColumnName = "SafeID"
        ColumnExpression7.Table = Table1
        Column7.Expression = ColumnExpression7
        ColumnExpression8.ColumnName = "IsActive"
        ColumnExpression8.Table = Table1
        Column8.Expression = ColumnExpression8
        ColumnExpression9.ColumnName = "TypeID"
        ColumnExpression9.Table = Table1
        Column9.Expression = ColumnExpression9
        ColumnExpression10.ColumnName = "CODEID"
        ColumnExpression10.Table = Table1
        Column10.Expression = ColumnExpression10
        ColumnExpression11.ColumnName = "BranchID"
        ColumnExpression11.Table = Table1
        Column11.Expression = ColumnExpression11
        ColumnExpression12.ColumnName = "Notes"
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
        SelectQuery1.Name = "EMPORCUSTWITHDRAWALTB"
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
        'RPTEMPWITHDRAWAL2
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.TopMargin, Me.BottomMargin, Me.ReportHeader, Me.Detail})
        Me.ComponentStorage.AddRange(New System.ComponentModel.IComponent() {Me.SqlDataSource1})
        Me.DataMember = "EMPORCUSTWITHDRAWALTB"
        Me.DataSource = Me.SqlDataSource1
        Me.Dpi = 254.0!
        Me.Font = New DevExpress.Drawing.DXFont("Arial", 9.75!)
        Me.Margins = New DevExpress.Drawing.DXMargins(37.0!, 29.0!, 21.17083!, 21.88174!)
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
    Friend WithEvents XrPictureBox2 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel21 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel5 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel25 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox32 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox33 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel81 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel82 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel83 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel84 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel85 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel86 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox34 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel87 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel88 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox35 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox36 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel91 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel93 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel94 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel95 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel96 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox7 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel12 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel14 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents CURRENCYID As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox9 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel17 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox10 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrShape3 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrShape2 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrShape1 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrPictureBox1 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel11 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox20 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox21 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox25 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel9 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox26 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel24 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox23 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox24 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox43 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel35 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel43 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo7 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrLabel44 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo8 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents IDNo As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents PaidFor As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox14 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox6 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox5 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel58 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel55 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel60 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel52 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents Phone As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox4 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel13 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel3 As DevExpress.XtraReports.UI.XRLabel
End Class
