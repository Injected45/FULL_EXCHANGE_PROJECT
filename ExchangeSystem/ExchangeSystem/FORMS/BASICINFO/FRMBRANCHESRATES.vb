Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class FRMBRANCHESRATES
    Public IsUpdate, IsExist As Boolean
    Sub NEWRECORD()
        IsUpdate = False
        'FirstBranchID.EditValue = -1
        LOADFBRANCH()
        SecondBranchID.EditValue = -1
        Dim bindlis As New List(Of EntryBRATES)

        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
    End Sub
#Region "GVROLE"
    Sub DVGFormat()
        GVRole.AddNewRow()
        GVRole.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.True
        GVRole.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True
        'GVRole.OptionsBehavior.Editable = False
        'GVRole.Columns("Delete").OptionsColumn.AllowEdit = True
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
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(101, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then LayoutControlItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


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


    Private Sub GVRole_RowCountChanged(sender As Object, e As EventArgs) Handles GVRole.RowCountChanged
        For i As Integer = 0 To GVRole.RowCount - 1
            GVRole.SetRowCellValue(i, "SN", i + 1)
        Next
    End Sub
    Private Sub GCRole_DoubleClick(sender As Object, e As EventArgs) Handles GCRole.DoubleClick
        If IsUpdate = False Then
            If GVRole.RowCount = 2 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "SBranchName")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                FirstBranchID.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub

    Sub DeleteRow(BranchID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        RUN_EXUTE_PRO("BranchRatesTempTb_Delete", PRM)
        LOADSBRANCH()
    End Sub
    Private Sub GCRole_Click(sender As Object, e As EventArgs) Handles GCRole.Click
        If IsUpdate = False Then
            If GVRole.RowCount > 0 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "SBranchName")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                FirstBranchID.Focus()
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub
    Sub AddCatToDVG()
        If FirstBranchID.EditValue = -1 Then
            FirstBranchID.ErrorText = "يرجى اختيار الفرع أولاً"
            Exit Sub
        End If
        If SecondBranchID.EditValue = -1 Then
            SecondBranchID.ErrorText = "يرجى اختيار الفرع أولاً"
            Exit Sub
        End If
        If FirstBranchR.EditValue < 0.000 Then
            FirstBranchR.ErrorText = "النسبة لا يجب أن تكون أقل من صفر"
            Exit Sub
        End If
        If SecondBranchR.EditValue < 0.000 Then
            SecondBranchR.ErrorText = "النسبة لا يجب أن تكون أقل من صفر"
            Exit Sub
        End If
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "SBranchName", SecondBranchID.Text)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "FBRate", FirstBranchR.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "SBRate", SecondBranchR.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "FBranchID", FirstBranchID.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "SBranchID", SecondBranchID.EditValue)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "ID", 1)
        DVGFormat()
        SecondBranchID.EditValue = -1
        FirstBranchR.EditValue = 0.000
        SecondBranchR.EditValue = 0.000
        LOADSBRANCH()
        SecondBranchID.Focus()
    End Sub
#End Region
    Sub LOADFBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("BranchRatesTb_LoadFBranchToLKP")
        If DT.Rows.Count > 0 Then
            FirstBranchID.Properties.DataSource = DT
            FirstBranchID.Properties.ValueMember = "ID"
            FirstBranchID.Properties.DisplayMember = "BName"
            FirstBranchID.Properties.ShowHeader = False
            FirstBranchID.Properties.PopulateColumns()
            FirstBranchID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Public Sub BranchRatesTempTb_Insert(BrID As ULong)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BrID", SqlDbType.BigInt) With {.Value = BrID}
        RUN_EXUTE_PRO("BranchRatesTempTb_Insert", PRM)
    End Sub
    Sub LOADSBRANCH()
        SecondBranchID.EditValue = -1

        If FirstBranchID.EditValue = -1 Or FirstBranchID.Text = String.Empty Then
            FirstBranchID.ErrorText = "يجب اختيار الفرع الأول"
            Exit Sub
        ElseIf FirstBranchID.EditValue <> -1 Or FirstBranchID.Text <> String.Empty Then
            If GVRole.RowCount > 0 Then
                IsExist = True
            Else
                IsExist = False
            End If
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = FirstBranchID.EditValue}
            PR(1) = New SqlParameter("@IsExist", SqlDbType.Bit) With {.Value = IsExist}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("BranchRatesTb_LoadSBranchToLKP", PR)
            If DT.Rows.Count > 0 Then
                SecondBranchID.Properties.DataSource = DT
                SecondBranchID.Properties.ValueMember = "ID"
                SecondBranchID.Properties.DisplayMember = "BName"
                SecondBranchID.Properties.ShowHeader = False
                SecondBranchID.Properties.PopulateColumns()
                SecondBranchID.Properties.Columns("ID").Visible = False
            Else
                SecondBranchID.EditValue = -1
                SecondBranchID.Properties.DataSource = Nothing
            End If
        Else
            SecondBranchID.EditValue = -1
            SecondBranchID.Properties.DataSource = Nothing
        End If
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles FirstBranchID.TextChanged
        SecondBranchID.EditValue = -1

        If FirstBranchID.EditValue = -1 Or FirstBranchID.Text = String.Empty Then
            FirstBranchID.ErrorText = "يجب اختيار الفرع الأول"
            Exit Sub
        ElseIf FirstBranchID.EditValue <> -1 Or FirstBranchID.Text <> String.Empty Then
            If GVRole.RowCount > 0 Then
                IsExist = True
            Else
                IsExist = False
            End If
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = FirstBranchID.EditValue}
            PR(1) = New SqlParameter("@IsExist", SqlDbType.Bit) With {.Value = IsExist}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("BranchRatesTb_LoadSBranchToLKP", PR)
            If DT.Rows.Count > 0 Then
                SecondBranchID.Properties.DataSource = DT
                SecondBranchID.Properties.ValueMember = "ID"
                SecondBranchID.Properties.DisplayMember = "BName"
                SecondBranchID.Properties.ShowHeader = False
                SecondBranchID.Properties.PopulateColumns()
                SecondBranchID.Properties.Columns("ID").Visible = False
            Else
                SecondBranchID.EditValue = -1
                SecondBranchID.Properties.DataSource = Nothing
            End If
        Else
            SecondBranchID.EditValue = -1
            SecondBranchID.Properties.DataSource = Nothing
        End If
    End Sub
    Dim brra As New BRRATES
