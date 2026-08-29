Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMVIEWOPENINBALANCE
    Public IsPartner As Boolean
    'Sub LOADACCOUNTTYPE()
    '    AccountType.Properties.Items.Clear()
    '    Dim Coll As ComboBoxItemCollection = AccountType.Properties.Items
    '    Coll.BeginUpdate()
    '    If BranchID.EditValue = MAINBID Then
    '        Try
    '            Coll.Add(New ACCOUNTSADD("الخزائن"))
    '            Coll.Add(New ACCOUNTSADD("الموظفون"))
    '            Coll.Add(New ACCOUNTSADD("العملاء"))
    '            Coll.Add(New ACCOUNTSADD("الجواري"))
    '            Coll.Add(New ACCOUNTSADD("المصارف"))
    '        Finally
    '            Coll.EndUpdate()
    '        End Try
    '    ElseIf BranchID.EditValue <> MAINBID Then
    '        If IsPartner = True Then
    '            Try
    '                Coll.Add(New ACCOUNTSADD("الخزائن"))
    '                Coll.Add(New ACCOUNTSADD("الموظفون"))
    '                Coll.Add(New ACCOUNTSADD("العملاء"))
    '                Coll.Add(New ACCOUNTSADD("رأس المال"))
    '            Finally
    '                Coll.EndUpdate()
    '            End Try
    '        ElseIf IsPartner = False Then
    '            Try
    '                Coll.Add(New ACCOUNTSADD("الخزائن"))
    '                Coll.Add(New ACCOUNTSADD("الموظفون"))
    '                Coll.Add(New ACCOUNTSADD("العملاء"))
    '            Finally
    '                Coll.EndUpdate()
    '            End Try
    '        End If
    '    End If
    '    AccountType.SelectedIndex = -1
    'End Sub
    'Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
    '    BranchID.Properties.PopulateColumns()
    '    BranchID.Properties.Columns("DBRID").Visible = False
    '    BranchID.Properties.Columns("BranchType").Visible = False
    'End Sub
    'Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = RUN_QUARY_QUERY_ONLY("Select IsPartner from CoBranch where ID='" & BranchID.EditValue & "'")
    '    If DT.Rows.Count > 0 Then
    '        IsPartner = DT.Rows(0)("IsPartner")
    '    End If
    '    AccountType.Properties.Items.Clear()

    'End Sub
    Public Sub LoadData()
        GCRole.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[OpeningBalanceTb_LoadToDVG]", PR)
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

    Private Sub FRMVIEWOPENINBALANCE_Load(sender As Object, e As EventArgs) Handles Me.Load
        DVGFROMAT()
        GCRole.DataSource = Nothing
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

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then

            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            FRMOPENINGBALANCE.SHOWRECORD(CO)
            FRMOPENINGBALANCE.IsUpdate = True
            FRMOPENINGBALANCE.BtnEdit.Enabled = DevExpress.XtraBars.BarItemVisibility.Never
            'FRMOPENINGBALANCE.BtnEdit.Caption = "استرجاع القيمة"
            FRMOPENINGBALANCE.BtnSave.Enabled = False
            FRMOPENINGBALANCE.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        End If
        Me.Close()
    End Sub
End Class