Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewCanceledTransfer
    Sub New()

        InitializeComponent()

    End Sub
    Public Sub DVGFormat()
        'GVROLE.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.Editable = True
        GVRole.OptionsBehavior.ReadOnly = False
        GVRole.Columns("RowHandle").OptionsColumn.AllowEdit = False
        GVRole.Columns("Code").OptionsColumn.AllowEdit = False
        GVRole.Columns("InsertDate").OptionsColumn.AllowEdit = False
        GVRole.Columns("SenderName").OptionsColumn.AllowEdit = False
        GVRole.Columns("SPhone").OptionsColumn.AllowEdit = False
        GVRole.Columns("RecievedName").OptionsColumn.AllowEdit = False
        GVRole.Columns("RPhone").OptionsColumn.AllowEdit = False
        GVRole.Columns("OverallVal").OptionsColumn.AllowEdit = False
        GVRole.Columns("ExVal").OptionsColumn.AllowEdit = False
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.True
        GVRole.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = True
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
        GridLocalizer.Active = New MyGridLocalizer()
    End Sub
    Public Sub LOADDATA()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_ViewCancelRequest", PR)
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
            DVGFormat()
        End If
    End Sub

    Private Sub FrmViewCanceledTransfer_Load(sender As Object, e As EventArgs) Handles Me.Load
        DVGFormat()
        LOADDATA()
    End Sub
    Private Sub GVROLE_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 100, 102), e.Bounds)
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

    Private Sub GVROLE_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName Is "OverallVal" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("OverallVal"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(46, 139, 87)
                e.Appearance.BackColor2 = Color.FromArgb(46, 139, 87)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "ExVal" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ExVal"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(46, 139, 87)
                e.Appearance.BackColor2 = Color.FromArgb(46, 139, 87)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "ExtraComission" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ExtraComission"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(220, 20, 60)
                e.Appearance.BackColor2 = Color.FromArgb(220, 20, 60)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "DetailsCol" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("DetailsCol"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "RowHandle" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("RowHandle"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "code" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("code"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "InsertDate" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("InsertDate"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "SenderName" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("SenderName"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "SPhone" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("SPhone"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "RecievedName" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("RecievedName"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
        If e.Column.FieldName Is "RPhone" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("RPhone"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
    End Sub
    Private Sub BtnDetails_Click(sender As Object, e As EventArgs) Handles BtnDetails.Click
        Dim sendstatus As String = GVRole.GetFocusedRowCellValue("CancelStatus")
        If sendstatus = "طلب الإلغاء جاهز" Then
            FrmConfirmCancel.ConfirmedType = True
            FrmConfirmCancel.LoadData()
            FrmConfirmCancel.ShowDialog()
            Me.Close()
        Else
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookAndFeelError, "هذه الحوالة تم تقديم طلب إلغاء عليها الرجاء الانتظار حتى يتم اتخاذ الإجراء", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

    End Sub
End Class