Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports System.Data.SqlClient

Public Class FrmNetTotalOFActivityBusiness
    Private Sub FrmProftsOrLossesInComeStatment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GridControl1.DataSource = Nothing
        LoadData()
    End Sub
    Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CONDB_ClosingBusinessActivityLoadDVG")
        If DT.Rows.Count > 0 Then
            GridControl1.DataSource = DT
        End If
    End Sub

    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub RepositoryItemButtonEdit1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonClick
        'Try
        '    Dim reslut1 = XtraMessageBox.Show("في حال الحفظ لايمكنك الرجوع عن العملية هل أنت واثق من الاستمرار؟", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        '    If reslut1 = DialogResult.No Then
        '        Exit Sub
        '    End If
        '    Dim PR(4) As SqlParameter
        '    'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = GridView1.GetFocusedRowCellValue("BName")}
        '    PR(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
        '    PR(1) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = GridView1.GetFocusedRowCellValue("ISID")}
        '    PR(2) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = GridView1.GetFocusedRowCellValue("ID")}
        '    PR(3) = New SqlParameter("@MsgSatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        '    PR(4) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        '    RUN_EXUTE_PRO("ACCOUNTSTB_ProfitaAndLossSharing", PR)
        '    If PR(3).Value = 0 Then
        '        ErrorMessage(Me, "رسالة تنبيه", PR(4).Value)
        '        Exit Sub
        '    End If
        '    CONFIRMMESSAGE.Show()
        '    GridControl1.DataSource = Nothing
        '    LoadData()
        'Catch ex As Exception
        '    ErrorMessage(Me, "رسالة خطأ", ex.Message)
        'End Try
    End Sub


End Class