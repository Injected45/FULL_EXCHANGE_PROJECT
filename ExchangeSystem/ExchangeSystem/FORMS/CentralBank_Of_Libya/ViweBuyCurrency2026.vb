Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid
Public Class ViweBuyCurrency2026
    Public ParentForm As Object

    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            ParentForm.GetRecord(CO)
            'ParentForm.BtnPrint.Enabled = True
        End If
        Me.Close()
    End Sub
End Class