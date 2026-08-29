Imports System.Data.SqlClient
Imports DevExpress.CodeParser
Imports DevExpress.Data
Imports DevExpress.XtraGrid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSplashScreen
Imports SelectPdf

Public Class FrmSettlement_with_Agent
    Public Sub NEwRecoreds()
        AgentID.EditValue = -1
        CountryID.EditValue = -1
        D1.EditValue = Now.Date
        D2.EditValue = Now.Date
        GridControl1.DataSource = Nothing
        LoadToControlar(AgentID, "CoBranches_LoadAgent", "BName", "DBRID", Nothing)
        LoadToControlar(CountryID, "CountriesTb_LoadToLKP", "CName", "ID", Nothing)
        DVGFormat(GVRole1)
    End Sub

    Private Sub FrmSettlement_with_Agent_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEwRecoreds()
    End Sub
    Dim report As New RPT_serrlemnt_with_agent
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}
    Public Sub LoadData()
        If Not ValidateControl(AgentID, "اسم الوكيل") Then Exit Sub
        If Not ValidateControl(CountryID, "مكان التسليم") Then Exit Sub
        If D1.Text = String.Empty Then
            D1.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If


        If D2.Text = String.Empty Then
            D2.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If

        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "عذراً لايمكن ان يكون التاريخ الاول اكبر من تاريخ الثاني"
            Exit Sub
        End If

        GridControl1.DataSource = Nothing
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = AgentID.EditValue}
        PR(1) = New SqlParameter("@CountryTo", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PR(2) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(3) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("Settlement_with_Agent", PR)
        If DT.Rows.Count > 0 Then
            GridControl1.DataSource = DT
            NEWDVGFROMAT(GVRole1)
            SumTotal()
        End If


    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        LoadData()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Try
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = AgentID.EditValue}
            PR(1) = New SqlParameter("@CountryTo", SqlDbType.Int) With {.Value = CountryID.EditValue}
            PR(2) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(3) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}

            Dim dt As New DataTable
            dt = RUN_QUARY_PRO("Settlement_with_Agent", PR)

            If dt.Rows.Count > 0 Then
                Dim report As New RPT_serrlemnt_with_agent
                report.DataSource = dt
                report.DataMember = "Settlement_with_Agent"
                report.agentname.Text = AgentID.Text
                report.XrLabel13.Text = CountryID.Text
                report.XrLabel25.Text = OverallDepit.Text
                report.XrLabel23.Text = OverallCredit.Text
                report.D1.Text = D1.Text
                report.D2.Text = D2.Text

                Dim tool As New ReportPrintTool(report)
                report.CreateDocument()
                report.ExportToPdf(pdfExportFile, PdfExportOptions)

                SINTWATSAPP_document(
            get_gruop_id(AgentID.EditValue),
            pdfExportFile,
            $"كشف الاقفالات اليومية {AgentID.Text}",
            "كشف الاقفالات اليومية.pdf"
        )

                MessageBox.Show(
            "تم إرسال الكشف بنجاح ✅",
            "تأكيد",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )
            End If

            SQLCON.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Sub SumTotal()
        OverallDepit.EditValue = 0.000
        OverallCredit.EditValue = 0.000
        GridColumnSummaryItem_grivview(GVRole1, "NewCurrRecievedVal", OverallCredit)
        GridColumnSummaryItem_grivview(GVRole1, "SumNewFinalTotal", OverallDepit)

    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        If GVRole1.RowCount = 0 Then
            ErrorMessage(Me, "رسالة خطأ", "لا يوجد بيانات لطباعتها")
            Exit Sub
        End If
        Try
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = AgentID.EditValue}
            PR(1) = New SqlParameter("@CountryTo", SqlDbType.Int) With {.Value = CountryID.EditValue}
            PR(2) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(3) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("Settlement_with_Agent", PR)
            If dt.Rows.Count > 0 Then
                Dim report As New RPT_serrlemnt_with_agent
                report.DataSource = dt
                report.DataMember = "Settlement_with_Agent"
                report.agentname.Text = AgentID.Text
                report.XrLabel13.Text = CountryID.Text
                report.XrLabel25.Text = OverallDepit.Text
                report.XrLabel23.Text = OverallCredit.Text
                report.D1.Text = D1.Text
                report.D2.Text = D2.Text

                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
            SQLCON.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub AgentID_EditValueChanged(sender As Object, e As EventArgs) Handles AgentID.EditValueChanged

        CountryID.EditValue = Nothing
        GridControl1.DataSource = Nothing
        OverallCredit.EditValue = 0.000
        OverallDepit.EditValue = 0.000

    End Sub
End Class