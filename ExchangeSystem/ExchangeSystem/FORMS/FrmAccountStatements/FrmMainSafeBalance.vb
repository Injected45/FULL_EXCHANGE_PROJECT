Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Shape.Native
Imports DevExpress.XtraReports.UI

Public Class FrmMainSafeBalance
    Public LOADTYPE As Int32 = 0
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.GroupPanelText = ""
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10.5, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.White
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
    End Sub
    Sub LOADCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToLKP")
        CountryID.Properties.DataSource = DT
        CountryID.Properties.ValueMember = "ID"
        CountryID.Properties.DisplayMember = "CName"
        CountryID.Properties.ShowHeader = False
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)
        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            SType.Enabled = dt.Rows(0)("Can_branch")
            CountryID.Enabled = dt.Rows(0)("Can_branch")

        Else
            BranchID.Enabled = False
            SType.Enabled = False
            CountryID.Enabled = False
        End If
    End Sub
    Public Sub GET_TABLE_FOR_Costof_PROC()
        Try
            GridControl1.DataSource = Nothing
            GridControl11.DataSource = Nothing
            NewNetTotal.EditValue = 0.000
            NewPrice.EditValue = 0.000
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(3) As SqlParameter
            prm(0) = New SqlParameter("@BranchId", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
            prm(1) = New SqlParameter("@ISInOrOut", SqlDbType.Int) With {.Value = SafeToInt(SType.SelectedIndex)}
            prm(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
            prm(3) = New SqlParameter("@CompanyOrCust", SqlDbType.Int) With {.Value = 0}
            dt = RUN_QUARY_PRO("GET_TABLE_FOR_Costof_Main_safs", prm)
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt

            Else

            End If


            Dim dt1 As New DataTable
            dt1.Clear()
            Dim prm1(3) As SqlParameter
            prm1(0) = New SqlParameter("@BranchId", SqlDbType.Int) With {.Value = SafeToInt(BranchID.EditValue)}
            prm1(1) = New SqlParameter("@ISInOrOut", SqlDbType.Int) With {.Value = SafeToInt(SType.SelectedIndex)}
            prm1(2) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
            prm1(3) = New SqlParameter("@CompanyOrCust", SqlDbType.Int) With {.Value = 1}
            dt1 = RUN_QUARY_PRO("GET_TABLE_FOR_Costof_Main_safs", prm1)
            If dt1.Rows.Count > 0 Then
                GridControl11.DataSource = dt1
            Else

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub LoadData()
        Try

            If SType.SelectedIndex = -1 Then
                SType.ErrorText = "الرجاء اختيار المستوى"
                Exit Sub
            End If
            If BranchID.EditValue = -1 And SType.SelectedIndex = 0 Then
                BranchID.ErrorText = "الرجاء اختيار الفرع"
                Exit Sub
            End If
            If CountryID.EditValue = -1 And SType.SelectedIndex = 1 Then
                CountryID.ErrorText = "الرجاء اختيار الدولة"
                Exit Sub
            End If
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(1) As SqlParameter
            If CountryID.EditValue = MAINCountryID Then
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            Else
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = CountryID.EditValue}
            End If
            prm(1) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = SType.SelectedIndex}
            GCROLE.DataSource = Nothing
            dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafes", prm)
            If dt.Rows.Count > 0 Then
                GCROLE.DataSource = dt
                GVRole.Columns("AccID").Visible = False
                DVGFROMAT()
            Else
                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                ' force Message Boxes to use the "MyCustomSkin"
                XtraMessageBox.AllowCustomLookAndFeel = True
                XtraMessageBox.Show(lookAndFeelError, "لا يوجد بيانات لعرضها خلال هذه الفترة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GCROLE.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub BranchIDd_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False
        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
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
    Private Sub FrmMainSafeBalance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADCountry()
        LOADBRANCH()
        SType.SelectedIndex = 0
        BranchID.EditValue = BID
        NewNetTotal.EditValue = 0.000
        NewPrice.EditValue = 0.000
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 25)
    End Sub
    Private Sub FrmMainSafeBalance_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        BranchID.EditValue = -1
        GVRole.Columns.Clear()
        GCROLE.DataSource = Nothing
    End Sub



    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            LOADTYPE = 0
            FRMBRANCHSAFEDETALIS.FilterType = 0
            Dim CO As ULong = Convert.ToUInt64(view.GetFocusedRowCellValue("AccID"))
            FRMBRANCHSAFEDETALIS.AccID = Convert.ToUInt64(view.GetFocusedRowCellValue("AccID"))
            'Try
            If SType.SelectedIndex = -1 Then
                SType.ErrorText = "الرجاء اختيار المستوى"
                Exit Sub
            End If
            If BranchID.EditValue = -1 And SType.SelectedIndex = 0 Then
                BranchID.ErrorText = "الرجاء اختيار الفرع"
                Exit Sub
            End If

            Dim dt As New DataTable
            dt.Clear()
            Dim prm(10) As SqlParameter
            If SType.SelectedIndex = 0 Then
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            End If
            If SType.SelectedIndex = 1 Then
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = 0}
            End If
            prm(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = CO}
            prm(2) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = SType.SelectedIndex}
            prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = Date.Now}
            prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = Date.Now}
            prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(10) = New SqlParameter("@LoadType", SqlDbType.Decimal) With {.Value = LOADTYPE}
            FRMBRANCHSAFEDETALIS.GCROLE.DataSource = Nothing

            dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafesDetails", prm)
            If dt.Rows.Count > 0 Then
                FRMBRANCHSAFEDETALIS.GCROLE.DataSource = dt
                'FRMBRANCHSAFEDETALIS.GVRole.Columns("الخزنة").Width = 300
                FRMBRANCHSAFEDETALIS.OverAllCredit.EditValue = prm(6).Value
                FRMBRANCHSAFEDETALIS.OverAllDebit.EditValue = prm(5).Value
                FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.EditValue = prm(8).Value
                FRMBRANCHSAFEDETALIS.PreviewsBalance.EditValue = prm(9)
                If FRMBRANCHSAFEDETALIS.OverAllDebit.EditValue > FRMBRANCHSAFEDETALIS.OverAllCredit.EditValue Then
                    FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.BackColor = Color.Green
                    FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Green
                ElseIf FRMBRANCHSAFEDETALIS.OverAllDebit.EditValue < FRMBRANCHSAFEDETALIS.OverAllCredit.EditValue Then
                    FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.BackColor = Color.Red
                    FRMBRANCHSAFEDETALIS.OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Red
                End If
                Dim PR1(1) As SqlParameter
                If SType.SelectedIndex = 0 Then
                    PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
                End If
                If SType.SelectedIndex = 1 Then
                    PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = 0}
                End If
                PR1(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = CO}
                Dim DT2 As New DataTable
                DT2 = RUN_QUARY_PRO("AccountsTb_MainBranchSafesDetailsNetTotal", PR1)
                If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                    FRMBRANCHSAFEDETALIS.OverAllTotal.EditValue = DT2.Rows(0)("أول")
                    FRMBRANCHSAFEDETALIS.OverallPrint = DT2.Rows(0)("أول")
                    FRMBRANCHSAFEDETALIS.OverAllTotal.BackColor = Color.Green
                Else
                    FRMBRANCHSAFEDETALIS.OverAllTotal.EditValue = DT2.Rows(0)("ثاني")
                    FRMBRANCHSAFEDETALIS.OverallPrint = DT2.Rows(0)("ثاني")
                    FRMBRANCHSAFEDETALIS.OverAllTotal.BackColor = Color.Red
                End If
                FRMBRANCHSAFEDETALIS.GVRole.Columns("طبيعة الحركة").Width = 400

                FRMBRANCHSAFEDETALIS.DVGFROMAT()
                Dim SafeNAme As String = GVRole.GetFocusedRowCellValue("الخزنة").ToString
                FRMBRANCHSAFEDETALIS.Text = SafeNAme
                FRMBRANCHSAFEDETALIS.ShowDialog()
            Else
                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                ' force Message Boxes to use the "MyCustomSkin"
                XtraMessageBox.AllowCustomLookAndFeel = True
                XtraMessageBox.Show(lookAndFeelError, "لا يوجد بيانات لعرضها خلال هذه الفترة", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
                FRMBRANCHSAFEDETALIS.GCROLE.DataSource = Nothing
            End If
            'Catch ex As Exception
            '    MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'End Try
        End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
    End Sub



    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim custIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = custIcon
        Dim lookFeelError As New UserLookAndFeel(Me)
        lookFeelError.Style = LookAndFeelStyle.Skin
        lookFeelError.UseDefaultLookAndFeel = False
        lookFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        If GVRole.RowCount = 0 Then
            XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لطباعتها", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Dim IsBrIDORCuntID As Integer
            Dim report As XtraReport = Nothing
            If SType.SelectedIndex = 0 Then
                IsBrIDORCuntID = BranchID.EditValue
                report = New RPTMainSafeBalance
            End If
            'If SType.SelectedIndex = 1 Then
            '    IsBrIDORCuntID = CountryID.EditValue
            '    report = New RPTMainSafeBalanc2
            'End If
            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", IsBrIDORCuntID)
            prm(1) = New SqlParameter("@SType", SType.SelectedIndex)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccountsTb_LoadMainBranchSafes", prm)
            If dt.Rows.Count > 0 Then

                report.DataSource = dt
                report.DataAdapter = ""
                report.DataMember = "AccountsTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
            Else
                XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        GET_TABLE_FOR_Costof_PROC()
    End Sub

    Private Sub TileView1_DoubleClick(sender As Object, e As EventArgs) Handles TileView1.DoubleClick
        If NewPrice.EditValue <= 0 Then
            NewPrice.ErrorText = "السعر لا يجب أن يكون أقل من أو يساوي صفر"
            Exit Sub
        End If
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@CurrName", SqlDbType.NVarChar, -1) With {.Value = TileView1.GetFocusedRowCellValue("CuName")}
        prm(1) = New SqlParameter("@CurrencyPower", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

        dt = RUN_QUARY_PRO("GET_Power_of_Currency_byCurrName", prm)


        If TileView1.RowCount > 0 Then

            If prm(1).Value = 1 Then
                NewNetTotal.EditValue = (TileView1.GetFocusedRowCellValue("Foreignbalance") * NewPrice.EditValue) - TileView1.GetFocusedRowCellValue("LocalBalance")
            Else
                NewNetTotal.EditValue = (TileView1.GetFocusedRowCellValue("Foreignbalance") / NewPrice.EditValue) - TileView1.GetFocusedRowCellValue("LocalBalance")
            End If

        End If

    End Sub

    Private Sub NewNetTotal_EditValueChanged(sender As Object, e As EventArgs) Handles NewNetTotal.EditValueChanged
        If NewNetTotal.EditValue > 0 Then
            NewNetTotal.BackColor = Color.Green
        ElseIf NewNetTotal.EditValue < 0 Then
            NewNetTotal.BackColor = Color.Red
        Else NewNetTotal.EditValue = 0
            NewNetTotal.BackColor = Color.Transparent
        End If
    End Sub

    Private Sub FrmMainSafeBalance_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown, SType.KeyDown, BranchID.KeyDown, GCROLE.KeyDown, GridControl1.KeyDown, GridControl11.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub

    Private Sub CountryID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CountryID.QueryPopUp
        CountryID.Properties.PopulateColumns()
        CountryID.Properties.Columns("ID").Visible = False
        CountryID.Properties.Columns("CCode").Visible = False
    End Sub

    Private Sub SType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SType.SelectedIndexChanged
        BranchID.EditValue = -1
        If SType.SelectedIndex = 0 Then
            CountryID.EditValue = MAINCountryID
            CountryID.Enabled = False
            BranchID.Enabled = True
            BranchID.EditValue = BID
        End If
        If SType.SelectedIndex = 1 Then
            CountryID.EditValue = -1
            CountryID.Enabled = True
            BranchID.Enabled = False
        End If
        GET_TABLE_FOR_Costof_PROC()
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 25)
    End Sub

    Private Sub CountryID_EditValueChanged(sender As Object, e As EventArgs) Handles CountryID.EditValueChanged
        GET_TABLE_FOR_Costof_PROC()
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click

        Try





            Dim IsBrIDORCuntID As Integer
            If SType.SelectedIndex = 0 Then
                IsBrIDORCuntID = BranchID.EditValue
            End If
            If SType.SelectedIndex = 1 Then
                IsBrIDORCuntID = CountryID.EditValue
            End If
            SplashScreenManager1.ShowWaitForm()

            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", IsBrIDORCuntID)
            prm(1) = New SqlParameter("@SType", SType.SelectedIndex)
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("ZRPT_AccountsTb_LoadMainBranchSafes", prm)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTMainSafeBalance
                report.DataSource = dt
                report.DataAdapter = ""
                report.DataMember = "AccountsTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                Dim pdfOptions As ImageExportOptions = report.ExportOptions.Image
                Dim stordpath As String
                stordpath = Application.StartupPath & "\TEMPWATS"

                Directory.CreateDirectory(stordpath)
                Dim newfilepathe As String
                newfilepathe = stordpath & "\" & "watsappmassg.jpeg"
                'If ExportOptionsTool.EditExportOptions(pdfOptions, report.PrintingSystem) = DialogResult.OK Then

                report.ExportToImage(newfilepathe, pdfOptions)
                If MAINBID = BranchID.EditValue Then
                    SINTWATSAPP_PDF_CLINT(get_gruop_id(IsBrIDORCuntID, 2), newfilepathe, "كشف الخزينة الرئيسية ")
                Else
                    SINTWATSAPP_PDF_CLINT(get_gruop_id(IsBrIDORCuntID), newfilepathe, "كشف الخزينة الرئيسية ")
                End If




            End If




            SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show($"ErorrFor MAsgg Applaction theis :  {ex.Message}")
        End Try

    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GET_TABLE_FOR_Costof_PROC()
    End Sub
End Class