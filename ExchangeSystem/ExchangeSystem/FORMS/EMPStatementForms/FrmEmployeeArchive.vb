Public Class FrmEmployeeArchive

    Public Sub NewRecord()
        New_Controlrs(Me)
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit2", "BName", "DBRID", Nothing, True)
        BranchID.EditValue = BID
    End Sub

    Private Sub FrmEmployeeArchive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        If Not ValidateControl(BranchID, "الفرع") Then Exit Sub
        LoadToControlar(GridControl1, "Employee_Archive", "", "", Nothing)
    End Sub
End Class