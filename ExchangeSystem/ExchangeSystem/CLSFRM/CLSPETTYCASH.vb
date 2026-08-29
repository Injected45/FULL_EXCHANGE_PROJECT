Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSPETTYCASH
    Public Function PettyCash_SelectMax(BranchID As Integer, EMPID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID
        PRM(1) = New SqlParameter("@EMPID", SqlDbType.Int)
        PRM(1).Value = EMPID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("PettyCashTb_MaxID", PRM)
        Return DT
    End Function
    Public Sub ACCOUNTSTB_insert(Code As String, InsertDate As Date, BranchID As Integer, EMPID As Integer, PettyCashVal As Decimal, IDCode As ULong, CurrencyID As Integer,
                                 SafeID As Integer, Notes As String, AccIDPetty As ULong, AccIDSafeID As ULong, MovementType As String, MovementType2 As String, IsUpdate As Boolean)
        Try
            Dim prm(15) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, (300)) With {.Value = Code}
            prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            prm(3) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = EMPID}
            prm(4) = New SqlParameter("@PettyCashVal", SqlDbType.Decimal) With {.Value = PettyCashVal}
            prm(5) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = IDCode}
            prm(6) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
            prm(7) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            prm(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            prm(9) = New SqlParameter("@AccIDPetty", SqlDbType.BigInt) With {.Value = AccIDPetty}
            prm(10) = New SqlParameter("@AccIDSafeID", SqlDbType.BigInt) With {.Value = AccIDSafeID}
            prm(11) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
            prm(12) = New SqlParameter("@MovementType2", SqlDbType.NVarChar, -1) With {.Value = MovementType2}
            prm(13) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            prm(14) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(15) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("PettyCashTb_Insert", prm)

            If prm(14).Value = 0 Then
                'MessageBox.Show(prm(15).Value, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ErrorMessage(FRMPettyCash, "رسالة تنبيه", prm(15).Value)
                If FRMPettyCash.IsUpdate = False Then
                    If (FRMPettyCash.BranchID.EditValue <> -1 Or FRMPettyCash.BranchID.Text <> String.Empty) And (FRMPettyCash.EMPID.EditValue <> -1 Or FRMPettyCash.EMPID.Text <> String.Empty) Then
                        FRMPettyCash.EMPID_TextChanged(Nothing, Nothing)
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
                FRMPettyCash.Print()
            End If
            FrmSavedSuccessfully.Show()
            FRMPettyCash.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    'Public Sub AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert(ByVal SafeID As Integer, ByVal Debit As Decimal, ByVal Credit As Decimal, ByVal InsertDate As Date, ByVal Code As String, TypeID As Int32, OperationTypeID As Integer,
    '                                        AccBranchID As Integer, AccIDFrom As Integer, AccIDTo As Integer, MovementType As String, Note As String)
    '    Dim PRM(11) As SqlParameter
    '    PRM(0) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
    '    PRM(1) = New SqlParameter("@Debit", SqlDbType.Decimal) With {.Value = Debit}
    '    PRM(2) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
    '    PRM(3) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
    '    PRM(4) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
    '    PRM(5) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
    '    PRM(6) = New SqlParameter("@OperationTypeID", SqlDbType.Int) With {.Value = OperationTypeID}
    '    PRM(7) = New SqlParameter("@AccBranchID", SqlDbType.Int) With {.Value = AccBranchID}
    '    PRM(8) = New SqlParameter("@AccIDFrom", SqlDbType.Int) With {.Value = AccIDFrom}
    '    PRM(9) = New SqlParameter("@AccIDTo", SqlDbType.Int) With {.Value = AccIDTo}
    '    PRM(10) = New SqlParameter("@MovementType", SqlDbType.NVarChar, -1) With {.Value = MovementType}
    '    PRM(11) = New SqlParameter("@Note", SqlDbType.NVarChar, -1) With {.Value = Note}
    '    RUN_EXUTE_PRO("AccSafeActivityTb_EMPORCUSTWITHDRAWALTBInsert", PRM)
    'End Sub
    Public Function SERACH_EMPORCUSTWITHDRAWALTB(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[PettyCashTb_Select]", PRM)
        Return DT
    End Function
    'Public Sub EMPORCUSTWITHDRAWALTB_Delete(ByVal Code As String)
    '    Dim PRM(0) As SqlParameter
    '    PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
    '    RUN_EXUTE_PRO("[PettyCashTb_Delete]", PRM)
    'End Sub
End Class
