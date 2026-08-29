Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraReports.UI

Public Class FrmInternalExDeliveredAfterConfirmCancel
    Dim inscls As New CLSINTERNALTRANSFER
    Dim empacc As New CLSACCEMPACTIVITY
    Dim bracc As New CLSBRANCHACTIVITY
    Dim clsaccsa As New CLSAccSafeActivity
    Private DT As New DataTable
    Private RBRID, DBRID, RBRTYPE, DBRTYPE As Integer
    Sub LOADDATA()

        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_LoadToDeliverCanceledInternal", PR)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            DVGFormat()
        Else
            GCROLE.DataSource = Nothing
            DVGFormat()
            'RBRID = DT.Rows(0)("BranchRecievedID")
        End If
    End Sub

    Sub LOADBRNACH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BtnBranchRecieved.DataSource = DT
        BtnBranchRecieved.ValueMember = "DBRID"
        BtnBranchRecieved.DisplayMember = "BName"
        BtnBranchRecieved.PopulateColumns()
        BtnBranchRecieved.Columns("DBRID").Visible = False
        BtnBranchRecieved.Columns("BranchType").Visible = False
        BtnBranchRecieved.ShowHeader = False


    End Sub

    Private Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
        UPDATEEXISTRECORD()
    End Sub
    Sub UPDATEEXISTRECORD()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
        Dim inval As Object = GVROLE.GetFocusedRowCellValue("OverallVal")
        Dim xval As Object = GVROLE.GetFocusedRowCellValue("ExVal")
        Dim brid As Integer = GVROLE.GetFocusedRowCellValue("BranchRecievedID")
        Dim bdid As Integer = GVROLE.GetFocusedRowCellValue("BranchDeliveredID")
        Dim RName As Object = GVROLE.GetFocusedRowCellValue("RecievedName")
        Dim RPH1 As Object = GVROLE.GetFocusedRowCellValue("RPhone1")
        Dim RPH2 As Object = GVROLE.GetFocusedRowCellValue("RPhone2")
        Dim DCID As Object = GVROLE.GetFocusedRowCellValue("DeliveredCurrencyID")
        Dim info As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim info1 As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim brD As String = GVROLE.GetRowCellDisplayText(info.RowHandle, GVROLE.Columns("BranchDeliveredID"))
        Dim brR As String = GVROLE.GetRowCellDisplayText(info1.RowHandle, GVROLE.Columns("BranchRecievedID"))
        GETSAFEVAL(UserAccID, BID, 1)
        If inval + xval > SAFEVAL Then
            XtraMessageBox.Show(lookFeelError, "رصيد الخزنة لا يسمح بتسليم الحوالة", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            inscls.UPDATETB_INTERNALTRANSFER(iscode, UserID, Date.Now, "", UserID, Date.Now, "", iscode, inval, xval)
            RUN_EXUTE_TXT("delete from TransCancelRequestTb where ISID='" & iscode & "'")
            FRMINTERNALTRANSFER.OverallVal.EditValue = GVROLE.GetFocusedRowCellValue("OverallVal")
            Print(iscode)
            FRMINTERNALTRANSFER.OverallVal.EditValue = 0.000
            CONFIRMMESSAGE.LBLTEXT.Text = "تمت عملية التسليم بنجاح"
            LOADDATA()
            FrmViewCanceledTransfer.LOADDATA()
            FRMCONFIRMISSUED.DiscountCancel = False
            'FRMCONFIRMISSUED.LOADFORCANCEL()
            FRMCONFIRMISSUED.LOADDATA()
        End If
    End Sub
    Sub LOADDELIVERYBRANCH()

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BtnBranchDeliveredID.DataSource = DT
        BtnBranchDeliveredID.ValueMember = "DBRID"
        BtnBranchDeliveredID.DisplayMember = "BName"
        BtnBranchDeliveredID.ShowHeader = False
        BtnBranchDeliveredID.PopulateColumns()
        BtnBranchDeliveredID.Columns("DBRID").Visible = False
        BtnBranchDeliveredID.Columns("BranchType").Visible = False
    End Sub
    Public Sub DVGFormat()
        'GVROLE.OptionsBehavior.EditingMode = True
        GVROLE.OptionsBehavior.Editable = True
        GVROLE.OptionsBehavior.ReadOnly = False
        GVROLE.Columns("Code").OptionsColumn.AllowEdit = False
        GVROLE.Columns("InsertDate").OptionsColumn.AllowEdit = False
        GVROLE.Columns("SenderName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("SPhone").OptionsColumn.AllowEdit = False
        GVROLE.Columns("RecievedName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("RPhone").OptionsColumn.AllowEdit = False
        GVROLE.Columns("OverallVal").OptionsColumn.AllowEdit = False
        GVROLE.Columns("ExVal").OptionsColumn.AllowEdit = False
        GVROLE.Columns("BranchRecievedID").OptionsColumn.AllowEdit = False
        GVROLE.Columns("BranchDeliveredID").OptionsColumn.AllowEdit = False
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
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVROLE.OptionsView.EnableAppearanceOddRow = True
        GridLocalizer.Active = New MyGridLocalizer()
    End Sub
    Private Sub GVROLE_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVROLE.RowCellStyle
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
        If e.Column.FieldName Is "BranchRecievedID" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("BranchRecievedID"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(37, 150, 190)
                e.Appearance.BackColor2 = Color.FromArgb(37, 150, 190)
            End If
        End If
        If e.Column.FieldName Is "BranchDeliveredID" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("BranchDeliveredID"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(0, 131, 204)
                e.Appearance.BackColor2 = Color.FromArgb(0, 131, 204)
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
    Private Sub GVROLE_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
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
    Private Sub FrmInternalExDeliveredAfterConfirmCancel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADBRNACH()
        LOADDELIVERYBRANCH()
        LOADDATA()
        DVGFormat()
    End Sub

    Private Sub FrmInternalExDeliveredAfterConfirmCancel_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        FrmViewCanceledTransfer.LOADDATA()
    End Sub

    Sub Print(Code As Object)
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", Code)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_InternalExValues_PrintRecords", PRM)
            dt.TableName = "InternalEx"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTRecieveInternalEx2
                report.DataSource = ds
                report.DataMember = "InternalEx"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub
End Class