Public Class FrmWWW_Wep
    Public Async Sub Lode(URL As String)
        Try
            Await WebView21.EnsureCoreWebView2Async(Nothing)
            WebView21.CoreWebView2.Navigate(URL)
        Catch ex As Exception
            MessageBox.Show("فشل تحميل الخريطة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class