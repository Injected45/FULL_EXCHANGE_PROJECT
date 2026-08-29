Imports System.Data.SqlClient
Imports DevExpress.Pdf.Native.BouncyCastle.Utilities.Collections

Public Class FRMCurrLimited
    Public IsUpdate As Boolean
    Sub NEWRECORD()

        IsUpdate = False
        MaxVal.EditValue = 0.000
        CurrencyFrom.EditValue = -1
        IsActiveTG.IsOn = True
        BtnSave.Enabled = True
        Code.Text = GETMAXID("CurrLimetedTB", "ID") + 1
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        IsEnabled(True)
        LOADCIDFROM(IsUpdate)
        lodePreportes()
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(165, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If
    End Sub

    Sub IsEnabled(IsEn As Boolean)
        MaxVal.Enabled = True
        CurrencyFrom.Enabled = IsEn
    End Sub

    Sub LOADCIDFROM(ISUpdate As Boolean)
        Dim DT As New DataTable
        If ISUpdate = False Then
            DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP_GetHasNoLimeted")
        Else
            DT = RUN_QUARY_PRO_ONLY("CurrencyMainTb_LOADTOLKP_bu")
        End If
        If DT.Rows.Count > 0 Then
            CurrencyFrom.Properties.DataSource = DT
            CurrencyFrom.Properties.ValueMember = "ID"
            CurrencyFrom.Properties.DisplayMember = "CuName"
            CurrencyFrom.Properties.ShowHeader = False
        Else
            CurrencyFrom.Properties.DataSource = Nothing
        End If
    End Sub


    Public Overrides Sub Save()
        SetData()
        NEWRECORD()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If MaxVal.EditValue <= 0 Then
                MaxVal.ErrorText = "القيمة يجب أن تكون أكبر من صفر"
                Return
            End If
            IsActiveTG.EditValue = True
            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code.Text.Trim}
            PRM(1) = New SqlParameter("@MaxVal", SqlDbType.Decimal) With {.Value = MaxVal.EditValue}
            PRM(2) = New SqlParameter("@CurID", SqlDbType.NVarChar, -1) With {.Value = CurrencyFrom.EditValue}
            'PRM(3) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}
            PRM(3) = New SqlParameter("@IsEnabled", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
            PRM(4) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            RUN_EXUTE_PRO("CurrLimetedTB_Insert", PRM)
        End If
        NEWRECORD()
        MyBase.UPDATERECORD()
    End Sub
    Public Overrides Sub SetData()
        If IsUpdate = False Then
            If CurrencyFrom.EditValue = -1 Then
                CurrencyFrom.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            If MaxVal.EditValue <= 0 Then
                MaxVal.ErrorText = "القيمة يجب أن تكون أكبر من صفر"
                Return
            End If

            IsActiveTG.EditValue = True

            Dim PRM(4) As SqlParameter
            PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code.Text.Trim}
            PRM(1) = New SqlParameter("@MaxVal", SqlDbType.Decimal) With {.Value = MaxVal.EditValue}
            PRM(2) = New SqlParameter("@CurID", SqlDbType.NVarChar, -1) With {.Value = CurrencyFrom.EditValue}
            'PRM(3) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = TypeID.SelectedIndex}
            PRM(3) = New SqlParameter("@IsEnabled", SqlDbType.Bit) With {.Value = IsActiveTG.EditValue}
            PRM(4) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
            RUN_EXUTE_PRO("CurrLimetedTB_Insert", PRM)
        End If
        MyBase.SetData()
    End Sub
    Public Function SERACH_EMPORC(Code As Integer) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = Code}

        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("CurrLimetedTB_Select", PRM)
        Return DT
    End Function
    Sub SHOW_EMCUSCODE(x)
        If Me.IsUpdate = True Then
            LOADCIDFROM(IsUpdate)
            Dim DT As New DataTable
            DT.Clear()
            DT = SERACH_EMPORC(x)
            If DT.Rows.Count > 0 Then
                Code.Text = DT.Rows(0)("ID")
                MaxVal.EditValue = DT.Rows(0)("MaxVal")
                CurrencyFrom.EditValue = DT.Rows(0)("CurID")
                IsActiveTG.EditValue = DT.Rows(0)("IsEnabled")
                'TypeID.SelectedIndex = DT.Rows(0)("TypeID")
            End If
        End If
    End Sub

    Private Sub Code_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles Code.ButtonClick
        FrmViewCurrLimited.ShowDialog()
    End Sub

    Private Sub FRMCurrLimited_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NEWRECORD()
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub
    Private Sub Code_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Code.KeyPress
        e.Handled = True
    End Sub
End Class