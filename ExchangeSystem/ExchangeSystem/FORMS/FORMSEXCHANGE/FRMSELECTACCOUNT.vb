Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports DevExpress.Xpo.DB.Helpers

Public Class FRMSELECTACCOUNT
    Public TransType As Integer
    Public AccName As String
    Public SendOrRec As Integer
    Dim Branch As Integer
    Private Sub FRMSELECTACCOUNT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        New_Controlrs(Me)
        Branch = BID
        Dim AddRoww As Boolean = True
        If SendOrRec = 1 And TransType = 0 Then
            Branch = FRMINTERNALTRANSFER.BranchDeliveredID.EditValue
            AddRoww = False
        End If
        If SendOrRec = 1 And TransType = 1 Then
            Branch = FRMEXTERNALTRANS.BranchDeliveredID.EditValue
            AddRoww = False
        End If
        Dim PR(0) As SqlParameter
        PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = Branch}
        LoadToControlar(AccountType, "InternalEX_AccountTBLoadLine3ToLKP", "AccName", "AccCode", PR, AddRoww, "مصروفات عمومية")
    End Sub


    Sub BtnClikInTernal()
        If SendOrRec = 0 Then
            If AccID.Text <> String.Empty Then
                Dim selectedRow As DataRowView = TryCast(AccID.GetSelectedDataRow(), DataRowView)
                If selectedRow IsNot Nothing Then
                    FRMINTERNALTRANSFER.AccFrom = AccID.EditValue
                    FRMINTERNALTRANSFER.SenderName.Text = AccID.Text
                    FRMINTERNALTRANSFER.SPhone1.Text = selectedRow("AccPhone").ToString
                    FRMINTERNALTRANSFER.SPhone2.Text = selectedRow("AccMobile").ToString
                    FRMINTERNALTRANSFER.SenderIDNo.Text = selectedRow("AccIDNo").ToString
                End If
            End If
        Else
            Dim selectedRow As DataRowView = TryCast(AccID.GetSelectedDataRow(), DataRowView)
            If selectedRow IsNot Nothing Then
                FRMINTERNALTRANSFER.TransAccIDTo = AccID.EditValue
                FRMINTERNALTRANSFER.RecievedName.Text = AccID.Text
                FRMINTERNALTRANSFER.RPhone1.Text = selectedRow("AccPhone").ToString
                FRMINTERNALTRANSFER.RPhone2.Text = selectedRow("AccMobile").ToString
                FRMINTERNALTRANSFER.RecievedIDNo.Text = selectedRow("AccIDNo").ToString
            End If
        End If
        Me.Close()
    End Sub

    Sub BtnClikExTernal()
        If SendOrRec = 0 Then
            If AccID.Text <> String.Empty Then
                Dim selectedRow As DataRowView = TryCast(AccID.GetSelectedDataRow(), DataRowView)
                If selectedRow IsNot Nothing Then
                    FRMEXTERNALTRANS.AccFrom = AccID.EditValue
                    FRMEXTERNALTRANS.SenderName.Text = AccID.Text
                    FRMEXTERNALTRANS.SPhone1.Text = selectedRow("AccPhone").ToString
                    FRMEXTERNALTRANS.SPhone2.Text = selectedRow("AccMobile").ToString
                    FRMEXTERNALTRANS.SenderIDNo.Text = selectedRow("AccIDNo").ToString
                End If
            End If
        Else
            Dim selectedRow As DataRowView = TryCast(AccID.GetSelectedDataRow(), DataRowView)
            If selectedRow IsNot Nothing Then
                FRMEXTERNALTRANS.TransAccIDTo = AccID.EditValue
                FRMEXTERNALTRANS.RecievedName.Text = AccID.Text
                FRMEXTERNALTRANS.RPhone1.Text = selectedRow("AccPhone").ToString
                FRMEXTERNALTRANS.RPhone2.Text = selectedRow("AccMobile").ToString
                FRMEXTERNALTRANS.OwnNatioNum.Text = selectedRow("AccIDNo").ToString
            End If
        End If
        Me.Close()
    End Sub

    Private Sub AccountType_EditValueChanged(sender As Object, e As EventArgs) Handles AccountType.EditValueChanged
        AccID.Properties.DataSource = Nothing
        If AccountType.Text <> String.Empty Then
            Dim PR(3) As SqlParameter
            PR(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = Branch}
            PR(1) = New SqlParameter("@AccParent", SqlDbType.BigInt) With {.Value = AccountType.EditValue}
            PR(2) = New SqlParameter("@SendOrRec", SqlDbType.TinyInt) With {.Value = SendOrRec}
            PR(3) = New SqlParameter("@CurrencyID", SqlDbType.Int) With {.Value = DefaultCurrency}
            LoadToControlar(AccID, "InternalEX_LOADINTOLKPBASEDONAccParent", "AccName", "AccID", PR)
            HideAllColumnsExceptDisplayAndVAl(AccID)
        End If
    End Sub

    Private Sub AccID_EditValueChanged(sender As Object, e As EventArgs) Handles AccID.EditValueChanged
        If AccID.Text <> String.Empty Then
            If TransType = 0 Then
                BtnClikInTernal()
            Else
                BtnClikExTernal()
            End If
        End If
    End Sub

End Class