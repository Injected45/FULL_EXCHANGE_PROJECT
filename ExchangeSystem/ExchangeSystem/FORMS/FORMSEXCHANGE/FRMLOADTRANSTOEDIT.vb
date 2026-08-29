Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FRMLOADTRANSTOEDIT
    Dim ETR As New EditTranReq
    Sub NewRecord()
        TransType.SelectedIndex = 0
        EditType.SelectedIndex = 2
        EditType_TextChanged(Nothing,Nothing)
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        SearchTxT.Text = String.Empty
        If TransType.SelectedIndex = -1 And EditType.SelectedIndex = -1 Then
            SearchTxT.Enabled = False
        Else
            SearchTxT.Enabled = True
        End If
    End Sub
    Sub DVGFROMAT(GVRole As GridView)
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
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
    Sub LOADTOLKPVIEW()
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
        PR(1) = New SqlParameter("@EditType", SqlDbType.TinyInt) With {.Value = EditType.SelectedIndex}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_LoadToEdit", PR)
        If DT.Rows.Count > 0 Then
            SearchTxT.Properties.DataSource = DT
            SearchTxT.Properties.ValueMember = "الرمز"
            SearchTxT.Properties.DisplayMember = "اسم الراسل"
        ElseIf DT.Rows.Count = 0 Then
            SearchTxT.Properties.DataSource = Nothing
        End If
        If GLKVIEW.RowCount > 0 And SearchTxT.Text <> String.Empty Then
            For i As Integer = 0 To GVRole.Columns.Count - 1
                GLKVIEW.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
                GLKVIEW.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
                GLKVIEW.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
                GLKVIEW.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
                GLKVIEW.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
            Next
            GLKVIEW.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
            GLKVIEW.OptionsView.EnableAppearanceEvenRow = True
            GLKVIEW.Appearance.OddRow.BackColor = Color.WhiteSmoke
            GLKVIEW.OptionsView.EnableAppearanceOddRow = True
        End If
    End Sub
    Sub LOADDATA()
        If SearchTxT.Text <> String.Empty Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
            PR(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = SearchTxT.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("InternalEx_SearchToEdit", PR)
            If DT.Rows.Count > 0 Then
                GCRole.DataSource = DT
            Else
                GCRole.DataSource = Nothing
            End If
        End If
    End Sub

    Private Sub EditType_TextChanged(sender As Object, e As EventArgs) Handles EditType.TextChanged
        GCRole.DataSource = Nothing
        GVRole.Columns.Clear()
        SearchTxT.Text = String.Empty
        If TransType.SelectedIndex = -1 And EditType.SelectedIndex = -1 Then
            SearchTxT.Enabled = False
        Else
            SearchTxT.Enabled = True
            LOADTOLKPVIEW()
        End If
    End Sub

    Private Sub RepSelectBtn_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles RepSelectBtn.ButtonClick
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim result = XtraMessageBox.Show(lookAndFeelError, "سيتم حفظ التعديلات ولا يمكن الرجوع عن العملية، هل تريد المتابعة؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If result = DialogResult.Yes Then
            Dim View = TryCast(GCRole.MainView, GridView)
            View.OptionsBehavior.Editable = True
            Dim Editor As ButtonEdit = CType(sender, ButtonEdit)
            Dim buttonIndex = Editor.Properties.Name
            If buttonIndex = "RepSelectBtn" Then
                Dim SName As String = View.GetFocusedRowCellValue("اسم الراسل").ToString
                Dim SPH As String = View.GetFocusedRowCellValue("هاتف الراسل").ToString
                Dim SMO As String = View.GetFocusedRowCellValue("جوال الراسل").ToString
                Dim RName As String = View.GetFocusedRowCellValue("اسم المستلم").ToString
                Dim RPH As String = View.GetFocusedRowCellValue("هاتف المستلم").ToString
                Dim RMO As String = View.GetFocusedRowCellValue("جوال المستلم").ToString
                Dim PR(1) As SqlParameter
                PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
                PR(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = SearchTxT.Text}
                Dim DT As New DataTable
                DT.Clear()
                DT = RUN_QUARY_PRO("InternalEx_SearchToEdit", PR)
                If DT.Rows.Count > 0 Then
                    Dim DSName As String = DT.Rows(0)("اسم الراسل").ToString
                    Dim DSPH As String = DT.Rows(0)("هاتف الراسل").ToString
                    Dim DSMO As String = DT.Rows(0)("جوال الراسل").ToString
                    Dim DRName As String = DT.Rows(0)("اسم المستلم").ToString
                    Dim DRPH As String = DT.Rows(0)("هاتف المستلم").ToString
                    Dim DRMO As String = DT.Rows(0)("جوال المستلم").ToString
                    If SName.Trim = DSName.Trim And SPH.Trim = DSPH.Trim And SMO.Trim = DSMO.Trim And RName.Trim = DRName.Trim And RPH.Trim = DRPH.Trim And RMO.Trim = DRMO.Trim Then
                        XtraMessageBox.Show(lookAndFeelError, "لم يتم إجراء أي تغيير على السجل يرجى التعديل على البيانات أولاً", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                End If
                Dim CO As String = View.GetFocusedRowCellValue("الرمز").ToString
                ETR.EditTranReq_Insert(CO, Date.Now, BID, SName, SPH, SMO, RName, RPH, RMO, TransType.SelectedIndex, EditType.SelectedIndex)
                CONFIRMMESSAGE.LBLTEXT.Text = "تمت عملية إرسال الطلب بنجاح"
                SearchTxT.EditValue = -1
                LOADTOLKPVIEW()
                LOADDATA()
                CONFIRMMESSAGE.ShowDialog()
                NewRecord()
            End If
        End If
    End Sub

    Private Function lookAndFeelError() As IWin32Window
        Throw New NotImplementedException()
    End Function

    Private Sub FRMLOADTRANSTOEDIT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SearchTxT.Properties.DataSource = Nothing
        NewRecord()
    End Sub

    Private Sub GLKVIEW_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GLKVIEW.CustomDrawColumnHeader
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

    Private Sub SearchTxT_TextChanged(sender As Object, e As EventArgs) Handles SearchTxT.TextChanged
        If SearchTxT.Text <> String.Empty Then
            LOADDATA()
            DVGFROMAT(GVRole)
            If GVRole.RowCount > 0 Then
                GVRole.Columns("اختيار").ColumnEdit = RepSelectBtn
                If EditType.SelectedIndex = 0 Then
                    GVRole.Columns("#").OptionsColumn.AllowEdit = False
                    GVRole.Columns("الرمز").OptionsColumn.AllowEdit = False
                    GVRole.Columns("اسم الراسل").OptionsColumn.AllowEdit = True
                    GVRole.Columns("هاتف الراسل").OptionsColumn.AllowEdit = True
                    GVRole.Columns("جوال الراسل").OptionsColumn.AllowEdit = True
                    GVRole.Columns("اسم المستلم").OptionsColumn.AllowEdit = False
                    GVRole.Columns("هاتف المستلم").OptionsColumn.AllowEdit = False
                    GVRole.Columns("جوال المستلم").OptionsColumn.AllowEdit = False
                    GVRole.Columns("اختيار").OptionsColumn.AllowEdit = True
                ElseIf EditType.SelectedIndex = 1 Then
                    GVRole.Columns("#").OptionsColumn.AllowEdit = False
                    GVRole.Columns("الرمز").OptionsColumn.AllowEdit = False
                    GVRole.Columns("اسم الراسل").OptionsColumn.AllowEdit = False
                    GVRole.Columns("هاتف الراسل").OptionsColumn.AllowEdit = False
                    GVRole.Columns("جوال الراسل").OptionsColumn.AllowEdit = False
                    GVRole.Columns("اسم المستلم").OptionsColumn.AllowEdit = True
                    GVRole.Columns("هاتف المستلم").OptionsColumn.AllowEdit = True
                    GVRole.Columns("جوال المستلم").OptionsColumn.AllowEdit = True
                    GVRole.Columns("اختيار").OptionsColumn.AllowEdit = True
                Else
                    GVRole.Columns("#").OptionsColumn.AllowEdit = False
                    GVRole.Columns("الرمز").OptionsColumn.AllowEdit = False
                    GVRole.Columns("اسم الراسل").OptionsColumn.AllowEdit = True
                    GVRole.Columns("هاتف الراسل").OptionsColumn.AllowEdit = True
                    GVRole.Columns("جوال الراسل").OptionsColumn.AllowEdit = True
                    GVRole.Columns("اسم المستلم").OptionsColumn.AllowEdit = True
                    GVRole.Columns("هاتف المستلم").OptionsColumn.AllowEdit = True
                    GVRole.Columns("جوال المستلم").OptionsColumn.AllowEdit = True
                    GVRole.Columns("اختيار").OptionsColumn.AllowEdit = True
                End If
            Else
                Exit Sub
            End If
        End If

    End Sub

    Private Sub FRMLOADTRANSTOEDIT_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub
End Class
Public Class EditTranReq
    Public Sub EditTranReq_Insert(ByVal Code As String, ByVal InsertDate As Date, ByVal BranchID As Integer, ByVal SenderName As String, ByVal SPhone1 As String,
                                          Phone2 As String, RecievedName As String, RPhone1 As String, RPhone2 As String, TransType As Integer, EditType As Integer)
        Dim PRM(10) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(3) = New SqlParameter("@SenderName", SqlDbType.NVarChar, -1) With {.Value = SenderName}
        PRM(4) = New SqlParameter("@SPhone1", SqlDbType.NVarChar, (50)) With {.Value = SPhone1}
        PRM(5) = New SqlParameter("@Phone2", SqlDbType.NVarChar, (50)) With {.Value = Phone2}
        PRM(6) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = RecievedName}
        PRM(7) = New SqlParameter("@RPhone1", SqlDbType.NVarChar, (50)) With {.Value = RPhone1}
        PRM(8) = New SqlParameter("@Rhpone2", SqlDbType.NVarChar, (50)) With {.Value = RPhone2}
        PRM(9) = New SqlParameter("@TransType", SqlDbType.TinyInt) With {.Value = TransType}
        PRM(10) = New SqlParameter("@EditType", SqlDbType.TinyInt) With {.Value = EditType}
        RUN_EXUTE_PRO("TransEditRequistTb_Insert", PRM)
    End Sub
End Class