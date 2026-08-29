Imports System.Data.SqlClient
Imports MetroFramework

Module MD_CONNECTION
    Public NUMBER_CONNECTIN As Integer = 0
    'Public SQLCON As New SqlConnection


    'Sub New()
    '    If NUMBER_CONNECTIN = 0 Then
    '        SQLCON = New SqlConnection("Server=AHMEDA\AHMEDA;Database=ShippingTransportSystem;User Id=sa; Password=12345")
    '    ElseIf NUMBER_CONNECTIN = 1 Then
    '        SQLCON = New SqlConnection("Server=AHMEDA\AHMEDA;Database=ShippingTransportSystem;User Id=sa; Password=12345")
    '    End If
    'End Sub
    'Public Sub OPENCONNECTION()
    '    If SQLCON.State = 1 Then SQLCON.Close()
    '    Try
    '        SQLCON.ConnectionString = "Server=AHMEDA\AHMEDA;Database=ShippingTransportSystem;User Id=sa; Password=12345"
    '        ' SQlConn.ConnectionString = "server=PC\ALWANI;database=SirtEmigrant; User Id=sa; Password=theold1980@_sea"
    '        'SQlConn.ConnectionString = "Data Source=SQL5069.site4now.net;Initial Catalog=DB_A6D8EB_ahmeda78;User Id=DB_A6D8EB_ahmeda78_admin;Password=theold1980@_sea"
    '        SQLCON.Open()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "فشل في عميلة الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign)
    '        SQLCON.Close()
    '        End
    '    End Try
    'End Sub
    'Public Sub OPEN_CON()
    '    If SQLCON.State = ConnectionState.Closed Then
    '        SQLCON.Open()
    '    End If
    'End Sub
    'Public Sub CLOSE_CON()
    '    If SQLCON.State = ConnectionState.Open Then
    '        SQLCON.Close()
    '    End If
    'End Sub

    'Public Function RUN_QUARY_TXT(ByVal QUARY As String) As DataTable
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    Dim DA As New SqlDataAdapter(QUARY, SQLCON)
    '    DA.Fill(DT)
    '    Return DT
    'End Function
    'Public Function RUN_QUARY_Field(ByVal QUARY As String, x As Integer) As DataTable
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    Dim DA As New SqlDataAdapter(QUARY, SQLCON)
    '    DA.Fill(DT)
    '    Return DT
    'End Function

    'Public Function RUN_QUARY_PRO(ByVal QUARY As String, ByVal PRM() As SqlParameter) As DataTable
    '    Dim CMD As New SqlCommand
    '    CMD.CommandType = CommandType.StoredProcedure
    '    CMD.CommandText = QUARY
    '    CMD.Connection = SQLCON
    '    OPENCONNECTION()
    '    For I As Integer = 0 To PRM.Length - 1
    '        CMD.Parameters.Add(PRM(I))
    '    Next
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    Dim DA As New SqlDataAdapter(CMD)
    '    DA.Fill(DT)
    '    Return DT
    'End Function
    'Public Sub RUN_EXUTE_TXT(ByVal QUARY As String)
    '    Dim CMD As New SqlCommand
    '    CMD.CommandType = CommandType.Text
    '    CMD.CommandText = QUARY
    '    CMD.Connection = SQLCON
    '    OPENCONNECTION()
    '    CMD.ExecuteNonQuery()
    '    'CLOSE_CON()
    'End Sub

    'Public Sub RUN_EXUTE_PRO(ByVal QUARY As String, ByVal PRM() As SqlParameter)
    '    Dim CMD As New SqlCommand
    '    CMD.CommandType = CommandType.StoredProcedure
    '    CMD.CommandText = QUARY
    '    CMD.Connection = SQLCON
    '    OPENCONNECTION()
    '    For I As Integer = 0 To PRM.Length - 1
    '        CMD.Parameters.Add(PRM(I))
    '    Next
    '    CMD.ExecuteNonQuery()
    '    ' CLOSE_CON()
    'End Sub
    'Public Sub RUN_FUN_PRO(ByVal QUARY As String, ByVal PRM() As SqlParameter)
    '    Dim CMD As New SqlCommand
    '    CMD.CommandType = CommandType.StoredProcedure
    '    CMD.CommandText = QUARY
    '    CMD.Connection = SQLCON
    '    OPENCONNECTION()
    '    For I As Integer = 0 To PRM.Length - 1
    '        CMD.Parameters.Add(PRM(I))
    '    Next
    '    CMD.ExecuteNonQuery()
    '    ' CLOSE_CON()
    'End Sub
    'Public Function RUN_FUNCTION_PARM(ByVal QUARY As String, ByVal PRM() As SqlParameter) As DataTable

    '    Dim CMD As New SqlCommand
    '    CMD.CommandType = CommandType.Text
    '    CMD.CommandText = "select dbo." & QUARY
    '    CMD.Connection = SQLCON
    '    OPENCONNECTION()
    '    For I As Integer = 0 To PRM.Length - 1
    '        CMD.Parameters.Add(PRM(I))
    '    Next
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    Dim ADP As New SqlDataAdapter(CMD)
    '    ADP.Fill(DT)
    '    Return DT
    '    'CLOSE_CON()

    'End Function

    'Public Function GETMAXID(TableName, OrderByField) As Integer
    '    GETMAXID = 0
    '    Dim str = ("select * from " & TableName & " Order by " & OrderByField)
    '    Dim adp = New SqlClient.SqlDataAdapter(str, SQLCON)
    '    Dim ds = New DataSet
    '    adp.Fill(ds)
    '    Dim dt As DataTable
    '    dt = ds.Tables(0)
    '    If dt.Rows.Count <> 0 Then
    '        Dim i = dt.Rows.Count - 1
    '        GETMAXID = Val(dt.Rows(i).Item(0))
    '    End If
    'End Function
    'Public Function IsNumber(ByVal KCode As String, frm As Form) As Boolean
    '    If Not IsNumeric(KCode) And KCode <> ChrW(Keys.Back) And KCode <> ChrW(Keys.Enter) And KCode <> "."c Then
    '        MetroMessageBox.Show(frm, "يرجى كتابة رقم فقط", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End If
    '    Return True
    'End Function
End Module
