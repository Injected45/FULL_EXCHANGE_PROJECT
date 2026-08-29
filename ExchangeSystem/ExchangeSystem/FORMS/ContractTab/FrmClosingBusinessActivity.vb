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
Public Class FrmClosingBusinessActivity
    Public Sub lodadate()
        Try
            Sumcredit.EditValue = 0.00
            SUMdibet.EditValue = 0.00
            OverAllTotal.EditValue = 0.00
            Dim pram(2) As SqlParameter
            pram(0) = New SqlParameter("@ActivityType", SqlDbType.BigInt) With {.Value = branchID.EditValue}
            pram(1) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            pram(2) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("dbo.CONDB_Activity_LoadToDVGBasedOnActivityType", pram)
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
            End If
            DVGFROMAT1()
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

    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        branchID.Properties.DataSource = Nothing
        DT.Clear()
        DT.NewRow()
        DT = RUN_QUARY_TXT("CONDB_ActivityType_LoadDataIntoLookUpEdit")
        If DT.Rows.Count > 0 Then
            branchID.Properties.DataSource = DT
            branchID.Properties.ValueMember = "AccCode"
            branchID.Properties.DisplayMember = "AccName"
            branchID.Properties.ShowHeader = False
        End If
    End Sub

    Private Sub FRMINCOMESTATMENT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserType = 1 Then
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            LayoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
        LOADBRANCH()
        DVGFROMAT1()
        branchID.EditValue = -1
        DT1.EditValue = Date.Now
        DT2.EditValue = Date.Now
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If DT1.EditValue > DT2.EditValue Then
            XtraMessageBox.Show("تاريخ البداية لا يجب أن يكون أكبر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        lodadate()
        DVGFROMAT1()
        Sumtotal()
    End Sub

    Sub Sumtotal()
        Sumcredit.EditValue = 0.000
        SUMdibet.EditValue = 0.000
        OverAllTotal.EditValue = 0.000
        If GridView1.RowCount > 0 Then
            Dim Credit As New GridColumnSummaryItem()
            Credit.SummaryType = SummaryItemType.Sum
            Credit.FieldName = "Credit"
            GridView1.Columns("Credit").Summary.Add(Credit)
            Dim Debit As New GridColumnSummaryItem()
            Debit.SummaryType = SummaryItemType.Sum
            Debit.FieldName = "Debit"
            GridView1.Columns("Debit").Summary.Add(Debit)
            Sumcredit.EditValue = Convert.ToDouble(GridView1.Columns("Credit").SummaryItem.SummaryValue)
            SUMdibet.EditValue = Convert.ToDouble(GridView1.Columns("Debit").SummaryItem.SummaryValue)
        End If
        OverAllTotal.EditValue = Sumcredit.EditValue - SUMdibet.EditValue
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView2_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView1.ColumnFilterChanged, GridView1.FocusedRowChanged
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
            If GridView1.RowCount <= 0 Then
                XtraMessageBox.Show("يرجى عرض البيانات لتتم عملية الإقفال", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            Dim reslut = XtraMessageBox.Show("هل ترغب حقا في إقفال النشاط للفترة من" & vbNewLine & Format(DT1.EditValue, "yyyy-MM-dd") & vbNewLine & "إلى" & vbNewLine & Format(DT2.EditValue, "yyyy-MM-dd"), "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If reslut = DialogResult.No Then
                Exit Sub
            End If
            Dim reslut1 = XtraMessageBox.Show("في حال الحفظ لايمكنك الرجوع عن العملية هل أنت واثق من الاستمرار؟", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If reslut1 = DialogResult.No Then
                Exit Sub
            End If
            Dim PR(5) As SqlParameter
            PR(0) = New SqlParameter("@ActivityCode", SqlDbType.BigInt) With {.Value = branchID.EditValue}
            PR(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
            PR(2) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DT1.EditValue}
            PR(3) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DT2.EditValue}
            PR(4) = New SqlParameter("@MsgSatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PR(5) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("CONDB_ClosingBusinessActivity_TB", PR)
            If PR(4).Value = 0 Then
                ErrorMessage(Me, "رسالة تنبيه", PR(5).Value)
                Exit Sub
            End If
            CONFIRMMESSAGE.Show()
            Sumcredit.EditValue = 0.00
            SUMdibet.EditValue = 0.00
            OverAllTotal.EditValue = 0.00
            GridControl1.DataSource = Nothing
        Catch ex As Exception
        ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub

    Private Sub branchID_EditValueChanged(sender As Object, e As EventArgs) Handles branchID.EditValueChanged
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
    End Sub

    Private Sub DT1_EditValueChanged(sender As Object, e As EventArgs) Handles DT1.EditValueChanged
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
    End Sub

    Private Sub DT2_EditValueChanged(sender As Object, e As EventArgs) Handles DT2.EditValueChanged
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
    End Sub

    Private Sub DT1_EditValueChanging(sender As Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles DT1.EditValueChanging
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
    End Sub

    Private Sub DT2_EditValueChanging(sender As Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles DT2.EditValueChanging
        Sumcredit.EditValue = 0.00
        SUMdibet.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        GridControl1.DataSource = Nothing
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FrmNetTotalOFActivityBusiness.ShowDialog()
    End Sub
End Class