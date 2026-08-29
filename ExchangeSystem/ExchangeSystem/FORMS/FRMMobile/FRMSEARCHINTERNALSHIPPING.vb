Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Localization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Public Class FRMSEARCHINTERNALSHIPPING

    Sub DVGFORMAT()
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        Next
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 7, FontStyle.Regular)
        GVRole.OptionsFind.AlwaysVisible = True

        GVRole.Appearance.EvenRow.BackColor = Color.FromArgb(200, 255, 249, 196)
        GVRole.OptionsView.EnableAppearanceEvenRow = True
        GVRole.ShowFindPanel()
        GVRole.Appearance.OddRow.BackColor = Color.FloralWhite
        GVRole.OptionsBehavior.Editable = False
    End Sub

    Private Sub FRMARSEARCHINTERNALSHIPPING_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GET_Deteels_fOr_taxe(BID)
        DVGFORMAT()







        If FrmDriverDeliveryInternalShipping.IsUpdate = True Then

            If FrmDriverDeliveryInternalShipping.GridView2.DataRowCount > 1 Then


                Dim roid As Integer = Me.GVRole.RowCount - 1

                For i As Integer = 0 To FrmDriverDeliveryInternalShipping.GridView2.DataRowCount - 1

                    For j As Integer = roid To 0 Step -1
                        Dim CellValue As Object = FrmDriverDeliveryInternalShipping.GridView2.GetRowCellValue(i, "Code")
                        Dim valuecell As Object = Me.GVRole.GetRowCellValue(j, "Code")

                        If Not IsDBNull(CellValue) Then




                            If CellValue = valuecell Then

                                Me.GVRole.DeleteRow(j)

                            End If
                        End If


                    Next
                Next
            End If
        ElseIf FrmDriverDeliveryInternalShipping.IsUpdate = False Then


            Dim rowIdx As Integer = FrmDriverDeliveryInternalShipping.GridView2.DataRowCount - 1

            Dim roid As Integer = Me.GVRole.RowCount - 1
            For i As Integer = rowIdx To 0 Step -1
                For j As Integer = roid To 0 Step -1
                    Dim CellValue As Object = FrmDriverDeliveryInternalShipping.GridView2.GetRowCellValue(i, "Code")
                    Dim valuecell As Object = Me.GVRole.GetRowCellValue(j, "Code")

                    If valuecell Is Nothing OrElse IsDBNull(valuecell) OrElse String.IsNullOrWhiteSpace(valuecell.ToString()) OrElse CellValue = valuecell Then

                        Me.GVRole.DeleteRow(j)

                    End If
                    '  End If
                Next
            Next
        End If
    End Sub

    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        ' Handle this event to paint columns headers manually
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(62, 187, 158), e.Bounds)
        e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect)
        ' Draw the filter and sort buttons.
        For Each info As DrawElementInfo In e.Info.InnerElements
            If Not info.Visible Then
                Continue For
            End If
            ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo)
        Next info
        e.Handled = True
    End Sub

    Private Sub GVRole_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles GVRole.CustomUnboundColumnData
        If e.Column.FieldName = "SN" And e.IsGetData Then
            e.Value = GVRole.GetRowHandle(e.ListSourceRowIndex) + 1
        End If
    End Sub

    Public Sub GET_Deteels_fOr_taxe(DeliveryPlaceID As ULong)
        Try


            Dim prm(1) As SqlParameter
            prm(0) = New SqlParameter("@DeliveryPlaceID", SqlDbType.Int) With {.Value = DeliveryPlaceID}
            prm(1) = New SqlParameter("@ConfirmType", SqlDbType.Int) With {.Value = 8}
            GCRole.DataSource = Nothing
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO("GET_Deteels_fOr_taxe", prm)
            If dt.Rows.Count = Nothing Then Return
            If dt.Rows.Count > 0 Then
                GCRole.DataSource = dt
            Else
                GCRole.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub GVRole_DoubleClick(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)

        If info.InRow OrElse info.InRowCell Then
            Dim roleId As String = view.GetFocusedRowCellValue("Code")
            'FrmDriverDeliveryInternalShipping.IsUpdate = False
            FrmDriverDeliveryInternalShipping.BtnSave.Enabled = True
            FrmDriverDeliveryInternalShipping.BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            FrmDriverDeliveryInternalShipping.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            FrmDriverDeliveryInternalShipping.BTDelete.Enabled = False
            FrmDriverDeliveryInternalShipping.BtnEdit.Enabled = False
            FrmDriverDeliveryInternalShipping.BTNSEARCH1.Enabled = False


            Dim fisd As String = view.GetFocusedRowCellValue("Code")

            'FrmDriverDeliveryInternalShipping.SHOW_UP_TOTP()
            FrmDriverDeliveryInternalShipping.SHOW_IS(fisd)
            If FrmDriverDeliveryInternalShipping.IsUpdate = True Then
                FrmDriverDeliveryInternalShipping.BtnSave.Enabled = False
                FrmDriverDeliveryInternalShipping.BTDelete.Enabled = True
                FrmDriverDeliveryInternalShipping.BtnEdit.Enabled = True
                FrmDriverDeliveryInternalShipping.BTNSEARCH1.Enabled = True
            End If



            Me.Close()
        End If

    End Sub


End Class