CxList methods = Find_MethodDecls();
CxList stmtCollect = Find_StatementCollection().FindByFathers(methods);
CxList retStmt = All.NewCxList();
foreach(CxList method in stmtCollect)
{
	StatementCollection col = method.TryGetCSharpGraph<StatementCollection>();	
	if(col.Count <= 1)
	{
		CxList methodFather = method.GetFathers();
		CSharpGraph methodFatherGraph = methodFather.TryGetCSharpGraph<CSharpGraph>();
		result.Add(methodFatherGraph.NodeId, methodFatherGraph);
		foreach(Statement s in col)
		{
			retStmt.Add(s.NodeId, s);
		}
	}
}
result -= retStmt.GetAncOfType(typeof(MethodDecl));
result -= retStmt.GetAncOfType(typeof(LambdaExpr));