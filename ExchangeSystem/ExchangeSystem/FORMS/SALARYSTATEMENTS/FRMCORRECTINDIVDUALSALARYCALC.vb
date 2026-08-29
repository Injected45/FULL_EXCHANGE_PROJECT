Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraReports.UI

Public Class FRMCORRECTINDIVDUALSALARYCALC




    Sub LOADDATA()
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO_ONLY("SalaryCalculationTb_ViewIndividualSalaryByEMPNotActive")
        If DTT.Rows.Count > 0 Then
            GCRole.DataSource = DTT
            DVGFROMAT()
            'GVRole.Columns("ID").Visible = False
        Else
            ErrorMessage(Me, "رسالة معلومات", "لا يوجد مرتبات تم احتسابها لهذا الموظف")
            Exit Sub
        End If
    End Sub




    Private Sub FRMINDIVDUALSALARYCALC_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADDATA()
    End Sub
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        'GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.EditingMode = False
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsFind.AllowFindPanel = True
        GVRole.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVRole.ShowFindPanel()
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

    Private Sub Print_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles Print.ButtonClick
        Try

            Dim PRR(0) As SqlParameter
            PRR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = GVRole.GetFocusedRowCellValue("CodeID")}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("ZRPT_SalaryCalculationTb_ViewIndividualSalaryByEMPNotActive", PRR)
            If DTT.Rows.Count > 0 Then
                Dim report As New RPTCORRECTINDIVDUALSALARYCALC
                report.DataSource = DTT
                report.DataMember = "SalaryCalculationTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                ErrorMessage(Me, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رساله تنبية ", "رسالة خطأ")
        End Try
    End Sub

    'Private Sub Print_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles Print.ButtonClick


    'End Sub
End Class