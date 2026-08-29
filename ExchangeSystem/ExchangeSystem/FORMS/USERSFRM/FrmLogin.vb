Imports ExchangeSystem.ExchangeSystem.CLSFRM
Imports System.Data.SqlClient
Imports System.Threading

Public Class FrmLogin
    Public Sub RIBBONPAGECHECK()
        CHECKRIBBONPAGE_FalseTrue(FRMMAIN.RPBASICINFO.Tag, UserID, GProfIDLog, FRMMAIN.RPBASICINFO)
        CHECKRIBBONPAGE_FalseTrue(FRMMAIN.RP2.Tag, UserID, GProfIDLog, FRMMAIN.RP2)
        CHECKRIBBONPAGE_FalseTrue(FRMMAIN.RP3.Tag, UserID, GProfIDLog, FRMMAIN.RP3)
        CHECKRIBBONPAGE_FalseTrue(FRMMAIN.RP4.Tag, UserID, GProfIDLog, FRMMAIN.RP4)
        CHECKRIBBONPAGE_FalseTrue(FRMMAIN.RP5.Tag, UserID, GProfIDLog, FRMMAIN.RP5)
        CHECKRIBBONPAGE_FalseTrue(FRMMAIN.RP6.Tag, UserID, GProfIDLog, FRMMAIN.RP6)
    End Sub

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For fadein = 0 To 2 Step 0.1
            Me.Opacity = fadein
            Me.Refresh()
            Threading.Thread.Sleep(50)
        Next
        OPENCONNECTION()

    End Sub
    Public Function GETMAINBR()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranch_GetMainBranch")
        MAINBID = dt.Rows(0)("ID")
        MAINCountryID = dt.Rows(0)("CountryId")
        Return dt
    End Function

    Public Function CHECK_USER_LOG(ByVal UNameLog As String, ByVal UPass As String)

        Dim dt As New DataTable
        dt.Clear()
        Dim parm(1) As SqlParameter
        parm(0) = New SqlParameter("@UNameLog", SqlDbType.VarChar, -1)
        parm(0).Value = UNameLog.Trim
        parm(1) = New SqlParameter("@UPass", SqlDbType.VarChar, -1)
        parm(1).Value = UPass
        dt = RUN_QUARY_PRO("User_TB_CHECKACOUNT", parm)
        Return dt
        dt.Dispose()
    End Function
    Public Property Actions As List(Of Master.Actions)
    Public Property MetroFramework As Object

    Public Sub LOGINTRY()
        If UserName.Text.Trim = "" Then
            UserName.ErrorText = "يرجى إدخال اسم المستخدم"
            UserName.Focus()
            Exit Sub
        End If
        If UserPassword.Text.Trim = "" Then
            UserPassword.ErrorText = "يرجى إدخال كلمة المرور"
            UserPassword.Focus()
            Exit Sub
        End If
        Dim dtloguser As New DataTable
        dtloguser.Clear()
        dtloguser = CHECK_USER_LOG(UserName.Text.Trim, UserPassword.Text.Trim)
        If dtloguser.Rows.Count > 0 Then
            If dtloguser.Rows(0).Item("IsActive") = False Then
                ErrorMessage(Me, "رسالة تنبيه", "تم تعطيل هذا الحساب، يرجى مراجعة الإدارة")
                UserName.Select()
                New_Controlrs(Me)

                Exit Sub
            ElseIf dtloguser.Rows(0).Item("IsActive") = True Then
                UserID = dtloguser.Rows(0)("USID")
                Dim chkepetca As New DataTable
                chkepetca.Clear()
                chkepetca = CHECKUSERHASPETTYCASH(UserID)
                If chkepetca.Rows.Count > 0 Then
                    ErrorMessage(Me, "رسالة تنبيه", "لديك عهدة ولم يتم تسويتها يرجى تسوية العهدة أولاً")
                    Exit Sub
                End If
            End If
        End If
        Try

            If dtloguser.Rows.Count > 0 And dtloguser.Rows(0).Item("IsActive") = True Then
                UserPhone = dtloguser.Rows(0)("Phone").ToString
                FrmLogInOTP.lodeDate(UserPhone)
                FrmLogInOTP.ShowDialog()
                If FrmLogInOTP.chickLog = True Or FRMMAIN.BarButtonItem130.Caption = " 2026 تجريب منظومة الي فووق داتا سنتر" Then
                    UserID = dtloguser.Rows(0)("USID")
                    GProfIDLog = dtloguser.Rows(0)("USettingProfileID")
                    BID = dtloguser.Rows(0)("BranchID")
                    UserLogName = dtloguser.Rows(0)("UNameLog")
                    GetUserName = dtloguser.Rows(0)("UName")
                    UserPass = dtloguser.Rows(0)("UPass")
                    GetBranchName = dtloguser.Rows(0)("BName")
                    BRKey = dtloguser.Rows(0)("branchkey")
                    COUNTRYNID = dtloguser.Rows(0)("CountryID")
                    CITYID = dtloguser.Rows(0)("CityID")

                    UserAccID = dtloguser.Rows(0)("AccID")
                    UserType = dtloguser.Rows(0)("UserType")
                    IsLimited = dtloguser.Rows(0)("IsLimited")
                    LimitedVal = dtloguser.Rows(0)("LimitedVal")


                    LodeSEcreen()

                    FRMMAIN.Enabled = True



                    GETMAINBR()
                    GETDefaultCurr(dtloguser.Rows(0)("CountryID"))
                    Me.Close()
                Else
                    ErrorMessage(Me, "تنبيه", "عذرا رقم الكود غير صحيح الرجاء اعادة المحاولة")
                End If
            Else
                    UserName.Select()
                New_Controlrs(Me)
                ErrorMessage(Me, "رسالة خطأ", "اسم المستخدم أو كلمة المرور غير صحيحة")
                UserName.Text = String.Empty : UserPassword.Text = String.Empty : UserName.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            UserName.Select()
            New_Controlrs(Me)
            ErrorMessage(Me, "رسالة خطأ", "بيانات الدخول غير صحيحة . يرجى المحاولة مجدداً")
        End Try
    End Sub
    ''اجراء خاص بتعبئة الشاشات من الصلاحيات
    Public Sub LodeSEcreen()
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileGID", SqlDbType.Int) With {.Value = GProfIDLog}
        PRM(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = UserID}
        Module2.CHECKOFORMVISIBEL_FalseOrTrue(FRMMAIN, "UserAccessProfileTemplate_ueserId_roles", PRM, "ShortName", "CanShow")
        'If FRMMAIN.BtnCustomerMovement.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
        '    FRMMAIN.LayoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        'Else
        '    FRMMAIN.LayoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        'End If
        'If FRMMAIN.BarButtonItem108.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
        '    FRMMAIN.LayoutControlGroup5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        'Else
        '    FRMMAIN.LayoutControlGroup5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        'End If
        'If FRMMAIN.BarButtonItem112.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
        '    FRMMAIN.LayoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        'Else
        '    FRMMAIN.LayoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        'End If

    End Sub
    '' get Worck From profile and manie 
    'Public Sub git_min(type As Integer)
    '    Dim PRM(1) As SqlParameter
    '    PRM(0) = New SqlParameter("@Userid", SqlDbType.Int) With {.Value = GProfIDLog}
    '    PRM(1) = New SqlParameter("@type", SqlDbType.Int) With {.Value = UserID}
    '    Module2.CHECKOFORMVISIBEL_FalseOrTrue(FRMMAIN, "git_min", PRM, "ShortName", "CanShowtet")
    'End Sub

    Public Overridable Sub EnterKeyMove()
        FrmLogin_KeyDown(Nothing, Nothing)
    End Sub
    Private Sub Login_Click(sender As Object, e As EventArgs) Handles Login.Click
        LOGINTRY()
        'FRMMAIN.Timer2.Start()
    End Sub
    Private Sub CancelLog_Click(sender As Object, e As EventArgs) Handles CancelLog.Click
        SQLCON.Dispose()
        QSCON.Dispose()
        Application.Exit()
    End Sub

    Private Sub FrmLogin_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BaWo.DoWork
        refresh_table(BID)
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BaWo.RunWorkerCompleted
        refresh_table(BID)
    End Sub

End Class