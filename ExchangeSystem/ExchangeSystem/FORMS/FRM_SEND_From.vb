Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraReports.UI

Public Class FRM_SEND_From
    Dim report As New RPTEXTERNALFRM
    Public GroupID As String
    Public Sub NEwRecoreds()
        BranchDeliveredID.EditValue = -1
        branchID.EditValue = -1
        DT1.EditValue = Now.Date
        DT2.EditValue = Now.Date
        GridControl1.DataSource = Nothing
        LOADBRANCH()
        DVGFROMAT()
        BranchDeliveredID.EditValue = 0
        branchID.EditValue = BID
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 120)
    End Sub
    Private Sub FRM_SEND_From_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEwRecoreds()
    End Sub

    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Try
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
            prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)
            If dt.Rows.Count > 0 Then

                branchID.Enabled = dt.Rows(0)("Can_branch")
                If dt.Rows(0)("Can_branch") = 0 Then
                    LayoutControlItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                Else
                    LayoutControlItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                End If
            Else
                branchID.Enabled = False
                LayoutControlItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub LODEFrom_edit()
        BranchDeliveredID.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT.NewRow()
        DT.Columns.Add("DBRID", GetType(Integer))
        DT.Columns.Add("BName", GetType(String))
        DT = RUN_QUARY_PRO_ONLY("GET_FORBRnchIS")
        DT.Rows.Add(0, "كل الوكلاء")
        BranchDeliveredID.Properties.DataSource = DT
        BranchDeliveredID.Properties.ValueMember = "DBRID"
        BranchDeliveredID.Properties.DisplayMember = "BName"
    End Sub

    Sub DVGFROMAT()
        GVRole1.OptionsSelection.MultiSelectMode = False
        GVRole1.ShowFindPanel()
        GVRole1.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        For i As Integer = 0 To GVRole1.Columns.Count - 1
            GVRole1.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceCell.TextOptions.VAlignment = VertAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole1.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole1.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole1.Appearance.OddRow.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Try
            GridControl1.DataSource = Nothing
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchID.EditValue}
            LoadToControlar(GridControl1, "ExternalEx_LODEforSendWhats", "", "", prm)
            GVRole1.ShowFindPanel()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub GVRole1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub RepositoryItemButtonEdit1_Click(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.Click
        Dim SerT As Integer
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = GVRole1.GetFocusedRowCellValue("code")}
        PR(1) = New SqlParameter("@SerType", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("ExternalEx_LoadRecordToPrint1", PR)

        If DT.Rows.Count > 0 Then
            SerT = DT.Rows(0)("SerType")
            report.DataSource = DT
            report.DataMember = "ExternalEx"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.XrLabel48.Text = Cur_Code(DT.Rows(0)("العملة المسلمة"), DT.Rows(0)("قيمة الحوالة"), False, "n2")
            report.XrLabel15.Text = Cur_Code(DT.Rows(0)("العملة المستلمة"), DT.Rows(0)("القيمة"), False, "n2")
            If DT.Rows(0)("IsInOrOut") = 0 Then
                report.XrLabel7.Text = "حوالات خارجية - صادرة"
            Else
                report.XrLabel7.Text = "حوالات خارجية -واردة"
            End If
            report.CreateDocument()
            report.ShowPreview


            Dim Phone_send = ""
            If SerT > 0 Then
                report.ServiceName.Visible = True
                report.ServiceVal.Visible = True
            Else
                report.ServiceName.Visible = False
                report.ServiceVal.Visible = False
            End If
        Else
            ErrorMessage(Me, "رسالة معلومات", "رمز الحوالة خطأ يرجى التأكد من البيانات")
        End If
    End Sub

    Private Sub RepositoryItemCheckEdit1_CheckedChanged(sender As Object, e As EventArgs) Handles RepositoryItemCheckEdit1.CheckedChanged


        If RepositoryItemCheckEdit1.ValueChecked = True Then
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@code", SqlDbType.NVarChar) With {.Value = GVRole1.GetFocusedRowCellValue("code")}
            RUN_EXUTE_PRO("Update_ISCick", prm)
        Else
            ErrorMessage(Me, "رسالة معلومات", "عذرا تم تاكيد هذه الحوالة مسبقاً")
        End If

    End Sub

    Sub LOADBRANCH()
        branchID.Properties.DataSource = Nothing
        LoadToControlar(branchID, "COBRANCHTB_LoadDataIntoLookUpEdit_FILL_pro", "BName", "DBRID", Nothing)

    End Sub

    Private Sub SenWhatsUp_Click(sender As Object, e As EventArgs) Handles SenWhatsUp.Click

        Dim info As GridHitInfo = GVRole1.CalcHitInfo(GridControl1.PointToClient(Cursor.Position))
        Dim iscode As Object = GVRole1.GetFocusedRowCellValue("Code")
        Dim Rphone As Object = GVRole1.GetFocusedRowCellValue("RPhone")
        Dim DeliveredVal As String = GVRole1.GetFocusedRowCellValue("CurrDeliveredVal")
        Dim SerType As String = GVRole1.GetFocusedRowCellValue("ServiceName")
        GroupID = GVRole1.GetFocusedRowCellValue("IDGroup").ToString
        Dim Txt As String
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchID.EditValue}
        Dim DT As New DataTable
        DT = RUN_QUARY_PRO("CoBranch_CheckBranchIsPartner", PR)
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("IsAttached") = True Then
                If SerType.Contains("يد") Then
                    Txt = "*شركة الرحالة للصرافة*" & vbNewLine & "نرجوا إفادتنا بشأن الحوالة رقم" & vbNewLine & "(" & iscode & ")" & vbNewLine & "بقيمة" & ":" & Space(1) & Cur_Code(GVRole1.GetFocusedRowCellValue("CuName").ToString, DeliveredVal, True, "n2")
                Else
                    Txt = "مطلوب سكرين" & vbNewLine & "رقم الهاتف: " & Rphone & vbNewLine & "قيمة الحوالة: " & Cur_Code(GVRole1.GetFocusedRowCellValue("CuName").ToString, DeliveredVal, True, "n2") & vbNewLine & "للغرفة" & ":" & Space(1) & GroupID
                End If
                Dim result As DialogResult = MessageBox.Show("هل تريد ارسال بيانات الحوالة رقم : " & vbNewLine & iscode, "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    WATSAPPMsAG("218911934764", Txt, True)
                    InfoMessage(Me, "رسالة معلومات", "تم ارسال البيانات بنجاح")
                Else
                    Exit Sub
                End If
            Else
                InfoMessage(Me, "رسالة معلومات", "عذرا لا يمكنك إرسال استفسار عن حوالة يرجى مراجعة الإدارة")
            End If
        End If
    End Sub


End Class