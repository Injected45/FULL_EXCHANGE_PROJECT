Imports System.Data.SqlClient

Public Class FrmAddCancelReason
    Public IsUpdate As Boolean
    'Public Overrides Sub CHECKBUTTONS()
    '    MyBase.CHECKBUTTONS()
    'End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(23, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NEWRECORD()
        InsertDate.EditValue = Date.Now
        IsUpdate = False
        CodeID.Text = GETMAXID("AddCancelReason", "ID") + 1
        Notes.Text = ""
        Notes.Select()

        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnDelete.Enabled = False
        BtnEdit.Enabled = False
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            Dim PRM(2) As SqlParameter
            PRM(0) = New SqlParameter("@NewCause", Notes.Text.Trim)
            PRM(1) = New SqlParameter("@InsertDate", InsertDate.EditValue)
            PRM(2) = New SqlParameter("@SafeID", UserID)
            RUN_EXUTE_PRO("AddCancelReason_Insert", PRM)
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
            Dim PRM(3) As SqlParameter
            PRM(0) = New SqlParameter("@ID", CodeID.Text.Trim)
            PRM(1) = New SqlParameter("@NewCause", Notes.Text.Trim)
            PRM(2) = New SqlParameter("@InsertDate", InsertDate.EditValue)
            PRM(3) = New SqlParameter("@SafeID", UserID)
            RUN_EXUTE_PRO("AddCancelReason_Update", PRM)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub Remove()
        If IsUpdate = True Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ID", CodeID.Text.Trim)
            RUN_EXUTE_PRO("AddCancelReason_DeleteID", PRM)
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_TXT("AddCancelReason_ReseedCounter")
        End If
        NEWRECORD()
        MyBase.Remove()
    End Sub
    Sub GetRecord(x)
        If IsUpdate = True Then
            Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = x
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AddCancelReason_GetReasonByID", PRM)
        If DT.Rows.Count > 0 Then
            CodeID.Text = DT.Rows(0)("ID")
            InsertDate.EditValue = DT.Rows(0)("InsertDate")
            Notes.Text = DT.Rows(0)("NewCause").ToString
            BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            BtnSave.Enabled = False
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
        End If
        End If
    End Sub

    Private Sub FrmAddCancelReason_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub FrmAddCancelReason_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IsUpdate = False
        NEWRECORD()
    End Sub

    Private Sub BtnView_Click(sender As Object, e As EventArgs) Handles BtnView.Click
        FrmViewAddCancelReason.ShowDialog()
    End Sub


End Class