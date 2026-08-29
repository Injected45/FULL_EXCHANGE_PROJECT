Imports System.Data.SqlClient

Public Class FrmDeleteRecord
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If YesNoMessage(Me, "تأكيد", "هل تريد الحذف؟") Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar) With {.Value = Code.Text.Trim}
            PRM(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}

            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("AccSafeActivityTb_DeleteRecord", PRM)

            If DT.Rows.Count > 0 Then
                Dim msg As String = DT.Rows(0)("Message").ToString()
                MessageBox.Show(msg)
            End If
        Else
            InfoMessage(Me, "رسالة معلومات", "تم الخروج من الإجراء ولم يتم حذف السجل")
        End If

    End Sub
End Class