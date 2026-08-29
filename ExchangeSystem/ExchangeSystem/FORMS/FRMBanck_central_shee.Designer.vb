<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMBanck_central_shee
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMBanck_central_shee))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.REFERENCE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TYPE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.STATE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.FULL_NAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.PHONE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.IBAN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BANK_NAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CASH_PRICE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BANK_TRANSFER_PRICE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AMOUNT_REQUESTED = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.COST = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.btnImportExcel = New DevExpress.XtraEditors.SimpleButton()
        Me.btnImportExcel1 = New DevExpress.XtraEditors.SimpleButton()
        Me.TextEdit1 = New DevExpress.XtraEditors.DateEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem2 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.SplashScreenManager1 = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.ExchangeSystem.WaitForm3), True, True)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.btnImportExcel)
        Me.LayoutControl1.Controls.Add(Me.btnImportExcel1)
        Me.LayoutControl1.Controls.Add(Me.TextEdit1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1207, 519)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(16, 60)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1175, 399)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.REFERENCE, Me.TYPE, Me.STATE, Me.FULL_NAME, Me.PHONE, Me.IBAN, Me.BANK_NAME, Me.CASH_PRICE, Me.BANK_TRANSFER_PRICE, Me.AMOUNT_REQUESTED, Me.COST, Me.SN})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        '
        'REFERENCE
        '
        Me.REFERENCE.Caption = "رقم المعاملة"
        Me.REFERENCE.FieldName = "REFERENCE"
        Me.REFERENCE.Name = "REFERENCE"
        Me.REFERENCE.UnboundDataType = GetType(Integer)
        '
        'TYPE
        '
        Me.TYPE.Caption = "نوع المعاملة"
        Me.TYPE.FieldName = "TYPE"
        Me.TYPE.Name = "TYPE"
        Me.TYPE.Visible = True
        Me.TYPE.VisibleIndex = 4
        Me.TYPE.Width = 94
        '
        'STATE
        '
        Me.STATE.Caption = "الحالة"
        Me.STATE.FieldName = "STATE"
        Me.STATE.Name = "STATE"
        Me.STATE.Width = 87
        '
        'FULL_NAME
        '
        Me.FULL_NAME.Caption = "الاسم بالكامل"
        Me.FULL_NAME.FieldName = "FULL_NAME"
        Me.FULL_NAME.Name = "FULL_NAME"
        Me.FULL_NAME.Visible = True
        Me.FULL_NAME.VisibleIndex = 1
        Me.FULL_NAME.Width = 291
        '
        'PHONE
        '
        Me.PHONE.Caption = "الهاتف"
        Me.PHONE.FieldName = "PHONE"
        Me.PHONE.Name = "PHONE"
        Me.PHONE.Visible = True
        Me.PHONE.VisibleIndex = 3
        Me.PHONE.Width = 150
        '
        'IBAN
        '
        Me.IBAN.Caption = "رقم الحساب"
        Me.IBAN.FieldName = "IBAN"
        Me.IBAN.Name = "IBAN"
        Me.IBAN.Visible = True
        Me.IBAN.VisibleIndex = 2
        Me.IBAN.Width = 212
        '
        'BANK_NAME
        '
        Me.BANK_NAME.Caption = "المصرف"
        Me.BANK_NAME.FieldName = "BANK_NAME"
        Me.BANK_NAME.Name = "BANK_NAME"
        '
        'CASH_PRICE
        '
        Me.CASH_PRICE.Caption = "الشراء"
        Me.CASH_PRICE.FieldName = "CASH_PRICE"
        Me.CASH_PRICE.Name = "CASH_PRICE"
        Me.CASH_PRICE.Visible = True
        Me.CASH_PRICE.VisibleIndex = 6
        Me.CASH_PRICE.Width = 79
        '
        'BANK_TRANSFER_PRICE
        '
        Me.BANK_TRANSFER_PRICE.Caption = "البيع"
        Me.BANK_TRANSFER_PRICE.FieldName = "BANK_TRANSFER_PRICE"
        Me.BANK_TRANSFER_PRICE.Name = "BANK_TRANSFER_PRICE"
        Me.BANK_TRANSFER_PRICE.Visible = True
        Me.BANK_TRANSFER_PRICE.VisibleIndex = 8
        Me.BANK_TRANSFER_PRICE.Width = 74
        '
        'AMOUNT_REQUESTED
        '
        Me.AMOUNT_REQUESTED.Caption = "قيمة الدولار"
        Me.AMOUNT_REQUESTED.FieldName = "AMOUNT_REQUESTED"
        Me.AMOUNT_REQUESTED.Name = "AMOUNT_REQUESTED"
        Me.AMOUNT_REQUESTED.Visible = True
        Me.AMOUNT_REQUESTED.VisibleIndex = 5
        Me.AMOUNT_REQUESTED.Width = 79
        '
        'COST
        '
        Me.COST.Caption = "القيمة بالدينار"
        Me.COST.FieldName = "COST"
        Me.COST.Name = "COST"
        Me.COST.Visible = True
        Me.COST.VisibleIndex = 7
        Me.COST.Width = 102
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 62
        '
        'btnImportExcel
        '
        Me.btnImportExcel.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success
        Me.btnImportExcel.Appearance.Options.UseBackColor = True
        Me.btnImportExcel.ImageOptions.Image = CType(resources.GetObject("btnImportExcel.ImageOptions.Image"), System.Drawing.Image)
        Me.btnImportExcel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.btnImportExcel.Location = New System.Drawing.Point(16, 16)
        Me.btnImportExcel.Name = "btnImportExcel"
        Me.btnImportExcel.Size = New System.Drawing.Size(166, 38)
        Me.btnImportExcel.StyleController = Me.LayoutControl1
        Me.btnImportExcel.TabIndex = 2
        Me.btnImportExcel.Text = "تصدير ملفات اكسل"
        '
        'btnImportExcel1
        '
        Me.btnImportExcel1.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success
        Me.btnImportExcel1.Appearance.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportExcel1.Appearance.Options.UseBackColor = True
        Me.btnImportExcel1.Appearance.Options.UseFont = True
        Me.btnImportExcel1.ImageOptions.Image = CType(resources.GetObject("btnImportExcel1.ImageOptions.Image"), System.Drawing.Image)
        Me.btnImportExcel1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.btnImportExcel1.Location = New System.Drawing.Point(188, 16)
        Me.btnImportExcel1.Name = "btnImportExcel1"
        Me.btnImportExcel1.Size = New System.Drawing.Size(157, 38)
        Me.btnImportExcel1.StyleController = Me.LayoutControl1
        Me.btnImportExcel1.TabIndex = 2
        Me.btnImportExcel1.Text = "حفظ"
        '
        'TextEdit1
        '
        Me.TextEdit1.EditValue = Nothing
        Me.TextEdit1.Location = New System.Drawing.Point(351, 16)
        Me.TextEdit1.Name = "TextEdit1"
        Me.TextEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TextEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TextEdit1.Properties.DisplayFormat.FormatString = ""
        Me.TextEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.TextEdit1.Properties.EditFormat.FormatString = ""
        Me.TextEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.TextEdit1.Properties.MaskSettings.Set("mask", "d")
        Me.TextEdit1.Properties.UseMaskAsDisplayFormat = True
        Me.TextEdit1.Size = New System.Drawing.Size(796, 36)
        Me.TextEdit1.StyleController = Me.LayoutControl1
        Me.TextEdit1.TabIndex = 5
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.EmptySpaceItem2, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1207, 519)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1181, 405)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.btnImportExcel
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(172, 44)
        Me.LayoutControlItem2.TextVisible = False
        '
        'EmptySpaceItem2
        '
        Me.EmptySpaceItem2.Location = New System.Drawing.Point(0, 449)
        Me.EmptySpaceItem2.Name = "EmptySpaceItem2"
        Me.EmptySpaceItem2.Size = New System.Drawing.Size(1181, 44)
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnImportExcel1
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(172, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(163, 44)
        Me.LayoutControlItem3.Text = "LayoutControlItem2"
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.TextEdit1
        Me.LayoutControlItem4.Location = New System.Drawing.Point(335, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(846, 44)
        Me.LayoutControlItem4.Text = "التاريخ"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(28, 21)
        '
        'SplashScreenManager1
        '
        Me.SplashScreenManager1.ClosingDelay = 500
        '
        'FRMBanck_central_shee
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1207, 519)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "FRMBanck_central_shee"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "كشف مبيعات ليبيا المركزي"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnImportExcel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents btnImportExcel1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents REFERENCE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TYPE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents STATE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents FULL_NAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents PHONE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents IBAN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BANK_NAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CASH_PRICE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BANK_TRANSFER_PRICE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AMOUNT_REQUESTED As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents COST As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EmptySpaceItem2 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TextEdit1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager
End Class
