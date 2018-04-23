using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;
using System.Collections;
using System.Drawing;
using DevExpress.Data;
using DevExpress.XtraPrinting;
using System.IO;
using System.Net.Mime;

public delegate void CustomizeColumnsCollectionEventHandler(object source, ColumnsCreationEventArgs e);
public delegate void CustomizeColumnEventHandler(object source, ControlCustomizationEventArgs e);

public class MVCReportGeneratonHelper {
    XtraReport report;
    const int initialGroupOffset = 0;
    const int subGroupOffset = 10;
    const int bandHeight = 20;
    const bool shouldRepeatGroupHeadersOnEveryPage = false;
    Hashtable detailsInfo = new Hashtable();

    public event CustomizeColumnsCollectionEventHandler CustomizeColumnsCollection;
    public event CustomizeColumnEventHandler CustomizeColumn;


    public XtraReport GenerateMVCReport(MVCxGridViewState gridViewState, object model) {
        report = new XtraReport();
        report.Landscape = true;
        report.PaperKind = System.Drawing.Printing.PaperKind.Letter;

        InitDataSource(model);
        InitDetailsAndPageHeader(gridViewState);
        InitSortings(gridViewState);
        InitGroupHeaders(gridViewState);
        InitFilters(gridViewState);
        InitTotalSummaries(gridViewState);
        return report;
    }

    void InitTotalSummaries(MVCxGridViewState gridViewState) {
        if (gridViewState.TotalSummary.Count > 0) {
            report.Bands.Add(new ReportFooterBand() {
                HeightF = bandHeight
            });
            foreach (MVCxSummaryItemState item in gridViewState.TotalSummary) {
                GridViewDataColumnState col = gridViewState.Columns[item.ShowInColumn == string.Empty ? item.FieldName : item.ShowInColumn];
                if (col != null) {
                    if (detailsInfo.Contains(col)) {
                        XRLabel label = new XRLabel();
                        label.LocationF = ((XRTableCell) detailsInfo[col]).LocationF;
                        label.SizeF = ((XRTableCell) detailsInfo[col]).SizeF;
                        label.DataBindings.Add("Text", null, col.FieldName);
                        label.Summary = new XRSummary() {
                            Running = SummaryRunning.Report
                        };
                        label.Summary.FormatString = item.DisplayFormat;
                        label.Summary.Func = GetSummaryFunc(item.SummaryType);
                        report.Bands[BandKind.ReportFooter].Controls.Add(label);
                    }
                }
            }
        }
    }

    void InitDataSource(object model) {
        report.DataSource = model;
    }
    void InitGroupHeaders(MVCxGridViewState gridViewState) {
        GridViewDataColumnStateCollection groupedColumns = gridViewState.GroupedColumns;
        for (int i = groupedColumns.Count - 1; i >= 0; i--) {
            {
                GridViewDataColumnState groupedColumn = groupedColumns[i];
                GroupHeaderBand gb = new GroupHeaderBand();
                gb.Height = bandHeight;
                XRLabel l = new XRLabel();
                l.Text = groupedColumn.FieldName + ": [" + groupedColumn.FieldName + "]";
                l.LocationF = new PointF(initialGroupOffset + i * 10, 0);
                l.BackColor = Color.Beige;
                l.SizeF = new SizeF((report.PageWidth - (report.Margins.Left + report.Margins.Right)) - (initialGroupOffset + i * subGroupOffset), bandHeight);
                gb.Controls.Add(l);
                gb.RepeatEveryPage = shouldRepeatGroupHeadersOnEveryPage;
                GroupField gf = new GroupField(groupedColumn.FieldName, groupedColumn.SortOrder == ColumnSortOrder.Ascending ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending);
                gb.GroupFields.Add(gf);
                report.Bands.Add(gb);
            }
        }
    }
    void InitSortings(MVCxGridViewState gridViewState) {
        GridViewDataColumnStateCollection columns = gridViewState.Columns;
        GridViewDataColumnStateCollection groupedColumns = gridViewState.GroupedColumns;
        for (int i = 0; i < columns.Count; i++) {
            if (!groupedColumns.Contains(columns[i])) {
                if (columns[i].SortOrder != ColumnSortOrder.None)
                    ((DetailBand) report.Bands[BandKind.Detail]).SortFields.Add(new GroupField(columns[i].FieldName, columns[i].SortOrder == ColumnSortOrder.Ascending ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending));
            }
        }
    }

    void InitFilters(MVCxGridViewState gridViewState) {
        report.FilterString = gridViewState.FilterExpression;
    }

