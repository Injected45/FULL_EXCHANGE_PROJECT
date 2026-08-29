Imports System.Data.SqlClient
Imports DevExpress.XtraPrinting.Shape.Native

Public Class CLSINTERNALTRANSFER
    Public Function SERACH_INTERNALTRANSFER(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_SearchByCode", PRM)
        Return DT
    End Function
    Public Function CHECK_EMP_NAME(ByVal EMPNAME As String, BranchID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, 250)
        PRM(0).Value = EMPNAME.Trim
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(1).Value = BranchID
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("EMPLOYEETB_SEARCH_BYNAME", PRM)
        End If
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB__INTERNALTRANSFER(Code As String, InsertDate As Date, SenderName As String, SPhone1 As String, SPhone2 As String, SenderIDNo As String, RecievedName As String, RPhone1 As String, RPhone2 As String, RecievedIDNo As String,
                                          RecievedCurrencyID As Integer, DeliveredCurrencyID As Integer, OverallVal As Double, ExVal As Double, SafeRecievedID As Integer, DeliveryPlace As String, BranchRecievedID As Integer,
                                          BranchDeliveredID As Integer, DSST As Integer, IsDelivered As Boolean, IsConfirmed As Integer, Notes As String)
        Dim PRM(21) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@SenderName", SqlDbType.NVarChar, -1) With {.Value = SenderName}
        PRM(3) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, -1) With {.Value = SPhone1}
        PRM(4) = New SqlParameter("@SPhone2", SqlDbType.NVarChar, -1) With {.Value = SPhone2}
        PRM(5) = New SqlParameter("@SenderIDNo", SqlDbType.NVarChar, -1) With {.Value = SenderIDNo}
        PRM(6) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = RecievedName}
        PRM(7) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, -1) With {.Value = RPhone1}
        PRM(8) = New SqlParameter("@RPhone2", SqlDbType.NVarChar, -1) With {.Value = RPhone2}
        PRM(9) = New SqlParameter("@RecievedIDNo", SqlDbType.NVarChar, -1) With {.Value = RecievedIDNo}
        PRM(10) = New SqlParameter("@RecievedCurrencyID", SqlDbType.Int) With {.Value = RecievedCurrencyID}
        PRM(11) = New SqlParameter("@DeliveredCurrencyID", SqlDbType.Int) With {.Value = DeliveredCurrencyID}
        PRM(12) = New SqlParameter("@OverallVal", SqlDbType.Float) With {.Value = OverallVal}
        PRM(13) = New SqlParameter("@ExVal", SqlDbType.Float) With {.Value = ExVal}
        PRM(14) = New SqlParameter("@SafeRecievedID", SqlDbType.Int) With {.Value = SafeRecievedID}
        PRM(15) = New SqlParameter("@DeliveryPlace", SqlDbType.NVarChar, -1) With {.Value = DeliveryPlace}
        PRM(16) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID}
        PRM(17) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(18) = New SqlParameter("@DSST", SqlDbType.Int) With {.Value = DSST}
        PRM(19) = New SqlParameter("@IsDelivered", SqlDbType.Bit) With {.Value = IsDelivered}
        PRM(20) = New SqlParameter("@IsConfirmed", SqlDbType.Int) With {.Value = IsConfirmed}
        PRM(21) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        RUN_EXUTE_PRO("InternalEx_Insert", PRM)

    End Sub
    Public Sub INSERTTB__INTERNALTRANSFER1(Code As String, InsertDate As Date, SenderName As String, SPhone1 As String, SPhone2 As String, SenderIDNo As String, RecievedName As String, RPhone1 As String, RPhone2 As String,
                                           RecievedIDNo As String, RecievedCurrencyID As Integer, DeliveredCurrencyID As Integer, OverallVal As Double, ExVal As Double, SafeRecievedID As Integer, DeliveryPlace As Integer,
                                           BranchRecievedID As Integer, BranchDeliveredID As Integer, DSST As Integer, IsDelivered As Boolean, IsConfirmed As Integer, Notes As String, dt As DataTable, Debit As Double,
                                           Credit As Double, Description As String, BranchRecievedName As String, CurrencyID As Integer, AgentCheck As Boolean, TransType As Int32, IsAccFrom As Int32,
                                           AccFID As ULong, EMPCUSTSELECT As Integer, IsAccTo As Int32,
                                           TransAccIDTo As ULong, EMPTOSELECT As Integer, IsCash As Int32, BBRANCHACCID As ULong, BBRANCHID As Integer, EXTRVAL As Decimal, ServiceType As Integer)
        Dim PRM(43) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@SenderName", SqlDbType.NVarChar, -1) With {.Value = SenderName}
        PRM(3) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, -1) With {.Value = SPhone1}
        PRM(4) = New SqlParameter("@SPhone2", SqlDbType.NVarChar, -1) With {.Value = SPhone2}
        PRM(5) = New SqlParameter("@SenderIDNo", SqlDbType.NVarChar, -1) With {.Value = SenderIDNo}
        PRM(6) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = RecievedName}
        PRM(7) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, -1) With {.Value = RPhone1}
        PRM(8) = New SqlParameter("@RPhone2", SqlDbType.NVarChar, -1) With {.Value = RPhone2}
        PRM(9) = New SqlParameter("@RecievedIDNo", SqlDbType.NVarChar, -1) With {.Value = RecievedIDNo}
        PRM(10) = New SqlParameter("@RecievedCurrencyID", SqlDbType.Int) With {.Value = RecievedCurrencyID}
        PRM(11) = New SqlParameter("@DeliveredCurrencyID", SqlDbType.Int) With {.Value = DeliveredCurrencyID}
        PRM(12) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = OverallVal}
        PRM(13) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal}
        PRM(14) = New SqlParameter("@SafeRecievedID", SqlDbType.Int) With {.Value = SafeRecievedID}
        PRM(15) = New SqlParameter("@DeliveryPlace", SqlDbType.Int) With {.Value = DeliveryPlace}
        PRM(16) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID}
        PRM(17) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(18) = New SqlParameter("@DSST", SqlDbType.Int) With {.Value = DSST}
        PRM(19) = New SqlParameter("@IsDelivered", SqlDbType.Bit) With {.Value = IsDelivered}
        PRM(20) = New SqlParameter("@IsConfirmed", SqlDbType.Int) With {.Value = IsConfirmed}
        PRM(21) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(22) = New SqlParameter("@IEXVAL", SqlDbType.Structured) With {.Value = dt}
        PRM(23) = New SqlParameter("@msgIN", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(24) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
        PRM(25) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
        PRM(26) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(27) = New SqlParameter("@BranchRecievedName", SqlDbType.NVarChar, 50) With {.Value = BranchRecievedName}
        PRM(28) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(29) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(30) = New SqlParameter("@IDCODE", SqlDbType.Int) With {.Value = FRMINTERNALTRANSFER.IDCode}
        PRM(31) = New SqlParameter("@AgentCheck", SqlDbType.Bit) With {.Value = AgentCheck}
        PRM(32) = New SqlParameter("@TransType", SqlDbType.TinyInt) With {.Value = TransType}
        PRM(33) = New SqlParameter("@IsAccFrom", SqlDbType.TinyInt) With {.Value = IsAccFrom}
        PRM(34) = New SqlParameter("@AccFID", SqlDbType.BigInt) With {.Value = AccFID}
        PRM(35) = New SqlParameter("@EMPCUSTSELECT", SqlDbType.Int) With {.Value = EMPCUSTSELECT}
        PRM(36) = New SqlParameter("@IsAccTo", SqlDbType.TinyInt) With {.Value = IsAccTo}
        PRM(37) = New SqlParameter("@TransAccIDTo", SqlDbType.BigInt) With {.Value = TransAccIDTo}
        PRM(38) = New SqlParameter("@EMPTOSELECT", SqlDbType.Int) With {.Value = EMPTOSELECT}
        PRM(39) = New SqlParameter("@IsCash", SqlDbType.TinyInt) With {.Value = IsCash}
        PRM(40) = New SqlParameter("@BBRANCHACCID", SqlDbType.BigInt) With {.Value = BBRANCHACCID}
        PRM(41) = New SqlParameter("@BBRANCHID", SqlDbType.Int) With {.Value = BBRANCHID}
        PRM(42) = New SqlParameter("@EXTRVAL", SqlDbType.Decimal) With {.Value = EXTRVAL}
        PRM(43) = New SqlParameter("@ServiceType", SqlDbType.Int) With {.Value = ServiceType}
        RUN_EXUTE_PRO("InternalEx_Insert1", PRM)
        If PRM(23).Value = 0 Then
            ErrorMessage(FRMINTERNALTRANSFER, "رسالة تنبيه", PRM(29).Value)
            InternalEx_MaxID(1, BID, UserID, COUNTRYNID, CITYID)
            FRMINTERNALTRANSFER.MsgStatus = 0
        Else

            FRMINTERNALTRANSFER.MsgStatus = 1
            FRMINTERNALTRANSFER.Print()
            FRMINTERNALTRANSFER.NEWRECORD()
        End If
    End Sub

    Public Sub UPDATE__INTERNALTRANSFERCUREENTRECORD(Code As String, InsertDate As Date, SenderName As String, SPhone1 As String, SPhone2 As String, SenderIDNo As String, RecievedName As String, RPhone1 As String, RPhone2 As String, RecievedIDNo As String,
                                          RecievedCurrencyID As Integer, DeliveredCurrencyID As Integer, OverallVal As Double, ExVal As Double, SafeRecievedID As Integer, DeliveryPlace As String, BranchRecievedID As Integer,
                                          BranchDeliveredID As Integer, DSST As Integer, IsDelivered As Boolean, IsConfirmed As Integer, Notes As String)
        Dim PRM(21) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@SenderName", SqlDbType.NVarChar, -1) With {.Value = SenderName}
        PRM(3) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, -1) With {.Value = SPhone1}
        PRM(4) = New SqlParameter("@SPhone2", SqlDbType.NVarChar, -1) With {.Value = SPhone2}
        PRM(5) = New SqlParameter("@SenderIDNo", SqlDbType.NVarChar, -1) With {.Value = SenderIDNo}
        PRM(6) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = RecievedName}
        PRM(7) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, -1) With {.Value = RPhone1}
        PRM(8) = New SqlParameter("@RPhone2", SqlDbType.NVarChar, -1) With {.Value = RPhone2}
        PRM(9) = New SqlParameter("@RecievedIDNo", SqlDbType.NVarChar, -1) With {.Value = RecievedIDNo}
        PRM(10) = New SqlParameter("@RecievedCurrencyID", SqlDbType.Int) With {.Value = RecievedCurrencyID}
        PRM(11) = New SqlParameter("@DeliveredCurrencyID", SqlDbType.Int) With {.Value = DeliveredCurrencyID}
        PRM(12) = New SqlParameter("@OverallVal", SqlDbType.Float) With {.Value = OverallVal}
        PRM(13) = New SqlParameter("@ExVal", SqlDbType.Float) With {.Value = ExVal}
        PRM(14) = New SqlParameter("@SafeRecievedID", SqlDbType.Int) With {.Value = SafeRecievedID}
        PRM(15) = New SqlParameter("@DeliveryPlace", SqlDbType.NVarChar, -1) With {.Value = DeliveryPlace}
        PRM(16) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID}
        PRM(17) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(18) = New SqlParameter("@DSST", SqlDbType.Int) With {.Value = DSST}
        PRM(19) = New SqlParameter("@IsDelivered", SqlDbType.Bit) With {.Value = IsDelivered}
        PRM(20) = New SqlParameter("@IsConfirmed", SqlDbType.Int) With {.Value = IsConfirmed}
        PRM(21) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        RUN_EXUTE_PRO("InternalEx_UpdateCurrentRecord", PRM)
    End Sub
    Public Sub AccSafeActivityTb_UPDATEEXISTRECORD(SafeID As ULong, InsertDate As Date, Description As String, ISID As String, RBShare As Decimal, DBVal As Decimal, OverallVal As Decimal, ExVal As Decimal,
                                                   MainBrShare As Decimal)
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
    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_INTERNALTRANSFER(Code As String, SafeDeliveredID As Integer, RecievedDate As Date, Notes As String,
                                         SafeID As ULong, InsertDate As Date, Description As String, ISID As String, OverallVal As Decimal, ExVal As Decimal)
        Dim PRM(11) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeDeliveredID", SqlDbType.Int) With {.Value = SafeDeliveredID}
        PRM(2) = New SqlParameter("@RecievedDate", SqlDbType.Date) With {.Value = RecievedDate}
        PRM(3) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(4) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(5) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(6) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(7) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(8) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = OverallVal}
        PRM(9) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal}
        PRM(10) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

        RUN_EXUTE_PRO("InternalEx_UpdateExistRecord", PRM)
        If PRM(10).Value = 0 Then
            FRMINTERNALTRANSFER.SenForMassgWtsa = PRM(10).Value
            ErrorMessage2("رسالة تنبية ", PRM(11).Value)
        Else
            FRMINTERNALTRANSFER.SenForMassgWtsa = PRM(10).Value
        End If
    End Sub

    Public Sub UPDATETB_INTERNALTRANSFER1(Code As String, BranchDeliveredID As Integer, SafeDeliveredID As Integer, RecievedDate As Date, Notes As String)
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeDeliveredID", SqlDbType.Int) With {.Value = SafeDeliveredID}
        PRM(2) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(3) = New SqlParameter("@RecievedDate", SqlDbType.Date) With {.Value = RecievedDate}
        PRM(4) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}

        RUN_EXUTE_PRO("InternalEx_UpdateExistRecord1", PRM)
    End Sub
    Public Sub INSERTTB__InternalExValues(ISID As String, InsertDate As Date, OverallVal As Double, TransCharge As Double, RecievedSafeID As Integer, RecievedBranchID As Integer, DeliveredBranchID As Integer)
        Dim PRM(6) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, (250)) With {.Value = ISID}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@OverallVal", SqlDbType.Float) With {.Value = OverallVal}
        PRM(3) = New SqlParameter("@TransCharge", SqlDbType.Float) With {.Value = TransCharge}
        PRM(4) = New SqlParameter("@RecievedSafeID", SqlDbType.Int) With {.Value = RecievedSafeID}
        PRM(5) = New SqlParameter("@RecievedBranchID ", SqlDbType.Int) With {.Value = RecievedBranchID}
        PRM(6) = New SqlParameter("@DeliveredBranchID ", SqlDbType.Int) With {.Value = DeliveredBranchID}
        RUN_EXUTE_PRO("InternalExValues_Insert", PRM)
    End Sub
    Public Sub UPDATECURRENTRECROD__InternalExValues(ISID As String, InsertDate As Date, OverallVal As Double, TransCharge As Double, RecievedSafeID As Integer, RecievedBranchID As Integer, DeliveredBranchID As Integer)
        Dim PRM(6) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, (250)) With {.Value = ISID}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@OverallVal", SqlDbType.Float) With {.Value = OverallVal}
        PRM(3) = New SqlParameter("@TransCharge", SqlDbType.Float) With {.Value = TransCharge}
        PRM(4) = New SqlParameter("@RecievedSafeID", SqlDbType.Int) With {.Value = RecievedSafeID}
        PRM(5) = New SqlParameter("@RecievedBranchID", SqlDbType.Int) With {.Value = RecievedBranchID}
        PRM(6) = New SqlParameter("@DeliveredBranchID ", SqlDbType.Int) With {.Value = DeliveredBranchID}
        RUN_EXUTE_PRO("InternalExValues_UPDATECURRNETRECORD", PRM)
    End Sub
    Public Sub UPDATEEXISTRECROD__InternalExValues(ISID As String, DeliveredSafeID As Integer, DeliveredBranchID As Integer)
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, (250)) With {.Value = ISID}
        PRM(1) = New SqlParameter("@DeliveredSafeID", SqlDbType.Int) With {.Value = DeliveredSafeID}
        PRM(2) = New SqlParameter("@DeliveredBranchID", SqlDbType.Int) With {.Value = DeliveredBranchID}
        RUN_EXUTE_PRO("InternalExValues_UPDATEEXISTRECORD", PRM)
    End Sub
    Public Sub InternalEx_MaxID(typID As Integer, BranchID As Integer, USRID As Integer, CountryID As Integer, CityID As Integer)
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@typID", SqlDbType.Int) With {.Value = typID}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(2) = New SqlParameter("@USRID", SqlDbType.Int) With {.Value = USRID}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
        PRM(4) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = CityID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("InternalEx_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMINTERNALTRANSFER.CodeID.Text = dt.Rows(0)("Code")
            FRMINTERNALTRANSFER.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub UPDATETBFORAGENTDELIVERED_INTERNALTRANSFER(BranchDeliveredID As Integer, Code As String)
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(1) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(2) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
        RUN_EXUTE_PRO("AccSafeActivityTb_UPDATEDBRANCHID", PRM)
    End Sub
    Public Sub AccSafeActivityTb_UPDATECHANGEDEBRANCHID(BranchDeliveredID As Integer, Code As String, BranchRecievedID As Integer, SafeID As Integer)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@NewBranchDeliverdID", SqlDbType.Int) With {.Value = BranchDeliveredID}
        PRM(1) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(2) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID}
        PRM(3) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        RUN_EXUTE_PRO("AccSafeActivityTb_UPDATECHANGEDEBRANCHID", PRM)
    End Sub
    Public Sub AccSafeActivityTb_UPDATECHANGEDEEXVAl(NewEXVal As Decimal, Code As String)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@NewEXTRVAL", SqlDbType.Decimal) With {.Value = NewEXVal}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        RUN_EXUTE_PRO("AccSafeActivityTb_UPDATECHANGEDEEXVAl", PRM)
    End Sub
    Public Sub InternalEx_CONFIRMINTERNALTRANS(SafeID As ULong, InsertDate As Date, ISID As String, OverallVal As Decimal, ExVal As Decimal, DeliveryPlace As String, BranchRecievedID As Integer,
                                                     BranchDeliveredID As Integer, ConfirmDate As Date, RecievedName As String, RPhone1 As String, RPhone2 As String, ISHandallEX As Boolean, HandallExVal As Decimal, HandallExVal2 As Decimal)
        Dim prm(14) As SqlParameter
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
        prm(12) = New SqlParameter("@ISHandallEX", SqlDbType.Bit) With {.Value = ISHandallEX}
        prm(13) = New SqlParameter("@HandallExVal", SqlDbType.Decimal) With {.Value = HandallExVal}
        prm(14) = New SqlParameter("@HandallExVal2", SqlDbType.Decimal) With {.Value = HandallExVal2}
        RUN_EXUTE_PRO("InternalEx_CONFIRMUpdate", prm)
    End Sub
End Class
