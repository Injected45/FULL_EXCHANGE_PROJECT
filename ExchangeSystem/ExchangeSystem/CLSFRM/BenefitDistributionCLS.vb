Imports System.Data.SqlClient

Public Class BenefitDistributionCLS
    Public Sub BenefitDistribution_InsertIfMainReceived(ISID As String, InsertDate As Date, BranchRecievedID As Integer, BranchDeliveredID As Integer, FirstVal As Double, SecondVal As Double, ThirdVal As Double, ISIDType As Integer,
                                                        SafeID As Integer, MBID As Integer, RType As Integer, DType As Integer)
        Dim PRM(11) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@RBID", SqlDbType.Int) With {.Value = BranchRecievedID}
        PRM(3) = New SqlParameter("@DBBID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(4) = New SqlParameter("@RBVal", SqlDbType.Decimal) With {.Value = FirstVal}
        PRM(5) = New SqlParameter("@DBVal", SqlDbType.Decimal) With {.Value = SecondVal}
        PRM(6) = New SqlParameter("@MainBVal", SqlDbType.Decimal) With {.Value = ThirdVal}
        PRM(7) = New SqlParameter("@ISIDType", SqlDbType.Int) With {.Value = ISIDType}
        PRM(8) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(9) = New SqlParameter("@MainBID", SqlDbType.Int) With {.Value = MBID}
        PRM(10) = New SqlParameter("@BRType", SqlDbType.Int) With {.Value = RType}
        PRM(11) = New SqlParameter("@DBRType", SqlDbType.Int) With {.Value = DType}
        RUN_EXUTE_PRO("BenefitDistribution_INSERT", PRM)
    End Sub

End Class
