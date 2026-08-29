
Imports System.Data.SqlClient

Imports DevExpress.XtraGrid.Views.Base

Public Class FRMTransfer_commissions

    Public Overrides Sub BNew()
        New_Controlrs(Me)
        LoadToControlar(GridControl1, "Transfer_commissions_select", "", "", Nothing)
        DVGFormat(GridView1)
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Caption = "اضافة عمولة تحويل "
        MyBase.BNew()
    End Sub

    Private Sub FRMTransfer_commissions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnNew.PerformClick()
    End Sub
    Public Overrides Sub Save()
        Transfer_commissions_inserert(0, 0)
        BtnNew.PerformClick()
        FrmSavedSuccessfully.ShowDialog()
        MyBase.Save()
    End Sub

    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Public Sub Transfer_commissions_inserert(Type_ID As Integer, ID As ULong)
        Try
            SplashScreenManager1.ShowWaitForm()

            Dim prm(6) As SqlParameter
            prm(0) = New SqlParameter("@First_Value", SqlDbType.Float) With {.Value = Daily.EditValue}
            prm(1) = New SqlParameter("@Second_value", SqlDbType.Float) With {.Value = Weekly.EditValue}
            prm(2) = New SqlParameter("@Commission_value", SqlDbType.Float) With {.Value = Annual.EditValue}
            prm(3) = New SqlParameter("@typeInsertrt", SqlDbType.Int) With {.Value = Type_ID}
            prm(4) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
            prm(5) = New SqlParameter("@SMG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("[Transfer_commissions_inserert]", prm)

            If prm(5).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                ErrorMessage(Me, "رسالة خطأ", prm(6).Value)
            Else
                SplashScreenManager1.CloseWaitForm()
            End If

        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Daily.EditValue = GridView1.GetFocusedRowCellValue("First_Value")
        Weekly.EditValue = GridView1.GetFocusedRowCellValue("Second_value")
        Annual.EditValue = GridView1.GetFocusedRowCellValue("Commission_value")
        Transfer_commissions_inserert(1, GridView1.GetFocusedRowCellValue("ID"))
        LoadToControlar(GridControl1, "Transfer_commissions_select", "", "", Nothing)
    End Sub

    Private Sub Annual_Leave(sender As Object, e As EventArgs) Handles Annual.Leave
        BtnSave.PerformClick()
    End Sub
End Class