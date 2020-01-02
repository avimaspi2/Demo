CxList methods = All.FindByType(typeof(MethodDecl));
CxList stmtCollect = All.FindByType(typeof(StatementCollection)).FindByFathers(methods);
CxList retStmt = All.NewCxList();
foreach(CxList method in stmtCollect)
{
	StatementCollection col = method.data.GetByIndex(0) as StatementCollection;	
	if(col.Count <= 1)
	{
		CxList methodFather = method.GetFathers();
		CSharpGraph methodFatherGraph = methodFather.data.GetByIndex(0) as CSharpGraph;
		result.Add(methodFatherGraph.NodeId, methodFatherGraph);
		foreach(Statement s in col)
		{
			retStmt.Add(s.NodeId, s);
		}
	}
}
result = result - retStmt.GetAncOfType(typeof(MethodDecl)) /*.FindByType(typeof(ReturnStmt)).GetAncOfType(typeof(MethodDecl))*/;