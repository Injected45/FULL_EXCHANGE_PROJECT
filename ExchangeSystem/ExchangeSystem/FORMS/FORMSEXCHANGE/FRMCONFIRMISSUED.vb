Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraPrinting.Native

Public Class FRMCONFIRMISSUED
    Dim inscls As New CLSINTERNALTRANSFER
    Dim clsaccsa As New CLSAccSafeActivity
    Dim clsagent As New CLSAGENTACTIVITY
    Private DT As New DataTable
    Public RBRID, DBRID, RBRTYPE, DBRTYPE As Integer
    Public DiscountVal, HandallExVal, HandallExVal2 As Decimal
    Public DiscountStatus, DiscountCancel, ConfirmCancelRequest, DiscountST, ISHandallEX As Boolean
    Public isisdcode As String
    Private Delegate Sub UICallback()
    Dim dd As String
    Sub LOADDATA()
        Try
            GCROLE.DataSource = Nothing
            Dim Type As Integer
            If RB1.Checked = True Then
                Type = 1
            ElseIf RB2.Checked = True Then
                Type = 2
            ElseIf RB3.Checked = True Then
                Type = 3
            ElseIf RB4.Checked = True Then
                Type = 4
            Else
                Type = 0
            End If
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = Type}
            LoadToControlar(GCROLE, "InternalEx_LOADTOCONFIRM", "", "", PR)
            DVGFormat(GVROLE)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub FRMCONFIRMISSUED_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADDATA()
        DVGFormat(GVROLE)
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVROLE.CustomUnboundColumnData
        If e.Column.FieldName = "RowHandle" And e.IsGetData Then
            e.Value = GVROLE.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub BtnConfirm_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BtnConfirm.ButtonClick
        Try
            Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
            If RB1.Checked = True Then
                FRMINTERNALTRANSFER.ConfirmType = 1
                FRMINTERNALTRANSFER.NEWRECORD()
                FRMINTERNALTRANSFER.ShowCurrentRecord(iscode)
                FRMINTERNALTRANSFER.ShowDialog()
                If FRMINTERNALTRANSFER.MsgStatus = 1 Then
                    CONFIRMMESSAGE.Show()
                End If
                LOADDATA()

            ElseIf RB3.Checked = True Then
                FRMINTERNALTRANSFER.ConfirmType = 5
                FRMINTERNALTRANSFER.NEWRECORD()
                FRMINTERNALTRANSFER.ShowCurrentRecord(iscode)
                FRMINTERNALTRANSFER.ShowDialog()
                LOADDATA()
            ElseIf RB2.Checked = True Then
                FRMEXTERNALTRANS.ConfirmType = 1
                FRMEXTERNALTRANS.ISUpdate = 1
                FRMEXTERNALTRANS.SHOW_RECORD(iscode)
                FRMEXTERNALTRANS.ShowDialog()
                GCROLE.DataSource = Nothing
                LOADDATA()
            ElseIf RB4.Checked = True Then
                Dim brid As Integer = GVROLE.GetFocusedRowCellValue("Brid")
                RBRTYPE = 0
                If brid > 0 And IsDBNull(brid) = False Then
                    DT = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & brid & "'")
                    RBRTYPE = DT.Rows(0)("BranchType")
                Else
                    RBRTYPE = 0
                End If
                If RBRTYPE = 3 Then
                    FRMEXTERNALTRANS.ConfirmType = 3
                    FRMEXTERNALTRANS.ISUpdate = 1
                    FRMEXTERNALTRANS.SHOW_RECORD(iscode)
                    FRMEXTERNALTRANS.ShowDialog()
                    GCROLE.DataSource = Nothing
                    LOADDATA()
                    CONFIRMMESSAGE.Show()
                Else
                    RUN_EXUTE_TXT("Update ExternalEx  Set ConfirmCanceledDate = '" & Date.Now.ToString("yyyy-MM-dd") & "',ConfirmCanceledSafeID = " & UserID & ",ConfirmCancelBranch=" & BID & ", IsCanceled= 2 Where Code =N'" & iscode & "'")
                End If
                LOADDATA()
            End If

        Catch ex As Exception
            ErrorMessage2("رسالة تنبية", ex.Message)
        End Try

    End Sub

    Private Sub RB4_CheckedChanged(sender As Object, e As EventArgs) Handles RB4.CheckedChanged
        LOADDATA()
    End Sub
    Public Sub RB1_CheckedChanged(sender As Object, e As EventArgs) Handles RB1.CheckedChanged
        LOADDATA()
    End Sub
    Public Sub RB3_CheckedChanged(sender As Object, e As EventArgs) Handles RB3.CheckedChanged
        LOADDATA()
    End Sub


    Private Sub GCROLE_KeyDown(sender As Object, e As KeyEventArgs) Handles GCROLE.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub

    Private Sub RB2_CheckedChanged(sender As Object, e As EventArgs) Handles RB2.CheckedChanged
        LOADDATA()
    End Sub



End Class