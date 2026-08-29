Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewAssest
    Public Sub LoadData()
        GVRole.Columns.Clear()

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CONDB_AddAssestTb_LoadToDVG")
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
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
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("الرمز"))
            FrmAddAssest.IsUpdate = True
            FrmAddAssest.LOADBRANCH()
            FrmAddAssest.Pro_SelectByID(roleId)
            FrmAddAssest.BtnSave.Enabled = False
            FrmAddAssest.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FrmAddAssest.BtnEdit.Enabled = True
            FrmAddAssest.BtnPrint.Enabled = True
        End If
        Me.Close()
    End Sub
End Class