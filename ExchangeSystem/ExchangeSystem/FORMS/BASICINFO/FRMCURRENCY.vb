Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel

Public Class FRMCURRENCY
    Dim clsc As New CLSCURRENCY
    Public Property CCBID As Integer
    Public Property IsUpdate As Boolean
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub LOADCURNAME()
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_TXT("CurrencyMainTb_LOADTOLSBOX")
        'CurrencyName.Properties.DataSource = DT
        'CurrencyName.Properties.ValueMember = "ID"
        'CurrencyName.Properties.DisplayMember = "CuName"
        'CurrencyName.Properties.ShowHeader = False
    End Sub
    Sub newRecord()
        Code.Text = Format(GETMAXID("CurrencyTb", "ID") + 1, "CR00000")
        LOADBRANCH()
        LOADCURNAME()
        BranchID.EditValue = -1
        IsActive.IsOn = True
        CurrencyName.EditValue = -1
        CurrencyName.Select()
        PartName.Text = String.Empty
        IsDefault.Checked = False
        IsActive.IsOn = True
        IsLocal.IsOn = False
        ExchangeRate.EditValue = 0.000
        EqualValue.EditValue = 0.000
        EqualValue.Enabled = False
        PartValue.SelectedIndex = 2
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If CurrencyName.Text = String.Empty Then
                CurrencyName.ErrorText = "يرجى إخال اسم العملة"
                Exit Sub
            End If
            Dim dt As New DataTable
            dt = clsc.CHECK_BRANCH_NAME(CurrencyName.Text.Trim, BranchID.EditValue)
            If dt.Rows.Count > 0 Then
                CurrencyName.ErrorText = "هذا الاسم موجود مسبقا"
                Exit Sub
            End If
            If PartName.Text = String.Empty Then
                PartName.ErrorText = "يرجى إدخال اسم جزء العملة"
                Exit Sub
            End If
            If ExchangeRate.EditValue = 0.000 Then
                ExchangeRate.ErrorText = "يرجى إدخال معدل الصرف"
                Exit Sub
            End If
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Exit Sub
            End If

            clsc.INSERTTB__CURRENCY(Code.Text.Trim, CurrencyName.Text.Trim, PartName.Text.Trim, PartValue.SelectedIndex, Format(Convert.ToDecimal(ExchangeRate.EditValue), "N3"),
                                    Format(Convert.ToDecimal(EqualValue.EditValue), "N3"), IsLocal.EditValue, IsActive.EditValue, IsDefault.Checked, BranchID.EditValue)
            'MetroMessageBox.Show(Me, "تم حفظ البيانات بنجاح", "رسالة تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information)
            newRecord()
            Dim frm As FRMVIEWCURRENCY = New FRMVIEWCURRENCY
            frm.LoadData()
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If CurrencyName.Text = String.Empty Then
                CurrencyName.ErrorText = "يرجى إخال اسم العملة"
                Exit Sub
            End If
            If PartName.Text = String.Empty Then
                PartName.ErrorText = "يرجى إدخال اسم جزء العملة"
                Exit Sub
            End If
            If ExchangeRate.EditValue = 0.000 Then
                ExchangeRate.ErrorText = "يرجى إدخال معدل الصرف"
                Exit Sub
            End If
            If BranchID.EditValue = -1 Then
                BranchID.ErrorText = "يرجى اختيار الفرع"
                Exit Sub
            End If
            clsc.UPDATETB_CURRENCY(Code.Text.Trim, CurrencyName.Text.Trim, PartName.Text.Trim, PartValue.SelectedIndex, Format(Convert.ToDecimal(ExchangeRate.EditValue), "N3"),
                                    Format(Convert.ToDecimal(EqualValue.EditValue), "N3"), IsLocal.EditValue, IsActive.EditValue, IsDefault.Checked, BranchID.EditValue)
            newRecord()
            Dim frm As FRMVIEWCURRENCY = New FRMVIEWCURRENCY
            frm.LoadData()
        End If
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_CURRENCY(x As String)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clsc.SERACH_CURRENCY(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                CurrencyName.Text = DT.Rows(0)("CurrencyName").ToString
                PartName.Text = DT.Rows(0)("PartName").ToString
                PartValue.SelectedIndex = DT.Rows(0)("PartValue")
                lblcurrencyname.Text = " كل 1 " + CurrencyName.Text.Trim + " = "
                lblpartvalue.Text = PartName.Text
                ExchangeRate.EditValue = DT.Rows(0)("ExchangeRate")
                EqualValue.EditValue = DT.Rows(0)("EqualValue")
                IsDefault.Checked = DT.Rows(0)("IsDefault")
                IsActive.IsOn = DT.Rows(0)("IsActive")
                IsLocal.IsOn = DT.Rows(0)("IsLocal")
                BranchID.EditValue = DT.Rows(0)("BranchID")
            End If
        End If
    End Sub
    Private Sub CurrencyName_TextChanged(sender As Object, e As EventArgs) Handles CurrencyName.TextChanged
        If CurrencyName.Text <> String.Empty Then
            lblcurrencyname.Text = "كل 1" + CurrencyName.Text.Trim + " = "
        End If
    End Sub

    Private Sub PartName_TextChanged(sender As Object, e As EventArgs) Handles PartName.TextChanged
        If PartName.Text <> String.Empty Then
            lblpartvalue.Text = PartName.Text.Trim
        End If
    End Sub
    Private Sub FRMCURRENCY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo

        newRecord()

        'CHECKBUTTONS()
    End Sub

    Private Sub ExchangeRate_EditValueChanged(sender As Object, e As EventArgs) Handles ExchangeRate.EditValueChanged
        If CDbl(ExchangeRate.EditValue) > 0 Then
            EqualValue.EditValue = 1 / CDbl(ExchangeRate.EditValue)
        End If
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
    End Sub

    Private Sub PictureEdit11_EditValueChanged(sender As Object, e As EventArgs) Handles PictureEdit11.EditValueChanged

    End Sub

    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        FRMVIEWCURRENCY.ShowDialog()
    End Sub

    Private Sub CurrencyName_Click(sender As Object, e As EventArgs) Handles CurrencyName.Click

    End Sub

    Private Sub CurrencyName_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles CurrencyName.ButtonClick
        If e.Button.Index = 1 Then
            CurrencyMain.ShowDialog()
        End If
    End Sub
End Class