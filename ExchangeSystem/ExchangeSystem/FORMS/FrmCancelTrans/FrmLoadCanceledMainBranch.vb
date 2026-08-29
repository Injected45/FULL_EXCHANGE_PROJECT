Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraReports.UI

Public Class FrmLoadCanceledMainBranch
    Dim clsaccsa As New CLSAccSafeActivity
    Dim MODREST As Decimal
    Dim UpdateType, DBRID, RBRTYPE, DBRTYPE, ConfirmTRAS As Integer

    Private Sub FrmLoadCanceledMainBranch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        DVGFormat()
        'LOADDATA()
    End Sub
    Public Sub DVGFormat()
        'GVROLE.OptionsBehavior.EditingMode = True
        GVROLE.OptionsBehavior.Editable = True
        GVROLE.OptionsBehavior.ReadOnly = False
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
        GVROLE.Columns("DeliveredCurrencyID").Visible = False
        GVROLE.Columns("BranchRecievedID").Visible = False
        GVROLE.Columns("BranchDeliveredID").Visible = False
    End Sub
    Sub LOADBRANCH()

        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CoBranches_LoadDataToGetType", PR)
        If DT.Rows.Count > 0 Then
            RBRTYPE = DT.Rows(0)("BranchType")
        End If
    End Sub
    'Sub LOADDATA()
    '    Dim PR(0) As SqlParameter
    '    PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
    '    PR(0).Value = BID
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = RUN_QUARY_PRO("InternalEx_LoadRefusedRequestCancel", PR)
    '    If DT.Rows.Count > 0 Then
    '        GCROLE.DataSource = DT
    '        DVGFormat()
    '        GVROLE.Columns("DeliveredCurrencyID").Visible = False
    '        GVROLE.Columns("BranchRecievedID").Visible = False
    '        GVROLE.Columns("BranchDeliveredID").Visible = False
    '    Else
    '        GCROLE.DataSource = Nothing
    '    End If

    'End Sub

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
        If e.Column.FieldName Is "ExtraComission" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ExtraComission"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(220, 20, 60)
                e.Appearance.BackColor2 = Color.FromArgb(220, 20, 60)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "ConfirmCol" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ConfirmCol"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
    End Sub
    Dim inscls As New CLSINTERNALTRANSFER
    Dim empacc As New CLSACCEMPACTIVITY
    Dim bracc As New CLSBRANCHACTIVITY
    Private Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
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
            FRMINTERNALTRANSFER.OverallVal.EditValue = GVROLE.GetFocusedRowCellValue("OverallVal")
            Print(iscode)
            FRMINTERNALTRANSFER.OverallVal.EditValue = 0
            inscls.UPDATETB_INTERNALTRANSFER(iscode, UserID, Date.Now, "", UserID, Date.Now, "", iscode, inval, xval)
            RUN_EXUTE_TXT("delete from TransCancelRequestTb where ISID='" & iscode & "'")
            CONFIRMMESSAGE.LBLTEXT.Text = "تمت عملية التسليم بنجاح"
            'LOADDATA()
            FrmViewCanceledTransfer.LOADDATA()
            FRMCONFIRMISSUED.DiscountCancel = False
            'FRMCONFIRMISSUED.LOADFORCANCEL()
            FRMCONFIRMISSUED.LOADDATA()
        End If
        'LOADDATA()
        RefreshRecord()
    End Sub
    Private sqlDependency As SqlDependency
    Public Sub RefreshRecord()


        ' Start SqlDependency.
        'SqlDependency.Start(RefreshConnection)

        'Using sqlConnection As New SqlConnection(RefreshConnection)
        '    sqlConnection.Open()
        '    Using sqlCommand As SqlCommand = New SqlCommand("ExchangeSystem_FastAnalyz", sqlConnection)

        '        sqlCommand.CommandType = CommandType.StoredProcedure
        '        sqlCommand.Parameters.Add(New SqlParameter("@BranchID", BID))

        '        Dim DA As SqlDataAdapter = New SqlDataAdapter()
        '        DA.SelectCommand = sqlCommand
        '        Dim ds As DataSet = New DataSet()

        '        DA.Fill(ds)
        '        Dim dt As New DataTable
        '        dt = ds.Tables(0)

        '        GCROLE.DataSource = dt

        '        ' Register the event handler.
        '        Me.sqlDependency = New SqlDependency(sqlCommand)
        '        AddHandler Me.sqlDependency.OnChange, AddressOf sqlDependency_OnChange

        '        ' If you need a query, call sqlCommand.ExecuteReader()
        '        sqlCommand.ExecuteNonQuery()
        '    End Using
        'End Using


    End Sub
    Private Sub sqlDependency_OnChange(sender As Object, e As SqlNotificationEventArgs)
        System.Diagnostics.Debug.WriteLine("OnChange")

        ' If you do not need any more delete event
        RemoveHandler Me.sqlDependency.OnChange, AddressOf sqlDependency_OnChange
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