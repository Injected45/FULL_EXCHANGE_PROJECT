Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SelectPdf
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading
Public Class FRMPartnerMovment
    Sub NEWRECORD()
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRNCHDIERCT(BranchID)
        OverAllTotal.EditValue = 0.000
        GCRole.DataSource = Nothing
        BranchID.EditValue = BID
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
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
        NEWDVGFROMAT(GVRole)
        LOADCIDFROMT()
        LoadPartnerAccounts()
        CurrencyTo.EditValue = 1
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 144)
    End Sub


    Sub LoadData()
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        'GVRole.BeginUpdate()
        NEWDVGFROMAT(GVRole)
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If CUST.EditValue = -1 Then
            CUST.ErrorText = "يجب اختيار اسم الشريك"
            Return
        End If
        If CurrencyTo.EditValue = -1 Then
            CurrencyTo.ErrorText = "يجب اختيار العملة"
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
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        DT = RUN_QUARY_PRO("AccSafeActivityTb_GETPartnerMOVEMENT", PRM)
        If DT.Rows.Count >= 0 Then
            GVRole.Columns.Clear()
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            GCRole.DataSource = DT
            Dim PR111(1) As SqlParameter
            PR111(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = CUST.EditValue}
            PR111(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim DT21 As New DataTable
            DT21 = RUN_QUARY_PRO("AccSafeActivityTb_GETPartnerMOVEMENTNETTOTAL", PR111)
            If DT21.Rows(0)("أول") > DT21.Rows(0)("ثاني") Then
                OverAllTotal1.EditValue = DT21.Rows(0)("أول")
                OverAllTotal1.BackColor = Color.Red
            Else
                OverAllTotal1.EditValue = DT21.Rows(0)("ثاني")
                OverAllTotal1.BackColor = Color.Green
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
    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}

        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DT.Rows.Add(1, "دينار ليبي")
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
            DVGFormat(GridView2)
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub

    Sub LoadPartnerAccounts()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}

        Dim dt As New DataTable
        dt.Clear()
        CUST.Properties.DataSource = Nothing
        dt = RUN_QUARY_PRO("PartnerAccounts_LOADTOLKP", PR)
        If dt.Rows.Count > 0 Then
            CUST.Properties.DataSource = dt
            CUST.Properties.ValueMember = "AccID"
            CUST.Properties.DisplayMember = "AccName"
            CUST.Properties.ShowHeader = False

        End If
    End Sub

    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        If GVRole.RowCount > 0 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "دائن"
            GVRole.Columns("دائن").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "مدين"
            GVRole.Columns("مدين").Summary.Add(DebitSum)
            GVRole.Columns("مدين").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("مدين").SummaryItem.SummaryValue)
            OverAllDebit.Properties.Appearance.BackColor = Color.Red
            GVRole.Columns("دائن").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن").SummaryItem.SummaryValue)
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Green

            If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                OverAllTotal.BackColor = Color.Red
            ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                OverAllTotal.BackColor = Color.Green
            End If

        End If

    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 178, 148), e.Bounds)
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

    Private Sub FRMDebtorsMovment_Load(sender As Object, e As EventArgs) Handles Me.Load
        FormLocation(Me)
        NEWRECORD()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Try



            LoadData()
            If GVRole.RowCount > 0 Then
                GVRole.Columns("#").Width = 50
                GVRole.Columns("مدين").Width = 130
                GVRole.Columns("دائن").Width = 130
                GVRole.Columns("الرصيد").Width = 170

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CUST.QueryPopUp
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            CUST.Properties.PopulateColumns()
            CUST.Properties.Columns("AccID").Visible = False
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Then
            LoadPartnerAccounts()
        End If
    End Sub

    Private Sub OverAllCredit_TextChanged(sender As Object, e As EventArgs) Handles OverAllCredit.TextChanged
        'If GVRole.RowCount > 0 Then
        OverAllTotal.EditValue = Val(OverAllCredit.EditValue) - Val(OverAllDebit.EditValue)
        'End If
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
            BranchID.Enabled = False
            BranchID.EditValue = BID
        End If
    End Sub
    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles CUST.EditValueChanged
        'If EMPID.Text = String.Empty Then
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        'End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
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

            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
            PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            dt = RUN_QUARY_PRO("AccSafeActivityTb_GETPartnerMOVEMENT", PRM)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTPartnerMovment
                report.DataSource = dt
                report.DataMember = "AccSafeActivityTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.D1.Text = Format(D1.EditValue, "yyyy/MM/dd").ToString
                report.D2.Text = Format(D2.EditValue, "yyyy/MM/dd").ToString
                report.OverAllTotal1.Text = Format(OverAllTotal1.EditValue, "N2")
                report.XrLabel4.Text = CUST.Text
                report.XrLabel6.Text = BranchID.Text
                report.XrLabel21.Text = "كشف حساب شريك"
                If OverAllTotal1.BackColor = Color.Green Then
                    report.XrPictureBox5.Image = My.Resources.G_dollar
                Else
                    report.XrPictureBox5.Image = My.Resources.R_dollar
                End If
                report.XrPictureBox2.Visible = True
                report.XrLabel6.Visible = True
                report.CreateDocument()
                report.ShowPreview()
                'SQLCON.Close()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Dim report As New RPTCustomerMovement
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs)
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
        'Try
        Dim dt As New DataTable

        dt.Clear()

        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        dt = RUN_QUARY_PRO("AccSafeActivityTb_GETPartnerMOVEMENT", PRM)
        If dt.Rows.Count > 0 Then
            Dim report As New RPTPartnerMovment
            report.DataSource = dt
            report.DataMember = "AccSafeActivityTb"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ExportToPdf(pdfExportFile, PdfExportOptions)

            ' إرسال PDF عبر WhatsApp
            SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(CUST.EditValue), pdfExportFile, $"كشف حساب  {CUST.Text} ", " كشف الحساب" & ".pdf")
            'SQLCON.Close()
            'SQLCON.Close()
        Else
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub

End Class