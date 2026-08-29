Imports System.Data.SqlClient
Imports System.Management
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils.Animation
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraTab
Imports DevExpress.XtraTab.ViewInfo

Public Class FrmAddSecrein

    Public Sub FrmMainTb_getdDvg()
        MainScreenID.Properties.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_ONLY("FrmMainTb_getdDvg")
        If dt.Rows.Count > 0 Then
            If dt.Rows.Count > 0 Then
                MainScreenID.Properties.DataSource = dt
                MainScreenID.Properties.DisplayMember = "MainName"
                MainScreenID.Properties.ValueMember = "MainID"
                DVGFROMAT2(GridLookUpEdit1View)
            End If
            dt.Dispose()
        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()


        dt = SElectUEserFormButtn(107, UserID)



        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Public Sub Group_main_getdDvg(MAneID_Group As Integer)
        GroupNAme_ID.Properties.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@mainID", SqlDbType.Int) With {.Value = MAneID_Group}
        dt = RUN_QUARY_PRO("[Group-main_getdDvg]", prm)
        If dt.Rows.Count > 0 Then
            If dt.Rows.Count > 0 Then
                GroupNAme_ID.Properties.DataSource = dt
                GroupNAme_ID.Properties.DisplayMember = "GroupNAme"
                GroupNAme_ID.Properties.ValueMember = "id"
                DVGFROMAT2(GridLookUpEdit1View)
            End If
            dt.Dispose()
        End If
    End Sub

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

    Public Overrides Sub BNew()
        FrmMainTb_getdDvg()

        ScreenID.Text = String.Empty
        ScreenName.Text = String.Empty
        EnglishName.Text = String.Empty
        GroupNAme_ID.EditValue = -1
        MainScreenID.EditValue = -1
        ShortName.Text = String.Empty
        FrmScreensTb2_maxID()
        MyBase.BNew()
    End Sub
    Private Sub FrmAddSecrein_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnNew.PerformClick()
        lodePreportes()
    End Sub


    Public Sub FrmScreensTb2_maxID()
        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Direction = ParameterDirection.Output}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("FrmScreensTb2_maxID", prm)
            ScreenID.EditValue = prm(0).Value
        Catch ex As Exception
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            ' pass the UserLookAndFeel as a Parameter in the show method
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالـــــــــــة خطــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Sub FrmScreensTb2_insert()
        Try


            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("@ScreenName", SqlDbType.NVarChar, -1) With {.Value = ScreenName.Text}
            prm(1) = New SqlParameter("@MainScreenID", SqlDbType.Int) With {.Value = MainScreenID.EditValue}
            prm(2) = New SqlParameter("@EnglishName", SqlDbType.NVarChar, -1) With {.Value = EnglishName.Text}
            prm(3) = New SqlParameter("@ShortName", SqlDbType.NVarChar, 50) With {.Value = ShortName.Text}
            prm(4) = New SqlParameter("@smg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@msg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@GRoup_ID", SqlDbType.Int) With {.Value = GroupNAme_ID.EditValue}
            prm(7) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID.EditValue}
            RUN_EXUTE_PRO("FrmScreensTb2_insert", prm)

            If prm(4).Value = 0 Then
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                'lookAndFeelError.SkinName = "MilkShake"
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
                ' force Message Boxes to use the "MyCustomSkin"
                XtraMessageBox.AllowCustomLookAndFeel = True

                ' pass the UserLookAndFeel as a Parameter in the show method
                XtraMessageBox.Show(lookAndFeelError, prm(5).Value, "رسالـــــــــــة خطــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                BtnNew.PerformClick()
            End If

        Catch ex As Exception
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            ' pass the UserLookAndFeel as a Parameter in the show method
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالـــــــــــة خطــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub



    Public Overrides Sub SetData()


        If ScreenName.Text = String.Empty Then
            ScreenName.ErrorText = "هذا الحقل المطلوب"
            Return
        End If

        If MainScreenID.EditValue = -1 Then
            MainScreenID.ErrorText = "هذا الحقل المطلوب"
            Return
        End If

        If EnglishName.Text = String.Empty Then
            EnglishName.ErrorText = "هذا الحقل المطلوب"
            Return
        End If


        If ShortName.Text = String.Empty Then
            ShortName.ErrorText = "هذا الحقل المطلوب"
            Return
        End If
        If GroupNAme_ID.EditValue = -1 Then
            GroupNAme_ID.ErrorText = "هذه الحقل مطلوب"
            Exit Sub
        End If

        FrmScreensTb2_insert()

        MyBase.SetData()
    End Sub


    Public Overrides Sub Save()

        SetData()
        MyBase.Save()
    End Sub

    Private Sub MainScreenID_EditValueChanged(sender As Object, e As EventArgs) Handles MainScreenID.EditValueChanged

        If MainScreenID.EditValue > -1 Then

            Group_main_getdDvg(MainScreenID.EditValue)
        End If
    End Sub
End Class