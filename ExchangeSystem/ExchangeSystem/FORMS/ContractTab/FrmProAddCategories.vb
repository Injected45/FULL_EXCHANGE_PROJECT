Imports System.Data.SqlClient

Public Class FrmProAddCategories
    Public IsUpdate As Boolean, msgST As Integer, AccID As ULong
    Public Sub LOADITEMS()
        ItemID.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CONDB_ItemsTb_Select")
        If DT.Rows.Count > 0 Then
            ItemID.Properties.DataSource = DT
            ItemID.Properties.DisplayMember = "ItemName"
            ItemID.Properties.ValueMember = "ID"
            ItemID.Properties.ShowHeader = False
            ItemID.Properties.PopulateColumns()
            ItemID.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Public Sub LOADCONTRIES()
        CountryID.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CountriesTb_LoadToLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.DisplayMember = "CName"
            CountryID.Properties.ValueMember = "ID"
            CountryID.Properties.ShowHeader = False
            CountryID.Properties.PopulateColumns()
            CountryID.Properties.Columns("ID").Visible = False
            CountryID.Properties.Columns("CCode").Visible = False
        End If
    End Sub
    Sub LOADBRANCH()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CoBranches_LoadToLKPWITHOUTAGENT")
        If dt.Rows.Count > 0 Then
            BranchID.Properties.DataSource = dt
            BranchID.Properties.ValueMember = "DBRID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.ShowHeader = False
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.Columns("DBRID").Visible = False
        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(155, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If
    End Sub
    Sub NewRecord()
        DISAPLEDCONTROLS(True)
        ItemID.EditValue = -1
        CountryID.EditValue = -1
        BranchID.EditValue = -1
        LOADBRANCH()
        LOADITEMS()
        LOADCONTRIES()
        BranchID.EditValue = BID
        CodeID.Text = GETMAXID("ContractDB.dbo.CategoriesTb", "ID") + 1
        ITMDescription.Text = String.Empty
        IsActiveTG.EditValue = 1
        lodePreportes
    End Sub

    Private Sub FrmProAddCategories_Load(sender As Object, e As EventArgs) Handles Me.Load
        NewRecord()
    End Sub
    Public Sub CUSTOMER_INSERT()
        Try
            Dim PRM(11) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = CodeID.Text}
            PRM(1) = New SqlParameter("@CateNName", SqlDbType.NVarChar, -1) With {.Value = ItemID.Text.Trim}
            PRM(2) = New SqlParameter("@ItemID", SqlDbType.Int) With {.Value = ItemID.EditValue}
            PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
            PRM(4) = New SqlParameter("@ITMDescription", SqlDbType.NVarChar, 300) With {.Value = ITMDescription.Text.Trim}
            PRM(5) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(6) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PRM(7) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(8) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(9) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
            PRM(10) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID}
            PRM(11) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            RUN_EXUTE_PRO("CONDB_CategoriesTb_Insert", PRM)
            Me.msgST = PRM(7).Value
            If PRM(7).Value = 0 Then
                ErrorMessage(Me, "رسالة خطأ", PRM(8).Value.ToString)
                Exit Sub
            Else
                Me.BtnNew.PerformClick()
            End If

        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub
    Public Sub Pro_SelectByID(x)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = x}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_AddAssestTb_Select", PRM)
        If dt.Rows.Count > 0 Then
            CodeID.Text = dt.Rows(0).Item("ID")
            ItemID.Text = dt.Rows(0).Item("AssestName")
            BranchID.EditValue = dt.Rows(0).Item("BranchID")
            IsActiveTG.IsOn = dt.Rows(0).Item("IsActiveTG")
        End If
    End Sub
    Public Sub DISAPLEDCONTROLS(IsEnabled As Boolean)
        ItemID.Enabled = IsEnabled
        BranchID.Enabled = IsEnabled
        IsActiveTG.Enabled = IsEnabled
        BranchID.Enabled = IsEnabled
        CountryID.Enabled = IsEnabled
        CodeID.Enabled = False
    End Sub

    Public Overrides Sub SetData()
        If ItemID.Text = String.Empty Then
            ItemID.ErrorText = "يرجى إخال اسم المشروع"
            Exit Sub
        End If
        If BranchID.EditValue = -1 Then
            BranchID.ErrorText = "يرجى إختيار الفرع"
            Exit Sub
        End If
        CUSTOMER_INSERT()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'FrmViewProAddCategories.ShowDialog()
    End Sub

    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub

    'Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
    '    FrmViewAssest.ShowDialog()
    'End Sub

    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
End Class