Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Reflection
Imports DevExpress.DataProcessing.InMemoryDataProcessor
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSpellChecker

Public Class FRMWorkshopOperations
    Public IsUpdate As Boolean
    Public IDCode As ULong
    Public OpType As Integer
    Dim UserMovement As String
    Sub Newrecord()
        ActivityType.EditValue = -1
        BranchID.EditValue = -1
        IsUpdate = False
        LOADBRANCH()
        AcType()
        InsertDate.EditValue = Date.Now
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        AccountType.SelectedIndex = -1
        NetVal.EditValue = 0
        Notes.Text = String.Empty
        BankNo.Text = String.Empty
        CustName.Text = String.Empty
        CustBankNo.Text = String.Empty
        CustPhone.Text = String.Empty
        SafeID.EditValue = -1
        CountOfOperation.EditValue = 0
        CustName.Enabled = False
        CustBankNo.Enabled = False
        CustPhone.Enabled = False
        If BranchID.EditValue <> -1 Or BranchID.Text <> "" Then
            LoadSafeID(BranchID.EditValue)
            LOADRECURRENCY()
            CURRENCYID.EditValue = DefaultCurrency
        End If
        EnabledCONTROLS()
        'FrmScreensTb_Details_UESIRID_GETFrom(UserID, 162)
    End Sub
    Sub LOADRECURRENCY()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CURRENCYTB_LoadWithBranch", PRM)
        If DT.Rows.Count > 0 Then
            CURRENCYID.Properties.DataSource = DT
            CURRENCYID.Properties.ValueMember = "ID"
            CURRENCYID.Properties.DisplayMember = "CurrencyName"
            CURRENCYID.Properties.ShowHeader = False
            CURRENCYID.Properties.PopulateColumns()
            CURRENCYID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Sub LoadSafeID(BranchLkp As Integer)
        SafeID.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchLkp}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("HagezOmlaAgnbia", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.DataSource = dt
                SafeID.Properties.ValueMember = "AccID"
                SafeID.Properties.DisplayMember = "AccName"
                SafeID.Properties.ShowHeader = False
                'SafeID.Properties.PopulateColumns()
                'SafeID.Properties.Columns("AccID").Visible = False
            ElseIf dt.Rows.Count = 0 Then
                SafeID.Properties.DataSource = Nothing
            End If
        End If
    End Sub
    Sub DISAPLEDCONTROLS()
        BranchID.Enabled = False
        AccountType.Enabled = False
        NetVal.Enabled = False
        Notes.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = False
        BtnPrint.Enabled = True
        CURRENCYID.Enabled = False
        SafeID.Enabled = False
        ActivityType.Enabled = False
        CountOfOperation.Enabled = False
        CustName.Enabled = False
        CustBankNo.Enabled = False
        CustPhone.Enabled = False
    End Sub
    Sub EnabledCONTROLS()
        BranchID.Enabled = True
        AccountType.Enabled = True
        NetVal.Enabled = True
        Notes.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        CURRENCYID.Enabled = True
        SafeID.Enabled = True
        ActivityType.Enabled = True
        CountOfOperation.Enabled = True
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Sub AcType()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CONDB_ActivityType_LoadDataIntoLookUpEdit")
        If dt.Rows.Count > 0 Then
            ActivityType.Properties.DataSource = dt
            ActivityType.Properties.ValueMember = "AccCode"
            ActivityType.Properties.DisplayMember = "AccName"
            ActivityType.Properties.ShowHeader = False
            ActivityType.ItemIndex = 0
        End If
    End Sub

    'Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
    '    Dim prm(1) As SqlParameter
    '    prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
    '    prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

    '    If dt.Rows.Count > 0 Then
    '        BranchID.Enabled = dt.Rows(0)("Can_branch")
    '        SafeID.Enabled = dt.Rows(0)("Can_safID")
    '        SafeID.EditValue = UserAccID
    '        BranchID.EditValue = BID
    '    Else
    '        BranchID.Enabled = False
    '        SafeID.Enabled = False
    '        SafeID.EditValue = UserAccID
    '        BranchID.EditValue = BID
    '    End If
    'End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub FRMWorkshopOperations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Newrecord()
    End Sub
    Public Overrides Sub BNew()
        Newrecord()
        MyBase.BNew()
    End Sub
    Public Sub WorkshopOperations_MaxID(BranchID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_WorkshopOperations_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            CodeID.Text = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue <> -1 And BranchID.Text <> "" And IsUpdate = False Then
            WorkshopOperations_MaxID(BranchID.EditValue)
        End If
        If BranchID.EditValue <> -1 And BranchID.Text <> "" Then
            LoadSafeID(BranchID.EditValue)
            LOADRECURRENCY()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Public Overrides Sub SetData()
        IsDataValidLKP(BranchID)
        If ActivityType.EditValue = -1 Then
            ActivityType.ErrorText = "يجب اختيار النشاط"
            Exit Sub
        End If

        If NetVal.EditValue <= 0.000 Then
            NetVal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If

        If AccountType.SelectedIndex = 0 Then
            OpType = 81
            UserMovement = "سداد مصروف"
        ElseIf AccountType.SelectedIndex = 1 Then
            OpType = 82
            UserMovement = "تكلفة العملة"
        ElseIf AccountType.SelectedIndex = 2 Then
            OpType = 83
            UserMovement = "مبيعات عملة"
        End If
        If IsUpdate = 0 Then
            ANOTHEREXPENSTB_Insert()
        End If
        MyBase.SetData()
    End Sub
    Public Sub ANOTHEREXPENSTB_Insert()
        'Dim MOV As String = "مقابل صرف صنف" & Space(1) & ITMACCID.Text
        Try
            Dim prm(18) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
            prm(1) = New SqlParameter("@CodeID", SqlDbType.NVarChar, -1) With {.Value = IDCode}
            prm(2) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate.EditValue}
            prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(4) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = AccountType.SelectedIndex}
            prm(5) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = UserID}
            prm(6) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = SafeID.EditValue}
            prm(7) = New SqlParameter("@AccIDTO", SqlDbType.BigInt) With {.Value = 0}
            prm(8) = New SqlParameter("@NetVal", SqlDbType.Decimal) With {.Value = NetVal.EditValue}
            prm(9) = New SqlParameter("@Note", SqlDbType.NVarChar, -1) With {.Value = Notes.Text.Trim}
            prm(10) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(12) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            prm(13) = New SqlParameter("@ActivityType", SqlDbType.BigInt) With {.Value = ActivityType.EditValue}
            prm(14) = New SqlParameter("@BankNo", SqlDbType.NVarChar, 250) With {.Value = SafeToString(BankNo.Text)}
            prm(15) = New SqlParameter("@CustName", SqlDbType.NVarChar, 250) With {.Value = SafeToString(CustName.Text)}
            prm(16) = New SqlParameter("@CustBankNo", SqlDbType.NVarChar, 250) With {.Value = SafeToString(CustBankNo.Text)}
            prm(17) = New SqlParameter("@CustPhone", SqlDbType.NVarChar, 250) With {.Value = SafeToString(CustPhone.Text)}
            prm(18) = New SqlParameter("@CountOfOperation", SqlDbType.Int) With {.Value = CountOfOperation.EditValue}
            RUN_EXUTE_PRO("CONDB_WorkshopOperationsTB_Insert", prm)
            If IsUpdate = 0 Then
                Dim MSGSTATUES As Integer = prm(10).Value
                Dim MSGBOX As String = prm(11).Value
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
                        Me.Newrecord()
                    Else
                        FrmSavedSuccessfully.Show()
                        Me.Newrecord()
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
                    Me.Newrecord()
                Else
                    Me.Newrecord()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub SHOW_EMCUSCODE(x)
        If Me.IsUpdate = True Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = x}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("WorkshopOperationsTB_LOADTOSELECT", PR)
            If DT.Rows.Count > 0 Then
                BranchID_EditValueChanged(Nothing, Nothing)
                LoadSafeID(DT.Rows(0)("BranchID"))
                CodeID.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                ActivityType.EditValue = DT.Rows(0)("ActivityAccCode")
                AccountType.SelectedIndex = DT.Rows(0)("TypeID")
                SafeID.EditValue = DT.Rows(0)("UserAccID")
                InsertDate.EditValue = DT.Rows(0)("InsertDate")
                AccountType.SelectedIndex = DT.Rows(0)("TypeID")
                NetVal.EditValue = DT.Rows(0)("NetVal")
                CountOfOperation.EditValue = DT.Rows(0)("CountOfOperation")
                BankNo.Text = DT.Rows(0)("BankNo").ToString
                CustName.Text = DT.Rows(0)("CustName").ToString
                CustBankNo.Text = DT.Rows(0)("CustBankNo").ToString
                CustPhone.Text = DT.Rows(0)("CustPhone").ToString

                CURRENCYID.Text = "دينار ليبي"
                Notes.Text = DT.Rows(0)("Note").ToString
            End If
        End If
    End Sub
    Public Overrides Sub UPDATERECORD()
        'IsDataValidLKP(BranchID)
        'If AccountType.SelectedIndex = 3 Or AccountType.SelectedIndex = 5 Then
        '    If AccIDTO.EditValue = -1 Then
        '        AccIDTO.ErrorText = "يجب اختيار الحساب"
        '        Exit Sub
        '    End If
        'End If
        'If NetVal.EditValue <= 0.000 Then
        '    NetVal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
        '    Exit Sub
        'End If

        'If AccountType.SelectedIndex = 0 Then
        '    OpType = 81
        '    UserMovement = "سداد مصروف ورشة"
        'ElseIf AccountType.SelectedIndex = 1 Then
        '    OpType = 82
        '    UserMovement = "شراء أصل للورشة"
        'ElseIf AccountType.SelectedIndex = 2 Then
        '    OpType = 83
        '    UserMovement = "شراء مواد للورشة"
        'ElseIf AccountType.SelectedIndex = 3 Then
        '    OpType = 84
        '    UserMovement = "تسوية مواد للورشة"
        'ElseIf AccountType.SelectedIndex = 4 Then
        '    OpType = 85
        '    UserMovement = "مبيعات خارجية من الورشة"
        'ElseIf AccountType.SelectedIndex = 5 Then
        '    OpType = 86
        '    UserMovement = "مبيعات للمشاريع من الورشة"
        'End If
        'If IsUpdate = 1 Then
        '    ANOTHEREXPENSTB_Insert()
        'End If
        MyBase.UPDATERECORD()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMViewWorkshopOperations.ShowDialog()
    End Sub
    Public Overrides Sub Print()
        Dim Prm(0) As SqlParameter
        Prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_ZRPT_WorkshopOperationsTB_SelectByCode", Prm)
        If dt.Rows.Count > 0 Then
            Dim report As New RPTWorkshopOperations
            report.DataSource = dt
            report.DataMember = "WorkshopOperationsTb"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.XrLabel36.Text = Cur_Code(CURRENCYID.Text, NetVal.EditValue, False, "n2")
            If AccountType.SelectedIndex = 2 Then
                report.XrLabel3.Visible = True
                report.XrLabel5.Visible = True
                report.XrLabel6.Visible = True
                report.XrLabel17.Visible = True
                report.XrLabel18.Visible = True
                report.XrLabel16.Visible = True
                report.XrLabel19.Visible = True
                report.XrPictureBox6.Visible = True
                report.XrPictureBox12.Visible = True
                report.XrPictureBox14.Visible = True
            Else
                report.XrLabel3.Visible = False
                report.XrLabel5.Visible = False
                report.XrLabel6.Visible = False
                report.XrLabel17.Visible = False
                report.XrLabel18.Visible = False
                report.XrLabel16.Visible = False
                report.XrLabel19.Visible = False
                report.XrPictureBox6.Visible = False
                report.XrPictureBox12.Visible = False
                report.XrPictureBox14.Visible = False
            End If
            report.CreateDocument()
            report.ShowPreview()
        End If
        MyBase.Print()
    End Sub

    Private Sub ActivityType_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ActivityType.QueryPopUp
        ActivityType.Properties.PopulateColumns()
        ActivityType.Properties.Columns("AccCode").Visible = False
    End Sub

    Private Sub ActivityType_EditValueChanged(sender As Object, e As EventArgs) Handles ActivityType.EditValueChanged
        AccountType.SelectedIndex = -1
    End Sub

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        HideAllColumnsExceptDisplay(SafeID)
    End Sub

    Private Sub SafeID_EditValueChanged(sender As Object, e As EventArgs) Handles SafeID.EditValueChanged
        BankNo.Text = SafeToString(GetLKPColumnVal(SafeID, "AccountNo"))
    End Sub

    Private Sub AccountType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AccountType.SelectedIndexChanged
        CustName.Text = String.Empty
        CustBankNo.Text = String.Empty
        CustPhone.Text = String.Empty
        If AccountType.SelectedIndex = 2 And IsUpdate = 0 Then
            CustName.Enabled = True
            CustBankNo.Enabled = True
            CustPhone.Enabled = True
        Else
            CustName.Enabled = False
            CustBankNo.Enabled = False
            CustPhone.Enabled = False
        End If
    End Sub
End Class