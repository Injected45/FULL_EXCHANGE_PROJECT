Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports ExchangeSystem.ExchangeSystem
Imports System.Data.SqlClient
Imports System.Threading
Public Class FRMaccounDEtells
    Public IsUpdate, ExistVal As Boolean
    Private _Helper As MyCellMergeHelper
    Sub DVGFROMAT(GVRole As GridView)
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
        GVRole.Columns("insertdate").Width = 60
        GVRole.Columns("safID").Width = 60
        GVRole.Columns("BName").Width = 150
        DVGFROMAT2()
    End Sub


    Sub DVGFROMAT2()
        GridLocalizer.Active = New MyGridLocalizer()
        GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView1.OptionsView.ShowGroupPanel = False
        GridView1.GroupPanelText = ""
        GridView1.OptionsView.ShowFooter = False
        GridView1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        For i As Integer = 0 To GridView1.Columns.Count - 1
            GridView1.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
    End Sub

    Private Sub FRMaccounDEtells_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        DVGFROMAT(GVRole)
    End Sub
    Public Sub loaddate(ACCCODES As ULong, CurrencyFrom As Integer)
        Try
            sumlabeldebit.Text = 0.00
            smblabecredetl.Text = 0.00
            sumleabel.Text = 0.00
            lodetoltat(ACCCODES)
            GridControl1.DataSource = Nothing
            Dim PRM(8) As SqlParameter
            PRM(0) = New SqlParameter("@ACCCODE", SqlDbType.Int) With {.Value = ACCCODES}
            PRM(1) = New SqlParameter("@date1", SqlDbType.Date) With {.Value = DT1.EditValue}
            PRM(2) = New SqlParameter("@date2", SqlDbType.Date) With {.Value = dt2.EditValue}
            PRM(3) = New SqlParameter("@smblabecredetl", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            PRM(4) = New SqlParameter("@sumlabeldebit", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            PRM(5) = New SqlParameter("@lbealtotal", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            PRM(6) = New SqlParameter("@totalacount", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
            PRM(7) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom}
            PRM(8) = New SqlParameter("@BranchID", SqlDbType.NChar) With {.Value = FRMselectACountes.BranchIDd.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[ACCOUNTSTB_SelectICOUNTDetelles2]", PRM)
            If dt.Rows.Count > 0 Then
                ExistVal = 1
                ACCCODE.EditValue = dt.Rows(0)("AccCode")
                ACCNAME.Text = dt.Rows(0)("AccName")
                GridControl1.DataSource = dt
                DVGFROMAT(GVRole)
                sumlabeldebit.Text = PRM(4).Value
                smblabecredetl.Text = PRM(3).Value
                If PRM(5).Value > 0 Then
                    sumleabel.AppearanceItemCaption.BackColor = Color.Green
                Else
                    sumleabel.AppearanceItemCaption.BackColor = Color.Red
                End If
                Dim valuen As Double
                valuen = PRM(5).Value
                sumleabel.Text = Math.Abs(valuen)
                Dim dtt1 As New DataTable
                dtt1.Clear()
                Dim prm2(3) As SqlParameter
                prm2(0) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = ACCCODES}
                prm2(1) = New SqlParameter("@DT1", SqlDbType.Date) With {.Value = DT1.EditValue}
                prm2(2) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom}
                prm2(3) = New SqlParameter("@ExistVal", SqlDbType.Bit) With {.Value = ExistVal}
                dtt1 = RUN_QUARY_PRO("AccSafeActivityTbPOC_TOTETDATE", prm2)
                Dim dtr1 As DataTable = TryCast(GridControl1.DataSource, DataTable)
                _Helper = New MyCellMergeHelper(GVRole)
                Dim row As DataRow = dtr1.NewRow()
                If dtt1.Rows.Count > 0 Then
                    row("Code") = "رصيد سابق"
                    row("debit") = dtt1.Rows(0)("debit")
                    row("CREDET") = dtt1.Rows(0)("CREDET")
                    row("totel") = dtt1.Rows(0)("totel")
                    dt.Rows.InsertAt(row, 0)
                    GridControl1.DataSource = dtr1
                    _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
                End If
                dt.Dispose()
            ElseIf dt.Rows.Count = 0 Then
                ExistVal = 0
                Dim dtt1 As New DataTable
                dtt1.Clear()
                Dim prm2(3) As SqlParameter
                prm2(0) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = ACCCODES}
                prm2(1) = New SqlParameter("@DT1", SqlDbType.Date) With {.Value = DT1.EditValue}
                prm2(2) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom}
                prm2(3) = New SqlParameter("@ExistVal", SqlDbType.Bit) With {.Value = ExistVal}
                dtt1 = RUN_QUARY_PRO("AccSafeActivityTbPOC_TOTETDATE", prm2)
                If dtt1.Rows.Count > 0 Then
                    GridControl1.DataSource = dtt1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub lodetoltat(accode As Integer)
        GridControl2.DataSource = Nothing
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ACCCODE", SqlDbType.BigInt) With {.Value = accode}
        Dim dt2 As New DataTable
        dt2.Clear()
        dt2 = RUN_QUARY_PRO("accacounselecttotaldor", PRM)
        If dt2.Rows.Count Then
            GridControl2.DataSource = dt2
            dt2.Dispose()
        End If

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If DT1.EditValue > dt2.EditValue Then
            DT1.ErrorText = "لا يمكن ان يكون التاريخ الاول اكبر من التاريخ الثاني"
            DT1.Focus()
            Exit Sub
        End If
        If DT1.Text = String.Empty Then
            DT1.ErrorText = "يرجا اختيار التاريخ"
            DT1.Focus()
            Exit Sub
        End If
        If dt2.Text = String.Empty Then
            dt2.ErrorText = "يرجا اختيار التاريخ"
            dt2.Focus()
            Exit Sub
        End If
        loaddate(ACCCODE.Tag, 1)
    End Sub
    Sub PRINT()
        Try
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@ACCCODE", ACCCODE.Tag)
            PRM(1) = New SqlParameter("@date1", Convert.ToDateTime(DT1.EditValue))
            PRM(2) = New SqlParameter("@date2", Convert.ToDateTime(dt2.EditValue))
            Dim ds As New DataSet
            Dim dt As DataTable = RUN_QUARY_PRO("FRMaccounDEtells_print", PRM)
            dt.TableName = "ACCOUNTSTB"
            ds.Tables.Add(dt)

            'If dt.Rows.Count > 0 Then
            '    Dim report As New FRMaccounDEtells_Print
            '    report.DataSource = ds
            '    report.DataAdapter = DA
            '    report.DataMember = "ACCOUNTSTB"
            '    Dim tool As ReportPrintTool = New ReportPrintTool(report)
            '    report.CreateDocument()
            '    report.ShowPreview()
            'Else
            '    MetroMessageBox.Show(Me, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبــــــــــــــية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            If e.ListSourceRowIndex <> 0 Then
                e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1 - 1
            End If
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        PRINT()
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        If GridView1.RowCount > 0 Then
            If DT1.EditValue > dt2.EditValue Then
                DT1.ErrorText = "لا يمكن ان يكون التاريخ الاول اكبر من التاريخ الثاني"
                DT1.Focus()
                Exit Sub
            End If
            If DT1.Text = String.Empty Then
                DT1.ErrorText = "يرجا اختيار التاريخ"
                DT1.Focus()
                Exit Sub
            End If
            If dt2.Text = String.Empty Then
                dt2.ErrorText = "يرجا اختيار التاريخ"
                dt2.Focus()
                Exit Sub
            End If
            loaddate(ACCCODE.Tag, GridView1.GetFocusedRowCellValue("ID_CRUNSE"))
        End If
    End Sub
    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName = "total" Then
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
End Class