Imports System.Data.SqlClient

Public Class CLSAccSafeActivity
    Public IsDelivared As Boolean
    Public Sub AccSafeActivity_insert(SafeID As ULong, Debit As Double, Credit As Double, InsertDate As Date, Description As String, ISID As String, TypeID As Integer, OperationTypeID As Integer, AccBranchID As Integer, AccIDFrom As Integer,
                                AccIDTo As Integer, IsConfirmed As Boolean, IsCanceled As Integer, MovementType As String, CurrencyID As Integer, DailyClosed As Boolean, SafeIDDailyClose As Integer)
        Dim prm(16) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
        prm(2) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
        prm(3) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(4) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        prm(5) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(6) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        prm(7) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        prm(8) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
        prm(9) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = AccIDFrom}
        prm(10) = New SqlParameter("@AccIDTo", SqlDbType.Int) With {.Value = AccIDTo}
        prm(11) = New SqlParameter("@IsConfirmed", SqlDbType.Bit) With {.Value = IsConfirmed}
        prm(12) = New SqlParameter("@IsCanceled", SqlDbType.Int) With {.Value = IsCanceled}
        prm(13) = New SqlParameter("@MovementType", SqlDbType.NVarChar, 80) With {.Value = MovementType}
        prm(14) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        prm(15) = New SqlParameter("@DailyClosed", SqlDbType.Bit) With {.Value = DailyClosed}
        prm(16) = New SqlParameter("@SafeIDDailyClose", SqlDbType.Int) With {.Value = SafeIDDailyClose}
        RUN_EXUTE_PRO("AccSafeActivityTb_Insert", prm)
    End Sub
    Public Sub AccSafeActivityTb_InsertSafeTrance(SafeID As ULong, Debit As Double, Credit As Double, InsertDate As Date, ISID As String, AccBranchID As Integer, AccIDFrom As Integer,
                                AccIDTo As Integer, MovementType As String, CurrencyID As Integer, Description As String)
        Dim prm(10) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
        prm(2) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
        prm(3) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(4) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(5) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
        prm(6) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = AccIDFrom}
        prm(7) = New SqlParameter("@AccIDTo", SqlDbType.Int) With {.Value = AccIDTo}
        prm(8) = New SqlParameter("@MovementType", SqlDbType.NVarChar, 80) With {.Value = MovementType}
        prm(9) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        prm(10) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}

        RUN_EXUTE_PRO("AccSafeActivityTb_InsertSafeTrance", prm)
    End Sub
    Public Sub AccSafeActivityTb_DeleteSafeTrance(ISID As String, IsUpdate As Boolean)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(1) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("AccSafeActivityTb_DeletSafeTrance", prm)
    End Sub
    Public Sub AccSafeActivityTb_UPDATEEXISTRECORD(SafeID As ULong, InsertDate As Date, Description As String, ISID As String, RBShare As Decimal, DBVal As Decimal, OverallVal As Decimal, ExVal As Decimal, MainBrShare As Decimal)
        Dim prm(10) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(2) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        prm(3) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(4) = New SqlParameter("@RBShare", SqlDbType.Decimal) With {.Value = RBShare}
        prm(5) = New SqlParameter("@DBVal", SqlDbType.Decimal) With {.Value = DBVal}
        prm(6) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = OverallVal}
        prm(7) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal}
        prm(8) = New SqlParameter("@MainBrShare", SqlDbType.Decimal) With {.Value = MainBrShare}
        prm(9) = New SqlParameter("@SecondPrm", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(10) = New SqlParameter("@FirstPrm", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("AccSafeActivityTb_UPDATEEXISTRECORD", prm)
        If prm(9).Value = 0 Then
            MsgBox(prm(10).Value)
        End If
    End Sub
    Public Sub AccSafeActivityTb_CONFIRMINTERNALTRANS(SafeID As ULong, InsertDate As Date, ISID As String, OverallVal As Decimal, ExVal As Decimal, DeliveryPlace As String, BranchRecievedID As Integer,
                                                      BranchDeliveredID As Integer, ConfirmDate As Date, RecievedName As String, RPhone1 As String, RPhone2 As String)
        Dim prm(11) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(2) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(3) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = OverallVal}
        prm(4) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal}
        prm(5) = New SqlParameter("@DeliveryPlace", SqlDbType.NVarChar, -1) With {.Value = DeliveryPlace}
        prm(6) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID}
        prm(7) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        prm(8) = New SqlParameter("@ConfirmDate", SqlDbType.Date) With {.Value = ConfirmDate}
        prm(9) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = RecievedName}
        prm(10) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, -1) With {.Value = RPhone1}
        prm(11) = New SqlParameter("@RPhone2", SqlDbType.NVarChar, -1) With {.Value = RPhone2}
        RUN_EXUTE_PRO("AccSafeActivityTb_CONFIRMINTERNALTRANS", prm)
    End Sub
    Public Sub AccSafeActivityTb_CancelInternalEx(SafeID As ULong, InsertDate As Date, Description As String, ISID As String, OverallVal As Decimal, ExVal As Decimal, DiscountEx As Decimal, DiscountStatus As Boolean)
        '1
        Dim prm(11) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(2) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        prm(3) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(4) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = OverallVal}
        prm(5) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal}
        prm(6) = New SqlParameter("@SecondPrm", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(7) = New SqlParameter("@FirstPrm", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(8) = New SqlParameter("@DiscountEx", SqlDbType.Decimal) With {.Value = DiscountEx}
        prm(9) = New SqlParameter("@DiscountStatus", SqlDbType.Bit) With {.Value = DiscountStatus}
        prm(10) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("AccSafeActivityTb_CancelInternalEx", prm)
        If prm(10).Value = 0 Then
            ErrorMessage2("رسالة تنبية ", prm(11).Value)
            IsDelivared = True
        Else
            IsDelivared = False
        End If
    End Sub
    Public Sub AccSafeActivityTb_CancelDeliveredInternalEx(SafeID As ULong, InsertDate As Date, Description As String, ISID As String, OverallVal As Decimal, ExVal As Decimal, DiscountEx As Decimal, DiscountStatus As Boolean)
        Dim prm(11) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(2) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        prm(3) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        prm(4) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = OverallVal}
        prm(5) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal}
        prm(6) = New SqlParameter("@SecondPrm", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(7) = New SqlParameter("@FirstPrm", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(8) = New SqlParameter("@DiscountEx", SqlDbType.Decimal) With {.Value = DiscountEx}
        prm(9) = New SqlParameter("@DiscountStatus", SqlDbType.Decimal) With {.Value = DiscountStatus}
        prm(10) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("AccSafeActivityTb_CancelInternalEx", prm)
        If prm(10).Value = 0 Then
            ErrorMessage2("رسالة تنبية ", prm(11).Value)
            IsDelivared = True
        Else
            IsDelivared = False
        End If
    End Sub
End Class
