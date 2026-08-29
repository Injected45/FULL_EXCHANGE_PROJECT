Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI

Public Class FRMCUSTOMER
    Dim clscust As New CLSCUSTOMER
    Public Property InsDate As Date
    Public CustID As Integer
    Public Property IsUpdate As Boolean
    Public msgST As Int16
    Public Overrides Sub CHECKBUTTONS()
        MyBase.CHECKBUTTONS()
    End Sub
    Public Sub LOADNATIONALITY()
        LoadToControlar(Nationality, "NationalityTb_SelectAll", "NATNAME", "ID", Nothing)
    End Sub
    'Public Sub LOADBANK()
    '    LoadToControlar(AccID, "BanksTb_SelectAll", "BankName", "ID", Nothing)
    'End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(13, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    'Sub LOADBRANCH()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
    '    If dt.Rows.Count > 0 Then
    '        BranchID.Properties.DataSource = dt
    '        BranchID.Properties.ValueMember = "DBRID"
    '        BranchID.Properties.DisplayMember = "BName"
    '        BranchID.Properties.ShowHeader = false
    '    End If
    'End Sub
    Private Sub FRMCUSTOMER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

#Region "Save, Update"
    Public Overrides Sub SetData()

        If CUSTNAME.Text = String.Empty Then
            CUSTNAME.ErrorText = "هذ الحقل مطلوب"
            CUSTNAME.Select()
            Return
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "هذ الحقل مطلوب"
            BranchID.Select()
            Return
        End If
        'If NatNumber.Text.Length <> 12 Then
        '    NatNumber.ErrorText = "يجب أن يتكون الرقم الوطني من 12 رقم"
        '    NatNumber.Select()
        '    Return
        'End If
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = clscust.CustomersTb_CheckPHoneExist(PHONE1.Text.Trim, PHONE2.Text.Trim)
        'If DT.Rows.Count > 0 Then
        '    ErrorMessage(Me, "رسالة خطأ", "رقم الهاتف موجود مسبقاً")
        '    Exit Sub
        'End If

        'If sand_Group_ID.EditValue = True Then
        '    WATSAPPMsAG(FRMCUSTOMER.PHONE1.Text.Trim, mms, True)
        'Else
        '    WATSAPPMsAG(Group_ID_fro_watssap, mms, True)
        'End If


        clscust.CUSTOMER_INSERT(Date.Now, CodeID.Text, CUSTNAME.Text.Trim, PHONE1.Text.Trim, IDNo.Text.Trim, CUSTADDRESS.Text.Trim, BranchID.EditValue, IsUpdate, 0, CanDebit.SelectedIndex,
                                ISHidden.IsOn, NatNumber.Text.Trim, Email.Text.Trim, MartialStaus.SelectedIndex, UserID, Password.Text.Trim, ParentName.Text.Trim, Nationality.EditValue,
                                BIRTHDATE.EditValue, EmpReg.Text, AccNo.Text.Trim, AccID.Text.Trim, Group_ID_fro_watssap.Text, sand_Group_ID.EditValue, AccType.SelectedIndex, Registry1.Text.Trim,
Registry2.Text.Trim, Ownername.Text.Trim)
        If msgST = 1 Then
            MyBase.SetData()
        End If

    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWCUSTOMERS.ShowDialog()
    End Sub

    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then

            If CUSTNAME.Text = String.Empty Then
                CUSTNAME.ErrorText = "هذ الحقل مطلوب"
                CUSTNAME.Select()
                Return
            End If
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "هذ الحقل مطلوب"
                BranchID.Select()
                Return
            End If
            Dim DT As New DataTable
            DT.Clear()
            'DT = clscust.CustomersTb_CheckPHoneExistUpdate(CustID)
            'If DT.Rows.Count > 0 Then
            '    ErrorMessage(Me, "رسالة خطأ", "رقم الهاتف موجود مسبقاً")
            '    Exit Sub
            'End If
            clscust.CUSTOMER_INSERT(Date.Now, CodeID.Text, CUSTNAME.Text.Trim, PHONE1.Text.Trim, IDNo.Text.Trim, CUSTADDRESS.Text.Trim, BranchID.EditValue, IsUpdate, 0, CanDebit.SelectedIndex,
                                ISHidden.IsOn, NatNumber.Text.Trim, Email.Text.Trim, MartialStaus.SelectedIndex, UserName.Text.Trim, Password.Text.Trim, ParentName.Text.Trim, Nationality.EditValue,
                                BIRTHDATE.EditValue, EmpReg.Text, AccNo.Text.Trim, AccID.Text.Trim, Group_ID_fro_watssap.Text, sand_Group_ID.EditValue, AccType.SelectedIndex, Registry1.Text.Trim,
Registry2.Text.Trim, Ownername.Text.Trim)
        End If
        If msgST = 1 Then
            MyBase.UPDATERECORD()
        End If
    End Sub
