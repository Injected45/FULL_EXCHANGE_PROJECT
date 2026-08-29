Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports DevExpress.CodeParser
Imports Nancy.Json

Module SandWatsappMasggs

    ''SQl_@RPhone and Sql_@Mobile1 جلب رقم الفرع المستلم او رقم هاتف المستلم من قاعدة البيانات وتخزين قيمهن في متغيرين اسمهن 
    Public sql_RPhone As String, sql_Mobile1 As String, sql_SPhone1 As String
#Region "حوالة الداخلية "


    Public Sub RPhone_get_forWatsab_and_CoBranch_Mobile(code As String, BRanchIDFrom As Integer)
        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = code}
        prm(1) = New SqlParameter("@id", SqlDbType.Int) With {.Value = BRanchIDFrom}
        prm(2) = New SqlParameter("@RPhone", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(3) = New SqlParameter("@Mobile1", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(4) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("RPhone_get_forWatsab_and_CoBranch_Mobile", prm)
        sql_RPhone = prm(2).Value
        sql_Mobile1 = prm(3).Value
        sql_SPhone1 = prm(4).Value
    End Sub

    ''' حوالة داخلية كود ارسال حوالة الي الراسل 
    Public Sub BuildMessageForStandardTransferw(SenderBrnchPhoner As String, CodeID As String,
                                                     OverallVal As Decimal, RecievedName As String,
                                                     BranchDeliveredID As String, BRancID As ULong)

        RPhone_get_forWatsab_and_CoBranch_Mobile(CodeID, BRancID)
        Dim message As String = "حوالة محلية صادرة " & vbNewLine &
                            "CODE : " & CodeID & vbNewLine &
                            "بقيمة : " & Cur_Code("دينار ليبي", OverallVal, True, "n2") & vbNewLine &
                            Cur_Code("دينار ليبي", OverallVal, False, "N2") & vbNewLine &
                            "الى : " & RecievedName & vbNewLine &
                            "هـ : " & sql_SPhone1 & vbNewLine &
                            "مكان التسليم : " & BranchDeliveredID & vbNewLine &
                            "للإستفسار هـ : " & sql_Mobile1 & vbNewLine
        message &= "سنعلمكم بتسليم الحوالة" & vbNewLine & "شكراً لتعاملكم معنا"
        WATSAPPMsAG(sql_SPhone1, message, whatsapp_contacts(sql_SPhone1))
    End Sub



    '   الداخلية بناء الرسالة للحوالة الواردة
    Public Sub BuildIncomingTransferMessage(CodeID As String, SenderName As String,
                                                  OverallVal As String, BranchDeliveredID As String,
                                            BRancID As ULong, ACCto As Integer)

        RPhone_get_forWatsab_and_CoBranch_Mobile(CodeID, BRancID)
        Dim message As String = My.Settings.Combny_name & vbNewLine
        Select Case ACCto
            Case 0
                message &= "لديك حوالة محلية واردة" & vbNewLine
            Case 1
                message &= "دخول حوالة محلية " & vbNewLine
        End Select




        message &= "CODE : " & CodeID & vbNewLine &
           "من : " & SenderName & vbNewLine &
           "القيمة : " & Cur_Code("دينار ليبي", OverallVal, True, "n2") & vbNewLine &
            Cur_Code("دينار ليبي", OverallVal, False, "n2") & vbNewLine &
           "تفضل بالإستلام بـ : " & BranchDeliveredID & vbNewLine &
           "مصحوباً بإثبات شخصي" & vbNewLine &
           "للإستفسار هـ : " & sql_Mobile1 & vbNewLine &
           "شكراً لتعاملكم معنا"

        WATSAPPMsAG(sql_RPhone, message, whatsapp_contacts(sql_RPhone))
    End Sub

    '   الداخلية بناء الرسالة للحوالة المسلمة
    Public Sub BuildDeliveredTransferMessage(CodeID As String, RecievedName As String, BranchDeliveredID As Integer)

        RPhone_get_forWatsab_and_CoBranch_Mobile(CodeID, BranchDeliveredID)
        Dim messg = My.Settings.Combny_name & vbNewLine &
           "تم تسليم الحوالة" & vbNewLine &
           "CODE : " & CodeID & vbNewLine &
           "إلى السيد/ة : " & RecievedName & vbNewLine &
           "شكراً لتعاملكم معنا"

        WATSAPPMsAG(sql_SPhone1, messg, whatsapp_contacts(sql_SPhone1))
    End Sub


    Public Function GetFractionOrInteger(EXVale As Double) As String
        ' التحقق من وجود جزء عشري
        If EXVale Mod 1 <> 0 Then
            ' إذا كان الجزء العشري أكبر من صفر
            If EXVale - Math.Floor(EXVale) > 0 Then
                ' استخراج الجزء الكسرى
                ' إزالة "0."
                Return Convert.ToString(Math.Round(EXVale, 1)) + "د.ل " ' إرجاع الجزء الكسرى فقط
            End If
        End If
        ' إذا لم يكن هناك كسر أو كان الكسر صفر، إرجاع العدد الصحيح
        Return Cur_Code("دينار ليبي", EXVale, True, "N2")
    End Function
    ''' ارسال رسالة تبيلغ حوالة داخلية الي وكيل 

    Public Sub sEnFRoRElode(ISID As String, bdidfrom As Integer, type As Integer, IScteck As Integer, ExValShare As Decimal, ExValShare1 As Decimal)
        Try

            RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, MAINBID)
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = ISID}
            dt = RUN_QUARY_PRO("GET_colmens_InternalEx", prm)

            If dt.Rows.Count > 0 Then
                RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, MAINBID)

                Dim dd As String = My.Settings.Combny_name & vbNewLine &
                    "CODE :" & Space(1) & dt.Rows(0)("Code") & vbNewLine &
                            "مكان التسليم :" & Space(1) & dt.Rows(0)("CityName") & vbNewLine &
                               "مـ :" & Space(1) & dt.Rows(0)("RecievedName") & vbNewLine &
                                "هـ :" & Space(1) & dt.Rows(0)("RPhone1") & vbNewLine &
                                "القيمه :" & Space(1) & Cur_Code("دينار ليبي", dt.Rows(0)("OverallVal"), True, "N2") & vbNewLine &
             Cur_Code("دينار ليبي", dt.Rows(0)("OverallVal"), False, True) & vbNewLine

                Select Case type And IScteck
                    Case 0 And 1
                        dd &= "العمولة :" & Space(1) & GetFractionOrInteger(ExValShare) & vbNewLine
                    Case 1 And 1
                        dd &= "العمولة :" & Space(1) & GetFractionOrInteger(ExValShare1) & vbNewLine
                    Case Else
                        dd &= "العمولة :" & Space(1) & Cur_Code("دينار ليبي", 0.00, True, "N2") & vbNewLine
                End Select
                dd &= "للإستفسار هـ : " & Space(1) & sql_Mobile1 & vbNewLine &
                              "شكراً لتعاملكم معنا"

                WATSAPPMsAG(get_gruop_id(bdidfrom), dd, True)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try


    End Sub

    Function SafeVall(row As DataRow, col As String) As String
        If IsDBNull(row(col)) Then Return ""
        Return row(col).ToString()
    End Function
    Public Sub sEnFRoCancle(ISID As String, bdidfrom As Integer)
        'Try
        Dim dt As New DataTable
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = ISID}

            dt = RUN_QUARY_PRO("GET_colmens_InternalEx", prm)

            If dt.Rows.Count > 0 Then

                Dim r As DataRow = dt.Rows(0)

            Dim dd As String =
            "❌ تم إلغاء الحوالة" & vbNewLine &
            "🔹 مرسلة من: " & SafeVall(r, "BRName").ToString & vbNewLine &
            "CODE: " & SafeVall(r, "Code").ToString & vbNewLine &
            "👤 اسم المستلم: " & SafeVall(r, "RecievedName").ToString & vbNewLine &
            "📞 هاتف المستلم: " & SafeVall(r, "RPhone1").ToString & vbNewLine &
            "📍 مدينة: " & SafeVall(r, "CityName").ToString & vbNewLine &
            "💰 القيمة: " & Cur_Code("دينار ليبي", SafeToDecimal(dt.Rows(0)("OverallVal")), True, "N2") & vbNewLine &
            "✍️ " & Cur_Code("دينار ليبي", SafeToDecimal(dt.Rows(0)("OverallVal")), False, True) & vbNewLine &
            "🧾 العمولة: " & Cur_Code("دينار ليبي", SafeToDecimal(dt.Rows(0)("BrShare")), True, "N2") & vbNewLine &
            "📅 التاريخ: " & dt.Rows(0)("ConfirmCanceledDate").ToString() & vbNewLine &
            "⏰ الوقت: " & dt.Rows(0)("ConfirmCanceledTime").ToString() & vbNewLine &
            "⚠️ هذه الحوالة ملغاة ولن يتم تنفيذها" & vbNewLine &
            "🙏 شكراً لتعاملكم معنا"


            WATSAPPMsAG(get_gruop_id(bdidfrom), dd, True)


            End If

        'Catch ex As Exception
        '    MsgBox("خطأ: " & ex.Message)
        'End Try


    End Sub


