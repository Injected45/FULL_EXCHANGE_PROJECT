Imports System.Data.SqlClient
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Public Class View_orders
    Private Sub View_orders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEwRecoreds()
    End Sub
    Public Sub NEwRecoreds()
        New_Controlrs(Me)
        GridControl1.DataSource = Nothing
        LoadToControlar(GridControl1, "card_bookings_sELECT", "", "", Nothing)
        DVGFormat2(GridView1)
        GridView1.ShowFindPanel()
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
    Public Sub card_bookings_UPDATE_CANCEL(stutes As Integer)
        Try
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = GridView1.GetFocusedRowCellValue("ID")}
            PR(1) = New SqlParameter("@MSGSTUTE", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PR(2) = New SqlParameter("@MASGEBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PR(3) = New SqlParameter("@stutes", SqlDbType.Int) With {.Value = stutes}
            PR(4) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = UserID}
            RUN_QUARY_PRO("card_bookings_UPDATE_CANCEL", PR)
            If PR(1).Value = 0 Then
                MessageBox.Show(PR(3).Value, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                NEwRecoreds()
            End If
            FrmSavedSuccessfully.ShowDialog()
            If stutes = 1 Then
                card_bookings_sELECprint(GridView1.GetFocusedRowCellValue("ID"))
            End If
            NEwRecoreds()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnok_Click(sender As Object, e As EventArgs) Handles btnok.Click
        card_bookings_UPDATE_CANCEL(1)
    End Sub
    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        card_bookings_UPDATE_CANCEL(2)
    End Sub
    Public Sub DVGFormat2(GVRole As GridView)
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = False
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = False
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Public Sub card_bookings_sELECprint(ID As ULong)

        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = ID}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("card_bookings_sELECprint", prm)
            If dt.Rows.Count > 0 Then
                Dim rpt As New rpt
                rpt.DataSource = dt
                Dim tool As ReportPrintTool = New ReportPrintTool(rpt)
                rpt.CreateDocument()
                'rpt.ShowPreview()
                rpt.Print()
            Else
                MessageBox.Show("لا يوجد بيانات للطباعة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        card_bookings_sELECprint(GridView1.GetFocusedRowCellValue("ID"))
    End Sub
End Class