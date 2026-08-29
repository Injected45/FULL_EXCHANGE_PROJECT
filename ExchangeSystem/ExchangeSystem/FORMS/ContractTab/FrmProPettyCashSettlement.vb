Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SelectPdf
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FrmProPettyCashSettlement
    Public StID, AccLine, AccCat, EMID As Integer
    Public IsUpdate, UpdateBySalary As Boolean
    Public SettlementVal As Decimal
    Public AcID, IDCode, AccCode, AccEm, CodeID, ExpenseID, AccIDPetty As ULong
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
    Sub LOADPROJECT()
        If BranchID.EditValue <> -1 Or BranchID.Text <> "" Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CONDB_LoadToPcStelment_BasedOnBranch", PR)
            If DT.Rows.Count > 0 Then
                ProjectID.Properties.DataSource = DT
                ProjectID.Properties.ValueMember = "StockAccID"
                ProjectID.Properties.DisplayMember = "ProName"
                ProjectID.Properties.ShowHeader = False
                ProjectID.Properties.PopulateColumns()
                ProjectID.Properties.Columns("StockAccID").Visible = False
                ProjectID.Properties.Columns("ID").Visible = False
            End If
        End If
    End Sub
    Sub NEWRECORD()
        Code.Enabled = False
        InsertDate.EditValue = Date.Now
        ISID.Text = ""
        BranchID.EditValue = -1
        LOADBRANCH()
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
        EXQTN.EditValue = 1
        OverAllPrice.EditValue = 0.000

        Dim bindlis As List(Of EntryPROPCSETTLEMENT) = New List(Of EntryPROPCSETTLEMENT)
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
        ProjectID.EditValue = -1
        PaidVal.EditValue = 0.000
        AssestVal.EditValue = 0.000
        AssestPaidVal.EditValue = 0.000
        SettlementType.SelectedIndex = -1
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 140)
    End Sub
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
        dt.Columns.Add("ColQTN")
        dt.Columns.Add("ColOverAllTotal")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "ExName")
            Dim MOVTYPE As String = "مقابل مصروفات لحساب" & Space(1) & CellValue
            If CellValue <> String.Empty Then
                dt.Rows.Add(GVRole.GetRowCellValue(i, "ExVal"), GVRole.GetRowCellValue(i, "AccEX"),
                            GVRole.GetRowCellValue(i, "ID"), GVRole.GetRowCellValue(i, "NotesDe").ToString, GVRole.GetRowCellValue(i, "ColQTN"), GVRole.GetRowCellValue(i, "ColOverAllTotal"))
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
            If SettlementType.SelectedIndex = 1 Then
                If OverAllExpens.EditValue > PCVal.EditValue Then
                    ErrorMessage(Me, "رسالة خطأ", "القيمة المدفوعة أكبر من قيمة الحساب")
                    Return
                End If
                Return
            End If
            dt = PCSettlementTB_CHECKCODE(Code.Text.Trim)
            'For i As Integer = 0 To GVRole.RowCount - 1

            PCSettlement_insert(Code.Text.Trim, InsertDate.EditValue, EMPID.EditValue, BranchID.EditValue, UserID, CurrencyID.EditValue, ISID.Text.Trim, PCVal.EditValue,
                                                  SettlementVal, Notes.Text.Trim, CodeID, SafeID.EditValue, EMPID.EditValue, IsUpdate, 1, 0, "", EMPID.Text, GCRole_seteing(),
                                                  SettlementType.SelectedIndex, EMPID.EditValue)
            ''Next
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Function PCSettlementTB_CHECKCODE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[CONDB_PCSettlementTB_CHECKCODE]", PRM)
        Return DT
    End Function
    Public Sub PCSettlement_insert(Code As String, InsertDate As Date, EMPID As Integer, BranchID As Integer, SafeID As Integer, CurrencyID As Integer, ISID As String, PCVal As Decimal,
                                   SettlementVal As Decimal, Notes As String, IDCode As ULong,
                                   AccIDSafeID As ULong, AccIDPetty As ULong, IsUpdate As Boolean, ExpensVal As Decimal, AccIDEX As ULong, NotesDe As String, EMPNAME As String,
                                   dt As DataTable, SettlementType As Integer, ContractorAccID As ULong)
        Try
            Dim prm(20) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (300)) With {.Value = Code}
            prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            prm(2) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            prm(4) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            prm(5) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            prm(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, 300) With {.Value = ISID}
            prm(7) = New SqlParameter("@PCVal", SqlDbType.Decimal) With {.Value = PCVal}
            prm(8) = New SqlParameter("@SettlementVal", SqlDbType.Decimal) With {.Value = SettlementVal}
            prm(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            prm(10) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = IDCode}
            prm(11) = New SqlParameter("@AccIDSafeID", SqlDbType.BigInt) With {.Value = AccIDSafeID}
            prm(12) = New SqlParameter("@AccIDPetty", SqlDbType.BigInt) With {.Value = AccIDPetty}
            prm(13) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            prm(14) = New SqlParameter("@Type", SqlDbType.Structured) With {.Value = dt}
            prm(15) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, -1) With {.Value = EMPNAME}
            prm(16) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(17) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(18) = New SqlParameter("@ExType", SqlDbType.Int) With {.Value = ExType.SelectedIndex}
            prm(19) = New SqlParameter("@SettlementType", SqlDbType.TinyInt) With {.Value = SettlementType}
            prm(20) = New SqlParameter("@ContractorAccID", SqlDbType.BigInt) With {.Value = ContractorAccID}
            RUN_EXUTE_PRO("CONDB_PCSettlementTB_Insert", prm)

            If prm(16).Value = 0 Then
                'MessageBox.Show(prm(15).Value, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ErrorMessage(Me, "رسالة تنبيه", prm(17).Value)
                If Me.IsUpdate = False Then
                    If (Me.BranchID.EditValue <> -1 Or Me.BranchID.Text <> String.Empty) And (Me.EMPID.EditValue <> -1 Or Me.EMPID.Text <> String.Empty) Then
                        Me.EMPID_TextChanged(Nothing, Nothing)
                        Exit Sub
                    End If
                End If

            End If
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                Me.Print()
                FrmSavedSuccessfully.Show()
                Me.NEWRECORD()
            Else
                FrmSavedSuccessfully.Show()
                Me.NEWRECORD()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Dim report As New RPTPettyCashSettlement
    Private pdfExportFile As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\" & report.Name & ".pdf"

    ' Specify PDF export options
    Private PdfExportOptions As New PdfExportOptions() With {.PdfACompatibility = PdfCompressionLevel.Normal}
    Public Overrides Sub Print()
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("CONDB_ZRPT_PCSettlementTB_SELECTByCODE", PRM)
            Dim ds As New DataSet
            dt.TableName = "PCSettlementTB"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                        Dim report As New RPTProPettyCashSettlement
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
                        report.ExportToPdf(pdfExportFile, PdfExportOptions)
                        report.ShowPreview()

                        If IsUpdate = 0 Then
                            ' ارسال التقرير في صورة بي دي اف عبر تطبيق واتساب
                            SINTWATSAPP_document(GET_PHONE_SaenFroWtsaap(EMPID.EditValue), pdfExportFile, $"تسوية عهدة {EMPID.Text} ", " تسوية عهدة" & ".pdf")
                        End If
                    End If
        Catch ex As Exception
            ErrorMessage(Me, "رساله تنبية ", ex.Message)
        End Try
    End Sub
    Public Function SERACH_PCST(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[CONDB_PCSettlementTB_SELECTByCODE]", PRM)
        Return DT
    End Function
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
            DT = SERACH_PCST(x)
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
                EMPID.EditValue = DT.Rows(0)("EMPID")
                SettlementVal = DT.Rows(0)("SettlementVal")
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = x}
                Dim DTT As New DataTable
                DTT.Clear()
                DTT = RUN_QUARY_PRO("CONDB_PCSettlementDetailsTB_SELECTByCODE", PR)
                If DTT.Rows.Count > 0 Then
                    GCRole.DataSource = DTT
                End If
            End If
        End If
    End Sub
    'Public Overrides Sub UPDATERECORD()
    '    If IsUpdate = True Then
    '        Dim dt As New DataTable
    '        dt.Clear()
    '        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
    '            BranchID.ErrorText = "يرجى اختيار الفرع"
    '            Return
    '        End If
    '        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
    '            SafeID.ErrorText = "يرجى اختيار الخزنة"
    '            Return
    '        End If
    '        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
    '            CurrencyID.ErrorText = "يرجى اختيار العملة"
    '            Return
    '        End If
    '        If ISID.Text.Trim = String.Empty Then
    '            ISID.ErrorText = "يرجى اختيار العهدة"
    '            Return
    '        End If
    '        If PCVal.EditValue = 0.000 Then
    '            PCVal.ErrorText = "القيمة يجب أن لا تساوي صفر أو أٌل"
    '            Return
    '        End If
    '        If GVRole.RowCount = 0 Then
    '            ErrorMessage(Me, "رسالة خطأ", "يجب اختيار مصروف واحد على الأقل")
    '            Exit Sub
    '        End If
    '        dt = CLSPC.PCSettlementTB_CHECKCODE(Code.Text.Trim)

    '        CLSPC.PCSettlement_insert(Code.Text.Trim, InsertDate.EditValue, EMPID.EditValue, BranchID.EditValue, UserID, CurrencyID.EditValue, ISID.Text.Trim, PCVal.EditValue,
    '                                      SettlementVal, Notes.Text.Trim, CodeID, SafeID.EditValue, EMPID.EditValue, IsUpdate, 1, 0, "", EMPID.Text, GCRole_seteing())
    '    End If
    '    NEWRECORD()
    '    MyBase.UPDATERECORD()
    'End Sub
