Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI

Public Class FRMRPTProPettyCashSettlement
    Dim PcVal As Decimal
    Sub LOADDATA(Code As String)
        GCRole.DataSource = Nothing
        PcVal = 0.000
        If PettyCashID.Text = String.Empty Then
            PettyCashID.ErrorText = "يجب اختيار العهدة"
            Return
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_ZRPT_RPTProPettyCashSettlement", PR)
        If DT.Rows.Count > 0 Then
            'MsgBox(DT.Rows(0)("ISID"))
            PcVal = DT.Rows(0)("PCVal")
            SPNPCVAL.EditValue = PcVal
            GCRole.DataSource = DT
            DVGFormat(GVRole)
            'If GVRole.RowCount > 0 Then
            '    GVRole.Columns("ISID").Visible = False
            '    GVRole.Columns("NotesDe").Visible = False
            '    GVRole.Columns("AccIDEX").Visible = False
            '    GVRole.Columns("InsertDate").Visible = False
            '    GVRole.Columns("EMPID").Visible = False
            '    GVRole.Columns("SafeID").Visible = False
            '    GVRole.Columns("CurrencyID").Visible = False
            '    GVRole.Columns("AccName").Visible = False
            '    GVRole.Columns("SettlementVal").Visible = False
            '    GVRole.Columns("NetTotal").Visible = False
            '    GVRole.Columns("AccSafeID").Visible = False
            '    GVRole.Columns("IDCode").Visible = False
            '    GVRole.Columns("AccIDPC").Visible = False
            '    GVRole.Columns("ProName").Visible = False
            'End If
        End If
    End Sub

    Sub LOADPETTYCASH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CONDB_PettyCashTb_LOADTOSTATEMENT")
        If dt.Rows.Count > 0 Then
            PettyCashID.Properties.DataSource = dt
            PettyCashID.Properties.ValueMember = "ISID"
            PettyCashID.Properties.DisplayMember = "ISID"
            PettyCashID.Properties.ShowHeader = False
            PettyCashID.Properties.PopulateColumns()
            PettyCashID.Properties.Columns(0).Width = 170
        End If
    End Sub

    Private Sub FRMRPTProPettyCashSettlement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PettyCashID.EditValue = -1
        LOADPETTYCASH()
        GCRole.DataSource = Nothing
        DVGFormat(GVRole)
        LOADDATA(PettyCashID.Text)
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LOADDATA(PettyCashID.Text)
        SumTotal()
    End Sub

    Private Sub SimpleButton12_Click(sender As Object, e As EventArgs) Handles SimpleButton12.Click
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", PettyCashID.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("CONDB_ZRPT_RPTProPettyCashSettlement", PRM)
            Dim ds As New DataSet
            dt.TableName = "PCSettlementTB"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                        Dim report As New RPTBASEDONPETYCASHPAYMENT
                        report.DataSource = ds
                        report.DataMember = "PCSettlementTB"
                        Dim tool As ReportPrintTool = New ReportPrintTool(report)
                        report.FilterString = GVRole.ActiveFilterString
                        report.XrLabel2.Text = GVRole.FilterPanelText
                        report.CreateDocument()
                        'report.ExportToPdf(pdfExportFile, PdfExportOptions)
                        report.ShowPreview()

                        'If IsUpdate = 0 Then
                        '    ' ارسال التقرير في صورة بي دي اف عبر تطبيق واتساب
                        '    SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(EMPID.EditValue), pdfExportFile, $"تسوية عهدة {EMPID.Text} ", " تسوية عهدة" & ".pdf")
                        'End If
                    End If
        Catch ex As Exception
            ErrorMessage(Me, "رساله تنبية ", ex.Message)
        End Try
    End Sub
    Sub SumTotal()
        OverAllExpens.EditValue = 0.000
        SurplusVal.EditValue = 0.000
        DeserevedVal.EditValue = 0.000
        If GVRole.RowCount > 0 Then
            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "ExVal"
            GVRole.Columns("ExVal").Summary.Add(OverallSalary)

            OverAllExpens.EditValue = 0.000
            OverAllExpens.EditValue = Convert.ToDouble(GVRole.Columns("ExVal").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            '---------------------------------------------------
            If PcVal > OverAllExpens.EditValue Then
                SurplusVal.EditValue = PcVal - OverAllExpens.EditValue
            End If
            If PcVal < OverAllExpens.EditValue Then
                SurplusVal.EditValue = OverAllExpens.EditValue - PcVal
            End If
        End If
    End Sub
    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub
End Class