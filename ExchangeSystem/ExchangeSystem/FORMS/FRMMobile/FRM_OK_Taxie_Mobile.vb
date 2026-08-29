Imports System.Data.SqlClient
Public Class FRM_OK_Taxie_Mobile
    Public Async Sub LodeUREl(URL As String)
        Try



            Await WebView21.EnsureCoreWebView2Async(Nothing)
            WebView21.CoreWebView2.Navigate(URL)
            'SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception

            MessageBox.Show("فشل تحميل الخريطة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub loddate(urlsataes As String, codeTax As String, OverVale As String, Rsevetname As String, Rphone As String)
        SplashScreenManager1.ShowWaitForm()
        New_Controlrs(Me)
        Code.Text = codeTax
        OverVale_TXt.Text = OverVale
        Rname.Text = Rsevetname
        RphoneTxr.Text = Rphone
        LodeUREl(urlsataes)
        SplashScreenManager1.CloseWaitForm()
    End Sub

    Public Sub InternalEx_update_Taxi_ISMobile()
        Try


            SplashScreenManager1.ShowWaitForm()
            Dim prm(3) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code.Text}
            prm(1) = New SqlParameter("@TaxiValues", SqlDbType.Decimal, 18, 3) With {.Value = TaxiValue.EditValue}
            prm(2) = New SqlParameter("@Smg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(3) = New SqlParameter("@Masge", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("InternalEx_update_Taxi_ISMobile", prm)
            If prm(2).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                ErrorMessage2("رسالة تنبية", prm(3).Value)
            Else
                FrmReceivetaxirequestfromtheapp.GET_Deteels_fOr_taxe(BID)
                FrmSavedSuccessfully.ShowDialog()
                SplashScreenManager1.CloseWaitForm()
                Me.Close()
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage2("رسالة تنبية", ex.Message)
        End Try

    End Sub

    Private Sub FRM_OK_Taxie_Mobile_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If Code.Text = String.Empty Then
            Code.ErrorText = "هذه الحقل مطلوب"
            Return
        End If

        If TaxiValue.Text = String.Empty Then
            TaxiValue.ErrorText = "هذه الحقل مطلوب"
            Return
        End If
        InternalEx_update_Taxi_ISMobile()
    End Sub
End Class