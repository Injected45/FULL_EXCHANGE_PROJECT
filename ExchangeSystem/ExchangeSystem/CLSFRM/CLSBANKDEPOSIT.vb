Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSBANKDEPOSIT
    Public Sub BankDipWdTb_MaxID(TypeID As Integer, BranchID As Integer)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.TinyInt) With {.Value = BranchID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("BankDipWdTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMBANKDEPOSIT.CodeID.Text = dt.Rows(0)("Code")
            FRMBANKDEPOSIT.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub BANKDEPOSIT_Insert(ByVal Code As String, ByVal InsertDate As Date, ByVal BranchID As Integer, ByVal SafeAccID As ULong, ByVal AccFrom As ULong,
                                            AccTo As ULong, CODEID As ULong, BillVal As Decimal, IsDiscount As Boolean, DiscountFrom As Int32, DiscountType As Int32, OverAllTotal As Decimal,
                                            DiscountVal As Decimal, BillNo As String, Notes As String, IsActive As Boolean, IsUpdate As Boolean, TypeID As Integer, OperationTypeID As Integer,
                                            CurrencyID As Integer, MovementType2 As String, UserID As Integer, GetAccountName As String, DiscountAccID As ULong, TxtName As String, TxtPhone As String,
                                  EXID As Integer, AccParent As ULong)
        Try
            Dim PRM(29) As SqlParameter
            PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(3) = New SqlParameter("@SafeAccID", SqlDbType.BigInt) With {.Value = SafeAccID}
            PRM(4) = New SqlParameter("@AccFrom", SqlDbType.BigInt) With {.Value = AccFrom}
            PRM(5) = New SqlParameter("@AccTo", SqlDbType.BigInt) With {.Value = AccTo}
            PRM(6) = New SqlParameter("@CODEID", SqlDbType.BigInt) With {.Value = CODEID}
            PRM(7) = New SqlParameter("@BillVal", SqlDbType.Decimal) With {.Value = BillVal}
            PRM(8) = New SqlParameter("@IsDiscount", SqlDbType.Bit) With {.Value = IsDiscount}
            PRM(9) = New SqlParameter("@DiscountFrom", SqlDbType.TinyInt) With {.Value = DiscountFrom}
            PRM(10) = New SqlParameter("@DiscountType", SqlDbType.TinyInt) With {.Value = DiscountType}
            PRM(11) = New SqlParameter("@OverAllTotal", SqlDbType.Decimal) With {.Value = OverAllTotal}
            PRM(12) = New SqlParameter("@DiscountVal", SqlDbType.Decimal) With {.Value = DiscountVal}
            PRM(13) = New SqlParameter("@BillNo", SqlDbType.NVarChar, 50) With {.Value = BillNo}
            PRM(14) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(15) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
            PRM(16) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(17) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
            PRM(18) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
            PRM(19) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            PRM(20) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = MovementType2}
            PRM(21) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            PRM(22) = New SqlParameter("@GetAccountName", SqlDbType.NVarChar, -1) With {.Value = GetAccountName}
            PRM(23) = New SqlParameter("@DiscountAccID", SqlDbType.BigInt) With {.Value = DiscountAccID}
            PRM(24) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(25) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(26) = New SqlParameter("@OwnAccountName", SqlDbType.NVarChar, 50) With {.Value = TxtName}
            PRM(27) = New SqlParameter("@OwnAccountPhone", SqlDbType.NVarChar, 50) With {.Value = TxtPhone}
            PRM(28) = New SqlParameter("@EXID", SqlDbType.Int) With {.Value = EXID}
            PRM(29) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccParent}
            RUN_EXUTE_PRO("BankDipWdTb_Insert", PRM)
            If PRM(24).Value = 0 Then
                ErrorMessage(FRMBANKDEPOSIT, "رسالة تنبيه", PRM(25).Value)
                If FRMBANKDEPOSIT.IsUpdate = False Then
                    BankDipWdTb_MaxID(FRMBANKDEPOSIT.LOADTYPE, FRMBANKDEPOSIT.BranchID.EditValue)
                    Exit Sub
                End If

            End If
            If FRMBANKDEPOSIT.LOADTYPE = 16 Then
                Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "تم إيداع مصرفي مبلغ " & Space(1) & ":" & Space(1) &
              Cur_Code(FRMBANKDEPOSIT.CURRENCYID.Text, OverAllTotal, True, "n2") & vbNewLine &
              Cur_Code(FRMBANKDEPOSIT.CURRENCYID.Text, OverAllTotal, False, "n2") & vbNewLine &
              "رقم المعاملة" & Space(1) & ":" & Space(1) & BillNo & vbNewLine & "من حساب" &
              Space(1) & ":" & Space(1) & FRMBANKDEPOSIT.TxtName.Text &
           vbNewLine & "رقم الحساب" & Space(1) & ":" & Space(1) & TxtPhone & vbNewLine & "شكرا لتعاونكم معنا"
                WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(AccFrom), mms, True)
            Else
                Dim mms As String = "شركة الرحالة للصرافة " & vbNewLine & "تم سحب مصرفي مبلغ " & Space(1) & ":" & Space(1) &
             Cur_Code(FRMBANKDEPOSIT.CURRENCYID.Text, OverAllTotal, True, "n2") & vbNewLine &
                Cur_Code(FRMBANKDEPOSIT.CURRENCYID.Text, OverAllTotal, False, "n2") & vbNewLine &
             "رقم المعاملة" & Space(1) & ":" & Space(1) & BillNo & vbNewLine & "الي حساب" & Space(1) & ":" & Space(1) & FRMBANKDEPOSIT.TxtName.Text &
          vbNewLine & "رقم الحساب" & Space(1) & ":" & Space(1) & TxtPhone & vbNewLine & "شكرا لتعاونكم معنا"
                WATSAPPMsAG(GET_PHONE_SaenFroWtsaap(AccTo), mms, True)
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
                FRMBANKDEPOSIT.Print()
                FRMBANKDEPOSIT.NEWRECORD()
                FrmSavedSuccessfully.Show()
            Else
                FRMBANKDEPOSIT.NEWRECORD()
                FrmSavedSuccessfully.Show()
            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try


    End Sub
    Public Function BankDipWdTb_SelectByCode(Code As String, TypeID As Int32) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("BankDipWdTb_SelectByCode", PRM)
        Return DT
    End Function
End Class
