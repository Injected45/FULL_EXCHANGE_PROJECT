Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FRMEXTERNALTRANS
    Public AccFromOrTo, CurrencyByCountry, BNKID, BankServID, EMPCUSTSELECT, ectypeto, MsgStatus, IsInOrOut, ConfirmType, OurAccID, SerID, RBRTYPE, AccountType As Integer
    Public AccTrans As Integer = 0
    Public IsAccFrom As Int32 = 0
    Public IsAccTo As Int32 = 0
    Public AccFrom, TransAccIDTo, IDCode, BBranchAccID As ULong
    Public ISUpdate, DTCheck, IsPrivateAccount, IsServiceVal, IsHandelExAVal, IsToBank As Boolean
    Public TransPriceVal As Boolean
    Public SerExVal, FinalVal, ServiceRate, HandelExAVal, publicPrice As Decimal
    Public NetFinalTotal As ULong
    Dim report As New RPTEXTERNALFRM
    Public Sub SendTypeTB_Roll_luckbedit(ScreenID As ULong)
        Try


            Dim dt As New DataTable
            dt.Clear()
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
            prm(1) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = UserID}
            dt = RUN_QUARY_PRO("SendTypeTB_Roll_luckbedit", prm)

            If dt.Rows.Count > 0 Then

                SendTypeTB_Roll_luckbedit_loc(ScreenID)
                IsCash.Enabled = True
            Else
                LoadToControlar(IsCash, "SendTypeTB_LoadToLKP", "SName", "SID", Nothing)
                HideAllColumnsExceptDisplay(IsCash)
                IsCash.EditValue = 0
                IsCash.Enabled = False
            End If
            dt.Dispose()
        Catch ex As Exception
            ErrorMessage(Me, " ex.Message)", ex.Message)
        End Try
    End Sub
    Public Sub SendTypeTB_Roll_luckbedit_loc(ScreenID)
        Dim prm1(1) As SqlParameter
        prm1(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        prm1(1) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = UserID}
        LoadToControlar(IsCash, "SendTypeTB_Roll_luckbedit", "SName", "SID", prm1)
        HideAllColumnsExceptDisplay(IsCash)
    End Sub
    Sub NewRecord()
        AccountType = 0
        Me.Text = "تحويل خارجي"
        ISUpdate = 0
        IsAccFrom = 0
        IsAccTo = 0
        RBRTYPE = 0
        IsToBank = False
        SendTypeTB_Roll_luckbedit(17)
        IsHandelExAVal = 0
        LoadReceivedCurrency()
        BranchRecievedID.EditValue = -1
        ISUpdate = False
        IsCash.EditValue = 0
        BBranchID.EditValue = -1
        BankServiceType.EditValue = -1
        RecievedCurrencyID.EditValue = -1
        BankExVAL.EditValue = 0.000
        ServiceTotalVal.EditValue = 0.000
        SenderName.Text = ""
        SenderIDNo.Text = ""
        SPhone1.Text = ""
        SPhone2.Text = ""
        RecievedName.Text = ""
        RPhone1.Text = ""
        RPhone2.Text = ""
        ToCityOrBankID.EditValue = -1
        CountryIDTo.EditValue = -1
        'ClearLKPEdit(CountryIDTo)
        TransType.SelectedIndex = -1
        ServiceTransID.SelectedIndex = -1
        OwnAccNo.Text = ""
        OwnNatioNum.Text = ""
        ServiceType.EditValue = -1
        BranchDeliveredID.EditValue = -1
        DeliveredCurrencyID.EditValue = -1
        RecievedName.Text = ""
        'RecievedIDNo.Text = ""
        RPhone1.Text = ""
        RPhone2.Text = ""
        CurrRecievedVal.EditValue = 0.000
        ExVal.EditValue = 0.000
        ExtraVal.EditValue = 0.000
        BranchDeliveredID.EditValue = -1
        ServiceExVal.Text = 0.000
        TransPrice.Text = 0.000
        TransPrice1.Text = 0.000
        Notes.Text = ""
        InsertDate.EditValue = Date.Now
        BranchRecievedID.EditValue = BID
        If CountryIDFrom.EditValue = COUNTRYNID Then
            RecievedCurrencyID.EditValue = DefaultCurrency
        Else
            RecievedCurrencyID.EditValue = -1
        End If
        EnabledCTRL(True)
        IsDelivered.Enabled = False
        If BID <> MAINBID Then
            TransPrice1.Enabled = True
        Else
            TransPrice1.Enabled = False
        End If
        LayoutControlItem10.Text = "الوجهة"
        ExtraVal.ReadOnly = True
        ConfirmType = 0
        If UserType = 1 Or UserType = 3 Then
            LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        LOADSERCICETYPE()
    End Sub
    Public Sub EnabledCTRL(IsEnabled As Boolean)
        If CountryIDFrom.EditValue = COUNTRYNID Then
            If BID = MAINBID Then
                CountryIDFrom.Enabled = IsEnabled
            Else
                CountryIDFrom.Enabled = False
            End If
            BranchRecievedID.Enabled = IsEnabled
            ExVal.Enabled = IsEnabled
            ServiceTransID.Enabled = IsEnabled
            'SenderName.Properties.Buttons(0).Enabled = IsEnabled
            If IsCash.EditValue = 2 Then
                'BBranchID.Enabled = IsEnabled
                BankServiceType.Enabled = IsEnabled
            End If
        Else
            CountryIDFrom.Enabled = False
            IsCash.Enabled = False
            'BBranchID.Enabled = False
            BankServiceType.Enabled = False
            ExVal.Enabled = False
            ServiceTransID.Enabled = False
            'SenderName.Properties.Buttons(0).Enabled = False
        End If
        BranchRecievedID.Enabled = IsEnabled
        SenderName.Enabled = IsEnabled

        SenderIDNo.Enabled = IsEnabled
        SPhone1.Enabled = IsEnabled
        SPhone2.Enabled = IsEnabled
        CountryIDTo.Enabled = IsEnabled
        ServiceType.Enabled = IsEnabled
        ServiceExVal.Enabled = False
        BankExVAL.Enabled = False
        ServiceTotalVal.Enabled = False
        BranchDeliveredID.Enabled = IsEnabled
        DeliveredCurrencyID.Enabled = False
        RecievedName.Enabled = IsEnabled
        ToCityOrBankID.Enabled = IsEnabled
        RPhone1.Enabled = IsEnabled
        RPhone2.Enabled = IsEnabled
        CurrRecievedVal.Enabled = IsEnabled
        ExtraVal.Enabled = IsEnabled
        Notes.Enabled = IsEnabled
        OwnNatioNum.Enabled = IsEnabled
        RecievedCurrencyID.Enabled = False
        If BID = MAINBID Then
            CountryIDFrom.Enabled = IsEnabled
            BranchRecievedID.Enabled = IsEnabled
        Else
            CountryIDFrom.Enabled = False
            BranchRecievedID.Enabled = False
        End If
    End Sub
    Public Sub lodePreportes()
        Invoke(Sub()
                   Dim dt As New DataTable
                   dt.Clear()
                   If ConfirmType = 0 Then
                       dt = SElectUEserFormButtn(17, UserID)
                   ElseIf ConfirmType = 4 Then

                       dt = SElectUEserFormButtn(21, UserID)
                   End If

                   If dt.Rows.Count > 0 Then


                       If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                       If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                       If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

                   End If
               End Sub)
    End Sub

#Region "أكواد تعبئة أدوات الجزء الخاص بالراسل"
    Sub LoadCountryFrom()
        Invoke(Sub()
                   Dim DT As New DataTable
                   DT.Clear()
                   DT = RUN_QUARY_TXT("CountriesTb_LoadToGViewLKP")
                   If DT.Rows.Count > 0 Then
                       CountryIDFrom.Properties.DataSource = DT
                       CountryIDFrom.Properties.ValueMember = "CouID"
                       CountryIDFrom.Properties.DisplayMember = "CountryName"
                       CountryIDFrom.Properties.ShowHeader = False
                   End If
               End Sub)
    End Sub


    Sub LOADReceivedBranch()
        BranchRecievedID.EditValue = -1
        If CountryIDFrom.EditValue <> -1 Or CountryIDFrom.Text <> String.Empty Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int)
            PRM(0).Value = CountryIDFrom.EditValue
            PRM(1) = New SqlParameter("@BID", SqlDbType.Int)
            PRM(1).Value = BID
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPWITHCOUNTRYAndBID", PRM)
            BranchRecievedID.Properties.DataSource = DT
            BranchRecievedID.Properties.ValueMember = "DBRID"
            BranchRecievedID.Properties.DisplayMember = "BName"
            BranchRecievedID.Properties.ShowHeader = False
        End If
    End Sub

    Sub LoadReceivedCurrency()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CurrencyMainTb_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            RecievedCurrencyID.Properties.DataSource = DT
            RecievedCurrencyID.Properties.ValueMember = "ID"
            RecievedCurrencyID.Properties.DisplayMember = "CuName"
            RecievedCurrencyID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADBRANCHSERVICES()
        If ConfirmType <> 0 Then Exit Sub
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryId", SqlDbType.Int)
        PRM(0).Value = CountryIDFrom.EditValue
        LoadToControlar(BBranchID, "BanksTb_LOADTOLKP_BasedONCountryID", "BankName", "BNKID", PRM)
        'Dim DT As New DataTable
        'DT = RUN_QUARY_PRO_ONLY("BBranchTb_LoadBasedOnServices")
        'If DT.Rows.Count > 0 Then
        '    BBranchID.Properties.DataSource = DT
        '    BBranchID.Properties.ValueMember = "ID"
        '    BBranchID.Properties.DisplayMember = "BranchName"
        '    DVGLKP.Columns("ID").Visible = False
        '    DVGLKP.Columns("AccID").Visible = False
        '    NEWDVGFROMAT(DVGLKP)
        'End If
    End Sub
    Sub LOADSERCICETYPE()
        BankServiceType.Properties.DataSource = Nothing
        BankServiceType.EditValue = -1
        'If IsCash.EditValue = 2 And BBranchID.EditValue <> -1 Or BBranchID.Text <> String.Empty Then
        'Dim PR(0) As SqlParameter
        '    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BBranchID.EditValue}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_alter("BankService_LoadALLToLKP", Nothing)
        If DT.Rows.Count > 0 Then
                BankServiceType.Properties.DataSource = DT
                BankServiceType.Properties.ValueMember = "ID"
                BankServiceType.Properties.DisplayMember = "ServiceName"
                NEWDVGFROMAT(SerDvglkp)
            End If
        'End If
    End Sub
