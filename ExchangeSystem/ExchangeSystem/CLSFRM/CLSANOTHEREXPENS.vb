Imports System.Data.SqlClient

Public Class CLSANOTHEREXPENS

    Public Function ANOTHEREXPENSTB_CHECKCODE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[ANOTHEREXPENSTB_CHECKCODE]", PRM)
        Return DT
    End Function

    Public Function ANOTHEREXPENSTB_MaxID(BranchID As Integer, SAFEID As ULong) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(0).Value = BranchID
        PRM(1) = New SqlParameter("@SAFEID", SqlDbType.BigInt)
        PRM(1).Value = SAFEID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[ANOTHEREXPENSTB_MaxID]", PRM)
        Return DT
    End Function

    Public Sub ANOTHEREXPENSTB_Insert(Code As String, InsertDate As Date, BranchID As Integer, SafeID As ULong, CurrencyID As Integer,
                                  ExpensVal As Decimal, AccIDEX As ULong,
                                   Notes As String, IDCode As ULong, IsActive As Boolean, AccIDSafeID As ULong, Movement As String, IsUpdate As Boolean, UserInsert As ULong)
        Dim prm(13) As SqlParameter
        prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        prm(1) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        prm(2) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        prm(3) = New SqlParameter("@SafeID", SqlDbType.BigInt) With {.Value = SafeID}
        prm(4) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = CurrencyID}
        prm(5) = New SqlParameter("@ExpensVal", SqlDbType.Decimal) With {.Value = ExpensVal}
        prm(6) = New SqlParameter("@AccIDEX", SqlDbType.BigInt) With {.Value = AccIDEX}
        prm(7) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
        prm(8) = New SqlParameter("@IDCode", SqlDbType.BigInt) With {.Value = IDCode}
        prm(9) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
        prm(10) = New SqlParameter("@AccIDSafeID", SqlDbType.BigInt) With {.Value = AccIDSafeID}
        prm(11) = New SqlParameter("@Movement", SqlDbType.NVarChar, -1) With {.Value = Movement}
        prm(12) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        prm(13) = New SqlParameter("@UserInsert", SqlDbType.BigInt) With {.Value = UserInsert}
        RUN_EXUTE_PRO("ANOTHEREXPENSTB_Insert", prm)
        If IsUpdate = 0 Then
            Dim mms As String = "*شركة الرحالة القابضة*" & vbNewLine &
              FRMANOTHEREXPENS.BranchID.Text & vbNewLine & "ايصال مصروفات عمومية رقم " & ":" & Space(1) & vbNewLine & Code & vbNewLine &
                "مقابل" & ":" & Space(1) & FRMANOTHEREXPENS.AccIDEX.Text & vbNewLine &
                "بقيمة" & ":" & Space(1) & Cur_Code(FRMANOTHEREXPENS.CurrencyID.Text, ExpensVal, True, "n2") & vbNewLine &
                Cur_Code(FRMANOTHEREXPENS.CurrencyID.Text, ExpensVal, False, "n2") & vbNewLine &
                "من خزينة" & ":" & Space(1) & FRMANOTHEREXPENS.SafeID.Text & vbNewLine &
                "ملاحظات" & ":" & Space(1) & FRMANOTHEREXPENS.Notes.Text
            WATSAPPMsAG(get_gruop_id(BranchID), mms, False)

        End If
    End Sub

    Public Function SERACH_ANOTHEREXPENSTB(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = Code}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("[ANOTHEREXPENSTB_SELECTByCODE]", PRM)
        Return DT
    End Function

End Class
