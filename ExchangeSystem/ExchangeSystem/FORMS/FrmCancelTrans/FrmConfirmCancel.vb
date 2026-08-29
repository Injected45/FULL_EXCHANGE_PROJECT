Imports System.Data.SqlClient
Imports DevExpress.XtraReports.UI

Public Class FrmConfirmCancel
    Public RBID, DBID, RBTYPE, DBRTYPE As Integer
    Public ISIDCode As String
    Public IsConfirmed, ConfirmedType As Boolean
    Sub NEWRECORD()
        InsertDate.ReadOnly = True
        CodeID.ReadOnly = True
        ISIDID.ReadOnly = True
        ReasonID.ReadOnly = True
        RDG.ReadOnly = True
        Notes.ReadOnly = True
        LOADREASONS()
    End Sub
    Public Sub LoadData()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1)
        PRM(0).Value = FrmViewCanceledTransfer.GVRole.GetFocusedRowCellValue("Code")
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TransCancelRequestTb_GetToConfirmOrCancel", PRM)
        If DT.Rows.Count > 0 Then
            CodeID.Text = DT.Rows(0)("ID")
            InsertDate.EditValue = DT.Rows(0)("InsertDate")
            ReasonID.EditValue = DT.Rows(0)("ReasonID")
            ISIDID.Text = Convert.ToString(DT.Rows(0)("ISID")).ToString
            Notes.Text = DT.Rows(0)("Notes").ToString
            If DT.Rows(0)("ISIDTYPE") = 1 Then
                RDG.SelectedIndex = 0
            ElseIf DT.Rows(0)("ISIDTYPE") = 2 Then
                RDG.SelectedIndex = 1
            End If
            RBID = DT.Rows(0)("BranchID")
        End If
        DT.Dispose()
    End Sub
    Public Sub LoadDataConfirmCancel()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ISID", SqlDbType.NVarChar, -1)
        PRM(0).Value = FRMCONFIRMISSUED.isisdcode

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("TransCancelRequestTb_GetToConfirmOrCancel", PRM)
        If DT.Rows.Count > 0 Then
            CodeID.Text = DT.Rows(0)("ID")
            InsertDate.EditValue = DT.Rows(0)("InsertDate")
            ReasonID.EditValue = DT.Rows(0)("ReasonID")
            ISIDID.Text = Convert.ToString(DT.Rows(0)("ISID")).ToString
            Notes.Text = DT.Rows(0)("Notes").ToString
            If DT.Rows(0)("ISIDTYPE") = 1 Then
                RDG.SelectedIndex = 0
            ElseIf DT.Rows(0)("ISIDTYPE") = 2 Then
                RDG.SelectedIndex = 1
            End If
            RBID = DT.Rows(0)("BranchID")
        End If
        DT.Dispose()
    End Sub
    Sub LOADREASONS()
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("TransCancelRequestTb_GetReasonID")
        If DT.Rows.Count > 0 Then
            ReasonID.Properties.DataSource = DT
            ReasonID.Properties.ValueMember = "ID"
            ReasonID.Properties.DisplayMember = "NewCause"
            ReasonID.Properties.PopulateColumns()
            ReasonID.Properties.Columns("ID").Visible = False
        End If
        DT.Dispose()
    End Sub

    Private Sub FrmConfirmCancel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim DT As New DataTable
        DT = RUN_QUARY_TXT("select BranchType from CoBranch where ID='" & RBID & "'")
        RBTYPE = DT.Rows(0)("BranchType")
        If RBTYPE <> 3 And DBID <> 2 Or DBID <> 1 Then
            If IsConfirmed = False Then
                RUN_EXUTE_TXT("update InternalEx set IsCanceled=2, IsConfirmed=3,ConfirmCanceled=1,ConfirmCancelBranch=" & RBID & "  where Code='" & ISIDID.Text & "'")
                RUN_EXUTE_TXT("update TransCancelRequestTb set IsConfirmCancel=4,IsCanceledRequest=3 where ISID='" & ISIDID.Text & "'")
            End If
            If IsConfirmed = True Then
                RUN_EXUTE_TXT("update InternalEx set IsCanceled=2, IsConfirmed=3,ConfirmCanceled=1,ConfirmCancelBranch=" & RBID & "  where Code='" & ISIDID.Text & "'")
                RUN_EXUTE_TXT("update TransCancelRequestTb set IsConfirmCancel=4,IsCanceledRequest=3 where ISID='" & ISIDID.Text & "'")
            End If
        End If
        If ISIDID.Text <> String.Empty Then
            SEndForCanselMassg(0, ISIDID.Text)
            SEndForCanselMassg(1, ISIDID.Text)
        End If


        CONFIRMMESSAGE.LBLTEXT.Text = "تم اعتماد الموافقة على الطلب بنجاح"
        CONFIRMMESSAGE.ShowDialog()
        Me.Dispose()
        FrmViewCanceledTransfer.GCRole.DataSource = Nothing
        FrmViewCanceledTransfer.LOADDATA()
        FRMCONFIRMISSUED.ConfirmCancelRequest = True
        FRMCONFIRMISSUED.DiscountCancel = True
        'FRMCONFIRMISSUED.RefreshDVG()
        FRMCONFIRMISSUED.LOADDATA()
        'FRMCONFIRMISSUED.LOADFORCANCEL()
        refresh_table(BID)
        refresh_table(MAINBID)
        DT.Dispose()
    End Sub

    Public Sub SEndForCanselMassg(type_send As Integer, ISID As String)
        Dim dt As New DataTable

        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = ISID
        dt.Clear()
        dt = RUN_QUARY_PRO("GET_colmens_InternalEx", PRM)
        If dt.Rows.Count > 0 Then




            ' sql_RPhone
            If type_send = 0 Then
                RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, dt.Rows(0)("BranchDeliveredID"))
                Dim messg As String = My.Settings.Combny_name & vbNewLine
                messg &= "تم إلغاء الحوالة المحلية" & vbNewLine &
                    "CODE : " & ISID & vbNewLine &
                    "بإسم : " & dt.Rows(0)("RecievedName").ToString() & vbNewLine &
                "القيمة : " & dt.Rows(0)("OverallVal") & vbNewLine &
                "المرسله اليكم من  : " & dt.Rows(0)("SenderName") & vbNewLine &
                " للإستفسار هـ : " & sql_Mobile1 & vbNewLine &
                "شكراً لتعاملكم معنا"
                WATSAPPMsAG(dt.Rows(0)("RPhone1"), messg, False)

            Else
                RPhone_get_forWatsab_and_CoBranch_Mobile(ISID, dt.Rows(0)("BranchDeliveredID"))
                Dim messg As String = My.Settings.Combny_name & vbNewLine
                messg &= "تمت الموافقه علي طلب" & vbNewLine &
                    "إلغاء الحوالة المحلية" & vbNewLine &
                    "CODE : " & ISID & vbNewLine &
                    "بإسم : " & dt.Rows(0)("RecievedName") & vbNewLine &
                "القيمة : " & dt.Rows(0)("OverallVal") & vbNewLine &
              "نرجوك التفضل بالإستلام" & vbNewLine &
                " للإستفسار هـ : " & sql_Mobile1 & vbNewLine &
                "شكراً لتعاملكم معنا"
                WATSAPPMsAG(dt.Rows(0)("SPhone1"), messg, False)

            End If
        End If



    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If ConfirmedType = True Then

            If BID = MAINBID Then
                RUN_EXUTE_TXT("update InternalEx set ConfirmCancelBranch=" & BID & ", IsCanceled=0,IsConfirmed=1,IsDelivered=0,Refusecanceled=1,ConfirmCanceled=0 where Code='" & ISIDID.Text & "'")
                RUN_EXUTE_TXT("delete from TransCancelRequestTb  where ISID='" & ISIDID.Text & "'")
            ElseIf BID <> MAINBID Then
                RUN_EXUTE_TXT("update InternalEx set ConfirmCancelBranch=" & BID & ", IsCanceled=9,IsConfirmed=6,Refusecanceled=1,IsDelivered=0,ConfirmCanceled=3 where Code='" & ISIDID.Text & "'")
                RUN_EXUTE_TXT("update TransCancelRequestTb set IsConfirmCancel=6, IsCanceledRequest=5 where ISID='" & ISIDID.Text & "'")
                FrmViewCanceledTransfer.GCRole.DataSource = Nothing
                FrmViewCanceledTransfer.LOADDATA()
                FrmInternalExDeliveredAfterConfirmCancel.ShowDialog()
                Me.Close()
            Else
                FrmViewCanceledTransfer.GCRole.DataSource = Nothing
                FrmViewCanceledTransfer.LOADDATA()
                FrmInternalExDeliveredAfterConfirmCancel.ShowDialog()
                Me.Close()
            End If
        ElseIf ConfirmedType = False Then
            RUN_EXUTE_TXT("update InternalEx set ConfirmCancelBranch=" & FRMCONFIRMISSUED.GVROLE.GetFocusedRowCellValue("BranchDeliveredID") & ", IsCanceled=0,IsConfirmed=1,IsDelivered=0,ConfirmCanceled=0 where Code='" & ISIDID.Text & "'")
            RUN_EXUTE_TXT("delete from TransCancelRequestTb  where ISID='" & ISIDID.Text & "'")
            FrmRemoveMessage.LBLTEXT.Text = "تم إلغاء الطلب بنجاح"
            FrmRemoveMessage.ShowDialog()
            Me.Close()
        End If
        Me.Close()
        FRMCONFIRMISSUED.ConfirmCancelRequest = False
        FRMCONFIRMISSUED.DiscountCancel = False
        FRMCONFIRMISSUED.LOADDATA()
        'FRMCONFIRMISSUED.LOADFORCANCEL()
        refresh_table(BID)
        refresh_table(MAINBID)

    End Sub


End Class