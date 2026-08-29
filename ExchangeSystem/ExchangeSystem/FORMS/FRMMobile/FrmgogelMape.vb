Imports GMap.NET
Imports GMap.NET.WindowsForms
Imports GMap.NET.WindowsForms.Markers
Imports System.Net.Http
Imports System.Drawing
Imports Newtonsoft.Json.Linq
Imports System.Globalization

Public Class FrmgogelMape



    ' ماركر شفاف
    Public Class TransparentMarker
        Inherits GMapMarker

        Private ReadOnly labelText As String
        Private ReadOnly font As Font = New Font("Tahoma", 12, FontStyle.Bold)
        Private ReadOnly textBrush As Brush = Brushes.Black

        Public Sub New(pos As PointLatLng, text As String)
            MyBase.New(pos)
            labelText = text
        End Sub

        Public Overrides Sub OnRender(g As Graphics)
            g.DrawString(labelText, font, textBrush, LocalPosition)
        End Sub
    End Class

    ' ماركر مخصص بنص
    Public Class TextMarker
        Inherits GMapMarker

        Private ReadOnly labelText As String
        Private ReadOnly font As Font
        Private ReadOnly brush As Brush

        Public Sub New(pos As PointLatLng, text As String, Optional customFont As Font = Nothing, Optional customBrush As Brush = Nothing)
            MyBase.New(pos)
            Me.labelText = text
            Me.font = If(customFont, New Font("Droid Arabic Kufi", 14, FontStyle.Bold))
            Me.brush = If(customBrush, Brushes.Black)
        End Sub

        Public Overrides Sub OnRender(g As Graphics)
            g.DrawString(labelText, font, brush, LocalPosition)
        End Sub
    End Class

    ' دالة لحساب المسافة بين نقطتين
    Private Function CalculateDistance(lat1 As Double, lon1 As Double, lat2 As Double, lon2 As Double) As Double
        Dim R As Double = 6371
        Dim dLat As Double = (lat2 - lat1) * Math.PI / 180
        Dim dLon As Double = (lon2 - lon1) * Math.PI / 180

        Dim a As Double = Math.Sin(dLat / 2) ^ 2 +
                          Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                          Math.Sin(dLon / 2) ^ 2

        Dim c As Double = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a))
        Return R * c
    End Function

    ' رسم المسار بين نقطتين
    Public Async Function DrawRouteBetweenPoints(lat1 As Double, lon1 As Double, lat2 As Double, lon2 As Double, clientName As String, branchName As String, phone As String) As Task
        ' التحقق من صحة الإحداثيات
        If lat1 = 0 OrElse lon1 = 0 OrElse lat2 = 0 OrElse lon2 = 0 Then
            MessageBox.Show("الإحداثيات غير صالحة. الرجاء التأكد من البيانات.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Function
        End If

        Dim fromPoint As New PointLatLng(lat1, lon1)
        Dim toPoint As New PointLatLng(lat2, lon2)
        Dim culture As CultureInfo = CultureInfo.InvariantCulture

        ' رابط API باستخدام المعلمات الصحيحة والمسموحة
        Dim apiUrl As String = $"https://graphhopper.com/api/1/route?point={lat1.ToString(culture)},{lon1.ToString(culture)}&point={lat2.ToString(culture)},{lon2.ToString(culture)}&vehicle=car&locale=ar&points_encoded=false&key=243bebe0-ecb2-413a-9d3c-a00e20e43487"

        Using client As New HttpClient()
            client.Timeout = TimeSpan.FromSeconds(15)

            Try
                Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
                Dim responseContent As String = Await response.Content.ReadAsStringAsync()

                If Not response.IsSuccessStatusCode Then
                    Dim errorMsg As String = "فشل الطلب."

                    Try
                        Dim errJson As JObject = JObject.Parse(responseContent)
                        errorMsg = errJson("message")?.ToString()
                    Catch
                        errorMsg &= vbCrLf & "الرد غير مفهوم: " & responseContent
                    End Try

                    MessageBox.Show($"الكود: {response.StatusCode}{vbCrLf}الرسالة: {errorMsg}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If

                Dim json As JObject = JObject.Parse(responseContent)
                Dim path = json("paths")?.FirstOrDefault()

                If path Is Nothing Then
                    MessageBox.Show("لم يتم العثور على مسار صالح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Function
                End If

                Dim coordinates = path("points")?("coordinates")
                If coordinates Is Nothing OrElse Not coordinates.Any() Then
                    MessageBox.Show("لا توجد إحداثيات مرجعة من الخادم.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Function
                End If

                ' إعداد الطبقة والمسار
                Dim overlay As New GMapOverlay("routeOverlay")
                Dim routePoints As New List(Of PointLatLng)

                For Each point As JArray In coordinates
                    routePoints.Add(New PointLatLng(point(1).ToObject(Of Double)(), point(0).ToObject(Of Double)()))
                Next

                Dim route As New GMapRoute(routePoints, "Route") With {.Stroke = New Pen(Color.Blue, 2)}
                overlay.Routes.Add(route)

                ' إعداد العلامات
                Dim distance As Double = path("distance").ToObject(Of Double)() / 1000

                Dim fromMarker As New GMarkerGoogle(fromPoint, GMarkerGoogleType.green) With {
                .ToolTipText = $"بداية الفرع - {branchName}",
                .ToolTipMode = MarkerTooltipMode.Always
            }

                Dim toMarker As New GMarkerGoogle(toPoint, GMarkerGoogleType.red) With {
                .ToolTipText = $"العميل: {clientName}" & vbCrLf & $"رقم الهاتف: {phone}",
                .ToolTipMode = MarkerTooltipMode.Always
            }

                overlay.Markers.Add(fromMarker)
                overlay.Markers.Add(toMarker)

                ' تحديث خريطة العميل
                customerMapLink = $"https://www.google.com/maps/search/?api=1&query={lat2.ToString(culture)},{lon2.ToString(culture)}"

                ' إعداد واجهة المستخدم
                Panel1.Controls.Clear()

                Dim fromBtn As New Button With {.Text = $"الفرع: {branchName}", .Width = 160, .Height = 50, .BackColor = Color.LightGreen}
                AddHandler fromBtn.Click, Sub()
                                              GMapControl1.Position = fromPoint
                                              GMapControl1.Zoom = 16
                                          End Sub

                Dim toBtn As New Button With {
                .Text = $"العميل: {clientName}" & vbCrLf & $"المسافة: {distance:F2} كم" & vbCrLf & $"رقم الهاتف: {phone}",
                .Width = 160,
                .Height = 90,
                .BackColor = Color.LightCoral
            }
                AddHandler toBtn.Click, Sub()
                                            GMapControl1.Position = toPoint
                                            GMapControl1.Zoom = 16
                                        End Sub

                Dim mapLinkBtn As New Button With {.Text = "عرض موقع العميل على الخريطة", .Width = 160, .Height = 40, .BackColor = Color.LightSkyBlue}
                AddHandler mapLinkBtn.Click, Sub()
                                                 If Not String.IsNullOrEmpty(customerMapLink) Then
                                                     Process.Start(New ProcessStartInfo(customerMapLink) With {.UseShellExecute = True})
                                                 End If
                                             End Sub

                Dim copyLinkBtn As New Button With {.Text = "نسخ رابط الموقع", .Width = 160, .Height = 40, .BackColor = Color.LightYellow}
                AddHandler copyLinkBtn.Click, Sub()
                                                  Clipboard.SetText(customerMapLink)
                                                  MessageBox.Show("تم نسخ رابط موقع العميل إلى الحافظة.", "تم النسخ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                              End Sub

                ' إضافة الأزرار
                Panel1.Controls.AddRange({fromBtn, toBtn, mapLinkBtn, copyLinkBtn})
                fromBtn.Top = 0
                toBtn.Top = fromBtn.Bottom + 10
                mapLinkBtn.Top = toBtn.Bottom + 10
                copyLinkBtn.Top = mapLinkBtn.Bottom + 10
                Panel1.Size = New Size(Panel1.Width, copyLinkBtn.Bottom + 10)

                ' تحديث الخريطة
                GMapControl1.Overlays.Clear()
                GMapControl1.Overlays.Add(overlay)
                GMapControl1.Position = New PointLatLng((lat1 + lat2) / 2, (lon1 + lon2) / 2)

                Select Case distance
                    Case < 1 : GMapControl1.Zoom = 16
                    Case < 5 : GMapControl1.Zoom = 14
                    Case < 20 : GMapControl1.Zoom = 12
                    Case < 50 : GMapControl1.Zoom = 10
                    Case < 100 : GMapControl1.Zoom = 8
                    Case Else : GMapControl1.Zoom = 6
                End Select

            Catch ex As Exception
                MessageBox.Show("حدث خطأ أثناء تحميل المسار: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Function

End Class