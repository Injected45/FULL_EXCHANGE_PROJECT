Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSEMINCREASE
    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub SEARCH_EMPDIS(InsertDate As Date, EMPID As Integer, DiscountTypeID As Integer, DISVAL As Decimal, Code As String, IsActive As Boolean)
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        PRM(2) = New SqlParameter("@IncreaseTypeID", SqlDbType.Int) With {.Value = DiscountTypeID}
        PRM(3) = New SqlParameter("@INCVAL", SqlDbType.Decimal) With {.Value = DISVAL}
        PRM(4) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(5) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        RUN_EXUTE_PRO("IncreaseValTb_Select", PRM)
    End Sub
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB_EMPDIS(InsertDate As Date, EMPID As Integer, DiscountTypeID As Integer, DISVAL As Decimal, Code As String, IsConstant As Boolean, Notes As String)
        Try
            Dim PRM(8) As SqlParameter
            PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            PRM(2) = New SqlParameter("@IncreaseTypeID", SqlDbType.Int) With {.Value = DiscountTypeID}
            PRM(3) = New SqlParameter("@INCVAL", SqlDbType.Decimal) With {.Value = DISVAL}
            PRM(4) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(5) = New SqlParameter("@IsConstant", SqlDbType.Bit) With {.Value = IsConstant}
            PRM(6) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(7) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(8) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("IncreaseValTb_Insert", PRM)
            If PRM(7).Value = 0 Then
                ErrorMessage(FRMEMPADDINCREASE, "رسالة تنبيه", PRM(8).Value)
                EMPDIS_MaxID(FRMEMPADDINCREASE.BRANCHID.EditValue)
                Exit Sub
            End If
            ' Dim mms As String = " *شركة الرحالة القابضة*" & vbNewLine &
            ' "الاسم " & ":" & Space(1) & FRMEMPADDINCREASE.EMPID.Text & vbNewLine &
            ' "الرقم الوظيفي" & ":" & Space(1) & GET_EMPcodefor_Acount_SaenFroWtsaap(EMPID) & vbNewLine &
            ' "يسرنا اعلامكم بمنحكم " & vbNewLine & "علاوة" & ":" & Space(1) & FRMEMPADDINCREASE.DISTYPEID.Text & vbNewLine &
            ' "بقيمة" & ":" & Space(1) & DISVAL & Cur_Code1(FRMEMPADDINCREASE.CURRENCYID.Text) & vbNewLine &
            '"تقديرًا لجهودكم 
            ' واصل العطاء فأنتم محل ثقة وفخر"

            ' WATSAPPMsAG(GET_EMPPHONE_SaenFroWtsaap(FRMEMPADDINCREASE.EMPID.EditValue), mms)

            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                FRMEMPADDINCREASE.Print()
            End If
            FrmSavedSuccessfully.Show()
            FRMEMPADDINCREASE.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_EMP(InsertDate As Date, EMPID As Integer, DiscountTypeID As Integer, DISVAL As Decimal, Code As String)
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        PRM(2) = New SqlParameter("@IncreaseTypeID", SqlDbType.Int) With {.Value = DiscountTypeID}
        PRM(3) = New SqlParameter("@INCVAL", SqlDbType.Decimal) With {.Value = DISVAL}
        PRM(4) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        RUN_EXUTE_PRO("IncreaseValTb_Update", PRM)
    End Sub
    Public Sub EMPDIS_MaxID(BranchID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@OpTypID", SqlDbType.Int) With {.Value = 36}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("IncreaseValTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMEMPADDINCREASE.CodeID.Text = dt.Rows(0)("Code")
            'FRMINTERNALTRANSFER.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(ByVal SafeID As Integer, ByVal Debit As Decimal, ByVal Credit As Decimal, ByVal InsertDate As Date, ByVal Code As String, TypeID As Int32, OperationTypeID As Integer,
                                        AccBranchID As Integer, AccIDFrom As Integer, AccIDTo As Integer, MovementType As String)
        Dim PRM(10) As SqlParameter
        PRM(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(1) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
        PRM(2) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
        PRM(3) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(4) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(5) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        PRM(6) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        PRM(7) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
        PRM(8) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = AccIDFrom}
        PRM(9) = New SqlParameter("@AccIDTo", SqlDbType.Int) With {.Value = AccIDTo}
        PRM(10) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
        RUN_EXUTE_PRO("AccSafeActivityTb_IncreaseValTbInsert", PRM)
    End Sub
    Public Sub EMPDIS_DELETE(Code As String, EMPID As Integer, INCVAL As Decimal)
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        PRM(2) = New SqlParameter("@INCVAL", SqlDbType.Decimal) With {.Value = INCVAL}
        RUN_EXUTE_PRO("IncreaseValTb_Delete", PRM)
    End Sub
    Public Sub EMPDIS_DELETEBYEMPID(EMPID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        RUN_EXUTE_PRO("IncreaseValTb_DeleteByEMPID", PRM)
    End Sub
End Class
