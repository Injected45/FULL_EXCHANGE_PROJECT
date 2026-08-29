Imports System.Data.SqlClient

Public Class FRMCODEPYMENT_em_cu2


    Public Tvalue As Double
    Dim mm As Double = 0
    Dim ss As Integer = 0
    Dim hh As Integer = 0
    Dim RN As New Random
    Dim excode As String
    Public icccunt As Integer
    Dim ff As String
    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        mm = 0
        ss = 0
        ff = 0
        Timer1.Stop()

        Me.Close()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If ss = 60 Then
            ss = 0
            mm += 1
        ElseIf mm = 60 Then
            hh = 0
            hh += 1
        ElseIf hh = 24 Then
            hh = 0
        End If
        ss += 1

        ff = Convert.ToString(mm) + ":" + Convert.ToString(ss)
        LabelControl1.Text = "اجمالي الوقت المنقضي هو " + Space(1) + ff + Space(1) + "اجمالي الوقت هوا " + "3:00  دقايق"

        If mm = 3 Then
            ff = "3:00"
            LabelControl1.Text = "الوقت المتبقي للسحب هو " + Space(1) + ff + Space(1) + "اجمالي الوقت هوا " + "3:00  دقايق"
            Timer1.Stop()
            LabelControl1.Text = "عذرا تم انتهاء الوقت الرجاء المحاولة مرة اخرة "
            LabelControl1.ForeColor = Color.Red
            sandranfd()
            excode = RN.Next(1000, 10000)

        End If

    End Sub
    Public Sub sandranfd()

        excode = RN.Next(1000, 10000)




    End Sub

    Public Sub lodeDate(type As String, nameAcc As String, nameID As ULong, prase As Double, crunse As String, phone As String, type_From As Integer, COdefrom As String)
        Try


            TypeAccount.Text = type
            STORNAME.Text = nameAcc
            OverAllPrice.EditValue = prase
            Crunses.Text = crunse
            CODEISACTIVE.Focus()
            TextEdit5.Text = phone
            Timer1.Start()
            sandranfd()
            LabelControl1.ForeColor = Color.Black
            If type_From = 1 Then



                SandCODEFROMSTICOUNT()
            ElseIf type_From = 4 Then
                SandCODEFROMSTICOUNT_Seand_sel(nameID, type_From, COdefrom)
            ElseIf type_From = 5 Then
                SandCODEFROMSTICOUNfrom()
            Else

                SandCODEFROMSTICOUNT_Seand(nameID, type_From)
            End If
            CODEISACTIVE.Text = String.Empty
            CODEISACTIVE.Select()
        Catch ex As Exception
            MessageBox.Show($"Erroor mesgg : {ex.Message}")
        End Try
    End Sub



    Public Sub SandCODEFROMSTICOUNT()
        Try


            Dim msg As String
            msg = "*شركة الرحالة للصرافة*" & vbNewLine
            msg &= "رمز التحقق : " & excode & vbNewLine & "صلاحيته : " & "3:00" & Space(1) & "دقائق" & vbNewLine & "لا تشاركه مع أي شخص  " & vbNewLine

            If FRMEMPWITHDRAWAL.Text = "سند صرف لموظف" Then
                msg &= "طلبك : سند صرف لموظف"
            End If
            If FRMEMPWITHDRAWAL.Text = "سند صرف لعميل" Then
                msg &= "طلبك : سند صرف لعميل"
            End If
            If FRMEMPWITHDRAWAL.Text = "سند صرف من حساب وكيل" Then
                msg &= "طلبك : سند صرف من حساب وكيل"
            End If
            If FRMEMPWITHDRAWAL.Text = "سند صرف من حساب موظف بدون رصيد" Then
                msg &= "طلبك : سند صرف لموظف بدون رصيد"
            End If
            'If FRMADDINCOME.LOADTYPE = 36 Then
            '    msg &= "طلبك : سند صرف محطه"
            'End If

            Dim dt As New DataTable
            dt.Clear()

            WATSAPPMsAG(TextEdit5.Text, msg, True)

        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try

    End Sub

    '' ارسال حوالة ارسال كود 
    Public Sub SandCODEFROMSTICOUNT_Seand(id As Integer, tyefrom As Integer)
        Try


            Dim msg As String
            msg = "شركة الرحالة للصرافة" & vbNewLine

            msg &= "رمز التحقق : " & excode & vbNewLine & "صلاحيته : " & "3:00" & Space(1) & "دقائق" & vbNewLine & "لا تشاركه مع أي شخص  " & vbNewLine

            Select Case tyefrom
                Case 2
                    msg &= "طلبك : إصدار حوالة داخلية"
                Case Else
                    msg &= "طلبك : إصدار حوالة خارجية"
            End Select
            Dim dt As New DataTable
            dt.Clear()

            WATSAPPMsAG(TextEdit5.Text, msg, True)

        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try

    End Sub


    Public Sub SandCODEFROMSTICOUNT_Seand_sel(id As Integer, tyefrom As Integer, codeID As String)
        Try


            Dim msg As String
            msg = My.Settings.Combny_name & vbNewLine &
          "لاتمام عملية" & Space(1)


            Select Case tyefrom
                Case 5
                    msg &= "بيع"
                Case Else
                    msg &= "شراء"

            End Select
            msg &= Space(1) & "عملة" & vbNewLine & "CODE : " & codeID & vbNewLine
            msg &= "من حسابكم رقم  : " & GET_codefor_Acount_SaenFroWtsaap(id) & vbNewLine &
      "بقيمة : " & Cur_Code(Crunses.Text, OverAllPrice.EditValue, True, "n2") & vbNewLine &
      "زودنا برمز التحقق : " & excode & vbNewLine &
      "شكراً لتعاونكم معنا"


            Dim dt As New DataTable
            dt.Clear()

            WATSAPPMsAG(TextEdit5.Text, msg, True)

        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try

    End Sub


    Public Function chick() As Boolean
        If excode = CODEISACTIVE.Text Then

            Timer1.Stop()
            Me.Close()
            Return True
        Else
            Return False
            Me.Close()
        End If
    End Function

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If CODEISACTIVE.Text = String.Empty Then
            CODEISACTIVE.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        chick()
        Me.Close()
    End Sub

    Public Sub SandCODEFROMSTICOUNfrom()
        Try


            Dim msg As String
            msg = My.Settings.Combny_name & vbNewLine & "رمز التحقق" & Space(1) & ":" & Space(1) + excode + vbNewLine & "لسحب قيمة" & Space(1) & ":" & Space(1) & Cur_Code(Crunses.Text,
             OverAllPrice.EditValue, True, False) & vbNewLine &
              "من الجمعية " & vbNewLine & "شكراً لتعاونكم معنا"

            Dim dt As New DataTable
            dt.Clear()

            WATSAPPMsAG(TextEdit5.Text, msg, True)

        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try

    End Sub

    Public Sub SandCODEForLogIn()
        Try


            Dim msg As String
            msg = My.Settings.Combny_name & vbNewLine & "رمز التحقق" & Space(1) & ":" & Space(1) + excode + vbNewLine &
             "شكراً لتعاونكم معنا"

            Dim dt As New DataTable
            dt.Clear()

            WATSAPPMsAG(TextEdit5.Text, msg, True)

        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try

    End Sub
    Private Sub PrintSticker_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        excode = RN.Next(1000, 10000)
        Timer1.Stop()
        ss = 0
        mm = 0
        Timer1.Start()
        SandCODEFROMSTICOUNT()

    End Sub

    Private Sub CODEISACTIVE_KeyDown(sender As Object, e As KeyEventArgs) Handles CODEISACTIVE.KeyDown
        If e.KeyCode = Keys.Enter Then
            SimpleButton1.PerformClick()
        End If
    End Sub

End Class