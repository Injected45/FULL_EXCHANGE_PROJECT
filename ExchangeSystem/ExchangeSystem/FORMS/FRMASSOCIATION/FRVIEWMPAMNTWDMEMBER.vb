Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid
Imports System.ComponentModel
Imports System.Data.SqlClient
Public Class FRMVIEWMPAMNTWDMEMBER
    Public Sub LoadData()
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = FRMPAMNTWDMEMBER.LOADTYPE}
        PR(1) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = AssID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("MPAMNTWDMEMBERTB_LOADTODVG", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("AssID").Visible = False
            GVRole.Columns("SafeID").Visible = False
            GVRole.Columns("TypeID").Visible = False
            If FRMPAMNTWDMEMBER.LOADTYPE = 22 Then
                GVRole.Columns("MBACCID").Visible = False
                GVRole.Columns("MBACCID").Visible = False
                GVRole.Columns("MBACCID").Visible = False
            End If
            NEWDVGFROMAT(GVRole)
        End If
    End Sub
    Sub LOADASS()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("ASSOCIATIONNAMETB_LOADTODVG")
        If dt.Rows.Count > 0 Then
            AssID.Properties.DataSource = dt
            AssID.Properties.ValueMember = "ID"
            AssID.Properties.DisplayMember = "ASSNAME"
            AssID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub FRVIEWMPAMNTWDMEMBER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADASS()
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
            FRMPAMNTWDMEMBER.IsUpdate = True
            FRMPAMNTWDMEMBER.LOADSafeID()
            FRMPAMNTWDMEMBER.LOADASS()
            FRMPAMNTWDMEMBER.AssID.EditValue = Convert.ToInt32(view.GetFocusedRowCellValue("AssID"))
            If FRMPAMNTWDMEMBER.LOADTYPE = 22 Then
                FRMPAMNTWDMEMBER.MEBACCID = Convert.ToInt64(view.GetFocusedRowCellValue("MBACCID"))
            End If
            FRMPAMNTWDMEMBER.SHOW_EMCUSCODE(CO, TypeID)
            FRMPAMNTWDMEMBER.BtnSave.Enabled = False
            FRMPAMNTWDMEMBER.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMPAMNTWDMEMBER.BtnEdit.Enabled = True
            FRMPAMNTWDMEMBER.BtnPrint.Enabled = True
            FRMPAMNTWDMEMBER.DISAPLEDCONTROLS()
            FRMPAMNTWDMEMBER.BtnEdit.Caption = "استرجاع القيمة"
        End If
        Me.Close()
    End Sub
    Private Sub AssID_TextChanged(sender As Object, e As EventArgs) Handles AssID.TextChanged
        LoadData()
    End Sub

    Private Sub AssID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AssID.QueryPopUp
        AssID.Properties.PopulateColumns()
        AssID.Properties.Columns("ID").Visible = False
    End Sub
End Class