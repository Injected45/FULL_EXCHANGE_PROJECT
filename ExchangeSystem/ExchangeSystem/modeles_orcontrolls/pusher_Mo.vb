Imports System.Threading.Tasks
Imports PusherClient
Imports Newtonsoft.Json.Linq
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports System.Net.Http
Imports System.Text

Module PusherModule

    Private pusher As Pusher
    Private isPusherStarted As Boolean = False

    Private UrlRout As String = "102.214.165.242:8080"


    Public Async Sub StartPusherAfterLogin()
        If isPusherStarted Then Return
        isPusherStarted = True

        Try
            Dim options As New PusherOptions With {
                .Cluster = "mt1",
                .Encrypted = True
            }

            pusher = New Pusher("0d6948b6c9f89be31a87", options)



            Await pusher.ConnectAsync()

            Dim channel = Await pusher.SubscribeAsync("notifications")
            channel.Bind("notification.sent", Sub(evt As PusherEvent)
                                                  Try
                                                      Dim json As JObject = JObject.Parse(evt.Data)
                                                      Dim message As String = If(json("message") IsNot Nothing, json("message").ToString(), "لا توجد رسالة")
                                                      ShowNotificationToast("📢 إشعار: " & message)
                                                  Catch ex As Exception
                                                      ShowNotificationToast("❌ خطأ في قراءة الإشعار: " & ex.Message)
                                                  End Try
                                              End Sub)

        Catch ex As Exception
            MessageBox.Show("❌ حدث خطأ أثناء بدء Pusher: " & ex.Message,
                            "خطأ",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub ShowNotificationToast(text As String)
        If Application.OpenForms.Count > 0 AndAlso Application.OpenForms(0).InvokeRequired Then
            Application.OpenForms(0).Invoke(Sub() ShowNotificationToastInternal(text))
        Else
            ShowNotificationToastInternal(text)
        End If
    End Sub


    Private Sub ShowNotificationToastInternal(text As String)
        Try
            Dim dt As New DataTable
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = BID}
            dt = RUN_QUARY_PRO("Request_to_summon_driversTB_NAvtion", prm)
            Dim frm As New FRM_Nvaction_roll(text)
            If dt.Rows.Count > 0 Then
                UpdateNavtion()
                frm.Show()
            Else

            End If
        Catch ex As Exception
            MessageBox.Show(text, "إشعار", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub UpdateNavtion()
        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = BID}
            RUN_EXUTE_PRO("Request_to_UpdateNavtion", prm)
        Catch ex As Exception

        End Try
    End Sub

    Async Function StoreNavctionAsync(Type_ID As Integer, Name_NEvction As String) As Task
        Using client As New HttpClient()
            Try

                Dim url As String = "http://" & UrlRout & "/api/device/storeNavction"


                Dim postData = New With {
                    .Type_ID = Type_ID,
                    .Name_NEvction = Name_NEvction,
                    .IS_showe = 0,
                    .BracnID = BID
                }

                Dim json = JsonConvert.SerializeObject(postData)
                Dim content = New StringContent(json, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(url, content)
                Dim result = Await response.Content.ReadAsStringAsync()
                Console.WriteLine("API Response: " & result)
            Catch ex As Exception
                Console.WriteLine("خطأ أثناء الإرسال: " & ex.Message)
            End Try
        End Using
    End Function

    Public Sub Main_Send_API(Name_NEvction As String, Type_ID As Integer)

        StoreNavctionAsync(Type_ID, Name_NEvction).Wait()

        Console.WriteLine("تم إرسال البيانات إلى API")
        Console.ReadLine()
    End Sub


    Public Async Function SendNotificationAsync(massg As String) As Task
        Dim url As String = "http://102.214.165.242:8080/api/device/send-notification-vbnet"

        ' 🔹 تجهيز JSON
        Dim jsonData As String = "{ ""message"": """ & massg & """ }"
        Dim content As New StringContent(jsonData, Encoding.UTF8, "application/json")

        Using client As New HttpClient()
            Try
                Dim response As HttpResponseMessage = Await client.PostAsync(url, content)
                Dim responseText As String = Await response.Content.ReadAsStringAsync()
                Console.WriteLine("📩 Response: " & responseText)
            Catch ex As Exception
                Console.WriteLine("❌ Error: " & ex.Message)
            End Try
        End Using
    End Function


End Module
