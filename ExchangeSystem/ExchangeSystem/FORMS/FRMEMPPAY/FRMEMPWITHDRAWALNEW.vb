Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Reflection
Imports DevExpress.CodeParser
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraReports.UI

Public Class FRMEMPWITHDRAWALNEW
    Dim clsemwdn As New CLSEMPWITHDRAWALNEW
    Dim clse As New CLSEMPWITHDRAWAL
    Public IDCode As ULong
    Public LOADTYPE, EMPID, TYPEs As Integer
    Public IsUpdate, CanChangeSafe As Boolean
    Public SalaryVal, AdvancedPaymentVal As Decimal
    Sub DISAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        WDValue.Enabled = False
        WithdrawalFrom.Enabled = False
        WithdrawalValue.Enabled = False
        SafeID.Enabled = False
    End Sub
    Public frmid As New Integer

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(frmid, UserID)


        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then
                SafeID.Enabled = False
                CanChangeSafe = False
            Else
                SafeID.Enabled = True
                CanChangeSafe = True
            End If
        End If


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
    End Sub
    Sub NEWRECORD()
        clsemwdn.EMPORCUSTWITHDRAWALTB_MaxID(5)
        IsUpdate = False
        ENAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        WithdrawalDate.EditValue = Date.Now
        BranchID.EditValue = -1
        LOADBRANCH()
        LOADCIDFROM()
        BranchID.Select()
        BranchID.EditValue = BID
        SalaryVal = 0.000
        AdvancedPaymentVal = 0.000
        CurrentSalaryVal.EditValue = 0.000
        WithdrawalValue.EditValue = 0.000
        WDValue.EditValue = 0.000
        WithdrawalFrom.EditValue = -1
        'SafeID.EditValue = UserID
        BtnSave.Enabled = True
        'BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        Notes.Text = ""
        LOADTYPE = 5
        CurrencyFrom.Enabled = False
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, frmid)
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadconnectedBranch")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADCIDFROM()
        Dim DT As New DataTable
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@TYPE", SqlDbType.Int) With {.Value = 1}
        DT = RUN_QUARY_PRO("[CurrencyMainTb_LOADTOLKP_buk]", prm)
        DVGFormat(GVROLE)
        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "ID"
            CurrencyFrom.Properties.DisplayMember = "CuName"
            Dim firstValue = CurrencyFrom.Properties.GetKeyValue(0)
            CurrencyFrom.EditValue = firstValue
        Else
            CurrencyFrom.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub
    Sub LOADCUST()
        If BranchID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CustomersTb_LOADTOLKPBasedOnBranchID", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "CustName"
                WithdrawalFrom.Properties.ShowHeader = False
            End If
        End If
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

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
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
    Private Sub FRMEMPWITHDRAWALNEW_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Sub LOADCUSTORSAFE()
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            SafeID.EditValue = -1
            LOADSafeID()
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADBASICDATATONEWWD", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "EMPNAME"
                WithdrawalFrom.Properties.ShowHeader = False
            Else
                WithdrawalFrom.Properties.DataSource = dt
            End If
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

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        NEWRECORD()
        BranchID.EditValue = BID
        FRMVIEWFRMEMPWITHDRAWALNEW.GCRole.DataSource = Nothing
        FRMVIEWFRMEMPWITHDRAWALNEW.GVRole.Columns.Clear()
        FRMVIEWFRMEMPWITHDRAWALNEW.ShowDialog()
    End Sub


