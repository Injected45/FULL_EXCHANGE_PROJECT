Public Class FRMCurrencyMovements
    Public frm = New USRCurrencyMovements
    Public TypeID As Int32
    Private Sub FRMCurrencyMovements_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnRefreish.PerformClick()
    End Sub


    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        frm = New USRCurrencyMovements
        frm.Dock = DockStyle.Fill
        PanelDOck.Controls.Clear()
        PanelDOck.Controls.Add(frm)
        frm.TYPElock.SelectedIndex = 1
        frm.BanksTb_LODE()
        CurrencyPriceShow(TypeID)
    End Sub

    Public Sub CurrencyPriceShow(TypeID As Int32)
        GridControl1.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("CurrencyPriceShow")
        If dt.Rows.Count > 0 Then
            GridControl1.DataSource = dt
        Else

        End If
        dt.Dispose()
    End Sub

    Private Sub BarButtonItem10_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnRefreish.ItemClick
        CurrencyPriceShow(TypeID)
        frm = New USRCurrencyMovements
        frm.Dock = DockStyle.Fill
        PanelDOck.Controls.Clear()
        PanelDOck.Controls.Add(frm)
        frm.NOWrecored()
    End Sub


End Class