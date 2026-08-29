Imports System.Data.SqlClient

Public Class FRMAGNETBENEFITGROUPS
    Public IsUpdate As Boolean
    Sub NEWRECORD()
        IsUpdate = False
        CodeID.Text = GET_LAST_RECORD("AGNETBENEFITGROUP", "ID") + 1
        SEARCHTXT.Text = ""
        GRNAME.Text = ""
        AGNETRATE.EditValue = 0.000
        SECONDPARTYRATE.EditValue = 0.000
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If GRNAME.Text = "" Then
                GRNAME.ErrorText = "هذا الحقل مطلوب"
                GRNAME.Select()
                Exit Sub
            End If
            If AGNETRATE.EditValue = 0.000 Then
                AGNETRATE.ErrorText = "هذا الحقل لا يجب أن تكون قيمته صفر"
                AGNETRATE.Select()
                Exit Sub
            End If
            If SECONDPARTYRATE.EditValue = 0.000 Then
                SECONDPARTYRATE.ErrorText = "هذا الحقل لا يجب أن تكون قيمته صفر"
                SECONDPARTYRATE.Select()
                Exit Sub
            End If

            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@GCODE", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
            PR(1) = New SqlParameter("@GRNAME", SqlDbType.NVarChar, -1) With {.Value = GRNAME.Text.Trim}
            PR(2) = New SqlParameter("@AGNETRATE", SqlDbType.Decimal) With {.Value = AGNETRATE.EditValue}
            PR(3) = New SqlParameter("@SECONDPARTYRATE", SqlDbType.Decimal) With {.Value = SECONDPARTYRATE.EditValue}
            RUN_QUARY_PRO("AGNETBENEFITGROUP_INSERT", PR)
        End If
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If GRNAME.Text = "" Then
                GRNAME.ErrorText = "هذا الحقل مطلوب"
                GRNAME.Select()
                Exit Sub
            End If
            If AGNETRATE.EditValue = 0.000 Then
                AGNETRATE.ErrorText = "هذا الحقل لا يجب أن تكون قيمته صفر"
                AGNETRATE.Select()
                Exit Sub
            End If
            If SECONDPARTYRATE.EditValue = 0.000 Then
                SECONDPARTYRATE.ErrorText = "هذا الحقل لا يجب أن تكون قيمته صفر"
                SECONDPARTYRATE.Select()
                Exit Sub
            End If

            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@GCODE", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text.Trim}
            PR(1) = New SqlParameter("@GRNAME", SqlDbType.NVarChar, -1) With {.Value = GRNAME.Text.Trim}
            PR(2) = New SqlParameter("@AGNETRATE", SqlDbType.Decimal) With {.Value = AGNETRATE.EditValue}
            PR(3) = New SqlParameter("@SECONDPARTYRATE", SqlDbType.Decimal) With {.Value = SECONDPARTYRATE.EditValue}
            RUN_QUARY_PRO("AGNETBENEFITGROUP_UPDATE", PR)
        End If
        '===============================================
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_RECROD_TO_UPDATE()
        If SEARCHTXT.Text <> String.Empty Then
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = SEARCHTXT.Text}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AGNETBENEFITGROUP_SEARCH", PRM)
            If DT.Rows.Count > 0 Then
                CodeID.Text = DT.Rows(0)("GCODE").ToString
                GRNAME.Text = DT.Rows(0)("GRNAME").ToString
                AGNETRATE.EditValue = DT.Rows(0)("AGNETRATE")
                SECONDPARTYRATE.EditValue = DT.Rows(0)("SECONDPARTYRATE")
                IsUpdate = True
                BtnSave.Enabled = False
                BtnEdit.Enabled = True
            End If
        End If
    End Sub

    Private Sub SEARCHTXT_KeyDown(sender As Object, e As KeyEventArgs) Handles SEARCHTXT.KeyDown
        If e.KeyCode = Keys.Enter Then
            SHOW_RECROD_TO_UPDATE()
            e.Handled = True
        End If
    End Sub

    Private Sub SEARCHTXT_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles SEARCHTXT.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            SHOW_RECROD_TO_UPDATE()
        End If
    End Sub

    Private Sub FRMAGNETBENEFITGROUPS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub
End Class