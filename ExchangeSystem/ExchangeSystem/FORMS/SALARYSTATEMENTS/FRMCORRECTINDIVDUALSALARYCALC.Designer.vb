<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMCORRECTINDIVDUALSALARYCALC
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions3 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject9 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject10 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject11 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject12 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.FRSP = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.SYEAR = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.CodeID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.EMPNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SALARYMONTH = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SALARYEAR = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SalaryVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ConstanceVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BONUSVAL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DiscountsVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AdvancePaymentDisc = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SALARYTOTAL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BTNPrint = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Print = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlGroup2 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.FRSP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SYEAR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Print, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FRSP
        '
        Me.FRSP.AutoHeight = False
        Me.FRSP.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinLeft, "", -1, True, False, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.FRSP.MaskSettings.Set("mask", "n3")
        Me.FRSP.MaskSettings.Set("hideInsignificantZeros", False)
        Me.FRSP.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.FRSP.Name = "FRSP"
        Me.FRSP.UseMaskAsDisplayFormat = True
        '
        'SYEAR
        '
        Me.SYEAR.AutoHeight = False
        Me.SYEAR.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinLeft, "", -1, True, False, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.SYEAR.MaskSettings.Set("mask", "n3")
        Me.SYEAR.MaskSettings.Set("hideInsignificantZeros", False)
        Me.SYEAR.MaskSettings.Set("autoHideDecimalSeparator", False)
        Me.SYEAR.Name = "SYEAR"
        Me.SYEAR.UseMaskAsDisplayFormat = True
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1346, 367)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GCRole.Location = New System.Drawing.Point(32, 59)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.Print})
        Me.GCRole.Size = New System.Drawing.Size(1282, 276)
        Me.GCRole.TabIndex = 6
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.CodeID, Me.EMPNAME, Me.BName, Me.SALARYMONTH, Me.SALARYEAR, Me.SalaryVal, Me.ConstanceVal, Me.BONUSVAL, Me.DiscountsVal, Me.AdvancePaymentDisc, Me.SALARYTOTAL, Me.BTNPrint})
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsFind.FindPanelLocation = DevExpress.XtraGrid.Views.Grid.GridFindPanelLocation.Panel
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'CodeID
        '
        Me.CodeID.Caption = "الرمز"
        Me.CodeID.FieldName = "CodeID"
        Me.CodeID.Name = "CodeID"
        Me.CodeID.Visible = True
        Me.CodeID.VisibleIndex = 0
        '
        'EMPNAME
        '
        Me.EMPNAME.Caption = "الموظف"
        Me.EMPNAME.FieldName = "EMPNAME"
        Me.EMPNAME.Name = "EMPNAME"
        Me.EMPNAME.Visible = True
        Me.EMPNAME.VisibleIndex = 1
        '
        'BName
        '
        Me.BName.Caption = "الفرع"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 2
        '
        'SALARYMONTH
        '
        Me.SALARYMONTH.Caption = "الشهر"
        Me.SALARYMONTH.FieldName = "SALARYMONTH"
        Me.SALARYMONTH.Name = "SALARYMONTH"
        Me.SALARYMONTH.Visible = True
        Me.SALARYMONTH.VisibleIndex = 3
        '
        'SALARYEAR
        '
        Me.SALARYEAR.Caption = "السنة"
        Me.SALARYEAR.FieldName = "SALARYEAR"
        Me.SALARYEAR.Name = "SALARYEAR"
        Me.SALARYEAR.Visible = True
        Me.SALARYEAR.VisibleIndex = 4
        '
        'SalaryVal
        '
        Me.SalaryVal.Caption = "الراتب"
        Me.SalaryVal.FieldName = "SalaryVal"
        Me.SalaryVal.Name = "SalaryVal"
        Me.SalaryVal.Visible = True
        Me.SalaryVal.VisibleIndex = 5
        '
        'ConstanceVal
        '
        Me.ConstanceVal.Caption = "علاوات ثابتة"
        Me.ConstanceVal.FieldName = "ConstanceVal"
        Me.ConstanceVal.Name = "ConstanceVal"
        Me.ConstanceVal.Visible = True
        Me.ConstanceVal.VisibleIndex = 6
        '
        'BONUSVAL
        '
        Me.BONUSVAL.Caption = "علاوات مؤقتة"
        Me.BONUSVAL.FieldName = "BONUSVAL"
        Me.BONUSVAL.Name = "BONUSVAL"
        Me.BONUSVAL.Visible = True
        Me.BONUSVAL.VisibleIndex = 7
        '
        'DiscountsVal
        '
        Me.DiscountsVal.Caption = "الخصميات"
        Me.DiscountsVal.FieldName = "DiscountsVal"
        Me.DiscountsVal.Name = "DiscountsVal"
        Me.DiscountsVal.Visible = True
        Me.DiscountsVal.VisibleIndex = 8
        '
        'AdvancePaymentDisc
        '
        Me.AdvancePaymentDisc.Caption = "خصم سلفة"
        Me.AdvancePaymentDisc.FieldName = "AdvancePaymentDisc"
        Me.AdvancePaymentDisc.Name = "AdvancePaymentDisc"
        Me.AdvancePaymentDisc.Visible = True
        Me.AdvancePaymentDisc.VisibleIndex = 9
        '
        'SALARYTOTAL
        '
        Me.SALARYTOTAL.Caption = "الصافي"
        Me.SALARYTOTAL.FieldName = "SALARYTOTAL"
        Me.SALARYTOTAL.Name = "SALARYTOTAL"
        Me.SALARYTOTAL.Visible = True
        Me.SALARYTOTAL.VisibleIndex = 10
        '
        'BTNPrint
        '
        Me.BTNPrint.Caption = "طباعة"
        Me.BTNPrint.ColumnEdit = Me.Print
        Me.BTNPrint.FieldName = "BTNPrint"
        Me.BTNPrint.Name = "BTNPrint"
        Me.BTNPrint.Visible = True
        Me.BTNPrint.VisibleIndex = 11
        '
        'Print
        '
        Me.Print.AutoHeight = False
        EditorButtonImageOptions3.SvgImage = Global.ExchangeSystem.My.Resources.Resources.printer
        EditorButtonImageOptions3.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.Print.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions3, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject9, SerializableAppearanceObject10, SerializableAppearanceObject11, SerializableAppearanceObject12, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.Print.Name = "Print"
        Me.Print.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlGroup2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1346, 367)
        Me.Root.TextVisible = False
        '
        'LayoutControlGroup2
        '
        Me.LayoutControlGroup2.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger
        Me.LayoutControlGroup2.AppearanceGroup.Options.UseBorderColor = True
        Me.LayoutControlGroup2.CustomizationFormText = "القيمة السابقة"
        Me.LayoutControlGroup2.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3})
        Me.LayoutControlGroup2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup2.Name = "LayoutControlGroup2"
        Me.LayoutControlGroup2.OptionsItemText.TextToControlDistance = 6
        Me.LayoutControlGroup2.Size = New System.Drawing.Size(1320, 341)
        Me.LayoutControlGroup2.Text = "بيانات سابقة"
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GCRole
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(1288, 282)
        Me.LayoutControlItem3.Text = "LayoutControlItem1"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'FRMCORRECTINDIVDUALSALARYCALC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1346, 367)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "FRMCORRECTINDIVDUALSALARYCALC"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "تعديل إخلاء طرف"
        CType(Me.FRSP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SYEAR, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Print, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents FRSP As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents SYEAR As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup2 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CodeID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EMPNAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SALARYMONTH As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SALARYEAR As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SalaryVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ConstanceVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BONUSVAL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DiscountsVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AdvancePaymentDisc As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SALARYTOTAL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BTNPrint As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Print As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
End Class
