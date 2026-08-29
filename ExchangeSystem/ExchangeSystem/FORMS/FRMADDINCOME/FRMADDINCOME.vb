Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting
Imports System.IO

Public Class FRMADDINCOME
    Dim clsempwd As New CLSADINCOME
    Public IDCode As ULong, TYPEs As Integer
    Public LOADTYPE, EMPID As Integer
    Public IsUpdate, CanChangeSafe As Boolean
    Public Frmid As Integer
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(Frmid, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

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

    Sub DISAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        WDValue.Enabled = False
        WithdrawalFrom.Enabled = False
        WithdrawalValue.Enabled = False
        Notes.Enabled = False
        PaidFor.Enabled = False
        IDNo.Enabled = False
        Phone.Enabled = False
    End Sub
    Sub ENAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalValue.Enabled = False
        WithdrawalDate.Enabled = False
        WDValue.Enabled = True
        WithdrawalFrom.Enabled = True
        If CanChangeSafe = True Then
            SafeID.Enabled = True
        Else
            SafeID.Enabled = False
        End If
        Notes.Enabled = True
        PaidFor.Enabled = True
        IDNo.Enabled = True
        Phone.Enabled = True
    End Sub
    Sub NEWRECORD()
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(LOADTYPE)
        IsUpdate = False
        ENAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        WithdrawalDate.EditValue = Date.Now
        BranchID.EditValue = -1
        LOADBRNCHDIERCT(BranchID)
        BranchID.Select()
        WithdrawalFrom.EditValue = -1
        BranchID.EditValue = BID
        BtnSave.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        WithdrawalValue.Text = ""
        Notes.Text = ""
        WDValue.Text = ""
        PaidFor.Text = ""
        Phone.Text = ""
        IDNo.Text = ""
        LOADCIDFROM(TYPEs)
        If TYPEs = 1 Then
            CurrencyFrom.EditValue = 1
            CurrencyFrom.Enabled = False
        Else
            CurrencyFrom.EditValue = -1
            CurrencyFrom.Enabled = True
        End If
        If LOADTYPE = 6 Or LOADTYPE = 8 Or LOADTYPE = 32 Or LOADTYPE = 33 Or LOADTYPE = 28 Or LOADTYPE = 29 Then
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        Else
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        If LOADTYPE = 35 Then
            LayoutControlItem12.Text = "صرف لصالح"
        ElseIf LOADTYPE = 34 Then
            LayoutControlItem12.Text = "قبض من"
        End If
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, Frmid)

    End Sub
    'Sub LOADBRANCH()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
    '    If dt.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = dt
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.ShowHeader = False
    '    End If
    'End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Sub LOADSafeID()
        SafeID.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@TYPEs", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasValORNOT", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.DataSource = dt
                SafeID.Properties.ValueMember = "AccID"
                SafeID.Properties.DisplayMember = "UNAME"
                SafeID.Properties.KeyMember = BranchID.EditValue
                SafeID.Properties.ShowHeader = False
            End If
        End If
    End Sub
    Private Sub SafeID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SafeID.QueryPopUp
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@TYPEs", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasValORNOT", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.PopulateColumns()
                SafeID.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub
    Private Sub FRMEMPWITHDRAWAL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'If LOADTYPE = 35 Then
        '    GETSAFEVAL(UserAccID, BID, DefaultCurrnecy)
        '    If SAFEVAL <= 0 Then
        '        ErrorMessage(Me, "رسالة تنبيه", "عذرا لا يمكن فتح هذه الشاشة لعدم وجود رصيد في الخزنة")
        '        Me.Close()
        '        Exit Sub
        '    End If
        'End If
        lodePreportes()
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, Frmid)
        NEWRECORD()
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        Try
            WithdrawalFrom.Properties.DataSource = Nothing
            WithdrawalFrom.EditValue = -1
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
                SafeID.EditValue = -1
                LOADSafeID()
                SafeID.EditValue = UserAccID
                If TYPEs = 1 Then
                    CurrencyFrom.EditValue = 1
                    CurrencyFrom.Enabled = False

                Else
                    CurrencyFrom.EditValue = 2
                    CurrencyFrom.Enabled = True
                End If

                Dim PR(4) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
                PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
                PR(3) = New SqlParameter("@crunsfrom", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
                PR(4) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("EXCHANGESYS2024EMPORCUST_LOADINTOLKPBASEDONACCID", PR)
                If dt.Rows.Count > 0 Then
                    WithdrawalFrom.Properties.DataSource = dt
                    WithdrawalFrom.Properties.ValueMember = "AccID"
                    WithdrawalFrom.Properties.DisplayMember = "EMCUST"
                Else
                    WithdrawalFrom.Properties.DataSource = dt
                End If
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Sub LOADCUSTORSAFE(BranchIDs As Integer)
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            SafeID.EditValue = -1
            LOADSafeID()
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchIDs}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PR(3) = New SqlParameter("@crunsfrom", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            PR(4) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EXCHANGESYS2024EMPORCUST_LOADINTOLKPBASEDONACCID", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "EMCUST"
            Else
                WithdrawalFrom.Properties.DataSource = dt
            End If
        End If
    End Sub
    Private Sub WithdrawalFrom_TextChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.TextChanged
        If BranchID.EditValue = -1 Or BranchID.Text = "" Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = "" Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If CurrencyFrom.Text = String.Empty Or CurrencyFrom.EditValue = -1 Then

            CurrencyFrom.ErrorText = "الرجاء اختيار العملة"

            CurrencyFrom.Select()
            Return
        End If
        WithdrawalValue.Text = ""
        If BranchID.EditValue = -1 Or BranchID.Text = "" Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = "" Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If CurrencyFrom.EditValue = -1 Then
            CurrencyFrom.ErrorText = "الرجاء اختيار العملة "
            CurrencyFrom.Select()
            Exit Sub
        End If
        GETVALTOLTElCASHONLY()
    End Sub
    Sub LOADCIDFROM(TYPE As Integer)
        Dim DT As New DataTable
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = TYPE}
        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DVGFormat(GVROLE)
        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "ID"
            CurrencyFrom.Properties.DisplayMember = "CuName"
        Else
            CurrencyFrom.Properties.DataSource = Nothing
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
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Dim mms As String

    Sub SHOW_EMCUSCODE(x, s)
        Try
            If Me.IsUpdate = True Then
                LOADSafeID()
                Dim DT As New DataTable
                DT.Clear()
                DT = clsempwd.SERACH_EMPORCUSTWITHDRAWALTB(x, s)
                If DT.Rows.Count > 0 Then
                    WDCode.Text = DT.Rows(0)("Code").ToString
                    BranchID.EditValue = DT.Rows(0)("BranchID")
                    LOADCUSTORSAFE(BranchID.EditValue)
                    SafeID.EditValue = Convert.ToUInt64(DT.Rows(0)("SafeID"))
                    WithdrawalDate.EditValue = DT.Rows(0)("InsertDate")
                    WDValue.Text = DT.Rows(0)("WDVAL")
                    WithdrawalFrom.EditValue = DT.Rows(0)("EMPID")
                    CurrencyFrom.EditValue = DT.Rows(0)("CurrencyFrom")
                    Notes.EditValue = DT.Rows(0)("Notes")
                    PaidFor.Text = DT.Rows(0)("PaidFor")
                    IDNo.Text = DT.Rows(0)("IDNo")
                    Phone.Text = DT.Rows(0)("Phone")
                    GETVALTOLTElCASHONLY()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub SetData()




        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الحساب"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If WDValue.Text = "" Then
            WDValue.ErrorText = "يجب إدخال قيمة السحب"
            Exit Sub
        End If
        If LOADTYPE = 36 Or LOADTYPE = 37 Then
            If WDValue.EditValue > Val(WithdrawalValue.Text) Then
                WDValue.ErrorText = "القيمة لا يجب أن تكون أكبر من قيمة الحساب"
                Exit Sub
            End If
            If Val(WithdrawalValue.Text) <= 0.000 Then
                WithdrawalValue.ErrorText = "الحساب لا يوجد به قيمة كافية"
                Exit Sub
            End If
        End If
        'If LOADTYPE = 29 Then
        '    If WDValue.EditValue > Val(WithdrawalValue.Text) Then
        '        WDValue.ErrorText = "القيمة لا يجب أن تكون أكبر من قيمة الحساب"
        '        Exit Sub
        '    End If
        '    If Val(WithdrawalValue.Text) <= 0.000 Then
        '        WithdrawalValue.ErrorText = "الحساب لا يوجد به قيمة كافية"
        '        Exit Sub
        '    End If
        'End If
        'If LOADTYPE = 35 Then
        '    If WDValue.EditValue > Val(WithdrawalValue.Text) Then
        '        WDValue.ErrorText = "القيمة لا يجب أن تكون أكبر من قيمة الحساب"
        '        Exit Sub
        '    End If
        'If Val(WithdrawalValue.Text) <= 0.000 Then
        '        WithdrawalValue.ErrorText = "الحساب لا يوجد به قيمة كافية"
        '        Exit Sub
        '    End If
        'End If
        'If LOADTYPE = 35 Then
        '    GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, CurrencyFrom.EditValue)
        '    If SAFEVAL < WDValue.EditValue Then
        '        ErrorMessage(Me, "رسالة تنبيه", "رصيد الخزنة غير كافي الرجاء التأكد من رصيد الخزنة")
        '        Exit Sub
        ''    End If
        'End If

        Dim OPTYPE As Integer
        If LOADTYPE = 34 Then
            OPTYPE = 65
        ElseIf LOADTYPE = 35 Then
            OPTYPE = 65
        ElseIf LOADTYPE = 36 Then
            OPTYPE = 66
        ElseIf LOADTYPE = 37 Then
            OPTYPE = 66
        End If
        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""
        If LOADTYPE = 34 Or LOADTYPE = 35 Then

            MOTYPE = "إيداع في حساب إيراد"
            MOTYPE2 = "إيداع في حساب" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, WithdrawalDate.EditValue, EMPID, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode,
                                          BranchID.EditValue, 0.000, Notes.Text.Trim, IsUpdate, OPTYPE, WithdrawalFrom.EditValue,
                                          SafeID.EditValue, MOTYPE, MOTYPE2, UserID, CurrencyFrom.EditValue,
                                          PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        ElseIf LOADTYPE = 36 Or LOADTYPE = 37 Then
            MOTYPE = "صرف من حساب"
            MOTYPE2 = "صرف من حساب" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, WithdrawalDate.EditValue, EMPID, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode,
                                          BranchID.EditValue, 0.000, Notes.Text.Trim, IsUpdate, OPTYPE, WithdrawalFrom.EditValue,
                                          SafeID.EditValue, MOTYPE, MOTYPE2, UserID, CurrencyFrom.EditValue,
                                          PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        Else
            ErrorMessage(Me, "تنبية", "عذرا رقم الكود غير صحيح الرجاء اعادة المحاولة")
        End If

        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        Try


            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = WDCode.Text}
            prm(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ZRPT_EMPORCUSTWITHDRAWALTB_SelectByCode", prm)


            If dt.Rows.Count Then
                Dim report As New RPTEMPWITHDRAWAL2
                report.DataSource = dt

                report.DataMember = "EMPORCUSTWITHDRAWALTB"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()

                report.XrLabel2.Text = Me.CurrencyFrom.Text
                report.XrLabel93.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.Text, True, "n2")
                report.XrLabel25.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.Text, False, "n2")
                ''  report.XrLabel25.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.Text, False, False)
                report.ShowPreview()


                Dim pdfOptions As ImageExportOptions = report.ExportOptions.Image

                Dim dtmobyl As New DataTable
                dtmobyl.Clear()

                Dim stordpath As String
                stordpath = Application.StartupPath & "\TEMPWATS"
                Directory.CreateDirectory(stordpath)
                Dim newfilepathe As String
                newfilepathe = stordpath & "\" & "watsappmassg.jpeg"

                report.ExportToImage(newfilepathe, pdfOptions)
                If LOADTYPE = 34 Then
                    SINTWATSAPP_PDF_CLINT("120363175442297756@g.us", newfilepathe, "", "", "")

                ElseIf LOADTYPE = 35 Then
                    SINTWATSAPP_PDF_CLINT("0925093709", newfilepathe, "", "", "")

                End If
            End If
            If SQLCON.State = ConnectionState.Open Then
                SQLCON.Close()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Public Overrides Sub UPDATERECORD()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الحساب"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If WDValue.Text = "" Then
            WDValue.ErrorText = "يجب إدخال قيمة السحب"
            Exit Sub
        End If
        Dim OPTYPE As Integer = 0
        If LOADTYPE = 5 Then
            OPTYPE = 38
        ElseIf LOADTYPE = 6 Then
            OPTYPE = 39
        ElseIf LOADTYPE = 7 Then
            OPTYPE = 40
        ElseIf LOADTYPE = 8 Then
            OPTYPE = 41
        ElseIf LOADTYPE = 28 Then
            OPTYPE = 56
        End If
        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""
        If LOADTYPE = 5 Then
            MOTYPE = "معالجة خطأ سحب من حساب موظف"
            MOTYPE2 = "معالجة خطأ سحب من حساب الموظف" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, WithdrawalDate.EditValue, EMPID, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
        Notes.Text.Trim, IsUpdate, OPTYPE, WithdrawalFrom.EditValue, SafeID.EditValue, MOTYPE, MOTYPE2, UserID, CurrencyFrom.EditValue, PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        ElseIf LOADTYPE = 6 Then
            MOTYPE = "معالجة خطأ سحب من حساب عميل"
            MOTYPE2 = "معالجة خطأ سحب من حساب العميل" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, WithdrawalDate.EditValue, EMPID, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
        Notes.Text.Trim, IsUpdate, OPTYPE, WithdrawalFrom.EditValue, SafeID.EditValue, MOTYPE, MOTYPE2, UserID, CurrencyFrom.EditValue, PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        ElseIf LOADTYPE = 7 Then
            If TYPEs = 1 Then
                If SAFEVAL < Val(WDValue.Text) Then
                    InfoMessage(Me, "رسالة معلومات", "الخزنة لا يوجد بها رصيد كافٍ لاتمام عملية الترجيع")
                    Return
                End If
            End If
            MOTYPE = "معالجة خطأ إيداع في حساب موظف"
            MOTYPE2 = "معالجة خطأ إيداع في حساب الموظف" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, WithdrawalDate.EditValue, EMPID, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, OPTYPE, WithdrawalFrom.EditValue, SafeID.EditValue, MOTYPE, MOTYPE2, UserID, CurrencyFrom.EditValue,
                                                  PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        ElseIf LOADTYPE = 8 Then
            If TYPEs = 1 Then
                If SAFEVAL < Val(WDValue.Text) Then
                    InfoMessage(Me, "رسالة معلومات", "الخزنة لا يوجد بها رصيد كافٍ لاتمام عملية الترجيع")
                    Return
                End If
            End If
            MOTYPE = "معالجة خطأ إيداع في حساب عميل"
            MOTYPE2 = "معالجة خطأ إيداع في حساب العميل" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, WithdrawalDate.EditValue, EMPID, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                              Notes.Text.Trim, IsUpdate, OPTYPE, WithdrawalFrom.EditValue, SafeID.EditValue, MOTYPE, MOTYPE2, UserID, CurrencyFrom.EditValue,
                                              PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Try
            NEWRECORD()
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "الرجاء اختيار الحقل "
                Return
            End If
            FRMVIEWADDINCOME.GCRole.DataSource = Nothing
            FRMVIEWADDINCOME.GVRole.Columns.Clear()
            FRMVIEWADDINCOME.LoadData(1, BranchID.EditValue, LOADTYPE)
            FRMVIEWADDINCOME.ShowDialog()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Private Sub WithdrawalFrom_EditValueChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.EditValueChanged
        EMPID = GridLookUpEdit1View.GetFocusedRowCellValue("ID_LOCK")
    End Sub
    Private Sub CurrencyFrom_EditValueChanged(sender As Object, e As EventArgs) Handles CurrencyFrom.EditValueChanged
        WithdrawalValue.Text = ""
        If BranchID.EditValue = -1 Or BranchID.Text = "" Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Or WithdrawalFrom.Text = "" Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الحساب"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = "" Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        EMPID = GridLookUpEdit1View.GetFocusedRowCellValue("ID_LOCK")
        'GETVALTOLTElCASHONLY()
    End Sub

    Private Sub FRMEMPWITHDRAWAL_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub



    Private Sub WithdrawalFrom_Leave(sender As Object, e As EventArgs) Handles WithdrawalFrom.Leave
        EMPID = GridLookUpEdit1View.GetFocusedRowCellValue("ID_LOCK")
    End Sub

    Public Sub GETVALTOLTEl()
        Try
            If WithdrawalFrom.Text = String.Empty Or WithdrawalFrom.EditValue = -1 Then
                WithdrawalFrom.ErrorText = "الرجاء اختيار الحساب"
                WithdrawalFrom.Select()
                Return
            End If
            If CurrencyFrom.Text = String.Empty Or CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "الرجاء اختيار العملة "
                CurrencyFrom.Select()
                Return
            End If
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = WithdrawalFrom.EditValue}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("[EMPORCUST_GetAccVal](@AccName,@crunseType) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.Text = dt.Rows(0)("GetAccVal")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Public Sub GETVALTOLTElCASHONLY()
        Try
            If WithdrawalFrom.Text = String.Empty Or WithdrawalFrom.EditValue = -1 Then
                WithdrawalFrom.ErrorText = "الرجاء اختيار الحساب"
                WithdrawalFrom.Select()
                Return
            End If
            If CurrencyFrom.Text = String.Empty Or CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "الرجاء اختيار العملة "
                CurrencyFrom.Select()
                Return
            End If
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = WithdrawalFrom.EditValue}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            PR(2) = New SqlParameter("@LoadType", SqlDbType.Int) With {.Value = LOADTYPE}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("[EMPORCUST_GetAccValCashOnly](@AccName,@crunseType,@LoadType) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.Text = dt.Rows(0)("GetAccVal")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub

End Class
