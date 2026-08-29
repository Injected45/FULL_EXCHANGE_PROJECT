Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient

Public Class FRMOWNUSRCURRENCYPRICEDTTELSS
    Public C, t, TypeID As Integer

    Public Sub LOADCIDFROM(IDTYPE As Integer)
        Try
            GCROLEMAIN.DataSource = Nothing
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@TIPE_ID", SqlDbType.Int) With {.Value = IDTYPE}
            Dim DT As New DataTable
            DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOLKP_MAIN", prm)
            If DT.Rows.Count > 0 Then
                GCROLEMAIN.DataSource = DT
                TileView1Format()
                DVGFormat()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطا", ex.Message)
        End Try
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False

        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        'GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        'GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

    End Sub
    Sub TileView1Format()
        TileView1.OptionsBehavior.EditingMode = True
        TileView1.OptionsBehavior.ReadOnly = True
        TileView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        For i As Integer = 0 To TileView1.Columns.Count - 1
            TileView1.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            TileView1.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            TileView1.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            TileView1.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            TileView1.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(2, 84, 100), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub TileView1_DoubleClick(sender As Object, e As EventArgs) Handles TileView1.DoubleClick
        If TileView1.RowCount > 0 Then
            LOADATA(TileView1.GetFocusedRowCellValue("ID"), TileView1.GetFocusedRowCellValue("ITYPE"))
        End If
    End Sub

    Public Sub LOADATA(CurrencyIDFrom As Integer, typeMSGN As Integer)
        Try
            GCROLEDETAILS.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(3) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyIDFrom", SqlDbType.Int) With {.Value = CurrencyIDFrom}
            prm(1) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(2) = New SqlParameter("@typeMSGN", SqlDbType.Int) With {.Value = typeMSGN}
            prm(3) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 2}
            dt = RUN_QUARY_PRO("OwnCurrencyPriceDetailsTb_Grid", prm)
            If dt.Rows.Count > 0 Then
                Me.GCROLEDETAILS.DataSource = dt
                FRMOWNCURRENCYPRICEDETAILS.LabelControl2.Text = prm(1).Value
                If typeMSGN = 1 Then
                    If CurrencyIDFrom = 1 Then
                        GVRole.Columns("cluemnsEdit").Visible = True
                    Else
                        GVRole.Columns("cluemnsEdit").Visible = False
                    End If
                Else
                    GVRole.Columns("cluemnsEdit").Visible = True
                End If
            End If
            C = CurrencyIDFrom
            t = typeMSGN
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطا", ex.Message)
        End Try

    End Sub
    Private Sub RepositoryItemButtonEdit1_DoubleClick(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.DoubleClick
        FRMBTNEOWNCURRENCYEDIT.CurrencyPriceCategory(GVRole.GetFocusedRowCellValue("IDCruns"), GVRole.GetFocusedRowCellValue("Typesd"), 2)
    End Sub
    Public Sub printRPT()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@CurrencyIDFrom", C)
            PRM(1) = New SqlParameter("@MSG", ParameterDirection.Output)
            PRM(2) = New SqlParameter("@typeMSGN", t)
            PRM(3) = New SqlParameter("@TypeID", 2)
            Dim ds As New DataSet
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CurrencyPriceDetailsTb_Grid", PRM)
            dt.TableName = "CurrencyPricesTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTCURRENCYPRICEDTTELSS
                report.DataSource = ds
                report.DataMember = "CurrencyPricesTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطا", ex.Message)
        End Try
    End Sub
End Class