#Region "Save,Update,Delete"
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GVRole.RowCount = 0 Then
            ErrorMessage(Me, "رسالة خطأ", "يجب اختيار فرع واحد على الأقل")
            Exit Sub
        End If
        For i = 0 To GVRole.RowCount - 1
            If GVRole.GetRowCellValue(i, "FBRate") + GVRole.GetRowCellValue(i, "SBRate") > 100.0 Then
                ErrorMessage(Me, "رسالة خطأ", "النسبة لا يجب أن تكون أكبر من 100")
                Exit Sub
            Else
                brra.CURRENCYPRICE_Insert(0, GVRole.GetRowCellValue(i, "FBranchID"), GVRole.GetRowCellValue(i, "FBRate"), GVRole.GetRowCellValue(i, "SBranchID"), GVRole.GetRowCellValue(i, "SBRate"), IsUpdate)
            End If
        Next
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
#End Region
    Private Sub FRMBRANCHESRATES_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        NEWRECORD()
    End Sub

    Private Sub PCVal1_Leave(sender As Object, e As EventArgs) Handles SecondBranchR.Leave
        If SecondBranchID.EditValue <> -1 Or SecondBranchID.Text <> String.Empty Then
            Dim SBRID = SecondBranchID.EditValue
            BranchRatesTempTb_Insert(SBRID)
        End If
        AddCatToDVG()
        BranchID_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub SecondBranchiD_TextChanged(sender As Object, e As EventArgs) Handles SecondBranchID.TextChanged
        If IsUpdate = False Then

            Dim rowIdx As Integer = GVRole.DataRowCount - 1
            For i As Integer = rowIdx To 0 Step -1
                Dim CellValue As Object = GVRole.GetRowCellValue(i, "SBranchName")
                If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                    GVRole.DeleteRow(i)
                End If
            Next
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMBranchRateUpdate.ShowDialog()
    End Sub
    Private Sub FirstBranchID_EditValueChanged(sender As Object, e As EventArgs) Handles FirstBranchID.EditValueChanged
        BtnNew.PerformClick()
        LOADFBRANCH()
    End Sub

    Private Sub FirstBranchID_Click(sender As Object, e As EventArgs) Handles FirstBranchID.Click
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("BranchRatesTb_LoadFBranchToLKP")
        If DT.Rows.Count = 0 Then
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True
            XtraMessageBox.Show(lookAndFeelError, "لا يوجد فرع جديد في الوقت الحالي", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        If FirstBranchID.EditValue <> -1 Or FirstBranchID.Text <> String.Empty Then
            FFBRID = FirstBranchID.EditValue
        Else
            FFBRID = 0
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FFBRID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BranchRatesTempTb_Truncate", PR)
        If DT.Rows.Count > 0 Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FFBRID}
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("BranchRatesTempTb_Truncate", PRM)
        End If
        FirstBranchID.EditValue = -1
        NEWRECORD()
    End Sub
    Dim FFBRID As Integer
    Private Sub SimpleButton1111_Click(sender As Object, e As EventArgs) Handles SimpleButton1111.Click
        If FirstBranchID.EditValue <> -1 Or FirstBranchID.Text <> String.Empty Then
            FFBRID = FirstBranchID.EditValue
        Else
            FFBRID = 0
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FFBRID}
        Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("BranchRatesTempTb_Truncate", PR)
            If DT.Rows.Count > 0 Then
                Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FFBRID}
            Dim DTT As New DataTable
                DTT.Clear()
                DTT = RUN_QUARY_PRO("BranchRatesTempTb_Truncate", PRM)
            End If

            FirstBranchID.EditValue = -1
        NEWRECORD()
        Me.Close()
    End Sub

    Private Sub FRMBRANCHESRATES_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        FirstBranchID.EditValue = -1
    End Sub
End Class
Public Class BRRATES
    Public Sub CURRENCYPRICE_Insert(ID As ULong, FBranchID As Integer, FBRate As Decimal, SBranchID As Integer, SBRate As Decimal, IsUpdate As Boolean)
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = ID}
        PRM(1) = New SqlParameter("@FBranchID", SqlDbType.Int) With {.Value = FBranchID}
        PRM(2) = New SqlParameter("@FBRate ", SqlDbType.Decimal) With {.Value = FBRate}
        PRM(3) = New SqlParameter("@SBranchID", SqlDbType.Int) With {.Value = SBranchID}
        PRM(4) = New SqlParameter("@SBRate ", SqlDbType.Decimal) With {.Value = SBRate}
        PRM(5) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("BranchRatesTb_Insert", PRM)

    End Sub
End Class
Public Class EntryBRATES
    Public Property SN() As Integer
    Public Property SBranchName() As String
    Public Property FBRate() As Decimal
    Public Property SBRate() As Decimal
    Public Property FBranchID() As Integer
    Public Property SBranchID() As Integer
    Public Property ID() As ULong

End Class