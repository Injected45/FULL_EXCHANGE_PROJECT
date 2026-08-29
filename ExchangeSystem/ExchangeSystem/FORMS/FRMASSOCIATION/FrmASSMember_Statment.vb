Imports System.Data.SqlClient
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities.IO
Imports DevExpress.XtraLayout
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSpellChecker

Public Class FrmASSMember_Statment
    Sub NewRecord()
        GCRole.DataSource = Nothing
        AssID.Properties.DataSource = Nothing
        AssID.EditValue = -1
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        LoadToControlar(AssID, "ASSOCIATIONNAMETB_LOADTODVG", "ASSNAME", "ID", Nothing)
        DVGFormat(GVRole)
    End Sub
    Private Sub AssID_TextChanged(sender As Object, e As EventArgs) Handles AssID.TextChanged, AssID.EditValueChanged
        MemberAccID.Properties.DataSource = Nothing
        MemberAccID.EditValue = -1
        If AssID.EditValue <> -1 Or AssID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = AssID.EditValue}
            LoadToControlar(MemberAccID, "ASSOCIATIONTB_LOADBASEDONASSID", "MEMBERNAME", "AccID", PR)
        End If
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        GCRole.DataSource = Nothing
        If Not ValidateControl(AssID, "الجمعية") Then Exit Sub
        If Not ValidateControl(MemberAccID, "المشترك") Then Exit Sub
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = MemberAccID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}

        LoadToControlar(GCRole, "ASSMember_Statment", "", "", PR)
        AddSerialColumn(GVRole)
        DVGFormat(GVRole)
        SomTotal()
    End Sub

    Private Sub FrmASSMember_Statment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub

    Sub SomTotal()
        OverAllCredit.EditValue = 0
        OverAllDebit.EditValue = 0
        OverAllTotal1.EditValue = 0
        GridColumnSummaryItem_grivview(GVRole, "Debit", OverAllDebit)
        GridColumnSummaryItem_grivview(GVRole, "Credit", OverAllCredit)
        OverAllTotal1.EditValue = Val(OverAllCredit.EditValue) - Val(OverAllDebit.EditValue)
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Dim dt As New DataTable
        dt.Clear()
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = MemberAccID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        dt = RUN_QUARY_PRO("ASSMember_Statment", PR)
        If dt.Rows.Count > 0 Then
            Dim report As New RPTASSMember_Statment
            report.DataSource = dt
            report.DataMember = "AccSafeActivityTb"
            report.XrLabel4.Text = MemberAccID.Text
            report.D1.Text = D1.Text
            report.D2.Text = D2.Text
            report.XrLabel6.Text = AssID.Text
            report.XrLabel25.Text = OverAllDebit.Text
            report.XrLabel20.Text = OverAllCredit.Text
            report.OverAllTotal1.Text = OverAllTotal1.Text
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        End If
    End Sub
End Class