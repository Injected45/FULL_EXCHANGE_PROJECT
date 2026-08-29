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
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.ViewInfo

Namespace ExchangeSystem
	Public Class MyGridPainter
		Inherits GridPainter


		Public Sub New(ByVal view As GridView)
			MyBase.New(view)

		End Sub

		Private _IsCustomPainting As Boolean
		Public Property IsCustomPainting() As Boolean
			Get
				Return _IsCustomPainting
			End Get
			Set(ByVal value As Boolean)
				_IsCustomPainting = value
			End Set
		End Property

		Public Sub DrawMergedCell(ByVal cell As MyMergedCell, ByVal e As PaintEventArgs)
			Dim delta As Integer = cell.Column1.VisibleIndex - cell.Column2.VisibleIndex
			If Math.Abs(delta) > 1 Then
				Return
			End If
			Dim vi As GridViewInfo = TryCast(View.GetViewInfo(), GridViewInfo)
			Dim gridCellInfo1 As GridCellInfo = vi.GetGridCellInfo(cell.RowHandle, cell.Column1)
			Dim gridCellInfo2 As GridCellInfo = vi.GetGridCellInfo(cell.RowHandle, cell.Column2)
			Dim gridCellInfo3 As GridCellInfo = vi.GetGridCellInfo(cell.RowHandle, cell.Column3)

			If gridCellInfo1 Is Nothing OrElse gridCellInfo2 Is Nothing Then
				Return
			End If
			Dim targetRect As Rectangle = Rectangle.Union(gridCellInfo1.Bounds, gridCellInfo2.Bounds)
			targetRect = Rectangle.Union(targetRect, gridCellInfo3.Bounds)

			gridCellInfo1.Bounds = targetRect
			gridCellInfo1.CellValueRect = targetRect
			gridCellInfo2.Bounds = targetRect
			gridCellInfo2.CellValueRect = targetRect
			If delta < 0 Then
				gridCellInfo1 = gridCellInfo2
			End If
			Dim bounds As Rectangle = gridCellInfo1.ViewInfo.Bounds
			bounds.Width = targetRect.Width
			bounds.Height = targetRect.Height
			gridCellInfo1.ViewInfo.Bounds = bounds
			gridCellInfo1.ViewInfo.CalcViewInfo(e.Graphics)
			IsCustomPainting = True
			Dim cache As New GraphicsCache(e.Graphics)
			gridCellInfo1.Appearance.FillRectangle(cache, gridCellInfo1.Bounds)
			DrawRowCell(New GridViewDrawArgs(cache, vi, vi.ViewRects.Bounds), gridCellInfo1)
			IsCustomPainting = False

		End Sub

	End Class

End Namespace
