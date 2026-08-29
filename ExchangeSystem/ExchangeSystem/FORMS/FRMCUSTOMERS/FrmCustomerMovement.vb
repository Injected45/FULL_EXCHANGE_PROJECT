Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports ExchangeSystem.ExchangeSystem
Imports SelectPdf
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading

Public Class FrmCustomerMovement
    Private _Helper As MyCellMergeHelper
    Dim IsValIn As Boolean
    Public TypeNewRe As Integer

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(54, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSearch") = 0 Then LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then LayoutControlItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If

    End Sub
    Sub NEWRECORD()
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRNCHDIERCT(BranchID)
        OverAllTotal.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        GCRole.DataSource = Nothing
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        CUST.EditValue = -1
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now

        If OverAllCredit.EditValue = 0.000 Then
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
        Else
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
        End If
        If OverAllCredit.EditValue = 0.000 Then
            OverAllDebit.Properties.Appearance.BackColor = Color.Red
        Else
            OverAllDebit.Properties.Appearance.BackColor = Color.Red
        End If
        NEWDVGFROMAT(GVRole)
        LOADCIDFROMT()
        GVRole.Columns.Clear()
        GVRole1.Columns.Clear()
        GVRole11.Columns.Clear()
        GCRole.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl11.DataSource = Nothing
        CurrencyTo.EditValue = -1
        TabbedControlGroup1.SelectedTabPageIndex = 0
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 54)
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
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub
    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}

        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DT.Rows.Add(1, "دينار ليبي")
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"
            DVGFormat(GridView2)
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADDATA()
        GVRole.Columns.Clear()
        GVRole1.Columns.Clear()
        GVRole11.Columns.Clear()
        GCRole.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl11.DataSource = Nothing
        'GVRole.BeginUpdate()
        NEWDVGFROMAT(GVRole)
        NEWDVGFROMAT(GVRole1)
        NEWDVGFROMAT(GVRole11)
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If CUST.EditValue = -1 Then
            CUST.ErrorText = "يجب اختيار اسم العميل"
            Return
        End If
        If CurrencyTo.EditValue = -1 Then
            CurrencyTo.ErrorText = "يجب اختيار العملة"
            Return
        End If
        If D1.EditValue Is Nothing Then
            D1.ErrorText = "يجب اختيار التاريخ أولاً"
            Return
        End If
        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية"
            Return
        End If
        Dim DT As New DataTable
        DT.Clear()
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = CUST.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CurrencyTo ", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        PRM(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}

        DT = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENT", PRM)
        GVRole.Columns.Clear()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        IsValIn = 0

        If DT.Rows.Count >= 0 Then
            If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                GCRole.DataSource = DT
                OverAllDebit.EditValue = 0.000
                OverAllCredit.EditValue = 0.000
                NEWDVGFROMAT(GVRole)
            ElseIf TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                GridControl1.DataSource = DT
                OverAllDebit.EditValue = 0.000
                OverAllCredit.EditValue = 0.000
                NEWDVGFROMAT(GVRole1)
            ElseIf TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                GridControl11.DataSource = DT
                OverAllDebit.EditValue = 0.000
                OverAllCredit.EditValue = 0.000
                NEWDVGFROMAT(GVRole11)
            End If


            If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                OverAllTotal.BackColor = Color.Red
            Else
                OverAllTotal.BackColor = Color.Green
            End If



            Dim PR111(2) As SqlParameter
            PR111(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = CUST.EditValue}
            PR111(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            PR111(2) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}

            Dim DT21 As New DataTable
            DT21 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTNETTOTAL", PR111)
            If DT21.Rows(0)("أول") > DT21.Rows(0)("ثاني") Then
                OverAllTotal1.EditValue = DT21.Rows(0)("أول")
                OverAllTotal1.BackColor = Color.Red
            Else
                OverAllTotal1.EditValue = DT21.Rows(0)("ثاني")
                OverAllTotal1.BackColor = Color.Green
            End If
            If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
                GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Green
            End If
            If TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                GVRole1.Columns("مدين").AppearanceCell.BackColor = Color.Red
                GVRole1.Columns("دائن").AppearanceCell.BackColor = Color.Green
            End If
            If TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                GVRole11.Columns("مدين").AppearanceCell.BackColor = Color.Red
                GVRole11.Columns("دائن").AppearanceCell.BackColor = Color.Green
            End If
            NEWDVGFROMAT(GVRole)
            NEWDVGFROMAT(GVRole1)
            NEWDVGFROMAT(GVRole11)
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Green
            If OverAllCredit.EditValue = 0.000 Then
                OverAllCredit.Properties.Appearance.BackColor = Color.Green
            Else
                OverAllCredit.Properties.Appearance.BackColor = Color.Green
            End If
            If OverAllCredit.EditValue = 0.000 Then
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            Else
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            End If


            Dim PR11(4) As SqlParameter
            PR11(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
            PR11(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR11(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
            PR11(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            PR11(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
            Dim DT1 As New DataTable
            DT1 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTPREVIEWS", PR11)
            Dim rDt As DataTable
            Dim row As DataRow
            If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                rDt = TryCast(GCRole.DataSource, DataTable)
                _Helper = New MyCellMergeHelper(GVRole)
                row = rDt.NewRow()
                If DT1.Rows.Count > 0 Then
                    row("الرمز") = "رصيد منقول"
                    row("مدين") = DT1.Rows(0)("مدين")
                    row("دائن") = DT1.Rows(0)("دائن")
                    DT.Rows.InsertAt(row, 0)
                    GCRole.DataSource = rDt
                    _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")

                End If
            ElseIf TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                rDt = TryCast(GridControl1.DataSource, DataTable)
                _Helper = New MyCellMergeHelper(GVRole1)
                row = rDt.NewRow()

                If DT1.Rows.Count > 0 Then
                    row("الرمز") = "رصيد منقول"
                    row("مدين") = DT1.Rows(0)("مدين")
                    row("دائن") = DT1.Rows(0)("دائن")
                    DT.Rows.InsertAt(row, 0)
                    GridControl1.DataSource = rDt
                    _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
                End If
            ElseIf TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                rDt = TryCast(GridControl11.DataSource, DataTable)
                _Helper = New MyCellMergeHelper(GVRole11)
                row = rDt.NewRow()

                If DT1.Rows.Count > 0 Then
                    row("الرمز") = "رصيد منقول"
                    row("مدين") = DT1.Rows(0)("مدين")
                    row("دائن") = DT1.Rows(0)("دائن")
                    DT.Rows.InsertAt(row, 0)
                    GridControl11.DataSource = rDt
                    _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")

                End If
            End If
            'GVRole.Columns("#").Width = 70
            'GVRole.Columns("طبيعة الحركة").Width = 500
            'GVRole.Columns("ملاحظات").Width = 500

        ElseIf DT.Rows.Count = 0 Then


            GCRole.DataSource = Nothing
            GridControl1.DataSource = Nothing
            GridControl11.DataSource = Nothing
            GVRole.Columns.Clear()
            IsValIn = 1
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
            PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
            PR(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            PR(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
            Dim DT1 As New DataTable
            DT1.Clear()
            DT1 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTPREVIEWS", PR)
            If DT1.Rows.Count >= 0 Then
                If DT1.Rows(0)("مدين") > 0.000 And DT1.Rows(0)("دائن") > 0.000 Then
                    If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                        GCRole.DataSource = DT1
                    ElseIf TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                        GridControl1.DataSource = DT1

                    ElseIf TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                        GridControl11.DataSource = DT1

                    End If
                    OverAllDebit.EditValue = 0.000
                    OverAllCredit.EditValue = 0.000
                    NEWDVGFROMAT(GVRole)
                    NEWDVGFROMAT(GVRole1)
                    NEWDVGFROMAT(GVRole11)
                End If
            Else
                'GCRole.DataSource = Nothing
                'GVRole.Columns.Clear()
            End If
            Dim PR1(2) As SqlParameter
            PR1(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = CUST.EditValue}
            PR1(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            PR1(2) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}

            Dim DT2 As New DataTable
            DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTNETTOTAL", PR1)

            'If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
            '    OverAllTotal1.EditValue = DT2.Rows(0)("أول")
            '    OverAllTotal1.BackColor = Color.Red
            'Else
            '    OverAllTotal1.EditValue = DT2.Rows(0)("ثاني")
            '    OverAllTotal1.BackColor = Color.Green
            'End If
            If OverAllCredit.EditValue = 0.000 Then
                OverAllCredit.Properties.Appearance.BackColor = Color.Green
            Else
                OverAllCredit.Properties.Appearance.BackColor = Color.Green
            End If
            If OverAllCredit.EditValue = 0.000 Then
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            Else
                OverAllDebit.Properties.Appearance.BackColor = Color.Red
            End If
            OverAllCredit.Properties.Appearance.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Green
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Green

        End If
        SumTotal()
    End Sub

    Sub SumTotal()
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
            If GVRole.RowCount > 0 Then
                Dim CreditSum As New GridColumnSummaryItem()
                CreditSum.SummaryType = SummaryItemType.Sum
                CreditSum.FieldName = "دائن"
                GVRole.Columns("دائن").Summary.Add(CreditSum)
                Dim DebitSum As New GridColumnSummaryItem()
                DebitSum.SummaryType = SummaryItemType.Sum
                DebitSum.FieldName = "مدين"
                GVRole.Columns("مدين").Summary.Add(DebitSum)

                OverAllDebit.EditValue = Convert.ToDouble(GVRole.Columns("مدين").SummaryItem.SummaryValue)

                OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن").SummaryItem.SummaryValue)
                If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Red
                ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Green
                Else
                    OverAllTotal.BackColor = Color.Green
                End If
            End If
        End If

        If TabbedControlGroup1.SelectedTabPageIndex = 1 Then
            If GVRole1.RowCount > 0 Then
                Dim CreditSum As New GridColumnSummaryItem()
                CreditSum.SummaryType = SummaryItemType.Sum
                CreditSum.FieldName = "دائن"
                GVRole1.Columns("دائن").Summary.Add(CreditSum)
                Dim DebitSum As New GridColumnSummaryItem()
                DebitSum.SummaryType = SummaryItemType.Sum
                DebitSum.FieldName = "مدين"
                GVRole1.Columns("مدين").Summary.Add(DebitSum)

                OverAllDebit.EditValue = Convert.ToDouble(GVRole1.Columns("مدين").SummaryItem.SummaryValue)

                OverAllCredit.EditValue = Convert.ToDouble(GVRole1.Columns("دائن").SummaryItem.SummaryValue)
                If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Red
                ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Green
                Else
                    OverAllTotal.BackColor = Color.Green
                End If
            End If
        End If

        If TabbedControlGroup1.SelectedTabPageIndex = 2 Then
            If GVRole11.RowCount > 0 Then
                Dim CreditSum As New GridColumnSummaryItem()
                CreditSum.SummaryType = SummaryItemType.Sum
                CreditSum.FieldName = "دائن"
                GVRole11.Columns("دائن").Summary.Add(CreditSum)
                Dim DebitSum As New GridColumnSummaryItem()
                DebitSum.SummaryType = SummaryItemType.Sum
                DebitSum.FieldName = "مدين"
                GVRole11.Columns("مدين").Summary.Add(DebitSum)

                OverAllDebit.EditValue = Convert.ToDouble(GVRole11.Columns("مدين").SummaryItem.SummaryValue)

                OverAllCredit.EditValue = Convert.ToDouble(GVRole11.Columns("دائن").SummaryItem.SummaryValue)
                If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Red
                ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Green
                Else
                    OverAllTotal.BackColor = Color.Green
                End If
            End If
        End If

    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged, GVRole1.FocusedRowChanged, GVRole11.FocusedRowChanged

        SumTotal()
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader, GVRole1.CustomDrawColumnHeader, GVRole11.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 178, 148), e.Bounds)
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

    Public Sub FrmCustomerMovement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If TypeNewRe = 0 Then
            NEWRECORD()
        End If
        lodePreportes()
        FormLocation(Me)
        'If UserType = 1 Then
        '    BranchID.Enabled = True
        'Else
        '    BranchID.Enabled = False
        'End If


        'Dim currentSkin As Skin = CommonSkins.GetSkin(DevExpress.LookAndFeel.UserLookAndFeel.Default)
        'Dim Color = currentSkin.Colors("DisabledControl")
        'OverAllCredit.Properties.AppearanceDisabled.BackColor = Color
        'OverAllDebit.Properties.AppearanceDisabled.BackColor = Color
        'OverAllTotal1.Properties.AppearanceDisabled.BackColor = Color
    End Sub

    Public Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Try



            LOADDATA()
            If GVRole.RowCount > 0 Then
                GVRole.Columns("مدين").Width = 170
                GVRole.Columns("دائن").Width = 170
                GVRole.Columns("الرصيد").Width = 170
                GVRole.Columns("#").Width = 70
                GVRole.Columns("طبيعة الحركة").Width = 500
                GVRole.Columns("ملاحظات").Width = 500
            End If
            If GVRole1.RowCount > 0 Then
                GVRole1.Columns("مدين").Width = 170
                GVRole1.Columns("دائن").Width = 170
                GVRole1.Columns("الرصيد").Width = 170
                GVRole1.Columns("#").Width = 70
                GVRole1.Columns("طبيعة الحركة").Width = 500
                GVRole1.Columns("ملاحظات").Width = 500
            End If
            If GVRole11.RowCount > 0 Then
                GVRole11.Columns("مدين").Width = 170
                GVRole11.Columns("دائن").Width = 170
                GVRole11.Columns("الرصيد").Width = 170
                GVRole11.Columns("#").Width = 70
                GVRole11.Columns("طبيعة الحركة").Width = 500
                GVRole11.Columns("ملاحظات").Width = 500
            End If
            NEWDVGFROMAT(GVRole)
            NEWDVGFROMAT(GVRole1)
            NEWDVGFROMAT(GVRole11)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CUST.QueryPopUp

        'If BranchID.Text <> String.Empty Then
        '    CUST.Properties.PopulateColumns()
        '    CUST.Properties.Columns("AccID").Visible = False
        'End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged

    End Sub
    Public Sub LOADCUST_WITHBRANCH2()

        'Dim PR(1) As SqlParameter
        'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        'PR(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = UserID}
        'LoadToControlar(CUST, "CustomersTb_LOADTOLKPBasedOnBranchIDAndCanShowHidden", "CustName", "AccID", PR)
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = UserID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CustomersTb_LOADTOLKPBasedOnBranchIDAndCanShowHidden", PR)
        If dt.Rows.Count > 0 Then
            CUST.Properties.DataSource = dt
            CUST.Properties.ValueMember = "AccID"
            CUST.Properties.DisplayMember = "CustName"
            CUST.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub lodetoltat(accode As Integer)
        GridControl21.DataSource = Nothing
        If LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ACCCODE", SqlDbType.Int)
            PRM(0).Value = accode
            Dim dt2 As New DataTable
            dt2.Clear()
            dt2 = RUN_QUARY_PRO("accacounselecttotaldor", PRM)

            If dt2.Rows.Count Then

                'If dt2.Rows(0)("total") > 0 Then
                '    GridView1.Columns("total").AppearanceCell.BackColor = Color.Green
                'Else
                '    GridView1.Columns("total").AppearanceCell.BackColor = Color.Red
                '    dt2.Rows(0)("total") = Math.Abs(dt2.Rows(0)("total"))


            End If
            GridControl21.DataSource = dt2
            dt2.Dispose()
        End If
    End Sub

    Private Sub OverAllCredit_TextChanged(sender As Object, e As EventArgs) Handles OverAllCredit.TextChanged
        'If GVRole.RowCount > 0 Then
        OverAllTotal.EditValue = Val(OverAllCredit.EditValue) - Val(OverAllDebit.EditValue)
        'End If
    End Sub

    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles CUST.EditValueChanged
        'If EMPID.Text = String.Empty Then
        GVRole.Columns.Clear()
        GVRole1.Columns.Clear()
        GVRole11.Columns.Clear()
        GCRole.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl11.DataSource = Nothing
        OverAllTotal.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        CustCode.Text = ""
        lodetoltat(CUST.EditValue)
        CustCode.Text = GetLKPColumnVal(CUST, "AccID")
        'End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GridControl21.DataSource = Nothing
        CUST.Properties.DataSource = Nothing
        CUST.EditValue = -1
        GVRole.Columns.Clear()
        GVRole1.Columns.Clear()
        GVRole11.Columns.Clear()
        GCRole.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl11.DataSource = Nothing
        OverAllTotal.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        LOADCUST_WITHBRANCH2()
    End Sub


    Sub print()
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
        If GVRole.RowCount < 2 And GVRole1.RowCount < 2 And GVRole11.RowCount < 2 And GVRole.ActiveFilterString = Nothing Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim dt As New DataTable

            dt.Clear()

            Dim prm(6) As SqlParameter
            prm(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
            prm(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
            prm(5) = New SqlParameter("@NEt_Totale", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@sumtotal", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            dt = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_GETCUSTMOVEMENT", prm)
            If dt.Rows.Count > 0 Then


                Dim report As New RPTCustomerMovement
                report.DataSource = dt


                If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                    report.FilterString = GVRole.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                    report.FilterString = GVRole1.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                    report.FilterString = GVRole11.ActiveFilterString
                End If
                report.DataMember = "AccSafeActivityTb"
                report.OverAllTotal.Text = prm(5).Value
                report.OverAllTotal1.Text = prm(6).Value
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'SQLCON.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub


    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        print()
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged, GVRole1.ColumnFilterChanged, GVRole11.ColumnFilterChanged
        SumTotal()
    End Sub

    Private Sub TabbedControlGroup1_SelectedPageChanged(sender As Object, e As DevExpress.XtraLayout.LayoutTabPageChangedEventArgs) Handles TabbedControlGroup1.SelectedPageChanged
        GVRole.Columns.Clear()
        GVRole1.Columns.Clear()
        GVRole11.Columns.Clear()
        GCRole.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl11.DataSource = Nothing
        OverAllTotal.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
    End Sub
    Dim report As New RPTCustomerMovement
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}

    Private Sub ارسالكشفعبرالواتسابToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ارسالكشفعبرالواتسابToolStripMenuItem.Click
        Try
            Dim dt As New DataTable

            dt.Clear()

            Dim prm(6) As SqlParameter
            prm(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = CUST.EditValue}
            prm(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
            prm(5) = New SqlParameter("@NEt_Totale", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@sumtotal", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            dt = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_GETCUSTMOVEMENT", prm)


            If dt.Rows.Count > 0 Then


                Dim report As New RPTCustomerMovement
                report.DataSource = dt


                If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                    report.FilterString = GVRole.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                    report.FilterString = GVRole1.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                    report.FilterString = GVRole11.ActiveFilterString
                End If
                report.DataMember = "AccSafeActivityTb"
                report.OverAllTotal.Text = prm(5).Value
                report.OverAllTotal1.Text = prm(6).Value

                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ExportToPdf(pdfExportFile, PdfExportOptions)

                ' إرسال PDF عبر WhatsApp
                SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(CUST.EditValue), pdfExportFile, $"كشف حساب  {CUST.Text} ", " كشف الحساب" & ".pdf")
                'SQLCON.Close()
                MessageBox.Show("تم ارسال الكشف عبر واتساب بنجاح")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try


    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Dim mms As String = "*شركة الرحالة للصرافة*" & Space(1) & "-" & Space(1) & Me.BranchID.Text & vbNewLine &
         "🤵‍♂" & Space(1) & "الاسم " & Space(1) & ":" & Space(1) & CUST.Text & vbNewLine &
          "📱" & Space(1) & "رقم الهاتف" & Space(1) & ":" & Space(1) & GET_PHONE_SaenFroWtsaap(CUST.EditValue) & vbNewLine &
           "🔐" & Space(1) & "كود الحساب" & Space(1) & ":" & Space(1) & CUST.EditValue
        WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(CUST.EditValue), mms, True)
        XtraMessageBox.Show("✅ تم إرسال الرسالة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


End Class