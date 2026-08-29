Imports System.Threading
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid
Imports System.ComponentModel
Imports System.Data.SqlClient


Public Class FRMVIEWEMPADVANCEPAYMENT

    Public Sub LoadData()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.NVarChar, -1) With {.Value = BID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AdvancePaymentTb_LOADTODVG", PR)
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

    Private Sub FRMVIEWEMPADVANCEPAYMENT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        LoadData()
        DVGFROMAT()
        GVRole.OptionsBehavior.Editable = False
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            FRMEMPADVANCEPAYMENT.DISAPLEDCONTROLS()
            FRMEMPADVANCEPAYMENT.IsUpdate = True
            FRMEMPADVANCEPAYMENT.BtnSave.Enabled = False
            FRMEMPADVANCEPAYMENT.BtnEdit.Enabled = True
            FRMEMPADVANCEPAYMENT.BtnPrint.Enabled = True
            FRMEMPADVANCEPAYMENT.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMEMPADVANCEPAYMENT.SHOW_RECORD(CO)
        End If
        Me.Close()
        FRMEMPADVANCEPAYMENT.IsUpdate = True
    End Sub
End Class