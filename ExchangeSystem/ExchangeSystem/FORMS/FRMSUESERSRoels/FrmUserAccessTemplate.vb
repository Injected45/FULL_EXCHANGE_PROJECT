Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.Data.SqlClient
Public Class FrmUserAccessTemplate

    Sub DVGFROMAT2(GridView As GridView)

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
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(106, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BarButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BarButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BarButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BarButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BarButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BarButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanSearch") = 0 Then BarButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BarButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub

    Public Sub FrmMainTbgetdvg()
        Try
            ' إنشاء DataTable وDataSet
            Dim dtMaster As New DataTable

            Dim ds As New DataSet

            ' استدعاء الإجراءات للحصول على البيانات
            dtMaster = RUN_QUARY_PRO_ONLY("FrmMainTbgetdvg") ' الجدول الرئيسي


            ' التحقق من وجود بيانات
            If dtMaster.Rows.Count > 0 Then
                ' إضافة الجداول إلى DataSet
                ds.Tables.Add(dtMaster)



                GridControl1.DataSource = ds.Tables(0)

            Else
                MessageBox.Show("لا توجد بيانات للعرض.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء تحميل البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Public Sub Group_main_DVF(main_ID As Integer, gcvrol As GridControl, Gviwe As GridView)
        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@main_ID", SqlDbType.Int) With {.Value = main_ID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("Group_main_DVF", prm)
            If dt.Rows.Count > 0 Then

                gcvrol.DataSource = dt
                DVGFROMAT2(Gviwe)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    '' جلب القروبات الفرعية ------------------------------------
    Public Sub LodeGroupGrid()
        Group_main_DVF(1, GridControl11, GridView12)
        Group_main_DVF(2, GridControl12, GridView13)
        Group_main_DVF(3, GridControl13, GridView14)
        Group_main_DVF(4, GridControl14, GridView15)
        Group_main_DVF(5, GridControl15, GridView16)
        Group_main_DVF(6, GridControl16, GridView17)
        Group_main_DVF(7, GridControl17, GridView18)
        Group_main_DVF(8, GridControl18, GridView19)
        Group_main_DVF(9, GridControl19, GridView20)
        Group_main_DVF(10, GridControl20, GridView21)
        Group_main_DVF(11, GridControl21, GridView22)
        Group_main_DVF(12, GridControl23, GridView2)

    End Sub

    Public Function UserROOLMAINPROFILETP_INSERTR() As DataTable
        Dim dt As New DataTable
        Try
            ' إعداد أعمدة الجدول
            dt.Columns.Add("canshow")
            dt.Columns.Add("mainid")

            ' تعبئة البيانات من GridView الرئيسي
            For i As Integer = 0 To GridView1.RowCount - 1
                dt.Rows.Add(GridView1.GetRowCellValue(i, "CanShow"), GridView1.GetRowCellValue(i, "MainID"))
            Next
        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الرئيسي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function


    Public Function Ueser_Group_main_insertType() As DataTable
        Dim dt As New DataTable
        Try
            ' إعداد أعمدة الجدول
            dt.Columns.Add("GroupID")
            dt.Columns.Add("CanShow")
            dt.Columns.Add("MainID")
            For i As Integer = 0 To GridView12.RowCount - 1
                dt.Rows.Add(GridView12.GetRowCellValue(i, "id"), GridView12.GetRowCellValue(i, "Canshowe"), GridView12.GetRowCellValue(i, "Main_ID"))
            Next
            '' 2


            For i As Integer = 0 To GridView13.RowCount - 1
                dt.Rows.Add(GridView13.GetRowCellValue(i, "id"), GridView13.GetRowCellValue(i, "Canshowe"), GridView13.GetRowCellValue(i, "Main_ID"))
            Next
            ''3
            For i As Integer = 0 To GridView14.RowCount - 1
                dt.Rows.Add(GridView14.GetRowCellValue(i, "id"), GridView14.GetRowCellValue(i, "Canshowe"), GridView14.GetRowCellValue(i, "Main_ID"))
            Next
            ''4
            For i As Integer = 0 To GridView15.RowCount - 1
                dt.Rows.Add(GridView15.GetRowCellValue(i, "id"), GridView15.GetRowCellValue(i, "Canshowe"), GridView15.GetRowCellValue(i, "Main_ID"))
            Next
            ''5
            For i As Integer = 0 To GridView16.RowCount - 1
                dt.Rows.Add(GridView16.GetRowCellValue(i, "id"), GridView16.GetRowCellValue(i, "Canshowe"), GridView16.GetRowCellValue(i, "Main_ID"))
            Next
            ''6
            For i As Integer = 0 To GridView17.RowCount - 1
                dt.Rows.Add(GridView17.GetRowCellValue(i, "id"), GridView17.GetRowCellValue(i, "Canshowe"), GridView17.GetRowCellValue(i, "Main_ID"))
            Next

            ''7
            For i As Integer = 0 To GridView18.RowCount - 1
                dt.Rows.Add(GridView18.GetRowCellValue(i, "id"), GridView18.GetRowCellValue(i, "Canshowe"), GridView18.GetRowCellValue(i, "Main_ID"))
            Next
            ''8
            For i As Integer = 0 To GridView19.RowCount - 1
                dt.Rows.Add(GridView19.GetRowCellValue(i, "id"), GridView19.GetRowCellValue(i, "Canshowe"), GridView19.GetRowCellValue(i, "Main_ID"))
            Next
            ''9
            For i As Integer = 0 To GridView20.RowCount - 1
                dt.Rows.Add(GridView20.GetRowCellValue(i, "id"), GridView20.GetRowCellValue(i, "Canshowe"), GridView20.GetRowCellValue(i, "Main_ID"))
            Next

            ''10
            For i As Integer = 0 To GridView21.RowCount - 1
                dt.Rows.Add(GridView21.GetRowCellValue(i, "id"), GridView21.GetRowCellValue(i, "Canshowe"), GridView21.GetRowCellValue(i, "Main_ID"))
            Next
            ''11
            For i As Integer = 0 To GridView22.RowCount - 1
                dt.Rows.Add(GridView22.GetRowCellValue(i, "id"), GridView22.GetRowCellValue(i, "Canshowe"), GridView22.GetRowCellValue(i, "Main_ID"))
            Next
            ''12
            For i As Integer = 0 To GridView2.RowCount - 1
                dt.Rows.Add(GridView2.GetRowCellValue(i, "id"), GridView2.GetRowCellValue(i, "Canshowe"), GridView2.GetRowCellValue(i, "Main_ID"))
            Next


        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الفرعي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function







    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles Man.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = Man.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles SRF.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = SRF.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView4_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles Crun.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = Crun.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView5_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles SNM.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = SNM.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView6_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles Rs.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = Rs.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView7_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles EM.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = EM.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView8_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles BAnk.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = BAnk.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView9_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView9.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView9.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView10_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GM.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GM.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView11_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles ST.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = ST.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView12_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles ST.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = ST.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Public Sub ProfileNamegetmax()
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("ProfileNamegetmax")
        If dt.Rows.Count > 0 Then
            ProfID.Text = dt.Rows(0)("id")
        End If

    End Sub
    Public Sub FrmScreensTb2_getMainid(minid As ULong, grid As GridControl, gvive As GridView)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@minid", SqlDbType.Int) With {.Value = minid}
        grid.DataSource = Nothing
        Dim dt As New DataTable
        dt = RUN_QUARY_PRO("FrmScreensTb2_getMainid", prm)
        If dt.Rows.Count > 0 Then
            grid.DataSource = dt
            DVGFORMAT(gvive)
        End If
    End Sub

    Sub DVGFORMAT(gridView As GridView)
        Try


            For i As Integer = 0 To gridView.Columns.Count - 1
                gridView.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                gridView.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
                gridView.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
                gridView.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            Next
            gridView.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
            gridView.OptionsView.EnableAppearanceEvenRow = True
            gridView.Appearance.OddRow.BackColor = Color.WhiteSmoke
            gridView.OptionsView.EnableAppearanceOddRow = True

            gridView.Columns("GroupNAme").Visible = True
            gridView.Columns("GroupNAme").VisibleIndex = 1
            gridView.Columns("GroupNAme").Width = 150
            gridView.Columns("ScreenName").Width = 250
            gridView.Columns("SN").Width = 60
            gridView.Columns("CanShow").Width = 90
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ: {ex.Message}")
        End Try
    End Sub

    Public Sub FILLGRids()
        FrmScreensTb2_getMainid(1, GCRole, Man)
        FrmScreensTb2_getMainid(2, GridControl2, SRF)
        FrmScreensTb2_getMainid(3, GridControl3, Crun)
        FrmScreensTb2_getMainid(4, GridControl4, SNM)
        FrmScreensTb2_getMainid(5, GridControl5, Rs)
        FrmScreensTb2_getMainid(6, GridControl6, EM)
        FrmScreensTb2_getMainid(7, GridControl7, BAnk)
        FrmScreensTb2_getMainid(8, GridControl8, GridView9)
        FrmScreensTb2_getMainid(9, GridControl9, GM)
        FrmScreensTb2_getMainid(10, GridControl10, ST)
        FrmScreensTb2_getMainid(11, GridControl22, MO)
        FrmScreensTb2_getMainid(12, GridControl24, GridView4)

    End Sub

    Public Sub insertprofilename(isupdate As Integer)
        Try
            SplashScreenManager1.ShowWaitForm()

            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("@IDPROFILE", SqlDbType.Int) With {.Value = ProfID.Text}
            prm(1) = New SqlParameter("@NAMEPROFILE", SqlDbType.NVarChar - 1) With {.Value = TextEdit1.Text}
            prm(2) = New SqlParameter("@TypeUserROOLMAIN", SqlDbType.Structured) With {.Value = UserROOLMAINPROFILETP_INSERTR()}
            prm(3) = New SqlParameter("@Ueser_Group_main_insertType", SqlDbType.Structured) With {.Value = Ueser_Group_main_insertType()}
            prm(4) = New SqlParameter("@UserAccessProfileTemplate_type", SqlDbType.Structured) With {.Value = UserAccessProfileTemplate_type()}
            prm(5) = New SqlParameter("@OperationType", SqlDbType.Bit) With {.Value = isupdate}
            prm(6) = New SqlParameter("@msg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@MSGStat", SqlDbType.Int) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("insertprofilename", prm)
            If prm(7).Value = 0 Then
                MessageBox.Show(prm(6).Value, "رسالة تنبية ", MessageBoxButtons.OK, MessageBoxIcon.Error)

                btnnew.PerformClick()
            Else

                btnnew.PerformClick()
                FrmSavedSuccessfully.Show()
            End If
            SplashScreenManager1.CloseWaitForm()




        Catch ex As Exception
            MessageBox.Show(ex.Message)
            SplashScreenManager1.CloseWaitForm()
        End Try
        FrmLogin.LodeSEcreen()
    End Sub
    Private Sub ToggleSwitch1_Properties_EditValueChanging(sender As Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles SelectAll.Properties.EditValueChanging


        GVEROELTyeAdmein(Man)
        GVEROELTyeAdmein(SRF)
        GVEROELTyeAdmein(GridView13)
        GVEROELTyeAdmein(Crun)
        GVEROELTyeAdmein(SNM)
        GVEROELTyeAdmein(Rs)
        GVEROELTyeAdmein(EM)
        GVEROELTyeAdmein(BAnk)
        GVEROELTyeAdmein(GridView9)
        GVEROELTyeAdmein(GM)
        GVEROELTyeAdmein(ST)
        GVEROELTyeAdmein(MO)
        GVEROELTyeAdmein(GridView4)

    End Sub


    Public Sub GVEROELTyeAdmein(GVRoles As GridView)
        If GVRoles.RowCount > 0 Then
            For i As Integer = 0 To GVRoles.RowCount
                If SelectAll.IsOn = False Then

                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanSave"), True)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanShow"), True)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanEdit"), True)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanDelete"), True)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanSearch"), True)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanPrint"), True)
                Else

                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanSave"), False)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanShow"), False)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanEdit"), False)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanDelete"), False)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanSearch"), False)
                    GVRoles.SetRowCellValue(i, GVRoles.Columns("CanPrint"), False)

                End If
            Next
        End If
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnnew.ItemClick
        Try


            ProfID.Text = String.Empty
            BarButtonItem3.Enabled = False
            BarButtonItem5.Enabled = False
            BarButtonItem2.Enabled = True
            BarButtonItem1.Enabled = False
            FrmMainTbgetdvg()
            FILLGRids()
            ProfileNamegetmax()
            LodeGroupGrid()
            TextEdit1.Text = String.Empty
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub FrmUserAccessTemplate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btnnew.PerformClick()
        lodePreportes()
    End Sub

    Private Sub GridControl1_Click(sender As Object, e As EventArgs) Handles GridControl1.Click
        Try


            If GridView1.RowCount > 1 Then
                XtraTabControl1.SelectedTabPageIndex = GridView1.GetFocusedRowCellValue("MainID") - 1
                TabGRoup_Man.SelectedTabPageIndex = GridView1.GetFocusedRowCellValue("MainID") - 1





            End If
        Catch ex As Exception

        End Try
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


        If Man.RowCount > 0 Then
            For i = 0 To Man.RowCount - 1
                dt.Rows.Add(Man.GetRowCellValue(i, "MainScreenID"), Man.GetRowCellValue(i, "GRoup_ID"), Man.GetRowCellValue(i, "ScreenID"),
                Man.GetRowCellValue(i, "CanShow"), Man.GetRowCellValue(i, "CanSave"), Man.GetRowCellValue(i, "CanEdit"), Man.GetRowCellValue(i, "CanDelete"),
                Man.GetRowCellValue(i, "CanSearch"), Man.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If SRF.RowCount > 0 Then
            For i = 0 To SRF.RowCount - 1
                dt.Rows.Add(SRF.GetRowCellValue(i, "MainScreenID"), SRF.GetRowCellValue(i, "GRoup_ID"), SRF.GetRowCellValue(i, "ScreenID"),
                SRF.GetRowCellValue(i, "CanShow"), SRF.GetRowCellValue(i, "CanSave"), SRF.GetRowCellValue(i, "CanEdit"), SRF.GetRowCellValue(i, "CanDelete"),
                SRF.GetRowCellValue(i, "CanSearch"), SRF.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If Crun.RowCount > 0 Then
            For i = 0 To Crun.RowCount - 1
                dt.Rows.Add(Crun.GetRowCellValue(i, "MainScreenID"), Crun.GetRowCellValue(i, "GRoup_ID"), Crun.GetRowCellValue(i, "ScreenID"),
                Crun.GetRowCellValue(i, "CanShow"), Crun.GetRowCellValue(i, "CanSave"), Crun.GetRowCellValue(i, "CanEdit"), Crun.GetRowCellValue(i, "CanDelete"),
                Crun.GetRowCellValue(i, "CanSearch"), Crun.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If SNM.RowCount > 0 Then
            For i = 0 To SNM.RowCount - 1
                dt.Rows.Add(SNM.GetRowCellValue(i, "MainScreenID"), SNM.GetRowCellValue(i, "GRoup_ID"), SNM.GetRowCellValue(i, "ScreenID"),
                SNM.GetRowCellValue(i, "CanShow"), SNM.GetRowCellValue(i, "CanSave"), SNM.GetRowCellValue(i, "CanEdit"), SNM.GetRowCellValue(i, "CanDelete"),
                SNM.GetRowCellValue(i, "CanSearch"), SNM.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If Rs.RowCount > 0 Then
            For i = 0 To Rs.RowCount - 1
                dt.Rows.Add(Rs.GetRowCellValue(i, "MainScreenID"), Rs.GetRowCellValue(i, "GRoup_ID"), Rs.GetRowCellValue(i, "ScreenID"),
                Rs.GetRowCellValue(i, "CanShow"), Rs.GetRowCellValue(i, "CanSave"), Rs.GetRowCellValue(i, "CanEdit"), Rs.GetRowCellValue(i, "CanDelete"),
                Rs.GetRowCellValue(i, "CanSearch"), Rs.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If EM.RowCount > 0 Then
            For i = 0 To EM.RowCount - 1
                dt.Rows.Add(EM.GetRowCellValue(i, "MainScreenID"), EM.GetRowCellValue(i, "GRoup_ID"), EM.GetRowCellValue(i, "ScreenID"),
                EM.GetRowCellValue(i, "CanShow"), EM.GetRowCellValue(i, "CanSave"), EM.GetRowCellValue(i, "CanEdit"), EM.GetRowCellValue(i, "CanDelete"),
                EM.GetRowCellValue(i, "CanSearch"), EM.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If BAnk.RowCount > 0 Then
            For i = 0 To BAnk.RowCount - 1
                dt.Rows.Add(BAnk.GetRowCellValue(i, "MainScreenID"), BAnk.GetRowCellValue(i, "GRoup_ID"), BAnk.GetRowCellValue(i, "ScreenID"),
                BAnk.GetRowCellValue(i, "CanShow"), BAnk.GetRowCellValue(i, "CanSave"), BAnk.GetRowCellValue(i, "CanEdit"), BAnk.GetRowCellValue(i, "CanDelete"),
                BAnk.GetRowCellValue(i, "CanSearch"), BAnk.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GridView9.RowCount > 0 Then
            For i = 0 To GridView9.RowCount - 1
                dt.Rows.Add(GridView9.GetRowCellValue(i, "MainScreenID"), GridView9.GetRowCellValue(i, "GRoup_ID"), GridView9.GetRowCellValue(i, "ScreenID"),
                GridView9.GetRowCellValue(i, "CanShow"), GridView9.GetRowCellValue(i, "CanSave"), GridView9.GetRowCellValue(i, "CanEdit"), GridView9.GetRowCellValue(i, "CanDelete"),
                GridView9.GetRowCellValue(i, "CanSearch"), GridView9.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If GM.RowCount > 0 Then
            For i = 0 To GM.RowCount - 1
                dt.Rows.Add(GM.GetRowCellValue(i, "MainScreenID"), GM.GetRowCellValue(i, "GRoup_ID"), GM.GetRowCellValue(i, "ScreenID"),
                GM.GetRowCellValue(i, "CanShow"), GM.GetRowCellValue(i, "CanSave"), GM.GetRowCellValue(i, "CanEdit"), GM.GetRowCellValue(i, "CanDelete"),
                GM.GetRowCellValue(i, "CanSearch"), GM.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If ST.RowCount > 0 Then
            For i = 0 To ST.RowCount - 1
                dt.Rows.Add(ST.GetRowCellValue(i, "MainScreenID"), ST.GetRowCellValue(i, "GRoup_ID"), ST.GetRowCellValue(i, "ScreenID"),
                ST.GetRowCellValue(i, "CanShow"), ST.GetRowCellValue(i, "CanSave"), ST.GetRowCellValue(i, "CanEdit"), ST.GetRowCellValue(i, "CanDelete"),
                ST.GetRowCellValue(i, "CanSearch"), ST.GetRowCellValue(i, "CanPrint"))
            Next
        End If

        If MO.RowCount > 0 Then
            For i = 0 To MO.RowCount - 1
                dt.Rows.Add(MO.GetRowCellValue(i, "MainScreenID"), MO.GetRowCellValue(i, "GRoup_ID"), MO.GetRowCellValue(i, "ScreenID"),
                MO.GetRowCellValue(i, "CanShow"), MO.GetRowCellValue(i, "CanSave"), MO.GetRowCellValue(i, "CanEdit"), MO.GetRowCellValue(i, "CanDelete"),
                MO.GetRowCellValue(i, "CanSearch"), MO.GetRowCellValue(i, "CanPrint"))
            Next
        End If




        Return dt
    End Function


    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Try


            If ProfID.Text = String.Empty Then

                ProfID.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            If TextEdit1.Text = String.Empty Then

                TextEdit1.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            If UserROOLMAINPROFILETP_INSERTR.Rows.Count <= 0 Then
                MessageBox.Show("الرجاء الضغط علي زر جديد", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            insertprofilename(0)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BarButtonItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        FRMshowPRofils.ShowDialog()
    End Sub

    Public Sub UserAccessProfileTemplate_sharch(ProfileIDs As Integer, Main As Integer, GCRoles As GridControl, gridviwe As GridView)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfileIDs}
        prm(1) = New SqlParameter("@MainID", SqlDbType.Int) With {.Value = Main}
        GCRoles.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        GCRoles.DataSource = Nothing

        dt = RUN_QUARY_PRO("UserAccessProfileTemplate_sharch", prm)

        If dt.Rows.Count > 0 Then
            GCRoles.DataSource = dt
            DVGFROMAT2(gridviwe)
        End If
    End Sub

    Public Sub UserROOLMAINPROFILETP_sharch(ProfileIDs As Integer)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@profid", SqlDbType.Int) With {.Value = ProfileIDs}
        GridControl1.DataSource = Nothing

        Dim dt As New DataTable
        dt.Clear()

        dt = RUN_QUARY_PRO("UserROOLMAINPROFILETP_sharch", prm)
        If dt.Rows.Count > 0 Then
            GridControl1.DataSource = dt
            DVGFROMAT2(GridView1)
        End If
    End Sub
    Public Sub Ueser_Group_main_shrch(ProfileIDs As Integer, Main As Integer, GCRoles As GridControl, gridviwe As GridView)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@Profile", SqlDbType.Int) With {.Value = ProfileIDs}
        prm(1) = New SqlParameter("@ManID", SqlDbType.Int) With {.Value = Main}
        GCRoles.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        GCRoles.DataSource = Nothing
        dt = RUN_QUARY_PRO("Ueser_Group_main_shrch", prm)
        If dt.Rows.Count > 0 Then
            GCRoles.DataSource = dt
            DVGFROMAT2(gridviwe)
        End If
    End Sub

    Public Sub Ueser_Group_main_shrch_lode(ProfileIDs As Integer)
        Ueser_Group_main_shrch(ProfileIDs, 1, GridControl11, GridView12)
        Ueser_Group_main_shrch(ProfileIDs, 2, GridControl12, GridView13)
        Ueser_Group_main_shrch(ProfileIDs, 3, GridControl13, GridView14)
        Ueser_Group_main_shrch(ProfileIDs, 4, GridControl14, GridView15)
        Ueser_Group_main_shrch(ProfileIDs, 5, GridControl15, GridView16)
        Ueser_Group_main_shrch(ProfileIDs, 6, GridControl16, GridView17)
        Ueser_Group_main_shrch(ProfileIDs, 7, GridControl17, GridView18)
        Ueser_Group_main_shrch(ProfileIDs, 8, GridControl18, GridView19)
        Ueser_Group_main_shrch(ProfileIDs, 9, GridControl19, GridView20)
        Ueser_Group_main_shrch(ProfileIDs, 10, GridControl20, GridView21)
        Ueser_Group_main_shrch(ProfileIDs, 11, GridControl21, GridView22)
        Ueser_Group_main_shrch(ProfileIDs, 12, GridControl23, GridView2)

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Try



            If ProfID.Text = String.Empty Then

                ProfID.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            If TextEdit1.Text = String.Empty Then

                TextEdit1.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            If UserROOLMAINPROFILETP_INSERTR.Rows.Count <= 0 Then
                MessageBox.Show("الرجاء الضغط علي زر جديد", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            TextEdit1.Select()
            insertprofilename(1)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BarButtonItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick

        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ProfID", ProfID.EditValue)
        Dim dt As DataTable = RUN_QUARY_PRO("RptGroup_print", PRM)
        If dt.Rows.Count > 0 Then
                    Dim report As New RptGroup_print
                    dt.TableName = "RptGroup_print"
                    Dim ds As New DataSet
                    ds.Tables.Add(dt)
                    report.DataSource = ds
                    report.DataMember = "RptGroup_print"
                    Dim tool As ReportPrintTool = New ReportPrintTool(report)
                    report.CreateDocument()
                    report.ShowPreview()

                    '' Specify the pages to export.
                    'report.ExportToDocx(docxExportFile, DocxExportOptions)


                    'SINTWATSAPP_document(id, docxExportFile, "كشف حركة الامانات المسلمة للزبائن بفرع", GetBranchName, "كشف مخزون الامانات المسلمة للزبائن بتاريخ اليوم" + GetBranchName & ".docx")
                Else


                    Dim lookAndFeelError As New UserLookAndFeel(Me)
                    lookAndFeelError.Style = LookAndFeelStyle.Skin
                    lookAndFeelError.UseDefaultLookAndFeel = False
                    lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
                    ' force Message Boxes to use the "MyCustomSkin"
                    XtraMessageBox.AllowCustomLookAndFeel = True

                    ' pass the UserLookAndFeel as a Parameter in the show method
                    XtraMessageBox.Show(lookAndFeelError, "عذرا لايوجد بيانات في الوقت الحالي الرجاء المحاولة في وقت لاحق", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)





                End If

    End Sub

    Private Sub BarButtonItem1_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Try

            If ProfID.Text = String.Empty AndAlso TextEdit1.Text Then
                ProfID.ErrorText = "الرجاء هذا الحقل مطلوب"
                Exit Sub
            End If
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            ' pass the UserLookAndFeel as a Parameter in the show method
            If XtraMessageBox.Show(lookAndFeelError, "هل تريد حذف هذه المجموعة ", "رسالة خطأ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub

            SplashScreenManager1.ShowWaitForm()


            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@ProfID", SqlDbType.Int) With {.Value = 1}
            prm(1) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(2) = New SqlParameter("@msgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("ProfileName_delete_from", prm)
            If prm(1).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                ErrorMessage(Me, "رسالة خطــــــــــــــــــــــــــأ", prm(2).Value)
            Else
                SplashScreenManager1.CloseWaitForm()
                FrmRemoveMessage.Show()
                btnnew.PerformClick()
            End If

        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage(Me, "رسالة خطــــــــــــــــــــــــــأ", ex.Message)
        End Try
    End Sub
End Class