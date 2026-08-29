Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FRMEMPDISCOUNT
    Dim CLSEMD As New CLSEMPDISCOUNT
    Dim clsempwd As New CLSEMPWITHDRAWAL
    Public IsUpdate As Boolean
    Public INSERTDATE As Date
    Public AccID As ULong
    Public EMID As Integer
    Sub GetAccID()
        If BRANCHID.Text <> String.Empty Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BRANCHID.EditValue}
            PRM(1) = New SqlParameter("@AccParent", SqlDbType.Decimal) With {.Value = 45}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AccountsTb_GetAccIDBaseOnBranchID", PRM)
            If DT.Rows.Count > 0 Then
                AccID = DT.Rows(0)("AccID")
            End If
        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(71, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub NEWRECORD()
        LOADBRNCHHasEmp(BRANCHID)
        LOADRECURRENCY()
        LOADDISTYPEID()
        BRANCHID.EditValue = -1
        CodeID.Text = ""
        CURRENCYID.Text = "دينار ليبي"
        DISTYPEID.EditValue = -1
        CURRENCYID.Enabled = False
        EMPID.EditValue = -1
        EMPID.Properties.DataSource = Nothing
        Notes.Text = ""
        DISVAL.EditValue = 0.000
        CodeID.Enabled = False
        BRANCHID.Enabled = True
        EMPID.Enabled = True
        IsActiveTG.Enabled = True
        DISTYPEID.Enabled = True
        DISVAL.Enabled = True
        Notes.Enabled = True
        IsUpdate = False
        IsActiveTG.IsOn = True
        BtnSave.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnDelete.Enabled = False
        BtnEdit.Enabled = False
        BtnEdit.Caption = "إلغاء قيمة الخصم"
        BRANCHID.EditValue = BID
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
        If BRANCHID.Text <> String.Empty Then
            'EMPID.Properties.DataSource = Nothing
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BRANCHID.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONACCID", PR)
            If dt.Rows.Count > 0 Then
                EMPID.Properties.DataSource = dt
                EMPID.Properties.ValueMember = "AccID"
                EMPID.Properties.DisplayMember = "EMPNAME"
                EMPID.Properties.ShowHeader = False
            End If
        End If
    End Sub
    Sub LOADDISTYPEID()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("DiscountValTb_LOADINTOLKP")
        If DT.Rows.Count > 0 Then
            DISTYPEID.Properties.DataSource = DT
            DISTYPEID.Properties.ValueMember = "ID"
            DISTYPEID.Properties.DisplayMember = "DISNAME"
            DISTYPEID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub EMPID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles EMPID.QueryPopUp
        If BRANCHID.Text <> String.Empty Then
            'If EMPID.Text <> Nothing Then
            EMPID.Properties.PopulateColumns()
            EMPID.Properties.Columns("AccID").Visible = False
            'End If
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
        Dim MOTYPE As String = "خصم بسبب" & Space(1) & DISTYPEID.Text.Trim
        CLSEMD.INSERTTB_EMPDIS(Date.Now, EMID, DISTYPEID.EditValue, DISVAL.EditValue, UserID, CodeID.Text.Trim, BRANCHID.EditValue, CURRENCYID.EditValue, Notes.Text.Trim, IsUpdate)
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
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_DiscountValTb_Select", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "DiscountValTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTEMPDISCOUNT
                report.DataSource = ds
                report.DataMember = "DiscountValTb"
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
        If IsUpdate = True Then
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
            Dim MOTYPE As String = "معالجة خطأ خصم بسبب" & Space(1) & DISTYPEID.Text.Trim
            CLSEMD.INSERTTB_EMPDIS(Date.Now, EMID, DISTYPEID.EditValue, DISVAL.EditValue, UserID, CodeID.Text.Trim, BRANCHID.EditValue, CURRENCYID.EditValue, Notes.Text.Trim, IsUpdate)
            'clsempwd.AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(UserID, 0.000, DISVAL.EditValue, Date.Now, CodeID.Text.Trim, 11, 34, BRANCHID.EditValue, EMPID.EditValue, AccID, MOTYPE)
            'clsempwd.AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(UserID, DISVAL.EditValue, 0.000, Date.Now, CodeID.Text.Trim, 11, 34, BRANCHID.EditValue, AccID, EMPID.EditValue, MOTYPE)
        End If
        NEWRECORD()
        DISVAL.EditValue = 0.000
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub Remove()
        CLSEMD.DELETE_EMP(CodeID.Text)
        MyBase.Remove()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Sub SHOW_RECORD(X)
        If IsUpdate = True Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = X}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("DiscountValTb_Select", PR)
            If DT.Rows.Count > 0 Then
                CodeID.Text = X.ToString
                BRANCHID.EditValue = DT.Rows(0)("BranchID")
                EMPID.Text = DT.Rows(0)("EMPNAME").ToString
                EMID = DT.Rows(0)("ID").ToString
                DISTYPEID.EditValue = DT.Rows(0)("DiscountTypeID")
                DISVAL.EditValue = DT.Rows(0)("DISVAL")
                IsActiveTG.IsOn = DT.Rows(0)("IsActive")
                INSERTDATE = DT.Rows(0)("InsertDate")
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

        If BRANCHID.Text <> String.Empty Then
            If IsUpdate = False Then
                CLSEMD.EMPDIS_MaxID(BRANCHID.EditValue)
            End If
            'If BRANCHID.Text <> String.Empty Then
            EMPID.Properties.DataSource = Nothing
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BRANCHID.EditValue}

                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONACCID", PR)
                If dt.Rows.Count > 0 Then
                    EMPID.Properties.DataSource = dt
                    EMPID.Properties.ValueMember = "AccID"
                    EMPID.Properties.DisplayMember = "EMPNAME"
                    EMPID.Properties.ShowHeader = False
                End If
                ' End If
                GetAccID()
            End If

    End Sub

    Private Sub FRMEMPDISCOUNT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWEMPDISCOUNT.ShowDialog()
    End Sub
    Sub DISAPLEDCONTROLS()
        CodeID.Enabled = False
        BRANCHID.Enabled = False
        CURRENCYID.Enabled = False
        EMPID.Enabled = False
        IsActiveTG.Enabled = False
        DISTYPEID.Enabled = False
        DISVAL.Enabled = False
        Notes.Enabled = False
        BtnSave.Enabled = False
        BtnDelete.Visibility = False
        BtnEdit.Enabled = True
        BtnEdit.Caption = "إلغاء قيمة الخصم"
    End Sub
    Sub ENAPLEDCONTROLS()
        CodeID.Enabled = False
        BRANCHID.Enabled = True
        CURRENCYID.Enabled = True
        EMPID.Enabled = True
        IsActiveTG.Enabled = True
        DISTYPEID.Enabled = True
        DISVAL.Enabled = True
        Notes.Enabled = True
        BtnSave.Enabled = True
        BtnDelete.Visibility = False
        BtnEdit.Enabled = False
        BtnEdit.Caption = "إلغاء قيمة الخصم"
    End Sub

    Private Sub FRMEMPDISCOUNT_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IsUpdate = False
        NEWRECORD()
        ENAPLEDCONTROLS()
    End Sub

    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        If EMPID.Text <> String.Empty Then
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_QUERY_ONLY("SELECT ID FROM EmployeeTb where EMPNAME='" & EMPID.Text.Trim & "'")
            If DT.Rows.Count > 0 Then
                EMID = DT.Rows(0)("ID")
            End If
        End If
    End Sub
End Class