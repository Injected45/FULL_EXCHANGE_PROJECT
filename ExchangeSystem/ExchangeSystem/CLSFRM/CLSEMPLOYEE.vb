Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSEMPLOYEE
    Public Function SERACH_EMPLOYEE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        If FRMEMPLOYEE.DataBaseType = 1 Then
            DT = RUN_QUARY_PRO("EMPLOYEETB_Search", PRM)
        Else
            DT = RUN_QUARY_PRO("CONBD_EMPLOYEETB_Search", PRM)
        End If

        Return DT
    End Function
    Public Function SERACHBYID_EMPLOYEE(Code As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        'If FRMEMPLOYEE.DataBaseType = 1 Then
        DT = RUN_QUARY_PRO("EMPLOYEETB_SearchByID", PRM)
            'Else
            '    DT = RUN_QUARY_PRO("CONDB_EMPLOYEETB_SearchByID", PRM)
            'End If

            Return DT
    End Function
    Public Function EMPACC_EMPLOYEE(AccName As String) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1)
        PRM(0).Value = AccName
        Dim DT As New DataTable
        DT.Clear()
        If FRMEMPLOYEE.DataBaseType = 1 Then
            DT = RUN_QUARY_PRO("EmployeeTb_SelectEmAcc", PRM)
        Else
            DT = RUN_QUARY_PRO("CONDB_EmployeeTb_SelectEmAcc", PRM)
        End If

        Return DT
    End Function
    Public Function CHECK_EMP_NAME(ByVal EMPNAME As String, BranchID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, 250)
        PRM(0).Value = EMPNAME.Trim
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(1).Value = BranchID
        Dim DT As New DataTable
        DT.Clear()
        If NUMBER_FORM = 0 Then
            DT = RUN_QUARY_PRO("EMPLOYEETB_SEARCH_BYNAME", PRM)
        End If
        Return DT
    End Function
    '-----------------PUBLIC SUB INSERT ----------
    Public Sub INSERTTB__EMP(Code As String, EMPNAME As String, NATNUMBER As Long, CLASSIFICATION As Integer, PHONE1 As String, PHONE2 As String, BIRTHDATE As Date,
                             EMPDATE As Date, PassportNo As String, Nationality As String, CertificateType As String, SalaryVal As Double, EMail As String, ISACTIVE As Boolean,
                             BranchID As Integer, IsUpdate As Boolean, ID As Integer, BankID As Integer, BBranchID As Integer, AccOwner As String, BankNo As String, CanDebit As Int16,
                             Jobgrade As Integer, BankSalaryCalc As Boolean)
        Dim PRM(26) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, -1) With {.Value = EMPNAME}
        PRM(2) = New SqlParameter("@NATNUMBER", SqlDbType.NVarChar, -1) With {.Value = NATNUMBER}
        PRM(3) = New SqlParameter("@CLASSIFICATION", SqlDbType.Int) With {.Value = CLASSIFICATION}
        PRM(4) = New SqlParameter("@PHONE1", SqlDbType.NVarChar, 50) With {.Value = PHONE1}
        PRM(5) = New SqlParameter("@PHONE2", SqlDbType.NVarChar, 50) With {.Value = PHONE2}
        PRM(6) = New SqlParameter("@BIRTHDATE", SqlDbType.Date) With {.Value = BIRTHDATE}
        PRM(7) = New SqlParameter("@EMPDATE", SqlDbType.Date) With {.Value = EMPDATE}
        PRM(8) = New SqlParameter("@PassportNo", SqlDbType.NVarChar, -1) With {.Value = PassportNo}
        PRM(9) = New SqlParameter("@Nationality", SqlDbType.NVarChar, -1) With {.Value = Nationality}
        PRM(10) = New SqlParameter("@CertificateType", SqlDbType.NVarChar, -1) With {.Value = CertificateType}
        PRM(11) = New SqlParameter("@SalaryVal", SqlDbType.Float) With {.Value = SalaryVal}
        PRM(12) = New SqlParameter("@EMail", SqlDbType.NVarChar, -1) With {.Value = EMail}
        PRM(13) = New SqlParameter("@ISACTIVE", SqlDbType.Bit) With {.Value = ISACTIVE}
        PRM(14) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(15) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(16) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        PRM(17) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(18) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(19) = New SqlParameter("@BankID", SqlDbType.Int) With {.Value = BankID}
        PRM(20) = New SqlParameter("@BBranchID", SqlDbType.Int) With {.Value = BBranchID}
        PRM(21) = New SqlParameter("@AccOwner", SqlDbType.NVarChar, -1) With {.Value = AccOwner}
        PRM(22) = New SqlParameter("@BankNo", SqlDbType.NVarChar, -1) With {.Value = BankNo}
        PRM(23) = New SqlParameter("@CanDebit", SqlDbType.TinyInt) With {.Value = CanDebit}
        PRM(24) = New SqlParameter("@Jobgrade", SqlDbType.Int) With {.Value = Jobgrade}
        PRM(25) = New SqlParameter("@BankSalaryCalc", SqlDbType.Bit) With {.Value = BankSalaryCalc}
        PRM(26) = New SqlParameter("@ContractID", SqlDbType.BigInt) With {.Value = 0}

        RUN_EXUTE_PRO("EMPLOYEETB_Insert", PRM)


        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.DevExpressDark)
        XtraMessageBox.AllowCustomLookAndFeel = True
        FRMEMPLOYEE.msgST = PRM(17).Value
        If PRM(17).Value = 0 Then
            XtraMessageBox.Show(lookAndFeelError, PRM(18).Value, "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            If FRMEMPLOYEE.IsUpdate = False Then
                Dim mms As String = " *شركة الرحالة القابضة*" & vbNewLine &
                    "الموظف " & ":" & Space(1) & EMPNAME & vbNewLine &
                    "نفيدكم بأنه قد تم تسجيلكم ضمن كوادر الشركة تحت الرقم الوظيفي " & ":" & Space(1) & Code & vbNewLine &
                    "والمسمى الوظيفي " & ":" & Space(1) & FRMEMPLOYEE.CLASSIFICATION.Text & vbNewLine &
                    "نرحب بكم ضمن فريق *شركة الرحالة القابضة* ونتمنى لكم رحلة مهنية مليئة بالعطاء والنجاح." & vbNewLine &
                    "*أهلًا وسهلًا بكم معنا*"

                WATSAPPMsAG(FRMEMPLOYEE.PHONE1.Text.Trim(), mms, True)
            End If
            FRMEMPLOYEE.BtnNew.PerformClick()
        End If
    End Sub



    '-----------------PUBLIC SUB UPDATE ----------
    Public Sub UPDATETB_EMPAccountData(AccCode As ULong, AccName As String, AccParent As Decimal, BranchID As Integer, IDcode As ULong, AccID As ULong)
        Dim PRM(5) As SqlParameter
        PRM(0) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = AccCode}
        PRM(1) = New SqlParameter("@AccName", SqlDbType.NVarChar, -1) With {.Value = AccName}
        PRM(2) = New SqlParameter("@AccParent", SqlDbType.Decimal) With {.Value = AccParent}
        PRM(3) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(4) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDcode}
        PRM(5) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Value = AccID}
        RUN_EXUTE_PRO("ACCOUNTSTB_UPDATECHANGEDBRANCH", PRM)
    End Sub
    Public Sub UPDATETBSALARY_EMP(Code As String, SalaryVal As Double)
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@SalaryVal", SqlDbType.Float) With {.Value = SalaryVal}
        If FRMEMPLOYEE.DataBaseType = 1 Then
            RUN_EXUTE_PRO("EMPLOYEETB_UpdateByIdSalary", PRM)
        Else
            RUN_EXUTE_PRO("CONBD_EMPLOYEETB_UpdateByIdSalary", PRM)
        End If

    End Sub
    Public Sub UPDATETB_EMP(Code As String, EMPNAME As String, NATNUMBER As Long, CLASSIFICATION As Integer, PHONE1 As String, PHONE2 As String, BIRTHDATE As Date, EMPDATE As Date, PassportNo As String,
                            Nationality As String, CertificateType As String, SalaryVal As Double, EMail As String, Debit As Double, Credit As Double, ISACTIVE As Boolean, BranchID As Integer, ID As Integer)
        Dim PRM(17) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(1) = New SqlParameter("@EMPNAME", SqlDbType.NVarChar, -1) With {.Value = EMPNAME}
        PRM(2) = New SqlParameter("@NATNUMBER", SqlDbType.NVarChar, -1) With {.Value = NATNUMBER}
        PRM(3) = New SqlParameter("@CLASSIFICATION", SqlDbType.Int) With {.Value = CLASSIFICATION}
        PRM(4) = New SqlParameter("@PHONE1", SqlDbType.NVarChar, 50) With {.Value = PHONE1}
        PRM(5) = New SqlParameter("@PHONE2", SqlDbType.NVarChar, 50) With {.Value = PHONE2}
        PRM(6) = New SqlParameter("@BIRTHDATE", SqlDbType.Date) With {.Value = BIRTHDATE}
        PRM(7) = New SqlParameter("@EMPDATE", SqlDbType.Date) With {.Value = EMPDATE}
        PRM(8) = New SqlParameter("@PassportNo", SqlDbType.NVarChar, -1) With {.Value = PassportNo}
        PRM(9) = New SqlParameter("@Nationality", SqlDbType.NVarChar, -1) With {.Value = Nationality}
        PRM(10) = New SqlParameter("@CertificateType", SqlDbType.NVarChar, -1) With {.Value = CertificateType}
        PRM(11) = New SqlParameter("@SalaryVal", SqlDbType.Float) With {.Value = SalaryVal}
        PRM(12) = New SqlParameter("@EMail", SqlDbType.NVarChar, -1) With {.Value = EMail}
        PRM(13) = New SqlParameter("@Debit", SqlDbType.Float) With {.Value = Debit}
        PRM(14) = New SqlParameter("@Credit", SqlDbType.Float) With {.Value = Credit}
        PRM(15) = New SqlParameter("@ISACTIVE", SqlDbType.Bit) With {.Value = ISACTIVE}
        PRM(16) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(17) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}


        RUN_EXUTE_PRO("EMPLOYEETB_UpdateById", PRM)
    End Sub
    Public Sub EMPLOYEETB_DeleteById(ID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.NVarChar, -1) With {.Value = ID}
        RUN_EXUTE_PRO("EMPLOYEETB_DeleteById", PRM)
    End Sub
End Class
