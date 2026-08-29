
Imports DevExpress
Imports System.Data.SqlClient
Public Class FrmUpdateUEsers2

    Public Sub lodePreportes()
        Dim dt As New DataTable

        dt.Clear()
        dt = SElectUEserFormButtn(135, UserID)
        'If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always




    End Sub
    Public Function LOAD_COBRANCHWithOutPRM() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit2")
        Return DT
    End Function
    Sub LOADBRANCHES()
        Dim DT As New DataTable
        DT.Clear()
        DT = LOAD_COBRANCHWithOutPRM()
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ValueMember = "DBRID"

        End If
    End Sub

    Public Sub Get_ueser_from_UPdate()
        Try


            Dim dt As New DataTable
            dt.Clear()
            LOADBRANCHES()


            ID_ueser.Text = UserID
            Name_forUeser.Text = GetUserName


            dt = RUN_QUARY_TXT("select * from TB_Users where USID='" & UserID & "'")
            If dt.Rows.Count > 0 Then
                UPass.Properties.TextEditStyle = XtraEditors.Controls.TextEditStyles.Standard
                UPass.Text = dt.Rows(0)("UPass").ToString
                BranchID.EditValue = dt.Rows(0)("BranchID")
                ACCID.EditValue = dt.Rows(0)("ACCID")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub FrmUpdateUEsers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        lodePreportes()
        BtnSave.Caption = "تغير اعدادات الدخول او كلمة المرور "
        Get_ueser_from_UPdate()
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        LOADSAFEID()
    End Sub
    Sub LOADSAFEID()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ACCBRACN", BranchID.EditValue)
        'PRM(1) = New SqlParameter("@ACCform", 1)
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ACCOUNTSTB_lodebranch_safueser", PRM)
        If DT.Rows.Count > 0 Then
            ACCID.Properties.DataSource = DT
            ACCID.Properties.ValueMember = "ACCID"
            ACCID.Properties.DisplayMember = "ACCNAME"

        End If
    End Sub

    Public Sub UpdateUserForAccount()
        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@UserId", SqlDbType.Int) With {.Value = ID_ueser.EditValue}
        prm(1) = New SqlParameter("@BranchId", SqlDbType.Int) With {.Value = BranchID.EditValue}
        prm(2) = New SqlParameter("@AccAccount", SqlDbType.Int) With {.Value = ACCID.EditValue}
        prm(3) = New SqlParameter("@UPass", SqlDbType.Int) With {.Value = UPass.Text}
        prm(4) = New SqlParameter("@msgstatue", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("UpdateUserForAccount", prm)
        If prm(4).Value = 0 Then
            SplashScreenManager1.CloseWaitForm()
            Exit Sub
        Else
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show("الرجاء اغلاء النظام لتمم عملية التحديث من الجديد ", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    Public Overrides Sub SetData()
        If ID_ueser.EditValue = -1 Then
            ID_ueser.ErrorText = "هذه الحقل مطلوب"
            Return
        End If

        If Name_forUeser.Text = String.Empty Then
            Name_forUeser.ErrorText = "هذه الحقل مطلوب"
            Return
        End If

        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "هذه الحقل مطلوب"
            Return
        End If



        If ACCID.EditValue = -1 Then
            ACCID.ErrorText = "هذه الحقل مطلوب"
            Return
        End If

        If ACCID.Text = String.Empty Then
            ACCID.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        SplashScreenManager1.ShowWaitForm()

        UpdateUserForAccount()

        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Private Sub UPass_ButtonClick(sender As Object, e As XtraEditors.Controls.ButtonPressedEventArgs) Handles UPass.ButtonClick
        If UPass.Properties.UseSystemPasswordChar = True Then
            UPass.Properties.UseSystemPasswordChar = False
        Else
            UPass.Properties.UseSystemPasswordChar = True
        End If
    End Sub
End Class