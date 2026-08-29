Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewOrederLoading


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


    Private Sub FrmViewAddOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GCRole.DataSource = Nothing
        OrderType.EditValue = -1
        LoadToControlar(OrderType, "CONDB_OrderTB_LoadParentToLKP", "AccName", "AccCode", Nothing)
    End Sub

    Private Sub OrderType_TextChanged(sender As Object, e As EventArgs) Handles OrderType.TextChanged
        GCRole.DataSource = Nothing
        If OrderType.Text <> String.Empty Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = GetLKPColumnVal(OrderType, "AccCode")}
            LoadToControlar(GCRole, "CONDB_OrderTbDetilas_LoadToDVG", "", "", PRM)
        End If
    End Sub
End Class