Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSPCSETTLEMENT
    Public Function PettyCash_SelectMax(BranchID As Integer, EMPID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int)
        PRM(1).Value = EMPID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[PCSettlementTB_MaxID]", PRM)
        Return DT
    End Function
    Public Function PCSettlementTB_CHECKCODE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[PCSettlementTB_CHECKCODE]", PRM)
        Return DT
    End Function
    Public Sub PCSettlement_insert(Code As String, InsertDate As Date, EMPID As Integer, BranchID As Integer, SafeID As Integer, CurrencyID As Integer, ISID As String, PCVal As Decimal,
                                   SettlementVal As Decimal, Notes As String, IDCode As ULong,
                                   AccIDSafeID As ULong, AccIDPetty As ULong, IsUpdate As Boolean, ExpensVal As Decimal, AccIDEX As ULong, NotesDe As String, EMPNAME As String, dt As DataTable)
        Try
            Dim prm(17) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (300)) With {.Value = Code}
            prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            prm(2) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            prm(4) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            prm(5) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            prm(6) = New SqlParameter("@ISID", SqlDbType.NVarChar, 300) With {.Value = ISID}
            prm(7) = New SqlParameter("@PCVal", SqlDbType.Decimal) With {.Value = PCVal}
            prm(8) = New SqlParameter("@SettlementVal", SqlDbType.Decimal) With {.Value = SettlementVal}
            prm(9) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            prm(10) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = IDCode}
            prm(11) = New SqlParameter("@AccIDSafeID", SqlDbType.BigInt) With {.Value = AccIDSafeID}
            prm(12) = New SqlParameter("@AccIDPetty", SqlDbType.BigInt) With {.Value = AccIDPetty}
            prm(13) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            prm(14) = New SqlParameter("@Type", SqlDbType.Structured) With {.Value = dt}
            prm(15) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, -1) With {.Value = EMPNAME}
            prm(16) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(17) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("PCSettlementTB_Insert", prm)

            If prm(16).Value = 0 Then
                'MessageBox.Show(prm(15).Value, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ErrorMessage(FRMPettyCash, "رسالة تنبيه", prm(17).Value)
                If FRMPettyCashSettlement.IsUpdate = False Then
                    If (FRMPettyCashSettlement.BranchID.EditValue <> -1 Or FRMPettyCashSettlement.BranchID.Text <> String.Empty) And (FRMPettyCashSettlement.EMPID.EditValue <> -1 Or FRMPettyCashSettlement.EMPID.Text <> String.Empty) Then
                        FRMPettyCashSettlement.EMPID_TextChanged(Nothing, Nothing)
                        Exit Sub
                    End If
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
                FRMPettyCashSettlement.Print()
            End If
            FrmSavedSuccessfully.Show()
            FRMPettyCashSettlement.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Sub PCSettlementTB_InsertAccDetails(Code As String, InsertDate As Date, BranchID As Integer, SafeID As Integer, CurrencyID As Integer,
                                   SettlementVal As Decimal, ExpensVal As Decimal, AccIDEX As ULong,
                                   NotesDe As String, AccIDSafeID As ULong, IsUpdate As Boolean, ISID As String, PCVal As Decimal, EXID As ULong)
        Dim prm(13) As SqlParameter
        prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (300)) With {.Value = Code}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(3) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        prm(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        prm(5) = New SqlParameter("@SettlementVal", SqlDbType.Decimal) With {.Value = SettlementVal}
        prm(6) = New SqlParameter("@ExpensVal", SqlDbType.Decimal) With {.Value = ExpensVal}
        prm(7) = New SqlParameter("@AccIDEX", SqlDbType.BigInt) With {.Value = AccIDEX}
        prm(8) = New SqlParameter("@NotesDe", SqlDbType.NVarChar, -1) With {.Value = NotesDe}
        prm(9) = New SqlParameter("@AccIDSafeID", SqlDbType.BigInt) With {.Value = AccIDSafeID}
        prm(10) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        prm(11) = New SqlParameter("@ISID", SqlDbType.NVarChar, 300) With {.Value = ISID}
        prm(12) = New SqlParameter("@PCVal", SqlDbType.Decimal) With {.Value = PCVal}
        prm(13) = New SqlParameter("@EXID", SqlDbType.BigInt) With {.Value = EXID}

        RUN_EXUTE_PRO("PCSettlementTB_InsertAccDetails", prm)
    End Sub
    Public Function SERACH_PCSETTLEMENT(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[PCSettlementTB_SELECTByCODE]", PRM)
        Return DT
    End Function
End Class
