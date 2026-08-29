Imports System.Data.SqlClient

Public Class FrmLogInOTP



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

    End Sub
    Public Sub sandranfd()

        excode = RN.Next(1000, 10000)

    End Sub

    Public Sub lodeDate(phone As String)
        Try

            CODEISACTIVE.Focus()
            TextEdit5.Text = phone
            Timer1.Start()
            sandranfd()
            SandCODEForLogIn()
            CODEISACTIVE.Text = String.Empty
            CODEISACTIVE.Select()
        Catch ex As Exception
            MessageBox.Show($"Erroor mesgg : {ex.Message}")
        End Try
    End Sub





    Public Function chickLog() As Boolean
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
        chickLog()
        Me.Close()
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
        SandCODEForLogIn()

    End Sub

    Private Sub CODEISACTIVE_KeyDown(sender As Object, e As KeyEventArgs) Handles CODEISACTIVE.KeyDown
        If e.KeyCode = Keys.Enter Then
            SimpleButton1.PerformClick()
        End If
    End Sub


End Class