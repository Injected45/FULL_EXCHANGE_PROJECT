Imports System.Data.SqlClient

Public Class CLSSAFE
    Public Function Convert(ByVal e As Integer) As String
        Select Case e
            Case 0
                Return "دائن"
            Case 1
                Return "مدين"
            Case 2
                Return "كلاهما"
        End Select
        Return e
    End Function
    Public Function SERACH_SAFE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SAFETB_Search", PRM)
        Return DT
    End Function
    Public Function SELECTALLTB_SAFE() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECTALLTB_PROFILE_COMPANY")
        Return DT
    End Function
    Public Function CHECK_SAFE_NAME(ByVal SafeName As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@SafeName", SqlDbType.NVarChar, 250)
        PRM(0).Value = SafeName.Trim
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("SAFE_SEARCH_BYNAME", PRM)
        End If
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB__Store(Code As String, SafeName As String, SafeType As Integer, IsActive As Boolean)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 500) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeName", SqlDbType.NVarChar, 500) With {.Value = SafeName}
        PRM(2) = New SqlParameter("@SafeType", SqlDbType.Int) With {.Value = SafeType}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        RUN_EXUTE_PRO("SAFETB_Insert", PRM)

    End Sub

    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_SAFE(Code As String, SafeName As String, SafeType As Integer, IsActive As Boolean)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeName", SqlDbType.NVarChar, 250) With {.Value = SafeName}
        PRM(2) = New SqlParameter("@SafeType", SqlDbType.Int) With {.Value = SafeType}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        RUN_EXUTE_PRO("SAFETB_UpdateById", PRM)
    End Sub
End Class
