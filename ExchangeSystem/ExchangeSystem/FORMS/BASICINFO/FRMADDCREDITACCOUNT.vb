Imports System.Data.SqlClient

Public Class FRMADDCREDITACCOUNT
    Public IsUpdate As Boolean
    Dim acct As New ACCCREDIT


    Const designWidth As Single = 1024.0F
    Const designHeight As Single = 768.0F
    Const defaultFont As Single = 8.2F
    Private runtimeWidth As Integer = Screen.PrimaryScreen.Bounds.Width
    Private runtimeHeight As Integer = Screen.PrimaryScreen.Bounds.Height


    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(8, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub NEWRECORD()
        Code.Text = GETMAXID("CreditAccountsTb", "ID") + 1
        InsertDate.EditValue = Date.Now
        AccName.Text = String.Empty
        SPhone1.Text = String.Empty
        SPhone2.Text = String.Empty
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LOADBRNCHDIERCT(BranchID)
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        IsUpdate = False
    End Sub

    Sub Show_Crditaccount(x)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = x}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[CreditAccountsTb_SelectALL]", PR)
        If DT.Rows.Count > 0 Then

            Code.Text = DT.Rows(0)("Code").ToString
            IsActiveTG.IsOn = DT.Rows(0)("isactive")
            BranchID.EditValue = DT.Rows(0)("BranchID")
            AccName.EditValue = DT.Rows(0)("AccName")
            SPhone1.EditValue = DT.Rows(0)("Phone1")
            SPhone2.EditValue = DT.Rows(0)("Phone2")
            BranchID.Enabled = False
        End If
        'End If
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub SetData()
        If AccName.Text = String.Empty Then
            AccName.ErrorText = "الاسم لا يجب أن يكون فارغا"
            Exit Sub
        End If
        Dim DT As New DataTable
        DT.Clear()
        DT = acct.CHECK_ACCCREDIT_NAME(AccName.Text.Trim)
        If DT.Rows.Count > 0 Then
            AccName.ErrorText = "اسم الحساب موجود مسبقا"
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If
        acct.CreditAccountsTb_Insert(Code.Text, AccName.Text.Trim, SPhone1.Text.Trim, SPhone2.Text.Trim, BranchID.EditValue, IsActiveTG.EditValue, IsUpdate, Code.Text)
        NEWRECORD()
        MyBase.SetData()
    End Sub

    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then


            If AccName.Text = String.Empty Then
                AccName.ErrorText = "الاسم لا يجب أن يكون فارغا"
                Exit Sub
            End If
            Dim DT As New DataTable
            DT.Clear()
            DT = acct.CHECK_ACCCREDIT_NAME(AccName.Text.Trim)
            If DT.Rows.Count > 0 Then
                AccName.ErrorText = "اسم الحساب موجود مسبقا"
            End If
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يجب اختيار الفرع"
                Exit Sub
            End If
            acct.CreditAccountsTb_Insert(Code.Text, AccName.Text.Trim, SPhone1.Text.Trim, SPhone2.Text.Trim, BranchID.EditValue, IsActiveTG.EditValue, IsUpdate, Code.Text)
            NEWRECORD()
        End If
        MyBase.UPDATERECORD()
    End Sub

    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub


    Private Sub FRMADDCREDITACCOUNT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim width_ratio As Single = (runtimeWidth / designWidth)
        'Dim heigh_ratio As Single = (runtimeHeight / designHeight)
        'Dim scale As SizeF = New SizeF(width_ratio, heigh_ratio)
        'Me.Scale(scale)
        'Dim designRatio As Single = designWidth / designHeight
        'Dim runtimeRatio As Single = runtimeWidth / runtimeHeight
        'Dim fontPercentageF As Single = designRatio * 100 / runtimeRatio
        'Dim defaultFontResultiveDimention As Single = defaultFont * fontPercentageF / 100

        'For Each control As Control In Me.Controls
        '    Dim resultantFont As Single = control.Font.SizeInPoints * fontPercentageF / 100
        '    control.Font = New Font("Microsoft Sans Serif", resultantFont)
        'Next
        'Me.StartPosition = FormStartPosition.CenterScreen
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMViewADDCREDITACCOUNT.ShowDialog()
    End Sub
End Class

Public Class ACCCREDIT
    Public Function CHECK_ACCCREDIT_NAME(ByVal AccName As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@AccName", SqlDbType.NVarChar, 250) With {.Value = AccName.Trim}
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("CreditAccountsTb_CHECKNAME", PRM)
        End If
        Return DT

    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub CreditAccountsTb_Insert(Code As String, AccName As String, Phone1 As String, Phone2 As String,
                                         BranchID As Integer, IsActive As Boolean, IsUpdate As Boolean, ID As Integer)
        Dim PRM(7) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1) With {.Value = AccName}
        PRM(2) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
        PRM(3) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
        PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(5) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(6) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(7) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("CreditAccountsTb_Insert", PRM)

    End Sub

    Public Sub CreditAccountsTb_Update(Code As String, AccName As String, Phone1 As String, Phone2 As String,
                                         BranchID As Integer, IsActive As Boolean, IsUpdate As Boolean, ID As Integer)
        Dim PRM(7) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1) With {.Value = AccName}
        PRM(2) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
        PRM(3) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
        PRM(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(5) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(6) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(7) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("CreditAccountsTb_Insert", PRM)

    End Sub


End Class