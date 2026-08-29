Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FrmSafeTransfer
    Dim clswd As New CLSWITHDRAWALFORM
    Dim clsaem As New CLSACCEMPACTIVITY
    Dim EMCode As String
    Public IsUpdate As Boolean
    Public SafeVal As Decimal
    Public IDCode As ULong
    Sub NABLEDTOOLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        SafeBalance.Enabled = False
        BranchID.Enabled = True
        WithdrawalFrom.Enabled = True
        WithdrawalTo.Enabled = True
        CurrencyID.Enabled = True
        WithdrawalValue.Enabled = True
        Notes.Enabled = True
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(42, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
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
            WithdrawalFrom.Enabled = dt.Rows(0)("Can_safID")
            WithdrawalFrom.EditValue = UserAccID
        Else
            BranchID.Enabled = False
            WithdrawalFrom.Enabled = False
            WithdrawalFrom.EditValue = UserAccID

        End If
    End Sub

    Sub DISAPLEDTOOLS()
        WDCode.Enabled = False
        WithdrawalDate.Enabled = False
        SafeBalance.Enabled = False
        BranchID.Enabled = False
        WithdrawalFrom.Enabled = False
        WithdrawalTo.Enabled = False
        CurrencyID.Enabled = False
        WithdrawalValue.Enabled = False
        Notes.Enabled = False
        BtnSave.Enabled = False
        BtnEdit.Enabled = True
        BtnEdit.Caption = "استرجاع القيمة"
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub

    Sub LOADCIDFROM()
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CuName"
            CurrencyID.Properties.ShowHeader = False
        Else
            CurrencyID.Properties.DataSource = Nothing
        End If
    End Sub

#Region "SAVE and UPDATE AND DELETE"
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Exit Sub
            End If
            If WithdrawalFrom.EditValue = -1 Then
                WithdrawalFrom.ErrorText = "يرجى اختيار الخزنة المنقول منها"
                Exit Sub
            End If
            If WithdrawalTo.EditValue = -1 Or WithdrawalTo.Text = String.Empty Then
                WithdrawalTo.ErrorText = "يرجى اختيار الخزنة المنقول إليها"
                Exit Sub
            End If
            If Val(Convert.ToDecimal(WithdrawalValue.Text)) > Val(Convert.ToDecimal(SafeBalance.Text)) Then
                WithdrawalValue.ErrorText = "القيمة المنقولة لا يجب أن تكون أكبر من القيمة الموجودة"
                Exit Sub
            End If
            If WithdrawalValue.Text = "0.000" Or WithdrawalValue.Text = String.Empty Then
                WithdrawalValue.ErrorText = "القيمة المنقولة لا يجب أن تكون صفر أو فارغة"
                Exit Sub
            End If
            If SafeBalance.Text = "0.000" Or SafeBalance.Text = String.Empty Or Val(Convert.ToDouble(SafeBalance.Text <= 0.000)) Then
                WithdrawalValue.ErrorText = "قيمة رصيد الخزنة لا يجب أن تكون صفر أو أقل"
                Exit Sub
            End If
            IsUpdate = False
            clswd.INSERTTB_WITHDRAWAL(WDCode.Text.Trim, WithdrawalDate.EditValue, WithdrawalFrom.EditValue, WithdrawalTo.EditValue, WithdrawalValue.EditValue, Notes.Text.Trim,
                                      BranchID.EditValue, UserID, CurrencyID.EditValue, "نقل من" & Space(1) & WithdrawalFrom.Text & Space(1) & " " & Space(1) & Notes.Text.Trim,
                                       "نقل إلى" & Space(1) & WithdrawalTo.Text & Space(1) & " " & Space(1) & Notes.Text.Trim, IsUpdate, IDCode)

            '"نقل إلى" & Space(1) & WithdrawalTo.Text
        End If

        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
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
            PRM(0) = New SqlParameter("@WDCode", WDCode.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_WithdrawalTb_SEARCH", PRM)
            If dt.Rows.Count > 0 Then
                dt.TableName = "WithdrawalTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                Dim report As New RPTSafeTransfer
                report.DataSource = ds
                report.DataMember = "WithdrawalTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub




    Sub SHOW_WD_DATA(x)
        Dim DT As New DataTable
        DT.Clear()
        DT = clswd.SHOW_WITHDRAWAL_DATA(x)
        If DT.Rows.Count > 0 Then
            WDCode.Text = DT.Rows(0)("WDCode").ToString
            WithdrawalDate.Text = DT.Rows(0)("WithdrawalDate")
            BranchID.EditValue = DT.Rows(0)("BranchID")
            CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
            LOADWithdrawalFrom()
            BtnSave.Enabled = False
            BtnPrint.Enabled = True
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
            WithdrawalFrom.EditValue = DT.Rows(0)("WithdrawalFrom")
            LOADWITHDRAWALTO()
            WithdrawalTo.EditValue = DT.Rows(0)("WithdrawalTo")
            Notes.Text = DT.Rows(0)("Notes").ToString
            'WithdrawalValue.Text = Val(SafeBalance.Text) + DT.Rows(0)("WithdrawalValue")
            Dim SafeVal As Double = Val(SafeBalance.Text)
            'SafeBalance.Text = SafeVal + DT.Rows(0)("WithdrawalValue")
            WithdrawalValue.EditValue = DT.Rows(0)("WithdrawalValue")

        End If
    End Sub
    Sub AccEmpActivityTb_Delete(ISID As String)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID}
        RUN_EXUTE_PRO("AccEmpActivityTb_Delete", PRM)
    End Sub
    Public Overrides Sub UPDATERECORD()
        IsUpdate = True
        If IsUpdate = True Then
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يجب اختيار الفرع أولا"
                Exit Sub
            End If
            If WithdrawalFrom.EditValue = -1 Then
                WithdrawalFrom.ErrorText = "يجب اختيار الخزنة المراد النقل منها"
                Exit Sub
            End If
            If WithdrawalTo.EditValue = -1 Then
                WithdrawalTo.ErrorText = "يجب اختيار الخزنة المراد النقل إليها"
                Exit Sub
            End If
            clswd.INSERTTB_WITHDRAWAL(WDCode.Text.Trim, WithdrawalDate.EditValue, WithdrawalFrom.EditValue, WithdrawalTo.EditValue, WithdrawalValue.EditValue, Notes.Text.Trim,
                                      BranchID.EditValue, UserID, CurrencyID.EditValue, "معالجة خطأ في نقل إلى خزينة" & Space(1) & WithdrawalTo.Text,
                                      "معالجة خطأ في نقل إلى خزينة" & Space(1) & WithdrawalTo.Text, IsUpdate, IDCode)
            'clswd.INSERTTB_WITHDRAWAL(WDCode.Text.Trim, WithdrawalDate.EditValue, WithdrawalFrom.EditValue, WithdrawalTo.EditValue, WithdrawalValue.EditValue, Notes.Text.Trim,
            '                          BranchID.EditValue, UserID, CurrencyID.EditValue, "معالجة خطأ في نقل إلى خزينة" & Space(1) & WithdrawalTo.Text,
            '                          "معالجة خطأ نقل من" & Space(1) & WithdrawalFrom.Text & Space(1) & "إلى" & Space(1) & WithdrawalTo.Text, IsUpdate, IDCode)
        End If
        NEWRECORD()
        MyBase.Update()
    End Sub
    Public Overrides Sub Remove()
        If IsUpdate = True Then
            clswd.DELETE_WITHDRAWAL(WDCode.Text.Trim)
            clsaem.DELETE_ACCEMPACTIVITY(WDCode.Text.Trim, 0, 20)
        End If
        NEWRECORD()
        MyBase.Remove()
    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        LCI.Text = "رصيد الخزنة"
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        NABLEDTOOLS()
        EMCode = Format(GETMAXID("AccEmpActivityTb", "ID") + 1, "EM000000")
        LOADBRNCHDIERCT(BranchID)
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        CurrencyID.EditValue = 1
        LOADWithdrawalFrom()
        WDCode.Enabled = False
        WDCode.ReadOnly = True
        WithdrawalDate.EditValue = Date.Now
        WithdrawalDate.Enabled = False
        WithdrawalDate.ReadOnly = True
        WithdrawalTo.Properties.DataSource = Nothing
        WithdrawalFrom.EditValue = -1
        WithdrawalTo.EditValue = -1
        SafeBalance.Text = "0.000"
        SafeBalance.Enabled = False
        WithdrawalValue.EditValue = 0.000
        Notes.Text = ""
        clswd.WITHDRAWAL_MaxID(14, BranchID.EditValue, UserID, COUNTRYNID, CITYID)
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 42)
    End Sub
