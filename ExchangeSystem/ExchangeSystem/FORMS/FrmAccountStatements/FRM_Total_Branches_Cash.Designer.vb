<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_Total_Branches_Cash
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM_Total_Branches_Cash))
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Debit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Credit = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.netfortotAL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchID = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 0)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(1146, 711)
        Me.GridControl1.TabIndex = 0
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.BName, Me.Debit, Me.Credit, Me.netfortotAL, Me.BranchID})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 70
        '
        'BName
        '
        Me.BName.Caption = "اسم الفرع"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 1
        Me.BName.Width = 252
        '
        'Debit
        '
        Me.Debit.AppearanceCell.BackColor = System.Drawing.Color.Green
        Me.Debit.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.Debit.AppearanceCell.Options.UseBackColor = True
        Me.Debit.AppearanceCell.Options.UseForeColor = True
        Me.Debit.Caption = "النقدية"
        Me.Debit.FieldName = "Debit"
        Me.Debit.Name = "Debit"
        Me.Debit.Visible = True
        Me.Debit.VisibleIndex = 2
        Me.Debit.Width = 221
        '
        'Credit
        '
        Me.Credit.AppearanceCell.BackColor = System.Drawing.Color.Red
        Me.Credit.AppearanceCell.ForeColor = System.Drawing.Color.Yellow
        Me.Credit.AppearanceCell.Options.UseBackColor = True
        Me.Credit.AppearanceCell.Options.UseForeColor = True
        Me.Credit.Caption = "الاتزمات"
        Me.Credit.FieldName = "Credit"
        Me.Credit.Name = "Credit"
        Me.Credit.Visible = True
        Me.Credit.VisibleIndex = 3
        Me.Credit.Width = 246
        '
        'netfortotAL
        '
        Me.netfortotAL.Caption = "الصافي"
        Me.netfortotAL.FieldName = "netfortotAL"
        Me.netfortotAL.Name = "netfortotAL"
        Me.netfortotAL.Visible = True
        Me.netfortotAL.VisibleIndex = 4
        Me.netfortotAL.Width = 208
        '
        'BranchID
        '
        Me.BranchID.Caption = "رقم الفرع"
        Me.BranchID.FieldName = "BranchID"
        Me.BranchID.Name = "BranchID"
        '
        'FRM_Total_Branches_Cash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1146, 711)
        Me.Controls.Add(Me.GridControl1)
        Me.IconOptions.LargeImage = CType(resources.GetObject("FRM_Total_Branches_Cash.IconOptions.LargeImage"), System.Drawing.Image)
        Me.MaximizeBox = False
        Me.Name = "FRM_Total_Branches_Cash"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "اجمالي نقدية الفروع "
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Debit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Credit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents netfortotAL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BranchID As DevExpress.XtraGrid.Columns.GridColumn
End Class
