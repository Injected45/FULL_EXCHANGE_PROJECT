Imports DevExpress.XtraGrid.Views.Base



Public Class FRM_Retuns_ueser_Regstir_for_Actvion_Accoun_Rect

    Public Sub new_Recorres()
        New_Controlrs(Me)
        DVGFormat(GVRole)
        GVRole.ShowFindPanel()
        LoadToControlar(GridControl1, "users_Activated_user_accounts_NO", "", "", Nothing, 0)
    End Sub

    Private Sub FRM_Retuns_ueser_Regstir_for_Actvion_Accoun_Rect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        new_Recorres()
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
End Class