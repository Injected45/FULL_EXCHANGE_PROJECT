Imports System.Data.SqlClient

Public Class CLSSCREENACCESSPROFILE
    Public Sub RibbonPermission_Insert(ProfileID As Integer, RPID As Integer, CanShow As Boolean, IsUpdate As Boolean)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfileID}
        PRM(1) = New SqlParameter("@RPID", SqlDbType.Int) With {.Value = RPID}
        PRM(2) = New SqlParameter("@CanShow", SqlDbType.Bit) With {.Value = CanShow}
        PRM(3) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("RibbonPermission_Insert", PRM)
    End Sub
    Public Function RibbonPermission_Select(ProfileID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileID", SqlDbType.Int)
        PRM(0).Value = ProfileID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("RibbonPermission_Select", PRM)
        Return DT
    End Function
    Public Function RibbonPermission_ScreenDetails(ProfileID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileID", SqlDbType.Int)
        PRM(0).Value = ProfileID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("UserAccessProfileDetails_Select", PRM)
        Return DT
    End Function
End Class
