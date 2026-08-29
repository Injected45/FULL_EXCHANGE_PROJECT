Imports DevExpress.CodeParser
Imports DevExpress.Data
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities.IO
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraReports.ReportGeneration
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Runtime.InteropServices.ComTypes

Public Class FRMINCOMESTATMENT
    Public Sub PRINT(tybID As Integer, tybID2 As Integer)
        Try

            Dim pram(4) As SqlParameter
            pram(0) = New SqlParameter("@bracnID", SqlDbType.Int) With {.Value = branchID.EditValue}
            pram(1) = New SqlParameter("@accacount", SqlDbType.Int) With {.Value = tybID}
            pram(2) = New SqlParameter("@accacount2", SqlDbType.Int) With {.Value = tybID2}
            pram(3) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            pram(4) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            'pram(4) = New SqlParameter("@SUMcredet", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            'pram(5) = New SqlParameter("@SUMdebit", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            'pram(6) = New SqlParameter("@ToralPrase", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("dbo.ACCOUNTSTB_FRMINCOMESTATMENT1", pram)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTINCOME
                report.DataSource = dt
                report.DataMember = "AccountsTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            branchID.Enabled = dt.Rows(0)("Can_branch")
            branchID.EditValue = BID
        Else
            branchID.Enabled = False
            branchID.EditValue = BID
        End If
    End Sub
    Public Sub lodadate(tybID As Integer)
        Try
            Sumcredit.EditValue = 0.00
            SUMdibet.EditValue = 0.00
            OverAllTotal.EditValue = 0.00

            Dim pram(6) As SqlParameter
            pram(0) = New SqlParameter("@bracnID", SqlDbType.Int) With {.Value = branchID.EditValue}
            pram(1) = New SqlParameter("@accacount", SqlDbType.Int) With {.Value = tybID}
            pram(2) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            pram(3) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            pram(4) = New SqlParameter("@SUMcredet", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            pram(5) = New SqlParameter("@SUMdebit", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            pram(6) = New SqlParameter("@ToralPrase", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            Dim dt As New DataTable
            dt.Clear()
            If tybID = 4 Then
                GridControl1.DataSource = Nothing
                dt = RUN_QUARY_PRO("dbo.ACCOUNTSTB_FRMINCOMESTATMENT", pram)
                If dt.Rows.Count > 0 Then
                    GridControl1.DataSource = dt

                End If
            ElseIf tybID = 3 Then
                GridControl2.DataSource = Nothing
                dt = RUN_QUARY_PRO("dbo.ACCOUNTSTB_FRMINCOMESTATMENT", pram)
                If dt.Rows.Count > 0 Then
                    GridControl2.DataSource = dt

                End If
            End If
            DVGFROMAT1()
            DVGFROMAT()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Sub DVGFROMAT1()
        GridLocalizer.Active = New MyGridLocalizer()
        GridView1.OptionsBehavior.Editable = False
        GridView1.OptionsBehavior.EditingMode = False
        GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView1.OptionsView.ShowGroupPanel = False
        GridView1.GroupPanelText = ""
        GridView1.OptionsView.ShowFooter = False
        GridView1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GridView1.Columns.Count - 1
            GridView1.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GridView1.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView1.OptionsView.EnableAppearanceEvenRow = True
        GridView1.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView1.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Sub DVGFROMAT()
        GridLocalizer.Active = New MyGridLocalizer()
        GridView2.OptionsBehavior.Editable = False
        GridView2.OptionsBehavior.EditingMode = False
        GridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView2.OptionsView.ShowGroupPanel = False
        GridView2.GroupPanelText = ""
        GridView2.OptionsView.ShowFooter = False
        GridView2.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GridView2.Columns.Count - 1
            GridView2.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView2.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView2.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView2.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GridView2.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView2.OptionsView.EnableAppearanceEvenRow = True
        GridView2.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView2.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        branchID.Properties.DataSource = Nothing
        DT.Clear()
        DT.NewRow()
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("BName", GetType(String))
        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        DT.Rows.Add(0, "كل الفروع")
        If DT.Rows.Count > 0 Then
            branchID.Properties.DataSource = DT
            branchID.Properties.ValueMember = "DBRID"
            branchID.Properties.DisplayMember = "BName"
            branchID.Properties.ShowHeader = False
        End If
    End Sub

    Private Sub FRMINCOMESTATMENT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserType = 1 Or UserType = 3 Then
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
        LOADBRANCH()
        DVGFROMAT()
        DVGFROMAT1()
        branchID.EditValue = BID
        DT1.EditValue = Date.Now
        DT2.EditValue = Date.Now
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If DT1.EditValue > DT2.EditValue Then
            XtraMessageBox.Show("تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        lodadate(3)
        lodadate(4)
        DVGFROMAT()
        DVGFROMAT1()

        Sumtotal()




    End Sub

    Sub Sumtotal()
        Sumcredit.EditValue = 0.000
        SUMdibet.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If GridView1.RowCount > 0 Then
            Dim OverallVal As New GridColumnSummaryItem()
            OverallVal.SummaryType = SummaryItemType.Sum
            OverallVal.FieldName = "tolal"
            GridView1.Columns("tolal").Summary.Add(OverallVal)
            Sumcredit.EditValue = Convert.ToDouble(GridView1.Columns("tolal").SummaryItem.SummaryValue)

        End If
        If GridView2.RowCount > 0 Then
            Dim ExVal As New GridColumnSummaryItem()
            ExVal.SummaryType = SummaryItemType.Sum
            ExVal.FieldName = "tolal2"
            GridView2.Columns("tolal2").Summary.Add(ExVal)

            SUMdibet.EditValue = Convert.ToDouble(GridView2.Columns("tolal2").SummaryItem.SummaryValue)

        End If
        OverAllTotal.EditValue = Sumcredit.EditValue - SUMdibet.EditValue
    End Sub


    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView2.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub branchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles branchID.QueryPopUp
        branchID.Properties.PopulateColumns()
        branchID.Properties.Columns("DBRID").Visible = False
        'branchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private dataSet1 As DataSet
    Private dataTable1 As DataTable
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'PRINTINCOME.ShowDialog()
        If GridView1.RowCount > 0 Or GridView2.RowCount > 0 Then
            Dim report As New RPTINCOME
            Dim pram(6) As SqlParameter
            pram(0) = New SqlParameter("@bracnID", SqlDbType.Int) With {.Value = branchID.EditValue}
            pram(1) = New SqlParameter("@accacount", SqlDbType.Int) With {.Value = 3}
            pram(2) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            pram(3) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            pram(4) = New SqlParameter("@SUMcredet", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            pram(5) = New SqlParameter("@SUMdebit", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            pram(6) = New SqlParameter("@ToralPrase", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("dbo.ACCOUNTSTB_FRMINCOMESTATMENT", pram)
            If dt.Rows.Count > 0 Then
                report.DetailReport1.DataSource = dt
                report.DataMember = "AccountsTb"
            End If
            Dim PR(6) As SqlParameter
            PR(0) = New SqlParameter("@bracnID", SqlDbType.Int) With {.Value = branchID.EditValue}
            PR(1) = New SqlParameter("@accacount", SqlDbType.Int) With {.Value = 4}
            PR(2) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            PR(3) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            PR(4) = New SqlParameter("@SUMcredet", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            PR(5) = New SqlParameter("@SUMdebit", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            PR(6) = New SqlParameter("@ToralPrase", SqlDbType.Decimal) With {.Direction = ParameterDirection.Output}
            Dim dt21 As New DataTable
            dt21.Clear()
            dt21 = RUN_QUARY_PRO("dbo.ACCOUNTSTB_FRMINCOMESTATMENT", PR)
            If dt21.Rows.Count > 0 Then
                report.DetailReport.DataSource = dt21
                report.DataMember = "AccountsTb"
            End If
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        Else
            ErrorMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها")
        End If
    End Sub

    Private Sub GridView2_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView2.ColumnFilterChanged, GridView1.ColumnFilterChanged, GridView2.FocusedRowChanged, GridView1.FocusedRowChanged
        Sumtotal()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Try
            If DT1.EditValue > DT2.EditValue Then
                XtraMessageBox.Show("تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If branchID.EditValue <= 0 Then
                XtraMessageBox.Show("يجب تحديد الفرع", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            If GridView1.RowCount <= 0 And GridView2.RowCount <= 0 Then
                XtraMessageBox.Show("يرجى عرض البيانات لتتم عملية الإقفال", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            Dim reslut = XtraMessageBox.Show("هل ترغب حقا في إقفال قائمة الدخل للفترة من" & vbNewLine & Format(DT1.EditValue, "yyyy-MM-dd") & vbNewLine & "إلى" & vbNewLine & Format(DT2.EditValue, "yyyy-MM-dd"), "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If reslut = DialogResult.No Then
                Exit Sub
            End If
            Dim reslut1 = XtraMessageBox.Show("في حال الحفظ لايمكنك الرجوع عن العملية هل أنت واثق من الاستمرار؟", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If reslut1 = DialogResult.No Then
                Exit Sub
            End If
            Dim PR(5) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchID.EditValue}
            PR(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
            PR(2) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            PR(3) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            PR(4) = New SqlParameter("@MsgSatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PR(5) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("ACCOUNTSTB_ClosingINCOMESTATMENT", PR)
            If PR(4).Value = 0 Then
                ErrorMessage(Me, "رسالة تنبيه", PR(5).Value)
                Exit Sub
            End If
            CONFIRMMESSAGE.Show()
            Sumcredit.EditValue = 0.00
            SUMdibet.EditValue = 0.00
            OverAllTotal.EditValue = 0.00
            GridControl1.DataSource = Nothing
            GridControl2.DataSource = Nothing
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub

    Private Sub branchID_EditValueChanged(sender As Object, e As EventArgs) Handles branchID.EditValueChanged
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
    End Sub

    Private Sub DT1_EditValueChanged(sender As Object, e As EventArgs) Handles DT1.EditValueChanged
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
    End Sub

    Private Sub DT2_EditValueChanged(sender As Object, e As EventArgs) Handles DT2.EditValueChanged
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
    End Sub

    Private Sub DT1_EditValueChanging(sender As Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles DT1.EditValueChanging
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
    End Sub

    Private Sub DT2_EditValueChanging(sender As Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles DT2.EditValueChanging
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
    End Sub
End Class