#End Region

#Region "أكواد الحفظ والطباعة والتعديل والتنظيف"
    Public Overrides Sub BNew()
        ISUpdate = 0
        ConfirmType = 0
        FRMEXTERNALTRANS_Load(Nothing, Nothing)
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()

        ExternalEx_Insert()

        If MsgStatus = 1 Then
            MyBase.SetData()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        'If IsInOrOut = 0 Then
        Dim SerT As Integer
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
        PR(1) = New SqlParameter("@SerType", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("ExternalEx_LoadRecordToPrint1", PR)
        If DT.Rows.Count > 0 Then
            SerT = DT.Rows(0)("SerType")
            report.DataSource = DT
            'report.DataAdapter = DA
            report.DataMember = "ExternalEx"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.XrLabel48.Text = Cur_Code(DT.Rows(0)("العملة المسلمة"), DT.Rows(0)("قيمة الحوالة"), False, "n2")
            report.XrLabel15.Text = Cur_Code(DT.Rows(0)("العملة المستلمة"), DT.Rows(0)("القيمة"), False, "n2")
            If DT.Rows(0)("IsInOrOut") = 0 Then
                report.XrLabel7.Text = "حوالات خارجية - صادرة"
            Else
                report.XrLabel7.Text = "حوالات خارجية -واردة"
            End If

            report.CreateDocument()

            report.ShowPreview
            Dim Phone_send = ""


            If SerT > 0 Then

            Else
                'report.LServiceName.Visible = False
                report.ServiceName.Visible = False
                report.ServiceVal.Visible = False
                'report.LServiceVal.Visible = False
            End If
        Else
            ErrorMessage(Me, "رسالة معلومات", "رمز الحوالة خطأ يرجى التأكد من البيانات")
        End If
        'End If

        MyBase.Print()
    End Sub
#End Region
#Region "رسائل الوتساب"



    'استندر الرسالة 








    ' إرسال الرسالة عبر WhatsApp
    Private Sub SendMessage(message As String, Phone As String)
        WATSAPPMsAG(Phone, message, whatsapp_contacts(Phone))
    End Sub

#Region "ارسال رسائل الي مجموعة"


#End Region


#End Region
#Region "الأحداث"
    Private Sub FRMEXTERNALTRANS_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        lodePreportes()
        LoadCountryFrom()
        If ConfirmType = 0 Then
            BtnSave.Enabled = True
            BtnPrint.Enabled = False
            CountryIDFrom.EditValue = -1
            CountryIDFrom.EditValue = COUNTRYNID

            NewRecord()
        Else
            BtnSave.Enabled = False
            BtnPrint.Enabled = True
            'End If
            'If ConfirmType = 1 Then
        End If
        RecievedCurrencyID_TextChanged(Nothing, Nothing)
        DeliveredCurrencyID_TextChanged(Nothing, Nothing)
        DeliveredCurrencyID_EditValueChanged(Nothing, Nothing)
    End Sub

    Private Sub CountryIDFrom_EditValueChanged(sender As Object, e As EventArgs) Handles CountryIDFrom.EditValueChanged
        LOADReceivedBranch()
        LoadCountryTo()
        If ISUpdate = 0 Then
            NewRecord()
        End If
        If CountryIDFrom.EditValue <> -1 Then
            LoadCurrencyByCountry(CountryIDFrom.EditValue)
            RecievedCurrencyID.EditValue = CurrencyByCountry
        End If
        If ConfirmType = 0 Then
            InternalEx_MaxID()
        End If
        If CountryIDFrom.EditValue = COUNTRYNID Then
            IsCash.Enabled = True
        Else
            IsCash.EditValue = 0
            IsCash.Enabled = False
        End If
    End Sub

    Private Sub BranchRecievedID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchRecievedID.EditValueChanged
        IsCash.EditValue = 0
        IsCash_SelectedIndexChanged(Nothing, Nothing)
        'If BranchRecievedID.EditValue > -1 Then
        '    If CoBranch_BranchType(BranchRecievedID.EditValue) <> 1 Then
        '        SenderName.Properties.Buttons(0).Enabled = False
        '    Else
        '        SenderName.Properties.Buttons(0).Enabled = True
        '    End If
        'End If
        'If BranchRecievedID.EditValue = MAINBID Then
        '    IsCash.Enabled = True
        'Else
        '    IsCash.SelectedIndex = 0
        '    IsCash.Enabled = False
        'End If
    End Sub

    Private Sub IsCash_SelectedIndexChanged(sender As Object, e As EventArgs) Handles IsCash.EditValueChanged
        BankExVAL.EditValue = 0.000
        ServiceTotalVal.EditValue = 0.000
        BBranchID.EditValue = -1
        BankServiceType.EditValue = -1
        SenderName.Text = ""
        SenderIDNo.Text = ""
        SPhone1.Text = ""
        SPhone2.Text = ""
        IsAccFrom = 0
        If CountryIDFrom.EditValue = COUNTRYNID Then
            If ConfirmType = 0 Then
                If IsCash.EditValue = 1 Then
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
                    'BranchRecievedID.EditValue = BID
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
                If IsCash.EditValue = 2 Then
                    LOADBRANCHSERVICES()
                    'BBranchID.Enabled = True
                    BankServiceType.Enabled = True
                Else
                    'BBranchID.Enabled = False
                    BankServiceType.Enabled = False
                End If
            End If
        Else
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

    End Sub
    Private Sub BBranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BBranchID.EditValueChanged

    End Sub
    Private Sub SenderName_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles SenderName.ButtonClick
        FRMSELECTACCOUNT.SendOrRec = 0
        AccFrom = 0
        SenderName.Text = ""
        SPhone1.Text = ""
        SPhone2.Text = ""
        SenderIDNo.Text = ""
        FRMSELECTACCOUNT.ShowDialog()
    End Sub
    Private Sub ServiceTransID_TextChanged(sender As Object, e As EventArgs) Handles ServiceTransID.TextChanged
        If CountryIDTo.EditValue = COUNTRYNID Then
            IsAccTo = 0
            'BranchDeliveredID.EditValue = -1
          RecievedName.Text = ""
            RPhone1.Text = ""
            RPhone2.Text = ""
        'OwnNatioNum.Text = ""
            If ServiceTransID.SelectedIndex = 0 Then
                RecievedName.Properties.Buttons(0).Enabled = True
                RecievedName.ReadOnly = True
                RPhone1.ReadOnly = True
                RPhone2.ReadOnly = True
                OwnNatioNum.ReadOnly = True
                IsAccTo = 1
            Else
                RecievedName.Properties.Buttons(0).Enabled = False
                RecievedName.ReadOnly = False
                RPhone1.ReadOnly = False
                RPhone2.ReadOnly = False
                OwnNatioNum.ReadOnly = False
            End If
        Else
            OwnAccNo.Text = ""
            If ConfirmType = 0 Then
                If ServiceTransID.SelectedIndex = 0 Then
                    OwnAccNo.Enabled = True
                Else
                    OwnAccNo.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub ServiceTransID_EnabledChanged(sender As Object, e As EventArgs) Handles ServiceTransID.EnabledChanged
        OwnAccNo.Text = ""
        'OwnNatioNum.Text = ""
        If ConfirmType = 0 Then
            If ServiceTransID.SelectedIndex = 0 Then
                OwnAccNo.Enabled = True
            Else
                OwnAccNo.Enabled = False
            End If
        End If
    End Sub

    Private Sub CountryIDTo_EditValueChanged(sender As Object, e As EventArgs) Handles CountryIDTo.EditValueChanged
        TransType.SelectedIndex = -1
        ServiceType.Properties.DataSource = Nothing
        ServiceType.EditValue = -1
        RecievedName.Properties.Buttons(0).Enabled = False
        LOADDeliveredBranchID()
        ToCityOrBankID.EditValue = -1
        ToCityOrBankID.Properties.DataSource = Nothing
        LoadCities()
        LoadServiceType(CountryIDTo.EditValue)
        ServiceTransID.SelectedIndex = -1
        If CountryIDTo.EditValue <> -1 Then
            LoadCurrencyByCountry(CountryIDTo.EditValue)
            DeliveredCurrencyID.EditValue = CurrencyByCountry
        End If
        If CountryIDTo.EditValue = COUNTRYNID Then
            ServiceTransID.Enabled = True
            'RecievedName.Properties.Buttons(0).Enabled = True
            ServiceType.Enabled = False
            OwnAccNo.Enabled = False
            ExtraVal.Enabled = False
        Else
            'RecievedName.Properties.Buttons(0).Enabled = False
            ServiceTransID.Enabled = False
            TransType.Enabled = False
            TransType.SelectedIndex = 1
            ServiceType.Enabled = True
            OwnAccNo.Enabled = True
            ExtraVal.Enabled = True
        End If
    End Sub
    Private Sub TransType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TransType.SelectedIndexChanged
        LOADDeliveredBranchID()
        If ConfirmType = 0 Then
            'If TransType.SelectedIndex = 0 Then
            '    ServiceTransID.SelectedIndex = 0
            '    ServiceTransID.Enabled = True
            'Else
            '    ServiceTransID.SelectedIndex = -1
            '    ServiceTransID.Enabled = False
            'End If
        End If
        If CountryIDTo.EditValue <> COUNTRYNID And (ConfirmType = 1 Or ConfirmType = 0) Then
            If (TransType.SelectedIndex = 0 Or TransType.SelectedIndex = 2) Then
                BranchDeliveredID.EditValue = -1
                BranchDeliveredID.Enabled = True
            Else
                BranchDeliveredID.EditValue = -1
                BranchDeliveredID.Enabled = False
            End If
        End If
    End Sub
    Private Sub RecievedCurrencyID_EditValueChanged(sender As Object, e As EventArgs) Handles RecievedCurrencyID.EditValueChanged
        LoadDeliveredCurrency(RecievedCurrencyID.EditValue)
        If RecievedCurrencyID.Text <> String.Empty Then
            SubCurrOvarallVal.Text = Cur_Code1(RecievedCurrencyID.Text)
            SubCurrExVal.Text = Cur_Code1(RecievedCurrencyID.Text)
        Else
            SubCurrOvarallVal.Text = String.Empty
            SubCurrExVal.Text = String.Empty
        End If
    End Sub
    Private Sub DeliveredCurrencyID_TextChanged(sender As Object, e As EventArgs) Handles DeliveredCurrencyID.TextChanged
        If DeliveredCurrencyID.Text <> String.Empty Then
            SubCurrNetTotal.Text = Cur_Code1(DeliveredCurrencyID.Text)
            SubCurrExValTotal.Text = Cur_Code1(DeliveredCurrencyID.Text)
            SubCurrDeliveredVal.Text = Cur_Code1(DeliveredCurrencyID.Text)
        Else
            SubCurrNetTotal.Text = String.Empty
            SubCurrExValTotal.Text = String.Empty
            SubCurrDeliveredVal.Text = String.Empty
        End If
    End Sub

    Public Sub get_NetTotalVal()
        Try
            If RecievedCurrencyID.EditValue = -1 Or RecievedCurrencyID.Text = String.Empty Then
                RecievedCurrencyID.ErrorText = "الرجاء اختيار العملة الاولى "
                CurrRecievedVal.EditValue = 0.000
                Exit Sub
            End If
            If DeliveredCurrencyID.EditValue = -1 Or DeliveredCurrencyID.Text = String.Empty Then
                DeliveredCurrencyID.ErrorText = "الرجاء اختيار العملة الثانية "
                CurrRecievedVal.EditValue = 0.000
                Exit Sub
            End If
            If CountryIDTo.EditValue = -1 Or CountryIDTo.Text = String.Empty Then
                CountryIDTo.ErrorText = "الرجاء اختيار الدولة "
                CurrRecievedVal.EditValue = 0.000
                Exit Sub
            End If
            If CountryIDTo.EditValue <> COUNTRYNID Then
                If TransType.SelectedIndex = -1 Or TransType.Text = String.Empty Then
                    TransType.ErrorText = "الرجاء اختيار نوع التحويل "
                    CurrRecievedVal.EditValue = 0.000
                    Exit Sub
                End If
                If ServiceType.EditValue = -1 Or ServiceType.Text = String.Empty Then
                    ServiceType.ErrorText = "الرجاء اختيار نوع الخدمة "
                    CurrRecievedVal.EditValue = 0.000
                    Exit Sub
                End If
            End If
            Dim AccountType As Integer
            Dim BasicCountry As Integer
            If CountryIDTo.EditValue = COUNTRYNID Then
                BasicCountry = CountryIDFrom.EditValue
                AccountType = 3
            End If
            If CountryIDFrom.EditValue = COUNTRYNID Then
                BasicCountry = CountryIDTo.EditValue
                If TransType.SelectedIndex = 0 Then
                    AccountType = 0
                End If
                If TransType.SelectedIndex = 1 Then
                    AccountType = 3
                End If
                If TransType.SelectedIndex = 2 Then
                    AccountType = 1
                End If
            End If
            If ConfirmType = 0 Then
                NetTotal.Text = 0.000
                NetFinalTotal = 0.000
            End If
            Dim prm(8) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = RecievedCurrencyID.EditValue}
            prm(1) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = CurrRecievedVal.EditValue}
            prm(2) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = DeliveredCurrencyID.EditValue}
            prm(3) = New SqlParameter("@BPrice11", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@Purchaseprice", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@AccountType", SqlDbType.Int) With {.Value = AccountType}
            prm(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchDeliveredID.EditValue}
            prm(7) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = BasicCountry}
            prm(8) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = SafeToInt(ServiceType.EditValue)}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EXEX_get_NetTotal", prm)
            TransPrice.Text = prm(4).Value
            TransPrice1.Text = prm(4).Value
            If ConfirmType = 0 Then
                If CurrRecievedVal.EditValue > 0 Then
                    NetFinalTotal = Convert.ToDouble(prm(3).Value)
                    CurrDeliveredVal.EditValue = Convert.ToDouble(prm(3).Value)
                Else
                    NetTotal.Text = 0.00
                    CurrDeliveredVal.EditValue = 0.00
                    CurrRecievedVal.ErrorText = "الرجاء ادخال القيمة الاولى "
                End If

                If CountryIDFrom.EditValue = COUNTRYNID Then
                    GetServiceVal()
                Else
                    SerExVal = 0.000
                End If

                If SerExVal > 0 Then
                    FinalVal = NetFinalTotal - SerExVal
                    NetTotal.Text = Convert.ToString(FinalVal)
                    ExValTotal.Text = Cur_Code(DeliveredCurrencyID.Text, SerExVal + ExtraVal.EditValue, True)
                Else
                    NetTotal.Text = NetFinalTotal
                    ExValTotal.Text = Cur_Code(DeliveredCurrencyID.Text, 0.000, True)
                End If
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ في نظـــــــــام", "الرجاء التأكد من إدخال أسعار للعملات في دولة" & Space(1) & CountryIDTo.Text & Space(1) & "ونوع التحويل" & Space(1) & TransType.Text & vbNewLine & "إن استمرت المشكلة الرجاء الاتصال بقسم التطوير")
        End Try
    End Sub


