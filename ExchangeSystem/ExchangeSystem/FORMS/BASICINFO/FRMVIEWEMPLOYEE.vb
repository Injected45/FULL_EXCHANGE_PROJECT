Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FRMVIEWEMPLOYEE

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Me.Close()
    End Sub
    Public Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        GCRole.DataSource = Nothing
        If FRMEMPLOYEE.DataBaseType = 1 Then
            DT = RUN_QUARY_TXT("EMPLOYEE_LoadDataIntoDataGridview")
        Else
            DT = RUN_QUARY_TXT("CONDB_EMPLOYEE_LoadDataIntoDataGridview")
        End If
        GVRole.ShowFindPanel()
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            If FRMEMPLOYEE.DataBaseType = 1 Then
                GVRole.Columns("ConstantInc").Visible = True
                GVRole.Columns("AnotherInc").Visible = True
                GVRole.Columns("Disconts").Visible = True
            Else
                GVRole.Columns("ConstantInc").Visible = False
                GVRole.Columns("AnotherInc").Visible = False
                GVRole.Columns("Disconts").Visible = False
            End If

            For i As Integer = 0 To GVRole.Columns.Count - 1
                GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
                GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            Next
        End If
    End Sub

    Private Sub FRMVIEWEMPLOYEE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        GVRole.OptionsBehavior.Editable = False

        'Dim DT As New DataTable
        'DT.Clear()
        'DT = CHECKOPERATIONS_FalseOrTrue(24, GProfIDLog)
        'If BtnView.Visibility = DT.Rows(0).Item("CanShow") = True Then BtnView.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

    End Sub
    'Public Function SEARCH_CURRENCY(NAME As String) As DataTable
    '    Dim PRM(0) As SqlParameter
    '    PRM(0) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, 300)
    '    PRM(0).Value = NAME.Trim
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = RUN_QUARY_PRO("EMPLOYEE_SearchByTitle", PRM)
    '    Return DT
    'End Function
    'Private Sub SearchTxt_TextChanged(sender As Object, e As EventArgs) Handles SearchTxt.TextChanged
    '    If SearchTxt.Text <> String.Empty Then
    '        GCRole.DataSource = Nothing

    '        Dim DT As New DataTable
    '        DT.Clear()
    '        DT = SEARCH_CURRENCY(SearchTxt.Text.Trim)
    '        If DT.Rows.Count > 0 Then

    '            GCRole.DataSource = DT
    '            'GVRole.Columns("ID").Caption = "الرقم"
    '            'GVRole.Columns("Code").Caption = "الرمز"
    '            'GVRole.Columns("EMPNAME").Caption = "اسم الموظف"
    '            'GVRole.Columns("ECNAME").Caption = "التصنيف"
    '            'GVRole.Columns("IsActive").Caption = "الحالة"
    '            For i As Integer = 0 To GVRole.Columns.Count - 1
    '                GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    '                GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    '                GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    '                GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
    '            Next
    '        End If
    '    Else
    '        LoadData()
    '    End If
    'End Sub
    Dim clse As New CLSEMPLOYEE
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
            Dim CO As String = view.GetFocusedRowCellValue("Code").ToString
            'Dim CO As String = view.GetFocusedRowCellValue("CODE").ToString
            FRMEMPLOYEE.IsUpdate = True
            FRMEMPLOYEE.BtnSave.Enabled = False
            FRMEMPLOYEE.BtnEdit.Enabled = True
            FRMEMPLOYEE.BtnPrint.Enabled = True
            'FRMEMPLOYEE.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMEMPLOYEE.EMBID = roleId
            FRMEMPLOYEE.EMNAME = view.GetFocusedRowCellValue("EMPNAME").ToString
            'FRMEMPLOYEE.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMEMPLOYEE.SHOW_EMPBID(roleId)

        End If
        Me.Close()
    End Sub

    Private Sub BarButtonItem11_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnView.ItemClick
        Dim frm As FRMEMPLOYEE = New FRMEMPLOYEE
        LoadData()
        frm.Show()
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


End Class