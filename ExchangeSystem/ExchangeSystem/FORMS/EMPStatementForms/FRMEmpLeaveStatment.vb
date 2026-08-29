Imports DevExpress.XtraGrid.Views.Base

Public Class FRMEmpLeaveStatment
    Private Sub FRMEmpLeaveStatment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GCRole.DataSource = Nothing
        LoadToControlar(GCRole, "LeaveTB_LeaveStatment", "", "", Nothing)
        DVGFormat(GVRole)
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
End Class