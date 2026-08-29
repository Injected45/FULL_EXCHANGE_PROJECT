Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.XtraReports.UI

Public Class FRMINTERNALTRANSFER
    Dim inscls As New CLSINTERNALTRANSFER
    Public ConfirmType, RBRTYPE, DBRTYPE, ConfirmTRAS, CitySelected, BranchDID, RecBID, TransType, AccFromOrTo, AccountType, ectypeto, BBRID, SerID, CountID, CTID, MsgStatus As Integer
    Public AccTrans As Integer = 0
    Public IsAccFrom As Int32 = 0
    Public IsAccTo As Int32 = 0
    Dim DT As New DataTable
    Public IDCode, AccFrom, TransAccIDTo, BBRANCHACCID As ULong
    Public IsUpdate, IsConfirm, IsSelectAcc, CanDebit, ISHandallEX As Boolean
    Public MovementType, brR, brD As String
    Public ServiceRate, HandallExVal, HandallExVal2 As Decimal
    Public SenForMassgWtsa As Int16
    Dim report As New RPTRecieveInternalEx2


#Region "رسائل الوتساب"
    Public Sub sender_for_Watsaap_seindFromSendreClint()
        If ConfirmType = 0 Then


            BuildMessageForStandardTransferw("", CodeID.Text, OverallVal.Text,
                                                   RecievedName.Text, AgentCityID.Text, BID)
        ElseIf ConfirmType = 1 Then

            BuildIncomingTransferMessage(CodeID.Text, SenderName.Text, OverallVal.Text, BranchDeliveredID.Text, BranchDeliveredID.EditValue, IsAccTo)


            If GetLKPColumnVal(BranchRecievedID, "BranchType") <> 3 And GetLKPColumnVal(BranchDeliveredID, "BranchType") = 3 Then
                sEnFRoRElode(CodeID.Text, BranchDeliveredID.EditValue, 0, 1, HandallExVal, HandallExVal2)
            End If
            If GetLKPColumnVal(BranchRecievedID, "BranchType") = 3 And GetLKPColumnVal(BranchDeliveredID, "BranchType") = 3 Then
                sEnFRoRElode(CodeID.Text, BranchDeliveredID.EditValue, 1, 1, HandallExVal, HandallExVal2)
            End If
        ElseIf ConfirmType = 2 Then
            BuildDeliveredTransferMessage(CodeID.Text, RecievedName.Text, BranchDeliveredID.EditValue)

        ElseIf ConfirmType = 5 And GetLKPColumnVal(BranchRecievedID, "BranchType") = 3 Then
            sEnFRoCancle(CodeID.Text, BranchRecievedID.EditValue)
        End If




    End Sub
#End Region


#Region "التحميل للأدوات"
    Private Sub FRMINTERNALTRANSFER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ConfirmType = 0 Then
            CHECKBUTTONS()
            BtnNew.PerformClick()
            LOADDELIVEREDBRANCH()
        End If
    End Sub

    '' تحميل الخدمات الالكترونية
    Sub LOADSERCICETYPE(brID As Integer)
        ServiceID.Properties.DataSource = Nothing
        ServiceID.EditValue = -1
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = brID}
        LoadToControlar(ServiceID, "BBranchTb_LoadBasedOnServices", "BranchName", "ID", PR)
    End Sub
    '' تحميل المصارف
    Sub LOADBBranch()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryId", SqlDbType.Int) With {.Value = COUNTRYNID}
        LoadToControlar(BBranchID, "BanksTb_LOADTOLKP_BasedONCountryID", "BankName", "BNKID", PRM)
    End Sub
    '' تحميل المدن
    Sub LOADAGENTCITY(br As Integer)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = br}
        LoadToControlar(AgentCityID, "CitiesTb_LOADFORAGENTTRANSNOTEXIST", "CityName", "ID", PR)
    End Sub
    '' تحميل العملة
    Sub LOADRECURRENCYLDINAR()
        LoadToControlar(RecievedCurrencyID, "CurrencyMainTb_LOADTOLKP_Dl", "CuName", "ID", Nothing)
        LoadToControlar(DeliveredCurrencyID, "CurrencyMainTb_LOADTOLKP_Dl", "CuName", "ID", Nothing)
    End Sub
    '' تحميل الفرع الراسل
    Sub LOADBRNACH()
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        PR(0).Value = COUNTRYNID
        PR(1) = New SqlParameter("@BID", SqlDbType.Int)
        PR(1).Value = BID
        LoadToControlar(BranchRecievedID, "CoBranches_LOADTOGLKPWITHCOUNTRYAndBID", "BName", "DBRID", PR)
    End Sub
    '' تحميل الفرع المسلم
    Sub LOADDELIVEREDBRANCH()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchId", SqlDbType.Int)
        PRM(0).Value = BranchRecievedID.EditValue
        LoadToControlar(BranchDeliveredID, "CoBranches_LoadForBranchNotMain", "BName", "DBRID", PRM)
    End Sub

