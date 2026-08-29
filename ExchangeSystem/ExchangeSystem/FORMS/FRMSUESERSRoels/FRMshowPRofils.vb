
Imports DevExpress.Utils
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient
Imports System.Threading
Public Class FRMshowPRofils
    Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.ShowFindPanel()
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 11, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

    End Sub

    Sub LOADUSERSTOLB()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("Select ProfID, ProfileName from ProfileName")
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            GVRole.Columns("ProfileName").Caption = "اسم المجوعة"
            GVRole.Columns("ProfID").Caption = "رقم المجوعة"
            ' GVRole.Columns("ProfID").Visible = False
            '  GVRole.OptionsView.ShowColumnHeaders = False
            GVRole.OptionsFilter.InHeaderSearchMode = GridInHeaderSearchMode.TextSearch
            GVRole.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways
            GVRole.Columns("ProfileName").OptionsFilter.AllowInHeaderSearch = DevExpress.Utils.DefaultBoolean.True
            GVRole.Columns("ProfileName").OptionsFilter.InHeaderSearchPrompt = "انقر هنا للبحث ..."
            GVRole.OptionsView.ShowFilterPanelMode = False
            GVRole.OptionsFilter.ShowInHeaderSearchTextMode = ShowInHeaderSearchTextMode.Text
            FormLocation(Me)
            DVGFormat()

        End If
    End Sub

    Private Sub FRMshowPRofils_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADUSERSTOLB()
    End Sub

    Private Sub GCRole_DoubleClick(sender As Object, e As EventArgs) Handles GCRole.DoubleClick
        Try


            If GVRole.RowCount > 0 Then

                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 1, FrmUserAccessTemplate.GCRole, FrmUserAccessTemplate.Man)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 2, FrmUserAccessTemplate.GridControl2, FrmUserAccessTemplate.SRF)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 3, FrmUserAccessTemplate.GridControl3, FrmUserAccessTemplate.Crun)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 4, FrmUserAccessTemplate.GridControl4, FrmUserAccessTemplate.Crun)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 5, FrmUserAccessTemplate.GridControl5, FrmUserAccessTemplate.SNM)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 6, FrmUserAccessTemplate.GridControl6, FrmUserAccessTemplate.Rs)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 7, FrmUserAccessTemplate.GridControl7, FrmUserAccessTemplate.EM)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 8, FrmUserAccessTemplate.GridControl8, FrmUserAccessTemplate.BAnk)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 9, FrmUserAccessTemplate.GridControl9, FrmUserAccessTemplate.GridView9)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 10, FrmUserAccessTemplate.GridControl10, FrmUserAccessTemplate.ST)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 11, FrmUserAccessTemplate.GridControl11, FrmUserAccessTemplate.MO)
                FrmUserAccessTemplate.UserAccessProfileTemplate_sharch(GVRole.GetFocusedRowCellValue("ProfID"), 12, FrmUserAccessTemplate.GridControl12, FrmUserAccessTemplate.MO)
                FrmUserAccessTemplate.UserROOLMAINPROFILETP_sharch(GVRole.GetFocusedRowCellValue("ProfID"))
                FrmUserAccessTemplate.Ueser_Group_main_shrch_lode(GVRole.GetFocusedRowCellValue("ProfID"))
                FrmUserAccessTemplate.ProfID.Text = GVRole.GetFocusedRowCellValue("ProfID")
                FrmUserAccessTemplate.TextEdit1.Text = GVRole.GetFocusedRowCellValue("ProfileName")

                FrmUserAccessTemplate.BarButtonItem3.Enabled = True
                FrmUserAccessTemplate.BarButtonItem5.Enabled = True
                FrmUserAccessTemplate.BarButtonItem2.Enabled = False
                FrmUserAccessTemplate.BarButtonItem1.Enabled = True
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


End Class