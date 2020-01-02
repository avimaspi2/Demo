// Database Filter

CxList methods = Find_Methods();

CxList DataTable_Select = methods.FindByMemberAccess("Table.Select"); // DataTable.Select
DataTable_Select.Add(methods.FindByMemberAccess("Table.Compute"));    // DataView.Table.Select
DataTable_Select.Add(All.FindByMemberAccess("DataView.RowFilter"));   // DataView.RowFilter Property

// DataView Constructor: e.g. new DataView(table, rowFilter, ..)
CxList dataView = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("DataView");
CxList rowFilter = All.GetParameters(dataView, 1);

result.Add(DataTable_Select);
result.Add(rowFilter);