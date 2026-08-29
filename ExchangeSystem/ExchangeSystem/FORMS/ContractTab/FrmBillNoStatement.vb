Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FrmBillNoStatement
    Private CodesList As New BindingList(Of CodeEntry)
    Public BillType As Integer = 0 ' 1= فاتورة توريد 0= فاتورة تصدير
    Public Sub LoadData()
        If BillNo.Text = String.Empty Then
            BillNo.ErrorText = "يرجى ادخال رقم الفاتورة"
            BillNo.Focus()
            Exit Sub
        End If
        GCRole.DataSource = Nothing

        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = BillNo.Text}
        If BillType = 1 Then
            PR(1) = New SqlParameter("@Action", SqlDbType.NVarChar, -1) With {.Value = 2}
        ElseIf BillType = 0 Then
            PR(1) = New SqlParameter("@Action", SqlDbType.NVarChar, -1) With {.Value = 4}
        End If
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_CategoriesTb_LoadIDACCIDTOEXPORT", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFROMAT()
            GVRole.OptionsBehavior.Editable = True
            If BillType = 1 Then
                GVRole.Columns("OverallTotal").Visible = False
            ElseIf BillType = 0 Then
                GVRole.Columns("OverallTotal").Visible = True
                GVRole.Columns("OverallTotal").VisibleIndex = 5
                GVRole.Columns("PrintRecord").VisibleIndex = 6

            End If
            For Each col In GVRole.Columns
                If col.FieldName = "PrintRecord" OrElse col.ColumnEdit Is BtnPrint Then
                    col.OptionsColumn.AllowEdit = True
                Else
                    col.OptionsColumn.AllowEdit = False
                End If
            Next
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
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub DVGFROMAT2()
        GVCODE.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVCODE.OptionsBehavior.Editable = False
        GVCODE.OptionsBehavior.EditingMode = False
        GVCODE.OptionsBehavior.ReadOnly = True
        GVCODE.OptionsView.ShowGroupPanel = False
        GVCODE.OptionsView.ShowFooter = False
        GVCODE.OptionsSelection.EnableAppearanceFocusedRow = False
        GVCODE.OptionsSelection.MultiSelectMode = False
        GVCODE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVCODE.Columns.Count - 1
            GVCODE.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVCODE.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVCODE.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVCODE.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVCODE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVCODE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVCODE.OptionsView.EnableAppearanceEvenRow = True
        GVCODE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVCODE.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Private Sub FRMViewPettyCash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFROMAT()
        GVRole.OptionsBehavior.Editable = False
        GCRole.DataSource = Nothing
        GCCODE.DataSource = CodesList
        DVGFROMAT2()
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


    Private Sub PrintRpt_Click(sender As Object, e As EventArgs) Handles PrintRpt.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True



        'If GVRole.RowCount = 0 Then
        '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        Try
            If GVRole.RowCount > 0 Then
                Dim PR(1) As SqlParameter
                PR(0) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = BillNo.Text}
                If BillType = 1 Then
                    PR(1) = New SqlParameter("@Action", SqlDbType.NVarChar, -1) With {.Value = 2}
                ElseIf BillType = 0 Then
                    PR(1) = New SqlParameter("@Action", SqlDbType.NVarChar, -1) With {.Value = 4}
                End If
                Dim DT As New DataTable
                    DT.Clear()
                    DT = RUN_QUARY_PRO("CONDB_CategoriesTb_LoadIDACCIDTOEXPORT", PR)
                    If DT.Rows.Count > 0 Then
                        Dim report As New RPTBillNo
                        report.DataSource = DT
                        report.DataMember = "PROEXPORTITEM"
                        Dim tool As ReportPrintTool = New ReportPrintTool(report)
                        report.BillNo.Text = BillNo.Text.Trim
                        report.CreateDocument()
                        report.ShowPreview()
                    Else
                        XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من رقم الفاتورة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    XtraMessageBox.Show(lookFeelError, "يرجى تعبئة البيانات أولاً", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
    End Sub


    Private Sub BtnPrint_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BtnPrint.ButtonClick
        Try

            Dim codeVal = GVRole.GetFocusedRowCellValue("Code")

            If codeVal Is Nothing OrElse codeVal.ToString().Trim() = "" Then
                InfoMessage(Me, "رسالة معلومات", "لا يمكن الطباعة لأن قيمة Code فارغة.")
                Exit Sub
            End If

            Dim RP(0) As SqlParameter
            RP(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = codeVal}

            Dim DT As New DataTable
            DT = RUN_QUARY_PRO("CONDB_ZRPT_PettyCashTb_LOADPROEXPORTITEM", RP)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                Dim report As New RPTBillNoIndividualRecord
                report.DataSource = DT
                report.DataMember = "PROEXPORTITEM"

                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.XrLabel18.Text = Cur_Code(DefaultCurrency, GVRole.GetFocusedRowCellValue("ExpensVal"), False)

                report.CreateDocument()
                report.ShowPreview()
            Else
                InfoMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها يرجى التحقق من الرمز")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        If CodesList.Any(Function(x) x.Code = BillNo1.Text.Trim) Then
            XtraMessageBox.Show("هذا الكود مضاف مسبقاً!")
            Return
        End If
        If BillNo1.Text.Trim = "" Then
            XtraMessageBox.Show("الرجاء إدخال الكود")
            Return
        End If

        ' إنشاء صف جديد
        Dim newRow As New CodeEntry With {
            .SN = CodesList.Count + 1,
            .Code = BillNo1.Text.Trim
        }

        ' إضافة الصف للقائمة
        CodesList.Add(newRow)

        ' تفريغ حقل الإدخال
        BillNo1.Text = ""
        BillNo1.Focus()
    End Sub

    Private Sub BillNo1_Leave(sender As Object, e As EventArgs) Handles BillNo1.Leave
        If CodesList.Any(Function(x) x.Code = BillNo1.Text.Trim) Then
            XtraMessageBox.Show("هذا الكود مضاف مسبقاً!")
            Return
        End If


        ' إنشاء صف جديد
        Dim newRow As New CodeEntry With {
            .SN = CodesList.Count + 1,
            .Code = BillNo1.Text.Trim
        }

        ' إضافة الصف للقائمة
        CodesList.Add(newRow)

        ' تفريغ حقل الإدخال
        BillNo1.Text = ""
        BillNo1.Focus()
    End Sub
    Function GetAllBillNos() As String
        Dim lst As New List(Of String)

        For i As Integer = 0 To GVCODE.RowCount - 1
            Dim val = GVCODE.GetRowCellValue(i, "Code")
            If val IsNot Nothing AndAlso val.ToString() <> "" Then
                lst.Add("'" & val.ToString() & "'")
            End If
        Next

        If lst.Count = 0 Then
            Return ""
        End If

        Return String.Join(",", lst)
    End Function

    Private Sub SimpleButton112_Click(sender As Object, e As EventArgs) Handles SimpleButton112.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True



        'If GVRole.RowCount = 0 Then
        '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        'Try
        If GVCODE.RowCount > 0 Then

                Dim bills As String = GetAllBillNos()

                If bills = "" Then
                    XtraMessageBox.Show("لا توجد أرقام فواتير للطباعة")
                    Exit Sub
                End If

                Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = bills}
            If BillType = 1 Then
                PR(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 3}
            ElseIf BillType = 0 Then
                PR(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 5}
            End If
            Dim DT As New DataTable
                DT = RUN_QUARY_PRO("CONDB_CategoriesTb_LoadIDACCIDTOEXPORT", PR)

                If DT.Rows.Count > 0 Then

                Dim report As New RPTBillNo
                report.Parameters("pAction").Value = If(BillType = 1, 3, 5)
                report.DataSource = DT
                    report.DataMember = "PROEXPORTITEM"

                    Dim tool As New ReportPrintTool(report)
                    report.CreateDocument()
                    tool.ShowPreview()

                Else
                    XtraMessageBox.Show("لا يوجد بيانات لعرضها")
                End If

            Else
                XtraMessageBox.Show("يرجى تعبئة البيانات أولاً")
            End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End Try
    End Sub

    Private Sub SimpleButton1111_Click(sender As Object, e As EventArgs) Handles SimpleButton1111.Click
        CodesList.Clear()
        GVCODE.RefreshData()
    End Sub

    Private Sub SimpleButton11111_Click(sender As Object, e As EventArgs) Handles SimpleButton11111.Click
        GCRole.DataSource = Nothing
        DVGFROMAT()
    End Sub
End Class
Public Class CodeEntry
    Public Property SN As Integer
    Public Property Code As String
End Class