Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class Association_expenses
    Sub NEWRECORD()
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        NEWDVGFROMAT(GVRole)
        LOADASS()
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        GCRole.DataSource = Nothing
        AssID.EditValue = -1
        AssID.EditValue = -1
        D1.DateTime = Date.Now
        D2.DateTime = Date.Now
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
    End Sub
    Sub LOADASS()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("ASSOCIATIONNAMETB_LOADBASEONACCID")
        If dt.Rows.Count > 0 Then
            AssID.Properties.DataSource = dt
            AssID.Properties.ValueMember = "ID"
            AssID.Properties.DisplayMember = "ASSNAME"
        End If
    End Sub


    Sub LOADDATA()
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        GVRole.Columns.Clear()
        GCRole.DataSource = Nothing
        NEWDVGFROMAT(GVRole)
        If AssID.EditValue = -1 Then
            AssID.ErrorText = "يجب اختيار الجمعية"
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
        Dim memberacc As ULong = GridLookUpEdit1View.GetFocusedRowCellValue("ASSOACCID")
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = memberacc}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("Association expenses", PRM)
        If DT.Rows.Count > 0 Then
            GVRole.Columns.Clear()
            GCRole.DataSource = DT
            GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
            GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Green
            NEWDVGFROMAT(GVRole)
            GVRole.Columns("#").Width = 70
            GVRole.Columns("طبيعة الحركة").Width = 800
            GVRole.Columns("ملاحظات").Width = 350
        ElseIf DT.Rows.Count = 0 Then
            Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
            Dim lookFeelError As New UserLookAndFeel(Me)
            lookFeelError.Style = LookAndFeelStyle.Skin
            lookFeelError.UseDefaultLookAndFeel = False
            lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها خلال هذه الفترة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
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
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        SomTotal()
    End Sub
    Private Sub FRMLOADSALARIES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)
        NEWRECORD()
        AssID.EditValue = -1
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        LOADDATA()
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        If AssID.EditValue = -1 Then
            AssID.ErrorText = "يجب اختيار الجمعية"
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
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", GridLookUpEdit1View.GetFocusedRowCellValue("ASSOACCID"))
        PRM(1) = New SqlParameter("@D1", D1.EditValue)
        PRM(2) = New SqlParameter("@D2", D2.EditValue)
        Dim dt As DataTable = RUN_QUARY_PRO("ZRPTAssociation_expenses", PRM)
        dt.TableName = "AssActivityTb"
        Dim ds As New DataSet
        ds.Tables.Add(dt)
        If dt.Rows.Count > 0 Then
            Dim report As New RPT_Association_revenues
            report.DataSource = ds
            report.DataMember = "AssActivityTb"
            report.FilterString = GVRole.ActiveFilterString
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
            'Else
            '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function lookFeelError() As IWin32Window
        Throw New NotImplementedException()
    End Function

    Private Sub AssID_TextChanged(sender As Object, e As EventArgs) Handles AssID.TextChanged
        GCRole.DataSource = Nothing
    End Sub


    Sub SomTotal()
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
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
            If OverAllCredit.EditValue > OverAllDebit.EditValue Then
                OverAllTotal1.BackColor = Color.Green
            Else
                OverAllTotal1.BackColor = Color.Red
            End If
            OverAllTotal1.EditValue = OverAllCredit.EditValue - OverAllDebit.EditValue
        End If
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SomTotal()
    End Sub
End Class
