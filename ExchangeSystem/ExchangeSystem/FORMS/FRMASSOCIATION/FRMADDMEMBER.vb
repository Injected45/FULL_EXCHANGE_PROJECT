Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading

Public Class FRMADDMEMBER
    Dim clse As New CLSADDMEMBER
    Public CLSA As New CLSAccount
    Public AcID, IDCode, AccCode, AccEm, AcIDID As ULong
    Public Property AccID As ULong
    Public Property EMNAME As String
    Public StID, AccLine, AccCat As Integer
    Public Property X As String
    Public Property AccNew As String
    Public IsUpdate, UpdateBySalary As Boolean
    Sub DISAPLETOOLS()
        CodeID.Enabled = False
        EMPNAME.Enabled = False
        ASSOCIATION.Enabled = False
        PHONE1.Enabled = False
        PassportNo.Enabled = False
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(92, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If


    End Sub
    Sub ENABLETOOLS()
        EMPNAME.Enabled = True
        ASSOCIATION.Enabled = True
        PHONE1.Enabled = True
        PassportNo.Enabled = True
    End Sub
    Public Sub LOADASSOCIATION()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ASSOCIATIONNAMETB_LOADTODVG")
        If DT.Rows.Count > 0 Then
            ASSOCIATION.Properties.DataSource = DT
            ASSOCIATION.Properties.DisplayMember = "ASSNAME"
            ASSOCIATION.Properties.ValueMember = "ID"
            ASSOCIATION.Properties.ShowHeader = False
        End If
    End Sub
    Sub NewRecord()
        IsUpdate = False
        CodeID.Enabled = False
        CodeID.Text = GETMAXID("ASSOCIATIONTB", "ID") + 1
        ENABLETOOLS()
        LOADASSOCIATION()
        IsActiveTG.IsOn = True
        EMPNAME.Text = String.Empty
        EMPNAME.Select()
        ASSOCIATION.EditValue = -1
        PHONE1.Text = String.Empty
        PHONE1.Text = String.Empty
        PassportNo.Text = String.Empty
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    End Sub

    Private Sub ASSOCIATION_ButtonClick(sender As Object, e As Controls.ButtonPressedEventArgs) Handles ASSOCIATION.ButtonClick
        If e.Button.Index = 1 Then
            FRMADDASSOCIATION.ShowDialog()
        End If
    End Sub

    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
    Public Overrides Sub EnterKeyMove()
        MyBase.EnterKeyMove()
    End Sub
    Public Overrides Sub SetData()

        If IsUpdate = False Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clse.ASSOCIATIONTB_CHECKMEMBERNAME(EMPNAME.Text.Trim, ASSOCIATION.EditValue)
            If DT.Rows.Count > 0 Then
                Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
                XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
                XtraMessageBox.AllowCustomLookAndFeel = True
                Dim reuslt = XtraMessageBox.Show(lookAndFeelError, "اسم العضو موجود مسبقا في هذه الجمعية", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If EMPNAME.Text = String.Empty Then
                EMPNAME.ErrorText = "يرجى إخال اسم الموظف"
                Exit Sub
            End If

            If ASSOCIATION.EditValue = -1 Then
                ASSOCIATION.ErrorText = "يرجى اختيار التصنيف"
                Exit Sub
            End If
            clse.INSERTTB_MEMBER(CodeID.Text, EMPNAME.Text.Trim, PHONE1.Text.Trim, PassportNo.Text.Trim, ASSOCIATION.EditValue, IsActiveTG.IsOn, IsUpdate, MAINBID)
        End If
        NewRecord()
        MyBase.SetData()
    End Sub

    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        FRMADDEXISITMEMBER.ShowDialog()
    End Sub

    Public Overrides Sub UPDATERECORD()
        If Me.IsUpdate = True Then
            If EMPNAME.Text = String.Empty Then
                EMPNAME.ErrorText = "يرجى إخال اسم الموظف"
                Exit Sub
            End If
            If ASSOCIATION.EditValue = -1 Then
                ASSOCIATION.ErrorText = "يرجى اختيار التصنيف"
                Exit Sub
            End If
            clse.INSERTTB_MEMBER(CodeID.Text, EMPNAME.Text.Trim, PHONE1.Text.Trim, PassportNo.Text.Trim, ASSOCIATION.EditValue, IsActiveTG.IsOn, IsUpdate, MAINBID)
        End If
        NewRecord()
        MyBase.UPDATERECORD()
    End Sub

    Sub SHOW_EMP(x)
        If Me.IsUpdate = True Then
            Dim DT As New DataTable
            DT.Clear()
            DT = clse.SERACH_EMPLOYEE(x)
            If DT.Rows.Count > 0 Then
                EMPNAME.Text = DT.Rows(0)("MEMBERNAME").ToString
                CodeID.Text = DT.Rows(0)("ID")
                PHONE1.Text = DT.Rows(0)("PHONE").ToString
                PassportNo.Text = DT.Rows(0)("IDNO").ToString
                ASSOCIATION.EditValue = DT.Rows(0)("ASSOCIATIONID")
                IsActiveTG.EditValue = DT.Rows(0)("IsActive")
                AccID = DT.Rows(0)("AccID")
            End If
        End If
    End Sub
    Public Overrides Sub Remove()
        MyBase.Remove()
    End Sub
    Private Sub FRMEMPLOOYE_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        lodePreportes()
        'CHECKBUTTONS()
        If IsUpdate = False Then
            NewRecord()
        End If
    End Sub
    Private Sub FRMEMPLOYEE_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        ENABLETOOLS()
        IsUpdate = False
        NewRecord()
    End Sub

    Private Sub ASSOCIATION_QueryPopUp(sender As Object, e As CancelEventArgs) Handles ASSOCIATION.QueryPopUp
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("ASSOCIATIONNAMETB_LOADTODVG")
        If DT.Rows.Count > 0 Then
            ASSOCIATION.Properties.PopulateColumns()
            ASSOCIATION.Properties.Columns("ID").Visible = False
        End If
    End Sub

    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        'FRMVIEWMEMBER.GVRole.Columns.Clear()
        'FRMVIEWMEMBER.GCRole.DataSource = Nothing
        FRMVIEWMEMBER.AssID.EditValue = -1
        FRMVIEWMEMBER.ShowDialog()
    End Sub
End Class
Public Class CLSADDMEMBER
    Public Sub INSERTTB_MEMBER(ID As Integer, MEMBERNAME As String, PHONE As String, IDNO As String, ASSOCIATIONID As Integer, IsActive As Boolean, IsUpdate As Boolean, BranchID As Integer)
        Dim PRM(7) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(1) = New SqlParameter("@MEMBERNAME", SqlDbType.NVarChar, -1) With {.Value = MEMBERNAME}
        PRM(2) = New SqlParameter("@PHONE", SqlDbType.NVarChar, -1) With {.Value = PHONE}
        PRM(3) = New SqlParameter("@IDNO", SqlDbType.NVarChar, -1) With {.Value = IDNO}
        PRM(4) = New SqlParameter("@ASSOCIATIONID", SqlDbType.Int) With {.Value = ASSOCIATIONID}
        PRM(5) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(6) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(7) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}

        RUN_EXUTE_PRO("ASSOCIATIONTB_Insert", PRM)

    End Sub
    Public Function SERACH_EMPLOYEE(ID As ULong) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.BigInt)
        PRM(0).Value = ID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ASSOCIATIONTB_Select", PRM)
        Return DT
    End Function
    Public Function ASSOCIATIONTB_CHECKMEMBERNAME(MEMBERNAME As String, ASSOCIATIONID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@MEMBERNAME", SqlDbType.NVarChar, -1)
        PRM(0).Value = MEMBERNAME
        PRM(1) = New SqlParameter("@ASSOCIATIONID", SqlDbType.Int)
        PRM(1).Value = ASSOCIATIONID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ASSOCIATIONTB_CHECKMEMBERNAME", PRM)
        Return DT
    End Function
End Class