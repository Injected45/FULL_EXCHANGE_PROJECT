Imports DevExpress.XtraReports.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RPTCurrencyMovement
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
        Dim Column20 As DevExpress.DataAccess.Sql.Column = New DevExpress.DataAccess.Sql.Column()
        Dim ColumnExpression20 As DevExpress.DataAccess.Sql.ColumnExpression = New DevExpress.DataAccess.Sql.ColumnExpression()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RPTCurrencyMovement))
        Dim XrWatermark1 As DevExpress.XtraReports.UI.XRWatermark = New DevExpress.XtraReports.UI.XRWatermark()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.XrPanel2 = New DevExpress.XtraReports.UI.XRPanel()
        Me.XrLabel21 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel8 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo2 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.PrintTime = New DevExpress.XtraReports.UI.XRLabel()
        Me.PrintDate = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPageInfo1 = New DevExpress.XtraReports.UI.XRPageInfo()
        Me.ReportHeader = New DevExpress.XtraReports.UI.ReportHeaderBand()
        Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel6 = New DevExpress.XtraReports.UI.XRLabel()
        Me.D1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.D2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel22 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel18 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel17 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox1 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel16 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.GroupHeader1 = New DevExpress.XtraReports.UI.GroupHeaderBand()
        Me.table1 = New DevExpress.XtraReports.UI.XRTable()
        Me.tableRow1 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.tableCell3 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell4 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell1 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell6 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell7 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell3 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell4 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell5 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.table2 = New DevExpress.XtraReports.UI.XRTable()
        Me.tableRow2 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.tableCell8 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell9 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell10 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell11 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell12 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell13 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.tableCell14 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.XrTableCell2 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.SqlDataSource1 = New DevExpress.DataAccess.Sql.SqlDataSource(Me.components)
        Me.Title = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailCaption1 = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailData1 = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.DetailData3_Odd = New DevExpress.XtraReports.UI.XRControlStyle()
        Me.PageInfo = New DevExpress.XtraReports.UI.XRControlStyle()
        CType(Me.table1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.table2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'TopMargin
        '
        Me.TopMargin.Dpi = 254.0!
        Me.TopMargin.HeightF = 26.46!
        Me.TopMargin.Name = "TopMargin"
        '
        'BottomMargin
        '
        Me.BottomMargin.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrPanel2})
        Me.BottomMargin.Dpi = 254.0!
        Me.BottomMargin.HeightF = 119.6344!
        Me.BottomMargin.Name = "BottomMargin"
        '
        'XrPanel2
        '
        Me.XrPanel2.BorderColor = System.Drawing.Color.Silver
        Me.XrPanel2.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrPanel2.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel21, Me.XrLabel8, Me.XrPageInfo2, Me.PrintTime, Me.PrintDate, Me.XrLabel7, Me.XrPageInfo1})
        Me.XrPanel2.Dpi = 254.0!
        Me.XrPanel2.LocationFloat = New DevExpress.Utils.PointFloat(8.000121!, 0!)
        Me.XrPanel2.Name = "XrPanel2"
        Me.XrPanel2.SizeF = New System.Drawing.SizeF(2808.0!, 87.3125!)
        Me.XrPanel2.StylePriority.UseBorderColor = False
        Me.XrPanel2.StylePriority.UseBorders = False
        '
        'XrLabel21
        '
        Me.XrLabel21.Dpi = 254.0!
        Me.XrLabel21.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel21.LocationFloat = New DevExpress.Utils.PointFloat(2539.161!, 10.09871!)
        Me.XrLabel21.Name = "XrLabel21"
        Me.XrLabel21.SizeF = New System.Drawing.SizeF(254.0679!, 66.99608!)
        Me.XrLabel21.StylePriority.UseFont = False
        Me.XrLabel21.StylePriority.UseTextAlignment = False
        Me.XrLabel21.Text = ":اسم المستخدم"
        Me.XrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrLabel8
        '
        Me.XrLabel8.Dpi = 254.0!
        Me.XrLabel8.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel8.LocationFloat = New DevExpress.Utils.PointFloat(1833.165!, 10.09871!)
        Me.XrLabel8.Name = "XrLabel8"
        Me.XrLabel8.SizeF = New System.Drawing.SizeF(702.9961!, 66.99608!)
        Me.XrLabel8.StylePriority.UseFont = False
        Me.XrLabel8.StylePriority.UseTextAlignment = False
        Me.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPageInfo2
        '
        Me.XrPageInfo2.Dpi = 254.0!
        Me.XrPageInfo2.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.25!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo2.LocationFloat = New DevExpress.Utils.PointFloat(1281.283!, 10.09871!)
        Me.XrPageInfo2.Name = "XrPageInfo2"
        Me.XrPageInfo2.SizeF = New System.Drawing.SizeF(551.8826!, 66.99608!)
        Me.XrPageInfo2.StyleName = "PageInfo"
        Me.XrPageInfo2.StylePriority.UseFont = False
        Me.XrPageInfo2.StylePriority.UseTextAlignment = False
        Me.XrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrPageInfo2.TextFormatString = "Page {0} of {1}"
        '
        'PrintTime
        '
        Me.PrintTime.Dpi = 254.0!
        Me.PrintTime.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.PrintTime.LocationFloat = New DevExpress.Utils.PointFloat(1052.857!, 10.09871!)
        Me.PrintTime.Name = "PrintTime"
        Me.PrintTime.SizeF = New System.Drawing.SizeF(228.4261!, 66.99608!)
        Me.PrintTime.StylePriority.UseFont = False
        Me.PrintTime.StylePriority.UseTextAlignment = False
        Me.PrintTime.Text = "تاريخ الطباعة"
        Me.PrintTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.PrintTime.TextFormatString = "{0:hh:mm tt}"
        '
        'PrintDate
        '
        Me.PrintDate.Dpi = 254.0!
        Me.PrintDate.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.PrintDate.LocationFloat = New DevExpress.Utils.PointFloat(780.2434!, 10.09871!)
        Me.PrintDate.Name = "PrintDate"
        Me.PrintDate.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.PrintDate.SizeF = New System.Drawing.SizeF(272.6135!, 66.99608!)
        Me.PrintDate.StylePriority.UseFont = False
        Me.PrintDate.StylePriority.UseTextAlignment = False
        Me.PrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.PrintDate.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel7
        '
        Me.XrLabel7.Dpi = 254.0!
        Me.XrLabel7.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!)
        Me.XrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(592.7597!, 10.09871!)
        Me.XrLabel7.Name = "XrLabel7"
        Me.XrLabel7.SizeF = New System.Drawing.SizeF(187.4837!, 66.99608!)
        Me.XrLabel7.StylePriority.UseFont = False
        Me.XrLabel7.StylePriority.UseTextAlignment = False
        Me.XrLabel7.Text = " وقت الطباعة"
        Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel7.TextFormatString = "{0:hh:mm tt}"
        '
        'XrPageInfo1
        '
        Me.XrPageInfo1.Dpi = 254.0!
        Me.XrPageInfo1.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrPageInfo1.LocationFloat = New DevExpress.Utils.PointFloat(25.00001!, 10.09871!)
        Me.XrPageInfo1.Name = "XrPageInfo1"
        Me.XrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime
        Me.XrPageInfo1.SizeF = New System.Drawing.SizeF(567.7597!, 66.99608!)
        Me.XrPageInfo1.StylePriority.UseFont = False
        Me.XrPageInfo1.StylePriority.UseTextAlignment = False
        Me.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrPageInfo1.TextFormatString = "{0:hh:mm:ss}"
        '
        'ReportHeader
        '
        Me.ReportHeader.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel4, Me.XrLabel6, Me.D1, Me.D2, Me.XrLabel22, Me.XrLabel18, Me.XrLabel17, Me.XrPictureBox1, Me.XrLabel16, Me.XrLabel15, Me.XrLabel10})
        Me.ReportHeader.Dpi = 254.0!
        Me.ReportHeader.HeightF = 314.8241!
        Me.ReportHeader.Name = "ReportHeader"
        '
        'XrLabel4
        '
        Me.XrLabel4.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel4.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel4.Dpi = 254.0!
        Me.XrLabel4.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(1044.394!, 66.2916!)
        Me.XrLabel4.Multiline = True
        Me.XrLabel4.Name = "XrLabel4"
        Me.XrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel4.SizeF = New System.Drawing.SizeF(566.8929!, 90.16833!)
        Me.XrLabel4.StylePriority.UseBorderColor = False
        Me.XrLabel4.StylePriority.UseBorders = False
        Me.XrLabel4.StylePriority.UseFont = False
        Me.XrLabel4.StylePriority.UseTextAlignment = False
        Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel6
        '
        Me.XrLabel6.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel6.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel6.Dpi = 254.0!
        Me.XrLabel6.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel6.LocationFloat = New DevExpress.Utils.PointFloat(1044.394!, 156.46!)
        Me.XrLabel6.Multiline = True
        Me.XrLabel6.Name = "XrLabel6"
        Me.XrLabel6.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel6.SizeF = New System.Drawing.SizeF(566.8929!, 90.16846!)
        Me.XrLabel6.StylePriority.UseBorderColor = False
        Me.XrLabel6.StylePriority.UseBorders = False
        Me.XrLabel6.StylePriority.UseFont = False
        Me.XrLabel6.StylePriority.UseTextAlignment = False
        Me.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'D1
        '
        Me.D1.BorderColor = System.Drawing.Color.LightGray
        Me.D1.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.D1.Dpi = 254.0!
        Me.D1.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.D1.LocationFloat = New DevExpress.Utils.PointFloat(567.2167!, 101.1685!)
        Me.D1.Multiline = True
        Me.D1.Name = "D1"
        Me.D1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.D1.SizeF = New System.Drawing.SizeF(336.691!, 90.16833!)
        Me.D1.StylePriority.UseBorderColor = False
        Me.D1.StylePriority.UseBorders = False
        Me.D1.StylePriority.UseFont = False
        Me.D1.StylePriority.UseTextAlignment = False
        Me.D1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        Me.D1.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'D2
        '
        Me.D2.BorderColor = System.Drawing.Color.LightGray
        Me.D2.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.D2.Dpi = 254.0!
        Me.D2.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.D2.LocationFloat = New DevExpress.Utils.PointFloat(567.2167!, 191.3369!)
        Me.D2.Multiline = True
        Me.D2.Name = "D2"
        Me.D2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.D2.SizeF = New System.Drawing.SizeF(336.691!, 90.16844!)
        Me.D2.StylePriority.UseBorderColor = False
        Me.D2.StylePriority.UseBorders = False
        Me.D2.StylePriority.UseFont = False
        Me.D2.StylePriority.UseTextAlignment = False
        Me.D2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        Me.D2.TextFormatString = "{0:yyyy-MM-dd}"
        '
        'XrLabel22
        '
        Me.XrLabel22.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel22.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel22.Dpi = 254.0!
        Me.XrLabel22.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel22.LocationFloat = New DevExpress.Utils.PointFloat(1611.287!, 156.4601!)
        Me.XrLabel22.Multiline = True
        Me.XrLabel22.Name = "XrLabel22"
        Me.XrLabel22.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel22.SizeF = New System.Drawing.SizeF(252.8458!, 90.16846!)
        Me.XrLabel22.StylePriority.UseBorderColor = False
        Me.XrLabel22.StylePriority.UseBorders = False
        Me.XrLabel22.StylePriority.UseFont = False
        Me.XrLabel22.StylePriority.UseTextAlignment = False
        Me.XrLabel22.Text = "الفرع"
        Me.XrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel18
        '
        Me.XrLabel18.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel18.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel18.Dpi = 254.0!
        Me.XrLabel18.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel18.LocationFloat = New DevExpress.Utils.PointFloat(1611.287!, 66.29168!)
        Me.XrLabel18.Multiline = True
        Me.XrLabel18.Name = "XrLabel18"
        Me.XrLabel18.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel18.SizeF = New System.Drawing.SizeF(252.8457!, 90.1683!)
        Me.XrLabel18.StylePriority.UseBorderColor = False
        Me.XrLabel18.StylePriority.UseBorders = False
        Me.XrLabel18.StylePriority.UseFont = False
        Me.XrLabel18.StylePriority.UseTextAlignment = False
        Me.XrLabel18.Text = "العملة"
        Me.XrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel17
        '
        Me.XrLabel17.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel17.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel17.BorderWidth = 2.0!
        Me.XrLabel17.Dpi = 254.0!
        Me.XrLabel17.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 18.0!)
        Me.XrLabel17.ForeColor = System.Drawing.Color.Black
        Me.XrLabel17.LocationFloat = New DevExpress.Utils.PointFloat(1899.864!, 29.58333!)
        Me.XrLabel17.Name = "XrLabel17"
        Me.XrLabel17.SizeF = New System.Drawing.SizeF(901.3645!, 231.4012!)
        Me.XrLabel17.StyleName = "Title"
        Me.XrLabel17.StylePriority.UseBorderColor = False
        Me.XrLabel17.StylePriority.UseBorders = False
        Me.XrLabel17.StylePriority.UseBorderWidth = False
        Me.XrLabel17.StylePriority.UseFont = False
        Me.XrLabel17.StylePriority.UseForeColor = False
        Me.XrLabel17.StylePriority.UseTextAlignment = False
        Me.XrLabel17.Text = "كشف حركة عملة"
        Me.XrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrPictureBox1
        '
        Me.XrPictureBox1.Dpi = 254.0!
        Me.XrPictureBox1.ImageSource = New DevExpress.XtraPrinting.Drawing.ImageSource(Global.ExchangeSystem.My.Resources.Resources.RHICON, True)
        Me.XrPictureBox1.LocationFloat = New DevExpress.Utils.PointFloat(0!, 9.0!)
        Me.XrPictureBox1.Name = "XrPictureBox1"
        Me.XrPictureBox1.SizeF = New System.Drawing.SizeF(363.8825!, 270.5053!)
        Me.XrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        '
        'XrLabel16
        '
        Me.XrLabel16.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel16.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel16.Dpi = 254.0!
        Me.XrLabel16.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel16.LocationFloat = New DevExpress.Utils.PointFloat(567.2167!, 11.0!)
        Me.XrLabel16.Multiline = True
        Me.XrLabel16.Name = "XrLabel16"
        Me.XrLabel16.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel16.SizeF = New System.Drawing.SizeF(414.5583!, 90.16852!)
        Me.XrLabel16.StylePriority.UseBorderColor = False
        Me.XrLabel16.StylePriority.UseBorders = False
        Me.XrLabel16.StylePriority.UseFont = False
        Me.XrLabel16.StylePriority.UseTextAlignment = False
        Me.XrLabel16.Text = "خلال الفترة"
        Me.XrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel15
        '
        Me.XrLabel15.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel15.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel15.Dpi = 254.0!
        Me.XrLabel15.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(903.9222!, 101.1685!)
        Me.XrLabel15.Multiline = True
        Me.XrLabel15.Name = "XrLabel15"
        Me.XrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel15.SizeF = New System.Drawing.SizeF(77.85278!, 90.16833!)
        Me.XrLabel15.StylePriority.UseBorderColor = False
        Me.XrLabel15.StylePriority.UseBorders = False
        Me.XrLabel15.StylePriority.UseFont = False
        Me.XrLabel15.StylePriority.UseTextAlignment = False
        Me.XrLabel15.Text = "من"
        Me.XrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel10
        '
        Me.XrLabel10.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel10.Borders = CType((((DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top) _
            Or DevExpress.XtraPrinting.BorderSide.Right) _
            Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.XrLabel10.Dpi = 254.0!
        Me.XrLabel10.Font = New DevExpress.Drawing.DXFont("Droid Arabic Kufi", 11.0!)
        Me.XrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(903.9222!, 191.3369!)
        Me.XrLabel10.Multiline = True
        Me.XrLabel10.Name = "XrLabel10"
        Me.XrLabel10.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.XrLabel10.SizeF = New System.Drawing.SizeF(77.85278!, 90.16844!)
        Me.XrLabel10.StylePriority.UseBorderColor = False
        Me.XrLabel10.StylePriority.UseBorders = False
        Me.XrLabel10.StylePriority.UseFont = False
        Me.XrLabel10.StylePriority.UseTextAlignment = False
        Me.XrLabel10.Text = "إلى"
        Me.XrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
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
        Me.table1.LocationFloat = New DevExpress.Utils.PointFloat(0!, 0!)
        Me.table1.Name = "table1"
        Me.table1.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.tableRow1})
        Me.table1.SizeF = New System.Drawing.SizeF(2833.0!, 71.12!)
        '
        'tableRow1
        '
        Me.tableRow1.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.tableCell3, Me.tableCell4, Me.XrTableCell1, Me.tableCell6, Me.tableCell7, Me.XrTableCell3, Me.XrTableCell4, Me.XrTableCell5})
        Me.tableRow1.Dpi = 254.0!
        Me.tableRow1.Name = "tableRow1"
        Me.tableRow1.Weight = 1.0R
        '
        'tableCell3
        '
        Me.tableCell3.Dpi = 254.0!
        Me.tableCell3.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell3.Name = "tableCell3"
        Me.tableCell3.StyleName = "DetailCaption1"
        Me.tableCell3.StylePriority.UseFont = False
        Me.tableCell3.StylePriority.UseTextAlignment = False
        Me.tableCell3.Text = "طبيعة الحركة"
        Me.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell3.Weight = 0.488420731634337R
        '
        'tableCell4
        '
        Me.tableCell4.Dpi = 254.0!
        Me.tableCell4.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 8.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell4.Name = "tableCell4"
        Me.tableCell4.StyleName = "DetailCaption1"
        Me.tableCell4.StylePriority.UseFont = False
        Me.tableCell4.StylePriority.UseTextAlignment = False
        Me.tableCell4.Text = "إلى خزنة"
        Me.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell4.Weight = 0.28336761723959958R
        '
        'XrTableCell1
        '
        Me.XrTableCell1.Dpi = 254.0!
        Me.XrTableCell1.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell1.Multiline = True
        Me.XrTableCell1.Name = "XrTableCell1"
        Me.XrTableCell1.StyleName = "DetailCaption1"
        Me.XrTableCell1.StylePriority.UseFont = False
        Me.XrTableCell1.StylePriority.UseTextAlignment = False
        Me.XrTableCell1.Text = "من خزنة"
        Me.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell1.Weight = 0.25357247963118479R
        '
        'tableCell6
        '
        Me.tableCell6.Dpi = 254.0!
        Me.tableCell6.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell6.Name = "tableCell6"
        Me.tableCell6.StyleName = "DetailCaption1"
        Me.tableCell6.StylePriority.UseFont = False
        Me.tableCell6.StylePriority.UseTextAlignment = False
        Me.tableCell6.Text = "دائن"
        Me.tableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell6.Weight = 0.16158122138496472R
        '
        'tableCell7
        '
        Me.tableCell7.Dpi = 254.0!
        Me.tableCell7.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.tableCell7.Name = "tableCell7"
        Me.tableCell7.StyleName = "DetailCaption1"
        Me.tableCell7.StylePriority.UseFont = False
        Me.tableCell7.StylePriority.UseTextAlignment = False
        Me.tableCell7.Text = "مدين"
        Me.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell7.Weight = 0.15154971751162108R
        '
        'XrTableCell3
        '
        Me.XrTableCell3.Dpi = 254.0!
        Me.XrTableCell3.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell3.Name = "XrTableCell3"
        Me.XrTableCell3.StyleName = "DetailCaption1"
        Me.XrTableCell3.StylePriority.UseFont = False
        Me.XrTableCell3.StylePriority.UseTextAlignment = False
        Me.XrTableCell3.Text = "التاريخ"
        Me.XrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell3.Weight = 0.23115412204731325R
        '
        'XrTableCell4
        '
        Me.XrTableCell4.Dpi = 254.0!
        Me.XrTableCell4.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell4.Name = "XrTableCell4"
        Me.XrTableCell4.StyleName = "DetailCaption1"
        Me.XrTableCell4.StylePriority.UseFont = False
        Me.XrTableCell4.StylePriority.UseTextAlignment = False
        Me.XrTableCell4.Text = "الرمز"
        Me.XrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell4.Weight = 0.20708571421253966R
        '
        'XrTableCell5
        '
        Me.XrTableCell5.Dpi = 254.0!
        Me.XrTableCell5.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrTableCell5.Name = "XrTableCell5"
        Me.XrTableCell5.StyleName = "DetailCaption1"
        Me.XrTableCell5.StylePriority.UseFont = False
        Me.XrTableCell5.StylePriority.UseTextAlignment = False
        Me.XrTableCell5.Text = "#"
        Me.XrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell5.Weight = 0.095933556288133159R
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.table2})
        Me.Detail.Dpi = 254.0!
        Me.Detail.HeightF = 65.49544!
        Me.Detail.HierarchyPrintOptions.Indent = 50.8!
        Me.Detail.Name = "Detail"
        '
        'table2
        '
        Me.table2.Dpi = 254.0!
        Me.table2.LocationFloat = New DevExpress.Utils.PointFloat(0.0002441406!, 0!)
        Me.table2.Name = "table2"
        Me.table2.OddStyleName = "DetailData3_Odd"
        Me.table2.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.tableRow2})
        Me.table2.SizeF = New System.Drawing.SizeF(2833.0!, 63.42!)
        '
        'tableRow2
        '
        Me.tableRow2.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.tableCell8, Me.tableCell9, Me.tableCell10, Me.tableCell11, Me.tableCell12, Me.tableCell13, Me.tableCell14, Me.XrTableCell2})
        Me.tableRow2.Dpi = 254.0!
        Me.tableRow2.Name = "tableRow2"
        Me.tableRow2.Weight = 11.683999633789062R
        '
        'tableCell8
        '
        Me.tableCell8.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.tableCell8.Dpi = 254.0!
        Me.tableCell8.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[طبيعة الحركة]")})
        Me.tableCell8.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell8.Name = "tableCell8"
        Me.tableCell8.StyleName = "DetailData1"
        Me.tableCell8.StylePriority.UseBorders = False
        Me.tableCell8.StylePriority.UseFont = False
        Me.tableCell8.StylePriority.UseTextAlignment = False
        Me.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell8.Weight = 0.48277064358010346R
        '
        'tableCell9
        '
        Me.tableCell9.Dpi = 254.0!
        Me.tableCell9.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[إلى خزنة]")})
        Me.tableCell9.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell9.Name = "tableCell9"
        Me.tableCell9.StyleName = "DetailData1"
        Me.tableCell9.StylePriority.UseFont = False
        Me.tableCell9.StylePriority.UseTextAlignment = False
        Me.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell9.Weight = 0.28008950694418927R
        '
        'tableCell10
        '
        Me.tableCell10.Dpi = 254.0!
        Me.tableCell10.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[من خزنة]")})
        Me.tableCell10.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell10.Name = "tableCell10"
        Me.tableCell10.StyleName = "DetailData1"
        Me.tableCell10.StylePriority.UseFont = False
        Me.tableCell10.StylePriority.UseTextAlignment = False
        Me.tableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell10.Weight = 0.25063924768797929R
        '
        'tableCell11
        '
        Me.tableCell11.Dpi = 254.0!
        Me.tableCell11.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[دائن]")})
        Me.tableCell11.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell11.Name = "tableCell11"
        Me.tableCell11.StyleName = "DetailData1"
        Me.tableCell11.StylePriority.UseFont = False
        Me.tableCell11.StylePriority.UseTextAlignment = False
        Me.tableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell11.TextFormatString = "{0:N3}"
        Me.tableCell11.Weight = 0.159712093929636R
        '
        'tableCell12
        '
        Me.tableCell12.Dpi = 254.0!
        Me.tableCell12.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[مدين]")})
        Me.tableCell12.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell12.Name = "tableCell12"
        Me.tableCell12.StyleName = "DetailData1"
        Me.tableCell12.StylePriority.UseFont = False
        Me.tableCell12.StylePriority.UseTextAlignment = False
        Me.tableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell12.TextFormatString = "{0:N3}"
        Me.tableCell12.Weight = 0.14979650896984892R
        '
        'tableCell13
        '
        Me.tableCell13.Dpi = 254.0!
        Me.tableCell13.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[التاريخ]")})
        Me.tableCell13.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell13.Name = "tableCell13"
        Me.tableCell13.StyleName = "DetailData1"
        Me.tableCell13.StylePriority.UseFont = False
        Me.tableCell13.StylePriority.UseTextAlignment = False
        Me.tableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell13.TextFormatString = "{0:yyyy-MM-dd}"
        Me.tableCell13.Weight = 0.22848012213524305R
        '
        'tableCell14
        '
        Me.tableCell14.Dpi = 254.0!
        Me.tableCell14.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[الرمز]")})
        Me.tableCell14.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.tableCell14.Name = "tableCell14"
        Me.tableCell14.StyleName = "DetailData1"
        Me.tableCell14.StylePriority.UseFont = False
        Me.tableCell14.StylePriority.UseTextAlignment = False
        Me.tableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.tableCell14.Weight = 0.20469011718331787R
        '
        'XrTableCell2
        '
        Me.XrTableCell2.Dpi = 254.0!
        Me.XrTableCell2.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()")})
        Me.XrTableCell2.Font = New DevExpress.Drawing.DXFont("droid Arabic Kufi", 9.0!)
        Me.XrTableCell2.Multiline = True
        Me.XrTableCell2.Name = "XrTableCell2"
        Me.XrTableCell2.StyleName = "DetailData1"
        Me.XrTableCell2.StylePriority.UseFont = False
        Me.XrTableCell2.StylePriority.UseTextAlignment = False
        XrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report
        Me.XrTableCell2.Summary = XrSummary1
        Me.XrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrTableCell2.Weight = 0.094824106006855868R
        '
        'SqlDataSource1
        '
        Me.SqlDataSource1.ConnectionName = "localhost_EXCHANGESYS_TEST_Connection"
        Me.SqlDataSource1.Name = "SqlDataSource1"
        ColumnExpression1.ColumnName = "ID"
        Table3.Name = "DailyCloseTb"
        ColumnExpression1.Table = Table3
        Column1.Expression = ColumnExpression1
        ColumnExpression2.ColumnName = "InsertDate"
        ColumnExpression2.Table = Table3
        Column2.Expression = ColumnExpression2
        ColumnExpression3.ColumnName = "Code"
        ColumnExpression3.Table = Table3
        Column3.Expression = ColumnExpression3
        ColumnExpression4.ColumnName = "CurrnecyID"
        ColumnExpression4.Table = Table3
        Column4.Expression = ColumnExpression4
        ColumnExpression5.ColumnName = "SafeIDFrom"
        ColumnExpression5.Table = Table3
        Column5.Expression = ColumnExpression5
        ColumnExpression6.ColumnName = "SafeIDTo"
        ColumnExpression6.Table = Table3
        Column6.Expression = ColumnExpression6
        ColumnExpression7.ColumnName = "OverallDebit"
        ColumnExpression7.Table = Table3
        Column7.Expression = ColumnExpression7
        ColumnExpression8.ColumnName = "OverallCredit"
        ColumnExpression8.Table = Table3
        Column8.Expression = ColumnExpression8
        ColumnExpression9.ColumnName = "OverAllNetTotal"
        ColumnExpression9.Table = Table3
        Column9.Expression = ColumnExpression9
        ColumnExpression10.ColumnName = "OverallRBVAL"
        ColumnExpression10.Table = Table3
        Column10.Expression = ColumnExpression10
        ColumnExpression11.ColumnName = "OverallDBVAL"
        ColumnExpression11.Table = Table3
        Column11.Expression = ColumnExpression11
        ColumnExpression12.ColumnName = "RBVALCanceled"
        ColumnExpression12.Table = Table3
        Column12.Expression = ColumnExpression12
        ColumnExpression13.ColumnName = "RBVALFromBranches"
        ColumnExpression13.Table = Table3
        Column13.Expression = ColumnExpression13
        ColumnExpression14.ColumnName = "OverallMainVal"
        ColumnExpression14.Table = Table3
        Column14.Expression = ColumnExpression14
        ColumnExpression15.ColumnName = "RBVALFromCanceledBranches"
        ColumnExpression15.Table = Table3
        Column15.Expression = ColumnExpression15
        ColumnExpression16.ColumnName = "BenefitNetTotal"
        ColumnExpression16.Table = Table3
        Column16.Expression = ColumnExpression16
        ColumnExpression17.ColumnName = "SafeCID"
        ColumnExpression17.Table = Table3
        Column17.Expression = ColumnExpression17
        ColumnExpression18.ColumnName = "BranchID"
        ColumnExpression18.Table = Table3
        Column18.Expression = ColumnExpression18
        ColumnExpression19.ColumnName = "TransType"
        ColumnExpression19.Table = Table3
        Column19.Expression = ColumnExpression19
        ColumnExpression20.ColumnName = "IsActive"
        ColumnExpression20.Table = Table3
        Column20.Expression = ColumnExpression20
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
        SelectQuery1.Columns.Add(Column20)
        SelectQuery1.Name = "DailyCloseTb"
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
        Me.Title.Padding = New DevExpress.XtraPrinting.PaddingInfo(15, 15, 0, 0, 254.0!)
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
        Me.DetailCaption1.Padding = New DevExpress.XtraPrinting.PaddingInfo(15, 15, 0, 0, 254.0!)
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
        Me.DetailData1.Padding = New DevExpress.XtraPrinting.PaddingInfo(15, 15, 0, 0, 254.0!)
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
        Me.DetailData3_Odd.Padding = New DevExpress.XtraPrinting.PaddingInfo(15, 15, 0, 0, 254.0!)
        Me.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        '
        'PageInfo
        '
        Me.PageInfo.Font = New DevExpress.Drawing.DXFont("Arial", 8.25!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.PageInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.PageInfo.Name = "PageInfo"
        Me.PageInfo.Padding = New DevExpress.XtraPrinting.PaddingInfo(15, 15, 0, 0, 254.0!)
        '
        'RPTCurrencyMovement
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.TopMargin, Me.BottomMargin, Me.ReportHeader, Me.GroupHeader1, Me.Detail})
        Me.ComponentStorage.AddRange(New System.ComponentModel.IComponent() {Me.SqlDataSource1})
        Me.DataMember = "DailyCloseTb"
        Me.DataSource = Me.SqlDataSource1
        Me.Dpi = 254.0!
        Me.Font = New DevExpress.Drawing.DXFont("Arial", 9.75!)
        Me.Landscape = True
        Me.Margins = New DevExpress.Drawing.DXMargins(58.0!, 79.0!, 26.46!, 119.6344!)
        Me.PageHeight = 2100
        Me.PageWidth = 2970
        Me.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4
        Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
        Me.SnapGridSize = 25.0!
        Me.StyleSheet.AddRange(New DevExpress.XtraReports.UI.XRControlStyle() {Me.Title, Me.DetailCaption1, Me.DetailData1, Me.DetailData3_Odd, Me.PageInfo})
        Me.Version = "24.1"
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
    Friend WithEvents table2 As DevExpress.XtraReports.UI.XRTable
    Friend WithEvents tableRow2 As DevExpress.XtraReports.UI.XRTableRow
    Friend WithEvents tableCell8 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell9 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell10 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell11 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell12 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell13 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell14 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents SqlDataSource1 As DevExpress.DataAccess.Sql.SqlDataSource
    Friend WithEvents Title As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailCaption1 As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailData1 As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents DetailData3_Odd As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents PageInfo As DevExpress.XtraReports.UI.XRControlStyle
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel6 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents D1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents D2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel22 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel18 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel17 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox1 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel16 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPanel2 As DevExpress.XtraReports.UI.XRPanel
    Friend WithEvents XrLabel21 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo2 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents PrintTime As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents PrintDate As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPageInfo1 As DevExpress.XtraReports.UI.XRPageInfo
    Friend WithEvents XrTableCell2 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents table1 As DevExpress.XtraReports.UI.XRTable
    Friend WithEvents tableRow1 As DevExpress.XtraReports.UI.XRTableRow
    Friend WithEvents tableCell3 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell4 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell1 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell6 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents tableCell7 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell3 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell4 As DevExpress.XtraReports.UI.XRTableCell
    Friend WithEvents XrTableCell5 As DevExpress.XtraReports.UI.XRTableCell
End Class
