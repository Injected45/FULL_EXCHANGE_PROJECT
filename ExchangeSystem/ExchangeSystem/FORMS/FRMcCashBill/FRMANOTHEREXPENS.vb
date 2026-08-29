Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMANOTHEREXPENS
    Dim CLSPC As New CLSANOTHEREXPENS
    Public IsUpdate, UpdateBySalary, IsAseet As Boolean
    Public AcID, IDCode, AccCode, AccEm, CodeID, ExpenseID As ULong
    Sub NEWRECORD()
        AccIDEX.Properties.DataSource = Nothing
        Code.Enabled = False
        Code.Text = ""
        InsertDate.EditValue = Date.Now
        LOADBRANCH()
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
        BtnEdit.Caption = "إرجاع قيمة العهدة"
        BtnSave.Enabled = True
        BtnPrint.Enabled = False
        LOADRECURRENCY()
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        CurrencyID.Text = "دينار ليبي"
        CurrencyID.Enabled = False
        IsUpdate = False
        ENAPLEDCONTROLS()
        SafeID.EditValue = -1
        ExpensVal.EditValue = 0.000
        AccIDEX.EditValue = -1
        Notes.Text = ""
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        FormLocation(Me)
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 47)
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
    Sub ENAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = True
        SafeID.Enabled = True
        CurrencyID.Enabled = False
        Notes.Enabled = True
        InsertDate.Enabled = False
        AccIDEX.Enabled = True
        ExpensVal.Enabled = True
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(47, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub DISAPLEDCONTROLS()
        Code.Enabled = False
        BranchID.Enabled = False
        SafeID.Enabled = False
        CurrencyID.Enabled = False
        Notes.Enabled = False
        InsertDate.Enabled = False
        AccIDEX.Enabled = False
        ExpensVal.Enabled = False
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            Dim dt As New DataTable
            dt.Clear()
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "يرجى اختيار الخزنة"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If AccIDEX.EditValue = -1 Or AccIDEX.Text = String.Empty Then
                AccIDEX.ErrorText = "يرجى اختيار المصروف"
                Return
            End If
            If ExpensVal.EditValue <= 0.000 Then
                ExpensVal.ErrorText = "القيمة يجب أن لا تكون صفر أو أقل"
                Return
            End If
            GETSAFEVAL(SafeID.EditValue, BranchID.EditValue, CurrencyID.EditValue)
            If SAFEVAL < ExpensVal.EditValue Then
                ErrorMessage(Me, "رسالة تنبيه", "رصيد الخزنة غير كافي الرجاء التأكد من رصيد الخزنة")
                Exit Sub
            End If
            Dim MOV As String = "مقابل مصروفات لحساب" & Space(1) & AccIDEX.Text
            CLSPC.ANOTHEREXPENSTB_Insert(Code.Text.Trim, InsertDate.EditValue, BranchID.EditValue, UserID, CurrencyID.EditValue, ExpensVal.EditValue, AccIDEX.EditValue, Notes.Text.Trim, CodeID, 1,
                                         SafeID.EditValue, MOV, IsUpdate, UserID)





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
            'Else
            '    Exit Sub
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



        'If GVRole.RowCount = 0 Then
        '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", Code.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_PettyCashTb_LOADTORPTANOTHEREXPENS", PRM)
            Dim ds As New DataSet
            dt.TableName = "PettyCashTb"
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTANOTHEREXPENS
                report.DataSource = ds
                report.DataMember = "PettyCashTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.XrLabel18.Text = Cur_Code(CurrencyID.Text, ExpensVal.EditValue, False, "n2")
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub


    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            Dim dt As New DataTable
            dt.Clear()
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If SafeID.EditValue = -1 Or SafeID.Text = String.Empty Then
                SafeID.ErrorText = "يرجى اختيار الخزنة"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If AccIDEX.EditValue = -1 Or AccIDEX.Text = String.Empty Then
                AccIDEX.ErrorText = "يرجى اختيار المصروف"
                Return
            End If
            If ExpensVal.EditValue <= 0.000 Then
                ExpensVal.ErrorText = "القيمة يجب أن لا تكون صفر أو أقل"
                Return
            End If
            Dim MOV As String = "مقابل مصروفات لحساب" & Space(1) & AccIDEX.Text
            CLSPC.ANOTHEREXPENSTB_Insert(Code.Text.Trim, InsertDate.EditValue, BranchID.EditValue, UserID, CurrencyID.EditValue, ExpensVal.EditValue, AccIDEX.EditValue, Notes.Text.Trim, CodeID, 1,
                                         SafeID.EditValue, MOV, IsUpdate, UserID)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_EMCUSCODE(x)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = CLSPC.SERACH_ANOTHEREXPENSTB(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
                SafeID.EditValue = DT.Rows(0)("AccSafeID")
                InsertDate.EditValue = DT.Rows(0)("InsertDate")
                Notes.Text = DT.Rows(0)("Notes").ToString
                AccIDEX.EditValue = DT.Rows(0)("AccIDPC")
                ExpensVal.EditValue = DT.Rows(0)("ExpensVal")
            End If
        End If
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
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
        DT = RUN_QUARY_PRO_ONLY("CURRENCYTB_LoadToLKP")
        CurrencyID.Properties.DataSource = DT
        CurrencyID.Properties.ValueMember = "ID"
        CurrencyID.Properties.DisplayMember = "CurrencyName"
        CurrencyID.Properties.ShowHeader = False
    End Sub
    Sub LOADSafeID()
        SafeID.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasVal", PR)
            If dt.Rows.Count > 0 Then
                SafeID.Properties.DataSource = dt
                SafeID.Properties.ValueMember = "AccID"
                SafeID.Properties.DisplayMember = "UNAME"
                SafeID.Properties.KeyMember = BranchID.EditValue
                SafeID.Properties.ShowHeader = False
            End If
        Else
            SafeID.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADEXPANSTYPE()
        AccIDEX.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty And BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.DataSource = dt
                AccIDEX.Properties.ValueMember = "AccID"
                AccIDEX.Properties.DisplayMember = "ExName"
                AccIDEX.Properties.ShowHeader = False
            End If
        Else
            AccIDEX.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADِAseet()
        AccIDEX.Properties.DataSource = Nothing
        If BranchID.Text <> String.Empty And BranchID.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 1}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ExpensesTb_LOADTOLKPBasedOnExType", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.DataSource = dt
                AccIDEX.Properties.ValueMember = "AccID"
                AccIDEX.Properties.DisplayMember = "AccName"
                AccIDEX.Properties.ShowHeader = False
            End If
        Else
            AccIDEX.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWANOTHEREXPENS.ShowDialog()
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub FRMANOTHEREXPENS_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasVal", PR)
        If dt.Rows.Count > 0 Then
            SafeID.Properties.PopulateColumns()
            SafeID.Properties.Columns("AccID").Visible = False
        End If
    End Sub
    Private Sub CurrencyID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CurrencyID.QueryPopUp
        CurrencyID.Properties.PopulateColumns()
        CurrencyID.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub AccIDEX_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccIDEX.QueryPopUp
        If IsAseet = False Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("[ExpensesTb_LOADTOLKPBasedOnBranchID]", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.PopulateColumns()
                AccIDEX.Properties.Columns("AccID").Visible = False
            End If
        End If
        If IsAseet = True Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 1}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ExpensesTb_LOADTOLKPBasedOnExType", PR)
            If dt.Rows.Count > 0 Then
                AccIDEX.Properties.PopulateColumns()
                AccIDEX.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        LOADSafeID()
        If IsAseet = False Then
            LOADEXPANSTYPE()
        End If
        If IsAseet = True Then
            LOADِAseet()
        End If
    End Sub
    Private Sub FRMANOTHEREXPENS_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub SafeID_TextChanged(sender As Object, e As EventArgs) Handles SafeID.TextChanged
        If SafeID.Text <> String.Empty Or SafeID.EditValue <> -1 Then
            If IsUpdate = False Then
                Dim DTT As New DataTable
                DTT.Clear()
                DTT = CLSPC.ANOTHEREXPENSTB_MaxID(BranchID.EditValue, SafeID.EditValue)
                CodeID = DTT.Rows(0)("ID")

                Code.Text = DTT.Rows(0)("Code")
            End If
        End If
    End Sub
End Class