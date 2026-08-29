Imports System.Data.SqlClient

Public Class ChatterData

    Private Shared m_ConnectionString As String = "Server=.;Database=Chatter;User ID=ChatterLogin;pwd=chat"
        Private m_sqlConn As SqlConnection = Nothing
        Public Delegate Sub NewMessage()
        Public Event OnNewMessage As NewMessage

        Public Sub New()
            SqlDependency.[Stop](m_ConnectionString)
            SqlDependency.Start(m_ConnectionString)
            m_sqlConn = New SqlConnection(m_ConnectionString)
        End Sub

        Protected Overrides Sub Finalize()
            SqlDependency.[Stop](m_ConnectionString)
        End Sub

        Public Function GetMessages() As DataTable
            Dim dt As DataTable = New DataTable()

            Try
            Dim cmd As SqlCommand = New SqlCommand("ExchangeSystem_FastAnalyz", SQLCON)
            cmd.CommandType = CommandType.StoredProcedure
                cmd.Notification = Nothing
                Dim dependency As SqlDependency = New SqlDependency(cmd)
                AddHandler dependency.OnChange, New OnChangeEventHandler(AddressOf OnChange)
            dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection))
        Catch ex As Exception
                Throw ex
            End Try

            Return dt
        End Function

    Private Sub OnChange(ByVal sender As Object, ByVal e As SqlNotificationEventArgs)
            Dim dependency As SqlDependency = TryCast(sender, SqlDependency)
            RemoveHandler dependency.OnChange, AddressOf OnChange
            RaiseEvent OnNewMessage()
        End Sub

    Public Shared Sub AddMessage()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        prm(0).Value = BID

        Dim DT As New DataTable
        DT.Clear()
        DT.Clear()
        DT = RUN_QUARY_PRO("ExchangeSystem_FastAnalyz", prm)
        If DT.Rows.Count > 0 Then
            FRMMAIN.Timer1.Start()
            FRMMAIN.OutComeNotDelivered.Text = DT.Rows(0)("ReCount")
            FRMMAIN.OutComeDelivered.Text = DT.Rows(0)("ReCountNotDel")
            FRMMAIN.InNotConfirmed.Text = DT.Rows(0)("IntNotConfirmed")
            FRMMAIN.IntIncomeNotDel.Text = DT.Rows(0)("IntIncomeNotDel")
            'FRMMAIN.ConfirmInternalEx.Text = DT.Rows(0)("NeedToConfirm")
            'FRMMAIN.Timer1.Interval = DT.Rows.Count
        End If


    End Sub

    'Public Shared Function GetUsers() As DataTable
    '    Dim Conn As SqlConnection = New SqlConnection()
    '    Dim cmd As SqlCommand = New SqlCommand("SELECT ID, Name FROM dbo.Person", SQLCON)
    '    Dim dt As DataTable = New DataTable()
    '        Conn.Open()

    '        Try
    '            dt.Load(cmd.ExecuteReader())
    '        Finally
    '            Conn.Close()
    '        End Try

    '        Return dt
    '    End Function
End Class
