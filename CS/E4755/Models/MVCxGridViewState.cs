using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using DevExpress.Data;

public class MVCxGridViewState {
    public MVCxGridViewState(MVCxGridView gridView) {
        Columns = new GridViewDataColumnStateCollection(gridView.VisibleColumns);
        GroupedColumns = new GridViewDataColumnStateCollection(gridView.GetGroupedColumns());
        TotalSummary = new System.Collections.ObjectModel.Collection<MVCxSummaryItemState>();
        foreach (ASPxSummaryItem item in gridView.TotalSummary) {
            TotalSummary.Add(new MVCxSummaryItemState(item));
        }
        FilterExpression = gridView.FilterExpression;
    }

    public GridViewDataColumnStateCollection Columns {
        get;
        protected set;
    }

    public GridViewDataColumnStateCollection GroupedColumns {
        get;
        protected set;
    }

    public System.Collections.ObjectModel.Collection<MVCxSummaryItemState> TotalSummary {
        get;
        protected set;
    }

    public string FilterExpression {
        get;
        protected set;
    }
}

public class GridViewDataColumnState {
    public GridViewDataColumnState(GridViewDataColumn column) {
        Caption = column.Caption;
        FieldName = column.FieldName;
        SortOrder = column.SortOrder;
    }

    public string Caption {
        get;
        protected set;
    }

    public string FieldName {
        get;
        protected set;
    }

    public ColumnSortOrder SortOrder {
        get;
        protected set;
    }
}

public class GridViewDataColumnStateCollection : System.Collections.ObjectModel.Collection<GridViewDataColumnState> {
    public GridViewDataColumnStateCollection(ICollection<GridViewColumn> columns) {
        foreach (GridViewColumn col in columns) {
            GridViewDataColumn dataCol = col as GridViewDataColumn;
            if (dataCol != null) {
                this.Add(new GridViewDataColumnState(dataCol));
            }
        }
    }

    public GridViewDataColumnStateCollection(ICollection<GridViewDataColumn> columns) {
        foreach (GridViewDataColumn col in columns) {
            this.Add(new GridViewDataColumnState(col));
        }
    }

    public GridViewDataColumnState this[string fieldName] {
        get {
            return this.Single((item) => item.FieldName == fieldName);
        }
    }
}

public class MVCxSummaryItemState {
    public MVCxSummaryItemState(ASPxSummaryItem item) {
        FieldName = item.FieldName;
        ShowInColumn = item.ShowInColumn;
        DisplayFormat = item.DisplayFormat;
        SummaryType = item.SummaryType;
    }

    public string FieldName {
        get;
        protected set;
    }
    public string ShowInColumn {
        get;
        protected set;
    }
    public string DisplayFormat {
        get;
        protected set;
    }
    public SummaryItemType SummaryType {
        get;
        protected set;
    }
}