Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Utils
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewBranchSafeDetails
    Sub BranchToLKP()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadconnectedBranch")
        DT.Rows.Add(0, "كل الفروع")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            BranchID.EditValue = BID
        End If
    End Sub
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 13, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub NewRecord()
        BranchToLKP()
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        DVGFROMAT()
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 168)
    End Sub
    Sub LOADMAINBRNCHACC()
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If
        If D1.EditValue > D2.EditValue Then
            ErrorMessage(Me, "رسالة خطأ", "تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية")
            Exit Sub
        End If
        DVGFROMAT()
        GCRole.DataSource = Nothing
        If BranchID.EditValue <> -1 Then
            Dim PR(2) As SqlParameter
            PR(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = D1.EditValue}
            PR(1) = New SqlParameter("@SecondDate", SqlDbType.Date) With {.Value = D2.EditValue}
            PR(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("FrmViewBranchSafeDetails", PR)
            If DTT.Rows.Count > 0 Then
                GCRole.DataSource = DTT
                DVGFROMAT()
            End If
        End If
    End Sub
    Private Sub FrmViewBranchSafeDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        NewRecord()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        LOADMAINBRNCHACC()
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        GCRole.DataSource = Nothing
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
End Class