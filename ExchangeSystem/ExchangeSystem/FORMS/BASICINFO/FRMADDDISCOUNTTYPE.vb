Imports System.Data.SqlClient
Imports System.Threading

Public Class FRMADDDISCOUNTTYPE
    Dim clsdis As New CLSEDISCOUNTTYPE
    Public IsUpdate As Boolean






    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(104, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub
    Sub NEWRECORD()
        IsUpdate = False
        CodeID.Text = GETMAXID("DiscountTypeTb", "ID") + 1
        DISNAME.Text = ""
        DISNAME.Select()
        IsActiveTG.IsOn = True
        BtnSave.Enabled = True
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnEdit.Enabled = False
    End Sub
    Public Overrides Sub SetData()
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@DISNAME", SqlDbType.NVarChar, 50) With {.Value = DISNAME.Text.Trim}
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("DiscountTypeTb_Select", PR)
        If DT.Rows.Count > 0 Then
            DISNAME.ErrorText = "هذا الاسم موجود مسبقاً"
            Return
        End If
        If DISNAME.Text = "" Then
            DISNAME.ErrorText = "هذا الحقل مطلوب"
            Return
        End If
        clsdis.INSERTTB__DiscountTypeTb(DISNAME.Text.Trim)
        NEWRECORD()
        MyBase.SetData()
    End Sub
    Public Overrides Sub Save()
        SetData()
        MyBase.Save()
    End Sub
    Public Overrides Sub UPDATERECORD()
        If IsUpdate = True Then
            If DISNAME.Text = "" Then
                DISNAME.ErrorText = "هذا الحقل مطلوب"
                Return
            End If
            clsdis.UPDATETB_DiscountTypeTb(CodeID.Text, DISNAME.Text.Trim)
        End If
        MyBase.UPDATERECORD()
    End Sub
    Sub SHOW_REC(X)
        IsUpdate = True
        Dim PR(0) As SqlParameter
            PR(0) = New SqlParameter("@DISNAME", SqlDbType.NVarChar, 50) With {.Value = X}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("DiscountTypeTb_Select", PR)
            If DT.Rows.Count > 0 Then
                DISNAME.Text = DT.Rows(0)("DISNAME")
                CodeID.Text = DT.Rows(0)("ID")
            End If

    End Sub
    Private Sub FRMADDDISCOUNTTYPE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        lodePreportes()
        NEWRECORD()
    End Sub
    Private Sub PictureEdit11_Click(sender As Object, e As EventArgs) Handles PictureEdit11.Click
        FRMVIEWDISCOUNT.ShowDialog()
    End Sub
End Class