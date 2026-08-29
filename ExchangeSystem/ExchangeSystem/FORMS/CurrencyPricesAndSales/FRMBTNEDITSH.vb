Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class FRMBTNEDITSH

    Public lookAndFeelError As New UserLookAndFeel(Me)
    Private Sub ID_CODE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ID_CODE.KeyPress
        e.Handled = True
    End Sub

    Private Sub ACCFROM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CurrencyIDFrom.KeyPress
        e.Handled = True
    End Sub

    Private Sub ACCTO_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CurrencyIDTo.KeyPress
        e.Handled = True
    End Sub

    Private Sub TextEdit2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles BankName.KeyPress
        e.Handled = True
    End Sub

    Public Sub NewREcorids()
        ID_CODE.Text = String.Empty
        Typesf.Enabled = False
        Typesf.SelectedIndex = -1
        BankName.Text = String.Empty
        CurrencyIDFrom.Text = String.Empty
        CurrencyIDTo.Text = String.Empty
        BuyPrice.EditValue = 0.00
        SalePrice.EditValue = 0.00
    End Sub


    Public Sub CurrencyPricCatogre(ID As ULong, Type As Integer, TypeID As Integer)
        Try


            'NewREcorids()
            Dim prm(2) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID}
            prm(1) = New SqlParameter("@Tipe", SqlDbType.Int) With {.Value = Type}
            prm(2) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 1}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("CurrencyPricCatogre", prm)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("Typesf") = 0 Then
                    LayoutControlItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                Else
                    LayoutControlItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                End If
                ID_CODE.Text = dt.Rows(0)("ID")
                BankName.Text = dt.Rows(0)("BankName")
                CurrencyIDFrom.Text = dt.Rows(0)("CurrencyIDFrom")
                CurrencyIDTo.Text = dt.Rows(0)("CurrencyIDTo")
                Typesf.SelectedIndex = dt.Rows(0)("Typesf")
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
        CurrencyPricCatogreUpdate()


    End Sub

    Public Sub CurrencyPricCatogreUpdate()
        Try
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(SkinStyle.MetropolisDark)

            If XtraMessageBox.Show(lookAndFeelError, "", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then

            End If


            Dim prm(7) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID_CODE.EditValue}
            prm(1) = New SqlParameter("@BuyPrice", SqlDbType.Decimal, 12, 3) With {.Value = BuyPrice.EditValue}
            prm(2) = New SqlParameter("@SalePrice", SqlDbType.Decimal, 12, 3) With {.Value = SalePrice.EditValue}
            prm(3) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
            prm(4) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = Typesf.SelectedIndex}
            prm(5) = New SqlParameter("@TYpes", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(6) = New SqlParameter("@crUNSEFROM", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(7) = New SqlParameter("@AccountType", SqlDbType.TinyInt) With {.Value = 1}
            RUN_EXUTE_PRO("CurrencyPricCatogreUpdate", prm)

            FRMCURRENCYPRICEDTTELSS.frm.LOADATA(prm(6).Value, prm(5).Value)

            XtraMessageBox.Show(lookAndFeelError, "تمت عملية التعديل نجاح", "رسالة تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            ErrorMessage(Me, "رسالة خطأ", ex.Message)
        End Try
    End Sub




    Private Sub FRMBTNEDITSH_Closed(sender As Object, e As EventArgs) Handles Me.Closed


    End Sub
End Class