#End Region

    Public Sub SendTypeTB_Roll_luckbedit(ScreenID As ULong)
        Try
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
            prm(1) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = UserID}
            dt = RUN_QUARY_PRO("SendTypeTB_Roll_luckbedit", prm)
            If dt.Rows.Count > 0 Then
                Dim prm1(1) As SqlParameter
                prm1(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
                prm1(1) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = UserID}
                LoadToControlar(SendType, "SendTypeTB_Roll_luckbedit", "SName", "SID", prm1)
                HideAllColumnsExceptDisplay(SendType)
                SendType.Enabled = True
            Else
                LoadToControlar(SendType, "SendTypeTB_LoadToLKP", "SName", "SID", Nothing)
                HideAllColumnsExceptDisplay(SendType)
                SendType.EditValue = 0
                SendType.Enabled = False
            End If
        Catch ex As Exception
            ErrorMessage(Me, "ErrorMessage ex ", ex.Message)
        End Try
    End Sub
#Region "الحفظ والتسليم والإعتماد وإلخ"

    '' كود تنظيف الشاشة
    Sub NEWRECORD()
        New_Controlrs(Me)
        Enable_Controls(Me, True)
        If ConfirmType = 0 Then
            BranchRecievedID.EditValue = BID
            If BID = MAINBID Then
                BranchRecievedID.Enabled = True
            Else
                BranchRecievedID.Enabled = False
            End If
        End If
        'Dim PR(0) As SqlParameter
        'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        'PR(0).Value = BID
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_PRO("TransCancelRequestTb_LOADTODVG", PR)
        'If DT.Rows.Count > 0 Then
        '    MessageBox.Show(Me, "لديك طلب إلغاء حوالة يرجى الإطلاع عليها واتمام إجرائها أولاً", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Me.Close()
        'End If
        SendTypeTB_Roll_luckbedit(14)
        LOADBBranch()
        ServiceVal.EditValue = 0.000
        ServiceTotalVal.EditValue = 0.000
        BBranchID.EditValue = -1
        ServiceID.EditValue = -1
        MsgStatus = 0
        CountID = 0
        CTID = 0
        AccountType = 0
        IsUpdate = False
        IsConfirm = False
        ISHandallEX = False
        HandallExVal = 0
        HandallExVal2 = 0
        IsAccFrom = 0
        IsAccTo = 0
        Me.Text = "تحويل داخلي"
        LOADBRNACH()
        LOADRECURRENCYLDINAR()
        LayoutControlItem29.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never ''حالة الحوالة
        BBranchID.Enabled = False
        ServiceID.Enabled = False
        ServiceVal.Enabled = False
        ServiceTotalVal.Enabled = False
        RecievedCurrencyID.EditValue = DefaultCurrency
        DeliveredCurrencyID.EditValue = DefaultCurrency
        AgentCityID.Enabled = False
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnPrint.Enabled = False
        IsDelivered.Checked = False
        RecievedCurrencyID.Enabled = False
        DeliveredCurrencyID.Enabled = False
        InsertDate.EditValue = Date.Now
        SenderName.Select()
        BranchRecievedID.EditValue = BID
        SenderName.Properties.Buttons(0).Enabled = False
        SenderName.ReadOnly = False
        SPhone1.ReadOnly = False
        SPhone2.ReadOnly = False
        SenderIDNo.ReadOnly = False
        SenderName.Text = ""
        SPhone1.Text = ""
        SPhone2.Text = ""
        SenderIDNo.Text = ""
        LOADAGENTCITY(BranchRecievedID.EditValue)
        OutSysCh.Checked = False
        OutSysCh.Enabled = True
        'IsChecked()
        If ConfirmType = 0 Then
            inscls.InternalEx_MaxID(1, BranchRecievedID.EditValue, UserID, COUNTRYNID, CITYID)
        End If
        SendType.EditValue = 0
        RecievType.SelectedIndex = 0
        IsDelivered.Enabled = False
    End Sub


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' كود الحفظ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub INSERTTB__INTERNALTRANSFER1()
        Dim PRM(31) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text}
        PRM(1) = New SqlParameter("@SenderName", SqlDbType.NVarChar, -1) With {.Value = SenderName.Text.Trim}
        PRM(2) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, -1) With {.Value = SPhone1.Text.Trim}
        PRM(3) = New SqlParameter("@SPhone2", SqlDbType.NVarChar, -1) With {.Value = SPhone2.Text.Trim}
        PRM(4) = New SqlParameter("@SenderIDNo", SqlDbType.NVarChar, -1) With {.Value = SenderIDNo.Text.Trim}
        PRM(5) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = RecievedName.Text.Trim}
        PRM(6) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, -1) With {.Value = RPhone1.Text.Trim}
        PRM(7) = New SqlParameter("@RPhone2", SqlDbType.NVarChar, -1) With {.Value = RPhone2.Text.Trim}
        PRM(8) = New SqlParameter("@RecievedIDNo", SqlDbType.NVarChar, -1) With {.Value = RecievedIDNo.Text.Trim}
        PRM(9) = New SqlParameter("@RecievedCurrencyID", SqlDbType.Int) With {.Value = RecievedCurrencyID.EditValue}
        PRM(10) = New SqlParameter("@DeliveredCurrencyID", SqlDbType.Int) With {.Value = DeliveredCurrencyID.EditValue}
        PRM(11) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = SafeToDecimal(OverallVal.EditValue)}
        PRM(12) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = SafeToDecimal(ExVal.EditValue)}
        PRM(13) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
        PRM(14) = New SqlParameter("@DeliveryPlace", SqlDbType.Int) With {.Value = SafeToInt(AgentCityID.EditValue)}
        PRM(15) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = SafeToInt(BranchRecievedID.EditValue)}
        PRM(16) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = SafeToInt(BranchDeliveredID.EditValue)}
        PRM(17) = New SqlParameter("@ConfirmType", SqlDbType.Int) With {.Value = ConfirmType}
        PRM(18) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes.Text.Trim}
        PRM(19) = New SqlParameter("@msgIN", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(20) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(21) = New SqlParameter("@IDCODE", SqlDbType.Int) With {.Value = Me.IDCode}
        PRM(22) = New SqlParameter("@IsAccFrom", SqlDbType.TinyInt) With {.Value = SendType.EditValue}
        PRM(23) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = AccFrom}
        PRM(24) = New SqlParameter("@IsAccTo", SqlDbType.TinyInt) With {.Value = RecievType.SelectedIndex}
        PRM(25) = New SqlParameter("@AccIDTo", SqlDbType.BigInt) With {.Value = TransAccIDTo}
        PRM(26) = New SqlParameter("@BBRANCHID", SqlDbType.Int) With {.Value = If(ServiceID.EditValue > 0, ServiceID.EditValue, 0)}
        PRM(27) = New SqlParameter("@EXTRVAL", SqlDbType.Decimal) With {.Value = ServiceVal.EditValue}
        PRM(28) = New SqlParameter("@ServiceType", SqlDbType.Int) With {.Value = If(ServiceID.EditValue > 0, GetLKPColumnVal(ServiceID, "SID"), 0)}
        PRM(29) = New SqlParameter("@ISHandallEX", SqlDbType.Bit) With {.Value = ISHandallEX}
        PRM(30) = New SqlParameter("@HandallExVal", SqlDbType.Decimal) With {.Value = HandallExVal}
        PRM(31) = New SqlParameter("@HandallExVal2", SqlDbType.Decimal) With {.Value = HandallExVal2}
        RUN_EXUTE_PRO("InternalEx_Insert1", PRM)
        If PRM(19).Value = 0 Or PRM(19).Value = 2 Then
            ErrorMessage(Me, "رسالة تنبيه", PRM(20).Value.ToString)
            If PRM(19).Value = 2 Then
                inscls.InternalEx_MaxID(1, BranchRecievedID.EditValue, UserID, COUNTRYNID, CITYID)
            End If
            MsgStatus = 0
        Else

            sender_for_Watsaap_seindFromSendreClint()
            MsgStatus = 1

        End If
    End Sub
    Public Overrides Sub SetData()
        Try
            If OverallVal.EditValue <= 0.000 Then
                OverallVal.ErrorText = "يرجى إدخال القيمة المرسلة"
                Exit Sub
            End If
            If ConfirmType = 0 Then
                If SenderName.Text.Trim = "" Then
                    SenderName.ErrorText = "هذا الحقل مطلوب"
                    Exit Sub
                End If
                If RecievedName.Text.Trim = "" Then
                    RecievedName.ErrorText = "هذا الحقل مطلوب"
                    Exit Sub
                End If
                If SendType.EditValue = 2 Then
                    If BBranchID.EditValue = -1 Or BBranchID.Text = String.Empty Then
                        BBranchID.ErrorText = "يجب اختيار المصرف"
                        Exit Sub
                    End If
                    If ServiceID.EditValue = -1 Or ServiceID.Text = String.Empty Then
                        ServiceID.ErrorText = "يجب اختيار نوع الخدمة"
                        Exit Sub
                    End If
                End If
                If BranchRecievedID.EditValue = -1 Or BranchRecievedID.Text = String.Empty Then
                    BranchRecievedID.ErrorText = "هذا الحقل مطلوب"
                    Exit Sub
                End If
                If RecievedCurrencyID.EditValue = -1 Or RecievedCurrencyID.Text = String.Empty Then
                    RecievedCurrencyID.ErrorText = ""
                    Exit Sub
                End If
                If OutSysCh.Checked = False Then
                    If BranchDeliveredID.EditValue = -1 Then
                        BranchDeliveredID.ErrorText = "يجب اختيار الوجهة"
                        Exit Sub
                    End If
                ElseIf OutSysCh.Checked = True Then
                    If AgentCityID.EditValue = -1 Then
                        AgentCityID.ErrorText = "يجب اختيار الوجهة"
                        Exit Sub
                    End If
                End If
                If SendType.EditValue = 1 Then
                    FRMCODEPYMENT_em_cu2.lodeDate("حوالة مالية داخلية", SenderName.Text, AccFrom, OverallVal.EditValue, RecievedCurrencyID.Text, SPhone1.Text, 2, "")
                    FRMCODEPYMENT_em_cu2.ShowDialog()
                    If FRMCODEPYMENT_em_cu2.chick = False Then
                        ErrorMessage(Me, "رسالة تنبية", "عذرا رقم الكود خطأ الرجاء اعادة المحاولة")
                        Exit Sub
                        Return
                    End If
                End If
            End If
            If ConfirmType = 1 Or ConfirmType = 11 Then
                If BranchDeliveredID.EditValue = -1 Or BranchDeliveredID.Text = String.Empty Then
                    BranchDeliveredID.ErrorText = "يجب اختيار الوجهة"
                    Exit Sub
                End If
                If BranchDeliveredID.EditValue = BranchRecievedID.EditValue Then
                    ErrorMessage(Me, "رسالة خطأ", "يجب اختيار وجهة مختلفة عن الوجهة المستلمة")
                    Exit Sub
                End If
                If GetLKPColumnVal(BranchRecievedID, "BranchType") <> 3 And GetLKPColumnVal(BranchDeliveredID, "BranchType") = 3 Then
                    ISHandallEX = True
                    ExValSahreByHand.IsAgintToAgint = False
                    ExValSahreByHand.LayoutControlItem2.Text = "عمولة الوكيل"
                    ExValSahreByHand.LayoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                    ExValSahreByHand.NEWRECORD()
                    ExValSahreByHand.LoadExVal()
                    ExValSahreByHand.ShowDialog()

                End If
                If GetLKPColumnVal(BranchRecievedID, "BranchType") = 3 And GetLKPColumnVal(BranchDeliveredID, "BranchType") = 3 Then
                    ISHandallEX = True
                    ExValSahreByHand.IsAgintToAgint = True
                    ExValSahreByHand.LayoutControlItem2.Text = "الوكيل الراسل"
                    ExValSahreByHand.LayoutControlItem6.Text = "الوكيل المسلم"
                    ExValSahreByHand.LayoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    ExValSahreByHand.NEWRECORD()
                    ExValSahreByHand.LoadExVal()
                    ExValSahreByHand.ShowDialog()

                End If
                If GetLKPColumnVal(BranchDeliveredID, "BranchType") <> 3 Then
                    AgentCityID.EditValue = GetLKPColumnVal(BranchDeliveredID, "CityID")
                End If
            End If
            INSERTTB__INTERNALTRANSFER1()
            If MsgStatus = 1 Then
                If ConfirmType = 0 Or ConfirmType = 2 Then
                    Me.Print()
                    Me.NEWRECORD()
                End If
                If ConfirmType <> 0 Then
                    Me.Close()
                End If
            End If

        Catch ex As Exception
            MD_MYSQL.LogAppError("caught in form", Me, ex)
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub IsDelivered_CheckedChanged(sender As Object, e As EventArgs) Handles IsDelivered.CheckedChanged
        Try

            If IsDelivered.Checked = True Then
                'If ConfirmType = 1 Then
                SetData()
                'End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Sub DELIVEREDRECORD()
        If IsLimited = True Then
            If OverallVal.EditValue + ExVal.EditValue > LimitedVal Then
                ErrorMessage(Me, "رسالة خطأ", "القيمة المراد تسليمها أكبر من السقف المسموح به")
                Exit Sub
            End If
        End If
        inscls.UPDATETB_INTERNALTRANSFER(CodeID.Text.Trim, UserID, Date.Now, Notes.Text.Trim, UserID, Date.Now, "", CodeID.Text.Trim, OverallVal.EditValue, ExVal.EditValue)
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' تحميل الحوالة السابقة للاعتماد أو الالغاء او التسليم''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Sub ShowCurrentRecord(x)
        'NEWRECORD()
        Enable_Controls(Me, False)
        BtnSave.Enabled = False
        BtnEdit.Enabled = False
        BtnPrint.Enabled = False
        IsDelivered.Enabled = True
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = x
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_SearchForCurrentRecorde", PRM)
        If DT.Rows.Count > 0 Then
            LoadToControlar(BranchRecievedID, "CoBranches_LoadDataIntoLookUpEdit", "BName", "DBRID", Nothing)
            CodeID.Text = DT.Rows(0)("Code").ToString
            InsertDate.EditValue = DT.Rows(0)("InsertDate")
            RecievedCurrencyID.EditValue = DT.Rows(0)("RecievedCurrencyID")
            DeliveredCurrencyID.EditValue = DT.Rows(0)("DeliveredCurrencyID")
            OverallVal.EditValue = DT.Rows(0)("OverallVal")
            ExVal.EditValue = DT.Rows(0)("ExVal")
            BranchRecievedID.EditValue = DT.Rows(0)("BranchRecievedID")
            SendType.EditValue = DT.Rows(0)("IsAccFrom")
            RecievType.SelectedIndex = DT.Rows(0)("IsAccTo")
            Dim PRM1(0) As SqlParameter
            PRM1(0) = New SqlParameter("@BranchId", SqlDbType.Int)
            PRM1(0).Value = DT.Rows(0)("BranchRecievedID")
            LoadToControlar(BranchDeliveredID, "COBRANCHTB_LoadDataIntoLookUpEdit", "BName", "DBRID", PRM1)
            BranchDeliveredID.EditValue = DT.Rows(0)("BranchDeliveredID")
            LoadToControlar(AgentCityID, "CitiesTb_LOADFORCONFIRM", "CityName", "ID", Nothing)
            AgentCityID.EditValue = DT.Rows(0)("DeliveryPlace")
            Notes.Text = DT.Rows(0)("Notes").ToString
            SenderName.Text = DT.Rows(0)("SenderName").ToString
            SPhone1.Text = DT.Rows(0)("SPhone1").ToString
            SPhone2.Text = DT.Rows(0)("SPhone2").ToString
            SenderIDNo.Text = DT.Rows(0)("SenderIDNo").ToString
            RecievedName.Text = DT.Rows(0)("RecievedName").ToString
            RPhone1.Text = DT.Rows(0)("RPhone1").ToString
            RPhone2.Text = DT.Rows(0)("RPhone2").ToString
            RecievedIDNo.Text = DT.Rows(0)("RecievedIDNo").ToString
            If DT.Rows(0)("IsAccFrom") = 2 Then
                LOADBBranch()
                LOADSERCICETYPE(DT.Rows(0)("BranchRecievedID"))
                'BBranchID.EditValue = DT.Rows(0)("BBranchID")
                ServiceVal.EditValue = DT.Rows(0)("EXTRVAL")
                ServiceID.EditValue = DT.Rows(0)("BBranchAccID")
            End If
            If ConfirmType = 1 Then
                BtnDelete.Enabled = True
                BtnDelete.Caption = "رفض الإعتماد"
                If DT.Rows(0)("Type_Moble") = 0 Or GetLKPColumnVal(BranchRecievedID, "BranchType") = 3 Then
                    OverallVal.Enabled = True
                    ExVal.Enabled = True
                End If
                If DT.Rows(0)("IsAccTo") = 0 Then
                BranchDeliveredID.Enabled = True
            End If
            IsDelivered.Text = "تأكيد الإعتماد"
            End If
            If ConfirmType = 2 Then
                IsDelivered.Text = "تأكيد التسليم"
            End If
            If ConfirmType = 5 Then
                BtnDelete.Enabled = True
                BtnDelete.Caption = "رفض الإلغاء"
                IsDelivered.Text = "تأكيد الإلغاء"
                BtnDelete.Caption = "رفض الإلغاء"
            End If
            If ConfirmType = 11 Then
                IsDelivered.Text = "تأكيد الوجهة"
                BranchDeliveredID.Enabled = True
            End If
            If ConfirmType = 12 Then
                IsDelivered.Enabled = False
                LayoutControlItem29.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                InternalStatus.Text = DT.Rows(0)("SName").ToString
                Me.Text = DT.Rows(0)("RUName").ToString + Space(1) + DT.Rows(0)("DUName").ToString
                BtnPrint.Enabled = True
            End If

        End If
    End Sub
    Sub ShowRecored(x)
        LOADBRNACH()
        'CoBranches_LoadForBranchNotMain()
        Enable_Controls(Me, False)
        BtnPrint.Enabled = True
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = x
        'PRM(1) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int)
        'PRM(1).Value = BID
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalExValues_SearchForInternalStutas", PRM)
        If DT.Rows.Count > 0 Then
            IsDelivered.Enabled = True
            CodeID.Text = DT.Rows(0)("Code").ToString
            InsertDate.EditValue = DT.Rows(0)("InsertDate")
            SenderName.Text = DT.Rows(0)("SenderName").ToString
            SPhone1.Text = DT.Rows(0)("SPhone1").ToString
            SPhone2.Text = DT.Rows(0)("SPhone2").ToString
            SenderIDNo.Text = DT.Rows(0)("SenderIDNo").ToString
            RecievedName.Text = DT.Rows(0)("RecievedName").ToString
            RPhone1.Text = DT.Rows(0)("RPhone1").ToString
            RPhone2.Text = DT.Rows(0)("RPhone2").ToString
            RecievedIDNo.Text = DT.Rows(0)("RecievedIDNo").ToString
            BranchRecievedID.EditValue = FrmInternalFastCall.GVROLE.GetFocusedRowCellValue("BranchRecievedID")
            LOADDELIVEREDBRANCH()
            RecievedCurrencyID.EditValue = DT.Rows(0)("RecievedCurrencyID")
            OverallVal.EditValue = DT.Rows(0)("OverallVal")
            ExVal.EditValue = DT.Rows(0)("ExVal")
            BranchDeliveredID.EditValue = DT.Rows(0)("BranchDeliveredID")
            DeliveredCurrencyID.EditValue = DT.Rows(0)("DeliveredCurrencyID")
            AgentCityID.EditValue = DT.Rows(0)("DeliveryPlace")
            Notes.Text = DT.Rows(0)("Notes").ToString
            SendType.EditValue = DT.Rows(0)("IsCash")
            If SendType.EditValue = 2 Then
                'LOADBBranch()
                BBranchID.EditValue = DT.Rows(0)("ID")
                ServiceVal.EditValue = DT.Rows(0)("EXTRVAL")
                ServiceID.EditValue = DT.Rows(0)("SID")
            End If

        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''كود الطباعة''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Overrides Sub Print()
        Try
            ' Route through RUN_QUARY_PRO (MySQL-aware) instead of opening SQLCON directly. Under MySQL mode
            ' SQLCON (a System.Data.SqlClient.SqlConnection) is never given a connection string, so SQLCON.Open()
            ' threw "The ConnectionString property has not been initialized." — and because it ran here inside
            ' Save -> Print AFTER the insert, the transfer saved but the receipt failed with that message.
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text}
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_InternalExValues_PrintRecords", PRM)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                dt.TableName = "InternalEx"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                report.DataSource = ds
                report.DataMember = "InternalEx"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                ''''''''''''''''sand fo Watsaapp
                Dim pdfOptions As ImageExportOptions = report.ExportOptions.Image
                Dim stordpath As String
                stordpath = Application.StartupPath & "\TEMPWATS"
                Directory.CreateDirectory(stordpath)
                Dim newfilepathe As String
                newfilepathe = stordpath & "\" & "watsappmassg.jpeg"
                report.ExportToImage(newfilepathe, pdfOptions)
                '''  end asnd for wtsap --------------------------------
                ' التحقق من القيمة و إرسال القيمة فارغة إذا كانت NULL
                Dim Phone_send As String = If(IsDBNull(dt.Rows(0)("Phone2")), "", dt.Rows(0)("Phone2").ToString)
                If IsDelivered.Checked = True Then
                Else
                    'REsevot_for_Watsaap(Phone_send, dt.Rows(0)("SPhone1"))
                    'sender_for_Watsaap(Phone_send, dt.Rows(0)("SPhone1"))
                End If
            Else
                ErrorMessage(Me, "رسالة معلومات", "رمز الحوالة خطأ يرجى التأكد من البيانات")
            End If
        Catch ex As Exception
            MD_MYSQL.LogAppError("caught in form", Me, ex)
            ErrorMessage(Me, "رسالة خطأ", $" {ex.Message}")
        End Try
        MyBase.Print()
    End Sub

    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub Remove()
        Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
        Dim lookAndFeelError2 As New UserLookAndFeel(Me)
        lookAndFeelError2.Style = LookAndFeelStyle.Skin
        lookAndFeelError2.UseDefaultLookAndFeel = False
        lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل أنت متأكد من أنك تريد رفض الإعتماد؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        ConfirmType = 10
        If result = DialogResult.Yes Then
            SetData()
            FrmRemoveMessage.Text = "تم الرفض بنجاح"
            MyBase.Remove()
        End If
    End Sub
