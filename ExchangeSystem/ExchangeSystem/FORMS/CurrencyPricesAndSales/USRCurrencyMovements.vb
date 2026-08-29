Imports System.Data.SqlClient
Imports DevExpress
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Class USRCurrencyMovements
    Public lookAndFeelError As New UserLookAndFeel(Me)

    Public Sub CurrencyMovements_fillCrid()
        Try
            GridControl1.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()

            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = TYPElock.SelectedIndex}
            prm(1) = New SqlParameter("@ISID", SqlDbType.BigInt) With {.Value = BanckID.EditValue}
            prm(2) = New SqlParameter("@DT1", SqlDbType.Date) With {.Value = DateEdit1.EditValue}
            prm(3) = New SqlParameter("@Ueserinsert", SqlDbType.Int) With {.Value = SafeID.EditValue}
            prm(4) = New SqlParameter("@TypeMovet", SqlDbType.Int) With {.Value = TextEdit5.SelectedIndex}
            prm(5) = New SqlParameter("@DT2", SqlDbType.Date) With {.Value = DateEdit2.EditValue}
            dt = RUN_QUARY_PRO("CurrencyMovements_fillCrid", prm)

            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
                DVGFormat()
            Else
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
                XtraMessageBox.Show(lookAndFeelError, "لايوجد بيانات في الوقت الحالي", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            MD_MYSQL.LogAppError("caught in form", Me, ex)   ' also record it in mysql_errors.log
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالة خطأ في البرنامج الرجاء التواصل مع الدعم الفني", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Sub LOADSafeID()


        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("USID", GetType(Integer))
        dt.Columns.Add("UName", GetType(String))
        dt = RUN_QUARY_TXT("TB_Users_lode")
        dt.Rows.Add(0, "الكل")
        If dt.Rows.Count > 0 Then
            SafeID.Properties.DataSource = dt
            SafeID.Properties.ValueMember = "USID"
            SafeID.Properties.DisplayMember = "UName"
            SafeID.Properties.KeyMember = BID
            SafeID.Properties.ShowHeader = False
        End If

    End Sub

    Sub BanksTb_LODE()



        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("BankName", GetType(String))
        dt = RUN_QUARY_TXT("BanksTb_LODE")
        dt.NewRow()

        dt.Rows.Add(0, "الكل")
        If dt.Rows.Count > 0 Then
            BanckID.Properties.DataSource = dt
            BanckID.Properties.ValueMember = "ID"
            BanckID.Properties.DisplayMember = "BankName"
            BanckID.Properties.KeyMember = BID
            BanckID.Properties.ShowHeader = False
        End If

    End Sub

    Private Sub USRCurrencyMovements_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NOWrecored()
    End Sub
    Sub DVGFormat()

        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = True
        GVRole.ShowFindPanel()
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

        ' GridLocalizer.Active = New MyGridLocalizer()
        If TYPElock.SelectedIndex = 2 Then
            GVRole.Columns("TYPE").Visible = True

        Else
            GVRole.Columns("TYPE").Visible = False

        End If

        GVRole.Columns("SN").Width = 70
        GVRole.Columns("TYPE").Width = 70
        GVRole.Columns("ISID").Width = 150
        GVRole.Columns("CurrencyFrom").Width = 150
        GVRole.Columns("CurrencyTo").Width = 150
        GVRole.Columns("BuyPrice").Width = 100
        GVRole.Columns("SalePrice").Width = 100
        GVRole.Columns("RetBuyPrice").Width = 100
        GVRole.Columns("retSalePrice").Width = 100
        GVRole.Columns("Ueserinsert").Width = 120
        GVRole.Columns("InsertDate").Width = 90
        GVRole.Columns("DateForTime").Width = 120
        GVRole.Columns("TypeMovet").Width = 70
    End Sub
    Public Sub NOWrecored()
        TYPElock.SelectedIndex = -1
        SafeID.EditValue = -1
        DateEdit1.EditValue = Date.Now
        DateEdit2.EditValue = Date.Now
        TextEdit5.SelectedIndex = -1
        BanckID.EditValue = -1
        GridControl1.DataSource = Nothing
        LOADSafeID()
        BanksTb_LODE()
        DVGFormat()
    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 100, 102), e.Bounds)
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

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If TYPElock.SelectedIndex = -1 Then
            TYPElock.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        If DateEdit1.Text = String.Empty Then
            DateEdit1.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        If DateEdit2.Text = String.Empty Then
            DateEdit2.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        If TextEdit5.SelectedIndex = -1 Then
            TextEdit5.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        If BanckID.EditValue = -1 Then
            BanckID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        CurrencyMovements_fillCrid()
    End Sub
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName = "TypeMovet" Then
            Dim _length As String = CStr(e.CellValue)
            If _length = "اضافة" Then
                e.Appearance.ForeColor = Color.White
                e.Appearance.BackColor = Color.Green
            End If
        End If
        If e.Column.FieldName = "TypeMovet" Then
            Dim _length As String = CStr(e.CellValue)
            If _length = "تعديل" Then
                e.Appearance.ForeColor = Color.Black
                e.Appearance.BackColor = Color.Yellow
            End If
        End If

        If e.Column.FieldName = "TypeMovet" Then
            Dim _length As String = CStr(e.CellValue)
            If _length = "حذف" Then
                e.Appearance.ForeColor = Color.White
                e.Appearance.BackColor = Color.Red
            End If
        End If


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

            Dim PRM(5) As SqlParameter
            PRM(0) = New SqlParameter("@TYPE", TYPElock.SelectedIndex)
            PRM(1) = New SqlParameter("@ISID", BanckID.EditValue)
            PRM(2) = New SqlParameter("@DT1", DateEdit1.EditValue)
            PRM(3) = New SqlParameter("@Ueserinsert", SafeID.EditValue)
            PRM(4) = New SqlParameter("@TypeMovet", TextEdit5.SelectedIndex)
            PRM(5) = New SqlParameter("@DT2", DateEdit2.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CurrencyMovements_fillCrid", PRM)
            Dim ds As New DataSet
            dt.TableName = "CurrencyMovementsTable"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTCurrencyMovements
                report.DataSource = ds
                report.DataMember = "CurrencyMovementsTable"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'Else
                '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


End Class