#Region "Save,Update,Search"
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
        LOADTYPE = 5
        If CurrencyFrom.EditValue = -1 Then
            CurrencyFrom.ErrorText = "يجب اختيار العملة"
            Exit Sub
        End If
        GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, 1)

        If WDValue.EditValue > SAFEVAL Then
                ErrorMessage(Me, "رسالة معلومات", "القيمة المصروفة لا يجب أن تكون أكبر من قيمة الخزنة")

                Exit Sub
            End If
            If WDValue.EditValue > CurrentSalaryVal.EditValue Then
                ErrorMessage(Me, "رسالة معلومات", "القيمة المصروفة لا يجب أن تكون أكبر من القيمة الموجودة بالحساب")
                Exit Sub
            End If


        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""

        MOTYPE = "سحب من حساب الموظف"
            MOTYPE2 = "سحب من حساب الموظف" & Space(1) & WithdrawalFrom.Text
            FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue = WithdrawalFrom.EditValue
            FRMCODEPYMENT_em_cu2.lodeDate(MOTYPE2, WithdrawalFrom.Text, WithdrawalFrom.EditValue, WDValue.EditValue, CurrencyFrom.Text, GET_PHONE_SaenFroWtsaap(WithdrawalFrom.EditValue), 1, "")
            FRMCODEPYMENT_em_cu2.ShowDialog()
            If FRMCODEPYMENT_em_cu2.chick = True Then

            EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, 0, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, CurrencyFrom.EditValue,
                                                  "", "", "")
        Else
                ErrorMessage(Me, "تنبية", "عذرا رقم الكود غير صحيح الرجاء اعادة المحاولة")
            End If

        MyBase.SetData()
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_Insert(ByVal Code As String, ByVal AccParent As ULong, ByVal WDVAL As Double, ByVal SafeID As Integer,
                                            TypeID As Int32, CODEID As ULong, BranchID As Integer, DPSVAL As Decimal, Notes As String, IsUpdate As Boolean, AccIDFrom As ULong,
                                              CurrencyID As Integer, PaidFor As String, Phone As String, IDNo As String)
        'Try
        Dim PRM(17) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccParent}
        PRM(2) = New SqlParameter("@WDVAL", SqlDbType.Decimal) With {.Value = WDVAL}
        PRM(3) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(4) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        PRM(5) = New SqlParameter("@CODEID", SqlDbType.BigInt) With {.Value = CODEID}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@DPSVAL", SqlDbType.Decimal) With {.Value = DPSVAL}
        PRM(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(9) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(10) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = AccIDFrom}
        PRM(11) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(12) = New SqlParameter("@MSG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(13) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(14) = New SqlParameter("@PaidFor", SqlDbType.NVarChar, -1) With {.Value = PaidFor}
        PRM(15) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = Phone}
        PRM(16) = New SqlParameter("@IDNo", SqlDbType.NVarChar, -1) With {.Value = IDNo}
        PRM(17) = New SqlParameter("@OPType", SqlDbType.Int) With {.Value = 90}
        RUN_EXUTE_PRO("EMPORCUSTWITHDRAWALTB_Insert", PRM)
        Dim status As Integer = Convert.ToInt32(PRM(12).Value)
        Dim msg As String = Convert.ToString(PRM(13).Value)
        If status = 0 OrElse status = 2 Then
            ErrorMessage(Me, "رسالة تنبيه", msg)
            If status = 2 Then
                EMPORCUSTWITHDRAWALTB_MaxID(LOADTYPE)
            End If
            Exit Sub
        End If
        Print()
        'كود_رسائل الواتساب لسندات الصرف
        Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "CODE " & ":" & Space(1) & WDCode.Text & vbNewLine & "تم سحب مبلغ" & Space(1) & ":" & Space(1) &
                    Cur_Code(CurrencyFrom.Text, WDValue.EditValue, True, "n2") & vbNewLine &
            Cur_Code(CurrencyFrom.Text, WDValue.EditValue, False, "n2") & vbNewLine
        mms &= "من حساب" & Space(1) & ":" & Space(1) & WithdrawalFrom.Text & vbNewLine & "شكرا لتعاملكم معنا"
        WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(WithdrawalFrom.EditValue), mms, True)

            FrmSavedSuccessfully.Show()
        NEWRECORD()
        'Catch ex As Exception
        'MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_MaxID(TypeID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            WDCode.Text = dt.Rows(0)("Code")
            IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Sub SHOW_EMCUSCODE(x, s)
        If Me.IsUpdate = True Then
            LOADSafeID()
            Dim DT As New DataTable
            DT.Clear()
            DT = clsemwdn.SERACH_EMPORCUSTWITHDRAWALTB(x, s)
            If DT.Rows.Count > 0 Then
                WDCode.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                SafeID.EditValue = Convert.ToUInt64(DT.Rows(0)("SafeID"))
                WithdrawalDate.EditValue = DT.Rows(0)("InsertDate")
                LOADCUSTORSAFE()
                WithdrawalFrom.EditValue = DT.Rows(0)("EMPID")
                WDValue.Text = DT.Rows(0)("WDVAL")
            End If
        End If
    End Sub

    Private Sub WithdrawalFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles WithdrawalFrom.QueryPopUp
        If BranchID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADBASICDATATONEWWD", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.PopulateColumns()
                WithdrawalFrom.Properties.Columns("AccID").Visible = False
                WithdrawalFrom.Properties.Columns("SalaryVal").Visible = False
                WithdrawalFrom.Properties.Columns("ValPerMonth").Visible = False
            End If
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
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@Code", WDCode.Text)
            PRM(1) = New SqlParameter("@TypeID", LOADTYPE)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_EMPORCUSTWITHDRAWALTB_SelectByCode", PRM)
            dt.TableName = "EMPORCUSTWITHDRAWALTB"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTEMPWITHDRAWAL
                report.DataSource = ds
                report.DataMember = "EMPORCUSTWITHDRAWALTB"
                        report.XrLabel25.Text = Cur_Code(CurrencyFrom.Text, WDValue.EditValue, False)
                        Dim tool As ReportPrintTool = New ReportPrintTool(report)
                        report.CreateDocument()
                        report.XrLabel25.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.EditValue, False, "n2")
                        report.XrLabel25.Text = Cur_Code(Me.CurrencyFrom.Text, Me.WDValue.EditValue, False, "n2")
                        report.ShowPreview()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
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
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADBASICDATATONEWWD", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "EMPNAME"
                WithdrawalFrom.Properties.ShowHeader = False

            Else
                WithdrawalFrom.Properties.DataSource = dt
            End If
        End If
    End Sub

    Private Sub WithdrawalFrom_TextChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.TextChanged
        SalaryVal = 0.000
        AdvancedPaymentVal = 0.000
        CurrentSalaryVal.EditValue = 0.000
        WithdrawalValue.EditValue = 0.000
        WDValue.EditValue = 0.000
        If BranchID.EditValue = -1 Or BranchID.Text = "" Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 Or BranchID.Text = "" Then
            WithdrawalFrom.ErrorText = "يرجى اختيار الحساب"
            Exit Sub
        End If
        If SafeID.EditValue = -1 Or SafeID.Text = "" Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If WithdrawalFrom.Text <> String.Empty Or WithdrawalFrom.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = WithdrawalFrom.EditValue}
            PR(1) = New SqlParameter("@crunseType", SqlDbType.Int) With {.Value = CurrencyFrom.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("EMPORCUST_GetAccVal(@AccName,@crunseType) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.EditValue = dt.Rows(0)("GetAccVal")
            End If
            Dim DD As New DataTable
            If LOADTYPE = 5 Or LOADTYPE = 7 Then
                DD.Clear()
                DD = RUN_QUARY_QUERY_ONLY("select ID From dbo.EmployeeTb where EMPNAME='" & WithdrawalFrom.Text.Trim & "'")
                If DD.Rows.Count > 0 Then
                    EMPID = DD.Rows(0)("ID")
                End If
            ElseIf LOADTYPE = 6 Or LOADTYPE = 8 Then
                DD.Clear()
                DD = RUN_QUARY_QUERY_ONLY("select ID From dbo.CustomersTB where Custname='" & WithdrawalFrom.Text.Trim & "'")
                If DD.Rows.Count > 0 Then
                    EMPID = DD.Rows(0)("ID")
                End If
            End If
            'LOADEMPDATA()
        End If
        Dim PRR(0) As SqlParameter
        PRR(0) = New SqlParameter("@ACCID", SqlDbType.BigInt) With {.Value = WithdrawalFrom.EditValue}
        Dim dtt As New DataTable
        dtt.Clear()
        dtt = RUN_QUARY_PRO("EMPORCUST_LOADBASICDATATONEWWDANDACCID", PRR)
        If dtt.Rows.Count > 0 Then
            SalaryVal = dtt.Rows(0)("SalaryVal")
            AdvancedPaymentVal = dtt.Rows(0)("ValPerMonth")
        End If
        Dim currentDate As DateTime = DateTime.Now
        Dim dayNumber As Integer = currentDate.Day
        Dim SalarPerDay As Double
        Dim SandANet = SalaryVal - AdvancedPaymentVal
        Dim ValRest As Decimal = SandANet / 30
        Dim SalaryNet As Decimal = ValRest * dayNumber
        SalarPerDay = WithdrawalValue.EditValue + SalaryNet
        Dim dr As Integer = modFORnamber(SalarPerDay)
        CurrentSalaryVal.EditValue = dr
        If CurrentSalaryVal.EditValue <= 0.000 Then
            WDValue.Enabled = False
            BtnSave.Enabled = False
        Else
            WDValue.Enabled = True
            BtnSave.Enabled = True
        End If
    End Sub

    Private Sub CurrencyFrom_Popup(sender As Object, e As EventArgs) Handles CurrencyFrom.Popup
        If CurrencyFrom.Text <> Nothing Or CurrencyFrom.EditValue <> -1 Then
            CurrencyFrom.Properties.View.FocusedRowHandle = 0
        End If
    End Sub

#End Region
End Class