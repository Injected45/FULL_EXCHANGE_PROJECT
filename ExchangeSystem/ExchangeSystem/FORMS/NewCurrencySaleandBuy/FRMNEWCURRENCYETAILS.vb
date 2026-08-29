Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Public Class FRMNEWCURRENCYETAILS
    Public C, t, BRID, COUNID, BANID As Integer
    Sub NEWRECORD()
        LoadCountry()
        CountryID.EditValue = -1
        PriceType.SelectedIndex = -1
        'BankID.Enabled = False
        BankID.EditValue = -1
        AccountType.SelectedIndex = -1
        AccountType.Enabled = False
        BranchID.EditValue = -1
        BranchID.Enabled = False
        GCROLE1.DataSource = Nothing
        GCROLE1.Visible = True
        GCROLE2.Visible = True
        GCROLE2.DataSource = Nothing
    End Sub
    Public Sub LOADCIDFROM()
        Try
            'If PriceType.SelectedIndex <> 3 Then
            GCROLE2.DataSource = Nothing
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = PriceType.SelectedIndex}
            prm(1) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
            prm(2) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = SafeToInt(BankID.EditValue)}
            Dim DT As New DataTable
            DT = RUN_QUARY_PRO("NewCurrencyMainTb_LOADTOLKP_MAIN", prm)
            If DT.Rows.Count > 0 Then
                GCROLE2.DataSource = DT
                TileView1Format()
                DVGFormat()
            End If
            'End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة تنبيه", ex.Message)
        End Try
    End Sub
    Sub DVGFormat()
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = True
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsView.ShowFooter = False
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Sub TileView1Format()
        TView.OptionsBehavior.EditingMode = True
        TView.OptionsBehavior.ReadOnly = True
        TView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        For i As Integer = 0 To TView.Columns.Count - 1
            TView.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            TView.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            TView.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            TView.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            TView.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        Next
    End Sub
#Region "LOADCONTROLS"
    Public Sub LoadCountry()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CountriesTb_LoadToGViewLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "CouID"
            CountryID.Properties.DisplayMember = "CountryName"
            NEWDVGFROMAT(CountryGV)
        End If
    End Sub
    Public Sub LoadAgent()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("CountryID", SqlDbType.Int) With {.Value = CountryID.EditValue}
        BranchID.Properties.DataSource = Nothing
        If AccountType.SelectedIndex = 1 Then
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LoadAgentHasCurrencyPrices", PR)
            If DT.Rows.Count > 0 Then
                BranchID.Properties.DataSource = DT
                BranchID.Properties.ValueMember = "DBRID"
                BranchID.Properties.DisplayMember = "BName"
                NEWDVGFROMAT(AgentGV)
            Else
                BranchID.Properties.DataSource = Nothing
            End If
        ElseIf AccountType.SelectedIndex = 2 Then
            Dim DTT As New DataTable
            DTT.Clear()
            DTT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPNotSelectCOUNTRY", PR)
            If DTT.Rows.Count > 0 Then
                BranchID.Properties.DataSource = DTT
                BranchID.Properties.ValueMember = "DBRID"
                BranchID.Properties.DisplayMember = "BName"
                NEWDVGFROMAT(AgentGV)
            Else
                BranchID.Properties.DataSource = Nothing
            End If
        ElseIf AccountType.SelectedIndex = 0 Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@CountryID", SqlDbType.Int)
            PRM(0).Value = CountryID.EditValue
            PRM(1) = New SqlParameter("@TransType", SqlDbType.Int)
            PRM(1).Value = 0
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CoBranches_LOADTOGLKPWITHTransType", PRM)
            If DT.Rows.Count > 0 Then
                BranchID.Properties.DataSource = DT
                BranchID.Properties.ValueMember = "DBRID"
                BranchID.Properties.DisplayMember = "BName"
                NEWDVGFROMAT(AgentGV)
            Else
                BranchID.Properties.DataSource = Nothing
            End If
        End If
    End Sub
    Public Sub LoadBank()
        BankID.Properties.DataSource = Nothing
        BankID.EditValue = -1
        If IsEmpty(CountryID) Or IsEmpty(PriceType) Then Exit Sub
        If PriceType.SelectedIndex = 2 Then
            Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = SafeToInt(CountryID.EditValue)}
            LoadToControlar(BankID, "TransTypeTb_SelectByCountry", "SRNAME", "SRID", PR)
        Else
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@CountryId", SqlDbType.Int)
            PRM(0).Value = SafeToInt(CountryID.EditValue)
            LoadToControlar(BankID, "BanksTb_LOADTOLKP_2026", "BankName", "BNKID", PRM, True, "نقدا")
        End If
    End Sub
