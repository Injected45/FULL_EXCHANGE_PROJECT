<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class USRCurrencyMovements
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TYPE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ISID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyFrom = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CurrencyTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BuyPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SalePrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RetBuyPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.retSalePrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Ueserinsert = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DateForTime = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TypeMovet = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SafeID = New DevExpress.XtraEditors.LookUpEdit()
        Me.TextEdit5 = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.BanckID = New DevExpress.XtraEditors.LookUpEdit()
        Me.TYPElock = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.DateEdit2 = New DevExpress.XtraEditors.DateEdit()
        Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem6 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem7 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem8 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SafeID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit5.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BanckID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TYPElock.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton1)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton2)
        Me.LayoutControl1.Controls.Add(Me.SafeID)
        Me.LayoutControl1.Controls.Add(Me.TextEdit5)
        Me.LayoutControl1.Controls.Add(Me.BanckID)
        Me.LayoutControl1.Controls.Add(Me.TYPElock)
        Me.LayoutControl1.Controls.Add(Me.DateEdit2)
        Me.LayoutControl1.Controls.Add(Me.DateEdit1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1108, 598)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(16, 99)
        Me.GridControl1.MainView = Me.GVRole
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1076, 484)
        Me.GridControl1.TabIndex = 12
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.TYPE, Me.ISID, Me.CurrencyFrom, Me.CurrencyTo, Me.BuyPrice, Me.SalePrice, Me.RetBuyPrice, Me.retSalePrice, Me.Ueserinsert, Me.InsertDate, Me.DateForTime, Me.TypeMovet})
        Me.GVRole.DetailHeight = 334
        Me.GVRole.GridControl = Me.GridControl1
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        '
        'TYPE
        '
        Me.TYPE.Caption = "نوع العملية"
        Me.TYPE.FieldName = "TYPE"
        Me.TYPE.Name = "TYPE"
        Me.TYPE.Visible = True
        Me.TYPE.VisibleIndex = 1
        '
        'ISID
        '
        Me.ISID.Caption = "أسم المصرف"
        Me.ISID.FieldName = "ISID"
        Me.ISID.Name = "ISID"
        Me.ISID.Visible = True
        Me.ISID.VisibleIndex = 2
        '
        'CurrencyFrom
        '
        Me.CurrencyFrom.Caption = "عملة المحلية"
        Me.CurrencyFrom.FieldName = "CurrencyFrom"
        Me.CurrencyFrom.Name = "CurrencyFrom"
        Me.CurrencyFrom.Visible = True
        Me.CurrencyFrom.VisibleIndex = 3
        '
        'CurrencyTo
        '
        Me.CurrencyTo.Caption = "العملة الاجنبية"
        Me.CurrencyTo.FieldName = "CurrencyTo"
        Me.CurrencyTo.Name = "CurrencyTo"
        Me.CurrencyTo.Visible = True
        Me.CurrencyTo.VisibleIndex = 4
        '
        'BuyPrice
        '
        Me.BuyPrice.Caption = "سعر البيع"
        Me.BuyPrice.FieldName = "BuyPrice"
        Me.BuyPrice.Name = "BuyPrice"
        Me.BuyPrice.Visible = True
        Me.BuyPrice.VisibleIndex = 6
        '
        'SalePrice
        '
        Me.SalePrice.Caption = "سعر الشراء"
        Me.SalePrice.FieldName = "SalePrice"
        Me.SalePrice.Name = "SalePrice"
        Me.SalePrice.Visible = True
        Me.SalePrice.VisibleIndex = 7
        '
        'RetBuyPrice
        '
        Me.RetBuyPrice.Caption = "نسبة البيع"
        Me.RetBuyPrice.FieldName = "RetBuyPrice"
        Me.RetBuyPrice.Name = "RetBuyPrice"
        Me.RetBuyPrice.Visible = True
        Me.RetBuyPrice.VisibleIndex = 8
        '
        'retSalePrice
        '
        Me.retSalePrice.Caption = "نسبه الشراء"
        Me.retSalePrice.FieldName = "retSalePrice"
        Me.retSalePrice.Name = "retSalePrice"
        Me.retSalePrice.Visible = True
        Me.retSalePrice.VisibleIndex = 9
        '
        'Ueserinsert
        '
        Me.Ueserinsert.Caption = "اسم المستخدم"
        Me.Ueserinsert.FieldName = "Ueserinsert"
        Me.Ueserinsert.Name = "Ueserinsert"
        Me.Ueserinsert.Visible = True
        Me.Ueserinsert.VisibleIndex = 10
        '
        'InsertDate
        '
        Me.InsertDate.Caption = "تاريخ العملية"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 11
        '
        'DateForTime
        '
        Me.DateForTime.Caption = "وقت العملية"
        Me.DateForTime.FieldName = "DateForTime"
        Me.DateForTime.Name = "DateForTime"
        Me.DateForTime.Visible = True
        Me.DateForTime.VisibleIndex = 12
        '
        'TypeMovet
        '
        Me.TypeMovet.Caption = "نوع الحركة"
        Me.TypeMovet.FieldName = "TypeMovet"
        Me.TypeMovet.Name = "TypeMovet"
        Me.TypeMovet.Visible = True
        Me.TypeMovet.VisibleIndex = 5
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.BackColor = System.Drawing.Color.Red
        Me.SimpleButton1.Appearance.Options.UseBackColor = True
        Me.SimpleButton1.Location = New System.Drawing.Point(16, 57)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(159, 28)
        Me.SimpleButton1.StyleController = Me.LayoutControl1
        Me.SimpleButton1.TabIndex = 10
        Me.SimpleButton1.Text = "طباعة"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Appearance.BackColor = System.Drawing.Color.Black
        Me.SimpleButton2.Appearance.Options.UseBackColor = True
        Me.SimpleButton2.Location = New System.Drawing.Point(218, 57)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Padding = New System.Windows.Forms.Padding(50, 0, 20, 0)
        Me.SimpleButton2.Size = New System.Drawing.Size(208, 28)
        Me.SimpleButton2.StyleController = Me.LayoutControl1
        Me.SimpleButton2.TabIndex = 11
        Me.SimpleButton2.Text = "عرض"
        '
        'SafeID
        '
        Me.SafeID.Location = New System.Drawing.Point(469, 15)
        Me.SafeID.Name = "SafeID"
        Me.SafeID.Properties.Appearance.Options.UseTextOptions = True
        Me.SafeID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SafeID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SafeID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.SafeID.Properties.NullText = ""
        Me.SafeID.Size = New System.Drawing.Size(200, 36)
        Me.SafeID.StyleController = Me.LayoutControl1
        Me.SafeID.TabIndex = 4
        '
        'TextEdit5
        '
        Me.TextEdit5.Location = New System.Drawing.Point(761, 57)
        Me.TextEdit5.Name = "TextEdit5"
        Me.TextEdit5.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.TextEdit5.Properties.Appearance.Options.UseBackColor = True
        Me.TextEdit5.Properties.Appearance.Options.UseTextOptions = True
        Me.TextEdit5.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TextEdit5.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TextEdit5.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TextEdit5.Properties.Items.AddRange(New Object() {"اضافة ", "تعديل", "حذف ", "الكل"})
        Me.TextEdit5.Size = New System.Drawing.Size(245, 36)
        Me.TextEdit5.StyleController = Me.LayoutControl1
        Me.TextEdit5.TabIndex = 8
        '
        'BanckID
        '
        Me.BanckID.Location = New System.Drawing.Point(469, 57)
        Me.BanckID.Name = "BanckID"
        Me.BanckID.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.BanckID.Properties.Appearance.Options.UseBackColor = True
        Me.BanckID.Properties.Appearance.Options.UseTextOptions = True
        Me.BanckID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BanckID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BanckID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BanckID.Properties.NullText = ""
        Me.BanckID.Size = New System.Drawing.Size(200, 36)
        Me.BanckID.StyleController = Me.LayoutControl1
        Me.BanckID.TabIndex = 9
        '
        'TYPElock
        '
        Me.TYPElock.Location = New System.Drawing.Point(761, 15)
        Me.TYPElock.Name = "TYPElock"
        Me.TYPElock.Properties.Appearance.Options.UseTextOptions = True
        Me.TYPElock.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.TYPElock.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.TYPElock.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.TYPElock.Properties.Items.AddRange(New Object() {"النقدي", "عالمصرف", "الكل"})
        Me.TYPElock.Properties.PopupSizeable = True
        Me.TYPElock.Size = New System.Drawing.Size(245, 36)
        Me.TYPElock.StyleController = Me.LayoutControl1
        Me.TYPElock.TabIndex = 7
        '
        'DateEdit2
        '
        Me.DateEdit2.EditValue = Nothing
        Me.DateEdit2.Location = New System.Drawing.Point(16, 15)
        Me.DateEdit2.Name = "DateEdit2"
        Me.DateEdit2.Properties.Appearance.Options.UseTextOptions = True
        Me.DateEdit2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DateEdit2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DateEdit2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit2.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.DateEdit2.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.DateEdit2.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.DateEdit2.Properties.UseMaskAsDisplayFormat = True
        Me.DateEdit2.Size = New System.Drawing.Size(157, 36)
        Me.DateEdit2.StyleController = Me.LayoutControl1
        Me.DateEdit2.TabIndex = 2
        '
        'DateEdit1
        '
        Me.DateEdit1.EditValue = Nothing
        Me.DateEdit1.Location = New System.Drawing.Point(218, 15)
        Me.DateEdit1.Name = "DateEdit1"
        Me.DateEdit1.Properties.Appearance.Options.UseTextOptions = True
        Me.DateEdit1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DateEdit1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit1.Properties.MaskSettings.Set("mask", "yyyy/MM/dd")
        Me.DateEdit1.Properties.MaskSettings.Set("useAdvancingCaret", False)
        Me.DateEdit1.Properties.MaskSettings.Set("spinWithCarry", False)
        Me.DateEdit1.Properties.UseMaskAsDisplayFormat = True
        Me.DateEdit1.Size = New System.Drawing.Size(210, 36)
        Me.DateEdit1.StyleController = Me.LayoutControl1
        Me.DateEdit1.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem6, Me.LayoutControlItem5, Me.LayoutControlItem4, Me.LayoutControlItem7, Me.LayoutControlItem8, Me.LayoutControlItem9, Me.LayoutControlItem3, Me.LayoutControlItem2})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1108, 598)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem1.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem1.Control = Me.SafeID
        Me.LayoutControlItem1.Location = New System.Drawing.Point(453, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(292, 42)
        Me.LayoutControlItem1.Text = "أسم المستخدم"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(70, 21)
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem6.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem6.Control = Me.BanckID
        Me.LayoutControlItem6.Location = New System.Drawing.Point(453, 42)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.Size = New System.Drawing.Size(292, 42)
        Me.LayoutControlItem6.Text = "أسم الحساب"
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(70, 21)
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem5.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem5.Control = Me.TextEdit5
        Me.LayoutControlItem5.Location = New System.Drawing.Point(745, 42)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(337, 42)
        Me.LayoutControlItem5.Text = "نوع الحركة"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(70, 21)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.AppearanceItemCaption.ForeColor = System.Drawing.Color.White
        Me.LayoutControlItem4.AppearanceItemCaption.Options.UseForeColor = True
        Me.LayoutControlItem4.Control = Me.TYPElock
        Me.LayoutControlItem4.Location = New System.Drawing.Point(745, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(337, 42)
        Me.LayoutControlItem4.Text = "نوع العملية"
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(70, 21)
        '
        'LayoutControlItem7
        '
        Me.LayoutControlItem7.Control = Me.SimpleButton1
        Me.LayoutControlItem7.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem7.Name = "LayoutControlItem7"
        Me.LayoutControlItem7.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 40, 3, 3)
        Me.LayoutControlItem7.Size = New System.Drawing.Size(202, 42)
        Me.LayoutControlItem7.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem7.TextVisible = False
        '
        'LayoutControlItem8
        '
        Me.LayoutControlItem8.Control = Me.SimpleButton2
        Me.LayoutControlItem8.Location = New System.Drawing.Point(202, 42)
        Me.LayoutControlItem8.Name = "LayoutControlItem8"
        Me.LayoutControlItem8.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 40, 3, 3)
        Me.LayoutControlItem8.Size = New System.Drawing.Size(251, 42)
        Me.LayoutControlItem8.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem8.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.GridControl1
        Me.LayoutControlItem9.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(1082, 490)
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem3.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem3.Control = Me.DateEdit1
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(202, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(251, 42)
        Me.LayoutControlItem3.Text = "من"
        Me.LayoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(19, 28)
        Me.LayoutControlItem3.TextToControlDistance = 16
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 11.25!)
        Me.LayoutControlItem2.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem2.Control = Me.DateEdit2
        Me.LayoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem2.CustomizationFormText = "التاريخ"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(202, 42)
        Me.LayoutControlItem2.Text = "الي"
        Me.LayoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(23, 28)
        Me.LayoutControlItem2.TextToControlDistance = 16
        '
        'USRCurrencyMovements
        '
        Me.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(84, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LayoutControl1)
        Me.Name = "USRCurrencyMovements"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Size = New System.Drawing.Size(1108, 598)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SafeID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit5.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BanckID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TYPElock.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit2.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TYPE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ISID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyFrom As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CurrencyTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BuyPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SalePrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem6 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem7 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem8 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RetBuyPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents retSalePrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Ueserinsert As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DateForTime As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TypeMovet As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Public WithEvents TYPElock As DevExpress.XtraEditors.ComboBoxEdit
    Public WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Public WithEvents SafeID As DevExpress.XtraEditors.LookUpEdit
    Public WithEvents TextEdit5 As DevExpress.XtraEditors.ComboBoxEdit
    Public WithEvents BanckID As DevExpress.XtraEditors.LookUpEdit
    Public WithEvents DateEdit2 As DevExpress.XtraEditors.DateEdit
    Public WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
End Class
