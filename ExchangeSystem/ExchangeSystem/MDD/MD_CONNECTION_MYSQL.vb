' =====================================================================================
'  MD_CONNECTION_MYSQL.vb  -  MySQL/MariaDB data-access layer for ExchangeSystem
'  (ported from the proven ShippingSystem implementation — same design, same gotchas)
'
'  PURPOSE: a config-switchable MySQL path that mirrors the SqlClient helpers in Module1,
'  WITHOUT changing any CLS* class or form. They keep building SqlParameter() arrays; this layer
'  converts each to a MySqlParameter (p_-prefixed to match the migrated MySQL procs), executes
'  against MariaDB, and copies OUTPUT values back into the original SqlParameter objects.
'
'  HOW TO ENABLE:
'   1) NuGet package MySqlConnector referenced (packages.config + .vbproj).
'   2) This file is a Compile item in ExchangeSystem.vbproj.
'   3) Module1 helpers delegate here when USE_MYSQL is True (i.e. DB_ENGINE=MYSQL).
'   4) RhallaConfig.ini (next to the exe) supplies DB_ENGINE and MYSQL_CONN_LOCAL / MYSQL_CONN_PROD.
'
'  Set DB_ENGINE=SQLSERVER in RhallaConfig.ini to fall back to the original SQL Server path at any time
'  (that is the default; DB_ENGINE=MYSQL selects this layer). No rebuild needed either way.
' =====================================================================================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports MySqlConnector

