Imports DevExpress.Xpo.DB.Helpers
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors
Imports Newtonsoft.Json
Imports RestSharp
Imports System.Data.SqlClient
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Web
Imports Method = RestSharp.Method
Imports DevExpress.XtraGrid
Imports DevExpress.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraReports.UI
Imports System.Reflection
Imports DevExpress.XtraGrid.Views.Base

Module puplec_controlls_standers

    ''' <summary>
    ''' Safely turn a DataTable/DataRow value into a String for assignment to a .Text property.
    ''' </summary>
    ''' <remarks>
    ''' Assigning a NULL column straight into .Text throws
    '''     "Conversion from type 'DBNull' to type 'String' is not valid"
    ''' which surfaces as the generic support dialog and takes the whole screen down.
    '''
    ''' This is worth a shared helper rather than a one-off fix: a column that was never NULL on SQL Server
    ''' can be NULL here whenever a lookup finds no row, and every screen that writes a query result into
    ''' .Text is exposed to it. Returns "" for NULL/DBNull so the control simply shows nothing.
    ''' </remarks>
    Public Function NullSafeText(v As Object) As String
        If v Is Nothing OrElse TypeOf v Is DBNull Then Return String.Empty
        Return Convert.ToString(v)
    End Function

#Region "Load To LKP"
    Public Sub HandleError(message As String, ex As Exception)
        XtraMessageBox.Show($"{message}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    ''التحقق من الحقل ليس فارغ وارجاع رسالة
    Public Function ValidateControl(ctrl As Control, fieldName As String) As Boolean
        Dim isEmpty As Boolean = False

        If TypeOf ctrl Is DevExpress.XtraEditors.TextEdit Then
            isEmpty = String.IsNullOrWhiteSpace(DirectCast(ctrl, DevExpress.XtraEditors.TextEdit).Text)

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.ComboBoxEdit Then
            isEmpty = String.IsNullOrWhiteSpace(DirectCast(ctrl, DevExpress.XtraEditors.ComboBoxEdit).Text)

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.LookUpEdit Then
            isEmpty = DirectCast(ctrl, DevExpress.XtraEditors.LookUpEdit).EditValue Is Nothing

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.MemoEdit Then
            isEmpty = String.IsNullOrWhiteSpace(DirectCast(ctrl, DevExpress.XtraEditors.MemoEdit).Text)

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.DateEdit Then
            isEmpty = DirectCast(ctrl, DevExpress.XtraEditors.DateEdit).EditValue Is Nothing
        End If

        If isEmpty Then
            DevExpress.XtraEditors.XtraMessageBox.Show(
        "الحقل [" & fieldName & "] لا يمكن أن يكون فارغ",
        "تنبيه",
        MessageBoxButtons.OK,
        MessageBoxIcon.Warning
    )
            ctrl.Focus()
            Return False
        End If

        Return True


    End Function

    ''التأكد من الحقل ليس فارغ
    Public Function IsEmpty(ctrl As Control) As Boolean
        ' للتحقق من أدوات DevExpress
        If TypeOf ctrl Is DevExpress.XtraEditors.TextEdit Then
            Return String.IsNullOrWhiteSpace(DirectCast(ctrl, DevExpress.XtraEditors.TextEdit).Text)

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.ComboBoxEdit Then
            Return String.IsNullOrWhiteSpace(DirectCast(ctrl, DevExpress.XtraEditors.ComboBoxEdit).Text)

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.LookUpEdit Then
            Return DirectCast(ctrl, DevExpress.XtraEditors.LookUpEdit).EditValue Is Nothing

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.MemoEdit Then
            Return String.IsNullOrWhiteSpace(DirectCast(ctrl, DevExpress.XtraEditors.MemoEdit).Text)

        ElseIf TypeOf ctrl Is DevExpress.XtraEditors.DateEdit Then
            Return DirectCast(ctrl, DevExpress.XtraEditors.DateEdit).EditValue Is Nothing

        Else
            ' لو الأداة مش معرفة هنا نرجع False
            Return False
        End If
    End Function
    '' تحميل البيانات للأدوات حسب نوع الأداة
    Public Sub LoadToControlar(ctrl As Object, query As String, displayMember As String, valueMember As String, PrmType() As SqlParameter, Optional includeAllRow As Boolean = False, Optional includePublic As String = "الكل")
        Try
            ' إعداد البراميتر

            ' تنفيذ الاستعلام
            Dim dt As New DataTable
            dt.Clear()
            dt = RUN_QUARY_PRO_alter(query, PrmType)

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

            ' LookUpEdit
            Dim lkp = TryCast(ctrl, DevExpress.XtraEditors.LookUpEdit)

            If lkp IsNot Nothing Then
                With lkp.Properties
                    .DataSource = Nothing
                    .DataSource = dt
                    .DisplayMember = displayMember
                    .ValueMember = valueMember
                    .ShowHeader = False
                    If includeAllRow Then
                        If dt.Columns.Contains(valueMember) AndAlso dt.Columns.Contains(displayMember) Then
                            Dim newRow As DataRow = dt.NewRow()
                            newRow(valueMember) = 0
                            newRow(displayMember) = includePublic
                            dt.Rows.InsertAt(newRow, 0)
                        End If
                    End If
                    HideAllColumnsExceptDisplay(lkp)
                    dt.Dispose()
                End With
                Return
            End If

            ' GridLookUpEdit
            Dim gridLkp = TryCast(ctrl, DevExpress.XtraEditors.GridLookUpEdit)
            If gridLkp IsNot Nothing Then
                With gridLkp.Properties
                    .DataSource = Nothing
                    .DataSource = dt
                    .DisplayMember = displayMember
                    .ValueMember = valueMember
                End With
                Return
            End If

            ' GridControl
            Dim grid = TryCast(ctrl, GridControl)
            If grid IsNot Nothing Then
                grid.DataSource = Nothing
                grid.DataSource = dt
                Return
            End If

            ' *************** RepositoryItemLookUpEdit *******************
            Dim repoLkp = TryCast(ctrl, DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit)
            If repoLkp IsNot Nothing Then
                With repoLkp
                    .DataSource = Nothing
                    .DataSource = dt
                    .DisplayMember = displayMember
                    .ValueMember = valueMember
                    .ShowHeader = False

                    If includeAllRow AndAlso dt.Columns.Contains(valueMember) AndAlso dt.Columns.Contains(displayMember) Then
                        Dim newRow As DataRow = dt.NewRow()
                        newRow(valueMember) = 0
                        newRow(displayMember) = includePublic
                        dt.Rows.InsertAt(newRow, 0)
                    End If
                End With
                Return
                HideAllColumnsExceptDisplay(repoLkp)
            End If

            'أدوات أخرى يمكن إضافتها هنا...

        Catch ex As Exception
            HandleError("خطأ في تحميل البيانات", ex)
        End Try
    End Sub

    '' إخفاء جميع الحقول في التري ليست ما عدا حقل واحد
    Public Sub ShowOnlyColumn(tree As DevExpress.XtraTreeList.TreeList, fieldNameToShow As String)
        If tree Is Nothing Then Return

        For Each col As DevExpress.XtraTreeList.Columns.TreeListColumn In tree.Columns
            If col.FieldName = fieldNameToShow Then
                col.Visible = True
            Else
                col.Visible = False
            End If
        Next
    End Sub

    '' إخفاء جميع الأعمدة ماعدا عامود العرض
    Public Sub HideAllColumnsExceptDisplay(ByVal ctrl As Object)
        Try
            ' معالجة LookUpEdit
            Dim lkp = TryCast(ctrl, DevExpress.XtraEditors.LookUpEdit)
            If lkp IsNot Nothing Then
                If lkp.Properties.DisplayMember = String.Empty Then Exit Sub

                lkp.Properties.PopulateColumns()
                Dim displayColumn As String = lkp.Properties.DisplayMember

                For Each col As DevExpress.XtraEditors.Controls.LookUpColumnInfo In lkp.Properties.Columns
                    col.Visible = (col.FieldName = displayColumn)
                Next
                Return
            End If

            ' معالجة RepositoryItemLookUpEdit
            Dim repoLkp = TryCast(ctrl, DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit)
            If repoLkp IsNot Nothing Then
                If repoLkp.DisplayMember = String.Empty Then Exit Sub

                repoLkp.PopulateColumns()
                Dim displayColumn As String = repoLkp.DisplayMember

                For Each col As DevExpress.XtraEditors.Controls.LookUpColumnInfo In repoLkp.Columns
                    col.Visible = (col.FieldName = displayColumn)
                Next
                Return
            End If

        Catch ex As Exception
            HandleError("خطأ في تهيئة الأعمدة", ex)
        End Try
    End Sub
    '' إخفاء جميع الأعمدة ماعدا عامود العرض وعامود القيمة
    Public Sub HideAllColumnsExceptDisplayAndVAl(ByVal lkp As LookUpEdit)
        Try
            If lkp Is Nothing OrElse lkp.Properties.DisplayMember = String.Empty Then Exit Sub

            ' توليد الأعمدة
            lkp.Properties.PopulateColumns()

            ' الاحتفاظ فقط بعمود العرض
            Dim displayColumn As String = lkp.Properties.DisplayMember
            Dim ValColumn As String = lkp.Properties.ValueMember
            For Each col As DevExpress.XtraEditors.Controls.LookUpColumnInfo In lkp.Properties.Columns
                col.Visible = (col.FieldName = displayColumn OrElse col.FieldName = ValColumn)
            Next
        Catch ex As Exception
            HandleError("خطأ في تهيئة الأعمدة", ex)
        End Try
    End Sub
    '' تنظيف جميع الأدوات في الشاشة
    Public Sub New_Controlrs(ByVal parent As Control)
        For Each ctrl As Control In parent.Controls
            Select Case True
                Case TypeOf ctrl Is TextBox
                    CType(ctrl, TextBox).Clear()

                Case TypeOf ctrl Is DevExpress.XtraEditors.TextEdit
                    CType(ctrl, DevExpress.XtraEditors.TextEdit).EditValue = Nothing

                Case TypeOf ctrl Is ComboBox
                    CType(ctrl, ComboBox).SelectedIndex = -1

                Case TypeOf ctrl Is DevExpress.XtraEditors.ComboBoxEdit
                    CType(ctrl, DevExpress.XtraEditors.ComboBoxEdit).SelectedIndex = -1

                Case TypeOf ctrl Is DevExpress.XtraEditors.LookUpEdit
                    CType(ctrl, DevExpress.XtraEditors.LookUpEdit).EditValue = -1

                Case TypeOf ctrl Is CheckBox
                    CType(ctrl, CheckBox).Checked = False

                Case TypeOf ctrl Is DevExpress.XtraEditors.CheckEdit
                    CType(ctrl, DevExpress.XtraEditors.CheckEdit).Checked = False

                Case TypeOf ctrl Is RadioButton
                    CType(ctrl, RadioButton).Checked = False

                Case TypeOf ctrl Is ToggleSwitch
                    CType(ctrl, ToggleSwitch).EditValue = True

                Case TypeOf ctrl Is DevExpress.XtraEditors.DateEdit
                    CType(ctrl, DevExpress.XtraEditors.DateEdit).EditValue = Date.Now

                Case TypeOf ctrl Is RichTextBox
                    CType(ctrl, RichTextBox).Clear()

                Case TypeOf ctrl Is NumericUpDown
                    CType(ctrl, NumericUpDown).Value = CType(ctrl, NumericUpDown).Minimum
                Case TypeOf ctrl Is GridControl
                    CType(ctrl, GridControl).DataSource = Nothing

                Case TypeOf ctrl Is GridLookUpEdit
                    CType(ctrl, GridLookUpEdit).EditValue = -1

                Case TypeOf ctrl Is ListBox
                    CType(ctrl, ListBox).DataSource = Nothing
            End Select

            ' استدعاء ذاتي إذا كانت الأداة تحتوي على أدوات داخلية (مثل GroupBox أو Panel...)
            If ctrl.HasChildren Then
                New_Controlrs(ctrl)
            End If
        Next
    End Sub
    '' قفل وفتح جميع الأدوات في الشاشة حسب المتغير IsEnabled
    Public Sub Enable_Controls(ByVal container As Control, ByVal isEnabled As Boolean)
        For Each ctrl As Control In container.Controls
            ' التعامل مع الحاويات الداخلية مثل Panel أو GroupBox
            If ctrl.HasChildren Then
                Enable_Controls(ctrl, isEnabled)
            End If

            If ShouldEnable(ctrl) Then
                ctrl.Enabled = isEnabled
            End If
        Next
    End Sub
    Private Function ShouldEnable(ctrl As Control) As Boolean
        Return TypeOf ctrl Is TextBox _
            OrElse TypeOf ctrl Is DevExpress.XtraEditors.TextEdit _
            OrElse TypeOf ctrl Is ComboBox _
            OrElse TypeOf ctrl Is DevExpress.XtraEditors.ComboBoxEdit _
            OrElse TypeOf ctrl Is DevExpress.XtraEditors.LookUpEdit _
            OrElse TypeOf ctrl Is CheckBox _
            OrElse TypeOf ctrl Is DevExpress.XtraEditors.CheckEdit _
            OrElse TypeOf ctrl Is RadioButton _
            OrElse TypeOf ctrl Is DevExpress.XtraEditors.DateEdit _
            OrElse TypeOf ctrl Is RichTextBox _
            OrElse TypeOf ctrl Is NumericUpDown _
            OrElse TypeOf ctrl Is SimpleButton _
            OrElse TypeOf ctrl Is GridLookUpEdit
    End Function

    '' كود جمع الاعمدة في داتاا لقريد فيو 
    Public Sub GridColumnSummaryItem_grivview(GridVie As GridView, filD_Name As String, Name_cat As Object)
        Try
            ' تحقق من القريد
            If GridVie Is Nothing Then
                Exit Sub
            End If

            ' تحقق من العمود
            Dim col = GridVie.Columns.ColumnByFieldName(filD_Name)
            If col Is Nothing Then
                Exit Sub
            End If

            ' إنشاء الملخص
            Dim SUMDRiverSHEr As New GridColumnSummaryItem()
            SUMDRiverSHEr.SummaryType = SummaryItemType.Sum
            SUMDRiverSHEr.FieldName = filD_Name
            col.Summary.Add(SUMDRiverSHEr)

            GridVie.OptionsView.ShowFooter = False
            Name_cat.Text = Format((col.SummaryItem.SummaryValue), "N3")

            ' إزالة أي ملخصات قديمة
            For Each column As GridColumn In GridVie.Columns
                Dim item As GridSummaryItem = column.SummaryItem
                If item IsNot Nothing Then
                    column.Summary.Remove(item)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''دالة لجلب قيمة عامود من الأداة LookUpEdit
    Public Function GetLKPColumnVal(LKP As LookUpEdit, columnName As String) As Object
        If LKP.EditValue Is Nothing Then
            Return Nothing
        End If

        Dim row As DataRowView = TryCast(LKP.Properties.GetDataSourceRowByKeyValue(LKP.EditValue), DataRowView)

        If row IsNot Nothing AndAlso row.Row.Table.Columns.Contains(columnName) Then
            Return row(columnName)
        End If

        Return Nothing
    End Function
    ''إجراء الطباعة
    Public Sub LoadToPrint(query As String, DataMember As String, reportName As String, PrmType() As SqlParameter, Optional filter As String = "")
        Try
            ' تنفيذ الاستعلام
            Dim dt As DataTable = RUN_QUARY_PRO_alter(query, PrmType)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

            ' البحث عن نوع التقرير
            Dim asm As Assembly = Assembly.GetExecutingAssembly()
            Dim reportType As Type = asm.GetTypes().FirstOrDefault(Function(t) t.Name.Equals(reportName, StringComparison.OrdinalIgnoreCase))
            If reportType Is Nothing Then
                Throw New Exception("التقرير '" & reportName & "' غير موجود في المشروع.")
            End If

            ' إنشاء التقرير ديناميكيًا
            Dim report As XtraReport = CType(Activator.CreateInstance(reportType), XtraReport)

            ' ربط البيانات
            report.DataSource = dt
            report.DataMember = DataMember

            ' تطبيق الفلترة لو فيه شرط
            If Not String.IsNullOrEmpty(filter) Then
                report.FilterString = filter
            End If

            ' عرض التقرير
            Dim tool As New ReportPrintTool(report)
            report.CreateDocument()
            report.ShowPreview()
        Catch ex As Exception
            HandleError("خطأ في تحميل البيانات", ex)
        End Try
    End Sub
    Private rowNumbersDict As New Dictionary(Of Object, Dictionary(Of Integer, Integer))
    Public Sub AddRowNumberColumnWithFilter(gridView As DevExpress.XtraGrid.Views.Grid.GridView)
        If gridView.Columns.ColumnByFieldName("SN") Is Nothing Then
            Dim col As New DevExpress.XtraGrid.Columns.GridColumn()
            col.Caption = "#"
            col.FieldName = "SN"
            col.Visible = True
            col.VisibleIndex = 0
            col.OptionsColumn.AllowEdit = False
            col.OptionsColumn.ReadOnly = True
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer
            gridView.Columns.Insert(0, col)
        End If
        Dim gridKey As Object = gridView
        RemoveHandler gridView.ColumnFilterChanged, AddressOf OnColumnFilterChanged
        RemoveHandler gridView.CustomColumnDisplayText, AddressOf OnCustomColumnDisplayText
        If rowNumbersDict.ContainsKey(gridKey) Then
            rowNumbersDict.Remove(gridKey)
        End If
        AddHandler gridView.ColumnFilterChanged, AddressOf OnColumnFilterChanged
        AddHandler gridView.CustomColumnDisplayText, AddressOf OnCustomColumnDisplayText
        UpdateRowNumbers(gridView)
    End Sub
    Private Sub OnColumnFilterChanged(sender As Object, e As EventArgs)
        Dim gridView = CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        UpdateRowNumbers(gridView)
    End Sub

    Private Sub OnCustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs)
        If e.Column.FieldName = "SN" AndAlso e.ListSourceRowIndex >= 0 Then
            Dim gridView = CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)
            Dim handle = gridView.GetRowHandle(e.ListSourceRowIndex)
            Dim gridKey As Object = gridView

            If rowNumbersDict.ContainsKey(gridKey) AndAlso rowNumbersDict(gridKey).ContainsKey(handle) Then
                e.DisplayText = rowNumbersDict(gridKey)(handle).ToString()
            Else
                e.DisplayText = (e.ListSourceRowIndex + 1).ToString() '
            End If
        End If
    End Sub

    Private Sub UpdateRowNumbers(gridView As DevExpress.XtraGrid.Views.Grid.GridView)
        Dim gridKey As Object = gridView
        Dim numbers As New Dictionary(Of Integer, Integer)

        For i As Integer = 0 To gridView.DataRowCount - 1
            Dim visibleIndex = i
            Dim handle = gridView.GetDataRowHandleByGroupRowHandle(i)
            If handle >= 0 Then
                numbers(handle) = visibleIndex + 1
            End If
        Next

        rowNumbersDict(gridKey) = numbers
        gridView.RefreshData()
    End Sub
    '' إضافة عامود للتسلسل
    Public Sub AddSerialColumn(gridView As GridView)
        ' تحقق من عدم وجود العمود مسبقاً
        If gridView.Columns.ColumnByFieldName("SN") Is Nothing Then
            Dim serialColumn As New GridColumn With {
                .FieldName = "SN",
                .Caption = "#",
                .UnboundType = UnboundColumnType.Integer,
                .Visible = True,
                .VisibleIndex = 0,
                .Width = 50
            }

            gridView.Columns.Insert(0, serialColumn)
        End If

        ' منع تكرار ربط الحدث
        RemoveHandler gridView.CustomColumnDisplayText, AddressOf Serial_CustomColumnDisplayText
        AddHandler gridView.CustomColumnDisplayText, AddressOf Serial_CustomColumnDisplayText
    End Sub


    Public Sub Serial_CustomColumnDisplayText(sender As Object, e As CustomColumnDisplayTextEventArgs)
        If e.Column.FieldName = "SN" AndAlso e.ListSourceRowIndex >= 0 Then
            e.DisplayText = (e.ListSourceRowIndex + 1).ToString()
        End If
    End Sub


