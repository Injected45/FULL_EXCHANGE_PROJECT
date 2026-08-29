Imports System.Data.SqlClient
Imports DevExpress.CodeParser
Imports DevExpress.DataProcessing.InMemoryDataProcessor
Imports DevExpress.LookAndFeel
Imports DevExpress.Pdf.Xmp
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting.Shape.Native
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraRichEdit.Model

Public Class FrmMultiAcountEdit
    Public IsUpdate As Boolean
    Sub NewRecord()
        New_Controlrs(Me)
        Enable_Controls(Me, True)
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit2", "BName", "DBRID", Nothing)
        LoadToControlar(BranchIDTo, "CoBranches_LoadDataIntoLookUpEdit2", "BName", "DBRID", Nothing)
        LoadToControlar(CurrencyID, "CurrencyMainTb_LOAD_Defult_TOLKP", "CuName", "ID", Nothing)
        BranchID.EditValue = BID
        BranchIDTo.EditValue = BID
        CurrencyID.EditValue = DefaultCurrency
        DateEdit11.EditValue = Now.Date
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        Dim bindlis As List(Of EntryPROPCSETTLEME) = New List(Of EntryPROPCSETTLEME)
        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GCRole.DataSource = binddata
        '===========================
        GCRole.LookAndFeel.UseDefaultLookAndFeel = False
        GVRole.BorderStyle = BorderStyles.NoBorder
        DVGFormat()
        IsUpdate = False
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 172)
        lodePreportes()
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
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(172, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If
    End Sub

    Private Sub FrmMultiAcountEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub
    Sub addCatToDVG(AccIDFrom As LookUpEdit, AccIDTo As LookUpEdit, TypeVal As Integer)
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False

        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccName", SecondAccID.Text.Trim)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccID", Convert.ToUInt64(FirstAccID.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "AccIDTo", Convert.ToUInt64(SecondAccID.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "NotesDe", Notes2.Text.Trim)
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Branch", Convert.ToInt32(BranchID.EditValue))
        GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "BranchIDTo", Convert.ToInt32(BranchIDTo.EditValue))

        Dim val As Decimal = 0D
        If OverAllTotal.EditValue IsNot Nothing Then val = Convert.ToDecimal(OverAllTotal.EditValue)

        If TypeVal = 0 Then
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Debit", val)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Credit", 0D)
        Else
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Debit", 0D)
            GVRole.SetRowCellValue(GCRole.NewItemRowHandle, "Credit", val)
        End If

        DVGFormat()
    End Sub
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

    Private Sub FirstAccMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FirstAccMain.SelectedIndexChanged
        FirstAccParent.Properties.DataSource = Nothing
        FirstAccParent.EditValue = -1
        If BranchID.Text = String.Empty Then Exit Sub
        If FirstAccMain.SelectedIndex = -1 Then Exit Sub
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@AccParent", SqlDbType.Int) With {.Value = FirstAccMain.SelectedIndex + 1}
        LoadToControlar(FirstAccParent, "OpeningBalanceTb_LoadMainParents", "FAccName", "FAccCode", PR)
    End Sub

    Private Sub FirstAccParent_EditValueChanged(sender As Object, e As EventArgs) Handles FirstAccParent.EditValueChanged
        FirstAccID.Properties.DataSource = Nothing
        FirstAccID.EditValue = -1
        If BranchID.Text = String.Empty Then Exit Sub
        If FirstAccParent.Text = String.Empty Then Exit Sub
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        PR(1) = New SqlParameter("@AccCode", SqlDbType.Decimal) With {.Value = FirstAccParent.EditValue}
        LoadToControlar(FirstAccID, "OpeningBalanceTb_LoadMainParentsExist", "FSAccName", "FSAccID", PR)
    End Sub

    Private Sub SecondAccMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SecondAccMain.SelectedIndexChanged

        SecondAccParent.Properties.DataSource = Nothing
        SecondAccParent.EditValue = -1
        If BranchID.Text = String.Empty Then Exit Sub
        If SecondAccMain.SelectedIndex = -1 Then Exit Sub
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchIDTo.EditValue}
        PR(1) = New SqlParameter("@AccParent", SqlDbType.Int) With {.Value = SecondAccMain.SelectedIndex + 1}
        LoadToControlar(SecondAccParent, "OpeningBalanceTb_LoadMainParents", "FAccName", "FAccCode", PR)
    End Sub

    Private Sub SecondAccParent_EditValueChanged(sender As Object, e As EventArgs) Handles SecondAccParent.EditValueChanged
        SecondAccID.Properties.DataSource = Nothing
        SecondAccID.EditValue = -1
        If BranchID.Text = String.Empty Then Exit Sub
        If SecondAccParent.Text = String.Empty Then Exit Sub
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchIDTo.EditValue}
        PR(1) = New SqlParameter("@AccCode", SqlDbType.Decimal) With {.Value = SecondAccParent.EditValue}
        LoadToControlar(SecondAccID, "OpeningBalanceTb_LoadMainParentsExist", "FSAccName", "FSAccID", PR)
    End Sub

    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        FirstAccMain.SelectedIndex = -1
        SecondAccMain.SelectedIndex = -1
        ValueType.SelectedIndex = -1
        Code.Text = "55" + "-" + UserID.ToString + "-" + (GETMAXID("MultiAcountEditTB", "IDCode") + 1).ToString
    End Sub

    Private Sub SimpleButton111_Click(sender As Object, e As EventArgs) Handles SimpleButton111.Click
        If FirstAccID.Text = String.Empty Or SecondAccID.Text = String.Empty Then
            ErrorMessage(Me, "خطأ", "يجب اختيار الحسابين الأول والثاني")
            Exit Sub
        End If
        If ValueType.SelectedIndex = -1 Then
            ErrorMessage(Me, "خطأ", "يجب اختيار طبيعة القيمة")
            Exit Sub
        End If
        If CurrencyID.EditValue = -1 Then
            ErrorMessage(Me, "خطأ", "يجب اختيار العملة")
            Exit Sub
        End If
        If AccVal.EditValue = 0.000 Then
            AccVal.ErrorText = "القيمة يجب أن تكون أكبر من صفر"
            Exit Sub
        End If
        'If OverAllTotal.EditValue > AccVal.EditValue Then
        '    ErrorMessage(Me, "خطأ", "قيمة الحساب الثاني لا يجب أن تكون أكبر من قيمة الحساب الأول")
        '    Exit Sub
        'End If
        Dim TypeVal2 As Integer
        If ValueType.SelectedIndex = 0 Then
            TypeVal2 = 1
        Else
            TypeVal2 = 0
        End If
        'addCatToDVG(FirstAccID, SecondAccID, ValueType.SelectedIndex)
        addCatToDVG(SecondAccID, FirstAccID, TypeVal2)
        SecondAccID.EditValue = -1
        SecondAccID.Select()
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
            If OverAllTotal.EditValue < 0 Then
                OverAllTotal.ErrorText = "القيمة يجب أن تكون أكبر من صفر"
                Return
            End If
            If MovmentType.Text = String.Empty Then
                MovmentType.ErrorText = "هذا الحقل لايجب أن يكون فارغا"
                Return
            End If
            If GVRole.RowCount < 1 Then
                ErrorMessage(Me, "رسالة خطأ", "يجب اختيار عملية واحدة على الأقل")
                Exit Sub
            End If

            MultiAccountEdit_insert(GCRole_seteing())
        End If
        MyBase.SetData()
    End Sub
    Public Sub MultiAccountEdit_insert(dt As DataTable)
        Try
            Dim prm(12) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (300)) With {.Value = Code.Text.Trim}
            prm(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(2) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
            prm(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID.EditValue}
            prm(4) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            prm(5) = New SqlParameter("@Type", SqlDbType.Structured) With {.Value = dt}
            prm(6) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(8) = New SqlParameter("@MovmentType", SqlDbType.NVarChar, -1) With {.Value = MovmentType.Text.Trim}
            prm(9) = New SqlParameter("@OverAllVal", SqlDbType.Decimal) With {.Value = AccVal.EditValue}
            prm(10) = New SqlParameter("@ValType", SqlDbType.Int) With {.Value = ValueType.SelectedIndex}
            prm(11) = New SqlParameter("@FirstAccID", SqlDbType.BigInt) With {.Value = FirstAccID.EditValue}
            prm(12) = New SqlParameter("@BranchIDTo", SqlDbType.Int) With {.Value = BranchIDTo.EditValue}
            RUN_EXUTE_PRO("MultiAcountEditTB_Insert", prm)
            If prm(6).Value = 0 Then
                ErrorMessage(Me, "رسالة تنبيه", prm(7).Value)
                If Me.IsUpdate = False Then
                    Code.Text = "55" + "-" + UserID.ToString + "-" + (GETMAXID("MultiAcountEditTB", "IDCode") + 1).ToString
                    Exit Sub
                End If

            End If
            FrmSavedSuccessfully.Show()
            Print()
            NewRecord()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Function GCRole_seteing() As DataTable
        Dim dt As New DataTable
        dt.Clear()

        dt.Columns.Add("AccID")
        dt.Columns.Add("Debit")
        dt.Columns.Add("Credit")
        dt.Columns.Add("NotesDe")
        dt.Columns.Add("AccIDTo")
        dt.Columns.Add("Branch")
        dt.Columns.Add("BranchIDTo")
        For i As Integer = 0 To GVRole.RowCount - 1
            Dim CellValue As Object = GVRole.GetRowCellValue(i, "AccName")
            Dim MOVTYPE As String = "مقابل مصروفات لحساب" & Space(1) & CellValue
            If CellValue <> String.Empty Then
                dt.Rows.Add(GVRole.GetRowCellValue(i, "AccID"), GVRole.GetRowCellValue(i, "Debit"), GVRole.GetRowCellValue(i, "Credit").ToString,
                            GVRole.GetRowCellValue(i, "NotesDe"), GVRole.GetRowCellValue(i, "AccIDTo"), GVRole.GetRowCellValue(i, "Branch"), GVRole.GetRowCellValue(i, "BranchIDTo"))
            End If
        Next
        Return dt
    End Function
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@AccCode", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ZPRT_MultiAcountEditTB_Select", PR)
        If dt.Rows.Count > 0 Then
            Dim report As New MultiAccount
            report.DataSource = dt
            report.DataMember = "MultiAcountEditDetailsTB"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.FilterString = GVRole.ActiveFilterString
            report.XrLabel8.Text = Code.Text
            report.XrLabel13.Text = DateEdit11.Text
            report.XrLabel6.Text = BranchID.Text
            report.XrLabel1.Text = CurrencyID.Text
            report.XrLabel2.Text = GetUserName
            report.CreateDocument()
            report.ShowPreview()
        Else
            MessageBox.Show("عذرا لايوجد بيانات لطباعتها", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        MyBase.Print()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FrmViewMultiAcountEdit.ShowDialog()
    End Sub

    Private Sub BranchIDTo_EditValueChanged(sender As Object, e As EventArgs) Handles BranchIDTo.EditValueChanged
        SecondAccMain.SelectedIndex = -1
    End Sub
End Class

Public Class EntryPROPCSETTLEME

    Public Property AccName() As String
    Public Property AccID() As ULong
    Public Property Debit() As Decimal
    Public Property Credit() As Decimal
    Public Property NotesDe() As String
    Public Property AccIDTo() As ULong
    Public Property Branch() As Integer
    Public Property BranchIDTo() As Integer

End Class