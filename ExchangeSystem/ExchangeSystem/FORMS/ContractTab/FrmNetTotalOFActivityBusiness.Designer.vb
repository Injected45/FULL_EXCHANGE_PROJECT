<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmNetTotalOFActivityBusiness
    Inherits TemplateForm

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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmNetTotalOFActivityBusiness))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ISID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Credit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Debit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.MovementType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ClosingDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DailyClosed = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Share = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.ID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        Me.AccName = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1601, 744)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl1
        '
        Me.GridControl1.Location = New System.Drawing.Point(16, 60)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit1})
        Me.GridControl1.Size = New System.Drawing.Size(1569, 668)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.ISID, Me.AccName, Me.Credit, Me.Debit, Me.MovementType, Me.ClosingDate, Me.DailyClosed, Me.Share, Me.ID})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        '
        'SN
        '
        Me.SN.AppearanceCell.BackColor = System.Drawing.Color.Transparent
        Me.SN.AppearanceCell.Options.UseTextOptions = True
        Me.SN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SN.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.SN.AppearanceHeader.Options.UseBackColor = True
        Me.SN.AppearanceHeader.Options.UseTextOptions = True
        Me.SN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.OptionsColumn.AllowEdit = False
        Me.SN.OptionsColumn.ReadOnly = True
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 100
        '
        'ISID
        '
        Me.ISID.AppearanceCell.Options.UseTextOptions = True
        Me.ISID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ISID.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ISID.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.ISID.AppearanceHeader.Options.UseBackColor = True
        Me.ISID.AppearanceHeader.Options.UseTextOptions = True
        Me.ISID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ISID.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.ISID.Caption = "الرمز"
        Me.ISID.FieldName = "ISID"
        Me.ISID.Name = "ISID"
        Me.ISID.OptionsColumn.AllowEdit = False
        Me.ISID.OptionsColumn.ReadOnly = True
        Me.ISID.Visible = True
        Me.ISID.VisibleIndex = 1
        Me.ISID.Width = 175
        '
        'Credit
        '
        Me.Credit.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Credit.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Credit.AppearanceCell.ForeColor = System.Drawing.Color.White
        Me.Credit.AppearanceCell.Options.UseBackColor = True
        Me.Credit.AppearanceCell.Options.UseFont = True
        Me.Credit.AppearanceCell.Options.UseForeColor = True
        Me.Credit.AppearanceCell.Options.UseTextOptions = True
        Me.Credit.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Credit.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Credit.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.Credit.AppearanceHeader.Options.UseBackColor = True
        Me.Credit.AppearanceHeader.Options.UseTextOptions = True
        Me.Credit.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Credit.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Credit.Caption = "الأرباح"
        Me.Credit.FieldName = "Credit"
        Me.Credit.Name = "Credit"
        Me.Credit.OptionsColumn.AllowEdit = False
        Me.Credit.OptionsColumn.ReadOnly = True
        Me.Credit.Visible = True
        Me.Credit.VisibleIndex = 5
        Me.Credit.Width = 166
        '
        'Debit
        '
        Me.Debit.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.Debit.AppearanceCell.Font = New System.Drawing.Font("Droid Arabic Kufi", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Debit.AppearanceCell.ForeColor = System.Drawing.Color.White
        Me.Debit.AppearanceCell.Options.UseBackColor = True
        Me.Debit.AppearanceCell.Options.UseFont = True
        Me.Debit.AppearanceCell.Options.UseForeColor = True
        Me.Debit.AppearanceCell.Options.UseTextOptions = True
        Me.Debit.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Debit.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Debit.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.Debit.AppearanceHeader.Options.UseBackColor = True
        Me.Debit.AppearanceHeader.Options.UseTextOptions = True
        Me.Debit.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Debit.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Debit.Caption = "الخسائر"
        Me.Debit.FieldName = "Debit"
        Me.Debit.Name = "Debit"
        Me.Debit.OptionsColumn.AllowEdit = False
        Me.Debit.OptionsColumn.ReadOnly = True
        Me.Debit.Visible = True
        Me.Debit.VisibleIndex = 6
        Me.Debit.Width = 159
        '
        'MovementType
        '
        Me.MovementType.AppearanceCell.Options.UseTextOptions = True
        Me.MovementType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.MovementType.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.MovementType.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.MovementType.AppearanceHeader.Options.UseBackColor = True
        Me.MovementType.AppearanceHeader.Options.UseTextOptions = True
        Me.MovementType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.MovementType.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.MovementType.Caption = "فترة الإقفال"
        Me.MovementType.FieldName = "MovementType"
        Me.MovementType.Name = "MovementType"
        Me.MovementType.OptionsColumn.AllowEdit = False
        Me.MovementType.OptionsColumn.ReadOnly = True
        Me.MovementType.Visible = True
        Me.MovementType.VisibleIndex = 4
        Me.MovementType.Width = 246
        '
        'ClosingDate
        '
        Me.ClosingDate.AppearanceCell.Options.UseTextOptions = True
        Me.ClosingDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ClosingDate.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.ClosingDate.AppearanceHeader.Options.UseBackColor = True
        Me.ClosingDate.AppearanceHeader.Options.UseTextOptions = True
        Me.ClosingDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ClosingDate.Caption = "تاريخ الإقفال"
        Me.ClosingDate.FieldName = "ClosingDate"
        Me.ClosingDate.Name = "ClosingDate"
        Me.ClosingDate.Visible = True
        Me.ClosingDate.VisibleIndex = 3
        Me.ClosingDate.Width = 118
        '
        'DailyClosed
        '
        Me.DailyClosed.AppearanceCell.Options.UseTextOptions = True
        Me.DailyClosed.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DailyClosed.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DailyClosed.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.DailyClosed.AppearanceHeader.Options.UseBackColor = True
        Me.DailyClosed.AppearanceHeader.Options.UseTextOptions = True
        Me.DailyClosed.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DailyClosed.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.DailyClosed.Caption = "الحالة"
        Me.DailyClosed.FieldName = "DailyClosed"
        Me.DailyClosed.Name = "DailyClosed"
        Me.DailyClosed.OptionsColumn.AllowEdit = False
        Me.DailyClosed.OptionsColumn.ReadOnly = True
        Me.DailyClosed.Visible = True
        Me.DailyClosed.VisibleIndex = 7
        Me.DailyClosed.Width = 190
        '
        'Share
        '
        Me.Share.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.Share.AppearanceHeader.Options.UseBackColor = True
        Me.Share.AppearanceHeader.Options.UseTextOptions = True
        Me.Share.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Share.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.Share.Caption = "تقسيم"
        Me.Share.ColumnEdit = Me.RepositoryItemButtonEdit1
        Me.Share.Name = "Share"
        Me.Share.Visible = True
        Me.Share.VisibleIndex = 8
        Me.Share.Width = 118
        '
        'RepositoryItemButtonEdit1
        '
        Me.RepositoryItemButtonEdit1.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = Global.ExchangeSystem.My.Resources.Resources.money_payment_6400
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
        Me.RepositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'ID
        '
        Me.ID.Caption = "رقم العملية"
        Me.ID.FieldName = "ID"
        Me.ID.Name = "ID"
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.EmptySpaceItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1601, 744)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1575, 674)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.AllowHotTrack = False
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(1575, 44)
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'AccName
        '
        Me.AccName.AppearanceCell.Options.UseTextOptions = True
        Me.AccName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccName.AppearanceHeader.BackColor = System.Drawing.Color.CadetBlue
        Me.AccName.AppearanceHeader.Options.UseBackColor = True
        Me.AccName.AppearanceHeader.Options.UseTextOptions = True
        Me.AccName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccName.Caption = "الحساب"
        Me.AccName.FieldName = "AccName"
        Me.AccName.Name = "AccName"
        Me.AccName.Visible = True
        Me.AccName.VisibleIndex = 2
        Me.AccName.Width = 265
        '
        'FrmNetTotalOFActivityBusiness
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1601, 744)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FrmNetTotalOFActivityBusiness.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FrmNetTotalOFActivityBusiness"
        Me.Text = "صافي دخل نشاط"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ISID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Credit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Debit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DailyClosed As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents MovementType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Share As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents ID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents ClosingDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccName As DevExpress.XtraGrid.Columns.GridColumn
End Class
