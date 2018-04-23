using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraPrinting.Shape;

namespace E4755.Controllers {
    public class HomeController : Controller {
        

        public ActionResult Index() {
            return View();
        }

        public ActionResult Export() {
            var model = Product.GetProducts();

            MVCxGridViewState gridViewState = (MVCxGridViewState) Session["gridViewState"];

            if (gridViewState != null) {
                MVCReportGeneratonHelper generator = new MVCReportGeneratonHelper();
                generator.CustomizeColumnsCollection += new CustomizeColumnsCollectionEventHandler(generator_CustomizeColumnsCollection);
                generator.CustomizeColumn += new CustomizeColumnEventHandler(generator_CustomizeColumn);
                XtraReport report = generator.GenerateMVCReport(gridViewState, model);
                generator.WritePdfToResponse(Response, "test.pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
                return null;
            }
            else
                return View("Index");
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial() {
            var model = Product.GetProducts();
            return PartialView("_GridViewPartial", model);
        }

        void generator_CustomizeColumn(object source, ControlCustomizationEventArgs e) {
            if (e.FieldName == "Discontinued") {
                XRShape control = new XRShape();
                control.SizeF = e.Owner.SizeF;
                control.LocationF = new System.Drawing.PointF(0, 0);
                e.Owner.Controls.Add(control);
                control.Shape = new ShapeStar() {
                    StarPointCount = 5,
                    Concavity = 30
                };
                control.BeforePrint += new System.Drawing.Printing.PrintEventHandler(control_BeforePrint);
                e.IsModified = true;
            }
        }

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            if (Convert.ToBoolean(((XRShape) sender).Report.GetCurrentColumnValue("Discontinued")) == true)
                ((XRShape) sender).FillColor = Color.Yellow;
            else
                ((XRShape) sender).FillColor = Color.White;
        }

        void generator_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e) {
            e.ColumnsInfo[1].ColumnWidth *= 2;
            e.ColumnsInfo[4].ColumnWidth += 30;
            e.ColumnsInfo[3].ColumnWidth -= 30;
            e.ColumnsInfo[e.ColumnsInfo.Count - 1].IsVisible = false;
        }
        
    }
}