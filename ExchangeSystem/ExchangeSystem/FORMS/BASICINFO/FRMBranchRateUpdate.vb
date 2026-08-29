Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMBranchRateUpdate
    Sub NEWRECORD()
        BranchID.EditValue = -1
        LOADBRANCH()
        GCROLE.DataSource = Nothing
        DVGFormat()
    End Sub
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("BranchRatesTb_LoadFBranchToLKPIsRated")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "ID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADDATA()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BranchRatesTb_LoadToUpdate", PR)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            DVGFormat()
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("ID").Visible = False
    End Sub
    Public Sub DVGFormat()
        'GVROLE.OptionsBehavior.EditingMode = True
        GVROLE.OptionsBehavior.Editable = True
        GVROLE.OptionsBehavior.ReadOnly = False
        GVROLE.Columns("FBranchName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("SBranchName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("FBRate").OptionsColumn.AllowEdit = True
        GVROLE.Columns("SBRate").OptionsColumn.AllowEdit = True
        GVROLE.Columns("TBID").OptionsColumn.ReadOnly = False
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsFind.AlwaysVisible = True
        GVROLE.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        Next
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVROLE.OptionsView.EnableAppearanceOddRow = True
        'GridLocalizer.Active = New MyGridLocalizer()
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
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
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        If BranchID.EditValue = -1 Or BranchID.Text = "" Then
            BranchID.ErrorText = "يجب اختيار الفرع أولاً"
            Exit Sub
        End If
        LOADDATA()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim IsUpdate As Boolean = True
        Dim customIcon As New Icon(Application.StartupPath & "\warning.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Warning) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        'For i = 0 To GVROLE.RowCount - 1

        For i = 0 To GVROLE.RowCount - 1

            If GVROLE.GetRowCellValue(i, "FBRate") + GVROLE.GetRowCellValue(i, "SBRate") > 100.0 Then
                GVROLE.Appearance.EvenRow.BackColor = Color.Red
                XtraMessageBox.Show(lookAndFeelError, "النسبة لا يجب أن تكون أكبر من 100", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Next
        'Next
        For i = 0 To GVROLE.RowCount - 1
            If GVROLE.GetRowCellValue(i, "FBRate") + GVROLE.GetRowCellValue(i, "SBRate") > 100.0 Then
                GVROLE.Appearance.EvenRow.BackColor = Color.Red
                XtraMessageBox.Show(lookAndFeelError, "النسبة لا يجب أن تكون أكبر من 100", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Next
        For i = 0 To GVROLE.RowCount - 1
            If GVROLE.GetRowCellValue(i, "FBranchID") = MAINBID Or GVROLE.GetRowCellValue(i, "SBranchID") = MAINBID Then
                If GVROLE.GetRowCellValue(i, "FBRate") + GVROLE.GetRowCellValue(i, "SBRate") < 100.0 Then
                    GVROLE.Appearance.EvenRow.BackColor = Color.Red
                    XtraMessageBox.Show(lookAndFeelError, "الرجاء التأكد أن مجموع النسب =100", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        Next
        Dim resu = XtraMessageBox.Show(lookAndFeelError, "سيتم تعديل البيانات ولا يمكن التراجع عن ذلك، هل تريد الاستمرار؟", "رسالة تبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If resu = DialogResult.Yes Then
            For i = 0 To GVROLE.RowCount - 1
                If GVROLE.GetRowCellValue(i, "FBRate") + GVROLE.GetRowCellValue(i, "SBRate") > 100.0 Then
                    XtraMessageBox.Show(lookAndFeelError, "النسبة لا يجب أن تكون أكبر من 100", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Dim PRM(5) As SqlParameter
                    PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = GVROLE.GetRowCellValue(i, "TBID")}
                    PRM(1) = New SqlParameter("@FBranchID", SqlDbType.Int) With {.Value = 0}
                    PRM(2) = New SqlParameter("@FBRate ", SqlDbType.Decimal) With {.Value = GVROLE.GetRowCellValue(i, "FBRate")}
                    PRM(3) = New SqlParameter("@SBranchID", SqlDbType.Int) With {.Value = 0}
                    PRM(4) = New SqlParameter("@SBRate ", SqlDbType.Decimal) With {.Value = GVROLE.GetRowCellValue(i, "SBRate")}
                    PRM(5) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
                    RUN_EXUTE_PRO("BranchRatesTb_Insert", PRM)
                End If
            Next
        Else
            Exit Sub
            NEWRECORD()
        End If
        NEWRECORD()
    End Sub

    Private Sub FRMBranchRateUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub GVROLE_RowStyle(sender As Object, e As RowStyleEventArgs) Handles GVROLE.RowStyle
        'For i = 0 To GVROLE.RowCount - 1
        '    Dim Frate As Decimal = Convert.ToDecimal(GVROLE.GetRowCellValue(e.RowHandle, "FBRate"))
        '    Dim Srate As Decimal = Convert.ToDecimal(GVROLE.GetRowCellValue(e.RowHandle, "SBRate"))
        '    If Frate + Srate > 100.0 Then
        '        e.Appearance.BackColor = Color.Red
        '        'XtraMessageBox.Show(lookAndFeelError, "النسبة لا يجب أن تكون أكبر من 100", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Exit Sub
        '    End If
        'Next
    End Sub
End Class