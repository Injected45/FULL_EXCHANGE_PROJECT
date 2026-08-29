<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMLOG
    Inherits DevExpress.XtraEditors.DirectXForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMLOG))
        Me.HtmlTemplateCollection1 = New DevExpress.Utils.Html.HtmlTemplateCollection()
        Me.HtmlTemplate1 = New DevExpress.Utils.Html.HtmlTemplate()
        Me.HtmlContentControl1 = New DevExpress.XtraEditors.HtmlContentControl()
        Me.DirectXFormContainerControl1 = New DevExpress.XtraEditors.DirectXFormContainerControl()
        CType(Me.HtmlContentControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DirectXFormContainerControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'HtmlTemplateCollection1
        '
        Me.HtmlTemplateCollection1.AddRange(New DevExpress.Utils.Html.HtmlTemplate() {Me.HtmlTemplate1})
        '
        'HtmlTemplate1
        '
        Me.HtmlTemplate1.Name = "HtmlTemplate1"
        Me.HtmlTemplate1.PreviewType = GetType(DevExpress.XtraEditors.DirectXForm)
        Me.HtmlTemplate1.Styles = resources.GetString("HtmlTemplate1.Styles")
        Me.HtmlTemplate1.Template = resources.GetString("HtmlTemplate1.Template")
        '
        'HtmlContentControl1
        '
        Me.HtmlContentControl1.Appearance.Options.UseTextOptions = True
        Me.HtmlContentControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.HtmlContentControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.HtmlContentControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.HtmlContentControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.HtmlContentControl1.HtmlTemplate.Styles = resources.GetString("HtmlContentControl1.HtmlTemplate.Styles")
        Me.HtmlContentControl1.HtmlTemplate.Template = resources.GetString("HtmlContentControl1.HtmlTemplate.Template")
        Me.HtmlContentControl1.Location = New System.Drawing.Point(0, 56)
        Me.HtmlContentControl1.Name = "HtmlContentControl1"
        Me.HtmlContentControl1.Size = New System.Drawing.Size(373, 394)
        Me.HtmlContentControl1.TabIndex = 0
        '
        'DirectXFormContainerControl1
        '
        Me.DirectXFormContainerControl1.Controls.Add(Me.HtmlContentControl1)
        Me.DirectXFormContainerControl1.Location = New System.Drawing.Point(0, 0)
        Me.DirectXFormContainerControl1.Name = "DirectXFormContainerControl1"
        Me.DirectXFormContainerControl1.Size = New System.Drawing.Size(373, 450)
        Me.DirectXFormContainerControl1.TabIndex = 0
        '
        'FRMLOG
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ChildControls.Add(Me.DirectXFormContainerControl1)
        Me.ClientSize = New System.Drawing.Size(373, 450)
        Me.ControlBox = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FRMLOG"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FRMLOG"
        CType(Me.HtmlContentControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DirectXFormContainerControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents HtmlTemplateCollection1 As DevExpress.Utils.Html.HtmlTemplateCollection
    Friend WithEvents HtmlTemplate1 As DevExpress.Utils.Html.HtmlTemplate
    Friend WithEvents HtmlContentControl1 As DevExpress.XtraEditors.HtmlContentControl
    Friend WithEvents DirectXFormContainerControl1 As DevExpress.XtraEditors.DirectXFormContainerControl
End Class
