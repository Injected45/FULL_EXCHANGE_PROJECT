Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class FrmWaitingLogin
    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        SQLCON.Dispose()
        QSCON.Dispose()
        Application.Exit()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If UserPass = UserPassword.Text Then
            Me.Close()
        Else
            ErrorMessage(Me, "رسالة تنبيه", "كلمة المرور خاطئة الرجاء إعادة المحاولة")
        End If
    End Sub


    Private Sub FrmWaitingLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UserPassword.Text = ""
        UserPassword.Focus()
    End Sub

    Private Sub UserPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles UserPassword.KeyDown

        If e.KeyCode = Keys.Enter Then
            SimpleButton2.PerformClick()
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True 'this will prevent ding sound 
    End Sub

    Private Sub FrmWaitingLogin_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        FRMMAIN.Refreshtimer()
    End Sub
End Class