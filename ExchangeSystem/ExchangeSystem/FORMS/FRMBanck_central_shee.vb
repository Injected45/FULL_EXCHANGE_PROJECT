Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports ExcelDataReader

Public Class FRMBanck_central_shee
    Private Sub btnImportExcel_Click(sender As Object, e As EventArgs) Handles btnImportExcel.Click
        New_Controlrs(Me)
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"

            If ofd.ShowDialog() = DialogResult.OK Then

                Try
                    Using stream As FileStream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read)
                        Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(stream)

                            Dim result As DataSet = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                                .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                    .UseHeaderRow = True
                                }
                            })

                            If result.Tables.Count = 0 Then
                                MessageBox.Show("الملف لا يحتوي على أي صفحات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Exit Sub
                            End If


                            Dim combinedTable As DataTable = Nothing

                            For Each table As DataTable In result.Tables


                                If table.Rows.Count = 0 Then Continue For

                                If combinedTable Is Nothing Then

                                    combinedTable = table.Clone()
                                End If


                                For Each row As DataRow In table.Rows
                                    combinedTable.ImportRow(row)
                                Next

                            Next

                            If combinedTable IsNot Nothing AndAlso combinedTable.Rows.Count > 0 Then
                                GridControl1.DataSource = combinedTable
                                GridView1.BestFitColumns()
                                DVGFormat2(GridView1)

                            Else
                                MessageBox.Show("لا توجد بيانات داخل الصفحات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End If

                        End Using
                    End Using

                Catch ex As Exception
                    MessageBox.Show("خطأ أثناء قراءة الملف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

            End If
        End Using
    End Sub
    Public Sub DVGFormat2(GVRole As GridView)
        GVRole.OptionsBehavior.EditingMode = True
        GVRole.OptionsBehavior.ReadOnly = False
        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.OptionsFind.AlwaysVisible = False
        GVRole.OptionsView.ShowFooter = False
        GVRole.ShowFindPanel()

        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 9, FontStyle.Regular)
        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.Appearance.OddRow.BackColor = Color.WhiteSmoke
        GVRole.OptionsView.EnableAppearanceOddRow = True
    End Sub
    Private Sub GridView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GridView1.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GridView1.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub
    Private Sub GridView1_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GridView1.CustomDrawColumnHeader
        For Each column As GridColumn In GridView1.Columns
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
#Region "Insert Data"
    Public Function Banck_Sheet_Tigare_insertttYPE() As DataTable
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Add("REFERENCE")
        dt.Columns.Add("TYPE")
        dt.Columns.Add("PHONE")
        dt.Columns.Add("IBAN")
        dt.Columns.Add("BANK_NAME")
        dt.Columns.Add("CASH_PRICE")
        dt.Columns.Add("BANK_TRANSFER_PRICE")
        dt.Columns.Add("AMOUNT_REQUESTED")
        dt.Columns.Add("COST")
        dt.Columns.Add("UiseridImpotr")
        dt.Columns.Add("insertDate")


        If GridView1.RowCount > 0 Then
            For i = 0 To GridView1.RowCount - 1
                dt.Rows.Add((GridView1.GetRowCellValue(i, "REFERENCE")), GridView1.GetRowCellValue(i, "TYPE"), GridView1.GetRowCellValue(i, "PHONE"),
                            GridView1.GetRowCellValue(i, "IBAN"), GridView1.GetRowCellValue(i, "BANK_NAME"), GridView1.GetRowCellValue(i, "CASH_PRICE"),
                            GridView1.GetRowCellValue(i, "BANK_TRANSFER_PRICE"), GridView1.GetRowCellValue(i, "AMOUNT_REQUESTED"),
                            GridView1.GetRowCellValue(i, "COST"),
                             UserID, TextEdit1.Text)
            Next
        End If
        Return dt
    End Function



    Public Sub Banck_central_shee_Type_insert()
        Try

            SplashScreenManager1.ShowWaitForm()

            If Banck_Sheet_Tigare_insertttYPE.Rows.Count = 0 Then
                SplashScreenManager1.CloseWaitForm()
                MessageBox.Show("لا توجد بيانات لإدراجها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If TextEdit1.Text = String.Empty Then
                TextEdit1.ErrorText = "يرجى إدخال تاريخ الإدراج."
                Return
            End If
            Dim PRM(0) As SqlParameter

            PRM(0) = New SqlParameter("@TYPE", SqlDbType.Structured) With {.Value = Banck_Sheet_Tigare_insertttYPE()}


            RUN_EXUTE_PRO("Banck_central_shee_Type_insert", PRM)
            SplashScreenManager1.CloseWaitForm()
            FrmSavedSuccessfully.ShowDialog()
        Catch ex As Exception
            SplashScreenManager1.CloseWaitForm()
            MessageBox.Show("خطأ أثناء إدراج البيانات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnImportExcel1_Click(sender As Object, e As EventArgs) Handles btnImportExcel1.Click
        Banck_central_shee_Type_insert()
        New_Controlrs(Me)
    End Sub

#End Region




End Class