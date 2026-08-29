Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FRMVIEWFRMEMPWITHDRAWALNEW
    Public Sub LoadData()
        GVRole.Columns.Clear()
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = FRMEMPWITHDRAWALNEW.LOADTYPE}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FRMEMPWITHDRAWALNEW.BranchID.EditValue}
        PR(2) = New SqlParameter("@accTIPE", SqlDbType.Int) With {.Value = 1}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_LOADTODVG", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("BranchID").Visible = False
            GVRole.Columns("SafeID").Visible = False
            GVRole.Columns("TypeID").Visible = False
            NEWDVGFROMAT(GVRole)
        End If
    End Sub

    Private Sub FRMVIEWEMPWITHDRAWAL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        'NEWDVGFROMAT(GVRole)
        'GVRole.OptionsBehavior.Editable = False
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.Click
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            Dim TypeID As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("TypeID"))
            FRMEMPWITHDRAWALNEW.IsUpdate = True
            FRMEMPWITHDRAWALNEW.LOADBRANCH()
            FRMEMPWITHDRAWALNEW.BranchID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("BranchID"))
            FRMEMPWITHDRAWALNEW.SafeID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("SafeID"))
            FRMEMPWITHDRAWALNEW.SHOW_EMCUSCODE(CO, 5)
            FRMEMPWITHDRAWALNEW.BtnSave.Enabled = False
            FRMEMPWITHDRAWALNEW.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            FRMEMPWITHDRAWALNEW.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMEMPWITHDRAWALNEW.BtnEdit.Enabled = True
            FRMEMPWITHDRAWALNEW.BtnPrint.Enabled = True
            FRMEMPWITHDRAWALNEW.DISAPLEDCONTROLS()
            FRMEMPWITHDRAWALNEW.BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        End If
        Me.Close()
    End Sub
End Class