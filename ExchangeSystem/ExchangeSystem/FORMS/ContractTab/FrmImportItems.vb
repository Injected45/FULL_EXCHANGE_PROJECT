Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraReports.UI

Public Class FrmImportItems
    Public msgST As Integer, IsUpdate As Boolean, ITMACCID As ULong, SUPACCID As ULong


    Sub GETNETTOTAL()
        If ITMQUT.EditValue <> 0.000 Or UnitPrice.EditValue <> 0.000 Then
            OverallTotal.EditValue = ITMQUT.EditValue * UnitPrice.EditValue
        End If
    End Sub
    Private Sub MeasurmentType_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles MeasurmentType.ButtonClick
        If e.Button.Index = 1 Then
            FrmProMeasurmentUnit.ShowDialog()
        End If
    End Sub
    Public Sub LOADMUID()
        MeasurmentType.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CONDB_MeasurmentUnitTb_LoadToLSBOX")
        If DT.Rows.Count > 0 Then
            MeasurmentType.Properties.DataSource = DT
            MeasurmentType.Properties.DisplayMember = "MUName"
            MeasurmentType.Properties.ValueMember = "ID"
            MeasurmentType.Properties.ShowHeader = False
            MeasurmentType.Properties.PopulateColumns()
            MeasurmentType.Properties.Columns("ID").Visible = False
        End If
    End Sub
    Sub LOADSUPID()
        SUPID.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CONDB_SupplierTb_LoadIDACCID")
        If DT.Rows.Count > 0 Then
            SUPID.Properties.DataSource = DT
            SUPID.Properties.DisplayMember = "CustName"
            SUPID.Properties.ValueMember = "ID"
            SUPID.Properties.ShowHeader = False
            SUPID.Properties.PopulateColumns()
            SUPID.Properties.Columns("ID").Visible = False
            SUPID.Properties.Columns("AccID").Visible = False
        End If
    End Sub
    Sub LOADITMID()
        ITMID.Properties.DataSource = Nothing
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO_ONLY("CONDB_CategoriesTb_LoadIDACCID")
        If DT.Rows.Count > 0 Then
            ITMID.Properties.DataSource = DT
            ITMID.Properties.DisplayMember = "ItemName"
            ITMID.Properties.ValueMember = "ID"
            ITMID.Properties.ShowHeader = False
            ITMID.Properties.PopulateColumns()
            ITMID.Properties.Columns("ID").Visible = False
            ITMID.Properties.Columns("AccID").Visible = False
        End If
    End Sub
    Sub NEWRECORD()
        ISENABLED(True)
        SUPID.EditValue = -1
        ITMID.EditValue = -1
        ITMQUT.EditValue = 0.000
        UnitPrice.EditValue = 0.000
        OverallTotal.EditValue = 0.000
        MeasurmentType.EditValue = -1
        Notes.Text = ""
        CodeID.Enabled = False
        CodeID.Text = GETMAXID("ContractDB.dbo.ImportItemsTb", "ID") + 1
        LOADMUID()
        LOADITMID()
        LOADSUPID()
        lodePreportes()
        BtnSave.Enabled = True
    End Sub
    Sub ISENABLED(IsEn As Boolean)
        SUPID.Enabled = IsEn
        ITMID.Enabled = IsEn
        ITMQUT.Enabled = IsEn
        UnitPrice.Enabled = IsEn
        OverallTotal.Enabled = IsEn
        MeasurmentType.Enabled = IsEn
        Notes.Enabled = IsEn
        CodeID.Enabled = False
    End Sub
    Public Sub CUSTOMER_INSERT(InsertDate As Date, Code As String, SUPID As Integer, ITMID As Integer, MUID As Integer, ITMQUT As Decimal, UnitPrice As Decimal, OverallTotal As Decimal,
                               Notes As String, IsUpdate As Boolean, UserID As Integer, ITMACCID As ULong, SUPACCID As ULong, BranchID As Integer, ITMNAME As String, SUPNAME As String, BillNo As String)
        Dim PRM(18) As SqlParameter
        PRM(0) = New SqlParameter("@InsertDate", SqlDbType.Date) With {.Value = InsertDate}
        PRM(1) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = Code}
        PRM(2) = New SqlParameter("@SUPID", SqlDbType.Int) With {.Value = SUPID}
        PRM(3) = New SqlParameter("@ITMID", SqlDbType.Int) With {.Value = ITMID}
        PRM(4) = New SqlParameter("@MUID", SqlDbType.Int) With {.Value = MUID}
        PRM(5) = New SqlParameter("@ITMQUT", SqlDbType.Decimal) With {.Value = ITMQUT}
        PRM(6) = New SqlParameter("@UnitPrice", SqlDbType.Decimal) With {.Value = UnitPrice}
        PRM(7) = New SqlParameter("@OverallTotal", SqlDbType.Decimal) With {.Value = OverallTotal}
        PRM(8) = New SqlParameter("@Notes", SqlDbType.NVarChar, 300) With {.Value = Notes}
        PRM(9) = New SqlParameter("@IsUpdate", SqlDbType.Bit) With {.Value = IsUpdate}
        PRM(10) = New SqlParameter("@MSGSTatues ", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
        PRM(11) = New SqlParameter("@MsgBox", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
        PRM(12) = New SqlParameter("@UserID", SqlDbType.Int) With {.Value = UserID}
        PRM(13) = New SqlParameter("@ITMACCID", SqlDbType.BigInt) With {.Value = ITMACCID}
        PRM(14) = New SqlParameter("@SUPACCID", SqlDbType.BigInt) With {.Value = SUPACCID}
        PRM(15) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BranchID}
        PRM(16) = New SqlParameter("@ITMNAME", SqlDbType.NVarChar, 300) With {.Value = ITMNAME}
        PRM(17) = New SqlParameter("@SUPNAME", SqlDbType.NVarChar, 300) With {.Value = SUPNAME}
        PRM(18) = New SqlParameter("@BillNo", SqlDbType.NVarChar, -1) With {.Value = BillNo}
        RUN_EXUTE_PRO("CONDB_ImportItemsTb_Insert", PRM)
        Me.msgST = PRM(10).Value
        If PRM(10).Value = 0 Then
            ErrorMessage(Me, "رسالة خطأ", PRM(11).Value)
            Exit Sub
        Else
            Me.BtnNew.PerformClick()
        End If
    End Sub
    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()

        dt = SElectUEserFormButtn(157, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

        End If
    End Sub
    Public Overrides Sub SetData()
        If ITMID.EditValue = -1 Then
            ITMID.ErrorText = "هذ الحقل مطلوب"
            ITMID.Select()
            Return
        End If
        If SUPID.EditValue = -1 Then
            SUPID.ErrorText = "هذ الحقل مطلوب"
            SUPID.Select()
            Return
        End If

        CUSTOMER_INSERT(Date.Now, CodeID.Text, SUPID.EditValue, ITMID.EditValue, MeasurmentType.EditValue, ITMQUT.EditValue, UnitPrice.EditValue, OverallTotal.EditValue, Notes.Text.Trim,
                        IsUpdate, UserID, ITMACCID, SUPACCID, BID, ITMID.Text.Trim, SUPID.Text.Trim, BillNo.Text.Trim)
        If msgST = 1 Then
            MyBase.SetData()
        End If
    End Sub
    Public Overrides Sub Save()
        SetData()
        NEWRECORD()
        MyBase.Save()
    End Sub

    Private Sub ITMID_EditValueChanged(sender As Object, e As EventArgs) Handles ITMID.EditValueChanged
        If ITMID.EditValue <> -1 Or ITMID.Text <> String.Empty Then
            Dim editor As LookUpEdit = TryCast(sender, LookUpEdit)
            Dim value As Object = editor.GetColumnValue("AccID")
            ITMACCID = value
        End If
    End Sub

    Private Sub SUPID_EditValueChanged(sender As Object, e As EventArgs) Handles SUPID.EditValueChanged
        If SUPID.EditValue <> -1 Or SUPID.Text <> String.Empty Then
            Dim editor As LookUpEdit = TryCast(sender, LookUpEdit)
            Dim value As Object = editor.GetColumnValue("AccID")
            SUPACCID = value
        End If
    End Sub

    Private Sub FrmImportItems_Load(sender As Object, e As EventArgs) Handles Me.Load
        NEWRECORD()
    End Sub

    Private Sub ITMQUT_Leave(sender As Object, e As EventArgs) Handles ITMQUT.Leave
        GETNETTOTAL()
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        FrmViewImportItems.ShowDialog()
    End Sub

    Private Sub ITMQUT_EditValueChanged(sender As Object, e As EventArgs) Handles ITMQUT.EditValueChanged
        GETNETTOTAL()
    End Sub

    Private Sub UnitPrice_EditValueChanged(sender As Object, e As EventArgs) Handles UnitPrice.EditValueChanged
        GETNETTOTAL()
    End Sub

    Private Sub UnitPrice_Leave(sender As Object, e As EventArgs) Handles UnitPrice.Leave
        GETNETTOTAL()
    End Sub
    Public Sub Pro_SelectByID(x)
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = x}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_ImportItemsTb_Select", PRM)
        If dt.Rows.Count > 0 Then
            CodeID.Text = dt.Rows(0).Item("Code")
            SUPID.EditValue = dt.Rows(0).Item("SUPID")
            ITMID.EditValue = dt.Rows(0).Item("ITMID")
            MeasurmentType.EditValue = dt.Rows(0).Item("MUID")
            ITMQUT.EditValue = dt.Rows(0).Item("ITMQUT")
            UnitPrice.EditValue = dt.Rows(0).Item("UnitPrice")
            OverallTotal.EditValue = dt.Rows(0).Item("OverallTotal")
            Notes.Text = dt.Rows(0).Item("Notes")
        End If
    End Sub
    Public Overrides Sub BNew()
        NEWRECORD()
        MyBase.BNew()
    End Sub

    Public Overrides Sub Print()
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@Code", SqlDbType.NVarChar, 300) With {.Value = CodeID.Text}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("CONDB_ImportItemsTb_LoadToPrint", PRM)
        If dt.Rows.Count > 0 Then
            Dim report As New RPTImportItems
            report.DataSource = dt
            report.DataMember = "ImportItemsTb"
            Dim tool As ReportPrintTool = New ReportPrintTool(report)
            report.XrLabel18.Text = Cur_Code("دينار ليبي", OverallTotal.EditValue, False, "n2")
            report.CreateDocument()
            report.ShowPreview()
        End If
        MyBase.Print()
    End Sub
End Class