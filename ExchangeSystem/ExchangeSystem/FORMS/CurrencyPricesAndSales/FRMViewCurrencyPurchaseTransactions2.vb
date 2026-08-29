Imports DevExpress
Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports ExchangeSystem.ExchangeSystem
Imports System.ComponentModel
Imports System.Data.SqlClient
Public Class FRMViewCurrencyPurchaseTransactions2
    Private _Helper As MyCellMergeHelper
    Public ExistVal As Boolean
    Public Profit As Double, Losses As Double


    Sub LOADCIDFROMT()
        Dim DT As New DataTable
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("CuName", GetType(String))
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 2}
        If TextEdit4.SelectedIndex = 1 Then
            DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
            DT.Rows.Add(0, "الكل")
        Else
            DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        End If
        If DT.Rows.Count > 0 Then
            CurrencyTo.Properties.DataSource = DT
            CurrencyTo.Properties.ValueMember = "ID"
            CurrencyTo.Properties.DisplayMember = "CuName"

        Else
            CurrencyTo.Properties.DataSource = Nothing
        End If
    End Sub

    Sub LoadBID()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        Branchid.Properties.DataSource = DT
        Branchid.Properties.ValueMember = "DBRID"
        Branchid.Properties.DisplayMember = "BName"
        Branchid.Properties.ShowHeader = False
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
        GridView1.OptionsBehavior.EditingMode = True
        GridView1.OptionsBehavior.ReadOnly = True
        GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GridView1.OptionsView.ShowGroupPanel = False


        For i As Integer = 0 To GridView1.Columns.Count - 1
            GridView1.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView1.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
        GridView1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        'GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView1.OptionsView.EnableAppearanceEvenRow = True
        'GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView1.OptionsView.EnableAppearanceOddRow = True

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

        Try


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
            If Branchid.Text = String.Empty Then
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

            'If TextEdit4.SelectedIndex = 0 Then


            Dim prm(3) As SqlParameter
            prm(0) = New SqlParameter("@Bid", SqlDbType.Int) With {.Value = Branchid.EditValue}
            prm(1) = New SqlParameter("@dt1", SqlDbType.Date) With {.Value = DateEdit11.EditValue}
            prm(2) = New SqlParameter("@dt2", SqlDbType.Date) With {.Value = DateEdit2.EditValue}
            prm(3) = New SqlParameter("@ACCFRom", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
            Dim dt As New DataTable
            dt.Clear()

            dt = RUN_QUARY_PRO("CurrencymovementPruse_FILLGRid3", prm)



            If dt.Rows.Count >= 0 Then

                ExistVal = 1

                GridControl2.DataSource = dt


                If TextEdit4.SelectedIndex = 0 Then
                    GridControl2.MainView = GVRole
                    CRedetTO.Caption = "مدين"
                    ACC_DEPET_TO.Caption = "دائن"
                End If


                If TextEdit4.SelectedIndex = 1 Then
                    GridControl2.MainView = GridView1
                    'CRedetTO1.Caption = "عملة مشتراه"
                    'ACC_DEPET_TO1.Caption = "عملة مباعة"
                    'GridView1.Columns("CRedetTO").VisibleIndex = 4
                    GridView1.Columns("Currencyquantity").VisibleIndex = 4
                    GridView1.Columns("buyprice").VisibleIndex = 5
                    GridView1.Columns("CRedetDL").VisibleIndex = 6
                    'GridView1.Columns("ACC_DEPET_TO").VisibleIndex = 7
                    GridView1.Columns("salesPurchaseprice").VisibleIndex = 8
                    GridView1.Columns("ACC_DEPET_DL").VisibleIndex = 9
                    GridView1.Columns("saleprice").VisibleIndex = 10
                    GridView1.Columns("Revenue").VisibleIndex = 11
                    GridView1.Columns("NetSale").VisibleIndex = 12
                    GridView1.Columns("UName").VisibleIndex = 13
                    GridView1.Columns("TYPEFROM").VisibleIndex = 14
                    GridView1.Columns("buyprice").Width = 70
                    GridView1.Columns("saleprice").Width = 70
                    GridView1.Columns("salesPurchaseprice").Width = 70
                    GridView1.Columns("NetSale").Width = 100
                End If



                Dim PR(3) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = Branchid.EditValue}
                PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyTo.EditValue}
                'PR(2) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
                PR(2) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = DateEdit11.EditValue}
                PR(3) = New SqlParameter("@ExistVal", SqlDbType.Bit) With {.Value = ExistVal}
                Dim DT1 As New DataTable
                DT1 = RUN_QUARY_PRO("AccSafeActivityTb_SelectByCurEmSafePreviews", PR)

                Dim rDt As DataTable = TryCast(GridControl2.DataSource, DataTable)
                _Helper = New MyCellMergeHelper(GVRole)
                Dim row As DataRow = rDt.NewRow()

                If DT1.Rows.Count > 0 Then

                    If TextEdit4.SelectedIndex = 0 Then
                        row("ISID") = "رصيد سابق"
                        row("CRedetTO") = DT1.Rows(0)("مدين")
                        row("ACC_DEPET_TO") = DT1.Rows(0)("دائن")
                        dt.Rows.InsertAt(row, 0)
                        GridControl2.DataSource = rDt
                        _Helper.AddMergedCell(0, 0, 1, 2, "MyMergedCell1 (Very long text)")
                    End If
                End If

            ElseIf dt.Rows.Count = 0 Then
                GridControl2.DataSource = Nothing
                MessageBox.Show("لا يوجد بيانات لعرضها", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ExistVal = 0
                DVGFormat()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try


    End Sub

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        CurrencymovementPruse_FILLGRid()
        If TextEdit4.SelectedIndex = 0 Then
            Sumtotal1()
        Else
            SumTotal2()
        End If
    End Sub

    Private Sub FRMViewCurrencyPurchaseTransactions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFormat()
        LoadBID()
        LOADCIDFROMT()
        GridControl1.DataSource = Nothing
        GridControl2.DataSource = Nothing
        TextEdit2.EditValue = 0.00
        TextEdit21.EditValue = 0.00
        TextEdit22.EditValue = 0.00
        Branchid.EditValue = -1
        TextEdit4.SelectedIndex = -1
        CurrencyTo.EditValue = -1
        DateEdit11.EditValue = Date.Now
        DateEdit2.EditValue = Date.Now
        Branchid.EditValue = BID
        If UserType = 1 Then
            Branchid.Enabled = True
        Else
            Branchid.Enabled = False
        End If

    End Sub


    Public Sub GET_TABLE_FOR_Costof_PROC()
        Try
            GridControl1.DataSource = Nothing
            GridControl2.DataSource = Nothing
            TextEdit2.EditValue = 0.00
            TextEdit21.EditValue = 0.00
            TextEdit22.EditValue = 0.00
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@BranchId", SqlDbType.Int) With {.Value = Branchid.EditValue}
            GridControl1.DataSource = Nothing
            dt = RUN_QUARY_PRO("GET_TABLE_FOR_Costof_PROC", prm)
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

    Sub Sumtotal1()
        TextEdit2.EditValue = 0.000
        TextEdit21.EditValue = 0.000
        TextEdit22.EditValue = 0.000
        Try
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum





            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum



            If TextEdit4.SelectedIndex = 0 Then
                CreditSum.FieldName = "CRedetTO"
                GVRole.Columns("CRedetTO").Summary.Add(CreditSum)
                DebitSum.FieldName = "ACC_DEPET_TO"
                GVRole.Columns("ACC_DEPET_TO").Summary.Add(DebitSum)

                TextEdit2.EditValue = Convert.ToDouble(GVRole.Columns("CRedetTO").SummaryItem.SummaryValue)

                TextEdit21.EditValue = Convert.ToDouble(GVRole.Columns("ACC_DEPET_TO").SummaryItem.SummaryValue)
                TextEdit22.EditValue = TextEdit2.EditValue - TextEdit21.EditValue
                LayoutControlItem6.Text = "إجمالي الدائن"
                LayoutControlItem4.Text = "إجمالي المدين"
                TextEdit2.BackColor = Color.Green
                TextEdit21.BackColor = Color.Red

            End If

        Catch ex As Exception
            MessageBox.Show("خطأ", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        'If GVRole.RowCount > 0 Then




        Sumtotal1()


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



    Private Sub GridView1_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Try
            Dim View As GridView = TryCast(sender, GridView)

            If e.Column.FieldName = "Currencyquantity" Then
                Dim _length As String = CStr(e.CellValue)
                If _length > 0 Then
                    'e.Appearance.ForeColor = Color.White
                    e.Appearance.BackColor = Color.FromArgb(0, 192, 0)
                End If

            End If



            If e.Column.FieldName = "Currencyquantity" Then
                Dim _length As String = CStr(e.CellValue)
                If _length <= 0 Then
                    e.Appearance.ForeColor = Color.White
                    e.Appearance.BackColor = Color.Red
                End If
            End If

            If e.Column.FieldName = "NetSale" Then
                Dim _length As String = CStr(e.CellValue)
                If _length > 0 Then
                    e.Appearance.ForeColor = Color.White
                    e.Appearance.BackColor = Color.Green
                End If
            End If
            If e.Column.FieldName = "NetSale" Then
                Dim _length As String = CStr(e.CellValue)
                If _length < 0 Then
                    e.Appearance.ForeColor = Color.White
                    e.Appearance.BackColor = Color.Red
                End If
            End If





        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub PrintRPT(sender As Object, e As EventArgs) Handles SimpleButton1.Click
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
            If TextEdit4.SelectedIndex = 0 Then
                If GVRole.RowCount < 1 Then
                    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

            End If
            If TextEdit4.SelectedIndex = 1 Then
                If GridView1.RowCount < 1 Then
                    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If
            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@bid", Branchid.EditValue)
            PRM(1) = New SqlParameter("@dt1", DateEdit11.EditValue)
            PRM(2) = New SqlParameter("@dt2", DateEdit2.EditValue)
            PRM(3) = New SqlParameter("@ACCFRom", CurrencyTo.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_CurrencymovementPruse_FILLGRid", PRM)
            Dim ds As New DataSet
            dt.TableName = "CurrencymovementPruse"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTViewCurrencyPurchaseTransactions
                Dim report0 As New RPTViewuCurrencyPurchaseTransactions0


                report.DataSource = ds
                report.DataMember = "CurrencymovementPruse"


                report0.DataSource = ds
                report0.DataMember = "CurrencymovementPruse"

                'report.DataSource = ds
                'report.DataAdapter = DA
                'report.DataMember = "CurrencymovementPruse"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                Dim tool0 As ReportPrintTool = New ReportPrintTool(report0)
                If TextEdit4.SelectedIndex = 0 Then

                    report0.FilterString = GVRole.ActiveFilterString
                    report0.CreateDocument()
                    report0.ShowPreview()
                ElseIf TextEdit4.SelectedIndex = 1 Then
                    report.FilterString = GridView1.ActiveFilterString
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



    Private Sub TextEdit4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TextEdit4.SelectedIndexChanged
        LOADCIDFROMT()
        If TextEdit4.SelectedIndex = 1 Then
            CurrencyTo.EditValue = 0
            CurrencyTo.Enabled = False
        Else
            CurrencyTo.EditValue = -1
            CurrencyTo.Enabled = True
        End If
        GridControl2.DataSource = Nothing


    End Sub

    Private Sub bid_EditValueChanged(sender As Object, e As EventArgs) Handles Branchid.EditValueChanged
        GET_TABLE_FOR_Costof_PROC()
    End Sub

    Private Sub bid_QueryPopUp(sender As Object, e As CancelEventArgs) Handles Branchid.QueryPopUp
        Branchid.Properties.PopulateColumns()
        Branchid.Properties.Columns("DBRID").Visible = False

    End Sub

    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
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



    Private Sub TileView1_ItemCustomize(sender As Object, e As Views.Tile.TileViewItemCustomizeEventArgs) Handles TileView1.ItemCustomize
        If e.Item.Elements(0).Text = "دينار ليبي" Then
            e.Item.Elements(2).Text = ""
            e.Item.Elements(3).Text = "رصيد العملة"
            e.Item.Elements(4).Text = ""
            e.Item.Elements(6).Text = ""
            e.Item.Elements(7).Text = ""
            e.Item.Elements(8).Text = ""
            e.Item.Elements(9).Text = ""
            e.Item.Elements(10).Text = ""
            e.Item.Elements(11).Text = ""
            e.Item.Elements(0).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(5).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(1).Appearance.Normal.FontSizeDelta = 5
            e.Item.Elements(5).Appearance.Normal.FontSizeDelta = 5
        End If
    End Sub

    Private Sub GridView1_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView1.ColumnFilterChanged
        Dim CreditSum As New GridColumnSummaryItem()
        CreditSum.SummaryType = SummaryItemType.Sum
        Dim DebitSum As New GridColumnSummaryItem()
        DebitSum.SummaryType = SummaryItemType.Sum
        If TextEdit4.SelectedIndex = 1 Then
            CreditSum.FieldName = "CRedetDL"
            GridView1.Columns("CRedetDL").Summary.Add(CreditSum)
            DebitSum.FieldName = "ACC_DEPET_DL"
            GridView1.Columns("ACC_DEPET_DL").Summary.Add(DebitSum)
            TextEdit2.EditValue = Convert.ToDouble(GridView1.Columns("CRedetDL").SummaryItem.SummaryValue)
            TextEdit21.EditValue = Convert.ToDouble(GridView1.Columns("ACC_DEPET_DL").SummaryItem.SummaryValue)
            TextEdit22.EditValue = TextEdit21.EditValue - TextEdit2.EditValue
            LayoutControlItem4.Text = "اجمالي المصروف بالعملة المحلية"
            LayoutControlItem6.Text = "إجمالي المقبوض بالعملة المحلية"
            TextEdit2.BackColor = Color.Red
            TextEdit21.BackColor = Color.Green
            If GridView1.RowCount > 0 Then
                Profit = 0
                Losses = 0
                For i = 0 To GridView1.RowCount - 1
                    If GridView1.GetRowCellValue(i, "NetSale") > 0 Then
                        Profit += GridView1.GetRowCellValue(i, "NetSale")
                    End If
                    If GridView1.GetRowCellValue(i, "NetSale") < 0 Then
                        Losses += GridView1.GetRowCellValue(i, "NetSale")
                    End If
                Next
            End If
        End If
    End Sub
    Sub SumTotal2()
        TextEdit2.EditValue= 0.000
        TextEdit21.EditValue = 0.000
        TextEdit22.EditValue = 0.000
        Try
            Dim CreditSum As New GridColumnSummaryItem()
            CreditSum.SummaryType = SummaryItemType.Sum
            Dim DebitSum As New GridColumnSummaryItem()
            DebitSum.SummaryType = SummaryItemType.Sum
            If TextEdit4.SelectedIndex = 1 Then
                CreditSum.FieldName = "CRedetDL"
                GridView1.Columns("CRedetDL").Summary.Add(CreditSum)
                DebitSum.FieldName = "ACC_DEPET_DL"
                GridView1.Columns("ACC_DEPET_DL").Summary.Add(DebitSum)
                TextEdit2.EditValue = Convert.ToDouble(GridView1.Columns("CRedetDL").SummaryItem.SummaryValue)
                TextEdit21.EditValue = Convert.ToDouble(GridView1.Columns("ACC_DEPET_DL").SummaryItem.SummaryValue)
                TextEdit22.EditValue = TextEdit21.EditValue - TextEdit2.EditValue
                LayoutControlItem4.Text = "اجمالي المصروف بالعملة المحلية"
                LayoutControlItem6.Text = "إجمالي المقبوض بالعملة المحلية"
                TextEdit2.BackColor = Color.Red
                TextEdit21.BackColor = Color.Green
                If GridView1.RowCount > 0 Then
                    Profit = 0
                    Losses = 0
                    For i = 0 To GridView1.RowCount - 1
                        If GridView1.GetRowCellValue(i, "NetSale") > 0 Then
                            Profit += GridView1.GetRowCellValue(i, "NetSale")
                        End If
                        If GridView1.GetRowCellValue(i, "NetSale") < 0 Then
                            Losses += GridView1.GetRowCellValue(i, "NetSale")
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub GridView1_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        SumTotal2()
    End Sub

    Private Sub GVRole_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GVRole.ColumnFilterChanged
        Sumtotal1()
    End Sub

    Private Sub GridView1_ColumnWidthChanged(sender As Object, e As ColumnEventArgs) Handles GridView1.ColumnWidthChanged
        SumTotal2()
    End Sub
End Class