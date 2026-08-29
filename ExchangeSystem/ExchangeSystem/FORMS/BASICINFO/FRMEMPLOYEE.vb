Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.DocumentServices.ServiceModel.DataContracts
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraReports.UI

Public Class FRMEMPLOYEE
    Dim clse As New CLSEMPLOYEE
    Public CLSA As New CLSAccount
    Public AcID, IDCode, AccCode, AccEm, AcIDID As ULong
    Public Property EMBID As Integer
    Public Property EMNAME As String
    Public StID, AccLine, AccCat As Integer
    Public Property X As String
    Public Property AccNew As String
    Public IsUpdate, UpdateBySalary As Boolean
    Public msgST, DataBaseType As Int16
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(FRmIDsql, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then LayoutControlItem16.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem16.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub DISAPLETOOLS()
        CodeID.Enabled = False
        EMPNAME.Enabled = False
        NATNUMBER.Enabled = False
        CLASSIFICATION.Enabled = False
        PHONE1.Enabled = False
        PHONE2.Enabled = False
        BIRTHDATE.Enabled = False
        EMPDATE.Enabled = False
        PassportNo.Enabled = False
        Nationality.Enabled = False
        BranchID.Enabled = False
        CertificateType.Enabled = False
        EMail.Enabled = False
        Debit.Enabled = False
        Credit.Enabled = False
        PictureEdit1.Enabled = False
        BtnPrint.Enabled = True
    End Sub
    Sub ENABLETOOLS()
        EMPNAME.Enabled = True
        NATNUMBER.Enabled = True
        CLASSIFICATION.Enabled = True
        PHONE1.Enabled = True
        PHONE2.Enabled = True
        BIRTHDATE.Enabled = True
        EMPDATE.Enabled = True
        PassportNo.Enabled = True
        Nationality.Enabled = True
        BranchID.Enabled = True
        CertificateType.Enabled = True
        EMail.Enabled = True
        Debit.Enabled = True
        Credit.Enabled = True
        PictureEdit1.Enabled = True
        BtnPrint.Enabled = False
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit2")
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
        If DataBaseType = 1 Then
            If dt.Rows.Count > 0 Then
                BranchID.Enabled = dt.Rows(0)("Can_branch")
                'SafeID.Enabled = dt.Rows(0)("Can_safID")
                'SafeID.EditValue = UserAccID
            Else
                BranchID.Enabled = False
                'SafeID.Enabled = False
                'SafeID.EditValue = UserAccID
            End If
        End If
    End Sub
    Public Sub LOADCLASSIFICATION()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("EmployeeClassificationTb_SelectAll")
        If DT.Rows.Count > 0 Then
            CLASSIFICATION.Properties.DataSource = DT
            CLASSIFICATION.Properties.DisplayMember = "ECNAME"
            CLASSIFICATION.Properties.ValueMember = "ID"
            CLASSIFICATION.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub LOADNATIONALITY()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("NationalityTb_SelectAll")
        If DT.Rows.Count > 0 Then
            Nationality.Properties.DataSource = DT
            Nationality.Properties.DisplayMember = "NATNAME"
            Nationality.Properties.ValueMember = "ID"
            Nationality.Properties.ShowHeader = False
        End If
    End Sub
    Sub NewRecord()
        IsUpdate = False
        BranchID.Enabled = True
        CodeID.Enabled = False
        ENABLETOOLS()
        LOADBRANCH()
        LOADCLASSIFICATION()
        LOADNATIONALITY()
        EMPNAME.Enabled = True
        BranchID.EditValue = -1
        IsActiveTG.IsOn = True
        EMPNAME.Text = String.Empty
        EMPNAME.Select()
        NATNUMBER.Text = String.Empty
        CLASSIFICATION.EditValue = -1
        PHONE1.Text = String.Empty
        PHONE2.Text = String.Empty
        PHONE1.Text = String.Empty
        PHONE2.Text = String.Empty
        Nationality.EditValue = -1
        CertificateType.Text = String.Empty
        PassportNo.Text = String.Empty
        EMail.Text = String.Empty
        BIRTHDATE.EditValue = Date.Now
        EMPDATE.EditValue = Date.Now
        SalaryVal.EditValue = 0.000
        Debit.EditValue = 0.000
        Credit.EditValue = 0.000
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        'BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Caption = "طباعة كرت مالي"
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
        BranchID.EditValue = BID
        LOADBANK()
        BankID.EditValue = -1
        BBranchID.EditValue = -1
        Jobgrade.EditValue = -1
        CanDebit.SelectedIndex = 0
        AccOwner.Text = String.Empty
        BankNo.Text = String.Empty
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
        LoadToControlar(Jobgrade, "Companies_Crud", "CompanyName1", "ID", PR)
    End Sub
    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub SetData()
        Try


            If IsUpdate = False Then

                If NATNUMBER.Text.Trim <> String.Empty Then
                    Dim PRM(0) As SqlParameter
                    PRM(0) = New SqlParameter("@NATNUMBER", SqlDbType.BigInt) With {.Value = NATNUMBER.Text}
                    Dim DTT As New DataTable
                    DTT.Clear()
                    If DataBaseType = 1 Then
                        DTT = RUN_QUARY_PRO("EmployeeTb_SelectNatNum", PRM)
                    Else
                        DTT = RUN_QUARY_PRO("CONDB_EmployeeTb_SelectNatNum", PRM)
                    End If
                    If DTT.Rows.Count > 0 Then
                        NATNUMBER.ErrorText = "الرقم موجود مسبقا"
                        Exit Sub
                    End If
                End If
                If EMPNAME.Text = String.Empty Then
                    EMPNAME.ErrorText = "يرجى إخال اسم الموظف"
                    Exit Sub
                End If
                If NATNUMBER.Text <> "" Then
                    If NATNUMBER.Text.Length <> 12 Then
                        NATNUMBER.ErrorText = "الرقم الوطني يجب أن يتكون من 12 رقم"
                        Exit Sub
                    End If
                End If
                If CLASSIFICATION.EditValue = -1 Then
                    CLASSIFICATION.ErrorText = "يرجى اختيار التصنيف"
                    Exit Sub
                End If
                If BranchID.EditValue = -1 Then
                    BranchID.ErrorText = "يرجى اختيار الفرع"
                    Exit Sub
                End If
                If SalaryVal.EditValue <= 0.000 Then
                    SalaryVal.ErrorText = "قيمة الراتب لا يجب أن تكون صفر أو أقل"
                    Exit Sub
                End If

            End If



            clse.INSERTTB__EMP(CodeID.Text, EMPNAME.Text.Trim, NATNUMBER.Text.Trim, CLASSIFICATION.EditValue, PHONE1.Text.Trim, PHONE2.Text.Trim,
                               BIRTHDATE.EditValue, EMPDATE.EditValue,
                               PassportNo.Text.Trim, Nationality.EditValue, CertificateType.Text.Trim, SalaryVal.EditValue, EMail.Text.Trim,
                               IsActiveTG.IsOn, BranchID.EditValue, IsUpdate, EMBID, BankID.EditValue, BBranchID.EditValue, AccOwner.Text, BankNo.Text, CanDebit.SelectedIndex, Jobgrade.EditValue, BankSalaryCalc.SelectedIndex)

            If msgST = 1 Then
                MyBase.SetData()
            End If
        Catch ex As Exception
            ErrorMessage2("", ex.Message)
        End Try
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If Me.IsUpdate = True Then
            If IsActiveTG.EditValue = False Then
                If DataBaseType = 1 Then
                    Dim CHEKPETYCASH As New DataTable
                    CHEKPETYCASH.Clear()
                    CHEKPETYCASH = CHECKEMPHASPETTYCASH(EMBID)
                    If CHEKPETYCASH.Rows.Count > 0 Then
                        ErrorMessage(Me, "رسالة تنبيه", "الموظف عليه عهدة ويجب تسويتها قبل إيقاف تنشيطه")
                        IsActiveTG.EditValue = True
                        Exit Sub
                    End If
                    Dim PR(0) As SqlParameter
                    PR(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMBID}

                    Dim Disc As New DataTable
                    Disc.Clear()
                    Disc = RUN_FUNCTION_PARM("EMP_GetDiscTtoals(@EMPID) AS GetDiscTtoals", PR)
                    If Disc.Rows.Count > 0 Then
                        If Disc.Rows(0)("GetDiscTtoals") > 0 Then
                            ErrorMessage(Me, "رسالة تنبيه", "الموظف عليه خصم ويجب تسويته قبل إيقاف تنشيطه")
                            IsActiveTG.EditValue = True
                            Exit Sub
                        End If
                    End If
                    Dim PRM(0) As SqlParameter
                    PRM(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMBID}
                    Dim ADVPMNT As New DataTable
                    ADVPMNT.Clear()
                    ADVPMNT = RUN_FUNCTION_PARM("EMP_GetADVPMNTTtoalsIndivdual(@EMPID) AS ADVPMNTTtoals", PRM)
                    If ADVPMNT.Rows.Count > 0 Then
                        If ADVPMNT.Rows(0)("ADVPMNTTtoals") > 0 Then
                            ErrorMessage(Me, "رسالة تنبيه", "الموظف عليه سلفة ويجب تسويتها قبل إيقاف تنشيطه")
                            IsActiveTG.EditValue = True
                            Exit Sub
                        End If
                    End If
                End If
            End If
            If EMPNAME.Text = String.Empty Then
                EMPNAME.ErrorText = "يرجى إخال اسم الموظف"
                Exit Sub
            End If
            If NATNUMBER.Text.Length <> 12 Then
                NATNUMBER.ErrorText = "الرقم الوطني يجب أن يتكون من 12 رقم"
                Exit Sub
            End If
            If CLASSIFICATION.EditValue = -1 Then
                CLASSIFICATION.ErrorText = "يرجى اختيار التصنيف"
                Exit Sub
            End If
            If Jobgrade.EditValue = -1 Then
                Jobgrade.ErrorText = "يرجى اختيار الشركة"
                Exit Sub
            End If
            If UpdateBySalary = False Then
                CodeID.Select()
                clse.INSERTTB__EMP(CodeID.Text, EMPNAME.Text.Trim, NATNUMBER.Text.Trim, CLASSIFICATION.EditValue, PHONE1.Text.Trim, PHONE2.Text.Trim, BIRTHDATE.EditValue, EMPDATE.EditValue,
                               PassportNo.Text.Trim, Nationality.EditValue, CertificateType.Text.Trim, SalaryVal.EditValue, EMail.Text.Trim, IsActiveTG.IsOn, BranchID.EditValue, IsUpdate,
                               EMBID, BankID.EditValue, BBranchID.EditValue, AccOwner.Text, BankNo.Text, CanDebit.SelectedIndex, Jobgrade.EditValue, BankSalaryCalc.SelectedIndex)
            ElseIf UpdateBySalary = True Then
                clse.UPDATETBSALARY_EMP(CodeID.Text, SalaryVal.EditValue)
                FRMSALARYCALCULATION.LoadData()
                IsUpdate = False
                Me.Close()
            End If
        End If
        If msgST = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub
    Sub SHOW_EMP(x)
        IsUpdate = True
        BranchID.Enabled = False
        UpdateBySalary = False
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clse.SERACH_EMPLOYEE(x)
            If DT.Rows.Count > 0 Then
                EMPNAME.Text = DT.Rows(0)("EMPNAME").ToString
                CodeID.Text = DT.Rows(0)("CODE").ToString
                NATNUMBER.Text = DT.Rows(0)("NATNUMBER").ToString
                CLASSIFICATION.EditValue = DT.Rows(0)("CLASSIFICATION")
                PHONE1.Text = DT.Rows(0)("PHONE1").ToString
                PHONE2.Text = DT.Rows(0)("PHONE2").ToString
                BIRTHDATE.EditValue = DT.Rows(0)("BIRTHDATE")
                EMPDATE.EditValue = DT.Rows(0)("EMPDATE")
                PassportNo.Text = DT.Rows(0)("PassportNo").ToString
                Nationality.EditValue = DT.Rows(0)("Nationality")
                CertificateType.EditValue = DT.Rows(0)("CertificateType").ToString
                SalaryVal.EditValue = DT.Rows(0)("SalaryVal")
                EMail.Text = DT.Rows(0)("EMail").ToString
                Debit.EditValue = DT.Rows(0)("Debit")
                Credit.EditValue = DT.Rows(0)("Credit")
                CanDebit.SelectedIndex = DT.Rows(0)("CanDebit")
                Jobgrade.EditValue = DT.Rows(0)("Jobgrade")
                IsActiveTG.IsOn = DT.Rows(0)("ISACTIVE")
                BranchID.EditValue = DT.Rows(0)("BranchID")
                EMBID = DT.Rows(0)("ID")
                BankID.EditValue = DT.Rows(0)("BankID")
                BBranchID.EditValue = DT.Rows(0)("BBranchID")
                AccOwner.Text = DT.Rows(0)("AccOwner").ToString
                BankNo.Text = DT.Rows(0)("BankNo").ToString
                BankSalaryCalc.SelectedIndex = DT.Rows(0)("BankSalaryCalc")
            End If
        End If
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = clse.EMPACC_EMPLOYEE(EMPNAME.Text)
        If DTT.Rows.Count > 0 Then
            AccEm = DTT.Rows(0)("AccID")
        End If
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@EMPID", SqlDbType.BigInt) With {.Value = EMBID}
        Dim DTT1 As New DataTable
        DTT1.Clear()
        DTT1 = RUN_QUARY_PRO("TB_Users_LOADUSERBASEDONEMPID", PRM)
        If DTT1.Rows.Count > 0 Then
            EMPNAME.Enabled = False
        Else
            EMPNAME.Enabled = True
        End If
    End Sub
    Sub SHOW_EMPBID(EMID)
        Try


            IsUpdate = True
            UpdateBySalary = False
            BranchID.Enabled = False
            If Me.IsUpdate = True Then
                Dim DT As New DataTable
                DT.Clear()
                DT = clse.SERACHBYID_EMPLOYEE(EMID)
                If DT.Rows.Count > 0 Then
                    EMPNAME.Text = DT.Rows(0)("EMPNAME").ToString
                    AccNew = "عُهد" & Space(1) & DT.Rows(0)("EMPNAME").ToString
                    CodeID.Text = DT.Rows(0)("CODE").ToString
                    NATNUMBER.Text = DT.Rows(0)("NATNUMBER").ToString
                    CLASSIFICATION.EditValue = DT.Rows(0)("CLASSIFICATION")
                    PHONE1.Text = DT.Rows(0)("PHONE1").ToString
                    PHONE2.Text = DT.Rows(0)("PHONE2").ToString
                    BIRTHDATE.EditValue = DT.Rows(0)("BIRTHDATE")
                    EMPDATE.EditValue = DT.Rows(0)("EMPDATE")
                    PassportNo.Text = DT.Rows(0)("PassportNo").ToString
                    Nationality.EditValue = DT.Rows(0)("Nationality")
                    CanDebit.SelectedIndex = DT.Rows(0)("CanDebit")
                    Jobgrade.EditValue = DT.Rows(0)("Jobgrade")
                    CertificateType.EditValue = DT.Rows(0)("CertificateType").ToString
                    SalaryVal.EditValue = DT.Rows(0)("SalaryVal")
                    EMail.Text = DT.Rows(0)("EMail").ToString
                    'Debit.EditValue = DT.Rows(0)("Debit")
                    'Credit.EditValue = DT.Rows(0)("Credit")
                    IsActiveTG.IsOn = DT.Rows(0)("ISACTIVE")
                    BranchID.EditValue = DT.Rows(0)("BranchID")
                    EMBID = DT.Rows(0)("ID")
                    BankID.EditValue = DT.Rows(0)("BankID")
                    BBranchID.EditValue = DT.Rows(0)("BBranchID")
                    AccOwner.Text = DT.Rows(0)("AccOwner").ToString
                    BankNo.Text = DT.Rows(0)("BankNo").ToString
                    BankSalaryCalc.SelectedIndex = DT.Rows(0)("BankSalaryCalc")
                    Dim PR(0) As SqlParameter
                    PR(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMID}
                    Dim DTT1 As New DataTable
                    DTT1.Clear()
                    DTT1 = RUN_QUARY_PRO("EmployeeTb_GetEMPBounsTotal", PR)
                    If DTT1.Rows.Count > 0 Then
                        TotalConastanceVal.EditValue = DTT1.Rows(0)("TotalConastanceVal")
                        TotalTempVal.EditValue = DTT1.Rows(0)("TotalTempVal")
                    End If

                    Dim PRM(0) As SqlParameter
                    PRM(0) = New SqlParameter("@EMPID", SqlDbType.BigInt) With {.Value = EMBID}
                    Dim DTT As New DataTable
                    DTT.Clear()
                    DTT = RUN_QUARY_PRO("TB_Users_LOADUSERBASEDONEMPID", PRM)
                    If DTT.Rows.Count > 0 Then
                        EMPNAME.Enabled = False
                    Else
                        EMPNAME.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            ErrorMessage2("EmployeeTb_GetEMPBounsTotal", ex.Message)
        End Try
    End Sub
    Public Overrides Sub Remove()
        MyBase.Remove()
    End Sub
    Private Sub CLASSIFICATION_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CLASSIFICATION.ButtonClick
        If e.Button.Index = 1 Then
            FRMEmployeeClassification.ShowDialog()
        End If
    End Sub
    Private Sub PictureEdit1_Click(sender As Object, e As EventArgs) Handles PictureEdit1.Click
        FRMVIEWEMPLOYEE.ShowDialog()
    End Sub


    Private Sub FRMEMPLOOYE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        CHECKBUTTONS()
        lodePreportes()

        If IsUpdate = False Then
            NewRecord()
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub FRMEMPLOYEE_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        ENABLETOOLS()
        IsUpdate = False
        NewRecord()
    End Sub
    Private Sub CLASSIFICATION_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CLASSIFICATION.QueryPopUp
        CLASSIFICATION.Properties.PopulateColumns()
        CLASSIFICATION.Properties.Columns("ID").Visible = False
    End Sub

    Private Sub BankID_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BankID.ButtonClick
        If e.Button.Index = 1 Then
            ExBanks.NEWRECORD()
            ExBanks.ShowDialog()
        End If
    End Sub

    Private Sub BBranchID_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BBranchID.ButtonClick
        If e.Button.Index = 1 Then
            FRMEXBBRANCH.ShowDialog()
        End If
    End Sub


    Private Sub Nationality_QueryPopUp(sender As Object, e As CancelEventArgs) Handles Nationality.QueryPopUp
        Nationality.Properties.PopulateColumns()
        Nationality.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub Nationality_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles Nationality.ButtonClick
        If e.Button.Index = 1 Then
            FRMNATIONALITY.ShowDialog()
        End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If IsUpdate = False Then
            If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
                If DataBaseType = 1 Then
                    CodeID.Text = BranchID.EditValue & "0" & "1" & "0" & GETIDMAX_Pro("EmployeeTb", "IDCode") + 1
                Else
                    CodeID.Text = BranchID.EditValue & "0" & "3" & "0" & GETMAXID("ContractDB.dbo.EmployeeTb", "ID") + 1  '' الرقم 3 يرمز لموظفي المقاولات
                End If

            End If
        End If
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If IsUpdate = True Then
            If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
                CodeID.Text = String.Empty
                If DataBaseType = 1 Then
                    CodeID.Text = BranchID.EditValue & "0" & "1" & "0" & EMBID
                Else
                    CodeID.Text = BranchID.EditValue & "0" & "3" & "0" & EMBID
                End If
            End If
        End If
    End Sub
    'Public IsActiveValue As Boolean
    Public Overrides Sub Print()

        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@EmpID", SqlDbType.Int) With {.Value = EMBID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("SalaryCalculationTb_MoneyCard", prm)
            If dt.Rows.Count Then
                Dim report As New RPTMoneyCard
                report.DataSource = dt
                report.DataMember = "EmployeeTb"
                'report.IsActiveValue = Convert.ToBoolean(IsActiveTG.EditValue)
                Dim tool As ReportPrintTool = New ReportPrintTool(report)

                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
        MyBase.Print()
    End Sub
    Public Sub LOADBANK()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 6}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExBanksTb_CRUD", PR)
        BankID.Properties.DataSource = DT
        BankID.Properties.ValueMember = "ID"
        BankID.Properties.DisplayMember = "BankName"
        BankID.Properties.ShowHeader = False
    End Sub
    Public Sub LOADBBRANCH()
        BBranchID.Properties.DataSource = Nothing
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID.EditValue}
        PR(1) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 6}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExBBranchTb_CRUD", PR)
        If DT.Rows.Count > 0 Then
            BBranchID.Properties.DataSource = DT
            BBranchID.Properties.DisplayMember = "BranchName"
            BBranchID.Properties.ValueMember = "ID"
            BBranchID.Properties.PopulateColumns()
            BBranchID.Properties.Columns("ID").Visible = False
            BBranchID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub BankID_EditValueChanged(sender As Object, e As EventArgs) Handles BankID.EditValueChanged
        If BankID.EditValue IsNot Nothing Then
            LOADBBRANCH()
            BBranchID.EditValue = -1
        End If
    End Sub
End Class