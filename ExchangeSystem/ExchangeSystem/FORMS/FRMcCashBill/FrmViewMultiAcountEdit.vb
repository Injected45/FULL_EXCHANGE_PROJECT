Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmViewMultiAcountEdit
    Private Sub FrmViewMultiAcountEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GCRole.DataSource = Nothing
        LoadToControlar(GCRole, "MultiAcountEditTB_Select", "", "", Nothing)
    End Sub

    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            FrmMultiAcountEdit.IsUpdate = True
            FrmMultiAcountEdit.GCRole.DataSource = Nothing
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CO}
            LoadToControlar(FrmMultiAcountEdit.GCRole, "MultiAcountEditDetailsTB_LoadToDVG", "", "", prm)
            FrmMultiAcountEdit.Code.Text = CO
            FrmMultiAcountEdit.DateEdit11.EditValue = view.GetFocusedRowCellValue("التاريخ")
            FrmMultiAcountEdit.MovmentType.Text = view.GetFocusedRowCellValue("وصف العملية").ToString
            FrmMultiAcountEdit.BtnSave.Enabled = False
            Enable_Controls(FrmMultiAcountEdit, False)
            FrmMultiAcountEdit.SimpleButton11.Enabled = True
        End If
        Me.Close()
    End Sub
End Class