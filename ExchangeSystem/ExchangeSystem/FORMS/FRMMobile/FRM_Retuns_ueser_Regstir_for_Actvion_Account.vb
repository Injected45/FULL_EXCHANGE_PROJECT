Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraMap
Imports GMap.NET.MapProviders
Imports System.Data.SqlClient
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Public Class FRM_Retuns_ueser_Regstir_for_Actvion_Account

    Public Sub new_Recorres()
        New_Controlrs(Me)
        DVGFormat(GVRole)
        GVRole.ShowFindPanel()
        LoadToControlar(GridControl1, "users_Activated_user_accounts", "", "", Nothing, 0)
    End Sub

    Private Sub FRM_Retuns_ueser_Regstir_for_Actvion_Account_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        new_Recorres()
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Async Sub RepositoryItemButtonEdit1_Click(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.Click
        Try
            If MessageBox.Show("هل تريد اعادة تفعيل هذه الحساب", "رسالة تنبية", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                Dim prm(0) As SqlParameter
                prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = GVRole.GetFocusedRowCellValue("id")}

                ' تحديث الحساب في قاعدة البيانات
                RUN_EXUTE_PRO("users_Activated_user_accounts_update", prm)

                ' 🔹 استدعاء API لإعادة التفعيل
                Await ReActivateAsync(GVRole.GetFocusedRowCellValue("id"))

                ' إرسال رسالة واتساب
                WATSAPPMsAG(GVRole.GetFocusedRowCellValue("phone"), "تم اعاده تفعيل حساب التطبيق بنجاح", 1)

                ' تحديث الجدول
                LoadToControlar(GridControl1, "users_Activated_user_accounts", "", "", Nothing, 0)

                ' رسالة تأكيد
                MessageBox.Show("تمت عملية اعادة التفعيل بنجاح", "رسالة تاكيد", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Public Shared Async Function ReActivateAsync(userId As Integer) As Task(Of String)
        Dim url As String = "http://102.214.165.242:8080/api/device/reActivate"

        ' تجهيز JSON بالـ user_id
        Dim jsonData As String = "{ ""user_id"": " & userId & " }"
        Dim content As New StringContent(jsonData, Encoding.UTF8, "application/json")

        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.PostAsync(url, content)
            Dim responseText As String = Await response.Content.ReadAsStringAsync()
            Return responseText
        End Using
    End Function


End Class