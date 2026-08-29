Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMVIEWEMPWITHDRAWAL
    Public Sub LoadData(tUPE As Integer, BranchID As Integer, LOADTYPE As Integer)
        GVRole.Columns.Clear()
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = LOADTYPE}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PR(2) = New SqlParameter("@accTIPE", SqlDbType.Int) With {.Value = tUPE}
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

    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            Dim TypeID As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("TypeID"))
            If TypeID = 41 Or TypeID = 46 Then
                FRMPROEMPWITHDRAWAL.IsUpdate = True
                FRMPROEMPWITHDRAWAL.LOADBRANCH()
                FRMPROEMPWITHDRAWAL.BranchID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("BranchID"))
                FRMPROEMPWITHDRAWAL.SafeID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("SafeID"))
                FRMPROEMPWITHDRAWAL.SHOW_EMCUSCODE(CO, TypeID)
                FRMPROEMPWITHDRAWAL.BtnSave.Enabled = False
                FRMPROEMPWITHDRAWAL.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                FRMPROEMPWITHDRAWAL.BtnEdit.Enabled = True
                FRMPROEMPWITHDRAWAL.BtnPrint.Enabled = True
                FRMPROEMPWITHDRAWAL.DISAPLEDCONTROLS()
                FRMPROEMPWITHDRAWAL.BtnEdit.Caption = "استرجاع القيمة"
                FRMPROEMPWITHDRAWAL.BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                FRMPROEMPWITHDRAWAL.BtnEdit.Enabled = True
            Else
                FRMEMPWITHDRAWAL.IsUpdate = True
                'FRMEMPWITHDRAWAL.LOADBRANCH()
                FRMEMPWITHDRAWAL.BranchID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("BranchID"))
                FRMEMPWITHDRAWAL.SafeID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("SafeID"))
                FRMEMPWITHDRAWAL.SHOW_EMCUSCODE(CO, TypeID)
                FRMEMPWITHDRAWAL.BtnSave.Enabled = False
                FRMEMPWITHDRAWAL.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                FRMEMPWITHDRAWAL.BtnEdit.Enabled = True
                FRMEMPWITHDRAWAL.BtnPrint.Enabled = True
                Enable_Controls(FRMEMPWITHDRAWAL, False)
                FRMEMPWITHDRAWAL.BtnEdit.Caption = "استرجاع القيمة"
            End If
        End If
        Me.Close()
    End Sub


End Class