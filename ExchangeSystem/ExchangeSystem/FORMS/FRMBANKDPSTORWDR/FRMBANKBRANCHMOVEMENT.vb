Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports ExchangeSystem.ExchangeSystem
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading
Public Class FRMBANKBRANCHMOVEMENT
    Private _Helper As MyCellMergeHelper
    Dim IsValIn As Boolean
    Public NET, Period, Prievew As Integer
    Sub NEWRECORD()
        LOADBANK()
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRNCHDIERCT(BranchID)
        OverAllTotal.EditValue = 0.000
        GCRole.DataSource = Nothing
        BranchID.EditValue = -1
        BBANKID.EditValue = -1
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        If OverAllCredit.EditValue = 0.000 Then
            OverAllCredit.Properties.Appearance.BackColor = Color.Red
        Else
            OverAllCredit.Properties.Appearance.BackColor = Color.Red
        End If
        If OverAllCredit.EditValue = 0.000 Then
            OverAllDebit.Properties.Appearance.BackColor = Color.Green
        Else
            OverAllDebit.Properties.Appearance.BackColor = Color.Green
        End If
        NEWDVGFROMAT(GVRole)

    End Sub

    Public Sub LOADBANK()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("BanksTb_SelectAll")
        BANKID.Properties.DataSource = DT
        BANKID.Properties.ValueMember = "ID"
        BankID.Properties.DisplayMember = "BankName"
        BankID.Properties.ShowHeader = False
    End Sub
    Sub LOADDATA()

        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        'GVRole.BeginUpdate()
        NEWDVGFROMAT(GVRole)
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If BBANKID.EditValue = -1 Then
            BBANKID.ErrorText = "يجب اختيار اسم المصرف"
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

        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = BBANKID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccSafeActivityTb_GETBBranchMOVEMENT", PRM)


        If DT.Rows.Count = 0 Then
            GCRole.DataSource = Nothing
            GVRole.Columns.Clear()
            IsValIn = 1
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = BBANKID.EditValue}
            PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
            PR(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}
            PR(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = 0}
            Dim DT1 As New DataTable
            DT1.Clear()
            DT1 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTPREVIEWS", PR)


            If DT1.Rows.Count > 0 Then

                GCRole.DataSource = DT1
                OverAllDebit.EditValue = 0.000
                OverAllCredit.EditValue = 0.000
                NEWDVGFROMAT(GVRole)


            End If

            Dim PR1(2) As SqlParameter
            PR1(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = BBANKID.EditValue}
            PR1(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}
            PR1(2) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = 0}

            Dim DT2 As New DataTable
            DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTNETTOTAL", PR1)

            If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                NET = -1
                OverAllTotal1.EditValue = DT2.Rows(0)("أول")
                OverAllTotal1.BackColor = Color.Green
            Else
                NET = 1
                OverAllTotal1.EditValue = DT2.Rows(0)("ثاني")
                OverAllTotal1.BackColor = Color.Red
            End If
            If OverAllCredit.EditValue = 0.000 Then
                OverAllCredit.Properties.Appearance.BackColor = Color.Red
            Else
                OverAllCredit.Properties.Appearance.BackColor = Color.Red
            End If
            If OverAllDebit.EditValue = 0.000 Then
                OverAllDebit.Properties.Appearance.BackColor = Color.Green
            Else
                OverAllDebit.Properties.Appearance.BackColor = Color.Green
            End If
            OverAllCredit.Properties.Appearance.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Red
        Else
            GVRole.Columns.Clear()
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            IsValIn = 0
            GCRole.DataSource = DT
            'GVRole.Columns("id").Visible = False
            If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                Period = 1
                OverAllTotal.BackColor = Color.Green
            Else
                Period = -1
                OverAllTotal.BackColor = Color.Red
            End If
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = BBANKID.EditValue}
            PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(2) = New SqlParameter("@IsValIn", SqlDbType.Bit) With {.Value = IsValIn}
            PR(3) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}
            PR(4) = New SqlParameter("@Type", SqlDbType.TinyInt) With {.Value = 0}
            Dim DT1 As New DataTable
            DT1 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTPREVIEWS", PR)
            Dim rDt As DataTable = TryCast(GCRole.DataSource, DataTable)
            _Helper = New MyCellMergeHelper(GVRole)
            Dim row As DataRow = rDt.NewRow()



            If DT1.Rows.Count > 0 Then
                row("الرمز") = "رصيد منقول"
                row("مدين") = DT1.Rows(0)("مدين")
                row("دائن") = DT1.Rows(0)("دائن")
                DT.Rows.InsertAt(row, 0)
                GCRole.DataSource = rDt
                _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
                If DT1.Rows(0)("مدين") > 0.000 Then
                    Prievew = -1
                Else
                    Prievew = 1
                End If

            End If

            Dim PR1(2) As SqlParameter
            PR1(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = BBANKID.EditValue}
            PR1(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = 1}
            PR1(2) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = 0}
            Dim DT2 As New DataTable
            DT2 = RUN_QUARY_PRO("AccSafeActivityTb_GETCUSTMOVEMENTNETTOTAL", PR1)
            If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                NET = -1
                OverAllTotal1.EditValue = DT2.Rows(0)("أول")
                OverAllTotal1.BackColor = Color.Green
            Else
                NET = 1
                OverAllTotal1.EditValue = DT2.Rows(0)("ثاني")
                OverAllTotal1.BackColor = Color.Red
            End If
            GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Green
            GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Red
            NEWDVGFROMAT(GVRole)
            OverAllCredit.Properties.Appearance.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Red
            If OverAllCredit.EditValue = 0.000 Then
                OverAllCredit.Properties.Appearance.BackColor = Color.Red
            Else
                OverAllCredit.Properties.Appearance.BackColor = Color.Red
            End If
            If OverAllCredit.EditValue = 0.000 Then
                OverAllDebit.Properties.Appearance.BackColor = Color.Green
            Else
                OverAllDebit.Properties.Appearance.BackColor = Color.Green
            End If
        End If
        'GVRole.Columns("#").Width = 70
        'GVRole.Columns("طبيعة الحركة").Width = 500
        'GVRole.Columns("ملاحظات").Width = 500

    End Sub

    Sub SumTotal()
        OverAllDebit.EditValue= 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
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
            OverAllDebit.Properties.Appearance.BackColor = Color.Green

            OverAllCredit.EditValue = Convert.ToDouble(GVRole.Columns("دائن").SummaryItem.SummaryValue)
            OverAllCredit.Properties.Appearance.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceDisabled.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceFocused.BackColor = Color.Red
            OverAllCredit.Properties.AppearanceReadOnly.BackColor = Color.Red

            If OverAllDebit.EditValue > OverAllCredit.EditValue Then
                Period = 1
                OverAllTotal.BackColor = Color.Green
            ElseIf OverAllDebit.EditValue < OverAllCredit.EditValue Then
                Period = -1
                OverAllTotal.BackColor = Color.Red
            End If
        End If
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
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

    Private Sub FRMBANKBRANCHMOVEMENT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)
        NEWRECORD()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LOADDATA()
        SumTotal()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BBANKID.QueryPopUp
        If BANKID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BANKID.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_LOADTOLKPBasedOnBankID", PR)
            If dt.Rows.Count > 0 Then
                BBANKID.Properties.PopulateColumns()
                BBANKID.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub

    Private Sub OverAllCredit_TextChanged(sender As Object, e As EventArgs) Handles OverAllCredit.TextChanged
        If GVRole.RowCount > 0 Then
            OverAllTotal.EditValue = Val(OverAllDebit.EditValue) - (OverAllCredit.EditValue)
        End If
    End Sub

    Private Sub EMPID_EditValueChanged(sender As Object, e As EventArgs) Handles BBANKID.EditValueChanged
        'If EMPID.Text = String.Empty Then
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        'End If
    End Sub

    Private Sub OverAllDebit_TextChanged(sender As Object, e As EventArgs) Handles OverAllDebit.TextChanged
        If GVRole.RowCount > 0 Then
            OverAllTotal.EditValue = Val(OverAllDebit.EditValue) - (OverAllCredit.EditValue)
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", BBANKID.EditValue)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            PRM(3) = New SqlParameter("@CurrencyTo", 1)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_GETBBranchMOVEMENT", PRM)
            dt.TableName = "AccountsTb"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTBANKBRANCHMOVEMENT
                report.DataSource = ds
                report.DataMember = "AccountsTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'Else
                '   XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
    Private Sub BANKID_TextChanged(sender As Object, e As EventArgs) Handles BANKID.TextChanged
        BBANKID.Properties.DataSource = Nothing
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        If BANKID.Text <> String.Empty Then
            LOADBBRANCH_WITHBankID(BANKID, BBANKID)
        End If
    End Sub
End Class