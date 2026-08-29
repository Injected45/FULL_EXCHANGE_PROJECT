Imports System.Data.SqlClient

Public Class CLSOPENINGBALANCE
    Public Sub OpeningBalance_MaxID(BranchID As Integer, AccID As ULong)
        If FRMOPENINGBALANCE.FirstAccID.EditValue <> -1 Or FRMOPENINGBALANCE.FirstAccID.Text <> String.Empty Then
            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(1) = New SqlParameter("@AccID", SqlDbType.BigInt) With {.Value = AccID}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("OpeningBalanceTb_MaxID", PRM)
            If dt.Rows.Count > 0 Then
                FRMOPENINGBALANCE.Code.Text = dt.Rows(0)("Code")
                FRMOPENINGBALANCE.IDCode = dt.Rows(0)("ID")
            End If
        End If
    End Sub
    Public Function SERACH_OpeningBalance(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("OpeningBalanceTb_SelectByCode", PRM)
        Return DT
    End Function
    Public Sub OpeningBalanceTb_Insert(Code As String, InsertDate As Date, BranchID As Integer, ValTybe As Boolean, FirstAccMain As Integer, FirstAccParent As ULong, FirstAccID As ULong, SecondAccMain As Integer, SecondAccParent As ULong, SecondAccID As ULong, CurrencyID As Integer, OppVal As Decimal,
                                           Notes As String, IsActive As Boolean, SafeID As Integer)
        Dim PRM(14) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(3) = New SqlParameter("@ValType", SqlDbType.Bit) With {.Value = ValTybe}
        PRM(4) = New SqlParameter("@FirstAccMain", SqlDbType.Int) With {.Value = FirstAccMain}
        PRM(5) = New SqlParameter("@FirstAccParent", SqlDbType.BigInt) With {.Value = FirstAccParent}
        PRM(6) = New SqlParameter("@FirstAccID", SqlDbType.BigInt) With {.Value = FirstAccID}
        PRM(7) = New SqlParameter("@SecondAccMain", SqlDbType.Int) With {.Value = SecondAccMain}
        PRM(8) = New SqlParameter("@SecondAccParent", SqlDbType.BigInt) With {.Value = SecondAccParent}
        PRM(9) = New SqlParameter("@SecondAccID", SqlDbType.BigInt) With {.Value = SecondAccID}
        PRM(10) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        PRM(11) = New SqlParameter("@OppVal", SqlDbType.Decimal) With {.Precision = 18, .Size = 3, .Value = OppVal}
        PRM(12) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        PRM(13) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        PRM(14) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
        RUN_EXUTE_PRO("OpeningBalance_Tb_Insert", PRM)
        FRMOPENINGBALANCE.NEWRECORD()
    End Sub
    Public Sub OpeningBalanceTb_Delete(Code As String, IsUpdate As Boolean)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        RUN_EXUTE_PRO("OpeningBalanceTb_DeleteByCode", PRM)
        FRMOPENINGBALANCE.NEWRECORD()
    End Sub
End Class
