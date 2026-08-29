Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Drawing.Helpers
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Partial Public Class FRMCATEGORYTYPES
    Public IsUpdate As Integer
    Public msgST As Int16
    Public MaxValue, MaxSerVal As Decimal
    Public Sub NEWRECORD()
        Code.Text = GETMAXID("CATEGORYTYPESTB", "ID") + 1
        TypeNoTxT.EditValue = 0
        RateTypeTxT.SelectedIndex = -1
        ValFromTxT.EditValue = 0.000
        ValToTxT.EditValue = 0.000
        DisValTxT.EditValue = 0.000
        CountryID.EditValue = -1
        TransTypeTxT.EditValue = -1
        IsUpdate = 0
        LOADCOUNTRIES()
        Dim bindlis As List(Of EntryTRANSTYPE) = New List(Of EntryTRANSTYPE)
        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
        CountryID.Enabled = True
        TransTypeTxT.Enabled = True
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(102, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub TransTypeLoad()
        If CountryID.EditValue = -1 Or CountryID.Text = String.Empty Then
            CountryID.ErrorText = "يجب اختيار الدولة"
            Exit Sub
        End If
        TransTypeTxT.Properties.DataSource = Nothing
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TransTypeTb_SelectByCountry", PR)
        If DT.Rows.Count > 0 Then
            TransTypeTxT.Properties.DataSource = DT
            TransTypeTxT.Properties.ValueMember = "SRID"
            TransTypeTxT.Properties.DisplayMember = "SRNAME"
            GLGV.OptionsView.ShowColumnHeaders = False
            GLGV.Columns("SRID").Visible = False
            NEWDVGFROMAT(GLGV)
        End If
        DT.Dispose()
    End Sub
    Sub LOADCOUNTRIES()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "ID"
            CountryID.Properties.DisplayMember = "CName"
            GLGV1.OptionsView.ShowColumnHeaders = False
            NEWDVGFROMAT(GLGV1)
        End If
        DT.Dispose()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
#Region "GVROLE"
    Sub DVGFormat()
        GVRole.AddNewRow()
        GVRole.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.True
        GVRole.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True
        'GVRole.OptionsBehavior.Editable = False
        GVRole.Columns("Delete").OptionsColumn.AllowEdit = True
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
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
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
    Private Sub GVRole_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVRole.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName Is "Delete" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("Delete"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(0, 153, 204)
                e.Appearance.BackColor2 = Color.FromArgb(0, 153, 204)
            End If
        End If
    End Sub

    Private Sub GVRole_RowCountChanged(sender As Object, e As EventArgs) Handles GVRole.RowCountChanged
        For i As Integer = 0 To GVRole.RowCount - 1
            GVRole.SetRowCellValue(i, "SN", i + 1)
        Next
    End Sub
    Private Sub GCRole_DoubleClick(sender As Object, e As EventArgs) Handles GCRole.DoubleClick
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        ValFromTxT.Focus()
        GVRole.RefreshRow(GVRole.FocusedRowHandle)
        DVGFormat()
        If IsUpdate = 0 Then
            If GVRole.RowCount > 0 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "TypeTo")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Or CellValue = 0 Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                ValFromTxT.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub

    Sub DeleteRow(BranchID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        RUN_EXUTE_PRO("BranchRatesTempTb_Delete", PRM)
        'LOADSBRANCH()
    End Sub
    Private Sub GCRole_Click(sender As Object, e As EventArgs) Handles GCRole.Click
        GVRole.OptionsBehavior.Editable = False
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        ValFromTxT.Focus()
        GVRole.RefreshRow(GVRole.FocusedRowHandle)
        DVGFormat()
        If IsUpdate = 0 Then
            If GVRole.RowCount > 0 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "TypeTo")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Or CellValue = 0 Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                ValFromTxT.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub
    Sub AddCatToDVG()

        If CountryID.EditValue = -1 Then
            CountryID.ErrorText = "هذا الحل مطلوب"
            Exit Sub
        End If
        If TransTypeTxT.EditValue = -1 Then
            TransTypeTxT.ErrorText = "هذا الحل مطلوب"
            Exit Sub
        End If
        If TypeNoTxT.EditValue = 0 Or TypeNoTxT.EditValue = 1 Then
            TypeNoTxT.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If
        If ValFromTxT.EditValue < 0.000 Then
            ValFromTxT.ErrorText = "القيمة لا يجب أن تكون أقل من صفر"
            Exit Sub
        End If
        If ValToTxT.EditValue < 0.000 Then
            ValToTxT.ErrorText = "القيمة لا يجب أن تكون أقل من صفر"
            Exit Sub
        End If
        If ValFromTxT.EditValue > ValToTxT.EditValue Then
            ValFromTxT.ErrorText = "القيمة الأولى لا يجب أن تكون أكبر من القيمة الثانية"
            Exit Sub
        End If
        If MaxValue <= 0 Then
            ErrorMessage(Me, "رسالة خطأ", "لم يتم تحديد قيمة قصوى لهذه الخدمة الرجاء الذهاب لشاشة إضافة خدمة وتحديد القيمة من هناك")
            Exit Sub
        End If
        If ValToTxT.EditValue > MaxValue Then
            ErrorMessage(Me, "رسالة خطأ", "عذرا أقصى قيمة للتحويل في هذه الخدمة هي" & Space(1) & MaxValue & Space(1))
            Exit Sub
        End If
        If DisValTxT.EditValue > MaxSerVal Then
            ErrorMessage(Me, "رسالة خطأ", "عذرا أقصى قيمة للخصم في هذه الخدمة هي" & Space(1) & MaxSerVal & Space(1))
            Exit Sub
        End If
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "TypeNo", TypeNoTxT.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "RateType", RateTypeTxT.SelectedIndex)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "TypeFrom", Format(ValFromTxT.EditValue, "N3"))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "TypeTo", Format(ValToTxT.EditValue, "N3"))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "DisVal", Format(DisValTxT.EditValue, "N3"))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "IDCAT", 1)
        DVGFormat()
        ValFromTxT.EditValue = 0.000
        ValToTxT.EditValue = 0.000
        DisValTxT.EditValue = 0.000
        ValFromTxT.Focus()
    End Sub
    Private Sub TransTypeTxT_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles TransTypeTxT.ButtonClick
        If e.Button.Index = 1 Then
            FRMSERVICETYPE.ShowDialog()
        End If
    End Sub
