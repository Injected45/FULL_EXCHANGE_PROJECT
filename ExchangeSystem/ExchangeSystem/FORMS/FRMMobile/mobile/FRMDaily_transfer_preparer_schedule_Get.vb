Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMDaily_transfer_preparer_schedule_Get
    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        XtraForm2.BtnNew.PerformClick()
        XtraForm2.ShowDialog()
    End Sub
    Public Sub lodedate()
        DVGFormat(GridView1)
        GridView1.ShowFindPanel()
        New_Controlrs(Me)
        LoadToControlar(GridControl1, "Daily_transfer_preparer_schedule_fillGridViesw", "", "", Nothing)
    End Sub
    Private Sub FRMDaily_transfer_preparer_schedule_Get_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodedate()
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
    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Try


            Dim findueserTyp As New Object
            findueserTyp = GridView1.GetFocusedRowCellValue("ueserTyp")
            If findueserTyp IsNot Nothing AndAlso Not IsDBNull(findueserTyp) AndAlso findueserTyp.ToString() <> String.Empty Then
                XtraForm2.lod_data(findueserTyp)
                XtraForm2.ShowDialog()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RepositoryItemButtonEdit1_Click(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.Click
        Try


            Dim findueserTyp As New Object
            findueserTyp = GridView1.GetFocusedRowCellValue("ueserTyp")
            If findueserTyp IsNot Nothing AndAlso Not IsDBNull(findueserTyp) AndAlso findueserTyp.ToString() <> String.Empty Then
                FRMDaily_transfer_preparer_schedule_DEttelse.lodedate(findueserTyp)
                FRMDaily_transfer_preparer_schedule_DEttelse.ShowDialog()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class