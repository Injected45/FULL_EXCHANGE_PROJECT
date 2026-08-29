Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMVIEWPI
    Public Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("PayIncrease_LoadDataIntoDataGridview")
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFormat(GVRole)
        End If
    End Sub

    Private Sub FRMVIEWPI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            FrmPayIncrease.IsUpdate = True
            FrmPayIncrease.BtnSave.Enabled = False
            FrmPayIncrease.BtnEdit.Enabled = True
            FrmPayIncrease.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FrmPayIncrease.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FrmPayIncrease.SHOW_DATA(CO)
        End If
        Me.Close()



    End Sub
End Class