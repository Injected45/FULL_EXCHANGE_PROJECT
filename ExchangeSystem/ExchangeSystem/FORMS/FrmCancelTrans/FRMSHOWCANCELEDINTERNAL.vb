Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class FRMSHOWCANCELEDINTERNAL
    Dim clsaccsa As New CLSAccSafeActivity
    Dim MODREST As Decimal
    Dim UpdateType, DBRID, RBRTYPE, DBRTYPE, ConfirmTRAS As Integer

    Private Sub FRMSHOWCANCELEDINTERNAL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        DVGFormat()
        LOADDATA()
    End Sub
    'تعريف متغير وعمل صب للفرق بين قيمة العمولة وقيمة الخصم
    ' حفظ في خزنة الفرع: قيمة الحوالة في الدائن وقيمة العمولة المتبقية في الدائن وقيمة المتغير في المدين والباقي يذهب للرئيسي في الدائن
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
        GVROLE.Columns("RBName").Visible = False
        GVROLE.Columns("DBName").Visible = False
        GVROLE.Columns("RecievedCurrencyID").Visible = False

    End Sub
    Sub LOADBRANCH()

        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CoBranches_LoadDataToGetType", PR)
        If DT.Rows.Count > 0 Then
            RBRTYPE = DT.Rows(0)("BranchType")
        End If
    End Sub
    Sub LOADDATA()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PR(0).Value = BID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_LOADTOFINISHCANCEL", PR)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            DVGFormat()
        Else
            GCROLE.DataSource = Nothing
        End If
        If GVROLE.RowCount > 0 Then
            GVROLE.Columns("RBName").Visible = False
            GVROLE.Columns("DBName").Visible = False
            GVROLE.Columns("RecievedCurrencyID").Visible = False
        End If
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
        If e.Column.FieldName Is "ConfirmCol" Then
            View.GetRowCellDisplayText(e.RowHandle, View.Columns("ConfirmCol"))
            If View.RowCount > 0 Then
                e.Appearance.BackColor = Color.FromArgb(245, 222, 179)
                e.Appearance.BackColor2 = Color.FromArgb(245, 222, 179)
            End If
        End If
    End Sub
    Dim inscls As New CLSINTERNALTRANSFER
    Dim empacc As New CLSACCEMPACTIVITY
    Dim bracc As New CLSBRANCHACTIVITY
    Dim IsConfirm As Integer
    Private Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
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


        clsaccsa.AccSafeActivityTb_CancelInternalEx(UserID, Date.Now, "ألغاء حوالة داخلية صادرة", iscode, inval, xval, bdid, True)
        If clsaccsa.IsDelivared = True Then
            LOADDATA()
            Exit Sub
        End If
        If clsaccsa.IsDelivared = 1 Then Exit Sub
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
        'clsaccsa.AccSafeActivityTb_CancelInternalEx(UserID, Date.Now, "", iscode, inval, xval, bdid, True)
        '======================================================
        RUN_EXUTE_TXT("Update InternalEx Set ConfirmCanceledDate = '" & Date.Now.ToString("yyyy-MM-dd") & "',ConfirmCanceledSafeID = " & UserID & ",ConfirmCancelBranch=" & BID & ", IsConfirmed=7,IsCanceled= 7,ConfirmCanceled=0 Where Code =N'" & iscode & "'")
        '======================================================
        RUN_EXUTE_TXT("Update InternalExValues Set ConfirmCanceledDate = '" & Date.Now.ToString("yyyy-MM-dd") & "',ConfirmCanceledSafeID = " & UserID & ",ConfirmCancelBranch=" & BID & ", IsCanceled= 7,ConfirmCanceled=0 Where ISID =N'" & iscode & "'")
        '=======================================================
        RUN_EXUTE_TXT("Update BenefitDistribution Set IsActive=0, IsCanceled=1 Where OperationTypeID=6 and ISID =N'" & iscode & "'")
        '=========================================================
        RUN_EXUTE_TXT("Update MainBRBenefitsBBranchesTB Set IsActive=0,IsCanceled=1 Where OperationTypeID=13 and ISID =N'" & iscode & "'")
        '=========================================================
        Dim info As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim info1 As GridHitInfo = GVROLE.CalcHitInfo(GCROLE.PointToClient(Cursor.Position))
        Dim brD As String = GVROLE.GetRowCellDisplayText(info.RowHandle, GVROLE.Columns("DBName"))
        Dim brR As String = GVROLE.GetRowCellDisplayText(info1.RowHandle, GVROLE.Columns("RBName"))
        Dim DCID As Object = GVROLE.GetFocusedRowCellValue("RecievedCurrencyID")
        Dim TotalNet As Decimal = inval + xval - bdid

        If bdid > 0.000 Then
            If BID <> MAINBID Then



                Dim PRM1(8) As SqlParameter
                PRM1(0) = New SqlParameter("@ISID", iscode)
                PRM1(1) = New SqlParameter("@InsertDate", Date.Now)
                PRM1(2) = New SqlParameter("@RBID", BID)
                PRM1(3) = New SqlParameter("@RBVal", GetBranchVal)
                PRM1(4) = New SqlParameter("@ISIDType", 1)
                PRM1(5) = New SqlParameter("@OperationTypeID", 7)
                PRM1(6) = New SqlParameter("@SafeID", UserID)
                PRM1(7) = New SqlParameter("@BRType", RBRTYPE)
                PRM1(8) = New SqlParameter("@CurrencyID", DCID)
                RUN_EXUTE_PRO("BenefitDistribution_INSERTRECEIVEDBRANCH", PRM1)


                Dim PRM2(9) As SqlParameter
                PRM2(0) = New SqlParameter("@InsertDate", Date.Now)
                PRM2(1) = New SqlParameter("@ISID", iscode)
                PRM2(2) = New SqlParameter("@MainVal", GetBranchVal)
                PRM2(3) = New SqlParameter("@RBID", BID)
                PRM2(4) = New SqlParameter("@DVBID", MAINBID)
                PRM2(5) = New SqlParameter("@OverAllExVal", GVROLE.GetFocusedRow("ExVal"))
                PRM2(6) = New SqlParameter("@TypeID", 1)
                PRM2(7) = New SqlParameter("@SafeID", UserID)
                PRM2(8) = New SqlParameter("@OperationTypeID", 8)
                PRM2(9) = New SqlParameter("@CurrencyID", DCID)
                RUN_EXUTE_PRO("MainBRBenefitsCanceledIntenalTB_Insert", PRM2)

            Else

                Dim PRM3(8) As SqlParameter
                PRM3(0) = New SqlParameter("@ISID", iscode)
                PRM3(1) = New SqlParameter("@InsertDate", Date.Now)
                PRM3(2) = New SqlParameter("@RBID", BID)
                PRM3(3) = New SqlParameter("@RBVal", GetBranchVal)
                PRM3(4) = New SqlParameter("@ISIDType", 1)
                PRM3(5) = New SqlParameter("@OperationTypeID", 7)
                PRM3(6) = New SqlParameter("@SafeID", UserID)
                PRM3(7) = New SqlParameter("@BRType", RBRTYPE)
                PRM3(8) = New SqlParameter("@CurrencyID", DCID)
                RUN_EXUTE_PRO("BenefitDistribution_INSERTRECEIVEDBRANCH", PRM3)


            End If
        ElseIf bdid = 0.000 Then

        End If
        GCROLE.DataSource = Nothing
        LOADDATA()
        CONFIRMMESSAGE.LBLTEXT.Text = "تمت عملية ترجيع الحوالة بنجاح"

    End Sub
End Class