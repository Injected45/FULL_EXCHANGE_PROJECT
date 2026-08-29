Imports System.Data.SqlClient
Imports DevExpress.XtraRichEdit.Model

Public Class FRMADDASSOCIATION
    Public IsUpdate As Boolean
    Dim cu As New ADDASSOCIATIONCLSS
    Public AccID, ASSOACCID As ULong

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(91, UserID)

        If dt.Rows.Count > 0 Then

            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always




        End If


    End Sub
    Sub NEWRECORD()

        IsUpdate = False
        BANKNAME.Text = ""
        CodeID.Enabled = False
        BANKNAME.Select()
        IsActiveTG.IsOn = True
        'BtnDelete.Enabled = False
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("ASSOCIATIONNAMETB", "ID") + 1
        LOADDATA()
        LSBOX.SelectedIndex = -1
        FRMADDMEMBER.LOADASSOCIATION()
        'FRMEMPLOYEE.LOADCLASSIFICATION()
    End Sub

    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ASSOCIATIONNAMETB_LOADTODVG")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "ASSNAME"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        NEWRECORD()

    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If IsUpdate = False Then
            If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            cu.EMPCSFT_INSERT(CodeID.Text, BANKNAME.Text.Trim, IsUpdate, IsActiveTG.EditValue, MAINBID, AssValue.EditValue, 1)
            FrmSavedSuccessfully.Show()
            NEWRECORD()
        End If
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If BANKNAME.Text = String.Empty Then
                BANKNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            'IsActiveTG.EditValue = True
            cu.EMPCSFT_INSERT(CodeID.Text, BANKNAME.Text.Trim, IsUpdate, IsActiveTG.EditValue, MAINBID, AssValue.EditValue, 1)
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub

    Private Sub FRMADDASSOCIATION_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
        For I As Integer = 0 To LSBOX.Items.Count - 1
            Dim DT As New DataTable
            DT.Clear()
            DT = cu.EMPCSFT_Select(LSBOX.SelectedValue)
            If DT.Rows.Count > 0 Then
                IsUpdate = True
                'BtnDelete.Enabled = True
                BtnEdit.Enabled = True
                BtnSave.Enabled = False
                BANKNAME.Text = DT.Rows(0)("ASSNAME").ToString
                CodeID.Text = DT.Rows(0)("ID")
                AssValue.EditValue = DT.Rows(0)("AssValue")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
                AccID = DT.Rows(0)("AccID")
                ASSOACCID = DT.Rows(0)("ASSOACCID")
            End If
        Next
    End Sub

    Private Sub FRMADDASSOCIATION_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        FRMADDMEMBER.LOADASSOCIATION()
    End Sub
End Class
Public Class ADDASSOCIATIONCLSS
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ASSOCIATIONNAMETB_Select", PRM)
        Return DT
    End Function
    Public Function EMPCSFT_SelectAll() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("EmployeeClassificationTb_SelectAll")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(ID As Integer, BankName As String, IsUpdate As Boolean, IsActive As Boolean, BranchID As Integer, AssValue As Double, INSERTTYPE As Integer)
        Dim PRM(6) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@ASSNAME", SqlDbType.NVarChar, -1) With {.Value = BankName}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(3) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(5) = New SqlParameter("@AssValue", SqlDbType.Decimal) With {.Value = AssValue}
        PRM(6) = New SqlParameter("@INSERTTYPE", SqlDbType.Int) With {.Value = 1}
        RUN_EXUTE_PRO("ASSOCIATIONNAMETB_Insert", PRM)
    End Sub
    Public Sub EMPCSFT_DELETE(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("EmployeeClassificationTb_Delete", PRM)
    End Sub
End Class