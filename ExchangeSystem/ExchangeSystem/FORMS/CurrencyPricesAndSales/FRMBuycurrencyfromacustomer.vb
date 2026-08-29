Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRMBuycurrencyfromacustomer
    Dim IDcod As ULong, msgST As Int16

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(35, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub LOADCIDTO()
        If CurrencyFrom.EditValue = -1 Or CurrencyFrom.Text = String.Empty Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة الأولى"
            Exit Sub
        End If
        CurrencyTo.Properties.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("CurrencyMainTb_LOADTOLKPNOTEXIST", PR)
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
    Sub LOADSafeID(NGridLookUpEdit As GridLookUpEdit, UserID As Integer, UESERTYPE As Integer, BRnchID As Integer, Type As ULong)
        Try

            NGridLookUpEdit.Properties.DataSource = Nothing
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = Type}
            PR(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PR(2) = New SqlParameter("@UESERTYPE", SqlDbType.Int) With {.Value = UESERTYPE}
            PR(3) = New SqlParameter("@BRnchID", SqlDbType.Int) With {.Value = BRnchID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("SafeID_LODE", PR)
            If dt.Rows.Count > 0 Then
                NGridLookUpEdit.Properties.DataSource = dt
                NGridLookUpEdit.Properties.ValueMember = "AccID"
                NGridLookUpEdit.Properties.DisplayMember = "AccName"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Sub LOADSafeID2(NGridLookUpEdit As GridLookUpEdit, UserID As Integer, UESERTYPE As Integer, BRnchID As Integer, Type As ULong)
        Try

            NGridLookUpEdit.Properties.DataSource = Nothing
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = Type}
            PR(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PR(2) = New SqlParameter("@UESERTYPE", SqlDbType.Int) With {.Value = UESERTYPE}
            PR(3) = New SqlParameter("@BRnchID", SqlDbType.Int) With {.Value = BRnchID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("SafeID_LODE3", PR)
            If dt.Rows.Count > 0 Then
                NGridLookUpEdit.Properties.DataSource = dt
                NGridLookUpEdit.Properties.ValueMember = "AccID"
                NGridLookUpEdit.Properties.DisplayMember = "AccName"


            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub CurrenciesBuyandsellTB_selectTYPe(NGridLookUpEdit As GridLookUpEdit, Typesf As Integer, GridLoo As GridView, brnchID As Integer)

        NGridLookUpEdit.Properties.DataSource = Nothing

        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = brnchID}
        PR(1) = New SqlParameter("@Typesf", SqlDbType.Int) With {.Value = Typesf}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CurrenciesBuyandsellTB_selectTYPe", PR)
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
        DateEdit1.Enabled = False
        CurrencyFrom.Enabled = False
        BPrice1.Enabled = False
        BPrice2.Enabled = False
        BranchID.Enabled = False

        BPrice2.Enabled = False
        SAFTypeform.Enabled = False
        SafTypeTo.Enabled = False
        SAFFACCTO.Enabled = False
        ISbaunk.Enabled = False
        ISbaunk.Enabled = False
        MemoEdit1.Enabled = False
        SafeID.Enabled = False
        Purchaseprice.Enabled = False
        TextEdit4.Enabled = False
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
        TextEdit3.Enabled = False
        ACCVALFORM.Enabled = False
        ACCValTo.Enabled = False
    End Sub

    Sub enabeld()
        DateEdit1.Enabled = True
        CurrencyFrom.Enabled = True
        BPrice1.Enabled = True
        BPrice2.Enabled = True
        SAFTypeform.Enabled = True
        SafTypeTo.Enabled = True
        SAFFACCTO.Enabled = True
        ISbaunk.Enabled = True
        ISbaunk.Enabled = True
        MemoEdit1.Enabled = True
        SafeID.Enabled = True
        Purchaseprice.Enabled = True
        TextEdit4.Enabled = True
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        TextEdit3.Enabled = True
        ACCVALFORM.Enabled = True
        ACCValTo.Enabled = True
    End Sub
    Public Sub NEWRecored()
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnPrint.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        CurrenciesBuyandsellTB_MAxID()
        DateEdit1.EditValue = Date.Now
        LOADCIDFROM()
        CurrencyFrom.EditValue = -1
        BPrice1.EditValue = 0.00
        CurrencyTo.EditValue = -1
        BPrice2.EditValue = 0.00
        CurrencyTo.EditValue = 1
        CurrencyTo.Enabled = False
        SAFTypeform.Properties.DataSource = Nothing
        SafTypeTo.Properties.DataSource = Nothing
        SafeID.Properties.DataSource = Nothing
        SAFFACCTO.Properties.DataSource = Nothing

        CurrenciesBuyandsellTB_selectTYPe(SAFTypeform, 0, GridView1, BranchID.EditValue)
        CurrenciesBuyandsellTB_selectTYPe(SafTypeTo, 0, gvroll, BranchID.EditValue)
        LOADBRNACH()
        DVGFormat(GridView1)
        DVGFormat(gvroll)
        SAFTypeform.EditValue = -1
        BranchID.EditValue = -1
        SafTypeTo.EditValue = -1
        SAFFACCTO.EditValue = -1
        ISbaunk.SelectedIndex = 0
        ISbaunk.SelectedIndex = 0
        MemoEdit1.Text = String.Empty
        SafeID.EditValue = -1
        Purchaseprice.EditValue = 0.00
        TextEdit4.SelectedIndex = 1
        BranchID.EditValue = BID
        TextEdit1.Text = String.Empty
        TextEdit2.Text = String.Empty
        TextEdit3.Text = String.Empty
        LayoutControlItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LayoutControlItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
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
            enabeld()
            MyBase.BNew()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ في النظام ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub CurrencyFrom_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.EditValueChanged
        If CurrencyFrom.EditValue > -1 Or CurrencyFrom.Text <> String.Empty Then
            LOADCIDTO()
            get_BracuNetBurnnc()
            If SAFTypeform.EditValue = -1 Or SAFTypeform.Text = String.Empty Then
                SAFTypeform.ErrorText = "الرجاء اختيار العملة الاولي"
                Return
            End If
            If SAFTypeform.EditValue = -1 Then
                SAFTypeform.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If SafeID.Text = String.Empty Then
                SafeID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If SAFTypeform.EditValue = 0 Then
                GETVALTOLTEl(SafeID.EditValue, CurrencyFrom.EditValue, ACCVALFORM, SAFTypeform.EditValue)
            Else
                GETVALTOLTEl(UserAccID, CurrencyFrom.EditValue, ACCVALFORM, 0)

            End If


        End If







    End Sub
    Public Sub GETVALTOLTEl(ACCTO As ULong, CurrencyFrom As Integer, WithdrawalValue As TextEdit, crunseType As Integer)
        Try






            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@ACCAcount", SqlDbType.BigInt) With {.Value = ACCTO}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = CurrencyFrom}
            PR(2) = New SqlParameter("@ACCTYPE", SqlDbType.Int) With {.Value = crunseType}
            PR(3) = New SqlParameter("@ISBANCK", SqlDbType.BigInt) With {.Value = ISbaunk.SelectedIndex}
            'PR(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("GET_VAL_FOR_CUST_ORSAFA", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.Text = dt.Rows(0)("GetAccVal")

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
    Private Sub BPrice11_KeyPress(sender As Object, e As KeyPressEventArgs)
        e.Handled = True

    End Sub
    Private Sub Code_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Code.KeyPress
        e.Handled = True
    End Sub
    Private Sub FRMINTCURSALES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        BtnNew.PerformClick()
        Me.Width = 813
        Me.Height = 770


    End Sub
    Public Sub get_BracuNetBurnnc()
        Try


            If CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "الرجاء اختيار العملة الاولى "
                Return
            End If



            If CurrencyTo.Text = String.Empty Then
                CurrencyTo.ErrorText = "الرجاء اختيار العملة الاولى "
                Exit Sub
            End If

            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            prm(1) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
            prm(2) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(3) = New SqlParameter("@BPrice11", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@Purchaseprice", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@ISbaunk", SqlDbType.Int) With {.Value = ISbaunk.SelectedIndex}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("get_BracuNetBurnnc_SalePrice", prm)



            If BPrice1.EditValue > 0 Then
                BPrice2.Text = prm(3).Value
            Else
                BPrice2.Text = 0.00
                BPrice1.ErrorText = "الرجاء ادخال القيمة الاولى "
            End If

            Purchaseprice.Text = prm(4).Value
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ في نظـــــــــام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub CurrencyTo_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyTo.EditValueChanged
        If CurrencyTo.EditValue > -1 Then
            get_BracuNetBurnnc()

            If SafTypeTo.EditValue = -1 Or SafTypeTo.Text = String.Empty Then
                SafTypeTo.ErrorText = "الرجاء اختيار العملة الاولي"
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



            GETVALTOLTEl(SAFFACCTO.EditValue, CurrencyTo.EditValue, ACCValTo, SafTypeTo.EditValue)

        End If







    End Sub
    Private Sub BPrice1_EditValueChanged(sender As Object, e As EventArgs) Handles BPrice1.EditValueChanged
        If CurrencyTo.EditValue > 0 Then
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
            Dim prm(20) As SqlParameter
            prm(0) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDcod}
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
            prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(3) = New SqlParameter("@SaFACCount", SqlDbType.Int) With {.Value = SafeID.EditValue}
            prm(4) = New SqlParameter("@FirstCurrency", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            prm(5) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
            prm(6) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(7) = New SqlParameter("@BPrice2", SqlDbType.Float) With {.Value = BPrice2.EditValue}
            prm(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = MemoEdit1.Text}
            prm(9) = New SqlParameter("@UeserInset", SqlDbType.Int) With {.Value = UserID}
            prm(10) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(12) = New SqlParameter("@IsCachorBank", SqlDbType.Bit) With {.Value = Convert.ToBoolean(ISbaunk.SelectedIndex)}
            prm(13) = New SqlParameter("@SAFTypeform", SqlDbType.BigInt) With {.Value = SAFTypeform.EditValue}
            prm(14) = New SqlParameter("@SafTypeTo", SqlDbType.BigInt) With {.Value = SafTypeTo.EditValue}
            prm(15) = New SqlParameter("@SAFFACCTO", SqlDbType.BigInt) With {.Value = SAFFACCTO.EditValue}
            prm(16) = New SqlParameter("@Purchaseprice", SqlDbType.Decimal, 18, 3) With {.Value = Purchaseprice.EditValue}
            prm(17) = New SqlParameter("@CusIDNo", SqlDbType.NVarChar, -1) With {.Value = TextEdit3.EditValue}
            prm(18) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = TextEdit2.Text}
            prm(19) = New SqlParameter("@thepurpose", SqlDbType.TinyInt) With {.Value = TextEdit4.SelectedIndex}
            prm(20) = New SqlParameter("@CustName", SqlDbType.NVarChar, -1) With {.Value = TextEdit1.Text}
            RUN_EXUTE_PRO("CurrenciesBuyandsellTSElecSPreceB_Insert", prm)
            msgST = prm(10).Value
            If prm(10).Value = 0 Or prm(10).Value = 2 Then
                MessageBox.Show(prm(11).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If prm(10).Value = 2 Then
                    CurrenciesBuyandsellTB_MAxID()
                End If
                Return
            Else
                FRMIViewNTCURSALES.IsBuyORSale = 2
                Print()
                BtnNew.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تحذير وجود مشكلة في نظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub CurrenciesBuyandsellTSElecSPreceB_Updata()
        Try
            Dim prm(20) As SqlParameter
            prm(0) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDcod}
            prm(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
            prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(3) = New SqlParameter("@SaFACCount", SqlDbType.Int) With {.Value = SafeID.EditValue}
            prm(4) = New SqlParameter("@FirstCurrency", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            prm(5) = New SqlParameter("@BPrice1", SqlDbType.Float) With {.Value = BPrice1.EditValue}
            prm(6) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(7) = New SqlParameter("@BPrice2", SqlDbType.Float) With {.Value = BPrice2.EditValue}
            prm(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = MemoEdit1.Text}
            prm(9) = New SqlParameter("@UeserInset", SqlDbType.Int) With {.Value = UserID}
            prm(10) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(12) = New SqlParameter("@IsCachorBank", SqlDbType.Bit) With {.Value = Convert.ToBoolean(ISbaunk.SelectedIndex)}
            prm(13) = New SqlParameter("@SAFTypeform", SqlDbType.BigInt) With {.Value = SAFTypeform.EditValue}
            prm(14) = New SqlParameter("@SafTypeTo", SqlDbType.BigInt) With {.Value = SafTypeTo.EditValue}
            prm(15) = New SqlParameter("@SAFFACCTO", SqlDbType.BigInt) With {.Value = SAFFACCTO.EditValue}
            prm(16) = New SqlParameter("@Purchaseprice", SqlDbType.Decimal, 18, 3) With {.Value = Purchaseprice.EditValue}
            prm(17) = New SqlParameter("@CusIDNo", SqlDbType.NVarChar, -1) With {.Value = TextEdit3.EditValue}
            prm(18) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = TextEdit2.Text}
            prm(19) = New SqlParameter("@thepurpose", SqlDbType.TinyInt) With {.Value = TextEdit4.SelectedIndex}
            prm(20) = New SqlParameter("@CustName", SqlDbType.NVarChar, -1) With {.Value = TextEdit1.Text}
            RUN_EXUTE_PRO("CurrenciesBuyandsellTSElecSPreceB_Updata", prm)
            msgST = prm(10).Value
            If prm(10).Value = 0 Then
                MessageBox.Show(prm(11).Value, "رسالةخطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Else
                FRMIViewNTCURSALES.IsBuyORSale = 4
                Print()
                BtnNew.PerformClick()
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

        If TextEdit1.EditValue = String.Empty Then
            TextEdit1.ErrorText = "الرجاء ادخال اسم العميل"
            Return
        End If

        If SAFTypeform.EditValue = 0 Then
            If SafTypeTo.EditValue = 0 Then
                If SafeID.EditValue <> SAFFACCTO.EditValue Then
                    SafeID.ErrorText = "عذرا يجب ان تكون الخزينة المباعة هيا نفسها الخزينة المستلمة"
                    Return
                End If
            End If
        Else
            If SafTypeTo.EditValue = 0 Then
                If UserAccID <> SAFFACCTO.EditValue Then
                    SAFFACCTO.ErrorText = "عذرا يجب ان تكون الخزينة المباعة هيا نفسها الخزينة المستلمة"
                    Return
                End If
            End If
        End If
        If BPrice1.EditValue > ACCVALFORM.EditValue Then

            BPrice1.ErrorText = "عذرا القيمة المصروفة أكبر من رصيد العملة"
            Return

        End If

        CurrenciesBuyandsellTB_Insert()

        If msgST = 1 Then
            MyBase.SetData()
        End If

    End Sub
    Private Sub BPrice2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles BPrice2.KeyPress
        e.Handled = True
    End Sub
    Private Sub TextEdit1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Purchaseprice.KeyPress
        e.Handled = True
    End Sub
    Dim CURnsSENT As String
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
    Private Sub TextEdit1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ISbaunk.SelectedIndexChanged
        If ISbaunk.SelectedIndex = 1 Then
            CurrencyTo.EditValue = 1
            CurrencyTo.Enabled = False
            SAFFACCTO.Properties.DataSource = Nothing
            CurrenciesBuyandsellTB_selectTYPe(SafTypeTo, 1, gvroll, BranchID.EditValue)
            DVGFormat(gvroll)
            LayoutControlItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem23.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem21.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        Else
            LayoutControlItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem23.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem21.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            CurrencyTo.EditValue = -1
            CurrencyTo.Enabled = True
            SAFFACCTO.Properties.DataSource = Nothing
            CurrenciesBuyandsellTB_selectTYPe(SafTypeTo, 0, GridView1, BranchID.EditValue)
        End If
    End Sub
    Private Sub TextEdit4_EditValueChanged(sender As Object, e As EventArgs) Handles SafTypeTo.EditValueChanged
        If SafTypeTo.EditValue > -1 Then

            SAFFACCTO.Properties.DataSource = Nothing
            LOADSafeID2(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafTypeTo.EditValue)

            DVGFormat(GridView3)
            If SafTypeTo.EditValue = 0 Then
                SAFFACCTO.EditValue = UserAccID
                SAFFACCTO.Enabled = False
            Else
                SAFFACCTO.EditValue = -1
                SAFFACCTO.Enabled = True
            End If
        End If
    End Sub
    Private Sub TextEdit7_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit7.KeyPress
        e.Handled = True
    End Sub
    Public Overrides Sub Save()
        SetData()

        MyBase.Save()

    End Sub
    Public Overrides Sub UPDATERECORD()
        If SAFTypeform.EditValue <> 0 Then
            GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, CurrencyFrom.EditValue)
            If (SAFEVAL * -1) < BPrice1.EditValue Then
                MessageBox.Show("رصيد العميل غير كافي الرجاء التأكد من رصيد العميل", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If
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

        If TextEdit1.EditValue = String.Empty Then
            TextEdit1.ErrorText = "الرجاء ادخال اسم العميل"
            Return
        End If

        If SAFTypeform.EditValue = 0 Then
            If SafTypeTo.EditValue = 0 Then
                If SafeID.EditValue <> SAFFACCTO.EditValue Then
                    SafeID.ErrorText = "عذرا يجب ان تكون الخزينة المباعة هيا نفسها الخزينة المستلمة"
                    Return
                End If
            End If
        Else
            If SafTypeTo.EditValue = 0 Then
                If UserAccID <> SAFFACCTO.EditValue Then
                    SAFFACCTO.ErrorText = "عذرا يجب ان تكون الخزينة المباعة هيا نفسها الخزينة المستلمة"
                    Return
                End If
            End If
        End If

        CurrenciesBuyandsellTSElecSPreceB_Updata()


        If msgST = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub


    Private Sub SAFTypeform_EditValueChanged(sender As Object, e As EventArgs) Handles SAFTypeform.EditValueChanged
        If SAFTypeform.EditValue > -1 Then
            SafeID.Properties.DataSource = Nothing
            LOADSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SAFTypeform.EditValue)
            If SAFTypeform.EditValue = 0 Then
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

            If SAFTypeform.EditValue = -1 Or SAFTypeform.Text = String.Empty Then
                SAFTypeform.ErrorText = "الرجاء اختيار العملة الاولي"
                Return
            End If
            If SAFTypeform.EditValue = -1 Then
                SAFTypeform.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If CurrencyFrom.Text = String.Empty Then
                SafeID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If SAFTypeform.EditValue = 0 Then
                GETVALTOLTEl(SafeID.EditValue, CurrencyFrom.EditValue, ACCVALFORM, SAFTypeform.EditValue)
            Else
                GETVALTOLTEl(UserAccID, CurrencyFrom.EditValue, ACCVALFORM, 0)

            End If



        End If
    End Sub

    Private Sub SAFFACCTO_EditValueChanged(sender As Object, e As EventArgs) Handles SAFFACCTO.EditValueChanged
        If SAFFACCTO.EditValue > -1 Then

            If SafTypeTo.EditValue = -1 Or SafTypeTo.Text = String.Empty Then
                SafTypeTo.ErrorText = "الرجاء اختيار العملة الاولي"
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
            GETVALTOLTEl(SAFFACCTO.EditValue, CurrencyTo.EditValue, ACCValTo, SafTypeTo.EditValue)
        End If
    End Sub

    Private Sub MemoEdit1_KeyDown(sender As Object, e As KeyEventArgs) Handles MemoEdit1.KeyDown
        If e.Handled = Keys.Enter Then
            BtnSave.PerformClick()
        End If
    End Sub

    Private Sub Code_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles Code.ButtonClick
        FRMIViewNTCURSALES.IsBuyORSale = 2
        FRMIViewNTCURSALES.ShowDialog()
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
        For i As Integer = 0 To GridView1.Columns.Count - 1
            gvrolls.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            gvrolls.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        gvrolls.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        gvrolls.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        gvrolls.OptionsView.EnableAppearanceEvenRow = True
        gvrolls.Appearance.OddRow.BackColor = Color.WhiteSmoke
        gvrolls.OptionsView.EnableAppearanceOddRow = True

        ' GridLocalizer.Active = New MyGridLocalizer()


    End Sub

    Sub GetRecord(x)
        BtnNew.PerformClick()

        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.NVarChar, -1) With {.Value = x}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CurrenciesBuyandsellTB_loadtoFrm", prm)
        If dt.Rows.Count > 0 Then
            Code.Text = dt.Rows(0)("Code").ToString
            DateEdit1.EditValue = dt.Rows(0)("InsertDate")
            BranchID.EditValue = dt.Rows(0)("BranchID")
            ISbaunk.SelectedIndex = dt.Rows(0)("IsCachorBank")
            TextEdit3.Text = dt.Rows(0)("CustIDNo").ToString
            TextEdit2.Text = dt.Rows(0)("Phone").ToString
            TextEdit8.EditValue = dt.Rows(0)("BankID")
            TextEdit7.EditValue = dt.Rows(0)("AccAcountBank")
            TextEdit10.EditValue = dt.Rows(0)("CheckType")
            TextEdit9.EditValue = dt.Rows(0)("AccAcountBank")
            TextEdit5.EditValue = dt.Rows(0)("Typeoftransaction")
            TextEdit6.EditValue = dt.Rows(0)("Transactionnumber")


            CurrencyFrom.EditValue = dt.Rows(0)("FirstCurrency")
            BPrice1.EditValue = dt.Rows(0)("BPrice1")
            TextEdit4.SelectedIndex = dt.Rows(0)("thepurpose")


            CurrencyTo.EditValue = dt.Rows(0)("CurrencyTo")
            Purchaseprice.EditValue = dt.Rows(0)("Purchasingprice")
            BPrice2.EditValue = dt.Rows(0)("BPrice2")
            MemoEdit1.Text = dt.Rows(0)("Notes")
            TextEdit1.Text = dt.Rows(0)("CusName")


            SAFTypeform.EditValue = dt.Rows(0)("SafeTypefrom")
            LOADSafeID(SafeID, UserID, GProfIDLog, BranchID.EditValue, SAFTypeform.EditValue)
            SafeID.EditValue = dt.Rows(0)("SaFACCount")
            SafTypeTo.EditValue = dt.Rows(0)("SafeTypeTo")
            ACCVALFORM.EditValue = dt.Rows(0)("ACCVALFORM")
            ACCValTo.EditValue = dt.Rows(0)("ACCValTo")
            LOADSafeID(SAFFACCTO, UserID, GProfIDLog, BranchID.EditValue, SafTypeTo.EditValue)
            SAFFACCTO.EditValue = dt.Rows(0)("AccSafeTo")
        End If





        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        BtnPrint.Enabled = True
        BtnSave.Enabled = False
        BtnDelete.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = True
        disabled()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue > 0 Then
            SAFTypeform.EditValue = -1
            SafeID.EditValue = -1
            SafTypeTo.EditValue = -1
            SAFFACCTO.EditValue = -1
            CurrenciesBuyandsellTB_selectTYPe(SAFTypeform, 0, GridView1, BranchID.EditValue)
            CurrenciesBuyandsellTB_selectTYPe(SafTypeTo, 0, gvroll, BranchID.EditValue)
        End If
    End Sub



    Public Overrides Sub Print()

        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True



        'If GVRole.RowCount = 0 Then
        '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ID", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CurrenciesBuyandsellTB_loadtoFrm", PRM)
            Dim ds As New DataSet
            dt.TableName = "CurrenciesBuyandsellTB"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                        Dim report As New RPTINTCURSALES1

                        report.XrLabel34.Text = SAFTypeform.Text
                        report.XrLabel62.Text = SafTypeTo.Text
                        report.XrLabel26.Text = Cur_Code(CurrencyTo.Text, BPrice2.Text, False, "n2")
                        report.XrLabel41.Text = Cur_Code(CurrencyFrom.Text, BPrice1.Text, True, "n2")
                        report.XrLabel19.Text = Cur_Code(CurrencyTo.Text, BPrice2.Text, True, "n2")

                        report.DataSource = ds
                        report.DataMember = "CurrenciesBuyandsellTB"
                        Dim tool As ReportPrintTool = New ReportPrintTool(report)
                        report.CreateDocument()
                        report.ShowPreview()
                        'Else
                        '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        MyBase.Print()
    End Sub
End Class