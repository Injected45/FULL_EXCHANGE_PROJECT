Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors

Public Class FRMTRANSTYPE
    Public IsUpdate As Boolean
    Public msgST As Short
    Public Sub NewRecord()
        TransName.Text = String.Empty
        CodeID.Text = GETMAXID("TransTypeTb", "ID") + 1
        LSBOX.DataSource = Nothing
        LSBOX.SelectedIndex = -1
        CountryID.EditValue = -1
        WBP.Buttons(1).Properties.Enabled = True
        WBP.Buttons(2).Properties.Enabled = False
        LOADCOUNTRIES()
        LOADLSBOX()
    End Sub
    Sub LOADCOUNTRIES()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "ID"
            CountryID.Properties.DisplayMember = "CName"
            GLGV1.OptionsView.ShowColumnHeaders = False
            NEWDVGFROMAT(GLGV1)
        End If
    End Sub
    Public Sub LOADLSBOX()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("TransTypeTb_Select")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "TransTypeName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub

    Public Sub TRANSTYPE_INSERT()
        Dim PRM(6) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text.Trim}
        PRM(1) = New SqlParameter("@TransTypeName", SqlDbType.NVarChar, -1) With {.Value = TransName.Text.Trim}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(4) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(5) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(6) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        RUN_EXUTE_PRO("TransTypeTb_Insert", PRM)
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.DevExpressDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        msgST = PRM(4).Value
        If PRM(4).Value = 0 Then
            XtraMessageBox.Show(lookAndFeelError, PRM(5).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
    End Sub
    Public Function TRANSTYPE_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TransTypeTb_SelectByID", PRM)
        Return DT
    End Function
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        Dim DT As New DataTable
        DT.Clear()
        DT = TRANSTYPE_Select(LSBOX.SelectedValue)
        If DT.Rows.Count > 0 Then

            IsUpdate = True
            'BtnDelete.Enabled = True
            WBP.Buttons(2).Properties.Enabled = True
            WBP.Buttons(1).Properties.Enabled = False
            TransName.Text = DT.Rows(0)("TransTypeName").ToString
            CodeID.Text = DT.Rows(0)("ID")
            CountryID.EditValue = DT.Rows(0)("CountryID")
            IsActiveTG.EditValue = DT.Rows(0)("IsActive")
        End If
    End Sub
    Private Sub FRMTRANSTYPE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub
    Private Sub WBP_ButtonClick(sender As Object, e As ButtonEventArgs) Handles WBP.ButtonClick
        Dim tag As String = CType(e.Button, WindowsUIButton).Tag.ToString()
        Select Case tag
            Case "1"
                NewRecord()
            Case "2"
                If IsUpdate = False Then
                    If TransName.Text = String.Empty Then
                        TransName.ErrorText = "هذا الحقل مطلوب"
                        Exit Sub
                    End If
                    If CountryID.Text = String.Empty Or CountryID.EditValue = -1 Then
                        CountryID.ErrorText = "هذا الحقل مطلوب"
                        Exit Sub
                    End If
                    TRANSTYPE_INSERT()
                    If msgST = 1 Then
                        FrmSavedSuccessfully.ShowDialog()
                        NewRecord()
                        FRMCATEGORYTYPES.TransTypeLoad()
                    End If
                End If
            Case "3"
                If IsUpdate = True Then
                    If TransName.Text = String.Empty Then
                        TransName.ErrorText = "هذا الحقل مطلوب"
                        Exit Sub
                    End If
                    If CountryID.Text = String.Empty Or CountryID.EditValue = -1 Then
                        CountryID.ErrorText = "هذا الحقل مطلوب"
                        Exit Sub
                    End If
                    TRANSTYPE_INSERT()
                    If msgST = 1 Then
                        FrmEditMessage.ShowDialog()
                        NewRecord()
                        FRMCATEGORYTYPES.TransTypeLoad()
                    End If
                End If
        End Select

    End Sub

    Private Sub FRMTRANSTYPE_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        FRMCATEGORYTYPES.TransTypeLoad()
    End Sub
End Class