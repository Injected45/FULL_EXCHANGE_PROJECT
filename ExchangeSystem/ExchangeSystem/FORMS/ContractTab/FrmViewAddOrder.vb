Imports System.Data.SqlClient

Public Class FrmViewAddOrder
    Private Sub FrmViewAddOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GCRole.DataSource = Nothing
        OrderType.EditValue = -1
        LoadToControlar(OrderType, "CONDB_OrderTB_LoadParentToLKP", "AccName", "AccCode", Nothing)
    End Sub

    Private Sub OrderType_TextChanged(sender As Object, e As EventArgs) Handles OrderType.TextChanged
        GCRole.DataSource = Nothing
        If OrderType.Text <> String.Empty Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = GetLKPColumnVal(OrderType, "AccCode")}
            LoadToControlar(GCRole, "CONDB_OrderTb_LoadToDVG", "", "", PRM)
        End If
    End Sub
End Class