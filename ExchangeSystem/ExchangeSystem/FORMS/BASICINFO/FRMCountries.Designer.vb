<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMCountries
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
        Me.LB = New DevExpress.XtraEditors.ListBoxControl()
        Me.CName = New DevExpress.XtraEditors.TextEdit()
        Me.CurrencyID = New DevExpress.XtraEditors.LookUpEdit()
        Me.DefaultCountry = New DevExpress.XtraEditors.CheckEdit()
        Me.Code = New DevExpress.XtraEditors.TextEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem4 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem5 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.LB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DefaultCountry.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.LayoutControl1.Controls.Add(Me.LB)
        Me.LayoutControl1.Controls.Add(Me.CName)
        Me.LayoutControl1.Controls.Add(Me.CurrencyID)
        Me.LayoutControl1.Controls.Add(Me.DefaultCountry)
        Me.LayoutControl1.Controls.Add(Me.Code)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 53)
        Me.LayoutControl1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(591, 425)
        Me.LayoutControl1.TabIndex = 4
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'LB
        '
        Me.LB.Location = New System.Drawing.Point(22, 210)
        Me.LB.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.LB.Name = "LB"
        Me.LB.Size = New System.Drawing.Size(547, 198)
        Me.LB.TabIndex = 5
        '
        'CName
        '
        Me.CName.Location = New System.Drawing.Point(22, 69)
        Me.CName.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CName.Name = "CName"
        Me.CName.Size = New System.Drawing.Size(424, 46)
        Me.CName.TabIndex = 4
        '
        'CurrencyID
        '
        Me.CurrencyID.Location = New System.Drawing.Point(22, 121)
        Me.CurrencyID.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.CurrencyID.Name = "CurrencyID"
        Me.CurrencyID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CurrencyID.Properties.NullText = ""
        Me.CurrencyID.Size = New System.Drawing.Size(424, 46)
        Me.CurrencyID.TabIndex = 6
        '
        'DefaultCountry
        '
        Me.DefaultCountry.Location = New System.Drawing.Point(22, 173)
        Me.DefaultCountry.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.DefaultCountry.Name = "DefaultCountry"
        Me.DefaultCountry.Properties.Caption = "دولة افتراضية"
        Me.DefaultCountry.Size = New System.Drawing.Size(407, 31)
        Me.DefaultCountry.TabIndex = 7
        '
        'Code
        '
        Me.Code.Enabled = False
        Me.Code.Location = New System.Drawing.Point(22, 17)
        Me.Code.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Code.Name = "Code"
        Me.Code.Size = New System.Drawing.Size(424, 46)
        Me.Code.TabIndex = 4
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItem3, Me.LayoutControlItem4, Me.LayoutControlItem5})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(591, 425)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.CName
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(555, 52)
        Me.LayoutControlItem1.Text = "الاسم"
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(101, 27)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.LB
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 193)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(555, 204)
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.CurrencyID
        Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 104)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.Size = New System.Drawing.Size(555, 52)
        Me.LayoutControlItem3.Text = "العملة الافتراضية"
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(101, 27)
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.DefaultCountry
        Me.LayoutControlItem4.Location = New System.Drawing.Point(0, 156)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.Padding = New DevExpress.XtraLayout.Utils.Padding(4, 144, 3, 3)
        Me.LayoutControlItem4.Size = New System.Drawing.Size(555, 37)
        Me.LayoutControlItem4.TextVisible = False
        '
        'LayoutControlItem5
        '
        Me.LayoutControlItem5.Control = Me.Code
        Me.LayoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem5.CustomizationFormText = "الاسم"
        Me.LayoutControlItem5.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem5.Name = "LayoutControlItem5"
        Me.LayoutControlItem5.Size = New System.Drawing.Size(555, 52)
        Me.LayoutControlItem5.Text = "الرمز"
        Me.LayoutControlItem5.TextSize = New System.Drawing.Size(101, 27)
        '
        'FRMCountries
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(591, 478)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.SvgImage = Global.ExchangeSystem.My.Resources.Resources.country
        Me.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRMCountries"
        Me.Text = "إضافة دولة"
        Me.Controls.SetChildIndex(Me.LayoutControl1, 0)
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.LB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CurrencyID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DefaultCountry.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents CName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LB As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents CurrencyID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents DefaultCountry As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem4 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LayoutControlItem5 As DevExpress.XtraLayout.LayoutControlItem
End Class
