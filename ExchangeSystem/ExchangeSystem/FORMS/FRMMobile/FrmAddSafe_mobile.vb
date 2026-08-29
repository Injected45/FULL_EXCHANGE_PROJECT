
Imports System.Data.SqlClient
Public Class FrmAddSafe_mobile
    Public Frmid As Integer
    Public Sub newRecorides()
        New_Controlrs(Me)
        LoadToControlar(BranchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        lodePreportes()
    End Sub
    Public Overrides Sub BNew()
        newRecorides()
        MyBase.BNew()
    End Sub

    Private Sub FrmAddSafe_mobile_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub FrmAddSafe_mobile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        newRecorides()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(Frmid, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Public clsa As New CLSAccount
    Public Sub Get_AccountID(fatherparent As Decimal)
        Dim dt As New DataTable
        dt.Clear()
        dt = clsa.ACCOUNTSTB_SelectMax(fatherparent, 1)
        If dt.Rows.Count > 0 Then
            AccCode.EditValue = dt.Rows(0)("code")
            'AccCat.SelectedIndex = dt.Rows(0)("Accline")

        End If
    End Sub

    Public Function fatherparent()
        Dim fatherparents As New Decimal

        Dim dt As New DataTable
        dt.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        dt = RUN_QUARY_PRO("AccountsTb_acode", prm)

        Return dt.Rows(0)("AccCode")
    End Function

    Private Sub LookUpEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        If BranchID.Text <> Nothing Then


            If BranchID.EditValue > -1 Then
                Get_AccountID(fatherparent)


            End If
        End If
    End Sub



    Public Overrides Sub SetData()
        Try


            If AccCode.Text = String.Empty Then
                AccCode.ErrorText = "هذه الحقل مطلوب"
                Return
            End If

            If BranchID.Text = String.Empty Then
                BranchID.ErrorText = "هذه الحقل مطلوب"
                Return
            End If

            If AccName.Text = String.Empty Then
                AccName.ErrorText = "هذه الحقل مطلوب"
                Return
            End If

            If AccPhone.Text = String.Empty Then
                AccPhone.ErrorText = "هذه الحقل مطلوب"
                Return
            End If
            If cackid_phone(AccPhone.Text.Trim, True) = False Then
                AccPhone.ErrorText = "عذرا رقم الهاتف  غير مسجل في وتساب"
                Return
            End If
            Dim prm(8) As SqlParameter
            prm(0) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = AccCode.EditValue}
            prm(1) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1) With {.Value = AccName.EditValue}
            prm(2) = New SqlParameter("@AccPhone", SqlDbType.NVarChar, -1) With {.Value = AccPhone.EditValue}
            prm(3) = New SqlParameter("@AccMobile", SqlDbType.NVarChar, -1) With {.Value = AccMobile.EditValue}
            prm(4) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(5) = New SqlParameter("@AddUser", SqlDbType.BigInt) With {.Value = UserID}
            prm(6) = New SqlParameter("@LimitedVal", SqlDbType.Float) With {.Value = AccCode.EditValue}
            prm(7) = New SqlParameter("@Imsg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(8) = New SqlParameter("@MSg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("AccountsTb_SafeMobile", prm)
            If prm(7).Value = 0 Then
                ErrorMessage(Me, "AccountsTb_SafeMobile ", prm(8).Value)
            Else
                newRecorides()
                FrmSavedSuccessfully.Show()
            End If
        Catch ex As Exception
            ErrorMessage(Me, "ex_Message", ex.Message)
        End Try
        MyBase.SetData()
    End Sub

    Public Overrides Sub Save()
        SetData()

        MyBase.Save()

    End Sub
End Class