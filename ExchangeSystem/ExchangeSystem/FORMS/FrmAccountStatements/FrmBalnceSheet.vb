Imports System.Data.SqlClient
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.Wizards
Imports DevExpress.XtraTreeList

Public Class FrmBalnceSheet
    Sub NewRecord()
        New_Controlrs(Me)
        LoadToControlar(branchID, "CoBranches_LoadToLKPWITHOUTAGENT", "BName", "DBRID", Nothing)
        branchID.EditValue = BID
    End Sub
    Private Sub FrmBalnceSheet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewRecord()
    End Sub



    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If Not ValidateControl(branchID, "الفرع") Then Exit Sub
        GridControl2.DataSource = Nothing
        GridControl21.DataSource = Nothing
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchID.EditValue}
        prm(1) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = 1}
        LoadToControlar(GridControl2, "BalanceSheet_Statment", "", "", prm)
        Dim prm1(1) As SqlParameter
        prm1(0) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = branchID.EditValue}
        prm1(1) = New SqlParameter("@Type", SqlDbType.Int) With {.Value = 2}
        LoadToControlar(GridControl21, "BalanceSheet_Statment", "", "", prm1)
        DVGFormat(GridView2)
        DVGFormat(GridView21)
        GroupGridView(GridView2, Sumcredit)
        GroupGridView(GridView21, SUMdibet)



    End Sub

    Sub GroupGridView(GView As GridView, SPN As SpinEdit)
        With GView

            ' التجميع حسب TypeName
            .ClearGrouping()
            .Columns("TypeName").GroupIndex = 0

            ' ترتيب
            .SortInfo.Clear()
            .SortInfo.Add(.Columns("TypeName"), DevExpress.Data.ColumnSortOrder.Ascending)

            ' إخفاء أعمدة
            .Columns("TypeID").Visible = False

            ' تنسيق الأعمدة
            .Columns("AccName").Caption = "الحساب"
            .Columns("Accval").Caption = "القيمة"

            ' تنسيق القيم
            .Columns("Accval").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            .Columns("Accval").DisplayFormat.FormatString = "n2"

        End With

        GView.GroupSummary.Clear()

        Dim groupSum2 As New DevExpress.XtraGrid.GridGroupSummaryItem()
        groupSum2.FieldName = "Accval"
        groupSum2.SummaryType = DevExpress.Data.SummaryItemType.Sum
        groupSum2.DisplayFormat = " = {0:n2}"
        groupSum2.ShowInGroupColumnFooter = GView.Columns("Accval")

        GView.GroupSummary.Add(groupSum2)


        GView.GroupSummary.Clear()

        Dim grpSum As New DevExpress.XtraGrid.GridGroupSummaryItem()
        grpSum.FieldName = "Accval"
        grpSum.SummaryType = DevExpress.Data.SummaryItemType.Sum

        ' هذا السطر هو المهم 👇
        grpSum.DisplayFormat = "  = {0:n2}"

        ' يظهر في عنوان الجروب نفسه
        grpSum.ShowInGroupColumnFooter = Nothing

        GView.GroupSummary.Add(grpSum)

        GView.Appearance.GroupRow.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Bold)
        GView.Appearance.GroupRow.ForeColor = Color.Black
        GridColumnSummaryItem_grivview(GView, "Accval", SPN)
    End Sub

    Private Sub branchID_EditValueChanged(sender As Object, e As EventArgs) Handles branchID.EditValueChanged
        GridControl2.DataSource = Nothing
        GridControl21.DataSource = Nothing
    End Sub

    Private Sub GridView2_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView2.ColumnFilterChanged
        GroupGridView(GridView2, Sumcredit)

    End Sub

    Private Sub GridView21_ColumnFilterChanged(sender As Object, e As EventArgs) Handles GridView21.ColumnFilterChanged
        GroupGridView(GridView21, SUMdibet)
    End Sub

    Private Sub GridView2_DoubleClick(sender As Object, e As EventArgs) Handles GridView2.DoubleClick
        Dim view As DevExpress.XtraGrid.Views.Grid.GridView =
       CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)

        'التأكد أن المستخدم ضغط على صف فعلي
        If view.FocusedRowHandle < 0 Then Exit Sub

        'جلب قيمة العمود
        Dim value As Object = view.GetRowCellValue(view.FocusedRowHandle, "AccParent")
        Dim value1 As Object = view.GetRowCellValue(view.FocusedRowHandle, "ParentName")
        Dim value2 As Object = view.GetRowCellValue(view.FocusedRowHandle, "AccID")

        FrmBalnceSheet_Detials.NewRecord(value)
        FrmBalnceSheet_Detials.Text = value1.ToString
        FrmBalnceSheet_Detials.AccID.EditValue = value2
        FrmBalnceSheet_Detials.ShowDialog()
    End Sub

    Private Sub GridView21_DoubleClick(sender As Object, e As EventArgs) Handles GridView21.DoubleClick
        Dim view As DevExpress.XtraGrid.Views.Grid.GridView =
CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)

        'التأكد أن المستخدم ضغط على صف فعلي
        If view.FocusedRowHandle < 0 Then Exit Sub

        'جلب قيمة العمود
        Dim value As Object = view.GetRowCellValue(view.FocusedRowHandle, "AccParent")
        Dim value1 As Object = view.GetRowCellValue(view.FocusedRowHandle, "ParentName")
        Dim value2 As Object = view.GetRowCellValue(view.FocusedRowHandle, "AccID")

        FrmBalnceSheet_Detials.NewRecord(value)
        FrmBalnceSheet_Detials.Text = value1.ToString
        FrmBalnceSheet_Detials.AccID.EditValue = value2
        FrmBalnceSheet_Detials.ShowDialog()
    End Sub
End Class