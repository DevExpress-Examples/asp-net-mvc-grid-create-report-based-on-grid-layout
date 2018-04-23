Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraReports.UI
Imports System.Drawing
Imports DevExpress.XtraPrinting.Shape

Namespace E4755.Controllers
	Public Class HomeController
		Inherits Controller


		Public Function Index() As ActionResult
			Return View()
		End Function

		Public Function Export() As ActionResult
			Dim model = Product.GetProducts()

			Dim gridViewState As MVCxGridViewState = CType(Session("gridViewState"), MVCxGridViewState)

			If gridViewState IsNot Nothing Then
				Dim generator As New MVCReportGeneratonHelper()
				AddHandler generator.CustomizeColumnsCollection, AddressOf generator_CustomizeColumnsCollection
				AddHandler generator.CustomizeColumn, AddressOf generator_CustomizeColumn
				Dim report As XtraReport = generator.GenerateMVCReport(gridViewState, model)
				generator.WritePdfToResponse(Response, "test.pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString())
				Return Nothing
			Else
				Return View("Index")
			End If
		End Function

		<ValidateInput(False)> _
		Public Function GridViewPartial() As ActionResult
			Dim model = Product.GetProducts()
			Return PartialView("_GridViewPartial", model)
		End Function

		Private Sub generator_CustomizeColumn(ByVal source As Object, ByVal e As ControlCustomizationEventArgs)
			If e.FieldName = "Discontinued" Then
				Dim control As New XRShape()
				control.SizeF = e.Owner.SizeF
				control.LocationF = New System.Drawing.PointF(0, 0)
				e.Owner.Controls.Add(control)
				control.Shape = New ShapeStar() With {.StarPointCount = 5, .Concavity = 30}
				AddHandler control.BeforePrint, AddressOf control_BeforePrint
				e.IsModified = True
			End If
		End Sub

		Private Sub control_BeforePrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs)
			If Convert.ToBoolean((CType(sender, XRShape)).Report.GetCurrentColumnValue("Discontinued")) = True Then
				CType(sender, XRShape).FillColor = Color.Yellow
			Else
				CType(sender, XRShape).FillColor = Color.White
			End If
		End Sub

		Private Sub generator_CustomizeColumnsCollection(ByVal source As Object, ByVal e As ColumnsCreationEventArgs)
			e.ColumnsInfo(1).ColumnWidth *= 2
			e.ColumnsInfo(4).ColumnWidth += 30
			e.ColumnsInfo(3).ColumnWidth -= 30
			e.ColumnsInfo(e.ColumnsInfo.Count - 1).IsVisible = False
		End Sub

	End Class
End Namespace