Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox

Public Class FrmConfirmAgentCanceled
    Dim clsaccsa As New CLSAccSafeActivity
    Private DT As New DataTable
    Public RBRID, RBRTYPE, DBRTYPE, ReasonID, CheckType As Integer
    Public DiscountVal As Decimal
    Public DiscountST = False, DiscountCancel As Boolean
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
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
    Private Sub FrmConfirmAgentsIssue_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        GCROLE.DataSource = Nothing
        BranchToLKP()
        CheckEdit1.Checked = False
        CheckEdit2.Checked = False
        D1.Enabled = False
        D2.Enabled = False
        CheckType = 0
        ISID.Enabled = False
        ISID.Text = ""
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        BranchID.EditValue = -1
        LOADBRNACH()
        Call BINGLKP(BtnBranchDeliveredID)
    End Sub

    Sub LOADDATA()
        GCROLE.DataSource = Nothing
        If CheckEdit1.Checked = True And CheckEdit2.Checked = False Then
            CheckType = 1
        ElseIf CheckEdit2.Checked = True And CheckEdit1.Checked = False Then
            CheckType = 2
        Else
            CheckType = 0
        End If
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PRM(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
        PRM(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
        PRM(3) = New SqlParameter("@CheckType", SqlDbType.TinyInt) With {.Value = CheckType}
        PRM(4) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = ISID.Text}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_GetRecordForAgentCancelDelivered", PRM)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            If GVROLE.RowCount > 0 Then
                GVROLE.Columns("BranchRecievedID").Visible = False
                GVROLE.Columns("BranchDeliveredIDID").Visible = False
                GVROLE.Columns("BranchType").Visible = False
                GVROLE.Columns("ConfirmCol").Width = 70
                GVROLE.Columns("OverallVal").AppearanceCell.BackColor = Color.Green
                GVROLE.Columns("ExVal").AppearanceCell.BackColor = Color.Green
            Else
                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                ' force Message Boxes to use the "MyCustomSkin"
                XtraMessageBox.AllowCustomLookAndFeel = True
                XtraMessageBox.Show(lookAndFeelError, "لا يوجد بيانات لعرضها خلال هذه الفترة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GCROLE.DataSource = Nothing
            End If
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
    Sub LOADDELIVERYBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        If DT.Rows.Count > 0 Then
            'BtnBranchDeliveredID.DataSource = DT
            'BtnBranchDeliveredID.ValueMember = "DBRID"
            'BtnBranchDeliveredID.DisplayMember = "BName"
            'BtnBranchDeliveredID.KeyMember = "BranchType"
            'GLKPVIEW.Columns("DBRID").Visible = False
            'GLKPVIEW.Columns("BranchType").Visible = False

        End If

    End Sub
    Sub BranchToLKP()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadAgent")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Public Sub DVGFormat()
        GVROLE.OptionsBehavior.Editable = True
        GVROLE.OptionsBehavior.ReadOnly = False
        GVROLE.Columns("Code").OptionsColumn.AllowEdit = False
        GVROLE.Columns("InsertDate").OptionsColumn.AllowEdit = False
        GVROLE.Columns("SenderName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("RecievedName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("BranchDeliveredID").OptionsColumn.AllowEdit = False
        GVROLE.Columns("BName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("OverallVal").OptionsColumn.AllowEdit = False
        GVROLE.Columns("ExVal").OptionsColumn.AllowEdit = False
        GVROLE.Columns("BranchRecievedID").OptionsColumn.AllowEdit = False
        'GVROLE.Columns("BranchDeliveredIDID").OptionsColumn.AllowEdit = False

        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsFind.AlwaysVisible = False
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
        GVROLE.Columns("ConfirmCol").Width = 70
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVROLE.CustomUnboundColumnData
        If e.Column.FieldName = "RowHandle" And e.IsGetData Then
            e.Value = GVROLE.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub CheckEdit1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit1.CheckedChanged
        CheckEdit2.Checked = False
        If CheckEdit1.Checked = True Then
            D1.Enabled = True
            D2.Enabled = True
            CheckType = 1
        Else
            D1.Enabled = False
            D2.Enabled = False
            CheckType = 0
        End If
    End Sub

    Private Sub CheckEdit2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit2.CheckedChanged
        CheckEdit1.Checked = False
        If CheckEdit2.Checked = True Then
            ISID.Enabled = True
            CheckType = 2
        Else
            ISID.Text = ""
            ISID.Enabled = False
            CheckType = 0
        End If
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'GCROLE.DataSource = Nothing
        If CheckEdit1.Checked = True Then
            If D1.EditValue > D2.EditValue Then
                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                ' force Message Boxes to use the "MyCustomSkin"
                XtraMessageBox.AllowCustomLookAndFeel = True
                XtraMessageBox.Show(lookAndFeelError, "تاريخ البداية يجب أن يكون أصغر من تاريخ النهاية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يجب اختيار الفرع"
            Exit Sub
        End If

        If CheckEdit2.Checked = True Then
            If ISID.Text = "" Then
                ISID.ErrorText = "يجب اختيار كود الحوالة"
                Exit Sub
            End If
        End If
        LOADBRNACH()
        Call BINGLKP(BtnBranchDeliveredID)
        'LOADDELIVERYBRANCH()
        LOADDATA()
        DVGFormat()
    End Sub
    Dim IsConfirm As Integer
    Private Sub BtnConfirm_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BtnConfirm.ButtonClick
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
        Dim inval As Object = GVROLE.GetFocusedRowCellValue("OverallVal")
        Dim xval As Object = GVROLE.GetFocusedRowCellValue("ExVal")

        Dim brid As Integer = GVROLE.GetFocusedRowCellValue("BranchRecievedID")
        Dim bdid As Integer = GVROLE.GetFocusedRowCellValue("BranchDeliveredIDID")
        Dim RName As Object = GVROLE.GetFocusedRowCellValue("RecievedName")
        Dim DName As Object = GVROLE.GetFocusedRowCellValue("SenderName")

        Dim info As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim info1 As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim brR As String = GVROLE.GetRowCellDisplayText(info1.RowHandle, GVROLE.Columns("BranchRecievedID"))
        Dim brD As String = GVROLE.GetRowCellDisplayText(info.RowHandle, GVROLE.Columns("BranchDeliveredIDID"))

        DT = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & brid & "'")
        RBRTYPE = DT.Rows(0)("BranchType")
        DT = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & bdid & "'")
        DBRTYPE = DT.Rows(0)("BranchType")


        Dim customIcon As New Icon(Application.StartupPath & "\question.ico", Icon.Width = 5, Icon.Height = 5)
        XtraMessageBox.Icons(MessageBoxIcon.Exclamation) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True

        Dim ConfirmMsg = XtraMessageBox.Show(lookAndFeelError, "هل تريد إلغاء هذه الحوالة بالفعل؟", "رسالة تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If ConfirmMsg = DialogResult.Yes Then

            Dim PRM(6) As SqlParameter
            PRM(0) = New SqlParameter("@InsertDate", Date.Now)
            PRM(1) = New SqlParameter("@ReasonID", ReasonID)
            PRM(2) = New SqlParameter("@SafeID", UserID)
            PRM(3) = New SqlParameter("@BranchID", brid)
            PRM(4) = New SqlParameter("@ISID", iscode)
            PRM(5) = New SqlParameter("@ISIDTYPE", 1)
            PRM(6) = New SqlParameter("@Notes", "")
            RUN_EXUTE_PRO("TransCancelRequestTb_Insert", PRM)

            RUN_EXUTE_TXT("update InternalEx set IsCanceled=1 where Code='" & iscode & "'")

            LOADDATA()
        Else
            Exit Sub
        End If
        sEnFRoRElode(iscode)
        GCROLE.DataSource = Nothing
        BranchID.EditValue = -1
        LOADDATA()
        refresh_table(BID)
        CONFIRMMESSAGE.Show()

    End Sub


    Public Sub sEnFRoRElode(ISID As String)
        ''ارسال رسالة في مجموعة الوكيل لتبليغ بالحوالة الوكيل

        RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, MAINBID)
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = ISID}
        dt = RUN_QUARY_PRO("GET_colmens_InternalEx", prm)

        If dt.Rows.Count > 0 Then
            Dim dd As String
            'If IStype = 0 Then


            dd = My.Settings.Combny_name & vbNewLine &
                    "تم إلغاء الحوالة" &
                             "CODE :" & Space(1) & dt.Rows(0)("Code") & vbNewLine &
                               "مـ :" & Space(1) & dt.Rows(0)("RecievedName") & vbNewLine &
                                "القيمه :" & Space(1) & dt.Rows(0)("OverallVal") & vbNewLine &
                                 "للإستفسار هـ : " & Space(1) & sql_Mobile1 & vbNewLine &
                                  "شكراً لتعاملكم معنا"
            WATSAPPMsAG(get_gruop_id(dt.Rows(0)("BranchDeliveredID")), dd, False)
            'End If



        End If






    End Sub

    Private Sub BtnRedirection_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BtnRedirection.ButtonClick
        Dim bdtype As Integer = GVROLE.GetFocusedRowCellValue("BranchType")
        If bdtype <> 3 Then
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim reuslt = XtraMessageBox.Show(lookAndFeelError, "الفرع المسلم يجب أن يكون وكيل", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        FRMAGENTREDIRECTION.LOADDATA()
        FRMAGENTREDIRECTION.LOADBRNACH()
        FRMAGENTREDIRECTION.LOADDELIVERYBRANCH()
        FRMAGENTREDIRECTION.ShowDialog()
    End Sub
    'Dim bdtype As Integer
    Sub BINGLKP(ByVal GLK As RepositoryItemGridLookUpEdit)
        Dim DT As New DataTable
        DT.Clear()
        GLK.DataSource = Nothing
        DT = RUN_QUARY_TXT("select ID as DBRID, BName as BranchDeliveredID, BranchType from CoBranch")
        If DT.Rows.Count > 0 Then
            GLK.DataSource = DT
            GLK.ValueMember = "DBRID"
            GLK.DisplayMember = "BranchDeliveredID"
            GLK.View.Columns.Clear()
            GLK.View.Columns.AddVisible("DBRID", "الرقم")
            GLK.View.Columns.AddVisible("BranchDeliveredID", "الاسم")
            GLK.View.Columns.AddVisible("BranchType", "النوع")
        Else
            GLK.DataSource = Nothing
        End If
    End Sub









    Dim sss As Object
    Private Sub BtnBranchDeliveredID_EditValueChanged(sender As Object, e As EventArgs) Handles BtnBranchDeliveredID.EditValueChanged
        'Dim view As GridView = CType(BtnBranchDeliveredID.View, GridView)
        'Dim val As Object = view.GetRowCellValue(view.FocusedRowHandle, "BranchDeliveredID")
        'GVROLE.SetRowCellValue(GVROLE.FocusedRowHandle, "BranchType", val)


        'Dim editor As GridLookUpEdit = TryCast(sender, GridLookUpEdit)
        'Dim index As Integer = editor.Properties.GetIndexByKeyValue(editor.EditValue)
        'If index < 0 Then
        '    Return
        'End If

        'sss = (TryCast(editor.Properties.View.GetRow(index), DataRowView)).Row("BranchType")

        'GVROLE.SetFocusedRowCellValue("NameN", sss)
    End Sub

    Private Sub GLKPVIEW_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GLKPVIEW.RowCellClick

    End Sub

    Private Sub BtnBranchDeliveredID_EditValueChanging(sender As Object, e As ChangingEventArgs) Handles BtnBranchDeliveredID.EditValueChanging

    End Sub

    Private Sub GCROLE_Click(sender As Object, e As EventArgs) Handles GCROLE.Click

    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        GCROLE.DataSource = Nothing
    End Sub

    Private Sub SimpleButton112_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub FrmConfirmAgentCanceled_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub

    Private Sub GVROLE_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GVROLE.RowCellClick

        'If e.Column.FieldName = "BranchType" Then
        '    bdtype = GVROLE.GetRowCellValue(e.RowHandle, e.Column)
        '    MsgBox(bdtype)
        'End If
        'bdtype = GLKPVIEW.GetRowHandle("BranchType").ToString
        ''bdtype = Convert.ToInt32(BtnBranchDeliveredID.GetDisplayText("BranchType").ToString)
        'MsgBox(bdtype)

    End Sub

    Private Sub GVROLE_RowClick(sender As Object, e As RowClickEventArgs) Handles GVROLE.RowClick
        'Dim editor As GridLookUpEdit = TryCast(sender, GridLookUpEdit)
        'Dim index As Integer = editor.Properties.GetIndexByKeyValue(editor.EditValue)
        'If index < 0 Then
        '    Return
        'End If

        'sss = (TryCast(editor.Properties.View.GetRow(index), DataRowView)).Row("BranchType")

        'GVROLE.SetFocusedRowCellValue("NameN", sss)




        If TypeOf sender Is GridLookUpEdit Then
                Dim lk As GridLookUpEdit = CType(sender, GridLookUpEdit)
                Dim row As DataRow = lk.Properties.View.GetDataRow(lk.Properties.View.FocusedRowHandle)
            GVROLE.SetRowCellValue(GVROLE.FocusedRowHandle, "NameN", row("BranchType"))
            sss = GVROLE.GetRowCellValue(GVROLE.FocusedRowHandle, "NameN", row("BranchType"))
        End If

    End Sub

    Private Sub GLKPVIEW_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles GLKPVIEW.CellValueChanged
        'Dim editor As GridLookUpEdit = CType(sender, GridLookUpEdit)

        'GVROLE.SetRowCellValue(GVROLE.FocusedRowHandle, "BName", editor.EditValue)
    End Sub

    Private Sub GLKPVIEW_CustomColumnDisplayText(sender As Object, e As CustomColumnDisplayTextEventArgs) Handles GLKPVIEW.CustomColumnDisplayText
        'bdtype = GLKPVIEW.GetRowHandle("BranchType").ToString
        'MsgBox(bdtype)
    End Sub

    Private Sub GLKPVIEW_MouseEnter(sender As Object, e As EventArgs) Handles GLKPVIEW.MouseEnter
        'Dim editor As GridLookUpEdit = CType(sender, GridLookUpEdit)
        'Dim row As DataRowView = CType(editor.GetSelectedDataRow, DataRowView)
        'bdtype = row("BranchType")
    End Sub

End Class