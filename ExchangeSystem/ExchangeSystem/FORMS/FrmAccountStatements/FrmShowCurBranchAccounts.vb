Imports DevExpress
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Tile
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient
Public Class FrmShowCurBranchAccounts
    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}

        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DT.Rows.Add(0, "الكل")
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"

        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False


        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        'GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        'GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(2, 84, 100), e.Bounds)
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


    Public Sub CurrencymovementPruse_FILLGRid()
        If CurrencyTo.EditValue = -1 Or CurrencyTo.Text = String.Empty Then
            CurrencyTo.ErrorText = "الرجاء اختيار العملة "
            Return
        End If
        If TextEdit4.SelectedIndex = -1 Then
            TextEdit4.ErrorText = " هذا الحقل مطلوب"
            Return
        End If

        If DateEdit11.Text = String.Empty Then
            DateEdit11.ErrorText = " هذا الحقل مطلوب"
            Return
        End If


        If DateEdit2.Text = String.Empty Then
            DateEdit2.ErrorText = " هذا الحقل مطلوب"
            Return
        End If


        If DateEdit11.EditValue > DateEdit2.EditValue Then
            DateEdit11.ErrorText = "عذرا يجب ان يكون التاريخ الاول اصغر او يساوي التاريخ الثاني"
            Return
        End If

        TextEdit2.EditValue = 0.00
        TextEdit21.EditValue = 0.00
        TextEdit22.EditValue = 0.00
        GridControl2.DataSource = Nothing

        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = TextEdit4.SelectedIndex}
        prm(1) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DateEdit11.EditValue}
        prm(2) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DateEdit2.EditValue}
        prm(3) = New SqlParameter("@ACCFRom", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
        prm(4) = New SqlParameter("@UName", SqlDbType.Int) With {.Value = FrmShowSafeMovement.SafeID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CurrencymovementEMPSAFE_FILLGRid", prm)

        If dt.Rows.Count > 0 Then
            GridControl2.DataSource = dt
            DVGFormat()
        Else
            MessageBox.Show("عذرا لايوجد بيانات في الوقت الحالي ", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        CurrencymovementPruse_FILLGRid()
        If TextEdit4.SelectedIndex = 0 Then
            ACC_DEPET_TO.Visible = False
            salesPurchaseprice.Visible = False
            Revenue.Visible = False
            CRedetTO.Visible = True
            buyprice.Visible = True
            CRedetDL.Visible = True


        End If



        If TextEdit4.SelectedIndex = 1 Then
            CRedetTO.Visible = False
            buyprice.Visible = False
            CRedetDL.Visible = False
            ACC_DEPET_TO.Visible = True
            Revenue.Visible = True
            salesPurchaseprice.Visible = True
        End If
        If TextEdit4.SelectedIndex = 2 Then
            CRedetTO.Visible = True
            buyprice.Visible = True
            CRedetDL.Visible = True
            ACC_DEPET_TO.Visible = True
            salesPurchaseprice.Visible = True
            Revenue.Visible = True
        End If


    End Sub

    Private Sub FRMViewCurrencyPurchaseTransactions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CurrencyTo.EditValue = FrmShowSafeMovement.CurrencyID.EditValue
        DateEdit11.EditValue = FrmShowSafeMovement.D1.EditValue
        DateEdit2.EditValue = FrmShowSafeMovement.D2.EditValue
        TextEdit4.SelectedIndex = -1
        GridControl2.DataSource = Nothing
        TextEdit2.Text = 0.000
        TextEdit21.Text = 0.000
        TextEdit22.Text = 0.000
        DVGFormat()
        GET_TABLE_FOR_Costof_PROC()
        LOADCIDFROMT()
        DVGFormat(GridLookUpEdit1View)
    End Sub


    Public Sub GET_TABLE_FOR_Costof_PROC()
        Try
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ACCID", SqlDbType.Int) With {.Value = FrmShowSafeMovement.SafeID.EditValue}

            Dim dt As New DataTable
            dt.Clear()

            GridControl1.DataSource = Nothing
            dt = RUN_QUARY_PRO("GET_TABLE_FOR_Costof_UESER_AccACount_PROC", prm)
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
            Else

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub TextEdit2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit2.KeyPress
        e.Handled = True
    End Sub



    Private Sub TextEdit22_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit22.KeyPress
        e.Handled = True
    End Sub

    Private Sub TextEdit21_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit21.KeyPress
        e.Handled = True
    End Sub

    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        If GVRole.RowCount > 0 Then
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            CreditSum.FieldName = "CRedetDL"
            GVRole.Columns("CRedetDL").Summary.Add(CreditSum)




            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            DebitSum.FieldName = "Revenue"
            GVRole.Columns("Revenue").Summary.Add(DebitSum)
            GVRole.Columns("Revenue").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0

            TextEdit2.EditValue = Convert.ToDouble(GVRole.Columns("CRedetDL").SummaryItem.SummaryValue)
            GVRole.Columns("CRedetDL").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            TextEdit21.EditValue = Convert.ToDouble(GVRole.Columns("Revenue").SummaryItem.SummaryValue)

            TextEdit22.EditValue = TextEdit2.EditValue - TextEdit21.EditValue

        End If
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

            Dim prm(4) As SqlParameter
            prm(0) = New SqlParameter("@TYPE", TextEdit4.SelectedIndex)
            prm(1) = New SqlParameter("@dt1", DateEdit11.EditValue)
            prm(2) = New SqlParameter("@dt2", DateEdit2.EditValue)
            prm(3) = New SqlParameter("@ACCFRom", CurrencyTo.EditValue)
            prm(4) = New SqlParameter("@UName", FrmShowSafeMovement.SafeID.EditValue)

            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CurrencymovementPruseUser_FILLGRid", prm)
            If dt.Rows.Count > 0 Then

                Dim report0 As New RPTViewCurrencyPurchaseUser0
                Dim report1 As New RPTViewCurrencyPurchaseUser1
                Dim report As New RPTViewCurrencyPurchaseUser



                report0.DataSource = dt
                report0.DataAdapter = ""
                report0.DataMember = "CurrencymovementPruse"
                Dim tool0 As ReportPrintTool = New ReportPrintTool(report0)


                report1.DataSource = dt
                report1.DataAdapter = ""
                report1.DataMember = "CurrencymovementPruse"
                Dim tool1 As ReportPrintTool = New ReportPrintTool(report1)

                report.DataSource = dt
                report.DataAdapter = ""
                report.DataMember = "CurrencymovementPruse"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)

                If TextEdit4.SelectedIndex = 0 Then
                    report0.FilterString = GVRole.ActiveFilterString
                    report0.CreateDocument()
                    report0.ShowPreview()
                End If

                If TextEdit4.SelectedIndex = 1 Then
                    report1.FilterString = GVRole.ActiveFilterString
                    report1.CreateDocument()
                    report1.ShowPreview()
                End If

                If TextEdit4.SelectedIndex = 2 Then
                    report.FilterString = GVRole.ActiveFilterString
                    report.CreateDocument()
                    report.ShowPreview()
                End If



                'Else
                '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub TileView1_ItemCustomize(sender As Object, e As TileViewItemCustomizeEventArgs) Handles TileView1.ItemCustomize
        If e.Item.Elements(0).Text = "دينار ليبي" Then
            e.Item.Elements(2).Text = ""
            e.Item.Elements(3).Text = ""
            e.Item.Elements(6).Text = ""
            e.Item.Elements(7).Text = ""
            e.Item.Elements(0).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(4).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(5).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(1).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(5).Appearance.Normal.ForeColor = Color.White
            e.Item.Elements(1).Appearance.Normal.ForeColor = Color.White
        End If
    End Sub
End Class