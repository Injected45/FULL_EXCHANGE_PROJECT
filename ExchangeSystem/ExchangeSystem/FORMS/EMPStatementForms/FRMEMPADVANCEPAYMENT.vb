Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraReports.UI

Public Class FRMEMPADVANCEPAYMENT
    Dim clsemadv As New CLSEMPADVANCEPAYMENT
    Public EmpAccID, ADVPMTACCID As ULong
    Public IsUpdate As Boolean
    Sub DISAPLEDCONTROLS()
        IsUpdate = True
        CodeID.Enabled = False
        InsertDate.Enabled = False
        ValPerMonth.Enabled = False
        BRANCHID.Enabled = False
        CURRENCYID.Enabled = False
        EMPID.Enabled = False
        OverAllVal.Enabled = False
        Notes.Enabled = False
        RepaymentPeroid.Enabled = False
        BtnEdit.Caption = "إرجاع قيمة سلفة"
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(69, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub ENAPLEDCONTROLS()
        CodeID.Enabled = False
        InsertDate.Enabled = False
        ValPerMonth.Enabled = False
        BRANCHID.Enabled = True
        CURRENCYID.Enabled = False
        EMPID.Enabled = True
        OverAllVal.Enabled = True
        Notes.Enabled = True
        RepaymentPeroid.Enabled = True
        BtnEdit.Caption = "إرجاع قيمة سلفة"
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()

        MyBase.BNew()

    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        InsertDate.EditValue = Date.Now
        InsertDate.Enabled = False
        CodeID.Enabled = False
        LOADBRNCHHasEmp(BRANCHID)
        LOADRECURRENCY()
        CURRENCYID.Text = "دينار ليبي"
        CURRENCYID.Enabled = False
        Notes.Text = ""
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Caption = "إرجاع قيمة سلفة"
        clsemadv.EMPDIS_MaxID(BRANCHID.EditValue)
        BRANCHID.EditValue = BID
        If BRANCHID.Text <> "" Then
            LOADEMP()
            clsemadv.EMPDIS_MaxID(BRANCHID.EditValue)
        End If
        EMPID.EditValue = -1
        'CURRENCYID.EditValue = -1
        RepaymentPeroid.Properties.MinValue = 1
        RepaymentPeroid.Properties.MaxValue = 24
        RepaymentPeroid.EditValue = 1
        OverAllVal.EditValue = 0.000
        ValPerMonth.EditValue = 0.000
        ValPerMonth.Enabled = False
        BtnPrint.Enabled= False
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        ENAPLEDCONTROLS()
        If UserType = 1 Then
            BRANCHID.Enabled = True

        Else
            BRANCHID.Enabled = False

        End If
    End Sub
    Sub GETVALBYMONTH()
        If OverAllVal.EditValue > 0.000 Then
            If RepaymentPeroid.EditValue > 0 And OverAllVal.EditValue > 0.000 Then
                ValPerMonth.EditValue = OverAllVal.EditValue \ RepaymentPeroid.EditValue
            End If
        End If
    End Sub
    Sub LOADRECURRENCY()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@IsDefault", SqlDbType.Bit)
        PRM(0).Value = 1
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CURRENCYTB_LoadDataIntoLookUpEdit", PRM)
        CURRENCYID.Properties.DataSource = DT
        CURRENCYID.Properties.ValueMember = "ID"
        CURRENCYID.Properties.DisplayMember = "CurrencyName"
        CURRENCYID.Properties.ShowHeader = False
    End Sub
    'Sub LOADBRNACH()
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
    '    BRANCHID.Properties.DataSource = DT
    '    BRANCHID.Properties.ValueMember = "DBRID"
    '    BRANCHID.Properties.DisplayMember = "BName"
    '    BRANCHID.Properties.ShowHeader = False
    'End Sub
    Sub LOADEMP()
        If BRANCHID.Text <> "" Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PR(0).Value = BRANCHID.EditValue
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDOnBRANCH", PR)
            If DT.Rows.Count > 0 Then
                EMPID.Properties.DataSource = DT
                EMPID.Properties.ValueMember = "ID"
                EMPID.Properties.DisplayMember = "EMPNAME"
                EMPID.Properties.PopulateColumns()
                EMPID.Properties.ShowHeader = False
            Else
                EMPID.EditValue = -1
                EMPID.Properties.DataSource = Nothing
            End If
        End If
    End Sub
    Private Sub BRANCHID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BRANCHID.QueryPopUp
        BRANCHID.Properties.PopulateColumns()
        BRANCHID.Properties.Columns("DBRID").Visible = False
        'BRANCHID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        If BRANCHID.Text <> "" Then
            EMPID.Properties.PopulateColumns()
            EMPID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Private Sub CURRENCYID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CURRENCYID.QueryPopUp
        CURRENCYID.Properties.PopulateColumns()
        CURRENCYID.Properties.Columns("ID").Visible = False
        CURRENCYID.Properties.Columns("ExchangeRate").Visible = False
        CURRENCYID.Properties.Columns("IsDefault").Visible = False
    End Sub

    Private Sub BRANCHID_TextChanged(sender As Object, e As EventArgs) Handles BRANCHID.TextChanged
        If BRANCHID.Text <> "" Then
            LOADEMP()
            clsemadv.EMPDIS_MaxID(BRANCHID.EditValue)
        End If
    End Sub

    Private Sub RepaymentPeroid_Leave(sender As Object, e As EventArgs) Handles RepaymentPeroid.Leave
        If OverAllVal.EditValue > 0.000 Then
            If RepaymentPeroid.EditValue > 0 Then
                GETVALBYMONTH()
            End If
        End If
    End Sub
    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.Text <> String.Empty Then
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_QUERY_ONLY("SELECT b.AccID FROM dbo.EmployeeTb as a LEFT join AccountsTb as b on a.AccID=B.AccID WHERE a.ID='" & EMPID.EditValue & "'")
            If DT.Rows.Count > 0 Then
                EmpAccID = DT.Rows(0)("AccID")
            End If
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_QUERY_ONLY("SELECT AccID from AccountsTb where AccParent=1010801 and BranchID='" & BRANCHID.EditValue & "'")
            If DTT.Rows.Count > 0 Then
                ADVPMTACCID = DTT.Rows(0)("AccID")
            End If
        End If
    End Sub
    Private Sub FRMEMPADVANCEPAYMENT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
#Region "Insert,Update,Delete,Search...etc"
    Public Overrides Sub SetData()

        If BRANCHID.EditValue = -1 Then
            BRANCHID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If EMPID.EditValue = -1 Then
            EMPID.ErrorText = "يجب اختيار الموظف"
            Return
        End If
        If OverAllVal.EditValue <= 0.000 Then
            OverAllVal.ErrorText = "يجب إدخال قيمة صحيحة"
            Return
        End If
        If RepaymentPeroid.EditValue <= 0 Then
            RepaymentPeroid.ErrorText = "يجب إدخال القيمة صحيحة"
            Return
        End If
        If CURRENCYID.EditValue = -1 Then
            CURRENCYID.ErrorText = "يجب اختيار العملة"
            Return
        End If

        'Dim PRM(2) As SqlParameter
        'PRM(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = EmpAccID}
        'PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BRANCHID.EditValue}
        'PRM(2) = New SqlParameter("@DDay", SqlDbType.Int) With {.Value = 0}

        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_PRO("SalaryCalc_LoadToCalculateByEMPAccID", PRM)
        'If DT.Rows(0)("باقي السلفة") > 0 Then
        '    ErrorMessage(Me, "رسالة تنبيه", "عذرا الموظف عليه سلفة سابقة الرجاء تسويتها أولا")
        '    Exit Sub
        'End If

        Dim MOTYPE As String = "سلفة لحساب الموظف"
        clsemadv.INSERTTB_EMPDIS(CodeID.Text.Trim, InsertDate.EditValue, EMPID.EditValue, BRANCHID.EditValue, OverAllVal.EditValue, RepaymentPeroid.EditValue, ValPerMonth.EditValue, UserID, CURRENCYID.EditValue,
                                 Notes.Text.Trim, ADVPMTACCID, EmpAccID, MOTYPE, IsUpdate)
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
            PRM(0) = New SqlParameter("@Code", CodeID.Text)
            Dim ds As New DataSet
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AdvancePaymentTb_SelectByCode", PRM)
            dt.TableName = "AdvancePaymentTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTEMPADVANCEPAYMENT
                report.DataSource = ds
                report.DataMember = "AdvancePaymentTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'Else
                '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWEMPADVANCEPAYMENT.LoadData()
        FRMVIEWEMPADVANCEPAYMENT.ShowDialog()
    End Sub

    Sub SHOW_RECORD(X)
        IsUpdate = True
        If IsUpdate = True Then
            LOADBRNCHHasEmp(BRANCHID)
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = X}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AdvancePaymentTb_SelectByCode", PR)
            If DT.Rows.Count > 0 Then
                CodeID.Text = DT.Rows(0)("Code").ToString
                InsertDate.EditValue = DT.Rows(0)("InsertDate")
                EMPID.EditValue = DT.Rows(0)("EMPID")
                BRANCHID.EditValue = DT.Rows(0)("BRANCHID")
                OverAllVal.EditValue = DT.Rows(0)("OverAllVal")
                RepaymentPeroid.EditValue = DT.Rows(0)("RepaymentPeroid")
                InsertDate.EditValue = DT.Rows(0)("InsertDate")
                ValPerMonth.EditValue = DT.Rows(0)("ValPerMonth")
                CURRENCYID.EditValue = DT.Rows(0)("CURRENCYID")
                Notes.Text = DT.Rows(0)("Notes").ToString
            End If
        End If
    End Sub
    Public Overrides Sub UPDATERECORD()
        If BRANCHID.EditValue = -1 Then
            BRANCHID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If EMPID.EditValue = -1 Then
            EMPID.ErrorText = "يجب اختيار الموظف"
            Return
        End If
        If OverAllVal.EditValue <= 0.000 Then
            OverAllVal.ErrorText = "يجب إدخال قيمة صحيحة"
            Return
        End If
        If RepaymentPeroid.EditValue <= 0 Then
            RepaymentPeroid.ErrorText = "يجب إدخال القيمة صحيحة"
            Return
        End If
        If CURRENCYID.EditValue = -1 Then
            CURRENCYID.ErrorText = "يجب اختيار العملة"
            Return
        End If
        Dim MOTYPE As String = "إرجاع سلفة من حساب الموظف" & Space(1) & EMPID.Text.Trim
        clsemadv.INSERTTB_EMPDIS(CodeID.Text.Trim, InsertDate.EditValue, EMPID.EditValue, BRANCHID.EditValue, OverAllVal.EditValue, RepaymentPeroid.EditValue, ValPerMonth.EditValue, UserID, CURRENCYID.EditValue,
                                 Notes.Text.Trim, ADVPMTACCID, EmpAccID, MOTYPE, IsUpdate)
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Private Sub FRMEMPADVANCEPAYMENT_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IsUpdate = False
        ENAPLEDCONTROLS()
        NEWRECORD()
    End Sub

    Private Sub RepaymentPeroid_TextChanged(sender As Object, e As EventArgs) Handles RepaymentPeroid.TextChanged
        GETVALBYMONTH()
    End Sub

    Private Sub OverAllVal_TextChanged(sender As Object, e As EventArgs) Handles OverAllVal.TextChanged
        GETVALBYMONTH()
    End Sub
#End Region
End Class