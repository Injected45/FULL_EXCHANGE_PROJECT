Public Class arabicconverter
    Private strAhad(10000) As String
    Private strRead As String = ""
    Private Num01 As Short
    Private Num02 As Short
    Private Num03 As Short
    Private عشر As String = "عشر"
    Private الف As String = "الف"
    Private الفان As String = "الفان"
    Private آلاف As String = "آلاف"
    Private الفا As String = "الفاً"
    Private مليون As String = "مليون"
    Private مليونان As String = "مليونان"
    Private مليار As String = "مليار"
    Private Sub تسمية_خاصة()
        strAhad(13) = "عشر"
        strAhad(15) = ""
        strAhad(20) = "عشرون"
        strAhad(30) = "ثلاثون"
        strAhad(40) = "اربعون"
        strAhad(50) = "خمسون"
        strAhad(60) = "ستون"
        strAhad(70) = "سبعون"
        strAhad(80) = "ثمانون"
        strAhad(90) = "تسعون"
        strAhad(100) = "مائة"
        strAhad(200) = "مائتا"
        strAhad(1000) = "الف"
        strAhad(2000) = "الفان"


    End Sub
    Private Sub الآحاد()
        strAhad(0) = "صفر"
        strAhad(1) = "واحد"
        strAhad(2) = "اثنان"
        strAhad(3) = "ثلاثة"
        strAhad(4) = "اربعة"
        strAhad(5) = "خمسة"
        strAhad(6) = "ستة"
        strAhad(7) = "سبعة"
        strAhad(8) = "ثمانية"
        strAhad(9) = "تسعة"
        strAhad(10) = "عشرة"
        strAhad(11) = "إحدى عشر"
        strAhad(12) = "إثنى عشر"
    End Sub
    Private Function خانة_الآحاد(ByVal nValue As Long) As String
        خانة_الآحاد = ""
        Select Case nValue
            Case 15
                خانة_الآحاد = strAhad(nValue)

            Case 0 To 12
                خانة_الآحاد = strAhad(nValue)
            Case 1000, 2000
                خانة_الآحاد = strAhad(nValue)
        End Select
    End Function
    Private Function خانة_العشرات(ByVal nValue As Long) As String
        strRead = ""
        If nValue.ToString.Length = 2 Then
            Select Case nValue
                Case 10, 11, 12
                    خانة_العشرات = strRead
                    Exit Function
            End Select

            'من 13 الى 99
            Num01 = CType(Microsoft.VisualBasic.Right(nValue.ToString, 1), Long) 'Val(nValue.ToString.Last())
            Num02 = nValue - Num01

            If Num01 = 0 Then
                strRead = strAhad(Num02)
            ElseIf nValue <= 19 Then
                strRead = strAhad(Num01) & " " & strAhad(Num02).Replace("ة", "")
            Else
                strRead = strAhad(Num01) & " و" & strAhad(Num02)
            End If


        End If
        خانة_العشرات = strRead
    End Function
    Private Function خانة_المئات(ByVal nValue As Long) As String
        strRead = ""
        If nValue.ToString.Length = 3 Then
            Dim AValue As Long
            Dim BValue As Long
            AValue = CType(Microsoft.VisualBasic.Right(nValue.ToString, 2), Long)
            BValue = nValue - AValue
            If BValue = 100 Or BValue = 200 Then
                If AValue = 0 Then
                    strRead = strAhad(BValue)
                ElseIf AValue <= 12 Then
                    strRead = strAhad(BValue) & " و" & strAhad(AValue)

                Else
                    strRead = strAhad(BValue) & " و" & خانة_العشرات(AValue)
                End If

            Else
                If AValue >= 1 And AValue <= 12 Then

                    strRead = strAhad(Val(Microsoft.VisualBasic.Left(BValue.ToString, 1))).Remove(strAhad(Val(Microsoft.VisualBasic.Left(BValue.ToString, 1))).Count - 1) & strAhad(100) & _
                              " و" & strAhad(AValue)


                ElseIf AValue <> 0 Then

                    strRead = strAhad(Val(Microsoft.VisualBasic.Left(BValue.ToString, 1))).Remove(strAhad(Val(Microsoft.VisualBasic.Left(BValue.ToString, 1))).Count - 1) & strAhad(100) & _
                              " و" & خانة_العشرات(AValue)
                ElseIf AValue = 0 Then
                    strRead = strAhad(Val(Microsoft.VisualBasic.Left(BValue.ToString, 1))).Remove(strAhad(Val(Microsoft.VisualBasic.Left(BValue.ToString, 1))).Count - 1) & strAhad(100)


                End If
            End If

        End If
        خانة_المئات = strRead
    End Function

    Private Function ReadHoorof(ByVal nValue As Long) As String
        Call تسمية_خاصة()
        Call الآحاد()
        If nValue <= 12 Then
            strRead = خانة_الآحاد(nValue)
        ElseIf nValue.ToString.Length = 2 Then
            strRead = خانة_العشرات(nValue)
        ElseIf nValue.ToString.Length = 3 Then
            strRead = خانة_المئات(nValue)


        End If

        ReadHoorof = strRead

    End Function
    Private Function GetHoorof(ByVal nValue As Long) As String
        Dim AValue As Long = 0
        Dim BValue As Long = 0

        Dim strRead As String = ""
        Call تسمية_خاصة()
        Call الآحاد()

        Select Case nValue.ToString.Length()
            Case 1
                strRead = strAhad(nValue)
            Case 2
                Select Case nValue
                    Case 10, 11, 12
                        strRead = strAhad(nValue)
                    Case 13 To 19
                        strRead = strAhad(nValue) & " " & عشر

                End Select
        End Select
        GetHoorof = strRead
    End Function
    Public Function numtolit(ByVal الرقم As Decimal, ByVal عدد_الخانات_العشرية As Byte, ByVal تسمية_العملة As String, ByVal تسمية_الوحدة As String, ByVal إضافة_كلمة_فقط As Boolean, ByVal إضافة_كلمة_لاغير As Boolean) As String

        Dim مضروب_الخانات_العشرية As Integer

        Select Case عدد_الخانات_العشرية
            Case 1
                مضروب_الخانات_العشرية = 10
            Case 2
                مضروب_الخانات_العشرية = 100
            Case 3
                مضروب_الخانات_العشرية = 1000
            Case 4
                مضروب_الخانات_العشرية = 10000
            Case 5
                مضروب_الخانات_العشرية = 100000
        End Select
        Dim كلمة_لاغير As String = ""
        Dim كلمة_فقط As String = ""

        If إضافة_كلمة_لاغير = True Then
            كلمة_لاغير = "لاغير"
        Else
            كلمة_لاغير = ""

        End If
        If إضافة_كلمة_فقط = True Then
            كلمة_فقط = "فقط"
        Else
            كلمة_فقط = ""

        End If

        Dim قائمة_القيم As New List(Of Long)
        Dim عدد_صحيح As Long
        Dim عدد_عشري As Long

        عدد_صحيح = Math.Truncate(الرقم)
        عدد_عشري = Math.Truncate((Decimal.Subtract(الرقم, Decimal.Floor(الرقم))) * مضروب_الخانات_العشرية)


        Dim delimStr As String = ","
        Dim delimiter As Char() = delimStr.ToCharArray()
        Dim words As String = FormatNumber(عدد_صحيح, 0, TriState.True).ToString() ' "one two,three:four."
        Dim split As String() = Nothing

        Dim I As Integer
        Dim sum As Byte = 0
        For I = 0 To words.Count - 1
            If words(I) = "," Then
                sum = sum + 1
            End If
        Next I

        split = words.Split(delimiter, sum + 1)
        Dim s As String
        Dim ss As String = ""

        For Each s In split
            قائمة_القيم.Add(CType(s, Long))
        Next s
        Dim strHorof As String = ""
        Dim strHorof1 As String = ""
        Dim strHorof2 As String = ""
        Dim strHorof3 As String = ""
        Dim strHorof4 As String = ""
        Select Case قائمة_القيم.Count
            Case 1

                strHorof = ReadHoorof(قائمة_القيم.Item(0))

            Case 2
                If قائمة_القيم.Item(1) = 0 Then
                    Select Case قائمة_القيم.Item(0)
                        Case 1
                            strHorof = "الف"
                        Case 2
                            strHorof = "الفان"
                        Case 3 To 10
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "آلاف"
                        Case Else
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "الف"
                    End Select
                Else
                    Select Case قائمة_القيم.Item(0)
                        Case 1
                            strHorof = "الف" & " و" & ReadHoorof(قائمة_القيم.Item(1))
                        Case 2
                            strHorof = "الفان" & " و" & ReadHoorof(قائمة_القيم.Item(1))
                        Case 3 To 10
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "آلاف" & " و" & ReadHoorof(قائمة_القيم.Item(1))
                        Case Else
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "الف" & " و" & ReadHoorof(قائمة_القيم.Item(1))
                    End Select

                End If

            Case 3
                If قائمة_القيم.Item(1) = 0 And قائمة_القيم.Item(2) = 0 Then
                    Select Case قائمة_القيم.Item(0)
                        Case 1
                            strHorof = "مليون"
                        Case 2
                            strHorof = "مليونان"
                        Case Else
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "مليون"
                    End Select

                ElseIf قائمة_القيم.Item(2) = 0 Then
                    Select Case قائمة_القيم.Item(1)
                        Case 1
                            strHorof1 = "الف"
                        Case 2
                            strHorof1 = "الفان"
                        Case 3 To 10
                            strHorof1 = ReadHoorof(قائمة_القيم.Item(1)) & " " & "آلاف"
                        Case Else
                            strHorof1 = ReadHoorof(قائمة_القيم.Item(1)) & " " & "الف"
                    End Select
                    Select Case قائمة_القيم.Item(0)
                        Case 1
                            strHorof = "مليون" & " و" & strHorof1
                        Case 2
                            strHorof = "مليونان" & " و" & strHorof1
                        Case Else
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "مليون" & " و" & strHorof1
                    End Select

                Else
                    strHorof2 = ReadHoorof(قائمة_القيم.Item(2))
                    Select Case قائمة_القيم.Item(1)
                        Case 1
                            strHorof1 = "الف"
                        Case 2
                            strHorof1 = "الفان"
                        Case 3 To 10
                            strHorof1 = ReadHoorof(قائمة_القيم.Item(1)) & " " & "آلاف"
                        Case Else
                            strHorof1 = ReadHoorof(قائمة_القيم.Item(1)) & " " & "الف"
                    End Select
                    Select Case قائمة_القيم.Item(0)
                        Case 1
                            strHorof = "مليون" & " و" & strHorof1 & " و" & strHorof2
                        Case 2
                            strHorof = "مليونان" & " و" & strHorof1 & " و" & strHorof2
                        Case Else
                            strHorof = ReadHoorof(قائمة_القيم.Item(0)) & " " & "مليون" & " و" & strHorof1 & " و" & strHorof2
                    End Select

                End If


            Case 4

        End Select
        If عدد_عشري = 0 Then
            numtolit = كلمة_فقط & " " & strHorof & " " & تسمية_العملة & " " & كلمة_لاغير
        Else
            numtolit = كلمة_فقط & " " & strHorof & " " & تسمية_العملة & " و" & عدد_عشري.ToString() & " " & تسمية_الوحدة & " " & كلمة_لاغير
        End If
    End Function
End Class
