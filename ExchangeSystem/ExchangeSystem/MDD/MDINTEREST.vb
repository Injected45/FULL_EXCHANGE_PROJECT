Imports System.Data.SqlClient

Module MDINTEREST
    Public RBShare, DBShare, MainBRShare, AgentShare, MediumShare, RestShare As Decimal
    Public Function InterestDIS(RBRANCH As Integer, DBRANCH As Integer, INPUTVAL As Decimal) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@RBranchID", SqlDbType.Int) With {.Value = RBRANCH}
        PRM(1) = New SqlParameter("@DBranchID", SqlDbType.Int) With {.Value = DBRANCH}
        PRM(2) = New SqlParameter("@INPUTVAL", SqlDbType.Int) With {.Value = INPUTVAL}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CoBranch_GetBenefitRates", PRM)
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("RBShare") Is "NULL" Then
                RBShare = 0.000
            Else
                RBShare = Convert.ToDecimal(DT.Rows(0)("RBShare"))
            End If
            If DT.Rows(0)("DBShare") Is "NULL" Then
                RBShare = 0.000
            Else
                DBShare = DT.Rows(0)("DBShare")
            End If
            If RBRANCH And DBRANCH <> MAINBID Then
                If DT.Rows(0)("RestShare") Is "NULL" Then
                    RestShare = 0.000
                Else
                    RestShare = DT.Rows(0)("RestShare")
                End If
            End If
        End If
            Return DT
    End Function
End Module