#End Region
    Private Sub PriceType_TextChanged(sender As Object, e As EventArgs) Handles PriceType.TextChanged
        GCROLE1.DataSource = Nothing
        GCROLE2.DataSource = Nothing
        BranchID.EditValue = -1
        AccountType.SelectedIndex = -1
        BranchID.Properties.DataSource = Nothing
        LoadBank()
        If PriceType.SelectedIndex = 0 Then
            BranchID.Enabled = False
            'BankID.Enabled = True
            AccountType.Enabled = False
        ElseIf PriceType.SelectedIndex = 1 Then
            AccountType.Enabled = False
            AccountType.SelectedIndex = 0
            BranchID.Enabled = False
            'BankID.Enabled = True
        ElseIf PriceType.SelectedIndex = 2 Then
            AccountType.Enabled = True
            'BankID.Enabled = False
            If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Then
                BranchID.Enabled = True
            Else
                BranchID.Enabled = True
            End If
        ElseIf PriceType.SelectedIndex = 3 Then
            BranchID.Enabled = False
            AccountType.Enabled = False
            'BankID.Enabled = True

            BANID = SafeToInt(BankID.EditValue)
        End If
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(2, 84, 100), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ErrorMessage2("رسالة خطأ", "لا يوجد بيانات")
        GCROLE1.DataSource = Nothing

        If CountryID.EditValue = -1 Then
            CountryID.ErrorText = "يجب اختيار الدولة"
            Exit Sub
        End If
        If PriceType.SelectedIndex = -1 Then
            PriceType.ErrorText = "يجب تحديد نوع التعسير"
            Exit Sub
        End If
        If PriceType.SelectedIndex = 2 Then
            If AccountType.SelectedIndex = -1 Then
                AccountType.ErrorText = "يجب اختيار الحساب"
                Exit Sub
            End If
        End If
        If PriceType.SelectedIndex = 2 Then
            If AccountType.SelectedIndex = 1 Then
                If BranchID.EditValue = -1 Then
                    BranchID.ErrorText = "يجب اختيار الفرع"
                    Exit Sub
                End If
            End If
        End If

        'If PriceType.SelectedIndex <> 2 Then
        If BankID.EditValue = -1 Then
                BankID.ErrorText = "يجب اختيار التسعير"
                Exit Sub
            End If
        'End If

        LOADCIDFROM()

    End Sub
    Private Sub TView_DoubleClick(sender As Object, e As EventArgs) Handles TView.DoubleClick
        If TView.RowCount > 0 Then

            LOADATA(TView.GetFocusedRowCellValue("ID"), AccountType.SelectedIndex, BRID, CountryID.EditValue, SafeToInt(BankID.EditValue), PriceType.SelectedIndex)
        End If
    End Sub
    Public Sub LOADATA(CurrencyIDFrom As Integer, AccounType As Integer, BranchID As Integer, CountryID As Integer, BankID As Integer, PriceTyp As Integer)
        Try
            GCROLE1.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(6) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyIDFrom", SqlDbType.Int) With {.Value = CurrencyIDFrom}
            prm(1) = New SqlParameter("@MSG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(2) = New SqlParameter("@AccounType", SqlDbType.Int) With {.Value = AccounType}
            prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            prm(4) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
            prm(5) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID}
            prm(6) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = PriceTyp}
            dt = RUN_QUARY_PRO("NEWCurrencyPriceDetailsTb_Grid", prm)
            If dt.Rows.Count > 0 Then
                Me.GCROLE1.DataSource = dt
                FRMCURRENCYPRICEDTTELSS.LabelControl2.Text = prm(1).Value
                'If PriceType = 1 Then
                If CurrencyIDFrom = 1 Then
                    GVRole.Columns("cluemnsEdit").Visible = True
                Else
                    GVRole.Columns("cluemnsEdit").Visible = False
                End If
            Else
                GVRole.Columns("cluemnsEdit").Visible = True
            End If
            'End If
            C = CurrencyIDFrom
            t = PriceTyp
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub BranchID_TextChanged(sender As Object, e As EventArgs) Handles BranchID.TextChanged
        GCROLE1.DataSource = Nothing
        GCROLE2.DataSource = Nothing
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Then
            BRID = AgentGV.GetFocusedRowCellValue("DBRID")
        End If
    End Sub

    Private Sub CountryID_TextChanged(sender As Object, e As EventArgs) Handles CountryID.TextChanged
        GCROLE1.DataSource = Nothing
        GCROLE2.DataSource = Nothing
        LoadBank()
    End Sub

    Private Sub BankID_TextChanged(sender As Object, e As EventArgs) Handles BankID.TextChanged

        BANID = SafeToInt(BankID.EditValue)

    End Sub



    Private Sub AccountType_TextChanged(sender As Object, e As EventArgs) Handles AccountType.TextChanged
        GCROLE1.DataSource = Nothing
        GCROLE2.DataSource = Nothing
        BranchID.EditValue = -1
        BranchID.Properties.DataSource = Nothing
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Then
            'BankID.Enabled = False
            BranchID.Enabled = True
        Else
            BranchID.Enabled = True
        End If
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Or AccountType.SelectedIndex = 0 Then
            LoadAgent()
            BranchID.Enabled = True
            BRID = AgentGV.GetFocusedRowCellValue("DBRID")
        Else
            BranchID.Enabled = False
        End If
        If AccountType.SelectedIndex = 0 Then
            LayoutControlItem8.Text = "الحساب"
        End If
        If AccountType.SelectedIndex = 1 Then
            LayoutControlItem8.Text = "الوكيل"
        End If
        If AccountType.SelectedIndex = 2 Then
            LayoutControlItem8.Text = "الفرع"
        End If
    End Sub

    Private Sub RepositoryItemButtonEdit1_Click(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.Click
        FRMBTNEOWNCURRENCYEDIT.PriceType.SelectedIndex = PriceType.SelectedIndex
        If PriceType.SelectedIndex = 2 Then
            FRMBTNEOWNCURRENCYEDIT.AccountType = AccountType.SelectedIndex
        End If
        If PriceType.SelectedIndex = 2 Then
            If AccountType.SelectedIndex = 1 Then
                FRMBTNEOWNCURRENCYEDIT.AgentID = BRID
            End If
        End If
        'If PriceType.SelectedIndex = 3 Then
        If AccountType.SelectedIndex = 1 Then
            FRMBTNEOWNCURRENCYEDIT.BankID = SafeToInt(BankID.EditValue)
        End If
        'End If
        FRMBTNEOWNCURRENCYEDIT.CurrencyPriceCategory(GVRole.GetFocusedRowCellValue("IDCruns"), PriceType.SelectedIndex, 1)
    End Sub

    Private Sub BankID_EditValueChanged(sender As Object, e As EventArgs) Handles BankID.EditValueChanged
        GCROLE1.DataSource = Nothing
        GCROLE2.DataSource = Nothing
        SafeToInt(BankID.EditValue)
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        GCROLE1.DataSource = Nothing
        GCROLE2.DataSource = Nothing
        If AccountType.SelectedIndex = 1 Or AccountType.SelectedIndex = 2 Then
            BRID = AgentGV.GetFocusedRowCellValue("DBRID")
        End If
    End Sub
End Class