#End Region

#Region "دوال لحماية النظام عند ارجاع قيمة فارغة من قاعدة البيانات"
    ' ترجّع نص أو "" لو القيمة NULL
    Public Function SafeToString(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then
            Return String.Empty
        End If
        Return value.ToString().Trim()
    End Function

    ' ترجّع عدد صحيح أو 0 لو القيمة NULL أو مش رقم
    Public Function SafeToInt(value As Object) As Integer
        If value Is Nothing OrElse IsDBNull(value) Then
            Return 0
        End If
        Dim result As Integer
        If Integer.TryParse(value.ToString(), result) Then
            Return result
        End If
        Return 0
    End Function

    ' ترجّع رقم عشري أو 0.0 لو القيمة NULL أو مش رقم
    Public Function SafeToDecimal(value As Object) As Decimal
        If value Is Nothing OrElse IsDBNull(value) Then
            Return 0D
        End If
        Dim result As Decimal
        If Decimal.TryParse(value.ToString(), result) Then
            Return result
        End If
        Return 0D
    End Function

    ' ترجّع قيمة Boolean (True/False) بشكل آمن
    Public Function SafeToBool(value As Object) As Boolean
        If value Is Nothing OrElse IsDBNull(value) Then
            Return False
        End If
        Dim result As Boolean
        If Boolean.TryParse(value.ToString(), result) Then
            Return result
        End If
        Return False
    End Function
#End Region
End Module
