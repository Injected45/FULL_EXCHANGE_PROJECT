Imports System.Data.SqlClient
Imports DevExpress.Utils.Drawing

Public Class frm_addmain

    Public Sub NEWrECPR()
        New_Controlrs(Me)
        GetMAxID()

    End Sub

    Public Sub GetMAxID()
        Try
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("get_max_mainid", prm)
            ScreenID.Text = prm(0).Value
        Catch ex As Exception

        End Try

    End Sub

    Public Overrides Sub BNew()
        NEWrECPR()
        MyBase.BNew()
    End Sub

    Public Sub FrmMainTb_insrt()
        Try
            SplashScreenManager1.ShowWaitForm()

            Dim prm(3) As SqlParameter
            prm(0) = New SqlParameter("@main_name", SqlDbType.NVarChar, 50) With {.Value = ScreenName.Text}
            prm(1) = New SqlParameter("@short_name", SqlDbType.NVarChar, 250) With {.Value = EnglishName.Text}
            prm(2) = New SqlParameter("@SMG", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(3) = New SqlParameter("@MASG", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("FrmMainTb_insrt", prm)
            If prm(2).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                ErrorMessage(Me, "خطأ", prm(3).Value)
            Else
                SplashScreenManager1.CloseWaitForm()
                FrmSavedSuccessfully.ShowDialog()
                BtnNew.PerformClick()
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage(Me, "ex _FrmMainTb_insrt  ", ex.Message)
        End Try
    End Sub

    Public Overrides Sub SetData()
        If ScreenName.Text = String.Empty Then
            ScreenName.ErrorText = "هذه الحقل مطلوب"
            Exit Sub
        End If

        If EnglishName.Text = String.Empty Then
            EnglishName.ErrorText = "هذه الحقل مطلوب"
            Exit Sub
        End If


        If ScreenID.Text = String.Empty Then
            ScreenID.ErrorText = "هذه الحقل مطلوب"
            Exit Sub
        End If

        FrmMainTb_insrt()
    End Sub


    Public Overrides Sub Save()
        SetData()
    End Sub

    Private Sub frm_addmain_Load(sender As Object, e As EventArgs) Handles Me.Load
        BtnNew.PerformClick()
    End Sub
End Class