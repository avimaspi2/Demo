CxList methods = Find_MethodDeclaration();
CxList stmtCollect = Find_StatementCollection().FindByFathers(methods);
CxList retStmt = All.NewCxList();
foreach(CxList method in stmtCollect)
{
	try
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
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
result -= retStmt.GetAncOfType(typeof(MethodDecl));
result -= result.FindByShortName("Checkmarx_class*", false);