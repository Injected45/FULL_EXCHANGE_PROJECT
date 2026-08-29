Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid

Public Class ViewAccessProfile
    Public Sub LoadData()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("UserAccessProfileName_Select")
        If DT.Rows.Count > 0 Then
            GCRole.DataSource = DT
        End If
    End Sub

    Private Sub ViewAccessProfile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        LoadData()
        GVRole.OptionsBehavior.Editable = False
    End Sub

    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.Click
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            Dim CO As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("الرمز"))
            'FrmAccessProfile.NEWRECORD()

            Dim frm = New FrmAccessProfile(Convert.ToInt32(view.GetFocusedRowCellValue("الرمز")))
            FrmAccessProfile.BtnSave.Enabled = False
            FrmAccessProfile.BtnEdit.Enabled = True
            FrmAccessProfile.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            FrmAccessProfile.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            'FrmAccessProfile.FrmAccessProfile(CO)
            'frm.Show()
            'FrmAccessProfile.SHOW_RECORD(CO)
            'FrmAccessProfile.IsUpdate = True
            frm.ShowDialog()
        End If
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'FrmAccessProfile.IsUpdate = False
        FrmAccessProfile.ShowDialog()
    End Sub
End Class