#End Region

#Region "الأحداث"
#Region "أحداث تغير الفرع"
    Private Sub BranchDeliveredID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchDeliveredID.EditValueChanged
        If RecievType.SelectedIndex = 1 Then
            RPhone1.Text = ""
            RPhone2.Text = ""
            RecievedIDNo.Text = ""
            RecievedName.Text = ""
            TransAccIDTo = 0
        End If
        If IsEmpty(BranchDeliveredID) Then Exit Sub
        DBRTYPE = GetLKPColumnVal(BranchDeliveredID, "BranchType")
        AgentCityID.EditValue = GetLKPColumnVal(BranchDeliveredID, "CityID")
        'End If
    End Sub
#End Region
#Region "أحداث اختيار الشخص المرسل والمستلم"
    ''تغير نوع الاستلام
    Private Sub SendType_EditValueChanged(sender As Object, e As EventArgs) Handles SendType.EditValueChanged
        IsAccFrom = 0
        ServiceVal.EditValue = 0.000
        ServiceTotalVal.EditValue = 0.000
        BBranchID.EditValue = -1
        ServiceID.EditValue = -1
        If ConfirmType = 0 And SendType.Text <> String.Empty Then
            If SendType.EditValue = 1 Then
                BranchRecievedID.Enabled = False
                BranchRecievedID.EditValue = BID
                SenderName.Properties.Buttons(0).Enabled = True
                SenderName.ReadOnly = True
                SPhone1.ReadOnly = True
                SPhone2.ReadOnly = True
                SenderIDNo.ReadOnly = True
                SenderName.Text = ""
                SPhone1.Text = ""
                SPhone2.Text = ""
                SenderIDNo.Text = ""
                IsAccFrom = 1
            Else
                BranchRecievedID.EditValue = BID
                If BID = MAINBID Then
                    BranchRecievedID.Enabled = True
                Else
                    BranchRecievedID.Enabled = False
                End If
                SenderName.Properties.Buttons(0).Enabled = False
                SenderName.ReadOnly = False
                SPhone1.ReadOnly = False
                SPhone2.ReadOnly = False
                SenderIDNo.ReadOnly = False
                SenderName.Text = ""
                SPhone1.Text = ""
                SPhone2.Text = ""
                SenderIDNo.Text = ""
            End If
            If SendType.EditValue = 2 Then
                ExVal.Enabled = False
                LOADSERCICETYPE(BranchRecievedID.EditValue)
                'BBranchID.Enabled = True
                ServiceID.Enabled = True
            Else
                'BBranchID.Enabled = False
                ExVal.Enabled = True
                ServiceID.Enabled = False
            End If
        End If
    End Sub

    '' اختيار نوع الحساب المرسل
    Private Sub SenderName_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles SenderName.ButtonClick
        FRMSELECTACCOUNT.SendOrRec = 0
        AccFrom = 0
        SenderName.Text = ""
        SPhone1.Text = ""
        SPhone2.Text = ""
        SenderIDNo.Text = ""
        FRMSELECTACCOUNT.ShowDialog()
    End Sub
    ''تغير نوع التسليم
    Private Sub RecievType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RecievType.SelectedIndexChanged
        If ConfirmType = 0 Then
            IsAccTo = 0
            BranchDeliveredID.EditValue = -1
            RecievedName.Text = ""
            RPhone1.Text = ""
            RPhone2.Text = ""
            RecievedIDNo.Text = ""
            OutSysCh.Checked = False
            If RecievType.SelectedIndex = 1 Then
                RecievedName.Properties.Buttons(0).Enabled = True
                RecievedName.ReadOnly = True
                RPhone1.ReadOnly = True
                RPhone2.ReadOnly = True
                RecievedIDNo.ReadOnly = True
                IsAccTo = 1
            Else
                RecievedName.Properties.Buttons(0).Enabled = False
                RecievedName.ReadOnly = False
                RPhone1.ReadOnly = False
                RPhone2.ReadOnly = False
                RecievedIDNo.ReadOnly = False
            End If
        End If
    End Sub

    Private Sub OverallVal_EditValueChanged(sender As Object, e As EventArgs) Handles OverallVal.EditValueChanged
        BBExvalCalc()
    End Sub

    Private Sub ExVal_EditValueChanged(sender As Object, e As EventArgs) Handles ExVal.EditValueChanged
        BBExvalCalc()
    End Sub

    '' اختيار نوع الحساب المسلم
    Private Sub RecievedName_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles RecievedName.ButtonClick
        If BranchDeliveredID.EditValue = -1 Or BranchDeliveredID.Text = String.Empty Then
            BranchDeliveredID.ErrorText = "يجب اختيار الفرع المرسلة له أولاً"
            Exit Sub
        End If
        If e.Button.Index = 0 Then
            FRMSELECTACCOUNT.SendOrRec = 1
            TransAccIDTo = 0
            RecievedName.Text = ""
            RPhone1.Text = ""
            RPhone2.Text = ""
            RecievedIDNo.Text = ""
            FRMSELECTACCOUNT.ShowDialog()
        End If
    End Sub


