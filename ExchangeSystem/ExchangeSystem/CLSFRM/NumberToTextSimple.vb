Public Class NumberToTextSimple
    Dim I As Integer = 0
    Dim MyArry1(0 To 9), MyArry2(0 To 9), MyArry3(0 To 9), MyNo, RdNo, My100, My10, My1, My11, My12, GetTxt, Mybillion, MyMillion, MyThou, MyHun, MyFraction, ReMark As String



    Function NoToTxt(ByVal TheNo As Double, ByVal MyCur As String, ByVal MySubCur As String) As String
        Dim GetNo As String = Format(TheNo, "000000000000.00")
        Dim MyAnd As String = " و"


        If TheNo > 999999999999.99 Then Exit Function
        If TheNo = 0 Then
            NoToTxt = "صفر"
            Exit Function
        End If

        MyArry1(0) = String.Empty
        MyArry1(1) = "مائة"
        MyArry1(2) = "مائتان"
        MyArry1(3) = "ثلاثمائة"
        MyArry1(4) = "أربعمائة"
        MyArry1(5) = "خمسمائة"
        MyArry1(6) = "ستمائة"
        MyArry1(7) = "سبعمائة"
        MyArry1(8) = "ثمانمائة"
        MyArry1(9) = "تسعمائة"

        MyArry2(0) = String.Empty
        MyArry2(1) = " عشر"
        MyArry2(2) = "عشرون"
        MyArry2(3) = "ثلاثون"
        MyArry2(4) = "أربعون"
        MyArry2(5) = "خمسون"
        MyArry2(6) = "ستون"
        MyArry2(7) = "سبعون"
        MyArry2(8) = "ثمانون"
        MyArry2(9) = "تسعون"

        MyArry3(0) = String.Empty
        MyArry3(1) = "واحد"
        MyArry3(2) = "اثنان"
        MyArry3(3) = "ثلاثة"
        MyArry3(4) = "أربعة"
        MyArry3(5) = "خمسة"
        MyArry3(6) = "ستة"
        MyArry3(7) = "سبعة"
        MyArry3(8) = "ثمانية"
        MyArry3(9) = "تسعة"

        Do While I < 15
            If I < 12 Then
                MyNo = Mid$(GetNo, I + 1, 3)
            Else
                MyNo = "0" + Mid$(GetNo, I + 2, 2)
            End If

            If (Mid$(MyNo, 1, 3)) > 0 Then

                RdNo = Mid$(MyNo, 1, 1)
                My100 = MyArry1(RdNo)
                RdNo = Mid$(MyNo, 3, 1)
                My1 = MyArry3(RdNo)
                RdNo = Mid$(MyNo, 2, 1)
                My10 = MyArry2(RdNo)

                If Mid$(MyNo, 2, 2) = 11 Then My11 = "إحدى عشر"
                If Mid$(MyNo, 2, 2) = 12 Then My12 = "إثنى عشر"
                If Mid$(MyNo, 2, 2) = 10 Then My10 = "عشرة"

                If ((Mid$(MyNo, 1, 1)) > 0) And ((Mid$(MyNo, 2, 2)) > 0) Then My100 = My100 + MyAnd
                If ((Mid$(MyNo, 3, 1)) > 0) And ((Mid$(MyNo, 2, 1)) > 1) Then My1 = My1 + MyAnd

                GetTxt = My100 + My1 + My10

                If ((Mid$(MyNo, 3, 1)) = 1) And ((Mid$(MyNo, 2, 1)) = 1) Then
                    GetTxt = My100 + My11
                    If ((Mid$(MyNo, 1, 1)) = 0) Then GetTxt = My11
                End If

                If ((Mid$(MyNo, 3, 1)) = 2) And ((Mid$(MyNo, 2, 1)) = 1) Then
                    GetTxt = My100 + My12
                    If ((Mid$(MyNo, 1, 1)) = 0) Then GetTxt = My12
                End If

                If (I = 0) And (GetTxt <> String.Empty) Then
                    If ((Mid$(MyNo, 1, 3)) > 10) Then
                        Mybillion = GetTxt + " مليار"
                    Else
                        Mybillion = GetTxt + " مليارات"
                        If ((Mid$(MyNo, 1, 3)) = 2) Then Mybillion = " مليار"
                        If ((Mid$(MyNo, 1, 3)) = 2) Then Mybillion = " ملياران"
                    End If
                End If

                If (I = 3) And (GetTxt <> String.Empty) Then

                    If ((Mid$(MyNo, 1, 3)) > 10) Then
                        MyMillion = GetTxt + " مليون"
                    Else
                        MyMillion = GetTxt + " ملايين"
                        If ((Mid$(MyNo, 1, 3)) = 1) Then MyMillion = " مليون"
                        If ((Mid$(MyNo, 1, 3)) = 2) Then MyMillion = " مليونان"
                    End If
                End If

                If (I = 6) And (GetTxt <> String.Empty) Then
                    If ((Mid$(MyNo, 1, 3)) > 10) Then
                        MyThou = GetTxt + " ألف"
                    Else
                        MyThou = GetTxt + " آلاف"
                        If ((Mid$(MyNo, 3, 1)) = 1) Then MyThou = " ألف"
                        If ((Mid$(MyNo, 3, 1)) = 2) Then MyThou = " ألفان"
                    End If
                End If

                If (I = 9) And (GetTxt <> String.Empty) Then MyHun = GetTxt
                If (I = 12) And (GetTxt <> String.Empty) Then MyFraction = GetTxt
            End If

            I = I + 3
        Loop

        If (Mybillion <> String.Empty) Then
            If (MyMillion <> String.Empty) Or (MyThou <> String.Empty) Or (MyHun <> String.Empty) Then Mybillion = Mybillion + MyAnd
        End If

        If (MyMillion <> String.Empty) Then
            If (MyThou <> String.Empty) Or (MyHun <> String.Empty) Then MyMillion = MyMillion + MyAnd
        End If

        If (MyThou <> String.Empty) Then
            If (MyHun <> String.Empty) Then MyThou = MyThou + MyAnd
        End If

        If MyFraction <> String.Empty Then
            If (Mybillion <> String.Empty) Or (MyMillion <> String.Empty) Or (MyThou <> String.Empty) Or (MyHun <> String.Empty) Then
                NoToTxt = ReMark + Mybillion + MyMillion + MyThou + MyHun + " " + MyCur + MyAnd + MyFraction + " " + MySubCur
            Else
                NoToTxt = ReMark + MyFraction + " " + MySubCur
            End If
        Else
            NoToTxt = ReMark + Mybillion + MyMillion + MyThou + MyHun + " " + MyCur
        End If
    End Function
End Class
