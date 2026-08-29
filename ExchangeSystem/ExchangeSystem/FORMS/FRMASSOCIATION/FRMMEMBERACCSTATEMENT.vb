Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Drawing.Internal.Interop
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SelectPdf

Public Class FRMMEMBERACCSTATEMENT
    Public LOADTYPE As Integer = 2
    Sub NEWRECORD()
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        NEWDVGFROMAT(GVRole)
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        OverAllTotal1.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
    End Sub



    Sub LOADDATA()
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        'GVRole.BeginUpdate()
        NEWDVGFROMAT(GVRole)
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = FRMMEMBERSLOADALL.memberacc}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = FRMMEMBERSLOADALL.D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = FRMMEMBERSLOADALL.D2.EditValue}
        PRM(3) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = 0}
        PRM(4) = New SqlParameter("@LOADTYPE", SqlDbType.Int) With {.Value = 2}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AssActivityTb_GETMEMBER", PRM)
        If DT.Rows.Count > 0 Then
            GVRole.Columns.Clear()
            GCRole.DataSource = DT
            GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
            GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Green
            NEWDVGFROMAT(GVRole)
            GVRole.Columns("#").Width = 70
            GVRole.Columns("طبيعة الحركة").Width = 800
            GVRole.Columns("ملاحظات").Width = 350
        ElseIf DT.Rows.Count = 0 Then
            Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
            Dim lookFeelError As New UserLookAndFeel(Me)
            lookFeelError.Style = LookAndFeelStyle.Skin
            lookFeelError.UseDefaultLookAndFeel = False
            lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها خلال هذه الفترة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            OverAllTotal1.EditValue = 0.000
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
        End If

    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 178, 148), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub
    Private Sub FRMLOADSALARIES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)
        NEWRECORD()
        If GVRole.RowCount > 0 Then
            SumTotal()
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True



        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        'Try


        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = FRMMEMBERSLOADALL.memberacc}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = FRMMEMBERSLOADALL.D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = FRMMEMBERSLOADALL.D2.EditValue}
        PRM(3) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = 0}
        PRM(4) = New SqlParameter("@LOADTYPE", SqlDbType.Int) With {.Value = 2}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AssActivityTb_GETMEMBER", PRM)
        If DT.Rows.Count > 0 Then

            Dim report As New RPTMEMBERACCSTATEMENT

            report.DataSource = DT

            report.DataMember = "AssActivityTb"
            report.FilterString = GVRole.ActiveFilterString
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        Else
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub

    Sub SumTotal()
        If GVRole.RowCount > 0 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "دائن"
            GVRole.Columns("دائن").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "مدين"
            GVRole.Columns("مدين").Summary.Add(DebitSum)
            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("مدين").SummaryItem.SummaryValue)
            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن").SummaryItem.SummaryValue)
            If OverAllCredit.EditValue > OverAllDebit.EditValue Then
                OverAllTotal1.BackColor = Color.Green
            Else
                OverAllTotal1.BackColor = Color.Red
            End If
            OverAllTotal1.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
        End If
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        OverAllTotal1.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        SumTotal()
    End Sub
    Dim report As New RPTMEMBERACCSTATEMENT
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

        SplashScreenManager1.ShowWaitForm()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True

        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = FRMMEMBERSLOADALL.memberacc}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = FRMMEMBERSLOADALL.D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = FRMMEMBERSLOADALL.D2.EditValue}
        PRM(3) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = 0}
        PRM(4) = New SqlParameter("@LOADTYPE", SqlDbType.Int) With {.Value = 2}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AssActivityTb_GETMEMBER", PRM)

        If DT.Rows.Count > 0 Then


            report.DataSource = DT

            report.DataMember = "AssActivityTb"
            report.FilterString = GVRole.ActiveFilterString


            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ExportToPdf(pdfExportFile, PdfExportOptions)
            ' إرسال PDF عبر WhatsApp
            SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(FRMMEMBERSLOADALL.memberacc), pdfExportFile, $"كشف حساب", " كشف الحساب" & ".pdf")
            SplashScreenManager1.CloseWaitForm()
        Else
            SplashScreenManager1.CloseWaitForm()
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class