Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FRMViewModifyingAccountingEntries
    Private Sub LoadData()
        Dim dt As DataTable = RUN_QUARY_PRO_ONLY("ZRPT_ModifyingAccountingEntries_TB_LOADTODVG")
        GCRole.DataSource = dt
        'GVRole.Columns("ID").Caption = "الرقم"

        DVGFormat(GVRole)
    End Sub
    'Public Sub CHECKBUTTONS()
    '    CHECKOPERATIONS_FalseOrTrue(2, GProfIDLog)
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = CHECKOPERATIONS_FalseOrTrue(2, GProfIDLog)
    '    If BtnView.Visibility = DT.Rows(0).Item("CanShow") = True Then BtnView.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    'End Sub
    Private Sub ViewRolesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try


            LoadData()
            DVGFormat(GVRole)
            GVRole.OptionsBehavior.Editable = False
            'CHECKBUTTONS()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            Dim roleId As String = view.GetFocusedRowCellValue("الرمز").ToString
            ModifyingAccountingEntries.IsUpdate = True
            ModifyingAccountingEntries.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ModifyingAccountingEntries.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            ModifyingAccountingEntries.BtnSave.Enabled = DevExpress.XtraBars.BarItemVisibility.Never
            ModifyingAccountingEntries.ENABLEDCONTROLS(False)
            ModifyingAccountingEntries.SHOWRECORD(roleId)
            Me.Close()
        End If

    End Sub
End Class