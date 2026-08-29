Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSplashScreen
Public Class FRMEMPWITHDRAWAL
    Public IDCode As ULong, TYPEs As Integer
    Public LOADTYPE, EMPID As Integer
    Public IsUpdate, CanChangeSafe, CanDebit As Boolean

    Public isbanck As Int64
    Public Frmid As Integer
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(Frmid, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)
        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            SafeID.Enabled = dt.Rows(0)("Can_safID")
            SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            SafeID.Enabled = False
            SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub

    Sub ENAPLEDCONTROLS()
        Enable_Controls(Me, True)
        WDCode.Enabled = False
        WithdrawalValue.Enabled = False
        WithdrawalDate.Enabled = False
    End Sub
    Sub NEWRECORD()
        New_Controlrs(Me)
        BranchID.EditValue = -1
        EMPORCUSTWITHDRAWALTB_MaxID(LOADTYPE)
        IsUpdate = False
        ENAPLEDCONTROLS()
        WithdrawalDate.EditValue = Date.Now
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit2", "BName", "DBRID", Nothing)
        BranchID.Select()
        BtnSave.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        LOADCIDFROM(TYPEs)
        If TYPEs = 1 Then
            CurrencyFrom.EditValue = DefaultCurrency
            CurrencyFrom.Enabled = False
        Else
            CurrencyFrom.EditValue = -1
            CurrencyFrom.Enabled = True
        End If
        If LOADTYPE = 5 Then
            LayoutControlItem12.Text = "صرف لصالح"
        Else
            LayoutControlItem12.Text = "قبض من"
        End If
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, Frmid)



        BranchID_TextChanged(Nothing, Nothing)
        If isbanck = 6 Then
            AccountType.EditValue = 20101401
            AccountType.Enabled = False
            WithdrawalFrom.EditValue = 2262
            WithdrawalFrom.Enabled = False

        Else
            AccountType.EditValue = -1
            AccountType.Enabled = True
            WithdrawalFrom.Enabled = True
            WithdrawalFrom.EditValue = -1
        End If
    End Sub
    Sub LOADSafeID()
        SafeID.Properties.DataSource = Nothing
        If Not String.IsNullOrWhiteSpace(BranchID.Text) Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@TYPEs", SqlDbType.Int) With {.Value = SafeToInt(CurrencyFrom.EditValue)}
            LoadToControlar(SafeID, "AccountsTb_LoadEMPSafeToLKPHasValORNOT", "UNAME", "AccID", PR)
        End If
    End Sub
    Private Sub FRMEMPWITHDRAWAL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserType <> 1 Then
            If LOADTYPE = 5 Then
                GETSAFEVAL(UserAccID, BID, DefaultCurrency)
                If SAFEVAL <= 0 Then
                    ErrorMessage(Me, "رسالة تنبيه", "عذرا لا يمكن فتح هذه الشاشة لعدم وجود رصيد في الخزنة")
                    Me.Close()
                    Exit Sub
                End If
            End If
        End If
        NEWRECORD()
        lodePreportes()
    End Sub

    Private Sub WithdrawalFrom_TextChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.TextChanged
        If WithdrawalFrom.Text <> String.Empty Then
            WithdrawalValue.Text = ""
            WithdrawalValue.Text = GetLKPColumnVal(WithdrawalFrom, "ShowAccVal")
        End If
    End Sub
    Sub LOADCIDFROM(TYPE As Integer)
        Dim DT As New DataTable
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = TYPE}
        LoadToControlar(CurrencyFrom, "[CurrencyMainTb_LOADTOLKP_buk]", "CuName", "ID", prm)
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Dim mms As String

