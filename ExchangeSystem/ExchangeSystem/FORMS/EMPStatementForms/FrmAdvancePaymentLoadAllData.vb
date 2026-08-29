Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FrmAdvancePaymentLoadAllData
    Sub LOADDATA()
        GCRole.DataSource = Nothing
        'If BranchID.EditValue = -1 Then
        '    BranchID.ErrorText = "يجب اختيار الفرع"
        '    Return
        'End If
        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "بداية الفترة لا يجب أن تكون أكبر من نهاية الفترة"
            Return
        End If
        Dim PR(1) As SqlParameter
        'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(0) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PR(1) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AdvancePaymentTb_LoadAllData", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFROMAT()
        End If
    End Sub


    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
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

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LOADDATA()
    End Sub

    Private Sub FrmAdvancePaymentLoadAllData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        DVGFROMAT()
        GCRole.DataSource = Nothing
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        GVRole.OptionsBehavior.Editable = False
    End Sub

    Private Sub GVRole_CustomDrawCell(sender As Object, e As RowCellCustomDrawEventArgs) Handles GVRole.CustomDrawCell

    End Sub

    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName = "الحالة" Then
            Dim _length As String = CStr(e.CellValue)
            If _length = "مستمرة" Then
                e.Appearance.ForeColor = Color.White
                e.Appearance.BackColor = Color.Red
            End If
            If _length = "منتهية" Then
                e.Appearance.ForeColor = Color.White
                e.Appearance.BackColor = Color.Green
            End If
        End If
    End Sub


    Sub Print()

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
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@D1", D1.EditValue)
            PRM(1) = New SqlParameter("@D2", D2.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AdvancePaymentTb_LoadAllData", PRM)
            Dim ds As New DataSet
            dt.TableName = "AdvancePaymentTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTAdvancePaymentLoadAllData
                report.DataSource = ds
                report.DataMember = "AdvancePaymentTb"
                report.FilterString = GVRole.ActiveFilterString
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub



    Private Sub SimpleButton12_Click(sender As Object, e As EventArgs) Handles SimpleButton12.Click
        Print()
    End Sub
End Class