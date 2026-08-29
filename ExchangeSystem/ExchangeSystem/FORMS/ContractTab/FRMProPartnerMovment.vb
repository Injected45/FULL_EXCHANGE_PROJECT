Imports DevExpress.Utils
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Public Class FRMProPartnerMovment
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
        TypeID.SelectedIndex = -1
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        GCRole.DataSource = Nothing
        NEWDVGFROMAT(GVRole)
        BranchID.Properties.DataSource = Nothing
        BranchID.EditValue = -1
        BranchID.Enabled = True
    End Sub
    Sub BranchToLKP()
        Dim DT As New DataTable
        DT.Clear()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.BigInt) With {.Value = TypeID.SelectedIndex}
        DT = RUN_QUARY_PRO("CONDB_PartnerORContractor_LOADTOLKP", PRM)
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "AccID"
            BranchID.Properties.DisplayMember = "AccName"
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.ShowHeader = False
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.Columns(0).Visible = False
            DT.Rows.Add(0, "الكل")
        End If
    End Sub
    Private Sub FRMProPartnerMovment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub
    Sub LoadData()
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        'GVRole.BeginUpdate()
        NEWDVGFROMAT(GVRole)
        If BranchID.EditValue = -1 And TypeID.SelectedIndex <> 5 Then
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
        PRM(4) = New SqlParameter("@TypID ", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}
        DT = RUN_QUARY_PRO("CONDDB_AccSafeActivityTb_GETPartnerMOVEMENT", PRM)
        If DT.Rows.Count >= 0 Then
            GCRole.DataSource = DT
            Dim PR111(2) As SqlParameter
            PR111(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR111(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}
            PR111(2) = New SqlParameter("@TypID ", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}
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
            If TypeID.SelectedIndex = 5 Then
                GVRole.Columns("قيمة متبقيه").AppearanceCell.BackColor = Color.Red
            End If
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
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub

    Sub SumTotal()
        OverAllPeroid.EditValue = 0.00
        OverAllCredit.EditValue = 0.00
        OverAllDebit.EditValue = 0
        If GVRole.RowCount > 0 Then
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
            If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                OverAllPeroid.EditValue = OverAllDebit.EditValue - OverAllCredit.EditValue
                OverAllPeroid.BackColor = Color.Red
            ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                OverAllPeroid.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
                OverAllPeroid.BackColor = Color.Green
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

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        Try
            LoadData()
            If GVRole.RowCount > 0 Then
                GVRole.Columns("#").Width = 50
                GVRole.Columns("مدين").Width = 130
                GVRole.Columns("دائن").Width = 130
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GCRole.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllNetTotal.EditValue = 0.000
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
            PRM(4) = New SqlParameter("@TypID ", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}
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
                    If TypeID.SelectedIndex = 1 Then
                        report.XrLabel21.Text = "كشف حساب شريك"
                    ElseIf TypeID.SelectedIndex = 2 Then
                        report.XrLabel21.Text = "كشف حساب مقاول"
                    ElseIf TypeID.SelectedIndex = 3 Then
                        report.XrLabel21.Text = "كشف أصل"
                    ElseIf TypeID.SelectedIndex = 4 Then
                        report.XrLabel21.Text = "كشف حساب مشروع"
                    Else
                        report.XrLabel21.Text = "كشف حساب"
                    End If
                    If OverAllNetTotal.BackColor = Color.Green Then
                        'report.XrPictureBox5.Image = My.Resources.G_dollar
                    Else
                        'report.XrPictureBox5.Image = My.Resources.R_dollar
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

    Private Sub TypeID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeID.SelectedIndexChanged
        BranchID.Properties.DataSource = Nothing
        BranchID.EditValue = -1
        If TypeID.SelectedIndex = 5 Then
            BranchID.Enabled = False
        Else
            BranchID.Enabled = True
        End If
        If TypeID.SelectedIndex > -1 Then
            BranchToLKP()
        End If
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
End Class