<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMCities
    Inherits FrmMaster

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
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.CountriesID = New DevExpress.XtraEditors.LookUpEdit()
        Me.LB = New DevExpress.XtraEditors.ListBoxControl()
        Me.CityName = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.CountriesID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CityName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.CountriesID)
        Me.LayoutControl1.Controls.Add(Me.LB)
        Me.LayoutControl1.Controls.Add(Me.CityName)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 43)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(430, 395)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'CountriesID
        '
        Me.CountriesID.Location = New System.Drawing.Point(16, 16)
        Me.CountriesID.Name = "CountriesID"
        Me.CountriesID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CountriesID.Properties.NullText = ""
        Me.CountriesID.Properties.ShowFooter = False
        Me.CountriesID.Properties.ShowHeader = False
        Me.CountriesID.Size = New System.Drawing.Size(354, 36)
        Me.CountriesID.StyleController = Me.LayoutControl1
        Me.CountriesID.TabIndex = 0
        '
        'LB
        '
        Me.LB.Location = New System.Drawing.Point(16, 100)
        Me.LB.Name = "LB"
        Me.LB.Size = New System.Drawing.Size(398, 279)
        Me.LB.StyleController = Me.LayoutControl1
        Me.LB.TabIndex = 3
        '
        'CityName
        '
        Me.CityName.Location = New System.Drawing.Point(16, 58)
        Me.CityName.Name = "CityName"
        Me.CityName.Size = New System.Drawing.Size(354, 36)
        Me.CityName.StyleController = Me.LayoutControl1
        Me.CityName.TabIndex = 2
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(430, 395)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CityName
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 42)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(404, 42)
        Me.LayoutControlItem1.Text = "الاسم"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(28, 21)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.LB
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 84)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(404, 285)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.CountriesID
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(404, 42)
        Me.LayoutControlItem3.Text = "الدولة"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(28, 21)
        '
        'FRMCities
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(430, 438)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.city
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMCities"
        Me.Text = "إضافة مدينة"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.CountriesID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CityName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CityName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LB As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CountriesID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
End Class
