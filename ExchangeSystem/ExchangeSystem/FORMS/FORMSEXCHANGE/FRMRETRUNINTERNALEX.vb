Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraReports.UI

Public Class FRMRETRUNINTERNALEX
    Dim clsaccsa As New CLSAccSafeActivity
    Dim MODREST As Decimal
    Dim RBRTYPE, DBRTYPE, IsConfirm, IsCash As Integer
    Dim AccFrom As ULong
    Public OvarAllVall1 As Double

    Sub CALCULATERESULT()
        If GVROLE.RowCount > 0 Then

        End If
    End Sub
    Public Sub DVGFormat()

        'GVROLE.OptionsBehavior.EditingMode = True
        GVROLE.OptionsBehavior.Editable = True
        GVROLE.OptionsBehavior.ReadOnly = False
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVROLE.OptionsView.ShowGroupPanel = False
        GVROLE.OptionsFind.AlwaysVisible = True
        GVROLE.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVROLE.Columns.Count - 1
            GVROLE.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVROLE.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVROLE.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        GVROLE.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVROLE.OptionsView.EnableAppearanceEvenRow = True
        GVROLE.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVROLE.OptionsView.EnableAppearanceOddRow = True
        GridLocalizer.Active = New MyGridLocalizer()
    End Sub
    Sub LOADBRANCH()
        If InternalExCH.Checked = True Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PR(0).Value = BID
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LoadDataToGetType", PR)
        End If
    End Sub
    Sub LOADDATA()
        If InternalExCH.Checked = True Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PR(0).Value = BID
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("InternalEx_LOADTOFINISHCANCEL", PR)
            If DT.Rows.Count > 0 Then
                GCROLE.DataSource = DT
                DVGFormat()
                GVROLE.Columns("RBName").Visible = False
                GVROLE.Columns("DBName").Visible = False
                GVROLE.Columns("RecievedCurrencyID").Visible = False
                GVROLE.Columns("BRRID").Visible = False
                GVROLE.Columns("BRDID").Visible = False
                IsCash = DT.Rows(0)("IsCash")
                AccFrom = DT.Rows(0)("AccFrom")
            End If
        Else
            GCROLE.DataSource = Nothing
        End If
    End Sub

    Private Sub FRMRETRUNINTERNALEX_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DVGFormat()
        LOADDATA()
    End Sub

    Private Sub GVROLE_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 100, 102), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub



    Private Sub GVROLE_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GVROLE.RowCellStyle
        Dim View As GridView = TryCast(sender, GridView)
        If e.Column.FieldName Is "OverallVal" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("OverallVal"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(46, 139, 87)
                e.Appearance.BackColor2 = Color.FromArgb(46, 139, 87)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "ExVal" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ExVal"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(46, 139, 87)
                e.Appearance.BackColor2 = Color.FromArgb(46, 139, 87)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "ExtraComission" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ExtraComission"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(220, 20, 60)
                e.Appearance.BackColor2 = Color.FromArgb(220, 20, 60)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "NetTotal" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("NetTotal"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(220, 20, 60)
                e.Appearance.BackColor2 = Color.FromArgb(220, 20, 60)
                e.Appearance.ForeColor = Color.FromArgb(255, Color.White)
            End If
        End If
        If e.Column.FieldName Is "ConfirmCol" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ConfirmCol"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
    End Sub
    Private Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim GetBranchVal As Decimal
        LOADBRANCH()

        If GVROLE.RowCount > 0 Then
            If GVROLE.GetFocusedRow("ExtraComission") > 0.000 Then
                If BID <> MAINBID Then
                    MODREST = GVROLE.GetFocusedRow("ExVal") - GVROLE.GetFocusedRow("ExtraComission")
                    GetBranchVal = GVROLE.GetFocusedRow("ExtraComission") / 2
                Else
                    GetBranchVal = GVROLE.GetFocusedRow("ExtraComission")
                End If
            End If
        End If
        Dim iscode As Object = GVROLE.GetFocusedRowCellValue("Code")
        Dim brid As Object = GVROLE.GetFocusedRowCellValue("SenderName")
        Dim RPH1 As Object = GVROLE.GetFocusedRowCellValue("SPhone")
        Dim inval As Object = GVROLE.GetFocusedRowCellValue("OverallVal")
        Dim xval As Object = GVROLE.GetFocusedRowCellValue("ExVal")
        Dim bdid As Object = GVROLE.GetFocusedRowCellValue("ExtraComission")
        Dim BRRID As Object = GVROLE.GetFocusedRowCellValue("BRRID")
        Dim BRDID As Object = GVROLE.GetFocusedRowCellValue("BRDID")
        'Dim IsCash As Int32 = Convert.ToInt32(GVROLE.GetFocusedRowCellValue("IsCash"))
        OvarAllVall1 = GVROLE.GetFocusedRowCellValue("OverallVal") + GVROLE.GetFocusedRowCellValue("ExVal") - GVROLE.GetFocusedRowCellValue("ExtraComission")
        If IsCash = 0 Then
            If AccFrom = 0 Or AccFrom > 0 Then
                GETSAFEVAL(UserAccID, BID, 1)
                If inval + xval > SAFEVAL Then
                    XtraMessageBox.Show(lookFeelError, "رصيد الخزنة لا يسمح بتسليم الحوالة", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dim PR(7) As SqlParameter
        PR(0) = New SqlParameter("@INSERTDATE", SqlDbType.Date) With {.Value = Date.Now}
        PR(1) = New SqlParameter("@CODE", SqlDbType.NVarChar, -1) With {.Value = iscode}
        PR(2) = New SqlParameter("@SenderName", SqlDbType.NVarChar, (150)) With {.Value = brid}
        PR(3) = New SqlParameter("@SPhone", SqlDbType.NVarChar, (50)) With {.Value = RPH1}
        PR(4) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = inval}
        PR(5) = New SqlParameter("@Exval", SqlDbType.Decimal) With {.Value = xval}
        PR(6) = New SqlParameter("@ExtraCommission", SqlDbType.Decimal) With {.Value = bdid}
        PR(7) = New SqlParameter("@ModeRest", SqlDbType.Decimal) With {.Value = MODREST}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CanceledInternalExTb_Insert", PR)
        DT = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & BRRID & "'")
        RBRTYPE = DT.Rows(0)("BranchType")
        Dim DT1 As New DataTable
        DT1.Clear()
        If BRDID <> 0 Then
            DT1 = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & BRDID & "'")
            DBRTYPE = DT1.Rows(0)("BranchType")
        End If
        Dim DisStatus As Boolean
        If GVROLE.GetFocusedRow("ExtraComission") > 0.000 Then
            DisStatus = True
        Else
            DisStatus = False
        End If
        If DBRTYPE <> 3 Then
            clsaccsa.AccSafeActivityTb_CancelInternalEx(UserID, Date.Now, "", iscode, inval, xval, bdid, DisStatus)
            If clsaccsa.IsDelivared = True Then
                LOADDATA()
                Exit Sub
            End If
        End If
        If DBRTYPE = 3 Then
            clsaccsa.AccSafeActivityTb_CancelDeliveredInternalEx(UserID, Date.Now, "", iscode, inval, xval, bdid, DisStatus)
            If clsaccsa.IsDelivared = True Then
                LOADDATA()
                Exit Sub
            End If
        End If
        '======================================================
        RUN_EXUTE_TXT("Update InternalEx Set ConfirmCanceledDate = '" & Date.Now.ToString("yyyy-MM-dd") & "',ConfirmCanceledSafeID = " & UserID & ",ConfirmCancelBranch=" & BID & ", IsConfirmed=7,IsCanceled= 7,ConfirmCanceled=0 Where Code =N'" & iscode & "'")
        '=========================================================
        Dim info As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim info1 As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim brD As String = GVROLE.GetRowCellDisplayText(info.RowHandle, GVROLE.Columns("DBName"))
        Dim brR As String = GVROLE.GetRowCellDisplayText(info1.RowHandle, GVROLE.Columns("RBName"))
        Dim DCID As Object = GVROLE.GetFocusedRowCellValue("RecievedCurrencyID")
        Dim TotalNet As Decimal = inval + xval - bdid
        print(iscode)
        GCROLE.DataSource = Nothing
        LOADDATA()
        CONFIRMMESSAGE.LBLTEXT.Text = "تمت عملية ترجيع الحوالة بنجاح"
        CONFIRMMESSAGE.ShowDialog()
        FrmFollowingCanceledInternalEx.GCROLE.DataSource = Nothing
        FrmFollowingCanceledInternalEx.GVROLE.Columns.Clear()
        FrmFollowingCanceledInternalEx.LOADDATA()
        FrmConfirmCanceledInternalEx.GCROLE.DataSource = Nothing
        FrmConfirmCanceledInternalEx.GVROLE.Columns.Clear()
        FrmConfirmCanceledInternalEx.LOADDATA()
        SEndForCanselMassg(iscode)
    End Sub
    Public Sub SEndForCanselMassg(ISID As String)
        Dim dt As New DataTable

        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = ISID
        dt.Clear()
        dt = RUN_QUARY_PRO("GET_colmens_InternalEx", PRM)
        If dt.Rows.Count > 0 Then
            RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, dt.Rows(0)("BranchDeliveredID"))
            Dim messg As String = My.Settings.Combny_name & vbNewLine
            messg &= "تم تسليم الحوالة الملغيه" & vbNewLine &
                    "CODE : " & ISID & vbNewLine &
                    "إلى السيد/ة :  " & dt.Rows(0)("SenderName") & vbNewLine &
                "شكراً لتعاملكم معنا"
            WATSAPPMsAG(dt.Rows(0)("SPhone1"), messg, False)

        End If



    End Sub


    Sub print(Code As Object)
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", Code)
            Dim ds As New DataSet
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_InternalEx_LOADTOFINISHCANCEL", PRM)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                dt.TableName = "InternalEx"
                ds.Tables.Add(dt)

                Dim report As New RPTCancleInternalEx2
                report.DataSource = ds
                report.DataMember = "InternalEx"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "رمز الحوالة خطأ يرجى التأكد من البيانات", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class