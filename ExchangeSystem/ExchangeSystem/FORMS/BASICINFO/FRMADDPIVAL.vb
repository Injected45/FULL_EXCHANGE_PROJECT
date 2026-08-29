Imports DevExpress
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMADDPIVAL
    Sub LOADDATA()
        Dim DT As New DataTable
        Dim editor As ButtonEdit = New ButtonEdit
        'If FRMEMPLOYEE.PIVID.Properties.Buttons(0).Kind.Plus Then
        DT.Clear()
            DT = RUN_QUARY_TXT("PayIncrease_LOADTOLKP ")
            If DT.Rows.Count > 0 Then
                GVRole.Columns.Clear()
                GCRole.DataSource = DT
            End If
        'End If
        DVGFormat()
        GVRole.Columns("ID").Visible = False

    End Sub
    Public Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = True
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub

    Private Sub FRMADDPIVAL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFormat()
        LOADDATA()
    End Sub
    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            Dim roleId As String = view.GetFocusedRowCellValue("الرمز")
            'FRMEMPLOYEE.PIVID.Properties.TextEditStyle = XtraEditors.Controls.TextEditStyles.Standard
            'FRMEMPLOYEE.PIVID.Text = view.GetFocusedRowCellValue("الاسم")
            FRMEMPLOYEE.StID = view.GetFocusedRowCellValue("ID")
            'FRMEMPLOYEE.PIVALUE.EditValue = Convert.ToDouble(view.GetFocusedRowCellValue("القيمة"))

            '==================================================

        End If
        'FRMEMPLOYEE.PIVID.Properties.TextEditStyle = XtraEditors.Controls.TextEditStyles.Standard
        'FRMEMPLOYEE.PIVID.EditValue = view.GetFocusedRowCellValue("الاسم")
        'FRMEMPLOYEE.PIVALUE.EditValue = Convert.ToDouble(view.GetFocusedRowCellValue("القيمة"))
        FRMEMPLOYEE.StID = view.GetFocusedRowCellValue("ID")
        Me.Close()
    End Sub
End Class