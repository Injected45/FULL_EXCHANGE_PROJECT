Imports System.Data.SqlClient
Imports DevExpress.XtraEditors

Public Class FrmCompanies
    Dim IsUpdate As Int16
    Sub NewRecord()
        New_Controlrs(Me)
        LoadCompaniesTree()
        IsUpdate = 0
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        LoadToControlar(EMPID, "EmployeeTb_LOADINTOLKP", "EMPNAME", "ID", Nothing)
        Dim PR(1) As SqlParameter
        PR(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
        PR(1) = New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = 0}
        LoadToControlar(ParentID, "Companies_Crud", "CompanyName", "ID", PR)
        BtnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        BtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        treelistFormat()
        Code.Text = GETIDMAX("Companies", "ID") + 1
    End Sub

    Private Sub FrmCompanies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnNew.PerformClick()
    End Sub

    Sub Insert()

        Try
            ' 🧩 التحقق من المدخلات الأساسية الإجبارية
            If Not ValidateControl(CompanyName, "اسم الشركة") Then Exit Sub
            If Not ValidateControl(TypeID, "نوع الشركة") Then Exit Sub
            If Not ValidateControl(MangerName, "اسم المدير") Then Exit Sub

            ' ⚙️ إعداد المعاملات (تم دمج كل معامل في سطر واحد لتجنب خطأ المترجم)
            Dim prm() As SqlParameter = {
              New SqlParameter("@Action", SqlDbType.Int) With {.Value = 1},
              New SqlParameter("@CompanyName", SqlDbType.NVarChar, 300) With {.Value = SafeToString(CompanyName.Text.Trim)},
              New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = If(TypeID.EditValue IsNot Nothing AndAlso Not IsDBNull(TypeID.EditValue), SafeToInt(TypeID.EditValue), DBNull.Value)},
              New SqlParameter("@ParentID", SqlDbType.Int) With {.Value = If(ParentID.EditValue IsNot Nothing AndAlso Not IsDBNull(ParentID.EditValue), SafeToInt(ParentID.EditValue), DBNull.Value)},
              New SqlParameter("@EmpID", SqlDbType.Int) With {.Value = If(EMPID.EditValue IsNot Nothing AndAlso Not IsDBNull(EMPID.EditValue), SafeToInt(EMPID.EditValue), DBNull.Value)},
              New SqlParameter("@ManagerName", SqlDbType.NVarChar, 250) With {.Value = If(String.IsNullOrWhiteSpace(MangerName.Text), DBNull.Value, SafeToString(MangerName.Text.Trim))},
              New SqlParameter("@LicenseNumber", SqlDbType.NVarChar, 150) With {.Value = If(String.IsNullOrWhiteSpace(LicenseNumber.Text), DBNull.Value, SafeToString(LicenseNumber.Text.Trim))},
              New SqlParameter("@TaxNumber", SqlDbType.NVarChar, 50) With {.Value = If(String.IsNullOrWhiteSpace(TaxID.Text), DBNull.Value, SafeToString(TaxID.Text.Trim))}
          }

            ' 📡 تنفيذ الإجراء المخزن
            RUN_QUARY_PRO_alter("dbo.Companies_Crud", prm)

            ' ✅ رسالة نجاح
            XtraMessageBox.Show("تم حفظ بيانات الشركة بنجاح.", "معلومة",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' 🔄 إعادة تهيئة النموذج
            NewRecord()

        Catch ex As SqlClient.SqlException

            ' اصطياد الأخطاء بدقة بناءً على حالة الـ SQL
            Select Case ex.State
                Case 1
                    XtraMessageBox.Show(ex.Message, "تنبيه",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case Else
                    XtraMessageBox.Show("خطأ قاعدة بيانات: " & ex.Message,
                                      "خطأ",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

        Catch ex As Exception
            ' اصطياد أي أخطاء أخرى في بيئة الـ VB.NET
            XtraMessageBox.Show("خطأ غير متوقع في النظام: " & ex.Message,
                              "خطأ",
                              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Public Overrides Sub Save()
        Insert()
        MyBase.Save()
    End Sub

    Private Sub EMPID_TextChanged(sender As Object, e As EventArgs) Handles EMPID.TextChanged
        MangerName.Text = EMPID.Text.ToString
    End Sub

    Public Overrides Sub BNew()
        NewRecord()
        MyBase.BNew()
    End Sub
    Private Sub LoadCompaniesTree()
        Try
            TreeList1.DataSource = Nothing
            Dim prm(0) As SqlParameter
            prm(0) = New SqlParameter("@Action", SqlDbType.Int) With {.Value = 0}
            Dim dt As DataTable = RUN_QUARY_PRO_alter("Companies_Crud", prm)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub
            TreeList1.DataSource = dt
            TreeList1.KeyFieldName = "ID"
            TreeList1.ParentFieldName = "ParentID"
            'TreeList1.ExpandAll()
            TreeList1.CollapseAll()
            ShowOnlyColumn(TreeList1, "CompanyName1")
        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message)
        End Try
    End Sub

    Sub treelistFormat()
        ' ----- المظهر العام -----
        TreeList1.Appearance.Row.BackColor = Color.White
        TreeList1.Appearance.Row.BackColor2 = Color.LightGray
        TreeList1.Appearance.FocusedRow.BackColor = Color.CornflowerBlue
        TreeList1.Appearance.SelectedRow.BackColor = Color.SkyBlue
        TreeList1.Appearance.SelectedRow.ForeColor = Color.White
        TreeList1.Appearance.Row.Font = New Font("Arial", 10, FontStyle.Regular)
        TreeList1.Appearance.HeaderPanel.Font = New Font("Arial", 11, FontStyle.Bold)

        ' خطوط شبكية
        TreeList1.OptionsView.ShowHorzLines = True
        TreeList1.OptionsView.ShowVertLines = True
        TreeList1.OptionsView.ShowIndicator = True
        TreeList1.OptionsView.ShowButtons = True
        TreeList1.OptionsView.ShowRoot = True

        ' البحث والسحب
        TreeList1.OptionsFind.AllowFindPanel = True
        TreeList1.OptionsFind.ShowFindButton = True

    End Sub

    Private Sub TreeList1_RowCellClick(sender As Object, e As DevExpress.XtraTreeList.RowCellClickEventArgs) Handles TreeList1.RowCellClick
        Try
            ' 🚨 حماية هندسية: التأكد من أن العقدة المحددة ليست فارغة لتجنب الـ NullReferenceException
            If TreeList1.FocusedNode Is Nothing Then Exit Sub

            ' ⚙️ تغيير حالة أزرار التحكم في الشاشة
            IsUpdate = 1
            BtnSave.Enabled = False
            BtnEdit.Enabled = True

            ' 🔒 قفل حقول الإدخال (تتبع نفس أسلوبك القديم لحين الضغط على تعديل)
            'CompanyName.Enabled = False
            'TypeID.Enabled = False
            'ParentID.Enabled = False
            'EMPID.Enabled = False
            'MangerName.Enabled = False
            'LicenseNumber.Enabled = False
            'TaxID.Enabled = False

            ' 📡 جلب البيانات من العقدة المحددة (FocusedNode) وتوزيعها على عناصر الشاشة
            Code.Text = SafeToString(TreeList1.FocusedNode.GetValue("ID"))
            CompanyName.Text = SafeToString(TreeList1.FocusedNode.GetValue("CompanyName"))

            ' جلب القيم الرقمية والمعرفات (LookUpEdit / GridLookUpEdit)
            TypeID.SelectedIndex = TreeList1.FocusedNode.GetValue("TypeID")
            ParentID.EditValue = TreeList1.FocusedNode.GetValue("ParentID")
            EMPID.EditValue = TreeList1.FocusedNode.GetValue("EmpID")

            ' جلب الحقول النصية الأخرى مع حمايتها بـ SafeToString
            MangerName.Text = SafeToString(TreeList1.FocusedNode.GetValue("ManagerName"))
            LicenseNumber.Text = SafeToString(TreeList1.FocusedNode.GetValue("LicenseNumber"))
            TaxID.Text = SafeToString(TreeList1.FocusedNode.GetValue("TaxNumber"))

        Catch ex As Exception
            XtraMessageBox.Show("حدث خطأ أثناء عرض بيانات الشركة: " & ex.Message, "خطأ",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Overrides Sub UPDATERECORD()
        Try
            ' 🧩 1. التحقق من المدخلات الأساسية والإلزامية للتعديل
            If Not ValidateControl(Code, "الرمز") Then Exit Sub
            If Not ValidateControl(CompanyName, "اسم الشركة") Then Exit Sub
            If Not ValidateControl(TypeID, "نوع الشركة") Then Exit Sub
            If Not ValidateControl(MangerName, "اسم المدير") Then Exit Sub

            ' ⚙️ 2. إعداد المعاملات (كل معامل في سطر واحد تماماً لضمان سلامة البناء)
            Dim prm() As SqlParameter = {
                New SqlParameter("@Action", SqlDbType.Int) With {.Value = 2},
                New SqlParameter("@ID", SqlDbType.Int) With {.Value = SafeToInt(Code.Text.Trim)},
                New SqlParameter("@CompanyName", SqlDbType.NVarChar, 300) With {.Value = SafeToString(CompanyName.Text.Trim)},
                New SqlParameter("@TypeID", SqlDbType.Int) With {.Value = SafeToInt(TypeID.SelectedIndex)},
                New SqlParameter("@ParentID", SqlDbType.Int) With {.Value = If(ParentID.EditValue IsNot Nothing AndAlso Not IsDBNull(ParentID.EditValue), SafeToInt(ParentID.EditValue), DBNull.Value)},
                New SqlParameter("@EmpID", SqlDbType.Int) With {.Value = If(EMPID.EditValue IsNot Nothing AndAlso Not IsDBNull(EMPID.EditValue), SafeToInt(EMPID.EditValue), DBNull.Value)},
                New SqlParameter("@ManagerName", SqlDbType.NVarChar, 250) With {.Value = If(String.IsNullOrWhiteSpace(MangerName.Text), DBNull.Value, SafeToString(MangerName.Text.Trim))},
                New SqlParameter("@LicenseNumber", SqlDbType.NVarChar, 150) With {.Value = If(String.IsNullOrWhiteSpace(LicenseNumber.Text), DBNull.Value, SafeToString(LicenseNumber.Text.Trim))},
                New SqlParameter("@TaxNumber", SqlDbType.NVarChar, 50) With {.Value = If(String.IsNullOrWhiteSpace(TaxID.Text), DBNull.Value, SafeToString(TaxID.Text.Trim))}
            }

            ' 📡 3. تنفيذ الإجراء المخزن للتعديل
            RUN_QUARY_PRO_alter("dbo.Companies_Crud", prm)

            ' ✅ 4. رسالة نجاح العملية
            XtraMessageBox.Show("تم تعديل بيانات الشركة بنجاح.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' 🔄 5. إعادة تهيئة النموذج لتجهيز الشاشة لإدخال جديد
            NewRecord()

        Catch ex As SqlClient.SqlException
            ' 🔍 اصطياد الأخطاء بناءً على الـ State المبرمج في الـ SQL (استخدمنا State = 1 في قيود الإجراء المخزن)
            Select Case ex.State
                Case 1 ' مثل رسالة: "اسم الشركة موجود مسبقاً" أو "لا يمكن أن تكون الشركة تابعة لنفسها"
                    XtraMessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case Else
                    XtraMessageBox.Show("خطأ قاعدة بيانات غير متوقع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

        Catch ex As Exception
            ' اصطياد أخطاء النظام العامة في الـ VB.NET
            XtraMessageBox.Show("خطأ غير متوقع في النظام: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        MyBase.UPDATERECORD()
    End Sub


End Class