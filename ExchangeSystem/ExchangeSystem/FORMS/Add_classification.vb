Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraPrinting.Shape.Native
Imports Microsoft

Public Class Add_classification
    Sub NEWRECORD()
        New_Controlrs(Me)
        BranchID.EditValue = -1
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit2", "BName", "DBRID", Nothing)
        BranchID.Select()
        BtnSave.Enabled = True
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        'code.Text = GETMAXID("Classificationstable", "ID") + 1

    End Sub
    Private Sub Add_classification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub save()
        SetData()
        MyBase.Save()

    End Sub
    Public Overrides Sub SetData()

        Try
            If Not ValidateControl(code, "الرمز") Then Exit Sub
            If Not ValidateControl(BranchID, "الفرع") Then Exit Sub
            If Not ValidateControl(AseetType, "نوع التصنيف") Then Exit Sub
            If Not ValidateControl(CNAME, "اسم التصنيف") Then Exit Sub
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@keys", SqlDbType.NVarChar, -1) With {.Value = code.Text}
            PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(2) = New SqlParameter("@parent", SqlDbType.BigInt) With {.Value = AseetType.EditValue}
            PR(3) = New SqlParameter("@name", SqlDbType.NVarChar, -1) With {.Value = CNAME.Text}
            RUN_QUARY_PRO("Classifications_insertTP", PR)
            XtraMessageBox.Show("تم الحفظ بنجاح.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            NEWRECORD()

        Catch ex As SqlClient.SqlException
            Select Case ex.State
                Case 101
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case 100
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case Else
                    XtraMessageBox.Show("خطأ غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
        End Try
        MyBase.SetData()
    End Sub
    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        AseetType.Properties.DataSource = Nothing
        If BranchID.Text = String.Empty Then Exit Sub
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@ExType", SqlDbType.TinyInt) With {.Value = 1}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_PRO("FatherExpensesTb_LOADTOLKPBasedOnExType", PR)
        If DTT.Rows.Count > 0 Then
            AseetType.Properties.DataSource = DTT
            AseetType.Properties.ValueMember = "AccCode"
            AseetType.Properties.DisplayMember = "AccName"
            AseetType.Properties.ShowHeader = False
        End If
    End Sub

End Class