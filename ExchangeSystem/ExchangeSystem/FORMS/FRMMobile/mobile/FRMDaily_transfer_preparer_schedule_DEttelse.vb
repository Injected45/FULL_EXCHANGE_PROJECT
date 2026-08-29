Imports System.Data.SqlClient
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMDaily_transfer_preparer_schedule_DEttelse


    Public Sub lodedate(ueserType As ULong)
        DVGFormat(GridView1)
        GridView1.ShowFindPanel()
        New_Controlrs(Me)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ueserType", SqlDbType.Int) With {.Value = ueserType}
        LoadToControlar(GridControl1, "Daily_transfer_preparer_schedule_DEttelse_Get", "", "", prm)
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
        For Each column As GridColumn In GridView1.Columns
            column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
            column.OptionsFilter.AllowAutoFilter = False
            column.OptionsFilter.AllowFilter = False
            column.OptionsColumn.AllowMove = False
            column.OptionsColumn.AllowSize = False
            column.OptionsColumn.ReadOnly = True

        Next

        If e.Column Is Nothing Then
            Return
        End If

        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(64, 64, 64), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)

        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub RepositoryItemButtonEdit1_Click(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.Click
        Dim findueserTyp As New Object
        findueserTyp = GridView1.GetFocusedRowCellValue("ACCID")
        If findueserTyp IsNot Nothing AndAlso Not IsDBNull(findueserTyp) AndAlso findueserTyp.ToString() <> String.Empty Then
            Daily_transfer_preparer_schedule_DEttelse_GETUESER.lod_data(findueserTyp)
            Daily_transfer_preparer_schedule_DEttelse_GETUESER.ShowDialog()
        End If

    End Sub
End Class