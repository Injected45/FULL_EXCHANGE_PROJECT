' Developer Express Code Central Example:
' How to merge cells horizontally in GridView
' 
' This example demonstrates how to merge cells located in the same row. The main
' idea is to paint merged cell manually.
' You can find a helper class in this
' example, which can be easily connected to your existing GridView.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E2472


Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid.Drawing
Imports DevExpress.XtraGrid.Columns


Namespace ExchangeSystem
    Public Class MyCellMergeHelper
        Private _MergedCells As New List(Of MyMergedCell)()
        Public ReadOnly Property MergedCells() As List(Of MyMergedCell)
            Get
                Return _MergedCells
            End Get
        End Property

        Private painter As MyGridPainter


        Private _view As GridView

        Public Sub New(ByVal view As GridView)
            _view = view
            AddHandler view.CustomDrawCell, AddressOf view_CustomDrawCell
            AddHandler view.GridControl.Paint, AddressOf GridControl_Paint
            AddHandler view.CellValueChanged, AddressOf view_CellValueChanged
            painter = New MyGridPainter(view)
        End Sub

        Public Function AddMergedCell(ByVal rowHandle As Integer, ByVal col1 As GridColumn, ByVal col2 As GridColumn, ByVal col3 As GridColumn) As MyMergedCell
            Dim cell As New MyMergedCell(rowHandle, col1, col2, col3)
            _MergedCells.Add(cell)
            Return cell
        End Function
        Public Sub AddMergedCell(ByVal rowHandle As Integer, ByVal col1 As Integer, ByVal col2 As Integer, ByVal col3 As Integer, ByVal value As Object)
            AddMergedCell(rowHandle, _view.Columns(col1), _view.Columns(col2), _view.Columns(col3))
        End Sub

        Public Sub AddMergedCell(ByVal rowHandle As Integer, ByVal col1 As GridColumn, ByVal col2 As GridColumn, ByVal col3 As GridColumn, ByVal value As Object)
            Dim cell As MyMergedCell = AddMergedCell(rowHandle, col1, col2, col3)
            SafeSetMergedCellValue(cell, value)
        End Sub



        Public Sub SafeSetMergedCellValue(ByVal cell As MyMergedCell, ByVal value As Object)
            If cell IsNot Nothing Then
                SafeSetCellValue(cell.RowHandle, cell.Column1, value)
                SafeSetCellValue(cell.RowHandle, cell.Column2, value)
            End If
        End Sub

        Public Sub SafeSetCellValue(ByVal rowHandle As Integer, ByVal column As GridColumn, ByVal value As Object)
            If _view.GetRowCellValue(rowHandle, column) IsNot value Then
                _view.SetRowCellValue(rowHandle, column, value)
            End If
        End Sub


        Private Function GetMergedCell(ByVal rowHandle As Integer, ByVal column As GridColumn) As MyMergedCell
            For Each cell As MyMergedCell In _MergedCells
                If cell.RowHandle = rowHandle AndAlso (column Is cell.Column1 OrElse column Is cell.Column2) Then
                    Return cell
                End If
            Next cell
            Return Nothing
        End Function

        Private Function IsMergedCell(ByVal rowHandle As Integer, ByVal column As GridColumn) As Boolean
            Return GetMergedCell(rowHandle, column) IsNot Nothing
        End Function

        Private Sub DrawMergedCells(ByVal e As PaintEventArgs)
            For Each cell As MyMergedCell In _MergedCells
                painter.DrawMergedCell(cell, e)
            Next cell
        End Sub


        Private Sub view_CellValueChanged(ByVal sender As Object, ByVal e As CellValueChangedEventArgs)
            SafeSetMergedCellValue(GetMergedCell(e.RowHandle, e.Column), e.Value)
        End Sub

        Private Sub GridControl_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
            DrawMergedCells(e)
        End Sub

        Private Sub view_CustomDrawCell(ByVal sender As Object, ByVal e As RowCellCustomDrawEventArgs)
            If IsMergedCell(e.RowHandle, e.Column) Then
                e.Handled = Not painter.IsCustomPainting
            End If
        End Sub

    End Class
End Namespace