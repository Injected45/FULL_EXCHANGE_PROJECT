
Module MD_PUBLIC
    Public NUMBER_CHECK As Integer = 0
    Public NUMBER_FORM As Integer = 0
    Public Sub AUTO_NUMER_NUM(ByVal DGV As DataGridView, ByVal CLR As Color, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs, ByVal FON As Color)
        Try
            Dim ROW = DGV.Rows(e.RowIndex)
            Dim CELL = ROW.Cells(0)
            If Not ROW.IsNewRow Then
                Dim NUMBER As Integer = e.RowIndex + 1.ToString(System.Globalization.CultureInfo.CurrentUICulture)
                CELL.Value = (NUMBER).ToString
                CELL.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                CELL.Style.ForeColor = FON
                CELL.Style.BackColor = CLR
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub ONLY_NUMBER_TEXTBOX(ByVal SENDER As System.Object, ByVal E As System.Windows.Forms.KeyPressEventArgs)
        Dim TB As TextBox = SENDER
        Select Case E.KeyChar
            Case "0" To "9"
            Case Else
                E.Handled = True
        End Select
    End Sub


    Public Function CHECK_FOR_AD(ByVal COL As String, ByVal TB_ As String, ByVal NAME_ As String) As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECT " & COL.Trim & " FROM " & TB_ & " WHERE (" & COL.Trim & " = '" & NAME_.Trim & "'  )")
        If DT.Rows.Count > 0 Then
            NUMBER_CHECK = 1
        Else
            NUMBER_CHECK = 0
        End If
        Return DT
    End Function
    Public Function CHECK_FOR_UPDATE(ByVal COL As String, ByVal TB_ As String, ByVal NAME_ As String, ByVal GUID_ As String) As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECT " & COL & " FROM " & TB_ & " WHERE (" & COL & " = '" & NAME_ & "'  )AND(GUID<> '" & GUID_ & "')")
        If DT.Rows.Count > 0 Then
            NUMBER_CHECK = 1
        Else
            NUMBER_CHECK = 0
        End If
        Return DT
    End Function
    Public Function CHECK_(ByVal TB_ As String, ByVal COL As String, ByVal PR_K As String, ByVal VAL_PR_K As String) As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECT " & COL & " FROM " & TB_ & "  WHERE " & PR_K & "= '" & VAL_PR_K & "' ")
        If DT.Rows.Count > 0 Then
            NUMBER_CHECK = DT.Rows(0)("" & COL & "").ToString
        End If
        Return DT
    End Function


    Public Sub FILL_DGV(ByVal DGV As DataGridView, ByVal ROWS As Integer)
        ROWS = ROWS - DGV.Rows.Count
        For I As Integer = 0 To ROWS
            DGV.Rows.Add()
        Next
    End Sub


    Public Function FILL_ANY_COMBOX(ByVal COL_NAME As String,
                                    ByVal TBL_NAME As String,
                                    ByVal PK As String,
                                    ByVal COM As ComboBox) As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECT  *   FROM " & TBL_NAME & "")
        If DT.Rows.Count > 0 Then
            COM.DataSource = DT
            COM.DisplayMember = "" & COL_NAME & ""
            COM.ValueMember = "" & PK & ""
            COM.SelectedItem = 0
        Else
            DT.Clear()
            COM.Items.Clear()
            COM.DataSource = DT
        End If
        Return DT
    End Function

    Public Function FILL_ANY_COMBOX_PRM(ByVal COL_NAME As String,
                                      ByVal TBL_NAME As String,
                                      ByVal PK As String,
                                      ByVal VAL_PRM As String,
                                       ByVal PRM As String,
                                      ByVal COM As ComboBox) As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECT * FROM " & TBL_NAME & " WHERE " & PRM & " = '" & VAL_PRM & "'")
        If DT.Rows.Count > 0 Then
            COM.DataSource = DT
            COM.DisplayMember = "" & COL_NAME & ""
            COM.ValueMember = "" & PK & ""
            COM.SelectedItem = 0
        Else
            DT.Clear()
            COM.DataSource = DT
            COM.Items.Clear()
        End If
        Return DT

    End Function
    Public Sub ACTIVATE_ACC(GUID As String, VAL_ As Integer)
        Try
            Dim UPDATESTRING As String = "UPDATE TB_ACCOUNTS SET ACTIVATION=@ACTIVATION WHERE GUID='" & GUID & "'"
            Dim CMD As New SqlClient.SqlCommand(UPDATESTRING, SQLCON)
            CMD.Parameters.Add(New SqlClient.SqlParameter("@ACTIVATION", SqlDbType.Int)).Value = VAL_
            OPENCONNECTION()
            CMD.ExecuteNonQuery()

        Catch ex As Exception
        Finally
            ' CLOSE_CON()
        End Try
    End Sub
    Public Function CHECK_ACTIV(GUID As String) As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECT ACTIVATION FROM TB_ACCOUNTS WHERE GUID='" & GUID & "'")
        If DT.Rows.Count > 0 Then
            NUMBER_CHECK = DT.Rows(0)("ACTIVATION").ToString
        End If
        Return DT
    End Function

End Module
