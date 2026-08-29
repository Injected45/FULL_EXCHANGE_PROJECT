Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSADINCOME
    Public Sub EMPORCUSTWITHDRAWALTB_MaxID(TypeID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("IncomeTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMADDINCOME.WDCode.Text = dt.Rows(0)("Code")
            FRMADDINCOME.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_Insert(ByVal Code As String, ByVal InsertDate As Date, ByVal EMPID As Integer, ByVal WDVAL As Double, ByVal SafeID As Integer,
                                            TypeID As Int32, CODEID As ULong, BranchID As Integer, DPSVAL As Decimal, Notes As String, IsUpdate As Boolean, OperationTypeID As Integer, AccIDFrom As ULong,
                                            AccIDTo As ULong, MovementType As String, MovementType2 As String, UserID As Integer, CurrencyFrom As Integer, PaidFor As String, Phone As String, IDNo As String)
        Try
            Dim PRM(22) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(2) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            PRM(3) = New SqlParameter("@WDVAL", SqlDbType.Decimal) With {.Value = WDVAL}
            PRM(4) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            PRM(5) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
            PRM(6) = New SqlParameter("@CODEID", SqlDbType.BigInt) With {.Value = CODEID}
            PRM(7) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(8) = New SqlParameter("@DPSVAL", SqlDbType.Decimal) With {.Value = DPSVAL}
            PRM(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(10) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(11) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
            PRM(12) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = AccIDFrom}
            PRM(13) = New SqlParameter("@AccIDTo", SqlDbType.BigInt) With {.Value = AccIDTo}
            PRM(14) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
            PRM(15) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = MovementType2}
            PRM(16) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PRM(17) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom}
            PRM(18) = New SqlParameter("@MSG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(19) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(20) = New SqlParameter("@PaidFor", SqlDbType.NVarChar, -1) With {.Value = PaidFor}
            PRM(21) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = Phone}
            PRM(22) = New SqlParameter("@IDNo", SqlDbType.NVarChar, -1) With {.Value = IDNo}
            RUN_EXUTE_PRO("Income_Insert", PRM)
            If PRM(18).Value = 0 Then
                MessageBox.Show(PRM(19).Value, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                EMPORCUSTWITHDRAWALTB_MaxID(FRMADDINCOME.LOADTYPE)
                Exit Sub
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
                FRMADDINCOME.Print()
            End If
            'كود_رسائل الواتساب لسندات الصرف
            'If TypeID = 5 Or TypeID = 6 Or TypeID = 29 Or TypeID = 33 Then
            '    Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "CODE " & ":" & Space(1) & FRMEMPWITHDRAWAL.WDCode.Text & vbNewLine & "تم سحب مبلغ" & Space(1) & ":" & Space(1) &
            '    Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, True, False) & vbNewLine
            '    Select Case TypeID
            '        Case 5
            '            mms &= "ح موظف رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap((FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue)) & vbNewLine & "شكراً لتعاملكم معنا"
            '        Case 6
            '            mms &= "ح العملاء رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue) & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "شكراً لتعاملكم معنا"
            '        Case 29
            '            mms &= "ح المدينون رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue) & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "شكراً لتعاملكم معنا"
            '        Case 33
            '            mms &= "ح وكلاء رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue) & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "شكراً لتعاملكم معنا"
            '    End Select
            '    WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue), mms)
            '    'كود_رسائل الواتساب لسندات القبض
            'ElseIf TypeID = 7 Or TypeID = 8 Or TypeID = 28 Or TypeID = 32 Then
            '    Dim mms As String = "شركة الرحالة للصرافة" & vbNewLine & "CODE " & ":" & Space(1) & Code & vbNewLine & "دخول مبلغ " & Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, True, False) &
            '    vbNewLine
            '    Select Case TypeID
            '        Case 7
            '            mms &= "ح موظف رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue) & vbNewLine & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
            '        Case 8
            '            mms &= "ح العملاء رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue)
            '        Case 28
            '            mms &= "ح المدينون رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue)
            '        Case 32
            '            mms &= "ح وكلاء رقم" & Space(1) & ":" & Space(1) & GET_codefor_Acount_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue)
            '    End Select
            '    Select Case TypeID
            '        Case 8, 28, 32
            '            mms &= vbNewLine & "المودع / " & PaidFor & vbNewLine & "هـ / " & Phone
            '    End Select
            '    mms &= vbNewLine & "شكراً لتعاملكم معنا"
            '    ' Send the WhatsApp message
            '    WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue), mms)
            'End If
            FrmSavedSuccessfully.Show()
            FRMADDINCOME.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Function SERACH_EMPORCUSTWITHDRAWALTB(Code As String, TypeID As Int32) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_SelectByCode", PRM)
        Return DT
    End Function
End Class
