Imports DevExpress.Data
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmBBranchBalncesStatment
    Sub LoadData()
        GridControl2.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("BBranchTb_BalncesStatment")
        If DT.Rows.Count > 0 Then
            GridControl2.DataSource = DT
            DVGFormat(GVRole)
        End If
    End Sub
    Sub Sumtotal1()
        TextEdit22.EditValue = 0.000
        Try
            If GVRole.RowCount > 0 Then
                Dim NetTotal As New GridColumnSummaryItem()
                NetTotal.SummaryType = SummaryItemType.Sum
                NetTotal.FieldName = "NetTotal"
                GVRole.Columns("NetTotal").Summary.Add(NetTotal)
                TextEdit22.EditValue = Convert.ToDouble(GVRole.Columns("NetTotal").SummaryItem.SummaryValue)

                If TextEdit22.EditValue > 0 Then
                    TextEdit22.BackColor = Color.Green
                ElseIf TextEdit22.EditValue < 0 Then
                    TextEdit22.BackColor = Color.Red
                Else
                    TextEdit22.BackColor = Color.Blue
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        Try
            If GVRole.RowCount > 0 Then
                Dim View As GridView = TryCast(sender, GridView)
                If e.Column.FieldName = "NetTotal" Then
                    Dim _length As String = CStr(e.CellValue)
                    If _length >= 0 Then
                        e.Appearance.ForeColor = Color.White
                        e.Appearance.BackColor = Color.Green
                    End If
                End If
                If e.Column.FieldName = "NetTotal" Then
                    Dim _length As String = CStr(e.CellValue)
                    If _length < 0 Then
                        e.Appearance.ForeColor = Color.White
                        e.Appearance.BackColor = Color.Red
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged, GVRole.FocusedRowChanged
        Sumtotal1()
    End Sub

    Private Sub FrmBBranchBalncesStatment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
End Class