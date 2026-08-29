Imports System.Data.SqlClient

Public Class FRMSERVICETYPE

    Public IsUpdate As Boolean
    Dim cu As New SERVICECLSS
    Public AccID As ULong
    Public Sub LoadCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            NEWDVGFROMAT(CountryGV)
        End If
    End Sub




    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(10, UserID)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If

    End Sub

    Sub NEWRECORD()

        LoadCountry()
        CountryID.EditValue = -1
        IsUpdate = False
        ServiceName.Text = ""
        CodeID.Enabled = False
        ServiceName.Select()
        IsActiveTG.IsOn = True
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        CodeID.Text = GETMAXID("ExtTraServiceTypeTb", "ID") + 1
        LOADDATA()
        LSBOX.SelectedIndex = -1
        DisConstant.EditValue = 0
        MaxValue.EditValue = 0
        MaxSerValue.EditValue = 0
    End Sub
    Public Sub LOADDATA()
        LSBOX.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ExtTraServiceTypeTb_LoadToLSBOX")
        If DT.Rows.Count > 0 Then
            LSBOX.DataSource = DT
            LSBOX.DisplayMember = "ServiceName"
            LSBOX.ValueMember = "ID"
        End If
    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        lodePreportes()
        NEWRECORD()

    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Try
            If IsUpdate = False Then
                If ServiceName.Text = String.Empty Then
                    ServiceName.ErrorText = "هذا الحقل مطلوب"
                    Return
                End If
                IsActiveTG.EditValue = True
                cu.EMPCSFT_INSERT(CodeID.Text, ServiceName.Text.Trim, IsUpdate, CountryID.EditValue, 1, DisConstant.EditValue, MaxValue.EditValue, MaxSerValue.EditValue)
                FrmSavedSuccessfully.Show()
                NEWRECORD()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If IsUpdate = True Then
            If ServiceName.Text = String.Empty Then
                ServiceName.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            IsActiveTG.EditValue = True
            cu.EMPCSFT_INSERT(CodeID.Text, ServiceName.Text.Trim, IsUpdate, CountryID.EditValue, IsActiveTG.EditValue, DisConstant.EditValue, MaxValue.EditValue, MaxSerValue.EditValue)
            FrmEditMessage.Show()
            NEWRECORD()
        End If
    End Sub
    Private Sub FRMBANK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub LSBOX_Click(sender As Object, e As EventArgs) Handles LSBOX.Click
                   Dim DT As New DataTable
            DT.Clear()
            DT = cu.EMPCSFT_Select(LSBOX.SelectedValue)
        If DT.Rows.Count > 0 Then
            IsUpdate = True
            BtnEdit.Enabled = True
            BtnSave.Enabled = False
            ServiceName.Text = DT.Rows(0)("ServiceName").ToString
            CodeID.Text = DT.Rows(0)("ID")
            IsActiveTG.EditValue = DT.Rows(0)("IsActive")
            CountryID.EditValue = DT.Rows(0)("CountryID")
            DisConstant.EditValue = DT.Rows(0)("DisConstant")
            MaxValue.EditValue = DT.Rows(0)("MaxValue")
            MaxSerValue.EditValue = DT.Rows(0)("MaxSerVal")
        End If

    End Sub
End Class
Public Class SERVICECLSS
    Public Function EMPCSFT_Select(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ExtTraServiceTypeTb_SelectAllByID", PRM)
        Return DT
    End Function
    Public Function EMPCSFT_SelectAll() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ExtTraServiceTypeTb_LoadToLSBOX")
        Return DT
    End Function
    Public Sub EMPCSFT_INSERT(ID As Integer, ServiceName As String, IsUpdate As Boolean, CountryID As Integer, IsActive As Boolean, DisConstant As Double, MaxValue As Double, MaxSerValue As Double)
        Dim PRM(7) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@ServiceName", SqlDbType.NVarChar, -1) With {.Value = ServiceName}
        PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
        PRM(4) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(5) = New SqlParameter("@DisConstant", SqlDbType.Decimal) With {.Value = DisConstant}
        PRM(6) = New SqlParameter("@MaxValue", SqlDbType.Decimal) With {.Value = MaxValue}
        PRM(7) = New SqlParameter("@MaxSerValue", SqlDbType.Decimal) With {.Value = MaxSerValue}
        RUN_EXUTE_PRO("ExtTraServiceTypeTb_Insert", PRM)
    End Sub
    Public Sub EMPCSFT_DELETE(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        RUN_EXUTE_PRO("EmployeeClassificationTb_Delete", PRM)
    End Sub
End Class