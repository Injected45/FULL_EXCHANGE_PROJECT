Imports System.Data.SqlClient

Public Class FRMNATIONALITY
    Public IsUpdate As Boolean
    Dim cu As New NATIONALITY
    'Public Overrides Sub CHECKBUTTONS()
    '    lodePreportes()
    '    MyBase.CHECKBUTTONS()
    'End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(4, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            'If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If


    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        ECNAME.Text = ""
        CodeID.Enabled = False
        ECNAME.Select()
        IsActiveTG.IsOn = True
        BtnDelete.Enabled = False
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        CodeID.Text = GETMAXID("NationalityTb", "ID") + 1
        LOADDATA()
        LSBOX.SelectedIndex = -1
        FRMEMPLOYEE.LOADNATIONALITY()
        FRMDELEGATE.LOADNATIONALITY()
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("NationalityTb_SelectAll")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "NATNAME"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If ECNAME.Text.Trim = "" Then
                ECNAME.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            IsActiveTG.IsOn = True
            cu.EMPCSFT_INSERT(ECNAME.Text.Trim, CodeID.Text.Trim, IsUpdate)
        End If
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If ECNAME.Text.Trim = "" Then
                ECNAME.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            IsActiveTG.IsOn = True
            cu.EMPCSFT_INSERT(ECNAME.Text.Trim, CodeID.Text.Trim, IsUpdate)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub Remove()
        If IsUpdate = True Then
            If ECNAME.Text.Trim = "" Then
                ECNAME.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            cu.EMPCSFT_DELETE(CodeID.Text.Trim)
        End If
        NEWRECORD()
        MyBase.Remove()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        For I As Integer = 0 To LSBOX.Items.Count - 1
            Dim DT As New DataTable
            DT.Clear()
            DT = cu.EMPCSFT_Select(LSBOX.SelectedValue)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                BtnDelete.Enabled = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                ECNAME.Text = DT.Rows(0)("NATNAME").ToString
                CodeID.Text = DT.Rows(0)("ID")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            End If
        Next
    End Sub

    Private Sub FRMNATIONALITY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

End Class
Public Class NATIONALITY
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("NationalityTb_SelectByID", PRM)
        Return DT
    End Function
    Public Function EMPCSFT_SelectAll() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("NationalityTb_SelectAll")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(NATNAME As String, ID As Integer, IsUpdate As Boolean)
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@NATNAME", SqlDbType.NVarChar, -1) With {.Value = NATNAME}
        PRM(1) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("NationalityTb_Insert", PRM)
    End Sub
    Public Sub EMPCSFT_DELETE(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("NationalityTb_Delete", PRM)
    End Sub
End Class