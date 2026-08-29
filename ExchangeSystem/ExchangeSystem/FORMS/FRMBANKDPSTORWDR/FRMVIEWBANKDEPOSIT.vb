Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting.Shape.Native

Public Class FRMVIEWBANKDEPOSIT
    Public Sub LoadData()
        GVRole.Columns.Clear()
        Dim PR(1) As SqlParameter
        If Application.OpenForms().OfType(Of FRMBANKDEPOSIT).Any Then
            PR(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = FRMBANKDEPOSIT.LOADTYPE}
            PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FRMBANKDEPOSIT.BranchID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("BankDipWdTb_LOADTODVG", PR)
            If DT.Rows.Count > 0 Then
                GCRole.DataSource = DT
                GVRole.Columns("TypeID").Visible = False
                NEWDVGFROMAT(GVRole)
            End If
        End If
        If Application.OpenForms().OfType(Of FRMProBANKDEPOSIT).Any Then
            PR(0) = New SqlParameter("@TypeID", SqlDbType.TinyInt) With {.Value = FRMProBANKDEPOSIT.LOADTYPE}
            PR(1) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = FRMProBANKDEPOSIT.BranchID.EditValue}
            Dim DT As New DataTable
            DT.Clear()
            DT = RUN_QUARY_PRO("CONDB_BankDipWdTb_LOADTODVG", PR)
            If DT.Rows.Count > 0 Then
                GCRole.DataSource = DT
                GVRole.Columns("TypeID").Visible = False
                NEWDVGFROMAT(GVRole)
            End If
        End If


    End Sub

    Private Sub FRMVIEWBANKDEPOSIT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub
    Private Sub GVRole_CustomDrawColumnHeader(sender As Object, e As ColumnHeaderCustomDrawEventArgs) Handles GVRole.CustomDrawColumnHeader
        If e.Column Is Nothing Then
            Return
        End If
        ' Fill column headers with the specified colors.
        e.Appearance.ForeColor = Color.White
        e.Cache.FillRectangle(Color.FromArgb(231, 72, 86), e.Bounds)
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

    Private Sub GVRole_Click(sender As Object, e As EventArgs) Handles GVRole.DoubleClick
        Dim ea As DXMouseEventArgs = TryCast(e, DXMouseEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        Dim info = view.CalcHitInfo(ea.Location)
        If info.InRow OrElse info.InRowCell Then
            'Dim roleId As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("ID"))
            Dim CO As String = view.GetFocusedRowCellValue("الرمز").ToString
            Dim TypeID As Integer = Convert.ToInt32(view.GetFocusedRowCellValue("TypeID"))
            If Application.OpenForms().OfType(Of FRMBANKDEPOSIT).Any Then
                FRMBANKDEPOSIT.IsUpdate = True
                FRMBANKDEPOSIT.LOADBRANCH()
                FRMBANKDEPOSIT.LOADTYPE = TypeID
                FRMBANKDEPOSIT.BANKDEPOSIT_GetRecord(CO, TypeID)
                FRMBANKDEPOSIT.BtnSave.Enabled = False
                FRMBANKDEPOSIT.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                'FRMBANKDEPOSIT.BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                FRMBANKDEPOSIT.BtnEdit.Enabled = True
                FRMBANKDEPOSIT.BtnPrint.Enabled = True
                FRMBANKDEPOSIT.DISAPLEDCONTROLS()
                FRMBANKDEPOSIT.BtnEdit.Caption = "استرجاع القيمة"
            End If
            If Application.OpenForms().OfType(Of FRMProBANKDEPOSIT).Any Then
                FRMProBANKDEPOSIT.IsUpdate = True
                FRMProBANKDEPOSIT.LOADBRANCH()
                FRMProBANKDEPOSIT.LOADTYPE = TypeID
                FRMProBANKDEPOSIT.BANKDEPOSIT_GetRecord(CO, TypeID)
                FRMProBANKDEPOSIT.BtnSave.Enabled = False
                FRMProBANKDEPOSIT.BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                'FRMProBANKDEPOSIT.BtnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                'FRMProBANKDEPOSIT.BtnEdit.Enabled = True
                FRMProBANKDEPOSIT.BtnPrint.Enabled = True
                FRMProBANKDEPOSIT.DISAPLEDCONTROLS()
                FRMProBANKDEPOSIT.BtnEdit.Caption = "استرجاع القيمة"
            End If
        End If
        Me.Close()
    End Sub

End Class