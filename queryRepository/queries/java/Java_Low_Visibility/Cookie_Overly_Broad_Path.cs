///////////////////////////////////////////////////////////////////////////////
// Query  : Cookie_Overly_Broad_Path
// Purpose: Find cookies at root context "/"

// Find "/" string literal
CxList rootPath = Find_Strings().FindByShortName("\"/\"");

CxList methods = Find_Methods();
CxList concat = methods.FindByShortName("append");

CxList setPath = methods.FindByMemberAccess("Cookie.setPath");

// Find the "+" operators
CxList binary = Find_BinaryExpr();
// Reduce the binary list for the sake of performance
binary = binary.InfluencingOn(setPath);
foreach(CxList l in binary)
{
	try
	{
		BinaryExpr b = l.TryGetCSharpGraph<BinaryExpr>();
		if(b != null && b.Operator.ToString() == "Add")
		{
			concat.Add(l);
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

// Find the '+=' operators
CxList assignments = Find_AssignExpr();
// Reduce the assignments list for the sake of performance
assignments = assignments.InfluencingOn(setPath);
foreach(CxList assignment in assignments)
{
	try
	{
		AssignExpr graph = assignment.TryGetCSharpGraph<AssignExpr>();
		if(graph != null && graph.Operator == AssignOperator.AdditionAssign)
			concat.Add(assignment);	
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

// Find places where the concat is in the setPath call (e.g. cookie.setPath(str1 + "/");)
CxList concatInSetPath = concat.GetByAncs(setPath);

// Find first parameter of "setPath"
CxList pathParam = All.GetParameters(setPath, 0);

// Find only setPath invokation that are influenced by concat
CxList concatSetPath = pathParam.InfluencedBy(concat).GetAncOfType(typeof(MethodInvokeExpr));
concatSetPath.Add(concatInSetPath.GetAncOfType(typeof(MethodInvokeExpr)));

// setPath will now include only paths that are no concatenated
setPath -= concatSetPath;

result = setPath.InfluencedBy(rootPath);