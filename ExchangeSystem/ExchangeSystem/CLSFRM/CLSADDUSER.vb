Imports System.Data.SqlClient

Public Class CLSADDUSER
    Public Function CHECK_USER_NAME(ByVal UName As String, USID As Integer, IsUpdate As Boolean) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@UName", SqlDbType.NVarChar, -1) With {.Value = UName}
        PRM(1) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = USID}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("TB_Users_CHECKUName", PRM)
        End If
        Return DT
    End Function
    Public Function CHECK_USER_LOGNAME(ByVal UNameLog As String, USID As Integer, IsUpdate As Boolean) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@UNameLog", SqlDbType.NVarChar, -1) With {.Value = UNameLog}
        PRM(1) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = USID}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("TB_Users_CHECKUNameLog", PRM)
        End If
        Return DT
    End Function
    Public Sub TB_Users_Insert(USID As Integer, UName As String, UNameLog As String, UPass As String, IsActive As Boolean, BranchID As Integer, USettingProfileID As Integer, UserType As Int32,
                                IsUpdate As Boolean, EMPCurrentAccID As ULong, EMPID As Integer, IsEmpOrUser As Integer, ISHidden As Boolean, Phone As String)

        Try


            Dim PRM(15) As SqlParameter
            PRM(0) = New SqlParameter("@USID", SqlDbType.NVarChar, -1) With {.Value = USID}
            PRM(1) = New SqlParameter("@UName", SqlDbType.NVarChar, -1) With {.Value = UName}
            PRM(2) = New SqlParameter("@UNameLog", SqlDbType.NVarChar, -1) With {.Value = UNameLog}
            PRM(3) = New SqlParameter("@UPass", SqlDbType.NVarChar, -1) With {.Value = UPass}
            PRM(4) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
            PRM(5) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(6) = New SqlParameter("@USettingProfileID", SqlDbType.Int) With {.Value = USettingProfileID}
            PRM(7) = New SqlParameter("@UserType", SqlDbType.TinyInt) With {.Value = UserType}
            PRM(8) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(9) = New SqlParameter("@EMPCurrentAccID", SqlDbType.BigInt) With {.Value = EMPCurrentAccID}
            PRM(10) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            PRM(11) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(12) = New SqlParameter("@msgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(13) = New SqlParameter("@IsEmplloy", SqlDbType.Int) With {.Value = IsEmpOrUser}
            PRM(14) = New SqlParameter("@ISHidden", SqlDbType.Bit) With {.Value = ISHidden}
            PRM(15) = New SqlParameter("@Phone", SqlDbType.NVarChar, 450) With {.Value = Phone}

            RUN_EXUTE_PRO("TB_Users_Insert", PRM)
            If PRM(11).Value = 0 Then
                ErrorMessage(FRMADDUSER, "رسالة تنبية", PRM(12).Value)
            End If
            'If FRMADDUSER.IsUpdate = False And FRMADDUSER.IsEmpORUser.SelectedIndex = 0 Then
            If FRMADDUSER.IsUpdate = False Then
                Dim mms As String = " *شركة الرحالة للصرافة*" & vbNewLine &
                "اسم المستخدم " & ":" & Space(1) & UNameLog & vbNewLine &
                "كلمة المرور" & ":" & Space(1) & UPass & vbNewLine &
                "لا تشارك البيانات مع أحد"
                WATSAPPMsAG(FRMADDUSER.phone.Text, mms, True)
            End If
        Catch ex As Exception
            ErrorMessage(FRMADDUSER, "رسالة تنبية", ex.Message)
        End Try
    End Sub
    Public Function TB_Users_LOADUSERBASEDONID(USID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@USID", SqlDbType.Int)
        PRM(0).Value = USID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TB_Users_LOADUSERBASEDONID", PRM)
        Return DT
    End Function
End Class
