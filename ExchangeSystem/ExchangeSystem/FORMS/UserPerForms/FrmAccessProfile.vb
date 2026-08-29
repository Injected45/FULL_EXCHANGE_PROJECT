Imports DevExpress.XtraEditors.Repository
Imports DevExpress.LookAndFeel
Imports ExchangeSystem.ExchangeSystem
Imports ExchangeSystem.ExchangeSystem.CLSFRM
Imports DevExpress.Utils
Imports DevExpress.XtraTreeList.Nodes.Operations
Imports DevExpress.XtraTreeList.Nodes
'Namespace ExchangeSystem.Forms
Public Class FrmAccessProfile
    Private CLSACCP As New CLSUSERACCESSPROFILE
    Private CLASSSCAP As New CLSSCREENACCESSPROFILE
    Private profile As New DAL.UserAccessProfileName
    Public IsUpdate As Boolean
    Public RP1, RPAGE2, RPAGE3, RPAGE4, RPAGE5, RPAGE6 As Integer
    Public Overrides Sub CHECKBUTTONS()
        MyBase.CHECKBUTTONS()
    End Sub

    Public Sub New()
        InitializeComponent()
        RefreshData()
        GetData()
    End Sub
    Public Sub New(ByVal id As Integer)
        InitializeComponent()
        IsUpdate = True
        Using db = New DAL.DataClasses1DataContext()
            profile = db.UserAccessProfileNames.SingleOrDefault(Function(x) x.ID = id)
        End Using
        ProfileName.Text = profile.Name
        GetData()
        Dim DT As New DataTable
        DT.Clear()
        DT = CLASSSCAP.RibbonPermission_Select(id)
        If DT.Rows.Count > 0 Then
            RPBASICINFO.EditValue = DT.Rows(0)("CanShow")
            RP2.EditValue = DT.Rows(1)("CanShow")
            RP3.EditValue = DT.Rows(2)("CanShow")
            RP4.EditValue = DT.Rows(3)("CanShow")
            RP5.EditValue = DT.Rows(4)("CanShow")
            RP6.EditValue = DT.Rows(5)("CanShow")
        End If

        ' TreeListPerform()
    End Sub
    Public Sub [New]()
        profile = New DAL.UserAccessProfileName()

    End Sub
    Public Overrides Sub RefreshData()
        profile = New DAL.UserAccessProfileName()
        'ProfileName.Text = ""
    End Sub

    Private ins As CLSFRM.ScreensAccessProfile
    'Private scr As Screens

    Public Overrides Sub GetData()
        Dim data As List(Of CLSFRM.ScreensAccessProfile)

        'treeList1.DataSource = Screens.GetScreens
        Using db As DAL.DataClasses1DataContext = New DAL.DataClasses1DataContext
            data = (From s In Screens.GetScreens From d In db.UserAccessProfileDetails.Where(Function(x) x.ProfileID = profile.ID AndAlso x.ScreenID = s.ScreenID).DefaultIfEmpty() Select New CLSFRM.ScreensAccessProfile(s.ScreenName) With {
                            .CanAdd = If((d Is Nothing), True, d.CanAdd),
                            .CanDelete = If((d Is Nothing), True, d.CanDelete),
                            .CanEdit = If((d Is Nothing), True, d.CanEdit),
                            .CanOpen = If((d Is Nothing), True, d.CanOpen),
                            .CanPrint = If((d Is Nothing), True, d.CanPrint),
                            .CanShow = If((d Is Nothing), True, d.CanShow),
                            .Actions = s.Actions,
                            .ScreenName = s.ScreenName,
                            .ScreenCaption = s.ScreenCaption,
                            .ScreenID = s.ScreenID,
                            .ParentScreenID = s.ParentScreenID
                        }).ToList()
        End Using

        treeList1.DataSource = data
    End Sub

    'Public Function IsDataValid() As Boolean
    '    Dim flag As Integer = 0

    '    If textEdit1.Text.Trim() = String.Empty Then
    '        flag += 1
    '        textEdit1.ErrorText = "هذا الحقل مطلوب"
    '    End If

    '    Return (flag = 0)
    'End Function

    Public Overrides Sub SAVE()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub SetData()
        'If IsUpdate = False Then
        RP1 = 1
            RPAGE2 = 2
            RPAGE3 = 3
            RPAGE4 = 4
            RPAGE5 = 5
            RPAGE6 = 6
            If ProfileName.Text.Trim() = String.Empty Then
                ProfileName.ErrorText = "هذا الحقل مطلوب"
            End If
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RP1, RPBASICINFO.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE2, RP2.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE3, RP3.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE4, RP4.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE5, RP5.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE6, RP6.EditValue, IsUpdate)
            Dim db = New DAL.DataClasses1DataContext()

            If profile.ID = 0 Then
                db.UserAccessProfileNames.InsertOnSubmit(profile)
            Else
                db.UserAccessProfileNames.Attach(profile)
            End If

            profile.Name = ProfileName.Text
            db.SubmitChanges()
            db.UserAccessProfileDetails.DeleteAllOnSubmit(db.UserAccessProfileDetails.Where(Function(x) x.ProfileID = profile.ID))
            db.SubmitChanges()
            Dim data = TryCast(treeList1.DataSource, List(Of [CLSFRM].ScreensAccessProfile))
            Dim dbData = data.[Select](Function(s) New DAL.UserAccessProfileDetail With {
                    .CanAdd = s.CanAdd,
                    .CanDelete = s.CanDelete,
                    .CanEdit = s.CanEdit,
                    .CanOpen = s.CanOpen,
                    .CanPrint = s.CanPrint,
                    .CanShow = s.CanShow,
                    .ProfileID = profile.ID,
                    .ScreenID = s.ScreenID
                }).ToList()
            db.UserAccessProfileDetails.InsertAllOnSubmit(dbData)
            db.SubmitChanges()
            GetData()
            profile.Name = ""
            RefreshData()
        'End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub UPDATERECORD()
        IsUpdate = True
        If IsUpdate = True Then
            RP1 = 1
            RPAGE2 = 2
            RPAGE3 = 3
            RPAGE4 = 4
            RPAGE5 = 5
            RPAGE6 = 6
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RP1, RPBASICINFO.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE2, RP2.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE3, RP3.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE4, RP4.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE5, RP5.EditValue, IsUpdate)
            CLASSSCAP.RibbonPermission_Insert(profile.ID, RPAGE6, RP6.EditValue, IsUpdate)
            Dim db = New DAL.DataClasses1DataContext()

            If profile.ID = 0 Then
                db.UserAccessProfileNames.InsertOnSubmit(profile)
            Else
                db.UserAccessProfileNames.Attach(profile)
            End If

            profile.Name = ProfileName.Text
            db.SubmitChanges()
            db.UserAccessProfileDetails.DeleteAllOnSubmit(db.UserAccessProfileDetails.Where(Function(x) x.ProfileID = profile.ID))
            db.SubmitChanges()
            Dim data = TryCast(treeList1.DataSource, List(Of [CLSFRM].ScreensAccessProfile))
            Dim dbData = data.[Select](Function(s) New DAL.UserAccessProfileDetail With {
                    .ProfileID = profile.ID,
                    .ScreenID = s.ScreenID,
                    .CanShow = s.CanShow,
                    .CanOpen = s.CanOpen,
                    .CanAdd = s.CanAdd,
                    .CanEdit = s.CanEdit,
                    .CanDelete = s.CanDelete,
                    .CanPrint = s.CanPrint
                }).ToList()
            db.UserAccessProfileDetails.InsertAllOnSubmit(dbData)
            db.SubmitChanges()
            GetData()
            profile.Name = ""
            RefreshData()
        End If
        MyBase.UPDATERECORD()
    End Sub
    Public Sub TreeListPerform()
        ProfileName.Text = profile.Name
        AddHandler treeList1.CustomNodeCellEdit, AddressOf treeList1_CustomNodeCellEdit
        treeList1.KeyFieldName = NameOf(ins.ScreenID)
        treeList1.ParentFieldName = NameOf(ins.ParentScreenID)
        treeList1.Columns(NameOf(ins.ScreenName)).Visible = False
        treeList1.Columns(NameOf(ins.ScreenName)).OptionsColumn.AllowEdit = False
        treeList1.Columns(NameOf(ins.ScreenCaption)).OptionsColumn.AllowEdit = False
        treeList1.Columns(NameOf(ins.CanAdd)).Caption = "اضافه"
        treeList1.Columns(NameOf(ins.ScreenCaption)).Caption = "البيان"
        treeList1.Columns(NameOf(ins.CanDelete)).Caption = "حذف"
        treeList1.Columns(NameOf(ins.CanEdit)).Caption = "تعديل"
        treeList1.Columns(NameOf(ins.CanOpen)).Visible = False
        treeList1.Columns(NameOf(ins.CanPrint)).Caption = "طباعه"
        treeList1.Columns(NameOf(ins.CanShow)).Caption = "اظهار"
        treeList1.BestFitColumns()
    End Sub
    Private Sub FrmAccessProfile_Load(sender As Object, e As EventArgs) Handles Me.Load
        CHECKBUTTONS()
        ProfileName.Text = profile.Name
        AddHandler treeList1.CustomNodeCellEdit, AddressOf treeList1_CustomNodeCellEdit
        treeList1.KeyFieldName = NameOf(ins.ScreenID)
        treeList1.ParentFieldName = NameOf(ins.ParentScreenID)
        treeList1.Columns(NameOf(ins.ScreenName)).Visible = False
        treeList1.Columns(NameOf(ins.ScreenName)).OptionsColumn.AllowEdit = False
        treeList1.Columns(NameOf(ins.ScreenCaption)).OptionsColumn.AllowEdit = False
        treeList1.Columns(NameOf(ins.CanAdd)).Caption = "اضافه"
        treeList1.Columns(NameOf(ins.ScreenCaption)).Caption = "البيان"
        treeList1.Columns(NameOf(ins.CanDelete)).Caption = "حذف"
        treeList1.Columns(NameOf(ins.CanEdit)).Caption = "تعديل"
        treeList1.Columns(NameOf(ins.CanOpen)).Visible = False
        treeList1.Columns(NameOf(ins.CanPrint)).Caption = "طباعه"
        treeList1.Columns(NameOf(ins.CanShow)).Caption = "اظهار"
        treeList1.BestFitColumns()
        repoCheck = New RepositoryItemCheckEdit()
        repoCheck.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgRadio2
        repoCheck.CheckBoxOptions.SvgColorChecked = DXSkinColors.ForeColors.Information
        repoCheck.CheckBoxOptions.SvgColorUnchecked = DXSkinColors.ForeColors.DisabledText


        treeList1.Columns(NameOf(ins.CanAdd)).ColumnEdit = repoCheck
        treeList1.Columns(NameOf(ins.CanDelete)).ColumnEdit = repoCheck
        treeList1.Columns(NameOf(ins.CanEdit)).ColumnEdit = repoCheck
        treeList1.Columns(NameOf(ins.CanOpen)).ColumnEdit = repoCheck
        treeList1.Columns(NameOf(ins.CanPrint)).ColumnEdit = repoCheck
        treeList1.Columns(NameOf(ins.CanShow)).ColumnEdit = repoCheck

        treeList1.Columns(NameOf(ins.CanAdd)).Width = 25
        treeList1.Columns(NameOf(ins.CanDelete)).Width = 25
        treeList1.Columns(NameOf(ins.CanEdit)).Width = 25
        treeList1.Columns(NameOf(ins.CanOpen)).Width = 25
        treeList1.Columns(NameOf(ins.CanPrint)).Width = 25
        treeList1.Columns(NameOf(ins.CanShow)).Width = 25

        treeList1.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center
        treeList1.Appearance.Row.TextOptions.VAlignment = HorzAlignment.Center
        treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center
        treeList1.Appearance.HeaderPanel.TextOptions.VAlignment = HorzAlignment.Center

    End Sub

    Private Sub treeList1_CustomNodeCellEdit(sender As Object, e As DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs) Handles treeList1.CustomNodeCellEdit
        If e.Node.Id >= 0 Then
            Dim row = TryCast(treeList1.GetRow(e.Node.Id), ScreensAccessProfile)

            If row IsNot Nothing Then

                If e.Column.FieldName = NameOf(ins.CanAdd) AndAlso row.Actions.Contains(Master.Actions.Add) = False Then
                    e.RepositoryItem = New RepositoryItem()
                ElseIf e.Column.FieldName = NameOf(ins.CanDelete) AndAlso row.Actions.Contains(Master.Actions.Delete) = False Then
                    e.RepositoryItem = New RepositoryItem()
                ElseIf e.Column.FieldName = NameOf(ins.CanEdit) AndAlso row.Actions.Contains(Master.Actions.Edit) = False Then
                    e.RepositoryItem = New RepositoryItem()
                ElseIf e.Column.FieldName = NameOf(ins.CanOpen) AndAlso row.Actions.Contains(Master.Actions.Open) = False Then
                    e.RepositoryItem = New RepositoryItem()
                ElseIf e.Column.FieldName = NameOf(ins.CanPrint) AndAlso row.Actions.Contains(Master.Actions.Print) = False Then
                    e.RepositoryItem = New RepositoryItem()
                ElseIf e.Column.FieldName = NameOf(ins.CanShow) AndAlso row.Actions.Contains(Master.Actions.Show) = False Then
                    e.RepositoryItem = New RepositoryItem()
                End If
            End If
        End If
    End Sub

    Private repoCheck As RepositoryItemCheckEdit


    Private Class CSharpImpl
        <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
        Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class

    Private Sub SEDESEALL_EditValueChanged(sender As Object, e As EventArgs) Handles SEDESEALL.EditValueChanged
        '
        'Dim node As TreeListNode
        'Dim myobject = CType(treeList1.GetDataRecordByNode(node), ScreensAccessProfile)
        If SEDESEALL.IsOn = False Then
            treeList1.NodesIterator.DoOperation(New CustomNodeOperationUn())
            treeList1.RefreshDataSource()
        ElseIf SEDESEALL.IsOn = True Then
            treeList1.NodesIterator.DoOperation(New CustomNodeOperation())
            treeList1.RefreshDataSource()
        End If
    End Sub

    Private Sub SEDESEALL_TextChanged(sender As Object, e As EventArgs) Handles SEDESEALL.TextChanged
        'If SEDESEALL.IsOn = False Then
        '    'For i = 0 To treeList1.Columns.Count - 1
        '    treeList1.UncheckAll()
        'ElseIf SEDESEALL.IsOn = True Then
        '    treeList1.CheckAll()
        '    'Next
        'End If
    End Sub
    Class CustomNodeOperationUn
        Inherits TreeListOperation

        Public Sub New()

        End Sub 'New

        Public Overrides Sub Execute(ByVal node As TreeListNode)
            Dim myobject = CType(node.TreeList.GetDataRecordByNode(node), ScreensAccessProfile)
            If myobject IsNot Nothing Then
                myobject.CanAdd = False
                myobject.CanDelete = False
                myobject.CanEdit = False
                myobject.CanOpen = False
                myobject.CanPrint = False
                myobject.CanShow = False
            End If
        End Sub 'Execute

    End Class
    Class CustomNodeOperation
        Inherits TreeListOperation

        Public Sub New()

        End Sub 'New

        Public Overrides Sub Execute(ByVal node As TreeListNode)
            Dim myobject = CType(node.TreeList.GetDataRecordByNode(node), ScreensAccessProfile)
            If myobject IsNot Nothing Then
                myobject.CanAdd = True
                myobject.CanDelete = True
                myobject.CanEdit = True
                myobject.CanOpen = True
                myobject.CanPrint = True
                myobject.CanShow = True
            End If
        End Sub 'Execute

    End Class

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        ViewAccessProfile.ShowDialog()
    End Sub
End Class
'End Namespace
