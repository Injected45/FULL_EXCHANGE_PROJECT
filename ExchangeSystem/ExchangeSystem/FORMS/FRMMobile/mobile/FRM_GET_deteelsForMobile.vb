Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Public Class FRM_GET_deteelsForMobile
    Public Tipe_Rpint, ctieID_print, brcnchID_print As Integer
    Public branch_namee, ctnamee As String

    Public Sub GET_deteelsForMobile(Type_from As ULong, ctieID As ULong, brcnchID As ULong, cteName As String, brcnch_name As String)
        Try

            SplashScreenManager1.ShowWaitForm()
            New_Controlrs(Me)
            GridControl1.DataSource = Nothing
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@type", SqlDbType.Int) With {.Value = Type_from}
            prm(1) = New SqlParameter("@cties", SqlDbType.Int) With {.Value = ctieID}
            prm(2) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = brcnchID}
            Tipe_Rpint = Type_from
            ctieID_print = ctieID
            brcnchID_print = brcnchID
            branch_namee = brcnch_name
            ctnamee = cteName


            LoadToControlar(GridControl1, "GET_deteelsForMobile", "", "", prm)
            cety_name.Text = cteName
            brcnch_name_Txt.Text = brcnch_name
            DVGFormat(GridView1)
            GridView1.ShowFindPanel()
            sumDate()
            Me.ShowDialog()
            SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Public Sub sumDate()
        GridColumnSummaryItem_grivview(GridView1, "ExVal", commint_count)
        GridColumnSummaryItem_grivview(GridView1, "OverallVal", totla_over)
        SimpleLabelItem3.Text = GridView1.RowCount
    End Sub


    Private Sub PictureEdit1_Click(sender As Object, e As EventArgs) Handles PictureEdit1.Click
        Try

            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@type", SqlDbType.Int) With {.Value = Tipe_Rpint}
            prm(1) = New SqlParameter("@cties", SqlDbType.Int) With {.Value = ctieID_print}
            prm(2) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = brcnchID_print}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("GET_deteelsForMobile", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New XtraReport2dele
                report.DataSource = dt
                report.DataMember = "GET_deteelsForMobile"
                report.XrLabel5.Text = brcnch_name_Txt.Text
                report.OverAllTotal1.Text = cety_name.Text
                report.XrLabel8.Text = GetUserName

                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If


        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub

    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If

    End Sub
    Private Sub GridView2_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
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


End Class