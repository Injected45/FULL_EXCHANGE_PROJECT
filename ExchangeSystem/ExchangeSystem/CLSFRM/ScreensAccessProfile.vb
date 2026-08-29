Imports System
Imports System.Linq
Imports System.Reflection

Namespace ExchangeSystem.CLSFRM
    Public Class ScreensAccessProfile
        Public Shared MaxID As Integer = 1

        Public Sub New(ByVal name As String, ByVal Optional parent As ScreensAccessProfile = Nothing)
            ScreenName = name
            ScreenID = Math.Min(System.Threading.Interlocked.Increment(MaxID), MaxID - 1)

            If parent IsNot Nothing Then
                ParentScreenID = parent.ScreenID
            Else
                ParentScreenID = 0
            End If

            Actions = New List(Of Master.Actions)() From {
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print,
                Master.Actions.Show,
                Master.Actions.Open
            }
        End Sub

        Public Property ScreenID As Integer
        Public Property ParentScreenID As Integer
        Public Property ScreenName As String
        Public Property ScreenCaption As String
        Public Property CanShow As Boolean
        Public Property CanOpen As Boolean
        Public Property CanAdd As Boolean
        Public Property CanEdit As Boolean
        Public Property CanDelete As Boolean
        Public Property CanPrint As Boolean
        Public Property Actions As List(Of Master.Actions)
    End Class

    Public Module Screens
        '1
        Public Company As ScreensAccessProfile = New ScreensAccessProfile("BtnCompny") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "الشركة"
        }
        '2
        Public CompanyInfo As ScreensAccessProfile = New ScreensAccessProfile("FrmCoBranch", Company) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "الفروع"
        }
        '3
        Public AddExpenses As ScreensAccessProfile = New ScreensAccessProfile("FrmExpenses", Company) With {
             .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة نوع مصروف"
        }
        '4
        Public EmployeeClassification As ScreensAccessProfile = New ScreensAccessProfile("FrmEmployeeClassification", Company) With {
             .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "تصنيف موظف"
        }
        '5
        Public NATIONALITY As ScreensAccessProfile = New ScreensAccessProfile("FRMNATIONALITY", Company) With {
             .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة جنسية"
        }
        '6
        Public AddCurrency As ScreensAccessProfile = New ScreensAccessProfile("FRMCURRENCY", Company) With {
             .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "العملات"
        }
        '        7
        Public AddSafe As ScreensAccessProfile = New ScreensAccessProfile("FRMSAFE", Company) With {
             .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "الخزائن"
        }
        '8
        Public INCREASE As ScreensAccessProfile = New ScreensAccessProfile("BtnInCreases") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "العلاوات"
        }
        '9
        Public PayIncrease As ScreensAccessProfile = New ScreensAccessProfile("FrmPayIncrease", INCREASE) With {
             .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة نوع علاوة"
        }
        '10
        Public AddCancelReason As ScreensAccessProfile = New ScreensAccessProfile("FrmAddCancelReason") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "إضافة مبرر إلغاء حوالة"
        }
        '11
        Public AddAccount As ScreensAccessProfile = New ScreensAccessProfile("FrmAccountsTree") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete
            },
            .ScreenCaption = "إضافة حساب"
        }
        '12
        Public DISCOUNTMENUTYPE As ScreensAccessProfile = New ScreensAccessProfile("DISCOUNTMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "الخصم"
        }
        '13
        Public BTNADDDISCOUNTTYPE As ScreensAccessProfile = New ScreensAccessProfile("FRMADDDISCOUNTTYPE", DISCOUNTMENUTYPE) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة نوع خصم"
        }
        '14
        Public CUSTOMERMENU As ScreensAccessProfile = New ScreensAccessProfile("CUSTOMERMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "العملاء"
        }
        '15
        Public BtnCustomer As ScreensAccessProfile = New ScreensAccessProfile("FRMCUSTOMER", CUSTOMERMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة عميل"
        }
        '16
        Public BANKMENU As ScreensAccessProfile = New ScreensAccessProfile("BANKMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "المصارف"
        }
        '1175
        Public BtnAddBank As ScreensAccessProfile = New ScreensAccessProfile("FRMBANK", BANKMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة مصرف"
        }
        '18
        Public BtnAddBBranch As ScreensAccessProfile = New ScreensAccessProfile("FRMBBRANCH", BANKMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة فرع مصرف"
        }
        '19
        Public BtnDelegate As ScreensAccessProfile = New ScreensAccessProfile("FRMDELEGATE", BANKMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة مندوب"
        }
        '20
        Public BtnInternalEx As ScreensAccessProfile = New ScreensAccessProfile("FRMINTERNALTRANSFER") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "تحويل داخلي"
        }
        '21
        Public BtnConfirmInternal As ScreensAccessProfile = New ScreensAccessProfile("FRMCONFIRMISSUED") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "اعتماد حوالة"
        }
        '22
        Public BtnCancelRequest As ScreensAccessProfile = New ScreensAccessProfile("FrmCancelRequest") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                 Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "طلب إلغاء حوالة"
        }
        '23
        Public BtnAgentCancelRequest As ScreensAccessProfile = New ScreensAccessProfile("FrmConfirmAgentCanceled") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "إلغاء حوالة وكيل صادرة"
        }
        '24
        Public BtnTransBetweenSafes As ScreensAccessProfile = New ScreensAccessProfile("FrmSafeTransfer") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "النقل بين الخزائن"
        }
        '25
        Public BRANCHSTATEMENTMENU As ScreensAccessProfile = New ScreensAccessProfile("BRANCHSTATEMENTMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "استعلامات الفرع"
        }
        '26
        Public btnShowSafeMovement As ScreensAccessProfile = New ScreensAccessProfile("FrmShowSafeMovement", BRANCHSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "حركة خزنة موظف"
        }
        '27
        Public BntSelectAccountsBetweenBranch As ScreensAccessProfile = New ScreensAccessProfile("FrmSelectAccountsBetweenBranches", BRANCHSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "مطابقة الجواري"
        }
        '28
        Public BTNPROFITS As ScreensAccessProfile = New ScreensAccessProfile("FRMPROFITS", BRANCHSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "عرض الإيرادات"
        }
        '29
        Public BtnMainSafeBalance As ScreensAccessProfile = New ScreensAccessProfile("FrmMainSafeBalance", BRANCHSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "أرصدة الخزائن الرئيسية"
        }
        '30
        Public BtnCurrencyStatement As ScreensAccessProfile = New ScreensAccessProfile("FrmCurrencyMovement", BRANCHSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "عرض حركة العملة"
        }
        '31
        Public BtnAgentsMovement As ScreensAccessProfile = New ScreensAccessProfile("FrmViewAgentMovement") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "عرض حركة وكيل"
        }
        '32
        Public CUSTOMERSTATEMENTMENU As ScreensAccessProfile = New ScreensAccessProfile("CUSTOMERSTATEMENTMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "العملاء"
        }
        '33
        Public BtnCustomerMovement As ScreensAccessProfile = New ScreensAccessProfile("FrmCustomerMovement", CUSTOMERSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "كشف حساب عميل"
        }
        '34
        Public GeneralExpensesMenu As ScreensAccessProfile = New ScreensAccessProfile("GeneralExpensesMenu") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "المصروفات العمومية"
        }
        '35
        Public BtnPettyCashStatement As ScreensAccessProfile = New ScreensAccessProfile("FRMSETTLEMENTSTATEMENT", GeneralExpensesMenu) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "استعلام تسوية العهد"
        }
        '36
        Public BtnExpenseStatement As ScreensAccessProfile = New ScreensAccessProfile("FRMEXPESESMOVEMENTTATEMENTS", GeneralExpensesMenu) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "استعلام حركة المصروفات"
        }
        '37
        Public BANKSTATEMENTMENU As ScreensAccessProfile = New ScreensAccessProfile("BANKSTATEMENTMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "استعلامات المصارف"
        }
        '38
        Public BtnBBranchMovement As ScreensAccessProfile = New ScreensAccessProfile("FRMBANKBRANCHMOVEMENT") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "كشف حساب فرع مصرف"
        }
        '39
        Public BtnEMPLOYEE As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPLOYEE") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة موظف"
        }
        '40
        Public EMPSALARYMENU As ScreensAccessProfile = New ScreensAccessProfile("EMPSALARYMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "الرواتب"
        }
        '41
        Public BtnCalcAllEmpSalary As ScreensAccessProfile = New ScreensAccessProfile("FRMSALARYCALCULATION", EMPSALARYMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "احتساب رواتب كل الموظفين"
        }
        '42
        Public BtnINDIVDUALSALARYCALC As ScreensAccessProfile = New ScreensAccessProfile("FRMINDIVDUALSALARYCALC", EMPSALARYMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "إخلاء طرف موظف"
        }
        '43
        Public BTNEMPCORRECTSLALRY As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPCORRECTSLALRY", EMPSALARYMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "معالجة خطأ في احتساب راتب"
        }
        '44
        Public BtnIndividualSalaryEMP As ScreensAccessProfile = New ScreensAccessProfile("FrmIndividualSalaryEMP", EMPSALARYMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "احتساب راتب فردي"
        }
        '45
        Public BTNEMPADVANCEPAYMENT As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPADVANCEPAYMENT") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "السلف"
        }
        '46
        Public BTNEMPADDINCREASE As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPADDINCREASE") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "علاوة موظف"
        }
        '47
        Public BTNEMPDISCOUNT As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPDISCOUNT") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "خصم على موظف"
        }
        '48
        Public EMPSTATEMENTMENU As ScreensAccessProfile = New ScreensAccessProfile("EMPTATEMENTMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "الاستعلامات والتقارير"
        }
        '49
        Public BTNLOADSALARIES As ScreensAccessProfile = New ScreensAccessProfile("FRMLOADSALARIES", EMPSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "كشف حركة موظف"
        }
        '50
        Public BtnAdvancePaymentLoadAllData As ScreensAccessProfile = New ScreensAccessProfile("FrmAdvancePaymentLoadAllData", EMPSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "استعلام السلف"
        }
        '51
        Public BtnDiscountsLoadAllData As ScreensAccessProfile = New ScreensAccessProfile("FrmDiscountsLoadAllData", EMPSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "استعلام الخصميات"
        }
        '52
        Public BtnIncreaseLoadAllData As ScreensAccessProfile = New ScreensAccessProfile("FrmIncreaseLoadAllData", EMPSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "استعلام العلاوات"
        }
        '53
        Public BTNEMPORCUSTWITHDRAWALLoadAllData As ScreensAccessProfile = New ScreensAccessProfile("FrmEMPORCUSTWITHDRAWALLoadAllData", EMPSTATEMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "استعلام السحب والإيداع"
        }
        '54
        Public PETIESMENU As ScreensAccessProfile = New ScreensAccessProfile("PETIESMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                 Master.Actions.Show
            },
            .ScreenCaption = "العهد وسند المصروفات"
        }
        '55
        Public BtnPettyCash As ScreensAccessProfile = New ScreensAccessProfile("FRMPettyCash", PETIESMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "صرف عهدة"
        }
        '56
        Public btnPettyCashSettlement As ScreensAccessProfile = New ScreensAccessProfile("FRMPettyCashSettlement", PETIESMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "تسوية عهدة"
        }
        '57
        Public BtnANOTHEREXPENS As ScreensAccessProfile = New ScreensAccessProfile("FRMANOTHEREXPENS", PETIESMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "سند مصروفات"
        }
        '58
        Public BILLPAYMENTMENU As ScreensAccessProfile = New ScreensAccessProfile("BILLPAYMENTMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "سندات الصرف"
        }
        '59
        Public BtnEmpPayment As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPWITHDRAWAL", BILLPAYMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "سند صرف لموظف"
        }
        '60
        Public BtnCustomerPayment As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPWITHDRAWAL", BILLPAYMENTMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "سند صرف لعميل"
        }
        '61
        Public EMPORCUSTDEPOSITMENU As ScreensAccessProfile = New ScreensAccessProfile("EMPORCUSTDEPOSITMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "سندات القبض"
        }
        '62
        Public BtnEmpDeposit As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPWITHDRAWAL", EMPORCUSTDEPOSITMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "سند إيداع لموظف"
        }
        '63
        Public BtnCustDeposit As ScreensAccessProfile = New ScreensAccessProfile("FRMEMPWITHDRAWAL", EMPORCUSTDEPOSITMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "سند إيداع لعميل"
        }
        '64
        Public BANKDEPOORWITHDRAMENU As ScreensAccessProfile = New ScreensAccessProfile("BANKDEPOORWITHDRAMENU") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "إيداع وسحب مصرفي"
        }
        '65
        Public BTNBANKDEPOSIT As ScreensAccessProfile = New ScreensAccessProfile("FRMBANKDEPOSIT", BANKDEPOORWITHDRAMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "إيداع مصرفي في حساب موظف"
        }
        '66
        Public BTNCUSTBANKDEPOSIT As ScreensAccessProfile = New ScreensAccessProfile("FRMBANKDEPOSIT", BANKDEPOORWITHDRAMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "إيداع مصرفي في حساب عميل"
        }
        '67
        Public BTNEMPBANKWITHDRAWAL As ScreensAccessProfile = New ScreensAccessProfile("FRMBANKDEPOSIT", BANKDEPOORWITHDRAMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "سحب مصرفي من حساب موظف"
        }
        '68
        Public BTNCUSTBANKWITHDRAWAL As ScreensAccessProfile = New ScreensAccessProfile("FRMBANKDEPOSIT", BANKDEPOORWITHDRAMENU) With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Open
            },
            .ScreenCaption = "سحب مصرفي من حساب عميل"
        }
        '69
        Public BtnUserAccessTemplate As ScreensAccessProfile = New ScreensAccessProfile("ViewAccessProfile") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show
            },
            .ScreenCaption = "عرض نماذج الوصول"
        }
        '70
        Public AddUserAccessTemplate As ScreensAccessProfile = New ScreensAccessProfile("FrmAccessProfile") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "نماج صلاحيات الوصول"
        }
        '71
        Public BtnAddUser As ScreensAccessProfile = New ScreensAccessProfile("FRMADDUSER") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "إضافة مستخدم"
        }
        '72
        Public BtnOpeningBalance As ScreensAccessProfile = New ScreensAccessProfile("FRMOPENINGBALANCE") With {
            .Actions = New List(Of Master.Actions)() From {
                Master.Actions.Show,
                Master.Actions.Add,
                Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print
            },
            .ScreenCaption = "الأرصدة الافتتاحية"
        }

        Public ReadOnly Property GetScreens As List(Of ScreensAccessProfile)

            Get
                Dim t = GetType(Screens)
                Dim fields = t.GetFields(BindingFlags.Public Or BindingFlags.Static)

                Dim list = New List(Of ScreensAccessProfile)()
                For Each item In fields
                    Dim obj = item.GetValue(Nothing)
                    If obj IsNot Nothing AndAlso obj.GetType() Is GetType(ScreensAccessProfile) Then list.Add(CType(obj, ScreensAccessProfile))
                Next

                Return list

            End Get
        End Property
    End Module
End Namespace
