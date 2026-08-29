Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Tile
Imports DevExpress.XtraLayout
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports ExchangeSystem.ExchangeSystem
Imports MetroFramework

Public Class FrmShowSafeMovement
    Dim clswd As New CLSWITHDRAWALFORM
    Public IsUpdate, ExistVal, CanChangeSafe, CanChangeBranch As Boolean
    Dim rowTotal As GridRow
    Private _Helper As MyCellMergeHelper
    Public NET, Peroid As Integer
    Public OATotal, SafeTransferr As Double
    Dim WDCode As String
    Dim IDCode As ULong

    Sub LOADEMP()
        If BranchID.EditValue <> -1 Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PRM(0).Value = BranchID.EditValue
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("UserSafe_SelectSafeAccByBranch", PRM)
            If DT.Rows.Count > 0 Then
                SafeID.Properties.DataSource = DT
                SafeID.Properties.ValueMember = "AccID"
                SafeID.Properties.DisplayMember = "UName"
                SafeID.Properties.PopulateColumns()
                SafeID.Properties.ShowHeader = False
            End If
        End If
    End Sub

    Sub LOADCIDFROM()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CuName"
            CurrencyID.Properties.ShowHeader = False

        Else
            CurrencyID.Properties.DataSource = Nothing
        End If
    End Sub


    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Try


            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
            prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)
            If dt.Rows.Count > 0 Then
                BranchID.Enabled = dt.Rows(0)("Can_branch")
                SafeID.Enabled = dt.Rows(0)("Can_safID")
                If dt.Rows(0)("Can_Close_safid") = 0 Then LayoutControlItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                If dt.Rows(0)("CaN_calCylaTion") = 0 Then
                    LayoutControlItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                    Quantity.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                    CostOFBuyPriseLayout.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                    LayoutControlItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                Else
                    LayoutControlItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                    Quantity.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                    CostOFBuyPriseLayout.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                    LayoutControlItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                End If
            Else
                BranchID.Enabled = False
                SafeID.Enabled = False
                LayoutControlItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                LayoutControlItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                Quantity.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                CostOFBuyPriseLayout.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                LayoutControlItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If
        Catch ex As Exception
            MessageBox.Show($"{ex.Message}")
        End Try

    End Sub

    Sub LOADBankServices()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("BankServicesTb_LOADTOLISTBOX")
        If DT.Rows.Count > 0 Then
            BankServicesID.Properties.DataSource = DT
            BankServicesID.Properties.ValueMember = "ID"
            BankServicesID.Properties.DisplayMember = "ServiceName"
            BankServicesID.Properties.ShowHeader = False

        Else
            BankServicesID.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        SafeID.Properties.PopulateColumns()
        SafeID.Properties.Columns("AccID").Visible = False
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        GridControl2.DataSource = Nothing
        GridControl11.DataSource = Nothing
        GCROLE.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl3.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllPeroidTotal.EditValue = 0.000
        PreviewsBalance.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OATotal = 0.000
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then

            LOADEMP()
            LOADCIDFROM()
            'CurrencyID.Text = "دينار ليبي"
            'SafeID.Text = GetUserName
            SafeID.EditValue = UserAccID
        End If

        If SafeID.EditValue > -1 Or SafeID.Text <> String.Empty Then
            GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SafeID.EditValue)

        End If
    End Sub
    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        'Dim View As GridView = TryCast(sender, GridView)
        'Dim view As GridView = TryCast(sender, GridView)
        'If BranchID.EditValue = MAINBID Then
        '    If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
        '        If e.Column.FieldName = "مدين" Then
        '            e.Appearance.ForeColor = Color.Yellow
        '            e.Appearance.BackColor = Color.Green
        '        End If
        '        If e.Column.FieldName = "دائن" Then
        '            e.Appearance.ForeColor = Color.Yellow
        '            e.Appearance.BackColor = Color.Red
        '        End If
        '    Else
        '        If e.Column.FieldName = "مدين" Then
        '            e.Appearance.ForeColor = Color.Yellow
        '            e.Appearance.BackColor = Color.Red
        '        End If
        '        If e.Column.FieldName = "دائن" Then
        '            e.Appearance.ForeColor = Color.Yellow
        '            e.Appearance.BackColor = Color.Green
        '        End If
        '    End If
        'End If
    End Sub

    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
        DT.Dispose()
    End Sub




    '' Not showing the employee's safe locks
    Public Sub Noet_FromACCoiunrt_save()
        BranchID.Properties.DataSource = Nothing
        Dim prm(2) As SqlParameter
        prm(0) = New SqlParameter("@FrmScreensTb_ID", SqlDbType.Int) With {.Value = 24}
        prm(1) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = UserID}
        prm(2) = New SqlParameter("@ColmenValue", SqlDbType.Int) With {.Value = 2}
        Dim dt As New DataTable
        dt.Clear()

        dt = RUN_QUARY_PRO("cobrnch_lode_For_Setting_roles", prm)

        If dt.Rows.Count > 0 Then
            LayoutControlItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Quantity.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            CostOFBuyPriseLayout.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            LayoutControlItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        Else
            LayoutControlItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Quantity.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            CostOFBuyPriseLayout.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            LayoutControlItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        End If
    End Sub

    Sub LoadBankData()
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "الرجاء اختيار الخزنة"
            Exit Sub
        End If

        If D1.Text = String.Empty Then
            D1.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If


        If D2.Text = String.Empty Then
            D2.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If

        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "عذراً لايمكن ان يكون التاريخ الاول اكبر من تاريخ الثاني"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "الرجاء اختيار العملة"
            Exit Sub
        End If

        GridControl1.DataSource = Nothing
        GridView1.Columns.Clear()

        Dim dt As New DataTable
        dt.Clear()
        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        prm(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        prm(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
        prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}

        dt = RUN_QUARY_PRO("AccSafeActivityTb_SelectByBankEmSafe", prm)
        If dt.Rows.Count > 0 Then
            GridControl1.DataSource = dt
            NEWDVGFROMAT(GridView1)
            GridView1.Columns("#").Width = 70
            GridView1.Columns("طبيعة الحركة").Width = 700
        End If

    End Sub

    Sub LoadBankServicesIDData()
        If BankServicesID.EditValue = -1 Or BankServicesID.Text = String.Empty Then
            BankServicesID.ErrorText = "الرجاء اختيار نوع الخدمة"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "الرجاء اختيار الخزنة"
            Exit Sub
        End If

        If D1.Text = String.Empty Then
            D1.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If


        If D2.Text = String.Empty Then
            D2.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If

        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "عذراً لايمكن ان يكون التاريخ الاول اكبر من تاريخ الثاني"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "الرجاء اختيار العملة"
            Exit Sub
        End If

        GridControl3.DataSource = Nothing
        GridView2.Columns.Clear()

        Dim dt As New DataTable
        dt.Clear()
        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@BankService", SqlDbType.Int) With {.Value = BankServicesID.EditValue}
        prm(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        prm(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
        prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}

        dt = RUN_QUARY_PRO("AccSafeActivityTb_SelectByBankSerivecsEmSafe", prm)
        If dt.Rows.Count > 0 Then
            GridControl3.DataSource = dt
            NEWDVGFROMAT(GridView2)
            GridView2.Columns("#").Width = 70
            GridView2.Columns("اسم الحساب").Width = 300
            GridView2.Columns("طبيعة الحركة").Width = 500
            SumTotalBankServis()
        Else
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            OverAllTotal.EditValue = 0.000
        End If

    End Sub


    Public Sub LoadData()
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "الرجاء اختيار الخزنة"
            Exit Sub
        End If

        If D1.Text = String.Empty Then
            D1.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If


        If D2.Text = String.Empty Then
            D2.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If

        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "عذراً لايمكن ان يكون التاريخ الاول اكبر من تاريخ الثاني"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "الرجاء اختيار العملة"
            Exit Sub
        End If
        Try
            GCROLE.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(9) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
            prm(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
            prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            dt = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafe", prm)
            If dt.Rows.Count > 0 Then
                ExistVal = 1
                GCROLE.DataSource = dt
                _Helper = New MyCellMergeHelper(GVRole)
                _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
                NEWDVGFROMAT(GVRole)
                GVRole.OptionsFind.AllowFindPanel = True
                GVRole.ShowFindPanel()
                OverAllCredit.EditValue = prm(6).Value
                OverAllDebit.EditValue = prm(5).Value
                OverAllDebit.Properties.AppearanceDisabled.BackColor = Color.Green
                OverAllCredit.Properties.Appearance.BackColor = Color.Red
                OverAllPeroidTotal.EditValue = prm(8).Value
                PreviewsBalance.EditValue = dt.Rows(0)("OldPreviwovs")
                OverAllTotal.EditValue = dt.Rows(0)("OverAllNetTotal")
                If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                    Peroid = 1
                    OverAllPeroidTotal.BackColor = Color.Green
                    OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Green
                ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                    Peroid = -1
                    OverAllPeroidTotal.BackColor = Color.Red
                    OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Red
                End If
                'Dim PR(4) As SqlParameter
                'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                'PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
                'PR(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                'PR(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                'PR(4) = New SqlParameter("@ExistVal", SqlDbType.Bit) With {.Value = ExistVal}
                'Dim DT1 As New DataTable
                'DT1 = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafePreviews", PR)
                'Dim rDt As DataTable = TryCast(GCROLE.DataSource, DataTable)
                '_Helper = New MyCellMergeHelper(GVRole)
                'Dim row As DataRow = rDt.NewRow()
                'If DT1.Rows.Count > 0 Then
                '    row("الكود") = "رصيد سابق"
                '    row("مدين") = DT1.Rows(0)("مدين")
                '    row("دائن") = DT1.Rows(0)("دائن")
                '    dt.Rows.InsertAt(row, 0)
                '    GCROLE.DataSource = rDt
                '    _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
                'End If
                'If DT1.Rows(0)("مدين") > DT1.Rows(0)("دائن") Then
                '    PreviewsBalance.EditValue = DT1.Rows(0)("مدين")
                '    PreviewsBalance.BackColor = Color.Green
                'Else
                '    PreviewsBalance.EditValue = DT1.Rows(0)("دائن")
                '    PreviewsBalance.BackColor = Color.Red
                'End If
                'Dim PR1(2) As SqlParameter
                'PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                'PR1(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
                'PR1(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                'Dim DT2 As New DataTable
                'DT2 = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafeNetTotal", PR1)
                'If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                '    NET = 1
                '    OverAllTotal.EditValue = DT2.Rows(0)("أول")
                '    OverAllTotal.BackColor = Color.Green
                'Else
                '    NET = -1
                '    OverAllTotal.EditValue = DT2.Rows(0)("ثاني")
                '    OverAllTotal.BackColor = Color.Red
                'End If
                'ElseIf dt.Rows.Count = 0 Then
                '    GCROLE.DataSource = Nothing
                '    ExistVal = 0
                '    Dim PR3(4) As SqlParameter
                '    PR3(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                '    PR3(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
                '    PR3(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                '    PR3(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                '    PR3(4) = New SqlParameter("@ExistVal", SqlDbType.Bit) With {.Value = ExistVal}
                '    Dim DT11 As New DataTable
                '    DT11 = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafePreviews", PR3)
                '    If DT11.Rows.Count > 0 Then
                '        OverAllDebit.Properties.AppearanceDisabled.BackColor = Color.Green
                '        OverAllCredit.Properties.Appearance.BackColor = Color.Red
                '        OverAllPeroidTotal.Properties.Appearance.BackColor = Color.Green
                '        GCROLE.DataSource = DT11
                '        NEWDVGFROMAT(GVRole)
                '        GVRole.OptionsFind.AllowFindPanel = True
                '        GVRole.ShowFindPanel()
                '        If DT11.Rows(0)("مدين") > DT11.Rows(0)("دائن") Then
                '            PreviewsBalance.EditValue = DT11.Rows(0)("مدين")
                '            PreviewsBalance.BackColor = Color.Green
                '        Else
                '            PreviewsBalance.EditValue = DT11.Rows(0)("دائن")
                '            PreviewsBalance.BackColor = Color.Red
                '        End If
                '        Dim PR11(2) As SqlParameter
                '        PR11(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                '        PR11(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
                '        PR11(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                '        Dim DT22 As New DataTable
                '        DT22 = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafeNetTotal", PR11)
                '        If DT22.Rows(0)("أول") > DT22.Rows(0)("ثاني") Then
                '            NET = 1
                '            OverAllTotal.EditValue = DT22.Rows(0)("أول")
                '            OverAllTotal.BackColor = Color.Green
                '        Else
                '            NET = -1
                '            OverAllTotal.EditValue = DT22.Rows(0)("ثاني")
                '            OverAllTotal.BackColor = Color.Red
                '        End If
                '    End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub FrmBranshStoreValue_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'LOADBRANCH()
        LOADBRANCH()
        BranchID.EditValue = -1
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        'lodePreportes()
        NEWDVGFROMAT(GVRole)
        GVRole.OptionsFind.AllowFindPanel = True
        GVRole.ShowFindPanel()
        'LOADCURRENCY()
        BranchID.EditValue = BID
        BankServicesID.EditValue = -1
        LOADCIDFROM()
        LOADBankServices()
        'SafeID.EditValue = -1
        GCROLE.DataSource = Nothing
        OverAllDebit.EditValue = 0.00
        OverAllCredit.EditValue = 0.00
        OverAllPeroidTotal.EditValue = 0.00
        PreviewsBalance.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        OATotal = 0.00
        If OverAllDebit.EditValue > OverAllCredit.EditValue Then
            Peroid = 1
            OverAllPeroidTotal.BackColor = Color.Green
            OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Green
        ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
            Peroid = -1
            OverAllPeroidTotal.BackColor = Color.Red
            OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Red
        Else
            OverAllPeroidTotal.BackColor = Color.DodgerBlue
            OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.DodgerBlue
        End If
        If SafeID.EditValue > -1 Or SafeID.Text <> String.Empty Then
            GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SafeID.EditValue)
        Else
            SafeID.ErrorText = "هذا الحقل مطلوب"
        End If
        CalcValue.EditValue = 0.000
        CalcType.SelectedIndex = -1
        TabbedControlGroup2.SelectedTabPageIndex = 0
        NEWDVGFROMAT(GridView11)
        NEWDVGFROMAT(GridView1)
        NEWDVGFROMAT(GridView2)
        NEWDVGFROMAT(GridView111)
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        CurrencyID.EditValue = 1
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 24)
    End Sub

    Private Sub BranchIDd_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllPeroidTotal.EditValue = 0.000
        PreviewsBalance.EditValue = 0.000
        If TabbedControlGroup2.SelectedTabPageIndex = 0 Then
            LoadData()
        End If
        If TabbedControlGroup2.SelectedTabPageIndex = 2 Then
            LoadBankData()
        End If
        If TabbedControlGroup2.SelectedTabPageIndex = 1 Then
            LoadBankServicesIDData()
        End If

        If TabbedControlGroup2.SelectedTabPageIndex = 4 Then
            GET_Edited_InternalEX()
        End If
        'If SafeID.EditValue > -1 Or SafeID.Text <> String.Empty Then
        '    GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SafeID.EditValue)
        'Else
        '    SafeID.ErrorText = "هذا الحقل مطلوب"
        'End If
    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub OverAllDebit_TextChanged(sender As Object, e As EventArgs) Handles OverAllDebit.TextChanged
        OverAllDebit.Properties.AppearanceDisabled.BackColor = Color.Green
        OverAllDebit.BackColor = Color.Green
        OverAllDebit.Properties.Appearance.BackColor = Color.Green
    End Sub
    'Sub ADDCOLUMN()
    '    Dim colCounter As GridColumn = GVRole.Columns.AddVisible("RowHandle")
    '    colCounter.Caption = "#"
    '    colCounter.VisibleIndex = 0
    '    colCounter.Width = 70
    '    colCounter.UnboundType = DevExpress.Data.UnboundColumnType.Integer
    '    colCounter.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
    'End Sub
    'Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
    '    'If e.Column.FieldName = "SN" And e.IsGetData Then
    '    '    e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
    '    'End If
    '    If e.Column.FieldName = "RowHandle" And e.IsGetData Then
    '        e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
    '    End If

    'End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "الرجاء اختيار الخزنة"
            Exit Sub
        End If
        FrmShowCurBranchAccounts.CurrencyTo.EditValue = CurrencyID.EditValue
        FrmShowCurBranchAccounts.TextEdit4.SelectedIndex = 2
        FrmShowCurBranchAccounts.ShowDialog()
    End Sub

    Private Sub SafeID_EditValueChanged(sender As Object, e As EventArgs) Handles SafeID.EditValueChanged
        GridControl2.DataSource = Nothing
        GridControl11.DataSource = Nothing
        GCROLE.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl3.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllPeroidTotal.EditValue = 0.000
        PreviewsBalance.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If SafeID.EditValue > -1 Or SafeID.Text <> String.Empty Then
            GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SafeID.EditValue)
        End If
    End Sub

    Public Sub GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SAFID As Integer)
        Try
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "الرجاء اختيار الخزنة"
                Exit Sub
            End If

            Dim dt As New DataTable
            dt.Clear()
            GridControl2.DataSource = Nothing
            GridControl11.DataSource = Nothing
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ACCID", SqlDbType.BigInt) With {.Value = SAFID}
            dt = RUN_QUARY_PRO("GET_TABLE_FOR_Costof_UESER_AccACount_PROC", prm)
            If dt.Rows.Count > 0 Then
                GridControl11.DataSource = dt
                GridControl2.DataSource = dt
                'SafeTransferr = dt.Rows(0)("SafeTransfer")
                NEWDVGFROMAT(GridView11)
                GridView11.Columns("SN").Width = 70
                GridView11.Columns("SN").VisibleIndex = 0
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub GET_Edited_InternalEX()
        Try
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "الرجاء اختيار الخزنة"
                Exit Sub
            End If

            Dim dt As New DataTable
            dt.Clear()
            GridControl21.DataSource = Nothing
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
            prm(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            dt = RUN_QUARY_PRO("InternalEx_Log_Statement", prm)
            If dt.Rows.Count > 0 Then
                GridControl21.DataSource = dt
                NEWDVGFROMAT(GridView111)
                GridView11.Columns("SN").Width = 70
                GridView11.Columns("SN").VisibleIndex = 0
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public PreBalance As Decimal
    Sub print()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            If TabbedControlGroup2.SelectedTabPageIndex = 0 Then
                If GVRole.RowCount = 0 Then
                    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                Dim prm(9) As SqlParameter
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                prm(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
                prm(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
                prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafe", prm)
                If dt.Rows.Count > 0 Then
                    PreBalance = prm(9).Value

                    Dim report As New RPTShowSafeMovement
                    report.DataSource = dt
                    'report.DataAdapter = DA
                    report.DataMember = "AccSafeActivityTb"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()
                End If
            End If



            If TabbedControlGroup2.SelectedTabPageIndex = 1 Then
                If GridView2.RowCount = 0 Then
                    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                Dim pr(4) As SqlParameter
                pr(0) = New SqlParameter("@BankService", SqlDbType.Int) With {.Value = BankServicesID.EditValue}
                pr(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
                pr(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                pr(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                pr(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
                Dim dt1 As New DataTable
                dt1.Clear()
                dt1 = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_SelectByBankSerivecsEmSafe", pr)
                If dt1.Rows.Count > 0 Then
                    Dim report1 As New XtraReport3
                    report1.DataSource = dt1
                    'report.DataAdapter = DA
                    report1.DataMember = "AccSafeActivityTb"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report1)
                    report1.CreateDocument()
                    report1.ShowPreview()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        print()
    End Sub

    Private Sub TileView11_ItemCustomize(sender As Object, e As TileViewItemCustomizeEventArgs) Handles TileView11.ItemCustomize
        e.Item.Elements(1).Text = ""
        e.Item.Elements(3).Text = ""
        If e.Item.Elements(0).Text.Contains("الليبي") Then
            e.Item.Elements(2).Text = ""
            e.Item.Elements(4).Text = ""
            e.Item.Elements(5).Text = ""
            'e.Item.Elements(6).Text = ""
            e.Item.Elements(7).Text = "رصيد العملة"
            e.Item.Elements(8).Text = ""
            'e.Item.Elements(9).Text = ""
            'e.Item.Elements(12).Text = ""
            'e.Item.Elements(13).Text = ""

            'e.Item.Elements(9).Text = "العملة المحلية"

            e.Item.Elements(0).Appearance.Normal.FontSizeDelta = 3
            'e.Item.Elements(9).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(3).Appearance.Normal.FontSizeDelta = 3
            e.Item.Elements(6).Appearance.Normal.FontSizeDelta = 3
            e.Item.Elements(7).Appearance.Normal.FontSizeDelta = 3
        Else
            'e.Item.Elements(9).Text = "العملة"
            'e.Item.Elements(9).Appearance.Normal.ForeColor = Color.White
            'e.Item.Elements(0).Appearance.Normal.ForeColor = Color.White
            e.Item.Elements(0).Appearance.Normal.FontSizeDelta = 2
            e.Item.Elements(9).Appearance.Normal.FontSizeDelta = 2
        End If

        If e.Item.Elements(6).Text.StartsWith("-") Then

                e.Item.Elements(6).Appearance.Normal.ForeColor = Color.Red
                e.Item.Elements(7).Appearance.Normal.ForeColor = Color.Red

            End If
            If e.Item.Elements(6).Text.StartsWith("-") = False Then
            e.Item.Elements(6).Appearance.Normal.ForeColor = Color.Lime
            e.Item.Elements(7).Appearance.Normal.ForeColor = Color.Lime

        End If
    End Sub

    Public Sub WITHDRAWAL_MaxID()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@typID", SqlDbType.Int) With {.Value = 17}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(2) = New SqlParameter("@USRID", SqlDbType.Int) With {.Value = UserID}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = COUNTRYNID}
        PRM(4) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = CITYID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("WithdrawalTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            WDCode = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub WITHDRAWAL_SafeDailyClose()
        Dim PRM(16) As SqlParameter
        PRM(0) = New SqlParameter("@WDCode", SqlDbType.NVarChar, -1) With {.Value = WDCode}
        PRM(1) = New SqlParameter("@WithDrawalDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(2) = New SqlParameter("@WithdrawalFrom", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
        PRM(3) = New SqlParameter("@WithdrawalTo", SqlDbType.BigInt) With {.Value = 0}
        PRM(4) = New SqlParameter("@WithdrawalValue", SqlDbType.Decimal) With {.Value = OverAllTotal.EditValue}
        PRM(5) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = ""}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(7) = New SqlParameter("@SAFEID", SqlDbType.Int) With {.Value = UserID}
        PRM(8) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        PRM(9) = New SqlParameter("@WIDCode", SqlDbType.BigInt) With {.Value = IDCode}
        PRM(10) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = "إقفال خزنة الموظف" & Space(1) & "/" & SafeID.Text}
        PRM(11) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = "تم اقفال خزنة الموظف ونقل القيمه لخزينة" & Space(1) & CurrencyID.Text}
        PRM(12) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(13) = New SqlParameter("@TypID", SqlDbType.Int) With {.Value = 17}
        PRM(14) = New SqlParameter("@DailyClose", SqlDbType.Bit) With {.Value = 1}
        PRM(15) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(16) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        'PRM(13) = New SqlParameter("@WIDCode", SqlDbType.Int) With {.Value = WIDCode}
        RUN_EXUTE_PRO("WithdrawalTb_Insert", PRM)

    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "الرجاء اختيار الخزنة"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "الرجاء اختيار العملة"
            Exit Sub
        End If
        If GVRole.RowCount <= 1 Then
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookAndFeelError, "الرجاء عرض حركة الموظف لتتم عملية الاقفال", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Exit Sub
        End If
        Dim reslut = XtraMessageBox.Show("هل ترغب حقا في إقفال خزنة" & Space(1) & CurrencyID.Text & Space(1) & "للموظف", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If reslut = DialogResult.Yes Then
            WITHDRAWAL_MaxID()
            WITHDRAWAL_SafeDailyClose()
            OverAllDebit.EditValue = 0.00
            OverAllCredit.EditValue = 0.00
            OverAllPeroidTotal.EditValue = 0.00
            PreviewsBalance.EditValue = 0.00
            If TabbedControlGroup2.SelectedTabPageIndex = 0 Then
                OverAllTotal.EditValue = 0.000

            End If

            LoadData()
            GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SafeID.EditValue)
            print()
        End If

    End Sub
    Sub deficit_OR_surplus_Calc()
        Dim PRM(7) As SqlParameter
        PRM(0) = New SqlParameter("@CalcType", SqlDbType.Bit) With {.Value = CalcType.SelectedIndex}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(2) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
        PRM(3) = New SqlParameter("@CalcValue", SqlDbType.Decimal) With {.Value = CalcValue.EditValue}
        PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(5) = New SqlParameter("@SAFEID", SqlDbType.Int) With {.Value = UserID}
        PRM(6) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        PRM(7) = New SqlParameter("@BuyPrice", SqlDbType.Decimal) With {.Value = CostOFBuyPrise.EditValue}
        RUN_EXUTE_PRO("deficit_OR_surplus_Calc", PRM)
    End Sub
    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "الرجاء اختيار الخزنة"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "الرجاء اختيار العملة"
            Exit Sub
        End If
        If CalcType.SelectedIndex = -1 Then
            CalcType.ErrorText = "هذا  الحقل لايجب أن يكون فارغ"
            Exit Sub
        End If
        If CalcValue.EditValue <= 0.000 Or CalcValue.Text = String.Empty Then
            CalcValue.ErrorText = "القيمة لا يجب أن تكون أقل من أو تساوي صفر"
            Exit Sub
        End If
        If CalcType.SelectedIndex = 0 Then
            If CalcValue.EditValue > OverAllTotal.EditValue Then
                ErrorMessage(Me, "رسالة خطأ", "قيمة العجز لا يجب أن تكون أكبر من القيمة الموجودة في الخزينة")
                Exit Sub
            End If
            If SafeID.EditValue = 302 Or SafeID.EditValue = 303 Or SafeID.EditValue = 304 Then  ''يجب تغيير هذا الكود وربطه بأي مستخدم ليس لديه حساب موظف
                ErrorMessage(Me, "رسالة خطأ", "لا يمكن إجراء عجز على هذا المستخدم لأنه ليس  موظف")
                Exit Sub
            End If
        End If
        deficit_OR_surplus_Calc()
        OverAllDebit.EditValue = 0.00
        OverAllCredit.EditValue = 0.00
        OverAllPeroidTotal.EditValue = 0.00
        PreviewsBalance.EditValue = 0.00
        CalcValue.EditValue = 0.000
        CalcType.SelectedIndex = -1
        If TabbedControlGroup2.SelectedTabPageIndex = 0 Then
            OverAllTotal.EditValue = 0.000
        End If
        LoadData()
        GET_TABLE_FOR_Costof_UESER_AccACount_PROC(SafeID.EditValue)
    End Sub

    Private Sub CurrencyID_TextChanged(sender As Object, e As EventArgs) Handles CurrencyID.TextChanged
        If UserType = 0 Then
            If CurrencyID.EditValue = 1 Or CurrencyID.EditValue = -1 Then
                Quantity.Text = "القيمة"
                CostOFBuyPriseLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Else
                Quantity.Text = "الكمية"
                CostOFBuyPriseLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If
        End If
        GridControl1.DataSource = Nothing
        GridControl3.DataSource = Nothing
        GCROLE.DataSource = Nothing
    End Sub



    Private Sub TabbedControlGroup2_SelectedPageChanged(sender As Object, e As LayoutTabPageChangedEventArgs) Handles TabbedControlGroup2.SelectedPageChanged
        GridControl1.DataSource = Nothing
        GridControl3.DataSource = Nothing
        GCROLE.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllPeroidTotal.EditValue = 0.000
        PreviewsBalance.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If TabbedControlGroup2.SelectedTabPageIndex = 4 Then
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        If TabbedControlGroup2.SelectedTabPageIndex = 3 Then
            Dim TotalLB As New GridColumnSummaryItem()
            TotalLB.SummaryType = SummaryItemType.Sum
            TotalLB.FieldName = "LocalBalance"
            GridView11.Columns("LocalBalance").Summary.Add(TotalLB)
            OverAllTotal.EditValue = Convert.ToDouble(GridView11.Columns("LocalBalance").SummaryItem.SummaryValue)
            LayoutControlItem13.Text = "إجمالي الرصيد بالعملة المحلية"
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            OverAllTotal.BackColor = Color.DodgerBlue
        End If

        If TabbedControlGroup2.SelectedTabPageIndex = 0 Then
            LayoutControlItem13.Text = "الصافي"
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            OverAllTotal.EditValue = OATotal
            If OverAllTotal.EditValue > 0 Then
                OverAllTotal.BackColor = Color.Green
            ElseIf OverAllTotal.EditValue < 0 Then
                OverAllTotal.BackColor = Color.Red
            Else
                OverAllTotal.BackColor = Color.DodgerBlue
            End If

        End If
        If TabbedControlGroup2.SelectedTabPageIndex = 2 Then

            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        If TabbedControlGroup2.SelectedTabPageIndex = 1 Then

            LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            LayoutControlItem13.Text = "الصافي"
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            LayoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            SumTotalBankServis()
        End If
    End Sub

    Private Sub GridView11_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView11.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub



    'Private Sub GridView11_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView11.CustomUnboundColumnData
    '    If e.Column.FieldName = "SN" And e.IsGetData Then
    '        e.Value = GridView11.GetRowHandle(e.ListSourceRowIndex) + 1
    '    End If
    'End Sub

    Private Sub BankServicesID_TextChanged(sender As Object, e As EventArgs) Handles BankServicesID.TextChanged
        GridControl3.DataSource = Nothing
    End Sub

    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Dim view As GridView = TryCast(sender, GridView)
        If BranchID.EditValue = MAINBID Then
            If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
                If e.Column.FieldName = "دائن" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Green
                End If
                If e.Column.FieldName = "مدين" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Red
                End If
            Else
                If e.Column.FieldName = "دائن" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Red
                End If
                If e.Column.FieldName = "مدين" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Green
                End If
            End If
        End If
    End Sub

    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub GridView2_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView2.RowCellStyle
        Dim view As GridView = TryCast(sender, GridView)
        If BranchID.EditValue = MAINBID Then
            If view.IsRowVisible(e.RowHandle) = RowVisibleState.Visible Then
                If e.Column.FieldName = "دائن" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Red
                End If
                If e.Column.FieldName = "مدين" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Green
                End If
            Else
                If e.Column.FieldName = "دائن" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Green
                End If
                If e.Column.FieldName = "مدين" Then
                    e.Appearance.ForeColor = Color.Yellow
                    e.Appearance.BackColor = Color.Red
                End If
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Dim prm(9) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        prm(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        prm(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
        prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AccSafeActivityTb_SelectByEmSafe", prm)
        If dt.Rows.Count > 0 Then
            PreBalance = prm(9).Value

            Dim report As New RPTShowSafeMovement
            report.DataSource = dt
            'report.DataAdapter = DA
            report.DataMember = "AccSafeActivityTb"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            Dim pdfOptions As ImageExportOptions = report.ExportOptions.Image
            Dim stordpath As String
            stordpath = Application.StartupPath & "\TEMPWATS"

            report.CreateDocument()
            Directory.CreateDirectory(stordpath)
            Dim newfilepathe As String
            newfilepathe = stordpath & "\" & "watsappmassg.jpeg"


            report.ExportToImage(newfilepathe, pdfOptions)

            If MAINBID = BranchID.EditValue Then
                SINTWATSAPP_PDF_CLINT(get_gruop_id(BranchID.EditValue, 2), newfilepathe, "حركة خزينة " & Space(1) & SafeID.Text)
            Else
                SINTWATSAPP_PDF_CLINT(get_gruop_id(BranchID.EditValue), newfilepathe, "حركة خزينة " & Space(1) & SafeID.Text)
            End If
        End If
    End Sub

    Private Sub FrmShowSafeMovement_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        FRMMAIN.Timer2.Start()
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        Try
            Dim report As New RPTPrintdailySafstatement
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub FrmShowSafeMovement_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown, BranchID.KeyDown, SafeID.KeyDown, CurrencyID.KeyDown, GCROLE.KeyDown, GridControl1.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub

    Private Sub GridView2_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView2.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub BankServicesID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BankServicesID.QueryPopUp
        BankServicesID.Properties.PopulateColumns()
        BankServicesID.Properties.Columns("ID").Visible = False
    End Sub

    Sub SumTotalBankServis()
        If GridView2.RowCount > 0 Then
            Dim OverallVal As New GridColumnSummaryItem()
            OverallVal.SummaryType = SummaryItemType.Sum
            OverallVal.FieldName = "مدين"
            GridView2.Columns("مدين").Summary.Add(OverallVal)
            Dim ExVal As New GridColumnSummaryItem()
            ExVal.SummaryType = SummaryItemType.Sum
            ExVal.FieldName = "دائن"
            GridView2.Columns("دائن").Summary.Add(ExVal)
            OverAllCredit.EditValue = Convert.ToDouble(GridView2.Columns("دائن").SummaryItem.SummaryValue)
            'ExValtotal.Properties.Appearance.BackColor = Color.Green
            OverAllDebit.EditValue = Convert.ToDouble(GridView2.Columns("مدين").SummaryItem.SummaryValue)

            OverAllTotal.EditValue = OverAllDebit.EditValue - OverAllCredit.EditValue
            If OverAllTotal.EditValue > 0 Then
                OverAllTotal.BackColor = Color.Green
            ElseIf OverAllTotal.EditValue < 0 Then
                OverAllTotal.BackColor = Color.Red
            Else
                OverAllTotal.BackColor = Color.DodgerBlue
            End If
        End If
    End Sub
End Class