
Imports System.Data.SqlClient
Imports System.Drawing.Imaging
Imports System.IO
Imports DevExpress.CodeParser
Imports DevExpress.XtraLayout.Customization
Imports Microsoft.VisualBasic.Devices
Public Class FrmCompanyInfo
    Public Property IsUpdate As Boolean
    Public Overrides Sub CHECKBUTTONS()
        lodePreportes()
        MyBase.CHECKBUTTONS()
    End Sub

    Private Sub LoadCompanySettings()
        'Try
        Dim dt As DataTable = RUN_QUARY_PRO_ONLY("SELECTALLTB_PROFILE_COMPANY")
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            ' حفظ القيم في الإعدادات
            My.Settings.ARName = row("ARName").ToString()
            My.Settings.Mobile1 = row("Mobile1").ToString()
            My.Settings.Website = row("WebSite").ToString()
            My.Settings.EMail = row("EMail").ToString()
            My.Settings.FaceBook = row("FaceBook").ToString()
            My.Settings.Combny_name = row("Combny_name_Watsapp").ToString()
            My.Settings.Combny_name_2 = row("Combny_name_2_Watsapp").ToString()
            ' حفظ الصورة في الإعدادات كـ Base64 String
            If Not IsDBNull(row("IMG")) Then
                Dim companyImageBytes As Byte() = CType(row("IMG"), Byte())
                If companyImageBytes IsNot Nothing AndAlso companyImageBytes.Length > 0 Then
                    My.Settings.Company_Image = Convert.ToBase64String(companyImageBytes)
                Else
                    My.Settings.Company_Image = Nothing
                End If
            Else
                My.Settings.Company_Image = Nothing
            End If
            My.Settings.Save()
            ' عرض البيانات على الواجهة
            ARName.Text = My.Settings.ARName
            HeadOffice.Text = row("CityName").ToString()
            Mobile1.Text = My.Settings.Mobile1
            'inset_date.Text = row("inset_date").ToString()
            CoAddress.Text = $"{row("CoAddress")} "
            Website.Text = My.Settings.Website
            FaceBook.Text = My.Settings.FaceBook
            Combny_name.Text = My.Settings.Combny_name
            Combny_name_2.Text = My.Settings.Combny_name_2

            ' عرض الصورة
            If Not String.IsNullOrEmpty(My.Settings.Company_Image) Then
                Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
                LOGO.EditValue = imageBytes
            End If

        Else
            ' تحميل البيانات من الإعدادات في حالة عدم وجود بيانات في قاعدة البيانات
            ARName.Text = My.Settings.ARName
            HeadOffice.Text = My.Settings.HeadOffice

            If Not String.IsNullOrEmpty(My.Settings.Company_Image) Then
                'Dim imageBytes As Byte() = Convert.FromBase64String(My.Settings.Company_Image)
                LOGO.EditValue = My.Resources.cancel
            End If
        End If
        'Catch ex As Exception
        '    ErrorMessage(Me, "error Massg ", ex.Message)
        'End Try
    End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(1, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Private Sub FrmCompanyInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCompanySettings()
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        'BtnBarcodePrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        BtnSave.Caption = "حفظ الاعدادات"
        'BtnBarcodePrint.Caption = "اغلاق"
    End Sub
    Public Overrides Sub Save()
        Try
            'If LOGO.EditValue = My.Resources.cancel Then
            '    ErrorMessage(Me, "حدث خطأ أثناء الحفظ", "الرجاء اختيار الصورة")
            '    Exit Sub
            'End If
            SplashScreenManager1.ShowWaitForm()

            ' تحويل الصورة إلى بايت
            Dim imageBytes() As Byte = Nothing
            If LOGO.Image IsNot Nothing Then
                Using ms As New MemoryStream()
                    LOGO.Image.Save(ms, ImageFormat.Jpeg) ' يمكنك تغيير ImageFormat حسب حاجتك
                    imageBytes = ms.ToArray()
                End Using
            End If

            ' إنشاء الباراميترات
            Dim prm(9) As SqlParameter
            prm(0) = New SqlParameter("@ARName", SqlDbType.NVarChar, 300) With {.Value = ARName.Text}
            prm(1) = New SqlParameter("@Mobile1", SqlDbType.NVarChar, 50) With {.Value = Mobile1.Text}
            prm(2) = New SqlParameter("@IMG", SqlDbType.VarBinary) With {.Value = If(imageBytes, CType(DBNull.Value, Object))}
            prm(3) = New SqlParameter("@CityName", SqlDbType.NVarChar, 100) With {.Value = HeadOffice.Text}
            prm(4) = New SqlParameter("@CoAddress", SqlDbType.NVarChar, 250) With {.Value = CoAddress.Text}
            prm(5) = New SqlParameter("@WebSite", SqlDbType.NVarChar, 50) With {.Value = Website.Text}
            prm(6) = New SqlParameter("@EMail", SqlDbType.NVarChar, 50) With {.Value = EMail.Text}
            prm(7) = New SqlParameter("@FaceBook", SqlDbType.NVarChar, 300) With {.Value = FaceBook.Text}
            prm(8) = New SqlParameter("@Combny_name_Watsapp", SqlDbType.NVarChar, 450) With {.Value = Combny_name.Text}
            prm(9) = New SqlParameter("@Combny_name_2_Watsapp", SqlDbType.NVarChar, 450) With {.Value = Combny_name_2.Text}

            ' تنفيذ الإجراء المخزن
            RUN_EXUTE_PRO("CMD_INSERT_TB_PROFILE_COMPANY", prm)

            ' حفظ البيانات في الإعدادات
            My.Settings.ARName = ARName.Text
            My.Settings.Mobile1 = Mobile1.Text
            My.Settings.Website = Website.Text
            My.Settings.EMail = EMail.Text
            My.Settings.FaceBook = FaceBook.Text
            If imageBytes IsNot Nothing Then
                My.Settings.Company_Image = Convert.ToBase64String(imageBytes)
            Else
                My.Settings.Company_Image = Nothing
            End If
            My.Settings.Save()

            ' إعادة تحميل البيانات
            LoadCompanySettings()

            ' عرض رسالة النجاح
            FrmSavedSuccessfully.ShowDialog()
        Catch ex As Exception
            ErrorMessage(Me, "حدث خطأ أثناء الحفظ", ex.Message)
        Finally
            SplashScreenManager1.CloseWaitForm()
            MyBase.Save()
        End Try
    End Sub

End Class