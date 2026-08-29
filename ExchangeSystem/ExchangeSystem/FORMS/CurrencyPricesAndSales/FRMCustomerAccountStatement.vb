Imports DevExpress
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient
Public Class FRMCustomerAccountStatement
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
            DVGFormat(GridLookUpEdit1View)
        Else
            CurrencyTo.Properties.DataSource = Nothing
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
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit2")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        DVGFormat(GridView2)

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
    Sub DVGFormat2(GridView11 As GridView)

        GridView11.OptionsBehavior.EditingMode = True
        GridView11.OptionsBehavior.ReadOnly = True
        GridView11.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True

        GridView11.OptionsFind.AlwaysVisible = True
        'GridView11.ShowFindPanel()

        GridView11.OptionsView.ShowFooter = False
        For i As Integer = 0 To GridView11.Columns.Count - 1
            GridView11.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView11.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView11.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView11.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView11.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GridView11.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        GridView11.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView11.OptionsView.EnableAppearanceEvenRow = True
        GridView11.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView11.OptionsView.EnableAppearanceOddRow = True
        GridView11.Columns("SN").Width = 75
        ' GridLocalizer.Active = New MyGridLocalizer()
        GridView11.OptionsView.ShowGroupPanel = False
    End Sub

    Public Sub NOWRecored()
        LOADCIDFROMT()
        CurrencyTo.EditValue = -1
        LOADBRANCH()
        BranchID.EditValue = -1
        DVGFormat2(GridView1)
        TypeMov.SelectedIndex = -1
        GridControl1.DataSource = Nothing

        GridView1.OptionsView.ShowGroupPanel = False
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub

    Private Sub FRMCustomerAccountStatement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)
        If UserType = 1 Then
            BranchID.Enabled = True

        Else
            BranchID.Enabled = False


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

    Public Sub CustomerAccountStatement()
        Try
            GridControl1.DataSource = Nothing
            OverAllDebit.EditValue = 0.000
            OverAllCredit.EditValue = 0.000
            OverAllTotal1.EditValue = 0.000
            Dim dt As New DataTable
            Dim prm(6) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(1) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            prm(2) = New SqlParameter("@TypeMov", SqlDbType.Int) With {.Value = TypeMov.SelectedIndex}
            prm(3) = New SqlParameter("@OverAllDebit", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@OverAllCredit", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@OverAllTotal1", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = UserID}
            dt.Clear()
            dt = RUN_QUARY_PRO("CustomerAccountStatement", prm)
            GridView1.Columns("SN").Width = 75
            If TypeMov.SelectedIndex <> 2 Then
                OverAllDebit.Text = prm(3).Value
                OverAllCredit.Text = prm(4).Value
                OverAllTotal1.Text = prm(5).Value
            End If


            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
                dt.Dispose()
                DVGFormat2(GridView1)

            Else
                MessageBox.Show("عذرا لايوجد بيانات في الوقت الحالي", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
           



        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub



    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        CustomerAccountStatement()
        SumTotals()
    End Sub

    Private Sub GridView1_CustomUnboundColumnData_1(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle

        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName = "ACCNETtotel" Then
            Dim _length As String = CStr(e.CellValue)
            If _length <= 0 Then
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Red
            Else
                e.Appearance.ForeColor = Color.Yellow
                e.Appearance.BackColor = Color.Green
            End If

        End If

    End Sub


    Public Sub GET_Total_Currency_CustomersTb_PROC()
        Try


            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            If TypeMov.SelectedIndex = -1 Then
                TypeMov.ErrorText = "هذا الحقل مطلوب"
                Return
            End If

            GridControl21.DataSource = Nothing
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = TypeMov.SelectedIndex}
            prm(1) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("GET_Total_Currency_CustomersTb_PROC", prm)

            If dt.Rows.Count > 0 Then
                GridControl21.DataSource = dt

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GET_Total_Currency_CustomersTb_PROC()
    End Sub

    Private Sub TypeMov_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeMov.SelectedIndexChanged
        OverAllTotal1.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        GET_Total_Currency_CustomersTb_PROC()
        If TypeMov.SelectedIndex = 0 Then
            Me.Text = "كشف حسابات العملاء"
        End If
        If TypeMov.SelectedIndex = 1 Then
            Me.Text = "كشف حسابات الموظفين"
        End If
        If TypeMov.SelectedIndex = 2 Then
            Me.Text = "كشف حسابات العملاء و الموظفين"
        End If
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        If GridView1.RowCount > 0 Then
            FrmCustomerMovement.TypeNewRe = 1
            If GridView1.GetFocusedRowCellValue("TypeMov") = 0 Then


                FrmCustomerMovement.NEWRECORD()
                FrmCustomerMovement.BranchID.EditValue = BranchID.EditValue
                FrmCustomerMovement.LOADCUST_WITHBRANCH2()
                FrmCustomerMovement.CurrencyTo.EditValue = CurrencyTo.EditValue
                FrmCustomerMovement.CUST.EditValue = GridView1.GetFocusedRowCellValue("AccID")
                FrmCustomerMovement.CustCode.Text = GridView1.GetFocusedRowCellValue("AccID").ToString
                FrmCustomerMovement.ShowDialog()

            ElseIf GridView1.GetFocusedRowCellValue("TypeMov") = 1 Then


                FRMLOADSALARIES.NEWRECORD()

                FRMLOADSALARIES.BranchID.EditValue = BranchID.EditValue
                FRMLOADSALARIES.LOADEMP(BranchID.EditValue)
                FRMLOADSALARIES.EMPID.EditValue = GridView1.GetFocusedRowCellValue("AccID")
                FRMLOADSALARIES.CurrencyTo.EditValue = CurrencyTo.EditValue
                FRMLOADSALARIES.ShowDialog()

            End If
        End If

    End Sub

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click

        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GridView1.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim PRM(5) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", BranchID.EditValue)
            PRM(1) = New SqlParameter("@CurrencyTo", CurrencyTo.EditValue)
            PRM(2) = New SqlParameter("@TypeMov", TypeMov.SelectedIndex)
            PRM(3) = New SqlParameter("@OverAllDebit", ParameterDirection.Output)
            PRM(4) = New SqlParameter("@OverAllCredit", ParameterDirection.Output)
            PRM(5) = New SqlParameter("@OverAllTotal1", ParameterDirection.Output)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CustomerAccountStatement", PRM)
            Dim ds As New DataSet
            dt.TableName = "CustomersTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then

                        Dim report As New RPTCustomerAccountStatement
                        report.DataSource = ds
                        report.DataMember = "CustomersTb"
                        Dim tool As ReportPrintTool = New ReportPrintTool(report)
                        report.CreateDocument()
                        report.ShowPreview()
                        'SQLCON.Close()
                    Else
                        XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub
    Sub SumTotals()
        OverAllDebit.EditValue = 0.000
        OverAllCredit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
        If GridView1.RowCount > 0 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "Credit"
            GridView1.Columns("Credit").Summary.Add(CreditSum)
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "Debit"
            GridView1.Columns("Debit").Summary.Add(DebitSum)

            OverAllDebit.EditValue = GridView1.Columns("Debit").SummaryItem.SummaryValue
            OverAllDebit.Properties.Appearance.BackColor = Color.Red

            OverAllCredit.EditValue = GridView1.Columns("Credit").SummaryItem.SummaryValue

            OverAllTotal1.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
        End If
    End Sub
    Private Sub GridView1_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        SumTotals()
    End Sub

    Private Sub FRMCustomerAccountStatement_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        TypeMov.SelectedIndex = -1
    End Sub

    Private Sub GridView1_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView1.ColumnFilterChanged
        SumTotals()
    End Sub
End Class