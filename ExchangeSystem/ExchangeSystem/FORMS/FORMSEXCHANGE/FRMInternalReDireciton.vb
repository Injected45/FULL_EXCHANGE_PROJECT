Imports System.Data.SqlClient
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities.IO

Public Class FRMInternalReDireciton

    Sub LoadData()
        GCROLE.DataSource = Nothing
        Dim DT As New DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        LoadToControlar(GCROLE, "InternalEx_ReDireciton", "", "", PRM)
        GVROLE.ShowFindPanel()
    End Sub

    Private Sub FRMInternalReDireciton_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GCROLE.DataSource = Nothing
        BranchID.EditValue = -1
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit", "BName", "DBRID", Nothing)
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
    End Sub

    Private Sub BtnRedirection_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles BtnRedirection.ButtonClick
        FRMINTERNALTRANSFER.ConfirmType = 11
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
        FRMINTERNALTRANSFER.NEWRECORD()
        FRMINTERNALTRANSFER.ShowCurrentRecord(iscode)
        FRMINTERNALTRANSFER.ShowDialog()
        LoadData()
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GCROLE.DataSource = Nothing
    End Sub
End Class