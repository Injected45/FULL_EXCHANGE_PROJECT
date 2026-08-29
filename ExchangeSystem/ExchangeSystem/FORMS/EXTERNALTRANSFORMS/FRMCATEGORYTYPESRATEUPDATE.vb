Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Public Class FRMCATEGORYTYPESRATEUPDATE
    Public RatType As Integer
    Public MaxValue, MaxSerVal As Decimal
    Sub NEWRECORD()
        ServiceID.EditValue = -1
        LOADServiceID()
        GCROLE.DataSource = Nothing
        DVGFormat()
    End Sub
    Sub LOADServiceID()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ExtTraServiceTypeTb_LoadToLSBOX")
        If DT.Rows.Count > 0 Then
            ServiceID.Properties.DataSource = DT
            ServiceID.Properties.ValueMember = "ID"
            ServiceID.Properties.DisplayMember = "ServiceName"
            ServiceID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADDATA()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@ServiceID", SqlDbType.Int) With {.Value = ServiceID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExtTraServiceTypeTb_LoadRateToDVG", PR)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            DVGFormat()
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ServiceID.QueryPopUp
        ServiceID.Properties.PopulateColumns()
        ServiceID.Properties.Columns("ID").Visible = False
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

    Private Sub FRMCATEGORYTYPESRATEUPDATE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        GCROLE.DataSource = Nothing
        If ServiceID.EditValue = -1 Or ServiceID.Text = "" Then
            ServiceID.ErrorText = "يجب اختيار الخدمة أولاً"
            Exit Sub
        End If
        LOADDATA()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If ServiceID.EditValue = -1 Or ServiceID.Text = "" Then
            ErrorMessage(Me, "رسالة خطأ", "يجب اختيار الخدمة أولاً")
            Exit Sub
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@TransID", SqlDbType.Int) With {.Value = ServiceID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CATEGORYTYPESTB_LoadToUpdate", PR)
        If DT.Rows.Count > 0 Then
            FRMCATEGORYTYPES.Code.Text = DT.Rows(0)("ID")
            FRMCATEGORYTYPES.TypeNoTxT.EditValue = DT.Rows(0)("TypeNo")
            FRMCATEGORYTYPES.CountryID.EditValue = DT.Rows(0)("CountryID")
            FRMCATEGORYTYPES.TransTypeTxT.EditValue = DT.Rows(0)("TransType")
            FRMCATEGORYTYPES.IsUpdate = 2
            FRMCATEGORYTYPES.Code.Enabled = False
            FRMCATEGORYTYPES.CountryID.Enabled = False
            FRMCATEGORYTYPES.TransTypeTxT.Enabled = False
            Me.Close()
        Else
            ErrorMessage(Me, "رسالة خطأ", "لا يوجد شرائح لهذه الخدمة الرجاء إغلاق هذه الشاشة و تعبئة شرائح جديدة")
            FRMCATEGORYTYPES.NEWRECORD()
        End If

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'Dim IsUpdate As Boolean = True
        'Dim customIcon As New Icon(Application.StartupPath & "\warning.ico")
        'XtraMessageBox.Icons(MessageBoxIcon.Warning) = customIcon
        'Dim lookAndFeelError As New UserLookAndFeel(Me)
        ''lookAndFeelError.SkinName = "MilkShake"
        'lookAndFeelError.Style = LookAndFeelStyle.Skin
        'lookAndFeelError.UseDefaultLookAndFeel = False
        'lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        '' force Message Boxes to use the "MyCustomSkin"
        'XtraMessageBox.AllowCustomLookAndFeel = True

        If ServiceID.EditValue = -1 Or ServiceID.Text = "" Then
            ErrorMessage(Me, "رسالة خطأ", "يجب اختيار الخدمة أولاً")
            Exit Sub
        End If
        If GVROLE.RowCount <= 0 Then
            ErrorMessage(Me, "رسالة خطأ", "يجب عرض الشرائح أولا")
            Exit Sub
        End If
        Dim resu = XtraMessageBox.Show("سيتم تعديل البيانات ولا يمكن التراجع عن ذلك، هل تريد الاستمرار؟", "رسالة تبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If resu = DialogResult.Yes Then
            For i = 0 To GVROLE.RowCount - 1
                If GVROLE.GetRowCellValue(i, "ValTo") < GVROLE.GetRowCellValue(i, "ValFrom") Then
                    GVROLE.Appearance.EvenRow.BackColor = Color.Red
                    ErrorMessage(Me, "رسالة خطأ", "عذرا القيمة الأولى يجب أن تكون أكبر من القيمة الثانية")
                    Exit Sub
                End If
                If GVROLE.GetRowCellValue(i, "ValTo") > MaxValue Then
                    GVROLE.Appearance.EvenRow.BackColor = Color.Red
                    ErrorMessage(Me, "رسالة خطأ", "عذرا أقصى قيمة للتحويل في هذه الخدمة هي" & Space(1) & MaxValue & "الرجاء إختيار قيمة أقل")
                    Exit Sub
                End If
                If GVROLE.GetRowCellValue(i, "DisVal") > MaxSerVal Then
                    GVROLE.Appearance.EvenRow.BackColor = Color.Red
                    ErrorMessage(Me, "رسالة خطأ", "عذرا أقصى قيمة لعمولة التحويل في هذه الخدمة هي" & Space(1) & MaxSerVal & "الرجاء إختيار قيمة أقل")
                    Exit Sub
                End If
                Dim CellValue As Object = GVROLE.GetRowCellValue(i, "RateType")
                If CellValue = "قيمة" Then
                    RatType = 0
                ElseIf CellValue = "نسبة مئوية" Then
                    RatType = 1
                End If
                Dim DT As DataTable
                DT = GCRole_seteing(i)
                Dim PRM(9) As SqlParameter
                PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = GVROLE.GetRowCellValue(i, "ID")}
                PRM(1) = New SqlParameter("@TypeNo", SqlDbType.TinyInt) With {.Value = 0}
                PRM(2) = New SqlParameter("@RateType ", SqlDbType.TinyInt) With {.Value = RatType}
                PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = 0}
                PRM(4) = New SqlParameter("@TransType ", SqlDbType.TinyInt) With {.Value = 0}
                PRM(5) = New SqlParameter("@Type", SqlDbType.Structured) With {.Value = GCRole_seteing(i)}
                PRM(6) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = 1}
                PRM(7) = New SqlParameter("@IsUpdate", SqlDbType.TinyInt) With {.Value = 1}
                PRM(8) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                PRM(9) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
                RUN_EXUTE_PRO("CATEGORYTYPESTB_Insert", PRM)
            Next
            NEWRECORD()
            FrmSavedSuccessfully.Show()
        Else
            Exit Sub
        End If
    End Sub

    Public Function GCRole_seteing(i As Integer) As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("ID")
        dt.Columns.Add("ValFrom")
        dt.Columns.Add("ValTo")
        dt.Columns.Add("DisVal")
        dt.Columns.Add("RType")
        Dim CellValue As Object = GVROLE.GetRowCellValue(i, "ID")
        If CellValue IsNot Nothing OrElse CellValue.ToString() <> String.Empty Or CellValue <> 0 Then
            dt.Rows.Add(GVROLE.GetRowCellValue(i, "ID"), GVROLE.GetRowCellValue(i, "ValFrom"), GVROLE.GetRowCellValue(i, "ValTo"), GVROLE.GetRowCellValue(i, "DisVal"), GVROLE.GetRowCellValue(i, "RType"))
        End If
        Return dt
    End Function

    Private Sub ServiceID_EditValueChanged(sender As Object, e As EventArgs) Handles ServiceID.EditValueChanged
        GCROLE.DataSource = Nothing
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVROLE.CustomUnboundColumnData
        If e.Column.FieldName = "RowHandle" And e.IsGetData Then
            e.Value = GVROLE.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub ServiceID_TextChanged(sender As Object, e As EventArgs) Handles ServiceID.TextChanged
        Dim DT As New DataTable
        DT.Clear()
        DT = FRMCATEGORYTYPES.Cat_Get_maxVal(ServiceID.EditValue)
        If DT.Rows.Count > 0 Then
            MaxValue = DT.Rows(0)("MaxValue")
            MaxSerVal = DT.Rows(0)("MaxSerVal")
        Else
            MaxValue = 0
            MaxSerVal = 0
        End If
    End Sub

    Private Sub BtnDelete_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles BtnDelete.ButtonClick
        If GVROLE.RowCount = 1 Then
            ErrorMessage(Me, "رسالة خطأ", "عذرا لا يمكنك حذف جميع الشرائح يجب أن يكون هناك شريحة واحدة على الأقل")
            Exit Sub
        End If
        Dim resu = XtraMessageBox.Show("سيتم حذف هذه الشريحة، هل تريد الاستمرار؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If resu = DialogResult.Yes Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = GVROLE.GetFocusedRowCellValue("ID")}
            RUN_EXUTE_PRO("CATEGORYTYPESDETAILSTB_Delete", PRM)
            LOADDATA()
        Else
            Exit Sub
        End If
    End Sub
End Class