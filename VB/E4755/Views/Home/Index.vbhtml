@Code
    ViewBag.Title = "Home Page"
End Code
@Using Html.BeginForm("Export", "Home")
       
    @Html.DevExpress().Button(Sub(settings)
                                  settings.Name = "btExport"
                                  settings.Text = "Export to PDF via XtraReport"
                                  settings.UseSubmitBehavior = True
                              End Sub
).GetHtml()    
    
    @Html.Action("GridViewPartial")
End Using
