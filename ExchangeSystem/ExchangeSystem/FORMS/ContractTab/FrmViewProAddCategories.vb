Public Class FrmViewProAddCategories
    Private Sub FrmViewProAddCategories_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GCRole.DataSource = Nothing
        LoadToControlar(GCRole, "CONDB_CategoriesTb_Select", "", "", Nothing)
    End Sub
End Class