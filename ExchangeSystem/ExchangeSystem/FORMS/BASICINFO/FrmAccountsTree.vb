Imports System.ComponentModel
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class FrmAccountsTree
    Public clsa As New CLSAccount
    Public AccLine As Integer
    Public AcID, IDCode As UInt64
    Public IsUpdate As Boolean

    Public Overrides Sub CHECKBUTTONS()
        lodePreportes()
        MyBase.CHECKBUTTONS()
    End Sub



    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(9, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If

    End Sub
    Sub NEWRECORD()

        IsUpdate = False
        BtnDelete.Enabled = False
        BtnEdit.Enabled = False
        BtnSave.Enabled = True
        BtnPrint.Enabled = True
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRNACH()
        AccMaxLimit.Properties.Buttons(0).Visible = False
        AccMaxDuration.Properties.Buttons(0).Visible = False

        AccName.Text = ""
        AccType.SelectedIndex = -1
        AccParent.EditValue = -1
        AccDmType.SelectedIndex = -1
        AccFinal.SelectedIndex = -1
        AccCat.SelectedIndex = -1
        AccCat.ReadOnly = True
        BranchID.EditValue = -1
        AccPhone.Text = ""
        AccMobile.Text = ""
        AccEmail.Text = ""
        AccNote.Text = ""
        AccMaxLimit.EditValue = 0.00
        AccMaxDuration.EditValue = 0

        Get_AccountID(0, 0)
        clsa.Load_Tree()
        'Dim dt As New DataTable
        'dt.Clear()
        'dt = RUN_QUARY_TXT("ACCOUNTSTB_TReevive")
        'clsa.PopulateTreeView(dt, 0, Nothing)
        LoadAccParent()
    End Sub
    Sub AccountLevel()
        'If AccCode.Text.Length = 1 Then
        '    AccCat.SelectedIndex = 0
        'ElseIf AccCode.Text.Length = 2 Then
        '    AccCat.SelectedIndex = 1
        'ElseIf AccCode.Text.Length = 3 Then
        '    AccCat.SelectedIndex = 2
        'ElseIf AccCode.Text.Length = 4 Then
        '    AccCat.SelectedIndex = 3
        'ElseIf AccCode.Text.Length = 5 Then

        '    AccCat.SelectedIndex = 4
        'Else
        '    AccCat.SelectedIndex = 5


        'End If
        AccCat.SelectedIndex = AccLine
    End Sub
    Sub LoadAccParent()
        Dim DT As New DataTable
        DT.Clear()
        DT = clsa.ACCOUNTSTB_LoadAccParent()

        If DT.Rows.Count > 0 Then
            AccParent.Properties.DataSource = DT
            AccParent.Properties.DisplayMember = "AccName"
            AccParent.Properties.ValueMember = "AccCode"
        End If
    End Sub
    Sub LOADBRNACH()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("CoBranches_LoadDataIntoLookUpEdit")
        BranchID.Properties.DataSource = DT
        BranchID.Properties.ValueMember = "DBRID"
        BranchID.Properties.DisplayMember = "BName"
        BranchID.Properties.ShowHeader = False
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True

        If AccCode.Text = String.Empty Then
            AccCode.ErrorText = "هذه الحقل مطلوب"
            AccCode.Select()
            Exit Sub
        End If

        If AccName.Text = String.Empty Then
            AccName.ErrorText = "هذه الحقل مطلوب"
            AccName.Select()
            Exit Sub
        End If
        If AccType.SelectedIndex = -1 And AccParent.EditValue = -1 Then
            XtraMessageBox.Show(lookAndFeelError, "الرجاء اختيار الحساب الأب أولاً", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

            AccParent.Select()
            Exit Sub
        End If

        If AccFinal.SelectedIndex = -1 Then
            AccFinal.ErrorText = "هذه الحقل مطلوب"
            AccFinal.Select()
            Exit Sub
        End If
        'If AccCat.SelectedIndex > 4 Then
        If AccDmType.SelectedIndex = -1 Then
            AccDmType.ErrorText = "هذا الحقل مطلوب"
            Exit Sub

        End If

        '    ' pass the UserLookAndFeel as a Parameter in the show method
        '    XtraMessageBox.Show(lookAndFeelError, "المعذره لقد وصلت الي المستوى الاخير من دليل الحسابات", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        Dim parent As ULong
        If AccParent.EditValue = -1 Then
            parent = 0
        ElseIf AccParent.EditValue <> -1 Then
            parent = AccParent.EditValue
        End If
        Dim BRACCID As ULong
        If BranchID.EditValue = -1 Then
            BRACCID = 0
        ElseIf BranchID.EditValue <> -1 Then
            BRACCID = BranchID.EditValue
        End If
        clsa.ACCOUNTSTB_insert(AccCode.Text.Trim, AccName.Text.Trim, AccType.SelectedIndex, parent, AccDmType.SelectedIndex, AccFinal.SelectedIndex, AccPhone.Text.Trim, AccMobile.Text.Trim, AccEmail.Text.Trim,
                               AccAddress.Text.Trim, AccNote.Text.Trim, AccMaxLimit.EditValue, AccMaxDuration.EditValue, BRACCID, AccCat.SelectedIndex, IDCode)
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        BtnNew.PerformClick()
        MyBase.Save()
    End Sub

    Public Overrides Sub UPDATERECORD()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        If AccCode.Text = String.Empty Then
            AccCode.ErrorText = "هذه الحقل مطلوب"
            AccCode.Select()
            Exit Sub
        End If

        If AccName.Text = String.Empty Then
            AccName.ErrorText = "هذه الحقل مطلوب"
            AccName.Select()
            Exit Sub
        End If
        If AccType.SelectedIndex = 1 And AccParent.EditValue = -1 Then
            XtraMessageBox.Show(lookAndFeelError, "الرجاء اختيار الحساب الأب أولاً", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

            AccParent.Select()
            Exit Sub
        End If

        If AccFinal.SelectedIndex = -1 Then
            AccFinal.ErrorText = "هذه الحقل مطلوب"
            AccFinal.Select()
            Exit Sub
        End If
        'If AccCode.Text.Length > 15 Then
        '    XtraMessageBox.Show(lookAndFeelError, "المعذره لقد وصلت الي المستوى الاخير من دليل الحسابات", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If
        Dim parent As ULong
        If AccParent.EditValue = -1 Then
            parent = 0
        ElseIf AccParent.EditValue <> -1 Then
            parent = AccParent.EditValue
        End If

        If AcID = 0 Then Exit Sub
        Dim BRACCID As ULong
        If BranchID.EditValue = -1 Then
            BRACCID = 0
        ElseIf BranchID.EditValue <> -1 Then
            BRACCID = BranchID.EditValue
        End If

        clsa.ACCOUNTSTB_update(AccCode.Text.Trim, AccName.Text.Trim, AccType.SelectedIndex, parent, AccDmType.SelectedIndex, AccFinal.SelectedIndex, AccPhone.Text.Trim, AccMobile.Text.Trim, AccEmail.Text.Trim, AccAddress.Text.Trim,
                               AccNote.Text.Trim, AccMaxLimit.EditValue, AccMaxDuration.EditValue,
                               BRACCID, AcID, AccCat.SelectedIndex, AccActive.EditValue, IDCode)
        BtnNew.PerformClick()
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub Remove()
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim warn As New Icon(Application.StartupPath & "\warning.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Warning) = warn
        Dim ques As New Icon(Application.StartupPath & "\question.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Question) = ques
        Dim info As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Information) = info
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        'lookAndFeelError.SkinName = "MilkShake"
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        If TreeView1.Nodes.Count = 0 Then Exit Sub
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("delete from AccountsTb where AccCode=N'" & AccCode.Text & "'")
        'Dim c As Int16 = Me.TreeView1.SelectedNode.GetNodeCount(False)
        'If c > 0 Then
        '    XtraMessageBox.Show(lookAndFeelError, "معذرة قد يكون  الحساب " & Space(1) & Me.TreeView1.SelectedNode.Text & Space(1) & " أب إلى حسابات أخرى" & vbNewLine & Space(1) & " لا يمكن حذف هذا الحساب ", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'Else
        '    If XtraMessageBox.Show(lookAndFeelError, "أنت على وشك حذف هذا المجلد" & Space(1) & Me.TreeView1.SelectedNode.Text & "هل تريد حذف هذا الحساب ؟", "رسالة تنبيه قبل الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

        '        clsa.ACCOUNTSTB_delete(AcID, UserID)
        '        Me.TreeView1.SelectedNode.Remove()
        '        XtraMessageBox.Show(lookAndFeelError, "تمت عملية الحذف بنجاح ", "عملية الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)

        '    Else

        XtraMessageBox.Show(lookAndFeelError, "تم إلغاء عملية الحذف بنجاح ", "عملية الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)

        'End If
        BtnNew.PerformClick()
        'End If
        MyBase.Remove()
    End Sub
    Public Sub Get_AccountID(fatherparent As Decimal, ACCTYPE As Integer)
        Dim dt As New DataTable
        dt.Clear()
        dt = clsa.ACCOUNTSTB_SelectMax(fatherparent, ACCTYPE)
        If dt.Rows.Count > 0 Then
            AccCode.EditValue = dt.Rows(0)("code")
            AccLine = dt.Rows(0)("Accline")
            IDCode = dt.Rows(0)("IDcode")
            AccountLevel()
            'AccCat.SelectedIndex = dt.Rows(0)("Accline")

        End If
    End Sub

    Private Sub FrmAccountsTree_Load(sender As Object, e As EventArgs) Handles Me.Load
        lodePreportes()
        NEWRECORD()
        BtnNew.PerformClick()

    End Sub

    Private Sub AccParent_QueryPopUp(sender As Object, e As CancelEventArgs) Handles AccParent.QueryPopUp
        Dim DT As New DataTable
        DT.Clear()
        DT = clsa.ACCOUNTSTB_LoadAccParent()
        If DT.Rows.Count > 0 Then
            AccParent.Properties.ShowHeader = False
            AccParent.Properties.PopulateColumns()
            AccParent.Properties.Columns("AccCode").Visible = False
        End If
    End Sub
    Private Sub ACCTYPE_TextChanged(sender As Object, e As EventArgs) Handles AccType.TextChanged
        If AccType.SelectedIndex = 0 Then
            AccParent.EditValue = Nothing
            Get_AccountID(0, 0)
            AccountLevel()
            'AccCat.SelectedIndex = 0

        Else
            AccParent.Enabled = True
        End If
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("DBRID").Visible = False

        BranchID.Properties.Columns("BranchType").Visible = False
    End Sub


    Private Sub AccParent_TextChanged(sender As Object, e As EventArgs) Handles AccParent.TextChanged
        If AccParent.EditValue <> -1 And AccType.SelectedIndex <> -1 Then
            Get_AccountID(AccParent.EditValue, AccType.SelectedIndex)
            'AccCat.SelectedIndex = AccLine - 1
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        clsa.Load_Tree()
    End Sub
    Public Sub LoadData()
        Dim ss As UInt64
        Dim dt2 As New DataTable
        dt2.Clear()
        ss = TreeView1.SelectedNode.Tag
        dt2 = clsa.ACCOUNTSTB_selectByCode(ss)
        If dt2.Rows.Count > 0 Then
            'LoadAccParent()
            AcID = dt2.Rows(0)("AccID")
            AccName.Text = dt2.Rows(0)("AccName").ToString
            AccType.SelectedIndex = dt2.Rows(0)("AccType")
            AccLine = dt2.Rows(0)("Accline")
            AccCat.SelectedIndex = dt2.Rows(0)("Accline")
            If dt2.Rows(0)("AccParent") = 0 Then
                AccParent.EditValue = -1
                AccParent.EditValue = Nothing
            Else
                AccParent.EditValue = dt2.Rows(0)("AccParent")
            End If
            AccDmType.SelectedIndex = dt2.Rows(0)("AccDmType")
            AccFinal.SelectedIndex = dt2.Rows(0)("AccFinal")
            AccNote.Text = dt2.Rows(0)("AccNotes").ToString
            AccPhone.Text = dt2.Rows(0)("AccPhone").ToString
            AccMobile.Text = dt2.Rows(0)("AccMobile").ToString
            AccEmail.Text = dt2.Rows(0)("AccEmail").ToString
            AccAddress.Text = dt2.Rows(0)("AccAddress").ToString
            AccMaxLimit.EditValue = dt2.Rows(0)("AccMaxLimit")
            AccMaxDuration.EditValue = dt2.Rows(0)("AccMaxDuration")
            If dt2.Rows(0)("BranchID") = 0 Or dt2.Rows(0)("BranchID").ToString = "" Then
                BranchID.EditValue = -1
            Else
                BranchID.EditValue = dt2.Rows(0)("BranchID")
            End If
            AccActive.EditValue = dt2.Rows(0)("AccActive")
            BtnSave.Enabled = False
            BtnEdit.Enabled = True
            BtnDelete.Enabled = True
            AccCode.Text = dt2.Rows(0)("AccCode")
        End If
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        IsUpdate = True
        If IsUpdate = True Then
            LoadData()
        End If

    End Sub

    Private Sub AccCode_EditValueChanged(sender As Object, e As EventArgs) Handles AccCode.EditValueChanged

    End Sub


    Private Sub AccCode_TextChanged(sender As Object, e As EventArgs) Handles AccCode.TextChanged
        AccountLevel()
    End Sub
End Class