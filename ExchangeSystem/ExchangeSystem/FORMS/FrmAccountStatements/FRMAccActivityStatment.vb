Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities.IO
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base

Public Class FRMAccActivityStatment
    Private Sub FRMAccActivityStatment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        Code.Text = ""
        GridControl1.DataSource = Nothing
        RBDate.Checked = False
        RBCode.Checked = True
        SumDebit.EditValue = 0.000
        SumCredit.EditValue = 0.000
        BaseType.SelectedIndex = 0
    End Sub

    Private Sub RBCode_CheckedChanged(sender As Object, e As EventArgs) Handles RBCode.CheckedChanged
        If RBCode.Checked = True Then
            Code.Enabled = True
            D1.Enabled = False
            D2.Enabled = False
        Else
            Code.Enabled = False
            D1.Enabled = True
            D2.Enabled = True
        End If
    End Sub

    Private Sub RBDate_CheckedChanged(sender As Object, e As EventArgs) Handles RBDate.CheckedChanged
        If RBDate.Checked = True Then
            Code.Enabled = False
            D1.Enabled = True
            D2.Enabled = True
        Else
            Code.Enabled = True
            D1.Enabled = False
            D2.Enabled = False
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If RBCode.Checked = True Then
            If Code.Text = "" Then
                Code.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
        End If
        If RBDate.Checked = True Then
            If D1.EditValue > D2.EditValue Then
                D1.ErrorText = "لا يمكن أن يكون التاريخ الأول أكبر من التاريخ الثاني"
                Return
            End If
        End If
        GridControl1.DataSource = Nothing
        GridView1.ActiveFilterCriteria = Nothing
        SumDebit.EditValue = 0.000
        SumCredit.EditValue = 0.000
        Dim dt As New DataTable
        Dim Pr(4) As SqlParameter
        Pr(0) = New SqlParameter
        Pr(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = Code.Text.Trim}
        Pr(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        Pr(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Pr(3) = New SqlParameter("@ShearchType", SqlDbType.Bit) With {.Value = RBCode.Checked}
        Pr(4) = New SqlParameter("@BaseType", SqlDbType.Int) With {.Value = SafeToInt(BaseType.SelectedIndex)}
        dt.Clear()
        dt = RUN_QUARY_PRO("AccSafeActivityTb_Statment", Pr)
        If dt.Rows.Count > 0 Then
            GridControl1.DataSource = dt
            SumTotal()
        End If
    End Sub

    Sub SumTotal()
        SumDebit.EditValue = 0.000
        SumCredit.EditValue = 0.000
        If GridView1.RowCount > 0 Then
            Dim SumDeb As New GridColumnSummaryItem()
            SumDeb.SummaryType = SummaryItemType.Sum
            SumDeb.FieldName = "Debit"
            GridView1.Columns("Debit").Summary.Add(SumDeb)
            Dim SumCred As New GridColumnSummaryItem()
            SumCred.SummaryType = SummaryItemType.Sum
            SumCred.FieldName = "Credit"
            GridView1.Columns("Credit").Summary.Add(SumCred)
            SumCredit.EditValue = Convert.ToDouble(GridView1.Columns("Credit").SummaryItem.SummaryValue)
            SumDebit.EditValue = Convert.ToDouble(GridView1.Columns("Debit").SummaryItem.SummaryValue)
        End If
    End Sub

    Private Sub GridView1_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        SumTotal()
    End Sub

    Private Sub GridView1_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView1.ColumnFilterChanged
        SumTotal()
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
End Class