Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class FRMADDUSER
    Dim CLSAUS As New CLSADDUSER
    Public AccID, AccEmp As ULong
    Public IsUpdate As Boolean
    Dim Empployid As Integer
    Sub fillProfileName()
        UserType.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ProfileName_fill")
        If DT.Rows.Count > 0 Then
            UserType.Properties.DataSource = DT
            UserType.Properties.ValueMember = "ProfID"
            UserType.Properties.DisplayMember = "ProfileName"
        End If
    End Sub

    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
    End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(100, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub LOADEMPACCID()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONACCIDANDID", PRM)
        If DT.Rows.Count > 0 Then
            EMID.Properties.DataSource = DT
            EMID.Properties.ValueMember = "ID"
            EMID.Properties.DisplayMember = "EMPNAME"
        End If
    End Sub
    Sub LOADUSERSTOLSBOX()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("TB_Users_LOADTOLSBOX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.ValueMember = "USID"
            LSBOX.DisplayMember = "UName"
        End If
    End Sub
    Sub LOADACCESSPROFILE()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("UserAccessProfileName_LOADTOLKP")
        If DT.Rows.Count > 0 Then
            ACCPROFID.Properties.DataSource = DT
            ACCPROFID.Properties.ValueMember = "ID"
            ACCPROFID.Properties.DisplayMember = "Name"
        End If
    End Sub
    Private Sub ACCPROFID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ACCPROFID.QueryPopUp
        ACCPROFID.Properties.PopulateColumns()
        ACCPROFID.Properties.Columns("ID").Visible = False
    End Sub
    Sub NEWRECROD()
        BtnPrint.Caption = "إعادة إرسال الرمز"
        BtnPrint.ImageOptions.Reset()
        BtnPrint.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        IsUpdate = False
        EMID.Enabled = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        BranchID.Enabled = True
        CodeID.Text = GETUSERMAXID("TB_Users", "USID") + 1
        LOADBRANCH()
        LoadToControlar(UserTypeForEdit, "ProfileName_fill", "ProfileName", "ProfID", Nothing)
        'LOADUSERSTOLSBOX()
        LOADACCESSPROFILE()
        fillProfileName()
        UName.Text = ""
        UNameLog.Text = ""
        UPass.Text = ""
        phone.Text = ""
        BranchID.EditValue = -1
        ACCPROFID.EditValue = -1
        UserType.EditValue = -1
        UserTypeForEdit.EditValue = -1
        UName.Select()
        EMID.Properties.DataSource = Nothing
        EMID.EditValue = -1
        IsEmpORUser.SelectedIndex = -1
        IsEmpORUser.Enabled = True
        ISHidden.IsOn = False
    End Sub
    Private Sub FRMADDUSER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECROD()
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub


#Region "Insert,Update"
    Public Overrides Sub SetData()

        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        IsDataValidTextEdit(UName)
        IsDataValidTextEdit(UNameLog)
        IsDataValidTextEdit(UPass)
        IsDataValidLKP(BranchID)
        IsDataValidLKP(ACCPROFID)
        Dim DT As New DataTable
        DT.Clear()
        DT = CLSAUS.CHECK_USER_NAME(UName.Text.Trim, 0, IsUpdate)
        If DT.Rows.Count > 0 Then
            XtraMessageBox.Show(lookAndFeelError, "اسم المستخدم موجود مسبقا يرجى تغيير الاسم", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        DT = CLSAUS.CHECK_USER_NAME(UNameLog.Text.Trim, 0, IsUpdate)
        If DT.Rows.Count > 0 Then
            XtraMessageBox.Show(lookAndFeelError, "اسم الدخول موجود مسبقا يرجى تغيير الاسم", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If IsUpdate = False Then
            Dim Empid As Integer
            Dim AccID As ULong

            If IsEmpORUser.SelectedIndex = 0 Then
                Empid = Empployid
                AccID = GVROLEMPID.GetFocusedRowCellValue("EMPAccID")
            Else
                Empid = 0
                AccID = 0
            End If
            CLSAUS.TB_Users_Insert(CodeID.Text.Trim, UName.Text.Trim, UNameLog.Text.Trim, UPass.Text.Trim, IsActiveTG.EditValue,
                                   BranchID.EditValue, ACCPROFID.EditValue, UserType.EditValue, IsUpdate,
                                   AccID, Empid, IsEmpORUser.SelectedIndex, ISHidden.IsOn, phone.Text)
        End If
        NEWRECROD()
        MyBase.SetData()
    End Sub

    Public Overrides Sub BNew()
        NEWRECROD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        'If IsUpdate = True And IsEmpORUser.SelectedIndex = 0 Then
        If IsUpdate = True Then
            Dim mms As String = " *شركة الرحالة للصرافة*" & vbNewLine &
                "اسم المستخدم " & ":" & Space(1) & UNameLog.Text.Trim & vbNewLine &
                "كلمة المرور" & ":" & Space(1) & UPass.Text.Trim & vbNewLine &
                "لا تشارك البيانات مع أحد"
            WATSAPPMsAG(phone.Text, mms, True)
            FrmSavedSuccessfully.SimpleLabelItem1.Text = "تم الإرسال بنجاح"
            FrmSavedSuccessfully.Show()
        End If
        MyBase.Print()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
            Dim cusok As New MessageBoxButtons
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            IsDataValidTextEdit(UName)
            IsDataValidTextEdit(UNameLog)
            IsDataValidTextEdit(UPass)
            IsDataValidLKP(BranchID)
            IsDataValidLKP(ACCPROFID)
            Dim DT As New DataTable
            DT.Clear()
            DT = CLSAUS.CHECK_USER_NAME(UName.Text.Trim, CodeID.Text, IsUpdate)
            If DT.Rows.Count > 0 Then
                XtraMessageBox.Show(lookAndFeelError, "اسم المستخدم موجود مسبقا يرجى تغيير الاسم", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            DT = CLSAUS.CHECK_USER_NAME(UNameLog.Text.Trim, CodeID.Text, IsUpdate)
            If DT.Rows.Count > 0 Then
                XtraMessageBox.Show(lookAndFeelError, "اسم الدخول موجود مسبقا يرجى تغيير الاسم", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            Dim Empid As Integer
            Dim AccID As ULong
            If IsEmpORUser.SelectedIndex = 0 Then
                Empid = Empployid
                AccID = GVROLEMPID.GetFocusedRowCellValue("EMPAccID")
            Else
                Empid = 0
                AccID = 0
            End If

            CLSAUS.TB_Users_Insert(CodeID.Text.Trim, UName.Text.Trim, UNameLog.Text.Trim, UPass.Text.Trim, IsActiveTG.EditValue,
                                   BranchID.EditValue, ACCPROFID.EditValue, UserType.EditValue, IsUpdate,
                                   GVROLEMPID.GetFocusedRowCellValue("EMPAccID"), Empployid, IsEmpORUser.SelectedIndex, ISHidden.IsOn, phone.Text)
        End If
        NEWRECROD()
        MyBase.UPDATERECORD()
    End Sub

    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        If LSBOX.ItemCount <> -1 Then
            Dim DT As New DataTable
            DT.Clear()
            DT = CLSAUS.TB_Users_LOADUSERBASEDONID(LSBOX.SelectedValue)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                BtnPrint.Enabled = True
                'BtnDelete.Enabled = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                IsEmpORUser.Enabled = False
                BranchID.Enabled = False
                CodeID.Text = LSBOX.SelectedValue
                IsEmpORUser.SelectedIndex = DT.Rows(0)("ISEmpOrUser")
                UNameLog.Text = DT.Rows(0)("UNameLog").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                ACCPROFID.EditValue = DT.Rows(0)("USettingProfileID")
                UserType.EditValue = DT.Rows(0)("UserType")
                UPass.Text = DT.Rows(0)("UPass").ToString
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
                ISHidden.IsOn = DT.Rows(0)("CanShowHACC")
                BranchID_TextChanged(Nothing, Nothing)
                AccID = DT.Rows(0)("EMPAccID")
                EMID.EditValue = DT.Rows(0)("ID")
                UName.Text = DT.Rows(0)("UName").ToString
                phone.Text = DT.Rows(0)("Phone").ToString
                EMID.Enabled = False
                If IsEmpORUser.SelectedIndex = 0 Then
                    AccEmp = DT.Rows(0)("EmpAcc")
                Else
                    AccEmp = 0
                End If
            End If
        End If
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        Else
            LOADEMPACCID()
        End If

    End Sub

    Private Sub EMID_TextChanged(sender As Object, e As EventArgs) Handles EMID.TextChanged
        Try
            If EMID.Text <> String.Empty And IsUpdate = False Then
                If EMID.EditValue = -1 Then Return
                Empployid = GVROLEMPID.GetFocusedRowCellValue("ID")
                UName.Text = EMID.Text
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FRMADDUSER_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        FrmSavedSuccessfully.SimpleLabelItem1.Text = "تم حفظ البيانات بنجاح"
    End Sub

    Private Sub UserTypeForEdit_EditValueChanged(sender As Object, e As EventArgs) Handles UserTypeForEdit.EditValueChanged
        LSBOX.DataSource = Nothing
        If UserTypeForEdit.EditValue <> -1 And UserTypeForEdit.Text <> String.Empty Then
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = UserTypeForEdit.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("Tb_ueser_lodeUeserForProfile", prm)
            If dt.Rows.Count > 0 Then
                LSBOX.DataSource = dt
                LSBOX.DisplayMember = "UName"
                LSBOX.ValueMember = "USID"
            End If
        End If
    End Sub

    Private Sub ComboBoxEdit1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles IsEmpORUser.SelectedIndexChanged
        EMID.EditValue = -1
        UName.Text = ""
        If IsEmpORUser.SelectedIndex = 1 Then
            EMID.Enabled = False
            UName.Enabled = True
        Else
            EMID.Enabled = True
            UName.Enabled = False
        End If
    End Sub
#End Region
End Class