<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMLimitedStatment
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMLimitedStatment))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GridControl11 = New DevExpress.XtraGrid.GridControl()
        Me.GridView11 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LimitedVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AccVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LeftVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LimitedDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GridControl11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GridControl11)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1202, 507)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GridControl11
        '
        Me.GridControl11.Location = New System.Drawing.Point(18, 14)
        Me.GridControl11.MainView = Me.GridView11
        Me.GridControl11.Name = "GridControl11"
        Me.GridControl11.Size = New System.Drawing.Size(1166, 479)
        Me.GridControl11.TabIndex = 4
        Me.GridControl11.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView11})
        '
        'GridView11
        '
        Me.GridView11.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.AccType, Me.AccName, Me.LimitedVal, Me.AccVal, Me.LeftVal, Me.LimitedDate})
        Me.GridView11.DetailHeight = 254
        Me.GridView11.GridControl = Me.GridControl11
        Me.GridView11.Name = "GridView11"
        Me.GridView11.OptionsEditForm.PopupEditFormWidth = 914
        Me.GridView11.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView11.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView11.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView11.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView11.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.AppearanceCell.Options.UseTextOptions = True
        Me.SN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.AppearanceHeader.Options.UseTextOptions = True
        Me.SN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.MinWidth = 23
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 72
        '
        'AccType
        '
        Me.AccType.AppearanceCell.Options.UseTextOptions = True
        Me.AccType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccType.AppearanceHeader.Options.UseTextOptions = True
        Me.AccType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccType.Caption = "نوع الحساب"
        Me.AccType.FieldName = "AccType"
        Me.AccType.MinWidth = 23
        Me.AccType.Name = "AccType"
        Me.AccType.Visible = True
        Me.AccType.VisibleIndex = 1
        Me.AccType.Width = 119
        '
        'AccName
        '
        Me.AccName.AppearanceCell.BackColor = System.Drawing.Color.Transparent
        Me.AccName.AppearanceCell.BorderColor = System.Drawing.Color.Transparent
        Me.AccName.AppearanceCell.ForeColor = System.Drawing.Color.Black
        Me.AccName.AppearanceCell.Options.UseBackColor = True
        Me.AccName.AppearanceCell.Options.UseBorderColor = True
        Me.AccName.AppearanceCell.Options.UseForeColor = True
        Me.AccName.AppearanceCell.Options.UseTextOptions = True
        Me.AccName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccName.AppearanceHeader.Options.UseTextOptions = True
        Me.AccName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccName.Caption = "اسم الحساب"
        Me.AccName.FieldName = "AccName"
        Me.AccName.MinWidth = 23
        Me.AccName.Name = "AccName"
        Me.AccName.Visible = True
        Me.AccName.VisibleIndex = 2
        Me.AccName.Width = 288
        '
        'LimitedVal
        '
        Me.LimitedVal.AppearanceCell.BackColor = System.Drawing.Color.Transparent
        Me.LimitedVal.AppearanceCell.BorderColor = System.Drawing.Color.Transparent
        Me.LimitedVal.AppearanceCell.ForeColor = System.Drawing.Color.Black
        Me.LimitedVal.AppearanceCell.Options.UseBackColor = True
        Me.LimitedVal.AppearanceCell.Options.UseBorderColor = True
        Me.LimitedVal.AppearanceCell.Options.UseForeColor = True
        Me.LimitedVal.AppearanceCell.Options.UseTextOptions = True
        Me.LimitedVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LimitedVal.AppearanceHeader.Options.UseTextOptions = True
        Me.LimitedVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LimitedVal.Caption = "السقف بالدين"
        Me.LimitedVal.FieldName = "LimitedVal"
        Me.LimitedVal.MinWidth = 23
        Me.LimitedVal.Name = "LimitedVal"
        Me.LimitedVal.Visible = True
        Me.LimitedVal.VisibleIndex = 3
        Me.LimitedVal.Width = 185
        '
        'AccVal
        '
        Me.AccVal.AppearanceCell.Options.UseTextOptions = True
        Me.AccVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccVal.AppearanceHeader.Options.UseTextOptions = True
        Me.AccVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AccVal.Caption = "جاري+حوالات صادرة"
        Me.AccVal.FieldName = "AccVal"
        Me.AccVal.MinWidth = 23
        Me.AccVal.Name = "AccVal"
        Me.AccVal.Visible = True
        Me.AccVal.VisibleIndex = 4
        Me.AccVal.Width = 147
        '
        'LeftVal
        '
        Me.LeftVal.AppearanceCell.Options.UseTextOptions = True
        Me.LeftVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LeftVal.AppearanceHeader.Options.UseTextOptions = True
        Me.LeftVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LeftVal.Caption = "المسموح بالتحويل"
        Me.LeftVal.FieldName = "LeftVal"
        Me.LeftVal.MinWidth = 23
        Me.LeftVal.Name = "LeftVal"
        Me.LeftVal.Visible = True
        Me.LeftVal.VisibleIndex = 5
        Me.LeftVal.Width = 169
        '
        'LimitedDate
        '
        Me.LimitedDate.AppearanceCell.BackColor = System.Drawing.Color.Transparent
        Me.LimitedDate.AppearanceCell.BorderColor = System.Drawing.Color.Yellow
        Me.LimitedDate.AppearanceCell.ForeColor = System.Drawing.Color.Black
        Me.LimitedDate.AppearanceCell.Options.UseBackColor = True
        Me.LimitedDate.AppearanceCell.Options.UseBorderColor = True
        Me.LimitedDate.AppearanceCell.Options.UseForeColor = True
        Me.LimitedDate.AppearanceCell.Options.UseTextOptions = True
        Me.LimitedDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LimitedDate.AppearanceHeader.Options.UseTextOptions = True
        Me.LimitedDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LimitedDate.Caption = "أخر تعديل"
        Me.LimitedDate.FieldName = "LimitedDate"
        Me.LimitedDate.MinWidth = 23
        Me.LimitedDate.Name = "LimitedDate"
        Me.LimitedDate.Visible = True
        Me.LimitedDate.VisibleIndex = 6
        Me.LimitedDate.Width = 149
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1202, 507)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl11
        Me.LayoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1172, 485)
        Me.LayoutControlItem1.TextVisible = False
        '
        'FRMLimitedStatment
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1202, 507)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMLimitedStatment.IconOptions.Image"), System.Drawing.Image)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FRMLimitedStatment"
        Me.Text = "تقرير حدود السحب"
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GridControl11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControl11 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView11 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AccName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LimitedVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LimitedDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents AccVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LeftVal As DevExpress.XtraGrid.Columns.GridColumn
End Class
