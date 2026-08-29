Imports System.ComponentModel
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports MetroFramework

Public Class FrmCoBranch
    Dim clscb As New CLSCoBranch
    Public Property CBID As Integer
    Public Property BRNID As Integer
    Public IsUpdate, MsgStatus As Boolean
    Public AGG As Integer
    Public BRG As Integer, AccCode As ULong
    Public msgST As Int16





    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(2, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub LOADCOUNTRIES()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CountriesTb_LoadToLKP")
        If DT.Rows.Count > 0 Then
            CountryID.Properties.DataSource = DT
            CountryID.Properties.ValueMember = "ID"
            CountryID.Properties.DisplayMember = "CName"
            CountryID.Properties.ShowHeader = False
        End If
    End Sub
    Sub LOADCITIES()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        PR(0).Value = CountryID.EditValue
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CitiesTb_LoadToLKPBasedonCountry", PR)
        If DT.Rows.Count > 0 Then
            CityID.Properties.DataSource = DT
            CityID.Properties.ValueMember = "CTID"
            CityID.Properties.DisplayMember = "CityName"
            CityID.Properties.PopulateColumns()
            CityID.Properties.ShowHeader = False
            CityID.Properties.Columns("CTID").Visible = False
        End If
    End Sub
    Sub newRecord()
        LOADCOUNTRIES()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranch_CheckIsMainTrue")
        If DT.Rows(0)("IsMain") = True Then
            IsMain.Checked = False
            IsMain.Enabled = False
        End If
        Code.Text = GETMAXID("CoBranch", "ID") + 1
        BName.Enabled = True
        BranchType.SelectedIndex = 0
        Activation.IsOn = True
        BName.Text = String.Empty
        CountryID.EditValue = -1
        CityID.EditValue = -1
        BranchType.SelectedIndex = -1
        BAddress.Text = String.Empty
        Mobile1.Text = String.Empty
        Mobile2.Text = String.Empty
        Notes.Text = String.Empty
        BName.Select()
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        IsPartner.SelectedIndex = 0
        OwnerName.Text = ""
        IsUpdate = False
        GroupName.Text = String.Empty
        IDGroup.Text = String.Empty
    End Sub
    Public Overrides Sub SetData()
        'BranchType.SelectedIndex = -1
        If IsUpdate = 0 Then
            'BranchType.SelectedIndex = 0
            If BName.Text = String.Empty Then
                BName.ErrorText = "يرجى إدخال اسم الفرع"
                BName.Select()
                Exit Sub
            End If
            Dim dt As New DataTable
            dt = clscb.CHECK_BRANCH_NAME(BName.Text.Trim)
            If dt.Rows.Count > 0 Then
                BName.ErrorText = "هذا الاسم موجود مسبقا"
                BName.Select()
                Exit Sub
            End If
            If BranchType.SelectedIndex = -1 Then
                BranchType.ErrorText = "هذا الحقل مطلوب"
                Exit Sub
            End If
            If CountryID.EditValue = -1 Then
                CityID.ErrorText = "يجب اختيار المدينة"
                Exit Sub
            End If
            If CountryID.EditValue <> -1 Then
                If CityID.EditValue = -1 Then
                    CityID.ErrorText = "يجب اختيار المدينة"
                    Exit Sub
                End If
            End If
            Dim BRTYPE As Int16
            If BranchType.SelectedIndex = 0 Then
                BRTYPE = 1
            ElseIf BranchType.SelectedIndex = 1 Then
                BRTYPE = 2
            ElseIf BranchType.SelectedIndex = 2 Then
                BRTYPE = 3
            ElseIf BranchType.SelectedIndex = 3 Then
                BRTYPE = 4
            End If
            BRNID = GETMAXID("CoBranch", "ID") + 1
            clscb.INSERTTB_PROFILE_CoBranch(Code.Text.Trim, BName.Text.Trim, CountryID.EditValue, CityID.EditValue, BAddress.Text.Trim, Mobile1.Text.Trim, Mobile2.Text.Trim,
                                            Notes.Text.Trim, Activation.IsOn, IsMain.Checked, BRTYPE, IsPartner.SelectedIndex,
                                            IsUpdate, BRNID, OwnerName.Text.Trim, GroupName.Text, IDGroup.Text)
        End If

        If msgST = 1 Then
            MyBase.SetData()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        newRecord()
        MyBase.Save()
    End Sub
    Public Sub LoadPermissions()
        Try
            ' إنشاء كائن DataTable لجلب الصلاحيات
            Dim dt As New DataTable
            dt = SElectUEserFormButtn(2, UserID)

            ' التحقق من وجود بيانات في الجدول
            If dt.Rows.Count > 0 Then
                ' ضبط رؤية زر الحفظ بناءً على الصلاحيات
                BtnSave.Visibility = If(dt.Rows(0)("CanSave") = 0, DevExpress.XtraBars.BarItemVisibility.Never, DevExpress.XtraBars.BarItemVisibility.Always)

                ' ضبط رؤية زر التعديل بناءً على الصلاحيات
                BtnEdit.Visibility = If(dt.Rows(0)("CanEdit") = 0, DevExpress.XtraBars.BarItemVisibility.Never, DevExpress.XtraBars.BarItemVisibility.Always)

                ' ضبط رؤية زر الطباعة بناءً على الصلاحيات
                BtnPrint.Visibility = If(dt.Rows(0)("CanPrint") = 0, DevExpress.XtraBars.BarItemVisibility.Never, DevExpress.XtraBars.BarItemVisibility.Always)
            End If
        Catch ex As Exception
            ' معالجة الأخطاء عند فشل تحميل الصلاحيات
            MessageBox.Show("حدث خطأ أثناء تحميل الصلاحيات: " & ex.Message)
        End Try
    End Sub


    Public Overrides Sub BNew()
        newRecord()
        MyBase.BNew()
    End Sub
    Public Overrides Sub Remove()
        If IsUpdate = True Then
            Dim reslut = XtraMessageBox.Show("سيتم حذف البيانات المحددة، هل تريد الاستمرار؟", "رسالة تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If reslut = DialogResult.No Then
                Exit Sub
            End If
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@RID", Me.CBID)
            RUN_EXUTE_PRO("CoBranch_DeleteRoleById", PRM)
        End If
        InfoMessage(Me, "رسالة تأكيد", "تم حذف البيانات بنجاح")
        MyBase.Remove()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If BName.Text = String.Empty Then
                BName.ErrorText = "يرجى إدخال اسم الفرع"
                BName.Select()
                Exit Sub
            End If

            If BranchType.SelectedIndex = -1 Then
                BranchType.ErrorText = "هذا الحقل مطلوب"
            End If
            If CountryID.EditValue = -1 Then
                CityID.ErrorText = "يجب اختيار المدينة"
                Exit Sub
            End If
            If CountryID.EditValue <> -1 Then
                If CityID.EditValue = -1 Then
                    CityID.ErrorText = "يجب اختيار المدينة"
                    Exit Sub
                End If
            End If
            Dim BRTYPE As Integer
            If BranchType.SelectedIndex = 0 Then
                BRTYPE = 1
            ElseIf BranchType.SelectedIndex = 1 Then
                BRTYPE = 2
            ElseIf BranchType.SelectedIndex = 2 Then
                BRTYPE = 3
            ElseIf BranchType.SelectedIndex = 3 Then
                BRTYPE = 4
            End If
            clscb.INSERTTB_PROFILE_CoBranch(Code.Text.Trim, BName.Text.Trim, CountryID.EditValue, CityID.EditValue, BAddress.Text.Trim, Mobile1.Text.Trim, Mobile2.Text.Trim, Notes.Text.Trim, Activation.IsOn,
                                            IsMain.Checked, BRTYPE, IsPartner.SelectedIndex, IsUpdate, BRNID, OwnerName.Text.Trim, GroupName.Text, IDGroup.Text)

        End If
        If msgST = 1 Then
            MyBase.Update()
        End If
    End Sub
    Public Overrides Sub CHECKBUTTONS()
        MyBase.CHECKBUTTONS()
    End Sub
    Sub SHOWRECORD(x, f)
        'Try


        If Me.IsUpdate = True Then


            LOADCOUNTRIES()
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@ID", x)
            PRM(1) = New SqlParameter("@Code", f)
            Dim dt As DataTable = RUN_QUARY_PRO("CoBranch_ReloadDataForUpdate", PRM)
            Dim row As DataRow = dt.Rows(0)
                    Code.Text = row("Code").ToString
                    BName.Text = row("BName").ToString
                    CountryID.EditValue = row("CountryID")
                    CityID.EditValue = row("CityID")
                    BAddress.Text = row("BAddress").ToString
                    Mobile1.Text = row("Mobile1").ToString
                    Mobile2.Text = row("Mobile2").ToString
                    Notes.Text = row("Notes").ToString
                    Activation.IsOn = row("IsActive").ToString
                OwnerName.Text = row("OwnerName").ToString
                IDGroup.Text = row("IDGroup").ToString
                GroupName.Text = row("GroupName").ToString
                If row("IsMain") = True Then
                        IsMain.Enabled = True
                        IsMain.Checked = row("IsMain").ToString
                    Else
                        IsMain.Enabled = False
                        IsMain.Checked = False
                    End If
                If row("BranchType") = 1 Then
                    BranchType.SelectedIndex = 0
                ElseIf row("BranchType") = 2 Then
                    BranchType.SelectedIndex = 1
                ElseIf row("BranchType") = 3 Then
                    BranchType.SelectedIndex = 2
                End If


                BRNID = x
                BtnSave.Enabled = False
                    BtnEdit.Enabled = True
                    BtnDelete.Enabled = True
                    BName.Enabled = False
                    dt.Dispose()

            Else
                newRecord()
            End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End Try
    End Sub
    'Public Sub CHECKBUTTONS()
    '    CHECKOPERATIONS_FalseOrTrue(2, GProfIDLog)
    '    Dim DT As New DataTable
    '    DT.Clear()
    '    DT = CHECKOPERATIONS_FalseOrTrue(2, GProfIDLog)
    '    If BtnSave.Visibility = DT.Rows(0).Item("CanSave") = True Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    If BtnEdit.Visibility = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    If BtnDelete.Visibility = DT.Rows(0).Item("CanDelete") = True Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    '  If BtnSearch.Enabled = DT.Rows(0).Item("CanSearch") = True Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    'End Sub
    Private Sub FrmCoBranch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()

        newRecord()
        'CHECKBUTTONS()
    End Sub

    Private Sub PictureEdit1_Click(sender As Object, e As EventArgs) Handles PictureEdit1.Click
        FrmViewCoBranch.ShowDialog()
    End Sub

    Private Sub CountryID_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CountryID.ButtonClick
        If e.Button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Plus Then
            FRMCountries.ShowDialog()
        End If
    End Sub

    Private Sub CityID_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles CityID.ButtonClick
        If e.Button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Plus Then
            FRMCities.ShowDialog()
        End If
    End Sub

    Private Sub CountryID_TextChanged(sender As Object, e As EventArgs) Handles CountryID.TextChanged
        If CountryID.EditValue <> -1 Or CountryID.Text <> String.Empty Then
            LOADCITIES()
        End If
    End Sub



    Private Sub CountryID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles CountryID.QueryPopUp
        CountryID.Properties.PopulateColumns()
        CountryID.Properties.Columns("ID").Visible = False
        CountryID.Properties.Columns("CCode").Visible = False
    End Sub
End Class