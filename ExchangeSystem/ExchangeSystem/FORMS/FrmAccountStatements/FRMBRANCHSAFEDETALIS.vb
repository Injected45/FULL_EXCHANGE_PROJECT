Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRMBRANCHSAFEDETALIS
    Public LOADTYPE As Int32 = 0
    Public FilterType As Int16 = 0
    Public AccID As ULong
    Public OverallPrint As Decimal = 0.000
    Public SafeName As String
    Sub DVGFROMAT()
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.GroupPanelText = ""
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 10.5, FontStyle.Regular)
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.White
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
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

    Private Sub FRMBRANCHSAFEDETALIS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentCulture = CultureInfo
        D1.EditValue = Date.Now
        d2.EditValue = Date.Now
    End Sub

    Private Sub FilterByDate_Click(sender As Object, e As EventArgs) Handles FilterByDate.Click
        FilterType = 1
        GCROLE.DataSource = Nothing
        GVRole.Columns.Clear()
        LOADTYPE = 1
        Dim view As GridView = TryCast(sender, GridView)
        Dim CO As ULong = Convert.ToUInt64(FrmMainSafeBalance.GVRole.GetFocusedRowCellValue("AccID"))
        Try
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(10) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
            prm(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = AccID}
            prm(2) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = FrmMainSafeBalance.SType.SelectedIndex}
            prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(10) = New SqlParameter("@LoadType", SqlDbType.Decimal) With {.Value = LOADTYPE}
            Me.GCROLE.DataSource = Nothing
            dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafesDetails", prm)
            If dt.Rows.Count > 0 Then
                Me.GCROLE.DataSource = dt
                Me.OverAllCredit.EditValue = prm(6).Value
                Me.OverAllDebit.EditValue = prm(5).Value
                Me.OverAllPeroidTotal.EditValue = prm(8).Value
                Me.PreviewsBalance.EditValue = prm(9)
                If Me.OverAllDebit.EditValue > Me.OverAllCredit.EditValue Then
                    Me.OverAllPeroidTotal.BackColor = Color.Green
                    Me.OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Green
                ElseIf Me.OverAllDebit.EditValue < Me.OverAllCredit.EditValue Then
                    Me.OverAllPeroidTotal.BackColor = Color.Red
                    Me.OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Red
                End If
                Dim PR1(1) As SqlParameter
                PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
                PR1(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = CO}
                Dim DT2 As New DataTable
                DT2 = RUN_QUARY_PRO("AccountsTb_MainBranchSafesDetailsNetTotal", PR1)
                If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                    Me.OverAllTotal.EditValue = DT2.Rows(0)("أول")
                    Me.OverAllTotal.BackColor = Color.Green
                    OverallPrint = DT2.Rows(0)("أول")
                Else
                    Me.OverAllTotal.EditValue = DT2.Rows(0)("ثاني")
                    Me.OverAllTotal.BackColor = Color.Red
                    OverallPrint = DT2.Rows(0)("ثاني")
                End If
                Me.GVRole.Columns("طبيعة الحركة").Width = 400

                Me.DVGFROMAT()
            Else
                ErrorMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها خلال هذه الفترة")
                Me.GCROLE.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub FilterByDate1_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If GVRole.Columns.Count > 0 Then
            PRINT()
        End If
    End Sub
    Public Sub PRINT()
        SafeName = FrmMainSafeBalance.GVRole.GetFocusedRowCellValue("الخزنة").ToString
        If FilterType = 0 Then
            LOADTYPE = 0
            FilterType = 0
            Dim CO As ULong = Convert.ToUInt64(FrmMainSafeBalance.GVRole.GetFocusedRowCellValue("AccID"))
            Try
                Dim dt As New DataTable
                dt.Clear()
                Dim prm(10) As SqlParameter
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
                prm(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = CO}
                prm(2) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = FrmMainSafeBalance.SType.SelectedIndex}
                prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = Date.Now}
                prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = Date.Now}
                prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(10) = New SqlParameter("@LoadType", SqlDbType.Decimal) With {.Value = LOADTYPE}
                dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafesDetails", prm)
                If dt.Rows.Count > 0 Then
                    Dim report As New RPTMAINSAFEDETAILS
                    report.DataSource = dt
                    report.DataMember = "AccountsTb"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()
                Else
                    ErrorMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها خلال هذه الفترة")
                    Exit Sub
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        ElseIf FilterType = 1 Then
            Try
                LOADTYPE = 1
                Dim CO As ULong = Convert.ToUInt64(FrmMainSafeBalance.GVRole.GetFocusedRowCellValue("AccID"))
                Dim dt As New DataTable
                dt.Clear()
                Dim prm(10) As SqlParameter
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
                prm(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = AccID}
                prm(2) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = FrmMainSafeBalance.SType.SelectedIndex}
                prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
                prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(10) = New SqlParameter("@LoadType", SqlDbType.Decimal) With {.Value = LOADTYPE}
                'Me.GCROLE.DataSource = Nothing
                dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafesDetails", prm)
                If dt.Rows.Count > 0 Then
                    Dim report As New RPTMAINSAFEDETAILS
                    report.DataSource = dt
                    report.DataMember = "AccountsTb"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()
                Else
                    ErrorMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها خلال هذه الفترة")
                    Exit Sub
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        ElseIf FilterType = 2 Then
            LOADTYPE = 0
            Dim CO As ULong = Convert.ToUInt64(FrmMainSafeBalance.GVRole.GetFocusedRowCellValue("AccID"))
            Try
                Dim dt As New DataTable
                dt.Clear()
                Dim prm(10) As SqlParameter
                prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
                prm(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = AccID}
                prm(2) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = FrmMainSafeBalance.SType.SelectedIndex}
                prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
                prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
                prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
                prm(10) = New SqlParameter("@LoadType", SqlDbType.Decimal) With {.Value = LOADTYPE}
                'Me.GCROLE.DataSource = Nothing
                dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafesDetails", prm)
                If dt.Rows.Count > 0 Then
                    Dim report As New RPTMAINSAFEDETAILS
                    report.DataSource = dt
                    report.DataMember = "AccountsTb"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()
                Else
                    ErrorMessage(Me, "رسالة معلومات", "لا يوجد بيانات لعرضها خلال هذه الفترة")
                    Exit Sub
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        FilterType = 2
        GCROLE.DataSource = Nothing
        GVRole.Columns.Clear()
        LOADTYPE = 0
        Dim view As GridView = TryCast(sender, GridView)
        Dim CO As ULong = Convert.ToUInt64(FrmMainSafeBalance.GVRole.GetFocusedRowCellValue("AccID"))
        Try
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(10) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
            prm(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = AccID}
            prm(2) = New SqlParameter("@SType", SqlDbType.Int) With {.Value = FrmMainSafeBalance.SType.SelectedIndex}
            prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            prm(5) = New SqlParameter("@SumDebitFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@SumCreditFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@OverAllNetTotalFinal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(8) = New SqlParameter("@OverAllPeroidTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(9) = New SqlParameter("@PreviewBalanceTotal", SqlDbType.Decimal) With {.Precision = 15, .Scale = 3, .Direction = ParameterDirection.Output}
            prm(10) = New SqlParameter("@LoadType", SqlDbType.Decimal) With {.Value = LOADTYPE}
            Me.GCROLE.DataSource = Nothing
            dt = RUN_QUARY_PRO("AccountsTb_LoadMainBranchSafesDetails", prm)
            If dt.Rows.Count > 0 Then
                Me.GCROLE.DataSource = dt
                Me.OverAllCredit.EditValue = prm(6).Value
                Me.OverAllDebit.EditValue = prm(5).Value
                Me.OverAllPeroidTotal.EditValue = prm(8).Value
                Me.PreviewsBalance.EditValue = prm(9)
                If Me.OverAllDebit.EditValue > Me.OverAllCredit.EditValue Then
                    Me.OverAllPeroidTotal.BackColor = Color.Green
                    Me.OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Green
                ElseIf Me.OverAllDebit.EditValue < Me.OverAllCredit.EditValue Then
                    Me.OverAllPeroidTotal.BackColor = Color.Red
                    Me.OverAllPeroidTotal.Properties.AppearanceDisabled.BackColor = Color.Red
                End If
                Dim PR1(1) As SqlParameter
                PR1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FrmMainSafeBalance.BranchID.EditValue}
                PR1(1) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = CO}
                Dim DT2 As New DataTable
                DT2 = RUN_QUARY_PRO("AccountsTb_MainBranchSafesDetailsNetTotal", PR1)
                If DT2.Rows(0)("أول") > DT2.Rows(0)("ثاني") Then
                    Me.OverAllTotal.EditValue = DT2.Rows(0)("أول")
                    Me.OverAllTotal.BackColor = Color.Green
                    OverallPrint = Format(DT2.Rows(0)("أول"), "N2")
                Else
                    Me.OverAllTotal.EditValue = DT2.Rows(0)("ثاني")
                    Me.OverAllTotal.BackColor = Color.Red
                    OverallPrint = Format(DT2.Rows(0)("ثاني"), "N2")
                End If
                Me.GVRole.Columns("طبيعة الحركة").Width = 400

                Me.DVGFROMAT()
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
                Me.GCROLE.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class