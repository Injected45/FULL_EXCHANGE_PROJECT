Imports System.Data.SqlClient
Imports System.Management
Imports System.Net.NetworkInformation
Imports Microsoft.Win32
Imports System.Security.AccessControl
Imports System.Text
Imports System.Security.Cryptography
Imports DevExpress.XtraSplashScreen
Imports System.Net


Public Class WaitForm1


    Sub New()
        InitializeComponent()

        'Dim vrg As Integer
        'vrg = WaitForm2.ChickUpdate(vrg)
        'If vrg = 0 Then
        '    WaitForm2.ShowDialog()

        'End If

        Me.labelCopyright.Text = "جميع الحقوقة مححفوظة لشركة الرحالة للبرمجيات لسنة - " & DateTime.Now.Year.ToString()
    End Sub

    Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)


        MyBase.ProcessCommand(cmd, arg)

    End Sub

    Public Enum SplashScreenCommand


        SomeCommandId



    End Enum





End Class
