Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSWITHDRAWALFORM

    Public Function LOAD_WITHDRAWALTODVG() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("WithdrawalTb_SEARCH")
        Return DT
    End Function
    Public Function SHOW_WITHDRAWAL_DATA(WDCode As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@WDCode", SqlDbType.NVarChar, -1) With {.Value = WDCode.ToString}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("WithdrawalTb_SEARCH", PRM)
        Return DT
    End Function
    Public Sub INSERTTB_WITHDRAWAL(WDCode As String, WithDrawalDate As Date, WithdrawalFrom As ULong, WithdrawalTo As ULong, WithdrawalValue As Decimal, Notes As String, BranchID As Integer,
                                   SafeID As Integer, CurrencyID As Integer, MovementType As String, MovementType2 As String, IsUpdate As Boolean, IDCode As ULong)
        Dim PRM(16) As SqlParameter
        PRM(0) = New SqlParameter("@WDCode", SqlDbType.NVarChar, -1) With {.Value = WDCode}
        PRM(1) = New SqlParameter("@WithDrawalDate", SqlDbType.Date) With {.Value = WithDrawalDate}
        PRM(2) = New SqlParameter("@WithdrawalFrom", SqlDbType.BigInt) With {.Value = WithdrawalFrom}
        PRM(3) = New SqlParameter("@WithdrawalTo", SqlDbType.BigInt) With {.Value = WithdrawalTo}
        PRM(4) = New SqlParameter("@WithdrawalValue", SqlDbType.Decimal) With {.Value = WithdrawalValue}
        PRM(5) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@SAFEID", SqlDbType.Int) With {.Value = SafeID}
        PRM(8) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(9) = New SqlParameter("@WIDCode", SqlDbType.BigInt) With {.Value = IDCode}
        PRM(10) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
        PRM(11) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = MovementType2}
        PRM(12) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(13) = New SqlParameter("@TypID", SqlDbType.Int) With {.Value = 14}
        PRM(14) = New SqlParameter("@DailyClose", SqlDbType.Bit) With {.Value = 0}
        PRM(15) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(16) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        'PRM(13) = New SqlParameter("@WIDCode", SqlDbType.Int) With {.Value = WIDCode}
        RUN_EXUTE_PRO("WithdrawalTb_Insert", PRM)


        If PRM(15).Value = 0 Then
            ErrorMessage(FrmSafeTransfer, "رسالة تنبيه", PRM(16).Value)
            If FrmSafeTransfer.IsUpdate = False Then
                WITHDRAWAL_MaxID(14, FrmSafeTransfer.BranchID.EditValue, UserID, COUNTRYNID, CITYID)
                Exit Sub
            End If

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
            FrmSafeTransfer.Print()
            'Else
            '    Exit Sub
        End If
        FrmSafeTransfer.NEWRECORD()
    End Sub
    Public Sub update_WITHDRAWAL(WDCode As String, WithDrawalDate As Date, WithdrawalFrom As Integer, WithdrawalTo As Integer, WithdrawalValue As Double, Notes As String, BranchID As Integer,
                                 SafeID As Integer, CurrencyID As Integer, WIDCode As ULong)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@WDCode", SqlDbType.NVarChar, -1) With {.Value = WDCode.ToString}
        PRM(1) = New SqlParameter("@WithDrawalDate", SqlDbType.Date) With {.Value = WithDrawalDate}
        PRM(2) = New SqlParameter("@WithdrawalFrom", SqlDbType.Int) With {.Value = WithdrawalFrom}
        PRM(3) = New SqlParameter("@WithdrawalTo", SqlDbType.Int) With {.Value = WithdrawalTo}
        PRM(4) = New SqlParameter("@WithdrawalValue", SqlDbType.Float) With {.Value = WithdrawalValue}
        PRM(5) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(8) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(9) = New SqlParameter("@WIDCode", SqlDbType.Int) With {.Value = WIDCode}

        RUN_EXUTE_PRO("WithdrawalTb_UPDATE", PRM)
    End Sub
    Public Sub UPDATE_WithdrawalTb_ACCEMPACTIVITY(Code As String, SafeID As Integer, Debit As Double, Credit As Double, InsertDate As Date, Description As String, ISID As String, IsActive As Boolean, TypeID As Integer, OperationTypeID As Integer)
        Dim PRM(9) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(2) = New SqlParameter("@Debit", SqlDbType.Float) With {.Value = Debit}
        PRM(3) = New SqlParameter("@Credit", SqlDbType.Float) With {.Value = Credit}
        PRM(4) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(5) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = ISID}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        PRM(9) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        RUN_EXUTE_PRO("AccEmpActivityTb_Update", PRM)
    End Sub
    Public Sub DELETE_WITHDRAWAL(WDCode As String)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@WDCode", SqlDbType.NVarChar, -1) With {.Value = WDCode.ToString}
        RUN_EXUTE_PRO("Withdrawal_Delete", PRM)
    End Sub
    Public Sub INSERTTB_ACCEMPACTIVITY(Code As String, SafeID As Integer, Debit As Double, Credit As Double, InsertDate As Date, Description As String, ISID As String, IsActive As Boolean, TypeID As Integer,
                                   OperationTypeID As Integer, IsConfirmed As Boolean, AccBranchID As Integer, MovementType As String, CurrencyID As Integer)
        Dim PRM(13) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        PRM(2) = New SqlParameter("@Debit", SqlDbType.Float) With {.Value = Debit}
        PRM(3) = New SqlParameter("@Credit", SqlDbType.Float) With {.Value = Credit}
        PRM(4) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(5) = New SqlParameter("@Description", SqlDbType.NVarChar, -1) With {.Value = Description}
        PRM(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, (50)) With {.Value = ISID}
        PRM(7) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(8) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
        PRM(9) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
        PRM(10) = New SqlParameter("@IsConfirmed", SqlDbType.Bit) With {.Value = IsConfirmed}
        PRM(11) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
        PRM(12) = New SqlParameter("@MovementType", SqlDbType.NVarChar, 50) With {.Value = MovementType}
        PRM(13) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        RUN_EXUTE_PRO("AccEmpActivityTb_Insert", PRM)
    End Sub
    Public Sub WITHDRAWAL_MaxID(typID As Integer, BranchID As Integer, USRID As Integer, CountryID As Integer, CityID As Integer)
        Dim PRM(4) As SqlParameter
        PRM(0) = New SqlParameter("@typID", SqlDbType.Int) With {.Value = typID}
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(2) = New SqlParameter("@USRID", SqlDbType.Int) With {.Value = USRID}
        PRM(3) = New SqlParameter("@CountryID", SqlDbType.Int) With {.Value = CountryID}
        PRM(4) = New SqlParameter("@CityID", SqlDbType.Int) With {.Value = CityID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("WithdrawalTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FrmSafeTransfer.WDCode.Text = dt.Rows(0)("Code")
            FrmSafeTransfer.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
End Class
