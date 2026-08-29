Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Public Class FrmProPettyCash
    Dim CLSPC As New CLSPETTYCASH
    Public AcID, IDCode, AccCode, AccEm, CodeID As ULong
    Public Property EMBID As Integer
    Public StID, AccLine, AccCat, EMID As Integer
    Public Property X As String
    Public IsUpdate, UpdateBySalary As Boolean
    'Public Sub lodePreportes()
    'Dim dt As New DataTable
    'dt.Clear()
    'dt = SElectUEserFormButtn(40, UserID)
    'If dt.Rows.Count > 0 Then
    '    If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '    If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '    If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    'End If
    'End Sub
    Sub DISAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = False
        SafeID.Enabled = False
        CurrencyID.Enabled = False
        EMPID.Enabled = False
        PettyCashVal.Enabled = False
        IsActiveTG.Enabled = False
        Notes.Enabled = False
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
    Sub ENAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = True
        SafeID.Enabled = True
        CurrencyID.Enabled = False
        EMPID.Enabled = True
        PettyCashVal.Enabled = True
        IsActiveTG.Enabled = True
        Notes.Enabled = True
    End Sub
    Sub NEWRECORD()
        Code.Enabled = False
        LOADBRANCH()
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnEdit.Caption = "إرجاع قيمة العهدة"
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        LOADRECURRENCY()
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        CurrencyID.Text = "دينار ليبي"
        CurrencyID.Enabled = False
        IsUpdate = False
        ENAPLEDCONTROLS()
        EMPID.EditValue = -1
        Code.Text = ""
        SafeID.EditValue = -1
        IsActiveTG.EditValue = True
        PettyCashVal.EditValue = 0.000
        Notes.Text = ""
        If UserType = 1 Then
            BranchID.Enabled = True
            SafeID.Enabled = True
        Else
            BranchID.Enabled = False
            SafeID.Enabled = False
            SafeID.EditValue = UserAccID
        End If
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 139)

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

    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            ' CLSEMD.EMPDIS_MaxID(BranchID.EditValue)
            'If BRANCHID.Text <> String.Empty Then
            'EMPID.Properties.DataSource = Nothing
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CONDB_AddPartnerTb_LoadToLKP", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "PartnerName"
                EMPID.Properties.ShowHeader = False
            End If
            'Else
            '    EMPID.Properties.DataSource = Nothing
        End If
        LOADSafeID()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMViewProPettyCash.ShowDialog()
    End Sub

    Private Sub FRMPettyCash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@SettlementType", SqlDbType.Int) With {.Value = 0}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_AddPartnerTb_LOADINTOLKP", PR)
        If dt.Rows.Count > 0 Then
            EMPID.Properties.PopulateColumns()
            EMPID.Properties.Columns("AccID").Visible = False
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

    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub

    Private Sub FRMPettyCash_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Public Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.Text <> String.Empty Then
            EMID = EMPID.EditValue
        End If
        If IsUpdate = False Then
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = PettyCash_SelectMax(BranchID.EditValue, EMID)
            CodeID = DTT.Rows(0)("ID")
            Code.Text = DTT.Rows(0)("Code")
        End If
    End Sub

    Public Overrides Sub SetData()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
            EMPID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If PettyCashVal.EditValue <= 0.000 Then
            PettyCashVal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Return
        End If
        GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, 1)
        If PettyCashVal.EditValue > SAFEVAL Then
            XtraMessageBox.Show(lookFeelError, "القيمة المصروفة لا يجب أن تكون أكبر من قيمة الخزنة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim mov As String = ""
        Dim mov2 As String = ""
        If IsUpdate = False Then
            mov = "عهدة لموظف"
            mov2 = "عهدة للموظف" & Space(1) & EMPID.Text
            ACCOUNTSTB_insert(Code.Text.Trim, Date.Now, BranchID.EditValue, EMPID.EditValue, PettyCashVal.EditValue, CodeID, CurrencyID.EditValue,
                                    UserID, Notes.Text.Trim, 0, SafeID.EditValue, mov, mov2, IsUpdate)
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2

        End If
        MyBase.SetData()
    End Sub

    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
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
            Dim dt As DataTable = RUN_QUARY_PRO("CONDB_ZRPT_PettyCashTb_LOADTOREPORT", PRM)
            Dim ds As New DataSet
            dt.TableName = "PettyCashTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTPettyCash
                report.DataSource = ds
                report.DataMember = "PettyCashTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.XrLabel25.Text = Cur_Code(CurrencyID.Text, PettyCashVal.EditValue, False, "n2")
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
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
            CurrencyID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
            SafeID.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
        If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
            EMPID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If PettyCashVal.EditValue <= 0 Then
            PettyCashVal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Return
        End If

        If IsUpdate = True Then
            Dim mov As String = "معالجة خطأ في عهدة الموظف"
            Dim mov2 As String = "معالجة خطأ في عهدة الموظف" & Space(1) & EMPID.Text
            CLSPC.ACCOUNTSTB_insert(Code.Text.Trim, Date.Now, BranchID.EditValue, EMID, PettyCashVal.EditValue, CodeID, CurrencyID.EditValue,
                                    UserID, Notes.Text.Trim, 0, SafeID.EditValue, mov, mov2, IsUpdate)
            'CLSPC.EMPORCUSTWITHDRAWALTB_Delete(Code.Text.Trim)
            'CLSPC.AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(UserID, 0.000, PettyCashVal.EditValue, Date.Now, Code.Text.Trim, 13, 43,
            'BranchID.EditValue, EMPID.EditValue, SafeID.EditValue, mov, Notes.Text.Trim)

            'CLSPC.AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(UserID, PettyCashVal.EditValue, 0.000, Date.Now, Code.Text.Trim, 13, 43,
            'BranchID.EditValue, SafeID.EditValue, EMPID.EditValue, mov, Notes.Text.Trim)
        End If

        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Function SERACH_EMPORC(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[CONDB_PettyCashTb_Select]", PRM)
        Return DT
    End Function
    Sub SHOW_EMCUSCODE(x)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = SERACH_EMPORC(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                SafeID.EditValue = DT.Rows(0)("AccIDSafeID")
                ' WithdrawalDate.EditValue = DT.Rows(0)("InsertDate")
                PettyCashVal.EditValue = DT.Rows(0)("PettyCashVal")
                EMID = DT.Rows(0)("EMPID")
                BranchID_TextChanged(Nothing, Nothing)
                EMPID.EditValue = DT.Rows(0)("EmpAccID")
                CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
                Notes.Text = DT.Rows(0)("Notes")
            End If
        End If

    End Sub

    Private Sub FRMPettyCash_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        NEWRECORD()
        IsUpdate = False
        ENAPLEDCONTROLS()
    End Sub
#Region "FUNCTIONS"
    Public Function PettyCash_SelectMax(BranchID As Integer, EMPID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int)
        PRM(1).Value = EMPID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_PettyCashTb_MaxID", PRM)
        Return DT
    End Function
    Public Sub ACCOUNTSTB_insert(Code As String, InsertDate As Date, BranchID As Integer, EMPID As Integer, PettyCashVal As Decimal, IDCode As ULong, CurrencyID As Integer,
                                 SafeID As Integer, Notes As String, AccIDPetty As ULong, AccIDSafeID As ULong, MovementType As String, MovementType2 As String, IsUpdate As Boolean)
        Try
            Dim prm(15) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (300)) With {.Value = Code}
            prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            prm(3) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            prm(4) = New SqlParameter("@PettyCashVal", SqlDbType.Decimal) With {.Value = PettyCashVal}
            prm(5) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = IDCode}
            prm(6) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            prm(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            prm(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            prm(9) = New SqlParameter("@AccIDPetty", SqlDbType.BigInt) With {.Value = AccIDPetty}
            prm(10) = New SqlParameter("@AccIDSafeID", SqlDbType.BigInt) With {.Value = AccIDSafeID}
            prm(11) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
            prm(12) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = MovementType2}
            prm(13) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            prm(14) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(15) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("CONDB_PettyCashTb_Insert", prm)
            If prm(14).Value = 0 Then
                ErrorMessage(Me, "رسالة تنبيه", prm(15).Value)
                If Me.IsUpdate = False Then
                    If (Me.BranchID.EditValue <> -1 Or Me.BranchID.Text <> String.Empty) And (Me.EMPID.EditValue <> -1 Or Me.EMPID.Text <> String.Empty) Then
                        EMPID_TextChanged(Nothing, Nothing)
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
                Print()
            End If
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    'Public Sub AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(ByVal SafeID As Integer, ByVal Debit As Decimal, ByVal Credit As Decimal, ByVal InsertDate As Date, ByVal Code As String, TypeID As Int32, OperationTypeID As Integer,
    '                                        AccBranchID As Integer, AccIDFrom As Integer, AccIDTo As Integer, MovementType As String, Note As String)
    '    Dim PRM(11) As SqlParameter
    '    PRM(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
    '    PRM(1) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
    '    PRM(2) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
    '    PRM(3) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
    '    PRM(4) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
    '    PRM(5) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
    '    PRM(6) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
    '    PRM(7) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
    '    PRM(8) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = AccIDFrom}
    '    PRM(9) = New SqlParameter("@AccIDTo", SqlDbType.Int) With {.Value = AccIDTo}
    '    PRM(10) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
    '    PRM(11) = New SqlParameter("@Note", SqlDbType.NVarChar, -1) With {.Value = Note}
    '    RUN_EXUTE_PRO("AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert", PRM)
    'End Sub
    Public Function SERACH_EMPORCUSTWITHDRAWALTB(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[PettyCashTb_Select]", PRM)
        Return DT
    End Function

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            ' CLSEMD.EMPDIS_MaxID(BranchID.EditValue)
            'If BRANCHID.Text <> String.Empty Then
            'EMPID.Properties.DataSource = Nothing
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CONDB_AddPartnerTb_LoadToLKP", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "ID"
                EMPID.Properties.DisplayMember = "PartnerName"
                EMPID.Properties.ShowHeader = False
            End If
            'Else
            '    EMPID.Properties.DataSource = Nothing
        End If
        LOADSafeID()
    End Sub
    'Public Sub EMPORCUSTWITHDRAWALTB_Delete(ByVal Code As String)
    '    Dim PRM(0) As SqlParameter
    '    PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
    '    RUN_EXUTE_PRO("[PettyCashTb_Delete]", PRM)
    'End Sub
#End Region
End Class