Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMViewADDCREDITACCOUNT
    Sub LoadTODVG()
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_TXT("CreditAccountsTb_LOADTODVG")
        If DTT.Rows.Count > 0 Then
            GCRole.DataSource = DTT
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

    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            FRMADDCREDITACCOUNT.IsUpdate = True
            FRMADDCREDITACCOUNT.BtnSave.Enabled = False
            FRMADDCREDITACCOUNT.BtnEdit.Enabled = True
            FRMADDCREDITACCOUNT.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FRMADDCREDITACCOUNT.Show_Crditaccount(CO)
        End If
        Me.Close()
    End Sub

    Private Sub FRMViewADDCREDITACCOUNT_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadTODVG()
        DVGFROMAT()
    End Sub

End Class