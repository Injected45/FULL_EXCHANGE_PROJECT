Imports DevExpress.Data
Imports DevExpress.LookAndFeel.DXSkinColors
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports MetroFramework
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading

Public Class FrmDriverDeliveryInternalShipping
    Public IDcode As ULong
    Public IsUpdate As Boolean
    Public Frmid As Integer


    Sub DVGFormat()
        Dim rowIdx As Integer = GridView2.DataRowCount - 1
        For i As Integer = rowIdx To 0 Step -1
            Dim CellValue As Object = GridView2.GetRowCellValue(i, "Code")
            If CellValue Is Nothing OrElse IsDBNull(CellValue) OrElse String.IsNullOrWhiteSpace(CellValue.ToString()) Then
                GridView2.DeleteRow(i)
            End If
        Next

        GridView2.AddNewRow()

        GridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GridView2.OptionsView.ShowGroupPanel = False
        GridView2.OptionsView.ShowFooter = True
        GridView2.Appearance.Row.Font = New Font("Droid Arabic Kufi", 8, FontStyle.Regular)
        For i As Integer = 0 To GridView2.Columns.Count - 1
            GridView2.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView2.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GridView2.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GridView2.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GridView2.OptionsView.EnableAppearanceEvenRow = True
        GridView2.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GridView2.OptionsView.EnableAppearanceOddRow = True
        GridView2.Columns(0).Width = 100
    End Sub

    Public Sub lodePreportes()
        Dim dt As New DataTable
        dt.Clear()
        dt = SElectUEserFormButtn(Frmid, UserID)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CanSave") = 0 Then BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanEdit") = 0 Then BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanPrint") = 0 Then BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            If dt.Rows(0)("CanDelete") = 0 Then BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Else BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Public Sub neW_Recordirs()
        New_Controlrs(Me)
        TabbedControlGroup1.SelectedTabPageIndex = 0
        InsertDate.Text = Date.Now
        TaxiInvoiceDrivers_Select_ID_Codee()
        IsUpdate = False
        '====================================
        GridControl11.LookAndFeel.UseDefaultLookAndFeel = False

        GridView2.BorderStyle = BorderStyles.NoBorder
        Dim bindlis As List(Of Entrye) = New List(Of Entrye)

        Dim binddata As BindingSource = New BindingSource
        binddata.DataSource = bindlis
        GridControl11.DataSource = binddata
        DVGFormat()
        lodeDriver()
        lodePreportes()
    End Sub
    Private Sub GridView2_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView2.CustomDrawColumnHeader
        For Each column As GridColumn In GridView2.Columns
            column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
            column.OptionsFilter.AllowAutoFilter = False
            column.OptionsFilter.AllowFilter = False
            column.OptionsColumn.AllowMove = False
            column.OptionsColumn.AllowSize = False
            column.OptionsColumn.ReadOnly = True

        Next

        If e.Column Is Nothing Then
            Return
        End If

        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(64, 64, 64), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)

        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True


    End Sub

    Public Sub lodeDriver()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@branchID", SqlDbType.Int) With {.Value = BID}
        LoadToControlar(DriverID, "DriversTb_select", "DriverName", "ID", prm)
    End Sub
    Public Sub lode_mopile(DriverID As ULong)
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = DriverID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("DriversTb_GetPhone", prm)
        If dt.Rows.Count > 0 Then
            DPhone1.Text = dt.Rows(0)("Phone1")
            DPhone2.Text = dt.Rows(0)("Phone2")

        End If

    End Sub


    Public Sub TaxiInvoiceDrivers_Select_ID_Codee()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("TaxiInvoiceDrivers_Select_ID_Codee", prm)
        If dt.Rows.Count > 0 Then
            IDcode = dt.Rows(0)("ID_Cdoe")
            CodeID.Text = dt.Rows(0)("code")
        End If
    End Sub

    Public Overrides Sub BNew()
        neW_Recordirs()
        MyBase.BNew()
    End Sub

    Private Sub DriverID_EditValueChanged(sender As Object, e As EventArgs) Handles DriverID.EditValueChanged
        If DriverID.Text <> String.Empty AndAlso DriverID.EditValue > -1 Then
            lode_mopile(DriverID.EditValue)
        End If
    End Sub

    Private Sub FrmDriverDeliveryInternalShipping_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnNew.PerformClick()
    End Sub

    Private Sub BTNSEARCH_Click(sender As Object, e As EventArgs) Handles BTNSEARCH.Click
        FRMSEARCHINTERNALSHIPPING.ShowDialog()
    End Sub
    Sub SHOW_IS(code As String)


        Dim DT As New DataTable
        Dim DT1 As New DataTable
        DT.Clear()
        DT1.Clear()

        DT = SHSTIPINGDEVREFORDRIVE(code)
        For i As Integer = 0 To GridView2.RowCount - 2
            If GridView2.GetRowCellValue(i, "Code") = code Then
                MessageBox.Show(Me, "رقم الفاتورة موجود مسبقا", "رسالة خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Next

        If DT.Rows.Count > 0 Then
            Dim DT2 As New DataTable
            DT2.Clear()

            DT2 = SHSTIPINGDEVREFORDRIVE(code)
            GridView2.AddNewRow()
            GridView2.OptionsView.ShowGroupPanel = False
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "ID", DT2.Rows(0)("ID").ToString)
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "Code", DT2.Rows(0)("Code").ToString)
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "RecievedName", DT2.Rows(0)("RecievedName").ToString)
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "RPhone1", DT2.Rows(0)("RPhone1").ToString)
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "SenderName", DT2.Rows(0)("SenderName").ToString)
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "SPhone1", DT2.Rows(0)("SPhone1").ToString)
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "OverallVal", DT2.Rows(0)("OverallVal"))
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "Taxi_Ret_DriverS", DT2.Rows(0)("Taxi_Ret_DriverS"))
            GridView2.SetRowCellValue(GridControl.NewItemRowHandle, "TaxiValues", DT2.Rows(0)("TaxiValues"))


            DVGFormat()
            SumTotalPriceAndQT()

        End If







    End Sub
    Sub SumTotalPriceAndQT()

        DriverVal.EditValue = 0.00
        If GridView2.RowCount > 0 Then
            Dim SumCredit As New GridColumnSummaryItem()
            SumCredit.SummaryType = SummaryItemType.Sum
            SumCredit.FieldName = "Taxi_Ret_DriverS"
            GridView2.Columns("Taxi_Ret_DriverS").Summary.Add(SumCredit)
            GridView2.OptionsView.ShowFooter = False
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim SumDebit As New GridColumnSummaryItem()
            SumDebit.SummaryType = SummaryItemType.Sum
            SumDebit.FieldName = "OverallVal"
            GridView2.Columns("OverallVal").Summary.Add(SumDebit)
            GridView2.OptionsView.ShowFooter = False


            OverAllQt.Text = GridView2.RowCount - 1
            OverAllPrice.Text = Format((GridView2.Columns("OverallVal").SummaryItem.SummaryValue), "#,###.000")
            DriverShare.Text = Format((GridView2.Columns("Taxi_Ret_DriverS").SummaryItem.SummaryValue), "#,###.000")

        End If
        For Each column As GridColumn In GridView2.Columns
            Dim item As GridSummaryItem = column.SummaryItem
            If item IsNot Nothing Then
                column.Summary.Remove(item)
            End If

        Next column
    End Sub


    Public Function SHSTIPINGDEVREFORDRIVE(code As String) As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Dispose()
        Dim prm(0) As SqlParameter
        prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = code}
        dt = RUN_QUARY_PRO("SHSTIPINGDEVREFORDRIVE", prm)
        Return dt
    End Function

    Private Sub DriverShare_EditValueChanged(sender As Object, e As EventArgs) Handles DriverShare.EditValueChanged
        DriverVal.EditValue = Val(OverAllPrice.Text - DriverShare.Text)
    End Sub

    Private Sub OverAllPrice_EditValueChanged(sender As Object, e As EventArgs) Handles OverAllPrice.EditValueChanged
        DriverVal.Text = Val(OverAllPrice.Text - DriverShare.Text)
    End Sub

    Public Function InternalEx_type_isertAStaxi() As DataTable

        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("ID")

        For i As Integer = 0 To GridView2.RowCount - 1
            Dim ID_Detels As ULong = GridView2.GetRowCellValue(i, "ID")
            If ID_Detels <> 0 Then
                dt.Rows.Add(ID_Detels)
            End If

        Next
        Return dt
    End Function
    Public Overrides Sub SetData()
        If InternalEx_type_isertAStaxi.Rows.Count = 0 Then
            ErrorMessage(Me, "رسالة خطأ", "الرجاء اختيار صنف واحد علي اقل")
            Exit Sub
        End If
        If DriverID.EditValue = -1 Then
            DriverID.ErrorText = "الرجاء اختيار السائق"
            Exit Sub
        End If
        TaxiInvoiceDrivers_insert()
        MyBase.Save()
    End Sub
    Public Sub TaxiInvoiceDrivers_insert()
        Try


            SplashScreenManager1.ShowWaitForm()
            Dim prm(10) As SqlParameter
            prm(0) = New SqlParameter("@Code", SqlDbType.NVarChar, -1) With {.Value = CodeID.Text}
            prm(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = BID}
            prm(2) = New SqlParameter("@Driver_ID", SqlDbType.Int) With {.Value = DriverID.EditValue}
            prm(3) = New SqlParameter("@DriversShare", SqlDbType.Decimal) With {.Precision = 18, .Scale = 3, .Value = Convert.ToDecimal(DriverShare.Text)}
            prm(4) = New SqlParameter("@ueserIinsert", SqlDbType.Int) With {.Value = Convert.ToInt32(UserID)}
            prm(5) = New SqlParameter("@Notes", SqlDbType.NVarChar, -1) With {.Value = Notes.Text}
            prm(6) = New SqlParameter("@InternalEx_type_isertAStaxi", SqlDbType.Structured) With {.Value = InternalEx_type_isertAStaxi()}
            prm(7) = New SqlParameter("@ID_Code", SqlDbType.Int) With {.Value = Convert.ToInt32(IDcode)}
            prm(8) = New SqlParameter("@ismasg", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            prm(9) = New SqlParameter("@masge", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            prm(10) = New SqlParameter("@safeID_ACID", SqlDbType.Int) With {.Value = UserAccID}
            RUN_EXUTE_PRO("TaxiInvoiceDrivers_insert", prm)
            If prm(8).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                ErrorMessage2("ErrorMessage2", prm(9).Value)
                BtnNew.PerformClick()
            Else
                BtnNew.PerformClick()
                SplashScreenManager1.CloseWaitForm()
            End If
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            ErrorMessage2("Catch  Message ", ex.Message)
        End Try

    End Sub
    Public Overrides Sub Save()
        SetData()
    End Sub
End Class
Public Class Entrye
    Public Property ID() As ULong
    Public Property Code() As String
    Public Property RecievedName() As String
    Public Property RPhone1() As String
    Public Property SenderName() As String
    Public Property SPhone1() As String
    Public Property OverallVal() As Decimal
    Public Property Taxi_Ret_DriverS() As Decimal
    Public Property TaxiValues() As Decimal

End Class