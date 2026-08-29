Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSCUSTOMER

    Public Sub CUSTOMER_INSERT(InsertDate As Date, Code As String, CustName As String, Phone1 As String, Phone2 As String, CustmAddress As String, BranchID As Integer, IsUpdate As Boolean,
                               ID As Integer, CanDebit As Boolean, ISHidden As Boolean, NatNumber As String, Email As String, MartialStaus As Int32, UserName As String, Password As String,
                               ParentName As String, Nationality As Integer, BIRTHDATE As Date, EmpReg As String, AccountNo As String, BankID As String, Group_ID_fro_watssap As String,
                               sand_Group_ID As Boolean, AccType As Boolean, Registry1 As String, Registry2 As String, Ownername As String)


        Try


            Dim PRM(30) As SqlParameter
            PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(2) = New SqlParameter("@CustName", SqlDbType.NVarChar, 100) With {.Value = CustName}
            PRM(3) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
            PRM(4) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
            PRM(5) = New SqlParameter("@CustmAddress", SqlDbType.NVarChar, 150) With {.Value = CustmAddress}
            PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
            PRM(7) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(8) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(9) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            PRM(10) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
            PRM(11) = New SqlParameter("@CanDebit", SqlDbType.Bit) With {.Value = CanDebit}
            PRM(12) = New SqlParameter("@ISHidden", SqlDbType.Bit) With {.Value = ISHidden}
            PRM(13) = New SqlParameter("@NatNumber", SqlDbType.NVarChar, 50) With {.Value = NatNumber}
            PRM(14) = New SqlParameter("@accidRETURNS", SqlDbType.BigInt) With {.Direction = ParameterDirection.Output}
            PRM(15) = New SqlParameter("@Email", SqlDbType.NVarChar, -1) With {.Value = Email}
            PRM(16) = New SqlParameter("@MartialStaus", SqlDbType.TinyInt) With {.Value = MartialStaus}
            PRM(17) = New SqlParameter("@UserName", SqlDbType.NVarChar, -1) With {.Value = UserName}
            PRM(18) = New SqlParameter("@Password", SqlDbType.NVarChar, -1) With {.Value = Password}
            PRM(19) = New SqlParameter("@ParentName", SqlDbType.NVarChar, -1) With {.Value = ParentName}
            PRM(20) = New SqlParameter("@Nationality", SqlDbType.Int) With {.Value = Nationality}
            PRM(21) = New SqlParameter("@BIRTHDATE", SqlDbType.Date) With {.Value = BIRTHDATE}
            PRM(22) = New SqlParameter("@EmpReg", SqlDbType.NVarChar, 250) With {.Value = EmpReg}
            PRM(23) = New SqlParameter("@AccountNo", SqlDbType.NVarChar, -1) With {.Value = AccountNo}
            PRM(24) = New SqlParameter("@BankID", SqlDbType.NVarChar, 450) With {.Value = BankID}
            PRM(25) = New SqlParameter("@Group_ID_fro_watssap", SqlDbType.NVarChar, 901) With {.Value = Group_ID_fro_watssap}
            PRM(26) = New SqlParameter("@sand_Group_ID", SqlDbType.Bit) With {.Value = sand_Group_ID}
            PRM(27) = New SqlParameter("@AccType", SqlDbType.Bit) With {.Value = AccType}
            PRM(28) = New SqlParameter("@Registry1", SqlDbType.NVarChar, -1) With {.Value = Registry1}
            PRM(29) = New SqlParameter("@Registry2", SqlDbType.NVarChar, -1) With {.Value = Registry2}
            PRM(30) = New SqlParameter("@Ownername", SqlDbType.NVarChar, -1) With {.Value = Ownername}
            RUN_EXUTE_PRO("CustomersTb_Insert", PRM)
            Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
            XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.DevExpressDark)
            XtraMessageBox.AllowCustomLookAndFeel = True
            FRMCUSTOMER.msgST = PRM(8).Value
            If PRM(8).Value = 0 Then
                XtraMessageBox.Show(lookAndFeelError, PRM(9).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else
                If FRMCUSTOMER.IsUpdate = False Then
                    Dim taypeaccount As String
                    If FRMCUSTOMER.AccType.SelectedIndex = 0 Then
                        taypeaccount = "حساب افراد"
                    Else
                        taypeaccount = "حساب شركات"
                    End If
                    Dim mms As String = "*شركة الرحالة للصرافة*" & vbNewLine & "🎉 تم فتح حسابكم بنجاح" & vbNewLine & "بـ" & Space(1) & FRMCUSTOMER.BranchID.Text.Trim & Space(1) & "/" & "🧾" & Space(1) & taypeaccount & vbNewLine & "🤵‍♂" & " " & FRMCUSTOMER.CUSTNAME.Text.Trim & vbNewLine &
                        "📱" & Space(1) & "الهاتف" & Space(1) & ":" & Space(1) & FRMCUSTOMER.PHONE1.Text & vbNewLine &
                         "🔐" & Space(1) & "كود الحساب" & Space(1) & ":" & Space(1) & PRM(14).Value
                    If sand_Group_ID = False Then
                        WATSAPPMsAG(FRMCUSTOMER.PHONE1.Text.Trim, mms, True)
                    Else
                        WATSAPPMsAG(Group_ID_fro_watssap, mms, True)
                    End If

                    FRMCUSTOMER.AccID.Text = PRM(14).Value
                    FRMCUSTOMER.AccNo.Text = PRM(23).Value
                    FRMCUSTOMER.BtnNew.PerformClick()
                Else
                    'Dim mms As String = "مرحباً " & ":" & Space(1) & "*[" & FRMCUSTOMER.CUSTNAME.Text.Trim & "]*" & vbNewLine & "يسرنا إعلامك بفتح حساب لك في شركة الرحالة للصرافة " & vbNewLine & "برقم" & Space(1) & ":" & Space(1) & FRMCUSTOMER.CodeID.Text & vbNewLine & "للإستفسار: 0914200648 " & vbNewLine & "*شكرًا لثقتكم بنا*" & vbNewLine & "*فريق شركة الرحالة للصرافة*"
                    'WATSAPPMsAG(FRMCUSTOMER.PHONE1.Text.Trim, mms)
                    'FRMCUSTOMER.BtnNew.PerformClick()
                End If
            End If
        Catch ex As Exception
            ErrorMessage2("CustomersTb_Insert", ex.Message)
        End Try
    End Sub
    Public Sub EMPDIS_MaxID(BranchID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CustomersTb_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMCUSTOMER.CodeID.Text = dt.Rows(0)("Code")
        End If
    End Sub
    Public Sub CUSTOMER_Update(InsertDate As Date, Code As String, CustName As String, Phone1 As String, Phone2 As String, CustmAddress As String, BranchID As Integer)
        Dim PRM(8) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(2) = New SqlParameter("@CustName", SqlDbType.NVarChar, 100) With {.Value = CustName}
        PRM(3) = New SqlParameter("@Phone1", SqlDbType.NVarChar, 12) With {.Value = Phone1}
        PRM(4) = New SqlParameter("@Phone2", SqlDbType.NVarChar, 12) With {.Value = Phone2}
        PRM(5) = New SqlParameter("@CustmAddress", SqlDbType.NVarChar, 150) With {.Value = CustmAddress}
        PRM(6) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}

        RUN_EXUTE_PRO("CustomersTb_Update", PRM)

    End Sub
    Public Function CustomersTb_Select(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 150)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CustomersTb_Select", PRM)
        Return DT
    End Function
    Public Function CustomersTb_CheckPHoneExist(Phone1 As String, Phone2 As String) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Phone1", SqlDbType.NVarChar, -1) With {.Value = Phone1}
        PRM(1) = New SqlParameter("@Phone2", SqlDbType.NVarChar, -1) With {.Value = Phone2}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CustomersTb_CheckPHoneExist", PRM)
        Return DT
    End Function
    Public Function CustomersTb_CheckPHoneExistUpdate(ID As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CustomersTb_CheckPHoneExistUpdate", PRM)
        Return DT
    End Function
    Public Sub ACCOUNTSTB_UPDATECHANGEDBRANCH(ACCCODE As ULong, ACCNAME As String, FATHERPERINT As Decimal, ACCBRANCH As Integer, IDCode As ULong, AccID As ULong)
        Dim prm(5) As SqlParameter
        prm(0) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = ACCCODE}
        prm(1) = New SqlParameter("@AccName", SqlDbType.VarChar, 80) With {.Value = ACCNAME}
        prm(2) = New SqlParameter("@AccParent", SqlDbType.Decimal, 18, 0) With {.Value = FATHERPERINT}
        prm(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = ACCBRANCH}
        prm(4) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDCode}
        prm(5) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = AccID}
        RUN_EXUTE_PRO("ACCOUNTSTB_UPDATECHANGEDBRANCH", prm)
    End Sub


End Class
