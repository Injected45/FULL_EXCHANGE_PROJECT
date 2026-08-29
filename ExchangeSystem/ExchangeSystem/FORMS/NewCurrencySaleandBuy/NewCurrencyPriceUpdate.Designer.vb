<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewCurrencyPriceUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewCurrencyPriceUpdate))
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.HtmlContentControl1 = New DevExpress.XtraEditors.HtmlContentControl()
        Me.PRTYPE = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.SBNBSale = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        CType(Me.HtmlContentControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PRTYPE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SBNBSale, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HtmlContentControl1
        '
        Me.HtmlContentControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.HtmlContentControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HtmlContentControl1.HtmlTemplate.Styles = resources.GetString("HtmlContentControl1.HtmlTemplate.Styles")
        Me.HtmlContentControl1.HtmlTemplate.Template = resources.GetString("HtmlContentControl1.HtmlTemplate.Template")
        Me.HtmlContentControl1.Location = New System.Drawing.Point(0, 0)
        Me.HtmlContentControl1.LookAndFeel.UseDefaultLookAndFeel = False
        Me.HtmlContentControl1.Name = "HtmlContentControl1"
        Me.HtmlContentControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.PRTYPE, Me.SBNBSale})
        Me.HtmlContentControl1.Size = New System.Drawing.Size(573, 591)
        Me.HtmlContentControl1.TabIndex = 0
        Me.HtmlContentControl1.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.[True]
        '
        'PRTYPE
        '
        Me.PRTYPE.AutoHeight = False
        Me.PRTYPE.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, True, True, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.PRTYPE.Items.AddRange(New Object() {"بيع وشراء داخلي", "بيع وشراء خاجي", "تحويلات خارجية", "المصرف"})
        Me.PRTYPE.Name = "PRTYPE"
        '
        'SBNBSale
        '
        Me.SBNBSale.AutoHeight = False
        Me.SBNBSale.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, True, False, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.SBNBSale.MaskSettings.Set("mask", "n3")
        Me.SBNBSale.Name = "SBNBSale"
        '
        'NewCurrencyPriceUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(573, 591)
        Me.ControlBox = False
        Me.Controls.Add(Me.HtmlContentControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "NewCurrencyPriceUpdate"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "NewCurrencyPriceUpdate"
        CType(Me.HtmlContentControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PRTYPE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SBNBSale, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents HtmlContentControl1 As DevExpress.XtraEditors.HtmlContentControl
    Friend WithEvents PRTYPE As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents SBNBSale As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
End Class
