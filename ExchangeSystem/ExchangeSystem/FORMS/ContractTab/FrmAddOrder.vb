Imports System.Data.SqlClient

Public Class FrmAddOrder
    Public IsUpdate As Boolean
    Sub NewRecored()
        New_Controlrs(Me)
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        LoadToControlar(OrderType, "CONDB_OrderTB_LoadParentToLKP", "AccName", "AccCode", Nothing)
        IsUpdate = False
        lodePreportes()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(179, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Private Sub FrmAddOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecored()
    End Sub

    Private Sub OrderType_TextChanged(sender As Object, e As EventArgs) Handles OrderType.TextChanged
        If OrderType.Text <> String.Empty Then
            Code.Text = GetLKPColumnVal(OrderType, "AccCode").ToString + "0" + GetLKPColumnVal(OrderType, "MaxID").ToString
        End If
    End Sub

    Public Overrides Sub Save()
        Try
            Dim PRM(7) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.BigInt) With {.Value = Convert.ToInt64(Code.Text.Trim)}
            PRM(1) = New SqlParameter("@OrderName", SqlDbType.NVarChar, -1) With {.Value = OrderName.Text.Trim}
            PRM(2) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(3) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(4) = New SqlParameter("@MsgBox", SqlDbType.NVarChar - 1) With {.Direction = ParameterDirection.Output}
            PRM(5) = New SqlParameter("@Phone", SqlDbType.NVarChar - 1) With {.Value = Phone.Text.Trim}
            PRM(6) = New SqlParameter("@Notes", SqlDbType.NVarChar - 1) With {.Value = Notes.Text.Trim}
            PRM(7) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = GetLKPColumnVal(OrderType, "AccCode")}
            RUN_EXUTE_PRO("CONDB_OrderTb_Insert", PRM)
            If PRM(3).Value = 0 Then
                ErrorMessage(Me, "رسالة خطأ", PRM(4).Value.ToString)
                Exit Sub
            Else
                If IsUpdate = False Then
                    FrmSavedSuccessfully.Show()
                    NewRecored()
                ElseIf IsUpdate = True Then
                    FrmEditMessage.Show()
                    NewRecored()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        MyBase.Save()
    End Sub

    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        FrmViewAddOrder.ShowDialog()
    End Sub
    Public Overrides Sub BNew()
        NewRecored()
        MyBase.BNew()
    End Sub
End Class