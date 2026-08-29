Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSEMPDISCOUNT
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub DELETE_EMP(Code As String)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.BigInt) With {.Value = Code}
        RUN_EXUTE_PRO("DiscountValTb_Delete", PRM)

    End Sub

    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub SEARCH_EMPDIS(InsertDate As Date, EMPID As Integer, DiscountTypeID As Integer, DISVAL As Decimal, ID As ULong, IsActive As Boolean)
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
        PRM(2) = New SqlParameter("@DiscountTypeID", SqlDbType.Int) With {.Value = DiscountTypeID}
        PRM(3) = New SqlParameter("@DISVAL", SqlDbType.Decimal) With {.Value = DISVAL}
        PRM(4) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = ID}
        PRM(5) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        RUN_EXUTE_PRO("DiscountValTb_Select", PRM)
    End Sub
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB_EMPDIS(InsertDate As Date, EMPID As Integer, DiscountTypeID As Integer, DISVAL As Decimal, SafeID As Integer, Code As String, AccBranchID As Integer, CurrencyID As Integer,
                               Notes As String, IsUpdate As Boolean)
        Try
            Dim PRM(11) As SqlParameter
            PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            PRM(2) = New SqlParameter("@DiscountTypeID", SqlDbType.Int) With {.Value = DiscountTypeID}
            PRM(3) = New SqlParameter("@DISVAL", SqlDbType.Decimal) With {.Value = DISVAL}
            PRM(4) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            PRM(5) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(6) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
            PRM(7) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            PRM(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(9) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(10) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(11) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("DiscountValTb_Insert", PRM)
            If PRM(10).Value = 0 Then
                ErrorMessage(FRMEMPDISCOUNT, "رسالة تنبيه", PRM(11).Value)
                EMPDIS_MaxID(FRMEMPDISCOUNT.BRANCHID.EditValue)
                Exit Sub
            End If
            '  If IsUpdate = 0 Then
            '      Dim message As String

            '      message = " *شركة الرحالة القابضة*" & vbNewLine & "الاسم" & ":" & Space(1) & FRMEMPDISCOUNT.EMPID.Text & vbNewLine &
            '"الرقم الوظيفي" & ":" & Space(1) & GET_EMPcodefor_Acount_SaenFroWtsaap(EMPID) & vbNewLine &
            '"نُحيطكم علمًا بأنه تم خصم" & ":" & Space(1) & DISVAL & Cur_Code1(FRMEMPDISCOUNT.CURRENCYID.Text) & vbNewLine &
            '"بسبب" & ":" & Space(1) & FRMEMPDISCOUNT.DISTYPEID.Text & vbNewLine &
            '"وذلك حسب لوائح الشركة" & vbNewLine &
            '"نأمل منكم المزيد من الالتزام والتفاني في العمل"

            ''      WATSAPPMsAG(GET_EMPPHONE_SaenFroWtsaap(EMPID), message)
            'End If
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                FRMEMPDISCOUNT.Print()
            End If
            FrmSavedSuccessfully.Show()
            FRMEMPDISCOUNT.NEWRECORD()
            FRMEMPDISCOUNT.DISVAL.EditValue = 0.000
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Sub EMPDIS_MaxID(BranchID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@OpTypID", SqlDbType.Int) With {.Value = 11}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("DiscountValTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMEMPDISCOUNT.CodeID.Text = dt.Rows(0)("Code")
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
        RUN_EXUTE_PRO("AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert", PRM)
    End Sub
End Class
