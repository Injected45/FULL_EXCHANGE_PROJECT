Imports System.Threading
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMVIEWCUSTOMERS
    Public Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CustomersTb_LOADTODVG")
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            If GVRole.RowCount > 0 Then
                GVRole.Columns("AccID").Visible = False
            End If
        End If
    End Sub

    Private Sub FRMVIEWCUSTOMERS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        LoadData()
        GVRole.OptionsBehavior.Editable = False
    End Sub
    Private Sub GVRole_RowClick(sender As Object, e As RowClickEventArgs) Handles GVRole.RowClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            FRMCUSTOMER.NEWRECORD()
            FRMCUSTOMER.IsUpdate = True
            FRMCUSTOMER.BtnSave.Enabled = False
            FRMCUSTOMER.BtnEdit.Enabled = True
            'FRMCUSTOMER.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMCUSTOMER.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMCUSTOMER.SHOW_CUST(CO)
        End If
        Me.Close()
    End Sub
End Class