    void InitDetailsAndPageHeader(MVCxGridViewState gridViewState) {
        GridViewDataColumnStateCollection groupedColumns = gridViewState.GroupedColumns;

        int pagewidth = (report.PageWidth - (report.Margins.Left + report.Margins.Right)) - groupedColumns.Count * subGroupOffset;
        List<MVCxColumnInfo> columns = GetColumnsInfo(gridViewState, pagewidth);
        CustomizeColumnsCollection(report, new ColumnsCreationEventArgs(pagewidth) {
            ColumnsInfo = columns
        });

        report.Bands.Add(new DetailBand() {
            HeightF = bandHeight
        });
        report.Bands.Add(new PageHeaderBand() {
            HeightF = bandHeight
        });

        XRTable headerTable = new XRTable();
        XRTableRow row = new XRTableRow();
        XRTable detailTable = new XRTable();
        XRTableRow row2 = new XRTableRow();

        for (int i = 0; i < columns.Count; i++) {
            if (columns[i].IsVisible) {
                XRTableCell cell = new XRTableCell();
                cell.Width = columns[i].ColumnWidth;
                cell.Text = columns[i].FieldName;
                row.Cells.Add(cell);

                XRTableCell cell2 = new XRTableCell();
                cell2.Width = columns[i].ColumnWidth;
                ControlCustomizationEventArgs cc = new ControlCustomizationEventArgs() {
                    FieldName = columns[i].FieldName,
                    IsModified = false,
                    Owner = cell2
                };
                CustomizeColumn(report, cc);
                if (cc.IsModified == false)
                    cell2.DataBindings.Add("Text", null, columns[i].FieldName);
                detailsInfo.Add(columns[i].GridViewColumn, cell2);
                row2.Cells.Add(cell2);
            }
        }
        headerTable.Rows.Add(row);
        headerTable.Width = pagewidth;
        headerTable.LocationF = new PointF(groupedColumns.Count * subGroupOffset, 0);
        headerTable.Borders = BorderSide.Bottom;

        detailTable.Rows.Add(row2);
        detailTable.LocationF = new PointF(groupedColumns.Count * subGroupOffset, 0);
        detailTable.Width = pagewidth;

        report.Bands[BandKind.PageHeader].Controls.Add(headerTable);
        report.Bands[BandKind.Detail].Controls.Add(detailTable);
    }

    private List<MVCxColumnInfo> GetColumnsInfo(MVCxGridViewState gridViewState, int pagewidth) {
        List<MVCxColumnInfo> columns = new List<MVCxColumnInfo>();
        GridViewDataColumnStateCollection visibleColumns = gridViewState.Columns;
        foreach (GridViewDataColumnState dataColumn in visibleColumns) {
            MVCxColumnInfo column = new MVCxColumnInfo(dataColumn) {
                ColumnCaption = string.IsNullOrEmpty(dataColumn.Caption) ? dataColumn.FieldName : dataColumn.Caption,
                ColumnWidth = ((int) pagewidth / visibleColumns.Count),
                FieldName = dataColumn.FieldName,
                IsVisible = true
            };
            columns.Add(column);
        }
        return columns;

    }
    private SummaryFunc GetSummaryFunc(SummaryItemType summaryItemType) {
        switch (summaryItemType) {
            case SummaryItemType.Sum:
                return SummaryFunc.Sum;
            case SummaryItemType.Average:
                return SummaryFunc.Avg;
            case SummaryItemType.Max:
                return SummaryFunc.Max;
            case SummaryItemType.Min:
                return SummaryFunc.Min;
            default:
                return SummaryFunc.Custom;
        }
    }


    public void WritePdfToResponse(HttpResponseBase Response, string fileName, string type) {
        report.CreateDocument(false);
        using (MemoryStream ms = new MemoryStream()) {
            report.ExportToPdf(ms);
            ms.Seek(0, SeekOrigin.Begin);
            WriteResponse(Response, ms.ToArray(), type, fileName);
        }
    }
    public static void WriteResponse(HttpResponseBase response, byte[] filearray, string type, string fileName) {
        response.ClearContent();
        response.Buffer = true;
        response.Cache.SetCacheability(HttpCacheability.Private);
        response.ContentType = "application/pdf";
        ContentDisposition contentDisposition = new ContentDisposition();
        contentDisposition.FileName = fileName;
        contentDisposition.DispositionType = type;
        response.AddHeader("Content-Disposition", contentDisposition.ToString());
        response.BinaryWrite(filearray);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
        try {
            response.End();
        }
        catch (System.Threading.ThreadAbortException) {
        }

    }
}

public class ControlCustomizationEventArgs : EventArgs {
    XRControl owner;

    public XRControl Owner {
        get {
            return owner;
        }
        set {
            owner = value;
        }
    }
    bool isModified;

    public bool IsModified {
        get {
            return isModified;
        }
        set {
            isModified = value;
        }
    }
    string fieldName;

    public string FieldName {
        get {
            return fieldName;
        }
        set {
            fieldName = value;
        }
    }

}

public class ColumnsCreationEventArgs : EventArgs {
    int pageWidth;
    public int PageWidth {
        get {
            return pageWidth;
        }
    }
    public ColumnsCreationEventArgs(int pageWidth) {
        this.pageWidth = pageWidth;
    }
    List<MVCxColumnInfo> columnsInfo;

    public List<MVCxColumnInfo> ColumnsInfo {
        get {
            return columnsInfo;
        }
        set {
            columnsInfo = value;
        }
    }
}

public class MVCxColumnInfo {
    public MVCxColumnInfo(GridViewDataColumnState gridViewColumn) {
        this.gridViewColumn = gridViewColumn;
    }
    GridViewDataColumnState gridViewColumn;

    public GridViewDataColumnState GridViewColumn {
        get {
            return gridViewColumn;
        }
    }


    string columnCaption;
    public string ColumnCaption {
        get {
            return columnCaption;
        }
        set {
            columnCaption = value;
        }
    }
    string fieldName;

    public string FieldName {
        get {
            return fieldName;
        }
        set {
            fieldName = value;
        }
    }
    int columnWidth;

    public int ColumnWidth {
        get {
            return columnWidth;
        }
        set {
            columnWidth = value;
        }
    }
    bool isVisible;

    public bool IsVisible {
        get {
            return isVisible;
        }
        set {
            isVisible = value;
        }
    }


}
