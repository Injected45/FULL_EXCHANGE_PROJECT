Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Security.Cryptography.X509Certificates
Imports DevExpress.CodeParser
Imports DevExpress.Diagram.Core.Native
Imports DevExpress.XtraEditors
Imports DevExpress.XtraPrinting.Shape.Native
Imports DevExpress.XtraReports.UI

Public Class FrmLeave
    Public IsUpdate As Integer, EMPCODE As String, DiscountIs, MSG, DaysNum, isEnd As Integer
    Public CompanyNAme, BossName, Department As String
    Public Sub NewRecord()
        New_Controlrs(Me)
        LoadToControlar(LeaveTypeID, "AddLeaveTypeTb_LOADTOLSBOX", "LeaveName", "ID", Nothing)
        LoadToControlar(BranchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        EnabledControls(True)
        IsUpdate = 0
        BranchID.EditValue = BID
        BtnEdit.Caption = "تمديد الإجازة"
        BtnDelete.Caption = "إنهاء الإجازة"
        D1.EditValue = Date.Now
        BranchID.Select()
        lodePreportes()
        DaysNumber.Properties.MaxValue = 30
    End Sub
    Public Sub EnabledControls(IsEnabled As Boolean)
        Enable_Controls(Me, IsEnabled)
        IsActiveTG.Enabled = IsEnabled
        IsAbsence.Enabled = Not IsEnabled
        BtnEdit.Enabled = False
        BtnDelete.Enabled = Not IsEnabled
        BtnPrint.Enabled = Not IsEnabled
        BtnSave.Enabled = IsEnabled
        AbsenceID.Enabled = False
        IsDiscount.Enabled = False
        ABDays.Enabled = False
        DiscountVal.Enabled = False
        D2.Enabled = False
        D3.Enabled = False
    End Sub
    'Private Sub ResetFormControls()
    '    Code.Text = String.Empty
    '    BranchID.EditValue = -1
    '    EMPID.EditValue = -1
    '    VacationType.SelectedIndex = -1
    '    LeaveType.SelectedIndex = -1
    '    LeaveTypeID.EditValue = -1
    '    Notes.Text = String.Empty
    '    IsAbsence.Checked = False
    '    ABDays.EditValue = 0
    '    DiscountVal.EditValue = 0D
    '    DaysNumber.EditValue = 0
    '    IsDiscount.Enabled = False
    '    DiscountVal.Enabled = False
    '    'Code.Enabled = False
    '    Code.ReadOnly = True
    '    D1.EditValue = Date.Now
    '    D2.EditValue = Date.Now
    '    D3.EditValue = Date.Now
    'End Sub
    'Private Sub ConfigureButtons()
    '    BtnEdit.Caption = "تمديد الإجازة"
    '    BtnDelete.Caption = "إنهاء الإجازة"
    '    BtnEdit.Enabled = False
    '    BtnDelete.Enabled = False
    '    BtnSave.Enabled = True
    'End Sub
    'Public Sub ACTIVETOOLS(isEnabled As Boolean)
    '    Dim controlsToToggle As Control() = {
    '         VacationType, EMPID,
    '        LeaveType, LeaveTypeID, DaysNumber, Notes
    '    }

    '    'Code.Enabled = False
    '    Code.ReadOnly = True

    '    For Each ctrl As Control In controlsToToggle
    '        ctrl.Enabled = isEnabled
    '    Next

    '    If IsUpdate = 0 Then
    '        IsAbsence.Enabled = isEnabled
    '        AbsenceID.Enabled = isEnabled
    '        ABDays.Enabled = isEnabled
    '        DiscountVal.Enabled = isEnabled
    '    End If
    'End Sub
    Private Sub LeavType_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LeaveTypeID.ButtonClick
        If e.Button.Index = 1 Then
            ShowLeaveTypeForm()
        End If
    End Sub
    Private Sub ShowLeaveTypeForm()
        Using leaveTypeForm As New FrmAddLeaveType()
            leaveTypeForm.ShowDialog()
        End Using
    End Sub
    Private Sub FrmLeave_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IsUpdate = 0 Then
            NewRecord()
        End If
    End Sub
    Sub SHOW_EMCUSCODE(x)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = x}
        Dim results As DataTable = RUN_QUARY_PRO("LeaveTB_Select", PR)
        If results Is Nothing OrElse results.Rows.Count = 0 Then
            ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
            Exit Sub
        End If
        Dim row As DataRow = results.Rows(0)
        Try
            IsUpdate = 1
            EnabledControls(False)
            DiscountIs = 1
            BranchID.EditValue = SafeGetValue(row, "DBRID")
            VacationType.SelectedIndex = SafeGetValue(row, "VacationType", -1)
            D1.EditValue = SafeGetValue(row, "DateFrom")
            IsActiveTG.EditValue = SafeGetValue(row, "IsActive")
            EMPID.EditValue = SafeGetValue(row, "EMPID")
            DaysNumber.EditValue = SafeGetValue(row, "LDays")
            DaysNumber.Properties.MaxValue = SafeGetValue(row, "LDays")
            DaysNumber.Enabled = True
            DaysNum = SafeGetValue(row, "LDays")
            LeaveType.SelectedIndex = SafeGetValue(row, "LeaveType", -1)
            LeaveTypeID.EditValue = SafeGetValue(row, "LeaveTypeID")
            Notes.Text = SafeGetValue(row, "Notes", String.Empty)
            Dim isDis As Integer = SafeGetValue(row, "IsDiscount")
            If Application.OpenForms().OfType(Of FRMLeaveConfirm).Any Then
                isEnd = 1
            Else
                isEnd = SafeGetValue(row, "EndLeave")
            End If
            Dim isAbs As Boolean = SafeGetValue(row, "IsAbsence", False)
            If isAbs Then
                IsAbsence.Checked = True
                IsAbsence.Enabled = False
                AbsenceID.Enabled = False
                ABDays.Enabled = False
                IsDiscount.Enabled = False
                AbsenceID.SelectedIndex = SafeGetValue(row, "AbsenceID", -1)
                ABDays.EditValue = SafeGetValue(row, "ABDays")
                DiscountVal.EditValue = SafeGetValue(row, "DiscountVal")
            Else
                IsAbsence.Checked = False
            End If
            Select Case isDis
                Case 0
                    IsDiscount.Enabled = False
                Case 1
                    IsDiscount.SelectedIndex = 0
                Case 2
                    IsDiscount.SelectedIndex = 1
            End Select
            Code.Text = SafeGetValue(row, "Code").ToString
        Catch ex As Exception
            XtraMessageBox.Show("حدث خطأ أثناء تحميل البيانات: " & ex.Message,
                          "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub WhatsUp()
        If IsUpdate = 0 Then
            Dim mms As String = " *شركة الرحالة القابضة*" & vbNewLine &
"الاسم " & ":" & Space(1) & Me.EMPID.Text & vbNewLine &
        "الرقم الوظيفي " & ":" & Space(1) & GET_EMPcodefor_Acount_SaenFroWtsaap(Me.EMPID.EditValue) & vbNewLine &
        "نُحيطكم علمًا بأنه تم منحكم إجازة" & vbNewLine & Me.LeaveTypeID.Text & Space(1) & "(" & Me.LeaveType.Text & ")" & vbNewLine &
        "لمدة" & ":" & Space(1) & Me.DaysNumber.Text & Space(1) & "ايام" & vbNewLine &
        "على أن تكون المباشرة بتاريخ" & ":" & Space(1) & Me.D3.Text & vbNewLine &
        "سائلين الله لكم التوفيق والسلامة"
            WATSAPPMsAG(GET_EMPPHONE_SaenFroWtsaap(Me.EMPID.EditValue), mms, True)
        End If

    End Sub
    Sub WhatsUp2()
        Try
            If IsUpdate <> 0 Then

                Dim mms As String = " *شركة الرحالة القابضة*" & vbNewLine &
    "الاسم " & ":" & Space(1) & Me.EMPID.Text & vbNewLine &
            "الرقم الوظيفي " & ":" & Space(1) & GET_EMPcodefor_Acount_SaenFroWtsaap(Me.EMPID.EditValue) & vbNewLine &
            "نُحيطكم علمًا بأنتهاء اجازتكم وعودة سيادتكم لمباشرة العمل " & vbNewLine &
            "اعتبارًا من " & ":" & Space(1) & Me.D3.Text & vbNewLine &
            "نتمنى لكم دوام التوفيق والنجاح"
                WATSAPPMsAG(GET_EMPPHONE_SaenFroWtsaap(Me.EMPID.EditValue), mms, True)
            End If
        Catch ex As Exception
            ErrorMessage(Me, "حدث خطأ في العملية، يرجى المحاولة في وقت لاحق", ex.Message)
        End Try
    End Sub
#Region "SafeGetValue"
    Private Function SafeGetValue(row As DataRow, columnName As String, Optional defaultValue As Object = Nothing) As Object
        If row.Table.Columns.Contains(columnName) AndAlso Not IsDBNull(row(columnName)) Then
            Return row(columnName)
        End If
        Return defaultValue
    End Function
    Private Function SafeGetValue(Of T)(row As DataRow, columnName As String, Optional defaultValue As T = Nothing) As T
        If row.Table.Columns.Contains(columnName) AndAlso Not IsDBNull(row(columnName)) Then
            Try
                Return CType(row(columnName), T)
            Catch
                Return defaultValue
            End Try
        End If
        Return defaultValue
    End Function
    Private Function GetSafeString(value As Object) As String
        Return If(value Is Nothing, String.Empty, value.ToString().Trim())
    End Function

    Private Function GetSafeInteger(value As Object) As Integer
        If value Is Nothing Then Return 0
        Dim result As Integer
        Integer.TryParse(value.ToString(), result)
        Return result
    End Function

    Private Function GetSafeDecimal(value As Object) As Decimal
        If value Is Nothing Then Return 0D
        Dim result As Decimal
        Decimal.TryParse(value.ToString(), result)
        Return result
    End Function

    Private Function GetSafeDate(value As Object) As Date
        If value Is Nothing Then Return Date.MinValue
        Dim result As Date
        Date.TryParse(value.ToString(), result)
        Return result
    End Function

    Private Function GetSafeBoolean(value As Object) As Boolean
        If value Is Nothing Then Return False
        Dim result As Boolean
        Boolean.TryParse(value.ToString(), result)
        Return result
    End Function
#End Region
    Public Sub InsertOrUpdate()
        Try
            If Not ValidateRequiredFields() Then Exit Sub
            Dim success As Boolean = PerformLeaveOperation()
            If success Then
                Print()
                If IsUpdate <> 0 Then
                    WhatsUp2()
                ElseIf IsUpdate = 0 Then
                    WhatsUp()
                Else
                End If
                NewRecord()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "حدث خطأ في العملية، يرجى المحاولة في وقت لاحق", ex.Message)
        End Try
    End Sub

    Private Function ValidateRequiredFields() As Boolean
        If EMPID.EditValue Is Nothing OrElse CInt(EMPID.EditValue) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "يجب اختيار الموظف")
            Return False
        End If

        If DaysNumber.EditValue Is Nothing OrElse DaysNumber.EditValue =0 Then
            ErrorMessage(Me, "خطأ في البيانات", "يجب تحديد عدد أيام الإجازة")
            Return False
        End If
        If VacationType.Text Is Nothing OrElse CInt(VacationType.SelectedIndex) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "يجب اختيار طبيعة الإجازة")
            Return False
        End If
        If BranchID.EditValue Is Nothing OrElse CInt(BranchID.EditValue) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "هذا الحقل مطلوب")
            Return False
        End If
        If LeaveType.Text Is Nothing OrElse CInt(LeaveType.SelectedIndex) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "هذا الحقل مطلوب")
            Return False
        End If
        If LeaveTypeID.EditValue Is Nothing OrElse CInt(LeaveTypeID.EditValue) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "هذا الحقل مطلوب")
            Return False
        End If
        If IsAbsence.Checked = True Then
            If AbsenceID.Text Is Nothing OrElse CInt(AbsenceID.SelectedIndex) = -1 Then
                ErrorMessage(Me, "خطأ في البيانات", "هذا الحقل مطلوب")
                Return False
            End If
            If CInt(ABDays.EditValue) <= 0 Then
                ErrorMessage(Me, "خطأ في البيانات", "هذه القيمة لا يجب أن تكون صفر أو أصغر")
                Return False
            End If
            If AbsenceID.SelectedIndex = 2 Then
                If CInt(DiscountVal.EditValue) <= 0.000 Then
                    ErrorMessage(Me, "خطأ في البيانات", "هذه القيمة لا يجب أن تكون صفر أو أصغر")
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Private Function PerformLeaveOperation() As Boolean
        Dim parameters As List(Of SqlParameter) = CreateParameters()
        RUN_EXUTE_PRO("LeaveTB_Insert", parameters.ToArray())
        Dim status As Integer = CInt(parameters.FirstOrDefault(Function(p) p.ParameterName = "@MSGSTatues").Value)
        Dim message As String = parameters.FirstOrDefault(Function(p) p.ParameterName = "@MsgBox").Value.ToString()
        MSG = status
        If status = 0 Then
            ErrorMessage(Me, "رسالة خطأ", message)
            Return False
        End If

        Return True
    End Function

    Private Function CreateParameters() As List(Of SqlParameter)
        Dim parameters As New List(Of SqlParameter)
        parameters.Add(New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = GetSafeString(Code.Text.Trim)})
        parameters.Add(New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = GetSafeInteger(EMPID.EditValue)})
        parameters.Add(New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = GetSafeInteger(BranchID.EditValue)})
        parameters.Add(New SqlParameter("@DateFrom ", SqlDbType.Date) With {.Value = GetSafeDate(D1.EditValue)})
        parameters.Add(New SqlParameter("@DaysNumber", SqlDbType.Int) With {.Value = GetSafeInteger(DaysNumber.EditValue)})
        parameters.Add(New SqlParameter("@IsDiscount", SqlDbType.Int) With {.Value = GetSafeInteger(DiscountIs)})
        parameters.Add(New SqlParameter("@VacationType", SqlDbType.Int) With {.Value = GetSafeInteger(VacationType.SelectedIndex)})
        parameters.Add(New SqlParameter("@LeaveType", SqlDbType.Int) With {.Value = GetSafeInteger(LeaveType.SelectedIndex)})
        parameters.Add(New SqlParameter("@LeaveTypeID", SqlDbType.Int) With {.Value = GetSafeInteger(LeaveTypeID.EditValue)})
        parameters.Add(New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = GetSafeString(Notes.Text.Trim)})
        parameters.Add(New SqlParameter("@AbsenceID", SqlDbType.Int) With {.Value = GetSafeInteger(AbsenceID.SelectedIndex)})
        parameters.Add(New SqlParameter("@IsAbsence", SqlDbType.Bit) With {.Value = GetSafeBoolean(IsAbsence.Checked)})
        parameters.Add(New SqlParameter("@ABDays", SqlDbType.Int) With {.Value = GetSafeInteger(ABDays.EditValue)})
        parameters.Add(New SqlParameter("@DiscountVal", SqlDbType.Decimal) With {.Value = GetSafeDecimal(DiscountVal.EditValue)})
        parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = GetSafeBoolean(IsActiveTG.EditValue)})
        parameters.Add(New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = IsUpdate})
        parameters.Add(New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output})
        parameters.Add(New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output})
        Return parameters
    End Function

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(166, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            '    If dt.Rows(0)("CanSearch") = 0 Then
            '        SafeID.Enabled = False
            '        CanChangeSafe = False
            '    Else
            '        SafeID.Enabled = True
            '        CanChangeSafe = True
            '    End If
        End If
    End Sub

    Public Overrides Sub SetData()
        IsUpdate = 0
        If IsUpdate = 0 Then
            InsertOrUpdate()
            IsUpdate = 0
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub UPDATERECORD()
        IsUpdate = 1
        If IsUpdate = 1 Then
            InsertOrUpdate()
        End If
        If MSG = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub
    Public Overrides Sub Remove()
        IsUpdate = 2
        If IsUpdate = 2 Then
            InsertOrUpdate()
            If IsUpdate <> 0 Then
                WhatsUp2()
            End If
        End If
        If MSG = 1 Then
            MyBase.Remove()
        End If
    End Sub
#Region "Load Data"
    Public Overrides Sub Print()
        Try
            If IsUpdate <> 0 Then
                If isEnd = 0 Then
                    LeaveManagerOpinion.Notes.Select()
                    LeaveManagerOpinion.CodeID.Text = Code.Text.ToString
                    LeaveManagerOpinion.ShowDialog()
                Else
                    LeaveReportInfoCompany.ShowDialog()
                End If
            End If
        Catch ex As Exception
            XtraMessageBox.Show("حدث خطأ أثناء تحميل البيانات: " & ex.Message,
                      "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        MyBase.Print()
    End Sub
    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles EMPID.EditValueChanged
        Code.Text = String.Empty
        If EMPID.EditValue = Nothing Then Return
        If IsUpdate = False Then
            If EMPID.EditValue IsNot Nothing OrElse CInt(EMPID.EditValue) <> -1 Then
                EMPCODE = GETEMPCODE(EMPID.EditValue)
                Code.Text = Convert.ToString(GETMAXID("LeaveTB", "ID") + 1) + " - " + EMPCODE
            End If
        End If
    End Sub

    Private Sub Code_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles Code.ButtonClick
        If e.Button.Index = 0 Then
            FrmViewLeave.ShowDialog()
        End If
    End Sub

    Private Sub DaysNumber_EditValueChanged(sender As Object, e As EventArgs) Handles DaysNumber.EditValueChanged
        If DaysNumber.EditValue > 0 Then
            Dim selectedDate As Date = CDate(DateAdd("d", DaysNumber.EditValue + ABDays.EditValue, D1.EditValue))

            ' إذا كان التاريخ يوم جمعة، أضف يومًا
            If selectedDate.DayOfWeek = DayOfWeek.Friday Then
                selectedDate = selectedDate.AddDays(1)
            End If
            D2.EditValue = DateAdd("d", DaysNumber.EditValue - 1 + ABDays.EditValue, D1.EditValue)
            D3.EditValue = selectedDate
        Else
            D2.EditValue = D1.EditValue
            D3.EditValue = D1.EditValue
        End If
    End Sub



    Private Sub ABDays_EditValueChanged(sender As Object, e As EventArgs) Handles ABDays.EditValueChanged
        If DaysNumber.EditValue > 0 Then
            Dim selectedDate As Date = CDate(DateAdd("d", DaysNumber.EditValue + ABDays.EditValue, D1.EditValue))

            ' إذا كان التاريخ يوم جمعة، أضف يومًا
            If selectedDate.DayOfWeek = DayOfWeek.Friday Then
                selectedDate = selectedDate.AddDays(1)
            End If
            D2.EditValue = DateAdd("d", DaysNumber.EditValue - 1 + ABDays.EditValue, D1.EditValue)
            D3.EditValue = selectedDate
        Else
            D2.EditValue = D1.EditValue
            D3.EditValue = D1.EditValue
        End If
    End Sub

    Private Function arabicCulture() As IFormatProvider
        Throw New NotImplementedException()
    End Function

    Private Sub IsDiscount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles IsDiscount.SelectedIndexChanged
        If IsAbsence.Checked = False Then Exit Sub
        Select Case AbsenceID.SelectedIndex
            Case 0
                DiscountIs = IsDiscount.SelectedIndex
            Case 1
                DiscountIs = 0
            Case 2
                DiscountIs = 0
            Case -1
                DiscountIs = 0
        End Select
    End Sub

    Private Sub D1_EditValueChanged(sender As Object, e As EventArgs) Handles D1.EditValueChanged
        If DaysNumber.EditValue > 0 Then
            Dim selectedDate As Date = CDate(DateAdd("d", DaysNumber.EditValue + ABDays.EditValue, D1.EditValue))

            ' إذا كان التاريخ يوم جمعة، أضف يومًا
            If selectedDate.DayOfWeek = DayOfWeek.Friday Then
                selectedDate = selectedDate.AddDays(1)
            End If
            D2.EditValue = DateAdd("d", DaysNumber.EditValue - 1 + ABDays.EditValue, D1.EditValue)
            D3.EditValue = selectedDate
        Else
            D2.EditValue = D1.EditValue
            D3.EditValue = D1.EditValue
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue = Nothing Then Return
        If BranchID.EditValue IsNot Nothing And CInt(BranchID.EditValue) <> -1 Then
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            LoadToControlar(EMPID, "LeaveTb_LOADEmployeeToLKP", "EMPNAME", "ID", prm)
        End If
    End Sub

    Private Sub AbsenceID_EditValueChanged(sender As Object, e As EventArgs) Handles AbsenceID.EditValueChanged
        If AbsenceID.SelectedIndex < 0 Then Exit Sub
        IsDiscount.Enabled = False
        DiscountVal.Enabled = False
        IsDiscount.SelectedIndex = -1
        DiscountVal.EditValue = 0.000
        Select Case AbsenceID.SelectedIndex
            Case 0
                IsDiscount.Enabled = True
            Case 1
                IsDiscount.Enabled = False
                DiscountVal.Enabled = False
            Case 2
                DiscountVal.Enabled = True
        End Select
    End Sub
#End Region
    Private Sub IsAbsence_CheckedChanged(sender As Object, e As EventArgs) Handles IsAbsence.CheckedChanged
        If IsAbsence.Checked = True Then
            DiscountIs = 0
            IsAbsence.Enabled = False
            AbsenceID.Enabled = True
            BtnEdit.Enabled = True
            BtnDelete.Enabled = False
            ABDays.Enabled = True
            DaysNumber.Enabled = False
            DaysNumber.EditValue = DaysNum
        End If
    End Sub
End Class