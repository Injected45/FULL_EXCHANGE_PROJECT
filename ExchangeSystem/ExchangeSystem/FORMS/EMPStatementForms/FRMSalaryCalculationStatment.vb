Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Public Class FRMSalaryCalculationStatment
    Dim SelectType As Int16 = 0
    Sub LOADDATA()
            GCRole.DataSource = Nothing
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        Dim PR(3) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@Month", SqlDbType.TinyInt) With {.Value = D1.EditValue}
        PR(2) = New SqlParameter("@Yare", SqlDbType.Int) With {.Value = D2.EditValue}
        PR(3) = New SqlParameter("@IsAll", SqlDbType.TinyInt) With {.Value = If(IsAll.Checked, 1, 0)}
        Dim DT As New DataTable
            DT.Clear()
        DT = RUN_QUARY_PRO("SalaryCalculationTb_Statment", PR)
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
                GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center
                GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
            Next
            GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
            GVRole.OptionsView.EnableAppearanceEvenRow = True
            GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
            GVRole.OptionsView.EnableAppearanceOddRow = True
        End Sub

        Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
            If e.Column Is Nothing Then
                Return
            End If
            ' Fill column headers with the specified colors.
            e.Appearance.ForeColor = Color.White
            e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
            ' Draw the filter and sort buttons.
            For Each info As DrawElementInfo In e.Info.InnerElements
                If Not info.Visible Then
                    Continue For
                End If
                ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
            Next info
            e.Handled = True
        End Sub

        Private Sub FrmDiscountsLoadAllData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            DVGFROMAT()
        LOADBRNCHHasEmp(BranchID)
        GCRole.DataSource = Nothing
        BranchID.EditValue = -1
        BranchID.EditValue = BID
        GVRole.OptionsBehavior.Editable = False
        IsAll.Checked = False
        'FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
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
                'SafeID.Enabled = dt.Rows(0)("Can_safID")
                'SafeID.EditValue = UserAccID
                BranchID.EditValue = BID
            Else
                BranchID.Enabled = False
                'SafeID.Enabled = False
                'SafeID.EditValue = UserAccID
                BranchID.EditValue = BID
            End If
        End Sub
        Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.Columns("DBRID").Visible = False
        'BranchID.Properties.Columns("BranchType").Visible = False
    End Sub

        Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LOADDATA()

    End Sub


        Sub Print()

            Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
            Dim lookFeelError As New UserLookAndFeel(Me)
            lookFeelError.Style = LookAndFeelStyle.Skin
            lookFeelError.UseDefaultLookAndFeel = False
            lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            If GVRole.RowCount = 0 Then
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            Try
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يجب اختيار الفرع"
                Return
            End If
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@Month", SqlDbType.TinyInt) With {.Value = D1.EditValue}
            PR(2) = New SqlParameter("@Yare", SqlDbType.Int) With {.Value = D2.EditValue}
            PR(3) = New SqlParameter("@IsAll", SqlDbType.TinyInt) With {.Value = If(IsAll.Checked, 1, 0)}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("SalaryCalculationTb_Statment", PR)
            If DT.Rows.Count > 0 Then
                Dim report As New XtraReport4
                report.DataSource = DT
                report.DataMember = "SalaryCalculationTb"
                report.FilterString = GVRole.ActiveFilterString
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'Else
                '    XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
                MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub



    Private Sub SimpleButton1_Click_1(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Print()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GCRole.DataSource = Nothing
    End Sub

    Private Sub IsAll_CheckedChanged(sender As Object, e As EventArgs) Handles IsAll.CheckedChanged
        If IsAll.Checked = True Then
            SelectType = 1
        Else
            SelectType = 0
        End If
    End Sub
End Class