#Region "QueryPopUp"
    Private Sub CountryIDFrom_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CountryIDFrom.QueryPopUp
        Try
            CountryIDFrom.Properties.PopulateColumns()
            CountryIDFrom.Properties.Columns("CouID").Visible = False
        Catch ex As Exception
            CountryIDFrom.ErrorText = "عذرا لا يوجد بيانات في هذا الحقل"
        End Try
    End Sub

    Private Sub BranchRecievedID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchRecievedID.QueryPopUp
        If CountryIDFrom.EditValue = -1 Or CountryIDFrom.Text = String.Empty Then
            CountryIDFrom.ErrorText = "يجب إختيار الدولة الأولى"
            Exit Sub
        End If
        Try
            BranchRecievedID.Properties.PopulateColumns()
            BranchRecievedID.Properties.Columns("DBRID").Visible = False
            BranchRecievedID.Properties.Columns("BranchType").Visible = False
        Catch ex As Exception
            BranchRecievedID.ErrorText = "عذرا لا يوجد بيانات في هذا الحقل"
        End Try
    End Sub

    Private Sub RecievedCurrencyID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles RecievedCurrencyID.QueryPopUp
        Try
            RecievedCurrencyID.Properties.PopulateColumns()
            RecievedCurrencyID.Properties.Columns("ID").Visible = False
        Catch ex As Exception
            RecievedCurrencyID.ErrorText = "عذرا لا يوجد بيانات في هذا الحقل"
        End Try
    End Sub
    Private Sub DeliveredCurrencyID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DeliveredCurrencyID.QueryPopUp
        If RecievedCurrencyID.EditValue = -1 Or RecievedCurrencyID.Text = String.Empty Then
            DeliveredCurrencyID.ErrorText = "يجب إختيار العملة الأولى"
            Exit Sub
        End If
        Try
            DeliveredCurrencyID.Properties.PopulateColumns()
            DeliveredCurrencyID.Properties.Columns("ID").Visible = False
        Catch ex As Exception
            DeliveredCurrencyID.ErrorText = "عذرا لا يوجد بيانات في هذا الحقل"
        End Try
    End Sub
    Private Sub CountryIDTo_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CountryIDTo.QueryPopUp
        Try
            CountryIDTo.Properties.PopulateColumns()
            CountryIDTo.Properties.Columns("CouID").Visible = False
        Catch ex As Exception
            CountryIDTo.ErrorText = "عذرا لا يوجد بيانات في هذا الحقل"
        End Try
    End Sub
    Private Sub BranchDeliveredID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchDeliveredID.QueryPopUp
        If CountryIDTo.EditValue = -1 Or CountryIDTo.Text = String.Empty Then
            BranchDeliveredID.ErrorText = "يجب إختيار الدولة اولا"
            Exit Sub
        End If
        Try
            BranchDeliveredID.Properties.PopulateColumns()
            BranchDeliveredID.Properties.Columns("DBRID").Visible = False
            BranchDeliveredID.Properties.Columns("BranchType").Visible = False
        Catch ex As Exception
            BranchDeliveredID.ErrorText = "عذرا لا يوجد بيانات في هذا الحقل"
        End Try
    End Sub
