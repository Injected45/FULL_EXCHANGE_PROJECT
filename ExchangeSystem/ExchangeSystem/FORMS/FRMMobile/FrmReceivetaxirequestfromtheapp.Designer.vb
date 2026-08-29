<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReceivetaxirequestfromtheapp
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmReceivetaxirequestfromtheapp))
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.GVRole = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.SN = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Code = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ReceivedName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RPhone1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.AddressDescription = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.inserDate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OverallVal = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BTNok = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.OkBtn = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.cansel = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CanselBtn = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.Maplink = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.MAplinke = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.longitude = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Latitude = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.BName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Latitude_branchID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.longitude_branchID = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OkBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CanselBtn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MAplinke, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridControl1.Location = New System.Drawing.Point(0, 0)
        Me.GridControl1.MainView = Me.GVRole
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.OkBtn, Me.CanselBtn, Me.MAplinke})
        Me.GridControl1.Size = New System.Drawing.Size(1386, 407)
        Me.GridControl1.TabIndex = 1
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GVRole})
        '
        'GVRole
        '
        Me.GVRole.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.SN, Me.Code, Me.ReceivedName, Me.RPhone1, Me.AddressDescription, Me.inserDate, Me.OverallVal, Me.BTNok, Me.cansel, Me.Maplink, Me.longitude, Me.Latitude, Me.BName, Me.Latitude_branchID, Me.longitude_branchID})
        Me.GVRole.GridControl = Me.GridControl1
        Me.GVRole.Name = "GVRole"
        '
        'SN
        '
        Me.SN.Caption = "#"
        Me.SN.FieldName = "SN"
        Me.SN.Name = "SN"
        Me.SN.UnboundDataType = GetType(Integer)
        Me.SN.Visible = True
        Me.SN.VisibleIndex = 0
        Me.SN.Width = 64
        '
        'Code
        '
        Me.Code.Caption = "رمز الامانة"
        Me.Code.FieldName = "Code"
        Me.Code.Name = "Code"
        Me.Code.Visible = True
        Me.Code.VisibleIndex = 1
        Me.Code.Width = 226
        '
        'ReceivedName
        '
        Me.ReceivedName.Caption = "اسم المستلم "
        Me.ReceivedName.FieldName = "RecievedName"
        Me.ReceivedName.Name = "ReceivedName"
        Me.ReceivedName.Visible = True
        Me.ReceivedName.VisibleIndex = 2
        Me.ReceivedName.Width = 329
        '
        'RPhone1
        '
        Me.RPhone1.Caption = "هاتف المستلم"
        Me.RPhone1.FieldName = "RPhone1"
        Me.RPhone1.Name = "RPhone1"
        Me.RPhone1.Visible = True
        Me.RPhone1.VisibleIndex = 3
        Me.RPhone1.Width = 269
        '
        'AddressDescription
        '
        Me.AddressDescription.Caption = "العنوان"
        Me.AddressDescription.FieldName = "AddressDescription"
        Me.AddressDescription.Name = "AddressDescription"
        Me.AddressDescription.Width = 362
        '
        'inserDate
        '
        Me.inserDate.Caption = "تاريخ الطلب"
        Me.inserDate.FieldName = "InsertDate"
        Me.inserDate.Name = "inserDate"
        Me.inserDate.Visible = True
        Me.inserDate.VisibleIndex = 4
        Me.inserDate.Width = 153
        '
        'OverallVal
        '
        Me.OverallVal.Caption = "قيمة الحوالة"
        Me.OverallVal.FieldName = "OverallVal"
        Me.OverallVal.Name = "OverallVal"
        Me.OverallVal.Visible = True
        Me.OverallVal.VisibleIndex = 5
        Me.OverallVal.Width = 107
        '
        'BTNok
        '
        Me.BTNok.Caption = "قبول"
        Me.BTNok.ColumnEdit = Me.OkBtn
        Me.BTNok.FieldName = "BTNok"
        Me.BTNok.Name = "BTNok"
        Me.BTNok.Visible = True
        Me.BTNok.VisibleIndex = 6
        Me.BTNok.Width = 64
        '
        'OkBtn
        '
        Me.OkBtn.AutoHeight = False
        Me.OkBtn.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK)})
        Me.OkBtn.Name = "OkBtn"
        Me.OkBtn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'cansel
        '
        Me.cansel.Caption = "رفض"
        Me.cansel.ColumnEdit = Me.CanselBtn
        Me.cansel.FieldName = "cansel"
        Me.cansel.Name = "cansel"
        Me.cansel.Visible = True
        Me.cansel.VisibleIndex = 7
        Me.cansel.Width = 56
        '
        'CanselBtn
        '
        Me.CanselBtn.AutoHeight = False
        Me.CanselBtn.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)})
        Me.CanselBtn.Name = "CanselBtn"
        Me.CanselBtn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'Maplink
        '
        Me.Maplink.Caption = "عرض الموقع"
        Me.Maplink.ColumnEdit = Me.MAplinke
        Me.Maplink.FieldName = "Maplink"
        Me.Maplink.Name = "Maplink"
        Me.Maplink.Visible = True
        Me.Maplink.VisibleIndex = 8
        Me.Maplink.Width = 86
        '
        'MAplinke
        '
        Me.MAplinke.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = CType(resources.GetObject("EditorButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        EditorButtonImageOptions1.SvgImageSize = New System.Drawing.Size(20, 20)
        Me.MAplinke.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
        Me.MAplinke.Name = "MAplinke"
        Me.MAplinke.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
        '
        'longitude
        '
        Me.longitude.Caption = "longitude"
        Me.longitude.FieldName = "longitude"
        Me.longitude.Name = "longitude"
        '
        'Latitude
        '
        Me.Latitude.Caption = "Latitude"
        Me.Latitude.FieldName = "Latitude"
        Me.Latitude.Name = "Latitude"
        '
        'BName
        '
        Me.BName.Caption = "اسم الفرع"
        Me.BName.FieldName = "BName"
        Me.BName.Name = "BName"
        '
        'Latitude_branchID
        '
        Me.Latitude_branchID.Caption = "خط العرض الخاص بالفرع "
        Me.Latitude_branchID.FieldName = "Latitude_branchID"
        Me.Latitude_branchID.Name = "Latitude_branchID"
        '
        'longitude_branchID
        '
        Me.longitude_branchID.Caption = "خط الطول الخاص بالفرع"
        Me.longitude_branchID.FieldName = "longitude_branchID"
        Me.longitude_branchID.Name = "longitude_branchID"
        '
        'FrmReceivetaxirequestfromtheapp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1386, 407)
        Me.Controls.Add(Me.GridControl1)
        Me.Name = "FrmReceivetaxirequestfromtheapp"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.Text = "طلب توصيل داخلي غير معتمد"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GVRole, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OkBtn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CanselBtn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MAplinke, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GVRole As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents SN As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Code As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ReceivedName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RPhone1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents AddressDescription As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents inserDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BTNok As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OkBtn As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents cansel As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CanselBtn As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents Maplink As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents MAplinke As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents longitude As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Latitude As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Latitude_branchID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents longitude_branchID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents OverallVal As DevExpress.XtraGrid.Columns.GridColumn
End Class
