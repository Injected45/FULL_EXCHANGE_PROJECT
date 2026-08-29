Imports System.Data.Common
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMCONFIRMEDITTRANS
    Dim Ds As DataSet
    Sub DVGFROMAT()
        GVRole1.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole1.OptionsBehavior.Editable = False
        GVRole1.OptionsBehavior.EditingMode = False
        GVRole1.OptionsBehavior.ReadOnly = True
        GVRole1.OptionsView.ShowGroupPanel = False
        GVRole1.OptionsView.ShowFooter = False
        GVRole1.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole1.OptionsSelection.MultiSelectMode = False
        GVRole1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole1.Columns.Count - 1
            GVRole1.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole1.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole1.OptionsView.EnableAppearanceEvenRow = True
        GVRole1.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole1.OptionsView.EnableAppearanceOddRow = True
        GVRole1.Columns("تأكيد").OptionsColumn.AllowEdit = True
    End Sub
    Private Sub FRMCONFIRMEDITTRANS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try


            GCRole1.DataSource = Nothing
            GVRole1.Columns.Clear()
            OPENCONNECTION()

            Dim dtInternalEx As DataTable = RUN_QUARY_TXT("select a.ID, a.Code, a.InsertDate, b.BName, a.SenderName, a.SPhone1, a.SPhone2, a.RecievedName, a.RPhone1, a.RPhone2, '' as 'تأكيد' from InternalEx as a inner JOIN CoBranch as b on a.BranchRecievedID=b.ID WHERE (a.ConfirmType = 0 or a.ConfirmType = 1) AND a.IsActive = 1 AND IsEdit=1")
            dtInternalEx.TableName = "InternalEx"
            Dim dtTransEdit As DataTable = RUN_QUARY_TXT("select a.TransID, a.ISID as 'الرمز', a.InsertDate as 'التاريخ', b.BName as 'الفرع المعدل', a.SenderName 'الراسل', a.SPhone1 'هاتف الراسل', a.Phone2 'جوال الراسل', a.RecievedName 'المستلم', a.RPhone1 'هاتف المستلم', a.Rhpone2 as 'جوال المستلم' from TransEditRequistTb as a INNER JOIN CoBranch as b on a.BranchID=b.ID INNER JOIN InternalEx AS c ON a.TransID=c.id where a.IsUpdated = 1 AND  (c.ConfirmType = 0 or c.ConfirmType = 1) AND c.IsEdit=1 ")
            dtTransEdit.TableName = "TransEditRequistTb"
            Ds = New DataSet
            Ds.Tables.Add(dtInternalEx)
            Ds.Tables.Add(dtTransEdit)
            Ds.Tables("InternalEx").Constraints.Add("ID", Ds.Tables("InternalEx").Columns("ID"), True)
            Ds.Relations.Add("تفاصيل التعديل", Ds.Tables("InternalEx").Columns("ID"), Ds.Tables("TransEditRequistTb").Columns("TransID"))
            If dtInternalEx.Rows.Count > 0 Then
                GCRole1.DataSource = Ds.Tables("InternalEx")
                GVRole1.Columns("Code").Caption = "الرمز"
                GVRole1.Columns("InsertDate").Caption = "التاريخ"
                GVRole1.Columns("BName").Caption = "الفرع"
                GVRole1.Columns("SenderName").Caption = "الراسل"
                GVRole1.Columns("SPhone1").Caption = "هاتف الراسل"
                GVRole1.Columns("SPhone2").Caption = "جوال الراسل"
                GVRole1.Columns("RecievedName").Caption = "المستلم"
                GVRole1.Columns("RPhone1").Caption = "هاتف المستلم"
                GVRole1.Columns("RPhone2").Caption = "جوال المستلم"
                GVRole1.Columns("ID").Visible = False
                GVDE.ViewCaption = "تفاصيل التعديل"
                GVRole1.Columns("تأكيد").ColumnEdit = BtnConfirm
                DVGFROMAT()

                GVRole1.Columns("تأكيد").OptionsColumn.AllowEdit = True

                'DVGFROMAT(GVDE)
                GVRole1.Columns("تأكيد").OptionsColumn.AllowEdit = True
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Public Sub LoadChildData(rowIndex As Integer)
        Dim rowParent = Ds.Tables("InternalEx").Rows(rowIndex)
        Dim rowChild = rowParent.GetChildRows("InternalEx_TransEditRequistTb")
        Dim childTB As DataTable = Ds.Tables("TransEditRequistTb").Clone
        For Each row In rowChild
            childTB.ImportRow(row)
            GCRole1.DataSource = childTB
        Next
    End Sub


    Private Sub GVRole1_MasterRowExpanded(sender As Object, e As CustomMasterRowEventArgs) Handles GVRole1.MasterRowExpanded
        Dim masterView As GridView = TryCast(sender, GridView)
        Dim detailView As GridView = CType(masterView.GetDetailView(e.RowHandle, e.RelationIndex), GridView)
        detailView.Columns(0).Visible = False
        detailView.Appearance.HeaderPanel.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(1).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(2).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(3).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(4).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(5).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(6).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(7).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(8).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)
        detailView.Columns(9).AppearanceHeader.BackColor = Color.FromArgb(255, 102, 102)


        detailView.Columns(1).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(2).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(3).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(4).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(5).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(6).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(7).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(8).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(9).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center

        detailView.Columns(1).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(2).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(3).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(4).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(5).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(6).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(7).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(8).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(9).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center

        detailView.Columns(1).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(2).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(3).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(4).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(5).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(6).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(7).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(8).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
        detailView.Columns(9).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center

        detailView.Columns(1).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(2).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(3).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(4).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(5).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(6).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(7).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(8).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
        detailView.Columns(9).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center

        'DVGFROMAT(detailView)
    End Sub
    Private Sub GVRole1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole1.CustomDrawColumnHeader
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

    Private Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
        GVRole1.Columns("تأكيد").OptionsColumn.AllowEdit = True
        GVRole1.Columns("تأكيد").OptionsColumn.ReadOnly = False
        GVRole1.Columns("تأكيد").OptionsColumn.AllowFocus = True
        Dim IsConfirm As Boolean
        Dim iscode As Object = GVRole1.GetFocusedRowCellValue("Code")
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim resu = XtraMessageBox.Show(lookAndFeelError, "سيتم اعتماد التعديلات ولا يمكن التراجع، هل تريد الاستمرار؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If resu = DialogResult.Yes Then
            IsConfirm = True
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = iscode}
            PR(1) = New SqlParameter("@IsConfirm", SqlDbType.Bit) With {.Value = IsConfirm}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("InternalEx_UpdateAfterConfirmEdit", PR)
            CONFIRMMESSAGE.LBLTEXT.Text = "تم اعتماد عملية التعديل بنجاح"
            CONFIRMMESSAGE.ShowDialog()
            DT.Dispose()
            FRMCONFIRMEDITTRANS_Load(Nothing, Nothing)
        Else
            IsConfirm = False
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = iscode}
            PR(1) = New SqlParameter("@IsConfirm", SqlDbType.Bit) With {.Value = IsConfirm}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("InternalEx_UpdateAfterConfirmEdit", PR)
            CONFIRMMESSAGE.LBLTEXT.Text = "تم إلغاء عملية التعديل بنجاح"
            CONFIRMMESSAGE.ShowDialog()
            DT.Dispose()
            FRMCONFIRMEDITTRANS_Load(Nothing, Nothing)
            Exit Sub
        End If

    End Sub

    Private Sub GCRole1_Click(sender As Object, e As EventArgs) Handles GCRole1.Click
        Dim gridView = TryCast(GCRole1.MainView, GridView)
        gridView.OptionsBehavior.Editable = True

        For Each column As GridColumn In gridView.Columns

            If column.FieldName = "تأكيد" Then
                gridView.FocusedColumn = column
                column.OptionsColumn.AllowEdit = True
            Else
                column.OptionsColumn.AllowEdit = False
            End If
        Next
    End Sub

    Private Sub FRMCONFIRMEDITTRANS_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub
End Class