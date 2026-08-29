Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraRichEdit.Model

Public Class FRMAGENTREDIRECTION
    Dim OldeAgent As Integer
    Public HandallExVal, HandallExVal2 As Decimal
    Public ISHandallEX As Boolean
    Sub NEWRECORD()
        GCROLE.DataSource = Nothing
        GVROLE.Columns.Clear()
        DVGFormat()
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVROLE.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(0, 91, 150), e.Bounds)
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
    Public Sub DVGFormat()
        GVROLE.OptionsBehavior.Editable = True
        GVROLE.OptionsBehavior.ReadOnly = False
        GVROLE.Columns("Code").OptionsColumn.AllowEdit = False
        GVROLE.Columns("InsertDate").OptionsColumn.AllowEdit = False
        GVROLE.Columns("SenderName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("RecievedName").OptionsColumn.AllowEdit = False
        GVROLE.Columns("OverallVal").OptionsColumn.AllowEdit = False
        GVROLE.Columns("ExVal").OptionsColumn.AllowEdit = False
        GVROLE.Columns("BranchRecievedID").OptionsColumn.AllowEdit = False
        GVROLE.OptionsFind.AlwaysVisible = False
        GVROLE.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVROLE.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
        GVROLE.OptionsView.ShowGroupPanel = False
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
        GVROLE.Columns("ConfirmCol").Width = 70
    End Sub
    Sub LOADBRNACH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BtnBranchRecieved.DataSource = DT
        BtnBranchRecieved.ValueMember = "DBRID"
        BtnBranchRecieved.DisplayMember = "BName"
        BtnBranchRecieved.PopulateColumns()
        BtnBranchRecieved.Columns("DBRID").Visible = False
        BtnBranchRecieved.Columns("BranchType").Visible = False
        BtnBranchRecieved.ShowHeader = False
    End Sub
    Sub LOADDELIVERYBRANCH()
        Dim bdid As Integer = FrmConfirmAgentCanceled.GVROLE.GetFocusedRowCellValue("BranchRecievedID")
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmConfirmAgentCanceled.GVROLE.GetFocusedRowCellValue("BranchRecievedID")}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CoBranches_LoadToLKPWITHAGENT", PR)
        BtnBranchDeliveredID.DataSource = DT
        BtnBranchDeliveredID.ValueMember = "DBRID"
        BtnBranchDeliveredID.DisplayMember = "BName"
        BtnBranchDeliveredID.ShowHeader = False
        BtnBranchDeliveredID.PopulateColumns()
        BtnBranchDeliveredID.Columns("DBRID").Visible = False
        BtnBranchDeliveredID.Columns("BranchType").Visible = False
    End Sub
    Private Sub FRMAGENTREDIRECTION_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'NEWRECORD()
    End Sub
    Sub LOADDATA()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = FrmConfirmAgentCanceled.GVROLE.GetFocusedRowCellValue("Code")}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_AgentRedirection", PR)
        If DT.Rows.Count > 0 Then
            GCROLE.DataSource = DT
            LOADBRNACH()
            LOADDELIVERYBRANCH()
            DVGFormat()
        End If

        OldeAgent = GVROLE.GetFocusedRowCellValue("BranchDeliveredID")
    End Sub
    Public Sub AccSafeActivityTb_Redirection()
        '1
        Dim prm(12) As SqlParameter
        prm(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = Date.Now}
        prm(2) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = ""}
        prm(3) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = GVROLE.GetFocusedRowCellValue("Code")}
        prm(4) = New SqlParameter("@OverallVal", SqlDbType.Decimal) With {.Value = GVROLE.GetFocusedRowCellValue("OverallVal")}
        prm(5) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = GVROLE.GetFocusedRowCellValue("ExVal")}
        prm(6) = New SqlParameter("@SecondPrm", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        prm(7) = New SqlParameter("@FirstPrm", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(8) = New SqlParameter("@SecondAgent", SqlDbType.Int) With {.Value = GVROLE.GetFocusedRowCellValue("BranchDeliveredID")}
        prm(9) = New SqlParameter("@RecievedName", SqlDbType.NVarChar, -1) With {.Value = GVROLE.GetFocusedRowCellValue("RecievedName")}
        prm(10) = New SqlParameter("@ISHandallEX", SqlDbType.Bit) With {.Value = ISHandallEX}
        prm(11) = New SqlParameter("@HandallExVal", SqlDbType.Decimal) With {.Value = HandallExVal}
        prm(12) = New SqlParameter("@HandallExVal2", SqlDbType.Decimal) With {.Value = HandallExVal2}
        RUN_EXUTE_PRO("AccSafeActivityTb_AgentConfirmRedirection", prm)

        'If prm(9).Value = 0 Then

        'End If
    End Sub




    Public Sub rsEnFRoRElode(ISID As String)
        ''ارسال رسالة في مجموعة الوكيل لتبليغ بالحوالة الوكيل

        RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, MAINBID)
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = ISID}
        dt = RUN_QUARY_PRO("GET_colmens_InternalEx", prm)

        If dt.Rows.Count > 0 Then
            Dim dd As String
            'If IStype = 0 Then





            'Else
            dd = My.Settings.Combny_name & vbNewLine &
                    "تم إلغاء الحوالة" & vbNewLine &
                             "CODE :" & Space(1) & dt.Rows(0)("Code") & vbNewLine &
                               "مـ :" & Space(1) & dt.Rows(0)("RecievedName") & vbNewLine &
                                "القيمه :" & Space(1) & Cur_Code("دينار ليبي", dt.Rows(0)("OverallVal"), True, "n2") & vbNewLine &
                                 "للإستفسار هـ : " & Space(1) & sql_Mobile1 & vbNewLine &
                                  "شكراً لتعاملكم معنا"

            WATSAPPMsAG(get_gruop_id(dt.Rows(0)("BranchDeliveredID")), dd, False)
            'End If



        End If






    End Sub

    Private Sub BtnConfirm_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles BtnConfirm.ButtonClick
        Dim DBRTYPE As Integer = GVROLE.GetFocusedRowCellValue("DBRTYPE")
        Dim RBRTYPE As Integer = GVROLE.GetFocusedRowCellValue("RBRTYPE")
        Dim CellValue As Object = GVROLE.GetFocusedRowCellValue("BranchDeliveredID")
        If CellValue = 0 Then
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim reuslt = XtraMessageBox.Show(lookAndFeelError, "مكان التسليم لا يجب أن يكون فارغاً", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If OldeAgent = GVROLE.GetFocusedRowCellValue("BranchDeliveredID") Then
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim reuslt = XtraMessageBox.Show(lookAndFeelError, "يجب عليك إختيار الوجهة الجديدة", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        rsEnFRoRElode(GVROLE.GetFocusedRowCellValue("Code"))
        If (RBRTYPE = 1 Or RBRTYPE = 2) And DBRTYPE = 3 Then
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim reuslt2 = XtraMessageBox.Show(lookAndFeelError, "هل ترغب في إدخال العمولة بشكل يدوي؟", "تحديد العمولة", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
            If reuslt2 = DialogResult.Yes Then
                ISHandallEX = 1
                ExValSahreByHand.IsAgintToAgint = False
                ExValSahreByHand.LayoutControlItem2.Text = "عمولة الوكيل"
                ExValSahreByHand.LayoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                ExValSahreByHand.NEWRECORD()
                ExValSahreByHand.LoadRedrecitonExVal()
                ExValSahreByHand.ShowDialog()
                sEnFRoRElode(GVROLE.GetFocusedRowCellValue("Code"), GVROLE.GetFocusedRowCellValue("BranchDeliveredID"), 0, 1, HandallExVal, HandallExVal2)
            Else
                ISHandallEX = 0
            End If
        End If
        If RBRTYPE = 3 And DBRTYPE = 3 Then
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim reuslt2 = XtraMessageBox.Show(lookAndFeelError, "هل ترغب في إدخال العمولة بشكل يدوي؟", "تحديد العمولة", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
            If reuslt2 = DialogResult.Yes Then
                ISHandallEX = 1
                ExValSahreByHand.IsAgintToAgint = True
                ExValSahreByHand.LayoutControlItem2.Text = "الوكيل الراسل"
                ExValSahreByHand.LayoutControlItem6.Text = "الوكيل المسلم"
                ExValSahreByHand.LayoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                ExValSahreByHand.NEWRECORD()
                ExValSahreByHand.LoadRedrecitonExVal()
                ExValSahreByHand.ShowDialog()
                sEnFRoRElode(GVROLE.GetFocusedRowCellValue("Code"), GVROLE.GetFocusedRowCellValue("BranchDeliveredID"), 1, 1, HandallExVal, HandallExVal2)


            Else
                ISHandallEX = 0
            End If
        End If

        AccSafeActivityTb_Redirection()
        FrmSavedSuccessfully.Show()
        Me.Close()
        FrmConfirmAgentCanceled.GCROLE.DataSource = Nothing
        FrmConfirmAgentCanceled.LOADDATA()
    End Sub




End Class