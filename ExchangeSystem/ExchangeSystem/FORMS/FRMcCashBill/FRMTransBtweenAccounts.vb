Imports System.Data.SqlClient
Imports DevExpress.DataProcessing.InMemoryDataProcessor
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraPrinting.Shape.Native
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSpellChecker

Public Class FRMTransBtweenAccounts
    Public IsUpdate As Boolean
    Public IDCode As ULong
    Sub DISAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        BranchID.Enabled = False
        CURRENCYID.Enabled = False
        AccountType.Enabled = False
        AccFrom.Enabled = False
        AccFromValue.Enabled = False
        AccToValue.Enabled = False
        AccTo.Enabled = False
        BillVal.Enabled = False
        Notes.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = False
        BtnPrint.Enabled = True
        AccountType1.Enabled = False
    End Sub
    Sub ENAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        BranchID.Enabled = False
        AccountType.Enabled = True
        AccFrom.Enabled = True
        AccFromValue.Enabled = False
        AccTo.Enabled = True
        AccToValue.Enabled = False
        BillVal.Enabled = True
        CURRENCYID.Enabled = True
        Notes.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        AccountType1.Enabled = True
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
    Sub LOADRECURRENCY()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CurrencyMainTb_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            CURRENCYID.Properties.DataSource = DT
            CURRENCYID.Properties.ValueMember = "ID"
            CURRENCYID.Properties.DisplayMember = "CuName"
            CURRENCYID.Properties.ShowHeader = False
            CURRENCYID.Properties.PopulateColumns()
            CURRENCYID.Properties.Columns(0).Visible = False
        End If
    End Sub
    Sub LoadAccountType(LKP As LookUpEdit)
        AccountType.Properties.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AccountTB_LoadLine3ToLKP", PR)
        If dt.Rows.Count > 0 Then
            LKP.Properties.DataSource = dt
            LKP.Properties.ValueMember = "AccCode"
            LKP.Properties.DisplayMember = "AccName"
            LKP.Properties.ShowHeader = False
            LKP.Properties.PopulateColumns()
            LKP.Properties.Columns(0).Visible = False
        End If
    End Sub
    Sub NEWRECORD()
        AccFrom.Properties.DataSource = Nothing
        AccTo.Properties.DataSource = Nothing
        AccountType.Properties.DataSource = Nothing
        AccountType1.Properties.DataSource = Nothing
        AccountType.EditValue = -1
        IsUpdate = False
        ENAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        InsertDate.EditValue = Date.Now
        BranchID.EditValue = -1
        LOADBRANCH()
        LOADRECURRENCY()
        AccFrom.EditValue = -1
        AccTo.EditValue = -1
        BranchID.EditValue = BID
        CURRENCYID.EditValue = 1
        BranchID.Enabled = False
        AccFromValue.EditValue = 0.000
        AccToValue.EditValue = 0.000
        BillVal.EditValue = 0.000
        Notes.Text = ""
        AccountType1.EditValue = -1
        LoadAccountType(AccountType1)
        LoadAccountType(AccountType)
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 132)
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Sub LOADAccFromOrAccTo(LKP As LookUpEdit, Parent As ULong)
        Try
            If BranchID.Text <> String.Empty And Parent > -1 Then
                Dim PR(3) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 37}
                PR(2) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = Parent}
                PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CURRENCYID.EditValue}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("AccountsTb_LOADINTOLKPBASEDONAccParent", PR)
                If dt.Rows.Count > 0 Then
                    LKP.Properties.DataSource = dt
                    LKP.Properties.ValueMember = "AccID"
                    LKP.Properties.DisplayMember = "AccName"
                    LKP.Properties.ShowHeader = False
                    LKP.Properties.PopulateColumns()
                    HideAllColumnsExceptDisplay(LKP)
                Else
                    LKP.EditValue = -1
                    LKP.Properties.DataSource = Nothing
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AccountType_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType.EditValueChanged
        If AccountType.EditValue <> -1 Then
            LOADAccFromOrAccTo(AccFrom, AccountType.EditValue)
        End If
    End Sub

    Private Sub AccountType1_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType1.EditValueChanged
        If AccountType1.EditValue <> -1 Then
            LOADAccFromOrAccTo(AccTo, AccountType1.EditValue)
        End If
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub

    Private Sub FRMTransBtweenAccounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnNew.PerformClick()
    End Sub
    Public Overrides Sub SetData()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If
        If CURRENCYID.EditValue = -1 Then
            CURRENCYID.ErrorText = "يجب اختيار العملة"
            Exit Sub
        End If
        If AccountType.EditValue = -1 Then
            AccountType.ErrorText = "يجب اختيار نوع الحساب"
            Exit Sub
        End If
        If AccFrom.EditValue = -1 Or AccFrom.Text = String.Empty Then
            AccFrom.ErrorText = "يجب اختيار الحساب"
            Exit Sub
        End If
        If AccountType1.EditValue = -1 Then
            AccountType1.ErrorText = "يجب اختيار نوع الحساب"
            Exit Sub
        End If
        If AccTo.EditValue = -1 Or AccTo.Text = String.Empty Then
            AccTo.ErrorText = "يجب اختيار الحساب"
            Exit Sub
        End If
        If BillVal.EditValue <= 0.000 Then
            BillVal.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If
        'If BillVal.EditValue > AccFromValue.EditValue Then
        '    AccFromValue.ErrorText = "القيمة لا يجب أن تكون أكبر من رصيد الحساب الأول"
        '    Exit Sub
        'End If
        If AccTo.EditValue = AccFrom.EditValue Then
            AccTo.ErrorText = "عذرا لايمكن التحويل إلى نفس الحساب يجب اختيار حسابين مختلفين"
            Exit Sub
        End If
        TransBetweenAccountsTB_Insert(CodeID.Text, Date.Now, IDCode, BranchID.EditValue, AccountType.EditValue, AccFrom.EditValue, AccountType1.EditValue, AccTo.EditValue, BillVal.EditValue, Notes.Text.Trim, CURRENCYID.EditValue)
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then

            LoadAccountType(AccountType1)
            LoadAccountType(AccountType)
            TransBetweenAccountsTB_MaxID(BranchID.EditValue, UserID)
        End If
    End Sub
    Public Sub TransBetweenAccountsTB_MaxID(BranchID As Integer, SAFEID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(1) = New SqlParameter("@SAFEID", SqlDbType.Int) With {.Value = SAFEID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("TransBetweenAccountsTB_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            CodeID.Text = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub TransBetweenAccountsTB_Insert(ByVal Code As String, ByVal InsertDate As Date, CODEID As ULong, Branch As Integer, AccFromParent As ULong, AccIDFrom As ULong,
                                        AccToParent As ULong, AccIDTo As ULong, TransValue As Decimal, Notes As String, CurrencyyID As Integer)
        Try
            Dim PRM(14) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(2) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = CODEID}
            PRM(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = Branch}
            PRM(4) = New SqlParameter("@SafeAccID", SqlDbType.BigInt) With {.Value = UserAccID}
            PRM(5) = New SqlParameter("@AccFromParent", SqlDbType.BigInt) With {.Value = AccFromParent}
            PRM(6) = New SqlParameter("@AccFrom", SqlDbType.BigInt) With {.Value = AccIDFrom}
            PRM(7) = New SqlParameter("@AccToParent", SqlDbType.BigInt) With {.Value = AccToParent}
            PRM(8) = New SqlParameter("@AccTo", SqlDbType.BigInt) With {.Value = AccIDTo}
            PRM(9) = New SqlParameter("@TransValue", SqlDbType.Decimal) With {.Value = TransValue}
            PRM(10) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(11) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyyID}
            PRM(12) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
            PRM(13) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(14) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("TransBetweenAccountsTB_Insert", PRM)
            If PRM(13).Value = 0 Then
                MessageBox.Show(PRM(14).Value, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                TransBetweenAccountsTB_MaxID(BranchID.EditValue, UserID)
                Exit Sub
            End If
            Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "🔁عملية تحويل بين الحسابات" & vbNewLine & "CODE " & ":" & Space(1) & Code & vbNewLine & "*تم تحويل مبلغ*" & Space(1) & ":" & Cur_Code(CURRENCYID.Text, BillVal.EditValue, True, "n2") & vbNewLine & Cur_Code(CURRENCYID.Text, BillVal.EditValue, False, "n2") & vbNewLine & "من حسابكم" & Space(1) & "(" & "كود" & ":" & Space(1) & AccIDFrom & ")" & vbNewLine & "إلى حساب:" & Space(1) & AccTo.Text & vbNewLine & "(" & "كود " & ":" & Space(1) & AccIDTo & ")" & vbNewLine & "شكراً لتعاملكم معنا 🌿"

            Dim mms2 As String = "شركة الرحالة للصرافة " & vbNewLine & "🔁عملية تحويل بين الحسابات" &
            vbNewLine & "CODE " & ":" & Space(1) & Code & vbNewLine & "*تم دخول مبلغ*" & Space(1) & ":" & Space(1) & Cur_Code(CURRENCYID.Text, BillVal.EditValue, True, "n2") & vbNewLine & Cur_Code(CURRENCYID.Text, BillVal.EditValue, False, "n2") & vbNewLine & "إلى حسابكم" & Space(1) & "(" & "كود " & ":" & Space(1) & AccIDTo & ")" & vbNewLine & "من حساب:" & Space(1) & AccFrom.Text & vbNewLine & "(" & "كود " & ":" & Space(1) & AccIDFrom & ")" & vbNewLine & "شكراً لتعاملكم معنا 🌿"
            '    Dim Dt As New DataTable
            'Dt.Clear()
            'Dt = GETPhoneAccID(AccFrom.EditValue)
            '    Dim Phone1 = Dt.Rows(0)("AccPhone")
            '    Dt.Dispose()
            'Dt = GETPhoneAccID(AccTo.EditValue)
            'Dim Phone2 = Dt.Rows(0)("AccPhone")
            Dim Phone1 = GetLKPColumnVal(AccFrom, "AccPhone")
            Dim Phone2 = GetLKPColumnVal(AccTo, "AccPhone")
            WATSAPPMsAG(Phone1, mms, True)
        WATSAPPMsAG(Phone2, mms2, True)
        FrmSavedSuccessfully.Show()
        Print()
        NEWRECORD()
        Catch ex As Exception
        MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMViewTransBtweenAccounts.ShowDialog()
    End Sub
    Public Sub TransBtweenAccounts_GetRecord(X)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = X}
        Dim dt As New DataTable
        dt.Clear()
        dt = TransBtweenAccountsTB_SelectByCode(X)
        If dt.Rows.Count > 0 Then
            IsUpdate = True
            DISAPLEDCONTROLS()
            CodeID.Text = dt.Rows(0)("Code").ToString
            InsertDate.EditValue = dt.Rows(0)("TransDate")
            BranchID.EditValue = dt.Rows(0)("BranchID")
            AccountType.EditValue = dt.Rows(0)("AccFromParent")
            LOADAccFromOrAccTo(AccFrom, dt.Rows(0)("AccFromParent"))
            AccFrom.EditValue = dt.Rows(0)("TransFrom")
            AccountType1.EditValue = dt.Rows(0)("AccToParent")
            LOADAccFromOrAccTo(AccTo, dt.Rows(0)("AccToParent"))
            AccTo.EditValue = dt.Rows(0)("TransTo")
            CURRENCYID.EditValue = dt.Rows(0)("CurrencyID")
            BillVal.EditValue = dt.Rows(0)("TransValue")
            Notes.Text = dt.Rows(0)("Notes").ToString
        End If
    End Sub
    Public Function TransBtweenAccountsTB_SelectByCode(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TransBtweenAccountsTb_SelectByCode", PRM)
        Return DT
    End Function
    Public Sub GETVALTOLTEl(AccName As ULong, NetTotal As SpinEdit)
        Try
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = AccName}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = CURRENCYID.EditValue}
            PR(2) = New SqlParameter("@LadType", SqlDbType.Int) With {.Value = 5}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("[EMPORCUST_GetAccValCashOnly](@AccName,@crunseType,@LadType) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                NetTotal.EditValue = dt.Rows(0)("GetAccVal")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
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
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub
    Private Sub AccFrom_EditValueChanged(sender As Object, e As EventArgs) Handles AccFrom.EditValueChanged
        If AccFrom.EditValue <> -1 Then
            GETVALTOLTEl(AccFrom.EditValue, AccFromValue)
        End If
    End Sub

    Private Sub AccTo_EditValueChanged(sender As Object, e As EventArgs) Handles AccTo.EditValueChanged
        If AccTo.EditValue <> -1 Then
            GETVALTOLTEl(AccTo.EditValue, AccToValue)
        End If
    End Sub

    Public Overrides Sub Print()

        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("ZRPT_TransBtweenAccountsTb_SelectByCode", PRM)
            If DT.Rows.Count > 0 Then
                Dim report As New RPTTransBtweenAccounts
                report.DataSource = DT
                report.DataMember = "PettyCashTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub
End Class