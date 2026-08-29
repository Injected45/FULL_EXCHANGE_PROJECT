Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSEMPWITHDRAWAL
    Public Sub EMPORCUSTWITHDRAWALTB_MaxID(TypeID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("EMPORCUSTWITHDRAWALTB_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMEMPWITHDRAWAL.WDCode.Text = dt.Rows(0)("Code")
            FRMEMPWITHDRAWAL.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_Insert(ByVal Code As String, ByVal AccParent As ULong, ByVal WDVAL As Double, ByVal SafeID As Integer,
                                            TypeID As Int32, CODEID As ULong, BranchID As Integer, DPSVAL As Decimal, Notes As String, IsUpdate As Boolean, AccIDFrom As ULong,
                                              CurrencyFrom As Integer, PaidFor As String, Phone As String, IDNo As String)
        'Try
        Dim PRM(16) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccParent}
        PRM(2) = New SqlParameter("@WDVAL", SqlDbType.Decimal) With {.Value = WDVAL}
        PRM(3) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(4) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        PRM(5) = New SqlParameter("@CODEID", SqlDbType.BigInt) With {.Value = CODEID}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@DPSVAL", SqlDbType.Decimal) With {.Value = DPSVAL}
        PRM(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(9) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(10) = New SqlParameter("@AccIDFrom", SqlDbType.BigInt) With {.Value = AccIDFrom}
        PRM(11) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom}
        PRM(12) = New SqlParameter("@MSG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(13) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(14) = New SqlParameter("@PaidFor", SqlDbType.NVarChar, -1) With {.Value = PaidFor}
        PRM(15) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = Phone}
        PRM(16) = New SqlParameter("@IDNo", SqlDbType.NVarChar, -1) With {.Value = IDNo}
        RUN_EXUTE_PRO("EMPORCUSTWITHDRAWALTB_Insert", PRM)
        Dim status As Integer = Convert.ToInt32(PRM(12).Value)
        Dim msg As String = Convert.ToString(PRM(13).Value)
        If status = 0 OrElse status = 2 Then
            ErrorMessage(FRMEMPWITHDRAWAL, "رسالة تنبيه", msg)
            If status = 2 Then
                EMPORCUSTWITHDRAWALTB_MaxID(FRMEMPWITHDRAWAL.LOADTYPE)
            End If
            Exit Sub
        End If
        FRMEMPWITHDRAWAL.Print()
        'كود_رسائل الواتساب لسندات الصرف
        If TypeID = 5 Then
            Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "CODE " & ":" & Space(1) & FRMEMPWITHDRAWAL.WDCode.Text & vbNewLine & "تم سحب مبلغ" & Space(1) & ":" & Space(1) &
                    Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, True, False) & vbNewLine
            mms &= "من حساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text & vbNewLine & "لصالح" & Space(1) & "/" & Space(1) & PaidFor & vbNewLine & "هـ / " & Phone & vbNewLine & "رصيدكم الحالي هو" & vbNewLine & GetLKPColumnVal(FRMEMPWITHDRAWAL.WithdrawalFrom, "GetAccVal") - FRMEMPWITHDRAWAL.WDValue.EditValue & Space(1) & Cur_Code1(FRMEMPWITHDRAWAL.CurrencyFrom.Text) & vbNewLine & "شكرا لتعاملكم معنا"
            WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue), mms, True)
            'كود_رسائل الواتساب لسندات القبض
        ElseIf TypeID = 7 Then
            Dim mms As String = "شركة الرحالة للصرافة" & vbNewLine & "CODE " & ":" & Space(1) & Code & vbNewLine & "دخول مبلغ " & Cur_Code(FRMEMPWITHDRAWAL.CurrencyFrom.Text, FRMEMPWITHDRAWAL.WDValue.EditValue, True, False) &
                        vbNewLine
            mms &= "لحساب" & Space(1) & ":" & Space(1) & FRMEMPWITHDRAWAL.WithdrawalFrom.Text
            WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(FRMEMPWITHDRAWAL.WithdrawalFrom.EditValue), mms, True)
        End If
        FrmSavedSuccessfully.Show()
        FRMEMPWITHDRAWAL.NEWRECORD()
        'Catch ex As Exception
        'MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
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