#End Region

#Region "حوالة الخارجية "

    '' جلب بيانات الحوالة الخاجية 
    Public Function ExternalEx_SendForWatsapp(Code As String) As DataTable
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        dt = RUN_QUARY_PRO_alter("ExternalEx_SendForWatsapp", prm)
        Return dt
    End Function


    ''اجراء يقوم بارجاء نص الرسالة 
    Public Sub BuildTransferMessage(confirmType As String, code As String)
        Dim dt As New DataTable
        dt.Clear()
        dt = ExternalEx_SendForWatsapp(code)
        If dt.Rows.Count > 0 Then
            Dim messagea As String = My.Settings.Combny_name & vbNewLine & dt.Rows(0)("IsInOrOut") & vbNewLine
            messagea &= "CODE : " & dt.Rows(0)("code") & vbNewLine
            messagea &= dt.Rows(0)("ServiceName") & Space(1) & dt.Rows(0)("CityName2") & vbNewLine & "مـ : " & dt.Rows(0)("RecievedName") & vbNewLine
            messagea &= "هـ : " & dt.Rows(0)("RPhone1") & vbNewLine &
             "الرقم القومي : " & vbNewLine & dt.Rows(0)("OwnNatioNum") & vbNewLine
            Select Case confirmType
                Case 0
                    messagea &= "بقيمة : " & Cur_Code("دينار ليبي", dt.Rows(0)("CurrRecievedVal"), True, "n2") & vbNewLine &
                Cur_Code(dt.Rows(0)("RecievedCurrencyID"), dt.Rows(0)("CurrRecievedVal"), False, "n2") & vbNewLine &
                    "سعر الصرف : " & dt.Rows(0)("TransPrice")
                Case 1
                    messagea &= "القيمة : " & Cur_Code("دينار ليبي", dt.Rows(0)("NewCurrDeliveredVal"), True, "n2") & vbNewLine &
                Cur_Code("دينار ليبي", dt.Rows(0)("NewCurrDeliveredVal"), False, "n2") & vbNewLine &
                "سعر الصرف : " & dt.Rows(0)("NewTrancPrice")
            End Select
            messagea &= vbNewLine & "مصاريف الخدمه : " & dt.Rows(0)("ServiceExVal") & vbNewLine &
            "الصافي : " & Cur_Code(dt.Rows(0)("DeliveredCurrencyID"), dt.Rows(0)("CurrDeliveredVal"), True, "n2") & vbNewLine &
               "للإستفسار هـ : " & dt.Rows(0)("Mobile1") & vbNewLine &
               "شكراً لتعاملكم معنا"
            If confirmType = 0 Then

                WATSAPPMsAG(dt.Rows(0)("Phone1"), messagea, whatsapp_contacts(dt.Rows(0)("Phone1")))
            Else
                WATSAPPMsAG(dt.Rows(0)("IDGroup"), messagea, True)
                Dim messagea2 As String = BuildTransferMessageRecieved(dt.Rows(0)("code"),
                                                                       dt.Rows(0)("BranchDeliveredIDPhone"),
                                                        dt.Rows(0)("RecievedName"), dt.Rows(0)("SenderName"),
                                                       dt.Rows(0)("Phone1"))
                WATSAPPMsAG(dt.Rows(0)("RPhone1"), messagea2, whatsapp_contacts(dt.Rows(0)("RPhone1")))
            End If
        End If

    End Sub





    ''  ''   جلب رسالة المستلم في الحوالة الخارجية 
    Private Function BuildTransferMessageRecieved(CodeID As String, BranchDeliveredIDPhone As String,
                                                  RecievedName As String,
                                                  SenderName As String, SPhone1 As String) As String
        Return "لديك أمانه من" & Space(1) & My.Settings.Combny_name_2 & vbNewLine &
            "CODE : " & CodeID & vbNewLine &
            "باسم :  " & RecievedName & vbNewLine &
           "من : " & SenderName & vbNewLine &
           "هـ : " & SPhone1 &
         "للإستفسار هـ : " & BranchDeliveredIDPhone & vbNewLine &
                   "شكراً لتعاملكم معنا"
    End Function

