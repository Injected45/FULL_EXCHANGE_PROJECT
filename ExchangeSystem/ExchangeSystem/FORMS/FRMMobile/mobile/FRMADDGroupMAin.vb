Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class FRMADDGroupMAin


    Public Sub newRecors()
        Try


            New_Controlrs(Me)
            GEtmaxID()
            LoadToControlar(Main_ID, "FrmMainTb_getdDvg", "MainName", "MainID", Nothing)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub GEtmaxID()
        Try

            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            RUN_EXUTE_PRO("Group_main_getMAxID", prm)
            ScreenID.Text = prm(0).Value
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Overrides Sub BNew()
        newRecors()
        MyBase.BNew()
    End Sub

    Private Sub FRMADDGroupMAin_Load(sender As Object, e As EventArgs) Handles Me.Load
        BtnNew.PerformClick()
    End Sub

    Public Sub Group_main_INSERRT()

        Try
            SplashScreenManager1.ShowWaitForm()
            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@GroupNAme", SqlDbType.NVarChar, -1) With {.Value = GroupNAme.Text}
            prm(1) = New SqlParameter("@group_ena", SqlDbType.NVarChar, -1) With {.Value = group_ena.Text}
            prm(2) = New SqlParameter("@Main_ID", SqlDbType.Int) With {.Value = Main_ID.EditValue}
            prm(3) = New SqlParameter("@ShortName", SqlDbType.NVarChar, -1) With {.Value = ShortName.Text}
            prm(4) = New SqlParameter("@smg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(5) = New SqlParameter("@masg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}

            RUN_EXUTE_PRO("Group_main_INSERRT", prm)
            If prm(4).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                XtraMessageBox.Show(prm(5).Value, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                SplashScreenManager1.CloseWaitForm()
                BtnNew.PerformClick()
                FrmSavedSuccessfully.ShowDialog()
            End If

        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            XtraMessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Overrides Sub SetData()
        If ScreenID.Text = String.Empty Then
            ScreenID.ErrorText = "هذه الحقل مطلوب"
            Exit Sub
        End If

        If GroupNAme.Text = String.Empty Then
            GroupNAme.ErrorText = "هذه الحقل مطلوب"
            Exit Sub
        End If


        If group_ena.Text = String.Empty Then
            group_ena.ErrorText = "هذه الحقل مطلوب"

            Exit Sub
        End If

        If Main_ID.Text = String.Empty Then
            Main_ID.ErrorText = "هذه الحقل مطلوب"

            Exit Sub
        End If
        If ShortName.Text = String.Empty Then
            ShortName.ErrorText = "هذه الحقل مطلوب"

            Exit Sub
        End If

        Group_main_INSERRT()
    End Sub
    Public Overrides Sub Save()
        SetData()
    End Sub
End Class