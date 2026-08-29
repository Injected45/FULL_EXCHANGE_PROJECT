Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMPettyCashSettlement
    Dim CLSPC As New CLSPCSETTLEMENT
    Public StID, AccLine, AccCat, EMID As Integer
    Public IsUpdate, UpdateBySalary As Boolean
    Public SettlementVal As Decimal
    Public AcID, IDCode, AccCode, AccEm, CodeID, ExpenseID As ULong
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(41, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Sub NEWRECORD()
        Code.Enabled = False
        InsertDate.EditValue = Date.Now
        ISID.Text = ""
        BranchID.EditValue = -1
        LOADBRNCHHasEmp(BranchID)
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnEdit.Caption = "إرجاع قيمة العهدة"
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        LOADRECURRENCY()
        BranchID.EditValue = BID
        CurrencyID.Text = "دينار ليبي"
        CurrencyID.Enabled = False
        IsUpdate = False
        ENAPLEDCONTROLS()
        EMPID.EditValue = -1
        Code.Text = ""
        SafeID.EditValue = -1
        PCVal.EditValue = 0.000
        ExpensVal.EditValue = 0.000
        ExType.SelectedIndex = -1
        AccIDEX.EditValue = -1
        AccIDEX.Properties.DataSource = Nothing
        ExType.Enabled = True
        Notes.Text = ""
        Notes2.Text = ""
        SurplusVal.EditValue = 0.000
        DeserevedVal.EditValue = 0.000
        OverAllExpens.EditValue = 0.000
        Dim bindlis As List(Of EntryPCSETTLEMENT) = New List(Of EntryPCSETTLEMENT)
        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
        'LOADCATTOLKP()
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        FormLocation(Me)
        If UserType = 1 Then
            BranchID.Enabled = True
            SafeID.Enabled = True
        Else
            BranchID.Enabled = False
            SafeID.Enabled = False
            SafeID.EditValue = UserAccID
        End If
    End Sub
    Public Function AccActivityInsert() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("SafeID")
        dt.Columns.Add("Debit")
        dt.Columns.Add("Credit")
        dt.Columns.Add("InsertDate")
        dt.Columns.Add("Description")
        dt.Columns.Add("ISID")
        dt.Columns.Add("IsActive")
        dt.Columns.Add("TypeID")
        dt.Columns.Add("OperationTypeID")
        dt.Columns.Add("AccBranchID")
        dt.Columns.Add("AccIDFrom")
        dt.Columns.Add("AccIDTo")
        dt.Columns.Add("IsConfirmed")
        dt.Columns.Add("IsCanceled")
        dt.Columns.Add("MovementType")
        dt.Columns.Add("CurrencyID")
        dt.Columns.Add("DailyClosed")
        dt.Columns.Add("SafeIDDailyClose")
        dt.Columns.Add("Note")
        dt.Columns.Add("SafeIDMovement")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
            Dim MOVTYPE As String = "مقابل مصروفات لحساب" & Space(1) & CellValue
            If CellValue <> String.Empty Then
                dt.Rows.Add(UserID, GVRole.GetRowCellValue(i, "ExVal"), 0.000, InsertDate.EditValue, Notes, Code.Text.Trim, 1, 14, 44, BranchID.EditValue, GVRole.GetRowCellValue(i, "AccEX"), SafeID.EditValue,
                            1, 0, GVRole.GetRowCellValue(i, "NotesDe").ToString, CurrencyID.EditValue, 0, 0, GVRole.GetRowCellValue(i, "NotesDe").ToString, MOVTYPE)
            End If
        Next
        Return dt
    End Function
#Region "Insert,Update,New"
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Function GCRole_seteing() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("ExpensVal")
        dt.Columns.Add("AccIDEX")
        dt.Columns.Add("EXID")
        dt.Columns.Add("NotesDe")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
            Dim MOVTYPE As String = "مقابل مصروفات لحساب" & Space(1) & CellValue
            If CellValue <> String.Empty Then
                dt.Rows.Add(GVRole.GetRowCellValue(i, "ExVal"), GVRole.GetRowCellValue(i, "AccEX"), GVRole.GetRowCellValue(i, "ID"),
                             GVRole.GetRowCellValue(i, "NotesDe").ToString)
            End If
        Next
        Return dt
    End Function
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            Dim dt As New DataTable
            dt.Clear()
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "يرجى اختيار الخزنة"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If ISID.Text.Trim = String.Empty Then
                ISID.ErrorText = "يرجى اختيار العهدة"
                Return
            End If
            If PCVal.EditValue = 0.000 Then
                PCVal.ErrorText = "القيمة يجب أن لا تساوي صفر أو أٌل"
                Return
            End If
            If GVRole.RowCount = 0 Then
                ErrorMessage(Me, "رسالة خطأ", "يجب اختيار مصروف واحد على الأقل")
                Exit Sub
            End If
            dt = CLSPC.PCSettlementTB_CHECKCODE(Code.Text.Trim)
            CLSPC.PCSettlement_insert(Code.Text.Trim, InsertDate.EditValue, EMID, BranchID.EditValue, UserID, CurrencyID.EditValue, ISID.Text.Trim, PCVal.EditValue,
                                          SettlementVal, Notes.Text.Trim, CodeID, SafeID.EditValue, EMPID.EditValue, IsUpdate, 1, 0, "", EMPID.Text, GCRole_seteing())
        End If

        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_PCSettlementTB_SELECTByCODE", PRM)
            Dim ds As New DataSet
            dt.TableName = "PCSettlementTB"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTPettyCashSettlement
                report.DataSource = ds
                report.DataMember = "PCSettlementTB"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                If PCVal.EditValue > OverAllExpens.EditValue Then
                    report.SurplusVal.Text = Cur_Code("دينار ليبي", PCVal.EditValue - OverAllExpens.EditValue, True, "n2")
                    report.DeserevedVal.Text = "0.000" + " " + "د.ل"
                ElseIf PCVal.EditValue < OverAllExpens.EditValue Then
                    report.DeserevedVal.Text = Cur_Code("دينار ليبي", OverAllExpens.EditValue - PCVal.EditValue, True, "n2")
                    report.SurplusVal.Text = "0.000" + " " + "د.ل"
                Else
                    report.SurplusVal.Text = "0.000" + " " + "د.ل"
                    report.DeserevedVal.Text = "0.000" + " " + "د.ل"
                End If
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رساله تنبية ", ex.Message)
        End Try
    End Sub
    Sub SHOW_EMCUSCODE(x)
        GCRole.DataSource = Nothing
        BranchID.EditValue = -1
        If Me.IsUpdate = True Then
            GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
            GVRole.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False
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
            Dim DT As New DataTable
            DT.Clear()
            DT = CLSPC.SERACH_PCSETTLEMENT(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                BranchID_TextChanged(Nothing, Nothing)
                CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
                SafeID.EditValue = DT.Rows(0)("AccSafeID")
                InsertDate.EditValue = DT.Rows(0)("InsertDate")
                Notes.Text = DT.Rows(0)("Notes").ToString
                ISID.Text = DT.Rows(0)("ISID").ToString
                EMPID.EditValue = DT.Rows(0)("EmpAccID")
                PCVal.EditValue = DT.Rows(0)("PCVal")
                EMID = DT.Rows(0)("EMPID")
                SettlementVal = DT.Rows(0)("SettlementVal")
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = x}
                Dim DTT As New DataTable
                DTT.Clear()
                DTT = RUN_QUARY_PRO("PCSettlementDetailsTB_SELECTByCODE", PR)
                If DTT.Rows.Count > 0 Then
                    GCRole.DataSource = DTT
                End If
            End If
        End If
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            Dim dt As New DataTable
            dt.Clear()
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "يرجى اختيار الخزنة"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If ISID.Text.Trim = String.Empty Then
                ISID.ErrorText = "يرجى اختيار العهدة"
                Return
            End If
            If PCVal.EditValue = 0.000 Then
                PCVal.ErrorText = "القيمة يجب أن لا تساوي صفر أو أٌل"
                Return
            End If
            If GVRole.RowCount = 0 Then
                ErrorMessage(Me, "رسالة خطأ", "يجب اختيار مصروف واحد على الأقل")
                Exit Sub
            End If
            dt = CLSPC.PCSettlementTB_CHECKCODE(Code.Text.Trim)

            CLSPC.PCSettlement_insert(Code.Text.Trim, InsertDate.EditValue, EMID, BranchID.EditValue, UserID, CurrencyID.EditValue, ISID.Text.Trim, PCVal.EditValue,
                                          SettlementVal, Notes.Text.Trim, CodeID, SafeID.EditValue, EMPID.EditValue, IsUpdate, 1, 0, "", EMPID.Text, GCRole_seteing())
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
#End Region
    'Sub LOADBRANCH()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
    '    If dt.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = dt
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.ShowHeader = False
    '    End If
    'End Sub
    Sub LOADRECURRENCY()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CURRENCYTB_LoadToLKP")
        CurrencyID.Properties.DataSource = DT
        CurrencyID.Properties.ValueMember = "ID"
        CurrencyID.Properties.DisplayMember = "CurrencyName"
        CurrencyID.Properties.ShowHeader = False
    End Sub
    Sub LOADSafeID()
        SafeID.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasVal", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.DataSource = dt
                SafeID.Properties.ValueMember = "AccID"
                SafeID.Properties.DisplayMember = "UNAME"
                SafeID.Properties.KeyMember = BranchID.EditValue
                SafeID.Properties.ShowHeader = False
            End If
        Else
            SafeID.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADEXPANSTYPE()
        If ExType.SelectedIndex = -1 Then
            ExType.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = ExType.SelectedIndex}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[ExpensesTb_LOADTOLKPBasedOnExType]", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.DataSource = dt
                AccIDEX.Properties.ValueMember = "AccID"
                AccIDEX.Properties.DisplayMember = "AccName"
                AccIDEX.Properties.ShowHeader = False
            End If
        Else
            AccIDEX.Enabled = False
            AccIDEX.Properties.DataSource = Nothing
        End If
    End Sub
    Sub ENAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = True
        SafeID.Enabled = True
        CurrencyID.Enabled = False
        EMPID.Enabled = True
        PCVal.Enabled = False
        Notes.Enabled = True
        InsertDate.Enabled = False
        ISID.Enabled = False
        EMPID.Enabled = True
        AccIDEX.Enabled = True
        ExpensVal.Enabled = True
        Notes2.Enabled = True
    End Sub
    Sub DISAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = False
        SafeID.Enabled = False
        CurrencyID.Enabled = False
        EMPID.Enabled = False
        PCVal.Enabled = False
        Notes.Enabled = False
        InsertDate.Enabled = False
        ISID.Enabled = False
        EMPID.Enabled = False
        AccIDEX.Enabled = False
        ExpensVal.Enabled = False
        Notes2.Enabled = False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.AllowDeleteRows = False
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.ReadOnly = True
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles BTNPCVIEW.Click
        If Me.BranchID.Text = String.Empty Or Me.BranchID.EditValue = -1 Then
            Me.BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If Me.EMPID.Text = String.Empty Or Me.EMPID.EditValue = -1 Then
            Me.EMPID.ErrorText = "يجب اختيار الموظف"
            Return
        End If
        FRMVIEWPCTOSETTLEMENT.LoadData()
        FRMVIEWPCTOSETTLEMENT.ShowDialog()
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        Dim sumUnit As Integer = 0
        Dim sumTotal As Double = 0.000
        GVRole.Columns("ExName").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
        SettlementVal = Convert.ToDouble(GVRole.Columns("ExVal").SummaryItem.SummaryValue)
        SPCVALS()
    End Sub
    Private Sub BtnDele_Click(sender As Object, e As EventArgs) Handles BtnDele.Click
        If IsUpdate = False Then
            If BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
                If GVRole.RowCount = 2 Then
                    AccIDEX_TextChanged(Nothing, Nothing)
                    Exit Sub
                End If
            End If
        End If
        GVRole.DeleteRow(GVRole.FocusedRowHandle)
        Dim rowIdx As Integer = GVRole.DataRowCount - 1
        For i As Integer = rowIdx To 0 Step -1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
            If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                GVRole.DeleteRow(i)
            End If
        Next
        SPCVALS()
        AccIDEX.Focus()
        DVGFormat()
    End Sub
    Sub addCatToDVG()
        If AccIDEX.Text = "" Then
            AccIDEX.ErrorText = "يرجى اختيار الصنف أولاً"
            Exit Sub
        End If
        If ExpensVal.EditValue <= 0.000 Then
            ExpensVal.ErrorText = "القيمة لا يجب أن تكون صرف أو أقل"
            Exit Sub
        End If
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ExName", AccIDEX.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ExVal", ExpensVal.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "NotesDe", Notes2.Text.Trim)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccEX", AccIDEX.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ID", ExpenseID)
        DVGFormat()
        SPCVALS()
        AccIDEX.EditValue = -1
        ExpensVal.EditValue = 0.000
        AccIDEX.Focus()
    End Sub
    Sub SPCVALS()
        OverAllExpens.EditValue = SettlementVal
        If PCVal.EditValue > SettlementVal Then
            SurplusVal.EditValue = PCVal.EditValue - SettlementVal
            DeserevedVal.EditValue = 0.000
        ElseIf PCVal.EditValue < SettlementVal Then
            DeserevedVal.EditValue = SettlementVal - PCVal.EditValue
            SurplusVal.EditValue = 0.000
        Else
            SurplusVal.EditValue = 0.000
            DeserevedVal.EditValue = 0.000
        End If
    End Sub
    Sub DVGFormat()
        GVRole.AddNewRow()
        GVRole.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.True
        GVRole.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True
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
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
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
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)

        If e.Column.FieldName Is "ExVal" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ExVal"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(52, 69, 82)
                e.Appearance.BackColor2 = Color.FromArgb(52, 69, 82)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "Delete" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("Delete"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(232, Color.Aqua)
                e.Appearance.BackColor2 = Color.FromArgb(232, Color.Aqua)
            End If
        End If
    End Sub

    Private Sub AccIDEX_TextChanged(sender As Object, e As EventArgs) Handles AccIDEX.TextChanged
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ExName", SqlDbType.NVarChar, -1) With {.Value = AccIDEX.Text}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ExpensesTb_SelectID", PR)
        If dt.Rows.Count > 0 Then
            ExpenseID = dt.Rows(0)("ID")
        End If
        If IsUpdate = False Then
            Dim rowIdx As Integer = GVRole.DataRowCount - 1
            For i As Integer = rowIdx To 0 Step -1
                Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
                If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                    GVRole.DeleteRow(i)
                End If
            Next
        End If
    End Sub
    Private Sub GCRole_DoubleClick(sender As Object, e As EventArgs) Handles GCRole.DoubleClick
        If IsUpdate = False Then
            If GVRole.RowCount = 2 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                SPCVALS()
                AccIDEX.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub
    Private Sub Notes2_Leave(sender As Object, e As EventArgs) Handles Notes2.Leave
        addCatToDVG()
        Notes2.Text = ""
        AccIDEX.Focus()
    End Sub

    Private Sub GVRole_RowCountChanged(sender As Object, e As EventArgs) Handles GVRole.RowCountChanged
        For i As Integer = 0 To GVRole.RowCount - 1
            GVRole.SetRowCellValue(i, "SN", i + 1)
        Next
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWPCSETTLEMENT.ShowDialog()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub FRMPettyCashSettlement_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        If BranchID.EditValue <> -1 Or BranchID.Text <> "" Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasVal", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.PopulateColumns()
                SafeID.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub

    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub AccIDEX_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccIDEX.QueryPopUp
        If ExType.Text <> "" Or ExType.SelectedIndex <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.PopulateColumns()
                AccIDEX.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            LOADSafeID()
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONPETTYCASHACCIDHASVAL", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "EMPNAME"
                GVLKP.Columns("AccID").Visible = False
                NEWDVGFROMAT(GVLKP)
            End If
        Else
            EMPID.EditValue = -1
            EMPID.Properties.DataSource = Nothing
            EMPID.Enabled = False
            EMPID.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.Text <> String.Empty Then
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_QUERY_ONLY("SELECT ID FROM EmployeeTb where EMPNAME='" & EMPID.Text.Trim & "'")
            If DT.Rows.Count > 0 Then
                EMID = DT.Rows(0)("ID")
            End If
        End If
        If IsUpdate = False Then
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = CLSPC.PettyCash_SelectMax(BranchID.EditValue, EMID)
            CodeID = DTT.Rows(0)("ID")
            Code.Text = DTT.Rows(0)("Code")
        End If
    End Sub
    Private Sub FRMPettyCashSettlement_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub GCRole_Click(sender As Object, e As EventArgs) Handles GCRole.Click
        If IsUpdate = False Then
            If GVRole.RowCount > 0 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                AccIDEX.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        ExType.SelectedIndex = -1
        AccIDEX.EditValue = -1
        AccIDEX.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            LOADSafeID()
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONPETTYCASHACCIDHASVAL", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "EMPNAME"
                GVLKP.Columns("AccID").Visible = False
                NEWDVGFROMAT(GVLKP)
            End If
        Else
            EMPID.EditValue = -1
            EMPID.Properties.DataSource = Nothing
            EMPID.Enabled = False
            EMPID.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub ExType_TextChanged(sender As Object, e As EventArgs) Handles ExType.TextChanged
        If BranchID.Text = "" Or BranchID.EditValue = -1 Then
            ErrorMessage(Me, "رسالة خطأ", "يجب اختيار الفرع أولا ثم نوع الحساب")
            ExType.SelectedIndex = -1
            Exit Sub
        End If
        LOADEXPANSTYPE()
    End Sub
End Class
Public Class EntryPCSETTLEMENT
    Public Property SN() As Integer
    Public Property ExName() As String
    Public Property ExVal() As Decimal
    Public Property AccEX() As ULong
    Public Property ID() As ULong
    Public Property NotesDe() As String
End Class