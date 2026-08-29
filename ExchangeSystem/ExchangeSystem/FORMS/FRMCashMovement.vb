Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRMCashMovement

    Public Sub LoadData()
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If MovmentType.SelectedIndex = -1 Or MovmentType.Text = String.Empty Then
            MovmentType.ErrorText = "هذا الحقل مطلوب"
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

        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        Dim PR(4) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@accTIPE", SqlDbType.Int) With {.Value = MovmentType.SelectedIndex}
        PR(2) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        PR(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CashMonment_LOADTODVG", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            NEWDVGFROMAT(GVRole)
            GVRole.Columns("#").Width = 60
            GVRole.Columns("الرمز").Width = 120
            GVRole.Columns("التاريخ").Width = 120
            GVRole.Columns("القيمة").Width = 150
        End If


    End Sub

    Sub LOADBRNACH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
        DT.Rows.Add(0, "كل الفروع")
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False

    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 153, 153), e.Bounds)
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

    Private Sub FRMCashMovement_Load(sender As Object, e As EventArgs) Handles Me.Load
        LOADBRNACH()

        BranchID.EditValue = BID
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        OverallVal.EditValue = 0.000
        If UserType = 1 Then
            BranchID.Enabled = True

        Else
            BranchID.Enabled = False


        End If

    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
        SumTotal()
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged

        GCRole.DataSource = Nothing
        OverallVal.EditValue = 0.000
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then


            CURRENCYTB_LoadWithBranch_forbr(BranchID, CurrencyID)
            CurrencyID.Text = "دينار ليبي"

        End If
    End Sub

    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("CurrID").Visible = False
    End Sub

    Private Sub MovmentType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MovmentType.SelectedIndexChanged
        OverallVal.EditValue = 0.000
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()

        If MovmentType.SelectedIndex = 1 Then
            OverallVal.BackColor = Color.Green
        Else
            OverallVal.BackColor = Color.Red
        End If
        If MovmentType.SelectedIndex = 2 Then
            CurrencyID.Text = "دينار ليبي"
            CurrencyID.Enabled = False
        Else
            CurrencyID.Enabled = True
        End If
    End Sub
    Sub SumTotal()
        OverallVal.EditValue = 0.000
        If GVRole.RowCount > 0 Then


            Dim TotalLB As New GridColumnSummaryItem()
            TotalLB.SummaryType = SummaryItemType.Sum
            TotalLB.FieldName = "القيمة"
            GVRole.Columns("القيمة").Summary.Add(TotalLB)

            OverallVal.EditValue = Convert.ToDouble(GVRole.Columns("القيمة").SummaryItem.SummaryValue)
        End If
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SumTotal()
    End Sub

    Private Sub CurrencyID_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyID.EditValueChanged
        OverallVal.EditValue = 0.000
        GCRole.DataSource = Nothing
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim PR(4) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@accTIPE", SqlDbType.Int) With {.Value = MovmentType.SelectedIndex}
        PR(2) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        PR(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        If MovmentType.SelectedIndex = 3 Then
            LoadToPrint("CashMonment_LOADTODVG", "EMPORCUSTWITHDRAWALTB", "RPTCashMovement2", PR, GVRole.ActiveFilterString)
        Else
            LoadToPrint("ZRPT_CashMonment_LOADTODVG", "EMPORCUSTWITHDRAWALTB", "RPTCashMovement", PR, GVRole.ActiveFilterString)
        End If

        'Using cmd1 As SqlCommand = New SqlCommand("ZRPT_CashMonment_LOADTODVG")
        '    cmd1.CommandType = CommandType.StoredProcedure
        '    cmd1.Parameters.AddWithValue("@BranchID", BranchID.EditValue)
        '    cmd1.Parameters.AddWithValue("@CurrencyID", CurrencyID.EditValue)
        '    cmd1.Parameters.AddWithValue("@accTIPE", MovmentType.SelectedIndex)
        '    cmd1.Parameters.AddWithValue("@D1", D1.EditValue)
        '    cmd1.Parameters.AddWithValue("@D2", D2.EditValue)

        '    cmd1.Connection = SQLCON
        '    Dim DA As New SqlDataAdapter(cmd1)
        '    Dim ds As New DataSet
        '    DA.Fill(ds)
        '    Using dr1 As SqlDataReader = cmd1.ExecuteReader()
        '        dr1.Read()
        '        If dr1.HasRows Then
        '            Dim report As New RPTCashMovement
        '            report.DataSource = ds
        '            report.DataAdapter = DA
        '            report.DataMember = "EMPORCUSTWITHDRAWALTB"
        '            report.FilterString = GVRole.ActiveFilterString
        '            Dim tool As ReportPrintTool = New ReportPrintTool(report)

        '            report.CreateDocument()
        '            report.ShowPreview()
        '        Else
        '            XtraMessageBox.Show("لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        End If
        '    End Using
        'End Using

    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
End Class