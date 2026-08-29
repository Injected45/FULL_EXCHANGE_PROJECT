Imports System.Data.SqlClient

Public Class CLSUSERACCESSPROFILE
    Public Sub INSERTTB_AGENTACTIVITY(ProfileID As Integer, ScreenID As Integer, CanShow As Boolean, CanOpen As Boolean, CanAdd As Boolean, CanEdit As Boolean, CanDelete As Boolean, CanPrint As Boolean, IsUpdate As Boolean)
        Dim PRM(10) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfileID}
        PRM(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        PRM(2) = New SqlParameter("@CanShow", SqlDbType.Bit) With {.Value = CanShow}
        PRM(3) = New SqlParameter("@CanOpen", SqlDbType.Bit) With {.Value = CanOpen}
        PRM(4) = New SqlParameter("@CanAdd", SqlDbType.Bit) With {.Value = CanAdd}
        PRM(5) = New SqlParameter("@CanEdit", SqlDbType.Bit) With {.Value = CanEdit}
        PRM(6) = New SqlParameter("@CanDelete", SqlDbType.Bit) With {.Value = CanDelete}
        PRM(7) = New SqlParameter("@CanPrint", SqlDbType.Bit) With {.Value = CanPrint}
        PRM(8) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("UserAccessProfileDetails_Insert", PRM)
    End Sub
End Class
