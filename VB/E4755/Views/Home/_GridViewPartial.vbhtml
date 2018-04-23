@Imports DevExpress.Data
@Html.DevExpress().GridView(Sub(settings)
                                settings.Name = "GridView"
                                settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "GridViewPartial"}

                                settings.KeyFieldName = "ProductID"

                                settings.CommandColumn.Visible = True
                                settings.CommandColumn.ShowClearFilterButton = True

                                settings.Columns.Add("ProductID")
                                settings.Columns.Add("ProductName")
                                settings.Columns.Add("SupplierID")
                                settings.Columns.Add("CategoryID")
                                settings.Columns.Add("QuantityPerUnit")
                                settings.Columns.Add("UnitPrice")
                                settings.Columns.Add("UnitsInStock")
                                settings.Columns.Add("ReorderLevel")
                                settings.Columns.Add("Discontinued", MVCxGridViewColumnType.CheckBox)
                                settings.Columns.Add("EAN13")

                                settings.TotalSummary.Add(SummaryItemType.Sum, "UnitsInStock")
                                settings.TotalSummary.Add(SummaryItemType.Sum, "SupplierID")

                                settings.Settings.ShowFilterRow = True
                                settings.Settings.ShowFooter = True
                                settings.Settings.ShowGroupPanel = True

                                settings.PreRender = Sub(sender, e)
                                                         Dim gridView As MVCxGridView = sender
                                                         Session("gridViewState") = New MVCxGridViewState(gridView)
                                                     End Sub
                                settings.BeforeGetCallbackResult = settings.PreRender
                            End Sub
).Bind(Model).GetHtml()
