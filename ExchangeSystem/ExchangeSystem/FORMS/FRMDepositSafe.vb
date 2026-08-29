
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class FRMDepositSafe
    Sub LOADBRANCH()
        Dim DT As New DataTable
        DT.Clear()
        BranchID.Properties.DataSource = Nothing
        DT.Clear()
        DT.NewRow()
        DT.Columns.Add("ID", GetType(Integer))
        DT.Columns.Add("BName", GetType(String))
        DT = RUN_QUARY_TXT("COBRANCHTB_LoadFORSAFES")
        DT.Rows.Add(0, "كل الفروع")
        If DT.Rows.Count > 0 Then
            BranchID.Properties.DataSource = DT
            BranchID.Properties.ValueMember = "ID"
            BranchID.Properties.DisplayMember = "BName"
            BranchID.Properties.PopulateColumns()
            BranchID.Properties.ShowHeader = False
        End If
    End Sub
    Public Sub FrmScreensTb_Details_UESIRID_GETFrom(userID As Integer, ScreenID As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@ueserID", SqlDbType.Int) With {.Value = userID}
        prm(1) = New SqlParameter("@ScreenID", SqlDbType.Int) With {.Value = ScreenID}
        Dim dt As New DataTable
        dt.Clear()
        dt = RUN_QUARY_PRO("FrmScreensTb_Details_UESIRID_GETFrom", prm)

        If dt.Rows.Count > 0 Then
            BranchID.Enabled = dt.Rows(0)("Can_branch")
            'SafeID.Enabled = dt.Rows(0)("Can_safID")
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        Else
            BranchID.Enabled = False
            'SafeID.Enabled = False
            'SafeID.EditValue = UserAccID
            BranchID.EditValue = BID
        End If
    End Sub

    Public Sub NOWRecored()
        LOADBRANCH()
        BranchID.EditValue = -1
        GridControl11.DataSource = Nothing
        If UserType = 1 Then
            BranchID.Enabled = True

        Else
            BranchID.Enabled = False


        End If
    End Sub

    Private Sub FRMCustomerAccountStatement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NOWRecored()
        FormLocation(Me)
        'Thread.CurrentThread.CurrentUICulture = CultureInfo
        LOADBRANCH()
        DT1.EditValue = Date.Now
        DT2.EditValue = Date.Now
        FrmScreensTb_Details_UESIRID_GETFrom(UserID, 53)
    End Sub


    Sub DVGFROMAT()

        GVRole.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
        GVRole.OptionsView.ShowGroupPanel = False
        GVRole.GroupPanelText = String.Empty
        GVRole.OptionsView.ShowFooter = False
        GVRole.Appearance.Row.Font = New Font("Droid Arabic Kufi", 10, FontStyle.Regular)
        For i As Integer = 0 To GVRole.Columns.Count - 1
            GVRole.Columns(i).AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GVRole.Columns(i).AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.HorzAlignment.Center
            'Thread.CurrentThread.CurrentUICulture = CultureInfo
        Next
    End Sub

    Public Sub loda()
        Try
            If DT1.EditValue > DT2.EditValue Then
                DT1.ErrorText = "لا يمكن ان يكون التاريخ الاول اكبر من التاريخ الثاني"
                DT1.Focus()
                Exit Sub
            End If

            If DT1.Text = String.Empty Then
                DT1.ErrorText = "يرجا اختيار التاريخ"
                DT1.Focus()
                Exit Sub
            End If
            If DT2.Text = String.Empty Then
                DT2.ErrorText = "يرجا اختيار التاريخ"
                DT2.Focus()
                Exit Sub
            End If
            Dim dt As New DataTable
            dt.Clear()
            dt = ACCOUNTSTB_select_for(BranchID.EditValue)
            If dt.Rows.Count > 0 Then
                GridControl11.DataSource = dt
                DVGFROMAT()
            Else
                GridControl11.DataSource = Nothing
                MessageBox.Show("لا يوجد بيانات لعرضها", "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            'Thread.CurrentThread.CurrentUICulture = CultureInfo
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رساله تنبية", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
    Public Function ACCOUNTSTB_select_for(BranchID As Integer) As DataTable
        Dim DT As New DataTable
        Dim PRM(2) As SqlParameter
        PRM(0) = New SqlParameter("@dat1", SqlDbType.Date) With {.Value = DT1.EditValue}
        PRM(1) = New SqlParameter("@date2", SqlDbType.Date) With {.Value = DT2.EditValue}
        PRM(2) = New SqlParameter("@BrnchID", SqlDbType.TinyInt) With {.Value = .Value = BranchID}
        DT.Clear()
        DT = RUN_QUARY_PRO("ACCOUNTSTB_select_forDepositSafe", PRM)
        Return DT
    End Function

    Private Sub GridControl1_DoubleClick(sender As Object, e As EventArgs) Handles GridControl11.DoubleClick
        Try
            If GVRole.RowCount > 0 Then
                Dim PRM(7) As SqlParameter
                PRM(0) = New SqlParameter("@ACCCODE", SqlDbType.Int) With {.Value = GVRole.GetFocusedRowCellValue("ACCID")}
                PRM(1) = New SqlParameter("@date1", SqlDbType.Date) With {.Value = DT1.EditValue}
                PRM(2) = New SqlParameter("@date2", SqlDbType.Date) With {.Value = DT2.EditValue}
                PRM(3) = New SqlParameter("@smblabecredetl", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(4) = New SqlParameter("@sumlabeldebit", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(5) = New SqlParameter("@lbealtotal", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(6) = New SqlParameter("@totalacount", SqlDbType.Float) With {.Direction = ParameterDirection.Output}
                PRM(7) = New SqlParameter("@CurrencyFrom", SqlDbType.Float) With {.Value = 1}
                Dim dt As New DataTable
                dt.Clear()
                dt = RUN_QUARY_PRO("[dbo].[ACCOUNTSTB_SelectICOUNTDetelles2]", PRM)
                If dt.Rows.Count > 0 Then
                    FRMaccounDEtells.ACCCODE.Tag = GVRole.GetFocusedRowCellValue("ACCID")
                    FRMaccounDEtells.DT1.EditValue = DT1.EditValue
                    FRMaccounDEtells.dt2.EditValue = DT2.EditValue
                    FRMaccounDEtells.loaddate(GVRole.GetFocusedRowCellValue("ACCID"), 1)
                Else
                    FRMaccounDEtells.ACCCODE.Tag = GVRole.GetFocusedRowCellValue("ACCID")
                    FRMaccounDEtells.ACCCODE.EditValue = GVRole.GetFocusedRowCellValue("acccode")
                    FRMaccounDEtells.ACCNAME.Text = GVRole.GetFocusedRowCellValue("ACCNAME")
                    FRMaccounDEtells.DT1.Select()
                End If
                If FRMaccounDEtells.Visible = True Then FRMaccounDEtells.Visible = False
                FRMaccounDEtells.Show()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "رسالة خطأ في برنامج", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BranchID_QueryPopUp(sender As Object, e As CancelEventArgs) Handles BranchID.QueryPopUp
        BranchID.Properties.PopulateColumns()
        BranchID.Properties.Columns("ID").Visible = False
    End Sub
End Class