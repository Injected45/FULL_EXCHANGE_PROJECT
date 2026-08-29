Imports System.Data.SqlClient

Public Class ADDFatherExpeens
    Private Sub ADDFatherExpeens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        FatherName.Text = ""
        LB.DataSource = Nothing
        loadata()
    End Sub

    Sub loadata()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("FatherExpensesTb_LOADTOLSBOX")
        If DT.Rows.Count > 0 Then
            LB.DataSource = DT
            LB.ValueMember = "AccCode"
            LB.DisplayMember = "AccName"
        End If
    End Sub

    Public Overrides Sub Save()
        If FatherName.Text = "" Then
            FatherName.ErrorText = "الرجاء إدخال هذا الحقل"
            Exit Sub
        End If
        MsgBox("هل تريد حفظ هذا البيان", MsgBoxStyle.YesNo)
        If MsgBoxResult.No = True Then Exit Sub
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1) With {.Value = FatherName.Text.Trim}
        RUN_EXUTE_PRO("FatherExpensesTb_Insert", prm)
        FrmSavedSuccessfully.Show()
        loadata()
        FatherName.Text = ""
        MyBase.Save()
    End Sub
End Class