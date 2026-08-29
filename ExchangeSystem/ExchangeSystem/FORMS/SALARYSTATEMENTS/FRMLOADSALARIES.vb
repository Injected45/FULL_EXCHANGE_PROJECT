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

Public Class FRMLOADSALARIES
    Private _Helper As MyCellMergeHelper
    Dim IsValIn As Boolean
    Public OvarAllPrint As Double

    Sub NEWRECORD()
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        NEWDVGFROMAT(GVRole)
        LOADBRNCHHasEmp(BranchID)
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        GCRole.DataSource = Nothing
        GridView1.Columns.Clear()
        GridControl1.DataSource = Nothing
        GridView11.Columns.Clear()
        GridControl11.DataSource = Nothing
        'BranchID.EditValue = -1
        EMPID.EditValue = -1
        D1.DateTime = Date.Now
        D2.DateTime = Date.Now
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        LOADCIDFROMT()
        TabbedControlGroup1.SelectedTabPageIndex = 0
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
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
    'Sub LOADBRANCH()
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
    '    If DT.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = DT
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.PopulateColumns()
    '        BranchID.Properties.ShowHeader = False
    '    End If
    'End Sub
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
            GridView2.Columns("ID").Visible = False
        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADEMP(bid As Integer)
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        'If BranchID.Text <> String.Empty Then
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = bid
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONACCIDISACTIVE", PR)

        If DT.Rows.Count > 0 Then
            EMPID.Properties.DataSource = DT
            EMPID.Properties.ValueMember = "AccID"
            EMPID.Properties.DisplayMember = "EMPNAME"
            'EMPID.Properties.PopulateColumns()
            EMPID.Properties.ShowHeader = False

        Else
            EMPID.EditValue = -1
            EMPID.Properties.DataSource = Nothing
        End If
        'End If
    End Sub
    Sub LOADDATA()
        Try



            OverAllDebit.BackColor = Color.Red
            OverAllCredit.BackColor = Color.Green
            GVRole.Columns.Clear()
            GCRole.DataSource = Nothing
            GridControl1.DataSource = Nothing
            GridControl11.DataSource = Nothing
            'GVRole.BeginUpdate()
            NEWDVGFROMAT(GVRole)
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يجب اختيار الفرع"
                Return
            End If
            If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
                EMPID.ErrorText = "يجب اختيار الموظف"
                Exit Sub
            End If
            If D1.EditValue Is Nothing Then
                D1.ErrorText = "يجب اختيار التاريخ أولاً"
                Return
            End If
            If D1.EditValue > D2.EditValue Then
                D1.ErrorText = "تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية"
                Return
            End If

            If CurrencyTo.Text = String.Empty Then
                CurrencyTo.ErrorText = "هذا الحقل مطلوب "
                Return
            End If

            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = EMPID.EditValue}
            PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            PRM(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            PRM(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AccSafeActivityTb_GETEMPMOVEMENT", PRM)
            If DT.Rows.Count = 0 Then
                GCRole.DataSource = Nothing
                GridControl1.DataSource = Nothing
                GridControl11.DataSource = Nothing
                GVRole.Columns.Clear()
                IsValIn = 1
                Dim PR(4) As SqlParameter
                PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = EMPID.EditValue}
                PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                PR(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
                PR(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
                PR(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
                'PRM(3) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                Dim DT1 As New DataTable
                DT1.Clear()
                DT1 = RUN_QUARY_PRO("AccSafeActivityTb_GETEMPMOVEMENTPREVIEWS", PR)
                If DT1.Rows.Count > 0 Then
                    If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                        GCRole.DataSource = DT1
                        OverAllDebit.EditValue = 0.000
                        OverAllCredit.EditValue = 0.000
                        NEWDVGFROMAT(GVRole)
                    ElseIf TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                        GridControl1.DataSource = DT1
                        OverAllDebit.EditValue = 0.000
                        OverAllCredit.EditValue = 0.000
                        NEWDVGFROMAT(GridView1)
                    ElseIf TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                        GridControl11.DataSource = DT1
                        OverAllDebit.EditValue = 0.000
                        OverAllCredit.EditValue = 0.000
                        NEWDVGFROMAT(GridView11)
                    End If


                End If
                Dim PR1(2) As SqlParameter
                PR1(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = EMPID.EditValue}
                PR1(1) = New SqlParameter("@CurrencyTo", SqlDbType.BigInt) With {.Value = CurrencyTo.EditValue}
                PR1(2) = New SqlParameter("@Type", SqlDbType.BigInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}

                Dim DT2 As New DataTable
                DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETEMPMOVEMENTNETTOTAL", PR1)
                If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                    OverAllTotal1.EditValue = DT2.Rows(0)("أول")
                    OverAllTotal1.BackColor = Color.Red
                    OvarAllPrint = DT2.Rows(0)("أول") * -1
                Else
                    OverAllTotal1.EditValue = DT2.Rows(0)("ثاني")
                    OverAllTotal1.BackColor = Color.Green
                    OvarAllPrint = DT2.Rows(0)("ثاني")
                End If
            Else
                GVRole.Columns.Clear()
                GridView1.Columns.Clear()
                GridView11.Columns.Clear()
                IsValIn = 0
                If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                    GCRole.DataSource = DT
                ElseIf TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                    GridControl1.DataSource = DT

                ElseIf TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                    GridControl11.DataSource = DT

                End If

                NETTOTAL()

                If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                    OverAllTotal.BackColor = Color.Red
                Else
                    OverAllTotal.BackColor = Color.Green
                End If
                Dim PR(4) As SqlParameter
                PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = EMPID.EditValue}
                PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                PR(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
                PR(3) = New SqlParameter("@CurrencyTo", SqlDbType.BigInt) With {.Value = CurrencyTo.EditValue}
                PR(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
                Dim DT1 As New DataTable
                DT1 = RUN_QUARY_PRO("AccSafeActivityTb_GETEMPMOVEMENTPREVIEWS", PR)
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
                    _Helper = New MyCellMergeHelper(GridView1)
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
                    _Helper = New MyCellMergeHelper(GridView11)
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
                Dim PR1(2) As SqlParameter
                PR1(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = EMPID.EditValue}
                PR1(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
                PR1(2) = New SqlParameter("@Type", SqlDbType.BigInt) With {.Value = TabbedControlGroup1.SelectedTabPageIndex}
                Dim DT2 As New DataTable
                DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETEMPMOVEMENTNETTOTAL", PR1)
                If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                    OverAllTotal1.EditValue = DT2.Rows(0)("أول")
                    OverAllTotal1.BackColor = Color.Red
                    OvarAllPrint = DT2.Rows(0)("أول") * -1
                Else
                    OverAllTotal1.EditValue = DT2.Rows(0)("ثاني")
                    OverAllTotal1.BackColor = Color.Green
                    OvarAllPrint = DT2.Rows(0)("ثاني")
                End If
                'GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
                'GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Green
                NEWDVGFROMAT(GVRole)
                NEWDVGFROMAT(GridView1)
                NEWDVGFROMAT(GridView11)

            End If
            'GVRole.Columns("#").Width = 70
            'GVRole.Columns("طبيعة الحركة").Width = 250
            'GVRole.Columns("ملاحظات").Width = 250
        Catch ex As Exception

            MessageBox.Show(ex.Message, "رسالــــــــــة تنبيــــــــــــة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader, GridView1.CustomDrawColumnHeader, GridView11.CustomDrawColumnHeader
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
    Sub SUMTotals()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
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
            If GridView1.RowCount > 0 Then
                Dim CreditSum As New GridColumnSummaryItem()
                CreditSum.SummaryType = SummaryItemType.Sum
                CreditSum.FieldName = "دائن"
                GridView1.Columns("دائن").Summary.Add(CreditSum)
                Dim DebitSum As New GridColumnSummaryItem()
                DebitSum.SummaryType = SummaryItemType.Sum
                DebitSum.FieldName = "مدين"
                GridView1.Columns("مدين").Summary.Add(DebitSum)

                OverAllDebit.EditValue = Convert.ToDouble(GridView1.Columns("مدين").SummaryItem.SummaryValue)

                OverAllCredit.EditValue = Convert.ToDouble(GridView1.Columns("دائن").SummaryItem.SummaryValue)
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
            If GridView11.RowCount > 0 Then
                Dim CreditSum As New GridColumnSummaryItem()
                CreditSum.SummaryType = SummaryItemType.Sum
                CreditSum.FieldName = "دائن"
                GridView11.Columns("دائن").Summary.Add(CreditSum)
                Dim DebitSum As New GridColumnSummaryItem()
                DebitSum.SummaryType = SummaryItemType.Sum
                DebitSum.FieldName = "مدين"
                GridView11.Columns("مدين").Summary.Add(DebitSum)

                OverAllDebit.EditValue = Convert.ToDouble(GridView11.Columns("مدين").SummaryItem.SummaryValue)

                OverAllCredit.EditValue = Convert.ToDouble(GridView11.Columns("دائن").SummaryItem.SummaryValue)
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
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SUMTotals()
        NETTOTAL()
    End Sub
    Private Sub FRMLOADSALARIES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)


    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        LOADDATA()
        SUMTotals()
        NETTOTAL()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        'BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        'If BranchID.Text <> String.Empty Then
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BranchID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONACCIDISACTIVE", PR)
        If DT.Rows.Count > 0 Then
            EMPID.Properties.PopulateColumns()
            EMPID.Properties.Columns("AccID").Visible = False
        End If
        'End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Then
            LOADEMP(BranchID.EditValue)
        End If
    End Sub

    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles EMPID.EditValueChanged
        'If EMPID.Text = String.Empty Then
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        GridView1.Columns.Clear()
        GridControl1.DataSource = Nothing
        GridView11.Columns.Clear()
        GridControl11.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        'End If
        'lodetoltat(EMPID.EditValue)
    End Sub
    Public Sub lodetoltat(accode As Integer)
        GridControl21.DataSource = Nothing
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


            GridControl21.DataSource = dt2
            dt2.Dispose()
        End If

    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        GridView1.Columns.Clear()
        GridControl1.DataSource = Nothing
        GridView11.Columns.Clear()
        GridControl11.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
    End Sub
    Sub NETTOTAL()
        OverAllTotal.EditValue = 0.000
        OverAllTotal.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
    End Sub
    Private Sub OverAllDebit_TextChanged(sender As Object, e As EventArgs) Handles OverAllDebit.TextChanged
        'If GVRole.RowCount > 0 Then
        '    OverAllTotal.Text = Val(OverAllCredit.Text) - Val(OverAllDebit.Text)
        'End If
    End Sub


    Sub Print()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True

        If D1.EditValue > D2.EditValue Then
            XtraMessageBox.Show(lookFeelError, "يجب أن تختار تاريخ بداية مختلف عن تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If GVRole.RowCount < 2 And GridView1.RowCount < 2 And GridView11.RowCount < 2 And GVRole.ActiveFilterString = Nothing Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try
            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", EMPID.EditValue)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            PRM(3) = New SqlParameter("@CurrencyTo", CurrencyTo.EditValue)
            PRM(4) = New SqlParameter("@Type", TabbedControlGroup1.SelectedTabPageIndex)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_GETEMPMOVEMENT", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "AccSafeActivityTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTLOADSALARIES
                report.DataSource = ds
                If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                    report.FilterString = GVRole.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                    report.FilterString = GridView1.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                    report.FilterString = GridView11.ActiveFilterString
                End If
                report.DataMember = "AccSafeActivityTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Print()
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        'LOADDATA()
        SUMTotals()
        NETTOTAL()
    End Sub

    Private Sub TabbedControlGroup1_SelectedPageChanged(sender As Object, e As DevExpress.XtraLayout.LayoutTabPageChangedEventArgs) Handles TabbedControlGroup1.SelectedPageChanged
        GVRole.Columns.Clear()
        GridView1.Columns.Clear()
        GridView11.Columns.Clear()
        GCRole.DataSource = Nothing
        GridControl1.DataSource = Nothing
        GridControl11.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
    End Sub
    Private Rpt As New XtraReport() With {.Name = "Rpt_ACCFINAL_Account"}
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & Rpt.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs)
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True

        If D1.EditValue > D2.EditValue Then
            XtraMessageBox.Show(lookFeelError, "يجب أن تختار تاريخ بداية مختلف عن تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If GVRole.RowCount < 2 And GridView1.RowCount < 2 And GridView11.RowCount < 2 And GVRole.ActiveFilterString = Nothing Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", EMPID.EditValue)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            PRM(3) = New SqlParameter("@CurrencyTo", CurrencyTo.EditValue)
            PRM(4) = New SqlParameter("@Type", TabbedControlGroup1.SelectedTabPageIndex)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_GETEMPMOVEMENT", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "AccSafeActivityTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTLOADSALARIES
                report.DataSource = ds
                If TabbedControlGroup1.SelectedTabPageIndex = 0 Then
                    report.FilterString = GVRole.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 1 Then
                    report.FilterString = GridView1.ActiveFilterString
                End If
                If TabbedControlGroup1.SelectedTabPageIndex = 2 Then
                    report.FilterString = GridView11.ActiveFilterString
                End If
                report.DataMember = "AccSafeActivityTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                Try
                    report.ExportToPdf(pdfExportFile, PdfExportOptions)

                    ' إرسال PDF عبر WhatsApp
                    SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(EMPID.EditValue), pdfExportFile, " كشف حساب موظف", " كشف الحساب" & ".pdf")
                Catch ex As Exception

                End Try

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Try

            Dim EMID As Integer
            EMID = GetLKPColumnVal(EMPID, "ID")
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@EmpID", SqlDbType.Int) With {.Value = EMID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("SalaryCalculationTb_MoneyCard", prm)


            If dt.Rows.Count Then
                Dim report As New RPTMoneyCard
                report.DataSource = dt
                report.DataMember = "EmployeeTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
End Class