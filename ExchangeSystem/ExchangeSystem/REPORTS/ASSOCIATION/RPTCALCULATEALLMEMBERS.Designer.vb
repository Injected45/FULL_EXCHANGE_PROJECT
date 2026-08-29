<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class RPTCALCULATEALLMEMBERS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RPTCALCULATEALLMEMBERS))
        Dim XrSummary1 As DevExpress.XtraReports.UI.XRSummary = New DevExpress.XtraReports.UI.XRSummary()
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
        Dim XrWatermark1 As DevExpress.XtraReports.UI.XRWatermark = New DevExpress.XtraReports.UI.XRWatermark()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.XrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.PrintDate = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.PrintTime = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox1 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox23 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPictureBox6 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrPageInfo2 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrPageInfo1 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.ReportHeader = New DevExpress.XtraReports.UI.ReportHeaderBand()
        Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel16 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel22 = New DevExpress.XtraReports.UI.XRLabel()
        Me.YDATE = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel18 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel14 = New DevExpress.XtraReports.UI.XRLabel()
        Me.MDATE = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox10 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.GroupHeader1 = New DevExpress.XtraReports.UI.GroupHeaderBand()
        Me.table1 = New DevExpress.XtraReports.UI.XRTable()
        Me.tableRow1 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.XrTableCell4 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell2 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell4 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.table2 = New DevExpress.XtraReports.UI.XRTable()
        Me.tableRow2 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.tableCell5 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell6 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell8 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.SqlDataSource1 = New DevExpress.DataAccess.Sql.SqlDataSource(Me.components)
        Me.Title = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailCaption1 = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailData1 = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailData3_Odd = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.PageInfo = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.ReportFooter = New DevExpress.XtraReports.UI.ReportFooterBand()
        Me.XrLabel29 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox3 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        CType(Me.table1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.table2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'TopMargin
        '
        Me.TopMargin.Dpi = 254.0!
        Me.TopMargin.HeightF = 50.27083!
        Me.TopMargin.Name = "TopMargin"
        '
        'BottomMargin
        '
        Me.BottomMargin.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel10, Me.PrintDate, Me.PrintTime, Me.XrLabel2, Me.XrPictureBox1, Me.XrPictureBox23, Me.XrPictureBox6, Me.XrPageInfo2, Me.XrPageInfo1, Me.XrLabel4})
        Me.BottomMargin.Dpi = 254.0!
        Me.BottomMargin.HeightF = 146.7108!
        Me.BottomMargin.Name = "BottomMargin"
        '
        'XrLabel10
        '
        Me.XrLabel10.Dpi = 254.0!
        Me.XrLabel10.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(143.4748!, 31.139!)
        Me.XrLabel10.Name = "XrLabel10"
        Me.XrLabel10.SizeF = New System.Drawing.SizeF(187.4837!, 66.99608!)
        Me.XrLabel10.StylePriority.UseFont = False
        Me.XrLabel10.StylePriority.UseTextAlignment = False
        Me.XrLabel10.Text = " وقت الطباعة"
        Me.XrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel10.TextFormatString = "{0:hh:mm tt}"
        '
        'PrintDate
        '
        Me.PrintDate.Dpi = 254.0!
        Me.PrintDate.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.PrintDate.LocationFloat = New DevExpress.Utils.PointFloat(427.6033!, 31.139!)
        Me.PrintDate.Name = "PrintDate"
        Me.PrintDate.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.PrintDate.SizeF = New System.Drawing.SizeF(172.0719!, 66.99608!)
        Me.PrintDate.StylePriority.UseFont = False
        Me.PrintDate.StylePriority.UseTextAlignment = False
        Me.PrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.PrintDate.TextFormatString = "{0:dd/MM/yyyy}"
        '
        'PrintTime
        '
        Me.PrintTime.Dpi = 254.0!
        Me.PrintTime.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.PrintTime.LocationFloat = New DevExpress.Utils.PointFloat(599.6753!, 31.139!)
        Me.PrintTime.Name = "PrintTime"
        Me.PrintTime.SizeF = New System.Drawing.SizeF(189.3519!, 66.99609!)
        Me.PrintTime.StylePriority.UseFont = False
        Me.PrintTime.StylePriority.UseTextAlignment = False
        Me.PrintTime.Text = "تاريخ الطباعة"
        Me.PrintTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.PrintTime.TextFormatString = "{0:hh:mm tt}"
        '
        'XrLabel2
        '
        Me.XrLabel2.Dpi = 254.0!
        Me.XrLabel2.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(1711.819!, 31.139!)
        Me.XrLabel2.Name = "XrLabel2"
        Me.XrLabel2.SizeF = New System.Drawing.SizeF(211.1213!, 66.9961!)
        Me.XrLabel2.StylePriority.UseFont = False
        Me.XrLabel2.StylePriority.UseTextAlignment = False
        Me.XrLabel2.Text = ":اسم المستخدم"
        Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox1
        '
        Me.XrPictureBox1.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox1.Dpi = 254.0!
        Me.XrPictureBox1.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox1.ImageSource"))
        Me.XrPictureBox1.LocationFloat = New DevExpress.Utils.PointFloat(1923.874!, 25.0!)
        Me.XrPictureBox1.Name = "XrPictureBox1"
        Me.XrPictureBox1.SizeF = New System.Drawing.SizeF(73.12598!, 84.13501!)
        Me.XrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox1.StylePriority.UseBorderColor = False
        '
        'XrPictureBox23
        '
        Me.XrPictureBox23.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox23.Dpi = 254.0!
        Me.XrPictureBox23.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox23.ImageSource"))
        Me.XrPictureBox23.LocationFloat = New DevExpress.Utils.PointFloat(798.0265!, 34.07003!)
        Me.XrPictureBox23.Name = "XrPictureBox23"
        Me.XrPictureBox23.SizeF = New System.Drawing.SizeF(60.00598!, 61.0643!)
        Me.XrPictureBox23.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox23.StylePriority.UseBorderColor = False
        '
        'XrPictureBox6
        '
        Me.XrPictureBox6.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox6.Dpi = 254.0!
        Me.XrPictureBox6.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("svg", resources.GetString("XrPictureBox6.ImageSource"))
        Me.XrPictureBox6.LocationFloat = New DevExpress.Utils.PointFloat(355.9585!, 34.07003!)
        Me.XrPictureBox6.Name = "XrPictureBox6"
        Me.XrPictureBox6.SizeF = New System.Drawing.SizeF(60.00598!, 61.0643!)
        Me.XrPictureBox6.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox6.StylePriority.UseBorderColor = False
        '
        'XrPageInfo2
        '
        Me.XrPageInfo2.Dpi = 254.0!
        Me.XrPageInfo2.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.25!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo2.LocationFloat = New DevExpress.Utils.PointFloat(860.9787!, 31.139!)
        Me.XrPageInfo2.Name = "XrPageInfo2"
        Me.XrPageInfo2.SizeF = New System.Drawing.SizeF(253.1411!, 66.99608!)
        Me.XrPageInfo2.StylePriority.UseFont = False
        Me.XrPageInfo2.StylePriority.UseTextAlignment = False
        Me.XrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrPageInfo2.TextFormatString = "الصفحة {0} من {1}"
        '
        'XrPageInfo1
        '
        Me.XrPageInfo1.Dpi = 254.0!
        Me.XrPageInfo1.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo1.LocationFloat = New DevExpress.Utils.PointFloat(14.04199!, 31.139!)
        Me.XrPageInfo1.Name = "XrPageInfo1"
        Me.XrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo1.SizeF = New System.Drawing.SizeF(129.4328!, 66.99611!)
        Me.XrPageInfo1.StylePriority.UseFont = False
        Me.XrPageInfo1.StylePriority.UseTextAlignment = False
        Me.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrPageInfo1.TextFormatString = "{0:hh:mm:ss}"
        '
        'XrLabel4
        '
        Me.XrLabel4.Dpi = 254.0!
        Me.XrLabel4.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(1114.12!, 31.139!)
        Me.XrLabel4.Name = "XrLabel4"
        Me.XrLabel4.SizeF = New System.Drawing.SizeF(592.4076!, 66.99608!)
        Me.XrLabel4.StylePriority.UseFont = False
        Me.XrLabel4.StylePriority.UseTextAlignment = False
        Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        '
        'ReportHeader
        '
        Me.ReportHeader.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel1, Me.XrLabel16, Me.XrLabel15, Me.XrLabel22, Me.YDATE, Me.XrLabel18, Me.XrLabel14, Me.MDATE, Me.XrLabel3, Me.XrPictureBox10})
        Me.ReportHeader.Dpi = 254.0!
        Me.ReportHeader.HeightF = 348.1918!
        Me.ReportHeader.Name = "ReportHeader"
        '
        'XrLabel1
        '
        Me.XrLabel1.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel1.Dpi = 254.0!
        Me.XrLabel1.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 18.0!)
        Me.XrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(13.00012!, 20.0!)
        Me.XrLabel1.Multiline = True
        Me.XrLabel1.Name = "XrLabel1"
        Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel1.SizeF = New System.Drawing.SizeF(994.186!, 113.9809!)
        Me.XrLabel1.StylePriority.UseBorderColor = False
        Me.XrLabel1.StylePriority.UseBorders = False
        Me.XrLabel1.StylePriority.UseFont = False
        Me.XrLabel1.StylePriority.UseTextAlignment = False
        Me.XrLabel1.Text = "احتساب إشتراكات أعضاء جمعية"
        Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel16
        '
        Me.XrLabel16.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel16.Dpi = 254.0!
        Me.XrLabel16.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel16.LocationFloat = New DevExpress.Utils.PointFloat(1056.339!, 75.91487!)
        Me.XrLabel16.Multiline = True
        Me.XrLabel16.Name = "XrLabel16"
        Me.XrLabel16.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel16.SizeF = New System.Drawing.SizeF(728.4673!, 100.7517!)
        Me.XrLabel16.StylePriority.UseBorderColor = False
        Me.XrLabel16.StylePriority.UseBorders = False
        Me.XrLabel16.StylePriority.UseFont = False
        Me.XrLabel16.StylePriority.UseTextAlignment = False
        Me.XrLabel16.Text = "شركة الرحالة "
        Me.XrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel15
        '
        Me.XrLabel15.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel15.Dpi = 254.0!
        Me.XrLabel15.Font = New DevExpress.Drawing.DXFont("Univers Next Arabic Bold", 16.0!)
        Me.XrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(1056.339!, 176.6666!)
        Me.XrLabel15.Multiline = True
        Me.XrLabel15.Name = "XrLabel15"
        Me.XrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel15.SizeF = New System.Drawing.SizeF(728.4673!, 90.16837!)
        Me.XrLabel15.StylePriority.UseBorderColor = False
        Me.XrLabel15.StylePriority.UseBorders = False
        Me.XrLabel15.StylePriority.UseFont = False
        Me.XrLabel15.StylePriority.UseTextAlignment = False
        Me.XrLabel15.Text = "للصرافــة والخدمــات الماليــة"
        Me.XrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel22
        '
        Me.XrLabel22.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel22.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel22.Dpi = 254.0!
        Me.XrLabel22.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel22.LocationFloat = New DevExpress.Utils.PointFloat(1004.186!, 75.91487!)
        Me.XrLabel22.Multiline = True
        Me.XrLabel22.Name = "XrLabel22"
        Me.XrLabel22.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel22.SizeF = New System.Drawing.SizeF(5.000122!, 224.8958!)
        Me.XrLabel22.StylePriority.UseBorderColor = False
        Me.XrLabel22.StylePriority.UseBorders = False
        Me.XrLabel22.StylePriority.UseFont = False
        Me.XrLabel22.StylePriority.UseTextAlignment = False
        Me.XrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'YDATE
        '
        Me.YDATE.BackColor = System.Drawing.Color.Transparent
        Me.YDATE.BorderColor = System.Drawing.Color.LightGray
        Me.YDATE.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.YDATE.Dpi = 254.0!
        Me.YDATE.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.YDATE.LocationFloat = New DevExpress.Utils.PointFloat(191.0944!, 223.8714!)
        Me.YDATE.Multiline = True
        Me.YDATE.Name = "YDATE"
        Me.YDATE.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.YDATE.SizeF = New System.Drawing.SizeF(209.0596!, 76.93927!)
        Me.YDATE.StylePriority.UseBackColor = False
        Me.YDATE.StylePriority.UseBorderColor = False
        Me.YDATE.StylePriority.UseBorders = False
        Me.YDATE.StylePriority.UseFont = False
        Me.YDATE.StylePriority.UseTextAlignment = False
        Me.YDATE.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight
        '
        'XrLabel18
        '
        Me.XrLabel18.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel18.Dpi = 254.0!
        Me.XrLabel18.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.XrLabel18.LocationFloat = New DevExpress.Utils.PointFloat(408.7373!, 223.8714!)
        Me.XrLabel18.Multiline = True
        Me.XrLabel18.Name = "XrLabel18"
        Me.XrLabel18.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel18.SizeF = New System.Drawing.SizeF(114.8944!, 76.93927!)
        Me.XrLabel18.StylePriority.UseBorderColor = False
        Me.XrLabel18.StylePriority.UseBorders = False
        Me.XrLabel18.StylePriority.UseFont = False
        Me.XrLabel18.StylePriority.UseTextAlignment = False
        Me.XrLabel18.Text = ":سنة"
        Me.XrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter
        '
        'XrLabel14
        '
        Me.XrLabel14.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel14.Dpi = 254.0!
        Me.XrLabel14.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.XrLabel14.LocationFloat = New DevExpress.Utils.PointFloat(654.8646!, 223.8716!)
        Me.XrLabel14.Multiline = True
        Me.XrLabel14.Name = "XrLabel14"
        Me.XrLabel14.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel14.SizeF = New System.Drawing.SizeF(111.3789!, 76.93919!)
        Me.XrLabel14.StylePriority.UseBorderColor = False
        Me.XrLabel14.StylePriority.UseBorders = False
        Me.XrLabel14.StylePriority.UseFont = False
        Me.XrLabel14.StylePriority.UseTextAlignment = False
        Me.XrLabel14.Text = ":شهر"
        Me.XrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter
        '
        'MDATE
        '
        Me.MDATE.BackColor = System.Drawing.Color.Transparent
        Me.MDATE.BorderColor = System.Drawing.Color.LightGray
        Me.MDATE.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.MDATE.Dpi = 254.0!
        Me.MDATE.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 14.0!)
        Me.MDATE.LocationFloat = New DevExpress.Utils.PointFloat(523.6317!, 223.8716!)
        Me.MDATE.Multiline = True
        Me.MDATE.Name = "MDATE"
        Me.MDATE.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.MDATE.SizeF = New System.Drawing.SizeF(129.2324!, 76.93918!)
        Me.MDATE.StylePriority.UseBackColor = False
        Me.MDATE.StylePriority.UseBorderColor = False
        Me.MDATE.StylePriority.UseBorders = False
        Me.MDATE.StylePriority.UseFont = False
        Me.MDATE.StylePriority.UseTextAlignment = False
        Me.MDATE.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight
        '
        'XrLabel3
        '
        Me.XrLabel3.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel3.Dpi = 254.0!
        Me.XrLabel3.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 18.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrLabel3.ForeColor = System.Drawing.Color.RoyalBlue
        Me.XrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(8.000117!, 110.9149!)
        Me.XrLabel3.Multiline = True
        Me.XrLabel3.Name = "XrLabel3"
        Me.XrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel3.SizeF = New System.Drawing.SizeF(994.186!, 113.9809!)
        Me.XrLabel3.StylePriority.UseBorderColor = False
        Me.XrLabel3.StylePriority.UseBorders = False
        Me.XrLabel3.StylePriority.UseFont = False
        Me.XrLabel3.StylePriority.UseForeColor = False
        Me.XrLabel3.StylePriority.UseTextAlignment = False
        Me.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrPictureBox10
        '
        Me.XrPictureBox10.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox10.Dpi = 254.0!
        Me.XrPictureBox10.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox10.ImageSource"))
        Me.XrPictureBox10.LocationFloat = New DevExpress.Utils.PointFloat(1791.474!, 75.91487!)
        Me.XrPictureBox10.Name = "XrPictureBox10"
        Me.XrPictureBox10.SizeF = New System.Drawing.SizeF(233.5259!, 191.92!)
        Me.XrPictureBox10.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox10.StylePriority.UseBorderColor = False
        '
        'GroupHeader1
        '
        Me.GroupHeader1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.table1})
        Me.GroupHeader1.Dpi = 254.0!
        Me.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail
        Me.GroupHeader1.HeightF = 71.12!
        Me.GroupHeader1.Name = "GroupHeader1"
        '
        'table1
        '
        Me.table1.Dpi = 254.0!
        Me.table1.LocationFloat = New DevExpress.Utils.PointFloat(8.000244!, 0!)
        Me.table1.Name = "table1"
        Me.table1.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.tableRow1})
        Me.table1.SizeF = New System.Drawing.SizeF(2083.0!, 71.12!)
        Me.table1.StylePriority.UseTextAlignment = False
        Me.table1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'tableRow1
        '
        Me.tableRow1.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.XrTableCell4, Me.tableCell2, Me.tableCell4})
        Me.tableRow1.Dpi = 254.0!
        Me.tableRow1.Name = "tableRow1"
        Me.tableRow1.Weight = 1.0R
        '
        'XrTableCell4
        '
        Me.XrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Left
        Me.XrTableCell4.Dpi = 254.0!
        Me.XrTableCell4.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 10.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell4.Multiline = True
        Me.XrTableCell4.Name = "XrTableCell4"
        Me.XrTableCell4.StyleName = "DetailCaption1"
        Me.XrTableCell4.StylePriority.UseBorders = False
        Me.XrTableCell4.StylePriority.UseFont = False
        Me.XrTableCell4.StylePriority.UseTextAlignment = False
        Me.XrTableCell4.Text = "قيمة الاشتراك"
        Me.XrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell4.Weight = 0.48919872132601211R
        '
        'tableCell2
        '
        Me.tableCell2.Dpi = 254.0!
        Me.tableCell2.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 10.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell2.Name = "tableCell2"
        Me.tableCell2.StyleName = "DetailCaption1"
        Me.tableCell2.StylePriority.UseFont = False
        Me.tableCell2.StylePriority.UseTextAlignment = False
        Me.tableCell2.Text = "اسم العضو"
        Me.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell2.Weight = 0.61521897499982781R
        '
        'tableCell4
        '
        Me.tableCell4.Dpi = 254.0!
        Me.tableCell4.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 10.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell4.Name = "tableCell4"
        Me.tableCell4.StyleName = "DetailCaption1"
        Me.tableCell4.StylePriority.UseFont = False
        Me.tableCell4.StylePriority.UseTextAlignment = False
        Me.tableCell4.Text = "#"
        Me.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell4.Weight = 0.12469758712697676R
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.table2})
        Me.Detail.Dpi = 254.0!
        Me.Detail.HeightF = 63.42!
        Me.Detail.HierarchyPrintOptions.Indent = 50.8!
        Me.Detail.Name = "Detail"
        '
        'table2
        '
        Me.table2.Dpi = 254.0!
        Me.table2.LocationFloat = New DevExpress.Utils.PointFloat(8.000305!, 0!)
        Me.table2.Name = "table2"
        Me.table2.OddStyleName = "DetailData3_Odd"
        Me.table2.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.tableRow2})
        Me.table2.SizeF = New System.Drawing.SizeF(2083.0!, 63.42!)
        '
        'tableRow2
        '
        Me.tableRow2.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.tableCell5, Me.tableCell6, Me.tableCell8})
        Me.tableRow2.Dpi = 254.0!
        Me.tableRow2.Name = "tableRow2"
        Me.tableRow2.Weight = 11.683999633789062R
        '
        'tableCell5
        '
        Me.tableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Left
        Me.tableCell5.Dpi = 254.0!
        Me.tableCell5.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[قيمة الاشتراك]")})
        Me.tableCell5.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.tableCell5.Name = "tableCell5"
        Me.tableCell5.StyleName = "DetailData1"
        Me.tableCell5.StylePriority.UseBorders = False
        Me.tableCell5.StylePriority.UseFont = False
        Me.tableCell5.StylePriority.UseTextAlignment = False
        Me.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell5.TextFormatString = "{0:N0}"
        Me.tableCell5.Weight = 0.48919872798918662R
        '
        'tableCell6
        '
        Me.tableCell6.Dpi = 254.0!
        Me.tableCell6.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[الاسم]")})
        Me.tableCell6.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 8.0!)
        Me.tableCell6.Name = "tableCell6"
        Me.tableCell6.StyleName = "DetailData1"
        Me.tableCell6.StylePriority.UseFont = False
        Me.tableCell6.StylePriority.UseTextAlignment = False
        Me.tableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell6.Weight = 0.61521904837211971R
        '
        'tableCell8
        '
        Me.tableCell8.Dpi = 254.0!
        Me.tableCell8.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")})
        Me.tableCell8.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 7.5!)
        Me.tableCell8.Name = "tableCell8"
        Me.tableCell8.StyleName = "DetailData1"
        Me.tableCell8.StylePriority.UseFont = False
        Me.tableCell8.StylePriority.UseTextAlignment = False
        XrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report
        Me.tableCell8.Summary = XrSummary1
        Me.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell8.Weight = 0.12469770116149986R
        '
        'SqlDataSource1
        '
        Me.SqlDataSource1.ConnectionName = "localhost_Connection"
        Me.SqlDataSource1.Name = "SqlDataSource1"
        ColumnExpression1.ColumnName = "ID"
        Table3.Name = "ASSOCIATIONNAMETB"
        ColumnExpression1.Table = Table3
        Column1.Expression = ColumnExpression1
        ColumnExpression2.ColumnName = "ASSNAME"
        ColumnExpression2.Table = Table3
        Column2.Expression = ColumnExpression2
        ColumnExpression3.ColumnName = "IsActive"
        ColumnExpression3.Table = Table3
        Column3.Expression = ColumnExpression3
        ColumnExpression4.ColumnName = "AccID"
        ColumnExpression4.Table = Table3
        Column4.Expression = ColumnExpression4
        ColumnExpression5.ColumnName = "ASSOACCID"
        ColumnExpression5.Table = Table3
        Column5.Expression = ColumnExpression5
        ColumnExpression6.ColumnName = "AssValue"
        ColumnExpression6.Table = Table3
        Column6.Expression = ColumnExpression6
        SelectQuery1.Columns.Add(Column1)
        SelectQuery1.Columns.Add(Column2)
        SelectQuery1.Columns.Add(Column3)
        SelectQuery1.Columns.Add(Column4)
        SelectQuery1.Columns.Add(Column5)
        SelectQuery1.Columns.Add(Column6)
        SelectQuery1.Name = "ASSOCIATIONNAMETB"
        SelectQuery1.Tables.Add(Table3)
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
        'ReportFooter
        '
        Me.ReportFooter.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel29, Me.XrPictureBox3, Me.XrLabel7})
        Me.ReportFooter.Dpi = 254.0!
        Me.ReportFooter.HeightF = 149.6224!
        Me.ReportFooter.Name = "ReportFooter"
        '
        'XrLabel29
        '
        Me.XrLabel29.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel29.Dpi = 254.0!
        Me.XrLabel29.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel29.LocationFloat = New DevExpress.Utils.PointFloat(1721.236!, 19.0!)
        Me.XrLabel29.Multiline = True
        Me.XrLabel29.Name = "XrLabel29"
        Me.XrLabel29.Padding = New DevExpress.XtraPrinting.PaddingInfo(5.0!, 5.0!, 0!, 0!, 254.0!)
        Me.XrLabel29.SizeF = New System.Drawing.SizeF(243.7574!, 90.16841!)
        Me.XrLabel29.StylePriority.UseBorderColor = False
        Me.XrLabel29.StylePriority.UseBorders = False
        Me.XrLabel29.StylePriority.UseFont = False
        Me.XrLabel29.StylePriority.UseTextAlignment = False
        Me.XrLabel29.Text = ":إجمالي القيمة"
        Me.XrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox3
        '
        Me.XrPictureBox3.BorderColor = System.Drawing.Color.Transparent
        Me.XrPictureBox3.Dpi = 254.0!
        Me.XrPictureBox3.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("XrPictureBox3.ImageSource"))
        Me.XrPictureBox3.LocationFloat = New DevExpress.Utils.PointFloat(1964.994!, 34.56865!)
        Me.XrPictureBox3.Name = "XrPictureBox3"
        Me.XrPictureBox3.SizeF = New System.Drawing.SizeF(60.00635!, 61.06424!)
        Me.XrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox3.StylePriority.UseBorderColor = False
        '
        'XrLabel7
        '
        Me.XrLabel7.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel7.BorderWidth = 2.0!
        Me.XrLabel7.Dpi = 254.0!
        Me.XrLabel7.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 11.0!)
        Me.XrLabel7.ForeColor = System.Drawing.Color.Black
        Me.XrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(837.0524!, 19.00037!)
        Me.XrLabel7.Name = "XrLabel7"
        Me.XrLabel7.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel7.SizeF = New System.Drawing.SizeF(884.1837!, 90.16828!)
        Me.XrLabel7.StylePriority.UseBorderColor = False
        Me.XrLabel7.StylePriority.UseBorders = False
        Me.XrLabel7.StylePriority.UseBorderWidth = False
        Me.XrLabel7.StylePriority.UseFont = False
        Me.XrLabel7.StylePriority.UseForeColor = False
        Me.XrLabel7.StylePriority.UseTextAlignment = False
        Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.XrLabel7.TextFormatString = "{0:N}"
        '
        'RPTCALCULATEALLMEMBERS
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.TopMargin, Me.BottomMargin, Me.ReportHeader, Me.GroupHeader1, Me.Detail, Me.ReportFooter})
        Me.ComponentStorage.AddRange(New System.ComponentModel.IComponent() {Me.SqlDataSource1})
        Me.DataMember = "ASSOCIATIONNAMETB"
        Me.DataSource = Me.SqlDataSource1
        Me.Dpi = 254.0!
        Me.Font = New DevExpress.Drawing.DXFont("Arial", 9.75!)
        Me.Margins = New DevExpress.Drawing.DXMargins(26.0!, 42.0!, 50.27083!, 146.7108!)
        Me.PageHeightF = 2794.0!
        Me.PageWidthF = 2159.0!
        Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
        Me.SnapGridSize = 25.0!
        Me.StyleSheet.AddRange(New DevExpress.XtraReports.UI.XRControlStyle() {Me.Title, Me.DetailCaption1, Me.DetailData1, Me.DetailData3_Odd, Me.PageInfo})
        Me.Version = "25.1"
        XrWatermark1.Id = "Watermark1"
        Me.Watermarks.AddRange(New DevExpress.XtraPrinting.Drawing.Watermark() {XrWatermark1})
        CType(Me.table1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.table2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand
    Friend WithEvents ReportHeader As DevExpress.XtraReports.UI.ReportHeaderBand
    Friend WithEvents GroupHeader1 As DevExpress.XtraReports.UI.GroupHeaderBand
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents SqlDataSource1 As DevExpress.DataAccess.Sql.SqlDataSource
    Friend WithEvents Title As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailCaption1 As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailData1 As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailData3_Odd As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents PageInfo As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents XrLabel16 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel22 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents YDATE As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel18 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel14 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents MDATE As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox10 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents table1 As DevExpress.XtraReports.UI.XRTable
    Friend WithEvents tableRow1 As DevExpress.XtraReports.UI.XRTableRow
    Friend WithEvents XrTableCell4 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell2 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell4 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents table2 As DevExpress.XtraReports.UI.XRTable
    Friend WithEvents tableRow2 As DevExpress.XtraReports.UI.XRTableRow
    Friend WithEvents tableCell5 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell6 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell8 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents PrintDate As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents PrintTime As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox1 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox23 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPictureBox6 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrPageInfo2 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrPageInfo1 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents ReportFooter As DevExpress.XtraReports.UI.ReportFooterBand
    Friend WithEvents XrLabel29 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox3 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
End Class
