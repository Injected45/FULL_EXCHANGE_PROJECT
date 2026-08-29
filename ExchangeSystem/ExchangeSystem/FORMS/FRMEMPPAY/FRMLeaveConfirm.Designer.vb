<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FRMLeaveConfirm
    Inherits TemplateForm

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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMLeaveConfirm))
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions3 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject9 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject10 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject11 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject12 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LeaveType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.EMPNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallLeaveDays = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DateFrom = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DateTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BossName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TXTBossName = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.ConfirmCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnConfirm = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.RejectCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnReject = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.EndCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnEnd = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TXTBossName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnReject, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1742, 552)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCROLE
        '
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Location = New System.Drawing.Point(16, 16)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnConfirm, Me.BtnReject, Me.BtnEnd, Me.TXTBossName})
        Me.GCROLE.Size = New System.Drawing.Size(1710, 520)
        Me.GCROLE.TabIndex = 5
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.Code, Me.InsertDate, Me.LeaveType, Me.EMPNAME, Me.OverallLeaveDays, Me.DateFrom, Me.DateTo, Me.BossName, Me.ConfirmCol, Me.RejectCol, Me.EndCol})
        Me.GVROLE.DetailHeight = 266
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "#"
        Me.SN.Name = "SN"
        Me.SN.OptionsColumn.AllowEdit = False
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 78
        '
        'Code
        '
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "Code"
        Me.Code.MinWidth = 16
        Me.Code.Name = "Code"
        Me.Code.OptionsColumn.ReadOnly = True
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 122
        '
        'InsertDate
        '
        Me.InsertDate.Caption = "التاريخ"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.MinWidth = 16
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.OptionsColumn.ReadOnly = True
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 2
        Me.InsertDate.Width = 122
        '
        'LeaveType
        '
        Me.LeaveType.Caption = "نوع الإجازة"
        Me.LeaveType.FieldName = "LeaveType"
        Me.LeaveType.MinWidth = 16
        Me.LeaveType.Name = "LeaveType"
        Me.LeaveType.OptionsColumn.ReadOnly = True
        Me.LeaveType.Visible = True
        Me.LeaveType.VisibleIndex = 7
        Me.LeaveType.Width = 69
        '
        'EMPNAME
        '
        Me.EMPNAME.Caption = "اسم الموظف"
        Me.EMPNAME.FieldName = "EMPNAME"
        Me.EMPNAME.MinWidth = 16
        Me.EMPNAME.Name = "EMPNAME"
        Me.EMPNAME.OptionsColumn.ReadOnly = True
        Me.EMPNAME.Visible = True
        Me.EMPNAME.VisibleIndex = 3
        Me.EMPNAME.Width = 122
        '
        'OverallLeaveDays
        '
        Me.OverallLeaveDays.Caption = "عدد الأيام"
        Me.OverallLeaveDays.FieldName = "OverallLeaveDays"
        Me.OverallLeaveDays.MinWidth = 16
        Me.OverallLeaveDays.Name = "OverallLeaveDays"
        Me.OverallLeaveDays.OptionsColumn.ReadOnly = True
        Me.OverallLeaveDays.Visible = True
        Me.OverallLeaveDays.VisibleIndex = 4
        Me.OverallLeaveDays.Width = 122
        '
        'DateFrom
        '
        Me.DateFrom.Caption = "من تاريخ"
        Me.DateFrom.FieldName = "DateFrom"
        Me.DateFrom.MinWidth = 16
        Me.DateFrom.Name = "DateFrom"
        Me.DateFrom.OptionsColumn.ReadOnly = True
        Me.DateFrom.Visible = True
        Me.DateFrom.VisibleIndex = 5
        Me.DateFrom.Width = 122
        '
        'DateTo
        '
        Me.DateTo.Caption = "إلى تاريخ"
        Me.DateTo.FieldName = "DateTo"
        Me.DateTo.MinWidth = 16
        Me.DateTo.Name = "DateTo"
        Me.DateTo.OptionsColumn.ReadOnly = True
        Me.DateTo.Visible = True
        Me.DateTo.VisibleIndex = 6
        Me.DateTo.Width = 86
        '
        'BossName
        '
        Me.BossName.Caption = "اسم المدير"
        Me.BossName.ColumnEdit = Me.TXTBossName
        Me.BossName.FieldName = "BossName"
        Me.BossName.Name = "BossName"
        Me.BossName.Visible = True
        Me.BossName.VisibleIndex = 8
        '
        'TXTBossName
        '
        Me.TXTBossName.AutoHeight = False
        Me.TXTBossName.Name = "TXTBossName"
        '
        'ConfirmCol
        '
        Me.ConfirmCol.Caption = "اعتماد"
        Me.ConfirmCol.ColumnEdit = Me.BtnConfirm
        Me.ConfirmCol.FieldName = "ConfirmCol"
        Me.ConfirmCol.Name = "ConfirmCol"
        Me.ConfirmCol.Visible = True
        Me.ConfirmCol.VisibleIndex = 9
        Me.ConfirmCol.Width = 110
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
        'RejectCol
        '
        Me.RejectCol.Caption = "رفض"
        Me.RejectCol.ColumnEdit = Me.BtnReject
        Me.RejectCol.FieldName = "RejectCol"
        Me.RejectCol.Name = "RejectCol"
        Me.RejectCol.Visible = True
        Me.RejectCol.VisibleIndex = 10
        Me.RejectCol.Width = 117
        '
        'BtnReject
        '
        Me.BtnReject.AutoHeight = False
        EditorButtonImageOptions2.SvgImage = CType(resources.GetObject("EditorButtonImageOptions2.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions2.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnReject.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnReject.Name = "BtnReject"
        Me.BtnReject.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'EndCol
        '
        Me.EndCol.Caption = "إنهاء"
        Me.EndCol.ColumnEdit = Me.BtnEnd
        Me.EndCol.FieldName = "EndCol"
        Me.EndCol.Name = "EndCol"
        Me.EndCol.Visible = True
        Me.EndCol.VisibleIndex = 11
        '
        'BtnEnd
        '
        Me.BtnEnd.AutoHeight = False
        EditorButtonImageOptions3.SvgImage = CType(resources.GetObject("EditorButtonImageOptions3.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions3.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnEnd.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions3, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject9, SerializableAppearanceObject10, SerializableAppearanceObject11, SerializableAppearanceObject12, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnEnd.Name = "BtnEnd"
        Me.BtnEnd.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1742, 552)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1716, 526)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'FRMLeaveConfirm
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1742, 552)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMLeaveConfirm.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMLeaveConfirm"
        Me.Text = "عرض"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TXTBossName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnConfirm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnReject, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EMPNAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallLeaveDays As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DateFrom As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DateTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LeaveType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ConfirmCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnConfirm As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents RejectCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnReject As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents EndCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnEnd As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BossName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents TXTBossName As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
End Class
