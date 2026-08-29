Imports System.Data.SqlClient

Public Class Daily_transfer_preparer_schedule_DEttelse_GETUESER
    Public Overrides Sub BNew()
        New_Controlrs(Me)
        BtnPrint.Caption = "اغلاق"
        BtnEdit.Enabled = False
        BtnDelete.Enabled = False
        BtnSave.Enabled = True
        lode_locupEdit()
        New_Controlrs(Me)
        MyBase.BNew()
    End Sub

    Public Sub lode_locupEdit()
        ueserTyp.Enabled = False
        LoadToControlar(ueserTyp, "Daily_transfer_preparer_schedule_DEttelse_locukupEdit_proc", "NAMe_Type", "ID", Nothing)
    End Sub
    Public Overrides Sub Print()
        Me.Close()
        MyBase.Print()
    End Sub

    Public Sub lod_data(ueserType As ULong)
        Try
            BtnNew.PerformClick()
            Dim dt As New DataTable
            dt.Clear()
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@ACCID", SqlDbType.Int) With {.Value = ueserType}
            dt = RUN_QUARY_PRO("Daily_transfer_preparer_schedule_DEttelse_get_data", prm)

            If dt.Rows.Count > 0 Then
                Daily.EditValue = dt.Rows(0)("Daily")
                Weekly.EditValue = dt.Rows(0)("Weekly")
                monthly.EditValue = dt.Rows(0)("monthly")
                Annual.EditValue = dt.Rows(0)("Annual")
                Notes.Text = dt.Rows(0)("Notes")
                ueserTyp.EditValue = dt.Rows(0)("ACCID")
                ueserISType = dt.Rows(0)("ueserTyp")
            Else


            End If
        Catch ex As Exception
            ErrorMessage2(ex.Message, "Daily_transfer_preparer_schedule_DEttelse_get_data eroor ")
        End Try
    End Sub
    Dim ueserISType As Integer



    Public Sub Daily_transfer_preparer_schedule_DEttelse_update()
        Try
            If ueserTyp.EditValue = -1 Then
                ueserTyp.ErrorText = "الرجاء تحديد نوع المستخدم"
                Return
            End If
            Dim prm(5) As SqlParameter
            prm(0) = New SqlParameter("@Daily", SqlDbType.Decimal, 18, 3) With {.Value = Daily.EditValue}
            prm(1) = New SqlParameter("@Weekly", SqlDbType.Decimal, 18, 3) With {.Value = Weekly.EditValue}
            prm(2) = New SqlParameter("@monthly", SqlDbType.Decimal, 18, 3) With {.Value = Weekly.EditValue}
            prm(3) = New SqlParameter("@Annual", SqlDbType.Decimal, 18, 3) With {.Value = Annual.EditValue}
            prm(4) = New SqlParameter("@Notes", SqlDbType.NVarChar, 450) With {.Value = Notes.Text}
            prm(5) = New SqlParameter("@ACCID", SqlDbType.Int) With {.Value = ueserTyp.EditValue}
            RUN_EXUTE_PRO("Daily_transfer_preparer_schedule_DEttelse_update", prm)
            BtnNew.PerformClick()
            FRMDaily_transfer_preparer_schedule_DEttelse.lodedate(ueserISType)
            Me.Close()
        Catch ex As Exception
            ErrorMessage2(ex.Message, "Daily_transfer_preparer_schedule_DEttelse_update eroor ")
        End Try
    End Sub

    Public Overrides Sub Save()
        SetData()
        FrmSavedSuccessfully.ShowDialog()
        MyBase.Save()
    End Sub
    Public Overrides Sub SetData()
        Daily_transfer_preparer_schedule_DEttelse_update()

        MyBase.SetData()
    End Sub

End Class