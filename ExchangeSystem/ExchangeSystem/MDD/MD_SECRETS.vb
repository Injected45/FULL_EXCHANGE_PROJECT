' =====================================================================================
'  MD_SECRETS.vb  -  runtime secrets loader   (ported from the shipping system)
'
'  Keeps credentials (MySQL connection string, WhatsApp gateway session/API key) OUT of the
'  source code / git. Values are read once from a plain-text file "RhallaConfig.ini" that sits
'  next to ExchangeSystem.exe. Ship that file WITH the app (it is git-ignored), never commit it.
'
'  File format (key=value, one per line; '#' or ';' at line start = comment):
'    DB_ENGINE=SQLSERVER            (or MYSQL - picks the whole data-access path)
'    SQLSERVER_CONN=Data Source=.\SQLEXPRESS;Initial Catalog=EXCHANGESYS2026;Integrated Security=True
'    MYSQL_CONN=Server=127.0.0.1;Port=3306;Database=EXCHANGESYS2026;User Id=root;Password=;CharacterSet=utf8mb4;...
'    WA_SESSION_ID=...
'    WA_API_KEY=...
'
'  Only the FIRST '=' splits key/value, so connection strings (which contain '=') are preserved.
' =====================================================================================
Imports System.IO
Imports System.Collections.Generic

Public Module MD_SECRETS

    Private _loaded As Boolean = False
    Private ReadOnly _vals As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

    Private Sub EnsureLoaded()
        If _loaded Then Return
        _loaded = True
        Try
            Dim path As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RhallaConfig.ini")
            If System.IO.File.Exists(path) Then
                For Each line As String In System.IO.File.ReadAllLines(path)
                    Dim t As String = line.Trim()
                    If t.Length = 0 OrElse t.StartsWith("#") OrElse t.StartsWith(";") Then Continue For
                    Dim i As Integer = t.IndexOf("="c)
                    If i > 0 Then _vals(t.Substring(0, i).Trim()) = t.Substring(i + 1).Trim()
                Next
            End If
        Catch
            ' leave _vals empty; callers get "" and fail with a clear connection error
        End Try
    End Sub

    Public Function GetVal(key As String) As String
        EnsureLoaded()
        Dim v As String = Nothing
        If _vals.TryGetValue(key, v) Then Return v
        Return ""
    End Function

    ' Legacy single-target key. Still honoured so an existing RhallaConfig.ini keeps working unchanged.
    Public ReadOnly Property MySqlConn As String
        Get
            Return GetVal("MYSQL_CONN")
        End Get
    End Property

    ' ---- two-target support (see MD_MYSQL.USE_LOCAL_MYSQL / USE_PRODUCTION_MYSQL) -------------------
    ' LOCAL falls back to the legacy MYSQL_CONN when MYSQL_CONN_LOCAL is absent, so nothing breaks for
    ' installs that predate the switch.
    Public ReadOnly Property MySqlConnLocal As String
        Get
            Dim v As String = GetVal("MYSQL_CONN_LOCAL")
            If v.Length = 0 Then v = GetVal("MYSQL_CONN")
            Return v
        End Get
    End Property

    ' PRODUCTION deliberately has NO fallback: if MYSQL_CONN_PROD is missing we return "" so the switch
    ' fails loudly instead of silently pointing production traffic at the local database (or vice versa).
    Public ReadOnly Property MySqlConnProd As String
        Get
            Return GetVal("MYSQL_CONN_PROD")
        End Get
    End Property

    ' ---- WHICH DATABASE ENGINE? ---------------------------------------------------------------------
    ' DB_ENGINE decides which data-access path every helper in Module1 takes:
    '   DB_ENGINE=SQLSERVER  (default) -> the original SqlClient path: Module1.OPENCONNECTION + SQLCON,
    '                                     connection string from SQLSERVER_CONN below.
    '   DB_ENGINE=MYSQL                -> MD_MYSQL, using MYSQL_CONN_LOCAL / MYSQL_CONN_PROD above.
    ' Missing or unrecognised means SQLSERVER on purpose: an install whose config file is absent or
    ' mistyped must not silently land on a different database engine than the one it was shipped against.
    Public ReadOnly Property DbEngine As String
        Get
            Dim v As String = GetVal("DB_ENGINE").Trim().ToUpperInvariant()
            If v = "MYSQL" OrElse v = "MARIADB" Then Return "MYSQL"
            Return "SQLSERVER"
        End Get
    End Property

    ' SQL Server connection string used by Module1.OPENCONNECTION when DB_ENGINE=SQLSERVER.
    ' Empty (or no config file) -> OPENCONNECTION falls back to its built-in LOCAL_SQLSERVER_CONN
    ' (.\SQLEXPRESS / EXCHANGESYS2026, Integrated Security), so a workstation with the standard local
    ' instance needs no entry here at all. Set it to point at another server without rebuilding.
    Public ReadOnly Property SqlServerConn As String
        Get
            Return GetVal("SQLSERVER_CONN")
        End Get
    End Property

    ' Auto-update (Module2.ChickUpdate -> UpdateApp) DOWNLOADS THE VENDOR'S PRODUCTION BUILD over this exe.
    ' That binary is hardwired to the production server, so on any machine running a locally built exe -
    ' or pointing at a local database - it silently undoes the local setup on the next launch. Default OFF;
    ' opt in explicitly with AUTO_UPDATE=ON.
    Public ReadOnly Property AutoUpdateEnabled As Boolean
        Get
            Dim v As String = GetVal("AUTO_UPDATE").Trim().ToUpperInvariant()
            Return (v = "ON" OrElse v = "1" OrElse v = "TRUE" OrElse v = "YES")
        End Get
    End Property

    Public ReadOnly Property WaSessionId As String
        Get
            Return GetVal("WA_SESSION_ID")
        End Get
    End Property

    Public ReadOnly Property WaApiKey As String
        Get
            Return GetVal("WA_API_KEY")
        End Get
    End Property

End Module
