Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FRMViewTransBanks
    Public Sub LoadData()
        GVRole.Columns.Clear()
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = FRMTransBetweenBanks.LOADTYPE}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FRMTransBetweenBanks.BranchID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BankDipWdTb_LOADTODVG", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("TypeID").Visible = False
            GVRole.Columns("العمولة").Visible = False
            NEWDVGFROMAT(GVRole)
        End If
    End Sub

    Private Sub FRMVIEWBANKDEPOSIT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
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
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            Dim TypeID As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("TypeID"))
            FRMTransBetweenBanks.IsUpdate = True
            FRMTransBetweenBanks.LOADBRANCH()
            FRMTransBetweenBanks.LOADTYPE = TypeID
            FRMTransBetweenBanks.BANKDEPOSIT_GetRecord(CO, TypeID)
            FRMTransBetweenBanks.BtnSave.Enabled = False
            FRMTransBetweenBanks.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMTransBetweenBanks.BtnPrint.Enabled = True
            FRMTransBetweenBanks.DISAPLEDCONTROLS()
            FRMTransBetweenBanks.BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            FRMTransBetweenBanks.BtnEdit.Caption = "استرجاع القيمة"
        End If
        Me.Close()
    End Sub
End Class