<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMaster
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMaster))
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        Me.BtnNew = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnSave = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnEdit = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnDelete = New DevExpress.XtraBars.BarButtonItem()
        Me.BtnPrint = New DevExpress.XtraBars.BarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.TNM = New DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager(Me.components)
        Me.ACWIN = New DevExpress.XtraBars.Alerter.AlertControl(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TNM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.BtnNew, Me.BtnSave, Me.BtnEdit, Me.BtnDelete, Me.BtnPrint})
        Me.BarManager1.MaxItemId = 5
        '
        'Bar1
        '
        Me.Bar1.BarAppearance.Disabled.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.Bar1.BarAppearance.Disabled.Options.UseBackColor = True
        Me.Bar1.BarAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(72, Byte), Integer), CType(CType(86, Byte), Integer))
        Me.Bar1.BarAppearance.Normal.Options.UseBackColor = True
        Me.Bar1.BarName = "Tools"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 0
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.BtnNew, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.BtnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.BtnEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.BtnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.BtnPrint, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)})
        Me.Bar1.OptionsBar.AllowQuickCustomization = False
        Me.Bar1.OptionsBar.DrawBorder = False
        Me.Bar1.Text = "Tools"
        '
        'BtnNew
        '
        Me.BtnNew.Caption = "جديد"
        Me.BtnNew.Id = 0
        Me.BtnNew.ImageOptions.SvgImage = CType(resources.GetObject("BtnNew.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnNew.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnNew.ItemAppearance.Normal.ForeColor = System.Drawing.Color.White
        Me.BtnNew.ItemAppearance.Normal.Options.UseBackColor = True
        Me.BtnNew.ItemAppearance.Normal.Options.UseForeColor = True
        Me.BtnNew.Name = "BtnNew"
        '
        'BtnSave
        '
        Me.BtnSave.Caption = "حفظ"
        Me.BtnSave.Id = 1
        Me.BtnSave.ImageOptions.SvgImage = CType(resources.GetObject("BtnSave.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnSave.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnSave.ItemAppearance.Normal.ForeColor = System.Drawing.Color.White
        Me.BtnSave.ItemAppearance.Normal.Options.UseBackColor = True
        Me.BtnSave.ItemAppearance.Normal.Options.UseForeColor = True
        Me.BtnSave.Name = "BtnSave"
        '
        'BtnEdit
        '
        Me.BtnEdit.Caption = "تعديل"
        Me.BtnEdit.Id = 2
        Me.BtnEdit.ImageOptions.SvgImage = CType(resources.GetObject("BtnEdit.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnEdit.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnEdit.ItemAppearance.Normal.ForeColor = System.Drawing.Color.White
        Me.BtnEdit.ItemAppearance.Normal.Options.UseBackColor = True
        Me.BtnEdit.ItemAppearance.Normal.Options.UseForeColor = True
        Me.BtnEdit.Name = "BtnEdit"
        '
        'BtnDelete
        '
        Me.BtnDelete.Caption = "حذف"
        Me.BtnDelete.Id = 3
        Me.BtnDelete.ImageOptions.SvgImage = CType(resources.GetObject("BtnDelete.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnDelete.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnDelete.ItemAppearance.Normal.ForeColor = System.Drawing.Color.White
        Me.BtnDelete.ItemAppearance.Normal.Options.UseBackColor = True
        Me.BtnDelete.ItemAppearance.Normal.Options.UseForeColor = True
        Me.BtnDelete.Name = "BtnDelete"
        '
        'BtnPrint
        '
        Me.BtnPrint.Caption = "طباعة"
        Me.BtnPrint.Id = 4
        Me.BtnPrint.ImageOptions.SvgImage = CType(resources.GetObject("BtnPrint.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.BtnPrint.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnPrint.ItemAppearance.Normal.ForeColor = System.Drawing.Color.White
        Me.BtnPrint.ItemAppearance.Normal.Options.UseBackColor = True
        Me.BtnPrint.ItemAppearance.Normal.Options.UseForeColor = True
        Me.BtnPrint.Name = "BtnPrint"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.barDockControlTop.Appearance.Options.UseBackColor = True
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(514, 43)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 375)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(514, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 43)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 332)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(514, 43)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 332)
        '
        'TNM
        '
        Me.TNM.ApplicationIconPath = ""
        Me.TNM.ApplicationId = "شركة الرحالة ألولى للشحن والتفريغ"
        Me.TNM.ApplicationName = ""
        Me.TNM.Notifications.AddRange(New DevExpress.XtraBars.ToastNotifications.IToastNotificationProperties() {New DevExpress.XtraBars.ToastNotifications.ToastNotification("bc5cdda8-44d8-45cf-b518-8fd104b5a7bd", CType(resources.GetObject("TNM.Notifications"), System.Drawing.Image), "رسالة تأكيد", "تم حفظ البيانات بنجاح", "نحن هنا", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.ImageAndText01), New DevExpress.XtraBars.ToastNotifications.ToastNotification("f111051c-1d51-4698-b1c8-5904e2f22485", CType(resources.GetObject("TNM.Notifications1"), System.Drawing.Image), "رسالة تأكيد", "تم حذف البيانات بنجاح", "", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.ImageAndText01)})
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'FrmMaster
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(514, 375)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.Font = New System.Drawing.Font("Hacen Tunisia", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.Name = "FrmMaster"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmMaster"
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TNM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents BtnNew As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnSave As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnEdit As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnDelete As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BtnPrint As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents TNM As DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager
    Friend WithEvents ACWIN As DevExpress.XtraBars.Alerter.AlertControl
    Friend WithEvents Timer1 As Timer
End Class
