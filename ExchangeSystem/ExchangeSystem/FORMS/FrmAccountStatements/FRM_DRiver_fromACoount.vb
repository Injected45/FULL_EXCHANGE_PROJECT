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
Public Class FRM_DRiver_fromACoount
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

        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = BID}
        LoadToControlar(BranchID, "DriversTb_LoadAgent", "DriverName", "accontID", prm)

    End Sub
    Sub LoadData()
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        'GVRole.BeginUpdate()
        NEWDVGFROMAT(GVRole)
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار اسم الحساب"
            Return
        End If
        If D1.EditValue Is Nothing Then
            D1.ErrorText = "يجب اختيار التاريخ أولاً"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية"
            Return
        End If
        Dim DT As New DataTable
        DT.Clear()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = BranchID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = 1}
        PRM(4) = New SqlParameter("@TypID ", SqlDbType.Int) With {.Value = 0}
        DT = RUN_QUARY_PRO("CONDDB_AccSafeActivityTb_GETPartnerMOVEMENT", PRM)
        If DT.Rows.Count >= 0 Then
            GCRole.DataSource = DT
            Dim PR111(2) As SqlParameter
            PR111(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR111(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}
            PR111(2) = New SqlParameter("@TypID ", SqlDbType.Int) With {.Value = 0}
            Dim DT21 As New DataTable
            DT21 = RUN_QUARY_PRO("CONDB_AccSafeActivityTb_GETPartnerMOVEMENTNETTOTAL", PR111)
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
            'OverAllCredit.Properties.Appearance.BackColor = Color.Green
            'OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Green
            'OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Green
            'OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Green
            'If OverAllCredit.EditValue = 0.000 Then
            '    OverAllCredit.Properties.Appearance.BackColor = Color.Green
            'Else
            '    OverAllCredit.Properties.Appearance.BackColor = Color.Green
            'End If
            'If OverAllCredit.EditValue = 0.000 Then
            '    OverAllDebit.Properties.Appearance.BackColor = Color.Red
            'Else
            '    OverAllDebit.Properties.Appearance.BackColor = Color.Red
            'End If
        End If
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

    Private Sub FRM_DRiver_fromACoount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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


    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
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
            Dim dt As New DataTable

            dt.Clear()

            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = BranchID.EditValue}
            PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = 1}
            PRM(4) = New SqlParameter("@TypID ", SqlDbType.Int) With {.Value = 0}
            dt = RUN_QUARY_PRO("CONDDB_AccSafeActivityTb_GETPartnerMOVEMENT", PRM)
            If dt.Rows.Count > 0 Then
                If BranchID.EditValue = 0 Then
                    'Dim report As New RPTProPartnerMovment
                    'report.DataSource = dt
                    'report.DataMember = "AccSafeActivityTb"
                    'Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    'report.D1.Text = Format(D1.EditValue, "yyyy/MM/dd").ToString
                    'report.D2.Text = Format(D2.EditValue, "yyyy/MM/dd").ToString
                    'report.OverAllTotal1.Text = Format(OverAllNetTotal.EditValue, "N3")
                    ''report.XrLabel4.Text = FRMPartnerMovment.CUST.Text
                    'report.XrLabel4.Text = BranchID.Text
                    'If TypeID.SelectedIndex = 0 Then
                    '    report.XrLabel21.Text = "كشف حساب شريك"
                    'ElseIf TypeID.SelectedIndex = 1 Then
                    '    report.XrLabel21.Text = "كشف حساب مقاول"
                    'ElseIf TypeID.SelectedIndex = 2 Then
                    '    report.XrLabel21.Text = "كشف حساب"
                    'End If
                    'If OverAllNetTotal.BackColor = Color.Green Then
                    '    report.XrPictureBox5.Image = My.Resources.G_dollar
                    'Else
                    '    report.XrPictureBox5.Image = My.Resources.R_dollar
                    'End If
                    'report.XrPictureBox2.Visible = False
                    'report.XrLabel6.Visible = False
                    'report.CreateDocument()
                    'report.ShowPreview()
                Else
                    Dim report As New RPTPartnerMovment
                    report.DataSource = dt
                    report.DataMember = "AccSafeActivityTb"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.D1.Text = Format(D1.EditValue, "yyyy/MM/dd").ToString
                    report.D2.Text = Format(D2.EditValue, "yyyy/MM/dd").ToString
                    report.OverAllTotal1.Text = Format(OverAllNetTotal.EditValue, "N3")
                    'report.XrLabel4.Text = FRMPartnerMovment.CUST.Text
                    report.XrLabel4.Text = BranchID.Text
                    report.XrLabel21.Text = "كشف حساب مندوب"
                    If OverAllNetTotal.BackColor = Color.Green Then
                        report.XrPictureBox5.Image = My.Resources.G_dollar
                    Else
                        report.XrPictureBox5.Image = My.Resources.R_dollar
                    End If
                    report.XrPictureBox2.Visible = False
                    report.XrLabel6.Visible = False
                    report.CreateDocument()
                    report.ShowPreview()
                End If


            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

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