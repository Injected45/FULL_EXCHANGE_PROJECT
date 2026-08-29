Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FRMEMPADDINCREASE
    Dim CLSEMD As New CLSEMINCREASE
    Public IsUpdate As Boolean
    Public INSERTDATE As Date
    Public EmpAccID, BonusAccID As ULong
    Sub DISAPLEDCONTROLS()
        CodeID.Enabled = False
        BRANCHID.Enabled = False
        CURRENCYID.Enabled = False
        EMPID.Enabled = False
        IsActiveTG.Enabled = False
        IsConstant.Enabled = False
        DISTYPEID.Enabled = False
        DISVAL.Enabled = False
        Notes.Enabled = False
        BtnEdit.Caption = "إيقاف علاوة"
        BtnEdit.Enabled = True
        BtnPrint.Enabled = True
        Me.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Me.BtnSave.Enabled = False
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(70, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub ENAPLEDCONTROLS()
        CodeID.Enabled = False
        BRANCHID.Enabled = True
        EMPID.Enabled = True
        IsActiveTG.Enabled = True
        IsConstant.Enabled = True
        DISTYPEID.Enabled = True
        Notes.Enabled = True
        DISVAL.Enabled = True
        BtnEdit.Caption = "إيقاف علاوة"
        BtnEdit.Enabled = False
        BtnPrint.Enabled = False
        Me.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Me.BtnSave.Enabled = True
    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        IsActiveTG.IsOn = True
        DISVAL.EditValue = 0.000
        Me.BtnSave.Enabled = True
        BtnEdit.Caption = "إيقاف علاوة"
        Me.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        'Me.BtnPrint.Enabled = True
        BtnEdit.Enabled = False
        CURRENCYID.Enabled = False
        LOADBRNCHHasEmp(BRANCHID)
        LOADRECURRENCY()
        LOADDISTYPEID()
        CLSEMD.EMPDIS_MaxID(BRANCHID.EditValue)
        BRANCHID.EditValue = BID
        If BRANCHID.EditValue <> -1 Then
            LOADEMP()
            CLSEMD.EMPDIS_MaxID(BRANCHID.EditValue)
        End If
        CURRENCYID.Text = "دينار ليبي"
        DISTYPEID.EditValue = -1
        EMPID.EditValue = -1
        DISVAL.EditValue = 0.000
        Notes.Text = ""
        IsConstant.Checked = False
        Me.BtnDelete.Enabled = False
        ENAPLEDCONTROLS()
        If UserType = 1 Then
            BRANCHID.Enabled = True

        Else
            BRANCHID.Enabled = False


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
        If BRANCHID.EditValue <> -1 Then
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
    Sub LOADDISTYPEID()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("IncreasValTb_LOADINTOLKP")
        If DT.Rows.Count > 0 Then
            DISTYPEID.Properties.DataSource = DT
            DISTYPEID.Properties.ValueMember = "ID"
            DISTYPEID.Properties.DisplayMember = "PIName"
            DISTYPEID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        If BRANCHID.Text <> String.Empty Then
            EMPID.Properties.PopulateColumns()
            EMPID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Private Sub DISTYPEID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles DISTYPEID.QueryPopUp
        DISTYPEID.Properties.PopulateColumns()
        DISTYPEID.Properties.Columns("ID").Visible = False
    End Sub
    Public Overrides Sub SetData()
        If EMPID.EditValue = -1 Then
            EMPID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If DISVAL.EditValue <= 0.000 Then
            DISVAL.ErrorText = "قيمة الخصم لا يجب أن تكون أقل من أو صفر"
            Return
        End If
        If DISTYPEID.EditValue = -1 Then
            DISTYPEID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If BRANCHID.EditValue = -1 Then
            BRANCHID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If CURRENCYID.EditValue = -1 Then
            CURRENCYID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        Dim MOTYPE As String = "علاوة لصالح الموظف" & Space(1) & EMPID.Text.Trim
        CLSEMD.INSERTTB_EMPDIS(Date.Now, EMPID.EditValue, DISTYPEID.EditValue, DISVAL.EditValue, CodeID.Text.Trim, IsConstant.Checked, Notes.Text.Trim)
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
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_IncreaseValTb_Select", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "IncreaseValTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTEMPADDINCREASE
                report.DataSource = ds
                report.DataMember = "IncreaseValTb"
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


    Public Overrides Sub UPDATERECORD()
        If EMPID.EditValue = -1 Then
            EMPID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If DISVAL.EditValue <= 0.000 Then
            DISVAL.ErrorText = "قيمة الخصم لا يجب أن تكون أقل من أو صفر"
            Return
        End If
        If DISTYPEID.EditValue = -1 Then
            DISTYPEID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If BRANCHID.EditValue = -1 Then
            BRANCHID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        If CURRENCYID.EditValue = -1 Then
            CURRENCYID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        Dim MOTYPE As String = "إرجاع علاوة الموظف" & Space(1) & EMPID.Text.Trim
        CLSEMD.EMPDIS_DELETE(CodeID.Text.Trim, EMPID.EditValue, DISVAL.Text.Trim)
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub Remove()
        'CLSEMD.DELETE_EMP(CodeID.Text)
        MyBase.Remove()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Sub SHOW_RECORD(X)
        If IsUpdate = True Then
            LOADBRNCHHasEmp(BRANCHID)
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = X}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("IncreaseValTb_Select", PR)
            If DT.Rows.Count > 0 Then
                CodeID.Text = DT.Rows(0)("Code").ToString
                EMPID.EditValue = DT.Rows(0)("EMPID")
                DISTYPEID.EditValue = DT.Rows(0)("IncreaseTypeID")
                DISVAL.EditValue = DT.Rows(0)("INCVAL")
                IsActiveTG.IsOn = DT.Rows(0)("IsActive")
                INSERTDATE = DT.Rows(0)("InsertDate")
                IsConstant.Checked = DT.Rows(0)("IsConstant")
                Notes.Text = DT.Rows(0)("Notes")
            End If
        End If
    End Sub

    Private Sub BRANCHID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BRANCHID.QueryPopUp
        BRANCHID.Properties.PopulateColumns()
        BRANCHID.Properties.Columns("DBRID").Visible = False
        'BRANCHID.Properties.Columns("BranchType").Visible = False
    End Sub

    Private Sub CURRENCYID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CURRENCYID.QueryPopUp
        CURRENCYID.Properties.PopulateColumns()
        CURRENCYID.Properties.Columns("ID").Visible = False
    End Sub

    Private Sub BRANCHID_TextChanged(sender As Object, e As EventArgs) Handles BRANCHID.TextChanged
        If BRANCHID.EditValue <> -1 Then
            LOADEMP()
            CLSEMD.EMPDIS_MaxID(BRANCHID.EditValue)
        End If
    End Sub

    Private Sub FRMEMPADDINCREASE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        'If IsUpdate = False Then
        NEWRECORD()
        'End If
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWEMPINCREASE.ShowDialog()
    End Sub

    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.Text <> String.Empty Then
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_QUERY_ONLY("SELECT b.AccID FROM dbo.EmployeeTb as a LEFT join AccountsTb as b on a.AccID=B.AccID WHERE  a.ID	 ='" & EMPID.EditValue & "'")
            If DT.Rows.Count > 0 Then
                EmpAccID = DT.Rows(0)("AccID")
            End If
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_QUERY_ONLY("SELECT AccID from AccountsTb where AccParent=39 and BranchID='" & BRANCHID.EditValue & "'")
            If DTT.Rows.Count > 0 Then
                BonusAccID = DTT.Rows(0)("AccID")
            End If
        End If
    End Sub
End Class