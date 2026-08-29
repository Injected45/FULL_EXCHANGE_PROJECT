Imports System.Data.SqlClient

Imports DevExpress.XtraGrid.Views.Base

Public Class FRM_ADD_FROM_Costmer_Mobile
    Dim clscust As New CLSCUSTOMER
    Public Sub Table_ADD_forCostumerMobile_select()
        GridControl1.DataSource = Nothing
        LoadToControlar(GridControl1, "Table_ADD_forCostumerMobile_select", "", "", Nothing)
        DVGFormat(GridView1)
    End Sub

    Private Sub FRM_ADD_FROM_Costmer_Mobile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Table_ADD_forCostumerMobile_select()
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        CustomersTb_insert_Mobile(GridView1.GetFocusedRowCellValue("ID"), 0)

    End Sub

    Public Sub CustomersTb_insert_Mobile(ID As ULong, Type_update As Integer)
        Try
            SplashScreenManager1.ShowWaitForm()

            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
            prm(1) = New SqlParameter("@ueser_Accpit", SqlDbType.Int) With {.Value = UserID}
            prm(2) = New SqlParameter("@Type_update", SqlDbType.Int) With {.Value = Type_update}
            prm(3) = New SqlParameter("@MSG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@MasgBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@ACCIDFRom", SqlDbType.BigInt) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("CustomersTb_insert_Mobile", prm)
            If prm(3).Value = 0 Then
                ErrorMessage(Me, "رسالة تنبية", prm(4).Value)
            Else
                If Type_update = 0 Then

                    Dim CUSTOMER_name, CUSTOMER_phone As String

                    CUSTOMER_phone = GridView1.GetFocusedRowCellValue("phone")
                    CUSTOMER_name = GridView1.GetFocusedRowCellValue("NAME_for_Cousntas")
                    Dim mms As String = "*شركة الرحالة للصرافة*" & vbNewLine &
                        "🎉 تم فتح حسابكم بنجاح" &
                        vbNewLine & "🤵‍♂" & " " &
                        CUSTOMER_name & vbNewLine &
                 "📱" & Space(1) & "الهاتف" &
                 Space(1) & ":" & Space(1) &
                 CUSTOMER_phone & vbNewLine &
                  "🔐" & Space(1) &
                  "كود الحساب" &
                  Space(1) & ":" &
                  Space(1) & prm(5).Value
                    WATSAPPMsAG(CUSTOMER_phone, mms, False)
                    FrmSavedSuccessfully.ShowDialog()
                End If


                Table_ADD_forCostumerMobile_select()
            End If
            SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try
    End Sub

    Private Sub Cansel_Click(sender As Object, e As EventArgs) Handles Cansel.Click
        CustomersTb_insert_Mobile(GridView1.GetFocusedRowCellValue("ID"), 1)
    End Sub
End Class