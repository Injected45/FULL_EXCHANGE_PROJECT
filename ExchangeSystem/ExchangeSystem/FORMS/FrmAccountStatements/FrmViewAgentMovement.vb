Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports ExchangeSystem.ExchangeSystem
Imports SelectPdf
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmViewAgentMovement
    Public IsValIn As Boolean
    Public NETtotal, Peroid, PBalance As Integer
    Private _Helper As MyCellMergeHelper
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsSelection.EnableAppearanceHotTrackedRow = DefaultBoolean.False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = DefaultBoolean.False
        GVRole.OptionsSelection.EnableAppearanceFocusedCell = DefaultBoolean.False
        GVRole.OptionsSelection.EnableAppearanceFocusedCell = DefaultBoolean.False
        GVRole.OptionsSelection.MultiSelect = False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 11, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        Next
    End Sub
    Sub NewRecord()
        BranchToLKP()
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        NEWDVGFROMAT(GVRole)
        LOADCIDFROMT()
        If Application.OpenForms().OfType(Of FRMALLAgentMovment).Any = False Then
            BranchID.EditValue = -1
        End If
        CurrencyTo.EditValue = 1
    End Sub
    Sub BranchToLKP()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadAgent")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LoadData()
        If D1.EditValue > D2.EditValue Then
            MessageBox.Show("تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If
        If CurrencyTo.EditValue = -1 Then
            CurrencyTo.ErrorText = "يجب اختيار العملة"
            Return
        End If

        Dim PRM(6) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 18, .Scale = 3, .Direction = ParameterDirection.Output}
        PRM(4) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 18, .Scale = 3, .Direction = ParameterDirection.Output}
        PRM(5) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 18, .Scale = 3, .Direction = ParameterDirection.Output}
        PRM(6) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccSafeActivityTb_SelectByAgent", PRM)
        If DT.Rows.Count = 0 Then
            IsValIn = 1
            Dim PRM1(4) As SqlParameter
            PRM1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PRM1(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PRM1(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
            PRM1(3) = New SqlParameter("@PeroidValue", SqlDbType.Decimal) With {.Precision = 18, .Scale = 3, .Direction = ParameterDirection.Output}
            PRM1(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim DT1 As New DataTable
            DT1.Clear()
            DT1 = RUN_QUARY_PRO("AccSafeActivityTb_SelectPreviewByAgent", PRM1)
            If DT1.Rows.Count > 0 Then
                GCRole.DataSource = DT1
                OverAllDebit.EditValue = 0.000
                OverAllCredit.EditValue = 0.000
                NEWDVGFROMAT(GVRole)
                PreBalance.EditValue = PRM1(3).Value
                If PRM1(3).Value < 0.000 Then
                    PBalance = -1
                    PreBalance.BackColor = Color.FromArgb(211, 34, 52)
                ElseIf PRM1(3).Value >= 0.000 Then
                    PBalance = 1
                    PreBalance.BackColor = Color.FromArgb(28, 175, 87)
                End If
            End If
            Dim PR1(1) As SqlParameter
            PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR1(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim DT2 As New DataTable
            DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETAGENTTOTAL", PR1)
            If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                NETtotal = -1
                OverAllNetTotal.EditValue = DT2.Rows(0)("أول")
                OverAllNetTotal.BackColor = Color.FromArgb(211, 34, 52)
            Else
                NETtotal = 1
                OverAllNetTotal.EditValue = DT2.Rows(0)("ثاني")
                OverAllNetTotal.BackColor = Color.FromArgb(28, 175, 87)
            End If
        ElseIf DT.Rows.Count > 0 Then
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            IsValIn = 0
            GCRole.DataSource = DT
            Dim PRM1(4) As SqlParameter
            PRM1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PRM1(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PRM1(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
            PRM1(3) = New SqlParameter("@PeroidValue", SqlDbType.Decimal) With {.Precision = 18, .Scale = 3, .Direction = ParameterDirection.Output}
            PRM1(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim DT1 As New DataTable
            DT1.Clear()
            DT1 = RUN_QUARY_PRO("AccSafeActivityTb_SelectPreviewByAgent", PRM1)
            Dim rDt As DataTable = TryCast(GCRole.DataSource, DataTable)
            _Helper = New MyCellMergeHelper(GVRole)
            Dim row As DataRow = rDt.NewRow()
            row("رمز الحوالة") = "رصيد سابق"
            row("مدين") = DT1.Rows(0)("مدين")
            row("دائن") = DT1.Rows(0)("دائن")
            row("الصافي") = DT1.Rows(0)("الصافي")
            DT.Rows.InsertAt(row, 0)
            GCRole.DataSource = rDt
            _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
            PreBalance.EditValue = PRM1(3).Value
            If PRM1(3).Value < 0.000 Then
                PBalance = -1
                PreBalance.BackColor = Color.FromArgb(211, 34, 52)
            ElseIf PRM1(3).Value >= 0.000 Then
                PBalance = 1
                PreBalance.BackColor = Color.FromArgb(28, 175, 87)
            End If
            Dim PR1(1) As SqlParameter
            PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR1(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim DT2 As New DataTable
            DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETAGENTTOTAL", PR1)
            If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                NETtotal = -1
                OverAllNetTotal.EditValue = DT2.Rows(0)("أول")
                OverAllNetTotal.BackColor = Color.FromArgb(211, 34, 52)
            Else
                NETtotal = 1
                OverAllNetTotal.EditValue = DT2.Rows(0)("ثاني")
                OverAllNetTotal.BackColor = Color.FromArgb(28, 175, 87)
            End If
            If GVRole.RowCount > 0 Then
                GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
                GVRole.Columns("دائن").AppearanceCell.BackColor = Color.FromArgb(0, 192, 0)
                GVRole.Columns("#").Width = 50
                GVRole.Columns("مدين").Width = 110
                GVRole.Columns("دائن").Width = 110
                GVRole.Columns("الصافي").Width = 110
                GVRole.Columns("التاريخ").Width = 110
                GVRole.Columns("طبيعة الحركة").Width = 400
                If PRM(3).Value > PRM(4).Value Then
                    Peroid = -1
                    OverAllPeroid.BackColor = Color.Red
                Else
                    Peroid = 1
                    OverAllPeroid.BackColor = Color.Green
                End If
                BranchID.Select()
            Else
                GCRole.DataSource = Nothing
                GVRole.Columns.Clear()
                OverAllPeroid.EditValue = 0.000
                OverAllNetTotal.EditValue = 0.000
                PreBalance.EditValue = 0.000
            End If
        End If

        NEWDVGFROMAT(GVRole)
    End Sub
    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}
        DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOLKP_buk", prm)
        DT.Rows.Add(1, "دينار ليبي")
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LoadData2()
        If D1.EditValue > D2.EditValue Then
            MessageBox.Show("تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If
        If CurrencyTo.EditValue = -1 Then
            CurrencyTo.ErrorText = "يجب اختيار العملة"
            Return
        End If

        Dim DT As New DataTable
        DT.Clear()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = BranchID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        PRM(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = 0}
        DT = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENT", PRM)
        If DT.Rows.Count >= 0 Then
            GVRole.Columns.Clear()
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            GCRole.DataSource = DT
            Dim PR111(2) As SqlParameter
            PR111(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR111(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            PR111(2) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = 0}
            Dim DT21 As New DataTable
            DT21 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTNETTOTAL", PR111)
            If DT21.Rows(0)("أول") > DT21.Rows(0)("ثاني") Then
                OverAllNetTotal.EditValue = DT21.Rows(0)("أول")
                OverAllNetTotal.BackColor = Color.Red
            Else
                OverAllNetTotal.EditValue = DT21.Rows(0)("ثاني")
                OverAllNetTotal.BackColor = Color.Green
            End If
            GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
            GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Green
            NEWDVGFROMAT(GVRole)
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Green
            If OverAllCredit.EditValue = 0.000 Then
                OverAllCredit.Properties.Appearance.BackColor = Color.Green
            Else
                OverAllCredit.Properties.Appearance.BackColor = Color.Green
            End If
            If OverAllCredit.EditValue = 0.000 Then
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            Else
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            End If
        End If
    End Sub
    Sub SumTotal()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        If GVRole.RowCount > 1 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "دائن"
            GVRole.Columns("دائن").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "مدين"
            GVRole.Columns("مدين").Summary.Add(DebitSum)

            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("مدين").SummaryItem.SummaryValue)
            OverAllDebit.Properties.Appearance.BackColor = Color.Red

            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن").SummaryItem.SummaryValue)
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Green
            OverAllPeroid.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
            If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                Peroid = -1
                OverAllPeroid.BackColor = Color.Red
            ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                Peroid = 1
                OverAllPeroid.BackColor = Color.Green
            End If
        End If
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub

    Sub ADDCOLUMN()
        Dim colCounter As GridColumn
        colCounter = GVRole.Columns.AddVisible("RowHandle")
        colCounter.Caption = "#"
        colCounter.VisibleIndex = 0
        colCounter.Width = 50
        colCounter.UnboundType = UnboundColumnType.Integer
        colCounter.OptionsColumn.AllowSort = DefaultBoolean.False
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
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
    Private Sub FrmViewBranchSafe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        If Me.WindowState = FormWindowState.Maximized Then
            Dim pd As New Padding
            pd.Left = Me.Padding.Left
            pd.Right = Me.Padding.Right
            pd.Top = Me.Padding.Top
            pd.Bottom = Me.Height - Screen.PrimaryScreen.WorkingArea.Height
            Me.Padding = pd
        End If
        NewRecord()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        LoadData()
        SumTotal()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If GVRole.RowCount = 0 Then
            ErrorMessage(Me, "رسالة خطأ", "لا يوجد بيانات لطباعتها")
            Exit Sub
        End If
        Try
            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(1) = New SqlParameter("D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(2) = New SqlParameter("BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(3) = New SqlParameter("SumDebitFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("SumCreditFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("OverAllNetTotalFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@OverAllNetTotalFinal_total", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_SelectByAgent", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTViewAgentMovement
                report.DataSource = dt
                report.DataMember = "AccSafeActivityTb"
                report.OverAllTotal.Text = OverAllPeroid.Text
                report.OverAllNet.Text = prm(6).Value
                report.agentname.Text = BranchID.Text
                report.D1.Text = D1.Text
                report.D2.Text = D2.Text
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
            SQLCON.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Dim report As New RPTViewAgentMovement
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}


    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs)
        Try
            SplashScreenManager1.ShowWaitForm()


            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(1) = New SqlParameter("D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(2) = New SqlParameter("BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(3) = New SqlParameter("SumDebitFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("SumCreditFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("OverAllNetTotalFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@OverAllNetTotalFinal_total", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_SelectByAgent", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTViewAgentMovement
                report.DataSource = dt

                report.DataMember = "AccSafeActivityTb"

                report.OverAllTotal.Text = OverAllPeroid.Text
                report.OverAllNet.Text = prm(6).Value
                report.agentname.Text = BranchID.Text
                report.D1.Text = D1.Text
                report.D2.Text = D2.Text
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()


                report.ExportToPdf(pdfExportFile, PdfExportOptions)


                SINTWATSAPP_document(get_gruop_id(BranchID.EditValue), pdfExportFile, $"كشف حساب  {BranchID.Text} ", " كشف الحساب" & ".pdf")
            End If

            SplashScreenManager1.CloseWaitForm()

            SQLCON.Close()

        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub GVRole_ShowingEditor(sender As Object, e As CancelEventArgs) Handles GVRole.ShowingEditor
        e.Cancel = True
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
End Class