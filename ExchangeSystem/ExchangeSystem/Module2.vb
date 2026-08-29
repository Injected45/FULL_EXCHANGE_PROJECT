
Imports System.Data.SqlClient
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Net.Http
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Web
Imports System.Web.Script.Serialization
Imports DevExpress.Xpo.DB.Helpers
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports DevExpress.XtraLayout.Utils
Imports DevExpress.XtraGrid
Imports Nancy.Json
Imports Newtonsoft.Json
Imports RestSharp
Imports Method = RestSharp.Method

Module Module2
    Public customerMapLink As String
    Public SAFEVAL, EMPCUSTVAL, EMPCUSTCASHVAL As Decimal
    Public IsHaiddenAcc As Boolean
    Dim Box1 As Object
    Public arl As New arabicconverter
    Public FRmIDsql As Integer
#Region "جلب رمز العملة والقيمة بالحروف "

    Public Function Cur_Code(Cur_name As String, Cur_Val As Double, isNumber As Boolean)
        Dim Box_Val As Object
        Dim Box_ArbicNumber As Object
        If Cur_name.Contains("ليبي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار ليبي", "درهم", True, True)
            Box_Val = Format(Cur_Val, "N3") + " د.ل "
        ElseIf Cur_name.Contains("تونسي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار تونسي", "فلس", True, True)
            Box_Val = Format(Cur_Val, "N3") + " د.ت "
        ElseIf Cur_name.Contains("مصري") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "جنيه مصري", "قرش", True, True)
        ElseIf Cur_name.Contains("مغربي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "درهم مغربي", "سنتيم", True, True)
            Box_Val = Format(Cur_Val, "N") + " د.م "
        ElseIf Cur_name.Contains("أمريكي") Or Cur_name.Contains("امريكي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "دولار أمريكي", "سنت", True, True)
            Box_Val = Format(Cur_Val, "N") + " $ "
        ElseIf Cur_name.Contains("يورو") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "يورو", "سنت", True, True)
            Box_Val = Format(Cur_Val, "N") + " € "
        ElseIf Cur_name.Contains("سعودي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "ريال", "هللة", True, True)
            Box_Val = Format(Cur_Val, "N") + " ر.س "
        ElseIf Cur_name.Contains("سوداني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "جنيه سوداني", "قرش", True, True)
            Box_Val = Format(Cur_Val, "N") + " ج.س "
        ElseIf Cur_name.Contains("جزائري") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار جزائري", "ستتيم", True, True)
            Box_Val = Format(Cur_Val, "N3") + " د.ج "
        ElseIf Cur_name.Contains("أردني") Or Cur_name.Contains("اردني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "دينار أردني", "قرش", True, True)
            Box_Val = Format(Cur_Val, "N3") + " د.أ "
        ElseIf Cur_name.Contains("إماراتي") Or Cur_name.Contains("اماراتي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "درهم امارتي", "فلس", True, True)
            Box_Val = Format(Cur_Val, "N3") + " د.إ "
        ElseIf Cur_name.Contains("صيني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "يوان صيني", "فين", True, True)
            Box_Val = Format(Cur_Val, "N3") + " ¥ "
        ElseIf Cur_name.Contains("استرليني") Or Cur_name.Contains("أسترليني") Or Cur_name.Contains("إسترليني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "جنيه استرليني", "بنس", True, True)
            Box_Val = Format(Cur_Val, "N") + " £ "
        ElseIf Cur_name.Contains("تركي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "ليرة تركي", "قرش", True, True)
            Box_Val = Format(Cur_Val, "N3") + " ₺ "
        ElseIf Cur_name.Contains("تشاد") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "فرنك تشادي", "ستتيم", True, True)
            Box_Val = Format(Cur_Val, "N3") + " ف.و.أ.ت "
        Else
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار ليبي", "درهم", True, True)
            Box_Val = Format(Cur_Val, "N3")
        End If
        If isNumber = False Then
            Return Box_ArbicNumber
        Else
            Return Box_Val
        End If

    End Function
#End Region
#Region "جلب قيمة خزنة موظف والفرع"
    Public Sub GETSAFEVAL(UserAccID As ULong, BranchID As Integer, CurID As Integer)
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = UserAccID}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.BigInt) With {.Value = BranchID}
        PR(2) = New SqlParameter("@CurrencyId", SqlDbType.BigInt) With {.Value = CurID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("SafeAcc_GetAccVal(@AccName,@BranchID,@CurrencyId) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            SAFEVAL = dt.Rows(0)("GetAccVal")
        End If
    End Sub

#Region "التحقق من العميل إذا كان مخفي أو لا"
    Public Sub ISHaideenAcc(AccID As ULong)
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = AccID}
        PR(1) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("CustTB_GetIsHaideenAcc(@AccName ,@UserID) AS IsHaideen", PR)
        If dt.Rows.Count > 0 Then
            IsHaiddenAcc = dt.Rows(0)("IsHaideen")
        End If
    End Sub
#End Region
    Public Sub GETSAFECurrVAL(UserAccID As ULong, BranchID As Integer, CurID As Integer)
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = UserAccID}
        PR(1) = New SqlParameter("@BranchID", SqlDbType.BigInt) With {.Value = BranchID}
        PR(2) = New SqlParameter("@CurrId", SqlDbType.BigInt) With {.Value = CurID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("SafeIDEMP_GetCurrAccValByCurrency(@AccName,@BranchID,@CurrId) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            SAFEVAL = dt.Rows(0)("GetAccVal")
        End If
    End Sub
    Public Sub GETEMPCUST(EmpAccID As ULong)
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = EmpAccID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("EMPCUST_GetAccVal(@AccName) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            EMPCUSTVAL = dt.Rows(0)("GetAccVal")
        End If
    End Sub
    'جلب القيمة غير المصرفية في حساب العميل
    Public Sub GETCASHEMPCUST(EmpAccID As ULong)
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@AccName", SqlDbType.BigInt) With {.Value = EmpAccID}
        PR(1) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = DefaultCurrency}
        PR(2) = New SqlParameter("@IsBank", SqlDbType.TinyInt) With {.Value = 0}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("Account_GetAccVal(@AccName,@CurrencyID,@IsBank) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            EMPCUSTCASHVAL = dt.Rows(0)("GetAccVal")
        End If
    End Sub

    'جلب صافي الفرع
    Public Function GETBRANCHVAL(BranchID As Integer, D1 As Date, D2 As Date)
        Dim BranchVal As Decimal
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.BigInt) With {.Value = BranchID}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("BranchAllSafes_GetAccVal(@BranchID,@D1,@D2) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            BranchVal = dt.Rows(0)("GetAccVal")
        Else
            BranchVal = 0.000
        End If
        Return BranchVal
    End Function
    Public Function GETBRANCHCURRENTVAL(BranchID As Integer, D1 As Date, D2 As Date)
        Dim BranchVal As Decimal
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.BigInt) With {.Value = BranchID}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("BranchAllSafes_GetCurrentAccVal(@BranchID,@D1,@D2) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            BranchVal = dt.Rows(0)("GetAccVal")
        Else
            BranchVal = 0
        End If
        Return BranchVal
    End Function
    'جلب إيرادات الفرع
    Public Function GETBRANCHBENEFITSVAL(BranchID As Integer, D1 As Date, D2 As Date)
        Dim BranchVal As Decimal
        Dim PR(2) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.BigInt) With {.Value = BranchID}
        PR(1) = New SqlParameter("@D1", SqlDbType.Date) With {.Value = D1}
        PR(2) = New SqlParameter("@D2", SqlDbType.Date) With {.Value = D2}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_FUNCTION_PARM("BranchAllSafes_GetBenefits(@BranchID,@D1,@D2) AS GetAccVal", PR)
        If dt.Rows.Count > 0 Then
            BranchVal = dt.Rows(0)("GetAccVal")
        End If
        Return BranchVal
    End Function
#End Region
#Region "التحقق من السمتخدم "



    '''''''''''''''''''اجراء الخاص بجلب المكس '''''
    Public Sub SubMaxID(typID As Integer, BranchID As Integer, CODEID As Integer, USRID As Integer, strp As String, fieldName As TextEdit)
        Dim PRM(3) As SqlParameter
        PRM(0) = New SqlParameter("@typID", SqlDbType.Int)
        PRM(0).Value = typID
        PRM(1) = New SqlParameter("@BranchID", SqlDbType.Int)
        PRM(1).Value = BID
        PRM(2) = New SqlParameter("@CODEID", SqlDbType.Int)
        PRM(2).Value = CODEID
        PRM(3) = New SqlParameter("@USRID", SqlDbType.Int)
        PRM(3).Value = USRID
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO(strp, PRM)
        If DT.Rows.Count > 0 Then
            fieldName.Text = ""


            fieldName.Text = DT.Rows(0)(0).ToString
        End If




    End Sub





    Sub SENIMEGFRORCASTOMARS(txtphon As TextEdit, TextBox1 As String, textmesg As TextEdit)
        Dim WebRequest As HttpWebRequest

        Dim instance_id As String = "instance22577"
        Dim token As String = "gh30s5bsg0dcbsl9"



        Dim mobile_number As String
        mobile_number = "+218" + txtphon.Text.Trim
        txtphon.Text = mobile_number
        Dim ultramsgApiUrl As String = "https://api.ultramsg.com/" + instance_id + "/messages/image"
        Dim img As Byte() = IO.File.ReadAllBytes(TextBox1)
        Dim file11 As String = Convert.ToBase64String(img)
        WebRequest = HttpWebRequest.Create("https://api.ultramsg.com/instance130356/messages/image")
        Dim postdata As String = "token=gh30s5bsg0dcbsl9&to=" & txtphon.Text & "&image=" & Web.HttpUtility.UrlEncode(file11) & "&caption=" & textmesg.Text
        Dim enc As UTF8Encoding = New System.Text.UTF8Encoding()
        Dim postdatabytes As Byte() = enc.GetBytes(postdata)
        WebRequest.Method = "POST"
        WebRequest.ContentType = "application/x-www-form-urlencoded"

        WebRequest.GetRequestStream().Write(postdatabytes, 0, postdatabytes.Length)
        Dim ret As New System.IO.StreamReader(WebRequest.GetResponse().GetResponseStream())
        Console.WriteLine(ret.ReadToEnd())
    End Sub






    Sub SINTWATSAPP_PDF_CLINT(txtphon As String, TextBox1 As String, sandname As String, bid As String, msg As String)
        Dim WebRequest As HttpWebRequest
        Dim mobile_number As String
        mobile_number = txtphon
        Dim strnwme As String
        strnwme = sandname

        Dim bytes As Byte() = IO.File.ReadAllBytes(TextBox1)
        Dim file As String = Convert.ToBase64String(bytes)
        WebRequest = HttpWebRequest.Create("https://api.ultramsg.com/instance130356/messages/image")

        Dim postdata As String = "token=gh30s5bsg0dcbsl9&to=" & mobile_number & "&image=" & Web.HttpUtility.UrlEncode(file) & "&caption=" & msg
        Dim enc As UTF8Encoding = New System.Text.UTF8Encoding()
        Dim postdatabytes As Byte() = enc.GetBytes(postdata)
        WebRequest.Method = "POST"
        WebRequest.ContentType = "application/x-www-form-urlencoded"

        WebRequest.GetRequestStream().Write(postdatabytes, 0, postdatabytes.Length)

        Dim ret As New System.IO.StreamReader(WebRequest.GetResponse().GetResponseStream())
        Console.WriteLine(ret.ReadToEnd())
    End Sub




#End Region
#Region "جلب اعدادت المسبة الخاص بالمصرف "
    Public Sub Currency_settingForBancksRet_getstting(ID As Integer)
        Dim prm(4) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        prm(1) = New SqlParameter("@BuyPrice", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(2) = New SqlParameter("@SalePrice", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(3) = New SqlParameter("@RetBuyPrice", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        prm(4) = New SqlParameter("@retSalePrice", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("Currency_settingForBancksRet_getstting", prm)

        'My.Settings.BuyPrice = prm(1).Value

        'My.Settings.SalePrice = prm(2).Value
        'My.Settings.RetBuyPrice = prm(3).Value
        'My.Settings.retSalePrice = prm(4).Value

        My.Settings.Save()
        dt.Dispose()
    End Sub
#End Region
#Region "جلب رمز العملة والقيمة بالحروف "

    Public Function Cur_Code(Cur_name As String, Cur_Val As Double, isNumber As Boolean, Formats As String)
        Dim Box_Val As Object
        Dim Box_ArbicNumber As Object
        If Cur_name.Contains("ليبي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار ليبي", "درهم", True, True)
            Box_Val = Format(Cur_Val, Formats) + " د.ل "
        ElseIf Cur_name.Contains("تونسي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار تونسي", "فلس", True, True)
            Box_Val = Format(Cur_Val, Formats) + " د.ت "
        ElseIf Cur_name.Contains("مصري") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "جنيه مصري", "قرش", True, True)
            Box_Val = Format(Cur_Val, Formats) + " ج.م "
        ElseIf Cur_name.Contains("مغربي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "درهم مغربي", "سنتيم", True, True)
            Box_Val = Format(Cur_Val, Formats) + " د.م "
        ElseIf Cur_name.Contains("أمريكي") Or Cur_name.Contains("امريكي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "دولار أمريكي", "سنت", True, True)

            Box_Val = Format(Cur_Val, Formats) + " $ "

        ElseIf Cur_name.Contains("يورو") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "يورو", "سنت", True, True)

            Box_Val = Format(Cur_Val, Formats) + " € "

        ElseIf Cur_name.Contains("سعودي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "ريال", "هللة", True, True)

            Box_Val = Format(Cur_Val, Formats) + " ر.س "

        ElseIf Cur_name.Contains("سوداني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "جنيه سوداني", "قرش", True, True)

            Box_Val = Format(Cur_Val, Formats) + " ج.س "

        ElseIf Cur_name.Contains("جزائري") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار جزائري", "ستتيم", True, True)

            Box_Val = Format(Cur_Val, Formats) + " د.ج "

        ElseIf Cur_name.Contains("أردني") Or Cur_name.Contains("اردني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "دينار أردني", "قرش", True, True)

            Box_Val = Format(Cur_Val, Formats) + " د.أ "

        ElseIf Cur_name.Contains("إماراتي") Or Cur_name.Contains("اماراتي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "درهم امارتي", "فلس", True, True)

            Box_Val = Format(Cur_Val, Formats) + " د.إ "

        ElseIf Cur_name.Contains("صيني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "يوان صيني", "فين", True, True)

            Box_Val = Format(Cur_Val, Formats) + " ¥ "

        ElseIf Cur_name.Contains("استرليني") Or Cur_name.Contains("أسترليني") Or Cur_name.Contains("إسترليني") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "جنيه استرليني", "بنس", True, True)

            Box_Val = Format(Cur_Val, Formats) + " £ "

        ElseIf Cur_name.Contains("تركي") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "ليرة تركي", "قرش", True, True)

            Box_Val = Format(Cur_Val, Formats) + " ₺ "

        ElseIf Cur_name.Contains("تشاد") Then
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 2, "فرنك تشادي", "ستتيم", True, True)

            Box_Val = Format(Cur_Val, Formats) + " ف.وأ.ت "

        Else
            Box_ArbicNumber = arl.numtolit(Val(Cur_Val), 3, "دينار ليبي", "درهم", True, True)

            Box_Val = Format(Cur_Val, Formats)

        End If
        If isNumber = False Then
            Return Box_ArbicNumber
        Else
            Return Box_Val
        End If

    End Function
#End Region
#Region "جلب رمز العملة  "
    Public Function Cur_Code1(Cur_name As String)


        If Cur_name.Contains("ليبي") Then
            Box1 = "د.ل"
        ElseIf Cur_name.Contains("تونسي") Then

            Box1 = " د.ت "
        ElseIf Cur_name.Contains("مصري") Then

            Box1 = " ج.م "
        ElseIf Cur_name.Contains("أمريكي") Then

            Box1 = " $ "
        ElseIf Cur_name.Contains("يورو") Then

            Box1 = " € "
        ElseIf Cur_name.Contains("سعودي") Then

            Box1 = " ر.س "
        ElseIf Cur_name.Contains("سوداني") Then

            Box1 = " ج.س "
        ElseIf Cur_name.Contains("جزائري") Then

            Box1 = " د.ج "
        ElseIf Cur_name.Contains("مغربي") Then

            Box1 = " د.م "
        ElseIf Cur_name.Contains("أردني") Then

            Box1 = " د.أ "
        ElseIf Cur_name.Contains("إماراتي") Or Cur_name.Contains("اماراتي") Then

            Box1 = " د.إ "

        ElseIf Cur_name.Contains("صيني") Then

            Box1 = " ¥ "
        ElseIf Cur_name.Contains("استرليني") Or Cur_name.Contains("أسترليني") Or Cur_name.Contains("إسترليني") Then

            Box1 = " £ "


        ElseIf Cur_name.Contains("تركي") Then

            Box1 = " ₺ "
        ElseIf Cur_name.Contains("تشاد") Then

            Box1 = " ف.و.أ.ت "

        End If
        Return Box1
    End Function
#End Region
#Region "جلب العملة الافتراضية"
    Public Function GETDefaultCurr(CountryID As Integer)
        Dim parm(0) As SqlParameter
        parm(0) = New SqlParameter("@CountryID", SqlDbType.Int)
        parm(0).Value = CountryID
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CurrencyMainTb_Get_Defult_BasedOnCountry", parm)
        DefaultCurrency = dt.Rows(0)("ID")
        Return dt
    End Function
    Public Function GETPhoneAccID(AccID As ULong) As DataTable

        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_TXT("SELECT at.AccPhone FROM AccountsTb at WHERE accid=N'" & AccID & "'")

        Return dt
    End Function
#End Region
#Region "Get EMP Based on Branch"
    Public Function GETEMPCODE(ByVal ID As Integer) As String
        If ID <= 0 Then Return String.Empty
        Try
            Dim prm As SqlParameter() = {
            New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        }
            Dim dt As DataTable = RUN_QUARY_PRO("EMPLOYEETB_SearchByID", prm)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return If(IsDBNull(dt.Rows(0)("Code")), String.Empty, dt.Rows(0)("Code").ToString())
            End If
            Return String.Empty
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
#End Region
    Public Function modFORnamber(x As Integer) As Integer


        If x < 5 Then
            Return 0

        ElseIf x > 5 And x < 10 Then
            Return 5

        ElseIf x.ToString.Length = 2 Then
            Dim st As String = x
            Dim mid2 As String = st.Substring(1, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(1, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If
        ElseIf x.ToString.Length = 3 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(2, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(2, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If

            ''''''''''''''''''''''''''''''''''''''
        ElseIf x.ToString.Length = 4 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(3, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(3, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If

        ElseIf x.ToString.Length = 5 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(4, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(4, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" And mid2 > "0" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If
        ElseIf x.ToString.Length = 6 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(5, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(5, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x





            Else

                Return x
            End If


        ElseIf x.ToString.Length = 7 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(6, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(6, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If
        ElseIf x.ToString.Length = 8 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(7, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(7, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If


        ElseIf x.ToString.Length = 9 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(8, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(8, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If



        ElseIf x.ToString.Length = 10 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(9, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(9, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If

        ElseIf x.ToString.Length = 11 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(10, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(10, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If


        ElseIf x.ToString.Length = 12 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(11, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(11, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If

        ElseIf x.ToString.Length = 13 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(12, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(12, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If

        ElseIf x.ToString.Length = 14 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(13, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(13, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If


        ElseIf x.ToString.Length = 15 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(14, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(14, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If

        ElseIf x.ToString.Length = 16 Then

            Dim st As String = x
            Dim mid2 As String = st.Substring(15, 1)

            If mid2 < "5" And mid2 > "0" Then

                Dim istr As String = x
                Dim ostr As String = istr.Remove(15, 1)

                x = ostr + "0"

                Return x
            ElseIf mid2 > "5" Then
                'And mid2 < "10" Then
                Dim istr As String = x
                Dim ostr As Integer = "5" - mid2

                Dim dd As Integer
                dd = x + ostr

                x = dd

                Return x
            Else
                Return x
            End If
        Else
            Return x
        End If



    End Function
    Public Function SELECTALLTB_COMPANY() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("SELECTALLTB_PROFILE_COMPANY")
        Return DT
    End Function
#Region "شغل الصلاحيات+رسائل الواتس اب"
    Public SQLASSOCIATIONTB_phone As Integer ''جلب رقم المشترك في جمعية 
    'Public wtsabID_group_teset As String  '' = "120363380139020724@g.us" '' متغير يقوم بجلب الغرفة التجريب
    Public UpateAppliction As Boolean
    Public HIID As String, Procc As String, PCnames As String, PCBRanch As Integer
    ''select proc for secuorte -- جلب الصلاحيات 
    Public Function PCBRanchs(HIID As String, Procc As String) As Integer
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@HDDID", SqlDbType.NVarChar, -1) With {.Value = HIID}
        prm(1) = New SqlParameter("@ProccessID", SqlDbType.NVarChar, -1) With {.Value = Procc}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ActivivationTb_getprach", prm)
        If dt.Rows.Count > 0 Then
            Return PCBRanchs
        Else
            Return 0
        End If


    End Function

    Public Function get_sub_MACadders() 'داله تقوم باارجالع رقم ا لمعالج'

        Dim val() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
        Return val(1).GetPhysicalAddress.ToString


    End Function
    Public Function GetDriveSerialNumber() As String
        Dim DriveSerial As Long
        Dim fso As Object, Drv As Object

        fso = CreateObject("Scripting.FileSystemObject")
        Drv = fso.GetDrive(fso.GetDriveName("C:\"))

        With Drv
            If .IsReady Then
                DriveSerial = .SerialNumber
            Else
                DriveSerial = -1
            End If
        End With
        Drv = Nothing
        fso = Nothing
        GetDriveSerialNumber = Hex(DriveSerial)
    End Function

    Public Function get_sub_proceser() 'داله تقوم باارجالع رقم ا لمعالج'

        Dim val As String = ""

        Dim mos As New ManagementObjectSearcher("SELECT * FROM Win32_processor")
        Dim mo As New ManagementObject
        For Each mo In mos.Get
            val = mo("ProcessorID").ToString
        Next
        Return val
    End Function
    Public Function PCName() As String

        Dim hostName As String = Dns.GetHostName()
        PCName = hostName
    End Function
    Public Function SElectUEserFormButtn(ScreenID As Integer, USID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Clear()
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        prm(1) = New SqlParameter("@USID", SqlDbType.Int) With {.Value = USID}
        dt = RUN_QUARY_PRO("SElectUEserFormButtn", prm)

        Return dt
    End Function


    ''دالة تقوم بارجاع رقم تيلفون الموظفين + الهعملاء + االوكلاء + المدنيوين  
    Public Function GET_PHONE_SaenFroWtsaap(Id As ULong) As String
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = Id}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("GET_PHONE_SaenFroWtsaap", prm)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0)("PHONE1").ToString
        End If
    End Function

    '' جلب رقم هاتف الموظف بناءا على الاي دي
    Public Function GET_EMPPHONE_SaenFroWtsaap(Id As ULong) As String
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = Id}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("GET_EMPPHONE_SaenFroWtsaap", prm)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0)("PHONE1").ToString
        End If
    End Function

    ''جلب  حساب كود حساب الموظف او العميل او الوكيل علي حسب رقم المستهخدم 
    Public Function GET_codefor_Acount_SaenFroWtsaap(Id As ULong) As String
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = Id}
        Dim dt As New DataTable
        dt = RUN_QUARY_PRO("GET_PHONE_SaenFroWtsaap", prm)
        Return dt.Rows(0)("code")
    End Function
    ''جلب  حساب كود حساب الموظف علي حسب رقم الاي دي 
    Public Function GET_EMPcodefor_Acount_SaenFroWtsaap(Id As ULong) As String
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@EMPID", SqlDbType.Int) With {.Value = Id}
        Dim dt As New DataTable
        dt = RUN_QUARY_PRO("GET_EMPPHONE_SaenFroWtsaap", prm)
        Return dt.Rows(0)("code")
    End Function
    Public Function GET_codefor_Acount_BasedName(Id As ULong) As String
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@AccID", SqlDbType.Int) With {.Value = Id}
        Dim dt As New DataTable
        dt = RUN_QUARY_PRO("GET_PHONE_SaenFroWtsaap", prm)
        Return dt.Rows(0)("code")
    End Function

    Public Sub Get_Acctvionpc(fm As XtraForm)
        Try



            Dim hardid As String = GetDriveSerialNumber()

            Dim proccID As String = get_sub_proceser()

            Dim MaccID As String = get_sub_MACadders()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''
            HIID = hardid
            Procc = proccID
            PCnames = PCName()

            PCBRanch = PCBRanchs(HIID, Procc)

            Dim PRM(1) As SqlParameter
            PRM(0) = New SqlParameter("@ProccessID", SqlDbType.NVarChar - 1)
            PRM(0).Value = proccID
            PRM(1) = New SqlParameter("@HDDID", SqlDbType.NVarChar - 1)
            PRM(1).Value = hardid
            'PRM(2) = New SqlParameter("@MaccAddressID", SqlDbType.NVarChar - 1)
            'PRM(2).Value = MaccID
            Dim DTs As New DataTable
            DTs.Clear()

            DTs = RUN_QUARY_PRO("ActivivationTb_SELECHDDPROMac", PRM)

            ' DIAGNOSTIC: "هذا الجهاز غير مرخص" is reported for ANY empty result — including a data-layer
            ' failure that returned no rows. Log what we actually asked for and got, so the two are told apart.
            MD_MYSQL.LogMyInfo("ACTIVATION check: USE_MYSQL=" & MD_MYSQL.USE_MYSQL.ToString() &
                               " ProccessID=[" & proccID & "] HDDID=[" & hardid & "]" &
                               " -> " & If(DTs Is Nothing, "DataTable=Nothing (data layer failed)",
                                           "rows=" & DTs.Rows.Count.ToString()))

            If DTs Is Nothing OrElse DTs.Rows.Count = 0 Then
                ErrorMessage(fm, "رسالة معلومات", "هذا الجهاز غير مرخص، يرجى الاتصال بمطور النظام لتفعيله")
                SQLCON.Dispose()
                QSCON.Dispose()
                FRMMAIN.SplashScreenManager1.CloseWaitForm()
                Application.Exit()
            ElseIf DTs.Rows.Count > 0 Then

                FRMMAIN.SplashScreenManager1.CloseWaitForm() 'If UpateAppliction = 1 Then


            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة معلومات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    '' Get for Uesers rolls mane  
    'Public Sub CHECKOFORMVISIBEL_FalseOrTrue(frm As FRMMAIN, stordNAMe As String,
    '                                     prm() As SqlParameter, fieldNameShortName As String, fieldNameCanShow As String)
    '    Try
    '        Dim DT As New DataTable
    '        DT = RUN_QUARY_PRO(stordNAMe, prm)

    '        If DT.Rows.Count > 0 Then
    '            For Each row As DataRow In DT.Rows
    '                Dim controlName As String = row(fieldNameShortName).ToString()
    '                Dim canShow As Boolean = Convert.ToBoolean(row(fieldNameCanShow))


    '                Dim control = frm.Controls.Find(controlName, True).FirstOrDefault()

    '                If control IsNot Nothing Then
    '                    If TypeOf control Is SimpleButton Then

    '                        Dim targetButton As SimpleButton = DirectCast(control, SimpleButton)

    '                        targetButton.Visible = canShow
    '                    End If
    '                End If


    '                Dim barItem = frm.RibbonControl1.Items.OfType(Of BarButtonItem).FirstOrDefault(Function(x) x.Name = controlName)
    '                If barItem IsNot Nothing Then
    '                    barItem.Visibility = If(canShow, BarItemVisibility.Always, BarItemVisibility.Never)
    '                End If


    '                Dim barSubItem = frm.RibbonControl1.Items.OfType(Of BarSubItem).FirstOrDefault(Function(x) x.Name = controlName)
    '                If barSubItem IsNot Nothing Then
    '                    barSubItem.Visibility = If(canShow, BarItemVisibility.Always, BarItemVisibility.Never)
    '                End If

    '                Dim ribbonPage = frm.RibbonControl1.Pages.OfType(Of DevExpress.XtraBars.Ribbon.RibbonPage)().FirstOrDefault(Function(x) x.Name = controlName)
    '                If ribbonPage IsNot Nothing Then


    '                    ribbonPage.Visible = canShow
    '                End If






    '            Next
    '        Else
    '            ErrorMessage(FrmLogin, "رسالة خطأ", "عذرا هذه المستخدم لايوجد لديه صلاحيات في الوقت الحالي الرجاء اضافة صلاحيات لهذا المستخدم التواصل مع مدير النظام ")
    '            Application.Exit()
    '        End If
    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "رسالة خطأ من النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        Application.Exit()
    '    End Try
    'End Sub





    Public Sub CHECKOFORMVISIBEL_FalseOrTrue(frm As FRMMAIN, stordNAMe As String,
                                         prm() As SqlParameter, fieldNameShortName As String, fieldNameCanShow As String)
        Try
            Dim DT As New DataTable
            DT = RUN_QUARY_PRO(stordNAMe, prm)

            If DT.Rows.Count > 0 Then
                For Each row As DataRow In DT.Rows
                    Dim controlName As String = row(fieldNameShortName).ToString()
                    Dim canShow As Boolean = Convert.ToBoolean(row(fieldNameCanShow))

                    ' البحث عن عنصر تحكم عادي (Control مثل Button)
                    Dim control = frm.Controls.Find(controlName, True).FirstOrDefault()

                    If control IsNot Nothing Then
                        If TypeOf control Is SimpleButton Then
                            Dim targetButton As SimpleButton = DirectCast(control, SimpleButton)
                            targetButton.Visible = canShow
                        End If
                    End If

                    ' إخفاء العناصر في RibbonControl (BarButtonItem)
                    Dim barItem = frm.RibbonControl1.Items.OfType(Of BarButtonItem)().
                              FirstOrDefault(Function(x) x.Name = controlName)
                    If barItem IsNot Nothing Then
                        barItem.Visibility = If(canShow, BarItemVisibility.Always, BarItemVisibility.Never)
                    End If

                    ' إخفاء BarSubItem
                    Dim barSubItem = frm.RibbonControl1.Items.OfType(Of BarSubItem)().
                                 FirstOrDefault(Function(x) x.Name = controlName)
                    If barSubItem IsNot Nothing Then
                        barSubItem.Visibility = If(canShow, BarItemVisibility.Always, BarItemVisibility.Never)
                    End If

                    ' إخفاء RibbonPage
                    Dim ribbonPage = frm.RibbonControl1.Pages.OfType(Of Ribbon.RibbonPage)().
                                 FirstOrDefault(Function(x) x.Name = controlName)
                    If ribbonPage IsNot Nothing Then
                        ribbonPage.Visible = canShow
                    End If

                    ' إخفاء عناصر LayoutControlItem
                    For Each Layouts In frm.Controls.OfType(Of LayoutControl)()
                        Dim layoutItem = Layouts.Items.OfType(Of LayoutControlItem)().
                                     FirstOrDefault(Function(x) x.Name = controlName)

                        If layoutItem IsNot Nothing Then


                            layoutItem.Visibility = If(canShow,
                                                 LayoutVisibility.Always,
                                                 LayoutVisibility.Never)
                        End If
                    Next

                Next
            Else
                ErrorMessage(FrmLogin, "رسالة خطأ", "عذرا، هذا المستخدم لا يملك صلاحيات حاليًا. الرجاء إضافة صلاحيات لهذا المستخدم أو التواصل مع مدير النظام.")
                Application.Exit()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ من النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Application.Exit()
        End Try
    End Sub


#Region "اكواد الواتساب المحدثة كلياً"

    ' ---------------------------------------------------------
    ' ثوابت مشتركة لسيرفر wa.rhalla.online بناءً على التوثيق المرجعي
    ' ---------------------------------------------------------
    'Private SESSION_ID As String = Module1.session_id
    'Private API_KEY As String = "owa_k1_7c2dd55e99a11e97aef495d122ceba8e150e4942f450269a6a85ba1c020fda18"
    'Private BASE_URL As String = "https://wa.rhalla.online/api/sessions/" & SESSION_ID
    Private BASE_URL As String
    ' ---------------------------------------------------------
    ' دالة مساعدة: تنظيف رقم الهاتف
    ' ---------------------------------------------------------
    ' ---------------------------------------------------------
    ' دالة مساعدة: تنظيف رقم الهاتف وإضافة مفاتيح الدول بذكاء
    ' ---------------------------------------------------------
    Private Function CleanPhone(ByVal rawPhone As String) As String
        ' 1. حماية من القيم الفارغة
        If String.IsNullOrWhiteSpace(rawPhone) Then Return ""

        Dim number As String = rawPhone.Trim()

        ' 2. إذا كان الرقم قروب واتساب، نرجعه كما هو مباشرة دون التلاعب به
        If number.EndsWith("@g.us") Then
            Return number
        End If

        ' 3. إزالة لاحقة @c.us مؤقتاً (لو كانت موجودة) لكي ننظف الرقم براحتنا
        If number.EndsWith("@c.us") Then
            number = number.Replace("@c.us", "")
        End If

        ' 4. تنظيف الرقم من أي رموز (مسافات، +، شرطات) مع الإبقاء على الأرقام فقط
        number = New String(number.Where(Function(c) Char.IsDigit(c)).ToArray())

        ' 5. إزالة الأصفار المزدوجة في البداية إن وجدت (مثل 00218 أو 0020)
        If number.StartsWith("00") Then
            number = number.Substring(2)
        End If

        ' ==========================================
        ' 6. المعالجة الذكية للدول (ليبيا ومصر)
        ' ==========================================

        ' --- أولاً: الأرقام الليبية ---
        If number.StartsWith("09") AndAlso number.Length = 10 Then
            ' نحذف الصفر المحلي ونضيف 218
            number = "218" & number.Substring(1)
        ElseIf number.StartsWith("9") AndAlso number.Length = 9 Then
            number = "218" & number
        End If

        ' --- ثانياً: الأرقام المصرية ---
        If number.StartsWith("01") AndAlso number.Length = 11 Then
            ' نحذف الصفر المحلي ونضيف 20
            number = "20" & number.Substring(1)
        ElseIf number.StartsWith("1") AndAlso number.Length = 10 Then
            number = "20" & number
        End If

        ' 7. إرجاع الرقم مع إضافة لاحقة الواتساب الفردية الإجبارية للسيرفر
        Return number & "@c.us"
    End Function

    ' ---------------------------------------------------------
    ' دالة مساعدة: تنظيف النصوص لتتوافق مع معيار JSON الصارم
    ' ---------------------------------------------------------
    Private Function CleanJsonText(text As String) As String
        If String.IsNullOrEmpty(text) Then Return ""
        Return text.Replace("\", "\\").Replace("""", "\""").Replace(vbCrLf, "\n").Replace(vbLf, "\n")
    End Function

    ' ---------------------------------------------------------
    ' دالة مساعدة: إنشاء HttpWebRequest متوافق تماماً مع التوثيق الرسمي
    ' ---------------------------------------------------------
    Private Function CreateRequest(endpointUrl As String, method As String) As HttpWebRequest
        Dim req As HttpWebRequest = CType(Net.WebRequest.Create(endpointUrl), HttpWebRequest)
        req.Method = method
        req.ContentType = "application/json"
        req.Timeout = 30000 ' مهلة 30 ثانية لرفع الملفات الكبيرة بأمان

        ' التثبيت بناءً على الكود المرجعي المرفق منك
        req.Headers.Add("X-API-Key", apiKey)

        Return req
    End Function


    Private Function UploadFileToWebLink(filePath As String) As String
        Try
            ' 1. تجاوز أخطاء شهادات الأمان
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(se, cert, chain, sslerror) True
            System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)

            Dim boundary As String = "----WebKitFormBoundary" & DateTime.Now.Ticks.ToString("x")
            Dim req As HttpWebRequest = CType(Net.WebRequest.Create("https://tmpfiles.org/api/v1/upload"), HttpWebRequest)
            req.Method = "POST"
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            req.ContentType = "multipart/form-data; boundary=" & boundary
            req.Timeout = 60000

            Dim ms As New IO.MemoryStream()
            Dim writer As New IO.StreamWriter(ms)

            ' استخراج امتداد الملف الأصلي وتكوين اسم إنجليزي آمن جداً
            Dim fileExtension As String = IO.Path.GetExtension(filePath)
            Dim safeFileName As String = "doc_" & DateTime.Now.ToString("yyyyMMddHHmmss") & fileExtension

            writer.WriteLine("--" & boundary)
            writer.WriteLine("Content-Disposition: form-data; name=""file""; filename=""" & safeFileName & """")
            writer.WriteLine("Content-Type: application/octet-stream")
            writer.WriteLine()
            writer.Flush()

            Dim fileBytes As Byte() = IO.File.ReadAllBytes(filePath)
            ms.Write(fileBytes, 0, fileBytes.Length)

            writer.WriteLine()
            writer.WriteLine("--" & boundary & "--")
            writer.Flush()

            req.ContentLength = ms.Length
            Using reqStream As IO.Stream = req.GetRequestStream()
                ms.Position = 0
                ms.CopyTo(reqStream)
            End Using

            Using response As WebResponse = req.GetResponse()
                Using reader As New IO.StreamReader(response.GetResponseStream())
                    Dim json As String = reader.ReadToEnd()
                    Dim searchKey As String = """url"":"""
                    Dim idx As Integer = json.IndexOf(searchKey)
                    If idx > -1 Then
                        Dim startIdx As Integer = idx + searchKey.Length
                        Dim endIdx As Integer = json.IndexOf("""", startIdx)
                        If endIdx > -1 Then
                            Dim rawUrl As String = json.Substring(startIdx, endIdx - startIdx)
                            Return rawUrl.Replace("tmpfiles.org/", "tmpfiles.org/dl/")
                        End If
                    End If
                End Using
            End Using

        Catch ex As WebException
            Dim errorMsg As String = ex.Message
            If ex.Response IsNot Nothing Then
                Using reader As New IO.StreamReader(ex.Response.GetResponseStream())
                    errorMsg &= vbCrLf & "تفاصيل من السيرفر: " & reader.ReadToEnd()
                End Using
            End If
            MsgBox("تعذر رفع الملف برمجياً:" & vbCrLf & errorMsg, MsgBoxStyle.Critical, "تفاصيل الخطأ التقني")
        Catch ex As Exception
            MsgBox("خطأ عام في معالجة الملف:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "تفاصيل الخطأ التقني")
        End Try

        Return ""
    End Function

    ' ---------------------------------------------------------
    ' إرسال صورة (الاعتماد على الرفع المؤقت وإنشاء رابط مباشر)
    ' ---------------------------------------------------------
    Sub SINTWATSAPP_PDF_CLINT(txtphon As String, filePath As String, msg As String)
        ' [تعديل جديد]: التحقق من الرقم أولاً قبل بدء أي عملية رفع للملفات
        If Not cackid_phone(txtphon, True) Then Exit Sub

        If Not IO.File.Exists(filePath) Then
            MsgBox("الصورة غير موجودة: " & filePath, MsgBoxStyle.Exclamation, "تنبيه")
            Exit Sub
        End If

        Try
            ' 1. رفع الصورة وجلب رابطها المباشر
            Dim webUrl As String = UploadFileToWebLink(filePath)
            If String.IsNullOrEmpty(webUrl) Then
                MsgBox("فشل رفع الصورة للإنترنت لتكوين الرابط، يرجى التأكد من اتصالك.", MsgBoxStyle.Critical, "خطأ بالاتصال")
                Exit Sub
            End If

            Dim chatId As String = CleanPhone(txtphon)
            Dim cleanMsg As String = CleanJsonText(msg)

            ' 2. إرسال الرابط القصير (لن يسبب خطأ 413 أبداً)
            Dim jsonBody As String = "{" &
                """chatId"":""" & chatId & """," &
                """url"":""" & webUrl & """," &
                """caption"":""" & cleanMsg & """" &
                "}"
            BASE_URL = "https://wa.rhalla.online/api/sessions/" & session_id
            Dim req As HttpWebRequest = CreateRequest(BASE_URL & "/messages/send-image", "POST")
            Dim postBytes As Byte() = New System.Text.UTF8Encoding().GetBytes(jsonBody)
            req.ContentLength = postBytes.Length

            Using stream As Stream = req.GetRequestStream()
                stream.Write(postBytes, 0, postBytes.Length)
            End Using

            Using response As WebResponse = req.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Console.WriteLine("رد إرسال الصورة: " & reader.ReadToEnd())
                End Using
            End Using

        Catch webEx As WebException
            HandleWebException(webEx)
        Catch ex As Exception
            MsgBox("خطأ عام: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' ---------------------------------------------------------
    ' إرسال مستند (PDF أو إكسيل)
    ' ---------------------------------------------------------
    Sub SINTWATSAPP_document(txtphon As String, filePath As String, fileName As String, caption As String)
        ' [تعديل جديد]: التحقق من الرقم أولاً قبل إرهاق السيرفر برفع المستند
        If Not cackid_phone(txtphon, True) Then Exit Sub

        If Not IO.File.Exists(filePath) Then
            MsgBox("المستند غير موجود: " & filePath, MsgBoxStyle.Exclamation, "تنبيه")
            Exit Sub
        End If

        Try
            ' 1. رفع المستند وجلب رابطه المباشر
            Dim webUrl As String = UploadFileToWebLink(filePath)
            If String.IsNullOrEmpty(webUrl) Then
                MsgBox("فشل رفع المستند للإنترنت لتكوين الرابط، يرجى التأكد من اتصالك.", MsgBoxStyle.Critical, "خطأ بالاتصال")
                Exit Sub
            End If

            Dim chatId As String = CleanPhone(txtphon)
            Dim cleanFileName As String = CleanJsonText(fileName)

            ' 2. إرسال الرابط القصير للسيرفر
            Dim jsonBody As String = "{" &
                """chatId"":""" & chatId & """," &
                """url"":""" & webUrl & """," &
                """filename"":""" & cleanFileName & """" &
                "}"
            BASE_URL = "https://wa.rhalla.online/api/sessions/" & session_id
            Dim req As HttpWebRequest = CreateRequest(BASE_URL & "/messages/send-document", "POST")
            Dim postBytes As Byte() = New System.Text.UTF8Encoding().GetBytes(jsonBody)
            req.ContentLength = postBytes.Length

            Using stream As Stream = req.GetRequestStream()
                stream.Write(postBytes, 0, postBytes.Length)
            End Using

            Using response As WebResponse = req.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    Console.WriteLine("رد إرسال المستند: " & reader.ReadToEnd())
                End Using
            End Using

        Catch webEx As WebException
            HandleWebException(webEx)
        Catch ex As Exception
            MsgBox("خطأ عام: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' ---------------------------------------------------------
    ' إرسال رسالة نصية (متوافقة تماماً مع التوثيق المرجعي وعاملة بنجاح)
    ' ---------------------------------------------------------
    Sub WATSAPPMsAG(txtphon As String, textmesg As String, IsOTp As Boolean)
        If Not cackid_phone(txtphon, IsOTp) Then Exit Sub
        Try
            Dim chatId As String = CleanPhone(txtphon)
            Dim cleanMsg As String = CleanJsonText(textmesg)

            ' إصلاح مشكلة علامات الاقتباس الثلاثية الزائدة حول حقل text ليكون حقل JSON سليم
            Dim jsonBody As String = "{""chatId"":""" & chatId & """,""text"":""" & cleanMsg & """}"
            BASE_URL = "https://wa.rhalla.online/api/sessions/" & session_id
            Dim req As HttpWebRequest = CreateRequest(BASE_URL & "/messages/send-text", "POST")
            Dim postBytes As Byte() = New System.Text.UTF8Encoding().GetBytes(jsonBody)
            req.ContentLength = postBytes.Length

            Using stream As Stream = req.GetRequestStream()
                stream.Write(postBytes, 0, postBytes.Length)
            End Using

            Using response As WebResponse = req.GetResponse()
                Using reader As New StreamReader(response.GetResponseStream())
                    ' نجاح العملية تلقائياً
                End Using
            End Using

        Catch webEx As WebException
            HandleWebException(webEx)
        Catch ex As Exception
            MsgBox("خطأ عام: " & ex.Message, MsgBoxStyle.Critical, "حدث خطأ غير متوقع")
        End Try
    End Sub

    ' ---------------------------------------------------------
    ' دالة مركزية لالتقاط وتحليل أخطاء الويب بدقة جداً
    ' ---------------------------------------------------------
    Private Sub HandleWebException(webEx As WebException)
        If webEx.Response IsNot Nothing Then
            Using reader As New StreamReader(webEx.Response.GetResponseStream())
                Dim serverResponse As String = reader.ReadToEnd()
                MsgBox("رد السيرفر الفعلي: " & serverResponse, MsgBoxStyle.Critical, "تنبيه من السيرفر")
            End Using
        Else
            MsgBox("تعذر الاتصال بالسيرفر، يرجى مراجعة شبكة الإنترنت وحالة السيرفر.", MsgBoxStyle.Critical, "خطأ شبكة")
        End If
    End Sub

    ' ---------------------------------------------------------
    ' تحويل الاستدعاءات القديمة تلقائياً للنظام الجديد
    ' ---------------------------------------------------------
    Sub WATSAPPMsAG_TO(txtphon As String, textmesg As String, IsOTp As Boolean)
        WATSAPPMsAG(txtphon, textmesg, IsOTp)
    End Sub

    Sub WATSAPPMsAGnNew(txtphon As String, textmesg As String, IsOTp As Boolean)
        WATSAPPMsAG(txtphon, textmesg, IsOTp)
    End Sub

#End Region


#Region "جلب القروبات الخاصة بالغرف علي الوتساب"
    Public Function get_gruop_id(branchid As Integer, Optional G1OrG2 As Int16 = 1) As String

        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@branchid", SqlDbType.Int) With {.Value = branchid}
        prm(1) = New SqlParameter("@G1OrG2", SqlDbType.Int) With {.Value = G1OrG2}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO_alter("get_gruop_id", prm)
        Return dt.Rows(0)("IDGroup")
    End Function

    Public Function get_Agentgruop_id(branchid As Integer) As String

        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@branchid", SqlDbType.Int) With {.Value = branchid}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("get_Agentgruop_id", prm)
        Return dt.Rows(0)("IDGroup")
    End Function
#End Region
    ''CoBranch_BranchType 
    Public Function CoBranch_BranchType(ID As Integer) As Integer
        ' تعريف المتغيرات
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}

        ' إنشاء جدول بيانات جديد
        Dim dt As New DataTable
        dt.Clear()

        ' استدعاء الدالة RUN_QUARY_PRO لجلب البيانات
        dt = RUN_QUARY_PRO("CoBranch_BranchType", prm)

        ' التأكد من أن الجدول يحتوي على صفوف، ثم التحقق من القيمة
        If dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0)("BranchType")) Then
            ' إرجاع القيمة في العمود BranchType
            Return Convert.ToInt32(dt.Rows(0)("BranchType"))
        Else
            ' في حالة عدم وجود قيمة، إرجاع 0 أو أي قيمة افتراضية مناسبة
            Return 0
        End If
    End Function

    Public Function ASSOCIATIONTB_phone(ID As Integer, type As Integer) As String

        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        prm(1) = New SqlParameter("@tpey", SqlDbType.Int) With {.Value = type}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ASSOCIATIONTB_phone", prm)
        Return dt.Rows(0)("phone")
    End Function
    Public Function ASSOCIATIONTB_phone_ALL(ID As Integer, type As Integer) As DataTable

        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        prm(1) = New SqlParameter("@tpey", SqlDbType.Int) With {.Value = type}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ASSOCIATIONTB_phone", prm)
        Return dt
    End Function

    ''امكانية جلب رصيد المشترك في الجميعة او جميع المشتركين 
    Public Function ASSOCIATIONID_send_Frowtwsap_NETtotal_OR_ALL(ID As Integer, ASSOCIATIONID As Integer) As DataTable


        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
        prm(1) = New SqlParameter("ASSOCIATIONID", SqlDbType.Int) With {.Value = ASSOCIATIONID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("ASSOCIATIONID_send_Frowtwsap_NETtotal", prm)
        Return dt
    End Function







#End Region





#Region "كود التحديث التلقائي "
    Public Sub UpdateApp()
        Try
            Dim str As String = Application.StartupPath
            If Directory.Exists(str) Then
                File.Delete(str & "\APPFORUPDat1.exe")

            End If
            Dim txt, nowV, oldV, txt1 As String

            Dim URL As String = "https://www.dropbox.com/scl/fi/iqud27qa587f4oaesvyxu/update_app.txt?rlkey=uf9jwvhkuujocfwecai120spt&dl=1"
            Dim Request As HttpWebRequest = WebRequest.Create(URL)
            Dim Response As HttpWebResponse = Request.GetResponse()
            Using Reader As New StreamReader(Response.GetResponseStream())

                Dim OnlineVersion As String = Reader.ReadToEnd()
                txt = Application.ProductVersion.ToString
                txt1 = OnlineVersion

                nowV = Convert.ToString(txt1)
                oldV = Convert.ToString(txt)


                If oldV.Trim <> nowV.Trim Then
                    UpateAppliction = 0
                    Dim DownloadURL As String = "https://www.dropbox.com/scl/fi/zuhezfh8whrqy6oqjn579/ExchangeSystem.exe?rlkey=cxz91u1cx54il1gp34vd2l4il&dl=1"
                    Dim DownloadRequest As HttpWebRequest = WebRequest.Create(DownloadURL)
                    Dim DownloadResponse As HttpWebResponse = DownloadRequest.GetResponse()

                    Using DownloadStream As Stream = DownloadResponse.GetResponseStream()
                        Using FileStream As New FileStream("updated_app1.exe", FileMode.Create, FileAccess.Write, FileShare.None)
                            Dim Buffer As Byte() = New Byte(4096) {}
                            Dim BytesRead As Integer
                            Do
                                BytesRead = DownloadStream.Read(Buffer, 0, Buffer.Length)
                                FileStream.Write(Buffer, 0, BytesRead)
                            Loop While BytesRead > 0
                        End Using
                    End Using

                    My.Computer.FileSystem.RenameFile("ExchangeSystem.exe", "APPFORUPDat1.exe")

                    My.Computer.FileSystem.RenameFile("updated_app1.exe", "ExchangeSystem.exe")

                    Process.Start("ExchangeSystem.exe") ' تشغيل البرنامج الجديد

                    FRMMAIN.SplashScreenManager2.CloseWaitForm()
                    FRMMAIN.Close()
                    Application.Exit()


                    MessageBox.Show("تمت عمليه التحديث بنجاح")


                Else
                    UpateAppliction = 1
                    'SScreen1.Timer1.Enabled = True

                    'SScreen1.Timer1.Start()
                    MessageBox.Show("عذرا لايوجد تحديث في الوقت الحالي ")
                End If


            End Using
        Catch ex As Exception
            FRMMAIN.SplashScreenManager2.CloseWaitForm()
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Public Function ChickUpdate(Vrg As Integer) As Integer
        ' Returns 0 = an update is available, 1 = no update / could not check.
        '
        ' AUTO-UPDATE IS OFF BY DEFAULT (AUTO_UPDATE=ON in RhallaConfig.ini to enable it).
        ' UpdateApp() DOWNLOADS the vendor's production ExchangeSystem.exe from Dropbox, renames the local build
        ' to APPFORUPDat1.exe and replaces ExchangeSystem.exe with it. That downloaded binary is the vendor's
        ' build: it has no engine switch and connects straight to the PRODUCTION server (148.251.245.41).
        ' Left enabled it silently overwrites this build on every launch, so the app appears to "revert" — the
        ' configured database (local SQL Server, or MySQL) is never contacted and the activation lookup runs
        ' against the online database instead, which is what produced "هذا الجهاز غير مرخص".
        ' Any machine running a locally built binary, or pointing at a local database, must keep this OFF.
        If Not MD_SECRETS.AutoUpdateEnabled Then Return 1
        '
        ' The whole body is wrapped so an UNREACHABLE update host cannot abort startup. FRMMAIN_Load calls this
        ' BEFORE LoadFormEvent(); when the machine is offline (or dropbox.com is blocked/unresolvable) the
        ' WebRequest threw, the exception escaped to FRMMAIN_Load's Catch, and LoadFormEvent() never ran — the
        ' main form silently never initialised, showing only "the remote name could not be resolved". Failing to
        ' CHECK for an update must never be treated as "an update is pending": return 1 so startup continues.
        Try
            Dim txt, nowV, oldV, txt1 As String

            Dim URL As String = "https://www.dropbox.com/scl/fi/iqud27qa587f4oaesvyxu/update_app.txt?rlkey=uf9jwvhkuujocfwecai120spt&dl=1"
            Dim Request As HttpWebRequest = WebRequest.Create(URL)
            Request.Timeout = 10000                     ' don't hang the splash screen on a dead network
            Dim Response As HttpWebResponse = Request.GetResponse()
            Using Reader As New StreamReader(Response.GetResponseStream())

                Dim OnlineVersion As String = Reader.ReadToEnd()
                txt = Application.ProductVersion.ToString
                txt1 = OnlineVersion

                nowV = Convert.ToString(txt1)
                oldV = Convert.ToString(txt)


                If oldV.Trim <> nowV.Trim Then
                    Vrg = 0
                    Return Vrg
                Else
                    Vrg = 1
                    Return Vrg
                End If

            End Using
        Catch ex As Exception
            MD_MYSQL.LogMyError("ChickUpdate (update check skipped - update host unreachable)", Nothing, ex)
            Return 1
        End Try

    End Function
#End Region

    'التحقق من الحساب مسموح بالمدين
    Public Function CHECKISCREDIT(ID As ULong) As Boolean
        CHECKISCREDIT = False
        Dim str = ("select CandDebit from CustomersTb where AccID='" & ID & "'")
        Dim adp = New SqlClient.SqlDataAdapter(str, SQLCON)
        Dim ds = New DataSet
        adp.Fill(ds)
        Dim dt As DataTable
        dt = ds.Tables(0)
        If dt.Rows.Count <> 0 Then
            Dim i = dt.Rows.Count - 1
            CHECKISCREDIT = Val(dt.Rows(i).Item(0))
        End If
    End Function

    Public Sub LoadCompanySettings_lode()
        Try
            Dim dt As DataTable = RUN_QUARY_PRO_ONLY("SELECTALLTB_PROFILE_COMPANY")

            If dt.Rows.Count > 0 Then
                Dim row As DataRow = dt.Rows(0)

                ' حفظ القيم النصية في الإعدادات
                My.Settings.ARName = row("ARName").ToString()
                My.Settings.Mobile1 = row("Mobile1").ToString()
                My.Settings.Website = row("WebSite").ToString()
                My.Settings.EMail = row("EMail").ToString()
                My.Settings.Combny_name = row("Combny_name_Watsapp").ToString
                My.Settings.Combny_name_2 = row("Combny_name_2_Watsapp").ToString
                Module1.session_id = row("session_id").ToString
                Module1.apiKey = row("apiKey").ToString
                Module1.apiUrl = $"https://wa.rhalla.online/api/sessions/{SESSION_ID}/messages/send-text"
                ' حفظ صورة الشركة كـ Base64 إذا كانت موجودة
                If row.Table.Columns.Contains("IMG") AndAlso Not IsDBNull(row("IMG")) Then
                    Dim companyImageBytes As Byte() = TryCast(row("IMG"), Byte())
                    If companyImageBytes IsNot Nothing AndAlso companyImageBytes.Length > 0 Then
                        My.Settings.Company_Image = Convert.ToBase64String(companyImageBytes)
                    Else
                        My.Settings.Company_Image = String.Empty
                    End If
                Else
                    My.Settings.Company_Image = String.Empty
                End If

                ' حفظ الإعدادات
                My.Settings.Save()
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل إعدادات الشركة: " & ex.Message)
        End Try
    End Sub


End Module
