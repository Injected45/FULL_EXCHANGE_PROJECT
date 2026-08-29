Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSEMPWITHDRAWALNEW
    Public Sub EMPORCUSTWITHDRAWALTB_MaxID(TypeID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMEMPWITHDRAWALNEW.WDCode.Text = dt.Rows(0)("Code")
            FRMEMPWITHDRAWALNEW.IDCode = dt.Rows(0)("ID")
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
            RUN_EXUTE_PRO("EMPORCUSTWITHDRAWALTB_Insert", PRM)
            If PRM(18).Value = 0 Then
                ErrorMessage(FRMEMPWITHDRAWALNEW, "رسالة تنبيه", PRM(19).Value)
                EMPORCUSTWITHDRAWALTB_MaxID(5)
                Exit Sub
            End If
            Dim customIcon2 As New Icon(Application.StartupPath & "\Graphicloads-100-Flat-Information.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Information) = customIcon2
            Dim lookAndFeelError2 As New UserLookAndFeel(Me)
            lookAndFeelError2.Style = LookAndFeelStyle.Skin
            lookAndFeelError2.UseDefaultLookAndFeel = False
            lookAndFeelError2.SetSkinStyle(SkinStyle.Metropolis)
            XtraMessageBox.AllowCustomLookAndFeel = True
            'Dim result = XtraMessageBox.Show(lookAndFeelError2, "هل تريد طباعة التقرير ؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            'If result = DialogResult.Yes Then
            FRMEMPWITHDRAWALNEW.Print()
            'End If

            If TypeID = 5 Or TypeID = 6 Or TypeID = 29 Or TypeID = 33 Or TypeID = 64 Then

                Dim PR(2) As SqlParameter
                PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue}
                PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = FRMEMPWITHDRAWAL.CurrencyFrom.EditValue}
                PR(2) = New SqlParameter("@IsBank", SqlDbType.SmallInt) With {.Value = 2}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_FUNCTION_PARM("[Account_GetAccVal](@AccID,@CurrencyID,@IsBank) AS GetAccVal", PR)
                If dt.Rows.Count > 0 Then
                    'Dim raseed As ULong = dt.Rows(0)("GetAccVal")
                End If
                Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "CODE " & ":" & Space(1) & FRMEMPWITHDRAWAL.WDCode.Text & vbNewLine & "تم سحب مبلغ" & Space(1) & ":" & Space(1) &
                    Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, True, "n2") & vbNewLine &
            Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, False, "n2") & vbNewLine

                Select Case TypeID
                    Case 5
                        mms &= "من حساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text & vbNewLine & "رصيدكم الحالي هو" & vbNewLine & Format(dt.Rows(0)("GetAccVal"), "n2") & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
                    Case 6
                        mms &= "من حساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "رصيدكم الحالي هو" & vbNewLine & Format(dt.Rows(0)("GetAccVal"), "n2") & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
                    Case 29
                        mms &= "من حساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "رصيدكم الحالي هو" & vbNewLine & Format(dt.Rows(0)("GetAccVal"), "n2") & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
                    Case 33
                        mms &= "من حساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "رصيدكم الحالي هو" & vbNewLine & Format(dt.Rows(0)("GetAccVal"), "n2") & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
                    Case 64
                        mms &= "من " & FRMEMPWITHDRAWAL.WithdrawalFrom.Text & vbNewLine & "رصيدكم الحالي هو" & vbNewLine & Format(dt.Rows(0)("GetAccVal"), "n2") & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
                End Select
                WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue), mms, True)
                'كود_رسائل الواتساب لسندات القبض
            ElseIf TypeID = 7 Or TypeID = 8 Or TypeID = 28 Or TypeID = 32 Or TypeID = 63 Then
                Dim PR(2) As SqlParameter
                PR(0) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue}
                PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = FRMEMPWITHDRAWAL.CurrencyFrom.EditValue}
                PR(2) = New SqlParameter("@IsBank", SqlDbType.TinyInt) With {.Value = 2}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_FUNCTION_PARM("[Account_GetAccVal](@AccID,@CurrencyID,@IsBank) AS GetAccVal", PR)
                If dt.Rows.Count > 0 Then
                    'Dim raseed As ULong = dt.Rows(0)("GetAccVal")
                End If
                Dim mms As String = "شركة الرحالة للصرافة" & vbNewLine & "CODE " & ":" & Space(1) & Code & vbNewLine & "دخول مبلغ " & Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, True, "n2") &
                vbNewLine &
            Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, False, "n2") & vbNewLine
                Select Case TypeID
                    Case 7
                        mms &= "لحساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
                    Case 8
                        mms &= "لحساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
                    Case 28
                        mms &= "لحساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
                    Case 32
                        mms &= "لحساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
                    Case 63
                        mms &= " ل" & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
                End Select


                Select Case TypeID
                    Case 8, 28, 32
                        mms &= vbNewLine & "المودع / " & PaidFor & vbNewLine & "هـ / " & Phone
                End Select
                mms &= vbNewLine & "رصيدكم الحالي هو" & vbNewLine & Format(dt.Rows(0)("GetAccVal"), "n2") & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
                ' Send the WhatsApp message
                WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue), mms, True)
            End If

            FrmSavedSuccessfully.Show()
            FRMEMPWITHDRAWAL.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Function SERACH_EMPORCUSTWITHDRAWALTB(Code As String, TypeID As Int32) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_SelectByCode", PRM)
        Return DT
    End Function
End Class
