Imports System.Data.SqlClient
Imports DevExpress.CodeParser
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class ExValSahreByHand
    Public IsAgintToAgint As Boolean
    Sub NEWRECORD()
        CodeID.Text = ""
        ExVal.EditValue = -1
        ExValShare.EditValue = 0.000
        ExValShare1.EditValue = 0.000
    End Sub
    Public Sub LoadExVal()
        Dim DT1 As New DataTable
        DT1.Clear()
        DT1 = RUN_QUARY_TXT("Select Code,ExVal from InternalEx where Code='" & FRMINTERNALTRANSFER.CodeID.Text & "'")
        If DT1.Rows.Count > 0 Then
            CodeID.Text = DT1.Rows(0)("Code").ToString
            ExVal.EditValue = DT1.Rows(0)("ExVal")
        End If
    End Sub

    Public Sub LoadRedrecitonExVal()
        Dim DT1 As New DataTable
        DT1.Clear()
        DT1 = RUN_QUARY_TXT("Select Code,ExVal from InternalEx where Code='" & FRMINTERNALTRANSFER.CodeID.Text & "'")
        If DT1.Rows.Count > 0 Then
            CodeID.Text = DT1.Rows(0)("Code").ToString
            ExVal.EditValue = DT1.Rows(0)("ExVal")
        End If
    End Sub

    Private Sub ExValSahreByHand_Load(sender As Object, e As EventArgs) Handles Me.Load
        IScteck = 0
        InsertDate.EditValue = Date.Now
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If IsAgintToAgint = False Then
            If ExValShare.EditValue > ExVal.EditValue Then
                ErrorMessage(Me, "رسالة تنبيه", "عمولة الوكيل لايجب أن تكون أكبر من إجمالي العمولة")
                Exit Sub
                ExValShare1.EditValue = 0.000
            End If
        End If
        If IsAgintToAgint = True Then
            If ExValShare.EditValue + ExValShare1.EditValue > ExVal.EditValue Then
                ErrorMessage(Me, "رسالة تنبيه", "مجموع عمولة الوكلاء لايجب أن يكون أكبر من إجمالي العمولة")
                Exit Sub
            End If

        End If

        FRMINTERNALTRANSFER.HandallExVal = ExValShare.EditValue
        FRMINTERNALTRANSFER.HandallExVal2 = ExValShare1.EditValue
        FRMAGENTREDIRECTION.HandallExVal = ExValShare.EditValue
        FRMAGENTREDIRECTION.HandallExVal2 = ExValShare1.EditValue
        IScteck = 1
        Me.Close()
    End Sub

    Private Sub SimpleButton21_Click(sender As Object, e As EventArgs) Handles SimpleButton21.Click
        Dim customIcon As New Icon(Application.StartupPath & "\error.ico")
        XtraMessageBox.Icons(MessageBoxIcon.Error) = customIcon
        Dim lookAndFeelError As New UserLookAndFeel(Me)
        lookAndFeelError.Style = LookAndFeelStyle.Skin
        lookAndFeelError.UseDefaultLookAndFeel = False
        lookAndFeelError.SetSkinStyle(SkinStyle.Metropolis)
        XtraMessageBox.AllowCustomLookAndFeel = True
        Dim reuslt2 = XtraMessageBox.Show(lookAndFeelError, "في حال الموافقة سيتم تقسيم العمولة بشكل تلقائي", "هل أنت متأكد من إلغاء العملية؟", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reuslt2 = DialogResult.No Then
            Exit Sub
        End If
        FRMINTERNALTRANSFER.ISHandallEX = 0
        FRMAGENTREDIRECTION.ISHandallEX = 0
        Me.Close()
    End Sub
    Public IScteck As Integer = 0






End Class