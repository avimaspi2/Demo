// Database Filter

CxList methods = Find_Methods();
CxList DataTable_Select = 
	// DataTable.Select
	// DataView.Table.Select
	methods.FindByMemberAccess("Table.Select") +
	methods.FindByMemberAccess("Table.Compute")+

	// DataView.RowFilter Property 
	All.FindByMemberAccess("DataView.RowFilter");

	// DataView Constructor: e.g. new DataView(table, rowFilter, ..)
CxList dataView = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("DataView");
CxList rowFilter = All.GetParameters(dataView, 1);
	
result = DataTable_Select + rowFilter;