#End Region

#Region "أحداث المصارف"
    Public Sub BBExvalCalc()
        ServiceVal.EditValue = 0
        ServiceTotalVal.EditValue = 0
        If SendType.EditValue = 2 Then
            Dim ServiceRate As Decimal = Convert.ToDecimal(GetLKPColumnVal(ServiceID, "ValRate")) * OverallVal.EditValue
            Dim DifferentialVal As Decimal = Convert.ToDecimal(GetLKPColumnVal(ServiceID, "DifferentialVal"))
            Dim TransVal As Decimal = Convert.ToDecimal(GetLKPColumnVal(ServiceID, "TransVal"))
            If SendType.EditValue = 2 AndAlso OverallVal.EditValue > 0D Then
                ExVal.EditValue = TransVal * OverallVal.EditValue
                ServiceVal.EditValue = ServiceRate
                Dim TempVal As Decimal = OverallVal.EditValue + ServiceVal.EditValue + ExVal.EditValue
                ServiceTotalVal.EditValue = OverallVal.EditValue + ExVal.EditValue + ServiceVal.EditValue
            End If
        End If
    End Sub

    Private Sub BranchDeliveredID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchDeliveredID.QueryPopUp
        HideAllColumnsExceptDisplay(BranchDeliveredID)
    End Sub

    Private Sub BranchRecievedID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchRecievedID.EditValueChanged
        If BranchRecievedID.Text = String.Empty Then Exit Sub
        BranchDeliveredID.EditValue = -1
        LOADDELIVEREDBRANCH()
        If ConfirmType = 0 Then
            RBRTYPE = GetLKPColumnVal(BranchRecievedID, "BranchType")
            CTID = GetLKPColumnVal(BranchRecievedID, "CityID")
            inscls.InternalEx_MaxID(1, BranchRecievedID.EditValue, UserID, COUNTRYNID, CITYID)
        End If
    End Sub
    Private Sub ServiceID_EditValueChanged(sender As Object, e As EventArgs) Handles ServiceID.EditValueChanged
        If ServiceID.Text <> String.Empty Then
            BBranchID.EditValue = GetLKPColumnVal(ServiceID, "BankID")
            BBExvalCalc()
        End If
    End Sub
