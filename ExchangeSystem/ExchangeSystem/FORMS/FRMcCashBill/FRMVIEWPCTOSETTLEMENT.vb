Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMVIEWPCTOSETTLEMENT
    Public Sub LoadData()
        GCRole.DataSource = Nothing
        If FRMPettyCashSettlement.BranchID.Text = String.Empty Or FRMPettyCashSettlement.BranchID.EditValue = -1 Then
            FRMPettyCashSettlement.BranchID.ErrorText = "يجب اختيار الفرع"
            Me.Close()
            Return
        End If
        If FRMPettyCashSettlement.EMPID.Text = String.Empty Or FRMPettyCashSettlement.EMPID.EditValue = -1 Then
            FRMPettyCashSettlement.EMPID.ErrorText = "يجب اختيار الموظف"
            Me.Close()
            Return
        End If
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FRMPettyCashSettlement.BranchID.EditValue}
        PR(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = FRMPettyCashSettlement.EMID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[PettyCashTb_LOADTOSETLEMENT]", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFROMAT()
        End If
    End Sub

    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

    End Sub

    Private Sub FRMVIEWPCTOSETTLEMENT_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = "عهد الموظف" & Space(1) & FRMPettyCashSettlement.EMPID.Text
        DVGFROMAT()
        GVRole.OptionsBehavior.Editable = False
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
            Dim COVAL As Decimal = view.GetFocusedRowCellValue("القيمة").ToString
            FRMPettyCashSettlement.ISID.Text = CO
            FRMPettyCashSettlement.PCVal.EditValue = COVAL

        End If
        Me.Close()
    End Sub
End Class