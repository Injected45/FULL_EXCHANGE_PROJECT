Imports System.Data.SqlClient
Imports DevExpress.Xpo.Helpers.CannotLoadObjectsHelper
Imports DevExpress.XtraReports.UI

Public Class LeaveReportInfoCompany
    Private Sub LeaveReportInfoCompany_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CompanyName.SelectedIndex = -1
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub

    Public Overrides Sub Print()
        If CompanyName.SelectedIndex = -1 Then
            CompanyName.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If CompanyName.SelectedIndex = 1 Then
            FrmLeave.CompanyNAme = "لشركة" + "  " + "" + CompanyName.Text + "" + "  " + "" + FrmLeave.BranchID.Text + ""
            FrmLeave.BossName = "ادريس رافع الشوبكي"
            FrmLeave.Department = "مدير إدارة الفروع"
        End If
        If CompanyName.SelectedIndex = 0 Then
            FrmLeave.CompanyNAme = "لشركة" + "  " + "" + CompanyName.Text + "" + "  " + "" + FrmLeave.BranchID.Text + ""
            FrmLeave.BossName = "فهد الشوبكي"
            FrmLeave.Department = "مدير شركة الرحالة للصرافة"
        End If
        If CompanyName.SelectedIndex = 2 Then
            FrmLeave.CompanyNAme = "لشركة" + "  " + "" + CompanyName.Text + ""
            FrmLeave.BossName = "عبدالحميد العلواني"
            FrmLeave.Department = "مدير إدارة التطوير"
        End If
        If CompanyName.SelectedIndex = 3 Then
            FrmLeave.CompanyNAme = "لشركة" + "  " + "" + CompanyName.Text + ""
            FrmLeave.BossName = "المهدي الشوبكي"
            FrmLeave.Department = "مدير عام شركة الرحالة القابضة"
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (50)) With {.Value = FrmLeave.Code.Text.Trim}
        Dim results As DataTable = RUN_QUARY_PRO("LeaveTB_Select", PR)
        If results Is Nothing Or results.Rows.Count = 0 Then
            ErrorMessage(Me, "معلومة", "لا توجد بيانات متاحة للسجل المحدد")
            Exit Sub
        End If
        Dim row As DataRow = results.Rows(0)

        If results IsNot Nothing OrElse results.Rows.Count > 0 Then
            Dim report As New RPTLEAVE2
            report.DataSource = results
            report.DataMember = "LeaveTB"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            Dim DateString As String
            Dim arabicCulture As New Globalization.CultureInfo("ar-SA")
            DateString = " " + CDate(FrmLeave.D3.EditValue).ToString("dddd", arabicCulture) + "" + " الموافق " + Format(FrmLeave.D3.EditValue, "yyyy/MM/dd")
            report.XrLabel20.Text = "نُفيدكـــم بأن الموظف" + "/" + "  " + "" + FrmLeave.EMPID.Text + "" + "  " + "التابع" + " " + FrmLeave.CompanyNAme + " " + "قد باشر العمل بعد انتهاء إجازته إعتبارا من يوم" + " " + DateString
            report.XrLabel10.Text = FrmLeave.BossName
            report.XrLabel11.Text = FrmLeave.Department
            report.CreateDocument()
            report.ShowPreview()
            Me.Close()
            MyBase.Print()
        End If
    End Sub
End Class