#End Region
    Public Enum Gender
        Male
        Female
    End Enum

    Private Sub FRMCATEGORYTYPES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        BtnNew.PerformClick()

    End Sub

    Private Sub CountryID_TextChanged(sender As Object, e As EventArgs) Handles CountryID.TextChanged
        TransTypeLoad()
    End Sub

    Private Sub ValToTxT_TextChanged(sender As Object, e As EventArgs) Handles ValToTxT.TextChanged
        If ValToTxT.EditValue = 0.000 Then
            If IsUpdate = 0 Then

                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "TypeTo")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Or CellValue = 0.000 Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                ValFromTxT.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
            End If
        End If
    End Sub
    Public Function GCRole_seteing() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("CATID")
        dt.Columns.Add("TypeFrom")
        dt.Columns.Add("TypeTo")
        dt.Columns.Add("DisVal")
        dt.Columns.Add("RateType")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "TypeTo")
            If CellValue IsNot Nothing OrElse CellValue.ToString() <> String.Empty Or CellValue <> 0.000 Then
                dt.Rows.Add(Code.Text, GVRole.GetRowCellValue(i, "TypeFrom"), GVRole.GetRowCellValue(i, "TypeTo"), GVRole.GetRowCellValue(i, "DisVal"), GVRole.GetRowCellValue(i, "RateType"))
            End If
        Next
        Return dt
    End Function
    Dim DT As DataTable
    Public Sub CATEGORY_Insert()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.DevExpressDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If CountryID.EditValue = -1 Then
            CountryID.ErrorText = "هذا الحل مطلوب"
            Exit Sub
        End If
        If TransTypeTxT.EditValue = -1 Then
            TransTypeTxT.ErrorText = "هذا الحل مطلوب"
            Exit Sub
        End If
        If TypeNoTxT.EditValue = 0 Or TypeNoTxT.EditValue < 0 Then
            TypeNoTxT.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If
        If GVRole.RowCount < 0 Then
            XtraMessageBox.Show(lookAndFeelError, "يجب أن يكون هناك عنصر أو أكثر لاتمام عملية الحفظ", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        DT = GCRole_seteing()
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = Code.Text}
        PRM(1) = New SqlParameter("@TypeNo", SqlDbType.TinyInt) With {.Value = TypeNoTxT.EditValue}
        PRM(2) = New SqlParameter("@RateType ", SqlDbType.TinyInt) With {.Value = RateTypeTxT.SelectedIndex}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        PRM(4) = New SqlParameter("@TransType ", SqlDbType.Int) With {.Value = TransTypeTxT.EditValue}
        PRM(5) = New SqlParameter("@Type", SqlDbType.Structured) With {.Value = GCRole_seteing()}
        PRM(6) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive.EditValue}
        PRM(7) = New SqlParameter("@IsUpdate", SqlDbType.TinyInt) With {.Value = IsUpdate}
        PRM(8) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(9) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        RUN_EXUTE_PRO("CATEGORYTYPESTB_Insert", PRM)
        msgST = PRM(8).Value
        If PRM(8).Value = 0 Then
            XtraMessageBox.Show(lookAndFeelError, PRM(9).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            BtnNew.PerformClick()
        End If
    End Sub
    Public Overrides Sub SetData()
        CATEGORY_Insert()
        If msgST = 1 Then
            MyBase.SetData()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub



    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        FRMCATEGORYTYPESRATEUPDATE.ShowDialog()
    End Sub
    Public Function Cat_Get_maxVal(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExtTraServiceTypeTb_SelectAllByID", PRM)
        Return DT
    End Function

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        AddCatToDVG()
    End Sub

    Private Sub DisValTxT_Leave(sender As Object, e As EventArgs) Handles DisValTxT.Leave
        SimpleButton21.Focus()
    End Sub



    Private Sub TransTypeTxT_TextChanged(sender As Object, e As EventArgs) Handles TransTypeTxT.TextChanged
        Dim DT As New DataTable
        DT.Clear()
        DT = Cat_Get_maxVal(TransTypeTxT.EditValue)
        If DT.Rows.Count > 0 Then
            MaxValue = DT.Rows(0)("MaxValue")
            MaxSerVal = DT.Rows(0)("MaxSerVal")
        Else
            MaxValue = 0
            MaxSerVal = 0
        End If
    End Sub
End Class
Public Class EntryTRANSTYPE
    Public Property SN() As Integer = 0
    Public Property TypeNo() As Integer = 0
    Public Property RateType() As String = ""
    Public Property TypeFrom() As Decimal = Format(0.000, "N3")
    Public Property TypeTo() As Decimal = Format(0.000, "N3")
    Public Property DisVal() As Decimal = Format(0, "N3")
    Public Property IDCAT() As ULong

End Class
