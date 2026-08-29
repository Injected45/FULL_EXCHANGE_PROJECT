Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSEMPADVANCEPAYMENT
    Public Sub EMPDIS_MaxID(BranchID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@OpTypID", SqlDbType.Int) With {.Value = 10}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("AdvancePaymentTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMEMPADVANCEPAYMENT.CodeID.Text = dt.Rows(0)("Code")
        End If
    End Sub
    Public Sub INSERTTB_EMPDIS(Code As String, InsertDate As Date, EMPID As Integer, BranchID As Integer, OverAllVal As Decimal, RepaymentPeroid As Int32, ValPerMonth As Decimal, SafeID As Integer,
                               CurrencyID As Integer, Notes As String, AccIDFrom As ULong, AccIDTo As ULong, MovementType As String, IsUpdate As Boolean)
        Try
            Dim PRM(15) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(2) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            PRM(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(4) = New SqlParameter("@OverAllVal", SqlDbType.Decimal) With {.Value = OverAllVal}
            PRM(5) = New SqlParameter("@RepaymentPeroid", SqlDbType.TinyInt) With {.Value = RepaymentPeroid}
            PRM(6) = New SqlParameter("@ValPerMonth", SqlDbType.Decimal) With {.Value = ValPerMonth}
            PRM(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            PRM(8) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            PRM(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(10) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = AccIDFrom}
            PRM(11) = New SqlParameter("@AccIDTo", SqlDbType.BigInt) With {.Value = AccIDTo}
            PRM(12) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
            PRM(13) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(14) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(15) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("AdvancePaymentTb_Insert", PRM)
            If PRM(14).Value = 0 Then
                ErrorMessage(FRMEMPADVANCEPAYMENT, "رسالة تنبيه", PRM(15).Value)
                EMPDIS_MaxID(FRMEMPADVANCEPAYMENT.BRANCHID.EditValue)
                Exit Sub
            End If
            If IsUpdate = 0 Then
                Dim mms As String = "*شركة الرحالة القابضة*" & vbNewLine &
                    "نفيدكم بأنه تم منحكم سلفة مالية  " & vbNewLine & "بقيمة" & ":" & Space(1) & OverAllVal & Space(1) & Cur_Code1(FRMEMPADVANCEPAYMENT.CURRENCYID.Text) & vbNewLine &
                    "اسم الموظف " & ":" & Space(1) & FRMEMPADVANCEPAYMENT.EMPID.Text & vbNewLine &
                    "الرقم الوظيفي" & ":" & Space(1) & GET_EMPcodefor_Acount_SaenFroWtsaap(EMPID) & vbNewLine &
                    "وسيتم استقطاع مبلغ" & ":" & Space(1) & ValPerMonth & Space(1) & Cur_Code1(FRMEMPADVANCEPAYMENT.CURRENCYID.Text) & Space(1) & "شهريًا" & vbNewLine & "لمدة" & ":" & Space(1) & RepaymentPeroid & Space(1) & "شهر" & vbNewLine &
                    "مع تحيات قسم الشؤون الإدارية"
                WATSAPPMsAG(GET_EMPPHONE_SaenFroWtsaap(EMPID), mms, True)

            End If
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If result = DialogResult.Yes Then
                FRMEMPADVANCEPAYMENT.Print()
            End If
            FrmSavedSuccessfully.Show()
            FRMEMPADVANCEPAYMENT.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

End Class
