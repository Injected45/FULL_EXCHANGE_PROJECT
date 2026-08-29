Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Public Class FrmDiscountsAndBonuses
    Public IsUpdate As Boolean
    Dim DisCon As Integer
    Dim incCon As Integer
    Sub NewRecord()
        New_Controlrs(Me)
        Enable_Controls(Me, True)
        LoadToControlar(BranchID, "CoBranches_LoadconnectedBranch", "BName", "DBRID", Nothing)
        LoadToControlar(CurrencyID, "CurrencyMainTb_LOAD_Defult_TOLKP", "CuName", "ID", Nothing)
        BranchID.EditValue = BID
        CurrencyID.EditValue = DefaultCurrency
        CurrencyID.Enabled = False
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        Dim bindlis As List(Of EntryBonesAndDis) = New List(Of EntryBonesAndDis)
        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
        IsUpdate = False
        lodePreportes()
        'FrmScreensTb_Details_UESIRID_GETFrom(UserID, 173)
    End Sub
    Sub addCatToDVG()
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        If BounsOrDis.SelectedIndex = 0 Then
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccName", EmpID.Text)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "EmplloyID", EmpID.EditValue)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "DisVal", Convert.ToDecimal(MVal.EditValue))
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BounsVal", 0)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "NotesDe", Notes2.Text.Trim)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BonesOrDis", 0)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Type", TypeID.EditValue)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Branch", BranchID.EditValue)
            DVGFormat()
        End If
        If BounsOrDis.SelectedIndex = 1 Then
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccName", EmpID.Text)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "EmplloyID", EmpID.EditValue)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "DisVal", 0)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BounsVal", Convert.ToDecimal(MVal.EditValue))
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "NotesDe", Notes2.Text.Trim)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BonesOrDis", 1)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Type", TypeID.EditValue)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Branch", BranchID.EditValue)
            DVGFormat()
        End If
        BounsOrDis.SelectedIndex = -1
        TypeID.EditValue = -1
        MVal.EditValue = 0.000
        Notes2.Text = String.Empty
    End Sub
    Public Function GCRole_seteing() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("EmplloyID")
        dt.Columns.Add("DisVal")
        dt.Columns.Add("BounsVal")
        dt.Columns.Add("NotesDe")
        dt.Columns.Add("BonesOrDis")
        dt.Columns.Add("Type")
        dt.Columns.Add("Branch")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "AccName")
            If CellValue <> String.Empty Then
                dt.Rows.Add(GVRole.GetRowCellValue(i, "EmplloyID"), GVRole.GetRowCellValue(i, "DisVal"),
                            GVRole.GetRowCellValue(i, "BounsVal"), GVRole.GetRowCellValue(i, "NotesDe").ToString, GVRole.GetRowCellValue(i, "Type"), GVRole.GetRowCellValue(i, "BonesOrDis"), GVRole.GetRowCellValue(i, "Branch"))
            End If
        Next
        Return dt
    End Function
    Sub DVGFormat()
        GVRole.AddNewRow()
        GVRole.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.True
        GVRole.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True
        GVRole.OptionsBehavior.Editable = False
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
    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        If BounsOrDis.SelectedIndex = -1 Then
            ErrorMessage(Me, "خطأ", "يجب اختيار نوع العملية")
            Exit Sub
        End If
        If TypeID.Text = String.Empty Then
            ErrorMessage(Me, "خطأ", "يجب اختيار نوع العملية")
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Then
            ErrorMessage(Me, "خطأ", "يجب اختيار العملة")
            Exit Sub
        End If
        If EmpID.EditValue = -1 Then
            ErrorMessage(Me, "خطأ", "يجب اختيار الموظف")
            Exit Sub
        End If
        addCatToDVG()
        EmpID.EditValue = -1
        EmpID.Select()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(173, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            BranchID.EditValue = BID
        End If
    End Sub

    Private Sub GCRole_Click(sender As Object, e As EventArgs) Handles GCRole.Click
        If IsUpdate = False Then
            If GVRole.RowCount > 0 Then
                GVRole.DeleteRow(GVRole.FocusedRowHandle)
                Dim rowIdx As Integer = GVRole.DataRowCount - 1
                For i As Integer = rowIdx To 0 Step -1
                    Dim CellValue As Object = GVRole.GetRowCellValue(i, "AccName")
                    If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                        GVRole.DeleteRow(i)
                    End If
                Next
                GVRole.RefreshRow(GVRole.FocusedRowHandle)
                DVGFormat()
            End If
        End If
    End Sub

    Public Overrides Sub SetData()
        If IsUpdate = False Then
            Dim dt As New DataTable
            dt.Clear()
            If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Return
            End If
            If CurrencyID.EditValue = -1 Or CurrencyID.Text = String.Empty Then
                CurrencyID.ErrorText = "يرجى اختيار العملة"
                Return
            End If
            If MVal.EditValue < 0 Then
                MVal.ErrorText = "القيمة يجب أن تكون أكبر من صفر"
                Return
            End If
            If GVRole.RowCount < 2 Then
                ErrorMessage(Me, "رسالة خطأ", "يجب اختيار عملية واحدة على الأقل")
                Exit Sub
            End If
            MultiAcountEdit_insert(GCRole_seteing())
        End If
        MyBase.SetData()
    End Sub
    Public Sub MultiAcountEdit_insert(dt As DataTable)
        Try
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@Type", SqlDbType.Structured) With {.Value = dt}
            RUN_EXUTE_PRO("DiscountsAndBounses_Insert", prm)
            FrmSavedSuccessfully.Show()
            NewRecord()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        EmpID.Properties.DataSource = Nothing
        EmpID.EditValue = -1
        If BranchID.Text <> String.Empty Then
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            LoadToControlar(EmpID, "EmployeeTb_LOADINTOLKPBASEDOnBRANCH", "EMPNAME", "ID", prm)
        End If
    End Sub

    Private Sub BounsOrDis_SelectedIndexChanged(sender As Object, e As EventArgs) Handles BounsOrDis.SelectedIndexChanged
        TypeID.Properties.DataSource = Nothing
        TypeID.EditValue = -1
        If BounsOrDis.Text <> String.Empty Then
            If BounsOrDis.SelectedIndex = 0 Then
                LoadToControlar(TypeID, "DiscountValTb_LOADINTOLKP", "DISNAME", "ID", Nothing)
            Else
                LoadToControlar(TypeID, "IncreasValTb_LOADINTOLKP", "PIName", "ID", Nothing)
            End If
        End If
    End Sub

    Private Sub FrmDiscountsAndBonuses_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub
End Class

Public Class EntryBonesAndDis

    Public Property AccName() As String
    Public Property EmplloyID() As Integer
    Public Property DisVal() As Decimal
    Public Property BounsVal() As Decimal
    Public Property NotesDe() As String
    Public Property BonesOrDis() As Integer
    Public Property Type() As Integer
    Public Property Branch() As Integer

End Class