Imports DevExpress.Data
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient

Public Class FRMBANKSERVICESTATEMENTS
    Public Frmid As Integer
    'Sub LOADBRANCH()
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
    '    BranchID.Properties.DataSource = DT
    '    BranchID.Properties.ValueMember = "DBRID"
    '    BranchID.Properties.DisplayMember = "BName"
    '    BranchID.Properties.ShowHeader = False
    'End Sub
    Sub LOADBRANCHSERVICES()
        'Dim PR(0) As SqlParameter
        'PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
        'LoadToControlar(BBranchID, "BBranchTb_LoadBasedOnServices", "BranchName", "ID", Nothing)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@CountryId", SqlDbType.Int)
        PRM(0).Value = COUNTRYNID
        LoadToControlar(BBranchID, "BanksTb_LOADTOLKP_BasedONCountryID", "BankName", "BNKID", PRM)
    End Sub
    Sub LOADSERCICETYPE()
        ServiceID.Properties.DataSource = Nothing
        ServiceID.EditValue = -1
        LoadToControlar(ServiceID, "BankService_LoadALLToLKP", "ServiceName", "ID", Nothing, True)
    End Sub
    Public Sub LoadData()
        NEWDVGFROMAT(GVRole)
        If BranchID.EditValue = -1 Or BranchID.Text = String.Empty Then
            BranchID.ErrorText = "الرجاء اختيار الفرع"
            Exit Sub
        End If


        If D1.Text = String.Empty Then
            D1.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If


        If D2.Text = String.Empty Then
            D2.ErrorText = "هذا الحقل مطلوب"
            Exit Sub
        End If

        If D1.EditValue > D2.EditValue Then
            D1.ErrorText = "عذراً لايمكن ان يكون التاريخ الاول اكبر من تاريخ الثاني"
            Exit Sub
        End If
        If ServiceID.EditValue = -1 Or ServiceID.Text = String.Empty Then
            ServiceID.ErrorText = "الرجاء اختيار نوع الخدمة"
            Exit Sub
        End If
        If EMPID.EditValue = -1 Or EMPID.Text = String.Empty Then
            EMPID.ErrorText = "الرجاء اختيار الموظف"
            Exit Sub
        End If
        Try
            GCROLE.DataSource = Nothing
            OverAllDebit.EditValue = 0.00
            OverAllCredit.EditValue = 0.00
            OverAllTotal.EditValue = 0.00
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(4) As SqlParameter
            prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID.EditValue}
            prm(1) = New SqlParameter("@BBranchID", SqlDbType.Int) With {.Value = ServiceID.EditValue}
            prm(2) = New SqlParameter("@UserID", SqlDbType.BigInt) With {.Value = EMPID.EditValue}
            prm(3) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1.EditValue}
            prm(4) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2.EditValue}
            dt = RUN_QUARY_PRO("AccSafeActivityTb_SelectBankServices", prm)
            If dt.Rows.Count > 0 Then
                GCROLE.DataSource = dt
                NEWDVGFROMAT(GVRole)
                GVRole.Columns("مدين").AppearanceCell.BackColor = Color.Green
                GVRole.Columns("دائن").AppearanceCell.BackColor = Color.Red
                'GVRole.Columns("الإجمالي").AppearanceCell.BackColor = Color.Green
                'GVRole.Columns("الإجمالي").AppearanceCell.ForeColor = Color.White
                GVRole.Columns("مدين").AppearanceCell.ForeColor = Color.White
                GVRole.Columns("دائن").AppearanceCell.ForeColor = Color.White
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub FRMBANKSERVICESTATEMENTS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LOADBRNCHDIERCT(BranchID)
        LOADBRANCHSERVICES()
        BranchID.EditValue = -1
        EMPID.EditValue = -1
        LOADSERCICETYPE()
        ''Thread.CurrentThread.CurrentUICulture = CultureInfo
        NEWDVGFROMAT(GVRole)
        BranchID.EditValue = BID
        GCROLE.DataSource = Nothing
        OverAllDebit.EditValue = 0.00
        OverAllCredit.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        TabbedControlGroup2.SelectedTabPageIndex = 0
        NEWDVGFROMAT(GVRole)
        D1.EditValue = Date.Now
        D2.EditValue = Date.Now
        If UserType = 1 Then
            BranchID.Enabled = True
        Else
            BranchID.Enabled = False
        End If

        LoadToControlar(EMPID, "EmployeeTb_LOADINTOLKP_CanUseBankServies", "UName", "USID", Nothing, True)

    End Sub

    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        'If BranchID.EditValue <> -1 Or BranchID.Text <> String.Empty Then
        '    LOADBRANCHSERVICES()
        'End If
    End Sub

    Private Sub BBranchID_TextChanged(sender As Object, e As EventArgs) Handles BBranchID.TextChanged
        'If BBranchID.EditValue <> -1 Or BBranchID.Text <> String.Empty Then
        '    LOADSERCICETYPE()
        'End If
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        LoadData()
    End Sub


    Sub Summ()
        OverAllDebit.EditValue = 0.00
        OverAllCredit.EditValue = 0.00
        OverAllTotal.EditValue = 0.00

        Dim CreditSum As New GridColumnSummaryItem()
        CreditSum.SummaryType = SummaryItemType.Sum
        CreditSum.FieldName = "دائن"
        GVRole.Columns("دائن").Summary.Add(CreditSum)
        Dim DebitSum As New GridColumnSummaryItem()
        DebitSum.SummaryType = SummaryItemType.Sum
        DebitSum.FieldName = "مدين"
        GVRole.Columns("مدين").Summary.Add(DebitSum)

        'Dim Total As New GridColumnSummaryItem()
        'Total.SummaryType = SummaryItemType.Sum
        'Total.FieldName = "الإجمالي"
        'GVRole.Columns("الإجمالي").Summary.Add(Total)

        OverAllDebit.EditValue = GVRole.Columns("مدين").SummaryItem.SummaryValue
        OverAllDebit.Properties.Appearance.BackColor = Color.Red

        OverAllCredit.EditValue = GVRole.Columns("دائن").SummaryItem.SummaryValue

        'OverAllTotal.EditValue = GVRole.Columns("الإجمالي").SummaryItem.SummaryValue

    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
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

    Private Sub GVRole_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GVRole.FocusedRowChanged
        Summ()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
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
            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", BranchID.EditValue)
            PRM(1) = New SqlParameter("@D1", D1.EditValue)
            PRM(2) = New SqlParameter("@D2", D2.EditValue)
            PRM(3) = New SqlParameter("@BBranchID", ServiceID.EditValue)
            PRM(4) = New SqlParameter("@UserID", EMPID.EditValue)
            Dim dt As DataTable = RUN_QUARY_PRO("ZRPT_AccSafeActivityTb_SelectBankServices", PRM)
            dt.TableName = "AccountsTb"
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            If dt.Rows.Count > 0 Then
                Dim report As New RPTBANKSERVICESTATEMENTS
                report.DataSource = ds
                report.DataMember = "AccountsTb"
                Dim tool As ReportPrintTool = New ReportPrintTool(report)
                report.CreateDocument()
                report.ShowPreview()
                'Else
                '   XtraMessageBox.Show(lookFeelError, "لا يوجد بيانات لعرضها في هذا التاريخ", "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub ServiceID_EditValueChanged(sender As Object, e As EventArgs) Handles ServiceID.EditValueChanged
        GCROLE.DataSource = Nothing
        OverAllDebit.EditValue = 0.00
        OverAllCredit.EditValue = 0.00
        OverAllTotal.EditValue = 0.00
        If ServiceID.EditValue > 0 And ServiceID.Text <> String.Empty Then
            BBranchID.EditValue = GetLKPColumnVal(ServiceID, "BankID")
        End If
    End Sub
End Class