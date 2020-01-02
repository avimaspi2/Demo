CxList methods = All.FindByType(typeof(MethodDecl));
methods -= methods.FindByShortName("Method_*");
methods -= methods.FindByRegex("="); // remove assign expressions
methods -= methods.FindByRegex("="); // remove second assign expression

CxList stmtCollect = All.FindByType(typeof(StatementCollection)).FindByFathers(methods);
CxList retStmt = All.NewCxList();
foreach(CxList method in stmtCollect)
{
	StatementCollection col = method.data.GetByIndex(0) as StatementCollection;	
	if(col.Count <= 1)
	{
		result.Add(method.GetFathers());
		foreach(Statement s in col)
		{
			retStmt.Add(All.FindById(s.NodeId));
		}
	}
}
result = result - retStmt.GetAncOfType(typeof(MethodDecl));