#End Region

    ''خارج المنظومة
    Private Sub OutSysCh_CheckedChanged(sender As Object, e As EventArgs) Handles OutSysCh.CheckedChanged
        BranchDeliveredID.EditValue = -1
        AgentCityID.EditValue = -1
        If OutSysCh.Checked = True Then
            BranchDeliveredID.Enabled = False
            AgentCityID.Enabled = True
        Else
            BranchDeliveredID.Enabled = True
            AgentCityID.Enabled = False
        End If
    End Sub
    Private Sub OverallVal_Leave(sender As Object, e As EventArgs) Handles OverallVal.Leave
        If OverallVal.Text <> String.Empty And Application.OpenForms().OfType(Of FRMINTERNALTRANSFER).Any Then
            If OverallVal.EditValue > 0.000 And ConfirmType = 0 Then
                InfoMessage(Me, "رسالة معلومات", "قيمة الحوالة المدخلة:" & Space(1) & OverallVal.EditValue & vbNewLine & "بالحروف:" & Space(1) & Cur_Code("ليبي", OverallVal.EditValue, False, False))
            End If
        End If
    End Sub
    Private Sub CodeID_Enter(sender As Object, e As EventArgs) Handles CodeID.Enter
        If ConfirmType = 0 And BtnSave.Enabled = True Then
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد حفظ بيانات الحوالة؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                Save()
            End If
        End If
    End Sub
    Private Sub FRMINTERNALTRANSFER_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
