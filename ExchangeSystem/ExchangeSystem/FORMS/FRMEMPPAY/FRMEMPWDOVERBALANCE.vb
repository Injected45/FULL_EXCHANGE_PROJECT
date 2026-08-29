Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMEMPWDOVERBALANCE
    Dim clsempwd As New CLSEMPWITHDRAWAL
    Public IDCode As ULong
    Public LOADTYPE, EMPID As Integer
    Public IsUpdate As Boolean
    Sub DISAPLEDCONTROLS()
        WDCode.Enabled = False
        BranchID.Enabled = False
        SafeID.Enabled = False
        WithdrawalDate.Enabled = False
        WDValue.Enabled = False
        WithdrawalFrom.Enabled = False
        WithdrawalValue.Enabled = False
    End Sub
    Sub ENAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalValue.Enabled = False
        BranchID.Enabled = True
        SafeID.Enabled = True
        WithdrawalDate.Enabled = False
        WDValue.Enabled = True
        WithdrawalFrom.Enabled = True
    End Sub
    Sub NEWRECORD()
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(5)
        IsUpdate = False
        ENAPLEDCONTROLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        WithdrawalDate.EditValue = Date.Now
        BranchID.EditValue = -1
        LOADBRANCH()
        BranchID.Select()
        WithdrawalFrom.EditValue = -1
        BranchID.EditValue = BID
        BtnSave.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        WithdrawalValue.EditValue = 0.000
        Notes.Text = ""
        LOADTYPE = 5
        WDValue.EditValue = 0.000
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
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
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
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
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
    Private Sub WithdrawalFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles WithdrawalFrom.QueryPopUp
        If BranchID.Text <> String.Empty Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADINTOLKPBASEDONACCID", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.PopulateColumns()
                WithdrawalFrom.Properties.Columns("AccID").Visible = False
            End If

        End If
    End Sub

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
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
        NEWRECORD()
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
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 7}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADINTOLKPBASEDONACCID", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "EMCUST"
                WithdrawalFrom.Properties.ShowHeader = False
            Else
                WithdrawalFrom.Properties.DataSource = dt
            End If
        End If
    End Sub
    Sub LOADCUSTORSAFE()
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            SafeID.EditValue = -1
            LOADSafeID()
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADINTOLKPBASEDONACCID", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "EMCUST"
                WithdrawalFrom.Properties.ShowHeader = False
            Else
                WithdrawalFrom.Properties.DataSource = dt
            End If
        End If
    End Sub
    Private Sub WithdrawalFrom_TextChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.TextChanged
        WithdrawalValue.Text = ""
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
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = WithdrawalFrom.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("EMPORCUST_GetAccVal(@AccName) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.Text = dt.Rows(0)("GetAccVal")
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
        End If
    End Sub
#Region "save,update ...etc"
    Sub SHOW_EMCUSCODE(x, s)
        If Me.IsUpdate = True Then
            LOADCUSTORSAFE()
            LOADSafeID()
            Dim DT As New DataTable
            DT.Clear()
            DT = clsempwd.SERACH_EMPORCUSTWITHDRAWALTB(x, s)
            If DT.Rows.Count > 0 Then
                WDCode.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                SafeID.EditValue = Convert.ToUInt64(DT.Rows(0)("SafeID"))
                WithdrawalDate.EditValue = DT.Rows(0)("InsertDate")
                WDValue.Text = DT.Rows(0)("WDVAL")
                WithdrawalFrom.Text = DT.Rows(0)("EMPID").ToString
            End If
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


        GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, 1)
        If LOADTYPE = 5 Or LOADTYPE = 6 Then
            If WDValue.EditValue > SAFEVAL Then
                XtraMessageBox.Show(lookFeelError, "القيمة المصروفة لا يجب أن تكون أكبر من قيمة الخزنة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        Dim OPTYPE As Integer
        If LOADTYPE = 5 Then
            OPTYPE = 38
        ElseIf LOADTYPE = 6 Then
            OPTYPE = 39
        ElseIf LOADTYPE = 7 Then
            OPTYPE = 40
        ElseIf LOADTYPE = 8 Then
            OPTYPE = 41
        End If
        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""
        If LOADTYPE = 5 Then
            MOTYPE = "سحب من حساب الموظف"
            MOTYPE2 = "سحب من حساب الموظف" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, 0, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, DefaultCurrency,
                                                  "", "", "")
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
        NEWRECORD()
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
                        Dim tool As ReportPrintTool = New ReportPrintTool(report)
                        report.CreateDocument()
                        ''  report.XrLabel2.Text = FRMADDINCOME.CurrencyFrom.Text
                        report.XrLabel93.Text = Cur_Code("دينار ليبي", Me.WDValue.Text, True, "n2")
                        report.XrLabel25.Text = Cur_Code("دينار ليبي", Me.WDValue.Text, False, "n2")
                        report.ShowPreview()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub UPDATERECORD()
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
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
        If WDValue.EditValue > Val(WithdrawalValue.Text) Then
            WDValue.ErrorText = "القيمة لا يجب أن تكون أكبر من قيمة الحساب"
            Exit Sub
        End If
        If Val(WithdrawalValue.Text) <= 0.000 Then
            WithdrawalValue.ErrorText = "الحساب لا يوجد به قيمة كافية"
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
        End If
        Dim MOTYPE As String = ""
        Dim MOTYPE2 As String = ""
        If LOADTYPE = 5 Then
            MOTYPE = "معالجة خطأ سحب من حساب موظف"
            MOTYPE2 = "معالجة خطأ سحب من حساب الموظف" & Space(1) & WithdrawalFrom.Text
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(WDCode.Text.Trim, 0, WDValue.Text.Trim, SafeID.EditValue, LOADTYPE, IDCode, BranchID.EditValue, 0.000,
                                                  Notes.Text.Trim, IsUpdate, WithdrawalFrom.EditValue, DefaultCurrency,
                                                  "", "", "")
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        NEWRECORD()
        FRMVIEWEMPWDOVERBALANCE.GCRole.DataSource = Nothing
        FRMVIEWEMPWDOVERBALANCE.GVRole.Columns.Clear()
        FRMVIEWEMPWDOVERBALANCE.ShowDialog()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        WithdrawalFrom.Properties.DataSource = Nothing
        WithdrawalFrom.EditValue = -1
        WithdrawalValue.Text = ""
        WithdrawalValue.EditValue = -1

        If BranchID.EditValue = -1 Or BranchID.Text = "" Then
            BranchID.ErrorText = "يرجى اختيار الفرع"
            Exit Sub
        End If
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            LOADSafeID()
            SafeID.EditValue = UserAccID
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = LOADTYPE}
            PR(2) = New SqlParameter("@IsUpdate", SqlDbType.Int) With {.Value = IsUpdate}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EMPORCUST_LOADINTOLKPBASEDONACCID", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalFrom.Properties.DataSource = dt
                WithdrawalFrom.Properties.ValueMember = "AccID"
                WithdrawalFrom.Properties.DisplayMember = "EMCUST"
                WithdrawalFrom.Properties.ShowHeader = False
            Else
                WithdrawalFrom.Properties.DataSource = dt
            End If
        End If
    End Sub

    Private Sub WithdrawalFrom_EditValueChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.EditValueChanged
        WithdrawalValue.Text = ""
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
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = WithdrawalFrom.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("EMPORCUST_GetAccVal(@AccName) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                WithdrawalValue.Text = dt.Rows(0)("GetAccVal")
            End If
        End If
    End Sub
#End Region
End Class