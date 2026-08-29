Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FrmInternalEx_getinsert_DailyCount
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        SplashScreenManager1.ShowWaitForm()
        over_vall_count.Text = 0
        commint_count.Text = 0
        Dyle_count.Text = 0
        Wyke_count.Text = 0
        yer_count.Text = 0
        If TypeISint.SelectedIndex = -1 Then
            TypeISint.ErrorText = "هذه الحقل مطلوب"
            SplashScreenManager1.CloseWaitForm()
            Return
        End If

        If branchID.EditValue = -1 Or branchID.Text = String.Empty Then
            branchID.ErrorText = "هذه الحقل مطلوب"
            SplashScreenManager1.CloseWaitForm()
            Return
        End If
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@TypeISint", SqlDbType.Int) With {.Value = TypeISint.SelectedIndex}
        prm(1) = New SqlParameter("@BBRANCHID", SqlDbType.Int) With {.Value = branchID.EditValue}
        GridControl2.DataSource = Nothing
        LoadToControlar(GridControl2, "InternalEx_getinsert_DailyCount", "", "", prm)

        DVGFormat(GridView2)
        SplashScreenManager1.CloseWaitForm()
        summerDate_grid()
    End Sub

    Public Sub summerDate_grid()
        GridColumnSummaryItem_grivview(GridView2, "DailyCount", Dyle_count)
        GridColumnSummaryItem_grivview(GridView2, "WeeklyCount", Wyke_count)
        GridColumnSummaryItem_grivview(GridView2, "MonthlyCount", MonthY_count)
        GridColumnSummaryItem_grivview(GridView2, "YearlyCount", yer_count)
        GridColumnSummaryItem_grivview(GridView2, "TotalExVal", commint_count)
        GridColumnSummaryItem_grivview(GridView2, "TotalOverallVal", over_vall_count)
    End Sub
    Private Sub FrmInternalEx_getinsert_DailyCount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nower()
        LoadToControlar(branchID, "Lode_bransas", "BName", "ID", Nothing)
    End Sub
    Public Sub nower()
        New_Controlrs(Me)
        over_vall_count.Text = 0
        commint_count.Text = 0
        Dyle_count.Text = 0
        Wyke_count.Text = 0
        yer_count.Text = 0

    End Sub
    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView2.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click

        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@TypeISint", TypeISint.SelectedIndex)
            prm(1) = New SqlParameter("@BBRANCHID", branchID.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("InternalEx_getinsert_DailyCount", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New XtraReportapp
                report.OverAllTotal1.Text = TypeISint.Text
                report.XrLabel5.Text = branchID.Text
                report.XrLabel8.Text = GetUserName
                report.XrLabel9.Text = Dyle_count.Text
                report.XrLabel17.Text = Wyke_count.Text
                report.XrLabel18.Text = MonthY_count.Text
                report.XrLabel19.Text = yer_count.Text
                report.XrLabel20.Text = commint_count.Text
                report.XrLabel21.Text = over_vall_count.Text
                dt.TableName = "InternalEx_getinsert_DailyCount"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                report.DataSource = ds
                report.DataMember = "InternalEx_getinsert_DailyCount"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()

                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub

    Private Sub GridView2_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView2.CustomDrawColumnHeader
        For Each column As GridColumn In GridView2.Columns
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

    Private Sub GridView2_DoubleClick(sender As Object, e As EventArgs) Handles GridView2.DoubleClick
        Dim Ctid As String = GridView2.GetFocusedRowCellValue("CiteID")
        Dim Cti_name As String = GridView2.GetFocusedRowCellValue("CityName")
        If Ctid = Nothing Then Return
        If TypeISint.SelectedIndex = -1 Then
            TypeISint.ErrorText = "هذه لحقل مطلوب"
            Return
        End If
        FRM_GET_deteelsForMobile.GET_deteelsForMobile(TypeISint.SelectedIndex, Ctid, branchID.EditValue, Cti_name, branchID.Text)
    End Sub
End Class