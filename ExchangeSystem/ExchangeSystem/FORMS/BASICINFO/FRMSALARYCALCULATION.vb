Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient

Public Class FRMSALARYCALCULATION
    Dim clscs As New CLSSALARYCALC
    Public RepaymentP, PaymentTMS, SalaryCalc, EMPSalaary As Integer
    Dim AccID, EMAccID, ADVPMTACCID As ULong
    Public IsEMPsalary As Boolean

    Sub NEWRECORD()
        IsEMPsalary = False
        SALARYMONTH.EditValue = Date.Now
        YDATE.EditValue = Date.Now
        DVGFROMAT()
        GVRole.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        OverallSalaryTotal.EditValue = 0.000
        OverallConstanceTotal.EditValue = 0.000
        OverallBounusTotal.EditValue = 0.000
        OverallDiscount.EditValue = 0.000
        OverallAdvancePaymentTotal.EditValue = 0.000
        OverallNetTotal.EditValue = 0.000
        SalaryCalcType.SelectedIndex = 0

    End Sub
    Sub LoadBanks()
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_PRO_ONLY("BBranchTb_LOADTOGITHER")
        'BankID.Properties.DataSource = DT
        'BankID.Properties.DisplayMember = "BranchName"
        'BankID.Properties.ValueMember = "AccID"
        'BankID.Properties.PopulateColumns()
        'BankID.Properties.Columns("AccID").Visible = False
    End Sub
    Sub LoadData()
        GVRole.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        OverallSalaryTotal.EditValue = 0.000
        OverallConstanceTotal.EditValue = 0.000
        OverallBounusTotal.EditValue = 0.000
        OverallDiscount.EditValue = 0.000
        OverallAdvancePaymentTotal.EditValue = 0.000
        OverallNetTotal.EditValue = 0.000
        Dim MTIME As DateTime = SALARYMONTH.EditValue
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = YDATE.EditValue
        Dim ye As Integer = YTIME.Year
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = month}
        PR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = ye}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SalaryCalc_LoadToCalculate", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("ID").Visible = False
            GVRole.Columns("BranchID").Visible = False

            DVGFROMAT()
            GVRole.Columns("اسم الموظف").Width = 300
            GVRole.Columns("رقم هاتف الموظف").Visible = False
            GVRole.Columns("AccID").Visible = False
            GVRole.Columns("الرقمي الوظيفي").VisibleIndex = 1
        End If
    End Sub
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        If GVRole.RowCount > 0 Then

            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "الراتب الأساسي"
            GVRole.Columns("الراتب الأساسي").Summary.Add(OverallSalary)
            GVRole.Columns("الراتب الأساسي").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallSalaryTotal.EditValue = 0.000
            OverallSalaryTotal.EditValue = Convert.ToDouble(GVRole.Columns("الراتب الأساسي").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "علاوات ثابتة"
            GVRole.Columns("علاوات ثابتة").Summary.Add(OverallConstance)
            GVRole.Columns("علاوات ثابتة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallConstanceTotal.EditValue = 0.000
            OverallConstanceTotal.EditValue = Convert.ToDouble(GVRole.Columns("علاوات ثابتة").SummaryItem.SummaryValue)
            '---------------------------------------------------
            Dim OverallBounus As New GridColumnSummaryItem()
            OverallBounus.SummaryType = SummaryItemType.Sum
            OverallBounus.FieldName = "علاوات أخرى"
            GVRole.Columns("علاوات أخرى").Summary.Add(OverallBounus)
            GVRole.Columns("علاوات أخرى").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallBounusTotal.EditValue = 0.000
            OverallBounusTotal.EditValue = Convert.ToDouble(GVRole.Columns("علاوات أخرى").SummaryItem.SummaryValue)
            '-----------------------------------------------
            Dim OverallDiscountT As New GridColumnSummaryItem()
            OverallDiscountT.SummaryType = SummaryItemType.Sum
            OverallDiscountT.FieldName = "خصميات متنوعة"
            GVRole.Columns("خصميات متنوعة").Summary.Add(OverallDiscountT)
            GVRole.Columns("خصميات متنوعة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallDiscount.EditValue = 0.000
            OverallDiscount.EditValue = Convert.ToDouble(GVRole.Columns("خصميات متنوعة").SummaryItem.SummaryValue)
            '-----------------------------------------------
            Dim OverallAdvancePayment As New GridColumnSummaryItem()
            OverallAdvancePayment.SummaryType = SummaryItemType.Sum
            OverallAdvancePayment.FieldName = "خصم السلفة"
            GVRole.Columns("خصم السلفة").Summary.Add(OverallAdvancePayment)
            GVRole.Columns("خصم السلفة").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallAdvancePaymentTotal.EditValue = 0.000
            OverallAdvancePaymentTotal.EditValue = Convert.ToDouble(GVRole.Columns("خصم السلفة").SummaryItem.SummaryValue)
            '-----------------------------------------------
            Dim OverallNet As New GridColumnSummaryItem()
            OverallNet.SummaryType = SummaryItemType.Sum
            OverallNet.FieldName = "الصافي"
            GVRole.Columns("الصافي").Summary.Add(OverallNet)
            GVRole.Columns("الصافي").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallNetTotal.EditValue = 0.000
            OverallNetTotal.EditValue = Convert.ToDouble(GVRole.Columns("الصافي").SummaryItem.SummaryValue)
        End If
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub
    Sub CheckSelectedMonth()
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = SALARYMONTH.EditValue}
        PR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = YDATE.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AdvancePaymentTb_GetADPMNTTOUPDATE", PR)
        If DT.Rows.Count > 0 Then
            ErrorMessage(Me, "تم احتساب مرتبات هذا الشهر مسبقاً", "رسالة خطأ")
            Exit Sub
        End If
    End Sub
    Sub GetAccID(BRANCHID As Integer, AccParent As Decimal)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BRANCHID}
        PRM(1) = New SqlParameter("@AccParent", SqlDbType.Decimal) With {.Value = AccParent}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccountsTb_GetAccIDBaseOnBranchID", PRM)
        If DT.Rows.Count > 0 Then
            AccID = DT.Rows(0)("AccID")
        End If
    End Sub
    Sub GetEMAccID(EMID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = EMID}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EmployeeTb_SelectEmAccByID", PRM)
        If DT.Rows.Count > 0 Then
            EMAccID = DT.Rows(0)("AccID")
        End If
    End Sub
    Private Sub FRMSALARYCALCULATION_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
        AddSerialColumn(GVRole)
        DVGFROMAT()
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim CHMONTH As Integer = 0
        Dim CHYEAR As Integer = 0
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim MTIME As DateTime = SALARYMONTH.EditValue
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = YDATE.EditValue
        Dim ye As Integer = YTIME.Year
        Dim PRR(2) As SqlParameter
        PRR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = month}
        PRR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = ye}
        PRR(2) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = 0}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("SalaryCalculationTb_CheckMonthSelected", PRR)
        If DTT.Rows.Count > 0 Then
            CHMONTH = DTT.Rows(0)("SALARYMONTH")
            CHYEAR = DTT.Rows(0)("SALARYEAR")
        End If

        If GVRole.RowCount > 0 Then
            Dim CurrentMontDate As DateTime = Date.Now
            Dim CuMonth As Integer = CurrentMontDate.Month
            If GVRole.ActiveFilterString <> Nothing Then
                ErrorMessage(Me, "الرجاء إلغاء الفلترة لكي يتم الاحتساب", "رسالة خطأ")
                Exit Sub
            End If
            Dim resu = XtraMessageBox.Show(lookAndFeelError, "لم يتم احتساب شهر" & Space(1) & SALARYMONTH.Text & Space(1) & "هل تريد الاستمرار في الاحتساب", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
            If resu = DialogResult.Yes Then
                Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
                Dim lookAndFeelError2 As New UserLookAndFeel(Me)
                lookAndFeelError2.Style = LookAndFeelStyle.Skin
                lookAndFeelError2.UseDefaultLookAndFeel = False
                lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
                XtraMessageBox.AllowCustomLookAndFeel = True
                Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If result = DialogResult.Yes Then
                    Print()
                End If

                'SenForWhatsApp_SalaryCalc_LoadToCalculate()
                clscs.SalaryCalculationTb_insert2(UserID, SalaryCalc, month, ye, SalaryCalcType.SelectedIndex)
                FrmSavedSuccessfully.ShowDialog()
            Else
                Exit Sub
            End If
            'End If
        End If
        NEWRECORD()
    End Sub
    Private Function BuildTransENmasgeferMessage(EmNAme As String, selPrase As Double, SalaryVal As Double, Incres1 As Double, Incres2 As Double, Discount1 As Double, Discount2 As Double, Discount3 As Double) As String
        Dim message As String = "*شركة الرحالة القابضة*" & vbNewLine &
        "الموظف : " & EmNAme & vbNewLine &
        "راتب شهر " & Date.Now.ToString("MM") & " = " & Cur_Code("ليبي", SalaryVal, True, "n2") & vbNewLine &
        "العلاوة" & " : " & Cur_Code("ليبي", Incres1 + Incres2, True, "n2") & vbNewLine &
         "الخصم" & " : " & Cur_Code("ليبي", Discount1 + Discount2, True, "n2") & vbNewLine &
         "قسط السلفة" & " : " & Cur_Code("ليبي", Discount3, True, "n2") & vbNewLine &
          "*صافي المرتب*" & " : " & Cur_Code("ليبي", selPrase, True, "n2") & vbNewLine
        message &= "مع تمنياتنا لكم بالتوفيق."
        Return message
    End Function
    Public Sub SenForWhatsApp_SalaryCalc_LoadToCalculate()
        Try
            If GVRole.RowCount > 0 Then
                SplashScreenManager1.ShowWaitForm()
                For i = 0 To GVRole.RowCount - 1
                    WATSAPPMsAG(GVRole.GetRowCellValue(i, ("رقم هاتف الموظف")), BuildTransENmasgeferMessage(GVRole.GetRowCellValue(i, ("اسم الموظف")),
                                                                                                            GVRole.GetRowCellValue(i, ("الصافي")),
                                                                                                            GVRole.GetRowCellValue(i, ("الراتب الأساسي")),
                                                                                                            GVRole.GetRowCellValue(i, ("علاوات ثابتة")),
                                                                                                            GVRole.GetRowCellValue(i, ("علاوات أخرى")),
                                                                                                            GVRole.GetRowCellValue(i, ("خصميات متنوعة")),
                                                                                                            GVRole.GetRowCellValue(i, ("خصم إجازة")),
                                                                                                            GVRole.GetRowCellValue(i, ("خصم السلفة"))), True)
                Next
                SplashScreenManager1.CloseWaitForm()
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show($"رسالة تنبية : {ex.Message}")
        End Try
    End Sub
    Private Sub MDATE_TextChanged(sender As Object, e As EventArgs) Handles SALARYMONTH.TextChanged, YDATE.TextChanged
        GVRole.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        OverallSalaryTotal.EditValue = 0.000
        OverallConstanceTotal.EditValue = 0.000
        OverallBounusTotal.EditValue = 0.000
        OverallDiscount.EditValue = 0.000
        OverallAdvancePaymentTotal.EditValue = 0.000
        OverallNetTotal.EditValue = 0.000
    End Sub
    Public Sub AdvancePaymentTb_ADPMNTTOUPDATE(PaidVal As Decimal, PaymentTimes As Integer, EMPID As Integer)
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@PaidVal", SqlDbType.Decimal) With {.Value = PaidVal}
        PR(1) = New SqlParameter("@PaymentTimes", SqlDbType.Int) With {.Value = PaymentTimes}
        PR(2) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        RUN_EXUTE_PRO("AdvancePaymentTb_ADPMNTTOUPDATE", PR)
    End Sub
    Sub Print()

        Dim MTIME As DateTime = SALARYMONTH.EditValue
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = YDATE.EditValue
        Dim ye As Integer = YTIME.Year

        If GVRole.RowCount = 0 Then
            ErrorMessage(Me, "لا يوجد بيانات لطباعتها", "رسالة خطأ")
            Exit Sub
        End If
        Try
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@SALARYMONTH", month)
            PRM(1) = New SqlParameter("@SALARYEAR", ye)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_SalaryCalc_LoadToCalculate", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "SalaryCalculationTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)

                Dim report As New RPTSALARYCALCULATION
                Dim report1 As New RPTIndividualSalaryEMP2
                report.FilterString = GVRole.ActiveFilterString
                report1.FilterString = GVRole.ActiveFilterString

                report1.DataSource = ds
                report1.DataMember = "SalaryCalculationTb"

                report.DataSource = ds
                report.DataMember = "SalaryCalculationTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                Dim tool1 As ReportPrintTool = New ReportPrintTool(report1)
                If IsEMPsalary = True Then
                    report1.CreateDocument()
                    report1.ShowPreview()
                Else
                    report.CreateDocument()
                    report.ShowPreview()
                End If

            Else
                ErrorMessage(Me, "لا يوجد بيانات لطباعتها في هذا التاريخ", "رسالة خطأ")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Print()
    End Sub
    Private Sub GVRole_CustomSummaryCalculate(sender As Object, e As CustomSummaryEventArgs) Handles GVRole.CustomSummaryCalculate
        GVRole_FocusedRowChanged(Nothing, Nothing)
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged

        If GVRole.RowCount > 0 Then

            Dim OverallSalary As New GridColumnSummaryItem()
            OverallSalary.SummaryType = SummaryItemType.Sum
            OverallSalary.FieldName = "الراتب الأساسي"
            GVRole.Columns("الراتب الأساسي").Summary.Add(OverallSalary)
            OverallSalaryTotal.EditValue = 0.000
            OverallSalaryTotal.EditValue = Convert.ToDouble(GVRole.Columns("الراتب الأساسي").SummaryItem.SummaryValue)
            '-----------------------------------------------------------------------------
            Dim OverallConstance As New GridColumnSummaryItem()
            OverallConstance.SummaryType = SummaryItemType.Sum
            OverallConstance.FieldName = "علاوات ثابتة"
            GVRole.Columns("علاوات ثابتة").Summary.Add(OverallConstance)
            OverallConstanceTotal.EditValue = 0.000
            OverallConstanceTotal.EditValue = Convert.ToDouble(GVRole.Columns("علاوات ثابتة").SummaryItem.SummaryValue)
            '---------------------------------------------------
            Dim OverallBounus As New GridColumnSummaryItem()
            OverallBounus.SummaryType = SummaryItemType.Sum
            OverallBounus.FieldName = "علاوات أخرى"
            GVRole.Columns("علاوات أخرى").Summary.Add(OverallBounus)
            OverallBounusTotal.EditValue = 0.000
            OverallBounusTotal.EditValue = Convert.ToDouble(GVRole.Columns("علاوات أخرى").SummaryItem.SummaryValue)
            '-----------------------------------------------
            Dim OverallDiscountT As New GridColumnSummaryItem()
            OverallDiscountT.SummaryType = SummaryItemType.Sum
            OverallDiscountT.FieldName = "خصميات متنوعة"
            GVRole.Columns("خصميات متنوعة").Summary.Add(OverallDiscountT)
            OverallDiscount.EditValue = 0.000
            OverallDiscount.EditValue = Convert.ToDouble(GVRole.Columns("خصميات متنوعة").SummaryItem.SummaryValue)
            '-----------------------------------------------
            Dim OverallAdvancePayment As New GridColumnSummaryItem()
            OverallAdvancePayment.SummaryType = SummaryItemType.Sum
            OverallAdvancePayment.FieldName = "خصم السلفة"
            GVRole.Columns("خصم السلفة").Summary.Add(OverallAdvancePayment)
            OverallAdvancePaymentTotal.EditValue = 0.000
            OverallAdvancePaymentTotal.EditValue = Convert.ToDouble(GVRole.Columns("خصم السلفة").SummaryItem.SummaryValue)
            '-----------------------------------------------
            Dim OverallNet As New GridColumnSummaryItem()
            OverallNet.SummaryType = SummaryItemType.Sum
            OverallNet.FieldName = "الصافي"
            GVRole.Columns("الصافي").Summary.Add(OverallNet)
            OverallNetTotal.EditValue = 0.000
            OverallNetTotal.EditValue = Convert.ToDouble(GVRole.Columns("الصافي").SummaryItem.SummaryValue)
        End If
        If GVRole.RowCount = 1 Then
            If GVRole.ActiveFilterString.Contains("[اسم الموظف]") Then
                IsEMPsalary = True
            Else
                IsEMPsalary = False
            End If
            EMPSalaary = GVRole.GetRowCellValue(0, "الصافي")
        Else
            IsEMPsalary = False
        End If
        ' لو العمود مش موجود نضيفه
        If GVRole.Columns.ColumnByFieldName("SN") Is Nothing Then
            Dim col As New DevExpress.XtraGrid.Columns.GridColumn()
            col.Caption = "#"
            col.FieldName = "SN"
            col.Visible = True
            col.VisibleIndex = 0
            col.OptionsColumn.AllowEdit = False
            col.OptionsColumn.ReadOnly = True
            GVRole.Columns.Insert(0, col)
        End If

        AddRowNumberColumnWithFilter(GVRole)


    End Sub


End Class