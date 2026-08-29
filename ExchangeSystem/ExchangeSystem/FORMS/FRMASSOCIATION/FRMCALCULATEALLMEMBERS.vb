Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRMCALCULATEALLMEMBERS
    Sub NEWRECORD()
        MDATE.EditValue = Date.Now
        YDATE.EditValue = Date.Now
        DVGFROMAT()
        GVRole.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        OverallNetTotal.EditValue = 0.000
        LOADASSOCIATION()
        ASSOCIATION.EditValue = -1
    End Sub
    Public Sub LOADASSOCIATION()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ASSOCIATIONNAMETB_LOADTODVG")
        If DT.Rows.Count > 0 Then
            ASSOCIATION.Properties.DataSource = DT
            ASSOCIATION.Properties.DisplayMember = "ASSNAME"
            ASSOCIATION.Properties.ValueMember = "ID"
            ASSOCIATION.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(94, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always





        End If


    End Sub
    Sub LoadData()
        GVRole.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        OverallNetTotal.EditValue = 0.000
        Dim MTIME As DateTime = MDATE.EditValue
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = YDATE.EditValue
        Dim ye As Integer = YTIME.Year
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = month}
        PR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = ye}
        PR(2) = New SqlParameter("@ASSOCIATIONID", SqlDbType.Int) With {.Value = ASSOCIATION.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("MembersCalculationTb_LOADDATATOCALC", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("ID").Visible = False
            GVRole.Columns("MeAccID").Visible = False
            GVRole.Columns("AccID").Visible = False
            GVRole.Columns("ASSOACCID").Visible = False
            GVRole.Columns("ASSOCIATIONID").Visible = False
            GVRole.Columns("PHONE").Visible = False
            DVGFROMAT()
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
            Dim OverallNet As New GridColumnSummaryItem()
            OverallNet.SummaryType = SummaryItemType.Sum
            OverallNet.FieldName = "قيمة الاشتراك"
            GVRole.Columns("قيمة الاشتراك").Summary.Add(OverallNet)
            GVRole.Columns("قيمة الاشتراك").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            OverallNetTotal.EditValue = 0.000
            OverallNetTotal.EditValue = Convert.ToDouble(GVRole.Columns("قيمة الاشتراك").SummaryItem.SummaryValue)
        End If
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
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
    Sub CheckSelectedMonth()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = MDATE.EditValue}
        PR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = YDATE.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AdvancePaymentTb_GetADPMNTTOUPDATE", PR)
        If DT.Rows.Count > 0 Then
            XtraMessageBox.Show(lookAndFeelError, "تم احتساب مرتبات هذا الشهر مسبقاً", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
    End Sub

    Private Sub FRMCALCULATEALLMEMBERS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Function BuildTransENmasgeferMessage(EmNAme As String, selPrase As Double) As String
        Dim message As String = My.Settings.Combny_name & vbNewLine &
            "المشترك بجمعية العدايل" & vbNewLine &
        "السيد : " & EmNAme & vbNewLine &
        "عليك إشتراك شهر : " & MDATE.Text & "-" & YDATE.Text & vbNewLine &
        "بقيمة : " & Cur_Code("ليبي", selPrase, True, "n2") & vbNewLine
        message &= "مع خالص تحياتنا ،،،،،،،"

        Return message
    End Function

    Public Sub SenForWansapp_SalaryCalc_LoadToCalculate()
        Try
            If GVRole.RowCount > 0 Then
                SplashScreenManager1.ShowWaitForm()
                For i = 0 To GVRole.RowCount - 1
                    ' GVRole.GetRowCellValue(i, ("PHONE"))
                    'WATSAPPMsAG(GVRole.GetRowCellValue(i, ("PHONE"), BuildTransENmasgeferMessage(GVRole.GetRowCellValue(i, ("الاسم")), GVRole.GetRowCellValue(i, ("قيمة الاشتراك"))))
                    WATSAPPMsAG(GVRole.GetRowCellValue(i, ("PHONE")), BuildTransENmasgeferMessage(GVRole.GetRowCellValue(i, ("الاسم")), GVRole.GetRowCellValue(i, ("قيمة الاشتراك"))), True)
                Next
                SplashScreenManager1.CloseWaitForm()
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show($"رسالة تنبية : {ex.Message}")
        End Try
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
        DVGFROMAT()
    End Sub

    Sub Print()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True

        Dim MTIME As DateTime = MDATE.EditValue
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = YDATE.EditValue
        Dim ye As Integer = YTIME.Year

        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@SALARYMONTH", month)
            PRM(1) = New SqlParameter("@SALARYEAR", ye)
            PRM(2) = New SqlParameter("@ASSOCIATIONID", ASSOCIATION.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_MembersCalculationTb_LOADDATATOCALC", PRM)
            dt.TableName = "SalaryCalculationTb"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then

                Dim report As New RPTCALCULATEALLMEMBERS
                report.FilterString = GVRole.ActiveFilterString
                report.DataSource = ds
                report.DataMember = "SalaryCalculationTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)

                report.CreateDocument()
                report.ShowPreview()

            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Dim clscs As New CalculateMemebers
    Public RepaymentP, PaymentTMS, SalaryCalc As Integer

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Print()
    End Sub

    Private Sub ASSOCIATION_TextChanged(sender As Object, e As EventArgs) Handles ASSOCIATION.TextChanged
        GCRole.DataSource= Nothing
        OverallNetTotal.EditValue = 0.000
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If ASSOCIATION.Text = String.Empty Then
            ASSOCIATION.ErrorText = "يجب اختيار الجمعية أولاً"
            Exit Sub
        End If
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
        Dim MTIME As DateTime = MDATE.EditValue
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = YDATE.EditValue
        Dim ye As Integer = YTIME.Year
        Dim PRR(3) As SqlParameter
        PRR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = month}
        PRR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = ye}
        PRR(2) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = 0}
        PRR(3) = New SqlParameter("@AssoID", SqlDbType.Int) With {.Value = ASSOCIATION.EditValue}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("MembersCalculationTb_CheckMonthSelected", PRR)
        If DTT.Rows.Count > 0 Then
            CHMONTH = DTT.Rows(0)("SALARYMONTH")
            CHYEAR = DTT.Rows(0)("SALARYEAR")
            If CHMONTH = month And ye = CHYEAR Then
                XtraMessageBox.Show(lookAndFeelError, "تم احتساب اشتراك هذا الشهر مسبقاً", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        ElseIf DTT.Rows.Count = 0 Then
            Dim CurrentMontDate As DateTime = MDATE.EditValue
            Dim CuMonth As Integer = CurrentMontDate.Month

            Dim PreCurrentMontDate As DateTime = Date.Now
            Dim PreCuMonth As Integer = CurrentMontDate.Month - 1
            If month > CHMONTH Then
                Dim resu = XtraMessageBox.Show(lookAndFeelError, "لم يتم احتساب شهر" & Space(1) & PreCuMonth & Space(1) & "هل تريد الاستمرار في الاحتساب", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                If resu = DialogResult.Yes Then
                    Dim lookAndFeelError2 As New UserLookAndFeel(Me)
                    lookAndFeelError2.Style = LookAndFeelStyle.Skin
                    lookAndFeelError2.UseDefaultLookAndFeel = False
                    lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
                    XtraMessageBox.AllowCustomLookAndFeel = True
                    Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If result = DialogResult.Yes Then
                        Print()
                    End If
                    SenForWansapp_SalaryCalc_LoadToCalculate()
                    For i As Integer = 0 To GVRole.RowCount - 1
                        Dim MOTYPE As String = ""
                        Dim CodeID As String = ye & " - " & month & " - " & MAINBID & " - " & GVRole.GetRowCellValue(i, "ASSOCIATIONID") & " - " & GVRole.GetRowCellValue(i, "#")
                        clscs.MembersCalc_insert(Date.Now, GVRole.GetRowCellValue(i, "ID"), GVRole.GetRowCellValue(i, "قيمة الاشتراك"), GVRole.GetRowCellValue(i, "قيمة الاشتراك"),
                                                         month, ye, CodeID, UserID, 0, 1, GVRole.GetRowCellValue(i, "MeAccID"), ASSOCIATION.EditValue)
                    Next
                    FrmSavedSuccessfully.ShowDialog()
                Else
                    Exit Sub
                End If
            End If
        End If
        NEWRECORD()
    End Sub

    Private Sub ASSOCIATION_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ASSOCIATION.QueryPopUp
        ASSOCIATION.Properties.PopulateColumns()
        ASSOCIATION.Properties.Columns("ID").Visible = False
    End Sub
End Class
Public Class CalculateMemebers
    Public Sub MembersCalc_insert(INSERTDATE As Date, EMPID As Integer, SalaryVal As Decimal, SALARYTOTAL As Decimal, SALARYMONTH As Int32, SALARYEAR As Integer, CodeID As String,
                                          SafeID As Integer, IsIndivdual As Boolean, SalaryCalc As Integer, MeccID As ULong, AssoID As Integer)
        Dim prm(11) As SqlParameter
        prm(0) = New SqlParameter("@INSERTDATE", SqlDbType.Date) With {.Value = INSERTDATE}
        prm(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        prm(2) = New SqlParameter("@SalaryVal", SqlDbType.Decimal) With {.Value = SalaryVal}
        prm(3) = New SqlParameter("@SALARYTOTAL", SqlDbType.Decimal) With {.Value = SALARYTOTAL}
        prm(4) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = SALARYMONTH}
        prm(5) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = SALARYEAR}
        prm(6) = New SqlParameter("@CodeID", SqlDbType.NVarChar, -1) With {.Value = CodeID}
        prm(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(8) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = IsIndivdual}
        prm(9) = New SqlParameter("@SalaryCalc", SqlDbType.Int) With {.Value = SalaryCalc}
        prm(10) = New SqlParameter("@MeccID", SqlDbType.BigInt) With {.Value = MeccID}
        prm(11) = New SqlParameter("@AssoID", SqlDbType.Int) With {.Value = AssoID}
        RUN_EXUTE_PRO("MembersCalculationTb_Insert", prm)
    End Sub
End Class