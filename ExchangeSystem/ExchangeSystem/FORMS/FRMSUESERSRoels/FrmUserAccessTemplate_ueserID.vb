Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Custom
Imports DevExpress.XtraEditors.Helpers
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FrmUserAccessTemplate_ueserID
    Public Function LOAD_Prof() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("ProfID_LoadDataIntoLookUpEdit")
        Return DT
    End Function
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(108, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BarButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BarButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then LayoutControlItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else LayoutControlItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then BarButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BarButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView2.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView2.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView4_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView4.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView4.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView5_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView5.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView5.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView6_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView6.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView6.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView7_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView7.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView7.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView8_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView8.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView8.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView9_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView9.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView9.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView10_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView10.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView10.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView11_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView11.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView11.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView13_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView13.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView13.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Sub LOADProfID()
        Dim DT As New DataTable
        DT.Clear()
        DT = LOAD_Prof()

        If DT.Rows.Count > 0 Then
            ProfID.Properties.DataSource = DT
            ProfID.Properties.DisplayMember = "ProfileName"
            ProfID.Properties.ValueMember = "ProfID"
            DVGFROMAT2(GridLookUpEdit1View)
        End If
    End Sub

    Public Sub Tb_ueser_lodeUeserForProfile()
        Try

            DVGFROMAT2(GridView12)

            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfID.EditValue}
            GridControl1.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("Tb_ueser_lodeUeserForProfile", prm)


            If dt.Rows.Count > 0 Then

                GridControl1.DataSource = dt
                BarButtonItem1.Caption = "عدد المستخدمين الذين تحت مجموعة" + Space(1) + ProfID.Text + Space(1) + "هم" + Space(1) + "(" + Convert.ToString(dt.Rows.Count) + ")"
            Else
                BarButtonItem1.Caption = "عدد المستخدمين الذين تحت مجموعة" + Space(1) + ProfID.Text + Space(1) + "هم" + Space(1) + "(" + Convert.ToString(dt.Rows.Count) + ")"
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        'SplashScreenManager1.CloseWaitForm()
    End Sub



    Private Sub FrmUserAccessTemplate_ueserID_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        LOADProfID()

        NoweRecord()
    End Sub
    Public Sub NoweRecord()
        TextEdit1.Properties.DataSource = Nothing
        TextEdit1.EditValue = -1
        TextEdit1.Text = String.Empty
        FILLGRids()
        TextEdit1.Properties.DataSource = Nothing
        TextEdit1.EditValue = -1
    End Sub

    Sub DVGFORMAT(GVRole As GridView)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True


        '===============================================================

    End Sub

    Sub DVGFROMAT2(GridView As GridView)

        GridView.OptionsBehavior.EditingMode = True
        GridView.OptionsBehavior.ReadOnly = True
        GridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GridView.OptionsView.ShowGroupPanel = False
        GridView.OptionsFind.AlwaysVisible = True

        GridView.ShowFindPanel()
        GridView.OptionsView.ShowFooter = False
        For i As Integer = 0 To GridView.Columns.Count - 1
            GridView.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GridView.Appearance.Row.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        GridView.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView.OptionsView.EnableAppearanceEvenRow = True
        GridView.Appearance.OddRow.BackColor = Color.Honeydew
        GridView.OptionsView.EnableAppearanceOddRow = True


    End Sub

    Private Sub ProfID_EditValueChanged(sender As Object, e As EventArgs) Handles ProfID.EditValueChanged
        If ProfID.EditValue > -1 Then
            SplashScreenManager1.ShowWaitForm()
            Tb_ueser_lodeUeserForProfile()
            NoweRecord()
            SplashScreenManager1.CloseWaitForm()
        End If
    End Sub

    Public Sub FrmScreensTb2_getMainid(minid As ULong, grid As GridControl, gvive As GridView, ProfileIDs As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfileIDs}
        prm(1) = New SqlParameter("@MainID", SqlDbType.Int) With {.Value = minid}

        Dim dt As New DataTable
        dt.Clear()
        grid.DataSource = Nothing
        dt = RUN_QUARY_PRO("UserAccessProfileTemplate_sharch", prm)
        If dt.Rows.Count > 0 Then
            grid.DataSource = dt
            DVGFORMAT(gvive)
        End If
    End Sub
    Public Sub FILLGRids()
        If ProfID.EditValue > -1 Then
            FrmScreensTb2_getMainid(1, GCRole, GVRole, ProfID.EditValue)
            FrmScreensTb2_getMainid(2, GridControl2, GridView2, ProfID.EditValue)
            FrmScreensTb2_getMainid(3, GridControl3, GridView4, ProfID.EditValue)
            FrmScreensTb2_getMainid(4, GridControl4, GridView5, ProfID.EditValue)
            FrmScreensTb2_getMainid(5, GridControl5, GridView6, ProfID.EditValue)
            FrmScreensTb2_getMainid(6, GridControl6, GridView7, ProfID.EditValue)
            FrmScreensTb2_getMainid(7, GridControl7, GridView8, ProfID.EditValue)
            FrmScreensTb2_getMainid(8, GridControl8, GridView9, ProfID.EditValue)
            FrmScreensTb2_getMainid(9, GridControl9, GridView10, ProfID.EditValue)
            FrmScreensTb2_getMainid(10, GridControl10, GridView11, ProfID.EditValue)
            FrmScreensTb2_getMainid(11, GridControl11, GridView1, ProfID.EditValue)
            FrmScreensTb2_getMainid(12, GridControl12, GridView13, ProfID.EditValue)



        End If
    End Sub


    Public Sub UserAccessProfileTemplate_sharch(ProfileIDs As Integer, Main As Integer, GCRoles As GridControl, gridviwe As GridView)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfileIDs}
        prm(1) = New SqlParameter("@MainID", SqlDbType.Int) With {.Value = Main}
        GCRoles.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        GCRoles.DataSource = Nothing
        dt = RUN_QUARY_PRO("UserAccessProfileTemplate_ID_sharch", prm)

        If dt.Rows.Count > 0 Then
            GCRoles.DataSource = dt
            DVGFORMAT(gridviwe)
        End If
    End Sub


    Public Sub fill_getdvg()
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 1, GCRole, GVRole)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 2, GridControl2, GridView2)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 3, GridControl3, GridView4)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 4, GridControl4, GridView4)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 5, GridControl5, GridView5)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 6, GridControl6, GridView6)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 7, GridControl7, GridView7)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 8, GridControl8, GridView8)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 9, GridControl9, GridView9)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 10, GridControl10, GridView11)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 11, GridControl11, GridView1)
        UserAccessProfileTemplate_sharch(GridView12.GetFocusedRowCellValue("USID"), 12, GridControl12, GridView13)

    End Sub


    Public Sub GET_TABPEG_FRO_ROLS_ueSER()
        If TextEdit1.EditValue > 0 Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@ProfileGID", SqlDbType.Int) With {.Value = GProfIDLog}
            PRM(1) = New SqlParameter("@USERID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
            Me.CHECKOFORMVISIBEL_FalseOrTrueME("UserAccessProfileTemplate_ueserId_roles", PRM, "ShortName", "CanShow")
        End If
    End Sub
    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView12.DoubleClick
        SplashScreenManager1.ShowWaitForm()
        loDEinTRP(GridView12.GetFocusedRowCellValue("USID"))
        fill_getdvg()
        GET_TABPEG_FRO_ROLS_ueSER()
        SplashScreenManager1.CloseWaitForm()
    End Sub
    Public Function UserAccessProfileTemplate_type() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("MainID")
        dt.Columns.Add(" Group_ID ")
        dt.Columns.Add("ScreenID")
        dt.Columns.Add("CanShow")
        dt.Columns.Add("CanSave")
        dt.Columns.Add("CanEdit")
        dt.Columns.Add("CanDelete")
        dt.Columns.Add("CanSearch")
        dt.Columns.Add("CanPrint")


        If GVRole.RowCount > 0 Then
            For i = 0 To GVRole.RowCount - 1
                dt.Rows.Add(GVRole.GetRowCellValue(i, "MainScreenID"), GVRole.GetRowCellValue(i, "GRoup_ID"), GVRole.GetRowCellValue(i, "ScreenID"),
                GVRole.GetRowCellValue(i, "CanShow"), GVRole.GetRowCellValue(i, "CanSave"), GVRole.GetRowCellValue(i, "CanEdit"), GVRole.GetRowCellValue(i, "CanDelete"),
                GVRole.GetRowCellValue(i, "CanSearch"), GVRole.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView2.RowCount > 0 Then
            For i = 0 To GridView2.RowCount - 1
                dt.Rows.Add(GridView2.GetRowCellValue(i, "MainScreenID"), GridView2.GetRowCellValue(i, "GRoup_ID"), GridView2.GetRowCellValue(i, "ScreenID"),
                GridView2.GetRowCellValue(i, "CanShow"), GridView2.GetRowCellValue(i, "CanSave"), GridView2.GetRowCellValue(i, "CanEdit"), GridView2.GetRowCellValue(i, "CanDelete"),
                GridView2.GetRowCellValue(i, "CanSearch"), GridView2.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView4.RowCount > 0 Then
            For i = 0 To GridView4.RowCount - 1
                dt.Rows.Add(GridView4.GetRowCellValue(i, "MainScreenID"), GridView4.GetRowCellValue(i, "GRoup_ID"), GridView4.GetRowCellValue(i, "ScreenID"),
                GridView4.GetRowCellValue(i, "CanShow"), GridView4.GetRowCellValue(i, "CanSave"), GridView4.GetRowCellValue(i, "CanEdit"), GridView4.GetRowCellValue(i, "CanDelete"),
                GridView4.GetRowCellValue(i, "CanSearch"), GridView4.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView5.RowCount > 0 Then
            For i = 0 To GridView5.RowCount - 1
                dt.Rows.Add(GridView5.GetRowCellValue(i, "MainScreenID"), GridView5.GetRowCellValue(i, "GRoup_ID"), GridView5.GetRowCellValue(i, "ScreenID"),
                GridView5.GetRowCellValue(i, "CanShow"), GridView5.GetRowCellValue(i, "CanSave"), GridView5.GetRowCellValue(i, "CanEdit"), GridView5.GetRowCellValue(i, "CanDelete"),
                GridView5.GetRowCellValue(i, "CanSearch"), GridView5.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView6.RowCount > 0 Then
            For i = 0 To GridView6.RowCount - 1
                dt.Rows.Add(GridView6.GetRowCellValue(i, "MainScreenID"), GridView6.GetRowCellValue(i, "GRoup_ID"), GridView6.GetRowCellValue(i, "ScreenID"),
                GridView6.GetRowCellValue(i, "CanShow"), GridView6.GetRowCellValue(i, "CanSave"), GridView6.GetRowCellValue(i, "CanEdit"), GridView6.GetRowCellValue(i, "CanDelete"),
                GridView6.GetRowCellValue(i, "CanSearch"), GridView6.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView7.RowCount > 0 Then
            For i = 0 To GridView7.RowCount - 1
                dt.Rows.Add(GridView7.GetRowCellValue(i, "MainScreenID"), GridView7.GetRowCellValue(i, "GRoup_ID"), GridView7.GetRowCellValue(i, "ScreenID"),
                GridView7.GetRowCellValue(i, "CanShow"), GridView7.GetRowCellValue(i, "CanSave"), GridView7.GetRowCellValue(i, "CanEdit"), GridView7.GetRowCellValue(i, "CanDelete"),
                GridView7.GetRowCellValue(i, "CanSearch"), GridView7.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView8.RowCount > 0 Then
            For i = 0 To GridView8.RowCount - 1
                dt.Rows.Add(GridView8.GetRowCellValue(i, "MainScreenID"), GridView8.GetRowCellValue(i, "GRoup_ID"), GridView8.GetRowCellValue(i, "ScreenID"),
                GridView8.GetRowCellValue(i, "CanShow"), GridView8.GetRowCellValue(i, "CanSave"), GridView8.GetRowCellValue(i, "CanEdit"), GridView8.GetRowCellValue(i, "CanDelete"),
                GridView8.GetRowCellValue(i, "CanSearch"), GridView8.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView9.RowCount > 0 Then
            For i = 0 To GridView9.RowCount - 1
                dt.Rows.Add(GridView9.GetRowCellValue(i, "MainScreenID"), GridView9.GetRowCellValue(i, "GRoup_ID"), GridView9.GetRowCellValue(i, "ScreenID"),
                GridView9.GetRowCellValue(i, "CanShow"), GridView9.GetRowCellValue(i, "CanSave"), GridView9.GetRowCellValue(i, "CanEdit"), GridView9.GetRowCellValue(i, "CanDelete"),
                GridView9.GetRowCellValue(i, "CanSearch"), GridView9.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView10.RowCount > 0 Then
            For i = 0 To GridView10.RowCount - 1
                dt.Rows.Add(GridView10.GetRowCellValue(i, "MainScreenID"), GridView10.GetRowCellValue(i, "GRoup_ID"), GridView10.GetRowCellValue(i, "ScreenID"),
                GridView10.GetRowCellValue(i, "CanShow"), GridView10.GetRowCellValue(i, "CanSave"), GridView10.GetRowCellValue(i, "CanEdit"), GridView10.GetRowCellValue(i, "CanDelete"),
                GridView10.GetRowCellValue(i, "CanSearch"), GridView10.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView11.RowCount > 0 Then
            For i = 0 To GridView11.RowCount - 1
                dt.Rows.Add(GridView11.GetRowCellValue(i, "MainScreenID"), GridView11.GetRowCellValue(i, "GRoup_ID"), GridView11.GetRowCellValue(i, "ScreenID"),
                GridView11.GetRowCellValue(i, "CanShow"), GridView11.GetRowCellValue(i, "CanSave"), GridView11.GetRowCellValue(i, "CanEdit"), GridView11.GetRowCellValue(i, "CanDelete"),
                GridView11.GetRowCellValue(i, "CanSearch"), GridView11.GetRowCellValue(i, "CanPrint"))
            Next
        End If


        If GridView1.RowCount > 0 Then
            For i = 0 To GridView1.RowCount - 1
                dt.Rows.Add(GridView1.GetRowCellValue(i, "MainScreenID"), GridView1.GetRowCellValue(i, "GRoup_ID"), GridView1.GetRowCellValue(i, "ScreenID"),
                GridView1.GetRowCellValue(i, "CanShow"), GridView1.GetRowCellValue(i, "CanSave"), GridView1.GetRowCellValue(i, "CanEdit"), GridView1.GetRowCellValue(i, "CanDelete"),
                GridView1.GetRowCellValue(i, "CanSearch"), GridView1.GetRowCellValue(i, "CanPrint"))
            Next
        End If


        If GridView13.RowCount > 0 Then
            For i = 0 To GridView13.RowCount - 1
                dt.Rows.Add(GridView13.GetRowCellValue(i, "MainScreenID"), GridView13.GetRowCellValue(i, "GRoup_ID"), GridView13.GetRowCellValue(i, "ScreenID"),
                GridView13.GetRowCellValue(i, "CanShow"), GridView13.GetRowCellValue(i, "CanSave"), GridView13.GetRowCellValue(i, "CanEdit"), GridView13.GetRowCellValue(i, "CanDelete"),
                GridView13.GetRowCellValue(i, "CanSearch"), GridView13.GetRowCellValue(i, "CanPrint"))
            Next
        End If
        Return dt
    End Function

    Public Sub [UserAccessProfileTemplate_ID_update](isupdate As Integer)
        Try

            SplashScreenManager1.ShowWaitForm()
            Dim prm(3) As SqlParameter

            prm(0) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
            prm(1) = New SqlParameter("@UserAccessProfileTemplate_ID_insertype", SqlDbType.Structured) With {.Value = UserAccessProfileTemplate_type()}
            prm(2) = New SqlParameter("@msg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(3) = New SqlParameter("@MSGStat", SqlDbType.Int) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("UserAccessProfileTemplate_ID_update", prm)
            FrmEditMessage.Show()
            NoweRecord()
            SplashScreenManager1.CloseWaitForm()
            FrmLogin.LodeSEcreen()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            SplashScreenManager1.CloseWaitForm()
        End Try

    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        Try

            If TextEdit1.EditValue = -1 Then
                TextEdit1.ErrorText = "هذا الحقل مطلوب الرجاء اختيار المستخدم"
                Exit Sub
            End If


            If UserAccessProfileTemplate_type.Rows.Count <= 0 Then
                MessageBox.Show("الرجاء اختيار المستخدم", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            ProfID.Select()
            UserAccessProfileTemplate_ID_update(1)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub loDEinTRP(UESERid As Integer)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfID.EditValue}
        TextEdit1.Properties.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("Tb_ueser_lodeUeserForProfile", prm)

        If dt.Rows.Count > 0 Then
            TextEdit1.Properties.DataSource = dt
            TextEdit1.Properties.DisplayMember = "UName"
            TextEdit1.Properties.ValueMember = "USID"
            TextEdit1.EditValue = UESERid
            TextEdit1.Enabled = False
        End If

    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        If TextEdit1.EditValue = -1 Then
            TextEdit1.ErrorText = "الرجاء أختيار اسم المستخدم"
            Exit Sub
        End If
        printCustomerDeleviryShipping_LoadDeliveredShipping1()
    End Sub

    Public Sub printCustomerDeleviryShipping_LoadDeliveredShipping1()
        'Try


        '    SplashScreenManager1.ShowWaitForm()
        '    Dim dt As New DataTable
        '    dt.Clear()
        '    Dim prm(1) As SqlParameter
        '    prm(0) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
        '    prm(1) = New SqlParameter("@Type_ID_Secrein", SqlDbType.Int) With {.Value = 1}
        '    dt = RUN_QUARY_PRO("RptUserAccessProfileTemplate_ueserId2", prm)

        '    Dim report As New RptUeserActvionForTepelte
        '    If dt.Rows.Count > 0 Then
        '        report.DetailReport.DataSource = dt
        '    Else
        '        report.DetailReport.Visible = False
        '    End If

        '    Dim dt2 As New DataTable
        '    dt2.Clear()
        '    Dim prm2(1) As SqlParameter
        '    prm2(0) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
        '    prm2(1) = New SqlParameter("@Type_ID_Secrein", SqlDbType.Int) With {.Value = 2}
        '    dt2 = RUN_QUARY_PRO("RptUserAccessProfileTemplate_ueserId2", prm2)
        '    If dt2.Rows.Count > 0 Then
        '        If dt2.Rows.Count > 0 Then
        '            report.DetailReport1.Visible = True
        '            report.DetailReport1.DataSource = dt2
        '        Else
        '            report.DetailReport1.Visible = False
        '        End If
        '    End If



        '    Dim dt3 As New DataTable
        '    dt3.Clear()
        '    Dim prm3(1) As SqlParameter
        '    prm3(0) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
        '    prm3(1) = New SqlParameter("@Type_ID_Secrein", SqlDbType.Int) With {.Value = 3}
        '    dt3 = RUN_QUARY_PRO("RptUserAccessProfileTemplate_ueserId2", prm3)
        '    If dt3.Rows.Count > 0 Then
        '        If dt3.Rows.Count > 0 Then
        '            report.DetailReport2.Visible = True
        '            report.DetailReport2.DataSource = dt3

        '        Else
        '            report.DetailReport2.Visible = False
        '        End If
        '    End If


        '    Dim dt4 As New DataTable
        '    dt4.Clear()
        '    Dim prm4(1) As SqlParameter
        '    prm4(0) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
        '    prm4(1) = New SqlParameter("@Type_ID_Secrein", SqlDbType.Int) With {.Value = 4}
        '    dt4 = RUN_QUARY_PRO("RptUserAccessProfileTemplate_ueserId2", prm4)
        '    If dt4.Rows.Count > 0 Then
        '        If dt4.Rows.Count > 0 Then
        '            report.DetailReport3.Visible = True
        '            report.DetailReport3.DataSource = dt4
        '        Else
        '            report.DetailReport3.Visible = False
        '        End If
        '    End If



        '    Dim dt5 As New DataTable
        '    dt5.Clear()
        '    Dim prm5(1) As SqlParameter
        '    prm5(0) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = TextEdit1.EditValue}
        '    prm5(1) = New SqlParameter("@Type_ID_Secrein", SqlDbType.Int) With {.Value = 5}
        '    dt5 = RUN_QUARY_PRO("RptUserAccessProfileTemplate_ueserId2", prm5)
        '    If dt5.Rows.Count > 0 Then
        '        If dt5.Rows.Count > 0 Then
        '            report.DetailReport4.Visible = True
        '            report.DetailReport4.DataSource = dt5
        '        Else
        '            report.DetailReport4.Visible = False
        '        End If
        '    End If


        '    Dim dtHeader As DataTable = RUN_QUARY_PRO("TB_Users_Roll_NAME_forPRofilr",
        '                                       New SqlParameter() {New SqlParameter("@USID", TextEdit1.EditValue)})

        '    If dtHeader.Rows.Count > 0 Then
        '        report.XrLabel14.Text = "اسم:" + Space(1) + dtHeader.Rows(0)("UName")
        '        report.XrLabel2.Text = ": الصلاحية" + Space(1) + dtHeader.Rows(0)("ProfileName")
        '        report.XrLabel4.Text = dtHeader.Rows(0)("BName")
        '    End If


        '    SplashScreenManager1.CloseWaitForm()
        '    Dim tool As ReportPrintTool = New ReportPrintTool(report)
        '    report.CreateDocument()
        '    report.ShowPreview()
        '    dt2.Dispose()
        '    dt.Dispose()

        '    If SQLCON.State = ConnectionState.Open Then
        '        SQLCON.Close()
        '    End If
        'Catch ex As Exception
        '    ErrorMessage(Me, " Catch ex As Exception printCustomerDeleviryShipping_LoadDeliveredShipping1 ", ex.Message)
        'End Try
        ''    End Using
        ''End Using
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        If TextEdit1.EditValue = -1 Then
            TextEdit1.ErrorText = "الرجاء اختيار اسم المستخدم"
            Return
        End If
        If ProfID.EditValue = -1 Then
            ProfID.ErrorText = "الرجاء اختيار نوع الصلاحية"
            Return
        End If
        FRm_add_setting_uesers.MeShoweForSEtiing(TextEdit1.EditValue, ProfID.EditValue)
        FRm_add_setting_uesers.ShowDialog()
    End Sub

    Public Sub CHECKOFORMVISIBEL_FalseOrTrueME(stordNAMe As String, prm() As SqlParameter,
                                           fieldNameShortName As String, fieldNameCanShow As String)
        Try

            Dim DT As New DataTable
            DT = RUN_QUARY_PRO(stordNAMe, prm)


            If DT.Rows.Count > 0 Then

                For Each row As DataRow In DT.Rows

                    Dim controlName As String = row(fieldNameShortName).ToString()
                    Dim canShow As Boolean = Convert.ToBoolean(row(fieldNameCanShow))

                    Dim control = Me.Controls.Find(controlName, True).FirstOrDefault()

                    If control IsNot Nothing Then

                        If TypeOf control Is DevExpress.XtraTab.XtraTabPage Then
                            'Dim tabControl As DevExpress.XtraTab.XtraTabControl = DirectCast(control, DevExpress.XtraTab.XtraTabControl)
                            ' MsgBox(tabControl)

                            If التطبيق.TabPages.Count > 0 Then

                                For Each tabPage As DevExpress.XtraTab.XtraTabPage In التطبيق.TabPages

                                    If tabPage.Name = controlName Then

                                        tabPage.PageVisible = canShow
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            Else
                ' في حال كانت النتيجة فارغة أو لا توجد صلاحيات
                ErrorMessage(FrmLogin, "رسالة خطأ", "عذرا، هذا المستخدم لا يمتلك صلاحيات حالياً. الرجاء إضافة صلاحيات له من خلال التواصل مع مدير النظام.")
                Application.Exit()
            End If
        Catch ex As Exception
            ' عرض رسالة خطأ مع تفاصيل الاستثناء
            MessageBox.Show($"حدث خطأ: {ex.Message}", "رسالة خطأ من النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Application.Exit()
        End Try
    End Sub

    Private Sub GridView12_DetailTabStyle(sender As Object, e As DetailTabStyleEventArgs) Handles GridView12.DetailTabStyle

    End Sub
End Class
