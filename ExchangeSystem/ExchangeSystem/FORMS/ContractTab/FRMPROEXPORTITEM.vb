Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Public Class FRMPROEXPORTITEM
    Dim CLSPC As New CLSANOTHEREXPENS
    Public IsUpdate, UpdateBySalary, IsAseet As Boolean
    Public AcID, IDCode, AccCode, AccEm, CodeID, ExpenseID As ULong, ItemID As Integer
    Sub GETITMQT(ItemID As Integer, BillNo As String)
        'If IsUpdate = False Then
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@ItemID", SqlDbType.BigInt) With {.Value = ItemID}
        PR(1) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = BillNo}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("CONDDB_GETITEMQTBASEDBILLNO(@ItemID,@BillNo) AS AccVal", PR)
        If dt.Rows.Count > 0 Then
            ITMQT.EditValue = dt.Rows(0)("AccVal")
        End If
        'End If
    End Sub
    Sub NEWRECORD()
        ITMACCID.Properties.DataSource = Nothing
        Code.Enabled = False
        Code.Text = ""
        InsertDate.EditValue = Date.Now
        LOADBRANCH()
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnEdit.Caption = "إرجاع قيمة الصرف"
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        LOADRECURRENCY()
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        CurrencyID.Text = "دينار ليبي"
        CurrencyID.Enabled = False
        IsUpdate = False
        ENAPLEDCONTROLS()
        SafeID.EditValue = -1
        ITMQT.EditValue = 0.000
        ITMACCID.EditValue = -1
        ProjectID.EditValue = -1
        Notes.Text = ""
        UNTPRC.EditValue = 0
        ExpensVal.EditValue = 0
        OUTQT.EditValue = 0
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
        LOADBILLNO()
        BillNo.EditValue = -1
        BillNo.Text = String.Empty
        lodePreportes()
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 160)
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
    Sub ENAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = True
        SafeID.Enabled = True
        CurrencyID.Enabled = False
        Notes.Enabled = True
        InsertDate.Enabled = False
        ITMACCID.Enabled = True
        ITMQT.Enabled = True
        ProjectID.Enabled = True
        OUTQT.Enabled = True
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(47, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub DISAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = False
        SafeID.Enabled = False
        CurrencyID.Enabled = False
        Notes.Enabled = False
        InsertDate.Enabled = False
        ITMACCID.Enabled = False
        ITMQT.Enabled = False
        ProjectID.Enabled = False
        OUTQT.Enabled = False


    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            Dim dt As New DataTable
            dt.Clear()
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If ITMACCID.EditValue = -1 Or ITMACCID.Text = String.Empty Then
                ITMACCID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If ProjectID.EditValue = -1 Or ProjectID.Text = String.Empty Then
                ProjectID.ErrorText = "هذا الحقل مطلوب"
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
            If ITMACCID.EditValue = -1 Or ITMACCID.Text = String.Empty Then
                ITMACCID.ErrorText = "يرجى اختيار المصروف"
                Return
            End If
            If OUTQT.EditValue <= 0.000 Then
                OUTQT.ErrorText = "القيمة يجب أن لا تكون صفر أو أقل"
                Return
            End If
            'GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, CurrencyID.EditValue)
            'If SAFEVAL < ITMQT.EditValue Then
            '    ErrorMessage(Me, "رسالة تنبيه", "رصيد الخزنة غير كافي الرجاء التأكد من رصيد الخزنة")
            '    Exit Sub
            'End If
            Dim MOV As String = "مقابل صرف صنف" & Space(1) & ITMACCID.Text
            ANOTHEREXPENSTB_Insert()
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub


    Public Overrides Sub Print()

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
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("CONDB_ZRPT_PettyCashTb_LOADPROEXPORTITEM", PRM)
            Dim ds As New DataSet
            dt.TableName = "PROEXPORTITEM"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTPROEXPORTITEM
                report.DataSource = ds
                report.DataMember = "PROEXPORTITEM"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.XrLabel18.Text = Cur_Code(CurrencyID.Text, ExpensVal.EditValue, False)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub


    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            Dim dt As New DataTable
            dt.Clear()
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "يرجى اختيار الخزنة"
                Return
            End If
            If ITMACCID.EditValue = -1 Or ITMACCID.Text = String.Empty Then
                ITMACCID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If ProjectID.EditValue = -1 Or ProjectID.Text = String.Empty Then
                ProjectID.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If ITMACCID.EditValue = -1 Or ITMACCID.Text = String.Empty Then
                ITMACCID.ErrorText = "يرجى اختيار المصروف"
                Return
            End If
            If OUTQT.EditValue <= 0.000 Then
                OUTQT.ErrorText = "القيمة يجب أن لا تكون صفر أو أقل"
                Return
            End If
            Dim MOV As String = "مقابل مصروفات لحساب" & Space(1) & ITMACCID.Text
            ANOTHEREXPENSTB_Insert()
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_EMCUSCODE(x)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = SERACH_ANOTHEREXPENSTB(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                BranchID_TextChanged(Nothing, Nothing)
                CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
                SafeID.EditValue = DT.Rows(0)("SafeID")
                InsertDate.EditValue = DT.Rows(0)("InsertDate")
                Notes.Text = DT.Rows(0)("Notes").ToString
                ProjectID.EditValue = DT.Rows(0)("AccSafeID")
                ExpensVal.EditValue = DT.Rows(0)("ExpensVal")
                LOADBILLNO()
                BillNo.Text = DT.Rows(0)("BillNo")

                GETITMQT(DT.Rows(0)("AccIDPC"), BillNo.Text)
                'ITMQT.EditValue = DT.Rows(0)("ITMQT")
                ITMACCID.EditValue = DT.Rows(0)("AccIDPC")
                UNTPRC.EditValue = DT.Rows(0)("PRCAVG")
                OUTQT.EditValue = DT.Rows(0)("OUTQT")
            End If
        End If
    End Sub
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
        If BranchID.Text <> String.Empty Or BranchID.EditValue<>-1 Then
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
    Sub LOADPROJECTID()
        ProjectID.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[CONDB_ProjectTb_LoadBasedOnBranch]", PR)
            If dt.Rows.Count > 0 Then
                ProjectID.Properties.DataSource = dt
                ProjectID.Properties.ValueMember = "StockAccID"
                ProjectID.Properties.DisplayMember = "ProName"
                ProjectID.Properties.ShowHeader = False
                ProjectID.Properties.PopulateColumns()
                ProjectID.Properties.Columns("StockAccID").Visible = False
                ProjectID.Properties.Columns("ID").Visible = False
            End If
        Else
            ProjectID.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADEXPANSTYPE()
        'AccIDEX.Properties.DataSource = Nothing
        If BillNo.Text <> String.Empty Or BillNo.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = BillNo.Text}
            PR(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[CONDB_CategoriesTb_LoadIDACCIDTOEXPORT]", PR)
            If dt.Rows.Count > 0 Then
                ITMACCID.Properties.DataSource = dt
                ITMACCID.Properties.ValueMember = "AccID"
                ITMACCID.Properties.DisplayMember = "ItemName"
                ITMACCID.Properties.ShowHeader = False
                ITMACCID.Properties.PopulateColumns()
                ITMACCID.Properties.Columns("ID").Visible = False
                ITMACCID.Properties.Columns("AccID").Visible = False
            End If
        Else
            ITMACCID.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADBILLNO()
        'AccIDEX.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1}
            PR(1) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = ""}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[CONDB_CategoriesTb_LoadIDACCIDTOEXPORT]", PR)
            If dt.Rows.Count > 0 Then
                BillNo.Properties.DataSource = dt
                BillNo.Properties.ValueMember = "ID"
                BillNo.Properties.DisplayMember = "BillNo"
                BillNo.Properties.ShowHeader = False
                BillNo.Properties.PopulateColumns()
                BillNo.Properties.Columns("ID").Visible = False
            End If
        Else
            BillNo.Properties.DataSource = Nothing
        End If
    End Sub
    'Sub LOADِAseet()
    '    AccIDEX.Properties.DataSource = Nothing
    '    If BranchID.Text <> String.Empty And BranchID.EditValue <> -1 Then
    '        Dim PR(1) As SqlParameter
    '        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
    '        PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 1}
    '        Dim dt As New DataTable
    '        dt.Clear()
    '        dt = RUN_QUARY_PRO("ExpensesTb_LOADTOLKPBasedOnExType", PR)
    '        If dt.Rows.Count > 0 Then
    '            AccIDEX.Properties.DataSource = dt
    '            AccIDEX.Properties.ValueMember = "AccID"
    '            AccIDEX.Properties.DisplayMember = "AccName"
    '            AccIDEX.Properties.ShowHeader = False
    '        End If
    '    Else
    '        AccIDEX.Properties.DataSource = Nothing
    '    End If
    'End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'FrmViewProExportItems.ShowDialog()
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub FRMANOTHEREXPENS_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasVal", PR)
        If dt.Rows.Count > 0 Then
            SafeID.Properties.PopulateColumns()
            SafeID.Properties.Columns("AccID").Visible = False
        End If
    End Sub
    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub AccIDEX_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ITMACCID.QueryPopUp
        'If IsAseet = False Then
        '    Dim PR(0) As SqlParameter
        '    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        '    Dim dt As New DataTable
        '    dt.Clear()
        '    dt = RUN_QUARY_PRO("[ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
        '    If dt.Rows.Count > 0 Then
        '        AccIDEX.Properties.PopulateColumns()
        '        AccIDEX.Properties.Columns("AccID").Visible = False
        '    End If
        'End If
        'If IsAseet = True Then
        '    Dim PR(1) As SqlParameter
        '    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        '    PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 1}
        '    Dim dt As New DataTable
        '    dt.Clear()
        '    dt = RUN_QUARY_PRO("ExpensesTb_LOADTOLKPBasedOnExType", PR)
        '    If dt.Rows.Count > 0 Then
        '        AccIDEX.Properties.PopulateColumns()
        '        AccIDEX.Properties.Columns("AccID").Visible = False
        '    End If
        'End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        LOADSafeID()
        'LOADEXPANSTYPE()
        LOADPROJECTID()
    End Sub
    Private Sub FRMANOTHEREXPENS_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub SafeID_TextChanged(sender As Object, e As EventArgs) Handles SafeID.TextChanged
        If SafeID.Text <> String.Empty Or SafeID.EditValue <> -1 Then
            If IsUpdate = False Then
                Dim DTT As New DataTable
                DTT.Clear()
                DTT = ANOTHEREXPENSTB_MaxID(BranchID.EditValue, SafeID.EditValue)
                CodeID = DTT.Rows(0)("ID")
                Code.Text = DTT.Rows(0)("Code")
            End If
        End If
    End Sub
    Public Function ANOTHEREXPENSTB_CHECKCODE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[ANOTHEREXPENSTB_CHECKCODE]", PRM)
        Return DT
    End Function

    Public Function ANOTHEREXPENSTB_MaxID(BranchID As Integer, SAFEID As ULong) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(1) = New SqlParameter("@SAFEID", SqlDbType.BigInt) With {.Value = SAFEID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[CONDB_PROEXPORTITEM_MaxID]", PRM)
        Return DT
    End Function

    Public Sub ANOTHEREXPENSTB_Insert()
        Dim MOV As String = "مقابل صرف صنف" & Space(1) & ITMACCID.Text
        'Try
        Dim prm(19) As SqlParameter
        prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text.Trim}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate.EditValue}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        prm(3) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = UserID}
        prm(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
        prm(5) = New SqlParameter("@ExpensVal", SqlDbType.Decimal) With {.Value = ExpensVal.EditValue}
        prm(6) = New SqlParameter("@ProjectID", SqlDbType.BigInt) With {.Value = ProjectID.EditValue}
        prm(7) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes.Text.Trim}
        prm(8) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = CodeID}
        prm(9) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}
        prm(10) = New SqlParameter("@ITMACCID", SqlDbType.BigInt) With {.Value = ITMACCID.EditValue}
        prm(11) = New SqlParameter("@Movement", SqlDbType.NVarChar, -1) With {.Value = MOV}
        prm(12) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        prm(13) = New SqlParameter("@UserInsert", SqlDbType.BigInt) With {.Value = UserID}
        prm(14) = New SqlParameter("@ExID", SqlDbType.Int) With {.Value = ItemID}
        prm(15) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(16) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(17) = New SqlParameter("@PRCAVG", SqlDbType.Decimal) With {.Value = UNTPRC.EditValue}
        prm(18) = New SqlParameter("@OUTQT", SqlDbType.Decimal) With {.Value = OUTQT.EditValue}
        prm(19) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = BillNo.Text}
        RUN_EXUTE_PRO("CONDB_PROEXPORTITEM_Insert", prm)
        If IsUpdate = 0 Then
            Dim MSGSTATUES As Integer = prm(15).Value
            Dim MSGBOX As String = prm(16).Value
            If MSGSTATUES = 0 Then
                ErrorMessage(Me, "رسالة تنبيه", MSGBOX)
            Else
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
            End If
        End If
        If IsUpdate = 1 Then
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد بالفعل تعديل البيانات؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                Me.Print()
                FrmEditMessage.Show()
                Me.NEWRECORD()
            Else
                Me.NEWRECORD()
            End If
        End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub

    Public Function SERACH_ANOTHEREXPENSTB(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[CONDB_PROEXPORTITEM_SELECTByCODE]", PRM)
        Return DT
    End Function

    Private Sub AccIDEX_EditValueChanged(sender As Object, e As EventArgs) Handles ITMACCID.EditValueChanged
        If IsUpdate = False Then
            If ITMACCID.EditValue <> -1 Or ITMACCID.Text <> String.Empty Then
                Dim editor As LookUpEdit = TryCast(sender, LookUpEdit)
                Dim value As Object = editor.GetColumnValue("ID")
                ItemID = value
                GETITMQT(value, BillNo.Text)
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@ITMID", SqlDbType.Int) With {.Value = value}
                Dim DT As New DataTable
                DT.Clear()
                DT = RUN_QUARY_PRO("CONDB_GETITMAVG", PR)
                If DT.Rows.Count > 0 Then
                    UNTPRC.EditValue = DT.Rows(0)("UnitPrice")
                End If
            End If
        End If

    End Sub

    Private Sub OUTQT_EditValueChanged(sender As Object, e As EventArgs) Handles OUTQT.EditValueChanged
        If UNTPRC.EditValue > 0 Or OUTQT.EditValue > 0 Then
            ExpensVal.EditValue = UNTPRC.EditValue * OUTQT.EditValue
        End If
    End Sub

    Private Sub OUTQT_Leave(sender As Object, e As EventArgs) Handles OUTQT.Leave
        If UNTPRC.EditValue > 0 Or OUTQT.EditValue > 0 Then
            ExpensVal.EditValue = UNTPRC.EditValue * OUTQT.EditValue
        End If
    End Sub

    Private Sub BillNo_EditValueChanged(sender As Object, e As EventArgs) Handles BillNo.EditValueChanged
        ITMACCID.EditValue = -1
        ITMQT.EditValue = 0.000

        If BillNo.EditValue = -1 Or BillNo.Text = String.Empty Then
            BillNo.ErrorText = "يرجى اختيار رقم الفاتورة"
            Exit Sub
        ElseIf BillNo.Text <> String.Empty Or BillNo.EditValue <> -1 Then
            LOADEXPANSTYPE()
        End If

    End Sub
End Class