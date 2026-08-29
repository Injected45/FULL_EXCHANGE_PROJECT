Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Module LOADTOLKP
    Public Sub CURRENCYTB_LoadWithBranch_forbr(BranchID As LookUpEdit, LKP As LookUpEdit)
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CURRENCYTB_LoadWithBranch_forbr", PR)
            If DT.Rows.Count > 0 Then
                LKP.Properties.DataSource = DT
                LKP.Properties.ValueMember = "CurrID"
                LKP.Properties.DisplayMember = "CurrencyName"
                LKP.Properties.ShowHeader = False
            End If
        End If
    End Sub
    Sub NEWDVGFROMAT(GVRole As GridView)
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        'GVRole.OptionsBehavior.Editable = False
        'GVRole.OptionsBehavior.EditingMode = False
        'GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.OptionsSelection.EnableAppearanceFocusedRow = False
        GVRole.OptionsSelection.MultiSelectMode = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True





    End Sub
    Public Sub EMP_WITHOUTBRANCH(LKP As LookUpEdit)
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("EmployeeTb_LOADINTOLKPWITHACCIDANDNOBRANCHID")
        If dt.Rows.Count > 0 Then
            LKP.Properties.DataSource = dt
            LKP.Properties.ValueMember = "AccID"
            LKP.Properties.DisplayMember = "EMPNAME"
            LKP.Properties.ShowHeader = False
        Else
            LKP.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub EMP_WITHBRANCH(LKP As LookUpEdit, EMPlkp As LookUpEdit)
        If LKP.Text <> String.Empty Or LKP.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = LKP.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONPETTYCASHACCID", PR)
            If dt.Rows.Count > 0 Then
                EMPlkp.Properties.DataSource = dt
                EMPlkp.Properties.ValueMember = "AccID"
                EMPlkp.Properties.DisplayMember = "EMPNAME"
                EMPlkp.Properties.ShowHeader = False
            End If
        Else
            EMPlkp.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub ACTIVEEMP_WITHBRANCH(LKP As LookUpEdit, EMPlkp As LookUpEdit)
        If LKP.Text <> String.Empty Or LKP.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = LKP.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("EmployeeTb_LOADINTOLKPBASEDONPETTYCASHACCID", PR)
            If dt.Rows.Count > 0 Then
                EMPlkp.Properties.DataSource = dt
                EMPlkp.Properties.ValueMember = "AccID"
                EMPlkp.Properties.DisplayMember = "EMPNAME"
                EMPlkp.Properties.ShowHeader = False
            End If
        Else
            EMPlkp.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LOADCUST_WITHBRANCH(LKP As LookUpEdit, EMPlkp As LookUpEdit)
        If LKP.EditValue = -1 Or LKP.EditValue = Nothing Or LKP.Text = String.Empty Then
            LKP.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If LKP.Text <> String.Empty Or LKP.EditValue <> -1 Or LKP.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = LKP.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CustomersTb_LOADTOLKPBasedOnBranchID", PR)
            If dt.Rows.Count > 0 Then
                EMPlkp.Properties.DataSource = dt
                EMPlkp.Properties.ValueMember = "AccID"
                EMPlkp.Properties.DisplayMember = "CustName"
                EMPlkp.Properties.ShowHeader = False
            End If
        Else
            EMPlkp.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LOADBBRANCH_WITHBRANCH(LKP As LookUpEdit, EMPlkp As LookUpEdit)
        If LKP.EditValue = -1 Or LKP.EditValue = Nothing Or LKP.Text = String.Empty Then
            LKP.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If LKP.Text <> String.Empty Or LKP.EditValue <> -1 Or LKP.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = LKP.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_LOADTOLKPBasedOnBranchID", PR)
            If dt.Rows.Count > 0 Then
                EMPlkp.Properties.DataSource = dt
                EMPlkp.Properties.ValueMember = "AccID"
                EMPlkp.Properties.DisplayMember = "BranchName"
                EMPlkp.Properties.ShowHeader = False
            End If
        Else
            EMPlkp.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LOADBBRANCH_WITHBankID(LKP As LookUpEdit, EMPlkp As LookUpEdit)
        If LKP.EditValue = -1 Or LKP.EditValue = Nothing Or LKP.Text = String.Empty Then
            LKP.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If LKP.Text <> String.Empty Or LKP.EditValue <> -1 Or LKP.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = LKP.EditValue}

            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("BBranchTb_LOADTOLKPBasedOnBankID", PR)
            If dt.Rows.Count > 0 Then
                EMPlkp.Properties.DataSource = dt
                EMPlkp.Properties.ValueMember = "AccID"
                EMPlkp.Properties.DisplayMember = "BranchName"
                EMPlkp.Properties.ShowHeader = False
            End If
        Else
            EMPlkp.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LOADBRNCHDIERCT(LKP As LookUpEdit)
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit2")
        LKP.Properties.DataSource = DT
        LKP.Properties.ValueMember = "DBRID"
        LKP.Properties.DisplayMember = "BName"
        LKP.Properties.ShowHeader = False
    End Sub
    Public Sub LOADBRNCHHasEmp(LKP As LookUpEdit)
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadconnectedBranch")
        LKP.Properties.DataSource = DT
        LKP.Properties.ValueMember = "DBRID"
        LKP.Properties.DisplayMember = "BName"
        LKP.Properties.ShowHeader = False
    End Sub
    Public Sub LOADBRNCHFORCOUNTRY(LKP As GridLookUpEdit, CNTID As GridLookUpEdit)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("CountryID", SqlDbType.Int) With {.Value = CNTID.EditValue}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPWITHCOUNTRY", PR)
        LKP.Properties.DataSource = DT
        LKP.Properties.ValueMember = "DBRID"
        LKP.Properties.DisplayMember = "BName"

    End Sub
    Public Sub LOADCURRENCYFORALL(LKP As LookUpEdit)
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CURRENCYTB_LoadToLKP")
        If DT.Rows.Count > 0 Then
            LKP.Properties.DataSource = DT
            LKP.Properties.ValueMember = "ID"
            LKP.Properties.DisplayMember = "CurrencyName"
            LKP.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub LOADCURRENCYBASEDBRANCH(BranchID As LookUpEdit, LKP As LookUpEdit)
        If BranchID.Text <> String.Empty Or BranchID.EditValue <> -1 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CURRENCYTB_LoadWithBranch", PR)
            If DT.Rows.Count > 0 Then
                LKP.Properties.DataSource = DT
                LKP.Properties.ValueMember = "ID"
                LKP.Properties.DisplayMember = "CurrencyName"
                LKP.Properties.ShowHeader = False
            End If
        End If
    End Sub
    Public Sub LOADCURRENCYFORBRANCH(Branch As LookUpEdit, AccID As LookUpEdit, LKP As LookUpEdit)
        If Branch.EditValue = -1 Or Branch.EditValue = Nothing Or Branch.Text = String.Empty Then
            Branch.ErrorText = "يجب اختيار الفرع"
            Return
        End If
        If AccID.EditValue = -1 Or AccID.EditValue = Nothing Or AccID.Text = String.Empty Then
            AccID.ErrorText = "يجب اختيار الحساب"
            Return
        End If
        If Branch.Text <> String.Empty Or Branch.EditValue <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = Branch.EditValue}
            PR(1) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = AccID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CURRENCYTB_LoadBasedOnBranch", PR)
            If DT.Rows.Count > 0 Then
                LKP.Properties.DataSource = DT
                LKP.Properties.ValueMember = "ID"
                LKP.Properties.DisplayMember = "CurrencyName"
                LKP.Properties.ShowHeader = False
                LKP.Properties.PopulateColumns()
                LKP.Properties.Columns("ID").Visible = False
            End If
        Else
            LKP.Properties.DataSource = Nothing
        End If
    End Sub
    Public Sub LOADBANK(LKP As LookUpEdit)
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("BanksTb_SelectAll")
        LKP.Properties.DataSource = DT
        LKP.Properties.ValueMember = "ID"
        LKP.Properties.DisplayMember = "BankName"
        LKP.Properties.ShowHeader = False
    End Sub
    Public Sub IsDataValidTextEdit(textEd As TextEdit)

        If textEd.Text.Trim() = String.Empty Then

            textEd.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
    End Sub
    Public Sub IsDataValidLKP(lkp As LookUpEdit)
        Dim flag As Integer = 0
        If lkp.Text.Trim() = String.Empty Or lkp.EditValue = -1 Then

            lkp.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If

    End Sub
    Public Sub IsDataValidSpinEdit(sped As SpinEdit)
        If sped.EditValue <= 0.000 Then

            sped.ErrorText = "القيمة لا يجب أن تكون صفر أو أقل"
            Exit Sub
        End If
    End Sub
    Public Sub IsDataValidComboBoxEdit(coxed As ComboBoxEdit)
        If coxed.SelectedIndex = -1 Then
            coxed.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If
    End Sub
End Module
