Imports System.Data.SqlClient

Public Class FRMSETTLEMENTSTATEMENTDETAILS
    Sub LOADDATA()
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FRMSETTLEMENTSTATEMENT.BranchID.EditValue}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = FRMSETTLEMENTSTATEMENT.D1.EditValue}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = FRMSETTLEMENTSTATEMENT.D2.EditValue}
        PR(3) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = FRMSETTLEMENTSTATEMENT.GVRole.GetFocusedRowCellValue("الرمز").ToString}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccSafeActivityTb_GETPCEMPIDDETAILS", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("طبيعة الحركة").Width = 200
            GVRole.Columns("ملاحظات").Width = 200
            NEWDVGFROMAT(GVRole)
        End If
    End Sub

    Private Sub FRMSETTLEMENTSTATEMENTDETAILS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADDATA()
    End Sub
End Class