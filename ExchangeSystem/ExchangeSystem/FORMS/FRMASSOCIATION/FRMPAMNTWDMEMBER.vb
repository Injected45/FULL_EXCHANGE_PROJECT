Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI

Public Class FRMPAMNTWDMEMBER
    Dim clsempwd As New CLSMPAMNTWDMEMBER
    Public IDCode As ULong
    Public LOADTYPE, EMPID As Integer
    Public Property MBRACCID As ULong
    Public IsUpdate, CanChangeSafe As Boolean
    Sub DISAPLEDCONTROLS()
        WDCode.Enabled = False
        AssID.Enabled = False
        SafeID.Enabled = False
        InsertDate.Enabled = False
        WDValue.Enabled = False
        MemberID.Enabled = False
        WithdrawalValue.Enabled = False
        PaidFor.Enabled = False
        Phone.Enabled = False
        IDNo.Enabled = False
        Notes.Enabled = False
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        If LOADTYPE = 22 Then
            dt = SElectUEserFormButtn(95, UserID)
        End If
        If LOADTYPE = 23 Then
            dt = SElectUEserFormButtn(96, UserID)
        End If
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

            SimpleButton11.Visible = dt.Rows(0)("CanSearch")

        End If


    End Sub
    Sub ENAPLEDCONTROLS()
        If LOADTYPE = 22 Then
            GRP1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            MemberID.Enabled = True
            MemberID.EditValue = -1
        ElseIf LOADTYPE = 23 Then
            GRP1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            PaidFor.Enabled = True
            PaidFor.EditValue = -1
        End If
        WDCode.Enabled = False
        WithdrawalValue.Enabled = False
        AssID.Enabled = True
        'If CanChangeSafe = True Then
        SafeID.Enabled = True
        'Else
        '    SafeID.Enabled = False
        'End If
        InsertDate.Enabled = False
        WDValue.Enabled = True

        Phone.Enabled = True
        IDNo.Enabled = True
        Notes.Enabled = True
    End Sub
    Sub NEWRECORD()
        clsempwd.EMPORCUSTWITHDRAWALTB_MaxID(LOADTYPE)
        IsUpdate = False
        ENAPLEDCONTROLS()
        WDCode.Enabled = False
        InsertDate.Enabled = False
        InsertDate.EditValue = Date.Now
        AssID.EditValue = -1
        SafeID.EditValue = -1
        MemberID.EditValue = -1
        PaidFor.EditValue = -1
        WDValue.EditValue = 0.000
        If LOADTYPE = 22 Then
            GRP1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            MemberID.EditValue = -1
        ElseIf LOADTYPE = 23 Then
            GRP1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            PaidFor.EditValue = -1
        End If
        AssID.EditValue = -1
        LOADASS()
        AssID.Select()
        LOADSafeID()
        'SafeID.EditValue = UserID
        BtnSave.Enabled = True
        'BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Enabled = False
        BtnEdit.Enabled = False
        BtnEdit.Caption = "استرجاع القيمة"
        WithdrawalValue.Text = ""
        Notes.Text = ""
        WDValue.Text = ""
        PaidFor.Text = String.Empty
        Phone.Text = String.Empty
        IDNo.Text = String.Empty
        SafeID.EditValue = UserAccID
        If LOADTYPE = 6 Or LOADTYPE = 8 Then
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        ElseIf LOADTYPE = 23 Then
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

        Else
            GRP2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never

        End If
        'FrmScreensTb_Details_UESIRID_GETFrom(UserID, FRmIDsql)
    End Sub


    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            'BranchID.Enabled = dt.Rows(0)("Can_branch")
            SafeID.Enabled = dt.Rows(0)("Can_safID")
            SafeID.EditValue = UserAccID
            'BranchID.EditValue = BID
        Else
            'BranchID.Enabled = False
            SafeID.Enabled = False
            SafeID.EditValue = UserAccID
            'BranchID.EditValue = BID
        End If
    End Sub

    Sub LOADASS()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("ASSOCIATIONNAMETB_LOADTODVG")
        If dt.Rows.Count > 0 Then
            AssID.Properties.DataSource = dt
            AssID.Properties.ValueMember = "ID"
            AssID.Properties.DisplayMember = "ASSNAME"
            AssID.Properties.ShowHeader = False
        End If
    End Sub
    Private Sub AssID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AssID.QueryPopUp
        AssID.Properties.PopulateColumns()
        AssID.Properties.Columns("ID").Visible = False
    End Sub
    Private Sub AssID_TextChanged(sender As Object, e As EventArgs) Handles AssID.TextChanged
        If AssID.EditValue <> -1 Or AssID.Text <> String.Empty Then
            If LOADTYPE = 22 Then
                Dim PR(0) As SqlParameter
                PR(0) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = AssID.EditValue}
                LoadToControlar(MemberID, "ASSOCIATIONTB_LOADBASEDONASSID", "MEMBERNAME", "ID", PR)
            End If
            If LOADTYPE = 23 Then
                Dim PR1(0) As SqlParameter
                PR1(0) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = AssID.EditValue}
                LoadToControlar(PaidFor, "ASSOCIATIONTB_LOADBASEDONASSID", "MEMBERNAME", "ID", PR1, True, "مصروف عام")
            End If
            End If
    End Sub
    Private Sub MemberID_TextChanged(sender As Object, e As EventArgs) Handles MemberID.TextChanged
        MBRACCID = GridLookUpEdit1View.GetFocusedRowCellValue("AccID")
    End Sub
    Sub LOADSafeID()
        'SafeID.Properties.DataSource = Nothing
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = MAINBID}
        PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 7}
        PR(2) = New SqlParameter("@TYPEs", SqlDbType.Int) With {.Value = DefaultCurrency}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AccountsTb_LoadEMPSafeToLKPHasValORNOT", PR)
        If dt.Rows.Count > 0 Then
            SafeID.Properties.DataSource = dt
            SafeID.Properties.ValueMember = "AccID"
            SafeID.Properties.DisplayMember = "UNAME"
            SafeID.Properties.ShowHeader = False
        End If
    End Sub
    Sub ASSOVEALLVAL()

    End Sub
    Public MEBACCID As ULong
    Public AssVal As Decimal
    Dim AssAccID As ULong
    Public Overrides Sub SetData()
        If AssID.EditValue = -1 Then
            AssID.ErrorText = "يرجى اختيار الجمعية"
            Exit Sub
        End If
        If LOADTYPE = 22 Then
            If MemberID.EditValue = -1 Then
                MemberID.ErrorText = "يرجى اختيار المشترك"
                Exit Sub
            End If
        End If
        If LOADTYPE = 23 Then
            If PaidFor.EditValue = -1 Then
                PaidFor.ErrorText = "يرجى اختيار المشترك"
                Exit Sub
            End If
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If LOADTYPE = 22 Then
            If WDValue.Text = "" Then
                WDValue.ErrorText = "يجب إدخال قيمة القبض"
                Exit Sub
            End If
        End If


        Dim DTT As New DataTable
        DTT.Clear()
        DTT = RUN_QUARY_QUERY_ONLY("select ASSOACCID from ASSOCIATIONNAMETB where ID='" & AssID.EditValue & "'")
        If DTT.Rows.Count > 0 Then
            AssAccID = DTT.Rows(0)("ASSOACCID")
        End If
        If LOADTYPE = 23 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = AssAccID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_FUNCTION_PARM("Association_GetAccVal(@AccName) AS GetAccVal", PR)
            If dt.Rows.Count > 0 Then
                AssVal = dt.Rows(0)("GetAccVal")
                If WDValue.EditValue > AssVal Then
                    WDValue.ErrorText = "القيمة أكبر من رصيد الجمعية"
                    Exit Sub
                End If
            End If
        End If


        If LOADTYPE = 23 Then
            MEBACCID = GridLookUpEdit1View.GetFocusedRowCellValue("AccID")
            FRMCODEPYMENT_em_cu2.lodeDate("سند صرف جمعية ", PaidFor.Text, 0, WDValue.EditValue, "ليبي", Phone.Text, 5, "")
            FRMCODEPYMENT_em_cu2.ShowDialog()
            If FRMCODEPYMENT_em_cu2.chick = True Then


                clsempwd.EMPORCUSTWITHDRAWALTB_Insert(InsertDate.EditValue, WDCode.Text, 0, WDValue.EditValue, UserID, LOADTYPE, IDCode, MEBACCID,
                                              AssID.EditValue, SafeID.EditValue, 1, IsUpdate, Notes.Text.Trim, PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)

            Else
                ErrorMessage(Me, "تنبية", "عذرا رقم الكود غير صحيح الرجاء اعادة المحاولة")
            End If
        ElseIf LOADTYPE = 22 Then
            MEBACCID = GridLookUpEdit1View.GetFocusedRowCellValue("AccID")
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(InsertDate.EditValue, WDCode.Text, MemberID.EditValue, WDValue.EditValue, UserID, LOADTYPE, IDCode, MEBACCID,
                                                AssID.EditValue, SafeID.EditValue, 1, IsUpdate, Notes.Text.Trim, PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        End If
        MyBase.SetData()
    End Sub

    Public Sub senddorW()
        SplashScreenManager1.ShowWaitForm()
        Dim rphne As String
        Select Case LOADTYPE
            Case 23
                rphne = Phone.Text
            Case Else
                rphne = ASSOCIATIONTB_phone(MemberID.EditValue, 0)
        End Select
        WATSAPPMsAG(rphne, belderforMas(0), whatsapp_contacts(rphne))
        'WATSAPPMsAG(wtsabID_group_teset, belderforMas(0))
        rphne = ASSOCIATIONTB_phone(AssID.EditValue, 1)
        ''ارسال الي مجموعهة الجمعية السند القبض او الصرف 
        If rphne <> Nothing Then

            WATSAPPMsAG(rphne, belderforMas(3), True)

        End If
        'If LOADTYPE = 23 Then
        '    SEndForAll()
        'End If
        SplashScreenManager1.CloseWaitForm()
    End Sub
    Public Sub SEndForAll()
        Dim dt As New DataTable
        dt.Clear()


        For i = 0 To ASSOCIATIONTB_phone_ALL(0, 0).Rows.Count - 1
            ''تعطيل كود ارسال الي جميع المشتركين في الجمعية

            WATSAPPMsAG(ASSOCIATIONTB_phone_ALL(0, 0).Rows(i)("phone"), belderforMas(0), True)
        Next
    End Sub

    Public Function belderforMas(group As Integer) As String
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = AssAccID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("Association_GetAccVal(@AccName) AS GetAccVal", PR)

        AssVal = dt.Rows(0)("GetAccVal")
        Dim messg As String = My.Settings.Combny_name & vbNewLine

        Select Case LOADTYPE
            Case 23
                messg &= "تم صرف قيمة : " & Cur_Code("ليبي", WDValue.EditValue, True, "n2") & vbNewLine &
                     "من جمعية العدايل" & vbNewLine &
                    "ليد : " & PaidFor.Text & vbNewLine
                Select Case group
                    Case 3
                    Case Else
                        messg &= "لغرض : " & Notes.Text & vbNewLine
                End Select


                messg &= "رصيد الجمعية : " & Cur_Code("ليبي", AssVal, True, "n2") & vbNewLine
            Case 22
                messg &= "تم قبض مبلغ : " & Cur_Code("ليبي", WDValue.EditValue, True, "n2") & vbNewLine &
                    "CODE : " & WDCode.Text & vbNewLine &
                    "حساب : " & MemberID.Text & vbNewLine

        End Select
                Select Case LOADTYPE
            Case 22
                Dim asscvale As Double = ASSOCIATIONID_send_Frowtwsap_NETtotal_OR_ALL(MemberID.EditValue, AssID.EditValue).Rows(0)("NET_TOTal")
                messg &= "بجمعية العدايل " & vbNewLine
                messg &= "الرصيد الحالي: " &
              Cur_Code("ليبي", asscvale, True, "n2") & Space(1)
                If asscvale > 0 Then
                    messg &= "لك " & vbNewLine
                Else
                    messg &= "عليك " & vbNewLine
                End If
        End Select
        messg &= "مع خالص تحياتنا ،،،،،،،"
        Return messg
    End Function



    Sub SHOW_EMCUSCODE(x, s)
        If Me.IsUpdate = True Then
            LOADASS()
            AssID_TextChanged(Nothing, Nothing)
            LOADSafeID()
            Dim DT As New DataTable
            DT.Clear()
            DT = clsempwd.SERACH_EMPORCUSTWITHDRAWALTB(x, s)
            If DT.Rows.Count > 0 Then
                If s = 22 Then
                    InsertDate.EditValue = DT.Rows(0)("InsertDate")
                    WDCode.Text = DT.Rows(0)("Code").ToString
                    AssID.EditValue = DT.Rows(0)("AssID")
                    SafeID.Text = DT.Rows(0)("UName").ToString
                    WDValue.Text = DT.Rows(0)("Credit")
                    MemberID.EditValue = DT.Rows(0)("MEMBERID")
                    Notes.Text = DT.Rows(0)("Notes").ToString
                ElseIf s = 23 Then
                    InsertDate.EditValue = DT.Rows(0)("InsertDate")
                    WDCode.Text = DT.Rows(0)("Code").ToString
                    AssID.EditValue = DT.Rows(0)("AssID")
                    SafeID.Text = DT.Rows(0)("UName").ToString
                    WDValue.Text = DT.Rows(0)("Credit")
                    PaidFor.Text = DT.Rows(0)("MEMBERID").ToString
                    Phone.Text = DT.Rows(0)("Phone").ToString
                    IDNo.Text = DT.Rows(0)("IDNo").ToString
                    Notes.Text = DT.Rows(0)("Notes").ToString
                End If
            End If
        End If
    End Sub
    Public Overrides Sub UPDATERECORD()
        If AssID.EditValue = -1 Then
            AssID.ErrorText = "يرجى اختيار الجمعية"
            Exit Sub
        End If
        If LOADTYPE = 22 Then
            If MemberID.EditValue = -1 Then
                MemberID.ErrorText = "يرجى اختيار المشترك"
                Exit Sub
            End If
        End If
        If LOADTYPE = 23 Then
            If PaidFor.EditValue = -1 Then
                PaidFor.ErrorText = "يرجى اختيار المشترك"
                Exit Sub
            End If
        End If
        If SafeID.EditValue = -1 Then
            SafeID.ErrorText = "يرجى اختيار الخزنة"
            Exit Sub
        End If
        If LOADTYPE = 22 Then
            If WDValue.Text = "" Then
                WDValue.ErrorText = "يجب إدخال قيمة القبض"
                Exit Sub
            End If
        End If

        If LOADTYPE = 23 Then
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(InsertDate.EditValue, WDCode.Text, 0, WDValue.EditValue, UserID, LOADTYPE, IDCode, 0,
                                              AssID.EditValue, SafeID.EditValue, 1, IsUpdate, Notes.Text.Trim, PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        ElseIf LOADTYPE = 22 Then
            clsempwd.EMPORCUSTWITHDRAWALTB_Insert(InsertDate.EditValue, WDCode.Text, MemberID.EditValue, WDValue.EditValue, UserID, LOADTYPE, IDCode, MEBACCID,
                                                AssID.EditValue, SafeID.EditValue, 1, IsUpdate, Notes.Text.Trim, PaidFor.Text.Trim, Phone.Text.Trim, IDNo.Text.Trim)
        End If
        Print()
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Private Sub FRMPAMNTWDMEMBER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'If LOADTYPE = 23 Then
        '    GETSAFEVAL(UserAccID, BID, DefaultCurrency)
        '    If SAFEVAL <= 0 Then
        '        ErrorMessage(Me, "رسالة تنبيه", "عذرا لا يمكن فتح هذه الشاشة لعدم وجود رصيد في الخزنة")
        '        Me.Close()
        '        Exit Sub
        '    End If
        'End If
        lodePreportes()
        NEWRECORD()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub Print()
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Try
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@Code", WDCode.Text.Trim)
            PRM(1) = New SqlParameter("@TypeID", LOADTYPE)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_MPAMNTWDMEMBERTB_SelectByCode", PRM)
            dt.TableName = "MPAMNTWDMEMBERTB"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                If LOADTYPE = 22 Then
                    Dim report As New RPTPAMNTWDMEMBER
                    report.DataSource = ds
                    report.DataMember = "MPAMNTWDMEMBERTB"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()
                End If
                If LOADTYPE = 23 Then
                    Dim report1 As New RPTPAMNTWDMEMBER1
                    report1.DataSource = ds
                    report1.DataMember = "MPAMNTWDMEMBERTB"
                    Dim tool1 As ReportPrintTool = New ReportPrintTool(report1)
                    report1.CreateDocument()
                    report1.ShowPreview()
                End If

            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطابعتها يرجى التحقق من الرمز", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        MyBase.Print()
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FRMVIEWMPAMNTWDMEMBER.AssID.EditValue = -1
        FRMVIEWMPAMNTWDMEMBER.GCRole.DataSource = Nothing
        FRMVIEWMPAMNTWDMEMBER.GVRole.Columns.Clear()
        FRMVIEWMPAMNTWDMEMBER.ShowDialog()
    End Sub

    Private Sub SafeID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles SafeID.QueryPopUp
        SafeID.Properties.PopulateColumns()
        SafeID.Properties.Columns("AccID").Visible = False
    End Sub



    Private Sub PaidFor_QueryPopUp(sender As Object, e As CancelEventArgs) Handles PaidFor.QueryPopUp
        If AssID.EditValue <> -1 Or AssID.Text <> String.Empty Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@AssID", SqlDbType.Int) With {.Value = AssID.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ASSOCIATIONTB_LOADBASEDONASSID", PR)
            If dt.Rows.Count > 0 Then
                PaidFor.Properties.PopulateColumns()
                PaidFor.Properties.Columns("ID").Visible = False
                PaidFor.Properties.Columns("AccID").Visible = False
            End If
        End If
    End Sub
End Class