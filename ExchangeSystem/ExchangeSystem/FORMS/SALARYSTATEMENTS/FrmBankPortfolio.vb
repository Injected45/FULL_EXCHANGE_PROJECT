Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FrmBankPortfolio
    Public Sub LoadBank()
        Dim MTIME As DateTime
        Dim YTIME As DateTime

        If TypeOf D1.EditValue Is DateOnly Then
            MTIME = CType(D1.EditValue, DateOnly).ToDateTime(TimeOnly.MinValue)
        Else
            MTIME = Convert.ToDateTime(D1.EditValue)
        End If

        If TypeOf D2.EditValue Is DateOnly Then
            YTIME = CType(D2.EditValue, DateOnly).ToDateTime(TimeOnly.MinValue)
        Else
            YTIME = Convert.ToDateTime(D2.EditValue)
        End If
        Dim month As Int32 = MTIME.Month
        Dim ye As Integer = YTIME.Year
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 6}
        PR(1) = New SqlParameter("@SALARYMONTH", SqlDbType.Int) With {.Value = month}
        PR(2) = New SqlParameter("@SALARYYEAR", SqlDbType.Int) With {.Value = ye}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ExBanksTb_CRUD", PR)
        If dt.Rows.Count > 0 Then
            BankID.Properties.DataSource = dt
            BankID.Properties.ValueMember = "ID"
            BankID.Properties.DisplayMember = "BankName"
            BankID.Properties.ShowHeader = False
            BankID.Properties.PopulateColumns()
            BankID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Public Sub LoadBranch()
        If BankID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BankID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_LOADTOLKPBasedOnBankID_Haofez", PR)
            If dt.Rows.Count > 0 Then
                BranchID.Properties.DataSource = dt
                BranchID.Properties.ValueMember = "AccountNo"
                BranchID.Properties.DisplayMember = "BranchName"
                BranchID.Properties.ShowHeader = False
                BranchID.Properties.PopulateColumns()
                BranchID.Properties.Columns("AccountNo").Visible = False
            End If
        End If
    End Sub

    Public Sub LoadData()
        If BankID.EditValue Is Nothing Or BankID.Text = String.Empty Then
            BankID.ErrorText = "يرجى اختيار اسم البنك"
            BankID.Select()
            Exit Sub
        End If
        'If BranchID.EditValue Is Nothing Or BranchID.Text = String.Empty Then
        '    BranchID.ErrorText = "يرجى اختيار اسم الفرع"
        '    BranchID.Select()
        '    Exit Sub
        'End If
        GVRole.OptionsBehavior.Editable = False
        GCROLE.DataSource = Nothing
        GVRole.Columns.Clear()

        Dim MTIME As DateTime
        Dim YTIME As DateTime

        If TypeOf D1.EditValue Is DateOnly Then
            MTIME = CType(D1.EditValue, DateOnly).ToDateTime(TimeOnly.MinValue)
        Else
            MTIME = Convert.ToDateTime(D1.EditValue)
        End If

        If TypeOf D2.EditValue Is DateOnly Then
            YTIME = CType(D2.EditValue, DateOnly).ToDateTime(TimeOnly.MinValue)
        Else
            YTIME = Convert.ToDateTime(D2.EditValue)
        End If


        Dim month As Int32 = MTIME.Month
        Dim ye As Integer = YTIME.Year
        Try
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.Int) With {.Value = month}
            PR(1) = New SqlParameter("@SALARYYEAR", SqlDbType.Int) With {.Value = ye}
            PR(2) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID.EditValue}
            PR(3) = New SqlParameter("@BBranchID", SqlDbType.Int) With {.Value = 0}
            PR(4) = New SqlParameter("@TotalSalary", SqlDbType.Decimal)
            PR(4).Direction = ParameterDirection.Output
            PR(4).Precision = 18
            PR(4).Scale = 3
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("SalaryCalculation_LoadToBankPortfolio", PR)
            If DT.Rows.Count > 0 Then
                GCROLE.DataSource = DT
                'GVRole.Columns("ID").Visible = False
                'GVRole.Columns("BranchID").Visible = False
                OverAllTotal.EditValue = Convert.ToDecimal(PR(4).Value)
                DVGFROMAT()
            End If
        Catch ex As SqlException
            ErrorMessage(Me, "رسالة معلومات", ex.Message)
        Catch ex As Exception
            ErrorMessage(Me, "خطأ غير متوقع", ex.Message)
        End Try
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
    Private Sub BankID_EditValueChanged(sender As Object, e As EventArgs) Handles BankID.EditValueChanged
        If BankID.Text <> String.Empty Then
            BranchID.Properties.DataSource = Nothing
            LoadBranch()
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
    End Sub

    Private Sub FrmBankPortfolio_Load(sender As Object, e As EventArgs) Handles Me.Load
        BankID.EditValue = Nothing
        BranchID.EditValue = Nothing
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        GCROLE.DataSource = Nothing
        OverAllTotal.EditValue = 0D
        LoadBank()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If BankID.EditValue Is Nothing Or BankID.Text = String.Empty Then
            BankID.ErrorText = "يرجى اختيار اسم البنك"
            BankID.Select()
            Exit Sub
        End If
        'If BranchID.EditValue Is Nothing Or BranchID.Text = String.Empty Then
        '    BranchID.ErrorText = "يرجى اختيار اسم الفرع"
        '    BranchID.Select()
        '    Exit Sub
        'End If
        GVRole.OptionsBehavior.Editable = False


        Dim MTIME As DateTime
        Dim YTIME As DateTime

        If TypeOf D1.EditValue Is DateOnly Then
            MTIME = CType(D1.EditValue, DateOnly).ToDateTime(TimeOnly.MinValue)
        Else
            MTIME = Convert.ToDateTime(D1.EditValue)
        End If

        If TypeOf D2.EditValue Is DateOnly Then
            YTIME = CType(D2.EditValue, DateOnly).ToDateTime(TimeOnly.MinValue)
        Else
            YTIME = Convert.ToDateTime(D2.EditValue)
        End If


        Dim month As Int32 = MTIME.Month
        Dim ye As Integer = YTIME.Year
        Try
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@SALARYMONTH", SqlDbType.Int) With {.Value = month}
            PR(1) = New SqlParameter("@SALARYYEAR", SqlDbType.Int) With {.Value = ye}
            PR(2) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID.EditValue}
            PR(3) = New SqlParameter("@BBranchID", SqlDbType.Int) With {.Value = 0}
            PR(4) = New SqlParameter("@TotalSalary", SqlDbType.Decimal)
            PR(4).Direction = ParameterDirection.Output
            PR(4).Precision = 18
            PR(4).Scale = 3
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("SalaryCalculation_LoadToBankPortfolio", PR)
            If DT.Rows.Count > 0 Then
                If GVRole.Columns.Count > 0 Then
                    Dim report As New RptBankPortfolio
                    report.DataSource = DT

                    report.DataMember = "SalaryCalculationTb"
                    report.BankID.Text = BankID.Text
                    report.XrLabel2.Text = D1.Text
                    report.XrLabel6.Text = D2.Text
                    report.XrLabel11.Text = GetLKPColumnVal(BranchID, "AccountNo")
                    report.ArLetters.Text = Cur_Code("دينار ليبي", Convert.ToDecimal(PR(4).Value).ToString("N3"), False)
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()
                Else
                    ErrorMessage(Me, "رسالة معلومات", "القائمة لا يجب أن تكون فارغة")
                End If
            End If
        Catch ex As SqlException
            ErrorMessage(Me, "رسالة معلومات", ex.Message)
        Catch ex As Exception
            ErrorMessage(Me, "خطأ غير متوقع", ex.Message)
        End Try
    End Sub

    Private Sub D1_EditValueChanged(sender As Object, e As EventArgs) Handles D1.EditValueChanged
        BankID.Properties.DataSource = Nothing
        BankID.EditValue = Nothing
        GCROLE.DataSource = Nothing
        OverAllTotal.EditValue = 0D
        LoadBank()
    End Sub
End Class