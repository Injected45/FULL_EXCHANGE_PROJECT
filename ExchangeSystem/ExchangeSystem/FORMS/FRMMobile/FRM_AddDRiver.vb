
Imports System.Data.SqlClient
Imports DevExpress.Data
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI

Public Class FRM_AddDRiver
    Public Sub DriversTb_Add_From()
        Try


            SplashScreenManager1.ShowWaitForm()
            GridControl1.DataSource = Nothing
            DVGFormat(GridView1)
            GridView1.ShowFindPanel()
            LoadToControlar(GridControl1, "DriversTb_Add_From", "", "", Nothing)
            SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
        End Try
    End Sub

    Public Sub DriversTb_select_All()
        Try


            SplashScreenManager1.ShowWaitForm()
            GridControl2.DataSource = Nothing
            DVGFormat(GridView2)
            GridView1.ShowFindPanel()
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@BrnchIDInsert", SqlDbType.Int) With {.Value = branchID.EditValue}
            LoadToControlar(GridControl2, "DriversTb_select_All", "", "", prm)
            SplashScreenManager1.CloseWaitForm()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub FRM_AddDRiver_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DriversTb_Add_From()
        LoadToControlar(branchID, "Lode_bransas", "BName", "ID", Nothing)
        branchID.EditValue = BID
    End Sub
    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
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

    Public Sub DriversTb_insert(ID_Driver As ULong, branchID As ULong)
        Try
            SplashScreenManager1.ShowWaitForm()

            Dim Prm(4) As SqlParameter
            Prm(0) = New SqlParameter("@ID", SqlDbType.Int) With {.Value = ID_Driver}
            Prm(1) = New SqlParameter("@bracnhID", SqlDbType.Int) With {.Value = branchID}
            Prm(2) = New SqlParameter("@msg", SqlDbType.NVarChar, -1) With {.Direction = ParameterDirection.Output}
            Prm(3) = New SqlParameter("@ISmasge", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
            Prm(4) = New SqlParameter("@safID", SqlDbType.Int) With {.Value = UserID}
            RUN_EXUTE_PRO("DriversTb_insert", Prm)
            SplashScreenManager1.CloseWaitForm()
            If Prm(3).Value = 0 Then
                SplashScreenManager1.CloseWaitForm()
                MessageBox.Show(Prm(2).Value, "رسالة خطــــــــــــــــــــــأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                LoadToControlar(GridControl1, "DriversTb_Add_From", "", "", Nothing)
                FrmSavedSuccessfully.ShowDialog()
            End If
        Catch ex As Exception

            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub GridView1_Click(sender As Object, e As EventArgs) Handles GridView1.Click
        If MessageBox.Show("هل تريد اضافة هذه المندوب الي المنظومة", "رسالة تنبيـــــــــــــــــــة", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Dim Branch As Integer = Convert.ToInt32(branchID.EditValue)
            Dim ID As Integer = Convert.ToInt32(GridView1.GetFocusedRowCellValue("ID"))
            If branchID.EditValue Is Nothing OrElse GridView1.GetFocusedRowCellValue("ID") Is Nothing Then
                MessageBox.Show("الرجاء اختيار المندوب والفرع بشكل صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            If Branch <> 0 AndAlso ID <> 0 Then
                DriversTb_insert(ID, Branch)
            Else
                MessageBox.Show("الرجاء اختيار المندوب والفرع بشكل صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub branchID_EditValueChanged(sender As Object, e As EventArgs) Handles branchID.EditValueChanged
        If branchID.EditValue > -1 Then
            DriversTb_select_All()
        End If
    End Sub
    Private Sub GridView2_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView2.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
End Class