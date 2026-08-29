Imports System.Data.SqlClient
Imports MetroFramework

Public Class FRMSAFE
    Dim clas As New CLSSAFE
    Public Property SBID As Integer
    Public Property IsUpdate As Boolean

    Sub newRecord()
        Code.Text = Format(GETMAXID("SafeTb", "ID") + 1, "SF00000")
        IsActiveTG.IsOn = True
        SafeName.Text = String.Empty
        SafeName.Select()
        SafeType.SelectedIndex = -1
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub
    Public Overrides Sub SetData()
        If SafeName.Text = String.Empty Then
            SafeName.ErrorText = "يرجى إدخال اسم الخزنة"
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = clas.CHECK_SAFE_NAME(SafeName.Text.Trim)
        If dt.Rows.Count > 0 Then
            SafeName.ErrorText = "هذا الاسم موجود مسبقا"
            Exit Sub
        End If
        If SafeType.SelectedIndex = -1 Then
            SafeType.ErrorText = "يرجى إدخال طبيعة الحساب"
            Exit Sub
        End If
        If IsUpdate = False Then
            clas.INSERTTB__Store(Code.Text, SafeName.Text.Trim, SafeType.SelectedIndex, IsActiveTG.IsOn)
            InfoMessage(Me, "رسالة تأكيد", "تم حذف البيانات بنجاح")
            newRecord()
        End If
        MyBase.SetData()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If SafeName.Text = String.Empty Then
            SafeName.ErrorText = "يرجى إدخال اسم الخزنة"
            Exit Sub
        End If
        If SafeType.SelectedIndex = -1 Then
            SafeType.ErrorText = "يرجى إدخال طبيعة الحساب"
            Exit Sub
        End If
        If IsUpdate = True Then
            clas.UPDATETB_SAFE(Code.Text, SafeName.Text.Trim, SafeType.SelectedIndex, IsActiveTG.IsOn)
        End If
        newRecord()

        MyBase.Update()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Sub SHOW_SAFE(x As String)
        If IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clas.SERACH_SAFE(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("Code").ToString
                SafeName.Text = DT.Rows(0)("SafeName").ToString
                SafeType.SelectedIndex = DT.Rows(0)("SafeType").ToString
                IsActiveTG.IsOn = DT.Rows(0)("IsActive").ToString
            Else
                newRecord()
            End If
        End If
    End Sub
    Public Sub CHECKBUTTONS()
        Dim DT As New DataTable
        DT.Clear()
        DT = CHECKOPERATIONS_FalseOrTrue(18, GProfIDLog)
        If BtnSave.Visibility = DT.Rows(0).Item("CanSave") = True Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        If BtnEdit.Visibility = DT.Rows(0).Item("CanEdit") = True Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        If BtnDelete.Visibility = DT.Rows(0).Item("CanDelete") = True Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub

    Private Sub FRMSAFE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CHECKBUTTONS()
        If IsUpdate = False Then
            newRecord()
        End If
    End Sub
End Class