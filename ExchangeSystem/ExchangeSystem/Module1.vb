Imports DevExpress.LookAndFeel
Imports DevExpress.Skins
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Data.SqlClient

Public Class MyGridLocalizer
    Inherits GridLocalizer
    Public Overrides Function GetLocalizedString(ByVal id As GridStringId) As String
        Select Case id
            Case GridStringId.FindControlFindButton
                Return "البحث"
            Case GridStringId.FindControlClearButton
                Return "مسح"
            Case Else
                Return MyBase.GetLocalizedString(id)
        End Select
    End Function
End Class

Public Module Module1
    'Public CultureInfo As CultureInfo = CultureInfo.CreateSpecificCulture("ar")
    Public SQLCON, QSCON As New SqlConnection
    Public opx, poss, log_date, log_time, user, h1, h2, h3, hh, hhh As String
    Public tim, x As New Date
    Public IsLimited As Boolean
    Public BrRate, WithMainBrRate, withAgentRate, WithMediumRate, LimitedVal As Decimal
    Public UserID, GProfIDLog, BID, MAINBID, COUNTRYNID, CITYID, UserType, MAINCountryID, DefaultCurrency As Integer
    Public UserAccID As ULong
    Public UserLogName, GetUserName, GetBranchName, BRKey, BGID, CNNAME, CTNAME, UserPass, UserPhone As String
    Public WithEvents sqlDependency As SqlDependency

    ' ---------------------------------------------------------
    ' 1. بيانات الربط الخاصة بسيرفر wa.rhalla.online
    ' ---------------------------------------------------------
    'Public session_id As String = "91605fd0-e90c-4e14-89bd-3d802d49ad08"
    'Public apiKey As String = "owa_k1_7c2dd55e99a11e97aef495d122ceba8e150e4942f450269a6a85ba1c020fda18"
    'Public apiUrl As String = $"https://wa.rhalla.online/api/sessions/{session_id}/messages/send-text"
    Public session_id As String
    Public apiKey As String
    Public apiUrl As String 
    Public Sub AdjustFormSize(FRM As XtraForm)
        Dim screenWidth As Integer = Screen.PrimaryScreen.WorkingArea.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.WorkingArea.Height

        FRM.Width = screenWidth * 0.8 ' Adjust to 80% of screen width
        FRM.Height = screenHeight * 0.8 ' Adjust to 80% of screen height

    End Sub
    Public Sub SizeControls(FRM As XtraForm)
        Dim scaleX As Double = Screen.PrimaryScreen.WorkingArea.Width / 1920.0 ' Base resolution
        Dim scaleY As Double = Screen.PrimaryScreen.WorkingArea.Height / 1080.0 ' Base resolution

        For Each ctrl As Control In FRM.Controls
            ctrl.Width = CInt(ctrl.Width * scaleX)
            ctrl.Height = CInt(ctrl.Height * scaleY)
            ctrl.Left = CInt(ctrl.Left * scaleX)
            ctrl.Top = CInt(ctrl.Top * scaleY)
        Next
    End Sub

    Public Sub FormLocation(FRM As XtraForm)
        If FRM.WindowState = FormWindowState.Maximized Then
            Dim pd As New Padding
            pd.Left = FRM.Padding.Left
            pd.Right = FRM.Padding.Right
            pd.Top = FRM.Padding.Top
            pd.Bottom = FRM.Height - Screen.PrimaryScreen.WorkingArea.Height
            FRM.Padding = pd
        End If
    End Sub
    ' Built-in fallback for the SQL Server path: the LOCAL SQL Server Express instance on this machine.
    ' ".\SQLEXPRESS" rather than a machine name so the same build runs on any workstation with the standard
    ' instance, and Integrated Security so no password is compiled into the binary. Used only when
    ' RhallaConfig.ini supplies no SQLSERVER_CONN.
    Public Const LOCAL_SQLSERVER_CONN As String = "Data Source=.\SQLEXPRESS;Initial Catalog=EXCHANGESYS2026;Integrated Security=True;Connect Timeout=30;Pooling=True;Max Pool Size=2024"

    Public Sub OPENCONNECTION()
        ' MySQL path: every helper below routes to MD_MYSQL, which opens its own MySqlConnection per call.
        ' Opening (or even touching) the SQL Server connection here would be pointless and would hard-fail
        ' the app with "فشل في عميلة الاتصال" on machines that have no SQL Server at all.
        If MD_MYSQL.USE_MYSQL Then
            FRMMAIN.BarButtonItem130.Caption = "إصدار البرنامج: " & Application.ProductVersion + Space(1) + "MySQL"
            Exit Sub
        End If

        If SQLCON.State = 1 Then SQLCON.Close()
        Try
            ''SQLCON.ConnectionString = "Data Source=93.158.238.134,1433\MSSQLSERVER;Initial Catalog = EXCHANGESYS2026; Persist Security Info=True;User ID = new_admin; Password=theartof1980@_coding;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            ''FRMMAIN.BarButtonItem130.Caption = "صرافة 2026"

            'SQLCON.ConnectionString = "Data Source=15.160.199.233;Initial Catalog = EXCHANGESYS2026; Persist Security Info=True;User ID = sa; Password=Tk27x35Gvii92GrLy;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'FRMMAIN.BarButtonItem130.Caption = "إصدار البرنامج: " & Application.ProductVersion


            'SQLCON.ConnectionString = "Data Source=148.251.245.41;Initial Catalog = EXCHANGESYS2026; Persist Security Info=True;User ID = sa; Password=1fURhallayQV684@;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'FRMMAIN.BarButtonItem130.Caption = "إصدار البرنامج: " & Application.ProductVersion + Space(1) + "هاينز"

            ''-----------------السيرفر المحلي SQLEXPRESS-----------------
            ' The active server is NO LONGER chosen by commenting lines in and out. It comes from
            ' RhallaConfig.ini (SQLSERVER_CONN) next to the exe, so changing server needs no rebuild and no
            ' credential lives in source. With the key absent or blank - the shipped default - the app
            ' connects to the LOCAL SQL Server Express instance (LOCAL_SQLSERVER_CONN above). The servers
            ' listed below are kept as a record of the known environments; paste one into SQLSERVER_CONN.
            Dim CS As String = MD_SECRETS.SqlServerConn
            Dim SRVLABEL As String
            If CS Is Nothing OrElse CS.Trim().Length = 0 Then
                CS = LOCAL_SQLSERVER_CONN
                SRVLABEL = "محلي"
            Else
                SRVLABEL = "SQL Server"
            End If
            SQLCON.ConnectionString = CS
            FRMMAIN.BarButtonItem130.Caption = "إصدار البرنامج: " & Application.ProductVersion + Space(1) + SRVLABEL

            'SQLCON.ConnectionString = "Data Source=102.214.165.242,55910;Initial Catalog = EXCHANGESYS2026; Persist Security Info=True;User ID = sa; Password=123456789;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'FRMMAIN.BarButtonItem130.Caption = " 2026 تجريب منظومة الي فووق داتا سنتر"
            'SQLCON.ConnectionString = "Data Source=192.168.0.118,4022;Initial Catalog = EXCHANGESYS; Persist Security Info=True;User ID = sa; Password=123456;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'FRMMAIN.BarButtonItem130.Caption = "صرافة 2026"
            'SQLCON.ConnectionString = "Data Source=R-MOKAWALAT;Initial Catalog = EXCHANGESYS; Persist Security Info=True;User ID = sa; Password=123456; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'SQLCON.ConnectionString = "server=102.214.165.242,1600\AAS;Database=EXCHANGESYS;User Id=new_admin; Password=123456"
            'FRMMAIN.BarButtonItem130.Caption = "كمبيوتر شخصي"
            'SQLCON.ConnectionString = "Data Source=192.168.0.118,4022;Initial Catalog = EXCHANGESYS; Persist Security Info=True;User ID = sa; Password=123456;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'FRMMAIN.BarButtonItem130.Caption = "تجرب خدمات مصرفية"
            'SQLCON.ConnectionString = "Data Source=SRV4EC7E3AD9;Initial Catalog = EXCHANGESYS; Persist Security Info=True;User ID = sa; Password=123456;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'SQLCON.ConnectionString = "server=ALHALLA;Database=ExSyAccounts;User Id=sa; Password=123456"
            'SQLCON.ConnectionString = "server=R-MOKAWALAT\ALRHALLA;Database=ShippingTransportSystem;User Id=sa; Password=123456"
            'SQLCON.ConnectionString = "server=102.214.165.242, 4022\MD;Database=EXCHANGESYS;User Id=sa; Password=123456"
            'SQLCON.ConnectionString = "server=192.168.0.134;Database=rhalla2023;User Id=wesam; Password=123456789"
            'SQLCON.ConnectionString = "server=BASIC\PC;Database=EXCHANGESYS;User Id=AHMED; Password=123456"
            'SQLCON.ConnectionString = "server=HH\MM;Database=EXCHANGESYS;User Id=sa; Password=123456"
            'SQLCON.ConnectionString = "server=93.158.237.25;Database=EXCHANGESYS;User Id=sa; Password=theartof1980@_coding"
            ''-----------------السيرفر الرئيسي-----------------
            'SQLCON.ConnectionString = "server=93.158.238.134;Database=EXCHANGESYS;User Id=sa; Password=theartof1980@_coding"
            ''-----------------السيرفر الرئيسي تيست-----------------
            'SQLCON.ConnectionString = "server=93.158.238.134;Database=EXCHANGESYS_TEST;User Id=sa; Password=theartof1980@_coding"
            'SQLCON.ConnectionString = "server=93.158.238.134;Database=EXCHANGESYS_TEST_2;User Id=sa; Password=theartof1980@_coding"
            ''-----------------السيرفر المحلي-----------------
            'SQLCON.ConnectionString = "server=BASIC\PC;Database=EXCHANGESYS_TEST;User Id=AHMED; Password=123456"
            'SQLCON.ConnectionString = "server=BASIC\PC;Database=EXCHANGESYS_TEST_2;User Id=AHMED; Password=123456"

            'SQLCON.ConnectionString = "Data Source=102.214.165.242,55910;Initial Catalog = EXCHANGESYS; Persist Security Info=True;User ID = sa; Password=123456789;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024"
            'FRMMAIN.BarButtonItem130.Caption = "سيرفير التطبيق"
            SQLCON.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "فشل في عميلة الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign)
            SQLCON.Close()
            End
        End Try
    End Sub

    Sub LogFunc(st1 As String, st2 As String, st3 As String, st4 As String, st5 As String)
        ''conn = New OleDbConnection(SQlConn)
        ''con.Open()
        'Dim str = "select * from log_user"
        'Dim adp = New SqlClient.SqlDataAdapter(str, SQlConn)
        'Dim ds = New DataSet
        'adp.Fill(ds)
        'Dim dt As DataTable
        'dt = ds.Tables(0)
        'Dim dr = dt.NewRow
        'dr!username = FrmLogin.user_name.Text
        'dr!oprtype = st1
        'dr!cust_num = st2
        'dr!dateop = System.DateTime.Now
        'dt.Rows.Add(dr)
        'Dim cmd As New SqlClient.SqlCommandBuilder(adp)
        'adp.Update(dt)
    End Sub
    Function GET_LAST_RECORD(TableName, OrderByField) As Integer
        GET_LAST_RECORD = 0
        Dim str = ("select ID from " & TableName & " Order by " & OrderByField)
        ' Must follow USE_MYSQL: this reads the next ID straight off the global SqlConnection. Left on the
        ' SQL Server path while writes go to MariaDB it would hand back a stale ID -> duplicate-key on insert.
        If MD_MYSQL.USE_MYSQL Then
            Dim dtm As DataTable = MD_MYSQL.RUN_QUARY_TXT_MY(str)
            If dtm IsNot Nothing AndAlso dtm.Rows.Count <> 0 Then
                GET_LAST_RECORD = Val(dtm.Rows(dtm.Rows.Count - 1).Item(0))
            End If
            Return GET_LAST_RECORD
        End If
        Dim adp = New SqlClient.SqlDataAdapter(str, SQLCON)
        Dim ds = New DataSet
        adp.Fill(ds)
        Dim dt As DataTable
        dt = ds.Tables(0)
        If dt.Rows.Count <> 0 Then
            Dim i = dt.Rows.Count - 1
            GET_LAST_RECORD = Val(dt.Rows(i).Item(0))
        End If
    End Function
    Public Sub ClearFrmTool(frm As Form)
        For Each ctrl As Control In frm.Controls
            If TypeOf ctrl Is TextBox Then
                ctrl.Text = String.Empty
            End If
        Next
    End Sub
    Sub ClearDTB(frm As Form)
        For Each dtb As Control In frm.Controls
            If TypeOf dtb Is DateTimePicker Then
                dtb.Text = Date.Now
            End If
        Next
    End Sub
    Sub FillListBox(Lsbo As ListBox, Tablename As String, DisplayValue As String)
        Lsbo.Items.Clear()
        Dim sql = String.Empty
        sql = "select * from " & Tablename & " order by " & DisplayValue
        If MD_MYSQL.USE_MYSQL Then
            Dim dtm As DataTable = MD_MYSQL.RUN_QUARY_TXT_MY(sql)
            If dtm IsNot Nothing Then
                For I = 0 To dtm.Rows.Count - 1
                    Lsbo.Items.Add(dtm.Rows(I).Item(DisplayValue))
                Next
            End If
            Exit Sub
        End If
        Dim adp As New SqlClient.SqlDataAdapter(sql, SQLCON)
        Dim ds As New DataSet
        adp.Fill(ds)
        Dim dt = ds.Tables(0)
        '====================================================
        For I = 0 To dt.Rows.Count - 1
            Lsbo.Items.Add(dt.Rows(I).Item(DisplayValue))
        Next
    End Sub
    Function Gender(ByVal NID As String) As String
        Dim cod As Integer
        cod = Mid(NID, 1, 2)
        If cod Mod 2 = 0 Then
            Gender = "أنثى"
        Else
            Gender = "ذكر"
        End If
    End Function
    Public Function RUN_QUARY_TXT(ByVal QUARY As String) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_TXT_MY(QUARY)
        Dim DT As New DataTable
        DT.Clear()
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If
        Dim DA As New SqlDataAdapter(QUARY, SQLCON)
        DA.Fill(DT)
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        DA.Dispose()
        Return DT

    End Function
    Public Function RUN_TXT(ByVal QUARY As String) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_TXT_MY(QUARY)
        Dim DT As New DataTable
        DT.Clear()
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If
        Dim DA As New SqlDataAdapter(QUARY, SQLCON)
        DA.Fill(DT)
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        DA.Dispose()

        Return DT

    End Function
    Public Function RUN_QUARY_Field(ByVal QUARY As String, x As Integer) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_TXT_MY(QUARY)
        Dim DT As New DataTable
        DT.Clear()
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If
        Dim DA As New SqlDataAdapter(QUARY, SQLCON)
        DA.Fill(DT)
        Return DT
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        DA.Dispose()

    End Function
    Public Function RUN_QUARY_PRO(ByVal QUARY As String, ByVal PRM() As SqlParameter) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_PRO_MY(QUARY, PRM)

        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.StoredProcedure
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        For I As Integer = 0 To PRM.Length - 1
            CMD.Parameters.Add(PRM(I))
        Next
        Dim DT As New DataTable
        DT.Clear()
        Dim DA As New SqlDataAdapter(CMD)
        DA.Fill(DT)
        DA.Dispose()
        CMD.Dispose()
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If

        Return DT

    End Function
    Public Function RUN_QUARY_PRO_alter(ByVal QUARY As String, ByVal PRM() As SqlParameter) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_PRO_MY(QUARY, PRM)

        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.StoredProcedure
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If
        If PRM Is Nothing Then

        Else
            CMD.Parameters.AddRange(PRM)
        End If

        Dim DT As New DataTable
        DT.Clear()
        Dim DA As New SqlDataAdapter(CMD)
        DA.Fill(DT)
        DA.Dispose()
        CMD.Dispose()
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If

        Return DT

    End Function




    Public Function RUN_QUARY_PRO_ONLY(ByVal QUARY As String) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_PRO_ONLY_MY(QUARY)
        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.StoredProcedure
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        Dim DT As New DataTable
        DT.Clear()
        Dim DA As New SqlDataAdapter(CMD)
        DA.Fill(DT)
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        DA.Dispose()
        Return DT

    End Function
    Public Function RUN_QUARY_QUERY_ONLY(ByVal QUARY As String) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_QUARY_TXT_MY(QUARY)
        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.Text
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        Dim DT As New DataTable
        DT.Clear()
        Dim DA As New SqlDataAdapter(CMD)
        DA.Fill(DT)
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        DA.Dispose()
        Return DT

    End Function
    Public Sub RUN_EXUTE_TXT(ByVal QUARY As String)
        If MD_MYSQL.USE_MYSQL Then
            MD_MYSQL.RUN_EXUTE_TXT_MY(QUARY)
            refresh_table(BID)      ' keep the SQL Server path's side effect
            Exit Sub
        End If
        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.Text
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        CMD.ExecuteNonQuery()
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        CMD.Dispose()
        refresh_table(BID)
    End Sub
    Public Sub RUN_EXUTE_PRO(ByVal QUARY As String, ByVal PRM() As SqlParameter)
        If MD_MYSQL.USE_MYSQL Then
            MD_MYSQL.RUN_EXUTE_PRO_MY(QUARY, PRM)
            Exit Sub
        End If
        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.StoredProcedure
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        For I As Integer = 0 To PRM.Length - 1
            CMD.Parameters.Add(PRM(I))
        Next
        CMD.ExecuteNonQuery()
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        CMD.Dispose()

    End Sub
    Public Sub RUN_FUN_PRO(ByVal QUARY As String, ByVal PRM() As SqlParameter)
        If MD_MYSQL.USE_MYSQL Then
            MD_MYSQL.RUN_EXUTE_PRO_MY(QUARY, PRM)
            Exit Sub
        End If
        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.StoredProcedure
        CMD.CommandText = QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        For I As Integer = 0 To PRM.Length - 1
            CMD.Parameters.Add(PRM(I))
        Next
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If

        CMD.ExecuteNonQuery()
        CMD.Dispose()

    End Sub
    Public Function RUN_FUNCTION_PARM(ByVal QUARY As String, ByVal PRM() As SqlParameter) As DataTable
        If MD_MYSQL.USE_MYSQL Then Return MD_MYSQL.RUN_FUNCTION_PARM_MY(QUARY, PRM)
        Dim CMD As New SqlCommand
        CMD.CommandType = CommandType.Text
        CMD.CommandText = "select dbo." & QUARY
        CMD.Connection = SQLCON
        OpenConnection()
        For I As Integer = 0 To PRM.Length - 1
            CMD.Parameters.Add(PRM(I))
        Next
        Dim DT As New DataTable
        DT.Clear()
        Dim ADP As New SqlDataAdapter(CMD)
        ADP.Fill(DT)
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        CMD.Dispose()
        Return DT

    End Function
    Public Sub RatesBetweenBranchesTB_Insert(InsertDate As Date, ISID As String, BranchRecievedID As Integer, SafeRecievedID As Integer, IsShare As Double)
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = ISID}
        PRM(2) = New SqlParameter("@BranchRecievedID", SqlDbType.Int) With {.Value = BranchRecievedID}
        PRM(3) = New SqlParameter("@SafeRecievedID", SqlDbType.Int) With {.Value = SafeRecievedID}
        PRM(4) = New SqlParameter("@IsShare", SqlDbType.Decimal) With {.Value = IsShare}
        RUN_EXUTE_PRO("RatesBetweenBranchesTB_Insert", PRM)

    End Sub
    Public Function GETIDMAX(TableName, OrderByField) As Integer
        GETIDMAX = 0
        Dim str = ("select ID from " & TableName & " Order by " & OrderByField)
        If MD_MYSQL.USE_MYSQL Then
            Dim dtm As DataTable = MD_MYSQL.RUN_QUARY_TXT_MY(str)
            If dtm IsNot Nothing AndAlso dtm.Rows.Count <> 0 Then
                GETIDMAX = Val(dtm.Rows(dtm.Rows.Count - 1).Item(0))
            End If
            Return GETIDMAX
        End If
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If

        Dim adp = New SqlDataAdapter(str, SQLCON)
        Dim ds = New DataSet
        adp.Fill(ds)
        Dim dt As DataTable
        dt = ds.Tables(0)
        If dt.Rows.Count <> 0 Then
            Dim i = dt.Rows.Count - 1
            GETIDMAX = Val(dt.Rows(i).Item(0))
        End If
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        ds.Dispose()
        adp.Dispose()
    End Function
    Public Function GETIDMAX_Pro(TableName As String, FieldName As String, Optional TypeID As Integer = 0, Optional TypeIDFild As String = "TypeID") As Long

        Dim result As Long = 0
        Dim str As String

        If TypeID = 0 Then
            str = "SELECT ISNULL(MAX(" & FieldName & "), 0) FROM " & TableName
        Else
            str = "SELECT ISNULL(MAX(" & FieldName & "), 0) FROM " & TableName & " WHERE " & TypeIDFild & " = @TypeID"
        End If

        If MD_MYSQL.USE_MYSQL Then
            ' MySQL has no ISNULL() -> IFNULL(). Same shape, same result.
            Dim mysql As String = str.Replace("ISNULL(", "IFNULL(")
            If TypeID = 0 Then Return MD_MYSQL.SCALARLONG_MY(mysql, Nothing)
            Dim pr(0) As SqlParameter
            pr(0) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
            Return MD_MYSQL.SCALARLONG_MY(mysql, pr)
        End If

        Using cmd As New SqlCommand(str, SQLCON)
            If TypeID <> 0 Then
                cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = TypeID
            End If

            If SQLCON.State <> ConnectionState.Open Then
                SQLCON.Open()
            End If

            Dim obj = cmd.ExecuteScalar()

            If obj IsNot Nothing AndAlso Not IsDBNull(obj) Then
                result = Convert.ToInt64(obj)
            Else
                result = 0
            End If
        End Using

        If SQLCON.State = ConnectionState.Open Then SQLCON.Close()

        Return result

    End Function


    Public Function GETMAXID(TableName, OrderByField) As Integer
        GETMAXID = 0
        Dim str = ("select * from " & TableName & " Order by " & OrderByField)
        If MD_MYSQL.USE_MYSQL Then
            Dim dtm As DataTable = MD_MYSQL.RUN_QUARY_TXT_MY(str)
            If dtm IsNot Nothing AndAlso dtm.Rows.Count <> 0 Then
                GETMAXID = Val(dtm.Rows(dtm.Rows.Count - 1).Item(0))
            End If
            Return GETMAXID
        End If
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If

        Dim adp = New SqlDataAdapter(str, SQLCON)
        Dim ds = New DataSet
        adp.Fill(ds)
        Dim dt As DataTable
        dt = ds.Tables(0)
        If dt.Rows.Count <> 0 Then
            Dim i = dt.Rows.Count - 1
            GETMAXID = Val(dt.Rows(i).Item(0))
        End If
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        ds.Dispose()
        adp.Dispose()
    End Function
    Public Function GETUSERMAXID(TableName, OrderByField) As Integer
        GETUSERMAXID = 0
        Dim str = ("select USID from " & TableName & " Order by " & OrderByField)
        If MD_MYSQL.USE_MYSQL Then
            Dim dtm As DataTable = MD_MYSQL.RUN_QUARY_TXT_MY(str)
            If dtm IsNot Nothing AndAlso dtm.Rows.Count <> 0 Then
                GETUSERMAXID = Val(dtm.Rows(dtm.Rows.Count - 1).Item(0))
            End If
            Return GETUSERMAXID
        End If
        Dim adp = New SqlDataAdapter(str, SQLCON)
        If SQLCON.State = ConnectionState.Closed Then
            SQLCON.Open()
        End If

        Dim ds = New DataSet
        adp.Fill(ds)
        Dim dt As DataTable
        dt = ds.Tables(0)
        If dt.Rows.Count <> 0 Then
            Dim i = dt.Rows.Count - 1
            GETUSERMAXID = Val(dt.Rows(i).Item(0))
        End If
        If SQLCON.State = ConnectionState.Open Then
            SQLCON.Close()
        End If
        ds.Dispose()
        adp.Dispose()

    End Function
    Public Function CHECKUSERHASPETTYCASH(USERID As Integer) As DataTable
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = USERID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("USERCHECK_PETTYCASHACCIDHASVAL", PR)
        If dt.Rows.Count > 0 Then

        End If
        Return dt
    End Function
    Public Function CHECKEMPHASPETTYCASH(EMPID As Integer) As DataTable
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("EMPCHECK_PETTYCASHACCIDHASVAL", PR)
        If dt.Rows.Count > 0 Then

        End If
        Return dt
    End Function
    Public Function IsNumber(ByVal KCode As String, frm As Form) As Boolean
        If Not IsNumeric(KCode) And KCode <> ChrW(Keys.Back) And KCode <> ChrW(Keys.Enter) And KCode <> "."c Then
            ErrorMessage(frm, "رسالة خطأ", "يرجى كتابة رقم فقط")
        End If
        Return True

    End Function
    Public Function CHECKOPERATIONS_FalseOrTrue(FormID As Integer, ProfileGID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@FormID", SqlDbType.Int)
        PRM(0).Value = FormID
        PRM(1) = New SqlParameter("@ProfileGID", SqlDbType.Int)
        PRM(1).Value = ProfileGID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CHECKOPERATIONS_FalseorTrue", PRM)
        Return DT
    End Function
    Public Function CHECKBUTTON_TRUEORFALSE(FormID As Integer, UserID As Integer, ProfileID As Integer) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@FormID", SqlDbType.Int) With {.Value = FormID}
        PRM(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
        PRM(2) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfileID}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CHECKBUTTON_TRUEORFALSE", PRM)
        Return DT
    End Function
    Public Function CHECKOPERATIONS_FalseTrue(ScreenID As Integer, userid As Integer, profileid As Integer, btn As BarItem) As DataTable
        'BarButtonItem
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        PRM(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = userid}
        PRM(2) = New SqlParameter("@ProfileGID", SqlDbType.Int) With {.Value = profileid}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SHOWBRANCHSCREENS_ROLE", PRM)
        If DT.Rows.Count > 0 Then
            If TypeOf btn Is BarButtonItem Or TypeOf btn Is BarSubItem Then
                If btn.Visibility = DT.Rows(0)("CanShow") = True Then btn.Visibility = BarItemVisibility.Never
            End If
        End If
        Return DT
    End Function
    Public Function CHECKRIBBONPAGE_FalseTrue(ScreenID As Integer, userid As Integer, profileid As Integer, btn As RibbonPage) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        PRM(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = userid}
        PRM(2) = New SqlParameter("@ProfileGID", SqlDbType.Int) With {.Value = profileid}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SHOWBRANCHSCREENS_ROLE", PRM)
        If DT.Rows.Count > 0 Then
            btn.Visible = DT.Rows(0)("IsShow")
        End If
        Return DT
    End Function
    Public Function CHECKOFORMVISIBEL_FalseOrTrue(ProfileGID As Integer, USRID As Integer, ScreenID As Integer, ByVal BTN As SimpleButton) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileGID", SqlDbType.Int)
        PRM(0).Value = ProfileGID
        PRM(1) = New SqlParameter("@USERID", SqlDbType.Int)
        PRM(1).Value = USRID
        PRM(2) = New SqlParameter("@ScreenID", SqlDbType.Int)
        PRM(2).Value = ScreenID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SHOWBRANCHSCREENS_ROLE", PRM)
        If DT.Rows.Count > 0 Then
            BTN.Visible = DT.Rows(0)("CanShow")
        End If
        Return DT

    End Function
    Public Function CHECKOFORMVISIBEL_FORSCREENS(ProfileGID As Integer, USRID As Integer, ScreenID As Integer) As DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@ProfileGID", SqlDbType.Int)
        PRM(0).Value = ProfileGID
        PRM(1) = New SqlParameter("@USERID", SqlDbType.Int)
        PRM(1).Value = USRID
        PRM(2) = New SqlParameter("@ScreenID", SqlDbType.Int)
        PRM(2).Value = ScreenID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("SHOWBRANCHSCREENS_ROLE", PRM)
        Return DT

    End Function
#Region "DVG"
    Public Sub DVGFormat(GVRole As GridView)
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = False
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Public Sub ADDCOLUMN(GVRole As GridView)
        Dim colCounter As GridColumn
        colCounter = GVRole.Columns.AddVisible("RowHandle")
        colCounter.Caption = "#"
        colCounter.VisibleIndex = 0
        colCounter.Width = 50
        colCounter.UnboundType = DevExpress.Data.UnboundColumnType.Integer
        colCounter.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
        GVRole.Columns("RowHandle").AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        GVRole.Columns("RowHandle").AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        GVRole.Columns("RowHandle").AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        GVRole.Columns("RowHandle").AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
    End Sub
#End Region
    Public Sub CHECKOPENFORMORNOT(FRM As XtraForm, FRM2 As XtraForm)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_TransferPermission", PR)
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("IsConfirmed") = 3 And DT.Rows(0)("IsCanceled") = 2 And DT.Rows(0)("IsConfirmCancel") = 4 And DT.Rows(0)("IsCanceledRequest") = 3 And DT.Rows(0)("ConfirmCanceled") = 1 Then
                ErrorMessage(FRM, "رسالة معلومات", "لديك حوالة داخلية تم إلغاؤها ويجب اتمام عملية الترجيع")
                FRMRETRUNINTERNALEX.InternalExCH.Checked = True
                FRMRETRUNINTERNALEX.ExternalExCH.Checked = False
                FRMRETRUNINTERNALEX.ShowDialog()
            ElseIf DT.Rows(0)("IsConfirmed") = 6 And DT.Rows(0)("IsCanceled") = 0 And DT.Rows(0)("IsCanceledRequest") = 5 And DT.Rows(0)("IsConfirmCancel") = 6 And DT.Rows(0)("ConfirmCanceled") = 3 Then
                ErrorMessage(FRM, "رسالة معلومات", "لديك حوالة داخلية تم رفض إلغاؤها ويجب اتمام عملية التسليم")
                FRMRETRUNINTERNALEX.InternalExCH.Checked = True
                FRMRETRUNINTERNALEX.ExternalExCH.Checked = False
                FrmInternalExDeliveredAfterConfirmCancel.ShowDialog()
            ElseIf DT.Rows(0)("IsConfirmed") = 2 And DT.Rows(0)("IsCanceled") = 1 And DT.Rows(0)("IsConfirmCancel") = 1 And DT.Rows(0)("IsCanceledRequest") = 1 And DT.Rows(0)("ConfirmCanceled") = 1 Then
                ErrorMessage(FRM, "رسالة معلومات", "لديك حوالة داخلية تم إلغاؤها من المرسل وسيتم نقلك لشاشة الحوالات الملغية لاىخاذ الإجراء")
                FrmViewCanceledTransfer.ShowDialog()
            End If
        Else
            FRM2.ShowDialog()
        End If

    End Sub
    Public Sub refresh_table(BranchID As Integer)
        Try
            OpenConnection()
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PRM(0).Value = BranchID
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ExchangeSystem_Analyz", PRM)
            If dt.Rows.Count > 0 Then
                FRMMAIN.OutComeNotDelivered.Text = dt.Rows(0)("ReCount")
                FRMMAIN.ConfirmInternalEx.Text = dt.Rows(0)("ConfrimCount")
                FRMMAIN.OutComeDelivered.Text = dt.Rows(0)("ReCountNotDel")
                FRMMAIN.IntIncomeNotDel.Text = dt.Rows(0)("IntIncomeNotDel")
                FRMMAIN.InNotConfirmed.Text = dt.Rows(0)("IntNotConfirmed")
                'FRMMAIN.RefuseCanceled.Text = dt.Rows(0)("RefuseCanceled")
                FRMMAIN.FollowingInteral.Text = dt.Rows(0)("RecordCount")
                FRMMAIN.ConfirmInternalExCancel.Text = dt.Rows(0)("ConfirmCanceledInternal")
                FRMMAIN.RecordCountConfirmCancel.Text = dt.Rows(0)("RecordCountConfirmCancel")
                FRMMAIN.OutcomeDeliveredInEx.Text = dt.Rows(0)("OutcomeDeliveredInEx")
                FRMMAIN.CanceledInteralIncome.Text = dt.Rows(0)("CanceledInteralIncome")
                FRMMAIN.RecordCountDeliveredCancel.Text = dt.Rows(0)("DeliveredCancel")
                FRMMAIN.EditCount.Text = dt.Rows(0)("EditCount")
                FRMMAIN.ExtOutcomeNotDelivered.Text = dt.Rows(0)("ExtOutcomeNotDelivered")
                FRMMAIN.ExtCanceledConfrimed.Text = dt.Rows(0)("ExtCanceledConfrimed")
                FRMMAIN.ExternalConfirm.Text = dt.Rows(0)("ExternalConfirm")
                FRMMAIN.ExtCanceledConfrimed1.Text = dt.Rows(0)("ExtCONFIRMCANCEL")
                FRMMAIN.CountLeaveCon.Text = dt.Rows(0)("LeaveConfirm")
                FRMMAIN.CountLeaveEnd.Text = dt.Rows(0)("LeaveEnd")
                FRMMAIN.TaxiADD.Text = dt.Rows(0)("TaxiADD")
                FRMMAIN.TAxiNotSend.Text = dt.Rows(0)("TAxiNotSend")
                FRMMAIN.taxiSendFrom.Text = dt.Rows(0)("taxiSendFrom")
                FRMMAIN.TAxiCansel.Text = dt.Rows(0)("TAxiCansel")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function AccountsTb_GetParentAccCode(BranchID As Integer, AccParent As ULong) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID
        PRM(1) = New SqlParameter("@AccParent", SqlDbType.BigInt)
        PRM(1).Value = AccParent
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccountsTb_GetParentAccCode", PRM)
        Return DT
    End Function
    Public Function Tables_CLEARDATA() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("Tables_CLEARDATA")
        Return DT
    End Function
    ' Safe custom-icon loader for the message helpers below. The dialog icons are loose .ico files next to the
    ' exe (shipped as CopyToOutputDirectory content). If one is ever missing or unreadable, New Icon() throws
    ' "Argument 'picture' must be a picture that can be used as a Icon" — and because these run inside Leave/
    ' Validating handlers, that exception took down the whole form. Swallow it and let XtraMessageBox fall back
    ' to its built-in icon; the message still shows.
    Public Sub SetMsgIcon(kind As MessageBoxIcon, fileName As String)
        Try
            XtraMessageBox.Icons(kind) = New Icon(Application.StartupPath & fileName)
        Catch
            ' keep the default icon
        End Try
    End Sub
    Public Sub ErrorMessage(FRM As XtraForm, MSGADRESS As String, MSGTEXT As String)
        SetMsgIcon(MessageBoxIcon.Error, "\error.ico")
        Dim lookAndFeelError As New UserLookAndFeel(FRM)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        XtraMessageBox.Show(lookAndFeelError, MSGTEXT, MSGADRESS, MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Dim msgArgs As New XtraMessageBoxArgs()
        'msgArgs.ImageOptions.Icon = New Icon(Application.StartupPath & "\error.ico")
        'msgArgs.Caption = MSGADRESS
        'msgArgs.Text = MSGTEXT
        'XtraMessageBox.Show(msgArgs)
    End Sub
    Public Sub ErrorMessage2(MSGADRESS As String, MSGTEXT As String)
        Dim msgArgs As New XtraMessageBoxArgs()
        'msgArgs.HtmlTemplate.Assign(FRMMAIN.HtmlTemplateCollection1(0))
        'msgArgs.ImageOptions.SvgImage = FRMMAIN.SVGMG(0)
        msgArgs.Caption = MSGADRESS
        msgArgs.Text = MSGTEXT
        XtraMessageBox.Show(msgArgs)
    End Sub
    Public Sub ErrorMessageUC(FRM As UserControl, MSGADRESS As String, MSGTEXT As String)
        SetMsgIcon(MessageBoxIcon.Error, "\error.ico")
        Dim lookAndFeelError As New UserLookAndFeel(FRM)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        XtraMessageBox.Show(lookAndFeelError, MSGTEXT, MSGADRESS, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
    Public Function YeasNoMessage(FRM As XtraForm, MSGADRESS As String, MSGTEXT As String) As Boolean
        Dim result As DialogResult
        SetMsgIcon(MessageBoxIcon.Information, "\Graphicloads-100-Flat-Information.ico")
        Dim lookAndFeelError As New UserLookAndFeel(FRM)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        result = XtraMessageBox.Show(lookAndFeelError, MSGTEXT, MSGADRESS, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If result = DialogResult.No Then
            YeasNoMessage = False
        End If
        Return YeasNoMessage
    End Function
    Public Sub InfoMessage(FRM As XtraForm, MSGADRESS As String, MSGTEXT As String)
        SetMsgIcon(MessageBoxIcon.Information, "\Graphicloads-100-Flat-Information.ico")
        Dim lookAndFeelError As New UserLookAndFeel(FRM)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        XtraMessageBox.Show(lookAndFeelError, MSGTEXT, MSGADRESS, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Function WarningMessage(FRM As XtraForm, MSGADRESS As String, MSGTEXT As String) As DialogResult
        SetMsgIcon(MessageBoxIcon.Warning, "\warning.ico")
        Dim lookAndFeelError As New UserLookAndFeel(FRM)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        XtraMessageBox.Show(lookAndFeelError, MSGTEXT, MSGADRESS, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        Return WarningMessage
    End Function


    'Sub CompanyInfo()
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    dt = RUN_QUARY_TXT("")
    '    If dt.Rows.Count > 0 Then

    '    End If
    'End Sub
End Module

