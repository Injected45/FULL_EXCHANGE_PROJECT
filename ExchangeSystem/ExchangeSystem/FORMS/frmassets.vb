Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraSpellChecker

Public Class frmassets
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
        codee.Text = ""
        BranchID.EditValue = BID

    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Private Sub frmassets_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
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
        AseetType_EditValueChanged(Nothing, Nothing)
    End Sub

    Private Sub AseetTy_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles AseetTy.ButtonClick
        If e.Button.Index = 1 Then
            Add_classification.ShowDialog()
        End If
    End Sub
    Public Overrides Sub SetData()
        Try
            If Not ValidateControl(codee, "الرمز") Then Exit Sub
            If Not ValidateControl(BranchID, "الفرع") Then Exit Sub
            If Not ValidateControl(AseetType, "نوع الاصل") Then Exit Sub
            If Not ValidateControl(asyttname, "اسم الاصل") Then Exit Sub
            If Not ValidateControl(AseetTy, " التصنيف") Then Exit Sub
            Dim PR(4) As SqlParameter
            PR(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = codee.Text}
            PR(1) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            PR(2) = New SqlParameter("@parent", SqlDbType.BigInt) With {.Value = AseetType.EditValue}
            PR(3) = New SqlParameter("@clsf", SqlDbType.Int) With {.Value = AseetTy.EditValue}
            PR(4) = New SqlParameter("@clsname", SqlDbType.NVarChar, -1) With {.Value = asyttname.Text}
            RUN_QUARY_PRO("AseetT_typeinsertTP", PR)
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

    Private Sub AseetType_EditValueChanged(sender As Object, e As EventArgs) Handles AseetType.EditValueChanged
        AseetTy.Properties.DataSource = Nothing
        If BranchID.Text = String.Empty Then Exit Sub
        If AseetType.Text = String.Empty Then Exit Sub
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@parent", SqlDbType.BigInt) With {.Value = AseetType.EditValue}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        LoadToControlar(AseetTy, "ClassificationstableselectTP", "name", "ID", PR)
    End Sub

    Private Sub AseetTy_EditValueChanged(sender As Object, e As EventArgs) Handles AseetTy.EditValueChanged
        codee.Text = ""
        If AseetTy.Text = String.Empty Then Exit Sub
        If AseetTy.EditValue = Nothing Then Exit Sub
        codee.Text = GetLKPColumnVal(AseetTy, "keys") + "_" + (GetLKPColumnVal(AseetTy, "MaxID") + 1).ToString
    End Sub
End Class