Imports System.Data.SqlClient
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRM_Lde_View_orders


    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        card_bookings_sELECprint(GridView1.GetFocusedRowCellValue("ID"))
    End Sub

    Public Sub card_bookings_sELECprint(ID As ULong)

        Try

            SplashScreenManager1.ShowWaitForm()
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
                SplashScreenManager1.CloseWaitForm()
                rpt.ShowPreview()

            Else
                SplashScreenManager1.CloseWaitForm()
                MessageBox.Show("لا يوجد بيانات للطباعة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Sub DVGFormat2(GVRole As GridView)
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = False
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.ShowFindPanel()

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
    Public Sub NEwRecoreds()
        New_Controlrs(Me)
        GridControl1.DataSource = Nothing

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

    Public Sub card_bookings_SELECT_date()
        Try
            SplashScreenManager1.ShowWaitForm()
            If TextEdit1.EditValue > TextEdit2.EditValue Then
                TextEdit1.ErrorText = "عذرا لايمكن ان يكون التاريخ الاول اكبر من التاريخ الثاني"
                TextEdit1.Select()
                Return
            End If
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Date1", SqlDbType.Date) With {.Value = TextEdit1.EditValue}
            prm(1) = New SqlParameter("@Date2", SqlDbType.Date) With {.Value = TextEdit2.EditValue}
            LoadToControlar(GridControl1, "card_bookings_SELECT_date", "", "", prm)
            DVGFormat2(GridView1)
            SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
        End Try

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        card_bookings_SELECT_date()
    End Sub

    Private Sub FRM_Lde_View_orders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextEdit1.EditValue = Date.Now
        TextEdit2.EditValue = Date.Now
        DVGFormat2(GridView1)
    End Sub

    Private Sub FRM_Lde_View_orders_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub
End Class