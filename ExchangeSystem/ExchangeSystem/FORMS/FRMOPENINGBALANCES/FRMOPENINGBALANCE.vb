Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Public Class FRMOPENINGBALANCE
    Dim CLSOB As New CLSOPENINGBALANCE
    Public OpeningBalance As Integer
    Public IDCode, AccountParent As ULong
    Public IsUpdate, IsPartner As Boolean
    Public Acname As String = ""
    Sub ENABLEDCONTROLS(ISchick As Boolean)
        Code.Enabled = ISchick
        BranchID.Enabled = ISchick
        FirstAccMain.Enabled = ISchick
        FirstAccParent.Enabled = ISchick
        OppVal.Enabled = ISchick
        Notes.Enabled = ISchick
        FirstAccID.Enabled = ISchick
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = ISchick
        ValType.Enabled = ISchick
        FirstAccID.Enabled = ISchick
        SecondAccID.Enabled = ISchick
        SecondAccMain.Enabled = ISchick
        SecondAccParent.Enabled = ISchick
        CurrencyID.Enabled = ISchick
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(103, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub LOADACCOUNTTYPE()
        FirstAccMain.Properties.Items.Clear()
        Dim Coll As ComboBoxItemCollection = FirstAccMain.Properties.Items
        Coll.BeginUpdate()
        If BranchID.EditValue = MAINBID Then
            Try
                Coll.Add(New ACCOUNTSADD("الخزائن"))
                Coll.Add(New ACCOUNTSADD("الموظفون"))
                Coll.Add(New ACCOUNTSADD("العملاء"))
                Coll.Add(New ACCOUNTSADD("الجواري"))
                Coll.Add(New ACCOUNTSADD("المصارف"))
            Finally
                Coll.EndUpdate()
            End Try
        ElseIf BranchID.EditValue <> MAINBID Then
            If IsPartner = True Then
                Try
                    Coll.Add(New ACCOUNTSADD("الخزائن"))
                    Coll.Add(New ACCOUNTSADD("الموظفون"))
                    Coll.Add(New ACCOUNTSADD("العملاء"))
                    Coll.Add(New ACCOUNTSADD("رأس المال"))
                Finally
                    Coll.EndUpdate()
                End Try
            ElseIf IsPartner = False Then
                Try
                    Coll.Add(New ACCOUNTSADD("الخزائن"))
                    Coll.Add(New ACCOUNTSADD("الموظفون"))
                    Coll.Add(New ACCOUNTSADD("العملاء"))
                Finally
                    Coll.EndUpdate()
                End Try
            End If
        End If
        FirstAccMain.SelectedIndex = -1
    End Sub
    Sub LOADACCOUNT()
        FirstAccParent.Properties.DataSource = Nothing
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And FirstAccMain.Text <> String.Empty Or FirstAccMain.SelectedIndex <> -1 Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccType", SqlDbType.Decimal) With {.Value = AccountParent}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("OpeningBalanceTb_LoadMainAccounts", PR)
            If DT.Rows.Count > 0 Then
                FirstAccParent.Properties.DataSource = DT
                FirstAccParent.Properties.DisplayMember = "AccName"
                FirstAccParent.Properties.ValueMember = "FAccID"
            End If
        Else
            FirstAccParent.Properties.DataSource = Nothing
        End If
    End Sub
    Sub LOADCURRENCY()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CurrencyMainTb_LOAD_Defult_TOLKP")
        If DT.Rows.Count > 0 Then
            CurrencyID.Properties.DataSource = DT
            CurrencyID.Properties.ValueMember = "ID"
            CurrencyID.Properties.DisplayMember = "CuName"
            CurrencyID.Properties.ShowHeader = False
            'CurrencyID.Properties.PopulateColumns()
            'CurrencyID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Sub NEWRECORD()
        LOADCURRENCY()
        ENABLEDCONTROLS(True)
        IsUpdate = False
        LOADBRNCHDIERCT(BranchID)
        Code.Enabled = False
        Code.Text = ""
        BranchID.EditValue = BID
        FirstAccMain.SelectedIndex = -1
        ValType.SelectedIndex = 0
        CurrencyID.EditValue = 1
        FirstAccParent.EditValue = -1
        FirstAccID.EditValue = -1
        OppVal.EditValue = 0.000
        SecondAccMain.SelectedIndex = -1
        SecondAccParent.EditValue = -1
        SecondAccID.EditValue = -1
        Notes.Text = ""
        Me.BtnEdit.Enabled = False
        Me.BtnPrint.Enabled = False
        Me.BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Me.BtnSave.Enabled = True
        Me.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Private Sub FRMOPENINGBALANCE_Load(sender As Object, e As EventArgs) Handles Me.Load
        LOADCURRENCY()
        lodePreportes()
        NEWRECORD()
    End Sub
    Sub CallFAccount(AccParent As Integer)
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And FirstAccParent.EditValue <> -1 Or FirstAccParent.Text <> String.Empty Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.Int) With {.Value = AccParent}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("OpeningBalanceTb_LoadMainParents", PR)
            If DT.Rows.Count > 0 Then
                FirstAccParent.Properties.DataSource = DT
                FirstAccParent.Properties.DisplayMember = "FAccName"
                FirstAccParent.Properties.ValueMember = "FAccID"
                DVGFormat(FirstGLKB1)
                'FirstGLKB1.Columns("FAccCode").Width = 30
            End If
        End If
    End Sub
    Sub CallFSubAccount(FAccCode As ULong)
        FirstAccID.Properties.DataSource = Nothing
        If FirstAccParent.EditValue <> -1 Or FirstAccParent.Text <> String.Empty Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccCode", SqlDbType.Decimal) With {.Value = FAccCode}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("OpeningBalanceTb_LoadMainParentsExist", PR)
            If DT.Rows.Count > 0 Then
                FirstAccID.Properties.DataSource = DT
                FirstAccID.Properties.DisplayMember = "FSAccName"
                FirstAccID.Properties.ValueMember = "FSAccID"
                DVGFormat(FSGLKB1)
            End If
        End If
    End Sub
    Sub CallSAccount()
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And SecondAccParent.EditValue <> -1 Or SecondAccParent.Text <> String.Empty Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.Int) With {.Value = SecondAccMain.SelectedIndex + 1}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("OpeningBalanceTb_LoadMainParents2", PR)
            If DT.Rows.Count > 0 Then
                SecondAccParent.Properties.DataSource = DT
                SecondAccParent.Properties.DisplayMember = "SAccName"
                SecondAccParent.Properties.ValueMember = "SAccID"
                DVGFormat(SGLKB)
                SGLKB.Columns("SAccCode").Width = 30
            End If
        End If
    End Sub
    Sub CallSSubAccount(SAccCode As ULong)
        If SecondAccParent.EditValue <> -1 Or SecondAccParent.Text <> String.Empty Then
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(1) = New SqlParameter("@AccCode", SqlDbType.Decimal) With {.Value = SAccCode}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("OpeningBalanceTb_LoadMainParentsExist2", PR)
            If DT.Rows.Count > 0 Then
                SecondAccID.Properties.DataSource = DT
                SecondAccID.Properties.DisplayMember = "SSAccName"
                SecondAccID.Properties.ValueMember = "SSAccID"
                DVGFormat(SSGLKB)
            End If
        End If
    End Sub
    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub AccountType_TextChanged(sender As Object, e As EventArgs) Handles FirstAccMain.TextChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And FirstAccMain.Text <> String.Empty Or FirstAccMain.SelectedIndex <> -1 Then
            Call CallFAccount(FirstAccMain.SelectedIndex + 1)
        End If
    End Sub
    Sub CallSecondAccount()
        If FirstAccMain.SelectedIndex = 1 Or FirstAccMain.SelectedIndex = 0 Then
            FirstAccID.Enabled = True
            Dim PR(1) As SqlParameter
            PR(0) = New SqlParameter("@AccCode", SqlDbType.Decimal) With {.Value = AccountParent}
            PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("OpeningBalanceTb_LoadMainParentsNotExist", PR)
            If DT.Rows.Count > 0 Then
                FirstAccID.Properties.DataSource = DT
                FirstAccID.Properties.ValueMember = "AccID"
                FirstAccID.Properties.DisplayMember = "AccName"
            End If
        End If
    End Sub
    Private Sub AccID_TextChanged(sender As Object, e As EventArgs) Handles FirstAccParent.TextChanged
        FirstAccID.EditValue = -1
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            Call CallFSubAccount(FirstGLKB1.GetFocusedRowCellValue("FAccCode"))
        End If
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If IsUpdate = False Then
            Code.Enabled = False
            Code.Text = ""
            FirstAccMain.SelectedIndex = -1
            CurrencyID.EditValue = -1
            FirstAccParent.EditValue = -1
            OppVal.EditValue = 0.000
            Notes.Text = ""
        End If
    End Sub
