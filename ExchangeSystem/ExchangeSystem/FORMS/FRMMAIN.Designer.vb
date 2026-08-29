Imports DevExpress.XtraEditors.Controls

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMMAIN
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMMAIN))
        Dim SuperToolTip1 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
        Dim ToolTipTitleItem1 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
        Dim ToolTipTitleItem2 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm1), True, True)
        Me.RepositoryItemPictureEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
        Me.RibbonPage2 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RP2 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.BarSubItem9 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem112 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem108 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem113 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem21 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem104 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem105 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnCancelRequest = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAgentCancelRequest = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddCancelReason = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem22 = New DevExpress.XtraBars.BarSubItem()
        Me.btnShowSafeMovement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem114 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnMainSafeBalance = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem163 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem216 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem30 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem117 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem137 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem139 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem202 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem94 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem5 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem36 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem37 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem38 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem39 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem6 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem41 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnViewCanceledTransfers = New DevExpress.XtraBars.BarButtonItem()
        Me.RPBASICINFO = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.CompGR = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.BtnCompny = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem28 = New DevExpress.XtraBars.BarButtonItem()
        Me.CompanyInfo = New DevExpress.XtraBars.BarButtonItem()
        Me.AddExpenses = New DevExpress.XtraBars.BarButtonItem()
        Me.EmployeeClassification = New DevExpress.XtraBars.BarButtonItem()
        Me.NATIONALITY = New DevExpress.XtraBars.BarButtonItem()
        Me.AddCurrency = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem78 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem109 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnServiceType = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem126 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem131 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem143 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem199 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem201 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem217 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnInCreases = New DevExpress.XtraBars.BarSubItem()
        Me.BtnAddBonusType = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem175 = New DevExpress.XtraBars.BarButtonItem()
        Me.DISCOUNTMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BTNADDDISCOUNTTYPE = New DevExpress.XtraBars.BarButtonItem()
        Me.CUSTOMERMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnCustomer = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddPartner = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem133 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem184 = New DevExpress.XtraBars.BarButtonItem()
        Me.BrnClearFrom = New DevExpress.XtraBars.BarButtonItem()
        Me.AddSafe = New DevExpress.XtraBars.BarButtonItem()
        Me.BANKMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnAddBank = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddBBranch = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnDelegate = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem79 = New DevExpress.XtraBars.BarButtonItem()
        Me.CompanyMenu = New DevExpress.XtraBars.BarSubItem()
        Me.BtnCoBranch = New DevExpress.XtraBars.BarButtonItem()
        Me.FrmSafes = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem35 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem66 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem82 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem67 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnEmpAddBonus = New DevExpress.XtraBars.BarButtonItem()
        Me.BTNMPDISVAL = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem8 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem46 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem47 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem48 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem50 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem49 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem40 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem8 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem4 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem25 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem()
        Me.RibbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.BtnChangeUser = New DevExpress.XtraBars.BarButtonItem()
        Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
        Me.BarButtonItem7 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem9 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonGroup1 = New DevExpress.XtraBars.BarButtonGroup()
        Me.BarSubItem1 = New DevExpress.XtraBars.BarSubItem()
        Me.BarListItem1 = New DevExpress.XtraBars.BarListItem()
        Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
        Me.BarLinkContainerItem1 = New DevExpress.XtraBars.BarLinkContainerItem()
        Me.BarMdiChildrenListItem1 = New DevExpress.XtraBars.BarMdiChildrenListItem()
        Me.BarDockingMenuItem1 = New DevExpress.XtraBars.BarDockingMenuItem()
        Me.BarButtonGroup2 = New DevExpress.XtraBars.BarButtonGroup()
        Me.BarButtonGroup3 = New DevExpress.XtraBars.BarButtonGroup()
        Me.BarSubItem2 = New DevExpress.XtraBars.BarSubItem()
        Me.BarStaticItem2 = New DevExpress.XtraBars.BarStaticItem()
        Me.BarListItem2 = New DevExpress.XtraBars.BarListItem()
        Me.BarButtonItem10 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnBranchName = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnUserName = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnDate = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnTime = New DevExpress.XtraBars.BarButtonItem()
        Me.BarEditItem2 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemPictureEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
        Me.BarEditItem3 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemPictureEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
        Me.BarEditItem4 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemImageEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageEdit()
        Me.BarEditItem5 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemPictureEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
        Me.BarButtonItem11 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem12 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem13 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem14 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarEditItem6 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemHypertextLabel1 = New DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel()
        Me.BtnCNNAME = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnCTNAME = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem15 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem16 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem17 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem18 = New DevExpress.XtraBars.BarButtonItem()
        Me.BGPBranches = New DevExpress.XtraBars.BarButtonItem()
        Me.BGPAgents = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem20 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem21 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem22 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem27 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnCurrencyMovement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem30 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnSelectAccountsBetweenBranches = New DevExpress.XtraBars.BarButtonItem()
        Me.BTNADDDISCOUNTTYPE1 = New DevExpress.XtraBars.BarButtonItem()
        Me.CUSTOMERSTATEMENTMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnCustomerMovement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem62 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem80 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem116 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem115 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem129 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem132 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem147 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem197 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem97 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem7 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem42 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem43 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem44 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem45 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem31 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnEMPLOYEE = New DevExpress.XtraBars.BarButtonItem()
        Me.EMPSALARYMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnCalcAllEmpSalary = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnINDIVDUALSALARYCALC = New DevExpress.XtraBars.BarButtonItem()
        Me.BTNEMPCORRECTSLALRY = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnIndividualSalaryEMP = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem83 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem10 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem58 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem59 = New DevExpress.XtraBars.BarButtonItem()
        Me.EMPSTATEMENTMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BTNLOADSALARIES = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnDiscountsLoadAllData = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnIncreaseLoadAllData = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem134 = New DevExpress.XtraBars.BarButtonItem()
        Me.BTNEMPORCUSTWITHDRAWALLoadAllData = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem6 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem63 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem171 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem179 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem190 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem192 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAdvancePaymentLoadAllData = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem69 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem70 = New DevExpress.XtraBars.BarButtonItem()
        Me.GeneralExpensesMenu = New DevExpress.XtraBars.BarSubItem()
        Me.BtnPettyCashStatement = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnExpenseStatement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem26 = New DevExpress.XtraBars.BarButtonItem()
        Me.BILLPAYMENTMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnEmpDeposit = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnEmpPayment = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem107 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem150 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem138 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem203 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem206 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem207 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem208 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem209 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem81 = New DevExpress.XtraBars.BarButtonItem()
        Me.BANKDEPOORWITHDRAMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BTNEMPBANKWITHDRAWAL = New DevExpress.XtraBars.BarButtonItem()
        Me.BTNBANKDEPOSIT = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem157 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem205 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnUserAccessTemplate = New DevExpress.XtraBars.BarButtonItem()
        Me.BANKSTATEMENTMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnBBranchMovement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem84 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem177 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem145 = New DevExpress.XtraBars.BarButtonItem()
        Me.BRANCHSTATEMENTMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnCurrencyStatement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem72 = New DevExpress.XtraBars.BarButtonItem()
        Me.PETIESMENU = New DevExpress.XtraBars.BarSubItem()
        Me.BtnPettyCash = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnANOTHEREXPENS = New DevExpress.XtraBars.BarButtonItem()
        Me.btnPettyCashSettlement = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem144 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddUser = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnOpeningBalance = New DevExpress.XtraBars.BarButtonItem()
        Me.BTNCURRENCYPRICE = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem5 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem19 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem24 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem29 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem32 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem33 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem34 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem52 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem53 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem3 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem54 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem55 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem4 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem56 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem57 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem60 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem61 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem64 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem65 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem71 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem75 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem76 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem11 = New DevExpress.XtraBars.BarSubItem()
        Me.BarSubItem13 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem86 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem87 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem88 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem14 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem91 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem92 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem15 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem93 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem95 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem210 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem214 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem215 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem16 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem98 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem172 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem17 = New DevExpress.XtraBars.BarSubItem()
        Me.BarSubItem18 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem148 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem19 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem149 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem20 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem102 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem103 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem173 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem174 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem181 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem99 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem100 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem101 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem106 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem110 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem23 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem118 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem24 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem68 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem74 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem119 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnLeave = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem187 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem191 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem96 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem25 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem120 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem121 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem140 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem141 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem151 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem158 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem180 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem85 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem26 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem51 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem122 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem27 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem89 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem123 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem28 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem90 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem124 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem125 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem128 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem73 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem855 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem85585 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem29 = New DevExpress.XtraBars.BarSubItem()
        Me.BarSubItem31 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem182 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem183 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem32 = New DevExpress.XtraBars.BarSubItem()
        Me.BarSubItem33 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonGroup4 = New DevExpress.XtraBars.BarButtonGroup()
        Me.BarSubItem34 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem77 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem35 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem127 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem130 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem135 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem36 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem136 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem142 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem37 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem185 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddProject = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnProjectPartner = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddPettyCash = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnPettySettlement = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAnotherExpense = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddProExpense = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddAssest = New DevExpress.XtraBars.BarButtonItem()
        Me.AddBasiscMenu = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem153 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddItem = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddItemDetails = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnAddSupplier = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnProAddPettyCash = New DevExpress.XtraBars.BarSubItem()
        Me.BtnProPayPetty = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnContractorPayment = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem156 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnImportItem = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem165 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem166 = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnPROEXPORTITEM = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem167 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem176 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem152 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem39 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem154 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem155 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem162 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem164 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem168 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem169 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem170 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem178 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem40 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem159 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem160 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem161 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem12 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem186 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem41 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem189 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem193 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem194 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem195 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem196 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem198 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem200 = New DevExpress.XtraBars.BarButtonItem()
        Me.MoActivetion = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem188 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem146 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem204 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarSubItem38 = New DevExpress.XtraBars.BarSubItem()
        Me.BarButtonItem211 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem212 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem213 = New DevExpress.XtraBars.BarButtonItem()
        Me.RibbonPageCategory1 = New DevExpress.XtraBars.Ribbon.RibbonPageCategory()
        Me.RibbonPageCategory2 = New DevExpress.XtraBars.Ribbon.RibbonPageCategory()
        Me.RibbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup28 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup2 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RP5 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup26 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RP3 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.BranchSTGR = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RP4 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.AddEmpGR = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPage6 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup21 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.AssGrroup = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup19 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RP6 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup13 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPage3 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPageGroup3 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.RepositoryItemButtonEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.RibbonStatusBar1 = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
        Me.RibbonPageGroup7 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.IntIncomeNotDel = New System.Windows.Forms.Label()
        Me.BtnIntIncomeNotDel = New DevExpress.XtraEditors.SimpleButton()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.BtnExtIncomeNotDel = New DevExpress.XtraEditors.SimpleButton()
        Me.InNotConfirmed = New System.Windows.Forms.Label()
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
        Me.OutComeDelivered = New System.Windows.Forms.Label()
        Me.BtnOutComeDelivered = New DevExpress.XtraEditors.SimpleButton()
        Me.BtnOutComeNotDelivered = New DevExpress.XtraEditors.SimpleButton()
        Me.OutComeNotDelivered = New System.Windows.Forms.Label()
        Me.BtnIntIncomeNotDel1 = New DevExpress.XtraEditors.SimpleButton()
        Me.FollowingInteral = New System.Windows.Forms.Label()
        Me.BtnIntIncomeNotDel11 = New DevExpress.XtraEditors.SimpleButton()
        Me.CanceledInteralIncome = New System.Windows.Forms.Label()
        Me.BtnOutcomeDeliveredInEx = New DevExpress.XtraEditors.SimpleButton()
        Me.LookUpEdit1 = New DevExpress.XtraEditors.LookUpEdit()
        Me.OutcomeDeliveredInEx = New System.Windows.Forms.Label()
        Me.BtnRecordCountConfirmCancel = New DevExpress.XtraEditors.SimpleButton()
        Me.ExtCanceledConfrimed = New System.Windows.Forms.Label()
        Me.SimpleButton21 = New DevExpress.XtraEditors.SimpleButton()
        Me.RecordCountConfirmCancel = New System.Windows.Forms.Label()
        Me.CONMOXSHer = New DevExpress.XtraEditors.LookUpEdit()
        Me.ExtOutcomeNotDelivered = New System.Windows.Forms.Label()
        Me.BtnRecordCountDeliveredCancel = New DevExpress.XtraEditors.SimpleButton()
        Me.RecordCountDeliveredCancel = New System.Windows.Forms.Label()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem17 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem18 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem10 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem11 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.OutcomeDeliveredInExLY = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem14 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem19 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem20 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem26 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem27 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem40 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem28 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem25 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.TAxiNotSend = New System.Windows.Forms.Label()
        Me.EditCount = New System.Windows.Forms.Label()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.LayoutControl2 = New DevExpress.XtraLayout.LayoutControl()
        Me.BtnConfirm = New DevExpress.XtraEditors.SimpleButton()
        Me.ConfirmInternalEx = New System.Windows.Forms.Label()
        Me.BtnConfirmCanceled = New DevExpress.XtraEditors.SimpleButton()
        Me.ConfirmInternalExCancel = New System.Windows.Forms.Label()
        Me.SimpleButton51 = New DevExpress.XtraEditors.SimpleButton()
        Me.RefuseCanceled = New System.Windows.Forms.Label()
        Me.ExtConfirm = New DevExpress.XtraEditors.SimpleButton()
        Me.ExternalConfirm = New System.Windows.Forms.Label()
        Me.BtnExtConfirmCanc = New DevExpress.XtraEditors.SimpleButton()
        Me.ExtCanceledConfrimed1 = New System.Windows.Forms.Label()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.CountLeaveCon = New System.Windows.Forms.Label()
        Me.BtnIntIncomeNotDel111 = New DevExpress.XtraEditors.SimpleButton()
        Me.CountLeaveEnd = New System.Windows.Forms.Label()
        Me.SimpleButton12 = New DevExpress.XtraEditors.SimpleButton()
        Me.TAxiCansel = New System.Windows.Forms.Label()
        Me.SimpleButton12111 = New DevExpress.XtraEditors.SimpleButton()
        Me.taxiSendFrom = New System.Windows.Forms.Label()
        Me.SimpleButton1211 = New DevExpress.XtraEditors.SimpleButton()
        Me.TaxiADD = New System.Windows.Forms.Label()
        Me.SimpleButton121 = New DevExpress.XtraEditors.SimpleButton()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LCIConfirm = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCITXTConfirm = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCICanceled = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCITXTCanceled = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem15 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem16 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCIConfirm1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCITXTConfirm1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCIConfirm2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LCITXTConfirm2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem23 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem24 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem29 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem30 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem3 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem22 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem31 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem33 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem34 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem35 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem36 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem37 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem38 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem39 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.BarButtonItem23 = New DevExpress.XtraBars.BarButtonItem()
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.RibbonPage4 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPage5 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.RibbonPage7 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.BarButtonItem111 = New DevExpress.XtraBars.BarButtonItem()
        Me.SplashScreenManager2 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm2), True, True)
        Me.RepositoryItemHypertextLabel2 = New DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel()
        Me.RepositoryItemHypertextLabel3 = New DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel()
        Me.BagWo = New System.ComponentModel.BackgroundWorker()
        Me.RibbonPage8 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.LayoutControl4 = New DevExpress.XtraLayout.LayoutControl()
        Me.CustAccID = New DevExpress.XtraEditors.LookUpEdit()
        Me.LayoutControlGroup6 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem32 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem4 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.EmptySpaceItem5 = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemPictureEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemPictureEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemPictureEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemHypertextLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CONMOXSHer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OutcomeDeliveredInExLY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem26, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem27, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem40, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem28, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem25, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl2.SuspendLayout()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCIConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCITXTConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCICanceled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCITXTCanceled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCIConfirm1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCITXTConfirm1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCIConfirm2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LCITXTConfirm2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem29, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem30, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem31, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem33, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem34, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem35, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem36, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem37, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem38, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem39, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemHypertextLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemHypertextLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl4.SuspendLayout()
        CType(Me.CustAccID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem32, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'RepositoryItemPictureEdit1
        '
        Me.RepositoryItemPictureEdit1.Name = "RepositoryItemPictureEdit1"
        Me.RepositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        '
        'RibbonPage2
        '
        Me.RibbonPage2.Name = "RibbonPage2"
        Me.RibbonPage2.Text = "RibbonPage2"
        '
        'RP2
        '
        Me.RP2.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup1})
        Me.RP2.ImageOptions.SvgImage = CType(resources.GetObject("RP2.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RP2.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.RP2.Name = "RP2"
        Me.RP2.Tag = 2
        Me.RP2.Text = "الصرافة"
        '
        'RibbonPageGroup1
        '
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarSubItem9)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarSubItem21)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarSubItem22)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.BarSubItem30)
        Me.RibbonPageGroup1.Name = "RibbonPageGroup1"
        Me.RibbonPageGroup1.Text = "RibbonPageGroup1"
        '
        'BarSubItem9
        '
        Me.BarSubItem9.Caption = "التحويلات"
        Me.BarSubItem9.Id = 377
        Me.BarSubItem9.ImageOptions.SvgImage = CType(resources.GetObject("BarSubItem9.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarSubItem9.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem112), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem108), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem113)})
        Me.BarSubItem9.Name = "BarSubItem9"
        '
        'BarButtonItem112
        '
        Me.BarButtonItem112.Caption = "تحويل داخلي"
        Me.BarButtonItem112.Id = 500
        Me.BarButtonItem112.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem112.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem112.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem112.Name = "BarButtonItem112"
        '
        'BarButtonItem108
        '
        Me.BarButtonItem108.Caption = "حوالة خارجية"
        Me.BarButtonItem108.Id = 493
        Me.BarButtonItem108.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem108.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem108.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem108.Name = "BarButtonItem108"
        '
        'BarButtonItem113
        '
        Me.BarButtonItem113.Caption = "اعتماد حوالة"
        Me.BarButtonItem113.Id = 501
        Me.BarButtonItem113.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItem113.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.BarButtonItem113.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem113.Name = "BarButtonItem113"
        '
        'BarSubItem21
        '
        Me.BarSubItem21.Caption = "عمليات الحوالات"
        Me.BarSubItem21.Id = 468
        Me.BarSubItem21.ImageOptions.SvgImage = CType(resources.GetObject("BarSubItem21.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarSubItem21.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem104), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem105), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnCancelRequest), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAgentCancelRequest), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddCancelReason)})
        Me.BarSubItem21.Name = "BarSubItem21"
        '
        'BarButtonItem104
        '
        Me.BarButtonItem104.Caption = "طلب تعديل على بيانات حوالة"
        Me.BarButtonItem104.Id = 464
        Me.BarButtonItem104.Name = "BarButtonItem104"
        '
        'BarButtonItem105
        '
        Me.BarButtonItem105.Caption = "إعتماد تعديل حوالة"
        Me.BarButtonItem105.Id = 466
        Me.BarButtonItem105.Name = "BarButtonItem105"
        '
        'BtnCancelRequest
        '
        Me.BtnCancelRequest.Caption = "تقديم طلب لإلغاء حوالة"
        Me.BtnCancelRequest.Id = 75
        Me.BtnCancelRequest.ImageOptions.SvgImage = CType(resources.GetObject("BtnCancelRequest.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnCancelRequest.Name = "BtnCancelRequest"
        Me.BtnCancelRequest.Tag = 22
        '
        'BtnAgentCancelRequest
        '
        Me.BtnAgentCancelRequest.Caption = "إعادة توجيه حوالة"
        Me.BtnAgentCancelRequest.Id = 82
        Me.BtnAgentCancelRequest.ImageOptions.SvgImage = CType(resources.GetObject("BtnAgentCancelRequest.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnAgentCancelRequest.Name = "BtnAgentCancelRequest"
        Me.BtnAgentCancelRequest.Tag = 23
        '
        'BtnAddCancelReason
        '
        Me.BtnAddCancelReason.Caption = "إضافة مبرر إلغاء حوالة"
        Me.BtnAddCancelReason.Id = 498
        Me.BtnAddCancelReason.Name = "BtnAddCancelReason"
        '
        'BarSubItem22
        '
        Me.BarSubItem22.Caption = "الخزينة"
        Me.BarSubItem22.Id = 469
        Me.BarSubItem22.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.safebox_business_and_finance_svgrepo_com
        Me.BarSubItem22.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.btnShowSafeMovement), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem114), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnMainSafeBalance), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem163), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem216)})
        Me.BarSubItem22.Name = "BarSubItem22"
        '
        'btnShowSafeMovement
        '
        Me.btnShowSafeMovement.Caption = "خزائن الموظفين"
        Me.btnShowSafeMovement.Id = 257
        Me.btnShowSafeMovement.Name = "btnShowSafeMovement"
        Me.btnShowSafeMovement.Tag = 26
        '
        'BarButtonItem114
        '
        Me.BarButtonItem114.Caption = "النقل بين الخزائن"
        Me.BarButtonItem114.Id = 504
        Me.BarButtonItem114.Name = "BarButtonItem114"
        '
        'BtnMainSafeBalance
        '
        Me.BtnMainSafeBalance.Caption = "الخزينة الرئيسية"
        Me.BtnMainSafeBalance.Id = 260
        Me.BtnMainSafeBalance.Name = "BtnMainSafeBalance"
        Me.BtnMainSafeBalance.Tag = 29
        '
        'BarButtonItem163
        '
        Me.BarButtonItem163.Caption = "كشف نقدية الشركة"
        Me.BarButtonItem163.Id = 679
        Me.BarButtonItem163.Name = "BarButtonItem163"
        '
        'BarButtonItem216
        '
        Me.BarButtonItem216.Caption = "خزائن العملة"
        Me.BarButtonItem216.Id = 753
        Me.BarButtonItem216.Name = "BarButtonItem216"
        '
        'BarSubItem30
        '
        Me.BarSubItem30.Caption = "كشف حركة التحويلات"
        Me.BarSubItem30.Id = 589
        Me.BarSubItem30.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem117), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem137), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem139), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem202)})
        Me.BarSubItem30.Name = "BarSubItem30"
        '
        'BarButtonItem117
        '
        Me.BarButtonItem117.Caption = "كشف التحويلات"
        Me.BarButtonItem117.Id = 509
        Me.BarButtonItem117.Name = "BarButtonItem117"
        '
        'BarButtonItem137
        '
        Me.BarButtonItem137.Caption = "كشف الإيرادات والخسائر"
        Me.BarButtonItem137.Id = 634
        Me.BarButtonItem137.Name = "BarButtonItem137"
        '
        'BarButtonItem139
        '
        Me.BarButtonItem139.Caption = "كشف التحويلات الخارجي"
        Me.BarButtonItem139.Id = 637
        Me.BarButtonItem139.Name = "BarButtonItem139"
        '
        'BarButtonItem202
        '
        Me.BarButtonItem202.Caption = "كشف الاقفالات مع الوكلاء"
        Me.BarButtonItem202.Id = 738
        Me.BarButtonItem202.Name = "BarButtonItem202"
        '
        'BarButtonItem94
        '
        Me.BarButtonItem94.Caption = "حوالة خارجية واردة"
        Me.BarButtonItem94.Id = 545
        Me.BarButtonItem94.Name = "BarButtonItem94"
        '
        'BarSubItem5
        '
        Me.BarSubItem5.Caption = "السحب والإيداع"
        Me.BarSubItem5.Id = 117
        Me.BarSubItem5.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem36), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem37), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem38), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem39)})
        Me.BarSubItem5.Name = "BarSubItem5"
        '
        'BarButtonItem36
        '
        Me.BarButtonItem36.Caption = "سحب من حساب عميل"
        Me.BarButtonItem36.Id = 118
        Me.BarButtonItem36.Name = "BarButtonItem36"
        '
        'BarButtonItem37
        '
        Me.BarButtonItem37.Caption = "إيداع في حساب عميل"
        Me.BarButtonItem37.Id = 119
        Me.BarButtonItem37.Name = "BarButtonItem37"
        '
        'BarButtonItem38
        '
        Me.BarButtonItem38.Caption = "سحب من حساب موظف"
        Me.BarButtonItem38.Id = 120
        Me.BarButtonItem38.Name = "BarButtonItem38"
        '
        'BarButtonItem39
        '
        Me.BarButtonItem39.Caption = "إيداع في حساب موظف"
        Me.BarButtonItem39.Id = 121
        Me.BarButtonItem39.Name = "BarButtonItem39"
        '
        'BarSubItem6
        '
        Me.BarSubItem6.Caption = "استعلامات الموظفين"
        Me.BarSubItem6.Id = 127
        Me.BarSubItem6.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem41)})
        Me.BarSubItem6.Name = "BarSubItem6"
        '
        'BarButtonItem41
        '
        Me.BarButtonItem41.Caption = "استعلام عن العلاوات"
        Me.BarButtonItem41.Id = 128
        Me.BarButtonItem41.Name = "BarButtonItem41"
        '
        'BtnViewCanceledTransfers
        '
        Me.BtnViewCanceledTransfers.Caption = "عرض الحوالات الملغاة"
        Me.BtnViewCanceledTransfers.Id = 73
        Me.BtnViewCanceledTransfers.ImageOptions.Image = CType(resources.GetObject("BtnViewCanceledTransfers.ImageOptions.Image"), System.Drawing.Image)
        Me.BtnViewCanceledTransfers.ImageOptions.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnViewCanceledTransfers.Name = "BtnViewCanceledTransfers"
        '
        'RPBASICINFO
        '
        Me.RPBASICINFO.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.CompGR})
        Me.RPBASICINFO.ImageOptions.SvgImage = CType(resources.GetObject("RPBASICINFO.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RPBASICINFO.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.RPBASICINFO.Name = "RPBASICINFO"
        Me.RPBASICINFO.Tag = 1
        Me.RPBASICINFO.Text = "البيانات الأساسية"
        '
        'CompGR
        '
        Me.CompGR.ItemLinks.Add(Me.BtnCompny)
        Me.CompGR.ItemLinks.Add(Me.BtnInCreases)
        Me.CompGR.ItemLinks.Add(Me.DISCOUNTMENU)
        Me.CompGR.ItemLinks.Add(Me.CUSTOMERMENU)
        Me.CompGR.ItemLinks.Add(Me.BrnClearFrom)
        Me.CompGR.Name = "CompGR"
        Me.CompGR.Text = "الشركة"
        '
        'BtnCompny
        '
        Me.BtnCompny.Caption = "الشركة"
        Me.BtnCompny.Id = 249
        Me.BtnCompny.ImageOptions.SvgImage = CType(resources.GetObject("BtnCompny.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnCompny.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem28), New DevExpress.XtraBars.LinkPersistInfo(Me.CompanyInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.AddExpenses), New DevExpress.XtraBars.LinkPersistInfo(Me.EmployeeClassification), New DevExpress.XtraBars.LinkPersistInfo(Me.NATIONALITY, True), New DevExpress.XtraBars.LinkPersistInfo(Me.AddCurrency), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, False, Me.BarButtonItem78, False), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem109), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnServiceType), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem126), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem131), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem143), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem201), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem217)})
        Me.BtnCompny.Name = "BtnCompny"
        Me.BtnCompny.Tag = 1
        '
        'BarButtonItem28
        '
        Me.BarButtonItem28.Caption = "بيانات الشركة"
        Me.BarButtonItem28.Id = 318
        Me.BarButtonItem28.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem28.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem28.Name = "BarButtonItem28"
        '
        'CompanyInfo
        '
        Me.CompanyInfo.Caption = "الفروع"
        Me.CompanyInfo.Id = 250
        Me.CompanyInfo.ImageOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.Branch
        Me.CompanyInfo.Name = "CompanyInfo"
        Me.CompanyInfo.Tag = 2
        '
        'AddExpenses
        '
        Me.AddExpenses.Caption = "إضافة نوع مصروف"
        Me.AddExpenses.Id = 252
        Me.AddExpenses.ImageOptions.SvgImage = CType(resources.GetObject("AddExpenses.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.AddExpenses.Name = "AddExpenses"
        Me.AddExpenses.Tag = 3
        '
        'EmployeeClassification
        '
        Me.EmployeeClassification.Caption = "تصنيف موظف"
        Me.EmployeeClassification.Id = 251
        Me.EmployeeClassification.ImageOptions.SvgImage = CType(resources.GetObject("EmployeeClassification.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.EmployeeClassification.Name = "EmployeeClassification"
        Me.EmployeeClassification.Tag = 4
        '
        'NATIONALITY
        '
        Me.NATIONALITY.Caption = "إضافة جنسية"
        Me.NATIONALITY.Id = 253
        Me.NATIONALITY.ImageOptions.SvgImage = CType(resources.GetObject("NATIONALITY.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.NATIONALITY.Name = "NATIONALITY"
        Me.NATIONALITY.Tag = 5
        '
        'AddCurrency
        '
        Me.AddCurrency.Caption = "العملات"
        Me.AddCurrency.Id = 289
        Me.AddCurrency.ImageOptions.SvgImage = CType(resources.GetObject("AddCurrency.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.AddCurrency.Name = "AddCurrency"
        Me.AddCurrency.Tag = 6
        '
        'BarButtonItem78
        '
        Me.BarButtonItem78.Caption = "إضافة حساب مدين"
        Me.BarButtonItem78.Id = 386
        Me.BarButtonItem78.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem78.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem78.Name = "BarButtonItem78"
        Me.BarButtonItem78.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarButtonItem109
        '
        Me.BarButtonItem109.Caption = "إضافة حساب"
        Me.BarButtonItem109.Id = 495
        Me.BarButtonItem109.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem109.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem109.Name = "BarButtonItem109"
        '
        'BtnServiceType
        '
        Me.BtnServiceType.Caption = "إضافة نوع خدمة للحوالات الخارجية"
        Me.BtnServiceType.Id = 529
        Me.BtnServiceType.ImageOptions.SvgImage = CType(resources.GetObject("BtnServiceType.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnServiceType.Name = "BtnServiceType"
        '
        'BarButtonItem126
        '
        Me.BarButtonItem126.Caption = "إضافة دولة"
        Me.BarButtonItem126.Id = 575
        Me.BarButtonItem126.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem126.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem126.Name = "BarButtonItem126"
        '
        'BarButtonItem131
        '
        Me.BarButtonItem131.Caption = "اضافة مدينة"
        Me.BarButtonItem131.Id = 625
        Me.BarButtonItem131.Name = "BarButtonItem131"
        '
        'BarButtonItem143
        '
        Me.BarButtonItem143.Caption = "إضافة مصروف أصل"
        Me.BarButtonItem143.Id = 644
        Me.BarButtonItem143.Name = "BarButtonItem143"
        '
        'BarButtonItem199
        '
        Me.BarButtonItem199.Caption = "bbb"
        Me.BarButtonItem199.Id = 736
        Me.BarButtonItem199.Name = "BarButtonItem199"
        Me.BarButtonItem199.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarButtonItem201
        '
        Me.BarButtonItem201.Caption = "BarButtonItem201"
        Me.BarButtonItem201.Id = 737
        Me.BarButtonItem201.Name = "BarButtonItem201"
        Me.BarButtonItem201.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarButtonItem217
        '
        Me.BarButtonItem217.Caption = "إضافة شركة أو قسم"
        Me.BarButtonItem217.Id = 754
        Me.BarButtonItem217.Name = "BarButtonItem217"
        '
        'BtnInCreases
        '
        Me.BtnInCreases.Caption = "العلاوات"
        Me.BtnInCreases.Id = 94
        Me.BtnInCreases.ImageOptions.SvgImage = CType(resources.GetObject("BtnInCreases.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnInCreases.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddBonusType), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem175)})
        Me.BtnInCreases.Name = "BtnInCreases"
        Me.BtnInCreases.Tag = 8
        '
        'BtnAddBonusType
        '
        Me.BtnAddBonusType.Caption = "إضافة نوع علاوة"
        Me.BtnAddBonusType.Id = 95
        Me.BtnAddBonusType.ImageOptions.SvgImage = CType(resources.GetObject("BtnAddBonusType.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnAddBonusType.Name = "BtnAddBonusType"
        Me.BtnAddBonusType.Tag = 9
        '
        'BarButtonItem175
        '
        Me.BarButtonItem175.Caption = "سقف السحب لحساب"
        Me.BarButtonItem175.Id = 696
        Me.BarButtonItem175.Name = "BarButtonItem175"
        '
        'DISCOUNTMENU
        '
        Me.DISCOUNTMENU.Caption = "الخصم"
        Me.DISCOUNTMENU.Id = 86
        Me.DISCOUNTMENU.ImageOptions.SvgImage = CType(resources.GetObject("DISCOUNTMENU.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.DISCOUNTMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BTNADDDISCOUNTTYPE)})
        Me.DISCOUNTMENU.Name = "DISCOUNTMENU"
        Me.DISCOUNTMENU.Tag = 12
        '
        'BTNADDDISCOUNTTYPE
        '
        Me.BTNADDDISCOUNTTYPE.Caption = "إضافة نوع خصم"
        Me.BTNADDDISCOUNTTYPE.Id = 87
        Me.BTNADDDISCOUNTTYPE.ImageOptions.SvgImage = CType(resources.GetObject("BTNADDDISCOUNTTYPE.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BTNADDDISCOUNTTYPE.Name = "BTNADDDISCOUNTTYPE"
        Me.BTNADDDISCOUNTTYPE.Tag = 13
        '
        'CUSTOMERMENU
        '
        Me.CUSTOMERMENU.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.CUSTOMERMENU.Caption = "العملاء"
        Me.CUSTOMERMENU.Id = 108
        Me.CUSTOMERMENU.ImageOptions.SvgImage = CType(resources.GetObject("CUSTOMERMENU.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.CUSTOMERMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnCustomer), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddPartner), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem133), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem184)})
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Disabled.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Disabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Disabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Hovered.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Hovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Hovered.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Normal.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Pressed.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Pressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.AppearanceMenu.Pressed.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.HeaderItemAppearance.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.HeaderItemAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.HeaderItemAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.MenuBar.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.MenuBar.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.MenuBar.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.MenuCaption.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.MenuCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.MenuCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.SideStrip.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.SideStrip.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.SideStrip.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.SideStripNonRecent.Options.UseTextOptions = True
        Me.CUSTOMERMENU.MenuAppearance.SideStripNonRecent.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CUSTOMERMENU.MenuAppearance.SideStripNonRecent.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CUSTOMERMENU.Name = "CUSTOMERMENU"
        Me.CUSTOMERMENU.OptionsMultiColumn.TextHorizontalAlignment = DevExpress.Utils.Drawing.ItemHorizontalAlignment.Center
        Me.CUSTOMERMENU.RibbonStyle = CType((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
        Me.CUSTOMERMENU.Tag = 14
        '
        'BtnCustomer
        '
        Me.BtnCustomer.Caption = "إضافة عميل"
        Me.BtnCustomer.Id = 109
        Me.BtnCustomer.ImageOptions.SvgImage = CType(resources.GetObject("BtnCustomer.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnCustomer.Name = "BtnCustomer"
        Me.BtnCustomer.Tag = 15
        '
        'BtnAddPartner
        '
        Me.BtnAddPartner.Caption = "إضافة شريك"
        Me.BtnAddPartner.Id = 626
        Me.BtnAddPartner.Name = "BtnAddPartner"
        '
        'BarButtonItem133
        '
        Me.BarButtonItem133.Caption = "إضافة حساب خارجي"
        Me.BarButtonItem133.Id = 629
        Me.BarButtonItem133.Name = "BarButtonItem133"
        '
        'BarButtonItem184
        '
        Me.BarButtonItem184.Caption = "اعتماد عميل من التطبيق"
        Me.BarButtonItem184.Id = 706
        Me.BarButtonItem184.Name = "BarButtonItem184"
        '
        'BrnClearFrom
        '
        Me.BrnClearFrom.Caption = "تنظيف الجداول"
        Me.BrnClearFrom.Id = 193
        Me.BrnClearFrom.Name = "BrnClearFrom"
        '
        'AddSafe
        '
        Me.AddSafe.Caption = "الخزائن"
        Me.AddSafe.Id = 290
        Me.AddSafe.Name = "AddSafe"
        Me.AddSafe.Tag = 7
        '
        'BANKMENU
        '
        Me.BANKMENU.Caption = "الاظافات"
        Me.BANKMENU.Id = 220
        Me.BANKMENU.ImageOptions.SvgImage = CType(resources.GetObject("BANKMENU.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BANKMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddBank), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddBBranch), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnDelegate), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem79)})
        Me.BANKMENU.Name = "BANKMENU"
        Me.BANKMENU.Tag = 16
        '
        'BtnAddBank
        '
        Me.BtnAddBank.Caption = "إضافة مصرف"
        Me.BtnAddBank.Id = 221
        Me.BtnAddBank.ImageOptions.SvgImage = CType(resources.GetObject("BtnAddBank.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnAddBank.Name = "BtnAddBank"
        Me.BtnAddBank.Tag = 17
        '
        'BtnAddBBranch
        '
        Me.BtnAddBBranch.Caption = "إضافة فرع مصرف"
        Me.BtnAddBBranch.Id = 222
        Me.BtnAddBBranch.ImageOptions.SvgImage = CType(resources.GetObject("BtnAddBBranch.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnAddBBranch.Name = "BtnAddBBranch"
        Me.BtnAddBBranch.Tag = 18
        '
        'BtnDelegate
        '
        Me.BtnDelegate.Caption = "إضافة مندوب"
        Me.BtnDelegate.Id = 223
        Me.BtnDelegate.Name = "BtnDelegate"
        Me.BtnDelegate.Tag = 19
        '
        'BarButtonItem79
        '
        Me.BarButtonItem79.Caption = "إضافة نوع خدمة إكترونية"
        Me.BarButtonItem79.Id = 388
        Me.BarButtonItem79.Name = "BarButtonItem79"
        '
        'CompanyMenu
        '
        Me.CompanyMenu.Caption = "couk"
        Me.CompanyMenu.Id = 110
        Me.CompanyMenu.ImageOptions.SvgImage = CType(resources.GetObject("CompanyMenu.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.CompanyMenu.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnCoBranch), New DevExpress.XtraBars.LinkPersistInfo(Me.FrmSafes), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem35), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem66)})
        Me.CompanyMenu.Name = "CompanyMenu"
        '
        'BtnCoBranch
        '
        Me.BtnCoBranch.Caption = "الفروع"
        Me.BtnCoBranch.Id = 111
        Me.BtnCoBranch.Name = "BtnCoBranch"
        '
        'FrmSafes
        '
        Me.FrmSafes.Caption = "الخزائن"
        Me.FrmSafes.Id = 112
        Me.FrmSafes.Name = "FrmSafes"
        '
        'BarButtonItem35
        '
        Me.BarButtonItem35.Caption = "العملات"
        Me.BarButtonItem35.Id = 113
        Me.BarButtonItem35.Name = "BarButtonItem35"
        '
        'BarButtonItem66
        '
        Me.BarButtonItem66.Caption = "إضافة نوع مصروف"
        Me.BarButtonItem66.Id = 175
        Me.BarButtonItem66.Name = "BarButtonItem66"
        '
        'BarButtonItem82
        '
        Me.BarButtonItem82.Caption = "المصارف"
        Me.BarButtonItem82.Id = 218
        Me.BarButtonItem82.Name = "BarButtonItem82"
        '
        'BarButtonItem67
        '
        Me.BarButtonItem67.Caption = "BarButtonItem67"
        Me.BarButtonItem67.Id = 177
        Me.BarButtonItem67.Name = "BarButtonItem67"
        '
        'BtnEmpAddBonus
        '
        Me.BtnEmpAddBonus.Caption = "إضافة علاوة لموظف"
        Me.BtnEmpAddBonus.Id = 96
        Me.BtnEmpAddBonus.Name = "BtnEmpAddBonus"
        '
        'BTNMPDISVAL
        '
        Me.BTNMPDISVAL.Caption = "خصم على موظف"
        Me.BTNMPDISVAL.Id = 89
        Me.BTNMPDISVAL.Name = "BTNMPDISVAL"
        '
        'BarSubItem8
        '
        Me.BarSubItem8.Caption = "الموظفين"
        Me.BarSubItem8.Id = 138
        Me.BarSubItem8.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem46), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem47), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem48), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem50)})
        Me.BarSubItem8.Name = "BarSubItem8"
        '
        'BarButtonItem46
        '
        Me.BarButtonItem46.Caption = "احتساب رواتب كل الموظفين"
        Me.BarButtonItem46.Id = 139
        Me.BarButtonItem46.Name = "BarButtonItem46"
        '
        'BarButtonItem47
        '
        Me.BarButtonItem47.Caption = "تعديل راتب موظف بعد الاحتساب"
        Me.BarButtonItem47.Id = 140
        Me.BarButtonItem47.Name = "BarButtonItem47"
        '
        'BarButtonItem48
        '
        Me.BarButtonItem48.Caption = "احتساب راتب موظف فردي"
        Me.BarButtonItem48.Id = 141
        Me.BarButtonItem48.Name = "BarButtonItem48"
        '
        'BarButtonItem50
        '
        Me.BarButtonItem50.Caption = "سلف الموظفين"
        Me.BarButtonItem50.Id = 149
        Me.BarButtonItem50.Name = "BarButtonItem50"
        '
        'BarButtonItem49
        '
        Me.BarButtonItem49.Caption = "إضافة موظف"
        Me.BarButtonItem49.Id = 147
        Me.BarButtonItem49.Name = "BarButtonItem49"
        '
        'BarButtonItem40
        '
        Me.BarButtonItem40.Caption = "سلفة لموظف"
        Me.BarButtonItem40.Id = 125
        Me.BarButtonItem40.Name = "BarButtonItem40"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "الموظفين"
        Me.BarButtonItem1.Id = 3
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'BarButtonItem8
        '
        Me.BarButtonItem8.Caption = "الشركة"
        Me.BarButtonItem8.Id = 10
        Me.BarButtonItem8.Name = "BarButtonItem8"
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "الفروع"
        Me.BarButtonItem3.Id = 5
        Me.BarButtonItem3.Name = "BarButtonItem3"
        '
        'BarButtonItem4
        '
        Me.BarButtonItem4.Caption = "الخزائن"
        Me.BarButtonItem4.Id = 6
        Me.BarButtonItem4.Name = "BarButtonItem4"
        '
        'BarButtonItem25
        '
        Me.BarButtonItem25.Caption = "العملات"
        Me.BarButtonItem25.Id = 76
        Me.BarButtonItem25.Name = "BarButtonItem25"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.ActAsDropDown = True
        Me.BarButtonItem2.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
        Me.BarButtonItem2.Caption = "العلاوات"
        Me.BarButtonItem2.Id = 4
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'RibbonControl1
        '
        Me.RibbonControl1.ApplicationCaption = "الواجهة الرئيسية"
        Me.RibbonControl1.CaptionBarItemLinks.Add(Me.BtnChangeUser)
        Me.RibbonControl1.EmptyAreaImageOptions.ImagePadding = New System.Windows.Forms.Padding(30, 32, 30, 32)
        Me.RibbonControl1.ExpandCollapseItem.Id = 0
        Me.RibbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BtnChangeUser, Me.RibbonControl1.ExpandCollapseItem, Me.BarEditItem1, Me.BarButtonItem1, Me.BarButtonItem2, Me.BarButtonItem3, Me.BarButtonItem4, Me.BarButtonItem7, Me.BarButtonItem8, Me.BarButtonItem9, Me.BarButtonGroup1, Me.BarSubItem1, Me.BarListItem1, Me.BarStaticItem1, Me.BarLinkContainerItem1, Me.BarMdiChildrenListItem1, Me.BarDockingMenuItem1, Me.BarButtonGroup2, Me.BarButtonGroup3, Me.BarSubItem2, Me.BarStaticItem2, Me.BarListItem2, Me.BarButtonItem10, Me.BtnBranchName, Me.BtnUserName, Me.BtnDate, Me.BtnTime, Me.BarEditItem2, Me.BarEditItem3, Me.BarEditItem4, Me.BarEditItem5, Me.BarButtonItem11, Me.BarButtonItem12, Me.BarButtonItem13, Me.BarButtonItem14, Me.BarEditItem6, Me.BtnCNNAME, Me.BtnCTNAME, Me.BarButtonItem15, Me.BarButtonItem16, Me.BarButtonItem17, Me.BarButtonItem18, Me.BGPBranches, Me.BGPAgents, Me.BarButtonItem20, Me.BarButtonItem21, Me.BtnViewCanceledTransfers, Me.BarButtonItem22, Me.BtnCancelRequest, Me.BarButtonItem25, Me.BarButtonItem27, Me.BtnCurrencyMovement, Me.BarButtonItem30, Me.BtnAgentCancelRequest, Me.BtnSelectAccountsBetweenBranches, Me.BTNADDDISCOUNTTYPE1, Me.DISCOUNTMENU, Me.BTNADDDISCOUNTTYPE, Me.BTNMPDISVAL, Me.BtnInCreases, Me.BtnAddBonusType, Me.BtnEmpAddBonus, Me.CUSTOMERSTATEMENTMENU, Me.BtnCustomerMovement, Me.CUSTOMERMENU, Me.BtnCustomer, Me.CompanyMenu, Me.BtnCoBranch, Me.FrmSafes, Me.BarButtonItem35, Me.BarSubItem5, Me.BarButtonItem36, Me.BarButtonItem37, Me.BarButtonItem38, Me.BarButtonItem39, Me.BarButtonItem40, Me.BarSubItem6, Me.BarButtonItem41, Me.BarSubItem7, Me.BarButtonItem42, Me.BarButtonItem43, Me.BarButtonItem44, Me.BarButtonItem45, Me.BarSubItem8, Me.BarButtonItem46, Me.BarButtonItem47, Me.BarButtonItem48, Me.BarButtonItem31, Me.BarButtonItem49, Me.BarButtonItem50, Me.BtnEMPLOYEE, Me.EMPSALARYMENU, Me.BtnCalcAllEmpSalary, Me.BtnINDIVDUALSALARYCALC, Me.BTNEMPCORRECTSLALRY, Me.BarSubItem10, Me.BarButtonItem58, Me.BarButtonItem59, Me.EMPSTATEMENTMENU, Me.BTNLOADSALARIES, Me.BtnAdvancePaymentLoadAllData, Me.BtnDiscountsLoadAllData, Me.BtnIncreaseLoadAllData, Me.BTNEMPORCUSTWITHDRAWALLoadAllData, Me.BtnIndividualSalaryEMP, Me.BarButtonItem66, Me.BarButtonItem67, Me.BarButtonItem69, Me.BarButtonItem70, Me.GeneralExpensesMenu, Me.BtnPettyCashStatement, Me.BtnExpenseStatement, Me.BrnClearFrom, Me.BILLPAYMENTMENU, Me.BtnEmpPayment, Me.BtnEmpDeposit, Me.BarButtonItem81, Me.BarButtonItem82, Me.BANKMENU, Me.BtnAddBank, Me.BtnAddBBranch, Me.BtnDelegate, Me.BANKDEPOORWITHDRAMENU, Me.BTNBANKDEPOSIT, Me.BtnUserAccessTemplate, Me.BTNEMPBANKWITHDRAWAL, Me.BANKSTATEMENTMENU, Me.BtnBBranchMovement, Me.BtnCompny, Me.CompanyInfo, Me.EmployeeClassification, Me.AddExpenses, Me.NATIONALITY, Me.BRANCHSTATEMENTMENU, Me.btnShowSafeMovement, Me.BtnMainSafeBalance, Me.BtnCurrencyStatement, Me.PETIESMENU, Me.BtnPettyCash, Me.btnPettyCashSettlement, Me.AddCurrency, Me.AddSafe, Me.BtnAddUser, Me.BtnOpeningBalance, Me.BtnANOTHEREXPENS, Me.BTNCURRENCYPRICE, Me.BarButtonItem5, Me.BarButtonItem6, Me.BarButtonItem19, Me.BarButtonItem24, Me.BarButtonItem26, Me.BarButtonItem28, Me.BarButtonItem29, Me.BarButtonItem32, Me.BarButtonItem33, Me.BarButtonItem34, Me.BarButtonItem52, Me.BarButtonItem53, Me.BarSubItem3, Me.BarButtonItem54, Me.BarButtonItem55, Me.BarSubItem4, Me.BarButtonItem56, Me.BarButtonItem57, Me.BarButtonItem60, Me.BarButtonItem61, Me.BarButtonItem62, Me.BarButtonItem63, Me.BarButtonItem64, Me.BarButtonItem65, Me.BarButtonItem71, Me.BarButtonItem72, Me.BarSubItem9, Me.BarButtonItem75, Me.BarButtonItem76, Me.BarButtonItem78, Me.BarButtonItem79, Me.BarButtonItem80, Me.BarButtonItem84, Me.BarSubItem11, Me.BarSubItem13, Me.BarButtonItem86, Me.BarButtonItem87, Me.BarButtonItem88, Me.BarSubItem14, Me.BarButtonItem91, Me.BarButtonItem92, Me.BarSubItem15, Me.BarButtonItem93, Me.BarButtonItem95, Me.BarSubItem16, Me.BarSubItem17, Me.BarSubItem18, Me.BarSubItem19, Me.BarSubItem20, Me.BarButtonItem98, Me.BarButtonItem99, Me.BarButtonItem100, Me.BarButtonItem101, Me.BarButtonItem102, Me.BarButtonItem103, Me.BarButtonItem104, Me.BarButtonItem105, Me.BarSubItem21, Me.BarSubItem22, Me.BarButtonItem106, Me.BarButtonItem108, Me.BarButtonItem109, Me.BarButtonItem110, Me.BtnAddCancelReason, Me.BarButtonItem112, Me.BarButtonItem113, Me.BarButtonItem114, Me.BarButtonItem115, Me.BarButtonItem116, Me.BarSubItem23, Me.BarButtonItem117, Me.BarButtonItem118, Me.BarSubItem24, Me.BarButtonItem68, Me.BarButtonItem74, Me.BarButtonItem119, Me.BarSubItem25, Me.BarButtonItem120, Me.BarButtonItem121, Me.BarSubItem26, Me.BarButtonItem51, Me.BarButtonItem122, Me.BarSubItem27, Me.BarButtonItem89, Me.BarButtonItem123, Me.BarSubItem28, Me.BarSubItem29, Me.BtnServiceType, Me.BarButtonItem90, Me.BarButtonItem94, Me.BarButtonItem124, Me.BarButtonItem125, Me.BarButtonItem126, Me.BarButtonItem107, Me.BarSubItem30, Me.BarSubItem31, Me.BarSubItem32, Me.BarSubItem33, Me.BarButtonGroup4, Me.BarSubItem34, Me.BarButtonItem77, Me.BarSubItem35, Me.BarButtonItem127, Me.BarButtonItem128, Me.BarButtonItem129, Me.BarButtonItem130, Me.BarButtonItem131, Me.BtnAddPartner, Me.BarButtonItem132, Me.BarButtonItem133, Me.BarButtonItem134, Me.BarButtonItem135, Me.BarSubItem36, Me.BarButtonItem136, Me.BarButtonItem137, Me.BarButtonItem139, Me.BarButtonItem140, Me.BarButtonItem141, Me.BarButtonItem142, Me.BarSubItem37, Me.BarButtonItem143, Me.BarButtonItem144, Me.BarButtonItem147, Me.BarButtonItem148, Me.BarButtonItem149, Me.BarButtonItem150, Me.BarButtonItem151, Me.BtnAddProject, Me.BtnProjectPartner, Me.BtnAddPettyCash, Me.BtnPettySettlement, Me.BtnAnotherExpense, Me.BtnAddProExpense, Me.BtnAddAssest, Me.AddBasiscMenu, Me.BtnProAddPettyCash, Me.BtnProPayPetty, Me.BarButtonItem152, Me.BarButtonItem153, Me.BtnContractorPayment, Me.BarSubItem39, Me.BarButtonItem154, Me.BarButtonItem155, Me.BarButtonItem156, Me.BarButtonItem157, Me.BarButtonItem158, Me.BarSubItem40, Me.BarButtonItem159, Me.BarButtonItem160, Me.BarButtonItem161, Me.BarButtonItem162, Me.BarButtonItem163, Me.BarButtonItem164, Me.BtnAddItem, Me.BtnAddItemDetails, Me.BtnAddSupplier, Me.BtnImportItem, Me.BarButtonItem165, Me.BarButtonItem166, Me.BtnPROEXPORTITEM, Me.BarButtonItem167, Me.BarButtonItem168, Me.BarButtonItem169, Me.BarButtonItem170, Me.BarButtonItem171, Me.BarButtonItem172, Me.BarButtonItem173, Me.BarButtonItem174, Me.BarButtonItem175, Me.BarButtonItem176, Me.BarButtonItem177, Me.BarButtonItem178, Me.BtnLeave, Me.BarButtonItem179, Me.BarButtonItem180, Me.BarButtonItem181, Me.BarButtonItem182, Me.BarButtonItem183, Me.BarButtonItem184, Me.BarButtonItem185, Me.BarSubItem12, Me.BarButtonItem186, Me.BarButtonItem187, Me.BarSubItem41, Me.BarButtonItem188, Me.BarButtonItem189, Me.BarButtonItem190, Me.BarButtonItem191, Me.BarButtonItem192, Me.BarButtonItem193, Me.BarButtonItem194, Me.BarButtonItem195, Me.BarButtonItem196, Me.BarButtonItem197, Me.BarButtonItem198, Me.BarButtonItem200, Me.BarButtonItem73, Me.MoActivetion, Me.BarButtonItem855, Me.BarButtonItem85585, Me.BarButtonItem85, Me.BarButtonItem83, Me.BarButtonItem96, Me.BarButtonItem97, Me.BarButtonItem138, Me.BarButtonItem145, Me.BarButtonItem146, Me.BarButtonItem199, Me.BarButtonItem201, Me.BarButtonItem202, Me.BarButtonItem203, Me.BarButtonItem204, Me.BarButtonItem205, Me.BarButtonItem206, Me.BarButtonItem207, Me.BarButtonItem208, Me.BarButtonItem209, Me.BarButtonItem210, Me.BarSubItem38, Me.BarButtonItem211, Me.BarButtonItem212, Me.BarButtonItem213, Me.BarButtonItem214, Me.BarButtonItem215, Me.BarButtonItem216, Me.BarButtonItem217})
        Me.RibbonControl1.Location = New System.Drawing.Point(0, 0)
        Me.RibbonControl1.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.RibbonControl1.MaxItemId = 755
        Me.RibbonControl1.Name = "RibbonControl1"
        Me.RibbonControl1.OptionsSearchMenu.SearchItemPosition = DevExpress.XtraBars.Ribbon.SearchItemPosition.Caption
        Me.RibbonControl1.PageCategories.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageCategory() {Me.RibbonPageCategory1, Me.RibbonPageCategory2})
        Me.RibbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.RPBASICINFO, Me.RP2, Me.RibbonPage1, Me.RP5, Me.RP3, Me.RP4, Me.RibbonPage6, Me.AssGrroup, Me.RP6, Me.RibbonPage3})
        Me.RibbonControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit1, Me.RepositoryItemButtonEdit2, Me.RepositoryItemPictureEdit2, Me.RepositoryItemPictureEdit3, Me.RepositoryItemImageEdit1, Me.RepositoryItemPictureEdit4, Me.RepositoryItemHypertextLabel1})
        Me.RibbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal
        Me.RibbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.[False]
        Me.RibbonControl1.ShowItemCaptionsInCaptionBar = True
        Me.RibbonControl1.ShowItemCaptionsInPageHeader = True
        Me.RibbonControl1.ShowItemCaptionsInQAT = True
        Me.RibbonControl1.ShowToolbarCustomizeItem = False
        Me.RibbonControl1.Size = New System.Drawing.Size(1938, 180)
        Me.RibbonControl1.StatusBar = Me.RibbonStatusBar1
        Me.RibbonControl1.Toolbar.ShowCustomizeItem = False
        '
        'BtnChangeUser
        '
        Me.BtnChangeUser.ActAsDropDown = True
        Me.BtnChangeUser.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
        Me.BtnChangeUser.Id = 47
        Me.BtnChangeUser.ImageOptions.SvgImageSize = New System.Drawing.Size(64, 64)
        Me.BtnChangeUser.ItemAppearance.Disabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnChangeUser.ItemAppearance.Disabled.Options.UseFont = True
        Me.BtnChangeUser.Name = "BtnChangeUser"
        '
        'BarEditItem1
        '
        Me.BarEditItem1.Caption = "الموظفين"
        Me.BarEditItem1.Edit = Me.RepositoryItemPictureEdit1
        Me.BarEditItem1.Id = 2
        Me.BarEditItem1.ImageOptions.SvgImageSize = New System.Drawing.Size(48, 48)
        Me.BarEditItem1.Name = "BarEditItem1"
        '
        'BarButtonItem7
        '
        Me.BarButtonItem7.Caption = "نموذج تقسيم العمولات"
        Me.BarButtonItem7.Id = 9
        Me.BarButtonItem7.Name = "BarButtonItem7"
        '
        'BarButtonItem9
        '
        Me.BarButtonItem9.Caption = "نموذج توزيع العمولات حسب الوكيل"
        Me.BarButtonItem9.Id = 11
        Me.BarButtonItem9.Name = "BarButtonItem9"
        '
        'BarButtonGroup1
        '
        Me.BarButtonGroup1.Caption = "BarButtonGroup1"
        Me.BarButtonGroup1.Id = 12
        Me.BarButtonGroup1.Name = "BarButtonGroup1"
        '
        'BarSubItem1
        '
        Me.BarSubItem1.Caption = "نماذج تقسيم العمولة"
        ToolTipTitleItem1.Text = "تقسيم العمولة حسب الفرع"
        ToolTipTitleItem2.Text = "تقسيم العمولة حسب الوكلاء"
        SuperToolTip1.Items.Add(ToolTipTitleItem1)
        SuperToolTip1.Items.Add(ToolTipTitleItem2)
        Me.BarSubItem1.DropDownSuperTip = SuperToolTip1
        Me.BarSubItem1.Id = 13
        Me.BarSubItem1.Name = "BarSubItem1"
        '
        'BarListItem1
        '
        Me.BarListItem1.Caption = "نماذج تقسيم العمولة"
        Me.BarListItem1.Id = 14
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Disabled.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Disabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Disabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Hovered.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Hovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Hovered.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Normal.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Pressed.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Pressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.AppearanceMenu.Pressed.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.HeaderItemAppearance.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.HeaderItemAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.HeaderItemAppearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.MenuBar.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.MenuBar.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.MenuBar.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.MenuCaption.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.MenuCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.MenuCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.SideStrip.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.SideStrip.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.SideStrip.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.MenuAppearance.SideStripNonRecent.Options.UseTextOptions = True
        Me.BarListItem1.MenuAppearance.SideStripNonRecent.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BarListItem1.MenuAppearance.SideStripNonRecent.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BarListItem1.Name = "BarListItem1"
        Me.BarListItem1.Strings.AddRange(New Object() {"تقسيم العمولة حسب الفروع", "تقسيم العمولة حسب الوكلاء"})
        '
        'BarStaticItem1
        '
        Me.BarStaticItem1.Caption = "BarStaticItem1"
        Me.BarStaticItem1.Id = 15
        Me.BarStaticItem1.Name = "BarStaticItem1"
        Me.BarStaticItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
        '
        'BarLinkContainerItem1
        '
        Me.BarLinkContainerItem1.Caption = "BarLinkContainerItem1"
        Me.BarLinkContainerItem1.Id = 16
        Me.BarLinkContainerItem1.Name = "BarLinkContainerItem1"
        Me.BarLinkContainerItem1.OptionsMultiColumn.ColumnCount = 2
        Me.BarLinkContainerItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph
        '
        'BarMdiChildrenListItem1
        '
        Me.BarMdiChildrenListItem1.Caption = "BarMdiChildrenListItem1"
        Me.BarMdiChildrenListItem1.Id = 17
        Me.BarMdiChildrenListItem1.Name = "BarMdiChildrenListItem1"
        '
        'BarDockingMenuItem1
        '
        Me.BarDockingMenuItem1.Caption = "BarDockingMenuItem1"
        Me.BarDockingMenuItem1.Id = 18
        Me.BarDockingMenuItem1.Name = "BarDockingMenuItem1"
        '
        'BarButtonGroup2
        '
        Me.BarButtonGroup2.Caption = "BarButtonGroup2"
        Me.BarButtonGroup2.Id = 19
        Me.BarButtonGroup2.Name = "BarButtonGroup2"
        '
        'BarButtonGroup3
        '
        Me.BarButtonGroup3.Caption = "BarButtonGroup3"
        Me.BarButtonGroup3.Id = 20
        Me.BarButtonGroup3.Name = "BarButtonGroup3"
        '
        'BarSubItem2
        '
        Me.BarSubItem2.Caption = "BarSubItem2"
        Me.BarSubItem2.Id = 22
        Me.BarSubItem2.Name = "BarSubItem2"
        '
        'BarStaticItem2
        '
        Me.BarStaticItem2.Caption = "BarStaticItem2"
        Me.BarStaticItem2.Id = 23
        Me.BarStaticItem2.Name = "BarStaticItem2"
        '
        'BarListItem2
        '
        Me.BarListItem2.Caption = "نماذج تقسيم العمولة"
        Me.BarListItem2.Id = 24
        Me.BarListItem2.Name = "BarListItem2"
        Me.BarListItem2.Strings.AddRange(New Object() {"تقسيم العمولة حسب الفروع", "تقسيم العمولة حسب الوكالات"})
        '
        'BarButtonItem10
        '
        Me.BarButtonItem10.ActAsDropDown = True
        Me.BarButtonItem10.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
        Me.BarButtonItem10.Caption = "BarButtonItem10"
        Me.BarButtonItem10.Id = 25
        Me.BarButtonItem10.Name = "BarButtonItem10"
        '
        'BtnBranchName
        '
        Me.BtnBranchName.Caption = " "
        Me.BtnBranchName.Id = 30
        Me.BtnBranchName.Name = "BtnBranchName"
        '
        'BtnUserName
        '
        Me.BtnUserName.Caption = " "
        Me.BtnUserName.Id = 31
        Me.BtnUserName.Name = "BtnUserName"
        '
        'BtnDate
        '
        Me.BtnDate.Caption = " "
        Me.BtnDate.Id = 32
        Me.BtnDate.Name = "BtnDate"
        '
        'BtnTime
        '
        Me.BtnTime.Caption = " "
        Me.BtnTime.Id = 33
        Me.BtnTime.Name = "BtnTime"
        '
        'BarEditItem2
        '
        Me.BarEditItem2.Caption = "BarEditItem2"
        Me.BarEditItem2.Edit = Me.RepositoryItemPictureEdit2
        Me.BarEditItem2.Id = 34
        Me.BarEditItem2.Name = "BarEditItem2"
        '
        'RepositoryItemPictureEdit2
        '
        Me.RepositoryItemPictureEdit2.Name = "RepositoryItemPictureEdit2"
        '
        'BarEditItem3
        '
        Me.BarEditItem3.Caption = "اسم المستخدم"
        Me.BarEditItem3.Edit = Me.RepositoryItemPictureEdit3
        Me.BarEditItem3.Id = 35
        Me.BarEditItem3.Name = "BarEditItem3"
        '
        'RepositoryItemPictureEdit3
        '
        Me.RepositoryItemPictureEdit3.Caption.Alignment = System.Drawing.ContentAlignment.MiddleRight
        Me.RepositoryItemPictureEdit3.Name = "RepositoryItemPictureEdit3"
        '
        'BarEditItem4
        '
        Me.BarEditItem4.Caption = " "
        Me.BarEditItem4.Edit = Me.RepositoryItemImageEdit1
        Me.BarEditItem4.Id = 36
        Me.BarEditItem4.Name = "BarEditItem4"
        '
        'RepositoryItemImageEdit1
        '
        Me.RepositoryItemImageEdit1.AutoHeight = False
        Me.RepositoryItemImageEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageEdit1.Name = "RepositoryItemImageEdit1"
        '
        'BarEditItem5
        '
        Me.BarEditItem5.Caption = " "
        Me.BarEditItem5.Edit = Me.RepositoryItemPictureEdit4
        Me.BarEditItem5.Id = 37
        Me.BarEditItem5.Name = "BarEditItem5"
        '
        'RepositoryItemPictureEdit4
        '
        Me.RepositoryItemPictureEdit4.EnableLODImages = True
        Me.RepositoryItemPictureEdit4.Name = "RepositoryItemPictureEdit4"
        '
        'BarButtonItem11
        '
        Me.BarButtonItem11.Caption = " "
        Me.BarButtonItem11.Id = 38
        Me.BarButtonItem11.Name = "BarButtonItem11"
        '
        'BarButtonItem12
        '
        Me.BarButtonItem12.Caption = " "
        Me.BarButtonItem12.Id = 39
        Me.BarButtonItem12.Name = "BarButtonItem12"
        '
        'BarButtonItem13
        '
        Me.BarButtonItem13.Caption = " "
        Me.BarButtonItem13.Id = 40
        Me.BarButtonItem13.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem13.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem13.Name = "BarButtonItem13"
        '
        'BarButtonItem14
        '
        Me.BarButtonItem14.Caption = " "
        Me.BarButtonItem14.Id = 41
        Me.BarButtonItem14.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem14.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem14.Name = "BarButtonItem14"
        '
        'BarEditItem6
        '
        Me.BarEditItem6.Caption = " "
        Me.BarEditItem6.Edit = Me.RepositoryItemHypertextLabel1
        Me.BarEditItem6.Id = 42
        Me.BarEditItem6.Name = "BarEditItem6"
        '
        'RepositoryItemHypertextLabel1
        '
        Me.RepositoryItemHypertextLabel1.Name = "RepositoryItemHypertextLabel1"
        '
        'BtnCNNAME
        '
        Me.BtnCNNAME.Caption = " "
        Me.BtnCNNAME.Id = 43
        Me.BtnCNNAME.Name = "BtnCNNAME"
        '
        'BtnCTNAME
        '
        Me.BtnCTNAME.Caption = " "
        Me.BtnCTNAME.Id = 44
        Me.BtnCTNAME.Name = "BtnCTNAME"
        '
        'BarButtonItem15
        '
        Me.BarButtonItem15.Caption = " "
        Me.BarButtonItem15.Id = 45
        Me.BarButtonItem15.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem15.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem15.Name = "BarButtonItem15"
        '
        'BarButtonItem16
        '
        Me.BarButtonItem16.Caption = " "
        Me.BarButtonItem16.Id = 46
        Me.BarButtonItem16.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem16.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem16.Name = "BarButtonItem16"
        '
        'BarButtonItem17
        '
        Me.BarButtonItem17.Caption = "تبديل المستخدم"
        Me.BarButtonItem17.Id = 48
        Me.BarButtonItem17.Name = "BarButtonItem17"
        '
        'BarButtonItem18
        '
        Me.BarButtonItem18.Caption = "الخروج من البرنامج"
        Me.BarButtonItem18.Id = 49
        Me.BarButtonItem18.Name = "BarButtonItem18"
        '
        'BGPBranches
        '
        Me.BGPBranches.Caption = "تقسيم العمولة حسب الفرع"
        Me.BGPBranches.Id = 51
        Me.BGPBranches.ImageOptions.SvgImage = CType(resources.GetObject("BGPBranches.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BGPBranches.Name = "BGPBranches"
        Me.BGPBranches.Tag = "BGPBranches"
        '
        'BGPAgents
        '
        Me.BGPAgents.Caption = "تقسيم العمولة حسب الوكيل"
        Me.BGPAgents.Id = 52
        Me.BGPAgents.ImageOptions.SvgImage = CType(resources.GetObject("BGPAgents.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BGPAgents.Name = "BGPAgents"
        Me.BGPAgents.Tag = "BGPAgents"
        '
        'BarButtonItem20
        '
        Me.BarButtonItem20.Caption = "عرض الإيرادات"
        Me.BarButtonItem20.Id = 60
        Me.BarButtonItem20.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem20.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem20.Name = "BarButtonItem20"
        '
        'BarButtonItem21
        '
        Me.BarButtonItem21.Caption = "تقديم طلب إلغاء حوالة"
        Me.BarButtonItem21.Id = 72
        Me.BarButtonItem21.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem21.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem21.Name = "BarButtonItem21"
        '
        'BarButtonItem22
        '
        Me.BarButtonItem22.Caption = "أرصدة الخزائن الرئيسية"
        Me.BarButtonItem22.Id = 74
        Me.BarButtonItem22.Name = "BarButtonItem22"
        '
        'BarButtonItem27
        '
        Me.BarButtonItem27.Caption = "نقل من الخزنة الرئيسية لخزنة موظف"
        Me.BarButtonItem27.Id = 78
        Me.BarButtonItem27.Name = "BarButtonItem27"
        '
        'BtnCurrencyMovement
        '
        Me.BtnCurrencyMovement.Caption = "عرض حركات العملة"
        Me.BtnCurrencyMovement.Id = 79
        Me.BtnCurrencyMovement.ImageOptions.Image = CType(resources.GetObject("BtnCurrencyMovement.ImageOptions.Image"), System.Drawing.Image)
        Me.BtnCurrencyMovement.Name = "BtnCurrencyMovement"
        Me.BtnCurrencyMovement.Tag = 28
        '
        'BarButtonItem30
        '
        Me.BarButtonItem30.Caption = "حركة خزنة موظف"
        Me.BarButtonItem30.Id = 81
        Me.BarButtonItem30.Name = "BarButtonItem30"
        '
        'BtnSelectAccountsBetweenBranches
        '
        Me.BtnSelectAccountsBetweenBranches.Caption = "مطابقة الجواري"
        Me.BtnSelectAccountsBetweenBranches.Id = 83
        Me.BtnSelectAccountsBetweenBranches.ImageOptions.SvgImage = CType(resources.GetObject("BtnSelectAccountsBetweenBranches.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnSelectAccountsBetweenBranches.Name = "BtnSelectAccountsBetweenBranches"
        '
        'BTNADDDISCOUNTTYPE1
        '
        Me.BTNADDDISCOUNTTYPE1.Caption = "إضافة نوع خصم"
        Me.BTNADDDISCOUNTTYPE1.Id = 84
        Me.BTNADDDISCOUNTTYPE1.ImageOptions.SvgImage = CType(resources.GetObject("BTNADDDISCOUNTTYPE1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BTNADDDISCOUNTTYPE1.Name = "BTNADDDISCOUNTTYPE1"
        '
        'CUSTOMERSTATEMENTMENU
        '
        Me.CUSTOMERSTATEMENTMENU.Caption = "كشوفات حسابات"
        Me.CUSTOMERSTATEMENTMENU.Id = 103
        Me.CUSTOMERSTATEMENTMENU.ImageOptions.SvgImage = CType(resources.GetObject("CUSTOMERSTATEMENTMENU.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.CUSTOMERSTATEMENTMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnCustomerMovement), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem62), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem80), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem116), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem115), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem129), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem132), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem147), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem197), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem97)})
        Me.CUSTOMERSTATEMENTMENU.Name = "CUSTOMERSTATEMENTMENU"
        Me.CUSTOMERSTATEMENTMENU.Tag = 32
        '
        'BtnCustomerMovement
        '
        Me.BtnCustomerMovement.Caption = "كشف حساب عميل"
        Me.BtnCustomerMovement.Id = 104
        Me.BtnCustomerMovement.ImageOptions.SvgImage = CType(resources.GetObject("BtnCustomerMovement.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnCustomerMovement.ImageOptions.SvgImageSize = New System.Drawing.Size(22, 22)
        Me.BtnCustomerMovement.Name = "BtnCustomerMovement"
        Me.BtnCustomerMovement.Tag = 33
        '
        'BarButtonItem62
        '
        Me.BarButtonItem62.Caption = "حسابات العملاء"
        Me.BarButtonItem62.Id = 353
        Me.BarButtonItem62.Name = "BarButtonItem62"
        '
        'BarButtonItem80
        '
        Me.BarButtonItem80.Caption = "حسابات المدينون"
        Me.BarButtonItem80.Id = 390
        Me.BarButtonItem80.Name = "BarButtonItem80"
        '
        'BarButtonItem116
        '
        Me.BarButtonItem116.Caption = "حسابات الـوكلاء"
        Me.BarButtonItem116.Id = 507
        Me.BarButtonItem116.Name = "BarButtonItem116"
        '
        'BarButtonItem115
        '
        Me.BarButtonItem115.Caption = " حسابات الجواري"
        Me.BarButtonItem115.Id = 505
        Me.BarButtonItem115.Name = "BarButtonItem115"
        '
        'BarButtonItem129
        '
        Me.BarButtonItem129.Caption = "كشف بكل العملاء"
        Me.BarButtonItem129.Id = 622
        Me.BarButtonItem129.Name = "BarButtonItem129"
        '
        'BarButtonItem132
        '
        Me.BarButtonItem132.Caption = "كشف حسابات كل الوكلاء"
        Me.BarButtonItem132.Id = 628
        Me.BarButtonItem132.Name = "BarButtonItem132"
        '
        'BarButtonItem147
        '
        Me.BarButtonItem147.Caption = "كشف حساب شريك"
        Me.BarButtonItem147.Id = 648
        Me.BarButtonItem147.Name = "BarButtonItem147"
        '
        'BarButtonItem197
        '
        Me.BarButtonItem197.Caption = "كشف حساب مندوبين"
        Me.BarButtonItem197.Id = 721
        Me.BarButtonItem197.Name = "BarButtonItem197"
        '
        'BarButtonItem97
        '
        Me.BarButtonItem97.Caption = "كشف حسابات المستثمرين"
        Me.BarButtonItem97.Id = 732
        Me.BarButtonItem97.Name = "BarButtonItem97"
        '
        'BarSubItem7
        '
        Me.BarSubItem7.Caption = "استعلامات الموظفين"
        Me.BarSubItem7.Id = 130
        Me.BarSubItem7.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem42), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem43), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem44), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem45), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem31)})
        Me.BarSubItem7.Name = "BarSubItem7"
        '
        'BarButtonItem42
        '
        Me.BarButtonItem42.Caption = "استعلام عن العلاوات"
        Me.BarButtonItem42.Id = 131
        Me.BarButtonItem42.Name = "BarButtonItem42"
        '
        'BarButtonItem43
        '
        Me.BarButtonItem43.Caption = "استعلام عن الخصميات"
        Me.BarButtonItem43.Id = 132
        Me.BarButtonItem43.Name = "BarButtonItem43"
        '
        'BarButtonItem44
        '
        Me.BarButtonItem44.Caption = "استعلام عن السلف"
        Me.BarButtonItem44.Id = 134
        Me.BarButtonItem44.Name = "BarButtonItem44"
        '
        'BarButtonItem45
        '
        Me.BarButtonItem45.Caption = "حركة السحب والإيداع"
        Me.BarButtonItem45.Id = 136
        Me.BarButtonItem45.Name = "BarButtonItem45"
        '
        'BarButtonItem31
        '
        Me.BarButtonItem31.Caption = "كشف حساب موظف"
        Me.BarButtonItem31.Id = 145
        Me.BarButtonItem31.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem31.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem31.Name = "BarButtonItem31"
        '
        'BtnEMPLOYEE
        '
        Me.BtnEMPLOYEE.ActAsDropDown = True
        Me.BtnEMPLOYEE.Caption = "الموظفين"
        Me.BtnEMPLOYEE.Id = 153
        Me.BtnEMPLOYEE.ImageOptions.SvgImage = CType(resources.GetObject("BtnEMPLOYEE.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnEMPLOYEE.Name = "BtnEMPLOYEE"
        Me.BtnEMPLOYEE.Tag = 39
        '
        'EMPSALARYMENU
        '
        Me.EMPSALARYMENU.Caption = "المرتبات"
        Me.EMPSALARYMENU.Id = 154
        Me.EMPSALARYMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnCalcAllEmpSalary), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnINDIVDUALSALARYCALC), New DevExpress.XtraBars.LinkPersistInfo(Me.BTNEMPCORRECTSLALRY), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnIndividualSalaryEMP), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem83)})
        Me.EMPSALARYMENU.Name = "EMPSALARYMENU"
        Me.EMPSALARYMENU.Tag = 40
        '
        'BtnCalcAllEmpSalary
        '
        Me.BtnCalcAllEmpSalary.Caption = "احتساب رواتب كل الموظفين"
        Me.BtnCalcAllEmpSalary.Id = 155
        Me.BtnCalcAllEmpSalary.Name = "BtnCalcAllEmpSalary"
        Me.BtnCalcAllEmpSalary.Tag = 41
        '
        'BtnINDIVDUALSALARYCALC
        '
        Me.BtnINDIVDUALSALARYCALC.Caption = "إخلاء طرف موظف"
        Me.BtnINDIVDUALSALARYCALC.Id = 156
        Me.BtnINDIVDUALSALARYCALC.Name = "BtnINDIVDUALSALARYCALC"
        Me.BtnINDIVDUALSALARYCALC.Tag = 42
        '
        'BTNEMPCORRECTSLALRY
        '
        Me.BTNEMPCORRECTSLALRY.Caption = "معالجة خطأ في احتساب راتب"
        Me.BTNEMPCORRECTSLALRY.Id = 157
        Me.BTNEMPCORRECTSLALRY.Name = "BTNEMPCORRECTSLALRY"
        Me.BTNEMPCORRECTSLALRY.Tag = 43
        '
        'BtnIndividualSalaryEMP
        '
        Me.BtnIndividualSalaryEMP.Caption = "احتساب راتب فردي"
        Me.BtnIndividualSalaryEMP.Id = 173
        Me.BtnIndividualSalaryEMP.Name = "BtnIndividualSalaryEMP"
        Me.BtnIndividualSalaryEMP.Tag = 44
        '
        'BarButtonItem83
        '
        Me.BarButtonItem83.Caption = "حوافظ مصرفية"
        Me.BarButtonItem83.Id = 730
        Me.BarButtonItem83.Name = "BarButtonItem83"
        '
        'BarSubItem10
        '
        Me.BarSubItem10.Caption = "السحب والإيداع"
        Me.BarSubItem10.Id = 161
        Me.BarSubItem10.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem58), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem59)})
        Me.BarSubItem10.Name = "BarSubItem10"
        '
        'BarButtonItem58
        '
        Me.BarButtonItem58.Caption = "سحب من حساب موظف"
        Me.BarButtonItem58.Id = 162
        Me.BarButtonItem58.Name = "BarButtonItem58"
        '
        'BarButtonItem59
        '
        Me.BarButtonItem59.Caption = "إيداع في حساب موظف"
        Me.BarButtonItem59.Id = 163
        Me.BarButtonItem59.Name = "BarButtonItem59"
        '
        'EMPSTATEMENTMENU
        '
        Me.EMPSTATEMENTMENU.Caption = "تقارير الموظفين"
        Me.EMPSTATEMENTMENU.Id = 165
        Me.EMPSTATEMENTMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BTNLOADSALARIES), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnDiscountsLoadAllData), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnIncreaseLoadAllData), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem134), New DevExpress.XtraBars.LinkPersistInfo(Me.BTNEMPORCUSTWITHDRAWALLoadAllData), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem63), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem171), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem179), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem190), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem192)})
        Me.EMPSTATEMENTMENU.Name = "EMPSTATEMENTMENU"
        Me.EMPSTATEMENTMENU.Tag = 48
        '
        'BTNLOADSALARIES
        '
        Me.BTNLOADSALARIES.Caption = "كشف حركة موظف"
        Me.BTNLOADSALARIES.Id = 166
        Me.BTNLOADSALARIES.ImageOptions.SvgImage = CType(resources.GetObject("BTNLOADSALARIES.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BTNLOADSALARIES.Name = "BTNLOADSALARIES"
        Me.BTNLOADSALARIES.Tag = 49
        '
        'BtnDiscountsLoadAllData
        '
        Me.BtnDiscountsLoadAllData.Caption = "استعلام الخصميات"
        Me.BtnDiscountsLoadAllData.Id = 168
        Me.BtnDiscountsLoadAllData.ImageOptions.SvgImage = CType(resources.GetObject("BtnDiscountsLoadAllData.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnDiscountsLoadAllData.Name = "BtnDiscountsLoadAllData"
        Me.BtnDiscountsLoadAllData.Tag = 51
        '
        'BtnIncreaseLoadAllData
        '
        Me.BtnIncreaseLoadAllData.Caption = "استعلام العلاوات"
        Me.BtnIncreaseLoadAllData.Id = 169
        Me.BtnIncreaseLoadAllData.ImageOptions.SvgImage = CType(resources.GetObject("BtnIncreaseLoadAllData.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnIncreaseLoadAllData.Name = "BtnIncreaseLoadAllData"
        Me.BtnIncreaseLoadAllData.Tag = 52
        '
        'BarButtonItem134
        '
        Me.BarButtonItem134.Caption = "استعلام السلف"
        Me.BarButtonItem134.Id = 630
        Me.BarButtonItem134.Name = "BarButtonItem134"
        '
        'BTNEMPORCUSTWITHDRAWALLoadAllData
        '
        Me.BTNEMPORCUSTWITHDRAWALLoadAllData.Caption = "استعلام السحب والإيداع"
        Me.BTNEMPORCUSTWITHDRAWALLoadAllData.Id = 170
        Me.BTNEMPORCUSTWITHDRAWALLoadAllData.ImageOptions.SvgImage = CType(resources.GetObject("BTNEMPORCUSTWITHDRAWALLoadAllData.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BTNEMPORCUSTWITHDRAWALLoadAllData.Name = "BTNEMPORCUSTWITHDRAWALLoadAllData"
        Me.BTNEMPORCUSTWITHDRAWALLoadAllData.Tag = 53
        '
        'BarButtonItem6
        '
        Me.BarButtonItem6.Caption = "كل الموظفين"
        Me.BarButtonItem6.Id = 311
        Me.BarButtonItem6.Name = "BarButtonItem6"
        '
        'BarButtonItem63
        '
        Me.BarButtonItem63.Caption = "كشف حسابات الموظفين"
        Me.BarButtonItem63.Id = 356
        Me.BarButtonItem63.Name = "BarButtonItem63"
        '
        'BarButtonItem171
        '
        Me.BarButtonItem171.Caption = "كشف مرتبات الموظفين"
        Me.BarButtonItem171.Id = 692
        Me.BarButtonItem171.Name = "BarButtonItem171"
        '
        'BarButtonItem179
        '
        Me.BarButtonItem179.Caption = "حركة الخزائن"
        Me.BarButtonItem179.Id = 701
        Me.BarButtonItem179.Name = "BarButtonItem179"
        '
        'BarButtonItem190
        '
        Me.BarButtonItem190.Caption = "استعلام الإجازات"
        Me.BarButtonItem190.Id = 714
        Me.BarButtonItem190.Name = "BarButtonItem190"
        '
        'BarButtonItem192
        '
        Me.BarButtonItem192.Caption = "العلاوات والخصميات الجماعية"
        Me.BarButtonItem192.Id = 716
        Me.BarButtonItem192.Name = "BarButtonItem192"
        '
        'BtnAdvancePaymentLoadAllData
        '
        Me.BtnAdvancePaymentLoadAllData.Caption = "استعلام السلف"
        Me.BtnAdvancePaymentLoadAllData.Id = 167
        Me.BtnAdvancePaymentLoadAllData.ImageOptions.SvgImage = CType(resources.GetObject("BtnAdvancePaymentLoadAllData.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnAdvancePaymentLoadAllData.Name = "BtnAdvancePaymentLoadAllData"
        Me.BtnAdvancePaymentLoadAllData.Tag = 50
        '
        'BarButtonItem69
        '
        Me.BarButtonItem69.Caption = "تسوية عهدة"
        Me.BarButtonItem69.Id = 183
        Me.BarButtonItem69.Name = "BarButtonItem69"
        '
        'BarButtonItem70
        '
        Me.BarButtonItem70.Caption = "BarButtonItem70"
        Me.BarButtonItem70.Id = 186
        Me.BarButtonItem70.Name = "BarButtonItem70"
        '
        'GeneralExpensesMenu
        '
        Me.GeneralExpensesMenu.Caption = "المصروفات العمومية"
        Me.GeneralExpensesMenu.Id = 188
        Me.GeneralExpensesMenu.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnPettyCashStatement), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnExpenseStatement), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem26)})
        Me.GeneralExpensesMenu.Name = "GeneralExpensesMenu"
        Me.GeneralExpensesMenu.Tag = 34
        '
        'BtnPettyCashStatement
        '
        Me.BtnPettyCashStatement.Caption = "استعلام تسوية العهد"
        Me.BtnPettyCashStatement.Id = 189
        Me.BtnPettyCashStatement.Name = "BtnPettyCashStatement"
        Me.BtnPettyCashStatement.Tag = 35
        '
        'BtnExpenseStatement
        '
        Me.BtnExpenseStatement.Caption = "استعلام حركة المصروفات"
        Me.BtnExpenseStatement.Id = 192
        Me.BtnExpenseStatement.Name = "BtnExpenseStatement"
        Me.BtnExpenseStatement.Tag = 36
        '
        'BarButtonItem26
        '
        Me.BarButtonItem26.Caption = "استعلام عن حركة مصروف"
        Me.BarButtonItem26.Id = 316
        Me.BarButtonItem26.Name = "BarButtonItem26"
        '
        'BILLPAYMENTMENU
        '
        Me.BILLPAYMENTMENU.Caption = "العمليات الماليه"
        Me.BILLPAYMENTMENU.Id = 202
        Me.BILLPAYMENTMENU.ImageOptions.SvgImage = CType(resources.GetObject("BILLPAYMENTMENU.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BILLPAYMENTMENU.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BILLPAYMENTMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnEmpDeposit), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnEmpPayment), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem107), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem150), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem138), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem203), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem206), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem207), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem208), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem209)})
        Me.BILLPAYMENTMENU.Name = "BILLPAYMENTMENU"
        Me.BILLPAYMENTMENU.Tag = 58
        '
        'BtnEmpDeposit
        '
        Me.BtnEmpDeposit.Caption = "سند قبض"
        Me.BtnEmpDeposit.Id = 206
        Me.BtnEmpDeposit.ImageOptions.SvgImage = CType(resources.GetObject("BtnEmpDeposit.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnEmpDeposit.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BtnEmpDeposit.Name = "BtnEmpDeposit"
        Me.BtnEmpDeposit.Tag = 62
        '
        'BtnEmpPayment
        '
        Me.BtnEmpPayment.Caption = "سند صرف"
        Me.BtnEmpPayment.Id = 204
        Me.BtnEmpPayment.ImageOptions.SvgImage = CType(resources.GetObject("BtnEmpPayment.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnEmpPayment.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BtnEmpPayment.Name = "BtnEmpPayment"
        Me.BtnEmpPayment.Tag = 59
        '
        'BarButtonItem107
        '
        Me.BarButtonItem107.Caption = "صرف لموظف بدون رصيد"
        Me.BarButtonItem107.Id = 577
        Me.BarButtonItem107.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem107.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem107.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem107.Name = "BarButtonItem107"
        '
        'BarButtonItem150
        '
        Me.BarButtonItem150.Caption = "تحويل بين الحسابات"
        Me.BarButtonItem150.Id = 652
        Me.BarButtonItem150.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem150.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem150.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem150.Name = "BarButtonItem150"
        '
        'BarButtonItem138
        '
        Me.BarButtonItem138.Caption = "تحويل بين النقدي والمصرف"
        Me.BarButtonItem138.Id = 733
        Me.BarButtonItem138.Name = "BarButtonItem138"
        '
        'BarButtonItem203
        '
        Me.BarButtonItem203.Caption = "ايداع مصرفي نقدي ليبيا مركزي"
        Me.BarButtonItem203.Id = 739
        Me.BarButtonItem203.Name = "BarButtonItem203"
        '
        'BarButtonItem206
        '
        Me.BarButtonItem206.Caption = "عرض الطلبات الحجز"
        Me.BarButtonItem206.Id = 742
        Me.BarButtonItem206.Name = "BarButtonItem206"
        '
        'BarButtonItem207
        '
        Me.BarButtonItem207.Caption = "طلبات الموافق عليها ملغيه "
        Me.BarButtonItem207.Id = 743
        Me.BarButtonItem207.Name = "BarButtonItem207"
        '
        'BarButtonItem208
        '
        Me.BarButtonItem208.Caption = "كشف حساب مصرف التجاري"
        Me.BarButtonItem208.Id = 744
        Me.BarButtonItem208.Name = "BarButtonItem208"
        '
        'BarButtonItem209
        '
        Me.BarButtonItem209.Caption = "كشف مبيعات مصرف ليبيا المركزي"
        Me.BarButtonItem209.Id = 745
        Me.BarButtonItem209.Name = "BarButtonItem209"
        '
        'BarButtonItem81
        '
        Me.BarButtonItem81.Caption = "BarButtonItem81"
        Me.BarButtonItem81.Id = 217
        Me.BarButtonItem81.Name = "BarButtonItem81"
        '
        'BANKDEPOORWITHDRAMENU
        '
        Me.BANKDEPOORWITHDRAMENU.Caption = "المعاملات المصرفية"
        Me.BANKDEPOORWITHDRAMENU.Id = 227
        Me.BANKDEPOORWITHDRAMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BTNEMPBANKWITHDRAWAL), New DevExpress.XtraBars.LinkPersistInfo(Me.BTNBANKDEPOSIT), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem157), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem205)})
        Me.BANKDEPOORWITHDRAMENU.Name = "BANKDEPOORWITHDRAMENU"
        Me.BANKDEPOORWITHDRAMENU.Tag = 64
        '
        'BTNEMPBANKWITHDRAWAL
        '
        Me.BTNEMPBANKWITHDRAWAL.Caption = "سحب من حساب"
        Me.BTNEMPBANKWITHDRAWAL.Id = 235
        Me.BTNEMPBANKWITHDRAWAL.Name = "BTNEMPBANKWITHDRAWAL"
        Me.BTNEMPBANKWITHDRAWAL.Tag = 67
        '
        'BTNBANKDEPOSIT
        '
        Me.BTNBANKDEPOSIT.Caption = "إيداع في حساب"
        Me.BTNBANKDEPOSIT.Id = 228
        Me.BTNBANKDEPOSIT.Name = "BTNBANKDEPOSIT"
        Me.BTNBANKDEPOSIT.Tag = 65
        '
        'BarButtonItem157
        '
        Me.BarButtonItem157.Caption = "تحويل مصرفي"
        Me.BarButtonItem157.Id = 672
        Me.BarButtonItem157.Name = "BarButtonItem157"
        '
        'BarButtonItem205
        '
        Me.BarButtonItem205.Caption = "ايداع  بطاقات بتحويل الاغراض"
        Me.BarButtonItem205.Id = 741
        Me.BarButtonItem205.Name = "BarButtonItem205"
        '
        'BtnUserAccessTemplate
        '
        Me.BtnUserAccessTemplate.Caption = "نماذج المستخدمين"
        Me.BtnUserAccessTemplate.Id = 231
        Me.BtnUserAccessTemplate.Name = "BtnUserAccessTemplate"
        Me.BtnUserAccessTemplate.Tag = 69
        '
        'BANKSTATEMENTMENU
        '
        Me.BANKSTATEMENTMENU.Caption = "تقارير المصارف"
        Me.BANKSTATEMENTMENU.Id = 238
        Me.BANKSTATEMENTMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnBBranchMovement), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem84), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem177), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem145)})
        Me.BANKSTATEMENTMENU.Name = "BANKSTATEMENTMENU"
        Me.BANKSTATEMENTMENU.Tag = 37
        '
        'BtnBBranchMovement
        '
        Me.BtnBBranchMovement.Caption = "كشف حساب فرع مصرف"
        Me.BtnBBranchMovement.Id = 239
        Me.BtnBBranchMovement.Name = "BtnBBranchMovement"
        Me.BtnBBranchMovement.Tag = 38
        '
        'BarButtonItem84
        '
        Me.BarButtonItem84.Caption = "استعلام خدمة إكترونية"
        Me.BarButtonItem84.Id = 402
        Me.BarButtonItem84.Name = "BarButtonItem84"
        '
        'BarButtonItem177
        '
        Me.BarButtonItem177.Caption = "استعلام المصارف"
        Me.BarButtonItem177.Id = 698
        Me.BarButtonItem177.Name = "BarButtonItem177"
        '
        'BarButtonItem145
        '
        Me.BarButtonItem145.Caption = "كشف المعاملات المصرفية"
        Me.BarButtonItem145.Id = 734
        Me.BarButtonItem145.Name = "BarButtonItem145"
        '
        'BRANCHSTATEMENTMENU
        '
        Me.BRANCHSTATEMENTMENU.Caption = "استعلامات الفرع والوكلاء"
        Me.BRANCHSTATEMENTMENU.Id = 256
        Me.BRANCHSTATEMENTMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnCurrencyStatement), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem72)})
        Me.BRANCHSTATEMENTMENU.Name = "BRANCHSTATEMENTMENU"
        Me.BRANCHSTATEMENTMENU.Tag = "25"
        Me.BRANCHSTATEMENTMENU.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BtnCurrencyStatement
        '
        Me.BtnCurrencyStatement.Caption = "عرض حركة العملة"
        Me.BtnCurrencyStatement.Id = 267
        Me.BtnCurrencyStatement.ImageOptions.Image = CType(resources.GetObject("BtnCurrencyStatement.ImageOptions.Image"), System.Drawing.Image)
        Me.BtnCurrencyStatement.Name = "BtnCurrencyStatement"
        Me.BtnCurrencyStatement.Tag = 30
        '
        'BarButtonItem72
        '
        Me.BarButtonItem72.Caption = "خزينة الودائع"
        Me.BarButtonItem72.Id = 368
        Me.BarButtonItem72.Name = "BarButtonItem72"
        '
        'PETIESMENU
        '
        Me.PETIESMENU.Caption = "العهد والمصروفات العمومية"
        Me.PETIESMENU.Id = 269
        Me.PETIESMENU.ImageOptions.SvgImage = CType(resources.GetObject("PETIESMENU.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.PETIESMENU.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.PETIESMENU.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnPettyCash), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnANOTHEREXPENS), New DevExpress.XtraBars.LinkPersistInfo(Me.btnPettyCashSettlement), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem144)})
        Me.PETIESMENU.Name = "PETIESMENU"
        Me.PETIESMENU.Tag = 54
        '
        'BtnPettyCash
        '
        Me.BtnPettyCash.Caption = "صرف عهدة"
        Me.BtnPettyCash.Id = 270
        Me.BtnPettyCash.ImageOptions.SvgImage = CType(resources.GetObject("BtnPettyCash.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnPettyCash.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BtnPettyCash.Name = "BtnPettyCash"
        Me.BtnPettyCash.Tag = 55
        '
        'BtnANOTHEREXPENS
        '
        Me.BtnANOTHEREXPENS.Caption = "مصروفات عموميه" & Global.Microsoft.VisualBasic.ChrW(10)
        Me.BtnANOTHEREXPENS.Id = 301
        Me.BtnANOTHEREXPENS.ImageOptions.SvgImage = CType(resources.GetObject("BtnANOTHEREXPENS.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnANOTHEREXPENS.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BtnANOTHEREXPENS.Name = "BtnANOTHEREXPENS"
        Me.BtnANOTHEREXPENS.Tag = 57
        '
        'btnPettyCashSettlement
        '
        Me.btnPettyCashSettlement.Caption = "تسوية عهدة"
        Me.btnPettyCashSettlement.Id = 271
        Me.btnPettyCashSettlement.ImageOptions.SvgImage = CType(resources.GetObject("btnPettyCashSettlement.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.btnPettyCashSettlement.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.btnPettyCashSettlement.Name = "btnPettyCashSettlement"
        Me.btnPettyCashSettlement.Tag = 56
        '
        'BarButtonItem144
        '
        Me.BarButtonItem144.Caption = "شراء أصل"
        Me.BarButtonItem144.Id = 645
        Me.BarButtonItem144.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem144.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem144.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem144.Name = "BarButtonItem144"
        '
        'BtnAddUser
        '
        Me.BtnAddUser.Caption = "إضافة مستخدم"
        Me.BtnAddUser.Id = 293
        Me.BtnAddUser.Name = "BtnAddUser"
        Me.BtnAddUser.Tag = 71
        '
        'BtnOpeningBalance
        '
        Me.BtnOpeningBalance.Caption = "الأرصدة الافتتاحية"
        Me.BtnOpeningBalance.Id = 295
        Me.BtnOpeningBalance.Name = "BtnOpeningBalance"
        Me.BtnOpeningBalance.Tag = 72
        '
        'BTNCURRENCYPRICE
        '
        Me.BTNCURRENCYPRICE.Caption = "نشرة أسعار العملات"
        Me.BTNCURRENCYPRICE.Id = 303
        Me.BTNCURRENCYPRICE.Name = "BTNCURRENCYPRICE"
        '
        'BarButtonItem5
        '
        Me.BarButtonItem5.Caption = "توزيع النسب"
        Me.BarButtonItem5.Id = 305
        Me.BarButtonItem5.Name = "BarButtonItem5"
        '
        'BarButtonItem19
        '
        Me.BarButtonItem19.Caption = "BarButtonItem19"
        Me.BarButtonItem19.Id = 313
        Me.BarButtonItem19.Name = "BarButtonItem19"
        '
        'BarButtonItem24
        '
        Me.BarButtonItem24.Caption = "BarButtonItem24"
        Me.BarButtonItem24.Id = 315
        Me.BarButtonItem24.Name = "BarButtonItem24"
        '
        'BarButtonItem29
        '
        Me.BarButtonItem29.Caption = "نشرة العملات  "
        Me.BarButtonItem29.Id = 321
        Me.BarButtonItem29.Name = "BarButtonItem29"
        '
        'BarButtonItem32
        '
        Me.BarButtonItem32.Caption = "شــــــــــراء عملة"
        Me.BarButtonItem32.Id = 324
        Me.BarButtonItem32.Name = "BarButtonItem32"
        '
        'BarButtonItem33
        '
        Me.BarButtonItem33.Caption = "أعدادت نسبة عملة عالمصرف"
        Me.BarButtonItem33.Id = 327
        Me.BarButtonItem33.Name = "BarButtonItem33"
        '
        'BarButtonItem34
        '
        Me.BarButtonItem34.Caption = "تقرير عرض حركة أسعار العملات "
        Me.BarButtonItem34.Id = 330
        Me.BarButtonItem34.Name = "BarButtonItem34"
        '
        'BarButtonItem52
        '
        Me.BarButtonItem52.Caption = "BarButtonItem52"
        Me.BarButtonItem52.Id = 334
        Me.BarButtonItem52.Name = "BarButtonItem52"
        '
        'BarButtonItem53
        '
        Me.BarButtonItem53.Caption = "BarButtonItem53"
        Me.BarButtonItem53.Id = 335
        Me.BarButtonItem53.Name = "BarButtonItem53"
        '
        'BarSubItem3
        '
        Me.BarSubItem3.Caption = "ودائع العمل الاجنبية"
        Me.BarSubItem3.Id = 336
        Me.BarSubItem3.Name = "BarSubItem3"
        '
        'BarButtonItem54
        '
        Me.BarButtonItem54.Caption = "قبض نقد أجنبي لعميل"
        Me.BarButtonItem54.Id = 337
        Me.BarButtonItem54.Name = "BarButtonItem54"
        '
        'BarButtonItem55
        '
        Me.BarButtonItem55.Caption = "قبض نقد أجنبي"
        Me.BarButtonItem55.Id = 338
        Me.BarButtonItem55.Name = "BarButtonItem55"
        '
        'BarSubItem4
        '
        Me.BarSubItem4.Caption = "صرف النقد الاجنبي"
        Me.BarSubItem4.Id = 341
        Me.BarSubItem4.Name = "BarSubItem4"
        '
        'BarButtonItem56
        '
        Me.BarButtonItem56.Caption = " صرف نقد أجنبي"
        Me.BarButtonItem56.Id = 342
        Me.BarButtonItem56.Name = "BarButtonItem56"
        '
        'BarButtonItem57
        '
        Me.BarButtonItem57.Caption = " صرف نقد أجنبي العميل"
        Me.BarButtonItem57.Id = 343
        Me.BarButtonItem57.Name = "BarButtonItem57"
        '
        'BarButtonItem60
        '
        Me.BarButtonItem60.Caption = "شاشة عرض حركة العملات"
        Me.BarButtonItem60.Id = 347
        Me.BarButtonItem60.Name = "BarButtonItem60"
        '
        'BarButtonItem61
        '
        Me.BarButtonItem61.ActAsDropDown = True
        Me.BarButtonItem61.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
        Me.BarButtonItem61.Caption = "شراء عملة"
        Me.BarButtonItem61.Id = 350
        Me.BarButtonItem61.Name = "BarButtonItem61"
        '
        'BarButtonItem64
        '
        Me.BarButtonItem64.Caption = "بيع عملة "
        Me.BarButtonItem64.Id = 358
        Me.BarButtonItem64.Name = "BarButtonItem64"
        '
        'BarButtonItem65
        '
        Me.BarButtonItem65.Caption = "BarButtonItem65"
        Me.BarButtonItem65.Id = 361
        Me.BarButtonItem65.Name = "BarButtonItem65"
        '
        'BarButtonItem71
        '
        Me.BarButtonItem71.Caption = "بيع عملـــــــة"
        Me.BarButtonItem71.Id = 364
        Me.BarButtonItem71.Name = "BarButtonItem71"
        '
        'BarButtonItem75
        '
        Me.BarButtonItem75.Caption = "شرائح التحويل الخارجي"
        Me.BarButtonItem75.Id = 378
        Me.BarButtonItem75.Name = "BarButtonItem75"
        '
        'BarButtonItem76
        '
        Me.BarButtonItem76.Caption = "BarButtonItem76"
        Me.BarButtonItem76.Id = 380
        Me.BarButtonItem76.Name = "BarButtonItem76"
        '
        'BarSubItem11
        '
        Me.BarSubItem11.Caption = "الاظافات"
        Me.BarSubItem11.Id = 408
        Me.BarSubItem11.Name = "BarSubItem11"
        '
        'BarSubItem13
        '
        Me.BarSubItem13.Caption = "العملات الجديد"
        Me.BarSubItem13.Id = 419
        Me.BarSubItem13.Name = "BarSubItem13"
        '
        'BarButtonItem86
        '
        Me.BarButtonItem86.Caption = "إدخال أسعار العملات"
        Me.BarButtonItem86.Id = 420
        Me.BarButtonItem86.Name = "BarButtonItem86"
        '
        'BarButtonItem87
        '
        Me.BarButtonItem87.Caption = "مضاربة عملة"
        Me.BarButtonItem87.Id = 422
        Me.BarButtonItem87.Name = "BarButtonItem87"
        '
        'BarButtonItem88
        '
        Me.BarButtonItem88.Caption = "إضافة جمعية"
        Me.BarButtonItem88.Id = 424
        Me.BarButtonItem88.Name = "BarButtonItem88"
        '
        'BarSubItem14
        '
        Me.BarSubItem14.Caption = "السندات المالية"
        Me.BarSubItem14.Id = 428
        Me.BarSubItem14.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem91), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem92)})
        Me.BarSubItem14.Name = "BarSubItem14"
        '
        'BarButtonItem91
        '
        Me.BarButtonItem91.Caption = "سند قبض"
        Me.BarButtonItem91.Id = 429
        Me.BarButtonItem91.Name = "BarButtonItem91"
        '
        'BarButtonItem92
        '
        Me.BarButtonItem92.Caption = "سند صرف"
        Me.BarButtonItem92.Id = 430
        Me.BarButtonItem92.Name = "BarButtonItem92"
        '
        'BarSubItem15
        '
        Me.BarSubItem15.Caption = "تقارير الجمعيات"
        Me.BarSubItem15.Id = 431
        Me.BarSubItem15.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem93), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem95), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem210), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem214), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem215)})
        Me.BarSubItem15.Name = "BarSubItem15"
        '
        'BarButtonItem93
        '
        Me.BarButtonItem93.Caption = "كشف حركة جمعية"
        Me.BarButtonItem93.Id = 432
        Me.BarButtonItem93.Name = "BarButtonItem93"
        '
        'BarButtonItem95
        '
        Me.BarButtonItem95.Caption = "كشف حركة الأعضاء"
        Me.BarButtonItem95.Id = 434
        Me.BarButtonItem95.Name = "BarButtonItem95"
        '
        'BarButtonItem210
        '
        Me.BarButtonItem210.Caption = "كشف حركة عضو جمعية"
        Me.BarButtonItem210.Id = 746
        Me.BarButtonItem210.Name = "BarButtonItem210"
        '
        'BarButtonItem214
        '
        Me.BarButtonItem214.Caption = "كشف بالمصروفات"
        Me.BarButtonItem214.Id = 751
        Me.BarButtonItem214.Name = "BarButtonItem214"
        '
        'BarButtonItem215
        '
        Me.BarButtonItem215.Caption = "كشف بالايرادات"
        Me.BarButtonItem215.Id = 752
        Me.BarButtonItem215.Name = "BarButtonItem215"
        '
        'BarSubItem16
        '
        Me.BarSubItem16.Caption = "نشرات العملة"
        Me.BarSubItem16.Id = 440
        Me.BarSubItem16.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem33), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem34), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem86), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem98), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem172)})
        Me.BarSubItem16.Name = "BarSubItem16"
        '
        'BarButtonItem98
        '
        Me.BarButtonItem98.Caption = "تسعير العملات"
        Me.BarButtonItem98.Id = 446
        Me.BarButtonItem98.Name = "BarButtonItem98"
        '
        'BarButtonItem172
        '
        Me.BarButtonItem172.Caption = "الحد الأقصى للعملاء"
        Me.BarButtonItem172.Id = 693
        Me.BarButtonItem172.Name = "BarButtonItem172"
        '
        'BarSubItem17
        '
        Me.BarSubItem17.Caption = "ودائع النقد الأجنبي"
        Me.BarSubItem17.Id = 441
        Me.BarSubItem17.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem55), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem56)})
        Me.BarSubItem17.Name = "BarSubItem17"
        '
        'BarSubItem18
        '
        Me.BarSubItem18.Caption = "قبض ودائع النقد الأجنبي"
        Me.BarSubItem18.Id = 442
        Me.BarSubItem18.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem54), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem148)})
        Me.BarSubItem18.Name = "BarSubItem18"
        '
        'BarButtonItem148
        '
        Me.BarButtonItem148.Caption = "قبض نقد أجنبي لوكيل"
        Me.BarButtonItem148.Id = 649
        Me.BarButtonItem148.Name = "BarButtonItem148"
        '
        'BarSubItem19
        '
        Me.BarSubItem19.Caption = "صرف ودائع النقد الأجنبي"
        Me.BarSubItem19.Id = 443
        Me.BarSubItem19.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem57), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem149)})
        Me.BarSubItem19.Name = "BarSubItem19"
        '
        'BarButtonItem149
        '
        Me.BarButtonItem149.Caption = " صرف نقد أجنبي لوكيل"
        Me.BarButtonItem149.Id = 650
        Me.BarButtonItem149.Name = "BarButtonItem149"
        '
        'BarSubItem20
        '
        Me.BarSubItem20.Caption = "عمليات البيع والشراء"
        Me.BarSubItem20.Id = 444
        Me.BarSubItem20.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem60), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem87), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem102), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem103), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem173), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem174), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem181)})
        Me.BarSubItem20.Name = "BarSubItem20"
        '
        'BarButtonItem102
        '
        Me.BarButtonItem102.Caption = "شراء عملة"
        Me.BarButtonItem102.Id = 461
        Me.BarButtonItem102.Name = "BarButtonItem102"
        '
        'BarButtonItem103
        '
        Me.BarButtonItem103.Caption = "بيع عملة"
        Me.BarButtonItem103.Id = 462
        Me.BarButtonItem103.Name = "BarButtonItem103"
        '
        'BarButtonItem173
        '
        Me.BarButtonItem173.Caption = "حركة بيع وشراء العملة"
        Me.BarButtonItem173.Id = 694
        Me.BarButtonItem173.Name = "BarButtonItem173"
        '
        'BarButtonItem174
        '
        Me.BarButtonItem174.Caption = "حركة بيع عملة لعميل"
        Me.BarButtonItem174.Id = 695
        Me.BarButtonItem174.Name = "BarButtonItem174"
        '
        'BarButtonItem181
        '
        Me.BarButtonItem181.Caption = "أرباح بيع عملات"
        Me.BarButtonItem181.Id = 703
        Me.BarButtonItem181.Name = "BarButtonItem181"
        '
        'BarButtonItem99
        '
        Me.BarButtonItem99.Caption = "شاشة تعديل الأسعار"
        Me.BarButtonItem99.Id = 454
        Me.BarButtonItem99.Name = "BarButtonItem99"
        '
        'BarButtonItem100
        '
        Me.BarButtonItem100.Caption = "سحب من حساب بدون رصيد"
        Me.BarButtonItem100.Id = 458
        Me.BarButtonItem100.Name = "BarButtonItem100"
        '
        'BarButtonItem101
        '
        Me.BarButtonItem101.Caption = "BarButtonItem101"
        Me.BarButtonItem101.Id = 460
        Me.BarButtonItem101.Name = "BarButtonItem101"
        '
        'BarButtonItem106
        '
        Me.BarButtonItem106.Id = 482
        Me.BarButtonItem106.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem106.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem106.Name = "BarButtonItem106"
        '
        'BarButtonItem110
        '
        Me.BarButtonItem110.Caption = "BarButtonItem110"
        Me.BarButtonItem110.Id = 497
        Me.BarButtonItem110.Name = "BarButtonItem110"
        '
        'BarSubItem23
        '
        Me.BarSubItem23.Caption = "كشف حركة النقدية"
        Me.BarSubItem23.Id = 508
        Me.BarSubItem23.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem118)})
        Me.BarSubItem23.Name = "BarSubItem23"
        '
        'BarButtonItem118
        '
        Me.BarButtonItem118.Caption = "كشف حركة النقدية"
        Me.BarButtonItem118.Id = 510
        Me.BarButtonItem118.Name = "BarButtonItem118"
        '
        'BarSubItem24
        '
        Me.BarSubItem24.Caption = "السلف والعلاوات والخصميات"
        Me.BarSubItem24.Id = 511
        Me.BarSubItem24.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem68), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem74), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem119), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnLeave), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem187), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem191), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem96)})
        Me.BarSubItem24.Name = "BarSubItem24"
        '
        'BarButtonItem68
        '
        Me.BarButtonItem68.Caption = "السلف"
        Me.BarButtonItem68.Id = 512
        Me.BarButtonItem68.Name = "BarButtonItem68"
        '
        'BarButtonItem74
        '
        Me.BarButtonItem74.Caption = "العلاوات"
        Me.BarButtonItem74.Id = 513
        Me.BarButtonItem74.Name = "BarButtonItem74"
        '
        'BarButtonItem119
        '
        Me.BarButtonItem119.Caption = "الخصميات"
        Me.BarButtonItem119.Id = 514
        Me.BarButtonItem119.Name = "BarButtonItem119"
        '
        'BtnLeave
        '
        Me.BtnLeave.Caption = "إجازة موظف"
        Me.BtnLeave.Id = 700
        Me.BtnLeave.Name = "BtnLeave"
        '
        'BarButtonItem187
        '
        Me.BarButtonItem187.Caption = "علاوات وخصومات جماعية"
        Me.BarButtonItem187.Id = 710
        Me.BarButtonItem187.Name = "BarButtonItem187"
        '
        'BarButtonItem191
        '
        Me.BarButtonItem191.Caption = "طلب استقالة"
        Me.BarButtonItem191.Id = 715
        Me.BarButtonItem191.Name = "BarButtonItem191"
        '
        'BarButtonItem96
        '
        Me.BarButtonItem96.Caption = "أرشيف الموظفين"
        Me.BarButtonItem96.Id = 731
        Me.BarButtonItem96.Name = "BarButtonItem96"
        '
        'BarSubItem25
        '
        Me.BarSubItem25.Caption = "التقارير المالية"
        Me.BarSubItem25.Id = 515
        Me.BarSubItem25.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem120), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem121), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem140), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem141), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem151), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem158), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem180), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem85)})
        Me.BarSubItem25.Name = "BarSubItem25"
        '
        'BarButtonItem120
        '
        Me.BarButtonItem120.Caption = "قائمة الدخل"
        Me.BarButtonItem120.Id = 516
        Me.BarButtonItem120.Name = "BarButtonItem120"
        '
        'BarButtonItem121
        '
        Me.BarButtonItem121.Caption = "ميزان المراجعة"
        Me.BarButtonItem121.Id = 517
        Me.BarButtonItem121.Name = "BarButtonItem121"
        '
        'BarButtonItem140
        '
        Me.BarButtonItem140.Caption = "متابعة نقدية الفروع "
        Me.BarButtonItem140.Id = 638
        Me.BarButtonItem140.Name = "BarButtonItem140"
        '
        'BarButtonItem141
        '
        Me.BarButtonItem141.Caption = "كشف بميزانية الفرع"
        Me.BarButtonItem141.Id = 639
        Me.BarButtonItem141.Name = "BarButtonItem141"
        '
        'BarButtonItem151
        '
        Me.BarButtonItem151.Caption = "تقسيم إجمالي قائمة الدخل"
        Me.BarButtonItem151.Id = 653
        Me.BarButtonItem151.Name = "BarButtonItem151"
        '
        'BarButtonItem158
        '
        Me.BarButtonItem158.Caption = "جدول الحسابات"
        Me.BarButtonItem158.Id = 673
        Me.BarButtonItem158.Name = "BarButtonItem158"
        '
        'BarButtonItem180
        '
        Me.BarButtonItem180.Caption = "تقرير حدود السحب"
        Me.BarButtonItem180.Id = 702
        Me.BarButtonItem180.Name = "BarButtonItem180"
        '
        'BarButtonItem85
        '
        Me.BarButtonItem85.Caption = "الميزانية العمومية"
        Me.BarButtonItem85.Id = 729
        Me.BarButtonItem85.Name = "BarButtonItem85"
        '
        'BarSubItem26
        '
        Me.BarSubItem26.Caption = "إضافة جمعية وعضو"
        Me.BarSubItem26.Id = 518
        Me.BarSubItem26.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem51), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem122)})
        Me.BarSubItem26.Name = "BarSubItem26"
        '
        'BarButtonItem51
        '
        Me.BarButtonItem51.Caption = "إضافة جمعية"
        Me.BarButtonItem51.Id = 519
        Me.BarButtonItem51.Name = "BarButtonItem51"
        '
        'BarButtonItem122
        '
        Me.BarButtonItem122.Caption = "إضافة عضو"
        Me.BarButtonItem122.Id = 520
        Me.BarButtonItem122.Name = "BarButtonItem122"
        '
        'BarSubItem27
        '
        Me.BarSubItem27.Caption = "تعديل اشتراك واحتساب جمعية"
        Me.BarSubItem27.Id = 521
        Me.BarSubItem27.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem89), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem123)})
        Me.BarSubItem27.Name = "BarSubItem27"
        '
        'BarButtonItem89
        '
        Me.BarButtonItem89.Caption = "تعديل اشتراك جمعية"
        Me.BarButtonItem89.Id = 522
        Me.BarButtonItem89.Name = "BarButtonItem89"
        '
        'BarButtonItem123
        '
        Me.BarButtonItem123.Caption = "احتساب اشتراك جمعية"
        Me.BarButtonItem123.Id = 523
        Me.BarButtonItem123.Name = "BarButtonItem123"
        '
        'BarSubItem28
        '
        Me.BarSubItem28.Caption = "إعدادات المستخدمين"
        Me.BarSubItem28.Id = 524
        Me.BarSubItem28.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnUserAccessTemplate), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddUser), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem90), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem124), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem125), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem128), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem73), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem855), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem85585)})
        Me.BarSubItem28.Name = "BarSubItem28"
        '
        'BarButtonItem90
        '
        Me.BarButtonItem90.Caption = "صلاحيات المجموعات"
        Me.BarButtonItem90.Id = 542
        Me.BarButtonItem90.Name = "BarButtonItem90"
        '
        'BarButtonItem124
        '
        Me.BarButtonItem124.Caption = "صلاحيات المستخدمين"
        Me.BarButtonItem124.Id = 566
        Me.BarButtonItem124.Name = "BarButtonItem124"
        '
        'BarButtonItem125
        '
        Me.BarButtonItem125.Caption = "اضافة شاشة جديدة"
        Me.BarButtonItem125.Id = 569
        Me.BarButtonItem125.Name = "BarButtonItem125"
        '
        'BarButtonItem128
        '
        Me.BarButtonItem128.Caption = "تعديل صلاحيات دخول المستخدم"
        Me.BarButtonItem128.Id = 617
        Me.BarButtonItem128.Name = "BarButtonItem128"
        '
        'BarButtonItem73
        '
        Me.BarButtonItem73.Caption = "اضافة اشعار جديد"
        Me.BarButtonItem73.Id = 725
        Me.BarButtonItem73.Name = "BarButtonItem73"
        '
        'BarButtonItem855
        '
        Me.BarButtonItem855.Caption = "اضافة تبويب جديد"
        Me.BarButtonItem855.Id = 727
        Me.BarButtonItem855.Name = "BarButtonItem855"
        '
        'BarButtonItem85585
        '
        Me.BarButtonItem85585.Caption = "اضافة تبويب فرعي"
        Me.BarButtonItem85585.Id = 728
        Me.BarButtonItem85585.Name = "BarButtonItem85585"
        '
        'BarSubItem29
        '
        Me.BarSubItem29.Caption = "أرصدة افتتاحية وتوزيع نسب"
        Me.BarSubItem29.Id = 525
        Me.BarSubItem29.Name = "BarSubItem29"
        '
        'BarSubItem31
        '
        Me.BarSubItem31.Caption = "توزيع نسب العمولات"
        Me.BarSubItem31.Id = 590
        Me.BarSubItem31.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem182), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem183)})
        Me.BarSubItem31.Name = "BarSubItem31"
        '
        'BarButtonItem182
        '
        Me.BarButtonItem182.Caption = "عمولات التطبيق"
        Me.BarButtonItem182.Id = 704
        Me.BarButtonItem182.Name = "BarButtonItem182"
        '
        'BarButtonItem183
        '
        Me.BarButtonItem183.Caption = "معدل تحويلات التطبيق"
        Me.BarButtonItem183.Id = 705
        Me.BarButtonItem183.Name = "BarButtonItem183"
        '
        'BarSubItem32
        '
        Me.BarSubItem32.Caption = "شرائح الحوالات الخارجية"
        Me.BarSubItem32.Id = 591
        Me.BarSubItem32.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem75)})
        Me.BarSubItem32.Name = "BarSubItem32"
        '
        'BarSubItem33
        '
        Me.BarSubItem33.Caption = "إدخال الأرصدة الافتتاحية"
        Me.BarSubItem33.Id = 592
        Me.BarSubItem33.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnOpeningBalance)})
        Me.BarSubItem33.Name = "BarSubItem33"
        '
        'BarButtonGroup4
        '
        Me.BarButtonGroup4.Caption = "BarButtonGroup4"
        Me.BarButtonGroup4.Id = 602
        Me.BarButtonGroup4.Name = "BarButtonGroup4"
        '
        'BarSubItem34
        '
        Me.BarSubItem34.Caption = "BarSubItem34"
        Me.BarSubItem34.Id = 603
        Me.BarSubItem34.Name = "BarSubItem34"
        '
        'BarButtonItem77
        '
        Me.BarButtonItem77.Caption = "BarButtonItem77"
        Me.BarButtonItem77.Id = 604
        Me.BarButtonItem77.Name = "BarButtonItem77"
        '
        'BarSubItem35
        '
        Me.BarSubItem35.Caption = "الموظفين"
        Me.BarSubItem35.Id = 605
        Me.BarSubItem35.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem127)})
        Me.BarSubItem35.Name = "BarSubItem35"
        '
        'BarButtonItem127
        '
        Me.BarButtonItem127.Caption = "اضافة موظف"
        Me.BarButtonItem127.Id = 607
        Me.BarButtonItem127.Name = "BarButtonItem127"
        '
        'BarButtonItem130
        '
        Me.BarButtonItem130.Id = 624
        Me.BarButtonItem130.Name = "BarButtonItem130"
        '
        'BarButtonItem135
        '
        Me.BarButtonItem135.Caption = "BarButtonItem135"
        Me.BarButtonItem135.Id = 631
        Me.BarButtonItem135.Name = "BarButtonItem135"
        '
        'BarSubItem36
        '
        Me.BarSubItem36.Caption = "اعدادات الوستاب"
        Me.BarSubItem36.Id = 632
        Me.BarSubItem36.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem136)})
        Me.BarSubItem36.Name = "BarSubItem36"
        '
        'BarButtonItem136
        '
        Me.BarButtonItem136.Caption = "غرف الوتساب"
        Me.BarButtonItem136.Id = 633
        Me.BarButtonItem136.Name = "BarButtonItem136"
        '
        'BarButtonItem142
        '
        Me.BarButtonItem142.Caption = "تعديل قيود محاسبية"
        Me.BarButtonItem142.Id = 640
        Me.BarButtonItem142.Name = "BarButtonItem142"
        '
        'BarSubItem37
        '
        Me.BarSubItem37.Caption = "تعديل قيود محاسبية"
        Me.BarSubItem37.Id = 641
        Me.BarSubItem37.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem142), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem185)})
        Me.BarSubItem37.Name = "BarSubItem37"
        '
        'BarButtonItem185
        '
        Me.BarButtonItem185.Caption = "تسجيل عمليات مالية"
        Me.BarButtonItem185.Id = 707
        Me.BarButtonItem185.Name = "BarButtonItem185"
        '
        'BtnAddProject
        '
        Me.BtnAddProject.Caption = "إضافة مشروع"
        Me.BtnAddProject.Id = 654
        Me.BtnAddProject.Name = "BtnAddProject"
        '
        'BtnProjectPartner
        '
        Me.BtnProjectPartner.Caption = "إضافة شريك مشروع"
        Me.BtnProjectPartner.Id = 655
        Me.BtnProjectPartner.Name = "BtnProjectPartner"
        '
        'BtnAddPettyCash
        '
        Me.BtnAddPettyCash.Caption = "صرف عهدة"
        Me.BtnAddPettyCash.Id = 656
        Me.BtnAddPettyCash.Name = "BtnAddPettyCash"
        '
        'BtnPettySettlement
        '
        Me.BtnPettySettlement.Caption = "تسوية عهدة مشروع"
        Me.BtnPettySettlement.Id = 657
        Me.BtnPettySettlement.Name = "BtnPettySettlement"
        '
        'BtnAnotherExpense
        '
        Me.BtnAnotherExpense.Caption = "مصروف عمومي مشروع"
        Me.BtnAnotherExpense.Id = 658
        Me.BtnAnotherExpense.Name = "BtnAnotherExpense"
        '
        'BtnAddProExpense
        '
        Me.BtnAddProExpense.Caption = "إضافة نوع مصروف مشروع"
        Me.BtnAddProExpense.Id = 659
        Me.BtnAddProExpense.Name = "BtnAddProExpense"
        '
        'BtnAddAssest
        '
        Me.BtnAddAssest.Caption = "إضافة أصل مشروع"
        Me.BtnAddAssest.Id = 660
        Me.BtnAddAssest.Name = "BtnAddAssest"
        '
        'AddBasiscMenu
        '
        Me.AddBasiscMenu.Caption = "إضافات أساسية"
        Me.AddBasiscMenu.Id = 661
        Me.AddBasiscMenu.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddProject), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnProjectPartner), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddAssest), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddProExpense), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem153), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddItem), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddItemDetails), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAddSupplier)})
        Me.AddBasiscMenu.Name = "AddBasiscMenu"
        '
        'BarButtonItem153
        '
        Me.BarButtonItem153.Caption = "إضافة مقاول"
        Me.BarButtonItem153.Id = 666
        Me.BarButtonItem153.Name = "BarButtonItem153"
        '
        'BtnAddItem
        '
        Me.BtnAddItem.Caption = "إضافة مادة"
        Me.BtnAddItem.Id = 681
        Me.BtnAddItem.Name = "BtnAddItem"
        '
        'BtnAddItemDetails
        '
        Me.BtnAddItemDetails.Caption = "إضافة تفاصيل مادة"
        Me.BtnAddItemDetails.Id = 682
        Me.BtnAddItemDetails.Name = "BtnAddItemDetails"
        '
        'BtnAddSupplier
        '
        Me.BtnAddSupplier.Caption = "إضافة مورد"
        Me.BtnAddSupplier.Id = 683
        Me.BtnAddSupplier.Name = "BtnAddSupplier"
        '
        'BtnProAddPettyCash
        '
        Me.BtnProAddPettyCash.Caption = "العهد والمصروفات"
        Me.BtnProAddPettyCash.Id = 662
        Me.BtnProAddPettyCash.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BtnProPayPetty), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnPettySettlement), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnAnotherExpense), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnContractorPayment), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem156), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnImportItem), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem165), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem166), New DevExpress.XtraBars.LinkPersistInfo(Me.BtnPROEXPORTITEM), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem167), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem176)})
        Me.BtnProAddPettyCash.Name = "BtnProAddPettyCash"
        '
        'BtnProPayPetty
        '
        Me.BtnProPayPetty.Caption = "صرف عهدة مشروع"
        Me.BtnProPayPetty.Id = 663
        Me.BtnProPayPetty.Name = "BtnProPayPetty"
        '
        'BtnContractorPayment
        '
        Me.BtnContractorPayment.Caption = "سند صرف لمقاول"
        Me.BtnContractorPayment.Id = 667
        Me.BtnContractorPayment.Name = "BtnContractorPayment"
        '
        'BarButtonItem156
        '
        Me.BarButtonItem156.Caption = "سند صرف لأصل"
        Me.BarButtonItem156.Id = 671
        Me.BarButtonItem156.Name = "BarButtonItem156"
        '
        'BtnImportItem
        '
        Me.BtnImportItem.Caption = "أمر توريد صنف"
        Me.BtnImportItem.Id = 684
        Me.BtnImportItem.Name = "BtnImportItem"
        '
        'BarButtonItem165
        '
        Me.BarButtonItem165.Caption = "إيداع مصرفي لمشروع"
        Me.BarButtonItem165.Id = 685
        Me.BarButtonItem165.Name = "BarButtonItem165"
        '
        'BarButtonItem166
        '
        Me.BarButtonItem166.Caption = "سند صرف لمورد"
        Me.BarButtonItem166.Id = 686
        Me.BarButtonItem166.Name = "BarButtonItem166"
        '
        'BtnPROEXPORTITEM
        '
        Me.BtnPROEXPORTITEM.Caption = "أمر صرف صنف"
        Me.BtnPROEXPORTITEM.Id = 687
        Me.BtnPROEXPORTITEM.Name = "BtnPROEXPORTITEM"
        '
        'BarButtonItem167
        '
        Me.BarButtonItem167.Caption = "عمليات الأنشطة"
        Me.BarButtonItem167.Id = 688
        Me.BarButtonItem167.Name = "BarButtonItem167"
        '
        'BarButtonItem176
        '
        Me.BarButtonItem176.Caption = "إقفال دخل نشاط"
        Me.BarButtonItem176.Id = 697
        Me.BarButtonItem176.Name = "BarButtonItem176"
        '
        'BarButtonItem152
        '
        Me.BarButtonItem152.Caption = "BarButtonItem152"
        Me.BarButtonItem152.Id = 665
        Me.BarButtonItem152.Name = "BarButtonItem152"
        '
        'BarSubItem39
        '
        Me.BarSubItem39.Caption = "التقارير"
        Me.BarSubItem39.Id = 668
        Me.BarSubItem39.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem154), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem155), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem162), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem164), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem168), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem169), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem170), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem178)})
        Me.BarSubItem39.Name = "BarSubItem39"
        '
        'BarButtonItem154
        '
        Me.BarButtonItem154.Caption = "مصاريف المشاريع"
        Me.BarButtonItem154.Id = 669
        Me.BarButtonItem154.Name = "BarButtonItem154"
        '
        'BarButtonItem155
        '
        Me.BarButtonItem155.Caption = "كشف حساب"
        Me.BarButtonItem155.Id = 670
        Me.BarButtonItem155.Name = "BarButtonItem155"
        '
        'BarButtonItem162
        '
        Me.BarButtonItem162.Caption = "كشف حساب موظف"
        Me.BarButtonItem162.Id = 678
        Me.BarButtonItem162.Name = "BarButtonItem162"
        '
        'BarButtonItem164
        '
        Me.BarButtonItem164.Caption = "تقرير تسوية العهد"
        Me.BarButtonItem164.Id = 680
        Me.BarButtonItem164.Name = "BarButtonItem164"
        '
        'BarButtonItem168
        '
        Me.BarButtonItem168.Caption = "كشف حساب مورد"
        Me.BarButtonItem168.Id = 689
        Me.BarButtonItem168.Name = "BarButtonItem168"
        '
        'BarButtonItem169
        '
        Me.BarButtonItem169.Caption = "تقرير العهد"
        Me.BarButtonItem169.Id = 690
        Me.BarButtonItem169.Name = "BarButtonItem169"
        '
        'BarButtonItem170
        '
        Me.BarButtonItem170.Caption = "كشف الأصناف المستوردة"
        Me.BarButtonItem170.Id = 691
        Me.BarButtonItem170.Name = "BarButtonItem170"
        '
        'BarButtonItem178
        '
        Me.BarButtonItem178.Caption = "كشف إيرادات ومصروفات نشاط"
        Me.BarButtonItem178.Id = 699
        Me.BarButtonItem178.Name = "BarButtonItem178"
        '
        'BarSubItem40
        '
        Me.BarSubItem40.Caption = "الموظفون"
        Me.BarSubItem40.Id = 674
        Me.BarSubItem40.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem159), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem160), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem161)})
        Me.BarSubItem40.Name = "BarSubItem40"
        '
        'BarButtonItem159
        '
        Me.BarButtonItem159.Caption = "إضافة موظف"
        Me.BarButtonItem159.Id = 675
        Me.BarButtonItem159.Name = "BarButtonItem159"
        '
        'BarButtonItem160
        '
        Me.BarButtonItem160.Caption = "احتساب راتب فردي"
        Me.BarButtonItem160.Id = 676
        Me.BarButtonItem160.Name = "BarButtonItem160"
        '
        'BarButtonItem161
        '
        Me.BarButtonItem161.Caption = "سند صرف لموظف"
        Me.BarButtonItem161.Id = 677
        Me.BarButtonItem161.Name = "BarButtonItem161"
        '
        'BarSubItem12
        '
        Me.BarSubItem12.Caption = "تقرير الحوالات المالية"
        Me.BarSubItem12.Id = 708
        Me.BarSubItem12.ImageOptions.SvgImage = CType(resources.GetObject("BarSubItem12.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarSubItem12.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarSubItem12.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem186)})
        Me.BarSubItem12.Name = "BarSubItem12"
        '
        'BarButtonItem186
        '
        Me.BarButtonItem186.Caption = "حركة حولات المستخدمين"
        Me.BarButtonItem186.Id = 709
        Me.BarButtonItem186.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem186.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem186.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem186.Name = "BarButtonItem186"
        '
        'BarSubItem41
        '
        Me.BarSubItem41.Caption = "مندوبين"
        Me.BarSubItem41.Id = 711
        Me.BarSubItem41.ImageOptions.SvgImage = CType(resources.GetObject("BarSubItem41.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarSubItem41.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarSubItem41.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem189), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem193), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem194), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem195), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem196), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem198), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem200), New DevExpress.XtraBars.LinkPersistInfo(Me.MoActivetion)})
        Me.BarSubItem41.Name = "BarSubItem41"
        '
        'BarButtonItem189
        '
        Me.BarButtonItem189.Caption = "اضافة مندوب"
        Me.BarButtonItem189.Id = 713
        Me.BarButtonItem189.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem189.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem189.ImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.BarButtonItem189.Name = "BarButtonItem189"
        '
        'BarButtonItem193
        '
        Me.BarButtonItem193.Caption = "اضافة حساب تطبيق للفرع(الخزينة)"
        Me.BarButtonItem193.Id = 717
        Me.BarButtonItem193.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem193.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem193.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem193.Name = "BarButtonItem193"
        '
        'BarButtonItem194
        '
        Me.BarButtonItem194.Caption = "عرض التوصيل"
        Me.BarButtonItem194.Id = 718
        Me.BarButtonItem194.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem194.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem194.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem194.Name = "BarButtonItem194"
        '
        'BarButtonItem195
        '
        Me.BarButtonItem195.Caption = "حوالات تاكسي غير مرسلة"
        Me.BarButtonItem195.Id = 719
        Me.BarButtonItem195.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem195.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem195.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem195.Name = "BarButtonItem195"
        '
        'BarButtonItem196
        '
        Me.BarButtonItem196.Caption = "طلب توصيل داخي"
        Me.BarButtonItem196.Id = 720
        Me.BarButtonItem196.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem196.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem196.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem196.Name = "BarButtonItem196"
        '
        'BarButtonItem198
        '
        Me.BarButtonItem198.Caption = "ارسال فاتورة مع تاكسي"
        Me.BarButtonItem198.Id = 722
        Me.BarButtonItem198.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem198.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem198.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem198.Name = "BarButtonItem198"
        '
        'BarButtonItem200
        '
        Me.BarButtonItem200.Caption = "اعادة تفعيل حساب مستخدم"
        Me.BarButtonItem200.Id = 724
        Me.BarButtonItem200.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem200.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem200.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.BarButtonItem200.Name = "BarButtonItem200"
        '
        'MoActivetion
        '
        Me.MoActivetion.Caption = "حسابات التطبيق غير المفعلة"
        Me.MoActivetion.Id = 726
        Me.MoActivetion.ImageOptions.SvgImage = CType(resources.GetObject("MoActivetion.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.MoActivetion.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.MoActivetion.Name = "MoActivetion"
        '
        'BarButtonItem188
        '
        Me.BarButtonItem188.Caption = "BarButtonItem188"
        Me.BarButtonItem188.Id = 712
        Me.BarButtonItem188.Name = "BarButtonItem188"
        '
        'BarButtonItem146
        '
        Me.BarButtonItem146.Caption = "BarButtonItem146"
        Me.BarButtonItem146.Id = 735
        Me.BarButtonItem146.Name = "BarButtonItem146"
        '
        'BarButtonItem204
        '
        Me.BarButtonItem204.Caption = "ايداع مصرفي مصرف ليبيا المركزي"
        Me.BarButtonItem204.Id = 740
        Me.BarButtonItem204.Name = "BarButtonItem204"
        '
        'BarSubItem38
        '
        Me.BarSubItem38.Caption = "البطاقات"
        Me.BarSubItem38.Id = 747
        Me.BarSubItem38.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem211), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem212), New DevExpress.XtraBars.LinkPersistInfo(Me.BarButtonItem213)})
        Me.BarSubItem38.Name = "BarSubItem38"
        '
        'BarButtonItem211
        '
        Me.BarButtonItem211.Caption = "عمليات الأنشطة"
        Me.BarButtonItem211.Id = 748
        Me.BarButtonItem211.Name = "BarButtonItem211"
        '
        'BarButtonItem212
        '
        Me.BarButtonItem212.Caption = "إقفال"
        Me.BarButtonItem212.Id = 749
        Me.BarButtonItem212.Name = "BarButtonItem212"
        '
        'BarButtonItem213
        '
        Me.BarButtonItem213.Caption = "كشف المصروفات والايرادات"
        Me.BarButtonItem213.Id = 750
        Me.BarButtonItem213.Name = "BarButtonItem213"
        '
        'RibbonPageCategory1
        '
        Me.RibbonPageCategory1.Name = "RibbonPageCategory1"
        Me.RibbonPageCategory1.Text = "RibbonPageCategory1"
        '
        'RibbonPageCategory2
        '
        Me.RibbonPageCategory2.Name = "RibbonPageCategory2"
        Me.RibbonPageCategory2.Text = "RibbonPageCategory2"
        '
        'RibbonPage1
        '
        Me.RibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup28, Me.RibbonPageGroup2})
        Me.RibbonPage1.ImageOptions.SvgImage = CType(resources.GetObject("RibbonPage1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RibbonPage1.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.RibbonPage1.Name = "RibbonPage1"
        Me.RibbonPage1.Text = "العملات"
        '
        'RibbonPageGroup28
        '
        Me.RibbonPageGroup28.ItemLinks.Add(Me.BarSubItem16)
        Me.RibbonPageGroup28.ItemLinks.Add(Me.BarSubItem17)
        Me.RibbonPageGroup28.ItemLinks.Add(Me.BarSubItem20)
        Me.RibbonPageGroup28.Name = "RibbonPageGroup28"
        Me.RibbonPageGroup28.Text = "RibbonPageGroup28"
        '
        'RibbonPageGroup2
        '
        Me.RibbonPageGroup2.ItemLinks.Add(Me.BarSubItem38)
        Me.RibbonPageGroup2.Name = "RibbonPageGroup2"
        Me.RibbonPageGroup2.Text = "RibbonPageGroup2"
        '
        'RP5
        '
        Me.RP5.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup26})
        Me.RP5.ImageOptions.SvgImage = CType(resources.GetObject("RP5.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RP5.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.RP5.Name = "RP5"
        Me.RP5.Tag = 5
        Me.RP5.Text = "السندات المالية"
        '
        'RibbonPageGroup26
        '
        Me.RibbonPageGroup26.ItemLinks.Add(Me.PETIESMENU)
        Me.RibbonPageGroup26.ItemLinks.Add(Me.BILLPAYMENTMENU)
        Me.RibbonPageGroup26.Name = "RibbonPageGroup26"
        Me.RibbonPageGroup26.Text = "الصرف"
        '
        'RP3
        '
        Me.RP3.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.BranchSTGR})
        Me.RP3.ImageOptions.SvgImage = CType(resources.GetObject("RP3.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RP3.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.RP3.Name = "RP3"
        Me.RP3.Tag = 3
        Me.RP3.Text = "التقارير والاستعلامات"
        '
        'BranchSTGR
        '
        Me.BranchSTGR.ItemLinks.Add(Me.BRANCHSTATEMENTMENU)
        Me.BranchSTGR.ItemLinks.Add(Me.CUSTOMERSTATEMENTMENU)
        Me.BranchSTGR.ItemLinks.Add(Me.GeneralExpensesMenu)
        Me.BranchSTGR.ItemLinks.Add(Me.BarSubItem23)
        Me.BranchSTGR.ItemLinks.Add(Me.EMPSTATEMENTMENU)
        Me.BranchSTGR.ItemLinks.Add(Me.BANKSTATEMENTMENU)
        Me.BranchSTGR.ItemLinks.Add(Me.BarSubItem15)
        Me.BranchSTGR.ItemLinks.Add(Me.BarSubItem25)
        Me.BranchSTGR.Name = "BranchSTGR"
        Me.BranchSTGR.Text = "استعلام الفرع"
        '
        'RP4
        '
        Me.RP4.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.AddEmpGR})
        Me.RP4.ImageOptions.SvgImage = CType(resources.GetObject("RP4.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RP4.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.RP4.Name = "RP4"
        Me.RP4.Tag = 4
        Me.RP4.Text = "الموظفون"
        '
        'AddEmpGR
        '
        Me.AddEmpGR.ItemLinks.Add(Me.BarButtonGroup4)
        Me.AddEmpGR.ItemLinks.Add(Me.BarSubItem35)
        Me.AddEmpGR.ItemLinks.Add(Me.EMPSALARYMENU)
        Me.AddEmpGR.ItemLinks.Add(Me.BarSubItem24)
        Me.AddEmpGR.Name = "AddEmpGR"
        Me.AddEmpGR.Text = "إضافة موظف"
        '
        'RibbonPage6
        '
        Me.RibbonPage6.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup21})
        Me.RibbonPage6.ImageOptions.Image = CType(resources.GetObject("RibbonPage6.ImageOptions.Image"), System.Drawing.Image)
        Me.RibbonPage6.Name = "RibbonPage6"
        Me.RibbonPage6.Text = "المصارف"
        '
        'RibbonPageGroup21
        '
        Me.RibbonPageGroup21.ItemLinks.Add(Me.BANKMENU)
        Me.RibbonPageGroup21.ItemLinks.Add(Me.BANKDEPOORWITHDRAMENU)
        Me.RibbonPageGroup21.Name = "RibbonPageGroup21"
        Me.RibbonPageGroup21.Text = "RibbonPageGroup21"
        '
        'AssGrroup
        '
        Me.AssGrroup.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup19})
        Me.AssGrroup.ImageOptions.SvgImage = CType(resources.GetObject("AssGrroup.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.AssGrroup.ImageOptions.SvgImageSize = New System.Drawing.Size(30, 30)
        Me.AssGrroup.Name = "AssGrroup"
        Me.AssGrroup.Text = "الجمعيات"
        '
        'RibbonPageGroup19
        '
        Me.RibbonPageGroup19.ItemLinks.Add(Me.BarSubItem26)
        Me.RibbonPageGroup19.ItemLinks.Add(Me.BarSubItem27)
        Me.RibbonPageGroup19.ItemLinks.Add(Me.BarSubItem14)
        Me.RibbonPageGroup19.Name = "RibbonPageGroup19"
        Me.RibbonPageGroup19.Text = "RibbonPageGroup19"
        '
        'RP6
        '
        Me.RP6.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup13})
        Me.RP6.ImageOptions.SvgImage = CType(resources.GetObject("RP6.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.RP6.ImageOptions.SvgImageSize = New System.Drawing.Size(32, 32)
        Me.RP6.Name = "RP6"
        Me.RP6.Tag = 6
        Me.RP6.Text = "الإعدادات"
        '
        'RibbonPageGroup13
        '
        Me.RibbonPageGroup13.ItemLinks.Add(Me.BarSubItem28)
        Me.RibbonPageGroup13.ItemLinks.Add(Me.BarSubItem31)
        Me.RibbonPageGroup13.ItemLinks.Add(Me.BarSubItem32)
        Me.RibbonPageGroup13.ItemLinks.Add(Me.BarSubItem33)
        Me.RibbonPageGroup13.ItemLinks.Add(Me.BarSubItem36)
        Me.RibbonPageGroup13.ItemLinks.Add(Me.BarSubItem37)
        Me.RibbonPageGroup13.Name = "RibbonPageGroup13"
        Me.RibbonPageGroup13.Text = "RibbonPageGroup13"
        '
        'RibbonPage3
        '
        Me.RibbonPage3.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.RibbonPageGroup3})
        Me.RibbonPage3.ImageOptions.Image = CType(resources.GetObject("RibbonPage3.ImageOptions.Image"), System.Drawing.Image)
        Me.RibbonPage3.Name = "RibbonPage3"
        Me.RibbonPage3.Text = "تطبيق"
        '
        'RibbonPageGroup3
        '
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarSubItem12)
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarSubItem41)
        Me.RibbonPageGroup3.Name = "RibbonPageGroup3"
        Me.RibbonPageGroup3.Text = "RibbonPageGroup3"
        '
        'RepositoryItemButtonEdit1
        '
        Me.RepositoryItemButtonEdit1.AutoHeight = False
        Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)})
        Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
        '
        'RepositoryItemButtonEdit2
        '
        Me.RepositoryItemButtonEdit2.AutoHeight = False
        Me.RepositoryItemButtonEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)})
        Me.RepositoryItemButtonEdit2.Name = "RepositoryItemButtonEdit2"
        '
        'RibbonStatusBar1
        '
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem12)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BtnBranchName)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem11)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BtnUserName)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem13)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BtnDate)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem14)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BtnTime)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem15)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BtnCNNAME)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem16)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BtnCTNAME)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem106)
        Me.RibbonStatusBar1.ItemLinks.Add(Me.BarButtonItem130)
        Me.RibbonStatusBar1.Location = New System.Drawing.Point(0, 1042)
        Me.RibbonStatusBar1.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.RibbonStatusBar1.Name = "RibbonStatusBar1"
        Me.RibbonStatusBar1.Ribbon = Me.RibbonControl1
        Me.RibbonStatusBar1.Size = New System.Drawing.Size(1938, 57)
        '
        'RibbonPageGroup7
        '
        Me.RibbonPageGroup7.ItemLinks.Add(Me.BarButtonGroup1)
        Me.RibbonPageGroup7.ItemLinks.Add(Me.BarListItem1)
        Me.RibbonPageGroup7.ItemLinks.Add(Me.BarButtonGroup2)
        Me.RibbonPageGroup7.ItemLinks.Add(Me.BarButtonGroup3)
        Me.RibbonPageGroup7.ItemsLayout = DevExpress.XtraBars.Ribbon.RibbonPageGroupItemsLayout.TwoRows
        Me.RibbonPageGroup7.Name = "RibbonPageGroup7"
        Me.RibbonPageGroup7.Text = "نموذج تقسيم العمولات"
        '
        'IntIncomeNotDel
        '
        Me.IntIncomeNotDel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.IntIncomeNotDel.ForeColor = System.Drawing.Color.White
        Me.IntIncomeNotDel.Location = New System.Drawing.Point(40, 311)
        Me.IntIncomeNotDel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.IntIncomeNotDel.Name = "IntIncomeNotDel"
        Me.IntIncomeNotDel.Size = New System.Drawing.Size(33, 34)
        Me.IntIncomeNotDel.TabIndex = 1
        Me.IntIncomeNotDel.Text = "0"
        Me.IntIncomeNotDel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnIntIncomeNotDel
        '
        Me.BtnIntIncomeNotDel.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.BtnIntIncomeNotDel.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel.Appearance.Options.UseBackColor = True
        Me.BtnIntIncomeNotDel.Appearance.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel.Appearance.Options.UseTextOptions = True
        Me.BtnIntIncomeNotDel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnIntIncomeNotDel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnIntIncomeNotDel.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel.AppearanceDisabled.Options.UseTextOptions = True
        Me.BtnIntIncomeNotDel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnIntIncomeNotDel.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnIntIncomeNotDel.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel.AppearanceHovered.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel.AppearancePressed.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnIntIncomeNotDel.ImageOptions.SvgImage = CType(resources.GetObject("BtnIntIncomeNotDel.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnIntIncomeNotDel.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnIntIncomeNotDel.Location = New System.Drawing.Point(81, 311)
        Me.BtnIntIncomeNotDel.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnIntIncomeNotDel.Name = "BtnIntIncomeNotDel"
        Me.BtnIntIncomeNotDel.Size = New System.Drawing.Size(361, 34)
        Me.BtnIntIncomeNotDel.StyleController = Me.LayoutControl1
        Me.BtnIntIncomeNotDel.TabIndex = 6
        Me.BtnIntIncomeNotDel.Text = "حوالات داخلية واردة لم تسلم"
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.BtnExtIncomeNotDel)
        Me.LayoutControl1.Controls.Add(Me.IntIncomeNotDel)
        Me.LayoutControl1.Controls.Add(Me.BtnIntIncomeNotDel)
        Me.LayoutControl1.Controls.Add(Me.InNotConfirmed)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton3)
        Me.LayoutControl1.Controls.Add(Me.OutComeDelivered)
        Me.LayoutControl1.Controls.Add(Me.BtnOutComeDelivered)
        Me.LayoutControl1.Controls.Add(Me.BtnOutComeNotDelivered)
        Me.LayoutControl1.Controls.Add(Me.OutComeNotDelivered)
        Me.LayoutControl1.Controls.Add(Me.BtnIntIncomeNotDel1)
        Me.LayoutControl1.Controls.Add(Me.FollowingInteral)
        Me.LayoutControl1.Controls.Add(Me.BtnIntIncomeNotDel11)
        Me.LayoutControl1.Controls.Add(Me.CanceledInteralIncome)
        Me.LayoutControl1.Controls.Add(Me.BtnOutcomeDeliveredInEx)
        Me.LayoutControl1.Controls.Add(Me.LookUpEdit1)
        Me.LayoutControl1.Controls.Add(Me.OutcomeDeliveredInEx)
        Me.LayoutControl1.Controls.Add(Me.BtnRecordCountConfirmCancel)
        Me.LayoutControl1.Controls.Add(Me.ExtCanceledConfrimed)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton21)
        Me.LayoutControl1.Controls.Add(Me.RecordCountConfirmCancel)
        Me.LayoutControl1.Controls.Add(Me.CONMOXSHer)
        Me.LayoutControl1.Controls.Add(Me.ExtOutcomeNotDelivered)
        Me.LayoutControl1.Controls.Add(Me.BtnRecordCountDeliveredCancel)
        Me.LayoutControl1.Controls.Add(Me.RecordCountDeliveredCancel)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Right
        Me.LayoutControl1.Location = New System.Drawing.Point(1456, 180)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(482, 862)
        Me.LayoutControl1.TabIndex = 5
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'BtnExtIncomeNotDel
        '
        Me.BtnExtIncomeNotDel.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.BtnExtIncomeNotDel.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnExtIncomeNotDel.Appearance.Options.UseBackColor = True
        Me.BtnExtIncomeNotDel.Appearance.Options.UseForeColor = True
        Me.BtnExtIncomeNotDel.Appearance.Options.UseTextOptions = True
        Me.BtnExtIncomeNotDel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnExtIncomeNotDel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnExtIncomeNotDel.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnExtIncomeNotDel.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnExtIncomeNotDel.AppearanceDisabled.Options.UseTextOptions = True
        Me.BtnExtIncomeNotDel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnExtIncomeNotDel.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnExtIncomeNotDel.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnExtIncomeNotDel.AppearanceHovered.Options.UseForeColor = True
        Me.BtnExtIncomeNotDel.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnExtIncomeNotDel.AppearancePressed.Options.UseForeColor = True
        Me.BtnExtIncomeNotDel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnExtIncomeNotDel.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnExtIncomeNotDel.Location = New System.Drawing.Point(124, 618)
        Me.BtnExtIncomeNotDel.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnExtIncomeNotDel.Name = "BtnExtIncomeNotDel"
        Me.BtnExtIncomeNotDel.Size = New System.Drawing.Size(338, 34)
        Me.BtnExtIncomeNotDel.StyleController = Me.LayoutControl1
        Me.BtnExtIncomeNotDel.TabIndex = 4
        Me.BtnExtIncomeNotDel.Text = "حوالات خارجية واردة لم تسلم"
        '
        'InNotConfirmed
        '
        Me.InNotConfirmed.BackColor = System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(228, Byte), Integer))
        Me.InNotConfirmed.ForeColor = System.Drawing.Color.White
        Me.InNotConfirmed.Location = New System.Drawing.Point(40, 269)
        Me.InNotConfirmed.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.InNotConfirmed.Name = "InNotConfirmed"
        Me.InNotConfirmed.Size = New System.Drawing.Size(33, 34)
        Me.InNotConfirmed.TabIndex = 1
        Me.InNotConfirmed.Text = "0"
        Me.InNotConfirmed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(182, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(228, Byte), Integer))
        Me.SimpleButton3.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton3.Appearance.Options.UseBackColor = True
        Me.SimpleButton3.Appearance.Options.UseForeColor = True
        Me.SimpleButton3.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton3.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton3.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton3.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton3.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton3.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton3.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton3.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton3.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton3.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton3.Location = New System.Drawing.Point(81, 269)
        Me.SimpleButton3.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(361, 34)
        Me.SimpleButton3.StyleController = Me.LayoutControl1
        Me.SimpleButton3.TabIndex = 5
        Me.SimpleButton3.Text = "حوالات داخلية صادرة لم تعتمد بعد"
        '
        'OutComeDelivered
        '
        Me.OutComeDelivered.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.OutComeDelivered.ForeColor = System.Drawing.Color.White
        Me.OutComeDelivered.Location = New System.Drawing.Point(40, 143)
        Me.OutComeDelivered.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.OutComeDelivered.Name = "OutComeDelivered"
        Me.OutComeDelivered.Size = New System.Drawing.Size(33, 34)
        Me.OutComeDelivered.TabIndex = 1
        Me.OutComeDelivered.Text = "0"
        Me.OutComeDelivered.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnOutComeDelivered
        '
        Me.BtnOutComeDelivered.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.BtnOutComeDelivered.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeDelivered.Appearance.Options.UseBackColor = True
        Me.BtnOutComeDelivered.Appearance.Options.UseForeColor = True
        Me.BtnOutComeDelivered.Appearance.Options.UseTextOptions = True
        Me.BtnOutComeDelivered.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnOutComeDelivered.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnOutComeDelivered.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeDelivered.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnOutComeDelivered.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeDelivered.AppearanceHovered.Options.UseForeColor = True
        Me.BtnOutComeDelivered.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeDelivered.AppearancePressed.Options.UseForeColor = True
        Me.BtnOutComeDelivered.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnOutComeDelivered.ImageOptions.SvgImage = CType(resources.GetObject("BtnOutComeDelivered.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnOutComeDelivered.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnOutComeDelivered.Location = New System.Drawing.Point(81, 143)
        Me.BtnOutComeDelivered.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnOutComeDelivered.Name = "BtnOutComeDelivered"
        Me.BtnOutComeDelivered.Size = New System.Drawing.Size(361, 34)
        Me.BtnOutComeDelivered.StyleController = Me.LayoutControl1
        Me.BtnOutComeDelivered.TabIndex = 2
        Me.BtnOutComeDelivered.Text = "حوالات داخلية صادرة مسلمة"
        '
        'BtnOutComeNotDelivered
        '
        Me.BtnOutComeNotDelivered.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(183, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.BtnOutComeNotDelivered.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeNotDelivered.Appearance.Options.UseBackColor = True
        Me.BtnOutComeNotDelivered.Appearance.Options.UseForeColor = True
        Me.BtnOutComeNotDelivered.Appearance.Options.UseTextOptions = True
        Me.BtnOutComeNotDelivered.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnOutComeNotDelivered.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnOutComeNotDelivered.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeNotDelivered.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnOutComeNotDelivered.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeNotDelivered.AppearanceHovered.Options.UseForeColor = True
        Me.BtnOutComeNotDelivered.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnOutComeNotDelivered.AppearancePressed.Options.UseForeColor = True
        Me.BtnOutComeNotDelivered.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnOutComeNotDelivered.ImageOptions.SvgImage = CType(resources.GetObject("BtnOutComeNotDelivered.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnOutComeNotDelivered.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnOutComeNotDelivered.Location = New System.Drawing.Point(81, 227)
        Me.BtnOutComeNotDelivered.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnOutComeNotDelivered.Name = "BtnOutComeNotDelivered"
        Me.BtnOutComeNotDelivered.Size = New System.Drawing.Size(361, 34)
        Me.BtnOutComeNotDelivered.StyleController = Me.LayoutControl1
        Me.BtnOutComeNotDelivered.TabIndex = 4
        Me.BtnOutComeNotDelivered.Text = "حوالات داخلية صادرة لم تسلم"
        '
        'OutComeNotDelivered
        '
        Me.OutComeNotDelivered.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(183, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.OutComeNotDelivered.ForeColor = System.Drawing.Color.White
        Me.OutComeNotDelivered.Location = New System.Drawing.Point(40, 227)
        Me.OutComeNotDelivered.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.OutComeNotDelivered.Name = "OutComeNotDelivered"
        Me.OutComeNotDelivered.Size = New System.Drawing.Size(33, 34)
        Me.OutComeNotDelivered.TabIndex = 1
        Me.OutComeNotDelivered.Text = "0"
        Me.OutComeNotDelivered.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnIntIncomeNotDel1
        '
        Me.BtnIntIncomeNotDel1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.BtnIntIncomeNotDel1.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel1.Appearance.Options.UseBackColor = True
        Me.BtnIntIncomeNotDel1.Appearance.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel1.Appearance.Options.UseTextOptions = True
        Me.BtnIntIncomeNotDel1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnIntIncomeNotDel1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnIntIncomeNotDel1.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel1.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel1.AppearanceDisabled.Options.UseTextOptions = True
        Me.BtnIntIncomeNotDel1.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnIntIncomeNotDel1.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnIntIncomeNotDel1.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel1.AppearanceHovered.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel1.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel1.AppearancePressed.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel1.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnIntIncomeNotDel1.ImageOptions.SvgImage = CType(resources.GetObject("BtnIntIncomeNotDel1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnIntIncomeNotDel1.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnIntIncomeNotDel1.Location = New System.Drawing.Point(81, 353)
        Me.BtnIntIncomeNotDel1.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnIntIncomeNotDel1.Name = "BtnIntIncomeNotDel1"
        Me.BtnIntIncomeNotDel1.Size = New System.Drawing.Size(361, 34)
        Me.BtnIntIncomeNotDel1.StyleController = Me.LayoutControl1
        Me.BtnIntIncomeNotDel1.TabIndex = 7
        Me.BtnIntIncomeNotDel1.Text = "حوالات داخلية صادرة ملغاة تحت الإجراء"
        '
        'FollowingInteral
        '
        Me.FollowingInteral.BackColor = System.Drawing.Color.FromArgb(CType(CType(146, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.FollowingInteral.ForeColor = System.Drawing.Color.White
        Me.FollowingInteral.Location = New System.Drawing.Point(40, 353)
        Me.FollowingInteral.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.FollowingInteral.Name = "FollowingInteral"
        Me.FollowingInteral.Size = New System.Drawing.Size(33, 34)
        Me.FollowingInteral.TabIndex = 1
        Me.FollowingInteral.Text = "0"
        Me.FollowingInteral.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnIntIncomeNotDel11
        '
        Me.BtnIntIncomeNotDel11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.BtnIntIncomeNotDel11.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel11.Appearance.Options.UseBackColor = True
        Me.BtnIntIncomeNotDel11.Appearance.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel11.Appearance.Options.UseTextOptions = True
        Me.BtnIntIncomeNotDel11.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnIntIncomeNotDel11.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnIntIncomeNotDel11.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel11.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel11.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel11.AppearanceHovered.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel11.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel11.AppearancePressed.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnIntIncomeNotDel11.ImageOptions.SvgImage = CType(resources.GetObject("BtnIntIncomeNotDel11.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnIntIncomeNotDel11.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnIntIncomeNotDel11.Location = New System.Drawing.Point(81, 479)
        Me.BtnIntIncomeNotDel11.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnIntIncomeNotDel11.Name = "BtnIntIncomeNotDel11"
        Me.BtnIntIncomeNotDel11.Size = New System.Drawing.Size(361, 34)
        Me.BtnIntIncomeNotDel11.StyleController = Me.LayoutControl1
        Me.BtnIntIncomeNotDel11.TabIndex = 10
        Me.BtnIntIncomeNotDel11.Text = "حوالة داخلية واردة عليها طلب إلغاء"
        '
        'CanceledInteralIncome
        '
        Me.CanceledInteralIncome.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.CanceledInteralIncome.ForeColor = System.Drawing.Color.Black
        Me.CanceledInteralIncome.Location = New System.Drawing.Point(40, 479)
        Me.CanceledInteralIncome.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.CanceledInteralIncome.Name = "CanceledInteralIncome"
        Me.CanceledInteralIncome.Size = New System.Drawing.Size(33, 34)
        Me.CanceledInteralIncome.TabIndex = 1
        Me.CanceledInteralIncome.Text = "0"
        Me.CanceledInteralIncome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnOutcomeDeliveredInEx
        '
        Me.BtnOutcomeDeliveredInEx.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(72, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(98, Byte), Integer))
        Me.BtnOutcomeDeliveredInEx.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnOutcomeDeliveredInEx.Appearance.Options.UseBackColor = True
        Me.BtnOutcomeDeliveredInEx.Appearance.Options.UseForeColor = True
        Me.BtnOutcomeDeliveredInEx.Appearance.Options.UseTextOptions = True
        Me.BtnOutcomeDeliveredInEx.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnOutcomeDeliveredInEx.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnOutcomeDeliveredInEx.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnOutcomeDeliveredInEx.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnOutcomeDeliveredInEx.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnOutcomeDeliveredInEx.AppearanceHovered.Options.UseForeColor = True
        Me.BtnOutcomeDeliveredInEx.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnOutcomeDeliveredInEx.AppearancePressed.Options.UseForeColor = True
        Me.BtnOutcomeDeliveredInEx.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnOutcomeDeliveredInEx.ImageOptions.SvgImage = CType(resources.GetObject("BtnOutcomeDeliveredInEx.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnOutcomeDeliveredInEx.ImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.BtnOutcomeDeliveredInEx.Location = New System.Drawing.Point(81, 185)
        Me.BtnOutcomeDeliveredInEx.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnOutcomeDeliveredInEx.Name = "BtnOutcomeDeliveredInEx"
        Me.BtnOutcomeDeliveredInEx.Size = New System.Drawing.Size(361, 34)
        Me.BtnOutcomeDeliveredInEx.StyleController = Me.LayoutControl1
        Me.BtnOutcomeDeliveredInEx.TabIndex = 3
        Me.BtnOutcomeDeliveredInEx.Text = "حوالات داخلية واردة مسلمة"
        '
        'LookUpEdit1
        '
        Me.LookUpEdit1.Location = New System.Drawing.Point(20, 576)
        Me.LookUpEdit1.Margin = New System.Windows.Forms.Padding(4)
        Me.LookUpEdit1.Name = "LookUpEdit1"
        Me.LookUpEdit1.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 5.0!)
        Me.LookUpEdit1.Properties.Appearance.Options.UseFont = True
        Me.LookUpEdit1.Properties.Appearance.Options.UseTextOptions = True
        Me.LookUpEdit1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LookUpEdit1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)})
        Me.LookUpEdit1.Properties.NullText = ""
        Me.LookUpEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.LookUpEdit1.Size = New System.Drawing.Size(442, 34)
        Me.LookUpEdit1.StyleController = Me.LayoutControl1
        Me.LookUpEdit1.TabIndex = 4
        '
        'OutcomeDeliveredInEx
        '
        Me.OutcomeDeliveredInEx.BackColor = System.Drawing.Color.FromArgb(CType(CType(72, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(98, Byte), Integer))
        Me.OutcomeDeliveredInEx.ForeColor = System.Drawing.Color.White
        Me.OutcomeDeliveredInEx.Location = New System.Drawing.Point(40, 185)
        Me.OutcomeDeliveredInEx.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.OutcomeDeliveredInEx.Name = "OutcomeDeliveredInEx"
        Me.OutcomeDeliveredInEx.Size = New System.Drawing.Size(33, 34)
        Me.OutcomeDeliveredInEx.TabIndex = 1
        Me.OutcomeDeliveredInEx.Text = "0"
        Me.OutcomeDeliveredInEx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnRecordCountConfirmCancel
        '
        Me.BtnRecordCountConfirmCancel.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(127, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.BtnRecordCountConfirmCancel.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountConfirmCancel.Appearance.Options.UseBackColor = True
        Me.BtnRecordCountConfirmCancel.Appearance.Options.UseForeColor = True
        Me.BtnRecordCountConfirmCancel.Appearance.Options.UseTextOptions = True
        Me.BtnRecordCountConfirmCancel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnRecordCountConfirmCancel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnRecordCountConfirmCancel.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountConfirmCancel.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnRecordCountConfirmCancel.AppearanceDisabled.Options.UseTextOptions = True
        Me.BtnRecordCountConfirmCancel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnRecordCountConfirmCancel.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnRecordCountConfirmCancel.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountConfirmCancel.AppearanceHovered.Options.UseForeColor = True
        Me.BtnRecordCountConfirmCancel.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountConfirmCancel.AppearancePressed.Options.UseForeColor = True
        Me.BtnRecordCountConfirmCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnRecordCountConfirmCancel.ImageOptions.SvgImage = CType(resources.GetObject("BtnRecordCountConfirmCancel.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnRecordCountConfirmCancel.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnRecordCountConfirmCancel.Location = New System.Drawing.Point(81, 395)
        Me.BtnRecordCountConfirmCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnRecordCountConfirmCancel.Name = "BtnRecordCountConfirmCancel"
        Me.BtnRecordCountConfirmCancel.Size = New System.Drawing.Size(361, 34)
        Me.BtnRecordCountConfirmCancel.StyleController = Me.LayoutControl1
        Me.BtnRecordCountConfirmCancel.TabIndex = 8
        Me.BtnRecordCountConfirmCancel.Text = "حوالات داخلية صادرة ملغاة موافق عليها"
        '
        'ExtCanceledConfrimed
        '
        Me.ExtCanceledConfrimed.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(127, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ExtCanceledConfrimed.ForeColor = System.Drawing.Color.White
        Me.ExtCanceledConfrimed.Location = New System.Drawing.Point(20, 660)
        Me.ExtCanceledConfrimed.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ExtCanceledConfrimed.Name = "ExtCanceledConfrimed"
        Me.ExtCanceledConfrimed.Size = New System.Drawing.Size(96, 34)
        Me.ExtCanceledConfrimed.TabIndex = 9
        Me.ExtCanceledConfrimed.Text = "0"
        Me.ExtCanceledConfrimed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton21
        '
        Me.SimpleButton21.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(127, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.SimpleButton21.Appearance.Options.UseBackColor = True
        Me.SimpleButton21.Location = New System.Drawing.Point(124, 660)
        Me.SimpleButton21.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton21.Name = "SimpleButton21"
        Me.SimpleButton21.Size = New System.Drawing.Size(338, 34)
        Me.SimpleButton21.StyleController = Me.LayoutControl1
        Me.SimpleButton21.TabIndex = 13
        Me.SimpleButton21.Text = "حوالات  خارجية ملغاة موافق عليها"
        '
        'RecordCountConfirmCancel
        '
        Me.RecordCountConfirmCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(127, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.RecordCountConfirmCancel.ForeColor = System.Drawing.Color.White
        Me.RecordCountConfirmCancel.Location = New System.Drawing.Point(40, 395)
        Me.RecordCountConfirmCancel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.RecordCountConfirmCancel.Name = "RecordCountConfirmCancel"
        Me.RecordCountConfirmCancel.Size = New System.Drawing.Size(33, 34)
        Me.RecordCountConfirmCancel.TabIndex = 1
        Me.RecordCountConfirmCancel.Text = "0"
        Me.RecordCountConfirmCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CONMOXSHer
        '
        Me.CONMOXSHer.Location = New System.Drawing.Point(40, 101)
        Me.CONMOXSHer.Margin = New System.Windows.Forms.Padding(4)
        Me.CONMOXSHer.Name = "CONMOXSHer"
        Me.CONMOXSHer.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 5.0!)
        Me.CONMOXSHer.Properties.Appearance.Options.UseFont = True
        Me.CONMOXSHer.Properties.Appearance.Options.UseTextOptions = True
        Me.CONMOXSHer.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CONMOXSHer.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CONMOXSHer.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)})
        Me.CONMOXSHer.Properties.NullText = ""
        Me.CONMOXSHer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.CONMOXSHer.Size = New System.Drawing.Size(402, 34)
        Me.CONMOXSHer.StyleController = Me.LayoutControl1
        Me.CONMOXSHer.TabIndex = 0
        '
        'ExtOutcomeNotDelivered
        '
        Me.ExtOutcomeNotDelivered.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(215, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.ExtOutcomeNotDelivered.ForeColor = System.Drawing.Color.Black
        Me.ExtOutcomeNotDelivered.Location = New System.Drawing.Point(20, 618)
        Me.ExtOutcomeNotDelivered.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ExtOutcomeNotDelivered.Name = "ExtOutcomeNotDelivered"
        Me.ExtOutcomeNotDelivered.Size = New System.Drawing.Size(96, 34)
        Me.ExtOutcomeNotDelivered.TabIndex = 1
        Me.ExtOutcomeNotDelivered.Text = "0"
        Me.ExtOutcomeNotDelivered.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnRecordCountDeliveredCancel
        '
        Me.BtnRecordCountDeliveredCancel.Appearance.BackColor = System.Drawing.Color.Red
        Me.BtnRecordCountDeliveredCancel.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountDeliveredCancel.Appearance.Options.UseBackColor = True
        Me.BtnRecordCountDeliveredCancel.Appearance.Options.UseForeColor = True
        Me.BtnRecordCountDeliveredCancel.Appearance.Options.UseTextOptions = True
        Me.BtnRecordCountDeliveredCancel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnRecordCountDeliveredCancel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnRecordCountDeliveredCancel.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountDeliveredCancel.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnRecordCountDeliveredCancel.AppearanceDisabled.Options.UseTextOptions = True
        Me.BtnRecordCountDeliveredCancel.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnRecordCountDeliveredCancel.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnRecordCountDeliveredCancel.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountDeliveredCancel.AppearanceHovered.Options.UseForeColor = True
        Me.BtnRecordCountDeliveredCancel.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnRecordCountDeliveredCancel.AppearancePressed.Options.UseForeColor = True
        Me.BtnRecordCountDeliveredCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnRecordCountDeliveredCancel.ImageOptions.SvgImage = CType(resources.GetObject("BtnRecordCountDeliveredCancel.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnRecordCountDeliveredCancel.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnRecordCountDeliveredCancel.Location = New System.Drawing.Point(81, 437)
        Me.BtnRecordCountDeliveredCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnRecordCountDeliveredCancel.Name = "BtnRecordCountDeliveredCancel"
        Me.BtnRecordCountDeliveredCancel.Size = New System.Drawing.Size(361, 34)
        Me.BtnRecordCountDeliveredCancel.StyleController = Me.LayoutControl1
        Me.BtnRecordCountDeliveredCancel.TabIndex = 9
        Me.BtnRecordCountDeliveredCancel.Text = "حوالات داخلية صادرة ملغاة مسلمة"
        '
        'RecordCountDeliveredCancel
        '
        Me.RecordCountDeliveredCancel.BackColor = System.Drawing.Color.Red
        Me.RecordCountDeliveredCancel.ForeColor = System.Drawing.Color.White
        Me.RecordCountDeliveredCancel.Location = New System.Drawing.Point(40, 437)
        Me.RecordCountDeliveredCancel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.RecordCountDeliveredCancel.Name = "RecordCountDeliveredCancel"
        Me.RecordCountDeliveredCancel.Size = New System.Drawing.Size(33, 34)
        Me.RecordCountDeliveredCancel.TabIndex = 1
        Me.RecordCountDeliveredCancel.Text = "0"
        Me.RecordCountDeliveredCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup1, Me.LayoutControlItem26, Me.LayoutControlItem27, Me.LayoutControlItem40, Me.LayoutControlItem28, Me.LayoutControlItem25, Me.EmptySpaceItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(482, 862)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup1.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup1.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup1.AppearanceTabPage.Header.Options.UseTextOptions = True
        Me.LayoutControlGroup1.AppearanceTabPage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup1.AppearanceTabPage.Header.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem6, Me.LayoutControlItem1, Me.LayoutControlItem4, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.LayoutControlItem17, Me.LayoutControlItem18, Me.LayoutControlItem9, Me.LayoutControlItem10, Me.LayoutControlItem11, Me.OutcomeDeliveredInExLY, Me.LayoutControlItem5, Me.LayoutControlItem13, Me.LayoutControlItem14, Me.LayoutControlItem12, Me.LayoutControlItem19, Me.LayoutControlItem20})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(450, 521)
        Me.LayoutControlGroup1.Text = "الحوالات الداخلية"
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.SimpleButton3
        Me.LayoutControlItem6.Location = New System.Drawing.Point(41, 203)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.Options.UseTextOptions = True
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.AppearanceItemCaptionDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem1.Control = Me.BtnIntIncomeNotDel
        Me.LayoutControlItem1.Location = New System.Drawing.Point(41, 245)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem4.Control = Me.BtnOutComeDelivered
        Me.LayoutControlItem4.Location = New System.Drawing.Point(41, 77)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BtnOutComeNotDelivered
        Me.LayoutControlItem2.Location = New System.Drawing.Point(41, 161)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.OutComeNotDelivered
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 161)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.InNotConfirmed
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 203)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.IntIncomeNotDel
        Me.LayoutControlItem8.Location = New System.Drawing.Point(0, 245)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem17
        '
        Me.LayoutControlItem17.Control = Me.BtnIntIncomeNotDel1
        Me.LayoutControlItem17.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem17.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem17.Location = New System.Drawing.Point(41, 287)
        Me.LayoutControlItem17.Name = "LayoutControlItem17"
        Me.LayoutControlItem17.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem17.Text = "LayoutControlItem1"
        Me.LayoutControlItem17.TextVisible = False
        '
        'LayoutControlItem18
        '
        Me.LayoutControlItem18.Control = Me.FollowingInteral
        Me.LayoutControlItem18.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem18.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem18.Location = New System.Drawing.Point(0, 287)
        Me.LayoutControlItem18.Name = "LayoutControlItem18"
        Me.LayoutControlItem18.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem18.Text = "LayoutControlItem8"
        Me.LayoutControlItem18.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.BtnIntIncomeNotDel11
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(41, 413)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem9.Text = "LayoutControlItem1"
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem10
        '
        Me.LayoutControlItem10.Control = Me.CanceledInteralIncome
        Me.LayoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem10.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem10.Location = New System.Drawing.Point(0, 413)
        Me.LayoutControlItem10.Name = "LayoutControlItem10"
        Me.LayoutControlItem10.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem10.Text = "LayoutControlItem8"
        Me.LayoutControlItem10.TextVisible = False
        '
        'LayoutControlItem11
        '
        Me.LayoutControlItem11.Control = Me.BtnOutcomeDeliveredInEx
        Me.LayoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem11.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem11.Location = New System.Drawing.Point(41, 119)
        Me.LayoutControlItem11.Name = "LayoutControlItem11"
        Me.LayoutControlItem11.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem11.Text = "LayoutControlItem1"
        Me.LayoutControlItem11.TextVisible = False
        '
        'OutcomeDeliveredInExLY
        '
        Me.OutcomeDeliveredInExLY.Control = Me.OutcomeDeliveredInEx
        Me.OutcomeDeliveredInExLY.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.OutcomeDeliveredInExLY.CustomizationFormText = "LayoutControlItem8"
        Me.OutcomeDeliveredInExLY.Location = New System.Drawing.Point(0, 119)
        Me.OutcomeDeliveredInExLY.Name = "OutcomeDeliveredInExLY"
        Me.OutcomeDeliveredInExLY.Size = New System.Drawing.Size(41, 42)
        Me.OutcomeDeliveredInExLY.Text = "LayoutControlItem8"
        Me.OutcomeDeliveredInExLY.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.OutComeDelivered
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 77)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem5.TextVisible = False
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.RecordCountConfirmCancel
        Me.LayoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem13.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 329)
        Me.LayoutControlItem13.Name = "LayoutControlItem13"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem13.Text = "LayoutControlItem8"
        Me.LayoutControlItem13.TextVisible = False
        '
        'LayoutControlItem14
        '
        Me.LayoutControlItem14.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LayoutControlItem14.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseBackColor = True
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem14.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem14.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem14.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem14.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem14.Control = Me.CONMOXSHer
        Me.LayoutControlItem14.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem14.CustomizationFormText = "كود الامانة"
        Me.LayoutControlItem14.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleRight
        Me.LayoutControlItem14.ImageOptions.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.LayoutControlItem14.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem14.Name = "LayoutControlItem14"
        Me.LayoutControlItem14.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.BackColor = System.Drawing.Color.Red
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.Options.UseBackColor = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem14.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem14.Size = New System.Drawing.Size(410, 77)
        Me.LayoutControlItem14.Text = "الحوالات الداخلية"
        Me.LayoutControlItem14.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem14.TextSize = New System.Drawing.Size(99, 27)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.BtnRecordCountConfirmCancel
        Me.LayoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem12.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem12.Location = New System.Drawing.Point(41, 329)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem12.Text = "LayoutControlItem1"
        Me.LayoutControlItem12.TextVisible = False
        '
        'LayoutControlItem19
        '
        Me.LayoutControlItem19.Control = Me.BtnRecordCountDeliveredCancel
        Me.LayoutControlItem19.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem19.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem19.Location = New System.Drawing.Point(41, 371)
        Me.LayoutControlItem19.Name = "LayoutControlItem19"
        Me.LayoutControlItem19.Size = New System.Drawing.Size(369, 42)
        Me.LayoutControlItem19.Text = "LayoutControlItem1"
        Me.LayoutControlItem19.TextVisible = False
        '
        'LayoutControlItem20
        '
        Me.LayoutControlItem20.Control = Me.RecordCountDeliveredCancel
        Me.LayoutControlItem20.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem20.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem20.Location = New System.Drawing.Point(0, 371)
        Me.LayoutControlItem20.Name = "LayoutControlItem20"
        Me.LayoutControlItem20.Size = New System.Drawing.Size(41, 42)
        Me.LayoutControlItem20.Text = "LayoutControlItem8"
        Me.LayoutControlItem20.TextVisible = False
        '
        'LayoutControlItem26
        '
        Me.LayoutControlItem26.Control = Me.BtnExtIncomeNotDel
        Me.LayoutControlItem26.Location = New System.Drawing.Point(104, 598)
        Me.LayoutControlItem26.Name = "LayoutControlItem26"
        Me.LayoutControlItem26.Size = New System.Drawing.Size(346, 42)
        Me.LayoutControlItem26.TextVisible = False
        '
        'LayoutControlItem27
        '
        Me.LayoutControlItem27.Control = Me.ExtOutcomeNotDelivered
        Me.LayoutControlItem27.Location = New System.Drawing.Point(0, 598)
        Me.LayoutControlItem27.Name = "LayoutControlItem27"
        Me.LayoutControlItem27.Size = New System.Drawing.Size(104, 42)
        Me.LayoutControlItem27.TextVisible = False
        '
        'LayoutControlItem40
        '
        Me.LayoutControlItem40.Control = Me.SimpleButton21
        Me.LayoutControlItem40.Location = New System.Drawing.Point(104, 640)
        Me.LayoutControlItem40.Name = "LayoutControlItem40"
        Me.LayoutControlItem40.Size = New System.Drawing.Size(346, 42)
        Me.LayoutControlItem40.TextVisible = False
        '
        'LayoutControlItem28
        '
        Me.LayoutControlItem28.Control = Me.ExtCanceledConfrimed
        Me.LayoutControlItem28.Location = New System.Drawing.Point(0, 640)
        Me.LayoutControlItem28.Name = "LayoutControlItem28"
        Me.LayoutControlItem28.Size = New System.Drawing.Size(104, 42)
        Me.LayoutControlItem28.TextVisible = False
        '
        'LayoutControlItem25
        '
        Me.LayoutControlItem25.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LayoutControlItem25.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem25.AppearanceItemCaption.Options.UseBackColor = True
        Me.LayoutControlItem25.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem25.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem25.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem25.Control = Me.LookUpEdit1
        Me.LayoutControlItem25.Location = New System.Drawing.Point(0, 521)
        Me.LayoutControlItem25.Name = "LayoutControlItem25"
        Me.LayoutControlItem25.Size = New System.Drawing.Size(450, 77)
        Me.LayoutControlItem25.Text = "الحوالات الخارجية"
        Me.LayoutControlItem25.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem25.TextSize = New System.Drawing.Size(99, 27)
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 682)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(450, 148)
        '
        'TAxiNotSend
        '
        Me.TAxiNotSend.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.TAxiNotSend.ForeColor = System.Drawing.Color.Black
        Me.TAxiNotSend.Location = New System.Drawing.Point(40, 447)
        Me.TAxiNotSend.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TAxiNotSend.Name = "TAxiNotSend"
        Me.TAxiNotSend.Size = New System.Drawing.Size(29, 34)
        Me.TAxiNotSend.TabIndex = 1
        Me.TAxiNotSend.Text = "0"
        Me.TAxiNotSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'EditCount
        '
        Me.EditCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.EditCount.ForeColor = System.Drawing.Color.Black
        Me.EditCount.Location = New System.Drawing.Point(40, 363)
        Me.EditCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.EditCount.Name = "EditCount"
        Me.EditCount.Size = New System.Drawing.Size(29, 34)
        Me.EditCount.TabIndex = 1
        Me.EditCount.Text = "0"
        Me.EditCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.SimpleButton1.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1.Appearance.Options.UseBackColor = True
        Me.SimpleButton1.Appearance.Options.UseForeColor = True
        Me.SimpleButton1.Appearance.Options.UseTextOptions = True
        Me.SimpleButton1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton1.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton1.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton1.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton1.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton1.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton1.Location = New System.Drawing.Point(77, 363)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton1.StyleController = Me.LayoutControl2
        Me.SimpleButton1.TabIndex = 11
        Me.SimpleButton1.Text = "حوالة داخليه عليها طلب تعديل"
        '
        'LayoutControl2
        '
        Me.LayoutControl2.Controls.Add(Me.BtnConfirm)
        Me.LayoutControl2.Controls.Add(Me.TAxiNotSend)
        Me.LayoutControl2.Controls.Add(Me.ConfirmInternalEx)
        Me.LayoutControl2.Controls.Add(Me.EditCount)
        Me.LayoutControl2.Controls.Add(Me.BtnConfirmCanceled)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl2.Controls.Add(Me.ConfirmInternalExCancel)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton51)
        Me.LayoutControl2.Controls.Add(Me.RefuseCanceled)
        Me.LayoutControl2.Controls.Add(Me.ExtConfirm)
        Me.LayoutControl2.Controls.Add(Me.ExternalConfirm)
        Me.LayoutControl2.Controls.Add(Me.BtnExtConfirmCanc)
        Me.LayoutControl2.Controls.Add(Me.ExtCanceledConfrimed1)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl2.Controls.Add(Me.CountLeaveCon)
        Me.LayoutControl2.Controls.Add(Me.BtnIntIncomeNotDel111)
        Me.LayoutControl2.Controls.Add(Me.CountLeaveEnd)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton12)
        Me.LayoutControl2.Controls.Add(Me.TAxiCansel)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton12111)
        Me.LayoutControl2.Controls.Add(Me.taxiSendFrom)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton1211)
        Me.LayoutControl2.Controls.Add(Me.TaxiADD)
        Me.LayoutControl2.Controls.Add(Me.SimpleButton121)
        Me.LayoutControl2.Dock = System.Windows.Forms.DockStyle.Left
        Me.LayoutControl2.Location = New System.Drawing.Point(0, 180)
        Me.LayoutControl2.Margin = New System.Windows.Forms.Padding(4)
        Me.LayoutControl2.Name = "LayoutControl2"
        Me.LayoutControl2.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl2.Root = Me.LayoutControlGroup2
        Me.LayoutControl2.Size = New System.Drawing.Size(471, 862)
        Me.LayoutControl2.TabIndex = 8
        Me.LayoutControl2.Text = "LayoutControl2"
        '
        'BtnConfirm
        '
        Me.BtnConfirm.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(166, Byte), Integer), CType(CType(172, Byte), Integer))
        Me.BtnConfirm.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnConfirm.Appearance.Options.UseBackColor = True
        Me.BtnConfirm.Appearance.Options.UseForeColor = True
        Me.BtnConfirm.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnConfirm.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnConfirm.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnConfirm.AppearanceHovered.Options.UseForeColor = True
        Me.BtnConfirm.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnConfirm.AppearancePressed.Options.UseForeColor = True
        Me.BtnConfirm.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnConfirm.ImageOptions.SvgImage = CType(resources.GetObject("BtnConfirm.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnConfirm.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnConfirm.Location = New System.Drawing.Point(77, 66)
        Me.BtnConfirm.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnConfirm.Name = "BtnConfirm"
        Me.BtnConfirm.Size = New System.Drawing.Size(354, 34)
        Me.BtnConfirm.StyleController = Me.LayoutControl2
        Me.BtnConfirm.TabIndex = 4
        Me.BtnConfirm.Text = "حوالات داخلية مطلوب اعتمادها"
        '
        'ConfirmInternalEx
        '
        Me.ConfirmInternalEx.BackColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(166, Byte), Integer), CType(CType(172, Byte), Integer))
        Me.ConfirmInternalEx.ForeColor = System.Drawing.Color.White
        Me.ConfirmInternalEx.Location = New System.Drawing.Point(40, 66)
        Me.ConfirmInternalEx.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ConfirmInternalEx.Name = "ConfirmInternalEx"
        Me.ConfirmInternalEx.Size = New System.Drawing.Size(29, 34)
        Me.ConfirmInternalEx.TabIndex = 8
        Me.ConfirmInternalEx.Text = "0"
        Me.ConfirmInternalEx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnConfirmCanceled
        '
        Me.BtnConfirmCanceled.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(19, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(97, Byte), Integer))
        Me.BtnConfirmCanceled.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnConfirmCanceled.Appearance.Options.UseBackColor = True
        Me.BtnConfirmCanceled.Appearance.Options.UseForeColor = True
        Me.BtnConfirmCanceled.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnConfirmCanceled.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnConfirmCanceled.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnConfirmCanceled.AppearanceHovered.Options.UseForeColor = True
        Me.BtnConfirmCanceled.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnConfirmCanceled.AppearancePressed.Options.UseForeColor = True
        Me.BtnConfirmCanceled.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnConfirmCanceled.ImageOptions.SvgImage = CType(resources.GetObject("BtnConfirmCanceled.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnConfirmCanceled.ImageOptions.SvgImageSize = New System.Drawing.Size(15, 15)
        Me.BtnConfirmCanceled.Location = New System.Drawing.Point(77, 108)
        Me.BtnConfirmCanceled.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnConfirmCanceled.Name = "BtnConfirmCanceled"
        Me.BtnConfirmCanceled.Size = New System.Drawing.Size(354, 34)
        Me.BtnConfirmCanceled.StyleController = Me.LayoutControl2
        Me.BtnConfirmCanceled.TabIndex = 4
        Me.BtnConfirmCanceled.Text = "حوالات داخلية مطلوب اعتماد إلغاؤها"
        '
        'ConfirmInternalExCancel
        '
        Me.ConfirmInternalExCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(19, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(97, Byte), Integer))
        Me.ConfirmInternalExCancel.ForeColor = System.Drawing.Color.White
        Me.ConfirmInternalExCancel.Location = New System.Drawing.Point(40, 108)
        Me.ConfirmInternalExCancel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ConfirmInternalExCancel.Name = "ConfirmInternalExCancel"
        Me.ConfirmInternalExCancel.Size = New System.Drawing.Size(29, 34)
        Me.ConfirmInternalExCancel.TabIndex = 8
        Me.ConfirmInternalExCancel.Text = "0"
        Me.ConfirmInternalExCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton51
        '
        Me.SimpleButton51.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.SimpleButton51.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton51.Appearance.Options.UseBackColor = True
        Me.SimpleButton51.Appearance.Options.UseForeColor = True
        Me.SimpleButton51.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton51.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton51.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton51.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton51.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton51.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton51.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton51.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton51.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton51.ImageOptions.SvgImageSize = New System.Drawing.Size(25, 25)
        Me.SimpleButton51.Location = New System.Drawing.Point(77, 150)
        Me.SimpleButton51.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton51.Name = "SimpleButton51"
        Me.SimpleButton51.Size = New System.Drawing.Size(354, 37)
        Me.SimpleButton51.StyleController = Me.LayoutControl2
        Me.SimpleButton51.TabIndex = 9
        Me.SimpleButton51.Text = "حوالات داخلية تم رفض إلغاؤها غير مسلمة"
        '
        'RefuseCanceled
        '
        Me.RefuseCanceled.BackColor = System.Drawing.Color.FromArgb(CType(CType(219, Byte), Integer), CType(CType(89, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.RefuseCanceled.ForeColor = System.Drawing.Color.White
        Me.RefuseCanceled.Location = New System.Drawing.Point(40, 150)
        Me.RefuseCanceled.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.RefuseCanceled.Name = "RefuseCanceled"
        Me.RefuseCanceled.Size = New System.Drawing.Size(29, 37)
        Me.RefuseCanceled.TabIndex = 10
        Me.RefuseCanceled.Text = "0"
        Me.RefuseCanceled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ExtConfirm
        '
        Me.ExtConfirm.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(166, Byte), Integer), CType(CType(172, Byte), Integer))
        Me.ExtConfirm.Appearance.ForeColor = System.Drawing.Color.White
        Me.ExtConfirm.Appearance.Options.UseBackColor = True
        Me.ExtConfirm.Appearance.Options.UseForeColor = True
        Me.ExtConfirm.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.ExtConfirm.AppearanceDisabled.Options.UseForeColor = True
        Me.ExtConfirm.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.ExtConfirm.AppearanceHovered.Options.UseForeColor = True
        Me.ExtConfirm.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.ExtConfirm.AppearancePressed.Options.UseForeColor = True
        Me.ExtConfirm.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.ExtConfirm.ImageOptions.SvgImage = CType(resources.GetObject("ExtConfirm.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.ExtConfirm.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.ExtConfirm.Location = New System.Drawing.Point(77, 195)
        Me.ExtConfirm.Margin = New System.Windows.Forms.Padding(4)
        Me.ExtConfirm.Name = "ExtConfirm"
        Me.ExtConfirm.Size = New System.Drawing.Size(354, 34)
        Me.ExtConfirm.StyleController = Me.LayoutControl2
        Me.ExtConfirm.TabIndex = 4
        Me.ExtConfirm.Text = "حوالات خارجية مطلوب اعتمادها"
        '
        'ExternalConfirm
        '
        Me.ExternalConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(166, Byte), Integer), CType(CType(172, Byte), Integer))
        Me.ExternalConfirm.ForeColor = System.Drawing.Color.White
        Me.ExternalConfirm.Location = New System.Drawing.Point(40, 195)
        Me.ExternalConfirm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ExternalConfirm.Name = "ExternalConfirm"
        Me.ExternalConfirm.Size = New System.Drawing.Size(29, 34)
        Me.ExternalConfirm.TabIndex = 8
        Me.ExternalConfirm.Text = "0"
        Me.ExternalConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnExtConfirmCanc
        '
        Me.BtnExtConfirmCanc.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.BtnExtConfirmCanc.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnExtConfirmCanc.Appearance.Options.UseBackColor = True
        Me.BtnExtConfirmCanc.Appearance.Options.UseForeColor = True
        Me.BtnExtConfirmCanc.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnExtConfirmCanc.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnExtConfirmCanc.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnExtConfirmCanc.AppearanceHovered.Options.UseForeColor = True
        Me.BtnExtConfirmCanc.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnExtConfirmCanc.AppearancePressed.Options.UseForeColor = True
        Me.BtnExtConfirmCanc.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnExtConfirmCanc.ImageOptions.SvgImage = CType(resources.GetObject("BtnExtConfirmCanc.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnExtConfirmCanc.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnExtConfirmCanc.Location = New System.Drawing.Point(77, 237)
        Me.BtnExtConfirmCanc.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnExtConfirmCanc.Name = "BtnExtConfirmCanc"
        Me.BtnExtConfirmCanc.Size = New System.Drawing.Size(354, 34)
        Me.BtnExtConfirmCanc.StyleController = Me.LayoutControl2
        Me.BtnExtConfirmCanc.TabIndex = 4
        Me.BtnExtConfirmCanc.Text = "حوالات خارجية مطلوب اعتمدا إلغاؤها"
        '
        'ExtCanceledConfrimed1
        '
        Me.ExtCanceledConfrimed1.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.ExtCanceledConfrimed1.ForeColor = System.Drawing.Color.White
        Me.ExtCanceledConfrimed1.Location = New System.Drawing.Point(40, 237)
        Me.ExtCanceledConfrimed1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ExtCanceledConfrimed1.Name = "ExtCanceledConfrimed1"
        Me.ExtCanceledConfrimed1.Size = New System.Drawing.Size(29, 34)
        Me.ExtCanceledConfrimed1.TabIndex = 8
        Me.ExtCanceledConfrimed1.Text = "0"
        Me.ExtCanceledConfrimed1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(177, Byte), Integer))
        Me.SimpleButton11.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton11.Appearance.Options.UseBackColor = True
        Me.SimpleButton11.Appearance.Options.UseForeColor = True
        Me.SimpleButton11.Appearance.Options.UseTextOptions = True
        Me.SimpleButton11.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton11.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton11.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton11.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton11.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton11.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton11.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton11.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton11.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton11.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton11.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton11.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton11.Location = New System.Drawing.Point(77, 279)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton11.StyleController = Me.LayoutControl2
        Me.SimpleButton11.TabIndex = 5
        Me.SimpleButton11.Text = "إعتماد طلب إجازة"
        '
        'CountLeaveCon
        '
        Me.CountLeaveCon.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(182, Byte), Integer), CType(CType(177, Byte), Integer))
        Me.CountLeaveCon.ForeColor = System.Drawing.Color.Black
        Me.CountLeaveCon.Location = New System.Drawing.Point(40, 279)
        Me.CountLeaveCon.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.CountLeaveCon.Name = "CountLeaveCon"
        Me.CountLeaveCon.Size = New System.Drawing.Size(29, 34)
        Me.CountLeaveCon.TabIndex = 6
        Me.CountLeaveCon.Text = "0"
        Me.CountLeaveCon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnIntIncomeNotDel111
        '
        Me.BtnIntIncomeNotDel111.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.BtnIntIncomeNotDel111.Appearance.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel111.Appearance.Options.UseBackColor = True
        Me.BtnIntIncomeNotDel111.Appearance.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel111.Appearance.Options.UseTextOptions = True
        Me.BtnIntIncomeNotDel111.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BtnIntIncomeNotDel111.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BtnIntIncomeNotDel111.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel111.AppearanceDisabled.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel111.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel111.AppearanceHovered.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel111.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.BtnIntIncomeNotDel111.AppearancePressed.Options.UseForeColor = True
        Me.BtnIntIncomeNotDel111.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.BtnIntIncomeNotDel111.ImageOptions.SvgImage = CType(resources.GetObject("BtnIntIncomeNotDel111.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnIntIncomeNotDel111.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.BtnIntIncomeNotDel111.Location = New System.Drawing.Point(77, 321)
        Me.BtnIntIncomeNotDel111.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnIntIncomeNotDel111.Name = "BtnIntIncomeNotDel111"
        Me.BtnIntIncomeNotDel111.Size = New System.Drawing.Size(354, 34)
        Me.BtnIntIncomeNotDel111.StyleController = Me.LayoutControl2
        Me.BtnIntIncomeNotDel111.TabIndex = 4
        Me.BtnIntIncomeNotDel111.Text = "إجازات منتهية المدة تحاتج إعتماد"
        '
        'CountLeaveEnd
        '
        Me.CountLeaveEnd.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.CountLeaveEnd.ForeColor = System.Drawing.Color.Black
        Me.CountLeaveEnd.Location = New System.Drawing.Point(40, 321)
        Me.CountLeaveEnd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.CountLeaveEnd.Name = "CountLeaveEnd"
        Me.CountLeaveEnd.Size = New System.Drawing.Size(29, 34)
        Me.CountLeaveEnd.TabIndex = 1
        Me.CountLeaveEnd.Text = "0"
        Me.CountLeaveEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton12
        '
        Me.SimpleButton12.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.SimpleButton12.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12.Appearance.Options.UseBackColor = True
        Me.SimpleButton12.Appearance.Options.UseForeColor = True
        Me.SimpleButton12.Appearance.Options.UseTextOptions = True
        Me.SimpleButton12.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton12.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton12.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton12.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton12.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton12.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton12.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton12.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton12.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton12.Location = New System.Drawing.Point(77, 405)
        Me.SimpleButton12.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton12.Name = "SimpleButton12"
        Me.SimpleButton12.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton12.StyleController = Me.LayoutControl2
        Me.SimpleButton12.TabIndex = 12
        Me.SimpleButton12.Text = "حوالة داخليه عليها طلب توصيل داخلي"
        '
        'TAxiCansel
        '
        Me.TAxiCansel.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.TAxiCansel.ForeColor = System.Drawing.Color.Black
        Me.TAxiCansel.Location = New System.Drawing.Point(40, 531)
        Me.TAxiCansel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TAxiCansel.Name = "TAxiCansel"
        Me.TAxiCansel.Size = New System.Drawing.Size(29, 34)
        Me.TAxiCansel.TabIndex = 1
        Me.TAxiCansel.Text = "0"
        Me.TAxiCansel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton12111
        '
        Me.SimpleButton12111.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.SimpleButton12111.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12111.Appearance.Options.UseBackColor = True
        Me.SimpleButton12111.Appearance.Options.UseForeColor = True
        Me.SimpleButton12111.Appearance.Options.UseTextOptions = True
        Me.SimpleButton12111.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton12111.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton12111.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12111.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton12111.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12111.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton12111.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton12111.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton12111.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton12111.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton12111.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton12111.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton12111.Location = New System.Drawing.Point(77, 531)
        Me.SimpleButton12111.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton12111.Name = "SimpleButton12111"
        Me.SimpleButton12111.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton12111.StyleController = Me.LayoutControl2
        Me.SimpleButton12111.TabIndex = 15
        Me.SimpleButton12111.Text = "طلب الغاء من المندوب"
        '
        'taxiSendFrom
        '
        Me.taxiSendFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.taxiSendFrom.ForeColor = System.Drawing.Color.Black
        Me.taxiSendFrom.Location = New System.Drawing.Point(40, 489)
        Me.taxiSendFrom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.taxiSendFrom.Name = "taxiSendFrom"
        Me.taxiSendFrom.Size = New System.Drawing.Size(29, 34)
        Me.taxiSendFrom.TabIndex = 1
        Me.taxiSendFrom.Text = "0"
        Me.taxiSendFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton1211
        '
        Me.SimpleButton1211.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.SimpleButton1211.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1211.Appearance.Options.UseBackColor = True
        Me.SimpleButton1211.Appearance.Options.UseForeColor = True
        Me.SimpleButton1211.Appearance.Options.UseTextOptions = True
        Me.SimpleButton1211.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton1211.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton1211.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1211.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton1211.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1211.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton1211.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton1211.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton1211.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton1211.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton1211.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton1211.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton1211.Location = New System.Drawing.Point(77, 489)
        Me.SimpleButton1211.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton1211.Name = "SimpleButton1211"
        Me.SimpleButton1211.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton1211.StyleController = Me.LayoutControl2
        Me.SimpleButton1211.TabIndex = 14
        Me.SimpleButton1211.Text = "حوالات مرسلة مع تاكسي"
        '
        'TaxiADD
        '
        Me.TaxiADD.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.TaxiADD.ForeColor = System.Drawing.Color.Black
        Me.TaxiADD.Location = New System.Drawing.Point(40, 405)
        Me.TaxiADD.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.TaxiADD.Name = "TaxiADD"
        Me.TaxiADD.Size = New System.Drawing.Size(29, 34)
        Me.TaxiADD.TabIndex = 1
        Me.TaxiADD.Text = "0"
        Me.TaxiADD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SimpleButton121
        '
        Me.SimpleButton121.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(177, Byte), Integer), CType(CType(122, Byte), Integer))
        Me.SimpleButton121.Appearance.ForeColor = System.Drawing.Color.White
        Me.SimpleButton121.Appearance.Options.UseBackColor = True
        Me.SimpleButton121.Appearance.Options.UseForeColor = True
        Me.SimpleButton121.Appearance.Options.UseTextOptions = True
        Me.SimpleButton121.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SimpleButton121.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SimpleButton121.AppearanceDisabled.ForeColor = System.Drawing.Color.White
        Me.SimpleButton121.AppearanceDisabled.Options.UseForeColor = True
        Me.SimpleButton121.AppearanceHovered.ForeColor = System.Drawing.Color.White
        Me.SimpleButton121.AppearanceHovered.Options.UseForeColor = True
        Me.SimpleButton121.AppearancePressed.ForeColor = System.Drawing.Color.White
        Me.SimpleButton121.AppearancePressed.Options.UseForeColor = True
        Me.SimpleButton121.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        Me.SimpleButton121.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton121.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton121.ImageOptions.SvgImageSize = New System.Drawing.Size(14, 14)
        Me.SimpleButton121.Location = New System.Drawing.Point(77, 447)
        Me.SimpleButton121.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton121.Name = "SimpleButton121"
        Me.SimpleButton121.Size = New System.Drawing.Size(354, 34)
        Me.SimpleButton121.StyleController = Me.LayoutControl2
        Me.SimpleButton121.TabIndex = 13
        Me.SimpleButton121.Text = "حوالات تاكسي غير مرسلة"
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup2.GroupBordersVisible = False
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup3})
        Me.LayoutControlGroup2.Name = "Root"
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(471, 862)
        Me.LayoutControlGroup2.TextVisible = False
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup3.AppearanceGroup.Options.UseTextOptions = True
        Me.LayoutControlGroup3.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup3.AppearanceGroup.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup3.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlGroup3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlGroup3.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlGroup3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.LayoutControlGroup3.GroupStyle = DevExpress.Utils.GroupStyle.Light
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LCIConfirm, Me.LCITXTConfirm, Me.LCICanceled, Me.LCITXTCanceled, Me.LayoutControlItem15, Me.LayoutControlItem16, Me.LCIConfirm1, Me.LCITXTConfirm1, Me.LCIConfirm2, Me.LCITXTConfirm2, Me.LayoutControlItem23, Me.LayoutControlItem24, Me.LayoutControlItem29, Me.LayoutControlItem30, Me.EmptySpaceItem3, Me.LayoutControlItem21, Me.LayoutControlItem22, Me.LayoutControlItem31, Me.LayoutControlItem33, Me.LayoutControlItem34, Me.LayoutControlItem35, Me.LayoutControlItem36, Me.LayoutControlItem37, Me.LayoutControlItem38, Me.LayoutControlItem39})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup3.Name = "LayoutControlGroup3"
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(439, 830)
        Me.LayoutControlGroup3.Text = "إدارة"
        '
        'LCIConfirm
        '
        Me.LCIConfirm.Control = Me.BtnConfirm
        Me.LCIConfirm.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCIConfirm.CustomizationFormText = "LayoutControlItem1"
        Me.LCIConfirm.Location = New System.Drawing.Point(37, 0)
        Me.LCIConfirm.Name = "LCIConfirm"
        Me.LCIConfirm.Size = New System.Drawing.Size(362, 42)
        Me.LCIConfirm.Text = "LayoutControlItem1"
        Me.LCIConfirm.TextVisible = False
        '
        'LCITXTConfirm
        '
        Me.LCITXTConfirm.Control = Me.ConfirmInternalEx
        Me.LCITXTConfirm.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCITXTConfirm.CustomizationFormText = "LayoutControlItem8"
        Me.LCITXTConfirm.Location = New System.Drawing.Point(0, 0)
        Me.LCITXTConfirm.Name = "LCITXTConfirm"
        Me.LCITXTConfirm.Size = New System.Drawing.Size(37, 42)
        Me.LCITXTConfirm.Text = "LayoutControlItem8"
        Me.LCITXTConfirm.TextVisible = False
        '
        'LCICanceled
        '
        Me.LCICanceled.Control = Me.BtnConfirmCanceled
        Me.LCICanceled.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCICanceled.CustomizationFormText = "LayoutControlItem1"
        Me.LCICanceled.Location = New System.Drawing.Point(37, 42)
        Me.LCICanceled.Name = "LCICanceled"
        Me.LCICanceled.Size = New System.Drawing.Size(362, 42)
        Me.LCICanceled.Text = "LayoutControlItem1"
        Me.LCICanceled.TextVisible = False
        '
        'LCITXTCanceled
        '
        Me.LCITXTCanceled.Control = Me.ConfirmInternalExCancel
        Me.LCITXTCanceled.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCITXTCanceled.CustomizationFormText = "LayoutControlItem8"
        Me.LCITXTCanceled.Location = New System.Drawing.Point(0, 42)
        Me.LCITXTCanceled.Name = "LCITXTCanceled"
        Me.LCITXTCanceled.Size = New System.Drawing.Size(37, 42)
        Me.LCITXTCanceled.Text = "LayoutControlItem8"
        Me.LCITXTCanceled.TextVisible = False
        '
        'LayoutControlItem15
        '
        Me.LayoutControlItem15.Control = Me.SimpleButton51
        Me.LayoutControlItem15.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem15.CustomizationFormText = "LayoutControlItem13"
        Me.LayoutControlItem15.Location = New System.Drawing.Point(37, 84)
        Me.LayoutControlItem15.Name = "LayoutControlItem15"
        Me.LayoutControlItem15.Size = New System.Drawing.Size(362, 45)
        Me.LayoutControlItem15.Text = "LayoutControlItem13"
        Me.LayoutControlItem15.TextVisible = False
        '
        'LayoutControlItem16
        '
        Me.LayoutControlItem16.Control = Me.RefuseCanceled
        Me.LayoutControlItem16.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem16.CustomizationFormText = "LayoutControlItem14"
        Me.LayoutControlItem16.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem16.Name = "LayoutControlItem16"
        Me.LayoutControlItem16.Size = New System.Drawing.Size(37, 45)
        Me.LayoutControlItem16.Text = "LayoutControlItem14"
        Me.LayoutControlItem16.TextVisible = False
        '
        'LCIConfirm1
        '
        Me.LCIConfirm1.Control = Me.ExtConfirm
        Me.LCIConfirm1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCIConfirm1.CustomizationFormText = "LayoutControlItem1"
        Me.LCIConfirm1.Location = New System.Drawing.Point(37, 129)
        Me.LCIConfirm1.Name = "LCIConfirm1"
        Me.LCIConfirm1.Size = New System.Drawing.Size(362, 42)
        Me.LCIConfirm1.Text = "LayoutControlItem1"
        Me.LCIConfirm1.TextVisible = False
        Me.LCIConfirm1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        '
        'LCITXTConfirm1
        '
        Me.LCITXTConfirm1.Control = Me.ExternalConfirm
        Me.LCITXTConfirm1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCITXTConfirm1.CustomizationFormText = "LayoutControlItem8"
        Me.LCITXTConfirm1.Location = New System.Drawing.Point(0, 129)
        Me.LCITXTConfirm1.Name = "LCITXTConfirm1"
        Me.LCITXTConfirm1.Size = New System.Drawing.Size(37, 42)
        Me.LCITXTConfirm1.Text = "LayoutControlItem8"
        Me.LCITXTConfirm1.TextVisible = False
        '
        'LCIConfirm2
        '
        Me.LCIConfirm2.Control = Me.BtnExtConfirmCanc
        Me.LCIConfirm2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCIConfirm2.CustomizationFormText = "LayoutControlItem1"
        Me.LCIConfirm2.Location = New System.Drawing.Point(37, 171)
        Me.LCIConfirm2.Name = "LCIConfirm2"
        Me.LCIConfirm2.Size = New System.Drawing.Size(362, 42)
        Me.LCIConfirm2.Text = "LayoutControlItem1"
        Me.LCIConfirm2.TextVisible = False
        '
        'LCITXTConfirm2
        '
        Me.LCITXTConfirm2.Control = Me.ExtCanceledConfrimed1
        Me.LCITXTConfirm2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LCITXTConfirm2.CustomizationFormText = "LayoutControlItem8"
        Me.LCITXTConfirm2.Location = New System.Drawing.Point(0, 171)
        Me.LCITXTConfirm2.Name = "LCITXTConfirm2"
        Me.LCITXTConfirm2.Size = New System.Drawing.Size(37, 42)
        Me.LCITXTConfirm2.Text = "LayoutControlItem8"
        Me.LCITXTConfirm2.TextVisible = False
        '
        'LayoutControlItem23
        '
        Me.LayoutControlItem23.Control = Me.SimpleButton11
        Me.LayoutControlItem23.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem23.CustomizationFormText = "LayoutControlItem21"
        Me.LayoutControlItem23.Location = New System.Drawing.Point(37, 213)
        Me.LayoutControlItem23.Name = "LayoutControlItem23"
        Me.LayoutControlItem23.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem23.Text = "LayoutControlItem21"
        Me.LayoutControlItem23.TextVisible = False
        '
        'LayoutControlItem24
        '
        Me.LayoutControlItem24.Control = Me.CountLeaveCon
        Me.LayoutControlItem24.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem24.CustomizationFormText = "LayoutControlItem22"
        Me.LayoutControlItem24.Location = New System.Drawing.Point(0, 213)
        Me.LayoutControlItem24.Name = "LayoutControlItem24"
        Me.LayoutControlItem24.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem24.Text = "LayoutControlItem22"
        Me.LayoutControlItem24.TextVisible = False
        '
        'LayoutControlItem29
        '
        Me.LayoutControlItem29.Control = Me.BtnIntIncomeNotDel111
        Me.LayoutControlItem29.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem29.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem29.Location = New System.Drawing.Point(37, 255)
        Me.LayoutControlItem29.Name = "LayoutControlItem29"
        Me.LayoutControlItem29.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem29.Text = "LayoutControlItem1"
        Me.LayoutControlItem29.TextVisible = False
        '
        'LayoutControlItem30
        '
        Me.LayoutControlItem30.Control = Me.CountLeaveEnd
        Me.LayoutControlItem30.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem30.CustomizationFormText = "LayoutControlItem8"
        Me.LayoutControlItem30.Location = New System.Drawing.Point(0, 255)
        Me.LayoutControlItem30.Name = "LayoutControlItem30"
        Me.LayoutControlItem30.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem30.Text = "LayoutControlItem8"
        Me.LayoutControlItem30.TextVisible = False
        '
        'EmptySpaceItem3
        '
        Me.EmptySpaceItem3.Location = New System.Drawing.Point(0, 507)
        Me.EmptySpaceItem3.Name = "EmptySpaceItem3"
        Me.EmptySpaceItem3.Size = New System.Drawing.Size(399, 257)
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.Control = Me.SimpleButton1
        Me.LayoutControlItem21.Location = New System.Drawing.Point(37, 297)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem21.TextVisible = False
        '
        'LayoutControlItem22
        '
        Me.LayoutControlItem22.Control = Me.EditCount
        Me.LayoutControlItem22.Location = New System.Drawing.Point(0, 297)
        Me.LayoutControlItem22.Name = "LayoutControlItem22"
        Me.LayoutControlItem22.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem22.TextVisible = False
        '
        'LayoutControlItem31
        '
        Me.LayoutControlItem31.Control = Me.SimpleButton12
        Me.LayoutControlItem31.Location = New System.Drawing.Point(37, 339)
        Me.LayoutControlItem31.Name = "LayoutControlItem31"
        Me.LayoutControlItem31.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem31.TextVisible = False
        '
        'LayoutControlItem33
        '
        Me.LayoutControlItem33.Control = Me.TaxiADD
        Me.LayoutControlItem33.Location = New System.Drawing.Point(0, 339)
        Me.LayoutControlItem33.Name = "LayoutControlItem33"
        Me.LayoutControlItem33.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem33.TextVisible = False
        '
        'LayoutControlItem34
        '
        Me.LayoutControlItem34.Control = Me.SimpleButton121
        Me.LayoutControlItem34.Location = New System.Drawing.Point(37, 381)
        Me.LayoutControlItem34.Name = "LayoutControlItem34"
        Me.LayoutControlItem34.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem34.TextVisible = False
        '
        'LayoutControlItem35
        '
        Me.LayoutControlItem35.Control = Me.TAxiNotSend
        Me.LayoutControlItem35.Location = New System.Drawing.Point(0, 381)
        Me.LayoutControlItem35.Name = "LayoutControlItem35"
        Me.LayoutControlItem35.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem35.TextVisible = False
        '
        'LayoutControlItem36
        '
        Me.LayoutControlItem36.Control = Me.SimpleButton1211
        Me.LayoutControlItem36.Location = New System.Drawing.Point(37, 423)
        Me.LayoutControlItem36.Name = "LayoutControlItem36"
        Me.LayoutControlItem36.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem36.TextVisible = False
        '
        'LayoutControlItem37
        '
        Me.LayoutControlItem37.Control = Me.taxiSendFrom
        Me.LayoutControlItem37.Location = New System.Drawing.Point(0, 423)
        Me.LayoutControlItem37.Name = "LayoutControlItem37"
        Me.LayoutControlItem37.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem37.TextVisible = False
        '
        'LayoutControlItem38
        '
        Me.LayoutControlItem38.Control = Me.SimpleButton12111
        Me.LayoutControlItem38.Location = New System.Drawing.Point(37, 465)
        Me.LayoutControlItem38.Name = "LayoutControlItem38"
        Me.LayoutControlItem38.Size = New System.Drawing.Size(362, 42)
        Me.LayoutControlItem38.TextVisible = False
        '
        'LayoutControlItem39
        '
        Me.LayoutControlItem39.Control = Me.TAxiCansel
        Me.LayoutControlItem39.Location = New System.Drawing.Point(0, 465)
        Me.LayoutControlItem39.Name = "LayoutControlItem39"
        Me.LayoutControlItem39.Size = New System.Drawing.Size(37, 42)
        Me.LayoutControlItem39.TextVisible = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 2000
        '
        'BarButtonItem23
        '
        Me.BarButtonItem23.Caption = "تقديم طلب إلغاء حوالة"
        Me.BarButtonItem23.Id = 72
        Me.BarButtonItem23.ImageOptions.SvgImage = CType(resources.GetObject("BarButtonItem23.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BarButtonItem23.Name = "BarButtonItem23"
        '
        'Timer3
        '
        Me.Timer3.Enabled = True
        Me.Timer3.Interval = 1000
        '
        'RibbonPage4
        '
        Me.RibbonPage4.Name = "RibbonPage4"
        Me.RibbonPage4.Text = "RibbonPage4"
        '
        'RibbonPage5
        '
        Me.RibbonPage5.Name = "RibbonPage5"
        Me.RibbonPage5.Text = "RibbonPage5"
        '
        'RibbonPage7
        '
        Me.RibbonPage7.Name = "RibbonPage7"
        Me.RibbonPage7.Text = "RibbonPage7"
        '
        'BarButtonItem111
        '
        Me.BarButtonItem111.Caption = "إلغاء حوالة خارجية صادرة"
        Me.BarButtonItem111.Id = 547
        Me.BarButtonItem111.Name = "BarButtonItem111"
        '
        'SplashScreenManager2
        '
        Me.SplashScreenManager2.ClosingDelay = 500
        '
        'RepositoryItemHypertextLabel2
        '
        Me.RepositoryItemHypertextLabel2.Name = "RepositoryItemHypertextLabel2"
        '
        'RepositoryItemHypertextLabel3
        '
        Me.RepositoryItemHypertextLabel3.Name = "RepositoryItemHypertextLabel3"
        '
        'BagWo
        '
        '
        'RibbonPage8
        '
        Me.RibbonPage8.Name = "RibbonPage8"
        Me.RibbonPage8.Text = "RibbonPage8"
        '
        'LayoutControl4
        '
        Me.LayoutControl4.Controls.Add(Me.CustAccID)
        Me.LayoutControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl4.Location = New System.Drawing.Point(471, 180)
        Me.LayoutControl4.Margin = New System.Windows.Forms.Padding(4)
        Me.LayoutControl4.Name = "LayoutControl4"
        Me.LayoutControl4.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl4.Root = Me.LayoutControlGroup6
        Me.LayoutControl4.Size = New System.Drawing.Size(985, 862)
        Me.LayoutControl4.TabIndex = 11
        Me.LayoutControl4.Text = "LayoutControl4"
        '
        'CustAccID
        '
        Me.CustAccID.Location = New System.Drawing.Point(317, 55)
        Me.CustAccID.Margin = New System.Windows.Forms.Padding(4)
        Me.CustAccID.Name = "CustAccID"
        Me.CustAccID.Properties.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 5.0!)
        Me.CustAccID.Properties.Appearance.Options.UseFont = True
        Me.CustAccID.Properties.Appearance.Options.UseTextOptions = True
        Me.CustAccID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.CustAccID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.CustAccID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)})
        Me.CustAccID.Properties.NullText = ""
        Me.CustAccID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.CustAccID.Size = New System.Drawing.Size(389, 34)
        Me.CustAccID.StyleController = Me.LayoutControl4
        Me.CustAccID.TabIndex = 5
        '
        'LayoutControlGroup6
        '
        Me.LayoutControlGroup6.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup6.GroupBordersVisible = False
        Me.LayoutControlGroup6.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem32, Me.EmptySpaceItem4, Me.EmptySpaceItem5})
        Me.LayoutControlGroup6.Name = "LayoutControlGroup6"
        Me.LayoutControlGroup6.Size = New System.Drawing.Size(985, 862)
        Me.LayoutControlGroup6.TextVisible = False
        '
        'LayoutControlItem32
        '
        Me.LayoutControlItem32.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LayoutControlItem32.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem32.AppearanceItemCaption.Options.UseBackColor = True
        Me.LayoutControlItem32.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem32.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem32.AppearanceItemCaption.Options.UseTextOptions = True
        Me.LayoutControlItem32.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LayoutControlItem32.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.LayoutControlItem32.Control = Me.CustAccID
        Me.LayoutControlItem32.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleRight
        Me.LayoutControlItem32.Location = New System.Drawing.Point(297, 0)
        Me.LayoutControlItem32.Name = "LayoutControlItem32"
        Me.LayoutControlItem32.Size = New System.Drawing.Size(397, 830)
        Me.LayoutControlItem32.Text = "كود العميل"
        Me.LayoutControlItem32.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem32.TextSize = New System.Drawing.Size(67, 27)
        Me.LayoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        '
        'EmptySpaceItem4
        '
        Me.EmptySpaceItem4.Location = New System.Drawing.Point(694, 0)
        Me.EmptySpaceItem4.Name = "EmptySpaceItem4"
        Me.EmptySpaceItem4.Size = New System.Drawing.Size(259, 830)
        '
        'EmptySpaceItem5
        '
        Me.EmptySpaceItem5.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem5.Name = "EmptySpaceItem5"
        Me.EmptySpaceItem5.Size = New System.Drawing.Size(297, 830)
        '
        'FRMMAIN
        '
        Me.AllowFormGlass = DevExpress.Utils.DefaultBoolean.[True]
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 27.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1938, 1099)
        Me.Controls.Add(Me.LayoutControl4)
        Me.Controls.Add(Me.LayoutControl2)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Controls.Add(Me.RibbonStatusBar1)
        Me.Controls.Add(Me.RibbonControl1)
        Me.Enabled = False
        Me.IconOptions.SvgImage = CType(resources.GetObject("FRMMAIN.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.Name = "FRMMAIN"
        Me.Ribbon = Me.RibbonControl1
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.StatusBar = Me.RibbonStatusBar1
        Me.Text = "  "
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RibbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemPictureEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemPictureEdit3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemPictureEdit4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemHypertextLabel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CONMOXSHer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OutcomeDeliveredInExLY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem26, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem27, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem40, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem28, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem25, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl2.ResumeLayout(False)
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCIConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCITXTConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCICanceled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCITXTCanceled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCIConfirm1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCITXTConfirm1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCIConfirm2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LCITXTConfirm2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem29, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem30, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem31, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem33, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem34, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem35, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem36, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem37, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem38, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem39, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemHypertextLabel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemHypertextLabel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl4.ResumeLayout(False)
        CType(Me.CustAccID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem32, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RibbonPage2 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RP2 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RPBASICINFO As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem8 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents CompGR As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem4 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents BarButtonItem7 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem9 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonGroup1 As DevExpress.XtraBars.BarButtonGroup
    Friend WithEvents BarSubItem1 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarListItem1 As DevExpress.XtraBars.BarListItem
    Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents BarLinkContainerItem1 As DevExpress.XtraBars.BarLinkContainerItem
    Friend WithEvents BarMdiChildrenListItem1 As DevExpress.XtraBars.BarMdiChildrenListItem
    Friend WithEvents BarDockingMenuItem1 As DevExpress.XtraBars.BarDockingMenuItem
    Friend WithEvents BarButtonGroup2 As DevExpress.XtraBars.BarButtonGroup
    Friend WithEvents BarButtonGroup3 As DevExpress.XtraBars.BarButtonGroup
    Friend WithEvents RibbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
    Friend WithEvents RibbonStatusBar1 As DevExpress.XtraBars.Ribbon.RibbonStatusBar
    Friend WithEvents RibbonPageGroup7 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarListItem2 As DevExpress.XtraBars.BarListItem
    Friend WithEvents BarSubItem2 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarStaticItem2 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents RepositoryItemPictureEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Friend WithEvents BarButtonItem10 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents RepositoryItemButtonEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents BtnBranchName As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnUserName As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnDate As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnTime As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarEditItem2 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemPictureEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Friend WithEvents BarEditItem3 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemPictureEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Friend WithEvents BarEditItem4 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemImageEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemImageEdit
    Friend WithEvents BarEditItem5 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemPictureEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Friend WithEvents BarButtonItem11 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem12 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem13 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem14 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarEditItem6 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemHypertextLabel1 As DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel
    Friend WithEvents BtnCNNAME As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnCTNAME As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem15 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem16 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnChangeUser As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem17 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem18 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BGPBranches As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BGPAgents As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RP3 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem20 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BranchSTGR As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BtnOutComeNotDelivered As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OutComeDelivered As Label
    Friend WithEvents BtnOutComeDelivered As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OutComeNotDelivered As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents InNotConfirmed As Label
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents IntIncomeNotDel As Label
    Friend WithEvents BtnIntIncomeNotDel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarButtonItem21 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnViewCanceledTransfers As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem22 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LayoutControl2 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents BtnConfirm As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ConfirmInternalEx As Label
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LCIConfirm As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LCITXTConfirm As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Timer2 As Timer
    Friend WithEvents BtnConfirmCanceled As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ConfirmInternalExCancel As Label
    Friend WithEvents LCICanceled As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LCITXTCanceled As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnCancelRequest As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem23 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem25 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem27 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnCurrencyMovement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem30 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents Timer3 As Timer
    Friend WithEvents BtnAgentCancelRequest As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnSelectAccountsBetweenBranches As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents SimpleButton51 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem15 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RefuseCanceled As Label
    Friend WithEvents LayoutControlItem16 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnIntIncomeNotDel1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem17 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents FollowingInteral As Label
    Friend WithEvents LayoutControlItem18 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnIntIncomeNotDel11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CanceledInteralIncome As Label
    Friend WithEvents LayoutControlItem10 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnOutcomeDeliveredInEx As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OutcomeDeliveredInEx As Label
    Friend WithEvents LayoutControlItem11 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents OutcomeDeliveredInExLY As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnRecordCountConfirmCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents RecordCountConfirmCancel As Label
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BTNADDDISCOUNTTYPE1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents DISCOUNTMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BTNADDDISCOUNTTYPE As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BTNMPDISVAL As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnInCreases As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnAddBonusType As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnEmpAddBonus As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents CUSTOMERSTATEMENTMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnCustomerMovement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents CUSTOMERMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnCustomer As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents CompanyMenu As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnCoBranch As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents FrmSafes As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem35 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem5 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem36 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem37 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem38 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem39 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem40 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem6 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem41 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem7 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem42 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem43 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem44 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem45 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem8 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem46 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem47 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem48 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem31 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem49 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem50 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnEMPLOYEE As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents EMPSALARYMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnCalcAllEmpSalary As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnINDIVDUALSALARYCALC As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BTNEMPCORRECTSLALRY As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem10 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem58 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem59 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RP4 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents AddEmpGR As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents EMPSTATEMENTMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BTNLOADSALARIES As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAdvancePaymentLoadAllData As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnDiscountsLoadAllData As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnIncreaseLoadAllData As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BTNEMPORCUSTWITHDRAWALLoadAllData As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnIndividualSalaryEMP As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem66 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem67 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RP5 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem69 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem70 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents GeneralExpensesMenu As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnPettyCashStatement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BrnClearFrom As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnExpenseStatement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BILLPAYMENTMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnEmpPayment As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnEmpDeposit As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup26 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem82 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem81 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BANKMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnAddBank As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddBBranch As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnDelegate As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BANKDEPOORWITHDRAMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BTNBANKDEPOSIT As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup21 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BtnUserAccessTemplate As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RP6 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BTNEMPBANKWITHDRAWAL As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BANKSTATEMENTMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnBBranchMovement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnCompny As DevExpress.XtraBars.BarSubItem
    Friend WithEvents CompanyInfo As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents EmployeeClassification As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents AddExpenses As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents NATIONALITY As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BRANCHSTATEMENTMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents btnShowSafeMovement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnMainSafeBalance As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnCurrencyStatement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents PETIESMENU As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnPettyCash As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents btnPettyCashSettlement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents AddCurrency As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents AddSafe As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddUser As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnOpeningBalance As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnANOTHEREXPENS As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BTNCURRENCYPRICE As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem5 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem6 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem19 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem26 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem24 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem28 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem29 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem32 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem33 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem34 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage4 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem52 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem53 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem3 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem54 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem55 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem4 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem56 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem57 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem60 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem61 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage5 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem62 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem63 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem64 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage7 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem65 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem71 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem72 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents CONMOXSHer As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem14 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarSubItem9 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem75 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem76 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem78 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem79 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem80 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnRecordCountDeliveredCancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem19 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RecordCountDeliveredCancel As Label
    Friend WithEvents LayoutControlItem20 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarButtonItem84 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem11 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents RibbonPage6 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarSubItem13 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem86 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem87 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents AssGrroup As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup19 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem88 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem14 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem91 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem92 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem15 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem93 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem95 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem16 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem17 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem18 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem19 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem20 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents RibbonPageGroup28 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem98 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem99 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem100 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem102 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem103 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem101 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem104 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem105 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem21 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem22 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents EditCount As Label
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BarButtonItem106 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem108 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem109 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddCancelReason As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem110 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem112 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem113 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem114 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem115 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem116 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem23 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem117 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem118 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem24 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem68 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem74 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem119 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem25 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem120 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem121 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem26 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem51 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem122 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem27 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem89 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem123 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem28 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem29 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents RibbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BtnServiceType As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem90 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem94 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem111 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LookUpEdit1 As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents BtnExtIncomeNotDel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ExtOutcomeNotDelivered As Label
    Friend WithEvents ExtCanceledConfrimed As Label
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
    Friend WithEvents SplashScreenManager2 As DevExpress.XtraSplashScreen.SplashScreenManager
    Friend WithEvents BarButtonItem124 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem125 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem126 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem107 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents ExtConfirm As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ExternalConfirm As Label
    Friend WithEvents LCIConfirm1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LCITXTConfirm1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnExtConfirmCanc As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ExtCanceledConfrimed1 As Label
    Friend WithEvents LCIConfirm2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LCITXTConfirm2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarSubItem30 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem31 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem32 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarSubItem33 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents RibbonPageGroup13 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonPageCategory1 As DevExpress.XtraBars.Ribbon.RibbonPageCategory
    Friend WithEvents BarButtonGroup4 As DevExpress.XtraBars.BarButtonGroup
    Friend WithEvents BarSubItem34 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem77 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem35 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem127 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem128 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem129 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RepositoryItemHypertextLabel2 As DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel
    Friend WithEvents RepositoryItemHypertextLabel3 As DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel
    Friend WithEvents BarButtonItem130 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem131 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddPartner As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem132 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem133 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem134 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem135 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem36 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem136 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem137 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem139 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem140 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem141 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem142 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem37 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem143 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem144 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem147 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem148 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem149 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem150 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem151 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddProject As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnProjectPartner As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddPettyCash As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnPettySettlement As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAnotherExpense As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddProExpense As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddAssest As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents AddBasiscMenu As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnProAddPettyCash As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BtnProPayPetty As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem152 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem153 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnContractorPayment As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem39 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem154 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageCategory2 As DevExpress.XtraBars.Ribbon.RibbonPageCategory
    Friend WithEvents BarButtonItem155 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem156 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem157 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem158 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem40 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem159 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem160 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem161 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem162 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem163 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem164 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddItem As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddItemDetails As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnAddSupplier As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnImportItem As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem165 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem166 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnPROEXPORTITEM As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem167 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem168 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem169 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem170 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem171 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem172 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem173 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem174 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem175 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem176 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BagWo As System.ComponentModel.BackgroundWorker
    Friend WithEvents BarButtonItem177 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem178 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnLeave As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem179 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem180 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CountLeaveCon As Label
    Friend WithEvents LayoutControlItem23 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem24 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BtnIntIncomeNotDel111 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CountLeaveEnd As Label
    Friend WithEvents LayoutControlItem29 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem30 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BarButtonItem181 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem182 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem183 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem184 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem185 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem12 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem186 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPage3 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroup3 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonPage8 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents BarButtonItem187 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem41 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem189 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem188 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem190 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem191 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem192 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem193 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LayoutControl4 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents LayoutControlGroup6 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CustAccID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents EmptySpaceItem4 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents LayoutControlItem32 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem5 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents BarButtonItem194 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents SimpleButton12 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TaxiADD As Label
    Friend WithEvents BarButtonItem195 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents TAxiNotSend As Label
    Friend WithEvents SimpleButton121 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents BarButtonItem196 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem197 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem198 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents SimpleButton1211 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents taxiSendFrom As Label
    Friend WithEvents SimpleButton12111 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TAxiCansel As Label
    Friend WithEvents BarButtonItem200 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem73 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents MoActivetion As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents SimpleButton21 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents EmptySpaceItem3 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents BarButtonItem855 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem85585 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem85 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem83 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem96 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem97 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem138 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem145 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents LayoutControlItem25 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem26 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem27 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem40 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem28 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem22 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem31 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem33 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem34 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem35 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem36 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem37 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem38 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem39 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents BarButtonItem146 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem199 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem201 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem202 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem203 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem204 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem205 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem206 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem207 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem208 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem209 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem210 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarSubItem38 As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarButtonItem211 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup2 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem212 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem213 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem214 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem215 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem216 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem217 As DevExpress.XtraBars.BarButtonItem
End Class
