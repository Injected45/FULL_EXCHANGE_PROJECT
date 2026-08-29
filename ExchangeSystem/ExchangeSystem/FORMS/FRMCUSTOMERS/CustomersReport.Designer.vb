Imports System.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI

Partial Class CustomersReport
    Inherits XtraReport

    ' الباندات
    Private Detail As DetailBand
    Private TopMargin As TopMarginBand
    Private BottomMargin As BottomMarginBand
    Private PageHeader As PageHeaderBand
    Private ReportFooter As ReportFooterBand
    Private PageFooter As PageFooterBand

    ' الأدوات
    Private xrTitle As XRLabel
    Private xrLogo As XRPictureBox
    Private xrTable As XRTable
    Private xrApprovalText As XRLabel
    Private xrSignTable As XRTable
    Private xrPageInfo As XRPageInfo

    Public Sub New()
        ' إنشاء الباندات
        Me.Detail = New DetailBand() With {.HeightF = 250}
        Me.TopMargin = New TopMarginBand() With {.HeightF = 50}
        Me.BottomMargin = New BottomMarginBand() With {.HeightF = 50}
        Me.PageHeader = New PageHeaderBand() With {.HeightF = 80}
        Me.ReportFooter = New ReportFooterBand() With {.HeightF = 180}
        Me.PageFooter = New PageFooterBand() With {.HeightF = 30}

        Me.Bands.AddRange(New Band() {Detail, TopMargin, BottomMargin, PageHeader, ReportFooter, PageFooter})

        Me.PaperKind = Printing.PaperKind.A4
        Me.Margins = New Printing.Margins(50, 50, 50, 50)
        Me.RightToLeft = True
        Me.RightToLeftLayout = True

        ' إنشاء الأدوات
        BuildPageHeader()
        BuildDetail()
        BuildReportFooter()
        BuildPageFooter()
    End Sub

    Private Sub BuildPageHeader()
        ' شعار
        xrLogo = New XRPictureBox() With {.BoundsF = New RectangleF(10, 10, 80, 60), .Sizing = ImageSizeMode.ZoomImage}

        ' عنوان
        xrTitle = New XRLabel() With {
            .Text = "نموذج فتح حساب عميل",
            .Font = New Font("Tahoma", 14, FontStyle.Bold),
            .TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
            .BoundsF = New RectangleF(0, 10, 700, 40)
        }

        Me.PageHeader.Controls.AddRange(New XRControl() {xrLogo, xrTitle})
    End Sub

    Private Sub BuildDetail()
        ' جدول البيانات (تضع الحقول لاحقًا في Designer أو ExpressionBindings)
        xrTable = New XRTable() With {.BoundsF = New RectangleF(0, 0, 700, 250), .Borders = DevExpress.XtraPrinting.BorderSide.All, .Font = New Font("Tahoma", 9)}
        xrTable.BeginInit()

        ' أضف صفوف فارغة جاهزة للتعبئة لاحقًا
        For i As Integer = 1 To 10
            Dim row As New XRTableRow()
            Dim cellLabel As New XRTableCell() With {.Text = "اسم الحقل " & i, .Font = New Font("Tahoma", 9, FontStyle.Bold), .WidthF = 200}
            Dim cellValue As New XRTableCell() With {.Text = ""}
            row.Cells.AddRange({cellLabel, cellValue})
            xrTable.Rows.Add(row)
        Next

        xrTable.EndInit()
        Me.Detail.Controls.Add(xrTable)
    End Sub

    Private Sub BuildReportFooter()
        ' نص الإقرار
        xrApprovalText = New XRLabel() With {
            .Text = "أقر أنا المذكور أعلاه بصحة البيانات وأوافق على شروط فتح الحساب وفق أنظمة المصرف.",
            .Multiline = True,
            .BoundsF = New RectangleF(0, 0, 700, 40),
            .TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
            .Font = New Font("Tahoma", 10)
        }

        ' جدول التواقيع
        xrSignTable = New XRTable() With {.BoundsF = New RectangleF(0, 60, 700, 80), .Borders = DevExpress.XtraPrinting.BorderSide.None}
        xrSignTable.BeginInit()

        Dim signRow As New XRTableRow()
        Dim signCells As String() = {"توقيع العميل", "توقيع الموظف", "توقيع المدير"}

        For Each text As String In signCells
            Dim signCell As New XRTableCell() With {.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter}
            Dim line As New XRLine() With {.BoundsF = New RectangleF(0, 0, 200, 20)}
            Dim lbl As New XRLabel() With {.Text = text, .TopF = 25, .WidthF = 200, .TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter, .Font = New Font("Tahoma", 9, FontStyle.Bold)}
            signCell.Controls.Add(line)
            signCell.Controls.Add(lbl)
            signRow.Cells.Add(signCell)
        Next

        xrSignTable.Rows.Add(signRow)
        xrSignTable.EndInit()

        Me.ReportFooter.Controls.AddRange(New XRControl() {xrApprovalText, xrSignTable})
    End Sub

    Private Sub BuildPageFooter()
        xrPageInfo = New XRPageInfo() With {.BoundsF = New RectangleF(0, 0, 200, 30), .PageInfo = DevExpress.XtraPrinting.PageInfo.NumberOfTotal, .TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight}
        Me.PageFooter.Controls.Add(xrPageInfo)
    End Sub

    Friend WithEvents TopMarginBand1 As TopMarginBand
    Friend WithEvents DetailBand1 As DetailBand
    Friend WithEvents BottomMarginBand1 As BottomMarginBand
End Class
