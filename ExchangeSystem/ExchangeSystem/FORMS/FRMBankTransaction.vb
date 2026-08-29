Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI

Public Class FRMBankTransaction
    Private Sub FRMBankTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        New_Controlrs(Me)
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverallVal1.EditValue = 0
        ExValtotal.EditValue = 0
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        OverallVal1.EditValue = 0
        ExValtotal.EditValue = 0
        Dim prm(2) As SqlParameter
        prm(0) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TransType.SelectedIndex}
        prm(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        prm(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        LoadToControlar(GCRole, "BankTransaction_Stetment", "", "", prm)
        GVRole.IndicatorWidth = 50 ' عرض العمود الجانبي
        AddHandler GVRole.CustomDrawRowIndicator,
            Sub(s2, e2)
                If e2.Info.Kind = DevExpress.Utils.Drawing.IndicatorKind.Row AndAlso e2.RowHandle >= 0 Then
                    e2.Info.DisplayText = (e2.RowHandle + 1).ToString()
                End If
            End Sub
        DVGFormat(GVRole)
        GridColumnSummaryItem_grivview(GVRole, "القيمة", OverallVal1)
        GridColumnSummaryItem_grivview(GVRole, "العمولة", ExValtotal)
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim prm(2) As SqlParameter
        prm(0) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TransType.SelectedIndex}
        prm(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        prm(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim RPTName As String
        If TransType.SelectedIndex = 0 Then
            RPTName = "RPTBankTransaction"
        ElseIf TransType.SelectedIndex = 1 Then
            RPTName = "RPTBankTransaction1"
        ElseIf TransType.SelectedIndex = 2 Then
            RPTName = "RPTBankTransaction2"
        Else
            RPTName = ""
        End If
        LoadToPrint("BankTransaction_Stetment", "BankDipWdTb", RPTName, prm, GVRole.ActiveFilterString)
    End Sub

    Private Sub TransType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TransType.SelectedIndexChanged
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        OverallVal1.EditValue = 0
        ExValtotal.EditValue = 0
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        GridColumnSummaryItem_grivview(GVRole, "القيمة", OverallVal1)
        GridColumnSummaryItem_grivview(GVRole, "العمولة", ExValtotal)
    End Sub
End Class