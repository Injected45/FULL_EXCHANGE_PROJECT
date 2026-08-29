<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMCONFIRMISSUED
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.RB4 = New System.Windows.Forms.RadioButton()
        Me.RB3 = New System.Windows.Forms.RadioButton()
        Me.RB2 = New System.Windows.Forms.RadioButton()
        Me.RB1 = New System.Windows.Forms.RadioButton()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.RowHandle = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertTime = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SenderName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SPhone = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RecievedName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RPhone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ExVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DeliveryPlace = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchRecievedID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchDeliveredID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.UNameLog = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CANCELSTATUS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ConfirmCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnConfirm = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.IsAccTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Brid = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.RB4)
        Me.LayoutControl1.Controls.Add(Me.RB3)
        Me.LayoutControl1.Controls.Add(Me.RB2)
        Me.LayoutControl1.Controls.Add(Me.RB1)
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1867, 751)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'RB4
        '
        Me.RB4.Location = New System.Drawing.Point(16, 16)
        Me.RB4.Name = "RB4"
        Me.RB4.Size = New System.Drawing.Size(562, 25)
        Me.RB4.TabIndex = 8
        Me.RB4.Text = "حوالات خارجية ملغية"
        Me.RB4.UseVisualStyleBackColor = True
        '
        'RB3
        '
        Me.RB3.Location = New System.Drawing.Point(584, 16)
        Me.RB3.Name = "RB3"
        Me.RB3.Size = New System.Drawing.Size(345, 25)
        Me.RB3.TabIndex = 7
        Me.RB3.Text = "حوالات داخلية ملغية"
        Me.RB3.UseVisualStyleBackColor = True
        '
        'RB2
        '
        Me.RB2.Location = New System.Drawing.Point(935, 16)
        Me.RB2.Name = "RB2"
        Me.RB2.Size = New System.Drawing.Size(446, 25)
        Me.RB2.TabIndex = 6
        Me.RB2.Text = "حوالات خارجية"
        Me.RB2.UseVisualStyleBackColor = True
        '
        'RB1
        '
        Me.RB1.Checked = True
        Me.RB1.Location = New System.Drawing.Point(1387, 16)
        Me.RB1.Name = "RB1"
        Me.RB1.Size = New System.Drawing.Size(464, 25)
        Me.RB1.TabIndex = 5
        Me.RB1.TabStop = True
        Me.RB1.Text = "حوالات داخلية"
        Me.RB1.UseVisualStyleBackColor = True
        '
        'GCROLE
        '
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Location = New System.Drawing.Point(16, 47)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnConfirm})
        Me.GCROLE.Size = New System.Drawing.Size(1835, 688)
        Me.GCROLE.TabIndex = 4
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(196, Byte), Integer))
        Me.GVROLE.Appearance.EvenRow.Options.UseBackColor = True
        Me.GVROLE.Appearance.OddRow.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GVROLE.Appearance.OddRow.Options.UseBackColor = True
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.RowHandle, Me.Code, Me.InsertDate, Me.InsertTime, Me.SenderName, Me.SPhone, Me.RecievedName, Me.RPhone1, Me.OverallVal, Me.ExVal, Me.DeliveryPlace, Me.BranchRecievedID, Me.BranchDeliveredID, Me.UNameLog, Me.CANCELSTATUS, Me.ConfirmCol, Me.IsAccTo, Me.Brid})
        Me.GVROLE.DetailHeight = 294
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'RowHandle
        '
        Me.RowHandle.AppearanceCell.Options.UseTextOptions = True
        Me.RowHandle.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RowHandle.AppearanceHeader.Options.UseTextOptions = True
        Me.RowHandle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RowHandle.Caption = "#"
        Me.RowHandle.FieldName = "RowHandle"
        Me.RowHandle.Name = "RowHandle"
        Me.RowHandle.OptionsColumn.AllowEdit = False
        Me.RowHandle.OptionsColumn.ReadOnly = True
        Me.RowHandle.UnboundDataType = GetType(Integer)
        Me.RowHandle.Visible = True
        Me.RowHandle.VisibleIndex = 0
        Me.RowHandle.Width = 56
        '
        'Code
        '
        Me.Code.AppearanceCell.Options.UseTextOptions = True
        Me.Code.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.AppearanceHeader.Options.UseTextOptions = True
        Me.Code.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "Code"
        Me.Code.MinWidth = 16
        Me.Code.Name = "Code"
        Me.Code.OptionsColumn.AllowEdit = False
        Me.Code.OptionsColumn.ReadOnly = True
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 159
        '
        'InsertDate
        '
        Me.InsertDate.AppearanceCell.Options.UseTextOptions = True
        Me.InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertDate.AppearanceHeader.Options.UseTextOptions = True
        Me.InsertDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertDate.Caption = "التاريخ"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.MinWidth = 16
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.OptionsColumn.AllowEdit = False
        Me.InsertDate.OptionsColumn.ReadOnly = True
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 2
        Me.InsertDate.Width = 135
        '
        'InsertTime
        '
        Me.InsertTime.AppearanceCell.Options.UseTextOptions = True
        Me.InsertTime.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertTime.AppearanceHeader.Options.UseTextOptions = True
        Me.InsertTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertTime.Caption = "الوقت"
        Me.InsertTime.DisplayFormat.FormatString = "t"
        Me.InsertTime.FieldName = "InsertTime"
        Me.InsertTime.GroupFormat.FormatString = "t"
        Me.InsertTime.Name = "InsertTime"
        Me.InsertTime.OptionsColumn.AllowEdit = False
        Me.InsertTime.OptionsColumn.ReadOnly = True
        Me.InsertTime.Visible = True
        Me.InsertTime.VisibleIndex = 3
        Me.InsertTime.Width = 99
        '
        'SenderName
        '
        Me.SenderName.AppearanceCell.Options.UseTextOptions = True
        Me.SenderName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SenderName.AppearanceHeader.Options.UseTextOptions = True
        Me.SenderName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SenderName.Caption = "اسم الراسل"
        Me.SenderName.FieldName = "SenderName"
        Me.SenderName.MinWidth = 16
        Me.SenderName.Name = "SenderName"
        Me.SenderName.OptionsColumn.AllowEdit = False
        Me.SenderName.OptionsColumn.ReadOnly = True
        Me.SenderName.Visible = True
        Me.SenderName.VisibleIndex = 4
        Me.SenderName.Width = 204
        '
        'SPhone
        '
        Me.SPhone.Caption = "هاتف الراسل"
        Me.SPhone.FieldName = "SPhone"
        Me.SPhone.MinWidth = 16
        Me.SPhone.Name = "SPhone"
        Me.SPhone.OptionsColumn.ReadOnly = True
        Me.SPhone.Width = 142
        '
        'RecievedName
        '
        Me.RecievedName.AppearanceCell.Options.UseTextOptions = True
        Me.RecievedName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RecievedName.AppearanceHeader.Options.UseTextOptions = True
        Me.RecievedName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RecievedName.Caption = "اسم المستلم"
        Me.RecievedName.FieldName = "RecievedName"
        Me.RecievedName.MinWidth = 16
        Me.RecievedName.Name = "RecievedName"
        Me.RecievedName.OptionsColumn.AllowEdit = False
        Me.RecievedName.OptionsColumn.ReadOnly = True
        Me.RecievedName.Visible = True
        Me.RecievedName.VisibleIndex = 5
        Me.RecievedName.Width = 189
        '
        'RPhone1
        '
        Me.RPhone1.Caption = "هاتف"
        Me.RPhone1.FieldName = "RPhone1"
        Me.RPhone1.MinWidth = 16
        Me.RPhone1.Name = "RPhone1"
        Me.RPhone1.OptionsColumn.ReadOnly = True
        Me.RPhone1.Width = 142
        '
        'OverallVal
        '
        Me.OverallVal.AppearanceCell.Options.UseTextOptions = True
        Me.OverallVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallVal.AppearanceHeader.Options.UseTextOptions = True
        Me.OverallVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallVal.Caption = "قيمة الحوالة"
        Me.OverallVal.FieldName = "OverallVal"
        Me.OverallVal.MinWidth = 16
        Me.OverallVal.Name = "OverallVal"
        Me.OverallVal.OptionsColumn.AllowEdit = False
        Me.OverallVal.OptionsColumn.ReadOnly = True
        Me.OverallVal.Visible = True
        Me.OverallVal.VisibleIndex = 6
        Me.OverallVal.Width = 141
        '
        'ExVal
        '
        Me.ExVal.AppearanceCell.Options.UseTextOptions = True
        Me.ExVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExVal.AppearanceHeader.Options.UseTextOptions = True
        Me.ExVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExVal.Caption = "العمولة"
        Me.ExVal.FieldName = "ExVal"
        Me.ExVal.MinWidth = 16
        Me.ExVal.Name = "ExVal"
        Me.ExVal.OptionsColumn.AllowEdit = False
        Me.ExVal.OptionsColumn.ReadOnly = True
        Me.ExVal.Visible = True
        Me.ExVal.VisibleIndex = 7
        Me.ExVal.Width = 107
        '
        'DeliveryPlace
        '
        Me.DeliveryPlace.AppearanceCell.Options.UseTextOptions = True
        Me.DeliveryPlace.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DeliveryPlace.AppearanceHeader.Options.UseTextOptions = True
        Me.DeliveryPlace.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DeliveryPlace.Caption = "الوجهة"
        Me.DeliveryPlace.FieldName = "DeliveryPlace"
        Me.DeliveryPlace.MinWidth = 16
        Me.DeliveryPlace.Name = "DeliveryPlace"
        Me.DeliveryPlace.OptionsColumn.AllowEdit = False
        Me.DeliveryPlace.OptionsColumn.ReadOnly = True
        Me.DeliveryPlace.Width = 129
        '
        'BranchRecievedID
        '
        Me.BranchRecievedID.AppearanceCell.Options.UseTextOptions = True
        Me.BranchRecievedID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchRecievedID.AppearanceHeader.Options.UseTextOptions = True
        Me.BranchRecievedID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchRecievedID.Caption = "مكان الاستلام"
        Me.BranchRecievedID.FieldName = "BranchRecievedID"
        Me.BranchRecievedID.MinWidth = 16
        Me.BranchRecievedID.Name = "BranchRecievedID"
        Me.BranchRecievedID.OptionsColumn.AllowEdit = False
        Me.BranchRecievedID.OptionsColumn.ReadOnly = True
        Me.BranchRecievedID.Visible = True
        Me.BranchRecievedID.VisibleIndex = 8
        Me.BranchRecievedID.Width = 120
        '
        'BranchDeliveredID
        '
        Me.BranchDeliveredID.AppearanceCell.Options.UseTextOptions = True
        Me.BranchDeliveredID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchDeliveredID.AppearanceHeader.Options.UseTextOptions = True
        Me.BranchDeliveredID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchDeliveredID.Caption = "مكان التسليم"
        Me.BranchDeliveredID.FieldName = "BranchDeliveredID"
        Me.BranchDeliveredID.MinWidth = 16
        Me.BranchDeliveredID.Name = "BranchDeliveredID"
        Me.BranchDeliveredID.OptionsColumn.AllowEdit = False
        Me.BranchDeliveredID.OptionsColumn.ReadOnly = True
        Me.BranchDeliveredID.Visible = True
        Me.BranchDeliveredID.VisibleIndex = 10
        Me.BranchDeliveredID.Width = 143
        '
        'UNameLog
        '
        Me.UNameLog.AppearanceCell.Options.UseTextOptions = True
        Me.UNameLog.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UNameLog.AppearanceHeader.Options.UseTextOptions = True
        Me.UNameLog.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.UNameLog.Caption = "مقدم الطلب"
        Me.UNameLog.FieldName = "UNameLog"
        Me.UNameLog.Name = "UNameLog"
        Me.UNameLog.OptionsColumn.AllowEdit = False
        Me.UNameLog.OptionsColumn.ReadOnly = True
        Me.UNameLog.Visible = True
        Me.UNameLog.VisibleIndex = 9
        Me.UNameLog.Width = 209
        '
        'CANCELSTATUS
        '
        Me.CANCELSTATUS.Caption = "حالة الاعتماد"
        Me.CANCELSTATUS.FieldName = "CANCELSTATUS"
        Me.CANCELSTATUS.Name = "CANCELSTATUS"
        Me.CANCELSTATUS.Width = 51
        '
        'ConfirmCol
        '
        Me.ConfirmCol.AppearanceCell.Options.UseTextOptions = True
        Me.ConfirmCol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ConfirmCol.AppearanceHeader.Options.UseTextOptions = True
        Me.ConfirmCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ConfirmCol.Caption = "اعتماد"
        Me.ConfirmCol.ColumnEdit = Me.BtnConfirm
        Me.ConfirmCol.FieldName = "ConfirmCol"
        Me.ConfirmCol.Name = "ConfirmCol"
        Me.ConfirmCol.Visible = True
        Me.ConfirmCol.VisibleIndex = 11
        Me.ConfirmCol.Width = 112
        '
        'BtnConfirm
        '
        Me.BtnConfirm.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources.handconfirm
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnConfirm.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnConfirm.Name = "BtnConfirm"
        Me.BtnConfirm.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'IsAccTo
        '
        Me.IsAccTo.Caption = "نوع المستلم حساب او زبون عادي"
        Me.IsAccTo.FieldName = "IsAccTo"
        Me.IsAccTo.Name = "IsAccTo"
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1867, 751)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 31)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1841, 694)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.RB1
        Me.LayoutControlItem2.Location = New System.Drawing.Point(1371, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(470, 31)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.RB2
        Me.LayoutControlItem3.Location = New System.Drawing.Point(919, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(452, 31)
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.RB3
        Me.LayoutControlItem4.Location = New System.Drawing.Point(568, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Size = New System.Drawing.Size(351, 31)
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.RB4
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(568, 31)
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem5.TextVisible = False
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 2000
        '
        'Brid
        '
        Me.Brid.Caption = "BRID"
        Me.Brid.FieldName = "Brid"
        Me.Brid.Name = "Brid"
        '
        'FRMCONFIRMISSUED
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1867, 751)
        Me.Controls.Add(Me.LayoutControl1)
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Name = "FRMCONFIRMISSUED"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "اعتماد الحوالات"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SenderName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RecievedName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SPhone As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RPhone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ExVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DeliveryPlace As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BranchRecievedID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BranchDeliveredID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents RowHandle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ConfirmCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnConfirm As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents CANCELSTATUS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RB4 As RadioButton
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As Timer
    Friend WithEvents IsAccTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents UNameLog As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertTime As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RB3 As RadioButton
    Friend WithEvents RB2 As RadioButton
    Friend WithEvents RB1 As RadioButton
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Brid As DevExpress.XtraGrid.Columns.GridColumn
End Class
