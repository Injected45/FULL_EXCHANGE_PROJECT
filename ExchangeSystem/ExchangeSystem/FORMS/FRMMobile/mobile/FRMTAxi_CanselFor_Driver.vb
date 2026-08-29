
Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base

Public Class FRMTAxi_CanselFor_Driver
    Sub DVGFormat()

        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = True
        GVRole.ShowFindPanel()
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True

        ' GridLocalizer.Active = New MyGridLocalizer()
    End Sub

    Private Sub FRMTAxi_OK_NOW_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFormat()
        GET_Deteels_fOr_taxe(BID)

    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Public Sub GET_Deteels_fOr_taxe(DeliveryPlaceID As ULong)
        Try


            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@DeliveryPlaceID", SqlDbType.Int) With {.Value = DeliveryPlaceID}
            prm(1) = New SqlParameter("@ConfirmType", SqlDbType.Int) With {.Value = 10}
            GridControl1.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("GET_Deteels_fOr_taxe", prm)
            If dt.Rows.Count = Nothing Then Return
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
            Else
                GridControl1.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub MAplinke_Click(sender As Object, e As EventArgs) Handles MAplinke.Click



        ' استدعاء دالة رسم المسار بين النقاط
        CheckAndDrawRoute()

    End Sub

    Public Async Sub CheckAndDrawRoute()
        Try

            ' الحصول على القيم من GVRole مع التحقق من كونها ليست فارغة أو Null
            Dim longitudeBranch As Object = GVRole.GetFocusedRowCellValue("longitude_branchID")
            Dim latitudeBranch As Object = GVRole.GetFocusedRowCellValue("Latitude_branchID")
            Dim longitudeClient As Object = GVRole.GetFocusedRowCellValue("loge")
            Dim latitudeClient As Object = GVRole.GetFocusedRowCellValue("lat")
            Dim clientName As String = GVRole.GetFocusedRowCellValue("RecievedName")
            Dim Branch_name As String = GVRole.GetFocusedRowCellValue("BName")
            Dim phone As String = GVRole.GetFocusedRowCellValue("RPhone1")


            If longitudeBranch IsNot Nothing AndAlso latitudeBranch IsNot Nothing AndAlso
           longitudeClient IsNot Nothing AndAlso latitudeClient IsNot Nothing AndAlso
           Not String.IsNullOrEmpty(clientName) Then

                'If MessageBox.Show("هل تريد فتح موقع العميل علي خرائط قوقل", "رسالة تنبيية", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim customerMapLink As String = $"https://www.google.com/maps/dir/?api=1&origin={latitudeClient},{longitudeClient}&destination={longitudeBranch},{latitudeBranch}&travelmode=driving"


                Dim frm As New FrmWWW_Wep()
                frm.Lode(customerMapLink)
                frm.ShowDialog()
                'Process.Start(New ProcessStartInfo(customerMapLink) With {.UseShellExecute = True})
            Else
                ' إذا كانت القيم فارغة أو Null، اعرض رسالة للمستخدم
                ErrorMessage2("البيانات غير كاملة، يرجى التحقق من القيم المدخلة.", "erorr_msg")

            End If

        Catch ex As Exception
            ErrorMessage2("البيانات غير كاملة، يرجى التحقق من القيم المدخلة.", ex.Message)
        End Try
    End Sub

    Public Sub Rollback_Drivers_Taxi(code As String)

        Try

            SplashScreenManager1.ShowWaitForm()

            Dim prm(3) As SqlParameter
            prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = code}
            prm(1) = New SqlParameter("@ACCID_safe", SqlDbType.Int) With {.Value = UserAccID}
            prm(2) = New SqlParameter("@MSg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(3) = New SqlParameter("@MAsge", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("Rollback_Drivers_Taxi", prm)
            If prm(2).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                ErrorMessage(Me, " رسالة تننية ", prm(3).Value)
            Else
                SplashScreenManager1.CloseWaitForm()
                FrmSavedSuccessfully.Show()
            End If

            GET_Deteels_fOr_taxe(BID)
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage(Me, "Rollback_Drivers_Taxi ex.Message ", ex.Message)
        End Try
    End Sub

    Private Sub resend_date_Click(sender As Object, e As EventArgs) Handles resend_date.Click


        If MessageBox.Show("هل تريد استرجاع هذه الحوالة", "رسالة تنبية", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
            Rollback_Drivers_Taxi(GVRole.GetFocusedRowCellValue("Code"))
        Else
            ErrorMessage(Me, "رسالة الغاء", "تمت عملية الالغاء بنجاح")
        End If


    End Sub
End Class