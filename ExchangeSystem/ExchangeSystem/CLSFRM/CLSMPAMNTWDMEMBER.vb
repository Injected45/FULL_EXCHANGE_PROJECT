Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class CLSMPAMNTWDMEMBER
    Public Sub EMPORCUSTWITHDRAWALTB_MaxID(TypeID As Integer)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("MPAMNTWDMEMBERTB_MaxID", PRM)
        If dt.Rows.Count > 0 Then
            FRMPAMNTWDMEMBER.WDCode.Text = dt.Rows(0)("Code")
            FRMPAMNTWDMEMBER.IDCode = dt.Rows(0)("ID")
            'FRMEMPWDOVERBALANCE.WDCode.Text = dt.Rows(0)("Code")
            'FRMEMPWDOVERBALANCE.IDCode = dt.Rows(0)("ID")
            'FRMEMPWITHDRAWAL.WDCode.Text = dt.Rows(0)("Code")
            'FRMEMPWITHDRAWAL.IDCode = dt.Rows(0)("ID")
        End If
    End Sub
    Public Sub EMPORCUSTWITHDRAWALTB_Insert(ByVal InsertDate As Date, ByVal Code As String, ByVal MEMBERID As ULong, ByVal Credit As Double, ByVal SafeID As Integer,
                                            TypeID As Integer, CodeID As ULong, MBACCID As ULong, AssID As ULong, SafeAccID As ULong, IsActive As Boolean, IsUpdate As Boolean, Notes As String, PaidFor As String, Phone As String,
                                            IDNo As String)
        Try
            Dim PRM(17) As SqlParameter
            PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
            PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
            PRM(2) = New SqlParameter("@MEMBERID", SqlDbType.BigInt) With {.Value = MEMBERID}
            PRM(3) = New SqlParameter("@Credit", SqlDbType.Decimal) With {.Value = Credit}
            PRM(4) = New SqlParameter("@SafeID", SqlDbType.Int) With {.Value = SafeID}
            PRM(5) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID}
            PRM(6) = New SqlParameter("@CodeID", SqlDbType.BigInt) With {.Value = CodeID}
            PRM(7) = New SqlParameter("@MBACCID", SqlDbType.BigInt) With {.Value = MBACCID}
            PRM(8) = New SqlParameter("@AssID", SqlDbType.BigInt) With {.Value = AssID}
            PRM(9) = New SqlParameter("@SafeAccID", SqlDbType.BigInt) With {.Value = SafeAccID}
            PRM(10) = New SqlParameter("@IsActive", SqlDbType.Bit) With {.Value = IsActive}
            PRM(11) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            PRM(12) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes}
            PRM(13) = New SqlParameter("@PaidFor", SqlDbType.NVarChar, -1) With {.Value = PaidFor}
            PRM(14) = New SqlParameter("@Phone", SqlDbType.NVarChar, -1) With {.Value = Phone}
            PRM(15) = New SqlParameter("@IDNo", SqlDbType.NVarChar, -1) With {.Value = IDNo}
            PRM(16) = New SqlParameter("@MSGSTatues", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            PRM(17) = New SqlParameter("@MSGBOX", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("MPAMNTWDMEMBERTB_Insert", PRM)

            If PRM(16).Value = 0 Or PRM(16).Value = 2 Then
                ErrorMessage(FRMPAMNTWDMEMBER, "رسالة تنبيه", PRM(17).Value)
                If FRMPAMNTWDMEMBER.IsUpdate = False And PRM(16).Value = 0 Then
                    EMPORCUSTWITHDRAWALTB_MaxID(FRMPAMNTWDMEMBER.LOADTYPE)
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
                FRMPAMNTWDMEMBER.Print()
            End If
            FRMPAMNTWDMEMBER.senddorW()
            FrmSavedSuccessfully.Show()
            FRMPAMNTWDMEMBER.NEWRECORD()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Function SERACH_EMPORCUSTWITHDRAWALTB(Code As String, TypeID As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 50) With {.Value = Code}
        PRM(1) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = TypeID}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("MPAMNTWDMEMBERTB_SelectByCode", PRM)
        Return DT
    End Function
End Class
