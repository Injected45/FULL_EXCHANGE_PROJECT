Imports DevExpress.CodeParser
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraSpellChecker
Imports DevExpress.XtraSplashScreen
Imports ExchangeSystem.ExchangeSystem.CLSFRM
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports TableDependency.SqlClient

Public Class FRMMAIN
    Dim R As New ResizeControls()
    'Public cluture = New System.Globalization.CultureInfo("ar")
    'Public people_table_dependency As SqlTableDependency(Of People)
    Public SelectType As Integer = 0, SalaryCalc As Integer

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        If FrmPayIncrease.Visible = True Then
            FrmPayIncrease.Close()
        Else
            FrmPayIncrease.MdiParent = Me
            FrmPayIncrease.Show()
        End If
    End Sub
    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        FrmCoBranch.ShowDialog()
    End Sub
    Sub CHECKINTERNALRECIEVED()
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If BID <> MAINBID Then
            'Dim PR(0) As SqlParameter
            'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            'PR(0).Value = BID
            'Dim DT As New DataTable
            'DT.Clear()
            'DT = RUN_QUARY_PRO("InternalEx_TransferPermission", PR)
            'If DT.Rows.Count > 0 Then
            '    If DT.Rows(0)("IsConfirmed") = 3 And DT.Rows(0)("IsCanceled") = 2 And DT.Rows(0)("IsConfirmCancel") = 4 And DT.Rows(0)("IsCanceledRequest") = 3 And DT.Rows(0)("ConfirmCanceled") = 1 Then
            '        XtraMessageBox.Show(lookAndFeelError, "لديك حوالة تم إلغاؤها ويجب اتمام عملية الترجيع", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        FRMRETRUNINTERNALEX.InternalExCH.Checked = True
            '        FRMRETRUNINTERNALEX.ExternalExCH.Checked = False
            '        FRMRETRUNINTERNALEX.ShowDialog()
            '    ElseIf DT.Rows(0)("IsConfirmed") = 4 And DT.Rows(0)("IsCanceled") = 3 And DT.Rows(0)("IsConfirmCancel") = 2 And DT.Rows(0)("IsCanceledRequest") = 2 And DT.Rows(0)("ConfirmCanceled") = 1 Then
            '        XtraMessageBox.Show(lookAndFeelError, "يوجد حوالة داخلية تم الموافقة على إلغاؤها يرجى الانتظار حتى يتم التأكيد من غرفة التحكم", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    ElseIf DT.Rows(0)("IsConfirmed") = 6 And DT.Rows(0)("IsCanceled") = 9 And DT.Rows(0)("IsCanceledRequest") = 5 And DT.Rows(0)("IsConfirmCancel") = 6 And DT.Rows(0)("ConfirmCanceled") = 3 Then
            '        XtraMessageBox.Show(lookAndFeelError, "لديك حوالة تم رفض إلغاؤها ويجب اتمام عملية التسليم", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        FRMRETRUNINTERNALEX.InternalExCH.Checked = True
            '        FRMRETRUNINTERNALEX.ExternalExCH.Checked = False
            '        FrmInternalExDeliveredAfterConfirmCancel.ShowDialog()
            '    ElseIf DT.Rows(0)("IsConfirmed") = 2 And DT.Rows(0)("IsCanceled") = 1 And DT.Rows(0)("IsConfirmCancel") = 1 And DT.Rows(0)("IsCanceledRequest") = 1 And DT.Rows(0)("ConfirmCanceled") = 1 Then

            '        XtraMessageBox.Show(lookAndFeelError, "لديك طلب إلغاء حوالة وسيتم نقلك لشاشة الحوالات الملغية لاىخاذ الإجراء", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        FrmViewCanceledTransfer.ShowDialog()
            '    End If
            'Else
            If BID = MAINBID Then
                FRMINTERNALTRANSFER.BranchRecievedID.Enabled = True
            Else
                FRMINTERNALTRANSFER.BranchRecievedID.Enabled = False
            End If
            FRMINTERNALTRANSFER.BtnNew.PerformClick()
            FRMINTERNALTRANSFER.ShowDialog()
            'End If
        Else
            If BID = MAINBID Then
                FRMINTERNALTRANSFER.BranchRecievedID.Enabled = True
            Else
                FRMINTERNALTRANSFER.BranchRecievedID.Enabled = False
            End If
            FRMINTERNALTRANSFER.BtnNew.PerformClick()
            FRMINTERNALTRANSFER.ShowDialog()
        End If
    End Sub
    Sub CHECKINTERNALDELIVERED()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_TransferPermissionDeliveredBranch", PR)
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("IsConfirmed") = 6 And DT.Rows(0)("IsCanceled") = 0 And DT.Rows(0)("IsCanceledRequest") = 5 And DT.Rows(0)("IsConfirmCancel") = 6 Then
                ErrorMessage(Me, "رسالة معلومات", "لديك حوالة تم إلغاؤها ويجب اتمام عملية الترجيع")
                FRMRETRUNINTERNALEX.ShowDialog()
            End If
        ElseIf DT.Rows.Count = 0 Then
            FRMINTERNALTRANSFER.BtnNew.PerformClick()
            FRMINTERNALTRANSFER.ShowDialog()
        End If
    End Sub

    Private Sub BarButtonItem7_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem7.ItemClick
        FRMBENEFITGROUPS.ShowDialog()
    End Sub
    Private Sub BarButtonItem9_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        FRMAGNETBENEFITGROUPS.ShowDialog()
    End Sub

    Private Sub BGPBranches_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BGPBranches.ItemClick
        FRMBENEFITGROUPS.ShowDialog()
    End Sub

    Private Sub BGPAgents_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BGPAgents.ItemClick
        FRMAGNETBENEFITGROUPS.ShowDialog()
    End Sub


    Private Sub BarButtonItem21_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem21.ItemClick
        FrmCancelRequest.ShowDialog()
    End Sub
    Private Sub BtnViewCanceledTransfers_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnViewCanceledTransfers.ItemClick
        FrmViewCanceledTransfer.ShowDialog()
    End Sub
    Private Sub BtnCancelRequest_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnCancelRequest.ItemClick
        FrmCancelRequest.ShowDialog()
    End Sub
    Private Sub BarButtonItem25_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem25.ItemClick
        FRMVIEWCURRENCY.ShowDialog()
    End Sub
    Private Sub BtnTransBetweenSafes_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub
    Private Sub BarButtonItem27_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem27.ItemClick
        CurrencySafeTransfer.ShowDialog()
    End Sub

    Private Shared _scAccesses As List(Of ScreensAccessProfile)
    Public Shared ReadOnly Property SAccesses As List(Of ScreensAccessProfile)
        Get
            Return _scAccesses
        End Get
    End Property
    Public Shared Sub OpenFormByName(ByVal name As String)
        Dim frm As Form = Nothing
        Dim ins = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(Function(x) x.Name = name)
        If ins IsNot Nothing Then
            frm = TryCast(Activator.CreateInstance(ins), Form)
            If Application.OpenForms(frm.Name) IsNot Nothing Then
                frm = Application.OpenForms(frm.Name)
            Else
            End If
            frm.BringToFront()
        End If
        If frm IsNot Nothing Then
            frm.Name = name
            OpenForm(frm)
        End If
    End Sub
    Public Shared Sub OpenForm(ByVal frm As Form, ByVal Optional OpenInDialog As Boolean = False)
        Dim screen = Session.ScreensAccesses.SingleOrDefault(Function(x) x.ScreenName = frm.Name)
        If screen IsNot Nothing Then
            If screen.CanOpen = True Then
                If OpenInDialog Then
                    frm.ShowDialog()
                Else
                    frm.Show()
                End If
                Return
            Else
                XtraMessageBox.Show(text:="غير مصرح لك ", caption:="", icon:=MessageBoxIcon.[Error], buttons:=MessageBoxButtons.OK)
                Return
            End If
        End If
    End Sub
    Public WithEvents watcher As New IdleWatcher
    Public mmLoginWaiting As Double = 14
    Public ssLoginWaiting As Double = 60
    Public ffLoginWaiting As String
    Private Sub FRMMAIN_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Refreshtimer()
    End Sub
    'Private Sub watcher_idle(sender As Object) Handles watcher.Idle
    '    ' If login.show is modal, then you don't need to call MainForm.Close()
    '    ' This will depend on your implementation
    '    'If FrmWaitingLogin.Visible = True Then FrmWaitingLogin.Visible = False




    'End Sub

    Public Sub LoadFormEvent()
        SplashScreenManager1.ShowWaitForm()
        Get_Acctvionpc(Me)
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        'CheckForIllegalCrossThreadCalls = False
        BtnCurrencyStatement.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BarButtonItem72.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        ResizeControls.SubResize(Me, 50, 70)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.CenterToScreen()
        Me.WindowState = FormWindowState.Maximized

        LoadCompanySettings_lode()
        FrmLogin.ShowDialog()
        BtnBranchName.Caption = GetBranchName
        BtnUserName.Caption = GetUserName
        BtnCNNAME.Caption = CNNAME
        BtnCTNAME.Caption = CTNAME
        BtnDate.Caption = Date.Now.ToString("yyyy/MM/dd")
        Timer3.Enabled = True

        RibbonControl1.SelectPage(RP2)
    End Sub
    Private Sub FRMMAIN_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            OPENCONNECTION()

            Dim vrg As Integer
            vrg = ChickUpdate(vrg)
            If vrg = 0 And SQLCON.ConnectionString <> "Data Source=102.214.165.242,55910;Initial Catalog = EXCHANGESYS2026; Persist Security Info=True;User ID = sa; Password=123456789;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024" Then
                SplashScreenManager2.ShowWaitForm()
                UpdateApp()

            Else
                Me.Focus()
                LoadFormEvent()
            End If
            'End If
            If BarButtonItem130.Caption = " 2026 تجريب منظومة الي فووق داتا سنتر" Then
                BrnClearFrom.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            Else
                BrnClearFrom.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Timer3_Tick(sender As Object, e As System.EventArgs) Handles Timer3.Tick
        BtnTime.Caption = Date.Now.ToString("T")
    End Sub
    Private Sub FRMMAIN_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        Me.StartPosition = FormStartPosition.CenterScreen
        R.ResizeControls()
    End Sub
    Private Sub BtnConfirmCanceled_Click(sender As Object, e As System.EventArgs) Handles BtnConfirmCanceled.Click
        If BarButtonItem113.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FRMCONFIRMISSUED.RB3.Checked = True
            FRMCONFIRMISSUED.RB3_CheckedChanged(Nothing, Nothing)
            FRMCONFIRMISSUED.ShowDialog()
        End If

    End Sub
    Public t1 As Threading.Thread
    Public t2 As Threading.Thread

    Private Sub Timer2_Tick(sender As Object, e As System.EventArgs) Handles Timer2.Tick

        Dim isOtherFormOnTop As Boolean = False
        For Each f As Form In Application.OpenForms
            If f IsNot Me AndAlso f.Visible AndAlso f.WindowState <> FormWindowState.Minimized Then
                isOtherFormOnTop = True
                Exit For
            End If
        Next
        If isOtherFormOnTop Then
            Exit Sub
        Else
            refresh_table(BID)
            If BagWo.IsBusy Then
                BagWo.RunWorkerAsync()
            End If
        End If
    End Sub

    Private Sub backgroundworker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BagWo.DoWork
        Try
            Thread.Sleep(1000)
            Timer2.Start()
            refresh_table(BID)
        Catch ex As Exception
            MessageBox.Show("Error:" & vbLf & vbLf & ex.Message, "System", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
    Private Sub XtraForm2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        BagWo.RunWorkerAsync()
    End Sub
    Private Sub BagWo_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BagWo.RunWorkerCompleted

        SQLCON.Dispose()
        QSCON.Dispose()

        Application.Exit()
    End Sub
    Private Sub BtnConfirm_Click(sender As Object, e As System.EventArgs) Handles BtnConfirm.Click
        Try
            If BarButtonItem113.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
                FRMCONFIRMISSUED.RB1.Checked = True
                FRMCONFIRMISSUED.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Enum ChangeType
        None = 0
        Delete = 1
        Insert = 2
        Update = 3
    End Enum
    Private Class CSharpImpl
        <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
        Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
    Public Sub RefreshRecord()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", BID)
        Dim dt As DataTable = RUN_QUARY_PRO("ExchangeSystem_FastAnalyz", PRM)
        If dt.Rows.Count > 0 Then
            OutComeNotDelivered.Text = dt.Rows(0)("ReCount")
            OutComeDelivered.Text = dt.Rows(0)("ReCountNotDel")
            IntIncomeNotDel.Text = dt.Rows(0)("IntIncomeNotDel")
            InNotConfirmed.Text = dt.Rows(0)("IntNotConfirmed")
            RefuseCanceled.Text = dt.Rows(0)("RefuseCanceled")
            FollowingInteral.Text = dt.Rows(0)("RecordCount")
            ConfirmInternalExCancel.Text = dt.Rows(0)("ConfirmCanceledInternal")
        End If
    End Sub

    Private Sub BtnOutComeDelivered_Click(sender As Object, e As System.EventArgs) Handles BtnOutComeDelivered.Click
        'FrmInternalFastCall.SelectBtn = False
        SelectType = 3
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية صادرة مسلمة"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BtnOutComeNotDelivered_Click(sender As Object, e As System.EventArgs) Handles BtnOutComeNotDelivered.Click
        'FrmInternalFastCall.SelectBtn = False
        SelectType = 2
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية صادرة غير مسلمة"
        FrmInternalFastCall.OverAllEx.Visible = True
        FrmInternalFastCall.OverAllTotal.Visible = True
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BtnIntIncomeNotDel_Click(sender As Object, e As System.EventArgs) Handles BtnIntIncomeNotDel.Click
        'FrmInternalFastCall.SelectBtn = True
        SelectType = 4
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية واردة غير مسلمة"
        'FrmInternalFastCall.OverAllEx.Visible = True
        'FrmInternalFastCall.OverAllTotal.Visible = True
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub SimpleButton3_Click(sender As Object, e As System.EventArgs) Handles SimpleButton3.Click
        'FrmInternalFastCall.SelectBtn = False
        SelectType = 1
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية صادرة غير معتمدة"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BtnAgentCancelRequest_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAgentCancelRequest.ItemClick
        FRMInternalReDireciton.ShowDialog()
        'FrmConfirmAgentCanceled.ShowDialog()
    End Sub
    Private Sub SimpleButton51_Click(sender As Object, e As System.EventArgs) Handles SimpleButton51.Click
        If BarButtonItem113.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FrmLoadCanceledMainBranch.ShowDialog()
        End If
    End Sub
    Private Sub BtnIntIncomeNotDel1_Click(sender As Object, e As System.EventArgs) Handles BtnIntIncomeNotDel1.Click
        SelectType = 8
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية صادرة عليها طلب إلغاء"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BtnIntIncomeNotDel11_Click(sender As Object, e As System.EventArgs) Handles BtnIntIncomeNotDel11.Click
        SelectType = 5
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية واردة عليها طلب إلغاء"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BtnIntIncomeNotDel12_Click(sender As Object, e As System.EventArgs) Handles BtnRecordCountConfirmCancel.Click
        SelectType = 9
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية صادرة ملغاة قيد التسليم"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BtnOutcomeDeliveredInEx_Click(sender As Object, e As System.EventArgs) Handles BtnOutcomeDeliveredInEx.Click
        'FrmInternalFastCall.SelectBtn = False
        SelectType = 7
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية واردة مسلمة"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BTNADDDISCOUNTTYPE_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNADDDISCOUNTTYPE.ItemClick
        FRMADDDISCOUNTTYPE.NEWRECORD()
        FRMADDDISCOUNTTYPE.ShowDialog()
    End Sub
    Private Sub BtnAddBonusType_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddBonusType.ItemClick
        FrmPayIncrease.ShowDialog()
    End Sub
    Private Sub BtnSalaryViews_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnCustomerMovement.ItemClick
        FRmIDsql = 54
        FrmCustomerMovement.TypeNewRe = 0
        FrmCustomerMovement.ShowDialog()
    End Sub

    Private Sub BarButtonItem33_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnCoBranch.ItemClick
        FrmCoBranch.ShowDialog()
    End Sub
    Private Sub BarButtonItem35_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem35.ItemClick
        FRMVIEWCURRENCY.ShowDialog()
    End Sub
    Private Sub BarButtonItem32_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnCustomer.ItemClick
        FRMCUSTOMER.ShowDialog()
    End Sub
    Dim clsempwd As New CLSEMPWITHDRAWAL
    Private Sub BarButtonItem41_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem41.ItemClick
        FrmIncreaseLoadAllData.ShowDialog()
    End Sub
    Private Sub BarButtonItem43_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem43.ItemClick
        FrmDiscountsLoadAllData.ShowDialog()
    End Sub
    Private Sub BarButtonItem44_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem44.ItemClick
        FrmAdvancePaymentLoadAllData.ShowDialog()
    End Sub
    Private Sub BarButtonItem45_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem45.ItemClick
        FrmEMPORCUSTWITHDRAWALLoadAllData.ShowDialog()
    End Sub

    Private Sub BarButtonItem47_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem47.ItemClick
        FRMEMPCORRECTSLALRY.ShowDialog()
    End Sub
    Private Sub BarButtonItem31_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem31.ItemClick
        FRMLOADSALARIES.ShowDialog()
    End Sub

    Private Sub BarButtonItem52_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnCalcAllEmpSalary.ItemClick
        FRMSALARYCALCULATION.SalaryCalc = 1
        FRMSALARYCALCULATION.ShowDialog()
    End Sub
    Private Sub BarButtonItem54_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNEMPCORRECTSLALRY.ItemClick
        FRMEMPCORRECTSLALRY.SalaryCalc = 2
        FRMEMPCORRECTSLALRY.ShowDialog()
    End Sub
    Private Sub BarButtonItem53_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnINDIVDUALSALARYCALC.ItemClick
        FRMINDIVDUALSALARYCALC.SalaryCalc = 3
        FRMINDIVDUALSALARYCALC.ShowDialog()
    End Sub
    Private Sub BTNEMPADVANCEPAYMENT_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub
    Private Sub BarButtonItem57_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub
    Private Sub BarButtonItem56_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub
    Private Sub BarButtonItem63_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnIncreaseLoadAllData.ItemClick
        FRmIDsql = 75
        FrmIncreaseLoadAllData.ShowDialog()
    End Sub
    Private Sub BarButtonItem62_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnDiscountsLoadAllData.ItemClick
        FRmIDsql = 74
        FrmDiscountsLoadAllData.ShowDialog()
    End Sub
    Private Sub BtnAdvancePaymentLoadAllData_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAdvancePaymentLoadAllData.ItemClick
        FrmAdvancePaymentLoadAllData.ShowDialog()
    End Sub
    Private Sub BarButtonItem64_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNEMPORCUSTWITHDRAWALLoadAllData.ItemClick
        FRmIDsql = 76
        FrmEMPORCUSTWITHDRAWALLoadAllData.ShowDialog()
    End Sub
    Private Sub BTNLOADSALARIES_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNLOADSALARIES.ItemClick
        FRmIDsql = 72
        FRMLOADSALARIES.NEWRECORD()
        FRMLOADSALARIES.BranchID.EditValue = BID
        FRMLOADSALARIES.LOADEMP(BID)

        FRMLOADSALARIES.ShowDialog()
    End Sub
    Private Sub BarButtonItem65_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnIndividualSalaryEMP.ItemClick
        FRmIDsql = 67
        FrmIndividualSalaryEMP.SalaryCalc = 1
        FrmIndividualSalaryEMP.DataBaseType = 1
        FrmIndividualSalaryEMP.NEWRECORD()
        FrmIndividualSalaryEMP.ShowDialog()
    End Sub
    Private Sub BarButtonItem66_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem66.ItemClick
        FrmExpenses.ShowDialog()
    End Sub
    Private Sub BarButtonItem71_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnPettyCashStatement.ItemClick
        FRmIDsql = 59
        FRMSETTLEMENTSTATEMENT.ShowDialog()
    End Sub
    Private Sub BrnClearFrom_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BrnClearFrom.ItemClick
        If SQLCON.ConnectionString = "Data Source=93.158.238.134,1433\MSSQLSERVER;Initial Catalog = EXCHANGESYS; Persist Security Info=True;User ID = new_admin; Password=theartof1980@_coding;Connect Timeout=100000; Persist Security Info=True;Pooling=True; Max Pool Size=2024" Then
            ErrorMessage(Me, "رسالة خطأ", "عذرا لايمكن تنظيف قاعدة البيانات الرئيسية")
        End If
        If BarButtonItem130.Caption = "خارجي" Then
            ErrorMessage(Me, "رسالة خطأ", "عذرا لايمكن تنظيف قاعدة البيانات الرئيسية")
        End If
        Dim customIcon As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon
        Dim cusok As New MessageBoxButtons
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        XtraMessageBox.AllowHtmlText = True
        Dim resu = XtraMessageBox.Show("<Size=8><color=Black>سيتم مسح جميع البيانات، هل تريد الاستمرار؟</color>", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If resu = DialogResult.Yes Then
            Tables_CLEARDATA()
        Else
            Exit Sub
        End If
    End Sub
    Private Sub BtnCustomerPayment_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

        FRMEMPWITHDRAWAL.Frmid = 44
        FRMEMPWITHDRAWAL.Tag = 58
        FRMEMPWITHDRAWAL.Text = "سند صرف لعميل"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(6)
        FRMEMPWITHDRAWAL.LOADTYPE = 6
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem78_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMEMPWITHDRAWAL.Frmid = 49
        FRMEMPWITHDRAWAL.Tag = 61
        FRMEMPWITHDRAWAL.Text = "سند قبض من عميل"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(8)
        FRMEMPWITHDRAWAL.LOADTYPE = 8
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem75_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnEmpPayment.ItemClick
        FRMEMPWITHDRAWAL.Frmid = 43
        FRMEMPWITHDRAWAL.Tag = 57
        FRMEMPWITHDRAWAL.Text = "سند صرف لحساب"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(5)
        FRMEMPWITHDRAWAL.LOADTYPE = 5
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.isbanck = 0
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem77_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnEmpDeposit.ItemClick
        FRMEMPWITHDRAWAL.Frmid = 48
        FRMEMPWITHDRAWAL.Tag = 60
        FRMEMPWITHDRAWAL.Text = "سند إيداع لحساب"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(7)
        FRMEMPWITHDRAWAL.LOADTYPE = 7
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.isbanck = 0
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem79_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles EmployeeClassification.ItemClick
        FRMEmployeeClassification.ShowDialog()
    End Sub
    Private Sub BarButtonItem80_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles NATIONALITY.ItemClick
        FRMNATIONALITY.ShowDialog()
    End Sub
    Private Sub BarButtonItem83_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddBank.ItemClick
        FRMBANK.NEWRECORD()
        FRMBANK.ShowDialog()
    End Sub
    Private Sub BarButtonItem85_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnDelegate.ItemClick
        FRMDELEGATE.NEWRECORD()
        FRMDELEGATE.ShowDialog()
    End Sub
    Private Sub BarButtonItem84_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddBBranch.ItemClick
        FRMBBRANCH.ShowDialog()
    End Sub
    Private Sub BarButtonItem34_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnUserAccessTemplate.ItemClick
        ViewAccessProfile.ShowDialog()
    End Sub
    Private Sub BarButtonItem86_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNBANKDEPOSIT.ItemClick
        FRmIDsql = 83
        FRMBANKDEPOSIT.barDockControlTop.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnNew.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnDelete.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnEdit.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnPrint.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnSave.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.Tag = 63
        FRMBANKDEPOSIT.Text = "إيداع مصرفي"
        FRMBANKDEPOSIT.BtnNew.PerformClick()
        FRMBANKDEPOSIT.LOADTYPE = 16
        FRMBANKDEPOSIT.isbanck = 1
        FRMBANKDEPOSIT.ShowDialog()
    End Sub
    Private Sub BarButtonItem87_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRmIDsql = 84
        FRMBANKDEPOSIT.barDockControlTop.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnNew.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnDelete.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnEdit.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnPrint.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnSave.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.Tag = 64
        FRMBANKDEPOSIT.Text = "إيداع مصرفي في حساب عميل"
        FRMBANKDEPOSIT.BtnNew.PerformClick()
        FRMBANKDEPOSIT.LOADTYPE = 18
        FRMBANKDEPOSIT.ShowDialog()
    End Sub
    Private Sub BarButtonItem33_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNEMPBANKWITHDRAWAL.ItemClick
        FRmIDsql = 86
        FRMBANKDEPOSIT.barDockControlTop.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnNew.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnDelete.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnEdit.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnPrint.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnSave.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.Tag = 65
        FRMBANKDEPOSIT.Text = "سحب مصرفي"
        FRMBANKDEPOSIT.BtnNew.PerformClick()
        FRMBANKDEPOSIT.LOADTYPE = 17
        FRMBANKDEPOSIT.isbanck = 1
        FRMBANKDEPOSIT.ShowDialog()
    End Sub
    Private Sub BarButtonItem34_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

        FRmIDsql = 85
        FRMBANKDEPOSIT.barDockControlTop.Appearance.BackColor = System.Drawing.Color.Red
            FRMBANKDEPOSIT.BtnNew.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnDelete.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnEdit.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnPrint.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.BtnSave.Appearance.BackColor = System.Drawing.Color.Red
        FRMBANKDEPOSIT.Tag = 66
        FRMBANKDEPOSIT.Text = "سحب مصرفي من حساب عميل"
        FRMBANKDEPOSIT.BtnNew.PerformClick()
        FRMBANKDEPOSIT.LOADTYPE = 19
        FRMBANKDEPOSIT.ShowDialog()
    End Sub
    Private Sub BarButtonItem88_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnBBranchMovement.ItemClick

        FRMBANKBRANCHMOVEMENT.NEWRECORD()
        FRMBANKBRANCHMOVEMENT.ShowDialog()
    End Sub
    Private Sub BarButtonItem79_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles CompanyInfo.ItemClick
        FrmCoBranch.ShowDialog()
    End Sub
    Private Sub btnShowSafeMovement_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnShowSafeMovement.ItemClick
        Timer2.Stop()
        FrmShowSafeMovement.ShowDialog()
    End Sub
    Private Sub BTNPROFITS_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub
    Private Sub BtnMainSafeBalance_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnMainSafeBalance.ItemClick
        FrmMainSafeBalance.ShowDialog()
    End Sub
    Private Sub BarButtonItem5_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnCurrencyStatement.ItemClick
        FrmCurrencyMovement.ShowDialog()
    End Sub
    Private Sub BtnPettyCash_ItemClick_3(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnPettyCash.ItemClick
        FRMPettyCash.ShowDialog()
    End Sub
    Private Sub btnPettyCashSettlement_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPettyCashSettlement.ItemClick
        FRMPettyCashSettlement.ShowDialog()
    End Sub
    Private Sub AddExpenses_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles AddExpenses.ItemClick
        FrmExpenses.ShowDialog()
    End Sub
    Private Sub AddCurrency_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles AddCurrency.ItemClick
        CurrencyMain.ShowDialog()
    End Sub
    Private Sub BarButtonItem5_ItemClick_3(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddUser.ItemClick
        FRMADDUSER.ShowDialog()
    End Sub
    Private Sub BarButtonItem6_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnOpeningBalance.ItemClick
        FRMOPENINGBALANCE.NEWRECORD()
        FRMOPENINGBALANCE.ShowDialog()
    End Sub
    Private Sub BntSelectAccountsBetweenBranch_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

    End Sub
    Private Sub BtnANOTHEREXPENS_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnANOTHEREXPENS.ItemClick
        FRMANOTHEREXPENS.IsAseet = False
        FRMANOTHEREXPENS.Text = "مصروفات عمومية"
        FRMANOTHEREXPENS.ShowDialog()
    End Sub
    Private Sub BTNCURRENCYPRICE_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BTNCURRENCYPRICE.ItemClick
        FRMCURRENCYPRICE.ShowDialog()
    End Sub
    Private Sub BarButtonItem5_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        FRMBRANCHESRATES.ShowDialog()
    End Sub
    Private Sub BarButtonItem6_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        ALLEMPPRINT()
    End Sub
    Private Sub BarButtonItem72_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnExpenseStatement.ItemClick
        FRmIDsql = 60
        FRMEXPESESMOVEMENTTATEMENTS.ShowDialog()
    End Sub
    Private Sub BarButtonItem26_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem26.ItemClick
        FRmIDsql = 61
        FRMExpenseInquiry.ShowDialog()
    End Sub
    Private Sub BarButtonItem28_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem28.ItemClick
        FrmCompanyInfo.ShowDialog()
    End Sub
    Private Sub BarButtonItem29_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem29.ItemClick
        FRMCURRENCYPRICEDTTELSS.ShowDialog()
    End Sub
    Private Sub BarButtonItem32_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem32.ItemClick
        FRMINTCURSALES.ShowDialog()
    End Sub
    Private Sub BarButtonItem33_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem33.ItemClick
        Currencyonthebank2.ShowDialog()
    End Sub
    Private Sub BarButtonItem34_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem34.ItemClick
        FRMCurrencyMovements.ShowDialog()
    End Sub
    Private Sub BarButtonItem54_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem54.ItemClick
        FRMEMPWITHDRAWAL.Tag = 61
        FRMEMPWITHDRAWAL.Text = "سند ايداع نقد اجنبي لعميل"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(8)
        FRMEMPWITHDRAWAL.LOADTYPE = 8
        FRMEMPWITHDRAWAL.TYPEs = 2
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem55_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem55.ItemClick
        FRMEMPWITHDRAWAL.Frmid = 60
        FRMEMPWITHDRAWAL.Tag = 60
        FRMEMPWITHDRAWAL.Text = "سند ايداع نقد اجنبي لحساب"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(7)
        FRMEMPWITHDRAWAL.LOADTYPE = 7
        FRMEMPWITHDRAWAL.TYPEs = 2
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem56_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem56.ItemClick
        FRMEMPWITHDRAWAL.Frmid = 57
        FRMEMPWITHDRAWAL.Tag = 57
        FRMEMPWITHDRAWAL.Text = "سند صرف نقد اجنبي لحساب"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(5)
        FRMEMPWITHDRAWAL.LOADTYPE = 5
        FRMEMPWITHDRAWAL.TYPEs = 2
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem57_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem57.ItemClick
        FRMEMPWITHDRAWAL.Tag = 58
        FRMEMPWITHDRAWAL.Text = "سند صرف نقد اجنبي لعميل"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(6)
        FRMEMPWITHDRAWAL.LOADTYPE = 6
        FRMEMPWITHDRAWAL.TYPEs = 2
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem60_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem60.ItemClick
        FRMViewCurrencyPurchaseTransactions2.ShowDialog()
    End Sub
    Private Sub BarButtonItem61_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem61.ItemClick
        View_RecieveInternalEx.ShowDialog()
    End Sub
    Private Sub BarButtonItem62_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem62.ItemClick
        FRmIDsql = 55
        FRMCustomerAccountStatement.NOWRecored()
        FRMCustomerAccountStatement.BranchID.EditValue = BID
        FRMCustomerAccountStatement.CurrencyTo.EditValue = 1
        FRMCustomerAccountStatement.TypeMov.SelectedIndex = 0
        FRMCustomerAccountStatement.LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        FRMCustomerAccountStatement.Text = "كشف حسابات العملاء"

        FRMCustomerAccountStatement.ShowDialog()
    End Sub
    Private Sub BarButtonItem63_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem63.ItemClick
        FRmIDsql = 78
        FRMCustomerAccountStatement.NOWRecored()
        FRMCustomerAccountStatement.BranchID.EditValue = BID
        FRMCustomerAccountStatement.CurrencyTo.EditValue = 1
        FRMCustomerAccountStatement.TypeMov.SelectedIndex = 1
        FRMCustomerAccountStatement.Text = "كشف حسابات الموظفين"
        FRMCustomerAccountStatement.ShowDialog()
    End Sub
    Private Sub BarButtonItem71_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem71.ItemClick
        FRMBuycurrencyfromacustomer.ShowDialog()
    End Sub
    Private Sub BarButtonItem72_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem72.ItemClick
        FRMDepositSafe.ShowDialog()
    End Sub
    Private Sub CONMOXSHer_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CONMOXSHer.ButtonClick
        If CONMOXSHer.Text = String.Empty Then
            CONMOXSHer.ErrorText = "الرجاء ادخال رقم الكود"
            Return
        End If
        Try
            Dim dt As New DataTable
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
            PRM(0).Value = CONMOXSHer.Text
            dt.Clear()
            dt = RUN_QUARY_PRO("InternalEx_SearchForCurrentRecorde", PRM)
            If dt.Rows.Count = 0 Then
                ErrorMessage(Me, "رسالة خطأ", "لم يتم العثور على الحوالة, الرجاء التأكد من الكود")
                Exit Sub
            End If
            FRMINTERNALTRANSFER.ConfirmType = 12
            FRMINTERNALTRANSFER.NEWRECORD()
            FRMINTERNALTRANSFER.ShowCurrentRecord(CONMOXSHer.Text)
            FRMINTERNALTRANSFER.ShowDialog()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub
    Private Sub BarButtonItem75_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem75.ItemClick
        FRMCATEGORYTYPES.BtnNew.PerformClick()
        FRMCATEGORYTYPES.ShowDialog()
    End Sub
    Private Sub BarButtonItem77_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FROWNMCURRENCYPRICE.NEWRECORD()
        FROWNMCURRENCYPRICE.ShowDialog()
    End Sub
    Private Sub BarButtonItem79_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem79.ItemClick
        FRMBANKSERVICES.NEWRECORD()
        FRMBANKSERVICES.ShowDialog()
    End Sub
    Private Sub BtnRecordCountConfirmCancel1_Click(sender As Object, e As System.EventArgs) Handles BtnRecordCountDeliveredCancel.Click
        SelectType = 6
        FrmInternalFastCall.LOADDATA()
        FrmInternalFastCall.Text = "حوالات داخلية صادرة ملغاة مسلمة"
        FrmInternalFastCall.ShowDialog()
    End Sub
    Private Sub BarButtonItem84_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem84.ItemClick
        FRMBANKSERVICESTATEMENTS.ShowDialog()
    End Sub
    Private Sub BarButtonItem86_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem86.ItemClick
        NewBasicCurrencyPrcie.ShowDialog()
    End Sub
    Private Sub BarButtonItem87_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem87.ItemClick
        FRMCurrencySpeculation.ShowDialog()
    End Sub
    Private Sub BarButtonItem91_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem91.ItemClick
        FRmIDsql = 95
        FRMPAMNTWDMEMBER.LOADTYPE = 22
        FRMPAMNTWDMEMBER.Text = "سند قبض جمعية"
        FRMPAMNTWDMEMBER.ShowDialog()
    End Sub
    Private Sub BarButtonItem92_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem92.ItemClick
        FRmIDsql = 96
        FRMPAMNTWDMEMBER.LOADTYPE = 23
        FRMPAMNTWDMEMBER.Text = "سند صرف جمعية"
        FRMPAMNTWDMEMBER.ShowDialog()
    End Sub
    Private Sub BarButtonItem93_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem93.ItemClick
        FRMASSOCIATIONMOVEMENT.ShowDialog()
    End Sub
    Private Sub BarButtonItem95_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem95.ItemClick
        FRMMEMBERSLOADALL.ShowDialog()
    End Sub
    Private Sub BarButtonItem96_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMEMPWITHDRAWAL.Frmid = 51
        FRMEMPWITHDRAWAL.Tag = 61
        FRMEMPWITHDRAWAL.Text = "سند قبض في حساب وكيل"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(32)
        FRMEMPWITHDRAWAL.LOADTYPE = 32
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub
    Private Sub BarButtonItem97_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

        FRMEMPWITHDRAWAL.Frmid = 46
        FRMEMPWITHDRAWAL.Tag = 61
        FRMEMPWITHDRAWAL.Text = "سند صرف من حساب وكيل"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(33)
        FRMEMPWITHDRAWAL.LOADTYPE = 33
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem98_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem98.ItemClick
        FRMNEWCURRENCYETAILS.NEWRECORD()
        FRMNEWCURRENCYETAILS.ShowDialog()
    End Sub

    Private Sub BarButtonItem99_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem99.ItemClick
        NewCurrencyPriceUpdate.ShowDialog()
    End Sub
    Private Sub BarButtonItem103_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem103.ItemClick
        'FRMNEWCURRENCYSALE.PriceType.SelectedIndex = -1
        'FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 2
        'FRMNEWCURRENCYSALE.NEWRecored()
        'FRMNEWCURRENCYSALE.ShowDialog()
        FrmSaleCurrency2026.Frmid = 39
        FrmSaleCurrency2026.Type = 25
        FrmSaleCurrency2026.ShowDialog()
    End Sub

    Private Sub BarButtonItem104_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem104.ItemClick
        FRMLOADTRANSTOEDIT.ShowDialog()
    End Sub

    Private Sub BarButtonItem105_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem105.ItemClick
        FRMCONFIRMEDITTRANS.ShowDialog()
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As System.EventArgs) Handles SimpleButton1.Click

        If BarButtonItem105.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FRMCONFIRMEDITTRANS.ShowDialog()
        End If
    End Sub
    Private Sub BarButtonItem102_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem102.ItemClick
        'FRMNEWCURRENCYBUY.PriceType.SelectedIndex = -1
        'FRMNEWVIEWCURRENCYBUY.IsBuyORSale = 1
        'FRMNEWCURRENCYBUY.NEWRecored()
        'FRMNEWCURRENCYBUY.ShowDialog()
        FrmBuyCurrency2026.Frmid = 38
        FrmBuyCurrency2026.Type = 24
        FrmBuyCurrency2026.ShowDialog()
    End Sub

    Private Sub BarButtonItem108_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem108.ItemClick
        FRMSELECTACCOUNT.TransType = 1
        FRMEXTERNALTRANS.ConfirmType = 0
        FRMEXTERNALTRANS.NewRecord()
        FRMEXTERNALTRANS.ShowDialog()
    End Sub

    Private Sub BarButtonItem109_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem109.ItemClick
        FrmAccountsTree.ShowDialog()
    End Sub

    Private Sub BtnAddCancelReason_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddCancelReason.ItemClick
        FrmAddCancelReason.ShowDialog()
    End Sub

    Private Sub BarButtonItem112_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem112.ItemClick
        FRMSELECTACCOUNT.TransType = 0
        FRMINTERNALTRANSFER.ConfirmType = 0
        FRMINTERNALTRANSFER.ShowDialog()
        'CHECKINTERNALRECIEVED()
    End Sub

    Private Sub BarButtonItem113_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem113.ItemClick
        FRMCONFIRMISSUED.ShowDialog()
    End Sub

    Private Sub BarButtonItem115_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem115.ItemClick
        FRmIDsql = 58
        FrmSelectAccountsBetweenBranches.ShowDialog()
    End Sub

    Private Sub BarButtonItem116_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem116.ItemClick
        FrmViewAgentMovement.ShowDialog()
    End Sub

    Private Sub BarButtonItem117_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem117.ItemClick
        View_RecieveInternalEx.ShowDialog()
    End Sub

    Private Sub BarButtonItem118_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem118.ItemClick
        FRMCashMovement.ShowDialog()
    End Sub

    Private Sub BarButtonItem68_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem68.ItemClick
        FRMEMPADVANCEPAYMENT.ShowDialog()
    End Sub

    Private Sub BarButtonItem74_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem74.ItemClick
        FRMEMPADDINCREASE.NEWRECORD()
        FRMEMPADDINCREASE.ShowDialog()
    End Sub

    Private Sub BarButtonItem119_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem119.ItemClick
        FRMEMPDISCOUNT.ShowDialog()
    End Sub

    Private Sub BarButtonItem120_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem120.ItemClick
        FRmIDsql = 89
        FRMINCOMESTATMENT.ShowDialog()
    End Sub

    Private Sub BarButtonItem121_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem121.ItemClick
        FRmIDsql = 90
        FRMselectACountes.ShowDialog()
    End Sub

    Private Sub BarButtonItem51_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem51.ItemClick
        FRMADDASSOCIATION.ShowDialog()
    End Sub

    Private Sub BarButtonItem122_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem122.ItemClick
        FRMADDMEMBER.ShowDialog()
    End Sub

    Private Sub BarButtonItem89_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem89.ItemClick
        FRMEDITASSOCIATIONPAYMENT.ShowDialog()
    End Sub

    Private Sub BarButtonItem123_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem123.ItemClick
        FRMCALCULATEALLMEMBERS.ShowDialog()
    End Sub

    Private Sub FRMSERVICETYPE_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnServiceType.ItemClick
        FRMSERVICETYPE.ShowDialog()
    End Sub

    Private Sub BtnExtIncomeNotDel_Click(sender As Object, e As System.EventArgs) Handles BtnExtIncomeNotDel.Click
        FrmExternalFast.SelectBtn = True
        FrmExternalFast.SelectType = 4
        FrmExternalFast.LOADDATA()
        FrmExternalFast.Text = "حوالات خارجية واردة غير مسلمة"
        If FrmExternalFast.GVROLE.RowCount > 0 Then
            FrmExternalFast.GVROLE.Columns("RecievedBranchID").Visible = False
        End If
        FrmExternalFast.OverAllEx.Visible = True
        FrmExternalFast.OverAllTotal.Visible = True
        FrmExternalFast.ShowDialog()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As System.EventArgs) Handles SimpleButton21.Click
        FrmExternalFast.SelectBtn = True
        FrmExternalFast.SelectType = 5
        FrmExternalFast.LOADDATA()
        FrmExternalFast.Text = "حوالات خارجية ملغاة موافق عليها"
        If FrmExternalFast.GVROLE.RowCount > 0 Then
            FrmExternalFast.GVROLE.Columns("RecievedBranchID").Visible = False
        End If
        FrmExternalFast.OverAllEx.Visible = True
        FrmExternalFast.OverAllTotal.Visible = True
        FrmExternalFast.ShowDialog()
    End Sub

    Private Sub BarButtonItem90_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem90.ItemClick
        FrmUserAccessTemplate.ShowDialog()
    End Sub

    Private Sub BarButtonItem114_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem114.ItemClick
        FrmSafeTransfer.ShowDialog()
    End Sub

    Private Sub BarButtonItem125_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem125.ItemClick
        FrmAddSecrein.ShowDialog()
    End Sub

    Private Sub BarButtonItem124_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem124.ItemClick
        FrmUserAccessTemplate_ueserID.ShowDialog()
    End Sub

    Private Sub BarButtonItem126_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem126.ItemClick
        FRmIDsql = 111
        FRMCountries.ShowDialog()
    End Sub

    Private Sub BarButtonItem107_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem107.ItemClick
        FRmIDsql = 68
        FRMEMPWITHDRAWALNEW.frmid = 68
        FRMEMPWITHDRAWALNEW.NEWRECORD()
        FRMEMPWITHDRAWALNEW.LOADTYPE = 5
        FRMEMPWITHDRAWAL.LOADTYPE = 0
        FRMEMPWITHDRAWALNEW.TYPEs = 1
        FRMEMPWITHDRAWAL.isbanck = 0
        FRMEMPWITHDRAWALNEW.ShowDialog()
    End Sub

    Private Sub ExtConfirm_Click(sender As Object, e As System.EventArgs) Handles ExtConfirm.Click
        If BarButtonItem113.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FRMCONFIRMISSUED.RB2.Checked = True
            FRMCONFIRMISSUED.RB1.Checked = False
            FRMCONFIRMISSUED.RB3.Checked = False
            FRMCONFIRMISSUED.RB4.Checked = False
            'FRMCONFIRMISSUED.LOADDATAExternalEx()
            FRMCONFIRMISSUED.ShowDialog()
        End If

    End Sub
    Private Sub BtnExtConfirmCanc_Click(sender As Object, e As System.EventArgs) Handles BtnExtConfirmCanc.Click
        If BarButtonItem113.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FRMCONFIRMISSUED.RB2.Checked = False
            FRMCONFIRMISSUED.RB1.Checked = False
            FRMCONFIRMISSUED.RB3.Checked = False
            FRMCONFIRMISSUED.RB4.Checked = True
            'FRMCONFIRMISSUED.LOADCANCELEDExternalEx()
            FRMCONFIRMISSUED.ShowDialog()
        End If
    End Sub
    Private Sub BarButtonItem127_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem127.ItemClick
        FRmIDsql = 112
        FRMEMPLOYEE.DataBaseType = 1
        FRMEMPLOYEE.NewRecord()
        FRMEMPLOYEE.ShowDialog()
    End Sub

    Private Sub BarButtonItem106_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem106.ItemClick
        btnShowSafeMovement.PerformClick()
    End Sub

    Private Sub BarButtonItem128_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem128.ItemClick
        FrmUpdateUEsers2.ShowDialog()
    End Sub
    Private Sub LookUpEdit1_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles LookUpEdit1.ButtonClick
        If LookUpEdit1.Text = String.Empty Then
            LookUpEdit1.ErrorText = "الرجاء ادخال رقم الكود"
            Return
        End If
        FRMEXTERNALTRANS.NewRecord()
        FRMEXTERNALTRANS.ConfirmType = 4
        FRMEXTERNALTRANS.ISUpdate = True
        FRMEXTERNALTRANS.BtnSave.Enabled = False
        FRMEXTERNALTRANS.BtnEdit.Enabled = False
        FRMEXTERNALTRANS.BtnDelete.Enabled = False
        FRMEXTERNALTRANS.BtnPrint.Enabled = True
        FRMEXTERNALTRANS.SHOW_RECORD(LookUpEdit1.Text)
        Try
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@Code", LookUpEdit1.Text)
            Dim dt As DataTable = RUN_QUARY_PRO("ExternalEx_LoadRecordToConfirm", PRM)
            If dt.Rows.Count > 0 Then
                FRMEXTERNALTRANS.EnabledCTRL(False)
                FRMEXTERNALTRANS.CodeID.Text = LookUpEdit1.Text
                FRMEXTERNALTRANS.IsDelivered.Enabled = False
                FRMEXTERNALTRANS.ShowDialog()
                FRMEXTERNALTRANS.BranchRecievedID.Enabled = False
            Else
                ErrorMessage(Me, "رسالة خطأ", "رمز الحوالة خطأ يرجى التأكد من البيانات")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub

    Private Sub FRMMAIN_Activated(sender As Object, e As System.EventArgs) Handles MyBase.Activated
        Refreshtimer()
    End Sub
    Public Sub Refreshtimer()
        mmLoginWaiting = 14
        ssLoginWaiting = 60
        ffLoginWaiting = 0
        Timer1.Start()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        'If ssLoginWaiting = 0 Then
        '    ssLoginWaiting = 60
        '    mmLoginWaiting -= 1
        'End If
        'ssLoginWaiting -= 1
        'ffLoginWaiting = Convert.ToString(Format(mmLoginWaiting, "00")) + ":" + Convert.ToString(Format(ssLoginWaiting, "00"))
        'BarButtonItem130.Caption = ffLoginWaiting
        'If mmLoginWaiting = 0 And ssLoginWaiting = 1 Then
        '    Refreshtimer()
        '    Timer1.Stop()
        '    If Application.OpenForms().OfType(Of FrmWaitingLogin).Any Or Application.OpenForms().OfType(Of FrmLogin).Any Then
        '        Exit Sub
        '    End If
        '    FrmWaitingLogin.Focus()
        '    FrmWaitingLogin.ShowDialog()
        'End If
    End Sub

    Private Sub BarButtonItem129_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem129.ItemClick
        FRmIDsql = 115
        FRMALLCustomerStatement.ShowDialog()
    End Sub

    Private Sub BarButtonItem131_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem131.ItemClick
        FRmIDsql = 114
        FRMCities.NEWRECORD()
        FRMCities.ShowDialog()
    End Sub

    Private Sub BtnAddPartner_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddPartner.ItemClick
        FRmIDsql = 116
        FRMADDPARTENR.ShowDialog()
    End Sub

    Private Sub BarButtonItem132_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem132.ItemClick
        FRMALLAgentMovment.ShowDialog()
    End Sub

    Private Sub BarButtonItem133_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem133.ItemClick
        FRMADDFORIGNACCOUNT.ShowDialog()
    End Sub

    Private Sub BarButtonItem134_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem134.ItemClick
        FrmAdvancePaymentLoadAllData.ShowDialog()
    End Sub

    Private Sub BarButtonItem136_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem136.ItemClick
        FRMGetWatsappGroup.ShowDialog()
    End Sub

    Private Sub BarButtonItem137_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem137.ItemClick
        FRMPROFITS.ShowDialog()
    End Sub

    Private Sub BarButtonItem73_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMEMPWITHDRAWAL.Frmid = 119
        FRMEMPWITHDRAWAL.Text = "سند قبض في حساب شريك"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(63)
        FRMEMPWITHDRAWAL.LOADTYPE = 63
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem138_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)

        FRMEMPWITHDRAWAL.Frmid = 118
        FRMEMPWITHDRAWAL.Text = "سند صرف من حساب شريك"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(64)
        FRMEMPWITHDRAWAL.LOADTYPE = 64
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem139_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem139.ItemClick
        FRM_SEND_From.ShowDialog()
    End Sub

    Private Sub BarButtonItem140_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem140.ItemClick
        FRmIDsql = 122
        FRM_Total_Branches_Cash.ShowDialog()
    End Sub

    Private Sub BarButtonItem141_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem141.ItemClick
        FRmIDsql = 123
        FRMBranches_Budget.new_recordes()
        FRMBranches_Budget.ShowDialog()
    End Sub

    Private Sub BarButtonItem142_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem142.ItemClick
        ModifyingAccountingEntries.ShowDialog()
    End Sub
    Dim CLSNCOME As New CLSADINCOME
    Private Sub BarButtonItem143_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMADDINCOME.Frmid = 124
        'FRMEMPWITHDRAWAL.Tag = 66
        FRMADDINCOME.Text = "سند قبض إيراد محطة "
        LayoutControlItem5.Text = "قيمة الإيداع"
        CLSNCOME.EMPORCUSTWITHDRAWALTB_MaxID(34)
        FRMADDINCOME.LOADTYPE = 34
        FRMADDINCOME.TYPEs = 1
        FRMADDINCOME.ShowDialog()
    End Sub

    Private Sub BarButtonItem144_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMADDINCOME.Frmid = 125
        FRMADDINCOME.Text = "سند قبض إيراد شحن"
        LayoutControlItem5.Text = "قيمة الإيداع"
        CLSNCOME.EMPORCUSTWITHDRAWALTB_MaxID(35)
        FRMADDINCOME.LOADTYPE = 35
        FRMADDINCOME.TYPEs = 1
        FRMADDINCOME.ShowDialog()
    End Sub

    Private Sub BarButtonItem143_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem143.ItemClick
        FrmADDAsset.ShowDialog()
    End Sub

    Private Sub BarButtonItem144_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem144.ItemClick
        FRMANOTHEREXPENS.IsAseet = True
        FRMANOTHEREXPENS.Text = "شراء أصل"
        FRMANOTHEREXPENS.ShowDialog()
    End Sub

    Private Sub BarButtonItem145_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMADDINCOME.Frmid = 128
        FRMADDINCOME.Text = "سند صرف محطة"
        LayoutControlItem5.Text = "قيمة الصرف"
        CLSNCOME.EMPORCUSTWITHDRAWALTB_MaxID(36)
        FRMADDINCOME.LOADTYPE = 36
        FRMADDINCOME.TYPEs = 1
        FRMADDINCOME.ShowDialog()
    End Sub
    Private Sub BarButtonItem147_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem147.ItemClick
        FRmIDsql = 144
        FRMPartnerMovment.ShowDialog()
    End Sub

    Private Sub BarButtonItem148_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem148.ItemClick
        FRmIDsql = 130
        FRMEMPWITHDRAWAL.Text = "سند ايداع نقد اجنبي لوكيل"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(32)
        FRMEMPWITHDRAWAL.LOADTYPE = 32
        FRMEMPWITHDRAWAL.TYPEs = 2
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem149_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem149.ItemClick
        FRmIDsql = 131
        FRMEMPWITHDRAWAL.Text = "سند صرف نقد اجنبي لوكيل"
        LayoutControlItem5.Text = "قيمة السحب"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(33)
        FRMEMPWITHDRAWAL.LOADTYPE = 33
        FRMEMPWITHDRAWAL.TYPEs = 2
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem150_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem150.ItemClick
        FRmIDsql = 132
        FRMTransBtweenAccounts.ShowDialog()
    End Sub

    Private Sub BarButtonItem151_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem151.ItemClick
        FRmIDsql = 133
        FrmProftsOrLossesInComeStatment.ShowDialog()
    End Sub

    Private Sub BtnAddProject_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddProject.ItemClick
        FRmIDsql = 134
        FrmAddProject.ShowDialog()
    End Sub

    Private Sub BtnAddAssest_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddAssest.ItemClick
        FRmIDsql = 136
        FrmAddAssest.ShowDialog()
    End Sub

    Private Sub BtnProjectPartner_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnProjectPartner.ItemClick
        FRmIDsql = 135
        FrmAddProPartner.ShowDialog()
    End Sub

    Private Sub BarButtonItem152_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddProExpense.ItemClick
        FRmIDsql = 137
        FrmAddProExpenseAcc.ShowDialog()
    End Sub

    Private Sub BarButtonItem153_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnProPayPetty.ItemClick
        FRmIDsql = 139
        FrmProPettyCash.ShowDialog()
    End Sub

    Private Sub BtnPettySettlement_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnPettySettlement.ItemClick
        FRmIDsql = 140
        FrmProPettyCashSettlement.ShowDialog()
    End Sub

    Private Sub BtnAnotherExpense_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAnotherExpense.ItemClick
        FRmIDsql = 141
        FRMPROANOTHEREXPENS.ShowDialog()
    End Sub

    Private Sub BarButtonItem153_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem153.ItemClick

        FRmIDsql = 138
        FRMAddContractor.ShowDialog()
    End Sub

    Private Sub BtnContractorPayment_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnContractorPayment.ItemClick
        FRmIDsql = 142
        FRMPROEMPWITHDRAWAL.Text = "سند صرف لمقاول"
        LayoutControlItem5.Text = "قيمة السحب"
        FRMPROEMPWITHDRAWAL.EMPORCUSTWITHDRAWALTB_MaxID(41)
        FRMPROEMPWITHDRAWAL.LOADTYPE = 41
        FRMPROEMPWITHDRAWAL.TYPEs = 1
        FRMPROEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem154_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem154.ItemClick
        FRmIDsql = 143
        ProjectsStetment.ShowDialog()
    End Sub

    Private Sub BarButtonItem155_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem155.ItemClick
        FRMProPartnerMovment.ShowDialog()
    End Sub

    Private Sub BarButtonItem156_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem156.ItemClick
        FRMProBayAsset.ShowDialog()
    End Sub

    Private Sub BarButtonItem158_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem158.ItemClick
        FRMAccActivityStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem159_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem159.ItemClick
        FRMEMPLOYEE.DataBaseType = 2
        FRMEMPLOYEE.NewRecord()
        FRMEMPLOYEE.ShowDialog()
    End Sub

    Private Sub BarButtonItem160_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem160.ItemClick
        FrmIndividualSalaryEMP.SalaryCalc = 1
        FrmIndividualSalaryEMP.DataBaseType = 2
        FrmIndividualSalaryEMP.NEWRECORD()
        FrmIndividualSalaryEMP.ShowDialog()
    End Sub

    Private Sub BarButtonItem161_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem161.ItemClick
        FRMPROEMPWITHDRAWAL.Text = "سند صرف لموظف"
        LayoutControlItem5.Text = "قيمة السحب"
        FRMPROEMPWITHDRAWAL.EMPORCUSTWITHDRAWALTB_MaxID(46)
        FRMPROEMPWITHDRAWAL.LOADTYPE = 46
        FRMPROEMPWITHDRAWAL.TYPEs = 1
        FRMPROEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem162_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem162.ItemClick
        FRMEmpStetment.NEWRECORD()
        FRMEmpStetment.BranchID.EditValue = BID
        FRMEmpStetment.LOADEMP(BID)
        FRMEmpStetment.ShowDialog()
    End Sub

    Private Sub BarButtonItem157_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem157.ItemClick
        FRMTransBetweenBanks.BtnNew.PerformClick()
        FRMTransBetweenBanks.LOADTYPE = 47
        FRMTransBetweenBanks.ShowDialog()
    End Sub
    Private Sub BarButtonItem163_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem163.ItemClick
        FRMCashStatement.ShowDialog()
    End Sub

    Private Sub BarButtonItem164_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem164.ItemClick
        FRMRPTProPettyCashSettlement.ShowDialog()
    End Sub

    Private Sub BarButtonItem165_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddItem.ItemClick
        FRMADDITEM.ShowDialog()
    End Sub

    Private Sub BarButtonItem166_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddItemDetails.ItemClick
        FrmProAddCategories.ShowDialog()
    End Sub

    Private Sub BtnAddSupplier_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnAddSupplier.ItemClick
        FrmProSupplier.ShowDialog()
    End Sub

    Private Sub BtnImportItem_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnImportItem.ItemClick
        FrmImportItems.ShowDialog()
    End Sub

    Private Sub BarButtonItem165_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem165.ItemClick
        FRMProBANKDEPOSIT.Tag = 63
        FRMProBANKDEPOSIT.Text = "إيداع مصرفي لمشروع"
        FRMProBANKDEPOSIT.BtnNew.PerformClick()
        FRMProBANKDEPOSIT.LOADTYPE = 48
        FRMProBANKDEPOSIT.ShowDialog()
    End Sub

    Private Sub BarButtonItem166_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem166.ItemClick
        FRMPROEMPWITHDRAWAL.Text = "سند صرف لمورد"
        LayoutControlItem5.Text = "قيمة السحب"
        FRMPROEMPWITHDRAWAL.EMPORCUSTWITHDRAWALTB_MaxID(49)
        FRMPROEMPWITHDRAWAL.LOADTYPE = 49
        FRMPROEMPWITHDRAWAL.TYPEs = 1
        FRMPROEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BtnPROEXPORTITEM_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnPROEXPORTITEM.ItemClick
        FRMPROEXPORTITEM.ShowDialog()
    End Sub

    Private Sub BarButtonItem167_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem167.ItemClick
        FRMWorkshopOperations.ShowDialog()
    End Sub

    Private Sub BarButtonItem168_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem168.ItemClick
        FRmIDsql = 161
        FRMSuplierStatment.NEWRECORD()
        FRMSuplierStatment.BranchID.EditValue = BID
        FRMSuplierStatment.LOADEMP(BID)
        FRMSuplierStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem169_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem169.ItemClick
        FRMPETTYCASHTBLOADTOREPORT.ShowDialog()
    End Sub

    Private Sub BarButtonItem170_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem170.ItemClick
        ImportItemsStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem171_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem171.ItemClick
        FRmIDsql = 164
        FRMSalaryCalculationStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem172_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem172.ItemClick
        FRmIDsql = 165
        FRMCurrLimited.ShowDialog()
    End Sub

    Private Sub BarButtonItem173_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem173.ItemClick
        FrmCustomerCurrBayAndSale.ShowDialog()
    End Sub

    Private Sub BarButtonItem174_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem174.ItemClick
        FRMSaleCurrencyForCUST.ShowDialog()
    End Sub

    Private Sub BarButtonItem175_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem175.ItemClick
        FrmAccountLimitations.ShowDialog()
    End Sub

    Private Sub BarButtonItem176_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem176.ItemClick
        FrmClosingBusinessActivity.ShowDialog()
    End Sub

    Private Sub BarButtonItem177_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem177.ItemClick
        FrmBBranchBalncesStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem178_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem178.ItemClick
        FRMNewPROFITS.ShowDialog()
    End Sub

    Private Sub BtnLeave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnLeave.ItemClick
        FrmLeave.ShowDialog()
    End Sub

    Private Sub BarButtonItem179_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem179.ItemClick
        FrmViewBranchSafeDetails.ShowDialog()
    End Sub

    Private Sub BarButtonItem180_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem180.ItemClick

        FRMLimitedStatment.ShowDialog()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As System.EventArgs) Handles SimpleButton11.Click
        If BtnLeave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FRMLeaveConfirm.LeaveConType = 1
            FRMLeaveConfirm.ShowDialog()
        Else

        End If
    End Sub

    Private Sub BtnIntIncomeNotDel111_Click(sender As Object, e As System.EventArgs) Handles BtnIntIncomeNotDel111.Click
        If BtnLeave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always Then
            FRMLeaveConfirm.LeaveConType = 2
            FRMLeaveConfirm.ShowDialog()
        End If

    End Sub

    Private Sub BarButtonItem181_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem181.ItemClick
        FRmIDsql = 169
        CurrBenfitsStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem183_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem183.ItemClick
        FRMDaily_transfer_preparer_schedule_Get.ShowDialog()
    End Sub

    Private Sub BarButtonItem182_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem182.ItemClick
        FRMTransfer_commissions.ShowDialog()
    End Sub

    Private Sub BarButtonItem184_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem184.ItemClick
        FRM_ADD_FROM_Costmer_Mobile.ShowDialog()
    End Sub

    Private Sub BarButtonItem185_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem185.ItemClick
        FrmMultiAcountEdit.ShowDialog()
    End Sub

    Private Sub BarButtonItem186_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem186.ItemClick
        FrmInternalEx_getinsert_DailyCount.ShowDialog()
    End Sub

    Private Sub BarButtonItem187_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem187.ItemClick
        FrmDiscountsAndBonuses.ShowDialog()
    End Sub

    Private Sub BarButtonItem189_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem189.ItemClick
        FRM_AddDRiver.ShowDialog()
    End Sub

    Private Sub BarButtonItem190_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem190.ItemClick
        FRMEmpLeaveStatment.ShowDialog()
    End Sub

    Private Sub BarButtonItem191_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem191.ItemClick
        FrmResignation.ShowDialog()
    End Sub

    Private Sub BarButtonItem192_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem192.ItemClick
        FrmOverallDiscAndBounses.ShowDialog()
    End Sub

    Private Sub BarButtonItem193_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem193.ItemClick
        FrmAddSafe_mobile.ShowDialog()
    End Sub

    Private Sub LookUpEdit2_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles CustAccID.ButtonClick
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = Convert.ToUInt64(CustAccID.Text.Trim)}
        PR(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = UserID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CustomersTb_SherchByAccID", PR)
        If dt.Rows.Count > 0 Then
            FrmCustomerMovement.TypeNewRe = 1
            FrmCustomerMovement.NEWRECORD()
            FrmCustomerMovement.BranchID.EditValue = dt.Rows(0)("BranchID")
            FrmCustomerMovement.LOADCUST_WITHBRANCH2()
            FrmCustomerMovement.CurrencyTo.EditValue = DefaultCurrency
            FrmCustomerMovement.CUST.EditValue = Convert.ToUInt64(CustAccID.Text.Trim)
            FrmCustomerMovement.CustCode.Text = CustAccID.Text.Trim
            FrmCustomerMovement.SimpleButton11_Click(Nothing, Nothing)
            FrmCustomerMovement.ShowDialog()
        Else
            ErrorMessage(Me, "رسالة خطأ", "عذرا لا يوجد حساب بهذا الرقم")
        End If
    End Sub

    Private Sub BarButtonItem194_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem194.ItemClick
        FrmReceivetaxirequestfromtheapp.ShowDialog()
    End Sub

    Private Sub SimpleButton12_Click(sender As Object, e As System.EventArgs) Handles SimpleButton12.Click
        FrmReceivetaxirequestfromtheapp.ShowDialog()
    End Sub

    Private Sub BarButtonItem195_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem195.ItemClick
        FRMTAxi_OK_NOW.ShowDialog()
    End Sub

    Private Sub SimpleButton121_Click(sender As Object, e As System.EventArgs) Handles SimpleButton121.Click
        FRMTAxi_OK_NOW.ShowDialog()
    End Sub

    Private Sub BarButtonItem196_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem196.ItemClick
        FRM_Request_to_summon_drivers.ShowDialog()
    End Sub

    Private Sub BarButtonItem197_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem197.ItemClick
        FRM_DRiver_fromACoount.ShowDialog()
    End Sub

    Private Sub BarButtonItem198_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem198.ItemClick
        FrmDriverDeliveryInternalShipping.ShowDialog()
        'TaxiInvoiceDrivers_insert
    End Sub

    Private Sub SimpleButton1211_Click(sender As Object, e As System.EventArgs) Handles SimpleButton1211.Click
        FRmSandTaxiForMobile.ShowDialog()
    End Sub

    Private Sub BarButtonItem199_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        FRMEMPWITHDRAWAL.Frmid = 49
        FRMEMPWITHDRAWAL.Tag = 61
        FRMEMPWITHDRAWAL.Text = "سند قبض من مندوب"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(55)
        FRMEMPWITHDRAWAL.LOADTYPE = 55
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub SimpleButton12111_Click(sender As Object, e As System.EventArgs) Handles SimpleButton12111.Click
        FRMTAxi_CanselFor_Driver.ShowDialog()
    End Sub

    Private Sub BarButtonItem73_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem73.ItemClick
        FrmAddnavcation.ShowDialog()
    End Sub

    Private Sub BarButtonItem200_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem200.ItemClick
        FRM_Retuns_ueser_Regstir_for_Actvion_Account.ShowDialog()
    End Sub

    Private Sub BarButtonItem83_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles MoActivetion.ItemClick
        FRM_Retuns_ueser_Regstir_for_Actvion_Accoun_Rect.ShowDialog()
    End Sub

    Private Sub ExtCanceledConfrimed_Click(sender As Object, e As System.EventArgs) Handles ExtCanceledConfrimed.Click

    End Sub

    Private Sub ExtOutcomeNotDelivered_Click(sender As Object, e As System.EventArgs) Handles ExtOutcomeNotDelivered.Click

    End Sub

    Private Sub BarButtonItem85_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem855.ItemClick
        frm_addmain.ShowDialog()
    End Sub

    Private Sub BarButtonItem85_ItemClick_2(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem85585.ItemClick
        FRMADDGroupMAin.ShowDialog()
    End Sub

    Private Sub BarButtonItem85_ItemClick_3(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem85.ItemClick
        FrmBalnceSheet.ShowDialog()
    End Sub

    Private Sub BarButtonItem83_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem83.ItemClick
        FrmBankPortfolio.ShowDialog()
    End Sub

    Private Sub BarButtonItem96_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem96.ItemClick

    End Sub

    Private Sub BarButtonItem97_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem97.ItemClick
        FRmIDsql = 193
        FRMALLDebtorsMovment.ShowDialog()
    End Sub

    Private Sub BarButtonItem138_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem138.ItemClick
        FrmCash_BankTransfers.ShowDialog()
    End Sub

    Private Sub BarButtonItem145_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem145.ItemClick
        FRMBankTransaction.ShowDialog()
    End Sub



    Private Sub BarButtonItem201_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem201.ItemClick
        frmassets.ShowDialog()
    End Sub

    Private Sub BarButtonItem202_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem202.ItemClick
        FrmSettlement_with_Agent.ShowDialog()
    End Sub

    Private Sub BarButtonItem203_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem203.ItemClick
        FRMEMPWITHDRAWAL.Frmid = 198
        FRMEMPWITHDRAWAL.Tag = 60
        FRMEMPWITHDRAWAL.Text = "ايداع في حساب نقدي مصرف ليبيا المركزي"
        LayoutControlItem5.Text = "قيمة الإيداع"
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(7)
        FRMEMPWITHDRAWAL.LOADTYPE = 7
        FRMEMPWITHDRAWAL.TYPEs = 1
        FRMEMPWITHDRAWAL.isbanck = 6
        FRMEMPWITHDRAWAL.ShowDialog()
    End Sub

    Private Sub BarButtonItem204_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem204.ItemClick
        FRmIDsql = 83
        FRMBANKDEPOSIT.barDockControlTop.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnNew.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnDelete.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnEdit.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnPrint.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnSave.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.Tag = 63
        FRMBANKDEPOSIT.Text = "إيداع مصرفي"
        FRMBANKDEPOSIT.BtnNew.PerformClick()
        FRMBANKDEPOSIT.LOADTYPE = 16
        FRMBANKDEPOSIT.ShowDialog()
    End Sub

    Private Sub BarButtonItem205_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem205.ItemClick
        FRmIDsql = 199
        FRMBANKDEPOSIT.barDockControlTop.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnNew.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnDelete.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnEdit.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnPrint.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.BtnSave.Appearance.BackColor = System.Drawing.Color.Green
        FRMBANKDEPOSIT.Tag = 63
        FRMBANKDEPOSIT.Text = "ايداع بطاقة اغراض  الشخصية بقسائم تحويل"
        FRMBANKDEPOSIT.BtnNew.PerformClick()
        FRMBANKDEPOSIT.LOADTYPE = 16
        FRMBANKDEPOSIT.isbanck = 6
        FRMBANKDEPOSIT.ShowDialog()
    End Sub

    Private Sub BarButtonItem206_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem206.ItemClick
        View_orders.ShowDialog()
    End Sub

    Private Sub BarButtonItem207_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem207.ItemClick
        FRM_Lde_View_orders.ShowDialog()
    End Sub

    Private Sub BarButtonItem208_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem208.ItemClick
        FRmBanck_Sheet_Tigare.ShowDialog()
    End Sub

    Private Sub BarButtonItem209_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem209.ItemClick
        FRMBanck_central_shee.ShowDialog()
    End Sub

    Private Sub BarButtonItem210_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem210.ItemClick
        FrmASSMember_Statment.ShowDialog()
    End Sub

    Private Sub BarButtonItem80_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem80.ItemClick

    End Sub

    Private Sub BarButtonItem211_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem211.ItemClick
        FRMWorkshopOperations.ShowDialog()
    End Sub

    Private Sub BarButtonItem212_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem212.ItemClick
        FrmClosingBusinessActivity.ShowDialog()
    End Sub

    Private Sub BarButtonItem213_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem213.ItemClick
        FRMNewPROFITS.ShowDialog()
    End Sub

    Private Sub BarButtonItem214_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem214.ItemClick
        Association_expenses.ShowDialog()
    End Sub

    Private Sub BarButtonItem215_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem215.ItemClick
        Association_revenues.ShowDialog()
    End Sub

    Private Sub BarButtonItem78_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem78.ItemClick

    End Sub

    Private Sub BarButtonItem216_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem216.ItemClick
        FrmCurrnceyStatment2026.ShowDialog()
    End Sub

    Private Sub BarButtonItem217_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem217.ItemClick
        FrmCompanies.ShowDialog()
    End Sub

    Sub ALLEMPPRINT()
        Try
            Dim dt As DataTable = RUN_QUARY_PRO_ONLY("EmployeeTb_LOADBASISDATA")
            If dt.Rows.Count > 0 Then
                Dim report As New RPTEMPLAODBASICDATA
                dt.TableName = "EmployeeTb"
                Dim ds As New DataSet
                ds.Tables.Add(dt)
                report.DataSource = ds
                report.DataMember = "EmployeeTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                InfoMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها")
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

End Class


