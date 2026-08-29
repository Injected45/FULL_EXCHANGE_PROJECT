Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SelectPdf
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmSelectAccountsBetweenBranches
    Sub NewRecord()
        DVGFormat()
        BranchToLKP()
        BranchID.EditValue = BID
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverAllNetTotal.EditValue = 0.000
        OverAllBenefits.EditValue = 0.000
        OverAllCredit.EditValue = 0.0000
        OverAllDebit.EditValue = 0.000
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub

    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            BranchID.EditValue = BID
        Else
            If UserType = 1 Then
                BranchID.Enabled = True
            Else
                BranchID.Enabled = False
            End If

            BranchID.EditValue = BID
        End If
    End Sub
    Public Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = False
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
        If GVRole.Columns.Count > 0 Then
            GVRole.Columns(0).Visible = False
        End If


    End Sub
    Sub BranchToLKP()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
    End Sub
    Sub LOADDATA()
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        DVGFormat()
        If D1.EditValue Is Nothing Then
            D1.ErrorText = "يجب اختيار التاريخ أولاً"
            Exit Sub
        End If
        If D2.EditValue Is Nothing Then
            D2.ErrorText = "يجب اختيار التاريخ أولاً"
            Exit Sub
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع أولاً"
            Exit Sub
        End If
        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية"
            Exit Sub
        End If
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccSafeActivityTb_SelectAccountsBetweenBranches", PRM)
        If DT.Rows.Count = 0 Then
            GCRole.DataSource = Nothing
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim reuslt = XtraMessageBox.Show(lookAndFeelError, "لا يوجد بيانات لعرضها خلال هذه الفترة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            GCRole.DataSource = DT
            GVRole.Columns("مدين (له)").AppearanceCell.BackColor = Color.Red
            GVRole.Columns("دائن (عليه)").AppearanceCell.BackColor = Color.FromArgb(0, 192, 0)
            GVRole.Columns("ID").Visible = True
            DVGFormat()
            'OverAllNetTotal.EditValue = GETBRANCHCURRENTVAL(BranchID.EditValue, D1.EditValue, D2.EditValue)
            SumTotal()
            OverAllBenefits.EditValue = GETBRANCHBENEFITSVAL(BranchID.EditValue, D1.EditValue, D2.EditValue)

        End If
    End Sub

    Private Sub FrmSelectAccountsBetweenBranches_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        NewRecord()
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
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        'Dim View As GridView = TryCast(sender, GridView)
        Dim view As GridView = TryCast(sender, GridView)

        If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
            If e.Column.FieldName = "مدين (له)" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Red
            End If
            If e.Column.FieldName = "دائن (عليه)" Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Green
            End If
        End If
    End Sub
    Sub SumTotal()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllNetTotal.EditValue = 0.000
        If GVRole.RowCount > 0 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "دائن (عليه)"
            GVRole.Columns("دائن (عليه)").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "مدين (له)"
            GVRole.Columns("مدين (له)").Summary.Add(DebitSum)
            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("مدين (له)").SummaryItem.SummaryValue)
            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن (عليه)").SummaryItem.SummaryValue)
            OverAllNetTotal.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
        End If
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub BtnPrint_Click_1(sender As Object, e As EventArgs) Handles BtnPrint.Click
        LOADDATA()
    End Sub

    Private Sub GCRole_Click(sender As Object, e As EventArgs) Handles GCRole.Click
        If GVRole.RowCount > 0 Then
            FRMGETBRANCHSELECTEDDETAILS.ShowDialog()
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverAllNetTotal.EditValue = 0.000
        OverAllBenefits.EditValue = 0.000
        OverAllCredit.EditValue = 0.0000
        OverAllDebit.EditValue = 0.000
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
    End Sub

    Private Sub BtnPrint1_Click(sender As Object, e As EventArgs) Handles BtnPrint1.Click
        If GVRole.RowCount > 0 Then
            FrmGetAccountDetails.ShowDialog()
        End If
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
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", BranchID.EditValue)
            prm(1) = New SqlParameter("@D1", D1.EditValue)
            prm(2) = New SqlParameter("@D2", D2.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_SelectAccountsBetweenBranches", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTSelectAccountsBetweenBranches
                report.DataSource = dt
                report.DataAdapter = ""
                report.DataMember = "AccSafeActivityTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub


    Private Rpt As New XtraReport() With {.Name = "Rpt_ACCFINAL_Account_SEND"}
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & Rpt.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}


    Public Sub LODE_SEND_FRom()
        Try
            ' التحقق من وجود صفوف
            If GVRole.RowCount > 0 Then

                ' تكرار عبر الصفوف
                For i = 0 To GVRole.RowCount - 1
                    Dim branchID As Object = GVRole.GetRowCellValue(i, "ID")

                    ' تأكد من قيمة BranchID صالحة
                    If branchID IsNot Nothing Then
                        ' إعداد الـ Command مرة واحدة

                        Dim prM(6) As SqlParameter
                        prM(0) = New SqlParameter
                        prM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchID}
                        prM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                        prM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
                        prM(3) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
                        prM(4) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal, 18, 3) With {.Direction = ParameterDirection.Output}
                        prM(5) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
                        prM(6) = New SqlParameter("@NET_FORTotalFinal", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

                        Dim dtW As New DataTable
                        dtW.Clear()
                        dtW = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_SelectByAgent_ALL", prM)



                        ' تنفيذ الاستعلام باستخدام SqlDataAdapter


                        'التحقق من وجود بيانات
                        If dtW.Rows.Count > 0 Then
                            ' إنشاء التقرير وتصديره
                            Dim report As New RPTViewAgentMovement With {
                                    .DataSource = dtW,
                            .DataMember = "AccSafeActivityTb"}
                            report.D1.Text = D1.Text
                            report.D2.Text = D2.Text

                            report.OverAllTotal.Text = prM(5).Value
                            report.OverAllNet.Text = prM(6).Value



                            Dim tool As ReportPrintTool = New ReportPrintTool(report)

                            'report.ShowPreview()

                            report.CreateDocument()

                            ' تصدير التقرير إلى PDF
                            report.ExportToPdf(pdfExportFile, PdfExportOptions)

                            ' إرسال PDF عبر WhatsApp
                            SINTWATSAPP_document(get_gruop_id(branchID), pdfExportFile, " كشف التحويلات", $"   كشف التحويلات{GVRole.GetRowCellValue(i, 1)}" & ".pdf")

                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ' تحسين عرض الاستثناء
            MessageBox.Show(ex.Message, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        SplashScreenManager1.ShowWaitForm()

        If BranchID.EditValue > -1 AndAlso BranchID.Text <> String.Empty Then

            LODE_SEND_FRom()
        End If
        SplashScreenManager1.CloseWaitForm()
    End Sub
End Class