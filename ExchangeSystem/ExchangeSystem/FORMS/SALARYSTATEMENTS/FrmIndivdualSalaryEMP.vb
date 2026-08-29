Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmIndividualSalaryEMP
    Dim clscs As New CLSSALARYCALC
    Public RepaymentP, PaymentTMS, EMID, SalaryCalc, MonthDate, DataBaseType As Integer
    Dim AccID, EMAccID, ADVPMTACCID As ULong
    Public IsUpdate As Boolean = 0

    Private Sub FrmIndividualSalaryEMP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
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
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(67, UserID)
        If dt.Rows.Count > 0 Then
            SimpleButton1.Visible = dt.Rows(0)("CanPrint")

            BtnSave.Visible = dt.Rows(0)("CanSave")

        End If
    End Sub

    Sub NEWRECORD()
        If DataBaseType = 1 Then
            SID = GETMAXID("SalaryCalculationTb", "ID") + 1
        End If
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRNCHHasEmp(BranchID)
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        Dim MTIME As DateTime = Date.Now
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = Date.Now
        Dim ye As Integer = YTIME.Year
        Dim maxcurrmoval As Integer
        If ye >= YDATE.EditValue Then
            maxcurrmoval = month
        Else
            maxcurrmoval = 12
        End If
        MDATE.Properties.MaxValue = maxcurrmoval
        MDATE.EditValue = maxcurrmoval
        YDATE.Properties.MaxValue = ye
        YDATE.EditValue = ye
        OverAllTotal.EditValue = 0.000
        EMPID.EditValue = -1
        BtnSave.Enabled = True
        BranchID.Enabled = True
        EMPID.Enabled = True
        MDATE.Enabled = True
        YDATE.Enabled = True
        OverAllTotal.Enabled = True
        IsUpdate = False
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 148)
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            Dim CHMONTH As Integer = 0
            Dim CHYEAR As Integer = 0
            Dim eid As Integer = 0
            Dim eid2 As Integer = 0
            Dim PRR(3) As SqlParameter
            PRR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.TinyInt) With {.Value = MDATE.EditValue}
            PRR(1) = New SqlParameter("@SALARYEAR", SqlDbType.Int) With {.Value = YDATE.EditValue}
            PRR(2) = New SqlParameter("@IsIndivdual", SqlDbType.Bit) With {.Value = 0}
            PRR(3) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMID}
            Dim DTT As New DataTable
            DTT.Clear()
            If DataBaseType = 1 Then
                DTT = RUN_QUARY_PRO("SalaryCalculationTb_CheckMonthSelectedIndivdual", PRR)
            End If
            If DTT.Rows.Count > 0 Then
                CHMONTH = DTT.Rows(0)("SALARYMONTH")
                CHYEAR = DTT.Rows(0)("SALARYEAR")
                If CHMONTH = MDATE.EditValue And YDATE.EditValue = CHYEAR Then
                    ErrorMessage(Me, "رسالة خطأ", "تم احتساب مرتبات هذا الشهر مسبقاً")
                    Exit Sub
                End If
            End If

            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يجب اختيار الفرع"
                Return
            End If
            If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
                EMPID.ErrorText = "يجب اختيار الموظف"
                Return
            End If
            If OverAllTotal.EditValue <= 0.000 Then
                OverAllTotal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
                Return
            End If
            Dim customIcon As New Icon(Application.StartupPath & "\warning.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Warning) = customIcon
            '= New Size(12, 12)
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            Dim CodeID As String = YDATE.EditValue & " - " & MDATE.EditValue & " - " & BranchID.EditValue & " - " & "1"
            If DataBaseType = 1 Then
                clscs.SalaryCalculationTb_insert1(Date.Now, EMID, BranchID.EditValue, OverAllTotal.EditValue, 0.000, 0.000, 0.000, 0.000,
OverAllTotal.EditValue, MDATE.EditValue, YDATE.EditValue, CodeID, UserID, 1, 0.000, 0.000, 0.000,
                                  0.000, 0.000, 0.000, SalaryCalc, Notes.Text.Trim)
            End If

            Dim mms As String = " *شركة الرحالة القابضة*" & vbNewLine &
    "الموظف" & Space(1) & ":" & Space(1) & Me.EMPID.Text & vbNewLine &
    " تم احتساب فردي لشهر" & Space(1) & ":" & Space(1) & Me.MDATE.Text & vbNewLine &
    "صافي القيمة " & ":" & Space(1) & Cur_Code("ليبي", Me.OverAllTotal.EditValue, True, "n2") & vbNewLine &
    "مع تمنياتنا لكم بالتوفيق "
            WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(EMPID.EditValue), mms, True)

            Dim result = XtraMessageBox.Show(lookAndFeelError, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then

                Print()
            End If
            NEWRECORD()
            FrmSavedSuccessfully.ShowDialog()
        End If
    End Sub
    Public Sub SHOW_RECORD(x)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = x}
        Dim DT As New DataTable
        DT.Clear()
        If DataBaseType = 1 Then
            DT = RUN_QUARY_PRO("SalaryCalculationTb_IndividualSalaryByEMPSHOWRECORD", PR)

        End If

        If DT.Rows.Count > 0 Then
            LOADBRNCHHasEmp(BranchID)

            BranchID.EditValue = DT.Rows(0)("BranchID")
            MDATE.EditValue = DT.Rows(0)("SALARYMONTH")
            YDATE.EditValue = DT.Rows(0)("SALARYEAR")
            OverAllTotal.EditValue = DT.Rows(0)("SalaryVal")
            EMPWITHBRANCH(BranchID.EditValue, MDATE.EditValue)
            EMPID.EditValue = DT.Rows(0)("AccID")
            BtnSave.Enabled = False
            BranchID.Enabled = False
            EMPID.Enabled = False
            MDATE.Enabled = False
            YDATE.Enabled = False
            OverAllTotal.Enabled = False
        End If
    End Sub

    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.EditValue <> -1 Or EMPID.Text <> String.Empty Then
            Dim DTT As New DataTable
            DTT.Clear()
            If DataBaseType = 1 Then
                DTT = RUN_QUARY_QUERY_ONLY("select ID from EmployeeTb where IsActive=1 and EMPNAME='" & EMPID.Text.Trim & "'")
            End If

            If DTT.Rows.Count > 0 Then
                EMID = DTT.Rows(0)("ID")
            End If
        Else
            NEWRECORD()
        End If
    End Sub

    Private Sub SimpleButton1111_Click(sender As Object, e As EventArgs) Handles SimpleButton1111.Click
        FRMViewIndividualSalaryByEMP.ShowDialog()
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
    Public SID As Integer = 0
    Sub Print()
        Dim PRT As String
        If DataBaseType = 1 Then
            PRT = "ZRPT_SalaryCalculationTb_ViewIndividualSalaryByEMP"
        End If
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SID)
        Dim dt As DataTable = RUN_QUARY_PRO(PRT, PRM)
        If dt.Rows.Count > 0 Then
            dt.TableName = "SalaryCalculationTb"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            Dim report As New RPTINDEMPID
            report.DataSource = ds
            report.DataMember = "SalaryCalculationTb"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
            EMPID.ErrorText = "يجب اختيار الموظف"
            Return
        End If
        If OverAllTotal.EditValue <= 0.000 Then
            OverAllTotal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Return
        End If

        Print()
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

    Private Sub MDATE_EditValueChanged(sender As Object, e As EventArgs) Handles MDATE.EditValueChanged
        EMPID.Properties.DataSource = Nothing
        EMPID.EditValue = -1
        If IsUpdate = False Then
            If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And MDATE.Text <> String.Empty Then
            EMPWITHBRANCH(BranchID.EditValue, MDATE.EditValue)
        End If
            'If IsUpdate = False And DataBaseType = 1 Then
            'BranchID.EditValue = -1
            'EMPID.EditValue = -1
        End If
    End Sub
    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And MDATE.Text <> String.Empty Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@Salarymonth", SqlDbType.Int) With {.Value = MonthDate}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            If DataBaseType = 1 Then
                dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOIndivdualSalary", PR)
            End If
            If dt.Rows.Count > 0 Then
                EMPID.Properties.PopulateColumns()
                EMPID.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub

    Private Sub YDATE_EditValueChanged(sender As Object, e As EventArgs) Handles YDATE.EditValueChanged
        Dim MTIME As DateTime = Date.Now
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = Date.Now
        Dim ye As Integer = YTIME.Year
        Dim maxcurrmoval As Integer
        If ye = YDATE.EditValue Then
            maxcurrmoval = month
        Else
            maxcurrmoval = 12
        End If
        MDATE.Properties.MaxValue = maxcurrmoval
        MDATE.EditValue = maxcurrmoval
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        NEWRECORD()
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        EMPID.Properties.DataSource = Nothing
        EMPID.EditValue = -1
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And MDATE.Text <> String.Empty Then
            EMPWITHBRANCH(BranchID.EditValue, MDATE.EditValue)
        End If
    End Sub
    Public Sub EMPWITHBRANCH(BRID As Integer, SMonth As Integer)
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BRID}
            PR(1) = New SqlParameter("@Salarymonth", SqlDbType.Int) With {.Value = SMonth}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            If DataBaseType = 1 Then
                dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOIndivdualSalary", PR)
            End If
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "EMPNAME"
                EMPID.Properties.ShowHeader = False
            End If
        Else
            EMPID.Properties.DataSource = Nothing
        End If
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles EMPID.EditValueChanged
        If EMPID.EditValue <> -1 Or EMPID.Text <> String.Empty Then
            Dim DTT As New DataTable
            DTT.Clear()
            If DataBaseType = 1 Then
                DTT = RUN_QUARY_QUERY_ONLY("select ID from EmployeeTb where IsActive=1 and EMPNAME='" & EMPID.Text.Trim & "'")

            End If

            If DTT.Rows.Count > 0 Then
                EMID = DTT.Rows(0)("ID")
            End If
        Else
            NEWRECORD()
        End If
    End Sub

    Private Sub MDATE_TextChanged(sender As Object, e As EventArgs) Handles MDATE.TextChanged
        MonthDate = MDATE.EditValue
        If IsUpdate = False And DataBaseType = 1 Then
            BranchID.EditValue = -1
            EMPID.EditValue = -1
        End If
    End Sub

    Private Sub FrmIndividualSalaryEMP_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        NEWRECORD()
    End Sub
    'Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    'Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
End Class