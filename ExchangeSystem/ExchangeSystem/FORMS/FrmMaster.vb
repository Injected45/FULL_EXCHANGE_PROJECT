
Imports DevExpress.XtraEditors
Imports ExchangeSystem.ExchangeSystem
Imports ExchangeSystem.ExchangeSystem.CLSFRM
Partial Public Class FrmMaster
    Public Overridable Sub CHECKBUTTONS()
        CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        Dim DT As New DataTable
        DT.Clear()
        DT = CHECKBUTTON_TRUEORFALSE(Me.Tag, UserID, GProfIDLog)
        If DT.Rows.Count > 0 Then
            If BtnSave.Visibility = DT.Rows(0).Item("CanAdd") = True Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            If BtnEdit.Visibility = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            If BtnDelete.Visibility = DT.Rows(0).Item("CanDelete") = True Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            '  If BtnSearch.Enabled = DT.Rows(0).Item("CanSearch") = True Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        End If
    End Sub
    Public Shared Function CheckActionAuthorization(ByVal formName As String, ByVal actions As Master.Actions, ByVal Optional user As DAL.TB_User = Nothing) As Boolean
        If user Is Nothing Then user = Session.User

        'If user.UserType = CByte(Master.UserType.Admin) Then
        '    Return True
        'Else

        Dim screen = Session.ScreensAccesses.SingleOrDefault(Function(x) x.ScreenName = formName)
        Dim flag As Boolean = True

        If screen IsNot Nothing Then

            Select Case actions
                Case Master.Actions.Add
                    flag = screen.CanAdd
                Case Master.Actions.Edit
                    flag = screen.CanEdit
                Case Master.Actions.Delete
                    flag = screen.CanDelete
                Case Master.Actions.Print
                    flag = screen.CanPrint
                Case Else
            End Select
        End If

        If flag = False Then
            XtraMessageBox.Show(text:="غير مصرح لك ", caption:="", icon:=MessageBoxIcon.[Error], buttons:=MessageBoxButtons.OK)
        End If

        Return flag
        'End If
    End Function
    Public Shared ReadOnly Property TextError As String
        Get
            Return "هذا الحقل مطلوب"
        End Get
    End Property
    Public Overridable Sub Save()


    End Sub

    Public Overridable Sub RefreshData()
        GetData()
    End Sub
    Public Overridable Function IsDataValid() As Boolean
        Return True
    End Function
    Public Overridable Sub Remove()
        FrmRemoveMessage.Show()
    End Sub
    Public Overridable Sub UPDATERECORD()
        FrmEditMessage.Show()
        RefreshData()
    End Sub
    Public Overridable Sub GetData()

    End Sub
    Public Overridable Sub SetData()
        'FrmSavedSuccessfully.Show()
    End Sub
    Public Overridable Sub Print()

    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnSave.ItemClick
        Save()
    End Sub

    Public Sub BtnNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnNew.ItemClick
        BNew()
    End Sub
    Public Overridable Sub BNew()
    End Sub
    Private Sub BarButtonItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnDelete.ItemClick
        Remove()
    End Sub

    'Public Shared Function AskForDeletion() As Boolean
    'Return (MetroFramework.MetroMessageBox.Show(Form.ActiveForm, "سيتم حذف هذه البيانات من قاعدة البيانات نهائيا، هل تريد الاستمرار؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) = DialogResult.Yes)
    'End Function

    Private Sub BtnPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnPrint.ItemClick
        Print()
    End Sub

    Public Sub BtnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BtnEdit.ItemClick
        UPDATERECORD()
    End Sub

    Private Sub FrmMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CHECKBUTTONS()
    End Sub
    Public Overridable Sub EnterKeyMove()
        FrmMaster_KeyDown(Nothing, Nothing)
    End Sub
    Private Sub FrmMaster_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            FRMMAIN.Refreshtimer()
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True 'this will prevent ding sound 
    End Sub

    Private Sub FrmMaster_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        FRMMAIN.Refreshtimer()
    End Sub
End Class
