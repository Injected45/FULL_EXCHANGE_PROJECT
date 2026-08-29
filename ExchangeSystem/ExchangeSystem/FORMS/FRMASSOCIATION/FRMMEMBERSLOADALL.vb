Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SelectPdf
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMMEMBERSLOADALL
    Public LOADTYPE As Integer = 1
    Public memberacc As ULong
    Public membenam As String

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(98, UserID)


        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSearch") = 0 Then LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

            If dt.Rows(0)("CanPrint") = 0 Then LayoutControlItem10.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem10.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub

    Sub NEWRECORD()
        OverAllDebit.BackColor = Color.Red
        OverAllCredit.BackColor = Color.Green
        NEWDVGFROMAT(GVRole)
        LOADASS()
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        GCRole.DataSource = Nothing
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
        dt = RUN_QUARY_TXT("ASSOCIATIONNAMETB_LOADTODVG")
        If dt.Rows.Count > 0 Then
            AssID.Properties.DataSource = dt
            AssID.Properties.ValueMember = "ID"
            AssID.Properties.DisplayMember = "ASSNAME"
            AssID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub AssID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AssID.QueryPopUp
        AssID.Properties.PopulateColumns()
        AssID.Properties.Columns("ID").Visible = False
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
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = 0}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = AssID.EditValue}
        PRM(4) = New SqlParameter("@LOADTYPE", SqlDbType.Int) With {.Value = 1}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AssActivityTb_GETMEMBER", PRM)
        If DT.Rows.Count > 0 Then
            GVRole.Columns.Clear()
            GCRole.DataSource = DT
            GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Red
            GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Green
            NEWDVGFROMAT(GVRole)
            GVRole.Columns("#").Width = 70
            GVRole.Columns("AccID").Visible = False
            SumTotal()
        ElseIf DT.Rows.Count = 0 Then
            GVRole.Columns.Clear()
            GCRole.DataSource = Nothing
            ErrorMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها خلال هذه الفترة")
            OverAllCredit.EditValue = 0.000
            OverAllDebit.EditValue = 0.000
            OverAllTotal1.EditValue = 0.000
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
        SumTotal()
    End Sub
    Private Sub FRMLOADSALARIES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormLocation(Me)
        lodePreportes()
        NEWRECORD()
        AssID.EditValue = -1
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        LOADDATA()
    End Sub

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        If GVRole.RowCount > 0 Then
            memberacc = GVRole.GetFocusedRowCellValue("AccID")
            membenam = GVRole.GetFocusedRowCellValue("الاسم")
            FRMMEMBERACCSTATEMENT.Text = "كشف حركة العضو" & Space(1) & GVRole.GetFocusedRowCellValue("الاسم")
            FRMMEMBERACCSTATEMENT.LOADTYPE = 2
            FRMMEMBERACCSTATEMENT.GCRole.DataSource = Nothing
            FRMMEMBERACCSTATEMENT.LOADDATA()
            FRMMEMBERACCSTATEMENT.ShowDialog()
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
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

            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", 0)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            PRM(3) = New SqlParameter("@AssID", AssID.EditValue)
            PRM(4) = New SqlParameter("@LOADTYPE", 1)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AssActivityTb_GETMEMBER", PRM)
            dt.TableName = "AssActivityTb"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTMEMBERSLOADALL

                report.DataSource = ds
                report.DataMember = "AssActivityTb"
                report.FilterString = GVRole.ActiveFilterString
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Sub SumTotal()
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

    Private Sub AssID_TextChanged(sender As Object, e As EventArgs) Handles AssID.EditValueChanged
        GCRole.DataSource = Nothing
        OverAllCredit.EditValue = 0.000
        OverAllDebit.EditValue = 0.000
        OverAllTotal1.EditValue = 0.000
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        SumTotal()
    End Sub
    Dim report As New RPTCustomerMovement
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}




    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs)
        '    Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        '    XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        '    Dim lookFeelError As New UserLookAndFeel(Me)
        '    lookFeelError.Style = LookAndFeelStyle.Skin
        '    lookFeelError.UseDefaultLookAndFeel = False
        '    lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        '    XtraMessageBox.AllowCustomLookAndFeel = True



        '    If GVRole.RowCount = 0 Then
        '        XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Exit Sub
        '    End If
        '    Try
        '        If SQLCON.State = ConnectionState.Closed Then
        '            SQLCON.Open()
        '        End If

        '        Using cmd1 As SqlCommand = New SqlCommand("ZRPT_AssActivityTb_GETMEMBER")
        '            cmd1.CommandType = CommandType.StoredProcedure
        '            cmd1.Parameters.AddWithValue("@AccID", 0)
        '            cmd1.Parameters.AddWithValue("@D1", D1.EditValue)
        '            cmd1.Parameters.AddWithValue("@D2", D2.EditValue)
        '            cmd1.Parameters.AddWithValue("@AssID", AssID.EditValue)
        '            cmd1.Parameters.AddWithValue("@LOADTYPE", 1)


        '            cmd1.Connection = SQLCON
        '            Dim DA As New SqlDataAdapter(cmd1)
        '            Dim ds As New DataSet
        '            DA.Fill(ds)
        '            Using dr1 As SqlDataReader = cmd1.ExecuteReader()
        '                dr1.Read()
        '                If dr1.HasRows Then
        '                    Dim report As New RPTMEMBERSLOADALL

        '                    report.DataSource = ds
        '                    report.DataAdapter = DA
        '                    report.DataMember = "AssActivityTb"
        '                    report.FilterString = GVRole.ActiveFilterString
        '                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
        '                    report.CreateDocument()
        '                    report.ExportToPdf(pdfExportFile, PdfExportOptions)

        '                    ' إرسال PDF عبر WhatsApp
        '                    SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(AssID.EditValue), pdfExportFile, $"كشف حساب  {AssID.Text} ", " كشف الحساب" & ".pdf")
        '                    'SQLCON.Close()
        '                Else
        '                    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '                End If
        '            End Using
        '        End Using
        '        If SQLCON.State = ConnectionState.Open Then
        '            SQLCON.Close()
        '        End If
        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    End Try

    End Sub

End Class