Public Module MD_MYSQL

    ' ---------------------------------------------------------------------------------------------
    '  MASTER SWITCH.  True  -> every Module1 helper routes here, to MariaDB/MySQL.
    '                  False -> every Module1 helper stays on its original SQL Server path (SQLCON).
    '
    '  No longer hardcoded: read ONCE from RhallaConfig.ini (DB_ENGINE=SQLSERVER | MYSQL) next to the exe,
    '  the same way MYSQL_TARGET picks local-vs-production MySQL. Switching engines is therefore a one-line
    '  edit of a text file - no rebuild. Default is SQLSERVER (see MD_SECRETS.DbEngine).
    ' ---------------------------------------------------------------------------------------------
    Private _engineLoaded As Boolean = False
    Private _useMySql As Boolean = False

    Private Sub EnsureEngineLoaded()
        If _engineLoaded Then Return
        _engineLoaded = True
        _useMySql = (MD_SECRETS.DbEngine = "MYSQL")
        LogMyInfo("DB ENGINE at startup = " & If(_useMySql, "MYSQL", "SQLSERVER"))
        ' Hooked here rather than in FRMMAIN because this runs on the FIRST data access - earlier than any
        ' form's Load - so a crash during startup is captured too. It must run on BOTH engines: this is the
        ' application-wide error log, not a MySQL-only one. Idempotent.
        InstallGlobalErrorLog()
    End Sub

    Public ReadOnly Property USE_MYSQL As Boolean
        Get
            EnsureEngineLoaded()
            Return _useMySql
        End Get
    End Property

    ' ---------------------------------------------------------------------------------------------
    '  TARGET SWITCH — local MariaDB  <->  production MySQL
    '
    '  Both connection strings live in RhallaConfig.ini (next to the exe, git-ignored) as
    '  MYSQL_CONN_LOCAL and MYSQL_CONN_PROD, so no credential is ever compiled into the binary.
    '
    '  Call USE_LOCAL_MYSQL() or USE_PRODUCTION_MYSQL() to switch. Every helper below builds a fresh
    '  MySqlConnection(MYSQL_CONN) per call, so a switch takes effect on the NEXT data call — there is
    '  no connection to reopen. (Each target keeps its own MySqlConnector pool, keyed by the string.)
    '
    '  Defaults to LOCAL on purpose: production must be an explicit, deliberate act.
    ' ---------------------------------------------------------------------------------------------
    Public Enum MySqlTarget
        LocalDev = 0
        Production = 1
    End Enum

    Private _target As MySqlTarget = MySqlTarget.LocalDev
    Private _targetLoaded As Boolean = False

    ' The startup target comes from RhallaConfig.ini:  MYSQL_TARGET=LOCAL  (or PROD)
    ' so switching environments is a ONE-LINE EDIT of a text file next to the exe — no rebuild.
    ' Read once, lazily, on first data access. USE_LOCAL_MYSQL()/USE_PRODUCTION_MYSQL() still override at runtime.
    Private Sub EnsureTargetLoaded()
        If _targetLoaded Then Return
        _targetLoaded = True
        Dim v As String = MD_SECRETS.GetVal("MYSQL_TARGET").Trim().ToUpperInvariant()
        If v = "PROD" OrElse v = "PRODUCTION" Then
            If MD_SECRETS.MySqlConnProd.Trim().Length > 0 Then
                _target = MySqlTarget.Production
            Else
                ' Asked for production but no connection string configured. Fail SAFE to local rather than
                ' crash before any UI exists — but say so loudly, because "silently on the wrong database"
                ' is the worst outcome. DescribeTarget() will also report LOCAL everywhere it is shown.
                LogMyInfo("MYSQL_TARGET=PROD but MYSQL_CONN_PROD is empty -> STAYING ON LOCAL")
            End If
        End If
        LogMyInfo("DB TARGET at startup = " & DescribeTarget())
        ' Hooked here rather than in FRMMAIN because this runs on the FIRST data access — earlier than any
        ' form's Load — so a crash during startup is captured too. It is idempotent.
        InstallGlobalErrorLog()
    End Sub

    Public ReadOnly Property CURRENT_TARGET As MySqlTarget
        Get
            EnsureTargetLoaded()
            Return _target
        End Get
    End Property

    Public ReadOnly Property IS_PRODUCTION As Boolean
        Get
            EnsureTargetLoaded()
            Return _target = MySqlTarget.Production
        End Get
    End Property

    ''' <summary>Point all data access at the LOCAL MariaDB (MYSQL_CONN_LOCAL). Safe default.</summary>
    Public Sub USE_LOCAL_MYSQL()
        _target = MySqlTarget.LocalDev
        LogMyInfo("DB TARGET -> LOCAL : " & DescribeTarget())
    End Sub

    ''' <summary>
    ''' Point all data access at the PRODUCTION MySQL server (MYSQL_CONN_PROD).
    ''' WRITES GO TO LIVE COMPANY DATA. Throws if MYSQL_CONN_PROD is not configured, rather than
    ''' silently falling back to local — a silent fallback is how you corrupt the wrong database.
    ''' </summary>
    Public Sub USE_PRODUCTION_MYSQL()
        If MD_SECRETS.MySqlConnProd.Trim().Length = 0 Then
            Throw New InvalidOperationException(
                "MYSQL_CONN_PROD is not set in RhallaConfig.ini — refusing to switch to production.")
        End If
        _target = MySqlTarget.Production
        LogMyInfo("DB TARGET -> PRODUCTION : " & DescribeTarget())
    End Sub

    ' Connection string for the CURRENT target. Loaded at runtime from RhallaConfig.ini via MD_SECRETS,
    ' so credentials are NOT stored in source/git.
    ' ConvertZeroDateTime: migrated data can contain MySQL "zero dates" (0000-00-00); without this flag
    ' MySqlConnector throws InvalidCastException when a DataAdapter reads such a column.
    ' Do NOT also set AllowZeroDateTime — the two conflict (column-type mismatch).
    Public ReadOnly Property MYSQL_CONN As String
        Get
            EnsureTargetLoaded()
            If _target = MySqlTarget.Production Then Return MD_SECRETS.MySqlConnProd
            Return MD_SECRETS.MySqlConnLocal
        End Get
    End Property

    ''' <summary>"LOCAL 127.0.0.1/EXCHANGESYS2026" — Server/Database only, PASSWORD NEVER INCLUDED.
    ''' Safe to show in a status bar, a title bar or the log.</summary>
    Public Function DescribeTarget() As String
        Dim srv As String = "?", db As String = "?"
        For Each part As String In MYSQL_CONN.Split(";"c)
            Dim i As Integer = part.IndexOf("="c)
            If i <= 0 Then Continue For
            Dim k As String = part.Substring(0, i).Trim().ToLowerInvariant()
            Dim v As String = part.Substring(i + 1).Trim()
            If k = "server" OrElse k = "data source" Then srv = v
            If k = "database" OrElse k = "initial catalog" Then db = v
        Next
        Return If(_target = MySqlTarget.Production, "PRODUCTION ", "LOCAL ") & srv & "/" & db
    End Function

    ' Diagnostic log: every data-layer exception is appended here with the proc name and a dump of all
    ' parameters. This captures .NET-side failures that never reach MySQL (e.g. a parameter-conversion
    ' overflow thrown during binding), which the MySQL general_log cannot see. Lives next to the EXE.
    Public ReadOnly MYSQL_LOG_PATH As String =
        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mysql_errors.log")

    Public Sub LogMyInfo(msg As String)
        Try
            System.IO.File.AppendAllText(MYSQL_LOG_PATH,
                "---- " & DateTime.Now.ToString("HH:mm:ss.fff") & "  INFO: " & msg & Environment.NewLine)
        Catch
        End Try
    End Sub

    Public Sub LogMyError(name As String, prm() As SqlParameter, ex As Exception)
        Try
            Dim sb As New System.Text.StringBuilder
            sb.AppendLine("==================== " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") & " ====================")
            sb.AppendLine("CALL : " & If(name, "(nothing)"))
            Dim e As Exception = ex
            Dim depth As Integer = 0
            While e IsNot Nothing
                sb.AppendLine(New String(" "c, depth * 2) & "EX  : " & e.GetType().FullName & ": " & e.Message)
                e = e.InnerException
                depth += 1
            End While
            sb.AppendLine("STACK:")
            sb.AppendLine(ex.StackTrace)
            If prm IsNot Nothing Then
                sb.AppendLine("PARAMS (" & prm.Length & "):")
                For Each p As SqlParameter In prm
                    Dim valStr As String, valType As String
                    If p.Value Is Nothing Then
                        valStr = "Nothing" : valType = "-"
                    ElseIf p.Value Is DBNull.Value Then
                        valStr = "DBNull" : valType = "DBNull"
                    Else
                        valType = p.Value.GetType().Name
                        Try : valStr = Convert.ToString(p.Value, System.Globalization.CultureInfo.InvariantCulture) : Catch : valStr = "<unprintable>" : End Try
                        If valStr IsNot Nothing AndAlso valStr.Length > 200 Then valStr = valStr.Substring(0, 200) & "…"
                    End If
                    sb.AppendLine("   " & p.ParameterName & "  SqlDbType=" & p.SqlDbType.ToString() &
                                  "  Dir=" & p.Direction.ToString() & "  ClrType=" & valType & "  Value=[" & valStr & "]")
                Next
            End If
            sb.AppendLine()
            System.IO.File.AppendAllText(MYSQL_LOG_PATH, sb.ToString())
        Catch
            ' logging must never throw
        End Try
    End Sub

    ' ---- CATCH-ALL LOGGING ---------------------------------------------------------------------------
    ' LogMyError above only sees exceptions thrown INSIDE the data layer. A failure in a form - the classic
    ' one being "Conversion from type 'DBNull' to type 'String' is not valid", thrown when a screen assigns
    ' a NULL column straight into a String - happens AFTER the query returned, so it never reached the log
    ' and there was nothing to send to support beyond a screenshot.
    '
    ' InstallGlobalErrorLog() wires the two framework-level events that see EVERY unhandled exception,
    ' wherever it is raised, so the log becomes a complete record instead of a data-layer-only one.
    Private _globalHooked As Boolean = False

    Public Sub InstallGlobalErrorLog()
        If _globalHooked Then Exit Sub          ' idempotent: safe if startup calls it more than once
        _globalHooked = True
        Try
            ' exceptions on the UI thread (button handlers, grid events, form load, ...)
            AddHandler Application.ThreadException,
                Sub(sender As Object, e As Threading.ThreadExceptionEventArgs)
                    LogAppError("UI thread", sender, e.Exception)
                End Sub
            ' exceptions anywhere else (background threads, timers, ...)
            AddHandler AppDomain.CurrentDomain.UnhandledException,
                Sub(sender As Object, e As UnhandledExceptionEventArgs)
                    LogAppError("AppDomain", sender, TryCast(e.ExceptionObject, Exception))
                End Sub
            ' FIRST-CHANCE: fires the instant ANY exception is thrown, BEFORE a Catch can swallow it. The two
            ' handlers above only see UNHANDLED errors, so a form that does "Catch ex : MessageBox.Show(ex.Message)"
            ' (the pervasive pattern here) never reached the log — e.g. the raw-SQLCON "ConnectionString has not
            ' been initialized" was shown to the user but not recorded. This makes the log the complete record the
            ' team expects. Filtered to app/DB-originated exceptions so DevExpress/framework internal throws don't
            ' flood it, deduped against rapid repeats, and guarded against re-entry (the logger's own IO throwing).
            AddHandler AppDomain.CurrentDomain.FirstChanceException,
                Sub(sender As Object, e As System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs)
                    LogFirstChance(e.Exception)
                End Sub
        Catch
            ' never let logging setup stop the app starting
        End Try
    End Sub

    Private _inFirstChance As Boolean = False
    Private _lastFcMsg As String = ""
    Private _lastFcAt As DateTime = DateTime.MinValue
    Private Sub LogFirstChance(ex As Exception)
        If ex Is Nothing OrElse _inFirstChance Then Exit Sub
        Try
            ' keep only exceptions that came THROUGH our own code or the DB layer; skip framework/DevExpress noise
            Dim st As String = Nothing
            Try : st = ex.StackTrace : Catch : End Try
            Dim isApp As Boolean = (st IsNot Nothing AndAlso st.Contains("ExchangeSystem."))
            Dim isDb As Boolean = (TypeOf ex Is MySqlConnector.MySqlException) OrElse (TypeOf ex Is SqlClient.SqlException)
            If Not (isApp OrElse isDb) Then Exit Sub
            ' collapse rapid repeats of the same message (a Leave/Validating loop can re-throw many times a second)
            If ex.Message = _lastFcMsg AndAlso (DateTime.Now - _lastFcAt).TotalSeconds < 2 Then Exit Sub
            _lastFcMsg = ex.Message
            _lastFcAt = DateTime.Now
            _inFirstChance = True
            LogAppError("first-chance", Nothing, ex)
        Catch
            ' logging must never itself raise
        Finally
            _inFirstChance = False
        End Try
    End Sub

    ' Log an exception raised OUTSIDE the data layer. Same file and same block format as LogMyError, so one
    ' log tells the whole story. The active form/control name is recorded because a DBNull-conversion error
    ' carries no clue about WHICH screen produced it.
    Public Sub LogAppError(source As String, sender As Object, ex As Exception)
        Try
            Dim sb As New System.Text.StringBuilder
            sb.AppendLine("==================== " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") &
                          " ====================")
            sb.AppendLine("APP ERROR (" & source & ")")
            Try
                Dim f As Form = Form.ActiveForm
                sb.AppendLine("FORM : " & If(f IsNot Nothing, f.Name & "  [" & f.Text & "]", "(none)"))
            Catch
            End Try
            If sender IsNot Nothing Then sb.AppendLine("SENDER: " & sender.GetType().FullName)
            Dim e As Exception = ex
            Dim depth As Integer = 0
            While e IsNot Nothing
                sb.AppendLine(New String(" "c, depth * 2) & "EX  : " & e.GetType().FullName & ": " & e.Message)
                e = e.InnerException
                depth += 1
            End While
            sb.AppendLine("STACK:")
            sb.AppendLine(If(ex IsNot Nothing, ex.StackTrace, "(no exception object)"))
            sb.AppendLine()
            System.IO.File.AppendAllText(MYSQL_LOG_PATH, sb.ToString())
        Catch
            ' logging must never throw
        End Try
    End Sub

    ' Map a SqlParameter to a MySqlParameter. For stored procs use the p_ prefix (the migrated routines
    ' declare params as p_<name>); for inline function-call TEXT keep the original name so the @placeholders
    ' in the SQL text bind correctly. OUT/INOUT preserved.
    Private Function ToMy(p As SqlParameter, Optional prefix As Boolean = True) As MySqlParameter
        Dim bare As String = p.ParameterName.TrimStart("@"c)
        Dim nm As String = If(prefix, "@p_" & bare, "@" & bare)
        Dim mp As New MySqlParameter(nm, MapType(p.SqlDbType)) With {.Direction = p.Direction}
        If p.Size <> 0 Then mp.Size = p.Size
        If p.Direction <> ParameterDirection.Output Then mp.Value = If(p.Value, DBNull.Value)
        Return mp
    End Function

    Private Function MapType(t As SqlDbType) As MySqlDbType
        Select Case t
            Case SqlDbType.Int : Return MySqlDbType.Int32
            Case SqlDbType.BigInt : Return MySqlDbType.Int64
            Case SqlDbType.SmallInt : Return MySqlDbType.Int16
            ' SQL Server TINYINT is 0..255, but callers sometimes pass an out-of-range sentinel such as a
            ' ComboBox SelectedIndex = -1. Mapping to UByte (0..255) makes MySqlConnector throw a hard .NET
            ' "Arithmetic operation resulted in an overflow" during param prep. Int16 holds every valid
            ' tinyint value AND -1, so the value reaches MySQL (which coerces it) instead of crashing a save.
            Case SqlDbType.TinyInt : Return MySqlDbType.Int16
            Case SqlDbType.Bit : Return MySqlDbType.Bool
            Case SqlDbType.Decimal, SqlDbType.Money : Return MySqlDbType.Decimal
            Case SqlDbType.Float : Return MySqlDbType.Double
            Case SqlDbType.Real : Return MySqlDbType.Float
            Case SqlDbType.Date : Return MySqlDbType.Date
            Case SqlDbType.DateTime, SqlDbType.SmallDateTime, SqlDbType.DateTime2 : Return MySqlDbType.DateTime
            Case SqlDbType.Time : Return MySqlDbType.Time
            Case SqlDbType.VarBinary, SqlDbType.Binary, SqlDbType.Image : Return MySqlDbType.LongBlob
            Case SqlDbType.UniqueIdentifier : Return MySqlDbType.VarChar
            Case Else : Return MySqlDbType.VarChar   ' nvarchar/varchar/char/text/etc.
        End Select
    End Function

    ' copy OUT/INOUT values back into the original SqlParameter objects the caller reads
    '
    ' THREE things this must tolerate, all of which crashed it in the field with a NullReferenceException:
    '
    '  1) prm itself is Nothing. Dozens of callers go through LoadToControlar(...), which passes its
    '     optional PrmType straight through as Nothing ("LoadToControlar(CurrencyID, "..", .., Nothing)").
    '     The SQL Server path in Module1.RUN_QUARY_PRO_alter explicitly tests "If PRM Is Nothing", so those
    '     screens always worked on SQL Server and only broke once routed through here.
    '  2) an individual prm(i) is Nothing. The legacy style is "Dim PRM(43) As SqlParameter" followed by
    '     filling only some slots, so unfilled entries are Nothing.
    '  3) cmd.Parameters is NOT positionally aligned with prm. EnsureAllProcParams appends any parameter the
    '     proc declares but the caller omitted, and StageTvps removes TVP parameters, so index i on one side
    '     is not necessarily index i on the other. Matching BY NAME is correct regardless of ordering; the
    '     old code indexed cmd.Parameters(i) and would silently copy the WRONG value back into an OUTPUT
    '     parameter whenever the two lists diverged.
    Private Sub CopyOut(prm() As SqlParameter, cmd As MySqlCommand)
        If prm Is Nothing Then Exit Sub
        For i As Integer = 0 To prm.Length - 1
            Dim p As SqlParameter = prm(i)
            If p Is Nothing Then Continue For
            If p.Direction = ParameterDirection.Input Then Continue For
            Dim want As String = p.ParameterName
            If want Is Nothing Then Continue For
            want = want.TrimStart("@"c)
            For Each mp As MySqlParameter In cmd.Parameters
                ' ToMy() renames "@SumDebitFinal" to "@p_SumDebitFinal" (the migrated procs prefix every
                ' parameter with p_), and EnsureAllProcParams adds the proc's own already-prefixed names.
                ' RUN_FUNCTION_PARM_MY binds without the prefix, so BOTH spellings must be accepted.
                Dim got As String = mp.ParameterName.TrimStart("@"c)
                If got.StartsWith("p_", StringComparison.OrdinalIgnoreCase) Then got = got.Substring(2)
                If String.Equals(got, want, StringComparison.OrdinalIgnoreCase) Then
                    p.Value = mp.Value
                    Exit For
                End If
            Next
        Next
    End Sub

    ' A TVP parameter arrives from the CLS classes as a SqlParameter whose Value is a DataTable
    ' (SqlDbType.Structured). MySQL has no TVP, so the migrated proc reads a session TEMPORARY TABLE
    ' tvp_<paramname>; here we (re)create that table from the DataTable schema and bulk-load its rows,
    ' on the SAME connection, BEFORE calling the proc. Scalar params are bound normally.
    Private Function IsTvp(p As SqlParameter) As Boolean
        Return p.Value IsNot Nothing AndAlso TypeOf p.Value Is DataTable
    End Function

    Private Function MySqlColType(t As Type) As String
        If t Is GetType(Integer) OrElse t Is GetType(Short) OrElse t Is GetType(Byte) Then Return "BIGINT"
        If t Is GetType(Long) Then Return "BIGINT"
        If t Is GetType(Boolean) Then Return "TINYINT(1)"
        If t Is GetType(Decimal) OrElse t Is GetType(Double) OrElse t Is GetType(Single) Then Return "DOUBLE"
        If t Is GetType(Date) Then Return "DATETIME"
        Return "LONGTEXT"
    End Function

    ' Stage every TVP param into its tvp_<name> temp table. Returns the scalar-only param subset for the CALL.
    Private Function StageTvps(con As MySqlConnection, prm() As SqlParameter) As SqlParameter()
        If prm Is Nothing Then Return prm
        Dim scalars As New List(Of SqlParameter)
        For Each p In prm
            If IsTvp(p) Then
                Dim dt As DataTable = DirectCast(p.Value, DataTable)
                Dim tbl As String = "tvp_" & p.ParameterName.TrimStart("@"c)
                Using c1 As New MySqlCommand("DROP TEMPORARY TABLE IF EXISTS `" & tbl & "`", con) : c1.ExecuteNonQuery() : End Using
                ' Some forms declare TVP DataTable columns with square brackets (e.g. "[ProfileID]"), mimicking
                ' T-SQL. SQL Server maps TVP columns to the table type BY ORDINAL, so the brackets were harmless.
                ' MySQL has no TVP: we stage tvp_<name> and the procs read its columns BY NAME, so a bracketed
                ' name would never match (error 1054, masked by the proc's handler as a generic "error").
                ' Strip [ ] and .Trim() (a stray trailing space makes CREATE TEMPORARY TABLE fail with 1166).
                Dim cols = dt.Columns.Cast(Of DataColumn).Select(Function(dc) "`" & dc.ColumnName.Replace("[", "").Replace("]", "").Trim() & "` " & MySqlColType(dc.DataType))
                Using c2 As New MySqlCommand("CREATE TEMPORARY TABLE `" & tbl & "` (" & String.Join(", ", cols) & ")", con) : c2.ExecuteNonQuery() : End Using
                If dt.Rows.Count > 0 Then
                    Dim colNames = dt.Columns.Cast(Of DataColumn).Select(Function(dc) "`" & dc.ColumnName.Replace("[", "").Replace("]", "").Trim() & "`").ToArray()
                    For Each row As DataRow In dt.Rows
                        Using ci As New MySqlCommand("INSERT INTO `" & tbl & "` (" & String.Join(",", colNames) & ") VALUES (" &
                                String.Join(",", Enumerable.Range(0, dt.Columns.Count).Select(Function(i) "@v" & i)) & ")", con)
                            For i = 0 To dt.Columns.Count - 1
                                Dim cv As Object = If(row(i), DBNull.Value)
                                ' Many forms build untyped (String) DataTable columns and add VB Boolean values
                                ' (e.g. IsActive = True), so the cell holds the string "True"/"False". SQL Server
                                ' converted 'True' -> BIT 1 when staging the TVP; MySQL converts "True" -> 0,
                                ' silently deactivating every inserted row. Normalize back to 1/0.
                                If TypeOf cv Is String Then
                                    Dim sv As String = DirectCast(cv, String)
                                    If String.Equals(sv, "True", StringComparison.OrdinalIgnoreCase) Then
                                        cv = 1
                                    ElseIf String.Equals(sv, "False", StringComparison.OrdinalIgnoreCase) Then
                                        cv = 0
                                    End If
                                End If
                                ci.Parameters.AddWithValue("@v" & i, cv)
                            Next
                            ci.ExecuteNonQuery()
                        End Using
                    Next
                End If
            Else
                scalars.Add(p)
            End If
        Next
        Return scalars.ToArray()
    End Function

    Private Function AddParams(cmd As MySqlCommand, prm() As SqlParameter, Optional prefix As Boolean = True) As MySqlCommand
        If prm IsNot Nothing Then
            For Each p In prm
                cmd.Parameters.Add(ToMy(p, prefix))
            Next
        End If
        Return cmd
    End Function

    ' MySQL stored procedures have NO default parameter values, but the legacy T-SQL procs rely on them
    ' heavily (e.g. procs declared `@Description ... = NULL` that the read path calls with only a couple of
    ' args). MySqlConnector requires every declared proc parameter to be present in the command, otherwise it
    ' throws "Parameter 'p_x' not found in the collection." Here we look up the proc's actual parameters and
    ' add any the caller did not supply as NULL (OUT/INOUT direction preserved) — mirroring SQL Server's
    ' "a missing argument takes the parameter's default". Appended params go AFTER the caller's, so CopyOut's
    ' positional copy over the original scalar set is unaffected.
    Private Sub EnsureAllProcParams(con As MySqlConnection, procName As String, cmd As MySqlCommand)
        Try
            Dim schema As String = con.Database
            Dim sp As String = procName
            If sp.Contains(".") Then
                Dim parts = sp.Split("."c)
                schema = parts(0) : sp = parts(parts.Length - 1)
            End If
            Dim have As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each mp As MySqlParameter In cmd.Parameters
                have.Add(mp.ParameterName.TrimStart("@"c))
            Next
            Dim missing As New List(Of String())
            Using q As New MySqlCommand(
                "SELECT PARAMETER_NAME, PARAMETER_MODE FROM information_schema.parameters " &
                "WHERE SPECIFIC_SCHEMA = @s AND SPECIFIC_NAME = @p AND PARAMETER_NAME IS NOT NULL " &
                "ORDER BY ORDINAL_POSITION", con)
                q.Parameters.AddWithValue("@s", schema)
                q.Parameters.AddWithValue("@p", sp)
                Using r = q.ExecuteReader()
                    While r.Read()
                        Dim pn As String = r.GetString(0)
                        Dim mode As String = If(r.IsDBNull(1), "IN", r.GetString(1))
                        If Not have.Contains(pn) Then missing.Add(New String() {pn, mode})
                    End While
                End Using
            End Using
            For Each m In missing
                Dim mp As New MySqlParameter("@" & m(0), DBNull.Value)
                Select Case m(1).ToUpperInvariant()
                    Case "OUT" : mp.Direction = ParameterDirection.Output
                    Case "INOUT" : mp.Direction = ParameterDirection.InputOutput
                End Select
                cmd.Parameters.Add(mp)
            Next
        Catch
            ' if introspection fails, leave the command as-is and let the CALL proceed/fail normally
        End Try
    End Sub

    ' Normalize a stored-proc name for MySQL. Several call sites pass the T-SQL form with square-bracket
    ' quoting and/or a schema prefix — e.g. "[SAFETB_Insert]", "[dbo].[X]", "dbo.X". SQL Server accepts
    ' `EXEC [X]`; MySQL's `CALL [X]` is a syntax error. Strip brackets, the dbo. prefix, and trailing spaces.
    Private Function TrimName(n As String) As String
        Dim s As String = n.Trim()
        s = s.Replace("[", "").Replace("]", "")
        If s.Trim().ToLowerInvariant().StartsWith("dbo.") Then s = s.Trim().Substring(4)
        Return s.Trim()
    End Function

    ' True when the string is a single SQL identifier (a stored-proc name), e.g. "Tables_CLEARDATA".
    ' The legacy app passes such names to RUN_QUARY_TXT/RUN_EXUTE_TXT and relies on SQL Server's
    ' "EXEC is optional for a lone proc name in a batch" behavior. MySQL has no implicit EXEC, so these
    ' must be routed through CommandType.StoredProcedure (a CALL).
    Private Function IsBareName(q As String) As Boolean
        If String.IsNullOrWhiteSpace(q) Then Return False
        Dim t As String = TrimName(q)
        If t.Length = 0 Then Return False
        For Each ch As Char In t
            If Not (Char.IsLetterOrDigit(ch) OrElse ch = "_"c) Then Return False
        Next
        Return True
    End Function

    ' ---- MySQL equivalents of the Module1 helpers ----

    Public Function RUN_QUARY_PRO_MY(name As String, prm() As SqlParameter) As DataTable
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Dim scalars = StageTvps(con, prm)
                Using cmd As New MySqlCommand(TrimName(name), con) With {.CommandType = CommandType.StoredProcedure}
                    AddParams(cmd, scalars)
                    EnsureAllProcParams(con, TrimName(name), cmd)
                    Dim dt As New DataTable
                    ' Use a data adapter (NOT DataTable.Load): Load infers a primary key from the result-set
                    ' schema and MERGES rows sharing that key, silently collapsing multi-row grids to one row.
                    ' Fill appends every row, matching SqlDataAdapter.
                    Using da As New MySqlDataAdapter(cmd) : da.Fill(dt) : End Using
                    CopyOut(scalars, cmd)
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            LogMyError(name, prm, ex)
            Return Nothing
        End Try
    End Function

    Public Sub RUN_EXUTE_PRO_MY(name As String, prm() As SqlParameter)
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Dim scalars = StageTvps(con, prm)
                Using cmd As New MySqlCommand(TrimName(name), con) With {.CommandType = CommandType.StoredProcedure}
                    AddParams(cmd, scalars)
                    EnsureAllProcParams(con, TrimName(name), cmd)
                    cmd.ExecuteNonQuery()
                    CopyOut(scalars, cmd)
                End Using
            End Using
        Catch ex As Exception
            LogMyError(name, prm, ex)
            Throw
        End Try
    End Sub

    Public Function RUN_QUARY_PRO_ONLY_MY(name As String) As DataTable
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Using cmd As New MySqlCommand(TrimName(name), con) With {.CommandType = CommandType.StoredProcedure}
                    EnsureAllProcParams(con, TrimName(name), cmd)
                    Dim dt As New DataTable
                    Using da As New MySqlDataAdapter(cmd) : da.Fill(dt) : End Using
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            LogMyError(name, Nothing, ex)
            Return Nothing
        End Try
    End Function

    ' scalar SQL function: T-SQL "select dbo.fn(args)" -> MySQL "SELECT fn(args)"
    Public Function RUN_FUNCTION_PARM_MY(fnCall As String, prm() As SqlParameter) As DataTable
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Using cmd As New MySqlCommand("SELECT " & fnCall, con) With {.CommandType = CommandType.Text}
                    AddParams(cmd, prm, prefix:=False)   ' text call: @placeholders use the original names
                    Dim dt As New DataTable
                    Using da As New MySqlDataAdapter(cmd) : da.Fill(dt) : End Using
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            LogMyError(fnCall, prm, ex)
            Return Nothing
        End Try
    End Function

    Public Sub RUN_EXUTE_TXT_MY(sql As String)
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                If IsBareName(sql) Then
                    Using cmd As New MySqlCommand(TrimName(sql), con) With {.CommandType = CommandType.StoredProcedure}
                        cmd.ExecuteNonQuery()
                    End Using
                Else
                    Using cmd As New MySqlCommand(sql, con) With {.CommandType = CommandType.Text}
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            End Using
        Catch ex As Exception
            LogMyError(sql, Nothing, ex)
            Throw
        End Try
    End Sub

    ' Inline-text reads (RUN_QUARY_TXT / RUN_TXT / RUN_QUARY_Field / RUN_QUARY_QUERY_ONLY).
    ' A bare proc name -> CALL; otherwise the SELECT text is filled via a data adapter.
    Public Function RUN_QUARY_TXT_MY(sql As String) As DataTable
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Dim dt As New DataTable
                If IsBareName(sql) Then
                    Using cmd As New MySqlCommand(TrimName(sql), con) With {.CommandType = CommandType.StoredProcedure}
                        Using da As New MySqlDataAdapter(cmd) : da.Fill(dt) : End Using
                    End Using
                Else
                    Using cmd As New MySqlCommand(sql, con) With {.CommandType = CommandType.Text}
                        Using da As New MySqlDataAdapter(cmd) : da.Fill(dt) : End Using
                    End Using
                End If
                Return dt
            End Using
        Catch ex As Exception
            LogMyError(sql, Nothing, ex)
            Return Nothing
        End Try
    End Function

    ' Report/print support. Many report screens fill a DataSet straight from SQLCON (a SqlConnection
    ' hardwired to SQL Server) and bind it to a DevExpress XtraReport, BYPASSING this data layer — so under
    ' USE_MYSQL those reports would still read SQL Server while every write goes to MariaDB (records appear
    ' "missing" / "لا يوجد بيانات"). These return a DataSet sourced from MariaDB so a report site can do
    ' exactly:  report.DataSource = ds : report.DataMember = memberName.
    Public Function RUN_QUARY_DS_MY(name As String, prm() As SqlParameter, Optional memberName As String = "Table") As DataSet
        Dim ds As New DataSet
        Dim dt As DataTable = RUN_QUARY_PRO_MY(name, prm)
        If dt Is Nothing Then dt = New DataTable
        dt.TableName = memberName
        ds.Tables.Add(dt)
        Return ds
    End Function

    Public Function RUN_QUARY_DS_TXT_MY(sql As String, Optional memberName As String = "Table") As DataSet
        Dim ds As New DataSet
        Dim dt As DataTable = RUN_QUARY_TXT_MY(sql)
        If dt Is Nothing Then dt = New DataTable
        dt.TableName = memberName
        ds.Tables.Add(dt)
        Return ds
    End Function

    ' ---- Exchange-specific helpers ----
    ' Module1's GETMAXID / GETIDMAX / GETUSERMAXID / GET_LAST_RECORD build inline SQL and run it on the
    ' global SqlConnection directly (bypassing RUN_*). Under USE_MYSQL that connection points at SQL Server,
    ' so they must be routed here too or the app would read IDs from the WRONG database — producing duplicate
    ' keys on every insert. The SQL text ("select ID from T order by C") is already valid MySQL.
    Public Function SCALARINT_MY(sql As String) As Integer
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Using cmd As New MySqlCommand(sql, con) With {.CommandType = CommandType.Text}
                    Dim o As Object = cmd.ExecuteScalar()
                    If o Is Nothing OrElse o Is DBNull.Value Then Return 0
                    Return Convert.ToInt32(o)
                End Using
            End Using
        Catch ex As Exception
            LogMyError(sql, Nothing, ex)
            Return 0
        End Try
    End Function

    Public Function SCALARLONG_MY(sql As String, prm() As SqlParameter) As Long
        Try
            Using con As New MySqlConnection(MYSQL_CONN)
                con.Open()
                Using cmd As New MySqlCommand(sql, con) With {.CommandType = CommandType.Text}
                    AddParams(cmd, prm, prefix:=False)
                    Dim o As Object = cmd.ExecuteScalar()
                    If o Is Nothing OrElse o Is DBNull.Value Then Return 0
                    Return Convert.ToInt64(o)
                End Using
            End Using
        Catch ex As Exception
            LogMyError(sql, prm, ex)
            Return 0
        End Try
    End Function

End Module
