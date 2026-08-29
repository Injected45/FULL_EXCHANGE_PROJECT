Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Public Class FrmAddnavcation
    Public Sub frmNotifications_maxID()
        Try


            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.BigInt) With {.Direction = ParameterDirection.Output}
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("frmNotifications_maxID", prm)
            navcationID.EditValue = prm(0).Value
        Catch ex As Exception
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            ' pass the UserLookAndFeel as a Parameter in the show method
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالـــــــــــة خطــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub BNew()


        navcationID.Text = String.Empty
        ScreenName.Text = String.Empty
        ekhtesar1.Text = String.Empty
        ekhtesar2.EditValue = String.Empty

        frmNotifications_maxID()
        MyBase.BNew()
    End Sub
    Public Overrides Sub Save()

        SetData()
        MyBase.Save()
    End Sub

    Public Sub AddNotification_insert()
        Try


            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@NotificationID", SqlDbType.Int) With {.Value = navcationID.Text}
            prm(1) = New SqlParameter("@NotificationNAME", SqlDbType.NVarChar, -1) With {.Value = ScreenName.Text}
            prm(2) = New SqlParameter("@ButtonName", SqlDbType.NVarChar, 50) With {.Value = ekhtesar1.Text}
            prm(3) = New SqlParameter("@BUTTONNAME2", SqlDbType.NVarChar, 50) With {.Value = ekhtesar2.Text}
            prm(4) = New SqlParameter("@msg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@msages", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("AddNotification", prm)

            If prm(4).Value = 0 Then
                Dim lookAndFeelError As New UserLookAndFeel(Me)
                'lookAndFeelError.SkinName = "MilkShake"
                lookAndFeelError.Style = LookAndFeelStyle.Skin
                lookAndFeelError.UseDefaultLookAndFeel = False
                lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
                ' force Message Boxes to use the "MyCustomSkin"
                XtraMessageBox.AllowCustomLookAndFeel = True

                ' pass the UserLookAndFeel as a Parameter in the show method
                XtraMessageBox.Show(lookAndFeelError, prm(5).Value, "رسالـــــــــــة خطــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                BtnNew.PerformClick()
            End If

        Catch ex As Exception
            Dim lookAndFeelError As New UserLookAndFeel(Me)
            'lookAndFeelError.SkinName = "MilkShake"
            lookAndFeelError.Style = LookAndFeelStyle.Skin
            lookAndFeelError.UseDefaultLookAndFeel = False
            lookAndFeelError.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.DevExpressDark)
            ' force Message Boxes to use the "MyCustomSkin"
            XtraMessageBox.AllowCustomLookAndFeel = True

            ' pass the UserLookAndFeel as a Parameter in the show method
            XtraMessageBox.Show(lookAndFeelError, ex.Message, "رسالـــــــــــة خطــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub SetData()


        If ScreenName.Text = String.Empty Then
            ScreenName.ErrorText = "هذا الحقل المطلوب"
            Return
        End If

        If ekhtesar1.Text = String.Empty Then
            ekhtesar1.ErrorText = "هذا الحقل المطلوب"
            Return
        End If

        If ekhtesar2.Text = String.Empty Then
            ekhtesar2.ErrorText = "هذا الحقل المطلوب"
            Return
        End If

        AddNotification_insert()

        MyBase.SetData()
    End Sub

    Private Sub FrmAddnavcation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnNew.PerformClick()
    End Sub
End Class