<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class RptBankPortfolio
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RptBankPortfolio))
        Dim XrSummary1 As DevExpress.XtraReports.UI.XRSummary = New DevExpress.XtraReports.UI.XRSummary()
        Dim XrSummary2 As DevExpress.XtraReports.UI.XRSummary = New DevExpress.XtraReports.UI.XRSummary()
        Dim XrSummary3 As DevExpress.XtraReports.UI.XRSummary = New DevExpress.XtraReports.UI.XRSummary()
        Dim SelectQuery1 As DevExpress.DataAccess.Sql.SelectQuery = New DevExpress.DataAccess.Sql.SelectQuery()
        Dim Column1 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression1 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Table3 As DevExpress.DataAccess.Sql.Table = New DevExpress.DataAccess.Sql.Table()
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
        Dim Column13 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression13 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column14 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression14 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column15 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression15 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column16 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression16 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column17 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression17 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column18 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression18 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Column19 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression19 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim Table4 As DevExpress.DataAccess.Sql.Table = New DevExpress.DataAccess.Sql.Table()
        Dim Join1 As DevExpress.DataAccess.Sql.Join = New DevExpress.DataAccess.Sql.Join()
        Dim RelationColumnInfo1 As DevExpress.DataAccess.Sql.RelationColumnInfo = New DevExpress.DataAccess.Sql.RelationColumnInfo()
        Dim ShapeRectangle1 As DevExpress.XtraPrinting.Shape.ShapeRectangle = New DevExpress.XtraPrinting.Shape.ShapeRectangle()
        Dim XrWatermark1 As DevExpress.XtraReports.UI.XRWatermark = New DevExpress.XtraReports.UI.XRWatermark()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.XrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.TxtDate = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox10 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel21 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel22 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel16 = New DevExpress.XtraReports.UI.XRLabel()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.XrPictureBox1 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel14 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel24 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo2 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrPictureBox23 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.PrintTime = New DevExpress.XtraReports.UI.XRLabel()
        Me.PrintDate = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrPictureBox6 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel25 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo1 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.table2 = New DevExpress.XtraReports.UI.XRTable()
        Me.tableRow2 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.XrTableCell9 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell2 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell4 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell7 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell10 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell6 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.GroupHeader1 = New DevExpress.XtraReports.UI.GroupHeaderBand()
        Me.table1 = New DevExpress.XtraReports.UI.XRTable()
        Me.tableRow1 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.XrTableCell8 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell8 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell5 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell1 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell3 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell9 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.ReportFooter = New DevExpress.XtraReports.UI.ReportFooterBand()
        Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel6 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel23 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel20 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel19 = New DevExpress.XtraReports.UI.XRLabel()
        Me.ArLetters = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel12 = New DevExpress.XtraReports.UI.XRLabel()
        Me.OverallTotal = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.OverAllEmp = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel18 = New DevExpress.XtraReports.UI.XRLabel()
        Me.SqlDataSource1 = New DevExpress.DataAccess.Sql.SqlDataSource(Me.components)
        Me.ReportHeader = New DevExpress.XtraReports.UI.ReportHeaderBand()
        Me.XrLabel5 = New DevExpress.XtraReports.UI.XRLabel()
        Me.BankID = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel9 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel8 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel11 = New DevExpress.XtraReports.UI.XRLabel()
        Me.PageFooter = New DevExpress.XtraReports.UI.PageFooterBand()
        Me.XrShape2 = New DevExpress.XtraReports.UI.XRShape()
        CType(Me.table2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.table1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'TopMargin
        '
        Me.TopMargin.Dpi = 25.4!
        Me.TopMargin.HeightF = 3.704167!
        Me.TopMargin.Name = "TopMargin"
        '
        'XrLabel3
        '
        Me.XrLabel3.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel3.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel3.Dpi = 25.4!
        Me.XrLabel3.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.XrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(10.58845!, 20.09201!)
        Me.XrLabel3.Multiline = True
        Me.XrLabel3.Name = "XrLabel3"
        Me.XrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel3.SizeF = New System.Drawing.SizeF(58.65247!, 7.693918!)
        Me.XrLabel3.StylePriority.UseBackColor = False
        Me.XrLabel3.StylePriority.UseBorderColor = False
        Me.XrLabel3.StylePriority.UseBorders = False
        Me.XrLabel3.StylePriority.UseFont = False
        Me.XrLabel3.StylePriority.UseTextAlignment = False
        Me.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight
        Me.XrLabel3.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel4
        '
        Me.XrLabel4.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel4.Dpi = 25.4!
        Me.XrLabel4.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(69.24093!, 20.09201!)
        Me.XrLabel4.Multiline = True
        Me.XrLabel4.Name = "XrLabel4"
        Me.XrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel4.SizeF = New System.Drawing.SizeF(17.97441!, 8.193919!)
        Me.XrLabel4.StylePriority.UseBorderColor = False
        Me.XrLabel4.StylePriority.UseBorders = False
        Me.XrLabel4.StylePriority.UseFont = False
        Me.XrLabel4.StylePriority.UseTextAlignment = False
        Me.XrLabel4.Text = ":الإشاري"
        Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrLabel1
        '
        Me.XrLabel1.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel1.Dpi = 25.4!
        Me.XrLabel1.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(69.24093!, 11.89809!)
        Me.XrLabel1.Multiline = True
        Me.XrLabel1.Name = "XrLabel1"
        Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel1.SizeF = New System.Drawing.SizeF(17.97441!, 8.193919!)
        Me.XrLabel1.StylePriority.UseBorderColor = False
        Me.XrLabel1.StylePriority.UseBorders = False
        Me.XrLabel1.StylePriority.UseFont = False
        Me.XrLabel1.StylePriority.UseTextAlignment = False
        Me.XrLabel1.Text = ":الموافق"
        Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'TxtDate
        '
        Me.TxtDate.BackColor = System.Drawing.Color.Transparent
        Me.TxtDate.BorderColor = System.Drawing.Color.LightGray
        Me.TxtDate.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.TxtDate.Dpi = 25.4!
        Me.TxtDate.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.TxtDate.LocationFloat = New DevExpress.Utils.PointFloat(10.58845!, 11.89809!)
        Me.TxtDate.Multiline = True
        Me.TxtDate.Name = "TxtDate"
        Me.TxtDate.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.TxtDate.SizeF = New System.Drawing.SizeF(58.65247!, 7.693918!)
        Me.TxtDate.StylePriority.UseBackColor = False
        Me.TxtDate.StylePriority.UseBorderColor = False
        Me.TxtDate.StylePriority.UseBorders = False
        Me.TxtDate.StylePriority.UseFont = False
        Me.TxtDate.StylePriority.UseTextAlignment = False
        Me.TxtDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.TxtDate.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrPictureBox10
        '
        Me.XrPictureBox10.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox10.Dpi = 25.4!
        Me.XrPictureBox10.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox10.ImageSource"))
        Me.XrPictureBox10.LocationFloat = New DevExpress.Utils.PointFloat(176.6474!, 0!)
        Me.XrPictureBox10.Name = "XrPictureBox10"
        Me.XrPictureBox10.SizeF = New System.Drawing.SizeF(23.35258!, 19.192!)
        Me.XrPictureBox10.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox10.StylePriority.UseBorderColor = False
        '
        'XrLabel21
        '
        Me.XrLabel21.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel21.Dpi = 25.4!
        Me.XrLabel21.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 18.0!)
        Me.XrLabel21.LocationFloat = New DevExpress.Utils.PointFloat(10.58845!, 0.1024243!)
        Me.XrLabel21.Multiline = True
        Me.XrLabel21.Name = "XrLabel21"
        Me.XrLabel21.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel21.SizeF = New System.Drawing.SizeF(76.62688!, 11.39809!)
        Me.XrLabel21.StylePriority.UseBorderColor = False
        Me.XrLabel21.StylePriority.UseBorders = False
        Me.XrLabel21.StylePriority.UseFont = False
        Me.XrLabel21.StylePriority.UseTextAlignment = False
        Me.XrLabel21.Text = "حافظة مصرفية"
        Me.XrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel22
        '
        Me.XrLabel22.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel22.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel22.Dpi = 25.4!
        Me.XrLabel22.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel22.LocationFloat = New DevExpress.Utils.PointFloat(97.7186!, 0!)
        Me.XrLabel22.Multiline = True
        Me.XrLabel22.Name = "XrLabel22"
        Me.XrLabel22.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel22.SizeF = New System.Drawing.SizeF(0.5291666!, 22.48958!)
        Me.XrLabel22.StylePriority.UseBorderColor = False
        Me.XrLabel22.StylePriority.UseBorders = False
        Me.XrLabel22.StylePriority.UseFont = False
        Me.XrLabel22.StylePriority.UseTextAlignment = False
        Me.XrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel15
        '
        Me.XrLabel15.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel15.Dpi = 25.4!
        Me.XrLabel15.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(103.1339!, 11.07516!)
        Me.XrLabel15.Multiline = True
        Me.XrLabel15.Name = "XrLabel15"
        Me.XrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel15.SizeF = New System.Drawing.SizeF(72.84673!, 9.016837!)
        Me.XrLabel15.StylePriority.UseBorderColor = False
        Me.XrLabel15.StylePriority.UseBorders = False
        Me.XrLabel15.StylePriority.UseFont = False
        Me.XrLabel15.StylePriority.UseTextAlignment = False
        Me.XrLabel15.Text = "للصرافــة والخدمــات الماليــة"
        Me.XrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel16
        '
        Me.XrLabel16.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel16.Dpi = 25.4!
        Me.XrLabel16.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel16.LocationFloat = New DevExpress.Utils.PointFloat(103.1339!, 0!)
        Me.XrLabel16.Multiline = True
        Me.XrLabel16.Name = "XrLabel16"
        Me.XrLabel16.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel16.SizeF = New System.Drawing.SizeF(72.84673!, 10.07517!)
        Me.XrLabel16.StylePriority.UseBorderColor = False
        Me.XrLabel16.StylePriority.UseBorders = False
        Me.XrLabel16.StylePriority.UseFont = False
        Me.XrLabel16.StylePriority.UseTextAlignment = False
        Me.XrLabel16.Text = "شركة الرحالة "
        Me.XrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'BottomMargin
        '
        Me.BottomMargin.Dpi = 25.4!
        Me.BottomMargin.HeightF = 20.61115!
        Me.BottomMargin.Name = "BottomMargin"
        '
        'XrPictureBox1
        '
        Me.XrPictureBox1.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox1.Dpi = 25.4!
        Me.XrPictureBox1.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox1.ImageSource"))
        Me.XrPictureBox1.LocationFloat = New DevExpress.Utils.PointFloat(193.9938!, 4.0!)
        Me.XrPictureBox1.Name = "XrPictureBox1"
        Me.XrPictureBox1.SizeF = New System.Drawing.SizeF(7.312592!, 8.413501!)
        Me.XrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox1.StylePriority.UseBorderColor = False
        '
        'XrLabel14
        '
        Me.XrLabel14.Dpi = 25.4!
        Me.XrLabel14.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel14.LocationFloat = New DevExpress.Utils.PointFloat(172.7883!, 4.0!)
        Me.XrLabel14.Name = "XrLabel14"
        Me.XrLabel14.SizeF = New System.Drawing.SizeF(21.11214!, 6.69961!)
        Me.XrLabel14.StylePriority.UseFont = False
        Me.XrLabel14.StylePriority.UseTextAlignment = False
        Me.XrLabel14.Text = ":اسم المستخدم"
        Me.XrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrLabel24
        '
        Me.XrLabel24.Dpi = 25.4!
        Me.XrLabel24.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel24.LocationFloat = New DevExpress.Utils.PointFloat(113.0184!, 4.0!)
        Me.XrLabel24.Name = "XrLabel24"
        Me.XrLabel24.SizeF = New System.Drawing.SizeF(59.24075!, 6.699607!)
        Me.XrLabel24.StylePriority.UseFont = False
        Me.XrLabel24.StylePriority.UseTextAlignment = False
        Me.XrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        '
        'XrPageInfo2
        '
        Me.XrPageInfo2.Dpi = 25.4!
        Me.XrPageInfo2.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.25!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo2.LocationFloat = New DevExpress.Utils.PointFloat(87.70424!, 4.0!)
        Me.XrPageInfo2.Name = "XrPageInfo2"
        Me.XrPageInfo2.SizeF = New System.Drawing.SizeF(25.3141!, 6.699607!)
        Me.XrPageInfo2.StylePriority.UseFont = False
        Me.XrPageInfo2.StylePriority.UseTextAlignment = False
        Me.XrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrPageInfo2.TextFormatString = "الصفحة {0} من {1}"
        '
        'XrPictureBox23
        '
        Me.XrPictureBox23.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox23.Dpi = 25.4!
        Me.XrPictureBox23.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox23.ImageSource"))
        Me.XrPictureBox23.LocationFloat = New DevExpress.Utils.PointFloat(81.409!, 4.293102!)
        Me.XrPictureBox23.Name = "XrPictureBox23"
        Me.XrPictureBox23.SizeF = New System.Drawing.SizeF(6.000603!, 6.10643!)
        Me.XrPictureBox23.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox23.StylePriority.UseBorderColor = False
        '
        'PrintTime
        '
        Me.PrintTime.Dpi = 25.4!
        Me.PrintTime.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.PrintTime.LocationFloat = New DevExpress.Utils.PointFloat(61.57389!, 4.0!)
        Me.PrintTime.Name = "PrintTime"
        Me.PrintTime.SizeF = New System.Drawing.SizeF(18.93519!, 6.699609!)
        Me.PrintTime.StylePriority.UseFont = False
        Me.PrintTime.StylePriority.UseTextAlignment = False
        Me.PrintTime.Text = "تاريخ الطباعة"
        Me.PrintTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.PrintTime.TextFormatString = "{0:hh:mm tt}"
        '
        'PrintDate
        '
        Me.PrintDate.Dpi = 25.4!
        Me.PrintDate.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.PrintDate.LocationFloat = New DevExpress.Utils.PointFloat(44.36669!, 4.0!)
        Me.PrintDate.Name = "PrintDate"
        Me.PrintDate.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.PrintDate.SizeF = New System.Drawing.SizeF(17.20719!, 6.699607!)
        Me.PrintDate.StylePriority.UseFont = False
        Me.PrintDate.StylePriority.UseTextAlignment = False
        Me.PrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.PrintDate.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrPictureBox6
        '
        Me.XrPictureBox6.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox6.Dpi = 25.4!
        Me.XrPictureBox6.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox6.ImageSource"))
        Me.XrPictureBox6.LocationFloat = New DevExpress.Utils.PointFloat(37.2022!, 4.293102!)
        Me.XrPictureBox6.Name = "XrPictureBox6"
        Me.XrPictureBox6.SizeF = New System.Drawing.SizeF(6.000599!, 6.10643!)
        Me.XrPictureBox6.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox6.StylePriority.UseBorderColor = False
        '
        'XrLabel25
        '
        Me.XrLabel25.Dpi = 25.4!
        Me.XrLabel25.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel25.LocationFloat = New DevExpress.Utils.PointFloat(15.95384!, 4.0!)
        Me.XrLabel25.Name = "XrLabel25"
        Me.XrLabel25.SizeF = New System.Drawing.SizeF(18.74837!, 6.699607!)
        Me.XrLabel25.StylePriority.UseFont = False
        Me.XrLabel25.StylePriority.UseTextAlignment = False
        Me.XrLabel25.Text = " وقت الطباعة"
        Me.XrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel25.TextFormatString = "{0:hh:mm tt}"
        '
        'XrPageInfo1
        '
        Me.XrPageInfo1.Dpi = 25.4!
        Me.XrPageInfo1.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo1.LocationFloat = New DevExpress.Utils.PointFloat(3.010554!, 4.0!)
        Me.XrPageInfo1.Name = "XrPageInfo1"
        Me.XrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo1.SizeF = New System.Drawing.SizeF(12.94328!, 6.69961!)
        Me.XrPageInfo1.StylePriority.UseFont = False
        Me.XrPageInfo1.StylePriority.UseTextAlignment = False
        Me.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrPageInfo1.TextFormatString = "{0:hh:mm:ss}"
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.table2})
        Me.Detail.Dpi = 25.4!
        Me.Detail.HeightF = 6.342!
        Me.Detail.HierarchyPrintOptions.Indent = 5.08!
        Me.Detail.Name = "Detail"
        '
        'table2
        '
        Me.table2.Dpi = 25.4!
        Me.table2.LocationFloat = New DevExpress.Utils.PointFloat(0.000007629395!, 0!)
        Me.table2.Name = "table2"
        Me.table2.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.tableRow2})
        Me.table2.SizeF = New System.Drawing.SizeF(204.0!, 6.342!)
        '
        'tableRow2
        '
        Me.tableRow2.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.XrTableCell9, Me.XrTableCell2, Me.XrTableCell4, Me.XrTableCell7, Me.tableCell10, Me.XrTableCell6})
        Me.tableRow2.Dpi = 25.4!
        Me.tableRow2.Name = "tableRow2"
        Me.tableRow2.Weight = 11.683999633789062R
        '
        'XrTableCell9
        '
        Me.XrTableCell9.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrTableCell9.Dpi = 25.4!
        Me.XrTableCell9.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BankName_from]")})
        Me.XrTableCell9.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrTableCell9.Multiline = True
        Me.XrTableCell9.Name = "XrTableCell9"
        Me.XrTableCell9.StylePriority.UseBorders = False
        Me.XrTableCell9.StylePriority.UseFont = False
        Me.XrTableCell9.StylePriority.UseTextAlignment = False
        Me.XrTableCell9.Text = "XrTableCell9"
        Me.XrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell9.Weight = 0.37990038350901845R
        '
        'XrTableCell2
        '
        Me.XrTableCell2.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrTableCell2.Dpi = 25.4!
        Me.XrTableCell2.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[القيمة]")})
        Me.XrTableCell2.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrTableCell2.Name = "XrTableCell2"
        Me.XrTableCell2.StylePriority.UseBorders = False
        Me.XrTableCell2.StylePriority.UseFont = False
        Me.XrTableCell2.StylePriority.UseTextAlignment = False
        Me.XrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell2.Weight = 0.38789376556432237R
        '
        'XrTableCell4
        '
        Me.XrTableCell4.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrTableCell4.Dpi = 25.4!
        Me.XrTableCell4.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[رقم الحساب]")})
        Me.XrTableCell4.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrTableCell4.Name = "XrTableCell4"
        Me.XrTableCell4.StylePriority.UseBorders = False
        Me.XrTableCell4.StylePriority.UseFont = False
        Me.XrTableCell4.StylePriority.UseTextAlignment = False
        Me.XrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell4.TextFormatString = "{0:yyyy-MM-dd}"
        Me.XrTableCell4.Weight = 0.68473420474055935R
        '
        'XrTableCell7
        '
        Me.XrTableCell7.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrTableCell7.Dpi = 25.4!
        Me.XrTableCell7.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[الاسم]")})
        Me.XrTableCell7.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrTableCell7.Multiline = True
        Me.XrTableCell7.Name = "XrTableCell7"
        Me.XrTableCell7.StylePriority.UseBorders = False
        Me.XrTableCell7.StylePriority.UseFont = False
        Me.XrTableCell7.StylePriority.UseTextAlignment = False
        Me.XrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell7.Weight = 0.6614287956397793R
        '
        'tableCell10
        '
        Me.tableCell10.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.tableCell10.Dpi = 25.4!
        Me.tableCell10.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[codefromem]")})
        Me.tableCell10.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.tableCell10.Name = "tableCell10"
        Me.tableCell10.StylePriority.UseBorders = False
        Me.tableCell10.StylePriority.UseFont = False
        Me.tableCell10.StylePriority.UseTextAlignment = False
        Me.tableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell10.Weight = 0.33204934546031378R
        '
        'XrTableCell6
        '
        Me.XrTableCell6.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrTableCell6.Dpi = 25.4!
        Me.XrTableCell6.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")})
        Me.XrTableCell6.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.XrTableCell6.Name = "XrTableCell6"
        Me.XrTableCell6.StylePriority.UseBorders = False
        Me.XrTableCell6.StylePriority.UseFont = False
        Me.XrTableCell6.StylePriority.UseTextAlignment = False
        XrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report
        Me.XrTableCell6.Summary = XrSummary1
        Me.XrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        Me.XrTableCell6.Weight = 0.097766711382775956R
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.table1})
        Me.GroupHeader1.Dpi = 25.4!
        Me.GroupHeader1.HeightF = 7.112!
        Me.GroupHeader1.Name = "GroupHeader1"
        '
        'table1
        '
        Me.table1.BackColor = System.Drawing.Color.FromArgb(CType(CType(93, Byte), Integer), CType(CType(98, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.table1.BorderColor = System.Drawing.Color.White
        Me.table1.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.table1.Dpi = 25.4!
        Me.table1.ForeColor = System.Drawing.Color.White
        Me.table1.LocationFloat = New DevExpress.Utils.PointFloat(0.000003814697!, 0!)
        Me.table1.Name = "table1"
        Me.table1.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.tableRow1})
        Me.table1.SizeF = New System.Drawing.SizeF(204.0!, 7.112!)
        Me.table1.StylePriority.UseBorders = False
        Me.table1.StylePriority.UseForeColor = False
        '
        'tableRow1
        '
        Me.tableRow1.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.XrTableCell8, Me.tableCell8, Me.XrTableCell5, Me.XrTableCell1, Me.XrTableCell3, Me.tableCell9})
        Me.tableRow1.Dpi = 25.4!
        Me.tableRow1.Name = "tableRow1"
        Me.tableRow1.Weight = 1.0R
        '
        'XrTableCell8
        '
        Me.XrTableCell8.BorderColor = System.Drawing.Color.White
        Me.XrTableCell8.Dpi = 25.4!
        Me.XrTableCell8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell8.ForeColor = System.Drawing.Color.White
        Me.XrTableCell8.Multiline = True
        Me.XrTableCell8.Name = "XrTableCell8"
        Me.XrTableCell8.StylePriority.UseBorderColor = False
        Me.XrTableCell8.StylePriority.UseFont = False
        Me.XrTableCell8.StylePriority.UseForeColor = False
        Me.XrTableCell8.StylePriority.UseTextAlignment = False
        Me.XrTableCell8.Text = "فرع"
        Me.XrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell8.Weight = 0.26902823585991R
        '
        'tableCell8
        '
        Me.tableCell8.BorderColor = System.Drawing.Color.White
        Me.tableCell8.Dpi = 25.4!
        Me.tableCell8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell8.ForeColor = System.Drawing.Color.White
        Me.tableCell8.Name = "tableCell8"
        Me.tableCell8.StylePriority.UseBorderColor = False
        Me.tableCell8.StylePriority.UseFont = False
        Me.tableCell8.StylePriority.UseForeColor = False
        Me.tableCell8.StylePriority.UseTextAlignment = False
        Me.tableCell8.Text = "القيمة"
        Me.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell8.Weight = 0.27468867879271253R
        '
        'XrTableCell5
        '
        Me.XrTableCell5.BorderColor = System.Drawing.Color.White
        Me.XrTableCell5.Dpi = 25.4!
        Me.XrTableCell5.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell5.ForeColor = System.Drawing.Color.White
        Me.XrTableCell5.Multiline = True
        Me.XrTableCell5.Name = "XrTableCell5"
        Me.XrTableCell5.StylePriority.UseBorderColor = False
        Me.XrTableCell5.StylePriority.UseFont = False
        Me.XrTableCell5.StylePriority.UseForeColor = False
        Me.XrTableCell5.StylePriority.UseTextAlignment = False
        Me.XrTableCell5.Text = "رقم الحساب"
        Me.XrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell5.Weight = 0.48489778719655408R
        '
        'XrTableCell1
        '
        Me.XrTableCell1.BorderColor = System.Drawing.Color.White
        Me.XrTableCell1.Dpi = 25.4!
        Me.XrTableCell1.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell1.ForeColor = System.Drawing.Color.White
        Me.XrTableCell1.Multiline = True
        Me.XrTableCell1.Name = "XrTableCell1"
        Me.XrTableCell1.StylePriority.UseBorderColor = False
        Me.XrTableCell1.StylePriority.UseFont = False
        Me.XrTableCell1.StylePriority.UseForeColor = False
        Me.XrTableCell1.StylePriority.UseTextAlignment = False
        Me.XrTableCell1.Text = "اسم الموظف"
        Me.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell1.Weight = 0.46839397843128733R
        '
        'XrTableCell3
        '
        Me.XrTableCell3.BorderColor = System.Drawing.Color.White
        Me.XrTableCell3.Dpi = 25.4!
        Me.XrTableCell3.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell3.ForeColor = System.Drawing.Color.White
        Me.XrTableCell3.Multiline = True
        Me.XrTableCell3.Name = "XrTableCell3"
        Me.XrTableCell3.StylePriority.UseBorderColor = False
        Me.XrTableCell3.StylePriority.UseFont = False
        Me.XrTableCell3.StylePriority.UseForeColor = False
        Me.XrTableCell3.StylePriority.UseTextAlignment = False
        Me.XrTableCell3.Text = "الرقم الوظيفي"
        Me.XrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell3.Weight = 0.23514217117621578R
        '
        'tableCell9
        '
        Me.tableCell9.BorderColor = System.Drawing.Color.White
        Me.tableCell9.Dpi = 25.4!
        Me.tableCell9.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell9.ForeColor = System.Drawing.Color.White
        Me.tableCell9.Name = "tableCell9"
        Me.tableCell9.StylePriority.UseBorderColor = False
        Me.tableCell9.StylePriority.UseFont = False
        Me.tableCell9.StylePriority.UseForeColor = False
        Me.tableCell9.StylePriority.UseTextAlignment = False
        Me.tableCell9.Text = "#"
        Me.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell9.Weight = 0.069233987300073477R
        '
        'ReportFooter
        '
        Me.ReportFooter.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel2, Me.XrLabel6, Me.XrLabel23, Me.XrLabel20, Me.XrLabel19, Me.ArLetters, Me.XrLabel12, Me.OverallTotal, Me.XrLabel10, Me.OverAllEmp, Me.XrLabel18})
        Me.ReportFooter.Dpi = 25.4!
        Me.ReportFooter.HeightF = 124.3436!
        Me.ReportFooter.Name = "ReportFooter"
        '
        'XrLabel2
        '
        Me.XrLabel2.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel2.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel2.Dpi = 25.4!
        Me.XrLabel2.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(44.36669!, 36.56531!)
        Me.XrLabel2.Multiline = True
        Me.XrLabel2.Name = "XrLabel2"
        Me.XrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel2.SizeF = New System.Drawing.SizeF(8.976479!, 6.900158!)
        Me.XrLabel2.StylePriority.UseBackColor = False
        Me.XrLabel2.StylePriority.UseBorderColor = False
        Me.XrLabel2.StylePriority.UseBorders = False
        Me.XrLabel2.StylePriority.UseFont = False
        Me.XrLabel2.StylePriority.UseTextAlignment = False
        Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel2.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel6
        '
        Me.XrLabel6.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel6.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel6.Dpi = 25.4!
        Me.XrLabel6.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel6.LocationFloat = New DevExpress.Utils.PointFloat(15.95384!, 36.56531!)
        Me.XrLabel6.Multiline = True
        Me.XrLabel6.Name = "XrLabel6"
        Me.XrLabel6.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel6.SizeF = New System.Drawing.SizeF(16.2072!, 6.900158!)
        Me.XrLabel6.StylePriority.UseBackColor = False
        Me.XrLabel6.StylePriority.UseBorderColor = False
        Me.XrLabel6.StylePriority.UseBorders = False
        Me.XrLabel6.StylePriority.UseFont = False
        Me.XrLabel6.StylePriority.UseTextAlignment = False
        Me.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel6.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel23
        '
        Me.XrLabel23.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel23.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel23.Dpi = 25.4!
        Me.XrLabel23.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.XrLabel23.LocationFloat = New DevExpress.Utils.PointFloat(4.847251!, 85.42867!)
        Me.XrLabel23.Multiline = True
        Me.XrLabel23.Name = "XrLabel23"
        Me.XrLabel23.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel23.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel23.SizeF = New System.Drawing.SizeF(93.37135!, 38.19044!)
        Me.XrLabel23.StylePriority.UseBackColor = False
        Me.XrLabel23.StylePriority.UseBorderColor = False
        Me.XrLabel23.StylePriority.UseBorders = False
        Me.XrLabel23.StylePriority.UseFont = False
        Me.XrLabel23.StylePriority.UseTextAlignment = False
        Me.XrLabel23.Text = "خاص بالمصرف:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "تاريــــــــــــــــــــــــــــخ التسليم: ..../..../ ............." &
    "......" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "اسم الموظف: .........................................." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "الختم والتوقيع:." &
    "......................................"
        Me.XrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel23.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel20
        '
        Me.XrLabel20.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel20.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel20.Dpi = 25.4!
        Me.XrLabel20.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.XrLabel20.LocationFloat = New DevExpress.Utils.PointFloat(4.847251!, 47.23823!)
        Me.XrLabel20.Multiline = True
        Me.XrLabel20.Name = "XrLabel20"
        Me.XrLabel20.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel20.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel20.SizeF = New System.Drawing.SizeF(93.37135!, 38.19043!)
        Me.XrLabel20.StylePriority.UseBackColor = False
        Me.XrLabel20.StylePriority.UseBorderColor = False
        Me.XrLabel20.StylePriority.UseBorders = False
        Me.XrLabel20.StylePriority.UseFont = False
        Me.XrLabel20.StylePriority.UseTextAlignment = False
        Me.XrLabel20.Text = "اعتماد:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "الاسم:................................................" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "الصفة:.........." &
    "......................................" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "التوقيع:................................" &
    "..............."
        Me.XrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel20.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel19
        '
        Me.XrLabel19.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel19.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel19.Dpi = 25.4!
        Me.XrLabel19.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.XrLabel19.LocationFloat = New DevExpress.Utils.PointFloat(102.2296!, 47.23823!)
        Me.XrLabel19.Multiline = True
        Me.XrLabel19.Name = "XrLabel19"
        Me.XrLabel19.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel19.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel19.SizeF = New System.Drawing.SizeF(93.37135!, 38.19043!)
        Me.XrLabel19.StylePriority.UseBackColor = False
        Me.XrLabel19.StylePriority.UseBorderColor = False
        Me.XrLabel19.StylePriority.UseBorders = False
        Me.XrLabel19.StylePriority.UseFont = False
        Me.XrLabel19.StylePriority.UseTextAlignment = False
        Me.XrLabel19.Text = ":إعداد" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "الاسم:................................................" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "الصفة:..........." &
    "....................................." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "التوقيع:................................." &
    ".............."
        Me.XrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel19.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'ArLetters
        '
        Me.ArLetters.BackColor = System.Drawing.Color.Transparent
        Me.ArLetters.BorderColor = System.Drawing.Color.LightGray
        Me.ArLetters.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.ArLetters.Dpi = 25.4!
        Me.ArLetters.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.ArLetters.LocationFloat = New DevExpress.Utils.PointFloat(0.9999995!, 16.38782!)
        Me.ArLetters.Multiline = True
        Me.ArLetters.Name = "ArLetters"
        Me.ArLetters.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.ArLetters.SizeF = New System.Drawing.SizeF(93.37135!, 7.693914!)
        Me.ArLetters.StylePriority.UseBackColor = False
        Me.ArLetters.StylePriority.UseBorderColor = False
        Me.ArLetters.StylePriority.UseBorders = False
        Me.ArLetters.StylePriority.UseFont = False
        Me.ArLetters.StylePriority.UseTextAlignment = False
        Me.ArLetters.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.ArLetters.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel12
        '
        Me.XrLabel12.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel12.Dpi = 25.4!
        Me.XrLabel12.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel12.LocationFloat = New DevExpress.Utils.PointFloat(67.32539!, 8.693913!)
        Me.XrLabel12.Multiline = True
        Me.XrLabel12.Name = "XrLabel12"
        Me.XrLabel12.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel12.SizeF = New System.Drawing.SizeF(27.04595!, 7.693908!)
        Me.XrLabel12.StylePriority.UseBorderColor = False
        Me.XrLabel12.StylePriority.UseBorders = False
        Me.XrLabel12.StylePriority.UseFont = False
        Me.XrLabel12.StylePriority.UseTextAlignment = False
        Me.XrLabel12.Text = ":إجمالي القيمة"
        Me.XrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'OverallTotal
        '
        Me.OverallTotal.BackColor = System.Drawing.Color.Transparent
        Me.OverallTotal.BorderColor = System.Drawing.Color.LightGray
        Me.OverallTotal.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.OverallTotal.Dpi = 25.4!
        Me.OverallTotal.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumsum([القيمة])")})
        Me.OverallTotal.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.OverallTotal.LocationFloat = New DevExpress.Utils.PointFloat(1.000002!, 8.693913!)
        Me.OverallTotal.Multiline = True
        Me.OverallTotal.Name = "OverallTotal"
        Me.OverallTotal.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.OverallTotal.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.OverallTotal.SizeF = New System.Drawing.SizeF(66.32539!, 7.693913!)
        Me.OverallTotal.StylePriority.UseBackColor = False
        Me.OverallTotal.StylePriority.UseBorderColor = False
        Me.OverallTotal.StylePriority.UseBorders = False
        Me.OverallTotal.StylePriority.UseFont = False
        Me.OverallTotal.StylePriority.UseTextAlignment = False
        XrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report
        Me.OverallTotal.Summary = XrSummary2
        Me.OverallTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.OverallTotal.TextFormatString = "{0:N3} د.ل"
        '
        'XrLabel10
        '
        Me.XrLabel10.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel10.Dpi = 25.4!
        Me.XrLabel10.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(67.32539!, 1.000004!)
        Me.XrLabel10.Multiline = True
        Me.XrLabel10.Name = "XrLabel10"
        Me.XrLabel10.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel10.SizeF = New System.Drawing.SizeF(27.04595!, 7.693908!)
        Me.XrLabel10.StylePriority.UseBorderColor = False
        Me.XrLabel10.StylePriority.UseBorders = False
        Me.XrLabel10.StylePriority.UseFont = False
        Me.XrLabel10.StylePriority.UseTextAlignment = False
        Me.XrLabel10.Text = ":إجمالي الموظفين"
        Me.XrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'OverAllEmp
        '
        Me.OverAllEmp.BackColor = System.Drawing.Color.Transparent
        Me.OverAllEmp.BorderColor = System.Drawing.Color.LightGray
        Me.OverAllEmp.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.OverAllEmp.Dpi = 25.4!
        Me.OverAllEmp.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumCount([الاسم])")})
        Me.OverAllEmp.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.OverAllEmp.LocationFloat = New DevExpress.Utils.PointFloat(1.0!, 1.0!)
        Me.OverAllEmp.Multiline = True
        Me.OverAllEmp.Name = "OverAllEmp"
        Me.OverAllEmp.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.OverAllEmp.SizeF = New System.Drawing.SizeF(66.32539!, 7.693913!)
        Me.OverAllEmp.StylePriority.UseBackColor = False
        Me.OverAllEmp.StylePriority.UseBorderColor = False
        Me.OverAllEmp.StylePriority.UseBorders = False
        Me.OverAllEmp.StylePriority.UseFont = False
        Me.OverAllEmp.StylePriority.UseTextAlignment = False
        XrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report
        Me.OverAllEmp.Summary = XrSummary3
        Me.OverAllEmp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        '
        'XrLabel18
        '
        Me.XrLabel18.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel18.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel18.Dpi = 25.4!
        Me.XrLabel18.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 12.0!)
        Me.XrLabel18.LocationFloat = New DevExpress.Utils.PointFloat(2.539999!, 25.39448!)
        Me.XrLabel18.Multiline = True
        Me.XrLabel18.Name = "XrLabel18"
        Me.XrLabel18.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel18.SizeF = New System.Drawing.SizeF(198.92!, 19.071!)
        Me.XrLabel18.StylePriority.UseBackColor = False
        Me.XrLabel18.StylePriority.UseBorderColor = False
        Me.XrLabel18.StylePriority.UseBorders = False
        Me.XrLabel18.StylePriority.UseFont = False
        Me.XrLabel18.StylePriority.UseTextAlignment = False
        Me.XrLabel18.Text = resources.GetString("XrLabel18.Text")
        Me.XrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight
        Me.XrLabel18.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'SqlDataSource1
        '
        Me.SqlDataSource1.ConnectionName = "localhost_EXCHANGESYS_Connection"
        Me.SqlDataSource1.Name = "SqlDataSource1"
        ColumnExpression1.ColumnName = "ID"
        Table3.MetaSerializable = "<Meta X=""30"" Y=""30"" Width=""125"" Height=""429"" />"
        Table3.Name = "VW_SALARYCALCLAST"
        ColumnExpression1.Table = Table3
        Column1.Expression = ColumnExpression1
        ColumnExpression2.ColumnName = "AccID"
        ColumnExpression2.Table = Table3
        Column2.Expression = ColumnExpression2
        ColumnExpression3.ColumnName = "اسم الموظف"
        ColumnExpression3.Table = Table3
        Column3.Expression = ColumnExpression3
        ColumnExpression4.ColumnName = "الفرع"
        ColumnExpression4.Table = Table3
        Column4.Expression = ColumnExpression4
        ColumnExpression5.ColumnName = "الراتب الأساسي"
        ColumnExpression5.Table = Table3
        Column5.Expression = ColumnExpression5
        ColumnExpression6.ColumnName = "علاوات ثابتة"
        ColumnExpression6.Table = Table3
        Column6.Expression = ColumnExpression6
        ColumnExpression7.ColumnName = "علاوات أخرى"
        ColumnExpression7.Table = Table3
        Column7.Expression = ColumnExpression7
        ColumnExpression8.ColumnName = "خصميات متنوعة"
        ColumnExpression8.Table = Table3
        Column8.Expression = ColumnExpression8
        ColumnExpression9.ColumnName = "خصم إجازة"
        ColumnExpression9.Table = Table3
        Column9.Expression = ColumnExpression9
        ColumnExpression10.ColumnName = "خصم السلفة"
        ColumnExpression10.Table = Table3
        Column10.Expression = ColumnExpression10
        ColumnExpression11.ColumnName = "باقي السلفة"
        ColumnExpression11.Table = Table3
        Column11.Expression = ColumnExpression11
        ColumnExpression12.ColumnName = "الصافي"
        ColumnExpression12.Table = Table3
        Column12.Expression = ColumnExpression12
        ColumnExpression13.ColumnName = "BranchID"
        ColumnExpression13.Table = Table3
        Column13.Expression = ColumnExpression13
        ColumnExpression14.ColumnName = "رقم هاتف الموظف"
        ColumnExpression14.Table = Table3
        Column14.Expression = ColumnExpression14
        ColumnExpression15.ColumnName = "الرقمي الوظيفي"
        ColumnExpression15.Table = Table3
        Column15.Expression = ColumnExpression15
        ColumnExpression16.ColumnName = "EMPDATE"
        ColumnExpression16.Table = Table3
        Column16.Expression = ColumnExpression16
        ColumnExpression17.ColumnName = "BankSalaryCalc"
        ColumnExpression17.Table = Table3
        Column17.Expression = ColumnExpression17
        ColumnExpression18.ColumnName = "ISACTIVE"
        ColumnExpression18.Table = Table3
        Column18.Expression = ColumnExpression18
        ColumnExpression19.ColumnName = "EMPNAME"
        Table4.MetaSerializable = "<Meta X=""185"" Y=""30"" Width=""125"" Height=""789"" />"
        Table4.Name = "EmployeeTb"
        ColumnExpression19.Table = Table4
        Column19.Expression = ColumnExpression19
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
        SelectQuery1.Columns.Add(Column13)
        SelectQuery1.Columns.Add(Column14)
        SelectQuery1.Columns.Add(Column15)
        SelectQuery1.Columns.Add(Column16)
        SelectQuery1.Columns.Add(Column17)
        SelectQuery1.Columns.Add(Column18)
        SelectQuery1.Columns.Add(Column19)
        SelectQuery1.Name = "VW_SALARYCALCLAST"
        RelationColumnInfo1.NestedKeyColumn = "ID"
        RelationColumnInfo1.ParentKeyColumn = "ID"
        Join1.KeyColumns.Add(RelationColumnInfo1)
        Join1.Nested = Table4
        Join1.Parent = Table3
        SelectQuery1.Relations.Add(Join1)
        SelectQuery1.Tables.Add(Table3)
        SelectQuery1.Tables.Add(Table4)
        Me.SqlDataSource1.Queries.AddRange(New DevExpress.DataAccess.Sql.SqlQuery() {SelectQuery1})
        Me.SqlDataSource1.ResultSchemaSerializable = resources.GetString("SqlDataSource1.ResultSchemaSerializable")
        '
        'ReportHeader
        '
        Me.ReportHeader.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel5, Me.BankID, Me.XrLabel9, Me.XrLabel7, Me.XrLabel8, Me.XrLabel11, Me.XrLabel3, Me.XrLabel4, Me.XrLabel1, Me.TxtDate, Me.XrLabel16, Me.XrLabel21, Me.XrLabel22, Me.XrLabel15, Me.XrPictureBox10})
        Me.ReportHeader.Dpi = 25.4!
        Me.ReportHeader.HeightF = 41.27501!
        Me.ReportHeader.Name = "ReportHeader"
        '
        'XrLabel5
        '
        Me.XrLabel5.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel5.Dpi = 25.4!
        Me.XrLabel5.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel5.LocationFloat = New DevExpress.Utils.PointFloat(178.6474!, 31.0411!)
        Me.XrLabel5.Multiline = True
        Me.XrLabel5.Name = "XrLabel5"
        Me.XrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel5.SizeF = New System.Drawing.SizeF(22.81262!, 7.693909!)
        Me.XrLabel5.StylePriority.UseBorderColor = False
        Me.XrLabel5.StylePriority.UseBorders = False
        Me.XrLabel5.StylePriority.UseFont = False
        Me.XrLabel5.StylePriority.UseTextAlignment = False
        Me.XrLabel5.Text = ":السادة مصرف"
        Me.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'BankID
        '
        Me.BankID.BackColor = System.Drawing.Color.Transparent
        Me.BankID.BorderColor = System.Drawing.Color.LightGray
        Me.BankID.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.BankID.Dpi = 25.4!
        Me.BankID.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.BankID.LocationFloat = New DevExpress.Utils.PointFloat(143.5428!, 31.0411!)
        Me.BankID.Multiline = True
        Me.BankID.Name = "BankID"
        Me.BankID.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.BankID.SizeF = New System.Drawing.SizeF(35.10455!, 7.693913!)
        Me.BankID.StylePriority.UseBackColor = False
        Me.BankID.StylePriority.UseBorderColor = False
        Me.BankID.StylePriority.UseBorders = False
        Me.BankID.StylePriority.UseFont = False
        Me.BankID.StylePriority.UseTextAlignment = False
        Me.BankID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.BankID.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel9
        '
        Me.XrLabel9.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel9.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel9.Dpi = 25.4!
        Me.XrLabel9.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel9.LocationFloat = New DevExpress.Utils.PointFloat(37.2022!, 31.0411!)
        Me.XrLabel9.Multiline = True
        Me.XrLabel9.Name = "XrLabel9"
        Me.XrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel9.SizeF = New System.Drawing.SizeF(36.88075!, 7.693913!)
        Me.XrLabel9.StylePriority.UseBackColor = False
        Me.XrLabel9.StylePriority.UseBorderColor = False
        Me.XrLabel9.StylePriority.UseBorders = False
        Me.XrLabel9.StylePriority.UseFont = False
        Me.XrLabel9.StylePriority.UseTextAlignment = False
        Me.XrLabel9.Text = "نوع العملية : تحويل مرتبات"
        Me.XrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel9.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel7
        '
        Me.XrLabel7.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel7.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel7.Dpi = 25.4!
        Me.XrLabel7.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(4.847251!, 31.0411!)
        Me.XrLabel7.Multiline = True
        Me.XrLabel7.Name = "XrLabel7"
        Me.XrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel7.SizeF = New System.Drawing.SizeF(31.32451!, 7.693913!)
        Me.XrLabel7.StylePriority.UseBackColor = False
        Me.XrLabel7.StylePriority.UseBorderColor = False
        Me.XrLabel7.StylePriority.UseBorders = False
        Me.XrLabel7.StylePriority.UseFont = False
        Me.XrLabel7.StylePriority.UseTextAlignment = False
        Me.XrLabel7.Text = "نوع العملة : دينار ليبي"
        Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel7.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel8
        '
        Me.XrLabel8.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel8.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel8.Dpi = 25.4!
        Me.XrLabel8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel8.LocationFloat = New DevExpress.Utils.PointFloat(113.0184!, 31.04109!)
        Me.XrLabel8.Multiline = True
        Me.XrLabel8.Name = "XrLabel8"
        Me.XrLabel8.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel8.SizeF = New System.Drawing.SizeF(30.52439!, 7.693913!)
        Me.XrLabel8.StylePriority.UseBackColor = False
        Me.XrLabel8.StylePriority.UseBorderColor = False
        Me.XrLabel8.StylePriority.UseBorders = False
        Me.XrLabel8.StylePriority.UseFont = False
        Me.XrLabel8.StylePriority.UseTextAlignment = False
        Me.XrLabel8.Text = ": حسابنا طرفكم  رقم"
        Me.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel8.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel11
        '
        Me.XrLabel11.BackColor = System.Drawing.Color.Transparent
        Me.XrLabel11.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel11.Dpi = 25.4!
        Me.XrLabel11.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 9.0!)
        Me.XrLabel11.LocationFloat = New DevExpress.Utils.PointFloat(74.08295!, 31.0411!)
        Me.XrLabel11.Multiline = True
        Me.XrLabel11.Name = "XrLabel11"
        Me.XrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(0.5!, 0.5!, 0!, 0!, 25.4!)
        Me.XrLabel11.SizeF = New System.Drawing.SizeF(38.93539!, 7.693913!)
        Me.XrLabel11.StylePriority.UseBackColor = False
        Me.XrLabel11.StylePriority.UseBorderColor = False
        Me.XrLabel11.StylePriority.UseBorders = False
        Me.XrLabel11.StylePriority.UseFont = False
        Me.XrLabel11.StylePriority.UseTextAlignment = False
        Me.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel11.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'PageFooter
        '
        Me.PageFooter.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrPictureBox1, Me.XrPageInfo1, Me.XrLabel24, Me.XrPageInfo2, Me.XrPictureBox23, Me.PrintTime, Me.PrintDate, Me.XrPictureBox6, Me.XrLabel25, Me.XrLabel14, Me.XrShape2})
        Me.PageFooter.Dpi = 25.4!
        Me.PageFooter.HeightF = 13.96417!
        Me.PageFooter.Name = "PageFooter"
        '
        'XrShape2
        '
        Me.XrShape2.Dpi = 25.4!
        Me.XrShape2.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.XrShape2.LineWidth = 3.0!
        Me.XrShape2.LocationFloat = New DevExpress.Utils.PointFloat(1.000002!, 2.000007!)
        Me.XrShape2.Name = "XrShape2"
        ShapeRectangle1.Fillet = 20
        Me.XrShape2.Shape = ShapeRectangle1
        Me.XrShape2.SizeF = New System.Drawing.SizeF(201.0!, 11.96416!)
        Me.XrShape2.StylePriority.UseForeColor = False
        '
        'RptBankPortfolio
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.TopMargin, Me.BottomMargin, Me.Detail, Me.GroupHeader1, Me.ReportFooter, Me.ReportHeader, Me.PageFooter})
        Me.ComponentStorage.AddRange(New System.ComponentModel.IComponent() {Me.SqlDataSource1})
        Me.DesignerOptions.ShowPrintingWarnings = False
        Me.Dpi = 25.4!
        Me.Font = New DevExpress.Drawing.DXFont("Arial", 9.75!)
        Me.Margins = New DevExpress.Drawing.DXMargins(4.0!, 2.0!, 3.704167!, 20.61115!)
        Me.PageHeightF = 297.0!
        Me.PageWidthF = 210.0!
        Me.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4
        Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.Millimeters
        Me.ShowPrintMarginsWarning = False
        Me.SnapGridSize = 2.5!
        Me.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.Version = "25.1"
        XrWatermark1.Id = "Watermark1"
        Me.Watermarks.AddRange(New DevExpress.XtraPrinting.Drawing.Watermark() {XrWatermark1})
        CType(Me.table2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.table1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents XrPictureBox10 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel21 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel22 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel16 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents GroupHeader1 As DevExpress.XtraReports.UI.GroupHeaderBand
    Friend WithEvents table1 As DevExpress.XtraReports.UI.XRTable
    Friend WithEvents tableRow1 As DevExpress.XtraReports.UI.XRTableRow
    Friend WithEvents tableCell8 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell5 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell3 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell9 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents TxtDate As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents ReportFooter As DevExpress.XtraReports.UI.ReportFooterBand
    Friend WithEvents XrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents OverAllEmp As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel19 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents ArLetters As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel12 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents OverallTotal As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel23 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel20 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox1 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel14 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel24 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo2 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrPictureBox23 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents PrintTime As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents PrintDate As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrPictureBox6 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel25 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo1 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents SqlDataSource1 As DevExpress.DataAccess.Sql.SqlDataSource
    Friend WithEvents table2 As DevExpress.XtraReports.UI.XRTable
    Friend WithEvents tableRow2 As DevExpress.XtraReports.UI.XRTableRow
    Friend WithEvents XrTableCell2 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell4 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell10 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell6 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents ReportHeader As DevExpress.XtraReports.UI.ReportHeaderBand
    Friend WithEvents PageFooter As DevExpress.XtraReports.UI.PageFooterBand
    Friend WithEvents XrShape2 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrTableCell7 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell1 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell9 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell8 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrLabel18 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel6 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel5 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents BankID As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel9 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel11 As DevExpress.XtraReports.UI.XRLabel
End Class
