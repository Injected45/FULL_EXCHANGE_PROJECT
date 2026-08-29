Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Public Class FRMDELEGATE
    Public IsUpdate As Boolean
    Dim dcu As New DELEGATECLSS
    Public AccID As ULong
    Public Sub CHECKBUTTONS()
        CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        Dim DT As New DataTable
        DT.Clear()
        DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        If DT.Rows.Count > 0 Then
            If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
            If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True

        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(79, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always



        End If


    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        DNAME.Text = ""
        PHONE1.Text = ""
        PHONE2.Text = ""
        IDNo.Text = ""
        LOADNATIONALITY()
        NationalityID.EditValue = -1

        CodeID.Enabled = False
        DNAME.Select()

        IsActiveTG.IsOn = True
        'BtnDelete.Enabled = False
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("DelegateTb", "ID") + 1
        LOADDATA()
        LSBOX.SelectedIndex = -1
        FRMBBRANCH.LOADDelegate()
    End Sub
    Public Sub LOADNATIONALITY()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("NationalityTb_SelectAll")
        If DT.Rows.Count > 0 Then
            NationalityID.Properties.DataSource = DT
            NationalityID.Properties.DisplayMember = "NATNAME"
            NationalityID.Properties.ValueMember = "ID"
            NationalityID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub NationalityID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles NationalityID.QueryPopUp
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("NationalityTb_SelectAll")
        If DT.Rows.Count > 0 Then
            NationalityID.Properties.PopulateColumns()
            NationalityID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Private Sub NationalityID_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles NationalityID.ButtonClick
        If e.Button.Index = 1 Then
            FRMNATIONALITY.ShowDialog()
        End If
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("DelegateTb_LOADTOLBX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "DNAME"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        For I As Integer = 0 To LSBOX.Items.Count - 1
            Dim DT As New DataTable
            DT.Clear()
            DT = dcu.EMPCSFT_Select(LSBOX.SelectedValue)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                DNAME.Text = DT.Rows(0)("DNAME").ToString
                PHONE1.Text = DT.Rows(0)("PHONE1").ToString
                PHONE2.Text = DT.Rows(0)("PHONE2").ToString
                IDNo.Text = DT.Rows(0)("IDNo").ToString
                NationalityID.EditValue = DT.Rows(0)("NationalityID")
                CodeID.Text = DT.Rows(0)("ID")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            End If
        Next
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            Dim dt As New DataTable
            dt.Clear()
            dt = dcu.EMPCSFT_CHECKPOHNE(PHONE1.Text.Trim, PHONE2.Text.Trim)
            If dt.Rows.Count > 0 Then
                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                XtraMessageBox.AllowCustomLookAndFeel = True
                XtraMessageBox.Show(lookAndFeelError, "أحد أرقام الهواتف موجود مسبقا", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If DNAME.Text = String.Empty Then
                DNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If NationalityID.EditValue = -1 Or NationalityID.Text = String.Empty Then
                NationalityID.ErrorText = "يجب اختيار الجنسية"
                Return
            End If
            IsActiveTG.EditValue = True
            dcu.EMPCSFT_INSERT(CodeID.Text, DNAME.Text.Trim, PHONE1.Text.Trim, PHONE2.Text.Trim, IDNo.Text.Trim, NationalityID.EditValue, IsActiveTG.EditValue, IsUpdate)
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        End If
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If DNAME.Text = String.Empty Then
                DNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If NationalityID.EditValue = -1 Or NationalityID.Text = String.Empty Then
                NationalityID.ErrorText = "يجب اختيار الجنسية"
                Return
            End If
            'IsActiveTG.EditValue = True
            dcu.EMPCSFT_INSERT(CodeID.Text, DNAME.Text.Trim, PHONE1.Text.Trim, PHONE2.Text.Trim, IDNo.Text.Trim, NationalityID.EditValue, IsActiveTG.EditValue, IsUpdate)
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub

    Private Sub FRMDELEGATE_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True 'this will prevent ding sound 
    End Sub

    Private Sub FRMDELEGATE_Load(sender As Object, e As EventArgs) Handles Me.Load
        'CHECKBUTTONS()
        lodePreportes()
        NEWRECORD()
    End Sub
End Class
Public Class DELEGATECLSS
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("DelegateTb_SelectByID", PRM)
        Return DT
    End Function
    Public Function EMPCSFT_SelectAll() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("DelegateTb_SelectAll")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(ID As Integer, DNAME As String, Phone1 As String, Phone2 As String, IDNo As String, NationalityID As Integer, IsActive As Boolean, IsUpdate As Boolean)
        Dim PRM(7) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@DNAME", SqlDbType.NVarChar, -1) With {.Value = DNAME}
        PRM(2) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
        PRM(3) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
        PRM(4) = New SqlParameter("@IDNo", SqlDbType.NVarChar, -1) With {.Value = IDNo}
        PRM(5) = New SqlParameter("@NationalityID", SqlDbType.Int) With {.Value = NationalityID}
        PRM(6) = New SqlParameter("@IsActive", SqlDbType.BigInt) With {.Value = IsActive}
        PRM(7) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("DelegateTb_Insert", PRM)
    End Sub
    Public Function EMPCSFT_CHECKPOHNE(Phone1 As String, Phone2 As String) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
        PRM(1) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("DelegateTb_CHECK_PhoneNo", PRM)
        Return DT
    End Function
End Class