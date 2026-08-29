Imports System.Data.SqlClient

Public Class CLSEDISCOUNTTYPE
    Public Function SERACH_DiscountTypeTb(Code As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("DiscountTypeTb_Select", PRM)
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB__DiscountTypeTb(DISNAME As String)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@DISNAME", SqlDbType.NVarChar, 50) With {.Value = DISNAME}

        RUN_EXUTE_PRO("DiscountTypeTb_Insert", PRM)

    End Sub

    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_DiscountTypeTb(ID As Integer, DISNAME As String)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@DISNAME", SqlDbType.NVarChar, 50) With {.Value = DISNAME}

        RUN_EXUTE_PRO("DiscountTypeTb_Update", PRM)
    End Sub
End Class
