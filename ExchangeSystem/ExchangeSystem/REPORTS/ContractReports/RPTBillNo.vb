Imports DevExpress.XtraReports.UI

Public Class RPTBillNo
    Private Sub RPTBillNo_BeforePrint(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Me.BeforePrint

        Dim action As Integer = CInt(Me.Parameters("pAction").Value)

        Dim header As XRTableCell = xrOverallTotalHeader
        Dim detail As XRTableCell = xrOverallTotalDetail

        If action = 3 Then
            header.Visible = False
            detail.Visible = False
            XrTableCell4.Text = "اسم المشروع"

            XrLabel7.Visible = False
            XrLabel5.Visible = False

        ElseIf action = 5 Then
            header.Visible = True
            detail.Visible = True
            XrTableCell4.Text = "اسم المورد"

            XrLabel7.Visible = True
            XrLabel5.Visible = True
        End If

    End Sub

End Class