Imports DevExpress
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports TextEdit = DevExpress.XtraEditors.TextEdit


Public Class FRMNEWCURRENCYSALE
    Dim IDcod As ULong, msgST As Int16
    Public ISbaunk, CurrFrmID As Integer
#Region "رسائل الوتسساب"
    ''اجراء يقوم بارجاء نص الرسالة 
    Private Function BuildTransENmasgeferMessage(Phonew As String, SenderBrnchPhoner As String, type As Integer) As String
        Dim message As String = My.Settings.Combny_name & vbNewLine
        message &= BuildTransBodeferType(type)
        message &= "للإستفسار هـ : " & SenderBrnchPhoner & vbNewLine &
               "شكراً لتعاملكم معنا"
        Return message
    End Function

    Private Function BuildTransBodeferType(type As Integer) As String
        Dim message As String = "تمت عملية "

        ' تحديد نوع العملية بناءً على مقارنة 'type' مع 'SAFTypeform.EditValue'
        If type = 0 And SafeTypeFrom.EditValue = 0 Then
            message &= "شراء عملة "
        ElseIf type = 0 And SafeTypeFrom.EditValue <> 0 Then
            message &= "بيع عملة "
        ElseIf type = 1 And SafeTypeTo.EditValue <> 0 Then
            message &= "شراء عملية "
        End If

        ' إضافة معلومات إضافية حول العملية
        message &= vbNewLine & "CODE : " & Code.Text & vbNewLine &
               "القيمة  : " & Cur_Code(CurrencyFrom.Text, BPrice1.Text, True, "n2") & vbNewLine
        message &= "السعر :" & Purchaseprice.Text & vbNewLine
        ' إضافة تفاصيل العملية بناءً على نوع الحساب
        If SafeTypeFrom.EditValue = 0 And type = 0 Then
            message &= BuildMessageForAgenbodeyrTransfer(type) ' تأكد من أن دالة BuildMessageForAgenbodeyrTransfer تعمل بشكل صحيح
        ElseIf SafeTypeFrom.EditValue <> 0 And type = 0 Then
            message &= "من حسابكم رقم : " & GET_codefor_Acount_SaenFroWtsaap(SafeID.EditValue)
        End If
        ' إضافة تفاصيل العملية بناءً على نوع الحساب

        If SafeTypeTo.EditValue <> 0 And SafeTypeFrom.EditValue <> 0 And type = 1 Then

            message &= " حسابكم رقم : " & GET_codefor_Acount_SaenFroWtsaap(SAFFACCTO.EditValue)
        End If
        '''إضافة معلومات إضافية حول السعر والقيمة المقابلة
        message &= vbNewLine


        If SafeTypeTo.EditValue <> 0 And type = 0 Then
            message &= "بما تعادل : " & Cur_Code(CurrencyTo.Text, BPrice2.Text, True, "n2") & vbNewLine
        ElseIf SafeTypeTo.EditValue <> 0 And type = 1 Then
            message &= "دخول قيمة  : " & Cur_Code(CurrencyTo.Text, BPrice2.Text, True, "n2") & vbNewLine
        Else
            message &= "بما تعادل : " & Cur_Code(CurrencyTo.Text, BPrice2.Text, True, "n2") & vbNewLine
        End If

        ' إرجاع الرسالة المعدلة
        Return message
    End Function


    '' استندر الرسالة
    Private Function BuildMessageForAgenbodeyrTransfer(type As Integer) As String

        Dim message As String = ""
        Select Case type
            Case 0
                message &= "من العميل : " & CustomerName.Text & vbNewLine
            Case Else

                message &= "لـ : " & CustomerName.Text & vbNewLine
        End Select

        message &= "هـ : " & CustPhone.Text


        Return message
    End Function

    Private Sub SendMessage(message As String, Phone As String)
        WATSAPPMsAG(Phone, message, False)


    End Sub

#End Region
    Public Sub LoadCountries()

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            NEWDVGFROMAT(CountryGV)
            SetDataSource(CountryID, DT)
        End If

    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(39, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub SetDataSource(ByVal edit As GridLookUpEdit, ByVal dataSource As Object)
        edit.Properties.DataSource = dataSource
        Dim firstValue = edit.Properties.GetKeyValue(0)
        edit.EditValue = firstValue
    End Sub
#Region "LOADCONTROLS"
    Public Sub LoadCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            NEWDVGFROMAT(CountryGV)
        End If
    End Sub
    Public Sub LoadBank(NGridLookUpEdit As Object, CUID As Integer, Action As Integer)
        If IsEmpty(BranchID) Then Exit Sub
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CUID}
        PR(2) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = Action}
        LoadToControlar(NGridLookUpEdit, "BBranchTb_LOADBASEDCURRENCY", "AccName", "AccID", PR)
    End Sub
    Public Sub LoadBankMain(NGridLookUpEdit As LookUpEdit, CUID As Integer, Action As Integer)
        If IsEmpty(BranchID) Then Exit Sub
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = 0}
        PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = 0}
        PR(2) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = Action}
        LoadToControlar(NGridLookUpEdit, "BBranchTb_LOADBASEDCURRENCY", "AccName", "AccID", PR)
    End Sub
    Sub LoadSafeID(NGridLookUpEdit As GridLookUpEdit, UserID As Integer, UESERTYPE As Integer, BranchID As Integer, Type As ULong,
                   CounID As Integer, CurrID As Integer, TrType As Integer, Typesf As Integer, Action As Integer)
        Try
            NGridLookUpEdit.Properties.DataSource = Nothing
            Dim PR(8) As SqlParameter
            PR(0) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = Type}
            PR(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PR(2) = New SqlParameter("@UESERTYPE", SqlDbType.Int) With {.Value = UESERTYPE}
            PR(3) = New SqlParameter("@BRnchID", SqlDbType.Int) With {.Value = BranchID}
            PR(4) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CounID}
            PR(5) = New SqlParameter("@CurrID", SqlDbType.Int) With {.Value = CurrID}
            PR(6) = New SqlParameter("@TransType", SqlDbType.Int) With {.Value = TrType}
            PR(7) = New SqlParameter("@Typesf", SqlDbType.Int) With {.Value = Typesf}
            PR(8) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = Action}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CurrencyBuyAndSale_LoadSafe", PR)
            If dt.Rows.Count > 0 Then
                NGridLookUpEdit.Properties.DataSource = dt
                NGridLookUpEdit.Properties.ValueMember = "AccID"
                NGridLookUpEdit.Properties.DisplayMember = "AccName"
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try
    End Sub
    Sub LOADSafeID2(NGridLookUpEdit As LookUpEdit, UserID As Integer, UESERTYPE As Integer, BranchID As Integer, Type As ULong,
                    CounID As Integer, CurrID As Integer, TrType As Integer, Typesf As Integer, Action As Integer)
        Try
            NGridLookUpEdit.Properties.DataSource = Nothing
            Dim PR(8) As SqlParameter
            PR(0) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = Type}
            PR(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PR(2) = New SqlParameter("@UESERTYPE", SqlDbType.Int) With {.Value = UESERTYPE}
            PR(3) = New SqlParameter("@BRnchID", SqlDbType.Int) With {.Value = BranchID}
            PR(4) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CounID}
            PR(5) = New SqlParameter("@CurrID", SqlDbType.Int) With {.Value = CurrID}
            PR(6) = New SqlParameter("@TransType", SqlDbType.Int) With {.Value = TrType}
            PR(7) = New SqlParameter("@Typesf", SqlDbType.Int) With {.Value = Typesf}
            PR(8) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = Action}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CurrencyBuyAndSale_LoadSafe", PR)
            If dt.Rows.Count > 0 Then
                NGridLookUpEdit.Properties.DataSource = dt
                NGridLookUpEdit.Properties.ValueMember = "AccID"
                NGridLookUpEdit.Properties.DisplayMember = "AccName"
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try
    End Sub
    Public Sub CurrenciesBuyandsellTB_selectTYPe(NGridLookUpEdit As GridLookUpEdit, Typesf As Integer, GridLoo As GridView, BranchID As Integer, GLKPTYPE As Integer)
        NGridLookUpEdit.Properties.DataSource = Nothing
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PR(1) = New SqlParameter("@Typesf", SqlDbType.Int) With {.Value = Typesf}
        PR(2) = New SqlParameter("@GLKPTYPE", SqlDbType.Int) With {.Value = GLKPTYPE}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("NewCurrencyBuyAndSale_LoadGLKP", PR)
        If dt.Rows.Count > 0 Then
            NGridLookUpEdit.Properties.DataSource = dt
            NGridLookUpEdit.Properties.ValueMember = "AccCode"
            NGridLookUpEdit.Properties.DisplayMember = "AccName"
        End If
    End Sub
    Public Sub CurrenciesBuyandsellTB_MAxID()
        Dim prm(3) As SqlParameter
        Dim dt As New DataTable
        dt.Clear()
        prm(0) = New SqlParameter("@BrnchID", SqlDbType.Int) With {.Value = BID}
        prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(2) = New SqlParameter("@IDcode", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(3) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = 25}
        dt = RUN_QUARY_PRO("CurrenciesBuyandsellTB_MAxID", prm)
        If prm(2).Value > 0 Then
            IDcod = prm(2).Value
            Code.Properties.TextEditStyle = XtraEditors.Controls.TextEditStyles.Standard
            Code.Text = prm(1).Value
        Else
            BtnNew.PerformClick()
        End If
    End Sub
    Sub disabled()
        InsertDate.Enabled = False
        CurrencyFrom.Enabled = False
        BPrice1.Enabled = False
        BPrice2.Enabled = False
        BPrice2.Enabled = False
        SafeTypeFrom.Enabled = False
        SafeTypeTo.Enabled = False
        SAFFACCTO.Enabled = False
        MemoEdit1.Enabled = False
        SafeID.Enabled = False
        Purchaseprice.Enabled = False
        thepurpose.Enabled = False
        CustomerName.Enabled = False
        CustPhone.Enabled = False
        NationalNo.Enabled = False
        ACCVALFORM.Enabled = False
        ACCValTo.Enabled = False
        PriceType.Enabled = False
    End Sub
    Sub Enabled()
        InsertDate.Enabled = True
        CurrencyFrom.Enabled = True
        BPrice1.Enabled = True
        BPrice2.Enabled = True
        SafeTypeFrom.Enabled = True
        SafeTypeTo.Enabled = True
        SAFFACCTO.Enabled = True
        MemoEdit1.Enabled = True
        SafeID.Enabled = True
        Purchaseprice.Enabled = True
        thepurpose.Enabled = True
        CustomerName.Enabled = True
        CustPhone.Enabled = True
        NationalNo.Enabled = True
        ACCVALFORM.Enabled = True
        ACCValTo.Enabled = True
        If BID = MAINBID Then
            PriceType.Enabled = True
        Else
            PriceType.Enabled = False
        End If
    End Sub
    Public Sub NEWRecored()
        'New_Controlrs(Me)
        PriceType.SelectedIndex = 0
        LOADBRNACH()
        CountryID.EditValue = -1
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnPrint.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        CurrenciesBuyandsellTB_MAxID()
        InsertDate.EditValue = Date.Now
        LOADCIDFROM()
        CurrencyFrom.EditValue = -1
        BPrice1.EditValue = 0.00
        CurrencyTo.EditValue = -1
        BPrice2.EditValue = 0.00
        CurrencyTo.EditValue = 1
        CurrencyTo.Enabled = False
        SafeTypeFrom.Properties.DataSource = Nothing
        SafeTypeTo.Properties.DataSource = Nothing
        SafeID.Properties.DataSource = Nothing
        SAFFACCTO.Properties.DataSource = Nothing
        BranchID.EditValue = BID
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeFrom, PriceType.SelectedIndex, GridView1, BranchID.EditValue, 0)
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeTo, PriceType.SelectedIndex, gvroll, BranchID.EditValue, 1)
        LOADBRNACH()
        LoadCountries()
        DVGFormat(GridView1)
        DVGFormat(gvroll)
        SafeTypeFrom.EditValue = -1
        SafeTypeTo.EditValue = -1
        SAFFACCTO.EditValue = -1
        MemoEdit1.Text = String.Empty
        SafeID.EditValue = -1
        Purchaseprice.EditValue = 0.00
        thepurpose.SelectedIndex = 1

        CustomerName.Text = String.Empty
        CustPhone.Text = String.Empty
        NationalNo.Text = String.Empty
        LayoutControlItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LayoutControlItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LayoutControlItem23.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LayoutControlItem21.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LayoutControlItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        ACCVALFORM.EditValue = 0.000
        ACCValTo.EditValue = 0.000
    End Sub
    Public Overrides Sub BNew()
        Try
            NEWRecored()
            Enabled()
            MyBase.BNew()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ في النظام ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Sub LOADCIDTO(Action As Integer, ID As Integer)
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة الأولى"
            Exit Sub
        End If
        CurrencyTo.Properties.DataSource = Nothing
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = Action}
        PR(1) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOBUYANDSALE", PR)
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
            CurrencyTo.Properties.ShowHeader = False
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADCIDFROM()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP_bu")

        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "ID"
            CurrencyFrom.Properties.DisplayMember = "CuName"
            CurrencyFrom.Properties.ShowHeader = False
        Else
            CurrencyFrom.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADBRNACH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
    End Sub
#End Region
    Private Sub CurrencyFrom_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.EditValueChanged
        If BtnSave.Enabled = True Then
            If CurrencyFrom.EditValue > -1 Or CurrencyFrom.Text <> String.Empty Then
                If PriceType.SelectedIndex = 0 Or PriceType.SelectedIndex = 1 Then
                    LOADCIDTO(0, CurrFrmID)
                Else
                    LOADCIDTO(1, 1)
                End If
                get_BracuNetBurnnc()
                If SafeTypeFrom.EditValue = -1 Or SafeTypeFrom.Text = String.Empty Then
                    SafeTypeFrom.ErrorText = "الرجاء اختيار العملة الاولي"
                    Return
                End If
                If SafeTypeFrom.EditValue = -1 Then
                    SafeTypeFrom.ErrorText = "هذا الحقل مطلوب"
                    Return
                End If

                If SafeID.Text = String.Empty Then
                    SafeID.ErrorText = "هذا الحقل مطلوب"
                    Return
                End If
                GETVALTOLTEl(SafeID.EditValue, CurrencyFrom.EditValue, ACCVALFORM, SafeTypeFrom.EditValue, GridView1.GetFocusedRowCellValue("PrType"))
            End If
        End If
    End Sub
    Public Sub GETVALTOLTEl(ACCTO As ULong, CurrencyFrom As Integer, WithdrawalValue As TextEdit, crunseType As Integer, PrcType As Integer)
        Try
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@ACCAcount", SqlDbType.BigInt) With {.Value = ACCTO}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = CurrencyFrom}
            PR(2) = New SqlParameter("@ACCTYPE", SqlDbType.Int) With {.Value = crunseType}
            PR(3) = New SqlParameter("@ISBANCK", SqlDbType.BigInt) With {.Value = PrcType}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("GET_VAL_FOR_CUST_ORSAFA", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.Text = dt.Rows(0)("GetAccVal")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ في النظام", ex.Message)
        End Try
    End Sub
    Private Sub CurrencyFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyFrom.QueryPopUp
        CurrencyFrom.Properties.PopulateColumns()
        CurrencyFrom.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub CurrencyTo_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyTo.QueryPopUp
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then Return
        CurrencyTo.Properties.PopulateColumns()
        CurrencyTo.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub Code_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Code.KeyPress
        e.Handled = True
    End Sub
    Private Sub FRMINTCURSALES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        BtnNew.PerformClick()
        If UserType = 1 Then
            BranchID.Enabled = True
            SafeID.Enabled = True
        Else
            BranchID.Enabled = False
            SafeID.Enabled = False

        End If
    End Sub
    Public Sub get_BracuNetBurnnc()
        'Try
        If CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "الرجاء اختيار العملة الاولى "
                Return
            End If
            If CurrencyTo.Text = String.Empty Then
                CurrencyTo.ErrorText = "الرجاء اختيار العملة الاولى "
                Exit Sub
            End If
            If PriceType.SelectedIndex = -1 Then
                PriceType.ErrorText = "يرجى اختيار نوع التسعير"
                Return
            End If
            If PriceType.SelectedIndex = 2 Then
                If SAFFACCTO.EditValue = -1 Then
                    SAFFACCTO.ErrorText = "يرجى اختيار المصرف"
                    Return
                End If
            End If
            If CountryID.EditValue = -1 Then
                CountryID.ErrorText = "الرجاء اختيار الدولة "
                Return
            End If
            Dim prm(7) As SqlParameter
        prm(0) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        prm(1) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
        prm(2) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
        prm(3) = New SqlParameter("@BPrice11", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@Purchaseprice", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@ISbaunk", SqlDbType.Int) With {.Value = ISbaunk}
            prm(6) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
            prm(7) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BNKID}
            Dim dt As New DataTable
            dt.Clear()
        dt = RUN_QUARY_PRO("New_get_BracuNetBurnnc_SalePrice", prm)
        If dt.Rows.Count > 0 Then
                Dim De As Decimal = prm(4).Value
                Purchaseprice.Text = De
            If BPrice1.EditValue > 0 Then
                BPrice2.Text = prm(3).Value.ToString

            Else
                BPrice2.Text = 0.00
                    BPrice1.ErrorText = "الرجاء ادخال القيمة الاولى "
                End If
            End If
        'Catch ex As Exception
        '    ErrorMessage(Me, "رسالة خطأ في النظام", ex.Message)
        'End Try
    End Sub
    Private Sub CurrencyTo_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyTo.EditValueChanged
        If CurrencyTo.EditValue > -1 Then
            If ISbaunk = 3 Then
                If SafeTypeFrom.EditValue > 0 Then
                    LoadBank(SAFFACCTO, CurrencyTo.EditValue, 1)
                End If
            End If
            get_BracuNetBurnnc()

            If SafeTypeTo.EditValue = -1 Or SafeTypeTo.Text = String.Empty Then
                SafeTypeTo.ErrorText = "الرجاء اختيار العملة الاولي"
                Return
            End If
            If SAFFACCTO.EditValue = -1 Or SAFFACCTO.Text = String.Empty Then
                SAFFACCTO.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If CurrencyTo.Text = String.Empty Then
                CurrencyTo.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            GETVALTOLTEl(SAFFACCTO.EditValue, CurrencyTo.EditValue, ACCValTo, SafeTypeTo.EditValue, gvroll.GetFocusedRowCellValue("PrType"))
        End If
    End Sub
    Private Sub BPrice1_EditValueChanged(sender As Object, e As EventArgs) Handles BPrice1.EditValueChanged
        If CurrencyTo.EditValue > -1 Then
            get_BracuNetBurnnc()
        Else
            CurrencyTo.ErrorText = "الرجاء اختيار العملة الاولى "
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Public Sub CurrenciesBuyandsellTB_Insert()
        Try


            Dim prm(29) As SqlParameter
            prm(0) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDcod}
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
            prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(3) = New SqlParameter("@FirstCurrency", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            prm(4) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
            prm(5) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(6) = New SqlParameter("@BPrice2", SqlDbType.Float) With {.Value = BPrice2.EditValue}
            prm(7) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = MemoEdit1.Text}
            prm(8) = New SqlParameter("@UeserInset", SqlDbType.Int) With {.Value = UserID}
            prm(9) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(10) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(11) = New SqlParameter("@IsCachorBank", SqlDbType.Int) With {.Value = ISbaunk}
            prm(12) = New SqlParameter("@SAFTypeform", SqlDbType.BigInt) With {.Value = SafeTypeFrom.EditValue}
            prm(13) = New SqlParameter("@SaFACCount", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
            prm(14) = New SqlParameter("@SafTypeTo", SqlDbType.BigInt) With {.Value = SafeTypeTo.EditValue}
            prm(15) = New SqlParameter("@SAFFACCTO", SqlDbType.BigInt) With {.Value = SAFFACCTO.EditValue}
            prm(16) = New SqlParameter("@Purchaseprice", SqlDbType.Decimal, 18, 3) With {.Value = Purchaseprice.EditValue}
            prm(17) = New SqlParameter("@CusIDNo", SqlDbType.NVarChar, -1) With {.Value = NationalNo.Text}
            prm(18) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = CustPhone.Text}
            prm(19) = New SqlParameter("@thepurpose", SqlDbType.Int) With {.Value = thepurpose.SelectedIndex}
            prm(20) = New SqlParameter("@CustName", SqlDbType.NVarChar, -1) With {.Value = CustomerName.Text}
            prm(21) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BNKID}
            prm(22) = New SqlParameter("@AccAcountBank", SqlDbType.NVarChar, (50)) With {.Value = AccAccountBank}
            prm(23) = New SqlParameter("@CheckType", SqlDbType.Int) With {.Value = CHTYPE}
            prm(24) = New SqlParameter("@AccountOwnName", SqlDbType.NVarChar, -1) With {.Value = CustName}
            prm(25) = New SqlParameter("@Typeoftransaction", SqlDbType.Int) With {.Value = TypeofTransfer}
            prm(26) = New SqlParameter("@Transactionnumber", SqlDbType.BigInt) With {.Value = TransferNum}
            prm(27) = New SqlParameter("@AgentBankNumber", SqlDbType.NVarChar, (50)) With {.Value = AccAccountBank}
            prm(28) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
            If PriceType.SelectedIndex = 0 Then
                prm(29) = New SqlParameter("@PrType", SqlDbType.Int) With {.Value = 0}
            Else
                prm(29) = New SqlParameter("@PrType", SqlDbType.Int) With {.Value = GridView1.GetFocusedRowCellValue("PrType")}
            End If
            RUN_EXUTE_PRO("NewCurrencyBuyandSale_Insert", prm)
            msgST = prm(9).Value
            If prm(9).Value = 0 Or prm(9).Value = 2 Then
                ErrorMessage(Me, "رسالة خطأ", prm(10).Value)
                If prm(9).Value = 2 Then
                    CurrenciesBuyandsellTB_MAxID()
                End If
                Return
            Else
                FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 2
                'If PriceType.SelectedIndex = 0 Then
                '    RPhone_get_forWatsab_and_CoBranch_Mobile("", BID)
                '    If SafeTypeTo.EditValue <> 0 Then
                '        SendMessage(BuildTransENmasgeferMessage("", sql_Mobile1, 0), GET_PHONE_SaenFroWtsaap(SAFFACCTO.EditValue))
                '    End If


                '    If SafeTypeFrom.EditValue = 0 Then
                '        SendMessage(BuildTransENmasgeferMessage(Phone, sql_Mobile1, 1), TextEdit2.Text)
                '    Else
                '        SendMessage(BuildTransENmasgeferMessage("", sql_Mobile1, 1), GET_PHONE_SaenFroWtsaap(SafeID.EditValue))
                '    End If
                'End If
                Print()
                BtnNew.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تحذير وجود مشكلة في نظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub CurrenciesBuyandsellTSElecSPreceB_Updata()
        Try
            If ISbaunk = 0 Or ISbaunk = 1 Then
                Dim prm(22) As SqlParameter
                prm(0) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDcod}
                prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
                prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                prm(3) = New SqlParameter("@SaFACCount", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                prm(4) = New SqlParameter("@FirstCurrency", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
                prm(5) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
                prm(6) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
                prm(7) = New SqlParameter("@BPrice2", SqlDbType.Float) With {.Value = BPrice2.EditValue}
                prm(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = MemoEdit1.Text}
                prm(9) = New SqlParameter("@UeserInset", SqlDbType.Int) With {.Value = UserID}
                prm(10) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                prm(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
                prm(12) = New SqlParameter("@IsCachorBank", SqlDbType.Int) With {.Value = ISbaunk}
                prm(13) = New SqlParameter("@SAFTypeform", SqlDbType.BigInt) With {.Value = SafeTypeFrom.EditValue}
                prm(14) = New SqlParameter("@SafTypeTo", SqlDbType.BigInt) With {.Value = SafeTypeTo.EditValue}
                prm(15) = New SqlParameter("@SAFFACCTO", SqlDbType.BigInt) With {.Value = SAFFACCTO.EditValue}
                prm(16) = New SqlParameter("@Purchaseprice", SqlDbType.Decimal, 18, 3) With {.Value = Purchaseprice.EditValue}
                prm(17) = New SqlParameter("@CusIDNo", SqlDbType.NVarChar, -1) With {.Value = NationalNo.EditValue}
                prm(18) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = CustPhone.Text}
                prm(19) = New SqlParameter("@thepurpose", SqlDbType.TinyInt) With {.Value = thepurpose.SelectedIndex}
                prm(20) = New SqlParameter("@CustName", SqlDbType.NVarChar, -1) With {.Value = CustomerName.Text}
                prm(21) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BNKID}
                prm(22) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
                RUN_EXUTE_PRO("NewCurrenciesBuyandsellTSElecSPreceB_Updata", prm)
                msgST = prm(10).Value
                If prm(10).Value = 0 Then
                    MessageBox.Show(prm(11).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                Else
                    FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 4
                    Print()
                    BtnNew.PerformClick()
                End If
            ElseIf PriceType.SelectedIndex = 2 Then
                ISbaunk = 3
                    Dim prm(3) As SqlParameter
                    prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
                    prm(1) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                    prm(2) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
                    prm(3) = New SqlParameter("@IsCachorBank", SqlDbType.Int) With {.Value = ISbaunk}
                    RUN_EXUTE_PRO("NewCurrenciesBankSell_Update", prm)
                    msgST = prm(1).Value
                    If prm(1).Value = 0 Then
                        MessageBox.Show(prm(2).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    Else
                        FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 4
                        Print()
                        BtnNew.PerformClick()
                    End If
                End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تحذير وجود مشكلة في نظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub SetData()
        If Code.Text = String.Empty Then
            Code.ErrorText = "الرجاء التحقق من رقم الفرع"
            Return
        End If

        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "الرجاء التحقق من  الفرع"
            Return
        End If

        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "الرجاء التحقق من  الخزينة"
            Return
        End If

        If CurrencyFrom.EditValue = -1 Then
            CurrencyFrom.ErrorText = "هذا الحقل مطلوب "
            Return
        End If

        If BPrice1.EditValue <= 0.00 Then
            BPrice1.ErrorText = "يجب ان تكون القيمة اكبر من صفر  "
            Return
        End If

        If CurrencyTo.EditValue = -1 Then
            CurrencyTo.ErrorText = "هذا الحقل مطلوب "
            Return
        End If

        If BPrice2.EditValue <= 0 Then
            BPrice2.ErrorText = "الرجاء ادخال القيمة المصورفة"
            Return
        End If
        If SafeTypeFrom.Text.Contains("عملاء") = False Then
            If CustomerName.EditValue = String.Empty Then
                CustomerName.ErrorText = "الرجاء ادخال اسم العميل"
                Return
            End If
            If NationalNo.EditValue = String.Empty Then
                NationalNo.ErrorText = "الرجاء ادخال الرقم الوطني"
                Return
            End If
            If NationalNo.Text <> "" Then
                If NationalNo.Text.Length <> 12 Then
                    NationalNo.ErrorText = "الرقم الوطني يجب أن يتكون من 12 رقم"
                    Exit Sub
                End If
            End If
        End If
        If ISbaunk = 0 Or ISbaunk = 1 Then
            If SafeTypeFrom.EditValue = 0 Then
                If SafeTypeTo.EditValue = 0 Then
                    If SafeID.EditValue <> SAFFACCTO.EditValue Then
                        MsgBox("عذرا يجب ان تكون الخزينة المباعة هيا نفسها الخزينة المستلمة")
                        Return
                    End If
                End If
            End If
        Else
                If ISbaunk = 0 Or ISbaunk = 1 Then
                    If SafeTypeTo.EditValue = 0 Then
                        If UserAccID <> SAFFACCTO.EditValue Then
                        MsgBox("عذرا يجب ان تكون الخزينة المباعة هيا نفسها الخزينة المستلمة")
                        Return
                        End If
                    End If
                End If
            End If

            If BPrice1.EditValue > ACCVALFORM.EditValue And SafeTypeFrom.EditValue = 0 Then
            BPrice1.ErrorText = "عذرا القيمة المصروفة أكبر من رصيد العملة"
            Return
        End If
        If PriceType.SelectedIndex = -1 Then
            PriceType.ErrorText = "يرجى تحديد نوع التسعير"
            Return
        End If
        If CountryID.EditValue = -1 Then
            CountryID.ErrorText = "يرجى اختيار الدولة"
            Return
        End If
        If PriceType.SelectedIndex = 2 Then
            If BankID.EditValue = -1 Then
                BankID.ErrorText = "يرجى اختيار المصرف"
                Return
            End If
        End If
        CurrenciesBuyandsellTB_Insert()
        MyBase.SetData()
    End Sub
    Private Sub BPrice2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles BPrice2.KeyPress
        e.Handled = True
    End Sub
    Private Sub TextEdit1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Purchaseprice.KeyPress
        e.Handled = True
    End Sub
    Dim CURnsSENT As String
    Dim arC As New arabicconverter
    Private Sub BPrice2_EditValueChanged(sender As Object, e As EventArgs) Handles BPrice2.EditValueChanged
        Dim OnePERThousand As Boolean = False
        If CurrencyTo.EditValue = 1 Then
            CURnsSENT = "درهم"
            OnePERThousand = True
        ElseIf CurrencyTo.EditValue = 2 Then
            CURnsSENT = "سنت"
        ElseIf CurrencyTo.EditValue = 3 Then

            CURnsSENT = "قرش"
        ElseIf CurrencyTo.EditValue = 4 Then
            CURnsSENT = "سنت"
        End If
        BPrice2.EditValue = Math.Ceiling(BPrice2.EditValue / 5) * 5

        LabelControl1.Text = "المبلغ بالحروف " & Space(1) & ":" & kokotxt(Val(BPrice2.EditValue), CurrencyTo.Text, CURnsSENT, OnePERThousand)
    End Sub
    Private Sub SafTypeTo_EditValueChanged(sender As Object, e As EventArgs) Handles SafeTypeTo.EditValueChanged
        SAFFACCTO.Properties.DataSource = Nothing
        If SafeTypeTo.EditValue > -1 Then
            If PriceType.SelectedIndex = 1 Or PriceType.SelectedIndex = 0 Then
                LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafeTypeTo.EditValue, CountryID.EditValue, CurrencyTo.EditValue,
                        gvroll.GetFocusedRowCellValue("PrType"), gvroll.GetFocusedRowCellValue("Typesf"), 0)
            ElseIf PriceType.SelectedIndex = 2 Then
                If gvroll.GetFocusedRowCellValue("AccCode") = 0 Then

                    LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafeTypeTo.EditValue, CountryID.EditValue, CurrencyTo.EditValue,
                                0, 0, 0)
                Else
                    LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafeTypeTo.EditValue, CountryID.EditValue, CurrencyTo.EditValue,
                                gvroll.GetFocusedRowCellValue("PrType"), gvroll.GetFocusedRowCellValue("Typesf"), 0)

                End If
                DVGFormat(gvroll)
            End If
            If gvroll.GetFocusedRowCellValue("AccCode") = 0 Then
                SAFFACCTO.EditValue = UserAccID
                SAFFACCTO.Enabled = False
            Else
                SAFFACCTO.EditValue = -1
                SAFFACCTO.Enabled = True
            End If
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        CurrenciesBuyandsellTSElecSPreceB_Updata()
        If msgST = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub
    Private Sub SafeTypeFrom_EditValueChanged(sender As Object, e As EventArgs) Handles SafeTypeFrom.EditValueChanged

        If SafeTypeFrom.EditValue > -1 Then
            SafeID.Properties.DataSource = Nothing
            If PriceType.SelectedIndex = 1 Or PriceType.SelectedIndex = 0 Then
                LoadSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SafeTypeFrom.EditValue, CountryID.EditValue,
                           CurrencyFrom.EditValue, GridView1.GetFocusedRowCellValue("PrType"), GridView1.GetFocusedRowCellValue("Typesf"), 0)
            ElseIf PriceType.SelectedIndex = 2 Then
                LoadBank(SafeID, CurrencyFrom.EditValue, 0)
                If GridView1.GetFocusedRowCellValue("AccCode") = 0 Then
                    LoadSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SafeTypeFrom.EditValue, CountryID.EditValue,
                               CurrencyFrom.EditValue, 0, 0, 0)
                Else
                    LoadSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SafeTypeFrom.EditValue, CountryID.EditValue,
                               CurrencyFrom.EditValue, GridView1.GetFocusedRowCellValue("PrType"), GridView1.GetFocusedRowCellValue("Typesf"), 1)
                End If
            End If
            If GridView1.GetFocusedRowCellValue("AccCode") = 0 Then
                SafeID.EditValue = UserAccID
                SafeID.Enabled = False
            Else
                SafeID.EditValue = -1
                SafeID.Enabled = True
            End If
            DVGFormat(GridView2)
        End If
    End Sub
    Private Sub BPrice1_Leave(sender As Object, e As EventArgs) Handles BPrice1.Leave
        If BPrice1.EditValue < 0 Then
            BPrice1.ErrorText = "يجب ان تكون القيمة اكبر من صفر "
            BPrice1.EditValue = 0.00
            BPrice1.Select()
            Return
        End If
    End Sub
    Private Sub SafeID_EditValueChanged(sender As Object, e As EventArgs) Handles SafeID.EditValueChanged
        If SafeID.EditValue > -1 Then
            If SafeTypeFrom.EditValue = -1 Or SafeTypeFrom.Text = String.Empty Then
                SafeTypeFrom.ErrorText = "الرجاء اختيار العملة الاولي"
                Return
            End If
            If SafeTypeFrom.EditValue = -1 Then
                SafeTypeFrom.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If CurrencyFrom.Text = String.Empty Then
                SafeID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            GETVALTOLTEl(SafeID.EditValue, CurrencyFrom.EditValue, ACCVALFORM, SafeTypeFrom.EditValue, GridView1.GetFocusedRowCellValue("PrType"))
        End If
    End Sub
    Public GetPriceType As Boolean
    Private Sub SAFFACCTO_EditValueChanged(sender As Object, e As EventArgs) Handles SAFFACCTO.EditValueChanged
        If PriceType.SelectedIndex = 2 Then
            BNKID = SAFFACCTO.EditValue
            BuyPurpose = Convert.ToInt32(thepurpose.SelectedIndex)
            CHTYPE = Convert.ToInt32(CheckType.SelectedIndex)
            CusIDNo = NationalNo.Text
            Phone = CustPhone.Text
            CustName = CustomerName.Text
            AccAccountBank = AgentBankNumber.Text
            TypeofTransfer = Convert.ToInt32(TypeofTransaction.SelectedIndex)
            TransferNum = Convert.ToUInt64(TransactionNo.EditValue)
            get_BracuNetBurnnc()
        Else
            BNKID = 0
            BuyPurpose = 0
            CHTYPE = 0
            CusIDNo = "لا يوجد"
            Phone = "لا يوجد"
            CustName = "لا يوجد"
            AccAccountBank = "لا يوجد"
            TypeofTransfer = 0
            TransferNum = 0
        End If
        If SAFFACCTO.EditValue > -1 Then
            If SafeTypeTo.EditValue = -1 Or SafeTypeTo.Text = String.Empty Then
                SafeTypeTo.ErrorText = "الرجاء اختيار العملة الاولي"
                Return
            End If
            If SAFFACCTO.EditValue = -1 Or SAFFACCTO.Text = String.Empty Then
                SAFFACCTO.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If CurrencyTo.Text = String.Empty Then
                CurrencyTo.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If PriceType.SelectedIndex = 1 Or PriceType.SelectedIndex = 0 Then
                GetPriceType = gvroll.GetFocusedRowCellValue("PrType")
            Else
                GetPriceType = True
            End If
            GETVALTOLTEl(SAFFACCTO.EditValue, CurrencyTo.EditValue, ACCValTo, SafeTypeTo.EditValue, GetPriceType)
        End If
    End Sub
    Private Sub MemoEdit1_KeyDown(sender As Object, e As KeyEventArgs) Handles MemoEdit1.KeyDown
        If e.Handled = Keys.Enter Then
            BtnSave.PerformClick()
        End If
    End Sub
    Private Sub Code_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles Code.ButtonClick
        FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 2
        FRMNEWVIEWCURRENCYBUY.ShowDialog()
    End Sub
    Sub DVGFormat(GridView11 As GridView)
        Dim gvrolls As New GridView
        gvrolls = GridView11
        gvrolls.OptionsBehavior.EditingMode = True
        gvrolls.OptionsBehavior.ReadOnly = True
        gvrolls.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        gvrolls.OptionsView.ShowGroupPanel = False
        gvrolls.OptionsFind.AlwaysVisible = True
        gvrolls.ShowFindPanel()
        gvrolls.OptionsView.ShowFooter = False
        For i As Integer = 0 To GridView11.Columns.Count - 1
            gvrolls.Columns(i).AppearanceCell.TextOptions.HAlignment = Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceCell.TextOptions.VAlignment = Utils.VertAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.HAlignment = Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.VAlignment = Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        gvrolls.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        gvrolls.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        gvrolls.OptionsView.EnableAppearanceEvenRow = True
        gvrolls.Appearance.OddRow.BackColor = Color.WhiteSmoke
        gvrolls.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub GetRecord(x)
        'BtnNew.PerformClick()
        BtnSave.Enabled = False
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.NVarChar, -1) With {.Value = x}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CurrenciesBuyandsellTB_loadtoFrm", prm)
        If dt.Rows.Count > 0 Then

            Code.Text = dt.Rows(0)("Code").ToString
            InsertDate.EditValue = dt.Rows(0)("InsertDate")
            CountryID.EditValue = dt.Rows(0)("CountryID")
            BranchID.EditValue = dt.Rows(0)("BranchID")
            PriceType.SelectedIndex = dt.Rows(0)("IsCachorBank")
            ISbaunk = dt.Rows(0)("IsCachorBank")
            NationalNo.Text = dt.Rows(0)("CustIDNo").ToString
            CustPhone.Text = dt.Rows(0)("Phone").ToString
            BankID.EditValue = dt.Rows(0)("BankID")
            AgentBankNumber.EditValue = dt.Rows(0)("AccAcountBank")
            CheckType.EditValue = dt.Rows(0)("CheckType")
            AccountOwnerNo.EditValue = dt.Rows(0)("AccAcountBank")
            TypeofTransaction.EditValue = dt.Rows(0)("Typeoftransaction")
            TransactionNo.EditValue = dt.Rows(0)("Transactionnumber")
            thepurpose.SelectedIndex = dt.Rows(0)("thepurpose")
            Purchaseprice.EditValue = dt.Rows(0)("Purchasingprice")
            MemoEdit1.Text = dt.Rows(0)("Notes")
            CustomerName.Text = dt.Rows(0)("CusName")
            CurrenciesBuyandsellTB_selectTYPe(SafeTypeFrom, PriceType.SelectedIndex, GridView1, BranchID.EditValue, 0)
            CurrenciesBuyandsellTB_selectTYPe(SafeTypeTo, PriceType.SelectedIndex, gvroll, BranchID.EditValue, 1)
            SafeTypeFrom.EditValue = dt.Rows(0)("SafeTypefrom")
            CurrencyFrom.EditValue = dt.Rows(0)("FirstCurrency")
            If PriceType.SelectedIndex = 1 Or PriceType.SelectedIndex = 0 Then
                LoadSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SafeTypeFrom.EditValue, CountryID.EditValue,
                           CurrencyFrom.EditValue, GridView1.GetFocusedRowCellValue("PrType"), GridView1.GetFocusedRowCellValue("Typesf"), 0)
            ElseIf PriceType.SelectedIndex = 2 Then
                If GridView1.GetFocusedRowCellValue("AccCode") = 0 Then
                    LoadSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SafeTypeFrom.EditValue, CountryID.EditValue,
                               CurrencyFrom.EditValue, 0, 0, 0)
                Else
                    LoadSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SafeTypeFrom.EditValue, CountryID.EditValue,
                               CurrencyFrom.EditValue, GridView1.GetFocusedRowCellValue("PrType"), GridView1.GetFocusedRowCellValue("Typesf"), 0)
                End If
            End If
            If PriceType.SelectedIndex = 2 Then
                If SafeTypeFrom.EditValue > 0 Then
                    If CurrencyFrom.EditValue IsNot Nothing And CurrencyFrom.EditValue <> -1 Then
                        LoadBank(SafeID, CurrencyFrom.EditValue, 0)
                    End If
                End If
            End If
            SafeID.EditValue = dt.Rows(0)("SaFACCount")
            If PriceType.SelectedIndex = 0 Or PriceType.SelectedIndex = 1 Then
                LOADCIDTO(0, CurrencyFrom.EditValue)
            Else
                LOADCIDTO(1, DefaultCurrency)
            End If
            CurrencyTo.EditValue = dt.Rows(0)("CurrencyTo")
            SafeTypeTo.EditValue = dt.Rows(0)("SafeTypeTo")
            If PriceType.SelectedIndex = 1 Or PriceType.SelectedIndex = 0 Then
                SAFFACCTO.Properties.DataSource = Nothing
                LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafeTypeTo.EditValue, CountryID.EditValue, CurrencyTo.EditValue,
                        gvroll.GetFocusedRowCellValue("PrType"), gvroll.GetFocusedRowCellValue("Typesf"), 0)
            ElseIf PriceType.SelectedIndex = 2 Then
                If gvroll.GetFocusedRowCellValue("AccCode") = 0 Then
                    LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafeTypeTo.EditValue, CountryID.EditValue, CurrencyTo.EditValue,
                                0, 0, 0)
                Else
                    LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafeTypeTo.EditValue, CountryID.EditValue, CurrencyTo.EditValue,
                                gvroll.GetFocusedRowCellValue("PrType"), gvroll.GetFocusedRowCellValue("Typesf"), 0)
                End If
                DVGFormat(gvroll)
            End If
            If PriceType.SelectedIndex = 2 Then
                If SafeTypeTo.EditValue <> -1 Then
                    If CurrencyTo.EditValue IsNot Nothing And CurrencyTo.EditValue <> -1 Then
                        LoadBank(SAFFACCTO, CurrencyTo.EditValue, 1)
                    End If
                End If
            End If
            SAFFACCTO.EditValue = dt.Rows(0)("AccSafeTo")
            BPrice1.EditValue = dt.Rows(0)("BPrice1")
            BPrice2.EditValue = dt.Rows(0)("BPrice2")
        End If
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        BtnPrint.Enabled = True
        BtnSave.Enabled = False
        BtnDelete.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = True
        disabled()
    End Sub
    Public Overrides Sub Print()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ID", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CurrenciesBuyandsellTB_loadtoFrm", PRM)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTINTCURSALES1
                dt.TableName = "CurrenciesBuyandsellTB"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                report.DataSource = ds
                report.DataMember = "CurrenciesBuyandsellTB"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        MyBase.Print()
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        SafeTypeFrom.Properties.DataSource = Nothing
        SafeTypeTo.Properties.DataSource = Nothing
        SafeID.Properties.DataSource = Nothing
        SAFFACCTO.Properties.DataSource = Nothing
        SafeTypeFrom.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        If BranchID.EditValue > 0 Then
            SafeTypeFrom.EditValue = -1
            SafeID.EditValue = -1
            SafeTypeTo.EditValue = -1
            SAFFACCTO.EditValue = -1
            CurrenciesBuyandsellTB_selectTYPe(SafeTypeFrom, PriceType.SelectedIndex, GridView1, BranchID.EditValue, 0)
            CurrenciesBuyandsellTB_selectTYPe(SafeTypeTo, PriceType.SelectedIndex, gvroll, BranchID.EditValue, 1)
            LoadCountries()
            CountryID.EditValue = COUNTRYNID
        End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        LoadCountries()
        CountryID.EditValue = COUNTRYNID
    End Sub
    Public BuyPurpose, CHTYPE, TypeofTransfer, BNKID, CurDetailsID, AccType As Integer
    Private Sub Purchaseprice_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles Purchaseprice.ButtonClick
        LOADATA()
        FRMBTNEOWNCURRENCYEDIT.PriceType.SelectedIndex = AccType
        If AccType = 2 Then
            FRMBTNEOWNCURRENCYEDIT.AccountType = 0
        End If
        If PriceType.SelectedIndex = 3 Then
            FRMBTNEOWNCURRENCYEDIT.BankID = BNKID
        End If
        FRMBTNEOWNCURRENCYEDIT.CurrencyPriceCategory(CurDetailsID, AccType, 1)
        FRMBTNEOWNCURRENCYEDIT.SalePrice.Enabled = False
        FRMBTNEOWNCURRENCYEDIT.ShowDialog()
    End Sub

    Private Sub SafeTypeFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeTypeFrom.QueryPopUp
        Dim cancelled As Boolean = False

        If CurrencyFrom.EditValue Is Nothing Or CurrencyFrom.Text = String.Empty Then
            cancelled = True
        End If

        If cancelled Then
            e.Cancel = True
            CurrencyFrom.ErrorText = "الرجاء اختيار العملة الأولى "
        End If
    End Sub

    Public Sub LOADATA()
        If PriceType.SelectedIndex = 2 Then
            If SafeTypeFrom.EditValue <> 0 Then
                If SafeID.EditValue = -1 Then
                    SafeID.ErrorText = "الرجاء اختيار الحساب المصرفي"
                    Return
                End If
            End If
        End If
        If CurrencyFrom.EditValue = -1 Then
            CurrencyFrom.ErrorText = "الرجاء اختيار العملة الاولى "
            Return
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Return
        End If
        If CountryID.EditValue Is Nothing Or CountryID.EditValue = -1 Then
            CountryID.ErrorText = "الرجاء اختيار الدولة"
            Return
        End If
        If PriceType.SelectedIndex = 0 Then
            AccType = 0
        ElseIf PriceType.SelectedIndex = 1 Then
            AccType = 1
        Else
            AccType = 3
        End If
        Try
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(6) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyIDFrom", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(1) = New SqlParameter("@AccounType", SqlDbType.Int) With {.Value = 0}
            prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
            prm(4) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BNKID}
            prm(5) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = AccType}
            prm(6) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            dt = RUN_QUARY_PRO("NewGetCurrencyPriceDetailsTb_ID", prm)
            If dt.Rows.Count > 0 Then
                CurDetailsID = dt.Rows(0)("CurDetailsID")

            Else
                InfoMessage(Me, "رسالة تنبيه", "لا يوجد سعر لهذا النوع من البيع، يرجى إضافة النوع من شاشة تسعير العملات")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub PriceType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PriceType.SelectedIndexChanged
        SafeTypeFrom.Properties.DataSource = Nothing
        SafeTypeTo.Properties.DataSource = Nothing
        SafeID.Properties.DataSource = Nothing
        SAFFACCTO.Properties.DataSource = Nothing
        SafeTypeFrom.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeFrom, PriceType.SelectedIndex, GridView1, BranchID.EditValue, 0)
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeTo, PriceType.SelectedIndex, gvroll, BranchID.EditValue, 1)
        If PriceType.SelectedIndex = 2 Then
            'CurrencyTo.EditValue = DefaultCurrency
            'CurrencyTo.Enabled = False
            'SAFFACCTO.Properties.DataSource = Nothing
            DVGFormat(gvroll)
            LayoutControlItem2.Visibility = XtraBars.BarItemVisibility.Always
            LayoutControlItem1.Visibility = XtraBars.BarItemVisibility.Always
            LayoutControlItem3.Visibility = XtraBars.BarItemVisibility.Always
            LayoutControlItem23.Visibility = XtraBars.BarItemVisibility.Always
            LayoutControlItem21.Visibility = XtraBars.BarItemVisibility.Always
            LayoutControlItem22.Visibility = XtraBars.BarItemVisibility.Always
        Else
            LayoutControlItem2.Visibility = XtraBars.BarItemVisibility.Never
            LayoutControlItem1.Visibility = XtraBars.BarItemVisibility.Never
            LayoutControlItem3.Visibility = XtraBars.BarItemVisibility.Never
            LayoutControlItem23.Visibility = XtraBars.BarItemVisibility.Never
            LayoutControlItem21.Visibility = XtraBars.BarItemVisibility.Never
            LayoutControlItem22.Visibility = XtraBars.BarItemVisibility.Never
            CurrencyTo.EditValue = -1
            CurrencyTo.Enabled = True
            SAFFACCTO.Properties.DataSource = Nothing

        End If
        If PriceType.SelectedIndex = 1 Then
            CountryID.Enabled = True
        Else
            CountryID.Enabled = False
        End If

        If PriceType.SelectedIndex = 2 Then
            If SafeTypeFrom.EditValue > 0 Then
                If CurrencyFrom.EditValue IsNot Nothing And CurrencyFrom.EditValue <> -1 Then
                    LoadBank(SafeID, CurrencyFrom.EditValue, 0)
                End If
            End If
            If SafeTypeTo.EditValue <> -1 Then
                If CurrencyTo.EditValue IsNot Nothing And CurrencyTo.EditValue <> -1 Then
                    LoadBank(SAFFACCTO, CurrencyTo.EditValue, 1)
                End If
            End If
        End If
        If PriceType.SelectedIndex = 2 Then
            BankID.EditValue = -1
            LoadBankMain(BankID, 0, 2)
        End If
        If PriceType.SelectedIndex = 2 Then

            BuyPurpose = thepurpose.SelectedIndex
            CHTYPE = Convert.ToInt32(CheckType.SelectedIndex)
            CusIDNo = NationalNo.Text
            Phone = CustPhone.Text
            CustName = CustomerName.Text
            AccAccountBank = AgentBankNumber.Text
            TypeofTransfer = Convert.ToInt32(TypeofTransaction.SelectedIndex)
            TransferNum = Convert.ToUInt64(TransactionNo.EditValue)
            LOADCIDTO(1, DefaultCurrency)
            CurrencyTo.EditValue = DefaultCurrency
            CurrencyTo.Enabled = False

        Else
            BNKID = 0
            BuyPurpose = 0
            CHTYPE = 0
            CusIDNo = "لا يوجد"
            Phone = "لا يوجد"
            CustName = "لا يوجد"
            AccAccountBank = "لا يوجد"
            TypeofTransfer = 0
            TransferNum = 0
            CurrencyTo.EditValue = -1
            CurrencyTo.Enabled = True
            SafeTypeTo.EditValue = -1
            SafeTypeTo.Enabled = True
        End If
    End Sub
    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        SafeTypeFrom.Properties.DataSource = Nothing
        SafeTypeTo.Properties.DataSource = Nothing
        SafeID.Properties.DataSource = Nothing
        SAFFACCTO.Properties.DataSource = Nothing
        SafeTypeFrom.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeFrom, PriceType.SelectedIndex, GridView1, BranchID.EditValue, 0)
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeTo, PriceType.SelectedIndex, gvroll, BranchID.EditValue, 1)
    End Sub
    Public TransferNum As ULong
    Public CusIDNo, Phone, CustName, AccAccountBank As String
    Private Sub PriceType_EditValueChanged(sender As Object, e As EventArgs) Handles PriceType.EditValueChanged
        SafeTypeFrom.Properties.DataSource = Nothing
        SafeTypeTo.Properties.DataSource = Nothing
        SafeID.Properties.DataSource = Nothing
        SAFFACCTO.Properties.DataSource = Nothing
        SafeTypeFrom.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        SafeTypeTo.EditValue = -1
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeFrom, PriceType.SelectedIndex, GridView1, BranchID.EditValue, 0)
        CurrenciesBuyandsellTB_selectTYPe(SafeTypeTo, PriceType.SelectedIndex, gvroll, BranchID.EditValue, 1)
        If PriceType.SelectedIndex = 0 Then
            ISbaunk = 0
        ElseIf PriceType.SelectedIndex = 1 Then
            ISbaunk = 1
        Else
            ISbaunk = 3
        End If
        If PriceType.SelectedIndex = 2 Then

            BuyPurpose = thepurpose.SelectedIndex
            CHTYPE = Convert.ToInt32(CheckType.SelectedIndex)
            CusIDNo = NationalNo.Text
            Phone = CustPhone.Text
            CustName = CustomerName.Text
            AccAccountBank = AgentBankNumber.Text
            TypeofTransfer = Convert.ToInt32(TypeofTransaction.SelectedIndex)
            TransferNum = Convert.ToUInt64(TransactionNo.EditValue)
            LOADCIDTO(1, DefaultCurrency)
            CurrencyTo.EditValue = DefaultCurrency
                CurrencyTo.Enabled = False
            'SafeTypeTo.EditValue = 0
            'SafeTypeTo.Enabled = False
        Else
            BNKID = 0
            BuyPurpose = 0
            CHTYPE = 0
            CusIDNo = "لا يوجد"
            Phone = "لا يوجد"
            CustName = "لا يوجد"
            AccAccountBank = "لا يوجد"
            TypeofTransfer = 0
            TransferNum = 0
            CurrencyTo.EditValue = -1
            CurrencyTo.Enabled = True
            SafeTypeTo.EditValue = -1
            SafeTypeTo.Enabled = True
        End If
    End Sub

    Private Sub CurrencyFrom_TextChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.TextChanged
        CurrFrmID = CurrencyFrom.EditValue
    End Sub
End Class