#Region "save,update ...etc"
    Public Function SERACH_EMPORCUSTWITHDRAWALTB(Code As String, TypeID As Int32) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_SelectByCode", PRM)
        Return DT
    End Function
    Sub SHOW_EMCUSCODE(x, s)
        Try
            If Me.IsUpdate = True Then
                LOADSafeID()
                Dim DT As New DataTable
                DT.Clear()
                DT = SERACH_EMPORCUSTWITHDRAWALTB(x, s)
                If DT.Rows.Count > 0 Then
                    WDCode.Text = DT.Rows(0)("Code").ToString
                    BranchID.EditValue = DT.Rows(0)("BranchID")
                    AccountType.EditValue = DT.Rows(0)("ParentCode")
                    SafeID.EditValue = Convert.ToUInt64(DT.Rows(0)("SafeID"))
                    WithdrawalDate.EditValue = DT.Rows(0)("InsertDate")
                    WDValue.Text = DT.Rows(0)("WDVAL")
                    WithdrawalFrom.EditValue = DT.Rows(0)("EMPID")
                    CurrencyFrom.EditValue = DT.Rows(0)("CurrencyFrom")
                    Notes.EditValue = DT.Rows(0)("Notes")
                    PaidFor.Text = DT.Rows(0)("PaidFor")
                    IDNo.Text = DT.Rows(0)("IDNo")
                    Phone.Text = DT.Rows(0)("Phone")
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub SetData()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الحساب"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If WDValue.Text = 0 Then
            WDValue.ErrorText = "يجب إدخال قيمة"
            Exit Sub
        End If
        If GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always Then
            If PaidFor.Text = "" Then
                PaidFor.ErrorText = "هذا الحقل لا يجب أن يكون فارغا"
                Exit Sub
            End If

            If Phone.Text.Length < 10 Then
                Phone.ErrorText = "رقم الهاتف يجب ألا يكون أقل من 10 أرقام"
                Exit Sub
            End If
        End If

        If LOADTYPE = 5 And TYPEs = 1 Then
            If GetLKPColumnVal(SafeID, "GetAccVal") < WDValue.EditValue Then
                ErrorMessage(Me, "رسالة تنبيه", "رصيد الخزنة غير كافي الرجاء التأكد من رصيد الخزنة")
                Exit Sub
            End If
        End If
        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""
        If LOADTYPE = 5 Then
            MOTYPE = "سحب من حساب"
            MOTYPE2 = "سحب من حساب" & Space(1) & WithdrawalFrom.Text
            FRMCODEPYMENT_em_cu2.lodeDate(MOTYPE2, WithdrawalFrom.Text, WithdrawalFrom.EditValue, WDValue.EditValue, CurrencyFrom.Text, GET_PHONE_SaenFroWtsaap(WithdrawalFrom.EditValue), 1, "")
            FRMCODEPYMENT_em_cu2.ShowDialog()
            If FRMCODEPYMENT_em_cu2.chick = True Then
                EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, AccountType.EditValue, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, CurrencyFrom.EditValue,
                                                  PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
            Else
                ErrorMessage(Me, "تنبيه", "عذرا رقم الكود غير صحيح الرجاء اعادة المحاولة")
            End If
        ElseIf LOADTYPE = 7 Then
            MOTYPE = "إيداع في حساب"
            MOTYPE2 = "إيداع في حساب" & Space(1) & WithdrawalFrom.Text
            EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, AccountType.EditValue, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, CurrencyFrom.EditValue,
                                                  PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        Try

            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = WDCode.Text}
            prm(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ZRPT_EMPORCUSTWITHDRAWALTB_SelectByCode", prm)


            If dt.Rows.Count Then
                Dim report As New RPTEMPWITHDRAWAL2
                report.DataSource = dt
                report.DataMember = "EMPORCUSTWITHDRAWALTB"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.XrLabel2.Text = Me.CurrencyFrom.Text
                report.XrLabel93.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.Text, True, "n2")
                report.XrLabel25.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.Text, False, "n2")
                report.ShowPreview()

                Dim pdfOptions As ImageExportOptions = report.ExportOptions.Image

                Dim dtmobyl As New DataTable
                dtmobyl.Clear()

                Dim stordpath As String
                stordpath = Application.StartupPath & "\TEMPWATS"
                Directory.CreateDirectory(stordpath)
                Dim newfilepathe As String
                newfilepathe = stordpath & "\" & "watsappmassg.jpeg"

                report.ExportToImage(newfilepathe, pdfOptions)
                If AccountType.EditValue = 4010206 Then
                    SINTWATSAPP_PDF_CLINT("120363175442297756@g.us", newfilepathe, "", "", "")
                ElseIf AccountType.EditValue = 4010205 Then
                    SINTWATSAPP_PDF_CLINT("0925093709", newfilepathe, "", "", "")

                End If
            End If


        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_MaxID(TypeID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            WDCode.Text = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_Insert(ByVal Code As String, ByVal AccParent As ULong, ByVal WDVAL As Double, ByVal SafeID As Integer,
                                            TypeID As Int32, CODEID As ULong, BranchID As Integer, DPSVAL As Decimal, Notes As String, IsUpdate As Boolean, AccIDFrom As ULong,
                                              CurrencyID As Integer, PaidFor As String, Phone As String, IDNo As String)
        Try
            Dim PRM(16) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccParent}
        PRM(2) = New SqlParameter("@WDVAL", SqlDbType.Decimal) With {.Value = WDVAL}
        PRM(3) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(4) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        PRM(5) = New SqlParameter("@CODEID", SqlDbType.BigInt) With {.Value = CODEID}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@DPSVAL", SqlDbType.Decimal) With {.Value = DPSVAL}
        PRM(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(9) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(10) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = AccIDFrom}
        PRM(11) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(12) = New SqlParameter("@MSG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(13) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(14) = New SqlParameter("@PaidFor", SqlDbType.NVarChar, -1) With {.Value = PaidFor}
        PRM(15) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = Phone}
        PRM(16) = New SqlParameter("@IDNo", SqlDbType.NVarChar, -1) With {.Value = IDNo}
        RUN_EXUTE_PRO("EMPORCUSTWITHDRAWALTB_Insert", PRM)
        Dim status As Integer = Convert.ToInt32(PRM(12).Value)
        Dim msg As String = Convert.ToString(PRM(13).Value)
        If status = 0 OrElse status = 2 Then
            ErrorMessage(Me, "رسالة تنبيه", msg)
            If status = 2 Then
                EMPORCUSTWITHDRAWALTB_MaxID(LOADTYPE)
            End If
            Exit Sub
        End If
        Print()
            'كود_رسائل الواتساب لسندات الصرف
            'If TypeID = 5 Then

            FRMEMPWITHDRAWAL_SandForWtsapp(TypeID, WDCode.Text, CurrencyFrom.Text, WDValue.EditValue, WithdrawalFrom,
                                               PaidFor, GET_PHONE_SaenFroWtsaap(WithdrawalFrom.EditValue))


            FrmSavedSuccessfully.Show()
        NEWRECORD()
        Catch ex As Exception
        MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub UPDATERECORD()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الحساب"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If WDValue.Text = "" Then
            WDValue.ErrorText = "يجب إدخال قيمة السحب"
            Exit Sub
        End If

        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""
        If LOADTYPE = 5 Then
            MOTYPE = "معالجة خطأ سحب من حساب موظف"
            MOTYPE2 = "معالجة خطأ سحب من حساب الموظف" & Space(1) & WithdrawalFrom.Text
            EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, AccountType.EditValue, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, CurrencyFrom.EditValue,
                                                  PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        ElseIf LOADTYPE = 7 Then
            If TYPEs = 1 Then
                If GetLKPColumnVal(SafeID, "GetAccVal") < WDValue.EditValue Then
                    InfoMessage(Me, "رسالة معلومات", "الخزنة لا يوجد بها رصيد كافٍ لاتمام عملية الترجيع")
                    Return
                End If
            End If
            MOTYPE = "معالجة خطأ إيداع في حساب موظف"
            MOTYPE2 = "معالجة خطأ إيداع في حساب الموظف" & Space(1) & WithdrawalFrom.Text
            EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, AccountType.EditValue, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                              Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, CurrencyFrom.EditValue,
                                              PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        Try
            WithdrawalFrom.Properties.DataSource = Nothing
            SafeID.Properties.DataSource = Nothing
            If BranchID.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(BranchID.EditValue.ToString()) Then Exit Sub
            If BranchID.Text <> String.Empty Then
                WithdrawalFrom.EditValue = -1
                SafeID.EditValue = -1
                LOADSafeID()
                SafeID.EditValue = UserAccID
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                LoadToControlar(AccountType, "Withdrawal_AccountTBLoadLine3ToLKP", "AccName", "AccCode", PR)
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub



    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Try
            NEWRECORD()
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "الرجاء اختيار الحقل "
                Return
            End If
            FRMVIEWEMPWITHDRAWAL.GCRole.DataSource = Nothing
            FRMVIEWEMPWITHDRAWAL.LoadData(TYPEs, BranchID.EditValue, LOADTYPE)
            FRMVIEWEMPWITHDRAWAL.ShowDialog()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Private Sub CurrencyFrom_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.EditValueChanged
        If CurrencyFrom.Text <> String.Empty Then
            LOADSafeID()
            WithdrawalValue.Text = ""
            WithdrawalValue.Text = GetLKPColumnVal(WithdrawalFrom, "GetAccVal")
        End If
    End Sub
    Private Sub AccountType_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType.EditValueChanged

        WithdrawalFrom.Properties.DataSource = Nothing
        WithdrawalFrom.EditValue = -1
        If BranchID.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(BranchID.EditValue.ToString()) Then Exit Sub
        If AccountType.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(AccountType.EditValue.ToString()) Then Exit Sub
        If CurrencyFrom.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(CurrencyFrom.EditValue.ToString()) Then Exit Sub
        If AccountType.Text <> String.Empty Then
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            PR(2) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = If(LOADTYPE = 5, 0, 1)}
            PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}


            LoadToControlar(WithdrawalFrom, "Withdrawal_LOADINTOLKPBASEDONAccParent", "AccName", "AccID", PR)



        End If
    End Sub
    Private Sub FRMEMPWITHDRAWAL_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
#End Region
End Class