CxList methods = Find_MethodDecls();
CxList stmtCollect = Find_StatementCollection().FindByFathers(methods);
CxList retStmt = All.NewCxList();
foreach(CxList method in stmtCollect)
{
	StatementCollection col = method.TryGetCSharpGraph<StatementCollection>();	
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