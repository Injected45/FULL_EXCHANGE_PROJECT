Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FrmViewCoBranch

    Private Sub LoadData()
        Dim dt As DataTable = RUN_QUARY_PRO_ONLY("CoBranch_LoadDataIntoDataGridview")
        GCRole.DataSource = dt
        'GVRole.Columns("ID").Caption = "الرقم"
        GVRole.Columns("Code").Caption = "الرمز"
        GVRole.Columns("BName").Caption = "اسم الفرع"
        GVRole.Columns("BType").Caption = "النوع"
        GVRole.Columns("Mobile1").Caption = "هاتف"
        GVRole.Columns("Mobile2").Caption = "جوال"
        GVRole.Columns("IsActive").Caption = "الحالة"
        Dim ts As RepositoryItemToggleSwitch = New RepositoryItemToggleSwitch
        GCRole.RepositoryItems.Add(ts)
        GVRole.Columns("IsActive").ColumnEdit = ts

        DVGFormat(GVRole)
    End Sub
    'Public Sub CHECKBUTTONS()
    '    CHECKOPERATIONS_FalseOrTrue(2, GProfIDLog)
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = CHECKOPERATIONS_FalseOrTrue(2, GProfIDLog)
    '    If BtnView.Visibility = DT.Rows(0).Item("CanShow") = True Then BtnView.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    'End Sub
    Private Sub ViewRolesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try


            LoadData()
            DVGFormat(GVRole)
            GVRole.OptionsBehavior.Editable = False
            'CHECKBUTTONS()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("Code"))
            Dim BC As String = view.GetFocusedRowCellValue("Code")
            Dim frm As FrmCoBranch = New FrmCoBranch
            FrmCoBranch.IsUpdate = True
            FrmCoBranch.CBID = roleId
            FrmCoBranch.Code.Text = BC
            FrmCoBranch.IsUpdate = True
            FrmCoBranch.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FrmCoBranch.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FrmCoBranch.SHOWRECORD(roleId, BC)
            Me.Close()
            LoadData()
        End If

    End Sub
End Class