Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMViewIndividualSalaryByEMP
    Private Sub FRMViewiIndivdualSalaryByEMP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADDATA()
    End Sub
    Sub LOADDATA()
        Dim DTT As New DataTable
        DTT.Clear()
        If FrmIndividualSalaryEMP.DataBaseType = 1 Then
            DTT = RUN_QUARY_PRO_ONLY("SalaryCalculationTb_ViewIndividualSalaryByEMP")
        Else
            DTT = RUN_QUARY_PRO_ONLY("CONDB_SalaryCalculationTb_ViewIndividualSalaryByEMP")
        End If
        If DTT.Rows.Count > 0 Then
            GCRole.DataSource = DTT
            NEWDVGFROMAT(GVRole)
            GVRole.Columns("ID").Visible = False
        Else
            ErrorMessage(Me, "رسالة معلومات", "لا يوجد مرتبات تم احتسابها خلال هذا الشهر")
            Me.Dispose()
            Exit Sub
        End If
    End Sub

    Private Sub GVRole_RowClick(sender As Object, e As RowClickEventArgs) Handles GVRole.RowClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
            Dim MDATE As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("الشهر"))
            FrmIndividualSalaryEMP.IsUpdate = True
            LOADBRNCHHasEmp(FrmIndividualSalaryEMP.BranchID)
            FrmIndividualSalaryEMP.MonthDate = MDATE
            FrmIndividualSalaryEMP.sid = roleId
            FrmIndividualSalaryEMP.EMPWITHBRANCH(FrmIndividualSalaryEMP.BranchID.EditValue, MDATE)
            FrmIndividualSalaryEMP.SHOW_RECORD(roleId)
        End If
        Me.Close()
    End Sub
End Class