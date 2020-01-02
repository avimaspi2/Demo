// Database Access

CxList methods = Find_Methods();
CxList DataTable_Select = methods.FindByMemberAccess("Table.Select", false); // Table.Select 
DataTable_Select.Add(methods.FindByMemberAccess("Table.Compute", false));

// DataView.RowFilter Property 
DataTable_Select.Add(All.FindByMemberAccess("DataView.RowFilter", false));

// new DataView(table, rowFilter, ..)
CxList dataView = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("DataView", false);
CxList rowFilter = All.GetParameters(dataView, 1);
	
result = DataTable_Select;
result.Add(rowFilter);