#End Region
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            SafeID.Enabled = dt.Rows(0)("Can_safID")
            SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            SafeID.Enabled = False
            SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub
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
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If SettlementType.SelectedIndex = -1 Then
            SettlementType.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = ExType.SelectedIndex}
            PR(2) = New SqlParameter("@TypeEx", SqlDbType.TinyInt) With {.Value = SettlementType.SelectedIndex}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[CONDB_ExpensesTb_LoadToLKP]", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.DataSource = dt
                AccIDEX.Properties.ValueMember = "AccID"
                AccIDEX.Properties.DisplayMember = "ExName"
                AccIDEX.Properties.ShowHeader = False
                AccIDEX.Properties.PopulateColumns()
                AccIDEX.Properties.Columns("AccID").Visible = False
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
        ProjectID.Enabled = True
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
        ProjectID.Enabled = False
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
        If EMPID.Text = String.Empty Then
            EMPID.ErrorText = "يجب اختيار الموظف"
            Return
        End If
        FRMVIEWPROPCTOSETTLEMENT.LoadData()
        FRMVIEWPROPCTOSETTLEMENT.ShowDialog()
    End Sub
    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        Dim sumUnit As Integer = 0
        Dim sumTotal As Double = 0.000
        If IsUpdate = 0 Then
            GVRole.Columns("ExName").OptionsColumn.AllowEdit = Not e.FocusedRowHandle = 0
            SettlementVal = Convert.ToDecimal(GVRole.Columns("ColOverAllTotal").SummaryItem.SummaryValue)
        End If
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
    Sub GETTOTAL()
        Dim sumUnit As Integer = 0
        Dim sumTotal As Double = 0.000
        OverAllPrice.EditValue = Math.Floor(ExpensVal.EditValue * EXQTN.EditValue)
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
        If ExType.SelectedIndex = 0 Then
            AccIDPetty = ProjectID.EditValue
        ElseIf ExType.SelectedIndex = 1 Then
            AccIDPetty = AccIDEX.EditValue
        End If
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ExName", AccIDEX.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ExVal", ExpensVal.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ColQTN", EXQTN.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ColOverAllTotal", Convert.ToDecimal(OverAllPrice.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "NotesDe", Notes2.Text.Trim)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccEX", AccIDPetty)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ID", ExpenseID)
        DVGFormat()
        SPCVALS()
        AccIDEX.EditValue = -1
        ExpensVal.EditValue = 0.000
        EXQTN.EditValue = 1
        OverAllPrice.EditValue = 0.000
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
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@ExName", SqlDbType.NVarChar, -1) With {.Value = AccIDEX.Text}
        PR(1) = New SqlParameter("@ExType", SqlDbType.Int) With {.Value = ExType.SelectedIndex}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_ExpensesTb_SelectID", PR)
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
        If ExType.SelectedIndex = 1 Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccIDEX.EditValue}
            dt.Clear()
            dt = RUN_QUARY_PRO("CONDB_AddAssestTb_CheckPaidValue", PRM)
            If dt.Rows.Count > 0 Then
                AssestVal.EditValue = dt.Rows(0)("AssestVal")
                AssestPaidVal.EditValue = dt.Rows(0)("PaidVal")
            End If
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

    Private Sub ExpensVal_Leave(sender As Object, e As EventArgs) Handles ExpensVal.Leave
        If ExType.SelectedIndex = 1 Then
            Try

                Dim PRM(3) As SqlParameter
                PRM(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccIDEX.EditValue}
                PRM(1) = New SqlParameter("@AssestVal", SqlDbType.Decimal) With {.Value = ExpensVal.EditValue}
                PRM(2) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                PRM(3) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("CONDB_Assest_CheckValue", PRM)
                If PRM(2).Value = 0 Then
                    ErrorMessage(Me, "رسالة خطأ", PRM(3).Value.ToString)
                    ExpensVal.EditValue = 0.000
                    ExpensVal.Focus()
                    Exit Sub
                End If
            Catch ex As Exception
                ErrorMessage(Me, "رسالة خطأ", ex.Message)
            End Try
        End If
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

    Private Sub SettlementType_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles SettlementType.ButtonClick
        EMPID.EditValue = -1
    End Sub

    Private Sub AccIDEX_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccIDEX.QueryPopUp
        'If ExType.Text <> "" Or ExType.SelectedIndex <> -1 And SettlementType.SelectedIndex <> -1 Or SettlementType.Text <> String.Empty Then
        '    Dim PR(2) As SqlParameter
        '    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        '    PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = ExType.SelectedIndex}
        '    PR(2) = New SqlParameter("@TypeEx", SqlDbType.TinyInt) With {.Value = SettlementType.SelectedIndex}
        '    Dim dt As New DataTable
        '    dt.Clear()

        '    dt = RUN_QUARY_PRO("[CONDB_ExpensesTb_LoadToLKP]", PR)
        '    If dt.Rows.Count > 0 Then
        '        AccIDEX.Properties.PopulateColumns()
        '        AccIDEX.Properties.Columns("AccID").Visible = False
        '    Else
        '        AccIDEX.Properties.DataSource = Nothing
        '    End If
        'End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            LOADSafeID()
            LOADPROJECT()

        End If
    End Sub
    Private Function PettyCash_SelectMax(BranchID As Integer, EMPID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int)
        PRM(1).Value = EMPID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_PCSettlementTB_MaxID", PRM)
        Return DT
    End Function
    Public Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        Dim DT As New DataTable
        If EMPID.Text <> String.Empty Then
            DT.Clear()
            DT = RUN_QUARY_QUERY_ONLY("SELECT ID FROM ContractDB.dbo.ContractorTb where CustName='" & EMPID.Text.Trim & "'")
            If DT.Rows.Count > 0 Then
                EMID = DT.Rows(0)("ID")
            End If
            If SettlementType.SelectedIndex <> -1 Then
                If SettlementType.SelectedIndex = 1 Then
                    Dim PR(1) As SqlParameter
                    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                    PR(1) = New SqlParameter("@SettlementType", SqlDbType.Bit) With {.Value = SettlementType.SelectedIndex}
                    DT.Clear()
                    DT = RUN_QUARY_PRO("CONDB_AddPartnerTb_LOADINTOLKP", PR)
                    If DT.Rows.Count > 0 Then
                        PCVal.EditValue = DT.Rows(0)("TotalVal")
                    End If
                End If
            Else
                PCVal.EditValue = 0.000
            End If
        End If
        Dim CodeEmp As Integer
        If SettlementType.SelectedIndex = 1 Then
            CodeEmp = EMID
        ElseIf SettlementType.SelectedIndex = 0 Then
            CodeEmp = EMPID.EditValue
        End If
        If IsUpdate = False Then
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = PettyCash_SelectMax(BranchID.EditValue, CodeEmp)
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
            LOADPROJECT()
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CONDB_AddPartnerTb_LoadToLKP", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "PartnerName"
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

    Private Sub EXQTN_TextChanged(sender As Object, e As EventArgs) Handles EXQTN.TextChanged

        GETTOTAL()

    End Sub


    Private Sub ExpensVal_TextChanged(sender As Object, e As EventArgs) Handles ExpensVal.TextChanged
        GETTOTAL()
    End Sub

    Private Sub SettlementType_TextChanged(sender As Object, e As EventArgs) Handles SettlementType.TextChanged
        ExType.SelectedIndex = -1
        AccIDEX.EditValue = -1
        EMPID.EditValue = -1
        PCVal.EditValue = 0.000
        ISID.Text = ""
        If SettlementType.SelectedIndex <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@SettlementType", SqlDbType.Bit) With {.Value = SettlementType.SelectedIndex}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CONDB_AddPartnerTb_LOADINTOLKP", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "PartnerName"
                NEWDVGFROMAT(GVLKP)
            Else
                EMPID.EditValue = -1
                EMPID.Properties.DataSource = Nothing
            End If
        Else
            EMPID.EditValue = -1
            EMPID.Properties.DataSource = Nothing
            EMPID.Enabled = False
            EMPID.Properties.DataSource = Nothing
        End If
        Dim SettlmentCode As String = ""
        If SettlementType.SelectedIndex = 1 Then
            EMPID.Enabled = True
            ISID.Text = Code.Text
            BTNPCVIEW.Enabled = False
        Else
            BTNPCVIEW.Enabled = True
        End If
    End Sub

    Private Sub Code_TextChanged(sender As Object, e As EventArgs) Handles Code.TextChanged
        If SettlementType.SelectedIndex = 1 Then
            ISID.Text = Code.Text
        End If
    End Sub
End Class
Public Class EntryPROPCSETTLEMENT
    Public Property SN() As Integer
    Public Property ExName() As String
    Public Property ExVal() As Decimal
    Public Property AccEX() As ULong
    Public Property ColQTN() As Integer
    Public Property ColOverAllTotal() As Decimal
    Public Property ID() As ULong
    Public Property NotesDe() As String
End Class