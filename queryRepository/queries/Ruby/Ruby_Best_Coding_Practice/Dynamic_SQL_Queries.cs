//////////////////////////////////////////////////////////////////////////////////
// Query: Dynamic_SQL_Queries 
// Purpose: Find SQL queries that are built of concatenation of strings
//

CxList db = Find_SQL_DB_In();

// Find all binary expressions of type "Add"
CxList binary = All.FindByType(typeof(BinaryExpr));
CxList binaryAdd = All.NewCxList();
foreach(CxList s in binary)
{
	try
	{
		BinaryExpr graph = s.data.GetByIndex(0) as BinaryExpr;
		if(graph != null && graph.Operator.ToString().Equals("Add"))
		{
			binaryAdd.Add(s);
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList concat = binaryAdd;

CxList hashSanitizers =All.FindByAssignmentSide(CxList.AssignmentSide.Left).DataInfluencedBy(All.FindByShortName("CxHash"));

CxList dbSanitizedIn = All.FindByType(typeof(AssignExpr)).GetParameters(db);
dbSanitizedIn = dbSanitizedIn.GetAncOfType(typeof(MethodInvokeExpr)) 
	+ dbSanitizedIn.GetAncOfType(typeof(ObjectCreateExpr));

CxList sanitize = Find_SQL_Sanitize() + hashSanitizers + dbSanitizedIn;

result = concat.InfluencingOnAndNotSanitized(db, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);