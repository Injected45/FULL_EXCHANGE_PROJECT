Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Public Class ProjectsStetment
    Private Sub GridView1_CustomUnboundColumnData_1(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Sub DVGFormat(GridView11 As GridView)
        Dim gvrolls As New GridView
        gvrolls = GridView11
        gvrolls.OptionsBehavior.EditingMode = True
        gvrolls.OptionsBehavior.ReadOnly = True
        gvrolls.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        gvrolls.OptionsView.ShowGroupPanel = False
        gvrolls.OptionsFind.AlwaysVisible = True
        gvrolls.ShowFindPanel()
        gvrolls.OptionsView.ShowFooter = False
        For i As Integer = 0 To GridView11.Columns.Count - 1
            gvrolls.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        gvrolls.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        gvrolls.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        gvrolls.OptionsView.EnableAppearanceEvenRow = True
        gvrolls.Appearance.OddRow.BackColor = Color.WhiteSmoke
        gvrolls.OptionsView.EnableAppearanceOddRow = True

        ' GridLocalizer.Active = New MyGridLocalizer()


    End Sub

    Private Sub FRMALLDebtorsMovment_Load(sender As Object, e As EventArgs) Handles Me.Load
        GridControl1.DataSource = Nothing
        GridView1.OptionsView.ShowGroupPanel = False
        ProjectAccountStatement()
    End Sub

    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
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
    Public Sub ProjectAccountStatement()
        Try
            GridControl1.DataSource = Nothing
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            OverAllTotal1.EditValue = 0.000
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO_ONLY("CONDB_ProjectAccountStatement")
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
                Sumtotal()
                dt.Dispose()
            Else
                MessageBox.Show("عذرا لايوجد بيانات في الوقت الحالي", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName = "ACCNETtotel" Then
            Dim _length As String = CStr(e.CellValue)
            If _length <= 0 Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Red
            Else
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Green
            End If
        End If
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        If GridView1.RowCount > 0 Then
            FrmProjectExpenssMovment.LOADBRANCH()
            FrmProjectExpenssMovment.ProjectID.EditValue = GridView1.GetFocusedRowCellValue("AccID")
            FrmProjectExpenssMovment.NewRecord()
            FrmProjectExpenssMovment.LOADDATA()
            FrmProjectExpenssMovment.SumTotal()
            FrmProjectExpenssMovment.ShowDialog()
        End If
    End Sub

    Sub Sumtotal()
        OverAllDebit.EditValue = 0
        OverAllCredit.EditValue = 0
        OverAllTotal1.EditValue = 0
        If GridView1.RowCount > 0 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "Credit"
            GridView1.Columns("Credit").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "Debit"
            Dim Projectvalue As New GridColumnSummaryItem()
            Projectvalue.SummaryType = SummaryItemType.Sum
            Projectvalue.FieldName = "Projectvalue"
            GridView1.Columns("Debit").Summary.Add(DebitSum)
            GridView1.Columns("Projectvalue").Summary.Add(Projectvalue)
            OverAllDebit.EditValue = GridView1.Columns("Debit").SummaryItem.SummaryValue
            OverAllDebit.Properties.Appearance.BackColor = Color.Red
            OverAllCredit.EditValue = GridView1.Columns("Credit").SummaryItem.SummaryValue
            TotalProjectvalues.EditValue = GridView1.Columns("Projectvalue").SummaryItem.SummaryValue
            OverAllTotal1.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
            If OverAllTotal1.EditValue <= 0 Then
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            Else
                OverAllDebit.Properties.Appearance.BackColor = Color.Green
            End If
        End If
    End Sub
    Private Sub GridView1_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        Sumtotal()
    End Sub

    Private Sub GridView1_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView1.ColumnFilterChanged
        Sumtotal()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("CONDB_ProjectAccountStatement")
        If dt.Rows.Count > 0 Then
            Dim report As New RPTProjectsStetment
            report.DataSource = dt
            report.DataMember = "AccountsTb"
            If OverAllTotal1.EditValue <= 0 Then
                'report.XrPictureBox2.Image = My.Resources.R_dollar
            Else
                'report.XrPictureBox2.Image = My.Resources.G_dollar
            End If
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.FilterString = GridView1.ActiveFilterString
            report.CreateDocument()
            report.ShowPreview()
        Else
            MessageBox.Show("عذرا لايوجد بيانات لطباعتها", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class