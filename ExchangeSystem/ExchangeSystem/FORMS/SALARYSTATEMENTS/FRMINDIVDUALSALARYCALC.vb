Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraReports.UI
Public Class FRMINDIVDUALSALARYCALC
    Dim clscs As New CLSSALARYCALC
    Dim clse As New CLSEMPLOYEE
    Public RepaymentP, PaymentTMS, EMID, SalaryCalc, BranchID As Integer
    Public EMAccID, AccID, SafeAccID As ULong
    Sub NEWRECORD()
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        Dim MTIME As DateTime = Date.Now
        Dim month As Int32 = MTIME.Month
        MDATE.EditValue = month
        Dim YTIME As DateTime = Date.Now
        Dim ye As Integer = YTIME.Year
        YDATE.EditValue = ye
        Dim DDT As DateTime = Date.Now
        Dim DAY As Int32 = DDT.Day
        DDATE.EditValue = DAY
        DVGFROMAT()
        LOADEMP()
        EMPID.EditValue = -1
        GCRole.DataSource = Nothing
        GVRole.OptionsBehavior.Editable = False
        IsTotal.SelectedIndex = -1
        EmpAccVal.EditValue = 0.000
    End Sub
    Sub LOADEMP()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("EmployeeTb_LOADINTOLKPWITHACCIDANDNOBRANCHID")
        If dt.Rows.Count > 0 Then
            EMPID.Properties.DataSource = dt
            EMPID.Properties.ValueMember = "AccID"
            EMPID.Properties.DisplayMember = "EMPNAME"
            EMPID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LoadData()
        If EMPID.EditValue = -1 Or String.IsNullOrEmpty(EMPID.Text) Then
            EMPID.ErrorText = "يجب اختيار اسم الموظف"
            Exit Sub
        End If
        If IsTotal.SelectedIndex = -1 Then
            IsTotal.ErrorText = "يجب اختيار طبيعة الاحتساب"
            Exit Sub
        End If
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = EMAccID}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        If IsTotal.SelectedIndex = 1 Then
            PRM(2) = New SqlParameter("@DDay", SqlDbType.Int) With {.Value = DDATE.EditValue}
        Else
            PRM(2) = New SqlParameter("@DDay", SqlDbType.Int) With {.Value = 0}
        End If
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SalaryCalc_LoadToCalculateByEMPAccID", PRM)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            If IsTotal.SelectedIndex = 1 Then
                RestValue = DT.Rows(0)("القيمة المتبقية")
            Else
                RestValue = 0.000
            End If
            GVRole.Columns("AccID").Visible = False
            GVRole.Columns("ID").Visible = False
            GVRole.Columns("BranchID").Visible = False
            GETCASHEMPCUST(EMPID.EditValue)
            EmpAccVal.EditValue = EMPCUSTCASHVAL
            DVGFROMAT()
            NetEMPVal = EMPCUSTCASHVAL + GCRole.DataSource.Rows(0)("الصافي")
        End If
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
    End Sub
    Public RestValue, NetEMPVal As Decimal
    Sub GetEmpSafe()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EmployeeTb_GetUserAccID", PRM)
        If DT.Rows.Count > 0 Then
            SafeAccID = DT.Rows(0)("AccID")
        End If
    End Sub
    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        If EMPID.EditValue = -1 Or String.IsNullOrEmpty(EMPID.Text) Then
            EMPID.ErrorText = "يجب اختيار اسم الموظف"
            Exit Sub
        End If
        If IsTotal.SelectedIndex = -1 Then
            IsTotal.ErrorText = "يجب اختيار طبيعة الاحتساب"
            Exit Sub
        End If
        If GVRole.RowCount = 0 Then
            ErrorMessage(Me, "رسالة تنبيه", "الرجاء التأكد من عرض بيانات الموظف قبل الاحتساب")
            Exit Sub
        End If
        If SafeAccID > 0 Then
            GETSAFEVAL(SafeAccID, BranchID, DefaultCurrency)
            If SAFEVAL > 0 Then
                ErrorMessage(Me, "رسالة تنبيه", "لا يمكن إخلاء طرف الموظف قبل تصفية كامل الرصيد الموجود في عهدة الخزينة الخاصة به.")
                Me.Close()
                Exit Sub
            End If
        End If
        Dim DDT As DateTime = Date.Now
        Dim DAY As Int32 = DDT.Day
        If IsTotal.SelectedIndex = 0 Then
            DDATE.EditValue = DAY
        End If
        Dim CHEKPETYCASH As New DataTable
        CHEKPETYCASH.Clear()
        CHEKPETYCASH = CHECKEMPHASPETTYCASH(EMID)
        If CHEKPETYCASH.Rows.Count > 0 Then
            ErrorMessage(Me, "رسالة تنبيه", "الموظف عليه عهدة ويجب تسويتها قبل إخلاء طرفه")
            Exit Sub
        End If

        GETCASHEMPCUST(EMPID.EditValue)
        NetEMPVal = EMPCUSTCASHVAL + GCRole.DataSource.Rows(0)("الصافي")
        If EMPCUSTCASHVAL + GCRole.DataSource.Rows(0)("الصافي") < 0 Then
            ErrorMessage(Me, "رسالة تنبيه", "رصيد الموظف لايمكن أن يغطي التزاماته المالية، على الموظف إيداع مبلغ بقيمة " & (EMPCUSTCASHVAL + GCRole.DataSource.Rows(0)("الصافي")) * -1 & " د.ل " & " حتى يتم إخلاء طرفه")
            Exit Sub
        End If
        'If IsTotal.SelectedIndex = 0 Then
        '    If EMPCUSTCASHVAL + GCRole.DataSource.Rows(0)("الصافي") < 0 Then
        '        ErrorMessage(Me, "رسالة تنبيه", "رصيد الموظف لايمكن أن يغطي التزاماته المالية على الموظف إيداع مبلغ بقيمة " & (EMPCUSTCASHVAL + GCRole.DataSource.Rows(0)("الصافي")) * -1 & " د.ل " & " حتى يتم إخلاء طرفه")
        '        Exit Sub
        '    End If
        'Else
        '    'MsgBox((EMPCUSTCASHVAL + (GCRole.DataSource.Rows(0)("الصافي") - GCRole.DataSource.Rows(0)("الراتب الأساسي") + ((GCRole.DataSource.Rows(0)("الراتب الأساسي") / 30) * DDATE.EditValue))))
        '    'Exit Sub
        '    If (EMPCUSTCASHVAL + (GCRole.DataSource.Rows(0)("الصافي") - GCRole.DataSource.Rows(0)("الراتب الأساسي") + ((GCRole.DataSource.Rows(0)("الراتب الأساسي") / 30) * DDATE.EditValue))) < 0 Then
        '        ErrorMessage(Me, "رسالة تنبيه", "رصيد الموظف لايمكن أن يغطي التزاماته المالية على الموظف إيداع مبلغ بقيمة " & (EMPCUSTCASHVAL + (GCRole.DataSource.Rows(0)("الصافي") - GCRole.DataSource.Rows(0)("الراتب الأساسي") + ((GCRole.DataSource.Rows(0)("الراتب الأساسي") / 30) * DDATE.EditValue))) * -1 & " د.ل " & " حتى يتم إخلاء طرفه")
        '        Exit Sub
        '    End If
        'End If
        Dim CHMONTH As Integer = 0
        Dim CHYEAR As Integer = 0
        Dim PRR(3) As SqlParameter
        PRR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = MDATE.EditValue}
        PRR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = YDATE.EditValue}
        PRR(2) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = 0}
        PRR(3) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMID}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("SalaryCalculationTb_CheckMonthSelectedIndivdual", PRR)
        If DTT.Rows.Count > 0 Then
            CHMONTH = DTT.Rows(0)("SALARYMONTH")
            CHYEAR = DTT.Rows(0)("SALARYEAR")
            If CHMONTH = MDATE.EditValue And YDATE.EditValue = CHYEAR Then
                ErrorMessage(Me, "رسالة خطأ", "تم احتساب مرتبات هذا الشهر مسبقاً")
                Exit Sub
            End If
        End If
        Dim mms As String = " *شركة الرحالة القابضة*" & vbNewLine &
                    "الاسم " & ":" & Space(1) & Me.EMPID.Text & vbNewLine &
                    "الرقم الوظيفي " & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(EMAccID) & vbNewLine &
                    "نفيدكم بأنه تم إخلاء طرفكم من الشركة اعتبارًا من اليوم" & vbNewLine &
                    "نقدر لكم جهودكم خلال فترة عملكم معنا، ونتمنى لكم التوفيق في مسيرتكم القادمة"

        WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(EMAccID), mms, True)

        'If YeasNoMessage(Me, "رسالة معلومات", "هل تريد طباعة التقرير ؟") = True Then
        Print()
        'End If


        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CodeID As String = YDATE.EditValue & " - " & MDATE.EditValue & " - " & GVRole.GetRowCellValue(i, "BranchID") & " - " & GVRole.GetRowCellValue(i, "ID")
            clscs.SalaryCalculationTb_IndivdualInsert(Date.Now, GVRole.GetRowCellValue(i, "ID"), GVRole.GetRowCellValue(i, "BranchID"), GVRole.GetRowCellValue(i, "الراتب الأساسي"),
                                         GVRole.GetRowCellValue(i, "علاوات ثابتة"), GVRole.GetRowCellValue(i, "علاوات أخرى"), GVRole.GetRowCellValue(i, "خصميات متنوعة"),
                                         GVRole.GetRowCellValue(i, "باقي السلفة"), GVRole.GetRowCellValue(i, "الصافي"), MDATE.EditValue, YDATE.EditValue, CodeID, UserID, 1,
                                         0.000, 0.000, 0.000, 0.000, 0.000, 0.000, SalaryCalc, IsTotal.SelectedIndex, DDATE.EditValue, RestValue)
        Next

        clse.EMPLOYEETB_DeleteById(EMID)
        FrmSavedSuccessfully.ShowDialog()
        NEWRECORD()
    End Sub
    Private Sub FRMINDIVDUALSALARYCALC_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
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
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub Print()
        If GVRole.RowCount = 0 Then
            ErrorMessage(Me, "لا يوجد بيانات لطباعتها", "رسالة خطأ")
            Exit Sub
        End If
        Try
            Dim TypeCalc As Integer
            Dim PRR(2) As SqlParameter
            PRR(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = EMAccID}
            PRR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            If IsTotal.SelectedIndex = 1 Then
                TypeCalc = DDATE.EditValue
            Else
                TypeCalc = 0
            End If
            PRR(2) = New SqlParameter("@DDay", SqlDbType.Int) With {.Value = TypeCalc}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("ZRPT_SalaryCalc_LoadToCalculateByEMPID", PRR)
            If DTT.Rows.Count > 0 Then
                Dim report As New RPTINDIVDUALSALARYCALC
                report.DataSource = DTT
                report.DataMember = "SalaryCalculationTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                ErrorMessage(Me, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رساله تنبية ", "الرجاء إتمام عملية الإخلاء لتتم الطباعة")
        End Try
    End Sub
    Private Sub IsTotal_TextChanged(sender As Object, e As EventArgs) Handles IsTotal.TextChanged
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        Dim DDT As DateTime = Date.Now
        Dim DAY As Int32 = DDT.Day
        DDATE.EditValue = DAY
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        FRMCORRECTINDIVDUALSALARYCALC.ShowDialog()
    End Sub

    Private Sub DDATE_TextChanged(sender As Object, e As EventArgs) Handles DDATE.TextChanged
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Print()
    End Sub
    Sub CheckSelectedMonth()
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = MDATE.EditValue}
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
    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        EMPID.Properties.PopulateColumns()
        EMPID.Properties.Columns("AccID").Visible = False
    End Sub
    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        EmpAccVal.EditValue = 0.000
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_QUERY_ONLY("select AccID,ID,BranchID from EmployeeTb where IsActive=1 and AccID='" & EMPID.EditValue & "'")
        If DTT.Rows.Count > 0 Then
            EMID = DTT.Rows(0)("ID")
            BranchID = DTT.Rows(0)("BranchID")
            EMAccID = DTT.Rows(0)("AccID")
        End If
    End Sub
End Class