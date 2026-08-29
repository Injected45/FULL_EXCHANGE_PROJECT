Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout

Public Class FRm_add_setting_uesers

    Public Sub loDEinTRP(UESERid As Integer, ProfIDsd As Integer)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ProfileID", SqlDbType.Int) With {.Value = ProfIDsd}
        UeserIDsaf.Properties.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("Tb_ueser_lodeUeserForProfile", prm)

        If dt.Rows.Count > 0 Then
            UeserIDsaf.Properties.DataSource = dt
            UeserIDsaf.Properties.DisplayMember = "UName"
            UeserIDsaf.Properties.ValueMember = "USID"
            UeserIDsaf.EditValue = UESERid
            UeserIDsaf.Enabled = False

        End If

    End Sub
    Public Sub PcForUeserActivtion_select(UEserInser As Integer)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@userIDa", SqlDbType.Int) With {.Value = UEserInser}
        GridControl2.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("PcForUeserActivtion_select", prm)
        If dt.Rows.Count > 0 Then
            GridControl2.DataSource = dt
        End If
    End Sub

    Public Sub Ueser_Group_main_ID_getUeser(UEserInser As Integer)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UEserInser}
        GridControl1.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("Ueser_Group_main_ID_getUeser", prm)
        If dt.Rows.Count > 0 Then
            GridControl1.DataSource = dt
            DVGFROMA2(GridView12)

        End If
    End Sub

    Public Function LOAD_Prof() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("ProfID_LoadDataIntoLookUpEdit")
        Return DT
    End Function

    Public Sub ActivivationTb_selectBranchID(brchID As Integer)
        SplashScreenManager1.ShowWaitForm()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@brnchID", SqlDbType.Int) With {.Value = brchID}
        Dim dt As New DataTable
        dt.Clear()



        PcName.Properties.DataSource = Nothing

        dt.NewRow()
        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("MaccAddressID", GetType(String))

        dt = RUN_QUARY_PRO("ActivivationTb_selectBranchID", prm)

        dt.Rows.Add(0, "كل الاجهزة")
        If dt.Rows.Count > 0 Then
            PcName.Properties.DataSource = dt
            PcName.Properties.DisplayMember = "MaccAddressID"
            PcName.Properties.ValueMember = "ID"

        End If
        SplashScreenManager1.CloseWaitForm()

    End Sub
    Private Sub TextEdit2_EditValueChanged(sender As Object, e As EventArgs) Handles brnchIDform.EditValueChanged
        If brnchIDform.EditValue > -1 Then
            ActivivationTb_selectBranchID(brnchIDform.EditValue)
            If brnchIDform.EditValue = 0 Then
                PcName.EditValue = 0
                PcName.Enabled = False
            Else
                PcName.Enabled = True
            End If
        End If
    End Sub

    Sub LOADProfID(ProfIDs As Integer)
        Dim DT As New DataTable
        DT.Clear()
        DT = LOAD_Prof()

        If DT.Rows.Count > 0 Then
            ProfID.Properties.DataSource = DT
            ProfID.Properties.DisplayMember = "ProfileName"
            ProfID.Properties.ValueMember = "ProfID"
            ProfID.EditValue = ProfIDs
            ProfID.Enabled = False
        End If
    End Sub
    Public Sub UserAccessMainScreen_forUEserID_BYCODE(uesrInsert As Integer, GridControl1 As GridControl)
        Try
            GridControl1.DataSource = Nothing
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@SAFID", SqlDbType.BigInt) With {.Value = uesrInsert}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("UserAccessMainScreen_forUEserID_BYCODE", prm)
            If dt.Rows.Count > 0 Then
                GridControl1.DataSource = dt
            End If
            dt.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالــــــــــــــــــة تنبيــــة ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


    Public Sub MeShoweForSEtiing(uesrInsert As Integer, ProfIDd As Integer)
        Try
            loDEinTRP(uesrInsert, ProfIDd)
            PcForUeserActivtion_select(uesrInsert)
            LOADProfID(ProfIDd)
            UserAccessMainScreen_forUEserID_BYCODE(uesrInsert, GridControl11)
            FrmScreensTb_Details_UESIRID_get(UeserIDsaf.EditValue, 1, GETNEMA_FRom)
            FrmScreensTb_Details_UESIRID_get(UeserIDsaf.EditValue, 2, GridControl4)
            FrmScreensTb_Details_UESIRID_get(UeserIDsaf.EditValue, 3, GridControl8)
            Ueser_Group_main_ID_getUeser(uesrInsert)
            NotificationUSERID_Select(uesrInsert)
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message, "رسالــــــــــــــــــة تنبيــــة ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub


    Public Sub NotificationUSERID_Select(ueserID As ULong)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@userid", SqlDbType.Int) With {.Value = ueserID}
        LoadToControlar(GridControl7, "NotificationUSERID_Select", "", "", prm)
    End Sub
    Sub DVGFROMA2(GridView As GridView)

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

    Sub LOADBRANCH(BranchIDd As GridLookUpEdit)
        Dim DT As New DataTable
        DT.Clear()
        BranchIDd.Properties.DataSource = Nothing
        DT.Clear()
        DT.NewRow()
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("BName", GetType(String))


        DT = RUN_QUARY_TXT("COBRANCHTB_LoadFORSAFES")
        DT.Rows.Add(0, "كل الفروع")
        If DT.Rows.Count > 0 Then
            BranchIDd.Properties.DataSource = DT
            BranchIDd.Properties.ValueMember = "ID"
            BranchIDd.Properties.DisplayMember = "BName"


        End If
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


    Public Sub newrecores()
        SplashScreenManager1.ShowWaitForm()
        DVGFROMA2(GridView2)
        LOADBRANCH(brnchIDform)
        DVGFROMAT2(GridView4)
        DVGFROMAT2(GridView3)
        DVGFROMA2(GridView6)
        DVGFROMA2(GridView16)
        DVGFROMA2(GridView17)

        XtraTabControl1.SelectedTabPageIndex = 0
        brnchIDform.EditValue = -1
        PcName.EditValue = -1

        '' تعطيل الكود لحين اتنتهاء من نظام الصرافة معرفة اسماء الحقول مطلوب اخفاءها
        '' FrmScreensTb_selectITimsFormMovent_UeserID_fill_dvg()
        TabbedControlGroup1.SelectedTabPageIndex = 0
        SplashScreenManager1.CloseWaitForm()
    End Sub

    Private Sub FRm_add_setting_uesers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        newrecores()
    End Sub
    Public Sub PcForUeserActivtion_insert_forUpdate()
        Try


            Dim prm(4) As SqlParameter
            prm(0) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UeserIDsaf.EditValue}
            prm(1) = New SqlParameter("@pcName", SqlDbType.Int) With {.Value = PcName.EditValue}
            prm(2) = New SqlParameter("@brnchID", SqlDbType.Int) With {.Value = brnchIDform.EditValue}
            prm(3) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(4) = New SqlParameter("@msgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("PcForUeserActivtion_insert_forUpdate", prm)
            If prm(3).Value = 1 Then
                FrmSavedSuccessfully.Show()
                PcForUeserActivtion_select(UeserIDsaf.EditValue)
                PcName.EditValue = -1
                brnchIDform.EditValue = -1

            ElseIf prm(3).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                MessageBox.Show(prm(4).Value, "رســـــــــــالة خطـــــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message, "رسالــــــــــــة تنبيــــــــــــة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

        If brnchIDform.EditValue = -1 Then
            brnchIDform.ErrorText = "هذا الحقل مطلوب"
            Return
        End If

        If PcName.EditValue = -1 Then
            PcName.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        SplashScreenManager1.ShowWaitForm()
        PcForUeserActivtion_insert_forUpdate()
        SplashScreenManager1.CloseWaitForm()
    End Sub
    Private Sub RepositoryItemButtonEdit1_Click(sender As Object, e As EventArgs) Handles RepositoryItemButtonEdit1.Click
        Try
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            ' pass the UserLookAndFeel as a Parameter in the show method



            If XtraMessageBox.Show(lookAndFeelError, "هل تريد حذف هذه الشاشة من هذا المستخدم", "رسالة خطأ", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = DialogResult.Yes Then
                Dim prm(0) As SqlParameter
                prm(0) = New SqlParameter("@id", SqlDbType.Int) With {.Value = GridView3.GetFocusedRowCellValue("id")}
                RUN_EXUTE_PRO("PcForUeserActivtion_delete", prm)
                FrmRemoveMessage.Show()
                PcForUeserActivtion_select(UeserIDsaf.EditValue)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالــــــــــــة تنبيــــــــــــة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub GridView3_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView3.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView3.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        GridView9.ActiveFilterString = Nothing
        GridView12.ActiveFilterString = Nothing
        GridView12.ClearFindFilter()
        GridView7.ActiveFilterString = Nothing
        GridView7.ClearFindFilter()
        GridView2.ActiveFilterString = Nothing
        GridView2.ClearFindFilter()
        GridView9.ClearFindFilter()
        GridView17.ActiveFilterString = Nothing
        GridView17.ClearFindFilter()
        GridView16.ClearFindFilter()
        GridView16.ActiveFilterString = Nothing
        If GridView12.ActiveFilterString <> Nothing OrElse GridView12.FindFilterText <> Nothing Then
            ErrorMessage2("رسالة تنبية", "الرجاء اغلاق الفلترة")
            XtraTabControl1.SelectedTabPageIndex = 2
            GridView12.ActiveFilterString = Nothing
            GridView12.ClearFindFilter()
            Exit Sub
        End If
        If GridView7.ActiveFilterString <> Nothing OrElse GridView7.FindFilterText <> Nothing Then
            ErrorMessage2("رسالة تنبية", "الرجاء اغلاق الفلترة")
            XtraTabControl1.SelectedTabPageIndex = 1
            GridView7.ActiveFilterString = Nothing
            GridView7.ClearFindFilter()
            Exit Sub
        End If

        If GridView2.ActiveFilterString <> Nothing OrElse GridView2.FindFilterText <> Nothing Then
            ErrorMessage2("رسالة تنبية", "الرجاء اغلاق الفلترة")
            TabbedControlGroup1.SelectedTabPageIndex = 1
            GridView2.ActiveFilterString = Nothing
            GridView2.ClearFindFilter()
            Exit Sub
        End If



        If GridView17.ActiveFilterString <> Nothing OrElse GridView17.FindFilterText <> Nothing Then
            ErrorMessage2("رسالة تنبية", "الرجاء اغلاق الفلترة")
            XtraTabControl1.SelectedTabPageIndex = 5
            GridView17.ActiveFilterString = Nothing
            GridView17.ClearFindFilter()
            Exit Sub
        End If

        If GridView16.ActiveFilterString <> Nothing OrElse GridView16.FindFilterText <> Nothing Then
            ErrorMessage2("رسالة تنبية", "الرجاء اغلاق الفلترة")
            XtraTabControl1.SelectedTabPageIndex = 3
            GridView16.ActiveFilterString = Nothing
            GridView16.ClearFindFilter()
            Exit Sub
        End If



        GridView12.ActiveFilterString = Nothing
        GridView12.ClearFindFilter()
        GridView7.ActiveFilterString = Nothing
        GridView7.ClearFindFilter()
        GridView2.ActiveFilterString = Nothing
        GridView2.ClearFindFilter()


        UserAccessProfileTemplate_ID_inser_for_update()


    End Sub



    Public Sub UserAccessProfileTemplate_ID_inser_for_update()
        Try


            If UserROOLMAINPROFILETP_INSERTR.Rows.Count = 0 Then
                ErrorMessage(Me, "رسالة تنبية", "عذرا لايوجد بيانات في الوقت الحالي 1")
                Exit Sub

            End If

            If Ueser_Group_main_insertType.Rows.Count = 0 Then
                ErrorMessage(Me, "رسالة تنبية", "عذرا لايوجد بيانات في الوقت الحالي2 ")
                Exit Sub

            End If


            If FrmScreensTb_Details_UESIRID_type.Rows.Count = 0 Then
                ErrorMessage(Me, "رسالة تنبية", "عذرا لايوجد بيانات في الوقت الحالي3 ")
                Exit Sub

            End If



            'If NotificationUSERID_Type.Rows.Count = 0 Then
            '    ErrorMessage(Me, "رسالة تنبية", "عذرا لايوجد بيانات في الوقت الحالي4 ")
            '    Exit Sub

            'End If

            If UeserIDsaf.EditValue = -1 Then
                UeserIDsaf.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If

            If ProfID.EditValue = -1 Then
                ProfID.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If

            If FrmScreensTb_Details_UESIRID_can_banck.Rows.Count = 0 Then
                ErrorMessage(Me, "رسالة تنبية", "5عذرا لايوجد بيانات في الوقت الحالي ")
                Exit Sub

            End If


            SplashScreenManager1.ShowWaitForm()
            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("@UserROOLMAINPROFILETP_ID_insert_forupdate", SqlDbType.Structured) With {.Value = UserROOLMAINPROFILETP_INSERTR()}
            prm(1) = New SqlParameter("@Ueser_Group_main_ID_ype", SqlDbType.Structured) With {.Value = Ueser_Group_main_insertType()}
            prm(2) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UeserIDsaf.EditValue}
            prm(3) = New SqlParameter("@profid", SqlDbType.Int) With {.Value = ProfID.EditValue}
            prm(4) = New SqlParameter("@FrmScreensTb_Details_UESIRID", SqlDbType.Structured) With {.Value = FrmScreensTb_Details_UESIRID_type()}
            prm(5) = New SqlParameter("@FrmScreensTb_Details_ForUpdate", SqlDbType.Structured) With {.Value = FrmScreensTb_Details_ForUpdate()}
            prm(6) = New SqlParameter("@NotificationUSERID_Type", SqlDbType.Structured) With {.Value = NotificationUSERID_Type()}
            prm(7) = New SqlParameter("@FrmScreensTb_Details_UESIRID_can_banck", SqlDbType.Structured) With {.Value = FrmScreensTb_Details_UESIRID_can_banck()}


            RUN_EXUTE_PRO("[UserAccessProfileTemplate_ID_inser_for_update]", prm)
            UserAccessMainScreen_forUEserID_BYCODE(UeserIDsaf.EditValue, GridControl11)

            Ueser_Group_main_ID_getUeser(UeserIDsaf.EditValue)

            SplashScreenManager1.CloseWaitForm()
            FrmSavedSuccessfully.Show()
            XtraTabControl2.SelectedTabPageIndex = 0
            FrmScreensTb_Details_UESIRID_get(UeserIDsaf.EditValue, XtraTabPage3.Tag, GridControl3)
            NotificationUSERID_Select(UeserIDsaf.EditValue)
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage(Me, "رسالة تنبية", ex.Message)
        End Try
        FrmLogin.LodeSEcreen()
    End Sub
    Public Function UserROOLMAINPROFILETP_INSERTR() As DataTable
        Dim dt As New DataTable
        Try
            ' إعداد أعمدة الجدول
            dt.Columns.Add("canshow")
            dt.Columns.Add("mainid")

            ' تعبئة البيانات من GridView الرئيسي
            For i As Integer = 0 To GridView2.RowCount - 1
                dt.Rows.Add(GridView2.GetRowCellValue(i, "CanShow"), GridView2.GetRowCellValue(i, "MainID"))
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
                dt.Rows.Add(GridView12.GetRowCellValue(i, "id"), GridView12.GetRowCellValue(i, "Canshow"), GridView12.GetRowCellValue(i, "Main_ID"))
            Next

        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الفرعي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    Public Function FrmScreensTb_Details_UESIRID_type() As DataTable
        Dim dt As New DataTable
        Try
            dt.Clear()
            ' إعداد أعمدة الجدول
            dt.Columns.Add("ScreenID")
            dt.Columns.Add("Can_branch")
            dt.Columns.Add("Can_safID")
            dt.Columns.Add("Can_Close_safid")
            dt.Columns.Add("Can_Accfrom")
            dt.Columns.Add("Can_accTo")
            dt.Columns.Add("Can_ISbacnk")

            For i As Integer = 0 To GridView7.RowCount - 1
                dt.Rows.Add(GridView7.GetRowCellValue(i, "ScreenID"), GridView7.GetRowCellValue(i, "Can_branch"),
                            GridView7.GetRowCellValue(i, "Can_safID"), GridView7.GetRowCellValue(i, "Can_Close_safid"),
                         GridView7.GetRowCellValue(i, "Can_Accfrom"), GridView7.GetRowCellValue(i, "Can_accTo"),
                     GridView7.GetRowCellValue(i, "Can_ISbacnk"))

            Next

        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الفرعي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function




    Public Function NotificationUSERID_Type() As DataTable
        Dim dt As New DataTable
        Try
            dt.Clear()
            ' إعداد أعمدة الجدول
            dt.Columns.Add("NotificationID")
            dt.Columns.Add("canshow")
            For i As Integer = 0 To GridView16.RowCount - 1
                dt.Rows.Add(GridView16.GetRowCellValue(i, "NotificationID"), GridView16.GetRowCellValue(i, "canshow")
                          )

            Next

        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الفرعي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function





    Public Function FrmScreensTb_Details_ForUpdate() As DataTable
        Dim dt As New DataTable
        Try
            dt.Clear()
            ' إعداد أعمدة الجدول
            dt.Columns.Add("ScreenID")
            dt.Columns.Add("UeserID")
            dt.Columns.Add("CaN_calCylaTion")
            dt.Columns.Add("Can_Close_safid")
            For i As Integer = 0 To GridView9.RowCount - 1
                dt.Rows.Add(GridView9.GetRowCellValue(i, "ScreenID"), UeserIDsaf.EditValue, GridView9.GetRowCellValue(i, "CaN_calCylaTion"),
                            GridView9.GetRowCellValue(i, "Can_Close_safid"))
            Next

        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الفرعي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function



    Public Function FrmScreensTb_Details_UESIRID_can_banck() As DataTable
        Dim dt As New DataTable
        Try
            dt.Clear()
            ' إعداد أعمدة الجدول
            dt.Columns.Add("can_banck")
            dt.Columns.Add("can_Acount")
            dt.Columns.Add("can_cash")
            dt.Columns.Add("ScreenID")
            dt.Columns.Add("UeserID")
            For i As Integer = 0 To GridView17.RowCount - 1
                dt.Rows.Add(GridView17.GetRowCellValue(i, "can_banck"), GridView17.GetRowCellValue(i, "can_Acount"), GridView17.GetRowCellValue(i, "can_cash"),
                            GridView17.GetRowCellValue(i, "ScreenID"), UeserIDsaf.EditValue
)
            Next

        Catch ex As Exception
            MessageBox.Show($"خطأ أثناء استرجاع بيانات الجدول الفرعي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function



    Public Sub FrmScreensTb_Details_UESIRID_get(UeserID As Integer, ColmenValue As Integer, Grivecontroles As GridControl)
        Try


            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@UeserID", SqlDbType.Int) With {.Value = UeserID}
            prm(1) = New SqlParameter("@ColmenValue", SqlDbType.Int) With {.Value = ColmenValue}
            Dim dt As New DataTable
            Grivecontroles.DataSource = Nothing
            dt.Clear()
            dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_get", prm)
            If dt.Rows.Count > 0 Then
                Grivecontroles.DataSource = dt
                DVGFORMAT(GridView7)
            End If
        Catch ex As Exception
            ErrorMessage2("رسالة تنبية", ex.Message)
        End Try

    End Sub

    Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        XtraTabControl2.SelectedTabPageIndex = 0
        FrmScreensTb_Details_UESIRID_get(UeserIDsaf.EditValue, XtraTabPage3.Tag, GridControl3)
    End Sub
    Private Sub GridView9_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView9.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView7.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Private Sub GridView7_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView7.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView7.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub


    Private Sub GridView17_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView17.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView7.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
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

    Private Sub XtraTabControl2_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl2.SelectedPageChanged
        Try


            FrmScreensTb_Details_UESIRID_get(UeserIDsaf.EditValue, XtraTabControl2.SelectedTabPage.Tag, GETNEMA_FRom)
            DVGFORMAT(GETNEMA_FRom_GridView())
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''جلب اسم القريد كنترول علي حسب اسمه من  تاب  
    Public Function GETNEMA_FRom() As GridControl
        ' التحقق إذا كان هناك أي تاب صفحات داخل XtraTabControl
        If XtraTabControl2.TabPages.Count > 0 Then

            ' المرور عبر جميع التاب صفحات
            For Each tabPage As DevExpress.XtraTab.XtraTabPage In XtraTabControl2.TabPages

                ' التحقق إذا كان التاب المفتوح هو نفسه التاب الحالي
                If tabPage.TabIndex = XtraTabControl2.SelectedTabPageIndex Then

                    ' المرور عبر جميع عناصر التحكم داخل التاب الحالي
                    For Each control As Control In tabPage.Controls

                        ' التحقق إذا كان العنصر هو GridControl
                        If TypeOf control Is GridControl Then
                            ' هنا يمكننا الحصول على اسم الأداة (اسم المتغير أو اسم الأداة كما تم تسميته في التصميم)
                            Dim gridControl As GridControl = DirectCast(control, DevExpress.XtraGrid.GridControl)
                            Return gridControl
                        End If

                    Next
                End If
            Next
        End If



    End Function
    ''جلب اسم القريد الفيو علي حسب اسمه من تاب كنترول 
    Public Function GETNEMA_FRom_GridView() As GridView
        ' التحقق إذا كان هناك أي تاب صفحات داخل XtraTabControl
        If XtraTabControl2.TabPages.Count > 0 Then

            ' المرور عبر جميع التاب صفحات
            For Each tabPage As DevExpress.XtraTab.XtraTabPage In XtraTabControl2.TabPages

                ' التحقق إذا كان التاب المفتوح هو نفسه التاب الحالي
                If tabPage.TabIndex = XtraTabControl2.SelectedTabPageIndex Then

                    ' المرور عبر جميع عناصر التحكم داخل التاب الحالي
                    For Each control As Control In tabPage.Controls

                        ' التحقق إذا كان العنصر هو GridControl
                        If TypeOf control Is GridControl Then
                            ' تحويل العنصر إلى GridControl
                            Dim gridControl As GridControl = DirectCast(control, GridControl)

                            ' الحصول على GridView المرتبط بـ GridControl
                            Dim gridView As GridView = DirectCast(gridControl.MainView, GridView)

                            ' إرجاع GridView
                            Return gridView
                        End If

                    Next
                End If
            Next
        End If

        ' في حالة عدم العثور على GridControl أو GridView
        Return Nothing
    End Function





End Class