#End Region


    Sub Confirm()

        If (RBRTYPE = 1 Or RBRTYPE = 2) And DBRTYPE = 3 Then
            'ISHandallEX = 1
            ExValSahreByHand.IsAgintToAgint = False
            ExValSahreByHand.LayoutControlItem2.Text = "عمولة الوكيل"
            ExValSahreByHand.LayoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            ExValSahreByHand.NEWRECORD()
            ExValSahreByHand.LoadExVal()
            ExValSahreByHand.ShowDialog()
            'ExValSahreByHand.sEnFRoRElode(iscode, bdid, 0)
        End If
        If RBRTYPE = 3 And DBRTYPE = 3 Then
            'ISHandallEX = 1
            ExValSahreByHand.IsAgintToAgint = True
            ExValSahreByHand.LayoutControlItem2.Text = "الوكيل الراسل"
            ExValSahreByHand.LayoutControlItem6.Text = "الوكيل المسلم"
            ExValSahreByHand.LayoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            ExValSahreByHand.NEWRECORD()
            ExValSahreByHand.LoadExVal()
            ExValSahreByHand.ShowDialog()
            'ExValSahreByHand.sEnFRoRElode(iscode, bdid, 1)
        End If
    End Sub


#Region "الصلاحيات و الواتس"
    Public Overrides Sub CHECKBUTTONS()
        lodePreportes()
        MyBase.CHECKBUTTONS()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(14, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            'If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

















#End Region
End Class
