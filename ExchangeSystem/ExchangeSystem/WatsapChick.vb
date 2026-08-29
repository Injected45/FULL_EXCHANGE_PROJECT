
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.IO
Imports System.Data.SqlClient



Module WatsapChick

    Private  SESSION_ID As String = Module1.session_id
    Private API_KEY As String = "owa_k1_7c2dd55e99a11e97aef495d122ceba8e150e4942f450269a6a85ba1c020fda18"
    Private BASE_URL As String = "https://wa.rhalla.online/api/sessions/" & SESSION_ID
    Public Function cackid_phone(phone As String, otp As Boolean) As Boolean
        Try
            ' التحقق من القيمة الفارغة
            If String.IsNullOrWhiteSpace(phone) Then Return False

            ' المجموعات صالحة دائماً ✅
            If phone.EndsWith("@g.us") Then Return True

            ' تنظيف الرقم
            Dim cleanNumber As String = phone.Trim().Replace("+", "")
            If cleanNumber.StartsWith("00") Then cleanNumber = cleanNumber.Substring(2)
            If cleanNumber.EndsWith("@c.us") Then cleanNumber = cleanNumber.Replace("@c.us", "")

            ' إذا لم يكن OTP ولم يكن رقماً شخصياً → False
            If Not phone.EndsWith("@c.us") AndAlso otp = False Then Return False

            ' ---------------------------------------------------------
            ' التحقق من صحة الرقم (صيغة فقط بدون API مؤقتاً)
            ' ← الرقم يجب أن يكون أرقاماً فقط وطوله بين 7 و15 رقم
            ' ---------------------------------------------------------
            If Not cleanNumber.All(Function(c) Char.IsDigit(c)) Then Return False
            If cleanNumber.Length < 7 OrElse cleanNumber.Length > 15 Then Return False

            ' الرقم صالح الصيغة ✅
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function whatsapp_contacts(phone_chick) As Boolean

        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@phone_number", SqlDbType.NVarChar, -1) With {.Value = phone_chick}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("whatsapp_contacts", prm)
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
            dt.Dispose()
        Catch ex As Exception
            Return False
        End Try
    End Function

End Module
