Imports System.Data.SqlClient

Public Class FrmPayIncrease
    Dim clspi As New CLSPAYINCREASE
    Public Property EMBID As Integer
    Public Property IsUpdate As Boolean
    Dim NID As Long

    Sub newRecord()
        IsUpdate = False
        Code.Text = GETMAXID("PayIncrease", "ID") + 1
        IsActiveTG.IsOn = True
        PIName.Text = String.Empty
        PIName.Select()
        PIVal.EditValue = 0.000
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Public Overrides Sub CHECKBUTTONS()
        lodePreportes()
        MyBase.CHECKBUTTONS()
    End Sub



    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(11, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If

    End Sub
    Public Overrides Sub BNew()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If PIName.Text.Trim = "" Then
                PIName.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            Dim DT As New DataTable
            DT.Clear()
            DT = clspi.CHECK_PI_NAME(PIName.Text.Trim)
            If DT.Rows.Count > 0 Then
                PIName.ErrorText = "هذا الاسم موجود مسبقا"
                Exit Sub
            End If

            clspi.INSERTTB__PI(Code.Text.Trim, PIName.Text.Trim, PIVal.EditValue, IsActiveTG.EditValue)
        End If
        newRecord()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If PIName.Text.Trim = "" Then
                PIName.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If

            clspi.UPDATETB_PI(Code.Text.Trim, PIName.Text.Trim, PIVal.EditValue, IsActiveTG.EditValue)
        End If
        newRecord()
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_DATA(X)
        If IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clspi.SERACH_PI(X)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                PIName.Text = DT.Rows(0)("PIName").ToString
                PIVal.EditValue = DT.Rows(0)("PIVal")
                IsActiveTG.IsOn = DT.Rows(0)("IsActive")
            End If
        End If
    End Sub

    Private Sub FrmPayIncrease_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CHECKBUTTONS()
        newRecord()
    End Sub

    Private Sub PictureEdit11_EditValueChanged(sender As Object, e As EventArgs) Handles PictureEdit11.EditValueChanged

    End Sub

    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        FRMVIEWPI.ShowDialog()
    End Sub

    Private Sub FrmPayIncrease_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IsUpdate = False
        newRecord()
    End Sub
End Class
Public Class CLSPAYINCREASE
    Public Function SERACH_PI(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("PayIncrease_Search", PRM)
        Return DT
    End Function
    Public Function CHECK_PI_NAME(ByVal EMPNAME As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, 250)
        PRM(0).Value = EMPNAME.Trim
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("PayIncrease_SEARCH_BYNAME", PRM)
        End If
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB__PI(Code As String, PIName As String, PIVal As Double, ISACTIVE As Boolean)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@PIName", SqlDbType.NVarChar, -1) With {.Value = PIName}
        PRM(2) = New SqlParameter("@PIVal", SqlDbType.NVarChar, -1) With {.Value = PIVal}
        PRM(3) = New SqlParameter("@ISACTIVE", SqlDbType.Bit) With {.Value = ISACTIVE}
        RUN_EXUTE_PRO("PayIncrease_Insert", PRM)
    End Sub
    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_PI(Code As String, PIName As String, PIVal As Double, ISACTIVE As Boolean)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@PIName", SqlDbType.NVarChar, -1) With {.Value = PIName}
        PRM(2) = New SqlParameter("@PIVal", SqlDbType.NVarChar, -1) With {.Value = PIVal}
        PRM(3) = New SqlParameter("@ISACTIVE", SqlDbType.Bit) With {.Value = ISACTIVE}
        RUN_EXUTE_PRO("PayIncrease_UpdateById", PRM)
    End Sub
End Class