#End Region



    Public Sub FRMEMPWITHDRAWAL_SandForWtsapp(TypeFrom As Integer, WDCode As String, CurrencyFrom As String,
            WDValue As Decimal, WithdrawalFrom As Object, PaidFor As String, Phone As String)
        If TypeFrom = 5 Then
            Dim mms As String = My.Settings.Combny_name & vbNewLine & "CODE " & ":" & Space(1) & WDCode &
               vbNewLine & "تم سحب مبلغ" & Space(1) & ":" & Space(1) &
               Cur_Code(CurrencyFrom, WDValue, True, "n2") & vbNewLine &
               Cur_Code(CurrencyFrom, WDValue, False, "n2") & vbNewLine
            mms &= "من حساب" & Space(1) & ":" & Space(1) & WithdrawalFrom.text & vbNewLine & "لصالح" & Space(1) &
                "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "رصيدكم الحالي هو" &
                vbNewLine & GetLKPColumnVal(WithdrawalFrom, "GetAccVal") - WDValue & Space(1) &
                Cur_Code1(CurrencyFrom) & vbNewLine & "شكرا لتعاملكم معنا"
            WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(WithdrawalFrom.EditValue), mms, True)
        ElseIf TypeFrom = 7 Then


            Dim mms As String = My.Settings.Combny_name & vbNewLine & "CODE " & ":" & Space(1) & WDCode &
                   vbNewLine & "دخول مبلغ " & Cur_Code(CurrencyFrom, WDValue, True, "n2") &
                       vbNewLine &
                    Cur_Code(CurrencyFrom, WDValue, False, "n2") & vbNewLine
            mms &= "لحساب" & Space(1) & ":" & Space(1) & WithdrawalFrom.Text
            WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(WithdrawalFrom.EditValue), mms, True)

        End If


    End Sub


    Public Sub lodeDatatGroupForWataspp(DataGridView1 As Object)

        ' ---------------------------------------------------------
        ' 1. بيانات الربط الخاصة بسيرفر wa.rhalla.online
        ' ---------------------------------------------------------
        Dim url As String = $"https://wa.rhalla.online/api/sessions/{session_id}/groups"

        Try
            ' ---------------------------------------------------------
            ' 2. إنشاء الطلب مع Authorization Header
            ' ---------------------------------------------------------
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            request.ContentType = "application/json"
            request.Timeout = 15000
            request.Headers.Add("Authorization", "Bearer " & apiKey) ' ← الفرق الأساسي عن UltraMsg

            ' ---------------------------------------------------------
            ' 3. استلام الرد
            ' ---------------------------------------------------------
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim reader As New StreamReader(response.GetResponseStream())
            Dim json As String = reader.ReadToEnd()
            reader.Close()
            response.Close()

            ' ← سطر مؤقت للتشخيص: شاهد شكل JSON القادم من السيرفر
            ' MsgBox(json)

            ' ---------------------------------------------------------
            ' 4. تحويل JSON إلى كائنات
            ' ---------------------------------------------------------
            Dim serializer As New JavaScriptSerializer()
            Dim groups As List(Of WhatsAppGroup) =
                serializer.Deserialize(Of List(Of WhatsAppGroup))(json)

            ' ---------------------------------------------------------
            ' 5. إنشاء DataTable وتعبئة البيانات
            ' ---------------------------------------------------------
            Dim dt As New DataTable()
            dt.Columns.Add("Group_ID")
            dt.Columns.Add("Group_Name")
            dt.Columns.Add("Participants_Count")
            dt.Columns.Add("Pinned")
            dt.Columns.Add("Unread")

            For Each g In groups
                Dim participantsCount As Integer = 0
                If g.groupMetadata IsNot Nothing AndAlso g.groupMetadata.participants IsNot Nothing Then
                    participantsCount = g.groupMetadata.participants.Count
                End If
                dt.Rows.Add(g.id, g.name, participantsCount, g.pinned, g.unread)
            Next

            DataGridView1.DataSource = dt

            ' ---------------------------------------------------------
            ' 6. معالجة الأخطاء
            ' ---------------------------------------------------------
        Catch webEx As WebException
            If webEx.Response IsNot Nothing Then
                Using errorReader As New StreamReader(webEx.Response.GetResponseStream())
                    MsgBox("خطأ من السيرفر: " & errorReader.ReadToEnd(),
                           MsgBoxStyle.Critical, "خطأ في جلب المجموعات")
                End Using
            Else
                MsgBox("تعذر الاتصال بالسيرفر.", MsgBoxStyle.Critical, "خطأ اتصال")
            End If
        Catch ex As Exception
            MsgBox("خطأ غير متوقع: " & ex.Message, MsgBoxStyle.Critical, "خطأ عام")
        End Try

    End Sub

End Module


' يمثل الـ Root
Public Class WhatsAppGroup
    Public Property id As String
    Public Property name As String
    Public Property isGroup As Boolean
    Public Property pinned As Boolean
    Public Property unread As Integer
    Public Property groupMetadata As GroupMetadata
End Class

Public Class GroupMetadata
    Public Property participants As List(Of Participant)
End Class

Public Class Participant
    Public Property id As String
    Public Property isAdmin As Boolean
    Public Property isSuperAdmin As Boolean
End Class