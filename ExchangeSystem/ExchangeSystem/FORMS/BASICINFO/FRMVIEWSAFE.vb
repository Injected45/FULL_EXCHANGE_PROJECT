Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid
Public Class FRMVIEWSAFE
    Dim clas As New CLSSAFE
    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Me.Close()
    End Sub
    Public Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SAFETB_LoadDataIntoDataGridview")

        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("ID").Caption = "الرقم"
            GVRole.Columns("Code").Caption = "الرمز"
            GVRole.Columns("SafeName").Caption = "اسم الخزنة"
            GVRole.Columns("StatusString").Caption = "طبيعة الحساب"
            GVRole.Columns("IsActive").Caption = "الحالة"
            For i As Integer = 0 To GVRole.Columns.Count - 1
                GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
                GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            Next
        End If
    End Sub

    Private Sub FRMVIEWSAFE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        '    GVRole.Columns.Add(New DevExpress.XtraGrid.Columns.GridColumn() With {
        '.Caption = "طبيعة الحساب",
        '.FieldName = "ST",
        '.UnboundDataType = GetType(String),
        '.Visible = True})
        GVRole.OptionsBehavior.Editable = False

        Dim DT As New DataTable
        DT.Clear()
        DT = CHECKOPERATIONS_FalseOrTrue(18, GProfIDLog)
        If BtnView.Visibility = DT.Rows(0).Item("CanShow") = True Then BtnView.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

    End Sub
    Public Function SEARCH_SAFE(NAME As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@SafeName", SqlDbType.NVarChar, 250)
        PRM(0).Value = NAME
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SAFETB_SearchByTitle", PRM)
        Return DT
    End Function
    Private Sub SearchTxt_TextChanged(sender As Object, e As EventArgs) Handles SearchTxt.TextChanged
        If SearchTxt.Text <> String.Empty Then
            Dim DT As New DataTable
            DT.Clear()
            DT = SEARCH_SAFE(SearchTxt.Text.Trim)
            If DT.Rows.Count > 0 Then
                GCRole.DataSource = DT
                GVRole.Columns("ID").Caption = "الرقم"
                GVRole.Columns("Code").Caption = "الرمز"
                GVRole.Columns("SafeName").Caption = "اسم الخزنة"
                GVRole.Columns("StatusString").Caption = "طبيعة الحساب"
                GVRole.Columns("IsActive").Caption = "الحالة"
                For i As Integer = 0 To GVRole.Columns.Count - 1
                    GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                    GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
                    GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                Next
            End If
        Else
            LoadData()
        End If
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            Dim frm As FRMSAFE = New FRMSAFE
            Dim DT As New DataTable
            DT.Clear()
            DT = CHECKOPERATIONS_FalseOrTrue(18, GProfIDLog)
            If DT.Rows(0).Item("CanShow") = True Then
                frm.IsUpdate = True
                Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
                Dim CO As String = view.GetFocusedRowCellValue("Code").ToString
                frm.BtnSave.Enabled = False
                frm.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                frm.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                frm.SHOW_SAFE(CO)
                frm.ShowDialog()
                LoadData()
            Else
                ErrorMessage(Me, "رسالة تنبيه", "ليس لديك الصلاحية لفتح الشاشة")
            End If
        End If
    End Sub

    Private Sub BarButtonItem11_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnView.ItemClick
        Dim frm As FRMSAFE = New FRMSAFE
        LoadData()
        frm.Show()
    End Sub
End Class