Imports System.Data.SqlClient

Public Class FrmProMeasurmentUnit
    Public IsUpdate As Boolean, msgST As Integer
    Dim cu As New BANKCLSS
    Public AccID As ULong

    Public Sub lodePreportes()
        'Dim dt As New DataTable
        'dt.Clear()
        'dt = SElectUEserFormButtn(79, UserID)

        'If dt.Rows.Count > 0 Then
        '    If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        '    If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        'End If
    End Sub
    Public Sub CHECKBUTTONS()
        'CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        'If DT.Rows.Count > 0 Then
        '    If BtnSave.Visible = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visible = True
        '    If BtnEdit.Visible = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visible = True
        'End If
    End Sub
    Sub NEWRECORD()
        CHECKBUTTONS()
        IsUpdate = False
        ExName.Text = ""
        CodeID.Enabled = False
        ExName.Select()
        IsActiveTG.IsOn = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("ContractDB.dbo.MeasurmentUnitTb", "ID") + 1
        LSBOX.SelectedIndex = -1
        LOADDATA()
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CONDB_MeasurmentUnitTb_LoadToLSBOX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "MUName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()
        CHECKBUTTONS()
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            If ExName.Text = String.Empty Then
                ExName.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            EMPCSFT_INSERT()
            FrmSavedSuccessfully.Show()
            NEWRECORD()
            FrmImportItems.LOADMUID()
        End If
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If ExName.Text = String.Empty Then
                ExName.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            EMPCSFT_INSERT()
            FrmEditMessage.Show()
            NEWRECORD()
            FrmImportItems.LOADMUID()
        End If
    End Sub
    Private Sub FRMBANK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        Dim DT As New DataTable
        DT.Clear()
        DT = EMPCSFT_Select(LSBOX.SelectedValue)
        If DT.Rows.Count > 0 Then
            IsUpdate = True
            BtnEdit.Enabled = True
            BtnSave.Enabled = False
            ExName.Text = DT.Rows(0)("MUName").ToString
            CodeID.Text = DT.Rows(0)("ID")
            IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            'AccID = DT.Rows(0)("AccID")
        End If
    End Sub
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CONDB_MeasurmentUnitTb_SelectByID", PRM)
        Return DT
    End Function

    Public Sub EMPCSFT_INSERT()
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text.Trim}
        PRM(1) = New SqlParameter("@MUName", SqlDbType.NVarChar, -1) With {.Value = ExName.Text.Trim}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(3) = New SqlParameter("@IsActive ", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
        RUN_EXUTE_PRO("CONDB_MeasurmentUnitTb_Insert", PRM)
    End Sub

End Class