#End Region
#End Region
#Region "أكواد تعبئة أدوات الجزء الخاص بالمستلم"
    Sub LoadCountryTo()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryIDFrom", SqlDbType.Int)
        PRM(0).Value = CountryIDFrom.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CountriesTb_LoadToLKPNotExist", PRM)
        If DT.Rows.Count > 0 Then
            CountryIDTo.Properties.DataSource = DT
            CountryIDTo.Properties.ValueMember = "CouID"
            CountryIDTo.Properties.DisplayMember = "CountryName"
            CountryIDTo.Properties.ShowHeader = False
        End If
    End Sub

    Sub LoadCities()
        ToCityOrBankID.Properties.DataSource = Nothing
        ToCityOrBankID.Properties.Columns.Clear()
        ToCityOrBankID.EditValue = -1
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        PRM(0).Value = CountryIDTo.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CitiesTb_LoadToLKPBasedonCountry", PRM)
        If DT.Rows.Count > 0 Then
            ToCityOrBankID.Properties.DataSource = DT
            ToCityOrBankID.Properties.ValueMember = "CTID"
            ToCityOrBankID.Properties.DisplayMember = "CityName"
            ToCityOrBankID.Properties.ShowHeader = False
            ToCityOrBankID.Enabled = True
            ToCityOrBankID.Properties.PopulateColumns()
            HideAllColumnsExceptDisplay(ToCityOrBankID)
            'ToCityOrBankID.Properties.Columns("CTID").Visible = False
        End If
    End Sub

    Sub LoadBankTo(CounrtyID As Integer)
        ToCityOrBankID.Properties.DataSource = Nothing
        ToCityOrBankID.Properties.Columns.Clear()
        ToCityOrBankID.EditValue = -1
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryId", SqlDbType.Int)
        PRM(0).Value = CounrtyID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BanksTb_LOADTOLKP_BasedONCountryID", PRM)
        If DT.Rows.Count > 0 Then
            ToCityOrBankID.Properties.DataSource = DT
            ToCityOrBankID.Properties.ValueMember = "BNKID"
            ToCityOrBankID.Properties.DisplayMember = "BankName"
            ToCityOrBankID.Properties.ShowHeader = False
            ToCityOrBankID.Enabled = True
            ToCityOrBankID.Properties.PopulateColumns()
            'ToCityOrBankID.Properties.Columns("BNKID").Visible = False
        End If
    End Sub

    Private Sub RecievedCurrencyID_TextChanged(sender As Object, e As EventArgs) Handles RecievedCurrencyID.TextChanged
        If ConfirmType = 0 Then
            LoadDeliveredCurrency(RecievedCurrencyID.EditValue)
        End If
        If RecievedCurrencyID.Text <> String.Empty Then
            SubCurrOvarallVal.Text = Cur_Code1(RecievedCurrencyID.Text)
            SubCurrExVal.Text = Cur_Code1(RecievedCurrencyID.Text)
        Else
            SubCurrOvarallVal.Text = String.Empty
            SubCurrExVal.Text = String.Empty
        End If
    End Sub

    Private Sub IsDelivered_CheckedChanged(sender As Object, e As EventArgs) Handles IsDelivered.CheckedChanged
        Try


            If ConfirmType = 1 AndAlso IsInOrOut = 1 Then
                Dim mms As String
                If IsAccTo = 0 Then

                    mms = My.Settings.Combny_name & vbNewLine & "CODE" & ":" & Space(1) & CodeID.Text & vbNewLine & "السادة" & ":" &
                    Space(1) & RecievedName.Text & vbNewLine & "تم استلام" & ":" & Space(1) &
                    Cur_Code(RecievedCurrencyID.Text, CurrRecievedVal.EditValue, True, "n2") & vbNewLine &
                            "بسعر" & ":" & Space(1) & TransPrice.Text & vbNewLine &
                            "دخول لحسابكم مايعادل" & vbNewLine &
                            Cur_Code(DeliveredCurrencyID.Text, NetTotal.Text, True, "n2") & vbNewLine &
                            Cur_Code(DeliveredCurrencyID.Text, NetTotal.EditValue, False) & vbNewLine & "شكراً لتعاونكم معنا"
                    WATSAPPMsAG(RPhone1.Text, mms, whatsapp_contacts(RPhone1.Text))

                End If

            End If

            If IsDelivered.Checked = True Then
                If ConfirmType = 2 Then
                    GETSAFEVAL(UserAccID, BID, DefaultCurrency)
                    If SAFEVAL < CurrDeliveredVal.EditValue Then
                        ErrorMessage(Me, "رسالة تنبيه", "عذرا رصيد الخزنة غير كافي لإجراء هذه لعملية")
                        IsDelivered.Checked = False
                        Exit Sub
                    End If
                End If
                If ConfirmType = 1 And TransType.SelectedIndex = 2 Then
                    Dim PR(5) As SqlParameter
                    PR(0) = New SqlParameter("@RecievedCurrencyID", SqlDbType.BigInt) With {.Value = RecievedCurrencyID.EditValue}
                    PR(1) = New SqlParameter("@CountryIDTo", SqlDbType.BigInt) With {.Value = CountryIDTo.EditValue}
                    PR(2) = New SqlParameter("@DeliveredCurrencyID", SqlDbType.BigInt) With {.Value = DeliveredCurrencyID.EditValue}
                    PR(3) = New SqlParameter("@AccountType", SqlDbType.BigInt) With {.Value = 1}
                    PR(4) = New SqlParameter("@PriceType", SqlDbType.BigInt) With {.Value = 2}
                    PR(5) = New SqlParameter("@BranchID", SqlDbType.BigInt) With {.Value = BranchDeliveredID.EditValue}
                    Dim dtt As New DataTable
                    dtt.Clear()
                    dtt = RUN_FUNCTION_PARM("Get_CurrencyPower(@RecievedCurrencyID,@CountryIDTo,@DeliveredCurrencyID,@AccountType,@PriceType,@BranchID) AS Get_CurrencyPower", PR)
                    If dtt.Rows.Count > 0 Then
                        If dtt.Rows(0)("Get_CurrencyPower") <> 2 Then
                            If (dtt.Rows(0)("Get_CurrencyPower") = 0 And publicPrice > TransPrice.EditValue) Or (dtt.Rows(0)("Get_CurrencyPower") = 1 And publicPrice < TransPrice.EditValue) Then
                                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                                Dim lookAndFeelError As New UserLookAndFeel(Me)
                                lookAndFeelError.Style = LookAndFeelStyle.Skin
                                lookAndFeelError.UseDefaultLookAndFeel = False
                                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                                XtraMessageBox.AllowCustomLookAndFeel = True
                                Dim RESU = XtraMessageBox.Show(lookAndFeelError, Space(20) & "سعر الجمهور" & ":" & Space(1) & publicPrice & Space(5) & "وسعر الوكيل" & ":" & Space(1) & TransPrice.EditValue & vbNewLine & "بهذه الأسعار سيكون هناك خسائر للشركة هل أنت متأكد من الإعتماد؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                                If RESU = DialogResult.No Then
                                    IsDelivered.Checked = False
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If
                Dim dt As New DataTable
                dt.Clear()
                If CountryIDTo.EditValue = COUNTRYNID Then
                dt = RUN_QUARY_TXT("select ISnull(BranchType,0) as BranchType from CoBranch where ID='" & BranchDeliveredID.EditValue & "'")
                If dt.Rows.Count > 0 Then
                    RBRTYPE = dt.Rows(0)("BranchType")
                Else
                    RBRTYPE = 0
                End If
            Else
                    RBRTYPE = 0
                End If



                If ConfirmType = 1 And RBRTYPE = 3 Then
                    IsHandelExAVal = 1
                    EXternal_ExValSahreByHand.ShowDialog()
                    ExternalEx_Insert()
                    If MsgStatus = 1 Then
                        ConfirmType = 0
                        ISUpdate = 0
                        CONFIRMMESSAGE.Show()
                    Else
                        IsDelivered.Checked = False
                    End If
                Else
                    IsHandelExAVal = 0
                    ExternalEx_Insert()
                    If MsgStatus = 1 Then
                        ConfirmType = 0
                        ISUpdate = 0
                        CONFIRMMESSAGE.Show()
                    Else
                        IsDelivered.Checked = False
                    End If
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BranchDeliveredID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchDeliveredID.EditValueChanged

        If ConfirmType = 1 And TransType.SelectedIndex = 2 Then

            get_NetTotalVal()

        End If
    End Sub

    Private Sub ExVal_Leave(sender As Object, e As EventArgs) Handles ExVal.Leave

    End Sub

    Private Sub BankServiceType_EditValueChanged(sender As Object, e As EventArgs) Handles BankServiceType.EditValueChanged
        If BankServiceType.EditValue <> -1 And BankServiceType.Text <> String.Empty Then
            BBranchID.EditValue = SerDvglkp.GetFocusedRowCellValue("BankID")

        End If
    End Sub

    Sub LOADDeliveredBranchID()
        BranchDeliveredID.EditValue = -1
        If (CountryIDTo.EditValue <> -1 Or CountryIDTo.Text <> String.Empty) And ((TransType.SelectedIndex <> -1 Or TransType.Text <> String.Empty) Or (CountryIDTo.EditValue = COUNTRYNID)) Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int)
            PRM(0).Value = CountryIDTo.EditValue
            PRM(1) = New SqlParameter("@TransType", SqlDbType.Int)
            PRM(1).Value = TransType.SelectedIndex
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPWITHTransType", PRM)
            BranchDeliveredID.Properties.DataSource = DT
            BranchDeliveredID.Properties.ValueMember = "DBRID"
            BranchDeliveredID.Properties.DisplayMember = "BName"
            BranchDeliveredID.Properties.ShowHeader = False
        End If
    End Sub

    Private Sub ToCityOrBankID_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles ToCityOrBankID.ButtonClick
        If e.Button.Index = 1 Then
            If IsToBank = True Then
                FRMBANK.NEWRECORD()
                FRMBANK.CountryID.EditValue = CountryIDTo.EditValue
                FRMBANK.CountryID.Enabled = False
                FRMBANK.ShowDialog()

            Else
                FRMCities.NEWRECORD()
                FRMCities.CountriesID.EditValue = CountryIDTo.EditValue
                FRMCities.CountriesID.Enabled = False
                FRMCities.BtnNew.Enabled = False
                FRMCities.ShowDialog()
            End If
        End If
    End Sub

    Private Sub CurrRecievedVal_KeyDown(sender As Object, e As KeyEventArgs) Handles CurrRecievedVal.KeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
            e.Handled = True
        End If
    End Sub

    Private Sub CurrRecievedVal_Leave(sender As Object, e As EventArgs) Handles CurrRecievedVal.Leave
        If CurrRecievedVal.EditValue > 0.000 And ConfirmType = 0 Then
            InfoMessage(Me, "رسالة معلومات", "قيمة الحوالة المدخلة:" & Space(1) & CurrRecievedVal.EditValue & vbNewLine & "بالحروف:" & Space(1) & Cur_Code(RecievedCurrencyID.Text, CurrRecievedVal.EditValue, False, "n2"))
        End If
        If IsCash.EditValue = 2 Then
            GetServiceValue()
        End If
    End Sub
    Public Sub GetServiceValue()
        If BBranchID.EditValue = -1 Then
            MsgBox("يجب اختيار المصرف")
            CurrRecievedVal.EditValue = 0.000
            BBranchID.Select()
            Return
        End If
        If BankServiceType.EditValue = -1 Then
            MsgBox("يجب اختيار الخدمة")
            CurrRecievedVal.EditValue = 0.000
            BankServiceType.Select()
            Return
        End If
        ' قراءة القيم من الجدول
        Dim ServiceRate As Decimal = Convert.ToDecimal(SerDvglkp.GetFocusedRowCellValue("ValRate")) * CurrRecievedVal.EditValue
        Dim DifferentialVal As Decimal = Convert.ToDecimal(SerDvglkp.GetFocusedRowCellValue("DifferentialVal"))
        Dim TransVal As Decimal = Convert.ToDecimal(SerDvglkp.GetFocusedRowCellValue("TransVal"))
        If IsCash.EditValue = 2 AndAlso CurrRecievedVal.EditValue > 0D Then
            ExVal.EditValue = TransVal * CurrRecievedVal.EditValue
            BankExVAL.EditValue = ServiceRate
            Dim TempVal As Decimal = CurrRecievedVal.EditValue + BankExVAL.EditValue + ExVal.EditValue
            ServiceTotalVal.EditValue = CurrRecievedVal.EditValue + ExVal.EditValue + BankExVAL.EditValue
        End If
    End Sub

    Private Sub RecievedName_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles RecievedName.ButtonClick
        If BranchDeliveredID.EditValue = -1 Or BranchDeliveredID.Text = String.Empty Then
            RecievedName.ErrorText = "يجب اختيار الفرع المرسلة له أولاً"
            Exit Sub
        End If
        If e.Button.Index = 0 Then
            FRMSELECTACCOUNT.SendOrRec = 1
            TransAccIDTo = 0
            RecievedName.Text = ""
            RPhone1.Text = ""
            RPhone2.Text = ""
            OwnNatioNum.Text = ""
            FRMSELECTACCOUNT.ShowDialog()
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles BtnChangePrice.Click
        Dim accType As Integer
        If TransType.SelectedIndex = 0 Then
            accType = 0
        ElseIf TransType.SelectedIndex = 1 Then
            accType = 3
        ElseIf TransType.SelectedIndex = 2 Then
            accType = 1
        ElseIf TransType.SelectedIndex = 3 Then
            accType = 2
        End If
        FRMNEWCURRENCYETAILS.NEWRECORD()
        FRMNEWCURRENCYETAILS.CountryID.EditValue = CountryIDTo.EditValue
        FRMNEWCURRENCYETAILS.PriceType.SelectedIndex = 2
        FRMNEWCURRENCYETAILS.AccountType.SelectedIndex = accType
        If TransType.SelectedIndex = 2 Then
            FRMNEWCURRENCYETAILS.BranchID.EditValue = BranchDeliveredID.EditValue
            FRMNEWCURRENCYETAILS.BRID = BranchDeliveredID.EditValue
        End If
        FRMNEWCURRENCYETAILS.ShowDialog()
        BranchDeliveredID.EditValue = -1
    End Sub

    Private Sub TransPrice1_EditValueChanged(sender As Object, e As EventArgs) Handles TransPrice1.EditValueChanged, TransPrice1.TextChanged
        If IsEmpty(DeliveredCurrencyID) Then Exit Sub
        If IsEmpty(TransPrice1) Then Exit Sub

        If ConfirmType = 0 Then
                If CurrRecievedVal.EditValue > 0 Then
                    If GetLKPColumnVal(DeliveredCurrencyID, "CurrencyPower") = False Then
                        NetFinalTotal = CurrRecievedVal.EditValue * TransPrice1.EditValue
                    Else
                        NetFinalTotal = CurrRecievedVal.EditValue / TransPrice1.EditValue
                    End If
                    CurrDeliveredVal.EditValue = NetFinalTotal
                Else
                    NetTotal.Text = 0.00
                    CurrDeliveredVal.EditValue = 0.00
                    CurrRecievedVal.ErrorText = "الرجاء ادخال القيمة الاولى "
                End If

                If CountryIDFrom.EditValue = COUNTRYNID Then
                    GetServiceVal()
                Else
                    SerExVal = 0.000
                End If

                If SerExVal > 0 Then
                    FinalVal = NetFinalTotal - SerExVal
                    NetTotal.Text = Convert.ToString(FinalVal)
                    ExValTotal.Text = Cur_Code(DeliveredCurrencyID.Text, SerExVal + ExtraVal.EditValue, True)
                Else
                    NetTotal.Text = NetFinalTotal
                    ExValTotal.Text = Cur_Code(DeliveredCurrencyID.Text, 0.000, True)
                End If
            End If

    End Sub

    Sub CheckService()
        LoadCities()
        ServiceTransID.Enabled = False
        ServiceExVal.Text = 0.000
        SerExVal = 0.000
        IsToBank = False
        If ServiceType.EditValue <> -1 Or ServiceType.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@TransID", SqlDbType.Int) With {.Value = ServiceType.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("TransTypeTb_CheckService", PR)
            If DT.Rows.Count > 0 Then
                If IsDBNull(DT.Rows(0)("SRID")) = False Then
                    ServiceTransID.Enabled = True
                    IsServiceVal = True
                    LayoutControlItem10.Text = "الوجهة"
                Else
                    If DT.Rows(0)("IsBank") = True Then
                        ServiceTransID.Enabled = False
                        ServiceTransID.SelectedIndex = 0
                        IsServiceVal = True
                        IsToBank = True
                        If ConfirmType = 0 And CountryIDFrom.EditValue = COUNTRYNID Then
                            LayoutControlItem10.Text = "اسم المصرف"
                            LoadBankTo(CountryIDTo.EditValue)
                        End If
                    Else
                        LayoutControlItem10.Text = "الوجهة"
                        ServiceTransID.SelectedIndex = 1
                        ServiceTransID.Enabled = False
                        ServiceExVal.Text = 0.000
                        SerExVal = 0.000
                        IsServiceVal = False
                    End If
                End If
            Else
                LayoutControlItem10.Text = "الوجهة"
                ServiceTransID.SelectedIndex = 1
                ServiceTransID.Enabled = False
                ServiceExVal.Text = 0.000
                SerExVal = 0.000
                IsServiceVal = False
            End If
        End If
        If (ServiceType.EditValue = 1 Or ServiceType.EditValue = 2 Or ServiceType.EditValue = 9) Then
            ToCityOrBankID.EditValue = 4
            ToCityOrBankID.Enabled = False
        Else
            ToCityOrBankID.Enabled = True
        End If
    End Sub
    Sub GetServiceVal()
        ServiceExVal.EditValue = 0.000
        If NetFinalTotal > 0 Then
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@ServiceID", SqlDbType.Int) With {.Value = ServiceType.EditValue}
            PR(1) = New SqlParameter("@BetweenVal", SqlDbType.Decimal) With {.Value = NetFinalTotal}
            PR(2) = New SqlParameter("@IsHasEXVal", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PR(3) = New SqlParameter("@SerExVal", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CATEGORYTYPESDETAILSTB_GET_ServiceExVal", PR)
            If DT.Rows.Count > 0 Then
                If PR(2).Value = 1 Then
                    If ServiceTransID.SelectedIndex = 1 Then
                        ServiceExVal.Text = Cur_Code(DeliveredCurrencyID.Text, PR(3).Value, True)
                        SerExVal = Convert.ToDecimal(PR(3).Value)
                    Else
                        ServiceExVal.Text = "0.000"
                        ExValTotal.Text = "0.000"
                        SerExVal = 0.000
                    End If
                Else
                    SerExVal = 0.000
                End If
            End If
        End If
    End Sub

    Private Sub ServiceType_EditValueChanged(sender As Object, e As EventArgs) Handles ServiceType.EditValueChanged
        IsToBank = False
        NetTotal.EditValue = CurrDeliveredVal.EditValue
        ExValTotal.EditValue = 0.00

        If ServiceType.Enabled = True Then
            CheckService()
        End If
        If ConfirmType = 0 Then
            SerExVal = 0.000
            get_NetTotalVal()
        End If
    End Sub

    Private Sub CurrRecievedVal_EditValueChanged(sender As Object, e As EventArgs) Handles CurrRecievedVal.EditValueChanged
        If ConfirmType = 0 Then
            SerExVal = 0.000
            get_NetTotalVal()
        End If
    End Sub

    Private Sub NetTotal_TextChanged(sender As Object, e As EventArgs) Handles NetTotal.TextChanged
        Dim NetVal As Decimal = Convert.ToDecimal(NetTotal.Text)
    End Sub

    Private Sub ExVal_EditValueChanged(sender As Object, e As EventArgs) Handles ExVal.EditValueChanged
        If ConfirmType = 0 Then
            get_NetTotalVal()
        End If
    End Sub

    Sub LoadServiceType(CountryID As Integer)
        ServiceType.Properties.DataSource = Nothing
        ServiceType.EditValue = -1
        If ConfirmType = 0 Then
            If CountryIDTo.EditValue = -1 And CountryIDTo.Text = String.Empty Then
                CountryIDTo.ErrorText = "الرجاء اختيار الدولة"
                ServiceType.Enabled = False
                IsServiceVal = False
                Exit Sub
            End If
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TransTypeTb_SelectByCountry", PR)
        If DT.Rows.Count > 0 Then
            ServiceType.Properties.DataSource = DT
            ServiceType.Properties.ValueMember = "SRID"
            ServiceType.Properties.DisplayMember = "SRNAME"
            ServiceType.Enabled = True
            CheckService()
            'If ServiceTransID.Enabled = True Then
            '    If ServiceTransID.SelectedIndex = 0 Then
            '        IsServiceVal = False

            '    ElseIf ServiceTransID.SelectedIndex = 1 Then
            '        IsServiceVal = True
            '    End If
            'End If
        Else
            ServiceType.Enabled = False
            ServiceTransID.Enabled = False
            IsServiceVal = False
        End If
    End Sub
    Sub LoadDeliveredCurrency(CurrRecifed As Integer)
        If ConfirmType = 0 Then
            If RecievedCurrencyID.EditValue = -1 Or RecievedCurrencyID.Text = String.Empty Then
                RecievedCurrencyID.ErrorText = "يجب إختيار العملة الأولى أولا"
                DeliveredCurrencyID.Properties.DataSource = Nothing
                Exit Sub
            End If
        End If
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CurrRecifed}
        LoadToControlar(DeliveredCurrencyID, "CurrencyMainTb_LOADTOLKPNOTEXISTPRO", "CuName", "ID", PRM)
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOLKPNOTEXIST", PRM)
        'If DT.Rows.Count > 0 Then
        '    DeliveredCurrencyID.Properties.DataSource = DT
        '    DeliveredCurrencyID.Properties.ValueMember = "ID"
        '    DeliveredCurrencyID.Properties.DisplayMember = "CuName"
        '    DeliveredCurrencyID.Properties.ShowHeader = False
        'End If

    End Sub
#End Region

    Public Sub LoadCurrencyByCountry(CountryID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("Get_CurrencyIDByCountryID", PRM)
        If DT.Rows.Count > 0 Then
            CurrencyByCountry = DT.Rows(0)("CurrID")
        End If
    End Sub

#Region "Operations Code"
    Public Sub InternalEx_MaxID()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@typID", SqlDbType.Int) With {.Value = 2}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchRecievedID.EditValue}
        PRM(2) = New SqlParameter("@USRID", SqlDbType.Int) With {.Value = UserID}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryIDFrom.EditValue}
        PRM(4) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = CITYID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ExternalEx_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            CodeID.Text = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Function GetRecord() As DataTable
        Dim DT As New DataTable
        Return DT
    End Function

    Public Sub ExternalEx_Insert()

        If ConfirmType = 0 Then
            If CountryIDFrom.EditValue = -1 Or CountryIDFrom.Text = String.Empty Then
                CountryIDFrom.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If IsCash.EditValue = -1 Or IsCash.Text = String.Empty Then
                IsCash.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If IsCash.EditValue = 2 Then
                If BBranchID.EditValue = -1 Or BBranchID.Text = String.Empty Then
                    BBranchID.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                    MsgStatus = 0
                    Exit Sub
                End If
                If BankServiceType.EditValue = -1 Or BankServiceType.Text = String.Empty Then
                    BankServiceType.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                    MsgStatus = 0
                    Exit Sub
                End If
                If ServiceTotalVal.EditValue <= 0 Then
                    ServiceTotalVal.ErrorText = "عذرا صافي القيمة لايجب أن يكون صفر"
                    MsgStatus = 0
                    Exit Sub
                End If
            End If
            If IsCash.EditValue = 1 Then
                If IsAccFrom = 0 Then
                    SenderName.ErrorText = "يجب اختيار الحساب"
                    MsgStatus = 0
                    Exit Sub
                End If
            End If
            If BranchRecievedID.EditValue = -1 Or BranchRecievedID.Text = String.Empty Then
                BranchRecievedID.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If RecievedCurrencyID.EditValue = -1 Or RecievedCurrencyID.Text = String.Empty Then
                RecievedCurrencyID.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If SenderName.Text = String.Empty Then
                SenderName.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If DeliveredCurrencyID.EditValue = -1 Or DeliveredCurrencyID.Text = String.Empty Then
                DeliveredCurrencyID.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If RecievedName.Text = String.Empty Then
                RecievedName.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If RPhone1.Text = String.Empty Then
                RPhone1.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If CurrRecievedVal.EditValue <= 0 Or CurrRecievedVal.Text = String.Empty Then
                CurrRecievedVal.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If NetTotal.EditValue <= 0 Or NetTotal.Text = String.Empty Then
                NetTotal.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If ServiceTransID.SelectedIndex = 0 And CountryIDTo.EditValue <> COUNTRYNID Then
                If OwnAccNo.Text = "" Then
                    OwnAccNo.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                    MsgStatus = 0
                    Exit Sub
                End If
                If OwnNatioNum.Text = "" And CountryIDTo.EditValue <> COUNTRYNID Then
                    OwnNatioNum.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                    MsgStatus = 0
                    Exit Sub
                End If
            End If
            If CountryIDFrom.EditValue = COUNTRYNID Then
                If TransType.SelectedIndex = -1 Or TransType.Text = String.Empty Then
                    TransType.ErrorText = "عذرا يجب أن يتم أختيار نوع التحويل على الوكيل العام"
                    MsgStatus = 0
                    Exit Sub
                End If
                If ServiceType.Enabled = True Then
                    If ServiceTransID.SelectedIndex = -1 Or ServiceTransID.Text = String.Empty Then
                        ServiceTransID.ErrorText = "عذرا يجب أن يتم إختيار نوع آلية التسليم"
                        MsgStatus = 0
                        Exit Sub
                    End If
                End If
                If IsCash.EditValue = 1 Then
                    FRMCODEPYMENT_em_cu2.lodeDate("حوالة مالية داخلية", SenderName.Text, AccFrom, CurrRecievedVal.EditValue, RecievedCurrencyID.Text, SPhone1.Text, 2, "")
                    FRMCODEPYMENT_em_cu2.ShowDialog()
                    If FRMCODEPYMENT_em_cu2.chick = False Then
                        ErrorMessage(Me, "رسالة تنبية", "عذرا رقم الكود خطأ الرجاء اعادة المحاولة")
                        Exit Sub
                        Return
                    End If
                End If
                If TransPrice.EditValue <> TransPrice1.EditValue Then
                    If GetLKPColumnVal(DeliveredCurrencyID, "CurrencyPower") = True And (TransPrice1.EditValue < TransPrice.EditValue Or TransPrice1.EditValue > (TransPrice.EditValue + 0.05)) Then
                        ErrorMessage(Me, "رسالة تنبية", "عذرا سعر التحويل للفرع لايمكن أن يكون أصغر من سعر الجمهور ولا يمكن أن يكون فرق السعر أكبر من 0.05")
                        MsgStatus = 0
                        Exit Sub
                    End If
                    If GetLKPColumnVal(DeliveredCurrencyID, "CurrencyPower") = False And (TransPrice1.EditValue > TransPrice.EditValue Or TransPrice1.EditValue < (TransPrice.EditValue - 0.05)) Then
                        ErrorMessage(Me, "رسالة تنبية", "عذرا سعر التحويل للفرع لايمكن أن يكون أكبر من سعر الجمهور ولا يمكن أن يكون فرق السعر أكبر من 0.05")
                        MsgStatus = 0
                        Exit Sub
                    End If
                End If
                ElseIf CountryIDTo.EditValue = COUNTRYNID Then
                If BranchDeliveredID.EditValue = -1 Or BranchDeliveredID.Text = String.Empty Then
                    BranchDeliveredID.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                    MsgStatus = 0
                    Exit Sub
                End If
            End If
            If ExVal.EditValue = 0.000 And BID <> MAINBID And TransPrice1.EditValue = TransPrice.EditValue Then
                ExVal.ErrorText = "يرجى إدخال العمولة"
                Exit Sub
            End If
        End If  '' شروط التحقق قبل عملية الحفظ
        If ConfirmType = 1 Then
            If TransType.SelectedIndex = 1 Then
                ErrorMessage(Me, "رسالة خطأ", "عذرا لم يتم الإعتماد يجب إختيار نوع التحويل أولا")
                MsgStatus = 0
                Exit Sub
            End If
            If BranchDeliveredID.EditValue = -1 Or BranchDeliveredID.Text = String.Empty Then
                BranchDeliveredID.ErrorText = "هذا الحقل لا يمكن أن يكون فارغ"
                MsgStatus = 0
                Exit Sub
            End If
            If TransType.SelectedIndex = 0 Then
                GETSAFECurrVAL(BranchDeliveredID.EditValue, MAINBID, DeliveredCurrencyID.EditValue)
                If SAFEVAL < CurrDeliveredVal.EditValue Then
                    ErrorMessage(Me, "رسالة خطأ", "عذرا القيمة المراد تحويلها لا يمكن تغطيتها حاليا الرجاء إختيار وكيل او الإتصال بالإدارة")
                    MsgStatus = 0
                    Exit Sub
                End If
            End If
        End If '' شروط التحقق قبل عملية الإعتماد
        If ServiceTransID.SelectedIndex = 0 Then
            IsPrivateAccount = True
        ElseIf ServiceTransID.SelectedIndex = 1 Then
            IsPrivateAccount = False
        End If

        If CountryIDFrom.EditValue = COUNTRYNID Then
            IsInOrOut = 0
        ElseIf CountryIDTo.EditValue = COUNTRYNID Then
            IsInOrOut = 1
        Else
            IsInOrOut = 2
        End If
        If ConfirmType = 0 Then
        End If

        Dim PRM(61) As SqlParameter
        PRM(0) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = IDCode}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text}
        PRM(2) = New SqlParameter("@SenderName", SqlDbType.NVarChar, 50) With {.Value = SenderName.Text.Trim}
        PRM(3) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, 50) With {.Value = SPhone1.Text.Trim}
        PRM(4) = New SqlParameter("@SPhone2", SqlDbType.NVarChar, 50) With {.Value = SPhone2.Text.Trim}
        PRM(5) = New SqlParameter("@SenderIDNo", SqlDbType.NVarChar, 50) With {.Value = SenderIDNo.Text.Trim}
        PRM(6) = New SqlParameter("@RecievedCurrencyID", SqlDbType.Int) With {.Value = RecievedCurrencyID.EditValue}
        PRM(7) = New SqlParameter("@CountryIDFrom", SqlDbType.Int) With {.Value = CountryIDFrom.EditValue}
        PRM(8) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID.EditValue}
        PRM(9) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, 50) With {.Value = RecievedName.Text.Trim}
        PRM(10) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, 50) With {.Value = RPhone1.Text.Trim}
        PRM(11) = New SqlParameter("@RPhone2", SqlDbType.NVarChar, 50) With {.Value = RPhone2.Text.Trim}
        If IsToBank = False Then
            PRM(12) = New SqlParameter("@CityIDTo", SqlDbType.Int) With {.Value = ToCityOrBankID.EditValue}
        Else
            PRM(12) = New SqlParameter("@CityIDTo", SqlDbType.Int) With {.Value = 0}
        End If
        PRM(13) = New SqlParameter("@DeliveredCurrencyID", SqlDbType.Int) With {.Value = DeliveredCurrencyID.EditValue}
        PRM(14) = New SqlParameter("@CountryIDTo", SqlDbType.Int) With {.Value = CountryIDTo.EditValue}
        PRM(15) = New SqlParameter("@AgentCheck", SqlDbType.Int) With {.Value = TransType.SelectedIndex}
        PRM(16) = New SqlParameter("@IsPrivateAccount", SqlDbType.Bit) With {.Value = IsPrivateAccount}
        PRM(17) = New SqlParameter("@OwnNatioNum", SqlDbType.NVarChar, 50) With {.Value = OwnNatioNum.Text.Trim}
        PRM(18) = New SqlParameter("@OwnAccNo", SqlDbType.VarChar, 50) With {.Value = OwnAccNo.Text.Trim}
        PRM(19) = New SqlParameter("@ServiceType", SqlDbType.Int) With {.Value = ServiceType.EditValue}
        PRM(20) = New SqlParameter("@IsServiceVal", SqlDbType.Bit) With {.Value = IsServiceVal}
        PRM(21) = New SqlParameter("@ServiceExVal", SqlDbType.Decimal) With {.Value = SerExVal}
        If TransType.SelectedIndex = 2 Or ConfirmType = 0 Or ConfirmType = 2 Or ConfirmType = 3 Or RBRTYPE = 3 Or (ConfirmType = 1 And IsInOrOut = 1) Then
            PRM(22) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = BranchDeliveredID.EditValue}
        Else
            PRM(22) = New SqlParameter("@BranchDeliveredID", SqlDbType.Int) With {.Value = 0}
        End If
        PRM(23) = New SqlParameter("@CurrRecievedVal", SqlDbType.Decimal) With {.Value = CurrRecievedVal.EditValue}
        PRM(24) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal.EditValue}
        PRM(25) = New SqlParameter("@ExtraVal", SqlDbType.Decimal) With {.Value = ExtraVal.EditValue}
        PRM(26) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = NetTotal.EditValue}
        PRM(27) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(28) = New SqlParameter("@SafeRecievedID", SqlDbType.Int) With {.Value = UserID}
        PRM(29) = New SqlParameter("@SafeDeliveredID", SqlDbType.Int) With {.Value = UserID}
        PRM(30) = New SqlParameter("@RecievedDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(31) = New SqlParameter("@IsDelivered", SqlDbType.Int) With {.Value = ISUpdate}
        PRM(32) = New SqlParameter("@IsAccFrom", SqlDbType.Int) With {.Value = IsAccFrom}
        PRM(33) = New SqlParameter("@AccFID", SqlDbType.BigInt) With {.Value = AccFrom}
        PRM(34) = New SqlParameter("@IsAccTo", SqlDbType.Int) With {.Value = IsAccTo}
        PRM(35) = New SqlParameter("@TransAccIDTo", SqlDbType.BigInt) With {.Value = TransAccIDTo}
        PRM(36) = New SqlParameter("@IsCash", SqlDbType.Int) With {.Value = IsCash.EditValue}
        PRM(37) = New SqlParameter("@TransPrice", SqlDbType.Decimal) With {.Value = TransPrice.EditValue}
        PRM(38) = New SqlParameter("@BBranchAccID", SqlDbType.BigInt) With {.Value = If((BankServiceType.EditValue > 0 And ConfirmType = 0), BankServiceType.EditValue, 0)}
        PRM(39) = New SqlParameter("@BBRANCHID", SqlDbType.Int) With {.Value = If(BankServiceType.EditValue > 0 And ConfirmType = 0, SerDvglkp.GetFocusedRowCellValue("BBranchID"), 0)}
        PRM(40) = New SqlParameter("@EXTRVAL", SqlDbType.Decimal) With {.Value = If(BankServiceType.EditValue > 0 And ConfirmType = 0, BankExVAL.EditValue, 0)}
        PRM(41) = New SqlParameter("@BankServiceType", SqlDbType.Int) With {.Value = If(BankServiceType.EditValue > 0 And ConfirmType = 0, SerDvglkp.GetFocusedRowCellValue("SID"), 0)}
        PRM(42) = New SqlParameter("@EMPCUSTSELECT", SqlDbType.Int) With {.Value = EMPCUSTSELECT}
        PRM(43) = New SqlParameter("@EMPTOSELECT", SqlDbType.Int) With {.Value = ectypeto}
        PRM(44) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Notes.Text.Trim}
        PRM(45) = New SqlParameter("@msgIN", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(46) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(47) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes.Text.Trim}
        If IsInOrOut = 0 Then
            PRM(48) = New SqlParameter("@BranchRecievedName", SqlDbType.NVarChar, -1) With {.Value = "حـ خارجية " & Space(1) & BranchRecievedID.Text}
        End If
        If IsInOrOut = 1 Then
            PRM(48) = New SqlParameter("@BranchRecievedName", SqlDbType.NVarChar, -1) With {.Value = "حـ خارجية " & Space(1) & SenderName.Text.Trim}
        End If
        PRM(49) = New SqlParameter("@IsInOrOut", SqlDbType.TinyInt) With {.Value = IsInOrOut}
        PRM(50) = New SqlParameter("@ConfirmedType", SqlDbType.Int) With {.Value = ConfirmType}
        PRM(51) = New SqlParameter("@NewTrancPrice", SqlDbType.Decimal) With {.Value = Convert.ToDecimal(TransPrice.EditValue)}
        PRM(52) = New SqlParameter("@NewFinalTotal", SqlDbType.Decimal) With {.Value = Convert.ToDecimal(NetTotal.EditValue)}
        If TransType.SelectedIndex = 0 Then
            PRM(53) = New SqlParameter("@OurAccID", SqlDbType.BigInt) With {.Value = BranchDeliveredID.EditValue}
        Else
            PRM(53) = New SqlParameter("@OurAccID", SqlDbType.BigInt) With {.Value = 0}
        End If
        PRM(54) = New SqlParameter("@ConfirmedSafeID", SqlDbType.Int) With {.Value = UserID}
        PRM(55) = New SqlParameter("@ConfirmDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(56) = New SqlParameter("@CurrDeliveredVal", SqlDbType.Decimal) With {.Value = Convert.ToDecimal(CurrDeliveredVal.EditValue)}
        PRM(57) = New SqlParameter("@IsHandelExAVal", SqlDbType.Bit) With {.Value = IsHandelExAVal}
        PRM(58) = New SqlParameter("@HandelExAVal", SqlDbType.Decimal) With {.Value = Convert.ToDecimal(HandelExAVal)}
        If IsToBank = True Then
            PRM(59) = New SqlParameter("@BankIDTo", SqlDbType.Int) With {.Value = ToCityOrBankID.EditValue}
        Else
            PRM(59) = New SqlParameter("@BankIDTo", SqlDbType.Int) With {.Value = 0}
        End If
        PRM(60) = New SqlParameter("@NewPrice_SenDForWhatsapp", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(61) = New SqlParameter("@TransPrice1", SqlDbType.Decimal) With {.Value = TransPrice1.EditValue}
        RUN_EXUTE_PRO("ExternalEx_Insert", PRM)
        If PRM(45).Value = 0 Then
            ErrorMessage(Me, "رسالة تنبيه", PRM(46).Value.ToString)
            InternalEx_MaxID()
            Me.MsgStatus = 0
        ElseIf PRM(45).Value = 2 Then
            ErrorMessage(Me, "رسالة تنبيه", PRM(46).Value.ToString)
            Me.MsgStatus = 0
            Me.Close()
        Else
            Me.MsgStatus = 1
            If ConfirmType = 1 And TransType.SelectedIndex = 2 Then



            End If
            If ConfirmType = 0 And TransType.SelectedIndex = 1 And IsInOrOut = 0 And BranchRecievedID.EditValue <> 1 Then

            End If
            If ConfirmType = 2 Or ConfirmType = 0 Or ConfirmType = 3 Then
                Me.Print()
            End If
            If ConfirmType <> 3 Then
                BuildTransferMessage(ConfirmType, CodeID.Text)
            End If
            Me.NewRecord()
            CountryIDFrom.EditValue = -1
            CountryIDFrom.EditValue = COUNTRYNID
            IsDelivered.Enabled = False
            IsDelivered.Checked = False
        End If
    End Sub
    Private Sub ExtraVal_EditValueChanged(sender As Object, e As EventArgs) Handles ExtraVal.EditValueChanged
        Dim ExVal As Decimal
        If ExtraVal.EditValue > 0 Then
            ExVal = NetTotal.EditValue - ExtraVal.EditValue
            NetTotal.EditValue = ExVal
            ExValTotal.Text = Cur_Code(DeliveredCurrencyID.Text, SerExVal + ExtraVal.EditValue, True)
        Else
            NetTotal.Text = NetFinalTotal - SerExVal
            ExValTotal.Text = Cur_Code(DeliveredCurrencyID.Text, SerExVal, True)
        End If
    End Sub

    Private Sub ServiceType_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ServiceType.QueryPopUp
        If CountryIDTo.EditValue <> -1 And CountryIDTo.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryIDTo.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("TransTypeTb_SelectByCountry", PR)
            If DT.Rows.Count > 0 Then
                ServiceType.Properties.PopulateColumns()
                ServiceType.Properties.Columns("SRID").Visible = False
                ServiceType.Properties.Columns("DisConstant").Visible = False
            End If
        End If
    End Sub
#End Region
    Public Sub SHOW_RECORD(x)
        LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = x}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExternalEx_LoadRecordToConfirm", PR)
        If DT.Rows.Count > 0 Then
            LoadReceivedCurrency()
            LOADBRANCHSERVICES()
            CodeID.Text = DT.Rows(0)("Code").ToString
            InsertDate.EditValue = DT.Rows(0)("InsertDate")
            CountryIDFrom.EditValue = DT.Rows(0)("CountryIDFrom")
            IsCash.EditValue = DT.Rows(0)("IsCash")
            BBranchID.EditValue = DT.Rows(0)("BBRANCHID")
            BankServiceType.EditValue = DT.Rows(0)("BankServiceType")
            BankExVAL.EditValue = DT.Rows(0)("BankExVAL")
            BranchRecievedID.EditValue = DT.Rows(0)("RecievedBranchID")
            RecievedCurrencyID.EditValue = DT.Rows(0)("RecievedCurrencyID")
            SenderName.Text = DT.Rows(0)("SenderName").ToString
            SenderIDNo.Text = DT.Rows(0)("SIDNo").ToString
            SPhone1.Text = DT.Rows(0)("Phone1").ToString
            SPhone2.Text = DT.Rows(0)("Phone2").ToString
            CountryIDTo.EditValue = DT.Rows(0)("CountryIDTo")
            TransType.SelectedIndex = DT.Rows(0)("TransType")
            TransPrice1.EditValue = DT.Rows(0)("TransPrice1")
            'If CountryIDTo.EditValue = COUNTRYNID Then
            LOADDeliveredBranchID()
            'End If
            BranchDeliveredID.EditValue = DT.Rows(0)("BranchDeleviredID")
            RecievedName.Text = DT.Rows(0)("RecievedName").ToString
            ToCityOrBankID.EditValue = DT.Rows(0)("CityIDTo")
            RPhone1.Text = DT.Rows(0)("RPhone1").ToString
            RPhone2.Text = DT.Rows(0)("RPhone2").ToString

            LoadDeliveredCurrency(DT.Rows(0)("RecievedCurrencyID"))
            LoadServiceType(DT.Rows(0)("CountryIDTo"))
            OwnNatioNum.Text = DT.Rows(0)("OwnNatioNum").ToString
            CurrRecievedVal.EditValue = DT.Rows(0)("CurrRecievedVal")
            ExVal.EditValue = DT.Rows(0)("ExVal")
            ServiceExVal.EditValue = DT.Rows(0)("ServiceExVal")
            ExtraVal.EditValue = DT.Rows(0)("ExtraVal")
            TransPrice.EditValue = DT.Rows(0)("TransPrice")
            publicPrice = DT.Rows(0)("TransPrice")

            Notes.EditValue = DT.Rows(0)("Notes").ToString
            DeliveredCurrencyID.EditValue = DT.Rows(0)("ID")
            ServiceType.EditValue = DT.Rows(0)("ServiceType")
            CurrDeliveredVal.EditValue = DT.Rows(0)("CurrDeliveredVal")
            If DT.Rows(0)("ServiceType") = 4 Then
                LoadBankTo(DT.Rows(0)("CountryIDTo"))
            Else
                LoadCities()
            End If
            ToCityOrBankID.EditValue = DT.Rows(0)("CityIDTo_lode")
            EnabledCTRL(False)
            IsDelivered.Enabled = True
            TransPrice1.Enabled = False
            IsCash.Enabled = False
            OwnAccNo.Enabled = False
            If ConfirmType = 1 Then
                IsDelivered.Text = "تأكيد الإعتماد"
                If UserType = 1 Or UserType = 3 Then
                    LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Else
                    LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                End If
                If DT.Rows(0)("CountryIDTo") <> COUNTRYNID Then
                    TransType.Enabled = True
                End If
            End If
            If ConfirmType = 2 Then
                IsDelivered.Text = "تأكيد التسليم"
                LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If
            If ConfirmType = 3 Then
                IsDelivered.Text = "تأكيد الإلغاء"
                LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If
            If DT.Rows(0)("IsPrivateAccount") = True Then
                ServiceTransID.SelectedIndex = 0
            Else
                ServiceTransID.SelectedIndex = 1
            End If
            If ConfirmType = 4 Then
                Me.Text = DT.Rows(0)("UnameR")
                LayoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Else
                Me.Text = "تحويل خارجي"
            End If
            NetTotal.EditValue = DT.Rows(0)("NetTotal")
            OwnAccNo.Text = DT.Rows(0)("OwnAccNo").ToString
            SubCurrNetTotal.Text = Cur_Code1(DT.Rows(0)("DelCurName").ToString)
            SubCurrExValTotal.Text = Cur_Code1(DT.Rows(0)("DelCurName").ToString)
            SubCurrDeliveredVal.Text = Cur_Code1(DT.Rows(0)("DelCurName").ToString)
            If DT.Rows(0)("IsCash") = 2 Then
                ServiceTotalVal.EditValue = DT.Rows(0)("CurrRecievedVal") + DT.Rows(0)("ExVal") + DT.Rows(0)("BankExVAL")
            End If
        End If
    End Sub
    Private Sub FRMEXTERNALTRANS_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        NewRecord()
        ISUpdate = 0
        ConfirmType = 0
    End Sub

    Private Sub ServiceTransID_EditValueChanged(sender As Object, e As EventArgs) Handles ServiceTransID.EditValueChanged
        OwnAccNo.Text = ""
        'OwnNatioNum.Text = ""
        If ConfirmType = 0 Then
            If ServiceTransID.SelectedIndex = 0 Then
                OwnAccNo.Enabled = True
            Else
                OwnAccNo.Enabled = False
            End If
        End If
        'If ServiceTransID.Text <> "حوالـــــــــــــــــه" Then
        CurrRecievedVal_EditValueChanged(Nothing, Nothing)
        'ServiceExVal.Text = "0.000"
        'ExValTotal.Text = "0.000"
        'SerExVal = 0.000
        'End If
    End Sub

    Private Sub ServiceType_TextChanged(sender As Object, e As EventArgs) Handles ServiceType.TextChanged
        If ServiceType.Enabled = True Then
            CheckService()
        End If
    End Sub

    Private Sub DeliveredCurrencyID_EditValueChanged(sender As Object, e As EventArgs) Handles DeliveredCurrencyID.EditValueChanged
        If DeliveredCurrencyID.Text <> String.Empty Then
            SubCurrNetTotal.Text = Cur_Code1(DeliveredCurrencyID.Text)
            SubCurrExValTotal.Text = Cur_Code1(DeliveredCurrencyID.Text)
            SubCurrDeliveredVal.Text = Cur_Code1(DeliveredCurrencyID.Text)
        Else
            SubCurrNetTotal.Text = String.Empty
            SubCurrExValTotal.Text = String.Empty
            SubCurrDeliveredVal.Text = String.Empty
        End If
    End Sub
    Private Sub CodeID_Enter(sender As Object, e As EventArgs) Handles CodeID.Enter
        If ISUpdate = False And BtnSave.Enabled = True Then
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
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub

    Private Sub ExVal_KeyDown(sender As Object, e As KeyEventArgs) Handles ExVal.KeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
            e.Handled = True
        End If
    End Sub

    Private Sub ExtraVal_KeyDown(sender As Object, e As KeyEventArgs) Handles ExtraVal.KeyDown
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
            e.Handled = True
        End If
    End Sub
End Class
