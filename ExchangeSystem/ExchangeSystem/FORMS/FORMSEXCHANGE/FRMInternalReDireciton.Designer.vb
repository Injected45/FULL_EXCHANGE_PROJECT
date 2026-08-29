<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMInternalReDireciton
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
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMInternalReDireciton))
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.GCROLE = New DevExpress.XtraGrid.GridControl()
        Me.GVROLE = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.RowHandle = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InsertDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SenderName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RecievedName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchDeliveredID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ExVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Redirection = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BtnRedirection = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.BranchRecievedID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BranchDeliveredIDID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.SimpleButton11 = New DevExpress.XtraEditors.SimpleButton()
        Me.BranchID = New DevExpress.XtraEditors.LookUpEdit()
        Me.Root = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem9 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem21 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.EmptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnRedirection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.GCROLE)
        Me.LayoutControl1.Controls.Add(Me.SimpleButton11)
        Me.LayoutControl1.Controls.Add(Me.BranchID)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.OptionsView.RightToLeftMirroringApplied = True
        Me.LayoutControl1.Root = Me.Root
        Me.LayoutControl1.Size = New System.Drawing.Size(1664, 606)
        Me.LayoutControl1.TabIndex = 0
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'GCROLE
        '
        Me.GCROLE.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GCROLE.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Location = New System.Drawing.Point(16, 60)
        Me.GCROLE.MainView = Me.GVROLE
        Me.GCROLE.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.GCROLE.Name = "GCROLE"
        Me.GCROLE.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.BtnRedirection})
        Me.GCROLE.Size = New System.Drawing.Size(1632, 530)
        Me.GCROLE.TabIndex = 4
        Me.GCROLE.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVROLE})
        '
        'GVROLE
        '
        Me.GVROLE.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.RowHandle, Me.Code, Me.InsertDate, Me.SenderName, Me.RecievedName, Me.BName, Me.BranchDeliveredID, Me.OverallVal, Me.ExVal, Me.Redirection, Me.BranchRecievedID, Me.BranchDeliveredIDID})
        Me.GVROLE.DetailHeight = 308
        Me.GVROLE.GridControl = Me.GCROLE
        Me.GVROLE.Name = "GVROLE"
        Me.GVROLE.OptionsBehavior.ReadOnly = True
        Me.GVROLE.OptionsFind.FindNullPrompt = "اضغط هنا للبحث ..."
        Me.GVROLE.OptionsView.ShowGroupPanel = False
        '
        'RowHandle
        '
        Me.RowHandle.AppearanceCell.Options.UseTextOptions = True
        Me.RowHandle.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RowHandle.AppearanceHeader.Options.UseTextOptions = True
        Me.RowHandle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RowHandle.Caption = "#"
        Me.RowHandle.FieldName = "RowHandle"
        Me.RowHandle.Name = "RowHandle"
        Me.RowHandle.UnboundDataType = GetType(Integer)
        Me.RowHandle.Visible = True
        Me.RowHandle.VisibleIndex = 0
        Me.RowHandle.Width = 45
        '
        'Code
        '
        Me.Code.AppearanceCell.Options.UseTextOptions = True
        Me.Code.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.AppearanceHeader.Options.UseTextOptions = True
        Me.Code.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Code.Caption = "الرمز"
        Me.Code.FieldName = "Code"
        Me.Code.MinWidth = 16
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 200
        '
        'InsertDate
        '
        Me.InsertDate.AppearanceCell.Options.UseTextOptions = True
        Me.InsertDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertDate.AppearanceHeader.Options.UseTextOptions = True
        Me.InsertDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.InsertDate.Caption = "التاريخ"
        Me.InsertDate.FieldName = "InsertDate"
        Me.InsertDate.MinWidth = 16
        Me.InsertDate.Name = "InsertDate"
        Me.InsertDate.Visible = True
        Me.InsertDate.VisibleIndex = 2
        Me.InsertDate.Width = 180
        '
        'SenderName
        '
        Me.SenderName.AppearanceCell.Options.UseTextOptions = True
        Me.SenderName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SenderName.AppearanceHeader.Options.UseTextOptions = True
        Me.SenderName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.SenderName.Caption = "اسم الراسل"
        Me.SenderName.FieldName = "SenderName"
        Me.SenderName.MinWidth = 16
        Me.SenderName.Name = "SenderName"
        Me.SenderName.Visible = True
        Me.SenderName.VisibleIndex = 3
        Me.SenderName.Width = 219
        '
        'RecievedName
        '
        Me.RecievedName.AppearanceCell.Options.UseTextOptions = True
        Me.RecievedName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RecievedName.AppearanceHeader.Options.UseTextOptions = True
        Me.RecievedName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.RecievedName.Caption = "اسم المستلم"
        Me.RecievedName.FieldName = "RecievedName"
        Me.RecievedName.MinWidth = 16
        Me.RecievedName.Name = "RecievedName"
        Me.RecievedName.Visible = True
        Me.RecievedName.VisibleIndex = 4
        Me.RecievedName.Width = 219
        '
        'BName
        '
        Me.BName.AppearanceCell.Options.UseTextOptions = True
        Me.BName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BName.AppearanceHeader.Options.UseTextOptions = True
        Me.BName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BName.Caption = "من"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        Me.BName.Visible = True
        Me.BName.VisibleIndex = 5
        Me.BName.Width = 165
        '
        'BranchDeliveredID
        '
        Me.BranchDeliveredID.AppearanceCell.Options.UseTextOptions = True
        Me.BranchDeliveredID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchDeliveredID.AppearanceHeader.Options.UseTextOptions = True
        Me.BranchDeliveredID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchDeliveredID.Caption = "إلى"
        Me.BranchDeliveredID.FieldName = "BranchDeliveredID"
        Me.BranchDeliveredID.Name = "BranchDeliveredID"
        Me.BranchDeliveredID.Visible = True
        Me.BranchDeliveredID.VisibleIndex = 6
        Me.BranchDeliveredID.Width = 197
        '
        'OverallVal
        '
        Me.OverallVal.AppearanceCell.Options.UseTextOptions = True
        Me.OverallVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallVal.AppearanceHeader.Options.UseTextOptions = True
        Me.OverallVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.OverallVal.Caption = "قيمة الحوالة"
        Me.OverallVal.FieldName = "OverallVal"
        Me.OverallVal.MinWidth = 16
        Me.OverallVal.Name = "OverallVal"
        Me.OverallVal.Visible = True
        Me.OverallVal.VisibleIndex = 7
        Me.OverallVal.Width = 160
        '
        'ExVal
        '
        Me.ExVal.AppearanceCell.Options.UseTextOptions = True
        Me.ExVal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExVal.AppearanceHeader.Options.UseTextOptions = True
        Me.ExVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.ExVal.Caption = "العمولة"
        Me.ExVal.FieldName = "ExVal"
        Me.ExVal.MinWidth = 16
        Me.ExVal.Name = "ExVal"
        Me.ExVal.Visible = True
        Me.ExVal.VisibleIndex = 8
        Me.ExVal.Width = 129
        '
        'Redirection
        '
        Me.Redirection.AppearanceCell.Options.UseTextOptions = True
        Me.Redirection.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Redirection.AppearanceHeader.Options.UseTextOptions = True
        Me.Redirection.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.Redirection.Caption = "اختيار"
        Me.Redirection.ColumnEdit = Me.BtnRedirection
        Me.Redirection.FieldName = "Redirection"
        Me.Redirection.Name = "Redirection"
        Me.Redirection.Visible = True
        Me.Redirection.VisibleIndex = 9
        Me.Redirection.Width = 86
        '
        'BtnRedirection
        '
        Me.BtnRedirection.AutoHeight = False
        EditorButtonImageOptions2.SvgImage = Global.ExchangeSystem.My.Resources.Resources.refresh
        EditorButtonImageOptions2.SvgImageSize = New System.Drawing.Size(16, 16)
        Me.BtnRedirection.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.BtnRedirection.Name = "BtnRedirection"
        Me.BtnRedirection.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'BranchRecievedID
        '
        Me.BranchRecievedID.Caption = "الفرع الراسل"
        Me.BranchRecievedID.FieldName = "BranchRecievedID"
        Me.BranchRecievedID.Name = "BranchRecievedID"
        '
        'BranchDeliveredIDID
        '
        Me.BranchDeliveredIDID.Caption = "الفرع المستلم"
        Me.BranchDeliveredIDID.FieldName = "BranchDeliveredIDID"
        Me.BranchDeliveredIDID.Name = "BranchDeliveredIDID"
        '
        'SimpleButton11
        '
        Me.SimpleButton11.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton11.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter
        Me.SimpleButton11.ImageOptions.SvgImage = CType(resources.GetObject("SimpleButton11.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        Me.SimpleButton11.Location = New System.Drawing.Point(1148, 16)
        Me.SimpleButton11.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SimpleButton11.Name = "SimpleButton11"
        Me.SimpleButton11.Size = New System.Drawing.Size(158, 38)
        Me.SimpleButton11.StyleController = Me.LayoutControl1
        Me.SimpleButton11.TabIndex = 6
        Me.SimpleButton11.Text = "عرض "
        '
        'BranchID
        '
        Me.BranchID.Location = New System.Drawing.Point(1312, 16)
        Me.BranchID.Name = "BranchID"
        Me.BranchID.Properties.Appearance.Options.UseTextOptions = True
        Me.BranchID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.AppearanceDisabled.Options.UseTextOptions = True
        Me.BranchID.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.AppearanceDropDown.Options.UseTextOptions = True
        Me.BranchID.Properties.AppearanceDropDown.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.AppearanceDropDown.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.AppearanceDropDownHeader.Options.UseTextOptions = True
        Me.BranchID.Properties.AppearanceDropDownHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.AppearanceDropDownHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.AppearanceFocused.Options.UseTextOptions = True
        Me.BranchID.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.AppearanceFocused.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.AppearanceReadOnly.Options.UseTextOptions = True
        Me.BranchID.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.BranchID.Properties.AppearanceReadOnly.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BranchID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.BranchID.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.BranchID.Properties.NullText = ""
        Me.BranchID.Properties.PopupSizeable = False
        Me.BranchID.Size = New System.Drawing.Size(296, 36)
        Me.BranchID.StyleController = Me.LayoutControl1
        Me.BranchID.TabIndex = 0
        '
        'Root
        '
        Me.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.Root.GroupBordersVisible = False
        Me.Root.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem9, Me.LayoutControlItem21, Me.EmptySpaceItem1})
        Me.Root.Name = "Root"
        Me.Root.Size = New System.Drawing.Size(1664, 606)
        Me.Root.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GCROLE
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 44)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(1638, 536)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem9
        '
        Me.LayoutControlItem9.Control = Me.SimpleButton11
        Me.LayoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem9.CustomizationFormText = "LayoutControlItem9"
        Me.LayoutControlItem9.Location = New System.Drawing.Point(1132, 0)
        Me.LayoutControlItem9.Name = "LayoutControlItem9"
        Me.LayoutControlItem9.Size = New System.Drawing.Size(164, 44)
        Me.LayoutControlItem9.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem9.TextVisible = False
        '
        'LayoutControlItem21
        '
        Me.LayoutControlItem21.AppearanceItemCaption.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!)
        Me.LayoutControlItem21.AppearanceItemCaption.Options.UseFont = True
        Me.LayoutControlItem21.AppearanceItemCaptionDisabled.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!)
        Me.LayoutControlItem21.AppearanceItemCaptionDisabled.Options.UseFont = True
        Me.LayoutControlItem21.Control = Me.BranchID
        Me.LayoutControlItem21.ControlAlignment = System.Drawing.ContentAlignment.TopRight
        Me.LayoutControlItem21.CustomizationFormText = "الفرع ال"
        Me.LayoutControlItem21.Location = New System.Drawing.Point(1296, 0)
        Me.LayoutControlItem21.Name = "LayoutControlItem21"
        Me.LayoutControlItem21.OptionsPrint.AppearanceItem.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!)
        Me.LayoutControlItem21.OptionsPrint.AppearanceItem.Options.UseFont = True
        Me.LayoutControlItem21.OptionsPrint.AppearanceItemControl.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!)
        Me.LayoutControlItem21.OptionsPrint.AppearanceItemControl.Options.UseFont = True
        Me.LayoutControlItem21.OptionsPrint.AppearanceItemText.Font = New System.Drawing.Font("Droid Arabic Kufi", 9.0!)
        Me.LayoutControlItem21.OptionsPrint.AppearanceItemText.Options.UseFont = True
        Me.LayoutControlItem21.Size = New System.Drawing.Size(342, 44)
        Me.LayoutControlItem21.Text = "الفرع"
        Me.LayoutControlItem21.TextSize = New System.Drawing.Size(24, 22)
        '
        'EmptySpaceItem1
        '
        Me.EmptySpaceItem1.AllowHotTrack = False
        Me.EmptySpaceItem1.Location = New System.Drawing.Point(0, 0)
        Me.EmptySpaceItem1.Name = "EmptySpaceItem1"
        Me.EmptySpaceItem1.Size = New System.Drawing.Size(1132, 44)
        Me.EmptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
        '
        'FRMInternalReDireciton
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1664, 606)
        Me.Controls.Add(Me.LayoutControl1)
        Me.IconOptions.Image = CType(resources.GetObject("FRMInternalReDireciton.IconOptions.Image"), System.Drawing.Image)
        Me.Name = "FRMInternalReDireciton"
        Me.Text = "إعادة توجيه حوالة"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.GCROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVROLE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnRedirection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BranchID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Root, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EmptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents GCROLE As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVROLE As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents RowHandle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents InsertDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents SenderName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RecievedName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BranchDeliveredID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ExVal As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Redirection As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BtnRedirection As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents BranchRecievedID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BranchDeliveredIDID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Root As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents SimpleButton11 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem9 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents BranchID As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents LayoutControlItem21 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents EmptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
End Class
