Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid
Public Class FRMVIEWCURRENCY
    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Me.Close()
    End Sub
    Public Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CURRENCYTB_LOADINTODATAGRIDVIEW")
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("ID").Caption = "الرقم"
            GVRole.Columns("Code").Caption = "الرمز"
            GVRole.Columns("CurrencyName").Caption = "اسم العملة"
            GVRole.Columns("IsActive").Caption = "الحالة"
            For i As Integer = 0 To GVRole.Columns.Count - 1
                GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
                GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            Next
        End If
    End Sub
    Public Sub CHECKBUTTONS()
        'CHECKOPERATIONS_FalseOrTrue(22, GProfIDLog)
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = CHECKOPERATIONS_FalseOrTrue(22, GProfIDLog)
        'If BtnView.Visibility = DT.Rows(0).Item("CanShow") = True Then BtnView.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Private Sub FRMVIEWCURRENCY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LoadData()
        GVRole.OptionsBehavior.Editable = False
        'CHECKBUTTONS()
    End Sub
    Public Function SEARCH_CURRENCY(NAME As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CurrencyName", SqlDbType.NVarChar, 250)
        PRM(0).Value = NAME.Trim
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CURRENCYTB_SearchByTitle", PRM)
        Return DT
    End Function
    Private Sub SearchTxt_TextChanged(sender As Object, e As EventArgs) Handles SearchTxt.TextChanged
        If SearchTxt.Text <> String.Empty Then
            Dim DT As New DataTable
            DT.Clear()
            DT = SEARCH_CURRENCY(SearchTxt.Text.Trim)
            If DT.Rows.Count > 0 Then
                GCRole.DataSource = DT
                GVRole.Columns("ID").Caption = "الرقم"
                GVRole.Columns("Code").Caption = "الرمز"
                GVRole.Columns("CurrencyName").Caption = "اسم العملة"
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
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
            Dim CO As String = view.GetFocusedRowCellValue("Code").ToString

            FRMCURRENCY.IsUpdate = True
            FRMCURRENCY.SHOW_CURRENCY(CO)
            'FRMCURRENCY.ShowDialog()
            FRMCURRENCY.BtnSave.Enabled = False
            FRMCURRENCY.BtnEdit.Enabled = True
            FRMCURRENCY.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMCURRENCY.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            'LoadData()
            Me.Close()
        End If
        'CHECKOPERATIONS_FalseOrTrue(22, GProfIDLog)
        '    Dim DT As New DataTable
        '    DT.Clear()
        '    DT = CHECKOPERATIONS_FalseOrTrue(22, GProfIDLog)
        '    If DT.Rows(0).Item("CanShow") = True Then
        '        frm.SHOW_CURRENCY(CO)
        '        LoadData()
        '        frm.Show()
        '    Else
        '        MetroFramework.MetroMessageBox.Show(Me, "ليس لديك الصلاحية لعرض العملات", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    End If
        'End If

    End Sub


End Class