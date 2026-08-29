<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMEmpLeaveStatment
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMEmpLeaveStatment))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCRole = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.EMPNAME = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DateFrom = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DateTo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.VacationType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LeaveType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LeaveName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AbsenceID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ABDays = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.DiscountVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallLeaveDays = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn13 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.EndLeave = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCRole)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1684, 671)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCRole
        '
        Me.GCRole.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GCRole.Location = New System.Drawing.Point(16, 60)
        Me.GCRole.MainView = Me.GVRole
        Me.GCRole.Name = "GCRole"
        Me.GCRole.Size = New System.Drawing.Size(1652, 595)
        Me.GCRole.TabIndex = 3
        Me.GCRole.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.Code, Me.EMPNAME, Me.BName, Me.DateFrom, Me.DateTo, Me.VacationType, Me.LeaveType, Me.LeaveName, Me.AbsenceID, Me.ABDays, Me.DiscountVal, Me.OverallLeaveDays, Me.GridColumn13, Me.EndLeave})
        Me.GVRole.GridControl = Me.GCRole
        Me.GVRole.Name = "GVRole"
        Me.GVRole.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.AppearanceCell.Options.UseTextOptions = True
        Me.SN.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.AppearanceHeader.Options.UseTextOptions = True
        Me.SN.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 82
        '
        'Code
        '
        Me.Code.AppearanceCell.Options.UseTextOptions = True
        Me.Code.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.AppearanceHeader.Options.UseTextOptions = True
        Me.Code.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "Code"
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 165
        '
        'EMPNAME
        '
        Me.EMPNAME.AppearanceCell.Options.UseTextOptions = True
        Me.EMPNAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPNAME.AppearanceHeader.Options.UseTextOptions = True
        Me.EMPNAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EMPNAME.Caption = "اسم الموظف"
        Me.EMPNAME.FieldName = "EMPNAME"
        Me.EMPNAME.Name = "EMPNAME"
        Me.EMPNAME.Visible = True
        Me.EMPNAME.VisibleIndex = 2
        Me.EMPNAME.Width = 316
        '
        'BName
        '
        Me.BName.AppearanceCell.Options.UseTextOptions = True
        Me.BName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BName.AppearanceHeader.Options.UseTextOptions = True
        Me.BName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BName.Caption = "الفرع"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 3
        Me.BName.Width = 138
        '
        'DateFrom
        '
        Me.DateFrom.AppearanceCell.Options.UseTextOptions = True
        Me.DateFrom.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DateFrom.AppearanceHeader.Options.UseTextOptions = True
        Me.DateFrom.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DateFrom.Caption = "من"
        Me.DateFrom.FieldName = "DateFrom"
        Me.DateFrom.Name = "DateFrom"
        Me.DateFrom.Visible = True
        Me.DateFrom.VisibleIndex = 4
        Me.DateFrom.Width = 131
        '
        'DateTo
        '
        Me.DateTo.AppearanceCell.Options.UseTextOptions = True
        Me.DateTo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DateTo.AppearanceHeader.Options.UseTextOptions = True
        Me.DateTo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DateTo.Caption = "إلى"
        Me.DateTo.FieldName = "DateTo"
        Me.DateTo.Name = "DateTo"
        Me.DateTo.Visible = True
        Me.DateTo.VisibleIndex = 5
        Me.DateTo.Width = 120
        '
        'VacationType
        '
        Me.VacationType.AppearanceCell.Options.UseTextOptions = True
        Me.VacationType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.VacationType.AppearanceHeader.Options.UseTextOptions = True
        Me.VacationType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.VacationType.Caption = "طبيعة الإجازة"
        Me.VacationType.FieldName = "VacationType"
        Me.VacationType.Name = "VacationType"
        Me.VacationType.Width = 115
        '
        'LeaveType
        '
        Me.LeaveType.AppearanceCell.Options.UseTextOptions = True
        Me.LeaveType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LeaveType.AppearanceHeader.Options.UseTextOptions = True
        Me.LeaveType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LeaveType.Caption = "نوع الاحتساب"
        Me.LeaveType.FieldName = "LeaveType"
        Me.LeaveType.Name = "LeaveType"
        Me.LeaveType.Width = 115
        '
        'LeaveName
        '
        Me.LeaveName.AppearanceCell.Options.UseTextOptions = True
        Me.LeaveName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LeaveName.AppearanceHeader.Options.UseTextOptions = True
        Me.LeaveName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LeaveName.Caption = "نوع الإجازة"
        Me.LeaveName.FieldName = "LeaveName"
        Me.LeaveName.Name = "LeaveName"
        Me.LeaveName.Width = 116
        '
        'AbsenceID
        '
        Me.AbsenceID.AppearanceCell.Options.UseTextOptions = True
        Me.AbsenceID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AbsenceID.AppearanceHeader.Options.UseTextOptions = True
        Me.AbsenceID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.AbsenceID.Caption = "نوع الغياب"
        Me.AbsenceID.FieldName = "AbsenceID"
        Me.AbsenceID.Name = "AbsenceID"
        Me.AbsenceID.Width = 101
        '
        'ABDays
        '
        Me.ABDays.AppearanceCell.Options.UseTextOptions = True
        Me.ABDays.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ABDays.AppearanceHeader.Options.UseTextOptions = True
        Me.ABDays.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ABDays.Caption = "أيام الغياب"
        Me.ABDays.FieldName = "ABDays"
        Me.ABDays.Name = "ABDays"
        Me.ABDays.Visible = True
        Me.ABDays.VisibleIndex = 6
        Me.ABDays.Width = 117
        '
        'DiscountVal
        '
        Me.DiscountVal.AppearanceCell.Options.UseTextOptions = True
        Me.DiscountVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DiscountVal.AppearanceHeader.Options.UseTextOptions = True
        Me.DiscountVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.DiscountVal.Caption = "قيمة الخصم"
        Me.DiscountVal.FieldName = "DiscountVal"
        Me.DiscountVal.Name = "DiscountVal"
        Me.DiscountVal.Visible = True
        Me.DiscountVal.VisibleIndex = 7
        Me.DiscountVal.Width = 119
        '
        'OverallLeaveDays
        '
        Me.OverallLeaveDays.AppearanceCell.Options.UseTextOptions = True
        Me.OverallLeaveDays.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallLeaveDays.AppearanceHeader.Options.UseTextOptions = True
        Me.OverallLeaveDays.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallLeaveDays.Caption = "إجمالي أيام الإجازة"
        Me.OverallLeaveDays.FieldName = "OverallLeaveDays"
        Me.OverallLeaveDays.Name = "OverallLeaveDays"
        Me.OverallLeaveDays.Visible = True
        Me.OverallLeaveDays.VisibleIndex = 8
        Me.OverallLeaveDays.Width = 139
        '
        'GridColumn13
        '
        Me.GridColumn13.AppearanceCell.Options.UseTextOptions = True
        Me.GridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridColumn13.AppearanceHeader.Options.UseTextOptions = True
        Me.GridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridColumn13.Caption = "ملاحظات"
        Me.GridColumn13.FieldName = "Notes"
        Me.GridColumn13.Name = "GridColumn13"
        Me.GridColumn13.Visible = True
        Me.GridColumn13.VisibleIndex = 9
        Me.GridColumn13.Width = 172
        '
        'EndLeave
        '
        Me.EndLeave.AppearanceCell.Options.UseTextOptions = True
        Me.EndLeave.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EndLeave.AppearanceHeader.Options.UseTextOptions = True
        Me.EndLeave.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.EndLeave.Caption = "حالة الإجازة"
        Me.EndLeave.FieldName = "EndLeave"
        Me.EndLeave.Name = "EndLeave"
        Me.EndLeave.Visible = True
        Me.EndLeave.VisibleIndex = 10
        Me.EndLeave.Width = 121
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem3, Me.EmptySpaceItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1684, 671)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.GCRole
        Me.LayoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(1658, 601)
        Me.LayoutControlItem3.Text = "LayoutControlItem1"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextVisible = False
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.AllowHotTrack = False
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(1658, 44)
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'FRMEmpLeaveStatment
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1684, 671)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMEmpLeaveStatment.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMEmpLeaveStatment"
        Me.Text = "استعلام الإجازات"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GCRole As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EMPNAME As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DateFrom As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DateTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents VacationType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LeaveType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LeaveName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AbsenceID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ABDays As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents DiscountVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallLeaveDays As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn13 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents EndLeave As DevExpress.XtraGrid.Columns.GridColumn
End Class