#End Region
#Region "LOADDATA"
    Sub LOADWithdrawalFrom()
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        PRM(1) = New SqlParameter("@CurrencyId", SqlDbType.Int)
        PRM(1).Value = CurrencyID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccountsTb_LoadSafeToTransferHaveVal", PRM)
        DT.Rows.Add(0, "الخزنة الرئيسية")
        If DT.Rows.Count > 0 Then
            WithdrawalFrom.Properties.DataSource = DT
            WithdrawalFrom.Properties.ValueMember = "AccID"
            WithdrawalFrom.Properties.DisplayMember = "AccName"
            WithdrawalFrom.Properties.ShowHeader = False
            WithdrawalFrom.EditValue = UserAccID
        End If

    End Sub
    Sub LOADWITHDRAWALTO()
        If WithdrawalFrom.EditValue <> -1 Or WithdrawalFrom.EditValue <> Nothing Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PRM(0).Value = BranchID.EditValue
            PRM(1) = New SqlParameter("@AccID", SqlDbType.Int)
            PRM(1).Value = WithdrawalFrom.EditValue
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AccountsTb_LoadSafeToTransferNotExist", PRM)
            If DT.Rows.Count > 0 Then
                WithdrawalTo.Properties.DataSource = DT
                WithdrawalTo.Properties.ValueMember = "AccID"
                WithdrawalTo.Properties.DisplayMember = "AccName"
                WithdrawalTo.Properties.ShowHeader = False


            End If
            If WithdrawalFrom.EditValue <> 0 Then
                DT.Rows.Add(0, "الخزنة الرئيسية")
            End If
        Else
            WithdrawalTo.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub CurrencyID_TextChanged(sender As Object, e As EventArgs) Handles CurrencyID.TextChanged
        WithdrawalFrom.EditValue = -1
        WithdrawalFrom.Properties.DataSource = Nothing
        LOADWithdrawalFrom()

    End Sub

    Private Sub FrmSafeTransfer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub

    Private Sub BtnView_Click(sender As Object, e As EventArgs) Handles BtnView.Click
        FrmViewBalanceTransport.ShowDialog()
    End Sub

    Private Sub WithdrawalFrom_TextChanged(sender As Object, e As EventArgs) Handles WithdrawalFrom.TextChanged
        SafeBalance.Text = "0.000"
        If WithdrawalFrom.EditValue <> -1 Or WithdrawalFrom.EditValue <> Nothing Then
            LOADWITHDRAWALTO()
            'WithdrawalTo.Properties.PopulateColumns()
            'WithdrawalTo.Properties.Columns("AccID").Visible = False
        Else
            WithdrawalTo.Properties.DataSource = Nothing
        End If
        If WithdrawalFrom.EditValue <> -1 Or WithdrawalFrom.Text <> String.Empty And CurrencyID.EditValue <> -1 Or CurrencyID.Text <> String.Empty Then
            'If IsUpdate = False Then
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@AccIDFrom", SqlDbType.Int)
            PRM(0).Value = WithdrawalFrom.EditValue
            PRM(1) = New SqlParameter("@AccBranchID", SqlDbType.Int)
            PRM(1).Value = BranchID.EditValue
            PRM(2) = New SqlParameter("@CurrencyID", SqlDbType.Int)
            PRM(2).Value = CurrencyID.EditValue
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AccSafeActivityTb_GetSafeVal", PRM)
            If DT.Rows.Count > 0 Then
                '    MsgBox("لا يوجد رصيد في الخزنة من العملة المختارة")
                '    Exit Sub
                'Else
                SafeBalance.EditValue = DT.Rows(0)("NetTotal")
                SafeVal = Format(Convert.ToDecimal(DT.Rows(0)("NetTotal")), "N3")
            End If
        End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        SafeBalance.Text = "0.000"
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Or BranchID.EditValue <> Nothing Then
            'LOADWithdrawalFrom()
            WithdrawalFrom.EditValue=-1
            CurrencyID.EditValue = -1
            LOADCIDFROM()
            'WithdrawalFrom.Properties.PopulateColumns()
            'WithdrawalFrom.Properties.Columns("AccID").Visible = False
        End If
        clswd.WITHDRAWAL_MaxID(14, BranchID.EditValue, UserID, COUNTRYNID, CITYID)
    End Sub

    Private Sub WithdrawalFrom_QueryPopUp(sender As Object, e As CancelEventArgs) Handles WithdrawalFrom.QueryPopUp
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        PRM(1) = New SqlParameter("@CurrencyId", SqlDbType.Int)
        PRM(1).Value = CurrencyID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccountsTb_LoadSafeToTransferHaveVal", PRM)
        DT.Rows.Add(0, "الخزنة الرئيسية")
        If DT.Rows.Count > 0 Then
            WithdrawalFrom.Properties.PopulateColumns()
            WithdrawalFrom.Properties.Columns("AccID").Visible = False
        End If
    End Sub

    Private Sub WithdrawalTo_QueryPopUp(sender As Object, e As CancelEventArgs) Handles WithdrawalTo.QueryPopUp
        If BranchID.EditValue = -1 And BranchID.Text = String.Empty Then
            BranchID.ErrorText = "يجب إختيار الفرع"
            Exit Sub
        End If
        If WithdrawalFrom.EditValue = -1 And WithdrawalFrom.Text = String.Empty Then
            WithdrawalFrom.ErrorText = "يجب إختيار الخزينة المنقول منها"
            Exit Sub
        End If
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        PRM(1) = New SqlParameter("@AccID", SqlDbType.Int)
        PRM(1).Value = WithdrawalFrom.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccountsTb_LoadSafeToTransferNotExist", PRM)
        If DT.Rows.Count > 0 Then
            WithdrawalTo.Properties.PopulateColumns()
            WithdrawalTo.Properties.Columns("AccID").Visible = False
        End If
    End Sub

    Private Sub FrmSafeTransfer_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
#End Region
End Class