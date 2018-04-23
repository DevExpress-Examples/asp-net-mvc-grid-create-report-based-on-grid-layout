Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data

Public Class Product
	Private privateProductID As Integer
	Public Property ProductID() As Integer
		Get
			Return privateProductID
		End Get
		Protected Set(ByVal value As Integer)
			privateProductID = value
		End Set
	End Property

	Private privateProductName As String
	Public Property ProductName() As String
		Get
			Return privateProductName
		End Get
		Set(ByVal value As String)
			privateProductName = value
		End Set
	End Property

	Private privateSupplierID As Integer
	Public Property SupplierID() As Integer
		Get
			Return privateSupplierID
		End Get
		Set(ByVal value As Integer)
			privateSupplierID = value
		End Set
	End Property

	Private privateCategoryID As Integer
	Public Property CategoryID() As Integer
		Get
			Return privateCategoryID
		End Get
		Set(ByVal value As Integer)
			privateCategoryID = value
		End Set
	End Property

	Private privateQuantityPerUnit As String
	Public Property QuantityPerUnit() As String
		Get
			Return privateQuantityPerUnit
		End Get
		Set(ByVal value As String)
			privateQuantityPerUnit = value
		End Set
	End Property

	Private privateUnitPrice As Decimal
	Public Property UnitPrice() As Decimal
		Get
			Return privateUnitPrice
		End Get
		Set(ByVal value As Decimal)
			privateUnitPrice = value
		End Set
	End Property

	Private privateUnitsInStock As Short
	Public Property UnitsInStock() As Short
		Get
			Return privateUnitsInStock
		End Get
		Set(ByVal value As Short)
			privateUnitsInStock = value
		End Set
	End Property

	Private privateUnitsOnOrder As Short
	Public Property UnitsOnOrder() As Short
		Get
			Return privateUnitsOnOrder
		End Get
		Set(ByVal value As Short)
			privateUnitsOnOrder = value
		End Set
	End Property

	Private privateReorderLevel As Short
	Public Property ReorderLevel() As Short
		Get
			Return privateReorderLevel
		End Get
		Set(ByVal value As Short)
			privateReorderLevel = value
		End Set
	End Property

	Private privateDiscontinued As Boolean
	Public Property Discontinued() As Boolean
		Get
			Return privateDiscontinued
		End Get
		Set(ByVal value As Boolean)
			privateDiscontinued = value
		End Set
	End Property

	Private privateEAN13 As String
	Public Property EAN13() As String
		Get
			Return privateEAN13
		End Get
		Set(ByVal value As String)
			privateEAN13 = value
		End Set
	End Property

	Public Shared Function GetProducts() As List(Of Product)
		Dim prodData As DataTable = DataHelper.ProcessSelectCommand("SELECT * FROM [Products]")
		If prodData IsNot Nothing Then
			Dim products As New List(Of Product)()
			For Each row As DataRow In prodData.Rows
				Dim product As New Product() With {.ProductID = CInt(Fix(row("ProductID"))), .ProductName = CStr(row("ProductName")), .SupplierID = CInt(Fix(row("SupplierID"))), .CategoryID = CInt(Fix(row("CategoryID"))), .QuantityPerUnit = CStr(row("QuantityPerUnit")), .UnitPrice = CDec(row("UnitPrice")), .UnitsInStock = CShort(Fix(row("UnitsInStock"))), .UnitsOnOrder = CShort(Fix(row("UnitsOnOrder"))), .ReorderLevel = CShort(Fix(row("ReorderLevel"))), .Discontinued = CBool(row("Discontinued")), .EAN13 = CStr(row("EAN13"))}
				products.Add(product)
			Next row
			Return products
		End If
		Return Nothing


	End Function
End Class