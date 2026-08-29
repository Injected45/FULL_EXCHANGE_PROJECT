Module ConvertNumbersToWords_Module
    Dim and_Str As String = " و "

    Public Function ConvertNumbersToWords(ByVal Number As Decimal, ByVal NameOfUnitBeforeDecimal As String, ByVal NameOfUnitAfterDecimal As String, Optional ByVal CustomTextBeforeWordsOfDigits As String = "", Optional ByVal CustomTextAfterWordsOfDigits As String = "") As String
        ConvertNumbersToWords = String.Empty
        Dim numbersArray_Str(2) As String
        numbersArray_Str = Split(CStr(Number), ".")
        Dim numberBeforePoint_Lng, numberAfterPoint_Lng As Long
        If numbersArray_Str(0).Length <= 15 Then
            If numbersArray_Str(0) <> String.Empty Then
                numberBeforePoint_Lng = CLng(numbersArray_Str(0))
            End If
        Else
            Return String.Empty
            Exit Function
        End If
        If numbersArray_Str.Length > 1 Then
            If numbersArray_Str(1).Length <= 15 Then
                If numbersArray_Str(1) <> String.Empty Then
                    numberAfterPoint_Lng = CLng(numbersArray_Str(1))
                End If
            Else
                Return String.Empty
                Exit Function
            End If
        End If
        If numberAfterPoint_Lng <> 0 Then
            If numberBeforePoint_Lng > 0 Then
                Return (CustomTextBeforeWordsOfDigits & " " & wordsOfDigits(numberBeforePoint_Lng) & " " & NameOfUnitBeforeDecimal & and_Str & wordsOfDigits(numberAfterPoint_Lng) & " " & NameOfUnitAfterDecimal & " " & CustomTextAfterWordsOfDigits).Trim
            Else
                Return (CustomTextBeforeWordsOfDigits & " " & wordsOfDigits(numberAfterPoint_Lng) & " " & NameOfUnitAfterDecimal & " " & CustomTextAfterWordsOfDigits).Trim
            End If
        Else
            If numberBeforePoint_Lng > 0 Then
                Return (CustomTextBeforeWordsOfDigits & " " & wordsOfDigits(numberBeforePoint_Lng) & " " & NameOfUnitBeforeDecimal & " " & CustomTextAfterWordsOfDigits).Trim
            End If
        End If
    End Function

    Private Function wordsOfDigits(ByVal number As Long) As String
        wordsOfDigits = String.Empty
        If number <= 10 Then
            Select Case number
                Case Is = 1 : Return "واحد"
                Case Is = 2 : Return "اثنان"
                Case Is = 3 : Return "ثلاثة"
                Case Is = 4 : Return "أربعة"
                Case Is = 5 : Return "خمسة"
                Case Is = 6 : Return "ستة"
                Case Is = 7 : Return "سبعة"
                Case Is = 8 : Return "ثمانية"
                Case Is = 9 : Return "تسعة"
                Case Is = 10 : Return "عشرة"
            End Select
        End If
        If number >= 11 And number <= 19 Then
            Select Case number
                Case Is = 11 : Return "أحد عشر"
                Case Is = 12 : Return "اثنا عشر"
                Case Is = 13 : Return "ثلاثة عشر"
                Case Is = 14 : Return "أربعة عشر"
                Case Is = 15 : Return "خمسة عشر"
                Case Is = 16 : Return "ستة عشر"
                Case Is = 17 : Return "سبعة عشر"
                Case Is = 18 : Return "ثمانية عشر"
                Case Is = 19 : Return "تسعة عشر"
            End Select
        End If
        If number >= 20 And number <= 99 Then
            Dim firstDigit_Lng As Long = number Mod 10
            Dim secondDigit_Lng As Long = number \ 10
            Dim wordOfDigit_Str As String = String.Empty
            Select Case secondDigit_Lng
                Case Is = 2 : wordOfDigit_Str = "عشرون"
                Case Is = 3 : wordOfDigit_Str = "ثلاثون"
                Case Is = 4 : wordOfDigit_Str = "أربعون"
                Case Is = 5 : wordOfDigit_Str = "خمسون"
                Case Is = 6 : wordOfDigit_Str = "ستون"
                Case Is = 7 : wordOfDigit_Str = "سبعون"
                Case Is = 8 : wordOfDigit_Str = "ثمانون"
                Case Is = 9 : wordOfDigit_Str = "تسعون"
            End Select
            If firstDigit_Lng <> 0 Then
                wordOfDigit_Str = wordsOfDigits(firstDigit_Lng) & and_Str & wordOfDigit_Str
            End If
            Return wordOfDigit_Str
        End If
        If number >= 100 And number <= 999 Then
            Dim firstDigit_Lng As Long = number Mod 100
            Dim secondDigit_Lng As Long = number \ 100
            Dim wordOfDigit_Str As String = String.Empty
            Select Case secondDigit_Lng
                Case Is = 1 : wordOfDigit_Str = "مائة"
                Case Is = 2 : wordOfDigit_Str = "مائتان"
                Case Is = 3 : wordOfDigit_Str = "ثلاثمائة"
                Case Is = 4 : wordOfDigit_Str = "أربعمائة"
                Case Is = 5 : wordOfDigit_Str = "خمسمائة"
                Case Is = 6 : wordOfDigit_Str = "ستمائة"
                Case Is = 7 : wordOfDigit_Str = "سبعمائة"
                Case Is = 8 : wordOfDigit_Str = "ثمانمائة"
                Case Is = 9 : wordOfDigit_Str = "تسعمائة"
            End Select
            If firstDigit_Lng <> 0 Then
                wordOfDigit_Str = wordOfDigit_Str & and_Str & wordsOfDigits(firstDigit_Lng)
            End If
            Return wordOfDigit_Str
        End If
        If number >= 1000 And number <= 999999 Then
            Dim firstDigit_Lng As Long = number Mod 1000
            Dim secondDigit_Lng As Long = number \ 1000
            Dim wordOfDigit_Str As String = String.Empty
            Select Case secondDigit_Lng
                Case Is = 1 : wordOfDigit_Str = "ألف"
                Case Is = 2 : wordOfDigit_Str = "ألفان"
                Case Is <= 10
                    wordOfDigit_Str = " آلاف"
                    wordOfDigit_Str = wordsOfDigits(secondDigit_Lng) & wordOfDigit_Str
                Case Else
                    wordOfDigit_Str = " ألف"
                    wordOfDigit_Str = wordsOfDigits(secondDigit_Lng) & wordOfDigit_Str
            End Select
            If firstDigit_Lng <> 0 Then
                wordOfDigit_Str = wordOfDigit_Str & and_Str & wordsOfDigits(firstDigit_Lng)
            End If
            Return wordOfDigit_Str
        End If
        If number >= 1000000 And number <= 999999999 Then
            Dim firstDigit_Lng As Long = number Mod 1000000
            Dim secondDigit_Lng As Long = number \ 1000000
            Dim wordOfDigit_Str As String = String.Empty
            Select Case secondDigit_Lng
                Case Is = 1 : wordOfDigit_Str = "مليون"
                Case Is = 2 : wordOfDigit_Str = "مليونان"
                Case Is <= 10
                    wordOfDigit_Str = " ملايين"
                    wordOfDigit_Str = wordsOfDigits(secondDigit_Lng) & wordOfDigit_Str
                Case Else
                    wordOfDigit_Str = " مليون"
                    wordOfDigit_Str = wordsOfDigits(secondDigit_Lng) & wordOfDigit_Str
            End Select
            If firstDigit_Lng <> 0 Then
                wordOfDigit_Str = wordOfDigit_Str & and_Str & wordsOfDigits(firstDigit_Lng)
            End If
            Return wordOfDigit_Str
        End If
        If number >= 1000000000 And number <= 999999999999 Then
            Dim firstDigit_Lng As Long = number Mod 1000000000
            Dim secondDigit_Lng As Long = number \ 1000000000
            Dim wordOfDigit_Str As String = String.Empty
            Select Case secondDigit_Lng
                Case Is = 1 : wordOfDigit_Str = "مليار"
                Case Is = 2 : wordOfDigit_Str = "ملياران"
                Case Else
                    wordOfDigit_Str = " مليار"
                    wordOfDigit_Str = wordsOfDigits(secondDigit_Lng) & wordOfDigit_Str
            End Select
            If firstDigit_Lng <> 0 Then
                wordOfDigit_Str = wordOfDigit_Str & and_Str & wordsOfDigits(firstDigit_Lng)
            End If
            Return wordOfDigit_Str
        End If
        If number >= 1000000000000 And number <= 999999999999999 Then
            Dim firstDigit_Lng As Long = number Mod 1000000000000
            Dim secondDigit_Lng As Long = number \ 1000000000000
            Dim wordOfDigit_Str As String = String.Empty
            Select Case secondDigit_Lng
                Case Is = 1 : wordOfDigit_Str = "تريليون"
                Case Else
                    wordOfDigit_Str = " تريليون"
                    wordOfDigit_Str = wordsOfDigits(secondDigit_Lng) & wordOfDigit_Str
            End Select
            If firstDigit_Lng <> 0 Then
                wordOfDigit_Str = wordOfDigit_Str & and_Str & wordsOfDigits(firstDigit_Lng)
            End If
            Return wordOfDigit_Str
        End If
    End Function
End Module
