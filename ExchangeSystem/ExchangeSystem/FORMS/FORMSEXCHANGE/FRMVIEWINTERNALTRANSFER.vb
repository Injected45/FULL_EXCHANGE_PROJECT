Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMVIEWINTERNALTRANSFER
    Public Sub LoadData()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_LOADTODVG", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            NEWDVGFROMAT(GVRole)
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
    Public Function SEARCH_CURRENCY(NAME As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, 300)
        PRM(0).Value = NAME.Trim
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EMPLOYEE_SearchByTitle", PRM)
        Return DT
    End Function
    'Private Sub SearchTxt_TextChanged(sender As Object, e As EventArgs) Handles SearchTxt.TextChanged
    '    If SearchTxt.Text <> String.Empty Then
    '        Dim DT As New DataTable
    '        DT.Clear()
    '        DT = SEARCH_CURRENCY(SearchTxt.Text.Trim)
    '        If DT.Rows.Count > 0 Then
    '            GCRole.DataSource = DT
    '            GVRole.Columns("ID").Caption = "الرقم"
    '            GVRole.Columns("Code").Caption = "الرمز"
    '            GVRole.Columns("EMPNAME").Caption = "اسم الموظف"
    '            GVRole.Columns("StatusString").Caption = "التصنيف"
    '            GVRole.Columns("IsActive").Caption = "الحالة"
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
            Dim co As String = view.GetFocusedRowCellValue("الرمز")
            FRMINTERNALTRANSFER.RecBID = 0
            FRMINTERNALTRANSFER.IsUpdate = False
            'FRMINTERNALTRANSFER.UpdateType = 0
            FRMINTERNALTRANSFER.CodeID.Text = co
            'FRMINTERNALTRANSFER.ShowRecored(co)
        End If
        Me.Close()
    End Sub
End Class