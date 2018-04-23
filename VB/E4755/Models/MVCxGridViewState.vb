Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports DevExpress.Web.Mvc
Imports DevExpress.Web
Imports DevExpress.Data

Public Class MVCxColumnInfo
	Public Sub New(ByVal gridViewColumn As GridViewDataColumnState)
		Me.gridViewColumn_Renamed = gridViewColumn
	End Sub
	Private gridViewColumn_Renamed As GridViewDataColumnState

	Public ReadOnly Property GridViewColumn() As GridViewDataColumnState
		Get
			Return gridViewColumn_Renamed
		End Get
	End Property


	Private columnCaption_Renamed As String
	Public Property ColumnCaption() As String
		Get
			Return columnCaption_Renamed
		End Get
		Set(ByVal value As String)
			columnCaption_Renamed = value
		End Set
	End Property
	Private fieldName_Renamed As String

	Public Property FieldName() As String
		Get
			Return fieldName_Renamed
		End Get
		Set(ByVal value As String)
			fieldName_Renamed = value
		End Set
	End Property
	Private columnWidth_Renamed As Integer

	Public Property ColumnWidth() As Integer
		Get
			Return columnWidth_Renamed
		End Get
		Set(ByVal value As Integer)
			columnWidth_Renamed = value
		End Set
	End Property
	Private isVisible_Renamed As Boolean

	Public Property IsVisible() As Boolean
		Get
			Return isVisible_Renamed
		End Get
		Set(ByVal value As Boolean)
			isVisible_Renamed = value
		End Set
	End Property


End Class

Public Class MVCxGridViewState
	Public Sub New(ByVal gridView As MVCxGridView)
		Columns = New GridViewDataColumnStateCollection(gridView.VisibleColumns)
		GroupedColumns = New GridViewDataColumnStateCollection(gridView.GetGroupedColumns())
		TotalSummary = New System.Collections.ObjectModel.Collection(Of MVCxSummaryItemState)()
		For Each item As ASPxSummaryItem In gridView.TotalSummary
			TotalSummary.Add(New MVCxSummaryItemState(item))
		Next item
		FilterExpression = gridView.FilterExpression
	End Sub

	Private privateColumns As GridViewDataColumnStateCollection
	Public Property Columns() As GridViewDataColumnStateCollection
		Get
			Return privateColumns
		End Get
		Protected Set(ByVal value As GridViewDataColumnStateCollection)
			privateColumns = value
		End Set
	End Property

	Private privateGroupedColumns As GridViewDataColumnStateCollection
	Public Property GroupedColumns() As GridViewDataColumnStateCollection
		Get
			Return privateGroupedColumns
		End Get
		Protected Set(ByVal value As GridViewDataColumnStateCollection)
			privateGroupedColumns = value
		End Set
	End Property

	Private privateTotalSummary As System.Collections.ObjectModel.Collection(Of MVCxSummaryItemState)
	Public Property TotalSummary() As System.Collections.ObjectModel.Collection(Of MVCxSummaryItemState)
		Get
			Return privateTotalSummary
		End Get
		Protected Set(ByVal value As System.Collections.ObjectModel.Collection(Of MVCxSummaryItemState))
			privateTotalSummary = value
		End Set
	End Property

	Private privateFilterExpression As String
	Public Property FilterExpression() As String
		Get
			Return privateFilterExpression
		End Get
		Protected Set(ByVal value As String)
			privateFilterExpression = value
		End Set
	End Property
End Class

Public Class GridViewDataColumnState
	Public Sub New(ByVal column As GridViewDataColumn)
		Caption = column.Caption
		FieldName = column.FieldName
		SortOrder = column.SortOrder
	End Sub

	Private privateCaption As String
	Public Property Caption() As String
		Get
			Return privateCaption
		End Get
		Protected Set(ByVal value As String)
			privateCaption = value
		End Set
	End Property

	Private privateFieldName As String
	Public Property FieldName() As String
		Get
			Return privateFieldName
		End Get
		Protected Set(ByVal value As String)
			privateFieldName = value
		End Set
	End Property

	Private privateSortOrder As ColumnSortOrder
	Public Property SortOrder() As ColumnSortOrder
		Get
			Return privateSortOrder
		End Get
		Protected Set(ByVal value As ColumnSortOrder)
			privateSortOrder = value
		End Set
	End Property
End Class

Public Class GridViewDataColumnStateCollection
	Inherits System.Collections.ObjectModel.Collection(Of GridViewDataColumnState)
	Public Sub New(ByVal columns As ICollection(Of GridViewColumn))
		For Each col As GridViewColumn In columns
			Dim dataCol As GridViewDataColumn = TryCast(col, GridViewDataColumn)
			If dataCol IsNot Nothing Then
				Me.Add(New GridViewDataColumnState(dataCol))
			End If
		Next col
	End Sub

	Public Sub New(ByVal columns As ICollection(Of GridViewDataColumn))
		For Each col As GridViewDataColumn In columns
			Me.Add(New GridViewDataColumnState(col))
		Next col
	End Sub


    Default Public Overloads ReadOnly Property Item(ByVal fieldName As String) As GridViewDataColumnState
        Get
            Return Me.Single(Function(column) column.FieldName = fieldName)
        End Get
    End Property
End Class

Public Class MVCxSummaryItemState
	Public Sub New(ByVal item As ASPxSummaryItem)
		FieldName = item.FieldName
		ShowInColumn = item.ShowInColumn
		DisplayFormat = item.DisplayFormat
		SummaryType = item.SummaryType
	End Sub

	Private privateFieldName As String
	Public Property FieldName() As String
		Get
			Return privateFieldName
		End Get
		Protected Set(ByVal value As String)
			privateFieldName = value
		End Set
	End Property
	Private privateShowInColumn As String
	Public Property ShowInColumn() As String
		Get
			Return privateShowInColumn
		End Get
		Protected Set(ByVal value As String)
			privateShowInColumn = value
		End Set
	End Property
	Private privateDisplayFormat As String
	Public Property DisplayFormat() As String
		Get
			Return privateDisplayFormat
		End Get
		Protected Set(ByVal value As String)
			privateDisplayFormat = value
		End Set
	End Property
	Private privateSummaryType As SummaryItemType
	Public Property SummaryType() As SummaryItemType
		Get
			Return privateSummaryType
		End Get
		Protected Set(ByVal value As SummaryItemType)
			privateSummaryType = value
		End Set
	End Property
End Class