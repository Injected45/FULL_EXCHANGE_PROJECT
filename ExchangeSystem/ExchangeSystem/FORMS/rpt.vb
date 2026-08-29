Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports DevExpress.XtraReports.UI

Public Class rpt
    Private Sub XrPictureBox1_BeforePrint(
    sender As Object,
    e As CancelEventArgs
) Handles XrPictureBox1.BeforePrint

        Dim pic As DevExpress.XtraReports.UI.XRPictureBox =
        CType(sender, DevExpress.XtraReports.UI.XRPictureBox)

        Dim url As String = Convert.ToString(GetCurrentColumnValue("passport_photo"))

        If String.IsNullOrWhiteSpace(url) Then
            pic.Image = Nothing
            Return
        End If

        Try
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Using wc As New WebClient()
                Dim bytes As Byte() = wc.DownloadData(url)
                Using ms As New MemoryStream(bytes)
                    pic.Image = Image.FromStream(ms)
                End Using
            End Using

        Catch
            pic.Image = Nothing
        End Try
        XrLabel12.Text = GetUserName

    End Sub
End Class