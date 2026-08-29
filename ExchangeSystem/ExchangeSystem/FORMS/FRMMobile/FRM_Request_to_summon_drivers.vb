Imports System.Threading.Tasks
Imports PusherClient
Imports Newtonsoft.Json.Linq
Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class FRM_Request_to_summon_drivers

    Private LastUpdate As DateTime = New DateTime(1753, 1, 1)
    Private pusher As Pusher
    Private isPusherStarted As Boolean = False

    Public Async Sub Request_to_summon_driversTB_insert()
        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@UeserID_insert", SqlDbType.Int) With {.Value = UserID}
            prm(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}

            RUN_EXUTE_PRO("Request_to_summon_driversTB_insert", prm)

            Dim message As String = $"طلب استدعاء لتوصيل أمانات نقل داخلي خاص بــ{GetBranchName} بتاريخ {Date.Now}"
            Await SendNotificationAsync(message)

            LoadAllGridData()
            FrmSavedSuccessfully.Show()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub BarButtonItem2_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Request_to_summon_driversTB_insert()
    End Sub


    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Me.Close()
    End Sub

    Private Sub FRM_Request_to_summon_drivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAllGridData()
        StartPusherAfterLogin()
    End Sub



    Public Async Sub StartPusherAfterLogin()

        If Not Me.Visible Then
            Return
        End If

        If isPusherStarted Then Return
        isPusherStarted = True

        Try
            Dim options As New PusherOptions With {.Cluster = "mt1", .Encrypted = True}
            pusher = New Pusher("0d6948b6c9f89be31a87", options)
            Await pusher.ConnectAsync()


            If Not Me.Visible Then
                Await pusher.DisconnectAsync()
                isPusherStarted = False
                Return
            End If

            Dim channel = Await pusher.SubscribeAsync("notifications")
            channel.Bind("notification.sent", Sub(evt As PusherEvent)
                                                  If Me.Visible Then
                                                      Me.Invoke(Sub() UpdateGridNewAndExisting())
                                                  End If
                                              End Sub)

        Catch ex As Exception
            MessageBox.Show("❌ حدث خطأ أثناء بدء Pusher: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub LoadAllGridData()
        Try
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
            PRM(1) = New SqlParameter("@LastUpdate", SqlDbType.DateTime) With {.Value = LastUpdate}

            Dim dt As DataTable = RUN_QUARY_PRO("Request_to_summon_driversTB_SELECT_Updated", PRM)
            GridControl1.DataSource = dt

            If dt.Rows.Count > 0 Then
                LastUpdate = Convert.ToDateTime(dt.Compute("MAX(insertDate)", String.Empty))
            End If

            DVGFormat(GridView1)
            GridView1.ShowFindPanel()
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " & ex.Message)
        End Try
    End Sub


    Private Sub UpdateGridNewAndExisting()
        Try
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
            PRM(1) = New SqlParameter("@LastUpdate", SqlDbType.DateTime) With {.Value = LastUpdate}

            Dim dt As DataTable = RUN_QUARY_PRO("Request_to_summon_driversTB_SELECT_Updated", PRM)

            For Each row As DataRow In dt.Rows
                Dim existingRowHandle As Integer = GridView1.LocateByValue("ID", row("ID"))
                If existingRowHandle >= 0 Then

                    GridView1.SetRowCellValue(existingRowHandle, "BName", row("BName"))
                    GridView1.SetRowCellValue(existingRowHandle, "UName", row("UName"))
                    GridView1.SetRowCellValue(existingRowHandle, "insertDate", row("insertDate"))
                    GridView1.SetRowCellValue(existingRowHandle, "Time", row("Time"))
                    GridView1.SetRowCellValue(existingRowHandle, "IsAccpit", row("IsAccpit"))
                Else

                    GridView1.AddNewRow()
                    Dim newRowHandle As Integer = GridView1.FocusedRowHandle
                    GridView1.SetRowCellValue(newRowHandle, "ID", row("ID"))
                    GridView1.SetRowCellValue(newRowHandle, "BName", row("BName"))
                    GridView1.SetRowCellValue(newRowHandle, "UName", row("UName"))
                    GridView1.SetRowCellValue(newRowHandle, "insertDate", row("insertDate"))
                    GridView1.SetRowCellValue(newRowHandle, "Time", row("Time"))
                    GridView1.SetRowCellValue(newRowHandle, "IsAccpit", row("IsAccpit"))
                    GridView1.UpdateCurrentRow()
                End If
            Next


            If dt.Rows.Count > 0 Then
                LastUpdate = Convert.ToDateTime(dt.Compute("MAX(insertDate)", String.Empty))
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحديث البيانات: " & ex.Message)
        End Try
    End Sub

End Class
