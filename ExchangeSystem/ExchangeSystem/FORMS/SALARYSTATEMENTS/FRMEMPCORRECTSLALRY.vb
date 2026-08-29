Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FRMEMPCORRECTSLALRY
    Dim CLSEMD As New CLSEMINCREASE
    Dim clscs As New CLSSALARYCALC
    Public EMID, SalaryCalc, BranchID As Integer
    Dim AccID, EMAccID, ADVPMTACCID As ULong
    Sub NEWRECORD()
        GVRole.OptionsBehavior.Editable = False
        GVRole1.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        GCRole1.DataSource = Nothing
        DVGFROMAT()
        DVGFROMAT1()
        LOADEMP()
        Dim MTIME As DateTime = Date.Now
        Dim month As Int32 = MTIME.Month
        Dim YTIME As DateTime = Date.Now
        Dim ye As Integer = YTIME.Year
        MDATE.EditValue = month
        YDATE.EditValue = ye
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
    Sub DVGFROMAT1()
        GVRole1.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole1.OptionsBehavior.Editable = False
        GVRole1.OptionsBehavior.EditingMode = False
        GVRole1.OptionsBehavior.ReadOnly = True
        GVRole1.OptionsView.ShowGroupPanel = False
        GVRole1.OptionsView.ShowFooter = False
        GVRole1.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole1.OptionsSelection.MultiSelectMode = False
        GVRole1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole1.Columns.Count - 1
            GVRole1.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole1.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole1.OptionsView.EnableAppearanceEvenRow = True
        GVRole1.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole1.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub LOADDATA()
        If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
            EMPID.ErrorText = "الرجاء إختيار الموظف"
            Exit Sub
        End If
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID.EditValue}
        PR(1) = New SqlParameter("@MDATE", SqlDbType.Int) With {.Value = MDATE.EditValue}
        PR(2) = New SqlParameter("@YDATE", SqlDbType.Int) With {.Value = YDATE.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SalaryCalculationTb_GetEMPSalaryByEMPID", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("BranchID").Visible = False
            GVRole.Columns("EMPID").Visible = False

            BranchID = DT.Rows(0)("BranchID")
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID.EditValue}
            PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("SalaryCalc_LoadToCalculateByEMPID", PRM)
            If DTT.Rows.Count > 0 Then
                GCRole1.DataSource = DTT
                GVRole1.Columns("BranchID").Visible = False
                GVRole1.Columns("ID").Visible = False
            End If
        Else
            GCRole.DataSource = Nothing
            GCRole1.DataSource = Nothing
            XtraMessageBox.Show(lookAndFeelError, "لا يوجد بيانات لعرضها لهذا الموظف", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        DVGFROMAT()
        DVGFROMAT1()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LOADDATA()
    End Sub

    Private Sub FRMEMPCORRECTSLALRY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.Text.Trim <> String.Empty Then

            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, (300)) With {.Value = EMPID.Text.Trim}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("EMPLOYEETB_GETEMPIDID", PRM)
            If DTT.Rows.Count > 0 Then
                EMID = DTT.Rows(0)("ID")

            End If
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

    Sub Print()

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
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", EMPID.EditValue)
            PRM(1) = New SqlParameter("@BranchID", BranchID)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_SalaryCalc_LoadToCalculateByEMPAccID", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "SalaryCalculationTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTEMPCORRECTSLALRY
                report.DataSource = ds
                report.DataMember = "SalaryCalculationTb"
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

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
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
    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click

        If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
            EMPID.ErrorText = "الرجاء إختيار الموظف"
            Exit Sub
        End If

        Dim lookAndFeelError2 As New UserLookAndFeel(Me)

        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID.EditValue}
        PR(1) = New SqlParameter("@MDATE", SqlDbType.Int) With {.Value = MDATE.EditValue}
        PR(2) = New SqlParameter("@YDATE", SqlDbType.Int) With {.Value = YDATE.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SalaryCalculationTb_GetEMPSalaryByEMPID", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("BranchID").Visible = False
            GVRole.Columns("EMPID").Visible = False

            BranchID = DT.Rows(0)("BranchID")
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID.EditValue}
            PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("SalaryCalc_LoadToCalculateByEMPID", PRM)
            If DTT.Rows.Count > 0 Then
                GCRole1.DataSource = DTT
                GVRole1.Columns("BranchID").Visible = False
                GVRole1.Columns("ID").Visible = False
            End If

            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2

            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

            If result = DialogResult.Yes Then
                Print()
                'Else
                '    Exit Sub
            End If

        Else
            GCRole.DataSource = Nothing
            GCRole1.DataSource = Nothing
            XtraMessageBox.Show(lookAndFeelError2, "لا يوجد بيانات لعرضها لهذا الموظف", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If





        Dim MOTYPE As String = ""
        Dim CodeID As String = ""
        'For i As Integer = 0 To GVRole.RowCount - 1
        Dim i As Integer = GVRole.RowCount - 1
        For x As Integer = 0 To GVRole1.RowCount - 1
                CodeID = GVRole.GetRowCellValue(i, "الرمز")
                clscs.SalaryCalculationTb_insert1(Date.Now, GVRole1.GetRowCellValue(x, "ID"), GVRole.GetRowCellValue(i, "BranchID"),
                                                  GVRole.GetRowCellValue(i, "الراتب الأساسي"), GVRole.GetRowCellValue(i, "علاوة ثابتة"),
                                                  GVRole.GetRowCellValue(i, "علاوة مؤقتة"), GVRole.GetRowCellValue(i, "خصميات"), GVRole.GetRowCellValue(i, "خصم سلفة"),
                                                  GVRole.GetRowCellValue(i, "الصافي"), MDATE.EditValue, YDATE.EditValue, CodeID, UserID, 0,
                                                  GVRole1.GetRowCellValue(x, "الراتب الأساسي"), GVRole1.GetRowCellValue(x, "علاوات ثابتة"),
                                                  GVRole1.GetRowCellValue(x, "علاوات أخرى"), GVRole1.GetRowCellValue(x, "خصم السلفة"),
                                                  GVRole1.GetRowCellValue(x, "الصافي"), GVRole1.GetRowCellValue(x, "خصميات متنوعة"), SalaryCalc, "")
            Next
            'Next
            NEWRECORD()
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        EMPID.Properties.PopulateColumns()
        EMPID.Properties.Columns("AccID").Visible = False
    End Sub
End Class