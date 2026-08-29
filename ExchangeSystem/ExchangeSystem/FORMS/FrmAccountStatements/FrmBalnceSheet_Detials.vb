Imports System.Data.SqlClient
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SelectPdf
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports ExchangeSystem.ExchangeSystem
Imports System.ComponentModel
Imports System.Threading


Public Class FrmBalnceSheet_Detials
    Public Property SplashScreenManager1 As Object

    Sub NewRecord(AccParent As ULong)
        GCRole.DataSource = Nothing
        AccID.Properties.DataSource = Nothing
        AccID.EditValue = -1
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccParent}
        LoadToControlar(AccID, "AccountsTb_LOADINTOLKPBASEDONAccParent_WithoutBranch", "AccName", "AccID", prm)

        DVGFormat(GVRole)
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        GCRole.DataSource = Nothing
        If Not ValidateControl(AccID, "الحساب") Then Exit Sub
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = AccID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}

        LoadToControlar(GCRole, "BalanceSheet_Statment_Detials", "", "", PR)
        AddSerialColumn(GVRole)
        DVGFormat(GVRole)
        SomTotal()
    End Sub

    Sub SomTotal()
        OverAllCredit.EditValue = 0
        OverAllDebit.EditValue = 0
        OverAllTotal1.EditValue = 0
        GridColumnSummaryItem_grivview(GVRole, "Debit", OverAllDebit)
        GridColumnSummaryItem_grivview(GVRole, "Credit", OverAllCredit)
        If GetLKPColumnVal(AccID, "AccDmType") = 1 Then
            OverAllTotal1.EditValue = Val(OverAllCredit.EditValue) - Val(OverAllDebit.EditValue)
        Else
            OverAllTotal1.EditValue = Val(OverAllDebit.EditValue) - Val(OverAllCredit.EditValue)
        End If
    End Sub

    Private Sub AccID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AccID.QueryPopUp
        HideAllColumnsExceptDisplay(AccID)
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Dim dt As New DataTable
        dt.Clear()
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = AccID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        dt = RUN_QUARY_PRO("BalanceSheet_Statment_Detials", PR)
        If dt.Rows.Count > 0 Then
            Dim report As New RPTBalnceSheet_Detials
            report.DataSource = dt
            report.DataMember = "AccSafeActivityTb"
            report.XrLabel4.Text = AccID.Text
            report.D1.Text = D1.Text
            report.D2.Text = D2.Text
            report.XrLabel25.Text = OverAllCredit.Text
            report.XrLabel20.Text = OverAllDebit.Text
            report.OverAllTotal1.Text = OverAllTotal1.Text
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        End If
    End Sub
    Dim report As New RPTBalnceSheet_Detials
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}
    Private Sub SimpleButton112_Click(sender As Object, e As EventArgs) Handles SimpleButton112.Click
        Try
            'SplashScreenManager1.ShowWaitForm()
            Dim dt As New DataTable
            dt.Clear()
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = AccID.EditValue}
            PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            dt = RUN_QUARY_PRO("BalanceSheet_Statment_Detials", PR)
            If dt.Rows.Count > 0 Then
                report.DataSource = dt
                report.DataMember = "AccSafeActivityTb"
                report.XrLabel4.Text = AccID.Text
                report.D1.Text = D1.Text
                report.D2.Text = D2.Text
                report.XrLabel25.Text = OverAllCredit.Text
                report.XrLabel20.Text = OverAllDebit.Text
                report.OverAllTotal1.Text = OverAllTotal1.Text
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                'report.ShowPreview()
                report.ExportToPdf(pdfExportFile, PdfExportOptions)
                MsgBox(GetLKPColumnVal(AccID, "AccPhone"))
                SINTWATSAPP_document(GetLKPColumnVal(AccID, "AccPhone"), pdfExportFile, $"كشف حساب  {AccID.Text} ", " كشف الحساب" & ".pdf")
            End If
            'SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            'SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class