#Region "insert,update,search"
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub SetData()
        IsDataValidLKP(BranchID)
        IsDataValidComboBoxEdit(FirstAccMain)
        If OppVal.EditValue <= 0.000 Then
            OppVal.ErrorText = "يرجى إدخال القيمة"
            Exit Sub
        End If
        CLSOB.OpeningBalanceTb_Insert(Code.Text.Trim, Date.Now, BranchID.EditValue, ValType.SelectedIndex, FirstAccMain.SelectedIndex + 1, FirstAccParent.EditValue,
                                      FirstAccID.EditValue, SecondAccMain.SelectedIndex + 1, SecondAccParent.EditValue, SecondAccID.EditValue, CurrencyID.EditValue,
                                      OppVal.EditValue, Notes.Text.Trim, 1, UserID)
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Sub SHOWRECORD(x)
        Try
            Dim DT As New DataTable
            DT.Clear()
            DT = CLSOB.SERACH_OpeningBalance(x)
            If DT.Rows.Count > 0 Then
                AccountType_TextChanged(Nothing, Nothing)
                Code.Text = DT.Rows(0)("Code").ToString
                BranchID.EditValue = DT.Rows(0)("BranchID")
                ValType.SelectedIndex = DT.Rows(0)("ValType")
                FirstAccMain.SelectedIndex = DT.Rows(0)("FirstAccMain") - 1
                If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And FirstAccMain.Text <> String.Empty Or FirstAccMain.SelectedIndex <> -1 Then
                    Call CallFAccount(DT.Rows(0)("FirstAccMain"))
                    FirstAccParent.EditValue = DT.Rows(0)("FAccID")
                End If
                If FirstAccParent.EditValue <> -1 Or FirstAccParent.Text <> String.Empty Then
                    Call CallFSubAccount(DT.Rows(0)("FAccCode"))
                    FirstAccID.EditValue = DT.Rows(0)("FSAccID")
                End If
                SecondAccMain.SelectedIndex = DT.Rows(0)("SecondAccMain") - 1
                If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty And SecondAccMain.Text <> String.Empty Or SecondAccMain.SelectedIndex <> -1 Then
                    Call CallSAccount()
                    SecondAccParent.EditValue = DT.Rows(0)("SAccID")
                    Call CallSSubAccount(DT.Rows(0)("SAccCode"))
                    SecondAccID.EditValue = DT.Rows(0)("SSAccID")
                End If
                CurrencyID.EditValue = DT.Rows(0)("CurrencyID")
                OppVal.EditValue = DT.Rows(0)("OPPVal")
                Notes.Text = DT.Rows(0)("Notes").ToString
                ENABLEDCONTROLS(False)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub UPDATERECORD()
        IsDataValidLKP(BranchID)
        IsDataValidLKP(CurrencyID)
        'IsDataValidLKP(AccID)
        IsDataValidComboBoxEdit(FirstAccMain)
        If OppVal.EditValue <= 0.000 Then
            OppVal.ErrorText = "يرجى إدخال القيمة"
            Exit Sub
        End If
        'CLSOB.OpeningBalanceTb_Insert(Code.Text.Trim, Date.Now, BranchID.EditValue, FirstAccParent.EditValue, CurrencyID.EditValue, OppVal.EditValue,
        '                              OppVal.EditValue, UserID, Notes.Text.Trim, IDCode, IsUpdate, FirstAccMain.SelectedIndex, ValType.SelectedIndex)
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub Remove()
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim res = XtraMessageBox.Show(lookAndFeelError, "سيتم حذف السجل ولا يمكن التراجع مرة أخرى، هل تريد تأكيد الحذف؟", "رسالة تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If res = DialogResult.Yes Then
            CLSOB.OpeningBalanceTb_Delete(Code.Text.Trim, IsUpdate)
            NEWRECORD()
        Else
            NEWRECORD()
            Exit Sub
        End If
        MyBase.Remove()
    End Sub
    Private Sub VIEWFRM_Click(sender As Object, e As EventArgs) Handles VIEWFRM.Click
        FRMVIEWOPENINBALANCE.ShowDialog()
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            Call CallFAccount(FirstAccMain.SelectedIndex + 1)
            Call CallSAccount()
        End If
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_QUERY_ONLY("Select IsPartner from CoBranch where ID='" & BranchID.EditValue & "'")
        If DT.Rows.Count > 0 Then
            IsPartner = DT.Rows(0)("IsPartner")
        End If
    End Sub
    Private Sub AccountType1_TextChanged(sender As Object, e As EventArgs) Handles SecondAccMain.TextChanged
        CallSAccount()
    End Sub
    Private Sub FirstAccID_TextChanged(sender As Object, e As EventArgs) Handles FirstAccID.TextChanged

    End Sub

    Private Sub FirstAccParent_EditValueChanged(sender As Object, e As EventArgs) Handles FirstAccParent.EditValueChanged
        If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
            Call CallFSubAccount(FirstGLKB1.GetFocusedRowCellValue("FAccCode"))
        End If
    End Sub



    Private Sub AccID1_TextChanged(sender As Object, e As EventArgs) Handles SecondAccParent.TextChanged
        Call CallSSubAccount(SGLKB.GetFocusedRowCellValue("SAccCode"))
    End Sub
    Private Sub FRMOPENINGBALANCE_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        NEWRECORD()
    End Sub

    Private Sub FirstAccID_EditValueChanged(sender As Object, e As EventArgs) Handles FirstAccID.EditValueChanged
        If IsUpdate = False Then
            If FirstAccID.EditValue <> -1 Or FirstAccID.Text <> String.Empty Then
                CLSOB.OpeningBalance_MaxID(BranchID.EditValue, FirstAccID.EditValue)
            End If
        End If
    End Sub
#End Region
End Class
Public Class ACCOUNTSADD
    Private _firstName As String
    Public Sub New(ByVal firstName As String)
        _firstName = firstName
    End Sub
    Public Overrides Function ToString() As String
        Return _firstName
    End Function
End Class