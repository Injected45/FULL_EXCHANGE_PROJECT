Imports System.ComponentModel
Imports System.Data.SqlClient


Public Class FrmCancelRequest
    Dim IsUpdate As Boolean
    Dim BDID, isidtype As Integer


    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(21, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If

    End Sub
    Sub LOADBRANCH()
        LoadToControlar(BranchID, "CoBranches_LoadDataIntoLookUpEdit", "BName", "DBRID", Nothing)
    End Sub
    Sub LOADREASONS()
        LoadToControlar(ReasonID, "TransCancelRequestTb_GetReasonID", "NewCause", "ID", Nothing)
    End Sub
    Dim RBRTYPE, DBRTYPE, BranchDelivered As Integer
    Sub NEWRECORD()
        LOADBRANCH()
        LOADREASONS()
        IsUpdate = False
        ISIDID.Properties.DataSource = Nothing
        ISIDID.EditValue = -1
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        ReasonID.EditValue = -1
        Notes.Text = ""
        ReasonID.Select()
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnSave.Enabled = True
        BtnDelete.Enabled = False
        BtnEdit.Enabled = False
        BranchID.EditValue = BID
        If BID = MAINBID Then
            BranchID.Enabled = True
        Else
            BranchID.Enabled = False
        End If
        'Dim DT As New DataTable
        'DT = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & BranchID.EditValue & "'")
        'RBRTYPE = DT.Rows(0)("BranchType")
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        Try
            If ReasonID.EditValue = -1 Then
                ReasonID.ErrorText = "يجب اختيار سبب إلغاء الحوالة أولاً"
                Exit Sub
            End If
            If ISIDID.EditValue = -1 Or ISIDID.Text = String.Empty Then
                ISIDID.ErrorText = "يجب اختيار الحوالة أولاً"
                Exit Sub
            End If
            If RadioButton1.Checked = True Then
                RadioButton2.Checked = False
                Dim ConfType As Integer
                If GetLKPColumnVal(ISIDID, "ConfirmType") = 0 Then
                    ConfType = 3
                Else
                    ConfType = 4
                End If
                RUN_EXUTE_TXT("update InternalEx set  ConfirmType='" & ConfType & "' where ID='" & ISIDID.EditValue & "'")
                If GetLKPColumnVal(ISIDID, "RBType") <> 3 Then
                    Whats()
                End If
                FrmSavedSuccessfully.Show()
                End If
            If RadioButton2.Checked = True Then
                RUN_EXUTE_TXT("update ExternalEx set ConfirmCancelBranch='" & BranchID.EditValue & "', IsCanceled=1,ConfirmedType=3 where ID='" & ISIDID.EditValue & "'")
                FrmSavedSuccessfully.Show()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        NEWRECORD()
        MyBase.Save()
    End Sub
    Private Sub FrmCancelRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            ISIDID.Properties.DataSource = Nothing
            RadioButton2.Checked = False
            ISIDID.Properties.DataSource = Nothing
            ISIDID.EditValue = -1
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PRM(0).Value = BranchID.EditValue
            LoadToControlar(ISIDID, "TransCancelRequestTb_GetExISID", "Code", "ID", PRM)
        End If
    End Sub

    Private Sub BranchID_EditValueChanged(sender As Object, e As EventArgs) Handles BranchID.EditValueChanged
        ISIDID.Properties.DataSource = Nothing
        ISIDID.EditValue = -1
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        ReasonID.EditValue = -1
        Notes.Text = ""
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked = True Then
            ISIDID.Properties.DataSource = Nothing
            RadioButton1.Checked = False
            ISIDID.Properties.DataSource = Nothing
            ISIDID.EditValue = -1
            Dim PRM(0) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
            PRM(0).Value = BranchID.EditValue
            LoadToControlar(ISIDID, "ExternalEx_GetRequestByIDForCancel", "Code", "ID", PRM)
        End If
    End Sub
    Private Sub FrmCancelRequest_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            MyBase.Close()
        End If
    End Sub
    Sub Whats()
#Region "رسالة واتس للمرسل بتبليغ الالغاء"
        Dim DT As New DataTable
        Dim SendName As String
        Dim Price As Decimal
        Dim Phone As String
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = ISIDID.Text
        DT.Clear()
        DT = RUN_QUARY_PRO("InternalEx_SearchForCurrentRecorde", PRM)
        If DT.Rows.Count > 0 Then
            SendName = DT.Rows(0)("SenderName").ToString()
            Phone = DT.Rows(0)("SPhone1").ToString()
            Price = DT.Rows(0)("OverallVal")
        End If
        RPhone_get_forWatsab_and_CoBranch_Mobile(ISIDID.Text, BID)
        Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "طلب الغاء حوالة محلية" & vbNewLine & "CODE " & ":" & Space(1) & ISIDID.Text & vbNewLine &
              "باسم" & Space(1) & ":" & Space(1) & SendName & vbNewLine & "القيمة" & Space(1) & ":" & Space(1) & Cur_Code("دينار ليبي", Price, True, False) & vbNewLine & "الرجاء انتظار موافقة الادارة" & vbNewLine &
              "للاستفسار هــ" & Space(1) & ":" & Space(1) & sql_Mobile1 & vbNewLine & "شكراَ لتعاملكم معنا"
        WATSAPPMsAG(Phone, mms, True)


#End Region
    End Sub
End Class