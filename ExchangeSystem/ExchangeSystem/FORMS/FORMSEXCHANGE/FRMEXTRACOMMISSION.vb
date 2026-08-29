Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient

Public Class FRMEXTRACOMMISSION
    Public BRANCHRID As Integer
    Public LoadType As Integer
    Public OverallTotal, TransValue As Double
    Sub NEWRECORD()
        CodeID.Text = ""
        ExVal.EditValue = -1
        ExtraCommission.EditValue = 0.000
    End Sub
    Sub LOADINTERNAISID()
        CodeID.Text = FRMCONFIRMISSUED.GVROLE.GetFocusedRowCellValue("Code")
        ExVal.EditValue = FRMCONFIRMISSUED.GVROLE.GetFocusedRowCellValue("ExVal")
        BRANCHRID = FRMCONFIRMISSUED.GVROLE.GetFocusedRowCellValue("BranchRecievedID")
    End Sub
    Sub LOADFROMREQUEST()
        Dim DT1 As New DataTable
        DT1.Clear()
        DT1 = RUN_QUARY_TXT("Select Code,ExVal,BranchRecievedID,OverallVal from InternalEx where Code='" & FrmCancelRequest.ISIDID.Text & "'")
        If DT1.Rows.Count > 0 Then
            CodeID.Text = DT1.Rows(0)("Code").ToString
            BRANCHRID = DT1.Rows(0)("BranchRecievedID")
            ExVal.EditValue = DT1.Rows(0)("ExVal")
            OverallTotal = DT1.Rows(0)("OverallVal")
        End If
    End Sub
    Sub LOADINTERNAISIDFORCANCELDELIVERED()
        CodeID.Text = FrmConfirmAgentCanceled.GVROLE.GetFocusedRowCellValue("Code")
        ExVal.EditValue = FrmConfirmAgentCanceled.GVROLE.GetFocusedRowCellValue("ExVal")
        BRANCHRID = FrmConfirmAgentCanceled.GVROLE.GetFocusedRowCellValue("BranchRecievedID")

    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        If ExtraCommission.EditValue = 0.000 Then
            ExtraCommission.ErrorText = "قيمة الخصم لا يجب أن تكون صفر"
            Exit Sub
        End If
        TransValue = OverallTotal + ExVal.EditValue
        If ExtraCommission.EditValue > TransValue Then
            XtraMessageBox.Show(lookAndFeelError, "قيمة الخصم لا يجب أن تكون أكبر من قيمة الحوالة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If ExtraCommission.EditValue > ExVal.EditValue Then

            Dim result = XtraMessageBox.Show(lookAndFeelError, "قيمة الخصم أكبر من قيمة العمولة، هل تريد المتابعة؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.No Then
                Exit Sub
            End If
        End If


        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = Date.Now}
        PRM(1) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text}
        PRM(2) = New SqlParameter("@ExVal", SqlDbType.Decimal) With {.Value = ExVal.EditValue}
        PRM(3) = New SqlParameter("@BranchID", SqlDbType.Decimal) With {.Value = BRANCHRID}
        PRM(4) = New SqlParameter("@ExtraCommission", SqlDbType.Decimal) With {.Value = ExtraCommission.EditValue}
        PRM(5) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = UserID}

        RUN_EXUTE_PRO("ExtraCommissionTb_Insert", PRM)
        FRMCONFIRMISSUED.DiscountVal = ExtraCommission.EditValue
        FrmConfirmAgentCanceled.DiscountVal = ExtraCommission.EditValue
        FrmConfirmAgentCanceled.DiscountST = True
        FRMCONFIRMISSUED.DiscountStatus = True
        sEnFRoRElode(CodeID.Text)
        NEWRECORD()
        'If ExtraCommission.EditValue > 0.000 Then
        '    Dim GetBranchVal As Decimal = ExtraCommission.EditValue / 2
        '    empacc.INSERTTB_ACCEMPACTIVITY(CodeID.Text, UserID, 0.000, GetBranchVal, Date.Now, "حصة الفرع مقابل خصم من عمولة مرجعة", CodeID.Text, True, 1, 7, 1, BID)
        '    bracc.INSERTTB_BRANCHACTIVITY(CodeID.Text, UserID, 0.000, GetBranchVal, Date.Now, "حصة الفرع مقابل خصم من عمولة مرجعة", CodeID.Text, True, 1, 7, 1, BID, False, False)
        'End If
        FrmViewCanceledTransfer.GCRole.DataSource = Nothing
        FrmViewCanceledTransfer.LOADDATA()
        FrmSavedSuccessfully.Show()


        Me.Close()
    End Sub

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        Dim reslut = XtraMessageBox.Show("سيتم إلغاء خصم العمولة وحفظ التغييرات على أمر اعتماد الحوالة، هل تريد الاستمرار؟", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If reslut = DialogResult.No Then
            Exit Sub
        Else
            sEnFRoRElode(CodeID.Text)
            Me.Close()
        End If
    End Sub
    Public Sub sEnFRoRElode(ISID As String)
        RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, MAINBID)
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@code", SqlDbType.NVarChar, -1) With {.Value = ISID}
        dt = RUN_QUARY_PRO("GET_colmens_InternalEx", prm)

        If dt.Rows.Count > 0 Then
            RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, MAINBID)
            Dim dd As String = My.Settings.Combny_name & vbNewLine &
                          "CODE :" & Space(1) & dt.Rows(0)("Code") & vbNewLine &
                        "مكان التسليم :" & Space(1) & dt.Rows(0)("DeliveryPlace") & vbNewLine &
                           "مـ :" & Space(1) & dt.Rows(0)("RecievedName") & vbNewLine &
                            "هـ :" & Space(1) & dt.Rows(0)("RPhone1") & vbNewLine &
                            "القيمه :" & Space(1) & dt.Rows(0)("ExVal") & vbNewLine &
                            "العمولة :" & Space(1) & ExVal.Text & vbNewLine &
                            "للإستفسار هـ : " & Space(1) & sql_Mobile1 & vbNewLine &
                          "شكراً لتعاملكم معنا"

            WATSAPPMsAG(get_gruop_id(dt.Rows(0)("BranchDeliveredID")), dd, False)
            'WATSAPPMsAG(wtsabID_group_teset, dd)
        End If



    End Sub
    ''ارسال رسالة في مجموعة الوكيل لتبليغ بالحوالة الوكيل


    Private Sub FRMEXTRACOMMISSION_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'If LoadType = 0 Then
        '    LOADINTERNAISID()
        'ElseIf LoadType = 1 Then
        '    LOADINTERNAISIDFORCANCELDELIVERED()
        'ElseIf LoadType = 2 Then
        '    LOADFROMREQUEST()
        'End If
        'LOADINTERNAISIDFORCANCELDELIVERED()
        InsertDate.EditValue = Date.Now
        ExtraCommission.Select()
    End Sub
End Class