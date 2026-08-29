<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class RPTResignation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RPTResignation))
        Dim ShapeRectangle1 As DevExpress.XtraPrinting.Shape.ShapeRectangle = New DevExpress.XtraPrinting.Shape.ShapeRectangle()
        Dim XrWatermark1 As DevExpress.XtraReports.UI.XRWatermark = New DevExpress.XtraReports.UI.XRWatermark()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.ReportHeader = New DevExpress.XtraReports.UI.ReportHeaderBand()
        Me.XrLabel31 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel30 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel29 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel28 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel27 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel26 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel25 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel24 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel23 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel21 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel22 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel18 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel19 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel16 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel17 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel14 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel13 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel12 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel11 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel9 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel8 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrRichText1 = New DevExpress.XtraReports.UI.XRRichText()
        Me.XrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel5 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel20 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrShape4 = New DevExpress.XtraReports.UI.XRShape()
        Me.XrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrPictureBox1 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.XrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel6 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrShape2 = New DevExpress.XtraReports.UI.XRShape()
        CType(Me.XrRichText1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'TopMargin
        '
        Me.TopMargin.HeightF = 13.54167!
        Me.TopMargin.Name = "TopMargin"
        '
        'BottomMargin
        '
        Me.BottomMargin.HeightF = 3.710429!
        Me.BottomMargin.Name = "BottomMargin"
        '
        'Detail
        '
        Me.Detail.Expanded = False
        Me.Detail.HeightF = 1.041667!
        Me.Detail.Name = "Detail"
        '
        'ReportHeader
        '
        Me.ReportHeader.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel31, Me.XrLabel30, Me.XrLabel29, Me.XrLabel28, Me.XrLabel27, Me.XrLabel26, Me.XrLabel25, Me.XrLabel24, Me.XrLabel23, Me.XrLabel21, Me.XrLabel22, Me.XrLabel18, Me.XrLabel19, Me.XrLabel16, Me.XrLabel17, Me.XrLabel14, Me.XrLabel15, Me.XrLabel13, Me.XrLabel12, Me.XrLabel11, Me.XrLabel10, Me.XrLabel9, Me.XrLabel8, Me.XrLabel4, Me.XrLabel2, Me.XrRichText1, Me.XrLabel1, Me.XrLabel5, Me.XrLabel20, Me.XrShape4, Me.XrLabel7, Me.XrPictureBox1, Me.XrLabel3, Me.XrLabel6, Me.XrShape2})
        Me.ReportHeader.HeightF = 1103.353!
        Me.ReportHeader.Name = "ReportHeader"
        '
        'XrLabel31
        '
        Me.XrLabel31.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel31.CanGrow = False
        Me.XrLabel31.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EMPNAME]")})
        Me.XrLabel31.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel31.LocationFloat = New DevExpress.Utils.PointFloat(176.7628!, 1019.937!)
        Me.XrLabel31.Multiline = True
        Me.XrLabel31.Name = "XrLabel31"
        Me.XrLabel31.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel31.SizeF = New System.Drawing.SizeF(324.2458!, 35.49957!)
        Me.XrLabel31.StylePriority.UseBorderColor = False
        Me.XrLabel31.StylePriority.UseBorders = False
        Me.XrLabel31.StylePriority.UseFont = False
        Me.XrLabel31.StylePriority.UseTextAlignment = False
        Me.XrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter
        '
        'XrLabel30
        '
        Me.XrLabel30.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel30.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel30.CanGrow = False
        Me.XrLabel30.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel30.LocationFloat = New DevExpress.Utils.PointFloat(512.7609!, 1022.676!)
        Me.XrLabel30.Multiline = True
        Me.XrLabel30.Name = "XrLabel30"
        Me.XrLabel30.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel30.SizeF = New System.Drawing.SizeF(259.8351!, 32.76038!)
        Me.XrLabel30.StylePriority.UseBorderColor = False
        Me.XrLabel30.StylePriority.UseBorders = False
        Me.XrLabel30.StylePriority.UseFont = False
        Me.XrLabel30.StylePriority.UseTextAlignment = False
        Me.XrLabel30.Text = "       توقيع الموظف: .............."
        Me.XrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'XrLabel29
        '
        Me.XrLabel29.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel29.CanGrow = False
        Me.XrLabel29.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel29.LocationFloat = New DevExpress.Utils.PointFloat(31.90076!, 1019.937!)
        Me.XrLabel29.Multiline = True
        Me.XrLabel29.Name = "XrLabel29"
        Me.XrLabel29.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel29.SizeF = New System.Drawing.SizeF(144.862!, 35.49951!)
        Me.XrLabel29.StylePriority.UseBorderColor = False
        Me.XrLabel29.StylePriority.UseBorders = False
        Me.XrLabel29.StylePriority.UseFont = False
        Me.XrLabel29.StylePriority.UseTextAlignment = False
        Me.XrLabel29.Text = "اسم الموظف:"
        Me.XrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter
        '
        'XrLabel28
        '
        Me.XrLabel28.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel28.CanGrow = False
        Me.XrLabel28.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ResignatoinDate]")})
        Me.XrLabel28.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel28.LocationFloat = New DevExpress.Utils.PointFloat(213.0707!, 499.0186!)
        Me.XrLabel28.Multiline = True
        Me.XrLabel28.Name = "XrLabel28"
        Me.XrLabel28.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel28.SizeF = New System.Drawing.SizeF(171.3064!, 42.79108!)
        Me.XrLabel28.StylePriority.UseBorderColor = False
        Me.XrLabel28.StylePriority.UseBorders = False
        Me.XrLabel28.StylePriority.UseFont = False
        Me.XrLabel28.StylePriority.UseTextAlignment = False
        Me.XrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel28.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel27
        '
        Me.XrLabel27.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel27.CanGrow = False
        Me.XrLabel27.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel27.LocationFloat = New DevExpress.Utils.PointFloat(24.56952!, 795.3442!)
        Me.XrLabel27.Multiline = True
        Me.XrLabel27.Name = "XrLabel27"
        Me.XrLabel27.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel27.SizeF = New System.Drawing.SizeF(753.4849!, 141.7495!)
        Me.XrLabel27.StylePriority.UseBorderColor = False
        Me.XrLabel27.StylePriority.UseBorders = False
        Me.XrLabel27.StylePriority.UseFont = False
        Me.XrLabel27.StylePriority.UseTextAlignment = False
        Me.XrLabel27.Text = resources.GetString("XrLabel27.Text")
        Me.XrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'XrLabel26
        '
        Me.XrLabel26.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel26.CanGrow = False
        Me.XrLabel26.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel26.LocationFloat = New DevExpress.Utils.PointFloat(18.56952!, 547.8096!)
        Me.XrLabel26.Multiline = True
        Me.XrLabel26.Name = "XrLabel26"
        Me.XrLabel26.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel26.SizeF = New System.Drawing.SizeF(87.44348!, 42.79102!)
        Me.XrLabel26.StylePriority.UseBorderColor = False
        Me.XrLabel26.StylePriority.UseBorders = False
        Me.XrLabel26.StylePriority.UseFont = False
        Me.XrLabel26.StylePriority.UseTextAlignment = False
        Me.XrLabel26.Text = "(30) يوماً."
        Me.XrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel26.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel25
        '
        Me.XrLabel25.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel25.CanGrow = False
        Me.XrLabel25.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel25.LocationFloat = New DevExpress.Utils.PointFloat(384.3771!, 499.0186!)
        Me.XrLabel25.Multiline = True
        Me.XrLabel25.Name = "XrLabel25"
        Me.XrLabel25.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel25.SizeF = New System.Drawing.SizeF(397.6773!, 42.79108!)
        Me.XrLabel25.StylePriority.UseBorderColor = False
        Me.XrLabel25.StylePriority.UseBorders = False
        Me.XrLabel25.StylePriority.UseFont = False
        Me.XrLabel25.StylePriority.UseTextAlignment = False
        Me.XrLabel25.Text = "وفقا لفترة الإشعار المعتمدة في عقد العمل والتي تبلغ"
        Me.XrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel25.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel24
        '
        Me.XrLabel24.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel24.CanGrow = False
        Me.XrLabel24.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel24.LocationFloat = New DevExpress.Utils.PointFloat(19.83771!, 499.0186!)
        Me.XrLabel24.Multiline = True
        Me.XrLabel24.Name = "XrLabel24"
        Me.XrLabel24.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel24.SizeF = New System.Drawing.SizeF(193.233!, 42.79108!)
        Me.XrLabel24.StylePriority.UseBorderColor = False
        Me.XrLabel24.StylePriority.UseBorders = False
        Me.XrLabel24.StylePriority.UseFont = False
        Me.XrLabel24.StylePriority.UseTextAlignment = False
        Me.XrLabel24.Text = "وذلك اعتباراً من تاريخ:"
        Me.XrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel24.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel23
        '
        Me.XrLabel23.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel23.CanGrow = False
        Me.XrLabel23.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel23.LocationFloat = New DevExpress.Utils.PointFloat(18.56953!, 450.2275!)
        Me.XrLabel23.Multiline = True
        Me.XrLabel23.Name = "XrLabel23"
        Me.XrLabel23.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel23.SizeF = New System.Drawing.SizeF(764.4809!, 42.79105!)
        Me.XrLabel23.StylePriority.UseBorderColor = False
        Me.XrLabel23.StylePriority.UseBorders = False
        Me.XrLabel23.StylePriority.UseFont = False
        Me.XrLabel23.StylePriority.UseTextAlignment = False
        Me.XrLabel23.Text = "أتقدم إليكم بطلب استقالتي من العمل في شركة الرحالة القابضة."
        Me.XrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.XrLabel23.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel21
        '
        Me.XrLabel21.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel21.CanGrow = False
        Me.XrLabel21.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel21.LocationFloat = New DevExpress.Utils.PointFloat(18.56995!, 403.4364!)
        Me.XrLabel21.Multiline = True
        Me.XrLabel21.Name = "XrLabel21"
        Me.XrLabel21.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel21.SizeF = New System.Drawing.SizeF(137.2879!, 42.79105!)
        Me.XrLabel21.StylePriority.UseBorderColor = False
        Me.XrLabel21.StylePriority.UseBorders = False
        Me.XrLabel21.StylePriority.UseFont = False
        Me.XrLabel21.StylePriority.UseTextAlignment = False
        Me.XrLabel21.Text = "تاريخ الالتحاق:"
        Me.XrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel21.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel22
        '
        Me.XrLabel22.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel22.CanGrow = False
        Me.XrLabel22.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EMPDATE]")})
        Me.XrLabel22.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel22.LocationFloat = New DevExpress.Utils.PointFloat(162.858!, 403.4364!)
        Me.XrLabel22.Multiline = True
        Me.XrLabel22.Name = "XrLabel22"
        Me.XrLabel22.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel22.SizeF = New System.Drawing.SizeF(246.5701!, 42.79105!)
        Me.XrLabel22.StylePriority.UseBorderColor = False
        Me.XrLabel22.StylePriority.UseBorders = False
        Me.XrLabel22.StylePriority.UseFont = False
        Me.XrLabel22.StylePriority.UseTextAlignment = False
        Me.XrLabel22.Text = "____________________"
        Me.XrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel22.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel18
        '
        Me.XrLabel18.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel18.CanGrow = False
        Me.XrLabel18.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel18.LocationFloat = New DevExpress.Utils.PointFloat(424.1926!, 354.6454!)
        Me.XrLabel18.Multiline = True
        Me.XrLabel18.Name = "XrLabel18"
        Me.XrLabel18.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel18.SizeF = New System.Drawing.SizeF(129.9407!, 42.79105!)
        Me.XrLabel18.StylePriority.UseBorderColor = False
        Me.XrLabel18.StylePriority.UseBorders = False
        Me.XrLabel18.StylePriority.UseFont = False
        Me.XrLabel18.StylePriority.UseTextAlignment = False
        Me.XrLabel18.Text = "القسم/الإدارة:"
        Me.XrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel18.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel19
        '
        Me.XrLabel19.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel19.CanGrow = False
        Me.XrLabel19.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel19.LocationFloat = New DevExpress.Utils.PointFloat(559.7194!, 354.6454!)
        Me.XrLabel19.Multiline = True
        Me.XrLabel19.Name = "XrLabel19"
        Me.XrLabel19.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel19.SizeF = New System.Drawing.SizeF(222.3352!, 42.79105!)
        Me.XrLabel19.StylePriority.UseBorderColor = False
        Me.XrLabel19.StylePriority.UseBorders = False
        Me.XrLabel19.StylePriority.UseFont = False
        Me.XrLabel19.StylePriority.UseTextAlignment = False
        Me.XrLabel19.Text = "_________________"
        Me.XrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel19.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel16
        '
        Me.XrLabel16.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel16.CanGrow = False
        Me.XrLabel16.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel16.LocationFloat = New DevExpress.Utils.PointFloat(18.56982!, 354.6454!)
        Me.XrLabel16.Multiline = True
        Me.XrLabel16.Name = "XrLabel16"
        Me.XrLabel16.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel16.SizeF = New System.Drawing.SizeF(137.2879!, 42.79105!)
        Me.XrLabel16.StylePriority.UseBorderColor = False
        Me.XrLabel16.StylePriority.UseBorders = False
        Me.XrLabel16.StylePriority.UseFont = False
        Me.XrLabel16.StylePriority.UseTextAlignment = False
        Me.XrLabel16.Text = "المسمى الوظيفي:"
        Me.XrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel16.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel17
        '
        Me.XrLabel17.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel17.CanGrow = False
        Me.XrLabel17.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ECNAME]")})
        Me.XrLabel17.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel17.LocationFloat = New DevExpress.Utils.PointFloat(162.8578!, 354.6454!)
        Me.XrLabel17.Multiline = True
        Me.XrLabel17.Name = "XrLabel17"
        Me.XrLabel17.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel17.SizeF = New System.Drawing.SizeF(246.5701!, 42.79105!)
        Me.XrLabel17.StylePriority.UseBorderColor = False
        Me.XrLabel17.StylePriority.UseBorders = False
        Me.XrLabel17.StylePriority.UseFont = False
        Me.XrLabel17.StylePriority.UseTextAlignment = False
        Me.XrLabel17.Text = "__________________"
        Me.XrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel17.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel14
        '
        Me.XrLabel14.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel14.CanGrow = False
        Me.XrLabel14.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel14.LocationFloat = New DevExpress.Utils.PointFloat(497.2209!, 305.8544!)
        Me.XrLabel14.Multiline = True
        Me.XrLabel14.Name = "XrLabel14"
        Me.XrLabel14.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel14.SizeF = New System.Drawing.SizeF(115.8294!, 42.79105!)
        Me.XrLabel14.StylePriority.UseBorderColor = False
        Me.XrLabel14.StylePriority.UseBorders = False
        Me.XrLabel14.StylePriority.UseFont = False
        Me.XrLabel14.StylePriority.UseTextAlignment = False
        Me.XrLabel14.Text = "الرقم الوظيفي"
        Me.XrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        Me.XrLabel14.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel15
        '
        Me.XrLabel15.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel15.CanGrow = False
        Me.XrLabel15.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmpCode]")})
        Me.XrLabel15.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(618.0965!, 305.8545!)
        Me.XrLabel15.Multiline = True
        Me.XrLabel15.Name = "XrLabel15"
        Me.XrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel15.SizeF = New System.Drawing.SizeF(164.9037!, 42.79105!)
        Me.XrLabel15.StylePriority.UseBorderColor = False
        Me.XrLabel15.StylePriority.UseBorders = False
        Me.XrLabel15.StylePriority.UseFont = False
        Me.XrLabel15.StylePriority.UseTextAlignment = False
        Me.XrLabel15.Text = "____________________"
        Me.XrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel15.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel13
        '
        Me.XrLabel13.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel13.CanGrow = False
        Me.XrLabel13.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EMPNAME]")})
        Me.XrLabel13.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel13.LocationFloat = New DevExpress.Utils.PointFloat(200.3044!, 305.8545!)
        Me.XrLabel13.Multiline = True
        Me.XrLabel13.Name = "XrLabel13"
        Me.XrLabel13.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel13.SizeF = New System.Drawing.SizeF(287.9165!, 42.79105!)
        Me.XrLabel13.StylePriority.UseBorderColor = False
        Me.XrLabel13.StylePriority.UseBorders = False
        Me.XrLabel13.StylePriority.UseFont = False
        Me.XrLabel13.StylePriority.UseTextAlignment = False
        Me.XrLabel13.Text = "____________________"
        Me.XrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel13.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel12
        '
        Me.XrLabel12.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel12.CanGrow = False
        Me.XrLabel12.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel12.LocationFloat = New DevExpress.Utils.PointFloat(19.83783!, 745.4789!)
        Me.XrLabel12.Multiline = True
        Me.XrLabel12.Name = "XrLabel12"
        Me.XrLabel12.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel12.SizeF = New System.Drawing.SizeF(320.8569!, 42.58264!)
        Me.XrLabel12.StylePriority.UseBorderColor = False
        Me.XrLabel12.StylePriority.UseBorders = False
        Me.XrLabel12.StylePriority.UseFont = False
        Me.XrLabel12.StylePriority.UseTextAlignment = False
        Me.XrLabel12.Text = "☐ أخرى (يرجى كتابة التوضيح:"
        Me.XrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel12.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel11
        '
        Me.XrLabel11.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel11.CanGrow = False
        Me.XrLabel11.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel11.LocationFloat = New DevExpress.Utils.PointFloat(351.1115!, 745.4789!)
        Me.XrLabel11.Multiline = True
        Me.XrLabel11.Name = "XrLabel11"
        Me.XrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel11.SizeF = New System.Drawing.SizeF(430.943!, 42.58264!)
        Me.XrLabel11.StylePriority.UseBorderColor = False
        Me.XrLabel11.StylePriority.UseBorders = False
        Me.XrLabel11.StylePriority.UseFont = False
        Me.XrLabel11.StylePriority.UseTextAlignment = False
        Me.XrLabel11.Text = "....................................................................."
        Me.XrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        Me.XrLabel11.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel10
        '
        Me.XrLabel10.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel10.CanGrow = False
        Me.XrLabel10.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(586.153!, 649.0621!)
        Me.XrLabel10.Multiline = True
        Me.XrLabel10.Name = "XrLabel10"
        Me.XrLabel10.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel10.SizeF = New System.Drawing.SizeF(195.9015!, 46.95764!)
        Me.XrLabel10.StylePriority.UseBorderColor = False
        Me.XrLabel10.StylePriority.UseBorders = False
        Me.XrLabel10.StylePriority.UseFont = False
        Me.XrLabel10.StylePriority.UseTextAlignment = False
        Me.XrLabel10.Text = "☐ ظروف صحية "
        Me.XrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.XrLabel10.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel9
        '
        Me.XrLabel9.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel9.CanGrow = False
        Me.XrLabel9.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel9.LocationFloat = New DevExpress.Utils.PointFloat(285.3461!, 649.0621!)
        Me.XrLabel9.Multiline = True
        Me.XrLabel9.Name = "XrLabel9"
        Me.XrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel9.SizeF = New System.Drawing.SizeF(293.8068!, 85.41669!)
        Me.XrLabel9.StylePriority.UseBorderColor = False
        Me.XrLabel9.StylePriority.UseBorders = False
        Me.XrLabel9.StylePriority.UseFont = False
        Me.XrLabel9.StylePriority.UseTextAlignment = False
        Me.XrLabel9.Text = "☐ أسباب شخصية " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "☐ عدم التوافق مع بيئة العمل "
        Me.XrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.XrLabel9.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel8
        '
        Me.XrLabel8.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel8.CanGrow = False
        Me.XrLabel8.Font = New DevExpress.Drawing.DXFont("Sultan Medium", 16.0!)
        Me.XrLabel8.LocationFloat = New DevExpress.Utils.PointFloat(18.56958!, 649.0622!)
        Me.XrLabel8.Multiline = True
        Me.XrLabel8.Name = "XrLabel8"
        Me.XrLabel8.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel8.SizeF = New System.Drawing.SizeF(259.4848!, 85.41669!)
        Me.XrLabel8.StylePriority.UseBorderColor = False
        Me.XrLabel8.StylePriority.UseBorders = False
        Me.XrLabel8.StylePriority.UseFont = False
        Me.XrLabel8.StylePriority.UseTextAlignment = False
        Me.XrLabel8.Text = "☐ الانتقال إلى وظفية أخرى " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "☐ ظروف عائلية "
        Me.XrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.XrLabel8.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel4
        '
        Me.XrLabel4.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel4.CanGrow = False
        Me.XrLabel4.Font = New DevExpress.Drawing.DXFont("Sultan Medium", 16.0!)
        Me.XrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(24.56952!, 595.3122!)
        Me.XrLabel4.Multiline = True
        Me.XrLabel4.Name = "XrLabel4"
        Me.XrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel4.SizeF = New System.Drawing.SizeF(748.0265!, 41.74927!)
        Me.XrLabel4.StylePriority.UseBorderColor = False
        Me.XrLabel4.StylePriority.UseBorders = False
        Me.XrLabel4.StylePriority.UseFont = False
        Me.XrLabel4.StylePriority.UseTextAlignment = False
        Me.XrLabel4.Text = "وذلك لأحد الأسباب التالية (يرجى اختيار السبب)"
        Me.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        Me.XrLabel4.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrLabel2
        '
        Me.XrLabel2.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel2.CanGrow = False
        Me.XrLabel2.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(62.17646!, 75.8416!)
        Me.XrLabel2.Multiline = True
        Me.XrLabel2.Name = "XrLabel2"
        Me.XrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel2.SizeF = New System.Drawing.SizeF(243.278!, 35.49938!)
        Me.XrLabel2.StylePriority.UseBorderColor = False
        Me.XrLabel2.StylePriority.UseBorders = False
        Me.XrLabel2.StylePriority.UseFont = False
        Me.XrLabel2.StylePriority.UseTextAlignment = False
        Me.XrLabel2.Text = "نموذج طلب استقالة"
        Me.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        '
        'XrRichText1
        '
        Me.XrRichText1.CanGrow = False
        Me.XrRichText1.LocationFloat = New DevExpress.Utils.PointFloat(31.90075!, 201.6049!)
        Me.XrRichText1.Name = "XrRichText1"
        Me.XrRichText1.SerializableRtfString = resources.GetString("XrRichText1.SerializableRtfString")
        Me.XrRichText1.SizeF = New System.Drawing.SizeF(746.1536!, 57.29164!)
        Me.XrRichText1.StylePriority.UseFont = False
        '
        'XrLabel1
        '
        Me.XrLabel1.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel1.CanGrow = False
        Me.XrLabel1.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(181.0169!, 264.8965!)
        Me.XrLabel1.Multiline = True
        Me.XrLabel1.Name = "XrLabel1"
        Me.XrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel1.SizeF = New System.Drawing.SizeF(451.204!, 35.49942!)
        Me.XrLabel1.StylePriority.UseBorderColor = False
        Me.XrLabel1.StylePriority.UseBorders = False
        Me.XrLabel1.StylePriority.UseFont = False
        Me.XrLabel1.StylePriority.UseTextAlignment = False
        Me.XrLabel1.Text = "السلام عليـكم ورحمه الله وبركاته"
        Me.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel5
        '
        Me.XrLabel5.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel5.CanGrow = False
        Me.XrLabel5.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel5.LocationFloat = New DevExpress.Utils.PointFloat(224.8196!, 968.3436!)
        Me.XrLabel5.Multiline = True
        Me.XrLabel5.Name = "XrLabel5"
        Me.XrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel5.SizeF = New System.Drawing.SizeF(407.4014!, 35.49951!)
        Me.XrLabel5.StylePriority.UseBorderColor = False
        Me.XrLabel5.StylePriority.UseBorders = False
        Me.XrLabel5.StylePriority.UseFont = False
        Me.XrLabel5.StylePriority.UseTextAlignment = False
        Me.XrLabel5.Text = "تفضلوا بالاستلام مع فائق الاحترام"
        Me.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel20
        '
        Me.XrLabel20.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel20.CanGrow = False
        Me.XrLabel20.Font = New DevExpress.Drawing.DXFont("sultan medium", 16.0!)
        Me.XrLabel20.LocationFloat = New DevExpress.Utils.PointFloat(18.5697!, 305.8544!)
        Me.XrLabel20.Multiline = True
        Me.XrLabel20.Name = "XrLabel20"
        Me.XrLabel20.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel20.SizeF = New System.Drawing.SizeF(172.2919!, 42.79105!)
        Me.XrLabel20.StylePriority.UseBorderColor = False
        Me.XrLabel20.StylePriority.UseBorders = False
        Me.XrLabel20.StylePriority.UseFont = False
        Me.XrLabel20.StylePriority.UseTextAlignment = False
        Me.XrLabel20.Text = "الموقع أدناه، الموظف:"
        Me.XrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel20.TextTrimming = DevExpress.Drawing.DXStringTrimming.Word
        '
        'XrShape4
        '
        Me.XrShape4.BorderWidth = 1.0!
        Me.XrShape4.FillColor = System.Drawing.Color.DarkOrange
        Me.XrShape4.ForeColor = System.Drawing.Color.Transparent
        Me.XrShape4.LineWidth = 0
        Me.XrShape4.LocationFloat = New DevExpress.Utils.PointFloat(105.5708!, 108.3309!)
        Me.XrShape4.Name = "XrShape4"
        Me.XrShape4.SizeF = New System.Drawing.SizeF(156.25!, 3.010094!)
        Me.XrShape4.StylePriority.UseBorderWidth = False
        Me.XrShape4.StylePriority.UseForeColor = False
        '
        'XrLabel7
        '
        Me.XrLabel7.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel7.CanGrow = False
        Me.XrLabel7.Font = New DevExpress.Drawing.DXFont("univers Next Arabic", 24.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(435.0931!, 60.14523!)
        Me.XrLabel7.Multiline = True
        Me.XrLabel7.Name = "XrLabel7"
        Me.XrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel7.SizeF = New System.Drawing.SizeF(293.2779!, 58.84924!)
        Me.XrLabel7.StylePriority.UseBorderColor = False
        Me.XrLabel7.StylePriority.UseBorders = False
        Me.XrLabel7.StylePriority.UseFont = False
        Me.XrLabel7.StylePriority.UseTextAlignment = False
        Me.XrLabel7.Text = "شركة الرحالة القابضة"
        Me.XrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrPictureBox1
        '
        Me.XrPictureBox1.BorderColor = System.Drawing.Color.Transparent

        Me.XrPictureBox1.LocationFloat = New DevExpress.Utils.PointFloat(328.0329!, 47.48354!)
        Me.XrPictureBox1.Name = "XrPictureBox1"
        Me.XrPictureBox1.SizeF = New System.Drawing.SizeF(97.42752!, 88.5417!)
        Me.XrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        Me.XrPictureBox1.StylePriority.UseBorderColor = False
        '
        'XrLabel3
        '
        Me.XrLabel3.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel3.CanGrow = False
        Me.XrLabel3.Font = New DevExpress.Drawing.DXFont("times New Roman", 11.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(11.40906!, 22.687!)
        Me.XrLabel3.Multiline = True
        Me.XrLabel3.Name = "XrLabel3"
        Me.XrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel3.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel3.SizeF = New System.Drawing.SizeF(144.3196!, 35.4994!)
        Me.XrLabel3.StylePriority.UseBorderColor = False
        Me.XrLabel3.StylePriority.UseBorders = False
        Me.XrLabel3.StylePriority.UseFont = False
        Me.XrLabel3.StylePriority.UseTextAlignment = False
        Me.XrLabel3.Text = "نموذج رقم (22)"
        Me.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'XrLabel6
        '
        Me.XrLabel6.BorderColor = System.Drawing.Color.LightGray
        Me.XrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.XrLabel6.CanGrow = False
        Me.XrLabel6.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DayName]")})
        Me.XrLabel6.Font = New DevExpress.Drawing.DXFont("times New Roman", 16.0!, DevExpress.Drawing.DXFontStyle.Bold)
        Me.XrLabel6.LocationFloat = New DevExpress.Utils.PointFloat(435.0932!, 129.6867!)
        Me.XrLabel6.Name = "XrLabel6"
        Me.XrLabel6.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel6.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.XrLabel6.SizeF = New System.Drawing.SizeF(293.2779!, 35.4994!)
        Me.XrLabel6.StylePriority.UseBorderColor = False
        Me.XrLabel6.StylePriority.UseBorders = False
        Me.XrLabel6.StylePriority.UseFont = False
        Me.XrLabel6.StylePriority.UseTextAlignment = False
        Me.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify
        Me.XrLabel6.TextFormatString = "{0:dddd}"
        '
        'XrShape2
        '
        Me.XrShape2.BookmarkParent = Me.XrLabel2
        Me.XrShape2.BorderColor = System.Drawing.Color.Transparent
        Me.XrShape2.ForeColor = System.Drawing.Color.DarkOrange
        Me.XrShape2.LineWidth = 3
        Me.XrShape2.LocationFloat = New DevExpress.Utils.PointFloat(11.40905!, 16.66667!)
        Me.XrShape2.Name = "XrShape2"
        ShapeRectangle1.Fillet = 10
        Me.XrShape2.Shape = ShapeRectangle1
        Me.XrShape2.SizeF = New System.Drawing.SizeF(778.2316!, 1067.936!)
        Me.XrShape2.StylePriority.UseBorderColor = False
        Me.XrShape2.StylePriority.UseForeColor = False
        '
        'RPTResignation
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.TopMargin, Me.BottomMargin, Me.Detail, Me.ReportHeader})
        Me.Font = New DevExpress.Drawing.DXFont("Arial", 9.75!)
        Me.Margins = New DevExpress.Drawing.DXMargins(9.0!, 18.0!, 13.54167!, 3.710429!)
        Me.PageHeight = 1169
        Me.PageWidth = 827
        Me.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4
        Me.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes
        Me.RightToLeftLayout = DevExpress.XtraReports.UI.RightToLeftLayout.Yes
        Me.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        Me.Version = "24.1"
        XrWatermark1.Id = "Watermark1"
        Me.Watermarks.AddRange(New DevExpress.XtraPrinting.Drawing.Watermark() {XrWatermark1})
        CType(Me.XrRichText1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents ReportHeader As DevExpress.XtraReports.UI.ReportHeaderBand
    Friend WithEvents XrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrShape2 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrRichText1 As DevExpress.XtraReports.UI.XRRichText
    Friend WithEvents XrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel5 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel20 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrShape4 As DevExpress.XtraReports.UI.XRShape
    Friend WithEvents XrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrPictureBox1 As DevExpress.XtraReports.UI.XRPictureBox
    Friend WithEvents XrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel6 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel11 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel9 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel12 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel13 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel14 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel23 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel21 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel22 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel18 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel19 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel16 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel17 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel25 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel24 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel26 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel27 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel30 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel29 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel28 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents XrLabel31 As DevExpress.XtraReports.UI.XRLabel
End Class
