Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Filtering

Public Class CLSCoBranch
    Public Function Max_CoBranch()
        Dim Number As Integer
        Try
            Dim DT As DataTable = RUN_QUARY_TXT("Select Max(ID) From CoBranch ")
            Number = DT.Rows(0)(0)
        Catch ex As Exception
            Number = 0
        End Try
        Return Number
    End Function
    Public Function CHECK_BRANCH_NAME(ByVal NAME As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BName", SqlDbType.NVarChar, 250) With {.Value = NAME.Trim
        }
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("CoBranch_SEARCH_BRANCH_BYNAME", PRM)
        End If
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB_PROFILE_CoBranch(Code As String, BName As String, CountryID As Integer, CityID As Integer, BAddress As String, Mobile1 As String, Mobile2 As String,
                                         Notes As String, IsActive As Boolean, IsMain As Boolean, BrnanchType As Int16, IsPartner As Boolean,
                                         IsUpdate As Boolean, BRNID As Integer,
                                         OwnerName As String, GroupName As String, IDGroup As String)
        Try
            Dim PRM(18) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(1) = New SqlParameter("@BName", SqlDbType.NVarChar, -1) With {.Value = BName}
            PRM(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
            PRM(3) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = CityID}
            PRM(4) = New SqlParameter("@BAddress", SqlDbType.NVarChar, -1) With {.Value = BAddress}
            PRM(5) = New SqlParameter("@Mobile1", SqlDbType.NVarChar, -1) With {.Value = Mobile1}
            PRM(6) = New SqlParameter("@Mobile2", SqlDbType.NVarChar, -1) With {.Value = Mobile2}
            PRM(7) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(8) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
            PRM(9) = New SqlParameter("@IsMain", SqlDbType.Bit) With {.Value = IsMain}
            PRM(10) = New SqlParameter("@BranchType", SqlDbType.TinyInt) With {.Value = BrnanchType}
            PRM(11) = New SqlParameter("@IsPartner", SqlDbType.Bit) With {.Value = IsPartner}
            PRM(12) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(13) = New SqlParameter("@BID", SqlDbType.Int) With {.Value = BRNID}
            PRM(14) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(15) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(16) = New SqlParameter("@OwnerName", SqlDbType.NVarChar, -1) With {.Value = OwnerName}
            PRM(17) = New SqlParameter("@GroupName", SqlDbType.NVarChar, -1) With {.Value = GroupName}
            PRM(18) = New SqlParameter("@IDGroup", SqlDbType.NVarChar, -1) With {.Value = IDGroup}
            RUN_EXUTE_PRO("CoBranch_Insert", PRM)
            FrmCoBranch.msgST = PRM(14).Value
            If PRM(14).Value = 0 Then
                MessageBox.Show(PRM(15).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Else
                'Dim AccCode As ULong = GET_codefor_Acount_SaenFroWtsaap(BRNID)

                If FrmCoBranch.BranchType.SelectedIndex = 2 Then
                    Dim mms As String = "مرحباً " & ":" & Space(1) & "*[" & FrmCoBranch.BName.Text.Trim & "]*" & vbNewLine & "يسرنا إعلامك بفتح حساب لك في شركة الرحالة للصرافة " & vbNewLine & "برقم" & Space(1) & ":" & Space(1) & Code & vbNewLine & "للإستفسار: 0914200648 " & vbNewLine & "*شكرًا لثقتكم بنا*" & vbNewLine & "*فريق شركة الرحالة للصرافة*"
                    WATSAPPMsAG(FrmCoBranch.Mobile1.Text.Trim, mms, True)
                End If

                FrmCoBranch.BtnNew.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تحذير وجود مشكلة في نظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class
