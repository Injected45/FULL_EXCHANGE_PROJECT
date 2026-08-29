Imports System.Data.SqlClient

Public Class CurrencyMain
    Public IsUpdate As Boolean
    Dim cu As New CURNAME
    Public AccID As ULong
    'Public Sub CHECKBUTTONS()
    '    CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
    '    If DT.Rows.Count > 0 Then
    '        If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
    '        If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True

    '    End If
    'End Sub
    Sub NEWRECORD()
        'CHECKBUTTONS()
        IsUpdate = False
        CuName.Text = ""
        CuName.Enabled = True
        CodeID.Enabled = False
        CuName.Select()
        IsActiveTG.IsOn = True
        IBAN.Text = String.Empty
        CurCode.Text = String.Empty
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("CurrencyMainTb", "ID") + 1
        LOADDATA()
        LSBOX.SelectedIndex = -1
        FRMCURRENCY.LOADCURNAME()
    End Sub




    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(6, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If

    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLSBOX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "CuName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
        'CHECKBUTTONS()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            If CuName.Text = String.Empty Then
                CuName.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            Dim dt As New DataTable
            dt = cu.CurrencyMain_CHECK(CuName.Text.Trim)
            If dt.Rows.Count > 0 Then
                CuName.ErrorText = "اسم العملة موجود مسبقاً"
                Exit Sub
            End If
            If IsDefault.Checked = True Then
                Dim DTT As New DataTable
                DTT.Clear()
                DTT = cu.CurrencyMainTb_IsDefaultCHECK
                If DTT.Rows.Count > 0 Then
                    IsDefault.ErrorText = "يوجد عملة افتراضية موجودة مسبقاً"
                    Exit Sub
                End If
            End If
            IsActiveTG.EditValue = True
            'Dim res = WarningMessage(Me, "رسالة تحذير", "سيتم حفظ البيانات ولا يمكن التعديل عليها مرة أخرى، هل تريد الاستمرار؟")
            'If res = DialogResult.Yes Then
            cu.EMPCSFT_INSERT(CodeID.Text, CuName.Text.Trim, IBAN.Text.Trim, CurCode.Text.Trim, IsActiveTG.EditValue, IsUpdate, IsDefault.Checked)
                FrmSavedSuccessfully.Show()
                NEWRECORD()
            'Else
            '    NEWRECORD()
            '    Exit Sub
            'End If
        End If
    End Sub
    Private Sub CurrencyMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        Dim DT As New DataTable
        DT.Clear()
        DT = cu.EMPCSFT_Select(LSBOX.SelectedValue.ToString)
        If DT.Rows.Count > 0 Then
            IsUpdate = True
            BtnSave.Enabled = False
            CuName.Text = DT.Rows(0)("CuName").ToString
            IBAN.Text = DT.Rows(0)("IBAN").ToString
            CurCode.Text = DT.Rows(0)("CurCode").ToString
            CodeID.Text = DT.Rows(0)("ID")
            IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            IsDefault.Checked = DT.Rows(0)("IsDefault")
            CuName.Enabled = False
            IsDefault.Enabled = False
        End If
    End Sub
End Class
Public Class CURNAME
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CurrencyMainTb_Select", PRM)
        Return DT
    End Function
    Public Function CurrencyMain_CHECK(CuName As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CuName", SqlDbType.NVarChar, -1) With {.Value = CuName}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CurrencyMainTb_NAMECHECK", PRM)
        Return DT
    End Function
    Public Function CurrencyMainTb_IsDefaultCHECK() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_IsDefaultCHECK")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(ID As Integer, CuName As String, IBAN As Integer, CurCode As String, IsActive As Boolean, IsUpdate As Boolean, IsDefault As Boolean)
        Dim PRM(6) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@CuName", SqlDbType.NVarChar, -1) With {.Value = CuName}
        PRM(2) = New SqlParameter("@IBAN", SqlDbType.Int) With {.Value = IBAN}
        PRM(3) = New SqlParameter("@CurCode", SqlDbType.NVarChar, -1) With {.Value = CurCode}
        PRM(4) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(5) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(6) = New SqlParameter("@IsDefault", SqlDbType.Bit) With {.Value = IsDefault}
        RUN_EXUTE_PRO("CurrencyMainTb_Insert", PRM)
    End Sub
    Public Sub EMPCSFT_DELETE(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("EmployeeClassificationTb_Delete", PRM)
    End Sub
End Class