#End Region
    Sub NEWRECORD()
        New_Controlrs(Me)
        sand_Group_ID.EditValue = False
        Group_ID_fro_watssap.ReadOnly = False
        IsUpdate = False
        LOADBRNCHDIERCT(BranchID)
        LOADNATIONALITY()
        'LOADBANK()
        EmpReg.Text = GetUserName
        MartialStaus.SelectedIndex = 0
        Email.Text = String.Empty
        BranchID.EditValue = BID
        CUSTNAME.Text = String.Empty
        PHONE1.Text = String.Empty
        IDNo.Text = String.Empty
        CUSTADDRESS.Text = String.Empty
        IsActiveTG.IsOn = True
        ISHidden.IsOn = False
        CodeID.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        'BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        CanDebit.SelectedIndex = 1
        NatNumber.Text = String.Empty
        LiecenceNo.Enabled = False
        Ownername.Enabled = False
        Registry1.Enabled = False
        Registry2.Enabled = False
        AccType.SelectedIndex = 0
        LiecenceNo.Text = String.Empty
        Ownername.Text = String.Empty
        Registry1.Text = String.Empty
        Registry2.Text = String.Empty
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Sub SHOW_CUST(x)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clscust.CustomersTb_Select(x)
            If DT.Rows.Count > 0 Then

                CodeID.Text = DT.Rows(0)("Code").ToString
                CUSTNAME.Text = DT.Rows(0)("CustName").ToString
                PHONE1.Text = DT.Rows(0)("PHONE1").ToString
                IDNo.Text = DT.Rows(0)("PHONE2").ToString
                CUSTADDRESS.Text = DT.Rows(0)("CustmAddress").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                InsDate = DT.Rows(0)("InsertDate")
                CustID = DT.Rows(0)("ID")
                CanDebit.SelectedIndex = DT.Rows(0)("CanDebit")
                ISHidden.IsOn = DT.Rows(0)("HiddenAccount")
                NatNumber.Text = DT.Rows(0)("NatNumber")
                Email.Text = DT.Rows(0)("Email").ToString
                MartialStaus.SelectedIndex = DT.Rows(0)("MartialStaus")
                UserName.Text = DT.Rows(0)("UserName").ToString
                Password.Text = DT.Rows(0)("Password").ToString
                ParentName.Text = DT.Rows(0)("ParentName").ToString
                Nationality.EditValue = DT.Rows(0)("Nationality")
                BIRTHDATE.EditValue = DT.Rows(0)("BIRTHDATE")
                EmpReg.Text = SafeToString(DT.Rows(0)("EmpReg"))
                AccNo.Text = SafeToString(DT.Rows(0)("AccNo"))
                AccID.Text = SafeToString(DT.Rows(0)("BankID"))
                Group_ID_fro_watssap.Text = SafeToString(DT.Rows(0)("Group_ID_fro_watssap"))
                sand_Group_ID.EditValue = SafeToString(DT.Rows(0)("sand_Group_ID"))
                AccType.SelectedIndex = DT.Rows(0)("AccType")
                Registry1.Text = SafeToString(DT.Rows(0)("Registry1").ToString)
                Registry2.Text = SafeToString(DT.Rows(0)("Registry2").ToString)
                Ownername.Text = SafeToString(DT.Rows(0)("Ownername").ToString)
            End If
        End If
    End Sub

    Private Sub FRMCUSTOMER_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IsUpdate = False
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.EditValue Is Nothing OrElse BranchID.EditValue.ToString() = "" OrElse BranchID.EditValue = -1 Then
            Exit Sub
        End If

        ' فقط عند الإضافة يولد كود جديد
        If Not IsUpdate Then
            CodeID.Text = $"{BranchID.EditValue}020{GETMAXID("CustomersTb", "ID") + 1}"
            'ElseIf IsUpdate = True Then
            '    CodeID.Text = ""
            '    CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
        End If
    End Sub


    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.EditValue Is Nothing OrElse BranchID.EditValue.ToString() = "" OrElse BranchID.EditValue = -1 Then
            Exit Sub
        End If

        ' فقط عند الإضافة يولد كود جديد
        If Not IsUpdate Then
            CodeID.Text = $"{BranchID.EditValue}020{GETMAXID("CustomersTb", "ID") + 1}"
            'ElseIf IsUpdate = True Then
            '    CodeID.Text = ""
            '    CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
        End If
    End Sub

    Private Sub BranchID_ListChanged(sender As Object, e As ListChangedEventArgs) Handles BranchID.ListChanged
        If BranchID.EditValue Is Nothing OrElse BranchID.EditValue.ToString() = "" OrElse BranchID.EditValue = -1 Then
            Exit Sub
        End If

        ' فقط عند الإضافة يولد كود جديد
        If Not IsUpdate Then
            CodeID.Text = $"{BranchID.EditValue}020{GETMAXID("CustomersTb", "ID") + 1}"
        ElseIf IsUpdate = True Then
            CodeID.Text = ""
            CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
        End If
        'If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
        '    If IsUpdate = false Then
        '        CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & GETMAXID("CustomersTb", "ID") + 1
        '    ElseIf IsUpdate = true Then
        '        CodeID.Text = ""
        '        CodeID.Text = BranchID.EditValue & "0" & "2" & "0" & CustID
        '    End If
        'End If
    End Sub
    Public Overrides Sub Print()
        PRINTRE()
        MyBase.Print()
    End Sub
    Public Sub PRINTRE()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", CodeID.Text)
        Dim dt As DataTable = RUN_QUARY_PRO("CustomersTb_Select", PRM)
        dt.TableName = "CustomersTb_Select"
        Dim ds As New DataSet
        ds.Tables.Add(dt)
        Dim report As New addcustmerRPT
                report.DataSource = ds
                report.XrLabel47.Text = BranchID.Text
                report.DataMember = "CustomersTb_Select"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                If AccType.SelectedIndex = 1 Then
                    report.XrLabel58.Visible = True
                    report.XrLabel26.Visible = True
                    report.XrLabel61.Visible = True
                    report.XrLabel62.Visible = True
                    report.XrLabel63.Visible = True
                    report.XrLabel64.Visible = True
                    report.XrLabel65.Visible = True
                    report.XrLabel66.Visible = True
                    report.XrLabel67.Visible = True
                    report.XrLabel68.Visible = True
                Else
                    report.XrLabel58.Visible = False
                    report.XrLabel26.Visible = False
                    report.XrLabel61.Visible = False
                    report.XrLabel62.Visible = False
                    report.XrLabel63.Visible = False
                    report.XrLabel64.Visible = False
                    report.XrLabel65.Visible = False
                    report.XrLabel66.Visible = False
                    report.XrLabel67.Visible = False
                    report.XrLabel68.Visible = False
                End If
                report.CreateDocument()
                report.ShowPreview()
    End Sub

    Private Sub AccType_EditValueChanged(sender As Object, e As EventArgs) Handles AccType.EditValueChanged
        If AccType.SelectedIndex = 1 Then

            LiecenceNo.Enabled = True
            Ownername.Enabled = True
            Registry1.Enabled = True
            Registry2.Enabled = True
        Else

            LiecenceNo.Enabled = False
            Ownername.Enabled = False
            Registry1.Enabled = False
            Registry2.Enabled = False
        End If
    End Sub
End Class