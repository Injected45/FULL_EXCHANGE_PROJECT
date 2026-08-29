Imports System.Data.SqlClient
Imports DevExpress
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class Currencyonthebank2
    Public FUNCtionC As Boolean
    Public lookAndFeelError As New UserLookAndFeel(Me)
    Private Sub Currencyonthebank_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lodePreportes()
        BtnNew.PerformClick()
    End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(27, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

#Region "جديد و اضافة "
    Public Overrides Sub BNew()
        Try


            DateNOW.EditValue = Date.Now
            CurrencyFrom.Properties.TextEditStyle = XtraEditors.Controls.TextEditStyles.Standard
            CurrencyFrom.Text = "دينار الليبي"
            CurrencyFrom.Tag = 1
            BuyPrice.Text = 0.00
            SalePrice.Text = 0.00
            RetBuyPrice.Text = 0
            retSalePrice.Text = 0
            CurrencyTo.Properties.TextEditStyle = XtraEditors.Controls.TextEditStyles.Standard
            CurrencyTo.Text = "الدولار الامريكي"
            CurrencyTo.Tag = 2
            BtnSave.Caption = "تاكيد حفظ الاعدادت"
            BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Me.Height = 555
            Me.Width = 821
            Panel2.Height = 41
            GET_MAXID_Currency_settingForBancksRet()
            Currency_settingForBancksRet_getstting(1)
            'BuyPrice.EditValue = My.Settings.BuyPrice
            'SalePrice.EditValue = My.Settings.SalePrice
            'RetBuyPrice.EditValue = My.Settings.RetBuyPrice
            'retSalePrice.EditValue = My.Settings.retSalePrice
            Currency_settingForBancksRet_getUESER()
        Catch ex As Exception

            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            MD_MYSQL.LogAppError("caught in form", Me, ex)   ' also record it in mysql_errors.log
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالة خطأ في البرنامج الرجاء التواصل مع الدعم الفني", MessageBoxButtons.OK, MessageBoxIcon.Warning)


        End Try
        MyBase.BNew()
    End Sub
    Public Overrides Sub SetData()
        If BuyPrice.EditValue = 0.00 Then
            BuyPrice.ErrorText = "يجب ان تكون القيمة اكبر من صفر "
            Return
        End If

        If SalePrice.EditValue = 0.00 Then
            SalePrice.ErrorText = "يجب ان تكون القيمة اكبر من صفر "
            Return
        End If


        If RetBuyPrice.EditValue = 0.00 Then
            BuyPrice.ErrorText = "الرجاء التاكد من القيمة "
            Return
        End If

        If retSalePrice.EditValue = 0.00 Then
            SalePrice.ErrorText = "الرجاء التاكد من القيمة "
            Return
        End If


        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
        ' force Message Boxes to use the "MyCustomSkin"
        XtraMessageBox.AllowCustomLookAndFeel = True
        If XtraMessageBox.Show(lookAndFeelError, " تنبية انتا علي وشك تعديل جميع اسعار صرف العملات في جميع المصارف بنسبة الجديد" & vbNewLine &
                            " للاستمرار وتعديل الاسعار الرجاء الضغط علي موافق", "رسالة تنبية", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.No Then
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            XtraMessageBox.Show(lookAndFeelError, " تمت عملية الالغاء بنجاح", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If


        Currency_settingForBancksRet_insert()


        MyBase.SetData()
    End Sub

    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub

#End Region
    Private Sub CurrencyFrom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CurrencyFrom.KeyPress
        e.Handled = True
    End Sub
    Private Sub CurrencyTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CurrencyTo.KeyPress
        e.Handled = True
    End Sub
    Public Function Currencyonthebank_forselecPr(Type As Int16, SalePricevalu As Decimal) As Boolean

        Try
            Dim dt As New DataTable
            dt.Clear()

            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@CurrencyIDFrom", SqlDbType.Int) With {.Value = CurrencyFrom.Tag}
            prm(1) = New SqlParameter("@CurrencyIDTo", SqlDbType.Int) With {.Value = CurrencyTo.Tag}
            prm(2) = New SqlParameter("@SalePricevalue", SqlDbType.Decimal, 13, 3) With {.Value = SalePricevalu}
            prm(3) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = Type}
            prm(4) = New SqlParameter("@ReturnSalePricevalue", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@MSGsta", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            dt = RUN_QUARY_PRO("Currencyonthebank_forselecPr", prm)

            If prm(5).Value = 1 Then
                Timer1.Stop()
                lblmasg.Text = "تمت تعديل القيمة بنجاح"
                lblmasg.ForeColor = Color.Orange
                If Type = 0 Then
                    RetBuyPrice.Text = prm(4).Value

                ElseIf Type = 1 Then
                    retSalePrice.Text = prm(4).Value
                End If
                Return prm(5).Value
            Else

                lblmasg.Text = prm(4).Value
                lblmasg.ForeColor = Color.White
                Timer1.Start()
                Return prm(5).Value
            End If

        Catch ex As Exception
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            MD_MYSQL.LogAppError("caught in form", Me, ex)   ' also record it in mysql_errors.log
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالة خطأ في البرنامج الرجاء التواصل مع الدعم الفني", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Function
    Private Sub TextEdit5_EditValueChanged(sender As Object, e As EventArgs) Handles BuyPrice.EditValueChanged
        If BuyPrice.EditValue > 0 Then

            FUNCtionC = Currencyonthebank_forselecPr(0, BuyPrice.EditValue)
            If FUNCtionC = False Then
                RetBuyPrice.EditValue = 0.00
                BuyPrice.ErrorText = "الرجاء التاكد من القيمة المدخلة"
            End If
        End If

    End Sub
    Private Sub TextEdit6_EditValueChanged(sender As Object, e As EventArgs) Handles SalePrice.EditValueChanged
        If SalePrice.EditValue > 0 Then
            FUNCtionC = Currencyonthebank_forselecPr(1, SalePrice.EditValue)
            If FUNCtionC = False Then
                retSalePrice.EditValue = 0.00
                SalePrice.ErrorText = "الرجاء التاكد من القيمة المدخلة"
            End If
        End If
    End Sub
    Private Sub RetBuyPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles RetBuyPrice.KeyPress
        e.Handled = True
    End Sub
    Private Sub retSalePrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles retSalePrice.KeyPress
        e.Handled = True
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs)


        lblmasg.Text = String.Empty
        Timer1.Stop()


    End Sub
    Public Sub GET_MAXID_Currency_settingForBancksRet()
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@BrnchID", SqlDbType.Int) With {.Value = BID}
        prm(1) = New SqlParameter("@ID", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("GET_MAXID_Currency_settingForBancksRet", prm)
        Code.Text = prm(1).Value
        dt.Dispose()

    End Sub
    Private Sub UESERinsert_KeyPress(sender As Object, e As KeyPressEventArgs) Handles UESERinsert.KeyPress
        e.Handled = True
    End Sub
    Private Sub UpdateCount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles UpdateCount.KeyPress
        e.Handled = True
    End Sub
    Private Sub UeserUpdate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles UeserUpdate.KeyPress
        e.Handled = True
    End Sub
    Private Sub UpdateDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles UpdateDate.KeyPress
        e.Handled = True
    End Sub
    Public Sub Currency_settingForBancksRet_insert()
        Try


            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.TinyInt) With {.Value = Code.EditValue}
            prm(1) = New SqlParameter("@CurrencyFrom", SqlDbType.Int) With {.Value = CurrencyFrom.Tag}
            prm(2) = New SqlParameter("@CurrencyTo", SqlDbType.Int) With {.Value = CurrencyTo.Tag}
            prm(3) = New SqlParameter("@BuyPrice", SqlDbType.Decimal, 13, 3) With {.Value = BuyPrice.EditValue}
            prm(4) = New SqlParameter("@SalePrice", SqlDbType.Decimal, 13, 3) With {.Value = SalePrice.EditValue}
            prm(5) = New SqlParameter("@RetBuyPrice", SqlDbType.Decimal, 13, 3) With {.Value = RetBuyPrice.EditValue}
            prm(6) = New SqlParameter("@retSalePrice", SqlDbType.Decimal, 13, 3) With {.Value = retSalePrice.EditValue}
            prm(7) = New SqlParameter("@Userinser", SqlDbType.Int) With {.Value = UserID}

            RUN_EXUTE_PRO("Currency_settingForBancksRet_insert", prm)
            'My.Settings.BuyPrice = BuyPrice.EditValue
            'My.Settings.SalePrice = SalePrice.EditValue
            'My.Settings.RetBuyPrice = RetBuyPrice.EditValue
            'My.Settings.retSalePrice = retSalePrice.EditValue

            My.Settings.Save()

            BtnNew.PerformClick()
        Catch ex As Exception
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            MD_MYSQL.LogAppError("caught in form", Me, ex)   ' also record it in mysql_errors.log
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالة خطأ في البرنامج الرجاء التواصل مع الدعم الفني", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Sub Currency_settingForBancksRet_getUESER()
        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code.EditValue}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("Currency_settingForBancksRet_getUESER", prm)
            If dt.Rows.Count > 0 Then
                ' These three go into .Text (a String), so a NULL column throws
                '   "Conversion from type 'DBNull' to type 'String' is not valid"
                ' and the whole screen fails with the generic support dialog.
                '
                ' UESERinsert really can be NULL: the proc does "SELECT b.UName INTO v_UESERinsert .. WHERE
                ' a.ID = p_ID", so it stays NULL whenever the record is missing or its Userinser matches no
                ' TB_Users row — and Currency_settingForBancksRet is currently EMPTY. The proc wraps the
                ' other two columns in IFNULL but NOT this one, and the T-SQL original does exactly the same
                ' (verified), so this is pre-existing app fragility, not something the migration introduced.
                ' Fixed here rather than in the proc so the result set stays identical to SQL Server's.
                UESERinsert.Text = NullSafeText(dt.Rows(0)("UESERinsert"))
                UpdateCount.EditValue = dt.Rows(0)("UESERCOUNT")   ' EditValue is Object; DBNull is fine
                UeserUpdate.Text = NullSafeText(dt.Rows(0)("UESEIRRUpdate"))
                UpdateDate.Text = NullSafeText(dt.Rows(0)("UDATEDAte"))

            End If
        Catch ex As Exception
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            MD_MYSQL.LogAppError("caught in form", Me, ex)   ' also record it in mysql_errors.log
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالة خطأ في البرنامج الرجاء التواصل مع الدعم الفني", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        FRMCurrencyMovements.ShowDialog()
    End Sub
End Class