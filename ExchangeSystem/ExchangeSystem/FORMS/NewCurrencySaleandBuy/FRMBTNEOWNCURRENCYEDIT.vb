Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports System.Data.SqlClient

Public Class FRMBTNEOWNCURRENCYEDIT
    Public lookAndFeelError As New UserLookAndFeel(Me)
    Public PrcType, AccountType, AgentID, BankID As Integer
    Private Sub ID_CODE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ID_CODE.KeyPress
        e.Handled = True
    End Sub
    Private Sub ACCFROM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CurrencyIDFrom.KeyPress
        e.Handled = True
    End Sub
    Private Sub ACCTO_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CurrencyIDTo.KeyPress
        e.Handled = True
    End Sub
    Public Sub NewRecords()
        ID_CODE.Text = String.Empty
        PriceType.Enabled = False
        PriceType.SelectedIndex = -1
        CurrencyIDFrom.Text = String.Empty
        CurrencyIDTo.Text = String.Empty
        BuyPrice.EditValue = 0.00
        SalePrice.EditValue = 0.00
    End Sub
    Public Sub CurrencyPriceCategory(ID As ULong, PriceType As Integer, TypeID As Integer)
        'Type متغير لمعرفة نوع التسعير
        Try
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
            prm(1) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = PriceType}
            prm(2) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 2}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CurrencyPric_LoadToUpdate", prm)
            If dt.Rows.Count > 0 Then
                ID_CODE.Text = dt.Rows(0)("ID")
                CurrencyIDFrom.Text = dt.Rows(0)("CurrencyIDFrom")
                CurrencyIDTo.Text = dt.Rows(0)("CurrencyIDTo")
                BuyPrice.EditValue = dt.Rows(0)("BuyPrice")
                SalePrice.EditValue = dt.Rows(0)("SalePrice")
                Me.ShowDialog()
            Else
                MessageBox.Show("عذرا لايوجد بيانات في الوقت الحالي", "رسالة تنبية", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If BuyPrice.EditValue <= 0 Then
            BuyPrice.ErrorText = "عذرا يجب ان تكون القيمة اكبر من صفر "
            MessageBox.Show("عذرا يجب ان تكون القيمة اكبر من صفر ", "رساالة خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If SalePrice.EditValue <= 0 Then
            SalePrice.ErrorText = "عذرا يجب ان تكون القيمة اكبر من صفر "
            MessageBox.Show("عذرا يجب ان تكون القيمة اكبر من صفر ", "رساالة خطا", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        'FRMNEWCURRENCYBUY.Purchaseprice.EditValue = Me.BuyPrice.EditValue
        CurrencyPriceCategoryUpdate()
        FRMNEWCURRENCYBUY.get_BracuNetBurnnc()
    End Sub
    Public Sub CurrencyPriceCategoryUpdate()
        Try
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)
            If XtraMessageBox.Show(lookAndFeelError, "هل تريد تعديل الأسعار بالفعل؟", "رسالة معلومات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Dim prm(7) As SqlParameter
                prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID_CODE.EditValue}
                prm(1) = New SqlParameter("@BuyPrice", SqlDbType.Decimal, 12, 3) With {.Value = BuyPrice.EditValue}
                prm(2) = New SqlParameter("@SalePrice", SqlDbType.Decimal, 12, 3) With {.Value = SalePrice.EditValue}
                prm(3) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
                prm(4) = New SqlParameter("@PriceType", SqlDbType.Int) With {.Value = PriceType.SelectedIndex}
                prm(5) = New SqlParameter("@TYpes", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                prm(6) = New SqlParameter("@crUNSEFROM", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                prm(7) = New SqlParameter("@AccountType", SqlDbType.TinyInt) With {.Value = 1}
                RUN_EXUTE_PRO("NewCurrencyPricUpdate", prm)
                XtraMessageBox.Show(lookAndFeelError, "تمت عملية التعديل نجاح", "رسالة تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information)
                FRMNEWCURRENCYETAILS.LOADATA(prm(6).Value, FRMNEWCURRENCYETAILS.AccountType.SelectedIndex, FRMNEWCURRENCYETAILS.BRID, FRMNEWCURRENCYETAILS.CountryID.EditValue, FRMNEWCURRENCYETAILS.BANID, PriceType.SelectedIndex)
            End If
            Me.Close()
        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub
End Class