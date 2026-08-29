Imports System.Data.SqlClient
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FrmResignation
    Public IsUpdate As Boolean, EMPCODE, CompanyName, BossName, Department As String, DiscountIs, MSG, DaysNum As Integer
    Public Sub NewRecord()
        New_Controlrs(Me)
        LoadToControlar(BranchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        EnabledControls(True)
        IsUpdate = 0
        BranchID.EditValue = BID
        InsertDate.EditValue = Date.Now
        BranchID.Select()
        lodePreportes()
    End Sub
    Public Sub EnabledControls(IsEnabled As Boolean)
        Enable_Controls(Me, IsEnabled)
        IsActiveTG.Enabled = IsEnabled
        BtnEdit.Enabled = False
        BtnDelete.Enabled = Not IsEnabled
        BtnPrint.Enabled = Not IsEnabled
        BtnSave.Enabled = IsEnabled
        InsertDate.Enabled = False
    End Sub
    Private Sub FrmResignation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IsUpdate = 0 Then
            NewRecord()
        End If
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
                NewRecord()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "حدث خطأ في العملية، يرجى المحاولة في وقت لاحق", ex.Message)
        End Try
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(175, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If
    End Sub

    Private Function ValidateRequiredFields() As Boolean
        If EMPID.EditValue Is Nothing OrElse CInt(EMPID.EditValue) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "يجب اختيار الموظف")
            Return False
        End If
        If BranchID.EditValue Is Nothing OrElse CInt(BranchID.EditValue) = -1 Then
            ErrorMessage(Me, "خطأ في البيانات", "هذا الحقل مطلوب")
            Return False
        End If
        Return True
    End Function

    Private Function PerformLeaveOperation() As Boolean
        Dim parameters As List(Of SqlParameter) = CreateParameters()
        RUN_EXUTE_PRO("ResignationTb_Insert", parameters.ToArray())
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
        parameters.Add(New SqlParameter("@InsertDate ", SqlDbType.Date) With {.Value = GetSafeDate(InsertDate.EditValue)})
        parameters.Add(New SqlParameter("@ResignatoinDate", SqlDbType.Date) With {.Value = GetSafeDate(ResignatoinDate.EditValue)})
        parameters.Add(New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = GetSafeString(Notes.Text.Trim)})
        parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = GetSafeBoolean(IsActiveTG.EditValue)})
        parameters.Add(New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate})
        parameters.Add(New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output})
        parameters.Add(New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output})
        Return parameters
    End Function
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

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue = Nothing Then Return
        If BranchID.EditValue IsNot Nothing And CInt(BranchID.EditValue) <> -1 Then
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(1) = New SqlParameter("@ISUPDATE", SqlDbType.Bit) With {.Value = IsUpdate}
            LoadToControlar(EMPID, "ResignationTb_LOADEmployeeToLKP", "EMPNAME", "ID", prm)
        End If
    End Sub

    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            InsertOrUpdate()
        End If
        If MSG = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub
    Public Overrides Sub Remove()
        If IsUpdate = True Then
            InsertOrUpdate()
        End If
        If MSG = 1 Then
            MyBase.Remove()
        End If
    End Sub
    Public Overrides Sub Print()
        Try
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = Code.Text.Trim}
            Dim results As DataTable = RUN_QUARY_PRO("ResignationTb_Select", PR)
            If results Is Nothing Or results.Rows.Count = 0 Then
                ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
                Exit Sub
            End If
            Dim row As DataRow = results.Rows(0)

            If results IsNot Nothing OrElse results.Rows.Count > 0 Then
                Dim report As New RPTResignation
                report.DataSource = results
                report.DataMember = "ResignationTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                'report.XrLabel20.Text = "الموقع أدناه,الموظف" + ":" + "  " + EMPID.Text + "  " + "الرقم الوظيفي" + ":" + " " + SafeGetValue(row, "EmpCode").ToString + vbNewLine + "المسمى الوظيفي" + ":" + " " + SafeGetValue(row, "ECNAME").ToString + "  " + "تاريخ الإلتحاق" + ":" + " " + SafeGetValue(row, "EMPDATE").ToString + vbNewLine + "أتقدم إليكم بطلب استقالتي من العمل في شركة الرحالة القابضة" + "." + vbNewLine + "وذلك اعتبارا من تاريخ" + ":" + " " + SafeGetValue(row, "InsertDate") + " " + "وفقا لفترة الإشعار المعتمدة في عقد العمل والتي تبلغ 30 يوما" + "."
                'report.XrLabel13.Text = EMPID.Text
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            XtraMessageBox.Show("حدث خطأ أثناء تحميل البيانات: " & ex.Message,
                      "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        MyBase.Print()
    End Sub
    Sub SHOW_EMCUSCODE(x)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = x}
        Dim results As DataTable = RUN_QUARY_PRO("ResignationTb_Select", PR)
        If results Is Nothing OrElse results.Rows.Count = 0 Then
            ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
            Exit Sub
        End If
        Dim row As DataRow = results.Rows(0)
        Try
            IsUpdate = True
            EnabledControls(False)
            BranchID.EditValue = SafeGetValue(row, "BranchID")
            InsertDate.EditValue = SafeGetValue(row, "InsertDate")
            IsActiveTG.EditValue = SafeGetValue(row, "IsActive")
            BranchID_EditValueChanged(Nothing, Nothing)
            EMPID.EditValue = SafeGetValue(row, "EMPID")
            Notes.Text = SafeGetValue(row, "Notes", String.Empty)
            Code.Text = SafeGetValue(row, "Code").ToString
            ResignatoinDate.EditValue = SafeGetValue(row, "ResignatoinDate")
        Catch ex As Exception
            XtraMessageBox.Show("حدث خطأ أثناء تحميل البيانات: " & ex.Message,
                          "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles EMPID.EditValueChanged
        Code.Text = String.Empty
        If EMPID.EditValue = Nothing Then Return
        If IsUpdate = False Then
            If EMPID.EditValue IsNot Nothing OrElse CInt(EMPID.EditValue) <> -1 Then
                EMPCODE = GETEMPCODE(EMPID.EditValue)
                Code.Text = Convert.ToString(GETMAXID("ResignationTb", "ID") + 1) + " - " + EMPCODE
            End If
        End If
    End Sub

    Private Sub Code_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles Code.ButtonClick
        If e.Button.Index = 0 Then
            FrmViewResignation.ShowDialog()
        End If
    End Sub
End Class