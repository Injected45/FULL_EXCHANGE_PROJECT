Imports System.Data.SqlClient
Public Class CLSAccount
    Public Function ACCOUNTSTB_SelectMax(Code As Decimal, ACCTYPE As Integer) As DataTable
        Dim PRM(1) As SqlParameter
        PRM(0) = New SqlParameter("@fatherParent", SqlDbType.Decimal, 18, 0)
        PRM(0).Value = Code
        PRM(1) = New SqlParameter("@AccType", SqlDbType.Int)
        PRM(1).Value = ACCTYPE
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("ACCOUNTSTB_selectmax", PRM)
        Return DT
    End Function
    Public Sub ACCOUNTSTB_insert(ACCCODE As ULong, ACCNAME As String, ACCTYPE As Integer, FATHERPERINT As Decimal, ACCDMTYPE As Integer, ACCFINAL As Integer, ACCPHONE As String, ACCPHONE2 As String, ACCMAIL As String, ACCADDRESS As String,
                                 ACCNOTES As String, ACCMAX As Integer, ACCMIN As Integer, ACCBRANCH As Integer, Accline As Integer, IDCode As ULong)
        Dim prm(15) As SqlParameter
        prm(0) = New SqlParameter("@AccCode", SqlDbType.BigInt)
        prm(0).Value = ACCCODE
        prm(1) = New SqlParameter("@AccName", SqlDbType.VarChar, 80)
        prm(1).Value = ACCNAME
        prm(2) = New SqlParameter("@AccType", SqlDbType.TinyInt)
        prm(2).Value = ACCTYPE
        prm(3) = New SqlParameter("@AccParent", SqlDbType.Decimal, 18, 0)
        prm(3).Value = FATHERPERINT
        prm(4) = New SqlParameter("@AccDmType", SqlDbType.TinyInt)
        prm(4).Value = ACCDMTYPE
        prm(5) = New SqlParameter("@AccFinal", SqlDbType.TinyInt)
        prm(5).Value = ACCFINAL
        prm(6) = New SqlParameter("@AccPhone", SqlDbType.VarChar, 20)
        prm(6).Value = ACCPHONE
        prm(7) = New SqlParameter("@AccMobile", SqlDbType.VarChar, 20)
        prm(7).Value = ACCPHONE2
        prm(8) = New SqlParameter("@AccEmail", SqlDbType.VarChar, 80)
        prm(8).Value = ACCMAIL
        prm(9) = New SqlParameter("@AccAddress", SqlDbType.VarChar, -1)
        prm(9).Value = ACCADDRESS
        prm(10) = New SqlParameter("@AccNotes", SqlDbType.VarChar, -1)
        prm(10).Value = ACCNOTES
        prm(11) = New SqlParameter("@AccMaxLimit", SqlDbType.Int)
        prm(11).Value = ACCMAX
        prm(12) = New SqlParameter("@AccMaxDuration", SqlDbType.SmallInt)
        prm(12).Value = ACCMIN
        prm(13) = New SqlParameter("@BranchID", SqlDbType.Int)
        prm(13).Value = ACCBRANCH
        prm(14) = New SqlParameter("@Accline", SqlDbType.Int)
        prm(14).Value = Accline
        prm(15) = New SqlParameter("@IDcode", SqlDbType.BigInt)
        prm(15).Value = IDCode
        RUN_EXUTE_PRO("ACCOUNTSTB_insert", prm)
    End Sub
    Public Sub ACCOUNTSTB_update(ACCCODE As ULong, ACCNAME As String, ACCTYPE As Integer, FATHERPERINT As Decimal, ACCDMTYPE As Integer, ACCFINAL As Integer, ACCPHONE As String, ACCPHONE2 As String, ACCMAIL As String, ACCADDRESS As String,
                                 ACCNOTES As String, ACCMAX As Integer, ACCMIN As Integer, ACCBRANCH As Integer, ID As Integer, Accline As Integer, AccActive As Boolean, IDCode As ULong)
        Dim prm(17) As SqlParameter
        prm(0) = New SqlParameter("@AccCode", SqlDbType.BigInt)
        prm(0).Value = ACCCODE
        prm(1) = New SqlParameter("@AccName", SqlDbType.VarChar, 80)
        prm(1).Value = ACCNAME
        prm(2) = New SqlParameter("@AccType", SqlDbType.TinyInt)
        prm(2).Value = ACCTYPE
        prm(3) = New SqlParameter("@AccParent", SqlDbType.Decimal, 18, 0)
        prm(3).Value = FATHERPERINT
        prm(4) = New SqlParameter("@AccDmType", SqlDbType.TinyInt)
        prm(4).Value = ACCDMTYPE
        prm(5) = New SqlParameter("@AccFinal", SqlDbType.TinyInt)
        prm(5).Value = ACCFINAL
        prm(6) = New SqlParameter("@AccPhone", SqlDbType.VarChar, 20)
        prm(6).Value = ACCPHONE
        prm(7) = New SqlParameter("@AccMobile", SqlDbType.VarChar, 20)
        prm(7).Value = ACCPHONE2
        prm(8) = New SqlParameter("@AccEmail", SqlDbType.VarChar, 80)
        prm(8).Value = ACCMAIL
        prm(9) = New SqlParameter("@AccAddress", SqlDbType.VarChar, -1)
        prm(9).Value = ACCADDRESS
        prm(10) = New SqlParameter("@AccNotes", SqlDbType.VarChar, -1)
        prm(10).Value = ACCNOTES
        prm(11) = New SqlParameter("@AccMaxLimit", SqlDbType.Int)
        prm(11).Value = ACCMAX
        prm(12) = New SqlParameter("@AccMaxDuration", SqlDbType.SmallInt)
        prm(12).Value = ACCMIN
        prm(13) = New SqlParameter("@BranchID", SqlDbType.Int)
        prm(13).Value = ACCBRANCH
        prm(14) = New SqlParameter("@ID", SqlDbType.Int)
        prm(14).Value = ID
        prm(15) = New SqlParameter("@Accline", SqlDbType.Int)
        prm(15).Value = Accline
        prm(16) = New SqlParameter("@AccActive", SqlDbType.Bit)
        prm(16).Value = AccActive
        prm(17) = New SqlParameter("@IDcode", SqlDbType.BigInt)
        prm(17).Value = IDCode
        RUN_EXUTE_PRO("ACCOUNTSTB_update", prm)
    End Sub
    Public Sub ACCOUNTSTB_delete(id As Integer, userupdate As Integer)
        Dim prm(1) As SqlParameter
        prm(0) = New SqlParameter("@id", SqlDbType.Int)
        prm(0).Value = id
        prm(1) = New SqlParameter("@useupdate", SqlDbType.Int)
        prm(1).Value = userupdate
        RUN_EXUTE_PRO("ACCOUNTSTB_delete", prm)
    End Sub
    Public Function ACCOUNTSTB_LoadAccParent() As DataTable
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_TXT("ACCOUNTSTB_LoadAccParent")
        Return DT
    End Function

    Dim dtfilltree As New DataTable
    Public Sub FIll_tree_view(key As String, txt As String, n As TreeNode, tv As TreeView, imegindex As Integer)

        Dim nn As TreeNode

        If n Is Nothing Then
            nn = tv.Nodes.Add(key, txt, 31, 31)
        Else
            nn = n.Nodes.Add(key, txt, imegindex, imegindex + 3)
        End If

        nn.Tag = key

        Dim dv As DataView = dtfilltree.DefaultView
        Dim xkey As Int64 = Convert.ToInt64(key)
        dv.RowFilter = "AccParent='" & xkey & "'"

        For Each dr As DataRow In dv.ToTable.Rows

            Dim accCode As String = Convert.ToString(dr("acccode"))
            Dim accName As String = Convert.ToString(dr("AccName"))
            Dim accType As Integer = If(IsDBNull(dr("AccType")), 0, Convert.ToInt32(dr("AccType")))

            FIll_tree_view(accCode, accName, nn, tv, accType)

        Next

    End Sub

    Public Sub Load_Tree()
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_TXT("ACCOUNTSTB_TReevive")
        'For Each row As DataRow In DT.Rows
        '    Dim child As Decimal
        '    child = row("AccParent")

        '    Dim PR(0) As SqlParameter
        '    PR(0) = New SqlParameter("@ParentID", SqlDbType.Decimal) With {.Value = child}
        'dtfilltree = RUN_QUARY_PRO("ACCOUNTSTB_TReeviveBasedonparent", PR)
        dtfilltree = RUN_QUARY_TXT("ACCOUNTSTB_TReevive")
        FrmAccountsTree.TreeView1.BeginUpdate()
        FrmAccountsTree.TreeView1.Nodes.Clear()
        FIll_tree_view("0", "شجرة الدليل المحاسبي", Nothing, FrmAccountsTree.TreeView1, 4)
        FrmAccountsTree.TreeView1.TopNode.Expand()
        FrmAccountsTree.TreeView1.TopNode.NodeFont = New Font("Droid Arabic Kufi", 9, FontStyle.Bold)
        FrmAccountsTree.TreeView1.TopNode.ForeColor = Color.Blue
        FrmAccountsTree.TreeView1.Select()
        FrmAccountsTree.TreeView1.EndUpdate()
        'Next
    End Sub
    Public Sub PopulateTreeView(dtParent As DataTable, parentId As Integer, treeNode As TreeNode)
        For Each row As DataRow In dtParent.Rows
            Dim child As New TreeNode() With {
         .Text = row("AccName").ToString(),
         .Tag = row("AccID")
        }
            If parentId = 0 Then
                FrmAccountsTree.TreeView1.Nodes.Add(child)
                Dim xkey As Int64 = Convert.ToInt64(child.Tag)
                Dim rp(0) As SqlParameter
                rp(0) = New SqlParameter("@ParentID", SqlDbType.BigInt) With {.Value = xkey}
                Dim dtChild As DataTable = RUN_QUARY_PRO("ACCOUNTSTB_TReeviveBasedonparent", rp)
                PopulateTreeView(dtChild, xkey, child)
            Else
                treeNode.Nodes.Add(child)
            End If
        Next
    End Sub
    Public Function ACCOUNTSTB_selectByCode(Code As ULong) As DataTable
        Dim PRM(0) As SqlParameter
        PRM(0) = New SqlParameter("@AccCode", SqlDbType.BigInt)
        PRM(0).Value = Code
        Dim DT As New DataTable
        DT.Clear()
        DT = RUN_QUARY_PRO("AccountsTb_SelectByCode", PRM)
        Return DT
    End Function
    Public Sub ACCOUNTTREESTB_insert(ACCCODE As ULong, ACCNAME As String, ACCTYPE As Integer, FATHERPERINT As Decimal, ACCDMTYPE As Integer, ACCFINAL As Integer, ACCPHONE As String, ACCPHONE2 As String, ACCMAIL As String, ACCADDRESS As String,
                                ACCNOTES As String, ACCMAX As Integer, ACCMIN As Integer, ACCBRANCH As Integer, Accline As Integer, IDCode As ULong)
        Dim prm(15) As SqlParameter
        prm(0) = New SqlParameter("@AccCode", SqlDbType.BigInt)
        prm(0).Value = ACCCODE
        prm(1) = New SqlParameter("@AccName", SqlDbType.VarChar, 80)
        prm(1).Value = ACCNAME
        prm(2) = New SqlParameter("@AccType", SqlDbType.TinyInt)
        prm(2).Value = ACCTYPE
        prm(3) = New SqlParameter("@AccParent", SqlDbType.Decimal, 18, 0)
        prm(3).Value = FATHERPERINT
        prm(4) = New SqlParameter("@AccDmType", SqlDbType.TinyInt)
        prm(4).Value = ACCDMTYPE
        prm(5) = New SqlParameter("@AccFinal", SqlDbType.TinyInt)
        prm(5).Value = ACCFINAL
        prm(6) = New SqlParameter("@AccPhone", SqlDbType.VarChar, 20)
        prm(6).Value = ACCPHONE
        prm(7) = New SqlParameter("@AccMobile", SqlDbType.VarChar, 20)
        prm(7).Value = ACCPHONE2
        prm(8) = New SqlParameter("@AccEmail", SqlDbType.VarChar, 80)
        prm(8).Value = ACCMAIL
        prm(9) = New SqlParameter("@AccAddress", SqlDbType.VarChar, -1)
        prm(9).Value = ACCADDRESS
        prm(10) = New SqlParameter("@AccNotes", SqlDbType.VarChar, -1)
        prm(10).Value = ACCNOTES
        prm(11) = New SqlParameter("@AccMaxLimit", SqlDbType.Int)
        prm(11).Value = ACCMAX
        prm(12) = New SqlParameter("@AccMaxDuration", SqlDbType.SmallInt)
        prm(12).Value = ACCMIN
        prm(13) = New SqlParameter("@BranchID", SqlDbType.Int)
        prm(13).Value = ACCBRANCH
        prm(14) = New SqlParameter("@Accline", SqlDbType.Int)
        prm(14).Value = Accline
        prm(15) = New SqlParameter("@IDcode", SqlDbType.BigInt)
        prm(15).Value = IDCode
        RUN_EXUTE_PRO("ACCOUNTTEESTB_insert", prm)
    End Sub
    Public Sub ACCOUNTSTB_UPDATECHANGEDBRANCH(ACCCODE As ULong, ACCNAME As String, FATHERPERINT As Decimal, ACCBRANCH As Integer, IDCode As ULong)
        Dim prm(17) As SqlParameter
        prm(0) = New SqlParameter("@AccCode", SqlDbType.BigInt) With {.Value = ACCCODE}
        prm(1) = New SqlParameter("@AccName", SqlDbType.VarChar, 80) With {.Value = ACCNAME}
        prm(3) = New SqlParameter("@AccParent", SqlDbType.Decimal, 18, 0) With {.Value = FATHERPERINT}
        prm(13) = New SqlParameter("@BranchID", SqlDbType.Int) With {.Value = ACCBRANCH}
        prm(13).Value = ACCBRANCH
        prm(17) = New SqlParameter("@IDcode", SqlDbType.BigInt) With {.Value = IDCode}
        RUN_EXUTE_PRO("ACCOUNTSTB_update", prm)
    End Sub
End Class
